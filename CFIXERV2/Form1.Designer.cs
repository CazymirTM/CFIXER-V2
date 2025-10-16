namespace CFIXERV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblSelection = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnExitApp = new System.Windows.Forms.Button();
            this.btnApplyAll = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.progressBarFixes = new System.Windows.Forms.ProgressBar();
            this.btnContact = new System.Windows.Forms.Button();
            this.lblWarning = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSelection
            // 
            this.lblSelection.AutoSize = true;
            this.lblSelection.BackColor = System.Drawing.Color.Gray;
            this.lblSelection.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelection.Location = new System.Drawing.Point(131, 133);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(277, 19);
            this.lblSelection.TabIndex = 3;
            this.lblSelection.Text = "Please select what fix do you want to apply:";
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Gray;
            this.btnApply.Font = new System.Drawing.Font("Liberation Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(12, 196);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(114, 42);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnExitApp
            // 
            this.btnExitApp.BackColor = System.Drawing.Color.Gray;
            this.btnExitApp.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExitApp.Location = new System.Drawing.Point(11, 499);
            this.btnExitApp.Name = "btnExitApp";
            this.btnExitApp.Size = new System.Drawing.Size(77, 37);
            this.btnExitApp.TabIndex = 5;
            this.btnExitApp.Text = "Exit App";
            this.btnExitApp.UseVisualStyleBackColor = false;
            this.btnExitApp.Click += new System.EventHandler(this.BtnExitApp_Click);
            // 
            // btnApplyAll
            // 
            this.btnApplyAll.BackColor = System.Drawing.Color.Gray;
            this.btnApplyAll.Font = new System.Drawing.Font("Liberation Sans", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyAll.Location = new System.Drawing.Point(11, 133);
            this.btnApplyAll.Name = "btnApplyAll";
            this.btnApplyAll.Size = new System.Drawing.Size(114, 57);
            this.btnApplyAll.TabIndex = 7;
            this.btnApplyAll.Text = "Apply all fixes";
            this.btnApplyAll.UseVisualStyleBackColor = false;
            this.btnApplyAll.Click += new System.EventHandler(this.BtnApplyAll_Click);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLog.Location = new System.Drawing.Point(167, 291);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(207, 25);
            this.lblLog.TabIndex = 8;
            this.lblLog.Text = "Awaiting operation...";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "SFC Scan",
            "DISM Tools",
            "CHKDSK /R",
            "Network Fixes",
            "Windows Update Reset",
            "Firewall Reset"});
            this.checkedListBox1.Location = new System.Drawing.Point(131, 155);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(451, 94);
            this.checkedListBox1.TabIndex = 9;
            // 
            // progressBarFixes
            // 
            this.progressBarFixes.Location = new System.Drawing.Point(131, 253);
            this.progressBarFixes.Name = "progressBarFixes";
            this.progressBarFixes.Size = new System.Drawing.Size(451, 34);
            this.progressBarFixes.TabIndex = 10;
            // 
            // btnContact
            // 
            this.btnContact.BackColor = System.Drawing.Color.Gray;
            this.btnContact.Font = new System.Drawing.Font("Liberation Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContact.Location = new System.Drawing.Point(11, 470);
            this.btnContact.Name = "btnContact";
            this.btnContact.Size = new System.Drawing.Size(102, 23);
            this.btnContact.TabIndex = 11;
            this.btnContact.Text = "Contact Me";
            this.btnContact.UseVisualStyleBackColor = false;
            this.btnContact.Click += new System.EventHandler(this.BtnContact_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(12, 549);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(99, 20);
            this.lblWarning.TabIndex = 12;
            this.lblWarning.Text = "WARNING:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 569);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(463, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Only use chkdsk command if you have problems with your hard disk/ssd or with DISM" +
    " command.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::CFIXERV2.Properties.Resources.logocc;
            this.pictureBox1.Location = new System.Drawing.Point(132, -3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(772, 127);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CFIXERV2.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(1041, 595);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.btnContact);
            this.Controls.Add(this.progressBarFixes);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnApplyAll);
            this.Controls.Add(this.btnExitApp);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblSelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "CFIXER V2 by CEZEY";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSelection;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnExitApp;
        private System.Windows.Forms.Button btnApplyAll;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ProgressBar progressBarFixes;
        private System.Windows.Forms.Button btnContact;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

