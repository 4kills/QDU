using System;
using System.Text;
using System.Windows.Forms;
using QDU.Properties;

namespace QuickDataUpload
{
    /// <summary>
    /// Klasse, die eine Tastenkombination repräsentiert, mit der eine bestimmte
    /// Methode aufgerufen werden kann.
    /// </summary>
    internal sealed class Hotkey
    {
        #region Attribute
        
        /// <summary>
        /// zählt die anzahl der Hotkey-Instanzen
        /// </summary>
        private static int ID;
        /// <summary>
        /// id der jeweiligen Hotkey-Instanz
        /// </summary>
        public int id { get; private set; }
        /// <summary>
        /// checkbox des hotkeys
        /// </summary>
        private CheckBox cb;
        /// <summary>
        /// modifier-button 1 des hotkeys
        /// </summary>
        private Button btMod1;
        /// <summary>
        /// modifier-button 2 des hotkeys
        /// </summary>
        private Button btMod2;
        /// <summary>
        /// button der eigentlichen Taste
        /// </summary>
        private Button btKey;
        /// <summary>
        /// modifier 1, der gedrückt werden muss
        /// </summary>
        private uint mod1;
        /// <summary>
        /// modifier 2, der gedrückt werden muss
        /// </summary>
        private uint mod2;
        /// <summary>
        /// Taste, die gedrückt werden muss
        /// </summary>
        private uint key;
        /// <summary>
        /// Speichert, ob die Tastenkombination ausgeführt werden kann
        /// </summary>
        public bool active { get; private set; }
        
        #endregion 

        /// <summary>
        /// Erstellt den Hotkey mit den dazugehörigen windows.forms.controls
        /// und der übergeordneten form
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="mod1"></param>
        /// <param name="mod2"></param>
        /// <param name="key"></param>
        /// <param name="f"></param>
        public Hotkey(CheckBox cb, Button mod1, Button mod2, Button key, KeybindForm f)
        {
            id = ID++; // setzt id und zählt dann instanzen hoch
            this.cb = cb; btMod1 = mod1; btMod2 = mod2; btKey = key;

            // event für ändern der checkbox
            this.cb.CheckedChanged += (s, e) => active = this.cb.Checked;
            
            //Setzt events für buttonclicks
            btMod1.Click += HandleButton;
            btMod2.Click += HandleButton;
            btKey.Click += HandleButton;
            
            //Event für das setzten der Tasten
            f.KeyDown += HandleKeypress;
            LoadSettings();
            SyncUI();
        }

        /// <summary>
        /// Registriert die Tastenkombi in Windows
        /// </summary>
        /// <param name="kh">Der windows keyboard hook</param>
        /// <param name="hkh">Die Methode die ausgeführt werden soll</param>
        public void Register (KeyboardHook kh, HotkeyHandler hkh)
        {
            kh.RegisterHotKey(mod1, mod2, key, hkh); 
        }

        /// <summary>
        /// Wenn eine Taste gedrückt wird erhält jeder button, der auf eine Taste
        /// wartet die jeweilig gedrückte Taste zugewiesen. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e">Info über die gedrückte Taste</param>
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
        /// Überprüft welcher modifier (ctrl, alt, shift) gedrückt wurde
        /// </summary>
        /// <param name="e"></param>
        /// <returns>returned 0 wenn kein modifier gedrückt wurde</returns>
        private uint CheckMod (KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Escape) return 0; 
            else if (e.Shift) return (uint)ModifiersKeys.Shift;
            else if (e.Alt) return (uint)ModifiersKeys.Alt;
            else if (e.Control) return (uint)ModifiersKeys.Control;
            else return 0; 
        }

        /// <summary>
        /// Wenn ein button gedrückt wird, dann zeigt er den text "Press Key".
        /// Er kann nun eine Taste empfangen
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void HandleButton(object s, EventArgs e) => ((Button)s).Text = "Press Key";

        /// <summary>
        /// Synchronisiert die windows.forms.controls-Texte mit den attributen des hotkeys
        /// </summary>
        private void SyncUI()
        {
            cb.Checked = active;
            btMod1.Text = ((ModifiersKeys)mod1).ToString();
            btMod2.Text = ((ModifiersKeys)mod2).ToString();
            btKey.Text = ((Keys)key).ToString();
        }

        /// <summary>
        /// Speichert Tastenkombination in das dafür vorgesehene setting
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
        /// Kodiert die Tastenkombination des Hotkeys als string 
        /// </summary>
        /// <returns>Hotkey als string für settings</returns>
        private string ToSettings()
        {
            var strB = new StringBuilder("" + mod1);
            strB.Append(";" + mod2);
            strB.Append(";" + key);
            strB.Append(":" + active);
            return strB.ToString();  
        }

        /// <summary>
        /// Läd die Tastenkombination in den Hotkey aus dem Setting
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
        /// Dekodiert den string, der die Tastenkombination repräsentiert und
        /// setzt die Attribute
        /// </summary>
        /// <param name="s"></param>
        private void FromSettings(string s)
        {
            // Beispielsettingstring: "34;23;12:True"
            mod1 = Convert.ToUInt32(s.Substring(0, s.IndexOf(";")));
            mod2 = Convert.ToUInt32(s.Substring(s.IndexOf(";")+1, s.LastIndexOf(";")-s.IndexOf(";")-1));
            key = Convert.ToUInt32(s.Substring(s.LastIndexOf(";")+1, s.IndexOf(":")-s.LastIndexOf(";")-1));
            active = Convert.ToBoolean(s.Substring(s.IndexOf(":")+1));
        }
    }
}
