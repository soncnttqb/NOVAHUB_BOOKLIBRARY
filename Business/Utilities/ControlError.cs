using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Business.Utilities
{
    public class ControlError
    {
        public Control Control { get; }
        public ErrorProvider ErrorProvider { get; }
        public ControlError(Control ctrl, ErrorProvider errorProvider)
        {
            this.Control = ctrl;
            this.ErrorProvider = errorProvider;
        }
    }
}
