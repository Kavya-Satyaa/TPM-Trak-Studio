using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.LnTOdisha.Model.LnTOdishaDTO;

namespace Web_TPMTrakDashboard.LnTOdisha.Model
{
    public class LnTOdishaDBAccess
    {
        internal static List<string> GetMachineInfoForPM()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> macdata = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct MachineID from machineinformation order by MachineID", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        macdata.Add(rdr["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return macdata;
        }

        #region --- PM MAster ------
        internal static List<string> getPMActivityCategoryLnT()
        {
            List<string> list = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from PMActivityCategory order by SortOrder", sqlConn);
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["Category"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<ActivityInfoLnTEntity> getPMActivityMasterDetails(string Frequency, string MachineId)
        {
            List<ActivityInfoLnTEntity> activityInfoList = new List<ActivityInfoLnTEntity>();
            ActivityInfoLnTEntity activityInfoData = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            int i = 0;
            try
            {
                //SqlCommand cmd = new SqlCommand(@"select row_number() OVER (ORDER BY main.ActivityID) AS SlNo, main.ActivityID,main.Activity,chield.Frequency,chield.FreqID, main.Filename,  main.MachineID  from ActivityMaster_MGTL main inner join ActivityFreq_MGTL chield on main.FreqID=chield.FreqID  where (chield.Frequency=@Frequency or @Frequency='') and main.MachineID=@MachineID", sqlConn);
                SqlCommand cmd = new SqlCommand(@"S_ActivityMaster_MGTL_LnT", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@Frequency", Frequency == "All" ? "" : Frequency));
                cmd.Parameters.Add(new SqlParameter("@MachineID", MachineId));
                cmd.Parameters.AddWithValue("@Param", "ActivityMaster_View");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        activityInfoData = new ActivityInfoLnTEntity();
                        activityInfoData.SerialNumber = (++i).ToString(); //rdr["SlNo"].ToString();
                        activityInfoData.MachineID = rdr["MachineID"].ToString();
                        activityInfoData.ActivityID = rdr["ActivityID"].ToString();
                        activityInfoData.Activity = rdr["Activity"].ToString();
                        activityInfoData.Frequency = rdr["Frequency"].ToString();
                        activityInfoData.FrequencyID = rdr["FreqID"].ToString();
                        activityInfoData.FileName = rdr["Filename"].ToString();
                        activityInfoData.ActivityHasFile = DataBaseAccess.IsActivityFileAvailableInDB(Convert.ToInt32(activityInfoData.ActivityID), MachineId);
                        activityInfoData.Shifts = rdr["ShiftID"].ToString();
                        activityInfoData.Criteria = rdr["Criteria"].ToString();
                        activityInfoData.Category = rdr["Category"].ToString();
                        activityInfoData.AllotedTime = rdr["AllotedTime"].ToString();
                        activityInfoList.Add(activityInfoData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return activityInfoList;
        }

        internal static void InsertPMActivtyMasterDetails(int activityID, string activity, string frequency, string filename, byte[] contents, bool hasFile, string machineID, string criteria, string shift, string category, string allotedTime, out bool isUpdated)
        {
            isUpdated = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();

            try
            {
                string Param1 = "";
                if (hasFile)
                {
                    //                  query = @"IF EXISTS(Select * from ActivityMaster_MGTL where ActivityID = @ActivityID and MachineID=@MachineID)
                    //  BEGIN
                    //   UPDATE ActivityMaster_MGTL set [Activity] = @Activity, [FreqID] = @FreqID,[Filename] = @Filename, [ActivityFile] = @ActivityFile, 
                    //ShiftID= @ShiftID, Criteria=@Criteria,Category=@Category, AllotedTime=@AllotedTime
                    //   where ActivityID = @ActivityID and MachineID=@MachineID
                    //  END
                    //  ELSE
                    //  BEGIN
                    //    Insert into ActivityMaster_MGTL([Activity], [FreqID],[Filename],[ActivityFile], [MachineID],ShiftID,Criteria,Category, AllotedTime) 
                    //    Values(@Activity, @FreqID,@Filename,@ActivityFile,@MachineID,@ShiftID,@Criteria,@Category,@AllotedTime)";
                    Param1 = "WithActivityFile";
                }
                else
                {
                    //                    query = @"IF EXISTS(Select * from ActivityMaster_MGTL where ActivityID = @ActivityID and MachineID=@MachineID)
                    //BEGIN
                    //	UPDATE ActivityMaster_MGTL set [Activity] = @Activity, [FreqID] = @FreqID,ShiftID=@ShiftID,Criteria=@Criteria,Category=@Category, AllotedTime=@AllotedTime 
                    //	where ActivityID = @ActivityID and MachineID=@MachineID
                    //END
                    //ELSE
                    //BEGIN
                    //	Insert into ActivityMaster_MGTL([Activity], [FreqID], [MachineID],ShiftID,Criteria,Category, AllotedTime) Values(@Activity, @FreqID,@MachineID,@ShiftID,@Criteria,@Category,@AllotedTime) 
                    Param1 = "WithOutActivityFile";
                    //END";
                }

                SqlCommand cmd = new SqlCommand("S_ActivityMaster_MGTL_LnT", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@MachineID", machineID));
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityID));
                cmd.Parameters.Add(new SqlParameter("@Activity", activity));
                cmd.Parameters.Add(new SqlParameter("@FreqID", Convert.ToInt32(frequency)));
                cmd.Parameters.Add(new SqlParameter("@Filename", filename));
                if (hasFile)
                {
                    cmd.Parameters.Add(new SqlParameter("@ActivityFile", contents));
                }
                cmd.Parameters.Add(new SqlParameter("@ShiftID", shift));
                cmd.Parameters.Add(new SqlParameter("@Criteria", criteria));
                cmd.Parameters.Add(new SqlParameter("@Category", category));
                cmd.Parameters.Add(new SqlParameter("@AllotedTime", allotedTime));
                cmd.Parameters.AddWithValue("@Param", "ActivityMaster_Save");
                cmd.Parameters.AddWithValue("@Param1", Param1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    isUpdated = true;
                else
                    isUpdated = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                isUpdated = false;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static void DeletePMActivityMasterDetails(int activityID, string machineID, out bool isDeleted)
        {
            isDeleted = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"Delete from [dbo].[ActivityMaster_MGTL] where ActivityID = @ActivityID and MachineID=@MachineID";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ActivityID", activityID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                isDeleted = false;
                Logger.WriteErrorLog("Error in Deleting Grid Data From [dbo].[ActivityMaster_MGTL] - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static void InsertPMMasterImportData(DataTable dt)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlBulkCopy sqlBulkCopy = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from temp_ActivityMaster_MGTL", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

                sqlBulkCopy = new SqlBulkCopy(conn);
                sqlBulkCopy.DestinationTableName = "temp_ActivityMaster_MGTL";
                foreach (DataColumn col in dt.Columns)
                {
                    sqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                sqlBulkCopy.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (sqlBulkCopy != null) sqlBulkCopy.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void BulkInsertPMActivityData()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("S_Import_ActivityMasterData_LnT", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
                //cmd.Parameters.AddWithValue("",);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        #endregion

        #region --- PM Category Master ---
        internal static DataTable GetPMCategoryMasterData()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select Category as PMCategoryName,SortOrder from PMActivityCategory order by SortOrder", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetPMCategoryMasterData: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static string SavePMCateogryMasterData(string CategoryName, string SortOrder)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string isSaved = "";
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from PMActivityCategory where Category=@CategoryName)
begin
	insert into PMActivityCategory(Category, SortOrder) values(@CategoryName, @SortOrder)
end
else
begin
	Update PMActivityCategory set SortOrder = @SortOrder where Category = @CategoryName
end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                isSaved = cmd.ExecuteNonQuery() > 0 ? "Inserted/Updated" : "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SavePMCateogryMasterData: {ex.Message}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isSaved;
        }
        internal static string DeletePMCategoryMaster(string CategoryName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string IsDeleted = "";
            try
            {
                cmd = new SqlCommand(@"", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                IsDeleted = cmd.ExecuteNonQuery() > 0 ? "Deleted" : "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"DeletePMCategoryMaster: {ex.Message}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return IsDeleted;
        }
        #endregion

        #region ----PM Generation yearly ----
        internal static DataTable getPMGenerationYearlySummaryDetails(string year, string Month, string machine, out DataTable dt_Status)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            dt_Status = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"S_GetPMGeneration", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@YearNo", year);
                cmd.Parameters.AddWithValue("@MachineID", (machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine));
                cmd.Parameters.AddWithValue("@MonthNo", Month);
                cmd.Parameters.AddWithValue("@Param", "PM_Generation_View");
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt_Status.Load(rdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        internal static string insertPMGenerationYearlySummaryDetails(PMGenerationMonthEntity data)
        {
            string result = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"S_GetPMGeneration", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.Machine);
                cmd.Parameters.AddWithValue("@YearNo", data.Year);
                cmd.Parameters.AddWithValue("@MonthNo", data.MonthNo);
                cmd.Parameters.AddWithValue("@PMDate", string.IsNullOrEmpty(data.PlanDate) ? "" : Util.GetDateTime(data.PlanDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PMType", data.PlanStatus);
                cmd.Parameters.AddWithValue("@Param", "PM_Generation_Save");
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = rdr["SaveFlag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return result;
        }
        #endregion

        #region ----- PM Generation View Screen -----
        internal static DataTable getPMGenerationActivityDetails(string year, string from, string to, string machineId)
        {
            DataTable dt = new DataTable();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[S_GetActivityMasterYearlyData_MGTL]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@SDate", string.IsNullOrEmpty(from) ? "" : Util.GetDateTime(from).ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@NewDate", string.IsNullOrEmpty(to) ? "" : Util.GetDateTime(to).ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@Param", "View");

                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
        #endregion

        #region ---- PM Transaction ------
        internal static DataTable getActivityTransactionData(string machineid, string fromDate, string toDate, out DataTable dt_Remarks, out DataTable dtHeaderData)
        {
            DataTable dt = new DataTable();
            dtHeaderData = new DataTable();
            dt_Remarks = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("S_GetPMTransactionScreen", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@FDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@TDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "PMTransactionScreen_View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    if (dt.Columns.IndexOf("SaveFlag") == -1)
                    {
                        dt_Remarks.Load(sdr);
                        dtHeaderData.Load(sdr);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }
        internal static DataTable getActivityFileInformationDetails(string machineid, string frequency)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from PMActivityFileInfo_Cumi where MachineID=@MachineID and Frequency=@Frequency", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@Frequency", frequency);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }
        internal static DataTable getActivityFileInformationDetails_LnT(string machineid)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from ActivityMaster_MGTL where MachineID=@MachineID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }

        internal static List<ActivityInfoEntity> GetAllActivityForGrid_LnT(string MachineId)
        {
            List<ActivityInfoEntity> activityInfoList = new List<ActivityInfoEntity>();
            ActivityInfoEntity activityInfoData = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select row_number() OVER (ORDER BY main.ActivityID) AS SlNo, main.ActivityID,main.Activity,chield.Frequency,chield.FreqID, main.Filename,  main.MachineID,main.ShiftID,main.Criteria,main.Category  
from ActivityMaster_MGTL main inner join ActivityFreq_MGTL chield on main.FreqID=chield.FreqID 
and main.MachineID=@MachineID", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@MachineID", MachineId));
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        activityInfoData = new ActivityInfoEntity();
                        activityInfoData.SerialNumber = rdr["SlNo"].ToString();
                        activityInfoData.MachineID = rdr["MachineID"].ToString();
                        activityInfoData.ActivityID = rdr["ActivityID"].ToString();
                        activityInfoData.Activity = rdr["Activity"].ToString();
                        activityInfoData.Frequency = rdr["Frequency"].ToString();
                        activityInfoData.FrequencyID = rdr["FreqID"].ToString();
                        activityInfoData.FileName = rdr["Filename"].ToString();
                        activityInfoData.ActivityHasFile = DataBaseAccess.IsActivityFileAvailableInDB(Convert.ToInt32(activityInfoData.ActivityID), MachineId);
                        activityInfoData.Shifts = rdr["ShiftID"].ToString();
                        activityInfoData.Criteria = rdr["Criteria"].ToString();
                        activityInfoData.Category = rdr["Category"].ToString();
                        activityInfoList.Add(activityInfoData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return activityInfoList;
        }
        internal static bool IsActivityFileAvailableInDB(int activityID, string machineID)
        {
            bool isFileAvailable = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                string query = @"SELECT * FROM ActivityMaster_MGTL WHERE ActivityID = @ActivityID and MachineID=@MachineID and TRIM(Filename)<>'' and TRIM(ActivityFile)<>''";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityID));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machineID));
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    isFileAvailable = true;
                }
                else
                {
                    isFileAvailable = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("IsActivityFileAvailableInDB: " + ex.Message);
                isFileAvailable = false;
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return isFileAvailable;
        }

        internal static List<ActivityInfoEntity> GetAllActivityForGrid(string MachineId)
        {
            List<ActivityInfoEntity> activityInfoList = new List<ActivityInfoEntity>();
            ActivityInfoEntity activityInfoData = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM ActivityMaster_MGTL WHERE MachineID=@MachineID ", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@MachineID", MachineId));
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        activityInfoData = new ActivityInfoEntity();
                        activityInfoData.SerialNumber = rdr["SlNo"].ToString();
                        activityInfoData.MachineID = rdr["MachineID"].ToString();
                        activityInfoData.ActivityID = rdr["ActivityID"].ToString();
                        activityInfoData.Activity = rdr["Activity"].ToString();
                        if (rdr["Filename"].ToString() != "" && rdr["ActivityFile"].ToString() != "")
                        {
                            activityInfoData.ActivityHasFile = true;
                        }
                        else
                        {
                            activityInfoData.ActivityHasFile = false;
                        }
                        activityInfoList.Add(activityInfoData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return activityInfoList;
        }
        internal static List<ActivityInfoEntity> GetAllActivityForGrid(string Frequency, string MachineId)
        {
            List<ActivityInfoEntity> activityInfoList = new List<ActivityInfoEntity>();
            ActivityInfoEntity activityInfoData = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                //SqlCommand cmd = new SqlCommand(@"select row_number() OVER (ORDER BY main.ActivityID) AS SlNo, main.ActivityID,main.Activity,chield.Frequency,chield.FreqID, main.Filename,  main.MachineID  from ActivityMaster_MGTL main inner join ActivityFreq_MGTL chield on main.FreqID=chield.FreqID  where (chield.Frequency=@Frequency or @Frequency='') and main.MachineID=@MachineID", sqlConn);
                SqlCommand cmd = new SqlCommand(@"select row_number() OVER (ORDER BY main.ActivityID) AS SlNo, main.ActivityID,main.Activity,chield.Frequency,chield.FreqID, main.Filename,  main.MachineID,main.ShiftID,main.Criteria,main.Category  
from ActivityMaster_MGTL main inner join ActivityFreq_MGTL chield on main.FreqID=chield.FreqID  where (chield.Frequency=@Frequency or @Frequency='') 
and main.MachineID=@MachineID", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@Frequency", Frequency == "All" ? "" : Frequency));
                cmd.Parameters.Add(new SqlParameter("@MachineID", MachineId));
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        activityInfoData = new ActivityInfoEntity();
                        activityInfoData.SerialNumber = rdr["SlNo"].ToString();
                        activityInfoData.MachineID = rdr["MachineID"].ToString();
                        activityInfoData.ActivityID = rdr["ActivityID"].ToString();
                        activityInfoData.Activity = rdr["Activity"].ToString();
                        activityInfoData.Frequency = rdr["Frequency"].ToString();
                        activityInfoData.FrequencyID = rdr["FreqID"].ToString();
                        activityInfoData.FileName = rdr["Filename"].ToString();
                        activityInfoData.ActivityHasFile = DataBaseAccess.IsActivityFileAvailableInDB(Convert.ToInt32(activityInfoData.ActivityID), MachineId);
                        activityInfoData.Shifts = rdr["ShiftID"].ToString();
                        activityInfoData.Criteria = rdr["Criteria"].ToString();
                        activityInfoData.Category = rdr["Category"].ToString();
                        activityInfoList.Add(activityInfoData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return activityInfoList;
        }
        #endregion

        #region ---- Checklist Print out ----
        internal static DataTable getChecklistTransacionPrintout(string machineid, string year, string month, out DataTable dt_HeaderDetails)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            dt_HeaderDetails = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("S_GetTransactionChecklist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@YearNo", year);
                cmd.Parameters.AddWithValue("@monthNO", month);
                cmd.Parameters.AddWithValue("@Param", "TransactionChecklist_View");
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (dt.Columns.IndexOf("SaveFlag") == -1)
                    dt_HeaderDetails.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }
        internal static string insertChecklistTransacionPrintout(string machineid, string year, string month, string scheduleDate, string tlentry, string crewdata)
        {
            string result = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"S_GetTransactionChecklist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@YearNo", year);
                cmd.Parameters.AddWithValue("@MonthNo", month);
                //cmd.Parameters.AddWithValue("@Frequency", Util.GetDateTime(scheduleDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TeamLeader", tlentry);
                cmd.Parameters.AddWithValue("@CrewMemberName", crewdata);
                cmd.Parameters.AddWithValue("@Param", "TransactionChecklist_Save");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        result = sdr["SaveFlag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return result;
        }
        #endregion

        #region ----- PM Report ----
        internal static DataTable getPMReportDetails(string machineid, string Year, string FromDate, string ToDate, out DataTable dt_HeaderData, out DataTable dt_LastChecked)
        {
            DataTable dt = new DataTable();
            dt_HeaderData = new DataTable();
            dt_LastChecked = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"S_GetPMTransactionReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                if (!string.IsNullOrEmpty(Year))
                    cmd.Parameters.AddWithValue("@YearNo", Year);
                if(!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd"));
                }
                cmd.Parameters.AddWithValue("@Param", "PMTransactionReport_View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    if (dt.Columns.IndexOf("SaveFlag") == -1)
                    {
                        dt_HeaderData.Load(sdr);
                        dt_LastChecked.Load(sdr);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }
        #endregion
    }
}