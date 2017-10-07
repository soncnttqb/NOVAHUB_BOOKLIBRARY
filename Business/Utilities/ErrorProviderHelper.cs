using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Business.Utilities
{
    public static class ErrorProviderHelper
    {
        private static List<ControlError> controlErrorList = new List<ControlError>();
        public static void SetErrorMessage(Control ctl, string errorMessage)
        {
            ControlError controlError = controlErrorList.FirstOrDefault(x => x.Control == ctl);
            if (controlError != null)
            {
                controlError.ErrorProvider.SetError(ctl, errorMessage);
                return;
            }
            ErrorProvider errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.SetError(ctl, errorMessage);

            ControlError control = new ControlError(ctl, errorProvider);
            controlErrorList.Add(control);
        }
        public static void ClearError(Control ctl)
        {
            ControlError controlError = controlErrorList.FirstOrDefault(x => x.Control == ctl);
            if (controlError != null)
            {
                controlError.ErrorProvider.SetError(controlError.Control, string.Empty);
                controlError.ErrorProvider.Clear();
                controlErrorList.Remove(controlError);
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
            firstControl?.Control.Focus();
        }
    }
}
