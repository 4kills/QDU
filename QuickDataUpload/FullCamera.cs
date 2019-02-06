using System;
using System.Windows.Forms;
using System.Drawing;

namespace QuickDataUpload
{
   
    sealed class FullCamera : Camera
    {
        
        public FullCamera()
        {
            int minX = 0;
            int minY = 0;
            int maxX = 0;
            int maxY = 0;

            // loops through virtual screens to get size of picture
            foreach (var screen in Screen.AllScreens)
            {
                minX = Math.Min(minX, screen.Bounds.Left);
                minY = Math.Min(minY, screen.Bounds.Top);
                maxX = Math.Max(maxX, screen.Bounds.Left + screen.Bounds.Width);
                maxY = Math.Max(maxY, screen.Bounds.Top + screen.Bounds.Height);
            }

            // sets points for taking a screenshot
            PtDown = new Point(minX, minY);
            PtUp = new Point(maxX, maxY);

            // takes the pic
            Snap();
        }
    }
}
