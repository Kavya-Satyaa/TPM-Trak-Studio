using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Elmah;
using Web_TPMTrakDashboard;

namespace Web_TPMTrakDashboard.EnergyModule.Models
{
    public class DataBaseAccess
    {
        internal static List<string> GetAllShift()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> shiftList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"
select * from shiftDetails where running = 1 order by FromTime asc", sqlConn);
                shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    shiftList.Add(rdr["shiftName"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return shiftList;
        }

        //internal static UserDetails GetEmployeeDetails(string value)
        //{
        //    UserDetails userDetails = new UserDetails();
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    string query = @"select Name,upassword,isadmin from [employeeinformation] where [Employeeid]=@emp";
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@emp", value);
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            userDetails.UserID = rdr["Name"].ToString();
        //            userDetails.Password = rdr["upassword"].ToString();
        //            if (rdr["isadmin"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
        //                userDetails.IsAdmin = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //    finally
        //    {
        //        if (conn != null) conn.Close();
        //    }
        //    return userDetails;
        //}

        internal static string GetLogicalDay(string selectedTime)
        {
            string list = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select [dbo].[f_GetLogicalDayStart](' " + selectedTime + "') as logicalDay";
                cmd = new SqlCommand(sqlQuery, conn);
                //cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["logicalDay"]))
                        {
                            list = DateTime.Parse((sdr["logicalDay"]).ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                else
                {
                    list = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving Machines - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }


        internal static string GetLogicalDayEnd(string selectedTime)
        {
            string list = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select [dbo].[f_GetLogicalDayEnd](' " + selectedTime + "') as logicalDay";
                cmd = new SqlCommand(sqlQuery, conn);
                //cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["logicalDay"]))
                        {
                            list = DateTime.Parse((sdr["logicalDay"]).ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                else
                {
                    list = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving Machines - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }


        internal static List<string> GetAllMachines(string plantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            try
            {
                cmd = new SqlCommand(@"s_GetLookups", sqlConn);

                if (!string.IsNullOrEmpty(plantId))
                {
                    if (plantId.Equals("All")) plantId = string.Empty;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", "Machine");
                    cmd.Parameters.AddWithValue("@filter", plantId);
                }
                else
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", "Machine");
                }

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    machineList.Add(rdr["machineId"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return machineList;
        }
        internal static List<string> GetAllEM_Machines(string PlantID = "", string MachineType = "")
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            string Query = string.Empty;
            PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID;
            switch (PlantID)
            {
                case "":
                    Query = @"select distinct MachineId from EM_Machineinformation where MachineType=@MachineType";
                    break;
                default:
                    Query = @"select distinct MI.MachineId from EM_Machineinformation MI
                                inner join EM_PlantMachine PM on PM.MachineID=MI.MachineId
                                where PM.PlantID = @PlantID and MI.MachineType = @MachineType";
                    break;

            }
            try
            {
                cmd = new SqlCommand(Query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("PlantID", PlantID);
                cmd.Parameters.AddWithValue("@MachineType", MachineType);


                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    machineList.Add(rdr["MachineId"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return machineList;
        }
        internal static List<string> GetAllEM_Machines(string PlantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            string Query = string.Empty;
            //Query = @"select distinct MachineId from EM_Machineinformation ";
            Query = @"select distinct m.MachineId from EM_Machineinformation  m
left join EM_PlantMachine em on em.MachineID=m.MachineId
where (em.PlantID = @plantid or isnull(@plantid,'')='')";
            try
            {
                cmd = new SqlCommand(Query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plantid", PlantID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    machineList.Add(rdr["MachineId"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return machineList;
        }
        internal static List<string> GetAllEM_Machines_Kiswok(string PlantID, string CellID, string Param, string MachineType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            string Query = string.Empty;
            List<string> allCellIDs = GetCellIDs("");
            //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            if (allCellIDs.Count > 0)
            {
                if (Param == "")
                {
                    Query = @"select distinct m.MachineId from EM_Machineinformation  m
inner join EM_PlantMachine em on em.MachineID=m.MachineId
inner join PlantMachineGroups pm on pm.MachineID=m.MachineId
where (groupid = @groupid or isnull(@groupid,'')='')
and (em.PlantID = @plantid or isnull(@plantid,'')='')";
                }
                else
                {
                    Query = @"select distinct m.MachineId from EM_Machineinformation  m
inner join EM_PlantMachine em on em.MachineID=m.MachineId
inner join PlantMachineGroups pm on pm.MachineID=m.MachineId
where (groupid = @groupid or isnull(@groupid,'')='')
and (em.PlantID = @plantid or isnull(@plantid,'')='')
and machinetype=@machineType and IsDashboardEnabled=1";
                }
                if (MachineType == "Non-Machine EM")
                {

                    Query = @"select distinct Machineid from EM_Machineinformation where MachineType = 'Non-Machine EM' and IsDashboardEnabled=1";
                }
            }
            else
            {
                Query = @"select distinct MachineId from EM_Machineinformation where IsDashboardEnabled=1";
            }
            try
            {
                cmd = new SqlCommand(Query, sqlConn);
                cmd.CommandType = CommandType.Text;
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                if (allCellIDs.Count > 0)
                {
                    if (Param != "")
                    {
                        cmd.Parameters.AddWithValue("@machineType", MachineType);
                    }
                    cmd.Parameters.AddWithValue("@plantid", PlantID);
                    cmd.Parameters.AddWithValue("@groupid", CellID);
                }
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    machineList.Add(rdr["MachineId"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return machineList;
        }
        internal static DataTable GetDataForDateShift(string dateVal, string shiftVal, bool isLiveData, string MachineType, string Plant, string Cell)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            if (MachineType == "Non-Machine EM")
            {
                Plant = ""; Cell = "";
            }
            try
            {
                Logger.WriteDebugLog("s_GetEnergyData procedure call startTime" + " - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (dateVal == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    sqlQuery = "s_GetEnergyData";
                }
                else
                {
                    sqlQuery = "S_GetAggEnergyData";
                }
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                //if (shiftVal.Equals("First"))
                //{
                //    shiftVal = "A";
                //}
                //else if (shiftVal.Equals("Second"))
                //{
                //    shiftVal = "B";
                //}
                //else if (shiftVal.Equals("Third"))
                //{
                //    shiftVal = "C";
                //}

                cmd.Parameters.AddWithValue("@dDate", dateVal);
                if (shiftVal.Equals("All"))
                {
                    cmd.Parameters.AddWithValue("@Shift", "");
                    cmd.Parameters.AddWithValue("@Parameter", "Day");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Shift", shiftVal);
                    cmd.Parameters.AddWithValue("@Parameter", "Shift");
                }
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                cmd.Parameters.AddWithValue("@GroupID", Cell);
                cmd.Parameters.AddWithValue("@View", "Technodashboard");
                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                if (isLiveData)
                {
                    cmd.Parameters.AddWithValue("@HistoryLive", "Live");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HistoryLive", "");
                }

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    table.Load(reader);
                    table.AcceptChanges();
                    reader.Close();
                }
                Logger.WriteDebugLog("s_GetEnergyData procedure call EndTime" + " - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return table;
        }
        internal static List<GridSettings> GetGridInformation(string Type)
        {
            List<GridSettings> gridSettings = new List<GridSettings>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            string query = @"select * from CockpitDefaults where Parameter=@Parameter order by ValueInInt asc";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Parameter", Type);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    GridSettings grid = new GridSettings();
                    grid.ColumnName = rdr["ValueInText"].ToString();
                    grid.ColumnText = rdr["ValueInText2"].ToString();
                    grid.SortOrder = rdr["ValueInInt"].ToString();
                    if (rdr["ValueInBool"].ToString().Equals("0"))
                        grid.Visibility = false;
                    else
                        grid.Visibility = true;
                    gridSettings.Add(grid);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return gridSettings;
        }
        internal static List<string> GetAllPlants()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            List<string> plantid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"s_GetLookups", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Plant");
                plantid.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    plantid.Add(rdr["plantid"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return plantid;
        }
        internal static DataTable GetEnergyTargetDetails(string PlantID, string GroupID, string MachineType)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                if (PlantID == "All")
                {
                    PlantID = "";
                }
                //if (MachineType == "Non-Machine EM")
                //{

                //    sqlQuery = @"select distinct Machineid from EM_Machineinformation where MachineType = 'Non-Machine EM' and IsDashboardEnabled=1";
                //}
                //else
                //{
                sqlQuery = @"select M.Machineid,ET.[Target] from EM_PlantMachine PM
 inner join EM_Machineinformation M on PM.MachineID= M.MachineId 
 left join PlantMachineGroups PMG on PMG.MachineID=PM.MachineID
left outer join Energy_Target ET on  M.MachineId = ET.MachineID where (PM.PlantID=@plantID or @plantid='')
and (PMG.GroupID=@GroupID or @GroupID='') and machinetype=@machineType and IsDashboardEnabled=1";
                //}
                //                sqlQuery = @"select M.Machineid,ET.[Target] from EM_PlantMachine PM
                // inner join EM_Machineinformation M on PM.MachineID= M.MachineId 
                //left outer join Energy_Target ET on  M.MachineId = ET.MachineID where (PM.PlantID=@plantID or @plantid='')";
                //                sqlQuery = @"select M.Machineid,ET.[Target] from EM_PlantMachine PM
                // inner join EM_Machineinformation M on PM.MachineID= M.MachineId 
                // left join PlantMachineGroups PMG on PMG.MachineID=PM.MachineID
                //left outer join Energy_Target ET on  M.MachineId = ET.MachineID where (PM.PlantID=@plantID or @plantid='')
                //and (PMG.GroupID=@GroupID or @GroupID='')";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@plantid", PlantID);
                cmd.Parameters.AddWithValue("@machineType", MachineType);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                table.Load(reader);
                table.AcceptChanges();
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return table;
        }
        internal static void SaveEnergyTargetDetails(string MachineID, string Target, out bool isSuccessfull)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            isSuccessfull = false;
            try
            {
                sqlQuery = @"if not exists(select * from Energy_Target  where MachineID=@MachineID)
                    BEGIN
                    insert into Energy_Target(MachineID,Target)
                    Values(@MachineID,@Target)
                    END
                else
                    BEGIN
                    update Energy_Target set [Target]=@Target where MachineID=@MachineID
                    END";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                if (Target.Equals(""))
                    cmd.Parameters.AddWithValue("@Target", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Target", Target);

                cmd.CommandTimeout = 120;


                int ret = cmd.ExecuteNonQuery();
                isSuccessfull = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }
        internal static int SaveGridColumnsSettingsVals(string parameter, string headerText, string gridHeaderVal, bool checkState, int sortOrder)
        {
            int count = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            //string query = @"if not exists(select * from CockpitDefaults where Parameter=@Parameter and ValueInText=@ValueInText)
            //                    begin
            //                        INSERT INTO [dbo].[CockpitDefaults]([Parameter],[ValueInText] ,[ValueInText2] ,[ValueInBool],UpdatedTS) VALUES (@Parameter,@ValueInText,@ValueInText2,@ValueInBool,@UpdatedTS)
            //                    end
            //                else
            //                    begin
            //                        update CockpitDefaults set ValueInText2=@ValueInText2, ValueInBool=@ValueInBool,UpdatedTS=@UpdatedTS where Parameter=@Parameter and ValueInText=@ValueInText
            //                    end";
            string query = @"if not exists(select * from CockpitDefaults where Parameter=@Parameter and ValueInText=@ValueInText)
begin
    INSERT INTO [dbo].[CockpitDefaults]([Parameter],[ValueInText] ,[ValueInText2] ,[ValueInBool],UpdatedTS,ValueInInt) VALUES (@Parameter,@ValueInText,@ValueInText2,@ValueInBool,@UpdatedTS,@ValueInInt)
end
else
begin
    update CockpitDefaults set ValueInText2=@ValueInText2, ValueInBool=@ValueInBool,UpdatedTS=@UpdatedTS,ValueInInt=@ValueInInt where Parameter=@Parameter and ValueInText=@ValueInText
end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@ValueInText", headerText);
                cmd.Parameters.AddWithValue("@ValueInText2", gridHeaderVal);
                cmd.Parameters.AddWithValue("@ValueInInt", sortOrder);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 120;
                if (checkState)
                {
                    cmd.Parameters.AddWithValue("@ValueInBool", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ValueInBool", 0);
                }

                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return count;
        }
        internal static DataTable GetMonthWiseEnergyDataToExport(string startDate, string endDate, string plantId, string selectedMachines, string shiftId, string param, string MachineType, int showTopEnergyConsumption, out DataTable dt2, out DataTable dt3)
        {
            DataTable dt = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            try
            {
                cmd = new SqlCommand(@"s_GetMonthwiseEnergyData", sqlConn); // s_GetMonthwiseEnergyData
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"param", param);
                cmd.Parameters.AddWithValue(@"startTime", startDate);
                cmd.Parameters.AddWithValue(@"endTime", endDate);
                cmd.Parameters.AddWithValue(@"machineId", selectedMachines);
                cmd.CommandTimeout = 120;
                if (plantId.Equals("All")) plantId = string.Empty;
                cmd.Parameters.AddWithValue(@"plantId", plantId);
                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                cmd.Parameters.AddWithValue("@IsChecked", showTopEnergyConsumption);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                    dt2.Load(rdr);
                    dt2.AcceptChanges();
                    dt3.Load(rdr);
                    dt3.AcceptChanges();
                    rdr.Close();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        internal static DataTable GetEnergyDataToExport(string startDate, string endDate, string plantId, string selectedMachines, string shiftId, string param, string MachineType, int showTopEnergyConsumption)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> machineList = new List<string>();
            try
            {
                cmd = new SqlCommand(@"S_GetEnergyCockpit_CuttingDetails", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@Startdate", startDate);
                cmd.Parameters.AddWithValue("@Enddate", endDate);
                cmd.Parameters.AddWithValue("@MachineId", selectedMachines);
                cmd.CommandTimeout = 120;
                if (plantId.Equals("All")) plantId = string.Empty;
                cmd.Parameters.AddWithValue("@PlantId", plantId);


                if (shiftId.Equals("All")) shiftId = string.Empty;
                cmd.Parameters.AddWithValue("@Shift", shiftId);
                //cmd.Parameters.AddWithValue(@"type", "technoreport");
                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                cmd.Parameters.AddWithValue("@IsChecked", showTopEnergyConsumption);
                cmd.CommandTimeout = 3000;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                    rdr.Close();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        internal static List<LiveDataCs> GetDataLiveData(string dayVal, string liveHistory, string viewParam, string Plant, string Cell, string MachineType)
        {
            List<LiveDataCs> liveDatas = new List<LiveDataCs>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "s_GetEnergyData";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue(@"dDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue(@"Shift", "");
                cmd.Parameters.AddWithValue(@"MachineID", "");
                cmd.Parameters.AddWithValue(@"PlantID", Plant);
                cmd.Parameters.AddWithValue("@GroupID", Cell);
                cmd.Parameters.AddWithValue(@"Parameter", dayVal);
                cmd.Parameters.AddWithValue(@"View", viewParam);
                cmd.Parameters.AddWithValue(@"HistoryLive", liveHistory);
                //cmd.Parameters.AddWithValue(@"MachineType", "Non-Machine EM");
                cmd.Parameters.AddWithValue(@"MachineType", MachineType);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LiveDataCs liveData = new LiveDataCs();
                        liveData.Machineid = reader["machineid"].ToString();
                        liveData.DateTime = reader["LastArrivalTime"].ToString();
                        liveData.VLN_R = reader["V1"].ToString();
                        liveData.VLN_Y = reader["V2"].ToString();
                        liveData.VLN_B = reader["V3"].ToString();
                        liveData.VLN_R_Y = reader["V4"].ToString();
                        liveData.VLN_Y_B = reader["V5"].ToString();
                        liveData.VLN_B_R = reader["V6"].ToString();
                        liveData.R_AMP = reader["AR"].ToString();
                        liveData.Y_AMP = reader["AY"].ToString();
                        liveData.B_AMP = reader["AB"].ToString();
                        liveData.PowerFactor = reader["PF"].ToString();
                        liveData.Kw = reader["KW"].ToString();
                        liveData.Kwh = reader["KWH"].ToString();
                        liveData.LastArrival_TS = reader["LastArrivalTime"].ToString();

                        liveDatas.Add(liveData);
                    }
                }
                table.Load(reader);
                table.AcceptChanges();
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return liveDatas;
        }
        internal static DataTable GetEnergyData(string fromDateTime, string toDateTime, string machineId)
        {
            List<DateTime> timeStamps = new List<DateTime>();
            DataTable dt = new DataTable();

            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = "";
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["EnergyModuleWithTPM_EM_DB"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    query = "Select gtime,watt,AmpereR,AmpereY, AmpereB,pf,Volt1,Volt2,Volt3,Volt4,Volt5,Volt6 from [TPM_EM].[dbo].tcs_energyconsumption where gtime >= @gtimeStart  and gtime <= @gtimeEnd  and machineID =@machineId order by gtime ";
                }
                else
                {
                    query = "Select gtime,watt,AmpereR,AmpereY, AmpereB,pf,Volt1,Volt2,Volt3,Volt4,Volt5,Volt6 from tcs_energyconsumption where gtime >= @gtimeStart  and gtime <= @gtimeEnd  and machineID =@machineId order by gtime ";
                }

                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@gtimeStart", fromDateTime);//"2015-01-20 6:00:00");//
                cmd.Parameters.AddWithValue("@gtimeEnd", toDateTime);//"2015-01-20 14:00:00");//
                cmd.Parameters.AddWithValue("@Machineid", machineId);
                cmd.CommandTimeout = 120;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }

                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        internal static List<HourwiseMonitoringData> GetHourWiseEnergyData(string machineId, string dateVal, string shiftVal, bool liveData, string MachineType)
        {
            List<GridSettings> gridSettings = DataBaseAccess.GetGridInformation(MachineType);
            List<HourwiseMonitoringData> hourwiseMonitoringDatas = new List<HourwiseMonitoringData>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                Logger.WriteDebugLog("s_GetEnergyData procedure call startTime" + " - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (dateVal == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    sqlQuery = "s_GetEnergyData";
                }
                else
                {
                    sqlQuery = "S_GetAggEnergyData";
                }
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue(@"dDate", dateVal);
                cmd.Parameters.AddWithValue(@"Shift", shiftVal);
                cmd.Parameters.AddWithValue(@"Parameter", "Hour");
                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                cmd.Parameters.AddWithValue(@"MachineID", machineId);
                cmd.Parameters.AddWithValue(@"PlantID", "");
                cmd.Parameters.AddWithValue(@"View", "Technodashboard");

                if (liveData)
                {
                    cmd.Parameters.AddWithValue(@"HistoryLive", "Live");
                }
                else
                {
                    cmd.Parameters.AddWithValue(@"HistoryLive", "");
                }

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                Logger.WriteDebugLog("s_GetEnergyData procedure call EndTime" + " - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HourwiseMonitoringData data = new HourwiseMonitoringData();
                        data.ShiftHourID = reader["ShiftHourID"].ToString();
                        data.Volt1 = reader["Volt1"].ToString();
                        data.Volt2 = reader["Volt2"].ToString();
                        data.Volt3 = reader["Volt3"].ToString();
                        data.PF = reader["PowerFactor"].ToString();
                        data.Components = reader["ProductionCount"].ToString();
                        data.UtilisedTime = reader["ProductionTime"].ToString();
                        data.Energy = reader["Energy"].ToString();
                        data.Cost = reader["Cost"].ToString();
                        data.Target = reader["Target"].ToString();

                        data.Volt4 = reader["Volt4"].ToString();
                        data.Volt5 = reader["Volt5"].ToString();
                        data.Volt6 = reader["Volt6"].ToString();

                        data.AmpereR = reader["AmpereR"].ToString();
                        data.AmpereY = reader["AmpereY"].ToString();
                        data.AmpereB = reader["AmpereB"].ToString();

                        var result = gridSettings.Where(k => k.ColumnName.Equals("ProductionTime", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (result != null)
                        {
                            data.ProductionTime = reader["ProductionTime"].ToString();
                        }
                        result = gridSettings.Where(k => k.ColumnName.Equals("DownTime", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (result != null)
                        {
                            data.DownTime = reader["DownTime"].ToString();
                        }
                        result = gridSettings.Where(k => k.ColumnName.Equals("ProdTime_KWH", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (result != null)
                        {
                            data.ProdTime_KWH = reader["ProdTime_KWH"].ToString();
                        }
                        result = gridSettings.Where(k => k.ColumnName.Equals("DownTime_KWH", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (result != null)
                        {
                            data.DownTime_KWH = reader["DownTime_KWH"].ToString();
                        }
                        hourwiseMonitoringDatas.Add(data);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return hourwiseMonitoringDatas;
        }
        internal static string GetEnergySourceFor_EMData()
        {
            string EnergySource = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"SELECT ValueInText FROM ShopDefaults WHERE Parameter='EnergySource'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        EnergySource = sdr["ValueInText"].ToString();
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
            return EnergySource;
        }

        #region EM_Master

        internal static List<string> GetAllMachineInterfaceID()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Entity = new List<string>();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string sqlQuery = @"select distinct InterfaceID from machineinformation";
            try
            {
                cmd = new SqlCommand(sqlQuery, sqlConn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Entity.Add(rdr["InterfaceID"].ToString()); ;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return Entity;
        }

        internal static List<EnergyMachinemasterEntity> GetMachineData()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<EnergyMachinemasterEntity> Entity = new List<EnergyMachinemasterEntity>();
            SqlCommand cmd = null;
            SqlDataReader rdr = null; string sqlQuery = "";
            List<string> allCellIDs = GetCellIDs("");

            //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            if (allCellIDs.Count > 0)
            {
                sqlQuery = @"select MI.MachineId,MI.interfaceid,MI.IPAddress,MI.PortNo,MI.SortOrder,MI.IsEnabled,MI.MachineType,MI.IsDashboardEnabled,PM.PlantID,PMG.GroupID from EM_Machineinformation MI
                                inner join EM_PlantMachine PM on PM.MachineID=MI.MachineId 
								left join PlantMachineGroups PMG on PMG.MachineID=MI.MachineId";
            }
            else
            {
                sqlQuery = @"select MI.MachineId,MI.interfaceid,MI.IPAddress,MI.PortNo,MI.SortOrder,MI.IsEnabled,MI.MachineType,MI.IsDashboardEnabled,PM.PlantID from EM_Machineinformation MI
                                inner join EM_PlantMachine PM on PM.MachineID=MI.MachineId ";
            }
            try
            {
                cmd = new SqlCommand(sqlQuery, sqlConn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                    if (allCellIDs.Count > 0)
                    {
                        Entity.Add(new EnergyMachinemasterEntity
                        {
                            MachineID = rdr["MachineId"].ToString(),
                            PlantID = rdr["PlantID"].ToString(),
                            GroupID = rdr["GroupID"].ToString(),
                            MachineInterfaceID = rdr["interfaceid"].ToString(),
                            IPAddress = rdr["IPAddress"].ToString(),
                            PortNumber = rdr["PortNo"].ToString(),
                            IsEnabled = rdr["IsEnabled"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            SortOrder = rdr["SortOrder"].ToString(),
                            MachineType = rdr["MachineType"].ToString(),
                            IsDashboardEnabled = rdr["IsDashboardEnabled"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false
                        });
                    }
                    else
                    {
                        Entity.Add(new EnergyMachinemasterEntity
                        {
                            MachineID = rdr["MachineId"].ToString(),
                            PlantID = rdr["PlantID"].ToString(),
                            MachineInterfaceID = rdr["interfaceid"].ToString(),
                            IPAddress = rdr["IPAddress"].ToString(),
                            PortNumber = rdr["PortNo"].ToString(),
                            IsEnabled = rdr["IsEnabled"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            SortOrder = rdr["SortOrder"].ToString(),
                            MachineType = rdr["MachineType"].ToString(),
                            IsDashboardEnabled = rdr["IsDashboardEnabled"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return Entity;
        }

        internal static void SaveEnergyMachindData(string MachineID, string PlantID, string InterfaceID, string IPAddress, string PortNumber, bool Enabled, string SortOrder, string MachineType, bool DashboardEnabled)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null; string sqlQuery = "";
            List<string> allCellIDs = GetCellIDs("");
            //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            if (allCellIDs.Count > 0)
            {
                sqlQuery = @"if not exists(select * from EM_Machineinformation where MachineId = @MachineID and interfaceid = @InterfaceID)
begin
    insert into EM_Machineinformation(MachineId, interfaceid, IPAddress, PortNo, IsEnabled, SortOrder,MachineType,IsDashboardEnabled)
    values(@MachineID, @InterfaceID, @IPAddress, @PortNO, @Enabled, @SortOrder,@MachineType,@DashboardEnabled)
end
else
Begin
                update EM_Machineinformation set  IPAddress = @IPAddress, PortNo = @PortNO,MachineType=@MachineType, IsEnabled = @Enabled, SortOrder = @SortOrder,IsDashboardEnabled=@DashboardEnabled where MachineId = @MachineID and interfaceid = @InterfaceID
end
if not exists (select * from EM_PlantMachine where PlantID=@PlantID and MachineID=@MachineID)
begin
	insert into EM_PlantMachine(PlantID,MachineID)
	values(@PlantID,@MachineID)
end";
            }
            else
            {
                sqlQuery = @"if not exists(select * from EM_Machineinformation where MachineId = @MachineID and interfaceid = @InterfaceID)
begin
    insert into EM_Machineinformation(MachineId, interfaceid, IPAddress, PortNo, IsEnabled, SortOrder,MachineType,IsDashboardEnabled)
    values(@MachineID, @InterfaceID, @IPAddress, @PortNO, @Enabled, @SortOrder,@MachineType,@DashboardEnabled)
end
else
Begin
                update EM_Machineinformation set  IPAddress = @IPAddress, PortNo = @PortNO,MachineType=@MachineType, IsEnabled = @Enabled, SortOrder = @SortOrder,IsDashboardEnabled=@DashboardEnabled where MachineId = @MachineID and interfaceid = @InterfaceID
end
if not exists (select * from EM_PlantMachine where PlantID=@PlantID and MachineID=@MachineID)
begin
	insert into EM_PlantMachine(PlantID,MachineID)
	values(@PlantID,@MachineID)
end";
            }
            try
            {
                cmd = new SqlCommand(sqlQuery, sqlConn);
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                //if (allCellIDs.Count > 0)
                //{
                //    cmd.Parameters.AddWithValue("@GroupID", Cell);
                //}
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@InterfaceID", InterfaceID);
                cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                cmd.Parameters.AddWithValue("@PortNO", PortNumber);
                cmd.Parameters.AddWithValue("@Enabled", Enabled);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                cmd.Parameters.AddWithValue("@DashboardEnabled", DashboardEnabled);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static int DeleteEnergyMachindData(string MachineID, string InterfaceID, string PlantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null; string sqlQuery = "";
            int result = 0;
            sqlQuery = @"if exists(select * from EM_Machineinformation where MachineId = @MachineID and interfaceid = @InterfaceID)
                                    begin
                                       delete from EM_Machineinformation where MachineId = @MachineID and interfaceid = @InterfaceID
                                    end
                                if exists (select * from EM_PlantMachine where PlantID=@PlantID and MachineID=@MachineID)
                                    begin
	                                     delete from EM_PlantMachine where PlantID=@PlantID and MachineID=@MachineID
                                    end";
            try
            {
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@InterfaceID", InterfaceID);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return result;
        }

        #endregion

        #region ----------- Setting -------
        internal static int saveShopDefaultValueInText(string parameter, string valueInText)
        {
            int count = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            string query = @"if not exists(select * from ShopDefaults where Parameter=@Parameter)
                                begin
                                    INSERT INTO [dbo].[ShopDefaults]([Parameter],[ValueInText],UpdatedTS) VALUES (@Parameter,@ValueInText,@UpdatedTS)
                                end
                            else
                                begin
                                    update ShopDefaults set ValueInText=@ValueInText,UpdatedTS=@UpdatedTS where Parameter=@Parameter
                                end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@ValueInText", valueInText);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return count;
        }
        internal static List<ShopDefaultEntity> getShopDefaultValueSetting(string Param)
        {
            List<ShopDefaultEntity> list = new List<ShopDefaultEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            SqlDataReader sdr = null;
            string query = @"Select* from shopdefaults where Parameter in ('" +Param+ "')";
            try
            {
                cmd = new SqlCommand(query, conn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ShopDefaultEntity data = new ShopDefaultEntity();
                        data.Parameter = sdr["Parameter"].ToString();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        data.ValueInText2 = sdr["ValueInText2"].ToString();
                        data.ValueInInt = string.IsNullOrEmpty(sdr["ValueInInt"].ToString()) ? 0 : Convert.ToInt32(sdr["ValueInInt"].ToString());
                        list.Add(data);
                    }
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
            return list;
        }
        #endregion
        #region--------Default Launch Selection Settings----------------
        internal static string GetSelectedValues(string Param)
        {
            string Value = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from ShopDefaults where Parameter=@Parameter", con);
                cmd.Parameters.AddWithValue("@Parameter", Param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Value = sdr["ValueInText"].ToString();
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
            return Value;
        }
        internal static string SaveSelectedValues(string Param, string Value)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from ShopDefaults where Parameter=@Parameter)
begin
    INSERT INTO [dbo].[ShopDefaults]([Parameter],[ValueInText],UpdatedTS) VALUES (@Parameter,@ValueInText,@UpdatedTS)
	select 'Inserted' as SaveFlag
end
else
begin
    update ShopDefaults set ValueInText=@ValueInText,UpdatedTS=@UpdatedTS where Parameter=@Parameter
	select 'Updated' as SaveFlag
end
", con);
                cmd.Parameters.AddWithValue("@Parameter", Param);
                cmd.Parameters.AddWithValue("@ValueInText", Value);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
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
            return success;
        }
        #endregion
        internal static List<string> GetCellIDs(string Plant)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"SELECT distinct GroupID FROM PlantMachineGroups WHERE (PlantID=@PlantID OR ISNULL(@PlantID,'')='')", con);
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                list.Add("All");
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
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<ShiftTimings> GetShiftTimings()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<ShiftTimings> list = new List<ShiftTimings>();
            ShiftTimings data = null;
            try
            {
                cmd = new SqlCommand(@"select FromTime,ToTime,ShiftName from shiftdetails where Running=1", con);
                //cmd.Parameters.AddWithValue("@ShiftName", ShiftName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new ShiftTimings();
                        data.FromTime = sdr["FromTime"].ToString();
                        data.ToTime = sdr["ToTime"].ToString();
                        data.ShiftName = sdr["ShiftName"].ToString();
                        list.Add(data);
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
            return list;
        }
    }
}