using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public partial class AndonPagePradeepMetals : System.Web.UI.Page
    {
        public static AndonGeneralSettingsEntity settings = new AndonGeneralSettingsEntity();
        bool isPostBack = false;
        int cellCount = 0;
        int rows = 0;
        int rowsToTake = 0;
        int flips = 0;
        List<string> cellList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AndonPage"] = this;

            if (!IsPostBack)
            {
                if (HttpContext.Current.Request.Cookies["ComputerName"] == null)
                {
                    ComputerDiv.Visible = true;
                    return;
                }
                else
                {
                    ComputerDiv.Visible = false;
                }

                isPostBack = false;
                Session["AndonPlantID"] = null;
                Session["AndonCellID"] = null;
                Session["AndonMachineID"] = null;
                Session["Flips"] = null;
                Session["ProductionData"] = null;
                Session["RowsToTake"] = null;
                Session["Rows"] = null;
                Session["ColumnsList"] = null;

                setCompanyLogo();
                BindPlantID();
                BindCellID();
                BindMachineID();
                //BindFrequency();

                Cache.Remove($"BindProductionCacheData{HttpContext.Current.Session.SessionID}");
                Cache.Remove($"MainProductionCacheData{HttpContext.Current.Session.SessionID}");

                insertLatestDataToMainCache();
                BindProductionData();
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
                    CompanyLogo.ImageUrl = ImagePath + fileName[0];
                }
                else
                {
                    CompanyLogo.ImageUrl = "Image/companyIcon.png";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setCompanyLogo : " + ex.Message);
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> Plants = DBAccessPradeepMetals.GetPlantID();

                if (Plants.Count > 1)
                {
                    Plants.Insert(0, "All");
                }
                ddlPlantID.DataSource = Plants;
                ddlPlantID.DataBind();

                Session["AndonPlantID"] = ddlPlantID.SelectedValue;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlantID: " + ex.Message);
            }
        }

        private void BindCellID()
        {
            try
            {
                string ComputerName = string.Empty;
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                {
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
                }
                List<string> CellID = DBAccessPradeepMetals.GetCellIDComputerWise(ComputerName, ddlPlantID.SelectedValue == "All" ? "" : ddlPlantID.SelectedValue);
                if (CellID.Count > 1)
                {
                    CellID.Insert(0, "All");
                }
                ddlCellID.DataSource = CellID;
                ddlCellID.DataBind();

                Session["AndonCellID"] = ddlCellID.SelectedValue;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCellID: " + ex.Message);
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> list = DBAccessPradeepMetals.GetMachineID(ddlPlantID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue, ddlCellID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue);

                if (list.Count > 1)
                {
                    list.Insert(0, "All");
                }
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();

                Session["AndonMachineID"] = ddlMachineID.SelectedValue;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineID: " + ex.Message);
            }
        }

        private static void insertLatestDataToMainCache()
        {
            try
            {
                string ComputerName = string.Empty;
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();

                AllAndonProductionEntity data = new AllAndonProductionEntity();
                List<AndonSettingsEntity> AndonSettingsList = DBAccessPradeepMetals.GetAndonSettings();
                AndonGeneralSettingsEntity MainSettingsList = DBAccessPradeepMetals.ViewAndonGeneralSettings("View");


                #region Settings Data 
                MainSettingsList.ScreenFlipInterval = string.IsNullOrEmpty(MainSettingsList.ScreenFlipInterval) ? "10" : (Convert.ToInt32(MainSettingsList.ScreenFlipInterval) * 1000).ToString();
                MainSettingsList.DataRefreshInterval = string.IsNullOrEmpty(MainSettingsList.DataRefreshInterval) ? "30" : (Convert.ToInt32(MainSettingsList.DataRefreshInterval) * 1000).ToString();
                MainSettingsList.DateFormat = string.IsNullOrEmpty(MainSettingsList.DateFormat) ? "dd-MM-yyyy" : MainSettingsList.DateFormat;
                MainSettingsList.TimeFormat = string.IsNullOrEmpty(MainSettingsList.TimeFormat) ? "HH:mm:ss" : MainSettingsList.TimeFormat;
                #endregion
                data.AndonSettings = MainSettingsList;

                string ShiftName = "", PlantID= "", CellID = "", MachineID = "";

                
                ShiftName = DBAccessPradeepMetals.GetShiftStart(); //out ShiftStart, out ShiftEnd
                //ShiftStart = Convert.ToDateTime(ShiftStart).ToString("yyyy-MM-dd HH:mm:ss");
                //ShiftEnd = Convert.ToDateTime(ShiftEnd).ToString("yyyy-MM-dd HH:mm:ss");

                if (HttpContext.Current.Session["AndonPlantID"] != null)
                {
                    PlantID = HttpContext.Current.Session["AndonPlantID"].ToString();
                    PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(PlantID) ? "" : "'" + PlantID + "'";

                    CellID = HttpContext.Current.Session["AndonCellID"].ToString();
                    CellID = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(CellID) ? "" : "'" + CellID + "'";

                    MachineID = HttpContext.Current.Session["AndonMachineID"].ToString();
                    MachineID = MachineID.Equals("All", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(MachineID) ? "" : "'" + MachineID + "'";
                }

                data.ProductionData = DBAccessPradeepMetals.GetProductionAndonData(ComputerName, ShiftName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), MachineID, CellID, PlantID);
                //data.ProductionData = DBAccessPradeepMetals.GetProductionAndonData(ComputerName, "FIRST", "2023-04-26 00:00:00.000", MachineID, CellID, PlantID);
                data.HeaderData = AndonSettingsList;

                Guid guid = new Guid();
                data.AutoGenerateID = guid.ToString();
                HttpContext.Current.Cache.Remove($"MainProductionCacheData{HttpContext.Current.Session.SessionID}");
                HttpContext.Current.Cache.Insert($"MainProductionCacheData{HttpContext.Current.Session.SessionID}", data, null, DateTime.Now.AddMinutes(4), TimeSpan.Zero);
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
                setCompanyLogo();
                AllAndonProductionEntity AndonBindCacheData = new AllAndonProductionEntity();
                AllAndonProductionEntity AndonMainCacheData = new AllAndonProductionEntity();

                if (Cache[$"BindProductionCacheData{HttpContext.Current.Session.SessionID}"] == null)
                {
                    if (Cache[$"MainProductionCacheData{HttpContext.Current.Session.SessionID}"] != null)
                    {
                        Cache.Insert($"BindProductionCacheData{HttpContext.Current.Session.SessionID}", (AllAndonProductionEntity)Cache[$"MainProductionCacheData{HttpContext.Current.Session.SessionID}"], null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }
                }
                else
                {
                    if (Cache[$"MainProductionCacheData{HttpContext.Current.Session.SessionID}"] != null)
                    {
                        AndonBindCacheData = (AllAndonProductionEntity)Cache[$"MainProductionCacheData{HttpContext.Current.Session.SessionID}"];
                        AndonMainCacheData = (AllAndonProductionEntity)Cache[$"BindProductionCacheData{HttpContext.Current.Session.SessionID}"];
                        if (AndonBindCacheData.AutoGenerateID != AndonMainCacheData.AutoGenerateID)
                        {
                            Cache.Remove($"BindProductionCacheData{HttpContext.Current.Session.SessionID}");
                            Cache.Insert($"BindProductionCacheData{HttpContext.Current.Session.SessionID}", (AllAndonProductionEntity)Cache[$"MainProductionCacheData{HttpContext.Current.Session.SessionID}"], null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                        }
                    }
                }

                AndonBindCacheData = (AllAndonProductionEntity)Cache[$"BindProductionCacheData{HttpContext.Current.Session.SessionID}"];
                if (AndonBindCacheData != null)
                    settings = AndonBindCacheData.AndonSettings;
                string currentShift = DBAccessPradeepMetals.GetShiftStart();
                lblShift.Text = "Shift - " + currentShift;
                try
                {
                    string format = settings.DateFormat + " " + settings.TimeFormat;
                    lblDateTime.InnerText = DateTime.Now.ToString(format);
                }
                catch (Exception ex)
                {
                    lblDateTime.InnerText = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }
                headername.Text = settings.AndonTitle;
                if (Session["AndonPlantID"] != null)
                {
                    if (ddlPlantID.Items.FindByValue(Session["AndonPlantID"].ToString()) != null)
                        ddlPlantID.SelectedValue = Session["AndonPlantID"].ToString();
                }
                if (Session["AndonCellID"] != null)
                {
                    if (ddlCellID.Items.FindByValue(Session["AndonCellID"].ToString()) != null)
                        ddlCellID.SelectedValue = Session["AndonCellID"].ToString();
                }
                scrollingText.InnerText = settings.ScrollingText;

                //List<AndonEntity> list = new List<AndonEntity>();
                DataTable list = new DataTable();
                list = AndonBindCacheData.ProductionData;
                Session["ColumnsList"] = AndonBindCacheData.HeaderData;
                
                cellCount = 0;
                if (list.Rows.Count > 0) //list.Count > 0
                {
                    BindDataTable();

                    if (ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) || ddlCellID.SelectedValue.ToString().Equals(""))
                    {
                        //cellList = list.Select(x => x.GroupID).Distinct().ToList();
                        cellList = list.AsEnumerable().Select(x => x["GroupID"].ToString()).Distinct().ToList();
                        HttpContext.Current.Session["CellList"] = cellList;
                        if (HttpContext.Current.Session["CellCount"] == null || HttpContext.Current.Session["CellCount"].Equals(cellList.Count))
                        {
                            HttpContext.Current.Session["CellCount"] = 0;
                        }
                        int.TryParse(HttpContext.Current.Session["CellCount"].ToString(), out cellCount);
                        //list = list.Where(k => k.GroupID == cellList[cellCount]).ToList();
                        list = list.AsEnumerable().Where(k => k["GroupID"].ToString() == cellList[cellCount]).CopyToDataTable();

                        lblCellName.Text = cellList[cellCount];
                        lblSuperVisor.Text = "Supervisor Name: " + DBAccessPradeepMetals.GetSuperVisorName(currentShift, lblCellName.Text.Trim());
                        cellCount++;
                        HttpContext.Current.Session["CellCount"] = cellCount;
                    }
                    else
                    {
                        lblCellName.Text = ddlCellID.SelectedValue.Trim();
                        lblSuperVisor.Text = "Supervisor Name: " + DBAccessPradeepMetals.GetSuperVisorName(currentShift, ddlCellID.SelectedValue.Trim());
                    }

                    gvProductionContainer.DataSource = list;
                    gvProductionContainer.DataBind();

                    if (list.Rows.Count > 0) //list.Count > 0
                    {
                        //rows = flips = list.Count;
                        rows = flips = list.Rows.Count;
                        rowsToTake = 1;
                        Session["Flips"] = flips;
                        Session["ProductionData"] = list;
                        Session["RowsToTake"] = rowsToTake;
                        Session["Rows"] = rows;
                        rowsToTake++;
                    }

                    timer.Interval = Convert.ToInt32(settings.ScreenFlipInterval);
                    timer.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setFlipInterval", "setFlipInterval();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["Flips"] == null)
                {
                    timer.Enabled = false;
                    BindProductionData();
                    return;
                }
                int.TryParse(Session["Flips"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["RowsToTake"].ToString(), out rowsToTake);
                    int.TryParse(Session["Rows"].ToString(), out rows);
                    //List<AndonEntity> list = (List<AndonEntity>)Session["ProductionData"];
                    DataTable list = (DataTable)Session["ProductionData"];
                    int skipRows = rows * rowsToTake;
                    BindDataTable();
                    //gvProductionContainer.DataSource = list.Skip(skipRows).Take(rows);
                    DataTable dt = list.AsEnumerable().Skip(skipRows).Take(rows).CopyToDataTable();
                    gvProductionContainer.DataSource = dt;
                    gvProductionContainer.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["Flips"] = flips;
                    Session["RowsToTake"] = rowsToTake;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setThStyle", "setThStyle();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "setdtHeight", "setdtHeight();", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "setTableHeight", "setTableHeight();", true);
                }
                else
                {
                    if (Session["CellCount"] == null)
                    {
                        BindProductionData();
                    }
                    else
                    {
                        int.TryParse(Session["CellCount"].ToString(), out cellCount);
                        cellList = Session["CellList"] as List<string>;
                        if (cellCount >= cellList.Count)
                        {
                            HttpContext.Current.Session["cellCount"] = 0;
                        }
                        BindProductionData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("timer_Tick:" + ex.Message);
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["AndonPlantID"] = ddlPlantID.SelectedValue;
                BindCellID();
                ddlCellID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlantID_SelectedIndexChanged:" + ex.Message);
            }
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["AndonCellID"] = ddlCellID.SelectedValue;
                ddlMachineID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlCellID_SelectedIndexChanged:" + ex.Message);
            }
        }

        private void BindDataTable()
        {
            List<AndonSettingsEntity> HeaderList = new List<AndonSettingsEntity>();
            try
            {
                gvProductionContainer.Columns.Clear();
                if (Session["ColumnsList"] != null)
                {
                    HeaderList = (List<AndonSettingsEntity>)Session["ColumnsList"];
                }

                foreach (var li in HeaderList)
                {
                    if (li.Visibility == 1)
                    {
                        BoundField bf = new BoundField();
                        bf.DataField = li.ColumnName;
                        bf.HeaderText = li.DisplayText;
                        bf.ItemStyle.Font.Size = Convert.ToInt32(li.DataFontSize);
                        bf.HeaderStyle.Font.Size = Convert.ToInt32(li.LabelFontSize);

                        if (li.ColumnName.Equals("AE", StringComparison.OrdinalIgnoreCase))
                            Session["AEIndex"] = HeaderList.IndexOf(li);
                        if (li.ColumnName.Equals("PE", StringComparison.OrdinalIgnoreCase))
                            Session["PEIndex"] = HeaderList.IndexOf(li);
                        if (li.ColumnName.Equals("QE", StringComparison.OrdinalIgnoreCase))
                            Session["QEIndex"] = HeaderList.IndexOf(li);
                        if (li.ColumnName.Equals("OEE", StringComparison.OrdinalIgnoreCase))
                            Session["OEEIndex"] = HeaderList.IndexOf(li);

                        gvProductionContainer.Columns.Add(bf);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindDataTable: " + ex.ToString());
            }
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["MachineID"] = ddlMachineID.SelectedValue;
                insertLatestDataToMainCache();
                BindProductionData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
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

        protected void gvProductionContainer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    var lastIndex = e.Row.Cells.Count - 1;
                    if (gvProductionContainer.Columns[lastIndex].HeaderText.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
                        image.ID = "Status";
                        image.CssClass = "StatusImag";
                        if (e.Row.Cells[lastIndex].Text.Equals("stopped", StringComparison.OrdinalIgnoreCase) || e.Row.Cells[lastIndex].Text == "&nbsp;")
                        {
                            image.ImageUrl = @"~\GEA\Andon_GEA\Images\Status\Red_btn.jpg";
                        }
                        else if (e.Row.Cells[lastIndex].Text.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            image.ImageUrl = @"~\GEA\Andon_GEA\Images\Status\Green_btn.jpg";
                        }
                        else if (e.Row.Cells[lastIndex].Text.Equals("Setup", StringComparison.OrdinalIgnoreCase))
                        {
                            image.ImageUrl = @"~\GEA\Andon_GEA\Images\Status\Yellow_btn.jpg";
                        }
                        e.Row.Cells[lastIndex].Controls.Add(image);
                    }

                    if (Session["AEIndex"] != null)
                        e.Row.Cells[Convert.ToInt32(Session["AEIndex"].ToString())].Style["background-color"] = drv.Row["AEColor"].ToString() != "" ? "#" + GetLighterColor(drv.Row["AEColor"].ToString()).ToString()  : "white"; 
                    if (Session["PEIndex"] != null)
                        e.Row.Cells[Convert.ToInt32(Session["PEIndex"].ToString())].Style["background-color"] = drv.Row["PEColor"].ToString() != "" ? "#" + GetLighterColor(drv.Row["PEColor"].ToString()).ToString() : "white"; //
                    if (Session["QEIndex"] != null)
                        e.Row.Cells[Convert.ToInt32(Session["QEIndex"].ToString())].Style["background-color"] = drv.Row["QEColor"].ToString() != "" ? "#" + GetLighterColor(drv.Row["QEColor"].ToString()).ToString() : "White"; //
                    if (Session["OEEIndex"] != null)
                        e.Row.Cells[Convert.ToInt32(Session["OEEIndex"].ToString())].Style["background-color"] = drv.Row["OEEColor"].ToString() != "" ? "#" + GetLighterColor(drv.Row["OEEColor"].ToString()).ToString() : "white"; //
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private static string GetLighterColor(string colorValueInString)
        {
            string LighterColor = string.Empty;
            //Color color = new Color();
            Color color = Color.FromName(colorValueInString);
            try
            {
                //if (colorValueInString.Equals("Green", StringComparison.OrdinalIgnoreCase))
                //{
                    
                //    color = Color.FromArgb(1, 113, 241, 113);// 160, 243, 160
                //}
                //else if(colorValueInString.Equals("Red", StringComparison.OrdinalIgnoreCase))
                //{
                //    color = Color.FromArgb(1, 231, 135, 140); // 233, 177, 180
                //}
                //else if(colorValueInString.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                //{
                //    color = Color.FromArgb(1, 255, 255, 81); // 255, 255, 158
                //}
                Color LightColor = ControlPaint.Light(color);
                LighterColor = LightColor.R.ToString("X2") + LightColor.G.ToString("X2") + LightColor.B.ToString("X2");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetLighterColor:" + ex.Message);
            }
            return LighterColor;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string computerName = txtComputerName.Text.Trim();
                if (computerName != "")
                {
                    HttpContext.Current.Response.Cookies["ComputerName"].Value = computerName;
                    HttpContext.Current.Response.Cookies["ComputerName"].Expires = DateTime.MaxValue;
                    Response.Redirect("AndonPagePradeepMetals.aspx");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}