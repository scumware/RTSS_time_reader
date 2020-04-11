namespace RTSS_time_reader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnStopStart = new System.Windows.Forms.Button();
            this.lblPipeName = new System.Windows.Forms.Label();
            this.lblFolder = new System.Windows.Forms.Label();
            this.txtPipeName = new System.Windows.Forms.TextBox();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblConnected = new System.Windows.Forms.Label();
            this.lblConnectionInfo = new System.Windows.Forms.Label();
            this.lblWritingFile = new System.Windows.Forms.Label();
            this.lblFIleName = new System.Windows.Forms.Label();
            this.lblEroor = new System.Windows.Forms.Label();
            this.lblErrorInfo = new System.Windows.Forms.Label();
            this.chkFrapsFormat = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnStopStart, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblPipeName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFolder, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPipeName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFolder, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectFolder, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblConnected, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblConnectionInfo, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblWritingFile, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblFIleName, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblEroor, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblErrorInfo, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.chkFrapsFormat, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(805, 226);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnStopStart
            // 
            this.btnStopStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopStart.Location = new System.Drawing.Point(582, 119);
            this.btnStopStart.Name = "btnStopStart";
            this.btnStopStart.Size = new System.Drawing.Size(200, 23);
            this.btnStopStart.TabIndex = 3;
            this.btnStopStart.Text = "Stop listening";
            this.btnStopStart.UseVisualStyleBackColor = true;
            this.btnStopStart.Click += new System.EventHandler(this.btnStopStart_Click);
            // 
            // lblPipeName
            // 
            this.lblPipeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPipeName.AutoSize = true;
            this.lblPipeName.Location = new System.Drawing.Point(3, 42);
            this.lblPipeName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblPipeName.Name = "lblPipeName";
            this.lblPipeName.Size = new System.Drawing.Size(75, 13);
            this.lblPipeName.TabIndex = 0;
            this.lblPipeName.Text = "Pipe name:";
            // 
            // lblFolder
            // 
            this.lblFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(3, 84);
            this.lblFolder.Margin = new System.Windows.Forms.Padding(3, 26, 3, 3);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(75, 13);
            this.lblFolder.TabIndex = 1;
            this.lblFolder.Text = "Folder name:";
            // 
            // txtPipeName
            // 
            this.txtPipeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPipeName.Enabled = false;
            this.txtPipeName.Location = new System.Drawing.Point(84, 35);
            this.txtPipeName.MinimumSize = new System.Drawing.Size(100, 4);
            this.txtPipeName.Name = "txtPipeName";
            this.txtPipeName.Size = new System.Drawing.Size(492, 20);
            this.txtPipeName.TabIndex = 2;
            this.txtPipeName.Text = "RTSS_Frametime";
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(84, 81);
            this.txtFolder.Margin = new System.Windows.Forms.Padding(3, 23, 3, 3);
            this.txtFolder.MinimumSize = new System.Drawing.Size(100, 4);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(492, 20);
            this.txtFolder.TabIndex = 4;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFolder.Location = new System.Drawing.Point(582, 81);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(3, 23, 3, 3);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(200, 23);
            this.btnSelectFolder.TabIndex = 3;
            this.btnSelectFolder.Text = "Select folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblConnected
            // 
            this.lblConnected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnected.AutoSize = true;
            this.lblConnected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConnected.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblConnected.Location = new System.Drawing.Point(3, 169);
            this.lblConnected.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(75, 13);
            this.lblConnected.TabIndex = 0;
            this.lblConnected.Text = "Connected";
            this.lblConnected.Visible = false;
            // 
            // lblConnectionInfo
            // 
            this.lblConnectionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectionInfo.AutoSize = true;
            this.lblConnectionInfo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConnectionInfo.Location = new System.Drawing.Point(84, 169);
            this.lblConnectionInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblConnectionInfo.Name = "lblConnectionInfo";
            this.lblConnectionInfo.Size = new System.Drawing.Size(492, 13);
            this.lblConnectionInfo.TabIndex = 0;
            this.lblConnectionInfo.Text = "Pipe name:";
            // 
            // lblWritingFile
            // 
            this.lblWritingFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWritingFile.AutoSize = true;
            this.lblWritingFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWritingFile.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblWritingFile.Location = new System.Drawing.Point(3, 189);
            this.lblWritingFile.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblWritingFile.Name = "lblWritingFile";
            this.lblWritingFile.Size = new System.Drawing.Size(75, 13);
            this.lblWritingFile.TabIndex = 0;
            this.lblWritingFile.Text = "Writing File:";
            this.lblWritingFile.Visible = false;
            // 
            // lblFIleName
            // 
            this.lblFIleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFIleName.AutoSize = true;
            this.lblFIleName.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFIleName.ForeColor = System.Drawing.Color.Green;
            this.lblFIleName.Location = new System.Drawing.Point(84, 189);
            this.lblFIleName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblFIleName.Name = "lblFIleName";
            this.lblFIleName.Size = new System.Drawing.Size(492, 13);
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
            this.lblEroor.Size = new System.Drawing.Size(75, 13);
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
            this.lblErrorInfo.Location = new System.Drawing.Point(84, 210);
            this.lblErrorInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblErrorInfo.Name = "lblErrorInfo";
            this.lblErrorInfo.Size = new System.Drawing.Size(492, 13);
            this.lblErrorInfo.TabIndex = 0;
            this.lblErrorInfo.Text = "error info";
            // 
            // chkFrapsFormat
            // 
            this.chkFrapsFormat.AutoSize = true;
            this.chkFrapsFormat.Checked = true;
            this.chkFrapsFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFrapsFormat.Location = new System.Drawing.Point(84, 119);
            this.chkFrapsFormat.Name = "chkFrapsFormat";
            this.chkFrapsFormat.Size = new System.Drawing.Size(137, 17);
            this.chkFrapsFormat.TabIndex = 5;
            this.chkFrapsFormat.Text = "Write FRAPS file format";
            this.chkFrapsFormat.UseVisualStyleBackColor = true;
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
    }
}

