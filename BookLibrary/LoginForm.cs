using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Business;
using BookLibrary.Forms;
using Business.Utilities;
using Business.Bussiness;
using Business.Models;

namespace BookLibrary
{
    public partial class LoginForm : Form
    {
        private UserBusiness _userBusiness = new UserBusiness();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.IsValidForm())
            {
                GenericPrincipal principal = _userBusiness.Login(new UserModel() { Email = txtUsername.Text.Trim(), Password = txtPassword.Text.Trim() });
                if (principal != null)
                {
                    System.Threading.Thread.CurrentPrincipal = principal;
                    this.Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                }
                else
                {
                    Thread.CurrentPrincipal = null;
                    MessageBox.Show(Constants.ErrorLogin, Enums.MessageBoxCaption.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                }
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
            if (string.IsNullOrWhiteSpace(txtUsername.Text.Trim()))
            {
                ErrorProviderHelper.SetErrorMessage(txtUsername, "Username is required.");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text.Trim()))
            {
                ErrorProviderHelper.SetErrorMessage(txtPassword, "Password is required.");
                isValid = false;
            }
            return isValid;
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
