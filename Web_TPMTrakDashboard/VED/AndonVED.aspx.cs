using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using Web_TPMTrakDashboard.VED.Model;

namespace Web_TPMTrakDashboard.Andon
{
    public partial class AndonVED : System.Web.UI.Page
    {
        public static AndonSettingEntityVED uiSettings = new AndonSettingEntityVED();
        string ComputerName = string.Empty;
        int rows = 0;
        int rowsToTake = 0;
        int cellCount = 0;
        List<string> cellList = new List<string>();
        int Flips = 0;
        int curScreenIndex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    setCompanyLogo();
                    if (Request.Cookies["ComputerName"] != null)
                    {
                        ComputerName = Request.Cookies["ComputerName"].Value;
                        MainContainer.Visible = true;
                        AlertDiv.Visible = false;
                        ComputerDiv.Visible = false;
                    }
                    else
                    {
                        ComputerDiv.Visible = true;
                        MainContainer.Visible = false;
                        AlertDiv.Visible = false;
                        refreshTimer.Enabled = false;
                        return;
                    }
                    Session["AndonUISettings_VED"] = null;
                    Session["ScreenstoBeShownInAndon_VED"] = null;
                    Session["AndonChartFontSettings_VED"] = null;
                    Session["LatestDataForAndon_VED"] = null;
                    Session["CurrentFlipData_VED"] = null;
                    Session["KPIChartData"] = null;

                    Session["Rows"] = null;
                    Session["RowsToTake"] = null;
                    Session["Flips"] = null;
                    Session["CellCount"] = null;
                    Session["CellList"] = null;
                    Session["MainsettingList"] = null;
                    Session["CurrentScreen"] = null;
                    Session["CurrentScreenIndex"] = null;
                    Session["ImageVideoFlipInterval"] = null;
                    Session["AndonView"] = null;
                    Session["AndonCockpitBoxDimension"] = null;
                    Session["CustomWidthFlag"] = null;
                    Session["RunOption"] = null;

