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
    public partial class AuthorManagementForm : Form
    {
        public delegate void SaveFormHandler(ResponseModel model);
        public event SaveFormHandler Save;
        public int Id;
        //folder temp to storage image in local
        private string tempFolder = ConfigurationManager.AppSettings[Constants.ConfigKey.TempFolder].ToString();
        // folder server to storage image 
        private string serverFolder = ConfigurationManager.AppSettings[Constants.ConfigKey.ServerImageFolder].ToString();
        private string tempImageFilePath;

        public AuthorManagementForm()
        {
            InitializeComponent();
        }
        
        private void AuthorManagementForm_Load(object sender, EventArgs e)
        {
            // create folder to storage image
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            if (!Directory.Exists(serverFolder))
                Directory.CreateDirectory(serverFolder);

            if (this.Id == 0)
            {
                this.Text = "Add Author";
            }
            else
            {
                this.Text = "Edit Author";
                AuthorModel model = AuthorBusiness.Get(this.Id);
                if (model != null)
                {
                    txtTitle.Text = model.Title;
                    txtDescription.Text = model.Description;

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
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private string CoppyImageToTemp(string coverPhoto)
        {
            try
            {
                string tempFile = string.Format(@"{0}\{1}", tempFolder, Guid.NewGuid().ToString("N"));
                File.Copy(coverPhoto, tempFile);
                return tempFile;
            }
            catch
            {
                return string.Empty;
            }
        }
        private string CoppyImageToActive(string coverPhoto)
        {
            try
            {
                string tempFile = string.Format(@"{0}\{1}", tempFolder, Guid.NewGuid().ToString("N"));
                File.Copy(coverPhoto, tempFile);
                return tempFile;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                AuthorModel model = new AuthorModel() { Id = this.Id, Title = txtTitle.Text.Trim(), Description = txtDescription.Text.Trim() };
                if(!string.IsNullOrEmpty(tempImageFilePath) && File.Exists(tempImageFilePath))
                {
                    model.CoverPhoto= string.Format(@"{0}\{1}", serverFolder, Path.GetFileName(tempImageFilePath));
                }
                executeResponse(this.Id == 0 ? AuthorBusiness.Add(model) : AuthorBusiness.Update(model));
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
                            MessageBox.Show("Author is not exist in system.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case "InUse":
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

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text.Trim()))
            {
                errorProviderAuthor.SetError(txtTitle, "Author is required.");
                e.Cancel = true;
            }
            else
            {
                errorProviderAuthor.SetError(txtTitle, string.Empty);
                e.Cancel = false;
            }
        }

        private void AuthorManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            AutoValidate = AutoValidate.Disable;
        }

        private void txtTitle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
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

        private void AuthorManagementForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
