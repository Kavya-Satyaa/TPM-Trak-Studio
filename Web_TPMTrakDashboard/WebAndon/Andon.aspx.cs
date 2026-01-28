using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.WebAndon
{
    public partial class AndonViewPage : System.Web.UI.Page
    {
        List<string> listOfColNames = new List<string>();
        public SettingsGUI settings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] == null || Session["connectionString"] == null)
                    Response.Redirect("../SignIn.aspx", false);
                else
                {
                    DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                    DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlPlantName");
                    TextBox txtDate = (TextBox)Page.Master.FindControl("txtDate");
                    //DropDownList drpShift = (DropDownList)Page.Master.FindControl("ddlShift");
                    Button btnProcess = (Button)Page.Master.FindControl("btnProcess");
                    // TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");
                    drpPlantName.Visible = true;
                    if (Session["UserName"] == null || !Request.IsAuthenticated)
                    {
                        Session["UserName"] = AndonCockpitView.GetDefaultANDONUser();//SATYA
                    }
                    settings = new SettingsGUI();
                    //else
                    //{
                    if (Session["Mode"] != null && Session["Mode"].ToString().Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                    {
                        //txtToDate.Visible = true;
                        txtDate.Visible = true;
                        //drpShift.Visible = true;
                        btnProcess.Visible = true;
                        btnProcess.Click += new EventHandler(btnProcess_Click);
                       // drpShift.SelectedIndexChanged += new EventHandler(ddlDayShift_SelectedIndexChanged);
                    }
                    else
                    {
                        //txtToDate.Visible = false;
                        txtDate.Visible = false;
                        //drpShift.Visible = false;
                        btnProcess.Visible = false;
                        drpPlantName.SelectedIndexChanged += new EventHandler(drp_SelectedIndexChanged);
                    }
                    if (!IsPostBack)
                    {
                        Session["Mode"] = hdfMode.Value = Session["Mode"] == null ? "DESKTOP" : Session["Mode"].ToString().ToUpper();
                        if (Session["Mode"] != null && Session["Mode"].ToString().Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                        {
                            ddlDayShift_SelectedIndexChanged("frist", null);
                        }
                        BindData();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        #region "Day Shift Wise change Data"
        protected void ddlDayShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //TextBox txtFromDate = (TextBox)Page.Master.FindControl("txtDate");
                //DropDownList ddlDayShift = (DropDownList)Page.Master.FindControl("ddlShift");
                //DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlLine");
                //TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");
                //string selectedShift = ddlDayShift.SelectedValue.ToString();
                //string logicalDayStart = string.Empty, logicalDayEnd = string.Empty;
                //if (selectedShift.Contains("Today"))
                //{
                //    if (ddlDayShift.SelectedValue.Equals("Today - All"))
                //    {
                //        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                //        //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                //        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //        //txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                //        //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                //    }

                //    else
                //    {
                //        int index = selectedShift.IndexOf('-');
                //        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                //        if (shift != null)
                //        {
                //            logicalDayStart = shift[0];
                //            logicalDayEnd = shift[1];

                //            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                //            //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                //            //txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                //            //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                //        }
                //    }
                //}
                //else
                //{

                //    if (ddlDayShift.SelectedValue.Equals("Yesterday - All"))
                //    {
                //        logicalDayStart = CockpitDataBaseAccess.GetLogicalDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                //        txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                //        //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                //        logicalDayEnd = CockpitDataBaseAccess.GetLogicalDayEnd(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                //        //txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                //        //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                //    }

                //    else
                //    {
                //        int index = selectedShift.IndexOf('-');
                //        var shift = CockpitDataBaseAccess.GetCurrentShiftTime(selectedShift.Substring(index + 1).Trim());
                //        if (shift != null)
                //        {
                //            logicalDayStart = shift[0];
                //            logicalDayEnd = shift[1];

                //            txtFromDate.Text = Convert.ToDateTime(logicalDayStart).ToString("dd-MM-yyyy HH:mm");
                //            //dtpTimeFrom.Value = Convert.ToDateTime(logicalDayStart);

                //            //txtToDate.Text = Convert.ToDateTime(logicalDayEnd).ToString("dd-MM-yyyy HH:mm");
                //            //dtpTimeTo.Value = Convert.ToDateTime(logicalDayEnd);
                //        }
                //    }
                //}
                //if (!sender.ToString().Equals("frist", StringComparison.OrdinalIgnoreCase))
                //    BindData();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                if (drpPlantName != null)
                {
                    Session["PlantId"] = drpPlantName.SelectedValue.ToString();
                    Session["LineID"] = drpLine.SelectedValue.ToString();
                    hdfGroupId.Value = drpLine.SelectedValue.ToString();
                }
                BindData();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        void drp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList drp = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                if (drp != null && drpLine !=null)
                {
                    Session["PlantId"] = drp.SelectedValue.ToString();
                    Session["LineID"] = drpLine.SelectedValue.ToString();
                    hdfGroupId.Value = drpLine.SelectedValue.ToString();
                }

                BindData();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindData()
        {
            try
            {
                string strDate = string.Empty, shift = string.Empty, endDate = string.Empty;
                DateTime shiftDate = DateTime.Now.Date;
                DateTime toDate = DateTime.Now.Date;
                string guiMode = "DesktopMode";
                if (Session["Mode"] != null && Session["Mode"].ToString().Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                {

                    TextBox txtDate = (TextBox)Page.Master.FindControl("txtDate");
                    DropDownList drpShift = (DropDownList)Page.Master.FindControl("ddlShift");

                    endDate = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");
                    if (string.IsNullOrWhiteSpace(txtDate.Text))
                    {

                        strDate = VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        txtDate.Text = Convert.ToDateTime(strDate).ToString("dd-MM-yyyy HH:mm");

                        //endDate = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //txtToDate.Text = Convert.ToDateTime(endDate).ToString("dd-MM-yyyy HH:mm");

                    }
                    //else
                    //{
                    //    strDate = txtDate.Text == "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : txtDate.Text.Trim();
                    //    endDate = txtToDate.Text == "" ? DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") : txtToDate.Text.Trim();

                    //}
                    shiftDate = Util.GetDateTime(endDate);
                    toDate = Util.GetDateTime(endDate);


                    strDate = shiftDate.ToSQLDateTimeFormat();
                    endDate = toDate.ToSQLDateTimeFormat();

                    int index = drpShift.SelectedValue.ToString().IndexOf('-');
                    string shft = drpShift.SelectedValue.ToString().Substring(index + 1);
                    if (shft.Equals("All", StringComparison.OrdinalIgnoreCase))
                        shift = "";
                    else
                        shift = shft;
                    if (shift.Equals("All", StringComparison.OrdinalIgnoreCase))
                        shift = "";
                }
                else
                {
                    guiMode = "AndonMode";

                    strDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    endDate = "";
                    //strDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    shift = "";
                }

                //List<string> val = AndonCockpitView.GetOrderedLabels(out listOfColNames, Session["UserName"] == null ? "none" : Session["UserName"].ToString());//
                List<string> val = CockpitDataBaseAccess.GetOrderedLabels(out listOfColNames, "WebIonicViewAndon", Session["Language"] == null ? "en" : Session["Language"].ToString());
                DropDownList drpPlantName = (DropDownList)Page.Master.FindControl("ddlPlantName");
                DropDownList drpLine = (DropDownList)Page.Master.FindControl("ddlLine");
                if (drpPlantName != null && drpLine != null)
                {
                    List<CockpitData> COCKPIT_DATA = new List<CockpitData>();
                    var LineID = drpLine.SelectedValue.ToString();
                    var selectValue = drpPlantName.SelectedValue.ToString();
                    if (selectValue == "All")
                        selectValue = "";
                    if (LineID.Equals("All", StringComparison.OrdinalIgnoreCase) || LineID.Equals("LineAll", StringComparison.OrdinalIgnoreCase))
                        LineID = "";
                    if (AndonCockpitView.GetCompany == "1")
                    {
                        ColorUISetting colorSetting = settings.ColorUISetting;
                        if (guiMode == "AndonMode")
                        {
                            strDate = VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            endDate = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (CheckDateRange())
                        {
                            COCKPIT_DATA = AndonCockpitView.GetMachineCockpitData("s_GetCockpitData_WithTempTable_eshopx", strDate, endDate, selectValue, LineID, "", listOfColNames, val, "", colorSetting);
                        }
                        else
                        {
                            COCKPIT_DATA = AndonCockpitView.GetMachineCockpitData("s_GetCockpitData_eshopx", strDate, endDate, selectValue, LineID, "", listOfColNames, val, "", colorSetting);
                        }
                    }
                    LstCustomers.DataSource = COCKPIT_DATA;
                    LstCustomers.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private bool CheckDateRange()
        {
            bool isDateInRange = false;
            try
            {
                TextBox txtFromDate = (TextBox)Page.Master.FindControl("txtDate");
                //TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");

                DateTime dt1 = DateTime.Now.Date;
                dt1 = Util.GetDateTime(txtFromDate.Text);
                //Convert.ToDateTime(txtFromDate.Text, Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                DateTime dt2 = DateTime.Now.Date;
                //  Convert.ToDateTime(txtToDate.Text, Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                dt2 = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtFromDate.Text));
                var hours = (dt2 - dt1).TotalHours;

                if (Math.Abs(hours) <= 48)
                {
                    isDateInRange = true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
            return isDateInRange;
        }

        //Data refresh event - called from javascript
        protected void btnLoadPageEvent_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    if (hdfMode.Value != "" && hdfMode.Value.Equals("ANDON", StringComparison.OrdinalIgnoreCase))
                    {
                        DropDownList drp = (DropDownList)Page.Master.FindControl("ddlPlantName");
                        if (drp != null)
                        {
                            if (AndonCockpitView.GetRotate == "1")
                            {
                                int selectedIndex = drp.SelectedIndex;
                                int itemLength = drp.Items.Count;

                                //if video not enabled
                                if (settings.IconicUISetting.EnableImageVideo == "0")
                                {
                                    if (drp.SelectedIndex + 1 >= itemLength)
                                    {
                                        selectedIndex = 0;
                                    }
                                }

                                if (selectedIndex + 1 < itemLength)
                                {
                                    drp.SelectedIndex = selectedIndex + 1;
                                    Session["PlantId"] = drp.SelectedValue.ToString();

                                    //BindData();
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBegin()", true);
                                }
                                else
                                {
                                    // Enable DashBoard Page--------

                                    //if (settings.IconicUISetting.EnableDashBoard == "1")
                                    //{
                                    //    if (BindCockpitView.GetEnergyDasboard == "1")
                                    //        Response.Redirect("~/Dashboard.aspx", false);
                                    //    else if (settings.IconicUISetting.EnableImageVideo == "1")
                                    //        Response.Redirect("~/ImageAndVideoPage.aspx", false);
                                    //}

                                    //else
                                    if (settings.IconicUISetting.EnableImageVideo == "1")
                                        Response.Redirect("~/ImageAndVideoPage.aspx", false);
                                }
                            }
                            else
                            {
                                if (hdfGroupId.Value == "")
                                {
                                    //if (settings.IconicUISetting.EnableDashBoard == "1")
                                    //{
                                    //    if (BindCockpitView.GetEnergyDasboard == "1")
                                    //        Response.Redirect("~/Dashboard.aspx", false);
                                    //    else if (settings.IconicUISetting.EnableImageVideo == "1")
                                    //        Response.Redirect("~/ImageAndVideoPage.aspx", false);
                                    //}
                                    //else 
                                    if (settings.IconicUISetting.EnableImageVideo == "1")
                                        Response.Redirect("~/ImageAndVideoPage.aspx", false);
                                }
                            }

                            BindData();
                            if (hdfGroupId.Value != "")
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBeginDesktopMode()", true);
                            else
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBegin()", true);
                        }
                    }
                    else
                    {
                        BindData();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBeginDesktopMode()", true);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    lblMessages.Text = ex.ToString();
                }
            }
        }

        protected void lnkMachine_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lkBtn = (LinkButton)sender;
                string machineId = lkBtn.CommandArgument;
                string groupName = lkBtn.CommandName;
                if (!string.IsNullOrWhiteSpace(groupName))
                {
                    machineId = "";
                    hdfGroupId.Value = groupName;
                }
                else
                {
                    hdfGroupId.Value = "";
                    drp_SelectedIndexChanged(null, null);
                    if (hdfMode.Value != "" && hdfMode.Value.Equals("DESKTOP", StringComparison.OrdinalIgnoreCase))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBeginDesktopMode()", true);
                    if (hdfMode.Value.Equals("andon", StringComparison.OrdinalIgnoreCase))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBegin()", true);
                    return;
                }
                BindData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBeginDesktopMode()", true);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnPlantChange_Click(object sender, EventArgs e)
        {
            //DropDownList drp = (DropDownList)Page.Master.FindControl("ddlPlantName");
            //if (drp != null)
            //{
            ////    if (BindCockpitView.GetRotate == "1")
            ////    {
            ////        int selectedIndex = drp.SelectedIndex;
            ////        int itemLength = drp.Items.Count;

            ////        //if video not enabled
            ////        if (drp.SelectedIndex + 1 >= itemLength)
            ////        {
            ////            selectedIndex = 0;
            ////        }

            ////        if (selectedIndex + 1 < itemLength)
            ////        {
            ////            drp.SelectedIndex = selectedIndex + 1;
            ////            BindData();
            ////            ScriptManager.RegisterStartupScript(this, this.GetType(), "AfterPostback", "StartFromBegin()", true);
            ////        }
            ////        else
            ////        {
            ////            // To new Page
            ////            //Response.Redirect("~/ImageAndVideoPage");
            ////        }
            ////    }
            ////    else
            ////    {
            ////        Response.Redirect("~/ImageAndVideoPage");
            ////    }
            //}

        }
    }
}