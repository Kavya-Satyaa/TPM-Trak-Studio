using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.Web.Configuration;
using System.Configuration;

namespace Web_TPMTrakDashboard.Models
{

    public class VDGDataBaseAccess
    {

        public static int PageSize
        {
            get
            {
                return ConnectionManager.pageSize;
            }
        }
        #region--VDG Filter------
        internal static List<string> GetPlantID_VDG()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct PlantID from PlantInformation", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["PlantID"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        internal static List<string> GetCellID_VDG(string plantID)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct GroupID from PlantMachineGroups where (PlantID=@PlantID OR ISNULL(@PlantID,'')='')", con);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["GroupID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        internal static List<string> GetMachineID_VDG(string PlantID,string CellID)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"SELECT DISTINCT A1.machineid,A1.PlantID,A2.GroupID FROM PlantMachine A1
LEFT JOIN PlantMachineGroups A2 ON A1.PlantID=A2.PlantID AND A1.machineid=A2.MachineID
WHERE (A1.PlantID=@PlantID OR ISNULL(@PlantID,'')='') AND (A2.GroupID=@GroupID OR ISNULL(@GroupID,'')='')", con);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@GroupID", CellID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["machineid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        #endregion
        #region "Get Machine Data"
        internal static List<string> GetAllMachines(string plantId, string lineId)
        {
            plantId = plantId == "ALL" ? "" : plantId;
            plantId = plantId == "All" ? "" : plantId;
            lineId = lineId == "ALL" ? "" : lineId;
            lineId = lineId == "All" ? "" : lineId;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            try
            {
                // sqlConn.Open();
                cmd = new SqlCommand(@"s_GetLookups", sqlConn);

                if (!string.IsNullOrEmpty(plantId) && !string.IsNullOrEmpty(lineId))
                {
                    if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase)) plantId = string.Empty;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"name", "Machine");
                    cmd.Parameters.AddWithValue(@"filter", plantId);
                    cmd.Parameters.AddWithValue("@GroupId", lineId);
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
                        machineList.Add(rdr["MachineId"].ToString());
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

        internal static List<string> GetMachinesbyPlantCell(string plant, string cell)
        {
            List<string> MachineList = new List<string>();
            SqlConnection connection = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = string.Empty;
            if (string.IsNullOrEmpty(plant))
            {
                if (string.IsNullOrEmpty(cell))
                {
                    Query = @"select DISTINCT MachineID from PlantMachineGroups";
                }
                else
                {
                    Query = "select DISTINCT MachineID from PlantMachineGroups where GroupID=@groupID";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(cell))
                {
                    Query = "select DISTINCT MachineID from PlantMachineGroups where PlantID=@plantid";
                }
                else
                {
                    Query = @"select DISTINCT MachineID from PlantMachineGroups where PlantID=@plantid and GroupID=@groupID";
                }
            }
            try
            {
                cmd = new SqlCommand(Query, connection);
                cmd.Parameters.AddWithValue("@plantid", plant);
                cmd.Parameters.AddWithValue("@groupID", cell);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MachineList.Add(rdr["MachineID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (connection != null) connection.Close();
                if (rdr != null) rdr.Close();
            }
            return MachineList;
        }

        internal static List<string> GetAllLines(string plantId)
        {
            plantId = plantId == "ALL" ? "" : plantId;
            plantId = plantId == "All" ? "" : plantId;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> listOfLines = new List<string>();
            try
            {
                // sqlConn.Open();
                cmd = new SqlCommand(@"s_GetLookups", sqlConn);

                if (!string.IsNullOrEmpty(plantId))
                {
                    if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase)) plantId = string.Empty;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"name", "Group");
                    cmd.Parameters.AddWithValue(@"filter", plantId);
                }
                else
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"name", "Group");
                }

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        listOfLines.Add(rdr["GroupID"].ToString());
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
            return listOfLines;
        }

