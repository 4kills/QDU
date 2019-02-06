using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickDataUpload
{
    /// <summary>
    /// class for setting global hotkeys
    /// </summary>
    public partial class KeybindForm : Form
    {
        #region Attribute

        /// <summary>
        /// list for managing all hotkeys
        /// </summary>
        private List<Hotkey> hkList = new List<Hotkey>();

        /// <summary>
        /// array containing all methods for the hotkeys
        /// </summary>
        private HotkeyHandler[] HKHandlers; 

        /// <summary>
        /// passed keyhook 
        /// </summary>
        private KeyboardHook keyHook;

        #endregion 

        /// <summary>
        /// constructor, setting attributes, creating hotkeys 
        /// registering global hotkeys with the OS 
        /// </summary>
        /// <param name="keyHook"></param>
        /// <param name="HKHs">method with HotkeyHandler signature</param>
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
        /// register all active hotkeys with windows
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
        /// saves the settings
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
        /// overriden so settings arent lost when pressing x 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
