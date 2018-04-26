using System;
using System.Windows.Forms;
using System.Drawing;

namespace QuickDataUpload
{
    /// <summary>
    /// Macht ein Bild von allen Bildschirmen. Erbt von Camera. 
    /// Sehr polymorph
    /// </summary>
    sealed class FullCamera : Camera
    {
        /// <summary>
        /// sämtlicher code wird beim erzeugen der Instanz ausgeführt
        /// </summary>
        public FullCamera()
        {
            int minX = 0;
            int minY = 0;
            int maxX = 0;
            int maxY = 0;

            // loopt durch alle virtuellen bildschirme um die maße des bildes zu bekommen
            foreach (var screen in Screen.AllScreens)
            {
                minX = Math.Min(minX, screen.Bounds.Left);
                minY = Math.Min(minY, screen.Bounds.Top);
                maxX = Math.Max(maxX, screen.Bounds.Left + screen.Bounds.Width);
                maxY = Math.Max(maxY, screen.Bounds.Top + screen.Bounds.Height);
            }

            // setzt die Punkte zum malen des Bildes / screenshotten der bildschirme
            PtDown = new Point(minX, minY);
            PtUp = new Point(maxX, maxY);

            // macht das bild
            Snap();
        }
    }
}
