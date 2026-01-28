using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.MahindraAndMahindra
{
    public class MandMDataBaseAccess
    {
        internal static ProcessParameterData GetParamDashboardData(string selectedMachine)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            ProcessParameterData DataEntity = new ProcessParameterData();
            try
            {
                cmd = new SqlCommand("S_GetLiveDashboardData_MnM", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", selectedMachine);
                rdr = cmd.ExecuteReader();
                DataEntity.lstProcessParameters = new List<DTO>();
                List<DTO> MainEntity = new List<DTO>();
                while (rdr.Read())
                {

                    DTO entity = new DTO();
                    entity.ParameterId = (rdr["ParameterID"].ToString());
                    entity.ParameterName = rdr["ParameterName"].ToString();
                    entity.MinValue = rdr["MinValue"].ToString();
                    entity.DisplayText = rdr["DisplayText"].ToString();
                    entity.MaxValue = rdr["MaxValue"].ToString();
                    entity.TemplateType = rdr["TemplateType"].ToString();
                    entity.ParameterValue = rdr["ParameterValue"].ToString();
                    entity.Unit = rdr["Unit"].ToString();
                    switch (entity.TemplateType)
                    {
                        case "Text":
                            entity.Visibility = "none";
                            break;
                        case "High/Low Limits":
                            entity.Visibility = "";
                            break;
                        case "High/Low":
                            entity.Visibility = "";
                            break;
                        default:
                            entity.Visibility = "none";
                            break;
                    }
                    string paramColor = rdr["ParameterColor"].ToString();
                    entity.BackgroundColor = !string.IsNullOrEmpty(rdr["ParameterColor"].ToString()) ? string.Equals(rdr["ParameterColor"].ToString(), "Green", StringComparison.OrdinalIgnoreCase) ? "#28bb28" : string.Equals(rdr["ParameterColor"].ToString(), "white", StringComparison.OrdinalIgnoreCase) ? "#2775ea" : rdr["ParameterColor"].ToString() : "#2775ea";
                    entity.ForeColor = paramColor.Equals("Green", StringComparison.OrdinalIgnoreCase) ? "#555" : paramColor.Equals("Red", StringComparison.OrdinalIgnoreCase) ? "#FFFFFF" : "#2C639B";
                    entity.HeaderForeColor = paramColor.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || paramColor.Equals("Green", StringComparison.OrdinalIgnoreCase) ? System.Drawing.Color.Black : System.Drawing.Color.White;
                    MainEntity.Add(entity);
                }
                DataEntity.lstProcessParameters = MainEntity;
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
            return DataEntity;
        }

        internal static List<string> GetParameterList()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> dt = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct ParameterID from ProcessParameterMaster_BajajIoT where GraphView=1", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    dt.Add(rdr["ParameterID"].ToString());
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
        internal static Dictionary<string, string> GetAllParameterIDsandName(string machineid)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                string sqlQuery = "select distinct ParameterID,ParameterName,DisplayText from ProcessParameterMaster_BajajIoT where GraphView =1 and MachineID=@machine";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machine", machineid);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["ParameterID"]))
                        {
                            if (!list.ContainsKey(sdr["ParameterID"].ToString()))
                            {
                                if (string.IsNullOrEmpty(sdr["DisplayText"].ToString()))
                                    list.Add(sdr["ParameterID"].ToString(), sdr["ParameterID"].ToString());
                                else
                                    list.Add(sdr["ParameterID"].ToString(), sdr["DisplayText"].ToString());
                            }
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
            return list;
        }
        internal static DataTable GetParameterisedData(string machineID, string parameter, string StartDate, string EndDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("S_GetLiveDashboardData_Bajaj", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@Param", "Graph");
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dt = dt.AsEnumerable().OrderBy(k => k.Field<DateTime>("UpdatedtimeStamp")).CopyToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
        internal static DataTable getParameterMasterData(string machineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("select * from ProcessParameterMaster_BajajIoT where MachineID=@machineid", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineid", machineID);
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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