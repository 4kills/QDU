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
    /// Abstrakte Klasse die als Vorlage für die polymorphen sub-klassen wirkt
    /// </summary>
    abstract class Camera 
    {
        #region Attribute 
        
        /// <summary>
        /// Erster Punkt des Bildes
        /// </summary>
        protected Point PtDown;
        /// <summary>
        /// Zweiter Punkt des Bildes
        /// </summary>
        protected Point PtUp;
        /// <summary>
        /// Gemachtes Bild, welches sich im RAM befindet. 
        /// </summary>
        public static Bitmap BmpSS { get; protected set; }
        /// <summary>
        /// Wird true, wenn das Bild vollständig verarbeitet wurde
        /// </summary>
        public bool ProcedureDone { get; protected set; } = false;

        #endregion

        /// <summary>
        /// Speichert den Bereich als Bitmap-Image in den RAM der Camera-Klasse.
        /// </summary>
        /// <returns>returned false, wenn operation misslungen</returns>
        protected virtual bool SaveScreenshot()
        {
            // fehler wenn beide punkte gleich
            if (PtDown == PtUp) return false;

            double resFactor = CalcScreenRes();
            Rectangle rect = CalcRect(resFactor);

            if (rect.Width == 0 || rect.Height == 0) return false;
            
            // erstellt neue bitmap mit der größe des rechtecks
            BmpSS = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
            

            // benutzt graphics um auf die bitmap zu zeichnen
            using (Graphics g = Graphics.FromImage(BmpSS))
            {
                // zeichnet ausgewählten bildbereich auf die bitmap
                g.CopyFromScreen(rect.X, rect.Y, 0, 0,
                    new Size(rect.Size.Width, rect.Size.Height), 
                    CopyPixelOperation.SourceCopy); 
            }
            
            return true;
        }

        /// <summary>
        /// Verarbeitet das gemacht Bild, je nach ausgewähltem Modus
        /// </summary>
        protected void PicProceed()
        {
            if (OptionsData.Online)
            {
                // sendet Bild an den Server und erhält URL zurück. 
                // Sollte etwas schief gehen wird abgebrochen
                if(!Sender.SendPic()) return;

                // spielt einen selbst aufgenommenen ton, wenn der Upload erfolgreich ist, 
                // damit der benutzer weiß, wann er die URL in der Zwischenablage hat. 
                (new Thread(() => (new SoundPlayer(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("QDU.upload_sound.wav"))).Play()))
                    .Start();
            }
            else if (OptionsData.ToDisk) SaveToDisk();
            else if (OptionsData.ToClipboard)
            {
                // schreibt das bild vom RAM direkt in die Zwischenablage wo es via strg + v
                // verbreitet werden kann
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
        /// Führt nicht spezifischen Bilderstellungs/verfahrens-Code aus
        /// </summary>
        protected void Snap()
        {
            if (SaveScreenshot()) PicProceed(); // fährt nur fort, wenn bild erfolgreich gemacht

            BmpSS?.Dispose(); // Bereinigt RAM
            ProcedureDone = true; // teilt mit, dass der Bilderstellungsprozess beendet wure
            MemoryManager.MinimizeFootprint();
        }

        // Speichert Bild zur festplatte
        #region ToDisk

        /// <summary>
        /// Der windows-file-dialog der angezeigt wird um das bild zu speichern
        /// </summary>
        SaveFileDialog fileDiag;
        /// <summary>
        /// Konfiguriert den SaveFileDialog und zeigt diesen an. Damit kann der Benutzer einen Speicherort
        /// für das Bild auswählen. 
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
        /// Speichert das Bild an den ausgewählten Speicherort in dem angegebenen Format ab. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileDiag_FileOk(object sender, System.ComponentModel.CancelEventArgs e) // Nur wenn "Speichern" bestätigt -> bug fix
        {
            if (fileDiag.FileName != "") // Nur wenn Speicherort ausgewählt / benannt
            {
                System.IO.FileStream fileStream = (System.IO.FileStream)fileDiag.OpenFile(); // Neuer Output-Filestream / Schreibt auf ausgewählte Datei/Neue Datei
                switch (fileDiag.FilterIndex) //Betrachtet auf 1-basierenden Filterindex für Dateiformate
                {
                    // Speichern der Bitmap in ausgewählten Formaten
                    case 1: BmpSS.Save(fileStream, ImageFormat.Png); break;
                    case 2: BmpSS.Save(fileStream, ImageFormat.Gif); break;
                    case 3: BmpSS.Save(fileStream, ImageFormat.Bmp); break; // Unkompremiert; gut für evtl Bild-Bearbeitung
                    case 4: BmpSS.Save(fileStream, ImageFormat.Jpeg); break; // Nicht zu empfehlen, da schlechte Qualität bei schlechtester Kompression
                }
                fileStream.Close(); //Beendet FileStream und erstellt Datei
            }
        }
        #endregion

        /// <summary>
        /// Berechnet das Rechteck aus den beiden Rechteck-Punkten unter Betrachtung
        /// der OS-Vergrößerung (Scaling bei laptops, tablets etc) 
        /// </summary>
        /// <param name="resFactor">der vergrößerungsfaktor (default: 1)</param>
        /// <returns>Rechteck welches den Bildbereich darstellt</returns>
        private Rectangle CalcRect(double resFactor)
        {
            return new Rectangle(
                    (int)(resFactor * Math.Min(PtDown.X, PtUp.X)),
                    (int)(resFactor * Math.Min(PtDown.Y, PtUp.Y)),
                    (int)(resFactor * Math.Abs(PtDown.X - PtUp.X)),
                    (int)(resFactor * Math.Abs(PtDown.Y - PtUp.Y)));
        }
        /// <summary>
        /// Berechnet das Rechteck aus den beiden Rechteck-Punkten
        /// </summary>
        /// <returns></returns>
        private Rectangle CalcRect() { return CalcRect(1); }

        #region ErrorHandlingDerBildschirmaufloesung
        
        /// <summary>
        /// Berechnet die Screenskalierung von mobilen windows geräten. Liest aus der Registry aus.
        /// </summary>
        /// <returns>default: 1</returns>
        private double CalcScreenRes() 
        {
            double currentDPI = (int)
                (Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", null) ?? // <= win 8.1 u. älter
                Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", null) //win 10
                ?? ExecuteRegWarning());

            return currentDPI / 96f; // calculates factor. 96 are standardized 100% res
        }
        
        /// <summary>
        /// Thread, der den benutzer warnt, dass es möglicherweise probleme mit dem Auslesen der Auflösung geben hat
        /// </summary>
        private Thread warningThread;
        /// <summary>
        /// Führt die Warnung aus und gibt den Standardwert 96 DPI zurück
        /// </summary>
        /// <returns></returns>
        private double ExecuteRegWarning()
        {
            (warningThread = new Thread(RegistryWarning)).Start();
            return 96;
        }

        /// <summary>
        /// Teilt dem Benutzer mit, dass der gesuchte Registry-Key nicht gefunden wurde
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
