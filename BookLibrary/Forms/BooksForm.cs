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
    public partial class BooksForm : Form
    {
        private BookSearchCriteriaModel Criteria;
        public BooksForm()
        {
            InitializeComponent();
        }

        private void BooksForm_Load(object sender, EventArgs e)
        {
            grdList.AutoGenerateColumns = false;
            btnSearch.PerformClick();
            btnDelete.Visible = Thread.CurrentPrincipal.IsInRole(Enums.RoleTpe.Admin.ToString());
            grdList.CellDoubleClick += GrdList_CellDoubleClick;
            txtBook.KeyUp += textSearch_KeyUp;
            txtDescription.KeyUp += textSearch_KeyUp;
            ucPagingBook.ExecutePaging += UcPagingBook_ExecutePaging;
        }

        private void UcPagingBook_ExecutePaging(int pageIndex)
        {
            Criteria.PageIndex = pageIndex;
            LoadGrid();
        }

        private void textSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
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
            if (response != null && "Success".Equals(response.Message))
                LoadGrid();
        }

        private void LoadGrid()
        {
            grdList.SelectionChanged -= GrdList_SelectionChanged;
            var result = BookBusiness.Search(Criteria);
            grdList.DataSource = result.Results;
            ucPagingBook.TotalRecord = result.Total;
            btnEdit.Enabled = btnDelete.Enabled = grdList.RowCount > 0;
            loadAuthorAndCategory();
            grdList.SelectionChanged += GrdList_SelectionChanged;
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
                var response = BookBusiness.Delete(model.Id);
                if (response != null && "Success".Equals(response.Message))
                {
                    MessageBox.Show("Book has deleted successful.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        try
                        {
                            FileStream fs = new FileStream(model.Author.CoverPhoto, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            var bytes = new byte[fs.Length];
                            fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                            pictureBoxAuthor.Image = Image.FromStream(fs);
                            fs.Close();
                        }
                        catch { }
                    }
                }
                if (model.Category != null)
                {
                    txtCategory.Text = model.Category.Title;
                    txtDescriptionAuthor.Text = model.Category.Description;
                }
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
                    Filter = txtFilter.Text.Trim()
                };
                LoadGrid();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void BooksForm_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
