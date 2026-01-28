using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.VulkanMachineShop.Model
{
    public class VulkanMSDBAccess
    {
        #region --------- Cell Employee Association -------

        internal static DataTable getCellEmployeeAssociationDetails(string Plant)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[AssignPlantCellEmployee_kta]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getCellEmployeeAssociationDetails = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        internal static int insertDeleteEmployeeCellAssociation(string Plant, string cell, string employeeID, string param)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                string query = "";
                if (param.Equals("insert", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"if not exists (select * from PlantCellEmployeeAssociation_KTA where PlantID=@PlantID and GroupID=@GroupID and EmployeeID=@EmployeeID)
begin
	insert into PlantCellEmployeeAssociation_KTA(PlantID,GroupID,EmployeeID,UpdatedTS) values(@PlantID,@GroupID,@EmployeeID,@UpdatedTS)
end";
                }
                else if (param.Equals("delete", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"delete from PlantCellEmployeeAssociation_KTA where PlantID=@PlantID and GroupID=@GroupID and EmployeeID=@EmployeeID";
                }
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                cmd.Parameters.AddWithValue("@GroupID", cell);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertDeleteEmployeeCellAssociation = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return result;
        }
        #endregion

        #region --------- Schedule Master -------
        internal static List<string> getCellAssignedForEmployee(string Plant, string employeeID)
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct GroupID from PlantCellEmployeeAssociation_KTA where (PlantID = @PlantID or isnull(@PlantID,'')='') and EmployeeID=@EmployeeID", conn);
                cmd.Parameters.AddWithValue("@PlantID", Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr["GroupID"].ToString());
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
            return list;
        }
        internal static List<ListItem> getMachineIdInterfaceIDByPlantCell(string Plant, string cell, bool machineInterfaceIDRequired)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct m.machineid,m.InterfaceID  from PlantMachineGroups pmg 
inner join  machineinformation m on pmg.MachineID=m.machineid 
where (pmg.PlantID = @PlantID or isnull(@PlantID,'')='') and pmg.GroupID in (" + cell + ")", conn);
                cmd.Parameters.AddWithValue("@PlantID", Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ListItem data = new ListItem();
                        data.Text = rdr["machineid"].ToString();
                        if (machineInterfaceIDRequired)
                        {
                            data.Value = rdr["InterfaceID"].ToString();
                        }
                        else
                        {
                            data.Value = rdr["machineid"].ToString();
                        }
                        list.Add(data);
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
            return list;
        }
        internal static List<ListItem> getComponentIDWithInterface(string machineID)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct c.componentid,c.InterfaceID from componentinformation c inner join componentoperationpricing cop on c.componentid=cop.componentid where  cop.machineid=@MachineID", conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ListItem data = new ListItem();
                        data.Text = rdr["componentid"].ToString();
                        data.Value = rdr["InterfaceID"].ToString();
                        list.Add(data);
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
            return list;
        }
        internal static List<ListItem> getOperationWithInterface(string machineID, string compID)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct operationno,InterfaceID from componentoperationpricing where machineid=@machineid and componentid=@componentid", conn);
                cmd.Parameters.AddWithValue("@machineid", machineID);
                cmd.Parameters.AddWithValue("@componentid", compID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ListItem data = new ListItem();
                        data.Text = rdr["operationno"].ToString();
                        data.Value = rdr["InterfaceID"].ToString();
                        list.Add(data);
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
            return list;
        }
        internal static List<ScheduleMasterEntity> getScheduleMasterDetails(string plant, string cell, string machine, string compIDSearch1, string compIDSearch2, string compDescSearch1, string compaDescSearch2, string status, string fromDate, string toDate)
        {
            List<ScheduleMasterEntity> list = new List<ScheduleMasterEntity>();
            ScheduleMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", string.IsNullOrEmpty(fromDate) ? "" : Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", string.IsNullOrEmpty(toDate) ? "" : Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@Groupid", cell);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@Comp1", compIDSearch1);
                cmd.Parameters.AddWithValue("@Comp2", compIDSearch2);
                cmd.Parameters.AddWithValue("@Des1", compDescSearch1);
                cmd.Parameters.AddWithValue("@Des2", compaDescSearch2);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@param", "View");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {

                    while (rdr.Read())
                    {
                        data = new ScheduleMasterEntity();
                        data.IDD = rdr["AutoID"].ToString();
                        data.ScheduleDateTime = string.IsNullOrEmpty(rdr["ScheduleDate"].ToString()) ? "" : Util.GetDateTime(rdr["ScheduleDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        data.ScheduleDate = string.IsNullOrEmpty(rdr["ScheduleDate"].ToString()) ? "" : Util.GetDateTime(rdr["ScheduleDate"].ToString()).ToString("dd-MM-yyyy");
                        data.Plant = rdr["PlantID"].ToString();
                        data.Cell = rdr["GroupID"].ToString();
                        data.Machine = rdr["MachineID"].ToString();
                        data.MachineInterfaceID = rdr["MCinterface"].ToString();
                        data.Component = rdr["ComponentID"].ToString();
                        data.Operation = rdr["OperationNo"].ToString();
                        data.CompInterfaceId = rdr["CompInterface"].ToString();
                        data.OpnInterfaceId = rdr["OpnInterface"].ToString();
                        data.CompDesc = rdr["CompDescription"].ToString();
                        data.Status = rdr["Status"].ToString();
                        if (data.Status == "1")
                        {
                            data.Status = "Closed";
                            data.PriorityReadOnly = true;
                        }
                        else if (data.Status == "2")
                        {
                            data.Status = "New";
                        }
                        else if (data.Status == "3")
                        {
                            data.Status = "Running";
                            data.PriorityReadOnly = true;
                        }
                        data.Priorityno = rdr["PriorityNo"].ToString();
                        data.WorkOrder = rdr["WorkOrderNo"].ToString();
                        data.WorkOrderQty = rdr["ScheduleQty"].ToString();
                        list.Add(data);
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
            return list;
        }
        internal static string saveScheduleMasterDetails(ScheduleMasterEntity data)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@McInterface", data.MachineInterfaceID);
                cmd.Parameters.AddWithValue("@CompInterface", data.CompInterfaceId);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OprInterface", data.OpnInterfaceId);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@UpdatedBY", data.UpdatedBy);
                cmd.Parameters.AddWithValue("@Priority", data.Priorityno);
                cmd.Parameters.AddWithValue("@WorkOrderNo", data.WorkOrder == "" ? "WR-13EDR" : data.WorkOrder);
                cmd.Parameters.AddWithValue("@ScheduleQty", data.WorkOrderQty == "" ? "1" : data.WorkOrderQty);
                cmd.Parameters.AddWithValue("@param", "Save");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = rdr["Result"].ToString();
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
            return result;
        }
        internal static int updateScheduleMasterDetails(ScheduleMasterEntity data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                cmd.Parameters.AddWithValue("@WorkOrderNo", data.WorkOrder);
                if (data.Status == "Closed")
                {
                    data.Status = "1";
                }
                else if (data.Status == "New")
                {
                    data.Status = "2";
                }
                else if (data.Status == "Running")
                {
                    data.Status = "3";
                }
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@param", "Update");
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
        internal static int deleteScheduleMasterDetails(ScheduleMasterEntity data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                cmd.Parameters.AddWithValue("@WorkOrderNo", data.WorkOrder);
                if (data.Status == "Closed")
                {
                    data.Status = "1";
                }
                else if (data.Status == "New")
                {
                    data.Status = "2";
                }
                else if (data.Status == "Running")
                {
                    data.Status = "3";
                }
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@param", "Delete");
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

        internal static string UpdateStoreSchedulePriorityKTA(ScheduleMasterEntity data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string result = "";
            string query = @"if EXISTS(SELECT * FROM scheduleDetails_KTA WHERE PlantID=@PlantID AND GroupID=@Groupid and MachineID=@Machineid AND PriorityNo=@Priority and Status='2')
BEGIN
	SELECT 'Priority No Exists!' as SaveFlag
END
ELSE
BEGIN
	if EXISTS (SELECT * FROM scheduleDetails_KTA WHERE CAST(ScheduleDate AS DATE)=CAST(@Scheduledate AS DATE) AND MachineID=@Machineid AND ComponentID=@ComponentID AND 
	OperationNo=@OperatioNo  AND PlantID=@PlantID AND GroupID=@Groupid)
	BEGIN
		UPDATE scheduleDetails_KTA SET PriorityNo=@Priority,UpdatedTS=@UpdatedTS  WHERE CAST(ScheduleDate AS DATE)=CAST(@Scheduledate AS DATE) AND MachineID=@Machineid AND ComponentID=@ComponentID AND 
		OperationNo=@OperatioNo  AND PlantID=@PlantID AND GroupID=@Groupid
		SELECT 'Updated' as SaveFlag
	END
END";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Priority", data.Priorityno);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                result = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        #endregion

        internal static List<string> GetShiftDetails()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct shiftid, ShiftName from shiftdetails", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["ShiftName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetShiftDetails: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
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
                Logger.WriteErrorLog($"GetMachineID: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        #region --- Inspection Transaction ---
        internal static List<InspectionTransactionEntity> GetInspectionTransactionDetails(string MachineID, string Date, string Shift, out string result)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<InspectionTransactionEntity> list = new List<InspectionTransactionEntity>();
            List<InspectionTransactionEntity> list_heatCodes = new List<InspectionTransactionEntity>();
            InspectionTransactionEntity entity = new InspectionTransactionEntity();
            DataTable dt = new DataTable();
            DataTable dt_Operator = new DataTable();
            DataTable dt_ApprovedData = new DataTable();
            result = "";
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@Param1", "Approve");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt_Operator.Load(sdr);
                    dt_ApprovedData.Load(sdr);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Columns.IndexOf("SaveFlag") != -1)
                        {
                            result = dt.Rows[0]["SaveFlag"].ToString();
                            return list;
                        }
                        #region -- Header Details --
                        entity = new InspectionTransactionEntity();
                        entity.HeaderVisibility = true;
                        list_heatCodes = new List<InspectionTransactionEntity>();
                        for (int i = 12; i < dt.Columns.Count; i++)
                        {
                            InspectionTransactionEntity li = new InspectionTransactionEntity();
                            li.HeatCodeValue = dt.Columns[i].ColumnName.ToString();
                            li.HeaderVisibility = true;
                            list_heatCodes.Add(li);
                        }
                        entity.listofHeatCode = list_heatCodes;
                        list.Add(entity);
                        #endregion

                        #region -- Approve Details --
                        foreach (DataRow row in dt.Rows)
                        {
                            entity = new InspectionTransactionEntity();
                            entity.Date = Util.GetDateTime(row["Date"].ToString()).ToString("dd-MM-yyyy");
                            entity.Shift = row["Shift"].ToString();
                            entity.ComponentID = row["ComponentID"].ToString();
                            entity.OperationNo = row["OperationNo"].ToString();
                            entity.CharacteristicID = row["CharacteristicID"].ToString();
                            entity.CharacteristicCode = row["CharacteristicCode"].ToString();
                            entity.LSL = row["LSL"].ToString();
                            entity.USL = row["USL"].ToString();
                            entity.SetValue = row["SetValue"].ToString();
                            entity.ContentVisibility = true;
                            list_heatCodes = new List<InspectionTransactionEntity>();
                            for (int i = 12; i < dt.Columns.Count; i++)
                            {
                                InspectionTransactionEntity li = new InspectionTransactionEntity();
                                li.HeatCodeValue = row[i].ToString();
                                li.ContentVisibility = true;
                                list_heatCodes.Add(li);
                            }
                            entity.listofHeatCode = list_heatCodes;
                            list.Add(entity);

                        }
                        #endregion

                        #region -- Opertaor Row --
                        entity = new InspectionTransactionEntity();
                        entity.OperatorRowVisibility = true;
                        entity.CharacteristicID = "Operator Sign:";
                        list_heatCodes = new List<InspectionTransactionEntity>();
                        for (int i = 3; i < dt_Operator.Columns.Count; i++)
                        {
                            InspectionTransactionEntity li = new InspectionTransactionEntity();
                            li.HeatCodeValue = dt_Operator.Rows[0][i].ToString();
                            list_heatCodes.Add(li);
                        }
                        entity.listofHeatCode = list_heatCodes;
                        list.Add(entity);
                        #endregion

                        #region -- Footer --
                        entity = new InspectionTransactionEntity();
                        entity.FooterVisisbility = true;
                        if (HttpContext.Current.Session["UserRole"] != null)
                        {
                            var role = HttpContext.Current.Session["UserRole"].ToString();
                            if (dt_ApprovedData.Rows.Count == 0 && HttpContext.Current.Session["UserRole"].ToString().Equals("Supervisor", StringComparison.OrdinalIgnoreCase))
                                entity.ButtonVisibility = true;
                            else
                                if (dt_ApprovedData.Rows.Count > 0)
                                entity.ComponentID = "Approved By: " + dt_ApprovedData.Rows[0]["ApprovedBy"].ToString();
                        }
                        entity.ColSpanApproval = dt_Operator.Columns.Count;
                        list.Add(entity);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetInspectionTransactionDetails: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static string SaveApprovalDetails(string MachineID, string Date, string Shift)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string result = "";
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@SupervisorID", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@SupervisorTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "ApproveSave");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                        result = sdr["SaveFlag"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SaveApprovalDetails: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return result;
        }

        internal static DataTable GetInspectionReportData(string StartDate, string Shift, string MachineID, out DataTable dt_Operator, out DataTable dt_ApprovedData, out DataTable dt_HeatCodes)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt_InspectionData = new DataTable();
            dt_ApprovedData = new DataTable();
            dt_Operator = new DataTable();
            dt_HeatCodes = new DataTable();
            try
            {
                cmd = new SqlCommand("S_GetInspectionDetails_Vulkan", conn);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                //cmd.Parameters.AddWithValue("@StartDate", "2023-07-29");
                //cmd.Parameters.AddWithValue("@EndDate", "2023-08-02");
                //cmd.Parameters.AddWithValue("@MachineID", "MC-2,MC-1");
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt_InspectionData.Load(sdr);
                    if (dt_InspectionData.Columns.IndexOf("SaveFlag") == -1)
                    {
                        dt_Operator.Load(sdr);
                        dt_ApprovedData.Load(sdr);
                        dt_HeatCodes.Load(sdr);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetInspectionReportData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt_InspectionData;
        }

        internal static DataTable GetProductionReportDataHourWise(string StartDate, string ShiftID, string MachineID, out DataTable dt_Sign)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dtProduction = new DataTable();
            dt_Sign = new DataTable();
            try
            {
                cmd = new SqlCommand(@"s_Get_HourwiseProdAndDownDetails_Vulkan", conn);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Enddate", Util.GetDateTime(StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SHIFTNAME", ShiftID);
                cmd.Parameters.AddWithValue("@Machineid", MachineID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dtProduction.Load(sdr);
                    if (dtProduction.Columns.IndexOf("SaveFlag") == -1)
                        dt_Sign.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetProductionReportDataHourWise: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dtProduction;
        }

        internal static DataTable GetHourWiseProductionReportOEE(string StartDate, string ShiftID, string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = @"s_GetCockpitData_WithTempTable_eshopx";
            DataTable dt_Oee = new DataTable();
            DateTime ShiftStartTime = DateTime.MinValue, ShiftEndTime= DateTime.MaxValue;
            try
            {
                ShiftStartTime = GetShiftStartEndTimes(StartDate, ShiftID, out ShiftEndTime);
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartTime", ShiftStartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", ShiftEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@SortOrder", "MachineID");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt_Oee.Load(sdr);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog($"GetHourWiseProductionReportOEE: {ex.ToString()}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt_Oee;
        }

        internal static DateTime GetShiftStartEndTimes(string StartDate, string ShiftID, out DateTime ShiftEndTime)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DateTime ShiftStartTime = DateTime.MinValue;
            ShiftEndTime = DateTime.MaxValue;
            int addDays = 0;
            try
            {
                cmd = new SqlCommand(@"select top 1  * from shiftdetails where ShiftName=@ShiftName AND Running=1", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ShiftName", ShiftID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if(Convert.ToDateTime(sdr["FromTime"].ToString()).TimeOfDay > Convert.ToDateTime(sdr["ToTime"].ToString()).TimeOfDay)
                            addDays = 1;

                        var xx = sdr["FromTime"].ToString();
                        var yy = sdr["ToTime"].ToString();
                        ShiftStartTime = Util.GetDateTime(StartDate).Date.Add(Util.GetDateTime(sdr["FromTime"].ToString()).TimeOfDay);
                        ShiftEndTime = Util.GetDateTime(StartDate).Date.AddDays(addDays).Add(Util.GetDateTime(sdr["ToTime"].ToString()).TimeOfDay);
                    }
                }


            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog($"GetShiftStartEndTimes: {ex.ToString()}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return ShiftStartTime;
        }
        #endregion

        #region --- PM Checklist ---

        #region -- Master --
        internal static List<string> GetRevisionID(string MachineID, string Frequency)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@" select distinct RevID from PM_Master_MachineShop_Vulkan where MachineID=@MachineID and Frequency=@Frequency OR ISNULL(@Frequency,'')=''", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["RevID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetRevisionID: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        internal static string GetPMDocDetails(string MachineID, string Frequency, out string RevDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string DocNO = "";
            RevDate = "";
            try
            {
                cmd = new SqlCommand(@" select Top 1 * from PM_Master_MachineShop_Vulkan where MachineID=@MachineID and Frequency=@Frequency OR ISNULL(@Frequency,'')='' order by UpdatedTS DESC", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        DocNO = sdr["DocNo"].ToString();
                        RevDate = string.IsNullOrEmpty(sdr["RevDate"].ToString()) ? "" : Util.GetDateTime(sdr["RevDate"].ToString()).ToString("dd-MM-yyyy");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetPMDocDetails: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return DocNO;
        }

        internal static List<string> GetResponsibilityValues()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct EmployeeRole from employeeinformation where (EmployeeRole<>'' AND EmployeeRole is not null)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["EmployeeRole"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetResponsibilityValues: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        internal static string GetHeaderDataPM(string MachineID, out string ApprovedBy, out string VerifiedBy)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<PMMasterEntity> list = new List<PMMasterEntity>();
            string PreparedBy = string.Empty;
            ApprovedBy = VerifiedBy = string.Empty;
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "HeaderView");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        PreparedBy = sdr["PreparedBy"].ToString();
                        ApprovedBy = sdr["ApprovedBy"].ToString();
                        VerifiedBy = sdr["VerifiedBy"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetPMMasterData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return PreparedBy;
        }

        internal static List<PMMasterEntity> GetPMMasterData(string MachineID, string RevID, string Frequency)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<PMMasterEntity> list = new List<PMMasterEntity>();
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        PMMasterEntity entity = new PMMasterEntity();
                        entity.CheckpointID = sdr["CheckpointID"].ToString();
                        entity.CheckpointDesc = sdr["CheckpointDesc"].ToString();
                        entity.Particular = sdr["Particular"].ToString();
                        entity.Frequency = sdr["Frequency"].ToString();
                        entity.Responsibility = sdr["Responsibility"].ToString();
                        entity.ControlMethod = sdr["ControlMethod"].ToString();

                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetPMMasterData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static int SavePMHeaderDetails(string MachineID, string RevID, string PreparedBy, string ApprovedBy, string VerifiedBy)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int res = 0;
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@PreparedBy", PreparedBy);
                cmd.Parameters.AddWithValue("@PreparedTS", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@VerifiedBy", VerifiedBy);
                cmd.Parameters.AddWithValue("@VerifiedTS", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ApprovedBy", ApprovedBy);
                cmd.Parameters.AddWithValue("@ApprovedTS", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Param", "HeaderSave");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SavePMHeaderDetails: {ex.Message}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static int SavePMMasterData(PMMasterEntity data, string MachineID, string RevID, string DocNo, string RevDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int res = 0;
            try
            {
                cmd = new SqlCommand("S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@RevDate", Util.GetDateTime(RevDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@DocNo", DocNo);
                cmd.Parameters.AddWithValue("@CheckpointID", data.CheckpointID);
                cmd.Parameters.AddWithValue("@CheckpointDesc", data.CheckpointDesc);
                cmd.Parameters.AddWithValue("@Particular", data.Particular);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@ControlMethod", data.ControlMethod);
                cmd.Parameters.AddWithValue("@Responsibility", data.Responsibility);
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SavePMMasterData: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static int DeletePMMasterData(string MachineID, string RevID, string CheckpointID, string Frequency)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int res = 0;
            try
            {
                cmd = new SqlCommand("S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@CheckpointID", CheckpointID);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"DeletePMMasterData: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static int SaveNewRevisionNo(string MachineID, string RevID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int res = 0;
            try
            {
                cmd = new SqlCommand("S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@RevID", Convert.ToInt32(RevID));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Param", "Save_NewRevDetails");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SaveNewRevisionNo: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static int CopyRevData(string DestMachineID, string SrcMachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int res = 0;
            try
            {
                cmd = new SqlCommand("S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@OldMachineID", SrcMachineID);
                cmd.Parameters.AddWithValue("@MachineID", DestMachineID);
                cmd.Parameters.AddWithValue("@Param", "CopyFromOldToNewMachine");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SaveNewRevisionNo: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }
        #endregion

        #region -- Import Master -- 
        internal static string SaveImportDataToTemp_PMMaster(DataTable dt)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string isCopied = string.Empty;
            try
            {
                SqlBulkCopy blkObject = new SqlBulkCopy(conn);
                blkObject.DestinationTableName = "TempPM_Master_MachineShop_Vulkan";
                blkObject.ColumnMappings.Add("MachineID", "MachineID");
                blkObject.ColumnMappings.Add("CheckpointID", "CheckpointID");
                blkObject.ColumnMappings.Add("Particular", "Particular");
                blkObject.ColumnMappings.Add("CheckpointItem", "CheckpointDesc");
                blkObject.ColumnMappings.Add("Frequency", "Frequency");
                blkObject.ColumnMappings.Add("ControlMethod", "ControlMethod");
                blkObject.ColumnMappings.Add("Responsibility", "Responsibility");
                blkObject.ColumnMappings.Add("RevID", "RevID");
                blkObject.ColumnMappings.Add("RevNo", "RevNo");
                blkObject.ColumnMappings.Add("RevDate", "RevDate");
                blkObject.ColumnMappings.Add("DocNo", "DocNo");
                blkObject.WriteToServer(dt);
                isCopied = "Successful";
                cmd = new SqlCommand("S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "Import");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SaveImportDataToTemp_PMMaster: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isCopied;
        }
        #endregion

        #region -- Report -- 
        internal static DataTable GetPMReportData(string MachineID, string Year, string Month, out DataTable dt_Sign, out DataTable dt_Dailyapproval, out DataTable dt_WeeklyApproval, out DataTable dt_MonthlyApproval, out DataTable dt_HeaderTable, out DataTable dt_HolidayList)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt_TransactionData = new DataTable();
            dt_Sign = new DataTable();
            dt_Dailyapproval = new DataTable();
            dt_MonthlyApproval = new DataTable();
            dt_WeeklyApproval = new DataTable();
            dt_HeaderTable = new DataTable();
            dt_HolidayList = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@YearNo", string.IsNullOrEmpty(Year) ? DateTime.Now.Year : Convert.ToInt32(Year));
                cmd.Parameters.AddWithValue("@MonthNo", string.IsNullOrEmpty(Month) ? DateTime.Now.Month : Convert.ToInt32(Month));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt_TransactionData.Load(sdr);
                    dt_Sign.Load(sdr);
                    dt_Dailyapproval.Load(sdr);
                    dt_WeeklyApproval.Load(sdr);
                    dt_MonthlyApproval.Load(sdr);
                    dt_HeaderTable.Load(sdr);
                    dt_HolidayList.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt_TransactionData;
        }
        #endregion

        #region -- Approval PM --
        internal static List<PMTransactionEntity> GetPMTransactionDetails(string MachineID, string Year, string Month, string Frequency, out string Message)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            DataTable dt_Sign = new DataTable();
            DataTable dt_Header = new DataTable();
            DataTable dt_Holiday = new DataTable();
            List<PMTransactionEntity> list = new List<PMTransactionEntity>();
            PMTransactionEntity entity = null;
            PMTransactionEntity innerData = null;
            List<PMTransactionEntity> temp_list = null;
            List<string> ListDate = new List<string>();
            Message = "";
            try
            {
                cmd = new SqlCommand("S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "ApproveView");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@YearNo", Year);
                cmd.Parameters.AddWithValue("@MonthNo", Month);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt_Sign.Load(sdr);
                dt_Header.Load(sdr);
                dt_Holiday.Load(sdr);
                int i = 0;
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.IndexOf("Flag") != -1)
                    {
                        Message = "NoRecordsFound";
                    }
                    else
                    {
                        var Holiday_List = dt_Holiday.AsEnumerable().Select(x => Util.GetDateTime(x["Holiday"].ToString()).Day).ToList();
                        if (Frequency.Equals("Daily", StringComparison.OrdinalIgnoreCase))
                            ListDate = dt_Header.AsEnumerable().Select(x => Util.GetDateTime(x["Date"].ToString()).Day.ToString()).Distinct().ToList();
                        else if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                            ListDate = dt_Header.AsEnumerable().Select(x => x["WeekNo"].ToString()).Distinct().ToList();
                        else
                            ListDate = dt_Header.AsEnumerable().Select(x => x["MonthNo"].ToString()).Distinct().ToList();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (i == 0)
                            {
                                entity = new PMTransactionEntity();
                                entity.CheckpointID = "Checkpoint ID";
                                entity.CheckpointItem = "Checkpoint Item";
                                entity.Particular = "Particular";
                                entity.ControlMethod = "Control Method";
                                entity.Responsibility = "Responsibility";
                                entity.HeaderVisibility = true;
                                entity.BackgroundClass = "class-green";
                                temp_list = new List<PMTransactionEntity>();
                               
                                foreach (DataColumn column in dt_Sign.Columns)
                                {
                                    int res = 0;
                                    string columnName = column.ColumnName.ToString(), HeaderVal = "";
                                    List<string> colList = new List<string>();
                                    if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                                    {
                                        colList = columnName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        if (colList.Count > 1)
                                        {
                                            columnName = colList[0];
                                            var dateli = colList[1].Split(' ').ToList();
                                            if (dateli.Count > 1)
                                            {
                                                DateTime fromdate = new DateTime();
                                                DateTime.TryParse(dateli[0], out fromdate);
                                                DateTime todate = new DateTime();
                                                DateTime.TryParse(dateli[2], out todate);
                                                HeaderVal = " [" + fromdate.ToString("dd-MM-yyyy") + " to " + todate.ToString("dd-MM-yyyy") + "]";
                                            }
                                        }
                                        else
                                            continue;
                                    }
                                    int.TryParse(columnName, out res);
                                    if (res > 0)
                                    {
                                        innerData = new PMTransactionEntity();
                                        if (Holiday_List.Contains(res))
                                            innerData.BackgroundClass = "class-red";
                                        else
                                            innerData.BackgroundClass = "class-green";
                                        if (Frequency.Equals("monthly", StringComparison.OrdinalIgnoreCase))
                                            innerData.DayHeader = HelperClassGeneric.getAbbreviatedMonthName(res.ToString());
                                        else
                                        {
                                            innerData.DayHeader = columnName + HeaderVal;
                                        }
                                        innerData.HeaderVisibility = true;
                                        temp_list.Add(innerData);
                                    }
                                }
                                entity.TransactionData = temp_list;
                                list.Add(entity);
                            }

                            entity = new PMTransactionEntity();
                            temp_list = new List<PMTransactionEntity>();
                            entity.CheckpointID = row["CheckpointID"].ToString();
                            entity.CheckpointItem = row["CheckpointItem"].ToString();
                            entity.Particular = row["Particular"].ToString();
                            entity.ControlMethod = row["ControlMethod"].ToString();
                            entity.Responsibility = row["Responsibility"].ToString();
                            entity.ContentVisibility = true;
                            foreach (DataColumn column in dt_Sign.Columns)
                            {
                                int res = 0;
                                string columnName = column.ColumnName.ToString();
                                if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                                {
                                    var colList = columnName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    if (colList.Count > 1)
                                        columnName = colList[0];
                                    else
                                        continue;
                                }
                                int.TryParse(columnName, out res);
                                if (res > 0)
                                {
                                    innerData = new PMTransactionEntity();
                                    if (Holiday_List.Contains(res))
                                        innerData.BackgroundClass = "class-red-data";
                                    else
                                        innerData.BackgroundClass = "class-white-data";
                                    if (row[column.ColumnName].ToString().Equals("DONE", StringComparison.OrdinalIgnoreCase) || row[column.ColumnName].ToString().Equals("OK", StringComparison.OrdinalIgnoreCase))
                                    {
                                        innerData.DayValue = "&#10004;";
                                        innerData.innerDayValueClass = "green";
                                    }
                                    else if (row[column.ColumnName].ToString().Equals("DO", StringComparison.OrdinalIgnoreCase) || row[column.ColumnName].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                                    {
                                        innerData.DayValue = "&#10008;";
                                        innerData.innerDayValueClass = "red";
                                    }
                                    innerData.ContentVisibility = true;
                                    temp_list.Add(innerData);
                                }
                            }
                            entity.TransactionData = temp_list;
                            list.Add(entity);
                            i++;
                        }

                        #region -- Header Row -- 
                        entity = new PMTransactionEntity();
                        temp_list = new List<PMTransactionEntity>();
                        entity.ApproveVisibility = true;
                        entity.CheckpointID = "Operator Sign:";
                        DataRow row_sign = null;
                        if (Frequency == "Daily")
                        {
                            var dt_temp = dt_Sign.AsEnumerable().Where(x => x["Type"].ToString().Equals("Operator", StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                            if (dt_temp.Rows.Count > 0)
                                row_sign = dt_temp.Rows[0];
                        }
                        foreach (DataColumn column1 in dt_Sign.Columns)
                        {
                            int res = 0;
                            string columnName = column1.ColumnName.ToString();
                            if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                            {
                                var colList = columnName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                if (colList.Count > 1)
                                    columnName = colList[0];
                                else
                                    continue;
                            }
                            int.TryParse(columnName, out res);
                            if (res > 0)
                            {
                                innerData = new PMTransactionEntity();
                                if (Holiday_List.Contains(res))
                                    innerData.BackgroundClass = "class-red";
                                if (row_sign != null)
                                    innerData.DayValue = string.IsNullOrEmpty(row_sign[column1.ColumnName].ToString()) ? "" : row_sign[column1.ColumnName].ToString().Split('$')[0] + "[" + Util.GetDateTime(row_sign[column1.ColumnName].ToString().Split('$')[1]).ToString("dd-MM-yyyy") + "]";
                                innerData.ContentVisibility = true;
                                temp_list.Add(innerData);
                            }
                        }
                        entity.TransactionData = temp_list;
                        list.Add(entity);

                        entity = new PMTransactionEntity();
                        temp_list = new List<PMTransactionEntity>();
                        entity.ApproveVisibility = true;
                        entity.CheckpointID = "ME Sign:";
                        DataRow row_sign1 = null;
                        if (Frequency == "Weekly" || Frequency == "Monthly")
                        {
                            var dt_temp = dt_Sign.AsEnumerable().Where(x => x["Type"].ToString().Equals("MaintenanceEngg", StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                            if (dt_temp.Rows.Count > 0)
                                row_sign1 = dt_temp.Rows[0];
                        }
                        foreach (DataColumn column1 in dt_Sign.Columns)
                        {
                            int res = 0;
                            string columnName = column1.ColumnName.ToString();
                            if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                            {
                                var colList = columnName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                if (colList.Count > 1)
                                    columnName = colList[0];
                                else
                                    continue;
                            }
                            int.TryParse(columnName, out res);
                            if (res > 0)
                            {
                                innerData = new PMTransactionEntity();
                                if (Holiday_List.Contains(res))
                                    innerData.BackgroundClass = "class-red";
                                if (row_sign1 != null)
                                    innerData.DayValue = string.IsNullOrEmpty(row_sign1[column1.ColumnName].ToString()) ? "" : row_sign1[column1.ColumnName].ToString().Split('$')[0] + "[" + Util.GetDateTime(row_sign1[column1.ColumnName].ToString().Split('$')[1]).ToString("dd-MM-yyyy") + "]";
                                innerData.ContentVisibility = true;
                                temp_list.Add(innerData);
                            }
                        }
                        entity.TransactionData = temp_list;
                        list.Add(entity);
                        #endregion

                        #region -- Footer Row --
                        entity = new PMTransactionEntity();
                        temp_list = new List<PMTransactionEntity>();
                        entity.CheckpointID = "Approve:";
                        entity.ApproveVisibility = true;
                        entity.ApprovalcolSpan = 5;
                        foreach (DataColumn column in dt_Sign.Columns)
                        {
                            int res = 0;
                            string columnName = column.ColumnName.ToString();
                            if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                            {
                                var colList = columnName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                if (colList.Count > 1)
                                    columnName = colList[0];
                                else
                                    continue;
                            }
                            int.TryParse(columnName, out res);
                            if (res > 0)
                            {
                                innerData = new PMTransactionEntity();
                                innerData.DayHeader = res.ToString();
                                if (Holiday_List.Contains(res))
                                    innerData.BackgroundClass = "class-red";
                                else if (ListDate.Contains(res.ToString()))
                                {
                                    if (Frequency.Equals("Daily", StringComparison.OrdinalIgnoreCase))
                                        innerData.DayValue = dt_Header.AsEnumerable().Where(x => Util.GetDateTime(x["Date"].ToString()).Day.ToString() == res.ToString()).Select(x => x["SupervisorID"].ToString()).FirstOrDefault().ToUpper() + "[" + dt_Header.AsEnumerable().Where(x => Util.GetDateTime(x["Date"].ToString()).Day.ToString() == res.ToString()).Select(x => Util.GetDateTime(x["SupervisorTS"].ToString()).ToString("dd-MM-yyyy")).FirstOrDefault() + "]";
                                    else if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                                        innerData.DayValue = dt_Header.AsEnumerable().Where(x => x["WeekNo"].ToString() == res.ToString()).Select(x => x["SupervisorID"].ToString()).FirstOrDefault().ToUpper() + "[" + dt_Header.AsEnumerable().Where(x => x["WeekNo"].ToString() == res.ToString()).Select(x => Util.GetDateTime(x["SupervisorTS"].ToString()).ToString("dd-MM-yyyy")).FirstOrDefault() + "]";
                                    else
                                        innerData.DayValue = dt_Header.AsEnumerable().Select(x => x["SupervisorID"].ToString()).FirstOrDefault().ToUpper() + "[" + dt_Header.AsEnumerable().Select(x => Util.GetDateTime(x["SupervisorTS"].ToString()).ToString("dd-MM-yyyy")).FirstOrDefault() + "]";
                                    innerData.ContentVisibility = true;
                                }
                                else
                                {
                                    if (HttpContext.Current.Session["UserRole"].ToString().Equals("Supervisor", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Frequency.Equals("Daily", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (Convert.ToInt32(Month) <= DateTime.Now.Month)
                                                innerData.HeaderVisibility = true;
                                            if (res <= DateTime.Now.Day && res == DateTime.Now.Month)
                                                innerData.HeaderVisibility = true;
                                        }
                                        else if (Frequency.Equals("Monthly", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (res <= DateTime.Now.Month)
                                                innerData.HeaderVisibility = true;
                                        }
                                        else
                                            innerData.HeaderVisibility = true;
                                    }

                                }
                                temp_list.Add(innerData);
                            }
                        }
                        entity.TransactionData = temp_list;
                        list.Add(entity);
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetPMTransactionDetails: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static string SaveapprovedDetails(string MachineID, string Year, string Month, string Frequency, string WeekNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string SaveFlag = "";
            try
            {
                cmd = new SqlCommand("S_GetPMCheckSheetDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Param", "ApproveSave");
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@YearNo", Year);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);

                if (Frequency.Equals("Daily", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@MonthNo", Month);
                    Month = Convert.ToInt32(Month) < 10 ? $"{Month}" : Month;
                    cmd.Parameters.AddWithValue("@Date", $"{Year}-{Month}-{WeekNo} 00:00:00");
                    //dummy param
                    cmd.Parameters.AddWithValue("@WeekNo", Convert.ToInt32(DateTime.Now.DayOfYear/7));
                }
                else if (Frequency.Equals("Weekly", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@MonthNo", Month);
                    WeekNo = Convert.ToInt32(WeekNo) < 10 ? $"0{WeekNo}" : WeekNo;
                    cmd.Parameters.AddWithValue("@WeekNo", WeekNo);
                }
                else if (Frequency.Equals("Monthly", StringComparison.OrdinalIgnoreCase))
                    cmd.Parameters.AddWithValue("@MonthNo", WeekNo);

                cmd.Parameters.AddWithValue("@SupervisorID", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@SupervisorTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                        SaveFlag = sdr["SaveFlag"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SaveapprovedDetails: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return SaveFlag;
        }
        #endregion

        #endregion

        #region --- MachineShop Andon ---
        internal static List<MachineAndonData> GetMachineAndonData(string PlantID, string CellID, string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            List<MachineAndonData> andonData = new List<MachineAndonData>();
            MachineAndonData entity = null;
            List<ParameterData> listOfVals = null;
            ParameterData data = null;
            string ShiftID = string.Empty;
            try
            {

                cmd = new SqlCommand("S_GetAndonDetails_MachineShop_Vulkan", conn);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@ShiftID", ShiftID);
                //cmd.Parameters.AddWithValue("@PlantID", PlantID);
                //cmd.Parameters.AddWithValue("@GroupID", CellID);
                //cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    foreach (DataRow row in dt.Rows)
                    {
                        entity = new MachineAndonData();
                        listOfVals = new List<ParameterData>();
                        entity.Machineid = row["MachineID"].ToString();
                        entity.BackColor = row["OEColor"].ToString();
                        foreach (DataColumn column in dt.Columns)
                        {
                            data = new ParameterData();
                            if (column.ColumnName.Equals("Part", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "Part Number";
                                data.LabelValue = row["Part"].ToString();
                                data.SortOrder = 1;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("HeatCode", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "HeatCode";
                                data.LabelValue = row["HeatCode"].ToString();
                                data.SortOrder = 2;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("ProdCount", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "Expected Qty";
                                data.LabelValue = row["ProdCount"].ToString();
                                data.SortOrder = 3;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("ActualQty", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "Actual Qty";
                                data.LabelValue = row["ActualQty"].ToString();
                                data.BackColorTitle = data.BackColorValue = "#098e09";
                                data.ForeColor = "white";
                                data.SortOrder = 4;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("OEE", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "OEE";
                                data.LabelValue = row["OEE"].ToString();
                                data.BackColorValue = row["OEColor"].ToString();
                                data.SortOrder = 5;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("DownTimeInMins", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "Total Downtime";
                                data.LabelValue = row["DownTimeInMins"].ToString();
                                data.BackColorTitle = data.BackColorValue = "red";
                                data.ForeColor = "white";
                                data.SortOrder = 6;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("OperatorName", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "Operator Name";
                                data.LabelValue = row["OperatorName"].ToString();
                                data.SortOrder = 7;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("MaxDownReason", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "Max Down reason";
                                data.LabelValue = row["MaxDownReason"].ToString();
                                data.SortOrder = 8;
                                listOfVals.Add(data);
                            }
                            else if (column.ColumnName.Equals("RejCount", StringComparison.OrdinalIgnoreCase))
                            {
                                data.LabelText = "Rejection Qty";
                                data.LabelValue = row["RejCount"].ToString();
                                data.SortOrder = 9;
                                listOfVals.Add(data);
                            }

                        }
                        entity.ParametersData = listOfVals;
                        andonData.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetMachineAndonData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return andonData;
        }
        #endregion


        #region -- Tool Life --
        #region -- Master --
        internal static DataTable GetToolLifeMasterData(string MachineID, string ToolNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SELECT * FROM MachineToolLife_Master_Vulkan WHERE (MachineID=@MachineID OR ISNULL(@MachineID,'')='') AND (ToolNo=@ToolNo OR ISNULL(@ToolNo,'')='')", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ToolNo", ToolNo);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetToolLifeMasterData: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static int SaveToolLifeMasterData(ToolLifeEntity data, out string DuplicateInterfaceID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            DuplicateInterfaceID = "";
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS(SELECT * FROM MachineToolLife_Master_Vulkan WHERE MachineID=@MachineID AND ToolNo=@ToolNo)
	BEGIN
		IF NOT EXISTS(SELECT * FROM MachineToolLife_Master_Vulkan WHERE InterfaceID=@InterfaceID)
			BEGIN
				INSERT INTO MachineToolLife_Master_Vulkan(MachineID, ToolNo, InterfaceID, ToolType, ToolSpecification, ToolFeed, NoOfEdges, ToolLifeInMeter,UpdatedTS) 
				VALUES(@MachineID, @ToolNo, @InterfaceID, @ToolType, @ToolSpecification, @ToolFeed, @NoOfEdges, @ToolLifeInMeter,@UpdatedTS)
				SELECT 'Inserted' as SaveFlag
			END
		ELSE
			BEGIN
			SELECT 'DuplicateInterfaceID' as SaveFlag
			END
	END
ELSE
	BEGIN
		UPDATE MachineToolLife_Master_Vulkan SET InterfaceID=@InterfaceID, ToolType=@ToolType, ToolSpecification=@ToolSpecification, ToolFeed = @ToolFeed, NoOfEdges=@NoOfEdges, 
		ToolLifeInMeter=@ToolLifeInMeter, UpdatedTS=@UpdatedTS WHERE MachineID=@MachineID AND ToolNo=@ToolNo
        SELECT 'Updated' as SaveFlag
	END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@ToolNo", data.ToolNo);
                cmd.Parameters.AddWithValue("@InterfaceID", data.InterfaceID);
                cmd.Parameters.AddWithValue("@ToolType", data.ToolType);
                cmd.Parameters.AddWithValue("@ToolSpecification", data.ToolSpecification);
                cmd.Parameters.AddWithValue("@ToolFeed", data.ToolFeed);
                cmd.Parameters.AddWithValue("@NoOfEdges", data.NoOfEdges);
                cmd.Parameters.AddWithValue("@ToolLifeInMeter", data.ToolLifeInMeter);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                DuplicateInterfaceID = cmd.ExecuteScalar().ToString();
                if (DuplicateInterfaceID.Equals("Inserted", StringComparison.OrdinalIgnoreCase) || DuplicateInterfaceID.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                    result = 1;
                else
                    result = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SaveToolLifeMasterData: {ex.Message}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }

        internal static int DeleteToolLifeMasterData(string MachineID, string ToolNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                cmd = new SqlCommand(@"IF EXISTS(SELECT * FROM MachineToolLife_Master_Vulkan WHERE MachineID=@MachineID AND ToolNo=@ToolNo)
	BEGIN
	DELETE FROM MachineToolLife_Master_Vulkan WHERE MachineID=@MachineID AND ToolNo=@ToolNo
	END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ToolNo", ToolNo);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"DeleteToolLifeMasterData: {ex.ToString()}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        #endregion

        #region -- Tool Life Report --
        internal static DataTable GetToolLifeReportData(string FromDate, string ToDate, string MachineID, out DataTable dt_Header)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dtToolData = new DataTable();
            dt_Header = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_GetToolLifeReport_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dtToolData.Load(sdr);
                    dt_Header.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetToolLifeReportData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dtToolData;
        }
        #endregion

        #region -- Cycle Time Report --
        internal static DataTable GetCycleTimeReportData(string FromDate, string ToDate, string MachineID, out DataTable dtSummary)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dtCycleTimeData = new DataTable();
            dtSummary = new DataTable();
            try
            {
                cmd = new SqlCommand("S_GetCycleTimeDataSheet_MachineShop_Vulkan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dtCycleTimeData.Load(sdr);
                    dtSummary.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetCycleTimeReportData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dtCycleTimeData;
        }
        #endregion
        #endregion
    }
}