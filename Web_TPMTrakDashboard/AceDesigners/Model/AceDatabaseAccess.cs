using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AceDesigners.Model
{
    public class AceDatabaseAccess
    {
        internal static List<ScheduleEntity> getScheduleDetails(string machine, string status, string fromDate, string toDate, string compId, string PO, string viewType, string sapStatus, string MRP_Controller, out DataTable dtForValidation)
        {
            dtForValidation = new DataTable();
            List<ScheduleEntity> list = new List<ScheduleEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            ScheduleEntity data = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_ACE_SAP_GetScheduleDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 500;
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@SCHStatus", status);
                cmd.Parameters.AddWithValue("@SAPStatus", sapStatus);
                if (viewType.Equals("Date", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ProductionOrder", PO);
                    cmd.Parameters.AddWithValue("@ComponentID", compId);
                }
                cmd.Parameters.AddWithValue("@MRP_Controller", MRP_Controller.Equals("None", StringComparison.OrdinalIgnoreCase) ? "" : MRP_Controller);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new ScheduleEntity();
                        data.ID = rdr["IDD"].ToString();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.ProductionOrder = rdr["ProductionOrder"].ToString();
                        data.CompID = rdr["ComponentID"].ToString();
                        data.OpnNo = rdr["OperationNo"].ToString();
                        data.StdCycleTime = rdr["StdCycleTime"].ToString();
                        data.PlannedQty = rdr["ScheduleQty"].ToString();
                        data.ScheduleDate = string.IsNullOrEmpty(rdr["ScheduleDate"].ToString()) ? "" : Util.GetDateTime(rdr["ScheduleDate"].ToString()).ToString("dd-MM-yyyy");
                        data.SchedulePriority = rdr["SchedulePriority"].ToString();
                        data.CompletedQty = rdr["DeliveredQty"].ToString();
                        data.PalletNo = rdr["PalletNo"].ToString();
                        data.Sequence = rdr["Sequence"].ToString();
                        data.Status = rdr["SCHStatus"].ToString();
                        data.SAPStatus = rdr["SAPStatus"].ToString();
                        data.UpdatedTsHMI = string.IsNullOrEmpty(rdr["HMI_UpdatedTS"].ToString()) ? "" : Util.GetDateTime(rdr["HMI_UpdatedTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        data.SendToHMI = rdr["Select_Schedule_ToHMI"].ToString();
                        data.chkEnabled = false;
                        if ((!string.IsNullOrEmpty(data.ScheduleDate) && !string.IsNullOrEmpty(data.SchedulePriority))
                            && (data.SAPStatus.Equals("Rel", StringComparison.OrdinalIgnoreCase) || data.SAPStatus.Equals("PCNF", StringComparison.OrdinalIgnoreCase))
                            && (data.Status.Equals("New", StringComparison.OrdinalIgnoreCase) || data.Status.Equals("Hold", StringComparison.OrdinalIgnoreCase))
                            && (data.SendToHMI != "0" || (data.SendToHMI == "0" && data.Status.Equals("Hold", StringComparison.OrdinalIgnoreCase))))
                        {
                            data.chkEnabled = true;
                        }
                        if ((data.Status.Equals("New", StringComparison.OrdinalIgnoreCase) || data.Status.Equals("Hold", StringComparison.OrdinalIgnoreCase)) && (data.SendToHMI != "0" || (data.SendToHMI == "0" && data.Status.Equals("Hold", StringComparison.OrdinalIgnoreCase))))

                        {
                            data.ControlEnabled = true;
                        }
                        else
                        {
                            data.ControlEnabled = false;
                            //data.chkEnabled = false;
                        }
                        data.JobType = rdr["JobType"].ToString();
                        if ((data.JobType.Equals("Development", StringComparison.OrdinalIgnoreCase)))
                        {
                            data.ChkJobType = true;
                        }
                        else
                        {
                            data.ChkJobType = false;
                        }
                        list.Add(data);
                    }
                    rdr.NextResult();
                    dtForValidation.Load(rdr);
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
        internal static int saveScheduleDetails(ScheduleEntity data)
        {

            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                //cmd = new SqlCommand(@"update ACE_SAP_ScheduleDetails set SchedulePriority=@SchedulePriority,ScheduleDate=@ScheduleDate,Sequence=@Sequence,UpdatedBy=@UpdatedBy,
                //UpdatedTS=@UpdatedTS,JobType=@JobType where IDD=@IDD", conn);
                cmd = new SqlCommand(@"update ACE_SAP_ScheduleDetails set SchedulePriority=@SchedulePriority,ScheduleDate=@ScheduleDate,Sequence=@Sequence,UpdatedBy=@UpdatedBy,
                UpdatedTS=@UpdatedTS,JobType=@JobType
                where IDD in (
                Select distinct A1.IDD from ACE_SAP_ScheduleDetails A1
                inner join PlantMachineGroups A2 on A1.MachineID=A2.MachineID
                inner join (Select distinct ProductionOrder,ComponentID,OperationNo,ACE_SAP_ScheduleDetails.MachineID,PlantMachineGroups.GroupID from ACE_SAP_ScheduleDetails 
                inner join PlantMachineGroups on PlantMachineGroups.MachineID=ACE_SAP_ScheduleDetails.MachineID
                where IDD in (@IDD))A3 on A1.ProductionOrder=A3.ProductionOrder and A1.ComponentID=A3.ComponentID and A1.OperationNo=A3.OperationNo and A2.GroupID=A3.GroupID
                )", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                if (string.IsNullOrEmpty(data.SchedulePriority))
                    cmd.Parameters.AddWithValue("@SchedulePriority", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@SchedulePriority", data.SchedulePriority);

                if (string.IsNullOrEmpty(data.ScheduleDate))
                    cmd.Parameters.AddWithValue("@ScheduleDate", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ScheduleDate", Util.GetDateTime(data.ScheduleDate).ToString("yyyy-MM-dd HH:mm:ss"));

                if (string.IsNullOrEmpty(data.Sequence))
                    cmd.Parameters.AddWithValue("@Sequence", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Sequence", data.Sequence);
                cmd.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@IDD", data.ID);
                cmd.Parameters.AddWithValue("@JobType", data.JobType);
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

        internal static int saveScheduleStatusDetails(ScheduleEntity data)
        {

            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"update ACE_SAP_ScheduleDetails set UpdatedBy=@UpdatedBy, UpdatedTS=@UpdatedTS,SCHStatus=@Status
                where IDD in (
                Select distinct A1.IDD from ACE_SAP_ScheduleDetails A1
                inner join PlantMachineGroups A2 on A1.MachineID=A2.MachineID
                inner join (Select distinct ProductionOrder,ComponentID,OperationNo,ACE_SAP_ScheduleDetails.MachineID,PlantMachineGroups.GroupID from ACE_SAP_ScheduleDetails 
                inner join PlantMachineGroups on PlantMachineGroups.MachineID=ACE_SAP_ScheduleDetails.MachineID
                where IDD in (@IDD))A3 on A1.ProductionOrder=A3.ProductionOrder and A1.ComponentID=A3.ComponentID and A1.OperationNo=A3.OperationNo and A2.GroupID=A3.GroupID
                )", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                cmd.Parameters.AddWithValue("@UpdatedBy", data.UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@IDD", data.ID);
                cmd.Parameters.AddWithValue("@Status", data.Status);
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

        internal static int sendScheduleDataToHMI(string iddList)
        {

            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"update ACE_SAP_ScheduleDetails set Select_Schedule_ToHMI=1,HMI_UpdatedTS=null where IDD in (
                Select distinct A1.IDD from ACE_SAP_ScheduleDetails A1
                inner join PlantMachineGroups A2 on A1.MachineID=A2.MachineID inner join (Select distinct ProductionOrder,ComponentID,OperationNo,ACE_SAP_ScheduleDetails.MachineID,PlantMachineGroups.GroupID from ACE_SAP_ScheduleDetails 
                inner join PlantMachineGroups on PlantMachineGroups.MachineID=ACE_SAP_ScheduleDetails.MachineID
                where IDD in (" + iddList + ")) A3 on A1.ProductionOrder=A3.ProductionOrder and A1.ComponentID=A3.ComponentID and A1.OperationNo=A3.OperationNo and A2.GroupID=A3.GroupID)", conn);
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

        #region --- Ace TPM To SAP Pro. Rej. Details ----
        internal static DataTable GetAceTPMToSAPProdRejDetails(string machineID, string fromDate, string toDate, string workOrderNo, string MessageType)
        {

            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from ACE_TPM2SAP_ProdAndRejDetails where (Machineid=@Machineid or isnull(@Machineid,'')='') and (WorkOrderNo like '%' + @WorkOrderNo + '%' or isnull(@WorkOrderNo,'')='') and BatchStart>=@ST and BatchStart<=@ET and (Status=@status or isnull(@status,'')='')", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machineID);
                cmd.Parameters.AddWithValue("ST", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ET", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@WorkOrderNo", workOrderNo);
                cmd.Parameters.AddWithValue("@status", MessageType.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : MessageType);
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAceTPMToSAPProdRejDetails: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        #endregion

        #region------ ImportSchedule Error Data -------------
        internal static List<ScheduleImportErrorEntity> getScheduleImportErrorMsgDetails(string fromDate, string toDate, string PO, string viewType)
        {
            List<ScheduleImportErrorEntity> list = new List<ScheduleImportErrorEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            ScheduleImportErrorEntity data = null;
            try
            {
                cmd = new SqlCommand(@"
IF (isnull(@ProductionOrder,'')<>'')
BEGIN
	select * from ACE_SAP_ImportScheduleErrorLog 
	where (ProductionOrder like '%' +@ProductionOrder+ '%' or isnull(@ProductionOrder,'')='')
END
ELSE
BEGIN
	select * from ACE_SAP_ImportScheduleErrorLog 
	where (UpdatedTS>=@ST and UpdatedTS<=@ET)
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                if (viewType.Equals("Date", StringComparison.OrdinalIgnoreCase))
                {
                    PO = "";
                }
                cmd.Parameters.AddWithValue("@ST", string.IsNullOrEmpty(fromDate) ? "" : Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ET", string.IsNullOrEmpty(toDate) ? "" : Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                //else
                //{
                cmd.Parameters.AddWithValue("@ProductionOrder", PO);
                //}
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new ScheduleImportErrorEntity();
                        data.MachineDesc = rdr["MachineDescription"].ToString();
                        data.ProductionOrder = rdr["ProductionOrder"].ToString();
                        data.CompID = rdr["ComponentID"].ToString();
                        data.OpnNo = rdr["OperationNo"].ToString();
                        data.ErroMsg = rdr["ErrorMessage"].ToString();
                        data.UpdatedTS = string.IsNullOrEmpty(rdr["UpdatedTS"].ToString()) ? "" : Util.GetDateTime(rdr["UpdatedTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
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
        #endregion

        #region ---- Event Log screen ------
        internal static List<string> GetMachineID()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct machineid from machineinformation", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
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
                Logger.WriteErrorLog("GetMachineID: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<EventLogEntity> GetEventLogsData(string FromDate, string ToDate, string MachineID, string EventID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<EventLogEntity> list = new List<EventLogEntity>();
            bool MachineColumnVisibility = MachineID == "" ? true : (MachineID.Split(',').ToList().Count > 1 ? true : false);
            try
            {
                cmd = new SqlCommand(@"SP_EventLogScreenMockUp_ACE", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@eventid", EventID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (!sdr.IsClosed)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(sdr);

                        if (dt.Rows.Count > 0)
                        {
                            EventLogEntity EventEntity = new EventLogEntity();
                            EventEntity.EventID = dt.Rows[0]["EventID"].ToString();
                            EventEntity.RowSpan = dt.Rows.Count.ToString();
                            EventLogEntity entity = new EventLogEntity();
                            entity.HeaderVisibility = true;
                            entity.MachineColumnVisibility = MachineColumnVisibility;

                            EventEntity.EventDetails.Add(entity);
                            foreach (DataRow row in dt.Rows)
                            {
                                entity = new EventLogEntity();
                                {
                                    entity.SlNo = row["SlNo"].ToString();
                                    entity.AlarmTime = Util.GetDateTime(row["AlarmTime"].ToString().Trim()).ToString("dd-MM-yyyy hh:mm:ss tt");
                                    entity.EnableDisable = row["EnableDisable"].ToString();
                                    entity.MachineID = row["MachineID"].ToString();
                                    entity.EventID = row["EventID"].ToString();
                                    entity.MachineColumnVisibility = MachineColumnVisibility;
                                    entity.ContentVisibility = true;
                                }
                                EventEntity.EventDetails.Add(entity);
                            }

                            list.Add(EventEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetEventLogsData: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static DataTable GetEventLogsDataReport(string FromDate, string ToDate, string MachineID, string EventID, out DataTable dt_2, out DataTable dt_3)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<EventLogEntity> list = new List<EventLogEntity>();
            bool MachineColumnVisibility = MachineID == "" ? true : (MachineID.Split(',').ToList().Count > 1 ? true : false);
            DataTable dt_1 = new DataTable();
            dt_2 = new DataTable();
            dt_3 = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_EventLogScreenMockUp_ACE", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@eventid", EventID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt_1.Load(sdr);
                    if (!sdr.IsClosed)
                        dt_2.Load(sdr);
                    if (!sdr.IsClosed)
                        dt_3.Load(sdr);
                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetEventLogsData: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt_1;
        }
        #endregion
    }
}