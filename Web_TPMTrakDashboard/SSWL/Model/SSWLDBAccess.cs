using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.SSWL.Model
{
    public class SSWLDBAccess
    {
        #region ---------- Screen - Machie Association -----------
        public static List<string> getScreenNames()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand command = new SqlCommand("select distinct ScreenName from ScreenDetails_SSWL", con);
                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 360;
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["ScreenName"].ToString());
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
        public static DataTable getScreenMachineAssociationData(string screenName, string machineID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand command = new SqlCommand("[dbo].[AssignScreenToMachine_SSWL]", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Screen", screenName);
                command.Parameters.AddWithValue("@MachineID", machineID);
                command.CommandTimeout = 360;
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
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
            return dt;
        }

        internal static int insertDeleteScreenMachineAssociation(string screenName,string machineID, string param)
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
                    query = @"if not exists (select * from AssignMachinesToScreens_SSWL where ScreenName=@ScreenName and MachineID=@MachineID)
begin
	insert into AssignMachinesToScreens_SSWL(ScreenName,MachineID,UpdatedTS) values(@ScreenName,@MachineID,@UpdatedTS)
end";
                }
                else if (param.Equals("delete", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"delete from AssignMachinesToScreens_SSWL where ScreenName=@ScreenName  and MachineID=@MachineID ";
                }
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ScreenName", screenName);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertDeleteScreenMachineAssociation = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return result;
        }
        #endregion
    }
}