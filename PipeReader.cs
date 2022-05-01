using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace RTSS_time_reader
{
    public class PipeReader :IDisposable
    {
        private volatile NamedPipeServerStream m_pipeStream;
        private volatile FileStream m_fileStream;
        private volatile bool m_stopReadWriteLoops;
        private volatile IntPtr m_taskThreadHandle;

        private Exception m_lastException;
        private volatile Task m_task;

        private string m_startFileName;
        private int m_fileNumber;
        private readonly object m_fileStreamLocker = new object();
        private readonly object m_pipeStreamLocker = new object();


        private PipeReaderState m_state;
        private string m_pipeName;
        private bool m_writeFrapsFileFormat;

        private readonly object m_threadHandleLocker = new object();
        private volatile bool m_continueAcceptingConnections;
        private Process m_connectedProcess;
        public string ProcessName { get; private set; }

        public PipeReader()
        {
        }

        public PipeReaderState State
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return m_state; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected set
            {
                var oldState = m_state;
                m_state = value;
                if (m_state.GetDifference(oldState) != PipeReaderStateEnum.PipeIO)
                {
                    OnStateChanged();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetFlag(PipeReaderStateEnum p_flag, bool p_value)
        {
            var oldState = m_state;

            var newState = oldState;
            newState.SetFlag(p_flag, p_value);

            State = newState;
        }



        public event EventHandler StateChanged;

        public uint ClientProcessId { get; protected set; }

        public void Stop ()
        {
            m_stopReadWriteLoops = true;
            ClosePipe();
        }

        public Exception LastException
        {
            get { return m_lastException; }
        }

        public string StartFileName
        {
            get { return m_startFileName; }
            set
            {
                if (m_state.IsFileOpened)
                    throw new InvalidOperationException("invalid object state");
                m_startFileName = value;
            }
       }

        public string PipeName
        {
            get { return m_pipeName; }
            set
            {
                if (m_state.IsPipeCreated)
                    throw new InvalidOperationException("invalid object state");

                m_pipeName = value;
            }
        }

        public string PipeFullName
        {
            get { return Path.GetFullPath("\\\\.\\pipe\\" + m_pipeName); }
        }

        public bool WriteFrapsFileFormat
        {
            get { return m_writeFrapsFileFormat; }
            set
            {
                if (m_state.IsFileOpened)
                    throw new InvalidOperationException("invalid object state");

                m_writeFrapsFileFormat = value;
            }
        }

        public bool EnabledWritingFile
        {
            get { return m_state.EnabledWritingFile; }
            set
            {
                lock (m_fileStreamLocker)
                {
                    SetFlag(PipeReaderStateEnum.EnabledWritingFile, value);

                    if ((false == value) && m_state.IsFileOpened)
                        CloseFile();
                }
            }
        }

        public string OpenedFileName { get; protected set; }
        public string TargetFolder { get; set; }

        public void StartAcceptingConnections()
        {
            SetFlag(PipeReaderStateEnum.Starting, true);
            ClientProcessId = Win32A.INVALID_HANDLE_VALUE;

            var previousTask = m_task;
            m_stopReadWriteLoops = false;

            m_task = Task.Factory.StartNew(
                () =>
                {
                    lock (m_threadHandleLocker)
                    {
                        var threadId = Win32A.GetCurrentThreadId();
                        m_taskThreadHandle = Win32A.OpenThread(0x40000000, false, threadId);
                    }

                    try
                    {
                        if (previousTask != null)
                            try
                            {
                                previousTask.Wait();
                            }
                            catch (Exception)
                            {
                                ; //do nothing
                            }

                        SetFlag(PipeReaderStateEnum.Started, true);

                        if (false == CreatePipe())
                            return;

                        if (false == OpenFile())
                        {
                            ClosePipe();
                            return;
                        }

                        MainLoop();
                    }
                    finally
                    {
                        lock (m_threadHandleLocker)
                        {
                            if (m_taskThreadHandle != Win32A.INVALID_HANDLE_PTR)
                            {
                                Win32A.CloseHandle(m_taskThreadHandle);
                            }
                            m_taskThreadHandle = Win32A.INVALID_HANDLE_PTR;
                            m_task = null;
                        }

                        State = PipeReaderState.None;
                    }
                }
                , CancellationToken.None
                , TaskCreationOptions.LongRunning
                , TaskScheduler.Default
            );
        }


        private bool OpenFile()
        {
            try
            {
                SetFlag(PipeReaderStateEnum.FileOpening, true);

                var name = Path.GetFileNameWithoutExtension(m_startFileName) + "_" + ProcessName;
                var extension = Path.GetExtension(m_startFileName);
                var newFileName = name + "." + m_fileNumber.ToString("D2") + extension;
                var fullPath = Path.Combine(this.TargetFolder, newFileName);

                lock (m_fileStreamLocker)
                {
                    m_fileStream = File.Open(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                    SetFlag(PipeReaderStateEnum.FileOpened, true);
                    ++m_fileNumber;
                    OpenedFileName = newFileName;
                    WriteFileHeader();
                }
            }
            catch (Exception exception)
            {
                m_lastException = exception;
                SetFlag(PipeReaderStateEnum.Error, true);
                return false;
            }
            finally
            {
                SetFlag(PipeReaderStateEnum.FileOpening, false);
            }
            return true;
        }

        private bool CreatePipe()
        {
            var result = false;

            try
            {
                string fullPath = PipeFullName;

                var handle = Win32A.CreateNamedPipe(fullPath
                    , (uint) Win32A.PipeOpenModeFlags.PIPE_ACCESS_DUPLEX
                    ,
                    (uint)
                    (Win32A.PipeModeFlags.PIPE_TYPE_MESSAGE | Win32A.PipeModeFlags.PIPE_READMODE_MESSAGE |
                     Win32A.PipeModeFlags.PIPE_WAIT)
                    , 1
                    , 1024, 1024
                    , 0, IntPtr.Zero);

                if (handle.ToInt32() == -1)
                {
                    var win32Error = Marshal.GetLastWin32Error();
                    if (win32Error != Win32A.ERROR_SUCCESS)
                    {
                        throw new Win32Exception(win32Error);
                    }
                }

                var safePipeHandle = new SafePipeHandle(handle, true);
                m_pipeStream = new NamedPipeServerStream(PipeDirection.InOut, false, false, safePipeHandle);
                SetFlag(PipeReaderStateEnum.PipeCreated, true);

                m_lastException = null;

                result = true;
                WaitForConnection();
            }
            catch (TimeoutException exception)
            {
                m_lastException = exception;
                SetFlag(PipeReaderStateEnum.Error, true);
            }
            catch (System.OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                m_lastException = exception;
                ClosePipe();
                CloseConnectedProcess();
                SetFlag(PipeReaderStateEnum.Error, true);
            }

            return result;
        }

        private void WaitForConnection()
        {
            SetFlag(PipeReaderStateEnum.PipeIO, true);
            try
            {
                m_pipeStream.WaitForConnection();

                uint processId;
                if (Win32A.GetNamedPipeClientProcessId(m_pipeStream.SafePipeHandle.DangerousGetHandle(), out processId))
                {
                    ClientProcessId = processId;

                    m_connectedProcess = Process.GetProcessById((int) ClientProcessId);
                    ProcessName = m_connectedProcess.ProcessName;
                }
            }
            finally
            {
                SetFlag(PipeReaderStateEnum.PipeIO, false);
            }
            SetFlag(PipeReaderStateEnum.ConnectionAccepted, true);
        }


        private void MainLoop()
        {
            var numberFormat = (NumberFormatInfo) CultureInfo.CurrentCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = ".";
            numberFormat.NumberDecimalDigits = 3;

            const int valueStringLenght = 11;
            uint frameNumber = 0;
            uint totalTime = 0;
            var firstPass = true;
            var totalTimeSb = new StringBuilder(valueStringLenght);

            int bufferSize;
            unsafe
            {
                bufferSize = sizeof(FRAMETIME_PIPE_DATA);
            }

            var buffer = new byte[bufferSize];
            do //while (ContinueAcceptingConnections);
            {
                try
                {
                    try
                    {
                        unsafe
                        {
                            fixed (void* rowData = buffer)
                                while (false == m_stopReadWriteLoops)
                                {
                                    int readedCount;
                                    lock (m_pipeStreamLocker)
                                    {
                                        SetFlag(PipeReaderStateEnum.PipeIO, true);
                                        try
                                        {
                                            readedCount = m_pipeStream.Read(buffer, 0, bufferSize);
                                        }
                                        finally
                                        {
                                            SetFlag(PipeReaderStateEnum.PipeIO, false);
                                        }
                                    }

                                    if (readedCount == 0)
                                        break;

                                    if (m_stopReadWriteLoops)
                                        break;

                                    if (false == EnabledWritingFile)
                                        continue;

                                    ++frameNumber;

                                    var ppipeData = (FRAMETIME_PIPE_DATA*) rowData;
                                    var delta = ppipeData->dwFrametime;


                                    string strValue;

                                    if (false == m_writeFrapsFileFormat)
                                    {
                                        var valuesLineStringBuilder = new StringBuilder();

                                        if (false == firstPass)
                                        {
                                            valuesLineStringBuilder.AppendLine();
                                        }
                                        else
                                        {
                                            firstPass = false;
                                        }

                                        valuesLineStringBuilder
                                            .Append(((float) delta / 1000).ToString(numberFormat))
                                            .Append(",\t")
                                            .Append(m_connectedProcess.PrivilegedProcessorTime.ToString("g"))
                                            .Append(",\t")
                                            .Append(m_connectedProcess.UserProcessorTime.ToString("g"));

                                        strValue = valuesLineStringBuilder.ToString();
                                    }
                                    else
                                    {
                                        totalTime += delta;

                                        strValue = frameNumber.ToString("D5").ReplaceInplaceLeadingChars('0', ' ');
                                        strValue += ",";

                                        var totalTimeString = ((float) totalTime / 1000.0).ToString(numberFormat);
                                        if (totalTimeString.Length < valueStringLenght)
                                        {
                                            var whitespacesCount = valueStringLenght - totalTimeString.Length;

                                            totalTimeString = totalTimeSb
                                                .Append(' ', whitespacesCount)
                                                .Append(totalTimeString)
                                                .Append(Environment.NewLine)
                                                .ToString();
                                            totalTimeSb.Clear();
                                        }

                                        strValue += totalTimeString;
                                    }


                                    var stringBytes = Encoding.ASCII.GetBytes(strValue);
                                    lock (m_fileStreamLocker)
                                    {
                                        if (m_fileStream != null)
                                            m_fileStream.Write(stringBytes, 0, stringBytes.Length);
                                        else
                                        {
                                            OpenFile();

                                            frameNumber = 0;
                                            totalTime = 0;
                                            firstPass = true;
                                        }
                                    }
                                }
                        }
                    }
                    catch (System.OperationCanceledException)
                    {

                    }
                    catch (Exception exception)
                    {
                        m_lastException = exception;
                        SetFlag(PipeReaderStateEnum.Error, true);
                    }
                }
                finally
                {
                    SetFlag(PipeReaderStateEnum.PipeIO, false);
                    CloseFile();
                    CloseConnectedProcess();

                    lock (m_pipeStream)
                    {
                        if (m_pipeStream != null)
                            m_pipeStream.Disconnect();
                    }
                    SetFlag(PipeReaderStateEnum.ConnectionAccepted,false);

                    if (false == ContinueAcceptingConnections)
                    {
                        ClosePipe();
                    }
                }


                if (false == ContinueAcceptingConnections)
                    return;

                lock (m_pipeStreamLocker)
                {
                    try
                    {
                        WaitForConnection();
                        OpenFile();
                    }
                    catch (OperationCanceledException exception)
                    {
                        return;
                    }
                    catch (Exception exception)
                    {
                        m_lastException = exception;
                        SetFlag(PipeReaderStateEnum.Error, true);
                    }

                    frameNumber = 0;
                    totalTime = 0;
                    firstPass = true;

                    m_stopReadWriteLoops = false;
                }
            } while (ContinueAcceptingConnections);
        }

        public bool ContinueAcceptingConnections
        {
            get { return m_continueAcceptingConnections; }
            set { m_continueAcceptingConnections = value; }
        }

        private void WriteFileHeader()
        {
            string headerLine;
            if (m_writeFrapsFileFormat)
            {
                headerLine = "Frame, Time(ms)"+Environment.NewLine;
            }
            else
            {
                headerLine = "FrameTime(ms), PrivilegedProcessorTime, UserProcessorTime"  + Environment.NewLine;
            }

            var stringBytes = Encoding.ASCII.GetBytes(headerLine);
            m_fileStream.Write(stringBytes, 0, stringBytes.Length);
        }

        protected void ClosePipe()
        {
            ContinueAcceptingConnections = false;
            
            lock (m_threadHandleLocker)
            {
                if (m_state.ReadFlag(PipeReaderStateEnum.PipeIO) && m_taskThreadHandle != Win32A.INVALID_HANDLE_PTR)
                    Win32A.CancelSynchronousIo(m_taskThreadHandle);
            }

            lock (m_pipeStreamLocker)
            {
                if (m_pipeStream != null)
                {
                    m_pipeStream.Dispose();
                    m_pipeStream = null;
                }
            }

            SetFlag(PipeReaderStateEnum.ConnectionAccepted, false);
            SetFlag(PipeReaderStateEnum.PipeCreated, false);
        }

        protected void CloseFile()
        {
            lock (m_fileStreamLocker)
            {
                if (m_fileStream != null)
                {
                    m_fileStream.Close();
                    m_fileStream = null;
                }
            }

            SetFlag(PipeReaderStateEnum.FileOpened, false);
        }

        protected void CloseConnectedProcess()
        {
            if (m_connectedProcess != null)
            {
                m_connectedProcess.Dispose();
                m_connectedProcess = null;
                ProcessName = string.Empty;
            }
        }

        protected void OnStateChanged()
        {
            var stateChanged = StateChanged;
            if (stateChanged != null)
            {
                stateChanged(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            m_stopReadWriteLoops = true;
            m_pipeStream?.Dispose();
            lock (m_fileStreamLocker)
            {
                m_fileStream?.Dispose();
            }
        }

        public void FlushFileBuffer()
        {
            lock (m_fileStreamLocker)
            {
                if (m_fileStream != null)
                    m_fileStream.Flush(true);
            }
        }

        public void StopWritingFile()
        {
            lock (m_fileStreamLocker)
            {
                EnabledWritingFile = false;
                CloseFile();
            }
        }

        public void DropConnection()
        {
            if (false == m_state.IsConnectionAccepted)
                throw new InvalidOperationException("Invalid object state");

            m_stopReadWriteLoops = true;
            lock (m_threadHandleLocker)
            {
                if (m_state.ReadFlag(PipeReaderStateEnum.PipeIO) && m_taskThreadHandle != Win32A.INVALID_HANDLE_PTR)
                    Win32A.CancelSynchronousIo(m_taskThreadHandle);
            }
        }
    }
}