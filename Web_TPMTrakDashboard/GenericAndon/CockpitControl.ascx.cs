using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class CockpitControl : System.Web.UI.UserControl
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
            //settings=

        }
        public void BindCockpitData()
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
                        List<CockpitData> list = new List<CockpitData>();
                        list = andonData.CockpitData;

                        if (list.Count > 0)
                        {
                            if (Request.Cookies["AndonCellID"].Value.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) || Request.Cookies["AndonCellID"].Value.ToString().Equals(""))
                            {
                                cellList = list.Select(k => k.GroupName).Distinct().ToList();
                                Session["CellList_Cockpit"] = cellList;
                                if (Session["CellCount_Cockpit"] == null)
                                    Session["CellCount_Cockpit"] = 0;

                                int.TryParse(Session["CellCount_Cockpit"].ToString(), out cellCount);

                                list = list.Where(k => k.GroupName == cellList[cellCount]).ToList();
                                lblCellName.Text = cellList[cellCount];
                                cellCount++;
                                Session["CellCount_Cockpit"] = cellCount;
                                 
                            }
                            else
                                lblCellName.Text = "";

                            lvCockpit.DataSource = list;
                            lvCockpit.DataBind();

                            rows = flips = list.Count;
                            rowsToTake = 1;
                            Session["Flips_Cockpit"] = flips;
                            Session["CockpitData"] = list;
                            Session["RowsToTake_Cockpit"] = rowsToTake;
                            Session["Rows_Cockpit"] = rows;
                            rowsToTake++;
                            ScriptManager.RegisterStartupScript(this, GetType(), "setFlipIntervalProductionAndon", "setFlipInterval();", true);
                        }
                        else
                        {
                            List<ScreenEntity> screenList = Session["EnabledScreens"] as List<ScreenEntity>;
                            if (screenList.Count == 1 && screenList[0].Screen.Equals("CockpitControl", StringComparison.OrdinalIgnoreCase))
                            {
                                Logger.WriteDebugLog("Empty Data Case in Cockpit Andon");
                                List<CockpitData> tempList = new List<CockpitData>();
                                lvCockpit.DataSource = tempList;
                                lvCockpit.DataBind();
                                flipInterval.Enabled = true;
                                flipInterval.Interval = andonData.AdnonSetting.ScreenFlipInterval;
                            }
                            else
                            {
                                callNextScreen();
                            }
                        }
                        flipInterval.Enabled = true;
                        flipInterval.Interval = andonData.AdnonSetting.ScreenFlipInterval;
                    }
                    else
                    {
                        Session["RunningScreen"] = null;
                        callNextScreen();
                    }
                }
                else if (Session["View"].ToString().Equals("DesktopView", StringComparison.OrdinalIgnoreCase))
                {
                    if (Session["BindCacheData"] != null)
                    {
                        AllAdnonEntity andonData = (AllAdnonEntity)Session["BindCacheData"];
                        List<CockpitData> list = new List<CockpitData>();
                        list = andonData.CockpitData;
                        hdntotalWidth.Value = "";
                        hdnWidth.Value = "";
                        cellLabelDiv.Visible = false;

                        lvCockpit.DataSource = list;
                        lvCockpit.DataBind();

                        flipInterval.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidth", "SetIconicBoxWidth();", true);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCockpitData = " + ex.ToString());
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
                Logger.WriteErrorLog("callNextScreen = " + ex.ToString());
            }
        }

        protected void flipInterval_Tick(object sender, EventArgs e)
        {
            try
            {
                Logger.WriteDebugLog("Cockpit Control_Timer Tick");
                if (Session["Flips_Cockpit"] == null)
                {
                    BindCockpitData();
                    return;
                }
                int.TryParse(Session["Flips_Cockpit"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["RowsToTake_Cockpit"] != null ? Session["RowsToTake_Cockpit"].ToString() : "0", out rowsToTake);
                    int.TryParse(Session["Rows_Cockpit"] != null ? Session["Rows_Cockpit"].ToString() : "0", out rows);
                    List<CockpitData> list = Session["CockpitData"] != null ? (List<CockpitData>)Session["CockpitData"] : new List<CockpitData>();
                    int skipRows = rows * rowsToTake;
                    lvCockpit.DataSource = list.Skip(skipRows).Take(rows);
                    lvCockpit.DataBind();
                    flips--;
                    rowsToTake++;
                    Session["Flips_Cockpit"] = flips;
                    Session["RowsToTake_Cockpit"] = rowsToTake;


                    ScriptManager.RegisterStartupScript(this, GetType(), "setMachineFontSize", "setMachineFontSize();", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidth", "SetIconicBoxWidth();", true);
                }
                else
                {
                    if (Session["CellCount_Cockpit"] == null)
                    {
                        callNextScreen();
                    }
                    else
                    {
                        int.TryParse(Session["CellCount_Cockpit"].ToString(), out cellCount);
                        cellList = Session["CellList_Cockpit"] as List<string>;
                        if (cellCount >= cellList.Count)
                        {
                            callNextScreen(); //Last Cell Displayed
                        }
                        else
                        {
                            BindCockpitData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("flipInterval_Tick_Cockpit Screen = " + ex.ToString());
            }
        }

    }
}