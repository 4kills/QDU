using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuickDataUpload
{
    /// <summary>
    /// Passed delegate at hotkey creation
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e"></param>
    public delegate void HotkeyHandler(object s, KeyPressedEventArgs e);

    /// <summary>
    /// user32.dll windows keyboard api. Used for global hotkeys.
    /// </summary>
    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        #region Interne-Window-Klasse
        /// <summary>
        ///Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;
            private Dictionary<int, HotkeyHandler> HotKeys;
            public Window(Dictionary<int, HotkeyHandler> hotKeys)
            {
                HotKeys = hotKeys;
                // creates the handle for the window
                this.CreateHandle(new CreateParams());
                
            }

            /// <summary>
            /// overriden to receive message. 
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                
                foreach (var hotKey in HotKeys)
                {
                    if(m.Msg == WM_HOTKEY && (int)m.WParam == hotKey.Key)
                    {
                        //gets keys
                        Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                        ModifiersKeys modifier = (ModifiersKeys)((int)m.LParam & 0xFFFF);

                        //invokes events
                        hotKey.Value(this, new KeyPressedEventArgs(modifier, key));
                    }
                }
                
            }

            /// <summary>
            /// IDisposable method, destroys the window handle
            /// </summary>
            public void Dispose()
            {
                this.DestroyHandle();
            }

        }
        #endregion

        #region Attribute
        /// <summary>
        /// window for the key hook
        /// </summary>
        private Window _window;
        /// <summary>
        /// current, unique id for the winhotkey
        /// </summary>
        private static int _currentId;
        /// <summary>
        /// dictionary, whisch associates a given method with the hotkey id
        /// </summary>
        private Dictionary<int, HotkeyHandler> HotKeys;
        #endregion 

        /// <summary>
        /// registers the inner native window
        /// </summary>
        public KeyboardHook()
        {
            HotKeys = new Dictionary<int, HotkeyHandler>();
            _window = new Window(HotKeys);
        }
       
        /// <summary>
        /// registers a hotkey with windows
        /// </summary>
        /// <param name="modifier">modifier for the WinHotkey</param>
        /// <param name="key">the actual key </param>
        public void RegisterHotKey(ModifiersKeys modifier, Keys key, HotkeyHandler method)
        {
            _currentId++;
            
            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
            HotKeys.Add(_currentId, method); 
        }

        #region RegisterHotKey(...) überladungen
        /// <summary>
        /// registers a Hotkey with 2 modifier keys
        /// </summary>
        /// <param name="modifier1"></param>
        /// <param name="modifier2"></param>
        /// <param name="key"></param>
        /// <param name="method"></param>
        public void RegisterHotKey(ModifiersKeys modifier1, ModifiersKeys modifier2, Keys key,
            HotkeyHandler method)
        {
            RegisterHotKey(modifier1 | modifier2, key, method);
        }

        /// <summary>
        /// registers a Hotkey through uint-representation
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="key"></param>
        /// <param name="method"></param>
        public void RegisterHotKey(uint modifier, uint key, HotkeyHandler method)
        {
            // increment the counter.
            _currentId++;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, modifier, key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
            HotKeys.Add(_currentId, method);
        }

        /// <summary>
        /// registers a Hotkey with 2 modifier keys, through
        /// uint-Representation
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="modifier2"></param>
        /// <param name="key"></param>
        /// <param name="method"></param>
        public void RegisterHotKey(uint modifier, uint modifier2, uint key, HotkeyHandler method)
        {
            RegisterHotKey(modifier | modifier2, key, method); 
        }

        #endregion 

        /// <summary>
        /// unregisters all win hotkeys and resets unique ID 
        /// </summary>
        public void UnregisterAll()
        {
            foreach (var hotKey in HotKeys)
            {
                UnregisterHotKey(_window.Handle, hotKey.Key);
            }
            HotKeys.Clear();
            _currentId = 0; 
        }

        /// <summary>
        /// calls "UnregisterAll()" and disposes internal native window
        /// </summary>
        public void Dispose()
        {
            // unregister all the registered hot keys.
            UnregisterAll();

            // dispose the inner native window.
            _window.Dispose();
        }
    }

    /// <summary>
    /// EventArgs which are passed after hotkey is pressed
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Modifier-key 2
        /// </summary>
        private ModifiersKeys _modifier2; 
        /// <summary>
        /// Modifier-key 1
        /// </summary>
        private ModifiersKeys _modifier;
        /// <summary>
        /// actual key
        /// </summary>
        private Keys _key;

        /// <summary>
        /// constructor for one modifier
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="key"></param>
        internal KeyPressedEventArgs(ModifiersKeys modifier, Keys key)
        {
            _modifier = modifier;
            _modifier2 = ModifiersKeys.None;
            _key = key;
        }
        /// <summary>
        /// constructor for two modifiers
        /// </summary>
        /// <param name="modifier1"></param>
        /// <param name="modifier2"></param>
        /// <param name="key"></param>
        internal KeyPressedEventArgs(ModifiersKeys modifier1, ModifiersKeys modifier2, Keys key)
        {
            _modifier = modifier1;
            _modifier2 = modifier2;
            _key = key;
        }

        /// <summary>
        /// modifier 1 Property
        /// </summary>
        public ModifiersKeys Modifier
        {
            get { return _modifier; }
        }

        /// <summary>
        /// modifier 2 Property
        /// </summary>
        public ModifiersKeys Modifier2
        {
            get { return _modifier2; }
        }

        /// <summary>
        /// Key Property
        /// </summary>
        public Keys Key
        {
            get { return _key; }
        }
    }
    
    /// <summary>
    /// the enumeration of possible modifier keys
    /// </summary>
    [Flags]
    public enum ModifiersKeys : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
