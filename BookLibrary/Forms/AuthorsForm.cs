using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Business.Enums;

namespace BookLibrary
{
    public partial class AuthorsForm : Form
    {
        private BasePagingModel PagingModel = new BasePagingModel() { PageIndex = 1 };
        public AuthorsForm()
        {
            InitializeComponent();
        }
        private void AuthorsForm_Load(object sender, EventArgs e)
        {
            grdList.AutoGenerateColumns = false;
            LoadGrid();
            btnDelete.Visible = Thread.CurrentPrincipal.IsInRole(Enums.RoleTpe.Admin.ToString());
            grdList.CellDoubleClick += GrdList_CellDoubleClick;
            ucPagingAuthor.ExecutePaging += UcPagingAuthor_ExecutePaging;
            
        }

        private void UcPagingAuthor_ExecutePaging(int pageIndex)
        {
            PagingModel = new BasePagingModel()
            {
                PageIndex = pageIndex
            };
            LoadGrid();
        }

        private void GrdList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                editAuthor();
            }
        }

        private void LoadGrid()
        {
            var result = AuthorBusiness.Search(PagingModel);
            grdList.DataSource = result.Results;
            ucPagingAuthor.TotalRecord = result.Total;
            btnEdit.Enabled = btnDelete.Enabled = grdList.RowCount > 0;
            ucPagingAuthor.Config();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AuthorManagementForm authorManagementForm = new AuthorManagementForm();
            authorManagementForm.Save += authorManagementForm_Save;
            authorManagementForm.ShowDialog();
        }
        private void authorManagementForm_Save(ResponseModel response)
        {
            if (response != null && "Success".Equals(response.Message))
                LoadGrid();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editAuthor();
        }
        private void editAuthor()
        {
            AuthorManagementForm authorManagementForm = new AuthorManagementForm();
            authorManagementForm.Save += authorManagementForm_Save;
            authorManagementForm.Id = (grdList.CurrentRow.DataBoundItem as AuthorModel).Id;
            authorManagementForm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to delete this author?", MessageBoxCaption.Confirmation.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                AuthorModel model = grdList.CurrentRow.DataBoundItem as AuthorModel;
                var response = AuthorBusiness.Delete(model.Id);
                if (response != null && "Success".Equals(response.Message))
                {
                    MessageBox.Show("Author has deleted successful.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //delete cover photo
                    try
                    {
                        if (!string.IsNullOrEmpty(model.CoverPhoto) && File.Exists(model.CoverPhoto))
                        {
                            File.Delete(model.CoverPhoto);
                        }
                    }
                    catch { }
                    LoadGrid();
                }
            }
        }
    }
}
