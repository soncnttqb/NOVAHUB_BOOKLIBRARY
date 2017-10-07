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
    public partial class AuthorManagementForm : Form
    {
        public delegate void SaveFormHandler(ResponseModel model);
        public event SaveFormHandler Save;
        public int Id { get; set; }
        private AuthorBusiness _authorBusiness = new AuthorBusiness();
        private string tempImageFilePath;
        private string serverFolderPath;
        public AuthorManagementForm()
        {
            InitializeComponent();
        }

        private void setTitleForForm()
        {
            this.Text = this.Id == 0 ? "Add Author" : "Edit Author";
        }

        private void AuthorManagementForm_Load(object sender, EventArgs e)
        {
            serverFolderPath = _authorBusiness.GetFolderImagePath();
            FileHelper.CreateFolderIfNotExist(FileHelper.TempFolderPath);
            FileHelper.CreateFolderIfNotExist(serverFolderPath);

            setTitleForForm();
            if (this.Id == 0) return;

            AuthorModel model = _authorBusiness.Get(this.Id);
            if (model != null)
            {
                txtTitle.Text = model.Title;
                txtDescription.Text = model.Description;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                AuthorModel model = new AuthorModel() { Id = this.Id, Title = txtTitle.Text.Trim(), Description = txtDescription.Text.Trim() };
                if (!string.IsNullOrEmpty(tempImageFilePath) && File.Exists(tempImageFilePath))
                {
                    model.CoverPhoto = $@"{serverFolderPath}\{Path.GetFileName(tempImageFilePath)}";
                }
                executeResponse(this.Id == 0 ? _authorBusiness.Add(model) : _authorBusiness.Update(model));
            }
            else
            {
                ErrorProviderHelper.FocusFirstControl();
            }
        }

        private bool IsValidForm()
        {
            ErrorProviderHelper.ClearError();
            if (string.IsNullOrWhiteSpace(txtTitle.Text.Trim()))
            {
                ErrorProviderHelper.SetErrorMessage(txtTitle, "Author is required.");
                return false;
            }
            return true;
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
                            MessageBox.Show("Author is not exist in system.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case ResponseCode.InUse:
                        {
                            MessageBox.Show("Author is used in system.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                ErrorProviderHelper.ClearError(txtTitle);
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

    }
}
