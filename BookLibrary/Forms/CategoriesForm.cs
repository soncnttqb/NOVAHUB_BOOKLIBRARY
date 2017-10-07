
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
using Business.Models;
using static Business.Utilities.Enums;
using Business.Utilities;
using Business.Bussiness;

namespace BookLibrary.Forms
{
    public partial class CategoriesForm : Form
    {
        private BasePagingModel PagingModel = new BasePagingModel() { PageIndex = 1 };
        private CategoryBusiness _categoryBusiness = new CategoryBusiness();
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
            if (response != null && ResponseCode.Success.Equals(response.ResponseCode))
                LoadGrid();
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            grdListCategories.AutoGenerateColumns = false;
            LoadGrid();
            btnDelete.Visible = Thread.CurrentPrincipal.IsInRole(Enums.RoleTpe.Admin.ToString());
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
            var result = _categoryBusiness.Search(PagingModel);
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
                var response = _categoryBusiness.Delete((grdListCategories.CurrentRow.DataBoundItem as CategoryModel).Id);
                if(response!=null && ResponseCode.Success.Equals(response.ResponseCode))
                {
                    MessageBox.Show("Category has deleted successful.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGrid();
                }
            }
        }
    }
}
