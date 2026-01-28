using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class StoreAndonKTA : System.Web.UI.Page
    {
        public static SettingsViewEntityStore andonSetting = null;
        public static int refreshInterval = 0;
        int rows = 0;
        int count = 0;
        int flips = 0;
        int rowsToTake = 0;
        public int HeaderFontsize = 20;
        public int ContentFontsize = 19;
        public int topDowncode = 0;
        public string displaytype = "";
        int cellCount = 0;
        List<string> cellList = new List<string>();
        public string fontfamily = "";
        public string fontstyle = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    timer.Enabled = false;
                    Session["AndonStoreCellID"] = null;
                    Session["AndonStorePlant"] = null;
                    Session["CellCount"] = null;
                    Session["AndonType"] = "AndonView";

                    setCompanyLogo();
                    BindPlantID();
                    BindCellID();

                    Cache.Remove($"BindStoreCacheData{HttpContext.Current.Session.SessionID}");
                    Cache.Remove($"MainStoreCacheData{HttpContext.Current.Session.SessionID}");
                    chkDesktopView.Checked = true;
                    chkDesktopView_CheckedChanged(null, null);

                    //BindStoreData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Page_Load =" + ex.Message);
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

        private static void insertLatestDataToMainCache()
        {
            try
            {
                AllAdnonStoreEntity data = new AllAdnonStoreEntity();
                List<AndonDefaultsEntity> andonSettingList = AndonDBAccess.getAndonStoreSettingDetails();
                List<AndonDefaultsEntity> MainsettingList = AndonDBAccess.getMainStoreSettingDetails();

                #region ------ Setting Data -----------
                andonSetting = new SettingsViewEntityStore();
                andonSetting.PlantToDisplay = andonSettingList.Where(k => k.ValueInText.Equals("PlantToDisplay", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                string value = andonSettingList.Where(k => k.ValueInText.Equals("ScreenFlipInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.ScreenFlipInterval = (string.IsNullOrEmpty(value) ? 10 : Convert.ToInt32(value)) * 1000;
                value = andonSettingList.Where(k => k.ValueInText.Equals("DataDisplayInterval", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.DataDisplayInterval = (string.IsNullOrEmpty(value) ? 10 : Convert.ToInt32(value)) * 1000;
                andonSetting.DateFormat = andonSettingList.Where(k => k.ValueInText.Equals("DateFormatForHeader", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (string.IsNullOrEmpty(andonSetting.DateFormat))
                {
                    andonSetting.DateFormat = "dd-MM-yyyy";
                }
                andonSetting.TimeFormat = andonSettingList.Where(k => k.ValueInText.Equals("TimeFormatForHeader", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                if (string.IsNullOrEmpty(andonSetting.TimeFormat))
                {
                    andonSetting.TimeFormat = "HH:mm:ss";
                }
                andonSetting.AndonTitle = andonSettingList.Where(k => k.ValueInText.Equals("AndonTitle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontFamily = andonSettingList.Where(k => k.ValueInText.Equals("FontFamily", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                andonSetting.FontStyle = andonSettingList.Where(k => k.ValueInText.Equals("FontStyle", StringComparison.OrdinalIgnoreCase)).Select(k => k.ValueInText2).FirstOrDefault();
                data.AdnonSetting = andonSetting;
                #endregion

                string ShiftStart = "", ShiftEnd = "";
                ShiftStart = DBAccess.GetShiftstart(out ShiftEnd);
                ShiftStart = Convert.ToDateTime(ShiftStart).ToString("yyyy-MM-dd HH:mm:ss");
                ShiftEnd = Convert.ToDateTime(ShiftEnd).ToString("yyyy-MM-dd HH:mm:ss");
                string plantID = HttpContext.Current.Session["AndonStorePlant"].ToString();
                string cellID = HttpContext.Current.Session["AndonStoreCellID"].ToString();
                data.StoreData = AndonDBAccess.getStoreAndonKTAData(plantID, cellID, ShiftStart, ShiftEnd, "", MainsettingList, andonSettingList, HttpContext.Current.Session["AndonType"].ToString());

                Guid guid = Guid.NewGuid();
                data.AutoGeneratedID = guid.ToString();
                HttpContext.Current.Cache.Remove($"MainStoreCacheData{HttpContext.Current.Session.SessionID}");
                HttpContext.Current.Cache.Insert($"MainStoreCacheData{HttpContext.Current.Session.SessionID}", data, null, DateTime.Now.AddMinutes(4), TimeSpan.Zero);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertAllDataToMainCache_StoreAndon_KTA = " + ex.Message);
            }
        }

        private void BindStoreData()
        {
            try
            {
                setCompanyLogo();
                AllAdnonStoreEntity andonBindCacheData = new AllAdnonStoreEntity();
                AllAdnonStoreEntity andonMainCacheData = new AllAdnonStoreEntity();
                if (Cache[$"BindStoreCacheData{HttpContext.Current.Session.SessionID}"] == null)
                {
                    if (Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"] != null)
                    {
                        Cache.Insert($"BindStoreCacheData{HttpContext.Current.Session.SessionID}", (AllAdnonStoreEntity)Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"], null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }

                }
                else
                {
                    if (Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"] != null)
                    {
                        andonMainCacheData = (AllAdnonStoreEntity)Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"];
                        andonBindCacheData = (AllAdnonStoreEntity)Cache[$"BindStoreCacheData{HttpContext.Current.Session.SessionID}"];
                        if (andonBindCacheData.AutoGeneratedID != andonMainCacheData.AutoGeneratedID)
                        {
                            Cache.Remove($"BindStoreCacheData{HttpContext.Current.Session.SessionID}");
                            Cache.Insert($"BindStoreCacheData{HttpContext.Current.Session.SessionID}", (AllAdnonStoreEntity)Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"], null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                        }
                    }
                }
                andonBindCacheData = (AllAdnonStoreEntity)Cache[$"BindStoreCacheData{HttpContext.Current.Session.SessionID}"];
                andonSetting = andonBindCacheData.AdnonSetting;
                lblShift.Text = "Shift - " + AndonDBAccess.GetCurrentShift();
                try
                {
                    string format = andonBindCacheData.AdnonSetting.DateFormat + " " + andonBindCacheData.AdnonSetting.TimeFormat;
                    lblDateTime.InnerText = DateTime.Now.ToString(format);
                }
                catch (Exception ex)
                {
                    lblDateTime.InnerText = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }
                headerName.InnerText = andonSetting.AndonTitle;
                if (Session["AndonStorePlant"] != null)
                {
                    if (ddlPlantID.Items.FindByValue(Session["AndonStorePlant"].ToString()) != null)
                    {
                        ddlPlantID.SelectedValue = Session["AndonStorePlant"].ToString();
                    }
                }
                if (Session["AndonStoreCellID"] != null)
                {
                    if (ddlCellID.Items.FindByValue(Session["AndonStoreCellID"].ToString()) != null)
                    {
                        ddlCellID.SelectedValue = Session["AndonStoreCellID"].ToString();
                    }
                    else
                    {
                        BindCellID();
                    }
                }
                List<StoreAndonKTAEntity> list = new List<StoreAndonKTAEntity>();
                list = andonBindCacheData.StoreData;

                cellCount = 0;
                if (list.Count > 0)
                {
                    if (ddlCellID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) || ddlCellID.SelectedValue.ToString().Equals(""))
                    {
                        cellList = list.Select(k => k.CellID).Distinct().ToList();
                        HttpContext.Current.Session["CellList"] = cellList;
                        if (HttpContext.Current.Session["CellCount"] == null)
                        {
                            HttpContext.Current.Session["CellCount"] = 0;
                        }
                        if (HttpContext.Current.Session["CellCount"].Equals(cellList.Count))
                        {
                            HttpContext.Current.Session["CellCount"] = 0;
                        }
                        int.TryParse(HttpContext.Current.Session["CellCount"].ToString(), out cellCount);


                        list = list.Where(k => k.CellID == cellList[cellCount]).ToList();

                        lblCellName.Text = cellList[cellCount];
                        cellCount++;
                        HttpContext.Current.Session["CellCount"] = cellCount;
                    }
                    else
                    {
                        lblCellName.Text = "";
                    }

                    //int maxCount = list.Select(k => k.ComponentList.Count()).Max();
                    //int listCount = list.Count;
                    //for (int i = 0; i < listCount; i++)
                    //{
                    //    int compListCount = list[i].ComponentList.Count();
                    //    if (compListCount < maxCount)
                    //    {
                    //        for (int j = compListCount; j < maxCount; j++)
                    //        {
                    //            list[i].ComponentList.Add(new StoreAndonKTAEntity());
                    //        }
                    //    }
                    //}

                    lvStoreAndonKTA.DataSource = list;
                    lvStoreAndonKTA.DataBind();

                    if (list.Count > 0)
                    {
                        rows = flips = list.Count;
                        rowsToTake = 1;
                        Session["Flips"] = flips;
                        Session["StoreData"] = list;
                        Session["RowsToTake"] = rowsToTake;
                        Session["Rows"] = rows;
                        rowsToTake++;
                    }

                    timer.Interval = andonSetting.ScreenFlipInterval;
                    timer.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setFlipInterval", "setFlipInterval();", true);
                }
                else
                {
                    lvStoreAndonKTA.DataSource = new List<StoreAndonKTAEntity>();
                    lvStoreAndonKTA.DataBind();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindDesktopBindData()
        {
            try
            {
                setCompanyLogo();
                AllAdnonStoreEntity andonBindCacheData = new AllAdnonStoreEntity();
                AllAdnonStoreEntity andonMainCacheData = new AllAdnonStoreEntity();
                if (Cache[$"BindStoreCacheData{HttpContext.Current.Session.SessionID}"] == null)
                {
                    if (Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"] != null)
                    {
                        Cache.Insert($"BindStoreCacheData{HttpContext.Current.Session.SessionID}", (AllAdnonStoreEntity)Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"], null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                    }

                }
                else
                {
                    if (Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"] != null)
                    {
                        andonMainCacheData = (AllAdnonStoreEntity)Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"];
                        andonBindCacheData = (AllAdnonStoreEntity)Cache[$"BindStoreCacheData{HttpContext.Current.Session.SessionID}"];
                        if (andonBindCacheData.AutoGeneratedID != andonMainCacheData.AutoGeneratedID)
                        {
                            Cache.Remove($"BindStoreCacheData{HttpContext.Current.Session.SessionID}");
                            Cache.Insert($"BindStoreCacheData{HttpContext.Current.Session.SessionID}", (AllAdnonStoreEntity)Cache[$"MainStoreCacheData{HttpContext.Current.Session.SessionID}"], null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                        }
                    }
                }

                andonBindCacheData = (AllAdnonStoreEntity)Cache[$"BindStoreCacheData{HttpContext.Current.Session.SessionID}"];
                andonSetting = andonBindCacheData.AdnonSetting;
                lblShift.Text = "Shift - " + AndonDBAccess.GetCurrentShift();
                try
                {
                    string format = andonBindCacheData.AdnonSetting.DateFormat + " " + andonBindCacheData.AdnonSetting.TimeFormat;
                    lblDateTime.InnerText = DateTime.Now.ToString(format);
                }
                catch (Exception ex)
                {
                    lblDateTime.InnerText = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }
                headerName.InnerText = andonSetting.AndonTitle;

                List<StoreAndonKTAEntity> list = new List<StoreAndonKTAEntity>();
                list = andonBindCacheData.StoreData;
                if (list.Count > 0)
                {
                    //int maxCount = list.Select(k => k.ComponentList.Count()).Max();
                    //int listCount = list.Count;
                    //for (int i = 0; i < listCount; i++)
                    //{
                    //    int compListCount = list[i].ComponentList.Count();
                    //    if (compListCount < maxCount)
                    //    {
                    //        for (int j = compListCount; j < maxCount; j++)
                    //        {
                    //            list[i].ComponentList.Add(new StoreAndonKTAEntity());
                    //        }
                    //    }
                    //}
                    list.OrderBy(x => x.ComponentList.Count).ToList();
                    lblCellName.Text = "";
                    hdntotalWidth.Value = "";
                    hdnWidth.Value = "";
                    hdntdHeight.Value = "";
                    lvStoreAndonKTA.DataSource = list;
                    lvStoreAndonKTA.DataBind();
                    timer.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidth", "SetIconicBoxWidth();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindPlantID()
        {
            try
            {
                ddlPlantID.DataSource = AndonDBAccess.getPlantID();
                ddlPlantID.DataBind();
                if (ddlPlantID.Items.Count > 0)
                {
                    ddlPlantID.Items.Insert(0, new ListItem() { Text = "Plant All", Value = "All" });
                }
                Session["AndonStorePlant"] = ddlPlantID.SelectedValue;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
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
                if (Session["AndonStoreCellID"] != null)
                {
                    if (ddlCellID.Items.FindByValue(Session["AndonStoreCellID"].ToString()) != null)
                    {
                        ddlCellID.SelectedValue = Session["AndonStoreCellID"].ToString();
                    }
                }
                Session["AndonStoreCellID"] = ddlCellID.SelectedValue;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCellID =" + ex.Message);
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["AndonStorePlant"] = ddlPlantID.SelectedValue;
                BindCellID();
                ddlCellID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlantID_SelectedIndexChanged =" + ex.Message);
            }
        }
        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["AndonStoreCellID"] = ddlCellID.SelectedValue;
                insertLatestDataToMainCache();
                if (Session["AndonType"].ToString().Equals("DestopView", StringComparison.OrdinalIgnoreCase))
                {
                    BindDesktopBindData();
                }
                else
                    BindStoreData();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlCellID_SelectedIndexChanged =" + ex.ToString());
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
                Logger.WriteErrorLog("setFlipIntervalToSession (Store) = " + ex.ToString());
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
                Logger.WriteErrorLog("insertLatestDataToMainCacheMemory (Store) = " + ex.ToString());
            }
        }

        protected void flipInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Session["Flips"] == null)
                {
                    timer.Enabled = false;
                    BindStoreData();
                    return;
                }
                int.TryParse(Session["Flips"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["RowsToTake"].ToString(), out rowsToTake);
                    int.TryParse(Session["Rows"].ToString(), out rows);
                    List<StoreAndonKTAEntity> list = (List<StoreAndonKTAEntity>)Session["StoreData"];
                    int skipRows = rows * rowsToTake;
                    lvStoreAndonKTA.DataSource = list.Skip(skipRows).Take(rows);
                    lvStoreAndonKTA.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["Flips"] = flips;
                    Session["RowsToTake"] = rowsToTake;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setIconicBoxtdHeight", "setIconicBoxtdHeight();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidth", "SetIconicBoxWidth();", true);
                }
                else
                {
                    if (Session["CellCount"] == null)
                    {
                        BindStoreData();
                    }
                    else
                    {
                        int.TryParse(Session["CellCount"].ToString(), out cellCount);
                        cellList = Session["CellList"] as List<string>;
                        if (cellCount >= cellList.Count)
                        {
                            HttpContext.Current.Session["CellCount"] = 0;
                        }
                        BindStoreData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("flipInterval_Tick Store = " + ex.ToString());
            }
        }

        protected void chkDesktopView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                hdntype.Value = string.Empty;
                if (chkDesktopView.Checked)
                {
                    Session["AndonType"] = "DestopView";
                    hdntype.Value = "DestopView";
                    insertLatestDataToMainCache();
                    BindDesktopBindData();
                }
                else
                {
                    Session["AndonType"] = "AndonView";
                    hdntype.Value = "AndonView";
                    insertLatestDataToMainCache();
                    BindStoreData();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }


        protected void lnkComponent_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedlv = (ListViewItem)(sender as LinkButton).NamingContainer;
                string CompID = (selectedlv.FindControl("lnkComponent") as LinkButton).Text;
                string Op = (selectedlv.FindControl("OperationID") as Label).Text;
                string Desc = (selectedlv.FindControl("OperationDesc") as Label).Text;
                string URL = "CompOpDocumentView.aspx?" + string.Format("CompID={0}&OperationNo={1}&OpDesc={2}", HttpUtility.UrlEncode(CompID), HttpUtility.UrlEncode(Op), HttpUtility.UrlEncode(Desc));
                //URL = System.Web.HttpUtility.UrlEncode(URL);
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CompOpDocumentViewPage", "OpenCompOpDocumentView('" + URL + "')", true);
                chkDesktopView_CheckedChanged(null, null);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

    }
}