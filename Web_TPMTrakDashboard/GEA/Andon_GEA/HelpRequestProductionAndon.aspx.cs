using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using System.IO;
using System.Text;
using Web_TPMTrakDashboard.GEA.Andon_GEA.Model;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using System.Threading.Tasks;
using ModelClassLibrary;
using BusinessClassLibrary;
using System.DirectoryServices;

namespace Web_TPMTrakDashboard.GEA.Andon_GEA
{
    public partial class HelpRequestProductionAndon : System.Web.UI.Page
    {
        List<PODetails> listPOData = new List<PODetails>();
        int count = 0, noOfRows, flips = 0, Interval = 0;
        decimal refreshData = 0;
        public static string screenName = "";
        public static string refreshScreenName = "";
        public static string inputToBindData = "";
        ObjectCache cache = MemoryCache.Default;
        public List<UserAccessModel> useAccessData = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AndonPage"] = this;
            // Interval = Utility.ProductionStatusInterval;
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    if (Session["UserAccessData"] == null)
                        Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                    else
                        useAccessData = Session["UserAccessData"] as List<UserAccessModel>;

                    if (!(useAccessData.AsEnumerable().Where(x => x.Domain.Equals("GEA")).Where(x => x.Code.Equals("AndonSettings")).Select(x => x.Selected == true).SingleOrDefault()))
                    {
                        divAndonSettings.Visible = false;
                    }
                    else
                        divAndonSettings.Visible = true;

