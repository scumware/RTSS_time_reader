using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace RTSS_time_reader
{
    public partial class MainForm : Form
    {

        private TaskScheduler m_uiScheduler;
        private readonly PipeReader m_pipeReader;
        private readonly Timer m_stopWritingTimer;
        private readonly System.Timers.Timer m_flushFileTimer;

        private ushort? m_globalHotkeyAtom;
        private int? m_startStopWritingHotkeyId;
        private Keys? m_registredHotkey;
        private Win32A.KeyModifiers? m_registredHotkeyModifiers;
        private bool m_previousStateIsError = false;
        private TimeSpan m_remainedTimeSpan;

        public MainForm()
        {
            InitializeComponent();

            m_uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var newAtom = Win32A.GlobalAddAtom("RTSS_time_reader");
            if (newAtom != 0)
            {
                m_globalHotkeyAtom = newAtom;
            }

            m_flushFileTimer = new System.Timers.Timer();
            m_flushFileTimer.AutoReset = true;
            m_flushFileTimer.Interval = 1000;
            m_flushFileTimer.Elapsed += OnFlushFileTimerElapsed;

            m_stopWritingTimer = new System.Timers.Timer();
            m_stopWritingTimer.AutoReset = false;
            m_stopWritingTimer.Elapsed += StopWritingFileOnTimer;

            txtFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            m_pipeReader = new PipeReader();
            m_pipeReader.StateChanged += (p_sender, p_args) => UpdateStatus();
        }

        private void StopWritingFileOnTimer(object p_sender, ElapsedEventArgs p_elapsedEventArgs)
        {
            m_pipeReader.StopWritingFile();
        }

        private string FileFullName()
        {
            return Path.Combine(txtFolder.Text, "RTSS_Values.txt");
        }

        private void OnFlushFileTimerElapsed(object p_sender, ElapsedEventArgs p_elapsedEventArgs)
        {
            if (m_pipeReader != null && m_pipeReader.IsConnectionAccepted && m_pipeReader.EnabledWritingFile)
            {
                m_pipeReader.FlushFileBuffer();
                Action action = () =>
                {
                    m_remainedTimeSpan -= new TimeSpan(0, 0, 1);
                    lblRemained.Text = "remainded: " + m_remainedTimeSpan.ToString("c");
                };
                Invoke(action);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            m_globalHotkeyAtom = Win32A.GlobalAddAtom("RTSS_READER_HOTKEYs");
            var keyModifiers = Win32A.KeyModifiers.Alt | Win32A.KeyModifiers.Ctrl;
            var key = Keys.NumLock;

            var registered = RegisterHotkey(m_globalHotkeyAtom.Value, keyModifiers, key);
            if (registered)
                txtHotkeyEditor.Text = keyModifiers.GetDescription() + "+" + key;
            else
                txtHotkeyEditor.Text = Win32A.KeyModifiers.None.ToString();

            UpdateStatus();
            StartListening();
        }

        public bool RegisterHotkey(ushort atom, Win32A.KeyModifiers keyModifiers, Keys key)
        {
            var result = Win32A.RegisterHotKey(this.Handle, atom, keyModifiers, (int) Keys.NumLock);
            if (result)
            {
                m_registredHotkeyModifiers = keyModifiers;
                m_registredHotkey = key;
                m_globalHotkeyAtom = atom;
            }
            else
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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (m_globalHotkeyAtom.HasValue)
            {
                Win32A.GlobalDeleteAtom(m_globalHotkeyAtom.Value);
            }
            m_pipeReader.Dispose();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new FolderBrowserDialog();
            var dialogResult = dialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
                m_pipeReader.StartFileName = Path.Combine(txtFolder.Text, "RTSS_Values.txt");
            }
        }

        private void btnStopStart_Click(object sender, EventArgs e)
        {
            btnStopStart.Text = "perfomin operation...";
            btnStopStart.Enabled = false;

            if (m_pipeReader.IsStarted)
                m_pipeReader.Stop();
            else
                StartListening();
        }
        private void txtHotkeyEditor_Enter(object sender, EventArgs e)
        {
            var dialog = new HotkeyEditorDialog();
            dialog.Atom = m_globalHotkeyAtom;
            dialog.RegistredHotkeyModifiers = m_registredHotkeyModifiers;
            dialog.RegistredHotkey = m_registredHotkey;
            dialog.RegistredHotkeyId = m_startStopWritingHotkeyId;
            dialog.HotkeyProcessor = this;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                if (m_startStopWritingHotkeyId.HasValue)
                {
                    Win32A.UnregisterHotKey(this.Handle, m_startStopWritingHotkeyId.Value);
                    var win32Error = Marshal.GetLastWin32Error();
                    if (win32Error != Win32A.ERROR_SUCCESS)
                    {
                        var ex = new Win32Exception();
                        MessageBox.Show(this, ex.Message, "Cannot regitster hotkey", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                m_startStopWritingHotkeyId = dialog.RegistredHotkeyId;
                m_registredHotkey = dialog.RegistredHotkey;
            }

            this.ActiveControl = null;
        }

        private void rbUseHotkey_CheckedChanged(object sender, EventArgs e)
        {
            txtHotkeyEditor.Enabled = rbUseHotkey.Checked;
        }

        private void rbStartWritingImmediately_CheckedChanged(object sender, EventArgs e)
        {
            m_pipeReader.EnabledWritingFile = rbStartWritingImmediately.Checked;
        }

        public void UpdateStatus()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action (UpdateStatus));
                return;
            }
            m_flushFileTimer.Enabled = m_pipeReader.IsFileOpened;

            if (m_pipeReader.State != PipeReaderState.None && (false == m_previousStateIsError))
            {
                lblEroor.Visible = lblErrorInfo.Visible = m_pipeReader.IsError;
                lblErrorInfo.Text = m_pipeReader.IsError ? m_pipeReader.LastException.Message : string.Empty;
            }
            m_previousStateIsError = m_pipeReader.IsError;

            if (m_pipeReader.EnabledWritingFile && m_pipeReader.IsConnectionAccepted)
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
                    lblRemained.Text = "remainded: "+m_remainedTimeSpan.ToString("c");
                }
            }

            if ((false == chkStopOnTimer.Enabled) && (false == m_pipeReader.IsFileOpened))
            {
                chkStopOnTimer.Enabled = true;
                timePicker.Enabled = true;
                lblRemained.Visible = false;
            }


            lblConnected.Visible = lblConnectionInfo.Visible = m_pipeReader.IsPipeCreated;
            if (m_pipeReader.IsPipeCreated)
            {
                if (false == m_pipeReader.IsConnectionAccepted)
                {
                    lblConnected.Text = "Accepting connections\n\rto pipe:";
                    lblConnectionInfo.Text = m_pipeReader.PipeFullName;
                }
                else
                {
                    lblConnected.Text = "Connected";
                    lblConnectionInfo.Text = m_pipeReader.PipeFullName + " <-- pID=" + m_pipeReader.ClientProcessId;
                }
            } 

            lblFIleName.Visible  = lblWritingFile.Visible = m_pipeReader.IsFileOpened;
            if (m_pipeReader.IsFileOpened)
                lblFIleName.Text = m_pipeReader.StartFileName;

            chkFrapsFormat.Enabled =
            txtFolder.Enabled =
            btnSelectFolder.Enabled = (false == m_pipeReader.IsFileOpened);


            btnStopStart.Enabled = true;

            txtPipeName.Enabled = (false == m_pipeReader.IsStarted);

            if (m_pipeReader.IsStarted)
            {
                if (m_pipeReader.IsConnectionAccepted)
                {
                    btnStopStart.Text = "Close connection";
                }
                else if (m_pipeReader.IsPipeCreated)
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
            m_pipeReader.StartFileName = FileFullName();
            m_pipeReader.PipeName = txtPipeName.Text;
            m_pipeReader.WriteFrapsFileFormat = chkFrapsFormat.Checked;
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
                if (m_pipeReader.EnabledWritingFile && m_pipeReader.IsConnectionAccepted)
                {
                    m_pipeReader.StopWritingFile();
                }
                else
                {
                    m_pipeReader.EnabledWritingFile = true;
                }
            }
        }

    }
}
