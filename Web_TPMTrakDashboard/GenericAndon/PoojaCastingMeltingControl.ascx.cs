using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class PoojaCastingMeltingControl : System.Web.UI.UserControl
    {
        public static AndonSettingEntity settings = null;
        public static int refreshInterval = 0;
        int rows = 0;
        int flips = 0;
        int rowsToTake = 0;
        public int HeaderFontsize = 20;
        public int ContentFontsize = 19;
        public int topDowncode = 0;
        public string displaytype = "";
        List<string> cellList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindPoojaMeltingData()
        {
            try
            {
                if (Session["BindCacheData"] != null)
                {
                    AllAdnonEntity andonData = (AllAdnonEntity)Session["BindCacheData"];
                    settings = andonData.AdnonSetting;
                    DataTable inputList = new DataTable();

                    DataTable dt2 = new DataTable();
                    dt2 = andonData.PoojaCastingColors;
                    Session["PoojaColors"] = dt2;
                    List<PoojaCastingMeltingEntity> list = new List<PoojaCastingMeltingEntity>();
                    List<PoojaCastingFurnaceEntity> list1 = new List<PoojaCastingFurnaceEntity>();
                    hdnviewType.Value = settings.PoojaViewType;
                    inputList = andonData.PoojaCastingData;

                    if (inputList.Rows.Count > 0)
                    {
                        if (settings.PoojaViewType == "Table")
                        {
                            PoojaCastingMeltingCockpit.Visible = false;
                            var distFurnace = inputList.AsEnumerable().Select(k => k.Field<string>("MachineID")).Distinct().ToList();
                            PoojaCastingFurnaceEntity data = null;
                            foreach (var furnace in distFurnace)
                            {
                                var firstRow = inputList.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(furnace, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                DataTable FurnaceList = inputList.AsEnumerable().Where(k => k.Field<string>("MachineID") == furnace).CopyToDataTable();
                                Session["DynamicParam"] = FurnaceList;
                                if (!FurnaceList.Columns.Contains("MachineID"))
                                    FurnaceList.Columns.Add("MachineID");

                                data = new PoojaCastingFurnaceEntity();
                                data.Furnace = furnace;

                                data.HeatCode = firstRow["HeatCode"].ToString();
                                data.HeatNo = firstRow["HeatNo"].ToString();
                                data.GradeID = firstRow["GradeID"].ToString();
                                data.GradeNorm = firstRow["GradeNorm"].ToString();
                                data.Date = Convert.ToDateTime(firstRow["DateTime"].ToString());
                                data.Settings = Convert.ToInt32(firstRow["Settings"].ToString());
                                //FurnaceList.Columns.Remove("MachineID");
                                FurnaceList.Columns.Remove("HeatCode");
                                FurnaceList.Columns.Remove("HeatNo");
                                FurnaceList.Columns.Remove("GradeID");
                                FurnaceList.Columns.Remove("GradeNorm");
                                FurnaceList.Columns.Remove("DateTime");
                                FurnaceList.Columns.Remove("Settings");
                                //FurnaceList.Columns.Remove("Status");
                                if (data.Settings == 1 || data.Settings == 2)
                                    FurnaceList.Columns.Remove("DensityValue");
                                data.dtParameters = FurnaceList;
                                list1.Add(data);
                            }
                            lvMachineDetails.DataSource = list1.Take(2);
                            lvMachineDetails.DataBind();

                        }

                        if (settings.PoojaViewType == "Cockpit")
                        {
                            PoojaCastingMeltingTable.Visible = false;
                            for (int i = 0; i < inputList.Rows.Count; i++)
                            {
                                var row = inputList.Rows[i];
                                var rows = dt2.Rows[i];
                                PoojaCastingMeltingEntity data = new PoojaCastingMeltingEntity();
                                data.Furnace = row["MachineID"].ToString();
                                List<PoojaCastingParameterList> param = new List<PoojaCastingParameterList>();
                                param.Add(new PoojaCastingParameterList() { ParameterText = "HeatCode", ParameterValue = row["HeatCode"].ToString(), Display1 = "none" });
                                param.Add(new PoojaCastingParameterList() { ParameterText = "HeatNo", ParameterValue = row["HeatNo"].ToString(), Display1 = "none" });
                                param.Add(new PoojaCastingParameterList() { ParameterText = "GradeID", ParameterValue = row["GradeID"].ToString(), Display1 = "none" });
                                param.Add(new PoojaCastingParameterList() { ParameterText = "GradeNorm", ParameterValue = row["GradeNorm"].ToString(), Display1 = "none" });
                                param.Add(new PoojaCastingParameterList() { ParameterText = "DateTime", ParameterValue = row["DateTime"].ToString(), Display1 = "none" });
                                for (int col = 5; col < inputList.Columns.Count; col++)
                                {
                                    PoojaCastingParameterList data1 = new PoojaCastingParameterList();
                                    data1.ParameterText = inputList.Columns[col].ColumnName;

                                    data1.ParameterValue = row[data1.ParameterText].ToString();
                                    data1.backColor = rows[col].ToString();
                                    if (data1.ParameterText == "Status")
                                    {
                                        data1.Display1 = "block";
                                        data1.Display2 = "None";
                                    }
                                    else
                                    {
                                        data1.Display1 = "None";
                                        data1.Display2 = "block";
                                    }
                                    param.Add(data1);
                                }
                                data.ParameterList = param;
                                list.Add(data);
                            }
                            lvMeltingData.DataSource = list;
                            lvMeltingData.DataBind();
                        }

                        if (inputList.Rows.Count > 0)
                        {
                            rowsToTake = 2;
                            rows = 1;
                            flips = 3;
                            Session["RowsToTake"] = rowsToTake;
                            Session["Rows"] = rows;
                            Session["Flips"] = flips;
                            Session["PoojaMeltingData"] = list;
                            Session["PoojaMeltingCockpitData"] = list1;
                            rowsToTake++;
                        }
                        flipInterval1.Enabled = true;
                        flipInterval1.Interval = andonData.AdnonSetting.ScreenFlipInterval;
                        ScriptManager.RegisterStartupScript(this, GetType(), "setFlipIntervalPooja", "setFlipIntervalPooja();", true);
                    }
                    else
                        callNextScreen();
                }
                else
                    callNextScreen();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void callNextScreen()
        {
            try
            {
                flipInterval1.Enabled = false;
                ((AndonPage)this.Page).showNextControl();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPoojaMeltingData = " + ex.Message);
            }
        }

        protected void flipInterval1_Tick(object sender, EventArgs e)
        {
            try
            {

                Logger.WriteDebugLog("Pooja Andon Control_Timer Tick");
                if (Session["Flips"] == null)
                    BindPoojaMeltingData();
                int.TryParse(Session["Flips"].ToString(), out flips);
                if (flips > 1)
                {
                    int.TryParse(Session["RowsToTake"].ToString(), out rowsToTake);
                    int.TryParse(Session["Rows"].ToString(), out rows);
                    List<PoojaCastingFurnaceEntity> input = (List<PoojaCastingFurnaceEntity>)Session["PoojaMeltingCockpitData"];
                    List<PoojaCastingMeltingEntity> list = (List<PoojaCastingMeltingEntity>)Session["PoojaMeltingData"];
                    int skipRows = rows * rowsToTake;
                    if (settings.PoojaViewType == "Table")
                    {
                        lvMachineDetails.DataSource = input.Skip(skipRows).Take(2);
                        lvMachineDetails.DataBind();
                    }

                    if (settings.PoojaViewType == "Cockpit")
                    {
                        lvMeltingData.DataSource = list.Skip(skipRows).Take(rows);
                        lvMeltingData.DataBind();

                    }
                    flips--;
                    rowsToTake += 2;
                    Session["Flips"] = flips;
                    Session["RowsToTake"] = rowsToTake;
                    ScriptManager.RegisterStartupScript(this, GetType(), "setBoxWidthPooja", "SetIconicBoxWidthPooja();", true);
                }
                else
                    callNextScreen();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void gvParameter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtColor = new DataTable();
                    dtColor = Session["PoojaColors"] as DataTable;
                    GridView gridView = (GridView)sender;
                    DataRow furnaceColorRow = null;
                    for (int col = 0; col < e.Row.Cells.Count; col++)
                    {
                        if (col == 0)
                        {
                            furnaceColorRow = dtColor.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(e.Row.Cells[0].Text, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        }
                        else if (col == e.Row.Cells.Count - 1)
                        {
                            Label label = new Label();
                            label.ID = "lblStatus";
                            label.CssClass = "lblFurnaceStatusStyle";
                            label.Style["background-color"] = furnaceColorRow["Status"].ToString();
                            e.Row.Cells[col].Controls.Add(label);
                        }
                        else
                        {
                            string colNmae = gridView.HeaderRow.Cells[col].Text;
                            e.Row.Cells[col].Style["background-color"] = furnaceColorRow[colNmae].ToString();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

    }
}