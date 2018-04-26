using System.Net;
using QDU.Properties;

namespace QuickDataUpload
{
    /// <summary>
    /// Struktur welche den Server repräsentiert
    /// </summary>
    public struct Host
    {
        public string DomainName;
        public string IPString;
        public int Port;
        public IPEndPoint IP;
    }
    
    /// <summary>
    /// Klasse, die als Pipeline für verschieden gesetzte Optionen fungiert und diese überall
    /// zugänglich macht. 
    /// </summary>
    internal static class OptionsData
    {
        private static Host _Host;
        /// <summary>
        /// Gibt jeder Zeit den aktuellen Host zurück, mit allen nötigen Informationen.
        /// Diese werden direkt aus den Settings ausgelesen.
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
        
        /// <summary>
        /// Gibt zurück, ob das Bild im modus "online" gemacht wird.
        /// </summary>
        public static bool Online { get { return Settings.Default.Online; } }
        /// <summary>
        /// Gibt zurück, ob das Bild im modus "Clipboard" (Zwischenablage) gemacht wird.
        /// </summary>
        public static bool ToClipboard { get { return Settings.Default.ToClipboard; } }
        /// <summary>
        /// Gibt zurück, ob das Bild im modus "Speichern" gemacht wird.
        /// </summary>
        public static bool ToDisk { get { return Settings.Default.ToDisk; } }
        /// <summary>
        /// Gibt zurück, ob das Programm zusammen mit windows gestartet wird. 
        /// </summary>
        public static bool Autostart { get { return Settings.Default.Autostart; } }
    }
}
