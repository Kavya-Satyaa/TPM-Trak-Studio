using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class ProductionAndonTableView : System.Web.UI.Page
    {
        public static AndonSettingEntity settings = new AndonSettingEntity();
        public static MachineStatusColorStyle machineStatusColors = CockpitDataBaseAccess.GetMachineStatusColorValues();
        bool isPostBack = false;
        int cellCount = 0;
        int rows = 0;
        int rowsToTake = 0;
        int flips = 0;
        List<string> cellList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AndonPage_TableView"] = this;

            if (!IsPostBack)
            {
                Logger.WriteDebugLog("****** POSTBACK EVENT ********");
                if (HttpContext.Current.Request.Cookies["ComputerName_TableView"] == null)
                {
                    ComputerDiv.Visible = true;
                    divHeader.Visible = false;
                    setCompanyLogo();
                    return;
                }
                else
                {
                    ComputerDiv.Visible = false;
                    divHeader.Visible = true;
                }

                isPostBack = false;
                Session["MainCacheData_TableView"] = null;
                Session["BindCacheData_TableView"] = null;

                Session["EfficiencyColorValues_TableView"] = null;

                Session["Flips_TableView"] = null;
                Session["RowsToTake_TableView"] = null;
                Session["CellList_TableView"] = null;
                Session["Rows_TableView"] = null;
                Session["ColumnsList_TableView"] = null;
                Session["CellCount_TableView"] = null;

                Session["AndonView_TableView"] = null;
                Session["ProductionData_TableView"] = null;

                Session["MachineIDIndex_TableView"] = null;
                Session["AEIndex_TableView"] = null;
                Session["PEIndex_TableView"] = null;
                Session["QEIndex_TableView"] = null;
                Session["OEEIndex_TableView"] = null;
                Session["OPREIndex_TableView"] = null;

                Session["MainsettingList_TableView"] = null;
                Session["andonSettingList_TableView"] = null;

                Session["AndonRunoption_TableView"] = null;

                setCompanyLogo();
                BindPlantID();
                BindCellID();
                //BindFrequency();

                SetSettingsDetails();
                insertLatestDataToMainCache();
                BindProductionData();
            }
            else if (Request.Cookies["ComputerName_TableView"] != null && (Session["MainCacheData_TableView"] == null || Session["BindCacheData_TableView"] == null))
            {
                Logger.WriteDebugLog("Manual POSTBACK");
                Response.Redirect("ProductionAndonTableView.aspx");
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
                    CompanyLogo.ImageUrl = ImagePath + fileName[0];
                else
                    CompanyLogo.ImageUrl = "Image/companyIcon.png";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"setCompanyLogo_CockpitAndon_TableView: " + ex.Message);
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
                if (Request.Cookies["AndonPlantID_TableView"] == null)
                {
                    Response.Cookies["AndonPlantID_TableView"].Value = ddlPlantID.SelectedValue;
                    Response.Cookies["AndonPlantID_TableView"].Expires = DateTime.MaxValue;
                }
                else
                {
                    if (ddlPlantID.Items.FindByValue(Request.Cookies["AndonPlantID_TableView"].Value.ToString()) != null)
                        ddlPlantID.SelectedValue = Request.Cookies["AndonPlantID_TableView"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindPlantID_CockpitAndon_TableView: {ex}");
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

                if (Request.Cookies["AndonCellID_TableView"] == null)
                {
                    Response.Cookies["AndonCellID_TableView"].Value = ddlCellID.SelectedValue;
                    Response.Cookies["AndonCellID_TableView"].Expires = DateTime.MaxValue;
                }
                else
                {
                    if (ddlCellID.Items.FindByValue(Request.Cookies["AndonCellID_TableView"].Value.ToString()) != null)
                        ddlCellID.SelectedValue = Request.Cookies["AndonCellID_TableView"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindCellID_CockpitAndon_TableView: {ex}");
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies["ComputerName_TableView"].Value = txtComputerName.Text;
                Response.Cookies["ComputerName_TableView"].Expires = DateTime.MaxValue;
                Response.Redirect("ProductionAndonTableView.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BtnSave_Click_tableView: " + ex.ToString());
            }
        }

        protected void gvProductionContainer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    {
                        if (Session["EfficiencyColorValues_TableView"] != null)
                        {
                            EffyColorEntity ColorData = new EffyColorEntity();
                            ColorData = Session["EfficiencyColorValues_TableView"] as EffyColorEntity;


                            if (Session["PEIndex_TableView"] != null)
                            {
                                string PEColor = "";
                                if (drv.Row["ProductionEfficiency"].ToString() != "")
                                {
                                    if (Convert.ToDouble(drv.Row["ProductionEfficiency"]) > 0)
                                    {
                                        if (Convert.ToDouble(drv.Row["ProductionEfficiency"]) <= Int32.Parse(drv.Row["PERed"].ToString()))
                                            PEColor = ColorData.BadColor == "" ? "#FFFFFF" : ColorData.BadColor.Remove(1, 2);

                                        else if (Convert.ToDouble(drv.Row["ProductionEfficiency"]) >= Int32.Parse(drv.Row["PEGreen"].ToString()))
                                            PEColor = ColorData.GoodColor == "" ? "#FFFFFF" : ColorData.GoodColor.Remove(1, 2);

                                        else
                                            PEColor = ColorData.ModerateColor == "" ? "#FFFFFF" : ColorData.ModerateColor.Remove(1, 2);
                                    }
                                    else
                                        PEColor = "#FFFFFF";
                                }
                                else
                                    PEColor = "White";

                                e.Row.Cells[Convert.ToInt32(Session["PEIndex_TableView"].ToString())].Style["background-color"] = PEColor;
                            }

                            if (Session["OEEIndex_TableView"] != null)
                            {
                                string OEEColor = "";
                                if (drv.Row["OVERAllEfficiency"].ToString() != "")
                                {
                                    if (Convert.ToDouble(drv.Row["OVERAllEfficiency"]) > 0)
                                    {
                                        if (Convert.ToDouble(drv.Row["OVERAllEfficiency"]) <= Int32.Parse(drv.Row["OERed"].ToString()))
                                            OEEColor = ColorData.BadColor == "" ? "#FFFFFF" : ColorData.BadColor.Remove(1, 2);

                                        else if (Convert.ToDouble(drv.Row["OVERAllEfficiency"]) >= Int32.Parse(drv.Row["OEGreen"].ToString()))
                                            OEEColor = ColorData.GoodColor == "" ? "#FFFFFF" : ColorData.GoodColor.Remove(1, 2);

                                        else
                                            OEEColor = ColorData.ModerateColor == "" ? "#FFFFFF" : ColorData.ModerateColor.Remove(1, 2);
                                    }
                                    else
                                        OEEColor = "#FFFFFF";
                                }
                                else
                                    OEEColor = "White";

                                e.Row.Cells[Convert.ToInt32(Session["OEEIndex_TableView"].ToString())].Style["background-color"] = OEEColor;
                            }

                            if (Session["OPREIndex_TableView"] != null)
                            {
                                string OPREColor = "";
                                if (drv.Row["OperatorEfficiency"].ToString() != "")
                                {
                                    if (Convert.ToDouble(drv.Row["OperatorEfficiency"]) > 0)
                                    {
                                        if (Convert.ToDouble(drv.Row["OperatorEfficiency"]) <= Int32.Parse(drv.Row["OPRRed"].ToString()))
                                            OPREColor = ColorData.BadColor == "" ? "#FFFFFF" : ColorData.BadColor.Remove(1, 2);

                                        else if (Convert.ToDouble(drv.Row["OperatorEfficiency"]) >= Int32.Parse(drv.Row["OPRGreen"].ToString()))
                                            OPREColor = ColorData.GoodColor == "" ? "#FFFFFF" : ColorData.GoodColor.Remove(1, 2);

                                        else
                                            OPREColor = ColorData.ModerateColor == "" ? "#FFFFFF" : ColorData.ModerateColor.Remove(1, 2);
                                    }
                                    else
                                        OPREColor = "#FFFFFF";
                                }
                                else
                                    OPREColor = "White";

                                e.Row.Cells[Convert.ToInt32(Session["OPREIndex_TableView"].ToString())].Style["background-color"] = OPREColor;
                            }
                        }
                        else
                        {
                            if (Session["MachineIDIndex_TableView"] != null)
                            {
                                System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();

                                string MachineStatus = string.IsNullOrEmpty(drv["MachineStatus"].ToString()) ? "" : drv["MachineStatus"].ToString();
                                if (MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                                {
                                    image.ImageUrl = "../Image/McStatus/Running.gif";
                                }
                                else
                                {
                                    image.ImageUrl = "../Image/McStatus/Stopped.gif";
                                }

                                image.CssClass = drv["MachineStatus"].ToString();
                                image.Style["display"] = "inline-block";
                                image.Style["float"] = "right";
                                image.Style["float"] = "right";
                                image.ToolTip = MachineStatus;

                                Label label = new Label();
                                label.Text = drv["MachineID"].ToString();
                                label.Style["display"] = "inline-block";
                                label.Style["padding"] = "0px 10px 0px 0px";

                                e.Row.Cells[Convert.ToInt32(Session["MachineIDIndex_TableView"].ToString())].Controls.Add(label);
                                e.Row.Cells[Convert.ToInt32(Session["MachineIDIndex_TableView"].ToString())].Controls.Add(image);

                            }

                            if (Session["EfficiencyColorValues_TableView"] != null)
                            {
                                EffyColorEntity ColorData = new EffyColorEntity();
                                ColorData = Session["EfficiencyColorValues_TableView"] as EffyColorEntity;


                                if (Session["AEIndex_TableView"] != null)
                                {
                                    string AEColor = "";
                                    if (drv.Row["AeColor"].ToString() != "")
                                    {
                                        if (drv.Row["AeColor"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase)) AEColor = ColorData.BadColor.Remove(1, 2);
                                        if (drv.Row["AeColor"].ToString().Equals("Yellow", StringComparison.OrdinalIgnoreCase)) AEColor = ColorData.ModerateColor.Remove(1, 2);
                                        if (drv.Row["AeColor"].ToString().Equals("Green", StringComparison.OrdinalIgnoreCase)) AEColor = ColorData.GoodColor.Remove(1, 2);
                                    }
                                    else
                                        AEColor = "White";

                                    e.Row.Cells[Convert.ToInt32(Session["AEIndex_TableView"].ToString())].Style["background-color"] = AEColor;
                                }
                                if (Session["PEIndex_TableView"] != null)
                                {
                                    string PEColor = "";
                                    if (drv.Row["PeColor"].ToString() != "")
                                    {
                                        if (drv.Row["PeColor"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase)) PEColor = ColorData.BadColor.Remove(1, 2);
                                        if (drv.Row["PeColor"].ToString().Equals("Yellow", StringComparison.OrdinalIgnoreCase)) PEColor = ColorData.ModerateColor.Remove(1, 2);
                                        if (drv.Row["PeColor"].ToString().Equals("Green", StringComparison.OrdinalIgnoreCase)) PEColor = ColorData.GoodColor.Remove(1, 2);
                                    }
                                    else
                                        PEColor = "White";

                                    e.Row.Cells[Convert.ToInt32(Session["PEIndex_TableView"].ToString())].Style["background-color"] = PEColor;
                                }
                                if (Session["QEIndex_TableView"] != null)
                                {
                                    string QEColor = "";
                                    if (drv.Row["QeColor"].ToString() != "")
                                    {
                                        if (drv.Row["QeColor"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase)) QEColor = ColorData.BadColor.Remove(1, 2);
                                        if (drv.Row["QeColor"].ToString().Equals("Yellow", StringComparison.OrdinalIgnoreCase)) QEColor = ColorData.ModerateColor.Remove(1, 2);
                                        if (drv.Row["QeColor"].ToString().Equals("Green", StringComparison.OrdinalIgnoreCase)) QEColor = ColorData.GoodColor.Remove(1, 2);
                                    }
                                    else
                                        QEColor = "White";

                                    e.Row.Cells[Convert.ToInt32(Session["QEIndex_TableView"].ToString())].Style["background-color"] = QEColor;
                                }
                                if (Session["OEEIndex_TableView"] != null)
                                {
                                    string OEEColor = "";
                                    if (drv.Row["OeColor"].ToString() != "")
                                    {
                                        if (drv.Row["OeColor"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase)) OEEColor = ColorData.BadColor.Remove(1, 2);
                                        if (drv.Row["OeColor"].ToString().Equals("Yellow", StringComparison.OrdinalIgnoreCase)) OEEColor = ColorData.ModerateColor.Remove(1, 2);
                                        if (drv.Row["OeColor"].ToString().Equals("Green", StringComparison.OrdinalIgnoreCase)) OEEColor = ColorData.GoodColor.Remove(1, 2);
                                    }
                                    else
                                        OEEColor = "White";

                                    e.Row.Cells[Convert.ToInt32(Session["OEEIndex_TableView"].ToString())].Style["background-color"] = OEEColor;
                                }
                            }

                            //Check for Double value columns and round them off to 2 decmals.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("gvProductionContainer_RowDataBound_tableView: " + ex.ToString());
            }
        }


        protected void btnOKSetting_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Cookies["AndonPlantID_TableView"].Value = ddlPlantID.SelectedValue;
                Response.Cookies["AndonPlantID_TableView"].Expires = DateTime.MaxValue;

                Response.Cookies["AndonCellID_TableView"].Value = ddlCellID.SelectedValue;
                Response.Cookies["AndonCellID_TableView"].Expires = DateTime.MaxValue;

                //insertLatestDataToMainCache();
                //BindProductionData();
                Response.Redirect("ProductionAndonTableView.aspx");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnOKSetting_Click_tableView: " + ex.ToString());
            }
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["Flips_TableView"] == null)
                {
                    timer.Enabled = false;
                    BindProductionData();
                    return;
                }
                int.TryParse(Session["Flips_TableView"].ToString(), out flips);
                if (flips > 1)
                {
                    Logger.WriteErrorLog("Timer_tick: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    int.TryParse(Session["RowsToTake_TableView"].ToString(), out rowsToTake);
                    int.TryParse(Session["Rows_TableView"].ToString(), out rows);

                    Logger.WriteErrorLog(Session["ProductionData_TableView"] == null ? "null" : "Not NUll");
                    DataTable list = (DataTable)Session["ProductionData_TableView"];
                    int skipRows = rows * rowsToTake;
                    Logger.WriteErrorLog($"Timer_Tick: Machine Count, Flips, RowsTotake, Rows: {list.Rows.Count}, {flips}, {rowsToTake}, {rows}");
                    //gvProductionContainer.DataSource = list.Skip(skipRows).Take(rows);
                    DataTable dt = list.AsEnumerable().Skip(skipRows).Take(rows).CopyToDataTable();
                    gvProductionContainer.DataSource = dt;
                    gvProductionContainer.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["Flips_TableView"] = flips;
                    Session["RowsToTake_TableView"] = rowsToTake;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setThStyle", "setThStyle();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "setdtHeight", "setdtHeight();", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "setTableHeight", "setTableHeight();", true);
                }
                else
                {
                    if (Session["CellCount_TableView"] == null)
                    {
                        BindProductionData();
                    }
                    else
                    {
                        int.TryParse(Session["CellCount_TableView"].ToString(), out cellCount);
                        cellList = Session["CellList_TableView"] as List<string>;
                        if (cellCount >= cellList.Count)
                        {
                            HttpContext.Current.Session["cellCount_TableView"] = 0;
                        }
                        BindProductionData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("timer_Tick_tableView: " + ex.ToString());
            }
        }


        private static void SetSettingsDetails()
        {
            string ComputerName = "", Parameter = "CockpitAndonTableViewParameter", RunOption = "RunByCell";
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName_TableView"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName_TableView"].Value.ToString();

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    Parameter = "AndonParameter_TableView_KM";

                if (HttpContext.Current.Session["andonSettingList_TableView"] == null)
                    HttpContext.Current.Session["andonSettingList_TableView"] = AndonDBAccess.getAndonSettingDetails(ComputerName, "CockpitAndonTableViewUISettings"); // TV Level Display and Refresh Settings

                if (HttpContext.Current.Session["MainsettingList_TableView"] == null)
                    HttpContext.Current.Session["MainsettingList_TableView"] = AndonDBAccess.getCockpitSettingDetails(ComputerName, Parameter); //Fields to be displayed.

                if (HttpContext.Current.Session["AndonRunoption_TableView"] == null)
                    HttpContext.Current.Session["AndonRunOption_TableView"] = AndonDBAccess.GetRunOption(ComputerName, "ComputerRunOption_TableView");

                if (HttpContext.Current.Session["EfficiencyColorValues_TableView"] == null)
                    HttpContext.Current.Session["EfficiencyColorValues_TableView"] = AndonDBAccess.getEfficiencyColor();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"{ComputerName}: SetSettingsDetails= {ex}");
            }
        }

        private static void insertLatestDataToMainCache()
        {
            string ComputerName = "", RunOption = "", PlantID = "", CellID = "", Frequency = "", ShiftID = "";
            List<AndonSettingsEntity> cockpitSettingsList = new List<AndonSettingsEntity>(); //Fields to be displayed.
            List<AndonDefaultsEntity> GeneralSettingsList = new List<AndonDefaultsEntity>(); //TV Level Display and Refresh Settings
            try
            {
                if (HttpContext.Current.Request.Cookies["ComputerName_TableView"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName_TableView"].Value.ToString();

                AndonProductionEntity data = new AndonProductionEntity();



                if (HttpContext.Current.Session["andonSettingList_TableView"] != null && HttpContext.Current.Session["MainsettingList_TableView"] != null && HttpContext.Current.Session["AndonRunOption_TableView"] != null)
                {
                    GeneralSettingsList = HttpContext.Current.Session["andonSettingList_TableView"] as List<AndonDefaultsEntity>;
                    cockpitSettingsList = HttpContext.Current.Session["MainsettingList_TableView"] as List<AndonSettingsEntity>;
                    RunOption = HttpContext.Current.Session["AndonRunOption_TableView"].ToString();
                }
                else
                    HttpContext.Current.Response.Redirect("ProductionAndonTableView.aspx", true);

                #region --- Settings Data --- 
                AndonSettingEntity andonSetting = new AndonSettingEntity();
                string sortby = GeneralSettingsList.Where(k => k.ValueInText.Equals("OrderBy", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                var sortOption = sortby.Split(' ');
                if (sortOption.Length >= 2)
                {
                    andonSetting.OrderBy = sortOption[0].ToString().Trim();
                    andonSetting.SortOrder = (sortOption[1].ToString().Trim());
                }

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


                andonSetting.AndonTitle = GeneralSettingsList.Where(k => k.ValueInText.Equals("AndonTitle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontFamily = GeneralSettingsList.Where(k => k.ValueInText.Equals("FontFamily", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontStyle = GeneralSettingsList.Where(k => k.ValueInText.Equals("FontStyle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                data.AndonSettings = andonSetting;
                #endregion

                PlantID = HttpContext.Current.Request.Cookies["AndonPlantID_TableView"] != null ? HttpContext.Current.Request.Cookies["AndonPlantID_TableView"].Value.ToString().Trim() : "";
                CellID = HttpContext.Current.Request.Cookies["AndonCellID_TableView"] != null ? HttpContext.Current.Request.Cookies["AndonCellID_TableView"].Value.ToString().Trim() : "";

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                {
                    data.ProductionData = AndonDBAccess.GetProductionAndonData_TableView_KachMotors(PlantID, CellID, andonSetting.OrderBy, andonSetting.SortOrder);
                }

                else
                    data.ProductionData = AndonDBAccess.GetProductionAndonData_TableView(ComputerName, PlantID, CellID, andonSetting.OrderBy, andonSetting.SortOrder);
                //if (RunOption == "RunByMachine")
                //    data.ProductionData.AsEnumerable().ToList<DataRow>().ForEach(k =>
                //    {
                //        k["GroupID"] = "";
                //    });

                Logger.WriteDebugLog("Total Rows: " + data.ProductionData.Rows.Count.ToString());
                data.HeaderData = cockpitSettingsList;

                Guid guid = Guid.NewGuid();
                data.AutoGenerateID = guid.ToString();

                HttpContext.Current.Session["MainCacheData_TableView"] = data;
                Logger.WriteDebugLog($"{ComputerName} with Session {HttpContext.Current.Session.SessionID} : {data.AutoGenerateID}");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertLatestDataToMainCache:" + ex.ToString());
            }
        }

        private void BindProductionData()
        {
            try
            {
                timer.Enabled = false;
                setCompanyLogo();
                AndonProductionEntity AndonBindCacheData = new AndonProductionEntity();
                AndonProductionEntity AndonMainCacheData = new AndonProductionEntity();

                if (Session["BindCacheData_TableView"] == null)
                {
                    if (Session["MainCacheData_TableView"] != null)
                        Session["BindCacheData_TableView"] = Session["MainCacheData_TableView"] as AndonProductionEntity;
                }
                else
                {
                    if (Session["MainCacheData_TableView"] != null)
                    {
                        AndonMainCacheData = Session["MainCacheData_TableView"] as AndonProductionEntity;
                        AndonBindCacheData = Session["BindCacheData_TableView"] as AndonProductionEntity;
                        if (AndonBindCacheData.AutoGenerateID != AndonMainCacheData.AutoGenerateID)
                        {
                            Session["BindCacheData_TableView"] = null;
                            Session["BindCacheData_TableView"] = Session["MainCacheData_TableView"] as AndonProductionEntity;
                        }
                    }
                }

                AndonBindCacheData = Session["BindCacheData_TableView"] as AndonProductionEntity;
                if (AndonBindCacheData != null)
                    settings = AndonBindCacheData.AndonSettings;
                string currentShift = AndonDBAccess.GetCurrentShift();
                lblShift.Text = "Shift - " + currentShift;
                try
                {
                    string format = settings.DateFormatForHeader + " " + settings.TimeFormatForHeader;
                    lblDateTime.InnerText = DateTime.Now.ToString(format);
                }
                catch (Exception ex)
                {
                    lblDateTime.InnerText = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }
                headername.Text = settings.AndonTitle;

                scrollingText.InnerText = settings.ScrollingText;

                //List<AndonEntity> list = new List<AndonEntity>();

                DataTable list = new DataTable();
                list = AndonBindCacheData.ProductionData;
                Session["ColumnsList_TableView"] = AndonBindCacheData.HeaderData;

                if (Session["AndonView_TableView"] == null)
                    Session["AndonView_TableView"] = "AndonView";
                if (Session["AndonView_TableView"].ToString().Equals("AndonView", StringComparison.OrdinalIgnoreCase))
                {
                    cellCount = 0;
                    if (list.Rows.Count > 0) //list.Count > 0
                    {
                        Logger.WriteErrorLog(list.Rows.Count.ToString());
                        if (Session["AndonRunoption_TableView"] == null)
                            SetSettingsDetails();
                        if (Session["AndonRunoption_TableView"].ToString() == "RunByMachine")
                        {
                            DivCellLabel.Visible = false;
                            cellList = new List<string>() { "" };
                            HttpContext.Current.Session["CellList_TableView"] = cellList;
                            if (HttpContext.Current.Session["CellCount_TableView"] == null || HttpContext.Current.Session["CellCount_TableView"].Equals(cellList.Count))
                            {
                                HttpContext.Current.Session["CellCount_TableView"] = 0;
                            }
                            int.TryParse(HttpContext.Current.Session["CellCount_TableView"].ToString(), out cellCount);
                            cellCount++;
                            HttpContext.Current.Session["CellCount_TableView"] = cellCount;


                            BindDataTable("");
                        }
                        else
                        {
                            DivCellLabel.Visible = true;
                            if (ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) || ddlCellID.SelectedValue.ToString().Equals(""))
                            {
                                cellList = list.AsEnumerable().Select(x => x["GroupID"].ToString()).Distinct().ToList();
                                HttpContext.Current.Session["CellList_TableView"] = cellList;
                                if (HttpContext.Current.Session["CellCount_TableView"] == null || HttpContext.Current.Session["CellCount_TableView"].Equals(cellList.Count))
                                {
                                    HttpContext.Current.Session["CellCount_TableView"] = 0;
                                }
                                int.TryParse(HttpContext.Current.Session["CellCount_TableView"].ToString(), out cellCount);
                                list = list.AsEnumerable().Where(k => k["GroupID"].ToString() == cellList[cellCount]).CopyToDataTable();

                                BindDataTable(cellList[cellCount]);

                                lblCellName.Text = cellList[cellCount];
                                cellCount++;
                                HttpContext.Current.Session["CellCount_TableView"] = cellCount;
                            }
                            else
                            {
                                BindDataTable(ddlCellID.SelectedValue);
                                lblCellName.Text = ddlCellID.SelectedValue.Trim();
                            }
                        }


                        gvProductionContainer.DataSource = list;
                        gvProductionContainer.DataBind();
                        //rows = flips = list.Count;
                        rows = flips = list.Rows.Count;
                        rowsToTake = 1;
                        Session["Flips_TableView"] = flips;
                        Session["ProductionData_TableView"] = list;
                        Session["RowsToTake_TableView"] = rowsToTake;
                        Session["Rows_TableView"] = rows;
                        rowsToTake++;
                    }

                    timer.Interval = Convert.ToInt32(settings.ScreenFlipInterval);
                    timer.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setFlipInterval", "setFlipInterval();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindProductionData_TableView=  " + ex.ToString());
            }
        }


        private void BindDataTable(string CellID)
        {
            List<AndonSettingsEntity> HeaderList = new List<AndonSettingsEntity>();
            try
            {
                gvProductionContainer.Columns.Clear();
                if (Session["ColumnsList_TableView"] != null)
                {
                    HeaderList = (List<AndonSettingsEntity>)Session["ColumnsList_TableView"];
                }

                if (HeaderList.Count > 0)
                {
                    if (Session["AndonRunoption_TableView"] != null)
                    {
                        string RunOption = Session["AndonRunoption_TableView"].ToString();
                        if (RunOption != "RunByMachine" && !string.IsNullOrEmpty(CellID))
                            HeaderList = HeaderList.Where(x => (x.CellID.Equals(CellID, StringComparison.OrdinalIgnoreCase) || x.CellID.Equals("Master", StringComparison.OrdinalIgnoreCase))).OrderBy(x => string.IsNullOrEmpty(x.SortOrder) ? 0 : Convert.ToInt32(x.SortOrder)).ToList();
                        else
                            HeaderList = HeaderList.GroupBy(x => x.ColumnName).Select(x => x.FirstOrDefault()).OrderBy(x => string.IsNullOrEmpty(x.SortOrder) ? 0 : Convert.ToInt32(x.SortOrder)).ToList();  //Groups By Column Name and Fetches the 1st row from each grouped data; used to fetch distinct column names when runoption is runbymachine but multiple cell data is available. 
                    }
                    else
                        SetSettingsDetails();

                    if (HeaderList != null && HeaderList.Count > 0)
                    {
                        foreach (var li in HeaderList)
                        {
                            BoundField bf = new BoundField();
                            bf.DataField = li.ColumnName;
                            bf.HeaderText = li.DisplayText;
                            bf.ItemStyle.Font.Size = Convert.ToInt32(li.DataFontSize);
                            bf.HeaderStyle.Font.Size = Convert.ToInt32(li.LabelFontSize);

                            if (li.ColumnName.Equals("MachineID", StringComparison.OrdinalIgnoreCase))
                                Session["MachineIDIndex_TableView"] = HeaderList.IndexOf(li);

                            if (li.ColumnName.Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase))
                                Session["AEIndex_TableView"] = HeaderList.IndexOf(li);
                            if (li.ColumnName.Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase))
                                Session["PEIndex_TableView"] = HeaderList.IndexOf(li);
                            if (li.ColumnName.Equals("QualityEfficiency", StringComparison.OrdinalIgnoreCase))
                                Session["QEIndex_TableView"] = HeaderList.IndexOf(li);
                            if (li.ColumnName.Equals("OVERAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                Session["OEEIndex_TableView"] = HeaderList.IndexOf(li);
                            if (li.ColumnName.Equals("OperatorEfficiency", StringComparison.OrdinalIgnoreCase))
                                Session["OPREIndex_TableView"] = HeaderList.IndexOf(li);

                            gvProductionContainer.Columns.Add(bf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindDataTable_tableView: " + ex.ToString());
            }
        }

        protected void chkDesktopView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDesktopView.Checked)
                {
                    Session["AndonView_TableView"] = "DesktopView";
                    if (Session["BindCacheData_TableView"] != null)
                    {
                        AndonProductionEntity AndonData = (AndonProductionEntity)Session["BindCacheData_TableView"];
                        if (AndonData.ProductionData.Rows.Count > 0)
                        {
                            DivCellLabel.Visible = false;
                            BindDataTable("");
                            gvProductionContainer.DataSource = AndonData.ProductionData;
                            gvProductionContainer.DataBind();

                            ScriptManager.RegisterStartupScript(this, GetType(), "setThStyle", "setThStyle();", true);
                            ScriptManager.RegisterStartupScript(this, GetType(), "setdtHeight", "setdtHeight();", true);
                        }

                        timer.Enabled = false;
                    }
                }
                else
                {
                    Session["AndonView_TableView"] = "AndonView";
                    Session["Flips_TableView"] = null;
                    Session["RowsToTake_TableView"] = null;
                    Session["CellList_TableView"] = null;
                    Session["Rows_TableView"] = null;
                    Session["ColumnsList_TableView"] = null;
                    Session["CellCount_TableView"] = null;
                    DivCellLabel.Visible = true;
                    insertLatestDataToMainCache();
                    BindProductionData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"chkDesktopView_CheckedChanged: {ex.Message}");
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void setFlipIntervalToSession(int displayedItemCount)
        {
            try
            {
                HttpContext.Current.Session["Rows_TableView"] = displayedItemCount;
                if (displayedItemCount != 0)
                {
                    int flips = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(HttpContext.Current.Session["Flips_TableView"]) / displayedItemCount));
                    HttpContext.Current.Session["Flips_TableView"] = flips;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setFlipIntervalToSession(PM):" + ex.Message);
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
                Logger.WriteErrorLog("insertLatestDataToMainCacheMemory:" + ex.Message);
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


    }
}