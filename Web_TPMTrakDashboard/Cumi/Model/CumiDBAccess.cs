using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi.Model
{
    public class CumiDBAccess
    {
        public static List<string> GetAllMachinedByPlant(string Plantid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct MachineID from PlantMachine where (PlantID=@Plantid or @Plantid='' ) order by MachineID ", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Plantid", Plantid);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["MachineID"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAllMachinedByPlan: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        public static List<string> GetDownCodeForCategory(string downCategory)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct downid from downcodeinformation where (Catagory =@Catagory or ISNULL(@Catagory,'')='')", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Catagory", downCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : downCategory);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["downid"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDownCodeForCategory: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        #region ----Production Rejection  Details ----
        internal static DataTable GetProductionRejectionDetails(string procName, string screenType, string viewType, string plant, string machine, string fromDate, string toDate, string shift, string year, string month, bool isShiftReqiredForReport, out DataTable dtTotal, out DataTable dtRightTotal, out DataTable dtRightTotal_1)
        {
            DataTable dt = new DataTable();
            dtTotal = new DataTable();
            dtRightTotal = new DataTable();
            dtRightTotal_1 = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                shift = shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift;
                SqlCommand cmd = new SqlCommand(procName, con);
                cmd.Parameters.Add(new SqlParameter("@PlantID", plant));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machine));
                cmd.Parameters.Add(new SqlParameter("@param", screenType));
                if (procName.Equals("[SP_HourlyProductionAndRejectionData_CUMI]", StringComparison.OrdinalIgnoreCase) || procName.Equals("[SP_ShiftProductionAndRejectionData_CUMI]", StringComparison.OrdinalIgnoreCase) || procName.Equals("[SP_AggregatedShiftProductionAndRejectionData_CUMI]", StringComparison.OrdinalIgnoreCase))
                {

                    if (isShiftReqiredForReport)
                    {
                        cmd.Parameters.Add(new SqlParameter("@ShiftName", shift));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@ShiftName", viewType == "Report" ? "" : shift));
                    }
                    cmd.Parameters.Add(new SqlParameter("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                if (procName.Equals("[SP_DaywiseProductionAndRejectionData_CUMI]", StringComparison.OrdinalIgnoreCase) || procName.Equals("[SP_WeeklyProductionAndRejectionData_CUMI]", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.Add(new SqlParameter("@Year", year));
                    cmd.Parameters.Add(new SqlParameter("@Month", month));
                }
                if (procName.Equals("[SP_MonthwiseProductionAndRejectionData_CUMI]"))
                {
                    cmd.Parameters.Add(new SqlParameter("@Year", year));
                }


                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dtTotal.Load(sdr);
                dtRightTotal.Load(sdr);
                if (screenType.Equals("AcceptedScreen", StringComparison.OrdinalIgnoreCase) && procName.Equals("[SP_HourlyProductionAndRejectionData_CUMI]", StringComparison.OrdinalIgnoreCase))
                {
                    dtRightTotal_1.Load(sdr);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionRejectionDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();

            }
            return dt;
        }


        //internal static List<ProcessParameterSPCDataInfo> GetProcessParameterSPCData(string fromDate, string toDate, string machineid, out DataTable dt)
        internal static DataTable GetProcessParameterSPCData(string fromDate, string toDate, string machineid, out DataTable dt1)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ProcessParameterSPCDataInfo> list = new List<ProcessParameterSPCDataInfo>();
            ProcessParameterSPCDataInfo data = null;
            DataTable dt = new DataTable();
            dt1 = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand("s_GetDaywiseEnergyandSPCTrend_CUMI", sqlConn);//@"s_GetLookups"
                cmd.CommandTimeout = 450;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", "'" + machineid + "'");
                cmd.Parameters.AddWithValue("@PlantID", "");
                cmd.Parameters.AddWithValue("@param", "");
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(rdr);
                dt1.Load(rdr);
                //if (rdr.HasRows)
                //{

                //    ProcessParameterSPCDataInfo data1 = new ProcessParameterSPCDataInfo();
                //    //data1.AvgKwh = rdr["AvgKwh"].ToString();
                //    //data1.AvgDailyTonnage = rdr["AvgDailyTonnage"].ToString();
                //    //data1.AvgSPC = rdr["AvgSPC"].ToString();
                //    while (rdr.Read())
                //    {
                //        data = new ProcessParameterSPCDataInfo();
                //        data.MachineId = rdr["MachineID"].ToString();
                //        data.StartDate = rdr["StartDate"].ToString();
                //        data.Kwh = rdr["Kwh"].ToString();
                //        data.DailtTonnage = rdr["Dailytonnage"].ToString();
                //        data.SPC = rdr["SPC"].ToString();
                //        data1.ProcessParameterSPCData.Add(data);
                //    }
                //    list.Add(data1);
                //    dt.Load(rdr);
                //}
                //rdr.NextResult();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProcessParameterDetails: " + ex.ToString());
            }
            finally
            {

                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }

        internal static List<string> BindAllMachinesSPC()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> machineid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT [MachineId] FROM [dbo].[EM_Machineinformation] where IsDashboardEnabled=1", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    machineid.Add(rdr["MachineId"].ToString());
                }
                //rdr.Close();
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
            return machineid;
        }

        //internal static DataTable GetAllParametersByMachines(string machines)
        //{
        //    SqlConnection sqlConn = ConnectionManager.GetConnection();
        //    DataTable dt = new DataTable();
        //    List<MachineParameters> list = new List<MachineParameters>();
        //    try
        //    {
        //        var machineIds = machines.Split(',');
        //        for (int i = 0; i < machineIds.Length; i++)
        //        {
        //            SqlCommand cmd = new SqlCommand("select distinct ParameterID, DisplayText from ProcessParameterMaster_BajajIoT where (MachineID=@MachineID or @MachineID='' ) ", sqlConn);//@"s_GetLookups"
        //            cmd.CommandType = System.Data.CommandType.Text;
        //            cmd.Parameters.AddWithValue("@MachineID", machineIds[i]);
        //            SqlDataReader rdr = cmd.ExecuteReader();
        //            dt.Load(rdr);
        //            //while (rdr.Read())
        //            //{
        //            //    //MachineParameters parameter = new MachineParameters();
        //            //    //parameter.ParameterID = rdr["ParameterID"].ToString();
        //            //    //parameter.DisplayText = rdr["DisplayText"].ToString();
        //            //    //list.Add(parameter);
        //            //}
        //            rdr.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("GetAllMachinedByPlan: " + ex.ToString());
        //    }
        //    finally
        //    {
        //        if (sqlConn != null) sqlConn.Close();
        //    }
        //    return dt;
        //}
        internal static List<MachineParameters> GetAllParametersByMachines(string machines)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<MachineParameters> list = new List<MachineParameters>();
            try
            {
                var machineIds = machines.Split(',');
                List<string> MachinesInve = new List<string>();

                foreach (var mach in machineIds)
                {
                    MachinesInve.Add("'" + mach + "'");
                }
                machines = String.Join(",", MachinesInve);
                //for (int i = 0; i < machineIds.Length; i++)
                //{
                SqlCommand cmd = new SqlCommand("select distinct ParameterID, DisplayText from ProcessParameterMaster_BajajIoT where MachineID in (" + machines + ")  union select distinct ParameterID, DisplayText from ProcessParameterMaster_BajajIoT where ParameterID = '6'", sqlConn);//@"s_GetLookups"
                                                                                                                                                                                                                                                                                             //SqlCommand cmd = new SqlCommand("select distinct ParameterID, DisplayText from ProcessParameterMaster_BajajIoT where (MachineID=@MachineID or @MachineID='' ) ", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machines);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    MachineParameters parameter = new MachineParameters();
                    parameter.ParameterID = rdr["ParameterID"].ToString();
                    parameter.DisplayText = rdr["DisplayText"].ToString();
                    list.Add(parameter);
                }
                //rdr.Close();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAllMachinedByPlan: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
      
        internal static DataTable GetProductionRejectionParetoChartDetails(string plant, string machine, string fromDate, string toDate)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[SP_ViewRejectionReasonWiseCount_CUMI]", con);
                cmd.Parameters.Add(new SqlParameter("@PlantID", plant));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machine));
                cmd.Parameters.Add(new SqlParameter("@StartTime", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("@EndTime", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionRejectionParetoChartDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();

            }
            return dt;
        }
        #endregion

        #region ---- Plan Entry Details---
        public static List<ListItem> GetERPPODetails()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ListItem> list = new List<ListItem>();
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct ProductionOrder from ProductionOrder_CUMI order by ProductionOrder", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Text = rdr["ProductionOrder"].ToString(), Value = rdr["ProductionOrder"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetERPPODetails: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }     
        public static List<ListItem> GetERPItemDetails(string PO)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ListItem> list = new List<ListItem>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct A1.ItemCode, A2.description,(CASE when A1.ItemCode=A2.componentid  THEN '1' ELSE '0' end)AS MasterFlag from ProductionOrder_CUMI A1 
left join (select * from componentinformation) A2
on A1.ItemCode=A2.componentid 
where A1.ProductionOrder=@PO", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PO", PO);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Text = rdr["ItemCode"].ToString(), Value = rdr["description"].ToString() + ";;;" + rdr["MasterFlag"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetERPItemDetails: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        public static List<ListItem> GetComponentInformation()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ListItem> list = new List<ListItem>();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from componentinformation", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Text = rdr["componentid"].ToString(), Value = rdr["description"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetComponentInformation: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        public static List<PlanDetails> GetPlanDetails(string machineid, string fromDate, string toDate)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<PlanDetails> list = new List<PlanDetails>();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from MachineLevelTargetCountDetails_CUMI where (MachineID=@MachineID or @MachineID='' ) and  [Date]>=@FromDate and [Date]<=@ToDate order by MachineID,Date,shift ", sqlConn);//@"s_GetLookups"
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineid == "All" ? "" : machineid);
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd"));
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new PlanDetails() { MachineID = rdr["MachineID"].ToString(), Date = Util.GetDateTime(rdr["Date"].ToString()).ToString("dd-MM-yyyy"), Shift = rdr["shift"].ToString(), PONo = rdr["WorkOrderNumber"].ToString(), ItemCode = rdr["ComponentID"].ToString(), PlanValue = rdr["PlannedQty"].ToString(), IDD = rdr["RowID"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPlanDetails: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        internal static string SavePlanDetails(PlanDetails data)
        {
            //bool isUpdated = false;
            string output = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                //,[LowerValue]=@lowerValue,[HigherValue]=@higherValue
                string query = @"
                                   --IF @Param='New'
                                   --begin
                                   --IF EXISTS(Select * from MachineLevelTargetCountDetails_CUMI where [MachineID]=@machineId and [Date] = @Date and Shift=@Shift)
                                   --begin
                                  -- select 'Exist' as SaveFlag;
                                   --return
                                   --end
                                   --end


                            IF EXISTS(Select * from MachineLevelTargetCountDetails_CUMI where [MachineID]=@machineId and [Date] = @Date and [Shift]=@Shift and ComponentID=@ItemCode and WorkOrderNumber=@PONo)
                               BEGIN
                               UPDATE MachineLevelTargetCountDetails_CUMI set PlannedQty=@PlanValue 
                                where  [MachineID]=@machineId and [Date] = @Date and [Shift]=@Shift and  ComponentID=@ItemCode and WorkOrderNumber=@PONo;
                                    
                                select 'Updated' as SaveFlag;
                               END
                               ELSE
                               BEGIN
                               Insert into MachineLevelTargetCountDetails_CUMI(MachineID,[Date], [shift],[PlannedQty],WorkOrderNumber,ComponentID) 
                                                                            Values(@machineId,@Date, @Shift,@PlanValue,@PONo,@ItemCode) 

                                      select 'Inserted' as SaveFlag;

                              END";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@IDD", data.IDD));
                cmd.Parameters.Add(new SqlParameter("@Date", Util.GetDateTime(data.Date).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("@machineId", data.MachineID));
                cmd.Parameters.Add(new SqlParameter("@Shift", data.Shift));
                cmd.Parameters.Add(new SqlParameter("@PONo", data.PONo));
                cmd.Parameters.Add(new SqlParameter("@ItemCode", data.ItemCode));
                cmd.Parameters.Add(new SqlParameter("@PlanValue", (data.PlanValue)));
                cmd.Parameters.Add(new SqlParameter("@Param", data.NewOrEdit));
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
                Logger.WriteErrorLog("SavePlanDetails: " + ex.Message);
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
        internal static void DeletePlanData(string idd, out bool isDeleted)
        {
            isDeleted = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = @"Delete from MachineLevelTargetCountDetails_CUMI where RowID = @IDD";
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
                Logger.WriteErrorLog("DeletePlanData: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region ----- Process Parameter Master -----
        internal static List<ProcessParameterMasterDetails> GetProcessParameterMasterDetails(string machine, string component)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ProcessParameterMasterDetails> list = new List<ProcessParameterMasterDetails>();
            ProcessParameterMasterDetails data = null;
            string query = string.Empty;
            query = @"select ROW_NUMBER() OVER (order by IDD) as SerialNum, * from [ProcessParameterMaster_BajajIoT] where MachineID=@Machine order by SortOrder";
            //and ComponentID = @ComponentID
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machine", machine);
                //cmd.Parameters.AddWithValue("@ComponentID", component);
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
                        if (rdr["ComponentID"] != DBNull.Value)
                            data.ComponentId = rdr["ComponentID"].ToString();
                        if (rdr["ParameterID"] != DBNull.Value)
                            data.ParameterId = rdr["ParameterID"].ToString();
                        data.ParameterName = rdr["ParameterName"].ToString();
                        data.DisplayText = rdr["DisplayText"].ToString();
                        data.ParameterRegister = rdr["ParameterRegisterID"].ToString();
                        data.LowerValue = rdr["LowerValue"].ToString();
                        data.HigherValue = rdr["HigherValue"].ToString();
                        data.SourceDataType = rdr["SourceDataType"].ToString();
                        data.Unit = rdr["Unit"].ToString();
                        data.TemplateType = rdr["TemplateType"].ToString();
                        data.MinRegister = rdr["MinRegister"].ToString();
                        data.MaxRegister = rdr["MaxRegister"].ToString();
                        data.ChartType = rdr["ChartType"].ToString();
                        if (!string.IsNullOrEmpty(rdr["IsEnabled"].ToString()))
                        {
                            data.IsVisible = rdr["IsEnabled"].ToString() == "1" ? true : false;
                        }
                        if (rdr["SortOrder"] != DBNull.Value)
                            data.SortOrder = Convert.ToInt32(rdr["SortOrder"].ToString());
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
            string output = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                //and[ComponentID] = @ComponentId
                //and[ComponentID] = @ComponentId
                //and[ComponentID] = @ComponentId
                //ComponentID, @ComponentId
                string query = @"
                                IF @Param='New'
                                begin
                                IF EXISTS(Select * from ProcessParameterMaster_BajajIoT where [MachineID]=@machineId  and [ParameterID] = @ParameterID)
                                begin
                                 select 'Exist' as SaveFlag;
                                return
                                end
                                end


                            IF EXISTS(Select * from ProcessParameterMaster_BajajIoT where [MachineID]=@machineId   and [ParameterID] = @ParameterID)
                               BEGIN
                               UPDATE ProcessParameterMaster_BajajIoT set SourceDataType=@SourceDataType, [ParameterName] = @ParameterName, 
                              [DisplayText]=@displayText, ParameterRegisterID=@ParameterRegister, [LowerValue]=@LowerValue,[HigherValue]=@HigherValue,[Unit]=@Unit, 
                              [MinRegister]=@MinRegister,[MaxRegister]=@MaxRegister,
                              [TemplateType]= @TemplateType,[IsEnabled] = @IsVisible,[SortOrder] = @SortOrder, [ChartType]=@ChartType
                              where [MachineID]=@machineId  and [ParameterID] = @ParameterID;
                                    
                                select 'Updated' as SaveFlag;
                               END
                               ELSE
                               BEGIN
                              Insert into ProcessParameterMaster_BajajIoT(MachineID,  [ParameterID], [ParameterName], DisplayText, ParameterRegisterID, LowerValue, HigherValue,  [Unit], [TemplateType], [IsEnabled], [SortOrder],SourceDataType, [MinRegister], [MaxRegister], [ChartType]) 
Values(@machineId,@ParameterID, @ParameterName,@displayText,@ParameterRegister, @LowerValue, @HigherValue, @Unit, @TemplateType, @IsVisible, @SortOrder,@SourceDataType, @MinRegister, @MaxRegister,@ChartType) 


                                      select 'Inserted' as SaveFlag;

                              END";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@IDD", data.IDD));
                cmd.Parameters.Add(new SqlParameter("@ParameterID", data.ParameterId));
                cmd.Parameters.Add(new SqlParameter("@ParameterName", string.IsNullOrEmpty(data.ParameterName) ? data.ParameterId : data.ParameterName));
                cmd.Parameters.Add(new SqlParameter("@machineId", data.MachineId));
                //cmd.Parameters.Add(new SqlParameter("@ComponentId", data.ComponentId));
                cmd.Parameters.Add(new SqlParameter("@displayText", (data.DisplayText)));
                cmd.Parameters.Add(new SqlParameter("@ParameterRegister", (data.ParameterRegister)));
                cmd.Parameters.Add(new SqlParameter("@LowerValue", data.LowerValue));
                cmd.Parameters.Add(new SqlParameter("@HigherValue", data.HigherValue));
                cmd.Parameters.Add(new SqlParameter("@Unit", data.Unit));
                cmd.Parameters.Add(new SqlParameter("@MinRegister", data.MinRegister));
                cmd.Parameters.Add(new SqlParameter("@MaxRegister", data.MaxRegister));
                cmd.Parameters.Add(new SqlParameter("@TemplateType", string.IsNullOrEmpty(data.TemplateType) ? "" : data.TemplateType));
                cmd.Parameters.Add(new SqlParameter("@IsVisible", data.IsVisible));
                cmd.Parameters.Add(new SqlParameter("@SourceDataType", data.SourceDataType));
                cmd.Parameters.Add(new SqlParameter("@ChartType", data.ChartType));
                //cmd.Parameters.Add(new SqlParameter("@DataScreenGroup", "Live Dashboard"));
                cmd.Parameters.Add(new SqlParameter("@SortOrder", data.SortOrder));
                cmd.Parameters.Add(new SqlParameter("@Param", data.NewOrEdit));
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

        #region ---- Process Parameter Details---
        public static List<ProcessParametersInfo> GetProcessParameterDetails(string plant, string machine, string parameters, string fromdate, string todate, string shift, string param, out DataTable dt, out DataTable dtParam6)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ProcessParametersInfo> list = new List<ProcessParametersInfo>();
            ProcessParametersInfo data = null;
            dt = new DataTable();
            dtParam6 = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                var machines = machine.Split(',');
                List<string> MachinesInve = new List<string>();

                foreach (var mach in machines)
                {
                    MachinesInve.Add("'" + mach + "'");
                }
                machine = String.Join(",", MachinesInve);
                string tempParameters = parameters;
                if (param == "")
                {
                    parameters = parameters.Replace("6", "");
                }
                var parames = parameters != "" ? parameters.Split(',') : null;
                List<string> ParametersInve = new List<string>();

                if (parames != null)
                {
                    foreach (var para in parames)
                    {
                        ParametersInve.Add("'" + para + "'");
                    }
                }
                parameters = String.Join(",", ParametersInve);
                SqlCommand cmd = new SqlCommand("SP_ProcessParameterDashBoardView_CUMI", sqlConn);//@"s_GetLookups"
                cmd.CommandTimeout = 450;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@FromTime", Util.GetDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToTime", Util.GetDateTime(todate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@ParameterID", parameters);
                cmd.Parameters.AddWithValue("@Param", param);
                rdr = cmd.ExecuteReader();
                DataTable dtTemp = new DataTable();
                dtTemp.Load(rdr);
                if (param == "")
                {
                    // rdr.NextResult();
                    dt.Load(rdr);
                    dtParam6.Load(rdr);
                }
                if (dtTemp.Rows.Count > 0)
                {
                    string tempMachine = "";
                    foreach (DataRow row in dtTemp.Rows)
                    {
                        if (param == "" && (tempParameters.Contains("6") || tempParameters == ""))
                        {
                            if (tempMachine != row["machineid"].ToString())
                            {
                                if (dtParam6.Rows.Count > 0)
                                {
                                    data = getParameteValue(dtParam6.Rows[0], param);
                                    if (data != null)
                                    {
                                        data.MachineId = row["machineid"].ToString();
                                        list.Add(data);
                                    }
                                }
                            }
                            tempMachine = row["machineid"].ToString();
                        }
                        data = getParameteValue(row, param);
                        if (data != null && tempParameters != "6")
                        {
                            list.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProcessParameterDetails: " + ex.ToString());
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        private static ProcessParametersInfo getParameteValue(DataRow row, string param)
        {
            ProcessParametersInfo data = new ProcessParametersInfo();
            try
            {

                data.MachineId = row["machineid"].ToString();
                data.ParameterId = row["ParameterID"].ToString();
                data.ParameterName = row["ParameterName"].ToString();
                if (String.IsNullOrEmpty(data.ParameterName))
                {
                    return null;
                }
                //data.LowerValue = row["MasterMinValue"].ToString();
                //data.HigherValue = row["MasterMaxValue"].ToString();
                data.TemplateType = row["TemplateType"].ToString();
                data.MinValue = row["TranMinValue"].ToString();
                data.MaxValue = row["TranMaxValue"].ToString();
                data.Value = row["ParameterValue"].ToString();
                data.ChartType = row["ChartType"].ToString();
                data.Unit = row["Unit"].ToString();
                if (param == "")
                {
                    data.LowerValue = row["MasterMinValue"].ToString();
                    data.HigherValue = row["MasterMaxValue"].ToString();
                }
                else
                {
                    data.LowerValue = row["LowerValue"].ToString();
                    data.HigherValue = row["HigherValue"].ToString();
                    data.updatedtimestamp = Convert.ToDateTime(row["updatedtimestamp"]);
                }
                if (data.TemplateType.Equals("1 Value", StringComparison.OrdinalIgnoreCase))
                {
                    data.OneValueVisibility = "table-row";
                }
                else if (data.TemplateType.Equals("1 Value With High/Low", StringComparison.OrdinalIgnoreCase))
                {
                    data.OneValueVisibility = "table-row";
                    data.LowHighVisibility = "table-row";
                    if (!String.IsNullOrEmpty(data.LowerValue) && !String.IsNullOrEmpty(data.HigherValue) && !String.IsNullOrEmpty(data.Value))
                    {
                        if (InRange(Convert.ToDecimal(data.LowerValue), Convert.ToDecimal(data.HigherValue), Convert.ToDecimal(data.Value)))
                        {
                            data.BackgroundColor = "#49bd49";
                            data.ForeColor = "black";
                        }
                        else
                        {
                            data.BackgroundColor = "red";
                            data.ForeColor = "white";
                        }
                    }
                }
                else if (data.TemplateType.Equals("2 Value", StringComparison.OrdinalIgnoreCase))
                {
                    data.TwoValueVisibility = "table-row";
                }
                else if (data.TemplateType.Equals("2 Value With High/Low", StringComparison.OrdinalIgnoreCase))
                {
                    data.TwoValueVisibility = "table-row";
                    data.LowHighVisibility = "table-row";
                    if (!String.IsNullOrEmpty(data.LowerValue) && !String.IsNullOrEmpty(data.HigherValue) && !String.IsNullOrEmpty(data.MinValue) && !String.IsNullOrEmpty(data.MaxValue))
                    {
                        if (InRange(Convert.ToDecimal(data.LowerValue), Convert.ToDecimal(data.HigherValue), Convert.ToDecimal(data.MinValue)) && InRange(Convert.ToDecimal(data.LowerValue), Convert.ToDecimal(data.HigherValue), Convert.ToDecimal(data.MaxValue)))
                        {
                            data.BackgroundColor = "#49bd49";
                            data.ForeColor = "black";
                        }
                        else
                        {
                            data.BackgroundColor = "red";
                            data.ForeColor = "white";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return data;
        }

        public static bool InRange(decimal low, decimal high, decimal x)
        {
            return ((x - high) * (x - low) <= 0);
        }
        #endregion


        #region ----- Production DayWise Report Details ----
        internal static DataTable GetProductionDayWiseReportDetails(string plant, string machine, string fromDate, string toDate, string empid)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[SP_DayWiseProductionReport_CUMI]", con);
                cmd.Parameters.Add(new SqlParameter("@PlantID", plant));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", empid));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machine));
                cmd.Parameters.Add(new SqlParameter("@StartDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionDayWiseReportDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();

            }
            return dt;
        }
        #endregion

        #region ----- Production ShiftWise Report Details ----
        internal static DataTable GetProductionShiftWiseReportDetails(string plant, string machine, string date, string empid, string shift)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[SP_HourWiseProductionReport_CUMI]", con);
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", empid));
                cmd.Parameters.Add(new SqlParameter("@PlantID", plant));
                cmd.Parameters.Add(new SqlParameter("@MachineID", machine));
                cmd.Parameters.Add(new SqlParameter("@ShiftName", shift));
                cmd.Parameters.Add(new SqlParameter("@StartDate", Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.Add(new SqlParameter("@EndDate", Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionShiftWiseReportDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();

            }
            return dt;
        }
        #endregion
        internal static List<ListItem> GetCurrentShiftDetails()
        {
            List<ListItem> list = new List<ListItem>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("[s_GetCurrentShiftTime]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["StartTime"]))
                        {
                            list.Add(new ListItem() { Text = "ShiftStart", Value = DateTime.Parse(sdr["StartTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss") });
                        }
                        if (!Convert.IsDBNull(sdr["EndTime"]))
                        {
                            list.Add(new ListItem() { Text = "ShiftEnd", Value = DateTime.Parse(sdr["EndTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss") });
                        }
                        list.Add(new ListItem() { Text = "ShiftName", Value = sdr["ShiftName"].ToString() });
                    }
                }
                else
                {
                    list = null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetCurrentShiftDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static void BindRoleFromEmployeeInfo(DropDownList ddlEmployeeRole)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select distinct EmployeeRole from employeeinformation", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        string role = sdr["EmployeeRole"].ToString();
                        if (ddlEmployeeRole.Items.FindByValue(role) == null)
                        {
                            ddlEmployeeRole.Items.Add(new ListItem(role, role));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindRoleFromEmployeeInfo: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }
        #region ---- Loss Report -----
        internal static List<LossReportEntity> getLossReportDetails(string fromDate, string toDate, string plant, string machine, string downCategory, string downCode, out string mttr, out string mtbf)
        {
            mttr = ""; mtbf = "";
            List<LossReportEntity> list = new List<LossReportEntity>();
            LossReportEntity data = null;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("[S_Get_LossReportDetails_Cumi]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime((VDGDataBaseAccess.GetLogicalDayStart(fromDate))).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime((VDGDataBaseAccess.GetLogicalDayEnd(toDate))).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@PlantID", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                cmd.Parameters.AddWithValue("@MachineID", machine.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machine);
                cmd.Parameters.AddWithValue("@LossType", downCategory.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : downCategory);
                cmd.Parameters.AddWithValue("@SubLoss", downCode.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : downCode);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new LossReportEntity();
                        //data.Plant = sdr["Plant"].ToString();
                        //data.DownCategory = sdr["Plant"].ToString();
                        //data.DownCode = sdr["Plant"].ToString();
                        data.Machine = sdr["MachineID"].ToString();
                        data.MachineInterfaceID = sdr["MachineInterfaceID"].ToString();
                        data.StartDate = sdr["StartDate"].ToString();
                        data.EndDate = sdr["EndDate"].ToString();
                        data.StartTime = sdr["StartTime"].ToString();
                        data.EndTime = sdr["EndTime"].ToString();
                        data.TotalTime = sdr["TotalHoursInMin"].ToString();
                        data.PDT = sdr["PDT"].ToString();
                        data.LossType = sdr["LossType"].ToString();
                        data.SubLoss = sdr["SubLoss"].ToString();
                        data.LossID = sdr["LossID"].ToString();
                        data.OperatorID = sdr["OprID"].ToString();
                        //data.OperatorIDWithName = data.OperatorID + " [" + sdr["Plant"].ToString() + "]";
                        data.MntcEmpID = sdr["MaintInchargeID"].ToString();
                        //data.MntcEmpIDWithName = data.MntcEmpID + " [" + sdr["Plant"].ToString() + "]";
                        //data.Machine = sdr["Plant"].ToString();
                        //data.Remarks = sdr["Remarks"].ToString();
                        //data.MTTR = sdr["Plant"].ToString();
                        //data.MTBF = sdr["Plant"].ToString();
                        data.WhyWhyDocFileName = sdr["WhyWhy_Document"].ToString();
                        data.WhyWhyDocID = sdr["WhyWhy_DocumentID"].ToString();
                        //if (sdr["File1"].ToString() == "")
                        //{
                        //    data.WhyWhyDocFileInBase64 = "";
                        //}
                        //else
                        //{
                        //    byte[] bytes = (byte[])sdr["File1"];
                        //    data.WhyWhyDocFileInBase64 = Convert.ToBase64String(bytes);
                        //}
                        data.OtherDocFileName = sdr["Other_Document"].ToString();
                        data.OtherDocID = sdr["Other_DocumentID"].ToString();
                        //if (sdr["File1"].ToString() == "")
                        //{
                        //    data.OtherDocFileInBase64 = "";
                        //}
                        //else
                        //{
                        //    byte[] bytes = (byte[])sdr["File1"];
                        //    data.OtherDocFileInBase64 = Convert.ToBase64String(bytes);
                        //}
                        data.Remarks = sdr["Remarks"].ToString();
                        list.Add(data);
                    }
                    sdr.NextResult();
                    while (sdr.Read())
                    {
                        mttr = sdr["MTTR"].ToString();
                        mtbf = sdr["MTBF"].ToString();
                    }
                }
                else
                {
                    list = null;
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

        internal static int saveLossReportDetails(LossReportEntity data)
        {
            int result = 0;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand(@"if not exists(select * from LossReportDetails_Cumi where Mc=@Mc and StartTime=@StartTime)
begin
	insert into LossReportDetails_Cumi(Mc,StartTime,LossID,MaintIncharge,WhyWhy_Document,Other_Document,WhyWhy_DocumentID,Other_DocumentID,Remarks)
	values(@Mc,@StartTime,@LossID,@MaintIncharge,@WhyWhy_Document,@Other_Document,@WhyWhy_DocumentID,@Other_DocumentID,@Remarks)
end
else
begin
	update LossReportDetails_Cumi set LossID=@LossID,MaintIncharge=@MaintIncharge,WhyWhy_Document=@WhyWhy_Document,Other_Document=@Other_Document,
	WhyWhy_DocumentID=@WhyWhy_DocumentID,Other_DocumentID=@Other_DocumentID,Remarks=@Remarks  where Mc=@Mc and StartTime=@StartTime
end", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Mc", data.MachineInterfaceID);
                DateTime startDateTime = Util.GetDateTime(data.StartDate);
                //var splitTime = data.StartTime.Split(':');
                //startDateTime.AddHours(HelperClassGeneric.getDoubleValueFromString(splitTime[0]));
                //startDateTime.AddMinutes(HelperClassGeneric.getDoubleValueFromString(splitTime[1]));
                //startDateTime.AddSeconds(HelperClassGeneric.getDoubleValueFromString(splitTime[2]));
                // cmd.Parameters.AddWithValue("@StartTime", startDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@StartTime", startDateTime.ToString("yyyy-MM-dd") + " " + data.StartTime);
                cmd.Parameters.AddWithValue("@LossID", data.LossID);
                cmd.Parameters.AddWithValue("@MaintIncharge", data.MntcEmpID);
                cmd.Parameters.AddWithValue("@WhyWhy_Document", data.WhyWhyDocFileName);
                cmd.Parameters.AddWithValue("@Other_Document", data.OtherDocFileName);
                cmd.Parameters.AddWithValue("@WhyWhy_DocumentID", data.WhyWhyDocID);
                cmd.Parameters.AddWithValue("@Other_DocumentID", data.OtherDocID);
                cmd.Parameters.AddWithValue("@Remarks", data.Remarks);
                sdr = cmd.ExecuteReader();
                result++;
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
            return result;
        }
        #endregion

        #region -------------POScreenCumi------------
        internal static List<CumiPOScreen> getPOScreenData(string fromDate, string ToDate, string ComponentID, string ProductionOrder)
        {
            List<CumiPOScreen> list = new List<CumiPOScreen>();
            CumiPOScreen data = null;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from ProductionOrder_CUMI where UpdatedTS>=@FromDate and UpdatedTS<=@ToDate and 
(ItemCode like  '%'+@ItemCode+'%' or ISNULL(@ItemCode,'')='') 
and (ProductionOrder like  '%'+@ProductionOrder+'%' or ISNULL(@ProductionOrder,'')='')  order by UpdatedTS", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ItemCode", ComponentID);
                cmd.Parameters.AddWithValue("@ProductionOrder", ProductionOrder);
                //cmd.Parameters.AddWithValue("@FromDate", fromDate );
                //cmd.Parameters.AddWithValue("@ToDate", ToDate);
                //cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(ToDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new CumiPOScreen();
                        data.AutoID = sdr["AutoID"].ToString();
                        data.ItemCode = sdr["ItemCode"].ToString();
                        data.OperationStage = sdr["OperationStage"].ToString();
                        data.POQty = sdr["POQty"].ToString();
                        data.ProductionOrder = sdr["ProductionOrder"].ToString();
                        data.UpdatedTS = sdr["UpdatedTS"].ToString();
                        data.MouldWeight = sdr["MouldWeight"].ToString();
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
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static DataTable GetPODetails()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dtPODetails = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select top 1 * from ProductionOrder_CUMI order by UpdatedTS desc", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                dtPODetails.Load(rdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPODetails:" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return dtPODetails;
        }
        internal static string GetPOCount()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string POCount = "";
            try
            {
                cmd = new SqlCommand(@"select count(distinct ProductionOrder) as POCount from ProductionOrder_CUMI", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        POCount = rdr["POCount"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPOCount:" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();

            }
            return POCount;
        }

        #endregion

        #region--------------SPCTarget Master Cumi----------
        internal static DataTable GetTargetDetails_Cumi(string year)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from SPCTargetMaster_CUMI where DATEPART(YEAR,TargetDate)=@year", conn);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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


        internal static int SaveSPCTargetMasterDetails(DateTime targetDate, string targetValue)
        {
            int result = 0;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from SPCTargetMaster_CUMI where TargetDate=@TargetDate)
begin
	insert into SPCTargetMaster_CUMI(TargetDate,SPCTargetValue)
	values(@TargetDate,@SPCTargetValue)
end
else
begin
	update SPCTargetMaster_CUMI set SPCTargetValue=@SPCTargetValue where  TargetDate=@TargetDate
end
", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@TargetDate", targetDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SPCTargetValue", targetValue);
                sdr = cmd.ExecuteReader();
                result++;
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
            return result;
        }
        #endregion

        #region ---Hydraulic OilTemp Report---
        internal static DataTable GetHydraulicTempPressureReportDetails(string plant, string machine, string date, string shift)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetHydraulicOilAndTempReport_Cumi]", conn);                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@MachineID", machine.Replace("'",""));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region  -- Cooling Tower Report------
        internal static DataTable GetCoolingTowerReportDetails(DateTime fromDate, DateTime ToDate, string shift)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetCoolingTowerReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", ToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift);
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region ---Process Parameter Report--
        internal static List<ProcessParameterEntity> GetProcessParameterReport(string machineID, string shiftName, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<ProcessParameterEntity> list = new List<ProcessParameterEntity>();
            int Slno = 1;
            try
            {
                cmd = new SqlCommand(@"[S_GetProcessParameterReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ShiftName", shiftName);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    ProcessParameterEntity data = new ProcessParameterEntity();
                    data.Slno = Slno++;
                    data.StartDate = Util.GetDateTime(rdr["StartDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    data.EndDate = Util.GetDateTime(rdr["EndDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    data.Product = rdr["ItemCode"].ToString();
                    data.PONumber = rdr["PONumber"].ToString();
                    data.EmployeeID = rdr["EmployeeID"].ToString();
                    if(!string.IsNullOrEmpty(rdr["MachiningTime"].ToString()))
                    {
                        data.MachiningTime = Convert.ToDouble(rdr["MachiningTime"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["HydraulicPressure_Top"].ToString()))
                    {
                        data.HydraulicPressure_Top = Convert.ToDouble(rdr["HydraulicPressure_Top"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["HydraulicPressure_Bottom"].ToString()))
                    {
                        data.HydraulicPressure_Bottom = Convert.ToDouble(rdr["HydraulicPressure_Bottom"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["HydraulicTemp"].ToString()))
                    {
                        data.HydraulicTemp = Convert.ToDouble(rdr["HydraulicTemp"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["TopRamStroke"].ToString()))
                    {
                        data.TopRamStroke = Convert.ToDouble(rdr["TopRamStroke"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["BottomRamStroke"].ToString()))
                    {
                        data.BottomRamStroke = Convert.ToDouble(rdr["BottomRamStroke"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["LoadUnload"].ToString()))
                    {
                        data.LoadUnload = Convert.ToDouble(rdr["LoadUnload"].ToString());
                    }
                    list.Add(data);
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
            return list;
        }
        #endregion

        #region -- OEE Report ---
        internal static DataTable GetOEEReportDetails(DateTime fromDate, DateTime toDate, string plant, string machine, string shift)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetOEEReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartDate",fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machine.Replace("'",""));
                cmd.Parameters.AddWithValue("@ShiftName", shift.Replace("'", ""));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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
        internal static DataTable GetDownList()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetOEEReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@Param", "DownCodeList");
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region ---MO Status---
        internal static DataTable GetMotorStatusReportDetails(string date, string plant, string machine, string shift)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetMotorStatusReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(date).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machine.Replace("'", ""));
                cmd.Parameters.AddWithValue("@ShiftName", shift.Replace("'", ""));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region -- PO Status ---
        
        internal static List<POStatusEntity> GetPOStatusReport(string PONumber, string fromdate, string toDate)
        {
            List<POStatusEntity> list = new List<POStatusEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetPOStatusReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                if(!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
                {
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StartDate",fromdate);
                    cmd.Parameters.AddWithValue("@EndDate", toDate);
                }
                cmd.Parameters.AddWithValue("@PONumber", PONumber.Equals("All",StringComparison.OrdinalIgnoreCase)?"":PONumber);
                rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    POStatusEntity data = new POStatusEntity();
                    //data.Date = Util.GetDateTime(rdr["Date"].ToString()).ToString("yyyy-MM-dd");
                    //data.Shift= rdr["ShiftName"].ToString();
                    data.PONumber = rdr["PONumber"].ToString();
                    data.ItemCode = rdr["ItemCode"].ToString();
                    if(!string.IsNullOrEmpty(rdr["POQty"].ToString()))
                    {
                        data.POQty = Convert.ToInt32(rdr["POQty"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["ProducedQty"].ToString()))
                    {
                        data.ProducedQty = Convert.ToInt32(rdr["ProducedQty"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["BalancedQty"].ToString()))
                    {
                        data.BalanceQty = Convert.ToInt32(rdr["BalancedQty"].ToString());
                    }
                    //data.MachineID= rdr["MachineID"].ToString();
                    //data.EmployeeID = rdr["EmployeeID"].ToString();
                    list.Add(data);
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
            return list;
        }
        #endregion

        #region  -- Energy Report---
        internal static DataTable GetEnergyReportDetails(DateTime fromdate, DateTime toDate, string plant, string machine, string comp)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_Get_EnergyyConsumptionReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartDate", fromdate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@ComponentID", comp);
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region ---Quality Report---

        internal static List<QualityRejectionEntity> getQualityReportDetails(DateTime fromDate, DateTime toDate, string plantID, string machine,string Employee, string category, string RejectionCode)
        {
            List<QualityRejectionEntity> list = new List<QualityRejectionEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"SP_QualityRejectionReport_CUMI", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                cmd.Parameters.AddWithValue("@ToDate", toDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Operator", Employee);
                cmd.Parameters.AddWithValue("@RejectionReason", RejectionCode);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    QualityRejectionEntity data = new QualityRejectionEntity();
                    data.Date = Util.GetDateTime(rdr["Date"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    data.PONumber = rdr["PONumber"].ToString();
                    data.ItemCode = rdr["ItemCode"].ToString();
                    data.ItemDesc= rdr["ItemDescription"].ToString();
                    data.MachineId = rdr["machine"].ToString();
                    data.Operator = rdr["Operator"].ToString();
                    data.RejReason = rdr["Rejection_Reason"].ToString(); 
                    data.Document = rdr["Document"].ToString();
                    data.DocumentID = rdr["DocumentID"].ToString();
                    if (!string.IsNullOrEmpty(rdr["Rejection_Qty"].ToString()))
                    {
                        data.RejQty = Convert.ToInt32(rdr["Rejection_Qty"].ToString());
                    }
                    if (!string.IsNullOrEmpty(rdr["TotalWeight"].ToString()))
                    {
                        data.TotalWeight = HelperClassGeneric.getDoubleValueFromString(rdr["TotalWeight"].ToString());
                    }

                    list.Add(data);
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
            return list;
        }
        internal static int saveQualityReportDetails(QualityRejectionEntity data)
        {
            int result = 0;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = "if exists (select * from QualityReport_Cumi where MachineID=@MachineID and [Date]=@Date and PONumber=@PONumber and ItemCode=@ItemCode and ItemDescription = @ItemDescription and Operator = @Operator and RejectionReason = @RejectionReason) begin update QualityReport_Cumi set Document = @Document, DocumentID = @DocumentID where MachineID = @MachineID and[Date] = @Date and PONumber = @PONumber and ItemCode = @ItemCode and ItemDescription = @ItemDescription and Operator = @Operator and RejectionReason = @RejectionReason end else begin insert into QualityReport_Cumi(MachineID, [Date], PONumber, ItemCode, ItemDescription, Operator, RejectionReason, Quantity, DocumentID, Document) values(@MachineID, @Date, @PONumber, @ItemCode, @ItemDescription, @Operator, @RejectionReason, @Quantity, @DocumentID, @Document) end";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(data.Date.ToString()).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                cmd.Parameters.AddWithValue("@MachineID", data.MachineId);
                cmd.Parameters.AddWithValue("@PONumber", data.PONumber);
                cmd.Parameters.AddWithValue("@ItemCode", data.ItemCode);
                cmd.Parameters.AddWithValue("@Operator", data.Operator);
                cmd.Parameters.AddWithValue("@ItemDescription", data.ItemDesc);
                cmd.Parameters.AddWithValue("@Quantity", data.RejQty);
                cmd.Parameters.AddWithValue("@RejectionReason", data.RejReason);
                cmd.Parameters.AddWithValue("@Document", data.Document);
                cmd.Parameters.AddWithValue("@DocumentID", data.DocumentID);
                cmd.Parameters.AddWithValue("@Param", "Save");
                sdr = cmd.ExecuteReader();
                result++;
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
            return result;
        }


        #endregion

        #region Product Parameter Report ---
        internal static DataTable GetProductParameterReportDetails(DateTime fromdate, DateTime toDate, string plant, string machine, string shift)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[S_GetProductParameterReport_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@StartDate", fromdate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machine.Replace("'", ""));
                cmd.Parameters.AddWithValue("@ShiftName", shift.Replace("'", ""));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region  ---Quarterly Compliance---
        internal static DataTable GetQuarterlyComplianceDetails(int year, string plant, string machine)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"[s_Get_PMActivityInfo_Cumi]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@YearNo", year);
                cmd.Parameters.AddWithValue("@machineid", machine);
                cmd.Parameters.AddWithValue("@View", "QuarterlyReport");
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region  --Frequencywise Report--
        internal static DataTable GetFrequencywiseReportDetails(string machine, string year, string frequency)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[s_Get_PMActivityInfo_Cumi]", con);
                cmd.Parameters.Add(new SqlParameter("@YearNo", year));
                cmd.Parameters.Add(new SqlParameter("@machineid", machine));
                cmd.Parameters.Add(new SqlParameter("@frequency", frequency));
                cmd.Parameters.Add(new SqlParameter("@View", "FrequencyWisePMActivityReport"));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionShiftWiseReportDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();

            }
            return dt;
        }

        #endregion

        #region --ShiftPM Activity Report---
        internal static DataTable GetShiftPMActivityReportDetails(DateTime fromdate, DateTime toDate, string plant, string machine)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[s_Get_PMActivityInfo_Cumi]", con);
                cmd.Parameters.Add(new SqlParameter("@Starttime", fromdate));
                cmd.Parameters.Add(new SqlParameter("@Endtime", toDate));
                cmd.Parameters.Add(new SqlParameter("@machineid", machine));
                cmd.Parameters.Add(new SqlParameter("@frequency", "Shift"));
                cmd.Parameters.Add(new SqlParameter("@View", "WebView"));
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetShiftPMActivityReportDetails: " + ex.ToString());
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