                    ImgBtnSettings.Visible = false;
                    if (Session["AdminData"] != null)
                    {
                        if (Session["AdminData"].ToString().Equals("Admin"))
                        {
                            ImgBtnSettings.Visible = true;
                        }
                    }
                    else
                    {
                        //Response.Redirect("~/SignIn.aspx", false);
                    }
                }
                Cache.Remove("PlantID");
                Cache.Remove("POData");
                Cache.Remove("LeadingDetails");
                Cache.Remove("LaggingDetails");
                Cache.Remove("chartDetails");
                Cache.Remove("ShiftDayDecanterDetails");
                Cache.Remove("MonthDecanterDetails");
                Cache.Remove("ShiftDayMachineDetails");
                Cache.Remove("MonthMachineDetails");
                Session["ScreensToShow"] = null;
                bindPlantID();
                Cache["PlantID"] = ddlPlantID.SelectedValue;
                TimeSpan ts = new TimeSpan(200, 0, 0);
                Cache.Insert("PlantID", ddlPlantID.SelectedValue, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                setCompanyLogo();
                List<string> screenList = getScreensToShow();
                if (screenList.Count <= 0)
                {
                    lblErrorMsg.Visible = true;
                    laggingMachineContainer.Visible = false;
                    leadingMachineContainer.Visible = false;
                    downTimeContainer.Visible = false;
                    oeeConatiner.Visible = false;
                    imageVideoConatiner.Visible = false;
                    shiftDayContainer.Visible = false; //k
                    lblErrorMsg.Text = "Please select Screen";
                    return;
                }
                else
                {
                    lblErrorMsg.Visible = false;
                }

                imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                imgBtnSwitch_Click(null, null);
                setchartsDataToCache();
            }
        }
        private List<string> getScreensToShow()
        {
            List<string> listScreenToShow = new List<string>();
            if (Session["ScreensToShow"] == null)
            {
                if (Request.Cookies["POWithLeadLag"] != null)
                {
                    if (Request.Cookies["POWithLeadLag"].Value.ToString().Trim() == "1")
                    {
                        listScreenToShow.Add("POWithLeadLag");
                    }
                }
                if (Request.Cookies["POWithCharts"] != null)
                {
                    if (Request.Cookies["POWithCharts"].Value.ToString().Trim() == "1")
                    {
                        listScreenToShow.Add("POWithChart");
                    }
                }
                if (Request.Cookies["DecanterMachienShop"] != null)
                {
                    if (Request.Cookies["DecanterMachienShop"].Value.ToString().Trim() == "1")
                    {
                        listScreenToShow.Add("DecanterMachineShop");
                    }
                }
                Session["ScreensToShow"] = listScreenToShow;
            }
            listScreenToShow = Session["ScreensToShow"] as List<string>;
            return listScreenToShow;
        }
        private bool isScreensRequired(string screenname)
        {
            bool isThisscreenExists = false;
            List<string> listScreenToShow = new List<string>();
            if (Session["ScreensToShow"] == null)
            {
                Session["ScreensToShow"] = getScreensToShow();
            }
            listScreenToShow = Session["ScreensToShow"] as List<string>;
            isThisscreenExists = listScreenToShow.Contains(screenname);
            return isThisscreenExists;
        }
        private void setCompanyLogo()
        {
            //const string imagesPath = "~/CompanyLogo/";// "~/Image/Slideshow/";
            //var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

            ////filtering to jpgs, but ideally not required
            //List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
            //if (fileNames.Count > 0)
            //{
            //    Image2.ImageUrl = imagesPath + fileNames[0];
            //}
            //else
            //{
            //    Image2.ImageUrl = "Image/companyIcon.png";
            //}
            Image2.ImageUrl = Web_TPMTrakDashboard.Models.Util.getCompanyLogoPath();
        }
        public void BindAndonData()
        {
            if (isScreensRequired("POWithLeadLag"))
            {
                screenName = "postatuswithLeadLag";
                refreshScreenName = "postatuswithLeadLag";
            }
            else if (isScreensRequired("POWithChart"))
            {
                screenName = "POStatusWithChart";
                refreshScreenName = "POStatusWithChart";
            }
            else if (isScreensRequired("DecanterMachineShop"))
            {
                screenName = "shiftday";
                refreshScreenName = "shiftday";
            }

            ddlPlantID.SelectedValue = getPlantID();
            DataTable dtSetting = GEADatabaseAccess.getSettingDetails();
            spanWelcome.InnerText = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ScrollingText").Select(rows => rows.Field<string>("ValueInText2")).ToList()[0];
            headerName.InnerText = "Production Status";

            Interval = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "FlipInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
            //   int refreshInt = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "RefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
            #region ----- removed imgVideo Interval ----
            //int refreshInt;
            //string flipImagefloder = "";
            //try
            //{
            //    flipImagefloder = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "RefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0].ToString();
            //}
            //catch (Exception ex)
            //{
            //    flipImagefloder = "";
            //    Logger.WriteErrorLog(ex.Message);
            //}

            //if (flipImagefloder == null || flipImagefloder == "")
            //{
            //    refreshInt = 15000000;
            //}
            //else
            //{
            //    refreshInt = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "RefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
            //}
            //decimal refreshTick = Math.Ceiling(refreshInt / Convert.ToDecimal(Interval));
            //Session["RefreshInterval"] = refreshTick;
            #endregion
            Session["showImages"] = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ShowImage").Select(rows => rows.Field<int>("ValueInBool")).ToList()[0];
            Session["showVideo"] = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ShowVideo").Select(rows => rows.Field<int>("ValueInBool")).ToList()[0];
            string decanterDataType = "";
            decanterDataType = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ShowDecanterDataBy").Select(rows => rows.Field<string>("ValueInText2")).ToList().FirstOrDefault();
            if (decanterDataType == null || decanterDataType == "")
            {
                decanterDataType = "Shift";
            }
            Session["ShowDecanterDataBy"] = decanterDataType;
            int imageVideoFlipInterval = 0;
            imageVideoFlipInterval = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ImageVideoFlipInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList().FirstOrDefault();
            if (imageVideoFlipInterval == null || imageVideoFlipInterval == 0)
            {
                imageVideoFlipInterval = 7;
            }
            Session["ImageVideoFlipInterval"] = imageVideoFlipInterval;
            if (isScreensRequired("POWithLeadLag"))
            {
                BindPOData("LeadLag");
                // BindLeadingLaggingMachine(); //change1
                // displayLaggingAndLeadingMachine();  //change1
            }
            else if (isScreensRequired("POWithChart"))
            {
                BindPOData("charts");
            }
            else if (isScreensRequired("DecanterMachineShop"))
            {
                //decimal.TryParse(Session["RefreshInterval"].ToString(), out refreshData);
                //Session["RefreshInterval"] = refreshData - 1;
                BindShiftDayData();
            }


            poInterval.Enabled = true;
            poInterval.Interval = Interval * 1000;

        }
        public void bindPlantID()
        {
            List<string> list = GEADatabaseAccess.getPlantID();
            ddlPlantID.DataSource = list;
            ddlPlantID.DataBind();
            //ddlPlantID.Items.Insert(0, new ListItem("All", "All"));
        }
        public void displayLaggingAndLeadingMachine()
        {
            //ddlShiftDayType.Visible = false;
            poContainer.Visible = true;
            laggingMachineContainer.Visible = true;
            leadingMachineContainer.Visible = true;
            downTimeContainer.Visible = false;
            oeeConatiner.Visible = false;
            imageVideoConatiner.Visible = false;
            shiftDayContainer.Visible = false; //k
        }
        public void displayCharts()
        {
            //ddlShiftDayType.Visible = false;
            poContainer.Visible = true;
            laggingMachineContainer.Visible = false;
            leadingMachineContainer.Visible = false;
            downTimeContainer.Visible = true;
            oeeConatiner.Visible = true;
            imageVideoConatiner.Visible = false;
            shiftDayContainer.Visible = false; //k
        }
        public void displayImageVideoSlider()
        {
            //ddlShiftDayType.Visible = false;
            poInterval.Enabled = false;
            poContainer.Visible = false;
            laggingMachineContainer.Visible = false;
            leadingMachineContainer.Visible = false;
            downTimeContainer.Visible = false;
            oeeConatiner.Visible = false;
            imageVideoConatiner.Visible = true;
            shiftDayContainer.Visible = false; //k
        }
        public void displayShiftDayData()
        {
            //ddlShiftDayType.Visible = true;
            poContainer.Visible = false;
            laggingMachineContainer.Visible = false;
            leadingMachineContainer.Visible = false;
            downTimeContainer.Visible = false;
            oeeConatiner.Visible = false;
            imageVideoConatiner.Visible = false;
            shiftDayContainer.Visible = true; //k
        }
        private void showHideListviewColumn(ListView listview, List<AndonSettingData> list)
        {
            foreach (AndonSettingData data in list)
            {
                if (data.Visibility == false)
                {
                    (listview.FindControl("th" + data.Column) as HtmlControl).Visible = false;
                    for (int j = 0; j < listview.Items.Count; j++)
                    {
                        (listview.Items[j].FindControl("td" + data.Column) as HtmlControl).Visible = false;
                    }
                }
                else
                {
                    HtmlControl control = (listview.FindControl("th" + data.Column) as HtmlControl);
                    int flag = 0;
                    if (control.HasControls())
                    {
                        foreach (Control c in control.Controls)
                        {
                            if (c is HtmlImage)
                            {
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            (listview.FindControl("th" + data.Column) as HtmlTableCell).InnerText = data.CustomColumn;
                        }
                    }
                }

            }
            //for (int i = 0; i < tbl.Rows.Count; i++)
            //{
            //    if (tbl.Rows[i]["ValueInBool"].ToString() == "0")
            //    {
            //        (listview.FindControl("th" + tbl.Rows[i]["LanguageSpecified"].ToString()) as HtmlControl).Visible = false;
            //        for (int j = 0; j < listview.Items.Count; j++)
            //        {
            //            (listview.Items[j].FindControl("td" + tbl.Rows[i]["LanguageSpecified"].ToString()) as HtmlControl).Visible = false;
            //        }
            //    }

            //}
        }
        private string getPlantID()
        {
            string plantid = "";
            try
            {

                if (Cache["PlantID"] == null)
                {
                    plantid = ddlPlantID.SelectedValue;
                    Cache["PlantID"] = plantid;
                    TimeSpan ts = new TimeSpan(200, 0, 0);
                    Cache.Insert("PlantID", plantid, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);

                }
                else
                {
                    plantid = (string)Cache["PlantID"];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPlantID" + ex.Message);
            }
            return plantid;
        }
        private static string getPlantIDStatic()
        {
            string plantid = "";
            try
            {

                if (HttpContext.Current.Cache["PlantID"] != null)
                {
                    plantid = (string)HttpContext.Current.Cache["PlantID"];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPlantID" + ex.Message);
            }
            return plantid;
        }
        void BindPOData(string param)
        {
            try
            {
                inputToBindData = param;

                TimeSpan ts = new TimeSpan(0, 0, 300);
                if (Cache["POData"] == null)
                {
                    listPOData = GEADatabaseAccess.gridMachineStatusLoad("", getPlantID(), "1st Screen");
                    //stick the html in the literal tags and the cache
                    //  CacheItemPolicy policy = new CacheItemPolicy();
                    //update
                    //policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(10);
                    Cache["POData"] = listPOData;
                    Cache.Insert("POData", listPOData, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                }
                else

                {
                    listPOData = (List<PODetails>)Cache["POData"];
                }

                noOfRows = 5;
                if (listPOData != null && listPOData.Count > 0)
                {
                    DataTable dtSetting = GEADatabaseAccess.getSettingDetails();
                    IEnumerable<int> rowzz = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "NoOfRows").Select(rows => rows.Field<int>("ValueInInt"));
                    foreach (int rows in rowzz)
                    {
                        noOfRows = rows;
                    }
                    //Session["showImages"] = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ShowImage").Select(rows => rows.Field<int>("ValueInBool")).ToList()[0];
                    //Session["showVideo"] = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "ShowVideo").Select(rows => rows.Field<int>("ValueInBool")).ToList()[0];
                    count = listPOData.Count;
                    flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / noOfRows));
                    //IEnumerable<PODetails> data = listPOData.Take(noOfRows * 1);
                    lvPOData.DataSource = listPOData;
                    lvPOData.DataBind();


                    //DataTable result = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "POColumn").CopyToDataTable();
                    //showHideListviewColumn(lvPOData, result);
                    List<AndonSettingData> result = GEADatabaseAccess.getColumnData();
                    showHideListviewColumn(lvPOData, result);


                    //decimal.TryParse(Session["RefreshInterval"].ToString(), out refreshData); //removed imgVideo Interval
                    //Session["RefreshInterval"] = refreshData - 1;

                    Session["POFlips"] = flips;
                    Session["POData"] = listPOData;
                    Session["Rows"] = noOfRows;
                    Session["NoOfSkipRows"] = 1;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "bindNoOfRows", "setPORowsBasedOnHeight();", true);
                //if (screenName == "POStatusWithChart")
                //{
                //    displayCharts();
                //    headerName.InnerText = "Production Status";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts();", true);
                //}
                headerName.InnerText = "Production Status";
                if (param == "charts")
                {
                    displayCharts();
                    ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts();", true);
                }
                else
                {
                    BindLeadingLaggingMachine(); //change1
                    //displayLaggingAndLeadingMachine(); //change1
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding PO Details " + ex.Message);
            }

        }
        private void BindShiftDayData()
        {
            try
            {
                //decimal.TryParse(Session["RefreshInterval"].ToString(), out refreshData); //removed imgVideo Interval
                //Session["RefreshInterval"] = refreshData - 1; 
                headerName.InnerText = "Plant Production Status -" + getPlantID();
                displayShiftDayData();
                List<LeadingMachine> listMonthDecanter = new List<LeadingMachine>();
                List<LeadingMachine> listShiftDayDecanter = new List<LeadingMachine>();
                List<LeadingMachine> listMonthMachine = new List<LeadingMachine>();
                List<LeadingMachine> listShiftDayMachine = new List<LeadingMachine>();
                TimeSpan ts = new TimeSpan(0, 0, 300);
                string decanterDataType = Session["ShowDecanterDataBy"].ToString();
                //hdnShiftDayType is required for addDataToCacheMemory function. issue: if screen showing other screen at the same time addDataToCacheMemory hit then shifttype =undefined
                if (Cache["ShiftDayDecanterDetails"] == null || Cache["MonthDecanterDetails"] == null)
                {
                    listShiftDayDecanter = GEADatabaseAccess.getShiftDayMachineWise(getPlantID(), decanterDataType, out listMonthDecanter);
                    Cache["ShiftDayDecanterDetails"] = listShiftDayDecanter;
                    Cache.Insert("ShiftDayDecanterDetails", listShiftDayDecanter, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                    Cache["MonthDecanterDetails"] = listMonthDecanter;
                    Cache.Insert("MonthDecanterDetails", listMonthDecanter, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                }
                else
                {
                    listShiftDayDecanter = (List<LeadingMachine>)Cache["ShiftDayDecanterDetails"];
                    listMonthDecanter = (List<LeadingMachine>)Cache["MonthDecanterDetails"];
                }
                lvDecanterMonthView.DataSource = listMonthDecanter;
                lvDecanterMonthView.DataBind();
                lvDecanterShiftDay.DataSource = listShiftDayDecanter;
                lvDecanterShiftDay.DataBind();

                if (Cache["ShiftDayMachineDetails"] == null || Cache["MonthMachineDetails"] == null)
                {
                    listShiftDayMachine = GEADatabaseAccess.getShiftDayComponentWise(getPlantID(), decanterDataType, out listMonthMachine);
                    Cache["ShiftDayMachineDetails"] = listShiftDayMachine;
                    Cache.Insert("ShiftDayMachineDetails", listShiftDayMachine, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                    Cache["MonthMachineDetails"] = listMonthMachine;
                    Cache.Insert("MonthMachineDetails", listMonthMachine, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                }
                else
                {
                    listShiftDayMachine = (List<LeadingMachine>)Cache["ShiftDayMachineDetails"];
                    listMonthMachine = (List<LeadingMachine>)Cache["MonthMachineDetails"];
                }
                lvMachineShopShiftDay.DataSource = listShiftDayMachine;
                lvMachineShopShiftDay.DataBind();
                lvMachineShopMonth.DataSource = listMonthMachine;
                lvMachineShopMonth.DataBind();
                string headername = "";
                if (decanterDataType == "Shift")
                {
                    headername = "Current Shift Output";
                }
                else if (decanterDataType == "Day")
                {
                    headername = "Current Day Output";
                }
                else if (decanterDataType == "Week")
                {
                    headername = "Current Week Output";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "ShiftDayWeekHeader", "setMachineShopHeader('" + headername + "');", true);
            }
            catch (Exception ex)
            {

            }
        }

        protected void poInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["POFlips"] == null)
                {
                    // poInterval.Enabled = false;
                    //return;
                    Session["POFlips"] = 0;

                }
                int.TryParse(Session["POFlips"].ToString(), out flips);
                //if (Convert.ToDecimal(Session["RefreshInterval"].ToString()) <= 0) //removed imgVideo Interval
                //{
                //    displayImageVideoSlider();
                //    headerName.InnerText = "Images / Videos";
                //    bindImageVideo();
                //    return;
                //}
                if (flips > 1)
                {
                    int.TryParse(Session["Rows"].ToString(), out noOfRows);
                    listPOData = (List<PODetails>)Session["POData"];

                    IEnumerable<PODetails> data = listPOData.Skip(noOfRows * Convert.ToInt32(Session["NoOfSkipRows"].ToString())).Take(noOfRows);
                    lvPOData.DataSource = data;
                    lvPOData.DataBind();

                    DataTable dtSetting = GEADatabaseAccess.getSettingDetails();
                    //DataTable result = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "POColumn").CopyToDataTable();
                    //showHideListviewColumn(lvPOData, result);
                    List<AndonSettingData> result = GEADatabaseAccess.getColumnData();
                    showHideListviewColumn(lvPOData, result);

                    Session["NoOfSkipRows"] = Convert.ToInt32(Session["NoOfSkipRows"].ToString()) + 1;
                    //spanWelcome.InnerText = Session["NoOfSkipRows"].ToString() + ".Welcome to .......";
                    flips--;
                    Session["POFlips"] = flips;
                    //if(screenName== "POStatusWithChart")
                    //{
                    //    displayCharts();
                    //    headerName.InnerText = "Production Status";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts()", true);
                    //}
                    if (inputToBindData == "charts")
                    {
                        displayCharts();
                        headerName.InnerText = "Production Status";
                        ScriptManager.RegisterStartupScript(this, GetType(), "bind", "bindCharts()", true);
                    }
                    else
                    {
                        displayLaggingAndLeadingMachine();
                    }
                    //decimal.TryParse(Session["RefreshInterval"].ToString(), out refreshData); //removed imgVideo Interval
                    //Session["RefreshInterval"] = refreshData - 1;
                }
                else //removed imgVideo Interval
                {
                    showNextScreen();
                }
                #region --removed imgVideo Interval -----
                //else if (Convert.ToDecimal(Session["RefreshInterval"].ToString()) > 0)
                //{
                //    showNextScreen(); //change1
                //    //if (refreshScreenName == "postatuswithLeadLag")
                //    //{
                //    //    //BindPOData("charts");
                //    //    //refreshScreenName = "POStatusWithChart";
                //    //    showNextScreen("POWithLeadLag");
                //    //}
                //    //else if (refreshScreenName == "POStatusWithChart")
                //    //{
                //    //    //BindShiftDayData();
                //    //    //refreshScreenName = "shiftday";
                //    //    ////BindPOData("LeadLag");
                //    //    ////refreshScreenName = "postatuswithLeadLag";
                //    //    showNextScreen("POWithChart");
                //    //}
                //    //else if (refreshScreenName == "shiftday")
                //    //{
                //    //    //decimal.TryParse(Session["RefreshInterval"].ToString(), out refreshData);
                //    //    //Session["RefreshInterval"] = refreshData - 1;
                //    //    //BindPOData("LeadLag");
                //    //    //refreshScreenName = "postatuswithLeadLag";
                //    //    showNextScreen("DecanterMachineShop");
                //    //}
                //}
                //else if (Convert.ToDecimal(Session["RefreshInterval"].ToString()) <= 0)
                //{
                //    displayImageVideoSlider();
                //    headerName.InnerText = "Images / Videos";
                //    bindImageVideo();
                //}
                #endregion
                //else if (screenName == "POStatusWithChart" || Convert.ToDecimal(Session["RefreshInterval"].ToString()) <= 0)
                //{
                //    displayImageVideoSlider();
                //    headerName.InnerText = "Images / Videos";
                //    bindImageVideo();
                //}
                //else
                //{
                //    screenName = "POStatusWithChart";
                //    refreshScreenName = "POStatusWithChart";
                //    BindPOData("charts");
                //    //Response.Redirect("POStatusWithChart.aspx");
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Timer tick event " + ex.Message);
            }
        }
        private void showNextScreen()
        {
            try
            {
                string currentScreen = "";
                if (refreshScreenName == "postatuswithLeadLag")
                {
                    currentScreen = "POWithLeadLag";
                }
                else if (refreshScreenName == "POStatusWithChart")
                {
                    currentScreen = "POWithChart";
                }
                else if (refreshScreenName == "shiftday")
                {
                    currentScreen = "DecanterMachineShop";

                    // removed imgVideo Interval
                    displayImageVideoSlider();
                    headerName.InnerText = "Images / Videos";
                    bindImageVideo();
                    return;
                }
                List<string> listScreenList = getScreensToShow();
                if (listScreenList.Count > 0)
                {
                    int nextScreenIndex = 0;
                    string nextScreenName = "";
                    int currentScreenIndex = listScreenList.IndexOf(currentScreen);
                    if (currentScreenIndex == listScreenList.Count - 1)
                    {
                        nextScreenIndex = 0;
                        // removed imgVideo Interval
                        displayImageVideoSlider();
                        headerName.InnerText = "Images / Videos";
                        bindImageVideo();
                        return;
                    }
                    else
                    {
                        nextScreenIndex = currentScreenIndex + 1;
                    }
                    nextScreenName = listScreenList[nextScreenIndex];
                    if (string.Equals(nextScreenName, "POWithLeadLag", StringComparison.OrdinalIgnoreCase))
                    {
                        BindPOData("LeadLag");
                        refreshScreenName = "postatuswithLeadLag";
                    }
                    else if (string.Equals(nextScreenName, "POWithChart", StringComparison.OrdinalIgnoreCase))
                    {
                        BindPOData("charts");
                        refreshScreenName = "POStatusWithChart";
                    }
                    else if (string.Equals(nextScreenName, "DecanterMachineShop", StringComparison.OrdinalIgnoreCase))
                    {
                        BindShiftDayData();
                        refreshScreenName = "shiftday";
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private PODetails getPODetails(string machine, string status, string print, string setting, string alram, string user, string ae, string aeback, string pe, string peback, string oee, string oeeback, string plan, string act)
        {
            PODetails po1 = new PODetails();
            po1.Machine = machine;
            po1.Status = status;
            po1.Component = print;
            po1.Setting = setting;
            po1.Alaram = alram;
            po1.User = user;
            po1.AE = ae;
            po1.AEBackColor = aeback;
            po1.PE = pe;
            po1.PEBackColor = peback;
            po1.OEE = oee;
            po1.OEEBackColor = oeeback;
            po1.Plan = plan;
            po1.Act = act;
            return po1;
        }

        void BindLeadingMachine()
        {
            try
            {
                List<LeadingMachine> listData = new List<LeadingMachine>();
                listData = GEADatabaseAccess.gridLeadingmachinesLoad("", getPlantID(), "1st Screen");
                lvleadingMachine.DataSource = listData;
                lvleadingMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding Leading Machine Details " + ex.Message);
            }
        }
        private LeadingMachine getLeadingMachine(string item, string turning, string turnmill)
        {
            LeadingMachine data = new LeadingMachine();
            return data;
        }
        void BindLaggingMachine()
        {
            try
            {
                List<LeadingMachine> listData = new List<LeadingMachine>();
                listData = GEADatabaseAccess.gridLaggingmachinesLoad("", getPlantID(), "1st Screen");
                lvLaggingMachine.DataSource = listData;
                lvLaggingMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding Lagging Machine Details " + ex.Message);
            }
        }
        private void BindLeadingLaggingMachine()
        {
            try
            {
                displayLaggingAndLeadingMachine();
                List<LeadingMachine> listData = new List<LeadingMachine>();
                List<LeadingMachine> listLaggingData = null;
                TimeSpan ts = new TimeSpan(0, 0, 300);
                if (Cache["LeadingDetails"] == null || Cache["LaggingDetails"] == null)
                {
                    listData = GEADatabaseAccess.gridLeadingLaggingMachinesLoad("", getPlantID(), "1st Screen", out listLaggingData);
                    Cache["LeadingDetails"] = listData;
                    Cache.Insert("LeadingDetails", listData, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);

                    Cache["LaggingDetails"] = listLaggingData;
                    Cache.Insert("LaggingDetails", listLaggingData, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                }
                else
                {
                    listData = (List<LeadingMachine>)Cache["LeadingDetails"];
                    listLaggingData = (List<LeadingMachine>)Cache["LaggingDetails"];
                }
                //listData = GEADatabaseAccess.gridLeadingLaggingMachinesLoad("", ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue, "1st Screen", out listLaggingData);
                lvleadingMachine.DataSource = listData;
                lvleadingMachine.DataBind();
                lvLaggingMachine.DataSource = listLaggingData;
                lvLaggingMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While Binding Leading Machine Details " + ex.Message);
            }
        }
        private LaggingMachine getLaggingMachine(string item, string turning, string turnmill)
        {
            LaggingMachine data = new LaggingMachine();
            data.Item = item;
            data.IG1500 = turning;
            data.Turnmill02 = turnmill;
            return data;
        }


        public void bindImageVideo()
        {
            cache.Remove("CarouselInnerHtml");
            cache.Remove("CarouselIndicatorsHtml");
            if (Convert.ToInt32(Session["showVideo"].ToString()) == 0 && Convert.ToInt32(Session["showImages"].ToString()) == 0)
            {
                btnPost_Click(null, null);
            }
            else
            {
                BindSliderImage();
            }

        }
        private static int getImageVideoSort(string name)
        {
            int sortOrder = 0;
            try
            {
                List<CockpitEntity> list = new List<CockpitEntity>();
                if (HttpContext.Current.Session["ImageVideoSortData"] == null)
                {
                    list = GEADatabaseAccess.getAndonImageVideoSortOrderDetails();
                    HttpContext.Current.Session["ImageVideoSortData"] = list;
                }
                list = HttpContext.Current.Session["ImageVideoSortData"] as List<CockpitEntity>;
                sortOrder = list.Where(k => k.ValueInText2 == name).Select(k => k.ValueInInt).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return sortOrder;
        }
        private void BindSliderImage()
        {

            try
            {
                HttpContext.Current.Session["ImageVideoSortData"] = null;
                if (Session["showImages"] == null)
                {
                    Session["showImages"] = 1;
                }
                if (Session["showVideo"] == null)
                {
                    Session["showVideo"] = 1;
                }
                int im = Convert.ToInt32(Session["showImages"].ToString());
                int vi = Convert.ToInt32(Session["showVideo"].ToString());
                if (cache["CarouselInnerHtml"] != null && cache["CarouselIndicatorsHtml"] != null)
                {
                    //use the cached html
                    ltlCarouselImages.Text = cache["CarouselInnerHtml"].ToString();
                    ltlCarouselIndicators.Text = cache["CarouselIndicatorsHtml"].ToString();
                }
                else
                {
                    //get a list of images from the folder
                    const string imagesPath = "ImageVideoSilder/";// "~/Image/Slideshow/";
                    var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));
                    //filtering to jpgs, but ideally not required
                    List<string> fileNames = (from flInfo in dir.GetFiles("*.*", SearchOption.TopDirectoryOnly) select flInfo.Name).ToList();
                    //List<string> fileNames = (from flInfo in dir.GetFiles() where flInfo.Name.EndsWith(".jpg") select flInfo.Name).ToList();
                    if (fileNames.Count > 0)
                    {
                        List<CockpitEntity> imglist = new List<CockpitEntity>();
                        List<CockpitEntity> videoList = new List<CockpitEntity>();
                        for (int i = 0; i < fileNames.Count; i++)
                        {
                            var result = Path.GetExtension(fileNames[i]).ToLower();
                            if (!result.ToString().Equals(".scc", StringComparison.OrdinalIgnoreCase))
                            {
                                //first display images then display videos
                                if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") ||
                                   (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                                {
                                    CockpitEntity data = new CockpitEntity();
                                    data.Parameter = fileNames[i];
                                    data.ValueInInt = getImageVideoSort(fileNames[i]);
                                    videoList.Add(data);
                                }
                                else
                                {
                                    CockpitEntity data = new CockpitEntity();
                                    data.Parameter = fileNames[i];
                                    data.ValueInInt = getImageVideoSort(fileNames[i]);
                                    imglist.Add(data);
                                }
                            }
                        }
                        int slideCount = 0;
                        var carouselInnerHtml = new StringBuilder();
                        var indicatorsHtml = new StringBuilder(@"<ol class='carousel-indicators'>");
                        //loop through and build up the html for indicators + images
                        for (int iVCount = 0; iVCount < 2; iVCount++)  //first display images then display videos
                        {
                            List<CockpitEntity> list = new List<CockpitEntity>();
                            if (iVCount == 0)
                            {
                                list = imglist;
                            }
                            else
                            {
                                list = videoList;
                            }
                            list = list.OrderBy(k => k.ValueInInt).ToList();
                            for (int i = 0; i < list.Count; i++)
                            {
                                var result = Path.GetExtension(list[i].Parameter).ToLower();// fileNames[i].Substring(fileNames[i].LastIndexOf(".") + 1);
                                if (!result.ToString().Equals(".scc", StringComparison.OrdinalIgnoreCase))
                                {

                                    if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") ||
                                        (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                                    {
                                        //carouselInnerHtml.AppendFormat("<video  class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' playsinline='playsinline' controls autoplay>\r\n", i + 1);
                                        if (Convert.ToInt32(Session["showVideo"].ToString()) == 1)
                                        {
                                            if (carouselInnerHtml.ToString().Contains("active"))
                                            {
                                                carouselInnerHtml.AppendLine("<div class='item' data-slide='" + slideCount + "'>");
                                            }
                                            else
                                            {
                                                carouselInnerHtml.AppendLine("<div class='item active' data-slide='" + slideCount + "' >");
                                            }
                                            //carouselInnerHtml.AppendLine(i == 0 ? "<div class='item active'>" : "<div class='item'>");
                                            //carouselInnerHtml.AppendFormat("<video  class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' playsinline='playsinline' muted='muted' autoplay='autoplay' controls>\r\n", i + 1);
                                            carouselInnerHtml.AppendFormat("<video  class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' playsinline='playsinline'  autoplay='autoplay' controls>\r\n", slideCount + 1);
                                            carouselInnerHtml.AppendFormat("<source src='{0}' type='video/mp4'>\r\n", imagesPath + list[i].Parameter);
                                            carouselInnerHtml.AppendLine("</video>");
                                            carouselInnerHtml.AppendLine("</div>");
                                            indicatorsHtml.AppendLine(slideCount == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class=''></li>");
                                        }
                                    }
                                    else
                                    {
                                        //img-fluid img-thumbnail
                                        if (Convert.ToInt32(Session["showImages"].ToString()) == 1)
                                        {
                                            if (carouselInnerHtml.ToString().Contains("active"))
                                            {
                                                carouselInnerHtml.AppendLine("<div class='item' data-slide='" + slideCount + "'>");
                                            }
                                            else
                                            {
                                                carouselInnerHtml.AppendLine("<div class='item active' data-slide='" + slideCount + "'>");
                                            }
                                            // carouselInnerHtml.AppendLine(i == 0 ? "<div class='item active'>" : "<div class='item'>");
                                            carouselInnerHtml.AppendLine("<img class='img-responsive img-rounded center-block makeStyle' src='" + imagesPath + list[i].Parameter + "' alt='Slide #" + (slideCount + 1) + "'>");
                                            carouselInnerHtml.AppendLine("</div>");
                                            indicatorsHtml.AppendLine(slideCount == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class=''></li>");
                                        }
                                    }
                                    slideCount++;
                                }
                            }
                        }
                        //close tag                      
                        indicatorsHtml.AppendLine("</ol>");
                        //stick the html in the literal tags and the cache
                        CacheItemPolicy policy = new CacheItemPolicy();
                        //update
                        policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(10);
                        cache.Set("CarouselInnerHtml", carouselInnerHtml.ToString(), policy);
                        cache.Set("CarouselIndicatorsHtml", indicatorsHtml.ToString(), policy);


                        ltlCarouselImages.Text = carouselInnerHtml.ToString();
                        ltlCarouselIndicators.Text = indicatorsHtml.ToString();
                    }
                    else
                    {
                        btnPost_Click(null, null);
                    }
                }
            }
            catch (Exception)
            {
                //something is dodgy so flush the cache
                if (cache["CarouselInnerHtml"] != null)
                {
                    Cache.Remove("CarouselInnerHtml");
                }
                if (cache["CarouselIndicatorsHtml"] != null)
                {
                    Cache.Remove("CarouselIndicatorsHtml");
                }
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<int> getFontSize()
        {
            List<int> size = new List<int>();
            DataTable dt = GEADatabaseAccess.getSettingDetails();
            var header = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText2") == "Header" && x.Field<string>("ValueInText") == "FontSize").Select(x => x.Field<int>("ValueInInt")).ToList();
            size.Add(Convert.ToInt32(header[0]));
            var content = dt.AsEnumerable().Where(x => x.Field<string>("ValueInText2") == "Content" && x.Field<string>("ValueInText") == "FontSize").Select(x => x.Field<int>("ValueInInt")).ToList();
            size.Add(Convert.ToInt32(content[0]));

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["ValueInText2"].ToString() == "Header" && dt.Rows[i]["ValueInText"].ToString() == "FontSize")
            //    {
            //        size.Add(Convert.ToInt32(dt.Rows[i]["ValueInInt"].ToString()));
            //    }
            //    if (dt.Rows[i]["ValueInText2"].ToString() == "Content" && dt.Rows[i]["ValueInText"].ToString() == "FontSize")
            //    {
            //        size.Add(Convert.ToInt32(dt.Rows[i]["ValueInInt"].ToString()));
            //    }
            //}
            return size;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ObservableCollection<OEEData> getOEEChartData(string plant)
        {
            ObservableCollection<OEEData> listData = new ObservableCollection<OEEData>();
            listData = Utility.ShiftOrDaywise == 1 ? GEADatabaseAccess.GetOEEData("", plant, "3rd Screen") : GEADatabaseAccess.GetOEEData("", plant, "3rd Screen");
            return listData;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int getNumberOfImagesVideo()
        {
            //DirectoryInfo diInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ImageVideoSilder/"));
            int count = 0;
            return count;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ChartSeries getDownTimeChartData(string plant)
        {
            ChartSeries chartSeries = new ChartSeries();
            try
            {
                List<DataSeries> series = new List<DataSeries>();
                DataSeries LineSeries = new DataSeries();
                DataSeries ColumnSeries = new DataSeries();
                double[] lineData;
                string[] category;
                List<double[]> columnData = new List<double[]>();
                chartSeries.Title = "Down Time Report";
                ObservableCollection<DownTimeParetoDATAModel> DownTimeParetoData = new ObservableCollection<DownTimeParetoDATAModel>();
                DownTimeParetoData = Utility.ShiftOrDaywise == 1 ? GEADatabaseAccess.GetDownParetoData("", plant, "3rd Screen") : GEADatabaseAccess.GetDownParetoData("", plant, "3rd Screen");
                LineSeries.type = "spline";
                LineSeries.name = "DownTime";
                LineSeries.color = "#86ba35";
                LineSeries.yAxis = 1;
                LineSeries.zIndex = 1;
                ColumnSeries.type = "column";
                ColumnSeries.name = "DownTime";
                ColumnSeries.color = "#1ba1e2";
                ColumnSeries.yAxis = 0;
                if (DownTimeParetoData.Count > 0)
                {
                    lineData = new double[DownTimeParetoData.Count];
                    category = new string[DownTimeParetoData.Count];
                    for (int i = 0; i < DownTimeParetoData.Count; i++)
                    {
                        lineData[i] = DownTimeParetoData[i].YValue;
                        category[i] = DownTimeParetoData[i].XValue;
                    }
                    Marker lineMark = new Marker();
                    lineMark.radius = 5;
                    chartSeries.Category = category;
                    LineSeries.data = lineData;
                    LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                else
                {
                    lineData = new double[1] { 0 };
                    category = new string[1] { "" };
                    Marker lineMark = new Marker();
                    lineMark.radius = 0;
                    chartSeries.Category = category;
                    LineSeries.data = lineData;
                    LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                series.Add(LineSeries);
                series.Add(ColumnSeries);
                chartSeries.series = series;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
            }
            return chartSeries;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static ChartDta getChartData()
        {
            ChartDta chartdata = new ChartDta();
            ChartSeries chartSeries = new ChartSeries();
            ObservableCollection<OEEData> downpantOeedata = new ObservableCollection<OEEData>();
            TimeSpan ts = new TimeSpan(0, 0, 300);
            if (HttpContext.Current.Cache["chartDetails"] == null)
            {
                try
                {
                    string plant = getPlantIDStatic();
                    List<DataSeries> series = new List<DataSeries>();
                    DataSeries LineSeries = new DataSeries();
                    DataSeries ColumnSeries = new DataSeries();
                    double[] lineData;
                    string[] category;
                    List<double[]> columnData = new List<double[]>();
                    chartSeries.Title = "Down Time Report";
                    ObservableCollection<DownTimeParetoDATAModel> DownTimeParetoData = new ObservableCollection<DownTimeParetoDATAModel>();
                    DownTimeParetoData = Utility.ShiftOrDaywise == 1 ? GEADatabaseAccess.GetChartsData("", plant, "3rd Screen", out downpantOeedata) : GEADatabaseAccess.GetChartsData("", plant, "3rd Screen", out downpantOeedata);
                    chartdata.OEEData = downpantOeedata;
                    LineSeries.type = "pareto";
                    LineSeries.name = "DownTime";
                    LineSeries.color = "#86ba35";
                    LineSeries.yAxis = 1;
                    LineSeries.zIndex = 10;
                    LineSeries.baseSeries = 1;
                    ColumnSeries.type = "column";
                    ColumnSeries.name = "DownTime";
                    ColumnSeries.color = "#1ba1e2";
                    // ColumnSeries.yAxis = 0;
                    ColumnSeries.zIndex = 2;
                    if (DownTimeParetoData.Count > 0)
                    {
                        lineData = new double[DownTimeParetoData.Count];
                        category = new string[DownTimeParetoData.Count];
                        for (int i = 0; i < DownTimeParetoData.Count; i++)
                        {
                            lineData[i] = DownTimeParetoData[i].YValue;
                            category[i] = DownTimeParetoData[i].XValue;
                        }
                        Marker lineMark = new Marker();
                        lineMark.radius = 5;
                        chartSeries.Category = category;
                        // LineSeries.data = lineData;
                        // LineSeries.marker = lineMark;
                        ColumnSeries.data = lineData;
                    }
                    else
                    {
                        lineData = new double[1] { 0 };
                        category = new string[1] { "" };
                        Marker lineMark = new Marker();
                        lineMark.radius = 0;
                        chartSeries.Category = category;
                        //  LineSeries.data = lineData;
                        //   LineSeries.marker = lineMark;
                        ColumnSeries.data = lineData;
                    }
                    series.Add(LineSeries);
                    series.Add(ColumnSeries);
                    chartSeries.series = series;
                    chartdata.DownTimeData = chartSeries;
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
                }
                HttpContext.Current.Cache["chartDetails"] = chartdata;
                HttpContext.Current.Cache.Insert("chartDetails", chartdata, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
            }
            else
            {
                chartdata = (ChartDta)HttpContext.Current.Cache["chartDetails"];
            }
            return chartdata;
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshThePage();
        }
        //protected void ddlShiftDayType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        hdnShiftDayType.Value = ddlShiftDayType.SelectedValue;
        //        refreshThePage();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //}
        private void refreshThePage()
        {
            try
            {
                Cache.Remove("PlantID");
                Cache.Remove("POData");
                Cache.Remove("LeadingDetails");
                Cache.Remove("LaggingDetails");
                Cache.Remove("chartDetails");
                Cache.Remove("ShiftDayDecanterDetails");
                Cache.Remove("MonthDecanterDetails");
                Cache.Remove("ShiftDayMachineDetails");
                Cache.Remove("MonthMachineDetails");
                poInterval.Enabled = false;
                imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                imgBtnSwitch_Click(null, null);
                setchartsDataToCache();

                //if (imgBtnSwitch.ToolTip == "Switch to ANDON Mode")
                //{
                //    BindPOData("LeadLag");
                //    //  BindLeadingMachine();
                //    // BindLaggingMachine();
                //    BindLeadingLaggingMachine();
                //    displayLaggingAndLeadingMachine();
                //    poInterval.Enabled = false;
                //}
                //else
                //{
                //    bindPOWithLeadLag();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void btnSwitch_Click(object sender, EventArgs e)
        {
            //if (btnSwitch.Text == "Switch to ANDON Mode")
            //{
            //    bindPOWithLeadLag();
            //    btnSwitch.Text = "Switch to DESKTOP Mode";
            //}
            //else
            //{
            //    BindPOData("LeadLag");
            //    BindLeadingMachine();
            //    BindLaggingMachine();
            //    displayLaggingAndLeadingMachine();
            //    poInterval.Enabled = false;
            //    btnSwitch.Text = "Switch to ANDON Mode";
            //}
        }

        protected void imgBtnSwitch_Click(object sender, ImageClickEventArgs e)
        {
            if (imgBtnSwitch.ToolTip == "Switch to ANDON Mode")
            {

                BindAndonData();
                imgBtnSwitch.ToolTip = "Switch to DESKTOP Mode";
                imgBtnSwitch.ImageUrl = "Images/desktop1.png";
            }
            else
            {
                if (isScreensRequired("POWithLeadLag"))
                {
                    BindPOData("LeadLag");
                    //  BindLeadingLaggingMachine(); //change1
                    // displayLaggingAndLeadingMachine(); //change1
                }
                else if (isScreensRequired("POWithChart"))
                {
                    BindPOData("charts");
                }
                else if (isScreensRequired("DecanterMachineShop"))
                {
                    BindShiftDayData();
                }
                poInterval.Enabled = false;
                imgBtnSwitch.ToolTip = "Switch to ANDON Mode";
                imgBtnSwitch.ImageUrl = "Images/andon.jpg";
            }
        }
        public void setchartsDataToCache()
        {
            TimeSpan ts = new TimeSpan(0, 0, 300);
            #region ---------- chart data ---------------
            ChartDta chartdata = new ChartDta();
            ChartSeries chartSeries = new ChartSeries();
            ObservableCollection<OEEData> downpantOeedata = new ObservableCollection<OEEData>();
            try
            {
                List<DataSeries> series = new List<DataSeries>();
                DataSeries LineSeries = new DataSeries();
                DataSeries ColumnSeries = new DataSeries();
                double[] lineData;
                string[] category;
                List<double[]> columnData = new List<double[]>();
                chartSeries.Title = "Down Time Report";
                ObservableCollection<DownTimeParetoDATAModel> DownTimeParetoData = new ObservableCollection<DownTimeParetoDATAModel>();
                DownTimeParetoData = Utility.ShiftOrDaywise == 1 ? GEADatabaseAccess.GetChartsData("", getPlantID(), "3rd Screen", out downpantOeedata) : GEADatabaseAccess.GetChartsData("", getPlantID(), "3rd Screen", out downpantOeedata);
                chartdata.OEEData = downpantOeedata;
                LineSeries.type = "pareto";
                LineSeries.name = "DownTime";
                LineSeries.color = "#86ba35";
                LineSeries.yAxis = 1;
                LineSeries.zIndex = 10;
                LineSeries.baseSeries = 1;
                ColumnSeries.type = "column";
                ColumnSeries.name = "DownTime";
                ColumnSeries.color = "#1ba1e2";
                // ColumnSeries.yAxis = 0;
                ColumnSeries.zIndex = 2;
                if (DownTimeParetoData.Count > 0)
                {
                    lineData = new double[DownTimeParetoData.Count];
                    category = new string[DownTimeParetoData.Count];
                    for (int i = 0; i < DownTimeParetoData.Count; i++)
                    {
                        lineData[i] = DownTimeParetoData[i].YValue;
                        category[i] = DownTimeParetoData[i].XValue;
                    }
                    Marker lineMark = new Marker();
                    lineMark.radius = 5;
                    chartSeries.Category = category;
                    // LineSeries.data = lineData;
                    // LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                else
                {
                    lineData = new double[1] { 0 };
                    category = new string[1] { "" };
                    Marker lineMark = new Marker();
                    lineMark.radius = 0;
                    chartSeries.Category = category;
                    //  LineSeries.data = lineData;
                    //   LineSeries.marker = lineMark;
                    ColumnSeries.data = lineData;
                }
                series.Add(LineSeries);
                series.Add(ColumnSeries);
                chartSeries.series = series;
                chartdata.DownTimeData = chartSeries;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
            }
            HttpContext.Current.Cache["chartDetails"] = chartdata;
            HttpContext.Current.Cache.Insert("chartDetails", chartdata, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);

            #endregion
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setNoOfRowsToSession(int rows, int rowsLength)
        {
            HttpContext.Current.Session["Rows"] = rows;
            int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(rowsLength) / rows));
            HttpContext.Current.Session["POFlips"] = flips;
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            BindAndonData();
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setScreenName()
        {
            screenName = "postatuswithLeadLag";
        }

        protected void ImgBtnSettings_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                poInterval.Enabled = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenLoginPoPUp", " OpenLoginModal();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ImgBtnSettings_Click=  " + ex.Message);
            }
        }

        protected void btnSettingLogin_Click(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void addDataToCacheMemory()
        {
            try
            {
                string shiftdaytype = HttpContext.Current.Session["ShowDecanterDataBy"].ToString();
                string plant = getPlantIDStatic();
                var hhtp = HttpContext.Current;
                System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    // return ThumbnailHelper.CreateThumbnail(plant);
                    return Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                //HttpContext.Current = (HttpContext)hhtp;

                                TimeSpan ts = new TimeSpan(0, 0, 300);
                                List<PODetails> listPOData1 = new List<PODetails>();
                                listPOData1 = GEADatabaseAccess.gridMachineStatusLoad("", plant, "1st Screen");
                                // HttpContext.Current.Session["SPOData"] = listPOData1;
                                hhtp.Cache["POData"] = listPOData1;
                                hhtp.Cache.Insert("POData", listPOData1, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);


                                List<LeadingMachine> listData = new List<LeadingMachine>();
                                List<LeadingMachine> listLaggingData = null;
                                listData = GEADatabaseAccess.gridLeadingLaggingMachinesLoad("", plant, "1st Screen", out listLaggingData);
                                hhtp.Cache["LeadingDetails"] = listData;
                                hhtp.Cache.Insert("LeadingDetails", listData, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                                hhtp.Cache["LaggingDetails"] = listLaggingData;
                                hhtp.Cache.Insert("LaggingDetails", listLaggingData, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);




                                #region ---------- chart data ---------------
                                ChartDta chartdata = new ChartDta();
                                ChartSeries chartSeries = new ChartSeries();
                                ObservableCollection<OEEData> downpantOeedata = new ObservableCollection<OEEData>();
                                try
                                {
                                    List<DataSeries> series = new List<DataSeries>();
                                    DataSeries LineSeries = new DataSeries();
                                    DataSeries ColumnSeries = new DataSeries();
                                    double[] lineData;
                                    string[] category;
                                    List<double[]> columnData = new List<double[]>();
                                    chartSeries.Title = "Down Time Report";
                                    ObservableCollection<DownTimeParetoDATAModel> DownTimeParetoData = new ObservableCollection<DownTimeParetoDATAModel>();
                                    DownTimeParetoData = Utility.ShiftOrDaywise == 1 ? GEADatabaseAccess.GetChartsData("", plant, "3rd Screen", out downpantOeedata) : GEADatabaseAccess.GetChartsData("", plant, "3rd Screen", out downpantOeedata);
                                    chartdata.OEEData = downpantOeedata;
                                    LineSeries.type = "pareto";
                                    LineSeries.name = "DownTime";
                                    LineSeries.color = "#86ba35";
                                    LineSeries.yAxis = 1;
                                    LineSeries.zIndex = 10;
                                    LineSeries.baseSeries = 1;
                                    ColumnSeries.type = "column";
                                    ColumnSeries.name = "DownTime";
                                    ColumnSeries.color = "#1ba1e2";
                                    // ColumnSeries.yAxis = 0;
                                    ColumnSeries.zIndex = 2;
                                    if (DownTimeParetoData.Count > 0)
                                    {
                                        lineData = new double[DownTimeParetoData.Count];
                                        category = new string[DownTimeParetoData.Count];
                                        for (int i = 0; i < DownTimeParetoData.Count; i++)
                                        {
                                            lineData[i] = DownTimeParetoData[i].YValue;
                                            category[i] = DownTimeParetoData[i].XValue;
                                        }
                                        Marker lineMark = new Marker();
                                        lineMark.radius = 5;
                                        chartSeries.Category = category;
                                        // LineSeries.data = lineData;
                                        // LineSeries.marker = lineMark;
                                        ColumnSeries.data = lineData;
                                    }
                                    else
                                    {
                                        lineData = new double[1] { 0 };
                                        category = new string[1] { "" };
                                        Marker lineMark = new Marker();
                                        lineMark.radius = 0;
                                        chartSeries.Category = category;
                                        //  LineSeries.data = lineData;
                                        //   LineSeries.marker = lineMark;
                                        ColumnSeries.data = lineData;
                                    }
                                    series.Add(LineSeries);
                                    series.Add(ColumnSeries);
                                    chartSeries.series = series;
                                    chartdata.DownTimeData = chartSeries;
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteErrorLog("While getting  down time chart data " + ex.Message);
                                }
                                hhtp.Cache["chartDetails"] = chartdata;
                                hhtp.Cache.Insert("chartDetails", chartdata, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                                #endregion

                                #region --Add shift day data to cache ------
                                List<LeadingMachine> listMonthDecanter = new List<LeadingMachine>();
                                List<LeadingMachine> listShiftDayDecanter = new List<LeadingMachine>();
                                List<LeadingMachine> listMonthMachine = new List<LeadingMachine>();
                                List<LeadingMachine> listShiftDayMachine = new List<LeadingMachine>();
                                listShiftDayDecanter = GEADatabaseAccess.getShiftDayMachineWise(plant, shiftdaytype, out listMonthDecanter);
                                hhtp.Cache["ShiftDayDecanterDetails"] = listShiftDayDecanter;
                                hhtp.Cache.Insert("ShiftDayDecanterDetails", listShiftDayDecanter, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                                hhtp.Cache["MonthDecanterDetails"] = listMonthDecanter;
                                hhtp.Cache.Insert("MonthDecanterDetails", listMonthDecanter, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);

                                listShiftDayMachine = GEADatabaseAccess.getShiftDayComponentWise(plant, shiftdaytype, out listMonthMachine);
                                hhtp.Cache["ShiftDayMachineDetails"] = listShiftDayMachine;
                                hhtp.Cache.Insert("ShiftDayMachineDetails", listShiftDayMachine, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
                                hhtp.Cache["MonthMachineDetails"] = listMonthMachine;
                                hhtp.Cache.Insert("MonthMachineDetails", listMonthMachine, null, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteErrorLog(ex.Message);
                            }
                        });

                    // return ttak;
                });
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("addDataToCacheMemory " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                poInterval.Enabled = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static long getCacheIntervalValue()
        {
            DataTable dtSetting = GEADatabaseAccess.getSettingDetails();
            int refreshInt = dtSetting.AsEnumerable().Where(rows => rows.Field<string>("ValueInText") == "DataRefreshInterval").Select(rows => rows.Field<int>("ValueInInt")).ToList()[0];
            return refreshInt * 1000;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int getImageVideoFlipIntervalValue()
        {
            int ivFlipInterval = 7;
            try
            {
                if (HttpContext.Current.Session["ImageVideoFlipInterval"] != null)
                {
                    ivFlipInterval = Convert.ToInt32(HttpContext.Current.Session["ImageVideoFlipInterval"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return ivFlipInterval;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool ValidateUserForSettings(string UserName, string Password)
        {
            bool IsValid = false, isUserValid = false;
            try
            {
                List<UserAccessModel> useAccessData = new List<UserAccessModel>();

                string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                var arr = domainUser.Split(new char[] { '\\' }, StringSplitOptions.None);

                string domain = "LDAP://" + arr[0].ToString();
                using (DirectoryEntry deDirEntry = new DirectoryEntry(domain, UserName, Password, AuthenticationTypes.Secure))
                {
                    try
                    {
                        if (deDirEntry.Name != null)
                        {
                            isUserValid = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog(ex.Message.ToString());
                        isUserValid = false;
                    }


                    if (isUserValid)
                    {
                        if (HttpContext.Current.Session["UserAccessData"] == null)
                            useAccessData = BindCockpitView.bindListUserAccess(UserName);
                        else
                            useAccessData = HttpContext.Current.Session["UserAccessData"] as List<UserAccessModel>;

                        if ((useAccessData.AsEnumerable().Where(x => x.Domain.Equals("GEA")).Where(x => x.Code.Equals("AndonSettings")).Select(x => x.Selected == true).SingleOrDefault()))
                        {
                            IsValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return IsValid;
        }


    }
}