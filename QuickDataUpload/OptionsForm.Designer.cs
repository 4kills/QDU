namespace QuickDataUpload
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tbConnection = new System.Windows.Forms.TextBox();
            this.rbURL = new System.Windows.Forms.RadioButton();
            this.rbIPv4 = new System.Windows.Forms.RadioButton();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbClipboard = new System.Windows.Forms.RadioButton();
            this.rbDiskSave = new System.Windows.Forms.RadioButton();
            this.rbOnline = new System.Windows.Forms.RadioButton();
            this.chbAutostart = new System.Windows.Forms.CheckBox();
            this.btSave = new System.Windows.Forms.Button();
            this.btKeybinds = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbToken = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbConnection
            // 
            this.tbConnection.Location = new System.Drawing.Point(12, 24);
            this.tbConnection.Name = "tbConnection";
            this.tbConnection.Size = new System.Drawing.Size(177, 20);
            this.tbConnection.TabIndex = 0;
            // 
            // rbURL
            // 
            this.rbURL.AutoSize = true;
            this.rbURL.Location = new System.Drawing.Point(207, 12);
            this.rbURL.Name = "rbURL";
            this.rbURL.Size = new System.Drawing.Size(47, 17);
            this.rbURL.TabIndex = 1;
            this.rbURL.TabStop = true;
            this.rbURL.Text = "URL";
            this.rbURL.UseVisualStyleBackColor = true;
            // 
            // rbIPv4
            // 
            this.rbIPv4.AutoSize = true;
            this.rbIPv4.Location = new System.Drawing.Point(207, 35);
            this.rbIPv4.Name = "rbIPv4";
            this.rbIPv4.Size = new System.Drawing.Size(47, 17);
            this.rbIPv4.TabIndex = 2;
            this.rbIPv4.TabStop = true;
            this.rbIPv4.Text = "IPv4";
            this.rbIPv4.UseVisualStyleBackColor = true;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(45, 50);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(59, 20);
            this.tbPort.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbClipboard);
            this.groupBox1.Controls.Add(this.rbDiskSave);
            this.groupBox1.Controls.Add(this.rbOnline);
            this.groupBox1.Location = new System.Drawing.Point(12, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 96);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode";
            // 
            // rbClipboard
            // 
            this.rbClipboard.AutoSize = true;
            this.rbClipboard.Location = new System.Drawing.Point(6, 65);
            this.rbClipboard.Name = "rbClipboard";
            this.rbClipboard.Size = new System.Drawing.Size(109, 17);
            this.rbClipboard.TabIndex = 2;
            this.rbClipboard.TabStop = true;
            this.rbClipboard.Text = "Save to Clipboard";
            this.rbClipboard.UseVisualStyleBackColor = true;
            // 
            // rbDiskSave
            // 
            this.rbDiskSave.AutoSize = true;
            this.rbDiskSave.Location = new System.Drawing.Point(6, 42);
            this.rbDiskSave.Name = "rbDiskSave";
            this.rbDiskSave.Size = new System.Drawing.Size(86, 17);
            this.rbDiskSave.TabIndex = 1;
            this.rbDiskSave.TabStop = true;
            this.rbDiskSave.Text = "Save to Disk";
            this.rbDiskSave.UseVisualStyleBackColor = true;
            // 
            // rbOnline
            // 
            this.rbOnline.AutoSize = true;
            this.rbOnline.Location = new System.Drawing.Point(6, 19);
            this.rbOnline.Name = "rbOnline";
            this.rbOnline.Size = new System.Drawing.Size(55, 17);
            this.rbOnline.TabIndex = 0;
            this.rbOnline.TabStop = true;
            this.rbOnline.Text = "Online";
            this.rbOnline.UseVisualStyleBackColor = true;
            // 
            // chbAutostart
            // 
            this.chbAutostart.AutoSize = true;
            this.chbAutostart.Location = new System.Drawing.Point(12, 251);
            this.chbAutostart.Name = "chbAutostart";
            this.chbAutostart.Size = new System.Drawing.Size(117, 17);
            this.chbAutostart.TabIndex = 5;
            this.chbAutostart.Text = "Start with Windows";
            this.chbAutostart.UseVisualStyleBackColor = true;
            this.chbAutostart.Click += new System.EventHandler(this.chbAutostart_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(197, 247);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 6;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btKeybinds
            // 
            this.btKeybinds.Location = new System.Drawing.Point(197, 72);
            this.btKeybinds.Name = "btKeybinds";
            this.btKeybinds.Size = new System.Drawing.Size(75, 23);
            this.btKeybinds.TabIndex = 7;
            this.btKeybinds.Text = "Keybinds";
            this.btKeybinds.UseVisualStyleBackColor = true;
            this.btKeybinds.Click += new System.EventHandler(this.btKeybinds_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Account Token:";
            // 
            // lbToken
            // 
            this.lbToken.AutoSize = true;
            this.lbToken.Location = new System.Drawing.Point(102, 220);
            this.lbToken.Name = "lbToken";
            this.lbToken.Size = new System.Drawing.Size(116, 13);
            this.lbToken.TabIndex = 10;
            this.lbToken.Text = "Sets when using online";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 280);
            this.Controls.Add(this.lbToken);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btKeybinds);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.chbAutostart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.rbIPv4);
            this.Controls.Add(this.rbURL);
            this.Controls.Add(this.tbConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.RadioButton rbURL;
        public System.Windows.Forms.RadioButton rbIPv4;
        public System.Windows.Forms.TextBox tbConnection;
        public System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbClipboard;
        private System.Windows.Forms.RadioButton rbDiskSave;
        private System.Windows.Forms.RadioButton rbOnline;
        private System.Windows.Forms.CheckBox chbAutostart;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btKeybinds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbToken;
    }
}