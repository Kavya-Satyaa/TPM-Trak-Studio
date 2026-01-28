using Elmah;
using GEA_NonMachining.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.Andon_GEA.Model;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.GEA.MachineComponentMaster;

namespace Web_TPMTrakDashboard.GEA.DataBaseAccess
{
    public class GEADatabaseAccess
    {
        #region  ----------------- GEA ANDON -------------------
        public static List<getCurrentShiftTime> CurrentShift()
        {
            bool isInError = false;
            List<getCurrentShiftTime> listtime = new List<getCurrentShiftTime>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader Shiftreader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[s_GetCurrentShiftTime]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                string ss = string.Format("{0:yyyy-MM-dd hh:mm:ss tt}", DateTime.Now);
                cmd.Parameters.AddWithValue("@StartDate", string.Format("{0:yyyy-MM-dd hh:mm:ss tt}", DateTime.Now));
                cmd.Parameters.AddWithValue("@Param", "");
                Shiftreader = cmd.ExecuteReader();
                while (Shiftreader.Read())
                {
                    getCurrentShiftTime currentshiftTime = new getCurrentShiftTime();
                    currentshiftTime.Shiftname = Shiftreader["shiftname"].ToString();
                    currentshiftTime.Starttime = DateTime.Parse(Shiftreader["Starttime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    currentshiftTime.Endtime = DateTime.Parse(Shiftreader["Endtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    currentshiftTime.Shiftid = Shiftreader["shiftid"].ToString();

                    listtime.Add(currentshiftTime);
                }
                if (Shiftreader != null) Shiftreader.Close();
            }
            catch (Exception ex)
            {
                isInError = true;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
            }
            if (isInError)
            {
                if (Shiftreader != null) Shiftreader.Close();
            }
            return listtime;
        }

        internal static DataTable getFabricationNumber()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select  FabricationNo,UpdatedTS from
                (
                select distinct FabricationNo,UpdatedTS from MachineDataAssemblyDescription_GEA
                union
                select distinct FabricationNo,UpdatedTS from MachineDataAssemblySpecification_GEA
				union
				select distinct FabricationNo,UpdatedTS from BlueCardTransaction_GEA where Confirmation=2 and 
                OrderNo in (select distinct ProductionOrder from ProDecanterDetails_Gea)
                )t1  order by  UpdatedTS desc
                ", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        internal static DataTable getSerialNolst(string value)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            string Query = string.Empty;
            if (value.Equals("Bowl", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct SerialNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Bowl' order by UpdatedTS desc";
            }
            else if (value.Equals("Scroll", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct SerialNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Scroll' order by UpdatedTS desc";
            }
            else if (value.Equals("Bearing Hub", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct SerialNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Bearing Hub' order by UpdatedTS desc";
            }
            else if (value.Equals("Main Motor", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct mainmotor as SerialNo,UpdatedTS from [MachineDataAssemblySpecification_GEA] where ParameterID='Serial No' order by UpdatedTS desc";
            }
            else if (value.Equals("Secondary Motor", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct SecondaryMotor as SerialNo,UpdatedTS from [MachineDataAssemblySpecification_GEA] where ParameterID='Serial No' order by UpdatedTS desc";
            }
            else if (value.Equals("Main Gear", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct SerialNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Main Gear' order by UpdatedTS desc";
            }
            else if (value.Equals("Secondary Gear", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct SerialNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Secondary Gear' order by UpdatedTS desc";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;

        }

        internal static DataTable getPartNolst(string value)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            string Query = string.Empty;
            if (value.Equals("Bowl", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct PartNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Bowl' order by UpdatedTS desc";
            }
            else if (value.Equals("Scroll", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct PartNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Scroll' order by UpdatedTS desc";
            }
            else if (value.Equals("Bearing Hub", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct PartNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Bearing Hub' order by UpdatedTS desc";
            }
            else if (value.Equals("Main Gear", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct PartNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Main Gear' order by UpdatedTS desc";
            }
            else if (value.Equals("Secondary Gear", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct PartNo,UpdatedTS from MachineDataAssemblyDescription_GEA where ParameterID='Secondary Gear' order by UpdatedTS desc";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        internal static DataTable GetFabricationNo()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select  FabricationNo,UpdatedTS from
                (
                select distinct FabricationNo,UpdatedTS from MachineDataAssemblyDescription_GEA
                union
                select distinct FabricationNo,UpdatedTS from MachineDataAssemblySpecification_GEA
                )t1  order by  UpdatedTS desc
                ", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        internal static DataTable GetZCLFGMDReportDetails(string FabNo)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("[SP_ZCLFGMDReport_GEA]", con);
                cmd.Parameters.AddWithValue("@FabricationNumber", FabNo);
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        internal static DataTable GetTracebilityReport(string param, string partNo, string serialNo)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("[TraceabilityReportSearch_GEA]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ParameterID", param);
                cmd.Parameters.AddWithValue("@PartNumber", partNo);
                cmd.Parameters.AddWithValue("@SerialNo", serialNo);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        internal static bool checkroleMaintainnce(string EmployeeID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            SqlCommand cmd = null;
            bool present = false;
            try
            {
                cmd = new SqlCommand("select * from employeeinformation where EmployeeRole='MaintainanceEngineer' and Employeeid=@Empid ", con);
                cmd.Parameters.AddWithValue("@Empid", EmployeeID);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    present = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (reader != null) reader.Close();
            }
            return present;
        }

        //internal static string GetLatestIQCNumber(string Type)
        //{
        //    SqlConnection con = ConnectionManager.GetConnection();
        //    SqlDataReader reader = null;
        //    SqlCommand cmd = null;
        //    string present = "";
        //    string Query = "";
        //    switch(Type)
        //    {
        //        case "Quality"
        //            Query = "select Top 1 PlanNoAndRevNo from QualityIncomingMaster_GEA order by PlanNoAndRevNo desc";
        //            break;
        //        case ""
        //            Query = "select Top 1 from QualityIncomingMaster_GEA order by PlanNoAndRevNo desc";
        //            break;

        //    }
        //    try
        //    {
        //        cmd = new SqlCommand("Query ", con);
        //        reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //            present = reader[0].ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());
        //    }
        //    finally
        //    {
        //        if (con != null) con.Close();
        //        if (reader != null) reader.Close();
        //    }
        //    return present;
        //}

        public static SqlDataReader CurrShift()
        {
            START:
            bool isInError = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader Shiftreader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("s_GetCurrentShiftTime", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", string.Format("{0:yyyy-MM-dd hh:mm:ss tt}", DateTime.Now));
                cmd.Parameters.AddWithValue("@Param", "");
                Shiftreader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                isInError = true;
                Logger.WriteErrorLog(ex.ToString());
            }
            if (isInError)
            {
                if (Shiftreader != null) Shiftreader.Close();
                goto START;
            }
            return Shiftreader;
        }

        public static List<string> machineCount(string Plantname)
        {
            START:
            bool isInError = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader reader = null;
            List<string> MID = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                //cmd = new SqlCommand("Select count(MI.machineid) as count from PlantMachine PM inner join MachineInformation MI on PM.machineid = MI.machineid where PM.plantid = '"+ Plantname +"'", DBCon);               
                cmd = new SqlCommand("Select MI.machineid as count from PlantMachine PM inner join MachineInformation MI on PM.machineid = MI.machineid where PM.plantid = '" + Plantname + "' and TPMTrakenabled=1 order by MI.machineid", con);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MID.Add(reader[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                isInError = true;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (reader != null) reader.Close();
                con.Close();
            }
            if (isInError) goto START;
            return MID;
        }

        public static List<string> plantCount()
        {
            START:
            bool isInError = false;
            SqlConnection con = ConnectionManager.GetConnection();
            List<string> plant = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd = new SqlCommand("Select distinct Plantid from PlantInformation", con);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        plant.Add(reader["plantid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                isInError = true;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            if (isInError) goto START;
            return plant;
        }

        public static void updateSettingData(int header, int content, int noofRows, string imagePath, string videoPath, int showImage, int showVideo, string scrollingText, string flipInterval, string showDataDayWeek, string refreshInterval, string datarefreshInterval, out bool result1)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            result1 = true;
            try
            {
                SqlCommand cmd = new SqlCommand("update CockpitDefaults set ValueInInt=@header where Parameter='WebAndon' and ValueInText='FontSize' and ValueInText2='Header' update CockpitDefaults set ValueInInt = @content where Parameter = 'WebAndon' and ValueInText = 'FontSize' and ValueInText2 = 'Content' update CockpitDefaults set ValueInInt=@noofRows where Parameter='WebAndon' and ValueInText='NoOfRows' update CockpitDefaults set ValueInText2=@imagePath where Parameter='WebAndon' and ValueInText='ImagePath' update CockpitDefaults set ValueInText2=@videoPath where Parameter='WebAndon' and ValueInText='VideoPath' update CockpitDefaults set ValueInBool=@showImage where Parameter='WebAndon' and ValueInText='ShowImage' update CockpitDefaults set ValueInBool =@showVideo where Parameter = 'WebAndon' and ValueInText = 'ShowVideo' update CockpitDefaults set ValueInText2=@scrollingText where Parameter='WebAndon' and ValueInText='ScrollingText' update CockpitDefaults set ValueInInt=@flipInterval where Parameter='WebAndon' and ValueInText='FlipInterval' update CockpitDefaults set ValueInText2=@showDataDayWeek where Parameter='WebAndon' and ValueInText='ShowDataBy' update CockpitDefaults set ValueInInt=@refreshInterval where Parameter='WebAndon' and ValueInText='RefreshInterval' update CockpitDefaults set ValueInInt=@datarefreshInterval where Parameter='WebAndon' and ValueInText='DataRefreshInterval'", con);
                cmd.Parameters.AddWithValue("@header", header);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@noofRows", noofRows);
                cmd.Parameters.AddWithValue("@imagePath", imagePath);
                cmd.Parameters.AddWithValue("@videoPath", videoPath);
                cmd.Parameters.AddWithValue("@showImage", showImage);
                cmd.Parameters.AddWithValue("@showVideo", showVideo);
                cmd.Parameters.AddWithValue("@scrollingText", scrollingText);
                if (flipInterval != "")
                {
                    cmd.Parameters.AddWithValue("@flipInterval", Convert.ToInt32(flipInterval));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@flipInterval", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@showDataDayWeek", showDataDayWeek);
                if (refreshInterval != "")
                    cmd.Parameters.AddWithValue("@refreshInterval", Convert.ToInt32(refreshInterval));
                else
                    cmd.Parameters.AddWithValue("@refreshInterval", DBNull.Value);
                if (datarefreshInterval != "")
                    cmd.Parameters.AddWithValue("@datarefreshInterval", Convert.ToInt32(datarefreshInterval));
                else
                    cmd.Parameters.AddWithValue("@datarefreshInterval", DBNull.Value);
                sdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                result1 = false;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }

        }
        public static void saveAndonValueInText2(string valueintext, string valueintext2)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"if exists (select * from CockpitDefaults where Parameter='WebAndon' and ValueInText=@ValueInText)
begin
 update CockpitDefaults set ValueInText2 = @ValueInText2 where Parameter = 'WebAndon' and ValueInText = @ValueInText
end
else
                    begin
                      insert into CockpitDefaults(Parameter, ValueInText, ValueInText2) values('WebAndon', @ValueInText, @ValueInText2)
end ", con);
                cmd.Parameters.AddWithValue("@ValueInText", valueintext);
                cmd.Parameters.AddWithValue("@ValueInText2", valueintext2);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }

        }
        public static void saveAndonValueInIntByText(string valueintext, string valueinint)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"if exists (select * from CockpitDefaults where Parameter='WebAndon' and ValueInText=@ValueInText)
begin
 update CockpitDefaults set ValueInInt = @ValueInInt where Parameter = 'WebAndon' and ValueInText = @ValueInText
end
else
                    begin
                      insert into CockpitDefaults(Parameter, ValueInText, ValueInInt) values('WebAndon', @ValueInText, @ValueInInt)
end ", con);
                cmd.Parameters.AddWithValue("@ValueInText", valueintext);
                cmd.Parameters.AddWithValue("@ValueInInt", valueinint);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }

        }
        internal static void SaveWeekEntry(string WeekNumber, int YearNumber, string MachineID, string UserName)
        {

            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = @"if not exists (select * from MaintainanceInfo where [WeekNo]=@WeekNumber and [YearNo]= @YearNumber and MachineID=@MachineID)
	begin
		insert into MaintainanceInfo(MachineID,[WeekNo],[YearNo], UpdatedBy,UpdatedTS)
		values(@MachineID,@WeekNumber,@YearNumber, @UpdatedBy,@UpdatedTS)
	end
else
	begin
		update MaintainanceInfo set UpdatedBy=@UpdatedBy, UpdatedTS=@UpdatedTS where [YearNo]=@YearNumber and [WeekNo]= @WeekNumber and MachineID=@MachineID
	end";
            try
            {
                cmd = new SqlCommand(Query, Conn);
                cmd.Parameters.AddWithValue("@YearNumber", YearNumber);
                cmd.Parameters.AddWithValue("@UpdatedBy", UserName);
                cmd.Parameters.AddWithValue("@WeekNumber", WeekNumber);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }

        }
        internal static bool CheckForAlreadyLogin(string WeekNumber, int YearNumber, string MachineID)
        {
            bool status = false;
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = @"select * from MaintainanceInfo where  WeekNo=@WeekNumber and Yearno=@YearNumber and MachineID=@MachineID";
            try
            {
                cmd = new SqlCommand(Query, Conn);

                cmd.Parameters.AddWithValue("@WeekNumber", WeekNumber);
                cmd.Parameters.AddWithValue("@YearNumber", YearNumber);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                    status = true;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return status;
        }

        public static string getImageDetials()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string images = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("select ValueInText2 from CockpitDefaults where Parameter='WebAndon' and ValueInText='ImagePath'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        images = sdr["ValueInText2"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return images;
        }

        public static void setPOColumnSettings(List<AndonSettingData> listData, out bool result2)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            result2 = true;
            try
            {
                foreach (AndonSettingData data in listData)
                {
                    SqlCommand cmd = new SqlCommand("update AndonDefaults set ValueInText2=@customName ,ValueInBool=@visibility where Parameter='WebAndon' and ValueInText=@column", con);
                    cmd.Parameters.AddWithValue("@customName", data.CustomColumn);
                    cmd.Parameters.AddWithValue("@visibility", Convert.ToInt32(data.Visibility));
                    cmd.Parameters.AddWithValue("@column", data.Column);
                    sdr = cmd.ExecuteReader();
                    if (sdr != null) sdr.Close();
                }
            }
            catch (Exception ex)
            {
                result2 = false;
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();

            }
        }
        public static int insertImageVideoSortOrderDetails(string name, string order)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"if exists(select * from CockpitDefaults where Parameter='WebAndon' and ValueInText=@param and ValueInText2=@name)
begin
	update CockpitDefaults set ValueInInt=@sortOrder  where Parameter='WebAndon' and ValueInText=@param and ValueInText2=@name
end
else
begin
	insert into CockpitDefaults(Parameter,ValueInText,ValueInText2,ValueInInt) values('WebAndon',@param,@name,@sortOrder)
end", con);
                cmd.Parameters.AddWithValue("@param", "ImageVideoSortOrder");
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@sortOrder", Convert.ToInt32(order));
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();

            }
            return result;
        }
        public static int deleteAndonImageVideoSortOrderDetails(string name)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"delete from CockpitDefaults where Parameter='WebAndon' and ValueInText=@param and ValueInText2=@name", con);
                cmd.Parameters.AddWithValue("@param", "ImageVideoSortOrder");
                cmd.Parameters.AddWithValue("@name", name);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();

            }
            return result;
        }
        public static List<CockpitEntity> getAndonImageVideoSortOrderDetails()
        {
            List<CockpitEntity> list = new List<CockpitEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from CockpitDefaults where Parameter='WebAndon' and ValueInText='ImageVideoSortOrder'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        CockpitEntity data = new CockpitEntity();
                        data.Parameter = sdr["Parameter"].ToString();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        data.ValueInText2 = sdr["ValueInText2"].ToString();
                        data.ValueInInt = string.IsNullOrEmpty(sdr["ValueInInt"].ToString()) ? 0 : Convert.ToInt32(sdr["ValueInInt"].ToString());
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
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        public static DataTable getSettingDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from CockpitDefaults where Parameter='WebAndon'", con);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }

        public static List<string> getPlantID()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<string> list = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from PlantInformation", con);
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
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        public static List<PODetails> gridMachineStatusLoad(string MachineID, string PlantID, string Param)
        {
            List<PODetails> list = new List<PODetails>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand("[dbo].[s_GetANDONHelpCodeDetails_GEA]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-02-01 21:00:00"));
                // cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-06-10 00:00:00.000"));
                //cmd.Parameters.AddWithValue("@DateTime", "2020-02-01 21:00:00");
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.CommandTimeout = 40000;
                SqlDataReader rdr = cmd.ExecuteReader();
                try
                {
                    while (rdr.Read())
                    {
                        PODetails dto = new PODetails();
                        dto.Machine = rdr["MachineID"].ToString();

                        if (!string.IsNullOrEmpty(rdr["UtilisedTime"].ToString()) && Convert.ToInt32(rdr["UtilisedTime"]) > 0)
                        {
                            dto.Plan = rdr["Planned"].ToString();
                            dto.Act = rdr["Components"].ToString();
                            if (!string.IsNullOrEmpty(rdr["OverallEfficiency"].ToString()))
                            {
                                //dto.OEE = Convert.ToDouble(rdr["OverallEfficiency"].ToString()) > 100 ? 100.ToString() : rdr["OverallEfficiency"].ToString();
                                dto.OEE = Convert.ToDouble(rdr["OverallEfficiency"].ToString()).ToString();
                            }
                            if (Convert.ToDouble(dto.OEE) > Convert.ToInt32(rdr["OEGreen"].ToString()))
                            {
                                dto.OEEBackColor = "poGreenBg";
                                dto.Emoji = "Images/Smiley.png";
                            }
                            else if (Convert.ToDouble(dto.OEE) < Convert.ToInt32(rdr["OERed"].ToString()))
                            {
                                dto.OEEBackColor = "poPinkBg";
                                dto.Emoji = "Images/Sad.png";
                            }
                            else
                            {
                                dto.OEEBackColor = "poYellowBg";
                                dto.Emoji = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(rdr["ProductionEfficiency"].ToString()))
                            {
                                //dto.PE = Convert.ToDouble(rdr["ProductionEfficiency"].ToString()) > 100 ? 100.ToString() : rdr["ProductionEfficiency"].ToString();
                                dto.PE = Convert.ToDouble(rdr["ProductionEfficiency"].ToString()).ToString();
                            }
                            if (Convert.ToDouble(dto.PE) > Convert.ToInt32(rdr["PEGreen"].ToString()))
                            {
                                dto.PEBackColor = "poGreenBg";
                            }
                            else if (Convert.ToDouble(dto.PE) < Convert.ToInt32(rdr["PERed"].ToString()))
                            {
                                dto.PEBackColor = "poPinkBg";
                            }
                            else
                            {
                                dto.PEBackColor = "poYellowBg";
                            }

                            if (!string.IsNullOrEmpty(rdr["AvailabilityEfficiency"].ToString()))
                            {
                                //dto.AE = Convert.ToDouble(rdr["AvailabilityEfficiency"].ToString()) > 100 ? 100.ToString() : rdr["AvailabilityEfficiency"].ToString();
                                dto.AE = Convert.ToDouble(rdr["AvailabilityEfficiency"].ToString()).ToString();
                            }
                            if (Convert.ToDouble(dto.AE) > Convert.ToInt32(rdr["AEGreen"].ToString()))
                            {
                                dto.AEBackColor = "poGreenBg";
                            }
                            else if (Convert.ToDouble(dto.AE) < Convert.ToInt32(rdr["AERed"].ToString()))
                            {
                                dto.AEBackColor = "poPinkBg";
                            }
                            else
                            {
                                dto.AEBackColor = "poYellowBg";
                            }
                        }
                        else
                        {
                            dto.Plan = string.Empty;
                            dto.Act = string.Empty;
                            dto.OEE = string.Empty;
                            dto.AE = string.Empty;
                            dto.PE = string.Empty;
                            dto.Emoji = string.Empty;
                        }

                        if (rdr["ColorCode"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Status = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["ColorCode"].ToString().Equals("blue", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Status = "Images/Status/Blue_button.jpg";
                        }
                        else if (rdr["ColorCode"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Status = "Images/Status/Green_btn.jpg";
                        }
                        else if (rdr["ColorCode"].ToString().Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Status = "Images/Status/Yellow_btn.jpg";
                        }
                        else
                        {
                            dto.Status = string.Empty;
                        }
                        dto.Component = rdr["HelpCode1"].ToString();
                        if (rdr["HelpCode1"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Component = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode1"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Component = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode1"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Component = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.Component = string.Empty;
                        }
                        dto.Setting = rdr["HelpCode2"].ToString();
                        if (rdr["HelpCode2"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Setting = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode2"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Setting = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode2"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Setting = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.Setting = string.Empty;
                        }
                        dto.Alaram = rdr["HelpCode3"].ToString();
                        if (rdr["HelpCode3"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Alaram = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode3"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Alaram = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode3"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.Alaram = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.Alaram = string.Empty;
                        }
                        dto.User = rdr["HelpCode4"].ToString();
                        if (rdr["HelpCode4"].ToString().Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.User = "Images/Status/Red_btn.jpg";
                        }
                        else if (rdr["HelpCode4"].ToString().Equals("yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.User = "Images/Status/Yellow_btn.jpg";
                        }
                        else if (rdr["HelpCode4"].ToString().Equals("green", StringComparison.OrdinalIgnoreCase))
                        {
                            dto.User = "Images/Status/Green_btn.jpg";
                        }
                        else
                        {
                            dto.User = string.Empty;
                        }
                        //dto.Materialtime = (rdr["HelpCode1TS"].ToString() == "0:0" ? "" : rdr["HelpCode1TS"].ToString());
                        //dto.Maintainancetime = (rdr["HelpCode2TS"].ToString() == "0:0" ? "" : rdr["HelpCode2TS"].ToString());
                        //dto.Inspectiontime = (rdr["HelpCode3TS"].ToString() == "0:0" ? "" : rdr["HelpCode3TS"].ToString());
                        //dto.Supervisortime = (rdr["HelpCode4TS"].ToString() == "0:0" ? "" : rdr["HelpCode4TS"].ToString());

                        //if (Convert.ToInt32(dto.OEE) <= 50)
                        //{
                        //    dto.OeColor = "#FFC0C0";
                        //}
                        //else if (Convert.ToInt32(dto.OEE) > 50 && Convert.ToInt32(dto.OEE) <= 70)
                        //{
                        //    dto.OeColor = "#FFFFC0";
                        //}
                        //else
                        //{
                        //    dto.OeColor = "#90EE90";
                        //}
                        list.Add(dto);
                    }
                    if (rdr != null) rdr.Close();
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex.ToString());
                    throw;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                //if (rdr != null) rdr.Close();
            }
            return list;
        }

        public static List<AndonSettingData> getColumnData()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            List<AndonSettingData> list = new List<AndonSettingData>();
            AndonSettingData data = null;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from AndonDefaults where Parameter='webandon'", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("Component", StringComparison.OrdinalIgnoreCase) || sdr["ValueInText"].ToString().Equals("Setting", StringComparison.OrdinalIgnoreCase) || sdr["ValueInText"].ToString().Equals("Timer", StringComparison.OrdinalIgnoreCase) || sdr["ValueInText"].ToString().Equals("User", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        data = new AndonSettingData();
                        data.Column = sdr["ValueInText"].ToString();
                        data.CustomColumn = sdr["ValueInText2"].ToString();
                        if (sdr["ValueInBool"].ToString().Trim() == "0")
                        {
                            data.Visibility = false;
                        }
                        else
                        {
                            data.Visibility = true;
                        }
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
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        public static List<LeadingMachine> gridLeadingmachinesLoad(string MachineID, string PlantID, string Param)
        {
            List<LeadingMachine> leadingMachinesList = new List<LeadingMachine>();
            DataTable dtLeading = new DataTable();
            DataTable dtLeadingFinal = new DataTable();
            LeadingMachine leadingMachine = null;
            int count, effectiveCount = 0;
            List<getCurrentShiftTime> CurrentShiftTime = new List<getCurrentShiftTime>();
            SqlDataReader Cockpitreader = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                cmd = new SqlCommand("[dbo].[s_GetANDONHelpCodeDetails_GEA]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-02-01 21:00:00"));
                //cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-06-10 00:00:00.000"));
                //cmd.Parameters.AddWithValue("@DateTime", "2020-02-01 21:00:00");
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@Param", Param);
                //cmd.CommandTimeout = 40000;
                Cockpitreader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (Cockpitreader.HasRows)
                {
                    Cockpitreader.NextResult();
                    dtLeading.Load(Cockpitreader);
                    if (dtLeading != null && dtLeading.Rows.Count > 0)
                    {
                        count = dtLeading.Rows.Count;
                        if (count < 5)
                            effectiveCount = count % 2 == 0 ? count / 2 : count / 2 + 1;
                        else
                            effectiveCount = 3;
                        dtLeadingFinal = dtLeading.Rows.Cast<DataRow>().Take(effectiveCount).CopyToDataTable();


                        #region ----- Single Table ----
                        int finalTblCount = dtLeadingFinal.Rows.Count;
                        for (int i = 0; i < 6; i++)
                        {
                            leadingMachine = new LeadingMachine();
                            if (i == 0)
                            {
                                leadingMachine.Col1 = "";
                                if (effectiveCount == 1)
                                {
                                    leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MachineID"].ToString();
                                }
                                else if (effectiveCount == 2)
                                {
                                    leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MachineID"].ToString();
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MachineID"].ToString();
                                }
                                else
                                {
                                    leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MachineID"].ToString();
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MachineID"].ToString();
                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["MachineID"].ToString();
                                }
                            }
                            else if (i == 1)
                            {
                                leadingMachine.Col1 = "OEE%";
                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["OverAllEfficiency"].ToString();
                                if (effectiveCount == 2)
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["OverAllEfficiency"].ToString();
                                if (effectiveCount == 3)
                                {
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["OverAllEfficiency"].ToString();
                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["OverAllEfficiency"].ToString();
                                }
                            }
                            else if (i == 2)
                            {
                                leadingMachine.Col1 = "Prod. QTY";
                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["Components"].ToString();
                                if (effectiveCount == 2)
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Components"].ToString();
                                if (effectiveCount == 3)
                                {
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Components"].ToString();
                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["Components"].ToString();
                                }
                            }
                            else if (i == 3)
                            {
                                leadingMachine.Col1 = "Operator";
                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["Operatorid"].ToString();
                                if (effectiveCount == 2)
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Operatorid"].ToString();
                                if (effectiveCount == 3)
                                {
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Operatorid"].ToString();
                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["Operatorid"].ToString();
                                }
                            }
                            else if (i == 4)
                            {
                                leadingMachine.Col1 = "Max. DownTime";
                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MaxDowntime"].ToString();
                                if (effectiveCount == 2)
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDowntime"].ToString();
                                if (effectiveCount == 3)
                                {
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDowntime"].ToString();
                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["MaxDowntime"].ToString();
                                }
                            }
                            else
                            {
                                leadingMachine.Col1 = "Max. Down";
                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MaxDownReason"].ToString();
                                if (effectiveCount == 2)
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDownReason"].ToString();
                                if (effectiveCount == 3)
                                {
                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDownReason"].ToString();
                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["MaxDownReason"].ToString();
                                }
                            }
                            leadingMachinesList.Add(leadingMachine);
                        }

                        #endregion
                        //for (int i = 0; i < 5; i++)
                        //{
                        //    leadingMachine = new LeadingMachine();
                        //    if (effectiveCount == 1)
                        //    {
                        //        leadingMachine.Header1 = dtLeadingFinal.Rows[0]["MachineID"].ToString();
                        //    }
                        //    else if (effectiveCount == 2)
                        //    {
                        //        leadingMachine.Header1 = dtLeadingFinal.Rows[0]["MachineID"].ToString();
                        //        leadingMachine.Header2 = dtLeadingFinal.Rows[1]["MachineID"].ToString();
                        //    }
                        //    else
                        //    {
                        //        leadingMachine.Header1 = dtLeadingFinal.Rows[0]["MachineID"].ToString();
                        //        leadingMachine.Header2 = dtLeadingFinal.Rows[1]["MachineID"].ToString();
                        //        leadingMachine.Header3 = dtLeadingFinal.Rows[2]["MachineID"].ToString();
                        //    }
                        //    if (i == 0)
                        //    {
                        //        leadingMachine.Machine1 = dtLeadingFinal.Rows[0]["OverAllEfficiency"].ToString();
                        //        if (effectiveCount == 2)
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["OverAllEfficiency"].ToString();
                        //        if (effectiveCount == 3)
                        //        {
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["OverAllEfficiency"].ToString();
                        //            leadingMachine.Machine3 = dtLeadingFinal.Rows[2]["OverAllEfficiency"].ToString();
                        //        }
                        //    }
                        //    else if (i == 1)
                        //    {
                        //        leadingMachine.Machine1 = dtLeadingFinal.Rows[0]["Components"].ToString();
                        //        if (effectiveCount == 2)
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["Components"].ToString();
                        //        if (effectiveCount == 3)
                        //        {
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["Components"].ToString();
                        //            leadingMachine.Machine3 = dtLeadingFinal.Rows[2]["Components"].ToString();
                        //        }
                        //    }
                        //    else if (i == 2)
                        //    {
                        //        leadingMachine.Machine1 = dtLeadingFinal.Rows[0]["Operatorid"].ToString();
                        //        if (effectiveCount == 2)
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["Operatorid"].ToString();
                        //        if (effectiveCount == 3)
                        //        {
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["Operatorid"].ToString();
                        //            leadingMachine.Machine3 = dtLeadingFinal.Rows[2]["Operatorid"].ToString();
                        //        }
                        //    }
                        //    else if (i == 3)
                        //    {
                        //        leadingMachine.Machine1 = dtLeadingFinal.Rows[0]["MaxDowntime"].ToString();
                        //        if (effectiveCount == 2)
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["MaxDowntime"].ToString();
                        //        if (effectiveCount == 3)
                        //        {
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["MaxDowntime"].ToString();
                        //            leadingMachine.Machine3 = dtLeadingFinal.Rows[2]["MaxDowntime"].ToString();
                        //        }
                        //    }
                        //    else
                        //    {
                        //        leadingMachine.Machine1 = dtLeadingFinal.Rows[0]["MaxDownReason"].ToString();
                        //        if (effectiveCount == 2)
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["MaxDownReason"].ToString();
                        //        if (effectiveCount == 3)
                        //        {
                        //            leadingMachine.Machine2 = dtLeadingFinal.Rows[1]["MaxDownReason"].ToString();
                        //            leadingMachine.Machine3 = dtLeadingFinal.Rows[2]["MaxDownReason"].ToString();
                        //        }
                        //    }
                        //    leadingMachinesList.Add(leadingMachine);
                        //}
                    }

                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        leadingMachine = new LeadingMachine();
                        if (i == 1)
                        {
                            leadingMachine.Col1 = "OEE%";
                        }
                        else if (i == 2)
                        {
                            leadingMachine.Col1 = "Prod. QTY";
                        }
                        else if (i == 3)
                        {
                            leadingMachine.Col1 = "Operator";
                        }
                        else if (i == 4)
                        {
                            leadingMachine.Col1 = "Max. DownTime";
                        }
                        else if (i == 5)
                        {
                            leadingMachine.Col1 = "Max. Down";
                        }
                        leadingMachinesList.Add(leadingMachine);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                //throw ex;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return leadingMachinesList;
        }

        internal static List<string> Getcomponentbydatecomponent(string Component)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> component = new List<string>();
            string query = "";
            try
            {
                query = @"select DISTINCT componentId from componentinformation where componentId Like '%" + Component + "%'";
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

        public static List<LeadingMachine> gridLeadingLaggingMachinesLoad(string MachineID, string PlantID, string Param, out List<LeadingMachine> laggingMachinesList)        {            List<LeadingMachine> leadingMachinesList = new List<LeadingMachine>();            DataTable dtLeading = new DataTable();            DataTable dtLeadingFinal = new DataTable();            LeadingMachine leadingMachine = null;            int count, effectiveCount = 0;            SqlDataReader Cockpitreader = null;            SqlConnection con = ConnectionManager.GetConnection();            SqlCommand cmd = new SqlCommand();            laggingMachinesList = new List<LeadingMachine>();            DataTable dtLagging = new DataTable();            DataTable dtLaggingFinal = new DataTable();            LeadingMachine laggingMachine = null;            int countLag, effectiveCountLag = 0;            DataTable dt = new DataTable();            try            {                cmd = new SqlCommand("[dbo].[s_GetANDONHelpCodeDetails_GEA]", con);                cmd.CommandType = System.Data.CommandType.StoredProcedure;                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                // cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-02-01 21:00:00"));
                //cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-06-10 00:00:00.000"));
                cmd.Parameters.AddWithValue("@PlantID", PlantID);                cmd.Parameters.AddWithValue("@Param", Param);                cmd.CommandTimeout = 4000;                Cockpitreader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);                if (Cockpitreader.HasRows)                {
                    // Cockpitreader.NextResult();
                    dt.Load(Cockpitreader);                    dtLeading.Load(Cockpitreader);                    if (dtLeading.Rows.Count > 0)                    {                        count = dtLeading.Rows.Count;
                        //if (count < 5)
                        //    effectiveCount = count % 2 == 0 ? count / 2 : count / 2 + 1;
                        //else
                        //    effectiveCount = 3;
                        //if (count == 1 || count == 2)
                        //    effectiveCount = 1;
                        //else
                        //    effectiveCount = 2;
                        if (count > 2)                            effectiveCount = 2;                        else                            effectiveCount = count;                        dtLeadingFinal = dtLeading.Rows.Cast<DataRow>().Take(effectiveCount).CopyToDataTable();



























































































                        #region ----- Single Table ----                        int finalTblCount = dtLeadingFinal.Rows.Count;                        for (int i = 0; i < 6; i++)                        {                            leadingMachine = new LeadingMachine();                            if (i == 0)                            {                                leadingMachine.Col1 = "";                                if (effectiveCount == 1)                                {                                    leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MachineID"].ToString();                                }                                else if (effectiveCount == 2)                                {                                    leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MachineID"].ToString();                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MachineID"].ToString();                                }                                else                                {                                    leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MachineID"].ToString();                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MachineID"].ToString();                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["MachineID"].ToString();                                }                            }                            else if (i == 1)                            {                                leadingMachine.Col1 = "OEE%";                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["OverAllEfficiency"].ToString();                                if (effectiveCount == 2)                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["OverAllEfficiency"].ToString();                                if (effectiveCount == 3)                                {                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["OverAllEfficiency"].ToString();                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["OverAllEfficiency"].ToString();                                }                            }                            else if (i == 2)                            {                                leadingMachine.Col1 = "Prod. QTY";                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["Components"].ToString();                                if (effectiveCount == 2)                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Components"].ToString();                                if (effectiveCount == 3)                                {                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Components"].ToString();                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["Components"].ToString();                                }                            }                            else if (i == 3)                            {                                leadingMachine.Col1 = "Operator";                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["Operatorid"].ToString();                                if (effectiveCount == 2)                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Operatorid"].ToString();                                if (effectiveCount == 3)                                {                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["Operatorid"].ToString();                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["Operatorid"].ToString();                                }                            }                            else if (i == 4)                            {                                leadingMachine.Col1 = "Max. DownTime";                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MaxDowntime"].ToString();                                if (effectiveCount == 2)                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDowntime"].ToString();                                if (effectiveCount == 3)                                {                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDowntime"].ToString();                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["MaxDowntime"].ToString();                                }                            }                            else                            {                                leadingMachine.Col1 = "Max. Down";                                leadingMachine.Col2 = dtLeadingFinal.Rows[0]["MaxDownReason"].ToString();                                if (effectiveCount == 2)                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDownReason"].ToString();                                if (effectiveCount == 3)                                {                                    leadingMachine.Col3 = dtLeadingFinal.Rows[1]["MaxDownReason"].ToString();                                    leadingMachine.Col4 = dtLeadingFinal.Rows[2]["MaxDownReason"].ToString();                                }                            }                            leadingMachinesList.Add(leadingMachine);                        }


                        #endregion
                    }

                    //  Cockpitreader.NextResult();
                    dtLagging.Load(Cockpitreader);                    if (dtLagging.Rows.Count > 0)                    {                        countLag = dtLagging.Rows.Count;
                        //if (countLag < 5)
                        //    effectiveCountLag = countLag / 2;
                        //else
                        //    effectiveCountLag = 3;
                        // dtLaggingFinal = dtLagging.AsEnumerable().Reverse().Take(effectiveCountLag).CopyToDataTable();
                        //if (countLag == 1)                        //    effectiveCountLag = 0;                        //else if (countLag == 2 || countLag == 3)                        //    effectiveCountLag = 1;                        //else                        //    effectiveCountLag = 2;
                        if (countLag <= 2)                            effectiveCountLag = 0;                        else if (countLag == 3)                            effectiveCountLag = 1;
                        else if (countLag >= 4)                            effectiveCountLag = 2;                        dtLaggingFinal = dtLagging.Rows.Cast<DataRow>().Take(effectiveCount).CopyToDataTable();                        for (int i = 0; i < 6; i++)                        {                            laggingMachine = new LeadingMachine();                            if (i == 0)                            {                                laggingMachine.Col1 = "";                                if (effectiveCountLag == 1)                                {                                    laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MachineID"].ToString();                                }                                else if (effectiveCountLag == 2)                                {                                    laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MachineID"].ToString();                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MachineID"].ToString();                                }                                else                                {                                    laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MachineID"].ToString();                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MachineID"].ToString();                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["MachineID"].ToString();                                }                            }                            else if (i == 1)                            {                                laggingMachine.Col1 = "OEE%";                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["OverAllEfficiency"].ToString();                                if (effectiveCountLag == 2)                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["OverAllEfficiency"].ToString();                                if (effectiveCountLag == 3)                                {                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["OverAllEfficiency"].ToString();                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["OverAllEfficiency"].ToString();                                }                            }                            else if (i == 2)                            {                                laggingMachine.Col1 = "Prod. QTY";                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["Components"].ToString();                                if (effectiveCountLag == 2)                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Components"].ToString();                                if (effectiveCountLag == 3)                                {                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Components"].ToString();                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["Components"].ToString();                                }                            }                            else if (i == 3)                            {                                laggingMachine.Col1 = "Operator";                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["Operatorid"].ToString();                                if (effectiveCountLag == 2)                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Operatorid"].ToString();                                if (effectiveCountLag == 3)                                {                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Operatorid"].ToString();                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["Operatorid"].ToString();                                }                            }                            else if (i == 4)                            {                                laggingMachine.Col1 = "Max. DownTime";                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MaxDowntime"].ToString();                                if (effectiveCountLag == 2)                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDowntime"].ToString();                                if (effectiveCountLag == 3)                                {                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDowntime"].ToString();                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["MaxDowntime"].ToString();                                }                            }                            else                            {                                laggingMachine.Col1 = "Max. Down";                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MaxDownReason"].ToString();                                if (effectiveCountLag == 2)                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDownReason"].ToString();                                if (effectiveCountLag == 3)                                {                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDownReason"].ToString();                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["MaxDownReason"].ToString();                                }                            }                            laggingMachinesList.Add(laggingMachine);                        }                    }                }                else                {                    for (int i = 0; i < 6; i++)                    {                        leadingMachine = new LeadingMachine();                        if (i == 1)                        {                            leadingMachine.Col1 = "OEE%";                        }                        else if (i == 2)                        {                            leadingMachine.Col1 = "Prod. QTY";                        }                        else if (i == 3)                        {                            leadingMachine.Col1 = "Operator";                        }                        else if (i == 4)                        {                            leadingMachine.Col1 = "Max. DownTime";                        }                        else if (i == 5)                        {                            leadingMachine.Col1 = "Max. Down";                        }                        leadingMachinesList.Add(leadingMachine);                    }                    for (int i = 0; i < 6; i++)                    {                        laggingMachine = new LeadingMachine();                        if (i == 1)                        {                            laggingMachine.Col1 = "OEE%";                        }                        else if (i == 2)                        {                            laggingMachine.Col1 = "Prod. QTY";                        }                        else if (i == 3)                        {                            laggingMachine.Col1 = "Operator";                        }                        else if (i == 4)                        {                            laggingMachine.Col1 = "Max. DownTime";                        }                        else if (i == 5)                        {                            laggingMachine.Col1 = "Max. Down";                        }                        laggingMachinesList.Add(laggingMachine);                    }                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());
                //throw ex;
            }            finally            {                if (con != null) con.Close();            }            return leadingMachinesList;        }
        public static ObservableCollection<OEEData> GetOEEData(string machineId, string plantId, string param)
        {
            ObservableCollection<OEEData> DownPlantOEE = new ObservableCollection<OEEData>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader DownPlantOEEReader = null;
            SqlDataReader shiftreader = null;
            string query = "";
            try
            {
                shiftreader = CurrShift();
                if (shiftreader != null)
                {
                    query = "[dbo].[s_GetANDONHelpCodeDetails_GEA]";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    shiftreader.Read();
                    // cmd.Parameters.AddWithValue("@StartTime", DateTime.Parse(shiftreader["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                    //  cmd.Parameters.AddWithValue("@EndTime", DateTime.Parse(shiftreader["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                    //  cmd.Parameters.AddWithValue("@ShiftName", shiftreader["ShiftName"].ToString());
                    //  cmd.Parameters.AddWithValue("@MachineID", machineId);
                    cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //  cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2019-04-23 00:00:00.000"));
                    //cmd.Parameters.AddWithValue("@DateTime", "2020-02-01 21:00:00");
                    cmd.Parameters.AddWithValue("@PlantID", plantId);
                    cmd.Parameters.AddWithValue("@Param", param);
                    cmd.CommandTimeout = 120;
                    DownPlantOEEReader = cmd.ExecuteReader();
                }
                if (DownPlantOEEReader.HasRows)
                {
                    DownPlantOEEReader.NextResult();
                    while (DownPlantOEEReader.Read())
                    {
                        DownPlantOEE.Add(new OEEData(DownPlantOEEReader["PlantID"].ToString(), Convert.ToDouble(DownPlantOEEReader["OEE"].ToString())));
                    }
                }

            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (DownPlantOEEReader != null) DownPlantOEEReader.Close();
                if (shiftreader != null) shiftreader.Close();
            }
            return DownPlantOEE;
        }

        public static ObservableCollection<DownTimeParetoDATAModel> GetDownParetoData(string machine, string runningPlantName, string param)
        {
            //List<string> DownID = new List<string>();
            //List<double> DownTime = new List<double>();
            //List<double> downtimeTop10 = new List<double>();
            //List<string> downIdTop10 = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader downTimeReader = null;
            string query = "";
            ObservableCollection<DownTimeParetoDATAModel> downTimeCharts = new ObservableCollection<DownTimeParetoDATAModel>();
            try
            {
                List<string> PlantIDList = GEADatabaseAccess.plantCount();

                if (PlantIDList.Count > 0)
                {
                    List<string> MachineIdList = machineCount(runningPlantName);
                    query = "[dbo].[s_GetANDONHelpCodeDetails_GEA]";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    // cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-06-09 21:00:00"));
                    // cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2019-04-23 00:00:00.000"));
                    //  cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2019-04-23 00:00:00.000"));
                    //cmd.Parameters.AddWithValue("@DateTime", "2020-02-01 21:00:00");
                    cmd.Parameters.AddWithValue("@PlantID", runningPlantName);
                    cmd.Parameters.AddWithValue("@Param", param);
                    cmd.CommandTimeout = 120;
                    downTimeReader = cmd.ExecuteReader();

                    if (downTimeReader != null && downTimeReader.HasRows)
                    {
                        downTimeReader.NextResult();
                        downTimeReader.NextResult();
                        while (downTimeReader.Read())
                        {

                            DownTimeParetoDATAModel dtCharts = new DownTimeParetoDATAModel();
                            dtCharts.XValue = downTimeReader["DownID"].ToString();
                            dtCharts.YValue = Math.Round(Convert.ToDouble(downTimeReader["DownTime"].ToString()), 0);
                            downTimeCharts.Add(dtCharts);
                            //DownID.Add(downTimeReader["DownID"].ToString());
                            //DownTime.Add(Math.Round(Convert.ToDouble(downTimeReader["DownTime"].ToString()), 0));
                        }
                        //downIdTop10 = DownID;
                        //downtimeTop10 = DownTime;


                        if (downTimeCharts.Count > 10)
                        {
                            Double totalOthersDownTime = 0;
                            for (int i = 10; i < downTimeCharts.Count; i++)
                            {
                                totalOthersDownTime = totalOthersDownTime + downTimeCharts[i].YValue;
                            }
                            downTimeCharts = new ObservableCollection<DownTimeParetoDATAModel>(downTimeCharts.Take(10));
                            DownTimeParetoDATAModel dtCharts = new DownTimeParetoDATAModel();
                            dtCharts.XValue = "Others";
                            dtCharts.YValue = totalOthersDownTime;
                            downTimeCharts.Add(dtCharts);
                        }
                        //if (DownID.Count > 10)
                        //{
                        //    downIdTop10 = new List<string>(DownID.GetRange(0, 10));
                        //    downtimeTop10 = new List<double>(DownTime.GetRange(0, 10));
                        //    Double totalOthersDownTime = 0;
                        //    for (int i = 10; i < DownTime.Count; i++)
                        //    {
                        //        totalOthersDownTime = totalOthersDownTime + DownTime[i];
                        //    }
                        //    downIdTop10.Add("Others");
                        //    downtimeTop10.Add(totalOthersDownTime);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (downTimeReader != null) downTimeReader.Close();
            }
            //downtimeTop10OUT = downtimeTop10;
            return downTimeCharts;
        }

        public static ObservableCollection<DownTimeParetoDATAModel> GetChartsData(string machine, string runningPlantName, string param, out ObservableCollection<OEEData> DownPlantOEE)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            string query = "";
            ObservableCollection<DownTimeParetoDATAModel> downTimeCharts = new ObservableCollection<DownTimeParetoDATAModel>();
            DownPlantOEE = new ObservableCollection<OEEData>();
            try
            {
                query = "[dbo].[s_GetANDONHelpCodeDetails_GEA]";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //  cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-02-01 21:00:00"));
                // cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-06-10 00:00:00.000"));
                cmd.Parameters.AddWithValue("@PlantID", runningPlantName);
                cmd.Parameters.AddWithValue("@Param", param);
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.NextResult();
                    while (sdr.Read())
                    {
                        DownPlantOEE.Add(new OEEData(sdr["PlantID"].ToString(), Convert.ToDouble(sdr["OEE"].ToString())));
                    }
                    sdr.NextResult();
                    while (sdr.Read())
                    {

                        DownTimeParetoDATAModel dtCharts = new DownTimeParetoDATAModel();
                        dtCharts.XValue = sdr["DownID"].ToString();
                        dtCharts.YValue = Math.Round(Convert.ToDouble(sdr["DownTime"].ToString()), 0);
                        downTimeCharts.Add(dtCharts);
                        //DownID.Add(downTimeReader["DownID"].ToString());
                        //DownTime.Add(Math.Round(Convert.ToDouble(downTimeReader["DownTime"].ToString()), 0));
                    }
                    //downIdTop10 = DownID;
                    //downtimeTop10 = DownTime;
                    if (downTimeCharts.Count > 10)
                    {
                        Double totalOthersDownTime = 0;
                        for (int i = 10; i < downTimeCharts.Count; i++)
                        {
                            totalOthersDownTime = totalOthersDownTime + downTimeCharts[i].YValue;
                        }
                        downTimeCharts = new ObservableCollection<DownTimeParetoDATAModel>(downTimeCharts.Take(10));
                        DownTimeParetoDATAModel dtCharts = new DownTimeParetoDATAModel();
                        dtCharts.XValue = "Others";
                        dtCharts.YValue = totalOthersDownTime;
                        downTimeCharts.Add(dtCharts);
                    }
                    //if (DownID.Count > 10)
                    //{
                    //    downIdTop10 = new List<string>(DownID.GetRange(0, 10));
                    //    downtimeTop10 = new List<double>(DownTime.GetRange(0, 10));
                    //    Double totalOthersDownTime = 0;
                    //    for (int i = 10; i < DownTime.Count; i++)
                    //    {
                    //        totalOthersDownTime = totalOthersDownTime + DownTime[i];
                    //    }
                    //    downIdTop10.Add("Others");
                    //    downtimeTop10.Add(totalOthersDownTime);
                    //}

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return downTimeCharts;
        }

        public static List<LeadingMachine> gridLaggingmachinesLoad(string MachineID, string PlantID, string Param)
        {
            List<LeadingMachine> laggingMachinesList = new List<LeadingMachine>();
            DataTable dtLagging = new DataTable();
            DataTable dtLaggingFinal = new DataTable();
            LeadingMachine laggingMachine = null;
            int count, effectiveCount = 0;
            List<getCurrentShiftTime> CurrentShiftTime = new List<getCurrentShiftTime>();
            SqlDataReader rdr = null;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {

                CurrentShiftTime = CurrentShift();
                cmd = new SqlCommand("[dbo].[s_GetANDONHelpCodeDetails_GEA]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartTime", CurrentShiftTime[0].Starttime);
                // cmd.Parameters.AddWithValue("@EndTime", CurrentShiftTime[0].Endtime);
                //cmd.Parameters.AddWithValue("@StartTime", "2020-01-20 07:30:00");
                //cmd.Parameters.AddWithValue("@EndTime", "2020-01-21 17:00:00");
                //cmd.Parameters.AddWithValue("@SHIFTNAME", CurrentShiftTime[0].Shiftname);
                // cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-02-01 21:00:00"));
                //cmd.Parameters.AddWithValue("@DateTime", Convert.ToDateTime("2020-06-10 00:00:00.000"));
                //cmd.Parameters.AddWithValue("@DateTime", "2020-02-01 21:00:00");
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@Param", Param);
                //cmd.CommandTimeout = 40000;
                rdr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.NextResult();
                    rdr.NextResult();
                    dtLagging.Load(rdr);
                    if (dtLagging != null && dtLagging.Rows.Count > 0)
                    {
                        count = dtLagging.Rows.Count;
                        if (count < 5)
                            effectiveCount = count / 2;
                        else
                            effectiveCount = 3;
                        dtLaggingFinal = dtLagging.AsEnumerable().Reverse().Take(effectiveCount).CopyToDataTable();
                        for (int i = 0; i < 6; i++)
                        {
                            laggingMachine = new LeadingMachine();
                            if (i == 0)
                            {
                                laggingMachine.Col1 = "";
                                if (effectiveCount == 1)
                                {
                                    laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MachineID"].ToString();
                                }
                                else if (effectiveCount == 2)
                                {
                                    laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MachineID"].ToString();
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MachineID"].ToString();
                                }
                                else
                                {
                                    laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MachineID"].ToString();
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MachineID"].ToString();
                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["MachineID"].ToString();
                                }
                            }
                            else if (i == 1)
                            {
                                laggingMachine.Col1 = "OEE%";
                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["OverAllEfficiency"].ToString();
                                if (effectiveCount == 2)
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["OverAllEfficiency"].ToString();
                                if (effectiveCount == 3)
                                {
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["OverAllEfficiency"].ToString();
                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["OverAllEfficiency"].ToString();
                                }
                            }
                            else if (i == 2)
                            {
                                laggingMachine.Col1 = "Prod. QTY";
                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["Components"].ToString();
                                if (effectiveCount == 2)
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Components"].ToString();
                                if (effectiveCount == 3)
                                {
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Components"].ToString();
                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["Components"].ToString();
                                }
                            }
                            else if (i == 3)
                            {
                                laggingMachine.Col1 = "Operator";
                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["Operatorid"].ToString();
                                if (effectiveCount == 2)
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Operatorid"].ToString();
                                if (effectiveCount == 3)
                                {
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["Operatorid"].ToString();
                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["Operatorid"].ToString();
                                }
                            }
                            else if (i == 4)
                            {
                                laggingMachine.Col1 = "Max. DownTime";
                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MaxDowntime"].ToString();
                                if (effectiveCount == 2)
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDowntime"].ToString();
                                if (effectiveCount == 3)
                                {
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDowntime"].ToString();
                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["MaxDowntime"].ToString();
                                }
                            }
                            else
                            {
                                laggingMachine.Col1 = "Max. Down";
                                laggingMachine.Col2 = dtLaggingFinal.Rows[0]["MaxDownReason"].ToString();
                                if (effectiveCount == 2)
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDownReason"].ToString();
                                if (effectiveCount == 3)
                                {
                                    laggingMachine.Col3 = dtLaggingFinal.Rows[1]["MaxDownReason"].ToString();
                                    laggingMachine.Col4 = dtLaggingFinal.Rows[2]["MaxDownReason"].ToString();
                                }
                            }
                            laggingMachinesList.Add(laggingMachine);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        laggingMachine = new LeadingMachine();
                        if (i == 1)
                        {
                            laggingMachine.Col1 = "OEE%";
                        }
                        else if (i == 2)
                        {
                            laggingMachine.Col1 = "Prod. QTY";
                        }
                        else if (i == 3)
                        {
                            laggingMachine.Col1 = "Operator";
                        }
                        else if (i == 4)
                        {
                            laggingMachine.Col1 = "Max. DownTime";
                        }
                        else if (i == 5)
                        {
                            laggingMachine.Col1 = "Max. Down";
                        }
                        laggingMachinesList.Add(laggingMachine);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw ex;
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return laggingMachinesList;
        }


        public static List<LeadingMachine> getShiftDayMachineWise(string plant, string viewtype, out List<LeadingMachine> monthYearList)
        {
            List<LeadingMachine> listShiftDayDecanter = new List<LeadingMachine>();
            monthYearList = new List<LeadingMachine>();
            LeadingMachine ShiftDayDecanterData = null;
            LeadingMachine monthYearData = null;
            SqlDataReader sdr = null;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {

                SqlCommand cmd = new SqlCommand("[dbo].[S_GetAndonDisplay_GEA]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@param", "Machinewise");
                cmd.Parameters.AddWithValue("@PlantID", plant == "All" ? "" : plant);
                cmd.CommandTimeout = 300;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    monthYearData = new LeadingMachine();
                    monthYearData.Col1 = "";
                    monthYearData.Col2 = "Month - " + DateTime.Now.ToString("MMMM");
                    monthYearData.Col3 = "Year - " + DateTime.Now.ToString("yyyy");
                    monthYearList.Add(monthYearData);
                    int i = 0;
                    ShiftDayDecanterData = new LeadingMachine();
                    while (sdr.Read())
                    {
                        string machineQuantivalue = "";
                        if (viewtype == "Shift")
                        {
                            machineQuantivalue = sdr["MachineShift"].ToString();
                        }
                        else if (viewtype == "Day")
                        {
                            machineQuantivalue = sdr["MachineDay"].ToString();
                        }
                        else if (viewtype == "Week")
                        {
                            machineQuantivalue = sdr["MachineWeek"].ToString();
                        }
                        if (i == 0)
                        {
                            ShiftDayDecanterData.Col1 = machineQuantivalue;
                        }
                        else if (i == 1)
                        {
                            ShiftDayDecanterData.Col2 = machineQuantivalue;
                        }

                        if (i == 0)
                        {
                            monthYearData = new LeadingMachine();
                            monthYearData.Col1 = "Decanter";
                            monthYearData.Col2 = sdr["MachineMonth"].ToString();
                            monthYearData.Col3 = sdr["MachineYear"].ToString();
                            monthYearList.Add(monthYearData);
                        }
                        i++;
                    }
                    listShiftDayDecanter.Add(ShiftDayDecanterData);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listShiftDayDecanter;
        }
        public static List<LeadingMachine> getShiftDayComponentWise(string plant, string viewtype, out List<LeadingMachine> listMonthMachine)
        {
            List<LeadingMachine> listShiftDayMachine = new List<LeadingMachine>();
            listMonthMachine = new List<LeadingMachine>();
            LeadingMachine shiftDayMachineData = null;
            LeadingMachine monthMachineData = null;
            SqlDataReader sdr = null;
            SqlConnection con = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_GetAndonDisplay_GEA]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@param", "ComponentWise");
                cmd.Parameters.AddWithValue("@PlantID", plant == "All" ? "" : plant);
                cmd.CommandTimeout = 300;
                sdr = cmd.ExecuteReader();
                DataTable dtMachine = new DataTable();
                DataTable dtQuality = new DataTable();

                if (sdr.HasRows)
                {
                    dtMachine.Load(sdr);
                    dtQuality.Load(sdr);

                    monthMachineData = new LeadingMachine();
                    monthMachineData.Col1 = "";
                    monthMachineData.Col2 = "Month - " + DateTime.Now.ToString("MMMM");
                    monthMachineData.Col3 = "Year - " + DateTime.Now.ToString("yyyy");
                    listMonthMachine.Add(monthMachineData);
                    for (int i = 0; i < dtMachine.Rows.Count; i++)
                    {
                        string machineValue = "", qualityValue = "";
                        if (viewtype == "Shift")
                        {
                            machineValue = dtMachine.Rows[i]["MachineShift"].ToString();
                            qualityValue = dtQuality.Rows[i]["QualityShift"].ToString();
                        }
                        else if (viewtype == "Day")
                        {
                            machineValue = dtMachine.Rows[i]["MachineDay"].ToString();
                            qualityValue = dtQuality.Rows[i]["QualityDay"].ToString();
                        }
                        else if (viewtype == "Week")
                        {
                            machineValue = dtMachine.Rows[i]["MachineWeek"].ToString();
                            qualityValue = dtQuality.Rows[i]["QualityWeek"].ToString();
                        }
                        shiftDayMachineData = new LeadingMachine();
                        shiftDayMachineData.Col1 = dtMachine.Rows[i]["Component"].ToString();
                        shiftDayMachineData.Col2 = machineValue;
                        shiftDayMachineData.Col3 = qualityValue;
                        listShiftDayMachine.Add(shiftDayMachineData);

                        monthMachineData = new LeadingMachine();
                        monthMachineData.Col1 = dtMachine.Rows[i]["Component"].ToString();
                        monthMachineData.Col2 = dtMachine.Rows[i]["MonthQuantity"].ToString();
                        monthMachineData.Col3 = dtMachine.Rows[i]["YearQuantity"].ToString();
                        listMonthMachine.Add(monthMachineData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return listShiftDayMachine;
        }

        #endregion

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

        #region "Machine ID by Plant and Group"
        public static List<string> GetAllMachineByPlantandGroup(string Plantid, string GroupID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> plantid = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct MachineID from PlantMachineGroups where (PlantID=@Plantid or @Plantid='' ) and (GroupID=@GroupID or @GroupID='') order by MachineID ", sqlConn);
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

        internal static List<FrequencyEntity> GetAllFrequencies()
        {
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            List<FrequencyEntity> frequenciesList = new List<FrequencyEntity>();
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select distinct * from ActivityFreq_MGTL", Conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FrequencyEntity frequencyEntity = new FrequencyEntity()
                        {
                            FreqID = Convert.ToInt32(rdr["FreqID"]),
                            Frequency = rdr["Frequency"].ToString(),
                            FreqValue = rdr["Freqvalue"].ToString(),
                            FreqType = rdr["Freqtype"].ToString()
                        };
                        frequenciesList.Add(frequencyEntity);
                    }
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
            return frequenciesList;
        }

        internal static List<MaintenanceChecklistEntity> GetMaintenanceChklistData(string machineID, string frequency)
        {
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            List<MaintenanceChecklistEntity> dtMaintenanceChecklistData = new List<MaintenanceChecklistEntity>();
            string query = @"select * from ActivityMaster_MGTL where (MachineID=@MachineID or @MachineID='') and (FreqID=@FreqID or @FreqID='')";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(query, Conn);
                command.Parameters.AddWithValue("@MachineID", machineID);
                command.Parameters.AddWithValue("@FreqID", frequency);
                command.CommandType = CommandType.Text;
                rdr = command.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        MaintenanceChecklistEntity maintenanceChecklist = new MaintenanceChecklistEntity();
                        maintenanceChecklist.IDD = Convert.ToInt32(rdr["IDD"]);
                        maintenanceChecklist.ActivityID = Convert.ToInt32(rdr["ActivityID"]);
                        maintenanceChecklist.Activity = rdr["Activity"].ToString();
                        maintenanceChecklist.FreqID = Convert.ToInt32(rdr["FreqID"]);
                        maintenanceChecklist.MachineID = rdr["MachineID"].ToString();
                        maintenanceChecklist.IsEnabled = Convert.ToInt32(rdr["IsMandatory"]).Equals(1) ? true : false;
                        maintenanceChecklist.Method = !string.IsNullOrEmpty(rdr["Method"].ToString()) ? rdr["Method"].ToString() : "";
                        maintenanceChecklist.Criteria = !string.IsNullOrEmpty(rdr["Criteria"].ToString()) ? rdr["Criteria"].ToString() : "";
                        maintenanceChecklist.TemplateType = !string.IsNullOrEmpty(rdr["TemplateType"].ToString()) ? rdr["TemplateType"].ToString() : "";
                        dtMaintenanceChecklistData.Add(maintenanceChecklist);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtMaintenanceChecklistData;
        }

        internal static bool IsChecklistMasterKeyExists(string activityId, string machineID, string freqID)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select * from ActivityMaster_MGTL where ActivityID = @ActivityID and MachineID = @MachineID and FreqID = @FreqID";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@FreqID", freqID);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsExists = rdr.HasRows ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static void UpdateMaintenanceChecklist(string activityId, string activity, string freqID, string machineID, string method, string criteria, string tempType, int isEnabled)
        {
            SqlConnection conn = null;
            string query = @"if exists(select * from ActivityMaster_MGTL where ActivityID = @ActivityID and MachineID = @MachineID and FreqID = @FreqID) BEGIN update ActivityMaster_MGTL set ActivityID = @ActivityID, Activity = @Activity, Method = @Method, Criteria = @Criteria, TemplateType = @TemplateType, IsMandatory = @IsMandatory where ActivityID = @ActivityID and MachineID = @MachineID and FreqID = @FreqID END ELSE BEGIN insert into ActivityMaster_MGTL(ActivityID, Activity, FreqID, MachineID, Method, Criteria, TemplateType, IsMandatory) values(@ActivityID, @Activity, @FreqID, @MachineID, @Method, @Criteria, @TemplateType, @IsMandatory) END";
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@Activity", activity);
                cmd.Parameters.AddWithValue("@FreqID", freqID);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Method", method);
                cmd.Parameters.AddWithValue("@Criteria", criteria);
                cmd.Parameters.AddWithValue("@TemplateType", tempType);
                cmd.Parameters.AddWithValue("@IsMandatory", isEnabled);
                cmd.CommandType = CommandType.Text;
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

        internal static bool DeleteMaintenanceChklistData(string idd)
        {
            bool IsDeleted = false;
            SqlConnection conn = null;
            string query = @"delete from ActivityMaster_MGTL where IDD = @IDD";
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.CommandType = CommandType.Text;
                IsDeleted = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                IsDeleted = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return IsDeleted;
        }

        internal static DataTable GetWeeklyChecklistData(string lineId, string machineID, string year, string frequency)
        {
            SqlConnection Conn = null;
            DataTable dtWeeklyChecklistData = new DataTable();
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetWeekly_MasterCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Line", lineId);
                command.Parameters.AddWithValue("@Machine", machineID);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Frequency", frequency);
                command.Parameters.AddWithValue("@param", "View");
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dtWeeklyChecklistData);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtWeeklyChecklistData;
        }

        internal static bool GenerateActivityForFrequency(string lineId, string machineID, string year, string frequency)
        {
            bool success = false;
            SqlConnection Conn = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetWeekly_MasterCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Line", lineId);
                command.Parameters.AddWithValue("@Machine", machineID);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Frequency", frequency);
                command.Parameters.AddWithValue("@param", "GenerateFrequencyANDSave");
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool IsWeekAvailable(string machineId, string actID, string freqID, string year, string weekNo)
        {
            bool IsAvailable = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select * from ActivityMasterYearlyData_MGTL where MachineID = @MachineID and ActivityID = @ActivityID and FreqID = @FreqID and [Year] = @Year AND WeekNo = @WeekNo";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@ActivityID", actID);
                cmd.Parameters.AddWithValue("@FreqID", freqID);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@WeekNo", weekNo.Substring(4));
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsAvailable = rdr.HasRows ? false : true;
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsAvailable;
        }

        internal static bool UpdateScheduledWeek(string machineId, string actId, string activity, string freqID, string year, int oldMonthNo, int newMonthNo, string weekNo, string newWeekNo)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"IF EXISTS(Select * from ActivityMasterYearlyData_MGTL where MachineID=@MachineID and ActivityID=@ActivityID and FreqID=@FreqID and [Year]=@Year AND MonthNo=@OldMonthNo AND WeekNo=@WeekNo) 
              BEGIN
              DELETE FROM ActivityMasterYearlyData_MGTL where MachineID=@MachineID and ActivityID=@ActivityID and FreqID=@FreqID and [Year]=@Year AND MonthNo=@OldMonthNo AND WeekNo=@WeekNo
	          Insert into ActivityMasterYearlyData_MGTL(ActivityID, Activity, FreqID, [Year], WeekNo, MachineID, MonthNo) values(@ActivityID, @Activity, @FreqID, @Year, @NewWeekNo, @MachineID, @NewMonthNo)
              END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(query, Conn);
                command.Parameters.AddWithValue("@ActivityID", actId);
                command.Parameters.AddWithValue("@Activity", activity);
                command.Parameters.AddWithValue("@FreqID", freqID);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@WeekNo", weekNo.Substring(4));
                command.Parameters.AddWithValue("@MachineID", machineId);
                command.Parameters.AddWithValue("@OldMonthNo", oldMonthNo);
                command.Parameters.AddWithValue("@NewMonthNo", newMonthNo);
                command.Parameters.AddWithValue("@NewWeekNo", newWeekNo.Substring(4));
                command.CommandType = CommandType.Text;
                success = command.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static DataTable GetWeeklyChklistTransactionData(string lineId, string machineID)
        {
            SqlConnection Conn = null;
            DataTable dtWeeklyChklistTransactionData = new DataTable();
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetWeekly_TransactionCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Param", "view");
                command.Parameters.AddWithValue("@Line", lineId);
                command.Parameters.AddWithValue("@Machine", machineID);
                command.Parameters.AddWithValue("@Date", DateTime.Now);
                command.Parameters.AddWithValue("@freqid", "");
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dtWeeklyChklistTransactionData);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtWeeklyChklistTransactionData;
        }

        internal static DataTable GetDailyChecklistReportData(string lineID, string machineID, string startTime, out DataTable Secondresultset)
        {
            SqlConnection Conn = null;
            Secondresultset = new DataTable();
            DataTable dtDailyChklistReportData = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetDailyCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Param", "Report");
                command.Parameters.AddWithValue("@Line", lineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : lineID);
                command.Parameters.AddWithValue("@Machine", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : machineID);
                command.Parameters.AddWithValue("@Startdate", startTime);
                command.Parameters.AddWithValue("@Frequency", "Daily");
                command.CommandType = CommandType.StoredProcedure;
                rdr = command.ExecuteReader();
                dtDailyChklistReportData.Load(rdr);
                Secondresultset.Load(rdr);
                dtDailyChklistReportData.AcceptChanges();
                Secondresultset.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtDailyChklistReportData;
        }

        internal static DataTable GetWeeklyChklistReportData(string lineId, string machineID, int year)
        {
            SqlConnection Conn = null;
            DataTable dtWeeklyChklistReportData = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetWeekly_TransactionCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Param", "Report");
                command.Parameters.AddWithValue("@Line", lineId);
                command.Parameters.AddWithValue("@Machine", machineID);
                // command.Parameters.AddWithValue("@Date", DateTime.Now);
                DateTime date = new DateTime(year, 1, 1);
                command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@freqid", "");
                command.CommandType = CommandType.StoredProcedure;
                rdr = command.ExecuteReader();

                dtWeeklyChklistReportData.Load(rdr);
                dtWeeklyChklistReportData.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtWeeklyChklistReportData;
        }
        internal static DataTable GetDailyChecklistReportRemarksData(string lineID, string machineID, string startTime)
        {
            SqlConnection Conn = null;
            DataTable dtDailyChklistReportData = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetDailyCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Param", "Report_ViewRemarks");
                command.Parameters.AddWithValue("@Line", lineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : lineID);
                command.Parameters.AddWithValue("@Machine", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? string.Empty : machineID);
                command.Parameters.AddWithValue("@Startdate", startTime);
                command.Parameters.AddWithValue("@Frequency", "Daily");
                command.CommandType = CommandType.StoredProcedure;
                rdr = command.ExecuteReader();
                dtDailyChklistReportData.Load(rdr);
                dtDailyChklistReportData.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtDailyChklistReportData;
        }
        internal static DataTable GetWeeklyChklistReportData(string lineId, string machineID, int year, string Type)
        {
            SqlConnection Conn = null;
            DataTable dtWeeklyChklistReportData = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetWeekly_TransactionCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Line", lineId);
                command.Parameters.AddWithValue("@Machine", machineID);
                //command.Parameters.AddWithValue("@Date", DateTime.Now);
                DateTime date = new DateTime(year, 1, 1);
                command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@freqid", "");
                command.Parameters.AddWithValue("@param", "view2");
                command.CommandType = CommandType.StoredProcedure;
                rdr = command.ExecuteReader();

                dtWeeklyChklistReportData.Load(rdr);
                dtWeeklyChklistReportData.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtWeeklyChklistReportData;
        }

        internal static DataTable GetWeeklyChklistReportRemarksData(string lineId, string machineID, int year)
        {
            SqlConnection Conn = null;
            DataTable dtWeeklyChklistReportData = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetWeekly_TransactionCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Param", "Report_ViewRemarks");
                command.Parameters.AddWithValue("@Line", lineId);
                command.Parameters.AddWithValue("@Machine", machineID);
                // command.Parameters.AddWithValue("@Date", DateTime.Now);
                DateTime date = new DateTime(year, 1, 1);
                command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@freqid", "");
                command.CommandType = CommandType.StoredProcedure;
                rdr = command.ExecuteReader();

                dtWeeklyChklistReportData.Load(rdr);
                dtWeeklyChklistReportData.AcceptChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return dtWeeklyChklistReportData;
        }

        internal static bool UpdateChecklistTransactionData(string lineId, string macId, string freqId, string actId, string weekNo, string actValue, string remarks)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"s_GetWeekly_TransactionCheckListDetails_GEA", Conn);
                command.Parameters.AddWithValue("@Param", "Save");
                command.Parameters.AddWithValue("@Line", lineId);
                command.Parameters.AddWithValue("@Machine", macId);
                command.Parameters.AddWithValue("@Date", DateTime.Now);
                command.Parameters.AddWithValue("@freqid", freqId);
                command.Parameters.AddWithValue("@Activityid", actId);
                command.Parameters.AddWithValue("@weekno", weekNo);
                command.Parameters.AddWithValue("@ActivityValue", actValue);
                command.Parameters.AddWithValue("@Remarks", remarks);
                command.CommandType = CommandType.StoredProcedure;
                int i = command.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static bool UpdateQualityScheduleDetails(string priority, string machine, string prodOrder, string materialId, string opnNum, string qty, string GRNNumber, string SupplierName, bool NewProdOrder, string process)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = "";
            if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
            {
                query = @"if not exists(select * from ScheduleDetails_GEA where SchedulePriority = @SchedulePriority) BEGIN if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo  and GrnNo=@GrnNo) BEGIN insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority,UserPriority, UpdatedBy, UpdatedTS,GrnNo,Supplier,NewProdDevelopment) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @SchedulePriority,@UserPriority, @UpdatedBy, @UpdatedTS,@GrnNo,@Supplier,@NewProdDevelopment) END END";
            }
            else
            {
                query = @"if not exists(select * from ScheduleDetails_GEA where SchedulePriority = @SchedulePriority) BEGIN if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo) BEGIN insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority,UserPriority, UpdatedBy, UpdatedTS,GrnNo,Supplier,NewProdDevelopment) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @SchedulePriority,@UserPriority, @UpdatedBy, @UpdatedTS,@GrnNo,@Supplier,@NewProdDevelopment) END END";

            }

            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                cmd.Parameters.AddWithValue("@UserPriority", GetMaxUserPriority(machine));
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@GrnNo", GRNNumber);
                cmd.Parameters.AddWithValue("@Supplier", SupplierName);
                cmd.Parameters.AddWithValue("@NewProdDevelopment", NewProdOrder);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static bool UpdateScheduleDetails(string priority, string machine, string prodOrder, string materialId, string opnNum, string qty, string GRNNumber, string SupplierName, bool NewProdOrder)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if not exists(select * from ScheduleDetails_GEA where SchedulePriority = @SchedulePriority) BEGIN if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo) BEGIN insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority,UserPriority, UpdatedBy, UpdatedTS) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @SchedulePriority,@UserPriority, @UpdatedBy, @UpdatedTS) END END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                cmd.Parameters.AddWithValue("@UserPriority", GetMaxUserPriority(machine));
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }


        internal static void DeleteNewProductionSchedules(string machine)
        {
            SqlConnection Conn = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                //if (machine == "Assembly")
                //{
                SqlCommand cmd = new SqlCommand(@"delete from ScheduleDetails_GEA where Status='New' and  Machineid=@Machineid", Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.ExecuteNonQuery();
                //}
                //else
                //{
                //    SqlCommand cmd = new SqlCommand(@"delete from ScheduleDetails_GEA where Status='New'", Conn);
                //    cmd.CommandType = CommandType.Text;
                //    cmd.ExecuteNonQuery();
                //}


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

        internal static string GetChecklistRemarks(string machineID, string freqId, string actId, string weekNo)
        {
            string remark = string.Empty;
            string query = @"select top 1 Remarks from [ActivityTransaction_MGTL] where ActivityID = @ActivityID and Frequency = @FreqID and Machineid = @MachineID and WeekNo = @WeekNo";
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ActivityID", actId);
                cmd.Parameters.AddWithValue("@FreqID", freqId);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@WeekNo", weekNo);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        remark = rdr["Remarks"].ToString();
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
                if (Conn != null) Conn.Close();
            }
            return remark;
        }

        internal static bool ImportSchedueData(string priority, string machine, string prodOrder, string materialId, string opnNum, string qty)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            int Userpriority = 0;
            string query = @"if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo) BEGIN insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority, UpdatedBy, UpdatedTS,UserPriority) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @EndSchedulePriority, @UpdatedBy, @UpdatedTS,@UserPriority) END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@EndSchedulePriority", GetMaxSchedulePriority(out Userpriority) + 1);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@UserPriority", priority);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                //cmd.Parameters.AddWithValue("@GrnNo", GRNNumber);
                //cmd.Parameters.AddWithValue("@Supplier", SupplierName);
                //cmd.Parameters.AddWithValue("@NewProdDevelopment", NewProdDev.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static bool ImportQualitySchedueData(string priority, string machine, string prodOrder, string materialId, string opnNum, string qty, string GRNNumber, string SupplierName, string NewProdDev, string process)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            int Userpriority = 0;
            string query = "";
            if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
            {
                query = @"if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo  and GrnNo=@GrnNo) BEGIN insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority, UpdatedBy, UpdatedTS,UserPriority,GrnNo,Supplier,NewProdDevelopment) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @EndSchedulePriority, @UpdatedBy, @UpdatedTS,@UserPriority,@GrnNo,@Supplier,@NewProdDevelopment) END";
            }
            else
            {
                query = @"if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo) BEGIN insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority, UpdatedBy, UpdatedTS,UserPriority,GrnNo,Supplier,NewProdDevelopment) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @EndSchedulePriority, @UpdatedBy, @UpdatedTS,@UserPriority,@GrnNo,@Supplier,@NewProdDevelopment) END";
            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@EndSchedulePriority", GetMaxSchedulePriority(out Userpriority) + 1);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@UserPriority", priority);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                cmd.Parameters.AddWithValue("@GrnNo", GRNNumber);
                cmd.Parameters.AddWithValue("@Supplier", SupplierName);
                cmd.Parameters.AddWithValue("@NewProdDevelopment", NewProdDev.Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static List<ScheduleDetailsEntity> GetProductionSchedulesData(string machineID, string status, string prod_order, string material_id, string from_date, string to_date)
        {
            List<ScheduleDetailsEntity> prodScheduleDetails = new List<ScheduleDetailsEntity>();
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                status = !string.IsNullOrEmpty(status) && status.Last().Equals(',') ? status.Remove(status.Length - 1) : status;
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"S_ViewSchedulerDetails_GEA", Conn);
                cmd.Parameters.AddWithValue("@Machineid", machineID);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ProductionOrder", prod_order);
                cmd.Parameters.AddWithValue("@MaterialID", material_id);
                cmd.Parameters.AddWithValue("@Fromdate", !string.IsNullOrEmpty(from_date) ? Util.GetDateTime(from_date).ToSQLDateTimeFormat() : "");
                cmd.Parameters.AddWithValue("@ToDate", !string.IsNullOrEmpty(to_date) ? Util.GetDateTime(to_date).ToSQLDateTimeFormat() : "");
                cmd.CommandTimeout = 450;
                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ScheduleDetailsEntity scheduleDetails = new ScheduleDetailsEntity
                        {
                            IDD = Convert.ToInt32(dataReader["SCHID"]),
                            Priority = Convert.ToInt32(dataReader["SCHPriority"]),
                            UserPriority = dataReader["UserPriority"].Equals(DBNull.Value) ? "" : (dataReader["UserPriority"].ToString()),
                            ProductionOrderNumber = dataReader["PONumber"].ToString(),
                            MaterialID = dataReader["MaterialID"].ToString(),
                            Model = dataReader["Model"].ToString(),
                            ModelDescription = dataReader["ModelDescription"].ToString(),
                            OperationNumber = dataReader["Opnno"].ToString(),
                            Quantity = Convert.ToInt32(dataReader["Qty"]),
                            StdCycleTime = dataReader["StdCycleTime"].ToString(),
                            StdSetupTime = dataReader["StdSetupTime"].ToString(),
                            ScheduledStartTime = dataReader["ScheduledStarttime"].ToString(),
                            ScheduledEndTime = dataReader["ScheduledEndtime"].ToString(),
                            ActualStartTime = dataReader["ActualStartTime"].ToString(),
                            PredictedCompletionTime = dataReader["PredictedCompletion"].ToString(),
                            ActualEndTime = dataReader["ActualEndtime"].ToString(),
                            Status = dataReader["SCHStatus"].ToString(),
                            OldScheduledStartTime = dataReader["PreviousScheduleStart"].ToString(),
                            OldScheduledEndTime = dataReader["PreviousScheduleEnd"].ToString(),
                        };
                        prodScheduleDetails.Add(scheduleDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return prodScheduleDetails;
        }


        internal static List<ScheduleDetailsEntity> GetQualityProductionSchedulesData(string machineID, string status, string prod_order, string material_id, string from_date, string to_date)
        {
            List<ScheduleDetailsEntity> prodScheduleDetails = new List<ScheduleDetailsEntity>();
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                status = !string.IsNullOrEmpty(status) && status.Last().Equals(',') ? status.Remove(status.Length - 1) : status;
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"S_ViewSchedulerDetails_GEA", Conn);
                cmd.Parameters.AddWithValue("@Machineid", machineID);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ProductionOrder", prod_order);
                cmd.Parameters.AddWithValue("@MaterialID", material_id);
                cmd.Parameters.AddWithValue("@Fromdate", !string.IsNullOrEmpty(from_date) ? Util.GetDateTime(from_date).ToSQLDateTimeFormat() : "");
                cmd.Parameters.AddWithValue("@ToDate", !string.IsNullOrEmpty(to_date) ? Util.GetDateTime(to_date).ToSQLDateTimeFormat() : "");
                cmd.CommandTimeout = 450;
                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ScheduleDetailsEntity scheduleDetails = new ScheduleDetailsEntity
                        {
                            IDD = Convert.ToInt32(dataReader["SCHID"]),
                            Priority = Convert.ToInt32(dataReader["SCHPriority"]),
                            UserPriority = dataReader["UserPriority"].Equals(DBNull.Value) ? "" : (dataReader["UserPriority"].ToString()),
                            ProductionOrderNumber = dataReader["PONumber"].ToString(),
                            MaterialID = dataReader["MaterialID"].ToString(),
                            Model = dataReader["Model"].ToString(),
                            ModelDescription = dataReader["ModelDescription"].ToString(),
                            OperationNumber = dataReader["Opnno"].ToString(),
                            Quantity = Convert.ToInt32(dataReader["Qty"]),
                            StdCycleTime = dataReader["StdCycleTime"].ToString(),
                            StdSetupTime = dataReader["StdSetupTime"].ToString(),
                            ScheduledStartTime = dataReader["ScheduledStarttime"].ToString(),
                            ScheduledEndTime = dataReader["ScheduledEndtime"].ToString(),
                            ActualStartTime = dataReader["ActualStartTime"].ToString(),
                            PredictedCompletionTime = dataReader["PredictedCompletion"].ToString(),
                            ActualEndTime = dataReader["ActualEndtime"].ToString(),
                            Status = dataReader["SCHStatus"].ToString(),
                            GRNNumber = dataReader["GrnNo"].ToString(),
                            SupplierName = dataReader["Supplier"].ToString(),
                            NewProdDev = dataReader["NewProdDevelopment"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            OldScheduledStartTime = dataReader["PreviousScheduleStart"].ToString(),
                            OldScheduledEndTime = dataReader["PreviousScheduleEnd"].ToString(),
                        };
                        prodScheduleDetails.Add(scheduleDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return prodScheduleDetails;
        }

        internal static List<AssemblyScheduleDetailsEntity> GetAssemblySchedulesData(string machineID, string status, string prod_order, string material_id, string from_date, string to_date)
        {
            List<AssemblyScheduleDetailsEntity> prodScheduleDetails = new List<AssemblyScheduleDetailsEntity>();
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                status = !string.IsNullOrEmpty(status) && status.Last().Equals(',') ? status.Remove(status.Length - 1) : status;
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"S_ViewAssemblySchedulerDetails_GEA", Conn);
                cmd.Parameters.AddWithValue("@Machineid", machineID);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ProductionOrder", prod_order);
                cmd.Parameters.AddWithValue("@MaterialID", material_id);
                cmd.Parameters.AddWithValue("@Fromdate", !string.IsNullOrEmpty(from_date) ? Util.GetDateTime(from_date).ToSQLDateTimeFormat() : "");
                cmd.Parameters.AddWithValue("@ToDate", !string.IsNullOrEmpty(to_date) ? Util.GetDateTime(to_date).ToSQLDateTimeFormat() : "");
                cmd.CommandTimeout = 450;
                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        AssemblyScheduleDetailsEntity scheduleDetails = new AssemblyScheduleDetailsEntity
                        {
                            IDD = Convert.ToInt32(dataReader["SCHID"]),
                            Priority = Convert.ToInt32(dataReader["SCHPriority"]),
                            UserPriority = dataReader["UserPriority"].Equals(DBNull.Value) ? "" : (dataReader["UserPriority"].ToString()),
                            ProductionOrderNumber = dataReader["PONumber"].ToString(),
                            LocalExport = dataReader["Localorexport"].ToString(),
                            SaleOrder = dataReader["SaleOrder"].ToString(),
                            OperationNumber = dataReader["Opnno"].ToString(),
                            Model = dataReader["MaterialID"].ToString(),
                            RDDMachines = (dataReader["RDDMachines"] == null || dataReader["RDDMachines"] == DBNull.Value || dataReader["RDDMachines"].ToString() == "") ? "" : Util.GetDateTime(dataReader["RDDMachines"].ToString()).ToString("dd-MM-yyyy"),
                            FabricationNumber = dataReader["FabricationNo"].ToString(),
                            ScrollWelded = dataReader["ScrollWelded"].ToString(),
                            Quantity = Convert.ToInt32(dataReader["Qty"]),
                            StdCycleTime = dataReader["stdcycletime"].ToString(),
                            StdSetupTime = dataReader["StdSetupTime"].ToString(),
                            ScheduledStartTime = dataReader["ScheduledStarttime"].ToString(),
                            ScheduledEndTime = dataReader["ScheduledEndtime"].ToString(),
                            ActualStartTime = dataReader["ActualStartTime"].ToString(),
                            PredictedCompletionTime = dataReader["PredictedCompletion"].ToString(),
                            ActualEndTime = dataReader["ActualEndtime"].ToString(),
                            Status = dataReader["SCHStatus"].ToString(),
                            //Enabled = dataReader["SCHStatus"].ToString().Equals("new", StringComparison.OrdinalIgnoreCase) ? true : dataReader["SCHStatus"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase) ? true : dataReader["SCHStatus"].ToString().Equals("Parked", StringComparison.OrdinalIgnoreCase) ? true : false,
                            Customer = dataReader["Customer"].ToString(),
                            Location = dataReader["Location"].ToString(),
                            Activities = dataReader["SubActivities"].ToString(),
                            OldScheduledStartTime = dataReader["PreviousScheduleStart"].ToString(),
                            OldScheduledEndTime = dataReader["PreviousScheduleEnd"].ToString(),
                        };
                        prodScheduleDetails.Add(scheduleDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return prodScheduleDetails;
        }
        internal static bool UpdateScheduleData(string idd, string macId, string new_prodno, string new_matid, string new_opnno, string qty, string GRNNumber, string SupplierName, bool NewProdDeveloper)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if exists(select * from ScheduleDetails_GEA where IDD = @IDD) BEGIN update ScheduleDetails_GEA set ProductionOrder = @NewProductionOrder, MaterialID = @NewMaterialID, OperationNo = @NewOperationNo, ScheduleQty = @ScheduleQty, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD = @IDD END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.Parameters.AddWithValue("@Machineid", macId);
                cmd.Parameters.AddWithValue("@NewProductionOrder", new_prodno);
                cmd.Parameters.AddWithValue("@NewMaterialID", new_matid);
                cmd.Parameters.AddWithValue("@NewOperationNo", new_opnno);
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                //cmd.Parameters.AddWithValue("@GrnNo", GRNNumber);
                //cmd.Parameters.AddWithValue("@Supplier", SupplierName);
                //cmd.Parameters.AddWithValue("@NewProdDevelopment", NewProdDeveloper);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static bool UpdateQualityScheduleData(string idd, string macId, string new_prodno, string new_matid, string new_opnno, string qty, string GRNNumber, string SupplierName, bool NewProdDeveloper)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if exists(select * from ScheduleDetails_GEA where IDD = @IDD) BEGIN update ScheduleDetails_GEA set ProductionOrder = @NewProductionOrder, MaterialID = @NewMaterialID, OperationNo = @NewOperationNo, ScheduleQty = @ScheduleQty, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS,GrnNo=@GrnNo, Supplier=@Supplier,NewProdDevelopment=@NewProdDevelopment where IDD = @IDD END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.Parameters.AddWithValue("@Machineid", macId);
                cmd.Parameters.AddWithValue("@NewProductionOrder", new_prodno);
                cmd.Parameters.AddWithValue("@NewMaterialID", new_matid);
                cmd.Parameters.AddWithValue("@NewOperationNo", new_opnno);
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                cmd.Parameters.AddWithValue("@GrnNo", GRNNumber);
                cmd.Parameters.AddWithValue("@Supplier", SupplierName);
                cmd.Parameters.AddWithValue("@NewProdDevelopment", NewProdDeveloper);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static bool UpdateScheduleData(string idd, string macId, string new_prodno, string new_matid, string new_opnno, string qty, string newfabricationno, string localexport, string SaleOrder, string ScrollNumber, string RDDMachines, string Customer, string Location)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if exists(select * from ScheduleDetails_GEA where IDD = @IDD) BEGIN update ScheduleDetails_GEA set ProductionOrder = @NewProductionOrder, MaterialID = @NewMaterialID, OperationNo = @NewOperationNo, ScheduleQty = @ScheduleQty, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS , FabricationNo=@FabricationNo,Localorexport=@Localorexport,SaleOrder=@SaleOrder,ScrollWelded=@ScrollWelded,RDDMachines=@RDDMachines,Customer=@Customer,Location=@Location where IDD = @IDD END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.Parameters.AddWithValue("@Machineid", macId);
                cmd.Parameters.AddWithValue("@NewProductionOrder", new_prodno);
                cmd.Parameters.AddWithValue("@NewMaterialID", new_matid);
                cmd.Parameters.AddWithValue("@NewOperationNo", new_opnno);
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);

                cmd.Parameters.AddWithValue("@FabricationNo", newfabricationno);
                cmd.Parameters.AddWithValue("@Localorexport", localexport);
                cmd.Parameters.AddWithValue("@SaleOrder", SaleOrder);
                cmd.Parameters.AddWithValue("@ScrollWelded", ScrollNumber);
                cmd.Parameters.AddWithValue("@RDDMachines", Util.GetDateTime(RDDMachines).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Customer", Customer);
                cmd.Parameters.AddWithValue("@Location", Location);

                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }


        internal static bool UpdateScheduleData(string idd, string macId, string priority, string old_prodno, string new_prodno, string old_matid, string new_matid, string old_opnno, string new_opnno, string qty)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if exists(select * from ScheduleDetails_GEA where IDD = @IDD) BEGIN update ScheduleDetails_GEA set ProductionOrder = @NewProductionOrder, MaterialID = @NewMaterialID, OperationNo = @NewOperationNo, ScheduleQty = @ScheduleQty, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD = @IDD END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@Machineid", macId);
                cmd.Parameters.AddWithValue("@OldProductionOrder", old_prodno);
                cmd.Parameters.AddWithValue("@NewProductionOrder", new_prodno);
                cmd.Parameters.AddWithValue("@OldMaterialID", old_matid);
                cmd.Parameters.AddWithValue("@NewMaterialID", new_matid);
                cmd.Parameters.AddWithValue("@OldOperationNo", old_opnno);
                cmd.Parameters.AddWithValue("@NewOperationNo", new_opnno);
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static bool DeleteProdSchedule(string idd)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"delete from ScheduleDetails_GEA where IDD = @IDD";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool DeleteProdSchedule_Assembly(string idd)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"delete from ScheduleDetails_GEA_Assembly where IDD = @IDD";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }
        internal static List<PriorityType> GetAllSchedulePrioritiesKeyValue(string machine)
        {
            List<PriorityType> lstPriorities = new List<PriorityType>();
            SqlConnection Conn = null;
            SqlDataReader sqlDataReader = null;
            string query = @"select distinct SchedulePriority, UserPriority from ScheduleDetails_GEA where Machineid = @Machineid and status='New'";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        lstPriorities.Add(new PriorityType { SystemPriority = sqlDataReader["SchedulePriority"].ToString(), UserPriority = sqlDataReader["UserPriority"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return lstPriorities;
        }
        internal static List<string> GetAllSchedulePriorities(string machine)
        {
            List<string> lstPriorities = new List<string>();
            SqlConnection Conn = null;
            SqlDataReader sqlDataReader = null;
            string query = @"select distinct SchedulePriority from ScheduleDetails_GEA where Machineid = @Machineid";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        lstPriorities.Add(sqlDataReader["SchedulePriority"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return lstPriorities;
        }

        internal static bool IsProdMatIdUnique(string prodOrder, string materialId, string grnNo, string process)
        {
            bool IsUnique = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and  MaterialID <> @MaterialID";
            if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
            {
                //query = @"if exists(select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and GrnNo=@GrnNo and  MaterialID <> @MaterialID)
                //begin
                //  select 'exists' as result
                //end
                //if exists(select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and GrnNo=@GrnNo and  MaterialID = @MaterialID)
                //begin
                //  select 'exists' as result
                //end";
                //query = @"select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and GrnNo=@GrnNo and  MaterialID <> @MaterialID";
                query = @"if exists(select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and GrnNo=@GrnNo and  MaterialID = @MaterialID)
                begin
                  select 'exists' as result
                end";
            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@GrnNo", grnNo);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsUnique = rdr.HasRows ? false : true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsUnique;
        }

        internal static string GetComponentIDNumber(string productionOrder, string fabricationNo)
        {
            string IsUnique = "";
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select distinct Top 1 MaterialID from scheduledetails_gea where ProductionOrder=@productionOrder and FabricationNo=@FabNumber";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@productionOrder", productionOrder);
                cmd.Parameters.AddWithValue("@FabNumber", fabricationNo);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    IsUnique = rdr["MaterialID"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsUnique;
        }

        internal static bool IsProdMatIdOpnExists(string prodOrder, string materialId, string operationNo, string grnNo, string process)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and  MaterialID = @MaterialID and OperationNo = @OperationNo";
            if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
            {
                query = @"select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and  MaterialID = @MaterialID and OperationNo = @OperationNo   and GrnNo=@GrnNo";
            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", operationNo);
                cmd.Parameters.AddWithValue("@GrnNo", grnNo);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsExists = rdr.HasRows ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static bool IsMachineProdMatIdOpnExists(string machine, string prodOrder, string materialId, string opnNum, string grnNo, string process)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = "";
            if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
            {
                query = @"select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and  MaterialID = @MaterialID and OperationNo = @OperationNo  and GrnNo=@GrnNo";
            }
            else
            {
                query = @"select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and  MaterialID = @MaterialID and OperationNo = @OperationNo";
            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@GrnNo", grnNo);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsExists = rdr.HasRows ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static bool ChangeSchedulePriority(string Idd, int current_priority, int new_priority, string param, int current_User_priority, int new_User_priority)
        {
            bool success = false;
            string query = string.Empty;
            SqlConnection Conn = null;
            if (param.Equals("MoveBefore"))
            {
                query = @"update ScheduleDetails_GEA set SchedulePriority=SchedulePriority + 1 where idd in (select idd from ScheduleDetails_GEA where SchedulePriority >= @NewNo and IDD <> @IDD)
                  update ScheduleDetails_GEA set SchedulePriority = @NewNo, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD = @IDD";
                //query = @"update ScheduleDetails_GEA set SchedulePriority=SchedulePriority + 1 where idd in (select idd from ScheduleDetails_GEA where SchedulePriority >= @NewNo and IDD <> @IDD)
                //  update ScheduleDetails_GEA set SchedulePriority = @NewNo, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD = @IDD
                //      update ScheduleDetails_GEA set UserPriority=UserPriority + 1 where idd in (select idd from ScheduleDetails_GEA where UserPriority >= @NewUserNo and IDD <> @IDD)
                //  update ScheduleDetails_GEA set UserPriority = @NewUserNo, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD = @IDD";
            }
            else if (param.Equals("MoveToEnd"))
            {
                query = @"update ScheduleDetails_GEA set SchedulePriority = @NewNo, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where SchedulePriority = @currentNo";
                //update ScheduleDetails_GEA set UserPriority = @NewUserNo, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where UserPriority = @currentUserNo";
            }
            else
            {
                query = @"update ScheduleDetails_GEA set SchedulePriority=SchedulePriority + 1 where idd in (select idd from ScheduleDetails_GEA where SchedulePriority >= @currentNo and SchedulePriority < @NewNo) 
                  update ScheduleDetails_GEA set SchedulePriority=@currentNo, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where SchedulePriority=@NewNo";
                //update ScheduleDetails_GEA set UserPriority=UserPriority + 1 where idd in (select idd from ScheduleDetails_GEA
                //where UserPriority >= @currentUserNo and UserPriority < @NewUserNo) 
                //update ScheduleDetails_GEA set UserPriority=@currentNo, UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where UserPriority=@NewUserNo";
            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@IDD", Idd);
                cmd.Parameters.AddWithValue("@currentNo", current_priority);
                cmd.Parameters.AddWithValue("@NewNo", new_priority);
                cmd.Parameters.AddWithValue("@currentUserNo", current_User_priority);
                cmd.Parameters.AddWithValue("@NewUserNo", new_User_priority);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                success = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }
        internal static void UpdatePriorityChanged(string idd)
        {
            string query = string.Empty;
            SqlConnection Conn = null;
            query = @"update ScheduleDetails_GEA set PriorityChanged=@PriorityChanged Where IDD=@IDD";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@PriorityChanged", "Yes");
                cmd.Parameters.AddWithValue("@IDD", idd);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        internal static bool InsertQualityScheduleDetailsBefore(int priority, string machine, string prodOrder, string materialId, string opnNum, string qty, string GRNNumber, string SupplierName, bool NewProdOrder, string process)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = "";
            if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
            {
                query = @"if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo and GrnNo=@GrnNo)
              BEGIN
              update ScheduleDetails_GEA set SchedulePriority=SchedulePriority + 1 where idd in (select idd from ScheduleDetails_GEA where SchedulePriority >= @SchedulePriority)
              insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority,UserPriority, UpdatedBy, UpdatedTS,GrnNo,Supplier,NewProdDevelopment) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @SchedulePriority,@UserPriority, @UpdatedBy, @UpdatedTS,@GrnNo,@Supplier,@NewProdDevelopment)
              END";
            }
            else
            {
                query = @"if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo)
              BEGIN
              update ScheduleDetails_GEA set SchedulePriority=SchedulePriority + 1 where idd in (select idd from ScheduleDetails_GEA where SchedulePriority >= @SchedulePriority)
              insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority,UserPriority, UpdatedBy, UpdatedTS,GrnNo,Supplier,NewProdDevelopment) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @SchedulePriority,@UserPriority, @UpdatedBy, @UpdatedTS,@GrnNo,@Supplier,@NewProdDevelopment)
              END";
            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                cmd.Parameters.AddWithValue("@UserPriority", GetMaxUserPriority(machine));
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.Parameters.AddWithValue("@GrnNo", GRNNumber);
                cmd.Parameters.AddWithValue("@Supplier", SupplierName);
                cmd.Parameters.AddWithValue("@NewProdDevelopment", NewProdOrder);
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool InsertScheduleDetailsBefore(int priority, string machine, string prodOrder, string materialId, string opnNum, string qty, string GRNNumber, string SupplierName, bool NewProdOrder)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and MaterialID = @MaterialID and OperationNo = @OperationNo)
              BEGIN
              update ScheduleDetails_GEA set SchedulePriority=SchedulePriority + 1 where idd in (select idd from ScheduleDetails_GEA where SchedulePriority >= @SchedulePriority)
              insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority,UserPriority, UpdatedBy, UpdatedTS) values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status, @SchedulePriority,@UserPriority, @UpdatedBy, @UpdatedTS)
              END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@ScheduleQty", qty);
                cmd.Parameters.AddWithValue("@UserPriority", GetMaxUserPriority(machine));
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static int GetMaxSchedulePriority(out int UserPriority)
        {
            int max_schedule_priority = 0;
            UserPriority = 0;
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            string query = @"select max(SchedulePriority) as SchedulePriority,max(Userpriority) as UserPriority from ScheduleDetails_GEA";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        max_schedule_priority = Convert.ToInt32(dataReader["SchedulePriority"]);
                        UserPriority = Convert.ToInt32(dataReader["UserPriority"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (dataReader != null) dataReader.Close();
            }
            return max_schedule_priority;
        }

        internal static bool CalculateSchedulePlan(string calcMethod, string machine)
        {
            bool success = false;
            SqlConnection Conn = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"s_GenerateSchedules_GEA", Conn);
                cmd.Parameters.AddWithValue("@CalculatePlan", calcMethod);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool CancelProdSchedule(string idd, string machine, string prodOrder, string matId, string opnNum, string grnNumber, string process)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = "";
            if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
            {
                query = @"update ScheduleDetails_GEA set Status='Parked&Cancelled', UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD=@SCHID
 insert into ParkedScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, ScheduleStart, ScheduleEnd, ParkedBy, ParkedTS,
 ReasonForParking, Status, PendingQty, SchedulePriority,GrnNo) select Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, ScheduleStart, 
 ScheduleEnd, @ParkedBy, @ParkedTS, @ReasonForParking, 'Parked&Cancelled', PendingQty, SchedulePriority,GrnNo from ParkedScheduleDetails_GEA 
 where machineid=@MachineID and ProductionOrder=@ProdOrder and MaterialID=@MaterialID and OperationNo=@OperationNum  and GrnNo=@GrnNo";
            }
            else
            {
                query = @"update ScheduleDetails_GEA set Status='Parked&Cancelled', UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD=@SCHID
                           insert into ParkedScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, ScheduleStart, ScheduleEnd, ParkedBy, ParkedTS, ReasonForParking, Status, PendingQty, SchedulePriority) select Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, ScheduleStart, ScheduleEnd, @ParkedBy, @ParkedTS, @ReasonForParking, 'Parked&Cancelled', PendingQty, SchedulePriority from ParkedScheduleDetails_GEA where machineid=@MachineID and ProductionOrder=@ProdOrder and MaterialID=@MaterialID and OperationNo=@OperationNum";
            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@SCHID", idd);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@ProdOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", matId);
                cmd.Parameters.AddWithValue("@OperationNum", opnNum);
                cmd.Parameters.AddWithValue("@ParkedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@ParkedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ReasonForParking", "");
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool UpdateScheduleCalcDetails(string machine, string calcMethod, string startDate)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"insert into ScheduleCalculateMethod_GEA(Machineid, CalculatePlan, UpdatedBy, UpdatedTS, Status, UserInputDT) values(@Machineid, @CalculatePlan, @UpdatedBy, @UpdatedTS, @Status, @UserInputDT)";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@CalculatePlan", calcMethod);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Status", 0);
                if (!string.IsNullOrEmpty(startDate))
                    cmd.Parameters.AddWithValue("@UserInputDT", startDate);
                else
                    cmd.Parameters.AddWithValue("@UserInputDT", System.DBNull.Value);
                cmd.CommandType = CommandType.Text;
                success = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool UpdateDefaultCalculationMethod(string defaultCalcMethod, int defaultCalcMethodValue)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"if not exists(select * from ShopDefaults where Parameter = 'Scheduler_CalculatePlan_GEA') BEGIN insert into ShopDefaults(Parameter, ValueInText, ValueInText2, ValueInInt) values('Scheduler_CalculatePlan_GEA', @ValueInText, @ValueInText2, @ValueInInt) END ELSE BEGIN update ShopDefaults set ValueInText = @ValueInText, ValueInInt = @ValueInInt where Parameter = 'Scheduler_CalculatePlan_GEA' END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@ValueInText", defaultCalcMethod);
                cmd.Parameters.AddWithValue("@ValueInText2", "");
                cmd.Parameters.AddWithValue("@ValueInInt", defaultCalcMethodValue);
                cmd.CommandType = CommandType.Text;
                success = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool IsScheduleExists(string machine, string prodOrder, string matId, string opnNum)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and  MaterialID = @MaterialID and OperationNo = @OperationNo";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", matId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsExists = rdr.HasRows ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static List<string> GetInsPlanNumbers(string componentID, string OperationNo)
        {
            List<string> lstInsPlanNumbers = new List<string>();
            SqlConnection Conn = null;
            SqlDataReader sqlDataReader = null;
            string query = @"select distinct PlanNoAndRevNo from SPC_Characteristic where ComponentID =@ComponentID and OperationNo =@OperationNo order by PlanNoAndRevNo desc ";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@ComponentID", componentID);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNo);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        lstInsPlanNumbers.Add(sqlDataReader["PlanNoAndRevNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return lstInsPlanNumbers;
        }
        internal static List<string> GetInsPlanNumbers(string Type)
        {
            List<string> lstInsPlanNumbers = new List<string>();
            SqlConnection Conn = null;
            SqlDataReader sqlDataReader = null;
            string query = @"select * from SPC_Characteristic where ComponentID =@ComponentID and OperationNo =@OperationNo order by PlanNoAndRevNo desc ";
            switch (Type)
            {
                case "Quality":
                    query = "select distinct PlanNoAndRevNo from QualityIncomingMaster_GEA";
                    break;
                case "":
                    query = @"select distinct PlanNoAndRevNo from SPC_Characteristic";
                    break;

            }
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        lstInsPlanNumbers.Add(sqlDataReader["PlanNoAndRevNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return lstInsPlanNumbers;
        }
        internal static List<CompOpnEntity> getCompOpnForInspection()
        {
            List<CompOpnEntity> list = new List<CompOpnEntity>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = @"select distinct COP.componentid,COP.operationno from componentoperationpricing COP
inner join machineinformation MI on MI.machineid = COP.machineid 
where MI.Nonmachining is null  order by operationno ";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CompOpnEntity data = new CompOpnEntity();
                    data.Component = rdr["componentid"].ToString();
                    data.Operation = rdr["operationno"].ToString();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<InspectionMasterEntity> GetInspectionMasterData(string compID, string opnID, string insPlan)
        {
            List<InspectionMasterEntity> inspectionMasterDetails = new List<InspectionMasterEntity>();
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select * from SPC_Characteristic where ComponentID = @ComponentID and OperationNo = @OperationNo and PlanNoAndRevNo = @InsPlanNumber", Conn);
                cmd.Parameters.AddWithValue("@ComponentID", compID);
                cmd.Parameters.AddWithValue("@OperationNo", opnID);
                cmd.Parameters.AddWithValue("@InsPlanNumber", insPlan);
                cmd.CommandType = CommandType.Text;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        InspectionMasterEntity inspectionMasterData = new InspectionMasterEntity
                        {
                            ID = Convert.ToInt32(dataReader["ID"]),
                            InsParamID = dataReader["CharacteristicID"].ToString(),
                            InsParameter = dataReader["CharacteristicCode"].ToString(),
                            LSL = dataReader["LSL"].ToString(),
                            USL = dataReader["USL"].ToString(),
                            UOM = dataReader["UOM"].ToString(),
                            DataTemplate = dataReader["Datatype"].ToString(),
                            InsPlanNumber = dataReader["PlanNoAndRevNo"].ToString(),
                            AppliesToOperator = dataReader["AppliesToOpr"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            MandatoryForOperator = dataReader["IsMandatoryForOpr"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            AppliesToQuality = dataReader["AppliesToQuality"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            MandatoryForQuality = dataReader["IsMandatoryForQuality"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            IsEnabled = dataReader["IsEnabled"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false
                        };
                        inspectionMasterDetails.Add(inspectionMasterData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return inspectionMasterDetails;
        }

        internal static bool UpdateInsMasterData(string machineID, string compID, string opnNum, string insPlanNumber, string insParamID, string insParamName, string lsl, string usl, string unit, string dataTemplate, string appliesToOpr, string mandatoryForOpr, string appliesToQlty, string mandatoryForQlty, string isEnabled)
        {
            bool IsUpdated = false;
            SqlConnection Conn = null;
            string query = @"if exists(select * from componentoperationpricing where componentid = @ComponentID and operationno = @OperationNo) 
                BEGIN 
                if not exists(select * from SPC_Characteristic where ComponentID = @ComponentID and OperationNo = @OperationNo and PlanNoAndRevNo = @InsPlanNumber and CharacteristicID = @InsParamID)
                BEGIN
                insert into SPC_Characteristic(MachineID, ComponentID, OperationNo, PlanNoAndRevNo, CharacteristicID, CharacteristicCode, LSL, USL, UOM, Datatype, IsEnabled, AppliesToOpr, AppliesToQuality, IsMandatoryForOpr, IsMandatoryForQuality) values(@MachineID, @ComponentID, @OperationNo, @InsPlanNumber, @InsParamID,  @InsParamName, @LSL, @USL, @UOM, @Datatype, @IsEnabled, @AppliesToOpr, @AppliesToQuality, @IsMandatoryForOpr, @IsMandatoryForQuality)
                END
                ELSE
                BEGIN
                Update SPC_Characteristic set CharacteristicID = @InsParamID, LSL = @LSL, USL = @USL, UOM = @UOM, Datatype = @Datatype, IsEnabled = @IsEnabled, AppliesToOpr = @AppliesToOpr, AppliesToQuality = @AppliesToQuality, IsMandatoryForOpr = @IsMandatoryForOpr, IsMandatoryForQuality = @IsMandatoryForQuality where ComponentID = @ComponentID and OperationNo = @OperationNo and PlanNoAndRevNo = @InsPlanNumber and CharacteristicID = @InsParamID
                END
                END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand sqlCommand = new SqlCommand(query, Conn);
                sqlCommand.Parameters.AddWithValue("@MachineID", machineID);
                sqlCommand.Parameters.AddWithValue("@ComponentID", compID);
                sqlCommand.Parameters.AddWithValue("@OperationNo", opnNum);
                sqlCommand.Parameters.AddWithValue("@InsPlanNumber", insPlanNumber);
                sqlCommand.Parameters.AddWithValue("@InsParamID", insParamID);
                sqlCommand.Parameters.AddWithValue("@InsParamName", insParamName);
                sqlCommand.Parameters.AddWithValue("@LSL", lsl);
                sqlCommand.Parameters.AddWithValue("@USL", usl);
                sqlCommand.Parameters.AddWithValue("@UOM", unit);
                sqlCommand.Parameters.AddWithValue("@Datatype", dataTemplate);
                sqlCommand.Parameters.AddWithValue("@IsEnabled", !string.IsNullOrEmpty(isEnabled) ? (isEnabled.Equals("false", StringComparison.OrdinalIgnoreCase) ? "False" : "True") : "True");
                sqlCommand.Parameters.AddWithValue("@AppliesToOpr", !string.IsNullOrEmpty(appliesToOpr) ? (appliesToOpr.Equals("true", StringComparison.OrdinalIgnoreCase) ? "True" : "False") : "False");
                sqlCommand.Parameters.AddWithValue("@AppliesToQuality", !string.IsNullOrEmpty(appliesToQlty) ? (appliesToQlty.Equals("true", StringComparison.OrdinalIgnoreCase) ? "True" : "False") : "False");
                sqlCommand.Parameters.AddWithValue("@IsMandatoryForOpr", !string.IsNullOrEmpty(mandatoryForOpr) ? (mandatoryForOpr.Equals("true", StringComparison.OrdinalIgnoreCase) ? "True" : "False") : "False");
                sqlCommand.Parameters.AddWithValue("@IsMandatoryForQuality", !string.IsNullOrEmpty(mandatoryForQlty) ? (mandatoryForQlty.Equals("true", StringComparison.OrdinalIgnoreCase) ? "True" : "False") : "False");
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
                IsUpdated = true;
            }
            catch (Exception ex)
            {
                IsUpdated = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsUpdated;
        }

        internal static bool IsCompOpnExists(string compID, string opnNum)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select * from componentoperationpricing where componentid = @componentid and operationno = @operationno", Conn);
                cmd.Parameters.AddWithValue("@componentid", compID);
                cmd.Parameters.AddWithValue("@operationno", opnNum);
                cmd.CommandType = CommandType.Text;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                    IsExists = true;
                else
                    IsExists = false;
            }
            catch (Exception ex)
            {
                IsExists = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static bool IsMachineCompOpnExists(string machine, string compID, string opnNum)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select * from componentoperationpricing where machineid = @MachineID and componentid = @componentid and operationno = @operationno", Conn);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@componentid", compID);
                cmd.Parameters.AddWithValue("@operationno", opnNum);
                cmd.CommandType = CommandType.Text;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                    IsExists = true;
                else
                    IsExists = false;
            }
            catch (Exception ex)
            {
                IsExists = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static bool IsInsMasterKeyColumnExists(string compID, string opnNum, string insPlanNumber, string insParamID)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select * from SPC_Characteristic where ComponentID = @ComponentID and OperationNo = @OperationNo and PlanNoAndRevNo = @InsPlanNumber and CharacteristicID = @InsParamID", Conn);
                cmd.Parameters.AddWithValue("@ComponentID", compID);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@InsPlanNumber", insPlanNumber);
                cmd.Parameters.AddWithValue("@InsParamID", insParamID);
                cmd.CommandType = CommandType.Text;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                    IsExists = true;
                else
                    IsExists = false;
            }
            catch (Exception ex)
            {
                IsExists = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static bool DeleteInspectionMasterData(string idd)
        {
            bool IsDeleted = false;
            SqlConnection conn = null;
            string query = @"delete from SPC_Characteristic where ID = @ID";
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", idd);
                cmd.CommandType = CommandType.Text;
                IsDeleted = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                IsDeleted = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return IsDeleted;
        }

        internal static bool ValidateNewSchedule(string MachineID, string ComponentID, string OperationNumber)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            bool ValidateNewSchedule = false;
            try
            {
                cmd = new SqlCommand(@"SELECT * FROM componentoperationpricing COP 
                            INNER JOIN componentinformation CI ON CI.componentid = COP.componentid
                            INNER JOIN machineinformation MI ON MI.machineid = COP.machineid
                            WHERE COP.machineid = @MachineID AND COP.componentid = @ComponentID AND COP.operationno = @OperationNo", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;

                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ComponentID", ComponentID);
                cmd.Parameters.AddWithValue("@OperationNo", OperationNumber);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    ValidateNewSchedule = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ValidateNewSchedule= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return ValidateNewSchedule;
        }

        internal static int ShuffleScheduleMachine(string IDD, string MachineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int UpdatedRows = 0;
            try
            {
                cmd = new SqlCommand("UPDATE ScheduleDetails_GEA SET Machineid=@MachineID WHERE IDD=@IDD", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@IDD", IDD);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);

                UpdatedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ShuffleScheduleMachine= " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return UpdatedRows;
        }

        internal static string InsertDataToAssemblySchedules(string MachineID, string ProdOrder, string FabNo, string CompID, string OperNo)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string result = "";
            try
            {
                cmd = new SqlCommand(@"INSERT INTO AssemblyActivitySchedules_GEA(Machineid,ProductionOrder,FabricationNo,MaterialID,OperationNo,Activity,UpdatedBy,UpdatedTS)	                                    SELECT DISTINCT Station,@ProductionOrder,@FabNo,@Componentid,@OperationNo,Activity,@UpdatedBy,@UpdatedTS FROM AssemblyActivityMaster_GEA A1	                                    WHERE Station=@MachineID AND Componentid=@ComponentId AND IsDefault=1 AND NOT EXISTS(SELECT * FROM AssemblyActivitySchedules_GEA A2 WHERE	                                    A1.Station=A2.Machineid AND A1.Componentid=A2.MaterialID AND A1.Activity=A2.Activity AND A2.ProductionOrder=@ProductionOrder AND A2.FabricationNo=@FabNo)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@ProductionOrder", ProdOrder);
                cmd.Parameters.AddWithValue("@FabNo", FabNo);
                cmd.Parameters.AddWithValue("@Componentid", CompID);
                cmd.Parameters.AddWithValue("@OperationNo", OperNo);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.ExecuteNonQuery();
                result = "Inserted";
                
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("InsertDataToAssemblySchedules= " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }

        #region "Self Inspection Report"
        internal static List<string> GetProductionNumber(string ProdType, string CompSearch)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            string query = @"select distinct ProductionOrderNo from InspectionTransaction_GEA where (ProductionOrderNo=@ProductionOrderNo or isnull(@ProductionOrderNo,'')='') and (MaterialID=@MaterialID or isnull(@MaterialID,'')='')";
            switch (string.IsNullOrEmpty(ProdType))
            {
                case true:
                    {
                        switch (string.IsNullOrEmpty(CompSearch))
                        {
                            case true:
                                {
                                    query = @"select distinct ProductionOrderNo from InspectionTransaction_GEA ";
                                    break;
                                }
                            case false:
                                {
                                    query = @"select distinct ProductionOrderNo from InspectionTransaction_GEA where MaterialID Like '%" + CompSearch + "%'";
                                    break;
                                }
                        }
                        break;
                    }
                case false:
                    {
                        switch (string.IsNullOrEmpty(CompSearch))
                        {
                            case true:
                                {
                                    query = @"select distinct ProductionOrderNo from InspectionTransaction_GEA where ProductionOrderNo Like '%" + ProdType + "%'";
                                    break;
                                }
                            case false:
                                {
                                    query = @"select distinct ProductionOrderNo from InspectionTransaction_GEA where ProductionOrderNo Like '%" + ProdType + "%' and MaterialID Like '%" + CompSearch + "%'";
                                    break;
                                }
                        }
                        break;
                    }
            }


            SqlDataReader sdr = null;
            List<string> ponoList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                //cmd.Parameters.AddWithValue("@ProductionOrderNo", ProdType);
                //cmd.Parameters.AddWithValue("@MaterialID", CompSearch);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ponoList.Add(sdr["ProductionOrderNo"].ToString());
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
            return ponoList;
        }

        public static List<string> GetAllOperationNoByPONo(string PONumber)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> listOerationNo = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("select distinct OperationNo from InspectionTransaction_GEA where ProductionOrderNo = @PONumber ", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PONumber", PONumber);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listOerationNo.Add(rdr["OperationNo"].ToString());
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
            return listOerationNo;
        }

        internal static List<SelfInspectionDetails> GetSelfInpectionReportData(string PONumber, string OperationNumber, out List<SelfInspectionDetails> descriptionList)

        {
            SqlConnection Conn = null;
            DataTable dtDailyChklistReportData = new DataTable();
            SqlDataReader sdr = null;
            descriptionList = new List<SelfInspectionDetails>();
            List<SelfInspectionDetails> gridList = new List<SelfInspectionDetails>();
            SelfInspectionDetails descriptionDetails = null;
            SelfInspectionDetails gridDetails = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand command = new SqlCommand(@"S_ViewInspectionReport_GEA", Conn);
                command.Parameters.AddWithValue("@Param", "");
                command.Parameters.AddWithValue("@ProdOrderNo", PONumber);
                command.Parameters.AddWithValue("@OpnNo", OperationNumber);
                command.CommandType = CommandType.StoredProcedure;
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        descriptionDetails = new SelfInspectionDetails();
                        descriptionDetails.PartNumber = sdr["MaterialID"].ToString();
                        descriptionDetails.Description = sdr["description"].ToString();
                        descriptionDetails.ProductionOrder = sdr["ProductionOrderNo"].ToString();
                        descriptionDetails.MachineID = sdr["MachineID"].ToString();
                        descriptionDetails.PlanNumber = sdr["PlanAndRevNo"].ToString();
                        descriptionDetails.DrawingNumber = sdr["InspectionDrawing"].ToString();
                        descriptionDetails.SerailNumber = sdr["SerialNo"].ToString();
                        descriptionDetails.OperationNumber = sdr["OperationNo"].ToString();
                        descriptionList.Add(descriptionDetails);
                    }
                    sdr.NextResult();
                    while (sdr.Read())
                    {
                        gridDetails = new SelfInspectionDetails();
                        gridDetails.RowNumber = sdr["Rownumber"].ToString();
                        gridDetails.Parameter = sdr["Parameter"].ToString();
                        gridDetails.OperatorMeasurement = sdr["OperatorMeasurement"].ToString();
                        gridDetails.QualityMeasurement = sdr["QualityMeasurement"].ToString();
                        gridDetails.OperatorName = sdr["OperatorName"].ToString();
                        gridDetails.DateorShift = sdr["SDate"].ToString();
                        gridDetails.Remarks = sdr["Remarks"].ToString();
                        gridList.Add(gridDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (sdr != null) sdr.Close();
            }
            return gridList;
        }
        #endregion

        #region "Quality Incoming Master"
        internal static List<CompOpnEntity> getCompOpnForQuality()
        {
            List<CompOpnEntity> list = new List<CompOpnEntity>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            try
            {
                string sqlQuery = "select distinct componentid,operationno from componentoperationpricing	 where machineid in ('Quality Incoming','Quality In house')";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CompOpnEntity data = new CompOpnEntity();
                    data.Component = rdr["componentid"].ToString();
                    data.Operation = rdr["operationno"].ToString();
                    list.Add(data);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
                if (rdr != null) rdr.Close();
            }
            return list;
        }
        internal static List<string> GetInsPlanNumbersQuality(string CompID)
        {
            List<string> lstInsPlanNumbersQuality = new List<string>();
            SqlConnection Conn = null;
            SqlDataReader sqlDataReader = null;
            string query = @"select distinct PlanNoAndRevNo from QualityIncomingMaster_GEA where ComponentID=@CompID order by PlanNoAndRevNo desc";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@CompID", CompID);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        lstInsPlanNumbersQuality.Add(sqlDataReader["PlanNoAndRevNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return lstInsPlanNumbersQuality;
        }

        internal static List<QualityIncomingMasterEntity> GetQualityIncomingMasterData(string compID, string opnID, string insPlan)
        {
            List<QualityIncomingMasterEntity> qualityMasterDetails = new List<QualityIncomingMasterEntity>();
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select * from QualityIncomingMaster_GEA where ComponentID = @ComponentID and OperationNo = @OperationNo and PlanNoAndRevNo = @InsPlanNumber  order by SortOrder", Conn);
                cmd.Parameters.AddWithValue("@ComponentID", compID);
                cmd.Parameters.AddWithValue("@OperationNo", opnID);
                cmd.Parameters.AddWithValue("@InsPlanNumber", insPlan);
                cmd.CommandType = CommandType.Text;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        QualityIncomingMasterEntity qualityMasterData = new QualityIncomingMasterEntity
                        {
                            ID = Convert.ToInt32(dataReader["IDD"]),
                            CharacteristicID = dataReader["CharacteristicID"].ToString(),
                            CharacteristicCode = dataReader["CharacteristicCode"].ToString(),
                            InsPlanNumber = dataReader["PlanNoAndRevNo"].ToString(),
                            LSL = dataReader["LSL"].ToString(),
                            USL = dataReader["USL"].ToString(),
                            SetValue = dataReader["Value"].ToString(),
                            UOM = dataReader["UOM"].ToString(),
                            DataType = dataReader["Datatype"].ToString(),
                            IsMandatory = dataReader["IsMandatory"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            IsEnabled = dataReader["IsEnabled"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false,
                            Sequence = dataReader["SortOrder"].ToString(),
                        };
                        qualityMasterDetails.Add(qualityMasterData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return qualityMasterDetails;
        }

        internal static bool IsQualityKeyColumnExists(string compID, string opnNum, string insPlanNumber, string insParamID)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select * from QualityIncomingMaster_GEA where ComponentID = @ComponentID and OperationNo = @OperationNo and PlanNoAndRevNo = @InsPlanNumber and CharacteristicID = @InsParamID", Conn);
                cmd.Parameters.AddWithValue("@ComponentID", compID);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@InsPlanNumber", insPlanNumber);
                cmd.Parameters.AddWithValue("@InsParamID", insParamID);
                cmd.CommandType = CommandType.Text;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                    IsExists = true;
                else
                    IsExists = false;
            }
            catch (Exception ex)
            {
                IsExists = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static bool UpdateQualityMasterData(string compID, string opnNum, string insParamID, string insParamName, string insPlanNumber, string lsl, string usl, string setValue, string unit, string dataType, string isMandatory, string isEnabled, string sequence)
        {
            bool IsUpdated = false;
            SqlConnection Conn = null;
            string query = @"if exists(select * from componentoperationpricing where componentid = @ComponentID and operationno = @OperationNo) 
                BEGIN 
                if not exists(select * from QualityIncomingMaster_GEA where ComponentID = @ComponentID and OperationNo = @OperationNo and CharacteristicID = @InsParamID and PlanNoAndRevNo = @InsPlanNumber)
                BEGIN
                insert into QualityIncomingMaster_GEA(ComponentID, OperationNo, CharacteristicID, CharacteristicCode, PlanNoAndRevNo, LSL, USL, Value, UOM, DataType, IsMandatory, IsEnabled,SortOrder) values(@ComponentID, @OperationNo, @InsParamID, @InsParamName, @InsPlanNumber, @LSL, @USL, @SetValue, @UOM, @Datatype, @IsMandatory, @IsEnabled,@SortOrder)
                END
                ELSE
                BEGIN
                Update QualityIncomingMaster_GEA set CharacteristicCode = @InsParamName, LSL = @LSL, USL = @USL, Value = @SetValue, UOM = @UOM, DataType = @Datatype, IsMandatory = @IsMandatory, IsEnabled = @IsEnabled, SortOrder=@SortOrder where ComponentID = @ComponentID and OperationNo = @OperationNo and CharacteristicID = @InsParamID and PlanNoAndRevNo = @InsPlanNumber
                END
                END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand sqlCommand = new SqlCommand(query, Conn);
                sqlCommand.Parameters.AddWithValue("@ComponentID", compID);
                sqlCommand.Parameters.AddWithValue("@OperationNo", opnNum);
                sqlCommand.Parameters.AddWithValue("@InsPlanNumber", insPlanNumber);
                sqlCommand.Parameters.AddWithValue("@InsParamID", insParamID);
                sqlCommand.Parameters.AddWithValue("@InsParamName", insParamName);
                sqlCommand.Parameters.AddWithValue("@LSL", lsl);
                sqlCommand.Parameters.AddWithValue("@USL", usl);
                sqlCommand.Parameters.AddWithValue("@SetValue", setValue);
                sqlCommand.Parameters.AddWithValue("@UOM", unit);
                sqlCommand.Parameters.AddWithValue("@Datatype", dataType);
                sqlCommand.Parameters.AddWithValue("@IsMandatory", !string.IsNullOrEmpty(isMandatory) ? (isMandatory.Equals("true", StringComparison.OrdinalIgnoreCase) ? "True" : "False") : "False");
                sqlCommand.Parameters.AddWithValue("@IsEnabled", !string.IsNullOrEmpty(isEnabled) ? (isEnabled.Equals("false", StringComparison.OrdinalIgnoreCase) ? "False" : "True") : "True");
                sqlCommand.Parameters.AddWithValue("@SortOrder", string.IsNullOrEmpty(sequence) ? 0 : Convert.ToInt32(sequence));
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
                IsUpdated = true;
            }
            catch (Exception ex)
            {
                IsUpdated = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsUpdated;
        }

        internal static bool DeleteQualityMasterData(string idd)
        {
            bool IsDeleted = false;
            SqlConnection conn = null;
            string query = @"delete from QualityIncomingMaster_GEA where IDD = @ID";
            try
            {
                conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", idd);
                cmd.CommandType = CommandType.Text;
                IsDeleted = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                IsDeleted = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return IsDeleted;
        }

        internal static List<QltyIncomingTransactionEntity> GetQltyIncomingReportData(string machine_id, string prod_order_num, string comp_id, string operation_num, string ins_plan_number)
        {
            List<QltyIncomingTransactionEntity> qltyIncomingReportDataList = new List<QltyIncomingTransactionEntity>();
            SqlConnection sqlConn = null;
            SqlDataReader rdr = null;
            try
            {
                sqlConn = ConnectionManager.GetConnection();
                SqlCommand sqlCommand = new SqlCommand(@"S_ViewQualityInspectionDetails_GEA", sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Param", "View");
                sqlCommand.Parameters.AddWithValue("@ProdOrderNo", prod_order_num);
                sqlCommand.Parameters.AddWithValue("@PartID", comp_id);
                sqlCommand.Parameters.AddWithValue("@OpnNo", operation_num);
                sqlCommand.Parameters.AddWithValue("@MachineID", machine_id);
                sqlCommand.Parameters.AddWithValue("@PlanAndRevNo", ins_plan_number);
                rdr = sqlCommand.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        QltyIncomingTransactionEntity qltyIncomingTransactionData = new QltyIncomingTransactionEntity
                        {
                            ID = Convert.ToInt32(rdr["Slno"].ToString()),
                            CharacteristicSlNo = rdr["CharacteristicSlNo"].ToString(),
                            CharacteristicCode = rdr["CharacteristicCode"].ToString(),
                            LSL = rdr["LSL"].ToString(),
                            USL = rdr["USL"].ToString(),
                            SetValue = rdr["SetValue"].ToString(),
                            UOM = rdr["UOM"].ToString(),
                            DataType = rdr["Datatype"].ToString(),
                            InspectionValueOneA = rdr["1"].ToString(),
                            InspectionValueOneB = rdr["1A"].ToString(),
                            InspectionValueTwoA = rdr["2"].ToString(),
                            InspectionValueTwoB = rdr["2A"].ToString(),
                            InspectionValueThreeA = rdr["3"].ToString(),
                            InspectionValueThreeB = rdr["3A"].ToString(),
                            InspectionValueFourA = rdr["4"].ToString(),
                            InspectionValueFourB = rdr["4A"].ToString(),
                            InspectionValueFiveA = rdr["5"].ToString(),
                            InspectionValueFiveB = rdr["5A"].ToString(),
                            InspectedBy = rdr["InspectedBy"].ToString(),
                            Comments = rdr["Remarks"].ToString(),
                            InspectedTS = rdr["InspectedTS"].ToString(),
                            IsMandatory = Convert.ToBoolean(rdr["IsMandatory"].ToString())
                        };
                        qltyIncomingReportDataList.Add(qltyIncomingTransactionData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return qltyIncomingReportDataList;
        }
        #endregion

        #region "Quality Incoming Report"
        internal static List<string> GetAllQualityTransactionMachines()
        {
            List<string> lstMachineID = new List<string>();
            SqlConnection Conn = null;
            SqlDataReader sqlDataReader = null;
            string query = @"select distinct MachineID from QualityTransaction_GEA";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        lstMachineID.Add(sqlDataReader["MachineID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Close();
                if (Conn != null) Conn.Close();
            }
            return lstMachineID;
        }

        internal static List<string> GetAllQualityTransactionProdOrders()
        {
            SqlConnection sqlConn = null;
            SqlDataReader sdr = null;
            List<string> ponoList = new List<string>();
            try
            {
                sqlConn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select distinct ProductionOrderNo from QualityTransaction_GEA", sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ponoList.Add(sdr["ProductionOrderNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return ponoList;
        }

        internal static List<string> GetAllQualityTransactionMaterialIds()
        {
            SqlConnection sqlConn = null;
            SqlDataReader sdr = null;
            List<string> materialIdsList = new List<string>();
            try
            {
                sqlConn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"select distinct MaterialID from QualityTransaction_GEA", sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        materialIdsList.Add(sdr["MaterialID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return materialIdsList;
        }

        internal static List<string> GetAllQualityTransactionOpnForMaterialID(string materialID)
        {
            SqlConnection sqlConn = null;
            SqlDataReader sdr = null;
            List<string> opnNosList = new List<string>();
            string query = @"select distinct OperationNo from QualityTransaction_GEA where MaterialID = IIF((@MaterialID is null) or (@MaterialID = ''), MaterialID, @MaterialID)";
            try
            {
                sqlConn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MaterialID", materialID);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        opnNosList.Add(sdr["OperationNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return opnNosList;
        }

        internal static List<string> GetAllQualityTransactionPlanNoForMaterialAndOpn(string materialID, string opnNo)
        {
            SqlConnection sqlConn = null;
            SqlDataReader sdr = null;
            List<string> insPlanNumberList = new List<string>();
            string query = @"select distinct PlanAndRevNo from QualityTransaction_GEA where MaterialID = IIF((@MaterialID is null) or (@MaterialID = ''), MaterialID, @MaterialID) and OperationNo = IIF((@OperationNo is null) or (@OperationNo = ''), OperationNo, @OperationNo)";
            try
            {
                sqlConn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MaterialID", materialID);
                cmd.Parameters.AddWithValue("@OperationNo", opnNo);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        insPlanNumberList.Add(sdr["PlanAndRevNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return insPlanNumberList;
        }
        #endregion

        internal static List<_8DHeader> Get8DReport(string machineID, string productionOrder, string grnNumber, out string Issuer, out string ReportNo, out DateTime IssueDate, out string Field, out string ProblemOriginatedat)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            IssueDate = DateTime.Now; ProblemOriginatedat = "";
            Field = ""; Issuer = ""; ReportNo = "";
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<_8DHeader> _8DList = new List<_8DHeader>();
            try
            {
                cmd = new SqlCommand("[S_Quality8Dreport_GEA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PONo", productionOrder);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);
                rdr = cmd.ExecuteReader();
                _8DHeader Entity = new _8DHeader();
                _8DreportGridCol EntityCol = new _8DreportGridCol();
                List<_8DreportGridCol> GridEntityHeader = new List<_8DreportGridCol>();
                string HeaderText = ""; int slno = 1;
                while (rdr.Read())
                {
                    if (!(rdr["UpdatedTs"].ToString().Equals(DBNull.Value) || string.IsNullOrEmpty(rdr["UpdatedTs"].ToString())))
                        IssueDate = Convert.ToDateTime(rdr["UpdatedTs"].ToString());
                    Field = (rdr["Field"].ToString());
                    Issuer = (rdr["Issuer"].ToString());
                    //ReportNo = (rdr["Report"].ToString());
                    ProblemOriginatedat = (rdr["ProblemOriginatedAt"].ToString());
                }
                rdr.NextResult();
                while (rdr.Read())
                {
                    if (!HeaderText.Equals(rdr["Header"].ToString()))
                    {
                        _8DList.Add(Entity);
                        Entity = new _8DHeader();
                        Entity.GridData = new List<_8DreportGridCol>();
                        Entity.headerID = Convert.ToInt32(rdr["HeaderID"].ToString());
                        Entity.TemplateName = rdr["Header"].ToString();
                        HeaderText = Entity.TemplateName;
                        Entity.IsEnable = rdr["IsEnable"].ToString().Equals("1") ? true : false;
                        Entity.RichTextSubHeader = rdr["SubHeader"].ToString();

                        Entity.GridOrRichType = rdr["GridOrRichText"].ToString();
                        if (Entity.GridOrRichType.Equals("Text", StringComparison.OrdinalIgnoreCase))
                        {
                            Entity.GridOrRichText = rdr["TextValue"].ToString();
                        }
                        else
                        {
                            EntityCol = new _8DreportGridCol();
                            EntityCol.Measure = rdr["Item1"].ToString();
                            EntityCol.Responsible = rdr["Item2"].ToString();
                            EntityCol.Deadline = rdr["Item3"].ToString();
                            Entity.GridData.Add(EntityCol);
                        }
                    }
                    else
                    {
                        //Entity.SLNO = Convert.ToInt32(rdr["SubHeader"].ToString());
                        EntityCol = new _8DreportGridCol();
                        EntityCol.Measure = rdr["Item1"].ToString();
                        EntityCol.Responsible = rdr["Item2"].ToString();
                        EntityCol.Deadline = rdr["Item3"].ToString();
                        Entity.GridData.Add(EntityCol);
                    }
                }
                _8DList.Add(Entity);
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
            if (_8DList.Count > 0)
                _8DList.RemoveAt(0);
            _8DList[0].IsEnable = true;
            return _8DList;
        }

        internal static List<string> GetQualityMachines()
        {
            List<string> Machines = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select machineid from machineinformation where  Process='QualityIncoming' or Process='QualityInHouse'", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Machines.Add(rdr["machineid"].ToString());
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
            return Machines;
        }

        internal static List<string> Get8DproductionOrderList(string MachineID)
        {
            List<string> PONo = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct PONo from Quality_8DReportTransaction_GEA where MachineID=@MachineID", conn);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.CommandType = CommandType.Text;

                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(rdr["PONo"].ToString());
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
            return PONo;
        }

        internal static string GET8DReportNo(string machineID, string productionOrder)
        {
            string RepNo = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct ReportNo from Quality_8DReportTransaction_GEA where MachineID=@Machineid and PONo=@PONO ", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machineID);
                cmd.Parameters.AddWithValue("@PONO", productionOrder);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    RepNo = rdr["ReportNo"].ToString();
                    //break;
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
            return RepNo;
        }

        #region --- Assembly-------

        internal static bool CancelProdSchedule_Assembly(string idd, string machine, string prodOrder, string matId, string opnNum, string fabricationnum)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"update ScheduleDetails_GEA set Status='Parked&Cancelled', UpdatedBy = @UpdatedBy, UpdatedTS = @UpdatedTS where IDD=@SCHID
insert into ParkedScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, ScheduleStart, 
ScheduleEnd, ParkedBy, ParkedTS, ReasonForParking, Status, PendingQty, SchedulePriority, FabricationNo) 
select Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, ScheduleStart, ScheduleEnd, 
@ParkedBy, @ParkedTS, @ReasonForParking, 'Parked&Cancelled', PendingQty, SchedulePriority, FabricationNo from 
ParkedScheduleDetails_GEA where machineid=@MachineID and ProductionOrder=@ProdOrder and MaterialID=@MaterialID and OperationNo=@OperationNum and FabricationNo=@FabricationNo";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@SCHID", idd);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationnum);
                cmd.Parameters.AddWithValue("@ProdOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", matId);
                cmd.Parameters.AddWithValue("@OperationNum", opnNum);
                cmd.Parameters.AddWithValue("@ParkedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@ParkedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ReasonForParking", "");
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool IsAssemblyScheduleExists(string machine, string prodOrder, string matId, string opnNum, string fabricationNum)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder and  MaterialID = @MaterialID and OperationNo = @OperationNo and FabricationNo=@FabricationNo";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", matId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNum);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsExists = rdr.HasRows ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }
        internal static bool IsAutoScheduleMachineAssigned(string compID, string opnNo)
        {
            bool isAssigned = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            try
            {
                string query = @"select * from AssemblyScheduleDetails_GEA where Componentid=@Componentid and Operationno=@Operationno";
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Componentid", compID);
                cmd.Parameters.AddWithValue("@Operationno", opnNo);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                isAssigned = rdr.HasRows ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return isAssigned;
        }
        internal static bool IsProdMatIdOpnExists_Assembly(string prodOrder, string materialId, string operationNo)
        {
            bool IsExists = false;
            SqlConnection Conn = null;
            SqlDataReader rdr = null;
            string query = @"select * from ScheduleDetails_GEA where ProductionOrder = @ProductionOrder and  Model = @Model and OperationNo = @OperationNo";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@Model", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", operationNo);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                IsExists = rdr.HasRows ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return IsExists;
        }

        internal static void DeleteNewProductionSchedules_Assembly()
        {
            SqlConnection Conn = null;
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(@"delete from AssemblyScheduleDetails_GEA where Status='New'", Conn);
                cmd.CommandType = CommandType.Text;
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

        internal static bool ImportSchedueData_Assembly(string machine, string priority, string localorexport, string prodOrder, string saleorder, string opnNum, string materialid, string scrollweld, string rddmachine, string fabno, string customer, string location, out string errorMsg)
        {
            errorMsg = "";
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if not exists(select * from [ScheduleDetails_GEA] where Machineid = @Machineid and ProductionOrder = @ProductionOrder 
and MaterialID = @MaterialID and OperationNo = @OperationNo and FabricationNo=@FabricationNo ) 
BEGIN 
insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority,UserPriority, Localorexport,SaleOrder, ScrollWelded, RDDMachines, FabricationNo, Customer, Location ,UpdatedBy, UpdatedTS)
 values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status,@SchedulePriority,@UserPriority,@Localorexport,@SaleOrder,@ScrollWelded,@RDDMachines,@FabricationNo,@Customer,@Location,@UpdatedBy,@UpdatedTS) 
 END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialid);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@ScheduleQty", 1);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@SchedulePriority", GetMaxSchedulePriority(out int Userpriority) + 1);
                cmd.Parameters.AddWithValue("@UserPriority", priority);
                cmd.Parameters.AddWithValue("@Localorexport", localorexport);
                cmd.Parameters.AddWithValue("@SaleOrder", saleorder);
                cmd.Parameters.AddWithValue("@ScrollWelded", scrollweld);
                //cmd.Parameters.AddWithValue("@RDDMachines", Util.GetDateTime(rddmachine).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@RDDMachines", rddmachine);
                cmd.Parameters.AddWithValue("@FabricationNo", fabno);
                cmd.Parameters.AddWithValue("@Customer", customer);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                errorMsg = ex.Message;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }

        internal static bool UpdateScheduleDetails_Assembly(string priority, string machine, string prodOrder, string materialId, string opnNum, string qty, string localorexport, string saleorder, string scrollweld, string rddmachine, string fabno, string customer, string location)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if not exists(select * from ScheduleDetails_GEA where SchedulePriority = @SchedulePriority) 
BEGIN 
if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder 
and MaterialID = @MaterialID and OperationNo = @OperationNo and FabricationNo=@FabricationNo) 
BEGIN
 insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority, Localorexport,SaleOrder,
  ScrollWelded, RDDMachines, FabricationNo, Customer, Location ,UserPriority,UpdatedBy, UpdatedTS)
  values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status,@SchedulePriority,
  @Localorexport,@SaleOrder,@ScrollWelded,@RDDMachines,@FabricationNo,@Customer,@Location,@UserPriority,@UpdatedBy,@UpdatedTS) 
  END 
  END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);

                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@ScheduleQty", 1);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@Localorexport", localorexport);
                cmd.Parameters.AddWithValue("@SaleOrder", saleorder);
                cmd.Parameters.AddWithValue("@ScrollWelded", scrollweld);
                cmd.Parameters.AddWithValue("@RDDMachines", Util.GetDateTime(rddmachine).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@FabricationNo", fabno);
                cmd.Parameters.AddWithValue("@Customer", customer);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@UserPriority", GetMaxUserPriority(machine));
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }


        internal static bool InsertScheduleDetailsBefore_Assembly(int priority, string machine, string prodOrder, string materialId, string opnNum, string qty, string localorexport, string saleorder, string scrollweld, string rddmachine, string fabno, string customer, string location)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"if not exists(select * from ScheduleDetails_GEA where Machineid = @Machineid and ProductionOrder = @ProductionOrder 
and MaterialID = @MaterialID and OperationNo = @OperationNo and FabricationNo=@FabricationNo)
BEGIN
 update ScheduleDetails_GEA set SchedulePriority=SchedulePriority + 1 where idd in (select idd from ScheduleDetails_GEA where SchedulePriority >= @SchedulePriority)
insert into ScheduleDetails_GEA(Machineid, ProductionOrder, MaterialID, OperationNo, ScheduleQty, Status, SchedulePriority, Localorexport,SaleOrder,
  ScrollWelded, RDDMachines, FabricationNo, Customer, Location ,UserPriority,UpdatedBy, UpdatedTS)
  values(@Machineid, @ProductionOrder, @MaterialID, @OperationNo, @ScheduleQty, @Status,@SchedulePriority,
  @Localorexport,@SaleOrder,@ScrollWelded,@RDDMachines,@FabricationNo,@Customer,@Location,@UserPriority ,@UpdatedBy,@UpdatedTS) 
END";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@ScheduleQty", 1);
                cmd.Parameters.AddWithValue("@Status", "New");
                cmd.Parameters.AddWithValue("@SchedulePriority", priority);
                cmd.Parameters.AddWithValue("@Localorexport", localorexport);
                cmd.Parameters.AddWithValue("@SaleOrder", saleorder);
                cmd.Parameters.AddWithValue("@ScrollWelded", scrollweld);
                cmd.Parameters.AddWithValue("@RDDMachines", Util.GetDateTime(rddmachine).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@FabricationNo", fabno);
                cmd.Parameters.AddWithValue("@Customer", customer);
                cmd.Parameters.AddWithValue("@Location", location);

                cmd.Parameters.AddWithValue("@UserPriority", GetMaxUserPriority(machine));

                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool InsertSubActivities(string machine, string prodOrder, string materialId, string opnNum, string fabno, string activity)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = @"if not exists (select * from AssemblyActivitySchedules_GEA where Machineid=@Machineid and ProductionOrder=@ProductionOrder and
	FabricationNo=@FabricationNo and	MaterialID=@MaterialID	and OperationNo=@OperationNo and Activity=@Activity)
begin
insert into AssemblyActivitySchedules_GEA(Machineid,ProductionOrder,FabricationNo,MaterialID,OperationNo,Activity,UpdatedBy,UpdatedTS)
 values(@Machineid,@ProductionOrder,@FabricationNo,@MaterialID,@OperationNo,@Activity,@UpdatedBy,@UpdatedTS)
end";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@FabricationNo", fabno);
                cmd.Parameters.AddWithValue("@Activity", activity);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool InsertRemoveSubActivities(string machine, string prodOrder, string materialId, string opnNum, string fabno, string activity, string checkoruncheck)
        {
            bool success = false;
            SqlConnection Conn = null;
            string query = "";
            if (checkoruncheck == "Checked")
            {
                query = @"if not exists (select * from AssemblyActivitySchedules_GEA where Machineid=@Machineid and ProductionOrder=@ProductionOrder and
	FabricationNo=@FabricationNo and	MaterialID=@MaterialID	and OperationNo=@OperationNo and Activity=@Activity)
begin
insert into AssemblyActivitySchedules_GEA(Machineid,ProductionOrder,FabricationNo,MaterialID,OperationNo,Activity,UpdatedBy,UpdatedTS)
 values(@Machineid,@ProductionOrder,@FabricationNo,@MaterialID,@OperationNo,@Activity,@UpdatedBy,@UpdatedTS)
end";
            }
            else
            {
                query = @"if exists (select * from AssemblyActivitySchedules_GEA where Machineid=@Machineid and ProductionOrder=@ProductionOrder and
	FabricationNo=@FabricationNo and	MaterialID=@MaterialID	and OperationNo=@OperationNo and Activity=@Activity)
begin
delete from AssemblyActivitySchedules_GEA where Machineid=@Machineid and ProductionOrder=@ProductionOrder and
	FabricationNo=@FabricationNo and	MaterialID=@MaterialID	and OperationNo=@OperationNo and Activity=@Activity
end";
            }

            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                cmd.Parameters.AddWithValue("@OperationNo", opnNum);
                cmd.Parameters.AddWithValue("@FabricationNo", fabno);
                cmd.Parameters.AddWithValue("@Activity", activity);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }

        internal static bool InsertDefalultSubActivities_WhileImport(string machine, string compID, string PO)
        {
            bool success = false;
            SqlConnection Conn = null;
            //            string query = @" Insert into [dbo].[AssemblyActivitySchedules_GEA](
            //[Machineid],[ProductionOrder],[FabricationNo],[MaterialID],[OperationNo],[Activity],[UpdatedBy],[UpdatedTS])
            //Select distinct S.Machineid,S.ProductionOrder,S.FabricationNo,S.MaterialID,S.OperationNo,A.Activity,@UpdatedBy,@UpdatedTS from ScheduleDetails_GEA S
            //Cross join [dbo].[AssemblyActivityMaster_GEA] A
            //where S.Machineid=@Machineid and A.Station=S.Machineid and A.IsDefault=1
            //and not exists(Select * from [dbo].[AssemblyActivitySchedules_GEA] A1 where A1.Machineid=S.Machineid and S.MaterialID=A1.MaterialID
            //and S.OperationNo=A1.OperationNo and S.ProductionOrder=A1.ProductionOrder and S.FabricationNo=A1.FabricationNo and A.Activity=A1.Activity)";
            string query = @" Insert into [dbo].[AssemblyActivitySchedules_GEA](
[Machineid],[ProductionOrder],[FabricationNo],[MaterialID],[OperationNo],[Activity],[UpdatedBy],[UpdatedTS])
Select distinct S.Machineid,S.ProductionOrder,S.FabricationNo,a.Componentid,S.OperationNo,A.Activity,@UpdatedBy,@UpdatedTS from ScheduleDetails_GEA S
Cross join [dbo].[AssemblyActivityMaster_GEA] A
where S.Machineid=@Machineid and a.Componentid=@Componentid and s.ProductionOrder=@ProductionOrder  and A.Station=S.Machineid and A.IsDefault=1
and not exists(Select * from [dbo].[AssemblyActivitySchedules_GEA] A1 where A1.Machineid=S.Machineid and S.MaterialID=A1.MaterialID
and S.OperationNo=A1.OperationNo and S.ProductionOrder=A1.ProductionOrder and S.FabricationNo=A1.FabricationNo and A.Activity=A1.Activity)";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machine);
                cmd.Parameters.AddWithValue("@Componentid", compID);
                cmd.Parameters.AddWithValue("@ProductionOrder", PO);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return success;
        }
        internal static int GetMaxUserPriority(string MachineID)
        {
            int max_schedule_priority = 0;
            SqlConnection Conn = null;
            SqlDataReader dataReader = null;
            string query = @"select max(UserPriority) as UserPriority from ScheduleDetails_GEA where Machineid=@Machineid ";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", MachineID);
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        //if (dataReader["SchedulePriority"] == DBNull.Value || dataReader["SchedulePriority"] == null || )
                        //{
                        //    max_schedule_priority = 1;
                        //}
                        max_schedule_priority = Convert.ToInt32(dataReader["UserPriority"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
                if (dataReader != null) dataReader.Close();
            }
            return max_schedule_priority + 1;
        }
        internal static bool UpdateActivityData(string macId, string old_prdono, string new_prodno, string old_matid, string new_matid, string opnno, string fabricationno)
        {
            bool IsSaved = false;
            SqlConnection Conn = null;
            string query = @"if exists (select * from AssemblyActivitySchedules_GEA where Machineid=@Machineid and ProductionOrder=@OldProductionOrder and FabricationNo=@FabricationNo and MaterialID=@OldMaterialID and OperationNo=@OperationNo)
begin 
update AssemblyActivitySchedules_GEA
set ProductionOrder=@NewProductionOrder,  MaterialID=@NewMaterialID, UpdatedBy=@UpdatedBy, UpdatedTS=@UpdatedTS
where Machineid=@Machineid and ProductionOrder=@OldProductionOrder and FabricationNo=@FabricationNo and MaterialID=@OldMaterialID and OperationNo=@OperationNo
end";
            try
            {
                Conn = ConnectionManager.GetConnection();
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@Machineid", macId);
                cmd.Parameters.AddWithValue("@OldProductionOrder", old_prdono);
                cmd.Parameters.AddWithValue("@OldMaterialID", old_matid);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationno);
                cmd.Parameters.AddWithValue("@NewProductionOrder", new_prodno);
                cmd.Parameters.AddWithValue("@NewMaterialID", new_matid);
                cmd.Parameters.AddWithValue("@OperationNo", opnno);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"] != null ? HttpContext.Current.Session["UserName"].ToString() : "");
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                IsSaved = true;
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (Conn != null) Conn.Close();
            }
            return IsSaved;
        }
        #endregion

        #region ------ Machine Data Assembly and Electo-Tech Equipment Report ----
        internal static List<string> getMachineIDForNonMachiningData()
        {
            List<string> PONo = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select  distinct machineid from machineinformation where Process in ('Assembly','Testing','Packing','Stores')", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(rdr["machineid"].ToString());
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
            return PONo;
        }
        internal static string getPOMachineIDForAssemblyProcess(string productionOrder, string fabricationNumber, string param)
        {
            string machineID = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                string query = "";
                if (param == "machineAssembly")
                {
                    query = @"select top 1 t.Machineid from AssemblyActivityTransaction_GEA t join machineinformation m on m.machineid=t.Machineid
where ProductionOrder=@ProductionOrder and FabricationNo=@FabricationNo and m.Process='Assembly'";
                }
                else if (param == "electo")
                {
                    query = @"select top 1 Machineid from ElectrotechnicalTransaction_GEA where OrderNo=@ProductionOrder and FabricationNo=@FabricationNo";
                }
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductionOrder", productionOrder);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNumber);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    machineID = rdr["Machineid"].ToString();
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
            return machineID;
        }
        internal static List<ProdFabricationNumber> GetMachineDataAssemblyProductionOrderList(string machineID)
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct ProductionOrder,FabricationNo from AssemblyActivityTransaction_GEA where Machineid=@Machineid", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machineID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        ProdOrderNumber = rdr["ProductionOrder"].ToString(),
                        FabricationNumber = rdr["FabricationNo"].ToString()
                    });
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
            return PONo;
        }
        internal static List<ProdFabricationNumber> GetElectroTechProductionOrderList(string machineID)
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct OrderNo,FabricationNo from ElectrotechnicalTransaction_GEA  where MachineID=@MachineID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        ProdOrderNumber = rdr["OrderNo"].ToString(),
                        FabricationNumber = rdr["FabricationNo"].ToString()
                    });
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
            return PONo;
        }
        internal static List<ProdFabricationNumber> getProDecanterProductionOrderList()
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct ProductionOrder,FabricationNumber from ProDecanterDetails_Gea where  Process='Packing' and  ReportType='FinalControl'", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        ProdOrderNumber = rdr["ProductionOrder"].ToString(),
                        FabricationNumber = rdr["FabricationNumber"].ToString()
                    });
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
            return PONo;
        }
        internal static List<ProdFabricationNumber> GetPickingListProductionOrderList(string machineID, string Param)
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string query = "";
            if (Param.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                query = "select distinct ProductionOrderNo,FabricationNo from StoresItemTransactionSave_GEA WHERE (DateOfMissing is null OR DateOfMissing='1900-01-01 00:00:00.000')";
            else
                query = "select distinct ProductionOrderNo,FabricationNo from StoresItemTransactionSave_GEA WHERE (DateOfMissing is not null AND DateOfMissing <>'1900-01-01 00:00:00.000')";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                //cmd.Parameters.AddWithValue("@Machineid", machineID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        ProdOrderNumber = rdr["ProductionOrderNo"].ToString(),
                        FabricationNumber = rdr["FabricationNo"].ToString()
                    });
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
            return PONo;
        }
        //internal static string getFabricationNumber(string productionOrder)
        //{
        //    string list = string.Empty;
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    SqlCommand cmd = null;
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        cmd = new SqlCommand(@"select distinct FabricationNo from ScheduleDetails_GEA where Machineid=@Machineid and ProductionOrder=@PONO", conn);
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Parameters.AddWithValue("@Machineid", "Assembly");
        //        cmd.Parameters.AddWithValue("@PONO", productionOrder);
        //        rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            list = (rdr["FabricationNo"].ToString());
        //            //break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());
        //    }
        //    finally
        //    {
        //        if (conn != null) conn.Close();
        //        if (rdr != null) rdr.Close();
        //    }
        //    return list;
        //}

        //internal static string getProdNumber(string Fabricationnumber)
        //{
        //    string list = string.Empty;
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    SqlCommand cmd = null;
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        cmd = new SqlCommand(@"select distinct FabricationNo from ScheduleDetails_GEA where Machineid=@Machineid and ProductionOrder=@PONO", conn);
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Parameters.AddWithValue("@Machineid", "Assembly");
        //        cmd.Parameters.AddWithValue("@PONO", Fabricationnumber);
        //        rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            list = (rdr["FabricationNo"].ToString());
        //            //break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());
        //    }
        //    finally
        //    {
        //        if (conn != null) conn.Close();
        //        if (rdr != null) rdr.Close();
        //    }
        //    return list;
        //}

        internal static DataTable GetMachineDataAssemblyReportData(string prouOrder, string fabricationNum, string machineID, out DataTable secondGridData, out DataTable headerData, out DataTable ChecklistData)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            headerData = new DataTable();
            DataTable firstGridTbl = new DataTable();
            secondGridData = new DataTable();
            ChecklistData = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[S_GetMachineDataAssemblyReport_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialID", "");
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                // cmd.Parameters.AddWithValue("@MachineID", "Assembly");
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                rdr = cmd.ExecuteReader();
                headerData.Load(rdr);
                firstGridTbl.Load(rdr);
                ChecklistData.Load(rdr);
                secondGridData.Load(rdr);
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
            return firstGridTbl;
        }
        internal static DataTable GetElectroTechEquipmentReportData(string prouOrder, string fabricationNum, string machineID, out DataTable headerData)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            headerData = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[S_GetElectrotechnicalReport_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialID", "");
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                //cmd.Parameters.AddWithValue("@MachineID", "Assembly");
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                headerData.Load(rdr);
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
            return machineDataTbl;
        }
        #endregion

        #region -------- Testing Report -------
        //internal static List<string> GetProductionOrderListForTestingPackingReport(string table)
        //{
        //    List<string> PONo = new List<string>();
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    SqlCommand cmd = null;
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        string columnname = "OrderNo";
        //        if (table == "BalancingReportTransaction")
        //        {
        //            columnname = "Productionno";
        //        }

        //        cmd = new SqlCommand(@"select distinct " + columnname + " from " + table, conn);
        //        cmd.CommandType = CommandType.Text;
        //        rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            PONo.Add(rdr[columnname].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());
        //    }
        //    finally
        //    {
        //        if (conn != null) conn.Close();
        //        if (rdr != null) rdr.Close();
        //    }
        //    return PONo;
        //}

        internal static List<ProdFabricationNumber> GetProductionOrderListForBlueCard()
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {

                string Query = "select distinct FabricationNo as FabricationNo,OrderNo as OrderNo from BlueCardTransaction_GEA where Confirmation=2  and OrderNo not  in (select distinct ProductionOrder from ProDecanterDetails_Gea)";
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        FabricationNumber = rdr["FabricationNo"].ToString(),
                        ProdOrderNumber = rdr["OrderNo"].ToString()
                    });
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
            return PONo;
        }

        internal static List<ProdFabricationNumber> GetAllProdOrder()
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {

                string Query = "select distinct ProductionOrder as ProdOrderNumber,FabricationNo as FabricationNumber from ScheduleDetails_GEA where FabricationNo<>''";
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        FabricationNumber = rdr["FabricationNumber"].ToString(),
                        ProdOrderNumber = rdr["ProdOrderNumber"].ToString()
                    });
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
            return PONo;
        }

        internal static List<ProdFabricationNumber> GetProductionOrderListForTestingPackingReport(string table)
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {

                string Query = "";

                string columnname = "OrderNo";
                if (table == "BalancingReportTransaction")
                {
                    Query = @"select distinct b.Productionno,s.MaterialID from BalancingReportTransaction b  join  ScheduleDetails_GEA s on b.Productionno=s.ProductionOrder";

                }
                else
                {
                    Query = "select distinct OrderNo as ProdOrderNumber,FabricationNo as FabricationNumber from " + table;
                }

                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (table == "BalancingReportTransaction")
                    {
                        PONo.Add(new ProdFabricationNumber
                        {
                            MaterialNumber = rdr["MaterialID"].ToString(),
                            ProdOrderNumber = rdr["Productionno"].ToString()

                        });
                    }
                    else
                    {
                        PONo.Add(new ProdFabricationNumber
                        {
                            FabricationNumber = rdr["FabricationNumber"].ToString(),
                            ProdOrderNumber = rdr["ProdOrderNumber"].ToString()
                        });
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
            return PONo;
        }

        internal static List<string> GetAllComponentsByMachine(string machine)
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct componentid from componentoperationpricing where machineid=@Machineid", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", machine);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["componentid"].ToString());
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
            return list;
        }
        internal static string getMachineIDForTestingPackingReport(string process)
        {
            string machineid = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct machineid from machineinformation where process=@process", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@process", process);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    machineid = rdr["machineid"].ToString();
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
            return machineid;
        }

        internal static DataTable GetTestingReportData(string prouOrder, string fabricationNum, string componentid, string machineid, out DataTable headerData)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            headerData = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[dbo].[S_GetDecanterChecklistReport_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialID", componentid);
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                headerData.Load(rdr);
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
            return machineDataTbl;
        }
        #endregion

        #region ------- Vibration test protocol -------
        internal static DataTable GetVibrationTestProtocolReportData(string prouOrder, string fabricationNum, string componentid, string macineid, out DataTable remarksMeasureCondTbl, out DataTable measuredValueTbl)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            remarksMeasureCondTbl = new DataTable();
            measuredValueTbl = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[dbo].[S_GetVibrationTestProtocolReport_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialID", componentid);
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                cmd.Parameters.AddWithValue("@MachineID", macineid);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                remarksMeasureCondTbl.Load(rdr);
                measuredValueTbl.Load(rdr);
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
            return machineDataTbl;
        }
        #endregion

        #region ------- Noise Measurement protocol -------
        internal static DataTable GetNoiseMeasurementProtocolReportData(string prouOrder, string fabricationNum, string componentid, string machineid, out DataTable machineDimensionTbl, out DataTable speedCapacityTbl, out DataTable measuringPointTbl)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            machineDimensionTbl = new DataTable();
            speedCapacityTbl = new DataTable();
            measuringPointTbl = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[dbo].[S_GetNoiseMeasurementReport_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialID", componentid);
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                machineDimensionTbl.Load(rdr);
                speedCapacityTbl.Load(rdr);
                measuringPointTbl.Load(rdr);
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
            return machineDataTbl;
        }
        internal static bool isProductionOrderProMaterial(string prouOrder, string fabricationNum, string param)
        {
            bool isProReport = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {

                if (param.Equals("decanterChecklistPacking", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand(@"select * from DecanterChecklistPackingTransaction_GEA where OrderNo=@ProductionOrder and FabricationNo=@FabricationNo and IsProModel=1", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ProductionOrder", prouOrder);
                    cmd.Parameters.AddWithValue("@FabricationNo", fabricationNum);
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        isProReport = true;
                    }
                }
                else if (param.Equals("decanterFinalChecklistPacking", StringComparison.OrdinalIgnoreCase))
                {
                    cmd = new SqlCommand(@"select * from DecanterFinalTestingPackingTransaction_GEA  where OrderNo=@ProductionOrder and FabricationNo=@FabricationNo and IsProModel=1", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ProductionOrder", prouOrder);
                    cmd.Parameters.AddWithValue("@FabricationNo", fabricationNum);
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        isProReport = true;
                    }
                }
                else
                {
                    //                cmd = new SqlCommand(@"select s.MaterialID from ScheduleDetails_GEA  s inner join machineinformation m on s.Machineid=m.machineid where m.Process='Testing' and s.ProductionOrder=@ProductionOrder
                    //and s.FabricationNo=@FabricationNo", conn);
                    cmd = new SqlCommand(@"select s.MaterialID from ScheduleDetails_GEA  s inner join machineinformation m on s.Machineid=m.machineid where s.ProductionOrder=@ProductionOrder
and s.FabricationNo=@FabricationNo", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ProductionOrder", prouOrder);
                    cmd.Parameters.AddWithValue("@FabricationNo", fabricationNum);
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        string materialId = "";
                        while (rdr.Read())
                        {
                            materialId = rdr["MaterialID"].ToString().ToLower();
                            break;
                        }
                        if (materialId.Contains("pro"))
                        {
                            isProReport = true;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
            return isProReport;
        }
        internal static DataTable GetProNoiseMeasurementProtocolReportData(string prouOrder, string fabricationNum, string componentid, string machineid, out DataTable machineDimensionTbl, out DataTable speedCapacityTbl, out DataTable measuringPointTbl)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            machineDimensionTbl = new DataTable();
            speedCapacityTbl = new DataTable();
            measuringPointTbl = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[dbo].[S_GetNoiseMeasurementReportProDecanter_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaterialID", componentid);
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                machineDimensionTbl.Load(rdr);
                speedCapacityTbl.Load(rdr);
                measuringPointTbl.Load(rdr);
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
            return machineDataTbl;
        }
        #endregion

        #region ------ Decanter checklist - packing----
        internal static DataTable GetDecanterChecklistPackingReportData(string prouOrder, string fabricationNum, string machineid, out DataTable checklistData, string param)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            checklistData = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[dbo].[S_GetDecanterChecklistPackingReport_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                cmd.Parameters.AddWithValue("@Param", param);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                checklistData.Load(rdr);
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
            return machineDataTbl;
        }
        #endregion
        #region ------ Decanter final checklist - packing----
        internal static DataTable GetDecanterFinalChecklistPackingReportData(string prouOrder, string fabricationNum, string machineid, out DataTable checklistData, string param, out DataTable remarksTbl)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            checklistData = new DataTable();
            remarksTbl = new DataTable();
            SqlDataReader rdr = null;
            string query = @"[dbo].[S_GetDecanterFinalTestingPackingReport_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                cmd.Parameters.AddWithValue("@Param", param);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                checklistData.Load(rdr);
                remarksTbl.Load(rdr);
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
            return machineDataTbl;
        }
        #endregion
        #region ------ Decanter blue card report - packing----
        internal static DataTable GetDecanterAcceptanceTestCardReportData(string procName, string prouOrder, string fabricationNum, out DataTable speedChecklistTbl, out DataTable materialIdentificationTbl, out DataTable supervisorAssemblyTbl, out DataTable logisticTbl, out DataTable qualityTbl, out DataTable remarkTbl)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable machineDataTbl = new DataTable();
            speedChecklistTbl = new DataTable();
            materialIdentificationTbl = new DataTable();
            supervisorAssemblyTbl = new DataTable();
            logisticTbl = new DataTable();
            qualityTbl = new DataTable();
            remarkTbl = new DataTable();
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(procName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderNo", prouOrder);
                //cmd.Parameters.AddWithValue("@MachineID", machineid);
                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNum);
                rdr = cmd.ExecuteReader();
                machineDataTbl.Load(rdr);
                speedChecklistTbl.Load(rdr);
                materialIdentificationTbl.Load(rdr);
                supervisorAssemblyTbl.Load(rdr);
                qualityTbl.Load(rdr);
                logisticTbl.Load(rdr);
                remarkTbl.Load(rdr);
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
            return machineDataTbl;
        }
        #endregion

        #region SubOperation
        internal static List<SupOperation> GetSupOperation(string Station, string Component)
        {
            string RepNo = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<SupOperation> EntityList = new List<SupOperation>();
            try
            {
                cmd = new SqlCommand(@"select distinct * from AssemblyActivityMaster_GEA where Station=@Station and Componentid=@Componentid", conn);
                cmd.Parameters.AddWithValue("@Station", Station);
                cmd.Parameters.AddWithValue("@Componentid", Component);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EntityList.Add(new SupOperation
                    {
                        Station = rdr["Station"].ToString(),
                        MaterialID = rdr["Componentid"].ToString(),
                        Operation = rdr["OperationNo"].ToString(),
                        CycleTime = rdr["StdCycletime"].ToString(),
                        SubOperation = rdr["Activity"].ToString(),
                        Checked = DBNull.Value.ToString().Equals(rdr["IsDefault"].ToString()) ? false : rdr["IsDefault"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) ? true : false
                    });
                    //break;
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
            return EntityList;
        }

        internal static bool DeleteSubOperation(SupOperation Entity)
        {
            string RepNo = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool deletesuccessfully = false;
            string Query = @"if exists(select * from AssemblyActivityTransaction_GEA where Machineid=@Station and MaterialID=@MaterialID and Activity=@Activity )
begin
	DECLARE @Event nvarchar(50);
	select top 1 @Event=Event from AssemblyActivityTransaction_GEA where Machineid=@Station and MaterialID=@MaterialID and Activity=@Activity 
	order by EventTS desc
	if @Event='End'
	begin
		delete from AssemblyActivityMaster_GEA where Station=@station and Componentid=@MaterialID and Activity=@Activity and OperationNo=@Operation
	end
end
else
begin
 delete from AssemblyActivityMaster_GEA where Station=@station and Componentid=@MaterialID and Activity=@Activity and OperationNo=@Operation
end";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@Station", Entity.Station);
                cmd.Parameters.AddWithValue("@Operation", Entity.Operation);
                cmd.Parameters.AddWithValue("@Activity", Entity.SubOperation);
                cmd.Parameters.AddWithValue("@MaterialID", Entity.MaterialID);
                cmd.CommandType = CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    deletesuccessfully = true;
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
            return deletesuccessfully;
        }

        internal static void SaveSubOperationData(SupOperation Entity)
        {
            string RepNo = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            List<SupOperation> EntityList = new List<SupOperation>();
            string Query = @"if not exists(select * from AssemblyActivityMaster_GEA where Station=@Station and Activity=@Activity and OperationNo=@OperationNo and Componentid=@Componentid)
	                                begin
	                                insert into AssemblyActivityMaster_GEA(Station,OperationNo,Activity,IsDefault,UpdatedBy,UpdatedTS,StdCycletime,Componentid)
	                                values(@Station,@OperationNo,@Activity,@IsDefault,@UpdatedBy,@UpdatedTS,@StdCycletime,@Componentid)
	                                end
                                else
	                                begin
	                                update AssemblyActivityMaster_GEA set IsDefault=@IsDefault,StdCycletime=@StdCycletime,UpdatedBy=@UpdatedBy,UpdatedTS=@UpdatedTS 
	                                where Station=@Station and Activity=@Activity and OperationNo=@OperationNo and Componentid=@Componentid
                                end";
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@Station", Entity.Station);
                cmd.Parameters.AddWithValue("@OperationNo", Entity.Operation);
                cmd.Parameters.AddWithValue("@Activity", Entity.SubOperation);
                cmd.Parameters.AddWithValue("@StdCycletime", Entity.CycleTime);
                cmd.Parameters.AddWithValue("@IsDefault", Entity.Checked);
                cmd.Parameters.AddWithValue("@Componentid", Entity.MaterialID);
                cmd.Parameters.AddWithValue("@UpdatedBy", HttpContext.Current.Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
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

        internal static List<string> GetStationDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            List<string> Station = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd = new SqlCommand("select distinct machineid from machineinformation where  process in('Assembly','Packing','Testing','Stores')", con);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Station.Add(reader["machineid"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return Station;
        }

        internal static List<string> GetSubActivityDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            List<string> Station = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd = new SqlCommand("select distinct ComponentID from NonMachining_SubOperation_GEA", con);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Station.Add(reader["ComponentID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return Station;
        }


        internal static string GetLatestIQC(string page)
        {

            SqlConnection con = ConnectionManager.GetConnection();
            string PlanNoAndRevNo = "";
            string query = "";
            switch (page.ToLower())
            {
                case "quality":
                    query = @"select top 1 PlanNoAndRevNo from QualityIncomingMaster_GEA order by IDD desc";
                    break;
                default:
                    query = @"select top 1 PlanNoAndRevNo from SPC_Characteristic order by ID desc";
                    break;
            }
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd = new SqlCommand(query, con);
                reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    PlanNoAndRevNo = (reader["PlanNoAndRevNo"].ToString());
                }

            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return PlanNoAndRevNo;
        }

















































































































        #endregion

        #region -----Machine Componet Master ----------------------------------        public static List<string> GetComponents(string Machineid)        {            SqlConnection sqlConn = ConnectionManager.GetConnection();            List<string> plantid = new List<string>();            try            {                SqlCommand cmd = new SqlCommand("select  distinct c.description from componentinformation c inner join componentoperationpricing cop on c.componentid=cop.componentid where cop.machineid=@Machineid ", sqlConn);                cmd.CommandType = System.Data.CommandType.Text;                cmd.Parameters.AddWithValue("@Machineid", Machineid);                SqlDataReader rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    plantid.Add(rdr["description"].ToString());                }                rdr.Close();            }            catch (Exception ex)            {                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());                throw;            }            finally            {                if (sqlConn != null) sqlConn.Close();            }            return plantid;        }        public static List<MachineCompnentInfo> GetMachineComponentDetails(string machineid)        {            SqlConnection sqlConn = ConnectionManager.GetConnection();            List<MachineCompnentInfo> list = new List<MachineCompnentInfo>();            try            {                SqlCommand cmd = new SqlCommand("select * from  ComponentMaster where (MachineID=@MachineID or isnull(@MachineID,'')='')", sqlConn);                cmd.Parameters.AddWithValue("@MachineID", machineid == "All" ? "" : machineid);                cmd.CommandType = System.Data.CommandType.Text;                SqlDataReader rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    list.Add(new MachineCompnentInfo { MachineID = rdr["MachineID"].ToString(), Component = rdr["ComponentDescription"].ToString() });                }                rdr.Close();            }            catch (Exception ex)            {                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());                throw;            }            finally            {                if (sqlConn != null) sqlConn.Close();            }            return list;        }        public static string InsertUpdateMachineComponentDetails(MachineCompnentInfo input)        {            SqlConnection sqlConn = ConnectionManager.GetConnection();            string output = "";            try            {                SqlCommand cmd = new SqlCommand(@"if exists(select * from ComponentMaster where MachineID=@MachineId and ComponentDescription=@Component)begin		select 'Exists' as Outputendelsebegin		insert into ComponentMaster(MachineID,ComponentDescription) values(@MachineId,@Component)		select 'Inserted' as Outputend		", sqlConn);                cmd.CommandType = System.Data.CommandType.Text;                cmd.Parameters.AddWithValue("@MachineId", input.MachineID);                cmd.Parameters.AddWithValue("@Component", input.Component);                SqlDataReader rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    output = rdr["Output"].ToString();                }                rdr.Close();            }            catch (Exception ex)            {                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());                throw;            }            finally            {                if (sqlConn != null) sqlConn.Close();            }            return output;        }        internal static bool DeleteMachineComponentInfor(MachineCompnentInfo input)        {            bool IsDeleted = false;            SqlConnection conn = null;            string query = @"delete from ComponentMaster where MachineID=@MachineID and ComponentDescription=@Component";            try            {                conn = ConnectionManager.GetConnection();                SqlCommand cmd = new SqlCommand(query, conn);                cmd.Parameters.AddWithValue("@MachineID", input.MachineID);                cmd.Parameters.AddWithValue("@Component", input.Component);                cmd.CommandType = CommandType.Text;                IsDeleted = cmd.ExecuteNonQuery() > 0 ? true : false;            }            catch (Exception ex)            {                IsDeleted = false;                Logger.WriteErrorLog(ex.Message);            }            finally            {                if (conn != null) conn.Close();            }            return IsDeleted;        }
        #endregion

        #region ------ Balancing Report changes-----
        internal static DataTable GetBalancingCertificateReportData(string productionnumber)        {            DataTable dt = new DataTable();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand("[S_GetBalencingReport_GEA]", conn);                cmd.CommandType = CommandType.StoredProcedure;                cmd.Parameters.AddWithValue("@ProductionNo", productionnumber);                cmd.Parameters.AddWithValue("@Param", "view2");                rdr = cmd.ExecuteReader();                dt.Load(rdr);            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return dt;        }        internal static DataTable BalancingReportData(string Component)        {            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            DataTable ReportData = new DataTable();            try            {                cmd = new SqlCommand("S_GetBalencingReport_GEA", conn);                cmd.CommandType = CommandType.StoredProcedure;                cmd.Parameters.AddWithValue("@Component", Component);                cmd.Parameters.AddWithValue("@Param", "view1");                rdr = cmd.ExecuteReader();                ReportData.Load(rdr);            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return ReportData;        }
        internal static string getBalancingProductionOrderForFinalReport(string productionOrder)        {            string balancingPO = "";            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand("select * from MachineDataAssemblyDescription_GEA  where OrderNo=@OrderNo", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@OrderNo", productionOrder);                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        balancingPO = rdr["BalancingProductionOrder"].ToString();
                    }
                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return balancingPO;        }
        #endregion


        internal static bool CheckforMaintainanceData(string WeekNumber, string MachineID)
        {
            bool status = false;
            SqlConnection Conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            string Query = @"SELECT distinct       CASE WHEN (select count(ActivityMasterYearlyData_MGTL.activityid) from ActivityMasterYearlyData_MGTL where FreqID in (select FreqID from ActivityFreq_MGTL where Frequency<>'Daily')and ActivityMasterYearlyData_MGTL.WeekNo=@weeknoand [year] in (select YearNo from Calender where WeekDate=@Date)and ActivityMasterYearlyData_MGTL.MachineID=@Machineid )=(select count(ActivityTransaction_MGTL.ActivityID) from ActivityTransaction_MGTL where Frequency in (select FreqID from ActivityFreq_MGTL where Frequency<>'Daily' )and ActivityTransaction_MGTL.WeekNo ='Week'+@weeknoand datepart(yyyy,ActivityDoneTS) in (select YearNo from Calender where WeekDate=@Date)and ActivityTransaction_MGTL.MachineID=@Machineid )       THEN 1       ELSE 0       END AS [Enable]FROM ActivityMasterYearlyData_MGTL ,ActivityTransaction_MGTL";
            try
            {
                cmd = new SqlCommand(Query, Conn);

                cmd.Parameters.AddWithValue("@weekno", WeekNumber);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                cmd.Parameters.AddWithValue("@Machineid", MachineID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                        status = rdr["Enable"].ToString().Equals("1") ? true : false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (Conn != null) Conn.Close();
            }
            return status;
        }
        internal static string GetWeekNumber()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            string WeekNumber = "";
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("select WeekNumber from Calender where WeekDate=@Date", con);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    WeekNumber = rdr["WeekNumber"].ToString();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (rdr != null) rdr.Close();
            }
            return WeekNumber;
        }


        #region Model Assembly Balancing Master Data

        internal static List<Model> GetSaveDeleteModelData(string Param, string Model)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<Model> ModelData = new List<Model>();
            string Query = ""; int slno = 1;
            switch (Param.ToLower())
            {
                case "view":
                    {
                        Query = @"select * from [NonMachining_SubOperation_GEA]";
                        break;
                    }
                case "save":
                    {
                        Query = @"if not exists( select * from [NonMachining_SubOperation_GEA] where ComponentID=@ComponentID)
	                                begin 
		                                insert into NonMachining_SubOperation_GEA(ComponentID)
		                                values(@ComponentID)
	                                end";
                        break;
                    }
                case "delete":
                    {
                        Query = @"delete from [NonMachining_SubOperation_GEA]  where ComponentID=@ComponentID; delete from NoiseMeasurementFormulaMaster_GEA where MaterialID=@ComponentID";
                        break;
                    }
            }
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@ComponentID", Model);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ModelData.Add(new GEA.Model { Slno = slno++, _Model = rdr["ComponentID"].ToString() });
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
            return ModelData;
        }


        internal static List<string> GetAssemblyModelList(string type)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> ModelData = new List<string>();
            string query = type.Equals("Main") ? @"select distinct [AssemblyType] from [AssemblyeTypeMaster_GEA] where MotorType='MainMotor'" : @"select distinct [AssemblyType] from [AssemblyeTypeMaster_GEA] where MotorType='SecondaryMotor'";
            try
            {
                cmd = new SqlCommand(query, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ModelData.Add(rdr["AssemblyType"].ToString());
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
            return ModelData;
        }

        internal static void GetSaveDeleteAssemblyModel(string Param, ref AssemblyModel assembly)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<Model> ModelData = new List<Model>();
            string Query = "";
            switch (Param.ToLower())
            {
                case "view":
                    {
                        Query = @"select * from AssemblyeTypeMaster_GEA where AssemblyType=@AssemblyType and MotorType='MainMotor'";
                        break;
                    }
                case "viewsecondary":
                    {
                        Query = @"select * from AssemblyeTypeMaster_GEA where AssemblyType=@SecondaryAssemblyType and MotorType='SecondaryMotor'";
                        break;
                    }
                case "save":
                    {
                        Query = @"if not exists (select * from AssemblyeTypeMaster_GEA where AssemblyType=@AssemblyType and MotorType='MainMotor')
	                                begin
	                                insert into AssemblyeTypeMaster_GEA(AssemblyType,MotorType,Voltage,Curent,Frequency,Power,Speed,CosValue,Efficiency,Bearing_Drive,Bearing_NonDrive)
	                                values (@AssemblyType,'MainMotor',@MainVoltage,@MainCurent,@MainFrequency,@MainPower,@MainSpeed,@MainCosValue,@MainEfficiency,@MainBearing_Drive,@MainBearing_NonDrive)
	                                end
                                else
                                    begin
	                                    update AssemblyeTypeMaster_GEA set Voltage=@MainVoltage,Curent=@MainCurent,Frequency=@MainFrequency, Power=@MainPower,Speed=@MainSpeed,CosValue=@MainCosValue,Efficiency=@MainEfficiency,Bearing_Drive=@MainBearing_Drive, Bearing_NonDrive=@MainBearing_NonDrive where AssemblyType=@AssemblyType and MotorType='MainMotor'
                                    end";

                        break;
                    }
                case "savesecondary":
                    {
                        Query = @"if not exists (select * from AssemblyeTypeMaster_GEA where AssemblyType=@SecondaryAssemblyType and MotorType='SecondaryMotor')
	                                begin
	                                insert into AssemblyeTypeMaster_GEA(AssemblyType,MotorType,Voltage,Curent,Frequency,Power,Speed,CosValue,Efficiency,Bearing_Drive,Bearing_NonDrive)
	                                values (@SecondaryAssemblyType,'SecondaryMotor',@Voltage,@Curent,@Frequency,@Power,@Speed,@CosValue,@Efficiency,@Bearing_Drive,@Bearing_NonDrive)
	                                end
                                else
                                    begin
	                                    update AssemblyeTypeMaster_GEA set Voltage=@Voltage,Curent=@Curent,Frequency=@Frequency, Power=@Power,Speed=@Speed,CosValue=@CosValue,Efficiency=@Efficiency,Bearing_Drive=@Bearing_Drive, Bearing_NonDrive=@Bearing_NonDrive where AssemblyType=@SecondaryAssemblyType and MotorType='SecondaryMotor' end";

                        break;
                    }

                case "delete":
                    {
                        Query = @"Delete from AssemblyeTypeMaster_GEA where AssemblyType=@AssemblyType and MotorType='MainMotor'";
                        break;
                    }
                case "deletesecondary":
                    {
                        Query = @"Delete from AssemblyeTypeMaster_GEA where AssemblyType=@AssemblyType and MotorType='SecondaryMotor'";
                        break;
                    }
            }
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@AssemblyType", string.IsNullOrEmpty(assembly._AssemblyType) ? "" : assembly._AssemblyType);
                cmd.Parameters.AddWithValue("@SecondaryAssemblyType", string.IsNullOrEmpty(assembly._AssemblySecondaryType) ? "" : assembly._AssemblySecondaryType);
                cmd.Parameters.AddWithValue("@MainVoltage", string.IsNullOrEmpty(assembly.MainVoltage) ? "" : assembly.MainVoltage);
                cmd.Parameters.AddWithValue("@MainCurent", string.IsNullOrEmpty(assembly.MainCurrent) ? "" : assembly.MainCurrent);
                cmd.Parameters.AddWithValue("@MainSpeed", string.IsNullOrEmpty(assembly.MainSpeed) ? "" : assembly.MainSpeed);
                cmd.Parameters.AddWithValue("@MainFrequency", string.IsNullOrEmpty(assembly.MainFrequency) ? "" : assembly.MainFrequency);
                cmd.Parameters.AddWithValue("@MainPower", string.IsNullOrEmpty(assembly.MainPower) ? "" : assembly.MainPower);
                cmd.Parameters.AddWithValue("@MainEfficiency", string.IsNullOrEmpty(assembly.MainEfficiency) ? "" : assembly.MainEfficiency);
                cmd.Parameters.AddWithValue("@MainBearing_Drive", string.IsNullOrEmpty(assembly.MainBearing) ? "" : assembly.MainBearing);
                cmd.Parameters.AddWithValue("@MainBearing_NonDrive", string.IsNullOrEmpty(assembly.MainNonBearing) ? "" : assembly.MainNonBearing);
                cmd.Parameters.AddWithValue("@MainCosValue", string.IsNullOrEmpty(assembly.MainCosValue) ? "" : assembly.MainCosValue);
                cmd.Parameters.AddWithValue("@Voltage", string.IsNullOrEmpty(assembly.SecondaryVoltage) ? "" : assembly.SecondaryVoltage);
                cmd.Parameters.AddWithValue("@Curent", string.IsNullOrEmpty(assembly.SecondaryCurrent) ? "" : assembly.SecondaryCurrent);
                cmd.Parameters.AddWithValue("@Speed", string.IsNullOrEmpty(assembly.SecondarySpeed) ? "" : assembly.SecondarySpeed);
                cmd.Parameters.AddWithValue("@Frequency", string.IsNullOrEmpty(assembly.SecondaryFrequency) ? "" : assembly.SecondaryFrequency);
                cmd.Parameters.AddWithValue("@Power", string.IsNullOrEmpty(assembly.SecondaryPower) ? "" : assembly.SecondaryPower);
                cmd.Parameters.AddWithValue("@Efficiency", string.IsNullOrEmpty(assembly.SecondaryEfficiency) ? "" : assembly.SecondaryEfficiency);
                cmd.Parameters.AddWithValue("@Bearing_Drive", string.IsNullOrEmpty(assembly.SecondaryBearing) ? "" : assembly.SecondaryBearing);
                cmd.Parameters.AddWithValue("@Bearing_NonDrive", string.IsNullOrEmpty(assembly.SecondaryNonBearing) ? "" : assembly.SecondaryNonBearing);
                cmd.Parameters.AddWithValue("@CosValue", string.IsNullOrEmpty(assembly.SecondaryCosValue) ? "" : assembly.SecondaryCosValue);
                rdr = cmd.ExecuteReader();
                int Count = 0;
                while (rdr.Read())
                {
                    switch (Param.ToLower())
                    {
                        case "view":
                            {
                                assembly._AssemblyType = rdr["AssemblyType"].ToString();
                                assembly.MainVoltage = rdr["Voltage"].ToString();
                                assembly.MainCurrent = rdr["Curent"].ToString();
                                assembly.MainSpeed = rdr["Speed"].ToString();
                                assembly.MainFrequency = rdr["Frequency"].ToString();
                                assembly.MainPower = rdr["Power"].ToString();
                                assembly.MainEfficiency = rdr["Efficiency"].ToString();
                                assembly.MainBearing = rdr["Bearing_Drive"].ToString();
                                assembly.MainNonBearing = rdr["Bearing_NonDrive"].ToString();
                                assembly.MainCosValue = rdr["CosValue"].ToString();
                                Count++;
                                break;
                            }
                        case "viewsecondary":
                            {
                                assembly.SecondaryVoltage = rdr["Voltage"].ToString();
                                assembly.SecondaryCurrent = rdr["Curent"].ToString();
                                assembly.SecondarySpeed = rdr["Speed"].ToString();
                                assembly.SecondaryFrequency = rdr["Frequency"].ToString();
                                assembly.SecondaryPower = rdr["Power"].ToString();
                                assembly.SecondaryEfficiency = rdr["Efficiency"].ToString();
                                assembly.SecondaryBearing = rdr["Bearing_Drive"].ToString();
                                assembly.SecondaryNonBearing = rdr["Bearing_NonDrive"].ToString();
                                assembly.SecondaryCosValue = rdr["CosValue"].ToString();
                                break;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
        }

        internal static List<string> GetBalancingModelList()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<string> ModelData = new List<string>();
            try
            {
                cmd = new SqlCommand("select distinct Component from BalancingReportComponentMaster", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ModelData.Add(rdr["Component"].ToString());
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
            return ModelData;
        }

        internal static void GetSaveDeleteBalancingModel(string Param, ref BalancingModel balancing)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            List<Model> ModelData = new List<Model>();
            string Query = "";
            switch (Param.ToLower())
            {
                case "view":
                    {
                        Query = @"select * from BalancingReportComponentMaster where Component=@Component";
                        break;
                    }
                case "save":
                    {
                        Query = @"if not exists(select * from BalancingReportComponentMaster where Component=@Component and Param1='P1')
                                    begin
	                                    insert into BalancingReportComponentMaster (Component,Param1,Tol_g,R_mm,Dim_mm,ISO,Unit,Param2)
	                                    Values(@Component,'P1',@P1Tol,@P1R,@P1Dimb,@P1ISOm,@P1Unit,@P1Dima)
	                                    end
                                    else
                                    begin
                                    update BalancingReportComponentMaster set Tol_g=@P1Tol, R_mm=@P1R,Dim_mm=@P1Dimb,ISO=@P1ISOm,Unit=@P1Unit, Param2=@P1Dima where Component=@Component and Param1='P1'
                                    end
                                    if not exists(select * from BalancingReportComponentMaster where Component=@Component and Param1='ST')
                                    begin
	                                    insert into BalancingReportComponentMaster (Component,Param1,Tol_g,R_mm,Dim_mm,ISO,Unit,Param2)
	                                    Values(@Component,'ST',@STTol,@STR,@STDimb,@STISOm,@STUnit,@STDima)
	                                    end
                                    else
                                    begin
                                    update BalancingReportComponentMaster set Tol_g=@STTol, R_mm=@STR,Dim_mm=@STDimb,ISO=@STISOm,Unit=@STUnit, Param2=@STDima where Component=@Component and Param1='ST'
                                    end
                                    if not exists(select * from BalancingReportComponentMaster where Component=@Component and Param1='P2')
                                    begin
	                                    insert into BalancingReportComponentMaster (Component,Param1,Tol_g,R_mm,Dim_mm,ISO,Unit,Param2)
	                                    Values(@Component,'P2',@P2Tol,@P2R,@P2Dimb,@P2ISOm,@P2Unit,@P2Dima)
	                                    end
                                    else
                                    begin
                                    update BalancingReportComponentMaster set Tol_g=@P2Tol, R_mm=@P2R,Dim_mm=@P2Dimb,ISO=@P2ISOm,Unit=@P2Unit, Param2=@P2Dima where Component=@Component and Param1='P2'
                                    end";
                        break;
                    }
                case "delete":
                    {
                        Query = @"Delete from BalancingReportComponentMaster where Component=@Component";
                        break;
                    }
            }
            try
            {
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Query;
                cmd.Parameters.AddWithValue("@Component", balancing._BalancingType);
                cmd.Parameters.AddWithValue("@P1Tol", string.IsNullOrEmpty(balancing.P1Tol) ? "0" : balancing.P1Tol);
                cmd.Parameters.AddWithValue("@P1R", string.IsNullOrEmpty(balancing.P1R) ? "0" : balancing.P1R);
                cmd.Parameters.AddWithValue("@P1Dimb", string.IsNullOrEmpty(balancing.P1DimA) ? "0" : balancing.P1DimA);
                cmd.Parameters.AddWithValue("@P1Dima", string.IsNullOrEmpty(balancing.P1DimB) ? "a" : balancing.P1DimB);
                cmd.Parameters.AddWithValue("@P1ISOm", string.IsNullOrEmpty(balancing.P1ISO) ? "0" : balancing.P1ISO);
                cmd.Parameters.AddWithValue("@P1Unit", string.IsNullOrEmpty(balancing.P1Unt) ? "0" : balancing.P1Unt);
                cmd.Parameters.AddWithValue("@STTol", string.IsNullOrEmpty(balancing.STTol) ? "0" : balancing.STTol);
                cmd.Parameters.AddWithValue("@STR", string.IsNullOrEmpty(balancing.STR) ? "0" : balancing.STR);
                cmd.Parameters.AddWithValue("@STDimb", string.IsNullOrEmpty(balancing.STDimA) ? "0" : balancing.STDimA);
                cmd.Parameters.AddWithValue("@STDima", string.IsNullOrEmpty(balancing.STDimB) ? "b" : balancing.STDimB);
                cmd.Parameters.AddWithValue("@STISOm", string.IsNullOrEmpty(balancing.STISO) ? "0" : balancing.STISO);
                cmd.Parameters.AddWithValue("@STUnit", string.IsNullOrEmpty(balancing.STUnt) ? "0" : balancing.STUnt);
                cmd.Parameters.AddWithValue("@P2Tol", string.IsNullOrEmpty(balancing.P2Tol) ? "0" : balancing.P2Tol);
                cmd.Parameters.AddWithValue("@P2R", string.IsNullOrEmpty(balancing.P2R) ? "0" : balancing.P2R);
                cmd.Parameters.AddWithValue("@P2Dimb", string.IsNullOrEmpty(balancing.P2DimA) ? "0" : balancing.P2DimA);
                cmd.Parameters.AddWithValue("@P2Dima", string.IsNullOrEmpty(balancing.P2DimB) ? "c" : balancing.P2DimB);
                cmd.Parameters.AddWithValue("@P2ISOm", string.IsNullOrEmpty(balancing.P2ISO) ? "0" : balancing.P2ISO);
                cmd.Parameters.AddWithValue("@P2Unit", string.IsNullOrEmpty(balancing.P2Unt) ? "0" : balancing.P2Unt);
                rdr = cmd.ExecuteReader();
                int Count = 0;
                while (rdr.Read())
                {
                    switch (Count)
                    {
                        case 0:
                            {

                                balancing._BalancingType = rdr["Component"].ToString();
                                balancing.P1Tol = rdr["Tol_g"].ToString();
                                balancing.P1R = rdr["R_mm"].ToString();
                                balancing.P1DimA = rdr["Dim_mm"].ToString();
                                balancing.P1DimB = rdr["Param2"].ToString();
                                balancing.P1ISO = rdr["ISO"].ToString();
                                balancing.P1Unt = rdr["Unit"].ToString();
                                Count++;
                                break;
                            }
                        case 1:
                            {
                                balancing.STTol = rdr["Tol_g"].ToString();
                                balancing.STR = rdr["R_mm"].ToString();
                                balancing.STDimA = rdr["Dim_mm"].ToString();
                                balancing.STDimB = rdr["Param2"].ToString();
                                balancing.STISO = rdr["ISO"].ToString();
                                balancing.STUnt = rdr["Unit"].ToString();
                                Count++;
                                break;
                            }
                        case 2:
                            {
                                balancing.P2Tol = rdr["Tol_g"].ToString();
                                balancing.P2R = rdr["R_mm"].ToString();
                                balancing.P2DimA = rdr["Dim_mm"].ToString();
                                balancing.P2DimB = rdr["Param2"].ToString();
                                balancing.P2ISO = rdr["ISO"].ToString();
                                balancing.P2Unt = rdr["Unit"].ToString();
                                break;
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
                if (conn != null) conn.Close();
                if (rdr != null) rdr.Close();
            }
        }

        #endregion

        #region ------ Hardness Report ---
        internal static List<string> GetQualityInhouseProductionOrderList(string tablename, string poNocolumnName, string machine)
        {
            List<string> list = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select distinct " + poNocolumnName + " from " + tablename + "   where MachineID='" + machine + "'", conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr[poNocolumnName].ToString());
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
            return list;
        }
        internal static List<ProdFabricationNumber> GetQualityInhouseProductionOrderList(string tablename, string poNocolumnName, string machine, string compColumnName, string checkBoxColumn, string grnNoColumnName)
        {
            List<ProdFabricationNumber> list = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                string query = "";
                if (grnNoColumnName != "")
                {
                    query = "select distinct " + poNocolumnName + ", " + compColumnName + ", " + grnNoColumnName + " from " + tablename + "   where MachineID='" + machine + "'";
                }
                else if (compColumnName != "")
                {
                    query = "select distinct " + poNocolumnName + ", " + compColumnName + " from " + tablename + "   where MachineID='" + machine + "'";
                }
                else
                {
                    query = "select distinct " + poNocolumnName + " from " + tablename + "   where MachineID='" + machine + "'";
                }
                if (checkBoxColumn != "")
                {
                    query += "and " + checkBoxColumn + "=1";
                }
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProdFabricationNumber data = new ProdFabricationNumber();
                    if (compColumnName != "")
                    {
                        data.MaterialNumber = rdr[compColumnName].ToString();
                    }
                    if (grnNoColumnName != "")
                    {
                        data.GRNNumber = rdr[grnNoColumnName].ToString();
                    }
                    data.ProdOrderNumber = rdr[poNocolumnName].ToString();
                    list.Add(data);
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
            return list;
        }
        internal static string GetPartnumber(string productionorder, string machineid)        {            SqlConnection con = ConnectionManager.GetConnection();            SqlDataReader rdr = null;            SqlCommand cmd = null;            string Partnumber = "";            try            {                cmd = new SqlCommand("select MaterialID from ScheduleDetails_GEA where ProductionOrder= @ProductionOrder and Machineid=@machineid", con);                cmd.Parameters.AddWithValue("@ProductionOrder", productionorder);
                cmd.Parameters.AddWithValue("@machineid", machineid);                rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    Partnumber = rdr["MaterialID"].ToString();                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (con != null) con.Close();                if (rdr != null) rdr.Close();            }            return Partnumber;        }
        internal static DataTable GetHardnessReportData(string machine, string proudOrder, string partNo, string grnNumber, out DataTable secondGridData)
        {
            DataTable firstGridTbl = new DataTable();
            secondGridData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            string query = @"[S_GetHardnessReportDetails_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartNo", partNo);
                cmd.Parameters.AddWithValue("@ProductionOrderNo", proudOrder);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);
                rdr = cmd.ExecuteReader();
                firstGridTbl.Load(rdr);
                secondGridData.Load(rdr);
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
            return firstGridTbl;
        }
        #endregion

        #region ------ Dye Penetration -----
        internal static DataTable GetDyePenetrationReportData(string machine, string proudOrder, string partNo, string grnNumber, out DataTable secondGridData)
        {
            DataTable firstGridTbl = new DataTable();
            secondGridData = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            string query = @"[S_GetDyePenetrationReportDetails_GEA]";

            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartNo", partNo);
                cmd.Parameters.AddWithValue("@ProductionOrderNo", proudOrder);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);
                rdr = cmd.ExecuteReader();
                firstGridTbl.Load(rdr);
                secondGridData.Load(rdr);
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
            return firstGridTbl;
        }
        #endregion

        #region ------ First Sample -----
        internal static DataSet GetFirstSampleReportData(string machine, string proudOrder, string partNo, string grnNumber)
        {
            DataSet gridTbl = new DataSet();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataAdapter rdr = null;
            SqlCommand cmd = null;
            string query = @"[S_GetFirstSampleReportDetails_GEA]";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartNo", partNo);
                cmd.Parameters.AddWithValue("@ProductionOrderNo", proudOrder);
                cmd.Parameters.AddWithValue("@MachineID", machine);
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);
                cmd.Parameters.AddWithValue("@Param", "View");
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(gridTbl);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return gridTbl;
        }
        #endregion

        #region ---- DC Report -----
        internal static DataTable GetDCReportData(string machineID, string productionorder, string partnumber, string grnNumber)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("[dbo].[S_GetDeviationReportDetails_GEA]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionorder);
                cmd.Parameters.AddWithValue("@PartNo", partnumber);
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.NextResult();
                    dt.Load(rdr);
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
        internal static string getPartName(string componentID)
        {
            string PartName = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"Select  substring(description,charindex('[',description,2)+1,len(description)-charindex(']',description)-2) as ModelDescription
From componentinformation where componentid = @CompID", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@CompID", componentID);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        PartName = rdr["ModelDescription"].ToString();
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
            return PartName;
        }
        internal static string getSampleQty(string productionOrder, string machine)
        {
            string PartName = "";
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"select * from ScheduleDetails_GEA where ProductionOrder=@PO and Machineid=@Machine", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PO", productionOrder);
                cmd.Parameters.AddWithValue("@Machine", machine);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        PartName = rdr["SamplingQty"].ToString();
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
            return PartName;
        }
        #endregion

        #region ---- NC Report ---
        internal static DataTable GetNCRReportData(string machineID, string productionOrder, string partNumber, string grnNumber)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("[S_GetNonConformanceReportDetails_GEA]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);
                cmd.Parameters.AddWithValue("@PartNo", partNumber);
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.NextResult();
                    dt.Load(rdr);
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

        #region ----- IQ Report ----
        internal static DataTable GetIQReportData(string machineID, string productionOrder, string partnumber, string grnNumber)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("[S_GetInternalQualityReportDetails_GEA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);
                cmd.Parameters.AddWithValue("@PartNo", partnumber);
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.NextResult();
                    dt.Load(rdr);
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

        #region DPHU Report

        internal static DataTable GetQuaityDPHUReport(DateTime fromDate, DateTime toDate, string machineID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();

            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                string Query = @"";
                switch (machineID)
                {
                    case "Quality In house":
                        {
                            Query = @"select DISTINCT  d1.*,c1.description as CompDescription,e1.Name as EmployeeName,N1.Reason1,N1.Reason2,N1.Reason3,N1.Reason4,N1.Reason5  from Inhouse_DPHUReportDetails_GEA d1 
                                        left join componentinformation c1 on c1.componentid=d1.PartNo
                                        left join employeeinformation e1 on d1.InspectedBy=e1.Employeeid
                                        LEFT JOIN NonConformanceReportDetails_GEA N1 ON N1.PartNo=D1.PartNo AND N1.ProductionOrderNo=D1.ProductionOrderNo where D1.ScheduleDate >= @FromDate and D1.ScheduleDate <=@ToDate";
                            break;
                        }
                    case "Quality Incoming":
                        {
                            Query = @"select DISTINCT d1.*,c1.description as CompDescription,e1.Name as EmployeeName,N1.Reason1,N1.Reason2,N1.Reason3,N1.Reason4,N1.Reason5,S1.PriorityChanged from Incoming_DPHUReportDetails_GEA d1 
                                        left join componentinformation c1 on c1.componentid=d1.PartNo
                                        left join employeeinformation e1 on d1.InspectedBy=e1.Employeeid
                                        LEFT JOIN NonConformanceReportDetails_GEA N1 ON N1.PartNo=D1.PartNo AND N1.ProductionOrderNo=D1.ProductionOrderNo AND ISNULL(N1.GrnNo,'')=ISNULL(D1.GRNNo,'')
                                        LEFT JOIN ScheduleDetails_GEA S1 ON S1.MaterialID=D1.PartNo AND S1.ProductionOrder=D1.ProductionOrderNo AND ISNULL(S1.GrnNo,'')=ISNULL(S1.GrnNo,'') where D1.GRNDate >= @FromDate and D1.GRNDate <=@ToDate";
                            break;
                        }
                }
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@FromDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate.ToString("yyyy-MM-dd hh:mm:ss"))).ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate.ToString("yyyy-MM-dd hh:mm:ss"))).ToString("yyyy-MM-dd hh:mm:ss"));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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

        #region ----- Quality Test Report ----        internal static List<string> getComponentsForQuality(string machine, string productionOrder)        {            List<string> list = new List<string>();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select distinct MaterialID  from QualityIncomingActionDetails_GEA where MachineID=@MachineID and ProductionOrderNo=@ProductionOrderNo", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@MachineID", machine);                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);                rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    list.Add(rdr["MaterialID"].ToString());                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return list;        }        internal static List<string> getOperationForQuality(string machine, string productionOrder, string component)        {            List<string> list = new List<string>();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select distinct operationNo  from QualityIncomingActionDetails_GEA where MachineID=@MachineID and ProductionOrderNo=@ProductionOrderNo and MaterialID=@MaterialID", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@MachineID", machine);                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);                cmd.Parameters.AddWithValue("@MaterialID", component);                rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    list.Add(rdr["operationNo"].ToString());                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return list;        }        internal static List<string> getPlanAndRevNoForQuality(string machine, string productionOrder, string component, string operationNo)        {            List<string> list = new List<string>();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select distinct PlanAndRevNo  from QualityIncomingActionDetails_GEA where MachineID=@MachineID and ProductionOrderNo=@ProductionOrderNo and MaterialID=@MaterialID and OperationNo=@OperationNo", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@MachineID", machine);                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);                cmd.Parameters.AddWithValue("@MaterialID", component);                cmd.Parameters.AddWithValue("@OperationNo", operationNo);                rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    list.Add(rdr["PlanAndRevNo"].ToString());                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return list;        }



















































































































































































        #endregion
        #region ----- Quality Test Protocol Report ----        internal static DataTable GetQualityTestProtocol(string machine, string productionOrder, string materialId, string operationNo, string planNo, string grnNumber, out DataTable dt2)        {            DataTable dt = new DataTable();            dt2 = new DataTable();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand("[S_ViewQualityInspectionDetails_GEA]", conn);                cmd.CommandType = CommandType.StoredProcedure;                cmd.Parameters.AddWithValue("@Param", "Report");                cmd.Parameters.AddWithValue("@ProdOrderNo", productionOrder);                cmd.Parameters.AddWithValue("@PartID", materialId);                cmd.Parameters.AddWithValue("@OpnNo", operationNo);                cmd.Parameters.AddWithValue("@MachineID", machine);                cmd.Parameters.AddWithValue("@PlanAndRevNo", planNo);
                cmd.Parameters.AddWithValue("@GrnNo", grnNumber);                rdr = cmd.ExecuteReader();                if (rdr.HasRows)                {                    dt.Load(rdr);                    dt2.Load(rdr);                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return dt;        }
        //        internal static string GetQualityTestProtocolCompDesc(string materialId,string injk)
        //        {
        //            string desc = "";
        //            SqlConnection conn = ConnectionManager.GetConnection();
        //            SqlCommand cmd = null;
        //            SqlDataReader rdr = null;
        //            try
        //            {
        //                cmd = new SqlCommand(@"Select  substring(description,charindex('[',description,2)+1,len(description)-charindex(']',description)-2) as ModelDescription
        //From componentinformation where componentid = @compID", conn);
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@compID", materialId);
        //                rdr = cmd.ExecuteReader();
        //                if (rdr.HasRows)
        //                {
        //                    while (rdr.Read())
        //                    {
        //                        desc = rdr["ModelDescription"].ToString();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.WriteErrorLog(ex.ToString());
        //            }
        //            finally
        //            {
        //                if (conn != null) conn.Close();
        //                if (rdr != null) rdr.Close();
        //            }
        //            return desc;
        //        }
        internal static DataTable getTestProtocolPODetailsForFinalReport(string productionOrder)        {            DataTable dt = new DataTable();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand("select * from QualityIncomingActionDetails_GEA where ProductionOrderNo=@ProdOrderNo and MachineID=@MachineID", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@MachineID", "Quality In house");                cmd.Parameters.AddWithValue("@ProdOrderNo", productionOrder);                rdr = cmd.ExecuteReader();                dt.Load(rdr);            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return dt;        }
        #endregion

        #region Production Order Status

        internal static DataTable GetProdOrderStatus(string proType, DateTime FromDate, DateTime ToDate, string fabNumber)
        {
            SqlConnection conn = ConnectionManager.GetConnection();

            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                string Query = "";
                switch (proType)
                {
                    case "Pro":
                        {
                            Query = @"[dbo].[S_GetNonMachiningReportCompletionDetails_Pro_GEA]";
                            cmd = new SqlCommand(Query, conn);
                            cmd.Parameters.AddWithValue("@FabNo", fabNumber);
                            break;
                        }
                    case "NonPro":
                        {
                            Query = @"[dbo].[S_GetNonMachiningReportCompletionDetails_GEA]";
                            cmd = new SqlCommand(Query, conn);
                            cmd.Parameters.AddWithValue("@StartDate", FromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@EndDate", ToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@FabNo", fabNumber);
                            break;
                        }
                }
                //cmd = new SqlCommand(Query, conn);
                //cmd.Parameters.AddWithValue("@StartDate", FromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@EndDate", ToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@FabNo", fabNumber);
                cmd.CommandType = CommandType.StoredProcedure;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
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
        //internal static List<ProdFabricationNumber> GetProductionOrderStatusFabricationNum(string proType, string FromDate, string ToDate)
        internal static List<string> GetProductionOrderStatusFabricationNum(string proType, string FromDate, string ToDate)
        {
            //List<ProdFabricationNumber> FabNo = new List<ProdFabricationNumber>();
            List<string> FabNo = new List<string>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                string Query = "";
                switch (proType)
                {
                    case "Pro":
                        {
                            Query = @"select distinct ScheduleDetails_GEA.machineid,ProductionOrder,FabricationNo,MaterialID from ScheduleDetails_GEA 
	inner join machineinformation M on M.machineid=ScheduleDetails_GEA.Machineid
	where M.Process='Assembly' and ScheduleDetails_GEA.MaterialID  like '%pro%' and 
	ScheduleStart>=@Fromdate and ScheduleStart<=@ToDate";
                            break;
                        }
                    case "NonPro":
                        {
                            Query = @"select distinct ScheduleDetails_GEA.machineid,ProductionOrder,FabricationNo,MaterialID from ScheduleDetails_GEA 
	inner join machineinformation M on M.machineid=ScheduleDetails_GEA.Machineid
	where M.Process='Assembly' and ScheduleDetails_GEA.MaterialID not like '%pro%' and 
	ScheduleStart>=@Fromdate and ScheduleStart<=@ToDate";
                            break;
                        }
                }
                cmd = new SqlCommand(Query, conn);
                //cmd.Parameters.AddWithValue("@StartDate", FromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                //cmd.Parameters.AddWithValue("@EndDate", ToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Fromdate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(FromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(ToDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    FabNo.Add(rdr["FabricationNo"].ToString());
                    //FabNo.Add(new ProdFabricationNumber
                    //{
                    //    FabricationNumber = rdr["FabricationNo"].ToString(),

                    //});
                }
                //FabNo.Insert(0, new ProdFabricationNumber() { FabricationNumber = "All" });
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
            return FabNo;
        }
        internal static string getListBoxValueWithSingleQuote(ListBox listBox)
        {
            string FabNum = "";
            try
            {
                foreach (ListItem item in listBox.Items)
                {
                    if (item.Selected)
                    {
                        if (FabNum == "")
                            FabNum += "'" + item.Value + "'";
                        else
                            FabNum += ",'" + item.Value + "'";
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
            return FabNum;
        }
        internal static string getDropdownValueWithSingleQuote(DropDownList dropDownList)
        {
            string fabNumber = "";
            try
            {
                if (dropDownList.SelectedValue == "All")
                {
                    foreach (ListItem item in dropDownList.Items)
                    {
                        //if (item.Selected)
                        //{
                        if (fabNumber == "")
                            fabNumber += "'" + item.Value + "'";
                        //fabNumber += "'" + item + "'";
                        else
                            fabNumber += ",'" + item.Value + "'";
                        //fabNumber += ",'" + item + "'";
                        //}
                    }
                }
                else
                {
                    fabNumber += "'" + dropDownList.SelectedValue + "'";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineIDWithSeparator = " + ex.ToString());
            }
            finally
            {
            }
            return fabNumber;
        }
        internal static string GetAssemblyReprotData(string Type, string fabNumber, string prodOrder)
        {
            SqlConnection conn = ConnectionManager.GetConnection();

            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            string value = "No";
            try
            {
                string Query = "";
                switch (Type)
                {
                    case "Assembly":
                        {
                            Query = "select Confirmation from BlueCardTransaction_GEA where OrderNo=@OrderNo and FabricationNo=@FabricationNo and ParameterID='Bowl Hub'";
                            break;
                        }
                    case "Testing":
                        {
                            Query = "select Confirmation from BlueCardTransaction_GEA where OrderNo=@OrderNo and FabricationNo=@FabricationNo and ParameterID='Dynamically balanced________________________Test protocol'";
                            break;
                        }
                }
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@OrderNo", prodOrder);
                cmd.Parameters.AddWithValue("@FabricationNo", fabNumber);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    value = rdr["Confirmation"].ToString().Equals("0") ? "No" : "Yes";
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
            return value;
        }

        #endregion

        internal static List<MachinIdProcessEnity> getMachineIDProcessList()        {            List<MachinIdProcessEnity> list = new List<MachinIdProcessEnity>();            MachinIdProcessEnity data = null;            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select machineid,Process from machineinformation", conn);                cmd.CommandType = CommandType.Text;                rdr = cmd.ExecuteReader();                while (rdr.Read())                {                    data = new MachinIdProcessEnity();                    data.MachineID = rdr["machineid"].ToString();                    data.Process = rdr["Process"].ToString();                    list.Add(data);                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return list;        }

        #region -------- CE Checklist ----------
        internal static List<ProdFabricationNumber> getProductionOrderListForCEChecklist()
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {

                string Query = "select distinct ProductionOrder,FabricationNumber from CEChecklistDetails_GEA  where Confirmation=1";
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        FabricationNumber = rdr["FabricationNumber"].ToString(),
                        ProdOrderNumber = rdr["ProductionOrder"].ToString()
                    });
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
            return PONo;
        }
        internal static DataTable getCEChecklistReportTblData(string poNumber, string fabricationNo, out DataTable dtSchedule)        {            dtSchedule = new DataTable();            DataTable dt = new DataTable();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select * from ScheduleDetails_GEA s join machineinformation m on s.Machineid=m.machineid where m.Process='Testing' and ProductionOrder=@ProductionOrder
and FabricationNo = @FabricationNo

select * from CEChecklistDetails_GEA where ProductionOrder = @ProductionOrder and FabricationNumber = @FabricationNo", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@ProductionOrder", poNumber);                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNo);                rdr = cmd.ExecuteReader();                if (rdr.HasRows)
                {
                    dtSchedule.Load(rdr);
                    dt.Load(rdr);
                }

            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return dt;        }
        internal static string getMachineTypeForPO(string poNumber, string fabricationNo)        {            string machineType = "";            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select distinct MDA.MachineType from ScheduleDetails_GEA SDG                                inner join MachineDataAssemblyAccessories_GEA MDA on MDA.FabricationNo=SDG.FabricationNo and MDA.OrderNo=SDG.ProductionOrder                                where SDG.ProductionOrder=@prodOrder and SDG.FabricationNo=@FabNo", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@prodOrder", poNumber);                cmd.Parameters.AddWithValue("@FabNo", fabricationNo);                rdr = cmd.ExecuteReader();                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        machineType = rdr["MachineType"].ToString();
                    }
                }

            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return machineType;        }
        #endregion

        #region -------- Pro Decanter Report -------
        internal static List<ProdFabricationNumber> getProductionOrderListForProDecanter()
        {
            List<ProdFabricationNumber> PONo = new List<ProdFabricationNumber>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {

                string Query = @"select distinct OrderNo,FabricationNo from BlueCardTransaction_GEA where Confirmation=2 and 
OrderNo in (select distinct ProductionOrder from ProDecanterDetails_Gea)";
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PONo.Add(new ProdFabricationNumber
                    {
                        FabricationNumber = rdr["FabricationNo"].ToString(),
                        ProdOrderNumber = rdr["OrderNo"].ToString()
                    });
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
            return PONo;
        }
        internal static DataTable getProDecanterReportData(string poNumber, string fabricationNo, out DataTable dtSchedule)        {            dtSchedule = new DataTable();            DataTable dt = new DataTable();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select * from ScheduleDetails_GEA where ProductionOrder=@ProductionOrder and FabricationNo = @FabricationNo

select * from ProDecanterDetails_Gea where ProductionOrder=@ProductionOrder and FabricationNumber=@FabricationNo order by UpdatedTS ", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@ProductionOrder", poNumber);                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNo);                rdr = cmd.ExecuteReader();                if (rdr.HasRows)
                {
                    dtSchedule.Load(rdr);
                    dt.Load(rdr);
                }

            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return dt;        }
        internal static DataTable getProDecanterAssemblyReportData(string poNumber, string fabricationNo, string machineID)        {            DataTable dt = new DataTable();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"[dbo].[S_GetMachineDataAssemblyReportProDecanter_GEA]", conn);                cmd.CommandType = CommandType.StoredProcedure;                cmd.Parameters.AddWithValue("@OrderNo", poNumber);                cmd.Parameters.AddWithValue("@FabricationNumber", fabricationNo);
                cmd.Parameters.AddWithValue("@MachineID", machineID);                rdr = cmd.ExecuteReader();                if (rdr.HasRows)
                {
                    rdr.NextResult();
                    dt.Load(rdr);
                }

            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return dt;        }
        internal static string getMachineIDProDecanterAssemblyReportData(string poNumber, string fabricationNo)        {            string machineId = "";            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            try            {                cmd = new SqlCommand(@"select distinct MachineID from MachineDataAssemblyDescriptionProDecanter_GEA where OrderNo=@OrderNo and FabricationNo=@FabricationNo", conn);                cmd.CommandType = CommandType.Text;                cmd.Parameters.AddWithValue("@OrderNo", poNumber);                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNo);                rdr = cmd.ExecuteReader();                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        machineId = rdr["MachineID"].ToString();
                    }
                }

            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return machineId;        }
        #endregion

        #region ------- Noise measurement dimension data -----
        internal static List<NoiseDimensionModel> getNoiseMeasurementDimensionMasterDara()
        {
            List<NoiseDimensionModel> ModelData = new List<NoiseDimensionModel>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand("select m.ComponentID,d.Dimension,d.Value,d.FormulaSet from NonMachining_SubOperation_GEA m left join NoiseMeasurementFormulaMaster_GEA d on m.ComponentID=d.MaterialID", conn);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        NoiseDimensionModel data = new NoiseDimensionModel();
                        data.Model = rdr["ComponentID"].ToString();
                        data.Dimension = rdr["Dimension"].ToString();
                        data.Value = rdr["Value"].ToString();
                        ModelData.Add(data);
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
            return ModelData;
        }
        internal static int saveNoiseMeasurementDimensionMasterDara(NoiseDimensionModel data)
        {
            int result = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {
                cmd = new SqlCommand(@"if exists (select * from NoiseMeasurementFormulaMaster_GEA where MaterialID=@MaterialID)
begin
 update NoiseMeasurementFormulaMaster_GEA set Dimension=@Dimension,Value=@Value  where MaterialID=@MaterialID
end
else
begin
  insert into NoiseMeasurementFormulaMaster_GEA(MaterialID,Dimension,Value)
  values(@MaterialID,@Dimension,@Value)
end", conn);
                cmd.Parameters.AddWithValue("@MaterialID", data.Model);
                cmd.Parameters.AddWithValue("@Dimension", data.Dimension);
                cmd.Parameters.AddWithValue("@Value", data.Value);
                result = cmd.ExecuteNonQuery();
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
            return result;
        }
        #endregion

        #region Quality PC Report

        internal static DataTable GetQualityPCTimeConsolidatedReport(string machineID, string fromDate, string toDate)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            try
            {
                string Query = @"[dbo].[SP_ViewReportOccurances_GEA]";
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", "TimeConsolidated");
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
        internal static DataTable GetQualityPCTYearlyReport(string machineID, string year)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader rdr = null;
            SqlCommand cmd = null;
            try
            {
                string Query = @"[dbo].[SP_ViewReportOccurances_GEA]";
                cmd = new SqlCommand(Query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Param", "Year");
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

        #region ---Machine Mix Report---

        internal static DataTable GetMachineMixDetails(DateTime fromDate, DateTime toDate, out DataTable dTotal)
        {
            DataTable dt = new DataTable();            SqlConnection conn = ConnectionManager.GetConnection();            SqlCommand cmd = null;            SqlDataReader rdr = null;            dTotal = new DataTable();            try            {                cmd = new SqlCommand(@"[S_GetMachineMixReport_GEA]", conn);                cmd.CommandType = CommandType.StoredProcedure;                cmd.Parameters.AddWithValue("@FromDate", fromDate);                cmd.Parameters.AddWithValue("@ToDate", toDate);                rdr = cmd.ExecuteReader();                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                    dt.AcceptChanges();
                    dTotal.Load(rdr);
                }

            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex.ToString());            }            finally            {                if (conn != null) conn.Close();                if (rdr != null) rdr.Close();            }            return dt;
        }

        #endregion

        #region ----Parked Order Reason---
        internal static DataTable GetParkedOrderReasons(string cellID, string machineID, DateTime fromDate, DateTime toDate)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[S_GetParkedOrderReason_GEA]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 1800;
                cmd.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineIDs", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                rdr.Close();
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog("GetParkedOrderReasons: " + e.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return dt;
        }
        #endregion
        #region-------------Production Schedule Report------------
        internal static DataTable GetProductionScheduleData(string FromDate, string ToDate, string machineId, string status, string prodOrder, string materialID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"[SP_ScheduleReportView_GEA]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fromdate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(FromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ToDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(ToDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@MachineID", machineId);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ProductionOrder", prodOrder);
                cmd.Parameters.AddWithValue("@MaterialID", materialID);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (sdr != null) sdr.Close();
            }
            return dt;
        }
        #endregion
        #region--------Monthly Operator Efficiency Report Data--------------
        internal static DataTable GetMonthlyOperatorEfficiencyData(string startTime, string endTime, string shift, string plantID, string operatorID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"[S_GetAgg_OperatorWise_OEEReport_GEA]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@YearNo", year);
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(startTime)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(endTime)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift);
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@OperatorID", operatorID);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
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

        internal static List<string> GetOperatorIDs(string plant)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> Operator = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct e.employeeid from employeeinformation e left join PlantEmployee p on e.Employeeid=p.EmployeeID where p.PlantID=@plant or (ISNULL(@plant,'')='') ", con);
                //                cmd = new SqlCommand(@"select distinct e.Employeeid, e.Name from employeeinformation e left join PlantEmployee p on
                //e.Employeeid=p.EmployeeID where p.PlantID=@plant or 
                //(ISNULL (@plant,'')='')", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Operator.Add(sdr["employeeid"].ToString());
                        //Operator.Add(sdr["Name"].ToString());
                    }
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
            return Operator;
        }
        internal static List<Operator> GetOperatorName(string plant)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<Operator> OperatorNames = new List<Operator>();
            Operator data = null;
            try
            {
                //cmd = new SqlCommand(@"select distinct e.employeeid from employeeinformation e left join PlantEmployee p on e.Employeeid=p.EmployeeID where p.PlantID=@plant or (ISNULL(@plant,'')='') ", con);
                cmd = new SqlCommand(@"select distinct e.Employeeid, e.Name from employeeinformation e left join PlantEmployee p on
e.Employeeid=p.EmployeeID where p.PlantID=@plant or 
(ISNULL (@plant,'')='')", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@plant", plant.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plant);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new Operator();
                        data.OperatorID = sdr["Employeeid"].ToString();
                        data.OperatorName = sdr["Name"].ToString();
                        OperatorNames.Add(data);
                    }
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
            return OperatorNames;
        }
        #endregion

        #region--------------Model std.Time vs Actual Report Data-----------
        internal static DataTable GetModelStdVsActualData(string FromDate, string ToDate, string ProductionNum, string FabNo, string processType)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"[S_Get_Model_StdVsActualReport_GEA]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(FromDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EndTime", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(ToDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ProcessType", processType);
                cmd.Parameters.AddWithValue("@ProductionOrder", ProductionNum);
                cmd.Parameters.AddWithValue("@FabricationNo", FabNo);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
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
        #region-------------MachineWie Assembly Report-------------
        internal static DataTable GetMachineWiseAssemblyData(string fabNum, out DataTable data)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            data = new DataTable();
            try
            {
                cmd = new SqlCommand(@"[S_Get_MachineWiseAssemblyReport_GEA]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FabricationNo", fabNum);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
                data.Load(sdr);
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
            return data;
        }
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
        #region------------ComponentID & prodorder for ProductionScheduleReport-----------
        internal static List<string> GetAllComponentbyMachineforProductionSchedule(string Machineid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allComps = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Machineid))
                    sqlQuery = "select distinct MaterialID from ScheduleDetails_GEA where Machineid=@Machineid";
                else
                    sqlQuery = "select distinct MaterialID from ScheduleDetails_GEA";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                SqlDataReader rdr = cmd.ExecuteReader();
                allComps.Add("All");
                //while (rdr.Read())
                //{
                //    allComps.Add(new ProdFabricationNumber
                //    {
                //        MaterialNumber = rdr["MaterialID"].ToString(),
                //    });
                //}
                while (rdr.Read())
                {
                    allComps.Add(Convert.ToString(rdr["MaterialID"]));
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
        internal static List<string> GetAllProdOrderbyMachineforProductionSchedule(string Machineid)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> allComps = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Machineid))
                    sqlQuery = "select distinct ProductionOrder from ScheduleDetails_GEA where Machineid=@Machineid";
                else
                    sqlQuery = "select distinct ProductionOrder from ScheduleDetails_GEA";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Machineid", Machineid);
                SqlDataReader rdr = cmd.ExecuteReader();
                allComps.Add("All");
                //while (rdr.Read())
                //{
                //    allComps.Add(new ProdFabricationNumber
                //    {
                //        ProdOrderNumber = rdr["ProductionOrder"].ToString(),
                //    });
                //}
                while (rdr.Read())
                {
                    allComps.Add(Convert.ToString(rdr["ProductionOrder"]));
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
        #endregion
        #region----------------Target Screen Data---------------
        internal static List<TargetValues> GetTargetScreenData(string plantID, string Name)
        //internal static DataTable GetTargetScreenData(string plantID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<TargetValues> data = new List<TargetValues>();
            TargetValues Values = null;
            //DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(@"select E.Employeeid,E.Name,P.PlantID,T.TargetValue from employeeinformation E left join 
PlantEmployee P on E.Employeeid=P.EmployeeID left join
OperatorLevelOEETarget T on E.Employeeid=T.OperatorID 
where P.PlantID=@PlantID  and E.Name like '%" + Name + "%'", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Values = new TargetValues();
                        Values.EmployeeID = sdr["Employeeid"].ToString();
                        Values.EmployeeName = sdr["Name"].ToString();
                        Values.TargetValue = sdr["TargetValue"].ToString();
                        data.Add(Values);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            //return dt;
            return data;
        }
        internal static int SaveTargetScreenData(TargetValues data)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from OperatorLevelOEETarget where OperatorID=@OperatorID )
begin
	insert into OperatorLevelOEETarget (PlantID,OperatorID,TargetValue)
	values(@PlantID,@OperatorID,@TargetValue)
end
else
begin
	update OperatorLevelOEETarget  set PlantID=@PlantID,TargetValue=@TargetValue where OperatorID=@OperatorID
end", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@OperatorID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@TargetValue", data.TargetValue);
                cmd.Parameters.AddWithValue("@PlantID", data.PlantID);
                sdr = cmd.ExecuteReader();
                result++;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region -------- Auto Schedule Master ---------
        internal static List<AutoScheduleMasterEntity> getAutoScheduleMasterDetails(string compID, string operationNo, string machineID)
        {
            List<AutoScheduleMasterEntity> list = new List<AutoScheduleMasterEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            AutoScheduleMasterEntity data = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_GetAssemblyScheduleMaster_GEA]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ComponentNo", compID);
                cmd.Parameters.AddWithValue("@OperationNo", operationNo.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : operationNo);
                cmd.Parameters.AddWithValue("@MachineID", machineID);
                cmd.Parameters.AddWithValue("@Param", "View");
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AutoScheduleMasterEntity();
                        data.ComponentID = sdr["componentid"].ToString();
                        data.OperationNo = sdr["operationno"].ToString();
                        data.MachineID = sdr["machineid"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static string saveAutoScheduleMasterDetauls(AutoScheduleMasterEntity data)
        {
            string result = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[S_GetAssemblyScheduleMaster_GEA]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ComponentNo", data.ComponentID);
                cmd.Parameters.AddWithValue("@OperationNo", data.OperationNo);
                cmd.Parameters.AddWithValue("@OldMachine", data.OldMachineID);
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);
                cmd.Parameters.AddWithValue("@Param", "Save");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        result = sdr["SaveFlag"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return result;
        }
        internal static List<string> getAssemblyMachines(string process)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct machineid from machineinformation where Process in (" + process + ")", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
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
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static List<string> getOperationNoByMultiComp(string machineID, string compID)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct operationno from componentoperationpricing  where componentid in (" + compID + ") and (machineid=@machineid or isnull(@machineid,'')='')", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineid", machineID);
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
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
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static List<string> getAssemblyProcessComponentID()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct componentid from componentoperationpricing cop inner join machineinformation m on cop.machineid=m.machineid where m.Process in ('Assembly','Balancing')", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
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
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static List<AutoScheduleMasterEntity> getToBeSentToStoreMasterDetails(string compID, string operationNo, string machineID)
        {
            List<AutoScheduleMasterEntity> list = new List<AutoScheduleMasterEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            AutoScheduleMasterEntity data = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select componentid,operationno,machineid,ToBeSentToStores from componentoperationpricing  where componentid in (" + compID + ") and (operationno=@operationno or ISNULL(@operationno,'')='')  and (machineid=@machineid or isnull(@machineid,'')='')", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@operationno", operationNo.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : operationNo);
                cmd.Parameters.AddWithValue("@machineid", machineID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : machineID);
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AutoScheduleMasterEntity();
                        data.ComponentID = sdr["componentid"].ToString();
                        data.OperationNo = sdr["operationno"].ToString();
                        data.MachineID = sdr["machineid"].ToString();
                        data.ToBeSentToStores = sdr["ToBeSentToStores"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static int saveToBeSentToStoreMasterDetails(AutoScheduleMasterEntity data)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"update componentoperationpricing set ToBeSentToStores=@ToBeSentToStores where componentid=@componentid and 
operationno=@operationno and machineid=@machineid", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@componentid", data.ComponentID);
                cmd.Parameters.AddWithValue("@operationno", data.OperationNo);
                cmd.Parameters.AddWithValue("@machineid", data.MachineID);
                cmd.Parameters.AddWithValue("@ToBeSentToStores", data.ToBeSentToStores);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return result;
        }
        #endregion

        #region ---- Child Part Master data --------
        internal static List<string> getTopProductionOrderData()
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT distinct top 100 ProductionOrder,UpdatedTS
FROM (
  SELECT ProductionOrder, MAX(UpdatedTS) AS UpdatedTS
  FROM ScheduleDetails_GEA where Machineid='Stores'
  GROUP BY ProductionOrder
) t
ORDER BY UpdatedTS DESC", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["ProductionOrder"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static List<string> getFabricationNumberByPO(string productionOrder)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct FabricationNo from ScheduleDetails_GEA where ProductionOrder=@ProductionOrder", con);
                cmd.Parameters.AddWithValue("@ProductionOrder", productionOrder);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["FabricationNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static DataTable getChildPartMasterDetails(string productionOrder, string fabricationNo)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            AutoScheduleMasterEntity data = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from StoresItemList_GEA where ProductionOrderNo=@ProductionOrderNo and FabricationNo=@FabricationNo order by MaterialID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNo);
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return dt;
        }
        internal static void deleteChildPartMasterTempTable(string tableName)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            AutoScheduleMasterEntity data = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"delete from " + tableName, con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 450;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
        }
        public static string insertImportedChildPartMasterDataToTempTable(DataTable dt, string importType)
        {
            string result = "";
            SqlBulkCopy bulkCopy = default(SqlBulkCopy);
            SqlConnection con = ConnectionManager.GetConnection();
            string conString = WebConfigurationManager.ConnectionStrings[HttpContext.Current.Session["connectionString"].ToString()].ToString();
            try
            {
                bulkCopy = new SqlBulkCopy(conString);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = "[dbo].[StoresItemListTemp_GEA]";
                //bulkCopy.ColumnMappings.Add("ItemSortorder", "ItemSortorder");
                bulkCopy.ColumnMappings.Add("ProductionOrderNo", "ProductionOrderNo");
                bulkCopy.ColumnMappings.Add("PagedRequirement", "PagedRequirement");
                bulkCopy.ColumnMappings.Add("MaterialID", "MaterialID");
                bulkCopy.ColumnMappings.Add("MaterialDescription", "MaterialDescription");
                bulkCopy.ColumnMappings.Add("ReservationItem", "ReservationItem");
                bulkCopy.ColumnMappings.Add("RequirementQuantity", "RequirementQuantity");
                bulkCopy.ColumnMappings.Add("BaseUnitOfMeasure", "BaseUnitOfMeasure");
                bulkCopy.ColumnMappings.Add("QuantityWithDrawn", "QuantityWithDrawn");
                bulkCopy.ColumnMappings.Add("Shortage", "Shortage");
                bulkCopy.ColumnMappings.Add("ProdStorageBin", "ProdStorageBin");
                bulkCopy.ColumnMappings.Add("StorageLocation", "StorageLocation");
                bulkCopy.ColumnMappings.Add("ItemCategory", "ItemCategory");
                bulkCopy.ColumnMappings.Add("ProcurementType", "ProcurementType");
                bulkCopy.ColumnMappings.Add("SparePartIndicator", "SparePartIndicator");
                bulkCopy.ColumnMappings.Add("OpenQuantity", "OpenQuantity");
                bulkCopy.ColumnMappings.Add("FabricationNo", "FabricationNo");
                bulkCopy.NotifyAfter = 20;
                bulkCopy.SqlRowsCopied += delegate (object sender, SqlRowsCopiedEventArgs e)
                {
                    Logger.WriteDebugLog(string.Format("ChildPart Master: Row insertion Notified : {0} rows copied to Table dbo.StoresItemListTemp_GEA .", e.RowsCopied));
                };
                bulkCopy.WriteToServer(dt);
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();
                result = callChildPartMasterDataInsertProc(importType);
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (bulkCopy != null) bulkCopy.Close();
                if (con != null) con.Close();

            }
            return result;
        }
        public static string callChildPartMasterDataInsertProc(string importType)
        {
            string result = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[S_ImportStoreItemList_GEA]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", importType);
                cmd.CommandTimeout = 360;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = "error";
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }












































































        #endregion
        #region-----------GEA Material Tracking---------------          internal static List<ListItem> GetAllStoresEmployeesID()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ListItem> allEmps = new List<ListItem>();
            try
            {
                string sqlQuery = "select distinct EmployeeID,Name from EmployeeInformation where EmployeeRole='Store Assistant'";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                // allEmps.Add("All");
                while (rdr.Read())
                {
                    allEmps.Add(new ListItem() { Value = rdr["EmployeeID"].ToString(), Text = rdr["Name"].ToString() });
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
            return allEmps;
        }        internal static List<MaterialTracking_GEA> GetMaterialTracking_GEA(string fromDate, string toDate, string status)        {            List<MaterialTracking_GEA> list = new List<MaterialTracking_GEA>();            MaterialTracking_GEA data = null;            SqlConnection con = ConnectionManager.GetConnection();            SqlDataReader sdr = null;            SqlCommand cmd = null;            try            {                cmd = new SqlCommand(@"[dbo].[S_GetStoresTraceability_GEA]", con);                cmd.CommandType = CommandType.StoredProcedure;                cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(fromDate)).ToString("yyyy-MM-dd HH:mm:ss"));                cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(toDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Status", status.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : status);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.CommandTimeout = 450;                sdr = cmd.ExecuteReader();                if (sdr.HasRows)                {                    int i = 1;                    while (sdr.Read())                    {                        data = new MaterialTracking_GEA();                        data.SNo = i.ToString();                        data.DateOfSchedule = Convert.IsDBNull(sdr["Date"]) ? "" : Convert.ToDateTime(sdr["Date"].ToString()).ToString("dd-MM-yyyy");
                        data.CycleTime = Convert.IsDBNull(sdr["CycleTime"]) ? "" : Convert.ToDateTime(sdr["CycleTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");                        data.MachineID = sdr["MachineID"].ToString();                        data.ProductionOrderNo = sdr["ProductionOrderNo"].ToString();                        data.seriesNo = sdr["SeriesNo"].ToString();                        data.TimeWaitingAtStores = sdr["TimeWaitingAtStores"].ToString();                        data.WhoHasReceived = sdr["ReceiverID"].ToString();
                        data.ReceiverName = sdr["ReceiverName"].ToString();                        data.ReceiptCompletion = data.WhoHasReceived == "" ? false : true;                        data.DateTimeCompletion = Convert.IsDBNull(sdr["CompletionDateTime"]) ? "" : Convert.ToDateTime(sdr["CompletionDateTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");                        data.CompInterfaceID = sdr["InterfaceID"].ToString();
                        data.CompID = sdr["ComponentID"].ToString();
                        data.CompDesc = sdr["ComponentDescription"].ToString();                        data.Status = sdr["Status"].ToString();                        list.Add(data);                        i++;                    }                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex);            }            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }            return list;        }        internal static int SaveMaterialTracking(MaterialTracking_GEA data)        {            int result = 0;            SqlConnection con = ConnectionManager.GetConnection();            SqlDataReader sdr = null;            SqlCommand cmd = null;            try            {                cmd = new SqlCommand(@"[dbo].[S_GetStoresTraceability_GEA]", con);                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineID", data.MachineID);                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(data.DateOfSchedule).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ProductionOrderNo", data.ProductionOrderNo);
                cmd.Parameters.AddWithValue("@SeriesNo", data.seriesNo);
                cmd.Parameters.AddWithValue("@ReceiverName", data.WhoHasReceived);
                cmd.Parameters.AddWithValue("@CompletionDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@CycleTime", Util.GetDateTime(data.CycleTime).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@InterfaceID", data.CompInterfaceID);
                cmd.Parameters.AddWithValue("@ComponentID", data.CompID);
                cmd.Parameters.AddWithValue("@ComponentDescription", data.CompDesc);
                cmd.Parameters.AddWithValue("@Param", "Save");                sdr = cmd.ExecuteReader();                if (sdr.HasRows)
                {
                    result++;
                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex);            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }            return result;        }
        internal static string getWeekStartAndEndDate(string date, out string endDate)
        {
            string startDate = "";
            endDate = "";
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"declare @WeekNumber int=0;
SELECT @WeekNumber=WeekNumber FROM Calender 
		WHERE WeekDate=@Date
SELECT MIN(WeekDate) as StartDate,MAX(WeekDate) as EndDate  FROM Calender 
		WHERE MonthVal=DATEPART(MONTH,@Date) AND [YearNo]=DATEPART(YEAR,@Date) AND WeekNumber=@WeekNumber", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Date", Util.GetDateTime(date).ToString("yyyy-MM-dd"));
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        startDate = Convert.ToDateTime(sdr["StartDate"].ToString()).ToString("dd-MM-yyyy");
                        endDate = Convert.ToDateTime(sdr["EndDate"].ToString()).ToString("dd-MM-yyyy");
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
            return startDate;
        }
        internal static TraceabilityDashboardEntity getMaterialTraceabilityDashboardDetails(string viewType)        {
            TraceabilityDashboardEntity traceabilityData = new TraceabilityDashboardEntity();
            SqlConnection con = ConnectionManager.GetConnection();            SqlDataReader sdr = null;            SqlCommand cmd = null;            try            {                cmd = new SqlCommand(@"[dbo].[S_GetStoresTraceabilityDashboard_GEA]", con);                cmd.CommandType = CommandType.StoredProcedure;                if (viewType.Equals("week", StringComparison.OrdinalIgnoreCase))
                {
                    string endDate = "";
                    string startDate = getWeekStartAndEndDate(DateTime.Now.ToString("dd-MM-yyyy"), out endDate);
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(startDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(endDate)).ToString("yyyy-MM-dd HH:mm:ss"));
                }                else
                {
                    DateTime monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    cmd.Parameters.AddWithValue("@StartDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(monthStart.ToString("dd-MM-yyyy"))).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@EndDate", Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(monthStart.AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy"))).ToString("yyyy-MM-dd HH:mm:ss"));
                }                //cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Param", viewType);                cmd.CommandTimeout = 450;                sdr = cmd.ExecuteReader();                if (sdr.HasRows)                {                    while (sdr.Read())                    {
                        traceabilityData.StockAvailability = HelperClassGeneric.getDoubleValueFromString(sdr["CompletionCount"].ToString());
                        traceabilityData.TotalDownTime = sdr["TimeWaiting"].ToString();
                        traceabilityData.ReceiptCompletion = HelperClassGeneric.getDoubleValueFromString(sdr["CompletionPercent"].ToString());                    }                    sdr.NextResult();

                    List<MaterialTracking_GEA> storesList = new List<MaterialTracking_GEA>();
                    int i = 1;
                    while (sdr.Read())                    {
                        MaterialTracking_GEA data = new MaterialTracking_GEA();
                        data.SNo = i.ToString();
                        data.DateOfSchedule = Convert.IsDBNull(sdr["Date"]) ? "" : Convert.ToDateTime(sdr["Date"].ToString()).ToString("dd-MM-yyyy");
                        data.CycleTime = Convert.IsDBNull(sdr["CycleTime"]) ? "" : Convert.ToDateTime(sdr["CycleTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        data.MachineID = sdr["MachineID"].ToString();
                        data.ProductionOrderNo = sdr["ProductionOrderNo"].ToString();
                        data.seriesNo = sdr["SeriesNo"].ToString();
                        data.TimeWaitingAtStores = sdr["TimeWaitingAtStores"].ToString();
                        data.WhoHasReceived = sdr["ReceiverID"].ToString();                        data.ReceiverName = sdr["ReceiverName"].ToString();                        data.DateTimeCompletion = Convert.IsDBNull(sdr["CompletionDateTime"]) ? "" : Convert.ToDateTime(sdr["CompletionDateTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");                        data.CompID = sdr["ComponentID"].ToString();
                        data.CompDesc = sdr["ComponentDescription"].ToString();                        storesList.Add(data);                        i++;                    }
                    sdr.NextResult();

                    TraceabilityChartEntity weeklyChartData = new TraceabilityChartEntity();
                    List<string> categoryList = new List<string>();
                    List<double> dataList = new List<double>();
                    double monthLevelCompletionCount = 0;
                    while (sdr.Read())                    {
                        if (viewType.Equals("month", StringComparison.OrdinalIgnoreCase))
                        {
                            monthLevelCompletionCount += HelperClassGeneric.getDoubleValueFromString(sdr["CompletionCount"].ToString());
                        }
                        else
                        {
                            categoryList.Add("Week " + sdr["WeekNumber"].ToString());
                            dataList.Add(HelperClassGeneric.getDoubleValueFromString(sdr["CompletionCount"].ToString()));
                        }
                    }
                    if (viewType.Equals("month", StringComparison.OrdinalIgnoreCase))
                    {
                        categoryList.Add(HelperClassGeneric.getAbbreviatedMonthName(DateTime.Now.Month.ToString()));
                        dataList.Add(monthLevelCompletionCount);
                    }
                    weeklyChartData.Category = categoryList;
                    weeklyChartData.Data = dataList;
                    sdr.NextResult();

                    TraceabilityChartEntity materialStatusChartData = new TraceabilityChartEntity();
                    categoryList = new List<string>();
                    dataList = new List<double>();
                    List<double> dataList2 = new List<double>();
                    while (sdr.Read())                    {
                        categoryList.Add(sdr["Component"].ToString());
                        dataList.Add(HelperClassGeneric.getDoubleValueFromString(sdr["TotalCount"].ToString()));
                        dataList2.Add(HelperClassGeneric.getDoubleValueFromString(sdr["CompletionCount"].ToString()));
                    }
                    materialStatusChartData.Category = categoryList;
                    materialStatusChartData.Data = dataList;
                    materialStatusChartData.Data2 = dataList2;

                    traceabilityData.StoresList = storesList;
                    traceabilityData.WeeklyChartData = weeklyChartData;
                    traceabilityData.MaterialStatusChartData = materialStatusChartData;
                }            }            catch (Exception ex)            {                Logger.WriteErrorLog(ex);            }            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }            return traceabilityData;        }
        #endregion
        internal static List<string> getMachineIDBasedOnAssembly(string assembly)
        {
            List<string> list = new List<string>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                string query = "";
                if (assembly == "")
                {
                    query = @"select distinct machineid from machineinformation ";
                }
                else
                {
                    query = @"select distinct machineid from machineinformation where Process in (" + assembly + ")";
                }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
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
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static List<ListItem> GetComponentIDWithDescByMachine(string MachineID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<ListItem> list = new List<ListItem>();
            string Query = @"";
            if (string.IsNullOrEmpty(MachineID) || MachineID.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                Query = @"select distinct c.componentid,c.description from componentoperationpricing cop inner join componentinformation c on cop.componentid=c.componentid";
            }
            else
            {
                Query = @"select distinct c.componentid,c.description from componentoperationpricing cop inner join componentinformation c on cop.componentid=c.componentid where machineid=@MachineID";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(Query, sqlConn);//
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ListItem() { Value = rdr["componentid"].ToString(), Text = rdr["componentid"].ToString() + "-" + rdr["description"].ToString() });
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
            return list;
        }
        internal static string getProcessOfMachine(string machineID)
        {
            string process = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select Process from machineinformation where machineid=@machineid", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@machineID", machineID);
                sdr = cmd.ExecuteReader();
                //dt.Load(sdr);
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        process = sdr["Process"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return process;
        }

        #region ----- Receipt Completion Target ----
        internal static List<ReceiptCompletionTargetEntity> getReceiptCompletionTargetMasterDeatils(string year)
        {
            List<ReceiptCompletionTargetEntity> list = new List<ReceiptCompletionTargetEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct c.YearNo,c.WeekNumber,r.Target from Calender c left join RCTarget_GEA  r on c.YearNo=r.YearNo and c.WeekNumber=r.WeekNo
where c.YearNo=@YearNo", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@YearNo", year);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ReceiptCompletionTargetEntity data = new ReceiptCompletionTargetEntity();
                        data.Year = sdr["YearNo"].ToString();
                        data.WeekNo = sdr["WeekNumber"].ToString();
                        data.Target = sdr["Target"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return list;
        }
        internal static int saveReceiptCompletionTargetMasterDeatils(ReceiptCompletionTargetEntity data)
        {
            int result = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"if not exists(select * from RCTarget_GEA where YearNo=@YearNo and WeekNo=@WeekNo)
begin
	insert into RCTarget_GEA(YearNo,WeekNo,Target) values(@YearNo,@WeekNo,@Target)
end
else
begin
	update RCTarget_GEA set Target=@Target  where YearNo=@YearNo and WeekNo=@WeekNo
end
", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@YearNo", data.Year);
                cmd.Parameters.AddWithValue("@WeekNo", data.WeekNo);
                cmd.Parameters.AddWithValue("@Target", data.Target);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally            {                if (con != null) con.Close();                if (sdr != null) sdr.Close();            }
            return result;
        }
        #endregion

        #region ---- Picking list report / missing report 
        internal static List<MissingItemEntity> getPickingListReport(string productionOrder, string fabricationNo, string componentID)
        {
            List<MissingItemEntity> list = new List<MissingItemEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"StoresItemListViewAndSave_GEA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNo);
                cmd.Parameters.AddWithValue("@ComponentID", componentID);
                cmd.Parameters.AddWithValue("@Param", "View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        var xx = sdr["DateOfMissing"];
                        var yy = sdr["DateOfReIssue"];

                        MissingItemEntity data = new MissingItemEntity();
                        data.Item = sdr["ReservationItem"].ToString();
                        data.MaterialID = sdr["MaterialID"].ToString();
                        data.MaterialDesc = sdr["MaterialDescription"].ToString();
                        data.Qty = sdr["RequirementQuantity"].ToString();
                        data.DateOfMissing = sdr["DateOfMissing"] == DBNull.Value ? "" : (DateTime.Compare(Convert.ToDateTime(sdr["DateOfMissing"]), new DateTime(1900, 1, 1)) == 0 ? "" : Convert.ToDateTime(sdr["DateOfMissing"].ToString()).ToString("yyyy-MM-dd"));
                        data.IsIssued = sdr["Checked"] == DBNull.Value ? false : Convert.ToBoolean(sdr["Checked"].ToString());
                        data.Status = sdr["Status"].ToString();
                        data.DateOfReIssue = sdr["DateOfReIssue"] == DBNull.Value ? "" : (DateTime.Compare(Convert.ToDateTime(sdr["DateOfReIssue"]), new DateTime(1900, 1, 1)) == 0 ? "" : Convert.ToDateTime(sdr["DateOfReIssue"].ToString()).ToString("yyyy-MM-dd"));
                        data.Operator = sdr["UpdatedBy"].ToString();
                        data.OperatorName = sdr["EmployeeName"].ToString();
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
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }

        internal static List<MissingItemEntity> getPickingListMissingReport(string productionOrder, string fabricationNo, string componentID)
        {
            List<MissingItemEntity> list = new List<MissingItemEntity>();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"StoresItemListViewAndSave_GEA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionOrder);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNo);
                cmd.Parameters.AddWithValue("@ComponentID", componentID);
                cmd.Parameters.AddWithValue("@Param", "MissingItemListView");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        MissingItemEntity data = new MissingItemEntity();
                        data.Item = sdr["ReservationItem"].ToString();
                        data.MaterialID = sdr["MaterialID"].ToString();
                        data.MaterialDesc = sdr["MaterialDescription"].ToString();
                        data.Qty = sdr["RequirementQuantity"].ToString();
                        data.DateOfMissing = sdr["DateOfMissing"] == DBNull.Value ? "" : (DateTime.Compare(Convert.ToDateTime(sdr["DateOfMissing"]), new DateTime(1900, 1, 1)) == 0 ? "" : Convert.ToDateTime(sdr["DateOfMissing"].ToString()).ToString("yyyy-MM-dd"));
                        data.IsIssued = sdr["Checked"] == DBNull.Value ? false : Convert.ToBoolean(sdr["Checked"].ToString());
                        data.Status = sdr["Status"].ToString();
                        data.DateOfReIssue = sdr["DateOfReIssue"] == DBNull.Value ? "" : (DateTime.Compare(Convert.ToDateTime(sdr["DateOfReIssue"]), new DateTime(1900, 1, 1)) == 0 ? "" : Convert.ToDateTime(sdr["DateOfReIssue"].ToString()).ToString("yyyy-MM-dd"));
                        data.Operator = sdr["UpdatedBy"].ToString();
                        data.OperatorName = sdr["Name"].ToString();
                        data.ShortageQty = sdr["ShortageQty"].ToString();
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
                if (con != null) con.Close();
                if (sdr != null) sdr.Close();
            }
            return list;
        }
        internal static DataTable getPickingReportHeaderData(string productionOrder)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from ScheduleDetails_GEA where ProductionOrder=@ProductionOrder and Machineid='Stores'", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductionOrder", productionOrder);
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
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
        internal static string getPickinglistComponentID(string productionNo, string fabricationNo)
        {
            string compID = string.Empty;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand(@"select distinct ComponentID from StoresItemTransactionSave_GEA where ProductionOrderNo=@ProductionOrderNo and FabricationNo=@FabricationNo", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductionOrderNo", productionNo);
                cmd.Parameters.AddWithValue("@FabricationNo", fabricationNo);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        compID = sdr["ComponentID"].ToString();
                    }
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
            return compID;
        }
        #endregion

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

    }
}