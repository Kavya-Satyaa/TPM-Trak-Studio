using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.ProcessParameter
{
    public class ProcessParameterDataBaseAccess
    {
        public static ICockpitStyle values = CockpitDataBaseAccess.GetCockpitBackColorValues();
        public static MachineStatusColorStyle machineStatusColors = CockpitDataBaseAccess.GetMachineStatusColorValues();
        internal static List<ProcessParaMeterSettingsEntity> GetProcessParameterSettingsData(List<string> dataType, List<string> displayTemp,List<string> parameterid)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            List<ProcessParaMeterSettingsEntity> ProcessParaMeterSettingsEntityList = new List<ProcessParaMeterSettingsEntity>();
            string query = string.Empty;
            query = @"select * from [ProcessParameterSettings_BaluAuto]";
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ProcessParaMeterSettingsEntity ProcessParaMeterSettingsEntityData = new ProcessParaMeterSettingsEntity();
                        ProcessParaMeterSettingsEntityData.MachineID = rdr["MachineID"].ToString();
                        ProcessParaMeterSettingsEntityData.idd = Convert.ToInt32(rdr["idd"].ToString());
                        ProcessParaMeterSettingsEntityData.ParameterID = rdr["ParameterID"].ToString();
                        ProcessParaMeterSettingsEntityData.ParameterIDList = parameterid;
                        ProcessParaMeterSettingsEntityData.DisplayHeader = rdr["DisplayHeader"].ToString();
                        ProcessParaMeterSettingsEntityData.PlcAddress = rdr["PLCAddress"].ToString();
                        ProcessParaMeterSettingsEntityData.Datatype = rdr["DataType"].ToString();
                        ProcessParaMeterSettingsEntityData.DatatypeList = dataType;
                        ProcessParaMeterSettingsEntityData.Unit = rdr["Unit"].ToString();
                        ProcessParaMeterSettingsEntityData.ShowUnit = Convert.ToBoolean(rdr["ShowUnit"].ToString()); ;
                        ProcessParaMeterSettingsEntityData.ShowDatadate = Convert.ToBoolean(rdr["ShowdataDate"].ToString()); ;
                        ProcessParaMeterSettingsEntityData.GreenRange = rdr["GreenRange"].ToString();
                        ProcessParaMeterSettingsEntityData.YellowHigher = rdr["YellowHigherRange"].ToString();
                        ProcessParaMeterSettingsEntityData.YellowLower = rdr["YellowLowerRange"].ToString();
                        ProcessParaMeterSettingsEntityData.RedHigher = rdr["RedHigherRange"].ToString();
                        ProcessParaMeterSettingsEntityData.RedLower = rdr["RedLowerRange"].ToString();
                        ProcessParaMeterSettingsEntityData.Enable = Convert.ToBoolean(rdr["Enabled"].ToString()); ;
                        ProcessParaMeterSettingsEntityData.DisplayOrder = rdr["DisplayOrder"].ToString();
                        ProcessParaMeterSettingsEntityData.DisplayTemplate = rdr["DisplayTemplate"].ToString();
                        ProcessParaMeterSettingsEntityData.DisplayTemplateList = displayTemp;
                        ProcessParaMeterSettingsEntityList.Add(ProcessParaMeterSettingsEntityData);
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
            return ProcessParaMeterSettingsEntityList;

        }

        internal static void SaveProcessParameter(string machineID, string parameterID, string displayHeader, string plcAddress, string datatype, string unit, bool showDataDate, bool showUnit, string greenRange, string yellowHigher, string yellowLower, string redHigher, string redLower, bool isEnabled, string displayOrder, string displayTemplate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            query = @"if not exists (select * from ProcessParameterSettings_BaluAuto where MachineID=@MachineID and ParameterID=@ParameterID)
                      begin
                             insert into ProcessParameterSettings_BaluAuto (MachineID,ParameterID,DisplayHeader,PLCAddress,DataType,Unit,ShowUnit,ShowdataDate,GreenRange,YellowHigherRange,YellowLowerRange,RedHigherRange,RedLowerRange,[Enabled],DisplayOrder,DisplayTemplate)
                             values(@MachineID,@ParameterID,@DisplayHeader,@PLCAddress,@DataType,@Unit,@ShowUnit,@ShowdataDate,@GreenRange,@YellowHigherRange,@YellowLowerRange,@RedHigherRange,@RedLowerRange,@Enabled,@DisplayOrder,@DisplayTemplate)
                      end
                      else
                      begin
                            update ProcessParameterSettings_BaluAuto set DisplayHeader=@DisplayHeader,PLCAddress=@PLCAddress,DataType=@DataType,Unit=@Unit,ShowUnit=@ShowUnit,ShowdataDate=@ShowdataDate,GreenRange=@GreenRange,YellowHigherRange=@YellowHigherRange,YellowLowerRange=@YellowLowerRange,RedHigherRange=@RedHigherRange,RedLowerRange=@RedLowerRange,[Enabled]=@Enabled,DisplayOrder=@DisplayOrder,DisplayTemplate=@DisplayTemplate where MachineID=@MachineID and ParameterID=@ParameterID
                      end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ParameterID", parameterID);
                cmd.Parameters.AddWithValue("@DisplayHeader", displayHeader);
                cmd.Parameters.AddWithValue("@PLCAddress", plcAddress);
                cmd.Parameters.AddWithValue("@DataType", datatype);
                cmd.Parameters.AddWithValue("@Unit", unit);
                cmd.Parameters.AddWithValue("@ShowUnit", showUnit);
                cmd.Parameters.AddWithValue("@ShowdataDate", showDataDate);
                cmd.Parameters.AddWithValue("@GreenRange", greenRange);
                cmd.Parameters.AddWithValue("@YellowHigherRange", yellowHigher);
                cmd.Parameters.AddWithValue("@YellowLowerRange", yellowLower);
                cmd.Parameters.AddWithValue("@RedHigherRange", redHigher);
                cmd.Parameters.AddWithValue("@RedLowerRange", redLower);
                cmd.Parameters.AddWithValue("@Enabled", isEnabled);
                cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);
                cmd.Parameters.AddWithValue("@DisplayTemplate", displayTemplate);
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

        internal static void DeletProcessParameterSettings(string idd)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            query = @"delete from ProcessParameterSettings_BaluAuto where idd=@idd";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idd", idd);
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


        internal static DataTable GetProcessParameterData(out DataTable Header)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            Header = new DataTable();
            string query = string.Empty;
            DataTable ParameterList = new DataTable();

            query = @"s_GetProcessParameterDashboard_BaluAuto";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", "2019-05-10 06:00:00");
                cmd.Parameters.AddWithValue("@EndTime", "2019-05-10 14:00:00");
                rdr = cmd.ExecuteReader();
                Header.Load(rdr);
                Header.AcceptChanges();
                ParameterList.Load(rdr);
                ParameterList.AcceptChanges();
                ParameterList.Columns.Add("OEEColor");
                ParameterList.Columns.Add("StatusImage");
                ParameterList.Columns.Add("MachineOEE");
                ParameterList.Columns.Add("OKNOT");
                ParameterList.Columns.Add("Visibility");
                double Oee = 0.0;
                foreach (DataRow row in ParameterList.Rows)
                {
                    double.TryParse(row["OverallEfficiency"].ToString(), out Oee);
                    row["OverallEfficiency"] = Oee.ToString("0.00");
                    double greenoee = 0.0; double redoee = 0.0;
                    double.TryParse(row["OEGreen"].ToString(), out greenoee);
                    double.TryParse(row["OERed"].ToString(), out redoee);
                    if (Oee >= greenoee)
                    {
                        row["OEEColor"] = "Green";
                    }
                    else if (Oee <= redoee && Oee > 0)
                    {
                        row["OEEColor"] = "Red";
                    }
                    else if (Oee > redoee && Oee < greenoee)
                    {
                        row["OEEColor"] = "Yellow";
                    }
                    else
                    {
                        row["OEEColor"] = "White";
                    }

                    if (row["MCStatus"].ToString().Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                    {
                        row["StatusImage"] = "~/Image/McStatus/Stopped.gif";//list.StatusImage = "Image/McStatus/Stopped.gif";
                        row["MachineOEE"] = "Red";
                        row["OKNOT"] = "Stopped";
                        row["Visibility"] = "true";
                    }
                    else if (row["MCStatus"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase))
                    {
                        row["StatusImage"] = "~/Image/McStatus/Running.gif";//list.StatusImage = "Image/McStatus/Running.gif";
                        row["MachineOEE"] = "Green";
                        row["OKNOT"] = "Running";
                        row["Visibility"] = "true";
                    }
                    else
                    {
                        row["MCStatus"] = "";
                        row["MachineOEE"] = "White";
                        row["Visibility"] = "false";
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
            return ParameterList;
        }

    }
}