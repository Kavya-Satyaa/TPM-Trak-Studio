using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public class DBAccess
    {
        #region ------ Andon Setting ---------
        internal static void SaveSettings(string Parameter, string valueIntText, string valueInText2, string valueInInt, string datatype)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "";
            switch (datatype)
            {
                case "int":
                    query = @" if not exists (select * from [ShopDefaults] where Parameter = @Parameter and ValueInText = @ValueInText)
								BEGIN
								    Insert into [ShopDefaults](Parameter,ValueInText,ValueInInt,UpdatedTS)
								    Values (@Parameter,@ValueInText,@ValueInInt,@UpdatedTS)
							    END
							    ELSE
                                BEGIN
								     UPDATE  [ShopDefaults] Set ValueInInt=@ValueInInt,UpdatedTS=@UpdatedTS where Parameter=@Parameter and ValueInText=@ValueInText 
								END";
                    break;

                case "string":
                    query = @" if not exists (select * from [ShopDefaults] where Parameter = @Parameter and ValueInText = @ValueInText)
								BEGIN
								    Insert into [ShopDefaults](Parameter,ValueInText,ValueInText2,UpdatedTS)
								    Values (@Parameter,@ValueInText,@ValueInText2,@UpdatedTS)
							    END
							    ELSE
                                BEGIN
								     UPDATE  [ShopDefaults] Set  ValueInText2=@ValueInText2,UpdatedTS=@UpdatedTS where Parameter=@Parameter and ValueInText=@ValueInText 
								END";
                    break;
            }
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@ValueInText", valueIntText);
                cmd.Parameters.AddWithValue("@ValueInInt", valueInInt);
                cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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

        internal static SettingsViewEntityStore ViewSettingsDetails(string CellID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            if (conn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            SettingsViewEntityStore settings = new SettingsViewEntityStore();
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from AndonDefaults where Parameter='StoreAndonSetting' and CellID=@cellID)
                begin
                select parameter,ValueInText,ValueInText2,ValueInInt,'False' as ValueInBool,'Left' as TextAlign,'15' as LabelFontSize,'16' as DataFontSize,CellID from AndonDefaults where parameter='StoreAndonSetting' and CellID='Master'
                end
                else
                begin
                select * from AndonDefaults where Parameter='StoreAndonSetting' and CellID=@cellID order by ValueInInt
                end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@cellID", CellID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("MachineNameFontSize"))
                        {
                            settings.MachineFontSize = (sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("HeaderFontSize"))
                        {
                            settings.KTAHeaderFontSize = (sdr["LabelFontSize"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("ContentFontSize"))
                        {
                            settings.KTAContentFontSize = (sdr["DataFontSize"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return settings;
        }

        internal static int SaveRootFolderDetails(string path,string FolderTpe)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int success = 0;
            try
            {
                cmd = new SqlCommand(@"if not exists (select * from FolderPathDefinition where FolderType=@FolderTpe)
								BEGIN
							    Insert into FolderPathDefinition(FolderType,FolderPath)
								    Values (@FolderTpe,@FolderPath)
							    END
							    ELSE
                                BEGIN
								     UPDATE  FolderPathDefinition Set FolderPath=@FolderPath where FolderType=@FolderTpe
								END", conn);
                cmd.Parameters.AddWithValue("@FolderPath", path);
                cmd.Parameters.AddWithValue("@FolderTpe", FolderTpe);
                success= cmd.ExecuteNonQuery();
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
            return success;
        }
      
        internal static string GetRootPath(string folderType)
        {
            string Path = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("select FolderPath from FolderPathDefinition where FolderType=@FolderType", conn);
                cmd.Parameters.AddWithValue("@FolderType", folderType);
                rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    Path = rdr["FolderPath"].ToString();
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
            return Path;
        }

        internal static List<SettingsEntity> GetSettingsData()
        {
            List<SettingsEntity> styling = new List<SettingsEntity>();
            SettingsEntity settings = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("select * from ShopDefaults where Parameter='KTAWebAndon'", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    settings = new SettingsEntity();
                    while (rdr.Read())
                    {
                        switch (rdr["ValueInText"].ToString())
                        {
                            case "Header Font Size":
                                settings.HeaderFontSize = Convert.ToInt32(rdr["ValueInInt"]);
                                break;
                            case "Content Font Size":
                                settings.ContentFontSize = Convert.ToInt32(rdr["ValueInInt"]);
                                break;
                            case "Flip Time":
                                settings.FlipInterval = Convert.ToInt32(rdr["ValueInInt"]);
                                break;
                            case "Top DownCode":
                                settings.TopDownCode = Convert.ToInt32(rdr["ValueInInt"]);
                                break;
                            case "Display Type":
                                settings.DisplayType = rdr["ValueInText2"].ToString();
                                break;
                        }
                    }
                    styling.Add(settings);
                }
                else
                {
                    settings = new SettingsEntity();
                    settings.HeaderFontSize = 17;
                    settings.ContentFontSize = 14;
                    settings.FlipInterval = 20;
                    settings.TopDownCode = 5;
                    settings.DisplayType = "Horizontal View";
                    styling.Add(settings);
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
            return styling;
        }
        #endregion

        #region --------- Andon ---------
        internal static MachineEntity GetAndonData(string plantID, string cellID, string shiftStart, string shiftEnd, out DataTable TotalTimeDataTable)
        {

            MachineEntity machineEntitylist = new MachineEntity();
            List<ComponentData> ComponentList = new List<ComponentData>();
            List<TargetData> TargList = new List<TargetData>();
            List<DownCodeData> downCodesList = new List<DownCodeData>();
            TotalTimeDataTable = new DataTable();
            string MachineID = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_Get_MachinewiseAndon_KTASpindle", conn);
                cmd.Parameters.AddWithValue("@Plantid", plantID);
                cmd.Parameters.AddWithValue("@GroupID", cellID == "CellAll" ? "" : cellID);
                cmd.Parameters.AddWithValue("@StartTime", shiftStart);
                cmd.Parameters.AddWithValue("@EndTime", shiftEnd);
                cmd.Parameters.AddWithValue("@Param", "Machinewise");
                cmd.CommandType = CommandType.StoredProcedure;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ComponentData data = new ComponentData();
                    data.MachineID = rdr["MachineID"].ToString();
                    data.AE = rdr["AvailabilityEfficiency"].ToString();
                    data.OEE = rdr["OverallEfficiency"].ToString();
                    data.PE = rdr["ProductionEfficiency"].ToString();
                    data.QE = rdr["QualityEfficiency"].ToString();
                    data.Component = rdr["LastCycleCO"].ToString();
                    data.ActualCount = rdr["Components"].ToString();
                    data.Operation = rdr["LastCycleOperation"].ToString();
                    data.Operator = rdr["OperatorName"].ToString();
                    data.LastCycleEnd = rdr["LastCycletime"].ToString();
                    data.UtilizedTime = rdr["NetUtilisedTime"].ToString();
                    data.DownTime = rdr["DownTime"].ToString();
                    ComponentList.Add(data);
                }
                rdr.NextResult();
                while (rdr.Read())
                {
                    TargetData targetData = new TargetData();
                    targetData.MachineID = rdr["MachineID"].ToString();
                    if (rdr["HourID"] != null)
                    {
                        targetData.Hr = Convert.ToInt16(rdr["HourID"]);
                    }
                    if (rdr["Actual"] != null)
                    {
                        targetData.ActualQty = Convert.ToDouble(rdr["Actual"]);
                    }
                    TargList.Add(targetData);
                }
                rdr.NextResult();
                while (rdr.Read())
                {
                    downCodesList.Add(new DownCodeData { MachineID = rdr["MachineID"].ToString(), DownCode = rdr["DownId"].ToString(), Time = rdr["NetDownTime"].ToString() });
                }
                rdr.NextResult();
                TotalTimeDataTable.Load(rdr);
                machineEntitylist.CompList = ComponentList;
                machineEntitylist.TargetList = TargList;
                machineEntitylist.DownList = downCodesList;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return machineEntitylist;
        }       

        internal static string GetShiftstart(out string shiftEnd)
        {
            shiftEnd = "";
            string Shiftstart = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("s_GetCurrentShift", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Shiftstart = rdr["StartTime"].ToString();
                        shiftEnd = rdr["EndTime"].ToString();
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
            return Shiftstart;
        }
        #endregion

        #region --------- Schedule Master -------
        internal static List<string> getCellAssignedForEmployee(string Plant, string employeeID)
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct GroupID from PlantCellEmployeeAssociation_KTA where (PlantID = @PlantID or isnull(@PlantID,'')='') and EmployeeID=@EmployeeID", conn);
                cmd.Parameters.AddWithValue("@PlantID", Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr["GroupID"].ToString());
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
        internal static List<ListItem> getMachineIdInterfaceIDByPlantCell(string Plant, string cell, bool machineInterfaceIDRequired)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct m.machineid,m.InterfaceID  from PlantMachineGroups pmg inner join  machineinformation m on pmg.MachineID=m.machineid where (pmg.PlantID = @PlantID or isnull(@PlantID,'')='') and pmg.GroupID in (" + cell + ")", conn);
                cmd.Parameters.AddWithValue("@PlantID", Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ListItem data = new ListItem();
                        data.Text = rdr["machineid"].ToString();
                        if (machineInterfaceIDRequired)
                        {
                            data.Value = rdr["InterfaceID"].ToString();
                        }
                        else
                        {
                            data.Value = rdr["machineid"].ToString();
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<ListItem> getComponentIDWithInterface(string machineID)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct c.componentid,c.InterfaceID from componentinformation c inner join componentoperationpricing cop on c.componentid=cop.componentid where  cop.machineid=@MachineID", conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ListItem data = new ListItem();
                        data.Text = rdr["componentid"].ToString();
                        data.Value = rdr["InterfaceID"].ToString();
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
        internal static List<ListItem> getOperationWithInterface(string machineID, string compID)
        {
            List<ListItem> list = new List<ListItem>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct operationno,InterfaceID from componentoperationpricing where machineid=@machineid and componentid=@componentid", conn);
                cmd.Parameters.AddWithValue("@machineid", machineID);
                cmd.Parameters.AddWithValue("@componentid", compID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ListItem data = new ListItem();
                        data.Text = rdr["operationno"].ToString();
                        data.Value = rdr["InterfaceID"].ToString();
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
        internal static List<ScheduleMasterEntity> getScheduleMasterDetails(string plant, string cell, string machine, string compIDSearch1, string compIDSearch2, string compDescSearch1, string compaDescSearch2, string status, string fromDate, string toDate)
        {
            List<ScheduleMasterEntity> list = new List<ScheduleMasterEntity>();
            ScheduleMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", string.IsNullOrEmpty(fromDate) ? "" : Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", string.IsNullOrEmpty(toDate) ? "" : Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@Groupid", cell);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@Comp1", compIDSearch1);
                cmd.Parameters.AddWithValue("@Comp2", compIDSearch2);
                cmd.Parameters.AddWithValue("@Des1", compDescSearch1);
                cmd.Parameters.AddWithValue("@Des2", compaDescSearch2);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@param", "View");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {

                    while (rdr.Read())
                    {
                        data = new ScheduleMasterEntity();
                        data.IDD = rdr["AutoID"].ToString();
                        data.ScheduleDateTime = string.IsNullOrEmpty(rdr["ScheduleDate"].ToString()) ? "" : Util.GetDateTime(rdr["ScheduleDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        data.ScheduleDate = string.IsNullOrEmpty(rdr["ScheduleDate"].ToString()) ? "" : Util.GetDateTime(rdr["ScheduleDate"].ToString()).ToString("dd-MM-yyyy");
                        data.Plant = rdr["PlantID"].ToString();
                        data.Cell = rdr["GroupID"].ToString();
                        data.Machine = rdr["MachineID"].ToString();
                        data.MachineInterfaceID = rdr["MCinterface"].ToString();
                        data.Component = rdr["ComponentID"].ToString();
                        data.Operation = rdr["OperationNo"].ToString();
                        data.CompInterfaceId = rdr["CompInterface"].ToString();
                        data.OpnInterfaceId = rdr["OpnInterface"].ToString();
                        data.CompDesc = rdr["CompDescription"].ToString();
                        data.Status = rdr["Status"].ToString();
                        if (data.Status == "1")
                        {
                            data.Status = "Closed";
                            data.PriorityReadOnly = true;
                        }
                        else if (data.Status == "2")
                        {
                            data.Status = "New";
                        }
                        else if (data.Status == "3")
                        {
                            data.Status = "Running";
                            data.PriorityReadOnly = true;
                        }
                        data.Priorityno = rdr["PriorityNo"].ToString();
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
        internal static string saveScheduleMasterDetails(ScheduleMasterEntity data)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@McInterface", data.MachineInterfaceID);
                cmd.Parameters.AddWithValue("@CompInterface", data.CompInterfaceId);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OprInterface", data.OpnInterfaceId);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@UpdatedBY", data.UpdatedBy);
                cmd.Parameters.AddWithValue("@Priority", data.Priorityno);
                cmd.Parameters.AddWithValue("@param", "Save");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        result = rdr["Result"].ToString();
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
            return result;
        }
        internal static int updateScheduleMasterDetails(ScheduleMasterEntity data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                if (data.Status == "Closed")
                {
                    data.Status = "1";
                }
                else if (data.Status == "New")
                {
                    data.Status = "2";
                }
                else if (data.Status == "Running")
                {
                    data.Status = "3";
                }
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@param", "Update");
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
        internal static int deleteScheduleMasterDetails(ScheduleMasterEntity data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[SP_ScheduleDetailsSaveAndView_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                if (data.Status == "Closed")
                {
                    data.Status = "1";
                }
                else if (data.Status == "New")
                {
                    data.Status = "2";
                }
                else if (data.Status == "Running")
                {
                    data.Status = "3";
                }
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@param", "Delete");
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

        internal static bool UpdateStoreSchedulePriorityKTA(ScheduleMasterEntity data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            bool result = false;
            string query = @"if exists (select * from scheduleDetails_KTA where CAST(ScheduleDate AS DATE)=CAST(@Scheduledate AS DATE) and MachineID=@Machineid and ComponentID=@ComponentID and OperationNo=@OperatioNo  and PlantID=@PlantID and GroupID=@Groupid)
begin
update scheduleDetails_KTA set PriorityNo=@Priority  where CAST(ScheduleDate AS DATE)=CAST(@Scheduledate AS DATE) and MachineID=@Machineid and ComponentID=@ComponentID and OperationNo=@OperatioNo  and PlantID=@PlantID and GroupID=@Groupid
end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Scheduledate", Util.GetDateTime(data.ScheduleDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Machineid", data.Machine);
                cmd.Parameters.AddWithValue("@ComponentID", data.Component);
                cmd.Parameters.AddWithValue("@OperatioNo", data.Operation);
                cmd.Parameters.AddWithValue("@PlantID", data.Plant);
                cmd.Parameters.AddWithValue("@Groupid", data.Cell);
                cmd.Parameters.AddWithValue("@Priority", data.Priorityno);
                result = cmd.ExecuteNonQuery() > 0 ? true : false;

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }
        #endregion


        #region --------- Cell Employee Association -------

        internal static DataTable getCellEmployeeAssociationDetails(string Plant)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[AssignPlantCellEmployee_kta]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", Plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : Plant);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getCellEmployeeAssociationDetails = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return dt;
        }
        internal static int insertDeleteEmployeeCellAssociation(string Plant, string cell, string employeeID, string param)
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
                    query = @"if not exists (select * from PlantCellEmployeeAssociation_KTA where PlantID=@PlantID and GroupID=@GroupID and EmployeeID=@EmployeeID)
begin
	insert into PlantCellEmployeeAssociation_KTA(PlantID,GroupID,EmployeeID,UpdatedTS) values(@PlantID,@GroupID,@EmployeeID,@UpdatedTS)
end";
                }
                else if (param.Equals("delete", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"delete from PlantCellEmployeeAssociation_KTA where PlantID=@PlantID and GroupID=@GroupID and EmployeeID=@EmployeeID";
                }
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", Plant);
                cmd.Parameters.AddWithValue("@GroupID", cell);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertDeleteEmployeeCellAssociation = " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return result;
        }
        #endregion

        #region ------ Schedule Master ---------
        internal static List<ScheduleMasterEntity> getPOScheduleMasterDetails(string workOrderSearch, string compSearch, string status)
        {
            List<ScheduleMasterEntity> list = new List<ScheduleMasterEntity>();
            ScheduleMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"", conn);
                cmd.Parameters.AddWithValue("@Machine", workOrderSearch);
                cmd.Parameters.AddWithValue("@Machine", compSearch);
                cmd.Parameters.AddWithValue("@Machine", status);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {

                    while (rdr.Read())
                    {
                        data = new ScheduleMasterEntity();
                        data.WorkOrder = rdr["machineid"].ToString();
                        data.WorkOrderDate = rdr["componentid"].ToString();
                        data.Quantity = rdr["operationno"].ToString();
                        data.Component = rdr["CompInterface"].ToString();
                        data.Status = rdr["OpnInterface"].ToString();
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
        internal static List<string> getComponentID()
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct componentid from componentoperationpricing", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {

                    while (rdr.Read())
                    {
                        list.Add(rdr["componentid"].ToString());
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
        internal static string GetOperationDesc(string Component, string operation)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string OpDesc = string.Empty;
            try
            {
                SqlCommand cmd = null;
                cmd = new SqlCommand(@"select DISTINCT description from componentoperationpricing where componentId=@ComponentID and operationno=@OperationNo", sqlConn);
                cmd.Parameters.AddWithValue("@ComponentID", Component);
                cmd.Parameters.AddWithValue("@OperationNo", operation);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    OpDesc = rdr["description"].ToString();
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
            return OpDesc;
        }
        internal static List<ScheduleMasterEntity> getPOScheduleRoutingDetails(string workOrder, string component)
        {
            List<ScheduleMasterEntity> list = new List<ScheduleMasterEntity>();
            ScheduleMasterEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"", conn);
                cmd.Parameters.AddWithValue("@Machine", workOrder);
                cmd.Parameters.AddWithValue("@Machine", component);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {

                    while (rdr.Read())
                    {
                        data = new ScheduleMasterEntity();
                        data.WorkOrder = rdr["machineid"].ToString();
                        data.Machine = rdr["componentid"].ToString();
                        data.Component = rdr["CompInterface"].ToString();
                        data.CompInterfaceId = rdr["operationno"].ToString();
                        data.Operation = rdr["operationno"].ToString();
                        data.OpnInterfaceId = rdr["operationno"].ToString();
                        data.Quantity = rdr["operationno"].ToString();
                        data.IsAssigned = rdr["operationno"].ToString() == "1" ? true : false;
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
        internal static string insertUpdatePOScheduleMasterDetails(ScheduleMasterEntity data, string param1)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string success = "";
            try
            {
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", data.WorkOrder);
                cmd.Parameters.AddWithValue("@Componentid", Convert.ToInt32(data.WorkOrderDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Description", data.Quantity);
                cmd.Parameters.AddWithValue("@Customerid", data.Component);
                cmd.Parameters.AddWithValue("@Param", "Save");
                cmd.Parameters.AddWithValue("@Param1", param1);
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
                Logger.WriteErrorLog("insertUpdatePOScheduleMasterDetails: " + ex.Message);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return success;
        }
        internal static int deleteRoutingDetails(string workOrder, string compoent)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            int success = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", workOrder);
                cmd.Parameters.AddWithValue("@Customerid", compoent);
                success = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("deleteRoutingDetails: " + ex.Message);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return success;
        }
        internal static string insertUpdatePOScheduleRoutingDetails(ScheduleMasterEntity data, string param1)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string success = "";
            try
            {
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", data.WorkOrder);
                cmd.Parameters.AddWithValue("@Customerid", data.Machine);
                cmd.Parameters.AddWithValue("@Customerid", data.Component);
                cmd.Parameters.AddWithValue("@Customerid", data.CompInterfaceId);
                cmd.Parameters.AddWithValue("@Customerid", data.Operation);
                cmd.Parameters.AddWithValue("@Customerid", data.OpnInterfaceId);
                cmd.Parameters.AddWithValue("@Description", data.Quantity);

                cmd.Parameters.AddWithValue("@Param", "Save");
                cmd.Parameters.AddWithValue("@Param1", param1);
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
                Logger.WriteErrorLog("insertUpdatePOScheduleRoutingDetails: " + ex.Message);
            }
            finally
            {
                if (con != null) { con.Close(); }
                if (sdr != null) { sdr.Close(); }
            }
            return success;
        }
        #endregion

        #region ---DocumentView---

        internal static List<string> GetAllComponentsFromSchedule()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            List<string> complist = new List<string>();
            string query = @"Select distinct scheduledate, componentid as BussinessID, CompInterface as InterfaceID from scheduleDetails_KTA  where Status in ('2','3') order by ScheduleDate asc";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    complist.Add(rdr["BussinessID"].ToString());
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message.ToString());
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return complist;
        }
        #endregion

        #region -- Update store Component--
        internal static List<NextComponentEntity> GetNextComponentOperationData(string plant, string CellID, string machineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<NextComponentEntity> list = new List<NextComponentEntity>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_GetScheduleDetails_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                cmd.Parameters.AddWithValue("@Cell", CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : CellID);
                cmd.Parameters.AddWithValue("@MachineID", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                cmd.Parameters.AddWithValue("@Param", "View");
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    NextComponentEntity entity = new NextComponentEntity();
                    entity.PlantID = sdr["PlantID"].ToString();
                    entity.GroupID = sdr["GroupID"].ToString();
                    entity.Machineid = sdr["MachineID"].ToString();
                    entity.ComponentID = sdr["ComponentID"].ToString();
                    entity.OperationNo = sdr["OperationNo"].ToString();
                    entity.ComponentDesc = sdr["ComponentDescription"].ToString();
                    entity.Status = sdr["Status"].ToString();
                    if (!string.IsNullOrEmpty(sdr["PriorityNo"].ToString()) && sdr["PriorityNo"].ToString()!= "0")
                    {
                        entity.Priority = sdr["PriorityNo"].ToString();
                    }
                    if (!string.IsNullOrEmpty(sdr["UpdateStore"].ToString()))
                    {
                        entity.NextComponent = Convert.ToBoolean(sdr["UpdateStore"].ToString());
                    }
                    list.Add(entity);
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

        internal static int SaveNextPartSelectionDetails(NextComponentEntity data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetScheduleDetails_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", data.PlantID);
                cmd.Parameters.AddWithValue("@Cell", data.GroupID);
                cmd.Parameters.AddWithValue("@MachineID", data.Machineid);
                cmd.Parameters.AddWithValue("@Component", data.ComponentID);
                cmd.Parameters.AddWithValue("@Operation", data.OperationNo);
                cmd.Parameters.AddWithValue("@PriorityNo", data.Priority);
                cmd.Parameters.AddWithValue("@UpdateStore", data.NextComponent);
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@Param", "Update");
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
        #endregion

        #region  -- PartFamily--

        internal static DataTable GetPartFamilyDetails(string PartFam)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            string Query = string.Empty;
            if(string.IsNullOrEmpty(PartFam))
            {
                Query = @"SELECT * FROM PartFamilyMaster_KTA";
            }
            else
            {
                Query = @"SELECT * FROM PartFamilyMaster_KTA where PartFamily like '" + PartFam + "%'";
            }
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        internal static int SavePartFamilyMasterDetails(string PartFamilyID,string PartFamilyDesc, string ParamNewEdit, string Param)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "";
            int qcount = 0;
            if (Param.Equals("Save",StringComparison.OrdinalIgnoreCase))
            {
                switch(ParamNewEdit)
                {
                    case "New":
                        query = @"if not exists (select * from PartFamilyMaster_KTA where PartFamily = @PartFam)
								BEGIN
								    Insert into PartFamilyMaster_KTA(PartFamily,Description)
								    Values (@PartFam,@Description)
							    END";
                        break;
                    case "Edit":
                        query = @"if exists (select * from PartFamilyMaster_KTA where PartFamily = @PartFam)
                                BEGIN
								     UPDATE  PartFamilyMaster_KTA Set Description=@Description where PartFamily = @PartFam 
								END";
                        break;
                }
            }
            else
            {
                query = @"delete from PartFamilyMaster_KTA where PartFamily = @PartFam";
            }
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PartFam", PartFamilyID);
                cmd.Parameters.AddWithValue("@Description", PartFamilyDesc);
                qcount=cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return qcount;
        }

        #endregion

        #region --Inspection Transaction---
        internal static DataTable GetInspectionTransactionDetails(string machine, string Component, string Operation, DateTime Date, string Shift,out DataTable ApprovedDt,out DataTable QtyDetailsDt)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            ApprovedDt = new DataTable();
            QtyDetailsDt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"[S_GetInspectionDetails_KTA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Component", Component);
                cmd.Parameters.AddWithValue("@Operation", Operation);
                cmd.Parameters.AddWithValue("@Date", Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Shift", Shift.Equals("All",StringComparison.OrdinalIgnoreCase)?"":Shift);
                cmd.Parameters.AddWithValue("@Param", "View");

                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                dt.AcceptChanges();
                ApprovedDt.Load(rdr);
                ApprovedDt.AcceptChanges();
                QtyDetailsDt.Load(rdr);
                QtyDetailsDt.AcceptChanges();

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


        internal static string SaveInspectionTransactionDetails(string machine, string Component, string Operation, DateTime Date, string Shift, InspectionTransaction.InspectionEntity data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader rdr = null;
            string success = string.Empty;
            try
            {
                sqlCommand = new SqlCommand(@"[S_GetInspectionDetails_KTA]", conn);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Param", "Save");
                sqlCommand.Parameters.AddWithValue("@MachineID", machine);
                sqlCommand.Parameters.AddWithValue("@Component", Component);
                sqlCommand.Parameters.AddWithValue("@Operation", Operation);
                sqlCommand.Parameters.AddWithValue("@DimensionID", data.InspectionChar);
                sqlCommand.Parameters.AddWithValue("@BatchID", data.HeaderName);
                sqlCommand.Parameters.AddWithValue("@Value", data.JobValue);
                sqlCommand.Parameters.AddWithValue("@Date", Date.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@Shift", Shift);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", Date.ToString("yyyy-MM-dd HH:mm:ss"));
                rdr = sqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    success = rdr["SaveFlag"].ToString();
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
            return success;
        }
        internal static string UpdateApproverDetails(string machine, string Component, string Operation, DateTime Date, string shift, string PlantID, string cellID, string btnName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand sqlCommand = null;
            SqlDataReader rdr = null;
            string success = string.Empty;
            string UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string Username = string.IsNullOrEmpty(HttpContext.Current.Session["UserName"].ToString()) ? "" : HttpContext.Current.Session["UserName"].ToString();
            try
            {
                sqlCommand = new SqlCommand(@"[S_GetInspectionDetails_KTA]", conn);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Param", "Approve");
                sqlCommand.Parameters.AddWithValue("@MachineID", machine);
                sqlCommand.Parameters.AddWithValue("@Component", Component);
                sqlCommand.Parameters.AddWithValue("@Operation", Operation);
                sqlCommand.Parameters.AddWithValue("@PlantID", PlantID);
                sqlCommand.Parameters.AddWithValue("@Cell", cellID);               
                sqlCommand.Parameters.AddWithValue("@Date", Date.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@Shift", shift);
                sqlCommand.Parameters.AddWithValue("@UpdatedTS", Date.ToString("yyyy-MM-dd HH:mm:ss"));
                if(btnName.Equals("Line Inspector Approval", StringComparison.OrdinalIgnoreCase))
                {
                    sqlCommand.Parameters.AddWithValue("@Param1", "LineInspection");
                    sqlCommand.Parameters.AddWithValue("@LineInspectorID", Username);
                    sqlCommand.Parameters.AddWithValue("@LineInspectorTS", UpdatedDate);
                }
                else if (btnName.Equals("Drawing Checked By", StringComparison.OrdinalIgnoreCase))
                {
                    sqlCommand.Parameters.AddWithValue("@Param1", "Drawing");
                    sqlCommand.Parameters.AddWithValue("@DrawingCheckedBy", Username);
                    sqlCommand.Parameters.AddWithValue("@DrawingCheckedTS", UpdatedDate);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@Param1", "Supervisor");
                    sqlCommand.Parameters.AddWithValue("@SupervisorID", Username);
                    sqlCommand.Parameters.AddWithValue("@SupervisorTS", UpdatedDate);
                }
                rdr = sqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    success = rdr["SaveFlag"].ToString();
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
            return success;
        }

        #endregion

        #region Andon setting ---
        internal static SettingsViewEntityStore ViewSettings()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            if (conn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            SettingsViewEntityStore settings = new SettingsViewEntityStore();
            try
            {
                cmd = new SqlCommand(@"SELECT * FROM ShopDefaults WHERE Parameter = 'StoreAndonSetting'", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("ANDonTitle"))
                        {
                            settings.AndonTitle = (sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("PlantToDisplay"))
                        {
                            settings.PlantToDisplay = (sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("DataDisplayInterval"))
                        {
                            settings.DataDisplayInterval = Convert.ToInt32(sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("ScreenFlipInterval"))
                        {
                            settings.ScreenFlipInterval = Convert.ToInt32(sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("FontFamily"))
                        {
                            settings.FontFamily = (sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("FontStyle"))
                        {
                            settings.FontStyle = (sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("DateFormatForHeader"))
                        {
                            settings.DateFormat = (sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].Equals("TimeFormatForHeader"))
                        {
                            settings.TimeFormat = (sdr["ValueInText2"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return settings;
        }

        internal static List<string> GetPlantID()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = @"select distinct plantID from PlantMachineGroups";
            SqlDataReader sdr = null;
            List<string> plantIDList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        plantIDList.Add(sdr["PlantID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPlantID: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return plantIDList;
        }

        internal static void UpdateAndonSettings(string Parameter, string ValueInText, string ValueInText2, out string isSuccessful)
        {
            isSuccessful = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from ShopDefaults where Parameter = @Parameter and ValueInText = @ValueInText)
                                    begin
                                        Insert into ShopDefaults(Parameter, ValueInText, ValueInText2,UpdatedTS)values(@Parameter, @ValueInText, @ValueInText2,@UpdatedTS)
                                    end
                                    else
                                        begin
                                            Update ShopDefaults set ValueInText2 = @ValueInText2,UpdatedTS=@UpdatedTS where Parameter = @Parameter and ValueInText = @ValueInText
                                    end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
                cmd.Parameters.AddWithValue("@ValueInText2", ValueInText2);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int cnt= cmd.ExecuteNonQuery();
                if(cnt>0)
                {
                    isSuccessful = "Successful";
                }
               
            }
            catch (Exception ex)
            {
                isSuccessful = ex.Message;
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static void UpdateGridSettingData(string Parameter, string ValueInText, string ValueInText2, string ValueInInt, string ValueInBool, string TextAlign, string LabelFontSize, string DataFontSize, string CellID, out string isSuccessful)
        {
            isSuccessful = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = string.Empty;
            try
            {
                    query = "if not exists(select * from AndonDefaults where Parameter = @Parameter and ValueInText = @ValueInText and cellId=@cellID) begin Insert into AndonDefaults(Parameter, ValueInText, ValueInText2, ValueInInt, ValueInBool, TextAlign, DataFontSize, LabelFontSize, cellID)values(@Parameter, @ValueInText, @ValueInText2, @ValueInInt, @ValueInBool, @TextAlign, @DataFontSize, @LabelFontSize, @cellID) end else begin Update ANDONDefaults set ValueInText2 = @ValueInText2,ValueInInt = @ValueInInt,ValueInBool = @ValueInBool,TextAlign = @TextAlign, DataFontSize = @DataFontSize , LabelFontSize = @LabelFontSize  where Parameter = @Parameter and ValueInText = @ValueInText and cellId = @cellID end";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Parameter", Parameter);
                    cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
                    cmd.Parameters.AddWithValue("@ValueInText2", ValueInText2);
                    if (!ValueInInt.Equals(""))
                    {
                        cmd.Parameters.AddWithValue("@ValueInInt", Convert.ToInt16(ValueInInt));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ValueInInt", "");
                    }
                    cmd.Parameters.AddWithValue("@TextAlign", TextAlign);
                    cmd.Parameters.AddWithValue("@LabelFontSize", LabelFontSize);
                    cmd.Parameters.AddWithValue("@DataFontSize", DataFontSize);
                    cmd.Parameters.AddWithValue("@cellID", CellID);

                    if (!ValueInBool.Equals(""))
                    {
                        cmd.Parameters.AddWithValue("@ValueInBool", Convert.ToBoolean(ValueInBool));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ValueInBool", "");
                    }
                cmd.ExecuteNonQuery();
                isSuccessful = "Successful";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        #endregion

        #region ------- ComputerMachineAssociationSettings ----------
        public static DataTable ComputerMachineAssociationSettingsData(string Parameter, string plantID, string cellID, string ComputerName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            string query = string.Empty;
            SqlCommand cmd = null;
            try
            {
                if(Parameter.Equals("ComputerMachineAssociation", StringComparison.OrdinalIgnoreCase))
                {
                    query = "select p.PlantID,p.GroupID,p.MachineID,c.* from PlantMachineGroups p left join ( select * from CockpitDefaults where Parameter=@Parameter and  ValueInText=@PCName) c on p.MachineID=c.ValueInText2 where(p.PlantID = @PlantID or ISNULL(@PlantID, '') = '')  and(p.GroupID = @GroupID or ISNULL(@GroupID, '') = '')";
                }

                else if (Parameter.Equals("ComputerCellAssociation", StringComparison.OrdinalIgnoreCase))
                {
                    query = "select distinct p.PlantID,p.GroupID, c.* from PlantMachineGroups p left join ( select * from CockpitDefaults where Parameter=@Parameter and  ValueInText=@PCName) c on p.GroupID=c.ValueInText2 where(p.PlantID = @PlantID or ISNULL(@PlantID, '') = '')";
                }
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                if (Parameter.Equals("ComputerMachineAssociation", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@GroupID", cellID);
                }
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@PCName", ComputerName);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                dt.Columns.Add("Checked", typeof(bool));
                dt.AsEnumerable().ToList<DataRow>().ForEach(k => {
                    k["Checked"] = k["Parameter"].ToString() != "" ? true : false;
                });

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("ComputerMachineAssociationSettingsData" + ex.Message);
            }
            return dt;
        }

        public static bool UpdateInsertMachineComputerData(string Parameter, string ComputerName, string Value)
        {
            bool success = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = @"if exists(select * from CockpitDefaults where ValueInText=@ComputerName and ValueInText2=@Value and Parameter=@Parameter) 
            begin 
                    select 'Exists' as flag 
            end 
            else 
            begin
                    insert into CockpitDefaults(Parameter, ValueInText, ValueInText2, ValueInBool,UpdatedTS) values(@Parameter, @ComputerName, @Value, 0,@UpdatedTS) select 'inserted' as flag 
            end";

            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.Parameters.AddWithValue("@Value", Value);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var result = cmd.ExecuteScalar();

                if(result.ToString() == "inserted" || result.ToString() == "Exists")
                {
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return success;
        }

        public static bool DeleteMachineComputerData(string Parameter, string ComputerName, string Value)
        {
            bool result = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = "if not exists(select * from CockpitDefaults where ValueInText=@ComputerName and ValueInText2=@Value and Parameter=@Parameter) begin select 'notfound' as flag end else begin delete from CockpitDefaults where ValueInText=@ComputerName and ValueInText2=@Value and Parameter=@Parameter select 'deleted' as flag end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.Parameters.AddWithValue("@Value", Value);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                var res = cmd.ExecuteScalar();

                if (res.ToString() == "notfound" || res.ToString() == "deleted")
                {
                    result = true;
                }

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return result;
        }

        public static void SaveRunOptions(string ComputerName, string Value)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = "if not exists(select * from CockpitDefaults where ValueInText=@ComputerName and Parameter='ComputerRunOption') begin insert into CockpitDefaults(Parameter, ValueInText, ValueInText2,UpdatedTS) values('ComputerRunOption', @ComputerName, @Value,@UpdatedTS) end else begin update CockpitDefaults set ValueInText2=@Value,UpdatedTS=@UpdatedTS where ValueInText=@ComputerName and Parameter='ComputerRunOption' end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.Parameters.AddWithValue("@Value", Value);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        public static List<string> GetComputerNames()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            string query = "select distinct ValueInText as ComputerName from CockpitDefaults where Parameter='ComputerMachineAssociation'";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["ComputerName"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return list;
        }
        public static string GetRunOption(string computerName)
        {
            string RunOption = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            string query = "select ValueInText2 as RunOption from CockpitDefaults where Parameter='ComputerRunOption' and ValueInText=@ComputerName";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                RunOption = cmd.ExecuteScalar().ToString();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return RunOption;
        }

        internal static DataTable GetDisplayScreenData(string ComputerName)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string query = string.Empty;
            SqlCommand cmd;
           
            try
            {
                if (ComputerName != "")
                {
                    cmd = new SqlCommand("SP_ShowScreensForComputer_KTA", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Paramter", "KTAAndonDisplayScreens");
                    cmd.Parameters.AddWithValue("@Computer", ComputerName);
                }
                else
                {
                    query = "select ValueInText as DisplayScreen, ValueInBool as Checked from CockpitDefaults where Parameter='KTAAndonDisplayScreens'";
                    cmd = new SqlCommand(query, conn);
                }
                
                sdr = cmd.ExecuteReader();
                if(sdr != null)
                {
                    dt.Load(sdr);
                    dt.AsEnumerable().ToList<DataRow>().ForEach(k => {
                        k["Checked"] = Convert.ToBoolean(k["Checked"]);
                    });
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                //if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static bool InsertDisplayScreenSettings(string ComputerName, string DisplayScreen)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd;
            bool success = false;
            string query = "if not exists (select * from CockpitDefaults where Parameter='ComputerScreenAssociationKTA' and ValueInText=@ComputerName and ValueInText2=@DisplayScreen) begin insert into CockpitDefaults(Parameter, ValueInText, ValueInText2, ValueInBool,UpdatedTS) values('ComputerScreenAssociationKTA', @ComputerName, @DisplayScreen, '1',@UpdatedTS) select 'inserted' as result end else begin select 'exists' as result end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.Parameters.AddWithValue("@DisplayScreen", DisplayScreen);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string result = cmd.ExecuteScalar().ToString();
                if(result.Equals("exists", StringComparison.OrdinalIgnoreCase) || result.Equals("inserted", StringComparison.OrdinalIgnoreCase))
                {
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("InsertUpdateDisplayScreenSettings : " + ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }

        internal static bool DeleteDisplayScreenSettings(string computerName, string DisplayScreen)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            String query = "if exists (select * from CockpitDefaults where Parameter='ComputerScreenAssociationKTA' and ValueInText=@ComputerName and ValueInText2=@DisplayScreen) begin delete from CockpitDefaults where Parameter = 'ComputerScreenAssociationKTA' and ValueInText = @ComputerName and ValueInText2 = @DisplayScreen select 'deleted' as result end else begin select 'not found' as result end";
            bool success = false;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.Parameters.AddWithValue("@DisplayScreen", DisplayScreen);
                string result = cmd.ExecuteScalar().ToString();
                if(result.Equals("deleted", StringComparison.OrdinalIgnoreCase) || result.Equals("not found", StringComparison.OrdinalIgnoreCase))
                {
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("DeleteDisplayScreenSettings : " + ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }
        #endregion

        #region ---------- Production Report ------------
        internal static DataTable GetProductionreportData(string FromDate, string ToDate, string Shift, string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("SP_DaywiseProductionReport_KTA", conn);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@ShiftName", Shift);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 200;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }

            }
            catch(Exception ex)
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
        #endregion

        #region Shift details and hour definition KTA---
        internal static List<ShiftDataModel> GetAllshiftDetailsKTA(string ShiftType)
        {
            List<ShiftDataModel> list = new List<ShiftDataModel>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand(@"Select * from ShiftInformation where running=1 and ShiftType=@ShiftType order by shiftid", sqlConn);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ShiftDataModel shftVals = new ShiftDataModel();
                        if (!Convert.IsDBNull(rdr["ShiftType"]))
                        {
                            shftVals.ShiftType = Convert.ToString(rdr["ShiftType"]);
                        }

                        if (!Convert.IsDBNull(rdr["ShiftId"]))
                        {
                            shftVals.shiftId = Convert.ToString(rdr["ShiftId"]);
                        }

                        if (!Convert.IsDBNull(rdr["ShiftName"]))
                        {
                            shftVals.ShiftName = Convert.ToString(rdr["ShiftName"]);
                        }

                        if (!Convert.IsDBNull(rdr["FromDay"]))
                        {
                            if (rdr["FromDay"].ToString() == "0")
                                shftVals.FromDay = "Today";
                            else if (rdr["FromDay"].ToString() == "1")
                                shftVals.FromDay = "Tomorrow";
                            else
                                shftVals.FromDay = "Yesterday";
                        }
                        if (!Convert.IsDBNull(rdr["FromTime"]))
                        {
                            DateTime dt = Convert.ToDateTime(rdr["FromTime"]);
                            shftVals.FromTime = dt.ToString("hh:mm:ss tt");
                        }

                        if (!Convert.IsDBNull(rdr["ToDay"]))
                        {
                            if (rdr["ToDay"].ToString() == "0")
                                shftVals.ToDay = "Today";
                            else if (rdr["ToDay"].ToString() == "1")
                                shftVals.ToDay = "Tomorrow";
                            else
                                shftVals.ToDay = "Yesterday";
                        }

                        if (!Convert.IsDBNull(rdr["ToTime"]))
                        {
                            DateTime dt = Convert.ToDateTime(rdr["ToTime"]);
                            shftVals.ToTime = dt.ToString("hh:mm:ss tt");
                        }

                        list.Add(shftVals);
                    }

                }
                else
                {
                    list = null;
                }
            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        internal static ShiftDataModel GetShiftDetailsKTA(string shiftId, string ShiftType)
        {
            ShiftDataModel shftVals = new ShiftDataModel();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand(@"select * from ShiftInformation where running=1 and ShiftId = @ShiftId and ShiftType=@ShiftType", sqlConn);
                cmd.Parameters.AddWithValue("@shiftId", shiftId);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);

                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        //
                        if (!Convert.IsDBNull(rdr["ShiftType"]))
                        {
                            shftVals.ShiftType = Convert.ToString(rdr["ShiftType"]);
                        }

                        if (!Convert.IsDBNull(rdr["ShiftId"]))
                        {
                            shftVals.shiftId = Convert.ToString(rdr["ShiftId"]);
                        }

                        if (!Convert.IsDBNull(rdr["ShiftName"]))
                        {
                            shftVals.ShiftName = Convert.ToString(rdr["ShiftName"]);
                        }

                        if (!Convert.IsDBNull(rdr["FromDay"]))
                        {
                            shftVals.FromDay = Convert.ToString(rdr["FromDay"]);
                        }
                        if (!Convert.IsDBNull(rdr["FromTime"]))
                        {
                            shftVals.FromTime = Convert.ToString(rdr["FromTime"]);
                        }

                        if (!Convert.IsDBNull(rdr["ToDay"]))
                        {
                            shftVals.ToDay = Convert.ToString(rdr["ToDay"]);
                        }

                        if (!Convert.IsDBNull(rdr["ToTime"]))
                        {
                            shftVals.ToTime = Convert.ToString(rdr["ToTime"]);
                        }
                    }
                }
                else
                {
                    shftVals = null;
                }
            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return shftVals;
        }

        internal static void RemoveAllShiftdataKTA(string ShiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int rowsAffected = 0;
            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = "Update ShiftInformation SET Running = 0 where Running = 1 and ShiftType=@ShiftType";
            try
            {
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static bool CheckShiftIdKTA(string ShiftID, string ShiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool allreadyPresent = false;
            object obj = null;
            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select shiftId from ShiftInformation where shiftId = @shiftId and ShiftType=@ShiftType and Running = 1";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@shiftId", ShiftID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    allreadyPresent = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return allreadyPresent;
        }

        internal static void UpdateShiftDetailsKTA(string ShiftType, string ShiftID, string ShiftName, string FromDay, string ToDay, DateTime FromTime, DateTime ToTime)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int rowsAffected = 0, fDay = 0, tDay = 0;

            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            sqlQuery = "update ShiftInformation set shiftName = @shiftName ,fromDay =@fromDay, toDay= @toDay, fromTime= @fromTime,Totime= @Totime where shiftId= @shiftId and ShiftType=@ShiftType";

            try
            {
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.Parameters.AddWithValue("@shiftId", ShiftID);
                cmd.Parameters.AddWithValue("@shiftName", ShiftName);

                if (FromDay.Equals("Tomorrow")) fDay = 1;
                else if (FromDay.Equals("Yesterday")) fDay = 2;

                cmd.Parameters.AddWithValue("@fromDay", fDay);

                if (ToDay.Equals("Tomorrow")) tDay = 1;
                else if (ToDay.Equals("Yesterday")) tDay = 2;

                cmd.Parameters.AddWithValue("@toDay", tDay);

                cmd.Parameters.AddWithValue("@fromTime", FromTime);
                cmd.Parameters.AddWithValue("@toTime", ToTime);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static bool CheckForShiftName(string ShiftName, string ShiftID, string Shiftype)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlConnection DBcon = sqlConn;
            SqlCommand cmd = new SqlCommand();
            bool isPresent = false;
            object obj = null;
            try
            {

                cmd = new SqlCommand("Select shiftId from ShiftInformation where  shiftName = @shiftName and shiftId != @shiftId and ShiftType=@ShiftType", DBcon);
                cmd.Parameters.AddWithValue("@shiftId", ShiftID);
                cmd.Parameters.AddWithValue("@shiftName", ShiftName);
                cmd.Parameters.AddWithValue("@ShiftType", Shiftype);

                obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    isPresent = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                DBcon.Close();
            }
            return isPresent;
        }

        internal static void InsertShiftDetailsKTA(string shiftId, string shiftName, String ShiftType,string fromDay, string toDay, DateTime fromTime, DateTime toTime)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int rowsAffected = 0, fDay = 0, tDay = 0;

            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            sqlQuery = "Insert into ShiftInformation ([shiftName],[fromDay] ,[toDay],[fromTime] ,[toTime], [shiftId],Running,ShiftType) values  (@shiftName , @fromDay, @toDay,  @fromTime, @Totime,@shiftId,1,@ShiftType ) ";

            try
            {

                cmd = new SqlCommand(sqlQuery, conn);

                cmd.Parameters.AddWithValue("@shiftId", shiftId);
                cmd.Parameters.AddWithValue("@shiftName", shiftName);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);

                if (fromDay.Equals("Tomorrow")) fDay = 1;
                else if (fromDay.Equals("Yesterday")) fDay = 2;

                cmd.Parameters.AddWithValue("@fromDay", fDay);

                if (toDay.Equals("Tomorrow")) tDay = 1;
                else if (toDay.Equals("Yesterday")) tDay = 2;

                cmd.Parameters.AddWithValue("@toDay", tDay);

                cmd.Parameters.AddWithValue("@fromTime", fromTime);
                cmd.Parameters.AddWithValue("@toTime", toTime);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static bool SaveShiftType(string shiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int rowsAffected = 0;
            bool flag = false;

            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            sqlQuery = @"if not exists(select * from ShiftTypeMasterDetails where ShiftType=@ShiftType)
                        begin
                        insert into ShiftTypeMasterDetails(ShiftType)values(@ShiftType)
                        end";
            try
            {
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@ShiftType", shiftType);
             
                rowsAffected = cmd.ExecuteNonQuery();
                if(rowsAffected>0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return flag;
        }

        internal static List<ShiftDataModel> GetShiftType()
        {
            List<ShiftDataModel> list = new List<ShiftDataModel>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            int slno = 1;
            try
            {
                cmd = new SqlCommand("Select * from ShiftTypeMasterDetails order by IDD", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ShiftDataModel ent = new ShiftDataModel();
                    ent.Slno = slno++;
                    ent.ShiftType = rdr["ShiftType"].ToString();
                    list.Add(ent);
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
            return list;
        }

        internal static int DeleteShiftType(string shiftType)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "";
            int qcount = 0;
            query = @"delete from ShiftTypeMasterDetails where ShiftType=@ShiftType";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ShiftType", shiftType);
                qcount = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return qcount;
        }

        internal static void DeleteShiftDetailsBasedOnShiftType(string shiftType)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "";
            int qcount = 0;
            query = @"delete from ShiftInformation where ShiftType=@ShiftType";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ShiftType", shiftType);
                qcount = cmd.ExecuteNonQuery();
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

        internal static string bindtextShiftFromname(string shiftname,string ShiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                string sqlQuery = "select FromDay from ShiftInformation where ShiftType=@ShiftType and shiftname=@ShiftName and running=1";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ShiftName", shiftname);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    ShiftName = rdr["FromDay"].ToString();
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
            return ShiftName;
        }

        internal static string binddtpfromShiftname(string Shift, string ShiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("[s_GetShiftTime_KTA]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@startdatetime", "01-jan-2000");
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftName = rdr["StartTime"].ToString();
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
            return ShiftName;
        }
        internal static string FindShiftId(string shiftname, string ShiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftId = string.Empty;
            try
            {
                string sqlQuery = "select shiftid from ShiftInformation where ShiftName=@ShiftName and ShiftType=@ShiftType and running=1";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ShiftName", shiftname);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftId = rdr["shiftid"].ToString();
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
            return ShiftId;
        }
        internal static string bindtextShiftToname(string shiftname, string ShiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                string sqlQuery = "select ToDay from ShiftInformation where shiftname=@ShiftName and ShiftType=@ShiftType and running=1";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ShiftName", shiftname);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftName = rdr["ToDay"].ToString();
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
            return ShiftName;
        }
        internal static string bindtextdtptoShiftname(string Shift, string ShiftType)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("[s_GetShiftTime_KTA]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@startdatetime", "01-jan-2000");
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftName = rdr["EndTime"].ToString();
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
            return ShiftName;
        }
        internal static DataTable bindhourlytimeDataKTA(string shiftid, string ShiftType)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("select * from ShiftHourInformation where shiftid=@shiftid and ShiftType=@ShiftType order by HourID asc", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@shiftid", shiftid);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }
        internal static void DeleteDataFromHourlyDefinationKTA(string ShiftType, string shiftId, out string sucessFailure)
        {
            sucessFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("delete from ShiftHourInformation where ShiftID= @shiftname and ShiftType=@ShiftType", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@shiftname", shiftId);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.ExecuteNonQuery();
                sucessFailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving data - \n" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }
        internal static void InsertHourDefination(string ShiftType, string shiftid, string hrname, string HourId, string FromDay, string ToDay, string HourStart, string HourEnd, string min, out string successFailure)
        {
            successFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();

            string query = @"if not  exists(select * from ShiftHourInformation where [ShiftType]=@ShiftType and [ShiftID] = @shiftid and [HourName] =@hrname  and [HourID]=@HourId)
                BEGIN
                 insert into ShiftHourInformation([ShiftType],[ShiftID],[HourName],[HourID],[FromDay],[ToDay],[HourStart],[HourEnd],[Minutes],UpdatedTS) values(@ShiftType,@shiftid,@hrname,@HourId,@FromDay,@ToDay,@HourStart,@HourEnd,@min,@UpdatedTS)
                END
                else
                BEGIN
                update [ShiftHourInformation] set [FromDay]=@FromDay,[ToDay]=@ToDay,[HourStart]=@HourStart,[HourEnd]=@HourEnd,[Minutes]=@min,UpdatedTS=@UpdatedTS 
                where [ShiftID] = @shiftid and [HourName] =@hrname  and [HourID]=@HourId and [ShiftType]=@ShiftType
                END";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.Parameters.AddWithValue("@shiftid", shiftid);
                cmd.Parameters.AddWithValue("@hrname", hrname);
                cmd.Parameters.AddWithValue("@HourId", HourId);
                cmd.Parameters.AddWithValue("@FromDay", FromDay);
                cmd.Parameters.AddWithValue("@ToDay", ToDay);
                cmd.Parameters.AddWithValue("@HourStart", DateTime.Parse(HourStart).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@HourEnd", DateTime.Parse(HourEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@min", min);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                successFailure = "Successfull";
            }
            catch (Exception ex)
            {
                successFailure = ex.Message;
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

        #region --Machine Shift Assoc--

        internal static DataTable GetMachineShiftTypeAssocDetails(string CellId,string Machine,string ShiftType)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_Get_MachineWiseShiftDetails]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType == "All" ? "" : ShiftType);
                cmd.Parameters.AddWithValue("@MachineID", Machine == "All" ? "" : Machine);
                cmd.Parameters.AddWithValue("@CellID", CellId == "All" ? "" : CellId);
                cmd.Parameters.AddWithValue("@Param", "View_MachineShiftTypeDetails");
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getUserRoleRightsDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }
        internal static string CheckShiftTypeForMachine(string machineID, string effectiveDate)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            string ShiftType = string.Empty;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct ShiftType from MachineShiftDetails where MachineID=@MachineID and EffectiveFromDate=@EffDate", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EffDate", effectiveDate);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                sdr = cmd.ExecuteReader();
                while(sdr.Read())
                {
                    ShiftType = sdr["ShiftType"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getUserRoleRightsDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return ShiftType;
        }
        internal static int SaveMachineShiftTypeDetails(string machineID, string effectiveDate,string effToDate, string shiftType)
        {
            int successFailure = 0;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"[S_Get_MachineWiseShiftDetails]";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShiftType", shiftType);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@EffectiveFromDate", effectiveDate);
                cmd.Parameters.AddWithValue("@EffectiveToDate", effToDate);
                cmd.Parameters.AddWithValue("@Param", "Save");
                successFailure = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return successFailure;
        }
        internal static bool CheckShiftTypeForCurrentDate(string machineID, string shiftType)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            bool ExistCurrentDate = false;
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select count(*) as RowExists from MachineShiftDetails where MachineID=@MachineID and ShiftType=@ShiftType and cast(EffectiveToDate as date)=cast(@EffectiveDate as date)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EffectiveDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ShiftType", shiftType);
                sdr = cmd.ExecuteReader();
                while(sdr.Read())
                {
                    int count =Convert.ToInt32(sdr["RowExists"].ToString());
                    if(count>0)
                    {
                        ExistCurrentDate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getUserRoleRightsDetails - " + ex.Message);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return ExistCurrentDate;
        }

        #endregion

    }
}