using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.TAFE
{
    public class TafeDataBaseAccess
    {
        internal static List<string> ViewPlantToDisplay()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> lstPlantData = new List<string>();
            try
            {
                cmd = new SqlCommand(@"[s_GetLookups]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
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
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstPlantData;
        }

        internal static List<string> GetAllMachines()
        {
            List<string> allMachines = new List<string>();
            allMachines.Add("All");
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = @"select * from machineinformation";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        allMachines.Add(rdr["machineid"].ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return allMachines;
        }

        internal static List<string> GetLineIDsForPlant(string plantId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;

            string query = "";
            if (plantId.Equals("All") || string.IsNullOrEmpty(plantId))
            {
                query = @"select distinct GroupID from PlantMachineGroups";
            }
            else
            {
                query = @"select distinct GroupID from PlantMachineGroups where PlantID = @PlantID";
            }
            SqlDataReader sdr = null;
            List<string> lineIDList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lineIDList.Add(sdr["GroupID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lineIDList;
        }

        internal static DataTable GetPlanVsActualData(string plantId, string lineId, string date, out DataTable dtPlanVsActualDataCumulative)
        {
            dtPlanVsActualDataCumulative = new DataTable();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable planVsActualDataDaywise = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                if (lineId.Equals("Line All", StringComparison.OrdinalIgnoreCase)) lineId = "";
                SqlCommand cmd = new SqlCommand(@"s_GetTafe_PlanV/sActualReport", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", date);
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@Groupid", lineId);
                cmd.CommandTimeout = 120;
                rdr = cmd.ExecuteReader();
                planVsActualDataDaywise.Load(rdr);
                dtPlanVsActualDataCumulative.Load(rdr);
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
            return planVsActualDataDaywise;
        }

        internal static List<string> GetAllMachinesForPlantAndLine(string plantID, string lineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allMachines = new List<string>();
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = "[s_GetLookups]";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "MachineByCell");
                cmd.Parameters.AddWithValue("@filter", plantID);
                cmd.Parameters.AddWithValue("@GroupId", lineID);
                rdr = cmd.ExecuteReader();
                allMachines.Add("All");
                while (rdr.Read())
                {
                    allMachines.Add(Convert.ToString(rdr["Machineid"]));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return allMachines;
        }

        internal static List<string> GetComponentsForMachine(string MachineId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> componentIdList = new List<string>();
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = "[s_GetLookups]";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Comp");
                cmd.Parameters.AddWithValue("@filter", MachineId);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    componentIdList.Add(Convert.ToString(rdr["Componentid"]));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return componentIdList;
        }

        internal static DataTable GetHoldReportData(DateTime fromDate, DateTime toDate, string lineId, string machineId)
        {
            DataTable dtHoldReportData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[s_GetTafe_HoldReport]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stDateTime", fromDate);
                cmd.Parameters.AddWithValue("@EndDateTime", toDate);
                cmd.Parameters.AddWithValue("@MC", machineId);
                cmd.Parameters.AddWithValue("@GroupId", lineId);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dtHoldReportData.Load(rdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (conn != null) conn.Close();
            }
            return dtHoldReportData;
        }

        internal static DataSet GetOEEAndLosstimeDetails(DateTime fromDate, string machineId)
        {
            DataSet dsOEEAndLosstimeDetails = new DataSet();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataAdapter sqlDataAdapter = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"s_GetTafe_Agg_CategoryWiseOEEAndLossTimeReport", conn);
                sqlCommand.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@MachineID", machineId);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dsOEEAndLosstimeDetails);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataAdapter != null) sqlDataAdapter.Dispose();
                if (conn != null) conn.Close();
            }
            return dsOEEAndLosstimeDetails;
        }

        internal static DataTable GetRejectionReportData(DateTime fromDate, DateTime toDate, string plantID, string lineID, string category)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dtRejection = new DataTable();
            try
            {
                cmd = new SqlCommand("s_GetTafe_MaterialAndProcessRejectionReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stDateTime", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDateTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Plant", plantID);
                cmd.Parameters.AddWithValue("@GroupId", lineID);
                cmd.Parameters.AddWithValue("@Category", category);
                rdr = cmd.ExecuteReader();
                dtRejection.Load(rdr);
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
            return dtRejection;
        }

        internal static DataTable GetBatchWiseGraphDateReport(DateTime fromDate, string plantID, string lineID, string PartID, string category)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dtBatchwiseGraphdata = new DataTable();
            try
            {
                cmd = new SqlCommand("s_GetTafe_BatchwiseReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@PartID", PartID);
                cmd.Parameters.AddWithValue("@Groupid", lineID);
                cmd.Parameters.AddWithValue("@Catagory", category);
                cmd.Parameters.AddWithValue("@param", "Graph");
                rdr = cmd.ExecuteReader();
                dtBatchwiseGraphdata.Load(rdr);
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
            return dtBatchwiseGraphdata;
        }

        internal static DataTable GetBatchWiseDataReport(DateTime fromDate, string plantID, string lineID, string PartID, string category)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dtBatchwisedata = new DataTable();
            try
            {
                cmd = new SqlCommand("s_GetTafe_BatchwiseReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PlantID", plantID);
                cmd.Parameters.AddWithValue("@PartID", PartID);
                cmd.Parameters.AddWithValue("@Groupid", lineID);
                cmd.Parameters.AddWithValue("@Catagory", category);
                cmd.Parameters.AddWithValue("@param", "");
                rdr = cmd.ExecuteReader();
                dtBatchwisedata.Load(rdr);
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
            return dtBatchwisedata;
        }

        internal static string Getdescription(string partID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string partdescription = string.Empty;
            try
            {
                cmd = new SqlCommand("select description from componentinformation where componentid=@compid", conn);
                cmd.Parameters.AddWithValue("@compid", partID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        partdescription = rdr["description"].ToString();
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
            return partdescription;
        }

        #region "TAFE Machine History"
        internal static List<MachineHistory> GetMachineHistoryDatas(DateTime fromDateTime, DateTime toDateTime, string machineId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            List<MachineHistory> machineHistoryData = new List<MachineHistory>();
            SqlDataReader rdr = null;
            string query = @"[s_GetTafe_MachineHistoryViewAndSave]";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stDateTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndDateTime", toDateTime);
                cmd.Parameters.AddWithValue("@MC", machineId);
                cmd.Parameters.AddWithValue("@Param", "View");
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        MachineHistory macHistory = new MachineHistory();
                        macHistory.MachineID = rdr["MachineID"].ToString();
                        macHistory.DownCode = rdr["DownCode"].ToString();
                        macHistory.DownDescription = rdr["Downdescription"].ToString();
                        macHistory.Reason = rdr["Reason"].ToString();
                        macHistory.DownCategory = rdr["DownCategory"].ToString();
                        macHistory.BreakDownStart = Convert.ToDateTime(rdr["BreakDownStart"]).ToString("yyyy-MM-dd hh:mm:ss tt");
                        macHistory.BreakDownEnd = Convert.ToDateTime(rdr["BreakDownEnd"]).ToString("yyyy-MM-dd hh:mm:ss tt");
                        macHistory.ActionProposed = rdr["ActionProposed"].ToString();
                        macHistory.ActionToResolve = rdr["ActionToResolve"].ToString();
                        macHistory.Severity = rdr["Sevierty"].ToString();
                        macHistory.TimeLost = rdr["TimeLost"].ToString();
                        macHistory.ElapsedTime = rdr["ElapsedTime"].ToString();
                        macHistory.KindOfProblem = rdr["KindOfProblem"].ToString();
                        machineHistoryData.Add(macHistory);
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
                if (conn != null) conn.Close();
            }
            return machineHistoryData;
        }

        internal static bool SaveMachineHistoryData(string machineId, string downCode, string kindOfProb, string downCat, string breakDownStartDate, string reason, string resolveAction, string proposedAction, string severity)
        {
            bool saved = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = @"[s_GetTafe_MachineHistoryViewAndSave]";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@stDateTime", "");
                cmd.Parameters.AddWithValue("@EndDateTime", "");
                cmd.Parameters.AddWithValue("@MC", machineId);
                cmd.Parameters.AddWithValue("@DownCode", downCode);
                cmd.Parameters.AddWithValue("@Downcatagory", downCat);
                cmd.Parameters.AddWithValue("@BreakDownStartTime", breakDownStartDate);
                cmd.Parameters.AddWithValue("@Reason", reason);
                cmd.Parameters.AddWithValue("@ActionToResolve", resolveAction);
                cmd.Parameters.AddWithValue("@ActionProposed", proposedAction);
                cmd.Parameters.AddWithValue("@Sevierty", severity);
                cmd.Parameters.AddWithValue("@Param", "Save");
                cmd.Parameters.AddWithValue("@KindOfProblem", kindOfProb);
                cmd.CommandType = CommandType.StoredProcedure;
                int cont = cmd.ExecuteNonQuery();
                if (cont > 0)
                {
                    saved = true;
                }
            }
            catch (Exception ex)
            {
                saved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return saved;
        }
        #endregion

        #region Tafe Line Meter Report
        internal static DataTable GetLinemeterData(string LineID, string starttime, string endtime)
        {

            DataTable ret = new DataTable();
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand(@"[s_GetLineMeterGraph_Web_TAFE]", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StartDate", starttime);
            cmd.Parameters.AddWithValue("@EndDate", endtime);
            cmd.Parameters.AddWithValue("@LineID", LineID);
            SqlDataReader rdr = null;

            try
            {
                rdr = cmd.ExecuteReader();
                ret.Load(rdr);
                rdr.Close();
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
            return ret;
        }
        #endregion

        #region LineMeterReportSave
        internal static bool SaveLineMeterValue(string line, string NoOfManPower, string LoadingHours, DateTime Date)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool StatusMessage = false;
            string Query = @"  if not exists (select * from Tafe_LineMeterSchedule where [Date]=@Date and Line=@Line)
		                            begin
		                            insert into Tafe_LineMeterSchedule([Date],Line,LoadingHours,NoOfManpower)
		                            values(@Date,@Line,@LoadingHours,@NoOfManPower)
		                            end
                               else
		                            begin
		                            update Tafe_LineMeterSchedule set LoadingHours=@LoadingHours,NoOfManpower=@NoOfManPower where [Date]=@Date and Line=@Line
		                            end";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("Line", line);
                cmd.Parameters.AddWithValue("NoOfManPower", NoOfManPower);
                cmd.Parameters.AddWithValue("LoadingHours", LoadingHours);
                cmd.Parameters.AddWithValue("Date", Date.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
                StatusMessage = true;

            }
            catch (Exception ex)
            {
                StatusMessage = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return StatusMessage;
        }
        #endregion


        internal static List<LineMeterEntity> BindLineMeterGrid(string Line, DateTime Date)
        {
            List<LineMeterEntity> ret = new List<LineMeterEntity>();

            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand(@"s_GetTafe_LineMeterSchedule", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Line", Line);
            cmd.Parameters.AddWithValue("@StartDate", Date.ToString("yyyy-MM-dd 13:00:00"));
            SqlDataReader rdr = null;

            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        LineMeterEntity Data = new LineMeterEntity();
                        Data.DStart = Convert.ToDateTime(rdr["DStart"].ToString());
                        Data.Line = rdr["Line"].ToString();
                        Data.Loadinghrs = rdr["Loadinghrs"].ToString();
                        Data.Manpower = rdr["Manpower"].ToString();
                        ret.Add(Data);
                    }
                }
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
            return ret;
        }


        internal static string GellogicalmonthEnd(DateTime fromDate)
        {
            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalMonth( '" + fromDate.ToString("yyyy-MM-dd 13:00:00") + "','end')", Con);

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
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }

        internal static string Gellogicalmonthstart(DateTime fromDate)
        {

            SqlConnection Con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT dbo.f_GetLogicalMonth( '" + fromDate.ToString("yyyy-MM-dd 13:00:00") + "','start')", Con);

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
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(SEDate));
        }

        #region ---- PDI Master Details ----
        internal static List<PDIDetails> GetPDIMasterDetails(string machine)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<PDIDetails> list = new List<PDIDetails>();
            PDIDetails data = null;
            string query = string.Empty;
            query = @"select * from PDIMasterDetails_Tafe where MachineID=@machine";
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
                        data = new PDIDetails();
                        data.MachineID = rdr["MachineID"].ToString();
                        data.PartNo = rdr["PartNo"].ToString();
                        data.PartDesc = rdr["PartDescription"].ToString();
                        data.PartName = rdr["PartName"].ToString();
                        data.PartType = rdr["PartType"].ToString();
                        data.OperationNo = rdr["OperationNo"].ToString();
                        data.Material = rdr["Material"].ToString();
                        data.Version = rdr["Version"].ToString();
                        data.IssuedNo = rdr["IssuedNo"].ToString();
                        data.DocNo = rdr["DocNo"].ToString();
                        data.Type = rdr["Type"].ToString();
                        data.ImageName = rdr["ImageName"].ToString();
                        if (rdr["Image1"].ToString() == "")
                        {
                            data.ImageInBase64 = "";
                        }
                        else
                        {
                            byte[] bytes = (byte[])rdr["Image1"];
                            data.ImageInBase64 = Convert.ToBase64String(bytes);
                        }
                        list.Add(data);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetPDIMasterDetails: " + ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        internal static string SavePDIMasteDetails(PDIDetails data)
        {
            string output = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                string query = @"
IF @Param='Save'
BEGIN

	IF EXISTS(Select * from PDIMasterDetails_Tafe where [MachineID]=@MachineID and PartNo = @PartNo and OperationNo=@OperationNo and [Version]=@Version  and [Type]=@Type)
	BEGIN
		IF @Param1='Edit'
		BEGIN
			UPDATE PDIMasterDetails_Tafe set  PartDescription=@PartDesc, PartName=@PartName, PartType=@PartType, Material=@Material, IssuedNo=@IssuedNo, DocNo=@DocNo, Image1=@Image, ImageName=@ImageName
			where [MachineID]=@MachineID and PartNo = @PartNo and OperationNo=@OperationNo and [Version]=@Version and [Type]=@Type;
                                    
			select 'Updated' as SaveFlag;
		END
		ELSE 
		BEGIN
			select 'Exist' as SaveFlag;
		END
	END
	ELSE
	BEGIN
		Insert into PDIMasterDetails_Tafe(MachineID,PartNo, PartDescription, PartName, PartType, OperationNo, Material, [Version], IssuedNo, DocNo, [Type],Image1,ImageName) 
		Values(@MachineID,@PartNo,@PartDesc ,@PartName,@PartType,@OperationNo,@Material,@Version,@IssuedNo,@DocNo, @Type,@Image, @ImageName) 

		select 'Inserted' as SaveFlag;
	END
END
ELSE IF @Param='Delete'
BEGIN
	Delete from PDIMasterDetails_Tafe where [MachineID]=@MachineID and PartNo = @PartNo and OperationNo=@OperationNo and [Version]=@Version  and [Type]=@Type;
	select 'Deleted' as SaveFlag;
END";
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@MachineID", data.MachineID));
                cmd.Parameters.Add(new SqlParameter("@PartNo", data.PartNo));
                cmd.Parameters.Add(new SqlParameter("@PartDesc", data.PartDesc));
                cmd.Parameters.Add(new SqlParameter("@PartName", data.PartName));
                cmd.Parameters.Add(new SqlParameter("@PartType", data.PartType));
                cmd.Parameters.Add(new SqlParameter("@OperationNo", data.OperationNo));
                cmd.Parameters.Add(new SqlParameter("@Material", data.Material));
                cmd.Parameters.Add(new SqlParameter("@Version", data.Version));
                cmd.Parameters.Add(new SqlParameter("@IssuedNo", data.IssuedNo));
                cmd.Parameters.Add(new SqlParameter("@DocNo", data.DocNo));
                cmd.Parameters.Add(new SqlParameter("@Type", data.Type));
                cmd.Parameters.Add(new SqlParameter("@Image", data.Image));
                cmd.Parameters.Add(new SqlParameter("@ImageName", data.ImageName));
                cmd.Parameters.Add(new SqlParameter("@Param", data.Param));
                cmd.Parameters.Add(new SqlParameter("@Param1", data.NewOrEdit));
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
                Logger.WriteErrorLog("SavePDIMasteDetails: " + ex.Message);
                output = "Error";
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return output;
        }

        internal static List<ListItem> GetComponentMachine(string machineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ListItem> list = new List<ListItem>();
            string query = @"select distinct co.componentid,c.description from componentoperationpricing as co inner join componentinformation as c on c.componentid=co.componentid where co.machineid=@MachineID";
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Value = rdr["componentid"].ToString(), Text = rdr["description"].ToString() });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetComponentMachine:  " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        #endregion

        
    }
}