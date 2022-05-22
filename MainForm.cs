using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace RTSS_time_reader
{
    using RTSS_time_reader.RTSS_interop;
    using RTSS_time_reader.WindowsInterop;
    using Timer = System.Timers.Timer;

    public partial class MainForm : Form
    {
        private readonly PipeReader m_pipeReader;
        private readonly Timer m_stopWritingTimer;
        private readonly Timer m_flushFileTimer;


        private PipeReaderState m_previousPipeReaderState;
        private bool m_lblWritingFileVisible;
        private Hotkey? m_registredHotkey;
        private OSD m_OSD;
        private OSDSlot m_osdSlotTop;
        private OSDSlot m_osdSlotFile;
        private OSDSlot m_osdSlotBottomCPU;


        private bool m_previousStateIsError = false;
        private TimeSpan m_remainedTimeSpan;
        private TimeSpan m_flushTimerPeriod;

        private double m_previousOwnCPUTotal = 0.0;
        private double m_previousConnectedCPUTotal = 0.0;


        private ushort? m_globalHotkeyAtom;

        public Hotkey? RegistredHotkey
        {
            get { return m_registredHotkey; }
            private set
            {
                m_registredHotkey = value;
                if (value.HasValue)
                    txtHotkeyEditor.Text = value.Value.Modifiers.GetDescription() + "+" + value.Value.Key;
            }
        }

        public bool WriteFrapsFileFormat
        {
            get;
            private set;
        }

        public string TargetFolder 
        {
            get;
            private set;
        }

        public string StartFileName
        {
            get;
            private set;
        }

        public MainForm()
        {
            InitializeComponent();

            var newAtom = Win32A.GlobalAddAtom("RTSS_time_reader");
            if (newAtom != 0)
            {
                m_globalHotkeyAtom = newAtom;
            }

            m_OSD = new OSD("RTSS_time_reader");

            m_flushTimerPeriod = new TimeSpan(0, 0, 0, 1, 000);
            WriteFrapsFileFormat = chkFrapsFormat.Checked;

            m_flushFileTimer = new Timer();
            m_flushFileTimer.AutoReset = true;
            m_flushFileTimer.Interval = m_flushTimerPeriod.TotalMilliseconds;
            m_flushFileTimer.Elapsed += OnFlushFileTimerElapsed;
            m_flushFileTimer.Enabled = true;

            m_stopWritingTimer = new Timer();
            m_stopWritingTimer.AutoReset = false;
            m_stopWritingTimer.Elapsed += StopWritingFileOnTimer;

            txtFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            m_pipeReader = new PipeReader();
            m_pipeReader.StateChanged += (p_sender, p_args) => UpdateStatus();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_flushFileTimer.Stop();

                if (components != null)
                {
                    components.Dispose();
                }
                lock (m_OSD)
                {
                    m_OSD.Dispose();
                }
            }

            base.Dispose(disposing);
        }


        private void StopWritingFileOnTimer(object p_sender, ElapsedEventArgs p_elapsedEventArgs)
        {
            m_pipeReader.StopWritingFile();
        }

        private void OnFlushFileTimerElapsed(object p_sender, ElapsedEventArgs p_elapsedEventArgs)
        {
            m_flushFileTimer.Enabled = false;
            try
            {
                if (m_pipeReader == null || !m_pipeReader.State.IsConnectionAccepted)
                {
                    return;
                }


                if (m_pipeReader.State.IsFileOpened)
                {
                    UpdateOSD();

                    if (m_pipeReader.EnabledWritingFile)
                    {
                        UpdateGuiOnTimer();
                    }
                }
            }
            finally
            {
                m_flushFileTimer.Enabled = true;
            }
        }

        private void UpdateGuiOnTimer()
        {
            m_pipeReader.FlushFileBuffer();

            Action action = () =>
            {
                if (IsDisposed || Disposing)
                    return;

                m_remainedTimeSpan -= m_flushTimerPeriod;
                lblRemained.Text = "remainded: " + m_remainedTimeSpan.ToString("c");

                m_lblWritingFileVisible = !m_lblWritingFileVisible;
                lblWritingFile.ForeColor = m_lblWritingFileVisible
                    ? Color.OrangeRed
                    : this.BackColor;
            };
            Invoke(action);
        }

        private void UpdateOSD()
        {
            string osdAuxialInfo = m_pipeReader.WriteFrapsFileFormat ? ", FRAPS format" : ", with CPU times";
            var osdTextBottom = string.Format("recorded {0} frames{1}", m_pipeReader.RecordedFrameTimes, osdAuxialInfo);
            string osdCPU = "Process not connected";

            if (EnsureOSDSlotsClaimed())
                return;

            var connectedProcess = m_pipeReader.ConnectedProcess;
            if (connectedProcess != null)
            {
                var currentProcess = Process.GetCurrentProcess();

                var ownCpuTotalCurrentValue = currentProcess.TotalProcessorTime.TotalMilliseconds;
                var connectedCpuTotalCurrentValue = connectedProcess.TotalProcessorTime.TotalMilliseconds;

                var usedByCurrentProcess = ownCpuTotalCurrentValue - m_previousOwnCPUTotal;
                m_previousOwnCPUTotal = ownCpuTotalCurrentValue;

                var usedByConnectedProcess = connectedCpuTotalCurrentValue - m_previousConnectedCPUTotal;
                m_previousConnectedCPUTotal = connectedCpuTotalCurrentValue;

                var percent = usedByCurrentProcess / (usedByConnectedProcess + usedByCurrentProcess + double.Epsilon);
                osdCPU = string.Format("TotalCPU={0:hh\\:mm\\:ss\\.ff}, time_reader={1:P2}", connectedProcess.TotalProcessorTime, percent);
            }

            lock (m_OSD)
            {
                if (Disposing || IsDisposed)
                    return;

                m_OSD.Update(m_osdSlotFile, m_pipeReader.OpenedFileName);
                m_OSD.Update(m_osdSlotTop, osdTextBottom);
                m_OSD.Update(m_osdSlotBottomCPU, osdCPU);
            }
        }

        private bool EnsureOSDSlotsClaimed()
        {
            if (m_osdSlotTop == null)
            {
                lock (m_OSD)
                {
                    if (Disposing || IsDisposed)
                        return true;

                    var osdSlots = m_OSD.FindOsdSlots();
                    if (osdSlots.Count > 0)
                    {
                        m_osdSlotFile = osdSlots[0];
                        m_osdSlotTop = osdSlots[1];
                        m_osdSlotBottomCPU = osdSlots[2];
                    }
                    else
                    {
                        var grabOSDSlots = m_OSD.GrabOSDSlots(3);
                        m_osdSlotFile = grabOSDSlots[0];
                        m_osdSlotTop = grabOSDSlots[1];
                        m_osdSlotBottomCPU = grabOSDSlots[2];
                    }
                }
            }

            return false;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var hotkey = new Hotkey(Win32A.KeyModifiers.Alt | Win32A.KeyModifiers.Ctrl, Keys.NumLock);

            var registered = RegisterHotkey(m_globalHotkeyAtom.Value, hotkey);
            if (registered)
            {
                RegistredHotkey = hotkey;
            }
            else
            {
                txtHotkeyEditor.Text = Win32A.KeyModifiers.None.ToString();
                if (m_globalHotkeyAtom.HasValue)
                {
                    Win32A.GlobalDeleteAtom(m_globalHotkeyAtom.Value);
                    m_globalHotkeyAtom = null;
                }
            }

            UpdateStatus();
            StartListening();
        }

        public bool RegisterHotkey(ushort p_atom, Hotkey p_hotkey)
        {
            var result = Win32A.RegisterHotKey(this.Handle, p_atom, p_hotkey.Modifiers, (int) p_hotkey.Key);
            if (false == result)
            {
                var win32Error = Marshal.GetLastWin32Error();
                if (win32Error != Win32A.ERROR_SUCCESS)
                {
                    var ex = new Win32Exception();
                    MessageBox.Show(this, ex.Message, "Cannot regitster hotkey", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            return result;
        }

        public void UnregisterHotkey(ushort? p_atom)
        {
            if (p_atom.HasValue)
            {
                var atom = p_atom.Value;

                Win32A.UnregisterHotKey(this.Handle, atom);
                Win32A.GlobalDeleteAtom(atom);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            UnregisterHotkey(m_globalHotkeyAtom);

            m_pipeReader.Dispose();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = txtFolder.Text;

            var dialogResult = dialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnStopStart_Click(object sender, EventArgs e)
        {
            btnStopStart.Text = "perfomin operation...";
            btnStopStart.Enabled = false;

            var pipeReaderState = m_pipeReader.State;

            if (pipeReaderState.IsStarted)
            {
                if (pipeReaderState.IsConnectionAccepted)
                {
                    m_pipeReader.ContinueAcceptingConnections = true;
                       m_pipeReader.DropConnection();
                }
                else
                {
                    m_pipeReader.Stop();
                }
            }
            else
                StartListening();
        }
        private void txtHotkeyEditor_Enter(object sender, EventArgs e)
        {
            var dialog = new HotkeyEditorDialog();

            dialog.RegistredHotkey = RegistredHotkey; 

            dialog.HotkeyProcessor = this;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                if (m_globalHotkeyAtom.HasValue)
                {
                    Win32A.UnregisterHotKey(this.Handle, m_globalHotkeyAtom.Value);

                    var win32Error = Marshal.GetLastWin32Error();
                    if (win32Error != Win32A.ERROR_SUCCESS)
                    {
                        var ex = new Win32Exception();
                        MessageBox.Show(this, ex.Message, "Cannot regitster hotkey", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                RegistredHotkey = dialog.NewHotkey;
                m_globalHotkeyAtom = dialog.NewHotkeyAtom;
            }

            this.ActiveControl = null;
        }

        private void rbUseHotkey_CheckedChanged(object sender, EventArgs e)
        {
            txtHotkeyEditor.Enabled = chkUseHotkey.Checked;
        }

        private void rbStartWritingImmediately_CheckedChanged(object sender, EventArgs e)
        {
            m_pipeReader.EnabledWritingFile = chkStartWritingImmediately.Checked;
        }

        public void UpdateStatus()
        {
            var currentReaderState = m_pipeReader.State;
            var newFlags = currentReaderState.GetDifference(m_previousPipeReaderState);

            if (0 != (newFlags & PipeReaderStateEnum.ConnectionAccepted))
            {
                m_pipeReader.TargetFolder = TargetFolder;
            }

            if (currentReaderState.IsFileOpening)
            {
                if (false == currentReaderState.IsFileOpened)
                {
                    m_pipeReader.WriteFrapsFileFormat = WriteFrapsFileFormat;
                }
            }

            var gui_action = new Action(() =>
            {
                UpdateGUIStatus(currentReaderState, m_previousPipeReaderState);
            });


            if (InvokeRequired)
            {
                BeginInvoke(gui_action);
            }
            else
            {
                gui_action();
            }
            m_previousPipeReaderState = currentReaderState;
        }

        private void UpdateGUIStatus(PipeReaderState p_currentReaderState, PipeReaderState p_previousPipeReaderState)
        {
            var clearedFlags = p_previousPipeReaderState.GetClearedFlags(p_previousPipeReaderState);
            /*

                        Debug.Print(Environment.NewLine);
                        Debug.Print("=====================================================================");
                        Debug.Print("Invoked state: {0}", currentReaderState);

                        if (newFlags != PipeReaderState.None)
                            Debug.Print("New flags: {0}", newFlags);
                        if (clearedFlags != PipeReaderState.None)
                            Debug.Print("Cleared flags: {0}", clearedFlags);

            */

            if (clearedFlags == PipeReaderStateEnum.ConnectionAccepted)
            {
                m_pipeReader.EnabledWritingFile = chkStartWritingImmediately.Checked;
            }

            if (p_currentReaderState != PipeReaderState.None && (false == m_previousStateIsError))
            {
                lblEroor.Visible = lblErrorInfo.Visible = p_currentReaderState.IsError;
                lblErrorInfo.Text = p_currentReaderState.IsError ? m_pipeReader.LastException.Message : string.Empty;
            }

            m_previousStateIsError = p_currentReaderState.IsError;

            if (p_currentReaderState.IsConnectionAccepted)
            {
                if (m_pipeReader.EnabledWritingFile)
                {
                    if (chkStopOnTimer.Checked)
                    {
                        var timePickerValue = timePicker.Value;

                        var hours = timePickerValue.Hour;
                        var minutes = timePickerValue.Minute;
                        var seconds = timePickerValue.Second;
                        m_remainedTimeSpan = new TimeSpan(hours, minutes, seconds);

                        m_stopWritingTimer.Interval = m_remainedTimeSpan.TotalMilliseconds;
                        m_stopWritingTimer.Start();

                        chkStopOnTimer.Enabled = false;
                        timePicker.Enabled = false;
                        lblRemained.Visible = true;
                        lblRemained.Text = "remainded: " + m_remainedTimeSpan.ToString("c");
                    }
                }
            }

            if ((false == chkStopOnTimer.Enabled) && (false == p_currentReaderState.IsFileOpened))
            {
                chkStopOnTimer.Enabled = true;
                timePicker.Enabled = true;
                lblRemained.Visible = false;
            }


            lblConnected.Visible = lblConnectionInfo.Visible = p_currentReaderState.IsPipeCreated;
            if (p_currentReaderState.IsPipeCreated)
            {
                if (false == p_currentReaderState.IsConnectionAccepted)
                {
                    lblConnected.Text = "Accepting connections\n\rto pipe:";
                    lblConnectionInfo.Text = m_pipeReader.PipeFullName;
                }
                else
                {
                    lblConnected.Text = "Connected";
                    lblConnectionInfo.Text = m_pipeReader.PipeFullName + " <-- " + m_pipeReader.ProcessName + "  (pID:" + m_pipeReader.ClientProcessId + ")";
                }
            }

            lblFIleName.Visible = lblWritingFile.Visible = p_currentReaderState.IsFileOpened;
            if (p_currentReaderState.IsFileOpened)
            {
                lblFIleName.Text = m_pipeReader.OpenedFileName;
                if (false == m_pipeReader.EnabledWritingFile)
                {
                    lblWritingFile.Text = "Opened file:";
                    lblWritingFile.ForeColor = Color.LimeGreen;
                }
                else
                {
                    lblWritingFile.Text = "Writing file:";
                    lblWritingFile.ForeColor = Color.OrangeRed;
                }
            }

            chkFrapsFormat.Enabled =
                txtFolder.Enabled =
                    btnSelectFolder.Enabled = (false == p_currentReaderState.IsFileOpened);


            btnStopStart.Enabled = true;

            txtPipeName.Enabled = (false == p_currentReaderState.IsStarted);

            if (p_currentReaderState.IsStarted)
            {
                if (p_currentReaderState.IsConnectionAccepted)
                {
                    btnStopStart.Text = "Close connection";
                }
                else if (p_currentReaderState.IsPipeCreated)
                {
                    btnStopStart.Text = "Stop listening";
                }
                else
                {
                    //Debugger.Break();
                    btnStopStart.Text = "...";
                }
            }
            else
            {
                btnStopStart.Text = "Start listening";
            }
        }

        private void StartListening()
        {
            m_pipeReader.StartFileName = "RTSS_Values.txt";
            m_pipeReader.PipeName = txtPipeName.Text;
            m_pipeReader.WriteFrapsFileFormat = WriteFrapsFileFormat;
            m_pipeReader.TargetFolder = txtFolder.Text;
            m_pipeReader.ContinueAcceptingConnections = true;

            m_pipeReader.StartAcceptingConnections();
        }

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);

            if (msg.Msg == (int) Win32A.WindowsMessages.WM_HOTKEY)
            {
                var lParam = msg.LParam.ToInt32();
                Keys key = (Keys)((lParam >> 16) & 0xFFFF);
                Win32A.KeyModifiers modifier = (Win32A.KeyModifiers)(lParam & 0xFFFF);
                int hotkeyId = msg.WParam.ToInt32();

                StartAndStopWritingFile();
            }
        }

        private void StartAndStopWritingFile()
        {
            if (m_pipeReader.EnabledWritingFile && m_pipeReader.State.IsConnectionAccepted)
            {
                m_pipeReader.StopWritingFile();
            }
            else
            {
                m_pipeReader.EnabledWritingFile = true;
            }
        }

        private void chkFrapsFormat_CheckedChanged(object sender, EventArgs e)
        {
            WriteFrapsFileFormat = chkFrapsFormat.Checked;
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            StartFileName = Path.Combine("RTSS_Values.txt");
            TargetFolder = txtFolder.Text;
        }
    }
}
