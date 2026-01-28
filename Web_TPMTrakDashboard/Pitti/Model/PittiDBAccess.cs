using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Pitti.Model
{
    public class PittiDBAccess
    {
        internal static DataTable getWorkOrderTrackingDetails(string fromDate, string toDate, string workOrderSearch, string slnoSearch)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[SP_WorkOrder_SerialTrackingDashbord_Pitti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (fromDate != "" && toDate != "")
                {
                    cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                cmd.Parameters.AddWithValue("@WorkOrderNumber", workOrderSearch);
                cmd.Parameters.AddWithValue("@SerialNumber", slnoSearch);
                cmd.Parameters.AddWithValue("@Param", "View");
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
        internal static int insertWorkOrderRejectionDetails(string workOrder, string serialNo, string rejectionRemarks,string rejectionBy)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[SP_WorkOrder_SerialTrackingDashbord_Pitti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkOrderNumber", workOrder);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNo);
                cmd.Parameters.AddWithValue("@RejectionRemarks", rejectionRemarks);
                cmd.Parameters.AddWithValue("@RejectionTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@RejectionBy", rejectionBy);
                cmd.Parameters.AddWithValue("@Param", "insert");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        if (rdr["flag"].ToString() == "Inserted")
                        {
                            result = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }
            return result;
        }
    }
}