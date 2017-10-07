using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookLibrary.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            string loginFormName = (new LoginForm()).Name;
            Form openLoginForm = Application.OpenForms[loginFormName];
            openLoginForm?.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread.CurrentPrincipal = null;
            this.Close();
        }

        private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showChildForm<AuthorsForm>();
        }
        private void CloseChildForm()
        {
            foreach (var mdiChild in this.MdiChildren)
            {
                mdiChild.Close();
            }
        }
        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showChildForm<BooksForm>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            booksToolStripMenuItem_Click(null, null);
            this.lblEmailUser.Text = Thread.CurrentPrincipal.Identity.Name;
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showChildForm<CategoriesForm>();
        }
        private void showChildForm<T>() where T : Form
        {
            var existingForm = this.MdiChildren.OfType<T>().FirstOrDefault();
            if (existingForm != null)
            {
                existingForm.Activate();
                return;
            }
            CloseChildForm();
            var form = Activator.CreateInstance<T>();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
            this.Text = form.Text;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutForm()).ShowDialog();
        }
    }
}
