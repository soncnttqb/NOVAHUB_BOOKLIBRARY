using Business;
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
using static Business.Enums;

namespace BookLibrary
{
    public partial class BookManagementForm : Form
    {
        public delegate void SaveFormHandler(ResponseModel model);
        public event SaveFormHandler Save;
        public int Id;
        //folder temp to storage image in local
        private string tempFolder = ConfigurationManager.AppSettings[Constants.ConfigKey.TempFolder].ToString();
        // folder server to storage image 
        private string serverFolder = ConfigurationManager.AppSettings[Constants.ConfigKey.ServerImageFolder].ToString();
        private string tempImageFilePath;
        public BookManagementForm()
        {
            InitializeComponent();
        }

        private void BookManagementForm_Load(object sender, EventArgs e)
        {
            // create folder to storage image
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            if (!Directory.Exists(serverFolder))
                Directory.CreateDirectory(serverFolder);

            cboAuthor.DisplayMember = "DisplayMember";
            cboAuthor.ValueMember = "ValueMember";
            cboAuthor.DataSource = AuthorBusiness.GetSelectListAuthor();
            cboCategory.DisplayMember = "DisplayMember";
            cboCategory.ValueMember = "ValueMember";
            cboCategory.DataSource = CategoryBusiness.GetSelectListCategory();

            cboYear.DisplayMember = "DisplayMember";
            cboYear.ValueMember = "ValueMember";
            cboYear.DataSource = Utils.GetListYears();

            if (this.Id == 0)
            {
                this.Text = "Add Book";
            }
            else
            {
                this.Text = "Edit Book";
                BookModel model = BookBusiness.Get(this.Id);
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
                        //Load image
                        try
                        {
                            FileStream fs = new FileStream(model.CoverPhoto, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            var bytes = new byte[fs.Length];
                            fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                            picCoverPhoto.Image = Image.FromStream(fs);
                            fs.Close();

                            // copy image to temp
                            string fileName = Path.GetFileName(model.CoverPhoto);
                            tempImageFilePath = string.Format(@"{0}\{1}", tempFolder, fileName);

                            if (File.Exists(tempImageFilePath))
                                File.Delete(tempImageFilePath);

                            System.IO.FileStream _fileStream = new System.IO.FileStream(tempImageFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                            _fileStream.Write(bytes, 0, bytes.Length);
                            _fileStream.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
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

                string fileName = Path.GetFileName(file.FileName);
                if (!string.IsNullOrEmpty(tempImageFilePath) && File.Exists(tempImageFilePath))
                {
                    File.Delete(tempImageFilePath);
                }
                tempImageFilePath = string.Format(@"{0}\{1}{2}", tempFolder, Guid.NewGuid().ToString("N"), extend);

                //load image to picturebox
                FileStream fs = new FileStream(file.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                picCoverPhoto.Image = Image.FromStream(fs);
                fs.Close();
                //copy file to tempfolder
                System.IO.FileStream _fileStream = new System.IO.FileStream(tempImageFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                _fileStream.Write(bytes, 0, bytes.Length);
                _fileStream.Close();
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
                    model.CoverPhoto = string.Format(@"{0}\{1}", serverFolder, Path.GetFileName(tempImageFilePath));
                }
                executeResponse(this.Id == 0 ? BookBusiness.Add(model) : BookBusiness.Update(model));
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
                switch (response.Message)
                {
                    case "Success":
                        {
                            if (!string.IsNullOrEmpty(tempImageFilePath) && File.Exists(tempImageFilePath))
                            {
                                string fileName = Path.GetFileName(tempImageFilePath);
                                string serverPhotoFilePath = string.Format(@"{0}\{1}", serverFolder, fileName);

                                //copy file to image folder
                                FileStream fs = new FileStream(tempImageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                var bytes = new byte[fs.Length];
                                fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                                fs.Close();

                                System.IO.FileStream _fileStream = new System.IO.FileStream(serverPhotoFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                                _fileStream.Write(bytes, 0, bytes.Length);
                                _fileStream.Close();

                                File.Delete(tempImageFilePath);
                            }

                            if (this.Save != null)
                            {
                                Save(response);
                            }
                            this.Close();
                        }
                        break;
                    case "NotExist":
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

        private void control_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }

        private void BookManagementForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
