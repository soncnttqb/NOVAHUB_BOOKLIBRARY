using Business.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookLibrary
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!NetWorkHelper.IsNetWorkAvailable())
            {
                MessageBox.Show("Network is unvailable.\n Application will exit now.",
                    Enums.MessageBoxCaption.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            Application.Run(new LoginForm());
        }
    }
}
