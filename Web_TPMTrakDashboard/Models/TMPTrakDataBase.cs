using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Elmah;
using System.Threading.Tasks;
using TPM_TRAK_AnalyticsWebReports;
using ModelClassLibrary;
using Xceed.Document.NET;
using System.Configuration;

namespace Web_TPMTrakDashboard.Models
{
    public class TMPTrakDataBase
    {

        #region

        public static DataTable DantalProductionReport(string PlantID, string CellID, string MachineID, DateTime StartDate, DateTime EndDate, out DataTable dt2)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            dt2 = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetShiftwiseProductionReportFromAutodata", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@Param", SqlDbType.NVarChar).Value = "DantalProductionReport";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
                dt2.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }

            return values;
        }
        #endregion
        #region "Production report-Machinewise Shift-Format- 1"
        public static DataTable AnalysisMachinewiseShiftFormat1Report(DateTime StartDate, string ShiftIn, string CellID, string MachineID, string ComponentID, string OperationNo, string PlantID, DateTime EndDate, string Param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetShiftwiseProductionReportFromAutodata", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@ShiftIn", SqlDbType.NVarChar).Value = ShiftIn;
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@Param", SqlDbType.NVarChar).Value = Param;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }

            return values;
        }
        #endregion

        #region "Get Shift Name"
        public static DataTable GetShiftIDsandNames()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT ShiftName, ShiftID FROM ShiftDetails WHERE Running=1 ORDER BY ShiftID");
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 600;
                cmd.Connection = sqlConn;
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(dr);
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetShiftNames: " + ex.StackTrace);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        internal static string GetOperationNoByComp(string componentId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string op = string.Empty;
            try
            {
                SqlCommand cmd = null;
                cmd = new SqlCommand(@"select DISTINCT operationno from componentoperationpricing where componentId in (" + componentId + ") order by operationno", sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    op = rdr["operationno"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return op;

        }

        #endregion

        #region "Get Hourly Machine Production Report"
        public static SqlDataReader GetHourlyMachinewiseProduction(string strtTime, string endTime, string PlantID, string MachineID, string Shiftname, string cellID, out SqlConnection sqlConn)
        {

            sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand(@"[s_GetNSPL_Reports]", sqlConn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", strtTime);
            cmd.Parameters.AddWithValue("@EndDate", endTime);
            cmd.Parameters.AddWithValue("@Machine", MachineID);
            cmd.Parameters.AddWithValue("@PlantID", PlantID);
            cmd.Parameters.AddWithValue("@ShiftName", Shiftname);
            cmd.Parameters.AddWithValue("@ComparisonParam", "Shift");
            cmd.Parameters.AddWithValue("@GroupID", cellID);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                //if (sqlConn != null) sqlConn.Close();
            }
            return dr;
        }
        #endregion

        #region "Get Production Efficency"
        internal static SqlDataReader GetProductionEfficiency(string strtTime, string endTime, string plantid, string MachineId, string comparisonParam, string timeAxis, string shiftName, string type, string cellID, out SqlConnection sqlConn)
        {
            sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand(@"[s_GetEfficiencyFromAutodata]", sqlConn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartTime", strtTime);
            cmd.Parameters.AddWithValue("@EndTime", endTime);
            cmd.Parameters.AddWithValue("@MachineID", MachineId);
            cmd.Parameters.AddWithValue("@PlantID", plantid);
            cmd.Parameters.AddWithValue("@ComparisonParam", comparisonParam);
            cmd.Parameters.AddWithValue("@TimeAxis", timeAxis);
            cmd.Parameters.AddWithValue("@ShiftName", shiftName);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@GroupID", cellID);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                //if (sqlConn != null) sqlConn.Close();
            }
            return dr;
        }

        #endregion

        #region "Get Mando Data"
        internal static SqlDataReader GetMandoReport(string strtTime, string endTime, string plantid, string MachineId, string shiftID, string sheetNo, string format, string cellId, out SqlConnection sqlConn)
        {
            sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand(@"[s_GetMando_Reports]", sqlConn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            cmd.Parameters.AddWithValue("@StartDate", strtTime);
            cmd.Parameters.AddWithValue("@EndDate", endTime);
            cmd.Parameters.AddWithValue("@Machine", MachineId);
            cmd.Parameters.AddWithValue("@Plant", plantid);
            cmd.Parameters.AddWithValue("@ShiftID", shiftID);
            cmd.Parameters.AddWithValue("@SheetNo", sheetNo);
            cmd.Parameters.AddWithValue("@Format", format);
            cmd.Parameters.AddWithValue("@GroupID", cellId);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = null;
            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                //if (sqlConn != null) sqlConn.Close();
            }
            return dr;
        }
        #endregion

        #region "Production report-Machinewise Daily-Format-1 EXCEL"
        public static DataTable AnalysisMachinewiseDailyFormat1Report(DateTime StartDate, string MachineID, string ComponentID, string OperationNo, string PlantID, string CellID, DateTime EndDate, string param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetDailyProductionReportFromAutodata", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@param", SqlDbType.NVarChar).Value = param;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Production report-Machinewise Shift-Format- 3"
        public static DataTable AnalysisMachinewiseShiftFormat3Report(DateTime StartDate, string MachineID, string ShiftId, string PlantID, DateTime EndDate, string parm)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetNSPL_Reports", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@Machine", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@ShiftName", SqlDbType.NVarChar).Value = ShiftId;
            cmd.Parameters.Add("@ComparisonParam", SqlDbType.NVarChar).Value = parm;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Production report-Machinewise Shift-OEE Graphical report"
        public static DataTable AnalysisMachinewiseShiftOEEGraphicalReport(DateTime StartDate, string MachineID, string ShiftId, string PlantID, DateTime EndDate, string ComparisonParam, string TimeAxis, string TypeValye, string daywise, string cell)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetEfficiencyFromAutodata_Pragathi", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@ShiftName", SqlDbType.NVarChar).Value = ShiftId;
            cmd.Parameters.Add("@ComparisonParam", SqlDbType.NVarChar).Value = ComparisonParam;
            cmd.Parameters.Add("@TimeAxis", SqlDbType.NVarChar).Value = TimeAxis;
            cmd.Parameters.Add("@Type", SqlDbType.NVarChar).Value = TypeValye;
            cmd.Parameters.Add("@daywise", SqlDbType.NVarChar).Value = daywise;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = cell;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Bind Componentwise report---------------"
        public static DataTable ComponentwiseReport(DateTime StartDate, string ComponentID, string OperationNo, DateTime EndDate)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetComponentProductionReportFromAutoData", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Daily Prod and Down Log by day-Excel-----------------"
        public static DataTable DailyProdandDownLogbyDayReport(DateTime StartDate, string PlantId, string MachineID, DateTime EndDate, string RptProd_down)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_Get_Auma_Prod_Downreport", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantId;
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@Enddate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@RptProd_down", SqlDbType.NVarChar).Value = RptProd_down;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
                if (sdr != null) sdr.Close();
            }
            return values;
        }
        #endregion

        #region "Production and down time report- machinewise"
        public static DataTable ProductionandDownTimeReport_Machinewise(DateTime StartDate, string ShiftIn, string PlantId, string MachineID, DateTime EndDate, string ComponentID, string OperationNo, string Param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetShiftwiseProductionReportFromAutodata", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@ShiftIn", SqlDbType.NVarChar).Value = ShiftIn;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantId;
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@Param", SqlDbType.NVarChar).Value = Param;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }

        public static DataTable ProductionandDownTimeReport_Machinewise(DateTime StartDate, string MachineID, DateTime EndDate)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetCockpitDownData_eshopx", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "----------------Bind Component Information---------------"
        internal static List<string> GetComponentInformation()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> lstCompInfo = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select DISTINCT componentId  from componentinformation", sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lstCompInfo.Add(rdr["componentId"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstCompInfo;
        }


        internal static void DeleteCreateSchedule(string ReportName, string Exportname, string PlantID, string Operator, string Machine, string Shift, string RunReportForEvery)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = @"Delete from ScheduledReports where ReportName=@ReportName and ExportFileName=@ExportFileName and Machine=@Machine and Shift=@Shift and Operator=@Operator and PlantID=@PlantID and RunReportForEvery=@RunReportForEvery";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@ReportName", ReportName);
                cmd.Parameters.AddWithValue("@ExportFileName", Exportname);
                cmd.Parameters.AddWithValue("@Machine", Machine = Machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Machine);
                cmd.Parameters.AddWithValue("@Shift", Shift = Shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Shift);
                cmd.Parameters.AddWithValue("@Operator", Operator = Operator.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operator);
                cmd.Parameters.AddWithValue("@PlantID", PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@RunReportForEvery", RunReportForEvery);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static List<string> GetOperationNo(string componentId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> lstCompInfo = new List<string>();
            try
            {
                SqlCommand cmd = null;
                if (componentId.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand(@"select DISTINCT operationno from componentoperationpricing order by operationno", sqlConn);
                }
                else if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand(@"select DISTINCT operationno from componentoperationpricing where componentId in (" + componentId + ") order by operationno", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"select DISTINCT operationno from componentoperationpricing where componentId=@componentId order by operationno", sqlConn);
                    cmd.Parameters.AddWithValue("@componentId", componentId);
                }
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lstCompInfo.Add(rdr["operationno"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstCompInfo;
        }
        #endregion

        #region "Bind DownId Details"
        public static List<string> GetDownIdInfo(bool Show16Downs)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> lstCompInfo = new List<string>();
            try
            {
                string query = "";
                if (Show16Downs)
                    query = @"select distinct downId from downcodeinformation where downId not in (select DownId from PredefinedDownCodeInfo) ";
                else
                    query = @"select distinct downId from downcodeinformation ";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lstCompInfo.Add(rdr["downId"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstCompInfo;
        }
        #endregion

        #region "Down time report to Machine wise down time details"
        public static DataTable MachineWiseDownTimeDetails(DateTime StartDate, string MachineID, DateTime EndDate, string DownID, string PlantID, int Exclude, string cellID)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = null;
            switch (ConfigurationManager.AppSettings["IndiaNippon"].ToString())
            {
                case "0":
                    cmd = new SqlCommand("s_GetDailyBreakDownReport", Con);
                    break;
                case "1":
                    cmd = new SqlCommand("S_GetShiftWiseDowntimeReport_Nippon", Con);
                    break;
            }
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@DownID", SqlDbType.NVarChar).Value = DownID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@Exclude", SqlDbType.NVarChar).Value = Exclude;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = cellID;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Down time report to Machine wise down time details Agg."
        public static DataTable MachineWiseDownTimeDetailsAgg(DateTime StartDate, string MachineID, DateTime EndDate, string DownID, string PlantID, int Exclude, string cellID)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = null;
            cmd = new SqlCommand("s_GetShiftAgg_BreakdownReport", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@DownID", SqlDbType.NVarChar).Value = DownID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@Exclude", SqlDbType.NVarChar).Value = Exclude;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = cellID;
            cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar).Value = "AggBreakDownReport";
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "SM_Machinewise Prod Report From AutoData"
        public static DataTable SM_MachinewiseProdReportFromAutoData(DateTime StartDate, DateTime EndDate, string MachineID, string PlantID, string CellID)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetCockpitData_WithTempTable_eshopx", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Down time report for Machine down time matrix "
        public static DataTable MachineDownTimeMatrix(DateTime StartDate, DateTime EndDate, string MachineID, string PlantID, string DownID, int Exclude, string MatrixType, string cellId, string proc, string Type)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand(proc, Con);
            cmd.CommandTimeout = 600;
            if (proc.Equals("s_GetDownTimeMatrixfromAutoData", StringComparison.OrdinalIgnoreCase) || proc.Equals("s_GetSONA_BreakDownMatrix", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (proc.Equals("s_GetSONA_ShiftAgg_DowntimeMatrix", StringComparison.OrdinalIgnoreCase) || proc.Equals("s_GetSONA_AggBreakDownMatrix", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd");
            }
            //cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            //cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            if (proc.Equals("s_GetSONA_BreakDownMatrix", StringComparison.OrdinalIgnoreCase) || proc.Equals("s_GetSONA_AggBreakDownMatrix", StringComparison.OrdinalIgnoreCase))
            {
                if (Type.Equals("ByPhenomenon", StringComparison.OrdinalIgnoreCase))
                    cmd.Parameters.AddWithValue("@BrkDownID", DownID);
                else if (Type.Equals("ByCategory", StringComparison.OrdinalIgnoreCase))
                    cmd.Parameters.AddWithValue("@BrkDownCategory", DownID);
            }
            else
                cmd.Parameters.AddWithValue("@DownID", DownID);
            if (proc.Equals("s_GetSONA_ShiftAgg_DowntimeMatrix", StringComparison.OrdinalIgnoreCase))
                cmd.Parameters.AddWithValue("@Exclude", Exclude);
            else
                cmd.Parameters.AddWithValue("@Excludedown", Exclude);
            cmd.Parameters.AddWithValue("@MatrixType", MatrixType);
            cmd.Parameters.AddWithValue("@Groupid", cellId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellId);
            //cmd.Parameters.AddWithValue("@OperatorID", "");
            //cmd.Parameters.AddWithValue("@ComponentID", "");
            //cmd.Parameters.AddWithValue("@MachineIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@OperatorIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@DownIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@ComponentIDLabel", "ALL");             
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Down time report for Machine down time matrix "
        public static DataTable MachineDownTimeMatrixForConfidental(DateTime StartDate, DateTime EndDate, string MachineID, string PlantID, string DownID, int Exclude, string MatrixType, string cellId, string proc, string Type, string operatorID, string param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("[dbo].[s_GetShiftAgg_BreakdownReport]", Con);
            cmd.CommandTimeout = 600;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar).Value = param;
            cmd.Parameters.Add("@Exclude", SqlDbType.NVarChar).Value = 0;
            cmd.Parameters.Add("@OperatorID", SqlDbType.NVarChar).Value = operatorID;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        #region "Down Time Wise & Down Time Freq Asyn Data"
        internal static async Task<DataTable> GetDownTimeAndFreqData(DateTime StartDate, DateTime EndDate, string MachineID, string PlantID, string DownID, int Exclude, string MatrixType, string cellId)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetDownTimeMatrixfromAutoData", Con);
            cmd.CommandTimeout = 600;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.AddWithValue("@DownID", DownID);
            cmd.Parameters.AddWithValue("@Excludedown", Exclude);
            cmd.Parameters.AddWithValue("@MatrixType", MatrixType);
            cmd.Parameters.AddWithValue("@Groupid", cellId);
            //cmd.Parameters.AddWithValue("@OperatorID", "");
            //cmd.Parameters.AddWithValue("@ComponentID", "");
            //cmd.Parameters.AddWithValue("@MachineIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@OperatorIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@DownIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@ComponentIDLabel", "ALL");             
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = await cmd.ExecuteReaderAsync();
                values.Load(sdr);
                values.AcceptChanges();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }

        internal static void CopyData(InspectionMasterData inputtext, out bool status)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            status = false;
            try
            {
                string query = @"SP_MachineLevelSPCMasterReplication_Shanthi";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SourceMachine", inputtext.MachineID);
                cmd.Parameters.AddWithValue("@SourceComponent", inputtext.ComponentID);
                cmd.Parameters.AddWithValue("@SourceOperation", inputtext.OperationID);
                cmd.Parameters.AddWithValue("@TargetMachine", inputtext.SelectedMachineID);
                cmd.CommandType = CommandType.StoredProcedure;
                int cnt = cmd.ExecuteNonQuery();
                if (cnt > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While saving Inspection data details" + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        internal static void CopyComponentData(InspectionMasterData inputtext, out bool status)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            status = false;
            try
            {
                string query = @"SP_ComponentLevelSPCMasterReplication_KTA";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SourceComponent", inputtext.ComponentID);
                cmd.Parameters.AddWithValue("@SourceOperation", inputtext.OperationID);
                cmd.Parameters.AddWithValue("@TargetComponent", inputtext.SelectedComponentID);
                cmd.CommandType = CommandType.StoredProcedure;
                int cnt = cmd.ExecuteNonQuery();
                if (cnt > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While saving Inspection data details" + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
        }
        internal static async Task<DataTable> GetDownTimeAndFreqDataAggregate(DateTime StartDate, DateTime EndDate, string MachineID, string PlantID, string DownID, int Exclude, string MatrixType, string cellId)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetSONA_ShiftAgg_DowntimeMatrix", Con);
            cmd.CommandTimeout = 600;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.AddWithValue("@DownID", DownID);
            //cmd.Parameters.AddWithValue("@Excludedown", Exclude);
            cmd.Parameters.AddWithValue("@MatrixType", MatrixType);
            cmd.Parameters.AddWithValue("@Groupid", cellId);
            //cmd.Parameters.AddWithValue("@OperatorID", "");
            //cmd.Parameters.AddWithValue("@ComponentID", "");
            //cmd.Parameters.AddWithValue("@MachineIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@OperatorIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@DownIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@ComponentIDLabel", "ALL");             
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = await cmd.ExecuteReaderAsync();
                values.Load(sdr);
                values.AcceptChanges();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        internal static DataTable GetProductionDownPieDownTimeChartData(DateTime startDate, string plantId, string machineId, DateTime endDate, string downID, int exclude, string matrixType, string cellID)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetDownTimeMatrixfromAutoData", Con);
            try
            {
                cmd.CommandTimeout = 360;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = machineId;
                cmd.Parameters.AddWithValue("@DownID", downID);
                cmd.Parameters.AddWithValue("@OperatorID", "");
                cmd.Parameters.AddWithValue("@ComponentID", "");
                cmd.Parameters.AddWithValue("@MachineIDLabel", "");
                cmd.Parameters.AddWithValue("@OperatorIDLabel", "");
                cmd.Parameters.AddWithValue("@DownIDLabel", "");
                cmd.Parameters.AddWithValue("@ComponentIDLabel", "");
                cmd.Parameters.AddWithValue("@MatrixType", matrixType);
                cmd.Parameters.AddWithValue("@PlantID", "");
                cmd.Parameters.AddWithValue("@Excludedown", exclude);
                cmd.Parameters.AddWithValue("@Groupid", cellID);
                SqlDataReader sdr = null;
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
                values.AcceptChanges();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }

        internal static DataTable GetMachineWiseProdData(DateTime startDate, DateTime endDate, string machineId, string plantId, string cellID)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetCockpitData_WithTempTable_eshopx", Con);
            try
            {
                cmd.CommandTimeout = 360;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = machineId;
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                SqlDataReader sdr = null;
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
                values.AcceptChanges();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion

        internal static void Inserttomailserver(string EmailMethod, string server, string Port)
        {
            string sqlQry = string.Empty;
            SqlConnection Con = ConnectionManager.GetConnection();
            try
            {
                sqlQry = @"if exists(select valueintext from shopdefaults where parameter = 'ScheduledReports_Email')
				 Begin
					   Update shopdefaults set ValueInText = @EmailMethod, ValueInText2 = @Server,ValueInInt = @valueinint,UpdatedTS=@UpdatedTS where parameter = 'ScheduledReports_Email'
				 End
				 Else
				 Begin
					   insert into shopdefaults(ValueInText, ValueInText2,ValueInInt, parameter,UpdatedTS) values(@EmailMethod, @Server,@valueinint, 'ScheduledReports_Email',@UpdatedTS)
				 End";

                SqlCommand cmd = new SqlCommand(sqlQry, Con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@EmailMethod", EmailMethod);
                cmd.Parameters.AddWithValue("@Server", server);
                cmd.Parameters.AddWithValue("@valueinint", Port);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
        }

        internal static void Inserttomail(string Email, string Password)
        {
            string sqlQry = string.Empty;
            SqlConnection Con = ConnectionManager.GetConnection();
            try
            {
                sqlQry = @"if exists(select valueintext from shopdefaults where parameter = 'ScheduledReports_MailServerDomain')
				 Begin
					  Update shopdefaults set ValueInText = @MailID,ValueInText2 = @Password,UpdatedTS=@UpdatedTS where parameter = 'ScheduledReports_MailServerDomain'
				 End
				 Else
				 Begin
					   insert into shopdefaults(ValueInText, ValueInText2, parameter,UpdatedTS) values(@MailID, @Password, 'ScheduledReports_MailServerDomain',@UpdatedTS)
				 End";

                SqlCommand cmd = new SqlCommand(sqlQry, Con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MailID", Email);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
        }

        internal static void Inserttomail(string date)
        {

            string sqlQry = string.Empty;
            SqlConnection Con = ConnectionManager.GetConnection();
            try
            {
                sqlQry = @"if exists(select valueintext from shopdefaults where parameter = 'ScheduledReports_LastRunforTheDay')
				 Begin
					   Update shopdefaults set ValueInText = @Date,UpdatedTS=@UpdatedTS where parameter = 'ScheduledReports_LastRunforTheDay'
				 End
				 Else
				 Begin
					   insert into shopdefaults(ValueInText, parameter,UpdatedTS) values(@Date, 'ScheduledReports_LastRunforTheDay',@UpdatedTS)
				 End ";

                SqlCommand cmd = new SqlCommand(sqlQry, Con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
        }

        internal static void InsertEmailSettings(string emailMethod, string scheduledReports_LastRunforTheDay, string server, string port, string fromEmailID, string password)
        {
            string sqlQry = string.Empty;
            SqlConnection Con = ConnectionManager.GetConnection();
            try
            {
                sqlQry = @"if exists(select valueintext from shopdefaults where parameter = 'ScheduledReports_Email')
				 Begin
					   Update shopdefaults set ValueInText = @EmailMethod, ValueInText2 = @Server,ValueInInt = @valueinint,UpdatedTS=@UpdatedTS where parameter = 'ScheduledReports_Email'
				 End
				 Else
				 Begin
					   insert into shopdefaults(ValueInText, ValueInText2,ValueInInt, parameter,UpdatedTS) values(@EmailMethod, @Server,@valueinint, 'ScheduledReports_Email',@UpdatedTS)
				 End               
				 if exists(select valueintext from shopdefaults where parameter = 'ScheduledReports_LastRunforTheDay')
				 Begin
					   Update shopdefaults set ValueInText = @Date,UpdatedTS=@UpdatedTS where parameter = 'ScheduledReports_LastRunforTheDay'
				 End
				 Else
				 Begin
					   insert into shopdefaults(ValueInText, parameter,UpdatedTS) values(@Date, 'ScheduledReports_LastRunforTheDay',@UpdatedTS)
				 End
                
				 if exists(select valueintext from shopdefaults where parameter = 'ScheduledReports_MailServerDomain')
				 Begin
					  Update shopdefaults set ValueInText = @MailID,ValueInText2 = @Password,UpdatedTS=@UpdatedTS where parameter = 'ScheduledReports_MailServerDomain'
				 End
				 Else
				 Begin
					   insert into shopdefaults(ValueInText, ValueInText2, parameter,UpdatedTS) values(@MailID, @Password, 'ScheduledReports_MailServerDomain',@UpdatedTS)
				 End";

                SqlCommand cmd = new SqlCommand(sqlQry, Con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@EmailMethod", emailMethod);
                cmd.Parameters.AddWithValue("@Server", server);
                cmd.Parameters.AddWithValue("@valueinint", port);
                cmd.Parameters.AddWithValue("@Date", scheduledReports_LastRunforTheDay);
                cmd.Parameters.AddWithValue("@MailID", fromEmailID);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
            }
        }

        internal static string GetEmailCredentials(out string server, out string port, out string emailID, out string password)
        {
            string emailMethod = string.Empty;
            server = port = emailID = password = string.Empty;
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from shopdefaults where parameter = 'ScheduledReports_MailServerDomain';
					                              select * from shopdefaults where parameter = 'ScheduledReports_Email'", Con);
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    emailID = sdr["ValueInText"].ToString();
                    password = sdr["ValueInText2"].ToString();

                }

                sdr.NextResult();

                if (sdr.Read())
                {
                    emailMethod = sdr["ValueInText"].ToString();
                    server = sdr["ValueInText2"].ToString();
                    port = sdr["ValueInInt"].ToString();
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (Con != null) Con.Close();
            }
            return emailMethod;
        }

        internal static List<DTOShcheduleReport> GetDataForcmbReports()
        {
            List<DTOShcheduleReport> list = new List<DTOShcheduleReport>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DTOShcheduleReport vals = null;

            try
            {
                cmd = new SqlCommand(@"Select * from [CreateSchDuleTemp1]", conn);
                cmd.CommandType = System.Data.CommandType.Text;

                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    vals = new DTOShcheduleReport();
                    vals.Reports = sdr["Reports"].ToString();
                    vals.ExportType = sdr["ExportsType"].ToString();
                    //vals.PlantID = sdr["PlantID"].ToString();
                    //vals.Machine = sdr["Machine"].ToString();
                    //vals.Operator = sdr["Operator"].ToString();
                    vals.RunreportOn = sdr["RunReportOn"].ToString();
                    //vals.ShiftId = sdr["ShiftId"].ToString();
                    vals.ShiftIdAll = string.IsNullOrEmpty(sdr["ShiftIDAll"].ToString()) == true ? true : Convert.ToBoolean(sdr["ShiftIDAll"].ToString());
                    vals.PlantIDAll = string.IsNullOrEmpty(sdr["PlantIDAll"].ToString()) == true ? true : Convert.ToBoolean(sdr["PlantIDAll"].ToString());
                    vals.OperatorAll = string.IsNullOrEmpty(sdr["OperatorIDAll"].ToString()) == true ? true : Convert.ToBoolean(sdr["OperatorIDAll"].ToString());
                    vals.MachineAll = string.IsNullOrEmpty(sdr["MachineIDAll"].ToString()) == true ? true : Convert.ToBoolean(sdr["MachineIDAll"].ToString());
                    vals.GroupIDAll = string.IsNullOrEmpty(sdr["GroupIDAll"].ToString()) == true ? true : Convert.ToBoolean(sdr["GroupIDAll"].ToString());
                    vals.RunReportOnEvery = sdr["RunReportForEvery"].ToString();
                    if (sdr["RunReportforEveryVisibility"] != DBNull.Value)
                        vals.RunReportforEveryVisibility = string.IsNullOrEmpty(sdr["RunReportforEveryVisibility"].ToString()) == true ? true : Convert.ToBoolean(Convert.ToInt32(sdr["RunReportforEveryVisibility"]));
                    vals.RunReportOnVisibity = string.IsNullOrEmpty(sdr["RunReportOnVisibity"].ToString()) == true ? true : Convert.ToBoolean(Convert.ToInt32(sdr["RunReportOnVisibity"].ToString()));
                    vals.ReportTemplate = sdr["TemplateName"].ToString();
                    list.Add(vals);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return list;
        }

        internal static void InsertOrUpdateScheduleData(string reportName, string TemplateName, string cmbExportType, string cmbMachineId, string CmbOperatorId, string cmbPlantId, string cmbLineId, string CmbReportForEveryDay, string Subject, int Reportid, string cmbReports, string cmbShiftId, string txtExport, bool ChkEmail, string lstAddCC, string lstAddBCC, string lstAddTo, string slno, out bool SusccessFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SusccessFailure = false;
            try
            {
                string query = @"if exists(select * from ScheduledReports where ReportName=@ReportName and ReportFileName=@ReportFileName and Machine=@Machine and Shift=@Shift and Operator=@Operator and PlantID=@PlantID and GroupID=@LineId and RunReportForEvery=@RunReportForEvery)
                  Begin
                        update ScheduledReports set ReportName=@ReportName, ReportFileName=@ReportFileName,ExportType=@ExportType,ExportFileName=@ExportFileName,ExportPath=@ExportPath,DayBefores=@DayBefores,[Machine]=@Machine,Shift=@Shift,Operator=@Operator,PlantID=@PlantID,GroupID=@LineId,Email_Flag=@Email_Flag,Email_List_TO=@Email_List_TO,Email_List_CC=@Email_List_CC,Email_List_BCC=@Email_List_BCC,RunReportForEvery=@RunReportForEvery,Subject=@Subject where ReportFileName=@ReportFileName and [Machine]=@Machine and Shift=@Shift and Operator=@Operator and PlantID=@PlantID
                  End
                    Else
                  Begin
                        insert into ScheduledReports (ReportName,ReportFileName,ExportType,ExportFileName,ExportPath, DayBefores,[Machine],Shift,Operator,PlantID,GroupID,Email_Flag,Email_List_TO,Email_List_CC,Email_List_BCC,RunReportForEvery,Subject) values           (@ReportName,@ReportFileName,@ExportType,@ExportFileName,@ExportPath,@DayBefores,@Machine,@Shift,@Operator,@PlantID,@LineId,@Email_Flag,@Email_List_TO,@Email_List_CC,@Email_List_BCC,@RunReportForEvery,@Subject ) 
                  End";

                cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ReportName", cmbReports);
                cmd.Parameters.AddWithValue("@ReportFileName", TemplateName);
                cmbExportType = cmbExportType.Equals("Excel", StringComparison.OrdinalIgnoreCase) ? "0" : "4";
                cmd.Parameters.AddWithValue("@ExportType", cmbExportType);
                cmd.Parameters.AddWithValue("@ExportFileName", reportName);
                cmd.Parameters.AddWithValue("@ExportPath", txtExport);
                cmd.Parameters.AddWithValue("@DayBefores", Reportid);
                cmd.Parameters.AddWithValue("@Machine", cmbMachineId);
                cmd.Parameters.AddWithValue("@Shift", cmbShiftId);
                cmd.Parameters.AddWithValue("@Operator", CmbOperatorId);
                cmd.Parameters.AddWithValue("@PlantID", cmbPlantId);
                cmd.Parameters.AddWithValue("@LineId", cmbLineId);
                cmd.Parameters.AddWithValue("@Email_Flag", ChkEmail);
                cmd.Parameters.AddWithValue("@Email_List_TO", lstAddTo.TrimEnd(';', ','));
                cmd.Parameters.AddWithValue("@Email_List_CC", lstAddCC.TrimEnd(';', ','));
                cmd.Parameters.AddWithValue("@Email_List_BCC", lstAddBCC.TrimEnd(';', ','));
                cmd.Parameters.AddWithValue("@RunReportForEvery", CmbReportForEveryDay);
                cmd.Parameters.AddWithValue("@Subject", Subject);
                cmd.Parameters.AddWithValue("@slno", slno);
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Close();
                SusccessFailure = true;
            }
            catch (Exception ex)
            {
                SusccessFailure = false;
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }

        }

        internal static DataTable AllDataOfScheduleReport()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select * from ScheduledReports", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
                foreach (DataRow dtrow in dt.Rows)
                {
                    if (dtrow["ExportType"].ToString() == "0")
                        dtrow["ExportType"] = "Excel";
                    else
                        dtrow["ExportType"] = "PDF";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }

            return dt;
        }

        internal static List<ToolChangefrequency> ToolChangeFreqData(DateTime fromDate, DateTime toDate, string machineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<ToolChangefrequency> ToolChangeFreqList = new List<ToolChangefrequency>();
            ToolChangefrequency ToolChangefreqData = null;
            try
            {
                cmd = new SqlCommand("s_GetToolChangeFrequency", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate);
                cmd.Parameters.AddWithValue("@EndTime", toDate);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@PlantID", "");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ToolChangefreqData = new ToolChangefrequency();
                        ToolChangefreqData.Tool = rdr["Inserts"].ToString();
                        ToolChangefreqData.NoOfChanges = Convert.ToInt32(rdr["NoOfInserts"].ToString());
                        ToolChangefreqData.Starttime = rdr["StartTime"].ToString();
                        ToolChangefreqData.Endtime = rdr["EndTime"].ToString();
                        ToolChangefreqData.Operation = rdr["Operation"].ToString();
                        ToolChangefreqData.cyclecount = Convert.ToInt32(rdr["NumberOfOperations"].ToString());
                        ToolChangeFreqList.Add(ToolChangefreqData);
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
                if (rdr != null) rdr.Close();
            }
            return ToolChangeFreqList;
        }

        internal static List<MOWISEREPORTENTITY> MOWisedata(DateTime fromDate, DateTime toDate, string machineID, string plantID, string MOID)
        {
            List<MOWISEREPORTENTITY> MoWiseDataList = new List<MOWISEREPORTENTITY>();
            MOWISEREPORTENTITY MowiseData = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("S_GetMODetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Starttime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Endtime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machineid", machineID == "ALL" ? "" : machineID);
                cmd.Parameters.AddWithValue("@PlantID", plantID == "ALL" ? "" : plantID);
                cmd.Parameters.AddWithValue("@MO", MOID == "ALL" ? "" : MOID);
                cmd.Parameters.AddWithValue("@Param", "");
                cmd.CommandTimeout = 10000;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        MowiseData = new MOWISEREPORTENTITY();
                        if (!DBNull.Value.Equals(rdr["Sdate"]))
                            MowiseData.MOStartTime = rdr["Sdate"].ToString();
                        if (!DBNull.Value.Equals(rdr["Pdate"]))
                            MowiseData.MOEndTime = rdr["Pdate"].ToString();
                        MowiseData.CellNo = rdr["CellNo"].ToString();
                        MowiseData.MachineNO = rdr["McNo"].ToString();
                        MowiseData.MONumber = rdr["MONo"].ToString();
                        MowiseData.ItemCode = rdr["ItemCode"].ToString();
                        MowiseData.EmployeeName = rdr["EmployeeName"].ToString();
                        MowiseData.ActualCount = Convert.ToDouble(rdr["ActualCount"].ToString());
                        MowiseData.MOQty = Convert.ToInt32(rdr["MOQuantity"].ToString());
                        MowiseData.MOSettingtime = rdr["MOSettingTime"].ToString();
                        MowiseData.MORunningtime = rdr["MORunningTime"].ToString();
                        MowiseData.TotalCycletime = rdr["TotalCycleTime"].ToString();
                        MowiseData.Actualtimeconsumedforproduction = rdr["ActualTime"].ToString();
                        MowiseData.Remarks1 = rdr["Remarks1"].ToString();
                        MoWiseDataList.Add(MowiseData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return MoWiseDataList;
        }

        internal static DataTable GetsonaBFWdata(DateTime fromDate, DateTime toDate, string plantID, string machineID, string Proc)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(Proc, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (Proc.Equals("s_GetDailyProductionReport_SONA", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (Proc.Equals("s_GetAgg_DailyProductionReport_SONA", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                }

                cmd.Parameters.AddWithValue("@PlantID", plantID == "ALL" ? "" : plantID);
                cmd.Parameters.AddWithValue("@MachineID", machineID == "ALL" ? "" : machineID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
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
            return dt;
        }

        internal static DataTable GetmangalDowntime(DateTime fromDate, DateTime toDate, out DataTable toprows, out DataTable bottomrows, out DataTable lastcolumn)
        {
            DataTable data = new DataTable();
            toprows = new DataTable();
            bottomrows = new DataTable();
            lastcolumn = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("S_GetForgingProdAnalysisReport_Mangal", conn);
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    toprows.Load(rdr);
                    toprows.AcceptChanges();
                    bottomrows.Load(rdr);
                    bottomrows.AcceptChanges();
                    data.Load(rdr);
                    data.AcceptChanges();
                    lastcolumn.Load(rdr);
                    lastcolumn.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return data;
        }

        internal static DataTable GetProductionDowntimebaluauto(DateTime fromDate, DateTime toDate, string plantId, string machineId, string shift, out DataTable downdata)
        {
            DataTable data = new DataTable();
            downdata = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_GetShiftwiseProdReportFromAutodata_BaluAuto", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate);
                cmd.Parameters.AddWithValue("@EndDate", toDate);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@ShiftIn", shift);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    downdata.Load(rdr);
                    downdata.AcceptChanges();
                    data.Load(rdr);
                    data.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return data;
        }

        internal static DataTable GetSonaMisReport(string fromdate, string todate, string machineID, string plantID, string proc, out DataTable downTable)
        {
            downTable = new DataTable();
            DataTable shiftwiseData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(proc, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    downTable.Load(rdr);
                    downTable.AcceptChanges();
                    shiftwiseData.Load(rdr);
                    shiftwiseData.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return shiftwiseData;
        }

        internal static DataTable GetHytechdata(DateTime fromDate, string machineID, string plantID, out DataTable downids)
        {
            downids = new DataTable();
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_GetDownTimeGapAnalysis_Hytech", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dDate", fromDate.ToString("yyyy-MM-01"));
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();

                downids.Load(rdr);
                downids.AcceptChanges();
                dt.Load(rdr);
                dt.AcceptChanges();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }

        internal static async Task<DataTable> GetBreakDownTimeAndFreqData(DateTime StartDate, DateTime EndDate, string MachineID, string PlantID, string DownID, int Exclude, string MatrixType, string cellId, string Type)
        {
            //if (!string.IsNullOrEmpty(DownID))
            //	DownID = Util.splitfunction(DownID);
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetSONA_BreakDownMatrix", Con);
            cmd.CommandTimeout = 600;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            if (Type.Equals("ByPhenomenon", StringComparison.OrdinalIgnoreCase))
                cmd.Parameters.AddWithValue("@BrkDownID", DownID);
            else if (Type.Equals("ByCategory", StringComparison.OrdinalIgnoreCase))
                cmd.Parameters.AddWithValue("@BrkDownCategory", DownID);
            cmd.Parameters.AddWithValue("@Excludedown", Exclude);
            cmd.Parameters.AddWithValue("@MatrixType", MatrixType);
            cmd.Parameters.AddWithValue("@Groupid", cellId);
            //cmd.Parameters.AddWithValue("@OperatorID", "");
            //cmd.Parameters.AddWithValue("@ComponentID", "");
            //cmd.Parameters.AddWithValue("@MachineIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@OperatorIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@DownIDLabel", "ALL");
            //cmd.Parameters.AddWithValue("@ComponentIDLabel", "ALL");             
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = null;
            try
            {
                sdr = await cmd.ExecuteReaderAsync();
                values.Load(sdr);
                values.AcceptChanges();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }

        internal static List<string> GetBreakDownIdInfo(string Type, string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> lstCompInfo = new List<string>();
            string Query = "";
            if (Type.Equals("BreakdownID", StringComparison.OrdinalIgnoreCase))
                Query = @"select distinct BreakDownCode from SONA_BreakDownMasterInfo";
            else if (Type.Equals("Subsystem", StringComparison.OrdinalIgnoreCase))
            {
                if (!(string.IsNullOrEmpty(MachineID)))
                    Query = @"select DISTINCT BreakDownCategory from SONA_BreakDownMasterInfo LEFT join SONA_AssetMasterInfo on				
							SONA_BreakDownMasterInfo.BreakDownCategoryinterface = SONA_AssetMasterInfo.SubSystemID and
							SONA_AssetMasterInfo.MachineId = @MachineId order by BreakDownCategory";
                else
                    Query = @"select DISTINCT BreakDownCategory from SONA_BreakDownMasterInfo Inner join SONA_AssetMasterInfo on	
							SONA_BreakDownMasterInfo.BreakDownCategoryinterface = SONA_AssetMasterInfo.SubSystemID order by BreakDownCategory";
            }

            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);
                if (Type.Equals("Subsystem", StringComparison.OrdinalIgnoreCase) && !(string.IsNullOrEmpty(MachineID)))
                    cmd.Parameters.AddWithValue("@MachineId", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (Type.Equals("BreakdownID", StringComparison.OrdinalIgnoreCase))
                        lstCompInfo.Add(rdr["BreakDownCode"].ToString());
                    else if (Type.Equals("Subsystem", StringComparison.OrdinalIgnoreCase))
                        lstCompInfo.Add(rdr["BreakDownCategory"].ToString());

                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstCompInfo;
        }

        internal static DataTable GetPMTProdAndDownReportData(DateTime fromDate, out DataTable dtDownInformation)
        {
            DataTable dtProdAndDown = new DataTable();
            dtDownInformation = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_PMT_GetProductionAndDownReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", "");
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@Groupid", "");
                cmd.Parameters.AddWithValue("@Parameter", "");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtDownInformation.Load(rdr);
                    dtDownInformation.AcceptChanges();
                    dtProdAndDown.Load(rdr);
                    dtProdAndDown.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return dtProdAndDown;
        }

        internal static DataTable GetOperatorEfficiencyInfo(DateTime fromDate, string param)
        {
            DataTable dtGroupWiseOprEfficiency = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_PMT_GetGroupwiseOperatorEfficiency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", "");
                cmd.Parameters.AddWithValue("@Groupid", "");
                cmd.Parameters.AddWithValue("@Parameter", "");
                cmd.Parameters.AddWithValue("@OEEParam", param);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtGroupWiseOprEfficiency.Load(rdr);
                    dtGroupWiseOprEfficiency.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return dtGroupWiseOprEfficiency;
        }

        internal static DataTable GetMonthWiseEfficiencyData(DateTime fromDate)
        {
            DataTable dtMonthWiseEfficiency = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_PMT_GetMonthwiseEfficiency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", "");
                cmd.Parameters.AddWithValue("@Groupid", "");
                cmd.Parameters.AddWithValue("@Parameter", "");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtMonthWiseEfficiency.Load(rdr);
                    dtMonthWiseEfficiency.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return dtMonthWiseEfficiency;
        }

        internal static void GetBreakdownSona(DateTime fromDate, DateTime toDate, string plantID, string machineID, string subSystem, string timeFormat, string Type, out DataTable dt)
        {
            dt = new DataTable();
            Type = Type == "Daily" ? "Day" : Type;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_GetSONA_AggBreakDownPhenomenonReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@BrkDownCategory", subSystem);
                cmd.Parameters.AddWithValue("@DownTimeFormat", timeFormat);
                cmd.Parameters.AddWithValue("@ReportType", Type);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr != null)
                    dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
        }

        internal static string GetEmailForEmailSettings()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string email = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select valueintext from shopdefaults where parameter='ScheduledReports_MailServerDomain'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    email = rdr["valueintext"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return email;

        }

        internal static string GetPortForEmailSettings(string mailid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string port = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select valueinint from shopdefaults where parameter ='ScheduledReports_Email'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@emailid", mailid);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    port = rdr["valueinint"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return port;

        }

        internal static string GetServerForEmailSettings(string mailid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string server = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select valueintext2 from shopdefaults where valueintext =@emailid and Parameter='ScheduledReports_Email'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@emailid", mailid);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    server = rdr["valueintext2"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return server;
        }

        internal static string GetPasswordForEmailSettings()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string password = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select valueintext2 from shopdefaults where parameter='ScheduledReports_MailServerDomain'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    password = rdr["valueintext2"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return password;
        }

        internal static string getReleasingDate()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string shcduledDate = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select valueintext from shopdefaults where parameter='ScheduledReports_LastRunforTheDay'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    DateTime xyz = Convert.ToDateTime(rdr["valueintext"]);
                    shcduledDate = xyz.ToString("yyyy-MMM-dd");
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return shcduledDate;
        }

        internal static DataTable GetProdReportData(string fromdate, string todate, string shift, string plantId, string machineId, string Param, string cellID)
        {
            DataTable ProdReportData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                Logger.WriteDebugLog("s_GetShiftAgg_ProductionReport proc Execution Start :- " + DateTime.Now.ToString());
                SqlCommand cmd = new SqlCommand("s_GetShiftAgg_ProductionReport", conn);
                //SqlCommand cmd = new SqlCommand("s_GetShiftAgg_ProductionReport_OG", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@ShiftName", shift);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@OperatorID", "");
                cmd.Parameters.AddWithValue("@ReportType", "Machinewise");
                cmd.Parameters.AddWithValue("@Parameter", Param);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    ProdReportData.Load(rdr);
                    ProdReportData.AcceptChanges();
                }
                Logger.WriteDebugLog("s_GetShiftAgg_ProductionReport proc Execution End :- " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return ProdReportData;
        }

        internal static DataTable GetDantalProdReportData_Agg(string fromdate, string todate, string plantId, string cellID, string machineID, out DataTable dt2)//plant - ""
        {
            DataTable DantalProdReportData = new DataTable();
            dt2 = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                Logger.WriteDebugLog("s_GetShiftAgg_ProductionReport proc Execution Start :- " + DateTime.Now.ToString());
                SqlCommand cmd = new SqlCommand("S_GetShiftAgg_ProductionReport_Dantal", conn);
                //SqlCommand cmd = new SqlCommand("s_GetShiftAgg_ProductionReport_OG", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromdate);
                cmd.Parameters.AddWithValue("@ToDate", todate);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                // cmd.Parameters.AddWithValue("@OperatorID", "");
                //cmd.Parameters.AddWithValue("@ReportType", "Machinewise");
                //cmd.Parameters.AddWithValue("@Parameter", "");

                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    DantalProdReportData.Load(rdr);
                    dt2.Load(rdr);
                    DantalProdReportData.AcceptChanges();
                }
                Logger.WriteDebugLog("GetDantalProdReportData_Agg proc Execution End :- " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return DantalProdReportData;
        }

        internal static DataTable GetProdReportDataWithSummary(string fromdate, string todate, string shift, string plantId, string machineId, string Param, string cellID, out DataTable dtSummary)
        {
            DataTable ProdReportData = new DataTable();
            dtSummary = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                Logger.WriteDebugLog("s_GetShiftAgg_ProductionReport proc Execution Start :- " + DateTime.Now.ToString());
                SqlCommand cmd = new SqlCommand("s_GetShiftAgg_ProductionReport", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@ShiftName", shift);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@OperatorID", "");
                cmd.Parameters.AddWithValue("@ReportType", "Machinewise");
                cmd.Parameters.AddWithValue("@Parameter", Param);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    ProdReportData.Load(rdr);
                    ProdReportData.AcceptChanges();
                    dtSummary.Load(rdr);
                }
                Logger.WriteDebugLog("s_GetShiftAgg_ProductionReport proc Execution End :- " + DateTime.Now.ToString());
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
            return ProdReportData;
        }
        internal static DataTable GetProdReportSummaryDataForMachineAll(string fromdate, string todate, string shift, string plantId, string machineId)
        {
            DataTable ProdReportData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetShiftAgg_ProductionReport", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@ShiftName", shift);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@ReportType", "Plantwise");
                cmd.Parameters.AddWithValue("@Parameter", "SONA_AggCockpit");
                cmd.Parameters.AddWithValue("@GroupID", "");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    ProdReportData.Load(rdr);
                    ProdReportData.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return ProdReportData;
        }

        internal static DataTable GetProdDailyConfidentalReportData(string fromdate, string todate, string shift, string plantId, string machineId, string Param)
        {
            DataTable ProdReportData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[s_GetShiftAgg_ComparisonReports]", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@ShiftName", shift);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@ComparisonType", Param);
                cmd.Parameters.AddWithValue("@Parameter", "ProdReport");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    ProdReportData.Load(rdr);
                    ProdReportData.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return ProdReportData;
        }

        internal static DataTable GetAggregatedComparisonData(string fromdate, string todate, string plantID, string machineID, string type, string cellID)
        {
            DataTable AggregatedComparisonData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                Logger.WriteDebugLog("s_GetShiftAgg_ComparisonReports proc execution start : " + DateTime.Now.ToString());
                SqlCommand cmd = new SqlCommand("s_GetShiftAgg_ComparisonReports", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@ShiftName", "");
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@DownReason", "");
                cmd.Parameters.AddWithValue("@RejectionReason", "");
                cmd.Parameters.AddWithValue("@ComparisonType", type);
                cmd.Parameters.AddWithValue("@Parameter", "");
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    AggregatedComparisonData.Load(rdr);
                    AggregatedComparisonData.AcceptChanges();
                }
                Logger.WriteDebugLog("s_GetShiftAgg_ComparisonReports proc execution end : " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return AggregatedComparisonData;
        }

        internal static Dictionary<string, EfficiencyEntity> GetMachinewiseEfficiencyDetails(List<string> machineIDList, string startDate)
        {
            string sqlQuery = string.Empty;
            EfficiencyEntity efficiencyData = null;
            Dictionary<string, EfficiencyEntity> machineEfficiencyDetails = new Dictionary<string, EfficiencyEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                if (machineIDList != null && machineIDList.Count > 0)
                {
                    foreach (string machine in machineIDList)
                    {
                        efficiencyData = new EfficiencyEntity();
                        //sqlQuery = @"select isnull(AE,0)AE, isnull(PE,0)PE, isnull(QE,0)QE, isnull(OE,0)OE from Efficiencytarget where TargetLevel = @TargetLevel and MachineID = @MachineID and datepart(month,startdate)=datepart(month,@StartDate)";
                        sqlQuery = @"select isnull(AE,0)AE, isnull(PE,0)PE, isnull(QE,0)QE, isnull(OE,0)OE from Efficiencytarget where TargetLevel = @TargetLevel and MachineID = @MachineID and datepart(month,startdate)=datepart(month,@StartDate)  and datepart(YEAR,startdate)=datepart(YEAR,@StartDate)";
                        SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TargetLevel", "Month");
                        cmd.Parameters.AddWithValue("@MachineID", machine);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            if (rdr.Read())
                            {
                                efficiencyData.AE = Convert.ToDouble(rdr["AE"]);
                                efficiencyData.PE = Convert.ToDouble(rdr["PE"]);
                                efficiencyData.QE = Convert.ToDouble(rdr["QE"]);
                                efficiencyData.OE = Convert.ToDouble(rdr["OE"]);
                            }
                        }
                        else
                        {
                            efficiencyData.AE = 0;
                            efficiencyData.PE = 0;
                            efficiencyData.QE = 0;
                            efficiencyData.OE = 0;
                        }
                        machineEfficiencyDetails.Add(machine, efficiencyData);
                        if (rdr != null) rdr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error while getting machinewise efficiency details - GetMachinewiseEfficiencyDetails()" + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return machineEfficiencyDetails;
        }
        internal static List<EfficiencyEntity> GetMachinewiseMonthEfficiencyDetails(string fromDate, string toDate)
        {
            string sqlQuery = string.Empty;
            EfficiencyEntity efficiencyData = null;
            List<EfficiencyEntity> machineEfficiencyDetails = new List<EfficiencyEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                //if (machineIDList != null && machineIDList.Count > 0)
                //{
                //foreach (string machine in machineIDList)
                //{

                //                sqlQuery = @"select isnull(AE,0)AE, isnull(PE,0)PE, isnull(QE,0)QE, isnull(OE,0)OE,startdate,machineid from Efficiencytarget 
                //where TargetLevel = @TargetLevel  and (datepart(month,startdate)>=datepart(month,@StartDate)
                //and datepart(month,Enddate)<=datepart(month,@EndDate)) and datepart(year,startdate)=datepart(year,@StartDate) order by machineid,startdate";
                sqlQuery = @"select isnull(AE,0)AE, isnull(PE,0)PE, isnull(QE,0)QE, isnull(OE,0)OE,startdate,machineid from Efficiencytarget 
where TargetLevel = @TargetLevel  and startdate>=@StartDate
and Enddate<=@EndDate  order by machineid,startdate";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TargetLevel", "Month");
                //cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@StartDate", fromDate);
                cmd.Parameters.AddWithValue("@EndDate", toDate);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        efficiencyData = new EfficiencyEntity();
                        efficiencyData.MachineID = rdr["machineid"].ToString();
                        efficiencyData.StartDate = Util.GetDateTime(rdr["startdate"].ToString());
                        efficiencyData.AE = Convert.ToDouble(rdr["AE"]);
                        efficiencyData.PE = Convert.ToDouble(rdr["PE"]);
                        efficiencyData.QE = Convert.ToDouble(rdr["QE"]);
                        efficiencyData.OE = Convert.ToDouble(rdr["OE"]);
                        machineEfficiencyDetails.Add(efficiencyData);
                    }
                }
                if (rdr != null) rdr.Close();
                // }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error while getting machinewise month efficiency details - GetMachinewiseMonthEfficiencyDetails()" + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return machineEfficiencyDetails;
        }
        internal static DataTable GetFlowMeterReportData(DateTime fromDate, DateTime toDate, string plantID, string machineID, string shift)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"[S_GetBoschFlowMeterReport]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantId", plantID);
                cmd.Parameters.AddWithValue("@Mc", machineID);
                cmd.Parameters.AddWithValue("@ShiftName", shift.Equals("All") ? "" : shift);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving Machine - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        #region -----Get Daily Rejection Report Data--
        internal static DataTable GetDailyRejectionReportData(DateTime fromDate, DateTime toDate, string plantID, string machineID, string ComponentID, string cellID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"[dbo].[s_GetRejectionCodeDetails]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plantID = plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@machineID", machineID = machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@ComponentID", ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                cmd.Parameters.AddWithValue("@param", "");
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving Machine - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        #endregion

        internal static List<RejectAnalysisEntity> GetRejectionAnalysisMachineData(DateTime fromDate, DateTime toDate, string PlantID, string MachineID, string ComponentID, string OperationID, string Category, string RejectionID, string cellID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            List<RejectAnalysisEntity> listData = new List<RejectAnalysisEntity>();
            RejectAnalysisEntity data;
            try
            {
                cmd = new SqlCommand(@"[s_GetShiftAgg_RejectionAnalysisReport]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate);
                cmd.Parameters.AddWithValue("@EndTime", toDate);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@COmponentID", ComponentID);
                cmd.Parameters.AddWithValue("@OperationNo", OperationID);
                cmd.Parameters.AddWithValue("@RejCategory", Category);
                cmd.Parameters.AddWithValue("@RejectionID", RejectionID);
                cmd.Parameters.AddWithValue("@Reptype", "Machinewise");
                cmd.Parameters.AddWithValue("@Exclude", 0);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data = new RejectAnalysisEntity();
                        data.MachineID = reader["Machineid"].ToString();
                        data.RejectionDesc = reader["Rejectiondesc"].ToString();
                        data.RejQty = Convert.ToInt32(reader["RejQty"].ToString());
                        data.RRejQty = Convert.ToInt32(reader["RRejQty"].ToString());
                        data.RMRejQty = Convert.ToInt32(reader["RMRejQty"].ToString());
                        data.McRejQty = Convert.ToInt32(reader["McRejQty"].ToString());
                        data.TotalRej = Convert.ToInt32(reader["TotalRej"].ToString());
                        listData.Add(data);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving Rejection Analysis Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return listData;
        }
        internal static List<RejectAnalysisEntity> GetRejectionAnalysisComponentData(DateTime fromDate, DateTime toDate, string PlantID, string MachineID, string ComponentID, string OperationID, string Category, string RejectionID, string cellID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            List<RejectAnalysisEntity> listData = new List<RejectAnalysisEntity>();
            RejectAnalysisEntity data;
            try
            {
                cmd = new SqlCommand(@"[s_GetShiftAgg_RejectionAnalysisReport]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate);
                cmd.Parameters.AddWithValue("@EndTime", toDate);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@COmponentID", ComponentID);
                cmd.Parameters.AddWithValue("@OperationNo", OperationID);
                cmd.Parameters.AddWithValue("@RejCategory", Category);
                cmd.Parameters.AddWithValue("@RejectionID", RejectionID);
                cmd.Parameters.AddWithValue("@Reptype", "Componentwise");
                cmd.Parameters.AddWithValue("@Exclude", 0);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data = new RejectAnalysisEntity();
                        data.RejectionDesc = reader["Rejectiondesc"].ToString();
                        data.ComponentID = reader["Componentid"].ToString();
                        data.OperationNo = reader["OperationNo"].ToString();
                        data.RejQty = Convert.ToInt32(reader["RQty"].ToString());
                        data.RRejQty = Convert.ToInt32(reader["RejRQty"].ToString());
                        data.CoRejQty = Convert.ToInt32(reader["CORejQty"].ToString());
                        data.SCoRejQty = Convert.ToInt32(reader["SCoRejqty"].ToString());
                        data.TotalRej = Convert.ToInt32(reader["TOtRej"].ToString());
                        listData.Add(data);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving Rejection Analysis Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return listData;
        }

        internal static DataTable GetHelpRequestReportAverageMatrix(DateTime fromDate, DateTime toDate, string shift, string plantId, string machineId, string helpReqType)
        {
            DataTable helpRequestReportDetails = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            DateTime tempDate = DateTime.MinValue;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("S_readRequest_Ack_ResetDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@starttime", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.AddWithValue("@endtime", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.AddWithValue("@Shift", SqlDbType.NVarChar).Value = shift;
                cmd.Parameters.AddWithValue("@PlantID", SqlDbType.NVarChar).Value = plantId;
                cmd.Parameters.AddWithValue("@Machineid", SqlDbType.NVarChar).Value = machineId;
                cmd.Parameters.AddWithValue("@calltype", SqlDbType.NVarChar).Value = helpReqType;
                cmd.Parameters.AddWithValue("@param", SqlDbType.NVarChar).Value = "AvgCalltype";
                cmd.CommandTimeout = 600;
                sdr = cmd.ExecuteReader();
                helpRequestReportDetails.Load(sdr);
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
            return helpRequestReportDetails;
        }

        internal static DataTable GetHelpRequestReportData(DateTime fromDate, DateTime toDate, string shift, string plantId, string machineId, string helpReqType)
        {
            DataTable helpRequestReportDetails = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            DateTime tempDate = DateTime.MinValue;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("S_readRequest_Ack_ResetDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@starttime", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.AddWithValue("@endtime", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.AddWithValue("@Shift", SqlDbType.NVarChar).Value = shift;
                cmd.Parameters.AddWithValue("@PlantID", SqlDbType.NVarChar).Value = plantId;
                cmd.Parameters.AddWithValue("@Machineid", SqlDbType.NVarChar).Value = machineId;
                cmd.Parameters.AddWithValue("@calltype", SqlDbType.NVarChar).Value = helpReqType;
                cmd.Parameters.AddWithValue("@param", SqlDbType.NVarChar).Value = string.Empty;
                cmd.CommandTimeout = 600;
                sdr = cmd.ExecuteReader();
                helpRequestReportDetails.Load(sdr);
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
            return helpRequestReportDetails;
        }



        #region "Bind Group by Average Tools Life"
        public static string ToolDescription(string groupId)
        {
            string toolDesc = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"Select ToolDescription from ToolSequence where toolno=@GroupId", sqlConn);
                cmd.Parameters.AddWithValue("@GroupId", groupId);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    toolDesc = rdr["ToolDescription"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return toolDesc;
        }
        #endregion

        internal static void DeleteCreateSchedule(string ReportName, string Exportname, string PlantID, string LineID, string Operator, string Machine, string Shift, string RunReportForEvery)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = @"Delete from ScheduledReports where ReportName=@ReportName and ExportFileName=@ExportFileName and Machine=@Machine and Shift=@Shift and Operator=@Operator and PlantID=@PlantID and GroupID=@LineID and RunReportForEvery=@RunReportForEvery";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@ReportName", ReportName);
                cmd.Parameters.AddWithValue("@ExportFileName", Exportname);
                cmd.Parameters.AddWithValue("@Machine", Machine = Machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Machine);
                cmd.Parameters.AddWithValue("@Shift", Shift = Shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Shift);
                cmd.Parameters.AddWithValue("@Operator", Operator = Operator.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operator);
                cmd.Parameters.AddWithValue("@PlantID", PlantID = PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@RunReportForEvery", RunReportForEvery);
                cmd.Parameters.AddWithValue("@LineID", LineID = LineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : LineID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        #region DB_CycleAnalysis_Raksha

        internal static void GetCycAnalysis(DateTime selDate, string machineID, string compID, string opID, string selView, out DataTable dt)
        {
            dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("S_Get_MachineCycleTimeHistoryReport_PatelBrass", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", selDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@EndDate", "");
                cmd.Parameters.AddWithValue("@Machine", machineID);
                cmd.Parameters.AddWithValue("@Componenet", compID);
                cmd.Parameters.AddWithValue("@Operation", opID);
                cmd.Parameters.AddWithValue("@Param", selView);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr != null)
                    dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
        }

        internal static void GetCycAnalysisDayWise(DateTime fromDate, DateTime toDate, string machineID, string compID, string opID, string selView, out DataTable dt, out DataTable dt1, out DataTable dt2)
        {
            dt = new DataTable();
            dt1 = new DataTable();
            dt2 = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("S_GetShiftwiseMachineCycleTimeHistoryReport_PatelBrass", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machine", machineID);
                cmd.Parameters.AddWithValue("@Componenet", compID);
                cmd.Parameters.AddWithValue("@Operation", opID);
                cmd.Parameters.AddWithValue("@Param", selView);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();

                dt.Load(rdr);
                dt1.Load(rdr);
                dt2.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
        }
        #endregion

        internal static void GetCycAnalysisVariant(DateTime fromDate, DateTime toDate, string machineID, string compID, string opID, out DataTable dt)
        {
            dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("S_GetShiftwiseMachineCycleTimeHistoryReport_PatelBrass", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machine", machineID);
                cmd.Parameters.AddWithValue("@Componenet", compID);
                cmd.Parameters.AddWithValue("@Operation", opID);
                cmd.Parameters.AddWithValue("@Param", "CycleTime");
                cmd.Parameters.AddWithValue("@View", "Variant");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr != null)
                    dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
        }

        internal static DataTable GeHourlyPartCount(DateTime fromDate, string machineID, string shift, out DataTable dtDownCode, out DataTable dtDownData)
        {
            Logger.WriteDebugLog("Inside hourly partCount databasecall");
            DataTable dt = new DataTable();
            dtDownCode = new DataTable();
            dtDownData = new DataTable();

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_GetMCOOHourlyReport_PatelBrass", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machine", machineID);
                cmd.Parameters.AddWithValue("@ShiftName", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                Logger.WriteDebugLog("Inside commandexecuted");


                dtDownCode.Load(rdr);
                dt.Load(rdr);
                dtDownData.Load(rdr);
                Logger.WriteDebugLog("DatatableLoaded");
                //}
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

        #region "Inspection Master Data"
        #region ------Bind MachineID To Display----------
        public static List<string> GetMachineIdList()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstMachineID = new List<string>();
            try
            {
                cmd = new SqlCommand("Select Distinct MachineID from MachineInformation where TPMTrakEnabled=1", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstMachineID.Add(sdr["MachineID"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstMachineID;
        }
        #endregion

        #region ------Bind ComponentID To Display----------
        public static List<string> GetComponentIdList()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstComponentID = new List<string>();
            try
            {
                cmd = new SqlCommand("select Distinct ComponentID from componentinformation ", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstComponentID.Add(sdr["ComponentID"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstComponentID;
        }
        #endregion

        #region ------Bind OperationID To Display----------
        public static List<string> GetOperationIdList(string componentid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstOperationID = new List<string>();
            string query = "";
            componentid = componentid == "ComponentAll" ? "" : componentid;
            query = string.IsNullOrEmpty(componentid) ? "Select Distinct operationno from componentoperationpricing" : "Select Distinct operationno from componentoperationpricing where componentid = @componentid";
            try
            {
                cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@componentid", componentid);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstOperationID.Add(sdr["operationno"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstOperationID;
        }
        #endregion

        internal static List<InspectionMasterData> GetInspectionMasterData(string machineID, string CompID, string OprID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<InspectionMasterData> list = new List<InspectionMasterData>();
            InspectionMasterData masterData = null;
            string QueryString = string.Empty;
            try
            {
                if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1") || ConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1"))
                {
                    QueryString = @"Select * FROM SPC_Characteristic where (MachineID = @MachineID or isnull(@MachineID, '') = '') and (ComponentID = @ComponentID or isnull(@ComponentID, '') = '')  and (OperationNo = @OperationID or isnull(@OperationID, '') = '') order by SortOrder";
                }
                else
                {
                    QueryString = @"Select * FROM SPC_Characteristic where (MachineID = @MachineID or isnull(@MachineID, '') = '') and (ComponentID = @ComponentID or isnull(@ComponentID, '') = '')  and (OperationNo = @OperationID or isnull(@OperationID, '') = '')";
                }
                cmd = new SqlCommand(QueryString, con);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ComponentID", CompID);
                cmd.Parameters.AddWithValue("@OperationID", OprID);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            masterData = new InspectionMasterData();
                            masterData.MachineID = sdr["MachineID"].ToString();
                            masterData.ComponentID = sdr["ComponentID"].ToString();
                            masterData.OperationID = sdr["OperationNo"].ToString();
                            masterData.CharCode = sdr["CharacteristicCode"].ToString();
                            masterData.CharID = sdr["CharacteristicID"].ToString();
                            masterData.specification = sdr["SpecificationMean"].ToString();
                            masterData.LSL = sdr["LSL"].ToString();
                            masterData.USL = sdr["USL"].ToString();
                            masterData.UOM = sdr["UOM"].ToString();
                            if (sdr["Datatype"].ToString().Equals(DBNull.Value) || string.IsNullOrEmpty(sdr["Datatype"].ToString()))
                                masterData.DataTemplate = "";
                            else
                                masterData.DataTemplate = sdr["Datatype"].ToString();
                            masterData.UpperOperatingZone = sdr["UpperOperatingZoneLimit "].ToString();
                            masterData.LowerOperatingZone = sdr["LowerOperatingZoneLimit "].ToString();
                            masterData.UpperWarningZone = sdr["UpperWarningZoneLimit "].ToString();
                            masterData.SampleSize = sdr["SampleSize"].ToString();
                            masterData.LowerWarningZone = sdr["LowerWarningZoneLimit "].ToString();
                            masterData.InspectedBy = sdr["InspectedBy"].ToString();
                            masterData.IsEnabled = string.IsNullOrEmpty(sdr["IsEnabled"].ToString()) == true ? true : Convert.ToBoolean(sdr["IsEnabled"].ToString());
                            if (ConfigurationManager.AppSettings["MivinPages"].Equals("1"))
                            {
                                masterData.InputMethod = sdr["InputMethod"].ToString();
                                masterData.Channel = sdr["Channel"].ToString();
                            }
                            else
                            {
                                masterData.InputMethod = "";
                                masterData.Channel = "";
                            }
                            if (ConfigurationManager.AppSettings["KTASpindlePages"].Equals("1") || ConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1"))
                            {
                                masterData.SortOrder = sdr["SortOrder"].ToString();
                            }

                            list.Add(masterData);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteDebugLog(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While getting InspectionMaster details" + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        internal static void DeleteInspectionData(string machineID, string CompID, string OprID, string charid, string charcode, out string successFailure)
        {
            successFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string QueryString = string.Empty;
            if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1") || ConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1"))
            {
                QueryString = @"delete from SPC_Characteristic where ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID= @CharID";
            }
            else
            {
                QueryString = @"delete from SPC_Characteristic where  MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID= @CharID";
            }
            try
            {
                cmd = new SqlCommand(QueryString, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ComponentID", CompID);
                cmd.Parameters.AddWithValue("@OperationID", OprID);
                cmd.Parameters.AddWithValue("@CharID", charid);
                cmd.Parameters.AddWithValue("@CharCode", charcode);
                cmd.ExecuteNonQuery();
                successFailure = "Successfull";
            }
            catch (Exception ex)
            {
                successFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void InsertUpdateData(InspectionMasterData inputtext)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            try
            {
                DateTime now = DateTime.Now;

                if (ConfigurationManager.AppSettings["MivinPages"].ToString().Equals("1"))
                {
                    query = @"if not exists (select * from SPC_Characteristic where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID=@CharID)
                                            begin
                                            insert into SPC_Characteristic(CharacteristicID, CharacteristicCode,MachineID,ComponentID,OperationNo,SpecificationMean,LSL,USL,UOM,Datatype,InspectedBy,IsEnabled,UpperOperatingZoneLimit,LowerOperatingZoneLimit,UpperWarningZoneLimit,LowerWarningZoneLimit,SampleSize,InputMethod,Channel,UpdatedTS) values (@CharID, @CharCode, @MachineID, @ComponentID, @OperationID, @Specification,@LSL,@USL,@UOM,@DataTemplate,@InspectedBy,@IsEnabled,@UpperOperatingZoneLimit,@LowerOperatingZoneLimit,@UpperWarningZoneLimit,@LowerWarningZoneLimit,@SampleSize,@InputMethod,@Channel,@UpdatedTS)
                                            end 
                                            else
                                            begin
                                            update SPC_Characteristic set CharacteristicCode=@CharCode,SpecificationMean=@Specification, LSL=@LSL, SampleSize=@SampleSize, UpperOperatingZoneLimit=@UpperOperatingZoneLimit, LowerOperatingZoneLimit=@LowerOperatingZoneLimit, UpperWarningZoneLimit=@UpperWarningZoneLimit, LowerWarningZoneLimit=@LowerWarningZoneLimit, USL=@USL, UOM=@UOM, Datatype=@DataTemplate, InspectedBy=@InspectedBy, IsEnabled=@IsEnabled ,InputMethod=@InputMethod , Channel=@Channel, UpdatedTS=@UpdatedTS where MachineID=@MachineID and  ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID=@CharID
                                            end";
                }
                else if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1") || ConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1"))
                {
                    query = @"if not exists (select * from SPC_Characteristic where ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID=@CharID)
                            begin
                            insert into SPC_Characteristic(CharacteristicID, CharacteristicCode,MachineID,ComponentID,OperationNo,SpecificationMean,LSL,USL,UOM,Datatype,InspectedBy,IsEnabled,UpperOperatingZoneLimit,LowerOperatingZoneLimit,UpperWarningZoneLimit,LowerWarningZoneLimit,SampleSize,SortOrder,UpdatedTS) values (@CharID, @CharCode, @MachineID, @ComponentID, @OperationID, @Specification,@LSL,@USL,@UOM,@DataTemplate,@InspectedBy,@IsEnabled,@UpperOperatingZoneLimit,@LowerOperatingZoneLimit,@UpperWarningZoneLimit,@LowerWarningZoneLimit,@SampleSize,@SortOrder,@UpdatedTS)
                            end 
                            else
                            begin
                            update SPC_Characteristic set CharacteristicCode=@CharCode,SpecificationMean=@Specification, LSL=@LSL, SampleSize=@SampleSize, UpperOperatingZoneLimit=@UpperOperatingZoneLimit, LowerOperatingZoneLimit=@LowerOperatingZoneLimit, UpperWarningZoneLimit=@UpperWarningZoneLimit, LowerWarningZoneLimit=@LowerWarningZoneLimit, USL=@USL, UOM=@UOM, Datatype=@DataTemplate, InspectedBy=@InspectedBy, IsEnabled=@IsEnabled,SortOrder=@SortOrder,UpdatedTS=@UpdatedTS where ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID=@CharID
                            end";
                }
                else
                {
                    query = @"if not exists (select * from SPC_Characteristic where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID=@CharID)
                            begin
                            insert into SPC_Characteristic(CharacteristicID, CharacteristicCode,MachineID,ComponentID,OperationNo,SpecificationMean,LSL,USL,UOM,Datatype,InspectedBy,IsEnabled,UpperOperatingZoneLimit,LowerOperatingZoneLimit,UpperWarningZoneLimit,LowerWarningZoneLimit,SampleSize,UpdatedTS) values (@CharID, @CharCode, @MachineID, @ComponentID, @OperationID, @Specification,@LSL,@USL,@UOM,@DataTemplate,@InspectedBy,@IsEnabled,@UpperOperatingZoneLimit,@LowerOperatingZoneLimit,@UpperWarningZoneLimit,@LowerWarningZoneLimit,@SampleSize,@UpdatedTS)
                            end 
                            else
                            begin
                            update SPC_Characteristic set SpecificationMean=@Specification, LSL=@LSL, SampleSize=@SampleSize, UpperOperatingZoneLimit=@UpperOperatingZoneLimit, LowerOperatingZoneLimit=@LowerOperatingZoneLimit, UpperWarningZoneLimit=@UpperWarningZoneLimit, LowerWarningZoneLimit=@LowerWarningZoneLimit, USL=@USL, UOM=@UOM, Datatype=@DataTemplate, InspectedBy=@InspectedBy, IsEnabled=@IsEnabled,UpdatedTS=@UpdatedTS where MachineID=@MachineID and  ComponentID=@ComponentID and OperationNo=@OperationID and CharacteristicID=@CharID
                            end";
                }
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MachineID", inputtext.MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", inputtext.ComponentID);
                cmd.Parameters.AddWithValue("@OperationID", inputtext.OperationID);
                cmd.Parameters.AddWithValue("@CharID", inputtext.CharID);
                cmd.Parameters.AddWithValue("@CharCode", inputtext.CharCode);
                cmd.Parameters.AddWithValue("@Specification", inputtext.specification);
                cmd.Parameters.AddWithValue("@LSL", inputtext.LSL);
                cmd.Parameters.AddWithValue("@USL", inputtext.USL);
                cmd.Parameters.AddWithValue("@UOM", inputtext.UOM);
                cmd.Parameters.AddWithValue("@DataTemplate", inputtext.DataTemplate);
                cmd.Parameters.AddWithValue("@InspectedBy", inputtext.InspectedBy);

                cmd.Parameters.AddWithValue("@UpperOperatingZoneLimit", inputtext.UpperOperatingZone);
                cmd.Parameters.AddWithValue("@UpperWarningZoneLimit", inputtext.UpperWarningZone);
                cmd.Parameters.AddWithValue("@LowerWarningZoneLimit", inputtext.LowerWarningZone);
                cmd.Parameters.AddWithValue("@LowerOperatingZoneLimit", inputtext.LowerOperatingZone);

                cmd.Parameters.AddWithValue("@SampleSize", inputtext.SampleSize);
                cmd.Parameters.AddWithValue("@IsEnabled", inputtext.IsEnabled);
                if (ConfigurationManager.AppSettings["MivinPages"].Equals("1"))
                {
                    cmd.Parameters.AddWithValue("@Channel", inputtext.Channel);
                    cmd.Parameters.AddWithValue("@InputMethod", inputtext.InputMethod);
                }
                if (ConfigurationManager.AppSettings["KTASpindlePages"].Equals("1") || ConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString().Equals("1"))
                {
                    cmd.Parameters.AddWithValue("@SortOrder", inputtext.SortOrder);
                }
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While saving Inspection data details" + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        internal static DataTable ComponentBatchwiseReport(DateTime StartDate, string ComponentID, string OperationNo, DateTime EndDate, out DataTable dtotal)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            dtotal = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetBatchWiseComponentProductionReportFromAutoData_Dantal", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
                dtotal.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }


        #endregion

        internal static DataTable GetSAPOEEData(DateTime fromDate, DateTime toDate, string machine, string shift, string plantID, string cell, out DataTable headerDowm)
        {
            DataTable SAPOEEDATA = new DataTable();
            headerDowm = new DataTable();
            SqlDataReader rdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"s_GetSAPIntegrationReport_Advik", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ShiftName", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@CellID", cell.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cell);
                //cmd.Parameters.AddWithValue("@Parameter", "");
                cmd.Parameters.AddWithValue("@Parameter", "Day");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                headerDowm.Load(rdr);
                headerDowm.AcceptChanges();
                SAPOEEDATA.Load(rdr);
                SAPOEEDATA.AcceptChanges();
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
            return SAPOEEDATA;
        }

        internal static DataTable GetOEEDataKiswok(DateTime fromDate, DateTime toDate, string shift, string machine, string plant, string cell)
        {
            DataTable dtOeeDataKiswok = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"s_GetAggOEEReport_Kiswok", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ShiftName", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@PlantID", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                cmd.Parameters.AddWithValue("@GroupID", cell.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cell);
                cmd.Parameters.AddWithValue("@Parameter", "View");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtOeeDataKiswok.Load(rdr);
                    dtOeeDataKiswok.AcceptChanges();
                }
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
            return dtOeeDataKiswok;
        }


        #region  -- KKPillar report---
        internal static DataTable GetKKPillar_Production_DownCodeList()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[S_Get_KKPillar_ProductionDown_Report]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "DownCodeList");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
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

        internal static DataTable GetKKPillar_ProductionDown_Details(DateTime fromDate, DateTime toDate, string plantID, string machineId, string shiftID)
        {
            DataTable dtProductionDetails = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[S_Get_KKPillar_ProductionDown_Report]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@startdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@enddate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@ShiftName", shiftID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shiftID);
                cmd.Parameters.AddWithValue("@param", "");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtProductionDetails.Load(rdr);
                    dtProductionDetails.AcceptChanges();
                }
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
            return dtProductionDetails;
        }

        #endregion

        internal static DataTable GetOperatorWiseData(string plantId, DateTime fromDate, DateTime toDate, string Operator, string Param)
        {
            DataTable dtOperatordetails = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"s_WeeklyOperatorProductionReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@startdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@enddate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                cmd.Parameters.AddWithValue("@Operator", Operator.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operator);
                cmd.Parameters.AddWithValue("@parameter", Param);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtOperatordetails.Load(rdr);
                    dtOperatordetails.AcceptChanges();
                }
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
            return dtOperatordetails;
        }

        internal static DataTable GetMonthWiseOEEData(string cell, DateTime fromDate, string machineId, string PlantID)
        {
            DataTable dtMonthWiseOEE = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"s_GetMonthwiseEfficiencyFromAutodata_Shanthi", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@GroupID", cell.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cell);
                cmd.Parameters.AddWithValue("@MachineID", machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtMonthWiseOEE.Load(rdr);
                    dtMonthWiseOEE.AcceptChanges();
                }
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
            return dtMonthWiseOEE;
        }

        internal static DataTable RejectionORReworkReportShantiFormat(string cell, DateTime fromDate, string machineId, string plantID, DateTime toDate, string Shift)
        {
            DataTable dtMonthWiseOEE = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[s_GetShiftwiseRejectionReworkDetails_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@ShiftIn", Shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Shift);
                cmd.Parameters.AddWithValue("@GroupID", cell.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cell);
                cmd.Parameters.AddWithValue("@MachineID", machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtMonthWiseOEE.Load(rdr);
                    dtMonthWiseOEE.AcceptChanges();
                }
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
            return dtMonthWiseOEE;
        }

        internal static DataTable PMReport(string cell, DateTime fromDate, string machineId, string plantId, DateTime toDate)
        {
            DataTable dtPMReport = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"S_GetPMTransactionReport_Shanti", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machine", machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtPMReport.Load(rdr);
                    dtPMReport.AcceptChanges();
                }
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
            return dtPMReport;
        }

        internal static DataTable GetShanthiProdReport(DateTime fromDate, string machineId, string plantId, DateTime toDate, string shift)
        {
            DataTable dtPMReport = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cd = new SqlCommand(@"[s_GetShiftwiseRejectionReworkDetails_Shanti]", conn);
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.CommandTimeout = 600;
                cd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cd.Parameters.AddWithValue("@ShiftIn", shift.Equals("All") ? "" : shift);
                cd.Parameters.AddWithValue("@PlantID", plantId);
                cd.Parameters.AddWithValue("@MachineID", machineId);
                cd.Parameters.AddWithValue("@Param", "");
                rdr = cd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtPMReport.Load(rdr);
                    dtPMReport.AcceptChanges();
                }
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
            return dtPMReport;
        }

        internal static DataTable GetDrawingNoDetails(DateTime fromDate, DateTime toDate, string opId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            try
            {
                if (opId != "")
                {
                    cmd = new SqlCommand(@"select distinct * from [dbo].[DrawingNoAuditDetails] where UpdatedBy in ( " + opId + " ) and UpdatedTS>= @StartTime and UpdatedTS<=@EndTime", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"select distinct * from [dbo].[DrawingNoAuditDetails] where UpdatedTS>= @StartTime and UpdatedTS<=@EndTime", sqlConn);
                }
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }

        internal static DataTable GetShiftwiseOperatorPerformanceData(DateTime fromDate, DateTime toDate, string shiftID, string plantID, string machineId, string Operator)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetDevendra_ShiftwiseMCOOLevelProdDownReport]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Shiftname", shiftID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shiftID);
                cmd.Parameters.AddWithValue("@Plantid", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@Operatorid", Operator.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operator);
                cmd.Parameters.AddWithValue("@param", "");
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }


        internal static DataTable GetShiftwiseOperatorPerformanceData_Seyoon_Agg(DateTime fromDate, DateTime toDate, string shiftID, string plantID, string machineId, string Operator)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetShiftAgg_OperatorPerformanceReport_seyoon]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Starttime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Endtime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Shift", shiftID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shiftID);
                cmd.Parameters.AddWithValue("@Plantid", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@Operatorid", Operator.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operator);
                //cmd.Parameters.AddWithValue("@param", "");
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetShiftwiseOperatorPerformanceData_Seyoon_Agg: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static string ReportParameter_OPE()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string textvalue = "";
            try
            {
                cmd = new SqlCommand(@"select ValueInText from ShopDefaults where Parameter='OperatorPerformanceReport_OE'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        textvalue = sdr["ValueInText"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return textvalue;
        }
        internal static DataTable GetPMReport(string startTime, string endTime, string machineId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GenerateShantiPMReport]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@Starttime", startTime);
                cmd.Parameters.AddWithValue("@Endtime", endTime);
                cmd.Parameters.AddWithValue("@MachineID", machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static Dictionary<string, List<string>> GetCatAndSubCat(string strMacName)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand(@"
IF NOT EXISTS(select Category, SubCategory from PM_Information where MachineType=(SELECT TOP 1 Description FROM machineinformation WHERE machineid=@machid))
SELECT Category, SubCategory from PM_Information where MachineType='GENERAL'
ELSE
SELECT Category, SubCategory from PM_Information where MachineType=(SELECT TOP 1 Description FROM machineinformation WHERE machineid=@machid)
", Con);
            cmd.Parameters.AddWithValue("@machid", strMacName);
            SqlDataReader rdr;
            Dictionary<string, List<string>> dct = new Dictionary<string, List<string>>();

            try
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string k = rdr["Category"].ToString();
                    string v = rdr["SubCategory"].ToString();
                    if (dct.ContainsKey(k))
                    {
                        dct[k].Add(v);
                    }
                    else
                    {
                        dct.Add(k, new List<string>(new[] { v }));
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null)
                {
                    Con.Close();
                }
            }
            return dct;
        }

        internal static DataTable GetProductionDateGEA(string plantID, string cellID, string machineID, DateTime fromDate, DateTime toDate, out DataTable ExtraDatatable)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            ExtraDatatable = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_GetAggProductionOrderReport_GEA]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@Starttime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Endtime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machineid", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@GroupID", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellID);
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                ExtraDatatable.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static DataTable GetHydroTestData(DateTime fromDate, DateTime toDate, string process, string machineId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_GetHydroTestReportDetails_Allied]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Process", process.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : process);
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static DataTable TimeConsolidatedShanthiFormat(DateTime fromDate, string machineId, string plantId, DateTime toDate, string cellID, string Type)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetCockpitData_WithTempTable_eshopx]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                cmd.Parameters.AddWithValue("@PlantID", plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                cmd.Parameters.AddWithValue("@GroupID", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellID);
                cmd.Parameters.AddWithValue("@param", Type);
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static DataTable GetMachineReworkDetails(DateTime fromDate, DateTime toDate, string machine)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_Get_ReworkReportDetails_MGTL]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@Param ", "Report");
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static DataTable GetMachineWiseScrapDetails(DateTime fromDate, DateTime toDate, string machine)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[SP_ScrapReportDetails_AmarRaj]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Machine", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetPMReport: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        #region ------Jagdev----
        internal static List<JagdevRejectionData> GetJagdevRejectionAnalysisMachineData(DateTime fromDate, DateTime toDate, string PlantID, string MachineID, string ComponentID, string OperationID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            List<JagdevRejectionData> listData = new List<JagdevRejectionData>();
            JagdevRejectionData data;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetRejectionReport_Jagadev]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate);
                cmd.Parameters.AddWithValue("@EndTime", toDate);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", ComponentID);
                cmd.Parameters.AddWithValue("@Operation", OperationID);
                //cmd.Parameters.AddWithValue("@Exclude", 0);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data = new JagdevRejectionData();
                        data.MachineID = reader["MachineID"].ToString();
                        data.Date = Util.GetDateTime(reader["Date"].ToString()).ToString("dd-MM-yyyy");
                        data.Shift = reader["ShiftName"].ToString();
                        data.ComponentID = reader["ComponentID"].ToString();
                        data.OperationNo = reader["Operation"].ToString();
                        data.Operator = reader["OperatorID"].ToString();
                        data.RejQty = Convert.ToDecimal(reader["RejCount"].ToString());
                        data.Accepted = Convert.ToDecimal(reader["AcceptedCount"].ToString());
                        data.RejPercentage = Convert.ToDecimal(reader["RejPercentage"].ToString());
                        listData.Add(data);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving jagdev Rejection Analysis Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return listData;
        }

        internal static List<JagdevChartData> GetJagdevChartData(string year, string week, string componentID, out List<JagdevChartData> overallOEEDataList, out List<JagdevChartData> OEEDataList, out List<JagdevChartData> effPieChartData, out List<JagdevChartData> overallOEEPieChartData, out List<JagdevChartData> OEEPieChartData)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            List<JagdevChartData> effDataList = new List<JagdevChartData>();
            JagdevChartData data;

            overallOEEDataList = new List<JagdevChartData>();
            OEEDataList = new List<JagdevChartData>();
            effPieChartData = new List<JagdevChartData>();
            overallOEEPieChartData = new List<JagdevChartData>();
            OEEPieChartData = new List<JagdevChartData>();
            JagdevChartData pieChartData = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetWeeklySummeryChart_Jagadev]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WeekNo", week);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@ComponentID", componentID);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data = new JagdevChartData();
                        data.Date = reader["Date"].ToString();
                        data.Red = Convert.ToInt32(reader["PERed"].ToString());
                        data.Yellow = Convert.ToInt32(reader["PEYellow"].ToString());
                        data.Green = Convert.ToInt32(reader["PEGreen"].ToString());
                        effDataList.Add(data);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        data = new JagdevChartData();
                        data.Red = Convert.ToInt32(reader["PERed"].ToString());
                        data.Yellow = Convert.ToInt32(reader["PEYellow"].ToString());
                        data.Green = Convert.ToInt32(reader["PEGreen"].ToString());
                        effPieChartData.Add(data);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        data = new JagdevChartData();
                        data.Date = reader["Date"].ToString();
                        data.Red = Convert.ToInt32(reader["OERed"].ToString());
                        data.Yellow = Convert.ToInt32(reader["OEYellow"].ToString());
                        data.Green = Convert.ToInt32(reader["OEGreen"].ToString());
                        overallOEEDataList.Add(data);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        data = new JagdevChartData();
                        data.Red = Convert.ToInt32(reader["OERed"].ToString());
                        data.Yellow = Convert.ToInt32(reader["OEYellow"].ToString());
                        data.Green = Convert.ToInt32(reader["OEGreen"].ToString());
                        overallOEEPieChartData.Add(data);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        data = new JagdevChartData();
                        data.Date = reader["Date"].ToString();
                        data.Red = Convert.ToInt32(reader["OERed"].ToString());
                        data.Yellow = Convert.ToInt32(reader["OEYellow"].ToString());
                        data.Green = Convert.ToInt32(reader["OEGreen"].ToString());
                        OEEDataList.Add(data);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        data = new JagdevChartData();
                        data.Red = Convert.ToInt32(reader["OERed"].ToString());
                        data.Yellow = Convert.ToInt32(reader["OEYellow"].ToString());
                        data.Green = Convert.ToInt32(reader["OEGreen"].ToString());
                        OEEPieChartData.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving jagdev chart Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return effDataList;
        }

        internal static List<JDataset> GetJagdevDashboardData(DateTime date, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            List<JDataset> listData = new List<JDataset>();
            JDataset data;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_ViewProd&DowntimeDashboard_Jagadev]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", date.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (param == "AndonMode")
                    {
                        while (reader.Read())
                        {
                            data = new JDataset();
                            data.MachineID = reader["SortOrder"].ToString();
                            data.ShiftId = reader["ShiftName"].ToString();
                            data.CellId = reader["GroupID"].ToString();
                            data.EffBackColor = reader["PEcolor"].ToString();
                            data.OEEBackColor = reader["OEcolor"].ToString();
                            data.DowntimeBackColor = reader["DownColor"].ToString();
                            data.RejectionBackColor = reader["RejColor"].ToString();
                            listData.Add(data);

                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            data = new JDataset();
                            data.MachineID = reader["SortOrder"].ToString();
                            data.ShiftId = reader["ShiftName"].ToString();
                            data.CellId = reader["GroupID"].ToString();
                            data.DowntimeBackColor = reader["DownColor"].ToString();
                            listData.Add(data);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving jagdev dashboard Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return listData;
        }
        #region "Populate Start Date "
        public static string GetLogicalDayStart(string LRunDay)
        {
            Logger.WriteDebugLog(LRunDay);
            DateTime ts = Util.GetDateTime(LRunDay);
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalDayStart( '" + ts.ToString("yyyy-MM-dd HH:mm:ss") + "')", Con);

            object SEDate = null;
            try
            {
                SEDate = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null)
                {
                    Con.Close();
                }
            }
            if (SEDate == null || Convert.IsDBNull(SEDate))
            {
                return string.Empty;
            }
            string ss = string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }

        #endregion
        #endregion
        #region MivinInspectionData
        internal static DataTable GetMivinInspectionData(DateTime fromDate, DateTime toDate, string machineId, string shiftID, string InspectionType)
        {

            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            DataTable dtMivinData = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetProcessInspectionReport_Mivin", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineId", machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                cmd.Parameters.AddWithValue("@Shift", shiftID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shiftID);
                cmd.Parameters.AddWithValue("@Type", InspectionType);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                dtMivinData.Load(reader);
                dtMivinData.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return dtMivinData;
        }

        internal static DataTable GetMivinOEEData(DateTime fromDate, string Plant, out DataTable MonthOeeData, out DataTable DayOeedata)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            DataTable dtMivinData = new DataTable();
            DayOeedata = new DataTable();
            MonthOeeData = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetAggregatedReports_Mivin", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantId", Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                dtMivinData.Load(reader);
                dtMivinData.AcceptChanges();
                MonthOeeData.Load(reader);
                MonthOeeData.AcceptChanges();
                DayOeedata.Load(reader);
                DayOeedata.AcceptChanges();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return dtMivinData;
        }

        internal static DataTable GetVulkanProdAndDownReport(DateTime fromDate, DateTime toDate, string MachineID, string PlantID, string CellID, string Shift, out DataTable dtTotal)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            DataTable dtVulkanData = new DataTable();
            dtTotal = new DataTable();
            try
            {
                cmd = new SqlCommand(@"s_GetAGG_ProdAndDownReport_Vulkan", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : MachineID);
                cmd.Parameters.AddWithValue("@CellID", CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID);
                cmd.Parameters.AddWithValue("@ShiftName", Shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Shift);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                dtVulkanData.Load(reader);
                dtVulkanData.AcceptChanges();
                dtTotal.Load(reader);
                dtTotal.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return dtVulkanData;
        }

        internal static DataTable GetAAAPLPMReport(DateTime fromDate, DateTime toDate, string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            DataTable dtPMData = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select * from PM_Transaction_AAAPL
                                        where (MachineID=@MachineID or isnull(@MachineID,'')='') and UpdatedTS>=@StartDate and UpdatedTS<=@EndDate
                                        Order by Machineid,ActivityID,UpdatedTS", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : MachineID);
                cmd.CommandTimeout = 600;
                reader = cmd.ExecuteReader();
                dtPMData.Load(reader);
                dtPMData.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return dtPMData;
        }
        #endregion

        #region ---- Operator Efficiency Report --
        internal static DataTable GetOperatorEfficiencyReportData(string fromdate, string todate, string plantId, string operatorid, out DataTable summaryDt)
        {
            DataTable ProdReportData = new DataTable();
            summaryDt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[s_GetShiftAgg_ProductionReport]", conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@OperatorID", operatorid);
                cmd.Parameters.AddWithValue("@ReportType", "OperatorWise");
                cmd.Parameters.AddWithValue("@Parameter", "ProdReport");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    ProdReportData.Load(rdr);
                    ProdReportData.AcceptChanges();
                    summaryDt.Load(rdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return ProdReportData;
        }
        #endregion

        #region ---- Operator Incentive Report Anjali --
        internal static DataTable GetOperatorIncentiveReportData(string fromdate, string todate, string machineid, string operatorid, string cycleTimeType, out DataTable summaryDt)
        {
            DataTable reportData = new DataTable();
            summaryDt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_OperatorIncentiveReport]", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromdate);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.Parameters.AddWithValue("@OperatorID", operatorid);
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@Param", cycleTimeType);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    reportData.Load(rdr);
                    reportData.AcceptChanges();
                    summaryDt.Load(rdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetOperatorIncentiveReportData =" + ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return reportData;
        }
        #endregion
        #region "Production report-Machinewise Shift-Format- 4"
        public static DataTable AnalysisMachinewiseShiftFormat4Report(DateTime StartDate, string ShiftIn, string CellID, string MachineID, string ComponentID, string OperationNo, string PlantID, DateTime EndDate, string Param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetShiftwiseProductionReportFromAutodata_Kiswok", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@ShiftIn", SqlDbType.NVarChar).Value = ShiftIn;
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@Param", SqlDbType.NVarChar).Value = Param;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }

            return values;
        }
        #endregion

        #region --Component Setup Report----
        internal static DataTable GetComponentSetupDetails(DateTime fromDate, DateTime toDate, string PlantID, string cellID, string machineID, string Component, string Operator, out DataTable Setupdt)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            Setupdt = new DataTable();
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[S_Get_Component_Setup_Report_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Plantid", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@CellID", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellID);
                cmd.Parameters.AddWithValue("@MachineID", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@Operator", Operator);
                cmd.Parameters.AddWithValue("@param", "");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                    Setupdt.Load(rdr);
                    dt.AcceptChanges();
                }
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

        internal static DataTable GetDownCodeList()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[S_Get_Component_Setup_Report_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "DownCodeList");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
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
        #endregion

        #region "Production report-Machinewise Daily-Format-4"
        public static DataTable AnalysisMachinewiseDailyFormat4Report(DateTime StartDate, string MachineID, string ComponentID, string OperationNo, string PlantID, string CellID, DateTime EndDate, string param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("[dbo].[s_GetDailyProductionReportFromAutodata_Kiswok]", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar).Value = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@param", SqlDbType.NVarChar).Value = param;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
            }
            return values;
        }
        #endregion
        #region ---- OEE Daily Report Nippon
        internal static DataTable GetOEEDailyDataNippon(DateTime fromDate, DateTime toDate, string shift, string machine, string plant, string cell, out DataTable dtShiftDetail, out DataTable dtPartDetails)
        {
            DataTable dtOeeDataKiswok = new DataTable();
            dtShiftDetail = new DataTable();
            dtPartDetails = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[dbo].[s_GetAggOEEReport_Nippon]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ShiftName", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@PlantID", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                cmd.Parameters.AddWithValue("@GroupID", cell.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cell);
                cmd.Parameters.AddWithValue("@Parameter", "View");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtOeeDataKiswok.Load(rdr);
                    dtOeeDataKiswok.AcceptChanges();
                    dtShiftDetail.Load(rdr);
                    dtShiftDetail.AcceptChanges();
                    dtPartDetails.Load(rdr);
                    dtPartDetails.AcceptChanges();
                }
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
            return dtOeeDataKiswok;
        }

        #endregion

        #region --- New OEE Report Trellbrog-------

        internal static DataTable GetOEEReport_TrellborgInfo(DateTime fromDate, DateTime toDate, string plantID, string machineID, string groupID)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {

                SqlCommand cmd = new SqlCommand(@"[S_GetOEEReport_Trellborg]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@GroupID", groupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : groupID);
                cmd.Parameters.AddWithValue("@param", "");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtval.Load(rdr);
                    dtval.AcceptChanges();
                }
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
            return dtval;
        }

        internal static DataTable GetOEEReport_Trellborg(DateTime fromDate, DateTime toDate, string plantID, string machineID, string groupID, string Shift)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {

                SqlCommand cmd = new SqlCommand(@"[S_GetShiftWiseOEEReport_Trellborg]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@GroupID", groupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : groupID);
                cmd.Parameters.AddWithValue("@Shift", Shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Shift);
                cmd.Parameters.AddWithValue("@param", "");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtval.Load(rdr);
                    dtval.AcceptChanges();
                }
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
            return dtval;
        }
        #endregion

        #region ---Operator Incentive Hourwise Report - Anjali-------
        internal static DataTable GetOperatorIncentiveHourwiseReport(DateTime fromDate, DateTime toDate, string operatorID)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {

                SqlCommand cmd = new SqlCommand(@"[dbo].[S_Get_HourWiseOperatorIncentiveReport]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@OperatorID", operatorID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : operatorID);
                cmd.CommandTimeout = 450;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtval.Load(rdr);
                }
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
            return dtval;
        }
        #endregion

        #region ---Wipro Report-------
        internal static DataTable GetOEETrendReportData(DateTime fromDate, DateTime toDate, string shift, string plant, string machine, string param, string format)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {

                SqlCommand cmd = new SqlCommand(@"s_GetOEETrend", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@shift", shift);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Parameter", param);
                cmd.Parameters.AddWithValue("@Format", format);
                cmd.CommandTimeout = 450;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtval.Load(rdr);
                }
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
            return dtval;
        }
        #endregion

        #region ---- Daily Rejection ReportData_Agg ------------
        internal static DataTable GetDailyRejectionReportData_Agg(DateTime fromDate, DateTime toDate, string operatorName, string plantID, string machineId, string componentID, string cellID)
        {
            DataTable DailyRejectionData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("S_GetAggRejectionCodeDetails", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID ", plantID = plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@machineID", machineId = machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                cmd.Parameters.AddWithValue("@ComponentID", componentID = componentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : componentID);
                cmd.Parameters.AddWithValue("@OperatorID", operatorName = operatorName.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : operatorName);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                cmd.Parameters.AddWithValue("@param", "");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    DailyRejectionData.Load(rdr);
                    DailyRejectionData.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return DailyRejectionData;
        }
        #endregion

        #region ---Operator Incentive Hourwise Report - Anjali-------
        internal static DataTable GetTraceabilityReportAdvikPanthData(DateTime fromDate, DateTime toDate, string model, string qrCode)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {

                SqlCommand cmd = new SqlCommand(@"[dbo].[S_GetTraceabilityReportDetails_Advik]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@Model", model.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : model);
                cmd.Parameters.AddWithValue("@Barcode", qrCode);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                cmd.CommandTimeout = 450;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtval.Load(rdr);
                }
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
            return dtval;
        }
        #endregion

        #region ---Leonine Report ----
        internal static DataTable GetLeonineReportData(string shift, DateTime fromdate, DateTime todate, string cellID, string machineId)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("S_Get_Hourly_Production_Repotrs_Leonine", conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromdate);
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@Shift", (shift == "All" ? "" : shift));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetLeonineReportData: " + ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        #endregion
        #region ------- Production Analysis Report RTPL -------------
        public static List<ProductionData> GetProductionAnalysisReportRTPL(DateTime fromDate, DateTime toDate, string shifts, string plantId, string machineIds, string param, out ProductionAnalysisForExcelSummary Summaryresult,
     out List<string> cumProgramNo, out List<string> cumPartsCount, out List<string> cumOEE)
        {

            SqlConnection conn = ConnectionManager.GetConnection();
            List<ProductionData> prodDatas = new List<ProductionData>();
            cumProgramNo = new List<string>();
            cumPartsCount = new List<string>();
            cumOEE = new List<string>();
            int indexVal = 0;

            double sumPowerOnTime = 0;
            double sumStdShiftTime = 0;
            double sumCuttingTime = 0;
            double sumOperationTime = 0;
            double sumPartsCount = 0;
            double AvgOEE = 0;
            int i = 0;
            string prevShift = string.Empty;
            string prevDate = string.Empty;
            string prevMachine = string.Empty;
            string prevFromTime = string.Empty;

            bool isDuplicatePresent = false;
            bool firstRow = false;
            DateTime tempDate = DateTime.MinValue;
            string tempMachine = string.Empty;
            SqlDataReader sdr = null;
            Summaryresult = new ProductionAnalysisForExcelSummary();
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetFocasShiftwiseLiveDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Starttime", SqlDbType.DateTime).Value = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.AddWithValue("@Endtime", SqlDbType.DateTime).Value = toDate.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.AddWithValue("@Shiftname", SqlDbType.NVarChar).Value = shifts;
                cmd.Parameters.AddWithValue("@PlantID", SqlDbType.NVarChar).Value = plantId;
                cmd.Parameters.AddWithValue("@Machineid", SqlDbType.NVarChar).Value = machineIds;
                cmd.Parameters.AddWithValue("@Param", SqlDbType.NVarChar).Value = param;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (tempMachine != Convert.ToString(sdr["MachineID"]) && firstRow)
                        {
                            ProductionData pd1 = new ProductionData();
                            pd1.Shift = "Total";
                            pd1.MachineID = Convert.ToString(tempMachine);
                            pd1.PowerOnTime = Math.Round(sumPowerOnTime, 2);
                            pd1.StdShiftTime = Math.Round(sumStdShiftTime, 2);
                            pd1.CuttingTime = Math.Round(sumCuttingTime, 2);
                            pd1.OperationTime = Math.Round(sumOperationTime, 2);
                            pd1.PartsCount = Math.Round(sumPartsCount, 2);
                            if (AvgOEE > 0)
                            {
                                pd1.OEE = Math.Round(AvgOEE / i, 2);
                            }
                            else
                            {
                                pd1.OEE = 0;
                            }
                            // pd1.OEE = Math.Round(AvgOEE, 2);
                            sumPowerOnTime = 0;
                            sumStdShiftTime = 0;
                            sumCuttingTime = 0;
                            sumOperationTime = 0;
                            sumPartsCount = 0;
                            AvgOEE = 0;
                            i = 0;
                            prodDatas.Add(pd1);
                        }
                        i++;
                        firstRow = true;
                        ProductionData pd = new ProductionData();
                        pd.MachineID = Convert.ToString(sdr["MachineID"]);
                        if (!Convert.IsDBNull(sdr["MachineID"]))
                        {
                            pd.MachineID = Convert.ToString(sdr["MachineID"]);
                            tempMachine = Convert.ToString(sdr["MachineID"]);
                        }

                        if (param == "Shift" || param == "Hour")
                        {
                            pd.Shift = Convert.ToString(sdr["ShiftName"]);
                        }
                        if (param == "Hour")
                        {
                            pd.FromTime = sdr["From Time"].ToString();
                            pd.ToTime = sdr["To Time"].ToString();
                        }

                        if (param == "Day")
                        {
                            pd.FromTime = string.Format(sdr["From Time"].ToString(), "yyyy-MM-dd");
                        }

                        if (param == "Shift" || param == "Hour")
                        {
                            if (!Convert.IsDBNull(sdr["ShiftDate"]))
                            {
                                tempDate = Convert.ToDateTime(sdr["ShiftDate"]);
                                pd.Date = sdr.GetDateTime(sdr.GetOrdinal("ShiftDate")).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        if (!Convert.IsDBNull(sdr["PowerOnTime"]))
                        {
                            pd.PowerOnTime = Math.Round(Convert.ToDouble(sdr["PowerOnTime"]), 2);
                            if (param == "Shift")
                            {
                                if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                                    sumPowerOnTime = sumPowerOnTime + Convert.ToDouble(sdr["PowerOnTime"]);
                            }
                            else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                            {

                                sumPowerOnTime = sumPowerOnTime + Convert.ToDouble(sdr["PowerOnTime"]);
                            }
                        }
                        if (!Convert.IsDBNull(sdr["TotalTime"]))
                        {
                            pd.StdShiftTime = Math.Round(Convert.ToDouble(sdr["TotalTime"]), 2);
                            if (param == "Shift")
                            {
                                if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                                    sumStdShiftTime = sumStdShiftTime + Convert.ToDouble(sdr["TotalTime"]);
                            }
                            else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                            {

                                sumStdShiftTime = sumStdShiftTime + Convert.ToDouble(sdr["TotalTime"]);
                            }
                        }

                        if (!Convert.IsDBNull(sdr["Cutting time"]))
                        {
                            pd.CuttingTime = Math.Round(Convert.ToDouble(sdr["Cutting time"]), 2);
                            if (param == "Shift")
                            {
                                if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                                    sumCuttingTime = sumCuttingTime + Convert.ToDouble(sdr["Cutting time"]);
                            }
                            else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                            {
                                sumCuttingTime = sumCuttingTime + Convert.ToDouble(sdr["Cutting time"]);
                            }
                        }
                        if (!Convert.IsDBNull(sdr["Operating time"]))
                        {
                            pd.OperationTime = Math.Round(Convert.ToDouble(sdr["Operating time"]), 2);
                            if (param == "Shift")
                            {
                                if (prevShift != Convert.ToString(sdr["ShiftName"]) || prevMachine != Convert.ToString(sdr["MachineID"]) || prevDate != Convert.ToString(sdr["ShiftDate"]))
                                    sumOperationTime = sumOperationTime + Convert.ToDouble(sdr["Operating time"]);
                            }
                            else if ((prevFromTime != Convert.ToString(sdr["From Time"]) || prevMachine != Convert.ToString(sdr["MachineID"])))
                            {
                                sumOperationTime = sumOperationTime + Convert.ToDouble(sdr["Operating time"]);
                            }
                        }

                        if (param == "Shift" || param == "Hour")
                        {
                            prevShift = Convert.ToString(sdr["ShiftName"]);
                            prevDate = Convert.ToString(sdr["ShiftDate"]);
                        }
                        if (param == "Day" || param == "Hour")
                        {
                            prevFromTime = Convert.ToString(sdr["From Time"]);
                        }
                        prevMachine = Convert.ToString(sdr["MachineID"]);

                        // Added
                        if (!Convert.IsDBNull(sdr["ProgramNo"]))
                        {
                            pd.ProgramNoVal = Convert.ToString(sdr["ProgramNo"]);
                            //Added
                            if (pd.ProgramNoVal != "0")
                            {
                                if (cumProgramNo.Contains(pd.ProgramNoVal))
                                {
                                    indexVal = (int)cumProgramNo.IndexOf(Convert.ToString(sdr["ProgramNo"]));
                                    isDuplicatePresent = true;
                                }
                                else
                                {
                                    cumProgramNo.Add(Convert.ToString(sdr["ProgramNo"]));
                                }
                            }

                        }

                        if (!Convert.IsDBNull(sdr["PartsCount"]))
                        {
                            pd.PartsCount = Convert.ToDouble(sdr["PartsCount"]);
                            sumPartsCount = sumPartsCount + Convert.ToDouble(sdr["PartsCount"]);
                            if (pd.ProgramNoVal != "0")
                            {
                                if (isDuplicatePresent)
                                {
                                    int indexItem = Convert.ToInt32(cumPartsCount[indexVal]);
                                    // int newVal = (Convert.ToInt32(sdr["PartsCount"]));
                                    var res = indexItem + Math.Abs((Convert.ToInt32(sdr["PartsCount"])));
                                    cumPartsCount[indexVal] = Convert.ToString(res);
                                    isDuplicatePresent = false;
                                }
                                else
                                {
                                    cumPartsCount.Add(Convert.ToString(sdr["PartsCount"]));
                                }
                            }
                        }

                        if (!Convert.IsDBNull(sdr["OEE"]))
                        {
                            pd.OEE = Convert.ToDouble(sdr["OEE"]);
                            AvgOEE = AvgOEE + Convert.ToDouble(sdr["OEE"]);
                            if (pd.ProgramNoVal != "0")
                            {
                                if (isDuplicatePresent)
                                {
                                    int indexItem = Convert.ToInt32(cumOEE[indexVal]);
                                    // int newVal = (Convert.ToInt32(sdr["PartsCount"]));
                                    var res = indexItem + Math.Abs((Convert.ToInt32(sdr["OEE"])));
                                    cumOEE[indexVal] = Convert.ToString(res);
                                    isDuplicatePresent = false;
                                }
                                else
                                {
                                    cumOEE.Add(Convert.ToString(sdr["OEE"]));
                                }
                            }

                        }


                        pd.ProgramComment = sdr["PartNumber"].ToString();

                        //    pd.OEE = Convert.ToDouble(sdr["OEE"].ToString());

                        prodDatas.Add(pd);
                    }

                    #region To insert summission of last Date
                    ProductionData pd2 = new ProductionData();
                    //if (param.Equals("Day")) pd2.Date = "Total";
                    pd2.Shift = "Total";
                    //pd2.Date = Convert.ToString(tempDate);
                    pd2.MachineID = Convert.ToString(tempMachine);
                    pd2.PowerOnTime = Math.Round(sumPowerOnTime, 2);
                    pd2.StdShiftTime = Math.Round(sumStdShiftTime, 2);
                    pd2.CuttingTime = Math.Round(sumCuttingTime, 2);
                    pd2.OperationTime = Math.Round(sumOperationTime, 2);
                    pd2.PartsCount = Math.Round(sumPartsCount, 2);
                    if (AvgOEE > 0)
                    {
                        pd2.OEE = Math.Round(AvgOEE / i, 2);
                    }
                    else
                    {
                        pd2.OEE = 0;
                    }
                    sumPowerOnTime = 0;
                    sumStdShiftTime = 0;
                    sumCuttingTime = 0;
                    sumOperationTime = 0;
                    sumPartsCount = 0;
                    AvgOEE = 0;
                    i = 0;
                    prodDatas.Add(pd2);
                    #endregion
                }

                sdr.NextResult();

                while (sdr.Read())
                {
                    if (!Convert.IsDBNull(sdr["PowerOnTime"]))
                    {
                        Summaryresult.PowerOnTime = Convert.ToDouble(sdr["PowerOnTime"]);
                    }
                    if (!Convert.IsDBNull(sdr["TotalTime"]))
                    {
                        Summaryresult.StdShiftTime = Convert.ToDouble(sdr["TotalTime"]);
                    }

                    if (!Convert.IsDBNull(sdr["Cutting Time"]))
                    {
                        Summaryresult.CuttingTime = Convert.ToDouble(sdr["Cutting Time"]);
                    }

                    if (!Convert.IsDBNull(sdr["TotalTime"]))
                    {
                        Summaryresult.TotalTime = Convert.ToDouble(sdr["TotalTime"]);
                    }

                    if (!Convert.IsDBNull(sdr["OperatingWithoutCutting"]))
                    {
                        Summaryresult.OperatingWithoutCutting = Convert.ToDouble(sdr["OperatingWithoutCutting"]);
                    }

                    if (!Convert.IsDBNull(sdr["NonOperatingTime"]))
                    {
                        Summaryresult.NonOperatingTime = Convert.ToDouble(sdr["NonOperatingTime"]);
                    }
                    if (!Convert.IsDBNull(sdr["Operating Time"]))
                    {
                        Summaryresult.OperatingTime = Convert.ToDouble(sdr["Operating Time"]);
                    }
                    if (!Convert.IsDBNull(sdr["PowerOffTime"]))
                    {
                        Summaryresult.PowerOffTime = Convert.ToDouble(sdr["PowerOffTime"]);
                    }

                    if (!Convert.IsDBNull(sdr["PartsCount"]))
                    {
                        Summaryresult.PartsCount = Math.Round(Convert.ToDouble(sdr["PartsCount"]), 2);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null)
                    if (conn != null) conn.Close();
            }
            return prodDatas;
        }
        #endregion


        #region "Down time report for Machine down time matrix "
        public static DataTable GetShiftwiseDownTimeDetailsAutoTechData(DateTime startDate, DateTime endDate, string plantID, string cellID, string machineID)
        {
            DataTable dt = new DataTable();
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_ShiftwiseDownTimeDetails_Autotech]", Con);
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@StartTime", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (Con != null) Con.Close();
            }
            return dt;
        }
        #endregion

        #region ---- Spindle Idle Time Analysis Report LnTOdisha
        internal static DataTable GetSpindleIdleTimeAnalysisReportLnTOdisha(DateTime date, string machine, string shift, out DataTable dtTotalTime)
        {
            DataTable dt = new DataTable();
            dtTotalTime = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_Get_SpindleIdleTimeAnalysisReport_LnT]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dtTotalTime.Load(rdr);
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
            return dt;
        }

        #endregion
        #region ---- Spindle Run Time Analysis Report LnTOdisha
        internal static DataTable GetSpindleRunTimeReportLnTOdisha(DateTime fromDate, DateTime toDate, string shift, string plant, string cells, string machine, out DataTable dtTotalTime)
        {
            DataTable dt = new DataTable();
            dtTotalTime = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_Get_SpindleRunTimeReport_LnT]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@PlantID", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@GroupID", cells);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dtTotalTime.Load(rdr);
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
            return dt;
        }

        #endregion

        #region  ---Production and Down Report ACE ---
        internal static DataTable GetProductionandDownSummaryDataACE(DateTime fromDate, DateTime toDate, string machineID, out DataTable dtDownReason)
        {
            DataTable dt = new DataTable();
            dtDownReason = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_ACE_SAP_ProductionAndDownReport]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", "Summary");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dtDownReason.Load(rdr);
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
            return dt;
        }

        internal static DataTable GetProductionandDownCyclewiseDataACE(DateTime fromDate, DateTime toDate, string machineID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_ACE_SAP_ProductionAndDownReport]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", "CyclewiseAnalysis");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
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
            return dt;
        }

        internal static DataTable GetProductionandDownScrapDataACE(DateTime fromDate, DateTime toDate, string machineID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_ACE_SAP_ProductionAndDownReport]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", "Scrap");
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
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
            return dt;
        }

        #endregion

        #region --PartFamily KTA---

        internal static List<string> GetComponentIdListBasedOnPlantCell(string Plant, string Cell)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstComponentID = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct componentid from PlantMachineGroups p1 inner join componentoperationpricing cop on p1.MachineID=cop.machineid where p1.PlantID=@PlantId and p1.GroupID=@GroupID", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantId", Plant);
                cmd.Parameters.AddWithValue("@GroupID", Cell);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstComponentID.Add(sdr["ComponentID"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstComponentID;
        }

        internal static List<string> GetAllPartFamily()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstPartFamily = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct PartFamily from componentinformation where PartFamily<>''", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstPartFamily.Add(sdr["PartFamily"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstPartFamily;
        }

        internal static List<string> GetAllComponentByPartFamily(string PartFamilyName)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstComponent = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct componentid from componentinformation where PartFamily=@PartFamily", sqlConn);
                cmd.Parameters.AddWithValue("@PartFamily", PartFamilyName);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstComponent.Add(sdr["componentid"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstComponent;
        }
        #endregion

        #region ---- PDI Report Tafe Chennai---

        internal static DataTable GetPDIReportData(string slNo, out DataTable dtCrownHeader, out DataTable dtPinionHeader, out DataTable dtCrownData, out DataTable dtPinionData)
        {

            DataTable dt = new DataTable();
            dtCrownHeader = new DataTable();
            dtPinionHeader = new DataTable();
            dtCrownData = new DataTable();
            dtPinionData = new DataTable();
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;

            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[SP_ViewPDIDetails_Tafe]", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SerialNo", slNo);
                cmd.Parameters.AddWithValue("@Param", "Web Report");
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dtCrownHeader.Load(rdr);
                dtPinionHeader.Load(rdr);
                dtCrownData.Load(rdr);
                dtPinionData.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        #endregion

        #region  -- Incentive Report GK --
        internal static DataTable GetIncentiveReportGK(DateTime fromDate, DateTime toDate, string Operator)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("[SP_PeAndIncentiveReport_GK]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Todate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@OperatorName", Operator);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return dt;
        }

        #endregion
        #region--------Tool change Frequency Record Vulkan-----------------
        internal static DataTable GetToolChangeFrequencyData_Vulkan(DateTime FromDate, DateTime ToDate, string MachineID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_ToolChangeReport_Vulkan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@Machine", MachineID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        #endregion
        #region---------------Component standard Cycltime Comparison Report_KTA-----------------
        internal static DataTable GetStandardCycleComparisionData_KTA(DateTime FromDate, DateTime Todate, string MachineID, string ComponentID, string Operation)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_StdCycleTimeComparision_KTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fromdate", FromDate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@machineid", MachineID);
                cmd.Parameters.AddWithValue("@componentid", ComponentID);
                cmd.Parameters.AddWithValue("@operationno", Operation);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt;
        }
        #endregion

        #region -- Seyoon Custom Report --
        internal static DataTable GetSeyoonProductionRejectionData(string StartDate, string ToDate, string MachineID, string Shift, out DataTable dtRejection, out DataTable dtSummaryRowDown, out DataTable dtSummaryRowRejection, out DataTable dtSummaryRow2)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dtDown = new DataTable();
            dtRejection = new DataTable();
            dtSummaryRowDown = new DataTable();
            dtSummaryRowRejection = new DataTable();
            dtSummaryRow2 = new DataTable();
            try
            {
                cmd = new SqlCommand("S_GetProductionReport_Seyoon", conn);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dtDown.Load(sdr);
                    dtRejection.Load(sdr);
                    dtSummaryRowDown.Load(sdr);
                    dtSummaryRowRejection.Load(sdr);
                    dtSummaryRow2.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetSeyoonProductionRejectionData: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dtDown;
        }
        #endregion
        #region---------------Daily Production Report - KunAero------
        internal static DataTable GetDailyProductionReportData_KunAero(out DataTable dt1, DateTime Date, string MachineID, string ComponentID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            dt1 = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_GetDailyReport_KunAero", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", ComponentID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                    dt1.Load(sdr);
                    dt1.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }
        #endregion

        internal static DataTable ComponentwiseReportAgg(DateTime StartDate, string ComponentID, string OperationNo, DateTime EndDate)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("[SP_GetAggComponentProductionReport]", Con);
            cmd.CommandTimeout = 360;
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.Add("@ComponentID", SqlDbType.NVarChar).Value = ComponentID;
            cmd.Parameters.Add("@OperationNo", SqlDbType.NVarChar).Value = OperationNo;
            cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("GENERATED ERROR : \n" + ex.ToString());
            }
            finally
            {
                if (Con != null) Con.Close();
                if (sdr != null) sdr.Close();
            }
            return values;
        }

        #region --- Precision Engg ---
        internal static DataTable GetPrecisionEnggReport(DateTime startDate, DateTime endDate, string machine, string shift, string Component, string Operator, string Param)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {

                SqlCommand cmd = new SqlCommand(@"[S_GetProd&RejcReport_Precision]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@Shift", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@ComponentID", Component.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Component);
                cmd.Parameters.AddWithValue("@OperatorName", Operator.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Operator);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtval.Load(rdr);
                    dtval.AcceptChanges();
                }
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
            return dtval;
        }
        internal static DataTable GetMaintenanceParticularReport_precisionEnggData(DateTime Date, DateTime ToDate, string machine, string Type)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {

                SqlCommand cmd = new SqlCommand(@"[S_GetDailyCleaning&MaintanenceSheet_Precision]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", ToDate.ToString("yyyy-MM-dd"));
                if (Type == "Machinewise")
                {
                    cmd.Parameters.AddWithValue("@MachineID", machine);
                    cmd.Parameters.AddWithValue("@Param", "View_Transaction");
                }
                else if (Type == "Operatorwise")
                {
                    cmd.Parameters.AddWithValue("@OperatorID", machine);
                    cmd.Parameters.AddWithValue("@Param", "View_OperatorWise");
                }
                else if (Type == "Particularwise")
                {
                    cmd.Parameters.AddWithValue("@ParticularID", machine);
                    cmd.Parameters.AddWithValue("@Param", "View_ParticularWise");
                }
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtval.Load(rdr);
                    dtval.AcceptChanges();
                }
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
            return dtval;
        }

        internal static DataTable GetDailyCleaningAndMaintenance_PrecisionEnggReport(DateTime fromDate, string groupID, string machineId, string Param)
        {
            DataTable dtval = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_GetDailyCleaning&MaintanenceSheet_Precision]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@GroupID", groupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : groupID);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.CommandTimeout = 600;
                rdr = cmd.ExecuteReader();
                dtval.Load(rdr);
                dtval.AcceptChanges();
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
            return dtval;
        }

        #endregion
    }
}