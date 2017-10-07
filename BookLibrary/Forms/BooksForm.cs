using Business;
using Business.Bussiness;
using Business.Models;
using Business.Utilities;
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
using static Business.Utilities.Enums;

namespace BookLibrary.Forms
{
    public partial class BooksForm : Form
    {
        private BookSearchCriteriaModel Criteria;
        private BookBusiness _bookBusines = new BookBusiness();
        public BooksForm()
        {
            InitializeComponent();
        }

        private void BooksForm_Load(object sender, EventArgs e)
        {
            grdList.AutoGenerateColumns = false;
            btnSearch.PerformClick();
            btnDelete.Visible = Thread.CurrentPrincipal.IsInRole(Enums.RoleTpe.Admin.ToString());
        }

        private void UcPagingBook_ExecutePaging(int pageIndex)
        {
            Criteria.PageIndex = pageIndex;
            LoadGrid();
        }

        private void GrdList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                editBook();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BookManagementForm bookManagementForm = new BookManagementForm();
            bookManagementForm.Save += bookManagementForm_Save;
            bookManagementForm.ShowDialog();
        }
        private void bookManagementForm_Save(ResponseModel response)
        {
            if (response != null && ResponseCode.Success.Equals(response.ResponseCode))
                LoadGrid();
        }

        private void LoadGrid()
        {
            var result = _bookBusines.Search(Criteria);
            grdList.DataSource = result.Results;
            ucPagingBook.TotalRecord = result.Total;
            btnEdit.Enabled = btnDelete.Enabled = grdList.RowCount > 0;
            loadAuthorAndCategory();
            ucPagingBook.Config();
        }

        private void GrdList_SelectionChanged(object sender, EventArgs e)
        {
            loadAuthorAndCategory();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editBook();
        }

        private void editBook()
        {
            BookManagementForm bookManagementForm = new BookManagementForm();
            bookManagementForm.Save += bookManagementForm_Save;
            bookManagementForm.Id = (grdList.CurrentRow.DataBoundItem as BookModel).Id;
            bookManagementForm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to delete this book?", MessageBoxCaption.Confirmation.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                BookModel model = grdList.CurrentRow.DataBoundItem as BookModel;
                var response = _bookBusines.Delete(model.Id);
                if (response != null && ResponseCode.Success.Equals(response.ResponseCode))
                {
                    MessageBox.Show("Book has deleted successful.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //delete cover photo
                    FileHelper.DeleteFile(model.CoverPhoto);
                    LoadGrid();
                }
            }
        }

        private void loadAuthorAndCategory()
        {
            if (grdList.CurrentRow != null)
            {
                BookModel model = grdList.CurrentRow.DataBoundItem as BookModel;
                if (model.Author != null)
                {
                    txtAuthor.Text = model.Author.Title;
                    txtDescriptionAuthor.Text = model.Author.Description;
                    if (!string.IsNullOrEmpty(model.Author.CoverPhoto) && File.Exists(model.Author.CoverPhoto))
                    {
                        FileHelper.LoadImage(model.Author.CoverPhoto, pictureBoxAuthor);
                    }
                }
                if (model.Category != null)
                {
                    txtCategory.Text = model.Category.Title;
                    txtDescriptionAuthor.Text = model.Category.Description;
                }
            }
            else
            {
                txtAuthor.Text = txtDescriptionAuthor.Text = txtCategory.Text = txtDescriptionAuthor.Text = string.Empty;
                pictureBoxAuthor.Image = null;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ucPagingBook.PageIndex != 1)
            {
                ucPagingBook.ResetPaging();
            }
            else
            {
                Criteria = new BookSearchCriteriaModel()
                {
                    PageIndex = 1,
                    Title = txtBook.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Publisher = txtPublisher.Text.Trim(),
                    Year=txtYear.Text.Trim(),
                    Author = txtAuthorSearch.Text.Trim(),
                    Category = txtCategorySearch.Text.Trim()
                };
                LoadGrid();
            }
        }
    }
}
