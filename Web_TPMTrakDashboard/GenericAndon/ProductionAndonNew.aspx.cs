using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using System.Web.Caching;
using System.Data;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class ProductionAndonNew : System.Web.UI.Page
    {
        public static AndonSettingEntity settings = new AndonSettingEntity();
        bool isPostBack = false;
        int cellCount = 0;
        int rows = 0;
        int rowsToTake = 0;
        int flips = 0;
        List<string> cellList = new List<string>();
        //DateTime nextRefreshTime = DateTime.Now.Date.AddDays(1);

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AndonPage"] = this;
            Logger.WriteDebugLog("!...PAGE LOAD STARTED...!");

            if (!IsPostBack)
            {
                Logger.WriteDebugLog("******* PAGE POST BACK *********");
                Session["PageType"] = "ProductionAndon";
                //hdnRefreshTimer.Value = DateTime.Now.Date.AddDays(1).ToString();
                if (Request.Cookies["ComputerName"] == null)
                {
                    PageRefreshtimer.Enabled = false;
                    ComputerDiv.Visible = true;
                    divHeader.Visible = false;
                    setCompanyLogo();
                    //BindFrequency();
                    return;
                }
                else
                {
                    ComputerDiv.Visible = false;
                    divHeader.Visible = true;
                }


                Session["CockpitData"] = null;
                Session["Rows"] = null;
                Session["RowsToTake"] = null;
                Session["Flips"] = null;
                Session["CellCount"] = null;
                Session["CellList"] = null;
                Session["RunOption"] = null;
                Session["MainCacheData"] = null;
                Session["BindCacheData"] = null;
                Session["andonSettingList"] = null;
                Session["MainsettingList"] = null;
                Session["AndonView"] = null;
                Session["AndonCockpitBoxDimension"] = null;
                Session["CustomWidthFlag"] = null;

                BindPlantID();
                BindCellID();

                //BindFrequency();

                SetSettingsDetails();
                InsertLatestDataToMainCache();
                BindCockpitData();
            }

            else if (Request.Cookies["ComputerName"] != null && (Session["MainCacheData"] == null || Session["BindCacheData"] == null))
            {
                Logger.WriteDebugLog("Manual POSTBACK");
                Response.Redirect("ProductionAndonNew.aspx");
                return;
                //InsertLatestDataToMainCache();
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

        private void BindPlantID()
        {
            try
            {
                ddlPlantID.DataSource = AndonDBAccess.getPlantID_Andon();
                ddlPlantID.DataBind();
                if (ddlPlantID.Items.Count > 0)
                {
                    ddlPlantID.Items.Insert(0, new ListItem() { Text = "Plant All", Value = "All" });
                }

                if (Request.Cookies["AndonPlant"] == null)
                {
                    Response.Cookies["AndonPlant"].Value = ddlCellID.SelectedValue;
                    Response.Cookies["AndonPlant"].Expires = DateTime.MaxValue;
                }
                else
                {
                    if (ddlPlantID.Items.FindByValue(Request.Cookies["AndonPlant"].Value.ToString()) != null)
                        ddlPlantID.SelectedValue = Request.Cookies["AndonPlant"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindPlantID_CockpitAndon: {ex}");
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
                {
                    ddlCellID.Items.Insert(0, new ListItem() { Text = "Cell All", Value = "All" });
                }

                if (Request.Cookies["AndonCellID"] == null)
                {
                    Response.Cookies["AndonCellID"].Value = ddlCellID.SelectedValue;
                    Response.Cookies["AndonCellID"].Expires = DateTime.MaxValue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindCellID_CockpitAndon: {ex}");
            }
        }

        //private void BindFrequency()
        //{
        //    try
        //    {
        //        List<ListItem> list = new List<ListItem>();
        //        list.Add(new ListItem() { Value = "Last One", Text = "Latest One Hour" });
        //        list.Add(new ListItem() { Value = "Two Hours", Text = "Latest Two Hours" });
        //        list.Add(new ListItem() { Value = "Three Hours", Text = "Latest Three Hours" });
        //        list.Add(new ListItem() { Value = "Shift", Text = "Shift" });
        //        list.Add(new ListItem() { Value = "Day", Text = "Day" });
        //        ddlFrequency.DataSource = list;
        //        ddlFrequency.DataTextField = "Text";
        //        ddlFrequency.DataValueField = "Value";
        //        ddlFrequency.DataBind();
        //        if (Request.Cookies["AndonFrequency"] == null)
        //        {
        //            Response.Cookies["AndonFrequency"].Value = ddlFrequency.SelectedValue;
        //            Response.Cookies["AndonFrequency"].Expires = DateTime.MaxValue;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog($"BindFrequency_CockpitAndon: {ex}");
        //    }
        //}

        private static void SetSettingsDetails()
        {
            string ComputerName = "", Parameter = "CockpitAndonOrder";
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    Parameter = "AndonParameter_KM";

                if (HttpContext.Current.Session["andonSettingList"] == null)
                    HttpContext.Current.Session["andonSettingList"] = AndonDBAccess.getAndonSettingDetails(ComputerName, "AndonCockpitAppSettings");
                if (HttpContext.Current.Session["MainsettingList"] == null)
                    HttpContext.Current.Session["MainsettingList"] = AndonDBAccess.getCockpitSettingDetails(ComputerName, Parameter);
                if (HttpContext.Current.Session["RunOption"] == null)
                    HttpContext.Current.Session["RunOption"] = AndonDBAccess.GetRunOption(ComputerName, "ComputerRunOption");
                if (HttpContext.Current.Session["AndonCockpitBoxDimension"] == null)
                    HttpContext.Current.Session["AndonCockpitBoxDimension"] = AndonDBAccess.GetBoxDimensions(ComputerName);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SetSettingsDetails: {ex}");
            }
        }

        private static void InsertLatestDataToMainCache()
        {
            string ComputerName = "", RunOption = "", PlantID = "", CellID = "", Frequency = "", ShiftID = "";
            DataTable dt_Dimension = new DataTable();
            List<AndonSettingsEntity> cockpitSettingsList = new List<AndonSettingsEntity>(); //font settings
            List<AndonDefaultsEntity> GeneralSettingsList = new List<AndonDefaultsEntity>(); //General settings
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString().Trim();

                HttpContext.Current.Session[$"LastActivityTimeXYZ{HttpContext.Current.Session.SessionID}"] = DateTime.Now;
                Logger.WriteDebugLog($"Inserting New Data For PC: {ComputerName} at {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}");
                TimeSpan ts = new TimeSpan(10, 10, 10);


                AllAdnonEntity data = new AllAdnonEntity();
                if (HttpContext.Current.Session["andonSettingList"] != null && HttpContext.Current.Session["MainsettingList"] != null && HttpContext.Current.Session["RunOption"] != null && HttpContext.Current.Session["AndonCockpitBoxDimension"] != null)
                {
                    GeneralSettingsList = HttpContext.Current.Session["andonSettingList"] as List<AndonDefaultsEntity>;
                    cockpitSettingsList = HttpContext.Current.Session["MainsettingList"] as List<AndonSettingsEntity>;
                    RunOption = HttpContext.Current.Session["RunOption"] as string;
                    dt_Dimension = HttpContext.Current.Session["AndonCockpitBoxDimension"] as DataTable;
                }
                else
                    HttpContext.Current.Response.Redirect("ProductionAndonNew.aspx");

                #region --- Settings Data --- 
                AndonSettingEntity andonSetting = new AndonSettingEntity();
                andonSetting.ShowSmileyBlock = GeneralSettingsList.Where(k => k.ValueInText.Equals("ShowSmileyBlock", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? "" : "none";
                andonSetting.SmileySize = GeneralSettingsList.Where(k => k.ValueInText.Equals("ShowSmileyBlockSize", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                string sortby = GeneralSettingsList.Where(k => k.ValueInText.Equals("OrderBy", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (!string.IsNullOrEmpty(sortby))
                {
                    var sortOption = sortby.Split(' ');
                    if (sortOption.Length >= 2)
                    {
                        andonSetting.OrderBy = sortOption[0].ToString();
                        andonSetting.SortOrder = (sortOption[1].ToString().Equals("Ascending", StringComparison.OrdinalIgnoreCase) ? "Asc" : "Desc");
                    }
                }
                andonSetting.PlantToDisplay = GeneralSettingsList.Where(k => k.ValueInText.Equals("PlantToDisplay", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();

                string value = GeneralSettingsList.Where(k => k.ValueInText.Equals("ScreenFlipInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.ScreenFlipInterval = (string.IsNullOrEmpty(value) ? 10 : Convert.ToInt32(value)) * 1000;
                value = GeneralSettingsList.Where(k => k.ValueInText.Equals("DataDisplayInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.DataRefreshInterval = (string.IsNullOrEmpty(value) ? 10 : Convert.ToInt32(value)) * 1000;

                andonSetting.ShowFooterBlock = GeneralSettingsList.Where(k => k.ValueInText.Equals("ShowFooterBlock", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? "" : "none";
                andonSetting.ShowMsgBox = GeneralSettingsList.Where(k => k.ValueInText.Equals("MsgBlockEnabled", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? "inline-block" : "none";
                andonSetting.DateFormatForHeader = GeneralSettingsList.Where(k => k.ValueInText.Equals("DateFormatForHeader", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (string.IsNullOrEmpty(andonSetting.DateFormatForHeader))
                    andonSetting.DateFormatForHeader = "dd-MM-yyyy";

                andonSetting.TimeFormatForHeader = GeneralSettingsList.Where(k => k.ValueInText.Equals("TimeFormatForHeader", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (string.IsNullOrEmpty(andonSetting.TimeFormatForHeader))
                    andonSetting.TimeFormatForHeader = "HH:mm:ss";

                andonSetting.ScrollingText = GeneralSettingsList.Where(k => k.ValueInText.Equals("ScrollingText", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();

                andonSetting.ShowCurvedBox = GeneralSettingsList.Where(k => k.ValueInText.Equals("ShowCurvedBox", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInInt).FirstOrDefault() == 1 ? true : false;
                if (andonSetting.ShowCurvedBox)
                    andonSetting.BorderClass = "addBorder";
                else
                    andonSetting.BorderClass = "removeBorder";

                andonSetting.FormFontSize = GeneralSettingsList.Where(k => k.ValueInText.Equals("FormFontSize", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();

                andonSetting.AndonTitle = GeneralSettingsList.Where(k => k.ValueInText.Equals("AndonTitle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontFamily = GeneralSettingsList.Where(k => k.ValueInText.Equals("FontFamily", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontStyle = GeneralSettingsList.Where(k => k.ValueInText.Equals("FontStyle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                data.AdnonSetting = andonSetting;
                #endregion

                #region --- Filter Values ---
                PlantID = HttpContext.Current.Request.Cookies["AndonPlant"] != null ? HttpContext.Current.Request.Cookies["AndonPlant"].Value.ToString().Trim() : "";
                CellID = HttpContext.Current.Request.Cookies["AndonCellID"] != null ? HttpContext.Current.Request.Cookies["AndonCellID"].Value.ToString().Trim() : "";
                Frequency = ""; //HttpContext.Current.Request.Cookies["AndonFrequency"] != null ? HttpContext.Current.Request.Cookies["AndonFrequency"].Value.ToString().Trim() : "";

                //if (!Frequency.Equals("Day", StringComparison.OrdinalIgnoreCase))
                //{
                //    ShiftID = AndonDBAccess.GetCurrentShift();
                //}
                #endregion

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    data.CockpitData = AndonDBAccess.getMachineCockpitData_KachMotors(PlantID, CellID, andonSetting, cockpitSettingsList);
                else
                    data.CockpitData = AndonDBAccess.getMachineCockpitData("", PlantID, ShiftID, "", CellID, Frequency, cockpitSettingsList, ComputerName, RunOption, andonSetting.SortOrder, andonSetting.OrderBy, andonSetting.ShowSmileyBlock, andonSetting.SmileySize, andonSetting.ShowCurvedBox);
                if (RunOption == "RunByMachine")
                    data.CockpitData.ForEach(k =>
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
                        data.CockpitData.ForEach(x =>
                        {
                            x.CockpitBoxWidth = BoxWidth;
                            x.TopMargin = TopMargin;
                            x.LeftMargin = LeftMargin;
                            x.TableLayout = "fixed";
                        });
                    }
                }

                Guid guid = Guid.NewGuid();
                data.AutoGeneratedID = guid.ToString();
                HttpContext.Current.Session["MainCacheData"] = data;
                Logger.WriteDebugLog($"{ComputerName} with Session {HttpContext.Current.Session.SessionID} : {data.AutoGeneratedID}");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"InsertLatestDataToMainCache_CockpitAndonLog for PC {ComputerName} With Session {HttpContext.Current.Session.SessionID} - {ex}");
            }
        }

        private void BindCockpitData()
        {
            try
            {
                setCompanyLogo();
                AllAdnonEntity AndonBindCacheData = new AllAdnonEntity();
                AllAdnonEntity andonMainCacheData = new AllAdnonEntity();
                if (Session["BindCacheData"] == null)
                {
                    if (Session["MainCacheData"] != null)
                        Session["BindCacheData"] = (AllAdnonEntity)Session["MainCacheData"];
                }
                else
                {
                    if (Session["MainCacheData"] != null)
                    {
                        andonMainCacheData = (AllAdnonEntity)Session["MainCacheData"];
                        AndonBindCacheData = (AllAdnonEntity)Session["BindCacheData"];
                        if (AndonBindCacheData.AutoGeneratedID != andonMainCacheData.AutoGeneratedID)
                        {
                            Session["BindCacheData"] = null;
                            Session["BindCacheData"] = (AllAdnonEntity)Session["MainCacheData"];
                        }
                    }
                }
                AndonBindCacheData = (AllAdnonEntity)Session["BindCacheData"];
                if (AndonBindCacheData != null)
                    settings = AndonBindCacheData.AdnonSetting;

                lblShift.Text = "Shift - " + AndonDBAccess.GetCurrentShift();
                try
                {
                    string format = settings.DateFormatForHeader + " " + settings.TimeFormatForHeader;
                    lblDateTime.InnerText = DateTime.Now.ToString(format);
                }
                catch (Exception ex)
                {
                    lblDateTime.InnerText = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }

                if (scrollingText.InnerText != settings.ScrollingText)
                    scrollingText.InnerText = settings.ScrollingText;
                headerName.Text = settings.AndonTitle;
                //if (Request.Cookies["AndonFrequency"] != null) //Frequency Changes to Database Settings Instead of Cookie Settings 26-12-2023
                //{
                //    if (ddlFrequency.Items.FindByValue(Request.Cookies["AndonFrequency"].Value.ToString()) != null)
                //        ddlFrequency.SelectedValue = Request.Cookies["AndonFrequency"].Value.ToString();
                //}
                if (Request.Cookies["AndonPlant"] != null)
                {
                    if (ddlPlantID.Items.FindByValue(Request.Cookies["AndonPlant"].Value.ToString()) != null)
                        ddlPlantID.SelectedValue = Request.Cookies["AndonPlant"].Value.ToString();
                }
                if (Request.Cookies["AndonCellID"] != null)
                {
                    if (ddlCellID.Items.FindByValue(Request.Cookies["AndonCellID"].Value.ToString()) != null)
                        ddlCellID.SelectedValue = Request.Cookies["AndonCellID"].Value.ToString();
                }


                if (Session["AndonView"] == null)
                    Session["AndonView"] = "AndonView";
                if (Session["RunOption"] == null)
                    SetSettingsDetails();
                if (Session["AndonView"].ToString().Equals("AndonView", StringComparison.OrdinalIgnoreCase))
                {
                    if (Session["RunOption"].ToString() == "RunByMachine")
                        cellLabelDiv.Visible = false;
                    else
                        cellLabelDiv.Visible = true;

                    if (Session["CustomWidthFlag"] != null)
                        hdnUseCustomWidthFlag.Value = Session["CustomWidthFlag"] as string;

                    if (Session["BindCacheData"] != null)
                    {
                        AllAdnonEntity AndonData = (AllAdnonEntity)Session["BindCacheData"];
                        List<CockpitData> list = new List<CockpitData>();
                        list = AndonData.CockpitData;

                        if (list.Count > 0)
                        {
                            //Logger.WriteDebugLog($"Latest Bind Data_BindCockpitData: Machine Count - {list.Count.ToString()}");
                            if (Request.Cookies["AndonCellID"].Value.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(Request.Cookies["AndonCellID"].Value.ToString()))
                            {
                                cellList = list.Select(x => x.GroupName).Distinct().ToList();
                                Session["CellList"] = cellList;
                                if (Session["CellCount"] == null)
                                    Session["CellCount"] = 0;
                                int.TryParse(Session["CellCount"].ToString(), out cellCount);

                                list = list.Where(x => x.GroupName == cellList[cellCount]).ToList();
                                lblCellName.Text = cellList[cellCount];
                                cellCount++;
                                Session["CellCount"] = cellCount;
                            }
                            else
                                cellLabelDiv.Visible = false;

                            lvCockpit.DataSource = list;
                            lvCockpit.DataBind();

                            if (list.Count > 0)
                            {
                                rows = flips = list.Count;
                                rowsToTake = 1;
                                Session["CockpitData"] = list;
                                Session["RowsToTake"] = rowsToTake;
                                Session["Rows"] = rows;
                                Session["Flips"] = flips;
                                rowsToTake++;
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "setFlipInterval", "setFlipInterval();", true);
                        }

                    }

                    PageRefreshtimer.Interval = (Convert.ToInt32(settings.ScreenFlipInterval));
                    PageRefreshtimer.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindCockpitData_{(Request.Cookies["ComputerName"] != null ? Request.Cookies["ComputerName"].Value : "")} :- \n STACK TRACE: {ex.StackTrace}\nINNER EXCEPTION: {ex.InnerException}\nSOURCE: {ex.Source}\nEXCEPTION: {ex}");
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["AndonPlant"] = ddlPlantID.SelectedValue;

                BindCellID();
                //ddlCellID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlantID_SelectedIndexChanged =" + ex.ToString());
            }
        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            Response.Cookies["ComputerName"].Value = txtComputerName.Text;
            Response.Cookies["ComputerName"].Expires = DateTime.MaxValue;
            Response.Redirect("ProductionAndonNew.aspx", false);
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                Logger.WriteDebugLog("****** Timer_Tick Started******");
                if (Session["Flips"] == null)
                {
                    PageRefreshtimer.Enabled = false;
                    BindCockpitData();
                    return;
                }
                int.TryParse(Session["Flips"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["RowsToTake"].ToString(), out rowsToTake);
                    int.TryParse(Session["Rows"].ToString(), out rows);

                    List<CockpitData> list = new List<CockpitData>();
                    list = Session["CockpitData"] as List<CockpitData>;

                    int skiprows = rowsToTake * rows;
                    lvCockpit.DataSource = list.Skip(skiprows).Take(rows);
                    lvCockpit.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["Flips"] = flips;
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
                        }
                    }
                    BindCockpitData();
                    Logger.WriteDebugLog("****** Timer_Tick END******");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"timer_Tick_CockpitData:  {ex}");
            }
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
        public static void insertLatestDataToMainCacheMemory()
        {
            try
            {
                InsertLatestDataToMainCache();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"insertLatestDataToMainCacheMemory {HttpContext.Current.Session.SessionID} = {ex.Message}");
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> GetCellIDsPlantWise(string plantID)
        {
            List<string> list = new List<string>();
            try
            {
                if (plantID.ToLower() == "all") plantID = "";
                list = AndonDBAccess.getCellID(plantID);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetCellIDsPlantWise= {ex.Message}");
            }
            return list;
        }

        protected void chkDesktopView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDesktopView.Checked)
                {
                    Session["AndonView"] = "DesktopView";
                    if (Session["BindCacheData"] != null)
                    {
                        AllAdnonEntity AndonData = (AllAdnonEntity)Session["BindCacheData"];
                        List<CockpitData> list = new List<CockpitData>();
                        list = AndonData.CockpitData;
                        cellLabelDiv.Visible = false;
                        lvCockpit.DataSource = list;
                        lvCockpit.DataBind();

                        ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidth", "SetIconicBoxWidth();", true);
                        PageRefreshtimer.Enabled = false;
                    }
                }
                else
                {
                    Session["AndonView"] = "AndonView";
                    Session["Rows"] = null;
                    Session["RowsToTake"] = null;
                    Session["Flips"] = null;
                    Session["CellCount"] = null;
                    Session["CellList"] = null;
                    cellLabelDiv.Visible = true;
                    InsertLatestDataToMainCache();
                    BindCockpitData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"chkDesktopView_CheckedChanged: {ex.Message}");
            }
        }

        protected void btnOKSetting_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies["AndonPlant"].Value = ddlPlantID.SelectedValue;
                Response.Cookies["AndonPlant"].Expires = DateTime.MaxValue;

                Response.Cookies["AndonCellID"].Value = ddlCellID.SelectedValue;
                Response.Cookies["AndonCellID"].Expires = DateTime.MaxValue;

                //Response.Cookies["AndonFrequency"].Value = ddlFrequency.SelectedValue;
                //Response.Cookies["AndonFrequency"].Expires = DateTime.MaxValue;

                Response.Redirect("ProductionAndonNew.aspx");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnOKSetting_Click: {ex.Message}");
            }
        }
    }
}