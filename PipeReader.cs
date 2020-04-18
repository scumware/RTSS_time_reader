using System;
using System.ComponentModel;
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
        private volatile bool m_stop;
        private volatile IntPtr m_taskThreadHandle;

        private Exception m_lastException;
        private volatile Task m_task;

        private string m_StartFileName;
        private int m_fileNumber;
        private object m_fileStreamLocker = new object();
        private object m_pipeStreamLocker = new object();


        private volatile PipeReaderState m_state;
        private string m_pipeName;
        private bool m_writeFrapsFileFormat;

        private readonly object m_threadHandleLocker = new object();

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
                m_state = value;
                OnStateChanged();
            }
        }

        public event EventHandler StateChanged;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadFlag(PipeReaderState p_flag)
        {
            return (m_state & p_flag) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetFlag(PipeReaderState p_flag, bool p_value)
        {
            if (p_value)
                State = m_state | p_flag;
            else
                State = m_state & ~p_flag;
        }

        public bool IsStarted
        {
            get { return ReadFlag(PipeReaderState.Started); }
        }

        public bool IsPipeCreated
        {
            get { return ReadFlag(PipeReaderState.PipeCreated); }
        }

        public bool IsFileOpened
        {
            get { return ReadFlag(PipeReaderState.FileOpened); }
        }

        public bool IsConnectionAccepted
        {
            get { return ReadFlag(PipeReaderState.ConnectionAccepted); }
        }

        public bool IsError
        {
            get { return ReadFlag(PipeReaderState.Error); }
        }

        public uint ClientProcessId { get; protected set; }

        public void Stop ()
        {
            m_stop = true;
            ClosePipe();
        }

        public Exception LastException
        {
            get { return m_lastException; }
        }

        public string StartFileName
        {
            get { return m_StartFileName; }
            set
            {
                if (IsFileOpened)
                    throw new InvalidOperationException("invalid object state");
                m_StartFileName = value;
            }
       }

        public string PipeName
        {
            get { return m_pipeName; }
            set
            {
                if (IsPipeCreated)
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
                if (IsFileOpened)
                    throw new InvalidOperationException("invalid object state");

                m_writeFrapsFileFormat = value;
            }
        }

        public bool EnabledWritingFile
        {
            get { return ReadFlag(PipeReaderState.EnabledWritingFile); }
            set { SetFlag(PipeReaderState.EnabledWritingFile, value); }
        }

        public void StartAcceptingConnections()
        {
            SetFlag(PipeReaderState.Started, true);
            ClientProcessId = Win32A.INVALID_HANDLE_VALUE;

            var previousTask = m_task;
            m_stop = false;

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

                        if (false == CreatePipe())
                            return;

                        if (false == OpenFile())
                        {
                            ClosePipe();
                            return;
                        }

                        SetFlag(PipeReaderState.FileOpened, true);

                        WritingFileLoop();
                    }
                    finally
                    {
                        lock (m_threadHandleLocker)
                        {
                            if (m_taskThreadHandle != Win32A.INVALID_HANDLE_PTR)
                            {
                                Win32A.CloseHandle(m_taskThreadHandle);
                                m_taskThreadHandle = Win32A.INVALID_HANDLE_PTR;
                            }
                        }

                        m_task = null;
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
                var name = Path.GetFileNameWithoutExtension(m_StartFileName);
                var extension = Path.GetExtension(m_StartFileName);
                var newFileName = name + "." + m_fileNumber.ToString("D2") + "." + extension;
                lock (m_fileStreamLocker)
                {
                    m_fileStream = File.Open(newFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                    ++m_fileNumber;
                }
            }
            catch (Exception exception)
            {
                m_lastException = exception;
                SetFlag(PipeReaderState.Error, true);
                return false;
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

                SetFlag(PipeReaderState.PipeCreated, true);


                var safePipeHandle = new SafePipeHandle(handle, true);

                m_pipeStream = new NamedPipeServerStream(PipeDirection.InOut, false, false, safePipeHandle);

                m_lastException = null;

                SetFlag(PipeReaderState.PipeIO, true);
                try
                {
                    m_pipeStream.WaitForConnection();
                }
                finally
                {
                    SetFlag(PipeReaderState.PipeIO, false);
                }

                result = true;
                SetFlag(PipeReaderState.ConnectionAccepted, true);


                uint processId;
                if (Win32A.GetNamedPipeClientProcessId(handle, out processId))
                {
                    ClientProcessId = processId;
                }
            }
            catch (TimeoutException exception)
            {
                m_lastException = exception;
                SetFlag(PipeReaderState.Error, true);
            }
            catch (System.OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                m_lastException = exception;
                ClosePipe();
                SetFlag(PipeReaderState.Error, true);
            }

            return result;
        }


        private void WritingFileLoop()
        {
            var numberFormat = (NumberFormatInfo) CultureInfo.CurrentCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = ".";
            numberFormat.NumberDecimalDigits = 3;

            WriteFileHeader();

            const int sizeValueString = 11;
            uint frameNumber = 1;
            uint totalTime = 0;
            var firstPass = true;
            var totalTimeSb = new StringBuilder(sizeValueString);

            int bufferSize;
            unsafe
            {
                bufferSize = sizeof(FRAMETIME_PIPE_DATA);
            }
            var buffer = new byte[bufferSize];

            try
            {
                try
                {
                    unsafe
                    {
                        fixed (void* rowData = buffer)
                            while (false == m_stop)
                            {
                                int readedCount;
                                lock (m_pipeStreamLocker)
                                {
                                    SetFlag(PipeReaderState.PipeIO, true);
                                    readedCount = m_pipeStream.Read(buffer, 0, bufferSize);
                                    SetFlag(PipeReaderState.PipeIO, false);
                                }

                                if (readedCount == 0)
                                    break;

                                if (false == EnabledWritingFile)
                                    continue;

                                ++frameNumber;

                                var ppipeData = (FRAMETIME_PIPE_DATA*) rowData;
                                var delta = ppipeData->dwFrametime;


                                string strValue;

                                if (false == m_writeFrapsFileFormat)
                                {
                                    strValue = string.Empty;

                                    if (false == firstPass)
                                    {
                                        strValue = ", ";
                                    }
                                    else
                                    {
                                        firstPass = false;
                                    }

                                    strValue += ((float) delta / 1000).ToString(numberFormat);
                                }
                                else
                                {
                                    totalTime += delta;

                                    strValue = frameNumber.ToString("D5").ReplaceInplaceLeadingChars('0', ' ');
                                    strValue += ",";

                                    var totalTimeString = ((float) totalTime / 1000.0).ToString(numberFormat);
                                    if (totalTimeString.Length < sizeValueString)
                                    {
                                        totalTimeString = totalTimeSb
                                            .Append(' ', sizeValueString - totalTimeString.Length)
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
                                    m_fileStream.Write(stringBytes, 0, stringBytes.Length);
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
                    SetFlag(PipeReaderState.Error, true);
                }
            }
            finally
            {
                SetFlag(PipeReaderState.PipeIO, false);
                CloseFile();
                ClosePipe();
            }
        }

        private void WriteFileHeader()
        {
            if (m_writeFrapsFileFormat)
            {
                var headerLine = "Frame, Time(ms)" + Environment.NewLine + "    1,     0.000" + Environment.NewLine;

                var stringBytes = Encoding.ASCII.GetBytes(headerLine);
                m_fileStream.Write(stringBytes, 0, stringBytes.Length);
            }
        }

        protected void ClosePipe()
        {
            if (m_pipeStream != null)
            {
                lock (m_threadHandleLocker)
                {
                    if (ReadFlag(PipeReaderState.PipeIO) && m_taskThreadHandle != Win32A.INVALID_HANDLE_PTR)
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
            }

            SetFlag(PipeReaderState.ConnectionAccepted, false);
            SetFlag(PipeReaderState.PipeCreated, false);
        }

        protected void CloseFile()
        {
            lock (m_fileStreamLocker)
            {
                if (m_fileStream != null)
                {
                    m_fileStream.Flush(true);
                    m_fileStream.Close();
                    m_fileStream = null;
                }
            }

            SetFlag(PipeReaderState.FileOpened, false);
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
            m_stop = true;
            m_pipeStream?.Dispose();
            lock (m_fileStreamLocker)
            {
                m_fileStream?.Dispose();
            }
        }

        public void FlushFileBuffer()
        {
            if (m_fileStream != null)
                m_fileStream.Flush(true);
        }

        public void StopWritingFile()
        {
            lock (m_fileStreamLocker)
            {
                EnabledWritingFile = false;
                CloseFile();
            }
        }
    }
}