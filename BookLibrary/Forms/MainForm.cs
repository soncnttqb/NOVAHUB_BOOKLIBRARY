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

namespace BookLibrary
{
    public partial class MainForm : Form
    {
        #region private
        #endregion
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["LoginForm"].Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread.CurrentPrincipal = null;
            this.Close();
        }

        private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showChildForm(new AuthorsForm());
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
            showChildForm(new BooksForm());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            booksToolStripMenuItem_Click(null, null);
            this.lblEmailUser.Text = Thread.CurrentPrincipal.Identity.Name;
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showChildForm(new CategoriesForm());
        }
        private void showChildForm(Form form)
        {
            foreach (var mdiChild in this.MdiChildren)
            {
                if (mdiChild.Name.Equals(form.Name))
                {
                    mdiChild.Activate();
                    return;
                }
            }
            CloseChildForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
            this.Text = form.Text;
        }
    }
}
