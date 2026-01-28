using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class AndonPage : System.Web.UI.Page
    {
        public static AndonSettingEntity settings = new AndonSettingEntity();

        bool isPostBack = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["AndonPage"] = this;
                if (!IsPostBack)
                {
                    Logger.WriteDebugLog("!.. Post Back Event ..!");
                    if (Request.Cookies["ComputerName"] == null)
                    {
                        ddlScreenName.Visible = false;
                        ComputerDiv.Visible = true;
                        BindPlantID();
                        BindCellID();
                        BindFrequency();
                        return;
                    }
                    else
                    {
                        ComputerDiv.Visible = false;
                    }
                    if (WebConfigurationManager.AppSettings["KTASpindlePages"].ToString() == "1")
                    {
                        ddlScreenName.Visible = true;
                    }
                    else
                    {
                        ddlScreenName.Visible = false;
                    }
                    isPostBack = false;
                    Session["RunningScreen"] = null;
                    Session["View"] = "AndonView";
                    Session["RunOption"] = null;
                    //Cache.Remove($"BindCacheData{HttpContext.Current.Session.SessionID}");
                    //Cache.Remove($"MainCacheData{HttpContext.Current.Session.SessionID}");
                    Session["BindCacheData"] = null;
                    Session["MainCacheData"] = null;
                    Session["EnabledScreens"] = null;
                    Session["andonSettingList"] = null;
                    Session["MainsettingList"] = null;
                    Session["RunOption"] = null;
                    setScreens();
                    BindPlantID();
                    BindCellID();
                    BindFrequency();
                    insertLatestDataToMainCache();
                    BindDataFromBeginning();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Page_Load =" + ex.ToString());
            }
        }

        private void setScreens()
        {
            try
            {
                Session["EnabledScreens"] = null;
                List<ScreenEntity> list = new List<ScreenEntity>();
                if (WebConfigurationManager.AppSettings["AndonCockpit"].ToString() == "1")
                {
                    list.Add(new ScreenEntity() { Screen = "cockpitControl", Order = 1 });
                }
                if (WebConfigurationManager.AppSettings["ScheduleAndonKTA"].ToString() == "1")
                {
                    list.Add(new ScreenEntity() { Screen = "ScheduleKTAControl", Order = 2 });
                }
                if (WebConfigurationManager.AppSettings["PoojaAndonMelting"].ToString() == "1")
                {
                    list.Add(new ScreenEntity() { Screen = "PoojaCastingMeltingControl", Order = 3 });
                }
                list.OrderBy(k => k.Order);
                Session["EnabledScreens"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setScreens: " + ex.ToString());
            }
        }

        private static void setScreens_static()
        {
            try
            {
                HttpContext.Current.Session["EnabledScreens"] = null;
                List<ScreenEntity> list = new List<ScreenEntity>();

                if (WebConfigurationManager.AppSettings["AndonCockpit"].ToString() == "1")
                {
                    list.Add(new ScreenEntity() { Screen = "cockpitControl", Order = 1 });
                }
                if (WebConfigurationManager.AppSettings["ScheduleAndonKTA"].ToString() == "1")
                {
                    list.Add(new ScreenEntity() { Screen = "ScheduleKTAControl", Order = 2 });
                }
                if (WebConfigurationManager.AppSettings["PoojaAndonMelting"].ToString() == "1")
                {
                    list.Add(new ScreenEntity() { Screen = "PoojaCastingMeltingControl", Order = 3 });
                }
                list.OrderBy(k => k.Order);
                HttpContext.Current.Session["EnabledScreens"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setScreens: " + ex.ToString());
            }
        }


        private void BindPlantID()
        {
            try
            {
                ddlPlantID.DataSource = AndonDBAccess.getPlantID_Andon();
                ddlPlantID.DataBind();
                if (ddlPlantID.Items.Count > 0)
                    ddlPlantID.Items.Insert(0, new ListItem() { Text = "Plant All", Value = "All" });

                if (Session["AndonPlant"] == null)
                    Session["AndonPlant"] = ddlPlantID.SelectedValue;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindPlantID: {ex}");
            }
        }

        private void BindCellID()
        {
            try
            {
                string plantId = ddlPlantID.SelectedValue;
                ddlCellID.DataSource = AndonDBAccess.getCellID(plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                ddlCellID.DataBind();
                if (ddlCellID.Items.Count > 0)
                    ddlCellID.Items.Insert(0, new ListItem() { Text = "Cell All", Value = "All" });

                if (Request.Cookies["AndonCellID"] == null)
                {
                    Response.Cookies["AndonCellID"].Value = ddlCellID.SelectedValue;
                    Response.Cookies["AndonCellID"].Expires = DateTime.MaxValue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCellID =" + ex.ToString());
            }
        }

        private void BindFrequency()
        {
            try
            {
                List<ListItem> list = new List<ListItem>();
                list.Add(new ListItem() { Value = "Last One", Text = "Latest One Hour" });
                list.Add(new ListItem() { Value = "Two Hours", Text = "Latest Two Hours" });
                list.Add(new ListItem() { Value = "Three Hours", Text = "Latest Three Hours" });
                list.Add(new ListItem() { Value = "Shift", Text = "Shift" });
                list.Add(new ListItem() { Value = "Day", Text = "Day" });
                ddlFrequency.DataSource = list;
                ddlFrequency.DataTextField = "Text";
                ddlFrequency.DataValueField = "Value";
                ddlFrequency.DataBind();
                if (Request.Cookies["AndonFrequency"] == null)
                {
                    Response.Cookies["AndonFrequency"].Value = ddlFrequency.SelectedValue;
                    Response.Cookies["AndonFrequency"].Expires = DateTime.MaxValue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindFrequency =" + ex.ToString());
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                imageVideoInterval.Enabled = false;
                Session["AndonPlant"] = ddlPlantID.SelectedValue;
                BindCellID();
                //ddlCellID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlantID_SelectedIndexChanged =" + ex.ToString());
            }
        }


        public void BindDataFromBeginning()
        {
            try
            {
                Session["CellCount_Cockpit"] = null;
                Session["CellCount_Schedule"] = null;
                setCompanyLogo();
                AllAdnonEntity andonBindCacheData = new AllAdnonEntity();
                AllAdnonEntity andonMainCacheData = new AllAdnonEntity();
                if (Session["BindCacheData"] == null)
                {
                    if (Session["MainCacheData"] != null)
                        Session["BindCacheData"] = (AllAdnonEntity)Session["MainCacheData"];
                }
                else
                {
                    if (Session[$"MainCacheData{HttpContext.Current.Session.SessionID}"] != null)
                    {
                        andonMainCacheData = (AllAdnonEntity)Session["MainCacheData"];
                        andonBindCacheData = (AllAdnonEntity)Session["BindCacheData"];
                        if (andonBindCacheData.AutoGeneratedID != andonMainCacheData.AutoGeneratedID)
                        {
                            Session["BindCacheData"] = null;
                            Session["BindCacheData"] = (AllAdnonEntity)Session["MainCacheData"];
                        }
                    }
                }
                andonBindCacheData = (AllAdnonEntity)Session["BindCacheData"];

                string ComputerName = HttpContext.Current.Request.Cookies["ComputerName"] != null ? HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString() : "Sample PC";
                Logger.WriteDebugLog($"(BindDataFromBeginning)Bind Cache Latest ID for PC {ComputerName}: {andonBindCacheData.AutoGeneratedID}");

                lblShift.Text = "Shift - " + AndonDBAccess.GetCurrentShift();

                settings = andonBindCacheData.AdnonSetting;
                try
                {
                    string format = andonBindCacheData.AdnonSetting.DateFormatForHeader + " " + andonBindCacheData.AdnonSetting.TimeFormatForHeader;
                    lblDateTime.InnerText = DateTime.Now.ToString(format);
                }
                catch (Exception ex)
                {
                    lblDateTime.InnerText = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }

                if (scrollingText.InnerText != settings.ScrollingText)
                {
                    scrollingText.InnerText = settings.ScrollingText;
                }
                if (settings.EnableImage || settings.EnableVideo)
                {
                    imageVideoInterval.Enabled = true;
                    imageVideoInterval.Interval = settings.EnableSlideShowAfterNSecInterval;
                }
                else
                {
                    imageVideoInterval.Enabled = false;
                }
                headerName.InnerText = settings.AndonTitle;

                if (Request.Cookies["AndonFrequency"] != null)
                {
                    if (ddlFrequency.Items.FindByValue(Request.Cookies["AndonFrequency"].Value.ToString()) != null)
                        ddlFrequency.SelectedValue = Request.Cookies["AndonFrequency"].Value.ToString();
                }
                if (Session["AndonPlant"] != null)
                {
                    if (ddlPlantID.Items.FindByValue(Session["AndonPlant"].ToString()) != null)
                        ddlPlantID.SelectedValue = Session["AndonPlant"].ToString();
                }
                if (Request.Cookies["AndonCellID"] != null)
                {
                    if (ddlCellID.Items.FindByValue(Request.Cookies["AndonCellID"].Value.ToString()) != null)
                        ddlCellID.SelectedValue = Request.Cookies["AndonCellID"].Value.ToString();
                    else
                        BindCellID();
                }

                Logger.WriteDebugLog($"Start Screens [PC]: {ComputerName}");
                showNextControl();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindDataFromBeginning_Andon = " + ex.Message);
            }
        }

        public void showNextControl()
        {
            try
            {
                HideAllControls();
                List<ScreenEntity> screenList = Session["EnabledScreens"] as List<ScreenEntity>;
                ddlFrequency.Visible = true;
                if (screenList == null || screenList.Count > 0)
                {
                    setScreens();
                    screenList = Session["EnabledScreens"] as List<ScreenEntity>;
                }
                ScreenEntity userControl = new ScreenEntity();
                if (chkDesktopView.Checked)
                {
                    Session["View"] = "DesktopView";
                    ddlScreenName.Visible = true;
                    userControl.Screen = ddlScreenName.SelectedValue.ToString();

                    if (userControl.Screen.Equals("cockpitControl", StringComparison.OrdinalIgnoreCase))
                    {
                        cockpitControl.Visible = true;
                        cockpitControl.BindCockpitData();
                    }
                    else if (userControl.Screen.Equals("ScheduleKTAControl", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlFrequency.Visible = false;
                        headerName.InnerText = "Schedule Data";
                        ScheduleKTAControl.Visible = true;
                        ScheduleKTAControl.BindScheduleData();
                    }
                }
                else
                {
                    Session["View"] = "AndonView";
                    ddlScreenName.Visible = false;
                    if (Session["RunningScreen"] == null)
                        userControl = screenList[0];
                    else
                    {
                        userControl = Session["RunningScreen"] as ScreenEntity;
                        var result = screenList.Where(k => k.Order > userControl.Order).FirstOrDefault();
                        if (result == null)
                        {
                            Session["RunningScreen"] = null;
                            BindDataFromBeginning();
                            return;
                        }
                        else
                            userControl = result;
                    }
                    Session["RunningScreen"] = userControl;
                    userControl = Session["RunningScreen"] as ScreenEntity;
                    if (userControl.Screen.Equals("cockpitControl", StringComparison.OrdinalIgnoreCase))
                    {
                        cockpitControl.Visible = true;
                        cockpitControl.BindCockpitData();
                    }
                    else if (userControl.Screen.Equals("ScheduleKTAControl", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlFrequency.Visible = false;
                        headerName.InnerText = "Schedule Data";
                        ScheduleKTAControl.Visible = true;

                        ScheduleKTAControl.BindScheduleData();
                    }
                    else if (userControl.Screen.Equals("PoojaCastingMeltingControl", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlFrequency.Visible = false;
                        ddlCellID.Visible = false;
                        headerName.InnerText = "CHEMICAL COMPOSITION DETAILS";
                        PoojaCastingMeltingControl.Visible = true;
                        PoojaCastingMeltingControl.BindPoojaMeltingData();
                    }
                    else if (userControl.Screen.Equals("slideShowControl", StringComparison.OrdinalIgnoreCase))
                    {
                        headerName.InnerText = "Image/Video";
                        slideShowControl.Visible = true;
                        slideShowControl.BindSlideShowData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("showNextControl = " + ex);
            }
        }

        private void setCompanyLogo()
        {
            try
            {
                string imagesPath = "~/CompanyLogo/";
                var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(imagesPath));

                //filtering to jpgs, but ideally not required
                List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();
                if (fileNames.Count > 0)
                {
                    customerLogo.ImageUrl = imagesPath + fileNames[0];
                }
                else
                {
                    customerLogo.ImageUrl = "Image/companyIcon.png";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setCompanyLogo = " + ex.Message);
            }
        }

        private void HideAllControls()
        {
            try
            {
                cockpitControl.Visible = false;
                slideShowControl.Visible = false;
                ScheduleKTAControl.Visible = false;
                PoojaCastingMeltingControl.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("HideAllControls = " + ex.Message);
            }
        }

        private static void SetSettingsDetails()
        {
            string ComputerName = "";
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();

                if (HttpContext.Current.Session["andonSettingList"] == null)
                    HttpContext.Current.Session["andonSettingList"] = AndonDBAccess.getAndonSettingDetails(ComputerName, "AndonCockpitAppSettings");
                if (HttpContext.Current.Session["MainsettingList"] == null)
                    HttpContext.Current.Session["MainsettingList"] = AndonDBAccess.getCockpitSettingDetails(ComputerName, "CockpitAndonOrder");
                if (HttpContext.Current.Session["RunOption"] == null)
                    HttpContext.Current.Session["RunOption"] = AndonDBAccess.GetRunOption(ComputerName, "ComputerRunOption");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SetSettingsDetails: {ex}");
            }
        }

        private static void insertLatestDataToMainCache()
        {
            List<AndonDefaultsEntity> andonSettingList = new List<AndonDefaultsEntity>();
            List<AndonSettingsEntity> MainsettingList = new List<AndonSettingsEntity>();
            string RunOption = string.Empty, ComputerName = string.Empty, plantID = "", cellID = "", frequency = "", shiftId = "";
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();

                Logger.WriteDebugLog($"Last Data inserted Time for PC {ComputerName}: {HttpContext.Current.Session["LastActivityTimeXYZ"]}");
                HttpContext.Current.Session["LastActivityTimeXYZ"] = DateTime.Now;
                TimeSpan ts = new TimeSpan(10, 10, 10);

                SetSettingsDetails();

                AllAdnonEntity data = new AllAdnonEntity();
                if (HttpContext.Current.Session["andonSettingList"] != null && HttpContext.Current.Session["MainsettingList"] != null && HttpContext.Current.Session["RunOption"] != null)
                {
                    andonSettingList = HttpContext.Current.Session["andonSettingList"] as List<AndonDefaultsEntity>;
                    MainsettingList = HttpContext.Current.Session["MainsettingList"] as List<AndonSettingsEntity>;
                    RunOption = HttpContext.Current.Session["RunOption"] as string;
                }
                else
                    HttpContext.Current.Response.Redirect("AndonPage.aspx");

                #region ------ Setting Data -----------
                AndonSettingEntity andonSetting = new AndonSettingEntity();
                andonSetting.ShowSmileyBlock = andonSettingList.Where(k => k.ValueInText.Equals("ShowSmileyBlock", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? "" : "none";
                andonSetting.SmileySize = andonSettingList.Where(k => k.ValueInText.Equals("ShowSmileyBlockSize", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                string sortby = andonSettingList.Where(k => k.ValueInText.Equals("OrderBy", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                var sortOption = sortby.Split(' ');
                if (sortOption.Length >= 2)
                {
                    andonSetting.OrderBy = sortOption[0].ToString();
                    andonSetting.SortOrder = (sortOption[1].ToString().Equals("Ascending", StringComparison.OrdinalIgnoreCase) ? "Asc" : "Desc");
                }
                andonSetting.PlantToDisplay = andonSettingList.Where(k => k.ValueInText.Equals("PlantToDisplay", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();

                string value = andonSettingList.Where(k => k.ValueInText.Equals("ScreenFlipInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.ScreenFlipInterval = (string.IsNullOrEmpty(value) ? 10 : Convert.ToInt32(value)) * 1000;
                value = andonSettingList.Where(k => k.ValueInText.Equals("DataDisplayInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.DataRefreshInterval = (string.IsNullOrEmpty(value) ? 10 : Convert.ToInt32(value)) * 1000;

                andonSetting.ShowFooterBlock = andonSettingList.Where(k => k.ValueInText.Equals("ShowFooterBlock", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? "" : "none";
                andonSetting.ShowMsgBox = andonSettingList.Where(k => k.ValueInText.Equals("MsgBlockEnabled", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? "inline-block" : "none";
                andonSetting.DateFormatForHeader = andonSettingList.Where(k => k.ValueInText.Equals("DateFormatForHeader", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (string.IsNullOrEmpty(andonSetting.DateFormatForHeader))
                {
                    andonSetting.DateFormatForHeader = "dd-MM-yyyy";
                }
                andonSetting.TimeFormatForHeader = andonSettingList.Where(k => k.ValueInText.Equals("TimeFormatForHeader", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (string.IsNullOrEmpty(andonSetting.TimeFormatForHeader))
                {
                    andonSetting.TimeFormatForHeader = "HH:mm:ss";
                }

                andonSetting.ScrollingText = andonSettingList.Where(k => k.ValueInText.Equals("ScrollingText", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();

                andonSetting.ShowCurvedBox = andonSettingList.Where(k => k.ValueInText.Equals("ShowCurvedBox", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? true : false;
                if (andonSetting.ShowCurvedBox)
                {
                    andonSetting.BorderClass = "addBorder";
                }
                else
                {
                    andonSetting.BorderClass = "removeBorder";
                }
                andonSetting.FormFontSize = andonSettingList.Where(k => k.ValueInText.Equals("FormFontSize", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();

                andonSetting.EnableImage = andonSettingList.Where(k => k.ValueInText.Equals("EnableImage", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? true : false;
                andonSetting.EnableVideo = andonSettingList.Where(k => k.ValueInText.Equals("EnableVideo", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? true : false;
                value = andonSettingList.Where(k => k.ValueInText.Equals("ImageFlipInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.EnableSlideShowAfterNSecInterval = (string.IsNullOrEmpty(value) ? 60 : Convert.ToInt32(value)) * 1000;
                andonSetting.ImagePath = andonSettingList.Where(k => k.ValueInText.Equals("ImageFilePath", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.VideoPath = andonSettingList.Where(k => k.ValueInText.Equals("VideoFilePath", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();

                andonSetting.AndonTitle = andonSettingList.Where(k => k.ValueInText.Equals("AndonTitle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontFamily = andonSettingList.Where(k => k.ValueInText.Equals("FontFamily", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontStyle = andonSettingList.Where(k => k.ValueInText.Equals("FontStyle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                int interval = andonSettingList.Where(k => k.ValueInText.Equals("ScheduleInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault();
                interval = interval == 0 ? 10 : interval;
                andonSetting.ScheduleInterval = interval * 1000;
                andonSetting.PoojaViewType = andonSettingList.Where(k => k.ValueInText.Equals("PoojaViewType", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (andonSetting.PoojaViewType == "" || andonSetting.PoojaViewType == null)
                {
                    andonSetting.PoojaViewType = "Table";
                }
                data.AdnonSetting = andonSetting;
                #endregion

                if (HttpContext.Current.Session["AndonPlant"] != null)
                    plantID = HttpContext.Current.Session["AndonPlant"].ToString();
                if (HttpContext.Current.Request.Cookies["AndonCellID"] != null)
                    cellID = HttpContext.Current.Request.Cookies["AndonCellID"].Value.ToString();
                if (HttpContext.Current.Request.Cookies["AndonFrequency"] != null)
                    frequency = HttpContext.Current.Request.Cookies["AndonFrequency"].Value.ToString();

                List<ScreenEntity> screenList = new List<ScreenEntity>();
                screenList = HttpContext.Current.Session["EnabledScreens"] as List<ScreenEntity>;

                if (screenList == null || screenList.Count == 0)
                {
                    setScreens_static();
                    screenList = HttpContext.Current.Session["EnabledScreens"] as List<ScreenEntity>;
                }

                foreach (var li in screenList)
                {
                    if (li.Screen.Equals("cockpitControl", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!frequency.Equals("Day", StringComparison.OrdinalIgnoreCase))
                            shiftId = AndonDBAccess.GetCurrentShift();
                       
                        if (RunOption == "RunByMachine")
                        {
                            data.CockpitData = AndonDBAccess.getMachineCockpitData("", plantID, shiftId, "", cellID, frequency, MainsettingList, ComputerName, RunOption, andonSetting.OrderBy, andonSetting.SortOrder, andonSetting.ShowSmileyBlock, andonSetting.SmileySize, andonSetting.ShowCurvedBox);
                            data.CockpitData.ForEach(k =>
                            {
                                k.GroupName = "";
                            });
                        }
                        else
                            data.CockpitData = AndonDBAccess.getMachineCockpitData("", plantID, shiftId, "", cellID, frequency, MainsettingList, ComputerName, RunOption, andonSetting.OrderBy, andonSetting.SortOrder, andonSetting.ShowSmileyBlock, andonSetting.SmileySize, andonSetting.ShowCurvedBox);
                    }
                   
                    else if (li.Screen.Equals("PoojaAndonMelting", StringComparison.OrdinalIgnoreCase))
                    {
                        DataTable dt2;
                        data.PoojaCastingData = AndonDBAccess.GetPoojaAndonMeltingData(plantID, out dt2);
                        data.PoojaCastingColors = dt2;
                    }
                }

                Guid guid = Guid.NewGuid();
                data.AutoGeneratedID = guid.ToString();
                HttpContext.Current.Session["MainCacheData"] = null;
                HttpContext.Current.Session["MainCacheData"] = data;
                Logger.WriteDebugLog($"Latest Data for PC {ComputerName} : {data.AutoGeneratedID}");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertAllDataToMainCache = " + ex);
            }
        }


        protected void btnPost_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataFromBeginning();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCockpitData = " + ex.Message);
            }
        }

        protected void imageVideoInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                HideAllControls();
                headerName.InnerText = "Image/Video";
                imageVideoInterval.Enabled = false;
                slideShowControl.Visible = true;
                slideShowControl.BindSlideShowData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("imageVideoInterval_Tick = " + ex.Message);
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void insertLatestDataToMainCacheMemory()
        {
            try
            {
                insertLatestDataToMainCache();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertLatestDataToMainCacheMemory = " + ex);
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setFlipIntervalToSession(int displayedItemCount)
        {
            try
            {
                if (HttpContext.Current.Session["RunningScreen"] != null && (HttpContext.Current.Session["RunningScreen"] as ScreenEntity).Screen.Equals("CockpitControl", StringComparison.OrdinalIgnoreCase))
                {
                    HttpContext.Current.Session["Rows_Cockpit"] = displayedItemCount;
                    int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips_Cockpit"]) / displayedItemCount));
                    HttpContext.Current.Session["Flips_Cockpit"] = flips;
                }
                else if (HttpContext.Current.Session["RunningScreen"] != null && (HttpContext.Current.Session["RunningScreen"] as ScreenEntity).Screen.Equals("ScheduleKTAControl", StringComparison.OrdinalIgnoreCase))
                {
                    HttpContext.Current.Session["Rows_Schedule"] = displayedItemCount;
                    int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips_Schedule"]) / displayedItemCount));
                    HttpContext.Current.Session["Flips_Schedule"] = flips;
                }
                else
                {
                    HttpContext.Current.Session["Rows"] = displayedItemCount;
                    int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips"]) / displayedItemCount));
                    HttpContext.Current.Session["Flips"] = flips;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setFlipIntervalToSession (Cockpit) = " + ex.Message);
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string setImageVideoFlipValue(string filePath, string param)
        {
            string result = "";
            try
            {
                if (System.IO.Directory.Exists(filePath))
                {
                    var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(filePath));
                    List<string> fileNames = (from flInfo in dir.GetFiles() select flInfo.Name).ToList();

                    HttpContext.Current.Session["Flips"] = fileNames.Count;
                    result = fileNames.Count.ToString();
                }
                else
                {
                    result = "pathNotExists";
                    Logger.WriteDebugLog("setImageVideoFlipValue (SlideShow) = " + param);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setImageVideoFlipValue (SlideShow) = " + ex.Message);
            }
            return result;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string getImageVideoInBase64(string filePath, int index)
        {
            string fileBase64 = "";
            try
            {
                string[] FileArray = Directory.GetFiles(filePath);

                byte[] fileByte = System.IO.File.ReadAllBytes(FileArray[index]);
                if (fileByte != null)
                {
                    fileBase64 = Convert.ToBase64String(fileByte);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getImageVideoInBase64 (SlideShow) = " + ex.Message);
            }
            return fileBase64;
        }

        protected void BtnSave_Click1(object sender, EventArgs e)
        {
            Response.Cookies["ComputerName"].Value = txtComputerName.Text;
            Response.Cookies["ComputerName"].Expires = DateTime.MaxValue;
            Response.Redirect("AndonPage.aspx", false);
        }

        protected void chkDesktopView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDesktopView.Checked)
                {
                    ddlScreenName.Items.Clear();
                    List<ScreenEntity> screensList = HttpContext.Current.Session["EnabledScreens"] as List<ScreenEntity>;
                    if (screensList.Count > 0)
                    {
                        foreach (var li in screensList)
                        {
                            ListItem item = new ListItem();
                            item.Value = li.Screen;
                            if (li.Screen.Equals("CockpitControl", StringComparison.OrdinalIgnoreCase))
                                item.Text = "Cockpit Screen";
                            else if (li.Screen.Equals("ScheduleKTAControl", StringComparison.OrdinalIgnoreCase))
                                item.Text = "Schedule Screen";
                            ddlScreenName.Items.Add(item);
                        }
                    }
                    ddlScreenName.SelectedValue = (Session["RunningScreen"] as ScreenEntity).Screen.ToString();
                    ddlScreenName.Visible = true;
                }
                else
                {
                    ddlScreenName.Visible = false;
                }
                showNextControl();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlScreenName_SelectedIndexChanged(object sender, EventArgs e)
        {
            showNextControl();
        }

        protected void BtnHome_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BindDataFromBeginning();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnOKSetting_Click(object sender, EventArgs e)
        {
            try
            {
                imageVideoInterval.Enabled = false;
                Response.Cookies["AndonCellID"].Value = ddlCellID.SelectedValue;
                Response.Cookies["AndonCellID"].Expires = DateTime.MaxValue;

                Response.Cookies["AndonFrequency"].Value = ddlFrequency.SelectedValue;
                Response.Cookies["AndonFrequency"].Expires = DateTime.MaxValue;

                Response.Redirect("AndonPage.aspx");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnOKSetting_Click: {ex.Message}");
            }
        }
    }
}