using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickDataUpload
{
    /// <summary>
    /// Klasse zum festlegen der globalen Tastenkombinationen
    /// </summary>
    public partial class KeybindForm : Form
    {
        #region Attribute

        /// <summary>
        /// Liste zum gemeinsamen verwalten aller Hotkey
        /// </summary>
        private List<Hotkey> hkList = new List<Hotkey>();

        /// <summary>
        /// Array, welches alle Methoden für die Hotkeys beinhaltet
        /// (übergeben durch den Konstruktor von der Haupt(Main)-Form)
        /// </summary>
        private HotkeyHandler[] HKHandlers; 

        /// <summary>
        /// Übergebener Keyhook (wird übergeben von Main-Form) 
        /// </summary>
        private KeyboardHook keyHook;

        #endregion 

        /// <summary>
        /// Konstruktor, der die Attribute setzt, die Hotkeys erzeugt 
        /// und die globalen Tastenkombis beim OS registriert
        /// </summary>
        /// <param name="keyHook">Main-Form KeyboardHook übergeben</param>
        /// <param name="HKHs">Methoden mit HotkeyHandler signatur übergeben</param>
        public KeybindForm(KeyboardHook keyHook, params HotkeyHandler[] HKHs)
        {
            this.keyHook = keyHook;
            this.HKHandlers = HKHs;
            InitializeComponent();
            hkList.Add(new Hotkey(cbFullCam, button1, button2, button3, this));
            hkList.Add(new Hotkey(cbAreaCam, button4, button5, button6, this));
            hkList.Add(new Hotkey(cbOnline, button7, button8, button9, this));
            hkList.Add(new Hotkey(cbDisk, button10, button11, button12, this));
            hkList.Add(new Hotkey(cbClipboard, button13, button14, button15, this));
            SetHotkeys();
        }
        
        /// <summary>
        /// Registriert alle (aktiven) Tastenkombis beim OS. 
        /// </summary>
        private void SetHotkeys()
        {
            keyHook.UnregisterAll();
            for (int i = 0; i < hkList.Count; i++)
            {
                if (!hkList[i].active) continue;
                hkList[i].Register(keyHook, HKHandlers[i]);
            }
            MemoryManager.MinimizeFootprint();
        }

        /// <summary>
        /// Speichert die Settings bei click auf den "Save"-bt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            foreach(var hk in hkList)
            {
                hk.SaveSettings();
            }
            this.Hide();
            SetHotkeys();
        }

        /// <summary>
        /// Wird überschrieben damit die Form nicht verworfen wird beim
        /// schließen mit x-button
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