        #region "Get Time Formate Data----------"
        internal static string GetCockpitTimeFormat()
        {
            SqlDataReader sdr = null;
            string timeFormat = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("select valueinText from cockpitdefaults where Parameter='TimeFormat'", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        timeFormat = sdr["valueinText"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return timeFormat;
        }
        #endregion

        internal static async Task<DataTable> GetProductionAndDownData(string procName, string fromDateTime, string toDateTime, string machineId)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                sdr = await cmd.ExecuteReaderAsync();
                dt.Load(sdr);
                dt.AcceptChanges();
                if (procName.ToLower().Contains("s_getcockpitproductiondata_eshopx") || procName.ToLower().Contains("s_getcockpitdowndata_eshopx"))
                {
                    dt.Columns.Add("StartTimeToDisplay", typeof(string));
                    dt.Columns.Add("EndTimeToDisplay", typeof(string));
                    if (dt.Rows.Count > 0)
                    {
                        if (ConfigurationManager.AppSettings["GKReport"].ToString() == "1")
                        {
                            dt.AsEnumerable().ToList<DataRow>().ForEach(k =>
                            {
                                k["StartTimeToDisplay"] = k["StartTime"].ToString() == "" ? "" : Convert.ToDateTime(k["StartTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                                k["EndTimeToDisplay"] = k["EndTime"].ToString() == "" ? "" : Convert.ToDateTime(k["EndTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                            });
                        }
                        else
                        {
                            dt.AsEnumerable().ToList<DataRow>().ForEach(k =>
                            {
                                k["StartTimeToDisplay"] = k["StartTime"].ToString() == "" ? "" : Convert.ToDateTime(k["StartTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                                k["EndTimeToDisplay"] = k["EndTime"].ToString() == "" ? "" : Convert.ToDateTime(k["EndTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            });
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static DataTable GetMachineData(string procName, string fromDateTime, string toDateTime, string plantId, string machineId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                if ((plantId).Equals("All")) plantId = string.Empty;
                cmd.Parameters.AddWithValue("@PlantId", plantId);
                cmd.Parameters.AddWithValue("@SortOrder", "");
                cmd.Parameters.AddWithValue("@SortType", "");
                sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("Pross Name : " + procName + stopwatch.Elapsed.TotalSeconds);
            return dt;
        }

        internal static DataTable GetEventData(string fromDateTime, string toDateTime, string machineId)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"Select A1.IDD,A2.machineid,A1.Mc,A3.componentid,A1.Comp,A4.operationno,A1.Opn,A5.[Name] as OprName,A1.Opr,A5.Employeeid,A1.EventID,A6.EventDescription,A1.EventTS
	from EventTransactionDetails_Hawkins A1
	inner join machineinformation A2 on A1.Mc=A2.InterfaceID
	inner join componentinformation A3 on A1.Comp=A3.InterfaceID
	inner join componentoperationpricing A4 on A1.Opn=A4.InterfaceID and A4.componentid=A3.componentid and A4.machineid=A2.machineid
	inner join employeeinformation A5 on A1.Opr=A5.interfaceid
	inner join EventMasterDetails_Hawkins A6 on A1.EventID=A6.EventID
	where (EventTS>=@StartTime and EventTS<=@EndTime) and A2.machineid=@MachineId  order by A1.EventTS", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
        internal static DataTable GetProductionAndDownDataGraph(string procName, string fromDateTime, string toDateTime, string machineId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.AcceptChanges();

                stopwatch.Stop();
                Logger.WriteDebugLog("Pross Name : " + procName + " : " + stopwatch.Elapsed.TotalSeconds);
                if (procName.ToLower().Contains("s_getcockpitproductiondata_eshopx") || procName.ToLower().Contains("s_getcockpitdowndata_eshopx"))
                {
                    dt.Columns.Add("StartTimeToDisplay", typeof(string));
                    dt.Columns.Add("EndTimeToDisplay", typeof(string));
                    if (dt.Rows.Count > 0)
                    {
                        if (ConfigurationManager.AppSettings["GKReport"].ToString() == "1")
                        {
                            dt.AsEnumerable().ToList<DataRow>().ForEach(k =>
                            {
                                k["StartTimeToDisplay"] = k["StartTime"].ToString() == "" ? "" : Convert.ToDateTime(k["StartTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                                k["EndTimeToDisplay"] = k["EndTime"].ToString() == "" ? "" : Convert.ToDateTime(k["EndTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                            });
                        }
                        else
                        {
                            dt.AsEnumerable().ToList<DataRow>().ForEach(k =>
                            {
                                k["StartTimeToDisplay"] = k["StartTime"].ToString() == "" ? "" : Convert.ToDateTime(k["StartTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                                k["EndTimeToDisplay"] = k["EndTime"].ToString() == "" ? "" : Convert.ToDateTime(k["EndTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            });
                        }

                    }
                }
                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //DataTable datatable = new DataTable();
                //adapter.Fill(datatable);

                //await Task.Run(() => adapter.Fill(datatable));
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        //internal static async Task<DataTable> FillAsync(string procName, string fromDateTime, string toDateTime, string machineId)
        //{
        //    return await Task.Run(() => { return GetProductionAndDownData(procName, fromDateTime, toDateTime, machineId); });
        //}

        internal static List<string> GetComponentOperationForMachine(string fromDateTime, string toDateTime, string machineId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> ComponentOperation = new List<string>();
            try
            {
                string qry = @"SELECT DISTINCT componentinformation.componentid AS ComponentID, componentoperationpricing.operationno AS OperationNo ,componentoperationpricing.description as description FROM autodata INNER JOIN  machineinformation ON autodata.mc = machineinformation.InterfaceID INNER JOIN componentinformation ON autodata.comp = componentinformation.InterfaceID INNER JOIN componentoperationpricing ON autodata.opn = componentoperationpricing.InterfaceID AND componentinformation.componentid = componentoperationpricing.componentid AND componentoperationpricing.machineid = machineinformation.machineid WHERE (autodata.ndtime > @fromTime) AND  (autodata.ndtime <= @toTime)  AND  (machineinformation.machineid = @machineId) AND (autodata.datatype = 1) ";
                SqlCommand cmd = new SqlCommand(qry, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"fromTime", fromDateTime);
                cmd.Parameters.AddWithValue(@"toTime", toDateTime);
                cmd.Parameters.AddWithValue(@"machineId", machineId);
                cmd.CommandTimeout = 300;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ComponentOperation.Add(rdr["ComponentID"].ToString() + " | " + rdr["OperationNo"].ToString() + " | " + rdr["Description"].ToString());
                }
                rdr.Close();
                stopwatch.Stop();
                Logger.WriteDebugLog("GetComponentOperationForMachine drop down : " + stopwatch.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return ComponentOperation;
        }

        #region "Get Data of Component Info------"
        internal static VDGComponentValues GetCockpitComponentData(string fromDateTime, string toDateTime, string ComponentId, string OperationNo, string machineId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            VDGComponentValues vals = new VDGComponentValues();
            try
            {
                SqlCommand cmd = new SqlCommand("[s_GetCockpitComponentsData]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@ComponentId", ComponentId);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();
                    if (!Convert.IsDBNull(rdr["CycleTime"]))
                    {
                        vals.StCycleTime = rdr["CycleTime"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["LoadUnLoad"]))
                    {
                        vals.StLoadTime = rdr["LoadUnLoad"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["AverageCycleTime"]))
                    {
                        vals.AvgCycleTime = rdr["AverageCycleTime"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["AverageLoadUnLoad"]))
                    {
                        vals.AvgLoadTime = rdr["AverageLoadUnLoad"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["OperationCount"]))
                    {
                        vals.OperationCount = rdr["OperationCount"].ToString();
                    }
                }
                rdr.Close();
                stopwatch.Stop();
                Logger.WriteDebugLog("s_GetCockpitComponentsData : " + stopwatch.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return vals;
        }
        #endregion

        internal static string GetShopdefaultsTimeFormat()
        {
            SqlDataReader sdr = null;
            string timeFormat = "ss";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select valueinText from Shopdefaults where Parameter='TimeInFormat'", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        timeFormat = sdr["valueinText"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return timeFormat;
        }

        internal static List<string> GetMachineIDsLst(string plantID)
        {
            SqlDataReader sdr = null;
            List<string> MachLst =new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select mi.Machineid from machineinformation mi inner join PlantMachine pm on pm.Machineid = mi.Machineid and tpmtrakenabled=1 and 
                (mi.description like '%Shaft%' or mi.description like '%Head%')  and (ISNULL(@PlantId, '') = '' OR pm.PlantID  = @PlantId)  order by machineid asc", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantId", plantID.Equals("All",StringComparison.OrdinalIgnoreCase)? "" :plantID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while(sdr.Read())
                    {
                        MachLst.Add(sdr["Machineid"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return MachLst;
        }


        internal static void GetAllStdTimes(string MachineID, string ComponentID, string OperationNo, out string CycleTime, out string MachiningTime)
        {
            SqlDataReader sdr = null;
            MachiningTime = string.Empty;
            CycleTime = string.Empty;

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select machiningtime,cycleTime,loadUnload from componentOperationPricing where MachineID = @MachineID and ComponentID=@ComponentID and OperationNo=@operationNo", conn);

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"machineid", MachineID);
                cmd.Parameters.AddWithValue(@"componentid", ComponentID);
                cmd.Parameters.AddWithValue(@"OperationNo", OperationNo);

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        CycleTime = sdr["cycleTime"].ToString();
                        MachiningTime = sdr["machiningtime"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

        }

        internal static object GetAllHydroStaticMachineLst(string plantID)
        {
            SqlDataReader sdr = null;
            List<string> MachLst = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select mi.Machineid from machineinformation mi 
                                        inner join PlantMachine pm on pm.Machineid = mi.Machineid 
                                        where tpmtrakenabled=1 and 
                                        (mi.description like '%Hydro Static Machine%')  and (ISNULL(@PlantId, '') = '' OR pm.PlantID  = @PlantId)  
                                        order by machineid asc", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantId", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MachLst.Add(sdr["Machineid"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return MachLst;
        }

        internal static void UpdateAllStdTimes(string CycleTime, string LoadUnload, string MachineID, string ComponentID, string OperationNo, bool UpdateAllMachinesKTA, out string isSuccessfull)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            List<string> ComponentOperation = new List<string>();
            string query = string.Empty;
            isSuccessfull = string.Empty;
            try
            {
                var machinelist = MachineID.Split(',');
                MachineID = "";
                for (int i = 0; i < machinelist.Length; i++)
                {
                    if (MachineID == "")
                    {
                        MachineID += "'" + machinelist[i] + "'";
                    }
                    else
                    {
                        MachineID += ",'" + machinelist[i] + "'";
                    }
                }
                if(WebConfigurationManager.AppSettings["KTASpindlePages"].ToString() == "1")
                {
                    if(UpdateAllMachinesKTA)
                        query = "Update componentOperationPricing set machiningtime = @machiningTime ,CycleTime=@CycleTime  where ComponentID=@ComponentID and OperationNo=@operationNo";
                    else
                    {
                        query = "Update componentOperationPricing set machiningtime = @machiningTime ,CycleTime=@CycleTime  where MachineID in (" + MachineID + ")  and ComponentID=@ComponentID and OperationNo=@operationNo";
                    }
                }
                else
                {
                    query = "Update componentOperationPricing set machiningtime = @machiningTime ,CycleTime=@CycleTime  where MachineID in (" + MachineID + ")  and ComponentID=@ComponentID and OperationNo=@operationNo";
                }
                
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"machiningTime", CycleTime);
                cmd.Parameters.AddWithValue(@"CycleTime", LoadUnload);
                cmd.Parameters.AddWithValue(@"machineid", MachineID);
                cmd.Parameters.AddWithValue(@"componentid", ComponentID);
                cmd.Parameters.AddWithValue(@"OperationNo", OperationNo);

                int x = cmd.ExecuteNonQuery();
                isSuccessfull = "Successfull";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static void UpdateRemarks(string remarks, string ActionTaken, string production_ID, out bool IsRemarksUpdated)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            IsRemarksUpdated = false;
            try
            {
                SqlCommand cmd = new SqlCommand(@"update Autodata set remarks = @remarks, ActionTaken=@ActionTaken where id = @productionID", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@remarks", remarks);
                cmd.Parameters.AddWithValue("@ActionTaken", ActionTaken);
                cmd.Parameters.AddWithValue("@productionID", production_ID);
                int x = cmd.ExecuteNonQuery();

                if (x > 0)
                {
                    IsRemarksUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }


        internal static DateTime GetComponentsStartEndTime(string machineId, string componentId, out DateTime endTime)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            endTime = DateTime.Now;
            DateTime startTime = DateTime.Now.AddHours(-24);
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select timefrom = min(sttime), timeto = max(ndtime) from (select top " + componentId + " sttime, ndtime from autodata inner join machineinformation on autodata.mc = machineinformation.interfaceid where machineid = @machineId and datatype = 1 order by ndtime desc) as table1", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineId", machineId);
                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        startTime = Convert.ToDateTime(sdr["TimeFrom"]);
                        endTime = Convert.ToDateTime(sdr["TimeTo"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }

            return startTime;
        }

        internal static DataTable GetAnalysedAutoData(string fromDateTime, string toDateTime, string machineID, string Param)
        {
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("s_GetAnalysisData", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineID);
                cmd.Parameters.AddWithValue("@Param", Param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    {
                        dt.Load(sdr);
                        dt.AcceptChanges();

                        DataColumn Col = dt.Columns.Add("SerialNo", System.Type.GetType("System.Int32"));
                        Col.SetOrdinal(0);

                        dt.Rows[0][0] = 1;

                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i][0] = i + 1;
                        }
                        dt.AcceptChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return dt;
        }

        internal static VDGDataAnalysis GetValsForAutoAndRawData(string fromDateTime, string toDateTime, string machineID, string Param)
        {
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            VDGDataAnalysis vals = new VDGDataAnalysis();
            try
            {
                cmd = new SqlCommand("s_GetAnalysisData", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineID);
                cmd.Parameters.AddWithValue("@Param", Param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        // Raw Data
                        if ((sdr["Parameter"].ToString()).Equals("ProgramStart"))
                        {
                            vals.ProgramStart = sdr["pCount"].ToString();
                        }
                        else if ((sdr["Parameter"].ToString()).Equals("ProductionRecord"))
                        {
                            vals.ProductionRecord = sdr["pCount"].ToString();
                        }
                        else if ((sdr["Parameter"].ToString()).Equals("DownRecord"))
                        {
                            vals.DownRecord = sdr["pCount"].ToString();
                        }
                        else if ((sdr["Parameter"].ToString()).Equals("InCycleDownRecord"))
                        {
                            vals.InCycleDownRecord = sdr["pCount"].ToString();
                        }

                        // Auto Data
                        else if ((sdr["Parameter"].ToString()).Equals("ProductionRecordStarted"))
                        {
                            vals.ProductionRecordStart = sdr["pCount"].ToString();
                        }
                        else if ((sdr["Parameter"].ToString()).Equals("ProductionRecordEnded"))
                        {
                            vals.ProductionRecordEnded = sdr["pCount"].ToString();
                        }
                        else if ((sdr["Parameter"].ToString()).Equals("DownRecordStarted"))
                        {
                            vals.DownRecordStarted = sdr["pCount"].ToString();
                        }
                        else if ((sdr["Parameter"].ToString()).Equals("DownRecordEnded"))
                        {
                            vals.DownRecordEnded = sdr["pCount"].ToString();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return vals;
        }

        internal static DataTable GetComponentsVariance(string fromDateTime, string toDateTime, string machineID, string component, string operation, string varianceType, string variance, string varianceVal)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            List<string> ComponentOperation = new List<string>();
            try
            {
                string qry = GetSqlQuery(varianceType, variance);
                SqlCommand cmd = new SqlCommand(qry, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"fromTime", fromDateTime);
                cmd.Parameters.AddWithValue(@"toTime", toDateTime);
                cmd.Parameters.AddWithValue(@"machineId", machineID);
                cmd.Parameters.AddWithValue(@"componentId", component);
                cmd.Parameters.AddWithValue(@"operationNo", operation);
                cmd.Parameters.AddWithValue(@"varianceVal", varianceVal);
                cmd.CommandTimeout = 300;
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        private static string GetSqlQuery(string varianceType, string variance)
        {
            string qry = string.Empty;

            if (varianceType.Equals("Cutting Time"))
            {
                if (variance.Equals("Most Frequent Occ"))
                {
                    qry = @"  select  A.Cycletime As Val,Count(*)As FreqOcc From Autodata A Inner Join MachineInformation M on A.Mc=M.Interfaceid Inner Join ComponentInformation C ON A.Comp=C.Interfaceid
                             Inner Join ComponentOperationPricing O ON A.Opn=O.Interfaceid And C.ComponentId=O.ComponentId and O.Machineid = M.machineid Where Datatype=1 And Ndtime> @fromTime And Ndtime<= @toTime
                             And M.MachineID= @machineId And C.ComponentId= @componentID And OperationNo= @operationNo group by A.Cycletime having Count(*)=(Select Max(Number) From
                             (select  Count(*)As Number From Autodata A Inner Join MachineInformation M on A.Mc=M.Interfaceid Inner Join ComponentInformation C ON A.Comp=C.Interfaceid Inner Join ComponentOperationPricing O ON A.Opn=O.Interfaceid And C.ComponentId=O.ComponentId
                             and O.Machineid = M.machineid  Where Datatype=1 And Ndtime > @fromTime And Ndtime <= @toTime
                             And M.MachineID= @machineID And C.ComponentId= @componentID  And OperationNo= @operationNo group by A.Cycletime) As t1)";
                }
                else
                {
                    qry = @"Select StTime,NdTime,A.CycleTime From AutoData A Inner Join MachineInformation M on A.Mc=M.Interfaceid Inner Join ComponentInformation C ON A.Comp=C.Interfaceid Inner Join ComponentOperationPricing O ON A.Opn=O.Interfaceid And C.ComponentId=O.ComponentId
                           and O.Machineid = M.machineid Where DataType=1 And Ndtime> @fromTime And Ndtime<= @toTime And M.MachineID=@machineID And C.ComponentId=@componentId And OperationNo=@operationNo ";

                    if (variance.Equals("Above"))
                    {
                        qry = qry + "And A.CycleTime > O.Machiningtime + (O.Machiningtime/100) * (@VarianceVal)";
                    }

                    else
                    {
                        qry = qry + "And A.CycleTime < O.Machiningtime - (O.Machiningtime/100)*(@VarianceVal)";
                    }
                }

            }

            else
            {
                if (variance.Equals("Most Frequent Occ"))
                {
                    qry = @" select  A.Loadunload As Val,Count(*)As FreqOcc From Autodata A Inner Join MachineInformation M on A.Mc=M.Interfaceid
                            Inner Join ComponentInformation C ON A.Comp=C.Interfaceid
                            Inner Join ComponentOperationPricing O ON A.Opn=O.Interfaceid And C.ComponentId=O.ComponentId
                            and O.Machineid = M.machineid Where Datatype=1 And Ndtime> @fromTime And Ndtime<= @toTime And M.MachineID= @machineid And C.ComponentId= @componentID And OperationNo= @operationNo
                            group by A.Loadunload having Count(*)=(Select Max(Number) From (select  Count(*)As Number From Autodata A Inner Join MachineInformation M on A.Mc=M.Interfaceid Inner Join ComponentInformation C ON A.Comp=C.Interfaceid
                            Inner Join ComponentOperationPricing O ON A.Opn=O.Interfaceid And C.ComponentId=O.ComponentId
                            and O.Machineid = M.machineid  Where Datatype=1 And Ndtime > @fromTime And Ndtime<= @toTime And M.MachineID= @machineid And C.ComponentId= @componentID And OperationNo= @operationNo
                            group by A.Loadunload )As t1)";
                }
                else
                {
                    qry = @"Select StTime,NdTime,A.Loadunload From AutoData A Inner Join MachineInformation M on A.Mc=M.Interfaceid Inner Join ComponentInformation C ON A.Comp=C.Interfaceid
                           Inner Join ComponentOperationPricing O ON A.Opn=O.Interfaceid And C.ComponentId=O.ComponentId and O.Machineid = M.machineid Where DataType=1 And Ndtime> @fromTime And Ndtime<= @toTime
                           And M.MachineID= @machineID And C.ComponentId=@componentId  And OperationNo=@operationNo ";

                    if (variance.Equals("Above"))
                    {
                        qry = qry + "And A.LoadUnload>(O.Cycletime - O.Machiningtime)+((O.cycletime - O.machiningtime)/100)*(@VarianceVal)";
                    }

                    else
                    {
                        qry = qry + " And A.LoadUnload<(O.Cycletime - O.Machiningtime)-((O.Cycletime - O.Machiningtime)/100)*(@VarianceVal)";
                    }
                }
            }

            return qry;
        }

        internal static VDGComponentStatisticValues GetComponentStats(string fromDateTime, string toDateTime, string machineID, string component, string operation)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            VDGComponentStatisticValues vals = new VDGComponentStatisticValues();
            try
            {
                SqlCommand cmd = new SqlCommand("[s_GetComponent_Statistics]", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineID);
                cmd.Parameters.AddWithValue("@ComponentId", component);
                cmd.Parameters.AddWithValue("@OperationNo", operation);
                rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {

                    rdr.Read();
                    if (!Convert.IsDBNull(rdr["StdMachiningTime"]))
                    {
                        vals.CuttingStTime = rdr["StdMachiningTime"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["MaxCycleTime"]))
                    {
                        vals.CuttingMax = rdr["MaxCycleTime"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["MinCycleTime"]))
                    {
                        vals.CuttingMin = rdr["MinCycleTime"].ToString();
                    }

                    if (!Convert.IsDBNull(rdr["AvgCycleTime"]))
                    {
                        vals.CuttingAvgTime = rdr["AvgCycleTime"].ToString();
                    }

                    if (!Convert.IsDBNull(rdr["RangeCycleTime"]))
                    {
                        vals.CuttingRange = rdr["RangeCycleTime"].ToString();
                    }

                    if (!Convert.IsDBNull(rdr["stdLoadUnLoad"]))
                    {
                        vals.LoadUnLoadStTime = rdr["stdLoadUnLoad"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["maxLoadUnLoad"]))
                    {
                        vals.LoadUnLoadMax = rdr["maxLoadUnLoad"].ToString();
                    }
                    if (!Convert.IsDBNull(rdr["AvgLoadUnLoad"]))
                    {
                        vals.LoadUnLoadAvgTime = rdr["AvgLoadUnLoad"].ToString();
                    }

                    if (!Convert.IsDBNull(rdr["minLoadUnLoad"]))
                    {
                        vals.LoadUnLoadMin = rdr["minLoadUnLoad"].ToString();
                    }

                    if (!Convert.IsDBNull(rdr["rangeLoadUnLoad"]))
                    {
                        vals.LoadUnLoadRange = rdr["rangeLoadUnLoad"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return vals;
        }

        #region "Populate Start Date "
        public static string GetLogicalDayStart(string LRunDay)
        {
            //CultureInfo enUS = new CultureInfo("en-US");
            //string dateString = string.Empty;
            //DateTime dateValue = DateTime.Now;
            //if (DateTime.TryParseExact(LRunDay, "dd-MM-yyyy", enUS, DateTimeStyles.None, out dateValue))
            //{
            //    dateString = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //else
            //{
            //    dateString = DateTime.ParseExact(LRunDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            //}
            //SqlConnection Con = ConnectionManager.GetConnection();
            //SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayStart( '" + string.Format("{0:yyyy-MMM-dd hh:mm:ss}", DateTime.Parse(string.IsNullOrEmpty(dateString) ? LRunDay : dateString).AddSeconds(1)) + "')", Con);

            Logger.WriteDebugLog(LRunDay);
            DateTime ts = Util.GetDateTime(LRunDay);
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayStart( '" + ts.ToString("yyyy-MM-dd 13:00:00") + "')", Con);

            object SEDate = null;
            try
            {
                SEDate = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null)
                {
                    Con.Close();
                }
            }
            if (SEDate == null || Convert.IsDBNull(SEDate))
            {
                return string.Empty;
            }
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }
        #endregion

        #region "Populate Start Date For Live"
        public static string GetLogicalDayStartForLive(string LRunDay)
        {
            //CultureInfo enUS = new CultureInfo("en-US");
            //string dateString = string.Empty;
            //DateTime dateValue = DateTime.Now;
            //if (DateTime.TryParseExact(LRunDay, "dd-MM-yyyy", enUS, DateTimeStyles.None, out dateValue))
            //{
            //    dateString = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //else
            //{
            //    dateString = DateTime.ParseExact(LRunDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            //}
            //SqlConnection Con = ConnectionManager.GetConnection();
            //SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayStart( '" + string.Format("{0:yyyy-MMM-dd hh:mm:ss}", DateTime.Parse(string.IsNullOrEmpty(dateString) ? LRunDay : dateString).AddSeconds(1)) + "')", Con);

            Logger.WriteDebugLog(LRunDay);
            DateTime ts = Util.GetDateTime(LRunDay);
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayStart( '" + ts.ToString("yyyy-MM-dd HH:mm:ss") + "')", Con);

            object SEDate = null;
            try
            {
                SEDate = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null)
                {
                    Con.Close();
                }
            }
            if (SEDate == null || Convert.IsDBNull(SEDate))
            {
                return string.Empty;
            }
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }
        #endregion

        #region "Populate End Date"
        public static string GetLogicalDayEnd(string LRunDay)
        {
            //CultureInfo enUS = new CultureInfo("en-US");
            string dateString = string.Empty;
            //DateTime dateValue = DateTime.Now;
            //if (DateTime.TryParseExact(LRunDay, "dd-MM-yyyy", enUS, DateTimeStyles.None, out dateValue))
            //{
            //    dateString = dateValue.ToString();
            //}
            //else
            //{
            //    dateString = DateTime.ParseExact(LRunDay, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            //}
            //SqlConnection Con = ConnectionManager.GetConnection();
            //SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayEnd( '" + string.Format("{0:yyyy-MMM-dd hh:mm:ss}", DateTime.Parse(string.IsNullOrEmpty(dateString) ? LRunDay : dateString).AddSeconds(1)) + "')", Con);

            DateTime dateValue = Util.GetDateTime(LRunDay);

            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayEnd( '" + dateValue.ToString("yyyy-MM-dd 13:00:00") + "')", Con);


            object SEDate = null;
            try
            {
                SEDate = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null)
                {
                    Con.Close();
                }
            }
            if (SEDate == null || Convert.IsDBNull(SEDate))
            {
                return string.Empty;
            }
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }
        #endregion

        #region "Populate Start Date "
        public static string GetLogicalDayStart1(string LRunDay)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayStart( '" + string.Format("{0:yyyy-MM-dd}", LRunDay) + "')", Con);

            object SEDate = null;
            try
            {
                SEDate = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null)
                {
                    Con.Close();
                }
            }
            if (SEDate == null || Convert.IsDBNull(SEDate))
            {
                return string.Empty;
            }
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }
        #endregion

        #region "Populate End Date"
        public static string GetLogicalDayEnd1(string LRunDay)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayEnd( '" + string.Format("{0:yyyy-MM-dd}", LRunDay) + "')", Con);

            object SEDate = null;
            try
            {
                SEDate = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null)
                {
                    Con.Close();
                }
            }
            if (SEDate == null || Convert.IsDBNull(SEDate))
            {
                return string.Empty;
            }
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }
        #endregion

        internal static List<string> GetAllPlanNumbers()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("Select distinct PlanNo from SONA_ScheduleInfo_SAP2TPM", sqlConn);
            List<string> planNumberList = new List<string>();
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        planNumberList.Add(rdr["PlanNo"].ToString());
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
            return planNumberList;
        }

        internal static List<string> getpalnno(string fromdate, string todate, string machine)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> planNumberList = new List<string>();
            try
            {
                cmd = new SqlCommand("s_GetPlanNumber_SONA", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@MC", machine);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        planNumberList.Add(rdr["PlanNo"].ToString());
                    }
                }
                planNumberList.Insert(0, "All");
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
            return planNumberList;
        }

        #region ---- Tool wise cycle time -----
        internal static List<ToolWiseCycleTimeData> getVDGToolWiseCycleTimeDetails(string machine, string compid, string oprnum, string starttime, string endtime)
        {
            List<ToolWiseCycleTimeData> list = new List<ToolWiseCycleTimeData>();
            ToolWiseCycleTimeData data = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetToolwiseCycleTime]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 90;
                cmd.Parameters.AddWithValue("@MachineID", machine);
                //  cmd.Parameters.AddWithValue("@EndDate", compid);
                // cmd.Parameters.AddWithValue("@MC", oprnum);
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(starttime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Enddate", Util.GetDateTime(endtime).ToString("yyyy-MM-dd HH:mm:ss"));
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    ToolWiseCycleTimeData totalData = new ToolWiseCycleTimeData();
                    totalData.CycleStartTime = "Total";
                    while (rdr.Read())
                    {
                        data = new ToolWiseCycleTimeData();
                        data.CycleStartTime = rdr["CycleStart"].ToString();
                        data.CycleEndTime = rdr["CycleEnd"].ToString();
                        data.ToolNumber = rdr["ToolNumber"].ToString();
                        data.ToolTime = rdr["TotalTime"].ToString();
                        data.OperatingTime = rdr["OperatingTime"].ToString();
                        data.CuttingTime = rdr["CuttingTime"].ToString();

                        var valueSplit = data.ToolTime.Split(':');
                        data.ToolTimeTS = new TimeSpan(int.Parse(valueSplit[0]),
                                                         int.Parse(valueSplit[1]),
                                                         int.Parse(valueSplit[2]));
                        valueSplit = data.OperatingTime.Split(':');
                        data.OperatingTimeTS = new TimeSpan(int.Parse(valueSplit[0]),   
                                                         int.Parse(valueSplit[1]), 
                                                         int.Parse(valueSplit[2]));
                        valueSplit = data.CuttingTime.Split(':');
                        data.CuttingTimeTS = new TimeSpan(int.Parse(valueSplit[0]),
                                                         int.Parse(valueSplit[1]),
                                                         int.Parse(valueSplit[2]));
                        data.NonCuttingTimeTS = data.OperatingTimeTS.Subtract(data.CuttingTimeTS);
                        data.NonCuttingTime = data.NonCuttingTimeTS.ToString();


                        totalData.ToolTimeTS = totalData.ToolTimeTS.Add(data.ToolTimeTS);
                        totalData.OperatingTimeTS = totalData.OperatingTimeTS.Add(data.OperatingTimeTS);
                        totalData.CuttingTimeTS = totalData.CuttingTimeTS.Add(data.CuttingTimeTS);
                        totalData.NonCuttingTimeTS = totalData.NonCuttingTimeTS.Add(data.NonCuttingTimeTS);
                        data.ProgramBlock = rdr["ProgramBlock"].ToString();
                        list.Add(data);
                    }
                    list.Add(totalData);
                }
                else
                {
                    data = new ToolWiseCycleTimeData();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }

        #endregion

        #region -----Air Pressure Details -----
        internal static List<AirPressureEntity> getCockpitAirPressureDetails(string machine, string fromDateTime, string toDateTime)
        {
            List<AirPressureEntity> list = new List<AirPressureEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetEventTimeStamp]", conn);
                cmd.CommandTimeout = 450;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromTime", Util.GetDateTime(fromDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(toDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machineid", machine);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    int i = 1;
                    while (sdr.Read())
                    {
                        AirPressureEntity data = new AirPressureEntity();
                        data.SlNo = i.ToString();
                        data.MachineID = sdr["machineid"].ToString();
                        data.AirPressureLow = sdr["AirPressureLowTime"].ToString();
                        data.AirPressureRetained = sdr["AirPressureHighTime"].ToString();
                        data.LapsedTime = sdr["LapsedTime"].ToString();
                        list.Add(data);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        #endregion
        #region -----Spindle Runtime Details LG -----
        internal static List<SpindleRuntimeEntity> getCockpitSpindleRuntimeDetails(string machine, string fromDateTime, string toDateTime)
        {
            List<SpindleRuntimeEntity> list = new List<SpindleRuntimeEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_GetSpindleRuntimeInfo]", conn);
                cmd.CommandTimeout = 450;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fromTime", Util.GetDateTime(fromDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@endtime", Util.GetDateTime(toDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machineid", machine);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        SpindleRuntimeEntity data = new SpindleRuntimeEntity();
                        data.Machine = sdr["MachineID"].ToString();
                        data.RunTime = sdr["Runtime"].ToString();
                        data.Date = string.IsNullOrEmpty(sdr["shiftdate"].ToString())? sdr["shiftdate"].ToString():Util.GetDateTime(sdr["shiftdate"].ToString()).ToString("dd-MM-yyyy");
                        data.Shift = sdr["Shiftname"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        #endregion

        #region -------- COP Master Import ---------
        public static int deleteTempCOPMasterDetailsRecords()
        {
            int recordAffected = 0;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string cmdStr = System.String.Format("delete from ItemMaster");

                SqlCommand command = new SqlCommand(cmdStr, sqlConn);
                command.CommandType = System.Data.CommandType.Text;
                recordAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return recordAffected;
        }
        public static string saveImportedCOPDataToTempTable(DataTable dt, bool chkUpdateComponents)
        {
            string result = "";
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            string conString = WebConfigurationManager.ConnectionStrings[HttpContext.Current.Session["connectionString"].ToString()].ToString();
            try
            {								  
                bulkCopy = new SqlBulkCopy(conString);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[ItemMaster]";

                //			ItemNo	Iteminterfaceid	Itemdescription	customerid	Operationno	Opndescription	CNC M/C	Price	Drawingno	Opninterfaceid	LoadUnloadTime	CycleTime	SubOperations	StdSetupTime	MachiningTimeThreshold	TargetPercent	LoadUnloadTimeThreshold	SCIThreshold	DCLThreshold	ID	FinishedOperation	MinLoadUnloadThreshold	IncentiveTime																			

                bulkCopy.ColumnMappings.Add("ID", "ID");
                bulkCopy.ColumnMappings.Add("ItemNo", "ItemNo");
                bulkCopy.ColumnMappings.Add("Iteminterfaceid", "Iteminterfaceid");
                bulkCopy.ColumnMappings.Add("Itemdescription", "Itemdescription");
                bulkCopy.ColumnMappings.Add("Operationno", "Operationno");
                bulkCopy.ColumnMappings.Add("Opninterfaceid", "Opninterfaceid");
                bulkCopy.ColumnMappings.Add("Opndescription", "Opndescription");

                bulkCopy.ColumnMappings.Add("CNC M/C", "CNC M/C");

                bulkCopy.ColumnMappings.Add("MachiningTime", "MachiningTime");
                bulkCopy.ColumnMappings.Add("LoadUnloadTime", "LoadUnloadTime");
                bulkCopy.ColumnMappings.Add("CycleTime", "CycleTime");


                bulkCopy.ColumnMappings.Add("MachiningTimeThreshold", "MachiningTimeThreshold");
                bulkCopy.ColumnMappings.Add("LoadUnloadTimeThreshold", "LoadUnloadTimeThreshold");

                bulkCopy.ColumnMappings.Add("StdSetupTime", "StdSetupTime");
                bulkCopy.ColumnMappings.Add("Price", "Price");
                bulkCopy.ColumnMappings.Add("Drawingno", "Drawingno");
                bulkCopy.ColumnMappings.Add("SubOperations", "SubOperations");
                bulkCopy.ColumnMappings.Add("TargetPercent", "TargetPercent");
                bulkCopy.ColumnMappings.Add("customerid", "customerid");
                bulkCopy.ColumnMappings.Add("SCIThreshold", "SCIThreshold");
                bulkCopy.ColumnMappings.Add("DCLThreshold", "DCLThreshold");

                bulkCopy.ColumnMappings.Add("FinishedOperation", "FinishedOperation");
                bulkCopy.ColumnMappings.Add("MinLoadUnloadThreshold", "MinLoadUnloadThreshold");
                bulkCopy.ColumnMappings.Add("IncentiveTime", "IncentiveTime");
                if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    bulkCopy.ColumnMappings.Add("StdTestPressure", "StdTestPressure");
                    bulkCopy.ColumnMappings.Add("StdHoldingTime", "StdHoldingTime");
                }
                if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    bulkCopy.ColumnMappings.Add("PartFamily", "PartFamily");
                }
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.ItemMaster .", e.RowsCopied));
                };

                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
                result = getPumpModelImportIssueDetails(chkUpdateComponents);
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Logger.WriteErrorLog(string.Format("Exception in ProcessAlarmFile() method. Message :{0}", ex.ToString()));
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();

            }
            return result;
        }
        public static string getPumpModelImportIssueDetails(bool chkUpdateComponents)
        {
            string result = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand command = new SqlCommand("[dbo].[S_GetComponentDetails]", sqlConn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandTimeout = 360;
            if (chkUpdateComponents == true)
            {
                command.Parameters.Add("@Flag", SqlDbType.Int).Value = 1;
            }
            else
            {
                command.Parameters.Add("@Flag", SqlDbType.Int).Value = 0;
            }
            try
            {
                sdr = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }
            return result;
        }
        public static DataTable getCompOperationMasterData(out DataTable dtCOP)
        {
            DataTable dtComp = new DataTable();
            dtCOP = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                string cmdStr = System.String.Format("select * from componentinformation; select * from componentoperationpricing");
                SqlCommand command = new SqlCommand(cmdStr, sqlConn);
                command.CommandType = System.Data.CommandType.Text;
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    dtComp.Load(sdr);
                    dtCOP.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return dtComp;
        }
        #endregion
        #region----------ToolWise_Vulkan---
        internal static List<ToolWiseVulkan> GetToolWises_Vulkan(string machine, string starttime, string endtime, out ToolWiseChart toolWiseChart)
        {
            toolWiseChart = new ToolWiseChart();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<ToolWiseVulkan> list = new List<ToolWiseVulkan>();
            List<string> Category = new List<string>();
            List<string> slNo = new List<string>();
            List<ToolWiseChartData> Data = new List<ToolWiseChartData>();
            ToolWiseChartData chartData = null;
            ToolWiseVulkan data = null;
            SqlCommand cmd = null;
            try
            {
                //cmd = new SqlCommand("[dbo].[s_GetToolUsageGraphReport]", con);
                cmd = new SqlCommand("[dbo].[s_GetToolUsageGraphReport ]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machine", machine);
                //  cmd.Parameters.AddWithValue("@EndDate", compid);
                // cmd.Parameters.AddWithValue("@MC", oprnum);
                cmd.Parameters.AddWithValue("@CycleStart", Util.GetDateTime(starttime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@CycleEnd", Util.GetDateTime(endtime).ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    //ToolWiseVulkan totaldata = new ToolWiseVulkan();
                    //totaldata.CycleStartTime = "Total";
                    int i = 1;
                    while (sdr.Read())
                    {

                        data = new ToolWiseVulkan();
                        data.Tool = sdr["Tool"].ToString();
                        data.StartTime = sdr["starttime"].ToString();
                        data.EndTime = sdr["endtime"].ToString();
                        data.Ideal = sdr["frmtIdealToolUsage"].ToString();
                        data.Actual = sdr["frmtToolUsage"].ToString();
                        data.ActualToolUsage = sdr["ActualToolUsage"].ToString();
                        data.slNo = i.ToString();

                        list.Add(data);


                        chartData = new ToolWiseChartData();
                        chartData.y = Convert.ToDouble(sdr["ActualToolUsage"].ToString());
                        chartData.ToolName = sdr["tool"].ToString();
                        Data.Add(chartData);


                        Category.Add(i.ToString() + "-" + data.Tool);

                        toolWiseChart.Data = Data;
                        toolWiseChart.Category = Category;
                        i++;
                    }

                }
                else
                {
                    data = new ToolWiseVulkan();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) { sdr.Close(); sdr.Dispose(); }
                if (con != null) { con.Close(); con.Dispose(); }
            }
            return list;
        }
        #endregion

        internal static object GetNonMachineLst_PrecisionEngg(string plantID)
        {
            SqlDataReader sdr = null;
            List<string> MachLst = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select mi.Machineid from plantmachine pm inner join machineinformation mi on pm.Machineid = mi.Machineid and 
	TpmTrakEnabled=0 and (pm.plantid=@PlantId or @PlantId='') order by mi.Machineid", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantId", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MachLst.Add(sdr["Machineid"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return MachLst;
        }

        internal static DataTable GetProductionDownGridData(string fromDate, string toDate, string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("s_GetCockpitProductionAndDownData_eshopx", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@StartTime", fromDate);
                cmd.Parameters.AddWithValue("@EndTime", toDate);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetProductionDownGridData: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
    }
}