using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;
using Web_TPMTrakDashboard.Models;
using System.DirectoryServices;

namespace Web_TPMTrakDashboard
{

    public partial class Login : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }
        AuthenticationTypes AuthenticationType;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindServerData();
                if (DataBaseAccess.WindowAuthentication == "1")
                {
                    string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    var arr = domainUser.Split(new char[] { '\\' }, StringSplitOptions.None);
                    txtDomainName.Value = arr[0].ToString();
                    txtUserName.Value = arr[1].ToString();
                    tdDomain.Visible = true;
                }
            }
        }

        #region "Bind Server Data"
        private void BindServerData()
        {
            try
            {
                List<string> serverData = DataBaseAccess.ServerData();
                //string[] result = serverData.Split(new string[] { "," }, StringSplitOptions.None);
                ddlConnectionString.DataSource = serverData;
                ddlConnectionString.DataBind();
                Session["connectionString"] = ddlConnectionString.SelectedValue;				

			}
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessage.Text = ex.Message;
            }
        }
        #endregion


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {				
				HttpContext.Current.Session["Language"] = "zh";              
                bool isUserValid = false;
                if (!string.IsNullOrEmpty(txtUserName.Value) && !string.IsNullOrEmpty(txtPassword.Value))
                {					
					Session["connectionString"] = ddlConnectionString.SelectedValue;
                    if (DataBaseAccess.WindowAuthentication == "1")
                    {
                        string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        var arr = domainUser.Split(new char[] { '\\' }, StringSplitOptions.None);
                        //this.AutenticateUser(arr[0].ToString(), txtUserName.Text, txtPassword.Text);
                        isUserValid = IsExistInAD(txtUserName.Value, txtPassword.Value, arr[0].ToString());

                        if (isUserValid == false)
                        {
                            SuccessMessage = GetGlobalResourceObject("CommanResource", "LoginCondition").ToString();
                            pnlMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                            txtDomainName.Value = arr[0].ToString();
                        }

                        if (isUserValid)
                        {
                            string admindata = DataBaseAccess.CheckEmployeeExists(txtUserName.Value);
                            Session["UserName"] = txtUserName.Value.ToString();
                            if (admindata == "1")
                                Session["AdminData"] = "Admin";
                            else
                                Session["AdminData"] = "NonAdmin";
                            FormsAuthentication.SetAuthCookie(this.txtUserName.Value.Trim(), false);
                            FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, this.txtUserName.Value.Trim(),
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
                            Response.Redirect("~/Dashboard.aspx", false);
                            //Response.Redirect("~/IconicViewData.aspx", false);
                        }
                    }
                    else
                    {
                        string UserValid = DataBaseAccess.CheckEmployeeDetail(txtUserName.Value, txtPassword.Value);
                        if (!string.IsNullOrWhiteSpace(UserValid))
                        {
                            if (UserValid=="1")
                                Session["AdminData"] = "Admin";
                            else
                                Session["AdminData"] = "NonAdmin";

                            Session["UserName"] = txtUserName.Value.ToString();
                            FormsAuthentication.SetAuthCookie(this.txtUserName.Value.Trim(), false);
                            FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, this.txtUserName.Value.Trim(),
                                DateTime.Now, DateTime.Now.AddMinutes(10), false, "Admin");
                            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName,
                               FormsAuthentication.Encrypt(ticket1));
                            Response.Cookies.Add(cookie1);
                            String returnUrl; 

                            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                            {
                                returnUrl = "~/Dashboard.aspx";
                            }
                            else
                            {
                                returnUrl = Request.QueryString["ReturnUrl"];
                            }
                            Response.Redirect("~/Dashboard.aspx", false);
                        }
                        else
                        {
                            SuccessMessage = GetGlobalResourceObject("CommanResource", "LoginCondition").ToString();
                            pnlMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                        }
                    }
                    Session["Language"] = ddlLanguage.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessage.Text = ex.Message;
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