using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PTA
{
    public class DataBaseAccessPTA
    {
        public static string CurrentStartEndTime(string date, out string todate, out string shiftName)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand Cmd = null;
            SqlDataReader reader = null;
            string frmDate = string.Empty;
            todate = shiftName = string.Empty;
            try
            {
                Cmd = new SqlCommand("s_GetCurrentShiftTime", Con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@StartDate", date);
                reader = Cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        frmDate = Convert.ToDateTime(reader["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");//"2018-10-24 06:00:00"
                        todate = Convert.ToDateTime(reader["Endtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");//"2018-10-24 14:00:00";
                        shiftName = reader["ShiftName"].ToString();//""
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (Con != null) Con.Close();
            }
            return frmDate;
        }

        public static DataTable GetMachineUpDownTimes(string startDate, string toDate, string shift, string plantId, string machineId, string param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();// new SqlConnection(@"Data Source=AMIT-DEV7\SQL2017STD; Initial Catalog=Peekay_29-10-2018;User ID=sa; password=pctadmin$123");// 
            SqlCommand Cmd = null;
            SqlDataAdapter sda = null;
            DataTable dtPDT = new DataTable();
            machineId = machineId == "All" ? "" : machineId;
            plantId = plantId == "All" ? "" : plantId;
            try
            {
                Cmd = new SqlCommand("s_GetMachineUptimeDowntimeDetails", Con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandTimeout = 1000;
                Cmd.Parameters.AddWithValue("@StartTime", startDate);//"2018-10-24 06:00:00"
                Cmd.Parameters.AddWithValue("@EndTime", toDate);//"2018-10-24 14:00:00"																
                if (shift.Equals("All"))
                    Cmd.Parameters.AddWithValue("@shift", "");
                else
                    Cmd.Parameters.AddWithValue("@shift", shift);
                //Cmd.Parameters.AddWithValue("@Machineid", "'" + drpMachineId.Text + "'");
                Cmd.Parameters.AddWithValue("@Machineid", machineId);
                Cmd.Parameters.AddWithValue("@plantid", plantId);
                Cmd.Parameters.AddWithValue("@param", param);
                sda = new SqlDataAdapter(Cmd);
                sda.Fill(dtPDT);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sda != null) sda.Dispose();
                if (Con != null) Con.Close();
            }
            return dtPDT;
        }

        public static object IgnorePDT()
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand Cmd = null;
            object res = string.Empty;

            try
            {
                Cmd = new SqlCommand("Select ValueIntext from cockpitdefaults where parameter='Ignore_Dtime_4m_PLD' or parameter='Ignore_Ptime_4m_PLD'", Con);
                res = Cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return res;
        }

        public static DateTime[] GetLogicalStartEnd(string date)
        {
            DateTime[] LogicalStartEnd = new DateTime[2];
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand Cmd = null;

                string sqlQuery = string.Empty;
                object obj = null;

                sqlQuery = "select dbo.f_GetLogicalDay('" + DateTime.ParseExact(string.Format("{0} {1}", date, DateTime.Now.ToString("HH:mm:ss")), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss") + "','start')";

                Cmd = new SqlCommand(sqlQuery, conn);
                obj = Cmd.ExecuteScalar();
                LogicalStartEnd[0] = Convert.ToDateTime(obj.ToString()); //DateTime.Parse("2018-10-24 06:00:00");

                sqlQuery = "select dbo.f_GetLogicalDay('" + DateTime.ParseExact(string.Format("{0} {1}", date, DateTime.Now.ToString("HH:mm:ss")), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss") + "','end')";
                Cmd = new SqlCommand(sqlQuery, conn);
                obj = Cmd.ExecuteScalar();
                LogicalStartEnd[1] = Convert.ToDateTime(obj.ToString());// DateTime.Parse("2018-10-24 14:00:00");																  

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

            finally
            {
                if (conn != null) conn.Close();
            }
            return LogicalStartEnd;

        }

        public static string GetshiftTime(string shift)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand Cmd = null;
            string shiftTime = string.Empty;
            SqlDataReader reader = null;
            try
            {
                string query = "select * from shiftdetails where running =1 and shiftname=@shiftID";
                Cmd = new SqlCommand(query, Con);
                Cmd.Parameters.AddWithValue("@shiftID", shift);
                reader = Cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        shiftTime = string.Format("{0:HH:mm:ss}", DateTime.Parse(reader["ToTime"].ToString()));
                    }
                }
                else
                {
                    shiftTime = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (Con != null) Con.Close();
            }
            return shiftTime;
        }
        #region ---- Machine View and Plant view------
        public static List<string> getEmployeeForPlant(string plantID)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct Employeeid from plantemployee where plantid =@Plantid", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Plantid", plantID);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["Employeeid"].ToString());
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
                if (con != null) con.Close();
            }
            return list;
        }
        public static List<PlantMachineViewEntity> getMachineViewDetails(string date, string plantID)
        {
            List<PlantMachineViewEntity> list = new List<PlantMachineViewEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetCockpitdetails_PTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Plantid", plantID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@shift", "");
                cmd.Parameters.AddWithValue("@param", "Machinewise");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        PlantMachineViewEntity data = new PlantMachineViewEntity();
                        data.Machine = sdr["MachineID"].ToString();
                        data.ProductionTime = sdr["ProductionTime"].ToString() == "" ? "" : sdr["ProductionTime"].ToString() + "(" + sdr["ProdEff"].ToString() + "%)";
                        data.LoadUnLoad = sdr["LoadUnload"].ToString() == "" ? "" : sdr["LoadUnload"].ToString() + "(" + sdr["LoadUnloadEff"].ToString() + "%)";
                        data.DownTime = sdr["MachineStoppages"].ToString() == "" ? "" : sdr["MachineStoppages"].ToString() + "(" + sdr["MachineStoppageEff"].ToString() + "%)";
                        data.PlannedParts = sdr["PlannedParts"].ToString();
                        data.ActualParts = sdr["NumberOfCycles"].ToString();
                        data.Delivery = sdr["delivery"].ToString() == "" ? "" : sdr["delivery"].ToString() + "%";
                        double prodEffy = HelperClassGeneric.getDoubleValueFromString(sdr["ProdEff"].ToString());
                        if (prodEffy <= 65)
                        {
                            data.RowBackColor = "red";
                            data.RowForeColor = "white";
                        }
                        else if (prodEffy > 65 && prodEffy < 80)
                        {
                            data.RowBackColor = "yellow";
                            data.RowForeColor = "black";
                        }
                        else if (prodEffy >= 80)
                        {
                            data.RowBackColor = "green";
                            data.RowForeColor = "black";
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        public static string getPlantViewMachineCount(string plantID)
        {
            string machineCount = "0";
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("Select count(distinct machineid) as Mcount from PlantMachine where PlantId =@plantID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plantID", plantID);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        machineCount = sdr["Mcount"].ToString();
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
                if (con != null) con.Close();
            }
            return machineCount;
        }
        public static PlantMachineViewEntity getPlantViewDetails(string date, string plantID)
        {
            PlantMachineViewEntity data = new PlantMachineViewEntity();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetCockpitdetails_PTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Plantid", plantID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@shift", "");
                cmd.Parameters.AddWithValue("@param", "Plantwise");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data.ProductionTime = sdr["ProductionTime"].ToString() + "(" + sdr["ProdEff"].ToString() + "%)";
                        data.NonProductionTime = sdr["NonProductionTime"].ToString() + "(" + sdr["NonProdEff"].ToString() + "%)";
                        data.LoadUnLoad = sdr["LoadUnload"].ToString() + "(" + sdr["LoadunloadEff"].ToString() + "%)";
                        data.MachineStoppage = sdr["MachineStoppages"].ToString() + "(" + sdr["MachineStoppageEff"].ToString() + "%)";
                        data.ActualParts = sdr["NumberOfCycles"].ToString();
                        data.TargetRevenue = "Rs. " + sdr["TargetRevenue"].ToString();

                        data.ProductionEffy = HelperClassGeneric.getDoubleValueFromString(sdr["ProdEff"].ToString());
                        data.NonProductionEffy = HelperClassGeneric.getDoubleValueFromString(sdr["NonProdEff"].ToString());
                        data.LoadUnloadEffy = HelperClassGeneric.getDoubleValueFromString(sdr["LoadunloadEff"].ToString());
                        data.MachineStoppageEffy = HelperClassGeneric.getDoubleValueFromString(sdr["MachineStoppageEff"].ToString());
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
                if (con != null) con.Close();
            }
            return data;
        }
        #endregion
        #region --- Day and Shift View ----
        public static PlantMachineViewEntity getDaywiseMachineProductionTimeDetails(string date, string plantID, string machineID, string shift)
        {
            PlantMachineViewEntity data = new PlantMachineViewEntity();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetCockpitdetails_PTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Plantid", plantID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                if (shift.Equals("All", StringComparison.OrdinalIgnoreCase) || shift.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@shift", "");
                    cmd.Parameters.AddWithValue("@param", "Machinewise");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@shift", shift);
                    cmd.Parameters.AddWithValue("@param", "Shiftwise");
                }
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data.ProductionTime = sdr["ProductionTime"].ToString() + "(" + sdr["ProdEff"].ToString() + "%)";
                        data.NonProductionTime = sdr["NonProductionTime"].ToString() + "(" + sdr["NonProdEff"].ToString() + "%)";
                        data.LoadUnLoad = sdr["LoadUnload"].ToString() + "(" + sdr["LoadunloadEff"].ToString() + "%)";
                        data.MachineStoppage = sdr["MachineStoppages"].ToString() + "(" + sdr["MachineStoppageEff"].ToString() + "%)";
                        data.ActualParts = sdr["NumberOfCycles"].ToString();
                        data.TargetRevenue = "Rs. " + sdr["TargetRevenue"].ToString();

                        data.ProductionEffy = HelperClassGeneric.getDoubleValueFromString(sdr["ProdEff"].ToString());
                        data.NonProductionEffy = HelperClassGeneric.getDoubleValueFromString(sdr["NonProdEff"].ToString());
                        data.LoadUnloadEffy = HelperClassGeneric.getDoubleValueFromString(sdr["LoadunloadEff"].ToString());
                        data.MachineStoppageEffy = HelperClassGeneric.getDoubleValueFromString(sdr["MachineStoppageEff"].ToString());
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
                if (con != null) con.Close();
            }
            return data;
        }

        public static List<MachineStoppageEntity> getDaywiseMachineStoppageDetails(string date, string plantID, string machineID, string shift)
        {
            List<MachineStoppageEntity> list = new List<MachineStoppageEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetCockpitdetails_PTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Plantid", plantID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                if (shift.Equals("All", StringComparison.OrdinalIgnoreCase) || shift.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("shift", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("shift", shift);
                }
                cmd.Parameters.AddWithValue("@param", "MachinewiseDown");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MachineStoppageEntity data = new MachineStoppageEntity();
                        data.FromTime = sdr["StartTime"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["StartTime"].ToString()).ToString("HH:mm:ss");
                        data.ToTime = sdr["EndTime"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["EndTime"].ToString()).ToString("HH:mm:ss");
                        data.Duration = sdr["DownTime"].ToString();
                        data.Reason = sdr["DownDescription"].ToString();
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }

        public static List<PlantMachineViewEntity> getShiftwiseMachineProductionTimeDetails(string date, string plantID, string machineID)
        {
            List<PlantMachineViewEntity> list = new List<PlantMachineViewEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetCockpitdetails_PTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Plantid", plantID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@shift", "");
                cmd.Parameters.AddWithValue("@param", "Shiftwise");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        PlantMachineViewEntity data = new PlantMachineViewEntity();
                        data.Shift = sdr["shiftname"].ToString();
                        data.ProductionTime = sdr["ProductionTime"].ToString() + "(" + sdr["ProdEff"].ToString() + "%)";
                        data.NonProductionTime = sdr["NonProductionTime"].ToString() + "(" + sdr["NonProdEff"].ToString() + "%)";
                        data.LoadUnLoad = sdr["LoadUnload"].ToString();
                        data.MachineStoppage = sdr["MachineStoppages"].ToString();
                        data.ActualParts = sdr["NumberOfCycles"].ToString();

                        data.ProductionEffy = HelperClassGeneric.getDoubleValueFromString(sdr["ProdEff"].ToString());
                        data.NonProductionEffy = HelperClassGeneric.getDoubleValueFromString(sdr["NonProdEff"].ToString());
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        #endregion

        #region ----Operator Effy Report ----
        public static List<OperatorEffyReportEntity> getOperatorEffyReport(string fromDate, string toDate, string operatorId, string shift)
        {
            List<OperatorEffyReportEntity> list = new List<OperatorEffyReportEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetOperatorEfficiencyReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                if (shift.Equals("All", StringComparison.OrdinalIgnoreCase) || shift.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@ShiftName", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ShiftName", shift);
                }
                cmd.Parameters.AddWithValue("@Operator", operatorId);
                cmd.Parameters.AddWithValue("@param", "");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {

                    while (sdr.Read())
                    {
                        DateTime pDate = Convert.ToDateTime(sdr["PDate"].ToString());
                        OperatorEffyReportEntity data = new OperatorEffyReportEntity();
                        data.Pdate = Convert.ToDateTime(sdr["PDate"].ToString());
                        data.Shift = sdr["Shift"].ToString();
                        data.Machine = sdr["Machine"].ToString();
                        data.ProdTime = sdr["ProdTime"].ToString();
                        data.DwnTime = sdr["Downtime"].ToString();
                        data.Others = sdr["Others"].ToString();
                        data.AE = sdr["AE"].ToString();
                        data.PE = sdr["PE"].ToString();
                        data.OE = sdr["OE"].ToString();
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        #endregion

        #region ---- Unmaned Report -----------
        public static List<UnmanedReportEntity> getUnmanedReportSummary(string fromDate, string toDate, string plantID, string machineID, string shift)
        {
            List<UnmanedReportEntity> list = new List<UnmanedReportEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetShiftwiseMachineUsageReport", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shiftname", shift);
                cmd.Parameters.AddWithValue("@MachineID", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@Plantid", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@param", "summary");
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {

                    while (sdr.Read())
                    {
                        UnmanedReportEntity data = new UnmanedReportEntity();
                        data.MachineID = sdr["Machineid"].ToString();
                        data.UtilisedTime = sdr["Utilisedtime"].ToString();
                        data.DownTime = sdr["Downtime"].ToString();
                        data.NoOfComponents = sdr["Components"].ToString();
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        public static List<UnmanedReportEntity> getUnmanedReportDetails(string fromDate, string toDate, string plantID, string machineID, string shift)
        {
            List<UnmanedReportEntity> list = new List<UnmanedReportEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            string shiftTime = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetShiftwiseMachineUsageReport", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shiftname", shift);
                cmd.Parameters.AddWithValue("@MachineID", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@Plantid", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@param", "");
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {

                    while (sdr.Read())
                    {
                        UnmanedReportEntity data = new UnmanedReportEntity();
                        data.Date = sdr["PDate"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["PDate"]).ToString("dd-MMM-yyyy");
                        data.MachineID = sdr["Machineid"].ToString();
                        data.Component = sdr["Component"].ToString();
                        data.Operation = sdr["Operation"].ToString();
                        data.Operator = sdr["Operator"].ToString();
                        data.BatchStart = sdr["BatchStart"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["BatchStart"]).ToString("HH:mm");
                        data.BatchEnd = sdr["BatchEnd"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["BatchEnd"]).ToString("HH:mm");
                        data.NoOfComponents = sdr["Components"].ToString();
                        data.UtilisedTime = sdr["Utilisedtime"].ToString();
                        data.DownTime = sdr["Downtime"].ToString();
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        #endregion
    }
}