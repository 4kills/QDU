using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using QDU.Properties;

namespace QuickDataUpload
{
    /// <summary>
    /// static class managing all of the networking
    /// </summary>
    static class Sender
    {
        /// <summary>
        /// socket of the Client
        /// </summary>
        private static Socket socket;

        /// <summary>
        /// flag representing successfulness of sending procedure
        /// </summary>
        private static bool success;

        /// <summary>
        /// Establishes connection with server (specified in settings) and sends the pic.
        /// the server publishes the picture online and returns an url to that pic.
        /// </summary>
        /// <returns>returns success flag when done</returns>
        public static bool SendPic()
        {

            success = true;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { socket.Connect(OptionsData.MainHost.IP); } 
            catch { return false; }

            if (Settings.Default.Token == "null" || Settings.Default.Token == "") RequestToken();
            else { DeclareService(Service.SendPic); RecApproval(); }


            SendToken();
            RecApproval();
            byte[] buffer = ImageToByte(Camera.BmpSS);
            SendMetaData(buffer.Length);
            RecApproval();
            SendImage(buffer);
            ReceiveURL();
            socket.Close();
            return success;
        }

        private static void SendToken()
        {
            byte[] buffer = new byte[36];
            var chars = Encoding.ASCII.GetBytes(Settings.Default.Token);
            for (int i = 0; i < chars.Length; i++) buffer[i] = chars[i];
            if (socket.Send(buffer, 0, buffer.Length, 0) < buffer.Length) success = false;
        }
    
        private static void RequestToken()
        {
            DeclareService(Service.RequestToken);

            string str = Encoding.ASCII.GetString(Receive(36));

            Settings.Default.Token = str;
            Settings.Default.Save();

            SendApproval();
        }

        private enum Service : byte
        {
            SendPic = 0,
            RequestToken = 1
        }

        private static void DeclareService(Service serv)
        {
            SendOneByte((byte)serv);
        }

        /// <summary>
        /// waits for the server to tell client it is ready for more data, part of protocol
        /// </summary>
        private static void RecApproval()
        {
            Receive(1);
        }

        /// <summary>
        /// tells the server it can go ahead, part of transmisison protocol
        /// </summary>
        private static void SendApproval()
        {
            SendOneByte(1);
        }

        private static void SendOneByte(byte num)
        {
            byte[] buffer = new byte[1] { num };
            if (socket.Send(buffer, 0, buffer.Length, 0) < buffer.Length) success = false;
        }

        private static byte[] Receive (int size)
        {
            byte[] buffer = new byte[size];
            int rec = 0;
            while (rec < size)
            {
                rec += socket.Receive(buffer);
            }
            return buffer;
        }

        /// <summary>
        /// receives the urls from server-socket as byte-array, decodes it using ASCII
        /// and writes string to clipboard
        /// </summary>
        private static void ReceiveURL()
        {
            byte size = Receive(1)[0];

            SendApproval();
            
            string str = Encoding.ASCII.GetString(Receive(size));

            try
            {
                Clipboard.SetText(str);
            }
            catch { }
        }

        /// <summary>
        /// sends Byte-array representing the image
        /// </summary>
        /// <param name="buffer">the picture</param>
        private static void SendImage(byte[] buffer)
        {
            if (socket.Send(buffer, 0, buffer.Length, 0) < buffer.Length) success = false;
        }

        /// <summary>
        /// converts the pic (image) to a byte array using a byte stream
        /// </summary>
        /// <param name="img">an Image eg bmp</param>
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
        /// sends the size of the pic to server using a byte-array
        /// server needs to know the size to know when to proceed 
        /// </summary>
        /// <param name="size">size of the image byte array </param>
        private static void SendMetaData(int size)
        {
            byte[] metaData = new byte[16];
            for (int i = 0; i < metaData.Length; i++)
            {
                metaData[i] = 0;
            }
            string strSize = size.ToString();
            byte[] bStrSize = Encoding.ASCII.GetBytes(strSize);
            for (int i = 0; i < bStrSize.Length; i++)
            {
                metaData[i] = bStrSize[i];
            }
            if (socket.Send(metaData, 0, metaData.Length, 0) < metaData.Length) success = false;
        }
    }
}
