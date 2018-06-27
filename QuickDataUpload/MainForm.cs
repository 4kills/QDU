using System;
using System.Windows.Forms;
using QDU.Properties;
using System.Reflection;

namespace QuickDataUpload
{
    /// <summary>
    /// Haupt-Windows-Form, die das Tray Icon stellt und code verwaltet -> "Steuerung"
    /// </summary>
    public partial class MainForm : Form
    {
        #region Attribute
        /// <summary>
        /// user32.dll windows keyboard api. Wird benutzt für globale Tastenkombis,
        /// die in jedem Programm aktivieren. 
        /// </summary>
        private KeyboardHook globalHotKeyHook = new KeyboardHook(); //System API win hooks
        /// <summary>
        /// Option-Windows-Form zum Einstellen von verschiedenen Preferenzen 
        /// </summary>
        private OptionsForm oF;
        /// <summary>
        /// Windows-Form zum Auswählen von globalen tastenkombinationen
        /// </summary>
        private KeybindForm keyF;
        /// <summary>
        /// Polymorphe Camera-Klasse zum Aufnehmen von versch. Bildschirmteilen
        /// </summary>
        private Camera cam;
        #endregion

        /// <summary>
        /// Konstruktor, erzeugt tray-icon und neue keybind-form welche die tastenkombis
        /// bei Programmstart im OS registriert
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
        /// Bei click auf ein item des tray icons. Öffnet Optionen, verlässt Programm,
        /// erzeugt neue AreaCamera oder neue FullCamera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Info zu geclicktem Item</param>
        private void TrayContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == OptionsMenuStrip) oF = new OptionsForm(keyF);//opens options 

            else if (e.ClickedItem == ExitMenuStrip)
            {
                TrayIcon.Visible = false; Environment.Exit(0); // exits program
            }

            else if (e.ClickedItem == AreaMenuStrip) cam = new AreaCamera();//captures area

            else if (e.ClickedItem == FullMenuStrip) cam = new FullCamera();//caputres all
        }

        #region Methoden, die bei jeweiliger Tastenkombination ausgeführt werden

        /// <summary>
        /// Methode für Tastenkombi.
        /// Überprüft, ob bereits eine camera fotografiert, falls nicht,
        /// erzeugt neue areacamera die ein Bildschirmfoto macht
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void AreaHKHandler(object s, KeyPressedEventArgs e)
        {
            if (!cam?.ProcedureDone ?? false) return;

            cam = new AreaCamera();
        }

        /// <summary>
        /// Methode für Tastenkombi.
        /// Überprüft, ob bereits eine camera fotografiert, falls nicht,
        /// erzeugt neue fullcamera die ein Bildschirmfoto macht 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void FullHKHandler(object s, KeyPressedEventArgs e)
        {
            if (!cam?.ProcedureDone ?? false) return;
            cam = new FullCamera(); 
        }

        /// <summary>
        /// Methode für Tastenkombi.
        /// Wechselt modus zu "online"
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnlineHKHandler(object s, KeyPressedEventArgs e)
        {
            Settings.Default.Online = true; Settings.Default.ToClipboard = false;
            Settings.Default.ToDisk = false; Settings.Default.Save();
        }

        /// <summary>
        /// Methode für Tastenkombi.
        /// Wechselt modus zu "save to disk"
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void DiskHKHandler(object s, KeyPressedEventArgs e)
        {
            Settings.Default.Online = false; Settings.Default.ToClipboard = false;
            Settings.Default.ToDisk = true; Settings.Default.Save();
        }

        /// <summary>
        /// Methode für Tastenkombi.
        /// Wechselt modus zu "Clipboard"
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
