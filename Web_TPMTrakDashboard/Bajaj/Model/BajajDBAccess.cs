using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Bajaj.Model
{
    public class BajajDBAccess
    {
        #region ---- JH Master Details ----
        internal static List<ListItem> GetRevisionDetailsForMachine(string machineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                cmd = new SqlCommand(@"select distinct RevId, RevNo  from JH_CheckList_Master_Bajaj where machineid='" + machineId + "' order by RevId desc", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Text = rdr["RevNo"].ToString(), Value = rdr["RevID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetRevisionDetailsForMachine: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<ListItem> GetParametersForMachine(string machineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<ListItem> list = new List<ListItem>();
            try
            {
                cmd = new SqlCommand(@"select * from [ProcessParameterMaster_BajajIoT] where MachineID='" + machineId + "'", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Text = rdr["ParameterName"].ToString(), Value = rdr["ParameterID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetParametersForMachine: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<string> GetRoleDetails(string role)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select * from employeeinformation where EmployeeRole='" + role + "'", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["Employeeid"].ToString());
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetRoleDetails: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<JHMasterDetails> GetJHMasterDetails(string machine, string revid, bool isModifiable)
        {
            List<JHMasterDetails> list = new List<JHMasterDetails>();
            JHMasterDetails data = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_Get_JH_CheckList_MasterDetails_Bajaj]", con);
                cmd.Parameters.Add(new SqlParameter("@MachineID", machine));
                cmd.Parameters.Add(new SqlParameter("@RevNo", revid));
                cmd.Parameters.Add(new SqlParameter("@Param", "View"));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new JHMasterDetails();
                        if (string.IsNullOrEmpty(sdr["RevDate"].ToString()))
                        {
                            data.RevDate = sdr["RevDate"].ToString();
                        }
                        else
                        {
                            data.RevDate = Util.GetDateTime(sdr["RevDate"].ToString()).ToString("dd-MM-yyyy");
                        }

                        data.RevID = sdr["RevId"].ToString();
                        data.RevNo = sdr["RevNo"].ToString();
                        data.CheckPointNo = sdr["CheckpointNo"].ToString();
                        data.RouteNo = sdr["RouteNo"].ToString();
                        data.RelatedTo = sdr["RelatedTo"].ToString();
                        if (data.RelatedTo.Equals("Leads to Defect", StringComparison.OrdinalIgnoreCase))
                        {
                            data.RelatedToImagePath = "Images/LeadsToDefect.png";

                        }
                        else if (data.RelatedTo.Equals("Leads to Break down", StringComparison.OrdinalIgnoreCase))
                        {
                            data.RelatedToImagePath = "Images/LeadsToBreakDown.png";

                        }
                        else if (data.RelatedTo.Equals("Leads to Accident", StringComparison.OrdinalIgnoreCase))
                        {
                            data.RelatedToImagePath = "Images/LeadsToAccident.png";

                        }
                        data.Frequency = sdr["Frequency"].ToString();
                        data.C_L_I_RT_Value = sdr["C_L_I_R_T"].ToString();
                        data.Item = sdr["Item"].ToString();
                        data.CheckPoint = sdr["Checkpoint"].ToString();
                        data.Standard = sdr["Standard"].ToString();
                        data.IfNotOk = sdr["If_NotOK"].ToString();
                        data.Method = sdr["Method"].ToString();
                        data.DayNo = sdr["DayNo"].ToString();
                        data.Month = sdr["MonthNo"].ToString();
                        data.Day = sdr["Day"].ToString();
                        if (data.Frequency == "D" || data.Frequency == "W")
                        {
                            data.DayToDisplay = data.Day;
                        }
                        else if (data.Frequency == "Q")
                        {

                            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                            string strMonthName = mfi.GetMonthName(Convert.ToInt32(data.Month)).ToString();

                            data.DayToDisplay = data.DayNo + (string.IsNullOrEmpty(data.DayNo) ? "" : data.DayNo == "1" ? "st" : data.DayNo == "2" ? "nd" : data.DayNo == "3" ? "rd" : "th") + " " + data.Day + " of " + strMonthName;
                        }
                        else
                        {
                            data.DayToDisplay = "";
                        }
                        if (data.Day.Equals("Monday", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DayColor = "#974706";
                        }
                        else if (data.Day.Equals("Tuesday", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DayColor = "#ffff00";
                        }
                        else if (data.Day.Equals("Wednesday", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DayColor = "#632523";
                        }
                        else if (data.Day.Equals("Thursday", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DayColor = "#c00000";
                        }
                        else if (data.Day.Equals("Friday", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DayColor = "#1f497d";
                        }
                        else if (data.Day.Equals("Saturday", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DayColor = "#4f81bd";
                        }
                        else if (data.Day.Equals("Sunday", StringComparison.OrdinalIgnoreCase))
                        {
                            data.DayColor = "#4f6228";
                        }
                        else
                        {
                            data.DayColor = "";
                        }
                        data.MachineCondition = sdr["Machine_Condition"].ToString();
                        data.Time = sdr["Time"].ToString();
                        data.Remarks = sdr["Remarks"].ToString();
                        data.Min = sdr["MinValue"].ToString();
                        data.Max = sdr["MaxValue"].ToString();
                        data.Unit = sdr["Unit"].ToString();
                        data.MethodType = sdr["CheckpointType"].ToString();
                        data.ParameterID = sdr["ParameterID"].ToString();
                        data.ParameterName = sdr["ParameterName"].ToString();
                        data.DataType = sdr["DataType"].ToString();
                        data.Manager = sdr["Manager"].ToString();
                        data.GroupLeader = sdr["GroupLeader"].ToString();
                        data.DrawingName = sdr["FileName"].ToString();
                        if (sdr["FileData"].ToString() == "")
                        {
                            data.DrawingInBase64 = "";
                        }
                        else
                        {
                            byte[] bytes = (byte[])sdr["FileData"];
                            data.DrawingInBase64 = Convert.ToBase64String(bytes);
                        }
                        data.IsActionRequired = isModifiable;
                        data.Param = "View";
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetJHMasterDetails: " + ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static string SaveJHMasterDetails(JHMasterDetails data)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string output = "";
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_Get_JH_CheckList_MasterDetails_Bajaj]", con);
                cmd.Parameters.Add(new SqlParameter("@RevNo", data.RevNo));
                cmd.Parameters.Add(new SqlParameter("@OldRevNo", data.OldRevNo));
                if (!string.IsNullOrEmpty(data.RevDate))
                {
                    cmd.Parameters.Add(new SqlParameter("@RevDate", Util.GetDateTime(data.RevDate).ToString("yyyy-MM-dd")));
                }

                cmd.Parameters.Add(new SqlParameter("@OldmachineID", data.OldMachine));
                cmd.Parameters.Add(new SqlParameter("@MachineID", data.Machine));
                cmd.Parameters.Add(new SqlParameter("@Manager", data.Manager));
                cmd.Parameters.Add(new SqlParameter("@GroupLeader", data.GroupLeader));
                //cmd.Parameters.Add(new SqlParameter("@", data.MachineInterfaceID));
                if (!string.IsNullOrEmpty(data.CheckPointNo))
                {
                    cmd.Parameters.Add(new SqlParameter("@CheckpointNo", Convert.ToInt32(data.CheckPointNo)));
                }
                if (!string.IsNullOrEmpty(data.RouteNo))
                {
                    cmd.Parameters.Add(new SqlParameter("@RouteNo", Convert.ToInt32(data.RouteNo)));
                }
                cmd.Parameters.Add(new SqlParameter("@RelatedTo", data.RelatedTo));
                cmd.Parameters.Add(new SqlParameter("@Frequency", data.Frequency));
                cmd.Parameters.Add(new SqlParameter("@C_L_I_R_T", data.C_L_I_RT_Value));
                cmd.Parameters.Add(new SqlParameter("@Item", data.Item));
                cmd.Parameters.Add(new SqlParameter("@Checkpoint", data.CheckPoint));
                cmd.Parameters.Add(new SqlParameter("@Standard", data.Standard));
                cmd.Parameters.Add(new SqlParameter("@If_NotOK", data.IfNotOk));
                cmd.Parameters.Add(new SqlParameter("@Method", data.Method));
                cmd.Parameters.Add(new SqlParameter("@Day", data.Day));
                cmd.Parameters.Add(new SqlParameter("@DayNo", string.IsNullOrEmpty(data.DayNo) ? 0 : Convert.ToInt32(data.DayNo)));
                cmd.Parameters.Add(new SqlParameter("@MonthNo", string.IsNullOrEmpty(data.Month) ? 0 : Convert.ToInt32(data.Month)));
                cmd.Parameters.Add(new SqlParameter("@Machine_Condition", data.MachineCondition));
                if (!string.IsNullOrEmpty(data.Time))
                {
                    cmd.Parameters.Add(new SqlParameter("@Time", Convert.ToInt32(data.Time)));
                }
                cmd.Parameters.Add(new SqlParameter("@Remarks", data.Remarks));
                if (!string.IsNullOrEmpty(data.Min))
                {
                    cmd.Parameters.Add(new SqlParameter("@MinValue", Convert.ToDecimal(data.Min)));
                }
                if (!string.IsNullOrEmpty(data.Max))
                {
                    cmd.Parameters.Add(new SqlParameter("@MaxValue", Convert.ToInt32(data.Max)));
                }
                cmd.Parameters.Add(new SqlParameter("@Unit", data.Unit));
                cmd.Parameters.Add(new SqlParameter("@CheckpointType", data.MethodType));
                cmd.Parameters.Add(new SqlParameter("@ParameterID", data.ParameterID));
                cmd.Parameters.Add(new SqlParameter("@ParameterName", data.ParameterName));
                cmd.Parameters.Add(new SqlParameter("@DataType", data.DataType));
                cmd.Parameters.AddWithValue("@FileData", data.Drawing);
                cmd.Parameters.AddWithValue("@FileName", data.DrawingName);
                cmd.Parameters.Add(new SqlParameter("@Param", data.Param));
                cmd.Parameters.Add(new SqlParameter("@Param1", data.NewOrEdit));
                cmd.Parameters.Add(new SqlParameter("@CopyType", data.CopyType));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {

                        output = sdr["SaveFlag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveJHMasterDetails: " + ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return output;
        }

        #endregion

        #region ----- JH Report Details ------
        internal static DataTable GetJHReportDetails(string machine, string year, string month, out DataTable dtWeeklyDetails, out DataTable dtQuaterlyDetails)
        {
            DataTable dtDailyDetails = new DataTable();
            dtWeeklyDetails = new DataTable();
            dtQuaterlyDetails = new DataTable();
            DataTable dtMonthlyDetails = new DataTable();
            SqlDataReader sdr = null;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_Get_JH_CheckList_TransactionDetails_Bajaj]", con);
                cmd.Parameters.Add(new SqlParameter("@MachineID", machine));
                cmd.Parameters.Add(new SqlParameter("@YearNo", year));
                cmd.Parameters.Add(new SqlParameter("@MonthNo", month));
                cmd.Parameters.Add(new SqlParameter("@Param", "ReportView"));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dtDailyDetails.Load(sdr);
                dtWeeklyDetails.Load(sdr);
                dtMonthlyDetails.Load(sdr);
                dtQuaterlyDetails.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetJHReportDetails: " + ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dtDailyDetails;
        }
        #endregion

        #region ----- Process Parameter Master -----
        internal static List<string> GetAllGroupIDs()
        {
            List<string> GroupIDsList = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct GroupID from GroupInformation_MandM", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        GroupIDsList.Add(rdr["GroupID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAllGroupIDs: " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return GroupIDsList;
        }

        internal static List<string> GetAllParameterIDs()
        {
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {

                string sqlQuery = "select * from ParameterInformation_MandM";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["ParameterID"]))
                        {
                            list.Add(sdr["ParameterID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAllParameterIDs: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static string GetSelectedParameterName(string parameterId)
        {
            string parameter = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                string sqlQuery = "select * from ParameterInformation_MandM where ParameterID=@paramId";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@paramId", parameterId);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["ParameterName"]))
                        {
                            parameter = sdr["ParameterName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetSelectedParameterName: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return parameter;
        }
        internal static string GetControllerType(string machineID)
        {
            string ctype = string.Empty;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                //sqlConn.Open();
                SqlCommand cmd = new SqlCommand(@"select ControllerType from machineinformation where machineid=@machineid", sqlConn);
                cmd.Parameters.AddWithValue("@machineid", machineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ctype = rdr["ControllerType"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetControllerType: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return ctype;
        }

        internal static List<ProcessParameterMasterDetails> GetProcessParameterMasterDetails(string machine, bool isModifiable)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ProcessParameterMasterDetails> list = new List<ProcessParameterMasterDetails>();
            ProcessParameterMasterDetails data = null;
            string query = string.Empty;
            //if (machine.Equals("All", StringComparison.OrdinalIgnoreCase))
            //    query = @"select ROW_NUMBER() OVER (order by IDD) as SerialNum, * from [ProcessParameterMaster_BajajIoT]";
            //else
            query = @"select ROW_NUMBER() OVER (order by IDD) as SerialNum, * from [ProcessParameterMaster_BajajIoT] where MachineID=@machine order by ParameterID";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machine", machine);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new ProcessParameterMasterDetails();
                        if (rdr["SerialNum"] != DBNull.Value)
                            data.SerialNum = rdr["SerialNum"].ToString();
                        if (rdr["IDD"] != DBNull.Value)
                            data.IDD = rdr["IDD"].ToString();
                        if (rdr["MachineID"] != DBNull.Value)
                            data.MachineId = rdr["MachineID"].ToString();
                        data.SourceType = rdr["SourceType"].ToString();
                        if (rdr["ParameterID"] != DBNull.Value)
                            data.ParameterId = rdr["ParameterID"].ToString();
                        data.ParameterName = rdr["ParameterName"].ToString();
                        data.DisplayText = rdr["DisplayText"].ToString();
                        data.LowerValue = rdr["LowerValue"].ToString();
                        data.HigherValue = rdr["HigherValue"].ToString();
                        data.HighRedLimit = rdr["HighRedLimit"].ToString();
                        data.LowRedLimit = rdr["LowerRedLimit"].ToString();
                        data.HighGreenLimit = rdr["HighGreenLimit"].ToString();
                        data.LowGreenLimit = rdr["LowerGreenLimt"].ToString();
                        data.PollingType = rdr["PullingType"].ToString();
                        data.DataReadAddress = rdr["DataReadAddress"].ToString();
                        data.SourceDataType = rdr["SourceDataType"].ToString();
                        data.Unit = rdr["Unit"].ToString();
                        data.TemplateType = rdr["TemplateType"].ToString();
                        data.DBDataType = rdr["DBDataType"].ToString();
                        switch (rdr["PullingFreq"].ToString())
                        {
                            case "0.5":
                                data.Frequency = "500 ms";
                                break;
                            case "1":
                                data.Frequency = "1 Sec";
                                break;
                            case "60":
                                data.Frequency = "1 Min";
                                break;
                            case "120":
                                data.Frequency = "2 Min";
                                break;
                            case "300":
                                data.Frequency = "5 Min";
                                break;
                            case "600":
                                data.Frequency = "10 Min";
                                break;
                            case "900":
                                data.Frequency = "15 Min";
                                break;
                            case "1200":
                                data.Frequency = "20 Min";
                                break;
                            case "3600":
                                data.Frequency = "1 Hr";
                                break;

                        }
                        if (!string.IsNullOrEmpty(rdr["IsEnabled"].ToString()))
                        {
                            data.IsVisible = rdr["IsEnabled"].ToString() == "1" ? true : false;
                        }
                        data.IsGraphVisible = string.IsNullOrEmpty(rdr["GraphView"].ToString()) ? false : rdr["GraphView"].ToString().Equals("True") ? true : false;
                        data.IsDashboardVisible = string.IsNullOrEmpty(rdr["DashboardView"].ToString()) ? false : rdr["DashboardView"].ToString().Equals("True") ? true : false;
                        data.IsMobileVisible = string.IsNullOrEmpty(rdr["EnabledForMobile"].ToString()) ? false : rdr["EnabledForMobile"].ToString().Equals("True") ? true : false;
                        data.IsAlertVisible = string.IsNullOrEmpty(rdr["AlertView"].ToString()) ? false : rdr["AlertView"].ToString().Equals("True") ? true : false;
                        if (rdr["SortOrder"] != DBNull.Value)
                            data.SortOrder = Convert.ToInt32(rdr["SortOrder"].ToString());
                        data.IsActionRequired = isModifiable;
                        list.Add(data);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProcessParameterMasterDetails: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        internal static string SaveProcessParameterMasteDetails(ProcessParameterMasterDetails data)
        {
            //bool isUpdated = false;
            string output = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                //,[LowerValue]=@lowerValue,[HigherValue]=@higherValue
                string query = @"
                                IF @Param='New'
                                begin
                                IF EXISTS(Select * from ProcessParameterMaster_BajajIoT where [MachineID]=@machineId and [ParameterID] = @ParameterID)
                                begin
                                 select 'Exist' as SaveFlag;
                                return
                                end
                                end


                            IF EXISTS(Select * from ProcessParameterMaster_BajajIoT where [MachineID]=@machineId and [ParameterID] = @ParameterID)
                               BEGIN
                               UPDATE ProcessParameterMaster_BajajIoT set PullingType=@PullingType,DataReadAddress=@DataReadAddress,SourceDataType=@SourceDataType,[MachineID]=@machineId,[GraphView]=@GraphView,[SourceType]=@sourcetype,[PullingFreq]=@Frequency,  [ParameterName] = @ParameterName, [DisplayText]=@displayText,[HighRedLimit]=@HRL,[LowerRedLimit]=@LRL,[HighGreenLimit]=@HGL,[LowerGreenLimt]=@LGL,[Unit]=@Unit,[TemplateType]= @TemplateType,[DBDataType]=@DBtype,[IsEnabled] = @IsVisible,[SortOrder] = @SortOrder,[DashboardView]=@DashboardView,[AlertView]=@AlertView,[EnabledForMobile]=@MobileView,DataScreenGroup=@DataScreenGroup where [MachineID]=@machineId and [ParameterID] = @ParameterID;
                                    
                                select 'Updated' as SaveFlag;
                               END
                               ELSE
                               BEGIN
                               Insert into ProcessParameterMaster_BajajIoT(MachineID,[ParameterID], [ParameterName],[PullingFreq], [SourceType], DisplayText, HighRedLimit, LowerRedLimit, HighGreenLimit,LowerGreenLimt, [Unit], [TemplateType],DBDataType, [IsEnabled], [SortOrder],[GraphView],[DashboardView],[AlertView],[EnabledForMobile],SourceDataType,PullingType,DataReadAddress,DataScreenGroup) 
                                                                            Values(@machineId,@ParameterID, @ParameterName,@Frequency, @sourcetype, @displayText, @HRL, @LRL, @HGL, @LGL, @Unit, @TemplateType,@DBtype, @IsVisible, @SortOrder,@GraphView,@DashboardView,@AlertView,@MobileView,@SourceDataType,@PullingType,@DataReadAddress,@DataScreenGroup) 

                                      select 'Inserted' as SaveFlag;

                              END";
                // LowerValue, HigherValue, , @lowerValue, @higherValue,
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@IDD", data.IDD));
                cmd.Parameters.Add(new SqlParameter("@ParameterID", data.ParameterId));
                cmd.Parameters.Add(new SqlParameter("@ParameterName", string.IsNullOrEmpty(data.ParameterName) ? data.ParameterId : data.ParameterName));
                cmd.Parameters.Add(new SqlParameter("@machineId", data.MachineId));
                cmd.Parameters.Add(new SqlParameter("@sourcetype", string.IsNullOrEmpty(data.SourceType) ? "" : data.SourceType));
                cmd.Parameters.Add(new SqlParameter("@displayText", (data.DisplayText)));
                //cmd.Parameters.Add(new SqlParameter("@lowerValue", string.IsNullOrEmpty((data.LowerValue) ? "0" : (data.LowerValue));
                //cmd.Parameters.Add(new SqlParameter("@higherValue", string.IsNullOrEmpty((data.HigherValue) ? "0" : (data.HigherValue));
                cmd.Parameters.Add(new SqlParameter("@HRL", string.IsNullOrEmpty(data.HighRedLimit) ? "0" : data.HighRedLimit));
                cmd.Parameters.Add(new SqlParameter("@LRL", string.IsNullOrEmpty(data.LowRedLimit) ? "0" : data.LowRedLimit));
                cmd.Parameters.Add(new SqlParameter("@HGL", string.IsNullOrEmpty(data.HighGreenLimit) ? "0" : data.HighGreenLimit));
                cmd.Parameters.Add(new SqlParameter("@LGL", string.IsNullOrEmpty(data.LowGreenLimit) ? "0" : data.LowGreenLimit));
                cmd.Parameters.Add(new SqlParameter("@Unit", string.IsNullOrEmpty(data.Unit) ? "" : data.Unit));
                cmd.Parameters.Add(new SqlParameter("@TemplateType", string.IsNullOrEmpty(data.TemplateType) ? "" : data.TemplateType));
                cmd.Parameters.Add(new SqlParameter("@DBtype", string.IsNullOrEmpty(data.DBDataType) ? "" : data.DBDataType));
                cmd.Parameters.Add(new SqlParameter("@IsVisible", data.IsVisible));

                cmd.Parameters.Add(new SqlParameter("@SourceDataType", data.SourceDataType));
                cmd.Parameters.Add(new SqlParameter("@PullingType", data.PollingType));
                cmd.Parameters.Add(new SqlParameter("@DataReadAddress", data.DataReadAddress));

                cmd.Parameters.Add(new SqlParameter("@GraphView", data.IsGraphVisible));
                cmd.Parameters.Add(new SqlParameter("@DashboardView", data.IsDashboardVisible));
                cmd.Parameters.Add(new SqlParameter("@AlertView", data.IsAlertVisible));
                cmd.Parameters.Add(new SqlParameter("@MobileView", data.IsMobileVisible));
                cmd.Parameters.Add(new SqlParameter("@DataScreenGroup", "Live Dashboard"));
                double freq = 0;
                switch (data.Frequency)
                {
                    case "500 ms":
                        freq = 0.5;
                        break;
                    case "1 Sec":
                        freq = 1;
                        break;
                    case "1 Min":
                        freq = 60;
                        break;
                    case "2 Min":
                        freq = 120;
                        break;
                    case "5 Min":
                        freq = 300;
                        break;
                    case "10 Min":
                        freq = 600;
                        break;
                    case "15 Min":
                        freq = 900;
                        break;
                    case "20 Min":
                        freq = 1200;
                        break;
                    case "1 Hr":
                        freq = 3600;
                        break;
                }
                cmd.Parameters.Add(new SqlParameter("@Frequency", freq));
                cmd.Parameters.Add(new SqlParameter("@SortOrder", data.SortOrder));
                cmd.Parameters.Add(new SqlParameter("@Param", data.NewOrEdit));
                //int rowsAffected = cmd.ExecuteNonQuery();
                //if (rowsAffected > 0)
                //    isUpdated = true;
                //else
                //    isUpdated = false;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        output = sdr["SaveFlag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveProcessParameterMasteDetails: " + ex.Message);
                //isUpdated = false;
                output = "Error";
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return output;
        }
        internal static void DeleteProcessParameterMasterData(string idd, out bool isDeleted)
        {
            isDeleted = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"Delete from [dbo].[ProcessParameterMaster_BajajIoT] where IDD = @IDD";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IDD", idd);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                isDeleted = false;
                Logger.WriteErrorLog("DeleteProcessParameterMasterData: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region ---- Operator Message Details ----
        internal static List<OperatorMessageDetails> GetOperatorMessageDetails(string fromDate, string toDate, string plant, string machine)
        {
            List<OperatorMessageDetails> list = new List<OperatorMessageDetails>();
            OperatorMessageDetails data = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from operatormessageinfo where (MachineID=@MachineID or isnull(@MachineID,'')='') and AlarmTime>= @Fromdate and AlarmTime<=@Todate", con);
                cmd.Parameters.Add(new SqlParameter("@Fromdate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("@Todate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machine));
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new OperatorMessageDetails();
                        data.AlarmNo = sdr["AlarmNo"].ToString();
                        data.AlarmDate = sdr["AlarmTime"].ToString();
                        data.AlarmMessage = sdr["AlarmMessage"].ToString();
                        data.GroupNo = sdr["AlarmGroupNo"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetOperatorMessageDetails: " + ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        #endregion

        #region ----Tool Life Details ----
        internal static List<FocasToolLifeDetails> GetToolLifeDetails(string fromDate, string toDate, string plant, string machine)
        {
            List<FocasToolLifeDetails> list = new List<FocasToolLifeDetails>();
            FocasToolLifeDetails data = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"s_Focas_getToolLifedetails", con);
                cmd.Parameters.AddWithValue("@machineid", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@fromTime", DataBaseAccess.GetLogicalDay(Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.AddWithValue("@ToTime", DataBaseAccess.GetLogicalDayEnd(Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.AddWithValue("@Param", "");
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new FocasToolLifeDetails();
                        data.MachineID = sdr["Machineid"].ToString();
                        data.ProgramNo = sdr["ProgramNo"].ToString();
                        data.ToolNo = sdr["ToolNo"].ToString();
                        data.ToolDesc = sdr["ToolDescription"].ToString();
                        data.NoOfTimeChanged = sdr["NoOfTimesChanged"].ToString();
                        data.ChangeTime = sdr["ChangeTime"].ToString();
                        data.Type = sdr["Type"].ToString();
                        data.ToolTarget = sdr["ToolTarget"].ToString();
                        data.ToolActual = sdr["ToolActual"].ToString();
                        data.RemainingToolLife = sdr["RemainingToolLife"].ToString();
                        data.PartsCount = sdr["PartCount"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetOperatorMessageDetails: " + ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        #endregion
    }
}