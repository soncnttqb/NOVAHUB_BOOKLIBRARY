
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;
using System.Threading;
using static Business.Enums;

namespace BookLibrary
{
    public partial class CategoriesForm : Form
    {
        private BasePagingModel PagingModel = new BasePagingModel() { PageIndex = 1 };
        public CategoriesForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryManagementForm categoryManagementForm = new CategoryManagementForm();
            categoryManagementForm.Save += categoryManagementForm_Save;
            categoryManagementForm.ShowDialog();
        }

        private void categoryManagementForm_Save(ResponseModel response)
        {
            if (response != null && "Success".Equals(response.Message))
                LoadGrid();
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            grdListCategories.AutoGenerateColumns = false;
            LoadGrid();
            btnDelete.Visible = Thread.CurrentPrincipal.IsInRole(Enums.RoleTpe.Admin.ToString());
            grdListCategories.CellDoubleClick += GrdListCategories_CellDoubleClick;
            ucPagingCategory.ExecutePaging += UcPagingCategory_ExecutePaging;
        }
        private void UcPagingCategory_ExecutePaging(int pageIndex)
        {
            PagingModel = new BasePagingModel()
            {
                PageIndex = pageIndex
            };
            LoadGrid();
        }
        private void GrdListCategories_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                editCategory();
            }
        }

        private void LoadGrid()
        {
            var result = CategoryBusiness.Search(PagingModel);
            grdListCategories.DataSource = result.Results;
            ucPagingCategory.TotalRecord = result.Total;
            btnEdit.Enabled = btnDelete.Enabled = grdListCategories.RowCount > 0;
            ucPagingCategory.Config();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editCategory();
        }

        private void editCategory()
        {
            CategoryManagementForm categoryManagementForm = new CategoryManagementForm();
            categoryManagementForm.Save += categoryManagementForm_Save;
            categoryManagementForm.Id = (grdListCategories.CurrentRow.DataBoundItem as CategoryModel).Id;
            categoryManagementForm.ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure want to delete this catetory?", MessageBoxCaption.Confirmation.ToString(),MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
            {
                var response = CategoryBusiness.Delete((grdListCategories.CurrentRow.DataBoundItem as CategoryModel).Id);
                if(response!=null && "Success".Equals(response.Message))
                {
                    MessageBox.Show("Category has deleted successful.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGrid();
                }
            }
        }
    }
}
