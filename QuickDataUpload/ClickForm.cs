using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickDataUpload
{
    /// <summary>
    /// Form, die von AreaCamera erzeugt wird und Punkte für
    /// Bildaufnahme schreibt
    /// </summary>
    public partial class ClickForm : Form
    {
        #region Properties (Attribute mit get/set-Methoden)
        /// <summary>
        /// Punkt an dem die Maustaste gedrückt wurde
        /// </summary>
        public Point PtDown { get; private set; }
        /// <summary>
        /// Punkt an dem die Maustaste losgelassen wurde
        /// </summary>
        public Point PtUp { get; private set; }
        /// <summary>
        /// Bool, ob ESC gedrückt wurde und damit das Bild
        /// nicht gemacht werden soll. 
        /// </summary>
        public bool OperationCancelled { get; set; }
        #endregion 

        /// <summary>
        /// Konstruktor, initialisiert form und setzt
        /// OperationCancelled = false
        /// </summary>
        public ClickForm()
        {
            InitializeComponent();
            DoubleBuffered = true; // Improves quality of drawing
            OperationCancelled = false;
            TopMost = true;
        }

        /// <summary>
        /// Delegate (Methodenreferenz) für das "OnCapture"-event
        /// </summary>
        /// <param name="cF">übergibt die Form,
        /// die das event auslöst</param>
        public delegate void CapturedHandler(ClickForm cF);

        /// <summary>
        /// Event, welches die form auslöst, in der die Area zur
        /// Bildaufnahme liegt. 
        /// </summary>
        public event CapturedHandler OnCapture;

        #region Attribute
        /// <summary>
        /// Bool der festlegt, ob die maus gerade gedrückt wird
        /// </summary>
        private bool mouseDown = false;
        /// <summary>
        /// temporärer zuerst geclickter punkt
        /// </summary>
        private Point mouseDownPoint = Point.Empty;
        /// <summary>
        /// Punkt, an dem sich die mouse befindet
        /// </summary>
        private Point mousePoint = Point.Empty;
        #endregion

        /// <summary>
        /// Registriert Startpunkt des Maus-"ziehens"
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseDown = true;
            mousePoint = mouseDownPoint = e.Location;
            PtDown = this.PointToScreen(e.Location);
        }

        /// <summary>
        /// Registriert Endpunkt des Maus-"ziehens"
        /// und löst das Event aus (beendet auswahlvorgang)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            PtUp = this.PointToScreen(e.Location);
            if(mouseDown) OnCapture?.Invoke(this);
            mouseDown = false;
        }

        /// <summary>
        /// Erneuert anzeige, wenn maus sich bewegt während
        /// sie gedrückt wird. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mousePoint = e.Location;
            Invalidate();
        }

        /// <summary>
        /// Erschafft das Rechteckt der Maus-"ziehens"-Anzeige 
        /// und füllt es mit der transparenten Farbe
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (mouseDown)
            {
                Rectangle window = new Rectangle(
                    Math.Min(mouseDownPoint.X, mousePoint.X),
                    Math.Min(mouseDownPoint.Y, mousePoint.Y),
                    Math.Abs(mouseDownPoint.X - mousePoint.X),
                    Math.Abs(mouseDownPoint.Y - mousePoint.Y));
                Region r = new Region(this.ClientRectangle);
                r.Intersect(window);
                
                e.Graphics.FillRegion(Brushes.DimGray, r);
            }
        }

        /// <summary>
        /// Bei drücken der ESC-Taste wird das Bild-Machen
        /// abgebrochen. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Escape)
            {
                OperationCancelled = true;
                OnCapture?.Invoke(this);
            }
        }
    }
}
