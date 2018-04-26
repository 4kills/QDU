using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QuickDataUpload
{
    /// <summary>
    /// Klasse zum beeinflussen von System/OS Memory-Verteilung
    /// </summary>
    public static class MemoryManager
    {
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);
        
        // Einige Nutzer werden von zu viel ram-fressenden programmen abgeschreckt
        /// <summary>
        /// Minimiert die RAM-Belegung der App, indem ungenutzer RAM zurück
        /// an das Betriebssystem gegeben wird.
        /// </summary>
        public static void MinimizeFootprint()
        {
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
        }
    }
}
