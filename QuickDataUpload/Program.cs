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

            // setzt programm in gang
            MainForm optForm = new MainForm(); 
            Application.Run();
        }
    }
}
