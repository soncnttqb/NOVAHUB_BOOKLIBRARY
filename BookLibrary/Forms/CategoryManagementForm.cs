
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
using Business.Models;
using Business.Bussiness;
using Business.Utilities;
using static Business.Utilities.Enums;

namespace BookLibrary.Forms
{
    public partial class CategoryManagementForm : Form
    {
        public delegate void SaveFormHandler(ResponseModel model);
        public event SaveFormHandler Save;
        public int Id { get; set; }
        private CategoryBusiness _categoryBusienss = new CategoryBusiness();
        public CategoryManagementForm()
        {
            InitializeComponent();
        }

        private void setTitleForForm()
        {
            this.Text = this.Id == 0 ? "Add Category" : "Edit Category";
        }

        private void CategoryManagementForm_Load(object sender, EventArgs e)
        {
            setTitleForForm();
            if (this.Id == 0) return;
            CategoryModel model = _categoryBusienss.Get(this.Id);
            if (model != null)
            {
                txtTitle.Text = model.Title;
                txtDescription.Text = model.Description;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                CategoryModel model = new CategoryModel() { Id = this.Id, Title = txtTitle.Text.Trim(), Description = txtDescription.Text.Trim() };

                executeResponse(this.Id == 0 ? _categoryBusienss.Add(model) : _categoryBusienss.Update(model));
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
                ErrorProviderHelper.SetErrorMessage(txtTitle, "Category is required.");
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
                    case ResponseCode.Duplicate:
                        {
                            MessageBox.Show("Category is exist in system.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case ResponseCode.Success:
                        {
                            if (this.Save != null)
                            {
                                Save(response);
                            }
                            this.Close();
                        }
                        break;
                    case ResponseCode.NotExist:
                        {
                            MessageBox.Show("Category is not exist in system.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    case ResponseCode.InUse:
                        {
                            MessageBox.Show("Category is used in system.", MessageBoxCaption.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        
        private void control_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (!string.IsNullOrEmpty(textbox.Text.Trim()))
            {
                ErrorProviderHelper.ClearError(textbox);
            }
        }
        
    }
}
