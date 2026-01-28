using Elmah;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SignIn : System.Web.UI.Page
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
                string enCode = Base64Encode("pct1");
                BindServerData();
                BindVersionData();
                if (DataBaseAccess.WindowAuthentication == "1")
                {
                    string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    var arr = domainUser.Split(new char[] { '\\' }, StringSplitOptions.None);
                    txtDomainName.Value = arr[0].ToString();
                    txtUserName.Value = "";//arr[1].ToString();
                    tdDomain.Visible = true;
                }
                if (Page.ClientQueryString.Length > 0)
                {
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["DensoPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Request.QueryString["userID"] != null && Request.QueryString["password"] != null && Request.QueryString["pageName"] != null)
                        {
                            txtUserName.Value = Request.QueryString["userID"].ToString();
                            txtPassword.Value = Base64Decode(Request.QueryString["password"].ToString());
                            hdnPageName.Value = Request.QueryString["pageName"].ToString();
                            Session["HideSideBarMenu"] = "yes";
                            btnLogin_Click(null, null);
                        }
                    }
                    else
                    {
                        if (Request.QueryString["dbname"] != null)
                        {
                            string dbname = Request.QueryString["dbname"].ToString();
                            if (ddlConnectionString.Items.Count > 0 && ddlConnectionString.Items.FindByText(dbname) != null)
                            {
                                ddlConnectionString.SelectedValue = dbname;
                                ddlConnectionString.Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void BindVersionData()
        {
            try
            {
                lblVersionMsg.Text = "";
                lblDBVersion.Text = SoftwareDBVersion.DbVersion;
                lblPackageVersion.Text = SoftwareDBVersion.SoftwareVersion;
                lblScriptName.Text = SoftwareDBVersion.ScriptName;
                DBVersionEntity latestDBData = DataBaseAccess.getLatestDBVersionDetails();

                if (latestDBData.DbVersionNumber.Equals(lblDBVersion.Text, StringComparison.OrdinalIgnoreCase) && latestDBData.ScriptName.Equals(lblScriptName.Text, StringComparison.OrdinalIgnoreCase))
                {
                    lblVersionMsg.Text = "";
                }
                else
                {
                    lblVersionMsg.Text = System.Web.Configuration.WebConfigurationManager.AppSettings["SoftwareVersionMsg"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessage.Text = ex.Message;
            }
        }

        #region "Bind Server Data"
        private void BindServerData()
        {
            try
            {
                List<string> serverData = DataBaseAccess.ServerData();
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

        #region -- license -----
        public static string[] formats = new string[] { "d/MM/yyyy", "d/M/yy", "dd/M/yyyy", "dd-MM-yy", "dd/MM/yy", "d-M-yy", "d-MM-yy", "d/M/yyyy", "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "DD/MM/yyyy", "dd/MMM/yyyy", "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm", "dd-MM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy HH:mm", "dd-MMM-yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "dd-MM-yyyyTHH:mm:ss", "dd-MM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm" };
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        static string appPath = HttpContext.Current.Server.MapPath("");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "LicenseData", reportName);
            return src;
        }
        public string setandgetLincenseExpireDateToExcel()
        {
            string expireDate = "";
            try
            {
                //string enCodeDate = Base64Encode("2021-12-01");
                string templatefile = string.Empty;
                string Filename = "LicenseData.lic";

                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string decodeValue = File.ReadAllText(Source);
                expireDate = Base64Decode(decodeValue);
                if (expireDate != "")
                {
                    DateTime expiredDate;
                    DateTime.TryParseExact(expireDate.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out expiredDate);
                }
            }
            catch (Exception ex)
            {
                expireDate = "";
                Logger.WriteErrorLog(ex.Message);
            }
            return expireDate;
        }

        #endregion
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Session["RemainingDaysForShanti"] = null;
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["LicenseEnable"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    string expireValue = setandgetLincenseExpireDateToExcel();
                    if (expireValue == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "waringModal", "openWarningModal('Software license file not found.')", true);
                        return;
                    }
                    else
                    {
                        DateTime expiredDate;
                        DateTime currentDate;
                        DateTime.TryParseExact(expireValue.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out expiredDate);
                        DateTime.TryParseExact(DateTime.Now.ToString("yyyy-MM-dd"), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out currentDate);
                        if (currentDate >= expiredDate)
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "waringModal", "openWarningModal('Software license expired.')", true);
                            return;
                        }
                        int diffBetweenDate = Convert.ToInt32((expiredDate - currentDate).TotalDays.ToString());
                        int LicensePopUpIntimationDay = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["LicensePopUpIntimation"].ToString());
                        if (diffBetweenDate <= LicensePopUpIntimationDay)
                        {
                            Session["RemainingDaysForShanti"] = diffBetweenDate.ToString();
                        }
                    }
                }
                HttpContext.Current.Session["Language"] = "zh";

                Session["ScreenHeight"] = hdnScreenHeight.Value;
                Session["ScreenWidth"] = hdnScreenWidth.Value;

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
                            Session["Password"] = txtPassword.Value.ToString();
                            if (admindata == "1")
                                Session["AdminData"] = "Admin";
                            else
                                Session["AdminData"] = "NonAdmin";
                            FormsAuthentication.SetAuthCookie(this.txtUserName.Value.Trim(), false);
                            FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, this.txtUserName.Value.Trim(),
                                DateTime.Now, DateTime.MaxValue, false, Session["AdminData"].ToString());
                            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket1));
                            Response.Cookies.Add(cookie1);

                            String returnUrl;
                            string landingPage = DataBaseAccess.BindLandingPage(txtUserName.Value);
                            if (string.IsNullOrEmpty(landingPage))
                            {
                                landingPage = "IonicView.aspx";
                            }
                            if (Request.QueryString["ReturnUrl"] == null)
                            {
                                // returnUrl = "~/Dashboard.aspx";
                                returnUrl = "~/" + landingPage;
                            }
                            else
                            {
                                returnUrl = Request.QueryString["ReturnUrl"];
                            }
                            //Response.Redirect("~/Dashboard.aspx", false);
                            Response.Redirect("~/" + landingPage, false);
                            //Response.Redirect("~/IconicViewData.aspx", false);
                        }
                    }
                    else
                    {
                        string UserValid = DataBaseAccess.CheckEmployeeDetail(txtUserName.Value, txtPassword.Value);
                        if (!string.IsNullOrWhiteSpace(UserValid))
                        {
                            if (UserValid == "1")
                                Session["AdminData"] = "Admin";
                            else
                                Session["AdminData"] = "NonAdmin";

                            Session["UserName"] = txtUserName.Value.ToString();
                            Session["Password"] = txtPassword.Value.ToString();
                            FormsAuthentication.SetAuthCookie(this.txtUserName.Value.Trim(), false);
                            FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(1, this.txtUserName.Value.Trim(),
                                DateTime.Now, DateTime.MaxValue, false, "Admin");
                            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName,
                               FormsAuthentication.Encrypt(ticket1));
                            Response.Cookies.Add(cookie1);
                            String returnUrl;
                            string landingPage = DataBaseAccess.BindLandingPage(txtUserName.Value);
                            if (hdnPageName.Value != "")
                            {
                                landingPage = hdnPageName.Value;
                            }
                            if (string.IsNullOrEmpty(landingPage))
                            {
                                landingPage = "IonicView.aspx";
                            }
                            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                            {
                                //returnUrl = "~/EmployeeInformation.aspx";
                                returnUrl = "~/" + landingPage;
                            }
                            else
                            {
                                returnUrl = Request.QueryString["ReturnUrl"];
                            }
                            // Response.Redirect("~/EmployeeInformation.aspx", false);
                            Response.Redirect("~/" + landingPage, false);
                        }
                        else
                        {
                            SuccessMessage = GetGlobalResourceObject("CommanResource", "LoginCondition").ToString();
                            pnlMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                            Session["HideSideBarMenu"] = null;
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

        protected void ddlConnectionString_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["connectionString"] = ddlConnectionString.SelectedValue;
            BindVersionData();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getHideSideBarMenuValue()
        {
            string hideMenu = "";
            try
            {
                if (HttpContext.Current.Session["HideSideBarMenu"] != null)
                {
                    hideMenu = HttpContext.Current.Session["HideSideBarMenu"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return hideMenu;
        }
    }
}