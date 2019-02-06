using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using System.Reflection;

namespace QuickDataUpload
{
    /// <summary>
    /// abstract for specific camera implementation 
    /// </summary>
    abstract class Camera 
    {
        #region Attribute 
        
        /// <summary>
        /// start point 
        /// </summary>
        protected Point PtDown;
        /// <summary>
        /// end point
        /// </summary>
        protected Point PtUp;
        /// <summary>
        /// taken ss residing in memory 
        /// </summary>
        public static Bitmap BmpSS { get; protected set; }
        /// <summary>
        /// is set when picture is sucessfully taken
        /// </summary>
        public bool ProcedureDone { get; protected set; } = false;

        #endregion

        /// <summary>
        /// saves the area as bitmap
        /// </summary>
        /// <returns>returns false if method failed</returns>
        protected virtual bool SaveScreenshot()
        {
            // error if both points are the same
            if (PtDown == PtUp) return false;

            double resFactor = CalcScreenRes();
            Rectangle rect = CalcRect(resFactor);

            if (rect.Width == 0 || rect.Height == 0) return false;
            
            // creates bitmap with specified hight and width of destination rectangle
            BmpSS = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
            

            // graphics to paint on the bitmap
            using (Graphics g = Graphics.FromImage(BmpSS))
            {
                // copies chosen area to bitmap
                g.CopyFromScreen(rect.X, rect.Y, 0, 0,
                    new Size(rect.Size.Width, rect.Size.Height), 
                    CopyPixelOperation.SourceCopy); 
            }
            
            return true;
        }

        /// <summary>
        /// processes pic depending on chosen mode 
        /// </summary>
        protected void PicProceed()
        {
            if (OptionsData.Online)
            {
                // sends pic to the server and receives URL, returns if an error occurs
                if (!Sender.SendPic())
                {
                    Program.icon.ShowBalloonTip(3000, "Connection failed", "Your screenshot was not uploaded " +
                    "because the client couldn't connect to a QDU-server. Check you server data or " +
                    "Internet connection", ToolTipIcon.Error);
                    return; 
                }

                // plays a self-recorded sound when sending pic successful, notifying user about url
                (new Thread(() => (new SoundPlayer(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("QDU.upload_sound.wav"))).Play()))
                    .Start();
                Program.icon.ShowBalloonTip(1000, "Screenshot taken", "Your screenshot was " +
                    "successfully uploaded to a QDU-server", ToolTipIcon.Info); 
            }
            else if (OptionsData.ToDisk) SaveToDisk();
            else if (OptionsData.ToClipboard)
            {
                // writes bitmap to clipboard
                try
                {
                    Clipboard.SetImage(BmpSS);
                }
                catch (Exception e)
                {
                    throw new OperationCanceledException(e.Message + 
                        "\n\n Couldn't copy image to clipboard; " +
                        "missing permissions?"); 
                }
            }
        }

        /// <summary>
        /// executes generic snapping 
        /// </summary>
        protected void Snap()
        {
            if (SaveScreenshot()) PicProceed(); // proceeds if successful

            BmpSS?.Dispose(); // purges RAM 
            ProcedureDone = true; // signalizes the picture is taken
            MemoryManager.MinimizeFootprint(); // minimizes assigned ram by os 
        }

        // saves picture to disk
        #region ToDisk

        SaveFileDialog fileDiag;
        /// <summary>
        /// configures save file dialogue and opens it. The user can change a location
        /// </summary>
        public void SaveToDisk()
        {
            fileDiag = new SaveFileDialog();
            fileDiag.FileName = "Unbenannt";
            fileDiag.Filter = " PNG |*.png| GIF |*.gif| Unkomprimierte Bitmap |*.bmp| JPEG (nicht empfohlen) |*.jpg"; //Dateiformate
            fileDiag.Title = "Speichern des Koordinatensystems";
            fileDiag.FileOk += FileDiag_FileOk; // event, dass bei erfolgreichem auswählen ausgeführt wird
            fileDiag.ShowDialog();

        }

        /// <summary>
        /// saves the picture to the specified location in the specified format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileDiag_FileOk(object sender, System.ComponentModel.CancelEventArgs e) 
        {
            if (fileDiag.FileName != "") // only if path specified
            {
                System.IO.FileStream fileStream = (System.IO.FileStream)fileDiag.OpenFile(); // new file stream, writes to file / creates new file
                switch (fileDiag.FilterIndex) //Betrachtet auf 1-basierenden Filterindex für Dateiformate
                {
                    // Speichern der Bitmap in ausgewählten Formaten
                    case 1: BmpSS.Save(fileStream, ImageFormat.Png); break;
                    case 2: BmpSS.Save(fileStream, ImageFormat.Gif); break;
                    case 3: BmpSS.Save(fileStream, ImageFormat.Bmp); break; // not compressed; good for pic editing
                    case 4: BmpSS.Save(fileStream, ImageFormat.Jpeg); break; // not to be recommanded, bad qualitiy with bad compression
                }
                fileStream.Close(); //closes filestream and show file in file explorer
            }
        }
        #endregion

        /// <summary>
        /// calculates rectangle , considering OS magnification (scaling for laptops, tablets)
        /// </summary>
        /// <param name="resFactor">factor of magnification (default: 1)</param>
        /// <returns>rectangle as chosen picture area</returns>
        private Rectangle CalcRect(double resFactor)
        {
            return new Rectangle(
                    (int)(resFactor * Math.Min(PtDown.X, PtUp.X)),
                    (int)(resFactor * Math.Min(PtDown.Y, PtUp.Y)),
                    (int)(resFactor * Math.Abs(PtDown.X - PtUp.X)),
                    (int)(resFactor * Math.Abs(PtDown.Y - PtUp.Y)));
        }
        /// <summary>
        /// calculates the rectangle from two points
        /// </summary>
        /// <returns></returns>
        private Rectangle CalcRect() { return CalcRect(1); }

        #region ErrorHandlingDerBildschirmaufloesung
        
        /// <summary>
        /// reads screen scaling in win mobile devices from registry
        /// </summary>
        /// <returns>default: 1</returns>
        private double CalcScreenRes() 
        {
            double currentDPI = (int)
                (Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", null) ?? // <= win 8.1 and older
                Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", null) //win 10
                ?? ExecuteRegWarning());

            return currentDPI / 96f; // calculates factor. 96 are standardized 100% res
        }
        
        /// <summary>
        /// coroutine that warns user about issues reading from registry
        /// </summary>
        private Thread warningThread;
        /// <summary>
        /// executes warning and returns default of 96 dpi 
        /// </summary>
        /// <returns></returns>
        private double ExecuteRegWarning()
        {
            (warningThread = new Thread(RegistryWarning)).Start();
            return 96;
        }

        /// <summary>
        /// tells the user that registry could not be found
        /// </summary>
        private void RegistryWarning() 
        {
            warningThread.Join(700); 
            string strWarning = "No DPI-Scaling in your registry could be found" +
                                "\nThe Screenshot might have issues \n\nMissing keys: " +
                                "\nHKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics \t      AppliedDPI " +
                                "\nHKEY_CURRENT_USER\\Control Panel\\Desktop \t\t      LogPixels";
            MessageBox.Show(strWarning, "Error");
        }
        #endregion
    }
}
