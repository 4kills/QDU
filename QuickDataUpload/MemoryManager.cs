using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QuickDataUpload
{
    /// <summary>
    /// class to influence System/OS Memory-distribution
    /// </summary>
    public static class MemoryManager
    {
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);
        
        // some users dislike programs apparently using too much memory
        /// <summary>
        /// minimizes RAM-usage of the app by returning unused ram to the OS
        /// </summary>
        public static void MinimizeFootprint()
        {
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
        }
    }
}
