using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickDataUpload
{
    /// <summary>
    /// Form created by area camera for selecting snap area
    /// </summary>
    public partial class ClickForm : Form
    {
        #region Properties (Attribute mit get/set-Methoden)
        /// <summary>
        /// point where mouse 1 is pressed
        /// </summary>
        public Point PtDown { get; private set; }
        /// <summary>
        /// point where mouse 1 is released
        /// </summary>
        public Point PtUp { get; private set; }
        /// <summary>
        /// flag whether esc was pressed, cancelling taking pic
        /// </summary>
        public bool OperationCancelled { get; set; }
        #endregion 

        public ClickForm()
        {
            InitializeComponent();
            DoubleBuffered = true; // Improves quality of drawing
            OperationCancelled = false;
            TopMost = true;
        }

        /// <summary>
        /// Delegate for "OnCapture"-event
        /// </summary>
        /// <param name="cF">form that triggers event</param>
        public delegate void CapturedHandler(ClickForm cF);

        /// <summary>
        /// Event triggering form that includes selected area
        /// </summary>
        public event CapturedHandler OnCapture;

        #region Attribute
        /// <summary>
        /// flag for mouse pressed
        /// </summary>
        private bool mouseDown = false;
        /// <summary>
        /// temporary mouse down point
        /// </summary>
        private Point mouseDownPoint = Point.Empty;
        /// <summary>
        /// point where mouse is at regardless of state
        /// </summary>
        private Point mousePoint = Point.Empty;
        #endregion

        /// <summary>
        /// registers starting point of mouse dragging
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
        /// registers end pint of mouse dragging
        /// and triggers end of selection event
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
        /// refreshes display when mouse is moved while down
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mousePoint = e.Location;
            Invalidate();
        }

        /// <summary>
        /// creates rectangle and makes it transparent
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
        /// pressing esc cancels taking a picture
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
