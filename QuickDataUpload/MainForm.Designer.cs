namespace QuickDataUpload
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.FullMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.AreaMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipTitle = "QuickUpload";
            this.TrayIcon.ContextMenuStrip = this.TrayContextMenuStrip;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "QuickUpload";
            this.TrayIcon.Visible = true;
            // 
            // TrayContextMenuStrip
            // 
            this.TrayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FullMenuStrip,
            this.AreaMenuStrip,
            this.OptionsMenuStrip,
            this.ExitMenuStrip});
            this.TrayContextMenuStrip.Name = "TrayContextMenuStrip";
            this.TrayContextMenuStrip.Size = new System.Drawing.Size(144, 92);
            this.TrayContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.TrayContextMenuStrip_ItemClicked);
            // 
            // FullMenuStrip
            // 
            this.FullMenuStrip.Name = "FullMenuStrip";
            this.FullMenuStrip.Size = new System.Drawing.Size(143, 22);
            this.FullMenuStrip.Text = "Capture All";
            // 
            // AreaMenuStrip
            // 
            this.AreaMenuStrip.Name = "AreaMenuStrip";
            this.AreaMenuStrip.Size = new System.Drawing.Size(143, 22);
            this.AreaMenuStrip.Text = "Capture Area";
            // 
            // OptionsMenuStrip
            // 
            this.OptionsMenuStrip.Name = "OptionsMenuStrip";
            this.OptionsMenuStrip.Size = new System.Drawing.Size(143, 22);
            this.OptionsMenuStrip.Text = "Options";
            // 
            // ExitMenuStrip
            // 
            this.ExitMenuStrip.Name = "ExitMenuStrip";
            this.ExitMenuStrip.Size = new System.Drawing.Size(143, 22);
            this.ExitMenuStrip.Text = "Exit";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Options";
            this.TrayContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem OptionsMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem AreaMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FullMenuStrip;
    }
}

