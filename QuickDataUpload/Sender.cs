using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace QuickDataUpload
{
    /// <summary>
    /// Statische Klasse die gesamtes networking beinhaltet; Bild an server schickt
    /// </summary>
    static class Sender
    {
        /// <summary>
        /// socket des Clients
        /// </summary>
        private static Socket socket;

        /// <summary>
        /// ein bool der indiziert, ob der Sendevorgang erfolgreich war
        /// </summary>
        private static bool success;

        /// <summary>
        /// Etabliert eine Verbindung mit dem Server(Durch optionen festgelegt)
        /// und sendet diesem das bild. Der Server stellt das bild online bereit 
        /// und gibt dem Client die URL für das Bild zurück, welche geteilt werden kann
        /// </summary>
        /// <returns>Gibt true zurück, wenn Vorgang erfolgreich abgeschlossen</returns>
        public static bool SendPic()
        {
            success = true;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { socket.Connect(OptionsData.MainHost.IP); } // 192.168.2.118 local pi 
            catch { return false; }
            byte[] buffer = ImageToByte(Camera.BmpSS);
            SendMetaData(buffer.Length);
            RecApproval();
            SendImage(buffer);
            ReceiveURL();
            socket.Close();
            return success;
        }

        /// <summary>
        /// Wartet, bis der server-socket dem Client mitteilt, dass er bereit ist für 
        /// weitere Daten. Teil des Übertragungsprotokolls
        /// </summary>
        private static void RecApproval()
        {
            byte[] buffer = new byte[1];
            int rec = 0;
            rec = socket.Receive(buffer, 0, buffer.Length, 0);
            if (rec < buffer.Length) success = false;
        }

        /// <summary>
        /// Teilt dem server-socket mit, dass der Client bereit ist für weitere Daten.
        /// Teil des Übertragungsprotokolls
        /// </summary>
        private static void SendApproval()
        {
            byte[] buffer = new byte[1] { 1 };
            if (socket.Send(buffer, 0, buffer.Length, 0) < buffer.Length) success = false;
        }

        /// <summary>
        /// Erhält die URL vom server-socket als byte-array, dekodiert diese mit ASCII
        /// und schreibt den string in die Zwischenablage des benutzers
        /// </summary>
        private static void ReceiveURL()
        {
            byte[] buffer = new byte[1];
            int rec = 0;
            rec = socket.Receive(buffer, 0, buffer.Length, 0);
            if (rec < buffer.Length) success = false;

            SendApproval();

            byte[] buffer2 = new byte[buffer[0]];
            int rec2 = 0;
            rec2 = socket.Receive(buffer2, 0, buffer2.Length, 0);
            if (rec2 < buffer.Length) success = false;
            string str = Encoding.ASCII.GetString(buffer2);

            try
            {
                Clipboard.SetText(str);
            }
            catch { }
        }

        /// <summary>
        /// Sendet den Byte-array der das Image repräsentiert
        /// </summary>
        /// <param name="buffer">Das Bild</param>
        private static void SendImage(byte[] buffer)
        {
            if (socket.Send(buffer, 0, buffer.Length, 0) < buffer.Length) success = false;
        }

        /// <summary>
        /// Konvertiert ein Bild (Image) zu einem byte-array durch einen byte-stream
        /// </summary>
        /// <param name="img">Ein Image z.B. bmp</param>
        /// <returns></returns>
        private static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Sendet die Größe des Bildes an den server-socket, in einem byte-array
        /// mit der festen größe von 16 byte. Die Größe wird kodiert als ASCII-Satz.
        /// Diese Größe ist nötig, damit der server weiß,
        /// wie viel Daten er zu erwarten hat. 
        /// </summary>
        /// <param name="size">Die größe Bild-byte-arrays</param>
        private static void SendMetaData(int size)
        {
            byte[] metaData = new byte[16];
            for (int i = 0; i < metaData.Length; i++)
            {
                metaData[i] = 0;
            }
            string strSize = size.ToString(); // 1234
            byte[] bStrSize = Encoding.ASCII.GetBytes(strSize);
            for (int i = 0; i < bStrSize.Length; i++)
            {
                metaData[i] = bStrSize[i];
            }
            if (socket.Send(metaData, 0, metaData.Length, 0) < metaData.Length) success = false;
        }
    }
}