                    GetSettings();
                    UpdateDatainSessiontoLatest();
                    BindAndonData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Page_Load= " + ex.ToString());
            }
        }

        private void setCompanyLogo()
        {
            try
            {
                string ImagePath = "~/CompanyLogo/"; //Have to be changed to CompanyLogo
                var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(ImagePath));

                List<string> fileName = (from fileInfo in dir.GetFiles() select fileInfo.Name).ToList();

                if (fileName.Count > 0)
                {
                    customerLogo.ImageUrl = ImagePath + fileName[0];
                }
                else
                {
                    customerLogo.ImageUrl = "Image/companyIcon.png";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"setCompanyLogo_CockpitAndon: " + ex.Message);
            }
        }

        private static void GetSettings()
        {
            string ComputerName = string.Empty, Parameter = "CockpitAndonOrder";
            AndonSettingEntityVED settings = new AndonSettingEntityVED();
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();

                if (HttpContext.Current.Session["AndonUISettings_VED"] == null)
                    HttpContext.Current.Session["AndonUISettings_VED"] = settings = DBAccessVED.ViewAndonSettings(ComputerName, "AndonCockpitAppSettings");

                if (HttpContext.Current.Session["ScreenstoBeShownInAndon_VED"] == null)
                    HttpContext.Current.Session["ScreenstoBeShownInAndon_VED"] = DBAccessVED.GetAndonSettingsScreensData(ComputerName, settings, true);

                if (HttpContext.Current.Session["MainsettingList"] == null)
                    HttpContext.Current.Session["MainsettingList"] = AndonDBAccess.getCockpitSettingDetails(ComputerName, Parameter);

                if (HttpContext.Current.Session["AndonChartFontSettings_VED"] == null)
                    HttpContext.Current.Session["AndonChartFontSettings_VED"] = DBAccessVED.GetFontSettingsAndon(ComputerName);

                if (HttpContext.Current.Session["RunOption"] == null)
                    HttpContext.Current.Session["RunOption"] = AndonDBAccess.GetRunOption(ComputerName, "ComputerRunOption");

                if (HttpContext.Current.Session["AndonCockpitBoxDimension"] == null)
                    HttpContext.Current.Session["AndonCockpitBoxDimension"] = AndonDBAccess.GetBoxDimensions(ComputerName);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetSettings= " + ex.ToString());
            }
        }

        public static void UpdateDatainSessiontoLatest()
        {
            string ComputerName = string.Empty, RunOption = "RunByCell";
            AndonSettingEntityVED settings = new AndonSettingEntityVED();
            List<ScreenEntityVED> screenList = new List<ScreenEntityVED>();
            List<AndonSettingsEntity> andonParamDetails = new List<AndonSettingsEntity>();
            List<AndonFontSettingEntity> fontSettings = new List<AndonFontSettingEntity>();
            AndonEntity andonData = new AndonEntity();
            DataTable dt_Dimension = new DataTable();

            AndonSettingEntityVED andonSettings = new AndonSettingEntityVED();
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();

                if (HttpContext.Current.Session["AndonUISettings_VED"] == null || HttpContext.Current.Session["ScreenstoBeShownInAndon_VED"] == null || HttpContext.Current.Session["MainsettingList"] == null || HttpContext.Current.Session["AndonChartFontSettings_VED"] == null || HttpContext.Current.Session["RunOption_VED"] == null || HttpContext.Current.Session["AndonCockpitBoxDimension_VED"] == null)
                {
                    Logger.WriteDebugLog("Session expired. Settings resulted null.");
                    GetSettings();
                }

                andonSettings = HttpContext.Current.Session["AndonUISettings_VED"] as AndonSettingEntityVED;
                screenList = HttpContext.Current.Session["ScreenstoBeShownInAndon_VED"] as List<ScreenEntityVED>;
                andonParamDetails = HttpContext.Current.Session["MainsettingList"] as List<AndonSettingsEntity>;
                fontSettings = HttpContext.Current.Session["AndonChartFontSettings_VED"] as List<AndonFontSettingEntity>;
                RunOption = HttpContext.Current.Session["RunOption"] as string;
                dt_Dimension = HttpContext.Current.Session["AndonCockpitBoxDimension"] as DataTable;

                andonSettings.DataRefreshInterval = (andonSettings.DataRefreshInterval <= 0 ? 30 : andonSettings.DataRefreshInterval) * 1000;

                uiSettings = andonSettings;

                //if (settings.ImageEnabled == 1 || settings.VideoEnabled == 1)
                //    screenList.Add(new ScreenEntityVED() { ValueInText = "ImageVideoSlider", ScreenName = "Image / Video", Parameter = "", IsVisible = true });

                string curShift = string.Empty;
                curShift = AndonDBAccess.GetCurrentShift();

                HttpContext.Current.Session["ImageVideoFlipInterval"] = andonSettings.SlideshowInterval;

                foreach (ScreenEntityVED screen in screenList)
                {
                    if (screen.IsVisible)
                    {
                        switch (screen.ValueInText)
                        {
                            case "MonthlyPlantEfficiency":
                                andonData.KPIdata = DBAccessVED.GetPlantEfficiencyData_Andon(fontSettings);
                                continue;
                            case "CurrentMonthDowntime":
                                andonData.downTimeData = DBAccessVED.GetDownTimeforCurrentMonth_Andon(fontSettings);
                                continue;
                            case "HourlyPartCountMachineLevel":
                                andonData.HourlyTargetData = DBAccessVED.GetHourlyTargetAndon("", curShift, fontSettings);
                                continue;
                            case "Andon":
                                andonData.genericAndonData = AndonDBAccess.getMachineCockpitData("", "", "", "", "", "", andonParamDetails, ComputerName, "RunByCell", settings.Sortorder, settings.orderby, (settings.EmojiEnabled == 1 ? "" : "none"), settings.EmojiSize.ToString(), settings.ShowCurvedBoxes == 1);
                                if (RunOption == "RunByMachine")
                                    andonData.genericAndonData.ForEach(k =>
                                    {
                                        k.GroupName = "";
                                    });

                                if (dt_Dimension.Rows.Count > 0)
                                {
                                    if (dt_Dimension.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("UseCustomWidth", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInBool"].ToString()).FirstOrDefault() == "1")
                                    {
                                        HttpContext.Current.Session["CustomWidthFlag"] = "1";
                                        string BoxWidth = dt_Dimension.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("BoxWidth", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                                        string LeftMargin = dt_Dimension.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("LeftMargin", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                                        string TopMargin = dt_Dimension.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("TopMargin", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInText2"].ToString()).FirstOrDefault();
                                        andonData.genericAndonData.ForEach(x =>
                                        {
                                            x.CockpitBoxWidth = BoxWidth;
                                            x.TopMargin = TopMargin;
                                            x.LeftMargin = LeftMargin;
                                            x.TableLayout = "fixed";
                                        });
                                    }
                                }
                                continue;
                            default: continue;
                        }
                    }
                }

                HttpContext.Current.Session["LatestDataForAndon_VED"] = andonData;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateDatainSessiontoLatest= " + ex.ToString());
            }
        }

        public void BindAndonData()
        {
            try
            {
                if (Session["LatestDataForAndon_VED"] != null)
                    Session["CurrentFlipData_VED"] = Session["LatestDataForAndon_VED"] as AndonEntity;
                else
                {
                    Logger.WriteDebugLog(ComputerName + ": Latest data in session found to be NULL. Possible reasons might be Session expiration.");
                    Response.Redirect("AndonVED.aspx", true);
                }

                Session["CurrentScreen"] = null;
                curScreenIndex = 0;
                Session["CurrentScreenIndex"] = curScreenIndex;
                CallNextScreen();

                string curScreen = string.Empty;
                if (Session["CurrentScreen"] != null)
                    curScreen = Session["CurrentScreen"].ToString();

                if (!curScreen.Equals("ImageVideoSlider"))
                {
                    refreshTimer.Enabled = true;
                    refreshTimer.Interval = (uiSettings.ScreenFlipInterval > 0 ? uiSettings.ScreenFlipInterval : 10) * 1000;
                }
                else
                    refreshTimer.Enabled = false;



            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindAndonData= " + ex.ToString());
            }
        }

        private void CallNextScreen()
        {
            try
            {
                SetDateShift();
                if (Session["ScreenstoBeShownInAndon_VED"] != null)
                {
                    List<ScreenEntityVED> list = new List<ScreenEntityVED>();
                    list = Session["ScreenstoBeShownInAndon_VED"] as List<ScreenEntityVED>;

                    if (!list.Where(x => x.IsVisible == true).Any())
                    {
                        refreshTimer.Enabled = false;
                        AlertDiv.Visible = true;
                        ProductionAndonContainer.Visible = false;
                        lblAlertMessage.Text = "Enable screens in Settings for this device to run Andon.";
                        return;
                    }
                    else
                    {
                        AlertDiv.Visible = false;
                        ProductionAndonContainer.Visible = true;

                        List<ScreenEntityVED> screenList = new List<ScreenEntityVED>();
                        screenList = Session["ScreenstoBeShownInAndon_VED"] as List<ScreenEntityVED>;

                        AndonEntity andonData = new AndonEntity();
                        if (Session["CurrentFlipData_VED"] != null)
                            andonData = Session["CurrentFlipData_VED"] as AndonEntity;

                        int.TryParse(Session["CurrentScreenIndex"].ToString(), out curScreenIndex);
                        HideAllContainers();
                        if (curScreenIndex >= screenList.Count)
                        {
                            BindAndonData();
                            return;
                        }

                        string NextScreen = screenList[curScreenIndex].ValueInText;
                        Session["CurrentScreen"] = NextScreen;
                        headerName.Text = screenList[curScreenIndex].ScreenName;
                        switch (NextScreen)
                        {
                            case "MonthlyPlantEfficiency":
                                MonthlyPlantEfficiency.Visible = true;
                                resetVariables();

                                ScriptManager.RegisterStartupScript(this, GetType(), "EfficiencyChart", "BindEfficiencyCharts();", true);
                                break;
                            case "CurrentMonthDowntime":
                                CurrentMonthDowntime.Visible = true;
                                resetVariables();
                                ScriptManager.RegisterStartupScript(this, GetType(), "downtimcharts", "BindDowntimeCharts();", true);
                                break;
                            case "HourlyPartCountMachineLevel":
                                resetVariables();
                                HourlyPartCountMachineLevel.Visible = true;
                                ScriptManager.RegisterStartupScript(this, GetType(), "container", "SetContainerWidth();", true);
                                List<HourlyTargetAndonEntity> hourlyTargetData = new List<HourlyTargetAndonEntity>();
                                rows = 4;
                                hourlyTargetData = andonData.HourlyTargetData;
                                var result = hourlyTargetData.Select(item => new HourlyTargetAndonEntity { HourTiminigs = item.HourTiminigs, IsCurrentHour = item.IsCurrentHour, HourDataByMachine = item.HourDataByMachine.Take(rows).ToList() }).ToList();

                                lvhourlyTargetCount.DataSource = hourlyTargetData.Select(item => new HourlyTargetAndonEntity { HourTiminigs = item.HourTiminigs, fontSize=item.fontSize, IsCurrentHour = item.IsCurrentHour, HourDataByMachine = item.HourDataByMachine.Take(rows).ToList() }).ToList();
                                lvhourlyTargetCount.DataBind();

                                if (hourlyTargetData.Count > 0)
                                {

                                    Flips = Convert.ToInt32(Math.Ceiling((double)hourlyTargetData.Max(x => x.HourDataByMachine.Count()) / rows));

                                    rowsToTake = 1;
                                    Session["RowsToTake"] = rowsToTake;
                                    Session["Rows"] = rows;
                                    Session["Flips"] = Flips;

                                    rowsToTake++;
                                }


                                break;
                            case "Andon":
                                resetVariables();
                                Andon.Visible = true;
                                List<CockpitData> genericAndonData = new List<CockpitData>();
                                genericAndonData = andonData.genericAndonData;
                                if (genericAndonData.Count > 0)
                                {
                                    //if (Request.Cookies["AndonCellID"].Value.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(Request.Cookies["AndonCellID"].Value.ToString()))
                                    BindCockpitData(genericAndonData);
                                }
                                break;
                            case "ImageVideoSlider":
                                slideShowContainer.Visible = true;
                                refreshTimer.Enabled = false;
                                BindSliderImage();
                                break;
                            default: break;
                        }

                        curScreenIndex++;
                        Logger.WriteDebugLog(curScreenIndex.ToString());
                        Session["CurrentScreenIndex"] = curScreenIndex;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("CallNextScreen= " + ex.ToString());
            }
        }

        private void SetDateShift()
        {
            AndonSettingEntityVED settings = new AndonSettingEntityVED();
            try
            {
                if (Session["AndonUISettings_VED"] != null)
                    settings = Session["AndonUISettings_VED"] as AndonSettingEntityVED;

                string DateFormat = settings.DateFormat;
                string TimeFormat = settings.TimeFormat;

                string Shift = AndonDBAccess.GetCurrentShift();

                lblShift.Text = Shift.ToLower().Contains("shift") ? Shift : "Shift - " + Shift;

                try
                {
                    if (!string.IsNullOrEmpty(DateFormat) && !string.IsNullOrEmpty(TimeFormat))
                    {
                        string format = DateFormat + " " + TimeFormat;
                        lblDateTime.Text = DateTime.Now.ToString(format);
                    }
                    else
                        lblDateTime.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
                catch (Exception ex)
                {
                    lblDateTime.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SetDateShift= " + ex.ToString());
            }
        }

        private void resetVariables()
        {
            try
            {
                Flips = 0;
                rowsToTake = 0;
                rows = 0;
                cellCount = 0;
                cellList = new List<string>();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("resetVariables = " + ex.ToString());
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies["ComputerName"].Value = txtComputerName.Text;
                Response.Cookies["ComputerName"].Expires = DateTime.MaxValue;
                Response.Redirect("AndonVED.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BtnSave_Click= " + ex.ToString());
            }
        }

        protected void refreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["CurrentScreen"] != null)
                {
                    SetDateShift();
                    string CurrentScreen = Session["CurrentScreen"].ToString();

                    AndonEntity andonData = new AndonEntity();
                    if (Session["CurrentFlipData_VED"] != null)
                        andonData = Session["CurrentFlipData_VED"] as AndonEntity;

                    switch (CurrentScreen)
                    {
                        case "MonthlyPlantEfficiency":
                            CallNextScreen();
                            break;
                        case "CurrentMonthDowntime":
                            CallNextScreen();
                            break;
                        case "HourlyPartCountMachineLevel":
                            ScriptManager.RegisterStartupScript(this, GetType(), "container", "SetContainerWidth();", true);
                            int.TryParse(Session["Flips"].ToString(), out Flips);
                            int.TryParse(Session["RowsToTake"].ToString(), out rowsToTake);
                            int.TryParse(Session["Rows"].ToString(), out rows);
                            if (Flips <= 1)
                                CallNextScreen();
                            else
                            {
                                List<HourlyTargetAndonEntity> hourlyTargetData = new List<HourlyTargetAndonEntity>();
                                int skipRows = rows * rowsToTake;
                                hourlyTargetData = andonData.HourlyTargetData;

                                var result = hourlyTargetData.Select(item => new HourlyTargetAndonEntity { HourTiminigs = item.HourTiminigs, IsCurrentHour = item.IsCurrentHour, HourDataByMachine = item.HourDataByMachine.Skip(skipRows).Take(rows).ToList() }).ToList();

                                lvhourlyTargetCount.DataSource = hourlyTargetData.Select(item => new HourlyTargetAndonEntity { HourTiminigs = item.HourTiminigs, fontSize = item.fontSize, IsCurrentHour = item.IsCurrentHour, HourDataByMachine = item.HourDataByMachine.Skip(skipRows).Take(rows).ToList() }).ToList();
                                lvhourlyTargetCount.DataBind();

                                if (hourlyTargetData.Count > 0)
                                {
                                    rowsToTake++;
                                    Session["RowsToTake"] = rowsToTake;
                                }
                                Flips--;
                                Session["Flips"] = Flips;
                            }
                            break;
                        case "Andon":
                            if (Session["Flips"] != null)
                            {
                                int.TryParse(Session["Flips"].ToString(), out Flips);
                                int.TryParse(Session["Rows"].ToString(), out rows);

                                List<CockpitData> list = new List<CockpitData>();
                                list = andonData.genericAndonData;

                                if (Flips > 1)
                                {
                                    int.TryParse(Session["RowsToTake"].ToString(), out rowsToTake);
                                    int.TryParse(Session["Rows"].ToString(), out rows);

                                    if (Session["CockpitData"] != null)
                                        list = Session["CockpitData"] as List<CockpitData>;

                                    int skiprows = rowsToTake * rows;
                                    lvCockpit.DataSource = list.Skip(skiprows).Take(rows);
                                    lvCockpit.DataBind();
                                    Flips--;
                                    rowsToTake++;
                                    Session["Flips"] = Flips;
                                    Session["RowsToTake"] = rowsToTake;

                                    ScriptManager.RegisterStartupScript(this, GetType(), "setMachineFontSize", "setMachineFontSize();", true);
                                    ScriptManager.RegisterStartupScript(this, GetType(), "setBoxwidth", "SetIconicBoxWidth();", true);
                                }
                                else
                                {
                                    if (Session["CellCount"] != null)
                                    {
                                        int.TryParse(Session["CellCount"].ToString(), out cellCount);
                                        cellList = Session["CellList"] as List<string>;
                                        if (cellCount >= cellList.Count)
                                        {
                                            Session["CellCount"] = 0;
                                            CallNextScreen();
                                        }
                                        else
                                        {
                                            BindCockpitData(list);
                                        }

                                    }
                                    else
                                        CallNextScreen();
                                }
                            }
                            else
                                CallNextScreen();
                            break;
                        default: break;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("refreshTimer_Tick= " + ex.ToString());
            }
        }

        private void HideAllContainers()
        {
            MonthlyPlantEfficiency.Visible = false;
            CurrentMonthDowntime.Visible = false;
            HourlyPartCountMachineLevel.Visible = false;
            Andon.Visible = false;
            slideShowContainer.Visible = false;
        }

        private void BindCockpitData(List<CockpitData> genericAndonData)
        {
            try
            {
                if (Session["RunOption"] == null)
                    GetSettings();

                if (Session["RunOption"].ToString() == "RunByMachine")
                    cellLabelDiv.Visible = false;
                else
                    cellLabelDiv.Visible = true;

                if (Session["CustomWidthFlag"] != null)
                    hdnUseCustomWidthFlag.Value = Session["CustomWidthFlag"] as string;

                cellList = genericAndonData.Select(x => x.GroupName).Distinct().ToList();
                Session["CellList"] = cellList;
                if (Session["CellCount"] == null)
                    Session["CellCount"] = 0;
                int.TryParse(Session["CellCount"].ToString(), out cellCount);

                genericAndonData = genericAndonData.Where(x => x.GroupName == cellList[cellCount]).ToList();
                lblCellName.Text = cellList[cellCount];
                cellCount++;
                Session["CellCount"] = cellCount;

                //else
                //    cellLabelDiv.Visible = false;

                lvCockpit.DataSource = genericAndonData;
                lvCockpit.DataBind();

                if (genericAndonData.Count > 0)
                {
                    rows = Flips = genericAndonData.Count;
                    rowsToTake = 1;
                    Session["CockpitData"] = genericAndonData;
                    Session["RowsToTake"] = rowsToTake;
                    Session["Rows"] = rows;
                    Session["Flips"] = Flips;
                    rowsToTake++;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "andonFlipssettings", "setFlipInterval();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCockpitData= " + ex.ToString());
            }
        }

        private void BindSliderImage()
        {
            try
            {
                if (Session["AndonUISettings_VED"] != null)
                {
                    AndonSettingEntityVED andonData = (AndonSettingEntityVED)Session["AndonUISettings_VED"];

                    if (andonData.ImageEnabled == 1 || andonData.VideoEnabled == 1)
                    {
                        List<AndonDefaultsEntity> imgVideoList = new List<AndonDefaultsEntity>();
                        if (andonData.ImageEnabled == 1)
                        {
                            //var dir = new DirectoryInfo(settings.ImagePath);
                            List<string> fileNames = Directory.GetFiles(andonData.ImagePath).ToList();
                            if (fileNames.Count > 0)
                            {
                                for (int i = 0; i < fileNames.Count; i++)
                                {
                                    var dotSpint = fileNames[i].Split('.');
                                    string extension = dotSpint[dotSpint.Length - 1];
                                    if (extension.Equals("jpeg", StringComparison.OrdinalIgnoreCase) || extension.Equals("png", StringComparison.OrdinalIgnoreCase) || extension.Equals("jpg", StringComparison.OrdinalIgnoreCase))
                                    {
                                        AndonDefaultsEntity data = new AndonDefaultsEntity();
                                        byte[] fileByte = System.IO.File.ReadAllBytes(fileNames[i]);
                                        if (fileByte != null)
                                        {
                                            data.ValueInText = Convert.ToBase64String(fileByte);
                                            data.Parameter = fileNames[i];
                                            imgVideoList.Add(data);
                                        }
                                    }
                                }
                            }
                        }
                        if (andonData.VideoEnabled == 1)
                        {
                            var dir = new DirectoryInfo(andonData.VideoPath);
                            //List<string> fileNames = (from flInfo in dir.GetFiles("*.*", SearchOption.TopDirectoryOnly) select flInfo.Name).ToList();
                            List<string> fileNames = Directory.GetFiles(andonData.VideoPath).ToList();
                            if (fileNames.Count > 0)
                            {
                                for (int i = 0; i < fileNames.Count; i++)
                                {
                                    var dotSpint = fileNames[i].Split('.');
                                    string extension = dotSpint[dotSpint.Length - 1];
                                    if ((extension == "mp4") || (extension == "wmv") || (extension == "avi") || (extension == "mov") || (extension == "qt") || (extension == "yuv") || (extension == "mkv") ||
                                   (extension == "webm") || (extension == "flv") || (extension == "ogg"))
                                    {
                                        AndonDefaultsEntity data = new AndonDefaultsEntity();
                                        byte[] fileByte = System.IO.File.ReadAllBytes(fileNames[i]);
                                        if (fileByte != null)
                                        {
                                            data.Parameter = fileNames[i];
                                            data.ValueInText = Convert.ToBase64String(fileByte);

                                            imgVideoList.Add(data);
                                        }
                                    }
                                }
                            }
                        }

                        if (imgVideoList.Count > 0)
                        {
                            int slideCount = 0;
                            var carouselInnerHtml = new StringBuilder();
                            var indicatorsHtml = new StringBuilder(@"<ol class='carousel-indicators'>");

                            for (int i = 0; i < imgVideoList.Count; i++)
                            {
                                var result = Path.GetExtension(imgVideoList[i].Parameter).ToLower();// fileNames[i].Substring(fileNames[i].LastIndexOf(".") + 1);
                                if (!result.ToString().Equals(".scc", StringComparison.OrdinalIgnoreCase))
                                {

                                    if ((result == ".mp4") || (result == ".wmv") || (result == ".avi") || (result == ".mov") || (result == ".qt") || (result == ".yuv") || (result == ".mkv") ||
                                        (result == ".webm") || (result == ".flv") || (result == ".ogg"))
                                    {
                                        if (carouselInnerHtml.ToString().Contains("active"))
                                        {
                                            carouselInnerHtml.AppendLine("<div class='item' data-slide='" + slideCount + "'>");
                                        }
                                        else
                                        {
                                            carouselInnerHtml.AppendLine("<div class='item active' data-slide='" + slideCount + "' >");
                                        }
                                        carouselInnerHtml.AppendFormat("<video  class='slide-image embed-responsive-item center-block makeStyle' id='v{0}' alt='Slide #{0}' playsinline='playsinline'  autoplay='autoplay' muted='muted' controls>\r\n", slideCount + 1);
                                        carouselInnerHtml.AppendFormat("<source src='{0}' type='video/mp4'>\r\n", "data:video/mp4;base64," + imgVideoList[i].ValueInText);
                                        carouselInnerHtml.AppendLine("</video>");
                                        carouselInnerHtml.AppendLine("</div>");
                                        indicatorsHtml.AppendLine(slideCount == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class=''></li>");

                                    }
                                    else
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
                                        carouselInnerHtml.AppendLine("<img class='img-responsive img-rounded center-block makeStyle' src='data:image/png;base64," + imgVideoList[i].ValueInText + "' alt='Slide #" + (slideCount + 1) + "'>");
                                        carouselInnerHtml.AppendLine("</div>");
                                        indicatorsHtml.AppendLine(slideCount == 0 ? @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class='active'></li>" : @"<li data-target='#myCarousel' data-slide-to='" + slideCount + "' class=''></li>");

                                    }
                                    slideCount++;
                                }
                            }
                            indicatorsHtml.AppendLine("</ol>");

                            ltlCarouselImages.Text = carouselInnerHtml.ToString();
                            ltlCarouselIndicators.Text = indicatorsHtml.ToString();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setImages", "showImageVideo();", true);
                        }
                        else
                            BindAndonData();
                    }
                    else
                        BindAndonData();
                }
                else
                    BindAndonData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindSliderImage= " + ex.ToString());
            }

        }


        protected void btnPost_Click(object sender, EventArgs e)
        {
            try
            {
                BindAndonData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("refreshTimer_Tick= " + ex.ToString());
            }
        }

        [WebMethod(EnableSession = true)]
        public static AndonKPIEntity GetKPIChartData()
        {
            AndonKPIEntity data = new AndonKPIEntity();
            AndonEntity curFlipData = new AndonEntity();
            try
            {
                if (HttpContext.Current.Session["CurrentFlipData_VED"] != null)
                {
                    curFlipData = HttpContext.Current.Session["CurrentFlipData_VED"] as AndonEntity;

                    if (curFlipData.KPIdata != null)
                    {
                        data = curFlipData.KPIdata;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetKPIChartData= " + ex.ToString());
            }
            return data;
        }

        [WebMethod(EnableSession = true)]
        public static DowntimeMainEntity GetDowntimeScreenData()
        {
            DowntimeMainEntity data = new DowntimeMainEntity();
            AndonEntity curFlipData = new AndonEntity();
            try
            {
                if (HttpContext.Current.Session["CurrentFlipData_VED"] != null)
                {
                    curFlipData = HttpContext.Current.Session["CurrentFlipData_VED"] as AndonEntity;

                    data = curFlipData.downTimeData;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetKPIChartData= " + ex.ToString());
            }
            return data;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setFlipIntervalToSession(int displayedItemCount)
        {
            try
            {
                HttpContext.Current.Session["Rows"] = displayedItemCount;

                if (displayedItemCount != 0)
                {
                    int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips"]) / displayedItemCount));
                    HttpContext.Current.Session["Flips"] = flips;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"setFlipIntervalToSession {HttpContext.Current.Session.SessionID} = {ex.Message}");
            }
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
        public static void insertLatestDataToMainCacheMemory()
        {
            try
            {
                UpdateDatainSessiontoLatest();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"insertLatestDataToMainCacheMemory = {ex.Message}");
            }
        }

        protected void lvGroupKPIContainer_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static string LoginValidation(string userName, string password)
        {
            string result = "not Valid";
            try
            {
                //result = DataBaseAccess.
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("LoginValidation= " + ex.ToString());
            }
            return result;
        }

        protected void btnSettings_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}