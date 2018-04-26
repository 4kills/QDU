using System;
using System.Windows.Forms;
using System.Net;
using QDU.Properties;
using Microsoft.Win32;

namespace QuickDataUpload
{
    /// <summary>
    /// Win-Forms-Klasse zum festlegen der Präferenzen
    /// </summary>
    public partial class OptionsForm : Form
    {
        /// <summary>
        /// Von Main-Form übergebene KeybindForm
        /// </summary>
        private KeybindForm keyF;

        /// <summary>
        /// Konstruktor, KeybindForm wird übergeben
        /// und die Form zeigt sich selbst. 
        /// </summary>
        /// <param name="pKeyF">Referenz zur Keybindform</param>
        public OptionsForm(KeybindForm pKeyF)
        {
            keyF = pKeyF; 
            InitializeComponent();

            this.Show();

            LoadSettings();
        }

        /// <summary>
        /// Läd die Settings in die UI, damit der Benutzer sieht, was er eingestellt hat
        /// </summary>
        private void LoadSettings()
        {
            rbURL.Checked = Settings.Default.URL;
            rbIPv4.Checked = !Settings.Default.URL;
            rbOnline.Checked = Settings.Default.Online;
            rbClipboard.Checked = Settings.Default.ToClipboard;
            rbDiskSave.Checked = Settings.Default.ToDisk;
            chbAutostart.Checked = Settings.Default.Autostart;
            tbConnection.Text = (Settings.Default.URL) ? Settings.Default.DomainName :
                Settings.Default.IPString;
            tbPort.Text = Settings.Default.Port.ToString();
        }

        /// <summary>
        /// Speichert die angepassten Optionen in die Settings / Daten-Schicht
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

            Settings.Default.Save();
        }

        /// <summary>
        /// Bei Betätigen dieser Checkbox wird die App in das 
        /// Windows-Auto-Start-Verzeichnes in der Registry geschrieben
        /// oder gelöscht
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
        /// Speichert die Settings, schließt und verwirft die Option-Form
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
        /// Zeigt die Keybind-Form an 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btKeybinds_Click(object sender, EventArgs e)
        {
            keyF.Show(); 
        }
    }
}
