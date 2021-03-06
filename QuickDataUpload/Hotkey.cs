﻿using System;
using System.Text;
using System.Windows.Forms;
using QDU.Properties;

namespace QuickDataUpload
{
    /// <summary>
    /// class that represents a key combination which calls a given method
    /// </summary>
    internal sealed class Hotkey
    {
        #region Attribute
        
        /// <summary>
        /// counts the amount of hotkey instances
        /// </summary>
        private static int ID;
        /// <summary>
        /// id of the instance
        /// </summary>
        public int id { get; private set; }
        /// <summary>
        /// checkbox of the hotkey
        /// </summary>
        private CheckBox cb;
        /// <summary>
        /// modifier button 1
        /// </summary>
        private Button btMod1;
        /// <summary>
        /// modifier button 1
        /// </summary>
        private Button btMod2;
        /// <summary>
        /// hotkey 
        /// </summary>
        private Button btKey;
        /// <summary>
        /// modifier 1, which has to be pressed
        /// </summary>
        private uint mod1;
        /// <summary>
        /// modifier 2, whichs has to be pressed
        /// </summary>
        private uint mod2;
        /// <summary>
        /// key that has to be pressed
        /// </summary>
        private uint key;
        /// <summary>
        /// saves whether the hotkey can be executed
        /// </summary>
        public bool active { get; private set; }
        
        #endregion 

        /// <summary>
        /// creates hotkey with its windows.forms.controls
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="mod1"></param>
        /// <param name="mod2"></param>
        /// <param name="key"></param>
        /// <param name="f"></param>
        public Hotkey(CheckBox cb, Button mod1, Button mod2, Button key, KeybindForm f)
        {
            id = ID++; // sets id and increments instances
            this.cb = cb; btMod1 = mod1; btMod2 = mod2; btKey = key;

            // event for changing checkbox
            this.cb.CheckedChanged += (s, e) => active = this.cb.Checked;
            
            //Sets events for button clicks
            btMod1.Click += HandleButton;
            btMod2.Click += HandleButton;
            btKey.Click += HandleButton;
            
            //Event for setting the buttons
            f.KeyDown += HandleKeypress;
            LoadSettings();
            SyncUI();
        }

        /// <summary>
        /// registers the key with windows
        /// </summary>
        /// <param name="kh">the windows keyboard hook</param>
        /// <param name="hkh">the method to be called</param>
        public void Register (KeyboardHook kh, HotkeyHandler hkh)
        {
            kh.RegisterHotKey(mod1, mod2, key, hkh); 
        }

        private void HandleKeypress(object s, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (btMod1.Text == "Press Key") mod1 = CheckMod(e);
            if (btMod2.Text == "Press Key") mod2 = CheckMod(e);
            if (btKey.Text == "Press Key")
            {
                key = (uint)e.KeyValue;
                if (e.KeyValue == (int)Keys.Escape) key = 0;
            }
            SyncUI();
        }

        /// <summary>
        /// checks for pressed modifier (ctrl, alt, shift)
        /// </summary>
        /// <param name="e"></param>
        /// <returns>returns zero w/o modifier</returns>
        private uint CheckMod (KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Escape) return 0; 
            else if (e.Shift) return (uint)ModifiersKeys.Shift;
            else if (e.Alt) return (uint)ModifiersKeys.Alt;
            else if (e.Control) return (uint)ModifiersKeys.Control;
            else return 0; 
        }

        /// <summary>
        /// sets bvutton to listening state conceiveable by the user 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void HandleButton(object s, EventArgs e) => ((Button)s).Text = "Press Key";

        /// <summary>
        /// synchronizes with hotkey
        /// </summary>
        private void SyncUI()
        {
            cb.Checked = active;
            btMod1.Text = ((ModifiersKeys)mod1).ToString();
            btMod2.Text = ((ModifiersKeys)mod2).ToString();
            btKey.Text = ((Keys)key).ToString();
        }

        /// <summary>
        /// saves combination to settings
        /// </summary>
        public void SaveSettings()
        {
            switch (id)
            {
                case 0: Settings.Default.hk1 = ToSettings(); break;
                case 1: Settings.Default.hk2 = ToSettings(); break;
                case 2: Settings.Default.hk3 = ToSettings(); break;
                case 3: Settings.Default.hk4 = ToSettings(); break;
                case 4: Settings.Default.hk5 = ToSettings(); break;
            }
            Settings.Default.Save();
        }

        /// <summary>
        /// encodes combination as string
        /// </summary>
        /// <returns>Hotkey as string for settings</returns>
        private string ToSettings()
        {
            var strB = new StringBuilder("" + mod1);
            strB.Append(";" + mod2);
            strB.Append(";" + key);
            strB.Append(":" + active);
            return strB.ToString();  
        }

        /// <summary>
        /// loads combination from settings into hotkey
        /// </summary>
        private void LoadSettings()
        {
            switch (id)
            {
                case 0: FromSettings(Settings.Default.hk1); break;
                case 1: FromSettings(Settings.Default.hk2); break;
                case 2: FromSettings(Settings.Default.hk3); break;
                case 3: FromSettings(Settings.Default.hk4); break;
                case 4: FromSettings(Settings.Default.hk5); break;
            }
        }

        /// <summary>
        /// decodes string representing hotkey
        /// </summary>
        /// <param name="s"></param>
        private void FromSettings(string s)
        {
            // example string in settings: "34;23;12:True"
            mod1 = Convert.ToUInt32(s.Substring(0, s.IndexOf(";")));
            mod2 = Convert.ToUInt32(s.Substring(s.IndexOf(";")+1, s.LastIndexOf(";")-s.IndexOf(";")-1));
            key = Convert.ToUInt32(s.Substring(s.LastIndexOf(";")+1, s.IndexOf(":")-s.LastIndexOf(";")-1));
            active = Convert.ToBoolean(s.Substring(s.IndexOf(":")+1));
        }
    }
}
