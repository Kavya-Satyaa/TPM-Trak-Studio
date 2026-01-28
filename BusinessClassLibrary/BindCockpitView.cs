using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseClassLibrary;
using ModelClassLibrary;
using System.Diagnostics;

namespace BusinessClassLibrary
{
    public class BindCockpitView
    {

        #region ------Bind Plant To Display----------
        public static List<string> ViewPlantToDisplay()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstPlantData = new List<string>();
            try
            {
                cmd = new SqlCommand(@"[s_GetLookups]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Plant");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstPlantData.Add(sdr["Plantid"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstPlantData;
        }

        public static List<string> GetPPMGraphData()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region "Bind Cell Id To Display"
        public static List<string> ViewCellsToDisplay(string plantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstCellData = new List<string>();
			plantId = plantId == "All" ? "" : plantId;
            try
            {
                if (string.IsNullOrEmpty(plantId))
                {
                    cmd = new SqlCommand(@"select distinct GroupID from PlantMachineGroups", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"select distinct GroupID from PlantMachineGroups where PlantID = @PlantID", sqlConn);
                }
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstCellData.Add(sdr["GroupID"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstCellData;
        }

    
        #endregion

        #region ------Bind Component ID----------
        public static List<string> ViewComponentInfo()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstPlantData = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select DISTINCT componentId from componentinformation", sqlConn);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lstPlantData.Add(sdr["ComponentId"].ToString());
                    }
                }
                sdr.Close();
            }
            catch (Exception)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstPlantData;
        }
        #endregion

        #region ------Bind Component ID----------
        public static List<EmployeeInfo> ViewEmployeeInfo()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<EmployeeInfo> lstEmpData = new List<EmployeeInfo>();
            try
            {
                cmd = new SqlCommand(@"select * from employeeinformation", sqlConn);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        EmployeeInfo obj = new EmployeeInfo();
                        obj.EmpID = sdr["EmployeeId"].ToString();
                        obj.EmpName = sdr["Name"].ToString();
                        obj.EmpEmailId = sdr["Email"].ToString();
                        lstEmpData.Add(obj);
                    }
                }
                sdr.Close();
            }
            catch (Exception)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstEmpData;
        }

       
        #endregion

        #region --------------Bind Shift Data--------------
        public static List<string> GetAllShift()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> shiftList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from shiftDetails where running = 1", sqlConn);
                //shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    shiftList.Add(rdr["shiftName"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return shiftList;
        }
        #endregion

        #region "Populate Current Shift"
        public static string GetShift()
        {
            string shift = string.Empty;

            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetCurrentShift]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        shift = sdr["ShiftName"].ToString();
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return shift;
        }

        public static List<string> GetOperatorNameDataForPlant(string plant)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> operationList = new List<string>();
            try
            {
                SqlCommand cmd;
                if (plant.Trim() == "ALL" || plant.Trim() == "")
                {
                    cmd = new SqlCommand(@"select distinct e.Name from employeeinformation e left join PlantEmployee p on e.Employeeid=p.EmployeeID ", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"select distinct e.Name from employeeinformation e left join PlantEmployee p on e.Employeeid=p.EmployeeID where p.PlantID=@plant", sqlConn);
                    cmd.Parameters.AddWithValue("@plant", plant);
                }

                //shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    operationList.Add(rdr["Name"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return operationList;
        }
        #endregion

        #region "Bind DashBoard Table Details"
        public static List<DashboardDetails> BindTableData(string date, string shiftName, string plantId, string machineId, string comparisonType, string parameter, string Component, string Employee)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            List<DashboardDetails> lstData = new List<DashboardDetails>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetAggDrilldownTPMTrakData_Grid]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Component", Component);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        DashboardDetails obj = new DashboardDetails();
                        obj.MachineId = sdr["MachineID"].ToString();
                        obj.OEE = Convert.ToDecimal(sdr["OEffy"].ToString());
                        obj.AE = Convert.ToDecimal(sdr["AEffy"].ToString());
                        obj.PE = Convert.ToDecimal(sdr["PEffy"].ToString());
                        obj.QE = Convert.ToDecimal(sdr["QEffy"].ToString());
                        obj.Accepted = Convert.ToDecimal(sdr["AcceptedParts"].ToString());
                        obj.Rejected = Convert.ToDecimal(sdr["RejCount"].ToString());
                        obj.Rework = Convert.ToDecimal(sdr["ReworkPerformed"].ToString());
                        obj.Downtime = Convert.ToDecimal(sdr["DownTime"].ToString());
                        lstData.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("Table Pross Name : " + "(s_GetAggDrilldownTPMTrakData_Grid) " + stopwatch.Elapsed.TotalSeconds);
            return lstData;
        }
        #endregion

        #region "Bind Dashboard Graph YEAR information"
        public static DataTable BindDashBoardGraph(string date, string shiftName, string plantId, string machineId, string comparisonType, string parameter, string Component, string Employee, string SortColumn, string SortOrder)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            DataTable values = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetAggDrilldownTPMTrakData_Graph]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Component", Component);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@SortColumn", SortColumn);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                SqlDataReader sdr = cmd.ExecuteReader();
                values.Load(sdr);
                values.AcceptChanges();
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("Chart Pross Name (s_GetAggDrilldownTPMTrakData_Graph) : " + "(" + parameter + "parameter )" + comparisonType + " " + stopwatch.Elapsed.TotalSeconds);
            return values;
        }
        #endregion

        #region "Bind Accepted Information Details"
        public static DataTable AcceptedInformationData(string date, string shiftName, string plantId, string machineId, string comparisonType, string Component, string CellID, string Employee, string View)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            DataTable dt = new DataTable();
            CellID = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            List<DashboardDetails> lstData = new List<DashboardDetails>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetAggDrilldownPartsData]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Component", Component);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@Groupid", CellID);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@View", View);
                SqlDataReader sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.AcceptChanges();

                sdr.Close();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        #endregion

        #region "Bind Rejected,Rework and Downtime/Stoppage information"
        public static DataTable BindRejRewDowntimeInfo(string date, string shiftName, string plantId, string machineId, string comparisonType, string parameter, string Component, string CellID, string Employee, string view)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            CellID = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID;
            if (!(string.IsNullOrEmpty(plantId)) && view != "ComponentwiseView")
            {
                plantId = "'" + plantId + "'";
            }
            if (!(string.IsNullOrEmpty(CellID)) && view != "ComponentwiseView")
            {
                CellID = "'" + CellID + "'";
            }
            if (sqlConn.State == ConnectionState.Closed) return null;
            DataTable values = new DataTable();
            try
            {
                if (view == "ComponentwiseView")
                {
                    cmd = new SqlCommand(@"[s_GetAggDrilldownRejectionAndDownData_Component]", sqlConn);
                }
                else if (view == "OperatorwiseView")
                {
                    cmd = new SqlCommand(@"[s_GetAggDrilldownRejectionAndDownData_Operator]", sqlConn);
                }
                else if (view == "PlantwiseView")
                {
                    cmd = new SqlCommand(@"[s_GetAggDrilldownRejectionAndDownData_Plant]", sqlConn);
                }
                else if (view == "CellWiseView")
                {
                    cmd = new SqlCommand(@"[s_GetAggDrilldownRejectionAndDownData_Plant]", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"[s_GetAggDrilldownRejectionAndDownData]", sqlConn);
                }
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@EndDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Component", Component);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@Groupid", CellID);
                cmd.Parameters.AddWithValue("@Param", parameter);
                SqlDataReader sdr = cmd.ExecuteReader();
                values.Load(sdr);
                values.AcceptChanges();
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return values;
        }
        #endregion

        public static List<string> GetAllUserData()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Employeeid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select Employeeid from [dbo].[employeeinformation] order by employeeid asc", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Employeeid.Add(rdr["Employeeid"].ToString().ToUpper().Trim());
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
            return Employeeid;
        }

        public static UserAccessModel GetEmployeeDetails(string Userid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            UserAccessModel Employee = new UserAccessModel();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from employeeinformation where employeeid = @employee", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@employee", Userid);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {

                    Employee.Admin = sdr["isadmin"].ToString() == "0" ? false : true;
                    Employee.Password = sdr["upassword"] != DBNull.Value ? sdr["upassword"].ToString() : "";
                    Employee.UserName = sdr["Name"].ToString();
                }
                sdr.Close();
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
            return Employee;
        }

        #region "Get User Access Details"
        public static List<UserAccessModel> bindListUserAccess(string uid)
        {
            List<UserAccessModel> list = new List<UserAccessModel>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            UserAccessModel vals = null;
            try
            {

                cmd = new SqlCommand(@"ss_UserAccessRights", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", uid);
                cmd.Parameters.AddWithValue("@param", "WebView");
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    vals = new UserAccessModel();
                    vals.Domain = sdr["domain"].ToString();
                    vals.DisplayText = sdr["displaytext"].ToString();
                    vals.Code = sdr["Code"].ToString();
                    vals.Selected = Convert.ToBoolean(sdr["Isvisible"].ToString());
                    vals.Prochecked = Convert.ToBoolean(sdr["Isvisible"].ToString()) == true ? "checked" : "";
                    //if (!string.IsNullOrEmpty(sdr["WebColumn"].ToString())) vals.ColumnType = sdr["WebColumn"].ToString();
                    list.Add(vals);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        #endregion

        #region "Delete Use Access Rights"
        public static void DeleteDataUserAccessRights(string userid, string menu, out string successFailure)
        {
            successFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("delete from useraccessrights_Web where employeeid= @userid  and [type]=@type", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@type", menu);
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

        public static void InsertAsadmin(string employee, bool isAdmin, string password)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("update employeeinformation set isadmin = @isadmin,upassword=@password where employeeid = @employee", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@employee", employee);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@isadmin", isAdmin);
                cmd.ExecuteNonQuery();
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

        }

        public static void InsertDataUserAccessRights(string type, string Code, string EmployeeId, out string SuccessFailure)
        {
            SuccessFailure = string.Empty;
            SqlConnection ConSql = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                //                string query = @"if not exists(select * from useraccessrights where Domain=@Domain and [Code]=@Code and EmployeeId=@EmployeeId)
                //                   Begin
                //                        insert into useraccessrights([Domain],[type],[EmployeeId]) values(@Code,@type,@EmployeeId)
                //                  End";
                cmd = new SqlCommand(" insert into useraccessrights_Web([Domain],[type],[EmployeeId]) values(@Code,@type,@EmployeeId)", ConSql);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@Code", Code);
                cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                SuccessFailure = "Successfull";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving data - \n" + ex.ToString());
                throw;
            }
            finally { if (ConSql != null) ConSql.Close(); }

        }
        #endregion

        #region "Bind DashBoard Table Details Machine and Plant wise View"
        public static List<DashboardDetails> BindTableData(string date, string shiftName, string plantId, string machineId, string comparisonType, string parameter, string Component, string Employee, string cellId, string typeView)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            
            List<DashboardDetails> lstData = new List<DashboardDetails>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetAggDrilldownTPMTrakData_Grid]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId =="null" ? "":plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Component", Component);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@view", typeView);
                cmd.Parameters.AddWithValue("@Groupid", cellId);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        DashboardDetails obj = new DashboardDetails();
                        if (typeView.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                            obj.MachineId = sdr["MachineID"].ToString();
                        else
                            obj.MachineId = sdr["PlantID"].ToString();
                        obj.OEE = Convert.ToDecimal(sdr["OEffy"].ToString());
                        obj.AE = Convert.ToDecimal(sdr["AEffy"].ToString());
                        obj.PE = Convert.ToDecimal(sdr["PEffy"].ToString());
                        obj.QE = Convert.ToDecimal(sdr["QEffy"].ToString());
                        obj.Accepted = Convert.ToDecimal(sdr["AcceptedParts"].ToString());
                        obj.Rejected = Convert.ToDecimal(sdr["RejCount"].ToString());
                        obj.Rework = Convert.ToDecimal(sdr["ReworkPerformed"].ToString());
                        obj.Downtime = Convert.ToDecimal(sdr["DownTime"].ToString());
                        obj.McHrRate = Convert.ToDecimal(sdr["AvgMcHrRate"].ToString());
                        obj.PPM =(sdr["PPM"].ToString());
                        decimal Green = 0;
                        decimal Red = 0;
                        decimal.TryParse(sdr["OEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["OERed"].ToString(), out Red);
                        if(obj.OEE>=Green)
                        {
                            obj.OEEColor = "green";
                        }
                        else if (obj.OEE <= Red && obj.OEE!=0)
                        {
                            obj.OEEColor = "red";
                        }
                        else if (obj.OEE > Red && obj.OEE < Green)
                        {
                            obj.OEEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }

                        decimal.TryParse(sdr["AEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["AERed"].ToString(), out Red);
                        if (obj.AE >= Green)
                        {
                            obj.AEColor = "green";
                        }
                        else if (obj.AE <= Red && obj.AE != 0)
                        {
                            obj.AEColor = "red";
                        }
                        else if (obj.AE > Red && obj.AE < Green)
                        {
                            obj.AEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }

                        decimal.TryParse(sdr["PEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["PERed"].ToString(), out Red);
                        if (obj.PE >= Green)
                        {
                            obj.PEColor = "green";
                        }
                        else if (obj.PE <= Red && obj.PE != 0)
                        {
                            obj.PEColor = "red";
                        }
                        else if (obj.PE > Red && obj.PE < Green)
                        {
                            obj.PEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }

                        decimal.TryParse(sdr["QEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["QERed"].ToString(), out Red);
                        if (obj.QE >= Green)
                        {
                            obj.QEColor = "green";
                        }
                        else if (obj.QE <= Red && obj.QE != 0)
                        {
                            obj.QEColor = "red";
                        }
                        else if(obj.QE>Red && obj.QE<Green)
                        {
                            obj.QEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }
                       
                        if (typeView.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
							obj.MachineDescription = sdr["machineDescription"].ToString();
                        lstData.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
               // throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("Table Pross Name : " + "(s_GetAggDrilldownTPMTrakData_Grid) " + stopwatch.Elapsed.TotalSeconds);
            return lstData;
        }
        #endregion

        #region "Bind DashBoard Table Details Componet and Operator wise View"
        public static List<DashboardDetails> BindTableComponentAndOperatorData(string date, string shiftName, string plantId, string operatorID, string comparisonType, string parameter, string procName, string cellId, string typeView, string componentID)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            List<DashboardDetails> lstData = new List<DashboardDetails>();
            try
            {
                SqlCommand cmd = new SqlCommand(procName, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = date;
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId == "null" ? "":plantId);
                if (typeView.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                    cmd.Parameters.AddWithValue("@Component", componentID);
                else
                    cmd.Parameters.AddWithValue("@Operator", operatorID);
                cmd.Parameters.Add("@ComparisonType", SqlDbType.NVarChar).Value = comparisonType;
                cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar).Value = parameter;
                cmd.Parameters.AddWithValue("@Groupid", cellId);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        DashboardDetails obj = new DashboardDetails();
                        if (typeView.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                            obj.MachineId = sdr["ComponentID"].ToString();
                        else
                            obj.MachineId = sdr["OperatorID"].ToString();
                        obj.OEE = Convert.ToDecimal(sdr["OEffy"].ToString());
                        obj.AE = Convert.ToDecimal(sdr["AEffy"].ToString());
                        obj.PE = Convert.ToDecimal(sdr["PEffy"].ToString());
                        obj.QE = Convert.ToDecimal(sdr["QEffy"].ToString());
                        obj.Accepted = Convert.ToDecimal(sdr["AcceptedParts"].ToString());
                        obj.Rejected = Convert.ToDecimal(sdr["RejCount"].ToString());
                        obj.Rework = Convert.ToDecimal(sdr["ReworkPerformed"].ToString());
                        obj.Downtime = Convert.ToDecimal(sdr["DownTime"].ToString());
                        if(!procName.Equals("s_GetAggDrilldownTPMTrakOperatorData_Grid",StringComparison.OrdinalIgnoreCase) && !procName.Equals("s_GetAggDrilldownTPMTrakComponentData_Grid", StringComparison.OrdinalIgnoreCase))
                        {
                            obj.McHrRate = Convert.ToDecimal(sdr["AvgMcHrRate"].ToString());
                        }
                        obj.PPM = sdr["PPM"].ToString();
                        decimal Green = 0;
                        decimal Red = 0;
                        decimal.TryParse(sdr["OEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["OERed"].ToString(), out Red);
                        if (obj.OEE >= Green)
                        {
                            obj.OEEColor = "green";
                        }
                        else if (obj.OEE <= Red && obj.OEE != 0)
                        {
                            obj.OEEColor = "red";
                        }
                        else if (obj.OEE > Red && obj.OEE < Green)
                        {
                            obj.OEEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }

                        decimal.TryParse(sdr["AEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["AERed"].ToString(), out Red);
                        if (obj.AE >= Green)
                        {
                            obj.AEColor = "green";
                        }
                        else if (obj.AE <= Red && obj.AE != 0)
                        {
                            obj.AEColor = "red";
                        }
                        else if (obj.AE > Red && obj.AE < Green)
                        {
                            obj.AEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }

                        decimal.TryParse(sdr["PEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["PERed"].ToString(), out Red);
                        if (obj.PE >= Green)
                        {
                            obj.PEColor = "green";
                        }
                        else if (obj.PE <= Red && obj.PE != 0)
                        {
                            obj.PEColor = "red";
                        }
                        else if (obj.PE > Red && obj.PE < Green)
                        {
                            obj.PEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }

                        decimal.TryParse(sdr["QEGreen"].ToString(), out Green);
                        decimal.TryParse(sdr["QERed"].ToString(), out Red);
                        if (obj.QE >= Green)
                        {
                            obj.QEColor = "green";
                        }
                        else if (obj.QE <= Red && obj.QE != 0)
                        {
                            obj.QEColor = "red";
                        }
                        else if (obj.QE > Red && obj.QE < Green)
                        {
                            obj.QEColor = "yellow";
                        }
                        else
                        {
                            obj.QEColor = "";
                        }
                        lstData.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("Table Pross Name : " + "(s_GetAggDrilldownTPMTrakData_Grid) " + stopwatch.Elapsed.TotalSeconds);
            return lstData;
        }
        #endregion

        #region "Bind Dashboard Graph YEAR information"
        public static DataTable BindDashBoardGraph(string date, string shiftName, string plantId, string machineId, string comparisonType, string parameter, string Component, string Employee, string cellId, string SortColumn, string SortOrder, string typeView)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            DataTable values = new DataTable();

            string viewType = "Machine";
            if (typeView.Equals("MachinewiseView", StringComparison.OrdinalIgnoreCase))
                viewType = "Machine";
            else if (typeView.Equals("PlantwiseView", StringComparison.OrdinalIgnoreCase))
                viewType = "plant";
            else if (typeView.Equals("ComponentwiseView", StringComparison.OrdinalIgnoreCase))
                viewType = "Component";
            else if (typeView.Equals("OperatorwiseView", StringComparison.OrdinalIgnoreCase))
                viewType = "operator";
            else if (typeView.Equals("CellwiseView", StringComparison.OrdinalIgnoreCase))
                viewType = "Cell";
            if (comparisonType.Equals("CurrentSHIFT", StringComparison.OrdinalIgnoreCase))
                SortColumn = "Groupid";
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetAggDrilldownTPMTrakData_Graph]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId =="null" ? "" : plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Component", Component);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@SortColumn", SortColumn);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.Add("@view", SqlDbType.NVarChar).Value = viewType;
                cmd.Parameters.AddWithValue("@Groupid", cellId);
                 SqlDataReader sdr = cmd.ExecuteReader();
                values.Load(sdr);
                values.AcceptChanges();
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            stopwatch.Stop();
            Logger.WriteDebugLog("Chart Pross Name (s_GetAggDrilldownTPMTrakData_Graph) : " + "(" + parameter + "parameter )" + comparisonType + " " + stopwatch.Elapsed.TotalSeconds);
            return values;
        }
        #endregion

        public static DataTable PPMFirstLeveldata(string date, string shiftName, string plantId, string machineId, string comparisonType, string Component, string Employee, string View,string Cell,string Param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            DataTable dt = new DataTable();
            List<DashboardDetails> lstData = new List<DashboardDetails>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetAggDrilldownPPM1stLevelDetails]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName = shiftName.Equals("All",StringComparison.OrdinalIgnoreCase)?"":shiftName );
                cmd.Parameters.AddWithValue("@PlantID", plantId = plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId = machineId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineId);
                cmd.Parameters.AddWithValue("@Component", Component = Component.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Component);
                cmd.Parameters.AddWithValue("@Employee", Employee = Employee.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Employee);
                cmd.Parameters.AddWithValue("@GroupID", Cell = Cell.Equals("All",StringComparison.OrdinalIgnoreCase)? "" : Cell);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@View", View);
                cmd.Parameters.AddWithValue("@IgnoreMCO", Param);
                SqlDataReader sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.AcceptChanges();

                sdr.Close();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        #region --- Kavya --
        public static List<string> GetMachinesByCell(string cellID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct machineid from PlantMachineGroups where ((GroupID in (SELECT Item FROM SplitStrings(@groupid,',')) or isnull(@groupid,'')='') )",con);
                cmd.Parameters.AddWithValue("@groupid", cellID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["machineid"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        public static List<string> GetComponentsByMachine(string MachineID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct componentid from componentoperationpricing where ((machineid in (SELECT Item FROM SplitStrings(@MachineID,',')) or isnull(@machineid,'')='') )", con);
                cmd.Parameters.AddWithValue("@machineid", MachineID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["componentid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        public static List<string> GetOperationsByComponent(string ComponentID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct operationno from componentoperationpricing where ((componentid in (SELECT Item FROM SplitStrings(@componentid,',')) or isnull(@componentid,'')='') )", con);
                cmd.Parameters.AddWithValue("@componentid", ComponentID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["operationno"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        public static List<string> GetAllComponent()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> componentList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct componentid from componentinformation where componentid <> '' order by componentid", sqlConn);
                //shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    componentList.Add(rdr["componentid"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return componentList;
        }
        public static List<string> GetAllOperationData(string componentID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> operationList = new List<string>();
            try
            {
                SqlCommand cmd;
                if (componentID.Trim() == "ALL" || componentID.Trim() == "")
                {
                    cmd = new SqlCommand(@"select distinct operationno from componentoperationpricing order by operationno", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"select distinct operationno from componentoperationpricing where componentid=@comp order by operationno", sqlConn);
                    cmd.Parameters.AddWithValue("@comp", componentID);
                }

                //shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    operationList.Add(rdr["operationno"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return operationList;
        }
        public static List<string> GetOperatorDataForPlant(string plant)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> operationList = new List<string>();
            try
            {
                SqlCommand cmd;
                if (plant.Trim() == "ALL" || plant.Trim() == "")
                {
                    cmd = new SqlCommand(@"select distinct e.employeeid from employeeinformation e left join PlantEmployee p on e.Employeeid=p.EmployeeID ", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"select distinct e.employeeid from employeeinformation e left join PlantEmployee p on e.Employeeid=p.EmployeeID where p.PlantID=@plant", sqlConn);
                    cmd.Parameters.AddWithValue("@plant", plant);
                }

                //shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    operationList.Add(rdr["employeeid"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return operationList;
        }
        public static List<string> GetAllCategory()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> categoryList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct catagory from rejectioncodeinformation where catagory is not null order by catagory", sqlConn);
                //shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    categoryList.Add(rdr["catagory"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return categoryList;
        }
        public static List<string> GetAllRejectionData(string categoryID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> rejectionList = new List<string>();
            try
            {
                SqlCommand cmd;
                if (categoryID.Trim() == "ALL" || categoryID.Trim() == "")
                {
                    cmd = new SqlCommand(@"select distinct rejectionid from rejectioncodeinformation order by rejectionid", sqlConn);
                }
                else
                {
                    cmd = new SqlCommand(@"select distinct rejectionid from rejectioncodeinformation where catagory=@cat order by rejectionid", sqlConn);
                    cmd.Parameters.AddWithValue("@cat", categoryID);
                }

                //shiftList.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    rejectionList.Add(rdr["rejectionid"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //ErrorSignal.FromCurrentContext().Raise(ex);
                throw ex;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return rejectionList;
        }
        #endregion

        #region "Bind Focus Tools Life Data"
        public static List<Tuple<string, string>> FocusToolLifeData(string groupId)
        {
            List<Tuple<string, string>> toolLifeVale = new List<Tuple<string, string>>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"Select distinct FT.Toolno+' <'+T.ToolDescription+'>' as ToolNoDesc,FT.ToolNo from Focas_ToolLife FT
                                            inner join ProcessMachineMaster_ToolReport P on FT.machineid=P.Machineid
                                            inner join ToolSequence T on T.MachineID=FT.machineid and T.ToolNo=FT.ToolNo
                                          where P.process=@GroupId", sqlConn);
                cmd.Parameters.AddWithValue("@GroupId", groupId);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Tuple<string, string> value = new Tuple<string, string>(rdr["ToolNoDesc"].ToString(), rdr["ToolNo"].ToString());
                    toolLifeVale.Add(value);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return toolLifeVale;
        }
        #endregion

        #region "Bind Focus Tools Life Data"
        public static List<string> GetToolNumberForMachine(string machineId)
        {
            List<string> toolLifeVale = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"  select distinct ToolNo  from ToolSequence where  MachineID=@machineID", sqlConn);
                cmd.Parameters.AddWithValue("machineID", machineId);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    
                    toolLifeVale.Add(rdr["ToolNo"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return toolLifeVale;
        }
        #endregion

        public static List<string> GetAllLines(string plantId)
        {
            plantId = plantId == "ALL" ? "" : plantId;
            plantId = plantId == "All" ? "" : plantId;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> listOfLines = new List<string>();
            try
            {
                // sqlConn.Open();
                cmd = new SqlCommand(@"s_GetLookups", sqlConn);

                if (!string.IsNullOrEmpty(plantId))
                {
                    if (plantId.Equals("All", StringComparison.OrdinalIgnoreCase)) plantId = string.Empty;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"name", "Group");
                    cmd.Parameters.AddWithValue(@"filter", plantId);
                }
                else
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(@"name", "Group");
                }

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        listOfLines.Add(rdr["GroupID"].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return listOfLines;
        }
        public static string GetCombinedChartNames_OEEDAshboard()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string ChartType = "";
            try
            {
                cmd = new SqlCommand(@"select ValueInText from CockpitDefaults where Parameter='HistoricalDashboardCombinedCharts'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ChartType = sdr["ValueInText"].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return ChartType;
        }
        public static bool IsChartCombined_OEEDAshboard()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            bool isCombined = false;
            try
            {
                cmd = new SqlCommand(@"select ValueInText from CockpitDefaults where Parameter='HistoricalDashboardChartType'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("Separate", StringComparison.OrdinalIgnoreCase))
                        {
                            isCombined =false;
                        }
                        else
                        {
                            isCombined = true;
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return isCombined;
        }
    }
}
