using System.Net;
using QDU.Properties;

namespace QuickDataUpload
{
    /// <summary>
    /// structure representing the server
    /// </summary>
    public struct Host
    {
        public string DomainName;
        public string IPString;
        public int Port;
        public IPEndPoint IP;
    }
    
    /// <summary>
    /// class as pipeline for data, able to be used everywhere
    /// </summary>
    internal static class OptionsData
    {
        private static Host _Host;
        /// <summary>
        /// returns the up-to-date host by reading from settings
        /// </summary>
        public static Host MainHost
        {
            get
            {
                _Host.DomainName = Settings.Default.DomainName;
                _Host.IPString = Settings.Default.IPString;
                _Host.Port = Settings.Default.Port;
                bool URL = Settings.Default.URL;
                IPHostEntry IPHE = new IPHostEntry();
                _Host.IP = (URL) ? new IPEndPoint((IPHE = Dns.GetHostEntry(_Host.DomainName)).AddressList[0], _Host.Port) :
                    new IPEndPoint(IPAddress.Parse(_Host.IPString), _Host.Port);
                _Host.IPString = (URL) ? IPHE.ToString() : _Host.IPString;
                return _Host;
            }
        }
        
        public static bool RemLoc { get { return Settings.Default.RemLoc; } }

        public static string LastLoc
        {
            get { return Settings.Default.LastLoc; }

            set
            {
                Settings.Default.LastLoc = value;
                Settings.Default.Save(); 
            }
        }
        
        public static bool Online { get { return Settings.Default.Online; } }
     
        public static bool ToClipboard { get { return Settings.Default.ToClipboard; } }
     
        public static bool ToDisk { get { return Settings.Default.ToDisk; } }

        public static bool Autostart { get { return Settings.Default.Autostart; } }
    }
}
