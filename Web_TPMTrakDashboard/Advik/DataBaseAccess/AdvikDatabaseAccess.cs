using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.Andon_Advik.Model;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.WebAndon;

namespace Web_TPMTrakDashboard.Advik.DataBaseAccess
{
    public class AdvikDatabaseAccess
    {
        internal static List<string> GetMachines(string selectedPlant)
        {
            List<string> machines = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            string query = @"[s_GetLookups]";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Machine");
                cmd.Parameters.AddWithValue("@filter", selectedPlant.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : selectedPlant);
                cmd.CommandTimeout = 120;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        machines.Add(reader["Machineid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return machines;
        }

        public static List<string> GetPlantIDs()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from PlantInformation", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["PlantID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        internal static List<PMChecklistMasterEntity> GetChecklistMasterDatas(string selectedMachine, string jhType)
        {
            List<PMChecklistMasterEntity> masterDatas = new List<PMChecklistMasterEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            // string query = @"select * from JHCheckListMaster_Advik where MachineID=@machineId and JHType=@jhType order by SortOrder";
            string query = @"select * from JHCheckListMaster_Advik where MachineID=@machineId and (JHType=@jhType or isnull(@jhType,'')='') order by SortOrder";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@machineId", selectedMachine);
                cmd.Parameters.AddWithValue("@jhType", jhType);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PMChecklistMasterEntity masterData = new PMChecklistMasterEntity();
                        masterData.MachineID = reader["MachineID"].ToString();
                        masterData.ChecklistID = reader["ChecklistID"].ToString();
                        // masterData.ChecklistItem = reader["ChecklistItem"].ToString();
                        //masterData.NoOfCycles = Convert.ToInt32(reader["NoOfCycles"].ToString());
                        masterData.SortOrder = reader["SortOrder"].ToString();
                        masterData.JHType = reader["JHType"].ToString();
                        masterData.IsEnabled = Convert.ToBoolean(reader["IsEnabled"].ToString());
                        masterData.McArea = reader["McArea"].ToString();
                        masterData.Location = reader["Location"].ToString();
                        masterData.StdCondition = reader["StdCondition"].ToString();
                        masterData.CheckingMethod = reader["CheckingMethod"].ToString();
                        masterDatas.Add(masterData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return masterDatas;
        }


        internal static void SaveChecklistMasterData(string machineId, PMChecklistMasterEntity data, int sortOrder)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"If not exists(select * from JHCheckListMaster_Advik where MachineID=@machineId and ChecklistID=@checklistId and JHType=@jhType)
     begin
          insert into JHCheckListMaster_Advik(MachineID,ChecklistID,IsEnabled,SortOrder,JHType,McArea,Location,StdCondition,CheckingMethod) 
		  values (@machineId,@checklistId,@isEnabled,@sortOrder,@jhType,@mcarea,@location,@stdcondition,@checkingmethod)
     end
else
    begin
           update JHCheckListMaster_Advik set  McArea=@mcarea,Location=@location,StdCondition=@stdcondition,CheckingMethod=@checkingmethod, IsEnabled=@isEnabled, SortOrder=@sortOrder where MachineID=@machineId and ChecklistID=@checklistId and JHType=@jhType
     end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@machineId", machineId);
                cmd.Parameters.AddWithValue("@checklistId", data.ChecklistID);
                //cmd.Parameters.AddWithValue("@checklistItem", data.);
                //cmd.Parameters.AddWithValue("@noOfCycle", noOfCycles);
                cmd.Parameters.AddWithValue("@mcarea", data.McArea);
                cmd.Parameters.AddWithValue("@location", data.Location);
                cmd.Parameters.AddWithValue("@stdcondition", data.StdCondition);
                cmd.Parameters.AddWithValue("@checkingmethod", data.CheckingMethod);
                cmd.Parameters.AddWithValue("@isEnabled", data.IsEnabled);
                cmd.Parameters.AddWithValue("@sortOrder", sortOrder);
                cmd.Parameters.AddWithValue("@jhType", data.JHType);
                cmd.ExecuteNonQuery();
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

        internal static void LoadChecklistMasterDataToDB(DataTable dtChecklistMaster)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlBulkCopy sqlBulkCopy = null;
            try
            {
                sqlBulkCopy = new SqlBulkCopy(conn);
                sqlBulkCopy.DestinationTableName = "JHCheckListMaster_Advik";
                sqlBulkCopy.ColumnMappings.Add("MachineID", "MachineID");
                sqlBulkCopy.ColumnMappings.Add("ChecklistID", "ChecklistID");
                //sqlBulkCopy.ColumnMappings.Add("ChecklistItem", "ChecklistItem");
                sqlBulkCopy.ColumnMappings.Add("McArea", "McArea");
                sqlBulkCopy.ColumnMappings.Add("Location", "Location");
                sqlBulkCopy.ColumnMappings.Add("StdCondition", "StdCondition");
                sqlBulkCopy.ColumnMappings.Add("CheckingMethod", "CheckingMethod");
                //sqlBulkCopy.ColumnMappings.Add("NoOfCycles", "NoOfCycles");
                sqlBulkCopy.ColumnMappings.Add("IsEnabled", "IsEnabled");
                sqlBulkCopy.ColumnMappings.Add("SortOrder", "SortOrder");
                sqlBulkCopy.ColumnMappings.Add("JHType", "JHType");

                sqlBulkCopy.WriteToServer(dtChecklistMaster);
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
        internal static bool CheckForRowPresence(string machineID, string checklistID, string jhType)
        {
            bool isNotPresent = true;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            string query = @"select * from JHCheckListMaster_Advik where MachineID=@machineId and ChecklistID=@checklistId and JHType=@jhType";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@machineId", machineID);
                cmd.Parameters.AddWithValue("@checklistId", checklistID);
                cmd.Parameters.AddWithValue("@jhType", jhType);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    isNotPresent = false;
            }
            catch (Exception ex)
            {
                isNotPresent = true;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return isNotPresent;
        }



        #region -----JH Dashboard------

        internal static List<JHDashboardDetails> getJHDashboarddetails(string plant, string machineid, string shift, string jhtype, DateTime fromdate, DateTime todate, string GroupID)
        {
            List<JHDashboardDetails> listJHDetails = new List<JHDashboardDetails>();
            JHDashboardDetails jhdetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_View_JHChecklistDashboard_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machine", machineid);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@JHType", jhtype);
                // cmd.Parameters.AddWithValue("@machineId", plant);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@Enddate", todate);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        jhdetails = new JHDashboardDetails();
                        //  jhdetails.SlNo = reader["MachineID"].ToString();
                        jhdetails.Date = Convert.ToDateTime(reader["Sdate"].ToString()).ToString("dd-MMM-yyyy");
                        jhdetails.Shift = reader["ShiftName"].ToString();
                        jhdetails.Machine = reader["Machineid"].ToString();
                        //jhdetails.JHActivity = reader["JHChecklistName"].ToString();
                        jhdetails.JHActivityID = reader["JHChecklistID"].ToString();
                        jhdetails.McArea = reader["McArea"].ToString();
                        jhdetails.Location = reader["Location"].ToString();
                        jhdetails.StdCondition = reader["StdCondition"].ToString();
                        jhdetails.CheckingMethod = reader["CheckingMethod"].ToString();
                        jhdetails.JHType = reader["JHChecklistType"].ToString();
                        jhdetails.Status = reader["ChecklistStatus"].ToString();
                        //jhdetails.Remarks = reader["Remarks"].ToString();
                        listJHDetails.Add(jhdetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return listJHDetails;
        }


        internal static int SaveRemarks(string machineid, string jhactivity, string jhtype, string remarks, string empid, string chkValue, string param)
        {
            int success = 0;
            string query = "";

            if (param == "UpdateRemarks")
            {
                query = @"if exists(select * from JHChecklistTransaction_Advik where MachineID=@machineid and ChecklistID=@jhactivity and ChecklistType=@chkListType) 
begin
update JHChecklistTransaction_Advik set Remarks = @remarks,UpdatedTS=@remarksTS
where MachineID=@machineid and ChecklistID=@jhactivity  and ChecklistType=@chkListType
end";
            }
            else if (param == "UpdateSupervisorDetails")
            {
                query = @"if exists(select * from JHChecklistTransaction_Advik where MachineID=@machineid and ChecklistID=@jhactivity and ChecklistType=@chkListType) 
begin
update JHChecklistTransaction_Advik set SupervisorName=@empid,SupervisorTS=@chkTS,SupervisorValue=@chkValue
where MachineID=@machineid and ChecklistID=@jhactivity  and ChecklistType=@chkListType
end ";
            }

            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                if (query != "")
                {
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    cmd.Parameters.AddWithValue("@machineid", machineid);
                    cmd.Parameters.AddWithValue("@jhactivity", jhactivity);
                    cmd.Parameters.AddWithValue("@remarks", remarks);
                    cmd.Parameters.AddWithValue("@chkListType", jhtype);
                    cmd.Parameters.AddWithValue("@remarksTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@chkTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@chkValue", chkValue);
                    cmd.Parameters.AddWithValue("@empid", empid);
                    success = cmd.ExecuteNonQuery();
                }
                else
                {
                    success = 1;
                }

            }
            catch (Exception ex)
            {
                success = 0;
                Logger.WriteErrorLog("While save remarks in JH Dashboard" + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }

       
        internal static string getEmployeeRole(string username)
        {
            string role = "";
            string query = @"select employeerole from employeeinformation where employeeid=@empid";
            SqlDataReader sdr = null;
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@empid", username);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        role = sdr["employeerole"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                role = "";
                Logger.WriteErrorLog("While getting Employee Role " + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
                if (sdr != null) sdr.Close();
            }
            return role;
        }
        #endregion

        #region -----PM Master----
        internal static List<PMMasterDetails> getPMMasterDetails(string machineID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PMMasterDetails> listParameter = new List<PMMasterDetails>();
            PMMasterDetails parameter = null;
            try
            {
                cmd = new SqlCommand("select * from PM_Master_Advik where Machineid=@machineid", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineid", machineID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            parameter = new PMMasterDetails();
                            parameter.PMID = sdr["PMId"].ToString();
                            parameter.PMActivity = sdr["PMActivity"].ToString();
                            parameter.NoOfCycle = sdr["NoOfCycle"].ToString();
                            listParameter.Add(parameter);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in Preventive Master Details - " + ex.Message);
                        }
                    }
                }
                else
                {
                    parameter = new PMMasterDetails();
                    listParameter.Add(parameter);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getting OPerator details - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listParameter;
        }

        internal static int InsertUpdatePMMasterDetails(PMMasterDetails data, string param)
        {
            int success = 0;
            string query = "";
            if (param == "Insert")
            {
                query = @"if not exists(select * from PM_Master_Advik where Machineid=@machineid and PMActivity=@pmactivity) 
begin
 insert into PM_Master_Advik(Machineid, PMActivity, NoOfCycle) 
 values(@machineid, @pmactivity, @noofcycle) end";
            }
            if (param == "Update")
            {
                query = @"  if exists(select * from PM_Master_Advik where Machineid=@machineid and PMId=@pmid) 
begin
update PM_Master_Advik set PMActivity=@pmactivity, NoOfCycle = @noofcycle
where  Machineid=@machineid and PMId=@pmid
end";
            }

            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@machineid", data.MachineID);
                cmd.Parameters.AddWithValue("@pmactivity", data.PMActivity);
                cmd.Parameters.AddWithValue("@pmid", data.PMID);
                cmd.Parameters.AddWithValue("@noofcycle", data.NoOfCycle);
                success = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PRIMARY KEY"))
                {
                    success = -2;
                }
                else
                {
                    success = 0;
                    Logger.WriteErrorLog("While inserting Preventive Maintenance details" + ex.ToString());
                }

            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }
        internal static int deletePMActivityDetails(string machineid, string pmid)
        {
            int success = 0;
            string query = @" if not exists(select * from PM_Transaction_ADVIK where MachineID=@machineid and PM_ID=@pmid) begin 
 delete from PM_Master_Advik where MachineId = @machineid and PMId = @pmid end";

            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@pmid", pmid);
                success = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                success = 0;
                Logger.WriteErrorLog("While deleting PM Activity" + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }

        internal static int storeImportPMDetailsInfoExcelToDB(DataTable dtPMDetails, string machineid)
        {
            int success = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                int cnt = 0;
                foreach (DataRow row in dtPMDetails.Rows)
                {
                    SqlCommand sqlCommand = new SqlCommand(@"if not exists(select * from PM_Master_Advik where Machineid=@machineid and PMActivity=@pmactivity) 
begin
 insert into PM_Master_Advik(Machineid, PMActivity, NoOfCycle) 
 values(@machineid, @pmactivity, @noofcycle) end else begin update PM_Master_Advik set NoOfCycle=@noofcycle where  Machineid=@machineid and PMActivity=@pmactivity end ", con);
                    if (row["PMActivity"].ToString() == "")
                    {
                        continue;
                    }
                    if (row["NoOfCyle"].ToString() != "")
                    {
                        Int64 noofcycle = Convert.ToInt64(row["NoOfCyle"].ToString());
                        if (noofcycle <= 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    sqlCommand.Parameters.AddWithValue("@machineid", machineid);
                    sqlCommand.Parameters.AddWithValue("@pmactivity", row["PMActivity"].ToString());
                    sqlCommand.Parameters.AddWithValue("@noofcycle", row["NoOfCyle"].ToString());
                    cnt = sqlCommand.ExecuteNonQuery();
                    if (cnt > 0)
                    {
                        success += cnt;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
            }
            return success;
        }
        #endregion

        #region -----PM Dashboard---
        internal static List<PMMasterDetails> getPMDashboarddetails(string machineid, DateTime fromdate, DateTime todate, string PlantID, string GroupID)
        {
            List<PMMasterDetails> listPMDetails = new List<PMMasterDetails>();
            PMMasterDetails pmdetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_View_PMDashboard_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machine", machineid);
                cmd.Parameters.AddWithValue("@GroupID", GroupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : GroupID);
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@Enddate", todate);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pmdetails = new PMMasterDetails();
                        pmdetails.MachineID = reader["Machineid"].ToString();
                        pmdetails.PMID = reader["PMActivityID"].ToString();
                        pmdetails.PMActivity = reader["PMActivity"].ToString();
                        pmdetails.NoOfCycle = reader["TargetCycles"].ToString();
                        pmdetails.Status = reader["PMStatus"].ToString().Equals("1") ? "OK" : "POSTPONDED";
                        // pmdetails.Updatedby = reader["UpdatedBy"].ToString();
                        pmdetails.Updatedts = reader["UpdatedTS"].ToString();
                        listPMDetails.Add(pmdetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return listPMDetails;
        }

        internal static int SavePHTransactionDetails(string machineid, string pmid, string status, string updatedby)
        {
            int success = 0;
            string query = @"  if exists(select * from PM_Transaction_ADVIK where MachineID=@machineid and PM_ID=@pmid) 
begin
update PM_Transaction_ADVIK set Status = @status,UpdatedTS=@updatedts
where MachineID=@machineid and PM_ID=@pmid
end
else
begin
insert into PM_Transaction_ADVIK(PM_ID, MachineID, Status,UpdatedTS)
values(@pmid,@machineid,@status,@updatedts)
end ";

            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@pmid", pmid);
                cmd.Parameters.AddWithValue("@updatedby", updatedby);
                cmd.Parameters.AddWithValue("@updatedts", DateTime.Now);
                success = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                success = 0;
                Logger.WriteErrorLog("While PM Dashboard details" + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }
        #endregion

        #region ----Help Request Details---
        internal static List<HelpRequestReportDetails> getHelpRequestDetails(string plantid, string machineid, string shift, string helprequest, DateTime fromdate, DateTime todate, string GroupID, out List<string> plantIDList)
        {
            List<HelpRequestReportDetails> listHRDetails = new List<HelpRequestReportDetails>();
            HelpRequestReportDetails hrdetails = null;
            plantIDList = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_readRequest_Ack_ResetDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plantid == "All" ? "" : plantid);
                //cmd.Parameters.AddWithValue("@GroupID", GroupID == "All" ? "" : GroupID);
                cmd.Parameters.AddWithValue("@Machineid", machineid);
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@calltype", helprequest);
                cmd.Parameters.AddWithValue("@starttime", fromdate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@endtime", todate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@param", "");
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        hrdetails = new HelpRequestReportDetails();
                        hrdetails.PlantID = reader["Plantid"].ToString();
                        plantIDList.Add(reader["Plantid"].ToString());
                        string shiftdate = "", requestedtime = "", resettime = "", completetime = "", acktime = "";
                        if (!reader["ShiftDate"].Equals(DBNull.Value))
                        {
                            shiftdate = Convert.ToDateTime(reader["ShiftDate"].ToString()).ToString("dd-MM-yyyy");
                        }
                        hrdetails.ShiftDate = shiftdate;
                        hrdetails.MachineID = reader["Machineid"].ToString();
                        hrdetails.ShiftName = reader["ShiftName"].ToString();
                        hrdetails.RequestType = reader["Eventid"].ToString();
                        if (!reader["RequestedTime"].Equals(DBNull.Value))
                        {
                            requestedtime = Convert.ToDateTime(reader["RequestedTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        hrdetails.RequestedTime = requestedtime;
                        if (!reader["AcknowledgeTime"].Equals(DBNull.Value))
                        {
                            acktime = Convert.ToDateTime(reader["AcknowledgeTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        hrdetails.AckTime = acktime;
                        if (!reader["ResetTime"].Equals(DBNull.Value))
                        {
                            resettime = Convert.ToDateTime(reader["ResetTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        hrdetails.ResetTime = resettime;
                        if (!reader["Completedtime"].Equals(DBNull.Value))
                        {
                            completetime = Convert.ToDateTime(reader["Completedtime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        hrdetails.AckOperatorTime = completetime;
                        hrdetails.AckTimeFromTrigger = reader["ACKTime"].ToString();
                        hrdetails.ResetTimeFRomTrigger = reader["FIXTime"].ToString();
                        hrdetails.AckOperatorTimeFromTrigger = reader["CMPTime"].ToString();
                        listHRDetails.Add(hrdetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return listHRDetails;
        }

        internal static List<HelpRequestReportDetails> getCallTypeAvgDetails(string plantid, string machineid, string shift, string helprequest, DateTime fromdate, DateTime todate)
        {
            List<HelpRequestReportDetails> listHRDetails = new List<HelpRequestReportDetails>();
            HelpRequestReportDetails hrdetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_readRequest_Ack_ResetDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plantid == "All" ? "" : plantid);
                cmd.Parameters.AddWithValue("@Machineid", machineid);
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@calltype", helprequest);
                cmd.Parameters.AddWithValue("@starttime", fromdate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@endtime", todate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@param", "AvgCalltype");
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        hrdetails = new HelpRequestReportDetails();
                        hrdetails.RequestType = reader["Help_Description"].ToString();
                        hrdetails.AvgAckTimeFromTrigger = reader["AvgAcktime"].ToString();
                        hrdetails.AvgResetTimeFRomTrigger = reader["AvgFixTime"].ToString();
                        hrdetails.AvgAckOperatorTimeFromTrigger = reader["AvgCMPTime"].ToString();
                        hrdetails.MTBFValue = reader["MTBF"].ToString();
                        listHRDetails.Add(hrdetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return listHRDetails;
        }

        internal static List<string> getAllCallType()
        {
            List<string> callTypeList = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                cmd = new SqlCommand("select Help_Description from HelpCodeMaster", conn);
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        callTypeList.Add(reader["Help_Description"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return callTypeList;
        }
        #endregion

        #region"-----Andon Page-----"
        internal static List<AssemblyLineDataEntity> GetAdvikAssemblyLineAndonData(string plantID)
        {
            List<AssemblyLineDataEntity> assemblyLineDataList = new List<AssemblyLineDataEntity>();
            DataTable dtAndonData = new DataTable();
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                string shiftStart = Web_TPMTrakDashboard.Models.DataBaseAccess.GetCurrentShiftStart(out string endTime);
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"s_view_AssemblyLineParamAndon_Advik", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", shiftStart);
                cmd.Parameters.AddWithValue("@EndDate", endTime);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                cmd.Parameters.AddWithValue("@SortOrder", "");
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtAndonData.Load(reader);
                    if (dtAndonData != null && dtAndonData.Rows.Count > 0)
                    {
                        List<string> machineIDs = dtAndonData.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct().ToList();
                        foreach (string machineID in machineIDs)
                        {
                            List<MachineParameterData> machineParameterDataList = new List<MachineParameterData>();
                            DataTable parameterData = dtAndonData.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(machineID)).CopyToDataTable();
                            foreach (DataRow dataRow in parameterData.Rows)
                            {
                                machineParameterDataList.Add(new MachineParameterData()
                                {
                                    Parameter = dataRow["ParameterName"].ToString(),
                                    SuccessValue = Convert.ToInt32(dataRow["okCount"].ToString()),
                                    FailValue = Convert.ToInt32(dataRow["notokCount"].ToString())
                                });
                            }
                            assemblyLineDataList.Add(new AssemblyLineDataEntity()
                            {
                                MachineID = machineID,
                                ParameterDataList = machineParameterDataList
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return assemblyLineDataList;
        }

        internal static AndonPartsCountDataEntity GetAndonPartsCountData(string plantID)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            AndonPartsCountDataEntity andonPartsCountData = new AndonPartsCountDataEntity();
            List<AndonPartsCountData> andonPartsCounts = new List<AndonPartsCountData>();
            try
            {
                string shiftStart = Web_TPMTrakDashboard.Models.DataBaseAccess.GetCurrentShiftStart(out string endTime);
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"s_GetANDONDataDetails_ADVIK", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", shiftStart);
                cmd.Parameters.AddWithValue("@EndDate", endTime);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                cmd.Parameters.AddWithValue("@SortOrder", "");
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        andonPartsCounts.Add(new AndonPartsCountData()
                        {
                            PlantID = reader["PlantID"].ToString(),
                            MachineID = reader["MachineID"].ToString(),
                            TotalPartsCount = Convert.ToInt32(reader["Components"]),
                            OkPartsCount = Convert.ToInt32(reader["OkComponents"]),
                            NotOkPartsCount = Convert.ToInt32(reader["NotOkComponents"])
                        });
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        andonPartsCountData.DateShift = $"{reader["Date"]}, SHIFT - {reader["ShiftName"]}";
                        andonPartsCountData.TGT = Convert.ToInt32(reader["Target"]);
                        andonPartsCountData.ACT = Convert.ToInt32(reader["Components"]);
                        andonPartsCountData.Throughput = $"{Convert.ToInt32(reader["Throughput"])}%";
                    }
                }
                andonPartsCountData.AndonPartsCountDataList = andonPartsCounts;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return andonPartsCountData;
        }

        internal static DowntimeAndEfficiencyEntity GetAllDowntimeAndEfficiencyDetails(string plantID, string GroupID)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            DowntimeAndEfficiencyEntity AllDowntimeAndEfficiencyData = new DowntimeAndEfficiencyEntity();
            List<EfficiencyEntity> efficiencyDetails = new List<EfficiencyEntity>();
            List<DownTimeEntity> downTimeDetails = new List<DownTimeEntity>();
            MonthwiseEfficiencyEntity monthWiseAvgEfficiency = new MonthwiseEfficiencyEntity();
            try
            {
                string shiftStart = Web_TPMTrakDashboard.Models.DataBaseAccess.GetCurrentShiftStart(out string endTime);
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"s_GetANDONDataDetails_ADVIK", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", shiftStart);
                cmd.Parameters.AddWithValue("@EndDate", endTime);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.NextResult();
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["AE"] != null)
                            {
                                efficiencyDetails.Add(new EfficiencyEntity()
                                {
                                    PlantID = reader["PlantID"].ToString(),
                                    EfficiencyID = "AE",
                                    Efficiency = Math.Round(Convert.ToDouble(reader["AE"]), 2)
                                });
                            }
                            if (reader["OEE"] != null)
                            {
                                efficiencyDetails.Add(new EfficiencyEntity()
                                {
                                    PlantID = reader["PlantID"].ToString(),
                                    EfficiencyID = "OEE",
                                    Efficiency = Math.Round(Convert.ToDouble(reader["OEE"]), 2)
                                });
                            }
                            if (reader["PE"] != null)
                            {
                                efficiencyDetails.Add(new EfficiencyEntity()
                                {
                                    PlantID = reader["PlantID"].ToString(),
                                    EfficiencyID = "PE",
                                    Efficiency = Math.Round(Convert.ToDouble(reader["PE"]), 2)
                                });
                            }
                            if (reader["QE"] != null)
                            {
                                efficiencyDetails.Add(new EfficiencyEntity()
                                {
                                    PlantID = reader["PlantID"].ToString(),
                                    EfficiencyID = "QE",
                                    Efficiency = Math.Round(Convert.ToDouble(reader["QE"]), 2)
                                });
                            }
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            downTimeDetails.Add(new DownTimeEntity()
                            {
                                DownID = reader["DownID"].ToString(),
                                Downtime = Math.Round(Convert.ToDouble(reader["Downtime"]), 2)
                            });
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            monthWiseAvgEfficiency.Target = Convert.ToInt32(reader["Target"]);
                            monthWiseAvgEfficiency.Actual = Convert.ToInt32(reader["Components"]);
                            monthWiseAvgEfficiency.Throughput = Convert.ToInt32(reader["Throughput"]);
                        }
                    }
                }
                AllDowntimeAndEfficiencyData.EfficiencyDetails = efficiencyDetails;
                AllDowntimeAndEfficiencyData.DownTimeDetails = downTimeDetails;
                AllDowntimeAndEfficiencyData.monthwiseAvgEfficiency = monthWiseAvgEfficiency;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return AllDowntimeAndEfficiencyData;
        }
        #endregion

        #region ------------------- Andon --------------------
        public static List<string> getPlantID()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from PlantInformation", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["PlantID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        public static DataTable getSettingDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from CockpitDefaults where Parameter='AdvikWebandon'", con);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        public static List<PODetails> gridMachineStatusLoad(string MachineID, string PlantID, string Param)
        {
            List<PODetails> list = new List<PODetails>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetANDONHelpCodeDetails_ADVIK]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                //cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@Param", "");
                cmd.CommandTimeout = 40000;
                SqlDataReader rdr = cmd.ExecuteReader();
                try
                {
                    while (rdr.Read())
                    {
                        PODetails dto = new PODetails();
                        dto.Machine = rdr["MachineID"].ToString();

                        //if (!string.IsNullOrEmpty(rdr["UtilisedTime"].ToString()) && Convert.ToInt32(rdr["UtilisedTime"]) > 0)
                        //{
                        //    dto.Plan = rdr["Planned"].ToString();
                        //    dto.Act = rdr["Components"].ToString();
                        //    if (!string.IsNullOrEmpty(rdr["OverallEfficiency"].ToString()))
                        //    {
                        //        dto.OEE = Convert.ToDouble(rdr["OverallEfficiency"].ToString()) > 100 ? 100.ToString() : rdr["OverallEfficiency"].ToString();
                        //    }
                        //    if (Convert.ToDouble(dto.OEE) > Convert.ToInt32(rdr["OEGreen"].ToString()))
                        //    {
                        //        dto.OEEBackColor = "poGreenBg";
                        //        dto.Emoji = "Images/Smiley.png";
                        //    }
                        //    else if (Convert.ToDouble(dto.OEE) < Convert.ToInt32(rdr["OERed"].ToString()))
                        //    {
                        //        dto.OEEBackColor = "poPinkBg";
                        //        dto.Emoji = "Images/Sad.png";
                        //    }
                        //    else
                        //    {
                        //        dto.OEEBackColor = "poYellowBg";
                        //        dto.Emoji = string.Empty;
                        //    }

                        //    if (!string.IsNullOrEmpty(rdr["ProductionEfficiency"].ToString()))
                        //    {
                        //        dto.PE = Convert.ToDouble(rdr["ProductionEfficiency"].ToString()) > 100 ? 100.ToString() : rdr["ProductionEfficiency"].ToString();
                        //    }
                        //    if (Convert.ToDouble(dto.PE) > Convert.ToInt32(rdr["PEGreen"].ToString()))
                        //    {
                        //        dto.PEBackColor = "poGreenBg";
                        //    }
                        //    else if (Convert.ToDouble(dto.PE) < Convert.ToInt32(rdr["PERed"].ToString()))
                        //    {
                        //        dto.PEBackColor = "poPinkBg";
                        //    }
                        //    else
                        //    {
                        //        dto.PEBackColor = "poYellowBg";
                        //    }

                        //    if (!string.IsNullOrEmpty(rdr["AvailabilityEfficiency"].ToString()))
                        //    {
                        //        dto.AE = Convert.ToDouble(rdr["AvailabilityEfficiency"].ToString()) > 100 ? 100.ToString() : rdr["AvailabilityEfficiency"].ToString();
                        //    }
                        //    if (Convert.ToDouble(dto.AE) > Convert.ToInt32(rdr["AEGreen"].ToString()))
                        //    {
                        //        dto.AEBackColor = "poGreenBg";
                        //    }
                        //    else if (Convert.ToDouble(dto.AE) < Convert.ToInt32(rdr["AERed"].ToString()))
                        //    {
                        //        dto.AEBackColor = "poPinkBg";
                        //    }
                        //    else
                        //    {
                        //        dto.AEBackColor = "poYellowBg";
                        //    }
                        //}
                        //else
                        //{
                        //    dto.Plan = string.Empty;
                        //    dto.Act = string.Empty;
                        //    dto.OEE = string.Empty;
                        //    dto.AE = string.Empty;
                        //    dto.PE = string.Empty;
                        //    dto.Emoji = string.Empty;
                        //}

                        if (rdr["ColorCode"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Status = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["ColorCode"].ToString().Equals("blue", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Status = "Images/Status/Blue_button.jpg";
                        }
                        else if (rdr["ColorCode"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Status = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.Status = string.Empty;
                        }
                        dto.Component = rdr["HelpCode1"].ToString();
                        if (rdr["HelpCode1"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Component = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode1"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Component = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode1"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Component = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.Component = string.Empty;
                        }
                        dto.Setting = rdr["HelpCode2"].ToString();
                        if (rdr["HelpCode2"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Setting = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode2"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Setting = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode2"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Setting = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.Setting = string.Empty;
                        }
                        dto.Alaram = rdr["HelpCode3"].ToString();
                        if (rdr["HelpCode3"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Alaram = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode3"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Alaram = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode3"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Alaram = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.Alaram = string.Empty;
                        }
                        dto.User = rdr["HelpCode4"].ToString();
                        if (rdr["HelpCode4"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.User = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode4"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.User = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode4"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.User = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.User = string.Empty;
                        }

                        list.Add(dto);
                    }
                    if (rdr != null) rdr.Close();
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.ToString());
                    throw;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                //if (rdr != null) rdr.Close();
            }
            return list;
        }
        public static List<AndonSettingData> getColumnData()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<AndonSettingData> list = new List<AndonSettingData>();
            AndonSettingData data = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from AndonDefaults where Parameter='AdvikWebandon'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AndonSettingData();
                        data.Column = sdr["ValueInText"].ToString();
                        data.CustomColumn = sdr["ValueInText2"].ToString();
                        if (sdr["ValueInBool"].ToString().Trim() == "0")
                        {
                            data.Visibility = false;
                        }
                        else
                        {
                            data.Visibility = true;
                        }
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
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        public static ObservableCollection<DownTimeParetoDATAModel> GetChartsData(string machine, string runningPlantName, string param, out ObservableCollection<OEEData> DownPlantOEE)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string query = "";
            ObservableCollection<DownTimeParetoDATAModel> downTimeCharts = new ObservableCollection<DownTimeParetoDATAModel>();
            DownPlantOEE = new ObservableCollection<OEEData>();
            try
            {
                query = "[dbo].[s_GetANDONHelpCodeDetails_ADVIK]";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", runningPlantName);
                cmd.Parameters.AddWithValue("@Param", "");
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.NextResult();
                    while (sdr.Read())
                    {
                        DownPlantOEE.Add(new OEEData(sdr["PlantID"].ToString(), Convert.ToDouble(sdr["OEE"].ToString())));
                    }
                    sdr.NextResult();
                    while (sdr.Read())
                    {

                        DownTimeParetoDATAModel dtCharts = new DownTimeParetoDATAModel();
                        dtCharts.XValue = sdr["DownID"].ToString();
                        dtCharts.YValue = Math.Round(Convert.ToDouble(sdr["DownTime"].ToString()), 0);
                        downTimeCharts.Add(dtCharts);
                    }

                    if (downTimeCharts.Count > 10)
                    {
                        Double totalOthersDownTime = 0;
                        for (int i = 10; i < downTimeCharts.Count; i++)
                        {
                            totalOthersDownTime = totalOthersDownTime + downTimeCharts[i].YValue;
                        }
                        downTimeCharts = new ObservableCollection<DownTimeParetoDATAModel>(downTimeCharts.Take(10));
                        DownTimeParetoDATAModel dtCharts = new DownTimeParetoDATAModel();
                        dtCharts.XValue = "Others";
                        dtCharts.YValue = totalOthersDownTime;
                        downTimeCharts.Add(dtCharts);
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return downTimeCharts;
        }
        public static void updateSettingData(int header, int content, int noofRows, string imagePath, string videoPath, int showImage, int showVideo, string scrollingText, string flipInterval, string refreshInterval, string datarefreshInterval, out bool result1)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            result1 = true;
            try
            {

                SqlCommand cmd = new SqlCommand("update CockpitDefaults set ValueInInt=@header where Parameter='AdvikWebandon' and ValueInText='FontSize' and ValueInText2='Header' update CockpitDefaults set ValueInInt = @content where Parameter = 'AdvikWebandon' and ValueInText = 'FontSize' and ValueInText2 = 'Content' update CockpitDefaults set ValueInInt=@noofRows where Parameter='AdvikWebandon' and ValueInText='NoOfRows' update CockpitDefaults set ValueInText2=@imagePath where Parameter='AdvikWebandon' and ValueInText='ImagePath' update CockpitDefaults set ValueInText2=@videoPath where Parameter='AdvikWebandon' and ValueInText='VideoPath' update CockpitDefaults set ValueInBool=@showImage where Parameter='AdvikWebandon' and ValueInText='ShowImage' update CockpitDefaults set ValueInBool =@showVideo where Parameter = 'AdvikWebandon' and ValueInText = 'ShowVideo' update CockpitDefaults set ValueInText2=@scrollingText where Parameter='AdvikWebandon' and ValueInText='ScrollingText' update CockpitDefaults set ValueInInt=@flipInterval where Parameter='AdvikWebandon' and ValueInText='FlipInterval' update CockpitDefaults set ValueInInt=@refreshInterval where Parameter='AdvikWebandon' and ValueInText='RefreshInterval' update CockpitDefaults set ValueInInt=@datarefreshInterval where Parameter='AdvikWebandon' and ValueInText='DataRefreshInterval'", con);
                cmd.Parameters.AddWithValue("@header", header);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@noofRows", noofRows);
                cmd.Parameters.AddWithValue("@imagePath", imagePath);
                cmd.Parameters.AddWithValue("@videoPath", videoPath);
                cmd.Parameters.AddWithValue("@showImage", showImage);
                cmd.Parameters.AddWithValue("@showVideo", showVideo);
                cmd.Parameters.AddWithValue("@scrollingText", scrollingText);
                if (flipInterval != "")
                {
                    cmd.Parameters.AddWithValue("@flipInterval", Convert.ToInt32(flipInterval));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@flipInterval", DBNull.Value);
                }

                if (refreshInterval != "")
                    cmd.Parameters.AddWithValue("@refreshInterval", Convert.ToInt32(refreshInterval));
                else
                    cmd.Parameters.AddWithValue("@refreshInterval", DBNull.Value);
                if (datarefreshInterval != "")
                    cmd.Parameters.AddWithValue("@datarefreshInterval", Convert.ToInt32(datarefreshInterval));
                else
                    cmd.Parameters.AddWithValue("@datarefreshInterval", DBNull.Value);
                sdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                result1 = false;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }

        }
        public static void setPOColumnSettings(List<AndonSettingData> listData, out bool result2)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            result2 = true;
            try
            {
                foreach (AndonSettingData data in listData)
                {
                    SqlCommand cmd = new SqlCommand("update AndonDefaults set ValueInText2=@customName ,ValueInBool=@visibility where Parameter='AdvikWebandon' and ValueInText=@column", con);
                    cmd.Parameters.AddWithValue("@customName", data.CustomColumn);
                    cmd.Parameters.AddWithValue("@visibility", Convert.ToInt32(data.Visibility));
                    cmd.Parameters.AddWithValue("@column", data.Column);
                    sdr = cmd.ExecuteReader();
                    if (sdr != null) sdr.Close();
                }
            }
            catch (Exception ex)
            {
                result2 = false;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();

            }
        }
        #endregion

        #region AdvikStationHMI


        internal static DataTable GetAllHMIInfo()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("Select * from HMIInformation", conn);
            SqlDataReader rdr = null;
            DataTable dtHMIInfo = new DataTable();
            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtHMIInfo.Load(rdr);
                    dtHMIInfo.GetChanges();
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
            return dtHMIInfo;
        }


        internal static List<string> GetAssignedAndUnAssignedMachineIds(string query, string HMIId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@HMIID", HMIId);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["machineid"].ToString());
                }
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
            return list;
        }


        internal static int DeleteMachineToHMI(string selectedValue, string HMIID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "delete from HMIMachine where HMIID = @HMIID and MachineID = @machineID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@HMIID", HMIID);
                cmd.Parameters.AddWithValue("@machineID", selectedValue);
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }


        internal static int AddMachineToHMI(string selectedValue, string HMIID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "insert into HMIMachine (HMIID,MachineID) values (@HMIID, @MachineID)";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", selectedValue);
                cmd.Parameters.AddWithValue("@HMIID", HMIID);
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }


        internal static bool UpdateHMIInfo(string HMIId, string HMIDesc, string HMICode, out string message)
        {
            bool ok = false;
            message = "OK";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = @"if not exists(select * from HMIInformation where HMIID=@HMIID)
                                    BEGIN
                                    INSERT INTO [dbo].[HMIInformation] ([HMIID], [Description], [HMICode]) 
                                    VALUES (@HMIID, @Description, @HMICode)
                                    END
                                    else
                                    BEGIN
                                    UPDATE [dbo].[HMIInformation] SET Description=@Description, HMICode=@HMICode
									WHERE HMIID=@HMIID
                                    END";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@HMIID", HMIId);
                cmd.Parameters.AddWithValue("@Description", HMIDesc);
                cmd.Parameters.AddWithValue("@HMICode", HMICode);
                ok = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                message = ex.Message;
                ok = false;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return ok;
        }

        #endregion

        #region RegisterComp


        internal static int DeleteComponentRegisterMasterData(string machineID, string eventID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "delete from Advik_MXComponentDefaults where MachineID = @MachineID and EventID = @EventID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@EventID", eventID);
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }


        internal static List<RegisterCompEntity> GetRegisterDetails()
        {
            SqlCommand cmd = null;
            List<RegisterCompEntity> data = new List<RegisterCompEntity>();
            SqlDataReader rdr = null;
            SqlConnection conn = ConnectionManager.GetConnection(); int i = 1;
            string query = @"select * from Advik_MXComponentDefaults";
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    RegisterCompEntity entity = new RegisterCompEntity();
                    entity.SLNO = i++;
                    entity.MachineID = rdr["MachineID"].ToString();
                    entity.Operation = rdr["OperationNo"].ToString();
                    entity.EventID = rdr["EventID"].ToString();
                    entity.EventName = rdr["EventName"].ToString();
                    entity.EventType = rdr["EventType"].ToString();
                    entity.Register = rdr["RegisterValues"].ToString();
                    data.Add(entity);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return data;
        }



        internal static int UpdateComponentRegisterMasterData(string machineID, string Operationno, string eventType, string eventID, string eventName, string register)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = @"if not exists(select * from Advik_MXComponentDefaults where MachineID = @MachineID and EventID = @EventID)
begin
insert into Advik_MXComponentDefaults(MachineID,OperationNo,EventType,EventID,EventName,RegisterValues)
values(@MachineID,@OperationNo,@EventType,@EventID,@EventName,@RegisterValues)
end
else
begin
update Advik_MXComponentDefaults set OperationNo=@OperationNo ,EventType=@EventType, EventName=@EventName, RegisterValues=@RegisterValues where MachineID = @MachineID and EventID = @EventID
end";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@OperationNo", Operationno);
                cmd.Parameters.AddWithValue("@EventID", eventID);
                cmd.Parameters.AddWithValue("@EventType", eventType);
                cmd.Parameters.AddWithValue("@EventName", eventName);
                cmd.Parameters.AddWithValue("@RegisterValues", register);
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }

        #endregion

        #region Assign Down to machine

        internal static List<string> GetAssignedAndUnAssignedDowns(string query, string MachineId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineId", MachineId);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["downid"].ToString());
                }
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
            return list;
        }

        internal static int DeleteDownToMachine(string selectedValue, string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "delete from MachineDownCode where downid = @downid and MachineID = @MachineID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@downid", selectedValue);
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }

        internal static int AddDownToMachineID(string selectedValue, string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "insert into MachineDownCode (downid,MachineID) values (@downid, @MachineID)";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@downid", selectedValue);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }

        #endregion

        #region ------Employee Shift Association--------
        public static DataTable GetHelprequestRuleForEmployeeShift(string PlantID, string HelpRequestDescription)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("Plant ID", typeof(string));
            dt.Columns.Add("Employee ID", typeof(string));
            dt.Columns.Add("Shift1", typeof(bool));
            dt.Columns.Add("Shift2", typeof(bool));
            dt.Columns.Add("Shift3", typeof(bool));
            try
            {
                // SqlCommand cmd = new SqlCommand("s_GetEmployeeShiftDetails", conn);
                SqlCommand cmd = new SqlCommand("S_GetHelpRequestEmpShift", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PlantId", SqlDbType.NVarChar).Value = PlantID;
                cmd.Parameters.Add("@helpRequestType", SqlDbType.NVarChar).Value = HelpRequestDescription;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    string tempEmpid = string.Empty;
                    DataRow dr = dt.NewRow();
                    while (sdr.Read())
                    {
                        if (tempEmpid != Convert.ToString(sdr["EmpId"]))
                        {
                            if (tempEmpid != string.Empty) dt.Rows.Add(dr);
                            dr = dt.NewRow();
                        }
                        dr["Plant ID"] = Convert.ToString(sdr["PlantID"]);
                        dr["Employee ID"] = Convert.ToString(sdr["EmpId"]);
                        tempEmpid = Convert.ToString(sdr["EmpId"]);
                        if (!Convert.IsDBNull(sdr["isAssigned"]))
                        {
                            switch (Convert.ToInt32(sdr["isAssigned"]))
                            {
                                case 1:
                                    dr["Shift1"] = true;
                                    break;
                                case 2:
                                    dr["Shift2"] = true;
                                    break;
                                case 3:
                                    dr["Shift3"] = true;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Convert.ToInt32(sdr["shiftid"]))
                            {
                                case 1:
                                    dr["Shift1"] = false;
                                    break;
                                case 2:
                                    dr["Shift2"] = false;
                                    break;
                                case 3:
                                    dr["Shift3"] = false;
                                    break;
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
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
        internal static int InsertEmpShiftAssociation(string plant, string empname, int shiftid)
        {
            int success = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                int cnt = 0;
                SqlCommand cmd = new SqlCommand(@"if not exists(select * from HelpRequestShiftEmployee where PlantID=@plant and EmployeeID=@empname and shiftid=@shiftid)
begin
insert into HelpRequestShiftEmployee(PlantID,EmployeeID,shiftid) values(@plant,@empname,@shiftid)
end ", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", plant);
                cmd.Parameters.AddWithValue("@empname", empname);
                cmd.Parameters.AddWithValue("@shiftid", shiftid);
                cnt = cmd.ExecuteNonQuery();
                if (cnt > 0)
                {
                    success += cnt;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error while inserting Employee Shift Association data - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return success;
        }
        internal static int DeleteEmpShiftAssociation(string plant, string empname, int shiftid)
        {
            int rowsAffected = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                int cnt = 0;
                SqlCommand cmd = new SqlCommand(@"if exists(select * from HelpRequestShiftEmployee where PlantID=@plant and EmployeeID=@empname and shiftid=@shiftid)
begin
delete from HelpRequestShiftEmployee  where PlantID = @plant and EmployeeID = @empname and shiftid=@shiftid
end ", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", plant);
                cmd.Parameters.AddWithValue("@empname", empname);
                cmd.Parameters.AddWithValue("@shiftid", shiftid);
                cnt = cmd.ExecuteNonQuery();
                if (cnt > 0)
                {
                    rowsAffected += cnt;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error while deleting Employee shift Association data - " + ex.Message);
                rowsAffected = 0;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return rowsAffected;
        }
        #endregion

        #region -----Help Request Setting-----
        public static List<string> GetHelpActionId()
        {
            List<string> helpActionIds = new List<string>();
            string connString = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                //cmd = new SqlCommand("select Action from HelpCodeActionInfo where ActionNo in ('01') order by ActionNo", conn);
                cmd = new SqlCommand("select Action from HelpCodeActionInfo where ActionNo in  ('01','02','03','04') order by ActionNo", conn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["Action"]))
                        {
                            helpActionIds.Add(sdr["Action"].ToString());
                        }
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
                if (conn != null) conn.Close();
            }
            return helpActionIds;
        }

        public static List<EmployeeDetail> GetEmployeeDetail(string plantId)
        {
            List<EmployeeDetail> employeeDetails = new List<EmployeeDetail>();
            string sqlQuery = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                if (plantId == "All")
                {
                    sqlQuery = "select distinct ei.Employeeid,ei.phone from employeeinformation ei inner join PlantEmployee pe on pe.EmployeeID= ei.Employeeid order by ei.Employeeid";
                }
                else
                {
                    sqlQuery = "select ei.Employeeid,ei.phone from employeeinformation ei inner join PlantEmployee pe on pe.EmployeeID= ei.Employeeid where pe.PlantID='" + plantId + "'";
                }

                cmd = new SqlCommand(sqlQuery, conn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {

                    while (sdr.Read())
                    {
                        EmployeeDetail ed = new EmployeeDetail();
                        if (!Convert.IsDBNull(sdr["Employeeid"]))
                        {
                            ed.EmployeeId = sdr["Employeeid"].ToString();
                        }
                        if (!Convert.IsDBNull(sdr["phone"]))
                        {
                            ed.MobNo = sdr["phone"].ToString();
                        }
                        employeeDetails.Add(ed);
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
                if (conn != null) conn.Close();
            }
            return employeeDetails;
        }
        public static List<string> GetPlantIds(string logonEmployee)
        {
            List<string> PlantIds = new List<string>();
            string sqlQuery = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            if (CheckIsAdminEmployee(logonEmployee))
            {
                sqlQuery = "select distinct plantid from plantinformation order by plantid";
            }
            else
            {
                sqlQuery = "select distinct pe.PlantID from employeeinformation ei inner join PlantEmployee pe on pe.EmployeeID = ei.Employeeid where ei.Employeeid = '" + logonEmployee + "' order by pe.PlantID";
            }
            try
            {
                cmd = new SqlCommand(sqlQuery, conn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["plantid"]))
                        {
                            PlantIds.Add(sdr["plantid"].ToString());
                        }
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
                if (conn != null) conn.Close();
            }
            return PlantIds;
        }
        public static bool CheckIsAdminEmployee(string employee)
        {
            object obj = null;
            bool isAdmin = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select isadmin from employeeinformation where employeeid='" + employee + "'", conn);
                obj = cmd.ExecuteScalar();
                if (obj != null && !Convert.IsDBNull(obj))
                {
                    if (Convert.ToInt32(obj) == 1) isAdmin = true;
                    else isAdmin = false;
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
            return isAdmin;
        }

        public static bool CheckBusinessruleExistence(string plantId, string machineId, string helpRequestId, string helpAction)
        {
            bool allreadyPresent = false;
            object obj = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                //sqlQuery = "select hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action where P.PlantID='"+plantId+"' and hcm.Help_Description='"+ helpRequestId +"' and hca.Action='"+helpAction+"'";
                sqlQuery = "select hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action inner join machineinformation m on hr.machineid=m.InterfaceID where P.PlantID='" + plantId + "' and m.machineID='" + machineId + "' and hcm.Help_Description='" + helpRequestId + "' and hca.Action='" + helpAction + "'";
                cmd = new SqlCommand(sqlQuery, conn);
                obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    allreadyPresent = true;
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
            return allreadyPresent;
        }


        public static bool DeleteBusinessRulesBulk(string plantId, string machineId, string helpRequestId, string helpAction)
        {
            bool allreadyPresent = false;
            object obj = null;
            int slnNO = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                //sqlQuery = "select hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action where P.PlantID='"+plantId+"' and hcm.Help_Description='"+ helpRequestId +"' and hca.Action='"+helpAction+"'";
                sqlQuery = "select hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action inner join machineinformation m on hr.machineid=m.InterfaceID where P.PlantID='" + plantId + "' and m.machineID='" + machineId + "' and hcm.Help_Description='" + helpRequestId + "' and hca.Action='" + helpAction + "'";
                cmd = new SqlCommand(sqlQuery, conn);
                obj = cmd.ExecuteScalar();
                Int32.TryParse(Convert.ToString(obj), out slnNO);
                if (slnNO > 0)
                {
                    conn = ConnectionManager.GetConnection();
                    sqlQuery = "delete from helprequestrule where SlNo='" + slnNO + "'";
                    SqlCommand cmd1 = new SqlCommand(sqlQuery, conn);
                    cmd1.ExecuteNonQuery();
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
            return allreadyPresent;
        }
        public static void AddBusinessRules(string plantID, List<string> machineIds, string helpRequest, string actionId, string MobNoLevel1, string MobNoLevel2, string MobNoLevel3, int thresold, int SecndLvlthresold, string message)
        {
            //int threholdinSeconds = Convert.ToInt32(thresold) * 60;
            //int SecondLvlthreholdinSeconds = Convert.ToInt32(SecndLvlthresold) * 60;
            string sqlQuery = string.Empty;
            foreach (string machine in machineIds)
            {
                if (sqlQuery == string.Empty)
                {
                    sqlQuery = "INSERT INTO HelpRequestRule ([PlantId] ,[HelpCode],[Action],[MobileNo],[Level2Threshold],[Level2MobNo],[Level3Threshold],[Level3MobNo],[Message],[MachineId]) select P.PlantCode,HM.Help_code,HA.ActionNo,'" + MobNoLevel1 + "','" + thresold.ToString() + "','" + MobNoLevel2 + "','" + SecndLvlthresold + "','" + MobNoLevel3 + "','" + message + "',M.InterfaceID from plantinformation P,machineinformation M,HelpCodeActionInfo HA,HelpCodeMaster HM where P.PlantID='" + plantID + "' and M.MachineId='" + machine + "' and HM.Help_Description='" + helpRequest + "' and HA.Action='" + actionId + "'";
                }
                else
                {
                    sqlQuery = sqlQuery + ";" + "INSERT INTO HelpRequestRule ([PlantId] ,[HelpCode],[Action],[MobileNo],[Level2Threshold],[Level2MobNo],[Level3Threshold],[Level3MobNo],[Message],[MachineId]) select P.PlantCode,HM.Help_code,HA.ActionNo,'" + MobNoLevel1 + "','" + thresold.ToString() + "','" + MobNoLevel2 + "','" + SecndLvlthresold + "','" + MobNoLevel3 + "','" + message + "',M.InterfaceID from plantinformation P,machineinformation M,HelpCodeActionInfo HA,HelpCodeMaster HM where P.PlantID='" + plantID + "' and M.MachineId='" + machine + "' and HM.Help_Description='" + helpRequest + "' and HA.Action='" + actionId + "'";
                }
            }
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                //sqlQuery = "INSERT INTO HelpRequestRule select P.PlantCode,HM.Help_code,HA.ActionNo,'" + MobNoLevel1 + "','" + threholdinSeconds .ToString()+ "','" + MobNoLevel2 + "','" + message + "' from plantinformation P,HelpCodeActionInfo HA,HelpCodeMaster HM where P.PlantID='" + plantID + "' and HM.Help_Description='" + helpRequest + "' and HA.Action='" + actionId + "'";
                cmd = new SqlCommand(sqlQuery, conn);
                int ret = cmd.ExecuteNonQuery();
                //if (ret > 0)
                //{
                //    MessageBox.Show("HelpRequest rule added Successfully.");
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }
        public static SqlDataReader GetHelpRequestRule(string plantId, System.Web.UI.WebControls.ListBox clMachine, string helpRequestCode, string helpRequestAction, string MobNo, bool isFilterdRule)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            if (!isFilterdRule)
            {
                sqlQuery = "select p.PlantID,M.machineID,hcm.Help_Description,hca.Action,hr.MobileNo,hr.Level2Threshold,hr.Level2MobNo,hr.Level3Threshold,hr.Level3MobNo,hr.Message,hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action inner join machineinformation m on hr.machineid=m.InterfaceID order by hr.slNo desc";
            }
            else
            {
                // sqlQuery = "select p.PlantID,M.machineID,hcm.Help_Description,hca.Action,hr.MobileNo,hr.Threshold,hr.Level2MobNo,hr.Message,hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action inner join machineinformation m on hr.machineid=m.InterfaceID where p.PlantID='PRP and VSCP' and m.machineID in ('I HARD','Friction 2') and hcm.Help_Description='Maintenance' and hca.Action='Initiated' and hr.MobileNo like '9900307956%' order by hr.slNo desc";
                sqlQuery = "select p.PlantID,M.machineID,hcm.Help_Description,hca.Action,hr.MobileNo,hr.Level2Threshold,hr.Level2MobNo,hr.Level3Threshold,hr.Level3MobNo,hr.Message,hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action inner join machineinformation m on hr.machineid=m.InterfaceID";
                if (plantId != string.Empty)
                {
                    if (plantId == "All")
                    {

                    }
                    else
                    {
                        sqlQuery = string.Format("{0}{1}", sqlQuery, " where p.PlantID='" + plantId + "'");
                    }
                }
                if (clMachine.Items.Count > 0)
                {
                    List<string> helprequestmachines = new List<string>();
                    //string machineIds = string.Empty;
                    //foreach (ListItem item in clMachine.Items)
                    //{
                    //    if (machineIds == string.Empty)
                    //    {
                    //        machineIds = string.Format("'{0}'", item.Value);
                    //    }
                    //    else
                    //    {
                    //        machineIds = string.Format("{0},'{1}'", machineIds, item.Value);
                    //    }
                    //}
                    //machineIds = string.Format("({0})", machineIds);
                    //sqlQuery = string.Format("{0}{1}{2}", sqlQuery, " and m.machineID in", machineIds);
                    string machineIds = string.Empty;
                    foreach (ListItem item in clMachine.Items)
                    {
                        if (item.Selected == true)
                        {
                            machineIds += "'" + item.Value + "',";
                            helprequestmachines.Add(item.Value);
                        }

                    }
                    HttpContext.Current.Session["HelpRequestMachines"] = helprequestmachines;
                    if (machineIds.Length > 0)
                    {
                        machineIds = machineIds.Remove(machineIds.Length - 1);
                    }
                    else
                    {
                        machineIds = "''";
                    }
                    machineIds = string.Format("({0})", machineIds);
                    sqlQuery = string.Format("{0}{1}{2}", sqlQuery, " and m.machineID in", machineIds);

                }
                if (helpRequestCode != string.Empty)
                {
                    if (plantId == "All" && helpRequestCode == "All")
                    {

                    }
                    else if (plantId == "All" && helpRequestCode != "All")
                    {
                        sqlQuery = string.Format("{0}{1}", sqlQuery, " where hcm.Help_Description='" + helpRequestCode + "'");
                    }
                    else if (plantId != "All" && helpRequestCode == "All")
                    {
                    }
                    else
                    {
                        sqlQuery = string.Format("{0}{1}", sqlQuery, "and hcm.Help_Description='" + helpRequestCode + "'");
                    }
                }
                if (helpRequestAction != string.Empty)
                {
                    if (helpRequestAction == "All")
                    {
                    }
                    else
                    {
                        sqlQuery = string.Format("{0}{1}", sqlQuery, " and hca.Action='" + helpRequestAction + "'");
                    }
                }
                if (MobNo != string.Empty)
                {
                    if (MobNo == "All")
                    {
                    }
                    else
                    {
                        sqlQuery = string.Format("{0}{1}", sqlQuery, " and hr.MobileNo like '%" + MobNo + "%'");
                    }
                }

                if (plantId == "All" && helpRequestCode == "All" && helpRequestAction == "All" && MobNo == "All" && clMachine.Items.Count <= 0)
                {
                    sqlQuery = "select p.PlantID,M.machineID,hcm.Help_Description,hca.Action,hr.MobileNo,hr.Level2Threshold,hr.Level2MobNo,hr.Level3Threshold,hr.Level3MobNo,hr.Message,hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action inner join machineinformation m on hr.machineid=m.InterfaceID ";
                }
                sqlQuery = string.Format("{0}{1}", sqlQuery, "order by hr.slNo desc");
            }

            try
            {
                //sqlQuery = "select * from helprequestrule";
                // sqlQuery = "select p.PlantID,hcm.Help_Description,hca.Action,hr.MobileNo,hr.Threshold,hr.Level2MobNo,hr.Message,hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action order by hr.slNo desc";

                cmd = new SqlCommand(sqlQuery, conn);
                sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                //if (sdr != null) sdr.Close();
                //if (conn != null) conn.Close();
            }
            return sdr;
        }

        public static SqlDataReader GetBusinessRule(string slNo)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {

                sqlQuery = "select p.PlantID,m.machineID,hcm.Help_Description,hca.Action,hr.MobileNo,hr.Level2Threshold,hr.Level2MobNo,hr.Level3Threshold,hr.Level3MobNo,hr.Message,hr.slNo from plantinformation p inner join helprequestrule hr on hr.plantId=p.PlantCode inner join HelpCodeMaster hcm on hcm.Help_Code=hr.HelpCode inner join HelpCodeActionInfo hca on hca.ActionNo= hr.Action inner join machineinformation m on m.InterfaceId=hr.MachineId where hr.slNo='" + slNo + "'";
                cmd = new SqlCommand(sqlQuery, conn);
                sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                //if (sdr != null) sdr.Close();
                //if (conn != null) conn.Close();
            }
            return sdr;
        }
        public static List<string> GetShifIDs()
        {
            List<string> helpShiftIds = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select 'Shift-' + ShiftName as shiftId from shiftdetails where running = 1 order by shiftid", conn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["shiftId"]))
                        {
                            helpShiftIds.Add(sdr["shiftId"].ToString());
                        }
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
                if (conn != null) conn.Close();
            }
            return helpShiftIds;
        }
        #endregion

        internal static List<string> GetCell(string selectedPlant)
        {
            List<string> machines = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            string query = @"[s_GetLookups]";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Group");
                cmd.Parameters.AddWithValue("@filter", selectedPlant.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : selectedPlant);
                cmd.CommandTimeout = 120;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        machines.Add(reader["GroupID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            machines.Insert(0, "All");
            return machines;
        }
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

        #region -----Audit Date Details--
        internal static List<AudiDateDetails> getAuditDateDetails(string year)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<AudiDateDetails> listParameter = new List<AudiDateDetails>();
            AudiDateDetails parameter = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_Get_AuditDate_Advik]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", year);
                cmd.Parameters.AddWithValue("@Param", "View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            parameter = new AudiDateDetails();
                            if (sdr["AuditDate"] != null)
                            {
                                if (sdr["AuditDate"].ToString() != "")
                                {
                                    DateTime dayValue = new DateTime(Convert.ToDateTime(sdr["AuditDate"].ToString()).Year, Convert.ToDateTime(sdr["AuditDate"].ToString()).Month, Convert.ToDateTime(sdr["AuditDate"].ToString()).Day);


                                    parameter.AuditDate = dayValue.ToString("dddd") + " "+ Convert.ToDateTime(sdr["AuditDate"].ToString()).ToString("dd-MMM-yyyy");
                                  
                                   // parameter.Day = Convert.ToDateTime(sdr["AuditDate"].ToString()).ToString("dd") + " " + dayValue.ToString("dddd") + " " + Convert.ToDateTime(sdr["AuditDate"].ToString()).ToString("yyyy");
                                }
                                else
                                {
                                    parameter.AuditDate = sdr["AuditDate"].ToString();
                                    parameter.Day = "";
                                }
                            }
                            else
                            {
                                parameter.AuditDate = sdr["AuditDate"].ToString();
                                parameter.Day = "";
                            }




                            listParameter.Add(parameter);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getAuditDateDetails- " + ex.Message);
                        }
                    }
                }
                else
                {
                    parameter = new AudiDateDetails();
                    listParameter.Add(parameter);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listParameter;
        }

        internal static string generateAuditDates(string year)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            string result = "";
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_Get_AuditDate_Advik]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", year);
                cmd.Parameters.AddWithValue("@Param", "Create");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        result = sdr["Flag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Error";
                Logger.WriteErrorLog("Error in generate AuditDates - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static int reGenerateAuditDates(string year)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            int result =0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_Get_AuditDate_Advik]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", year);
                cmd.Parameters.AddWithValue("@Param", "Recreate");
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = -2;
                Logger.WriteErrorLog("Error in reGenerateAuditDates - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }

        #endregion

        #region -----Holiday List

        internal static int deleteHolidayDetails(string query)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            int result = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = -2;
                Logger.WriteErrorLog("Error in deleteHolidayDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }

        internal static int saveupdateHolidayDetails(string query)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            int result = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = -2;
                Logger.WriteErrorLog("Error in saveupdateHolidayDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }

        internal static List<string> getAllReasons()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> listParameter = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct Reason from HolidayList", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            listParameter.Add(sdr["Reason"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getAllReasons- " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listParameter;
        }

        internal static List<string> getMachineForPlant(string plant)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> listParameter = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct MachineID from Plantmachine where (PlantID=@plantid or isnull(@plantid,'')='')", con);
                cmd.Parameters.AddWithValue("@plantid", plant);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            listParameter.Add(sdr["MachineID"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getMachineForPlant- " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listParameter;
        }
        internal static List<HolidayListDetails> getHolidayList(string value, string param)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<HolidayListDetails> listParameter = new List<HolidayListDetails>();
            HolidayListDetails parameter = null;
            try
            {
                if (param == "ByDate")
                {
                    cmd = new SqlCommand("select * from HolidayList where Holiday=@value", con);
                    cmd.Parameters.AddWithValue("@value", value);
                }
                else if (param == "ByReason")
                {
                    cmd = new SqlCommand("select * from HolidayList where Reason=@value", con);
                    cmd.Parameters.AddWithValue("@value", value);
                }
                else if (param == "ByMachine")
                {
                    cmd = new SqlCommand("select * from HolidayList where MachineID in (" + value + ")", con);
                }
                else
                {
                    cmd = new SqlCommand("select * from HolidayList", con);
                }

                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            parameter = new HolidayListDetails();
                            if (sdr["Holiday"] != null)
                            {
                                if (sdr["Holiday"].ToString() != "")
                                {
                                    //DateTime date = DateTime.Now.Date;
                                    //string s = sdr["Holiday"].ToString();
                                    //date = Util.GetDateTime(sdr["Holiday"].ToString());
                                    //parameter.Holiday = date.ToString("dd-MMM-yyyy");
                                    parameter.Holiday = convertDateTimeToSpecificFormat(sdr["Holiday"].ToString());
                                }
                                else
                                {
                                    parameter.Holiday = sdr["Holiday"].ToString();
                                }

                            }
                            else
                            {
                                parameter.Holiday = sdr["Holiday"].ToString();
                            }

                            parameter.Reason = sdr["Reason"].ToString();
                            parameter.MachineID = sdr["MachineID"].ToString();
                            listParameter.Add(parameter);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getHolidayList- " + ex.Message);
                        }
                    }
                }
                else
                {
                    parameter = new HolidayListDetails();
                    listParameter.Add(parameter);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listParameter;
        }
        private static string convertDateTimeToSpecificFormat(string datetime)
        {
            string formateddatetime = "";
            try
            {
                formateddatetime = Convert.ToDateTime(datetime).ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return formateddatetime;
        }
        #endregion

        #region ---- JH Production Head Details ------
        //internal static List<JHDashboardDetails> getJHProdHeadObservationDetails(string plant, string machineid, string shift, string jhtype, DateTime fromdate, string todate, string GroupID, int noOfSelectedShift)
        //{
        //    List<JHDashboardDetails> listJHDetails = new List<JHDashboardDetails>();
        //    JHDashboardDetails jhdetails = null;
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    SqlDataReader reader = null;
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("[dbo].[S_View_JHChecklistProdHeadDashboard_Advik]", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Machine", machineid);
        //        cmd.Parameters.AddWithValue("@JHType", jhtype);
        //        cmd.Parameters.AddWithValue("@PlantID", plant);
        //        cmd.Parameters.AddWithValue("@GroupID", GroupID);
        //        cmd.Parameters.AddWithValue("@Shift", shift);
        //        cmd.Parameters.AddWithValue("@StartDate", fromdate);
        //        cmd.Parameters.AddWithValue("@Enddate", todate);
        //        cmd.CommandTimeout = 40000;
        //        reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {

        //            int i = 0;
        //            while (reader.Read())
        //            {

        //                jhdetails = new JHDashboardDetails();
        //                jhdetails.Date = Convert.ToDateTime(reader["AuditDate"].ToString()).ToString("dd-MMM-yyyy");
        //                jhdetails.AuditDate = reader["AuditDate"].ToString();
        //                jhdetails.Shift = reader["ShiftName"].ToString();
        //                jhdetails.Machine = reader["MachineId"].ToString();
        //                if (reader["ChecklistStatus"].ToString() == "OK")
        //                {
        //                    jhdetails.ProdHeadObservation = true;
        //                }
        //                else
        //                {
        //                    jhdetails.ProdHeadObservation = false;
        //                }
        //                jhdetails.ChkBoxVisibility = "none";
        //                jhdetails.ChkRowSpan = "1";
        //                jhdetails.JHType = reader["JHType"].ToString();
        //                jhdetails.OperatorStatus = reader["OperatorStatus"].ToString();
        //                jhdetails.SupervisorStatus = reader["SupervisorStatus"].ToString();
        //                if (i % noOfSelectedShift == 0)
        //                {
        //                    jhdetails.CellVisibility1 = "table-cell";
        //                    jhdetails.RowSpan = noOfSelectedShift.ToString();
        //                }
        //                else
        //                {
        //                    jhdetails.CellVisibility1 = "none";
        //                    jhdetails.RowSpan = noOfSelectedShift.ToString();
        //                }
        //                listJHDetails.Add(jhdetails);
        //                i++;
        //            }
        //            listJHDetails[0].ChkBoxVisibility = "table-cell";
        //            listJHDetails[0].ChkRowSpan = i.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.Message);
        //    }
        //    finally
        //    {
        //        if (reader != null) reader.Close();
        //        if (conn != null) conn.Close();
        //    }
        //    return listJHDetails;
        //}
        internal static List<JHDashboardDetails> getJHProdHeadObservationDetails(string plant, string machineid, string shift, string jhtype, DateTime fromdate, string todate, string GroupID, int noOfSelectedShift)
        {
            List<JHDashboardDetails> listJHDetails = new List<JHDashboardDetails>();
            JHDashboardDetails jhdetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_View_JHChecklistProdHeadDashboard_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machine", machineid);
                cmd.Parameters.AddWithValue("@JHType", jhtype);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@Enddate", todate);
                cmd.CommandTimeout = 40000;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    int i = 0;
                    string tempDate = "", tempMachine = "";
                    int firstRecord = 0;
                    int incrementCount = 1;
                    while (reader.Read())
                    {

                        jhdetails = new JHDashboardDetails();
                        jhdetails.Date = Convert.ToDateTime(reader["AuditDate"].ToString()).ToString("dd-MMM-yyyy");
                        jhdetails.AuditDate = reader["AuditDate"].ToString();
                        jhdetails.Shift = reader["ShiftName"].ToString();
                        jhdetails.Machine = reader["MachineId"].ToString();
                        if (reader["ChecklistStatus"].ToString() == "OK")
                        {
                            jhdetails.ProdHeadObservation = true;
                        }
                        else
                        {
                            jhdetails.ProdHeadObservation = false;
                        }
                        jhdetails.ChkBoxVisibility = "none";
                        jhdetails.ChkRowSpan = "1";
                        jhdetails.JHType = reader["JHType"].ToString();
                        jhdetails.OperatorStatus = reader["OperatorStatus"].ToString();
                        jhdetails.SupervisorStatus = reader["SupervisorStatus"].ToString();
                        if (jhdetails.Date != tempDate || jhdetails.Machine != tempMachine)
                        {
                            if (i != 0)
                            {
                                listJHDetails[firstRecord].RowSpan = incrementCount.ToString();
                            }
                            jhdetails.CellVisibility1 = "table-cell";
                            // jhdetails.RowSpan = noOfSelectedShift.ToString();
                            firstRecord = i;
                            incrementCount = 1;
                        }
                        else
                        {
                            incrementCount++;
                            jhdetails.CellVisibility1 = "none";
                            jhdetails.RowSpan = "1";
                        }
                        tempDate = jhdetails.Date;
                        tempMachine = jhdetails.Machine;
                        //if (i % noOfSelectedShift == 0)
                        //{
                        //    jhdetails.CellVisibility1 = "table-cell";
                        //    jhdetails.RowSpan = noOfSelectedShift.ToString();
                        //}
                        //else
                        //{
                        //    jhdetails.CellVisibility1 = "none";
                        //    jhdetails.RowSpan = noOfSelectedShift.ToString();
                        //}
                        listJHDetails.Add(jhdetails);
                        i++;
                    }
                    listJHDetails[firstRecord].RowSpan = incrementCount.ToString();
                    listJHDetails[0].ChkBoxVisibility = "table-cell";
                    listJHDetails[0].ChkRowSpan = i.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return listJHDetails;
        }
        internal static int SaveProductionHeadObservationDetails(JHDashboardDetails data)
        {
            int success = 0;
            string query = "";
            //            query = @"if exists(select * from JHChecklistProdHeadTransaction_Advik where MachineID=@machine and JHType=@jhtype and Shift=@shift)
            //begin
            //	update JHChecklistProdHeadTransaction_Advik set Result=@chkValue,UpdatedTS=@updatedts,ProdHeadName=@empid  where MachineID=@machine and  JHType=@jhtype and Shift=@shift
            //end
            //else
            //begin
            //	insert into JHChecklistProdHeadTransaction_Advik(MachineID,JHType,Result,UpdatedTS,ProdHeadName,AuditDate,Shift) values(@machine,@jhtype,@chkValue,@updatedts,@empid,@auditdate,@shift)
            //end";
            query = @"if exists(select * from JHChecklistProdHeadTransaction_Advik where MachineID=@machine and JHType=@jhtype and Shift=@shift and AuditDate=@auditdate)
begin
	update JHChecklistProdHeadTransaction_Advik set Result=@chkValue,UpdatedTS=@updatedts,ProdHeadName=@empid  where MachineID=@machine and  JHType=@jhtype and Shift=@shift and AuditDate=@auditdate
end
else
begin
	insert into JHChecklistProdHeadTransaction_Advik(MachineID,JHType,Result,UpdatedTS,ProdHeadName,AuditDate,Shift) values(@machine,@jhtype,@chkValue,@updatedts,@empid,@auditdate,@shift)
end";
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                if (query != "")
                {
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    cmd.Parameters.AddWithValue("@machine", data.Machine);
                    //cmd.Parameters.AddWithValue("@auditdate", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@auditdate", Util.GetDateTime(data.AuditDate).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@jhtype", data.JHType);
                    cmd.Parameters.AddWithValue("@chkValue", data.Status);
                    cmd.Parameters.AddWithValue("@updatedts", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@empid", data.ProdHeadName);
                    cmd.Parameters.AddWithValue("@shift", data.Shift);
                    success = cmd.ExecuteNonQuery();
                }
                else
                {
                    success = 1;
                }

            }
            catch (Exception ex)
            {
                success = 0;
                Logger.WriteErrorLog("While saving JH production Head Observation details" + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }
        internal static int SaveProdSupEmailsDetails(string shift, string RunReportForEvery, string ReportName)
        {
            int success = 0;
            string query = @"update ScheduledReports set runhistory=@Value , RunReportForEvery='Now' where ReportName='JH Audit report - On Demand'";
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                if (query != "")
                {
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    cmd.Parameters.AddWithValue("@Value", DBNull.Value);
                    //cmd.Parameters.AddWithValue("@RunReportForEvery", RunReportForEvery);
                    //cmd.Parameters.AddWithValue("@reportname", ReportName);
                    success = cmd.ExecuteNonQuery();
                }
                else
                {
                    success = 1;
                }

            }
            catch (Exception ex)
            {
                success = 0;
                Logger.WriteErrorLog("While updating JH production Head and supervisor Email details" + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }
        #endregion

        #region ---- JH Supervisor Observation -----
        internal static List<JHDashboardDetails> getJHSupervisorObservationdetails(string plant, string machineid, string shift, string jhtype, DateTime fromdate, DateTime todate, string GroupID)
        {
            List<JHDashboardDetails> listJHDetails = new List<JHDashboardDetails>();
            JHDashboardDetails jhdetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_View_JHChecklistSupervisorDashboard_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machine", machineid);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@JHType", jhtype);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@Enddate", todate);
                cmd.CommandTimeout = 40000;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        jhdetails = new JHDashboardDetails();
                        jhdetails.Date = Convert.ToDateTime(reader["AuditDate"].ToString()).ToString("dd-MMM-yyyy");
                        jhdetails.AuditDate = reader["AuditDate"].ToString();
                        jhdetails.Shift = reader["ShiftName"].ToString();
                        jhdetails.Machine = reader["MachineId"].ToString();
                        jhdetails.JHType = reader["JHType"].ToString();
                        jhdetails.OperatorStatus = reader["OperatorStatus"].ToString();
                        if (reader["ChecklistStatus"].ToString() == "OK")
                        {
                            jhdetails.SupervisorObservation = true;
                        }
                        else
                        {
                            jhdetails.SupervisorObservation = false;
                        }
                        if (reader["AuditCheck"].ToString() == "1")
                        {
                            jhdetails.BackColor = "auditRowColorCss";
                        }
                        listJHDetails.Add(jhdetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return listJHDetails;
        }
        internal static int SaveSupervisorDetails(JHDashboardDetails data)
        {
            int success = 0;
            string query = "";
            query = @"if exists(select * from JHChecklistSupervisorTransaction_Advik where MachineID=@machine and UpdatedTS=@updatedts and Shift=@shift and JHType=@jhtype)
            begin
            	update JHChecklistSupervisorTransaction_Advik set Result=@chkValue,AuditDate=@auditdate,SupervisorName=@empid where  MachineID=@machine and UpdatedTS=@updatedts and Shift=@shift and JHType=@jhtype
            end
            else
            begin
            	insert into JHChecklistSupervisorTransaction_Advik(MachineID,JHType,Result,UpdatedTS,SupervisorName,AuditDate,Shift) values(@machine,@jhtype,@chkValue,@updatedts,@empid,@auditdate,@shift)
            end ";
            //            query = @"if exists(select * from JHChecklistSupervisorTransaction_Advik where MachineID=@machine and UpdatedTS=@updatedts and Shift=@shift and JHType=@jhtype and AuditDate=@auditdate)
            //begin
            //	update JHChecklistSupervisorTransaction_Advik set Result=@chkValue,SupervisorName=@empid where  MachineID=@machine and UpdatedTS=@updatedts and Shift=@shift and JHType=@jhtype and AuditDate=@auditdate
            //end
            //else
            //begin
            //	insert into JHChecklistSupervisorTransaction_Advik(MachineID,JHType,Result,UpdatedTS,SupervisorName,AuditDate,Shift) values(@machine,@jhtype,@chkValue,@updatedts,@empid,@auditdate,@shift)
            //end ";
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                if (query != "")
                {
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    cmd.Parameters.AddWithValue("@machine", data.Machine);
                    cmd.Parameters.AddWithValue("@auditdate", DateTime.Now.ToString("yyyy-MM-dd"));
                    //cmd.Parameters.AddWithValue("@auditdate", Util.GetDateTime(data.Date).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@shift", data.Shift);
                    cmd.Parameters.AddWithValue("@jhtype", data.JHType);
                    cmd.Parameters.AddWithValue("@chkValue", data.SupervisorStatus);
                    cmd.Parameters.AddWithValue("@updatedts", Convert.ToDateTime(data.Date.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@empid", data.SupervisorName);
                    success = cmd.ExecuteNonQuery();
                }
                else
                {
                    success = 1;
                }

            }
            catch (Exception ex)
            {
                success = 0;
                Logger.WriteErrorLog("While saving JH Supervisor Observation details" + ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return success;
        }
        #endregion

        internal static System.Data.DataSet Getchecklistdata(string machineID, string shift, DateTime fromDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataSet dt = new DataSet();
            DataTable shift1val = new DataTable();
            DataTable shift2val = new DataTable();
            DataTable shift3val = new DataTable();
            DataTable shift1Oprsupval = new DataTable();
            DataTable shift2Oprsupval = new DataTable();
            DataTable shift3Oprsupval = new DataTable();
            try
            {
                cmd = new SqlCommand("s_GetJHCheckListReport_Advik", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", shift.Equals("All",StringComparison.OrdinalIgnoreCase)?"":shift);
                rdr = cmd.ExecuteReader();
                if(shift.Equals("All"))
                {
                    shift1val.Load(rdr);
                    shift2val.Load(rdr);
                    shift3val.Load(rdr);
                    shift1Oprsupval.Load(rdr);
                    shift2Oprsupval.Load(rdr);
                    shift3Oprsupval.Load(rdr);
                    dt.Tables.Add(shift1val);
                    dt.Tables.Add(shift1Oprsupval);
                    dt.Tables.Add(shift2val);
                    dt.Tables.Add(shift2Oprsupval);
                    dt.Tables.Add(shift3val);
                    dt.Tables.Add(shift3Oprsupval);
                }
                else
                {
                    shift1val.Load(rdr);
                    shift1Oprsupval.Load(rdr);
                    dt.Tables.Add(shift1val);
                    dt.Tables.Add(shift1Oprsupval);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }

    }
}