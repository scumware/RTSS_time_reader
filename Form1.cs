using System;
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


        public MainForm()
        {
            InitializeComponent();

            m_uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

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
            btnSelectFolder.Enabled =
            btnSelectFolder.Enabled = (false == m_pipeReader.IsStarted);

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
            m_pipeReader.PipeName = textPipeName.Text;
            m_pipeReader.WriteFrapsFileFormat = chkFrapsFormat.Checked;
            m_pipeReader.StartAcceptingConnections();
        }
    }
}
