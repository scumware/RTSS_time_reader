﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace RTSS_time_reader
{
    public partial class MainForm : Form
    {
        private readonly System.Timers.Timer m_timer;

        private TaskScheduler m_uiScheduler;
        private readonly PipeReader m_pipeReader;

        private ushort? m_globalHotkeyAtom;
        private int? m_startStopWritingHotkeyId;
        private Keys? m_registredHotkey;

        public MainForm()
        {
            InitializeComponent();

            m_uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var newAtom = Win32A.GlobalAddAtom("RTSS_time_reader");
            if (newAtom != 0)
            {
                m_globalHotkeyAtom = newAtom;
            }

            m_timer = new System.Timers.Timer();
            m_timer.AutoReset = true;
            m_timer.Interval = 1000;
            m_timer.Elapsed += OnTimerElapsed;

            txtFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            m_pipeReader = new PipeReader();
            m_pipeReader.StateChanged += (p_sender, p_args) => UpdateStatus();
        }

        private string FileFullName()
        {
            return Path.Combine(txtFolder.Text, "RTSS_Values.txt");
        }

        private void OnTimerElapsed(object p_sender, ElapsedEventArgs p_elapsedEventArgs)
        {
            if (m_pipeReader != null && m_pipeReader.IsConnectionAccepted)
            {
                m_pipeReader.FlushFileBuffer();
            }

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
             
            UpdateStatus();
            StartListening();
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


        public void UpdateStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action (UpdateStatus));
                return;
            }

            m_timer.Enabled = m_pipeReader.IsFileOpened;

            lblEroor.Visible = lblErrorInfo.Visible = m_pipeReader.IsError;
            lblErrorInfo.Text = m_pipeReader.IsError? m_pipeReader.LastException.Message:string.Empty;

            lblConnected.Visible = lblConnectionInfo.Visible = m_pipeReader.IsPipeCreated;
            if (m_pipeReader.IsPipeCreated)
            {
                if (false == m_pipeReader.IsConnectionAccepted)
                {
                    lblConnected.Text = "Accepting";
                    lblConnectionInfo.Text = m_pipeReader.PipeFullName;
                }
                else
                {
                    lblConnected.Text = "Connected";
                    lblConnectionInfo.Text = m_pipeReader.PipeFullName + " <-- " + m_pipeReader.ClientProcessId;
                }
            } 

            lblFIleName.Visible  = lblWritingFile.Visible = m_pipeReader.IsFileOpened;
            if (m_pipeReader.IsFileOpened)
                lblFIleName.Text = m_pipeReader.FileName;

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

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new FolderBrowserDialog();
            var dialogResult = dialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
                m_pipeReader.FileName = Path.Combine(txtFolder.Text, "RTSS_Values.txt");
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

        private void StartListening()
        {
            m_pipeReader.FileName = FileFullName();
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
            }
        }

        private void txtHotkeyEditor_Enter(object sender, EventArgs e)
        {
            var dialog = new HotkeyEditorDialog();
            dialog.Atom = m_globalHotkeyAtom;
            dialog.Hotkey = m_registredHotkey;
            dialog.RegistredHotkeyId = m_startStopWritingHotkeyId;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                m_startStopWritingHotkeyId = dialog.RegistredHotkeyId;
                m_registredHotkey = dialog.RegistredHotkey;
            }

            this.ActiveControl = null;
        }
    }
}
