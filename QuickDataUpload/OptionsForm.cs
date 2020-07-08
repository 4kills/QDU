using System;
using System.Windows.Forms;
using System.Net;
using QDU.Properties;
using Microsoft.Win32;

namespace QuickDataUpload
{
    
    public partial class OptionsForm : Form
    {
        /// <summary>
        /// keybind form passed by mainform
        /// </summary>
        private KeybindForm keyF;

        /// <summary>
        /// form shows itself
        /// </summary>
        /// <param name="pKeyF">pointer to Keybindform</param>
        public OptionsForm(KeybindForm pKeyF)
        {
            keyF = pKeyF; 
            InitializeComponent();

            this.Show();

            LoadSettings();
        }

        /// <summary>
        /// loads settings to ui for the user to see own settings
        /// </summary>
        private void LoadSettings()
        {
            cb_remember.Checked = Settings.Default.RemLoc; 
            rbURL.Checked = Settings.Default.URL;
            rbIPv4.Checked = !Settings.Default.URL;
            rbOnline.Checked = Settings.Default.Online;
            rbClipboard.Checked = Settings.Default.ToClipboard;
            rbDiskSave.Checked = Settings.Default.ToDisk;
            chbAutostart.Checked = Settings.Default.Autostart;
            tbConnection.Text = (Settings.Default.URL) ? Settings.Default.DomainName :
                Settings.Default.IPString;
            tbPort.Text = Settings.Default.Port.ToString();
            if (Settings.Default.Token != "null") lbToken.Text = Settings.Default.Token;
            else lbToken.Text = "is set when using 'online'";
        }

        /// <summary>
        /// saves the adjusted settings to data layer
        /// </summary>
        private void SaveToSettings()
        {
            if (rbURL.Checked)
            {
                Settings.Default.DomainName = tbConnection.Text;
                try { Settings.Default.IPString = Dns.GetHostEntry(tbConnection.Text).ToString(); }
                catch { tbConnection.Text = "No net connection"; }
            }
            else
            {
                Settings.Default.DomainName = "";
                Settings.Default.IPString = tbConnection.Text;
            }
            Settings.Default.Port = Convert.ToInt32(tbPort.Text);
            Settings.Default.URL = rbURL.Checked;
            Settings.Default.Online = rbOnline.Checked;
            Settings.Default.ToClipboard = rbClipboard.Checked;
            Settings.Default.ToDisk = rbDiskSave.Checked;
            Settings.Default.Autostart = chbAutostart.Checked;
            Settings.Default.RemLoc = cb_remember.Checked;

            Settings.Default.Save();
        }

        /// <summary>
        /// when checking this option the app is written to or 
        /// deleted from the autostart in registry 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbAutostart_Click(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (chbAutostart.Checked)
            {
                rk.SetValue("QuickDataUpload", Application.ExecutablePath);
            }
            else
            {
                rk.DeleteValue("QuickDataUpload", false);
            }
        }

        /// <summary>
        /// saves settings, closes and disposes form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            SaveToSettings();
            this.Close();
            this.Dispose();
            MemoryManager.MinimizeFootprint();
        }

        /// <summary>
        /// shows the keybinds form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btKeybinds_Click(object sender, EventArgs e)
        {
            keyF.Show(); 
        }
    }
}
