using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public class DBAccessPradeepMetals
    {
        public static List<PMSceduleScreenEntity> GetScheduleScreenData(string ScheduleStatus, string machineID, string PartNumber, string FromDate, string ToDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<PMSceduleScreenEntity> pmschedulescreen = new List<PMSceduleScreenEntity>();
            string query = "";
            SqlCommand cmd = null;
            try
            {
                if (FromDate == "" || ToDate == "")
                {
                    query = "select * from ScheduleDetails_PradeepMetals where (ScheduleStatus = @ScheduleStatus OR ISNULL(@ScheduleStatus, '') = '') and (MachineID = @MachineID OR ISNULL(@MachineID, '') = '') and (PartNumber = @PartNo OR ISNULL(@PartNo, '') = '')";
                    cmd = new SqlCommand(query, conn);
                }
                else
                {
                    query = "select * from ScheduleDetails_PradeepMetals where (ScheduleStatus = @ScheduleStatus OR ISNULL(@ScheduleStatus, '') = '') and (MachineID = @MachineID OR ISNULL(@MachineID, '') = '') and (PartNumber = @PartNo OR ISNULL(@PartNo, '') = '') and ScheduleDate >= @FromDate and ScheduleDate <= @ToDate ";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(FromDate).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(ToDate).ToString("yyyy-MM-dd"));
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ScheduleStatus", ScheduleStatus);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@PartNo", PartNumber);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        PMSceduleScreenEntity entity = new PMSceduleScreenEntity();

                        entity.MachineID = sdr["MachineID"].ToString();
                        entity.Priority = sdr["Priority"].ToString();
                        entity.PartNumber = sdr["PartNumber"].ToString();
                        entity.JobNumber = sdr["JobTravellerNo"].ToString();
                        entity.PlannedQuantity = sdr["PlannedQty"].ToString();
                        entity.LotCode = sdr["LotNumber"].ToString();
                        entity.OperationNo = sdr["OperationNo"].ToString();
                        entity.ScheduleDate = Convert.ToDateTime(sdr["ScheduleDate"].ToString()).ToString("dd-MM-yyyy");
                        entity.ScheduleStatus = sdr["ScheduleStatus"].ToString();
                        entity.HmiUpdatedTs = sdr["HmiUpdatedTS"].ToString() == "" ? "" : Convert.ToDateTime(sdr["HmiUpdatedTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        entity.ActualQty = sdr["ActualQty"].ToString();
                        entity.SendToHMIStatus = sdr["SendToHmiStatus"].ToString();
                        entity.RevNo = sdr["RevID"].ToString();
                        entity.IsDelete = entity.ScheduleStatus.Trim().Equals("Running", StringComparison.OrdinalIgnoreCase) ? false : true;
                        if(entity.ScheduleStatus.ToString().Trim().Equals("New", StringComparison.OrdinalIgnoreCase))
                            entity.IsReadOnly = false;


                        pmschedulescreen.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetScheduleScreenData: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return pmschedulescreen;

        }

        internal static int InsertScheduleData(DataTable dt)
        {
            string userName = "";
            int count = 0;
            string query = "";
            if (HttpContext.Current.Session["UserName"] != null)
            {
                userName = HttpContext.Current.Session["UserName"].ToString();
            }
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (i == 0)
                        {
                            query = "delete from ScheduleDetailsTemp_PradeepMetals " +
                                "insert into ScheduleDetailsTemp_PradeepMetals(MachineID, PartNumber, JobTravellerNo, LotNumber, OperationNo, ScheduleDate, Priority, PlannedQty, updatedBy, UpdatedTS,RevID) values(@MachineID, @PartNumber, @JobNo, @LotNo, @OperationNo, @ScheduleDate, @Priority, @PlannedQty, @UpdatedBy, @UpdatedTS,@RevID)";
                        }
                        else
                        {
                            query = "insert into ScheduleDetailsTemp_PradeepMetals(MachineID, PartNumber, JobTravellerNo, LotNumber, OperationNo, ScheduleDate, Priority, PlannedQty, updatedBy, UpdatedTS,RevID) values(@MachineID, @PartNumber, @JobNo, @LotNo, @OperationNo, @ScheduleDate, @Priority, @PlannedQty, @UpdatedBy, @UpdatedTS,@RevID)";
                        }
                        if (row["MachineID"].ToString() != "" && row["PMLPartNo"].ToString() != "" && row["JobTravellerNo"].ToString() != "" && row["LotCode"].ToString() != "" && row["OperationNo"].ToString() != "")
                        {
                            cmd = new SqlCommand(query, conn);
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 120;
                            cmd.Parameters.AddWithValue("@MachineID", row["MachineID"].ToString());
                            cmd.Parameters.AddWithValue("@PartNumber", row["PMLPartNo"].ToString());
                            cmd.Parameters.AddWithValue("@JobNo", row["JobTravellerNo"].ToString());
                            cmd.Parameters.AddWithValue("@LotNo", row["LotCode"].ToString());
                            cmd.Parameters.AddWithValue("@OperationNo", row["OperationNo"].ToString());
                            cmd.Parameters.AddWithValue("@ScheduleDate", Convert.ToDateTime(row["ScheduleDate"].ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
                            cmd.Parameters.AddWithValue("@Priority", row["Priority"].ToString());
                            cmd.Parameters.AddWithValue("@PlannedQty", row["PlannedQty"].ToString());
                            cmd.Parameters.AddWithValue("@UpdatedBy", userName);
                            cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@RevID", row["RevID"].ToString());
                            int x = cmd.ExecuteNonQuery();
                            if (x > 0)
                            {
                                count++;
                            }
                        }
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("InsertScheduleData:" + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return count;
        }

        internal static string InsertNewSchdule(PMSceduleScreenEntity entity)
        {
            string userName = "";
            string count = "";
            if (HttpContext.Current.Session["UserName"] != null)
            {
                userName = HttpContext.Current.Session["UserName"].ToString();
            }
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = @"IF NOT EXISTS(SELECT * FROM componentoperationpricing WHERE machineid=@MachineID AND componentid=@PartNumber AND operationno=@OperationNo)
BEGIN
	SELECT 'COP Not Available.' AS SaveFlag
END
ELSE
BEGIN
	IF NOT EXISTS(SELECT * FROM ScheduleDetails_PradeepMetals WHERE MachineID=@MachineID and PartNumber=@PartNumber and JobTravellerNo=@JobNo and 
	LotNumber=@LotNo and OperationNo=@OperationNo and ScheduleDate=@ScheduleDate)
	BEGIN
		insert into ScheduleDetails_PradeepMetals(MachineID, PartNumber, JobTravellerNo, LotNumber, OperationNo, ScheduleDate, Priority, PlannedQty, 
		updatedBy, UpdatedTS,RevID) values(@MachineID, @PartNumber, @JobNo, @LotNo, @OperationNo, @ScheduleDate, @Priority, @PlannedQty, @UpdatedBy, @UpdatedTS,@RevID) 
select 'Inserted' as SaveFlag
	END
	ELSE
	BEGIN
		select 'Schedule Already Exists.' as SaveFlag
	END
END";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", entity.MachineID);
                cmd.Parameters.AddWithValue("@PartNumber", entity.PartNumber);
                cmd.Parameters.AddWithValue("@JobNo", entity.JobNumber);
                cmd.Parameters.AddWithValue("@LotNo", entity.LotCode);
                cmd.Parameters.AddWithValue("@OperationNo", entity.OperationNo);
                cmd.Parameters.AddWithValue("@ScheduleDate", Util.GetDateTime(entity.ScheduleDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Priority", entity.Priority);
                cmd.Parameters.AddWithValue("@PlannedQty", entity.PlannedQuantity);
                cmd.Parameters.AddWithValue("@UpdatedBy", userName);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@RevID", entity.RevNo);

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    count = sdr["SaveFlag"].ToString();
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
            return count;
        }

        internal static void BulkInsertScheduleData()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("SP_MachineLoadingPlanImport_PradeepMetals", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
                //cmd.Parameters.AddWithValue("",);
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

        internal static int UpdateSchdules(string machineID, string Priority, string PartNumber, string JobNO, string PlannedQty, string LotCode, string OperationNo, string ScheduleDate, string ScheduleStatus, string RevID)
        {
            int x = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"UPDATE ScheduleDetails_PradeepMetals SET Priority=@Priority 
WHERE MachineID = @MachineID and PartNumber = @PartNumber and JobTravellerNo = @JobTravellerNo and LotNumber = @LotNumber and ScheduleDate = @ScheduleDate and 
PlannedQty = @PlannedQty and OperationNo = @OperationNo and ScheduleStatus = @ScheduleStatus";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Priority", Priority);
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber);
                cmd.Parameters.AddWithValue("@JobTravellerNo", JobNO);
                cmd.Parameters.AddWithValue("@LotNumber", LotCode);
                cmd.Parameters.AddWithValue("@ScheduleDate", Util.GetDateTime(ScheduleDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlannedQty", PlannedQty);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.Parameters.AddWithValue("@ScheduleStatus", ScheduleStatus);
                cmd.Parameters.AddWithValue("@RevID", RevID);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                x = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateSchdules: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return x;
        }

        internal static int DeleteSelectedSchedule(string MachineID, string PartNumber, string JobNo, string LotCode, string OperationNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int x = 0;
            try
            {
                cmd = new SqlCommand("delete from ScheduleDetails_PradeepMetals where MachineID=@MachineID and PartNumber=@PartNumber and JobTravellerNo=@JobTravellerNo and LotNumber=@LotNumber and OperationNo = @OperationNo", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber);
                cmd.Parameters.AddWithValue("@JobTravellerNo", JobNo);
                cmd.Parameters.AddWithValue("@LotNumber", LotCode);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                x = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("DeleteSelectedSchedule: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return x;
        }

        internal static int SoftDeleteSelectedSchedule(string MachineID, string PartNumber, string JobNo, string LotCode, string OperationNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "update ScheduleDetails_PradeepMetals set SendToHmiStatus='2' where MachineID=@MachineID and PartNumber=@PartNumber and JobTravellerNo=@JobTravellerNo and LotNumber=@LotNumber and OperationNo = @OperationNo";
            int res = 0;
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber);
                cmd.Parameters.AddWithValue("@JobTravellerNo", JobNo);
                cmd.Parameters.AddWithValue("@LotNumber", LotCode);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                res = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SoftDeleteSelectedSchedule: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static List<string> GetMachineIDForPMScheduleScreen()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = "select distinct MachineID from machineinformation where TPMTrakEnabled='1'";
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        string machine = sdr["MachineID"].ToString();
                        list.Add(machine);
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

        internal static List<string> GetPartsForPMScheduleScreen(string machineid)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = "select distinct PartNumber from ScheduleDetails_PradeepMetals where (MachineID=@MachineID OR ISNULL(@MachineID,'')='')";
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        string machine = sdr["PartNumber"].ToString();
                        list.Add(machine);
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

        internal static bool ValidatePartNumberOperationNo(string PartNumber, string OperationNumber, string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            bool isAvailable = false;
            try
            {
                cmd = new SqlCommand("select * from componentoperationpricing where componentid=@ComponentID and operationno=@OperationNo and machineid=@MachineID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComponentID", PartNumber);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNumber);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    isAvailable = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ValidatePartNumberOperationNo : " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return isAvailable;
        }

        public static List<PMSceduleScreenEntity> GetScheduleReportData(string ScheduleStatus, string Year, string Month)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<PMSceduleScreenEntity> pmschedulescreen = new List<PMSceduleScreenEntity>();
            string query = "";
            SqlCommand cmd = null;
            try
            {

                query = @"select * from ScheduleDetails_PradeepMetals where (ScheduleStatus IN(SELECT Item FROM SplitStrings(@ScheduleStatus, ','))) AND DATEPART(YEAR, ScheduleDate) = @Year AND DATEPART(MONTH, ScheduleDate)= @Month"; // and ScheduleDate >= @FromDate and ScheduleDate <= @ToDate  
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@ScheduleStatus", ScheduleStatus);

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        PMSceduleScreenEntity entity = new PMSceduleScreenEntity();

                        entity.MachineID = sdr["MachineID"].ToString();
                        entity.Priority = sdr["Priority"].ToString();
                        entity.PartNumber = sdr["PartNumber"].ToString();
                        entity.JobNumber = sdr["JobTravellerNo"].ToString();
                        entity.PlannedQuantity = sdr["PlannedQty"].ToString();
                        entity.LotCode = sdr["LotNumber"].ToString();
                        entity.OperationNo = sdr["OperationNo"].ToString();
                        entity.ScheduleDate = Convert.ToDateTime(sdr["ScheduleDate"].ToString()).ToString("dd-MM-yyyy");
                        entity.ScheduleStatus = sdr["ScheduleStatus"].ToString();
                        entity.HmiUpdatedTs = sdr["HmiUpdatedTS"].ToString() == "" ? "" : Convert.ToDateTime(sdr["HmiUpdatedTS"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        entity.ActualQty = sdr["ActualQty"].ToString();
                        entity.RevNo = sdr["RevID"].ToString();
                        pmschedulescreen.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetScheduleScreenData: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return pmschedulescreen;

        }

        #region ------------Andon Settings------------
        internal static List<AndonSettingsEntity> GetAndonSettings()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<AndonSettingsEntity> list = new List<AndonSettingsEntity>();
            try
            {
                cmd = new SqlCommand("select * from AndonDefaults where Parameter='AndonViewColumn' order by ValueInInt", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        AndonSettingsEntity entity = new AndonSettingsEntity();
                        entity.ColumnName = sdr["ValueInText"].ToString();
                        entity.DisplayText = sdr["ValueInText2"].ToString();
                        entity.SortOrder = sdr["ValueInInt"].ToString();
                        entity.Visibility = Convert.ToInt32(sdr["ValueInBool"].ToString());
                        entity.TextAlign = sdr["TextAlign"].ToString() == "" ? "Left" : sdr["TextAlign"].ToString();
                        entity.DataFontSize = sdr["DataFontSize"].ToString() == "" ? "14" : sdr["DataFontSize"].ToString();
                        entity.LabelFontSize = sdr["LabelFontSize"].ToString() == "" ? "14" : sdr["LabelFontSize"].ToString();

                        list.Add(entity);
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

        internal static int UpdateAndonViewSettings(string Parameter, string ValueInText, string ValueInText2, int ValueInInt, int ValueInBool, string TextAlign, string LabelFontSize, string DataFontSize)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int res = 0;
            string query = "if exists(select * from AndonDefaults where Parameter=@Parameter and ValueInText=@ValueInText) begin update AndonDefaults set ValueInText2 = @ValueInText2, ValueInInt = @ValueInInt, ValueInBool = @ValueInBool, TextAlign = @TextAlign, DataFontSize = @DataFontSize, LabelFontSize = @LabelFontSize where Parameter = @Parameter and ValueInText = @ValueInText end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
                cmd.Parameters.AddWithValue("@ValueInText2", ValueInText2);
                cmd.Parameters.AddWithValue("@ValueInInt", ValueInInt);
                cmd.Parameters.AddWithValue("@ValueInBool", ValueInBool);
                cmd.Parameters.AddWithValue("@TextAlign", TextAlign);
                cmd.Parameters.AddWithValue("@DataFontSize", DataFontSize);
                cmd.Parameters.AddWithValue("@LabelFontSize", LabelFontSize);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateAndonViewSettings: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static bool UpdateAndonGeneralSettings(string Parameter, string AndonTitle, string FontFamily, string FontStyle, string DataRefreshInterval, string ScreenFlipInterval, int FooterEnabled, int MsgEnabled, string ScrollingText, string DateFormat, string TimeFormat/*, int CurvedBoxes*/)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool success = false;
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetAndonUISettings]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@param", Parameter);
                cmd.Parameters.AddWithValue("@AndonTitle", AndonTitle);
                cmd.Parameters.AddWithValue("@FontFamily", FontFamily);
                cmd.Parameters.AddWithValue("@FontStyle", FontStyle);
                cmd.Parameters.AddWithValue("@DataDisplayInterval", DataRefreshInterval);
                cmd.Parameters.AddWithValue("@ScreenFlipInterval", ScreenFlipInterval);
                cmd.Parameters.AddWithValue("@ShowFooterBlock", FooterEnabled);
                cmd.Parameters.AddWithValue("@MsgBlockEnabled", MsgEnabled);
                cmd.Parameters.AddWithValue("@ScrollingText", ScrollingText);
                cmd.Parameters.AddWithValue("@DateFormatForHeader", DateFormat);
                cmd.Parameters.AddWithValue("@TimeFormatForHeader", TimeFormat);
                //cmd.Parameters.AddWithValue("@ShowCurvedBox", CurvedBoxes);
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateAndonGeneralSettings:" + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }

        // internal static bool UpdateColorCodeSettings(string Param, string ValueInText, string ValueInText2)
        // {
        //     SqlConnection conn = ConnectionManager.GetConnection();
        //     SqlCommand cmd = null;
        //     bool success = false;
        //     string query = "if not exists(select * from CockpitDefaults where Parameter='MachineEfficiencyColorAndon' and ValueInText=@ValueInText) begin insert into CockpitDefaults(Parameter, ValueInText, ValueInText2) values('MachineEfficiencyColorAndon', @ValueInText, @ValueInText2) end else begin update CockpitDefaults set ValueInText2 = @ValueInText2 where Parameter = 'MachineEfficiencyColorAndon' and ValueInText = @ValueInText end";
        //     try
        //     {
        //         cmd = new SqlCommand(query, conn);
        //         cmd.CommandType = CommandType.Text;
        //         cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
        //         cmd.Parameters.AddWithValue("@ValueInText2", ValueInText2);
        //         cmd.CommandTimeout = 120;
        //         int res = cmd.ExecuteNonQuery();
        //         if(res >= 0)
        //         {
        //             success = true;
        //         }
        //     }
        //     catch(Exception ex)
        //     {
        //         Logger.WriteErrorLog("UpdateColorCodeSettings:" + ex.Message);
        //     }
        //     finally
        //     {
        //         if (conn != null) conn.Close();
        //     }
        //     return success;
        // }

        internal static AndonGeneralSettingsEntity ViewAndonGeneralSettings(string Param)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            AndonGeneralSettingsEntity entity = new AndonGeneralSettingsEntity();
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetAndonUISettings]", conn);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("AndonTitle", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.AndonTitle = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("FontFamily", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.FontFamily = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("FontStyle", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.FontStyle = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("DataDisplayInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.DataRefreshInterval = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("ScreenFlipInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.ScreenFlipInterval = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("ShowFooterBlock", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.FooterEnabled = Convert.ToInt32(sdr["ValueInInt"].ToString()) == 1 ? "flex" : "none";
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("MsgBlockEnabled", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.MsgEnabled = Convert.ToInt32(sdr["ValueInInt"].ToString()) == 1 ? "inline-block" : "none";
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("DateFormatForHeader", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.DateFormat = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("TimeFormatForHeader", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.TimeFormat = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        if (sdr["ValueInText"].ToString().Equals("ScrollingText", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.ScrollingText = sdr["ValueInText2"].ToString();
                            continue;
                        }
                        //if (sdr["ValueInText"].ToString().Equals("ShowCurvedBox", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    entity.CurvedBoxes = Convert.ToInt32(sdr["ValueInInt"].ToString());
                        //    continue;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ViewAndonGeneralSettings: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return entity;
        }

        // internal static AndonEfficiencyColorEntity GetEfficiencyColorValues()
        // {
        //     AndonEfficiencyColorEntity colorEntity = new AndonEfficiencyColorEntity();
        //     SqlConnection conn = ConnectionManager.GetConnection();
        //     SqlCommand cmd = null;
        //     SqlDataReader sdr = null;
        //     string query = "select * from cockpitDefaults where Parameter='MachineEfficiencyColorAndon'";
        //     try
        //     {
        //         cmd = new SqlCommand(query, conn);
        //         cmd.CommandType = CommandType.Text;
        //         cmd.CommandTimeout = 120;
        //         sdr = cmd.ExecuteReader();
        //         if (sdr.HasRows)
        //         {
        //             while (sdr.Read())
        //             {
        //                 if (sdr["ValueInText"].ToString().Equals("GoodCOlor", StringComparison.OrdinalIgnoreCase))
        //                     colorEntity.GoodColor = sdr["ValueInText2"].ToString();
        //                 if (sdr["ValueInText"].ToString().Equals("ModerateColor", StringComparison.OrdinalIgnoreCase))
        //                     colorEntity.ModerateColor = sdr["ValueInText2"].ToString();
        //                 if (sdr["ValueInText"].ToString().Equals("BadColor", StringComparison.OrdinalIgnoreCase))
        //                     colorEntity.BadColor = sdr["ValueInText2"].ToString();
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Logger.WriteErrorLog("GetEfficiencyColorValues" + ex.Message);
        //     }
        //     finally
        //     {
        //         if (sdr != null) sdr.Close();
        //         if (conn != null) conn.Close();
        //     }
        //     return colorEntity;
        // }

        #endregion

        #region Andon Screen 
        internal static DataTable GetProductionAndonData(string ComputerName, string Shift, string Date, string MachineID, string CellID, string PlantID)
        {
            List<AndonEntity> list = new List<AndonEntity>();
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_GetProductionANDOn_PradeepMetals]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@PcName", "'" + ComputerName + "'");
                cmd.Parameters.AddWithValue("@CellID", CellID);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    //while (sdr.Read())
                    //{
                    //    AndonEntity entity = new AndonEntity();
                    //    entity.MachineID = sdr["MachineID"].ToString();
                    //    entity.GroupID = sdr["GroupID"].ToString();
                    //    entity.PartNo = sdr["PartNo"].ToString();

                    //    entity.AE = sdr["AE"].ToString();
                    //    entity.PE = sdr["PE"].ToString();
                    //    entity.QE = sdr["QE"].ToString();
                    //    entity.OEE = sdr["OEE"].ToString();

                    //    entity.DownTimeInMin = sdr["DownTimeInMin"].ToString();
                    //    entity.Operator = sdr["Operator"].ToString();
                    //    entity.ProductionTarget = sdr["ProductionTarget"].ToString();
                    //    entity.ActualProductionCount = sdr["ActualProductionCount"].ToString();
                    //    entity.CumulativeProductionTarget = sdr["CumulativeProductionTarget"].ToString();
                    //    entity.CumulativeActualProdCount = sdr["CumulativeActualProdCount"].ToString();
                    //    entity.Status = sdr["Status"].ToString();

                    //    list.Add(entity);
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionAndonData" + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
            //return list;
        }
        internal static string GetSuperVisorName(string currentShift, string cellID)
        {
            string SuperVisorName = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = "select cs.* from CellSupervisor_PradeepMetals cs left join shiftdetails sd on cs.shiftid=sd.shiftid where sd.ShiftName=@ShiftID and cs.cellid=@CellID";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CellID", cellID);
                cmd.Parameters.AddWithValue("@ShiftID", currentShift);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        SuperVisorName = sdr["Supervisor"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetSuperVisorName:" + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return SuperVisorName;
        }

        internal static List<string> GetCellIDComputerWise(string ComputerName, string PlantID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            string query = "IF EXISTS(SELECT DISTINCT ValueInText2 FROM CockpitDefaults WHERE ValueInText=@ComputerName AND Parameter='computercellassociation') BEGIN SELECT DISTINCT ValueInText2 as GROUPID FROM CockpitDefaults WHERE ValueInText = @ComputerName AND Parameter = 'computercellassociation' END ELSE BEGIN SELECT DISTINCT GroupID as GROUPID FROM PlantMachineGroups WHERE PlantID = @PlantID or ISNULL(@PlantID, '')= '' END";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
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
            return list;
        }

        #endregion

        #region ------------General-----------
        internal static List<string> GetPlantID()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct PlantID from plantinformation", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
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
                Logger.WriteErrorLog("GetPlantID: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<string> GetCellID(string plantID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = string.Empty;
            List<string> list = new List<string>();
            if (plantID == "")
            {
                query = "select distinct GroupID from PlantMachineGroups ";
            }
            else
            {
                query = "select distinct GroupID from PlantMachineGroups where PlantID=@PlantID";
            }
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["GroupID"].ToString());
                    }
                }
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCellID: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<string> GetMachineID(string PlantID, string CellID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            string query = "select distinct MachineID from PlantMachineGroups where (PlantID = @PlantID OR ISNULL(@PlantID, '') = '') and (GroupID = @GroupID OR ISNULL(@GroupID, '') = '')";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@GroupID", CellID);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetMachineID: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        #endregion

        #region--------------------PradeepMetals Reports----------------------------------
        internal static List<string> GetMachineIDs(string CellID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                //(((s.GroupID IN(SELECT Item FROM SplitStrings(@MachineID, ',')) or ISNULL(@machineid,'')= '')))
                cmd = new SqlCommand(@"select distinct machineid from plantmachinegroups where (((GroupID IN (SELECT Item FROM SplitStrings(@GroupID,',')) or ISNULL(@GroupID,'')='')))", con);
                //@"select distinct MachineID from PlantMachineGroups where GroupID=@GroupID or ISNULL(@GroupID,'')=''",
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@GroupID", CellID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        internal static string GetShiftStart()//out string ShiftStart,out string ShiftEnd
        {
            string ShiftName = "", ShiftStart = "", ShiftEnd = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("s_GetCurrentShift", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ShiftName = sdr["ShiftName"].ToString();
                        ShiftStart = sdr["StartTime"].ToString();
                        ShiftEnd = sdr["EndTime"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetShiftStart: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return ShiftName;
        }

        internal static DataTable MachineLevelDownTimePareto(string machineID, DateTime fromDate, DateTime toDate, string DownCategory, out DataTable dt2, out DataTable dt3, out DataTable dt4)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt1 = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            dt4 = new DataTable();
            try
            {
                cmd = new SqlCommand(@"SP_MaintenanceDownTimeDetails_PradeepMetals", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromDate.ToString()).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate.ToString()).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DownCategory", DownCategory);
                sdr = cmd.ExecuteReader();
                dt1.Load(sdr);
                dt2.Load(sdr);
                dt3.Load(sdr);
                dt4.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt1;
        }
        internal static DataTable MaintenanceSubsystemLevelDownTime(string machine, DateTime FromDate, DateTime ToDate, string DownCategory, out DataTable dt2, out DataTable dt3, out DataTable dt4)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt1 = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            dt4 = new DataTable();
            string Year = FromDate.Year.ToString();
            string Month = FromDate.Month.ToString().Length == 1 ? "0" + FromDate.Month.ToString() : FromDate.Month.ToString();
            string Year2 = ToDate.Year.ToString();
            string Month2 = ToDate.Month.ToString().Length == 1 ? "0" + ToDate.Month.ToString() : ToDate.Month.ToString();
            try
            {
                cmd = new SqlCommand(@"SP_SubSystemPareto_PradeepMetals", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Startyear", Year);
                cmd.Parameters.AddWithValue("@StartMonth", Month);
                cmd.Parameters.AddWithValue("@endyear", Year2);
                cmd.Parameters.AddWithValue("@endMonth", Month2);
                cmd.Parameters.AddWithValue("@DownCategory", DownCategory);
                sdr = cmd.ExecuteReader();
                dt1.Load(sdr);
                dt2.Load(sdr);
                dt3.Load(sdr);
                dt4.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt1;
        }
        internal static DataTable ParetoDownTimeReason(string machine, DateTime FromDate, DateTime ToDate, out DataTable dt2, out DataTable dt3, out DataTable dt4)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            dt4 = new DataTable();
            string Year = FromDate.Year.ToString();
            string Month = FromDate.Month.ToString().Length == 1 ? "0" + FromDate.Month.ToString() : FromDate.Month.ToString();
            string Year2 = ToDate.Year.ToString();
            string Month2 = ToDate.Month.ToString().Length == 1 ? "0" + ToDate.Month.ToString() : ToDate.Month.ToString();
            try
            {
                cmd = new SqlCommand(@"SP_ParetoDownTimeReason_PradeepMetals", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Startyear", Year);
                cmd.Parameters.AddWithValue("@endyear", Year2);
                cmd.Parameters.AddWithValue("@StartMonth", Month);
                cmd.Parameters.AddWithValue("@endMonth", Month2);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt2.Load(sdr);
                dt3.Load(sdr);
                dt4.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt;
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
            return machine;
        }
        internal static string getDropdownValueWithSingleQuote(DropDownList dropDownList)
        {
            string cellID = "";
            try
            {
                if (dropDownList.SelectedValue == "All")
                {
                    foreach (ListItem item in dropDownList.Items)
                    {
                        if (cellID == "")
                            cellID += "'" + item.Value + "'";
                        else
                            cellID += ",'" + item.Value + "'";
                    }
                }
                else
                {
                    cellID += "'" + dropDownList.SelectedValue + "'";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getDropdownValueWithSingleQuote = " + ex.ToString());
            }
            return cellID;
        }
        internal static DataTable OEEReportData(string machineID, DateTime FromDate, DateTime ToDate, string CellID, string Param)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            int Year = FromDate.Year;
            int Month = FromDate.Month;
            int Year2 = ToDate.Year;
            int Month2 = ToDate.Month;
            try
            {
                cmd = new SqlCommand(@"S_GetOEEReport_PradeepMetals", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Year2", Year2);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Month2", Month2);
                cmd.Parameters.AddWithValue("@CellID", CellID);
                cmd.Parameters.AddWithValue("@Param", Param);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt;
        }

        internal static DataTable ProductionReportCNCData(DateTime startDate, DateTime EndDate, string Machineid, string CellID, string Shift, out DataTable dt2, out DataTable dt3)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            try
            {
                cmd = new SqlCommand(@"S_GetProductionReport_PradeepMetals", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                cmd.Parameters.AddWithValue("@MachineID", Machineid);
                cmd.Parameters.AddWithValue("@CellID", CellID);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt2.Load(sdr);
                dt3.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return dt;
        }

        #endregion
        #region----------Cell Supervisor Screen PradeepMetals------------------
        internal static List<string> GetSupervisors()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select Employeeid from employeeinformation where EmployeeRole='Supervisor'", con);
                cmd.CommandType = CommandType.Text;
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
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        internal static List<CellSupervisorScreen> GetCellSupervisorData(string PlantID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<CellSupervisorScreen> list = new List<CellSupervisorScreen>();
            CellSupervisorScreen data = null;
            try
            {
                cmd = new SqlCommand(@"SELECT DISTINCT A1.GroupID,A2.ShiftID,A3.Supervisor FROM PlantMachineGroups A1
CROSS JOIN (SELECT DISTINCT shiftid FROM shiftdetails) A2
LEFT JOIN CellSupervisor_PradeepMetals A3 ON A1.GroupID=A3.CellID AND A2.shiftid=A3.ShiftID
WHERE (A1.PlantID=@PlantID OR ISNULL(@PlantID,'')='')", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new CellSupervisorScreen();
                        data.CellID = sdr["GroupID"].ToString();
                        data.Shift = sdr["shiftid"].ToString();
                        data.Supervisor = sdr["Supervisor"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        internal static string SaveCellSupervisors(CellSupervisorScreen data)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"if Exists(select * from dbo.CellSupervisor_PradeepMetals where CellID=@CellID and ShiftID=@ShiftID)
Begin
		Update dbo.CellSupervisor_PradeepMetals set Supervisor=@Supervisor
		where CellID=@CellID and ShiftID=@ShiftID
		Select 'Updated' as SaveFlag
End
Else 
Begin
	Insert into dbo.CellSupervisor_PradeepMetals(CellID,ShiftID,Supervisor)
	values(@CellID,@ShiftID,@Supervisor)

	Select 'Inserted' as SaveFlag
End", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CellID", data.CellID);
                cmd.Parameters.AddWithValue("@ShiftID", data.Shift);
                cmd.Parameters.AddWithValue("@Supervisor", data.Supervisor);
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
            return success;
        }
        internal static string DeleteCellSupervisor(CellSupervisorScreen data)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"delete from CellSupervisor_PradeepMetals where CellID=@CellID and ShiftID=@ShiftID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CellID", data.CellID);
                cmd.Parameters.AddWithValue("@ShiftID", data.Shift);
                int count = cmd.ExecuteNonQuery();
                if (count > 1)
                {
                    success = "Deleted";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return success;
        }
        #endregion

        #region------Computer Cell Wise Settings-------
        internal static DataTable GetComputerCellAssociationData(string computerName, string PlantID, string CellID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string query = "select distinct p.PlantID, p.GroupID,c.* from PlantMachineGroups p left join (select * from CockpitDefaults where Parameter='ComputerCellAssociation' and ValueInText=@ComputerName) c on p.GroupID = c.ValueInText2 where(p.GroupID = @GroupID or ISNULL(@GroupID, '') = '') and (p.PlantID = @PlantID or ISNULL(@PlantID, '') = '')";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@GroupID", CellID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);

                    dt.Columns.Add("Checked", typeof(bool));

                    foreach (DataRow row in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["Parameter"].ToString()))
                        {
                            row["Checked"] = true;
                        }
                        else
                        {
                            row["Checked"] = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetComputerCellAssociationData:" + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static bool UpdateComputerCellAssociationSettings(string ComputerName, string PlantID, string CellID, string Param)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "";
            bool success = false;
            try
            {
                if (Param.Equals("Insert", StringComparison.OrdinalIgnoreCase))
                {
                    query = "if not exists(select * from CockpitDefaults where Parameter='ComputerCellAssociation' and ValueInText=@ComputerName and ValueInText2=@GroupID) begin insert into CockpitDefaults(Parameter, ValueInText, ValueInText2, ValueInBool) values('ComputerCellAssociation', @ComputerName, @GroupID, 1) end";
                }
                else if (Param.Equals("Delete", StringComparison.OrdinalIgnoreCase))
                {
                    query = "delete from CockpitDefaults where Parameter='ComputerCellAssociation' and ValueInText=@ComputerName and ValueInText2=@GroupID ";
                }
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@GroupID", CellID);
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                int res = cmd.ExecuteNonQuery();
                if (res >= 0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateComputerCellAssociationSettings: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }

        internal static List<string> GetComputerNames()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct ValueInText as ComputerName from CockpitDefaults where Parameter='ComputerCellAssociation'", conn);
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
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetComputerNames:" + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return list;
        }
        #endregion

        internal static List<string> GetDownCategoryList()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            string query = "select distinct DownCategory from DownCategoryInformation";
            try
            {
                cmd = new SqlCommand(query, conn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["DownCategory"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDownCategoryList: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
    }
}