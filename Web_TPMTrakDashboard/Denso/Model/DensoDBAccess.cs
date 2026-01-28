using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Denso.Model
{
    public class DensoDBAccess
    {
        internal static string getShiftNameOfDate(DateTime date)
        {

            string shift = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[s_GetCurrentShiftTime]", conn);
                cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        shift = rdr["shiftname"].ToString();
                    }
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
            return shift;
        }
        internal static int sendScheduleDataToHMI(string iddList)
        {

            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"update ACE_SAP_ScheduleDetails set Select_Schedule_ToHMI=1,HMI_UpdatedTS=null where IDD in (" + iddList + ")", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                result = cmd.ExecuteNonQuery();
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
            return result;
        }

        #region -------- Daily Checklist Master ------------
        internal static List<ListItem> getDailyChecklistRevisionNumber(string machineID)
        {
            List<ListItem> list = new List<ListItem>();
            ListItem data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct RevID,RevNo from DailyInspectionCheckSheet_Master_Denso where MachineID=@MachineID order by RevID desc", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new ListItem();
                        data.Value = rdr["RevID"].ToString();
                        data.Text = rdr["RevNo"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static List<DailyChecklistMasterEntity> getDailyChecklistMasterEntity(string machineID, string revID)
        {
            List<DailyChecklistMasterEntity> list = new List<DailyChecklistMasterEntity>();
            DailyChecklistMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDailyInspectionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@RevID", revID);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new DailyChecklistMasterEntity();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.Category = rdr["Category"].ToString();
                        data.ChecklistID = rdr["InspectionItem"].ToString();
                        data.JudgementCriteria = rdr["JudgementCriteria"].ToString();
                        data.Method = rdr["Method"].ToString();
                        data.Cycle = rdr["Cycle"].ToString();
                        data.PersonInCharge = rdr["PersonInCharge"].ToString();
                        data.Frequency = rdr["Frequency"].ToString();
                        data.FormatNumber = rdr["FormatNumber"].ToString();
                        data.RevID = rdr["RevID"].ToString();
                        data.RevNo = rdr["RevNo"].ToString();
                        data.ChecklistDesc = rdr["InspectionItemDescription"].ToString();
                        data.SortOrder = rdr["SortOrder"].ToString();
                        data.ChecklistType = rdr["ChecklistType"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static string insertUpdateDailyChecklistMasterEntity(DailyChecklistMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDailyInspectionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Category", data.Category);
                cmd.Parameters.AddWithValue("@InspectionItem", data.ChecklistID);
                cmd.Parameters.AddWithValue("@JudgementCriteria", data.JudgementCriteria);
                cmd.Parameters.AddWithValue("@Method", data.Method);
                cmd.Parameters.AddWithValue("@Cycle", data.Cycle);
                cmd.Parameters.AddWithValue("@PersonInCharge", data.PersonInCharge);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@FormatNumber", data.FormatNumber);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.Parameters.AddWithValue("@RevDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@RevisedBy", data.RevisedBy);
                cmd.Parameters.AddWithValue("@InspectionItemDescription", data.ChecklistDesc);
                cmd.Parameters.AddWithValue("@ChecklistType", data.ChecklistType);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        internal static bool isDailyChecklistTransactionStarted(DailyChecklistMasterEntity data)
        {
            bool isTransExists = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from DailyInspectionCheckSheet_Transaction_Denso where MachineID=@MachineID and InspectionItem=@InspectionItem and RevID=@RevID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@InspectionItem", data.ChecklistID);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    isTransExists = true;
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
            return isTransExists;
        }
        internal static string deleteDailyChecklistMasterEntity(DailyChecklistMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDailyInspectionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Category", data.Category);
                cmd.Parameters.AddWithValue("@InspectionItem", data.ChecklistID);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = rdr["DeleteFlag"].ToString();
                    }
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
            return result;
        }
        internal static string copyDailyChecklistMasterDetails(string sourceMachine, string destMachine)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDailyInspectionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OldMachineID", sourceMachine);
                cmd.Parameters.AddWithValue("@MachineID", destMachine);
                cmd.Parameters.AddWithValue("@Param", "CopyFromOldToNewMachine");
                cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = "error";
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion

        #region --------- Daily Checkpoint Transaction ---------
        internal static DataTable getDailyCheckpointTransactionDetails(string machineID, string year, string month, string weekNo, string param)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDailyInspectionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);
                //cmd.Parameters.AddWithValue("@WeekNumber", weekNo);
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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
            return dt;
        }

        internal static string insertUpdateDailyChecklistTransactionEntity(DailyChecklistTransEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDailyInspectionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Category", data.Category);
                cmd.Parameters.AddWithValue("@InspectionItem", data.ChecklistID);
                cmd.Parameters.AddWithValue("@JudgementCriteria", data.JudgementCriteria);
                cmd.Parameters.AddWithValue("@Method", data.Method);
                cmd.Parameters.AddWithValue("@Cycle", data.Cycle);
                cmd.Parameters.AddWithValue("@PersonInCharge", data.PersonInCharge);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@FormatNumber", data.FormatNumber);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.Parameters.AddWithValue("@RevDate", data.RevDate);
                cmd.Parameters.AddWithValue("@RevisedBy", data.RevisedBy);
                cmd.Parameters.AddWithValue("@InspectionItemDescription", data.ChecklistDesc);
                cmd.Parameters.AddWithValue("@ChecklistType", data.ChecklistType);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@Year", data.Year);
                cmd.Parameters.AddWithValue("@Month", data.Month);
                cmd.Parameters.AddWithValue("@Date", data.Date);
                cmd.Parameters.AddWithValue("@Shift", data.Shift);
                cmd.Parameters.AddWithValue("@Value", data.ActualValue);
                cmd.Parameters.AddWithValue("@InspectedBy", data.InspectedBy);
                cmd.Parameters.AddWithValue("@InspectedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (data.FileInBase64 != "")
                {
                    cmd.Parameters.AddWithValue("@ImageName", data.FileName);
                    cmd.Parameters.AddWithValue("@ImageData", data.FileInByte);
                }
                cmd.Parameters.AddWithValue("@Param", "TransactionSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion

        #region ---- Static Accuracy MAster-----------
        internal static List<StaticAccuracyMasterEntity> getStaticAccuracyMasterDetails(string machineID)
        {
            List<StaticAccuracyMasterEntity> list = new List<StaticAccuracyMasterEntity>();
            StaticAccuracyMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetStaticAccuracySheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new StaticAccuracyMasterEntity();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.SortOrder = rdr["CheckpointID"].ToString();
                        data.Checkpoint = rdr["Checkpoint"].ToString();
                        data.CheckpointType = rdr["CheckpointType"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static string insertUpdateStaticAccuracyMasterEntity(StaticAccuracyMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetStaticAccuracySheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CheckpointID", data.SortOrder);
                cmd.Parameters.AddWithValue("@Checkpoint", data.Checkpoint);
                cmd.Parameters.AddWithValue("@CheckpointType", data.CheckpointType);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        internal static string deleteStaticAccuracyMasterEntity(StaticAccuracyMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetStaticAccuracySheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CheckpointID", data.SortOrder);
                cmd.Parameters.AddWithValue("@Checkpoint", data.Checkpoint);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = rdr["DeleteFlag"].ToString();
                    }
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
            return result;
        }
        #endregion

        #region ---- Static Accuracy transaction ------
        internal static DataTable getStaticAccuracyTransactionDetails(string machineID, string year, string month, string param)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetStaticAccuracySheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);
                //cmd.Parameters.AddWithValue("@WeekNumber", weekNo);
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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
            return dt;
        }
        internal static string insertUpdateStaticAccuracyTransactionDetails(StaticAccuracyTransEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetStaticAccuracySheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CheckpointID", data.SortOrder);
                cmd.Parameters.AddWithValue("@Checkpoint", data.Checkpoint);
                cmd.Parameters.AddWithValue("@CheckpointType", data.CheckpointType);
                cmd.Parameters.AddWithValue("@Year", data.Year);
                cmd.Parameters.AddWithValue("@Month", data.Month);
                cmd.Parameters.AddWithValue("@WeekNo", data.WeekNo);
                cmd.Parameters.AddWithValue("@Value", data.ActualValue);
                cmd.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (data.FileInBase64 != "")
                {
                    cmd.Parameters.AddWithValue("@ImageName", data.FileName);
                    cmd.Parameters.AddWithValue("@ImageData", data.FileInByte);
                }
                cmd.Parameters.AddWithValue("@Param", "TransactionSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion

        #region -------- PokayOke Checksheet Master ------------
        internal static List<ListItem> getPokayOkeChecksheetRevisionNumber(string machineID)
        {
            List<ListItem> list = new List<ListItem>();
            ListItem data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct RevID,RevNo from PokayokeFunctionCheckSheet_Master where MachineID=@MachineID order by RevID desc", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new ListItem();
                        data.Value = rdr["RevID"].ToString();
                        data.Text = rdr["RevNo"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static List<PokayOkeMasterEntity> getPokayOkeChecksheetMasterEntity(string machineID, string revID)
        {
            List<PokayOkeMasterEntity> list = new List<PokayOkeMasterEntity>();
            PokayOkeMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetPokayokeFunctionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@RevID", revID);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new PokayOkeMasterEntity();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.PokayOkeItem = rdr["Item"].ToString();
                        data.Function = rdr["Function"].ToString();
                        data.CheckMethod = rdr["CheckPoint"].ToString();
                        data.CheckInterval = rdr["CheckInterval"].ToString();
                        data.RevID = rdr["RevID"].ToString();
                        data.RevNo = rdr["RevNo"].ToString();
                        data.SortOrder = rdr["SortOrder"].ToString();
                        data.CheckPointType = rdr["ChecklistType"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static string insertUpdatePokayOkeChecksheetMasterEntity(PokayOkeMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetPokayokeFunctionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Item", data.PokayOkeItem);
                cmd.Parameters.AddWithValue("@Function", data.Function);
                cmd.Parameters.AddWithValue("@CheckPoint", data.CheckMethod);
                cmd.Parameters.AddWithValue("@CheckInterval", data.CheckInterval);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.Parameters.AddWithValue("@RevDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@RevisedBy", data.RevisedBy);
                cmd.Parameters.AddWithValue("@ChecklistType", data.CheckPointType);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        internal static string deletePokayOkeChecksheetMasterEntity(PokayOkeMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetPokayokeFunctionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Item", data.PokayOkeItem);
                cmd.Parameters.AddWithValue("@CheckPoint", data.CheckMethod);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = rdr["DeleteFlag"].ToString();
                    }
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
            return result;
        }
        internal static bool isPokayOkeChecksheetMasterEntity(PokayOkeMasterEntity data)
        {
            bool isTransStarted = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from PokayokeFunctionCheckSheet_Transaction where MachineID=@MachineID and Item=@Item and RevID=@RevID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Item", data.PokayOkeItem);
                cmd.Parameters.AddWithValue("@CheckPoint", data.CheckMethod);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    isTransStarted = true;
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
            return isTransStarted;
        }
        internal static string copyPokayOkeChecksheetMasterDetails(string sourceMachine, string destMachine)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetPokayokeFunctionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OldMachineID", sourceMachine);
                cmd.Parameters.AddWithValue("@MachineID", destMachine);
                cmd.Parameters.AddWithValue("@Param", "CopyFromOldToNewMachine");
                cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = "error";
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion

        #region ---- PokayOke  transaction ------
        internal static DataTable getPokayOkeTransactionDetails(string machineID, string year, string month, string param)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetPokayokeFunctionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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
            return dt;
        }
        internal static string insertUpdatePokayOkeTransactionDetails(PokeyOkeTransEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetPokayokeFunctionCheckSheet_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Item", data.PokayOkeItem);
                cmd.Parameters.AddWithValue("@Function", data.Function);
                cmd.Parameters.AddWithValue("@CheckPoint", data.CheckMethod);
                cmd.Parameters.AddWithValue("@CheckInterval", data.CheckInterval);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevNo);
                cmd.Parameters.AddWithValue("@RevDate", data.RevDate);
                cmd.Parameters.AddWithValue("@RevisedBy", data.RevisedBy);
                cmd.Parameters.AddWithValue("@Year", data.Year);
                cmd.Parameters.AddWithValue("@Month", data.Month);
                cmd.Parameters.AddWithValue("@WeekNo", data.WeekNo);
                cmd.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@ChecklistType", data.CheckpointType);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Value", data.ActualValue);
                if (data.FileInBase64 != "")
                {
                    cmd.Parameters.AddWithValue("@ImageName", data.FileName);
                    cmd.Parameters.AddWithValue("@ImageData", data.FileInByte);
                }
                cmd.Parameters.AddWithValue("@Param", "TransactionSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion

        #region ----- First Information Report FIR ----------

        internal static List<string> getDownCategoryData()
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from DownCategoryInformation", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr["DownCategory"].ToString());
                    }
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
        internal static List<string> getDownIDByCategory(string downCategory)
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct downid from downcodeinformation where Catagory in (" + downCategory + ")", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr["downid"].ToString());
                    }
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
        internal static List<FIRTransDataEntity> getFIRGridDetails(string machineID, string category, string downID, string fromDate, string toDate)
        {
            List<FIRTransDataEntity> list = new List<FIRTransDataEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFirstInformationReport_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@DownCategory", category);
                cmd.Parameters.AddWithValue("@DownID", downID);
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "FIRDetails_GridView");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FIRTransDataEntity data = new FIRTransDataEntity();
                        data.PlantID = rdr["PlantID"].ToString();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.DownCategory = rdr["Catagory"].ToString();
                        data.DownID = rdr["downid"].ToString();
                        data.StartTime = rdr["sttime"].ToString();
                        data.EndTime = rdr["ndtime"].ToString();
                        data.DownDesc = rdr["downdescription"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static DataTable getFIRTransactionDetails(string machineID, string downCategory, string downID, string startTime, string endTime, out List<FIRTransDataEntity> listActionTaken, out DataTable dt3)
        {
            DataTable dt = new DataTable();
            listActionTaken = new List<FIRTransDataEntity>();
            dt3 = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFirstInformationReport_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@DownCategory", downCategory);
                cmd.Parameters.AddWithValue("@DownID", downID);
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "FIRDetails_View");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                while (rdr.Read())
                {
                    FIRTransDataEntity data = new FIRTransDataEntity();
                    data.ActionTaken = rdr["ActionTaken"].ToString();
                    data.ActionTakenByWhom = rdr["ByWhom"].ToString();
                    data.ActionTakenResult = rdr["Result"].ToString();
                    listActionTaken.Add(data);
                }
                rdr.NextResult();
                dt3.Load(rdr);
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
            return dt;
        }
        internal static string insertUpdateFIRActionTakenDetails(FIRTransDataEntity data)
        {
            string result = "";
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFirstInformationReport_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", data.PlantID);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@DownCategory", data.DownCategory);
                cmd.Parameters.AddWithValue("@DownID", data.DownID);
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(data.StartTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(data.StartTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(data.EndTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ActionTaken", data.ActionTaken);
                cmd.Parameters.AddWithValue("@ByWhom", data.ActionTakenByWhom);
                cmd.Parameters.AddWithValue("@Result", data.ActionTakenResult);
                cmd.Parameters.AddWithValue("@Shift", data.Shift);
                cmd.Parameters.AddWithValue("@Param", "FIRActionTaken_Save");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        internal static string insertUpdateFIRTransDetails(FIRTransDataEntity data)
        {
            string result = "";
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFirstInformationReport_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", data.PlantID);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@DownCategory", data.DownCategory);
                cmd.Parameters.AddWithValue("@DownID", data.DownID);
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(data.StartTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(data.StartTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(data.EndTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", data.Shift);
                cmd.Parameters.AddWithValue("@RootCause", data.RootCause);
                cmd.Parameters.AddWithValue("@StockStatus", data.StockStatus);
                cmd.Parameters.AddWithValue("@StockImpact", data.StockImpact);
                cmd.Parameters.AddWithValue("@PresentStatus", data.PresentStatus);
                cmd.Parameters.AddWithValue("@Details", data.Details);
                cmd.Parameters.AddWithValue("@PartNo", data.PartNo);
                cmd.Parameters.AddWithValue("@PartName", data.PartName);
                cmd.Parameters.AddWithValue("@QtyHold", data.QtyHold);
                cmd.Parameters.AddWithValue("@NextActionDecided", data.NextActionDecided);
                cmd.Parameters.AddWithValue("@NextActionByWhom", data.NextActionDeciedByWhom);
                cmd.Parameters.AddWithValue("@NextActionResult", data.NextActionDeciedResult);
                cmd.Parameters.AddWithValue("@Param", "FIRDetails_Save");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        internal static string insertUpdateFIRAttendeesDetails(FIRTransDataEntity data, string employeeID, string department)
        {
            string result = "";
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFirstInformationReport_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", data.PlantID);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@DownCategory", data.DownCategory);
                cmd.Parameters.AddWithValue("@DownID", data.DownID);
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(data.StartTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(data.StartTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(data.EndTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", data.Shift);
                cmd.Parameters.AddWithValue("@Department", department);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@Param", "FIRAttendance_Save");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion

        #region ---- 5 S Checksheet MAster-----------
        internal static List<FiveSChecksheetMasterEntity> getFiveSChecksheetMasterDetails(string machineID)
        {
            List<FiveSChecksheetMasterEntity> list = new List<FiveSChecksheetMasterEntity>();
            FiveSChecksheetMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFiveSCheckSheet_Report_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new FiveSChecksheetMasterEntity();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.SortOrder = rdr["SortOrder"].ToString();
                        data.Shifts = rdr["SortOrder"].ToString();
                        data.Checkpoint = rdr["CheckItem"].ToString();
                        data.CheckpointType = rdr["ChecklistType"].ToString();
                        data.Shifts = rdr["Cycle"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static string insertUpdateFiveSChecksheetMasterEntity(FiveSChecksheetMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFiveSCheckSheet_Report_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@CheckItem", data.Checkpoint);
                cmd.Parameters.AddWithValue("@ChecklistType", data.CheckpointType);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Cycle", data.Shifts);
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        internal static string deleteFiveSChecksheetMasterEntity(FiveSChecksheetMasterEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFiveSCheckSheet_Report_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CheckItem", data.Checkpoint);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = rdr["DeleteFlag"].ToString();
                    }
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
            return result;
        }
        #endregion

        #region --------- Five S Checksheet Transaction ---------
        internal static DataTable getFiveSCheckpointTransactionDetails(string machineID, string year, string month, string param)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFiveSCheckSheet_Report_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);
                //cmd.Parameters.AddWithValue("@WeekNumber", weekNo);
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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
            return dt;
        }

        internal static string insertUpdateFiveSCheckpointTransactionEntity(FiveSChecksheetTransEntity data)
        {
            string result = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFiveSCheckSheet_Report_Denso]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CheckItem", data.ChecklistID);
                cmd.Parameters.AddWithValue("@ChecklistType", data.CheckpointType);
                cmd.Parameters.AddWithValue("@Cycle", data.Cycle);
                cmd.Parameters.AddWithValue("@Shift", data.Shift);
                cmd.Parameters.AddWithValue("@Date", data.Date);
                cmd.Parameters.AddWithValue("@Year", data.Year);
                cmd.Parameters.AddWithValue("@Month", data.Month);
                cmd.Parameters.AddWithValue("@Value", data.ActualValue);
                cmd.Parameters.AddWithValue("@UpdatedBy", data.InspectedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                if (data.FileInBase64 != "")
                {
                    cmd.Parameters.AddWithValue("@ImageName", data.FileName);
                    cmd.Parameters.AddWithValue("@ImageData", data.FileInByte);
                }
                cmd.Parameters.AddWithValue("@Param", "TransactionSave");
                cmd.CommandTimeout = 300;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return result;
        }
        #endregion

        #region ----- Modified Down Data -------
        internal static List<ModifiedDataEntity> GetAllParameterWithIntefaceAndCode(string StartTime, string EndTime, string plantid, string MacInt, string Comp, string Opn, string Opr, string downcode, string Datatype, string param, string downCategory)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ModifiedDataEntity> list = new List<ModifiedDataEntity>();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            ModifiedDataEntity data = null;
            try
            {
                if (string.Equals(plantid, "All", StringComparison.OrdinalIgnoreCase)) plantid = "";
                if (string.Equals(downCategory, "All", StringComparison.OrdinalIgnoreCase)) downCategory = "";
                cmd = new SqlCommand("select distinct interfaceid ,DownId, interfaceid +' <'+ downid +'>' as result from downcodeinformation where interfaceid is not null and   (Catagory = @Catagory or isnull(@Catagory, '') = '') order by interfaceid", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"StartTime", Util.GetDateTime(StartTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue(@"EndTime", Util.GetDateTime(EndTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@machineid", MacInt);
                cmd.Parameters.AddWithValue("@Component", Comp);
                cmd.Parameters.AddWithValue("@operation", Opn);
                cmd.Parameters.AddWithValue("@Operator", Opr);
                cmd.Parameters.AddWithValue("@DownCode", downcode);
                cmd.Parameters.AddWithValue("@Plantid", plantid);
                cmd.Parameters.AddWithValue(@"Datatype", Datatype);
                cmd.Parameters.AddWithValue(@"Catagory", downCategory);
                cmd.Parameters.AddWithValue("@param", param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    data = new ModifiedDataEntity();
                    data.InterfaceID = rdr["interfaceid"].ToString();
                    data.InterfaceIWithID = rdr["result"].ToString();
                    list.Add(data);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        #endregion
    }
}