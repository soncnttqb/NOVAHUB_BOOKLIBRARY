using Business;
using Business.Bussiness;
using Business.Models;
using Business.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Business.Utilities.Enums;

namespace BookLibrary.Forms
{
    public partial class BookManagementForm : Form
    {
        public delegate void SaveFormHandler(ResponseModel model);
        public event SaveFormHandler Save;
        public int Id { get; set; }
        private BookBusiness _bookBusiness = new BookBusiness();
        private AuthorBusiness _authorBusiness = new AuthorBusiness();
        private CategoryBusiness _categoryBusiness = new CategoryBusiness();
        private string tempImageFilePath;
        private string serverFolderPath;
        public BookManagementForm()
        {
            InitializeComponent();
        }

        private void setTitleForForm()
        {
            this.Text = this.Id == 0 ? "Add Book" : "Edit Book";
        }
        private void BookManagementForm_Load(object sender, EventArgs e)
        {
            serverFolderPath = _bookBusiness.GetFolderImagePath();
            FileHelper.CreateFolderIfNotExist(FileHelper.TempFolderPath);
            FileHelper.CreateFolderIfNotExist(serverFolderPath);

            cboAuthor.DisplayMember = "DisplayMember";
            cboAuthor.ValueMember = "ValueMember";
            cboAuthor.DataSource = _authorBusiness.GetSelectListAuthor();
            cboCategory.DisplayMember = "DisplayMember";
            cboCategory.ValueMember = "ValueMember";
            cboCategory.DataSource = _categoryBusiness.GetSelectListCategory();

            cboYear.DisplayMember = "DisplayMember";
            cboYear.ValueMember = "ValueMember";
            cboYear.DataSource = Utils.GetListYears();

            setTitleForForm();
            if (this.Id == 0) return;

            BookModel model = _bookBusiness.Get(this.Id);
            if (model != null)
            {
                txtBook.Text = model.Title;
                txtDescription.Text = model.Description;
                txtPublisher.Text = model.Publisher;
                cboAuthor.SelectedValue = model.AuthorId.GetValueOrDefault();
                cboCategory.SelectedValue = model.CategoryId.GetValueOrDefault();
                cboYear.SelectedValue = model.Year.GetValueOrDefault();

                if (!string.IsNullOrEmpty(model.CoverPhoto) && File.Exists(model.CoverPhoto))
                {
                    try
                    {
                        byte[] bytes = FileHelper.GetByteFromFile(model.CoverPhoto);
                        string fileName = Path.GetFileName(model.CoverPhoto);
                        tempImageFilePath = $@"{FileHelper.TempFolderPath}\{fileName}";
                        FileHelper.CopyImage(bytes, tempImageFilePath);
                        FileHelper.LoadImage(tempImageFilePath, picCoverPhoto);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnChangePhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = FileHelper.FilterExtension;
            if (file.ShowDialog() == DialogResult.OK)
            {
                string extend = Path.GetExtension(file.FileName);

                if (!FileHelper.IsValidExtensionFile(extend))
                {
                    MessageBox.Show(Constants.WrongExtensionFile, MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FileHelper.DeleteFile(tempImageFilePath);

                tempImageFilePath = $@"{FileHelper.TempFolderPath}\{Guid.NewGuid().ToString("N")}{extend}";
                byte[] bytes = FileHelper.GetByteFromFile(file.FileName);
                FileHelper.CopyImage(bytes, tempImageFilePath);
                FileHelper.LoadImage(tempImageFilePath, picCoverPhoto);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                BookModel model = new BookModel()
                {
                    Id = this.Id,
                    Title = txtBook.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Publisher = txtPublisher.Text.Trim(),
                    AuthorId = (int)cboAuthor.SelectedValue,
                    CategoryId = (int)cboCategory.SelectedValue,
                    Year = (int)cboYear.SelectedValue
                };
                if (!string.IsNullOrEmpty(tempImageFilePath) && File.Exists(tempImageFilePath))
                {
                    model.CoverPhoto = $@"{serverFolderPath}\{Path.GetFileName(tempImageFilePath)}";
                }
                executeResponse(this.Id == 0 ? _bookBusiness.Add(model) : _bookBusiness.Update(model));
            }
            else
            {
                ErrorProviderHelper.FocusFirstControl();
            }
        }

        private bool IsValidForm()
        {
            ErrorProviderHelper.ClearError();
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(txtBook.Text.Trim()))
            {
                ErrorProviderHelper.SetErrorMessage(txtBook, "Book Title is required.");
                isValid = false;
            }
            if (string.IsNullOrEmpty(cboAuthor.Text))
            {
                ErrorProviderHelper.SetErrorMessage(cboAuthor, "Author is required.");
                isValid = false;
            }
            if (string.IsNullOrEmpty(cboCategory.Text))
            {
                ErrorProviderHelper.SetErrorMessage(cboCategory, "Category is required.");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(cboYear.Text))
            {
                ErrorProviderHelper.SetErrorMessage(cboYear, "Publisher is required.");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtPublisher.Text.Trim()))
            {
                ErrorProviderHelper.SetErrorMessage(txtPublisher, "Publisher is required.");
                isValid = false;
            }

            return isValid;
        }

        private void executeResponse(ResponseModel response)
        {
            if (response != null)
            {
                switch (response.ResponseCode)
                {
                    case ResponseCode.Success:
                        {
                            if (!string.IsNullOrEmpty(tempImageFilePath) && File.Exists(tempImageFilePath))
                            {
                                string fileName = Path.GetFileName(tempImageFilePath);
                                string serverPhotoFilePath = $@"{serverFolderPath}\{fileName}";

                                byte[] bytes = FileHelper.GetByteFromFile(tempImageFilePath);
                                FileHelper.CopyImage(bytes, serverPhotoFilePath);
                                FileHelper.DeleteFile(tempImageFilePath);
                            }

                            if (this.Save != null)
                            {
                                Save(response);
                            }
                            this.Close();
                        }
                        break;
                    case ResponseCode.NotExist:
                        {
                            MessageBox.Show("Book is not exist in system.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    default:
                        MessageBox.Show("Error\n" + response.Message, MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            else
            {

            }
        }

        private void control_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textbox = (TextBox)sender;
                if (!string.IsNullOrEmpty(textbox.Text.Trim()))
                {
                    ErrorProviderHelper.ClearError(textbox);
                }
            }
            else if (sender is ComboBox)
            {
                ComboBox combobox = (ComboBox)sender;
                if (!string.IsNullOrEmpty(combobox.Text))
                {
                    ErrorProviderHelper.ClearError(combobox);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
