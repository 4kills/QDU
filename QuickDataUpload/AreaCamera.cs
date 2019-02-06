using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickDataUpload
{
    
    sealed class AreaCamera : Camera
    {
        /// <summary>
        /// list of necessary win.forms to choose area
        /// </summary>
        List<ClickForm> listClickForm = new List<ClickForm>();
        /// <summary>
        /// all of the code is run at initialization
        /// </summary>
        public AreaCamera()
        {
            // loops through virtual screens and creates a form for each
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
        /// event-handler for OnCapture-event of clickForms. 
        /// sets the chosen area and disposes of forms 
        /// can be cancelled with esc
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
