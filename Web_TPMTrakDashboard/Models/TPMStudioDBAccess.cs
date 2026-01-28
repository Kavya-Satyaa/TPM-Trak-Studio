using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard;

namespace Web_TPMTrakDashboard.Models
{
    public class TPMStudioDBAccess
    {
        public static DataTable GetPlants()
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                string sql = "select * from PlantInformation";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("TPMStudioDBAccess.GetPlants: " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }

        public static DataTable GetMachines(string plantID = null)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                string sql = "SELECT DISTINCT MI.Machineid FROM machineinformation MI INNER JOIN plantMachine PM ON MI.Machineid = PM.Machineid WHERE ( PM.Plantid IN (SELECT Item FROM SplitStrings(@plantid, ',')) OR ISNULL(@plantid, '') = '' );";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@plantid", plantID ?? string.Empty);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("TPMStudioDBAccess.GetMachines: " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }

        public static DataTable GetComponents(string machineID = null)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                string sql = "select distinct componentid from componentoperationpricing where ((machineid in (SELECT Item FROM SplitStrings(@MachineID,',')) or isnull(@machineid,'')='') )";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@MachineID", machineID ?? string.Empty);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("TPMStudioDBAccess.GetComponents: " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }

        public static DataTable GetOperations(string machineID = null, string compID = null)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                string sql = "SELECT DISTINCT operationno FROM componentoperationpricing WHERE (componentid IN (SELECT Item FROM SplitStrings(@componentid, ',')) OR ISNULL(@componentid, '') = '') AND (machineid IN (SELECT Item FROM SplitStrings(@machineid, ',')) OR ISNULL(@machineid, '') = '');";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@MachineID", machineID ?? string.Empty);
                da.SelectCommand.Parameters.AddWithValue("@componentid", compID ?? string.Empty);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("TPMStudioDBAccess.GetOperations: " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }

        public static DataTable GetShifts()
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                string sql = "SELECT DISTINCT ShiftName as Name FROM ShiftDetails ORDER BY ShiftName";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("TPMStudioDBAccess.GetShifts: " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }

        public static DataTable GetReportViews()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Rows.Add("Summary", "Summary Report");
            dt.Rows.Add("Detailed", "Detailed Report");
            dt.Rows.Add("Graphical", "Graphical View");
            dt.Rows.Add("Comparison", "Comparison View");
            return dt;
        }

        public static DataTable GetDummyData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Module", typeof(string));
            dt.Columns.Add("Machine", typeof(string));
            dt.Columns.Add("Status", typeof(bool));
            dt.Columns.Add("Value", typeof(int));
            dt.Columns.Add("Description", typeof(string));

            dt.Rows.Add("Cockpit", "CNC-01", true, 85, "High performance today");
            dt.Rows.Add("Quality", "CNC-02", false, 45, "Maintenance required");
            dt.Rows.Add("Production", "VMC-05", true, 92, "Optimal operation");
            dt.Rows.Add("Efficiency", "LATHE-03", true, 78, "Stable speed");

            return dt;
        }
    }
}
