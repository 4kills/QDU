using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickDataUpload
{
    /// <summary>
    /// Fotografiert nur einen ausgewählten bereich der Bildschirme. Erbt von Camera.
    /// Sehr polymorph
    /// </summary>
    sealed class AreaCamera : Camera
    {
        /// <summary>
        /// Eine Liste der benötigten windows.forms die zum bild machen benutzt werden
        /// </summary>
        List<ClickForm> listClickForm = new List<ClickForm>();
        /// <summary>
        /// sämtlicher code wird bei erzeugung der Instanz ausgeführt. 
        /// </summary>
        public AreaCamera()
        {
            // loopt durch die virtuellen Bildschirme und erschafft eine clickForm für jeden bildschirm.
            // Diese werden der listClickForm-Liste hinzugefügt
            foreach (var screen in Screen.AllScreens) //TODO: Can currently cause problems with certain graphics cards options!
            {
                var clickForm = new ClickForm();
                clickForm.Left = screen.WorkingArea.Left;
                clickForm.Top = screen.WorkingArea.Top;

                clickForm.OnCapture += OnSelectionHandle;
                
                listClickForm.Add(clickForm);
                clickForm.Show();
            }
        }

        /// <summary>
        /// event-handler für das OnCapture-event der clickForm's. 
        /// Setzt den ausgewählten Bereich als Punkte und vernichtet alle clickForm's.
        /// Wenn nicht esc gedrückt wurde wir das bild mit Snap() gemacht.
        /// </summary>
        /// <param name="cF"></param>
        private void OnSelectionHandle(ClickForm cF)
        {
            PtDown = cF.PtDown; PtUp = cF.PtUp;
            foreach (var vcF in listClickForm)
            {
                vcF.Hide();
                vcF.Close();
                vcF.Dispose();
            }

            if (cF.OperationCancelled)
            {
                ProcedureDone = true;
                MemoryManager.MinimizeFootprint();
                return;
            }

            Snap();
        }
    }
}
