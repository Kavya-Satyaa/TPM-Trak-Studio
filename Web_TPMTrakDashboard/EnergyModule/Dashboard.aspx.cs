using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule.Models;

namespace Web_TPMTrakDashboard.EnergyModule
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string selectedFromDate;
        private string selectedShift;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                List<string> allShifts = DataBaseAccess.GetAllShift();
                ddlShift.DataSource = allShifts;
                ddlShift.DataBind();
                BindPlantIDs();
                ddlPlant.SelectedValue = DataBaseAccess.GetSelectedValues("EM_PlantID");
                BindCellIDs();
                ddlCell.SelectedValue = DataBaseAccess.GetSelectedValues("EM_CellID");
                ddlMachineType.SelectedValue = DataBaseAccess.GetSelectedValues("EM_Type");

                List<string> allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    tdCell.Visible = true;
                    ddlCell.Visible = true;
                }
                else
                {
                    tdCell.Visible = false;
                    ddlCell.Visible = false;
                }
                if (ddlMachineType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    tdCell.Visible = false;
                    ddlCell.Visible = false;
                    tdPlant.Visible = false;
                    ddlPlant.Visible = false;
                }
                else
                {
                    tdCell.Visible = true;
                    ddlCell.Visible = true;
                    tdPlant.Visible = true;
                    ddlPlant.Visible = true;
                }
                btnProcess_Click(null, null);
            }

        }
        private void BindPlantIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void bindTable()
        {
            try
            {
                selectedFromDate = DataBaseAccess.GetLogicalDay(txtFromDate.Text);
                DateTime fromdateTime = DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                selectedFromDate = fromdateTime.ToString("yyyy-MM-dd");
                selectedShift = ddlShift.Text.ToString();
                DataTable dt = DataBaseAccess.GetDataForDateShift(selectedFromDate, selectedShift, false, ddlMachineType.SelectedValue.ToString(), ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    ClearDataGridColumns();
                    BoundField boundField1 = new BoundField();
                    boundField1.HeaderText = "Machine ID";
                    boundField1.DataField = dt.Columns[0].ColumnName;
                    gvDashboard.Columns.Add(boundField1);
                    List<GridSettings> gridSettings = DataBaseAccess.GetGridInformation(ddlMachineType.SelectedValue.ToString());
                    foreach (GridSettings settings in gridSettings)
                    {
                        if (settings.Visibility)
                        {
                            BoundField boundField = new BoundField();
                            boundField.HeaderText = settings.ColumnText;
                            boundField.DataField = settings.ColumnName;
                            gvDashboard.Columns.Add(boundField);
                        }
                    }
                }
                gvDashboard.DataSource = dt;
                gvDashboard.DataBind();
                //if (ddlShift.SelectedIndex != 0)
                //{
                //    foreach (GridViewRow row in gvDashboard.Rows)
                //    {
                //        HyperLink hyperLink = new HyperLink();
                //        hyperLink.ID = "hlHourwise";
                //        hyperLink.Text = row.Cells[0].Text;
                //        hyperLink.ToolTip = "View Hourwise Energy For (" + hyperLink.Text + ")";
                //        row.Cells[0].Controls.Add(hyperLink);
                //    }

                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }

        //private void LinkButton_Click(object sender, EventArgs e)
        //{
        //    LinkButton linkButton = (LinkButton)sender;
        //    string machineId = linkButton.Text;
        //    DateTime datePeriod = DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        //    string date = datePeriod.ToString("yyyy-MM-dd");
        //    string shift = ddlShift.SelectedValue;
        //    lblShiftnDate.Text = date + " (Shift - " + shift + ")";
        //    lblHourwiseHeader.Text = "Hourwise Energy Cockpit For - " + machineId;
        //    gvHourwiseMonitoring.DataSource = DataBaseAccess.GetHourWiseEnergyData(machineId, date, shift, true);
        //    gvHourwiseMonitoring.DataBind();
        //    ScriptManager.RegisterStartupScript(this, GetType(), "OpenHourwiseMonitoringPopup", "OpenHourwiseMonitoring();", true);
        //}

        private void ClearDataGridColumns()
        {
            if (gvDashboard.Columns.Count > 0)
            {
                for (int i = gvDashboard.Columns.Count - 1; i >= 0; i--)
                {
                    gvDashboard.Columns.RemoveAt(i);
                }
            }
        }
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            bool timerOff = false;
            if (timerToAutoRefresh.Enabled)
            {
                timerToAutoRefresh.Enabled = false;
                timerOff = true;
            }
            bindTable();
            ScriptManager.RegisterStartupScript(this, GetType(), "CallbtnProcess", "BtnProcessClick();", true);
            if (timerOff)
            {
                timerToAutoRefresh.Enabled = true;
                timerToAutoRefresh.Interval = 1000 * Convert.ToInt32(WebConfigurationManager.AppSettings["EnergyAutoRefreshInterval"].ToString());
            }
            

        }

        protected void timerToAutoRefresh_Tick(object sender, EventArgs e)
        {
            btnProcess_Click(null, null);
        }

        protected void cbAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoRefresh.Checked)
            {

                timerToAutoRefresh.Enabled = true;
                timerToAutoRefresh.Interval = 1000 * Convert.ToInt32(WebConfigurationManager.AppSettings["EnergyAutoRefreshInterval"].ToString());
            }
            else
            {
                timerToAutoRefresh.Enabled = false;
            }
            btnProcess_Click(null, null);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetCellID(string PlantID)
        {
            List<string> list= DataBaseAccess.GetCellIDs(PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetPlantID()
        {
            List<string> list = DataBaseAccess.GetAllPlants();
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSelectedCell()
        {
            string list = DataBaseAccess.GetSelectedValues("EM_CellID");
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSelectedPlant()
        {
           string list = DataBaseAccess.GetSelectedValues("EM_PlantID");
            return list;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSelectedMachineType()
        {
           string list = DataBaseAccess.GetSelectedValues("EM_Type");
            return list;
        }
        #region "Get Hourwise Monitoring Data"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<HourwiseMonitoringData> GetHourwiseMonitoring(string machine, string date, string shift, string MachineType)
        {
            List<HourwiseMonitoringData> hourwiseMonitoringDatas = new List<HourwiseMonitoringData>();
            List<GridSettings> gridSettings = DataBaseAccess.GetGridInformation(MachineType);
            DateTime fromdateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            date = fromdateTime.ToString("yyyy-MM-dd");
            hourwiseMonitoringDatas = DataBaseAccess.GetHourWiseEnergyData(machine, date, shift, true, MachineType);
            for (int i = 0; i < hourwiseMonitoringDatas.Count; i++)
            {
                string a = hourwiseMonitoringDatas[i].ShiftHourID;
            }
            if (hourwiseMonitoringDatas != null && hourwiseMonitoringDatas.Count > 0)
            {
                hourwiseMonitoringDatas[0].columns = new List<GridSettings>();
                hourwiseMonitoringDatas[0].columns = gridSettings;
            }
            return hourwiseMonitoringDatas;
        }
        #endregion

        #region "Plot Hourwise Energy Bar Chart"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DataForEnergy GetHourwiseEnergy(string machine, string date, string shift, string MachineType)
        {
            List<HourwiseMonitoringData> hourwiseMonitoringDatas = new List<HourwiseMonitoringData>();
            DateTime fromdateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            date = fromdateTime.ToString("yyyy-MM-dd");
            hourwiseMonitoringDatas = DataBaseAccess.GetHourWiseEnergyData(machine, date, shift, true, MachineType);
            DataForEnergy chartForEnergy = new DataForEnergy();
            try
            {
                chartForEnergy.TitleEnergy = "Energy Graph";
                chartForEnergy.xAxisTitleEnergy = "Hours";
                chartForEnergy.yAxisTitleEnergy = "Energy(KwH)";

                chartForEnergy.EnergyCategories = new List<string>();
                ChartSeries series1 = new ChartSeries();
                ChartSeries series2 = new ChartSeries();

                if (hourwiseMonitoringDatas != null && hourwiseMonitoringDatas.Count > 0)
                {
                    series1.name = "Energy";
                    series2.name = "Target";
                    series1.data = new List<double>();
                    series2.data = new List<double>();
                    foreach (HourwiseMonitoringData data in hourwiseMonitoringDatas)
                    {
                        chartForEnergy.EnergyCategories.Add(data.ShiftHourID);
                        series1.data.Add(Convert.ToDouble(data.Energy));
                        series2.data.Add(Convert.ToDouble(data.Target));
                    }
                    chartForEnergy.seriesEnergy = new List<ChartSeries>();
                    chartForEnergy.seriesEnergy.Add(series1);
                    chartForEnergy.seriesEnergy.Add(series2);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return chartForEnergy;
        }
        #endregion

        #region "Plot Hourwise Summary Chart"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DataForSummery GetHourwiseSummary(string machine, string date, string shift, string MachineType)
        {
            List<HourwiseMonitoringData> hourwiseMonitoringDatas = new List<HourwiseMonitoringData>();
            DateTime fromdateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            date = fromdateTime.ToString("yyyy-MM-dd");
            hourwiseMonitoringDatas = DataBaseAccess.GetHourWiseEnergyData(machine, date, shift, true, MachineType);
            DataForSummery chartForSummery = new DataForSummery();
            chartForSummery.data = new List<pieData>();

            try
            {
                if (hourwiseMonitoringDatas != null && hourwiseMonitoringDatas.Count > 0)
                {
                    chartForSummery.name = "Energy";
                    foreach (HourwiseMonitoringData data in hourwiseMonitoringDatas)
                    {
                        pieData pData = new pieData();
                        pData.name = data.ShiftHourID;
                        if (Convert.ToDouble(data.Energy) < 0)
                        {
                            pData.y = Convert.ToDouble(data.Energy) * -1;
                            pData.positive = false;
                        }

                        else
                        {
                            pData.y = Convert.ToDouble(data.Energy);
                            pData.positive = true;
                        }

                        chartForSummery.data.Add(pData);
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return chartForSummery;
        }
        #endregion

        #region "Get Pie chart for Status measure"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DataForSummery GetSummeryPieData(string fromDate, string selectedShift, string MachineType, string Plant, string Cell)
        {
            DateTime fromdateTime = DateTime.ParseExact(fromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            fromDate = fromdateTime.ToString("yyyy-MM-dd");
            DataTable dt = DataBaseAccess.GetDataForDateShift(fromDate, selectedShift, true, MachineType, Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant, Cell.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Cell);
            DataForSummery chartForSummery = new DataForSummery();
            chartForSummery.data = new List<pieData>();

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    chartForSummery.name = dt.Columns["Energy"].ColumnName;
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        pieData pData = new pieData();
                        pData.name = dataRow["MachineID"].ToString();
                        if (Convert.ToDouble(dataRow["Energy"].ToString()) < 0)
                        {
                            pData.y = Convert.ToDouble(dataRow["Energy"].ToString()) * -1;
                            pData.positive = false;
                        }

                        else
                        {
                            pData.y = Convert.ToDouble(dataRow["Energy"].ToString());
                            pData.positive = true;
                        }

                        chartForSummery.data.Add(pData);
                    }

                }
                //else
                //{
                //    chartForSummery.name = "Machine";
                //    chartForSummery.data = new List<pieData>
                //    {
                //        new pieData{ name="FANUC", y=286}
                //    };
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return chartForSummery;
        }
        #endregion

        #region "Get Bar chart for Energy measure"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static DataForEnergy GetEnergyBarData(string fromDate, string selectedShift, string MachineType, string Plant, string Cell)
        {
            DateTime fromdateTime = DateTime.ParseExact(fromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            fromDate = fromdateTime.ToString("yyyy-MM-dd");
            DataTable dt = DataBaseAccess.GetDataForDateShift(fromDate, selectedShift, true, MachineType, Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant, Cell.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Cell);
            DataForEnergy chartForEnergy = new DataForEnergy();
            try
            {
                chartForEnergy.TitleEnergy = "Energy Graph";
                chartForEnergy.xAxisTitleEnergy = "Machine";
                chartForEnergy.yAxisTitleEnergy = "Energy(KwH)";

                chartForEnergy.EnergyCategories = new List<string>();
                if (dt != null && dt.Rows.Count > 0&& dt.Columns.Contains("Energy")&&dt.Columns.Contains("Target"))
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        chartForEnergy.EnergyCategories.Add(dataRow[0].ToString());
                    }
                    chartForEnergy.seriesEnergy = new List<ChartSeries>();
                    ChartSeries series1 = new ChartSeries();
                    ChartSeries series2 = new ChartSeries();
                    series1.name = dt.Columns["Energy"].ColumnName;
                    series2.name = dt.Columns["Target"].ColumnName;
                    series1.data = new List<double>();
                    series2.data = new List<double>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        series1.data.Add(Convert.ToDouble(dt.Rows[i]["Energy"].ToString()));
                        series2.data.Add(Convert.ToDouble(dt.Rows[i]["Target"].ToString()));
                    }
                    chartForEnergy.seriesEnergy.Add(series1);
                    chartForEnergy.seriesEnergy.Add(series2);
                }
                //else
                //{
                //    chartForEnergy.seriesEnergy = new List<ChartSeries>
                //    {
                //        new ChartSeries{ name="Energy", data=new List<double>{34,54,35,23,53,23,32,35,75,76,56,36,76,86,34,54,35,23,53,23,32,35,75,76,89}},

                //    };
                //    for (int i = 0; i <= 24; i++)
                //    {
                //        chartForEnergy.EnergyCategories.Add("FANUC");
                //    }
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

            return chartForEnergy;
        }
        #endregion
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string IsKiswokEnabled()
        {
            string enabled = "";
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                enabled = "1";
            }
            else
            {
                enabled = "0";
            }
            return enabled;
        }
        protected void gvDashboard_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "ShowLoader", "showLoader() ", true);
                BindCellIDs();
                //ScriptManager.RegisterStartupScript(this, GetType(), "CallbtnProcess", "BtnProcessClick();", true);
                //btnProcess_Click(null, null);
                //ScriptManager.RegisterStartupScript(this, GetType(), "HideLoader", "hideLoader() ", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlMachineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlMachineType.SelectedValue.ToString().Equals("Non-Machine EM", StringComparison.OrdinalIgnoreCase))
                {
                    tdCell.Visible = false;
                    ddlCell.Visible = false;
                    tdPlant.Visible = false;
                    ddlPlant.Visible = false;
                }
                else
                {
                    tdCell.Visible = true;
                    ddlCell.Visible = true;
                    tdPlant.Visible = true;
                    ddlPlant.Visible = true;
                    //BindPlantIDs();
                    //BindCellIDs();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
    public class HourwiseMonitoringData
    {
        public string ShiftHourID { get; set; }
        public string UtilisedTime { get; set; }
        public string Components { get; set; }
        public string PF { get; set; }
        public string Cost { get; set; }
        public string Energy { get; set; }
        public string Volt1 { get; set; }
        public string Volt2 { get; set; }
        public string Volt3 { get; set; }
        public string Target { get; set; }
        public string Volt4 { get; set; }
        public string Volt5 { get; set; }
        public string Volt6 { get; set; }
        public string AmpereR { get; set; }
        public string AmpereY { get; set; }
        public string AmpereB { get; set; }
        public string ProdTime_KWH { get; set; }
        public string DownTime_KWH { get; set; }
        public string DownTime { get; set; }
        public string ProductionTime { get; set; }
        public List<GridSettings> columns { get; set; }
    }
    public class ChartSeries
    {
        public string name { get; set; }
        public List<double> data { get; set; }
    }
    public class DataForEnergy
    {
        public string TitleEnergy { get; set; }
        public string xAxisTitleEnergy { get; set; }
        public string yAxisTitleEnergy { get; set; }
        public int minValueEnergy { get; set; }
        public int maxValueEnergy { get; set; }

        public List<string> EnergyCategories { get; set; }
        public List<ChartSeries> seriesEnergy { get; set; }
    }
    public class pieData
    {
        public string name { get; set; }
        public double y { get; set; }
        public bool positive { get; set; }
    }
    public class DataForSummery
    {
        public string name { get; set; }
        public List<pieData> data { get; set; }
    }
    public class ShiftTimings
    {
        public string ShiftName { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }
}