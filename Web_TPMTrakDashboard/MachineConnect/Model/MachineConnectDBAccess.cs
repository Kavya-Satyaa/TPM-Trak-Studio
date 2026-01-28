using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MachineConnect.Model
{
    public class MachineConnectDBAccess
    {
        internal static SpindleChartEntity getSpindleParameterDashboardDetails(string fromDate, string toDate, string machineId, string axisNo)
        {
            SpindleChartEntity chartData = new SpindleChartEntity();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[Focas_GetSpindleDetails]", conn);
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@AxisNo", axisNo);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    List<double[]> Temperature = new List<double[]>();
                    List<double[]> SpindleSpeed = new List<double[]>();
                    List<double[]> SpindleLoad = new List<double[]>();
                    while (rdr.Read())
                    {
                        double timeInMilliSecond = (double)(Convert.ToDateTime(rdr["CNCTimeStamp"].ToString()) - new DateTime(1970, 1, 1)).TotalMilliseconds;

                        double[] data = new double[2];
                        data[0] = timeInMilliSecond;
                        data[1] = HelperClassGeneric.getDoubleValueFromString(rdr["Temperature"].ToString());
                        Temperature.Add(data);

                        data = new double[2];
                        data[0] = timeInMilliSecond;
                        data[1] = HelperClassGeneric.getDoubleValueFromString(rdr["SpindleLoad"].ToString());
                        SpindleLoad.Add(data);

                        data = new double[2];
                        data[0] = timeInMilliSecond;
                        data[1] = HelperClassGeneric.getDoubleValueFromString(rdr["SpindleSpeed"].ToString());
                        SpindleSpeed.Add(data);
                    }
                    chartData.Temperature = Temperature;
                    chartData.SpindleLoad = SpindleLoad;
                    chartData.SpindleSpeed = SpindleSpeed;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return chartData;
        }

        internal static List<ProgramUploadHistoryEntity> GetProgramUploadHistoryDetails(string machineID, string programNo, string FromDate, string ToDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = default;
            SqlDataReader rdr = default;
            List<ProgramUploadHistoryEntity> list = new List<ProgramUploadHistoryEntity>();
            try
            {
                cmd = new SqlCommand(@"select MachineID,ProgramNo,UpdatedBy,UpdatedTS from MachineProgramTransferDetails where MachineID in (select item from SplitStrings(@MachineID,',')) and (ProgramNo in (select item from SplitStrings(@ProgramNo,',')) or isnull(@ProgramNo,'')='') and UpdatedTS>=@FromDate and UpdatedTS<=@ToDate and Event='Upload' order by UpdatedTS asc", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ProgramNo", programNo);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    ProgramUploadHistoryEntity entity = new ProgramUploadHistoryEntity();
                    entity.MachineId = rdr["MachineID"].ToString();
                    entity.ProgramNo = rdr["ProgramNo"].ToString();
                    entity.Employee = rdr["UpdatedBy"].ToString();
                    entity.UpdatedTS = Util.GetDateTime(rdr["UpdatedTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss tt");
                    list.Add(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }

        internal static List<string> GetProgramForMachinedate(string machineId, string FromDate, string ToDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = default;
            SqlDataReader rdr = default;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct ProgramNo from MachineProgramTransferDetails where MachineID in (select item from SplitStrings(@MachineID,',')) and UpdatedTS>=@FromDate and UpdatedTS<=@ToDate", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["ProgramNo"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }

        internal static List<string> getAxisNumberData(string machineId)
        {
            List<string> list = new List<string>();
            string axisList = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from Focas_Defaults where [Parameter] = 'SpindleAxisList'  and ValueInText=@ValueInText", conn);
                cmd.Parameters.AddWithValue("@ValueInText", machineId);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        axisList = rdr["ValueInText2"].ToString();
                    }
                    if (axisList != "")
                    {
                        list = axisList.Split(',').ToList();
                    }
                }
                if (axisList == "")
                {
                    list.Add("Spindle");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }


        #region --- Production Analytics ----
        internal static DataTable getProductionAnalyticsData(string fromDate, string shiftVal, string plantId, string machineId, string procName)
        {
            DataTable dt = new System.Data.DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            if (machineId.Equals("All")) machineId = string.Empty;
            try
            {
                var cmd = new SqlCommand(procName, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                if (procName.Equals("s_GetFocasLiveDetailsForMultipleMac", StringComparison.OrdinalIgnoreCase))
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd"));
                    //}

                }
                else
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd"));
                    // }
                }
                cmd.Parameters.AddWithValue("@ShiftName", shiftVal);
                cmd.Parameters.AddWithValue("@plantId", plantId);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@param", "");
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        internal static HourlyRunTimeChartEntity GetProductionDataDayWise(string dateVal, string shiftVal, string plantId, string machineId, string procName, out HourlyRunTimeChartEntity sumarydata)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            DateTime tempDate = DateTime.MinValue;
            HourlyRunTimeChartEntity data = new HourlyRunTimeChartEntity();
            sumarydata = new HourlyRunTimeChartEntity();
            string tempMachine = string.Empty;
            SqlDataReader rdr = null;
            if (shiftVal.Equals("Day")) shiftVal = "";

            try
            {
                SqlCommand cmd = new SqlCommand(procName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                if (procName.Equals("s_GetFocasLiveDetailsForMultipleMac", StringComparison.OrdinalIgnoreCase))
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    //}

                }
                else
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    // }

                }

                cmd.Parameters.AddWithValue("@ShiftName", shiftVal);
                cmd.Parameters.AddWithValue("@plantId", plantId);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@param", "SummaryWeb");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        if (!Convert.IsDBNull(rdr["From Time"]))
                        {
                            var date = rdr.GetDateTime(1);
                            if (date.Minute == 0)
                            {
                                data.Date.Add(rdr.GetDateTime(1).ToString("HH") + "-" + rdr.GetDateTime(2).ToString("HH"));
                            }
                            else
                            {
                                data.Date.Add(rdr.GetDateTime(1).ToString("HH:mm") + "-" + rdr.GetDateTime(2).ToString("HH:mm"));
                            }
                        }
                        if (!Convert.IsDBNull(rdr["PowerOnTime"]))
                        {
                            data.PowerOntTime.Add(Math.Round(Convert.ToDouble(rdr["PowerOnTime"]), 2));
                        }
                        if (!Convert.IsDBNull(rdr["Operating time"]))
                        {
                            data.OperatingTime.Add(Math.Round(Convert.ToDouble(rdr["Operating time"]), 2));
                        }

                        if (!Convert.IsDBNull(rdr["Cutting time"]))
                        {
                            data.CuttingTime.Add(Math.Round(Convert.ToDouble(rdr["Cutting time"]), 2));
                        }

                        //if (!Convert.IsDBNull(rdr["partsCount"]))
                        //{
                        //    chartVals.partProgramCount.Add(Convert.ToDouble(rdr["partsCount"]));
                        //}

                    }
                    rdr.NextResult();
                    while (rdr.Read())
                    {
                        if (!Convert.IsDBNull(rdr["PowerOnTime"]))
                        {
                            sumarydata.SummaryPowerOntTime = Math.Round(Convert.ToDouble(rdr["PowerOnTime"]), 2);
                        }

                        if (!Convert.IsDBNull(rdr["Cutting Time"]))
                        {
                            sumarydata.SummaryCuttingTime = Math.Round(Convert.ToDouble(rdr["Cutting Time"]), 2);
                        }

                        if (!Convert.IsDBNull(rdr["TotalTime"]))
                        {
                            sumarydata.SummaryTotalTime = Math.Round(Convert.ToDouble(rdr["TotalTime"]), 2);
                        }

                        if (!Convert.IsDBNull(rdr["Operating Time"]))
                        {
                            sumarydata.SummaryOperatingTime = Math.Round(Convert.ToDouble(rdr["Operating Time"]), 2);
                        }
                        if (!Convert.IsDBNull(rdr["OperatingWithoutCutting"]))
                        {
                            sumarydata.SummaryOperatingWithoutCutting = Math.Round(Convert.ToDouble(rdr["OperatingWithoutCutting"]), 2);
                        }
                        if (!Convert.IsDBNull(rdr["NonOperatingTime"]))
                        {
                            sumarydata.SummaryNonOperatingTime = Math.Round(Convert.ToDouble(rdr["NonOperatingTime"]), 2);
                        }
                        if (!Convert.IsDBNull(rdr["PowerOffTime"]))
                        {
                            sumarydata.SummaryPowerOffTime = Math.Round(Convert.ToDouble(rdr["PowerOffTime"]), 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (rdr != null) if (con != null) con.Close();
            }
            return data;
        }
        internal static List<PARunChartEntity> GetRuntimeDowntimeData(string dateVal, string shiftVal, string plantId, string machineId, string param, string procName)
        {
            List<PARunChartEntity> datalist = new List<PARunChartEntity>();
            PARunChartEntity data = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            if (machineId.Equals("All")) machineId = string.Empty;

            DateTime startTime = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(dateVal));
            DateTime endTime = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(dateVal));
            if (!shiftVal.Equals("Day", StringComparison.OrdinalIgnoreCase) && shiftVal != "")
            {
                var shift = CockpitDataBaseAccess.GetShiftTime(shiftVal, dateVal);
                startTime = Util.GetDateTime(shift[0]);
                endTime = Util.GetDateTime(shift[1]);
            }
            try
            {
                var cmd = new SqlCommand(procName, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (procName.Equals("s_GetFocasLiveDetailsForMultipleMac", StringComparison.OrdinalIgnoreCase))
                {
                    //if (shiftVal == "Day" || shiftVal == "")
                    //{
                    //    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    //cmd.Parameters.AddWithValue("@date", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    //}
                    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(dateVal)).ToString("yyyy-MM-dd HH:mm:ss"));

                }
                else
                {
                    //if (shiftVal == "Day" || shiftVal == "")
                    //{
                    //    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    //cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    //}
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(dateVal)).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                cmd.Parameters.AddWithValue("@ShiftName", shiftVal);
                cmd.Parameters.AddWithValue("@plantId", plantId);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@param", param);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    int i = 0;
                    DateTime lastDateTime = new DateTime();
                    while (rdr.Read())
                    {
                        data = new PARunChartEntity();
                        string statusReason = Convert.ToString(rdr["Reason"]);
                        string startdate = rdr["Batchstart"].ToString();
                        string enddate = rdr["BatchEnd"].ToString();
                        data.StartDate = startdate;
                        data.EndDate = enddate;
                        DateTime startdatetime = Util.GetDateTime(startdate);
                        DateTime enddatetime = Util.GetDateTime(enddate);
                        lastDateTime = enddatetime;
                        data.x = (double)(startdatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        data.x2 = (double)(enddatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                        if (statusReason.Equals("Down"))
                        {

                            data.color = "#ff0000";
                            data.name = "Down";
                        }
                        else if (statusReason.Equals("Prod"))
                        {

                            data.color = "#00ff00";
                            data.name = "Prod";
                        }
                        else if (statusReason.Equals("NO_DATA"))
                        {
                            data.color = "black";
                            data.name = "NoData";
                        }
                        //data.alarmno = rdr["LiveAlarmsNo"].ToString();
                        if (i == 0)
                        {
                            if (startdatetime > startTime)
                            {
                                PARunChartEntity extraData = new PARunChartEntity();
                                extraData.StartDate = startTime.ToString();
                                extraData.EndDate = startdatetime.ToString();
                                extraData.x = (double)(startTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                                extraData.x2 = (double)(startdatetime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                                extraData.color = "black";
                                extraData.name = "NoData";
                                datalist.Add(extraData);
                            }
                        }
                        datalist.Add(data);
                        i++;
                    }
                    if (datalist.Count > 0)
                    {
                        if (DateTime.Now < endTime)
                        {
                            endTime = DateTime.Now;
                            datalist[0].EnabledTickIneterval = false;
                        }
                        if (lastDateTime < endTime)
                        {
                            PARunChartEntity extraData = new PARunChartEntity();
                            extraData.StartDate = lastDateTime.ToString();
                            extraData.EndDate = endTime.ToString();
                            extraData.x = (double)(lastDateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            extraData.x2 = (double)(endTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                            extraData.color = "black";
                            extraData.name = "NoData";
                            datalist.Add(extraData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

            finally
            {
                if (rdr != null) if (con != null) con.Close();
            }
            return datalist;
        }
        internal static DataTable GetStopagedata(string dateVal, string shiftVal, string plantId, string machineId, string procName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            if (machineId.Equals("All")) machineId = string.Empty;
            try
            {
                var cmd = new SqlCommand(procName, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                if (procName.Equals("s_GetFocasLiveDetailsForMultipleMac", StringComparison.OrdinalIgnoreCase))
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@date", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    //}
                }
                else
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    //}
                }

                cmd.Parameters.AddWithValue("@ShiftName", shiftVal);
                cmd.Parameters.AddWithValue("@plantId", plantId);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@param", "stoppages");
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

            finally
            {
                if (rdr != null) if (con != null) con.Close();
            }
            return dt;

        }
        internal static string GetDefaultThreshold()
        {
            string downtimeThreshold = string.Empty;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {

                SqlCommand cmd = new SqlCommand(@"select * from Focas_Defaults where Parameter = 'DowntimeThreshold'", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    downtimeThreshold = (rdr["ValueInText"].ToString());
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDefaultThreshold: " + ex.Message);

            }
            finally
            {
                if (con != null) con.Close();
            }
            return downtimeThreshold;
        }
        internal static PartCountChartEntity GetPartsCountData(string dateVal, string shiftVal, string plantId, string machineId, string procName)
        {
            DataTable dt = new DataTable();
            PartCountChartEntity partcountdata = new PartCountChartEntity();
            SqlConnection con = ConnectionManager.GetConnection();
            string currentDate = "";
            try
            {
                SqlCommand cmd = new SqlCommand(procName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                if (procName.Equals("s_GetFocasHourShiftwiseLiveDetails", StringComparison.OrdinalIgnoreCase))
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@Starttime", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //    cmd.Parameters.AddWithValue("@Endtime", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@Starttime", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Endtime", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    //}

                }
                else
                {
                    //if (shiftVal == "" || shiftVal == "Day")
                    //{
                    //    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //    cmd.Parameters.AddWithValue("@Enddate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //}
                    //else
                    //{
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Enddate", Util.GetDateTime(dateVal).ToString("yyyy-MM-dd"));
                    //}
                }
                cmd.Parameters.AddWithValue("@Shiftname", shiftVal);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@Machineid", machineId);
                cmd.Parameters.AddWithValue("@Param", procName.Equals("s_GetAggFocasDetailsForMultipleMac", StringComparison.OrdinalIgnoreCase) ? "PartsCount" : "hour");
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                    List<string> distinctprogramno = (from r in dt.AsEnumerable() select r["ProgramNo"].ToString()).Distinct().ToList();
                    distinctprogramno.Remove("");
                    foreach (DataRow row in dt.Rows)
                    {
                        var date = DateTime.Parse(row["From Time"].ToString());
                        if (date.Minute == 0)
                        {
                            currentDate = Convert.ToString(DateTime.Parse(row["From Time"].ToString()).ToString("HH") + "-" + DateTime.Parse(row["To Time"].ToString()).ToString("HH"));
                        }
                        else
                        {
                            currentDate = Convert.ToString(DateTime.Parse(row["From Time"].ToString()).ToString("HH:mm") + "-" + DateTime.Parse(row["To Time"].ToString()).ToString("HH:mm"));
                        }
                        if (partcountdata.XAxisData.Contains(currentDate))
                        {

                        }
                        else
                        {
                            partcountdata.XAxisData.Add(currentDate);
                        }

                    }

                    List<PartCountSeriesDataEntity> allseriesdata = new List<PartCountSeriesDataEntity>();
                    PartCountSeriesDataEntity seriesdata = null;
                    foreach (string prgmno in distinctprogramno)
                    {
                        DataTable dtcopy = new DataTable();
                        dtcopy = dt.AsEnumerable().Where(x => x.Field<string>("ProgramNo") == prgmno).CopyToDataTable();
                        seriesdata = new PartCountSeriesDataEntity();
                        seriesdata.name = prgmno;

                        foreach (string xdate in partcountdata.XAxisData)
                        {
                            bool isXValueExists = false;
                            double partcount = 0;
                            foreach (DataRow row in dtcopy.Rows)
                            {
                                var date = DateTime.Parse(row["From Time"].ToString());
                                if (date.Minute == 0)
                                {
                                    currentDate = Convert.ToString(DateTime.Parse(row["From Time"].ToString()).ToString("HH") + "-" + DateTime.Parse(row["To Time"].ToString()).ToString("HH"));
                                }
                                else
                                {
                                    currentDate = Convert.ToString(DateTime.Parse(row["From Time"].ToString()).ToString("HH:mm") + "-" + DateTime.Parse(row["To Time"].ToString()).ToString("HH:mm"));
                                }
                                if (xdate.Equals(currentDate, StringComparison.OrdinalIgnoreCase))
                                {
                                    //seriesdata.data.Add(Convert.ToDouble(row["PartsCount"].ToString()));
                                    partcount = Convert.ToDouble(row["PartsCount"].ToString());
                                    isXValueExists = true;
                                    break;
                                }
                            }
                            if (isXValueExists)
                            {
                                seriesdata.data.Add(partcount);
                            }
                            else
                            {
                                seriesdata.data.Add(Convert.ToDouble(null));
                            }
                        }
                        allseriesdata.Add(seriesdata);
                    }
                    partcountdata.partCountSeriesDatas = allseriesdata;
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPartsCountData: " + ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
            }
            return partcountdata;
        }
        #endregion

        #region ---- Wear Offset History ----
        internal static List<string> getMachinesOfDNCEnabled()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select MachineId from [MachineInformation] where (DNCTransferEnabled=1)", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["MachineId"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);

            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static string getMachineType(string machineid)
        {
            string machineType = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select MachineType from machineinformation where machineid = @mc", con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@mc", machineid);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        machineType = sdr["MachineType"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return machineType;
        }
        internal static int getOffsetRange(string machineType, string offsetType)
        {
            int offsetRange = 0;
            int startAddress = 0;
            int endAddress = 0;
            SqlDataReader sdr = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                if (machineType.Equals("Turning", StringComparison.OrdinalIgnoreCase))
                    sqlQuery = "select StartAddress, EndAddress from OffsetParameterAddressMaster where MachineType = @machineType and OffsetType = @offsetType";
                else
                    sqlQuery = "select StartAddress, EndAddress from OffsetParameterAddressMaster where MachineType = @machineType";
                cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineType", machineType);
                cmd.Parameters.AddWithValue("@offsetType", offsetType);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["StartAddress"]) && !Convert.IsDBNull(sdr["EndAddress"]))
                        {
                            if (int.TryParse(sdr["StartAddress"].ToString(), out startAddress) && int.TryParse(sdr["EndAddress"].ToString(), out endAddress))
                            {
                                offsetRange = (endAddress - startAddress) > offsetRange ? endAddress - startAddress : offsetRange;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return offsetRange;
        }
        internal static List<WearOffsetEntity> getWearOffsetHistoryDetails(string fromDate, string toDate, string machineId, string wearOffset, string offsetType, string param1, string param, bool enableChart, out WearOffsetChartEntity chartData)
        {
            chartData = new WearOffsetChartEntity();
            List<WearOffsetEntity> list = new List<WearOffsetEntity>();
            SqlDataReader sdr = null;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_FocasToolOffsetHistorySaveAndView]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Offsetno", wearOffset);
                cmd.Parameters.AddWithValue("@OffsetType", offsetType.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : offsetType);
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@Param1", param1);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    List<double[]> leftChartDataList = new List<double[]>();
                    List<double[]> rightChartDataList = new List<double[]>();
                    while (sdr.Read())
                    {
                        WearOffsetEntity data = new WearOffsetEntity();
                        data.Timestamp = sdr["MachineTimeStamp"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["MachineTimeStamp"].ToString()).ToString("dd-MM-yyyy HH:mm");
                        data.OffsetNo = sdr["OffsetNo"].ToString();
                        data.MachineMode = sdr["MachineMode"].ToString();
                        data.ProgramNo = sdr["ProgramNumber"].ToString();
                        data.OffsetX = sdr["WearOffsetX"].ToString();
                        data.OffsetZ = sdr["WearOffsetZ"].ToString();
                        data.OffsetR = sdr["WearOffsetR"].ToString();
                        data.OffsetT = sdr["WearOffsetT"].ToString();
                        list.Add(data);

                        if (enableChart)
                        {
                            double[] chartValue = new double[2];
                            DateTime dtime = Util.GetDateTime(data.Timestamp);
                            chartValue[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            chartValue[1] = HelperClassGeneric.getDoubleValueFromString(data.OffsetX);
                            leftChartDataList.Add(chartValue);

                            chartValue = new double[2];
                            chartValue[0] = (double)(dtime - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            chartValue[1] = HelperClassGeneric.getDoubleValueFromString(data.OffsetZ);
                            rightChartDataList.Add(chartValue);
                        }
                    }
                    chartData.LeftChartData = leftChartDataList;
                    chartData.RightChartData = rightChartDataList;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        #endregion

        #region ----- Export -----
        public static List<ProductionData> GetProductionData(string fromDate, string toDate, string shifts, string plantId, string machineIds, string param, out ProductionAnalysisForExcelSummary Summaryresult,
            out List<string> cumProgramNo, out List<string> cumPartsCount, out List<string> cumOEE)
        {
            SqlConnection _sqlConn = ConnectionManager.GetConnection();
            List<ProductionData> prodDatas = new List<ProductionData>();
            cumProgramNo = new List<string>();
            cumPartsCount = new List<string>();
            cumOEE = new List<string>();
            int indexVal = 0;

            double sumPowerOnTime = 0;
            double sumCuttingTime = 0;
            double sumOperationTime = 0;
            double sumPartsCount = 0;
            double AvgOEE = 0;
            int i = 0;
            string prevShift = string.Empty;
            string prevDate = string.Empty;
            string prevMachine = string.Empty;
            string prevFromTime = string.Empty;
            bool isDuplicatePresent = false;

            bool firstRow = false;
            DateTime tempDate = DateTime.MinValue;
            string tempMachine = string.Empty;
            SqlDataReader sdr = null;
            Summaryresult = new ProductionAnalysisForExcelSummary();
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetFocasShiftwiseLiveDetails", _sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Starttime", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.AddWithValue("@Endtime", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.AddWithValue("@Shiftname", SqlDbType.NVarChar).Value = shifts;
                cmd.Parameters.AddWithValue("@PlantID", SqlDbType.NVarChar).Value = plantId;
                cmd.Parameters.AddWithValue("@Machineid", SqlDbType.NVarChar).Value = machineIds;
                cmd.Parameters.AddWithValue("@Param", SqlDbType.NVarChar).Value = param;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (tempMachine != Convert.ToString(sdr["MachineID"]) && firstRow)
                        {
                            ProductionData pd1 = new ProductionData();
                            pd1.Shift = "Total";
                            pd1.MachineID = Convert.ToString(tempMachine);
                            pd1.PowerOnTime = Math.Round(sumPowerOnTime, 2);
                            pd1.CuttingTime = Math.Round(sumCuttingTime, 2);
                            pd1.OperationTime = Math.Round(sumOperationTime, 2);
                            pd1.PartsCount = Math.Round(sumPartsCount, 2);
                            if (AvgOEE > 0)
                            {
                                pd1.OEE = Math.Round(AvgOEE / i, 2);
                            }
                            else
                            {
                                pd1.OEE = 0;
                            }
                            // pd1.OEE = Math.Round(AvgOEE, 2);
                            sumPowerOnTime = 0;
                            sumCuttingTime = 0;
                            sumOperationTime = 0;
                            sumPartsCount = 0;
                            AvgOEE = 0;
                            i = 0;
                            prodDatas.Add(pd1);
                        }
                        i++;
                        firstRow = true;
                        ProductionData pd = new ProductionData();
                        pd.MachineID = Convert.ToString(sdr["MachineID"]);
                        if (!Convert.IsDBNull(sdr["MachineID"]))
                        {
                            pd.MachineID = Convert.ToString(sdr["MachineID"]);
                            tempMachine = Convert.ToString(sdr["MachineID"]);
                        }

                        if (param == "Shift" || param == "Hour")
                        {
                            pd.Shift = Convert.ToString(sdr["ShiftName"]);
                        }
                        if (param == "Hour")
                        {
                            pd.FromTime = sdr["From Time"].ToString();
                            pd.ToTime = sdr["To Time"].ToString();
                        }

                        if (param == "Day")
                        {
                            pd.FromTime = string.Format(sdr["From Time"].ToString(), "yyyy-MM-dd");
                        }

                        if (param == "Shift" || param == "Hour")
                        {
                            if (!Convert.IsDBNull(sdr["ShiftDate"]))
                            {
                                tempDate = Convert.ToDateTime(sdr["ShiftDate"]);
                                pd.Date = sdr.GetDateTime(sdr.GetOrdinal("ShiftDate")).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        if (!Convert.IsDBNull(sdr["PowerOnTime"]))
                        {
                            pd.PowerOnTime = Math.Round(Convert.ToDouble(sdr["PowerOnTime"]), 2);
                            if (param == "Shift")
                            {
                                if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                                    sumPowerOnTime = sumPowerOnTime + Convert.ToDouble(sdr["PowerOnTime"]);
                            }
                            else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                            {

                                sumPowerOnTime = sumPowerOnTime + Convert.ToDouble(sdr["PowerOnTime"]);
                            }
                        }


                        if (!Convert.IsDBNull(sdr["Cutting time"]))
                        {
                            pd.CuttingTime = Math.Round(Convert.ToDouble(sdr["Cutting time"]), 2);
                            if (param == "Shift")
                            {
                                if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                                    sumCuttingTime = sumCuttingTime + Convert.ToDouble(sdr["Cutting time"]);
                            }
                            else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                            {
                                sumCuttingTime = sumCuttingTime + Convert.ToDouble(sdr["Cutting time"]);
                            }
                        }
                        if (!Convert.IsDBNull(sdr["Operating time"]))
                        {
                            pd.OperationTime = Math.Round(Convert.ToDouble(sdr["Operating time"]), 2);
                            if (param == "Shift")
                            {
                                if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                                    sumOperationTime = sumOperationTime + Convert.ToDouble(sdr["Operating time"]);
                            }
                            else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                            {
                                sumOperationTime = sumOperationTime + Convert.ToDouble(sdr["Operating time"]);
                            }
                        }

                        if (param == "Shift" || param == "Hour")
                        {
                            prevShift = Convert.ToString(sdr["ShiftName"]);
                            prevDate = Convert.ToString(sdr["ShiftDate"]);
                        }
                        if (param == "Day" || param == "Hour")
                        {
                            prevFromTime = Convert.ToString(sdr["From Time"]);
                        }
                        prevMachine = Convert.ToString(sdr["MachineID"]);

                        // Added
                        if (!Convert.IsDBNull(sdr["ProgramNo"]))
                        {
                            pd.ProgramNoVal = Convert.ToString(sdr["ProgramNo"]);
                            //Added
                            if (pd.ProgramNoVal != "0")
                            {
                                if (cumProgramNo.Contains(pd.ProgramNoVal))
                                {
                                    indexVal = (int)cumProgramNo.IndexOf(Convert.ToString(sdr["ProgramNo"]));
                                    isDuplicatePresent = true;
                                }
                                else
                                {
                                    cumProgramNo.Add(Convert.ToString(sdr["ProgramNo"]));
                                }
                            }

                        }

                        if (!Convert.IsDBNull(sdr["PartsCount"]))
                        {
                            pd.PartsCount = Convert.ToDouble(sdr["PartsCount"]);
                            sumPartsCount = sumPartsCount + Convert.ToDouble(sdr["PartsCount"]);
                            if (pd.ProgramNoVal != "0")
                            {
                                if (isDuplicatePresent)
                                {
                                    int indexItem = Convert.ToInt32(cumPartsCount[indexVal]);
                                    // int newVal = (Convert.ToInt32(sdr["PartsCount"]));
                                    var res = indexItem + Math.Abs((Convert.ToInt32(sdr["PartsCount"])));
                                    cumPartsCount[indexVal] = Convert.ToString(res);
                                    isDuplicatePresent = false;
                                }
                                else
                                {
                                    cumPartsCount.Add(Convert.ToString(sdr["PartsCount"]));
                                }
                            }
                        }
                        //if (!Convert.IsDBNull(sdr["OEE"]))
                        //{
                        //    pd.OEE = Math.Round(Convert.ToDouble(sdr["OEE"]), 2);
                        //    if (param == "Shift")
                        //    {
                        //        if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                        //            AvgOEE = AvgOEE + Convert.ToDouble(sdr["OEE"]);
                        //    }
                        //    else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                        //    {

                        //        AvgOEE = AvgOEE + Convert.ToDouble(sdr["OEE"]);
                        //    }
                        //}

                        if (!Convert.IsDBNull(sdr["OEE"]))
                        {
                            pd.OEE = Convert.ToDouble(sdr["OEE"]);
                            AvgOEE = AvgOEE + Convert.ToDouble(sdr["OEE"]);
                            if (pd.ProgramNoVal != "0")
                            {
                                if (isDuplicatePresent)
                                {
                                    int indexItem = Convert.ToInt32(cumOEE[indexVal]);
                                    // int newVal = (Convert.ToInt32(sdr["PartsCount"]));
                                    var res = indexItem + Math.Abs((Convert.ToInt32(sdr["OEE"])));
                                    cumOEE[indexVal] = Convert.ToString(res);
                                    isDuplicatePresent = false;
                                }
                                else
                                {
                                    cumOEE.Add(Convert.ToString(sdr["OEE"]));
                                }
                            }

                        }


                        pd.ProgramComment = sdr["PartNumber"].ToString();

                        //    pd.OEE = Convert.ToDouble(sdr["OEE"].ToString());

                        prodDatas.Add(pd);
                    }

                    #region To insert summission of last Date
                    ProductionData pd2 = new ProductionData();
                    //if (param.Equals("Day")) pd2.Date = "Total";
                    pd2.Shift = "Total";
                    //pd2.Date = Convert.ToString(tempDate);
                    pd2.MachineID = Convert.ToString(tempMachine);
                    pd2.PowerOnTime = Math.Round(sumPowerOnTime, 2);
                    pd2.CuttingTime = Math.Round(sumCuttingTime, 2);
                    pd2.OperationTime = Math.Round(sumOperationTime, 2);
                    pd2.PartsCount = Math.Round(sumPartsCount, 2);
                    if (AvgOEE > 0)
                    {
                        pd2.OEE = Math.Round(AvgOEE / i, 2);
                    }
                    else
                    {
                        pd2.OEE = 0;
                    }
                    sumPowerOnTime = 0;
                    sumCuttingTime = 0;
                    sumOperationTime = 0;
                    sumPartsCount = 0;
                    AvgOEE = 0;
                    i = 0;
                    prodDatas.Add(pd2);
                    #endregion
                }

                sdr.NextResult();

                while (sdr.Read())
                {
                    if (!Convert.IsDBNull(sdr["PowerOnTime"]))
                    {
                        Summaryresult.PowerOnTime = Convert.ToDouble(sdr["PowerOnTime"]);
                    }

                    if (!Convert.IsDBNull(sdr["Cutting Time"]))
                    {
                        Summaryresult.CuttingTime = Convert.ToDouble(sdr["Cutting Time"]);
                    }

                    if (!Convert.IsDBNull(sdr["TotalTime"]))
                    {
                        Summaryresult.TotalTime = Convert.ToDouble(sdr["TotalTime"]);
                    }

                    if (!Convert.IsDBNull(sdr["OperatingWithoutCutting"]))
                    {
                        Summaryresult.OperatingWithoutCutting = Convert.ToDouble(sdr["OperatingWithoutCutting"]);
                    }

                    if (!Convert.IsDBNull(sdr["NonOperatingTime"]))
                    {
                        Summaryresult.NonOperatingTime = Convert.ToDouble(sdr["NonOperatingTime"]);
                    }
                    if (!Convert.IsDBNull(sdr["Operating Time"]))
                    {
                        Summaryresult.OperatingTime = Convert.ToDouble(sdr["Operating Time"]);
                    }
                    if (!Convert.IsDBNull(sdr["PowerOffTime"]))
                    {
                        Summaryresult.PowerOffTime = Convert.ToDouble(sdr["PowerOffTime"]);
                    }

                    if (!Convert.IsDBNull(sdr["PartsCount"]))
                    {
                        Summaryresult.PartsCount = Math.Round(Convert.ToDouble(sdr["PartsCount"]), 2);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (_sqlConn != null) _sqlConn.Close();

            }
            return prodDatas;
        }

        internal static List<StoppageDataEntity> GetStoppageData(string fromDate, string toDate, string shiftIds, string selectedPlant, string machineids, string parameter, string param1)
        {
            SqlConnection _sqlConn = ConnectionManager.GetConnection();
            List<StoppageDataEntity> StoppageDatas = new List<StoppageDataEntity>();
            double sumDailyStoppageTime = 0;
            double sumShiftStoppageTime = 0;
            double sumStoppageTime = 0;
            bool firstRow = false;
            DateTime tempDate = DateTime.MinValue;
            string tempMachine = string.Empty;
            string tempDateVal = string.Empty;
            string tempShiftVal = string.Empty;
            SqlDataReader sdr = null;

            try
            {
                SqlCommand cmd = new SqlCommand("[s_GetFocasShiftwiseLiveDetails]", _sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Starttime", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.AddWithValue("@Endtime", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.AddWithValue("@Shiftname", SqlDbType.NVarChar).Value = shiftIds;
                cmd.Parameters.AddWithValue("@PlantID", SqlDbType.NVarChar).Value = selectedPlant;
                cmd.Parameters.AddWithValue("@Machineid", SqlDbType.NVarChar).Value = machineids;
                cmd.Parameters.AddWithValue("@Param", SqlDbType.NVarChar).Value = parameter;
                cmd.Parameters.AddWithValue("@param1", SqlDbType.NVarChar).Value = param1;

                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (param1.Equals("Hour"))
                        {
                            if (tempMachine != Convert.ToString(sdr["MachineID"]) && firstRow)
                            {
                                StoppageDataEntity pd1 = new StoppageDataEntity();
                                pd1.MachineID = "Total";
                                pd1.MachineID = Convert.ToString(tempMachine);
                                pd1.StoppagetimeInInt = Math.Round(sumStoppageTime, 2);
                                sumStoppageTime = 0;

                                StoppageDatas.Add(pd1);
                            }
                        }

                        if (param1.Equals("DAY"))
                        {
                            if ((tempDateVal != Convert.ToString(sdr["From Time"]) && tempMachine == Convert.ToString(sdr["MachineID"]) && firstRow) ||
                                (tempDateVal != Convert.ToString(sdr["From Time"]) && tempMachine != Convert.ToString(sdr["MachineID"]) && firstRow))
                            {
                                StoppageDataEntity pdDaywise = new StoppageDataEntity();
                                pdDaywise.MachineID = "Total";
                                pdDaywise.MachineID = Convert.ToString(tempMachine);
                                pdDaywise.StoppagetimeInInt = Math.Round(sumDailyStoppageTime, 2);
                                sumDailyStoppageTime = 0;
                                StoppageDatas.Add(pdDaywise);
                            }
                        }

                        if (param1.Equals("Shift"))
                        {

                            //if (((tempShiftVal != Convert.ToString(sdr["ShiftName"]) &&
                            //    tempDateVal == Convert.ToString(sdr["From Time"])) || (tempShiftVal != Convert.ToString(sdr["ShiftName"])
                            //    && tempDateVal != Convert.ToString(sdr["From Time"]) && tempMachine != Convert.ToString(sdr["MachineID"])) || (tempShiftVal == Convert.ToString(sdr["ShiftName"])
                            //    && tempDateVal != Convert.ToString(sdr["From Time"])))
                            //    && tempMachine == Convert.ToString(sdr["MachineID"]) ||

                            if ((tempMachine == Convert.ToString(sdr["MachineID"]) && tempDateVal != Convert.ToString(sdr["From Time"]) && tempShiftVal == Convert.ToString(sdr["ShiftName"])) ||
                                (tempMachine == Convert.ToString(sdr["MachineID"]) && tempDateVal != Convert.ToString(sdr["From Time"]) && tempShiftVal != Convert.ToString(sdr["ShiftName"])) ||
                                (tempMachine == Convert.ToString(sdr["MachineID"]) && tempDateVal == Convert.ToString(sdr["From Time"]) && tempShiftVal != Convert.ToString(sdr["ShiftName"])) ||
                                (tempMachine == Convert.ToString(sdr["MachineID"]) && tempDateVal == Convert.ToString(sdr["From Time"]) && tempShiftVal != Convert.ToString(sdr["ShiftName"])) ||
                                (tempMachine != Convert.ToString(sdr["MachineID"]) && tempShiftVal != Convert.ToString(sdr["ShiftName"]) && tempDateVal == Convert.ToString(sdr["From Time"])) ||
                                (tempMachine != Convert.ToString(sdr["MachineID"]) && tempShiftVal == Convert.ToString(sdr["ShiftName"]) && tempDateVal != Convert.ToString(sdr["From Time"])))
                            {
                                StoppageDataEntity pdShiftwise = new StoppageDataEntity();
                                pdShiftwise.MachineID = "Total";
                                pdShiftwise.MachineID = Convert.ToString(tempMachine);
                                pdShiftwise.StoppagetimeInInt = Math.Round(sumShiftStoppageTime, 2);
                                sumShiftStoppageTime = 0;
                                StoppageDatas.Add(pdShiftwise);
                            }
                        }

                        firstRow = true;
                        StoppageDataEntity pd = new StoppageDataEntity();
                        pd.MachineID = Convert.ToString(sdr["MachineID"]);
                        if (!Convert.IsDBNull(sdr["MachineID"]))
                        {
                            pd.MachineID = Convert.ToString(sdr["MachineID"]);
                            tempMachine = Convert.ToString(sdr["MachineID"]);
                        }
                        pd.Shift = sdr["ShiftName"].ToString();
                        if (!Convert.IsDBNull(sdr["ShiftName"]))
                        {
                            pd.Shift = sdr["ShiftName"].ToString();
                            tempShiftVal = sdr["ShiftName"].ToString();
                        }

                        pd.BatchStart = sdr["Batchstart"].ToString();
                        pd.BatchEnd = sdr["BatchEnd"].ToString();
                        if (param1.Equals("Hour"))
                        {
                            pd.Date = sdr["Date"].ToString();
                            pd.FromTime = sdr["From Time"].ToString();
                        }
                        else
                        {
                            string aa = Convert.ToDateTime(sdr["From Time"]).ToString("dd-MMM-yyyy");
                            pd.FromTime = aa;
                        }

                        if (!Convert.IsDBNull(sdr["From Time"].ToString()))
                        {

                            tempDateVal = Convert.ToString(sdr["From Time"]);
                        }

                        pd.ToTime = sdr["To Time"].ToString();
                        if (!Convert.IsDBNull(sdr["StoppagetimeInSec"]))
                        {
                            pd.StoppagetimeInInt = Math.Round(Convert.ToDouble(sdr["StoppagetimeInSec"]), 2);

                            sumStoppageTime = sumStoppageTime + Convert.ToDouble(sdr["StoppagetimeInSec"]);

                            sumDailyStoppageTime = sumDailyStoppageTime + Convert.ToDouble(sdr["StoppagetimeInSec"]);
                            sumShiftStoppageTime = sumShiftStoppageTime + Convert.ToDouble(sdr["StoppagetimeInSec"]);
                        }
                        StoppageDatas.Add(pd);
                    }

                    #region To insert summission of last Date
                    StoppageDataEntity pd2 = new StoppageDataEntity();

                    pd2.MachineID = "Total";
                    if (param1.Equals("Hour"))
                    {
                        pd2.StoppagetimeInInt = Math.Round(sumStoppageTime, 2);
                    }
                    if (param1.Equals("Shift"))
                    {
                        pd2.StoppagetimeInInt = Math.Round(sumShiftStoppageTime, 2);
                    }
                    if (param1.Equals("DAY"))
                    {
                        pd2.StoppagetimeInInt = Math.Round(sumDailyStoppageTime, 2);
                    }
                    pd2.MachineID = Convert.ToString(tempMachine);
                    StoppageDatas.Add(pd2);
                    #endregion

                }


            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (_sqlConn != null) _sqlConn.Close();

            }
            return StoppageDatas;
        }

        #endregion

        internal static List<string> GetAllMachines(string plantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            try
            {
                cmd = new SqlCommand(@"s_GetLookups_Focas", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"name", "Machine");
                cmd.Parameters.AddWithValue(@"filter", plantId.Equals("All", StringComparison.OrdinalIgnoreCase)?"":plantId);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        machineList.Add(rdr["machineId"].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return machineList;
        }
    }
}