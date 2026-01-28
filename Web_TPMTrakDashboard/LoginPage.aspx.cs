using Elmah;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class LoginPage : System.Web.UI.Page
    {
        AuthenticationTypes AuthenticationType;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (DataBaseAccess.WindowAuthentication == "1")
                    {
                        string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        var arr = domainUser.Split(new char[] { '\\' }, StringSplitOptions.None);
                        txtDomainName.Text = arr[0].ToString();
                        txtUserName.Text = arr[1].ToString();
                        tdDomain.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            try
            {
                bool isUserValid = false;
                if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text))
                {
                    if (DataBaseAccess.WindowAuthentication == "1")
                    {
                        string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        var arr = domainUser.Split(new char[] { '\\' }, StringSplitOptions.None);
                        //this.AutenticateUser(arr[0].ToString(), txtUserName.Text, txtPassword.Text);
                        isUserValid = IsExistInAD(txtUserName.Text, txtPassword.Text, arr[0].ToString());

                        if (isUserValid == false)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please Enter Valid Credentials.";
                            txtDomainName.Text = arr[0].ToString();
                        }

                        if (isUserValid)
                        {
                            string admindata = DataBaseAccess.CheckEmployeeExists(txtUserName.Text);
                            Session["UserName"] = txtUserName.Text.ToString();
                            if (admindata == "1")
                                Session["AdminData"] = "Admin";
                            else
                                Session["AdminData"] = "NonAdmin";
                            FormsAuthentication.SetAuthCookie(this.txtUserName.Text.Trim(), false);
                            FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, this.txtUserName.Text.Trim(),
                                DateTime.Now, DateTime.Now.AddMinutes(10), false, Session["AdminData"].ToString());
                            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket1));
                            Response.Cookies.Add(cookie1);

                            String returnUrl;
                            if (Request.QueryString["ReturnUrl"] == null)
                            {
                                returnUrl = "~/Dashboard.aspx";
                            }
                            else
                            {
                                returnUrl = Request.QueryString["ReturnUrl"];
                            }
                            Response.Redirect(returnUrl, false);
                            //Response.Redirect("~/IconicViewData.aspx", false);
                        }
                    }
                    else
                    {
                        string UserValid = DataBaseAccess.CheckEmployeeDetail(txtUserName.Text, txtPassword.Text);
                        if (!string.IsNullOrWhiteSpace(UserValid))
                        {
                            if (UserValid == "1")
                                Session["AdminData"] = "Admin";
                            else
                                Session["AdminData"] = "NonAdmin";

                            Session["UserName"] = txtUserName.Text.ToString();
                            FormsAuthentication.SetAuthCookie(this.txtUserName.Text.Trim(), false);
                            FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, this.txtUserName.Text.Trim(),
                                DateTime.Now, DateTime.Now.AddMinutes(10), false, "Admin");
                            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName,
                               FormsAuthentication.Encrypt(ticket1));
                            Response.Cookies.Add(cookie1);
                            String returnUrl;
                            if (Request.QueryString["ReturnUrl"] == null)
                            {
                                returnUrl = "~/Dashboard.aspx";
                            }
                            else
                            {
                                returnUrl = Request.QueryString["ReturnUrl"];
                            }
                            Response.Redirect(returnUrl, false);
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Please Enter Valid Credentials.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessage.Text = ex.Message;
            }
        }

        bool IsExistInAD(string loginName, string password, string domain)
        {
            bool isUserValid = false;
            isUserValid = LoginToDomain(loginName, password, domain);
            return isUserValid;
        }

        private bool LoginToDomain(string userName, string password, string domain)
        {
            SetAuthenticationType(false);

            domain = "LDAP://" + domain;
            using (DirectoryEntry deDirEntry = new DirectoryEntry(domain, userName, password, AuthenticationType))
            {
                try
                {
                    if (deDirEntry.Name != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.Message.ToString());
                    return false;
                }
            }
            return false;
        }

        public void SetAuthenticationType(bool bValue)
        {
            if (bValue)
            {
                AuthenticationType = AuthenticationTypes.SecureSocketsLayer;
            }
            else
            {
                AuthenticationType = AuthenticationTypes.Secure;
            }
        }
    }
}