using System;
using System.Windows.Forms;
using QDU.Properties;
using System.Reflection;

namespace QuickDataUpload
{
    /// <summary>
    /// main window with the tray icon and control class
    /// </summary>
    public partial class MainForm : Form
    {
        #region Attribute
        /// <summary>
        /// user32.dll windows keyboard api. used for global hotkeys
        /// </summary>
        private KeyboardHook globalHotKeyHook = new KeyboardHook(); //System API win hooks
        /// <summary>
        /// Option-Windows-Form for preferences
        /// </summary>
        private OptionsForm oF;
        /// <summary>
        /// Windows-Form to create global hotkeys
        /// </summary>
        private KeybindForm keyF;
        /// <summary>
        /// Polymorph camera class for different snap modes 
        /// </summary>
        private Camera cam;
        #endregion

        /// <summary>
        /// constructor, creates tray-icon and new keybind-form which registers glob hotkeys
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            TrayIcon.Visible = true; // shows icon as tray

            keyF = new KeybindForm(globalHotKeyHook, FullHKHandler, AreaHKHandler, 
                OnlineHKHandler, DiskHKHandler, ClipboardHKHandler); 
            
            MemoryManager.MinimizeFootprint(); //decreases ram usage
        }

        /// <summary>
        /// triggers when tray icon is clicked. shows controls and triggers a sceen capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">info for pressed items</param>
        private void TrayContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem == AreaMenuStrip) cam = new AreaCamera();//captures area

            else if (e.ClickedItem == FullMenuStrip) cam = new FullCamera();//caputres all

            else if (e.ClickedItem == ViewImgsMenuStrip) ViewImagesOnline();

            else if (e.ClickedItem == OptionsMenuStrip) oF = new OptionsForm(keyF);//opens options 

            else if (e.ClickedItem == ExitMenuStrip)
            {
                TrayIcon.Visible = false; Environment.Exit(0); // exits program
            }
        }

        private void ViewImagesOnline()
        {
            if (Settings.Default.Token == "null" || Settings.Default.Token == "") return;
            try
            {
                System.Diagnostics.Process.Start($"http://{OptionsData.MainHost.DomainName}/?me={Settings.Default.Token}");
            }
            catch { }
        }

        #region Methods being triggered for specific hot keys

        /// <summary>
        /// Method for hotkey.
        /// checks whether a snap is being taken, if not tries to take one for an area
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void AreaHKHandler(object s, KeyPressedEventArgs e)
        {
            if (!cam?.ProcedureDone ?? false) return;

            cam = new AreaCamera();
        }

        /// <summary>
        /// Method for hotkey.
        /// checks whether a snap is being taken, if not tries to take one for all screens
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void FullHKHandler(object s, KeyPressedEventArgs e)
        {
            if (!cam?.ProcedureDone ?? false) return;
            cam = new FullCamera(); 
        }

        /// <summary>
        /// Method for hotkey.
        /// changes mode to online
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnlineHKHandler(object s, KeyPressedEventArgs e)
        {
            Settings.Default.Online = true; Settings.Default.ToClipboard = false;
            Settings.Default.ToDisk = false; Settings.Default.Save();
        }

        /// <summary>
        /// Method for hotkey.
        /// changes mode to save to disk
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void DiskHKHandler(object s, KeyPressedEventArgs e)
        {
            Settings.Default.Online = false; Settings.Default.ToClipboard = false;
            Settings.Default.ToDisk = true; Settings.Default.Save();
        }

        /// <summary>
        /// Method for hotkey.
        /// changes mode to clipboard
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void ClipboardHKHandler(object s, KeyPressedEventArgs e)
        {
            Settings.Default.Online = false; Settings.Default.ToClipboard = true;
            Settings.Default.ToDisk = false; Settings.Default.Save();
        }
        #endregion

        private void TrayIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(TrayIcon, null);
            }
        }
    }
}
