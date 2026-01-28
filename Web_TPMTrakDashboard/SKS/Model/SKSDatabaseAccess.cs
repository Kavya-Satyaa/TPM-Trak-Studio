using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.SKS.Model
{
    public class SKSDatabaseAccess
    {
        #region --------- Schedule Master ---------
        internal static List<ScheduleEntity> getScheduleMasterDetails(string machineID, string partID, string status, string workOrder)
        {
            List<ScheduleEntity> list = new List<ScheduleEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[SP_ViewScheduleDetails_SKS]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machineid", machineID);
                cmd.Parameters.AddWithValue("@CatalogCode", partID);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@WorkOrder", workOrder);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ScheduleEntity data = new ScheduleEntity();
                        data.UserPriority = rdr["UserPriority"].ToString();
                        data.SchedulePriority = rdr["SystemPriority"].ToString();
                        data.MachineID = rdr["MachineName"].ToString();
                        data.WorkOrder = rdr["WorkOrderNo"].ToString();
                        data.PartID = rdr["CatalogCode"].ToString();
                        data.PartDesc = rdr["CatalogCode_Description"].ToString();
                        data.ToolLayout = rdr["ToolLayout"].ToString();
                        data.DrawingNumber = rdr["Drawingnumber"].ToString();
                        data.RMGradeSize = rdr["RMGrade_Size"].ToString();
                        data.PlannedQtyNo = rdr["PlannedQty"].ToString();
                        data.PlannedQtyWt = rdr["PlnQtyWeightInKg"].ToString();
                        data.OperationNo = rdr["Operationno"].ToString();
                        data.Speed = rdr["Speed"].ToString();
                        data.Status = rdr["ScheduleStatus"].ToString();
                        data.UpdatedTS = rdr["UpdateTS"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getScheduleMasterDetails - \n " + ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<string> getAllSchedulePrioritiesKeyValue(string machine)
        {
            List<string> lstPriorities = new List<string>();
            SqlConnection Conn = null;
            SqlDataReader sqlDataReader = null;
            string query = @"select distinct AutoID from ScheduleDetailsMain_SKS where MachineName = @Machineid and status='New'";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        lstPriorities.Add(sqlDataReader["AutoID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return lstPriorities;
        }
        #endregion

        #region ------ Production Down Rejection Data --------
        internal static DataTable getProdDownRejectionData(string fromDate, string toDate, string shift, string machine, string partID, string param,string partDescSearch)
        {
            DataTable dt = new DataTable();
            SqlConnection Conn = null;
            SqlDataReader sdr = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand("[dbo].[SP_ViewERPData_sks]", Conn);
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Todate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@Componentid", partID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : partID);
                cmd.Parameters.AddWithValue("@ComponentDescription", partDescSearch);
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.Columns.Add("FormateDate", typeof(string));
                if (dt.Rows.Count > 0)
                {
                    dt.AsEnumerable().ToList().ForEach(k => k["FormateDate"] = Util.GetDateTime(k["Date"].ToString()).ToString("dd-MM-yyyy"));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (Conn != null) Conn.Close();
            }
            return dt;
        }
        #endregion

        #region --- Production Report SKS ---
        internal static DataTable GetForgingMachinePerformanceReport(string MachineID, string Date, out DataTable dt_HeaderData) //
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            //dt_ProdData = new DataTable();
            dt_HeaderData = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_ERPPerformanceReport_SKS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Machine", MachineID);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date).ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt_HeaderData.Load(sdr);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog($"GetForgingMachinePerformanceReport: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
        #endregion
    }
}