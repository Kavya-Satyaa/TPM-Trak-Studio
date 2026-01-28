using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ProcessParameterDashboard : System.Web.UI.Page
    {
        public static TempPresChartData tempData = new TempPresChartData();
        public static TempPresChartData presData = new TempPresChartData();
        public static DataTable dtTemp = new DataTable();
        public static DataTable dtPres = new DataTable();
        public static string lastviewedTime = string.Empty;
        public static string componentID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["lastviewedTime"] = "";
            string endTime = string.Empty;
            string startTime = DataBaseAccess.GetCurrentShiftStart(out endTime);
            txtFromDate.Text = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss");
            txtToDate.Text = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss");
            BindMachines();
        }

        private void BindMachines()
        {
            try
            {
                var allMachineName = VDGDataBaseAccess.GetAllMachines("All");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                    ddlMachineId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> BindCycleStartTime(string startDT, string endDT, string MachineID, string param)
        {
            List<string> cycleStartTime = new List<string>();
            try
            {
                cycleStartTime = DataBaseAccess.GetAllCycleStartTimes(startDT, endDT, MachineID, param);
                cycleStartTime.Insert(0, "All");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return cycleStartTime;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<PlotBands> GetOperatingLimitsTemprature(string MachineID)
        {
            string comID = componentID;
            List<PlotBands> plotBandList = new List<PlotBands>();
            PlotBands plotBand = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("Select * from ProcessParametersMaster where MachineID = @MachineID and Component = @Component and Parameters = @Parameters", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Component", comID);
                cmd.Parameters.AddWithValue("@Parameters", "Temperature");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        plotBand = new PlotBands();
                        plotBand.from = Convert.ToDouble(sdr["LSL"]);
                        plotBand.to = Convert.ToDouble(sdr["USL"]);
                        plotBand.color = "#EE7157";
                        plotBandList.Add(plotBand);
                        plotBand = new PlotBands();
                        plotBand.from = Convert.ToDouble(sdr["LowerWarningZoneLimit"]);
                        plotBand.to = Convert.ToDouble(sdr["UpperWarningZoneLimit"]);
                        plotBand.color = "#E7F18B";
                        plotBandList.Add(plotBand);
                        plotBand = new PlotBands();
                        plotBand.from = Convert.ToDouble(sdr["LowerOperatingZoneLimit"]);
                        plotBand.to = Convert.ToDouble(sdr["UpperOperatingZoneLimit"]);
                        plotBand.color = "#4EE670";
                        plotBandList.Add(plotBand);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return plotBandList;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<PlotBands> GetOperatingLimitsPressure(string MachineID)
        {
            string comID = componentID;
            List<PlotBands> plotBandList = new List<PlotBands>();
            PlotBands plotBand = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("Select * from ProcessParametersMaster where MachineID = @MachineID and Component = @Component and Parameters = @Parameters", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Component", comID);
                cmd.Parameters.AddWithValue("@Parameters", "Pressure");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        plotBand = new PlotBands();
                        plotBand.from = Convert.ToDouble(sdr["LSL"]);
                        plotBand.to = Convert.ToDouble(sdr["USL"]);
                        plotBand.color = "#EE7157";
                        plotBandList.Add(plotBand);
                        plotBand = new PlotBands();
                        plotBand.from = Convert.ToDouble(sdr["LowerWarningZoneLimit"]);
                        plotBand.to = Convert.ToDouble(sdr["UpperWarningZoneLimit"]);
                        plotBand.color = "#E7F18B";
                        plotBandList.Add(plotBand);
                        plotBand = new PlotBands();
                        plotBand.from = Convert.ToDouble(sdr["LowerOperatingZoneLimit"]);
                        plotBand.to = Convert.ToDouble(sdr["UpperOperatingZoneLimit"]);
                        plotBand.color = "#4EE670";
                        plotBandList.Add(plotBand);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return plotBandList;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TempPresChartData GetTempratureChartData(Header header)
        {
            long[] Data = null;
            TempPresChartData ChartFullData = new TempPresChartData();
            List<StartEndCycleTime> CycletimeList = new List<StartEndCycleTime>();
            List<TempPresLiveChartDTO> tempPresList = new List<TempPresLiveChartDTO>();
            List<long[]> tempDataList = null;
            List<long> updateDT = new List<long>();
            List<PlotLines> plotLinesList = new List<PlotLines>();
            PlotLines plotLineData = null;
            TempPresLiveChartDTO tempPresData = null;
            StartEndCycleTime CycleTimeData = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataSet dataSet = new DataSet();
            DataTable dt;
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetTempandPressureMonitoringGraph_Metso]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", header.FromDate);
                cmd.Parameters.AddWithValue("@EndTime", header.ToDate);
                cmd.Parameters.AddWithValue("@MachineID", header.MachineId);
                cmd.Parameters.AddWithValue("@Param", "Bytime");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
                //if (!header.Param.Equals("ByCycle", StringComparison.OrdinalIgnoreCase))
                //{
                dt = dataSet.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CycleTimeData = new StartEndCycleTime();
                        CycleTimeData.MachineId = dr["machineid"].ToString();
                        if (!Convert.IsDBNull(dr["CycleStart"]) && !string.IsNullOrEmpty(dr["CycleStart"].ToString()))
                            CycleTimeData.CycleStart = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        if (!Convert.IsDBNull(dr["CycleEnd"]) && !string.IsNullOrEmpty(dr["CycleEnd"].ToString()))
                            CycleTimeData.CycleEnd = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        CycletimeList.Add(CycleTimeData);
                    }
                }

                dt = dataSet.Tables[1];
                dtTemp = dt;

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                        {
                            updateDT.Add((long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);
                            HttpContext.Current.Session["lastviewedTime"] = Convert.ToDateTime(dr["UpdatedTS"]).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    for (int i = 2; i < 8; i++)
                    {
                        tempDataList = new List<long[]>();
                        tempPresData = new TempPresLiveChartDTO();
                        tempPresData.name = dt.Columns[i].ColumnName;
                        foreach (DataRow dr in dt.Rows)
                        {
                            Data = new long[2];
                            if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                            {
                                Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                                Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                            }
                            tempDataList.Add(Data);
                        }
                        tempPresData.data = tempDataList;
                        tempPresList.Add(tempPresData);
                    }
                }

                dt = dataSet.Tables[2];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["component"].ToString()))
                        {
                            componentID = dr["component"].ToString();
                        }
                    }
                }
                //}
                //else
                //{
                //    dt = dataSet.Tables[0];
                //    dtTemp = dt;
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            CycleTimeData = new StartEndCycleTime();
                //            CycleTimeData.MachineId = dr["machineid"].ToString();
                //            if (!Convert.IsDBNull(dr["CycleStart"]) && !string.IsNullOrEmpty(dr["CycleStart"].ToString()))
                //                CycleTimeData.CycleStart = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            if (!Convert.IsDBNull(dr["CycleEnd"]) && !string.IsNullOrEmpty(dr["CycleEnd"].ToString()))
                //                CycleTimeData.CycleEnd = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            else
                //                CycleTimeData.CycleEnd = dr["UpdatedTS"] != DBNull.Value ? (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds : (long)(DateTime.Now.AddMinutes(5).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            CycletimeList.Add(CycleTimeData);
                //        }

                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                //            {
                //                updateDT.Add((long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);
                //                HttpContext.Current.Session["lastviewedTime"] = Convert.ToDateTime(dr["UpdatedTS"]).ToString("yyyy-MM-dd HH:mm:ss");
                //            }
                //        }

                //        for (int i = 3; i < 9; i++)
                //        {
                //            tempDataList = new List<long[]>();
                //            tempPresData = new TempPresLiveChartDTO();
                //            tempPresData.name = dt.Columns[i].ColumnName;
                //            foreach (DataRow dr in dt.Rows)
                //            {
                //                Data = new long[2];
                //                if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                //                {
                //                    Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //                    Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                //                }
                //                tempDataList.Add(Data);
                //            }
                //            tempPresData.data = tempDataList;
                //            tempPresList.Add(tempPresData);
                //        }
                //    }

                //    dt = dataSet.Tables[1];
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            if (!string.IsNullOrEmpty(dr["component"].ToString()))
                //            {
                //                componentID = dr["component"].ToString();
                //            }
                //        }
                //    }
                //}

                ChartFullData.CycleTimeList = CycletimeList;
                ChartFullData.updatedTimeList = updateDT;
                ChartFullData.ChartDataList = tempPresList;
                foreach (StartEndCycleTime secTime in CycletimeList)
                {
                    if (secTime.CycleStart > 100000)
                    {
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "#FF0000";
                        plotLineData.width = 2;
                        plotLineData.value = secTime.CycleStart;
                        plotLineData.zIndex = 3;
                        plotLineData.label = labelParam;
                        plotLinesList.Add(plotLineData);
                    }

                    if (secTime.CycleEnd > 100000)
                    {
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "#FF0000";
                        plotLineData.width = 2;
                        plotLineData.value = secTime.CycleEnd;
                        plotLineData.zIndex = 3;
                        plotLineData.label = labelParam;
                        plotLinesList.Add(plotLineData);
                    }
                }
                ChartFullData.plotLinesList = plotLinesList;
                tempData = ChartFullData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return ChartFullData;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TempPresChartData GetPressureChartData(Header header)
        {
            long[] Data = null;
            TempPresChartData ChartFullData = new TempPresChartData();
            List<StartEndCycleTime> CycletimeList = new List<StartEndCycleTime>();
            List<TempPresLiveChartDTO> tempPresList = new List<TempPresLiveChartDTO>();
            List<long[]> tempDataList = null;
            List<long> updateDT = new List<long>();
            List<PlotLines> plotLinesList = new List<PlotLines>();
            PlotLines plotLineData = null;
            TempPresLiveChartDTO tempPresData = null;
            StartEndCycleTime CycleTimeData = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataSet dataSet = new DataSet();
            DataTable dt;
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetTempandPressureMonitoringGraph_Metso]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", header.FromDate);
                cmd.Parameters.AddWithValue("@EndTime", header.ToDate);
                cmd.Parameters.AddWithValue("@MachineID", header.MachineId);
                cmd.Parameters.AddWithValue("@Param", "Bytime");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
                //if (!header.Param.Equals("ByCycle", StringComparison.OrdinalIgnoreCase))
                //{
                dt = dataSet.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CycleTimeData = new StartEndCycleTime();
                        CycleTimeData.MachineId = dr["machineid"].ToString();
                        if (!Convert.IsDBNull(dr["CycleStart"]) && !string.IsNullOrEmpty(dr["CycleStart"].ToString()))
                            CycleTimeData.CycleStart = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        if (!Convert.IsDBNull(dr["CycleEnd"]) && !string.IsNullOrEmpty(dr["CycleEnd"].ToString()))
                            CycleTimeData.CycleEnd = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        CycletimeList.Add(CycleTimeData);
                    }
                }

                dt = dataSet.Tables[1];
                dtPres = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                        {
                            updateDT.Add((long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);
                        }
                    }

                    for (int i = 8; i < 10; i++)
                    {
                        tempDataList = new List<long[]>();
                        tempPresData = new TempPresLiveChartDTO();
                        tempPresData.name = dt.Columns[i].ColumnName;
                        foreach (DataRow dr in dt.Rows)
                        {
                            Data = new long[2];
                            if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                            {
                                Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                                Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                            }
                            tempDataList.Add(Data);
                        }
                        tempPresData.data = tempDataList;
                        tempPresList.Add(tempPresData);
                    }
                }
                //}
                //else
                //{
                //    dt = dataSet.Tables[0];
                //    dtPres = dt;
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            CycleTimeData = new StartEndCycleTime();
                //            CycleTimeData.MachineId = dr["machineid"].ToString();
                //            if (!Convert.IsDBNull(dr["CycleStart"]) && !string.IsNullOrEmpty(dr["CycleStart"].ToString()))
                //                CycleTimeData.CycleStart = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            if (!Convert.IsDBNull(dr["CycleEnd"]) && !string.IsNullOrEmpty(dr["CycleEnd"].ToString()))
                //                CycleTimeData.CycleEnd = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            else
                //                CycleTimeData.CycleEnd = dr["UpdatedTS"] != DBNull.Value ? (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds : (long)(DateTime.Now.AddMinutes(5).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            CycletimeList.Add(CycleTimeData);
                //        }

                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                //            {
                //                updateDT.Add((long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);
                //            }
                //        }

                //        for (int i = 9; i < 11; i++)
                //        {
                //            tempDataList = new List<long[]>();
                //            tempPresData = new TempPresLiveChartDTO();
                //            tempPresData.name = dt.Columns[i].ColumnName;
                //            foreach (DataRow dr in dt.Rows)
                //            {
                //                Data = new long[2];
                //                if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                //                {
                //                    Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //                    Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                //                }
                //                tempDataList.Add(Data);
                //            }
                //            tempPresData.data = tempDataList;
                //            tempPresList.Add(tempPresData);
                //        }
                //    }
                //}
                ChartFullData.CycleTimeList = CycletimeList;
                ChartFullData.updatedTimeList = updateDT;
                ChartFullData.ChartDataList = tempPresList;
                foreach (StartEndCycleTime secTime in CycletimeList)
                {
                    if (secTime.CycleStart > 100000)
                    {
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLineData.color = "#FF0000";
                        plotLineData.width = 2;
                        plotLineData.value = secTime.CycleStart;
                        plotLineData.zIndex = 3;
                        plotLineData.label = labelParam;
                        plotLinesList.Add(plotLineData);
                    }

                    if (secTime.CycleEnd > 100000)
                    {
                        plotLineData = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = -10;
                        plotLineData.color = "#FF0000";
                        plotLineData.width = 2;
                        plotLineData.value = secTime.CycleEnd;
                        plotLineData.zIndex = 3;
                        plotLineData.label = labelParam;
                        plotLinesList.Add(plotLineData);
                    }
                }
                ChartFullData.plotLinesList = plotLinesList;
                presData = ChartFullData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return ChartFullData;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TempPresChartDataRefresh GetTempratureChartDataRefresh(Header header)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataSet dataSet = new DataSet();
            DataTable dt;
            long[] Data = null;
            TempPresChartDataRefresh ChartFullData = new TempPresChartDataRefresh();
            List<TempPresLiveChartDTORefresh> tempPresList = new List<TempPresLiveChartDTORefresh>();
            List<PlotLines> plotLinesList = new List<PlotLines>();
            PlotLines plotLine = null;
            List<long[]> tempDataList = null;
            TempPresLiveChartDTORefresh tempPresData = null;
            try
            {
                string frmDt = DateTime.Now.AddSeconds(20).ToString("yyyy-MM-dd HH:mm:ss");
                cmd = new SqlCommand("[dbo].[s_GetTempandPressureMonitoringGraph_Metso]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", string.IsNullOrEmpty(HttpContext.Current.Session["lastviewedTime"].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : HttpContext.Current.Session["lastviewedTime"].ToString());
                cmd.Parameters.AddWithValue("@EndTime", frmDt);
                cmd.Parameters.AddWithValue("@MachineID", header.MachineId);
                cmd.Parameters.AddWithValue("@Param", "Bytime");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
                //if (!header.Param.Equals("ByCycle", StringComparison.OrdinalIgnoreCase))
                //{
                dt = dataSet.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["CycleStart"] != null)
                    {
                        plotLine = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLine.color = "#FF0000";
                        plotLine.width = 2;
                        plotLine.value = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        plotLine.zIndex = 3;
                        plotLine.label = labelParam;
                        plotLinesList.Add(plotLine);
                    }

                    if (dr["CycleEnd"] != null)
                    {
                        plotLine = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = -10;
                        plotLine.color = "#FF0000";
                        plotLine.width = 2;
                        plotLine.value = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        plotLine.zIndex = 3;
                        plotLine.label = labelParam;
                        plotLinesList.Add(plotLine);
                    }
                }

                dt = dataSet.Tables[1];
                for (int i = 2; i < 8; i++)
                {
                    tempDataList = new List<long[]>();
                    tempPresData = new TempPresLiveChartDTORefresh();
                    tempPresData.name = dt.Columns[i].ColumnName;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Data = new long[2];
                        if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                        {
                            Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                        }
                        tempDataList.Add(Data);
                    }
                    tempPresData.data = tempDataList;
                    tempPresList.Add(tempPresData);
                }
                tempPresList[0].data.Last();
                //}
                //else
                //{
                //    dt = dataSet.Tables[0];
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (dr["CycleStart"] != null)
                //        {
                //            plotLine = new PlotLines();
                //            plotLineLabelParam labelParam = new plotLineLabelParam();
                //            labelParam.text = "Start";
                //            labelParam.rotation = 90;
                //            labelParam.align = "left";
                //            labelParam.y = 10;
                //            labelParam.x = 10;
                //            plotLine.color = "#FF0000";
                //            plotLine.width = 2;
                //            plotLine.value = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            plotLine.zIndex = 3;
                //            plotLine.label = labelParam;
                //            plotLinesList.Add(plotLine);
                //        }

                //        if (dr["CycleEnd"] != null)
                //        {
                //            plotLine = new PlotLines();
                //            plotLineLabelParam labelParam = new plotLineLabelParam();
                //            labelParam.text = "End";
                //            labelParam.rotation = -90;
                //            labelParam.align = "right";
                //            labelParam.y = 10;
                //            labelParam.x = -10;
                //            plotLine.color = "#FF0000";
                //            plotLine.width = 2;
                //            plotLine.value = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            plotLine.zIndex = 3;
                //            plotLine.label = labelParam;
                //            plotLinesList.Add(plotLine);
                //        }
                //    }

                //    for (int i = 3; i < 9; i++)
                //    {
                //        tempDataList = new List<long[]>();
                //        tempPresData = new TempPresLiveChartDTORefresh();
                //        tempPresData.name = dt.Columns[i].ColumnName;
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            Data = new long[2];
                //            if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                //            {
                //                Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //                Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                //            }
                //            tempDataList.Add(Data);
                //        }
                //        tempPresData.data = tempDataList;
                //        tempPresList.Add(tempPresData);
                //    }
                //}
                ChartFullData.ChartDataList = tempPresList;
                ChartFullData.plotLinesList = plotLinesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return ChartFullData;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static TempPresChartDataRefresh GetPressureChartDataRefresh(Header header)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataSet dataSet = new DataSet();
            DataTable dt;
            long[] Data = null;
            TempPresChartDataRefresh ChartFullData = new TempPresChartDataRefresh();
            List<TempPresLiveChartDTORefresh> tempPresList = new List<TempPresLiveChartDTORefresh>();
            List<PlotLines> plotLinesList = new List<PlotLines>();
            PlotLines plotLine = null;
            List<long[]> tempDataList = null;
            TempPresLiveChartDTORefresh tempPresData = null;
            try
            {
                string frmDt = DateTime.Now.AddSeconds(20).ToString("yyyy-MM-dd HH:mm:ss");
                cmd = new SqlCommand("[dbo].[s_GetTempandPressureMonitoringGraph_Metso]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", string.IsNullOrEmpty(HttpContext.Current.Session["lastviewedTime"].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : HttpContext.Current.Session["lastviewedTime"].ToString());
                cmd.Parameters.AddWithValue("@EndTime", frmDt);
                cmd.Parameters.AddWithValue("@MachineID", header.MachineId);
                cmd.Parameters.AddWithValue("@Param", "Bytime");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
                HttpContext.Current.Session["lastviewedTime"] = frmDt;
                //if (!header.Param.Equals("ByCycle", StringComparison.OrdinalIgnoreCase))
                //{
                dt = dataSet.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["CycleStart"] != null)
                    {
                        plotLine = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "Start";
                        labelParam.rotation = 90;
                        labelParam.align = "left";
                        labelParam.y = 10;
                        labelParam.x = 10;
                        plotLine.color = "#FF0000";
                        plotLine.width = 2;
                        plotLine.value = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        plotLine.zIndex = 3;
                        plotLine.label = labelParam;
                        plotLinesList.Add(plotLine);
                    }

                    if (dr["CycleEnd"] != null)
                    {
                        plotLine = new PlotLines();
                        plotLineLabelParam labelParam = new plotLineLabelParam();
                        labelParam.text = "End";
                        labelParam.rotation = -90;
                        labelParam.align = "right";
                        labelParam.y = 10;
                        labelParam.x = -10;
                        plotLine.color = "#FF0000";
                        plotLine.width = 2;
                        plotLine.value = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                        plotLine.zIndex = 3;
                        plotLine.label = labelParam;
                        plotLinesList.Add(plotLine);
                    }
                }

                dt = dataSet.Tables[1];
                for (int i = 8; i < 10; i++)
                {
                    tempDataList = new List<long[]>();
                    tempPresData = new TempPresLiveChartDTORefresh();
                    tempPresData.name = dt.Columns[i].ColumnName;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Data = new long[2];
                        if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                        {
                            HttpContext.Current.Session["lastviewedTime"] = dr["UpdatedTS"].ToString();
                            Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                        }
                        tempDataList.Add(Data);
                    }
                    tempPresData.data = tempDataList;
                    tempPresList.Add(tempPresData);
                }
                //}
                //else
                //{
                //    dt = dataSet.Tables[0];
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        if (dr["CycleStart"] != null)
                //        {
                //            plotLine = new PlotLines();
                //            plotLineLabelParam labelParam = new plotLineLabelParam();
                //            labelParam.text = "Start";
                //            labelParam.rotation = 90;
                //            labelParam.align = "left";
                //            labelParam.y = 10;
                //            labelParam.x = 10;
                //            plotLine.color = "#FF0000";
                //            plotLine.width = 2;
                //            plotLine.value = (long)(Convert.ToDateTime(dr["CycleStart"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            plotLine.zIndex = 3;
                //            plotLine.label = labelParam;
                //            plotLinesList.Add(plotLine);
                //        }

                //        if (dr["CycleEnd"] != null)
                //        {
                //            plotLine = new PlotLines();
                //            plotLineLabelParam labelParam = new plotLineLabelParam();
                //            labelParam.text = "End";
                //            labelParam.rotation = -90;
                //            labelParam.align = "right";
                //            labelParam.y = 10;
                //            labelParam.x = -10;
                //            plotLine.color = "#FF0000";
                //            plotLine.width = 2;
                //            plotLine.value = (long)(Convert.ToDateTime(dr["CycleEnd"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //            plotLine.zIndex = 3;
                //            plotLine.label = labelParam;
                //            plotLinesList.Add(plotLine);
                //        }
                //    }

                //    for (int i = 9; i < 11; i++)
                //    {
                //        tempDataList = new List<long[]>();
                //        tempPresData = new TempPresLiveChartDTORefresh();
                //        tempPresData.name = dt.Columns[i].ColumnName;
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            Data = new long[2];
                //            if (!Convert.IsDBNull(dr["UpdatedTS"]) && !string.IsNullOrEmpty(dr["UpdatedTS"].ToString()))
                //            {
                //                HttpContext.Current.Session["lastviewedTime"] = dr["UpdatedTS"].ToString();
                //                Data[0] = (long)(Convert.ToDateTime(dr["UpdatedTS"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                //                Data[1] = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(dr[tempPresData.name])));
                //            }
                //            tempDataList.Add(Data);
                //        }
                //        tempPresData.data = tempDataList;
                //        tempPresList.Add(tempPresData);
                //    }
                //}
                ChartFullData.ChartDataList = tempPresList;
                ChartFullData.plotLinesList = plotLinesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return ChartFullData;
        }

        protected void btnExportParameter_Click(object sender, EventArgs e)
        {
            if (tempData.ChartDataList == null || presData.ChartDataList == null || tempData.ChartDataList.Count < 1 || presData.ChartDataList.Count < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No data to export for given time and machine.')", true);
                return;
            }
            else
            {
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    List<string> columnNames = dtPres.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                    columnNames.Remove("UpdatedTS");
                    columnNames.Remove("Machineid");
                    columnNames.Remove("IDD");
                    columnNames.Remove("component");
                    columnNames.Remove("operation");
                    columnNames.Remove("Machineinterface");
                    ExportExcelEpplus.ParameterReportGenerate(dtTemp, txtFromDate.Text, txtToDate.Text, ddlMachineId.SelectedValue.ToString(), columnNames);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No data to export for given time and machine.')", true);
                    return;
                }
            }
        }
    }
}