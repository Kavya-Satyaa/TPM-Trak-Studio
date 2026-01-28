using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.WebAndon
{
    public partial class AndonMaster : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        public SettingsGUI settings = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            //txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] == null || Session["connectionString"] == null)
                    Response.Redirect("../SignIn.aspx", false);
                else
                {
                    settings = new SettingsGUI();
                    if (!IsPostBack)
                    {
                        BindShiftData();
                        // Set Anti-XSRF token
                        ddlPlantName.Visible = false;
                        txtDate.Visible = false; 
                        //ddlShift.Visible = false;
                        btnProcess.Visible = false;
                        //txtToDate.Visible = false;

                        if (Session["Mode"] != null && Session["Mode"].ToString().Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                        {
                            Session["Mode"] = "DESKTOP";
                            btnToggel.Text = "Swich to ANDON Mode";
                            ddlPlantName.AutoPostBack = true;
                            ddlLine.AutoPostBack = false;
                            //ddlShift.AutoPostBack = true;
                        }
                        else
                        {
                            Session["Mode"] = "ANDON";
                            btnToggel.Text = "Swich to DESKTOP Mode";
                            ddlPlantName.AutoPostBack = true;
                            ddlLine.AutoPostBack = false;
                            //ddlShift.AutoPostBack = false;
                        }
                        //ddlShift.SelectedValue =ANDON
                        //BindCockpitView.GetShift();
                        //string shiftdate = BindCockpitView.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
                        //DateTime date = DateTime.Now;
                        //DateTime.TryParse(shiftdate, out date);
                        //txtDate.Text = date.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);               
                        ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                        ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
                        BindPlantId();
                        ddlPlantName.SelectedValue = Session["PlantId"] == null ? settings.AppUISettings.PlantToDisplay : Session["PlantId"].ToString();

                        //const string imagesPath = "~/CompanyLogo/";// "~/Image/Slideshow/";
                        //var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

                        ////filtering to jpgs, but ideally not required
                        //List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                        //if (fileNames.Count > 0)
                        //{
                        //    Image1.ImageUrl = imagesPath + fileNames[0];
                        //}
                        //else
                        //{
                        //    Image1.ImageUrl = "Image/companyIcon.png";
                        //}
                        Image1.ImageUrl = Util.getCompanyLogoPath();
                    }
                    else
                    {
                        // Validate the Anti-XSRF token
                        if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                            || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                        {
                            throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ddlPlantName.Visible = false;
                txtDate.Visible = false; fristDate.Visible = false;
                //ddlShift.Visible = false;
                ddlLine.Visible = false;
                btnProcess.Visible = false;
                lblErrorMsg.Text = ex.Message;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            // Context.GetOwinContext().Authentication.SignOut();
        }

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantName.DataSource = lstPlantData;
                ddlPlantName.DataBind();
                ddlPlantName.Items.Insert(0, new ListItem
                {
                    Text = "Plant All",
                    Value = "All"
                });
                ddlPlantName_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ddlPlantName.Visible = false;
                txtDate.Visible = false; fristDate.Visible = false;
                //ddlShift.Visible = false;
                btnProcess.Visible = false;
                ddlLine.Visible = false;
                lblErrorMsg.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Shift Data------------------------"
        private void BindShiftData()
        {
            try
            {
                //var allShift = AndonCockpitView.GetAllPredefinedShifts();
                //if (allShift != null && allShift.Count > 0)
                //{
                //    ddlShift.DataSource = allShift;
                //    ddlShift.DataBind();
                //}
                //int count = ddlShift.Items.Count;
                //string DefaultShift = settings.AppUISettings.DefaultPredefinedTimePeriod == null ? "CurrentShift" : settings.AppUISettings.DefaultPredefinedTimePeriod;
                //if (DefaultShift.Equals("CurrentShift", StringComparison.OrdinalIgnoreCase))
                //{
                //    string shiftName = AndonCockpitView.CurrentShiftTime();
                //    shiftName = "Today - " + shiftName;
                //    ddlShift.SelectedValue = shiftName;
                //}
                //else if (DefaultShift.Equals("Today", StringComparison.OrdinalIgnoreCase))
                //    ddlShift.SelectedIndex = 3;
                //else if (DefaultShift.Equals("Yesterday", StringComparison.OrdinalIgnoreCase))
                //{
                //    if (count > 0)
                //        ddlShift.SelectedIndex = count - 1;
                //}

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ddlPlantName.Visible = false;
                txtDate.Visible = false; fristDate.Visible = false;
                //ddlShift.Visible = false;
                ddlLine.Visible = false;
                btnProcess.Visible = false;
                lblErrorMsg.Text = ex.Message;
            }
        }
        #endregion

        #region "Toggle Button Click Event--------"
        protected void btnToggel_Click(object sender, EventArgs e)
        {
            try
            {
                string text = btnToggel.Text;
                if (text.Equals("Swich to ANDON Mode", StringComparison.OrdinalIgnoreCase))
                {
                    Session["Mode"] = "ANDON";
                    btnToggel.Text = "Swich to DESKTOP Mode";
                }
                else
                {
                    Session["Mode"] = "DESKTOP";
                    btnToggel.Text = "Swich to ANDON Mode";
                }
                Response.Redirect(Request.Url.OriginalString);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ddlPlantName.Visible = false;
                txtDate.Visible = false; fristDate.Visible = false;
               // ddlShift.Visible = false;
                ddlLine.Visible = false;
                btnProcess.Visible = false;
                lblErrorMsg.Text = ex.Message;
            }
        }
        #endregion

        protected void ddlPlantName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string plant = ddlPlantName.SelectedValue.ToString() == "All" ? "" : ddlPlantName.SelectedValue.ToString();
            List<string> LineID = BindCockpitView.ViewCellsToDisplay(plant);
            LineID.Insert(0, "LineAll");
            if(LineID!=null && LineID.Count>0)
            {
                ddlLine.DataSource = LineID;
                ddlLine.DataBind();
            }
        }
    }
}