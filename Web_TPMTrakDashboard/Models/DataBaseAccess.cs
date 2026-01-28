using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Elmah;
using System.Data;
using MachineStatusPage.Models;
using System.Configuration;
using System.Text;
using Web_TPMTrakDashboard.ShantiIron;
using System.Web.Configuration;
using Web_TPMTrakDashboard.ShantiIron.Model;
using System.Web.UI.WebControls;
using System.IO;

namespace Web_TPMTrakDashboard.Models
{
    public class DataBaseAccess
    {

        #region "------Window Authentication--------------"
        static public string WindowAuthentication
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["WindowAuthenticationForLoginPage"].ToString();
            }
        }

        #endregion

        static public string AutoRefreshData
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["AutoRefresh"].ToString();
            }
        }

        static public List<string> ServerData()
        {
            List<string> connectionStrings = new List<string>();
            int count = WebConfigurationManager.ConnectionStrings.Count;
            for (int i = 0; i < count; i++)
            {
                connectionStrings.Add(WebConfigurationManager.ConnectionStrings[i].Name);
            }
            return connectionStrings;
        }

        internal static List<string> GetPlantsFromDB()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader rdr = default;
            try
            {
                SqlCommand cmd = new SqlCommand("select PlantID from PlantInformation", con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["PlantID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMachinesForAParticularPlant: " + ex.ToString());
            }
            finally
            {
                rdr?.Close();
                con?.Close();
            }
            return list;
        }

        internal static List<string> GetMachinesForAParticularPlant(string plant)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader rdr = default;
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct MachineID from PlantMachine where PlantID=@PlantID", con);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["MachineID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMachinesForAParticularPlant: " + ex.ToString());
            }
            finally
            {
                rdr?.Close();
                con?.Close();
            }
            return list;
        }

        internal static void LoadInitialProgramTransferSettings(out string path, out string fileFormat)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = default;
            path = string.Empty;
            fileFormat = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from Focas_Defaults where ValueInText in ('ProgramsPath','ProgramFileExtention') and Parameter = 'FocasAppSettings'", sqlConn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["ValueInText"].ToString().Equals("ProgramsPath", StringComparison.OrdinalIgnoreCase))
                        path = rdr["ValueInText2"].ToString();
                    if (rdr["ValueInText"].ToString().Equals("ProgramFileExtention", StringComparison.OrdinalIgnoreCase))
                        fileFormat = rdr["ValueInText2"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("LoadInitialProgramTransferSettings: " + ex.ToString());
            }
            finally
            {
                rdr?.Close();
                sqlConn?.Close();
            }
        }

        internal static List<string> GetAllPONumber(DateTime fromDate, DateTime toDate, string PONo)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> PONoLst = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                string PONum = PONo.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PONo;
                if (!string.IsNullOrEmpty(PONo))
                {
                    sqlQuery = "select distinct ProductionOrder from ProductionOrder_CUMI where ProductionOrder like '%" + PONum + "%'";
                }
                else
                {
                    sqlQuery = "select distinct ProductionOrder from ProductionOrder_CUMI";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONoLst.Add(Convert.ToString(rdr["ProductionOrder"]));
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
            return PONoLst;
        }

        internal static void AddMachineToGroupID(string machineID, string Group)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "insert into MaintenanceGroupMachine_Precision(GroupID,MachineID)values(@GroupID, @MachineID)";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@GroupID", Group);
                recordsAffected = cmd.ExecuteNonQuery();
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

        internal static void DeleteMachineToGroup(string machineID, string GroupID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "delete from MaintenanceGroupMachine_Precision where GroupID = @GroupID and MachineID = @machineID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                recordsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static void GetProgTransferMachineDetailsFromDB(string MacID, out string dNCIP, out string dNCIPPort, out string oPCUAUrl, out bool programFoldersEnabled)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader rdr = default;
            dNCIP = string.Empty;
            dNCIPPort = string.Empty;
            oPCUAUrl = string.Empty;
            programFoldersEnabled = false;
            try
            {
                SqlCommand cmd = new SqlCommand("select DNCIP,DNCIPPortNo,OPCUAUrl,ProgramFoldersEnabled from machineinformation where machineid=@MacID", con);
                cmd.Parameters.AddWithValue("@MacID", MacID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    dNCIP = rdr["DNCIP"].ToString();
                    dNCIPPort = rdr["DNCIPPortNo"].ToString();
                    oPCUAUrl = rdr["OPCUAUrl"].ToString();
                    programFoldersEnabled = DBNull.Value != rdr["ProgramFoldersEnabled"] && Convert.ToBoolean(rdr["ProgramFoldersEnabled"]);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProgTransferMachineDetailsFromDB: " + ex.ToString());
            }
            finally
            {
                rdr?.Close();
                con?.Close();
            }
        }

        internal static List<string> GetMRPController()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct MRP_Controller from ACE_SAP_ScheduleDetails where MRP_Controller is not null", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            list.Add(sdr["MRP_Controller"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getAllMRPController- " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAllMRPController - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        internal static List<string> GetOrderedLabels(out List<string> listOfColNames)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            listOfColNames = new List<string>();

            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from dbo.AndonDefaults where [Parameter] = @param and ValueInBool=1", sqlConn);
                cmd.Parameters.AddWithValue("@param", "WebTPMTrakIconicView");
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["ValueInText2"].ToString());
                    listOfColNames.Add(rdr["ValueInText"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        internal static List<string> GetCurrentOrPreviousShiftVals(string procName)
        {
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = procName;
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["StartTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (!Convert.IsDBNull(sdr["EndTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (!Convert.IsDBNull(sdr["dDate"]))
                        {
                            list.Add(DateTime.Parse(sdr["dDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (!Convert.IsDBNull(sdr["ShiftName"]))
                        {
                            list.Add(sdr["ShiftName"].ToString());
                        }
                    }
                }
                else
                {
                    list = null;
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

        internal static DataTable getComponentMasterDetails(string PlantID, string cellId, string machineID)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = @"select distinct c.componentid,co.machineid,p1.GroupID,c.InterfaceID as CompInterfaceID,c.description as CompDesc,co.InterfaceID as OpnInterfaceID,co.operationno,
                            co.description as OpnDesc, (co.machiningtime) as cycletime, (co.cycletime-co.machiningtime) as loadunload,co.price,co.drawingno,co.SubOperations,co.StdSetupTime,
                            co.MachiningTimeThreshold,co.TargetPercent,co.loadunload as loadUnloadTimeThreshold, c.customerid,co.SCIThreshold,co.DCLThreshold,co.FinishedOperation,co.MinLoadUnloadThreshold
                            from componentinformation c
                            inner join componentoperationpricing co on c.componentid = co.componentid
                            left outer join PlantMachineGroups p1 on p1.MachineID=co.machineid
                            where (p1.GroupID in (" + cellId + ")) and (p1.MachineID in (" + machineID + ")) and (PlantID=@plantID or ISNULL(@plantID, '') = '')";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@CellID", cellId);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@plantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        internal static List<string> GetCurrentShiftVals(string procName)
        {
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = procName;
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["StartTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (!Convert.IsDBNull(sdr["EndTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        list.Add(sdr["ShiftName"].ToString());
                    }
                }
                else
                {
                    list = null;
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


        internal static List<string> GetOperationByQuotes(string MachineID, string ComponentID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Operation = new List<string>();
            string Query = @"";
            MachineID = MachineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : MachineID;
            ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID;
            if (string.IsNullOrEmpty(ComponentID) && string.IsNullOrEmpty(MachineID))
            {
                Query = @"select distinct operationno from componentoperationpricing";
            }
            else if (string.IsNullOrEmpty(ComponentID))
            {
                Query = @"select distinct operationno from componentoperationpricing where machineid in (" + MachineID + ")";
            }
            else if (string.IsNullOrEmpty(MachineID))
            {
                Query = @"select distinct operationno from componentoperationpricing where componentid in (" + ComponentID + ")";
            }
            else
            {
                Query = @"select distinct operationno from componentoperationpricing where machineid in(" + MachineID + ") and componentid in (" + ComponentID + ")";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@componentid", ComponentID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Operation.Add(rdr["operationno"].ToString());
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
            return Operation;
        }

        internal static bool UpdatePTSettingtoDB(string folderPath, string fileExtension)
        {
            bool res = false;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("Update Focas_Defaults set ValueInText2 = @ProgramFileExtention where ValueInText = 'ProgramFileExtention' and Parameter = 'FocasAppSettings'", con);
                cmd.Parameters.AddWithValue("@ProgramFileExtention", fileExtension);
                cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand("Update Focas_Defaults set ValueInText2 = @ProgramsPath where ValueInText = 'ProgramsPath' and Parameter = 'FocasAppSettings'", con);
                cmd2.Parameters.AddWithValue("@ProgramsPath", folderPath);
                cmd2.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdatePTSettingtoDB: " + ex.Message);
            }
            finally
            {
                con?.Close();
            }
            return res;
        }

        internal static string GetMachineDescription(string machineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string data = string.Empty;
            string query = string.Empty;
            query = @"SELECT distinct description FROM [dbo].[machineinformation] where machineid =@machineID";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@machineID", machineId);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = rdr["description"].ToString();
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
            return data;
        }

        internal static string GetDefaultCockpitDefaultShift()
        {
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            ICockpitStyle vals = new ICockpitStyle();
            try
            {
                cmd = new SqlCommand("select ValueInText from Cockpitdefaults where parameter = 'DefaultShift'", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        currentShift = sdr["ValueInText"].ToString();
                    }
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
            return currentShift;
        }

        internal static bool IsModifiedColumnVisibility(string Param)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            bool isVisible = false;
            try
            {
                cmd = new SqlCommand(@"select ValueInText from CockpitDefaults where Parameter=@Parameter", con);
                cmd.Parameters.AddWithValue("@Parameter", Param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        isVisible = sdr["ValueInText"].ToString().Equals("Y", StringComparison.OrdinalIgnoreCase) ? true : false;
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
            return isVisible;
        }
        internal static string UpdateModifiedColumnVisibility_CockpitDefaults(string Param, string valueInText, int valueinBool)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"update CockpitDefaults set ValueInBool=@ValueInBool where Parameter=@Parameter and ValueInText=@ValueInText
select 'Updated' as SaveFlag", con);
                cmd.Parameters.AddWithValue("@Parameter", Param);
                cmd.Parameters.AddWithValue("@ValueInText", valueInText);
                cmd.Parameters.AddWithValue("@ValueInBool", valueinBool);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
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
            return success;
        }
        internal static List<ColumnViewSetting> BindSettingPage(string param, string LanguageSpecified)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ColumnViewSetting> lstModelData = new List<ColumnViewSetting>();
            try
            {
                int i = 1;
                SqlCommand cmd = new SqlCommand("select * from dbo.cockpitdefaults where [Parameter] = @Param and LanguageSpecified=@LanguageSpecified order by ValueInInt asc", sqlConn);
                cmd.Parameters.AddWithValue("@param", param);
                cmd.Parameters.AddWithValue("@LanguageSpecified", LanguageSpecified);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ColumnViewSetting modelData = new ColumnViewSetting();
                    modelData.Parameter = rdr["Parameter"].ToString();
                    modelData.ValueInText = rdr["ValueInText"].ToString();
                    modelData.ValueInText2 = rdr["ValueInText2"].ToString();
                    if (string.IsNullOrEmpty(rdr["ValueInInt"].ToString()))
                        modelData.ValueInInt = 0;
                    else
                        modelData.ValueInInt = (int)rdr["ValueInInt"];
                    if (rdr["ValueInBool"].ToString() == "1")
                    {
                        modelData.ValueInBool = true;
                        modelData.Display = "";
                        modelData.Setvalue = 1;
                    }
                    else
                    {
                        modelData.ValueInBool = false;
                        modelData.Display = "none";
                        modelData.Setvalue = 2;
                    }
                    if (param.Equals("WebTPMTrak", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ConfigurationManager.AppSettings["sonapages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            if (modelData.ValueInText.Equals("Rejected", StringComparison.OrdinalIgnoreCase) || modelData.ValueInText.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                            {
                                modelData.ValueInBool = false;
                                modelData.Display = "none";
                                modelData.Setvalue = 2;
                            }
                        }

                    }
                    modelData.ValueInInt = Convert.ToInt32(rdr["ValueInInt"]);
                    i++;
                    lstModelData.Add(modelData);
                }
                rdr.Close();
            }
            catch (Exception)
            {
                // throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstModelData;
        }
        internal static string GetModifiedDataBackColor()
        {
            string color = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select ValueInText from CockpitDefaults where Parameter='VDGModifiedDataBackColor'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        color = sdr["ValueInText"].ToString();
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
            return color;
        }
        internal static bool GetModifiedDataLog(string Parameter)
        {
            bool isEnabled = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select ValueInText from CockpitDefaults where Parameter=@Parameter", con);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        isEnabled = sdr["ValueInText"].ToString().Equals("Y", StringComparison.OrdinalIgnoreCase) ? true : false;
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
            return isEnabled;
        }
        internal static string GetDownCode_Autodata(string ID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string dcode = "";
            try
            {
                cmd = new SqlCommand(@"select * from autodata  where id=@id and dcode is not null", con);
                cmd.Parameters.AddWithValue("@id", ID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        dcode = sdr["dcode"].ToString();
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
            return dcode;
        }
        internal static List<ModifiedData_VDG> GetModifiedDataHistory_VDG(string ID, string Param)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<ModifiedData_VDG> list = new List<ModifiedData_VDG>();
            ModifiedData_VDG data = null;
            string dcode = GetDownCode_Autodata(ID);
            try
            {
                if (dcode != "")
                {
                    cmd = new SqlCommand(@"SELECT A1.RecordType, M.machineid,C.componentid,O.operationno,E.Employeeid,D.downdescription,stdate,A1.sttime,nddate,ndtime,A1.datatype, A2.cycletime,A2.loadunload,compslno,post,
msttime,A1.PartsCount, WorkOrderNumber, KWH, NetKWH, ActionTaken, A2.SerialNumber, HeatCode, SplString1, SplString2, A1.UpdatedBy, A1.UpdatedTS
FROM Autodata_AuditLogDetails A1
INNER JOIN autodata A2 ON A1.mc = A2.mc and A1.sttime = A2.sttime and A1.datatype = A2.datatype
INNER JOIN machineinformation M ON M.InterfaceID = A1.mc
LEFT JOIN componentinformation C ON A1.comp = C.InterfaceID 
LEFT JOIN componentoperationpricing O ON A1.opn = O.InterfaceID and C.componentid = O.componentid and O.machineid = M.machineid
LEFT JOIN employeeinformation E ON E.interfaceid = A1.opr
LEFT JOIN downcodeinformation D ON D.interfaceid=A1.dcode
WHERE A2.id = @id
ORDER BY A1.AutoID DESC", con);
                }
                else
                {
                    cmd = new SqlCommand(@"SELECT A1.RecordType, M.machineid,C.componentid,O.operationno,E.Employeeid,A1.dcode,stdate,A1.sttime,nddate,ndtime,A1.datatype, A2.cycletime,A2.loadunload,compslno,post,
msttime,A1.PartsCount, WorkOrderNumber, KWH, NetKWH, ActionTaken, A2.SerialNumber, HeatCode, SplString1, SplString2, A1.UpdatedBy, A1.UpdatedTS
FROM Autodata_AuditLogDetails A1
INNER JOIN autodata A2 ON A1.mc = A2.mc and A1.sttime = A2.sttime and A1.datatype = A2.datatype
INNER JOIN machineinformation M ON M.InterfaceID = A1.mc
LEFT JOIN componentinformation C ON A1.comp = C.InterfaceID 
LEFT JOIN componentoperationpricing O ON A1.opn = O.InterfaceID and C.componentid = O.componentid and O.machineid = M.machineid
LEFT JOIN employeeinformation E ON E.interfaceid = A1.opr
WHERE A2.id = @id
ORDER BY A1.AutoID DESC", con);
                }

                cmd.Parameters.AddWithValue("@id", ID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new ModifiedData_VDG();
                        data.RecordType = sdr["RecordType"].ToString();
                        data.Component = sdr["componentid"].ToString();
                        data.Operation = sdr["operationno"].ToString();
                        data.Operator = sdr["Employeeid"].ToString();
                        data.PartsCount = Math.Round(Convert.ToDecimal(sdr["PartsCount"].ToString()), 2);
                        if (dcode != "")
                        {
                            data.DownID = sdr["downdescription"].ToString();
                        }
                        else
                        {
                            data.DownID = sdr["dcode"].ToString();
                        }
                        data.UpdatedTS = Convert.ToDateTime(sdr["UpdatedTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss tt");
                        list.Add(data);
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
            return list;
        }
        #region "Update View Column Setting Records "
        internal static int UpdateColumnViewSettings(string Param, string valueInText2, string valueInText, int ValueInBool, string LanguageSpecified)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int result = 0;
            try
            {
                SqlCommand cmd = null;
                cmd = new SqlCommand("update cockpitdefaults set  [ValueInText2] = @ValueInText2 , [ValueInBool] = @ValueInBool,UpdatedTS=@UpdatedTS where [Parameter] = @Param and ValueInText = @ValueInText and [LanguageSpecified]=@LanguageSpecified", sqlConn);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
                cmd.Parameters.AddWithValue("@ValueInText", valueInText);
                cmd.Parameters.AddWithValue("@ValueInBool", ValueInBool);
                cmd.Parameters.AddWithValue("@LanguageSpecified", LanguageSpecified);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                                                           // throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return result;
        }
        #endregion

        internal static int UpdateColumnViewSettingsIonic(string Param, string valueInText2, string valueInText, int ValueInBool, int ValueInInt, string LanguageSpecified)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int result = 0;
            try
            {
                SqlCommand cmd = null;
                cmd = new SqlCommand("update cockpitdefaults set  [ValueInText2] = @ValueInText2 ,[ValueInInt] = @ValueInInt, [ValueInBool] = @ValueInBool,UpdatedTS=@UpdatedTS where [Parameter] = @Param and ValueInText = @ValueInText and [LanguageSpecified]=@LanguageSpecified", sqlConn);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
                cmd.Parameters.AddWithValue("@ValueInText", valueInText);
                cmd.Parameters.AddWithValue("@ValueInBool", ValueInBool);
                cmd.Parameters.AddWithValue("@ValueInInt", ValueInInt);
                cmd.Parameters.AddWithValue("@LanguageSpecified", LanguageSpecified);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                                                           // throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return result;
        }

        #region "Populate Down Time Value "
        internal static string GetDownTimeText(string LanguageSpecified)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string downTimeText = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ValueInText2 from cockpitdefaults where Parameter = 'WebTPMTrak' and LanguageSpecified=@LanguageSpecified and valueInText='Downtime/Stoppage(min/hh:mm)'", sqlConn);
                cmd.Parameters.AddWithValue("@LanguageSpecified", LanguageSpecified);
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                downTimeText = rdr["ValueInText2"].ToString();
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return downTimeText;
        }
        #endregion

        internal static List<ColumnViewSetting> GetTblHeader()
        {
            List<ColumnViewSetting> hdr = new List<ColumnViewSetting>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("select ValueInText2,ValueInBool from dbo.AndonDefaults where [Parameter] = @Param", sqlConn);
                cmd.Parameters.AddWithValue("@param", "WebTPMTrak");
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ColumnViewSetting modelData = new ColumnViewSetting();
                    modelData.ValueInText2 = rdr["ValueInText2"].ToString();
                    modelData.ValueInBool = Convert.ToBoolean(rdr["ValueInBool"]);
                    hdr.Add(modelData);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return hdr;
        }

        #region "UpdateApplication Setting Records"
        internal static int applicationSettings(string Param, string valueIntext, string valueInText2, string LanguageSpecified)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                if (Param == "WebTPMTrak")
                    cmd = new SqlCommand(@"update cockpitdefaults set  [ValueInText2] = @ValueInText2,UpdatedTS=@UpdatedTS  where [Parameter] = @Param and ValueInText = @ValueInText and LanguageSpecified=@LanguageSpecified", sqlConn);
                else if (Param == "ExcludeTPMTrakDown")
                    cmd = new SqlCommand(@"Update [CockpitDefaults] set [ValueInInt]=@ValueInInt,UpdatedTS=@UpdatedTS where [Parameter] = @Param", sqlConn);
                else
                    cmd = new SqlCommand(@"update cockpitdefaults set [ValueInText2] = @ValueInText2,UpdatedTS=@UpdatedTS where [Parameter] = @Param and [ValueInText] =@ValueInText", sqlConn);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
                cmd.Parameters.AddWithValue("@ValueInText", valueIntext);
                if (Param == "WebTPMTrak")
                    cmd.Parameters.AddWithValue("@LanguageSpecified", LanguageSpecified);
                if (Param == "ExcludeTPMTrakDown")
                    cmd.Parameters.AddWithValue("@ValueInInt", Convert.ToInt32(valueInText2));
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return result;
        }
        #endregion

        #region ----------Globel View Setting Part--------------
        internal static AppUISettings ViewAppUISettings()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            AppUISettings settings = new AppUISettings();
            try
            {
                cmd = new SqlCommand(@"select * from cockpitdefaults where Parameter = 'TPMTrakAppSettings'  ", sqlConn);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("FormFontSize", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.FontSize = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("FontFamily", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.FontFamily = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("CockpitFontSize", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.CockpitFontSize = (sdr["ValueInText2"]).ToString();
                            if (sdr["ValueInText2"] != DBNull.Value)
                                settings.OuterCockpitFontSize = (Convert.ToInt32(sdr["ValueInText2"]) + 4).ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("FontStyle", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.FontStyle = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("DownTime", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.DownTime = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ShowSmileyImage", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.ShowSmileyImage = sdr["ValueInText2"].ToString();
                            settings.ShowSmileyBlock = settings.ShowSmileyImage == "1" ? "show" : "none";
                        }
                        else if (sdr["ValueInText"].ToString().Equals("SmileyImageSize", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.SmileyImageSize = sdr["ValueInText2"].ToString();
                            settings.SmileyBlockSize = settings.SmileyImageSize + "px";
                        }
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return settings;
        }

        internal static string getCellID(string MachineID, string PlantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string list = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select PMG.GroupID from PlantMachineGroups PMG  where pmg.Machineid=@Machineid and (pmg.PlantID=@plantID or ISNULL(@plantID, '') = '')", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", MachineID);
                cmd.Parameters.AddWithValue("@plantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list = rdr["GroupID"].ToString();
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
            return list;
        }
        #endregion

        #region "Table Header Informaction"
        internal static List<string> GetOrderedTableView(out List<string> listOfColNames, string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            listOfColNames = new List<string>();

            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from dbo.cockpitdefaults where [Parameter] = @param and ValueInBool=1", sqlConn);
                cmd.Parameters.AddWithValue("@param", "WebTableViewAndon");
                //cmd.Parameters.AddWithValue("@User", user);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["ValueInText2"].ToString());
                    listOfColNames.Add(rdr["ValueInText"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        internal static bool GetCompanyName()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool Status = false;

            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select count(*) as status from company where CompanyName Like '%KTA%'";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                Int32 count = (Int32)cmd.ExecuteScalar();
                if (count > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return Status;
        }
        #endregion

        #region "Table Item Information Data"
        internal static DataTable GetMachinetableview(string procName, string fromDateTime, string todate, string shiftId, string machineId, string plantId, string Line, string Param, string user, string mode)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(procName, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);//fromDateTime
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                //cmd.Parameters.AddWithValue("@Shift", shiftId);
                cmd.Parameters.AddWithValue("@GroupID", Line);
                //cmd.Parameters.AddWithValue("@Param", Param);
                //cmd.Parameters.AddWithValue("@UserID", user);
                //cmd.Parameters.AddWithValue("@Type", mode);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.Columns.Add("Pecolor"); dt.Columns.Add("Oeecolor"); dt.Columns.Add("Aecolor"); dt.Columns.Add("Qecolor");
                    foreach (DataRow row in dt.Rows)
                    {
                        //    if (Param.Equals("MachinewiseDetails", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        if (row["PingStatus"].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                        //            row["MachineStatus"] = "Image/McStatus/ping.png";
                        //        else if (row["ConnectionStatus"].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                        //            row["MachineStatus"] = "Image/McStatus/ping.png";
                        //        else if (row["MachineStatus"].ToString().Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        //            row["MachineStatus"] = "Image/McStatus/Stopped.gif";
                        //        else if (row["MachineStatus"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase))
                        //            row["MachineStatus"] = "Image/McStatus/Running.gif";
                        //    }
                        //    else if (row["MachineStatus"].ToString().Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        //        row["MachineStatus"] = "Image/McStatus/Stopped.gif";
                        //    else if (row["MachineStatus"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase))
                        //        row["MachineStatus"] = "Image/McStatus/Running.gif";
                        double PEGreen = 0.0, PERed = 0.0, AEGreen = 0.0, AERed = 0.0, OEEGreen = 0.0, OEERed = 0.0, QEGreen = 0.0, QERed = 0.0, PE = 0.0, QE = 0.0, OEE = 0.0, AE = 0.0;
                        PE = Convert.ToDouble(row["ProductionEfficiency"].ToString()); PEGreen = Convert.ToDouble(row["PEGreen"].ToString()); PERed = Convert.ToDouble(row["PERed"].ToString());
                        AE = Convert.ToDouble(row["AvailabilityEfficiency"].ToString()); AEGreen = Convert.ToDouble(row["AEGreen"].ToString()); AERed = Convert.ToDouble(row["AERed"].ToString());
                        QE = Convert.ToDouble(row["QualityEfficiency"].ToString()); QEGreen = Convert.ToDouble(row["QEGreen"].ToString()); QERed = Convert.ToDouble(row["QERed"].ToString());
                        OEE = Convert.ToDouble(row["OverAllEfficiency"].ToString()); OEEGreen = Convert.ToDouble(row["OEGreen"].ToString()); OEERed = Convert.ToDouble(row["OERed"].ToString());
                        if (PE == 0)
                        {
                            row["Pecolor"] = "white";
                        }
                        else if (PE > PEGreen)
                        {
                            row["Pecolor"] = "Green";
                        }
                        else if (PE < PERed)
                        {
                            row["Pecolor"] = "Red";
                        }
                        else if (PE <= PERed || PE >= PEGreen)
                        {
                            row["Pecolor"] = "Yellow";
                        }
                        if (QE == 0)
                        {
                            row["Qecolor"] = "white";
                        }
                        else if (QE > QEGreen)
                        {
                            row["Qecolor"] = "Green";
                        }
                        else if (QE < QERed)
                        {
                            row["Qecolor"] = "Red";
                        }
                        else if (QE <= QERed || QE >= QEGreen)
                        {
                            row["Qecolor"] = "Yellow";
                        }
                        if (AE == 0)
                        {
                            row["Aecolor"] = "white";
                        }
                        else if (AE > AEGreen)
                        {
                            row["Aecolor"] = "Green";
                        }
                        else if (AE < AERed)
                        {
                            row["Aecolor"] = "Red";
                        }
                        else if (AE <= PERed || AE >= PEGreen)
                        {
                            row["Aecolor"] = "Yellow";
                        }
                        if (OEE == 0)
                        {
                            row["Oeecolor"] = "white";
                        }
                        else if (OEE > OEEGreen)
                        {
                            row["Oeecolor"] = "Green";
                        }
                        else if (OEE < OEERed)
                        {
                            row["Oeecolor"] = "Red";
                        }
                        else if (OEE <= OEERed || OEE >= OEEGreen)
                        {
                            row["Oeecolor"] = "Yellow";
                        }

                    }
                    dt.AcceptChanges();
                }

            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error in retriving Machine - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
        #endregion

        internal static List<CockpitData> GetMachineCockpitDetails(string procName, string fromDateTime, string shiftId, string machineId, string plantId, string Param, List<string> Orderlist,
        List<string> sortOrder, string settings, string groupName, string user, string mode)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            string utilizedTime = string.Empty;
            SqlCommand cmd = null;
            CockpitData list = null;
            List<CockpitData> listOfVals = new List<CockpitData>();
            try
            {
                cmd = new SqlCommand(procName, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDateTime);//fromDateTime
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@PlantId", plantId);
                cmd.Parameters.AddWithValue("@Shift", shiftId);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@GroupID", groupName);
                cmd.Parameters.AddWithValue("@UserID", user);
                cmd.Parameters.AddWithValue("@Type", mode);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    list = new CockpitData();
                    CockpitUserControlData listData = null;
                    list.MachineId = Convert.ToString(sdr["MachineId"]);
                    //TODO Ping,connection, machineStatus
                    if (Param.Equals("MachinewiseDetails", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            if (sdr["PingStatus"].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                                list.MachineStatus = sdr["PingStatus"].ToString();
                            else if (sdr["ConnectionStatus"].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                                list.MachineStatus = sdr["ConnectionStatus"].ToString();
                            else
                                list.MachineStatus = sdr["MachineStatus"].ToString();// Convert.ToString(sdr["MachineStatus"] == null ? "" : sdr["MachineStatus"]);
                        }
                        catch
                        {
                            list.MachineStatus = sdr["MachineStatus"].ToString();
                        }
                    }
                    else
                    {
                        list.MachineStatus = sdr["MachineStatus"].ToString();
                    }
                    //if (BindCockpitView.GetCompany == "1")
                    //    list.MachineOEE = sdr["OeColor"].ToString();
                    //else
                    list.MachineOEE = sdr["SmileyColor"].ToString();

                    if (list.MachineOEE.Equals("white", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Deault.png";
                    else if (list.MachineOEE.Equals("Green", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Green.png";
                    else if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Yellow.png";
                    else if (list.MachineOEE.Equals("Red", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Red.png";

                    if (list.MachineStatus.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        list.StatusImage = "Image/McStatus/Stopped.gif";//list.StatusImage = "Image/McStatus/Stopped.gif";
                    else if (list.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        list.StatusImage = "Image/McStatus/Running.gif";//list.StatusImage = "Image/McStatus/Running.gif";
                    else if (list.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                        list.StatusImage = "Image/McStatus/PDT.gif";
                    else if (list.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                    {
                        if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || list.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                            list.StatusImage = "Image/McStatus/ping(4).png";
                        else
                            list.StatusImage = "Image/McStatus/ping(2).png";
                    }

                    if (Param.Equals("GroupwiseDeatails", StringComparison.OrdinalIgnoreCase))
                    {
                        list.GroupName = sdr["Machineid"].ToString();
                    }
                    // list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
                    if (string.IsNullOrEmpty(machineId))
                    {
                        for (int i = 0; i < Orderlist.Count; i++)
                        {
                            listData = new CockpitUserControlData();
                            listData.BackColor = string.Empty;
                            if (sdr[Orderlist[i]].GetType() == typeof(double))
                            {
                                listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(sdr[Orderlist[i]]));
                            }
                            else
                            {
                                listData.LabelValue = sdr[Orderlist[i]].ToString();
                            }
                            listData.ColorProperties = "#0033cc";
                            if (Orderlist[i].Equals("RunningComponent", StringComparison.OrdinalIgnoreCase))
                            {
                                if (listData.LabelValue.Equals("Part not defined for MC", StringComparison.OrdinalIgnoreCase)
                                    || listData.LabelValue.Equals("Part not defined", StringComparison.OrdinalIgnoreCase))
                                    listData.ColorProperties = "#FF0000";
                            }

                            if ((Orderlist[i].Equals("Components", StringComparison.OrdinalIgnoreCase) ||
                                //Orderlist[i].Equals("QtyGap", StringComparison.OrdinalIgnoreCase) ||
                                Orderlist[i].Equals("CurrentTimeShiftTarget", StringComparison.OrdinalIgnoreCase) ||
                                Orderlist[i].Equals("CurrentTimeQtyGap", StringComparison.OrdinalIgnoreCase)) && mode == "AndonMode")
                            {
                                listData.LabelText = sortOrder[i] + " @ " + DateTime.Now.ToString("hh:mm tt");
                            }
                            else
                            {
                                listData.LabelText = sortOrder[i];
                            }
                            listData.Tag = Orderlist[i];

                            if (Orderlist[i].Equals("AvailabilityEfficiency"))
                            {
                                listData.BackColor = Convert.ToString(sdr["AeColor"]);
                            }
                            else if (Orderlist[i].Equals("ProductionEfficiency"))
                            {
                                listData.BackColor = Convert.ToString(sdr["PeColor"]);
                            }
                            else if (Orderlist[i].Equals("OverAllEfficiency"))
                            {
                                listData.BackColor = Convert.ToString(sdr["OeColor"]);
                            }
                            list.Values.Add(listData);
                        }
                        listOfVals.Add(list);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return listOfVals;
        }

        #region -------Login Form---------------
        internal static string CheckEmployeeDetail(string userName, string password)
        {
            string adminData = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = " select isadmin from dbo.employeeinformation where [EmployeeId] = @employeeId and [upassword] = @upassword";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue(@"employeeId", userName);
                cmd.Parameters.AddWithValue(@"Upassword", password);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    sdr.Read();
                    {
                        adminData = sdr["isadmin"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//(ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return adminData;
        }

        internal static string CheckEmployeeExists(string userName)
        {
            string adminData = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select isadmin from dbo.employeeinformation where [EmployeeId] = @employeeId";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue(@"employeeId", userName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    sdr.Read();
                    {
                        adminData = (sdr["isadmin"]).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//(ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return adminData;
        }
        internal static string getLandingPage()
        {
            string landingPage = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from ShopDefaults where Parameter='LandingPage'";
                cmd = new SqlCommand(sqlQuery, conn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        landingPage = (sdr["ValueInText"]).ToString();
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
            }
            return landingPage;
        }
        #endregion

        internal static DataTable ShiftProductionCountHourlyBNG(DateTime sttime, string plantname, string machineid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetHourlyTarget_Count_followup]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@Startdate", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", "");
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@param", "BOSCH_BNG_CamShaft");
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
                dt.Columns.Add("HourLabel");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string hour = gethourminute(dt.Rows[i]["FromTime"].ToString(), dt.Rows[i]["ToTime"].ToString());
                    dt.Rows[i]["HourLabel"] = hour;
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
            return dt;
        }


        internal static string gethourminute(string fromtime, string totime)
        {
            var Fromtime = Convert.ToDateTime(fromtime);
            fromtime = (Fromtime.Minute == 0) ? Fromtime.ToString("HH") : Fromtime.ToString("HH:mm");

            var Totime = Convert.ToDateTime(totime);
            totime = (Totime.Minute == 0) ? Totime.ToString("HH") : Totime.ToString("HH:mm");

            return fromtime + " to " + totime;
        }

        internal static DataTable ShiftProductionCountHourlyBNGAeeLoss(DateTime sttime, string plantname, string machineid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetHourlyTarget_Count_followup]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@Startdate", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", "");
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@param", "BOSCH_BNG_AELosses");
                SqlDataReader rdr = cmd.ExecuteReader();
                int flag = 0;
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dt.Rows[i]["HourID"]) == 1 && Convert.ToInt32(dt.Rows[i]["ShiftID"]) != 1)
                            {
                                if (flag == 0)
                                {
                                    DataRow toInsert = dt.NewRow();
                                    toInsert[0] = 0;
                                    toInsert[1] = dt.Rows[i]["ShiftID"];
                                    toInsert[2] = dt.Rows[i]["DownCategory"];
                                    toInsert[3] = dt.Rows[i]["DownTime"];
                                    dt.Rows.InsertAt(toInsert, i);
                                    flag = 1;
                                }
                            }
                            else
                            {
                                flag = 0;
                            }
                        }
                        DataRow toInsert1 = dt.NewRow();
                        toInsert1[0] = 0;
                        toInsert1[1] = dt.Rows[dt.Rows.Count - 1]["ShiftID"];
                        toInsert1[2] = dt.Rows[dt.Rows.Count - 1]["DownCategory"];
                        toInsert1[3] = dt.Rows[dt.Rows.Count - 1]["DownTime"];
                        dt.Rows.Add(toInsert1);
                    }
                    dt.AcceptChanges();
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
            return dt;
        }


        #region "Bind Machine status information"
        public static List<MachineData> MachineStatusInfo(string plantId, string machineId)
        {
            DataTable dtMachineStatusColors = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<MachineData> lstStatusData = new List<MachineData>();
            try
            {
                dtMachineStatusColors = new DataTable();
                dtMachineStatusColors = GetMachineStatusColorCodes();
                SqlCommand cmd = new SqlCommand(@"[s_GetNODataAlerts]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Plantid", plantId);
                cmd.Parameters.AddWithValue("@Machineid", machineId);
                cmd.CommandTimeout = 120;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MachineData obj = new MachineData();
                        obj.MachineID = sdr["MachineID"].ToString();
                        obj.LastCompOpn = sdr["LastCompOpn"].ToString();
                        obj.LastdataArrivalTime = sdr["LastdataArrivalTime"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["LastdataArrivalTime"]).ToString("dd/MM/yyyy hh:mm:ss tt");
                        obj.LastDataArrivalTimeinMin = sdr["LastDataArrivalTimeinMin"].ToString();
                        if (sdr["LastArrivalStatus"] != DBNull.Value)
                            obj.LastArrivalStatus = sdr["LastArrivalStatus"].ToString() == "OK" ? "img/ok5.png" : "img/icon1.png";
                        obj.ConnectionTimestamp = sdr["ConnectionTimestamp"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["ConnectionTimestamp"]).ToString("dd/MM/yyyy hh:mm:ss tt");
                        if (sdr["ConnectionStatus"] != DBNull.Value)
                            obj.ConnectionStatus = sdr["ConnectionStatus"].ToString() == "OK" ? "img/ok5.png" : "img/icon1.png";
                        obj.PingTimestamp = sdr["PingTimestamp"] == DBNull.Value ? "" : Convert.ToDateTime(sdr["PingTimestamp"]).ToString("dd/MM/yyyy hh:mm:ss tt");
                        if (sdr["PingStatus"] != DBNull.Value)
                            obj.PingStatus = sdr["PingStatus"].ToString() == "OK" ? "img/ok5.png" : "img/icon1.png";
                        if (sdr["MachineStatus"] != DBNull.Value)
                        {
                            obj.MachineStatus = sdr["MachineStatus"].ToString();// == "OK" ? "img/ok5.png" : "img/cancel2.png";
                            obj.statusImg = sdr["MachineStatus"].ToString() == "OK" ? "img/ok5.png" : "img/icon1.png";
                        }
                        if (sdr["MachineLiveStatus"] != DBNull.Value)
                        {
                            obj.MachineLiveStatus = sdr["MachineLiveStatus"].ToString();
                            if (dtMachineStatusColors != null && dtMachineStatusColors.Rows.Count > 0)
                            {
                                DataTable dtColorCode = dtMachineStatusColors.AsEnumerable().Where(x => x.Field<string>("Status") == obj.MachineLiveStatus).CopyToDataTable();
                                if (dtColorCode != null && dtColorCode.Rows.Count > 0)
                                {
                                    obj.MachineStatusColor = dtColorCode.Rows[0].Field<string>("Colorcode").ToString();
                                }
                                else
                                {
                                    obj.MachineStatusColor = "Red";
                                }
                            }
                            else
                            {
                                obj.MachineStatusColor = "Red";
                            }
                        }
                        obj.strPingStatus = sdr["PingStatus"].ToString();
                        obj.strConStatus = sdr["ConnectionStatus"].ToString();
                        lstStatusData.Add(obj);
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstStatusData;
        }
        #endregion
        internal static string GetNodataStatusColor()
        {
            string color = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select Colorcode from Focas_MachineColorcode where [Status]='NoData'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        color = sdr["Colorcode"].ToString();
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
            return color;
        }
        internal static List<MachineStatusData> GetMachineStatusData(string plantId)
        {
            List<MachineStatusData> machineStatusDataList = new List<MachineStatusData>();
            MachineStatusData statusData = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand command = new SqlCommand("s_GetMachinewiseLiveStatus", conn);
                command.Parameters.AddWithValue("@Plantid", plantId);
                command.Parameters.AddWithValue("@Machineid", "");
                command.CommandType = CommandType.StoredProcedure;
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        statusData = new MachineStatusData
                        {
                            MachineID = sdr["MachineID"].ToString(),
                            MachineLiveStatus = sdr["MachineLiveStatus"].ToString(),
                            MachineStatusColor = sdr["MachineLiveStatusColor"].ToString()
                        };
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["KunAeroPages"].ToString() == "1")
                        {
                            if (statusData.MachineLiveStatus == "NoData")
                            {
                                statusData.MachineStatusColor = GetNodataStatusColor();
                            }
                        }
                        machineStatusDataList.Add(statusData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                //throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return machineStatusDataList;
        }
        //pawan add component information method

        internal static List<UserAccessDTO> bindListCheckBoxforUserAccesEshopx(string uid)
        {
            List<UserAccessDTO> list = new List<UserAccessDTO>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            UserAccessDTO vals = null;
            try
            {

                cmd = new SqlCommand(@"ss_UserAccessRights", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", uid);
                cmd.Parameters.AddWithValue("@param", "View");
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    vals = new UserAccessDTO();
                    vals.Domain = sdr["domain"].ToString();
                    vals.DisplayText = sdr["displaytext"].ToString();
                    vals.Code = sdr["Code"].ToString();
                    vals.Selected = Convert.ToBoolean(sdr["Isvisible"].ToString());

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


        internal static List<string> GetAllComponents()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allComps = new List<string>();
            try
            {
                string sqlQuery = "select distinct ComponentId from ComponentInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                allComps.Add("All");
                while (rdr.Read())
                {
                    allComps.Add(Convert.ToString(rdr["ComponentId"]));
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
            return allComps;
        }


        internal static List<string> GetAllInterface()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allInter = new List<string>();
            try
            {
                string sqlQuery = "select distinct InterfaceID from ComponentInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                allInter.Add("All");
                while (rdr.Read())
                {
                    allInter.Add(Convert.ToString(rdr["InterfaceID"]));
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
            return allInter;
        }

        internal static string GetShopdefaultsTimeFormat()
        {
            SqlDataReader sdr = null;
            string timeFormat = "ss";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select valueinText from Shopdefaults where Parameter='TimeInFormat'", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        timeFormat = sdr["valueinText"].ToString();
                    }
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

            return timeFormat;
        }


        internal static DataTable GetComponentDetails(string Componentid, string interfaceid, string param)
        {
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            string AchievedQty = string.Empty;
            List<ICockpitData> listOfVals = new List<ICockpitData>();
            DataTable dt = new DataTable();
            try
            {
                if (Componentid.Equals("All"))
                {
                    Componentid = string.Empty;
                }
                cmd = new SqlCommand("s_ViewComponentMaster", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@interfaceid", interfaceid);
                cmd.Parameters.AddWithValue("@Param", param);
                sdr = cmd.ExecuteReader();

                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                    foreach (DataColumn col in dt.Columns)
                    {
                        col.ReadOnly = false;
                    }

                    if (param == "ViewCompOpnInfo")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            //not created tpmsetting
                            //if (TPMSettings.timeFormat.Equals("mm", StringComparison.OrdinalIgnoreCase))
                            //{
                            //    row["StdSetupTime"] = row["StdSetupTime"] != null ? (Convert.ToDouble(row["StdSetupTime"]) / 60.00).ToString() : string.Empty;
                            //}

                            if (Convert.ToDouble(row["CycleTime"]) < 0)
                            {
                                row["CycleTime"] = 0;
                            }
                            else
                            {
                                row["CycleTime"] = Math.Round(Convert.ToDouble(row["CycleTime"]), 2);
                            }
                            if (Convert.ToDouble(row["MachiningTime"]) < 0)
                            {
                                row["MachiningTime"] = 0;
                            }
                            else
                            {
                                row["MachiningTime"] = Math.Round(Convert.ToDouble(row["MachiningTime"]), 2);
                            }
                        }
                    }

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

            return dt;
        }
        internal static DataTable GetComponentDetailsPE(string Componentid)
        {
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            string AchievedQty = string.Empty;
            List<ICockpitData> listOfVals = new List<ICockpitData>();
            DataTable dt = new DataTable();
            try
            {
                if (Componentid.Equals("All"))
                {
                    Componentid = string.Empty;
                }
                cmd = new SqlCommand(@"select * from componentinformation where (componentid = @ComponentID OR ISNULL(@ComponentID,'')='')", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                sdr = cmd.ExecuteReader();


                dt.Load(sdr);
                dt.AcceptChanges();
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

            return dt;
        }

        internal static List<string> GetAllCustomers()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allCustomerID = new List<string>();
            try
            {
                string sqlQuery = "select distinct CustomerName from CustomerInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    allCustomerID.Add("NONE");
                    while (rdr.Read())
                    {
                        allCustomerID.Add(Convert.ToString(rdr["CustomerName"]));
                    }
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
            return allCustomerID;
        }
        internal static List<string> GetallPartFamilies()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allPartFamilyID = new List<string>();
            try
            {
                string sqlQuery = "select distinct PartFamily from PartFamilyMaster_KTA";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    allPartFamilyID.Add("NONE");
                    while (rdr.Read())
                    {
                        allPartFamilyID.Add(Convert.ToString(rdr["PartFamily"]));
                    }
                    rdr.Close();
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
            return allPartFamilyID;
        }

        internal static DataTable GetAllComponentz()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                string sqlQuery = "select distinct ComponentId from ComponentInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.AcceptChanges();

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
            return dt;
        }


        //...............Insert/Update  Component Information........................
        internal static void InsertOrUpdateComponentIdDetails(string Componentid, string CompInterfaceID, string customerid, string description, string basicvalue, string InputWeight, string ForegingWeight, string operationno, string machineid, string price, string cycletime, string drawingno, string OpnInterfaceID, string loadunload, string machiningtime, string SubOperations, string StdSetupTime, string MachiningTimeThreshold,
         string TargetPercent, string UpdatedBy, string UpdatedTS, string LowerEnergyThreshold, string UpperEnergyThreshold, string SCIThreshold, string DCLThreshold,
         string McTimeMonitorLThreshold, string McTimeMonitorUThreshold, string StdDieCloseTime, string StdPouringTime, string StdSolidificationTime, string StdDieOpenTime, int FinishOperation, string param, string minLULThreshold, string process, string PartFamily, string inputCode, string outputCode, bool isManualOpn, string partType, string IncentiveTime, out bool isSuccessOrFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            isSuccessOrFailure = false;
            try
            {
                SqlCommand cmd = new SqlCommand("s_InsertComponentMaster", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 450;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@customerid", customerid);
                cmd.Parameters.AddWithValue("@CompInterfaceID", CompInterfaceID);
                cmd.Parameters.AddWithValue("@basicvalue", basicvalue);
                cmd.Parameters.AddWithValue("@InputWeight", InputWeight);
                cmd.Parameters.AddWithValue("@ForegingWeight", ForegingWeight);
                cmd.Parameters.AddWithValue("@operationno", operationno);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@cycletime", cycletime);
                cmd.Parameters.AddWithValue("@drawingno", drawingno);
                cmd.Parameters.AddWithValue("@OpnInterfaceID", OpnInterfaceID);
                cmd.Parameters.AddWithValue("@loadunload", loadunload);
                cmd.Parameters.AddWithValue("@machiningtime", machiningtime);
                cmd.Parameters.AddWithValue("@SubOperations", SubOperations);
                cmd.Parameters.AddWithValue("@StdSetupTime", StdSetupTime);
                cmd.Parameters.AddWithValue("@MachiningTimeThreshold", MachiningTimeThreshold);
                cmd.Parameters.AddWithValue("@TargetPercent", TargetPercent);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", UpdatedTS);
                cmd.Parameters.AddWithValue("@LowerEnergyThreshold", LowerEnergyThreshold);
                cmd.Parameters.AddWithValue("@UpperEnergyThreshold", UpperEnergyThreshold);
                cmd.Parameters.AddWithValue("@SCIThreshold", SCIThreshold);
                cmd.Parameters.AddWithValue("@DCLThreshold", DCLThreshold);
                cmd.Parameters.AddWithValue("@McTimeMonitorLThreshold", McTimeMonitorLThreshold);
                cmd.Parameters.AddWithValue("@McTimeMonitorUThreshold", McTimeMonitorUThreshold);
                cmd.Parameters.AddWithValue("@StdDieCloseTime", StdDieCloseTime);
                cmd.Parameters.AddWithValue("@StdPouringTime", StdPouringTime);
                cmd.Parameters.AddWithValue("@StdSolidificationTime", StdSolidificationTime);
                cmd.Parameters.AddWithValue("@StdDieOpenTime", StdDieOpenTime);
                cmd.Parameters.AddWithValue("@FinishedOperation", FinishOperation);
                cmd.Parameters.AddWithValue("@MinLoadUnloadThreshold", minLULThreshold);
                cmd.Parameters.AddWithValue("@IncentiveTime", IncentiveTime);
                if (ConfigurationManager.AppSettings["PAMSPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@Process", process);
                }
                if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@PartFamily", PartFamily);
                }
                if (ConfigurationManager.AppSettings["PittiPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@InputCode", inputCode);
                    cmd.Parameters.AddWithValue("@OutputCode", outputCode);
                    cmd.Parameters.AddWithValue("@OperationType", isManualOpn ? "Manual" : "Auto");
                    cmd.Parameters.AddWithValue("@PartType", partType);
                }
                int x = cmd.ExecuteNonQuery();
                isSuccessOrFailure = true;

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
        internal static void InsertOrUpdateComponentIdDetailsForAmarRajMangal(string Componentid, string CompInterfaceID, string customerid, string description, string basicvalue, string InputWeight, string ForegingWeight, string operationno, string machineid, string price, string cycletime, string drawingno, string OpnInterfaceID, string loadunload, string machiningtime, string SubOperations, string StdSetupTime, string MachiningTimeThreshold,
       string TargetPercent, string UpdatedBy, string UpdatedTS, string LowerEnergyThreshold, string UpperEnergyThreshold, string SCIThreshold, string DCLThreshold,
       string McTimeMonitorLThreshold, string McTimeMonitorUThreshold, string StdDieCloseTime, string StdPouringTime, string StdSolidificationTime, string StdDieOpenTime, int FinishOperation, string param, string minLULThreshold, string partsPerCycle, string strokesPartParts, string weight, string thickness, string grade, string length, string width, out bool isSuccessOrFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            isSuccessOrFailure = false;
            try
            {
                SqlCommand cmd = new SqlCommand("s_InsertComponentMaster", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 450;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@customerid", customerid);
                cmd.Parameters.AddWithValue("@CompInterfaceID", CompInterfaceID);
                cmd.Parameters.AddWithValue("@basicvalue", basicvalue);
                cmd.Parameters.AddWithValue("@InputWeight", InputWeight);
                cmd.Parameters.AddWithValue("@ForegingWeight", ForegingWeight);
                cmd.Parameters.AddWithValue("@operationno", operationno);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@cycletime", cycletime);
                cmd.Parameters.AddWithValue("@drawingno", drawingno);
                cmd.Parameters.AddWithValue("@OpnInterfaceID", OpnInterfaceID);
                cmd.Parameters.AddWithValue("@loadunload", loadunload);
                cmd.Parameters.AddWithValue("@machiningtime", machiningtime);
                cmd.Parameters.AddWithValue("@SubOperations", SubOperations);
                cmd.Parameters.AddWithValue("@StdSetupTime", StdSetupTime);
                cmd.Parameters.AddWithValue("@MachiningTimeThreshold", MachiningTimeThreshold);
                cmd.Parameters.AddWithValue("@TargetPercent", TargetPercent);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", UpdatedTS);
                cmd.Parameters.AddWithValue("@LowerEnergyThreshold", LowerEnergyThreshold);
                cmd.Parameters.AddWithValue("@UpperEnergyThreshold", UpperEnergyThreshold);
                cmd.Parameters.AddWithValue("@SCIThreshold", SCIThreshold);
                cmd.Parameters.AddWithValue("@DCLThreshold", DCLThreshold);
                cmd.Parameters.AddWithValue("@McTimeMonitorLThreshold", McTimeMonitorLThreshold);
                cmd.Parameters.AddWithValue("@McTimeMonitorUThreshold", McTimeMonitorUThreshold);
                cmd.Parameters.AddWithValue("@StdDieCloseTime", StdDieCloseTime);
                cmd.Parameters.AddWithValue("@StdPouringTime", StdPouringTime);
                cmd.Parameters.AddWithValue("@StdSolidificationTime", StdSolidificationTime);
                cmd.Parameters.AddWithValue("@StdDieOpenTime", StdDieOpenTime);
                cmd.Parameters.AddWithValue("@FinishedOperation", FinishOperation);
                cmd.Parameters.AddWithValue("@MinLoadUnloadThreshold", minLULThreshold);
                cmd.Parameters.AddWithValue("@PartsPerCycle", partsPerCycle);
                cmd.Parameters.AddWithValue("@StrokesPerPart", strokesPartParts);
                cmd.Parameters.AddWithValue("@Weight", weight);
                cmd.Parameters.AddWithValue("@Thickness", thickness);
                cmd.Parameters.AddWithValue("@Grade", grade);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@Width", width);
                int x = cmd.ExecuteNonQuery();
                isSuccessOrFailure = true;

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
        internal static void InsertOrUpdateComponentIdDetailsForAllied(string Componentid, string CompInterfaceID, string customerid, string description, string basicvalue, string InputWeight, string ForegingWeight, string operationno, string machineid, string price, string cycletime, string drawingno, string OpnInterfaceID, string loadunload, string machiningtime, string SubOperations, string StdSetupTime, string MachiningTimeThreshold,
        string TargetPercent, string UpdatedBy, string UpdatedTS, string LowerEnergyThreshold, string UpperEnergyThreshold, string SCIThreshold, string DCLThreshold,
        string McTimeMonitorLThreshold, string McTimeMonitorUThreshold, string StdDieCloseTime, string StdPouringTime, string StdSolidificationTime, string StdDieOpenTime, int FinishOperation, string param, string minLULThreshold, string stdTestPressure, string stdHoldingTime, out bool isSuccessOrFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            isSuccessOrFailure = false;
            try
            {
                SqlCommand cmd = new SqlCommand("s_InsertComponentMaster", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@customerid", customerid);
                cmd.Parameters.AddWithValue("@CompInterfaceID", CompInterfaceID);
                cmd.Parameters.AddWithValue("@basicvalue", basicvalue);
                cmd.Parameters.AddWithValue("@InputWeight", InputWeight);
                cmd.Parameters.AddWithValue("@ForegingWeight", ForegingWeight);
                cmd.Parameters.AddWithValue("@operationno", operationno);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@cycletime", cycletime);
                cmd.Parameters.AddWithValue("@drawingno", drawingno);
                cmd.Parameters.AddWithValue("@OpnInterfaceID", OpnInterfaceID);
                cmd.Parameters.AddWithValue("@loadunload", loadunload);
                cmd.Parameters.AddWithValue("@machiningtime", machiningtime);
                cmd.Parameters.AddWithValue("@SubOperations", SubOperations);
                cmd.Parameters.AddWithValue("@StdSetupTime", StdSetupTime);
                cmd.Parameters.AddWithValue("@MachiningTimeThreshold", MachiningTimeThreshold);
                cmd.Parameters.AddWithValue("@TargetPercent", TargetPercent);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", UpdatedTS);
                cmd.Parameters.AddWithValue("@LowerEnergyThreshold", LowerEnergyThreshold);
                cmd.Parameters.AddWithValue("@UpperEnergyThreshold", UpperEnergyThreshold);
                cmd.Parameters.AddWithValue("@SCIThreshold", SCIThreshold);
                cmd.Parameters.AddWithValue("@DCLThreshold", DCLThreshold);
                cmd.Parameters.AddWithValue("@McTimeMonitorLThreshold", McTimeMonitorLThreshold);
                cmd.Parameters.AddWithValue("@McTimeMonitorUThreshold", McTimeMonitorUThreshold);
                cmd.Parameters.AddWithValue("@StdDieCloseTime", StdDieCloseTime);
                cmd.Parameters.AddWithValue("@StdPouringTime", StdPouringTime);
                cmd.Parameters.AddWithValue("@StdSolidificationTime", StdSolidificationTime);
                cmd.Parameters.AddWithValue("@StdDieOpenTime", StdDieOpenTime);
                cmd.Parameters.AddWithValue("@FinishedOperation", FinishOperation);
                cmd.Parameters.AddWithValue("@MinLoadUnloadThreshold", minLULThreshold);
                cmd.Parameters.AddWithValue("@StdTestPressure", stdTestPressure);
                cmd.Parameters.AddWithValue("@StdHoldingTime", stdHoldingTime);
                int x = cmd.ExecuteNonQuery();
                isSuccessOrFailure = true;

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
        //...............Delete Component Information........................
        internal static void DeleteComponentOperationDetails(string param, string Componentid, string CompInterfaceID, string OpnInterfaceID, string operationno, string machineid, out bool isSuccessOrFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            isSuccessOrFailure = false;
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("s_InsertComponentMaster", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@CompInterfaceID", CompInterfaceID);
                cmd.Parameters.AddWithValue("@OpnInterfaceID", OpnInterfaceID);
                cmd.Parameters.AddWithValue("@operationno", operationno);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                int x = cmd.ExecuteNonQuery();
                isSuccessOrFailure = true;
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
        internal static int deleteComponentDetails(string Componentid, string CompInterfaceID)
        {
            int result = 0;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("s_InsertComponentMaster", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "DeleteCompInfo");
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@CompInterfaceID", CompInterfaceID);
                result = cmd.ExecuteNonQuery();
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
            return result;
        }

        //------ Component Operation Details -----
        internal static DataTable getComponentOperationDetails(string Componentid)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string sqlQuery = string.Empty;
            try
            {
                if (ConfigurationManager.AppSettings["AlliedPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    sqlQuery = @"select distinct c.componentid,co.machineid,p1.GroupID,c.InterfaceID as CompInterfaceID,c.description as CompDesc,co.InterfaceID as OpnInterfaceID,co.operationno,
                                co.description as OpnDesc, co.machiningtime, (co.cycletime-co.machiningtime) as loadunload, co.cycletime, co.price,co.drawingno,co.SubOperations,co.StdSetupTime,
                                co.MachiningTimeThreshold,co.TargetPercent,co.loadunload as loadUnloadTimeThreshold, c.customerid,co.SCIThreshold,co.DCLThreshold,co.FinishedOperation,co.MinLoadUnloadThreshold,co.StdTestPressure,co.StdHoldingTime
                                from componentinformation c
                                inner join componentoperationpricing co on c.componentid = co.componentid
                                left outer join PlantMachineGroups p1 on p1.MachineID=co.machineid
                                where
                                (c.componentid like  '%' + @Component + '%' or ISNULL(@Component, '') = '')";
                }
                else if (ConfigurationManager.AppSettings["KTASpindlePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    sqlQuery = @"select distinct c.componentid,co.machineid,p1.GroupID,c.InterfaceID as CompInterfaceID,c.description as CompDesc,co.InterfaceID as OpnInterfaceID,co.operationno,
                                co.description as OpnDesc, co.machiningtime, (co.cycletime-co.machiningtime) as loadunload, co.cycletime,co.price,co.drawingno,co.SubOperations,co.StdSetupTime,
                                co.MachiningTimeThreshold,co.TargetPercent,co.loadunload as loadUnloadTimeThreshold, c.customerid,co.SCIThreshold,co.DCLThreshold,co.FinishedOperation,co.MinLoadUnloadThreshold,c.PartFamily
                                from componentinformation c
                                inner join componentoperationpricing co on c.componentid = co.componentid
                                left outer join PlantMachineGroups p1 on p1.MachineID=co.machineid
                                where
                                (c.componentid like  '%' + @Component + '%' or ISNULL(@Component, '') = '')";
                }
                else
                {
                    sqlQuery = @"select distinct c.componentid,co.machineid,p1.GroupID,c.InterfaceID as CompInterfaceID,c.description as CompDesc,co.InterfaceID as OpnInterfaceID,co.operationno,
                                co.description as OpnDesc, co.machiningtime, (co.cycletime-co.machiningtime) as loadunload, co.cycletime, co.price,co.drawingno,co.SubOperations,co.StdSetupTime,
                                co.MachiningTimeThreshold,co.TargetPercent,co.loadunload as loadUnloadTimeThreshold, c.customerid,co.SCIThreshold,co.DCLThreshold,co.FinishedOperation,co.MinLoadUnloadThreshold,co.IncentiveTime
                                from componentinformation c
                                inner join componentoperationpricing co on c.componentid = co.componentid
                                left outer join PlantMachineGroups p1 on p1.MachineID=co.machineid
                                where
                                (c.componentid like  '%' + @Component + '%' or ISNULL(@Component, '') = '')";
                }
                //                SqlCommand cmd = new SqlCommand(@"select c.componentid,co.machineid,c.InterfaceID as CompInterfaceID,c.description as CompDesc,co.InterfaceID as OpnInterfaceID,co.operationno,
                //co.description as OpnDesc, co.machiningtime, co.cycletime, co.loadunload
                //from componentinformation c
                //inner
                //join componentoperationpricing co on c.componentid = co.componentid where
                //(c.componentid like  '%'+@Component+'%' or ISNULL(@Component,'')='')", sqlConn);
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Component", Componentid);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        //...............Component Operation methods  GetAllStdTimes........................
        internal static void GetAllStdTimes(string MachineID, string ComponentID, string OperationNo, out string CycleTime, out string MachiningTime, out string loadUnload)
        {
            SqlDataReader sdr = null;
            MachiningTime = "0";
            CycleTime = "0";
            loadUnload = "0";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select machiningtime,cycleTime,loadUnload from componentOperationPricing where MachineID = @MachineID and ComponentID=@ComponentID and OperationNo=@operationNo", conn);

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"machineid", MachineID);
                cmd.Parameters.AddWithValue(@"componentid", ComponentID);
                cmd.Parameters.AddWithValue(@"OperationNo", OperationNo);

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        CycleTime = sdr["cycleTime"].ToString();
                        MachiningTime = sdr["machiningtime"].ToString();
                        loadUnload = sdr["loadUnload"].ToString();
                    }
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

        }

        internal static List<string> GetAllMacForPlant(string PlantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allMachines = new List<string>();
            try
            {
                if (PlantId.Equals("All")) PlantId = string.Empty;
                //                string sqlQuery = @"select MachineInformation.machineid as MachineID , InterfaceID  from machineinformation inner join plantmachine on MachineInformation.MachineID=PlantMachine.MachineID 
                //                                    where (PlantID=@plantID or @plantID='') order by MachineInformation.MachineID";

                string sqlQuery = @"select distinct machineid from MachineInformation where TPMTrakEnabled = 1 order by machineid";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@plantID", PlantId);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allMachines.Add(Convert.ToString(rdr["MachineID"]));
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
            return allMachines;
        }
        internal static List<string> GetAllMacForPlant_COP(string PlantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allMachines = new List<string>();
            try
            {
                string sqlQuery = @"select distinct MachineID from PlantMachineGroups where (PlantID=@plantID or @plantID='')";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@plantID", PlantId);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allMachines.Add(Convert.ToString(rdr["MachineID"]));
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
            return allMachines;
        }


        internal static List<string> GetAllPlants()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allPlants = new List<string>();
            try
            {
                string sqlQuery = "[s_GetLookups]";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Plant");
                SqlDataReader rdr = cmd.ExecuteReader();
                allPlants.Add("All");
                while (rdr.Read())
                {
                    allPlants.Add(Convert.ToString(rdr["PlantID"]));
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
            return allPlants;
        }

        internal static string GetPlantID(string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string list = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select PIM.PlantID from plantmachine PM inner join PlantInformation PIM
                on PM.PlantID=PIM.PlantID  where Machineid=@Machineid", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list = rdr["PlantID"].ToString();
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
            return list;
        }

        internal static PlantData GetPlantData(string PlantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            PlantData pi = new PlantData();
            try
            {
                string sqlQuery = "select PlantID,Description,PlantCode,SlNo from plantinformation where PlantID = @PlantID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    pi.PlantID = rdr["PlantID"].ToString();
                    pi.PlantDescription = rdr["Description"].ToString();
                    pi.PlantCode = rdr["PlantCode"].ToString();
                    pi.PlantSLno = rdr["SlNo"].ToString();
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
            return pi;
        }

        //..................Add new code  ....................

        public static DataTable RejectionReason()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = null;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd;
                connection = ConnectionManager.GetConnection();
                cmd = new SqlCommand("select *  from rejectioncodeinformation", connection);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }


            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (connection != null) connection.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        internal static List<string> databindforCatagoryForRework()
        {
            List<string> lst = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            try
            {
                query = "select distinct ReworkCatagory from reworkinformation";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    lst.Add(sdr["ReworkCatagory"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lst;
        }
        internal static List<string> databindforCatagory()
        {
            List<string> lst = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            try
            {
                query = "select distinct Catagory from rejectioncodeinformation";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    lst.Add(sdr["Catagory"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lst;
        }
        internal static List<RejectionAndReworkinModel> getCatagorySubCatgoryList()
        {
            List<RejectionAndReworkinModel> lst = new List<RejectionAndReworkinModel>();
            RejectionAndReworkinModel data = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            try
            {
                query = "select distinct Catagory,SubCategory from rejectioncodeinformation";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    data = new RejectionAndReworkinModel();
                    data.Catagory = sdr["Catagory"].ToString();
                    data.SubCatagory = sdr["SubCategory"].ToString();
                    lst.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lst;
        }
        public static DataTable ReworkReason()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = null;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd;
                connection = ConnectionManager.GetConnection();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["PrecisionEngPages"].ToString() == "1")
                    cmd = new SqlCommand("select reworkid,reworkdescription,reworkcatagory,reworkinterfaceid,ReworkDescriptionInHindi from reworkinformation", connection);
                else
                    cmd = new SqlCommand("select reworkid,reworkdescription,reworkcatagory,reworkinterfaceid from reworkinformation", connection);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }


            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (connection != null) connection.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;

        }

        internal static bool Checkinterfaceid(string interfaceid, string rejectionid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool allreadyPresent = false;

            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select count(*) from rejectioncodeinformation where interfaceid=@interfaceid and rejectionid<>@rejectionid";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@interfaceid", interfaceid);
                cmd.Parameters.AddWithValue("@rejectionid", rejectionid);
                Int32 count = (Int32)cmd.ExecuteScalar();
                if (count > 0)
                {
                    allreadyPresent = true;
                }
                else
                {
                    allreadyPresent = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return allreadyPresent;
        }

        internal static bool CheckRejectionId(string rejectionid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool allreadyPresent = false;

            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from rejectioncodeinformation where rejectionid=@rejectionid";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@rejectionid", rejectionid);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    allreadyPresent = true;
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return allreadyPresent;

        }


        internal static void UpdateData(string Catagory, string rejectionid, string txtDescription, string txtInterfaceId, string subcategory, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GlobePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand("update rejectioncodeinformation set interfaceid=@Interfaceid,rejectiondescription=@Description,Catagory=@Catagory,SubCategory=@subcat,UpdatedTS=@UpdatedTS where rejectionid=@rejectionId", conn);
                }
                else
                {
                    cmd = new SqlCommand("update rejectioncodeinformation set interfaceid=@Interfaceid,rejectiondescription=@Description,Catagory=@Catagory,UpdatedTS=@UpdatedTS where rejectionid=@rejectionId", conn);
                }
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@rejectionId", rejectionid);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Trim());
                cmd.Parameters.AddWithValue("@Catagory", Catagory.Trim());
                cmd.Parameters.AddWithValue("@Interfaceid", txtInterfaceId.Trim());
                cmd.Parameters.AddWithValue("@subcat", subcategory.Trim());
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }


        internal static void insertData(string Catagory, string txtDescription, string rejectionid, string txtInterfaceId, string subcategory, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GlobePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand("insert into rejectioncodeinformation (rejectionid,rejectiondescription,Catagory,interfaceid,SubCategory,UpdatedTS) values (@rejectionId,@Description,@Catagory,@Interfaceid,@subcat,@UpdatedTS)", conn);
                }
                else
                {
                    cmd = new SqlCommand("insert into rejectioncodeinformation (rejectionid,rejectiondescription,Catagory,interfaceid,UpdatedTS) values (@rejectionId,@Description,@Catagory,@Interfaceid,@UpdatedTS)", conn);
                }
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@rejectionId", rejectionid);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Trim());
                cmd.Parameters.AddWithValue("@Catagory", Catagory.Trim());
                cmd.Parameters.AddWithValue("@Interfaceid", txtInterfaceId.Trim());
                cmd.Parameters.AddWithValue("@subcat", subcategory.Trim());
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static bool CheckReworkinterfaceid(string interfaceid, string Reworkid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool allreadyPresent = false;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select count(*)  from reworkinformation where Reworkinterfaceid=@interfaceid and Reworkid<>@Reworkid";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@interfaceid", interfaceid);
                cmd.Parameters.AddWithValue("@Reworkid", Reworkid);
                Int32 count = (Int32)cmd.ExecuteScalar();
                if (count > 0)
                {
                    allreadyPresent = true;
                }
                else
                {
                    allreadyPresent = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return allreadyPresent;
        }

        internal static bool CheckpresentReworkId(string reworkid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool allreadyPresent = false;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from reworkinformation where Reworkid=@Reworkid";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@Reworkid", reworkid);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    allreadyPresent = true;
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return allreadyPresent;
        }

        internal static void UpdateDataForRework(string Catagory, string txtDescription, string Reworkid, string txtInterfaceId, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("update reworkinformation set  Reworkinterfaceid=@Reworkinterfaceid,Reworkdescription=@Reworkdescription,ReworkCatagory=@ReworkCatagory,UpdatedTS=@UpdatedTS where Reworkid=@Reworkid", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Reworkid", Reworkid);
                cmd.Parameters.AddWithValue("@Reworkdescription", txtDescription);
                cmd.Parameters.AddWithValue("@ReworkCatagory", Catagory);
                cmd.Parameters.AddWithValue("@Reworkinterfaceid", txtInterfaceId);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void insertDataForRework(string reworkId, string txtDescription, string Catagory, string txtInterfaceId, out string sucessfailure)
        {

            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("insert into reworkinformation (Reworkid,Reworkdescription,ReworkCatagory,Reworkinterfaceid,UpdatedTS) values (@reworkId,@Description,@Catagory,@Interfaceid,@UpdatedTS)", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@reworkId", reworkId);
                cmd.Parameters.AddWithValue("@Description", txtDescription);
                cmd.Parameters.AddWithValue("@Catagory", Catagory);
                cmd.Parameters.AddWithValue("@Interfaceid", txtInterfaceId);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void DeleteDataForRejectionReason(string RejectionId, out string sucessfailure)
        {

            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("Delete from rejectioncodeinformation where rejectionid=@rejectionid", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@rejectionid", RejectionId);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }
        internal static void DeleteDataForReworkReason(string Reworkid, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("Delete from reworkinformation where Reworkid=@Reworkid", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Reworkid", Reworkid);
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
        }

        //....................customer page....................
        internal static DataTable AddDataForCustomerInformstion()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from customerinformation order by customerid";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                SqlDataReader sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.GetChanges();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static bool CheckCustomerId(string Customerid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool isPresent = false;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from customerinformation where customerid=@CustumerId";
                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@CustumerId", Customerid);

                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    isPresent = true;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return isPresent;
        }

        internal static void insertUpdateCustomerInfo(string CustomerId, string CustomerName, string address1, string place, string state, string country, string pin, string Phone, string email, string contactperson, out string sucessFailure)
        {
            sucessFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"insert into customerinformation (customername,address1,place,state,country,pin,phone,email,contactperson,customerid)
                                    values(@customername,@address,@place, @state,@country,@pin,@phone,@email,@contactperson,@customerid)", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@customername", CustomerName);
                cmd.Parameters.AddWithValue("@address", address1);
                cmd.Parameters.AddWithValue("@place", place);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@pin", pin);
                cmd.Parameters.AddWithValue("@phone", Phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@contactperson", contactperson);
                cmd.Parameters.AddWithValue("@customerid", CustomerId);
                cmd.ExecuteNonQuery();
                sucessFailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }


        internal static void UpdateCustomerInfo(string CustomerId, string CustomerName, string address1, string place, string state, string country, string pin, string Phone, string email, string contactperson, out string sucessFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("update customerinformation set customername=@customername,address1=@address,place = @place,state = @state, country=@country,pin=@pin,phone = @phone,email=@email,contactperson = @contactperson where customerid=@customerid", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@customername", CustomerName);
                cmd.Parameters.AddWithValue("@address", address1);
                cmd.Parameters.AddWithValue("@place", place);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@pin", pin);
                cmd.Parameters.AddWithValue("@phone", Phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@contactperson", contactperson);
                cmd.Parameters.AddWithValue("@customerid", CustomerId);
                cmd.ExecuteNonQuery();
                sucessFailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void DeleteCustomerInformation(string customerid, out string sucessFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("Delete from customerinformation where customerid=@customerid", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@customerid", customerid);
                cmd.ExecuteNonQuery();
                sucessFailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }


        //modifyData
        internal static List<string> GetAllMachinesForPlant(string PlantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allMachines = new List<string>();
            try
            {
                if (PlantId.Equals("All")) PlantId = string.Empty;
                string sqlQuery = @"select MachineInformation.machineid as MachineID , InterfaceID  from machineinformation inner join plantmachine on MachineInformation.MachineID=PlantMachine.MachineID 
                                    where (PlantID=@plantID or @plantID='') order by MachineInformation.MachineID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@plantID", PlantId);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allMachines.Add(Convert.ToString(rdr["InterfaceID"]) + " <" + Convert.ToString(rdr["MachineID"]) + ">");
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
            return allMachines;
        }

        internal static List<string> GetProdDownRejectiondata(DateTime StartTime, DateTime EndTime, string plantid, string MacInt, string Comp, string Opn, string Opr, string downcode, string Datatype, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                if (string.Equals(plantid, "All", StringComparison.OrdinalIgnoreCase)) plantid = "";
                cmd = new SqlCommand("[s_GetProdDownRejectiondata]", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"StartTime", StartTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue(@"EndTime", EndTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue("@machineid", MacInt);
                cmd.Parameters.AddWithValue("@Component", Comp);
                cmd.Parameters.AddWithValue("@operation", Opn);
                cmd.Parameters.AddWithValue("@Operator", Opr);
                cmd.Parameters.AddWithValue("@DownCode", downcode);
                cmd.Parameters.AddWithValue("@Plantid", plantid);
                cmd.Parameters.AddWithValue(@"Datatype", Datatype);
                cmd.Parameters.AddWithValue("@param", param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["result"].ToString());
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
            return list;
        }


        public static List<string> GetAllPlantsForPlantInfo()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> plantid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select plantid from PlantInformation order by Plantid", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                //  cmd.Parameters.AddWithValue(@"name", "Plant");
                //  plantid.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    plantid.Add(rdr["plantid"].ToString());
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
            return plantid;
        }
        internal static List<string> GetAllPlantsForPlantInfoPP()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> plantid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select DISTINCT plantid from PlantInformation order by Plantid", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                //  cmd.Parameters.AddWithValue(@"name", "Plant");
                //  plantid.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    plantid.Add(rdr["plantid"].ToString());
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
            return plantid;
        }

        internal static List<string> GetAssignedAndUnAssignedMachineIds(string sqlQuery, string PlantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["machineid"].ToString());
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
            return list;
        }

        internal static int DeleteMachineToPlant(string machineID, string plantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "delete from PlantMachine where PlantID = @plantID and MachineID = @machineID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }

        internal static int AddMachineToPlant(string machineID, string plantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordsAffected = 0;
            try
            {
                string sqlQuery = "insert into PlantMachine (PlantID,MachineID,UpdatedTS) values (@PlantID, @MachineID,@UpdatedTS)";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                recordsAffected = cmd.ExecuteNonQuery();
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
            return recordsAffected;
        }

        internal static bool CheckExistingPlant(string plantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            Boolean isPresent = false;
            string adminVal = string.Empty;
            try
            {
                string query = "select * from PlantInformation where PlantID= @plantID";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    isPresent = true;
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
            return isPresent;

        }

        internal static int AddPLantInfo(string plantID, string plantCode, string plantDescription, string action)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int val = 0;
            try
            {
                string sqlQuery = string.Empty;
                if (action == "Update")
                {
                    sqlQuery = "update PlantInformation set PlantCode= @plantCode, Description= @plantDescription  where PlantID = @plantID ";
                }
                else
                {
                    sqlQuery = "insert into PlantInformation(PlantID,Description,PlantCode) values (@plantID , @plantDescription , @plantCode )";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@plantDescription", plantDescription);
                cmd.Parameters.AddWithValue("@PlantCode", plantCode);
                val = cmd.ExecuteNonQuery();
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
            return val;
        }

        internal static bool CheckExistingPlantMachine(string plantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            Boolean isPresent = false;
            string adminVal = string.Empty;
            try
            {
                string query = "select * from plantMachine where PlantID= @plantID";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    isPresent = true;
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
            return isPresent;

        }

        internal static int DeletePlantInfo(string plantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordDeleted = 0;
            try
            {
                string sqlQuery = "delete from PlantInformation where PlantID = @plantID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                recordDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return recordDeleted;
        }


        //modified data
        internal static List<string> GetAllDownCode()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> downCode = new List<string>();
            try
            {
                string sqlQuery = @"select * from downcodeinformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    downCode.Add(Convert.ToString(rdr["downid"]));
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
            return downCode;
        }

        internal static List<string> GetAllComp()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allComps = new List<string>();
            try
            {
                string sqlQuery = "select distinct ComponentId from ComponentInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    allComps.Add(Convert.ToString(rdr["ComponentId"]));
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
            return allComps;
        }

        internal static List<string> GetAllComp(string Machineid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allComps = new List<string>();
            string sqlQuery = string.Empty;
            if (string.IsNullOrEmpty(Machineid))
                sqlQuery = @"select distinct ComponentId from ComponentInformation";
            else
                sqlQuery = @"select distinct componentID from componentoperationpricing where machineid =  @Machineid ";
            try
            {
                //sqlQuery = "select distinct ComponentId from ComponentInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    allComps.Add(Convert.ToString(rdr["ComponentId"]));
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
            return allComps;
        }

        internal static List<string> GetAllEmployees()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allEmps = new List<string>();
            try
            {
                string sqlQuery = "select distinct EmployeeID from EmployeeInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                allEmps.Add("All");
                while (rdr.Read())
                {
                    allEmps.Add(Convert.ToString(rdr["EmployeeID"]));
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
            return allEmps;
        }

        internal static List<string> GetAllEmployeesID()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allEmps = new List<string>();
            try
            {
                string sqlQuery = "select distinct EmployeeID from EmployeeInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                // allEmps.Add("All");
                while (rdr.Read())
                {
                    allEmps.Add(Convert.ToString(rdr["EmployeeID"]));
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
            return allEmps;
        }
        internal static List<string> GetEmployeesWithName()
        {
            List<string> allEmps = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string sqlQuery = "select distinct Name from EmployeeInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allEmps.Add(Convert.ToString(rdr["Name"]));
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
            return allEmps;
        }
        internal static List<string> GetCheckpointIDs_Precision()
        {
            List<string> list = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = "select distinct CheckpointID,CheckpointDesc from DailyCheckSheetMasterDetails_Precision";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["CheckpointID"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        internal static List<ListItem> GetAllEmployeesWithName()
        {
            List<ListItem> allEmps = new List<ListItem>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string sqlQuery = "select distinct EmployeeID,Name from EmployeeInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                allEmps.Add(new ListItem() { Text = "All", Value = "All" });
                while (rdr.Read())
                {
                    allEmps.Add(new ListItem() { Text = rdr["Name"].ToString() + " (" + rdr["EmployeeID"].ToString() + ")", Value = rdr["EmployeeID"].ToString() });
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
            return allEmps;
        }

        internal static List<System.Web.UI.WebControls.ListItem> GetAllEmployeesWithIntefaceid()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allEmps = new List<string>();
            List<System.Web.UI.WebControls.ListItem> list = new List<System.Web.UI.WebControls.ListItem>();
            try
            {
                string sqlQuery = "select distinct EmployeeID,interfaceid from EmployeeInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                allEmps.Add("All");
                while (rdr.Read())
                {
                    list.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(rdr["EmployeeID"]), rdr["interfaceid"].ToString()));
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
            return list;
        }
        internal static System.Data.DataTable GetProdDownEventRejectionGridDetails(string Machineid, DateTime StartTime, DateTime EndTime, string param)
        {
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            string AchievedQty = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("SS_GetModifiedData", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                cmd.Parameters.AddWithValue("@StartTime", StartTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue("@EndTime", EndTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.AcceptChanges();
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
            return dt;
        }
        internal static string GetCompInterfaceID(string compBusniessID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string CompInterfaceid = string.Empty;
            try
            {
                string sqlQuery = @"select distinct Interfaceid from componentinformation where Componentid=@componentid";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@componentid", compBusniessID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CompInterfaceid = (Convert.ToString(rdr["interfaceid"]));
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
            return CompInterfaceid;
        }
        internal static string GetEmpInterfaceID(string EmpBusniessID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string EmpInterfaceid = string.Empty;
            try
            {
                string sqlQuery = @"select distinct Interfaceid from Employeeinformation where Employeeid=@Employeeid";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Employeeid", EmpBusniessID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmpInterfaceid = (Convert.ToString(rdr["interfaceid"]));
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

            return EmpInterfaceid;
        }
        internal static string GetAllDownid(string downcode)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string downCode = string.Empty;
            try
            {
                string sqlQuery = @"select * from downcodeinformation where downid=@downcode";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@downcode", downcode);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    downCode = (Convert.ToString(rdr["interfaceid"]));
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
            return downCode;
        }

        internal static void UpdateMachineStatusColorCodes(string status, string color, out bool isUpdated)
        {
            isUpdated = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"Update Focas_MachineColorcode set Colorcode = @Colorcode where Status = @Status";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Colorcode", "#" + color.ToUpper());
                cmd.Parameters.AddWithValue("@Status", status);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    isUpdated = true;
            }
            catch (Exception ex)
            {
                isUpdated = false;
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void UpdateProductionDownRejectionData_NotBulk(string stTime, int id, DateTime StartTime, DateTime EndTime, string MacInt, string FromComponent, string ToComponent, string FromOperation, string ToOperation, string FromOperator, string ToOperator, Decimal FromPartsCount, Decimal ToPartsCount, string FromdownCode, string TodownCode, int Datatype, string param, string rejectQty, out bool IsUpdated, string UpdatedBy)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            IsUpdated = false;
            try
            {
                cmd = new SqlCommand(@"S_UpdateAutodataAuditLogDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"StartTime", StartTime);
                cmd.Parameters.AddWithValue(@"EndTime", EndTime);
                cmd.Parameters.AddWithValue("@sttime", stTime == "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToDateTime(stTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue(@"FromComp", FromComponent);
                cmd.Parameters.AddWithValue(@"ToComp", ToComponent);
                cmd.Parameters.AddWithValue(@"FromOperation", FromOperation);
                cmd.Parameters.AddWithValue(@"ToOperation", ToOperation);
                cmd.Parameters.AddWithValue(@"mc", MacInt);
                cmd.Parameters.AddWithValue(@"FromPartsCount", FromPartsCount);
                cmd.Parameters.AddWithValue(@"ToPartsCount", ToPartsCount);
                cmd.Parameters.AddWithValue(@"Fromdcode", FromdownCode);
                cmd.Parameters.AddWithValue(@"Todcode", TodownCode);
                cmd.Parameters.AddWithValue(@"datatype", Datatype);
                cmd.Parameters.AddWithValue(@"Param", param);
                cmd.Parameters.AddWithValue(@"id", id);
                cmd.Parameters.AddWithValue(@"UpdatedBy", UpdatedBy);
                cmd.Parameters.AddWithValue(@"BulkUpdate", 0);
                cmd.Parameters.AddWithValue("@FromOperator", FromOperator);
                cmd.Parameters.AddWithValue("@ToOperator", ToOperator);
                int x = cmd.ExecuteNonQuery();
                IsUpdated = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) con.Close();
            }
        }
        internal static void UpdateProdDownRejectiondata(int id, DateTime StartTime, DateTime EndTime, string MacInt, string FromComponent, string ToComponent, string FromOperation, string ToOperation,
       string FromOperator, string ToOperator, string FromWorkOrder, string ToWorkOrder, Decimal FromPartsCount, Decimal ToPartsCount, string downid, string FromdownCode,
       string TodownCode, string RejectionID, string FromRejectionCode, string ToRejectionCode, string Datatype, string param, string rejectQty, out bool IsUpdated)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            IsUpdated = false;
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"s_UpdateProdDownRejectiondata", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"StartTime", StartTime);
                cmd.Parameters.AddWithValue(@"EndTime", EndTime);
                cmd.Parameters.AddWithValue(@"FromComponent", FromComponent);
                cmd.Parameters.AddWithValue(@"ToComponent", ToComponent);
                cmd.Parameters.AddWithValue(@"FromOperation", FromOperation);
                cmd.Parameters.AddWithValue(@"ToOperation", ToOperation);
                cmd.Parameters.AddWithValue(@"Machineid", MacInt);
                cmd.Parameters.AddWithValue(@"Fromoperator", FromOperator);
                cmd.Parameters.AddWithValue(@"Tooperator", ToOperator);
                cmd.Parameters.AddWithValue(@"FromWorkOrder", FromWorkOrder);
                cmd.Parameters.AddWithValue(@"ToWorkOrder", ToWorkOrder);
                cmd.Parameters.AddWithValue(@"FromPartsCount", FromPartsCount);
                cmd.Parameters.AddWithValue(@"ToPartsCount", ToPartsCount);
                cmd.Parameters.AddWithValue(@"downid", downid);
                cmd.Parameters.AddWithValue(@"FromdownCode", FromdownCode);
                cmd.Parameters.AddWithValue(@"TodownCode", TodownCode);
                cmd.Parameters.AddWithValue(@"RejectionID", RejectionID);
                cmd.Parameters.AddWithValue(@"FromRejectionCode", FromRejectionCode);
                cmd.Parameters.AddWithValue(@"ToRejectionCode", ToRejectionCode);
                cmd.Parameters.AddWithValue(@"Datatype", Datatype);
                cmd.Parameters.AddWithValue(@"param", param);
                cmd.Parameters.AddWithValue(@"id", id);
                cmd.Parameters.AddWithValue(@"RejectionQty", rejectQty);
                int x = cmd.ExecuteNonQuery();
                IsUpdated = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        //Pawan //job card
        internal static List<string> bindDataForShiftName()
        {

            List<string> list = new List<string>();
            string sqlquery = "Select * from shiftdetails where running =1 ";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlquery, sqlConn);
                cmd.CommandType = CommandType.Text;
                //  cmd.Parameters.AddWithValue("@shiftid", shiftid);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {

                    list.Add(sdr["ShiftName"].ToString());

                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        internal static System.Data.DataTable GetJobCardDetails(DateTime pDate, string Shift, string Plantid, string Machineid, string Componentid, string operationNo, string operatorID, int AcceptedParts, int RepeatCycles,
        int dummyCycles, int ReworkPerformed, int markedforrework, int rejection, string updatedBy, DateTime UpdatedTs, string downid, int id, string param, string Remarks, string ActionTaken, string StartTime, string EndTime, out bool rowAdded, int deletedQty) //
        {
            if (Plantid.Equals("All")) Plantid = string.Empty;
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            string AchievedQty = string.Empty;
            DataTable dt = new DataTable();
            rowAdded = false;
            try
            {
                cmd = new SqlCommand("SS_getJobCardDetails", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pDate", pDate);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@operationNo", operationNo);
                cmd.Parameters.AddWithValue("@operatorID", operatorID);
                cmd.Parameters.AddWithValue("@AcceptedParts", AcceptedParts);
                cmd.Parameters.AddWithValue("@RepeatCycles", RepeatCycles);
                cmd.Parameters.AddWithValue("@dummyCycles", dummyCycles);
                cmd.Parameters.AddWithValue("@ReworkPerformed", ReworkPerformed);
                cmd.Parameters.AddWithValue("@markedforrework", markedforrework);
                cmd.Parameters.AddWithValue("@rejection", rejection);
                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
                cmd.Parameters.AddWithValue("@UpdateTS", UpdatedTs);
                cmd.Parameters.AddWithValue("@downid", downid);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@param", param);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@ActionTaken", ActionTaken);
                cmd.Parameters.AddWithValue("@DeletedRejQty", deletedQty);
                try
                {
                    StartTime = string.IsNullOrEmpty(StartTime) ? "" : DateTime.ParseExact(StartTime, "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                    EndTime = string.IsNullOrEmpty(EndTime) ? "" : DateTime.ParseExact(EndTime, "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog("Conversion of DateTimeFailed" + ex.Message);
                }
                cmd.Parameters.AddWithValue("@Starttime", StartTime);
                cmd.Parameters.AddWithValue("@Endtime", EndTime);
                sdr = cmd.ExecuteReader();
                rowAdded = true;
                dt.Load(sdr);
                dt.AcceptChanges();

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

            return dt;
        }

        internal static int GetJobCardRejRwkDetails(DateTime pDate, string Shift, string Plantid, string Machineid, string Componentid, string operationNo, string operatorID, int AcceptedParts, int RepeatCycles,
       int dummyCycles, int ReworkPerformed, int markedforrework, int rejection, string updatedBy, DateTime UpdatedTs, string downid, int id, string param, out bool rowAdded)
        {
            if (Plantid.Equals("All")) Plantid = string.Empty;
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            string AchievedQty = string.Empty;
            DataTable dt = new DataTable();
            int rejrework = 0;
            rowAdded = false;
            try
            {
                cmd = new SqlCommand("SS_getJobCardDetails", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pDate", pDate);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@operationNo", operationNo);
                cmd.Parameters.AddWithValue("@operatorID", operatorID);
                cmd.Parameters.AddWithValue("@AcceptedParts", AcceptedParts);
                cmd.Parameters.AddWithValue("@RepeatCycles", RepeatCycles);
                cmd.Parameters.AddWithValue("@dummyCycles", dummyCycles);
                cmd.Parameters.AddWithValue("@ReworkPerformed", ReworkPerformed);
                cmd.Parameters.AddWithValue("@markedforrework", markedforrework);
                cmd.Parameters.AddWithValue("@rejection", rejection);
                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
                cmd.Parameters.AddWithValue("@UpdateTS", UpdatedTs);
                cmd.Parameters.AddWithValue("@downid", downid);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();
                rowAdded = true;
                if (sdr.Read())
                {
                    {
                        rejrework = Convert.ToInt32(sdr["result"].ToString());
                    }
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

            return rejrework;
        }


        //Job card marked rework
        internal static List<string> GetAllRejectionRework(string param, string catagory)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string sqlQuery = string.Empty;
            List<string> result = new List<string>();
            try
            {
                if (string.Equals(param, "Rejection", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(catagory))
                {
                    sqlQuery = "select distinct(catagory) as result from rejectioncodeinformation where catagory is not null order by catagory";
                }
                else if (string.Equals(param, "Rework", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(catagory))
                {
                    sqlQuery = "select distinct(Reworkcatagory) as result  from reworkinformation where Reworkcatagory is not null order by Reworkcatagory";
                }
                else if (string.Equals(param, "Rejection", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(catagory))
                {
                    sqlQuery = "select distinct(rejectionid) as result from rejectioncodeinformation where rejectionid is not null and catagory=@Catagory order by rejectionid ";
                }
                else if (string.Equals(param, "Rework", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(catagory))
                {
                    //sqlQuery = "select distinct(reworkid) as result from reworkinformation where reworkid is not null and ReworkCatagory=@Catagory order by Reworkid";
                    sqlQuery = "select distinct(Reworkinterfaceid) as result from reworkinformation where reworkid is not null and ReworkCatagory=@Catagory order by Reworkinterfaceid";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Catagory", catagory);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    result.Add(Convert.ToString(rdr["result"]));
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
            return result;
        }

        internal static void insertRejRwkDetails(int rejection_Rework_Qty, string Rejectionrework_reason, string updatedBy, DateTime UpdatedTs, int id, int Slno, string param, out bool rowAdded)
        {

            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            string AchievedQty = string.Empty;
            DataTable dt = new DataTable();

            rowAdded = false;
            try
            {
                cmd = new SqlCommand("SS_getJobCardDetails", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RejectionRework_Qty", rejection_Rework_Qty);
                cmd.Parameters.AddWithValue("@RejectionRework_Reason", Rejectionrework_reason);
                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
                cmd.Parameters.AddWithValue("@UpdateTS", UpdatedTs);
                cmd.Parameters.AddWithValue("@RejectionReworkSlno", Slno);
                if (param == "deleteRwk" || param == "deleteRejection")
                {
                    cmd.Parameters.AddWithValue("@id", Slno);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@id", id);
                }

                cmd.Parameters.AddWithValue("@param", param);
                cmd.ExecuteNonQuery();
                rowAdded = true;
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
        }

        //OEE Target //Pawan
        internal static DataTable SearchOEETarget(string Date, string Machine, string Parameter, string efficiency)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[ss_ViewMachineEfficinecyTarget]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", Date);
                cmd.Parameters.AddWithValue("@machineid", Machine);
                cmd.Parameters.AddWithValue("@param", Parameter);
                cmd.Parameters.AddWithValue("@Efficiency", efficiency);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(rdr);
                    dt.GetChanges();
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
            return dt; ;
        }

        internal static void insertUpdateTargetOEE(string MachineId, string parameter, string StartDate, string endDate, string targetValue, string targetType, out string sucessFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("[s_InsertMachineMaster]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", MachineId);
                cmd.Parameters.AddWithValue("@Param", parameter);
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                if (targetType == "AE")
                {
                    cmd.Parameters.AddWithValue("@TargetAE", targetValue);
                    cmd.Parameters.AddWithValue("@TargetOE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetPE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetQE", DBNull.Value);
                }
                else if (targetType == "PE")
                {
                    cmd.Parameters.AddWithValue("@TargetAE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetOE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetPE", targetValue);
                    cmd.Parameters.AddWithValue("@TargetQE", DBNull.Value);
                }
                else if (targetType == "QE")
                {
                    cmd.Parameters.AddWithValue("@TargetAE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetOE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetPE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetQE", targetValue);
                }
                else if (targetType == "OE")
                {
                    cmd.Parameters.AddWithValue("@TargetAE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetOE", targetValue);
                    cmd.Parameters.AddWithValue("@TargetPE", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TargetQE", DBNull.Value);
                }
                //cmd.Parameters.AddWithValue("@InterfaceID","");
                //cmd.Parameters.AddWithValue("@IP", "");
                //cmd.Parameters.AddWithValue("@BulkDataTransferPortNo", "");
                //cmd.Parameters.AddWithValue("@MachinewiseOwner", "");
                //cmd.Parameters.AddWithValue("@QERED", "");
                //cmd.Parameters.AddWithValue("@QEGreen", "");
                cmd.ExecuteNonQuery();
                sucessFailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static List<string> GetAllCycleStartTimes(string startTime, string endTime, string machineId, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> cycleStartTimesList = new List<string>();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetTempandPressureMonitoringGraph_Metso]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Param", param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        cycleStartTimesList.Add(sdr["CycleStart"].ToString());
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
                if (sqlConn != null) sqlConn.Close();
            }
            return cycleStartTimesList;
        }

        internal static string GetCurrentShiftStart(out string endTime)
        {
            string dt = string.Empty;
            endTime = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                cmd = new SqlCommand("[s_GetCurrentShift]", conn);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    dt = sdr["StartTime"].ToString();
                    endTime = sdr["EndTime"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            if (string.IsNullOrEmpty(dt))
                dt = DateTime.Now.ToString();
            if (string.IsNullOrEmpty(endTime))
                endTime = DateTime.Now.ToString();

            return dt;
        }

        internal static DataTable GetScheduleDataForMachine(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            DataTable dtSchedule = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetSONA_ViewScheduleAndSAPData", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", fromDate);
                cmd.Parameters.AddWithValue("@Enddate", toDate);
                cmd.Parameters.AddWithValue("@EquipmentID", machineId);
                cmd.Parameters.AddWithValue("@Planno", planNumber);
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();
                dtSchedule.Load(sdr);
                dtSchedule.AcceptChanges();
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
            return dtSchedule;
        }

        internal static DataTable GetProductionDataForMachine(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            DataTable dtProduction = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetSONA_ViewScheduleAndSAPData", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", fromDate);
                cmd.Parameters.AddWithValue("@Enddate", toDate);
                cmd.Parameters.AddWithValue("@EquipmentID", machineId);
                cmd.Parameters.AddWithValue("@Planno", planNumber);
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();
                dtProduction.Load(sdr);
                dtProduction.AcceptChanges();
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
            return dtProduction;
        }

        internal static DataTable GetDowntimeDataForMachine(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            DataTable dtDowntime = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetSONA_ViewScheduleAndSAPData", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", fromDate);
                cmd.Parameters.AddWithValue("@Enddate", toDate);
                cmd.Parameters.AddWithValue("@EquipmentID", machineId);
                cmd.Parameters.AddWithValue("@Planno", planNumber);
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();
                dtDowntime.Load(sdr);
                dtDowntime.AcceptChanges();
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
            return dtDowntime;
        }

        internal static DataTable GetSummaryDataForMachine(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            DataTable dtSummary = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetSONA_ViewScheduleAndSAPData", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", fromDate);
                cmd.Parameters.AddWithValue("@Enddate", toDate);
                cmd.Parameters.AddWithValue("@EquipmentID", machineId);
                cmd.Parameters.AddWithValue("@Planno", planNumber);
                cmd.Parameters.AddWithValue("@Mode", mode);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();
                dtSummary.Load(sdr);
                dtSummary.AcceptChanges();
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
            return dtSummary;
        }

        internal static List<string> GetSpindleCycleStartTimes(string fromDT, string toDT, string machineID, string Param)
        {
            List<string> cycleStartTimeList = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("S_GetSpindleDetailsreport", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDT);
                cmd.Parameters.AddWithValue("@EndTime", toDT);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", Param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        cycleStartTimeList.Add(sdr["StartDate"].ToString() + "(" + sdr["SpindleRuntime"].ToString() + ")");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) { sdr.Close(); }
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return cycleStartTimeList;
        }

        internal static string GetTimeFormat()
        {
            string timeFormat = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select valueintext from ShopDefaults where Parameter = 'TimeInFormat'", sqlConn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        timeFormat = sdr["valueintext"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sdr != null) { sdr.Close(); }
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return timeFormat;
        }

        internal static List<string> shiftdetail()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> shiftdata = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct ShiftName From  shiftdetails where Running=1", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        shiftdata.Add(rdr["ShiftName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return shiftdata;
        }

        #region Alarm History - Abhilash
        internal static List<string> GetMachineInfo(string plantID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> macdata = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct m.MachineID from machineinformation m inner join PlantMachine p on m.machineid=p.MachineID where m.DNCTransferEnabled = 1 and  (PlantID=@PlantID  or ISNULL(@PlantID,'')='')", conn);
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        macdata.Add(rdr["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return macdata;
        }


        internal static List<Lookup> getAlarmDate()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<Lookup> alarmCategary = new List<Lookup>();
            Lookup lookup1 = new Lookup();
            lookup1.Name = "All";
            lookup1.Value = "ALL";
            alarmCategary.Add(lookup1);
            try
            {
                cmd = new SqlCommand("select * from Focas_AlarmCategory", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Lookup lookup = new Lookup();
                        lookup.Name = rdr[1].ToString();
                        lookup.Value = rdr[0].ToString();
                        alarmCategary.Add(lookup);
                    }
                }
            }
            catch (Exception es)
            {
                Logger.WriteErrorLog(es.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return alarmCategary;
        }

        internal static List<AlarmHistory> AlarmReportSummaryData(string StartTime, string EndTime, string MachineID, string AlarmGroup, string Param, string ShiftName, string FilterBy)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<AlarmHistory> AlarmList = new List<AlarmHistory>();
            AlarmHistory AlarmData = null;
            try
            {
                cmd = new SqlCommand("Focas_GetAlarmReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", StartTime);
                cmd.Parameters.AddWithValue("@EndTime", EndTime);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@AlarmGroup", AlarmGroup);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ShiftName", ShiftName);
                cmd.Parameters.AddWithValue("@AlarmFilter", FilterBy);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        AlarmData = new AlarmHistory();
                        AlarmData.SLNO = Convert.ToInt32(rdr["ID"]);
                        AlarmData.MachineID = rdr["MachineId"].ToString();
                        AlarmData.AlarmNo = rdr["Alarmno"].ToString();
                        AlarmData.Message = rdr["AlarmMessage"].ToString();
                        AlarmData.FirstOccurence = rdr["MinAlarmTime"].ToString();
                        AlarmData.LastOccurence = rdr["MaxAlarmTime"].ToString();
                        AlarmData.NoOfOccur = rdr["NOofOccurences"].ToString();
                        AlarmList.Add(AlarmData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();
            }
            return AlarmList;
        }

        internal static List<AlarmHistory> AlarmReportDetailData(string StartTime, string EndTime, string MachineID, string AlarmGroup, string Param, string ShiftName, string FilterBy)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<AlarmHistory> AlarmList = new List<AlarmHistory>();
            AlarmHistory AlarmData = null;
            try
            {
                cmd = new SqlCommand("Focas_GetAlarmReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", StartTime);
                cmd.Parameters.AddWithValue("@EndTime", EndTime);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@AlarmGroup", AlarmGroup);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ShiftName", ShiftName);
                cmd.Parameters.AddWithValue("@AlarmFilter", FilterBy);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        AlarmData = new AlarmHistory();
                        AlarmData.SLNO = Convert.ToInt32(rdr["ID"]);
                        AlarmData.MachineID = rdr["MachineId"].ToString();
                        AlarmData.AlarmNo = rdr["Alarmno"].ToString();
                        AlarmData.Message = rdr["AlarmMSG"].ToString();
                        AlarmData.StartTime = rdr["AlarmTime"].ToString();
                        AlarmData.Endtime = rdr["Endtime"].ToString();
                        AlarmData.duration = rdr["AlarmDuration"].ToString();
                        //AlarmData.FirstOccurence = rdr["MinAlarmTime"].ToString();
                        //AlarmData.LastOccurence = rdr["MaxAlarmTime"].ToString();
                        //AlarmData.NoOfOccur = rdr["NOofOccurences"].ToString();
                        AlarmList.Add(AlarmData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();
            }
            return AlarmList;
        }

        internal static List<string> GetAlldetails(string name)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            List<string> data = new List<string>();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("s_GetLookups", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data.Add(rdr[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();
            }
            return data;
        }

        internal static AlarmHistory GetAlarmNoPageDetails(string machine, string alarmNo)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            AlarmHistory data = new AlarmHistory();
            try
            {
                cmd = new SqlCommand("select * from Focas_Alarm_PDFDetails where MachineID=@MachineID and AlarmNo=@AlarmNo", Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@AlarmNo", alarmNo);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        data = new AlarmHistory();
                        data.PageStartNo = rdr["PageStart"].ToString();
                        data.PageEndNo = rdr["PageEnd"].ToString();
                        // string path = System.Web.Configuration.WebConfigurationManager.AppSettings["BajajAlarmPDFPath"].ToString();
                        string path = rdr["FilePath"].ToString();
                        string filename = rdr["FileName"].ToString();
                        //if (!File.Exists(System.IO.Path.Combine(System.Environment.CurrentDirectory, path+@"\"+filename)))
                        //{
                        //    data.PDFPath = "";
                        //}
                        //else
                        //{
                        //    data.PDFPath = path + @"\" +  filename;
                        //}
                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/" + path + "/" + filename)))
                        {
                            data.PDFPath = "";
                        }
                        else
                        {
                            data.PDFPath = path + "/" + filename;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("GetAlarmNoPageDetails: " + ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();
            }
            return data;
        }
        #endregion

        #region Energy Data - Deepak
        internal static string GetCostPerKWH()
        {
            DataTable dt = new DataTable();
            string Val = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("select ValueInText from Shopdefaults where Parameter ='CostPerKWH' ", sqlConn);
                cmd.CommandTimeout = 120;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Val = sdr["ValueInText"].ToString();
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return Val;
        }

        internal static string GetUnitCostMessage()
        {
            DataTable dt = new DataTable();
            string Val = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("select ValueInText  from Shopdefaults where Parameter ='UnitCostMessage' ", sqlConn);
                cmd.CommandTimeout = 120;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Val = sdr["ValueInText"].ToString();
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
            return Val;
        }

        internal static List<string> GetEnergyColumnsToBind(out List<string> colPropertyNames)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<string> colHeaderNames = new List<string>();
            colPropertyNames = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from CockpitDefaults where Parameter='TPMTrakWebEnergyViewColumnSettings' and ValueInBool=1 order by ValueInInt ", sqlConn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        colHeaderNames.Add(rdr["ValueInText2"].ToString());
                        colPropertyNames.Add(rdr["ValueInText"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return colHeaderNames;
        }

        internal static DataTable GetEnergyData(DateTime startDate, DateTime endDate, string shift, string selectedMachine, string selectedPlant, string nodeId, string param, string view, string sort, string Proc)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable energyDataTable = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(Proc, sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (Proc.Equals("S_GetSONA_EnergyCockpitDetails_Report", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@dDate", startDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Enddate", endDate.ToString("yyyy-MM-dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@dDate", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Enddate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@MachineID", selectedMachine);
                cmd.Parameters.AddWithValue("@PlantID", selectedPlant);
                cmd.Parameters.AddWithValue("@Node", nodeId);
                cmd.Parameters.AddWithValue("@Parameter", param);
                cmd.Parameters.AddWithValue("@View", view);
                cmd.Parameters.AddWithValue("@sort", sort);
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    energyDataTable.Load(rdr);
                    energyDataTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return energyDataTable;
        }

        internal static List<string> GetAllNodesForMachine(string machineId)
        {
            List<string> nodeIdList = new List<string>();
            string query = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            if (string.IsNullOrEmpty(machineId))
            {
                query = "select distinct NodeId from MachineNodeInformation";
            }
            else
            {
                query = "select NodeId from MachineNodeInformation where MachineId = @MachineId";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        nodeIdList.Add(rdr["NodeId"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return nodeIdList;
        }

        internal static List<string> GetAllEnabledMachines()
        {
            List<string> machineIds = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetLookups", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Machine");
                cmd.Parameters.AddWithValue("@filter", "");
                cmd.Parameters.AddWithValue("@order", "");
                cmd.Parameters.AddWithValue("@filter1", "");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        machineIds.Add(rdr["Machineid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return machineIds;
        }

        internal static List<EnergyChartData> GetEnergyChartData(string plantId, string fromDate, string todate)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<EnergyChartData> energyChartDataList = new List<EnergyChartData>();
            EnergyChartData energyChartData = null;
            SqlDataReader rdr = null;
            try
            {
                double val = 0;
                SqlCommand cmd = new SqlCommand("S_GetSONA_EnergyCockpitDetails", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dDate", fromDate);
                cmd.Parameters.AddWithValue("@Enddate", todate);
                cmd.Parameters.AddWithValue("@Shift", "");
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@Node", "");
                cmd.Parameters.AddWithValue("@Parameter", "");
                cmd.Parameters.AddWithValue("@View", "");
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        energyChartData = new EnergyChartData();
                        energyChartData.name = rdr["MachineID"].ToString() + " (" + rdr["NodeID"].ToString() + ")";
                        if (Double.TryParse(rdr["KW"].ToString(), out val))
                        {
                            energyChartData.y = val;
                        }
                        else
                        {
                            energyChartData.y = 0;
                        }
                        energyChartDataList.Add(energyChartData);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return energyChartDataList;
        }
        #endregion

        #region Node Machine -Abhilash

        internal static DataTable MachineNode()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<NodeMachineId> Nodedetails = new List<NodeMachineId>();
            DataTable Dt = new DataTable();
            try
            {
                cmd = new SqlCommand("Select * from MachineNodeInformation order by PLC_IP_Address, SortOrder", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Dt.Load(rdr);
                    Dt.GetChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return Dt;
        }
        internal static DataTable MachineNode_New(string Plant)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<NodeMachineId> Nodedetails = new List<NodeMachineId>();
            DataTable Dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"	Select M.MachineId,M.NodeInterface,M.NodeId,M.SubSystem,M.EM_ModelNumber,M.PLC_IP_Address,M.Enabled,M.SortOrder,M.UpdatedTS,M.EM_NodeId from MachineNodeInformation M 
inner join EM_PlantMachine PM on PM.MachineID=M.MachineId  where (PM.PlantID=@PlantID or isnull(@PlantID,'')='') order by M.PLC_IP_Address, M.SortOrder", conn);
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Dt.Load(rdr);
                    Dt.GetChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return Dt;
        }
        internal static DataTable MachineNode_Kiswok(string Plant, string Cell)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<NodeMachineId> Nodedetails = new List<NodeMachineId>();
            DataTable Dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"	Select M.MachineId,M.NodeInterface,M.NodeId,M.SubSystem,M.EM_ModelNumber,M.PLC_IP_Address,M.Enabled,M.SortOrder,M.UpdatedTS,M.EM_NodeId from MachineNodeInformation M 
inner join EM_PlantMachine PM on PM.MachineID=M.MachineId 
left join PlantMachineGroups PMG on PMG.MachineID=M.MachineId where (PM.PlantID=@PlantID or isnull(@PlantID,'')='') and (PMG.GroupID=@GroupID or isnull(@GroupID,'')='') order by M.PLC_IP_Address, M.SortOrder", conn);
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                cmd.Parameters.AddWithValue("@GroupID", Cell);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Dt.Load(rdr);
                    Dt.GetChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return Dt;
        }
        internal static bool NodeSave(int SortOrder, string NodeId, string NodeInterface, string MachineId)
        {
            bool ok = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            String Query = @"INSERT INTO [dbo].[MachineNodeInformation] ([MachineID],[SortOrder],[NodeId],[NodeInterface]) 
                                    VALUES (@MachineId,@SortOrder,@NodeId,@NodeInterface)";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@MachineId", MachineId);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@NodeId", NodeId);
                cmd.Parameters.AddWithValue("@NodeInterface", NodeInterface);
                ok = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                ok = false;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return ok;
        }


        internal static DataTable getmachinegriddata(string MachineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable Dt = new DataTable();
            try
            {
                cmd = new SqlCommand("Select * from MachineNodeInformation where MachineId=@MachineId", conn);
                cmd.Parameters.AddWithValue("MachineId", MachineId);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Dt.Load(rdr);
                    Dt.GetChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return Dt;
        }
        internal static string NodeSaveUpdate_EM(string MachineId, string NodeInterface, string NodeId, string SubSystem, string EmModelNumber, string PlcIpAddress, string Enabled, int SortOrder, string EMnodeinterfaceID, string UpdatedTS, out string message)
        {
            string Result = ""; message = "OK";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            String Query = @"if not exists (select * from MachineNodeInformation where NodeId=@nodeid and PLC_IP_Address=@PLC_IP_Address) 
begin
if not exists(select * from MachineNodeInformation where MachineId=@MachineId and NodeId=@NodeId and PLC_IP_Address=@PLC_IP_Address)
BEGIN
INSERT INTO [dbo].[MachineNodeInformation] ([MachineID], [NodeInterface],[EM_NodeId], [NodeId], [SubSystem], [EM_ModelNumber], [PLC_IP_Address], [Enabled], [SortOrder], [UpdatedTS]) 
VALUES (@MachineId, @NodeInterface,@EM_NodeId, @NodeId, @SubSystem, @EM_ModelNumber, @PLC_IP_Address, @Enabled, @SortOrder, @UpdatedTS)
	select 'inserted' as flag
END
else
BEGIN
UPDATE [dbo].[MachineNodeInformation] SET NodeInterface=@NodeInterface,EM_NodeId=@EM_NodeId, NodeId=@NodeId, SubSystem=@SubSystem, EM_ModelNumber=@EM_ModelNumber, Enabled=@Enabled, SortOrder=@SortOrder, UpdatedTS=@UpdatedTS
WHERE MachineId=@MachineId and NodeId=@NodeId and PLC_IP_Address=@PLC_IP_Address
	select 'Updated' as flag
END
end
else
begin

begin
 UPDATE [dbo].[MachineNodeInformation] SET NodeInterface=@NodeInterface,EM_NodeId=@EM_NodeId, NodeId=@NodeId, SubSystem=@SubSystem, EM_ModelNumber=@EM_ModelNumber, Enabled=@Enabled, SortOrder=@SortOrder, UpdatedTS=@UpdatedTS
WHERE NodeId=@NodeId and PLC_IP_Address=@PLC_IP_Address
	select 'Updated' as flag
end
end";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@MachineId", MachineId);
                cmd.Parameters.AddWithValue("@NodeInterface", NodeInterface);
                cmd.Parameters.AddWithValue("@EM_NodeId", EMnodeinterfaceID);
                cmd.Parameters.AddWithValue("@NodeId", NodeId);
                cmd.Parameters.AddWithValue("@SubSystem", SubSystem);
                cmd.Parameters.AddWithValue("@EM_ModelNumber", EmModelNumber);
                cmd.Parameters.AddWithValue("@PLC_IP_Address", PlcIpAddress);
                cmd.Parameters.AddWithValue("@Enabled", Enabled);
                cmd.Parameters.AddWithValue("@UpdatedTS", UpdatedTS);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Result = sdr["flag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                if (ex.Message.Contains("cs_ipaddrsortorder"))
                {
                    message = "Please verify IPAddress-Index combination for uniqueness";
                }
                else if (ex.Message.Contains("cs_ipaddrnodeinterface"))
                {
                    message = "Please verify IPAddress-Node Interface combination for uniqueness";
                }
                else
                {
                    message = ex.Message;
                }
                Result = "";
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return Result;
        }
        internal static bool NodeSaveUpdate(string MachineId, string NodeInterface, string NodeId, string SubSystem, string EmModelNumber, string PlcIpAddress, string Enabled, int SortOrder, string EMnodeinterfaceID, string UpdatedTS, out string message)
        {
            bool ok = false;
            message = "OK";
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            String Query = @"if not exists(select * from MachineNodeInformation where NodeId=@NodeId)
                                       BEGIN
                                       INSERT INTO [dbo].[MachineNodeInformation] ([MachineID], [NodeInterface],[EM_NodeId], [NodeId], [SubSystem], [EM_ModelNumber], [PLC_IP_Address], [Enabled], [SortOrder], [UpdatedTS]) 
                                       VALUES (@MachineId, @NodeInterface,@EM_NodeId, @NodeId, @SubSystem, @EM_ModelNumber, @PLC_IP_Address, @Enabled, @SortOrder, @UpdatedTS)
                                       END
                                       else
                                       BEGIN
                                       UPDATE [dbo].[MachineNodeInformation] SET MachineId=@MachineId, NodeInterface=@NodeInterface,EM_NodeId=@EM_NodeId, NodeId=@NodeId, SubSystem=@SubSystem, EM_ModelNumber=@EM_ModelNumber, Enabled=@Enabled,PLC_IP_Address=@PLC_IP_Address,  SortOrder=@SortOrder, UpdatedTS=@UpdatedTS
            WHERE NodeId=@NodeId
                                       END";

            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@MachineId", MachineId);
                cmd.Parameters.AddWithValue("@NodeInterface", NodeInterface);
                cmd.Parameters.AddWithValue("@EM_NodeId", EMnodeinterfaceID);
                cmd.Parameters.AddWithValue("@NodeId", NodeId);
                cmd.Parameters.AddWithValue("@SubSystem", SubSystem);
                cmd.Parameters.AddWithValue("@EM_ModelNumber", EmModelNumber);
                cmd.Parameters.AddWithValue("@PLC_IP_Address", PlcIpAddress);
                cmd.Parameters.AddWithValue("@Enabled", Enabled);
                cmd.Parameters.AddWithValue("@UpdatedTS", UpdatedTS);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    ok = true;
                }
                //ok = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                if (ex.Message.Contains("cs_ipaddrsortorder"))
                {
                    message = "Please verify IPAddress-Index combination for uniqueness";
                }
                else if (ex.Message.Contains("cs_ipaddrnodeinterface"))
                {
                    message = "Please verify IPAddress-Node Interface combination for uniqueness";
                }
                else
                {
                    message = ex.Message;
                }
                ok = false;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return ok;
        }
        internal static bool NodeDelete(string NodeId, string ipaddr)
        {
            bool ok = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"Delete from MachineNodeInformation where NodeId= @NodeId", conn);
                cmd.Parameters.AddWithValue("@NodeId", NodeId);
                //cmd.Parameters.AddWithValue("@ipaddr", ipaddr);
                ok = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                ok = false;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return ok;
        }

        internal static List<string> GetMachineInfo_nodeinterface()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> macdata = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct MachineID From machineinformation where TPMTrakEnabled = 1 Order By MachineID", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        macdata.Add(rdr["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return macdata;
        }
        #endregion


        internal static void SaveProcessMasterData(int ProcessID, string MachineID, string Parameters, string Component, string LSL, string USL, string LowerOperatingZoneLimit, string UpperOperatingZoneLimit, string LowerWarningZoneLimit, string UpperWarningZoneLimit, out int rowCount)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = string.Empty;
                query = @"if not exists(select * from ProcessParametersMaster where  [ProcessID] =@ProcessID and MachineID=@MachineID and Parameters=@Parameters and Component=@Component)
                                    BEGIN
                                    INSERT INTO [dbo].[ProcessParametersMaster] ([MachineID],[Parameters],[Component],[LSL],[USL],[LowerOperatingZoneLimit],UpperOperatingZoneLimit,LowerWarningZoneLimit,UpperWarningZoneLimit) 
                                         VALUES (@MachineID,@Parameters,@Component,@LSL,@USL,@LowerOperatingZoneLimit,@UpperOperatingZoneLimit,@LowerWarningZoneLimit,@UpperWarningZoneLimit)
                                    END
                                    else
                                    BEGIN
                                    UPDATE [dbo].[ProcessParametersMaster] SET MachineID=@MachineID,Parameters=@Parameters,Component=@Component,LSL=@LSL,USL=@USL,LowerOperatingZoneLimit=@LowerOperatingZoneLimit,UpperOperatingZoneLimit=@UpperOperatingZoneLimit,LowerWarningZoneLimit=@LowerWarningZoneLimit,
                                    UpperWarningZoneLimit=@UpperWarningZoneLimit WHERE [ProcessID] =@ProcessID And MachineID=@MachineID and Parameters=@Parameters and Component=@Component;
                                    END ";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@ProcessID", ProcessID));
                cmd.Parameters.Add(new SqlParameter("@MachineID", MachineID));
                cmd.Parameters.Add(new SqlParameter("@Parameters", Parameters));
                cmd.Parameters.Add(new SqlParameter("@Component", Component));
                cmd.Parameters.Add(new SqlParameter("@LSL", LSL));
                cmd.Parameters.Add(new SqlParameter("@USL", USL));
                cmd.Parameters.Add(new SqlParameter("@LowerOperatingZoneLimit", LowerOperatingZoneLimit));
                cmd.Parameters.Add(new SqlParameter("@UpperOperatingZoneLimit", UpperOperatingZoneLimit));
                cmd.Parameters.Add(new SqlParameter("@LowerWarningZoneLimit", LowerWarningZoneLimit));
                cmd.Parameters.Add(new SqlParameter("@UpperWarningZoneLimit", UpperWarningZoneLimit));
                rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static List<processparaconfig> GetProcessMasterData()
        {
            List<processparaconfig> data = new List<processparaconfig>();
            processparaconfig proinfo = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"Select * from ProcessParametersMaster inner join machineinformation 
  on ProcessParametersMaster.machineid = machineinformation.machineid where machineinformation.TPMTrakEnabled = 1  ", sqlConn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        proinfo = new processparaconfig();
                        proinfo.SLNO = rdr["ProcessID"].ToString();
                        proinfo.MachineID = rdr["MachineID"].ToString();
                        proinfo.Parameter = rdr["Parameters"].ToString();
                        proinfo.Component = rdr["Component"].ToString();
                        proinfo.LowError = rdr["LSL"].ToString();
                        proinfo.UppError = rdr["USL"].ToString();
                        proinfo.LowOp = rdr["LowerOperatingZoneLimit"].ToString();
                        proinfo.UppOp = rdr["UpperOperatingZoneLimit"].ToString();
                        proinfo.LowWar = rdr["LowerWarningZoneLimit"].ToString();
                        proinfo.UppWar = rdr["UpperWarningZoneLimit"].ToString();
                        data.Add(proinfo);
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
                if (rdr != null) rdr.Close();
            }
            return data;
        }

        internal static List<string> GetAllPArameter()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> ParameterList = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct Parameters from ProcessParametersMaster", sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ParameterList.Add(rdr["Parameters"].ToString());
                }
                rdr.Close();
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
            return ParameterList;

        }

        internal static List<string> GetAllMachines(string plantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> MachineList = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct machineid from machineinformation where TPMTrakEnabled = 1", sqlConn);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MachineList.Add(rdr["machineid"].ToString());
                }
                rdr.Close();
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
            return MachineList;
        }
        internal static List<string> GetAllMachinesFromMaster()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> MachineList = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct machineid from machineinformation", sqlConn);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MachineList.Add(rdr["machineid"].ToString());
                }
                rdr.Close();
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
            return MachineList;
        }

        internal static void deleteProcessMasterData(int deletionId, out int RowsAffected)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = "Delete from ProcessParametersMaster where ProcessID = @ProcessID";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@ProcessID", deletionId));
                RowsAffected = cmd.ExecuteNonQuery();
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
        }

        internal static List<string> GetComponentId()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> CompList = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct componentid from componentinformation", sqlConn);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CompList.Add(rdr["componentid"].ToString());
                }
                rdr.Close();
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
            return CompList;
        }

        #region "------Shift Level Drill Down--------------"
        static public bool ThirdLevelDrillDown
        {
            get
            {
                return Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["ShiftThirdLevelDrillDown"]);
            }
        }
        #endregion

        #region "------Hour Level Drill Down--------------"
        static public bool HourlyLevelDrillDown
        {
            get
            {
                return Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["HourlyLevelDrillDown"]);
            }
        }
        #endregion

        #region "------Hour Level Drill Down--------------"
        static public string AppDefaultView
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["DefaultView"].ToString();
            }
        }
        #endregion

        #region Machine SubSystem Details

        internal static List<MachineSubsystemDetails> Getmachinesubsystemdetails(string Machineid)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<MachineSubsystemDetails> machinesubsystemlist = new List<MachineSubsystemDetails>();
            string query;
            if (Machineid == "All")
            {
                query = @" select s.[MachineId],
							 --  s.[SubSystemID],
							     m.category, 
							    s.[AssetId],
								  s.[AssetDescription],
								   s.[WorkCenter] from[SONA_AssetMasterInfo] S
						    	inner join[SONA_SubSystem_MasterInfo] M on s.[SubSystemID] = m.[CategoryId]";
            }
            else
            {
                query = @"select s.[MachineId],
							 -- s.[SubSystemID],
							     m.category, 
							    s.[AssetId],
								  s.[AssetDescription],
								   s.[WorkCenter] from[SONA_AssetMasterInfo] S
								inner join[SONA_SubSystem_MasterInfo] M on s.[SubSystemID] = m.[CategoryId] where MachineId=@MachineId";
            }
            MachineSubsystemDetails Machinesubsystemdata = null;
            int i = 1;
            try
            {
                cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineId", Machineid);
                cmd.ExecuteNonQuery();
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Machinesubsystemdata = new MachineSubsystemDetails();
                        Machinesubsystemdata.SLNO = i.ToString();
                        i++;
                        Machinesubsystemdata.MachineID = rdr["MachineId"].ToString();
                        Machinesubsystemdata.Subsystem = rdr["category"].ToString();
                        Machinesubsystemdata.EquipmentID = rdr["AssetId"].ToString();
                        Machinesubsystemdata.EquipmentDetails = rdr["AssetDescription"].ToString();
                        Machinesubsystemdata.rowedit = false;
                        machinesubsystemlist.Add(Machinesubsystemdata);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                throw;
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();
            }
            return machinesubsystemlist;
        }

        internal static void savesubsystemdata(string machineID, string subsystem, string equip_ID, string equip_description)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "";
            query = @"if not exists(select * from SONA_AssetMasterInfo where  [MachineId] =@MachineId and SubSystemID=@SubSystemID)
                                    BEGIN
                                    INSERT INTO [dbo].[SONA_AssetMasterInfo] ([MachineID],[SubSystemID],[AssetId],[AssetDescription]) 
                                         VALUES (@MachineID,@SubSystemID,@AssetId,@AssetDescription)
                                    END
                                    else
                                    BEGIN
									UPDATE [dbo].[SONA_AssetMasterInfo] 
											SET MachineId=@MachineId,SubSystemID=@SubSystemID,AssetId=@AssetId,AssetDescription=@AssetDescription
											WHERE MachineId = @MachineId and SubSystemID=@SubSystemID;
                                    END ";
            try
            {
                cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineId", machineID);
                cmd.Parameters.AddWithValue("@SubSystemID", subsystem);
                cmd.Parameters.AddWithValue("@AssetId", equip_ID);
                cmd.Parameters.AddWithValue("@AssetDescription", equip_description);
                //cmd.Parameters.AddWithValue("@WorkCenter", PlantID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                throw;
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
        }


        internal static List<subsystem> Getsubsysteminfo()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            //List<string> data = new List<string>();
            List<subsystem> subsystemlist = new List<subsystem>();
            subsystem subsystemdata = null;
            try
            {
                cmd = new SqlCommand("Select * from SONA_SubSystem_MasterInfo", conn);
                cmd.ExecuteNonQuery();
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        subsystemdata = new subsystem();
                        subsystemdata.categoryID = Convert.ToInt32(rdr["CategoryId"]);
                        subsystemdata.Categorydescription = rdr["category"].ToString();
                        subsystemlist.Add(subsystemdata);
                        //data.Add(rdr["category"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return subsystemlist;
            //return data;
        }


        internal static List<string> getmachineidfromplant(string Plantid)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> data = new List<string>();
            string query = string.Empty;
            if (Plantid == "All" || Plantid == "" || Plantid.Contains("All"))
            {
                query = @"SELECT m.[machineid]
							FROM [dbo].[machineinformation] m
							inner join PlantMachine p on p.MachineID = m.machineid
							inner join PlantInformation pi on p.PlantID = pi.PlantID
							where m.TPMTrakEnabled = 1";
            }
            else
            {
                query = @"SELECT m.[machineid]
							FROM [dbo].[machineinformation] m
							inner join PlantMachine p on p.MachineID = m.machineid
							inner join PlantInformation pi on p.PlantID = pi.PlantID
							where m.TPMTrakEnabled = 1
							and pi.plantid = @PlantID";
            }
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PlantID", Plantid);
                cmd.ExecuteNonQuery();
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data.Add(rdr["machineid"].ToString());
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
            data.Insert(0, "All");
            return data;
        }


        internal static string Getsubsystemid(string subsystems)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string data = string.Empty;
            try
            {
                cmd = new SqlCommand("Select CategoryId from SONA_SubSystem_MasterInfo where category=@category", conn);
                cmd.Parameters.AddWithValue("@category", subsystems);
                cmd.ExecuteNonQuery();
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = rdr["CategoryId"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return data;
        }

        #endregion

        internal static DataTable GetAllPlantsInfo()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("Select * from PlantInformation", conn);
            SqlDataReader rdr = null;
            DataTable dtplantInfo = new DataTable();
            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtplantInfo.Load(rdr);
                    dtplantInfo.GetChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dtplantInfo;
        }

        internal static bool UpdatePlantInfo(string plantId, string plantDesc, string plantCode, out string message)
        {
            bool ok = false;
            message = "OK";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = @"if not exists(select * from PlantInformation where PlantID=@PlantID)
                                    BEGIN
                                    INSERT INTO [dbo].[PlantInformation] ([PlantID], [Description], [PlantCode]) 
                                    VALUES (@PlantID, @Description, @PlantCode)
                                    END
                                    else
                                    BEGIN
                                    UPDATE [dbo].[PlantInformation] SET Description=@Description, PlantCode=@PlantCode
									WHERE PlantID=@PlantID
                                    END";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@Description", plantDesc);
                cmd.Parameters.AddWithValue("@PlantCode", plantCode);
                ok = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                message = ex.Message;
                ok = false;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return ok;
        }

        internal static List<string> GetMachineInfo_IPAddress()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<string> IPAdressList = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct PLC_IP_Address from MachineNodeInformation order by PLC_IP_Address", sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    IPAdressList.Add(rdr["PLC_IP_Address"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                //throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return IPAdressList;
        }
        internal static List<string> GetCellIDs(string Plant)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"SELECT distinct GroupID FROM PlantMachineGroups WHERE (PlantID=@PlantID OR ISNULL(@PlantID,'')='')", con);
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                list.Add("All");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["GroupID"].ToString());
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
            return list;
        }
        internal static DataTable GetMachineGridDataForIPAddress(string ipAddress)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable Dt = new DataTable();
            try
            {
                cmd = new SqlCommand("Select * from MachineNodeInformation where (PLC_IP_Address=@ipaddr or @ipaddr='All')", conn);
                cmd.Parameters.AddWithValue("@ipaddr", ipAddress);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Dt.Load(rdr);
                    Dt.GetChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return Dt;
        }

        #region "Rework Details"
        internal static DataTable GetAllReworkDetails(string fromDate, string toDate, string machineID)
        {
            DataTable reworkDetails = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            //string query = @"SELECT MachineId, FGMaterialID, OperationID, OperatorID, SUM(Palletcount) as ReworkCount,PlanNo, PVNo, FGBatchID, ChildPartID, ChildBatchID FROM SONA_ReworkDetails Where (machineid=@MachineID  or ISNULL(@MachineID,'')='') and Sttime>=@StartTime and sttime<=@EndTime Group by MachineId, FGMaterialID, OperationID, OperatorID, PlanNo, PVNo, FGBatchID, ChildPartID, ChildBatchID Order by MachineId,Planno";


            string query = @"SELECT SONA_ReworkDetails.MachineId,M.description as Machinedescription, FGMaterialID, OperationID, OperatorID, SUM(Palletcount)				as ReworkCount,PlanNo, PVNo, FGBatchID, ChildPartID, ChildBatchID FROM SONA_ReworkDetails 
							INNER JOIN machineinformation M on M.machineid = SONA_ReworkDetails.MachineId
							Where (SONA_ReworkDetails.machineid=@MachineID  or ISNULL(@MachineID,'')='') and Sttime>=@StartTime and sttime<=@EndTime 
							Group by SONA_ReworkDetails.MachineId,M.description, FGMaterialID, OperationID, OperatorID, PlanNo, PVNo, FGBatchID, ChildPartID, ChildBatchID 
							Order by MachineId,Planno";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StartTime", fromDate);
                cmd.Parameters.AddWithValue("@EndTime", toDate);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    reworkDetails.Load(rdr);
                    reworkDetails.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return reworkDetails;
        }
        #endregion

        internal static DataTable GetPartWeightData(string component)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = "";
            query = component == "" ? "Select * from SONA_PartWeight_Master" : "Select * from SONA_PartWeight_Master where CompId like '%" + component + "%'";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CompId", component);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.Columns["CompId"].ColumnName = "ComponentID";
                    dt.Columns["UnitWeight"].ColumnName = "UnitWeight";
                    dt.Columns[2].ColumnName = "EffectiveDateTime";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }

        internal static bool Savepartweight(string ComponentID, string UnitWeight, string effectivedate)
        {
            bool savechanges = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @" If EXISTS (SELECT * from SONA_PartWeight_Master where CompID = @CompID and EffectiveTS=@EffectiveTS)
								BEGIN  UPDATE SONA_PartWeight_Master SET UnitWeight = @UnitWeight , UpdatedTS = @date  WHERE CompID = @CompID and EffectiveTS=@EffectiveTS END
							ELSE
							BEGIN INSERT INTO  SONA_PartWeight_Master(CompID,UnitWeight,EffectiveTS,UpdatedTS)
									VALUES(@CompID,@UnitWeight,@EffectiveTS,@date)   
							END";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CompID", ComponentID);
                cmd.Parameters.AddWithValue("@UnitWeight", UnitWeight);
                cmd.Parameters.AddWithValue("@EffectiveTS", effectivedate);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.ExecuteNonQuery();
                savechanges = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return savechanges;
        }

        internal static DataTable GetEnergyLivedata(string MachineID, string Nodeid, string Parameter)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            MachineID = MachineID == "All" ? "" : MachineID;
            Nodeid = Nodeid == "All" ? "" : Nodeid;
            try
            {
                cmd = new SqlCommand("S_GetSONA_EnergyCockpitDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReportType", Parameter);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Node", Nodeid);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }

        internal static DataTable GetEnergyReportDataSona(DateTime fromDate, DateTime toDate, string Proc)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(Proc, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Enddate", toDate.ToString("yyyy-MM-dd"));
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }

        public static List<string> GetAllPlantsForPlantInfo(string Username)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> plantid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select plantid from PlantInformation order by Plantid", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                //  cmd.Parameters.AddWithValue(@"name", "Plant");
                //  plantid.Add("All");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    plantid.Add(rdr["plantid"].ToString());
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
            return plantid;
        }

        #region "Get Plant Machine Group"
        internal static string GetGroupOrder(string plantID, string groupId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string str = null;
            try
            {
                string query = "select * from [dbo].[PlantMachineGroups] where PlantID=@PlantID and GroupID=@GroupID";// Plantcode= @plantCode and//Rk
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@GroupID", groupId);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        str = rdr["GroupOrder"] != DBNull.Value ? rdr["GroupOrder"].ToString() : "";
                    }
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
            return str;
        }
        #endregion

        internal static string GetEOLMachine(string plant)
        {
            string ret = "";
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                string query = @"SELECT MachineID FROM PlantMachine WHERE PlantID=@plant AND EndOfLineMachine=1";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", plant);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    ret = rdr["MachineID"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SetEOLMachine: " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return ret;
        }

        internal static List<LineGroupModel> GetLineGroupMachineInfo(string plantID, string groupId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<LineGroupModel> dt = new List<LineGroupModel>();
            try
            {
                string query = @"select distinct machineid,0 as EndOfGroupMachine,0 as EndOfLineMachine,0 as MachineSequence,0 as chkAssignMachin from [dbo].[PlantMachine]
                                    where plantid=@PlantID and machineid not in(select distinct machineid from [dbo].[PlantMachineGroups] where PlantID=@PlantID and machineid is not null) 
                                    UNION
                                    select distinct machineid,EndOfGroupMachine,EndOfLineMachine,MachineSequence,1 as chkAssignMachin from [PlantMachineGroups]
                                    where plantid=@PlantID and groupid=@GroupID  and (machineid is not null and MachineID<>'') order by MachineSequence";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@GroupID", groupId);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        LineGroupModel obj = new LineGroupModel();
                        obj.MachineId = rdr["machineid"] != DBNull.Value ? rdr["machineid"].ToString() : "";
                        obj.EndOfGroupMachine = rdr["EndOfGroupMachine"] != DBNull.Value ? Convert.ToInt32(rdr["EndOfGroupMachine"]) : 0;
                        obj.EndOfLineMachine = rdr["EndOfLineMachine"] != DBNull.Value ? Convert.ToInt32(rdr["EndOfLineMachine"]) : 0;
                        obj.MachineSequence = rdr["MachineSequence"] != DBNull.Value ? Convert.ToInt32(rdr["MachineSequence"]) : 0;
                        if (rdr["chkAssignMachin"] != DBNull.Value)
                            obj.chkAssignMachine = Convert.ToInt32(rdr["chkAssignMachin"]) == 1 ? true : false;
                        else
                            obj.chkAssignMachine = false;
                        dt.Add(obj);
                    }
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

        internal static int DeleteLineGroupAssignInfo(string plantID, string groupId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordDeleted = 0;
            try
            {
                string sqlQuery = "delete from [dbo].[PlantMachineGroups] where PlantID = @plantID and groupid=@GroupID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@GroupID", groupId);
                recordDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return recordDeleted;
        }

        internal static void SetEOLMachine(string plant, string machine)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                string query = @"UPDATE PlantMachine SET EndOfLineMachine=0,UpdatedTS=@UpdatedTS WHERE PlantID=@plant; UPDATE PlantMachine SET EndOfLineMachine=1,UpdatedTS=@UpdatedTS WHERE MachineID=@machid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machid", machine);
                cmd.Parameters.AddWithValue("@plant", plant);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SetEOLMachine: " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        internal static int AddLineGroupInfo(string plantID, string GroupId, string GroupOrder, string MachineID, int EndOfGroupMachine, int MachineSequence, string description, string EndOfLineMachine)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int val = 0;
            try
            {
                string sqlQuery = string.Empty;
                sqlQuery = @"INSERT INTO [dbo].[PlantMachineGroups]
                                       ([PlantID]
                                       ,[MachineID]
                                       ,[GroupID]
                                       ,[GroupOrder]
		                               ,[description]
                                       ,[EndOfGroupMachine]
                                       ,[EndOfLineMachine]
                                       ,[MachineSequence]
                                       ,[UpdatedTS])
                            Values(@PlantID,@MachineID,@GroupID,@GroupOrder,@description,@EndOfGroupMachine,@EndOfLineMachine,@MachineSequence,@UpdatedTS)";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@GroupID", GroupId);
                cmd.Parameters.AddWithValue("@GroupOrder", GroupOrder);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@EndOfGroupMachine", EndOfGroupMachine);
                cmd.Parameters.AddWithValue("@MachineSequence", MachineSequence);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@EndOfLineMachine", EndOfLineMachine);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                val = cmd.ExecuteNonQuery();
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
            return val;
        }

        internal static bool Savedata(string Lines, string groupid)
        {
            bool saved = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;

            string Query = @"if not exists(select * from [PlantMachineGroups] where [PlantID]=@PlantID and GroupID=@GroupID)
                                    BEGIN
                                    INSERT INTO [dbo].[PlantMachineGroups]([PlantID], GroupID,MachineID) 
                                    VALUES (@PlantID, @GroupID,'')
                                    END";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@PlantID", Lines);
                cmd.Parameters.AddWithValue("@GroupID", groupid);
                cmd.ExecuteNonQuery();
                saved = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return saved;
        }


        internal static List<string> GetCockpitdetails(out List<string> colProperty)
        {
            colProperty = new List<string>();
            List<string> Colheader = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from CockpitDefaults where Parameter='CockpitGridColumn' and ValueInBool=1 order by ValueInInt ", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Colheader.Add(rdr["ValueInText2"].ToString());
                        colProperty.Add(rdr["ValueInText"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Colheader;
        }


        internal static DataTable getcockpitaggregatedata(DateTime fromDate, DateTime toDate, string plantid, string machineid, string shiftname)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_GetShiftAgg_ProductionReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", plantid);
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@ShiftName", shiftname);
                cmd.Parameters.AddWithValue("@OperatorID", "");
                cmd.Parameters.AddWithValue("@ReportType", "Machinewise");
                cmd.Parameters.AddWithValue("@Parameter", "Consolidated");
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.Columns.Add("MachineDescription");
                dt.Columns.Add("AvailabilityEfficiency");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return dt;
        }


        internal static void Check16losses(bool @checked)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("Update [CockpitDefaults] set [ValueInInt]=@data,UpdatedTS=@UpdatedTS where [Parameter] = 'ExcludeTPMTrakDown'", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@data", @checked == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }


        internal static bool check16lossesshow()
        {
            bool showhide = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("select * from  [CockpitDefaults]  where [Parameter] = 'ExcludeTPMTrakDown'", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        int data = 0;
                        Int32.TryParse(rdr["ValueInInt"].ToString(), out data);
                        showhide = data == 1 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return showhide;
        }


        internal static List<string> Getcomponentfromtxt(string Component)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> component = new List<string>();
            string query = @"select DISTINCT componentId from componentinformation where componentId Like '%" + Component + "%'";
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        component.Add(rdr["componentId"].ToString());
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
            return component;
        }


        internal static List<string> getcomponentbydatecomponent(string fromdate, string todate, string Component)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> component = new List<string>();
            string query = "";
            try
            {
                if (!string.IsNullOrEmpty(Component) && !(string.IsNullOrEmpty(fromdate)) && !(string.IsNullOrEmpty(todate)))
                {
                    query = @"select distinct c.componentid from componentinformation c inner join (select distinct comp from autodata where sttime >= @startdate and ndtime <= @endtime) a on a.comp = c.InterfaceID and c.componentid like '%" + Component + "%'";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@startdate", fromdate);
                    cmd.Parameters.AddWithValue("@endtime", todate);
                }
                else if (string.IsNullOrEmpty(Component))
                {
                    query = @"select distinct c.componentid from componentinformation c inner join (select distinct comp from autodata where sttime >= @startdate and ndtime <= @endtime)  a on a.comp = c.InterfaceID";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@startdate", fromdate);
                    cmd.Parameters.AddWithValue("@endtime", todate);
                }
                else
                {
                    query = @"select DISTINCT componentId from componentinformation where componentId Like '" + Component + "%'";
                    cmd = new SqlCommand(query, conn);
                }
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        component.Add(rdr["componentId"].ToString());
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
            return component;
        }


        internal static List<string> GetMoIDS(string mOText, string type)
        {

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> MOID = new List<string>();
            string query = "";
            if (string.IsNullOrEmpty(mOText) || string.IsNullOrEmpty(type))
            {
                query = @"select  distinct MoNumber from MOSchedule where  ( ( MOStatus='4' and [Status]='R' )   or  ( MOStatus='5' and Status='C'))  order by MONumber";
            }
            else
            {
                if (type.Equals("MOsearchwithlike", StringComparison.OrdinalIgnoreCase))
                    query = @"select  distinct MoNumber from MOSchedule where  ( ( MOStatus='4' and [Status]='R' )   or  ( MOStatus='5' and Status='C')) and MoNumber like '%" + mOText + "%' order by MONumber ";
                else if (type.Equals("MOExactSearch", StringComparison.OrdinalIgnoreCase))
                    query = @"select  distinct MoNumber from MOSchedule where  ( ( MOStatus='4' and [Status]='R' )   or  ( MOStatus='5' and Status='C')) and MoNumber = @MoNumber order by MONumber";
                else
                    query = @"select  distinct MoNumber from MOSchedule where  ( ( MOStatus='4' and [Status]='R' )   or  ( MOStatus='5' and Status='C'))  order by MONumber";
            }
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MoNumber", mOText);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        MOID.Add(rdr["Monumber"].ToString());
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
            return MOID;
        }

        internal static void SaveCockpitDefaultsSettings(string Parameter, string ValueInText)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("if not exists(select * from CockpitDefaults where Parameter=@parameter and ValueInText=@valueInText)begin update [CockpitDefaults] set ValueInText=@valueInText,UpdatedTS=@UpdatedTS where Parameter=@parameter end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }

        internal static void SaveCockpitDefaultsSettings(string Parameter, string ValueInText, int ValueInInt)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("if not exists(select * from CockpitDefaults where Parameter=@parameter and ValueInText=@valueInText)begin update [CockpitDefaults] set ValueInText=@valueInText,ValueInInt=@valueInInt,UpdatedTS=@UpdatedTS where Parameter=@parameter end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@valueInInt", ValueInInt);
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }

        internal static void saveMachineSetting(string Value)
        {

            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("update [CockpitDefaults] set ValueInText=@valueInText,ValueInText2=@valueInText2,UpdatedTS=@UpdatedTS where Parameter='TpmEnbMac'", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", Value);
                sqlCommand.Parameters.AddWithValue("@valueInText2", Value);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }

        internal static string GetPlannedDownTimeValues(string Parameter)
        {
            string valueInText = string.Empty;
            SqlConnection connection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand command = new SqlCommand("select [ValueInText] from [CockpitDefaults] where [Parameter]=@parameter", connection);
                command.Parameters.AddWithValue("@parameter", Parameter);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        valueInText = reader["ValueInText"].ToString();
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return valueInText;
        }

        internal static string GetThreshold(string ValueInText)
        {
            string type1 = string.Empty;
            SqlConnection connection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand command = new SqlCommand("select ValueInText2 from ShopDefaults where Parameter='ANDONStatusThreshold' and ValueInText=@ValueInText", connection);
                command.Parameters.AddWithValue("@ValueInText", ValueInText);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        type1 = reader["ValueInText2"].ToString();
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return type1;
        }

        internal static void setCheckBoxListChecked(out bool checkPlantOee, out bool checkTimeConsolidated)
        {
            List<string> chkItem = new List<string>();
            checkPlantOee = false;
            checkTimeConsolidated = false;
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("select * from [ShopDefaults] where Parameter='Machine AE'", sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        chkItem.Add(reader["ValueInText"].ToString());
                    }
                    if (chkItem[0] == "Plant OEE")
                    {
                        checkPlantOee = true;
                    }
                    if (chkItem[0] == "Time Consolidated")
                    {
                        checkTimeConsolidated = true;
                    }
                    if (chkItem.ElementAtOrDefault(1) != null)
                    {
                        if (chkItem[1] == "Time Consolidated")
                        {
                            checkTimeConsolidated = true;
                        }
                        if (chkItem[1] == "Plant OEE")
                        {
                            checkPlantOee = true;
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }

        internal static void SaveShopDefaultSettings(string Parameter, string ValueInText, int ValueInInt)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("if not exists(select * from ShopDefaults where Parameter=@parameter and ValueInText=@valueInText)begin update ShopDefaults set ValueInText=@valueInText,ValueInInt=@valueInInt,UpdatedTS=@UpdatedTS where Parameter=@parameter end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@valueInInt", ValueInInt);
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }

        internal static void SaveShopDefaultSettings(string Parameter, string ValueInText)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("if not exists(select * from ShopDefaults where Parameter=@parameter and ValueInText=@valueInText) begin update ShopDefaults set ValueInText=@valueInText,UpdatedTS=@UpdatedTS where Parameter=@parameter end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }

        internal static void SaveShopDefaultSettingsForTextbox(string Parameter, int ValueInInt)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("if not exists(select * from ShopDefaults where Parameter=@parameter and ValueInInt=@valueInInt)begin update ShopDefaults set ValueInInt=@valueInInt,UpdatedTS=@UpdatedTS where Parameter=@parameter end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInInt", ValueInInt);
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }

        internal static void SaveShopDefaultSettingsForValueinText2(string Parameter, string ValueInText, string ValueInText2)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("if not exists(select * from ShopDefaults where Parameter=@parameter and ValueInText=@valueInText and ValueInText2=@valueInText2)begin update ShopDefaults set ValueInText2=@valueInText2,UpdatedTS=@UpdatedTS where ValueInText=@valueInText and Parameter=@parameter end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@valueInText2", ValueInText2);
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }
        internal static void SaveShopDefaultSettingsForValueInInt(string Parameter, string ValueInText, string ValueInInt)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"if exists(select * from ShopDefaults where Parameter=@parameter and ValueInText=@valueInText)
begin
update ShopDefaults set ValueInInt = @ValueInInt,UpdatedTS=@UpdatedTS where  Parameter = @parameter and ValueInText = @valueInText
end
else
                    begin
                     insert into ShopDefaults(Parameter, ValueInText, ValueInInt,UpdatedTS) values(@parameter, @valueInText, @ValueInInt,@UpdatedTS)
end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@ValueInInt", string.IsNullOrEmpty(ValueInInt) ? 0 : Convert.ToInt32(ValueInInt));
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }
        internal static void SaveShopDefaultSettingsForValueInInt(string Parameter, string ValueInInt)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"if exists(select * from ShopDefaults where Parameter=@parameter)
begin
update ShopDefaults set ValueInInt = @ValueInInt,UpdatedTS=@UpdatedTS where  Parameter = @parameter
end
else
                    begin
                     insert into ShopDefaults(Parameter, ValueInInt,UpdatedTS) values(@parameter, @ValueInInt,@UpdatedTS)
end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ValueInInt", string.IsNullOrEmpty(ValueInInt) ? 0 : Convert.ToInt32(ValueInInt));
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }
        internal static void SetDataForCheckBox(string ValueInText, bool isChecked)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            string query = string.Empty;
            switch (isChecked)
            {
                case true:
                    query = @"if not exists(select * from ShopDefaults where Parameter='Machine AE' and ValueInText=@valueInText)
                                    begin
                                        insert into ShopDefaults(Parameter,ValueInText,UpdatedTS) values('Machine AE',@valueInText,@UpdatedTS)
                                    end";
                    break;
                case false:
                    query = @"if exists(select * from ShopDefaults where Parameter='Machine AE' and ValueInText=@valueInText)
                                    begin
                                        delete from ShopDefaults where Parameter='Machine AE' and ValueInText=@valueInText
                                    end";
                    break;
            }
            try
            {

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }
        internal static void insertUpdateShopDefaultSettingsForValueInText2(string Parameter, string ValueInText, string ValueInText2)
        {
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"if  exists(select * from ShopDefaults where Parameter=@parameter and ValueInText=@valueInText)
begin

    update ShopDefaults set ValueInText2 = @valueInText2,UpdatedTS=@UpdatedTS where ValueInText = @valueInText and Parameter = @parameter
end
else
                    begin
                        insert into ShopDefaults(Parameter, ValueInText, ValueInText2,UpdatedTS) values(@parameter, @valueInText, @valueInText2,@UpdatedTS)
end", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@valueInText", ValueInText);
                sqlCommand.Parameters.AddWithValue("@valueInText2", ValueInText2);
                sqlCommand.Parameters.AddWithValue("@parameter", Parameter);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
        }
        internal static ShopDefaultsValues GetShopDefaultsValues()
        {
            ShopDefaultsValues defaultsValues = new ShopDefaultsValues();
            SqlConnection sqlConnection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("select * from shopdefaults where parameter = 'TimeInFormat' or parameter = 'MinLUForLR' or  parameter = 'SmartTrans_Application_shutdowntime' or parameter = 'TargetFrom' or parameter = 'JobCardUpdate' or   parameter = 'Smartdata Settings'or parameter = 'Job Card Settings'or  parameter = 'CycleIgnoreThreshold' or  parameter = 'MachineInterLockTime' or parameter = 'EMWeightsDisplay' or Parameter = 'FinancialYearFrom' or Parameter = 'SmartAgent_Shutdown' or Parameter = 'Machine AE'  or Parameter = 'QEVisibility' or Parameter = 'ComponentInfoInterfaceIdDataType' or Parameter='LoadUnloadInSeconds' or Parameter='LoadUnloadThreshold'", sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        if ((reader["Parameter"]).ToString().Equals("TimeInFormat"))
                        {
                            defaultsValues.TimeFormat = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("TargetFrom"))
                        {
                            defaultsValues.TargetForm = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("MinLUForLR"))
                        {
                            defaultsValues.MinLuForLr = (int)reader["ValueInInt"];
                        }
                        else if ((reader["Parameter"]).ToString().Equals("JobCardUpdate"))
                        {
                            defaultsValues.Jobcardupdate = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("Job Card Settings", StringComparison.OrdinalIgnoreCase))
                        {
                            defaultsValues.JobCardSetting = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("CycleIgnoreThreshold"))
                        {
                            defaultsValues.CycleIgnoreThreshold = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("SmartTrans_Application_shutdowntime", StringComparison.OrdinalIgnoreCase))
                        {
                            defaultsValues.SmartTransAutoCloseTime = (int)reader["ValueInInt"];
                        }
                        else if ((reader["Parameter"]).ToString().Equals("MachineInterLockTime"))
                        {
                            defaultsValues.InterlockTime = (int)reader["ValueInInt"];
                        }
                        else if ((reader["Parameter"]).ToString().Equals("EMWeightsDisplay"))
                        {
                            defaultsValues.EMweightsDisplay = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("Smartdata Settings", StringComparison.OrdinalIgnoreCase))
                        {
                            defaultsValues.SmartData = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("FinancialYearFrom"))
                        {
                            defaultsValues.FiancialyearFrom = (int)reader["ValueInInt"];
                        }
                        else if ((reader["Parameter"]).ToString().Equals("SmartAgent_Shutdown"))
                        {
                            defaultsValues.SmartAgentShutDown = reader["ValueInText"].ToString();
                        }
                        else if ((reader["Parameter"]).ToString().Equals("QEVisibility"))
                        {
                            defaultsValues.QEEnabled = reader["ValueInInt"].ToString() == "1" ? true : false;
                        }
                        else if (reader["Parameter"].ToString().Equals("ComponentInfoInterfaceIdDataType"))
                        {
                            defaultsValues.CompInterfaceIDDataType = reader["ValueInText"].ToString();
                        }
                        else if (reader["Parameter"].ToString().Equals("LoadUnloadInSeconds"))
                        {
                            defaultsValues.LoadUnloadInSeconds = Convert.ToInt32(reader["ValueInText"].ToString());
                        }
                        else if (reader["Parameter"].ToString().Equals("LoadUnloadThreshold"))
                        {
                            defaultsValues.LoadUnloadThreshold = Convert.ToInt32(reader["ValueInText"].ToString());
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
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return defaultsValues;
        }
        internal static string GetEmployeeName(string operatorID)
        {
            string EmpName = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string sqlQuery = "select Name from employeeinformation where Employeeid=@EmpID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@EmpID", operatorID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmpName = rdr["Name"].ToString();
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
            return EmpName;
        }

        internal static DataTable getOprInsentiveReportSetting(string parameter)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = ConnectionManager.GetConnection();
            try
            {
                SqlCommand command = new SqlCommand("Select * from ShopDefaults where Parameter=@Parameter", connection);
                command.Parameters.AddWithValue("@Parameter", parameter);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return dt;
        }
        internal static CockpitDefaultsValues GetCockpitDefaultsValues()
        {
            CockpitDefaultsValues cockpitDefaultsValues = new CockpitDefaultsValues();
            SqlConnection sqlConnection = ConnectionManager.GetConnection();

            try
            {
                string query = @"select * from Cockpitdefaults where parameter = 'TimeFormat' or parameter = 'DefaultShift' or parameter = 'TpmEnbMac' or parameter = 'VDG-CycleDefinition' or parameter = 'VDG-ComponentSetting'or parameter = 'DisplayTTFormat'or parameter = 'DisplayinIconicView'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["Parameter"].ToString().Equals("TimeFormat"))
                            cockpitDefaultsValues.TimeIn = reader["ValueInText"].ToString();
                        if (reader["Parameter"].ToString().Equals("DefaultShift"))
                            cockpitDefaultsValues.DefaultDisplay = reader["ValueInText"].ToString();
                        if (reader["Parameter"].ToString().Equals("TpmEnbMac"))
                            cockpitDefaultsValues.TpmEnbMac = reader["ValueInText"].ToString();
                        if (reader["Parameter"].ToString().Equals("VDG-CycleDefinition"))
                            cockpitDefaultsValues.VDG_PCD = reader["ValueInInt"].ToString();
                        if (reader["Parameter"].ToString().Equals("VDG-ComponentSetting"))
                            cockpitDefaultsValues.VDG_CD = reader["ValueInInt"].ToString();
                        if (reader["Parameter"].ToString().Equals("DisplayTTFormat"))
                            cockpitDefaultsValues.DisplayTime = reader["ValueInText"].ToString();
                        if (reader["Parameter"].ToString().Equals("DisplayinIconicView"))
                            cockpitDefaultsValues.IVDO = reader["ValueInText"].ToString();

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConnection != null) sqlConnection.Close();
            }
            return cockpitDefaultsValues;
        }
        internal static bool GetProductionDownLog(int datatype)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            bool isProductionLogEnabled = false;
            try
            {
                if (datatype == 1)
                {
                    cmd = new SqlCommand(@"select ValueInText from CockpitDefaults where Parameter='EnableProductionLog'", con);
                }
                else if (datatype == 2)
                {
                    cmd = new SqlCommand(@"select ValueInText from CockpitDefaults where Parameter='EnableDownLog'", con);
                }
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            isProductionLogEnabled = true;
                        }
                        else
                        {
                            isProductionLogEnabled = false;
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
            return isProductionLogEnabled;
        }
        internal static void UpdateModifiedData_Bulk(DateTime StartTime, DateTime EndTime, string MacInt, string FromComponent, string ToComponent, string FromOperation, string ToOperation,
       string FromOperator, string ToOperator, string FromWorkOrder, string ToWorkOrder, Decimal FromPartsCount, Decimal ToPartsCount, string downid, string FromdownCode,
       string TodownCode, string RejectionID, string FromRejectionCode, string ToRejectionCode, int Datatype, string param, string rejectQty, out bool IsUpdated, string UpdatedBy)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            IsUpdated = false;
            try
            {
                cmd = new SqlCommand(@"S_UpdateAutodataAuditLogDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@mc", MacInt);
                cmd.Parameters.AddWithValue("@datatype", Datatype);
                cmd.Parameters.AddWithValue("@StartTime", StartTime);
                cmd.Parameters.AddWithValue("@Endtime", EndTime);
                cmd.Parameters.AddWithValue("@FromComp", FromComponent);
                cmd.Parameters.AddWithValue("@ToComp", ToComponent);
                cmd.Parameters.AddWithValue("@FromOperation", FromOperation);
                cmd.Parameters.AddWithValue("@ToOperation", ToOperation);
                cmd.Parameters.AddWithValue("@Fromdcode", FromdownCode);
                cmd.Parameters.AddWithValue("@Todcode", TodownCode);
                cmd.Parameters.AddWithValue("@FromPartsCount", FromPartsCount);
                cmd.Parameters.AddWithValue("@ToPartsCount", ToPartsCount);
                cmd.Parameters.AddWithValue("@FromOperator", FromOperator);
                cmd.Parameters.AddWithValue("@ToOperator", ToOperator);
                cmd.Parameters.AddWithValue("@BulkUpdate", 1);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    IsUpdated = true;
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
        }
        internal static bool UpdateModifiedComponentData(string oldComponentValue, string oldOperationValue, string newComponentValue, string newOperationValue, string machineID, DateTime sttime, DateTime ndtime, int Datatype, out int updateRowCount)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updateRowCount = 0;
            try
            {
                if (Datatype == 3)
                {
                    cmd = new SqlCommand(@"update AutodataRejections set comp=@NewComponent,opn=@NewOperation where comp=@OldComponent and opn=@OldOperation and mc=@MachineID and 
createdts between @sttime and @ndtime", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"Update autodata set IsModified=1, comp = @NewComponent ,opn=@NewOperation where comp = @OldComponent and mc=@MachineID and  datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime and opn=@OldOperation", conn);
                }
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OldComponent", oldComponentValue);
                cmd.Parameters.AddWithValue("@OldOperation", oldOperationValue);
                cmd.Parameters.AddWithValue("@NewComponent", newComponentValue);
                cmd.Parameters.AddWithValue("@NewOperation", newOperationValue);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", Datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updateRowCount = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updateRowCount = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }

        internal static bool UpdateModifiedWorkOrderData(string oldWorkOrder, string newWorkOrder, string machineID, DateTime sttime, DateTime ndtime, int datatype, out int updatedRowCount)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRowCount = 0;
            try
            {
                cmd = new SqlCommand(@"Update autodata set IsModified=1, WorkOrderNumber = @NewWorkOrderNumber where WorkOrderNumber = @OldWorkOrderNumber and mc=@MachineID and  datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OldWorkOrderNumber", oldWorkOrder);
                cmd.Parameters.AddWithValue("@NewWorkOrderNumber", newWorkOrder);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updatedRowCount = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRowCount = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }

        internal static bool UpdateModifiedOperatorData(string oldOperationValue, string newOperationValue, string machineID, DateTime sttime, DateTime ndtime, int Datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                oldOperationValue = oldOperationValue.Contains("<") ? oldOperationValue.Split(new char[] { '<', '>' })[1].Trim() : oldOperationValue;
                newOperationValue = newOperationValue.Contains("<") ? newOperationValue.Split(new char[] { '<', '>' })[0].Trim() : newOperationValue;

                if (Datatype == 3)
                {
                    cmd = new SqlCommand(@"update AutodataRejections set opr=@NewOperation where opr=@OldOperation and mc=@MachineID and createdts between @sttime and @ndtime", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"Update autodata set IsModified=1, opr=@NewOperation where opr=@OldOperation  and mc=@MachineID and  datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime", conn);
                }
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OldOperation", oldOperationValue);
                cmd.Parameters.AddWithValue("@NewOperation", newOperationValue);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", Datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updatedRow = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRow = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }

        internal static bool UpdateModifiedPartCountData(string component, string operation, string machineID, DateTime sttime, DateTime ndtime, double oldPartCount, double newPartCount, int Datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                cmd = new SqlCommand(@"update autodata set IsModified=1, partscount =@NewPartCount where mc=@MachineID and comp = @Component and opn=@Operation and  datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime and partscount=@OldPartCount", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Component", component);
                cmd.Parameters.AddWithValue("@Operation", operation);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", Datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@NewPartCount", newPartCount);
                cmd.Parameters.AddWithValue("@OldPartCount", oldPartCount);
                updatedRow = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRow = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }

        internal static bool UpdateModifiedDownData(string oldDownValue, string newDownValue, string machineID, DateTime sttime, DateTime ndtime, int Datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                cmd = new SqlCommand(@"update autodata set IsModified=1,dcode=@NewDownCode where dcode=@OldDownCode  and  mc=@MachineID and datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OldDownCode", oldDownValue);
                cmd.Parameters.AddWithValue("@NewDownCode", newDownValue);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", Datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updatedRow = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRow = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }

        internal static bool UpdateJobCard(int iD, string downTime, DateTime sttime, DateTime splitDate, string Updatedby)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Success = false;
            string Query = @"update ShiftDownTimeDetails set Starttime = @StartDate,Endtime=@EndTime,DownTime=@DownTime,UpdatedBy=@UpdatedBy,UpdatedTS=@UpdatedTS where id = @ID ";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@ID", iD);
                cmd.Parameters.AddWithValue("@StartDate", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", splitDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@DownTime", downTime);
                cmd.Parameters.AddWithValue("@UpdatedBy", Updatedby);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                Success = true;
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
            return Success;
        }

        internal static bool InsertJobCard(DateTime splitDate, DateTime ndtime, string downTime, string updatedBy, DateTime UpdatedTS, DataRow dt)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            bool Success = false;
            SqlCommand cmd = null;

            //string Query = @"insert into ShiftDownTimeDetails([dDate],[Shift],[PlantID],[MachineID],[ComponentID],[OperationNo],[OperatorID],[StartTime],[EndTime],[DownCategory],[DownID],[DownTime],[ML_Flag],[TurnOver],[Threshold],[RetPerMcHour_Flag],[StdSetupTime],[PE_Flag],[UpdatedBy],[UpdatedTS],[WorkOrderNumber],[CriticalMachineEnabled],[GroupID],[PDT]) 
            //    values(@dDate,@Shift,@PlantID,@MachineID,@ComponentID,@OperationNo,@OperatorID,@StartTime,@EndTime,@DownCategory,@DownID,@DownTime,@ML_Flag,@TurnOver,@Threshold,@RetPerMcHour_Flag,@StdSetupTime,@PE_Flag,@UpdatedBy,@UpdatedTS,@WorkOrderNumber,@CriticalMachineEnabled,@GroupID,@PDT)";
            string Query = @"insert into ShiftDownTimeDetails([dDate],[Shift],[PlantID],[MachineID],[ComponentID],[OperationNo],[OperatorID],[StartTime],[EndTime],[DownCategory],[DownID],[DownTime],[ML_Flag],[TurnOver],[Threshold],[RetPerMcHour_Flag],[StdSetupTime],[PE_Flag],[UpdatedBy],[UpdatedTS],[WorkOrderNumber],[CriticalMachineEnabled],[GroupID]) 
                values(@dDate,@Shift,@PlantID,@MachineID,@ComponentID,@OperationNo,@OperatorID,@StartTime,@EndTime,@DownCategory,@DownID,@DownTime,@ML_Flag,@TurnOver,@Threshold,@RetPerMcHour_Flag,@StdSetupTime,@PE_Flag,@UpdatedBy,@UpdatedTS,@WorkOrderNumber,@CriticalMachineEnabled,@GroupID)";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@dDate", Convert.ToDateTime(dt["dDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", dt["Shift"].ToString());
                cmd.Parameters.AddWithValue("@PlantID", dt["PlantID"].ToString());
                cmd.Parameters.AddWithValue("@MachineID", dt["MachineID"].ToString());
                cmd.Parameters.AddWithValue("@ComponentID", dt["ComponentID"].ToString());
                cmd.Parameters.AddWithValue("@OperationNo", dt["OperationNo"].ToString());
                cmd.Parameters.AddWithValue("@OperatorID", dt["OperatorID"].ToString());
                cmd.Parameters.AddWithValue("@StartTime", splitDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@DownCategory", dt["DownCategory"].ToString());
                cmd.Parameters.AddWithValue("@DownID", dt["DownID"].ToString());
                cmd.Parameters.AddWithValue("@DownTime", downTime);
                cmd.Parameters.AddWithValue("@ML_Flag", dt["ML_Flag"].ToString());
                cmd.Parameters.AddWithValue("@TurnOver", dt["TurnOver"].ToString());
                cmd.Parameters.AddWithValue("@Threshold", dt["Threshold"].ToString());
                cmd.Parameters.AddWithValue("@RetPerMcHour_Flag", dt["RetPerMcHour_Flag"].ToString());
                cmd.Parameters.AddWithValue("@StdSetupTime", dt["StdSetupTime"].ToString());
                cmd.Parameters.AddWithValue("@PE_Flag", dt["PE_Flag"].ToString());
                cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@WorkOrderNumber", dt["WorkOrderNumber"].ToString());
                cmd.Parameters.AddWithValue("@CriticalMachineEnabled", dt["CriticalMachineEnabled"].ToString());
                cmd.Parameters.AddWithValue("@GroupID", dt["GroupID"].ToString());
                // cmd.Parameters.AddWithValue("@PDT", dt["PDT"].ToString());
                cmd.ExecuteNonQuery();
                Success = true;
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
            return Success;
        }


        internal static DataTable GetModifiedandJobDate(int iD, string Type)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = "";
            if (Type.Equals("ModifiedData", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"Select * from autodata where ID=@ID";
            }
            else
            {
                Query = @"Select * from ShiftDownTimeDetails where ID=@ID";
            }
            try
            {
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@ID", iD);
                rdr = cmd.ExecuteReader();
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


        internal static bool UpdateModifiedSplitData(int iD, DateTime ndtime)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool success = false;
            //string Query = @"update autodata set nddate = @nddate ,ndtime = @ndtime where id=@id";
            string Query = @"update autodata set nddate = @nddate ,ndtime = @ndtime where id=@id
update autodata set loadunload = (select DATEDIFF(second,sttime,ndtime) from autodata where id=@id)  where id=@id";
            try
            {
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@id", iD);
                cmd.Parameters.AddWithValue("@nddate", ndtime.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return success;
        }

        internal static bool InsertModifiedSplitDate(DateTime splitdate, DateTime ndtime, DataRow dataRow)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool success = false;
            string Query = @"insert into autodata(mc,comp,opn,opr,dcode,stdate,sttime,nddate,ndtime,datatype,cycletime,loadunload,compslno,remarks,post,msttime,partscount,WorkOrderNumber)
            values(@mc,@comp,@opn,@opr,@dcode,@stdate,@sttime,@nddate,@ndtime,@datatype,@cycletime,@loadunload,@compslno,@remark,@post,@mstime,@partcount,@workorder)";
            try
            {
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@mc", dataRow["mc"].ToString());
                cmd.Parameters.AddWithValue("@comp", dataRow["comp"].ToString());
                cmd.Parameters.AddWithValue("@opn", dataRow["opn"].ToString());
                cmd.Parameters.AddWithValue("@opr", dataRow["opr"].ToString());
                cmd.Parameters.AddWithValue("@dcode", dataRow["dcode"].ToString());
                cmd.Parameters.AddWithValue("@stdate", splitdate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@sttime", splitdate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@nddate", ndtime.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@datatype", dataRow["datatype"].ToString());
                cmd.Parameters.AddWithValue("@cycletime", dataRow["cycletime"].ToString());
                // cmd.Parameters.AddWithValue("@loadunload", dataRow["loadunload"].ToString());
                TimeSpan ts = ndtime - splitdate;
                cmd.Parameters.AddWithValue("@loadunload", ts.TotalSeconds);
                cmd.Parameters.AddWithValue("@compslno", dataRow["compslno"].ToString());
                cmd.Parameters.AddWithValue("@remark", dataRow["remarks"].ToString());
                cmd.Parameters.AddWithValue("@post", dataRow["post"].ToString());
                cmd.Parameters.AddWithValue("@mstime", dataRow["msttime"].ToString());
                cmd.Parameters.AddWithValue("@partcount", dataRow["partscount"].ToString());
                cmd.Parameters.AddWithValue("@workorder", dataRow["WorkOrderNumber"].ToString());
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return success;
        }

        internal static bool SaveMachineSortOrder(string Machineid, string SortOrder)
        {
            bool Proceed = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int saved = 0;
            try
            {
                cmd = new SqlCommand(@"if exists(select * from MachinewiseSortOrder where Machineid=@Machineid)
										Begin
										  update MachinewiseSortOrder set SortOrder=@SortOrder,UpdatedTS=@UpdatedTS where Machineid = @Machineid
										End
										Else
										Begin
											insert into MachinewiseSortOrder (Machineid,SortOrder,UpdatedTS)
											values(@Machineid,@SortOrder,@UpdatedTS ) 
										End", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                if (string.IsNullOrEmpty(SortOrder))
                    cmd.Parameters.AddWithValue("@SortOrder", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                saved = cmd.ExecuteNonQuery();
                if (saved >= 1)
                {
                    Proceed = true;
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
            }
            return Proceed;
        }

        internal static DataTable GetMachineSortOrderData()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"SELECT  M.[machineid] ,S.SortOrder FROM[machineinformation] M
										LEFT JOIN[MachinewiseSortOrder] S on M.[machineid] = S.[machineid]
										where M.TPMTrakEnabled = '1'", conn);
                cmd.CommandType = CommandType.Text;
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
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }

        internal static DataTable GetMachineStatusColorCodes()
        {
            DataTable dtMachineStatusColorCodes = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"SELECT * from Focas_MachineColorcode", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtMachineStatusColorCodes.Load(rdr);
                    dtMachineStatusColorCodes.AcceptChanges();
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
            return dtMachineStatusColorCodes;
        }

        internal static List<string> GetEshopxMachines(string deviceId, string param, out string defaultMachine)
        {
            List<string> list = new List<string>();
            defaultMachine = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[s_GetLoginInformation]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@deviceName", deviceId);
                cmd.Parameters.AddWithValue("@UserName", "");
                cmd.Parameters.AddWithValue("@password", "");
                cmd.Parameters.AddWithValue("@param", param);

                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["Machine"]))
                        {
                            defaultMachine = (Convert.ToString(sdr["Machine"]));
                        }
                    }
                }

                sdr.NextResult();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["MachineId"]))
                        {
                            var mcByte = Convert.FromBase64String(sdr["MachineId"].ToString());
                            string mc = Encoding.UTF8.GetString(mcByte);
                            list.Add(mc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving Machines - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<string> GetRejCatagory(string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string sqlQuery = string.Empty;
            List<string> result = new List<string>();
            try
            {
                if (string.Equals(param, "catagory", StringComparison.OrdinalIgnoreCase))
                {
                    sqlQuery = "select distinct(catagory) as result from rejectioncodeinformation where catagory is not null order by catagory";
                }
                else if (string.Equals(param, "code", StringComparison.OrdinalIgnoreCase))
                {
                    sqlQuery = "select distinct(Rejectionid) as result  from rejectioncodeinformation where Rejectionid is not null order by Rejectionid";
                }
                else if ((string.Equals(param, "code", StringComparison.OrdinalIgnoreCase)))
                {
                    sqlQuery = "select distinct(Rejectionid) as result  from rejectioncodeinformation where  catagory =@catagory order by Rejectionid";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    result.Add(Convert.ToString(rdr["result"]));
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
            if (string.Equals(param, "catagory", StringComparison.OrdinalIgnoreCase))
            {
                result.Insert(0, "select Catagory");
            }
            else if (string.Equals(param, "code", StringComparison.OrdinalIgnoreCase))
            {
                result.Insert(0, "select Code");
            }
            //result.Insert(0, "");
            return result;
        }

        internal static List<string> GetAllShifts(string OeeOrOthers)
        {
            List<string> allShifts = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ShiftName from shiftdetails where running = 1 order by shiftid", conn);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (OeeOrOthers.Equals("OEE"))
                {
                    allShifts.Add("Today - All");
                }
                else
                {
                    allShifts.Add("All");
                }
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        if (!Convert.IsDBNull(rdr["ShiftName"]))
                        {
                            allShifts.Add(Convert.ToString(rdr["ShiftName"]));
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
                // MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return allShifts;
        }
        internal static List<ListItem> GetAllShiftIds()
        {
            List<ListItem> allShifts = new List<ListItem>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select shiftid,ShiftName from shiftdetails where running = 1 order by shiftid", conn);
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        allShifts.Add(new ListItem() { Text = rdr["ShiftName"].ToString(), Value = rdr["shiftid"].ToString() });
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
            return allShifts;
        }
        internal static DataTable getrejectiondata(string logicalDayStart, string shiftproductin, string componentProduction, string operationProduction, string OperatorName, string selectedMachine, string rejectionCode, string rejQty, string Type)
        {
            SqlDataReader sdr = null;

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;


            DataTable dt = new DataTable();
            try
            {

                cmd = new SqlCommand("s_Eshopx_ViewAndInsertRejections", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RejDate", logicalDayStart);
                cmd.Parameters.AddWithValue("@Rejshift", shiftproductin);
                cmd.Parameters.AddWithValue("@ComponentID", componentProduction);
                cmd.Parameters.AddWithValue("@Operationno", operationProduction);
                cmd.Parameters.AddWithValue("@MachineID", selectedMachine);
                cmd.Parameters.AddWithValue("@Operatorid", OperatorName);
                cmd.Parameters.AddWithValue("@RejectionCode", rejectionCode);
                cmd.Parameters.AddWithValue("@RejQty", rejQty);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@Type", Type);
                //cmd.Parameters.AddWithValue("@LotNumber ", LotNumber);
                sdr = cmd.ExecuteReader();

                dt.Load(sdr);
                dt.AcceptChanges();
                //
                dt.Columns.Remove("Machineid");
                dt.Columns.Remove("Operationno");
                dt.Columns.Remove("ComponentID");
                dt.Columns.Remove("RejDate");
                dt.Columns.Remove("Shiftname");
                dt.Columns["id"].AllowDBNull = true;
                dt.Columns["id"].ReadOnly = true;

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

            return dt;
        }

        internal static string GetLogicalDayEnd(string selectedTime)
        {
            string list = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select [dbo].[f_GetLogicalDayEnd](' " + selectedTime + "') as logicalDay";
                cmd = new SqlCommand(sqlQuery, conn);
                //cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["logicalDay"]))
                        {
                            list = DateTime.Parse((sdr["logicalDay"]).ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                else
                {
                    list = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving Machines - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static string GetLogicalDay(string selectedTime)
        {
            string list = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select [dbo].[f_GetLogicalDayStart](' " + selectedTime + "') as logicalDay";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["logicalDay"]))
                        {
                            list = DateTime.Parse((sdr["logicalDay"]).ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                else
                {
                    list = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving Machines - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<string> GetCurrentShiftTime(string selectedShift, string selectedTimePeriod)
        {
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[s_GetShiftTime]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDateTime", selectedTimePeriod);
                cmd.Parameters.AddWithValue("@Shift", selectedShift);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["StartTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (!Convert.IsDBNull(sdr["EndTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
                else
                {
                    list = null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving Machines - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }


        internal static DataTable GetDownsForTheMachine(string selectedMachine, string logicalDayStart, string logicalDayEnd)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                Logger.WriteDebugLog("calling s_GetCockpitDownData_IMTEX2013 .. \n");
                cmd = new SqlCommand("s_GetCockpitDownData_IMTEX2013", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", logicalDayStart);//"22-jan-2015 03:00:00"); //'22-jan-2015 03:00:00','23-jan-2015 03:00:00','ct-23'
                cmd.Parameters.AddWithValue("@EndTime", logicalDayEnd);//"23-jan-2015 03:00:00");
                cmd.Parameters.AddWithValue("@MachineID", selectedMachine);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving Machine - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
                Logger.WriteDebugLog("Finished calling s_GetCockpitDownData_IMTEX2013 .. \n");
            }
            return dt;
        }

        internal static bool WorkOrderVisibility()
        {
            bool visible = true;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"select WorkOrder from SmartdataPortRefreshDefaults;";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    if (!string.Equals("Y", sdr["WorkOrder"].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error in WorkOrder:" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
                Logger.WriteDebugLog("Finished calling SmartdataPortRefreshDefaults .. \n");
            }
            return visible;
        }

        internal static void SaveRejectionData(string logicalDayStart, string shiftproductin, string Machineid, string ComponentProduction, string OperationProduction, string Operatorid, string RCode, int RejctionCount, int Id, string WorkOrderNo, string param, out string IsSuccessfull, string LotNumber)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            IsSuccessfull = string.Empty;

            try
            {
                cmd = new SqlCommand("s_Eshopx_ViewAndInsertRejections", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RejDate", logicalDayStart);
                cmd.Parameters.AddWithValue("@Rejshift", shiftproductin);
                cmd.Parameters.AddWithValue("@MachineID", Machineid);
                cmd.Parameters.AddWithValue("@ComponentID", ComponentProduction);
                cmd.Parameters.AddWithValue("@Operationno", OperationProduction);
                cmd.Parameters.AddWithValue("@Operatorid", Operatorid);
                cmd.Parameters.AddWithValue("@RejectionCode", RCode);
                cmd.Parameters.AddWithValue("@RejQty", RejctionCount);
                // cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@WorkOrderNumber", WorkOrderNo);
                cmd.Parameters.AddWithValue("@Param", param);
                if (WebConfigurationManager.AppSettings["SKSPages"].ToString() == "1")
                {
                    cmd.Parameters.AddWithValue("@LotNumber", LotNumber);
                }
                cmd.Parameters.AddWithValue("@Type", "Rejection");
                var i = cmd.ExecuteNonQuery();
                IsSuccessfull = "Success";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static bool DeleteRejectionEntry(int id, string RejCode)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int count = 0;
            bool res = false;
            try
            {
                cmd = new SqlCommand("s_Eshopx_ViewAndInsertRejections", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Recordid", id);
                cmd.Parameters.AddWithValue("@RejectionCode", RejCode);
                cmd.Parameters.AddWithValue("@Type", "Rejection");
                cmd.Parameters.AddWithValue("@Param", "delete");

                cmd.ExecuteNonQuery();
                res = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static List<string> GetSelectedRejCatagory(string selectedCatagory)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string sqlQuery = "select distinct(Rejectionid) as result  from rejectioncodeinformation where  catagory =@catagory order by Rejectionid";
            List<string> result = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@catagory", selectedCatagory);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    result.Add(Convert.ToString(rdr["result"]));
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

            result.Insert(0, "select Code");
            return result;
        }

        internal static DataTable GetProductionsForTheMachine(string selectedMachine, string selectedShift, string logicalDayStart, string logicalDayEnd)
        {

            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                Logger.WriteDebugLog("calling [s_GetShiftwiseProductionReportFromAutodata_IMTEX2013] .. \n");
                cmd = new SqlCommand("s_GetShiftwiseProductionReportFromAutodata_IMTEX2013", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StartDate", logicalDayStart);
                if (selectedShift == "Today - All")
                {
                    cmd.Parameters.AddWithValue("@ShiftIn", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ShiftIn", selectedShift);
                }
                cmd.Parameters.AddWithValue("@MachineID", selectedMachine);
                cmd.Parameters.AddWithValue("@ComponentID", "");
                cmd.Parameters.AddWithValue("@OperationNo", "");
                cmd.Parameters.AddWithValue("@PlantID", "");
                cmd.Parameters.AddWithValue("@EndDate", logicalDayEnd);
                cmd.Parameters.AddWithValue("@Param", "Fortablet");
                cmd.CommandTimeout = 300;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving Production - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
                Logger.WriteDebugLog("finished calling s_GetShiftwiseProductionReportFromAutodata_IMTEX2013 Data Retrived .. \n");
            }
            return dt;
        }

        internal static string GetDownTime(string selectedMachine, string logicalDayStart, string logicalDayEnd)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = string.Empty;
            string downtime = string.Empty;
            try
            {
                query = "[s_GetCockpitData_WithTempTable]";
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", DateTime.ParseExact(logicalDayStart, "yyyy-MM-dd HH:mm:ss", null));
                cmd.Parameters.AddWithValue("@EndTime", DateTime.ParseExact(logicalDayEnd, "yyyy-MM-dd HH:mm:ss", null));
                cmd.Parameters.AddWithValue("@MachineID", selectedMachine);
                cmd.Parameters.AddWithValue("@PlantID", "");
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    downtime = rdr["DownTime"].ToString();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }

            return downtime;
        }

        #region SupplierCode
        internal static List<string> GetSupplierCode()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = @"  select distinct SupplierCode from SupplierCode_TAFE";
            List<string> SupplierCode = new List<string>();
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    SupplierCode.Add(rdr["SupplierCode"].ToString());
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }

            return SupplierCode;
        }

        internal static List<string> GetComponentID()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = @"  select distinct componentid from componentinformation";
            List<string> Component = new List<string>();
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    Component.Add(rdr["componentid"].ToString());
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }

            return Component;
        }

        internal static List<SupplierCodeEntity> GetGridSupCodeDAta()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = @"Select * from  SupplierCode_TAFE";
            List<SupplierCodeEntity> SupplierCodeList = new List<SupplierCodeEntity>();
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                int i = 1;
                while (rdr.Read())
                {
                    SupplierCodeEntity SupplierCode = new SupplierCodeEntity();
                    SupplierCode.SLNo = i++;
                    SupplierCode.SupCode = rdr["SupplierCode"].ToString();
                    SupplierCode.idd = Convert.ToInt32(rdr["SLNO"].ToString());
                    SupplierCodeList.Add(SupplierCode);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }

            return SupplierCodeList;
        }

        internal static List<SupplierCodeComponentIDEntity> GetGridSupComponentID(string supplierCode, string componentID, List<string> ComponentID, List<string> Suppcode)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = "";
            supplierCode = supplierCode == "All" ? "" : supplierCode;
            componentID = componentID == "All" ? "" : componentID;
            if (string.IsNullOrEmpty(supplierCode) && string.IsNullOrEmpty(componentID))
            {
                query = @"Select * from  SupplierComponent_Tafe";
            }
            else if (string.IsNullOrEmpty(supplierCode))
            {
                query = @"Select * from  SupplierComponent_Tafe where [ComponentID]=@ComponentID";
            }
            else if (string.IsNullOrEmpty(componentID))
            {
                query = @"Select * from  SupplierComponent_Tafe where [SupplierCode]=@SupplierCode";
            }
            else
            {
                query = @"Select * from  SupplierComponent_Tafe where [SupplierCode]=@SupplierCode and [ComponentID] = @ComponentID";
            }
            List<SupplierCodeComponentIDEntity> SupplierComponentList = new List<SupplierCodeComponentIDEntity>();
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierCode", supplierCode);
                cmd.Parameters.AddWithValue("@ComponentID", componentID);
                rdr = cmd.ExecuteReader();
                int i = 1;
                while (rdr.Read())
                {
                    SupplierCodeComponentIDEntity SupplierCodeComponentID = new SupplierCodeComponentIDEntity();
                    SupplierCodeComponentID.SLNO = i++;
                    SupplierCodeComponentID.SupplierCode = rdr["SupplierCode"].ToString();
                    ComponentID.Remove("All");
                    SupplierCodeComponentID.lstComponentID = ComponentID;
                    Suppcode.Remove("All");
                    SupplierCodeComponentID.lstSupplierCode = Suppcode;
                    SupplierCodeComponentID.ComponentID = rdr["ComponentID"].ToString();
                    SupplierCodeComponentID.idd = Convert.ToInt32(rdr["idd"].ToString());
                    SupplierComponentList.Add(SupplierCodeComponentID);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }

            return SupplierComponentList;
        }

        internal static bool SaveSupplierCode(string txtsupcode, string idd)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if not exists (select * from SupplierCode_TAFE where SupplierCode=@SupplierCode)
                                 begin
                                            insert into SupplierCode_TAFE(SupplierCode)
                                            values(@SupplierCode)
                                 end
                             else
                                begin
                                            update SupplierCode_TAFE set SupplierCode=@SupplierCode where SlNo=@SLNO
                                end";
            bool SupplierCodeInsert = false;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierCode", txtsupcode);
                cmd.Parameters.AddWithValue("@SLNO", idd);
                cmd.ExecuteNonQuery();
                SupplierCodeInsert = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return SupplierCodeInsert;
        }

        internal static bool SaveSupplierCodeComponentID(string txtSupplierCode, string txtComponentID, string idd)
        {

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if not exists (select * from [SupplierComponent_Tafe] where idd=@idd)
                                 begin
                                            insert into [SupplierComponent_Tafe](SupplierCode,ComponentID)
                                            values(@SupplierCode,@ComponentID)
                                 end
                             else
                                begin
                                            update [SupplierComponent_Tafe] set SupplierCode=@SupplierCode,ComponentID=@ComponentID where idd=@idd
                                end";
            bool SupplierCodeInsert = false;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierCode", txtSupplierCode);
                cmd.Parameters.AddWithValue("@ComponentID", txtComponentID);
                cmd.Parameters.AddWithValue("@idd", idd);
                cmd.ExecuteNonQuery();
                SupplierCodeInsert = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return SupplierCodeInsert;
        }

        internal static bool DeleteSupCode(string hidfieldSuppCode, string idd)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"Delete from SupplierCode_TAFE where SlNo=@SLNO";
            bool SupplierCodeDelete = false;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SLNO", idd);
                cmd.ExecuteNonQuery();
                SupplierCodeDelete = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return SupplierCodeDelete;
        }

        internal static bool Checkdsupdataata(string txtsupcode)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"select * from SupplierCode_TAFE where SupplierCode=@suppliercode";
            bool SupplierCodepresent = false;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@suppliercode", txtsupcode);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    SupplierCodepresent = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return SupplierCodepresent;
        }

        internal static bool DeleteSupCodeComponentID(string hidfieldSuppCodeID, string hidfieldComponentID, string idd)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"Delete from [SupplierComponent_Tafe] where [idd]=@idd";
            bool SupplierCodeDelete = false;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idd", idd);
                cmd.ExecuteNonQuery();
                SupplierCodeDelete = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return SupplierCodeDelete;
        }

        internal static bool Checkdataforinsert(string txtSupplierCode, string txtComponentID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"select * from [SupplierComponent_Tafe] where SupplierCode=@suppcode and ComponentID=@compid";
            bool SupplierCodepresent = false;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@suppcode", txtSupplierCode);
                cmd.Parameters.AddWithValue("@compid", txtComponentID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    SupplierCodepresent = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return SupplierCodepresent;
        }
        #endregion

        internal static List<string> GetComponentsForPlant(string strDate, string endDate, string machineID)
        {
            List<string> componentList = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader dataReader = null;
            string query = @"SELECT DISTINCT C.componentid from Autodata A INNER JOIN machineinformation M  On M.InterfaceID = A.mc INNER JOIN PlantMachine P ON M.machineid = P.MachineID INNER JOIN componentinformation C ON C.InterfaceID = A.Comp INNER JOIN componentoperationpricing COP ON COP.InterfaceID = A.Opn AND  COP.machineid = M.machineid AND COP.componentid = C.componentid Where A.sttime >= '' + convert(nvarchar(30),@StartDate,120) +'' and A.ndtime <= '' + convert(nvarchar(30),@EndDate,120) + '' AND M.machineid  = '' + @MachineID + ''";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, conn);
                sqlCommand.Parameters.AddWithValue("@MachineID", machineID);
                sqlCommand.Parameters.AddWithValue("@StartDate", strDate);
                sqlCommand.Parameters.AddWithValue("@EndDate", endDate);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandTimeout = 120;
                dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        componentList.Add(dataReader["componentid"].ToString());
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
                if (dataReader != null) dataReader.Close();
            }
            return componentList;
        }

        #region "Flow Meter Graph Data"
        internal static FlowMeterChartModel GetFlowMeterData(string fromDate, string toDate, string machineId, string componentId)
        {
            DataTable dtFlowMeterData = new DataTable();
            FlowMeterChartModel flowMeterData = new FlowMeterChartModel();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader dataReader = null;
            try
            {
                SqlCommand command = new SqlCommand(@"S_GetBoschFlowMeterReport", conn);
                command.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(fromDate).ToSQLDateTimeFormat());
                command.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(toDate).ToSQLDateTimeFormat());
                command.Parameters.AddWithValue("@PlantId", "");
                command.Parameters.AddWithValue("@Mc", machineId);
                command.Parameters.AddWithValue("@Component", componentId);
                command.Parameters.AddWithValue("@param", "chart");
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(command);
                int rownum = sda.Fill(dtFlowMeterData);
                if (rownum > 0)
                {
                    flowMeterData = GetFlowMeterDataFiltered(dtFlowMeterData);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (dataReader != null) dataReader.Close();
            }
            return flowMeterData;
        }

        private static FlowMeterChartModel GetFlowMeterDataFiltered(DataTable dtFlowMeterData)
        {
            FlowMeterChartModel flowMeterData = new FlowMeterChartModel();
            List<FlowMeterChartData> flowMeterChart = new List<FlowMeterChartData>();
            List<double> flowValueData = new List<double>();
            List<FlowMeterChartSeriesData> listFlowMeterChartSeriesData = new List<FlowMeterChartSeriesData>();
            FlowMeterChartSeriesData flowMeterChartSeriesData = null;
            try
            {
                if (dtFlowMeterData != null && dtFlowMeterData.Rows.Count > 0)
                {
                    flowMeterData = new FlowMeterChartModel();
                    flowMeterData.MinValue = dtFlowMeterData.AsEnumerable().Select(x => x.Field<double>("MinFlowValue")).Max();
                    flowMeterData.MedianValue = dtFlowMeterData.AsEnumerable().Select(x => x.Field<double>("MedianValue")).Max();
                    flowMeterData.MaxValue = dtFlowMeterData.AsEnumerable().Select(x => x.Field<double>("MaxFlowValue")).Max();
                    if (flowMeterData.MinValue != null && flowMeterData.MinValue != 0)
                    {
                        flowMeterData.MinValue1 = flowMeterData.MinValue - 20;
                    }
                    if (flowMeterData.MaxValue != null && flowMeterData.MaxValue != 0)
                    {
                        flowMeterData.MaxValue1 = flowMeterData.MaxValue + 20;
                    }
                    FlowMeterChartData flowMeterChartDetails = new FlowMeterChartData();
                    flowMeterChartDetails.name = "Flow Value";
                    foreach (DataRow dataRow in dtFlowMeterData.Rows)
                    {
                        //flowValueData.Add(Convert.ToDouble(dataRow["Flowvalue1"]));
                        //flowValueData.Add(Convert.ToDouble(dataRow["Flowvalue2"]));
                        flowMeterChartSeriesData = new FlowMeterChartSeriesData();
                        flowMeterChartSeriesData.y = Convert.ToDouble(dataRow["Flowvalue1"]);
                        flowMeterChartSeriesData.StartDateTime = string.IsNullOrEmpty(dataRow["StartTime"].ToString()) ? "" : Util.GetDateTime(dataRow["StartTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        flowMeterChartSeriesData.EndDateTime = string.IsNullOrEmpty(dataRow["Endtime"].ToString()) ? "" : Util.GetDateTime(dataRow["Endtime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        listFlowMeterChartSeriesData.Add(flowMeterChartSeriesData);
                        flowMeterChartSeriesData = new FlowMeterChartSeriesData();

                        flowMeterChartSeriesData.y = Convert.ToDouble(dataRow["Flowvalue2"]);
                        flowMeterChartSeriesData.StartDateTime = string.IsNullOrEmpty(dataRow["StartTime"].ToString()) ? "" : Util.GetDateTime(dataRow["StartTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        flowMeterChartSeriesData.EndDateTime = string.IsNullOrEmpty(dataRow["Endtime"].ToString()) ? "" : Util.GetDateTime(dataRow["Endtime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        listFlowMeterChartSeriesData.Add(flowMeterChartSeriesData);
                    }
                    //flowMeterChartDetails.data = flowValueData.ToArray();
                    flowMeterChartDetails.data = listFlowMeterChartSeriesData;
                    flowMeterChart.Add(flowMeterChartDetails);
                    flowMeterData.flowMeterChartDatas = flowMeterChart;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return flowMeterData;
        }
        #endregion

        #region Flow Meter Master
        internal static List<FlowMeterBoschEntity> GetFlowMeterData(string PartNumber)
        {
            List<FlowMeterBoschEntity> data = new List<FlowMeterBoschEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null; int i = 0;
            string Query = @"";
            PartNumber = PartNumber == "All" ? "" : PartNumber;
            Query = PartNumber == "" ? "Select * from Bosch_FlowMeterSpecification" : "Select * from Bosch_FlowMeterSpecification where PartNumber like '" + PartNumber + "%'";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FlowMeterBoschEntity entity = new FlowMeterBoschEntity();
                        entity.idd = Convert.ToInt32(rdr["IDD"].ToString());
                        entity.Component = (rdr["PartNumber"].ToString());
                        entity.TypeDia = (rdr["TypeDia"].ToString());
                        entity.ComponentdrpEnable = false;
                        entity.ComponentlblEnable = true;
                        entity.SettingsAng = (rdr["Angle"].ToString());
                        entity.SettingsHt = (rdr["Height"].ToString());
                        entity.SettingsHTGuage = (rdr["HeightGauge"].ToString());
                        entity.SettingsPR = (rdr["Pr"].ToString());
                        entity.RotaMin = (rdr["HeadMinFlowValue"].ToString());
                        entity.RotaMax = (rdr["HeadMaxFlowValue"].ToString());
                        entity.RotaMedian = (rdr["HeadMedianValue"].ToString());
                        entity.ShaftTestPr = (rdr["ShaftMinFlowValue"].ToString());
                        entity.RotaFlow = (rdr["ShaftMaxFlowValue"].ToString());
                        entity.BaralInspection = (rdr["BarrelInscription"].ToString());
                        entity.SettingRemark = (rdr["HeadRotaFlowRemarks"].ToString());
                        entity.RotaRemark = (rdr["ShaftRemarks"].ToString());
                        entity.IsTGGType = DBNull.Value.ToString().Equals((rdr["IsTGGType"].ToString())) ? false : Convert.ToBoolean(rdr["IsTGGType"].ToString());
                        entity.slno = i++;
                        data.Add(entity);
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
            return data;
        }

        internal static bool UpdateFlowMeterData(string componentID, string txtTypeDia, string txtAngDeg, string txtHTMM, string txtHTGUAGE, string txtPRBAR, string txtROTAMIN, string txtRotaMax, string txtRotaMedian, string txtTestPr, string txtRotaFlow, string txtbarrelInspection, string idd, string txtshaftRemaks, string txtheadRemaks, bool isTGGType)
        {
            bool status = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = @"if not exists (select * from Bosch_FlowMeterSpecification where IDD=@idd)
                                    begin
                                        insert into Bosch_FlowMeterSpecification(PartNumber,TypeDia,Angle,Height,HeightGauge,Pr,HeadMinFlowValue,HeadMaxFlowValue,HeadMedianValue,ShaftMinFlowValue,ShaftMaxFlowValue,BarrelInscription,HeadRotaFlowRemarks,ShaftRemarks,IsTGGType)
                                        values(@PartNumber,@TypeDia,@Angle,@Height,@HeightGauge,@Pr,@HeadMinFlowValue,@HeadMaxFlowValue,@HeadMedianValue,@ShaftMinFlowValue,@ShaftMaxFlowValue,@BarrelInscription,@HeadRotaFlowRemarks,@ShaftRemarks,@isTGGType)
                                    end
                               else
                                    begin
                                        update Bosch_FlowMeterSpecification set PartNumber=@PartNumber,TypeDia=@TypeDia,Angle=@Angle,Height=@Height,HeightGauge=@HeightGauge,Pr=@Pr,HeadMinFlowValue=@HeadMinFlowValue,HeadMaxFlowValue=@HeadMaxFlowValue,HeadMedianValue=@HeadMedianValue,ShaftMinFlowValue=@ShaftMinFlowValue,ShaftMaxFlowValue=@ShaftMaxFlowValue,BarrelInscription=@BarrelInscription,ShaftRemarks=@ShaftRemarks,HeadRotaFlowRemarks=@HeadRotaFlowRemarks,IsTGGType=@isTGGType where IDD=@idd
                                    end";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@idd", idd);
                cmd.Parameters.AddWithValue("@PartNumber", componentID);
                cmd.Parameters.AddWithValue("@TypeDia", txtTypeDia);
                cmd.Parameters.AddWithValue("@Angle", txtAngDeg);
                cmd.Parameters.AddWithValue("@Height", txtHTMM);
                cmd.Parameters.AddWithValue("@HeightGauge", txtHTGUAGE);
                cmd.Parameters.AddWithValue("@Pr", txtPRBAR);
                cmd.Parameters.AddWithValue("@HeadMinFlowValue", txtROTAMIN);
                cmd.Parameters.AddWithValue("@HeadMaxFlowValue", txtRotaMax);
                cmd.Parameters.AddWithValue("@HeadMedianValue", txtRotaMedian);
                cmd.Parameters.AddWithValue("@ShaftMinFlowValue", txtTestPr);
                cmd.Parameters.AddWithValue("@ShaftMaxFlowValue", txtRotaFlow);
                cmd.Parameters.AddWithValue("@BarrelInscription", txtbarrelInspection);
                cmd.Parameters.AddWithValue("@HeadRotaFlowRemarks", txtheadRemaks);
                cmd.Parameters.AddWithValue("@ShaftRemarks", txtshaftRemaks);
                cmd.Parameters.AddWithValue("@isTGGType", isTGGType);

                cmd.ExecuteNonQuery();
                status = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return status;
        }

        internal static bool DeleteFlowMeter(string idd)
        {
            bool status = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"delete from Bosch_FlowMeterSpecification where idd= @idd", conn);
                cmd.Parameters.AddWithValue("@idd", idd);
                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return status;
        }

        internal static bool BoschFlowMeterImportedData(DataRow item)
        {
            bool status = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = @"if not exists (select * from Bosch_FlowMeterSpecification where PartNumber=@PartNumber)
                                    begin
                                        insert into Bosch_FlowMeterSpecification(PartNumber,TypeDia,Angle,Height,HeightGauge,Pr,HeadMinFlowValue,HeadMaxFlowValue,HeadMedianValue,ShaftMinFlowValue,ShaftMaxFlowValue,BarrelInscription,HeadRotaFlowRemarks,ShaftRemarks)
                                        values(@PartNumber,@TypeDia,@Angle,@Height,@HeightGauge,@Pr,@HeadMinFlowValue,@HeadMaxFlowValue,@HeadMedianValue,@ShaftMinFlowValue,@ShaftMaxFlowValue,@BarrelInscription,@HeadRotaFlowRemarks,@ShaftRemarks)
                                    end
                               else
                                    begin
                                        update Bosch_FlowMeterSpecification set TypeDia=@TypeDia,Angle=@Angle,Height=@Height,HeightGauge=@HeightGauge,Pr=@Pr,HeadMinFlowValue=@HeadMinFlowValue,HeadMaxFlowValue=@HeadMaxFlowValue,HeadMedianValue=@HeadMedianValue,ShaftMinFlowValue=@ShaftMinFlowValue,ShaftMaxFlowValue=@ShaftMaxFlowValue,BarrelInscription=@BarrelInscription,ShaftRemarks=@ShaftRemarks,HeadRotaFlowRemarks=@HeadRotaFlowRemarks where PartNumber=@PartNumber
                                    end";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@idd", 0);
                cmd.Parameters.AddWithValue("@PartNumber", item["PartName"]);
                cmd.Parameters.AddWithValue("@TypeDia", item["TypeDia"]);
                cmd.Parameters.AddWithValue("@Angle", item["Angle"]);
                cmd.Parameters.AddWithValue("@Height", item["Height"]);
                cmd.Parameters.AddWithValue("@HeightGauge", item["HeightGuage"]);
                cmd.Parameters.AddWithValue("@Pr", item["PrBar"]);
                cmd.Parameters.AddWithValue("@HeadMinFlowValue", item["RotaMin"]);
                cmd.Parameters.AddWithValue("@HeadMaxFlowValue", item["RotaMax"]);
                cmd.Parameters.AddWithValue("@HeadMedianValue", item["RotaMedian"]);
                cmd.Parameters.AddWithValue("@HeadRotaFlowRemarks", item["HeadRemarks"]);
                cmd.Parameters.AddWithValue("@ShaftRemarks", item["ShaftRemarks"]);
                cmd.Parameters.AddWithValue("@ShaftMinFlowValue", item["TestPr"]);
                cmd.Parameters.AddWithValue("@ShaftMaxFlowValue", item["RotaFlow"]);
                cmd.Parameters.AddWithValue("@BarrelInscription", item["BarrelInscription"]);
                cmd.ExecuteNonQuery();
                status = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return status;
        }
        #endregion

        public static DataTable PPMSecondLeveldata(string date, string shiftName, string plantId, string machineId, string comparisonType, string Component, string Employee, string View, string Opeartion, string CellID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetAggDrilldownPPM2ndLevelDetails]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@ShiftName", shiftName = shiftName.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shiftName);
                cmd.Parameters.AddWithValue("@PlantID", plantId = plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Component", Component = Component.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Component);
                cmd.Parameters.AddWithValue("@Employee", Employee = Employee.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Employee);
                cmd.Parameters.AddWithValue("@Operation", Opeartion);
                cmd.Parameters.AddWithValue("@ComparisonType", comparisonType);
                cmd.Parameters.AddWithValue("@Groupid", CellID = CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID);
                cmd.Parameters.AddWithValue("@View", View);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();

            }
            return dt;
        }

        internal static bool Checkgrouidpresent(string Plant, string text)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool present = false;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from PlantMachineGroups where plantID =@PlantID AND groupid = @groupid", sqlConn);
                cmd.Parameters.AddWithValue("@groupid", text);
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    present = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return present;
        }

        internal static DataTable GetSupComponentDetails()
        {
            DataTable dt = new DataTable();
            string query = @"[s_Tafe_ViewSupplierComponentDetails]";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                int count = sdr.FieldCount;
                for (int i = 0; i < count; i++)
                {
                    if (i == 0)
                        dt.Columns.Add(sdr.GetName(i), typeof(string));
                    else
                        dt.Columns.Add(sdr.GetName(i), typeof(bool));
                }
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        DataRow row = dt.NewRow();
                        for (int i = 0; i < count; i++)
                        {
                            if (i == 0)
                                row[i] = sdr[i].ToString();
                            else
                            {
                                if (sdr[i].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                                {
                                    row[i] = false;
                                }
                                else
                                {
                                    row[i] = true;
                                }
                            }
                        }
                        dt.Rows.Add(row);
                    }

                    //dt.Load(sdr);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return dt;
        }


        internal static bool SaveSupplierCodeComponent(string component, string supplier, bool supChecked)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            if (supChecked)
            {
                query = @"if not exists (select * from [SupplierComponent_Tafe] where SupplierCode=@SupplierCode and ComponentID=@ComponentID)
                                 begin
                                            insert into [SupplierComponent_Tafe](SupplierCode,ComponentID)
                                            values(@SupplierCode,@ComponentID);
                                 end
                             else
                                begin
                                            update [SupplierComponent_Tafe] set SupplierCode=@SupplierCode,ComponentID=@ComponentID where SupplierCode=@SupplierCode and ComponentID=@ComponentID;
                                end";
            }
            else
            {
                query = @"if exists (select * from [SupplierComponent_Tafe] where SupplierCode=@SupplierCode and ComponentID=@ComponentID)
                                 begin
                                            delete from [SupplierComponent_Tafe] where SupplierCode=@SupplierCode and ComponentID=@ComponentID;
                                 end";
            }
            bool SupplierCodeInsert = false;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierCode", supplier);
                cmd.Parameters.AddWithValue("@ComponentID", component);
                cmd.ExecuteNonQuery();
                SupplierCodeInsert = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return SupplierCodeInsert;
        }

        internal static Colorvalues GetColors(string viewType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            Colorvalues present = new Colorvalues();
            SqlDataReader sdr = null;
            string query = @"select * from TPMWEB_EfficiencyColorCoding where Type=@viewType";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@viewType", viewType);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        //if (sdr["PERed"].ToString().Equals("PERed", StringComparison.OrdinalIgnoreCase))
                        present.PERed = sdr["PERed"].ToString();
                        //if (sdr["AERed"].ToString().Equals("AERed", StringComparison.OrdinalIgnoreCase))
                        present.AERed = sdr["AERed"].ToString();
                        //if (sdr[""].ToString().Equals("QERed", StringComparison.OrdinalIgnoreCase))
                        present.QERed = sdr["QERed"].ToString();
                        //if (sdr[""].ToString().Equals("OERed", StringComparison.OrdinalIgnoreCase))
                        present.OERed = sdr["OEERed"].ToString();
                        //if (sdr[""].ToString().Equals("PEGreen", StringComparison.OrdinalIgnoreCase))
                        present.PEGreen = sdr["PEGreen"].ToString();
                        //if (sdr[""].ToString().Equals("AEGreen", StringComparison.OrdinalIgnoreCase))
                        present.AEGreen = sdr["AEGreen"].ToString();
                        //if (sdr[""].ToString().Equals("QEGreen", StringComparison.OrdinalIgnoreCase))
                        present.QEGreen = sdr["QEGreen"].ToString();
                        //if (sdr[""].ToString().Equals("OEGreen", StringComparison.OrdinalIgnoreCase))
                        present.OEGreen = sdr["OEEGreen"].ToString();
                        present.OperatorPEGreen = sdr["OPRGreen"].ToString();
                        present.OperatorPERed = sdr["OPRRed"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return present;
        }

        internal static void SaveColorData(string pEGreen, string pERed, string aEGreen, string aERed, string oEGreen, string oERed, string qEGreen, string qERed, string viewType, string OperatorPEGreen, string OperatorPERed)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            Colorvalues present = new Colorvalues();
            string query = @"if not exists(select * from TPMWEB_EfficiencyColorCoding where Type=@viewType)
begin
        insert into TPMWEB_EfficiencyColorCoding(Type,PEGreen,AEGreen,OEEGreen,QEGreen,PERed,AERed,OEERed,QERed,OPRGreen,OPRRed) 
        values(@viewType,@pegreen,@aegreen,@oeegreen,@qegreen,@pered,@aered,@oeered,@qered,@OPRGreen,@OPRRed)
end
else
begin
        update TPMWEB_EfficiencyColorCoding set PEGreen=@pegreen,AEGreen=@aegreen,OEEGreen=@oeegreen,QEGreen=@qegreen,
        PERed=@pered,AERed=@aered,OEERed=@oeered,QERed=@qered,OPRGreen=@OPRGreen,OPRRed=@OPRRed where Type=@viewType
end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@viewType", viewType);
                cmd.Parameters.AddWithValue("@pegreen", pEGreen);
                cmd.Parameters.AddWithValue("@pered", pERed);
                cmd.Parameters.AddWithValue("@aegreen", aEGreen);
                cmd.Parameters.AddWithValue("@aered", aERed);
                cmd.Parameters.AddWithValue("@oeegreen", oEGreen);
                cmd.Parameters.AddWithValue("@oeered", oERed);
                cmd.Parameters.AddWithValue("@qegreen", qEGreen);
                cmd.Parameters.AddWithValue("@qered", qERed);
                cmd.Parameters.AddWithValue("@OPRGreen", OperatorPEGreen);
                cmd.Parameters.AddWithValue("@OPRRed", OperatorPERed);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();

            }
        }

        internal static void SaveDashboarddetails(string Value, string Type)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"if not exists (select * from ShopDefaults where Parameter=@Type)
                                begin
                                insert into ShopDefaults (Parameter,ValueInText,UpdatedTS)
                                values(@Type,@Value,@UpdatedTS)
                                end
                                else
                                begin
                                update ShopDefaults set ValueInText=@Value,UpdatedTS=@UpdatedTS where Parameter=@Type
                                end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@Value", Value);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static void SaveComponentInfodetails(string Value, string Type)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"if not exists (select * from ShopDefaults where Parameter=@Type)
                                begin
                                insert into ShopDefaults (Parameter,ValueInText,UpdatedTS)
                                values(@Type,@Value,@UpdatedTS)
                                end
                                else
                                begin
                                update ShopDefaults set ValueInText=@Value,UpdatedTS=@UpdatedTS where Parameter=@Type
                                end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@Value", Value);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveComponentInfodetails: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static string GetType(string Type)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            string query = @"select * from ShopDefaults where Parameter=@Type";
            string InterfaceType = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@Type", Type);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        InterfaceType = rdr["ValueInText"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return InterfaceType;
        }

        internal static bool IsOperationFinished(string CompID, string OpnNo)
        {
            bool IsOprFinished = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"select * from componentoperationpricing where componentid = @componentid and operationno = @operationno and FinishedOperation = 1";
            SqlDataReader sqlDataReader = null;
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@componentid", CompID);
                cmd.Parameters.AddWithValue("@operationno", OpnNo);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                    IsOprFinished = true;
                else
                    IsOprFinished = false;
            }
            catch (Exception ex)
            {
                IsOprFinished = false;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return IsOprFinished;
        }

        internal static List<string> GetComponentsForMachine(string PlantId, string LineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allComp = new List<string>();
            string Query = string.Empty;
            if (string.IsNullOrEmpty(PlantId) && string.IsNullOrEmpty(LineID))
            {
                Query = @"select distinct C.componentid from componentinformation C
                            inner join componentoperationpricing COP on C.componentid = COP.componentid
                            Left outer join PlantMachineGroups P on P.MachineID = COP.machineid
                            Order by C.componentid";
            }
            else if (string.IsNullOrEmpty(PlantId))
            {
                Query = @"select distinct C.componentid from componentinformation C
                            inner join componentoperationpricing COP on C.componentid = COP.componentid
                            Left outer join PlantMachineGroups P on P.MachineID = COP.machineid
                            where(P.GroupID =@Line)
                            Order by C.componentid";
            }
            else if (string.IsNullOrEmpty(LineID))
            {
                Query = @"select distinct C.componentid from componentinformation C
                            inner join componentoperationpricing COP on C.componentid = COP.componentid
                            Left outer join PlantMachineGroups P on P.MachineID = COP.machineid
                            where(P.PlantID =@Plant)
                            Order by C.componentid";
            }
            else
            {
                Query = @"select distinct C.componentid from componentinformation C
                            inner join componentoperationpricing COP on C.componentid=COP.componentid
                            Left outer join PlantMachineGroups P on P.MachineID=COP.machineid
                            where P.PlantID=@Plant and P.GroupID=@Line
                            Order by C.componentid";
            }
            try
            {

                SqlCommand cmd = new SqlCommand(Query, sqlConn);
                cmd.Parameters.AddWithValue("@Line", LineID);
                cmd.Parameters.AddWithValue("@Plant", PlantId);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allComp.Add(Convert.ToString(rdr["Componentid"]));
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
            return allComp;
        }

        internal static void RemoveFinishOperation(string componentID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"update componentoperationpricing set FinishedOperation = 0 where componentid = @componentid";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@componentid", componentID);
                cmd.CommandType = CommandType.Text;
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
        internal static void RemoveFinishOperation(string componentID, string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"update componentoperationpricing set FinishedOperation = 0 where componentid = @componentid and MachineID=@MachineID ";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@componentid", componentID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandType = CommandType.Text;
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

        #region Rejection Page Globe

        public static List<string> GetAllGroupId(string plantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> plantid = new List<string>();
            string Query = @"";
            if (string.IsNullOrEmpty(plantId) || plantId.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct GroupID from PlantMachineGroups";
            }
            else
            {
                Query = "select distinct GroupID from PlantMachineGroups where PlantID=@PlantID";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);//
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    plantid.Add(rdr["GroupID"].ToString());
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
            return plantid;
        }

        #region "Group by Machine Bind"
        public static List<string> GetAllMachinedByLineandGroup(string Plantid, string GroupID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> plantid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct MachineID from PlantMachineGroups where (PlantID=@Plantid or @Plantid='' ) and (GroupID=@GroupID or @GroupID='') order by MachineID ", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    plantid.Add(rdr["MachineID"].ToString());
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
            return plantid;
        }
        #endregion

        #region "Dow Code By Machine Id"
        public static List<string> GetMachinewiseRejectionInfo(string subcategory, string category, string description)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> downCode = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                //if (machineID != "")
                //    sqlQuery = @"select distinct M.RejectionID from MachinewiseRejectioncodeMaster M inner join rejectioncodeinformation R on M.RejectionID = R.rejectionid 
                //   where M.Machineid in (" + machineID + ") and (R.Catagory=@Catagory or @Catagory='') order by M.RejectionID";
                //else
                //    sqlQuery = @"select distinct M.RejectionID from MachinewiseRejectioncodeMaster M inner join rejectioncodeinformation R on M.RejectionID = R.rejectionid
                //     inner join PlantMachineGroups PM on PM.MachineID = M.Machineid where (PM.GroupID=@groupId OR @groupId = '') and (R.Catagory=@Catagory or @Catagory='') 
                //     order by M.RejectionID";
                if (string.IsNullOrEmpty(category) || category.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(subcategory) && string.IsNullOrEmpty(description))
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation";
                    else if (string.IsNullOrEmpty(subcategory))
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where rejectiondescription=@rejectiondescription";
                    else if (string.IsNullOrEmpty(description))
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where SubCategory=@SubCategory";
                    else
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where rejectiondescription=@rejectiondescription and SubCategory=@SubCategory";
                }
                else
                {
                    if (string.IsNullOrEmpty(subcategory) && string.IsNullOrEmpty(description))
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where Catagory=@category";
                    else if (string.IsNullOrEmpty(subcategory))
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where Catagory=@category and rejectiondescription=@rejectiondescription";
                    else if (string.IsNullOrEmpty(description))
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where Catagory=@category and SubCategory=@SubCategory";
                    else
                        sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where Catagory=@category and rejectiondescription=@rejectiondescription and SubCategory=@SubCategory";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@SubCategory", subcategory);
                cmd.Parameters.AddWithValue("@rejectiondescription", description);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        if (!string.IsNullOrEmpty(rdr["RejectionID"].ToString()))
                            downCode.Add(rdr["RejectionID"].ToString());
                    }
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
            return downCode;
        }
        #endregion


        #region "Rejection Category"
        internal static List<string> databindforCatagorys()
        {
            List<string> lst = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            try
            {
                query = "select distinct Catagory from rejectioncodeinformation";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    if (!string.IsNullOrEmpty(sdr["Catagory"].ToString()))
                        lst.Add(sdr["Catagory"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lst;
        }
        #endregion

        #region "Rejection Code Info Data Chart"
        public static DataTable MachineRejectionCodeInfo(DateTime StartDate, DateTime EndDate, string PlantID, string Groupid, string MachineID, string ComponentID, string Operation, string Category, string SubCategory, string RejDescription, string Rejcode)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetAggDrilldownRejectionData_Dashboard", Con);
            cmd.CommandTimeout = 360;
            //cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            //cmd.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            //cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar).Value = PlantID;
            //cmd.Parameters.AddWithValue("@Groupid", Groupid);
            //cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = MachineID;
            //cmd.Parameters.AddWithValue("@Rejcatagory ", Rejcatagory);
            //cmd.Parameters.AddWithValue("@Rejcode", Rejcode);
            //cmd.Parameters.AddWithValue("@Param", ScrapBy);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@PlantID", PlantID);
            cmd.Parameters.AddWithValue("@Groupid", Groupid);
            cmd.Parameters.AddWithValue("@MachineID", MachineID);
            cmd.Parameters.AddWithValue("@Component", ComponentID);
            cmd.Parameters.AddWithValue("@Operation", Operation);
            cmd.Parameters.AddWithValue("@Rejcatagory", Category);
            cmd.Parameters.AddWithValue("@RejSubcatagory", SubCategory);
            cmd.Parameters.AddWithValue("@RejDescription", RejDescription);
            cmd.Parameters.AddWithValue("@Rejcode", Rejcode);
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
                if (sdr != null) sdr.Close();
            }
            return values;
        }

        public static List<TreeDataEntity> getTreeChartData(DateTime StartDate, DateTime EndDate, string PlantID, string Groupid, string MachineID, string ComponentID, string Operation, string Category, string SubCategory, string RejDescription, string Rejcode)
        {
            List<TreeDataEntity> list = new List<TreeDataEntity>();
            TreeDataEntity data = null;
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("[dbo].[s_GetAggDrilldownRejectionData_Chart]", Con);
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@PlantID", PlantID);
            cmd.Parameters.AddWithValue("@Groupid", Groupid);
            cmd.Parameters.AddWithValue("@MachineID", MachineID);
            cmd.Parameters.AddWithValue("@Component", ComponentID);
            cmd.Parameters.AddWithValue("@Operation", Operation);
            cmd.Parameters.AddWithValue("@Rejcatagory", Category);
            cmd.Parameters.AddWithValue("@RejSubcatagory", SubCategory);
            cmd.Parameters.AddWithValue("@RejDescription", RejDescription);
            cmd.Parameters.AddWithValue("@Rejcode", Rejcode);
            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new TreeDataEntity();
                        data.Catogory = sdr["RejCategory"].ToString();
                        data.Component = sdr["ComponentID"].ToString();
                        data.SubCatogory = sdr["RejSubcategory"].ToString();
                        data.Description = sdr["RejDesciption"].ToString();
                        data.Code = sdr["RejCode"].ToString();
                        data.CatogoryQty = sdr["CategoryTotal"].ToString();
                        data.ComponentQty = sdr["CompTotal"].ToString();
                        data.SubCatogoryQty = sdr["SubCategoryTotal"].ToString();
                        data.DescriptionQty = sdr["DescriptionTotal"].ToString();
                        data.CodeQty = sdr["RejCodeTotal"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        internal static List<string> GetComponentIDByMachine(string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> ComponentID = new List<string>();
            string Query = @"";
            if (string.IsNullOrEmpty(MachineID) || MachineID.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct componentid from componentoperationpricing";
            }
            else
            {
                Query = @"select distinct componentid from componentoperationpricing where machineid=@MachineID";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);//
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ComponentID.Add(rdr["componentid"].ToString());
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
            return ComponentID;
        }


        internal static List<string> GetOperation(string MachineID, string ComponentID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Operation = new List<string>();
            string Query = @"";
            MachineID = MachineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : MachineID;
            ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID;
            if (string.IsNullOrEmpty(ComponentID) && string.IsNullOrEmpty(MachineID))
            {
                Query = @"select distinct operationno from componentoperationpricing";
            }
            else if (string.IsNullOrEmpty(ComponentID))
            {
                Query = @"select distinct operationno from componentoperationpricing where machineid = @MachineID ";
            }
            else if (string.IsNullOrEmpty(MachineID))
            {
                Query = @"select distinct operationno from componentoperationpricing where componentid =@componentid";
            }
            else
            {
                Query = @"select distinct operationno from componentoperationpricing where machineid =@MachineID and componentid =@ComponentID";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@componentid", ComponentID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Operation.Add(rdr["operationno"].ToString());
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
            return Operation;
        }

        internal static List<string> GetOperationForMultiComponent(string MachineID, string ComponentID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Operation = new List<string>();
            string Query = @"";
            MachineID = MachineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : MachineID;
            ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID;
            if (string.IsNullOrEmpty(ComponentID) && string.IsNullOrEmpty(MachineID))
            {
                Query = @"select distinct operationno from componentoperationpricing";
            }
            else if (string.IsNullOrEmpty(ComponentID))
            {
                Query = @"select distinct operationno from componentoperationpricing where machineid = @MachineID ";
            }
            else if (string.IsNullOrEmpty(MachineID))
            {
                Query = @"select distinct operationno from componentoperationpricing where componentid in (" + ComponentID + ")";
            }
            else
            {
                Query = @"select distinct operationno from componentoperationpricing where machineid =@MachineID and componentid in (" + ComponentID + ")";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@componentid", ComponentID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Operation.Add(rdr["operationno"].ToString());
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
            return Operation;
        }
        internal static List<string> GetSubOperation(string MachineID, string ComponentID, string DefaultOrOptional)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> Operation = new List<string>();
            string Query = @"";
            MachineID = MachineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : MachineID;
            ComponentID = ComponentID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ComponentID;
            if (DefaultOrOptional == "Default")
            {
                Query = @"select distinct Activity from  AssemblyActivityMaster_GEA where Station=@MachineID and IsDefault=1  and (Componentid = @ComponentID or isnull(@ComponentID,'')='')";
            }
            else
            {
                Query = @"select distinct Activity from  AssemblyActivityMaster_GEA where Station=@MachineID and IsDefault=0  and (Componentid = @ComponentID or isnull(@ComponentID,'')='')";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ComponentID", ComponentID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Operation.Add(rdr["Activity"].ToString());
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
            return Operation;
        }
        #endregion
        #endregion


        #region "--------Bind Machine Info For Changging Setting----------- "
        internal static DataTable GetMachineInfoForChangeSetting(string Param, string Plantid, string Machineid, string SortOrder, string Visibility, string user)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("s_ANDON_MachinewiseSortOrder", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@Visibility", Visibility);
                cmd.Parameters.AddWithValue("@UserID", user);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }

            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error in retriving Machine - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }
        #endregion

        #region "---------Save Machine Setting Data------------------"
        internal static void SaveMachineSettingInfo(string Param, string Plantid, string Machineid, string SortOrder, string Visibility, out string SaveRecords, string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("s_ANDON_MachinewiseSortOrder", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@Visibility", Visibility);
                cmd.Parameters.AddWithValue("@UserID", user);
                cmd.ExecuteNonQuery();
                SaveRecords = "Successfull";
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error in retriving Machine - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion


        internal static AndonSettingsData GetAndonData()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            AndonSettingsData Data = new AndonSettingsData();
            try
            {
                cmd = new SqlCommand("", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        if (rdr["Parameter"].ToString().Equals("AndonTitle"))
                            Data.AndonTitle = rdr["ValueInText"].ToString();
                        if (rdr["Parameter"].ToString().Equals("SelectedAndonPlantID"))
                            Data.PlantID = rdr[""].ToString();
                        if (rdr["Parameter"].ToString().Equals("AndonRefreshData"))
                            Data.RefreshData = Convert.ToInt32(rdr["ValueInInt"].ToString());
                        if (rdr["Parameter"].ToString().Equals("AndonViewType"))
                            Data.Type = rdr["ValueInText"].ToString();
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
            return Data;
        }

        internal static string SaveAndonSettingsdata(string plantID, string refreshInterval, string title, string type)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string msg = "";
            string Query = @"if not exists(select * from CockpitDefaults where parameter='AndonTitle')
                                begin 
                                insert into CockpitDefaults(Parameter,ValueInText)
                                values('AndonTitle','@AndonTitle')
                                end
                                else
                                begin
                                update CockpitDefaults set ValueInText=@AndonTitle'' where Parameter='AndonTitle'
                                end
                            if not exists(select * from CockpitDefaults where parameter='SelectedAndonPlantID')
                                begin 
                                insert into CockpitDefaults(Parameter,ValueInText)
                                values('SelectedAndonPlantID','@PlantID')
                                end
                                else
                                begin
                                update CockpitDefaults set ValueInText='@RefreshInterval' where Parameter='@PlantID'
                                end
                            if not exists(select * from CockpitDefaults where parameter='AndonRefreshData')
                                begin 
                                insert into CockpitDefaults(Parameter,ValueInText)
                                values('AndonRefreshData','@RefreshInterval')
                                end
                                else
                                begin
                                update CockpitDefaults set ValueInText='@RefreshInterval' where Parameter='AndonRefreshData'
                                end
                            if not exists(select * from CockpitDefaults where parameter='AndonViewType')
                                begin 
                                insert into CockpitDefaults(Parameter,ValueInText)
                                values('AndonViewType','@AndonViewType')
                                end
                                else
                                begin
                                update CockpitDefaults set ValueInText='@AndonViewType' where Parameter='AndonViewType'
                                end";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@AndonTitle", title);
                cmd.Parameters.AddWithValue("@RefreshInterval", refreshInterval);
                cmd.Parameters.AddWithValue("@AndonViewType", type);
                cmd.ExecuteNonQuery();
                msg = "Data Saved";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return msg;
        }

        #region "Rejection Sub Category"
        internal static List<string> databindforSubCatagorys(string Category)
        {
            List<string> lst = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            try
            {
                query = "select distinct SubCategory from rejectioncodeinformation where (Catagory=@Catagory or isnull(@Catagory,'')='') ";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@Catagory", Category);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    if (!string.IsNullOrEmpty(sdr["SubCategory"].ToString()))
                        lst.Add(sdr["SubCategory"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lst;
        }
        #endregion

        #region "Rejection Des Category"
        internal static List<string> databindforDesCatagorys(string Category, string SubCategory)
        {
            List<string> lst = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            if (string.IsNullOrEmpty(Category) && string.IsNullOrEmpty(SubCategory))
            {
                query = "select distinct rejectiondescription from rejectioncodeinformation";
            }
            else if (string.IsNullOrEmpty(Category))
            {
                query = "select distinct rejectiondescription from rejectioncodeinformation where SubCategory=@SubCategory";
            }
            else if (string.IsNullOrEmpty(SubCategory))
            {
                query = "select distinct rejectiondescription from rejectioncodeinformation where Catagory=@Catagory";
            }
            else
            {
                query = "select distinct rejectiondescription from rejectioncodeinformation where SubCategory=@SubCategory and Catagory=@Catagory";
            }
            try
            {

                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.AddWithValue("@Catagory", Category);
                cmd.Parameters.AddWithValue("@SubCategory", SubCategory);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    if (!string.IsNullOrEmpty(sdr["rejectiondescription"].ToString()))
                        lst.Add(sdr["rejectiondescription"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error log -\n" + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lst;
        }
        #endregion

        #region "Get drilldown chart information"
        public static DataTable GetChartInfo(DateTime StartDate, DateTime EndDate, string PlantID, string Groupid, string MachineID, string ComponentID, string Operation, string Category, string SubCategory, string DesCategory, string Rejcode, string param)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            DataTable values = new DataTable();
            SqlCommand cmd = new SqlCommand("s_GetAggDrilldownRejectionData_Dashboard", Con);
            cmd.CommandTimeout = 360;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@PlantID", PlantID);
            cmd.Parameters.AddWithValue("@Groupid", Groupid);
            cmd.Parameters.AddWithValue("@MachineID", MachineID);
            cmd.Parameters.AddWithValue("@Component", ComponentID);
            cmd.Parameters.AddWithValue("@Operation", Operation);
            cmd.Parameters.AddWithValue("@Rejcatagory", Category);
            cmd.Parameters.AddWithValue("@RejSubcatagory", SubCategory);
            cmd.Parameters.AddWithValue("@RejDescription", DesCategory);
            cmd.Parameters.AddWithValue("@Rejcode", Rejcode);
            cmd.Parameters.AddWithValue("@Param", param);

            SqlDataReader sdr = null;
            try
            {
                sdr = cmd.ExecuteReader();
                values.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (Con != null) Con.Close();
                if (sdr != null) sdr.Close();
            }
            return values;
        }

        #endregion

        internal static object GetMachinewiseRejectionInfo_notGlobe(string Category)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> downCode = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                //if (machineID != "")
                //    sqlQuery = @"select distinct M.RejectionID from MachinewiseRejectioncodeMaster M inner join rejectioncodeinformation R on M.RejectionID = R.rejectionid 
                //   where M.Machineid in (" + machineID + ") and (R.Catagory=@Catagory or @Catagory='') order by M.RejectionID";
                //else
                //    sqlQuery = @"select distinct M.RejectionID from MachinewiseRejectioncodeMaster M inner join rejectioncodeinformation R on M.RejectionID = R.rejectionid
                //     inner join PlantMachineGroups PM on PM.MachineID = M.Machineid where (PM.GroupID=@groupId OR @groupId = '') and (R.Catagory=@Catagory or @Catagory='') 
                //     order by M.RejectionID";
                if (string.IsNullOrEmpty(Category) || Category.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    sqlQuery = @"select distinct RejectionID from rejectioncodeinformation";
                }
                else
                {
                    sqlQuery = @"select distinct RejectionID from rejectioncodeinformation where Catagory=@category";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@category", Category);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        if (!string.IsNullOrEmpty(rdr["RejectionID"].ToString()))
                            downCode.Add(rdr["RejectionID"].ToString());
                    }
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
            return downCode;
        }


        internal static void saveAndonTitledetails(string parameter, string ValueinText, string andonTitle)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            string Query = @"if not exists(select * from CockpitDefaults where Parameter=@Parameter and ValueInText=@ValueInText)
                                begin
                                insert into CockpitDefaults(Parameter,ValueInText,ValueInText2,UpdatedTS)
                                values(@Paramater,@ValueInText,@AndonTitle,@UpdatedTS)
                                end
                                else
                                begin
                                update CockpitDefaults set ValueInText2=@AndonTitle,UpdatedTS=@UpdatedTS where Parameter=@Parameter and ValueInText=@ValueInText
                                end";
            try
            {
                SqlCommand cmd = new SqlCommand(Query, Conn);
                cmd.Parameters.AddWithValue("@Paramater", parameter);
                cmd.Parameters.AddWithValue("@ValueInText", ValueinText);
                cmd.Parameters.AddWithValue("@AndonTitle", andonTitle);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();

            }
        }

        internal static string GetAndonSettingsData(string parameter, string ValueInText)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            string val = "";
            string Query = @"select ValueInText from CockpitDefaults where Parameter=@Parameter and ValueIntext=@valueInText";
            try
            {
                SqlCommand cmd = new SqlCommand(Query, Conn);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@valueInText", ValueInText);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    val = rdr["ValueInText2"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return val;
        }
        #region ------ Historical & Live Default View ------------------
        internal static string SaveHistoricalLiveDefaultViewdata(string parameter, string text1, string text2)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string msg = "";
            string Query = @"if not exists(select * from CockpitDefaults where parameter=@param and ValueInText=@text1)
                                begin 
                                insert into CockpitDefaults(Parameter,ValueInText,ValueInText2,UpdatedTS)
                                values(@param,@text1,@text2,@UpdatedTS)
                                end
                                else
                                begin
                                update CockpitDefaults set ValueInText2=@text2,UpdatedTS=@UpdatedTS where parameter=@param and ValueInText=@text1
                                end
                           ";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@param", parameter);
                cmd.Parameters.AddWithValue("@text1", text1);
                cmd.Parameters.AddWithValue("@text2", text2);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                msg = "Data Saved";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return msg;
        }
        internal static List<CockpitViewSettingClass> getHistoricalLiveDefaultViewData()
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            string Query = @"select * from CockpitDefaults where Parameter='HistoricalDefaultView' or Parameter='LiveDefaultView'";
            List<CockpitViewSettingClass> list = new List<CockpitViewSettingClass>();
            CockpitViewSettingClass data = null;
            try
            {
                SqlCommand cmd = new SqlCommand(Query, Conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    data = new CockpitViewSettingClass();
                    data.Parameter = rdr["Parameter"].ToString();
                    data.ValueInText = rdr["ValueInText"].ToString();
                    data.ValueInText2 = rdr["ValueInText2"].ToString();
                    list.Add(data);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        #endregion

        #region CELLDEFINATION_DELETE_BY_KAVYA
        internal static List<string> DeleteGrpId1(string plantID, string groupId)
        {
            List<string> result = new List<string>();

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;

            SqlDataReader dr = null;
            //bool success = false;
            try
            {
                string sqlQuery = "delete from [dbo].[PlantMachineGroups] where PlantID = @plantID and groupid=@GroupID";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@plantID", plantID);
                cmd.Parameters.AddWithValue("@GroupID", groupId);
                cmd.CommandType = System.Data.CommandType.Text;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(dr["PlantID"].ToString());
                    result.Add(dr["GroupID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (dr != null) dr.Close();
            }
            return result;
        }
        #endregion

        internal static List<string> GetVDGColHeader(string parameter, string Language, out List<string> val2)
        {
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            val2 = new List<string>();
            List<string> val = new List<string>();
            string Query = @"select * from CockpitDefaults where Parameter=@Parameter and languagespecified=@languagespecified order by valueinint";
            try
            {
                SqlCommand cmd = new SqlCommand(Query, Conn);
                cmd.Parameters.AddWithValue("@Parameter", parameter);
                cmd.Parameters.AddWithValue("@languagespecified", Language);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    val.Add(rdr["ValueInText2"].ToString());
                    val2.Add(rdr["ValueInText"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (rdr != null) rdr.Close();

            }
            return val;
        }
        internal static void DeleteFromCalender(int year, int monthNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"delete from Calender where YearNo=@yrNo and MonthVal>=@MonthVal
delete from Calender where YearNo=(@yrNo+1) and MonthVal<=(@MonthVal-1)";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@yrNo", year);
                cmd.Parameters.AddWithValue("@MonthVal", monthNo);
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

        internal static DataTable GetWeekInformationFromDB(int year)
        {
            DataTable dtWkInfo = new DataTable();
            dtWkInfo.Columns.AddRange(new DataColumn[] { new DataColumn("WeekDate", typeof(string)), new DataColumn("WeekNumber", typeof(int)), new DataColumn("MonthVal", typeof(int)), new DataColumn("YearNo", typeof(int)) });
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            string query = @"select * from Calender where YearNo=@year";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@year", year);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DataRow dataRow = dtWkInfo.NewRow();
                        dataRow["WeekDate"] = Convert.ToDateTime(reader["WeekDate"].ToString()).ToString("dd-MMM-yyyy");
                        dataRow["WeekNumber"] = Convert.ToInt32(reader["WeekNumber"].ToString());
                        dataRow["MonthVal"] = Convert.ToInt32(reader["MonthVal"].ToString());
                        dataRow["YearNo"] = Convert.ToInt32(reader["YearNo"]);
                        dtWkInfo.Rows.Add(dataRow);
                    }
                    //dtWkInfo.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return dtWkInfo;
        }

        internal static void BulkInsertIntoCalender(DataTable dtInsertInToCalender)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlBulkCopy sqlBulkCopy = null;
            try
            {
                sqlBulkCopy = new SqlBulkCopy(conn);
                sqlBulkCopy.DestinationTableName = "Calender";
                sqlBulkCopy.ColumnMappings.Add("WeekDate", "WeekDate");
                sqlBulkCopy.ColumnMappings.Add("WeekNumber", "WeekNumber");
                sqlBulkCopy.ColumnMappings.Add("MonthVal", "MonthVal");
                sqlBulkCopy.ColumnMappings.Add("YearNo", "YearNo");
                sqlBulkCopy.WriteToServer(dtInsertInToCalender);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (sqlBulkCopy != null) sqlBulkCopy.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static bool GetWorkOrder()
        {
            bool workorder = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                cmd = new SqlCommand("select WorkOrder from SmartdataPortRefreshDefaults", conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    workorder = reader["WorkOrder"].ToString().Equals("y", StringComparison.OrdinalIgnoreCase) ? true : false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn != null) conn.Close();
            }
            return workorder;
        }
        internal static DataTable GetMachineOperatordetails(DateTime Fromdate, DateTime Todate, string MachineID, string type)
        {
            List<ShanthiMachineOperator> data = new List<ShanthiMachineOperator>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> dates = new List<string>();
            string query = string.Empty;
            DataTable dt = new DataTable();

            DateTime date = DateTime.Now;
            query = @"s_GetEmployeeShiftAllocation_Shanti";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", Fromdate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Enddate", Todate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineId", MachineID);
                cmd.Parameters.AddWithValue("@Type", type);
                rdr = cmd.ExecuteReader();
                //while (rdr.Read())
                //{
                dt.Load(rdr);
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static void SaveEmployeeShift(string date, string machineID, string type, string shift, string Employee)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            try
            {
                query = @"if not exists(select * from EmployeeShiftAllocation_Shanti where MachineId=@MachineId and Employee=@Employee and [date]=@date and [shift]=@shift)
begin
insert into EmployeeShiftAllocation_Shanti(MachineId, Employee, [Type], [date], [shift])
values(@MachineId, @Employee, @Type, @date, @shift)
end ";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineId", machineID);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@shift", shift);
                cmd.Parameters.AddWithValue("@date", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static void DeleteEmployeeShift(string date, string machineID, string type, string Employee)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            try
            {
                query = @"if exists(select * from EmployeeShiftAllocation_Shanti where MachineId=@MachineId and Employee=@Employee and [date]=@date)
begin
delete from EmployeeShiftAllocation_Shanti  where MachineId=@MachineId and Employee=@Employee and [date]=@date
end ";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineId", machineID);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Employee", Employee);
                cmd.Parameters.AddWithValue("@date", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static bool GetStatusOfSubOperator()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool status = false;

            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from shopdefaults where parameter='SubOperation'", sqlConn);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    status = rdr["ValueInText"].ToString().Equals("Y", StringComparison.OrdinalIgnoreCase) ? true : false;
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return status;
        }

        internal static bool AggregateData(string machineID, DateTime fromDate, DateTime toDate, string LastAggrigate, string PlantID)
        {
            bool updated = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = @"s_Push_Prodn_Down_ShiftAggregation";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@LastAggrecord", LastAggrigate);
                cmd.Parameters.AddWithValue("@GroupID", "");
                cmd.Parameters.AddWithValue("@Shift", "");
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.ExecuteNonQuery();
                updated = true;
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
            return updated;
        }

        internal static bool Getverification(string userName)
        {
            bool updated = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = @"select * from employeeinformation where Employeeid = @emp";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@emp", userName);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    updated = true;
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
            return updated;
        }


        internal static void DeleteAggregatedData(string Query, string machineID, string lastAggregatedDate)
        {
            bool updated = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(Query, conn);
                //cmd.Parameters.AddWithValue("@MachineID", machineID);
                //cmd.Parameters.AddWithValue("@pDate", lastAggregatedDate);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }


        internal static List<AggregatedDataEntry> GetAggregatedData(string PlantID)
        {
            List<AggregatedDataEntry> entityList = new List<AggregatedDataEntry>();

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = @"";
            if (string.IsNullOrEmpty(PlantID))
            {
                Query = @"select distinct  T1.machineid as machineid,T1.lastdate as MaxDate from Shiftaggtrail S right 
outer join ( SELECT max(S.aggdate) as lastdate,M.Machineid, case when max(S.endtime) is not null then max(S.endtime) else ''''   
end as LastAggstart  from  plantmachine M  left outer  join ShiftAggTrail as S on M.Machineid=S.Machineid  
group by M.Machineid)  T1 on T1.Machineid=S.Machineid  and T1.lastdate=S.aggdate  and S.Endtime=T1.LastAggstart";
            }
            else
            {
                Query = @"select distinct  T1.machineid as machineid,T1.lastdate as MaxDate from Shiftaggtrail S right 
outer join ( SELECT max(S.aggdate) as lastdate,M.Machineid, case when max(S.endtime) is not null then max(S.endtime) else ''''   
end as LastAggstart  from  plantmachine M  left outer  join ShiftAggTrail as S on M.Machineid=S.Machineid where M.plantid = @PlantID 
group by M.Machineid)  T1 on T1.Machineid=S.Machineid  and T1.lastdate=S.aggdate  and S.Endtime=T1.LastAggstart ";
            }
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    AggregatedDataEntry entry = new AggregatedDataEntry();
                    entry.MachineID = rdr["machineid"].ToString();
                    entry.AggData = false;
                    if (rdr["MaxDate"].ToString().Equals(DBNull.Value) || string.IsNullOrEmpty(rdr["MaxDate"].ToString()))
                    {
                        entry.LastAggDate = "";
                    }
                    else
                    {
                        entry.LastAggDate = Convert.ToDateTime(rdr["MaxDate"].ToString()).ToString("dd-MM-yyyy");
                    }
                    entityList.Add(entry);
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
            return entityList;
        }

        internal static List<string> GetAllComponentbyMachine(string Machineid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allComps = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Machineid))
                    sqlQuery = "select distinct componentid from componentoperationpricing where machineid=@machineid";
                else
                    sqlQuery = "select distinct componentid from componentoperationpricing";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@machineid", Machineid);
                SqlDataReader rdr = cmd.ExecuteReader();
                allComps.Add("All");
                while (rdr.Read())
                {
                    allComps.Add(Convert.ToString(rdr["componentid"]));
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
            return allComps;
        }


        internal static List<LoadDefinitionEntity> GetLoadDefinitiondata(DateTime FromDate, DateTime ToDate, string PlantID, string machineid, string componentID, string Operation, int shiftCount)
        {
            List<LoadDefinitionEntity> EntityList = new List<LoadDefinitionEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = @"S_Get_LoadScheduleDetails";
            string shift = string.Empty, MachineID = string.Empty, ComponentID = string.Empty, operation = string.Empty;
            DateTime Date = DateTime.Now;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", FromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", ToDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineId", machineid);
                cmd.Parameters.AddWithValue("@ComponentId", componentID);
                cmd.Parameters.AddWithValue("@OperationId", Operation);
                cmd.Parameters.AddWithValue("@Param", "View");
                rdr = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(rdr);
                int count = 0;
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    int j = 1;
                    while (count < dataTable.Rows.Count)
                    {
                        DataTable dataForDate = dataTable.AsEnumerable().Skip(count).Take(shiftCount).CopyToDataTable();
                        if (dataForDate != null)
                        {
                            LoadDefinitionEntity data = new LoadDefinitionEntity();
                            data.SLNO = j;
                            data.MachineID = dataForDate.Rows[0]["Machineid"].ToString();
                            data.ComponentID = dataForDate.Rows[0]["Componentid"].ToString();
                            data.Operation = dataForDate.Rows[0]["Operationid"].ToString();
                            data.StdCycleTime = dataForDate.Rows[0]["StdCycleTime"].ToString();
                            if (shiftCount == 1)
                            {
                                data.Shift1Target = dataForDate.Rows[0]["Target"].ToString();
                            }
                            else if (shiftCount == 2)
                            {
                                data.Shift1Target = dataForDate.Rows[0]["Target"].ToString();
                                data.Shift2Target = dataForDate.Rows[1]["Target"].ToString();
                            }
                            else
                            {
                                data.Shift1Target = dataForDate.Rows[0]["Target"].ToString();
                                data.Shift2Target = dataForDate.Rows[1]["Target"].ToString();
                                data.Shift3Target = dataForDate.Rows[2]["Target"].ToString();
                            }
                            if (shiftCount == 1)
                            {
                                data.Calculatetgt1 = dataForDate.Rows[0]["ShiftTarget"].ToString();
                            }
                            else if (shiftCount == 2)
                            {
                                data.Calculatetgt1 = dataForDate.Rows[0]["ShiftTarget"].ToString();
                                data.Calculatetgt2 = dataForDate.Rows[1]["ShiftTarget"].ToString();
                            }
                            else
                            {
                                data.Calculatetgt1 = dataForDate.Rows[0]["ShiftTarget"].ToString();
                                data.Calculatetgt2 = dataForDate.Rows[1]["ShiftTarget"].ToString();
                                data.Calculatetgt3 = dataForDate.Rows[2]["ShiftTarget"].ToString();
                            }
                            data.Date = Convert.ToDateTime(dataForDate.Rows[0]["FromDate"].ToString()).ToString("dd-MM-yyyy");
                            data.TotalTarget = dataForDate.Rows[0]["TotalTarget"].ToString();

                            if (shiftCount == 1)
                            {
                                data.PDTShift1 = string.IsNullOrEmpty(dataForDate.Rows[0]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[0]["PDT"].ToString()), 0).ToString();
                            }
                            else if (shiftCount == 2)
                            {
                                data.PDTShift1 = string.IsNullOrEmpty(dataForDate.Rows[0]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[0]["PDT"].ToString()), 0).ToString();
                                data.PDTShift2 = string.IsNullOrEmpty(dataForDate.Rows[1]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[1]["PDT"].ToString()), 0).ToString();
                            }
                            else
                            {
                                data.PDTShift1 = string.IsNullOrEmpty(dataForDate.Rows[0]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[0]["PDT"].ToString()), 0).ToString();
                                data.PDTShift2 = string.IsNullOrEmpty(dataForDate.Rows[1]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[1]["PDT"].ToString()), 0).ToString();
                                data.PDTShift3 = string.IsNullOrEmpty(dataForDate.Rows[2]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[2]["PDT"].ToString()), 0).ToString();
                            }
                            EntityList.Add(data);
                            //EntityList.Add(new LoadDefinitionEntity()
                            //{
                            //    SLNO = j,
                            //    MachineID = dataForDate.Rows[0]["Machineid"].ToString(),
                            //    ComponentID = dataForDate.Rows[0]["Componentid"].ToString(),
                            //    Operation = dataForDate.Rows[0]["Operationid"].ToString(),
                            //    StdCycleTime = dataForDate.Rows[0]["StdCycleTime"].ToString(),
                            //    Shift1Target = dataForDate.Rows[0]["Target"].ToString(),
                            //    Shift2Target = dataForDate.Rows[1]["Target"].ToString(),
                            //    Shift3Target = dataForDate.Rows[2]["Target"].ToString(),
                            //    Calculatetgt1 = dataForDate.Rows[0]["ShiftTarget"].ToString(),
                            //    Calculatetgt2 = dataForDate.Rows[1]["ShiftTarget"].ToString(),
                            //    Calculatetgt3 = dataForDate.Rows[2]["ShiftTarget"].ToString(),
                            //    Date = Convert.ToDateTime(dataForDate.Rows[0]["FromDate"].ToString()).ToString("dd-MM-yyyy"),
                            //    TotalTarget = dataForDate.Rows[0]["TotalTarget"].ToString(),
                            //    PDTShift1 = string.IsNullOrEmpty(dataForDate.Rows[0]["PDT"].ToString())?"":Math.Round( Convert.ToDecimal(dataForDate.Rows[0]["PDT"].ToString()),0).ToString(),
                            //    PDTShift2 = string.IsNullOrEmpty(dataForDate.Rows[1]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[1]["PDT"].ToString()), 0).ToString() ,
                            //    PDTShift3 = string.IsNullOrEmpty(dataForDate.Rows[2]["PDT"].ToString()) ? "" : Math.Round(Convert.ToDecimal(dataForDate.Rows[2]["PDT"].ToString()), 0).ToString(),
                            //});
                        }
                        j++;
                        count += shiftCount;
                    }
                }
                //int i = 1;
                //while (rdr.Read())
                //{
                //    LoadDefinitionEntity entity = new LoadDefinitionEntity();
                //    entity.SLNO = i++;
                //    entity.MachineID = rdr["MachineId"].ToString();
                //    entity.ComponentID = rdr["ComponentId"].ToString();
                //    entity.Operation = rdr["OperationId"].ToString();
                //    entity.StdCycleTime = rdr["StdCycleTime"].ToString();
                //    entity.Shift1Target = rdr["idd"].ToString();
                //    entity.Shift2Target = rdr["idd"].ToString();
                //    entity.Shift3Target = rdr["idd"].ToString();
                //    entity.Date = Convert.ToDateTime(rdr["FromDate"].ToString()).ToString("dd-MM-yyyy");
                //    entity.TotalTarget = rdr["Target"].ToString();
                //    EntityList.Add(entity);
                //}
                //DataTable dt = new DataTable();
                //rdr.NextResult();
                //while (rdr.Read())
                //{
                //    foreach (LoadDefinitionEntity ent in EntityList)
                //    {
                //        if (ent.MachineID.Equals(rdr["MachineId"].ToString()) && ent.ComponentID.Equals(rdr["ComponentId"].ToString()) && ent.Operation.Equals(rdr["OperationId"].ToString()) && ent.Date.Equals(Convert.ToDateTime(rdr["FromDate"].ToString()).ToString("dd-MM-yyyy")))
                //        {
                //            ent.PDTShift1 = rdr[""].ToString();
                //            ent.PDTShift2 = rdr[""].ToString();
                //            ent.PDTShift3 = rdr[""].ToString();
                //        }
                //    }
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
            return EntityList;
        }


        internal static bool SaveLoadDefinition(int idd, string machineId, string component, string operation, DateTime date, string targetTotal, string shiftTarget, string Shift)
        {
            bool status = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if not exists(select * from LoadSchedule where [date]=@Date and [Shift]=@Shift and Machine=@Machine and Operation=@Operation and Component=@Component)
                                begin
                                insert into LoadSchedule ([date],[Shift],Machine,Component,Operation,IdealCount)
                                values(@Date,@Shift,@Machine,@Component,@Operation,@IdealCount)
                                end
                                else
                                begin
                                update LoadSchedule set IdealCount=@IdealCount where [date]=@Date and [Shift]=@Shift and Machine=@Machine and Operation=@Operation  and Component=@Component
                                end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("idd", idd);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Machine", machineId);
                cmd.Parameters.AddWithValue("@Component", component);
                cmd.Parameters.AddWithValue("@Operation", operation);
                cmd.Parameters.AddWithValue("@IdealCount", shiftTarget);
                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return status;
        }

        internal static bool DeleteLoadDefinition(int idd, DateTime Date, string machineid)
        {
            bool status = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if exists (select * from LoadSchedule where [date]=@Date and Machine=@Machine)
                                begin 
                                delete from LoadSchedule where [date]=@Date and Machine=@Machine
                                end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Machine", machineid);
                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return status;
        }

        internal static List<string> GetAllParameterWithInteface(DateTime StartTime, DateTime EndTime, string plantid, string MacInt, string Comp, string Opn, string Opr, string downcode, string Datatype, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                if (string.Equals(plantid, "All", StringComparison.OrdinalIgnoreCase)) plantid = "";
                cmd = new SqlCommand("[s_GetProdDownRejectiondata]", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"StartTime", StartTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue(@"EndTime", EndTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue("@machineid", MacInt);
                cmd.Parameters.AddWithValue("@Component", Comp);
                cmd.Parameters.AddWithValue("@operation", Opn);
                cmd.Parameters.AddWithValue("@Operator", Opr);
                cmd.Parameters.AddWithValue("@DownCode", downcode);
                cmd.Parameters.AddWithValue("@Plantid", plantid);
                cmd.Parameters.AddWithValue(@"Datatype", Datatype);
                cmd.Parameters.AddWithValue("@param", param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["interfaceid"].ToString());
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
            return list;
        }
        internal static List<ModifiedDataEntity> GetAllParameterWithIntefaceAndCode(DateTime StartTime, DateTime EndTime, string plantid, string MacInt, string Comp, string Opn, string Opr, string downcode, string Datatype, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ModifiedDataEntity> list = new List<ModifiedDataEntity>();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            ModifiedDataEntity data = null;
            try
            {
                if (string.Equals(plantid, "All", StringComparison.OrdinalIgnoreCase)) plantid = "";
                cmd = new SqlCommand("[s_GetProdDownRejectiondata]", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"StartTime", StartTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue(@"EndTime", EndTime.ToSQLDateTimeFormat());
                cmd.Parameters.AddWithValue("@machineid", MacInt);
                cmd.Parameters.AddWithValue("@Component", Comp);
                cmd.Parameters.AddWithValue("@operation", Opn);
                cmd.Parameters.AddWithValue("@Operator", Opr);
                cmd.Parameters.AddWithValue("@DownCode", downcode);
                cmd.Parameters.AddWithValue("@Plantid", plantid);
                cmd.Parameters.AddWithValue(@"Datatype", Datatype);
                cmd.Parameters.AddWithValue("@param", param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    data = new ModifiedDataEntity();
                    data.InterfaceID = rdr["interfaceid"].ToString();
                    data.InterfaceIWithID = rdr["result"].ToString();
                    list.Add(data);
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
            return list;
        }
        internal static bool UpdateModifiedRejectionData(string oldRejectionValue, string newRejectionValue, string machineID, DateTime sttime, DateTime ndtime, int Datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                cmd = new SqlCommand(@"update AutodataRejections set Rejection_Code=@newRejectionValue where Rejection_Code=@oldRejectionValue and mc=@machineID and  createdts between @StartTime and @EndTime", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@newRejectionValue", oldRejectionValue);
                cmd.Parameters.AddWithValue("@oldRejectionValue", newRejectionValue);
                cmd.Parameters.AddWithValue("@machineID", machineID);
                cmd.Parameters.AddWithValue("@StartTime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updatedRow = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRow = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }

        internal static List<OperationByPassEntity> GetDataForGrid(string Comp, string Date)
        {
            List<OperationByPassEntity> DataEntityList = new List<OperationByPassEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string Query = "";
            if (string.IsNullOrEmpty(Comp) && string.IsNullOrEmpty(Date))
            {
                Query = @"select distinct ci.componentid,ci.operationno,OB.EffectivedateFrom,OB.EffectivedateTo  from componentoperationpricing ci left join OperationBypassDetails_Shanti OB on ci.componentid = OB.[Componentid] and ci.operationno = OB.operationno";
            }
            else if (string.IsNullOrEmpty(Date))
            {
                Query = @"select distinct ci.componentid,ci.operationno,OB.EffectivedateFrom,OB.EffectivedateTo from componentoperationpricing ci  left join OperationBypassDetails_Shanti OB on ci.componentid = OB.[Componentid] and ci.operationno = OB.operationno where ci.componentid like '%" + Comp + "%'";
            }
            else if (string.IsNullOrEmpty(Comp))
            {
                Query = @"select distinct ci.componentid,ci.operationno,OB.EffectivedateFrom,OB.EffectivedateTo from componentoperationpricing ci  left join OperationBypassDetails_Shanti OB on ci.componentid = OB.[Componentid] 
and ci.operationno = OB.operationno where ('" + Util.GetDateTime(Date).ToString("yyyy-MM-dd HH:mm:ss") + "' between Ob.EffectivedateFrom and OB.EffectivedateTo)";
            }
            else
            {
                Query = @"select distinct ci.componentid,ci.operationno,OB.EffectivedateFrom,OB.EffectivedateTo from componentoperationpricing ci  left join OperationBypassDetails_Shanti OB on ci.componentid = OB.[Componentid] 
and ci.operationno = OB.operationno where ('" + Util.GetDateTime(Date).ToString("yyyy-MM-dd HH:mm:ss") + "' between Ob.EffectivedateFrom and OB.EffectivedateTo) and ci.componentid like '%" + Comp + "%'";
            }
            try
            {
                int i = 1;
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    OperationByPassEntity entity = new OperationByPassEntity();
                    entity.SLNO = i.ToString(); i++;
                    entity.ComponentID = sdr["componentid"].ToString();
                    entity.OperationNo = sdr["operationno"].ToString();
                    if (!sdr["EffectivedateFrom"].Equals(DBNull.Value))
                        entity.EffectiveFromDate = (Convert.ToDateTime(sdr["EffectivedateFrom"].ToString())).ToString("dd-MM-yyyy HH:mm");
                    if (!sdr["EffectivedateTo"].Equals(DBNull.Value))
                        entity.EffectiveToDate = (Convert.ToDateTime(sdr["EffectivedateTo"].ToString())).ToString("dd-MM-yyyy HH:mm");
                    DataEntityList.Add(entity);
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
            return DataEntityList;
        }


        internal static void SaveComponentOperationByPass(string componentID, string operation, DateTime effectiveFromDate, DateTime effectiveToDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if not exists(select * from OperationBypassDetails_Shanti where Componentid=@Comp and OperationNo=@OpnNo and EffectivedateFrom=@EffFromDate)
                                begin
                                insert into OperationBypassDetails_Shanti(Componentid,OperationNo,EffectivedateFrom,EffectivedateTo)
                                values(@Comp,@OpnNo,@EffFromDate,@EffToDate)
                                end
                                else
                                begin
                                update OperationBypassDetails_Shanti set EffectivedateFrom=@EffFromDate ,EffectivedateTo=@EffToDate where Componentid=@Comp and OperationNo=@OpnNo and EffectivedateFrom=@EffFromDate
                                end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Comp", componentID);
                cmd.Parameters.AddWithValue("@OpnNo", operation);
                cmd.Parameters.AddWithValue("@EffFromDate", effectiveFromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EffToDate", effectiveToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static void DeleteOperationBypass(string componentID, string operation, DateTime effectiveFromDate, DateTime effectiveToDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"if exists(select * from OperationBypassDetails_Shanti where Componentid=@Comp and OperationNo=@OpnNo and EffectivedateFrom=@EffFromDate)
                                begin
                                 Delete from OperationBypassDetails_Shanti where Componentid=@Comp and OperationNo=@OpnNo and EffectivedateFrom=@EffFromDate
                                end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Comp", componentID);
                cmd.Parameters.AddWithValue("@OpnNo", operation);
                cmd.Parameters.AddWithValue("@EffFromDate", effectiveFromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EffToDate", effectiveToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        #region -----modified data for shanti
        internal static bool UpdateModifiedSlnoData(string oldSlno, string newSlno, string machineID, DateTime sttime, DateTime ndtime, int datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                if (datatype == 3)
                {
                    cmd = new SqlCommand(@"Update AutodataRejections set  compslno= @NewSlno where compslno = @OldSlno and mc=@MachineID and   CreatedTS >= @sttime and CreatedTS <= @ndtime", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"Update autodata set  compslno= @NewSlno where compslno = @OldSlno and mc=@MachineID and  datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime", conn);
                }

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OldSlno", oldSlno);
                cmd.Parameters.AddWithValue("@NewSlno", newSlno);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updatedRow = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRow = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }
        internal static bool UpdateModifiedSupplierData(string oldSupplier, string newSupplier, string machineID, DateTime sttime, DateTime ndtime, int datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                if (datatype == 3)
                {
                    cmd = new SqlCommand(@"Update AutodataRejections set  SupplierCode= @NewSupplier where SupplierCode = @OldSupplier and mc=@MachineID and   CreatedTS >= @sttime and CreatedTS <= @ndtime", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"Update autodata set  SupplierCode= @NewSupplier where SupplierCode = @OldSupplier and mc=@MachineID and  datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime", conn);
                }
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OldSupplier", oldSupplier);
                cmd.Parameters.AddWithValue("@NewSupplier", newSupplier);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updatedRow = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRow = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }
        internal static bool UpdateModifiedSupervisorData(string oldSupervisor, string newSupervisor, string machineID, DateTime sttime, DateTime ndtime, int datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                if (datatype == 3)
                {
                    cmd = new SqlCommand(@"Update AutodataRejections set  Supervisorcode= @NewSupervisor where Supervisorcode = @OldSupervisor and mc=@MachineID and  CreatedTS >= @sttime and CreatedTS <= @ndtime", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"Update autodata set  Supervisorcode= @NewSupervisor where Supervisorcode = @OldSupervisor and mc=@MachineID and  datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime", conn);
                }

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OldSupervisor", oldSupervisor);
                cmd.Parameters.AddWithValue("@NewSupervisor", newSupervisor);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Datatype", datatype);
                cmd.Parameters.AddWithValue("@sttime", sttime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ndtime", ndtime.ToString("yyyy-MM-dd HH:mm:ss"));
                updatedRow = cmd.ExecuteNonQuery();
                Result = true;
            }
            catch (Exception ex)
            {
                updatedRow = 0;
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }
        internal static void UpdateProdDownRejectiondataForShanti(int id, DateTime StartTime, DateTime EndTime, string MacInt, string FromComponent, string ToComponent, string FromOperation, string ToOperation,
      string FromOperator, string ToOperator, string FromWorkOrder, string ToWorkOrder, Decimal FromPartsCount, Decimal ToPartsCount, string downid, string FromdownCode,
      string TodownCode, string RejectionID, string FromRejectionCode, string ToRejectionCode, string Datatype, string param, string FromSlno, string ToSlno, string FromSupervisorCode, string ToSupervisorCode, string FromSupplierCode, string ToSupplierCode, out bool IsUpdated)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            IsUpdated = false;
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"s_UpdateProdDownRejectiondata", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"StartTime", StartTime);
                cmd.Parameters.AddWithValue(@"EndTime", EndTime);
                cmd.Parameters.AddWithValue(@"FromComponent", FromComponent);
                cmd.Parameters.AddWithValue(@"ToComponent", ToComponent);
                cmd.Parameters.AddWithValue(@"FromOperation", FromOperation);
                cmd.Parameters.AddWithValue(@"ToOperation", ToOperation);
                cmd.Parameters.AddWithValue(@"Machineid", MacInt);
                cmd.Parameters.AddWithValue(@"Fromoperator", FromOperator);
                cmd.Parameters.AddWithValue(@"Tooperator", ToOperator);
                cmd.Parameters.AddWithValue(@"FromWorkOrder", FromWorkOrder);
                cmd.Parameters.AddWithValue(@"ToWorkOrder", ToWorkOrder);
                cmd.Parameters.AddWithValue(@"FromPartsCount", FromPartsCount);
                cmd.Parameters.AddWithValue(@"ToPartsCount", ToPartsCount);
                cmd.Parameters.AddWithValue(@"downid", downid);
                cmd.Parameters.AddWithValue(@"FromdownCode", FromdownCode);
                cmd.Parameters.AddWithValue(@"TodownCode", TodownCode);
                cmd.Parameters.AddWithValue(@"RejectionID", RejectionID);
                cmd.Parameters.AddWithValue(@"FromRejectionCode", FromRejectionCode);
                cmd.Parameters.AddWithValue(@"ToRejectionCode", ToRejectionCode);
                cmd.Parameters.AddWithValue(@"Datatype", Datatype);
                cmd.Parameters.AddWithValue(@"param", param);
                cmd.Parameters.AddWithValue(@"id", id);

                //kkkkkkkkkkkkkkkkkkkk
                //cmd.Parameters.AddWithValue(@"FromRejectionCode", FromSlno);
                //cmd.Parameters.AddWithValue(@"ToRejectionCode", ToSlno);
                //cmd.Parameters.AddWithValue(@"FromRejectionCode", FromSupervisorCode);
                //cmd.Parameters.AddWithValue(@"ToRejectionCode", ToSupervisorCode);
                //cmd.Parameters.AddWithValue(@"FromRejectionCode", FromSupplierCode);
                //cmd.Parameters.AddWithValue(@"ToRejectionCode", ToSupplierCode);

                int x = cmd.ExecuteNonQuery();
                IsUpdated = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region --- MaekedReowrk -----
        internal static string getCategoryForSelectedReason(string reason, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string sqlQuery = string.Empty;
            string result = "";
            try
            {
                if (param == "Rejection")
                {
                    sqlQuery = "select distinct Catagory  as result from rejectioncodeinformation where rejectionid=@reason";
                }
                else
                {
                    sqlQuery = "select distinct ReworkCatagory as result from Reworkinformation where Reworkinterfaceid=@reason";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@reason", reason);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    result = Convert.ToString(rdr["result"]);
                    break;
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
            return result;
        }
        #endregion


        #region ---------- Tool Sequence -------------


        internal static List<ToolSequanceData> getToolSequenceDetails(string machineid, string compid, string opno)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<ToolSequanceData> list = new List<ToolSequanceData>();
            ToolSequanceData data = null;
            try
            {
                cmd = new SqlCommand(@"select * from toolsequence where MachineID=@machine and ComponentID=@compid and OperationNo=@oprnum", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machine", machineid);
                cmd.Parameters.AddWithValue("@compid", compid);
                cmd.Parameters.AddWithValue("@oprnum", opno);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new ToolSequanceData();
                        data.MachineID = sdr["MachineID"].ToString();
                        data.ComponentID = sdr["ComponentID"].ToString();
                        data.OperationNumber = sdr["OperationNo"].ToString();
                        data.Sequence = sdr["SequenceNo"].ToString();
                        data.ToolNumber = sdr["ToolNo"].ToString();
                        data.IdealUsage = sdr["IdealUsage"].ToString();
                        data.Offset = sdr["Offset"].ToString();
                        data.ToolDescription = sdr["ToolDescription"].ToString();
                        data.ToolHolder = sdr["ToolHolder"].ToString();
                        data.RPM = sdr["RPM"].ToString();
                        data.Notes = sdr["Notes"].ToString();
                        data.Target = sdr["targetcount"].ToString();
                        data.DownCode = sdr["downcode"].ToString();

                        if (WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() == "1")
                        {
                            data.VulkanMSColumnsEnable = true;
                            data.ToolGPL = sdr["ToolGPL"].ToString();
                            data.DepthOfCut = sdr["DepthOfCut"].ToString();
                            data.FeedMM_Min = sdr["FeedMM_Min"].ToString();
                            data.FeedTooth = sdr["FeedTooth"].ToString();
                            data.NoOfCuttingEdges = sdr["NoOfCuttingEdge"].ToString();
                            data.NoOfCut = sdr["NoOfCut"].ToString();
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
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static int insertOrUpdateToolSequenceDetails(ToolSequanceData data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            string query = "";
            try
            {
                if (WebConfigurationManager.AppSettings["VulkanMachineShopPages"].ToString() == "1")
                {
                    cmd = new SqlCommand(@"if exists(select * from toolsequence where MachineID=@machine and ComponentID=@compid and OperationNo=@operationno and ToolNo=@toolno )
begin
	update toolsequence set IdealUsage=@idealusage,Offset=@offset,ToolDescription=@tooldesc,ToolHolder=@toolholder,RPM=@rpm,Notes=@notes,targetcount=@target,downcode=@downcode, ToolGPL=@ToolGPL, DepthOfCut=@DepthOfCut
	, FeedMM_Min=@FeedMM_Min, FeedTooth=@FeedTooth, NoOfCuttingEdge=@NoOfCuttingEdge, NoOfCut=@NoOfCut
	where  MachineID=@machine and ComponentID=@compid and OperationNo=@operationno and ToolNo=@toolno
end
else
begin
	declare @nextSequence as int
	select @nextSequence = (select CASE WHEN max(SequenceNo) IS NULL THEN 1 ELSE max(SequenceNo)+1 END AS nextSequence from toolsequence where MachineID=@machine and
	ComponentID=@compid and OperationNo=@operationno)
	insert into ToolSequence(SequenceNo, MachineID,ComponentID,OperationNo,ToolNo,IdealUsage,Offset,ToolDescription,ToolHolder,RPM,Notes,targetcount,downcode, ToolGPL, DepthOfCut,FeedMM_Min,FeedTooth,NoOfCuttingEdge,NoOfCut)
	values(@nextSequence,@machine,@compid,@operationno,@toolno,@idealusage,@offset,@tooldesc,@toolholder,@rpm,@notes,@target,@downcode,@ToolGPL,@DepthOfCut,@FeedMM_Min,@FeedTooth,@NoOfCuttingEdge,@NoOfCut)
end	", conn);
                    cmd.Parameters.AddWithValue("@ToolGPL", data.ToolGPL);
                    cmd.Parameters.AddWithValue("@DepthOfCut", data.DepthOfCut);
                    cmd.Parameters.AddWithValue("@FeedMM_Min", data.FeedMM_Min);
                    cmd.Parameters.AddWithValue("@FeedTooth", data.FeedTooth);
                    cmd.Parameters.AddWithValue("@NoOfCuttingEdge", data.NoOfCuttingEdges);
                    cmd.Parameters.AddWithValue("@NoOfCut", data.NoOfCut);
                }
                else
                    cmd = new SqlCommand(@"if exists(select * from toolsequence where MachineID=@machine and ComponentID=@compid and OperationNo=@operationno and ToolNo=@toolno )
begin
	update toolsequence set IdealUsage=@idealusage,Offset=@offset,ToolDescription=@tooldesc,ToolHolder=@toolholder,RPM=@rpm,Notes=@notes,targetcount=@target,downcode=@downcode 
	where  MachineID=@machine and ComponentID=@compid and OperationNo=@operationno and ToolNo=@toolno
end
else
begin
	declare @nextSequence as int
	select @nextSequence = (select CASE WHEN max(SequenceNo) IS NULL THEN 1 ELSE max(SequenceNo)+1 END AS nextSequence from toolsequence where MachineID=@machine and
	ComponentID=@compid and OperationNo=@operationno)
	insert into ToolSequence(SequenceNo, MachineID,ComponentID,OperationNo,ToolNo,IdealUsage,Offset,ToolDescription,ToolHolder,RPM,Notes,targetcount,downcode)
	values(@nextSequence,@machine,@compid,@operationno,@toolno,@idealusage,@offset,@tooldesc,@toolholder,@rpm,@notes,@target,@downcode)
end", conn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machine", data.MachineID);
                cmd.Parameters.AddWithValue("@compid", data.ComponentID);
                cmd.Parameters.AddWithValue("@operationno", Convert.ToInt32(data.OperationNumber.Trim()));
                cmd.Parameters.AddWithValue("@toolno", data.ToolNumber.Trim());
                cmd.Parameters.AddWithValue("@idealusage", Convert.ToInt32(data.IdealUsage.Trim() == "" ? "0" : data.IdealUsage.Trim()));
                cmd.Parameters.AddWithValue("@offset", data.Offset.Trim());
                cmd.Parameters.AddWithValue("@tooldesc", data.ToolDescription.Trim());
                cmd.Parameters.AddWithValue("@toolholder", data.ToolHolder.Trim());
                cmd.Parameters.AddWithValue("@rpm", Convert.ToInt32(data.RPM.Trim() == "" ? "0" : data.RPM.Trim()));
                cmd.Parameters.AddWithValue("@notes", data.Notes.Trim());
                cmd.Parameters.AddWithValue("@target", Convert.ToInt32(data.Target.Trim() == "" ? "0" : data.Target.Trim()));
                cmd.Parameters.AddWithValue("@downcode", data.DownCode.Trim());
                cmd.Parameters.AddWithValue("@sequence", Convert.ToInt32(data.Sequence.Trim() == "" ? "0" : data.Sequence.Trim()));
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                result = 0;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        internal static int deleteToolSequenceDetails(ToolSequanceData data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                cmd = new SqlCommand(@"delete from ToolSequence where MachineID=@machine and ComponentID=@compid and OperationNo=@operationno and  ToolNo=@toolno", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machine", data.MachineID);
                cmd.Parameters.AddWithValue("@compid", data.ComponentID);
                cmd.Parameters.AddWithValue("@operationno", Convert.ToInt32(data.OperationNumber));
                cmd.Parameters.AddWithValue("@toolno", data.ToolNumber);
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                result = 0;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }

        internal static string getMachineinterdaceid(string machineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string result = "";
            try
            {
                cmd = new SqlCommand(@"select InterfaceID from machineinformation where machineid='" + machineId + "'", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    result = rdr["InterfaceID"].ToString();
                }

            }
            catch (Exception ex)
            {
                result = "";
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return result;
        }

        #endregion

        #region ---- PDT -----------

        #region -- Holiday List ---
        internal static List<string> GetMachineIDForPlant(string PlantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allMachines = new List<string>();
            try
            {
                if (PlantId.Equals("All")) PlantId = string.Empty;
                //                string sqlQuery = @"select MachineInformation.machineid as MachineID , InterfaceID  from machineinformation inner join plantmachine on MachineInformation.MachineID=PlantMachine.MachineID 
                //                                    where (PlantID=@plantID or @plantID='') order by MachineInformation.MachineID";

                string sqlQuery = @"select distinct machineid from MachineInformation where TPMTrakEnabled = 1 order by machineid";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue("@plantID", PlantId);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allMachines.Add(Convert.ToString(rdr["MachineID"]));
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
            return allMachines;
        }

        internal static List<HolidayListDetails> getHolidayList(string value, string value2, string reason, string machine, string param)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<HolidayListDetails> listParameter = new List<HolidayListDetails>();
            HolidayListDetails parameter = null;
            try
            {
                if (param == "ByDate")
                {
                    string fromdateout, todateout;
                    fromdateout = GetPhysicalDate(value, value2, out todateout);
                    if (machine == "")
                    {
                        cmd = new SqlCommand("select * from HolidayList where Holiday>=@value and Holiday<=@toDate and (Reason=@reason or ISNULL(@reason,'')='') order by MachineID,Holiday", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("select * from HolidayList where Holiday>=@value and Holiday<=@toDate and (Reason=@reason or ISNULL(@reason,'')='') and MachineID in (" + machine + ") order by MachineID,Holiday", con);
                    }

                    cmd.Parameters.AddWithValue("@value", Util.GetDateTime(fromdateout).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@toDate", Util.GetDateTime(todateout).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@reason", reason);
                }
                else if (param == "ByReason")
                {
                    cmd = new SqlCommand("select * from HolidayList where  (Reason=@value or ISNULL(@value,'')='')  order by MachineID,Holiday", con);
                    cmd.Parameters.AddWithValue("@value", value);
                }
                else if (param == "ByMachine")
                {
                    if (value == "")
                    {
                        cmd = new SqlCommand("select * from HolidayList  order by MachineID,Holiday", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("select * from HolidayList where MachineID in (" + value + ")  order by MachineID,Holiday", con);
                    }

                }
                else
                {
                    cmd = new SqlCommand("select * from HolidayList  order by MachineID,Holiday", con);
                }

                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            parameter = new HolidayListDetails();
                            if (sdr["Holiday"] != null)
                            {
                                if (sdr["Holiday"].ToString() != "")
                                {
                                    //DateTime date = DateTime.Now.Date;
                                    //string s = sdr["Holiday"].ToString();
                                    //date = Util.GetDateTime(sdr["Holiday"].ToString());
                                    //parameter.Holiday = date.ToString("dd-MMM-yyyy");
                                    parameter.Holiday = Util.GetDateTime(sdr["Holiday"].ToString()).ToString("dd-MMM-yyyy");
                                    parameter.Date = Util.GetDateTime(sdr["Holiday"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    parameter.Holiday = sdr["Holiday"].ToString();
                                    parameter.Date = sdr["Holiday"].ToString();
                                }

                            }
                            else
                            {
                                parameter.Holiday = sdr["Holiday"].ToString();
                            }

                            parameter.Reason = sdr["Reason"].ToString();
                            parameter.MachineID = sdr["MachineID"].ToString();
                            listParameter.Add(parameter);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getHolidayList- " + ex.Message);
                        }
                    }
                }
                else
                {
                    parameter = new HolidayListDetails();
                    listParameter.Add(parameter);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listParameter;
        }
        internal static int deleteHolidayDetails(string query)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            int result = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = -2;
                Logger.WriteErrorLog("Error in deleteHolidayDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        private static string convertDateTimeToSpecificFormat(string datetime)
        {
            string formateddatetime = "";
            try
            {
                formateddatetime = Convert.ToDateTime(datetime).ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return formateddatetime;
        }
        #endregion

        internal static List<string> getAllReasons()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> listParameter = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct Reason from HolidayList", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            listParameter.Add(sdr["Reason"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getAllReasons- " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listParameter;
        }
        internal static List<PDTData> getWeeklyOffDetails(string fromDate, string toDate, string machineid, string reason, string viewType)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PDTData> list = new List<PDTData>();
            PDTData data = null;
            try
            {
                if (viewType == "ByDate")
                {
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='weeklyoff'  and StartTime>=@fromdate and EndTime<=@toDate and (DownReason=@reason or ISNULL(@reason,'')='') order by Machine,StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='weeklyoff'   and Machine in (" + machineid + ")  and StartTime>=@fromdate and EndTime<=@toDate   and (DownReason=@reason or ISNULL(@reason,'')='') order by Machine,StartTime", con);
                    }
                }
                else if (viewType == "ByReason")
                {
                    cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='weeklyoff'  and (DownReason=@reason or ISNULL(@reason,'')='') order by Machine,StartTime", con);
                }
                else if (viewType == "ByMachine")
                {
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='weeklyoff' order by Machine,StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='weeklyoff'   and Machine in (" + machineid + ") order by Machine,StartTime", con);
                    }
                }
                else
                {
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='weeklyoff'  order by Machine,StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='weeklyoff' and Machine in (" + machineid + ")  order by Machine,StartTime", con);
                    }
                }
                string fromdateout, todateout;
                fromdateout = GetPhysicalDate(fromDate, toDate, out todateout);
                cmd.Parameters.AddWithValue("@fromdate", Util.GetDateTime(fromdateout).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@toDate", Util.GetDateTime(todateout).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@reason", reason);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            data = new PDTData();

                            data.FromDateTime = Util.GetDateTime(Convert.ToDateTime(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd")).ToString("dd-MMM-yyyy");
                            // string s2=Convert.ToDateTime(s1).
                            data.ToDateTime = Util.GetDateTime(Convert.ToDateTime(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd")).ToString("dd-MMM-yyyy");
                            data.FromDateTime1 = sdr["StartTime"].ToString();
                            data.ToDateTime1 = sdr["EndTime"].ToString();
                            data.Reason = sdr["DownReason"].ToString();
                            data.Day = Util.GetDateTime(data.FromDateTime).ToString("dddd");
                            data.MachineID = sdr["Machine"].ToString();
                            data.ID = sdr["ID"].ToString();
                            list.Add(data);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getWeeklyOff details- " + ex.Message);
                        }
                    }
                }
                else
                {
                    data = new PDTData();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        public static List<PDTData> saveWeeklyOffDetails(DataTable dt, string param)
        {
            List<PDTData> pdtissuedetails = new List<PDTData>();
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            string conString = HttpContext.Current.Session["connectionString"] as string;
            conString = WebConfigurationManager.ConnectionStrings[conString].ToString();
            try
            {
                bulkCopy = new SqlBulkCopy(conString);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[TempPlannedDownTimes]";
                bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
                bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
                bulkCopy.ColumnMappings.Add("Machine", "Machine");
                bulkCopy.ColumnMappings.Add("DownReason", "DownReason");
                bulkCopy.ColumnMappings.Add("DownType", "DownType");
                bulkCopy.ColumnMappings.Add("DayName", "DayName");
                bulkCopy.ColumnMappings.Add("ShiftName", "ShiftName");
                bulkCopy.ColumnMappings.Add("ShiftStart", "ShiftStart");
                bulkCopy.ColumnMappings.Add("ShiftEnd", "ShiftEnd");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.PO_RawData .", e.RowsCopied));
                };

                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
                pdtissuedetails = getPDTIssueDetails(param);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(string.Format("Exception in ProcessAlarmFile() method. Message :{0}", ex.ToString()));
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();

            }
            return pdtissuedetails;
        }
        public static int deletePORawDataRecords()
        {
            int recordAffected = 0;
            string cmdStr = System.String.Format("delete from TempPlannedDownTimes");
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand command = new SqlCommand(cmdStr, sqlConn);
            command.CommandType = System.Data.CommandType.Text;
            try
            {
                recordAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }
            return recordAffected;
        }
        public static List<PDTData> getPDTIssueDetails(string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<PDTData> list = new List<PDTData>();
            PDTData data = null;
            SqlCommand command = new SqlCommand("[dbo].[s_GetDownReasons]", sqlConn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Param", param);
            command.CommandTimeout = 360;
            try
            {
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new PDTData();
                        data.FromDateTime = sdr["StartTime"].ToString();
                        data.ToDateTime = sdr["EndTime"].ToString();
                        data.MachineID = sdr["Machine"].ToString();
                        data.Reason = sdr["DownReason"].ToString();
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
                if (sqlConn != null)
                    sqlConn.Close();
            }
            return list;
        }
        internal static int deleteWeeklyOffDetails(string id)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            int result = 0;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"delete from PlannedDownTimes where ID=@id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = -2;
                Logger.WriteErrorLog("Error in deleteHolidayDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }


        #region ----- Shift Down---------------
        internal static List<PDTData> getShiftDownDetails(string fromDate, string toDate, string machineid, string selectedDays, string selectedShift, string reason, string viewType)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PDTData> list = new List<PDTData>();
            PDTData data = null;
            bool isFromFilter = false;
            try
            {
                if (viewType == "ByDate")
                {
                    isFromFilter = true;
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select p.StartTime,p.EndTime,p.DownReason,p.Machine,s.ShiftName,p.ID from PlannedDownTimes as p  join shiftdetails as s on (CAST(p.Starttime as time) = CAST(s.FromTime as time) 
                    and CAST(p.EndTime as time) = CAST(s.ToTime as time)) where s.ShiftName in (" + selectedShift + ") and p.DownType='shift' and s.Running=1  and p.StartTime>=@fromdate and p.StartTime<=@toDate and (p.DownReason = @reason or ISNULL(@reason, '') = '')  order by p.Machine,p.StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select p.StartTime,p.EndTime,p.DownReason,p.Machine,s.ShiftName,p.ID from PlannedDownTimes as p  join shiftdetails as s on (CAST(p.Starttime as time) = CAST(s.FromTime as time) 
                    and CAST(p.EndTime as time) = CAST(s.ToTime as time)) where s.ShiftName in (" + selectedShift + ") and p.DownType='shift' and s.Running=1 and p.Machine in (" + machineid + ")  and  p.StartTime >= @fromdate and p.StartTime <= @toDate and (p.DownReason = @reason or ISNULL(@reason, '') = '')   order by p.Machine, p.StartTime", con);
                    }
                }
                else if (viewType == "ByReason")
                {
                    // isFromFilter = true;
                    cmd = new SqlCommand(@"select p.StartTime,p.EndTime,p.DownReason,p.Machine,s.ShiftName,p.ID from PlannedDownTimes as p  join shiftdetails as s on (CAST(p.Starttime as time) = CAST(s.FromTime as time) 
                    and CAST(p.EndTime as time) = CAST(s.ToTime as time)) where  p.DownType='shift' and s.Running=1   and (p.DownReason = @reason or ISNULL(@reason, '') = '')  order by p.Machine,p.StartTime", con);
                }
                else if (viewType == "ByMachine")
                {
                    // isFromFilter = true;
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select p.StartTime,p.EndTime,p.DownReason,p.Machine,s.ShiftName,p.ID from PlannedDownTimes as p  join shiftdetails as s on (CAST(p.Starttime as time) = CAST(s.FromTime as time) 
and CAST(p.EndTime as time) = CAST(s.ToTime as time)) where  p.DownType='shift' and s.Running=1   order by p.Machine,p.StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select p.StartTime,p.EndTime,p.DownReason,p.Machine,s.ShiftName,p.ID from PlannedDownTimes as p  join shiftdetails as s on (CAST(p.Starttime as time) = CAST(s.FromTime as time) 
and CAST(p.EndTime as time) = CAST(s.ToTime as time)) where  p.DownType='shift' and s.Running=1 
and p.Machine in (" + machineid + ")   order by p.Machine,p.StartTime", con);
                    }
                }
                else
                {
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select p.StartTime,p.EndTime,p.DownReason,p.Machine,s.ShiftName,p.ID from PlannedDownTimes as p  join shiftdetails as s on (CAST(p.Starttime as time) = CAST(s.FromTime as time) 
and CAST(p.EndTime as time) = CAST(s.ToTime as time)) where  p.DownType='shift' and s.Running=1   order by p.Machine,p.StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select p.StartTime,p.EndTime,p.DownReason,p.Machine,s.ShiftName,p.ID from PlannedDownTimes as p  join shiftdetails as s on (CAST(p.Starttime as time) = CAST(s.FromTime as time) 
and CAST(p.EndTime as time) = CAST(s.ToTime as time)) where  p.DownType='shift' and s.Running=1 
and p.Machine in (" + machineid + ")   order by p.Machine,p.StartTime", con);
                    }
                }
                string fromdateout, todateout;
                fromdateout = GetPhysicalDate(fromDate, toDate, out todateout);
                cmd.Parameters.AddWithValue("@fromdate", Util.GetDateTime(fromdateout).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@toDate", Util.GetDateTime(todateout).ToString("yyyy-MM-dd HH:mm:ss"));
                // cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@reason", reason);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            data = new PDTData();
                            data.FromDateTime = Util.GetDateTime(Convert.ToDateTime(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd")).ToString("dd-MMM-yyyy");
                            data.ToDateTime = sdr["EndTime"].ToString();
                            data.Reason = sdr["DownReason"].ToString();
                            data.MachineID = sdr["Machine"].ToString();
                            data.ShiftName = sdr["ShiftName"].ToString();
                            data.ID = sdr["ID"].ToString();
                            if (isFromFilter)
                            {
                                string days = Util.GetDateTime(data.FromDateTime).ToString("ddd").ToLower();
                                data.Day = days;
                                if (selectedDays.IndexOf(days, StringComparison.OrdinalIgnoreCase) < 0)
                                {
                                    continue;
                                }

                            }
                            list.Add(data);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getWeeklyOff details- " + ex.Message);
                        }
                    }
                }
                else
                {
                    data = new PDTData();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static List<PDTData> getShiftTimeDetails(DateTime date)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PDTData> list = new List<PDTData>();
            PDTData data = null;
            try
            {

                cmd = new SqlCommand(@"[dbo].[s_GetCurrentShiftTime]", con);
                cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "ALL Shifts");
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            data = new PDTData();
                            data.FromDateTime = Util.GetDateTime(sdr["Starttime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            data.ToDateTime = Util.GetDateTime(sdr["Endtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            data.ShiftName = sdr["shiftname"].ToString();
                            list.Add(data);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getWeeklyOff details- " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        public static List<PDTData> saveShiftDownDetails(DataTable dt, string param)
        {
            List<PDTData> pdtissuedetails = new List<PDTData>();
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            string conString = HttpContext.Current.Session["connectionString"] as string;
            conString = WebConfigurationManager.ConnectionStrings[conString].ToString();
            try
            {
                bulkCopy = new SqlBulkCopy(conString);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[TempPlannedDownTimes]";
                bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
                bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
                bulkCopy.ColumnMappings.Add("Machine", "Machine");
                bulkCopy.ColumnMappings.Add("DownReason", "DownReason");
                bulkCopy.ColumnMappings.Add("DownType", "DownType");
                bulkCopy.ColumnMappings.Add("DayName", "DayName");
                bulkCopy.ColumnMappings.Add("ShiftName", "ShiftName");
                bulkCopy.ColumnMappings.Add("ShiftStart", "ShiftStart");
                bulkCopy.ColumnMappings.Add("ShiftEnd", "ShiftEnd");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.PO_RawData .", e.RowsCopied));
                };

                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
                pdtissuedetails = getPDTIssueDetails(param);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(string.Format("Exception in ProcessAlarmFile() method. Message :{0}", ex.ToString()));
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();

            }
            return pdtissuedetails;
        }

        internal static int deleteShiftDownDetails(string id, string machine, bool isHolidayData)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            int result = 0;
            SqlCommand cmd = null;
            try
            {
                if (isHolidayData)
                {
                    cmd = new SqlCommand(@"delete from HolidayList where Holiday in (" + id + ") and MachineID='" + machine + "'; delete from PlannedDownTimes where  StartTime in (" + id + ") and Machine='" + machine + "' and DownType='Holiday'", con);
                }
                else
                {
                    cmd = new SqlCommand(@"delete from PlannedDownTimes  where ID in (" + id + ")", con);
                }
                cmd.CommandType = CommandType.Text;
                // cmd.Parameters.AddWithValue("@id",Convert.ToInt64(id));
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = -2;
                Logger.WriteErrorLog("Error in deleteHolidayDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        #endregion

        #region ------ All Downs ------
        internal static List<PDTData> getAllDownDetails(DateTime fromDate, DateTime toDate, string machineid, string downtype)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PDTData> list = new List<PDTData>();
            PDTData data = null;
            try
            {
                if (downtype == "Holidays")
                {
                    cmd = new SqlCommand(@"select * from HolidayList where Holiday>=@fromdate and Holiday<=@toDate  and MachineID=@machineid order by MachineID,Holiday", con);
                }
                else
                {
                    cmd = new SqlCommand(@"select * from PlannedDownTimes where Machine=@machineid and StartTime>=@fromdate and StartTime<=@toDate and DownType=@downtype  order by Machine,StartTime", con);
                }
                cmd.Parameters.AddWithValue("@fromdate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@downtype", downtype);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            data = new PDTData();
                            if (downtype == "Holidays")
                            {

                                data.FromDateTime = Util.GetDateTime(Convert.ToDateTime(sdr["Holiday"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToString("dd-MMM-yyyy HH:mm:ss");
                                data.Reason = sdr["Reason"].ToString();
                                data.MachineID = sdr["MachineID"].ToString();
                                data.DownType = "Holidays";
                                data.ID = sdr["Holiday"].ToString();
                            }
                            else
                            {
                                data.FromDateTime = Util.GetDateTime(Convert.ToDateTime(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToString("dd-MMM-yyyy HH:mm:ss") + " - " + Util.GetDateTime(Convert.ToDateTime(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToString("dd-MMM-yyyy HH:mm:ss");
                                data.ToDateTime = sdr["EndTime"].ToString();
                                data.Reason = sdr["DownReason"].ToString();
                                data.MachineID = sdr["Machine"].ToString();
                                data.DownType = sdr["DownType"].ToString();
                                data.ID = sdr["ID"].ToString();
                            }
                            list.Add(data);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getWeeklyOff details- " + ex.Message);
                        }
                    }
                }
                else
                {
                    data = new PDTData();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getAuditDateDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        #endregion

        #region ------ Daily Down -----

        internal static List<PDTData> getDailyDownDetails(string fromDate, string toDate, string downreason, string machineid, string viewType)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PDTData> list = new List<PDTData>();
            PDTData data = null;
            try
            {
                if (viewType == "ByDate")
                {
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='DailyDown' and StartTime>=@fromdate and EndTime<=@todate  and    (DownReason=@downReason or ISNULL(@downReason,'')='')  order by Machine,StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='DailyDown' and StartTime>=@fromdate and EndTime<=@todate and Machine in (" + machineid + ") and (DownReason=@downReason or ISNULL(@downReason,'')='')  order by Machine,StartTime", con);
                    }
                }
                else if (viewType == "ByReason")
                {
                    cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='DailyDown' and  (DownReason=@downReason or ISNULL(@downReason,'')='')  order by Machine,StartTime", con);
                }
                else
                {
                    if (machineid.Trim() == "")
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='DailyDown'  order by Machine,StartTime", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"select * from PlannedDownTimes where DownType='DailyDown' and Machine in (" + machineid + ")  order by Machine,StartTime", con);
                    }
                }
                string fromdateout, todateout;
                fromdateout = GetPhysicalDate(fromDate, toDate, out todateout);
                cmd.Parameters.AddWithValue("@fromdate", Util.GetDateTime(fromdateout).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@todate", Util.GetDateTime(todateout).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@downReason", downreason);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        try
                        {
                            data = new PDTData();
                            data.FromDateTime = Util.GetDateTime(Convert.ToDateTime(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToString("dd-MMM-yyyy HH:mm:ss");
                            data.ToDateTime = Util.GetDateTime(Convert.ToDateTime(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")).ToString("dd-MMM-yyyy HH:mm:ss");
                            data.Reason = sdr["DownReason"].ToString();
                            data.MachineID = sdr["Machine"].ToString();
                            data.ID = sdr["ID"].ToString();
                            list.Add(data);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error in getDailyDownDetails details- " + ex.Message);
                        }
                    }
                }
                else
                {
                    data = new PDTData();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getDailyDownDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static List<string> getDailyDownReason(string param)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct DownReason from PlannedDownTimes where DownType=@param", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["DownReason"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in getDailyDownDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static string GetPhysicalDate(string fromdateinput, string todateinput, out string toDate)
        {
            DateTime startdate;
            startdate = Util.GetDateTime(fromdateinput.Trim());
            TimeSpan time = new TimeSpan(0, 0, 0);
            DateTime combinedStartDate = startdate.Add(time);
            string fromDate = combinedStartDate.ToString("yyyy-MM-dd HH:mm:ss");
            // DateTime enddate = Convert.ToDateTime(txtToDate.Text);
            DateTime enddate;
            enddate = Util.GetDateTime(todateinput.Trim());
            DateTime combinedEndDate = enddate.AddDays(1).AddTicks(-1);
            toDate = combinedEndDate.ToString("yyyy-MM-dd HH:mm:ss");
            return fromDate;
        }
        #endregion
        #endregion


        internal static FPAData FPAGetSerialNumber(string ComponentID, string SerialNumber)
        {
            string appPath = HttpContext.Current.Server.MapPath("~/ShantiIron/FPA_FIles");
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            FPAData data = new FPAData();
            SqlDataReader rdr = null;
            string query = @"select * from Shanthi_FPA_Transcation where SerialNumber=@SerialNumber and PartNumber=@PartNumber";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                cmd.Parameters.AddWithValue("@PartNumber", ComponentID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data.PartName = rdr["PartNumber"].ToString();
                        data.SerialNumber = rdr["SerialNumber"].ToString();
                        if (System.IO.File.Exists(System.IO.Path.Combine(appPath, "FPA", "UploadedTemplate", rdr["FPAFileName"].ToString())))
                        {
                            data.FPAFile = System.IO.Path.Combine(appPath, "FPA", "UploadedTemplate", (data.SerialNumber + "_" + data.PartName + "_FPA.xlsx"));
                            data.FPAName = data.SerialNumber + "_" + data.PartName + "_FPA.xlsx";
                        }
                        else
                        {
                            data.FPAFile = System.IO.Path.Combine(appPath, "FPA", "Template", "FPATemplate.xlsx");
                            data.FPAName = "Template.xlsx";
                        }
                        if (System.IO.File.Exists(System.IO.Path.Combine(appPath, "Layout", "UploadedTemplate", rdr["LayoutFileName"].ToString())))
                        {
                            data.LayoutFile = System.IO.Path.Combine(appPath, "Layout", "UploadedTemplate", (data.SerialNumber + "_" + data.PartName + "_Layout.xlsx"));
                            data.LayoutName = data.SerialNumber + "_" + data.PartName + "_Layout.xlsx";
                        }
                        else
                        {
                            data.LayoutFile = System.IO.Path.Combine(appPath, "Layout", "Template", "LayoutTemplate.xlsx");
                            data.LayoutName = "Template.xlsx";
                        }
                        if (DBNull.Value.ToString().Equals(rdr["CMMStatus"].ToString()))
                        {
                            data.CMMName = "";
                            data.CMMFile = "";
                            data.CMMStatus = "";
                        }
                        else if (rdr["CMMStatus"].ToString().Equals("CMMSent"))
                        {
                            data.CMMName = rdr["CMMFileName"].ToString();
                            data.CMMFile = System.IO.Path.Combine(appPath, "CMM", rdr["CMMFileName"].ToString()); ;
                            data.CMMStatus = "CMMClicked";
                        }
                        data.CMMStatus = rdr["CMMStatus"].ToString();
                        data.SerialStatus = (string.IsNullOrEmpty(rdr["SerialNumberStatus"].ToString())) ? "New" : rdr["SerialNumberStatus"].ToString();
                    }
                }
                else
                {
                    data.PartName = ComponentID;
                    data.SerialNumber = SerialNumber;
                    data.FPAFile = System.IO.Path.Combine(appPath, "FPA", "Template", "FPATemplate.xlsx");
                    data.FPAName = "Template.xlsx";
                    data.LayoutFile = System.IO.Path.Combine(appPath, "Layout", "Template", "LayoutTemplate.xlsx");
                    data.LayoutName = "Template.xlsx";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return data;
        }
        internal static FPAData FPAGetSerialNumber()
        {
            string appPath = HttpContext.Current.Server.MapPath("~/ShantiIron/FPA_FIles");
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            FPAData data = new FPAData();
            SqlDataReader rdr = null;
            string query = @"select SBSD.PartNumber,SBSD.PartSrlNo,SBSD.RevNo,SFT.FPAFileName,SFT.LayoutFileName,SFT.CMMFileName,SFT.CMMStatus,SFT.SerialNumberStatus from Shanti_BarCodeScanDetail SBSD
                        Left outer join Shanthi_FPA_Transcation SFT on SBSD.PartSrlNo=SFT.SerialNumber
                        where SBSD.Status=0 and SBSD.ProcessName='FPA'
                        declare @SerialNumber nvarchar(50)
                        declare @PartNumber nvarchar(50)
                        select @PartNumber = SBSD.PartNumber,@SerialNumber = SBSD.PartSrlNo from Shanti_BarCodeScanDetail SBSD
                        Left outer join Shanthi_FPA_Transcation SFT on SBSD.PartSrlNo=SFT.SerialNumber 
                        if exists(select SBSD.PartNumber,SBSD.PartSrlNo,SFT.FPAFileName,SFT.LayoutFileName,SFT.CMMFileName,SFT.CMMStatus,SFT.SerialNumberStatus from Shanti_BarCodeScanDetail SBSD
                        Left outer join Shanthi_FPA_Transcation SFT on SBSD.PartSrlNo=SFT.SerialNumber
                        where SBSD.Status=0 and SBSD.ProcessName='FPA')
                        update Shanti_BarCodeScanDetail set Status=1 where PartSrlNo =@SerialNumber and PartNumber=@PartNumber";
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data.PartName = rdr["PartNumber"].ToString() + "-" + rdr["RevNo"].ToString();
                        data.SerialNumber = rdr["PartSrlNo"].ToString();
                        if (System.IO.File.Exists(System.IO.Path.Combine(appPath, "FPA", "UploadedTemplate", rdr["FPAFileName"].ToString())))
                        {
                            data.FPAFile = System.IO.Path.Combine(appPath, "FPA", "UploadedTemplate", (data.SerialNumber + "_" + data.PartName + "_FPA.xlsx"));
                            data.FPAName = data.SerialNumber + "_" + data.PartName + "_FPA.xlsx";
                        }
                        else
                        {
                            data.FPAFile = System.IO.Path.Combine(appPath, "FPA", "Template", "FPATemplate.xlsx");
                            data.FPAName = "Template.xlsx";
                        }
                        if (System.IO.File.Exists(System.IO.Path.Combine(appPath, "Layout", "UploadedTemplate", rdr["LayoutFileName"].ToString())))
                        {
                            data.LayoutFile = System.IO.Path.Combine(appPath, "Layout", "UploadedTemplate", (data.SerialNumber + "_" + data.PartName + "_Layout.xlsx"));
                            data.LayoutName = data.SerialNumber + "_" + data.PartName + "_Layout.xlsx";
                        }
                        else
                        {
                            data.LayoutFile = System.IO.Path.Combine(appPath, "Layout", "Template", "LayoutTemplate.xlsx");
                            data.LayoutName = "Template.xlsx";
                        }
                        if (DBNull.Value.ToString().Equals(rdr["CMMStatus"].ToString()))
                        {
                            data.CMMName = "";
                            data.CMMFile = "";
                            data.CMMStatus = "";
                        }
                        else if (rdr["CMMStatus"].ToString().Equals("CMMClicked"))
                        {
                            data.CMMName = rdr["CMMFileName"].ToString();
                            data.CMMFile = System.IO.Path.Combine(appPath, "CMM", rdr["CMMFileName"].ToString()); ;
                            data.CMMStatus = "CMMClicked";
                        }
                        data.CMMStatus = rdr["CMMStatus"].ToString();
                        data.SerialStatus = (string.IsNullOrEmpty(rdr["SerialNumberStatus"].ToString())) ? "New" : rdr["SerialNumberStatus"].ToString();
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
            }
            return data;
        }
        internal static void SaveFPAChanges(string PartName, string SerialNumber, string FPALink, string LayoutLink, string CMMFileName, string CMMStatus, string ButtonStatus)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists( select * from Shanthi_FPA_Transcation where PartNumber=@PartNumber and SerialNumber=@SerialNumber)
                                            begin
                                                insert into Shanthi_FPA_Transcation(PartNumber, SerialNumber, FPAFileName, LayoutFileName, CMMFileName, CMMStatus, SerialNumberStatus)
                                                values(@PartNumber, @SerialNumber, @FPAFileName, @LayoutFileName, @CMMFileName, @CMMStatus, @SerialNumberStatus)
                                            end
                                        else
                                            begin
                                                update Shanthi_FPA_Transcation set FPAFileName = @FPAFileName, LayoutFileName = @LayoutFileName,CMMFileName = @CMMFileName where PartNumber = @PartNumber and SerialNumber = @SerialNumber
                                            end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PartNumber", PartName);
                cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                cmd.Parameters.AddWithValue("@FPAFileName", FPALink);
                cmd.Parameters.AddWithValue("@LayoutFileName", LayoutLink);
                cmd.Parameters.AddWithValue("@CMMFileName", CMMFileName);
                cmd.Parameters.AddWithValue("@CMMStatus", CMMStatus);
                cmd.Parameters.AddWithValue("@SerialNumberStatus", ButtonStatus);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static void SaveFPAData(string SerialNumber, string PartName, string Status)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            switch (Status)
            {

                case "CMMSent":
                    query = @"if exists( select * from Shanthi_FPA_Transcation where PartNumber=@PartNumber and SerialNumber=@SerialNumber)
begin
update Shanthi_FPA_Transcation set CMMStatus = @Value where PartNumber=@PartNumber and SerialNumber=@SerialNumber
end";
                    break;
                default:
                    query = @"if exists( select * from Shanthi_FPA_Transcation where PartNumber=@PartNumber and SerialNumber=@SerialNumber)
begin
update Shanthi_FPA_Transcation set SerialNumberStatus = @Value where PartNumber=@PartNumber and SerialNumber=@SerialNumber
end";
                    break;
            }
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PartNumber", PartName);
                cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                cmd.Parameters.AddWithValue("@Value", Status);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static DataTable GetNipponIndiaAndon(string strDate, string endDate, string cellID, out DataTable dtProductionCount)
        {
            dtProductionCount = new DataTable();
            DataTable dtMain = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_Get_ShiftWiseAndon_IndiaNippon]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", strDate);
                cmd.Parameters.AddWithValue("@EndTime", endDate);
                cmd.Parameters.AddWithValue("@GroupID", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellID);
                cmd.Parameters.AddWithValue("@Param", "MachineWise");
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                dtMain.Load(rdr);
                dtMain.AcceptChanges();
                dtProductionCount.Load(rdr);
                dtMain.AcceptChanges();
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
            return dtMain;
        }


        internal static string GetCockitSettings()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string type = "";
            try
            {
                cmd = new SqlCommand(@"select ValueInText from cockpitdefaults where parameter='TimeFormat'", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    type = rdr["ValueInText"].ToString();
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
            return type;
        }
        #region ------- PM Master-------
        internal static List<ListItem> getFrequencyForActivityMasters()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<ListItem> listData = new List<ListItem>();
            ListItem data = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from ActivityFreq_MGTL where IsEnabled=1 order by SortOrder", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new ListItem();
                        data.Value = sdr["FreqID"].ToString();
                        data.Text = sdr["Frequency"].ToString();
                        listData.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While getting Activity Master Frequency Data " + ex.Message);

            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return listData;
        }
        internal static List<ListItem> getFrequencyValueForActivityMasters()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<ListItem> listData = new List<ListItem>();
            ListItem data = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from ActivityFreq_MGTL where IsEnabled=1 order by SortOrder", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new ListItem();
                        data.Value = sdr["Freqvalue"].ToString();
                        data.Text = sdr["Frequency"].ToString();
                        listData.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("While getting Activity Master Frequency Data " + ex.Message);

            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return listData;
        }
        internal static List<ActivityInfoEntity> GetAllActivityForGrid(string Frequency, string MachineId)
        {
            List<ActivityInfoEntity> activityInfoList = new List<ActivityInfoEntity>();
            ActivityInfoEntity activityInfoData = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                //SqlCommand cmd = new SqlCommand(@"select row_number() OVER (ORDER BY main.ActivityID) AS SlNo, main.ActivityID,main.Activity,chield.Frequency,chield.FreqID, main.Filename,  main.MachineID  from ActivityMaster_MGTL main inner join ActivityFreq_MGTL chield on main.FreqID=chield.FreqID  where (chield.Frequency=@Frequency or @Frequency='') and main.MachineID=@MachineID", sqlConn);
                SqlCommand cmd = new SqlCommand(@"select row_number() OVER (ORDER BY main.ActivityID) AS SlNo, main.ActivityID,main.Activity,chield.Frequency,chield.FreqID, main.Filename,  main.MachineID,main.ShiftID,main.Criteria,main.Category  
from ActivityMaster_MGTL main inner join ActivityFreq_MGTL chield on main.FreqID=chield.FreqID  where (chield.Frequency=@Frequency or @Frequency='') 
and main.MachineID=@MachineID", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@Frequency", Frequency == "All" ? "" : Frequency));
                cmd.Parameters.Add(new SqlParameter("@MachineID", MachineId));
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        activityInfoData = new ActivityInfoEntity();
                        activityInfoData.SerialNumber = rdr["SlNo"].ToString();
                        activityInfoData.MachineID = rdr["MachineID"].ToString();
                        activityInfoData.ActivityID = rdr["ActivityID"].ToString();
                        activityInfoData.Activity = rdr["Activity"].ToString();
                        activityInfoData.Frequency = rdr["Frequency"].ToString();
                        activityInfoData.FrequencyID = rdr["FreqID"].ToString();
                        activityInfoData.FileName = rdr["Filename"].ToString();
                        activityInfoData.ActivityHasFile = DataBaseAccess.IsActivityFileAvailableInDB(Convert.ToInt32(activityInfoData.ActivityID), MachineId);
                        activityInfoData.Shifts = rdr["ShiftID"].ToString();
                        activityInfoData.Criteria = rdr["Criteria"].ToString();
                        activityInfoData.Category = rdr["Category"].ToString();
                        activityInfoList.Add(activityInfoData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return activityInfoList;
        }
        internal static bool IsActivityFileAvailableInDB(int activityID, string machineID)
        {
            bool isFileAvailable = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                string query = @"SELECT * FROM ActivityMaster_MGTL WHERE ActivityID = @ActivityID and MachineID=@MachineID and TRIM(Filename)<>'' and TRIM(ActivityFile)<>''";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityID));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machineID));
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    isFileAvailable = true;
                }
                else
                {
                    isFileAvailable = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("IsActivityFileAvailableInDB: " + ex.Message);
                isFileAvailable = false;
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return isFileAvailable;
        }
        internal static void UpdateActivityInfoDetails(int activityID, string activity, string frequency, string filename, byte[] contents, bool hasFile, string machineID, string criteria, string shift, string category, out bool isUpdated)
        {
            isUpdated = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = "";
                if (hasFile)
                {
                    query = @"IF EXISTS(Select * from ActivityMaster_MGTL where ActivityID = @ActivityID and MachineID=@MachineID)
    BEGIN
	    UPDATE ActivityMaster_MGTL set [Activity] = @Activity, [FreqID] = @FreqID,[Filename] = @Filename, [ActivityFile] = @ActivityFile, ShiftID= @ShiftID, Criteria=@Criteria,Category=@Category 
	    where ActivityID = @ActivityID and MachineID=@MachineID
    END
    ELSE
    BEGIN
	     Insert into ActivityMaster_MGTL([Activity], [FreqID],[Filename],[ActivityFile], [MachineID],ShiftID,Criteria,Category) 
	     Values(@Activity, @FreqID,@Filename,@ActivityFile,@MachineID,@ShiftID,@Criteria,@Category) 
    END";
                }
                else
                {
                    query = @"IF EXISTS(Select * from ActivityMaster_MGTL where ActivityID = @ActivityID and MachineID=@MachineID)
							       BEGIN
							       UPDATE ActivityMaster_MGTL set [Activity] = @Activity, [FreqID] = @FreqID,ShiftID=@ShiftID,Criteria=@Criteria,Category=@Category where ActivityID = @ActivityID and MachineID=@MachineID
							       END
							       ELSE
							       BEGIN
							       Insert into ActivityMaster_MGTL([Activity], [FreqID], [MachineID],ShiftID,Criteria,Category) Values(@Activity, @FreqID,@MachineID,@ShiftID,@Criteria,@Category) 
							      END";
                }

                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@MachineID", machineID));
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityID));
                cmd.Parameters.Add(new SqlParameter("@Activity", activity));
                cmd.Parameters.Add(new SqlParameter("@FreqID", Convert.ToInt32(frequency)));
                cmd.Parameters.Add(new SqlParameter("@Filename", filename));
                if (hasFile)
                {
                    cmd.Parameters.Add(new SqlParameter("@ActivityFile", contents));
                }
                cmd.Parameters.Add(new SqlParameter("@ShiftID", shift));
                cmd.Parameters.Add(new SqlParameter("@Criteria", criteria));
                cmd.Parameters.Add(new SqlParameter("@Category", category));
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    isUpdated = true;
                else
                    isUpdated = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                isUpdated = false;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static bool SaveActivityFileToDB(byte[] fileContents, int activityID, string filename)
        {
            bool success = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = @"UPDATE ActivityMaster_MGTL set [Filename] = @Filename, [ActivityFile] = @ActivityFile where ActivityID = @ActivityID";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityID));
                cmd.Parameters.Add(new SqlParameter("@Filename", filename));
                cmd.Parameters.Add(new SqlParameter("@ActivityFile", fileContents));
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    success = true;
                else
                    success = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                success = false;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return success;
        }
        internal static void DeleteActivityInfoData(int activityID, string machineID, out bool isDeleted)
        {
            isDeleted = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"Delete from [dbo].[ActivityMaster_MGTL] where ActivityID = @ActivityID and MachineID=@MachineID";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ActivityID", activityID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                isDeleted = false;
                Logger.WriteErrorLog("Error in Deleting Grid Data From [dbo].[ActivityMaster_MGTL] - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        internal static byte[] GetActivityFilePathFromDB(int activityId, string machineID)
        {
            //string filePath = string.Empty;
            byte[] filePath = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                //  filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\";
                string query = @"SELECT Filename, ActivityFile FROM ActivityMaster_MGTL WHERE ActivityID = @ActivityID and MachineID=@MachineID";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityId));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machineID));
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        //filePath += (string)rdr.GetValue(0);
                        //byte[] fileData = (byte[])rdr.GetValue(1);
                        //using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                        //{
                        //    using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                        //    {
                        //        bw.Write(fileData);
                        //        bw.Close();
                        //    }
                        //}
                        filePath = (byte[])rdr.GetValue(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return filePath;
        }
        #endregion

        #region ---------PM Time Generation-------------

        internal static DataTable GetActivityData(string freq, string year, string from, string to, string machineId)
        {
            DataTable dt = new DataTable();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[S_GetActivityMasterYearlyData_MGTL]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Frequency", freq);
                //cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@SDate", Util.GetDateTime(from).ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@NewDate", Util.GetDateTime(to).ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@Param", "View");

                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
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
            return dt;
        }
        internal static bool GenerateActivityData(string freq, string year, string startTime, string machineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool updated = false;

            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[S_GetActivityMasterYearlyData_MGTL]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Frequency", freq);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@SDate", Util.GetDateTime(startTime).ToString("yyyy-MM-dd HH:mm"));
                cmd.CommandTimeout = 120;
                int ret = cmd.ExecuteNonQuery();
                if (ret > 0) updated = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GenerateActivityData: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return updated;
        }
        internal static bool UpdateActivityData(string freq, string year, DateTime oldDate, DateTime newDate, string activity, string machineId, string shiftID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool updated = false;

            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[S_GetActivityMasterYearlyData_MGTL]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Frequency", freq);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Activity", activity);
                cmd.Parameters.AddWithValue("@SDate", oldDate);
                cmd.Parameters.AddWithValue("@NewDate", newDate);
                cmd.Parameters.AddWithValue("@ShiftID", shiftID);
                cmd.Parameters.AddWithValue("@Param", "Update");
                cmd.CommandTimeout = 120;
                int ret = cmd.ExecuteNonQuery();
                if (ret > 0) updated = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateActivityData: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return updated;
        }

        #endregion

        #region  ------ PM Transaction -------------
        internal static List<string> GetMachineInfoForPM()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> macdata = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct MachineID from machineinformation  where  TPMTrakEnabled=1 or DNCTransferEnabled=1  order by MachineID", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        macdata.Add(rdr["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return macdata;
        }
        internal static DataTable getActivityTransactionData(string machineid, string frequency, string datetime, string param, string frequencyValue)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[s_ViewActivityInfo_MGTL]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@frequency", frequency);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@Starttime", Util.GetDateTime(datetime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Screen", param);
                cmd.Parameters.AddWithValue("@FrequencyValue", frequencyValue);
                cmd.Parameters.AddWithValue("@View", "");
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("while getting Activity Transaction details " + ex.Message);

            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }
        internal static bool getActvityTransactionLoginAuthorization(string username, string password)
        {
            bool IsAuthorized = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                string query = "";
                string groupid = "";
                query = "select * from employeeinformation where  Employeeid=@employee and upassword= @password ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@employee", username);
                cmd.Parameters.AddWithValue("@password", password);
                sdr = cmd.ExecuteReader();
                IsAuthorized = sdr.HasRows;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getActvityTransactionLoginAuthorization " + ex.Message);

            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return IsAuthorized;
        }
        internal static bool saveActvityTransactionDetails(string activityID, string currentFreq, string activityTS, string activityDoneTS, object selectedMachinem)
        {
            bool isUpdated = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = @"IF EXISTS(Select * from ActivityTransaction_MGTL where ActivityTS = @ActivityTS and ActivityID = @ActivityID and Machineid=@Machineid)
							   BEGIN
							   select * from ActivityTransaction_MGTL where ActivityTS = @ActivityTS
							   END
							   ELSE
							   BEGIN
							   Insert into ActivityTransaction_MGTL([ActivityID], [Frequency], [ActivityTS], [ActivityDoneTS], [Machineid]) Values(@ActivityID, @Frequency, @ActivityTS, @ActivityDoneTS, @Machineid) 
							  END";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityID));
                cmd.Parameters.Add(new SqlParameter("@Frequency", currentFreq));
                cmd.Parameters.Add(new SqlParameter("@ActivityTS", activityTS));
                cmd.Parameters.Add(new SqlParameter("@ActivityDoneTS", activityDoneTS));
                cmd.Parameters.Add(new SqlParameter("@Machineid", selectedMachinem));
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    isUpdated = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                isUpdated = false;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return isUpdated;
        }
        internal static DataTable GetPreventiveMaintenanceExportData(string machineId, string frequency, string startTime, string endTime, int freqValue)
        {
            DataTable dtPrevMaintenance = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("[s_ViewActivityInfo_MGTL]", sqlConn);
            try
            {
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@machineid", SqlDbType.NVarChar).Value = machineId;
                cmd.Parameters.AddWithValue("@frequency", frequency);
                cmd.Parameters.Add("@Starttime", SqlDbType.DateTime).Value = startTime;
                cmd.Parameters.Add("@Endtime", SqlDbType.DateTime).Value = endTime;
                cmd.Parameters.AddWithValue("@screen", "Current");
                cmd.Parameters.AddWithValue("@FrequencyValue", freqValue);
                cmd.Parameters.AddWithValue("@view", "Report");
                SqlDataReader sdr = cmd.ExecuteReader();
                dtPrevMaintenance.Load(sdr);
                dtPrevMaintenance.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error - \n " + ex.ToString());
            }
            finally
            {
                //if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return dtPrevMaintenance;
        }
        //internal static DataTable GetPreventiveMaintenanceExportData(string machineId, string frequency, string startTime, string endTime, int freqValue)
        //{
        //    DataTable dtPrevMaintenance = new DataTable();
        //    SqlConnection sqlConn = ConnectionManager.GetConnection();
        //    SqlCommand cmd = new SqlCommand("s_ExportActivityInfo_MGTL", sqlConn);
        //    try
        //    {
        //        cmd.CommandTimeout = 120;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@MachineID", SqlDbType.NVarChar).Value = machineId;
        //        cmd.Parameters.AddWithValue("@frequency", frequency);
        //        cmd.Parameters.Add("@Starttime", SqlDbType.DateTime).Value = startTime;
        //        cmd.Parameters.Add("@Endtime", SqlDbType.DateTime).Value = endTime;
        //        cmd.Parameters.AddWithValue("@FrequencyValue", freqValue);
        //        SqlDataReader sdr = cmd.ExecuteReader();
        //        dtPrevMaintenance.Load(sdr);
        //        dtPrevMaintenance.AcceptChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("Error - \n " + ex.ToString());
        //    }
        //    finally
        //    {
        //        //if (sdr != null) sdr.Close();
        //        if (sqlConn != null) sqlConn.Close();
        //    }
        //    return dtPrevMaintenance;
        //}
        #endregion

        #region ----- Time Format ---------
        internal static string getTimeFormatFromCockpit()
        {
            string timeFormat = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("SELECT  ValueInText  FROM CockpitDefaults WHERE Parameter = 'TimeFormat'", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        timeFormat = rdr["ValueInText"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getTimeFormat = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return timeFormat;
        }
        #endregion
        #region ----- QE Visisbility ---------
        internal static bool isQERequired()
        {
            bool isVisible = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("SELECT  ValueInInt  FROM  ShopDefaults WHERE Parameter = 'QEVisibility'", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        if (rdr["ValueInInt"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            isVisible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("isQERequired = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            //isVisible = true;
            return isVisible;
        }
        internal static bool getShopDefaultReportColumnSetting(string parameter, string valueInText)
        {
            bool isVisible = true;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("SELECT  ValueInText2  FROM  ShopDefaults WHERE Parameter = @parameter and ValueInText=@valueInText", conn);
                cmd.Parameters.AddWithValue("@parameter", parameter);
                cmd.Parameters.AddWithValue("@valueInText", valueInText);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        if (rdr["ValueInText2"].ToString().Equals("N", StringComparison.OrdinalIgnoreCase))
                        {
                            isVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getShopDefaultReportColumnSetting = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            //isVisible = true;
            return isVisible;
        }
        #endregion
        #region ----- DB Version Data ---------
        internal static List<DBVersionEntity> getDBVersionDetails()
        {
            List<DBVersionEntity> list = new List<DBVersionEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("select * from DatabaseVersion order by Slno desc", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        DBVersionEntity data = new DBVersionEntity();
                        data.ScriptName = rdr["ScriptName"].ToString();
                        data.ScriptDate = rdr["ScriptDate"].ToString();
                        data.DbVersionNumber = rdr["DbVersionNumber"].ToString();
                        data.SoftwareVersionNumber = rdr["SoftwareVersionNumber"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getDBVersionDetails = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static DBVersionEntity getLatestDBVersionDetails()
        {
            DBVersionEntity data = new DBVersionEntity();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("select top 1 * from DatabaseVersion where ScriptName like '%TPMWEB%' order by Slno desc", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        data.ScriptName = rdr["ScriptName"].ToString();
                        data.ScriptDate = rdr["ScriptDate"].ToString();
                        data.DbVersionNumber = rdr["DbVersionNumber"].ToString();
                        data.SoftwareVersionNumber = rdr["SoftwareVersionNumber"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getDBVersionDetails = " + ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return data;
        }
        #endregion

        #region -------- getCellId with coma seperat ---
        internal static string getCellIDWithSeparator(ListBox listBox)
        {
            string cell = "";
            try
            {
                foreach (ListItem item in listBox.Items)
                {
                    if (item.Selected)
                    {
                        if (cell == "")
                            cell += item.Value;
                        else
                            cell += "," + item.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getCellIDWithSeparator = " + ex.ToString());
            }
            finally
            {
            }
            return cell;
        }
        internal static string getMachineIDWithSeparator(ListBox listBox)
        {
            string machine = "";
            try
            {
                foreach (ListItem item in listBox.Items)
                {
                    if (item.Selected)
                    {
                        if (machine == "")
                            machine += item.Value;
                        else
                            machine += "," + item.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineIDWithSeparator = " + ex.ToString());
            }
            finally
            {
            }
            return machine;
        }
        internal static string getListBoxValueWithSingleQuote(ListBox listBox)
        {
            string machine = "";
            try
            {
                foreach (ListItem item in listBox.Items)
                {
                    if (item.Selected)
                    {
                        if (machine == "")
                            machine += "'" + item.Value + "'";
                        else
                            machine += ",'" + item.Value + "'";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineIDWithSeparator = " + ex.ToString());
            }
            finally
            {
            }
            return machine;
        }
        internal static void setCellIDListBox(ListBox listBox, string cellID)
        {
            try
            {
                var cellAry = cellID.Split(',');
                foreach (ListItem item in listBox.Items)
                {
                    item.Selected = false;
                }
                for (int i = 0; i < cellAry.Length; i++)
                {
                    foreach (ListItem item in listBox.Items)
                    {
                        if (item.Value.Equals(cellAry[i], StringComparison.OrdinalIgnoreCase))
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("setCellIDListBox = " + ex.ToString());
            }
            finally
            {
            }
        }
        internal static string getMachineIDWithSeparatorForScreen(string cellID, string screenName)
        {
            string machineId = "";
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[SP_MultipleMachineSelection]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                cmd.Parameters.AddWithValue("@Screen", screenName);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["MACHINEID"].ToString());
                }
                machineId = string.Join(",", list.Select(k => k).ToArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineIDWithSeparatorForScreen = " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return machineId;
        }
        internal static List<string> getMachineIDListForScreen(string plantId, string cellID, string screenName)
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[SP_MultipleMachineSelection]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                cmd.Parameters.AddWithValue("@plantId", plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                cmd.Parameters.AddWithValue("@GroupID", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellID);
                cmd.Parameters.AddWithValue("@Screen", screenName);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["MACHINEID"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineIDListScreen = " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        #endregion

        #region ----Advik Panth Model ----
        internal static List<string> getAdvikPanthModel()
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct Model from ParameterTransactionDetails_Advik", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!string.IsNullOrEmpty(rdr["Model"].ToString()))
                    {
                        list.Add(rdr["Model"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getAdvikPanthModel = " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        #endregion

        #region ----Utilised DownTime Report ----
        //internal static List<UtilisedDowntimeReportEntity> getUtilisedDownTimeReportDetails(string plant,string cell,string mahineid,string fromDate,string toDate,string shift)
        //{
        //    List<UtilisedDowntimeReportEntity> list = new List<UtilisedDowntimeReportEntity>();
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    SqlCommand cmd = null;
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        cmd = new SqlCommand(@"select distinct Model from ParameterTransactionDetails_Advik", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("Plant", plant);
        //        cmd.Parameters.AddWithValue("Plant", cell);
        //        cmd.Parameters.AddWithValue("Plant", mahineid);
        //        cmd.Parameters.AddWithValue("Plant", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
        //        if (toDate != "")
        //        {
        //            cmd.Parameters.AddWithValue("Plant", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
        //        }
        //        cmd.Parameters.AddWithValue("Plant", shift);
        //        cmd.CommandTimeout = 300;
        //        rdr = cmd.ExecuteReader();
        //        if (rdr.HasRows)
        //        {
        //            while (rdr.Read())
        //            {
        //                UtilisedDowntimeReportEntity data = new UtilisedDowntimeReportEntity();
        //                data.Machine = rdr["Model"].ToString();
        //                data.UtilisedTime = rdr["Model"].ToString();
        //                data.DownTime = rdr["Model"].ToString();
        //                data.ManagementTime = rdr["Model"].ToString();
        //                list.Add(data);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("getUtilisedDownTimeReportDetails = " + ex.ToString());
        //    }
        //    finally
        //    {
        //        if (conn != null) conn.Close();
        //        if (rdr != null) rdr.Close();

        //    }
        //    return list;
        //}
        internal static DataTable getUtilisedDownTimeReportDetails(string plant, string cell, string mahineid, string fromDate, string toDate, string shift, string param, string year, string month)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_Get_ProductionAndDownTimeDetails_Sahyadri]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupID", cell);
                cmd.Parameters.AddWithValue("@MachineID", mahineid);
                if (param.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Shift", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                }
                else if (param.Equals("Month", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                    cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getUtilisedDownTimeReportDetails = " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return dt;
        }
        #endregion

        #region ------ Item Std Cycle Time Screen -----
        internal static DateTime GetItemStdCycleTimeFromDate()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DateTime fromDate = DateTime.Now;
            try
            {
                cmd = new SqlCommand(@"select min(updatedts) as FromTime from componentoperationpricing where UpdatedBy='ERP'", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if ((rdr["FromTime"] != DBNull.Value))
                    {
                        fromDate = Convert.ToDateTime(rdr["FromTime"].ToString());
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetItemStdCycleTimeFromDate: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return fromDate;
        }
        internal static List<string> GetItemStdCycleTimeUpdatedByDetails()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct UpdatedBy from componentoperationpricing", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["UpdatedBy"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetItemStdCycleTimeUpdatedByDetails: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static List<ItemStdCycleTimeDetails> GetItemStdCycleTimeDetails(string fromDate, string toDate, string machineId, string itemCode, string updatedBy, string param)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<ItemStdCycleTimeDetails> list = new List<ItemStdCycleTimeDetails>();
            try
            {
                cmd = new SqlCommand(@"SP_ViewAndSaveMachineCompAssociation_CUMI", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId == "All" ? "" : machineId);
                cmd.Parameters.AddWithValue("@ComponentID", itemCode);
                cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy == "All" ? "" : updatedBy);
                cmd.Parameters.AddWithValue("@Param", param);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ItemStdCycleTimeDetails() { ItemCode = rdr["componentid"].ToString(), ItemDescription = rdr["description"].ToString(), StdCycleTime = rdr["cycletime"].ToString(), StdMachiningTime = rdr["machiningtime"].ToString(), StdLoadUnloadTime = rdr["StdLoadunload"].ToString(), MachineId = rdr["machineid"].ToString(), OperationNo = rdr["OperationNo"].ToString(), UpdatedBy = rdr["UpdatedBy"].ToString(), UpdatedTS = rdr["UpdatedTS"].ToString() });
                    //
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetItemStdCycleTimeDetails: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        internal static string SaveItemStdCycleTimeDetails(ItemStdCycleTimeDetails data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"SP_ViewAndSaveMachineCompAssociation_CUMI", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineId);
                cmd.Parameters.AddWithValue("@ComponentID", data.ItemCode);
                cmd.Parameters.AddWithValue("@OperationNo", data.OperationNo);
                cmd.Parameters.AddWithValue("@LoadUnload", data.StdLoadUnloadTime);
                cmd.Parameters.AddWithValue("@MachiningTime", data.StdMachiningTime);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", data.Param);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    success = rdr["Flag"].ToString();
                }
            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog("GetItemStdCycleTimeDetails: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return success;
        }
        internal static DataTable GetNewAndTotalEntryDetails(out DataTable dtTotalEntry)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dtNewEntry = new DataTable();
            dtTotalEntry = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_ViewAndSaveMachineCompAssociation_CUMI", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "Count");
                rdr = cmd.ExecuteReader();
                dtNewEntry.Load(rdr);
                dtTotalEntry.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetNewAndTotalEntryDetails: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return dtNewEntry;
        }

        internal static List<HMIMachine> GetTPMTrakEnabledMachines()
        {
            List<HMIMachine> list = new List<HMIMachine>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select * from machineinformation where TPMTrakEnabled=1";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["IP"]))
                        {
                            list.Add(new HMIMachine { MachineID = Convert.ToString(sdr["Machineid"]), MachineName = Convert.ToString(sdr["description"]), IpAddress = Convert.ToString(sdr["IP"]), PortNo = Convert.ToInt32(sdr["IPPortNO"].ToString()) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving Machines - \n" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        #endregion

        internal static string BindLandingPage(string username)
        {
            string LandingPage = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("select LandingPage from employeeinformation where Employeeid=@Employeeid", conn);
                cmd.Parameters.AddWithValue("@Employeeid", username);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    LandingPage = rdr["LandingPage"] != null ? rdr["LandingPage"].ToString() : string.Empty;
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
            return LandingPage;
        }
        internal static void SaveDashboardDetails(string userid, string selectedValue)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("update employeeinformation set LandingPage=@LandingPage where Employeeid=@employee", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@employee", userid);
                cmd.Parameters.AddWithValue("@LandingPage", selectedValue);
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

        #region ----- PAMS Process ----
        internal static List<string> GetPAMSProcess()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct Process from ProcessMaster_PAMS", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 300;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["Process"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPAMSProcess: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return list;
        }
        #endregion

        #region ----------- PM Activity Transaction ------------

        internal static int getFinancialsMonth()
        {
            int month = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from ShopDefaults where Parameter='FinancialYearFrom'", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        month = DateTime.ParseExact(sdr["ValueInText"].ToString(), "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month;
                    }

                }
                else
                {
                    month = DateTime.Now.Month;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);

            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return month;
        }
        internal static List<string> getPMActivityCategory()
        {
            List<string> list = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from PMActivityCategory order by SortOrder", sqlConn);
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["Category"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static DataTable getActivityTransactionData(string machineid, string frequency, string frequencyValue, string fromDate, string toDate, string category, out DataTable dtEntraFields)
        {
            DataTable dt = new DataTable();
            dtEntraFields = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[s_Get_PMActivityInfo_Cumi]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@frequency", frequency);
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@Starttime", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Endtime", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Category", category.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : category);
                cmd.Parameters.AddWithValue("@View", "WebView");
                //cmd.Parameters.AddWithValue("@FrequencyValue", frequencyValue);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dtEntraFields.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);

            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }
        internal static DataTable getActivityFileInformationDetails(string machineid, string frequency)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from PMActivityFileInfo_Cumi where MachineID=@MachineID and Frequency=@Frequency", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@Frequency", frequency);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return dt;
        }
        internal static int insertActivityFileInformationDetails(string machineid, string frequency, string displayActivityStart, string fileName, string fileInBase64)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"if not exists(select * from PMActivityFileInfo_Cumi where MachineID=@MachineID and Frequency=@Frequency and DisplayActivityStart=@DisplayActivityStart)
begin

    insert into PMActivityFileInfo_Cumi(MachineID, Frequency, DisplayActivityStart,[FileName], FileData)

    values(@MachineID, @Frequency, @DisplayActivityStart, @FileName, @FileData)
end
else
                    begin
                        update PMActivityFileInfo_Cumi set[FileName] = @FileName, FileData = @FileData

     where MachineID = @MachineID and Frequency = @Frequency and DisplayActivityStart = @DisplayActivityStart
end", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@Frequency", frequency);
                cmd.Parameters.AddWithValue("@DisplayActivityStart", displayActivityStart);
                cmd.Parameters.AddWithValue("@FileName", fileName);
                if (fileInBase64 != "")
                {
                    byte[] fileInBytes = System.Convert.FromBase64String(fileInBase64.Substring(fileInBase64.LastIndexOf(',') + 1));
                    cmd.Parameters.AddWithValue("@FileData", fileInBytes);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FileData", DBNull.Value);
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);

            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return result;
        }
        internal static bool saveActvityTransactionDetails(string activityID, string currentFreq, string activityTS, string activityDoneTS, object selectedMachinem, string updatedBy, string shiftId, string criteria, string status, string action, string remarks, string category)
        {
            bool isUpdated = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = @"IF EXISTS(Select * from ActivityTransaction_MGTL where ActivityTS = @ActivityTS and ActivityID = @ActivityID and Machineid=@Machineid and (isnull(@ShiftID,'')=isnull(ShiftID,'')))
							   BEGIN
							   select * from ActivityTransaction_MGTL where ActivityTS = @ActivityTS
							   END
							   ELSE
							   BEGIN
							   Insert into ActivityTransaction_MGTL([ActivityID], [Frequency], [ActivityTS], [ActivityDoneTS], [Machineid],[Status],[Action],Remarks,Category,UpdatedBy,YearNo,MonthNo,[ShiftID])
	                            Values(@ActivityID, @Frequency, @ActivityTS, @ActivityDoneTS, @Machineid,@Status,@Action,@Remarks,@Category,@UpdatedBy,@YearNo, @MonthNo,@ShiftID) 
							  END";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@ActivityID", activityID));
                cmd.Parameters.Add(new SqlParameter("@Frequency", currentFreq));
                cmd.Parameters.Add(new SqlParameter("@ActivityTS", activityTS));
                cmd.Parameters.Add(new SqlParameter("@ActivityDoneTS", activityDoneTS));
                cmd.Parameters.Add(new SqlParameter("@Machineid", selectedMachinem));
                cmd.Parameters.Add(new SqlParameter("@ShiftID", shiftId));
                //cmd.Parameters.Add(new SqlParameter("@Criteria", criteria));
                cmd.Parameters.Add(new SqlParameter("@Status", status));
                cmd.Parameters.Add(new SqlParameter("@Action", action));
                cmd.Parameters.Add(new SqlParameter("@Remarks", remarks));
                cmd.Parameters.Add(new SqlParameter("@Category", category));
                cmd.Parameters.Add(new SqlParameter("@UpdatedBy", updatedBy));
                cmd.Parameters.AddWithValue("@YearNo", Convert.ToDateTime(activityTS).Year);
                cmd.Parameters.AddWithValue("@MonthNo", Convert.ToDateTime(activityTS).Month);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    isUpdated = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                isUpdated = false;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return isUpdated;
        }
        #endregion

        #region ---------- Week No. --------
        internal static List<string> getWeekNumbersOfYear(string year)
        {
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select distinct WeekNumber from Calender where YearNo=@YearNo", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@YearNo", year);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["WeekNumber"].ToString());
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
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static int getCurrentWeekNumber()
        {
            int currentWeek = 0;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select distinct WeekNumber from Calender where WeekDate=@WeekDate", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@WeekDate", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        currentWeek = Convert.ToInt32(sdr["WeekNumber"].ToString());
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
                if (conn != null) conn.Close();
            }
            return currentWeek;
        }
        #endregion

        #region Modbus Screen Data
        public static DataTable GetModbusData()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            string query = "select m.machineid as Machine_ID, s.* from machineinformation m left join SmartDataModbusRegisterInfo s on m.machineID=s.MachineID where DAPEnabled in ('2','3')";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ModbusScreenData:" + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        public static bool UpdateModbusScreenData(List<string> UpdatedValues)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            bool success = false;
            int result = 0;

            string query = "if exists(select * from SmartDataModbusRegisterInfo where MachineID=@MachineID) begin update SmartDataModbusRegisterInfo set HoldingRegisterForCommunication = @HoldingRegisterForCommunication, HoldingRegisterDateAndStatus = @HoldingRegisterDateAndStatus, HoldingRegisterDateAndStatusAckAddress = @HoldingRegisterDateAndStatusAckAddress, HoldingRegisterStartAddress_M1 = @HoldingRegisterStartAddress_M1, BytesToRead_M1 = @BytesToRead_M1, AckAddress_M1 = @AckAddress_M1, HoldingRegisterStartAddress_M2 = @HoldingRegisterStartAddress_M2, BytesToRead_M2 = @BytesToRead_M2, AckAddress_M2 = @AckAddress_M2, HoldingRegisterStartAddress_M3 = @HoldingRegisterStartAddress_M3, BytesToRead_M3 = @BytesToRead_M3, AckAddress_M3 = @AckAddress_M3, DownCodeRequestAddess = @DownCodeRequestAddess, DownCodeStartingAddress = @DownCodeStartingAddress, DownInterfaceIDStartingAddress = @DownInterfaceIDStartingAddress, TotalNumberOfDownCodeAddress = @TotalNumberOfDownCodeAddress, [Inserted Date/time] = @InsertedDatetime, [Updated Date/Time]= @UpdatedDateTime, [LastUpdated By]= @LastUpdatedBy, HMIRegister_M1 = @HMIRegister_M1, HMIRegister_M2 = @HMIRegister_M2,SharedMachineID = @SharedMachineID where MachineID = @MachineID End else begin insert into SmartDataModbusRegisterInfo(MachineID, HoldingRegisterForCommunication, HoldingRegisterDateAndStatus, HoldingRegisterDateAndStatusAckAddress, HoldingRegisterStartAddress_M1, BytesToRead_M1, AckAddress_M1, HoldingRegisterStartAddress_M2, BytesToRead_M2, AckAddress_M2, HoldingRegisterStartAddress_M3, BytesToRead_M3, AckAddress_M3, DownCodeRequestAddess, DownCodeStartingAddress, DownInterfaceIDStartingAddress, TotalNumberOfDownCodeAddress, [Inserted Date/time], [Updated Date/Time], [LastUpdated By], HMIRegister_M1, HMIRegister_M2, SharedMachineID)values(@MachineID, @HoldingRegisterForCommunication,@HoldingRegisterDateAndStatus, @HoldingRegisterDateAndStatusAckAddress, @HoldingRegisterStartAddress_M1, @BytesToRead_M1,@AckAddress_M1, @HoldingRegisterStartAddress_M2, @BytesToRead_M2, @AckAddress_M2, @HoldingRegisterStartAddress_M3,@BytesToRead_M3, @AckAddress_M3, @DownCodeRequestAddess, @DownCodeStartingAddress, @DownInterfaceIDStartingAddress,@TotalNumberOfDownCodeAddress, @InsertedDatetime, @UpdatedDateTime, @LastUpdatedBy, @HMIRegister_M1, @HMIRegister_M2, @SharedMachineID) end";

            //string query = "if exists(select * from SmartDataModbusRegisterInfo where MachineID=@MachineID) begin update SmartDataModbusRegisterInfo set HoldingRegisterForCommunication = @HoldingRegisterForCommunication, HoldingRegisterDateAndStatus = @HoldingRegisterDateAndStatus, HoldingRegisterDateAndStatusAckAddress = @HoldingRegisterDateAndStatusAckAddress, HoldingRegisterStartAddress_M1 = @HoldingRegisterStartAddress_M1, BytesToRead_M1 = @BytesToRead_M1, AckAddress_M1 = @AckAddress_M1, HoldingRegisterStartAddress_M2 = @HoldingRegisterStartAddress_M2, BytesToRead_M2 = @BytesToRead_M2, AckAddress_M2 = @AckAddress_M2, HoldingRegisterStartAddress_M3 = @HoldingRegisterStartAddress_M3, BytesToRead_M3 = @BytesToRead_M3, AckAddress_M3 = @AckAddress_M3, DownCodeRequestAddess = @DownCodeRequestAddess, DownCodeStartingAddress = @DownCodeStartingAddress, @InsertedDatetime, [Updated Date/Time]= @UpdatedDateTime, [LastUpdated By]= @LastUpdatedBy, HMIRegister_M1 = @HMIRegister_M1, HMIRegister_M2 = @HMIRegister_M2,SharedMachineID = @SharedMachineID where MachineID = @MachineID End else begin insert into SmartDataModbusRegisterInfo(MachineID, HoldingRegisterForCommunication, HoldingRegisterDateAndStatus, HoldingRegisterDateAndStatusAckAddress, HoldingRegisterStartAddress_M1, BytesToRead_M1, AckAddress_M1, HoldingRegisterStartAddress_M2, BytesToRead_M2, AckAddress_M2, HoldingRegisterStartAddress_M3, BytesToRead_M3, AckAddress_M3, DownCodeRequestAddess, DownCodeStartingAddress, [Inserted Date/time], [Updated Date/Time], [LastUpdated By], HMIRegister_M1, HMIRegister_M2, SharedMachineID) values(@MachineID, @HoldingRegisterForCommunication,@HoldingRegisterDateAndStatus, @HoldingRegisterDateAndStatusAckAddress, @HoldingRegisterStartAddress_M1, @BytesToRead_M1, @AckAddress_M1, @HoldingRegisterStartAddress_M2, @BytesToRead_M2, @AckAddress_M2, @HoldingRegisterStartAddress_M3, @BytesToRead_M3, @AckAddress_M3, @DownCodeRequestAddess, @DownCodeStartingAddress,@DownInterfaceIDStartingAddress, @TotalNumberOfDownCodeAddress, @InsertedDatetime, @UpdatedDateTime, @LastUpdatedBy, @HMIRegister_M1, @HMIRegister_M2, @SharedMachineID) end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", UpdatedValues[0]);
                cmd.Parameters.AddWithValue("@HoldingRegisterForCommunication", UpdatedValues[1]);
                cmd.Parameters.AddWithValue("@HoldingRegisterDateAndStatus", UpdatedValues[2]);
                cmd.Parameters.AddWithValue("@HoldingRegisterDateAndStatusAckAddress", UpdatedValues[3]);
                cmd.Parameters.AddWithValue("@HoldingRegisterStartAddress_M1", UpdatedValues[4]);
                cmd.Parameters.AddWithValue("@BytesToRead_M1", UpdatedValues[5]);
                cmd.Parameters.AddWithValue("@AckAddress_M1", UpdatedValues[6]);
                cmd.Parameters.AddWithValue("@HoldingRegisterStartAddress_M2", UpdatedValues[7]);
                cmd.Parameters.AddWithValue("@BytesToRead_M2", UpdatedValues[8]);
                cmd.Parameters.AddWithValue("@AckAddress_M2", UpdatedValues[9]);
                cmd.Parameters.AddWithValue("@HoldingRegisterStartAddress_M3", UpdatedValues[10]);
                cmd.Parameters.AddWithValue("@BytesToRead_M3", UpdatedValues[11]);
                cmd.Parameters.AddWithValue("@AckAddress_M3", UpdatedValues[12]);
                cmd.Parameters.AddWithValue("@DownCodeRequestAddess", UpdatedValues[13]);
                cmd.Parameters.AddWithValue("@DownCodeStartingAddress", UpdatedValues[14]);
                cmd.Parameters.AddWithValue("@DownInterfaceIDStartingAddress", UpdatedValues[15]);
                cmd.Parameters.AddWithValue("@TotalNumberOfDownCodeAddress", UpdatedValues[16]);
                cmd.Parameters.AddWithValue("@InsertedDatetime", UpdatedValues[17]);
                cmd.Parameters.AddWithValue("@UpdatedDateTime", UpdatedValues[18]);
                cmd.Parameters.AddWithValue("@LastUpdatedBy", UpdatedValues[19]);
                cmd.Parameters.AddWithValue("@HMIRegister_M1", UpdatedValues[20]);
                cmd.Parameters.AddWithValue("@HMIRegister_M2", UpdatedValues[21]);
                cmd.Parameters.AddWithValue("@SharedMachineID", UpdatedValues[22]);

                result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateModbusScreenData : " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }
        #endregion

        #region ---- PDI Report Tafe Chennai ---
        internal static List<string> getSerialNoForPDIReportTafeChennai(string slnoSearch)
        {
            List<string> list = new List<string>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct SerialNo from PDISpcDetailsTransaction_Tafe where SerialNo like '%" + slnoSearch + "%'", sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SerialNo", slnoSearch);

                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["SerialNo"].ToString());
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
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        #endregion

        #region ------- DownTime ActionTaken ------
        internal static List<string> GetDownIDMachineWise(string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            string query = "select distinct DownID from ShiftDownTimeDetails where MachineID=@MachineID";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["DownID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(":" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<DownTimeActionTakenEntity> GetDownTimeActionTakenScreenData(string downID, string MachineID, string StartDate, string EndDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<DownTimeActionTakenEntity> list = new List<DownTimeActionTakenEntity>();
            try
            {
                cmd = new SqlCommand("select distinct StartTime, EndTime, Remarks, ActionTaken from ShiftDownTimeDetails where DownID = @DownID and (dDate >= @StartDate AND dDate <= @EndDate) and MachineID = @MachineID  order by StartTime", conn);
                cmd.Parameters.AddWithValue("@DownID", downID);
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    while (sdr.Read())
                    {
                        DownTimeActionTakenEntity entity = new DownTimeActionTakenEntity();
                        entity.DownStartTime = Convert.ToDateTime(sdr["StartTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        entity.DownEndTime = Convert.ToDateTime(sdr["EndTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        entity.Remarks = sdr["Remarks"].ToString();
                        entity.ActionTaken = sdr["ActionTaken"].ToString();
                        list.Add(entity);
                    }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDownTimeActionTakenScreenData: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        #endregion

        #region ----- PITTI ENGI ------
        internal static Dictionary<string, string> GetSuperVisorInfo_Pitti()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                cmd = new SqlCommand("select * from employeeinformation where EmployeeRole='Supervisor'", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list[sdr["Employeeid"].ToString()] = sdr["Name"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetSuperVisorInfo_Pitti: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<DailyChecklistEntity_Pitti> GetDailyCheckpointData_Pitti(string MachineID, string RevNo, string RefNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<DailyChecklistEntity_Pitti> list = new List<DailyChecklistEntity_Pitti>();
            try
            {
                cmd = new SqlCommand(@"S_Get_AM_MaintenanceDetails_Pitti", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    int i = 0;
                    while (sdr.Read())
                    {
                        DailyChecklistEntity_Pitti entity = new DailyChecklistEntity_Pitti();
                        entity.SlNo = (++i).ToString();
                        entity.CheckPoint = sdr["SerialNo"].ToString();
                        entity.Standard = sdr["Standard"].ToString();
                        entity.Frequency = sdr["Frequency"].ToString();
                        entity.CheckPointDesc = sdr["CheckpointDescription"].ToString();
                        entity.SortOrder = sdr["SortOrder"].ToString();
                        list.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDailyCheckpointData_Pitti: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static bool SaveDailyCheckMasterData(DailyChecklistEntity_Pitti data, string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool isInserted = false;
            try
            {
                cmd = new SqlCommand(@"S_Get_AM_MaintenanceDetails_Pitti", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@SerialNo", data.CheckPoint);
                cmd.Parameters.AddWithValue("@CheckpointDescription", data.CheckPointDesc);
                cmd.Parameters.AddWithValue("@Standard", data.Standard);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                cmd.CommandTimeout = 120;
                int res = cmd.ExecuteNonQuery();

                if (res > 0)
                {
                    isInserted = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveDailyCheckMasterData: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isInserted;
        }

        internal static string DeleteCheckpointMasterData(string MachineID, string CheckpointID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string isDeleted = "";
            try
            {
                cmd = new SqlCommand("S_Get_AM_MaintenanceDetails_Pitti", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@SerialNo", CheckpointID);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                isDeleted = cmd.ExecuteNonQuery() > 0 ? "Deleted" : "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isDeleted;
        }

        internal static int CopyRevData(string ProcName, string DestMachineID, string SrcMachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int res = 0;
            try
            {
                cmd = new SqlCommand(ProcName, conn);
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

        internal static bool SaveDailyCheckHeaderData(string MachineID, string RefernceNo, string RevisionNo, string Supervisor)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            bool isInserted = false;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from DailyCheckListHeaderDetails_Pitti where MachineID=@MachineID)
begin
insert into DailyCheckListHeaderDetails_Pitti(MachineID, RefNo, RevNo, SupervisorID) values(@MachineID,@RefernceNo,@RevisionNo,@Supervisor)
end
else
begin
update DailyCheckListHeaderDetails_Pitti set RefNo=@RefernceNo, RevNo=@RevisionNo,SupervisorID=@Supervisor where MachineID=@MachineID
end", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@RefernceNo", RefernceNo);
                cmd.Parameters.AddWithValue("@RevisionNo", RevisionNo);
                cmd.Parameters.AddWithValue("@Supervisor", Supervisor);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    isInserted = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveDailyCheckHeaderData: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isInserted;
        }

        internal static DataTable GetHeaderDetails_Pitti(string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select * from DailyCheckListHeaderDetails_Pitti where MachineID=@MachineID", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
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
                Logger.WriteErrorLog("GetHeaderDetails_Pitti: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static List<DailyChecklistReportEntity_Pitti> GetCheckPointTransactionData_Pitti(string MachineID, int Year, int Month, out DataTable dt1)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            dt1 = new DataTable();
            List<DailyChecklistReportEntity_Pitti> list = new List<DailyChecklistReportEntity_Pitti>();
            try
            {
                cmd = new SqlCommand("S_Get_AM_MaintenanceDetails_Pitti", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    if (sdr != null)
                        dt1.Load(sdr);

                    if (dt.Rows.Count > 0)
                    {
                        DailyChecklistReportEntity_Pitti entity = new DailyChecklistReportEntity_Pitti();
                        List<InnerLViewEntity_Pitti> li_innerData = new List<InnerLViewEntity_Pitti>();

                        for (int k = 5; k < dt.Columns.Count; k++)
                        {
                            InnerLViewEntity_Pitti li = new InnerLViewEntity_Pitti();
                            li.DateValue = dt.Columns[k].ColumnName.ToString();
                            li.HeaderVisibility = true;
                            li_innerData.Add(li);
                        }
                        entity.HeaderVisibility = true;
                        entity.InnerListViewData = li_innerData;
                        list.Add(entity);

                        int i = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            entity = new DailyChecklistReportEntity_Pitti();
                            entity.SlNo = (++i).ToString();
                            entity.MachineID = row["MachineID"].ToString();
                            entity.CheckPointID = row["SerialNo"].ToString();
                            entity.CheckPointDesc = row["CheckpointDescription"].ToString();
                            entity.Standard = row["Standard"].ToString();
                            entity.Frequency = row["Frequency"].ToString();
                            //entity.Operator = row["OperatorID"].ToString();

                            li_innerData = new List<InnerLViewEntity_Pitti>();
                            for (int k = 5; k < dt.Columns.Count; k++)
                            {
                                InnerLViewEntity_Pitti li = new InnerLViewEntity_Pitti();

                                li.DateValue = string.IsNullOrEmpty(row[k].ToString()) ? " " : row[k].ToString().Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
                                li.ContentVisibility = true;
                                li_innerData.Add(li);
                            }
                            entity.ContentVisibility = true;
                            entity.InnerListViewData = li_innerData;

                            list.Add(entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCheckPointReportData_Pitti: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static DataTable GetCheckPointReportData_Pitti(string MachineID, int Year, int Month, out DataTable dt1)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            dt1 = new DataTable();
            try
            {
                cmd = new SqlCommand("S_Get_AM_MaintenanceDetails_Pitti", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    if (sdr != null)
                        dt1.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCheckPointReportData_Pitti: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        #endregion

        #region----------------------PM Cheksheet Details--------------------------------
        internal static List<ListItem> GetSrEngineers()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                cmd = new SqlCommand(@"select Employeeid,Name from employeeinformation where EmployeeRole='Sr Engineer'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ListItem() { Text = sdr["Name"].ToString(), Value = sdr["Employeeid"].ToString() });
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
            return list;
        }
        internal static List<ListItem> GetManagers()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                cmd = new SqlCommand(@"select Employeeid,Name from employeeinformation where EmployeeRole='Manager'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ListItem() { Text = sdr["Name"].ToString(), Value = sdr["Employeeid"].ToString() });
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
            return list;
        }
        internal static List<ListItem> GetHODs()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                cmd = new SqlCommand(@"select Employeeid,Name from employeeinformation where EmployeeRole='HOD'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ListItem() { Text = sdr["Name"].ToString(), Value = sdr["Employeeid"].ToString() });
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
            return list;
        }
        internal static List<ListItem> GetMaintenanceMnagers()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                cmd = new SqlCommand(@"select Employeeid,Name from employeeinformation where EmployeeRole='MaintenanceManager'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ListItem() { Text = sdr["Name"].ToString(), Value = sdr["Employeeid"].ToString() });
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
            return list;
        }
        internal static bool SavePMCheckSheetHeaderData(string MachineID, string PreparedBy, string CheckedBy, string ApprovedBy, string ReferenceNo, string RevisionNo, string Supervisor, string MaintenanceManager, string Frequency)
        {
            bool Isinserted = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if Exists(select * from dbo.PM_HeaderDetails_Pitti where MachineID=@MachineID and Frequency=@Frequency)
Begin
	Update dbo.PM_HeaderDetails_Pitti set RefNo = @RefernceNo,SupervisorID=@Supervisor,MaintenanceManagerID=@MaintenanceManagerID,RevNo=@RevisionNo,PreparedBy=@PreparedBy,CheckedBy=@CheckedBy,ApprovedBy=@ApprovedBy
	where machineid=@machineid and Frequency=@Frequency
	Select 'Updated' as SaveFlag
End
Else 
Begin
	Insert into dbo.PM_HeaderDetails_Pitti(MachineID,Frequency,RefNo,SupervisorID,MaintenanceManagerID,RevNo,PreparedBy,CheckedBy,ApprovedBy)
	values(@MachineID,@Frequency,@RefernceNo,@Supervisor,@MaintenanceManagerID,@RevisionNo,@PreparedBy,@CheckedBy,@ApprovedBy)
	Select 'Inserted' as SaveFlag
End", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                //cmd.Parameters.AddWithValue("@YearNo", Convert.ToInt32(DateTime.Now.ToString("yyyy")));
                //cmd.Parameters.AddWithValue("@MonthNo", Convert.ToInt32(DateTime.Now.ToString("MM")));
                cmd.Parameters.AddWithValue("@PreparedBy", PreparedBy);
                cmd.Parameters.AddWithValue("@CheckedBy", CheckedBy);
                cmd.Parameters.AddWithValue("@ApprovedBy", ApprovedBy);
                cmd.Parameters.AddWithValue("@RefernceNo", ReferenceNo);
                cmd.Parameters.AddWithValue("@RevisionNo", RevisionNo);
                cmd.Parameters.AddWithValue("@Supervisor", Supervisor);
                cmd.Parameters.AddWithValue("@MaintenanceManagerID", MaintenanceManager);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    Isinserted = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return Isinserted;
        }
        internal static string SavePMChecksheetMasterData(PreventiveMaintenanceChecksheet_Pitti data)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_Get_PM_MaintenanceDetails_Pitti", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@Category", data.Category);
                cmd.Parameters.AddWithValue("@SerialNo", data.CheckpointID);
                cmd.Parameters.AddWithValue("@CheckpointDescription", data.Description);
                cmd.Parameters.AddWithValue("@JudgementCriteria", data.JudgementalCriteria);
                cmd.Parameters.AddWithValue("@ResourcesNeeded", data.ResourcesNeeded);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                //cmd.Parameters.AddWithValue("@SerialNo", data.SortOrder);
                cmd.Parameters.AddWithValue("@Duration", data.Duration);
                cmd.Parameters.AddWithValue("@Param", "MasterSave");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
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
            return success;
        }
        internal static List<PreventiveMaintenanceChecksheet_Pitti> GetPMMasterData(string MachineID, string Frequency)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PreventiveMaintenanceChecksheet_Pitti> list = new List<PreventiveMaintenanceChecksheet_Pitti>();
            PreventiveMaintenanceChecksheet_Pitti data = null;
            try
            {
                cmd = new SqlCommand(@"S_Get_PM_MaintenanceDetails_Pitti", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new PreventiveMaintenanceChecksheet_Pitti();
                        data.CategoryID = sdr["CategoryID"].ToString();
                        data.Category = sdr["Category"].ToString();
                        data.CheckpointID = sdr["SerialNo"].ToString();
                        data.Description = sdr["CheckpointDescription"].ToString();
                        data.JudgementalCriteria = sdr["JudgementCriteria"].ToString();
                        data.ResourcesNeeded = sdr["ResourcesNeeded"].ToString();
                        data.Frequency = sdr["Frequency"].ToString();
                        data.SortOrder = sdr["SortOrder"].ToString();
                        data.Duration = sdr["Duration"].ToString();
                        list.Add(data);
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
            return list;
        }
        internal static string DeletePMMasterData(PreventiveMaintenanceChecksheet_Pitti data, string Frequency)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"S_Get_PM_MaintenanceDetails_Pitti", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@SerialNo", data.CheckpointID);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@Param", "MasterDelete");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
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
            return success;
        }
        internal static List<string> GetCategories()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct Category from PM_Master_Pitti", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["Category"].ToString());
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
            return list;
        }
        internal static DataTable GetPMChecksheetReportDetails(string year, string Frequency, string MachineID, string Category, out DataTable dt2, out DataTable dt3, out DataTable dt4)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            dt4 = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_Get_PM_MaintenanceDetails_Pitti", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Category", Category);
                cmd.Parameters.AddWithValue("@Param", "ReportView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt2.Load(sdr);
                    dt3.Load(sdr);
                    dt4.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt;
        }
        #endregion

        #region ---- Vulkan MachineShop ----
        internal static string getEmployeeRoleVulkanMS(string UserName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string UserRole = string.Empty;
            try
            {
                cmd = new SqlCommand(@"if exists(select * from employeeinformation where Employeeid=@UserName)
begin
	select EmployeeRole as EmployeeRole from employeeinformation where Employeeid=@UserName
end
else
begin
	select 'not exists' as EmployeeRole
end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                        UserRole = sdr["EmployeeRole"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"getEmployeeRoleVulkanMS: {ex}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return UserRole;
        }
        #endregion

        #region --- Master Pages Export ---
        internal static List<DownCodesModel> GetDowntimeCodesReportDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<DownCodesModel> list = new List<DownCodesModel>();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from downcodeinformation order by catagory", con);
                cmd.CommandType = CommandType.Text;

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (int.TryParse(sdr["interfaceid"].ToString(), out int res))
                            list.Add(new DownCodesModel()
                            {
                                downid = sdr["downid"].ToString(),
                                interfaceid = sdr["interfaceid"].ToString(),
                                Catagory = sdr["Catagory"].ToString(),
                                downdescription = sdr["downdescription"].ToString(),
                                chkAvaileffy = sdr["availeffy"].ToString(),
                                Owner = sdr["Owner"].ToString(),
                                Threshold = HelperClassGeneric.getDoubleValueFromString(sdr["Threshold"].ToString())
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDowntimeCodesReportDetails:  " + ex.Message);
            }
            finally
            {
                if (sdr != null) { sdr.Close(); sdr.Dispose(); }
                if (con != null) { con.Close(); con.Dispose(); }

            }
            return list;
        }
        internal static List<EmployeeInformationModel> GetEmployeeInfoReportDetails(string category, string Value)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<EmployeeInformationModel> list = new List<EmployeeInformationModel>();
            string query = "";
            try
            {
                if (category.Equals("EmployeeID", StringComparison.OrdinalIgnoreCase))
                    query = @"select A1.Employeeid,A1.Name,a1.interfaceid,a1.designation,a1.qualification,A1.address1,A1.phone,A1.Email,
                                (SELECT STRING_AGG(PlantID,', ') FROM PlantEmployee where EmployeeID = A1.Employeeid group by EmployeeID) AS PLANTID,a1.EmployeeRole
                                from employeeinformation A1 where Employeeid like ('%'+@value+'%') order by A1.EmployeeRole, A1.Employeeid";
                else if (category.Equals("name", StringComparison.OrdinalIgnoreCase))
                    query = @"select A1.Employeeid,A1.Name,a1.interfaceid,a1.designation,a1.qualification,A1.address1,A1.phone,A1.Email,
                                (SELECT STRING_AGG(PlantID,', ') FROM PlantEmployee where EmployeeID = A1.Employeeid group by EmployeeID) AS PLANTID,a1.EmployeeRole
                                from employeeinformation A1 where Name like ('%'+@value+'%') order by A1.EmployeeRole, A1.Employeeid";
                else if (category.Equals("interfaceid", StringComparison.OrdinalIgnoreCase))
                    query = @"select A1.Employeeid,A1.Name,a1.interfaceid,a1.designation,a1.qualification,A1.address1,A1.phone,A1.Email,
                                (SELECT STRING_AGG(PlantID,', ') FROM PlantEmployee where EmployeeID = A1.Employeeid group by EmployeeID) AS PLANTID,a1.EmployeeRole
                                from employeeinformation A1 where InterfaceID like ('%'+@value+'%') order by A1.EmployeeRole, A1.Employeeid";
                else
                    query = @"select A1.Employeeid,A1.Name,a1.interfaceid,a1.designation,a1.qualification,A1.address1,A1.phone,A1.Email,
                                (SELECT STRING_AGG(PlantID,', ') FROM PlantEmployee where EmployeeID = A1.Employeeid group by EmployeeID) AS PLANTID,a1.EmployeeRole
                                from employeeinformation A1 order by A1.EmployeeRole, A1.Employeeid";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Value", Value);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new EmployeeInformationModel()
                        {
                            employeeid = sdr["Employeeid"].ToString(),
                            name = sdr["Name"].ToString(),
                            interfaceid = sdr["interfaceid"].ToString(),
                            designation = sdr["designation"].ToString(),
                            qualification = sdr["qualification"].ToString(),
                            address = sdr["address1"].ToString(),
                            phone = sdr["phone"].ToString(),
                            email = sdr["Email"].ToString(),
                            plantid = sdr["PLANTID"].ToString(),
                            role = sdr["EmployeeRole"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) { sdr.Close(); sdr.Dispose(); }
                if (con != null) { con.Close(); con.Dispose(); }

            }
            return list;
        }

        internal static List<RejectionCodesModel> GetRejectionCodesReportDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<RejectionCodesModel> list = new List<RejectionCodesModel>();

            try
            {
                string query = @"select Catagory, rejectionno, rejectionid,interfaceid,rejectiondescription from rejectioncodeinformation order by Catagory,rejectionno";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new RejectionCodesModel()
                        {
                            Category = sdr["Catagory"].ToString(),
                            RejectionNo = sdr["rejectionno"].ToString(),
                            RejectionID = sdr["rejectionid"].ToString(),
                            InterfaceID = sdr["interfaceid"].ToString(),
                            RejectionDesc = sdr["rejectiondescription"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) { sdr.Close(); sdr.Dispose(); }
                if (con != null) { con.Close(); con.Dispose(); }

            }
            return list;
        }

        internal static List<CustomerInformationModel> GetCustomerInfoReportDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<CustomerInformationModel> list = new List<CustomerInformationModel>();

            try
            {
                string query = @"select * from customerinformation order by country, state";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new CustomerInformationModel()
                        {
                            customerid = sdr["customerid"].ToString(),
                            customername = sdr["customername"].ToString(),
                            address = sdr["address1"].ToString(),
                            place = sdr["place"].ToString(),
                            state = sdr["state"].ToString(),
                            country = sdr["country"].ToString(),
                            pin = sdr["pin"].ToString(),
                            phone = sdr["phone"].ToString(),
                            email = sdr["email"].ToString(),
                            contactperson = sdr["contactperson"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) { sdr.Close(); sdr.Dispose(); }
                if (con != null) { con.Close(); con.Dispose(); }

            }
            return list;
        }

        internal static List<ReworkCodesModel> GetReworkCodesReportDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<ReworkCodesModel> list = new List<ReworkCodesModel>();

            try
            {
                string query = @"select Reworkid, Reworkno, Reworkdescription, ReworkCatagory, Reworkinterfaceid from Reworkinformation order by  Reworkno, ReworkCatagory";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ReworkCodesModel()
                        {
                            reworkid = sdr["Reworkid"].ToString(),
                            reworkno = sdr["Reworkno"].ToString(),
                            reworkdesc = sdr["Reworkdescription"].ToString(),
                            reworkcategory = sdr["ReworkCatagory"].ToString(),
                            interfaceid = sdr["Reworkinterfaceid"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) { sdr.Close(); sdr.Dispose(); }
                if (con != null) { con.Close(); con.Dispose(); }

            }
            return list;
        }
        #endregion

        #region --- Precision Engg ---
        internal static void InsertOrUpdateComponentIdDetailsPE(string Componentid, string CompInterfaceID, string customerid, string description, string descriptionInHindi, out bool isSuccessOrFailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            isSuccessOrFailure = false;
            try
            {
                SqlCommand cmd = new SqlCommand(@"if Exists(Select * from componentinformation where componentid=@Componentid and InterfaceID=@CompInterfaceID and customerid=@customerid)
                                            BEGIN
                                            UPDATE componentinformation SET description=@description, CompDescriptionHindi=@DescriptionInHindi where componentid=@Componentid and InterfaceID=@CompInterfaceID and customerid=@customerid
                                            END", sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 450;
                cmd.Parameters.AddWithValue("@Componentid", Componentid);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@DescriptionInHindi", descriptionInHindi);
                cmd.Parameters.AddWithValue("@customerid", customerid);
                cmd.Parameters.AddWithValue("@CompInterfaceID", CompInterfaceID);
                int x = cmd.ExecuteNonQuery();
                isSuccessOrFailure = true;
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


        internal static void UpdateDataPE(string Catagory, string rejectionid, string txtDescription, string DescriptionInHindi, string txtInterfaceId, string subcategory, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GlobePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand("update rejectioncodeinformation set interfaceid=@Interfaceid,rejectiondescription=@Description,Catagory=@Catagory,SubCategory=@subcat,UpdatedTS=@UpdatedTS where rejectionid=@rejectionId", conn);
                }
                else
                {
                    cmd = new SqlCommand("update rejectioncodeinformation set interfaceid=@Interfaceid,rejectiondescription=@Description,RejDescriptionHindi=@DescriptionInHindi,Catagory=@Catagory,UpdatedTS=@UpdatedTS where rejectionid=@rejectionId", conn);
                }
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@rejectionId", rejectionid);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Trim());
                cmd.Parameters.AddWithValue("@DescriptionInHindi", DescriptionInHindi.Trim());
                cmd.Parameters.AddWithValue("@Catagory", Catagory.Trim());
                cmd.Parameters.AddWithValue("@Interfaceid", txtInterfaceId.Trim());
                cmd.Parameters.AddWithValue("@subcat", subcategory.Trim());
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }


        internal static void insertDataPE(string Catagory, string txtDescription, string DescriptionInHindi, string rejectionid, string txtInterfaceId, string subcategory, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["GlobePages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand("insert into rejectioncodeinformation (rejectionid,rejectiondescription,Catagory,interfaceid,SubCategory,UpdatedTS) values (@rejectionId,@Description,@Catagory,@Interfaceid,@subcat,@UpdatedTS)", conn);
                }
                else
                {
                    cmd = new SqlCommand("insert into rejectioncodeinformation (rejectionid,rejectiondescription,RejDescriptionHindi,Catagory,interfaceid,UpdatedTS) values (@rejectionId,@Description,@DescriptionInHindi,@Catagory,@Interfaceid,@UpdatedTS)", conn);
                }
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@rejectionId", rejectionid);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Trim());
                cmd.Parameters.AddWithValue("@DescriptionInHindi", DescriptionInHindi.Trim());
                cmd.Parameters.AddWithValue("@Catagory", Catagory.Trim());
                cmd.Parameters.AddWithValue("@Interfaceid", txtInterfaceId.Trim());
                cmd.Parameters.AddWithValue("@subcat", subcategory.Trim());
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void UpdateDataForReworkPE(string Catagory, string txtDescription, string RewDescriptionInHindi, string Reworkid, string txtInterfaceId, out string sucessfailure)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("update reworkinformation set  Reworkinterfaceid=@Reworkinterfaceid,Reworkdescription=@Reworkdescription, ReworkDescriptionInHindi=@DescriptionInHindi,ReworkCatagory=@ReworkCatagory,UpdatedTS=@UpdatedTS where Reworkid=@Reworkid", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Reworkid", Reworkid);
                cmd.Parameters.AddWithValue("@Reworkdescription", txtDescription);
                cmd.Parameters.AddWithValue("@DescriptionInHindi", RewDescriptionInHindi);
                cmd.Parameters.AddWithValue("@ReworkCatagory", Catagory);
                cmd.Parameters.AddWithValue("@Reworkinterfaceid", txtInterfaceId);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void insertDataForReworkPE(string reworkId, string txtDescription, string ReworkDescriptionInHinidi, string Catagory, string txtInterfaceId, out string sucessfailure)
        {

            sucessfailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("insert into reworkinformation (Reworkid,Reworkdescription,ReworkDescriptionInHindi,ReworkCatagory,Reworkinterfaceid,UpdatedTS) values (@reworkId,@Description,@DescriptionInHindi,@Catagory,@Interfaceid,@UpdatedTS)", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@reworkId", reworkId);
                cmd.Parameters.AddWithValue("@Description", txtDescription);
                cmd.Parameters.AddWithValue("@DescriptionInHindi", ReworkDescriptionInHinidi);
                cmd.Parameters.AddWithValue("@Catagory", Catagory);
                cmd.Parameters.AddWithValue("@Interfaceid", txtInterfaceId);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                sucessfailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessfailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        #endregion

        #region---WIP DASHBOARD DETAILS--------------
        public static List<string> GetWIPPartNo()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct componentid from componentinformation", con);
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
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        public static List<string> GetWIPMachineType()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct MachineType from HeatCodeGeneration_Tafe", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["MachineType"].ToString());
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
            return list;
        }
        public static List<WIPDashboardMachineDetails> GetWIPMachineDetails(out DataTable dt1, DateTime FromDate, DateTime ToDate, string MachineType, string PartNo, string WIPProcess)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            dt1 = new DataTable();
            List<WIPDashboardMachineDetails> list = new List<WIPDashboardMachineDetails>();
            WIPDashboardMachineDetails data = null;
            try
            {
                cmd = new SqlCommand(@"SP_WIPDashBoard_Tafe", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", ToDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                cmd.Parameters.AddWithValue("@PartNo", PartNo);
                cmd.Parameters.AddWithValue("@WipProcess", WIPProcess);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
                dt1.Load(sdr);
                dt1.AcceptChanges();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        data = new WIPDashboardMachineDetails();
                        data.MachineID = dtrow["MachineID"].ToString();
                        data.PartNo = dtrow["PartNumber"].ToString();
                        data.OperationNo = dtrow["OperationNo"].ToString();
                        data.Quantity = dtrow["QTY"].ToString();
                        list.Add(data);
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
            return list;
        }
        public static DataTable GetWIPCompleteDetals(out DataTable dt1, out DataTable dt2, out DataTable dt3, DateTime FromDate, DateTime ToDate, string MachineType, string PartNo, string WIPProcess)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            dt1 = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_WIPDashBoard_Tafe", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", ToDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                cmd.Parameters.AddWithValue("@PartNo", PartNo);
                cmd.Parameters.AddWithValue("@WipProcess", WIPProcess);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
                dt1.Load(sdr);
                dt1.AcceptChanges();
                dt2.Load(sdr);
                dt2.AcceptChanges();
                dt3.Load(sdr);
                dt3.AcceptChanges();
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

        #region------Inspection Report & Schedule Import-Rexnord-----------------
        internal static List<string> GetSlNo_Rexnord()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct SerialNumber from SPCAutodata", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["SerialNumber"].ToString());
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
            return list;
        }
        internal static List<string> GetOperaionNo_Rexnord(string SerialNo)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct Opn from SPCAutodata where SerialNumber=@SerialNumber", con);
                cmd.Parameters.AddWithValue("@SerialNumber", SerialNo);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["Opn"].ToString());
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
            return list;
        }
        internal static DataTable GetInspectionReportData_Rexnord(string SerialNo, string OperationNo)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionReport_REXNORD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SerialNo", SerialNo);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.Parameters.AddWithValue("@Param", "InspectionReport");
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }
        internal static void SaveImportDataToTemp_Rexnord(DataTable dt)
        {
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                //OperationDesc	WorkCenter	SerialNo	WorkOrderNo	Qty	ComponentID	ComponentDesc	Operation	UpdatedTS	ProcessingTime	IDD

                //OperationDesc WorkCenter  SerialNo WorkOrderNo Qty ComponentID ComponentDesc Operation   P_Number Action  Confirmation CA1 Unit1 CA2 Unit2 CA3 Unit3 SystemStatus    SetupTime ProcessingTime1 ProcessingTime2
                bulkCopy = new SqlBulkCopy(con);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[temp_ScheduledDetails_REXNORD]";
                bulkCopy.ColumnMappings.Add("OperationDesc", "OperationDesc");
                bulkCopy.ColumnMappings.Add("WorkCenter", "WorkCenter");
                bulkCopy.ColumnMappings.Add("SerialNo", "SerialNo");
                bulkCopy.ColumnMappings.Add("WorkOrderNo", "WorkOrderNo");
                bulkCopy.ColumnMappings.Add("Qty", "Qty");
                bulkCopy.ColumnMappings.Add("ComponentID", "ComponentID");
                bulkCopy.ColumnMappings.Add("ComponentDesc", "ComponentDesc");
                bulkCopy.ColumnMappings.Add("Operation", "Operation");
                bulkCopy.ColumnMappings.Add("ProcessingTime", "ProcessingTime");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.TempManualEntryPumpTestData_Mivin .", e.RowsCopied));
                };

                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
            }
        }
        internal static string ImportScheduledata_Rexnord()
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"s_ImportScheduledDetails_REXNORD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "Import");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
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
            return success;
        }
        internal static DataTable GetSheduleData_Rexnord()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select * from ScheduledDetails_REXNORD", con);
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }

        internal static List<WorkOrderTrackingData_Rexnord> GetWorkOrderTrackingData_Rexnord(string fromDate, string toDate, string workOrderSearch, string slnoSearch)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<WorkOrderTrackingData_Rexnord> list = new List<WorkOrderTrackingData_Rexnord>();
            WorkOrderTrackingData_Rexnord data = null;
            DataTable dt = new DataTable();
            //string slNo = "";
            try
            {
                cmd = new SqlCommand(@"[dbo].[SP_WorkOrder_SerialTrackingDashbord_Rexnord]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (fromDate != "" && toDate != "")
                {
                    cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                cmd.Parameters.AddWithValue("@WorkOrderNumber", workOrderSearch);
                cmd.Parameters.AddWithValue("@SerialNumber", slnoSearch);
                cmd.Parameters.AddWithValue("@Param", "View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
                if (dt.Rows.Count > 0)
                {
                    var distslNo = dt.AsEnumerable().Select(x => x.Field<string>("serialnumber")).Distinct().ToList();
                    int i = 1;
                    foreach (var slNo in distslNo)
                    {
                        DataTable dtSlNo = dt.AsEnumerable().Where(x => x.Field<string>("serialnumber").ToString().Equals(slNo, StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                        foreach (DataRow dtRow in dtSlNo.Rows)
                        {
                            data = new WorkOrderTrackingData_Rexnord();
                            data.WorkOrder = dtRow["WorkorderNumber"].ToString();
                            data.SerialNo = dtRow["serialnumber"].ToString();
                            data.ComponentID = dtRow["Componentid"].ToString();
                            data.OperationNo = dtRow["OperationNo"].ToString();
                            data.Machine = dtRow["MachineID"].ToString();
                            data.Operator = dtRow["OperatorName"].ToString();
                            data.StartTime = Convert.IsDBNull(dtRow["StTime"]) ? "" : Convert.ToDateTime(dtRow["StTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                            data.EndTime = Convert.IsDBNull(dtRow["NdTime"]) ? "" : Convert.ToDateTime(dtRow["NdTime"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                            data.ActualTime = dtRow["CycleTime"].ToString();
                            data.Rejection = dtRow["OperationStatus"].ToString();
                            //data.RejectionRemarks = dtRow["RejectionRemarks"].ToString();
                            //data.RejectionBy = dtRow["RejectionBy"].ToString();
                            data.OperationType = dtRow["OperationType"].ToString();
                            if (dtRow["OperationType"].ToString().Equals("Manual", StringComparison.OrdinalIgnoreCase))
                            {
                                data.colspan = 5;
                                data.ClassVisibility = true;
                                data.ColumnVisibility = false;
                                data.Machine = "Manual Cycle";
                                if (dtRow["OperationStatus"].ToString() == "Ok")
                                {
                                    data.AdditionalIconClass = "manual-status-icon glyphicon glyphicon-ok-circle manual-status-green";
                                }
                                else
                                {
                                    data.AdditionalIconClass = "manual-status-icon glyphicon glyphicon-remove-circle manual-status-red";
                                }
                            }
                            if (i % 2 == 0)
                            {
                                data.trBackColor = "#DCDCDC";
                            }
                            else
                            {
                                data.trBackColor = "#FFFFFF";
                            }
                            list.Add(data);
                        }
                        i++;
                    }
                }
                //while (sdr.Read())
                //{

                //}
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
            return list;
        }
        #endregion
        internal static List<string> GetLanguages()
        {
            List<string> langList = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string query = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MultiLingual_MC' AND TABLE_SCHEMA='dbo'";
            try
            {
                SqlCommand command = new SqlCommand(query, conn);
                sdr = command.ExecuteReader();
                int i = 0;
                while (sdr.Read())
                {
                    if (i > 2)
                    {
                        langList.Add(sdr["COLUMN_NAME"].ToString().ToLower());
                    }
                    i++;
                }
            }
            catch (Exception Ex)
            {
                Logger.WriteErrorLog(Ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return langList;
        }

        #region -- Clean Up WEB --
        internal static DataTable BindDB()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dataTable = new DataTable();
            try
            {
                //string conString = WebConfigurationManager.ConnectionStrings[0].ToString();

                string query = "select * from Shopdefaults where Parameter = 'DBBackUpName' or Parameter ='DBMaintenance' order by Parameter";
                cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);

                if (conn.State == ConnectionState.Open) conn.Close();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindDB" + ex);
            }

            return dataTable;
        }

        internal static bool AddUpdate(string ValueInInt, string ValueInText, string ValueInText2, string Parameter)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool success = false;
            try
            {

                string insertQuery = @"if not exists(select * from ShopDefaults where Parameter =@Parameter and ValueInText=@ValueInText)
BEGIN
	insert into Shopdefaults (Parameter, ValueInText, ValueInText2, ValueInInt, UpdatedTS) values (@Parameter, @ValueInText, @ValueInText2, @ValueInInt,@UpdatedTS)
END
else
BEGIN
	UPDATE ShopDefaults set ValueInText2=@ValueInText2, ValueInInt=@ValueInInt, UpdatedTS=@UpdatedTS where Parameter =@Parameter and ValueInText=@ValueInText
END";
                cmd = new SqlCommand(insertQuery, conn);

                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@ValueInInt", ValueInInt);
                cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
                cmd.Parameters.AddWithValue("@ValueInText2", ValueInText2);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                if (cmd.ExecuteNonQuery() >= 0)
                    success = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"AddUpdate: {ex}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }

        internal static DataTable BindRecords()
        {
            DataTable dataTable = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {

                string query = @"select ROW_NUMBER() over (order by Tablename asc) SlNo,TableName,NoOfDaysToRetain from TPM_DataCleanUpTableList order by SlNo";
                cmd = new SqlCommand(query, conn);
                adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindRecords_DB:" + ex);
            }
            finally
            {
                if (adapter != null) adapter.Dispose();
                if (conn != null) conn.Close();
            }
            return dataTable;
        }
        internal static bool UpdateRecords(string selectedValue, string days)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool success = true;
            try
            {
                //string conString = WebConfigurationManager.ConnectionStrings[0].ToString();

                string insertQuery = @"if not exists(select * from TPM_DataCleanUpTableList where TableName=@TableName)
BEGIN
	INSERT INTO TPM_DataCleanUpTableList(TableName, NoOfDaysToRetain) Values(@TableName, @NoOfDaysToRetain)
END
ELSE 
BEGIN
	Update TPM_DataCleanUpTableList set NoOfDaysToRetain=@NoOfDaysToRetain where TableName=@TableName
END";
                cmd = new SqlCommand(insertQuery, conn);

                cmd.Parameters.AddWithValue("@TableName", selectedValue);
                cmd.Parameters.AddWithValue("@NoOfDaysToRetain", days);
                //cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (cmd.ExecuteNonQuery() < 0)
                {
                    success = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateRecords:" + ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }

        internal static string GetNoOfDays(string TableName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string days = "";
            try
            {
                cmd = new SqlCommand(@"select * from TPM_DataCleanUpTableList where TableName=@TableName", conn);
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@TableName", TableName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                        days = sdr["NoOfDaysToRetain"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetNoOfDays:" + ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return days;
        }

        internal static string DeleteRecords(string TableName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int rowCount = 0;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"Update TPM_DataCleanUpTableList set NoOfDaysToRetain=0 where TableName=@TableName", conn);
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@TableName", TableName);
                rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetNoOfDays:" + ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            if (rowCount == 1)
            {
                success = "Deleted";
            }
            else
                success = "Error";
            return success;
        }
        #endregion

        #region -- Custom ProductionReport DynamicFlow Aggregated --
        internal static DataTable GetCustomProductionReport_DF(string CellID, string MachineID, string shift, DateTime FromDate, DateTime ToDate, out DataTable dt_SummaryData, out DataTable dt_DownID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            dt_SummaryData = new DataTable();
            dt_DownID = new DataTable();
            DataTable dt_ProductionData = new DataTable();
            try
            {
                cmd = new SqlCommand("S_GetProductionReport_DynamicFlow", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", FromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", ToDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@CellID", CellID);

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt_ProductionData.Load(sdr);
                    if (dt_ProductionData.Columns.IndexOf("SaveFlag") == -1)
                    {
                        dt_SummaryData.Load(sdr);
                        dt_DownID.Load(sdr);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCustomeProductionReport_DF" + ex.Message);
            }

            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return dt_ProductionData;
        }
        #endregion
        #region-----Work Order Details - Kun Aero---------------
        internal static List<string> GetWorkOrderID_KunAero()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct WorkOrderID from WorkOrderDetails_KunAero order by WorkOrderID desc", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["WorkOrderID"].ToString());
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
            return list;
        }
        internal static List<string> GetWorkOrderNo_KunAero()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct WorkOrderNumber from WorkOrderDetails_KunAero order by WorkOrderNumber desc", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["WorkOrderNumber"].ToString());
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
            return list;
        }
        internal static List<string> GetWorkOrderFY_KunAero()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct WorkOrderFY from WorkOrderDetails_KunAero order by WorkOrderFY desc", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["WorkOrderFY"].ToString());
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
            return list;
        }
        internal static DataTable GetWorkOrderData_Kunaero(string WorkOrderNumber, string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = @"select  WorkOrderNumber,WorkOrderDate,WorkOrderQty,WorkOrderFY,WorkOrderID from(select   WorkOrderNumber,WorkOrderDate,WorkOrderQty,workorderfy,workorderid,
		Dense_rank() over (partition by WorkOrderID order by WorkOrderDate desc)LatestDate,
		ROW_NUMBER() over (partition by WorkOrderID order by updatedts desc)LatestTSWithinDate
		from WorkOrderDetails_KunAero)WorkOrderDetails_KunAero 
		where LatestDate=1 and LatestTSWithinDate=1  and
	 (workorderdate>=@FromDate and workorderdate<=@ToDate) ";
            try
            {
                if (WorkOrderNumber == "")
                {
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@WorkOrderNumber", WorkOrderNumber);

                }
                else
                {
                    cmd = new SqlCommand(@"select  WorkOrderNumber,WorkOrderDate,WorkOrderQty,WorkOrderFY,WorkOrderID from(select WorkOrderNumber,WorkOrderDate,WorkOrderQty,workorderfy,workorderid,
		Dense_rank() over (partition by WorkOrderID order by WorkOrderDate desc)LatestDate,
		ROW_NUMBER() over (partition by WorkOrderID order by updatedts desc)LatestTSWithinDate
		from WorkOrderDetails_KunAero)WorkOrderDetails_KunAero 
		where LatestDate=1 and LatestTSWithinDate=1 
		and ((WorkOrderNumber like '%" + @WorkOrderNumber + "%') and (workorderdate>=@FromDate and workorderdate<=@ToDate))", con);
                }
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd"));
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }
        internal static DataTable GetWorkOrderData_Kunaero_New(string WorkOrderNumber, string WorkOrderDate, string WorkOrderFY, string WorkOrderID)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select  WorkOrderNumber,WorkOrderDate,WorkOrderQty,WorkOrderFY,WorkOrderID from(select   WorkOrderNumber,WorkOrderDate,WorkOrderQty,workorderfy,workorderid,
		Dense_rank() over (partition by WorkOrderID order by WorkOrderDate desc)LatestDate,
		ROW_NUMBER() over (partition by WorkOrderID order by updatedts desc)LatestTSWithinDate
		from WorkOrderDetails_KunAero)WorkOrderDetails_KunAero 
		where LatestDate=1 and LatestTSWithinDate=1 and ((WorkOrderNumber=@workordernumber) or isnull(@workordernumber,'')='')
 and ((workorderdate>=@workorderdate and workorderdate<=@workorderdate) or(ISNULL(@workorderdate,'')='')) and (WorkOrderFY in (SELECT Item FROM SplitStrings(@workorderfy,',')))
 and ((WorkOrderID=@WorkOrderID) or isnull(@WorkOrderID,'')='')", con);
                cmd.Parameters.AddWithValue("@workorderfy", WorkOrderFY);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.Parameters.AddWithValue("@workordernumber", WorkOrderNumber);
                cmd.Parameters.AddWithValue("@workorderdate", Util.GetDateTime(WorkOrderDate).ToString("yyyy-MM-dd"));
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }
        internal static string SaveWorkOrderData_Kunaero(string WorkOrderNumber, string Date, string Qty, string ID, string FY)
        {
            string success = string.Empty;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"insert into WorkOrderDetails_KunAero(WorkOrderNumber,WorkOrderDate,WorkOrderQty,UpdatedTS,WorkOrderID,WorkOrderFY) values(@WorkOrderNumber,@WorkOrderDate,@WorkOrderQty,@UpdatedTS,@WorkOrderID,@WorkOrderFY) select 'INSERTED' as SaveFlag", con);
                cmd.Parameters.AddWithValue("@WorkOrderNumber", WorkOrderNumber);
                cmd.Parameters.AddWithValue("@WorkOrderDate", Util.GetDateTime(Date));
                cmd.Parameters.AddWithValue("@WorkOrderQty", Qty);
                cmd.Parameters.AddWithValue("@WorkOrderID", ID);
                cmd.Parameters.AddWithValue("@WorkOrderFY", FY);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
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
            return success;
        }
        internal static string DeleteWorkOrderData_Kunaero(string WorkOrderNumber, string Date, string Qty, string ID, string FY)
        {
            string success = string.Empty;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"delete from WorkOrderDetails_KunAero where WorkOrderNumber=@WorkOrderNumber and WorkOrderDate=@WorkOrderDate and WorkOrderQty=@WorkOrderQty and WorkOrderID=@WorkOrderID and WorkOrderFY=@WorkOrderFY
select 'DELETED' as SaveFlag", con);
                cmd.Parameters.AddWithValue("@WorkOrderNumber", WorkOrderNumber);
                cmd.Parameters.AddWithValue("@WorkOrderDate", Convert.ToDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@WorkOrderQty", Qty);
                cmd.Parameters.AddWithValue("@WorkOrderID", ID);
                cmd.Parameters.AddWithValue("@WorkOrderFY", FY);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
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
            return success;
        }
        internal static void ImportWorkOrderData_KunAero(DataTable dt)
        {
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                //ID	WorkOrderFY	WorkOrderID	WorkOrderNumber	WorkOrderDate	WorkOrderQty	UpdatedTS
                bulkCopy = new SqlBulkCopy(con);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[WorkOrderDetails_KunAero]";
                bulkCopy.ColumnMappings.Add("WorkOrderNumber", "WorkOrderNumber");
                bulkCopy.ColumnMappings.Add("WorkOrderDate", "WorkOrderDate");
                bulkCopy.ColumnMappings.Add("WorkOrderQty", "WorkOrderQty");
                bulkCopy.ColumnMappings.Add("UpdatedTS", "UpdatedTS");
                bulkCopy.ColumnMappings.Add("WorkOrderFY", "WorkOrderFY");
                bulkCopy.ColumnMappings.Add("WorkOrderID", "WorkOrderID");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.WorkOrderDetails_KunAero .", e.RowsCopied));
                };

                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
            }
        }

        #endregion

        #region ---Daily Maintenance Checksheet ---
        internal static List<GroupDefintion> GetMaintenanceGroupDetails_Precision()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            List<GroupDefintion> list = new List<GroupDefintion>();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from MaintenanceGroupMaster_Precision", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    GroupDefintion entity = new GroupDefintion();
                    entity.GroupID = rdr["GroupID"].ToString();
                    entity.GroupDesc = rdr["GroupDesc"].ToString();
                    list.Add(entity);
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
            return list;
        }
        internal static List<string> GetMachines()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select distinct machineid from machineinformation", con);
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
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        internal static int SaveMaintenanceGroup_Precision(string groupID, string groupDesc)
        {
            int ok = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from MaintenanceGroupMaster_Precision where GroupID=@GroupID)
                begin
                    insert into MaintenanceGroupMaster_Precision(GroupID, GroupDesc)values(@GroupID, @GroupDesc)
                end
                else
                begin
                    update MaintenanceGroupMaster_Precision set GroupDesc = @GroupDesc where GroupID = @GroupID
                end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@GroupDesc", groupDesc);
                ok = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return ok;
        }
        internal static List<DailyCheckSheet_Precision> GetDailyMaintenanceDetails_Precision(string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<DailyCheckSheet_Precision> list = new List<DailyCheckSheet_Precision>();
            try
            {
                cmd = new SqlCommand(@"S_GetDailyCleaning&MaintanenceSheet_Precision", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Param", "View_MasterDetails");
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    DailyCheckSheet_Precision entity = new DailyCheckSheet_Precision();
                    entity.CheckPointID = sdr["CheckpointID"].ToString();
                    entity.CheckPointDesc = sdr["CheckpointDesc"].ToString();
                    entity.CheckPointDescInHindi = sdr["CheckpointDescInHindi"].ToString();
                    entity.Frequency = sdr["Frequency"].ToString();
                    entity.FrequencyOrder = sdr["FreqWiseSortOrder"].ToString();
                    list.Add(entity);
                }
            }
            catch (Exception Ex)
            {
                Logger.WriteErrorLog(Ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static int SaveDailyMaintenanceCheckSheet(DailyCheckSheet_Precision data)
        {
            int count = 0; string success = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetDailyCleaning&MaintanenceSheet_Precision]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CheckpointID", data.CheckPointID);
                cmd.Parameters.AddWithValue("@CheckpointDesc", data.CheckPointDesc);
                cmd.Parameters.AddWithValue("@CheckpointDescInHindi", data.CheckPointDescInHindi);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@FreqWiseOrder", data.FrequencyOrder);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "Save_MasterDetails");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SAVEFLAG"].ToString();
                    }
                }
                if (success != "")
                    count = 1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return count;
        }
        internal static int CopyDailyMaintenanceCheckSheetData(string SourceMachine, string DestMachine)
        {
            int count = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetDailyCleaning&MaintanenceSheet_Precision]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SourceMachineID", SourceMachine);
                cmd.Parameters.AddWithValue("@MachineID", DestMachine);
                cmd.Parameters.AddWithValue("@Param", "Copy_MasterDetails");
                count = cmd.ExecuteNonQuery();
                if (count != 0)
                    count = 1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return count;
        }
        internal static int deleteCheckPointID_Precision(string CheckPointID, string machine)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int recordDeleted = 0;
            try
            {
                string sqlQuery = "delete from DailyCheckSheetMasterDetails_Precision where CheckpointID=@CheckPointID and MachineID=@MachineID";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@CheckPointID", CheckPointID);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                recordDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return recordDeleted;
        }

        internal static List<string> GetMachineforGroup_Precision(string GroupID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from MaintenanceGroupMachine_Precision where (GroupID=@GroupID or ISNULL(@GroupID,'')='')", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@GroupID", GroupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : GroupID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["MachineID"].ToString());
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
            return list;
        }
        #endregion
        #region-------------Highway-------------------------
        internal static List<string> GetRevID(string Component, string Operation)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct RevID from InspectionMasterDetails_Highway where ComponentID=@ComponentID and OperationNo=@OperationNo", con);
                //cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
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
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<string> GetComponents()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select distinct componentid from componentoperationpricing", con);
                //cmd.Parameters.AddWithValue("@machineid", MachineID);
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
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<string> GetOperations(string Component)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select distinct operationno from componentoperationpricing where componentid=@componentid", con);
                //cmd.Parameters.AddWithValue("@machineid", MachineID);
                cmd.Parameters.AddWithValue("@componentid", Component);
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
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<string> GetDieNo(string MachineID, string Component, string OperationNo, string Shift, string Date)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select distinct DieNo from InspectionTransactionDetails_Highway where MachineID=@MachineID and ComponentID=@ComponentID and 
OperationNo=@OperationNo and Shift=@Shift and Date=@Date", con);
                cmd.Parameters.AddWithValue("@machineid", MachineID);
                cmd.Parameters.AddWithValue("@componentid", Component);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["DieNo"].ToString());
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
            return list;
        }
        internal static List<string> GetHeatNo(string MachineID, string Component, string OperationNo, string Shift, string Date)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select distinct HeatNo from InspectionTransactionDetails_Highway where MachineID=@MachineID and ComponentID=@ComponentID and 
OperationNo=@OperationNo and Shift=@Shift and Date=@Date", con);
                cmd.Parameters.AddWithValue("@machineid", MachineID);
                cmd.Parameters.AddWithValue("@componentid", Component);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["HeatNo"].ToString());
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
            return list;
        }

        #endregion

        #region  -- RNGupta --
        internal static DataTable GetProductionReport_RNGupta(DateTime fromDate, DateTime toDate, string shift, string cellId, string machineId)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetDailyProductionReport_RNGupta]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GroupId", cellId);
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@Machine", machineId);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
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

        internal static DataTable GetIncentiveReport_RNGupta(DateTime fromDate, DateTime toDate, string shift, string cellId, string machineId)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetIncentiveReport_RNGupta]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GroupId", cellId);
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@Machine", machineId);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
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

        internal static DataTable GetMonthlyProductionReport_RNGupta(string yearNo, string monthNo)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetMonthlyProductionReport_RNGupta]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@YearNo", yearNo);
                cmd.Parameters.AddWithValue("@MonthNo", monthNo);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
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




        #region  -- DEV/PRIME/DENZO Industries Program Change Dashboard --

        internal static List<DashboardChangeEntity> GetIncidentChangeData(string MachineID, string FromDate, string ToDate)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<DashboardChangeEntity> list = new List<DashboardChangeEntity>();
            DashboardChangeEntity data = default;
            DataTable dt = new DataTable();
            string sqlQuery = string.Empty;
            try
            {

                //sqlQuery = "SELECT * FROM ParameterDetailsLog_NewTech Where ChangedAt in (SELECT MAX(ChangedAT) from ParameterDetailsLog_NewTech where ChangedAt >= @StartDate AND ChangedAt<=@EndDate and MachineID=@MachineID GROUP BY MachineID)";

                sqlQuery = "SELECT * FROM ParameterDetailsLog where ChangedAt >= @StartDate AND ChangedAt<= @EndDate and(MachineID in (SELECT Item FROM SplitStrings(@MachineID, ',')) or isnull(@MachineID,'')= '') order by MachineID";

                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new DashboardChangeEntity();
                        data.MachineID = sdr["machineID"].ToString();
                        data.programID = sdr["ParameterID"].ToString();
                        data.PreviousData = sdr["PreviousData"].ToString().Replace("&$", "<br />");// '&$' New Line Seperator inserted from service
                         data.ChangedData = sdr["ChangedData"].ToString().Replace("&$", "<br />");// '&$' New Line Seperator inserted from service
                        data.changedTime = Convert.ToDateTime(sdr["ChangedAt"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                        list.Add(data);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetIncidentChangeData: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        internal static List<DashboardChangeEntity> GetIncidentChangeDataForProgramNos(string MachineID, string programNo, string FromDate, string ToDate)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<DashboardChangeEntity> list = new List<DashboardChangeEntity>();
            DashboardChangeEntity data = default;
            DataTable dt = new DataTable();
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "SELECT * FROM ParameterDetailsLog where ChangedAt >= @StartDate AND ChangedAt<= @EndDate and(MachineID in (SELECT Item FROM SplitStrings(@MachineID, ',')) or isnull(@MachineID,'')= '') and (ParameterID in (select Item from SplitStrings(@ParameterID, ',')) or isnull(@ParameterID,'')= '') order by MachineID";

                cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ParameterId", programNo);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new DashboardChangeEntity();
                        data.MachineID = sdr["machineID"].ToString();
                        data.programID = sdr["ParameterID"].ToString();
                        data.PreviousData = sdr["PreviousData"].ToString().Replace("&$", "<br />");
                        data.ChangedData = sdr["ChangedData"].ToString().Replace("&$", "<br />");
                        data.changedTime = Convert.ToDateTime(sdr["ChangedAt"].ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                        list.Add(data);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetIncidentChangeData: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }


        internal static List<string> GetProgramNoforMachine(string machineID, string fromDate, string toDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = " SELECT distinct ParameterID FROM ParameterDetailsLog where ChangedAt >= @StartDate AND ChangedAt<= @EndDate and(MachineID in (SELECT Item FROM SplitStrings(@MachineID, ',')) ) ";
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        string programNo = sdr["ParameterID"].ToString();
                        list.Add(programNo);
                    }
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
            return list;
        }

        #endregion

        internal static void SaveProgramUploadDetails(string machine, string userName, string ProgramNo, string Event)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = default;
            try
            {
                cmd = new SqlCommand(@"insert into MachineProgramTransferDetails(MachineID,ProgramNo,UpdatedBy,UpdatedTS,Event)
                values(@MachineID,@ProgramNo,@UpdatedBy,@UpdatedTS,@Event)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@ProgramNo", ProgramNo);
                cmd.Parameters.AddWithValue("@UpdatedBy", userName);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Event", Event);
                int success = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                conn?.Close();
                conn?.Dispose();
            }
        }


        internal static string InsertVEDScheduleDetails(VEDSchedule data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = default;
            string success = "";
            string query = "";
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS(SELECT * FROM ScheduleDetails_VEDH WHERE PlantID=@PlantID AND GroupID=@GroupID AND PartID=@ComponentID AND OperationNo=@OpnNo AND StartDate=@StartDate)
 BEGIN
	IF EXISTS(SELECT * FROM ScheduleDetails_VEDH WHERE PlantID=@PlantID AND GroupID=@GroupID  AND Priority=@PriorityNo and [status]=1)
	BEGIN
		SELECT 'Priority Number already Exists' as SaveFlag
		RETURN 
	END
	ELSE
	BEGIN
		INSERT INTO ScheduleDetails_VEDH(PlantID, GroupID, PartID, PartName, OperationNo, Priority, qty, StartDate, Status, UpdatedBy, UpdatedTS)
		VALUES(@PlantID, @GroupID, @ComponentID, @PartName, @OpnNo, @PriorityNo, @Qty, @StartDate, @Status, @UpdatedBy, GETDATE())
		SELECT 'Inserted' as SaveFlag
		RETURN
	END
 END
 ELSE
 BEGIN
	SELECT 'Schedule Already Exists' as SaveFlag
	RETURN
 END", conn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@startDate", Util.GetDateTime(data.StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@plantID", data.PlantID);
                cmd.Parameters.AddWithValue("@GroupID", data.CellID);
                cmd.Parameters.AddWithValue("@ComponentID", data.ComponentID);
                cmd.Parameters.AddWithValue("@PartName", data.ComponentDesc);
                cmd.Parameters.AddWithValue("@OpnNo", data.OperationNumber);
                cmd.Parameters.AddWithValue("@PriorityNo", data.PriorityNumber.Trim());
                cmd.Parameters.AddWithValue("@qty", data.Quantity.Trim());
                cmd.Parameters.AddWithValue("@enddate", data.EndDate);
                //if(data.Status.Equals("New", StringComparison.OrdinalIgnoreCase))
                cmd.Parameters.AddWithValue("@Updatedby", HttpContext.Current.Session["Username"].ToString());
                cmd.Parameters.AddWithValue("@Status", data.Status);
                //cmd.Parameters.AddWithValue("@sequence", Convert.ToInt32(data.Sequence.Trim() == "" ? "0" : data.Sequence.Trim()));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SaveFlag"].ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                //result = 0;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }



        internal static int UpdateVEDScheduleDetails(VEDSchedule data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = default;
            int success = 0;
            string query = "";
            try
            {
                cmd = new SqlCommand(@"UPDATE ScheduleDetails_VEDH
	
SET [Priority] = @Priority, Qty = @Qty, UpdatedBy = @UpdatedBy
	
WHERE PlantID = @PlantID and PartID = @PartID and GroupID = @GroupID and OperationNo = @OperationNo
	and Startdate = @Startdate ", conn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@startDate", Util.GetDateTime(data.StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@plantID", data.PlantID);
                cmd.Parameters.AddWithValue("@GroupID", data.CellID);
                cmd.Parameters.AddWithValue("@PartID", data.ComponentID);
                //cmd.Parameters.AddWithValue("@PartName", "");
                cmd.Parameters.AddWithValue("@OperationNo", data.OperationNumber);
                cmd.Parameters.AddWithValue("@Priority", data.PriorityNumber.Trim());
                cmd.Parameters.AddWithValue("@qty", data.Quantity.Trim());
                cmd.Parameters.AddWithValue("@enddate", "");
                //if(data.Status.Equals("New", StringComparison.OrdinalIgnoreCase))
                cmd.Parameters.AddWithValue("@Updatedby", "");
                // sdr = cmd.ExecuteReader();
                success = cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //result = 0;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }

        internal static List<VEDSchedule> GetVEDScheduleDetails(string plantID, string cellID, string startDate, string endDate, string status)

        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<VEDSchedule> list = new List<VEDSchedule>();
            VEDSchedule data = null;
            try
            {

                cmd = new SqlCommand(@"SELECT * FROM ScheduleDetails_VEDH WHERE (PlantID=@PlantID OR ISNULL(@PlantID,'')='') and (GroupID=@GroupID OR ISNULL(@GroupID,'')='') and ((startDate>=@StartDate OR ISNULL(@StartDate,'')='') and startdate<=@Enddate) and ((status in (SELECT Item FROM SplitStrings(@status,',')) or isnull(@status,'')='') ) ", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@GroupID", cellID);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@Enddate", endDate);
                cmd.Parameters.AddWithValue("@status", status);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new VEDSchedule();
                        data.StartDate = Util.GetDateTime(sdr["StartDate"].ToString()).ToString("dd-MM-yyyy");
                        data.PlantID = sdr["PlantID"].ToString();
                        data.CellID = sdr["GroupID"].ToString();
                        data.ComponentID = sdr["PartID"].ToString();
                        data.ComponentDesc = sdr["PartName"].ToString();
                        data.OperationNumber = sdr["OperationNo"].ToString();
                        data.PriorityNumber = sdr["Priority"].ToString();
                        data.Quantity = sdr["Qty"].ToString();
                        if (sdr["EndDate"].ToString() != "")
                        {
                            data.EndDate = Util.GetDateTime(sdr["EndDate"].ToString()).ToString("dd-MM-yyyy");
                        }
                        if (sdr["Status"].ToString() == "1")
                        {
                            data.Status = "New";
                            data.PriorityEnabled = true;
                            data.QtyEnabled = true;
                        }
                        else if (sdr["Status"].ToString() == "2")
                        {
                            data.Status = "Running";
                            data.PriorityEnabled = false;
                            data.QtyEnabled = false;
                        }
                        else
                        {
                            data.Status = "Closed";
                            data.PriorityEnabled = false;
                            data.QtyEnabled = false;
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
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }


        internal static DataTable GetVEDScheduleDetailsfortable()

        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<VEDSchedule> list = new List<VEDSchedule>();
            VEDSchedule data = null;
            DataTable dt = new DataTable();
            try
            {

                cmd = new SqlCommand(@"SELECT * FROM ScheduleDetails_VEDH", conn);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
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
            return dt;
        }





        internal static List<VEDSchedule> getComponentIDwithplantcell(string plant, string cell)
        {
            List<VEDSchedule> list = new List<VEDSchedule>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {	//SELECT distinct A1.componentid,A1.description FROM componentoperationpricing A1 INNER JOIN PlantMachineGroups A2 ON A1.machineid=A2.MachineID WHERE A2.PlantID=@PlantID AND A2.GroupID=@GroupID
                cmd = new SqlCommand(@"select distinct cop.componentid,c.description from componentoperationpricing cop inner join
componentinformation c on c.componentid=cop.componentid
where machineid in (select MachineID from PlantMachineGroups where PlantID=@PlantID and GroupID=@GroupID)", conn);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupID", cell);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        VEDSchedule data = new VEDSchedule();
                        data.ComponentID = rdr["componentid"].ToString();
                        //data.OperationNumber = rdr["operationno"].ToString();
                        data.ComponentDesc = rdr["description"].ToString();
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
        internal static List<VEDSchedule> getOperatiowithplantcell(string plant, string cell, string component)
        {
            List<VEDSchedule> list = new List<VEDSchedule>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"	SELECT distinct operationno FROM componentoperationpricing A1 INNER JOIN PlantMachineGroups A2 ON A1.machineid=A2.MachineID WHERE A2.PlantID=@PlantID AND A2.GroupID=@GroupID AND A1.ComponentID=@ComponentID", conn);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupID", cell);
                cmd.Parameters.AddWithValue("@ComponentID", component);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        VEDSchedule data = new VEDSchedule();
                        //data.ComponentID = rdr["componentid"].ToString();
                        data.OperationNumber = rdr["operationno"].ToString();
                        //data.ComponentDesc = rdr["description"].ToString();
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
        internal static int DeleteVEDScheduleDetails(VEDSchedule data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("delete from ScheduleDetails_VEDH where StartDate=@startdate and plantID=@plantID and groupiD=@groupiD and partid=@partID and operationNo=@operationNo", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@startdate", Util.GetDateTime(data.StartDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", data.PlantID);
                cmd.Parameters.AddWithValue("@Groupid", data.CellID);
                cmd.Parameters.AddWithValue("@partID", data.ComponentID);
                cmd.Parameters.AddWithValue("@ComponentID", data.ComponentID);
                cmd.Parameters.AddWithValue("@OperationNo", data.OperationNumber);

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

        internal static int CloseVEDScheduleDetails(VEDSchedule data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("Update ScheduleDetails_VEDH set Status=@Status,endDate = @EndDate where StartDate=@startDate and plantID=@PlantID and groupiD=@Groupid and partid=@PartID and operationNo=@operationNo ", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@startDate", Util.GetDateTime(data.StartDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", data.PlantID);
                cmd.Parameters.AddWithValue("@Groupid", data.CellID);
                cmd.Parameters.AddWithValue("@PartID", data.ComponentID);
                cmd.Parameters.AddWithValue("@operationNo", data.OperationNumber);
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(data.EndDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Status", data.Status);
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

        public static string saveImportedDataToTempTable(DataTable dt)
        {
            string result = "";
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            string conString = WebConfigurationManager.ConnectionStrings[HttpContext.Current.Session["connectionString"].ToString()].ToString();
            try
            {
                bulkCopy = new SqlBulkCopy(conString);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[Temp_ScheduleDetails_VEDH]";
                bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                bulkCopy.ColumnMappings.Add("PlantID", "PlantID");
                bulkCopy.ColumnMappings.Add("CellID", "GroupID");
                bulkCopy.ColumnMappings.Add("ComponentID", "PartID");
                bulkCopy.ColumnMappings.Add("ComponentDesc", "PartName");
                bulkCopy.ColumnMappings.Add("Operation", "OperationNo");
                bulkCopy.ColumnMappings.Add("Priority", "Priority");
                bulkCopy.ColumnMappings.Add("Quantity", "Qty");
                bulkCopy.ColumnMappings.Add("Status", "Status");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.Temp_ScheduleDetails_VEDH .", e.RowsCopied));
                };

                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
                result = getPumpModelImportIssueDetails();
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Logger.WriteErrorLog(string.Format("Exception in ProcessAlarmFile() method. Message :{0}", ex.ToString()));
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();

            }
            return result;
        }

        public static string getPumpModelImportIssueDetails()
        {
            string result = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand command = new SqlCommand("[dbo].[S_GetScheduleDetails_VEDH]", sqlConn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Param", "Import");
            command.Parameters.AddWithValue("@Status", "1");
            command.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["Username"].ToString());
            command.CommandTimeout = 360;
            try
            {
                sdr = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                result = "error";
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }
            return result;
        }
    }
}