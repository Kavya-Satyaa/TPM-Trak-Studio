using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Vibration
{
    public class VibrationDataBaseAccess
    {
        #region ---Vibration Master Page---

        internal static List<VibrationSettingsData> GetVibrationSettingsData(List<string> machineidList, List<ListItem> parameterList,string machine,string component,string parameter)
        {
            List<VibrationSettingsData> DataList = new List<VibrationSettingsData>();

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    cmd = new SqlCommand(@"select * from TAFE_PARWANOO_VibrationMasterTable where (MachineID = @MachineID or ISNULL(@MachineID, '') = '') and (ComponentID = @ComponentID or ISNULL(@ComponentID, '') = '') 
and(ParameterID = @ParameterID or ISNULL(@ParameterID, '') = '')  order by MachineID, ComponentID, Operation, ParameterID", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"select * from TAFE_PARWANOO_VibrationMasterTable where (MachineID = @MachineID or ISNULL(@MachineID, '') = '') and (ComponentID = @ComponentID or ISNULL(@ComponentID, '') = '')  order by MachineID, ComponentID, Operation", conn);
                }
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@ComponentID", component);
                cmd.Parameters.AddWithValue("@ParameterID", parameter);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        VibrationSettingsData Data = new VibrationSettingsData();
                        Data.MachineID = rdr["MachineID"].ToString();
                        Data.MachineIDlblVisible = true;
                        Data.MachineIDddlVisible = false;
                        Data.CompID = rdr["ComponentID"].ToString();
                        Data.cmbIDlblVisible = true;
                        Data.OperationID = rdr["Operation"].ToString();
                        Data.lblOpVisible = true;
                        Data.WarningUSL = rdr["UpperWarningLimit"].ToString();
                        Data.DangerUSl = rdr["UpperErrorLimit"].ToString();
                        Data.MachineIDList = machineidList;
                        Data.MValue = rdr["Total_M_Observation"].ToString().Equals("0") ? "" : rdr["Total_M_Observation"].ToString();
                        Data.NValue = rdr["ApplyRuleFor_N_Observation"].ToString().Equals("0") ? "" : rdr["ApplyRuleFor_N_Observation"].ToString();
                        Data.ParameterlblVisible = true;
                        Data.ParameterddlVisible = false;
                        if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                        {
                            Data.ParameterValue = rdr["ParameterID"].ToString();
                            Data.Parameter = parameterList.Where(k => k.Value == Data.ParameterValue).Select(k => k.Text).FirstOrDefault();
                        }
                        DataList.Add(Data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return DataList;
        }

        internal static List<string> GetAllComponentsbyMachine(string MachineID)
        {
            List<string> ComponentID = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_GetLookups", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filter", MachineID);
                cmd.Parameters.AddWithValue("@name", "Comp");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ComponentID.Add(rdr["Componentid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return ComponentID;
        }



        internal static bool SaveVibrationSettings(string machineID, string ComponentID, string Operation, string warningUSL, string dangerUSL, string MValue, string NValue,string parameter)
        {
            bool saved = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = "";
            if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
            {
                Query = @"if not exists(select * from TAFE_PARWANOO_VibrationMasterTable where MachineID=@MachineID and ComponentID=@ComponentID and Operation=@Operation and ParameterID=@ParameterID)
begin
	insert into TAFE_PARWANOO_VibrationMasterTable(MachineID,Operation,ComponentID,UpperErrorLimit,UpperWarningLimit,Total_M_Observation,ApplyRuleFor_N_Observation,ParameterID)
	values(@MachineID,@Operation,@ComponentID,@UpperErrorLimit,@UpperWarningLimit,@MValue,@NValue,@ParameterID)
end
else
begin
	update TAFE_PARWANOO_VibrationMasterTable set UpperWarningLimit=@UpperWarningLimit,UpperErrorLimit=@UpperErrorLimit,Total_M_Observation=@MValue,ApplyRuleFor_N_Observation=@NValue 
	where MachineID=@MachineID and ComponentID=@ComponentID and Operation=@Operation and ParameterID=@ParameterID
end";
            }
            else
            {
                Query = @"if not exists(select * from TAFE_PARWANOO_VibrationMasterTable where MachineID=@MachineID and ComponentID=@ComponentID and Operation=@Operation)
begin
	insert into TAFE_PARWANOO_VibrationMasterTable(MachineID,Operation,ComponentID,UpperErrorLimit,UpperWarningLimit,Total_M_Observation,ApplyRuleFor_N_Observation)
	values(@MachineID,@Operation,@ComponentID,@UpperErrorLimit,@UpperWarningLimit,@MValue,@NValue)
end
else
begin
	update TAFE_PARWANOO_VibrationMasterTable set UpperWarningLimit=@UpperWarningLimit,UpperErrorLimit=@UpperErrorLimit,Total_M_Observation=@MValue,ApplyRuleFor_N_Observation=@NValue 
	where MachineID=@MachineID and ComponentID=@ComponentID and Operation=@Operation 
end";
            }
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ComponentID", ComponentID);
                cmd.Parameters.AddWithValue("@Operation", Operation);
                cmd.Parameters.AddWithValue("@UpperWarningLimit", warningUSL);
                cmd.Parameters.AddWithValue("@UpperErrorLimit", dangerUSL);
                cmd.Parameters.AddWithValue("@MValue", MValue);
                cmd.Parameters.AddWithValue("@NValue", NValue);
                cmd.Parameters.AddWithValue("@ParameterID", parameter);
                cmd.ExecuteNonQuery();
                saved = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return saved;
        }

        internal static bool DeleteVibrationSettings(string machineID, string Component, string Operation,string Parameter)
        {
            bool saved = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                string Query = "";
                if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    Query = @"Delete from TAFE_PARWANOO_VibrationMasterTable where MachineID=@MachineID and ComponentID=@ComponentID and Operation=@Operation and ParameterID=@ParameterID";
                }
                else
                {
                    Query = @"Delete from TAFE_PARWANOO_VibrationMasterTable where MachineID=@MachineID and ComponentID=@ComponentID and Operation=@Operation ";
                }
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@Operation", Operation);
                cmd.Parameters.AddWithValue("@ParameterID", Parameter);
                cmd.ExecuteNonQuery();
                saved = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return saved;
        }

        #endregion

        #region ---Vibration Dashboard---

        internal static string GetCurrentShiftStart(out string endTime)
        {
            string dt = string.Empty;
            endTime = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                cmd = new SqlCommand("[s_GetCurrentShift]", conn);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    dt = sdr["StartTime"].ToString();
                    endTime = sdr["EndTime"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            if (string.IsNullOrEmpty(dt))
                dt = DateTime.Now.ToString();
            if (string.IsNullOrEmpty(endTime))
                endTime = DateTime.Now.ToString();

            return dt;
        }
        #endregion

        #region "Get Machine Data"
        internal static List<string> GetAllMachines(string plantId)
        {
            plantId = plantId == "ALL" ? "" : plantId;
            plantId = plantId == "All" ? "" : plantId;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            try
            {
                // sqlConn.Open();
                cmd = new SqlCommand(@"s_GetLookups", sqlConn);

                if (!string.IsNullOrEmpty(plantId))
                {
                    if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase)) plantId = string.Empty;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"name", "Machine");
                    cmd.Parameters.AddWithValue(@"filter", plantId);
                }
                else
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"name", "Machine");
                }

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
        #endregion

        internal static List<string> GetAllCycleStartTimes(string startTime, string endTime, string machineId, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> cycleStartTimesList = new List<string>();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_TAFEParawano_VibrationDashboard]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Param", param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        cycleStartTimesList.Add(sdr["CycleStart"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return cycleStartTimesList;
        }


        internal static List<PlotBands> GetPlotBandData(string MachineID)
        {
            List<PlotBands> plotBandList = new List<PlotBands>();
            PlotBands plotBand = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("Select * from TAFE_PARWANOO_VibrationLiveData where MachineID = @MachineID", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
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
                else
                {
                    plotBandList.Add(new PlotBands { color = "$EE7157", from = 1.6, to = 2.4 });
                    plotBandList.Add(new PlotBands { color = "E7F18B", from = 2.4, to = 3.2 });
                    plotBandList.Add(new PlotBands { color = "$EE7157", from = 0.6, to = 1.4 });
                    plotBandList.Add(new PlotBands { color = "E7F18B", from = 1.4, to = 3.2 });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return plotBandList;
        }


        internal static DataSet Getdataset(string fromDate, string toDate, string machineId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataSet dataSet = new DataSet();
            string Query = @"select * from TAFE_PARWANOO_VibrationLiveData where MachineID = @machineid
                            select UpdatedTS as UpdatedTS and ActualValue from TAFE_PARWANOO_VibrationLiveData where MachineID = @machineid
                            select ComponentID as component from TAFE_PARWANOO_VibrationLiveData where MachineID = @machineid";
            try
            {
                cmd = new SqlCommand(Query, sqlConn);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate);
                cmd.Parameters.AddWithValue("@EndTime", toDate);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Param", "Bytime");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return dataSet;
        }

        internal static DataTable GetDataRefresh(string machineId, string fromDate, string toDate,string parameterid, out DataTable CycleVibration)
        {
            CycleVibration = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable VibrationData = new DataTable();
            try
            {
                string frmDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime lastdate = Convert.ToDateTime(HttpContext.Current.Session["lastviewedTimeVibration"].ToString());
                if ((DateTime.Now - lastdate).TotalSeconds > 20)
                {
                    lastdate = (DateTime.Now.AddSeconds(-20));
                }
                cmd = new SqlCommand("[dbo].[S_TAFEParawano_VibrationDashboard]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", lastdate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Enddate", frmDt);
                cmd.Parameters.AddWithValue("@Machineid", machineId);
                if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    cmd.Parameters.AddWithValue("@ParameterID", parameterid);
                    cmd.Parameters.AddWithValue("@Param", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Param", "TAFE");
                }
                rdr = cmd.ExecuteReader();
                VibrationData.Load(rdr);
                CycleVibration.Load(rdr);
                if (!ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    if (VibrationData.Columns.Contains("ParameterID"))
                    {
                        VibrationData.Columns.Remove("ParameterID");
                        VibrationData.Columns.Add("ParameterID");
                    }
                    else
                    {
                        VibrationData.Columns.Add("ParameterID");
                    }
                }
                HttpContext.Current.Session["lastviewedTimeVibration"] = frmDt;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
                if (rdr != null) rdr.Close();
            }
            return VibrationData;
        }

        internal static DataTable Vibrationdata(string fromDate, string toDate, string machineId,string parameters, out DataTable Cyclevibration)
        {
            Cyclevibration = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataSet dataSet = new DataSet();
            SqlDataReader rdr = null;
            DataTable VibrationData = new DataTable();

            try
            {
                //string frmDt = DateTime.Now.AddSeconds(20).ToString("yyyy-MM-dd HH:mm:ss");
                cmd = new SqlCommand("[dbo].[S_TAFEParawano_VibrationDashboard]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Enddate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machineid", machineId);
                if (ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    cmd.Parameters.AddWithValue("@ParameterID", parameters);
                    cmd.Parameters.AddWithValue("@Param", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Param", "TAFE");
                }

                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                VibrationData.Load(rdr);
                Cyclevibration.Load(rdr);
                if (!ConfigurationManager.AppSettings["VibrationParameterEnable"].Equals("1"))
                {
                    if (VibrationData.Columns.Contains("ParameterID"))
                    {
                        VibrationData.Columns.Remove("ParameterID");
                        VibrationData.Columns.Add("ParameterID");
                    }
                    else
                    {
                        VibrationData.Columns.Add("ParameterID");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
                if (rdr != null) rdr.Close();
            }
            return VibrationData;
        }

    }
}