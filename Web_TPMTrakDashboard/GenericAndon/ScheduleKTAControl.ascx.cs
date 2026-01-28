using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using System.Threading;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class ScheduleKTAControl : System.Web.UI.UserControl
    {
        public static AndonSettingEntity settings = null;
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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindScheduleData()
        {
            try
            {
                if (Session["View"] == null)
                    Session["View"] = "AndonView";

                if (Session["RunOption"] != null && Session["RunOption"].ToString() != "RunByCell")
                    cellLabelDiv.Visible = false;
                else
                    cellLabelDiv.Visible = true;
                if (Session["View"].ToString().Equals("AndonView", StringComparison.OrdinalIgnoreCase))
                {

                    if (Session["BindCacheData"] != null)
                    {
                        AllAdnonEntity andonData = (AllAdnonEntity)Session["BindCacheData"];
                        settings = andonData.AdnonSetting;
                        List<ScheduleKTAEntity> list = new List<ScheduleKTAEntity>();
                        list = andonData.ScheduleKTAData;
                        if (list.Count > 0)
                        {
                            if (Request.Cookies["AndonCellID"].ToString().Equals("All", StringComparison.OrdinalIgnoreCase) || Request.Cookies["AndonCellID"].ToString().Equals(""))
                            {
                                cellList = list.Select(k => k.CellID).Distinct().ToList();
                                Session["CellList_Schedule"] = cellList;
                                if (Session["CellCount_Schedule"] == null)
                                    Session["CellCount_Schedule"] = 0;

                                int.TryParse(Session["CellCount_Schedule"].ToString(), out cellCount);
                                list = list.Where(k => k.CellID == cellList[cellCount]).ToList();

                                lblCellName.Text = cellList[cellCount];
                                cellCount++;
                                Session["CellCount_Schedule"] = cellCount;
                            }
                            else
                                lblCellName.Text = "";

                            int maxCount = list.Select(k => k.ComponentList.Count()).Max();
                            int listCount = list.Count;
                            for (int i = 0; i < listCount; i++)
                            {
                                int compListCount = list[i].ComponentList.Count();
                                if (compListCount < maxCount)
                                {
                                    for (int j = compListCount; j < maxCount; j++)
                                        list[i].ComponentList.Add(new ScheduleKTAEntity());
                                }
                            }

                            lvScheduleKTA.DataSource = list;
                            lvScheduleKTA.DataBind();
                            if (list.Count > 0)
                            {
                                rows = flips = list.Count;
                                rowsToTake = 1;
                                Session["Flips_Schedule"] = flips;
                                Session["ScheduleData"] = list;
                                Session["RowsToTake_Schedule"] = rowsToTake;
                                Session["Rows_Schedule"] = rows;
                                rowsToTake++;
                            }
                           

                            if (Convert.ToBoolean(Convert.ToInt32(list[0].KTAWrapContent)))
                                hdnWrapContent.Value = Convert.ToInt32(list[0].KTAWrapContent).ToString();
                            else
                                hdnWrapContent.Value = "0";
                            ScriptManager.RegisterStartupScript(this, GetType(), "setFlipIntervalKTA", "setFlipIntervalKTA();", true);
                        }
                        else
                        {
                            List<ScreenEntity> screenList = Session["EnabledScreens"] as List<ScreenEntity>;
                            if (screenList.Count == 1 && screenList[0].Screen.Equals("ScheduleKTAControl", StringComparison.OrdinalIgnoreCase))
                            {
                                Logger.WriteDebugLog("Empty Data Case in Shcedule Control");
                                List<ScheduleKTAEntity> tempList = new List<ScheduleKTAEntity>();
                                lvScheduleKTA.DataSource = tempList;
                                lvScheduleKTA.DataBind();
                            }
                            else
                            {
                                callNextScreen();
                            }
                        }
                        flipInterval.Enabled = true;
                        flipInterval.Interval = andonData.AdnonSetting.ScheduleInterval;
                    }
                    else
                    {
                        Session["RunningScreen"] = null;
                        callNextScreen();
                    }
                }
                else if (Session["View"].ToString().Equals("DesktopView", StringComparison.OrdinalIgnoreCase))
                {
                    hdntotalWidth.Value = "";
                    if (Session["BindCacheData"] != null)
                    {
                        AllAdnonEntity andonData = (AllAdnonEntity)Session["BindCacheData"];
                        settings = andonData.AdnonSetting;
                        List<ScheduleKTAEntity> list = new List<ScheduleKTAEntity>();
                        list = andonData.ScheduleKTAData;
                        if (list.Count > 0)
                        {
                            int maxCount = list.Select(k => k.ComponentList.Count()).Max();
                            int listCount = list.Count;
                            for (int i = 0; i < listCount; i++)
                            {
                                int compListCount = list[i].ComponentList.Count();
                                if (compListCount < maxCount)
                                {
                                    for (int j = compListCount; j < maxCount; j++)
                                    {
                                        list[i].ComponentList.Add(new ScheduleKTAEntity());
                                    }
                                }
                            }
                            cellLabelDiv.Visible = false;
                            hdntotalWidth.Value = "";
                            hdnWidth.Value = "";
                            hdntdHeight.Value = "";
                            lvScheduleKTA.DataSource = list;
                            lvScheduleKTA.DataBind();
                            flipInterval.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidthKTA", "SetIconicBoxWidthKTA();", true);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindScheduleData = " + ex.ToString());
            }
        }

        private void callNextScreen()
        {
            try
            {
                flipInterval.Enabled = false;
                ((AndonPage)this.Page).showNextControl();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindScheduleData = " + ex.Message);
            }
        }

        protected void flipInterval_Tick(object sender, EventArgs e)

        {
            try
            {
                Logger.WriteDebugLog("Schedule Control_Timer Tick");
                if (Session["Flips_Schedule"] == null)
                {
                    callNextScreen();
                    return;
                }

                int.TryParse(Session["Flips_Schedule"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["RowsToTake_Schedule"].ToString(), out rowsToTake);
                    int.TryParse(Session["Rows_Schedule"].ToString(), out rows);
                    List<ScheduleKTAEntity> list = (List<ScheduleKTAEntity>)Session["ScheduleData"];
                    int skipRows = rows * rowsToTake;
                    lvScheduleKTA.DataSource = list.Skip(skipRows).Take(rows);
                    lvScheduleKTA.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["Flips_Schedule"] = flips;
                    Session["RowsToTake_Schedule"] = rowsToTake;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setDivHeightKTA", "setDivHeightKTA();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidthKTA", "SetIconicBoxWidthKTA();", true);
                }

                else
                {
                    if (Session["CellCount_Schedule"] == null)
                        callNextScreen();
                    else
                    {
                        int.TryParse(Session["CellCount_Schedule"].ToString(), out cellCount);
                        cellList = Session["CellList_Schedule"] as List<string>;
                        if (cellCount >= cellList.Count)
                            callNextScreen();
                        else
                            BindScheduleData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("flipInterval_Tick Schedule KTA = " + ex.Message);
            }
        }
    }
}