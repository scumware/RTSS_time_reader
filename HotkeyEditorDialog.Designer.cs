namespace RTSS_time_reader
{
    partial class HotkeyEditorDialog
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
            this.btnOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtHotkeyEditor = new System.Windows.Forms.TextBox();
            this.layoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblEnterHotkey = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.layoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(231, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.txtHotkeyEditor, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.layoutPanelButtons, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblEnterHotkey, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(393, 117);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtHotkeyEditor
            // 
            this.txtHotkeyEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHotkeyEditor.Location = new System.Drawing.Point(81, 48);
            this.txtHotkeyEditor.Name = "txtHotkeyEditor";
            this.txtHotkeyEditor.Size = new System.Drawing.Size(229, 20);
            this.txtHotkeyEditor.TabIndex = 10;
            // 
            // layoutPanelButtons
            // 
            this.layoutPanelButtons.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.layoutPanelButtons, 2);
            this.layoutPanelButtons.Controls.Add(this.btnOk);
            this.layoutPanelButtons.Controls.Add(this.btnCancel);
            this.layoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutPanelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.layoutPanelButtons.Location = new System.Drawing.Point(81, 85);
            this.layoutPanelButtons.Name = "layoutPanelButtons";
            this.layoutPanelButtons.Size = new System.Drawing.Size(309, 29);
            this.layoutPanelButtons.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(150, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblEnterHotkey
            // 
            this.lblEnterHotkey.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEnterHotkey.AutoSize = true;
            this.lblEnterHotkey.Location = new System.Drawing.Point(5, 52);
            this.lblEnterHotkey.Name = "lblEnterHotkey";
            this.lblEnterHotkey.Size = new System.Drawing.Size(70, 13);
            this.lblEnterHotkey.TabIndex = 13;
            this.lblEnterHotkey.Text = "Enter hotkey:";
            // 
            // HotkeyEditorDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(393, 117);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HotkeyEditorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hotkey editor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.layoutPanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtHotkeyEditor;
        private System.Windows.Forms.FlowLayoutPanel layoutPanelButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblEnterHotkey;
    }
}