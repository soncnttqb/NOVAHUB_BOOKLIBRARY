using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Business
{
    public class ControlError
    {
        private Control control = null;
        public Control Control { get { return control; } }
        private ErrorProvider errorProvider = null;
        public ErrorProvider ErrorProvider { get { return errorProvider; } }
        public ControlError(Control ctrl, ErrorProvider errorProvider)
        {
            this.control = ctrl;
            this.errorProvider = errorProvider;
        }
    }
    public static class ErrorProviderHelper
    {
        private static List<ControlError> controlErrorList = new List<ControlError>();
        public static void SetErrorMessage(Control ctl, string errorMessage)
        {
            foreach (ControlError controlError in controlErrorList)
            {
                if (ctl == controlError.Control)
                {
                    controlError.ErrorProvider.SetError(ctl, errorMessage);
                    return;
                }
            }
            ErrorProvider errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.SetError(ctl, errorMessage);

            ControlError control = new ControlError(ctl, errorProvider);
            controlErrorList.Add(control);
        }
        public static void ClearError(Control ctl)
        {
            foreach (ControlError controlError in controlErrorList)
            {
                if (ctl == controlError.Control)
                {
                    controlError.ErrorProvider.SetError(controlError.Control, string.Empty);
                    controlError.ErrorProvider.Clear();
                    controlErrorList.Remove(controlError);
                    break;
                }
            }
        }
        public static void ClearError()
        {
            foreach (ControlError controlError in controlErrorList)
            {
                controlError.ErrorProvider.SetError(controlError.Control, string.Empty);
                controlError.ErrorProvider.Clear();
            }
            controlErrorList.Clear();
        }
        public static void FocusFirstControl()
        {
            ControlError firstControl = controlErrorList.FirstOrDefault();
            if (firstControl != null)
            {
                firstControl.Control.Focus();
            }
        }
    }
}
