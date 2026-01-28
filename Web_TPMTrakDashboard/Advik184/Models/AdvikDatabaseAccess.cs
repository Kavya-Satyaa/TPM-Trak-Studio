using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik184.Models
{
    public class AdvikDatabaseAccess
    {
        internal static string getFinalInspectionMachineID()
        {

            string machineid = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select machineid from MachineInformation_Advik where OpnNo=100", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        machineid = rdr["machineid"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineByOperation = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return machineid;
        }
        internal static string getMachineByOperation(string operation)
        {
            string machineid = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select machineid from machineinformation where OpnNo=@OpnNo", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OpnNo", operation);
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        machineid = rdr["machineid"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineByOperation = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return machineid;
        }
        internal static List<string> getMachineFromTwoTable()
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"Select Machineid from
(Select machineid,SortOrder from machineinformation
Union
Select machineid,SortOrder from MachineInformation_Advik)T
Order by SortOrder", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr["Machineid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineFromTwoTable = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        //internal static string getMachineOperationNumber(string machine)
        //{
        //    string opnNo = "";
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    SqlCommand cmd = null;
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        cmd = new SqlCommand(@"select OpnNo from MachineInformation_Advik where MachineID=@MachineID", conn);
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Parameters.AddWithValue("@MachineID", machine);
        //        cmd.CommandTimeout = 500;
        //        rdr = cmd.ExecuteReader();
        //        if (rdr.HasRows)
        //        {
        //            while (rdr.Read())
        //            {
        //                opnNo = rdr["OpnNo"].ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("getMachineOperationNumber = " + ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        if (conn != null) conn.Close();
        //        if (rdr != null) rdr.Close();
        //    }
        //    return opnNo;
        //}

        #region ------------ Final Inspection Dashboard-----------
        internal static List<FinalInspectionEnity> getFinalInspectionList(string slno)
        {
            List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDashboardDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", slno);
                cmd.Parameters.AddWithValue("@Param", "FI_DashboardView");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FinalInspectionEnity data = new FinalInspectionEnity();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.BackColor = rdr["BGColor"].ToString();
                        if (string.IsNullOrEmpty(data.BackColor))
                        {
                            data.BackColor = "#bfbfbf";
                        }
                        data.ForeColor = "white";
                        if (data.BackColor.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            data.ForeColor = "black";
                        }
                        List<FinalInspectionEnity> statuslist = new List<FinalInspectionEnity>();
                        // statuslist.Add(new FinalInspectionEnity() { Label = "PN", Value = row["PartNumber"].ToString() });
                        //statuslist.Add(new FinalInspectionEnity() { Label = "SN", Value = row["SerialNumber"].ToString() });
                        if (data.MachineID.IndexOf("laser", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Mark Time", Value = rdr["CycleStart"].ToString() });
                        }
                        else if (data.MachineID.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Cycle Time", Value = rdr["CycleStart"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Actual Value", Value = rdr["ActValue"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Std. Value", Value = rdr["StdValue"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Result", Value = rdr["Result"].ToString() });
                        }
                        else if (data.MachineID.IndexOf("final", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Inspection Time", Value = rdr["CycleStart"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Result", Value = rdr["Result"].ToString() });
                        }
                        data.StatusList = statuslist;
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static string isFinalInspectionLastStationCompleted(string slno)
        {

            string previsousStationStatus = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetMachineWiseLastOperationStatus_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", slno);
                cmd.Parameters.AddWithValue("@Param", "ValidationForFI_PanthNagar");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    //1-> OK, 2->NG, 3->NO RESULT, 4->REWORK , 5->REJECTED
                    while (rdr.Read())
                    {
                        previsousStationStatus = rdr["Result"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return previsousStationStatus;
        }
        internal static DataTable getFinalInspectionParameterList(string slno, string machineID, out DataTable dt2)
        {

            DataTable dt = new DataTable();
            dt2 = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDashboardDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", slno);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", "FinalInspectionDetails");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt2.Load(rdr);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        internal static int saveFinalInspectionParameterDetails(ParameterMasterEntity data,string dateTime)
        {

            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if  exists(select * from ParameterTransactionDetails_Advik where MachineID=@MachineID and Barcode=@Barcode and ParameterID=@ParameterID and FI_Result=1)
begin
	update ParameterTransactionDetails_Advik set Value=@Value,CycleStart=@CycleStart,UpdatedTS=@UpdatedTS
	where MachineID=@MachineID and Barcode=@Barcode and ParameterID=@ParameterID  and FI_Result=1
	
end
else 
begin
	insert into ParameterTransactionDetails_Advik(MachineID,Barcode,ParameterID,ParameterName,LSL,USL,Unit,Value,UpdatedTS,CycleStart,FI_Result)
	values(@MachineID,@Barcode,@ParameterID,@ParameterName,@LSL,@USL,@Unit,@Value,@UpdatedTS,@CycleStart,1)
end
", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Barcode", data.BarCode);
                cmd.Parameters.AddWithValue("@ParameterID", data.ParameterID);
                cmd.Parameters.AddWithValue("@ParameterName", data.ParameterName);
                cmd.Parameters.AddWithValue("@LSL", data.LSL);
                cmd.Parameters.AddWithValue("@USL", data.USL);
                cmd.Parameters.AddWithValue("@Unit", data.Unit);
                cmd.Parameters.AddWithValue("@Value", data.Value);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@CycleStart", Util.GetDateTime(dateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 500;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        internal static int insertFinalInspectionStatusRemarksDetails(ParameterMasterEntity data, string remarks, string updatedBy)
        {

            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                string query = "";

                query = @" if not exists(select * from FinalInspectionResultDetails_Advik where MachineID=@MachineID and ComponentID=@ComponentID and Barcode=@Barcode  and Status is null)
begin
	insert into FinalInspectionResultDetails_Advik(MachineID, ComponentID, Barcode, Status, Remarks, UpdatedBy, UpdatedTS, Model)
	 values(@MachineID, @ComponentID, @Barcode, @Status, @Remarks, @UpdatedBy, @UpdatedTS, @Model) 
end
else
begin
	update FinalInspectionResultDetails_Advik set Remarks=@Remarks,UpdatedBy=@UpdatedBy,UpdatedTS=@UpdatedTS  where MachineID=@MachineID and ComponentID=@ComponentID and Barcode=@Barcode and Status is null
end ";

                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", data.ComponentID);
                cmd.Parameters.AddWithValue("@Barcode", data.BarCode);
                cmd.Parameters.AddWithValue("@Status", DBNull.Value);
                cmd.Parameters.AddWithValue("@Remarks", remarks);
                cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Model", data.Model);
                cmd.CommandTimeout = 500;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("saveFinalInspectionStatusDetails = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        internal static int saveFinalInspectionStatusDetails(ParameterMasterEntity data, string status, string updatedBy, string reworkMachine, string reworkOpnNo,string dateTime)
        {

            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                string query = "";
                string value = "1";
                if (status.Equals("Reject", StringComparison.OrdinalIgnoreCase))
                {
                    value = "5";
                    query = @" insert into RejectionDetails_Advik(MachineID,ComponentID,Barcode,OperationNo, UpdatedTS)
 values(@MachineID,@ComponentID,@Barcode,@OperationNo,@UpdatedTS);";
                }
                else if (status.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                {
                    value = "4";
                    query = @"insert into ReworkDetails_Advik(MachineID,ComponentID,Barcode,OperationNo,UpdatedTS)
 values(@ReworkMachineID,@ComponentID,@Barcode,@reworkOpnNo,@UpdatedTS); 

if exists (select * from MachineWiseLastOperationStatus_Advik where MachineID=@ReworkMachineID and ComponentID=@ComponentID
and OperationNo=@reworkOpnNo and Barcode=@Barcode)
begin
update MachineWiseLastOperationStatus_Advik set Result=4,UpdatedTS=@UpdatedTS where MachineID=@ReworkMachineID and ComponentID=@ComponentID
and OperationNo=@reworkOpnNo and Barcode=@Barcode;
end
else
begin
insert into MachineWiseLastOperationStatus_Advik(MachineID,ComponentID,OperationNo,Barcode,Result,UpdatedTS)
values(@ReworkMachineID,@ComponentID,@reworkOpnNo,@Barcode,4,@UpdatedTS)
end;";
                }

                query += @" update FinalInspectionResultDetails_Advik set Status=@Status,UpdatedTS=@UpdatedTS where MachineID=@MachineID and ComponentID=@ComponentID and Barcode=@Barcode and Status is null;";


                query += @"insert into ParameterTransactionCycleDetails_Advik(MachineID,Barcode,ComponentID,OperationNo,Operator,CycleStart,CycleEnd,Model,UpdatedTS)
values(@MachineID,@Barcode,@ComponentID,@OperationNo,@UpdatedBy,@CycleStartEnd,@CycleStartEnd,@Model,@UpdatedTS);";

                query += @"insert into ParameterTransactionDetails_Advik(MachineID,Barcode,ParameterID,IsChildPartOrResult,Value,CycleStart,UpdatedTS,FI_Result)
values(@MachineID,@Barcode,'Result',1,@Value,@CycleStartEnd,@UpdatedTS,1) ;";

                if (status.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                {
                    query += @"update ParameterTransactionDetails_Advik set FI_Result=4 
where  MachineID=@MachineID  and Barcode=@Barcode and CycleStart=@CycleStartEnd and FI_Result=1;";
                }

                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", data.ComponentID);
                cmd.Parameters.AddWithValue("@Barcode", data.BarCode);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                cmd.Parameters.AddWithValue("@OperationNo", data.OperationNo);
                cmd.Parameters.AddWithValue("@reworkOpnNo", reworkOpnNo);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@CycleStartEnd", Util.GetDateTime(dateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ReworkMachineID", reworkMachine);
                cmd.Parameters.AddWithValue("@Model", data.Model);
                cmd.Parameters.AddWithValue("@Value", value);
                cmd.CommandTimeout = 500;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("saveFinalInspectionStatusDetails = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        internal static string finalInspectionLatestStatus(string slno, string machineID)
        {

            string previsousStationStatus = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select A1.* from FinalInspectionResultDetails_Advik A1
	inner join (select B1.MachineID,B1.ComponentID,B1.Barcode,Max(B1.UpdatedTS) as UTS from FinalInspectionResultDetails_Advik B1
	where MachineID=@MachineID and Barcode=@Barcode 
	group by B1.MachineID,B1.ComponentID,B1.Barcode
	)A2 on A1.MachineID=A2.MachineID and A1.Barcode=A2.Barcode and A1.ComponentID=A2.ComponentID and A1.UpdatedTS=A2.UTS", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Barcode", slno);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        previsousStationStatus = rdr["Status"].ToString();

                        if (string.IsNullOrEmpty(previsousStationStatus))
                        {
                            previsousStationStatus = "null";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return previsousStationStatus;
        }
        #endregion

        #region -------- List Status Andon -------
        internal static List<FinalInspectionEnity> getPlantStatusAndon(string plant)
        {
            List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetDashboardDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@Param", "DashboardView");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FinalInspectionEnity data = new FinalInspectionEnity();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.BackColor = rdr["BGColor"].ToString();
                        if (string.IsNullOrEmpty(data.BackColor))
                        {
                            data.BackColor = "#bfbfbf";
                        }
                        data.ForeColor = "white";
                        if (data.BackColor.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            data.ForeColor = "black";
                        }
                        List<FinalInspectionEnity> statuslist = new List<FinalInspectionEnity>();
                        statuslist.Add(new FinalInspectionEnity() { Label = "QR Code", Value = rdr["Barcode"].ToString() });
                        statuslist.Add(new FinalInspectionEnity() { Label = "Model", Value = rdr["Model"].ToString() });
                        if (data.MachineID.IndexOf("laser", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Mark Time", Value = rdr["CycleStart"].ToString() });
                        }
                        else if (data.MachineID.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Cycle Start", Value = rdr["CycleStart"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Cycle End", Value = rdr["CycleEnd"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Values", Value = rdr["ActValue"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Result", Value = rdr["Result"].ToString() });
                        }
                        else if (data.MachineID.IndexOf("final", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Inspection Time", Value = rdr["CycleStart"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Result", Value = rdr["Result"].ToString() });
                        }
                        data.StatusList = statuslist;
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        #endregion

        #region ------ Final Inspection Report -----
        internal static List<FinalInspectionEnity> getFinalInspectionReportSlnoList(string line, string status, string slno, string fromDate, string toDate, string viewType)
        {
            List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFinalInspectionDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", line.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : line);
                cmd.Parameters.AddWithValue("@Status", status.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : status);
                if (viewType.Equals("Date", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Barcode", slno);
                }
                cmd.Parameters.AddWithValue("@Param", "SummaryView");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FinalInspectionEnity data = new FinalInspectionEnity();
                        data.PlantID = rdr["PlantID"].ToString();
                        data.PartNumber = rdr["ComponentID"].ToString();
                        data.SerialNumber = rdr["Barcode"].ToString();
                        data.Date = rdr["Date"].ToString();
                        data.Status = rdr["Status"].ToString();
                        data.Remarks = rdr["Remarks"].ToString();
                        data.Model = rdr["Model"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionReportSlnoList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<FinalInspectionEnity> getFinalInspectionReportMachineStatus(string partnumber, string slno, string plant)
        {
            List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFinalInspectionDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ComponentID", partnumber);
                cmd.Parameters.AddWithValue("@Barcode", slno);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@Param", "OperationWiseStatus");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FinalInspectionEnity data = new FinalInspectionEnity();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.BackColor = rdr["BGColor"].ToString();
                        if (string.IsNullOrEmpty(data.BackColor))
                        {
                            data.BackColor = "#bfbfbf";
                        }
                        data.ForeColor = "white";
                        if (data.BackColor.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            data.ForeColor = "black";
                        }
                        List<FinalInspectionEnity> statuslist = new List<FinalInspectionEnity>();
                        // statuslist.Add(new FinalInspectionEnity() { Label = "PN", Value = row["PartNumber"].ToString() });
                        //statuslist.Add(new FinalInspectionEnity() { Label = "SN", Value = row["SerialNumber"].ToString() });
                        if (data.MachineID.IndexOf("laser", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Mark Time", Value = rdr["CycleStart"].ToString() });
                        }
                        else if (data.MachineID.IndexOf("rpm", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Cycle Time", Value = rdr["CycleStart"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Actual Value", Value = rdr["ActValue"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Std. Value", Value = rdr["StdValue"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Result", Value = rdr["Result"].ToString() });
                        }
                        else if (data.MachineID.IndexOf("final", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            statuslist.Add(new FinalInspectionEnity() { Label = "Inspection Time", Value = rdr["CycleStart"].ToString() });
                            statuslist.Add(new FinalInspectionEnity() { Label = "Result", Value = rdr["Result"].ToString() });
                        }
                        data.StatusList = statuslist;
                        list.Add(data);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionReportMachineStatus = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static DataTable getFinalInspectionReportParameterList(string partnumber, string slno, string plant, string machine, out DataTable dt2)
        {

            DataTable dt = new DataTable();
            dt2 = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetFinalInspectionDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", slno);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Param", "FinalInspectionDetails");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt2.Load(rdr);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getFinalInspectionList = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        #endregion

        #region -------- Traceability Report ------
        internal static DataTable getTraceabilityReportDetails(string plant, string machine, string barCode, string fromDate, string toDate, string viewType)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetTraceabilityDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                if (viewType.Equals("Date", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Barcode", barCode);
                }
                cmd.Parameters.AddWithValue("@Param", "DetailView");
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getTraceabilityReportDetails = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        #endregion

        #region ----------- Parameter Master ------
        internal static List<ParameterMasterEntity> getParameterMasterData(string machineID, string compID, string opnID)
        {
            List<ParameterMasterEntity> list = new List<ParameterMasterEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from ParameterMasterDetails_Advik where MachineID=@MachineID and  ComponentID =@ComponentID and OperationNo =@OperationNo", conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ComponentID", compID);
                cmd.Parameters.AddWithValue("@OperationNo", opnID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ParameterMasterEntity data = new ParameterMasterEntity();
                        data.ID = rdr["IDD"].ToString();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.ComponentID = rdr["ComponentID"].ToString();
                        data.OperationNo = rdr["OperationNo"].ToString();
                        data.ParameterID = rdr["ParameterID"].ToString();
                        data.ParameterName = rdr["ParameterName"].ToString();
                        data.LSL = rdr["LSL"].ToString();
                        data.USL = rdr["USL"].ToString();
                        data.Unit = rdr["Unit"].ToString();
                        data.RegisterAddress = rdr["RegisterAddress"].ToString();
                        data.MinRegAdd = rdr["MinValueRegisterAddress"].ToString();
                        data.MaxRegAdd = rdr["MaxValueRegisterAddress"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getParameterMasterData = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static string saveParameterMasterData(ParameterMasterEntity data)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"if @OperationNo<>'100'
begin
	if exists(select * from ParameterMasterDetails_Advik where RegisterAddress=@RegisterAddress and IDD<>@IDD )
	begin
		select 'RegisterAddress Exists' as SaveFlag
		return
	end
end

if exists(select * from ParameterMasterDetails_Advik where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and ParameterID=@ParameterID)
begin
	update ParameterMasterDetails_Advik set ParameterName=@ParameterName,LSL=@LSL,USL=@USL,Unit=@Unit,RegisterAddress=@RegisterAddress,
	MinValueRegisterAddress=@MinValueRegisterAddress,MaxValueRegisterAddress=@MaxValueRegisterAddress
		where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and ParameterID=@ParameterID
    select 'Save' as SaveFlag
end
else
begin
	insert into ParameterMasterDetails_Advik(MachineID,ComponentID,OperationNo,ParameterID,ParameterName,LSL,USL,Unit,RegisterAddress,IsChildPartOrResult,MinValueRegisterAddress,MaxValueRegisterAddress)
	values(@MachineID,@ComponentID,@OperationNo,@ParameterID,@ParameterName,@LSL,@USL,@Unit,@RegisterAddress,@IsChildPartOrResult,@MinValueRegisterAddress,@MaxValueRegisterAddress)
    select 'Save' as SaveFlag
end
", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IDD", data.ID);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", data.ComponentID);
                cmd.Parameters.AddWithValue("@OperationNo", data.OperationNo);
                cmd.Parameters.AddWithValue("@ParameterID", data.ParameterID);
                cmd.Parameters.AddWithValue("@ParameterName", data.ParameterName);
                cmd.Parameters.AddWithValue("@LSL", data.LSL);
                cmd.Parameters.AddWithValue("@USL", data.USL);
                cmd.Parameters.AddWithValue("@Unit", data.Unit);
                cmd.Parameters.AddWithValue("@RegisterAddress", data.RegisterAddress);
                cmd.Parameters.AddWithValue("@MinValueRegisterAddress", data.MinRegAdd);
                cmd.Parameters.AddWithValue("@MaxValueRegisterAddress", data.MaxRegAdd);
                if (data.ParameterID.Equals("RPM", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@IsChildPartOrResult", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IsChildPartOrResult", 0);
                }
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
                Logger.WriteErrorLog("saveParameterMasterData = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static int deleteParameterMasterData(string idd)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("delete from ParameterMasterDetails_Advik where IDD=@IDD", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IDD", idd);
                result = cmd.ExecuteNonQuery();

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
            return result;
        }
        #endregion

        #region ----------- Model Master ------
        internal static List<ModelMasterEntity> getModelMasterData()
        {
            List<ModelMasterEntity> list = new List<ModelMasterEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from ParameterMasterDetails_Advik", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ModelMasterEntity data = new ModelMasterEntity();
                        data.ID = rdr["IDD"].ToString();
                        data.ModelID = rdr["IDD"].ToString();
                        data.ModelName = rdr["MachineID"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getModelMasterData = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static string saveModelMasterData(ModelMasterEntity data)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"	if exists(select * from ParameterMasterDetails_Advik where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and ParameterID=@ParameterID)
	begin
		update ParameterMasterDetails_Advik set ParameterName=@ParameterName
		 where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and ParameterID=@ParameterID
        select 'Save' as SaveFlag
	end
	else
	begin
		insert into ParameterMasterDetails_Advik(MachineID,ComponentID,OperationNo,ParameterID,ParameterName)
		values(@MachineID,@ComponentID,@OperationNo,@ParameterID,@ParameterName)
        select 'Save' as SaveFlag
	end
", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IDD", data.ID);
                cmd.Parameters.AddWithValue("@MachineID", data.ModelID);
                cmd.Parameters.AddWithValue("@ComponentID", data.ModelName);
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
                Logger.WriteErrorLog("saveModelMasterData = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static int deleteModelMasterData(string idd)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("delete from ParameterMasterDetails_Advik where IDD=@IDD", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IDD", idd);
                result = cmd.ExecuteNonQuery();

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
            return result;
        }
        #endregion

        #region -------- Setting ---------
        internal static DataTable getAdvikSettingDetails()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from ShopDefaults where Parameter='Advik184'", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getAdvikSettingDetails = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        internal static int saveShopDefaultValueInIntByText(string parameter, string valueInIext, string valueInInt)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if exists(select * from ShopDefaults where Parameter=@Parameter and ValueInText=@ValueInText)
begin
	update ShopDefaults set ValueInInt=@ValueInInt where Parameter=@Parameter and ValueInText=@ValueInText
end
else
begin
	insert into ShopDefaults(Parameter,ValueInText,ValueInInt)
	values(@Parameter,@ValueInText,@ValueInInt)
end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@ValueInText", valueInIext);
                cmd.Parameters.AddWithValue("@ValueInInt", valueInInt);
                cmd.CommandTimeout = 500;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("saveShopDefaultValueInIntByText = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        internal static int saveShopDefaultValueInText2ByText(string parameter, string valueInIext, string valueInText2)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if exists(select * from ShopDefaults where Parameter=@Parameter and ValueInText=@ValueInText)
begin
	update ShopDefaults set ValueInText2=@ValueInText2 where Parameter=@Parameter and ValueInText=@ValueInText
end
else
begin
	insert into ShopDefaults(Parameter,ValueInText,ValueInText2)
	values(@Parameter,@ValueInText,@ValueInText2)
end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@ValueInText", valueInIext);
                cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
                cmd.CommandTimeout = 500;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("saveShopDefaultValueInText2ByText = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        #endregion

        #region ----------- Pokayoke Master ------
        internal static List<PokayokeMasterEntity> getPokayokeMasterData(string machineID, string compID, string opnID)
        {
            List<PokayokeMasterEntity> list = new List<PokayokeMasterEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                //cmd = new SqlCommand(@"select * from ParameterMasterDetails_Advik where MachineID=@MachineID and  ComponentID =@ComponentID and OperationNo =@OperationNo", conn);
                cmd = new SqlCommand(@"select * from PokayokeMasterDetails_Advik where MachineID=@MachineID", conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ComponentID", compID);
                cmd.Parameters.AddWithValue("@OperationNo", opnID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        PokayokeMasterEntity data = new PokayokeMasterEntity();
                        data.ID = rdr["IDD"].ToString();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.ComponentID = rdr["ComponentID"].ToString();
                        data.OperationNo = rdr["OperationNo"].ToString();
                        data.PokayokeID = rdr["PokayokeID"].ToString();
                        data.PokayokeName = rdr["PokayokeName"].ToString();
                        data.RegisterID = rdr["RegisterID"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPokayokeMasterData = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static string savePokayokeMasterData(PokayokeMasterEntity data)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"if exists(select * from PokayokeMasterDetails_Advik where RegisterID=@RegisterID and IDD<>@IDD )
begin
	select 'RegisterID Exists' as SaveFlag
end
else 
begin
	if exists(select * from PokayokeMasterDetails_Advik where MachineID=@MachineID and PokayokeID=@PokayokeID)
	begin
		update PokayokeMasterDetails_Advik set PokayokeName=@PokayokeName,RegisterID=@RegisterID
		where MachineID=@MachineID and PokayokeID=@PokayokeID
        select 'Save' as SaveFlag
	end
	else
	begin
		insert into PokayokeMasterDetails_Advik(MachineID,PokayokeID,PokayokeName,RegisterID)
		values(@MachineID,@PokayokeID,@PokayokeName,@RegisterID)
        select 'Save' as SaveFlag
	end
end
", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IDD", data.ID);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                //cmd.Parameters.AddWithValue("@ComponentID", data.ComponentID);
                // cmd.Parameters.AddWithValue("@OperationNo", data.OperationNo);
                cmd.Parameters.AddWithValue("@PokayokeID", data.PokayokeID);
                cmd.Parameters.AddWithValue("@PokayokeName", data.PokayokeName);
                cmd.Parameters.AddWithValue("@RegisterID", data.RegisterID);
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
                Logger.WriteErrorLog("savePokayokeMasterData = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static int deletePokayokeMasterData(string idd)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("delete from PokayokeMasterDetails_Advik where IDD=@IDD", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IDD", idd);
                result = cmd.ExecuteNonQuery();

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
            return result;
        }
        #endregion

        #region ------ JH Checjlist master --------
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
                        masterData.ChecklistItem = reader["ChecklistItem"].ToString();
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
          insert into JHCheckListMaster_Advik(MachineID,ChecklistID,ChecklistItem,IsEnabled,SortOrder,JHType,McArea,Location,StdCondition,CheckingMethod) 
		  values (@machineId,@checklistId,@ChecklistItem,@isEnabled,@sortOrder,@jhType,@mcarea,@location,@stdcondition,@checkingmethod)
     end
else
    begin
           update JHCheckListMaster_Advik set  McArea=@mcarea,Location=@location,StdCondition=@stdcondition,CheckingMethod=@checkingmethod, IsEnabled=@isEnabled, SortOrder=@sortOrder,ChecklistItem=@ChecklistItem where MachineID=@machineId and ChecklistID=@checklistId and JHType=@jhType
     end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@machineId", machineId);
                cmd.Parameters.AddWithValue("@checklistId", data.ChecklistID);
                cmd.Parameters.AddWithValue("@ChecklistItem", data.ChecklistItem);
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
                sqlBulkCopy.ColumnMappings.Add("ChecklistItem", "ChecklistItem");
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
        #endregion

        #region --------- PokayOke Transaction --------
        internal static List<PokayokeMasterEntity> getPoakayOkeDashoboardData(string machine, string fromDateTime,string toDateTime)
        {
            List<PokayokeMasterEntity> list = new List<PokayokeMasterEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            string query = @"select MachineID,ComponentID,OperationNo,PokayokeID,PokayokeName,RegisterID,
(case when Result='1' Then 'NOT DONE' when Result='2' then 'DONE' Else '' End) as Result,UpdatedTS
from PokayokeTransactionDetails_Advik where MachineID in (" + machine + ") and UpdatedTS>=@fromDateTime and UpdatedTS<=@toDateTime";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@machineId", machine);
                cmd.Parameters.AddWithValue("@fromDateTime", Util.GetDateTime(fromDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@toDateTime", Util.GetDateTime(toDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PokayokeMasterEntity data = new PokayokeMasterEntity();
                        data.MachineID = reader["MachineID"].ToString();
                        data.PokayokeID = reader["PokayokeID"].ToString();
                        data.PokayokeName = reader["PokayokeName"].ToString();
                        data.RegisterID = reader["RegisterID"].ToString();
                        data.Result = reader["Result"].ToString();
                        data.UpdatedTS = reader["UpdatedTS"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPoakayOkeDashoboardData = " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        #endregion

    }
}