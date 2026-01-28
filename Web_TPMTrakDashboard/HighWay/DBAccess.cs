using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.HighWay
{
    public class DBAccess
    {
        internal static bool IsSupervisor(string EmployeeID)
        {
            bool Isexists = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from employeeinformation where Employeeid=@Employeeid and EmployeeRole='Supervisor'", con);
                cmd.Parameters.AddWithValue("@Employeeid", EmployeeID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    Isexists = true;
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
            return Isexists;
        }
        internal static string GetEmployeeRole(string EmployeeID)
        {
            string Role = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from employeeinformation where Employeeid=@Employeeid", con);
                cmd.Parameters.AddWithValue("@Employeeid", EmployeeID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Role = Convert.IsDBNull(sdr["EmployeeRole"]) ? "" : sdr["EmployeeRole"].ToString();
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
            return Role;
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
        internal static List<string> GetComponentID(string MachineID)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                //cmd = new SqlCommand(@"select distinct componentid from componentoperationpricing where machineid=@machineid", con);
                cmd = new SqlCommand(@"select distinct componentid from InspectionTransactionDetails_Highway where machineid=@machineid", con);
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
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<string> GetOperationIDs(string MachineID, string Component)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                //cmd = new SqlCommand(@"select distinct operationno from componentoperationpricing where machineid=@machineid and componentid=@componentid", con);
                cmd = new SqlCommand(@"select distinct OperationNo from InspectionTransactionDetails_Highway where machineid=@machineid and componentid=@componentid", con);
                cmd.Parameters.AddWithValue("@machineid", MachineID);
                cmd.Parameters.AddWithValue("@componentid", Component);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["OperationNo"].ToString());
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
        #region-----------------------------Machine StartUp Master----------------------------------
        internal static List<MachineStartupChecksheetMasterData> GetMachineStartUpMasterData(string MachineID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<MachineStartupChecksheetMasterData> list = new List<MachineStartupChecksheetMasterData>();
            MachineStartupChecksheetMasterData data = null;
            try
            {
                cmd = new SqlCommand(@"S_GetMachineStartUpCheckSheetMaster_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Param", "MachineStartUpCheckSheet_View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new MachineStartupChecksheetMasterData();
                        //data.CharacteristicID = sdr["CharacteristicID"].ToString();
                        data.Description = sdr["Characteristics"].ToString();
                        data.PointsToBeChecked = sdr["PointsToBeChecked"].ToString();
                        data.DataType = sdr["DataType"].ToString();
                        if (!Convert.IsDBNull(sdr["SortOrder"]))
                        {
                            data.SortOrder = Convert.ToInt32(sdr["SortOrder"].ToString());
                        }
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
        internal static string SaveMachineStartupMasterData(MachineStartupChecksheetMasterData data, string MachineID)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetMachineStartUpCheckSheetMaster_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                //cmd.Parameters.AddWithValue("@CharacteristicID", data.CharacteristicID);
                cmd.Parameters.AddWithValue("@Characteristics", data.Description);
                cmd.Parameters.AddWithValue("@PointsToBeChecked", data.PointsToBeChecked);
                cmd.Parameters.AddWithValue("@DataType", data.DataType);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@Param", "MachineStartUpCheckSheet_Save");
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
        internal static string DeleteMachineStartupMasterData(MachineStartupChecksheetMasterData data, string MachineID)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetMachineStartUpCheckSheetMaster_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                //cmd.Parameters.AddWithValue("@CharacteristicID", data.CharacteristicID);
                cmd.Parameters.AddWithValue("@PointsToBeChecked", data.PointsToBeChecked);
                cmd.Parameters.AddWithValue("@Characteristics", data.Description);
                cmd.Parameters.AddWithValue("@Param", "MachineStartUpCheckSheet_Delete");
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
        internal static void ImportChecksheetMasterData(DataTable dt)
        {
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                // IDD MachineID   CharacteristicID Characteristics PointsToBeChecked DataType    SortOrder
                bulkCopy = new SqlBulkCopy(con);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[TempMachineStartUpCheckSheetMaster_Highway]";
                bulkCopy.ColumnMappings.Add("MachineID", "MachineID");
                bulkCopy.ColumnMappings.Add("CharacteristicID", "CharacteristicID");
                bulkCopy.ColumnMappings.Add("Characteristics", "Characteristics");
                bulkCopy.ColumnMappings.Add("PointsToBeChecked", "PointsToBeChecked");
                bulkCopy.ColumnMappings.Add("DataType", "DataType");
                bulkCopy.ColumnMappings.Add("SortOrder", "SortOrder");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.TempMachineStartUpCheckSheetMaster_Highway .", e.RowsCopied));
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
        internal static string ImportChecksheetData()
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetMachineStartUpCheckSheetMaster_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "Import");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = "Imported";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return success;
        }
        internal static string CopyChecksheetData(string source, string destination)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetMachineStartUpCheckSheetMaster_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OldMachineID", source);
                cmd.Parameters.AddWithValue("@MachineID", destination);
                cmd.Parameters.AddWithValue("@Param", "Copy");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["saveflag"].ToString();
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
        #endregion
        #region----------------------------Inspection Report Master---------------------------------------------
        internal static List<ListItem> GetRevID(string Component, string Operation)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                cmd = new SqlCommand(@"select distinct RevID,RevNo from InspectionMasterDetails_Highway where ComponentID=@ComponentID and OperationNo=@OperationNo order by RevID desc", con);
                //cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ListItem { Text = sdr["RevNo"].ToString(), Value = sdr["RevID"].ToString() });
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
        internal static string GetMaxRevID(string Component, string Operation)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string RevID = "";
            try
            {
                cmd = new SqlCommand(@"select Max(RevID) as RevID from InspectionMasterDetails_Highway where ComponentID=@ComponentID and OperationNo=@OperationNo", con);
                //cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        RevID = sdr["RevID"].ToString();
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
            return RevID;
        }
        internal static string GetMaxRevNo(string Component, string Operation, string RevID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string RevNo = "";
            try
            {
                cmd = new SqlCommand(@"select RevNo from InspectionMasterDetails_Highway where ComponentID=@ComponentID and OperationNo=@OperationNo and RevID=@RevID", con);
                //cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        RevNo = sdr["RevNo"].ToString();
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
            return RevNo;
        }
        internal static List<InspectionReportofShaftmasterData> GetInspectionMasterData(string Component, string Operation, string RevID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<InspectionReportofShaftmasterData> list = new List<InspectionReportofShaftmasterData>();
            InspectionReportofShaftmasterData data = null; int i = 1;
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@MachineID",MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                //cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@Param", "MasterView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new InspectionReportofShaftmasterData();
                        data.SlNo = i++;
                        data.BalNo = sdr["BalNo"].ToString();
                        data.CharacteristicID = sdr["CharacteristicID"].ToString();
                        data.Description = sdr["Characteristics"].ToString();
                        data.Specification = sdr["Specification"].ToString();
                        data.InspectionMethod = sdr["InspectionMethod"].ToString();
                        data.Frequency = sdr["Frequency"].ToString();
                        data.DocNo = sdr["DocNo"].ToString();
                        if (!(Convert.IsDBNull(sdr["Freq_Quantity"])))
                        {
                            data.FrequencyQty = Convert.ToSingle(sdr["Freq_Quantity"].ToString());
                        }
                        //data.RevDate =Convert.IsDBNull(sdr["RevDate"])?"":Convert.ToDateTime(sdr["RevDate"]).ToString("yyyy-MM-dd");
                        data.RevDate = sdr["RevDate"].ToString();
                        data.DataType = sdr["DataType"].ToString();
                        data.SortOrder = sdr["SortOrder"].ToString();
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
        internal static string SaveInspectionMasterData(InspectionReportofShaftmasterData data, string ComponenID, string Operation)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", ComponenID);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                cmd.Parameters.AddWithValue("@RevID", data.RevID);
                cmd.Parameters.AddWithValue("@RevNo", data.RevID);
                cmd.Parameters.AddWithValue("@Frequency", data.Frequency);
                cmd.Parameters.AddWithValue("@Freq_Qty", data.FrequencyQty);
                cmd.Parameters.AddWithValue("@CharacteristicID", data.CharacteristicID);
                cmd.Parameters.AddWithValue("@Characteristic", data.Description);
                cmd.Parameters.AddWithValue("@BalNo", data.BalNo);
                cmd.Parameters.AddWithValue("@Specification", data.Specification);
                cmd.Parameters.AddWithValue("@InspectionMethod", data.InspectionMethod);
                cmd.Parameters.AddWithValue("@RevDate", Convert.ToDateTime(data.RevDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@DocNo", data.DocNo);
                cmd.Parameters.AddWithValue("@DataType", data.DataType);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
        internal static string DeleteInspectionMasterData(InspectionReportofShaftmasterData data, string ComponenID, string Operation, string RevID)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", ComponenID);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.Parameters.AddWithValue("@CharacteristicID", data.CharacteristicID);
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
        internal static void SaveRevisionNo(string RevID, string ComponentID, string OperationNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Highway", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                //cmd.Parameters.AddWithValue("@RevID", Convert.ToInt32(RevID));
                cmd.Parameters.AddWithValue("@RevNo", RevID);
                cmd.Parameters.AddWithValue("@RevDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ComponentID", ComponentID);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.Parameters.AddWithValue("@Param", "Save_NewRevDetails");
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveRevisionNo: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }
        internal static string ImportInspectionData()
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "Import");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = "Imported";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return success;
        }
        internal static void ImportInspectionMasterData(DataTable dt)
        {
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                //IDD MachineID   ComponentID OperationNo CharacteristicID Characteristics BalNo Specification   InspectionMethod Frequency   RevID RevNo   RevDate DocNo   UpdatedTS Freq_Quantity   DataType SortOrder
                bulkCopy = new SqlBulkCopy(con);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[TempInspectionMasterDetails_Highway]";
                bulkCopy.ColumnMappings.Add("MachineID", "MachineID");
                bulkCopy.ColumnMappings.Add("ComponentID", "ComponentID");
                bulkCopy.ColumnMappings.Add("OperationNo", "OperationNo");
                bulkCopy.ColumnMappings.Add("CharacteristicID", "CharacteristicID");
                bulkCopy.ColumnMappings.Add("Characteristics", "Characteristics");
                bulkCopy.ColumnMappings.Add("BalNo", "BalNo");
                bulkCopy.ColumnMappings.Add("Specification", "Specification");
                bulkCopy.ColumnMappings.Add("InspectionMethod", "InspectionMethod");
                bulkCopy.ColumnMappings.Add("Frequency", "Frequency");
                bulkCopy.ColumnMappings.Add("RevID", "RevID");
                bulkCopy.ColumnMappings.Add("RevNo", "RevNo");
                bulkCopy.ColumnMappings.Add("RevDate", "RevDate");
                bulkCopy.ColumnMappings.Add("DocNo", "DocNo");
                bulkCopy.ColumnMappings.Add("UpdatedTS", "UpdatedTS");
                bulkCopy.ColumnMappings.Add("Freq_Quantity", "Freq_Quantity");
                bulkCopy.ColumnMappings.Add("DataType", "DataType");
                bulkCopy.ColumnMappings.Add("SortOrder", "SortOrder");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("Row insertion Notifed : {0} rows copied to Table dbo.TempInspectionMasterDetails_Highway .", e.RowsCopied));
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
        internal static string CopyInspectionData(string sourceComponent, string sourceOperation, string destinationcomponent, string destinationOperation)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"S_GetInspectionDetails_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OldComponentID", sourceComponent);
                cmd.Parameters.AddWithValue("@OldOperationNo", sourceOperation);
                cmd.Parameters.AddWithValue("@ComponentID", destinationcomponent);
                cmd.Parameters.AddWithValue("@OperationNo", destinationOperation);
                cmd.Parameters.AddWithValue("@Param", "CopyFromOldCO_To_NewCO");
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
        #endregion
        #region----------------------------Inspection Report Transaction---------------------------------------------
        internal static List<string> GetComponents_Machine(string MachineID)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                //cmd = new SqlCommand(@"select distinct componentid from componentoperationpricing where machineid=@machineid", con);
                cmd = new SqlCommand(@"select TOP 1 ComponentID from MachineStartUpCheckSheetTransaction_Highway where MachineID=@MachineID", con);
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
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<string> GetOperations_Machine(string MachineID, string Component)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                //cmd = new SqlCommand(@"select distinct operationno from componentoperationpricing where machineid=@machineid and componentid=@componentid", con);
                cmd = new SqlCommand(@"select TOP 1 OperationNo from MachineStartUpCheckSheetTransaction_Highway where MachineID=@MachineID and ComponentID=@ComponentID", con);
                cmd.Parameters.AddWithValue("@machineid", MachineID);
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
        internal static List<ListItem> GetShiftID()
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from shiftdetails where Running=1 order by shiftid asc", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(new ListItem { Text = sdr["ShiftName"].ToString(), Value = sdr["shiftid"].ToString() });
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
        internal static List<InspectionReportofShaftApprovalData> GetInspectionReportTransactionData(string MachineID, string Component, string OperationNo, string Shift, string Date, string DieNo, string HeatNo, int RevID, out string Result, out DataTable dt, out DataTable dt_Operator)
        {
            //            exec S_GetInspectionDetails_Highway @ComponentID = N'3900',@OperationNo = N'10',@HeatNo = N'222',@Revid = '12',
            //@DieNo = N'111',@Date = N'2024-01-01',@Shift = N'SECOND',@Param = N'TransactionView',@MachineID = 'M-1',@Param1 = 'ApproveView'
            List<InspectionReportofShaftApprovalData> list = new List<InspectionReportofShaftApprovalData>();
            List<InspectionReportofShaftApprovalData> list_Time = new List<InspectionReportofShaftApprovalData>();
            InspectionReportofShaftApprovalData data = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            Result = "";
            dt = new DataTable();
            dt_Operator = new DataTable();
            try
            {
                cmd = new SqlCommand("S_GetInspectionDetails_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.Parameters.AddWithValue("@HeatNo", HeatNo);
                cmd.Parameters.AddWithValue("@DieNo", DieNo);
                cmd.Parameters.AddWithValue("@Revid", Convert.ToInt32(RevID));
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Param", "ApproveView");
                sdr = cmd.ExecuteReader();
                int a = 1;
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                    dt_Operator.Load(sdr);
                    if (dt.Columns.IndexOf("SaveFlag") != -1)
                    {
                        Result = dt.Rows[0]["SaveFlag"].ToString();
                        return list;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        #region----Header-----
                        data = new InspectionReportofShaftApprovalData();
                        data.HeaderVisibility = true;
                        list_Time = new List<InspectionReportofShaftApprovalData>();
                        for (int i = 18; i < dt.Columns.Count; i++)
                        {
                            InspectionReportofShaftApprovalData li = new InspectionReportofShaftApprovalData();
                            var value = dt.Columns[i].ColumnName.ToString().Split('$');
                            li.TimeValue = "Time : " + value[0].ToString();
                            li.DieHeatValue = "Heat No :" + value[1].ToString() + "  " + "Die No :" + value[2].ToString();
                            li.HeaderVisibility = true;
                            list_Time.Add(li);
                        }
                        data.listofTime = list_Time;
                        list.Add(data);
                        #endregion---------------
                        #region -- Approve Details --
                        foreach (DataRow row in dt.Rows)
                        {
                            data = new InspectionReportofShaftApprovalData();
                            data.SlNo = a++;
                            data.BalNo = row["BalNo"].ToString();
                            data.Description = row["Characteristic"].ToString();
                            data.CharacteristicID = row["CharacteristicID"].ToString();
                            data.Specification = row["Specification"].ToString();
                            data.InspectionMethod = row["InspectionMethod"].ToString();
                            data.Frequency = row["Frequency"].ToString();
                            data.FrequencyQty = Convert.ToInt32(row["Freq_Qty"].ToString());
                            data.ContentVisibility = true;
                            list_Time = new List<InspectionReportofShaftApprovalData>();
                            for (int i = 18; i < dt.Columns.Count; i++)
                            {
                                InspectionReportofShaftApprovalData li = new InspectionReportofShaftApprovalData();
                                li.TimeValue = row[i].ToString();
                                li.ContentVisibility = true;
                                list_Time.Add(li);
                            }
                            data.ObservationsColSpan = list_Time.Count;
                            data.listofTime = list_Time;
                            list.Add(data);
                        }
                        #endregion
                        #region -- Footer --
                        data = new InspectionReportofShaftApprovalData();
                        data.FooterVisisbility = true;
                        if (dt_Operator != null && dt_Operator.Rows.Count > 0)
                        {
                            data.ProductionHOD = dt_Operator.Rows[0]["Production_HOD_ID"].ToString() != "" ? dt_Operator.Rows[0]["Production_HOD_ID"].ToString() + "-" + dt_Operator.Rows[0]["Production_HOD_TS"].ToString() : "";
                            data.QAHOD = dt_Operator.Rows[0]["QA_HOD_ID"].ToString() != "" ? dt_Operator.Rows[0]["QA_HOD_ID"].ToString() + "-" + dt_Operator.Rows[0]["QA_HOD_TS"].ToString() : "";
                            data.InspectedBy = dt_Operator.Rows[0]["InspectedBy"].ToString() != "" ? dt_Operator.Rows[0]["InspectedBy"].ToString() + "-" + dt_Operator.Rows[0]["InspectedTS"].ToString() : "";
                            if (Convert.IsDBNull(dt_Operator.Rows[0]["Production_HOD_ID"]) || dt_Operator.Rows[0]["Production_HOD_ID"].ToString() == "")
                            {
                                if (GetEmployeeRole(HttpContext.Current.Session["UserName"].ToString()) == "ProductionHOD")
                                {
                                    data.ProdButtonVisibility = true;
                                }
                            }
                            if (Convert.IsDBNull(dt_Operator.Rows[0]["QA_HOD_ID"]) || dt_Operator.Rows[0]["QA_HOD_ID"].ToString() == "")
                            {
                                if (GetEmployeeRole(HttpContext.Current.Session["UserName"].ToString()) == "QAHOD")
                                {
                                    data.QAButtonVisibility = true;
                                }
                            }
                            if (Convert.IsDBNull(dt_Operator.Rows[0]["InspectedBy"]) || dt_Operator.Rows[0]["InspectedBy"].ToString() == "")
                            {
                                if (GetEmployeeRole(HttpContext.Current.Session["UserName"].ToString()) == "QAInspector")
                                {
                                    data.InspectorButtonVisibility = true;
                                }
                            }
                            data.Remarks = "Remarks:- " + dt_Operator.Rows[0]["Remarks"].ToString();
                        }
                        else
                        {
                            data.Remarks = "Remarks:-";
                            if (GetEmployeeRole(HttpContext.Current.Session["UserName"].ToString()) == "ProductionHOD")
                            {
                                data.ProdButtonVisibility = true;
                            }
                            if (GetEmployeeRole(HttpContext.Current.Session["UserName"].ToString()) == "QAHOD")
                            {
                                data.QAButtonVisibility = true;
                            }
                            if (GetEmployeeRole(HttpContext.Current.Session["UserName"].ToString()) == "QAInspector")
                            {
                                data.InspectorButtonVisibility = true;
                            }
                            data.ButtonVisibility = true;
                        }
                        data.RemarksColspan = dt.Columns.Count - 10;
                        //data.InspectionColspan = Convert.ToInt32((dt.Columns.Count - 10) /3);
                        list.Add(data);
                        #endregion
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
        internal static string SaveInspectionApproveData(string MachineID, string Component, string OperationNo, string Shift, string Date, string DieNo, string HeatNo, string ProductionHOD, string QAHOD, string QAInspector, string Role)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string success = "";
            try
            {
                //                cmd = new SqlCommand(@"
                //if not exists(select * from InspectionApprovalDetails_Highway where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo 
                //and HeatCode=@HeatCode and DieNo=@DieNo and Date=@Date and Shift=@Shift)
                //begin
                //	insert into InspectionApprovalDetails_Highway(MachineID,ComponentID,OperationNo,HeatCode,DieNo,Date,Shift,OperatorID,Production_HOD_ID,
                //	Production_HOD_TS,QA_HOD_ID,QA_HOD_TS,Remarks,InspectedBy,InspectedTS)

                //	Values(@MachineID,@ComponentID,@OperationNo,@HeatCode,@DieNo,@Date,@Shift,@OperatorID,@Production_HOD_ID,@Production_HOD_TS,@QA_HOD_ID,@QA_HOD_TS,@Remarks,@InspectedBy,@InspectedTS)
                //	select 'Approved' as SaveFlag
                //end
                //else
                //begin
                //	update InspectionApprovalDetails_Highway set Production_HOD_ID=@Production_HOD_ID, Production_HOD_TS=@Production_HOD_TS, QA_HOD_ID=@QA_HOD_ID,QA_HOD_TS=@QA_HOD_TS,InspectedBy=@InspectedBy,InspectedTS=@InspectedTS
                //	where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and HeatCode=@HeatCode and DieNo=@DieNo and Date=@Date and Shift=@Shift
                //	select 'Approved' as SaveFlag
                //end
                //", con);
                cmd = new SqlCommand(@"if not exists(select * from InspectionApprovalDetails_Highway where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo 
and Date=@Date and Shift=@Shift)
begin
	insert into InspectionApprovalDetails_Highway(MachineID,ComponentID,OperationNo,Date,Shift,OperatorID,Production_HOD_ID,
	Production_HOD_TS,QA_HOD_ID,QA_HOD_TS,Remarks,InspectedBy,InspectedTS)

	Values(@MachineID,@ComponentID,@OperationNo,@Date,@Shift,@OperatorID,@Production_HOD_ID,@Production_HOD_TS,@QA_HOD_ID,@QA_HOD_TS,@Remarks,@InspectedBy,@InspectedTS)
	select 'Approved' as SaveFlag
end
else
begin
	if @Role='Inspector'
		Begin
			update InspectionApprovalDetails_Highway set InspectedBy=@InspectedBy,InspectedTS=@InspectedTS
			where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and Date=@Date and Shift=@Shift
			select 'Approved' as SaveFlag
		end
	if @Role='ProductionHOD'
		Begin
			update InspectionApprovalDetails_Highway set Production_HOD_ID=@Production_HOD_ID, Production_HOD_TS=@Production_HOD_TS
			where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and Date=@Date and Shift=@Shift
			select 'Approved' as SaveFlag
		end
	if @Role='QAHOD'
		Begin
			update InspectionApprovalDetails_Highway set QA_HOD_ID=@QA_HOD_ID,QA_HOD_TS=@QA_HOD_TS
			where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and Date=@Date and Shift=@Shift
			select 'Approved' as SaveFlag
		end
end", con);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                //cmd.Parameters.AddWithValue("@HeatCode", HeatNo);
                //cmd.Parameters.AddWithValue("@DieNo", DieNo);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@OperatorID", "");
                cmd.Parameters.AddWithValue("@InspectedBy", QAInspector);
                cmd.Parameters.AddWithValue("@InspectedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Production_HOD_ID", ProductionHOD);
                cmd.Parameters.AddWithValue("@Production_HOD_TS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@QA_HOD_ID", QAHOD);
                cmd.Parameters.AddWithValue("@QA_HOD_TS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Remarks", "");
                cmd.Parameters.AddWithValue("@Role", Role);
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
        #endregion
        #region-------------------------------------Machine Startup Transaction------------------------------------------
        internal static List<MachineStartupApprovalData> GetMachineStartupTransactionData(string Machine, string Component, string Operation, string Year, String Month, out DataTable dt, out string Message, out List<string> daylist, out List<string> shiftList, out DataTable dt_Operator, out DataTable dt_Supervisor)
        {
            List<MachineStartupApprovalData> list = new List<MachineStartupApprovalData>();
            MachineStartupApprovalData data = null;
            MachineStartupApprovalData innerData = null;
            List<MachineStartupApprovalData> temp_list = null;
            List<string> ListDate = new List<string>();
            dt = new DataTable();
            Message = "";
            DataTable dt_Remarks = new DataTable();
            dt_Operator = new DataTable();
            dt_Supervisor = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            int b = 1;
            daylist = new List<string>();
            shiftList = new List<string>();
            try
            {
                cmd = new SqlCommand("S_GetMachineStartUpCheckSheetTransaction_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", Machine);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                //cmd.Parameters.AddWithValue("@Date", "2024-01-02");
                cmd.Parameters.AddWithValue("@YearNo", Convert.ToInt32(Year));
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                int monthNumber = DateTime.ParseExact(Month, "MMM", CultureInfo.InvariantCulture).Month;
                cmd.Parameters.AddWithValue("@MonthNo", monthNumber);
                cmd.Parameters.AddWithValue("@Param", "MachineStartUpCheckSheetApproval_View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                    dt_Remarks.Load(sdr);
                    dt_Remarks.AcceptChanges();
                    dt_Operator.Load(sdr);
                    dt_Operator.AcceptChanges();
                    dt_Supervisor.Load(sdr);
                    dt_Supervisor.AcceptChanges();
                    int a = 0;
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Columns.IndexOf("SaveFlag") != -1)
                        {
                            Message = "NoRecordsFound";
                        }
                        else
                        {
                            foreach (DataRow dtRow in dt.Rows)
                            {
                                if (a == 0)
                                {
                                    data = new MachineStartupApprovalData();
                                    data.CharacteristicID = "Characteristic ID";
                                    data.Description = "Description";
                                    data.PointsToBeChecked = "Points To Be Checked";
                                    data.HeaderVisibility = true;
                                    data.BackgroundClass = "class-green";
                                    data.Rowspan = 2;
                                    temp_list = new List<MachineStartupApprovalData>();

                                    for (int j = 4; j < dt.Columns.Count; j++)
                                    {
                                        var day = dt.Columns[j].ColumnName.ToString().Split('_').ToList();
                                        daylist.Add(day[0]);
                                        shiftList.Add(day[1]);
                                    }
                                    daylist = daylist.Distinct().ToList();
                                    for (int c = 0; c < daylist.Count; c++)
                                    {
                                        innerData = new MachineStartupApprovalData();
                                        innerData.BackgroundClass = "class-green";
                                        innerData.DayHeader = daylist[c];
                                        data.Rowspan = 1;
                                        innerData.DateColspan = shiftList.Distinct().ToList().Count;
                                        innerData.HeaderVisibility = true;
                                        temp_list.Add(innerData);
                                    }
                                    data.TransactionData = temp_list;
                                    list.Add(data);
                                    data = new MachineStartupApprovalData();
                                    temp_list = new List<MachineStartupApprovalData>();
                                    for (int d = 0; d < shiftList.Count; d++)
                                    {
                                        innerData = new MachineStartupApprovalData();
                                        innerData.BackgroundClass = "class-green";
                                        data.Rowspan = 1;
                                        innerData.ShiftHeader = shiftList[d];
                                        innerData.Shiftvisibility = true;
                                        temp_list.Add(innerData);
                                    }
                                    shiftList = shiftList.Distinct().ToList();
                                    data.TransactionData = temp_list;
                                    list.Add(data);
                                }
                                data = new MachineStartupApprovalData();
                                temp_list = new List<MachineStartupApprovalData>();
                                DataTable dtCharacteristicID = dt.AsEnumerable().Where(x => x["CharacteristicID"].ToString().Equals(dtRow["CharacteristicID"].ToString(), StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                                data.SlNo = b++;
                                data.CharacteristicID = dtRow["CharacteristicID"].ToString();
                                data.Description = dtRow["Characteristics"].ToString();
                                data.PointsToBeChecked = dtRow["PointsToBeChecked"].ToString();
                                data.ContentVisibility = true;
                                for (int j = 4; j < dt.Columns.Count; j++)
                                {
                                    string columnName = dt.Columns[j].ColumnName.ToString();
                                    innerData = new MachineStartupApprovalData();
                                    innerData.BackgroundClass = "class-white-data";
                                    if (dtRow[dt.Columns[j].ColumnName].ToString().Equals("DONE", StringComparison.OrdinalIgnoreCase) || dtRow[dt.Columns[j].ColumnName].ToString().Equals("OK", StringComparison.OrdinalIgnoreCase))
                                    {
                                        innerData.ShiftValue = "&#10004;";
                                        innerData.innerDayValueClass = "green";
                                    }
                                    else if (dtRow[dt.Columns[j].ColumnName].ToString().Equals("DO", StringComparison.OrdinalIgnoreCase) || dtRow[dt.Columns[j].ColumnName].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                                    {
                                        innerData.ShiftValue = "&#10008;";
                                        innerData.innerDayValueClass = "red";
                                    }
                                    else
                                    {
                                        innerData.ShiftValue = dtRow[dt.Columns[j].ColumnName].ToString();
                                        innerData.innerDayValueClass = "black";
                                    }
                                    innerData.ContentVisibility = true;
                                    temp_list.Add(innerData);
                                }
                                data.Rowspan = 1;
                                data.TransactionData = temp_list;
                                list.Add(data);
                                a++;
                            }
                            #region---Operator Row-----
                            data = new MachineStartupApprovalData();
                            temp_list = new List<MachineStartupApprovalData>();
                            data.PointsToBeChecked = "Operator:";
                            data.ApproveVisibility = true;
                            data.ApprovalcolSpan = 3;
                            data.rowclass = "Operatortr";
                            if (dt_Operator != null && dt_Operator.Rows.Count > 0)
                            {
                                for (int k = 4; k < dt_Operator.Columns.Count; k++)
                                {
                                    innerData = new MachineStartupApprovalData();
                                    innerData.ShiftValue = dt_Operator.Rows[0][k].ToString();
                                    innerData.ContentVisibility = true;
                                    temp_list.Add(innerData);
                                }
                            }
                            data.TransactionData = temp_list;
                            list.Add(data);
                            #endregion
                            #region -- Footer Row --
                            data = new MachineStartupApprovalData();
                            temp_list = new List<MachineStartupApprovalData>();
                            data.PointsToBeChecked = "Supervisor:";
                            data.ApproveVisibility = true;
                            data.ApprovalcolSpan = 3;
                            data.rowclass = "approveRowtr";
                            if (dt_Supervisor != null && dt_Supervisor.Rows.Count > 0)
                            {
                                for (int k = 4; k < dt_Supervisor.Columns.Count; k++)
                                {
                                    innerData = new MachineStartupApprovalData();
                                    var values = dt_Supervisor.Columns[k].ColumnName.ToString().Split('_').ToList();
                                    innerData.DayHeader = values[0];
                                    innerData.ShiftHeader = values[1];
                                    innerData.ShiftValue = dt_Supervisor.Rows[0][k].ToString();
                                    innerData.ContentVisibility = true;
                                    var days = dt_Supervisor.Columns[k].ColumnName.Split('_').ToList();
                                    if (Convert.IsDBNull(dt_Supervisor.Rows[0][k]))
                                    {
                                        if (IsSupervisor(HttpContext.Current.Session["UserName"].ToString()))
                                        {
                                            if (Convert.ToInt32(Convert.ToDateTime(values[0]).ToString("dd")) <= DateTime.Now.Day)
                                                innerData.HeaderVisibility = true;
                                        }
                                        //if (Convert.ToInt32(Convert.ToDateTime(days[0]).ToString("MM")) <= DateTime.Now.Month)
                                        //    innerData.HeaderVisibility = true;
                                        //if (Convert.ToInt32(Convert.ToDateTime(days
                                        //    [0]).ToString("dd")) <= DateTime.Now.Day && Convert.ToInt32(Convert.ToDateTime(days[0]).ToString("MM")) == DateTime.Now.Month)
                                        //    innerData.HeaderVisibility = true;
                                    }
                                    temp_list.Add(innerData);
                                }
                            }
                            else
                            {
                                innerData = new MachineStartupApprovalData();
                                for (int k = 4; k < dt.Columns.Count; k++)
                                {
                                    innerData = new MachineStartupApprovalData();
                                    var values = dt.Columns[k].ColumnName.ToString().Split('_').ToList();
                                    innerData.DayHeader = values[0];
                                    innerData.ShiftHeader = values[1];
                                    innerData.ContentVisibility = true;
                                    var days = dt.Columns[k].ColumnName.Split('_').ToList();
                                    if (IsSupervisor(HttpContext.Current.Session["UserName"].ToString()))
                                    {
                                        if (Convert.ToInt32(Convert.ToDateTime(days[0]).ToString("MM")) <= DateTime.Now.Month)
                                            innerData.HeaderVisibility = true;
                                    }
                                    //if (Convert.ToInt32(Convert.ToDateTime(days[0]).ToString("MM")) <= DateTime.Now.Month)
                                    //    innerData.HeaderVisibility = true;
                                    //if (Convert.ToInt32(Convert.ToDateTime(days
                                    //    [0]).ToString("dd")) <= DateTime.Now.Day && Convert.ToInt32(Convert.ToDateTime(days[0]).ToString("MM")) == DateTime.Now.Month)
                                    //    innerData.HeaderVisibility = true;
                                    temp_list.Add(innerData);
                                }
                            }
                            data.TransactionData = temp_list;
                            list.Add(data);
                            #endregion
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
            return list;
        }
        internal static string ApproveMachineStartUpData(string Machine, string Component, string Operation, string shift, string Date, String Month, string Year)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string success = "";
            try
            {
                //                cmd = new SqlCommand(@"if not exists(select * from MachineStartUpCheckSheetApproval_Highway where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo 
                //and Date=@Date and SHift=@Shift and YearNo=@YearNo and MonthNo=@MonthNo)
                //begin
                //	insert into MachineStartUpCheckSheetApproval_Highway(MachineID,Date,Shift,SupervisorID,SupervisorTS,YearNo,MonthNo,ComponentID,OperationNo)
                //	Values(@MachineID,@Date,@Shift,@SupervisorID,@SupervisorTS,@YearNo,@MonthNo,@ComponentID,@OperationNo)
                //	select 'Approved' as SaveFlag
                //end
                //else
                //begin
                //	update MachineStartUpCheckSheetApproval_Highway set SupervisorID=@SupervisorID,SupervisorTS=@SupervisorTS
                //	where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and Date=@Date and SHift=@Shift and YearNo=@YearNo and MonthNo=@MonthNo
                //	select 'Approved' as SaveFlag
                //end", con);
                cmd = new SqlCommand(@"if not exists(select * from MachineStartUpCheckSheetApproval_Highway where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo 
and Date=@Date and SHift=@Shift)
begin
	insert into MachineStartUpCheckSheetApproval_Highway(MachineID,Date,Shift,SupervisorID,SupervisorTS,ComponentID,OperationNo)
	Values(@MachineID,@Date,@Shift,@SupervisorID,@SupervisorTS,@ComponentID,@OperationNo)
	select 'Approved' as SaveFlag
end
else
begin
	update MachineStartUpCheckSheetApproval_Highway set SupervisorID=@SupervisorID,SupervisorTS=@SupervisorTS
	where MachineID=@MachineID and ComponentID=@ComponentID and OperationNo=@OperationNo and Date=@Date and SHift=@Shift
	select 'Approved' as SaveFlag
end", con);
                cmd.Parameters.AddWithValue("@MachineID", Machine);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", Operation);
                //cmd.Parameters.AddWithValue("@YearNo", Convert.ToInt32(Year));
                //DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                //int monthNumber = DateTime.ParseExact(Month, "MMM", CultureInfo.InvariantCulture).Month;
                //cmd.Parameters.AddWithValue("@MonthNo", monthNumber);
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@SupervisorID", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@SupervisorTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
        #endregion
        internal static string GetMachineName(string MachineID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string MachineName = "";
            try
            {
                cmd = new SqlCommand(@"select description from machineinformation where machineid=@machineid", con);
                cmd.Parameters.AddWithValue("@machineid", MachineID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MachineName = sdr["description"].ToString();
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
            return MachineName;
        }
        internal static string GetComponentName(string ComponnetiD)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string MachineName = "";
            try
            {
                cmd = new SqlCommand(@"select description from componentoperationpricing where componentid=@componentid", con);
                cmd.Parameters.AddWithValue("@componentid", ComponnetiD);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MachineName = sdr["description"].ToString();
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
            return MachineName;
        }
        #region-------------------------------------------------------ANDON-----------------------------------------------------
        internal static string GetCurrentShift()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string CurrentShift = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[s_GetCurrentShift]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CurrentShift = (rdr["ShiftName"].ToString());
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
            return CurrentShift;
        }
        internal static DataTable GetAndonSettingData(string DeviceName)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"IF EXISTS (SELECT * FROM AndonDefaults WHERE [User]=@User AND Parameter='HighwayAndon')
BEGIN
	SELECT * FROM AndonDefaults WHERE [User]=@User AND Parameter='HighwayAndon'
END
ELSE
BEGIN
	SELECT * FROM AndonDefaults WHERE [User]='Master' AND Parameter='HighwayAndon'
END", con);
                cmd.Parameters.AddWithValue("@User", DeviceName);
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
        internal static string SaveAndonSettings_Highway(string Parameter, string ValueinText, string ValueinText2)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS(SELECT * FROM AndonDefaults WHERE Parameter=@Parameter and ValueInText=@ValueInText)
BEGIN
	INSERT INTO AndonDefaults(Parameter,ValueInText,ValueInText2)
	VALUES(@Parameter,@ValueInText,@ValueInText2)
	SELECT 'INSERTED' AS SAVEFLAG
END
ELSE
BEGIN
	UPDATE AndonDefaults SET ValueInText2=@ValueInText2 WHERE Parameter=@Parameter and ValueInText=@ValueInText
	SELECT 'UPDATED' AS SAVEFLAG
END", con);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@ValueInText", ValueinText);
                cmd.Parameters.AddWithValue("@ValueInText2", ValueinText2);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SAVEFLAG"].ToString();
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
            #endregion
        }
        internal static DataTable GetAndonData_Highway(string Date, string Shift, string MAchine, string Plant, string cell, out DataTable dtHeader, out DataTable dtDown)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            dtHeader = new DataTable();
            dtDown = new DataTable();
            try
            {
                cmd = new SqlCommand(@"s_GetShiftAndHourWise_AndonDetails_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ShiftName", Shift);
                cmd.Parameters.AddWithValue("@MachineID", MAchine);
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                cmd.Parameters.AddWithValue("@GroupID", cell);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dtHeader.Load(sdr);
                    dtHeader.AcceptChanges();
                    dtDown.Load(sdr);
                    dtDown.AcceptChanges();
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

        internal static string GetCellID()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            string cell = "";
            try
            {
                cmd = new SqlCommand(@"select distinct GroupID from PlantMachineGroups", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        cell = sdr["GroupID"].ToString();
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
            return cell;
        }
        internal static DataTable GetDownTimeData()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"", con);
                cmd.Parameters.AddWithValue("", "");
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
        #region-------------------------Traceability---------------------------------------
        internal static List<string> GetPlantIDs()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct PlantID from PlantInformation", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["PlantID"].ToString());
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
        internal static List<string> GetCellIds(string PlantID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct GroupID from PlantMachineGroups where (PlantID=@PlantID OR ISNULL(@PlantID,'')='')", con);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
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
        internal static List<string> GetSerialNo(string ComponentID,DateTime FromDate,DateTime ToDate)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                //cmd = new SqlCommand(@"select distinct SerialNo from ScannedInfo_Highway where ComponentId in (select item from splitstrings(@componentid,',')) or isnull(@componentid,'')=''", con);
                cmd = new SqlCommand(@"select distinct SerialNo from ScannedInfo_Highway where (ComponentId in (select item from splitstrings(@componentid,',')) or isnull(@componentid,'')='')
and (convert(date,ScannedTS)>=@fromDate and convert(date,ScannedTS)<=@ToDate)", con);
                cmd.Parameters.AddWithValue("@ComponentId", ComponentID);
                cmd.Parameters.AddWithValue("@fromDate",Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate",Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
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
                if (con != null) con.Close();
            }
            return list;
        }
        internal static DataTable GetTrackingDashboardData(string Plant, string Cell, string Comp, string SerialNO, string FromDate, string ToDate)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetSerialNoTracebilityFromAutodata_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantId", Plant);
                cmd.Parameters.AddWithValue("@GroupId", Cell);
                cmd.Parameters.AddWithValue("@ComponentId", Comp);
                cmd.Parameters.AddWithValue("@SerialNo", SerialNO);
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Param", "TracebilityFromAutodata");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
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
        internal static DataTable GetSerialNoTrackingDashboardData(string Plant, string Cell, string Comp, string SerialNO, string FromDate, string ToDate)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetSerialNoTracebilityFromAutodata_Highway", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantId", Plant);
                cmd.Parameters.AddWithValue("@GroupId", Cell);
                cmd.Parameters.AddWithValue("@ComponentId", Comp);
                cmd.Parameters.AddWithValue("@SerialNo", SerialNO);
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Param", "ScannedInfoDashboard");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
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
    }
}