using System.Windows.Forms;

namespace RTSS_time_reader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFolder = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lblPipeName = new System.Windows.Forms.Label();
            this.txtPipeName = new System.Windows.Forms.TextBox();
            this.lblWritingFile = new System.Windows.Forms.Label();
            this.lblFIleName = new System.Windows.Forms.Label();
            this.lblEroor = new System.Windows.Forms.Label();
            this.lblErrorInfo = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkFrapsFormat = new System.Windows.Forms.CheckBox();
            this.chkStartWritingImmediately = new System.Windows.Forms.CheckBox();
            this.chkUseHotkey = new System.Windows.Forms.CheckBox();
            this.txtHotkeyEditor = new System.Windows.Forms.TextBox();
            this.btnStopStart = new System.Windows.Forms.Button();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkStopOnTimer = new System.Windows.Forms.CheckBox();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.lblRemained = new System.Windows.Forms.Label();
            this.stackPipeState = new System.Windows.Forms.FlowLayoutPanel();
            this.lblConnected = new System.Windows.Forms.Label();
            this.lblConnectionInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.stackPipeState.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lblFolder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFolder, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPipeName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPipeName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblWritingFile, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblFIleName, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblEroor, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblErrorInfo, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnStopStart, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectFolder, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.stackPipeState, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(805, 226);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblFolder
            // 
            this.lblFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(3, 29);
            this.lblFolder.Margin = new System.Windows.Forms.Padding(3, 26, 3, 5);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(76, 13);
            this.lblFolder.TabIndex = 1;
            this.lblFolder.Text = "Folder name:";
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(85, 24);
            this.txtFolder.Margin = new System.Windows.Forms.Padding(3, 23, 3, 3);
            this.txtFolder.MinimumSize = new System.Drawing.Size(100, 4);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(485, 20);
            this.txtFolder.TabIndex = 4;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            // 
            // lblPipeName
            // 
            this.lblPipeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPipeName.AutoSize = true;
            this.lblPipeName.Location = new System.Drawing.Point(3, 76);
            this.lblPipeName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.lblPipeName.Name = "lblPipeName";
            this.lblPipeName.Size = new System.Drawing.Size(76, 13);
            this.lblPipeName.TabIndex = 0;
            this.lblPipeName.Text = "Pipe name:";
            // 
            // txtPipeName
            // 
            this.txtPipeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPipeName.Enabled = false;
            this.txtPipeName.Location = new System.Drawing.Point(85, 71);
            this.txtPipeName.MinimumSize = new System.Drawing.Size(100, 4);
            this.txtPipeName.Name = "txtPipeName";
            this.txtPipeName.Size = new System.Drawing.Size(485, 20);
            this.txtPipeName.TabIndex = 2;
            this.txtPipeName.Text = "RTSS_Frametime";
            // 
            // lblWritingFile
            // 
            this.lblWritingFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWritingFile.AutoSize = true;
            this.lblWritingFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWritingFile.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblWritingFile.Location = new System.Drawing.Point(3, 194);
            this.lblWritingFile.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblWritingFile.Name = "lblWritingFile";
            this.lblWritingFile.Size = new System.Drawing.Size(76, 13);
            this.lblWritingFile.TabIndex = 0;
            this.lblWritingFile.Text = "Opened file:";
            this.lblWritingFile.Visible = false;
            // 
            // lblFIleName
            // 
            this.lblFIleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFIleName.AutoSize = true;
            this.lblFIleName.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFIleName.ForeColor = System.Drawing.Color.Green;
            this.lblFIleName.Location = new System.Drawing.Point(85, 194);
            this.lblFIleName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblFIleName.Name = "lblFIleName";
            this.lblFIleName.Size = new System.Drawing.Size(485, 13);
            this.lblFIleName.TabIndex = 0;
            this.lblFIleName.Text = "File name:";
            // 
            // lblEroor
            // 
            this.lblEroor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEroor.AutoSize = true;
            this.lblEroor.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEroor.ForeColor = System.Drawing.Color.Red;
            this.lblEroor.Location = new System.Drawing.Point(3, 210);
            this.lblEroor.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblEroor.Name = "lblEroor";
            this.lblEroor.Size = new System.Drawing.Size(76, 13);
            this.lblEroor.TabIndex = 0;
            this.lblEroor.Text = "error";
            // 
            // lblErrorInfo
            // 
            this.lblErrorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrorInfo.AutoSize = true;
            this.lblErrorInfo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblErrorInfo.ForeColor = System.Drawing.Color.Red;
            this.lblErrorInfo.Location = new System.Drawing.Point(85, 210);
            this.lblErrorInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblErrorInfo.Name = "lblErrorInfo";
            this.lblErrorInfo.Size = new System.Drawing.Size(485, 13);
            this.lblErrorInfo.TabIndex = 0;
            this.lblErrorInfo.Text = "error info";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.chkFrapsFormat);
            this.flowLayoutPanel1.Controls.Add(this.chkStartWritingImmediately);
            this.flowLayoutPanel1.Controls.Add(this.chkUseHotkey);
            this.flowLayoutPanel1.Controls.Add(this.txtHotkeyEditor);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(85, 97);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(445, 49);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // chkFrapsFormat
            // 
            this.chkFrapsFormat.AutoSize = true;
            this.chkFrapsFormat.Checked = true;
            this.chkFrapsFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flowLayoutPanel1.SetFlowBreak(this.chkFrapsFormat, true);
            this.chkFrapsFormat.Location = new System.Drawing.Point(3, 3);
            this.chkFrapsFormat.Name = "chkFrapsFormat";
            this.chkFrapsFormat.Size = new System.Drawing.Size(137, 17);
            this.chkFrapsFormat.TabIndex = 5;
            this.chkFrapsFormat.Text = "Write FRAPS file format";
            this.chkFrapsFormat.UseVisualStyleBackColor = true;
            this.chkFrapsFormat.CheckedChanged += new System.EventHandler(this.chkFrapsFormat_CheckedChanged);
            // 
            // chkStartWritingImmediately
            // 
            this.chkStartWritingImmediately.AutoSize = true;
            this.chkStartWritingImmediately.Location = new System.Drawing.Point(3, 26);
            this.chkStartWritingImmediately.Name = "chkStartWritingImmediately";
            this.chkStartWritingImmediately.Size = new System.Drawing.Size(154, 17);
            this.chkStartWritingImmediately.TabIndex = 7;
            this.chkStartWritingImmediately.Text = "Start writing file immediately";
            this.chkStartWritingImmediately.UseVisualStyleBackColor = true;
            this.chkStartWritingImmediately.CheckedChanged += new System.EventHandler(this.rbStartWritingImmediately_CheckedChanged);
            // 
            // chkUseHotkey
            // 
            this.chkUseHotkey.AutoSize = true;
            this.chkUseHotkey.Checked = true;
            this.chkUseHotkey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseHotkey.Location = new System.Drawing.Point(163, 26);
            this.chkUseHotkey.Name = "chkUseHotkey";
            this.chkUseHotkey.Size = new System.Drawing.Size(143, 17);
            this.chkUseHotkey.TabIndex = 8;
            this.chkUseHotkey.Text = "Start and Stop by hotkey";
            this.chkUseHotkey.UseVisualStyleBackColor = true;
            this.chkUseHotkey.CheckedChanged += new System.EventHandler(this.rbUseHotkey_CheckedChanged);
            // 
            // txtHotkeyEditor
            // 
            this.txtHotkeyEditor.Location = new System.Drawing.Point(312, 26);
            this.txtHotkeyEditor.Name = "txtHotkeyEditor";
            this.txtHotkeyEditor.Size = new System.Drawing.Size(130, 20);
            this.txtHotkeyEditor.TabIndex = 9;
            this.txtHotkeyEditor.Enter += new System.EventHandler(this.txtHotkeyEditor_Enter);
            // 
            // btnStopStart
            // 
            this.btnStopStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopStart.Location = new System.Drawing.Point(576, 68);
            this.btnStopStart.Name = "btnStopStart";
            this.btnStopStart.Size = new System.Drawing.Size(206, 23);
            this.btnStopStart.TabIndex = 3;
            this.btnStopStart.Text = "Stop listening";
            this.btnStopStart.UseVisualStyleBackColor = true;
            this.btnStopStart.Click += new System.EventHandler(this.btnStopStart_Click);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFolder.Location = new System.Drawing.Point(576, 23);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(3, 23, 3, 3);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(206, 21);
            this.btnSelectFolder.TabIndex = 3;
            this.btnSelectFolder.Text = "Select folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.chkStopOnTimer);
            this.flowLayoutPanel2.Controls.Add(this.timePicker);
            this.flowLayoutPanel2.Controls.Add(this.lblRemained);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(576, 97);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanel2, 3);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(205, 39);
            this.flowLayoutPanel2.TabIndex = 8;
            // 
            // chkStopOnTimer
            // 
            this.chkStopOnTimer.AutoSize = true;
            this.chkStopOnTimer.Location = new System.Drawing.Point(3, 5);
            this.chkStopOnTimer.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.chkStopOnTimer.Name = "chkStopOnTimer";
            this.chkStopOnTimer.Size = new System.Drawing.Size(124, 17);
            this.chkStopOnTimer.TabIndex = 0;
            this.chkStopOnTimer.Text = "Stop writing file after:";
            this.chkStopOnTimer.UseVisualStyleBackColor = true;
            // 
            // timePicker
            // 
            this.flowLayoutPanel2.SetFlowBreak(this.timePicker, true);
            this.timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePicker.Location = new System.Drawing.Point(133, 3);
            this.timePicker.Name = "timePicker";
            this.timePicker.ShowUpDown = true;
            this.timePicker.Size = new System.Drawing.Size(69, 20);
            this.timePicker.TabIndex = 1;
            this.timePicker.Value = new System.DateTime(2020, 4, 18, 0, 4, 0, 0);
            // 
            // lblRemained
            // 
            this.lblRemained.AutoSize = true;
            this.lblRemained.ForeColor = System.Drawing.Color.Maroon;
            this.lblRemained.Location = new System.Drawing.Point(3, 26);
            this.lblRemained.Name = "lblRemained";
            this.lblRemained.Size = new System.Drawing.Size(56, 13);
            this.lblRemained.TabIndex = 2;
            this.lblRemained.Text = "remained: ";
            this.lblRemained.Visible = false;
            // 
            // stackPipeState
            // 
            this.stackPipeState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.stackPipeState, 2);
            this.stackPipeState.Controls.Add(this.lblConnected);
            this.stackPipeState.Controls.Add(this.lblConnectionInfo);
            this.stackPipeState.Location = new System.Drawing.Point(3, 152);
            this.stackPipeState.Name = "stackPipeState";
            this.stackPipeState.Size = new System.Drawing.Size(567, 39);
            this.stackPipeState.TabIndex = 9;
            // 
            // lblConnected
            // 
            this.lblConnected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnected.AutoSize = true;
            this.lblConnected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConnected.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblConnected.Location = new System.Drawing.Point(3, 0);
            this.lblConnected.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(68, 13);
            this.lblConnected.TabIndex = 0;
            this.lblConnected.Text = "Connected";
            this.lblConnected.Visible = false;
            // 
            // lblConnectionInfo
            // 
            this.lblConnectionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblConnectionInfo.AutoSize = true;
            this.lblConnectionInfo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConnectionInfo.Location = new System.Drawing.Point(77, 0);
            this.lblConnectionInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblConnectionInfo.Name = "lblConnectionInfo";
            this.lblConnectionInfo.Size = new System.Drawing.Size(67, 13);
            this.lblConnectionInfo.TabIndex = 0;
            this.lblConnectionInfo.Text = "Pipe name:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 226);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "RTSS frametime to CSV";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.stackPipeState.ResumeLayout(false);
            this.stackPipeState.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblPipeName;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox txtPipeName;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Button btnStopStart;
        private System.Windows.Forms.Label lblErrorInfo;
        private System.Windows.Forms.Label lblConnected;
        private System.Windows.Forms.Label lblWritingFile;
        private System.Windows.Forms.Label lblFIleName;
        private System.Windows.Forms.Label lblConnectionInfo;
        private System.Windows.Forms.Label lblEroor;
        private System.Windows.Forms.CheckBox chkFrapsFormat;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private CheckBox chkStartWritingImmediately;
        private CheckBox chkUseHotkey;
        private System.Windows.Forms.TextBox txtHotkeyEditor;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.CheckBox chkStopOnTimer;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.FlowLayoutPanel stackPipeState;
        private System.Windows.Forms.Label lblRemained;
    }
}

