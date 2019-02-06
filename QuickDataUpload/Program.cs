using System;
using System.Windows.Forms;

namespace QuickDataUpload
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // starts main program
            MainForm optForm = new MainForm();
            icon = optForm.TrayIcon; 
            Application.Run();
        }
        public static NotifyIcon icon; 
    }
}
