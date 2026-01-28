using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.ShantiIron.Model
{
    public class ShantiDataBaseAccess
    {
        #region --------------------- Final Inpection --------------------
        internal static int saveFinalInspcetionDetails(string slno, string compid, string remarks, string status, string operatorid, string chk90)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int Result = 0;
            try
            {
                cmd = new SqlCommand("[dbo].[S_Get_FinalInspectionSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InspectionDate", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss"));
                cmd.Parameters.AddWithValue("@CompID", compid);
                if (status == "save")
                {
                    cmd.Parameters.AddWithValue("@Status", DBNull.Value);
                }
                else
                if (status == "approve")
                {
                    cmd.Parameters.AddWithValue("@Status", "Approved");
                }
                else if (status == "rejected")
                {
                    cmd.Parameters.AddWithValue("@Status", "Rejected");
                }
                else if (status == "rework")
                {
                    cmd.Parameters.AddWithValue("@Status", "Rework");
                }
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@Remarks", remarks);
                cmd.Parameters.AddWithValue("@CheckedBy", operatorid);
                if (chk90 == "nochk")
                {
                    cmd.Parameters.AddWithValue("@OpStatus", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OpStatus", chk90);
                }
                cmd.Parameters.AddWithValue("@Param", "Save");
                cmd.CommandTimeout = 450;
                Result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Result = 0;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }
        internal static DataTable getFIDataForDate(string date, string plant, string cell, out List<FIDataDetails> fiList)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            fiList = new List<FIDataDetails>();
            FIDataDetails data = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_Get_FinalInspectionSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartTime", VDGDataBaseAccess.GetLogicalDayStart(Convert.ToDateTime(date.Trim()).ToString("yyyy-MM-dd")));
                cmd.Parameters.AddWithValue("@StartTime", VDGDataBaseAccess.GetLogicalDayStart(Util.GetDateTime(date.Trim()).ToString("yyyy-MM-dd")));
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupId", cell);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new FIDataDetails();
                        data.SerialNumber = sdr["SerialNumber"].ToString();
                        data.PlantID = sdr["PlantID"].ToString();
                        data.MachineID = sdr["MachineID"].ToString();
                        data.ComponentID = sdr["Componentid"].ToString();
                        data.OperationNumber = sdr["OperationNo"].ToString();
                        data.IssueFound = sdr["IssueFound"].ToString();
                        data.Refer = sdr["Refer"].ToString();
                         fiList.Add(data);
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
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        internal static DataTable getFIDataForSlno(string slno, string plant, string cell, out List<FIDataDetails> fiList)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            fiList = new List<FIDataDetails>();
            FIDataDetails data = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_Get_FinalInspectionSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupId", cell);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new FIDataDetails();
                        data.SerialNumber = sdr["SerialNumber"].ToString();
                        data.PlantID = sdr["PlantID"].ToString();
                        data.MachineID = sdr["MachineID"].ToString();
                        data.ComponentID = sdr["Componentid"].ToString();
                        data.OperationNumber = sdr["OperationNo"].ToString();
                        data.IssueFound = sdr["IssueFound"].ToString();
                        data.Refer = sdr["Refer"].ToString();
                        fiList.Add(data);
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
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        internal static bool isFinalInspectionFileGenerated(string slno, string compid)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            bool isFileGenerated = false;
            try
            {
                cmd = new SqlCommand(@"IF Exists(SELECT * FROM [dbo].[PackingStation_Shanti]
where ComponentId = @CompID and CompSlNo = @Slno and PDIReportName IS NOT NULL)
Begin
Select 'Report Found' as Status
End
Else
Begin
Select 'Report Not Found' as Status
End", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@CompID", compid);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if(sdr["Status"].ToString()== "Report Found")
                        {
                            isFileGenerated = true;
                        }
                        else
                        {
                            isFileGenerated = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFileGenerated = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return isFileGenerated;
        }

        internal static DataTable getSpcOperationData(string operationList,string componentId,string slno)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select Distinct A.SerialNumber ,M.MachineID,SP.Componentid,SP.OperationNo,SP.CharacteristicID,SP.CharacteristicCode,SP.LSL,SP.USL,SP.UOM Unit,A.Value,A.Timestamp,SpecificationMean
FROM SPC_Characteristic SP
inner join machineinformation M on M.machineid=Sp.MachineID
Left join SPCAutodata A on M.machineid=SP.machineid and A.comp=SP.Componentid
and A.opn=SP.operationno and A.Dimension=SP.CharacteristicID
where A.SerialNumber =@slno
and SP.Componentid = @compid
AND Sp.OperationNo in("+ operationList + ") order by Sp.OperationNo,A.Timestamp", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@compid", componentId);
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        #endregion

        #region -------------------------- Andon --------------------------
        internal static List<fontstyling> GetFontstyling()
        {
            List<fontstyling> fontstyling = new List<fontstyling>();
            fontstyling fontstyle = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("Select * from [AndonDefaults] where Parameter='AndonSonaSettings'", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    fontstyle = new fontstyling();
                    while (rdr.Read())
                    {
                        switch (rdr["ValueInText"].ToString())
                        {
                            case "Font Family":
                                fontstyle.fontfamily = rdr["ValueInText2"].ToString();
                                break;
                            case "Font Size":
                                fontstyle.fontsize = Convert.ToInt32(rdr["ValueInText2"].ToString());
                                break;
                            case "Font Style":
                                fontstyle.fontstyle = rdr["ValueInText2"].ToString();
                                break;
                            case "Number of Rows":
                                fontstyle.Rows = Convert.ToInt16(rdr["ValueInText2"].ToString());
                                break;
                            case "Header Font Style":
                                fontstyle.Headerfontsize = Convert.ToInt16(rdr["ValueInText2"].ToString());
                                break;
                            case "Header Color":
                                fontstyle.HeaderColor = rdr["ValueInText2"].ToString();
                                break;
                            case "Row Background Color":
                                fontstyle.RowColor = rdr["ValueInText2"].ToString();
                                break;
                            case "Alternative Row Color":
                                fontstyle.AlternativeRowColor = rdr["ValueInText2"].ToString();
                                break;
                        }
                    }
                    fontstyling.Add(fontstyle);
                }
                else
                {
                    fontstyle = new fontstyling();
                    fontstyle.fontfamily = "Verdana";
                    fontstyle.fontsize = 17;
                    fontstyle.fontstyle = "Normal";
                    fontstyle.Rows = 10;
                    fontstyle.Headerfontsize = 20;
                    fontstyle.HeaderColor = "#2665B2";
                    fontstyle.RowColor = "#CBEFFF";
                    fontstyle.AlternativeRowColor = "#F5F5F5";
                    fontstyling.Add(fontstyle);
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
            return fontstyling;
        }
        internal static string GetShiftstart(out string ShiftEnd)
        {
            ShiftEnd = "";
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
                        ShiftEnd = rdr["EndTime"].ToString();
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
            return Shiftstart;
        }
        internal static List<ShantiAndonEntity> GetShantiAndondata(string ShiftStart, string ShiftEnd, string plant)
        {
            List<ShantiAndonEntity> ShantiAndondataList = new List<ShantiAndonEntity>();
            ShantiAndonEntity ShantiAndondata = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetAndonDetails_Shanti]", conn);
                cmd.Parameters.AddWithValue("@StartTime", ShiftStart);
                cmd.Parameters.AddWithValue("@EndTime", ShiftEnd);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 90;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ShantiAndondata = new ShantiAndonEntity();
                        ShantiAndondata.MachineID = rdr["MachineID"].ToString();
                        ShantiAndondata.MachineDescription = rdr["MachineDescription"].ToString();
                        ShantiAndondata.RunningComponent = rdr["RunningComponent"].ToString();
                        ShantiAndondata.RunningOpn = rdr["RunningOpn"].ToString();
                        ShantiAndondata.RunningSlNo = rdr["RunningSlNo"].ToString();
                        ShantiAndondata.HeatCode = rdr["HeatCode"].ToString();
                        ShantiAndondata.CurrentOperator = rdr["CurrentOperator"].ToString();
                        ShantiAndondata.AvgCycletime = rdr["AvgCycletime"].ToString();
                        // ShantiAndondata.AvgLoadunload = rdr["AvgLoadunload"].ToString();
                        ShantiAndondata.AE = rdr["AvailabilityEfficiency"].ToString();
                        ShantiAndondata.PE = rdr["ProductionEfficiency"].ToString();
                        ShantiAndondata.QE = rdr["QualityEfficiency"].ToString();
                        ShantiAndondata.OEE = rdr["OverallEfficiency"].ToString();
                        ShantiAndondata.RejectCount = rdr["Rejcount"].ToString();
                        ShantiAndondata.Components = rdr["Components"].ToString();
                        ShantiAndondata.UtilisedTime = rdr["UtilisedTime"].ToString();
                        ShantiAndondataList.Add(ShantiAndondata);
                    }
                }
                else
                {
                    ShantiAndondata = new ShantiAndonEntity();
                    ShantiAndondataList.Add(ShantiAndondata);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                // throw;
            }
            finally
            {
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return ShantiAndondataList;
        }
        #endregion

        #region -----Serial Number Dashboard----
        internal static List<string> GetAllGroupIDs(string plantid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> groupidlist = new List<string>();
            try
            {
                string sqlQuery = "select distinct GroupID from PlantMachineGroups where PlantID=@plantId";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@plantId", plantid);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    groupidlist.Add(Convert.ToString(rdr["GroupID"]));
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
            return groupidlist;
        }
        internal static List<string> GetAllGroupIDsForFI(string plantid)        {            SqlConnection sqlConn = ConnectionManager.GetConnection();            List<string> groupidlist = new List<string>();            try            {                string sqlQuery = "select distinct GroupID from PlantMachineGroups where PlantID=@plantId";                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);                cmd.CommandType = System.Data.CommandType.Text;                cmd.Parameters.AddWithValue("@plantId", plantid);                SqlDataReader rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    if (!Convert.ToString(rdr["GroupID"]).Equals("Common", StringComparison.OrdinalIgnoreCase) && !Convert.ToString(rdr["GroupID"]).Equals("Shared", StringComparison.OrdinalIgnoreCase))                    {                        groupidlist.Add(Convert.ToString(rdr["GroupID"]));                    }                }                rdr.Close();            }            catch (Exception ex)            {                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());                throw;            }            finally            {                if (sqlConn != null) sqlConn.Close();            }            return groupidlist;        }
        internal static List<string> GetAllSerialNos()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allSerialNo = new List<string>();
            try
            {
                string sqlQuery = "select distinct compslno from autodata";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 450;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allSerialNo.Add(Convert.ToString(rdr["compslno"]));
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
            return allSerialNo;
        }
        internal static List<SerialNumberDashboardEntity> getSerialNumberDetialsByDate(string input, string plantid, string param,string groupid, out List<SerialNumberDashboardEntity> listSerialNoTimeDetails, out List<string> operationName)
        {
            List<SerialNumberDashboardEntity> listSerialNoOperationDetails = new List<SerialNumberDashboardEntity>();
            SerialNumberDashboardEntity slnoOperationDetails = null;
            listSerialNoTimeDetails = new List<SerialNumberDashboardEntity>();
            SerialNumberDashboardEntity slnoTimeDetails = null;
            List<OperationDetails> operationDetailsList = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            string slno = "";
            string compid = "";
            operationName = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_SerialNumberTraceabilityReport_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plantid == "All" ? "" : plantid);
                cmd.Parameters.AddWithValue("@GroupId", groupid == "All" ? "" : groupid);
                if (param == "Date")
                {
                    DateTime date = DateTime.Now;
                    if (input != "")
                    {
                        //date = Convert.ToDateTime(input);
                        date = Util.GetDateTime(input + " 11:00:00");
                    }
                    string date1 =  VDGDataBaseAccess.GetLogicalDayStart(date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@StartTime", date1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Slno", input);
                }
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        OperationDetails operationDetails = null;
                        if (slno != reader["compslno"].ToString() || compid != reader["ComponentID"].ToString())
                        {
                            slnoOperationDetails = new SerialNumberDashboardEntity();
                            slnoOperationDetails.plantId = reader["PlantID"].ToString();
                            slnoOperationDetails.groupid = reader["GroupId"].ToString();
                            slnoOperationDetails.SerialNumber = reader["compslno"].ToString();
                            slnoOperationDetails.ComponentID = reader["ComponentID"].ToString();
                            operationDetailsList = new List<OperationDetails>();
                            operationDetails = new OperationDetails();
                            operationDetails.OperationName = reader["OperationId"].ToString();
                            operationName.Add(reader["OperationId"].ToString() + "  " + reader["Description"].ToString());
                            operationDetails.Machine = reader["MachineId"].ToString();
                            operationDetails.Operator = reader["Operator"].ToString();
                            string startdt = "", enddt = "";
                            if (!reader["StartTime"].Equals(DBNull.Value))
                            {
                                startdt = convertDateTimeToSpecificFormat(reader["StartTime"].ToString());
                            }
                            if (!reader["Endtime"].Equals(DBNull.Value))
                            {
                                enddt = convertDateTimeToSpecificFormat(reader["Endtime"].ToString());
                            }
                            operationDetails.StartTime = startdt;
                            operationDetails.EndTime = enddt;
                            operationDetails.ComponentID = reader["ComponentID"].ToString();
                            operationDetails.OperationNameWithDescription = reader["Description"].ToString();
                            operationDetailsList.Add(operationDetails);
                            slnoOperationDetails.OperatioList = operationDetailsList;
                            listSerialNoOperationDetails.Add(slnoOperationDetails);
                            slno = reader["compslno"].ToString();
                            compid = reader["ComponentID"].ToString();
                        }
                        else
                        {
                            //slnoOperationDetails = new SerialNumberDashboardEntity();
                            //slnoOperationDetails.plantId = reader["PlantID"].ToString();
                            //slnoOperationDetails.SerialNumber = reader["compslno"].ToString();
                            if (slno == "" && reader["compslno"].ToString() == "" && compid == "" && reader["ComponentID"].ToString() == "")
                            {
                                break;
                            }
                            operationDetails = new OperationDetails();
                            operationDetails.OperationName = reader["OperationId"].ToString();
                            operationName.Add(reader["OperationId"].ToString() + "  " + reader["Description"].ToString());
                            operationDetails.Machine = reader["MachineId"].ToString();
                            operationDetails.Operator = reader["Operator"].ToString();
                            string startdt = "", enddt = "";
                            if (!reader["StartTime"].Equals(DBNull.Value))
                            {
                                startdt = convertDateTimeToSpecificFormat(reader["StartTime"].ToString());
                            }
                            if (!reader["Endtime"].Equals(DBNull.Value))
                            {
                                enddt = convertDateTimeToSpecificFormat(reader["Endtime"].ToString());
                            }
                            operationDetails.StartTime = startdt;
                            operationDetails.EndTime = enddt;
                            operationDetails.ComponentID = reader["ComponentID"].ToString();
                            operationDetails.OperationNameWithDescription = reader["Description"].ToString();
                            operationDetailsList.Add(operationDetails);
                            slnoOperationDetails.OperatioList = operationDetailsList;
                            //listSerialNoOperationDetails.Add(slnoOperationDetails);
                        }

                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        slnoTimeDetails = new SerialNumberDashboardEntity();
                        slnoTimeDetails.plantId = reader["PlantID"].ToString();
                        slnoTimeDetails.groupid = reader["GroupId"].ToString();
                        slnoTimeDetails.SerialNumber = reader["compslno"].ToString();
                        slnoTimeDetails.TotalTime = reader["TotalTime"].ToString();
                        slnoTimeDetails.ElapsedTime = reader["ElappsedTime"].ToString();
                        slnoTimeDetails.RunTime = reader["RunTime"].ToString();
                        slnoTimeDetails.ComponentID = reader["ComponentID"].ToString();
                        listSerialNoTimeDetails.Add(slnoTimeDetails);
                    }
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
            return listSerialNoOperationDetails;
        }

        internal static List<AlarmDetails> getAlarmDetails(string fromdate, string todate, string machine, string operationname, string slno, string componentId)
        {
            List<AlarmDetails> alarmDetailsList = new List<AlarmDetails>();
            AlarmDetails alarmDetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_SPCAndAlarmtracebilityReport_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromdate);
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@MachineId", machine);
                cmd.Parameters.AddWithValue("@opn", operationname);
                cmd.Parameters.AddWithValue("@Comp", componentId);
                cmd.Parameters.AddWithValue("@SlNo", slno);
                cmd.Parameters.AddWithValue("@Param", "Alarm");
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string startdt = "", enddt = "";
                        alarmDetails = new AlarmDetails();
                        alarmDetails.MachineID = reader["MachineID"].ToString();
                        alarmDetails.AlarmNo = reader["AlarmNo"].ToString();
                        alarmDetails.Desciption = reader["AlarmMSG"].ToString();
                        if (!reader["AlarmStartTime"].Equals(DBNull.Value))
                        {
                            startdt = convertDateTimeToSpecificFormat(reader["AlarmStartTime"].ToString());
                        }
                        alarmDetails.AlarmStartTime = startdt;
                        if (!reader["AlarmEndTime"].Equals(DBNull.Value))
                        {
                            enddt = convertDateTimeToSpecificFormat(reader["AlarmEndTime"].ToString());
                        }
                        alarmDetails.AlarmEndTime = enddt;
                        alarmDetailsList.Add(alarmDetails);
                    }
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
            return alarmDetailsList;
        }

        internal static List<MeasurementDetails> getMeasurementDetails(string fromdate, string todate, string machine, string operationname, string slno, string componentId)
        {
            List<MeasurementDetails> measurementDetailsList = new List<MeasurementDetails>();
            MeasurementDetails measurementDetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_SPCAndAlarmtracebilityReport_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromdate);
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@MachineId", machine);
                cmd.Parameters.AddWithValue("@opn", operationname);
                cmd.Parameters.AddWithValue("@Comp", componentId);
                cmd.Parameters.AddWithValue("@SlNo", slno);
                cmd.Parameters.AddWithValue("@Param", "SPC");
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string ts = "";
                        measurementDetails = new MeasurementDetails();
                        if (machine == "Leak Test Machine")
                        {
                            measurementDetails.Result = reader["Result"].ToString();
                            measurementDetails.LeakTestRemarks = reader["LeakTestRemarks"].ToString();
                        }
                        else if(machine== "Laser Marking Machine Phantom" || machine== "Laser Marking Machine Compact x")
                        {
                            measurementDetails.ScannedData = reader["ScannedData"].ToString();
                            measurementDetails.MarkingData = reader["MarkingData"].ToString();
                            measurementDetails.MarkingStatus = reader["MarkingStatus"].ToString();
                            measurementDetails.Status = reader["Status"].ToString();
                        }
                        else
                        {
                           
                            if(machine== "QA Gate")
                            {
                                measurementDetails.ComponentID = reader["Comp"].ToString();
                                measurementDetails.CharacteristicID = reader["Dimension"].ToString();
                                measurementDetails.CharecteristicCode = reader["CharacteristicCode"].ToString();
                                measurementDetails.LSL = reader["LSL"].ToString();
                                measurementDetails.USL = reader["USL"].ToString();
                                measurementDetails.Unit = reader["UOM"].ToString();
                                measurementDetails.Value = reader["Value"].ToString();
                                if (!reader["Timestamp"].Equals(DBNull.Value))
                                {
                                    ts = convertDateTimeToSpecificFormat(reader["Timestamp"].ToString());
                                }
                                measurementDetails.TimeStamp = ts;
                                measurementDetails.SpecificationMean = reader["Specification"].ToString();
                                measurementDetails.Remarks = reader["Remarks"].ToString();
                                if (!string.Equals(reader["DataType"].ToString(), "Numeric", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (measurementDetails.Value == "1")
                                    {
                                        measurementDetails.Value = "OK";
                                    }
                                    else
                                    {
                                        measurementDetails.Value = "NOT OK";
                                    }
                                }
                            }
                            else
                            {
                                measurementDetails.ComponentID = reader["Componentid"].ToString();
                                measurementDetails.CharacteristicID = reader["CharacteristicID"].ToString();
                                measurementDetails.CharecteristicCode = reader["CharacteristicCode"].ToString();
                                measurementDetails.LSL = reader["LSL"].ToString();
                                measurementDetails.USL = reader["USL"].ToString();
                                measurementDetails.Unit = reader["Unit"].ToString();
                                measurementDetails.Value = reader["Value"].ToString();
                                if (!reader["Timestamp"].Equals(DBNull.Value))
                                {
                                    ts = convertDateTimeToSpecificFormat(reader["Timestamp"].ToString());
                                }
                                measurementDetails.TimeStamp = ts;
                                measurementDetails.SpecificationMean = reader["SpecificationMean"].ToString();
                            }
                        }
                        measurementDetailsList.Add(measurementDetails);
                    }
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
            return measurementDetailsList;
        }
        internal static List<MeasurementDetails> getMeasurementDetailsForReport(string fromdate, string todate, string machine, string operationname, string slno, string componentId,out List<MeasurementDetails> leakTestMachineList,out List<MeasurementDetails> laserMachineList)
        {
            List<MeasurementDetails> measurementDetailsList = new List<MeasurementDetails>();
            leakTestMachineList = new List<MeasurementDetails>();
            laserMachineList = new List<MeasurementDetails>();
            MeasurementDetails measurementDetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_SPCAndAlarmtracebilityReport_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromdate);
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@MachineId", machine);
                cmd.Parameters.AddWithValue("@opn", operationname);
                cmd.Parameters.AddWithValue("@Comp", componentId);
                cmd.Parameters.AddWithValue("@SlNo", slno);
                cmd.Parameters.AddWithValue("@Param", "SPC");
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string ts = "";
                        measurementDetails = new MeasurementDetails();
                        measurementDetails.ComponentID = reader["Componentid"].ToString();
                        measurementDetails.CharacteristicID = reader["CharacteristicID"].ToString();
                        measurementDetails.CharecteristicCode = reader["CharacteristicCode"].ToString();
                        measurementDetails.LSL = reader["LSL"].ToString();
                        measurementDetails.USL = reader["USL"].ToString();
                        measurementDetails.Unit = reader["Unit"].ToString();
                        measurementDetails.Value = reader["Value"].ToString();
                        if (!reader["Timestamp"].Equals(DBNull.Value))
                        {
                            ts = convertDateTimeToSpecificFormat(reader["Timestamp"].ToString());
                        }
                        measurementDetails.TimeStamp = ts;
                        measurementDetails.SpecificationMean = reader["SpecificationMean"].ToString();
                        measurementDetails.OperationName = reader["OperationNo"].ToString();
                        measurementDetails.Remarks = reader["Remarks"].ToString();
                        if (reader["MachineID"].ToString() == "QA Gate")
                        {
                            if(!string.Equals(reader["DataType"].ToString(), "Numeric", StringComparison.OrdinalIgnoreCase))
                            {
                                if (measurementDetails.Value=="1")
                                {
                                    measurementDetails.Value = "OK";
                                }
                                else
                                {
                                    measurementDetails.Value = "NOT OK";
                                }
                            }
                        }
                        measurementDetailsList.Add(measurementDetails);
                    }
                    //For Leak Test Machine "Laser Marking Machine Phantom" and "Laser Marking Machine Compact x"
                    reader.NextResult();
                    while (reader.Read())
                    {
                        measurementDetails = new MeasurementDetails();
                        measurementDetails.ScannedData = reader["ScannedData"].ToString();
                        measurementDetails.MarkingData = reader["MarkingData"].ToString();
                        measurementDetails.MarkingStatus = reader["MarkingStatus"].ToString();
                        measurementDetails.Status = reader["Status"].ToString();
                        measurementDetails.OperationName = reader["OperationNo"].ToString();
                        laserMachineList.Add(measurementDetails);
                    }

                    //For Leak Test Machine
                    reader.NextResult();
                    while (reader.Read())
                    {

                        measurementDetails = new MeasurementDetails();
                        measurementDetails.Result = reader["Result"].ToString();
                        measurementDetails.LeakTestRemarks = reader["LeakTestRemarks"].ToString();
                        measurementDetails.OperationName = reader["OperationNo"].ToString();
                        leakTestMachineList.Add(measurementDetails);
                    }
                   
                   
                   
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
            return measurementDetailsList;
        }
        internal static DataTable getInspectionDetails(string fromdate, string todate, string machine, string operationname, string slno, string componentId, string plantid, string groupid)
        {
            DataTable dtInspectionDetails = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_FinalInspectionSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromdate);
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@OpnId", operationname);
                cmd.Parameters.AddWithValue("@CompID", componentId);
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@PlantID", plantid);
                cmd.Parameters.AddWithValue("@GroupId", groupid);
                //cmd.Parameters.AddWithValue("@StartTime", "2020-07-03 06:00:00");
                //cmd.Parameters.AddWithValue("@EndTime", "2020-07-04 06:00:00");
                //cmd.Parameters.AddWithValue("@MachineID", "VMC-22");
                //cmd.Parameters.AddWithValue("@OpnId", operationname);
                //cmd.Parameters.AddWithValue("@CompID", "7864554-DX01");
                //cmd.Parameters.AddWithValue("@Slno", "EF1");
                cmd.Parameters.AddWithValue("@Param", "Traceability");
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                dtInspectionDetails.Load(reader);

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
            return dtInspectionDetails;
        }
        internal static DataTable getInspectionDetailsForOpn90(string fromdate, string todate, string machine, string operationname, string slno, string componentId, string plantid, string groupid)
        {
            DataTable dtInspectionDetails = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT top 1 * FROM FinalInspection_Shanti WHERE   CompSlno = @Slno AND ComponentID = @CompId order by InspectionDate desc", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@CompId", componentId);
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                dtInspectionDetails.Load(reader);

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
            return dtInspectionDetails;
        }
        internal static List<InspectionDetails> getMPIInspectionDetails(string fromdate, string todate, string machine, string operationname, string slno, string componentId)
        {
            List<InspectionDetails> mpiList = new List<InspectionDetails>();
            InspectionDetails mpidetails = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_SPCAndAlarmtracebilityReport_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromdate);
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@MachineId", machine);
                cmd.Parameters.AddWithValue("@opn", operationname);
                cmd.Parameters.AddWithValue("@Comp", componentId);
                cmd.Parameters.AddWithValue("@SlNo", slno);
                cmd.Parameters.AddWithValue("@Param", "Rawdata");
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string ts = "";
                        mpidetails = new InspectionDetails();
                        mpidetails.SlNo = reader["PartSerialNumber"].ToString();
                        mpidetails.ComponentID = reader["PartNumber"].ToString();
                        mpidetails.ManualInsResult = reader["ManualInsResult"].ToString();
                        mpidetails.CameraInsResult = reader["CameraInsResult"].ToString();
                        mpidetails.CameraPicLink = reader["CameraPicLink"].ToString();
                        mpidetails.DeMagLevel = reader["DeMagLevel"].ToString();
                        mpidetails.MPIRemark = reader["Remarks"].ToString();
                        mpidetails.VisualInsResult = reader["VisualInsResult"].ToString();
                        mpidetails.OperationName = reader["OperationNo"].ToString();
                        mpiList.Add(mpidetails);
                    }
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
            return mpiList;
        }
        private static string convertDateTimeToSpecificFormat(string datetime)
        {
            string formateddatetime = "";
            try
            {
                formateddatetime = Convert.ToDateTime(datetime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return formateddatetime;
        }
        internal static DataTable getInspectionDetailsForReport(string fromdate, string todate, string machine, string operationname, string slno, string componentId, string plantid, string groupid)
        {
            DataTable dtInspectionDetails = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_Get_FinalInspectionSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromdate);
                cmd.Parameters.AddWithValue("@EndTime", todate);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@OpnId", operationname);
                cmd.Parameters.AddWithValue("@CompID", componentId);
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@PlantID", plantid);
                cmd.Parameters.AddWithValue("@GroupId", groupid);
                //cmd.Parameters.AddWithValue("@StartTime", "2020-07-03 06:00:00");
                //cmd.Parameters.AddWithValue("@EndTime", "2020-07-04 06:00:00");
                //cmd.Parameters.AddWithValue("@MachineID", "VMC-22");
                //cmd.Parameters.AddWithValue("@OpnId", operationname);
                //cmd.Parameters.AddWithValue("@CompID", "7864554-DX01");
                //cmd.Parameters.AddWithValue("@Slno", "EF1");
                cmd.Parameters.AddWithValue("@Param", "TraceabilityReport");
                cmd.CommandTimeout = 450;
                reader = cmd.ExecuteReader();
                dtInspectionDetails.Load(reader);

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
            return dtInspectionDetails;
        }
        #endregion
        #region ---------- Down Time Qualification ------------------
        internal static List<DownTimeData> getDownTimeQualificationDetails(string plantid, string cellid, string machine, string startime, string endtime)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<DownTimeData> list = new List<DownTimeData>();
            DownTimeData data = null;
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = @"Select A.id,M.machineid,D.downid,A.sttime,A.ndtime,A.mc as MachineInterfaceid,A.dcode as Downinterfaceid from Autodata A
inner join machineinformation M on A.mc=M.InterfaceID
Left Outer join PlantMachine P on M.machineid=P.MachineID
Left outer join PlantMachineGroups PMG on P.PlantID=PMG.PlantID and P.MachineID=PMG.MachineID
inner join downcodeinformation D on A.dcode=D.interfaceid
where (M.machineid=@machineid or isnull(@machineid,'')='')
and (P.PlantID=@Plantid or isnull(@Plantid,'')='') and (PMG.GroupID=@GroupID or isnull(@GroupID,'')='')
and (A.sttime>=@starttime and A.ndtime<=@endtime) and A.datatype=2 and D.downid='NO_DATA'
Order by M.machineid,A.sttime";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Plantid", plantid);
                cmd.Parameters.AddWithValue("@GroupID", cellid);
                cmd.Parameters.AddWithValue("@machineid", machine);
                cmd.Parameters.AddWithValue("@starttime", Convert.ToDateTime(startime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@endtime", Convert.ToDateTime(endtime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 40000;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        data = new DownTimeData();
                        data.AutodataID = rdr["id"].ToString();
                        data.MachineID = rdr["machineid"].ToString();
                        data.DownCode = rdr["downid"].ToString();
                        data.StartTime = rdr["sttime"].ToString();
                        data.EndTime = rdr["ndtime"].ToString();
                        data.DownInterfaceID = rdr["Downinterfaceid"].ToString();
                        data.MachineInterfaceID = rdr["MachineInterfaceid"].ToString();
                        list.Add(data);
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
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static void UpdateDownCodedata(DownTimeData data, out bool IsUpdated, out int updatedRow)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            IsUpdated = false;
            List<string> list = new List<string>();
            updatedRow = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(@"update autodata set dcode=@downcode where id=@id", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"id", data.AutodataID);
                cmd.Parameters.AddWithValue(@"downcode", data.DownInterfaceID);
                updatedRow = cmd.ExecuteNonQuery();
                IsUpdated = true;

            }
            catch (Exception ex)
            {
                updatedRow = 0;
                IsUpdated = false;
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static bool UpdateModifiedDownData(string oldDownValue, string newDownValue, string machineID, DateTime sttime, DateTime ndtime, int Datatype, out int updatedRow)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool Result = false;
            updatedRow = 0;
            try
            {
                cmd = new SqlCommand(@"update autodata set dcode=@NewDownCode where dcode=@OldDownCode  and  mc=@MachineID and datatype=@Datatype and sttime >= @sttime and ndtime <= @ndtime", conn);
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
        internal static List<string> GetDownCodeFromAutodata(DateTime startime, DateTime endtime, string machine)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = @"select distinct dcode as result from autodata  where  (mc=@Machineid or isnull(@Machineid,'')='') and datatype= 2 and sttime >= @StartTime and ndtime <= @EndTime and dcode is not null order by dcode";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@StartTime", startime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", endtime.ToString("yyyy-MM-dd HH:mm:ss"));
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr["result"].ToString());
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
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        #endregion

        #region ----Packing Station-----
        internal static int savePackingStationDetails(string slno, string compid, string remarks, string status, string operatorid)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int Result = 0;
            try
            {
                cmd = new SqlCommand("[dbo].[S_GetPackingStationSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InspectionDate", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss"));
                cmd.Parameters.AddWithValue("@CompID", compid);
                if (status == "save")
                {
                    cmd.Parameters.AddWithValue("@Status", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@Remarks", remarks);
                cmd.Parameters.AddWithValue("@CheckedBy", operatorid);
                cmd.Parameters.AddWithValue("@Param", "Save");
                cmd.CommandTimeout = 450;
                Result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Result = 0;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return Result;
        }
        internal static DataTable getPSDataForDate(string date, string plant, string cell, out List<FIDataDetails> fiList)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            fiList = new List<FIDataDetails>();
            FIDataDetails data = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_GetPackingStationSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", VDGDataBaseAccess.GetLogicalDayStart(Convert.ToDateTime(date.Trim()).ToString("yyyy-MM-dd")));
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupId", cell);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new FIDataDetails();
                        data.SerialNumber = sdr["SerialNumber"].ToString();
                        data.PlantID = sdr["PlantID"].ToString();
                        data.MachineID = sdr["MachineID"].ToString();
                        data.ComponentID = sdr["Componentid"].ToString();
                        data.OperationNumber = sdr["OperationNo"].ToString();
                        data.IssueFound = sdr["IssueFound"].ToString();
                        data.Refer = sdr["Refer"].ToString();
                        fiList.Add(data);
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
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        internal static DataTable getPSDataForSlno(string slno, string plant, string cell, out List<FIDataDetails> fiList)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            fiList = new List<FIDataDetails>();
            FIDataDetails data = null;
            try
            {
                cmd = new SqlCommand("[dbo].[S_GetPackingStationSave&View_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Slno", slno);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupId", cell);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new FIDataDetails();
                        data.SerialNumber = sdr["SerialNumber"].ToString();
                        data.PlantID = sdr["PlantID"].ToString();
                        data.MachineID = sdr["MachineID"].ToString();
                        data.ComponentID = sdr["Componentid"].ToString();
                        data.OperationNumber = sdr["OperationNo"].ToString();
                        data.IssueFound = sdr["IssueFound"].ToString();
                        data.Refer = sdr["Refer"].ToString();
                        fiList.Add(data);
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
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        internal static string getScannedSlnoFromTable(string param,out string partnumber)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string slno = "";
            partnumber = "";
            try
            {
                cmd = new SqlCommand("select PartSrlNo,PartNumber,RevNo from Shanti_BarCodeScanDetail where Status=0 and ProcessName=@name", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        slno = sdr["PartSrlNo"].ToString();
                        partnumber = sdr["PartNumber"].ToString() + "-" + sdr["RevNo"].ToString();
                        break;
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
                if (sdr != null) sdr.Close();
            }
            return slno;
        }
        internal static int updateSlnoStatus(string param,string slno)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                cmd = new SqlCommand("update Shanti_BarCodeScanDetail set Status=1 where ProcessName=@name and PartSrlNo=@slno", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@name", param);
                cmd.Parameters.AddWithValue("@slno", slno);
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
        #endregion

        #region ---Coolant TopUp Master------
        static string[] formats = new string[] { "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm", "dd-MM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy HH:mm", "dd-MMM-yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "dd-MM-yyyyTHH:mm:ss", "dd-MM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm", "dd-MM-yyyy HH:mm:ss tt", "dd-MM-yyyy H:mm:ss tt", "d-MM-yyyy HH:mm:ss tt", "d-M-yyyy hh:mm:ss tt", "d-M-yyyy HH:mm:ss tt", "MM-dd-yyyy HH:mm:ss tt", "M-d-yyyy HH:mm:ss tt", "yyyy-MM-dd HH:mm:ss tt", "yyyy-M-d HH:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss", "MM/dd/yyyy h:mm:ss tt", "MM/dd/yyyy h:mm:ss", "MM/dd/yyyy", "dd/MM/yyyy", "MM/d/yyyy hh:mm:ss tt", "MM/d/yyyy hh:mm:ss", "M/d/yyyy hh:mm:ss tt", "M/d/yyyy hh:mm:ss", "M/dd/yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss" , "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:m:ss tt", "M/d/yyyy hh:m:ss tt", "M/d/yyyy h:m:s tt" };
        public static bool show16losses = false;
        public static DateTime GetDateTime(string strDatetime)
        {
            DateTime datetime = DateTime.Now;
            if (!DateTime.TryParseExact(strDatetime.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datetime))
            {
                datetime = DateTime.Now;
                Logger.WriteErrorLog(string.Format("Not able to convert datetime string {0} to DateTime", strDatetime));
            }
            return datetime;
        }
        internal static List<CoolantTopUpData> getCoolantTopUpData(string plant,string cell, string machine,string fromDate,string toDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<CoolantTopUpData> list = new List<CoolantTopUpData>();
            CoolantTopUpData data = null;
            try
            {
                cmd = new SqlCommand(@"select * from CoolantTopUpMaster_Shanti where (PlantId=@plant or isnull(@plant, '') = '') and (CellID=@cell or isnull(@cell, '') = '') and (MachineId=@machine  or isnull(@machine, '') = '') 
and TopUpDateTime >= @fromDate and  TopUpDateTime <= @toDate", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", plant);
                cmd.Parameters.AddWithValue("@cell", cell);
                cmd.Parameters.AddWithValue("@machine", machine);
                cmd.Parameters.AddWithValue("@fromDate", Web_TPMTrakDashboard.Models.Util.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@toDate", Web_TPMTrakDashboard.Models.Util.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if(sdr.HasRows)
                {
                    while(sdr.Read())
                    {
                        data = new CoolantTopUpData();
                        data.PlantID = sdr["PlantId"].ToString();
                        data.CellID = sdr["CellID"].ToString();
                        data.MachineID = sdr["MachineId"].ToString();
                        data.TopUpValue = sdr["TopUp"].ToString();
                        string topDate = sdr["TopUpDateTime"].ToString();
                        if (topDate != "")
                        {
                            topDate =  ShantiDataBaseAccess.GetDateTime(topDate).ToString("dd-MM-yyyy HH:mm:ss");
                        }
                        data.TopUpDatetime = topDate;
                        data.Remarks = sdr["Remarks"].ToString();
                        data.Operator = sdr["Operator"].ToString();
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
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static List<string> getOperatorForPlant(string plant)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct EmployeeID from PlantEmployee where (PlantID=@plant  or isnull(@plant, '')='')", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", plant);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                       
                        list.Add(sdr["EmployeeID"].ToString());
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
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static int insertUpdateCoolantTopUpData(CoolantTopUpData data)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                cmd = new SqlCommand(@"if exists(select * from CoolantTopUpMaster_Shanti where  MachineId=@machine and  TopUpDateTime=@topDatetime)
begin
	update CoolantTopUpMaster_Shanti set Remarks=@remarks where  MachineId=@machine and TopUpDateTime=@topDatetime
end
else
begin
	insert into CoolantTopUpMaster_Shanti(PlantId ,CellID,MachineId, TopUp ,TopUpDateTime ,Remarks ,Operator ,UpdatedTS ) 
	values(@plant,@cell,@machine,@topup,@topDatetime,@remarks,@operator,@updatedts)
end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", data.PlantID);
                cmd.Parameters.AddWithValue("@machine", data.MachineID);
                cmd.Parameters.AddWithValue("@cell", data.CellID);
                cmd.Parameters.AddWithValue("@topDatetime", Util.GetDateTime(data.TopUpDatetime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@topup", data.TopUpValue);
                cmd.Parameters.AddWithValue("@remarks", data.Remarks);
                cmd.Parameters.AddWithValue("@operator", data.Operator);
                cmd.Parameters.AddWithValue("@updatedts", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 450;
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
        internal static List<string> GetAllMachinesForPlant(string PlantId)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            List<string> allMachines = new List<string>();
            SqlCommand cmd = null;
            try
            {
                if (PlantId.Equals("All")) PlantId = string.Empty;
                string sqlQuery = @"select MachineInformation.machineid as MachineID , InterfaceID  from machineinformation inner join plantmachine on MachineInformation.MachineID=PlantMachine.MachineID 
                                    where (PlantID=@plantID or @plantID='') order by MachineInformation.MachineID";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@plantID", PlantId);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    allMachines.Add(Convert.ToString(Convert.ToString(rdr["MachineID"])));
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
                if (conn != null) conn.Close();
            }
            return allMachines;
        }
        internal static List<CoolantTopUpData> getCoolantTopUpReportData(string plant, string cell, string machine, string fromDate, string toDate)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<CoolantTopUpData> list = new List<CoolantTopUpData>();
            CoolantTopUpData data = null;
            try
            {
                cmd = new SqlCommand(@"S_GetCoolantTopUpDetails_Shanti", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@CellID", cell);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@FromDate", ShantiDataBaseAccess.GetDateTime(fromDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", ShantiDataBaseAccess.GetDateTime(toDate).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new CoolantTopUpData();
                        data.PlantID = sdr["PlantID"].ToString();
                        data.CellID = sdr["CellID"].ToString();
                        data.MachineID = sdr["MachineID"].ToString();
                        data.TopUpValue = sdr["MCLevelTopUp"].ToString();
                        data.TotalCell = sdr["CellLevelTopUp"].ToString();
                        data.TotalPlant = sdr["PlantLevelTopUp"].ToString();
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
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        #endregion

        #region ---- Quality Gate -----
        internal static QualityGateSlnoStatusData getQualityGateSlnoStatus(string slno,string partnumber)
        {
            QualityGateSlnoStatusData data = new QualityGateSlnoStatusData();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select processstarttime,processendtime,datediff(second,processstarttime,isnull(processendtime,getdate())) as Actualtime,
Case when isnull(processendtime,'1900-01-01')='1900-01-01' then 'Running' else 'Completed' end as ProcessStatus,QualityGateStatus,SendToCMM,CMMFileName   
from QualityGateInfo_Shanti
where component=@partnum and serialno=@slno", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.Parameters.AddWithValue("@partnum", partnumber);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data.ProcessStatus = sdr["ProcessStatus"].ToString();
                        data.ProcessStartTime = sdr["processstarttime"].ToString();
                        data.ProcessCount = Convert.ToInt32(sdr["Actualtime"].ToString());
                        data.QualityGateStatus = sdr["QualityGateStatus"].ToString();
                        data.SendToCMMStatus = sdr["SendToCMM"].ToString();
                        data.CMMFileName = sdr["CMMFileName"].ToString();
                    }
                }
                else
                {
                    data.ProcessStatus = "Pending";
                    data.ProcessStartTime = "";
                    data.ProcessCount =0;
                    data.QualityGateStatus = "";
                    data.SendToCMMStatus = "";
                    data.CMMFileName = "";
                }
            }
            catch (Exception ex)
            {
                data.ProcessStatus = "Error";
                //data.ProcessStartTime = "";
                //data.ProcessCount = 0;
                //data.QualityGateStatus = "";
                //data.SendToCMMStatus = "";
                //data.CMMFileName = "";
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return data;
        }
        internal static List<MeasurementDetails> getSerialNumberOperationDetails(string slno,string partnumber)
        {
            List<MeasurementDetails> list = new List<MeasurementDetails>();
            MeasurementDetails data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select SP.CharacteristicID,SP.CharacteristicCode,SP.LSL,SP.USL,A.Value,A.Timestamp,
case when A.Value>= ISNULL(SP.LSL,0) and A.Value<= ISNULL(SP.USL,0) then 'Ok' else 'NotOk' End as DimensionStatus,
SP.UOM,P.PlantID,A.SerialNumber,A.Comp from SPCAutodata A
inner join machineinformation M on A.Mc=M.InterfaceID
inner join PlantMachine P on M.machineid=P.MachineID
inner join SPC_Characteristic SP on A.Comp=SP.ComponentID and A.Opn=SP.OperationNo and A.Dimension=SP.CharacteristicID
where A.SerialNumber=@slno and A.opn='60' and A.Comp=@partnum and M.machineid=SP.MachineID
order by SP.CharacteristicID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.Parameters.AddWithValue("@partnum", partnumber);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new MeasurementDetails();
                        data.CharacteristicID = sdr["CharacteristicID"].ToString();
                        data.CharecteristicCode = sdr["CharacteristicCode"].ToString();
                        data.LSL = sdr["LSL"].ToString();
                        data.USL = sdr["USL"].ToString();
                        data.Value = sdr["Value"].ToString();
                        data.TimeStamp = sdr["Timestamp"].ToString();
                        data.Status = sdr["DimensionStatus"].ToString();
                        data.Unit = sdr["UOM"].ToString();
                        data.PlantID = sdr["PlantID"].ToString();
                        data.SerialNumber = sdr["SerialNumber"].ToString();
                        data.ComponentID = sdr["Comp"].ToString();
                        if (data.Status != "Ok")
                        {
                            data.BackColor = "redBackColor";
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
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static List<MeasurementDetails> getSlnoManualInpsectionDetails(string slno,string partnumber)
        {
            List<MeasurementDetails> list = new List<MeasurementDetails>();
            MeasurementDetails data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_ViewQAGateDimensionalData_Shanti]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.Parameters.AddWithValue("@partnum", partnumber);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new MeasurementDetails();
                        data.CharacteristicID = sdr["CharacteristicID"].ToString();
                        data.CharecteristicCode = sdr["CharacteristicCode"].ToString();
                        data.LSL = sdr["LSL"].ToString();
                        data.USL = sdr["USL"].ToString();
                        data.Value = sdr["ActualValue"].ToString();
                        data.TimeStamp = sdr["UpdatedTimestamp"].ToString();
                        data.Unit = sdr["UOM"].ToString();
                        data.PlantID = sdr["PlantID"].ToString();
                        data.DataType = sdr["Datatype"].ToString();
                        data.UpdatedBy = sdr["UpdatedBy"].ToString();
                        data.SerialNumber = sdr["SerialNumber"].ToString();
                        data.ComponentID = sdr["Comp"].ToString();
                        data.MachineID = sdr["Mc"].ToString();
                        data.OperationName = sdr["opn"].ToString();
                        if (sdr["Flag"].ToString() == "Red")
                        {
                            data.BackColor = "redBackColor";
                        }
                        else
                        {
                            data.BackColor = "";
                        }
                        data.SpecificationMean = sdr["Specification"].ToString();
                        data.Result = sdr["Remarks"].ToString();
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
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static string saveManualInpectionDetails(string slno,string componentID,string characteristicID,string value,string userid,string machineid,string operation,string charcode,string lsl,string usl,string unit,string mean,string remarks,string datatype)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"declare @opr nvarchar(50)=''
select @opr=interfaceid from employeeinformation where Employeeid=@updatedby
if exists(select * from SPCAutodata where Mc=@machine and Comp=@partnumber and Opn=@operation and Dimension=@characteristicID and SerialNumber=@slno)
begin 
	update SPCAutodata set Value=@value,Timestamp=@updatedTS,BatchTS=@updatedTS,Remarks=@remarks,Opr=@opr
	where Mc=@machine and Comp=@partnumber and Opn=@operation and Dimension=@characteristicID and SerialNumber=@slno
end
else
begin
	insert into SPCAutodata(Mc,Comp,Opn,Opr,Dimension,Value,Timestamp,BatchTS,SerialNumber,Remarks,CharacteristicCode,LSL,USL,UOM,Specification,DataType)
	values(@machine,@partnumber,@operation,@opr,@characteristicID,@value,@updatedTS,@updatedTS,@slno,@remarks,@charcode,@lsl,@usl,@unit,@mean,@datatype)
end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machine", machineid);
                cmd.Parameters.AddWithValue("@partnumber", componentID);
                cmd.Parameters.AddWithValue("@updatedby", userid);
                cmd.Parameters.AddWithValue("@characteristicID", characteristicID);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@updatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.Parameters.AddWithValue("@operation", operation);
                cmd.Parameters.AddWithValue("@charcode", charcode);
                cmd.Parameters.AddWithValue("@lsl", lsl);
                cmd.Parameters.AddWithValue("@usl", usl);
                cmd.Parameters.AddWithValue("@unit", unit);
                cmd.Parameters.AddWithValue("@mean", mean);
                cmd.Parameters.AddWithValue("@remarks", remarks);
                cmd.Parameters.AddWithValue("@datatype", datatype);
                int res= cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static string saveQualityGateProcessStartEndDetails(string slno, string partnumber, string param,string userid,string processstarttime)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string insertedDatetime = "";
            try
            {
                if (param == "insert")
                {
                    cmd = new SqlCommand(@"
declare @opr nvarchar(50)=''
select @opr=interfaceid from employeeinformation where Employeeid=@updatedby
                    if not exists(select * from QualityGateInfo_Shanti where Component=@partnum and SerialNo=@slno)
                            begin
                            insert into QualityGateInfo_Shanti(Component,SerialNo,ProcessStarttime,UpdatedBy,UpdatedTS)
                            values(@partnum,@slno,@starttime,@opr,@updatedts)
                            select 'inserted' as result
                            end
                            else
                            begin
                             select 'exists' as result
                            end", conn);
                    insertedDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.AddWithValue("@starttime", insertedDatetime);
                }
                else
                {
                    cmd = new SqlCommand(@"update QualityGateInfo_Shanti set ProcessEndtime=@endtime where Component= @partnum and SerialNo=@slno and ProcessStarttime=@starttime
                         select 'updated' as result", conn);
                    cmd.Parameters.AddWithValue("@starttime",  ShantiDataBaseAccess.GetDateTime(processstarttime).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@endtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    result = "updated";
                }
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@partnum", partnumber);
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.Parameters.AddWithValue("@updatedby", userid);
                cmd.Parameters.AddWithValue("@updatedts", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                if(sdr.HasRows)
                {
                    while(sdr.Read())
                    {
                        result = sdr["result"].ToString();
                        if (result == "inserted")
                        {
                            result = insertedDatetime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "error";
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static string saveQualityGateProcessStatusDetails(string slno, string partnum, string starttime, string processStatus)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                if (processStatus == "SendToCMM")
                {
                    cmd = new SqlCommand(@"update QualityGateInfo_Shanti set QualityGateStatus=@status,SendToCMM=0 where Component= @partnum and SerialNo=@slno and ProcessStarttime=@starttime and SendToCMM is null", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"update QualityGateInfo_Shanti set QualityGateStatus=@status where Component= @partnum and SerialNo=@slno and ProcessStarttime=@starttime", conn);
                }
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@status", processStatus);
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.Parameters.AddWithValue("@partnum", partnum);
                cmd.Parameters.AddWithValue("@starttime", GetDateTime(starttime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@sendtocmm", DBNull.Value);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static bool isPartNumberAndSlnoExistsInSPCAutoTbl(string slno, string partnum)
        {
            bool result = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                    cmd = new SqlCommand(@"select * from SPCAutodata where Comp=@partnum and SerialNumber=@slno", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@slno", slno);
                cmd.Parameters.AddWithValue("@partnum", partnum);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        #endregion

        #region ------MPI Gate -------
        internal static string insertMPIGateSlnoDetails(string PartNumber, string RevNumber, string PartName, string SupplierCode, string SerialNumber, string HeatCode, string UpdatedBy,string UpdatedTS,string Result, string param)
        {
            string result = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                string query = "";
                if (param == "insert")
                {
                    result = UpdatedTS = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    query = @"insert into MPIGateInfo_Shanti(PartNumber,RevNo,PartName,SupplierCode,SerialNumber,HeatCode,UpdatedTS,Process,UpdatedBy)
values(@PartNumber,@RevNo,@PartName,@SupplierCode,@SerialNumber,@HeatCode,@UpdatedTS,@Process,@UpdatedBy)";
                }
                else if (param.Equals("updateCompValidation", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"update MPIGateInfo_Shanti set CompValidationResult=@result where  PartNumber=@PartNumber and RevNo=@RevNo and PartName=@PartName and SupplierCode=@SupplierCode
and SerialNumber=@SerialNumber and HeatCode=@HeatCode and Process=@Process and UpdatedTS=@UpdatedTS";
                    //   1 - compSlno allowed,  2 - Compslno not allowed
                }
                else if (param.Equals("updateACK", StringComparison.OrdinalIgnoreCase))
                {
                    query = @"update MPIGateInfo_Shanti set ACKFileGenerate=@result where  PartNumber=@PartNumber and RevNo=@RevNo and PartName=@PartName and SupplierCode=@SupplierCode
and SerialNumber=@SerialNumber and HeatCode=@HeatCode and Process=@Process and UpdatedTS=@UpdatedTS";
                    //   1 - ack file generated ,  2 - ack file not generated
                }
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber);
                cmd.Parameters.AddWithValue("@RevNo", RevNumber);
                cmd.Parameters.AddWithValue("@PartName", PartName);
                cmd.Parameters.AddWithValue("@SupplierCode", SupplierCode);
                cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                cmd.Parameters.AddWithValue("@HeatCode", HeatCode);
                cmd.Parameters.AddWithValue("@UpdatedTS", Util.GetDateTime(UpdatedTS).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.Parameters.AddWithValue("@result", Result);
                cmd.Parameters.AddWithValue("@Process", "MPI");
                int count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = "error";
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        internal static bool isMPIGateSlnoExist(string PartNumber, string RevNumber, string PartName, string SupplierCode, string SerialNumber, string HeatCode)
        {
            bool isExist = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from MPIGateInfo_Shanti where PartNumber=@PartNumber and RevNo=@RevNo and PartName=@PartName and SupplierCode=@SupplierCode
and SerialNumber=@SerialNumber and HeatCode=@HeatCode and Process=@Process and ACKFileGenerate=1", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber);
                cmd.Parameters.AddWithValue("@RevNo", RevNumber);
                cmd.Parameters.AddWithValue("@PartName", PartName);
                cmd.Parameters.AddWithValue("@SupplierCode", SupplierCode);
                cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                cmd.Parameters.AddWithValue("@HeatCode", HeatCode);
                cmd.Parameters.AddWithValue("@Process", "MPI");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return isExist;
        }
        internal static List<MPIGateData> getMPISlnoDetails(string SerialNumber)
        {
            List<MPIGateData> list = new List<MPIGateData>();
             SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select A.comp,A.compslno,A.WorkOrderNumber,A.SupplierCode,C.description from autodata A
inner join componentinformation C on A.comp=C.InterfaceID
where A.compslno=@SerialNumber and A.datatype=1
group by A.comp,A.compslno,A.WorkOrderNumber,A.SupplierCode,C.description", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while(sdr.Read())
                    {
                        MPIGateData data = new MPIGateData();
                        data.Component = sdr["comp"].ToString();
                        data.SerialNumber = sdr["compslno"].ToString();
                        data.HeatCode = sdr["WorkOrderNumber"].ToString();
                        data.SupplierCode = sdr["SupplierCode"].ToString();
                        data.CompDesc = sdr["description"].ToString();
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
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static List<MPIGateData> getAllMPIGeneratedSlnoDetails()
        {
            List<MPIGateData> list = new List<MPIGateData>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from MPIGateInfo_Shanti where ACKFileGenerate=1 and  cast(UpdatedTS as date)=cast(GETDATE() as date) order by UpdatedTS desc", conn);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MPIGateData data = new MPIGateData();
                        data.Component = sdr["PartNumber"].ToString();
                        data.RevisionNumber = sdr["RevNo"].ToString();
                        data.SerialNumber = sdr["SerialNumber"].ToString();
                        data.HeatCode = sdr["HeatCode"].ToString();
                        data.SupplierCode = sdr["SupplierCode"].ToString();
                        data.CompDesc = sdr["PartName"].ToString();
                        data.UpdatedBy = sdr["UpdatedBy"].ToString();
                        data.UpdatedTS = sdr["UpdatedTS"].ToString();
                        data.ACKValidation = sdr["ACKFileGenerate"].ToString();
                        data.CompValidation = sdr["CompValidationResult"].ToString();
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
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static DataTable getMPIMachineDetails()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from machineinformation where machineid='MPI' and Process='MPI'", conn);
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
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        public static string GetStatus28Type(string dataString)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand("s_GetProcessDataString", conn);
            string strTemp = string.Empty;
            cmd.CommandTimeout = 60;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@datastring", SqlDbType.NVarChar).Value = dataString;
            cmd.Parameters.Add("@IpAddress", SqlDbType.NVarChar).Value = "127.0.0.1";
            cmd.Parameters.Add("@OutputPara", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@LogicalPortNo", SqlDbType.SmallInt).Value = "33";
            object obj = null;
            try
            {
                obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    strTemp = obj.ToString();
                }
            }
            catch (Exception ex)
            {
               
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    cmd = null;
                    conn = null;
                }
            }

            //1=ok,  2=this not allowerd on this mc(mco valid),3=already exists,6=sl not exists, 8 = out of sequence, 4 -rej, 5=marked for rek, 
            //7 =this not allowerd on this mc, 9= next operation not exists for co
            return strTemp;
        }
      
        #endregion
    }
}