using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Pooja.Model
{
    public static class PoojaDBAccess
    {
        #region -----ANDON Setting ----
        internal static AndonSettingDetails GetANDONSettings()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            AndonSettingDetails settings = new AndonSettingDetails();
            try
            {
                cmd = new SqlCommand(@"select * from ShopDefaults where Parameter = 'PoojaAndonSettings'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {

                        if (sdr["ValueInText"].Equals("ScreenFlipInterval"))
                        {
                            if (!string.IsNullOrEmpty(sdr["ValueInInt"].ToString()))
                            {
                                settings.ScreenFlipInterval = Convert.ToInt32(sdr["ValueInInt"].ToString());
                            }
                        }
                        else if (sdr["ValueInText"].Equals("NoOfMachineToDisplay"))
                        {
                            if (!string.IsNullOrEmpty(sdr["ValueInInt"].ToString()))
                            {
                                settings.NoOfMachinesToDisplay = Convert.ToInt32(sdr["ValueInInt"].ToString());
                            }
                        }
                        else if (sdr["ValueInText"].Equals("FontFamily"))
                        {
                            settings.FontFamily = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("TableHeaderFontSize"))
                        {
                            settings.TableHeaderFontSize = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("TableContentFontSize"))
                        {
                            settings.TableContentFontSize = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("FontBold"))
                        {
                            settings.FontBold = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("MainHeaderName"))
                        {
                            settings.MainHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("MachineHeaderName"))
                        {
                            settings.MachineHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("ComponentAndOpnHeaderName"))
                        {
                            settings.ComponentAndOpnHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("OEEHeaderName"))
                        {
                            settings.OEEHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("DownTimeHeaderName"))
                        {
                            settings.DownTimeHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("OperatorHeaderName"))
                        {
                            settings.OperatorHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("ProductionTargetHeaderName"))
                        {
                            settings.ProductionTargetHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("ActualHeaderName"))
                        {
                            settings.ActualHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("StatusHeaderName"))
                        {
                            settings.StatusHeaderName = (sdr["ValueInText2"]).ToString();
                        }
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("AndonSettingDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return settings;
        }
        internal static void SaveANDONSettings(AndonSettingDetails data)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;

            AndonSettingDetails settings = null;
            try
            {
                cmd = new SqlCommand(@"if exists (select * from ShopDefaults where ValueInText = 'ScreenFlipInterval' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set ValueInInt =@ScreenFlipInterval where ValueInText = 'ScreenFlipInterval' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInInt) values( 'PoojaAndonSettings','ScreenFlipInterval',@ScreenFlipInterval )
end

if exists (select * from ShopDefaults where ValueInText = 'NoOfMachineToDisplay' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInInt =@NoOfMachineToDisplay  where ValueInText = 'NoOfMachineToDisplay' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInInt) values( 'PoojaAndonSettings','NoOfMachineToDisplay',@NoOfMachineToDisplay )
end


if exists (select * from ShopDefaults where ValueInText = 'FontFamily' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@FontFamily  where ValueInText = 'FontFamily' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','FontFamily',@FontFamily )
end

if exists (select * from ShopDefaults where ValueInText = 'TableHeaderFontSize' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@TableHeaderFontSize  where ValueInText = 'TableHeaderFontSize' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','TableHeaderFontSize',@TableHeaderFontSize )
end


if exists (select * from ShopDefaults where ValueInText = 'TableContentFontSize' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@TableContentFontSize  where ValueInText = 'TableContentFontSize' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','TableContentFontSize',@TableContentFontSize )
end

if exists (select * from ShopDefaults where ValueInText = 'MainHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@MainHeaderName  where ValueInText = 'MainHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','MainHeaderName',@MainHeaderName )
end


if exists (select * from ShopDefaults where ValueInText = 'MachineHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@MachineHeaderName  where ValueInText = 'MachineHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','MachineHeaderName',@MachineHeaderName )
end

if exists (select * from ShopDefaults where ValueInText = 'ComponentAndOpnHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@ComponentAndOpnHeaderName  where ValueInText = 'ComponentAndOpnHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','ComponentAndOpnHeaderName',@ComponentAndOpnHeaderName )
end

if exists (select * from ShopDefaults where ValueInText = 'OEEHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@OEEHeaderName  where ValueInText = 'OEEHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','OEEHeaderName',@OEEHeaderName )
end

if exists (select * from ShopDefaults where ValueInText = 'DownTimeHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@DownTimeHeaderName  where ValueInText = 'DownTimeHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','DownTimeHeaderName',@DownTimeHeaderName )
end


if exists (select * from ShopDefaults where ValueInText = 'OperatorHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@OperatorHeaderName  where ValueInText = 'OperatorHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','OperatorHeaderName',@OperatorHeaderName )
end

if exists (select * from ShopDefaults where ValueInText = 'ProductionTargetHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@ProductionTargetHeaderName  where ValueInText = 'ProductionTargetHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','ProductionTargetHeaderName',@ProductionTargetHeaderName )
end

if exists (select * from ShopDefaults where ValueInText = 'StatusHeaderName' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@StatusHeaderName  where ValueInText = 'StatusHeaderName' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','StatusHeaderName',@StatusHeaderName )
end

if exists (select * from ShopDefaults where ValueInText = 'FontBold' and Parameter = 'PoojaAndonSettings')
begin
Update ShopDefaults set  ValueInText2 =@FontBold  where ValueInText = 'FontBold' and Parameter = 'PoojaAndonSettings'
end
else
begin
insert into ShopDefaults(Parameter, ValueInText, ValueInText2) values( 'PoojaAndonSettings','FontBold',@FontBold )
end

", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@FontFamily", data.FontFamily);
                cmd.Parameters.AddWithValue("@TableHeaderFontSize", data.TableHeaderFontSize);
                cmd.Parameters.AddWithValue("@TableContentFontSize", data.TableContentFontSize);
                cmd.Parameters.AddWithValue("@ScreenFlipInterval", data.ScreenFlipInterval);
                cmd.Parameters.AddWithValue("@NoOfMachineToDisplay", data.NoOfMachinesToDisplay);
                cmd.Parameters.AddWithValue("@MainHeaderName", data.MainHeaderName);
                cmd.Parameters.AddWithValue("@MachineHeaderName", data.MachineHeaderName);
                cmd.Parameters.AddWithValue("@ComponentAndOpnHeaderName", data.ComponentAndOpnHeaderName);
                cmd.Parameters.AddWithValue("@OEEHeaderName", data.OEEHeaderName);
                cmd.Parameters.AddWithValue("@DownTimeHeaderName", data.DownTimeHeaderName);
                cmd.Parameters.AddWithValue("@OperatorHeaderName", data.OperatorHeaderName);
                cmd.Parameters.AddWithValue("@ProductionTargetHeaderName", data.ProductionTargetHeaderName);
                cmd.Parameters.AddWithValue("@ActualHeaderName", data.ActualHeaderName);
                cmd.Parameters.AddWithValue("@StatusHeaderName", data.StatusHeaderName);
                cmd.Parameters.AddWithValue("@FontBold", data.FontBold);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveSettings: " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        #endregion
        #region -----ANDON Details ----
        internal static List<ProductionDetails> GetANDONDetails(DateTime date, string shift, string plant, string cell)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<ProductionDetails> list = new List<ProductionDetails>();
            ProductionDetails data = null;
            try
            {
                cmd = new SqlCommand(@"[s_GetAndonDetails_Pooja]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shift);
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupID", cell);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new ProductionDetails();
                        data.Machine = sdr["MachineID"].ToString();
                        data.Component = sdr["RunningComp"].ToString();
                        data.Operation = sdr["RunningOpn"].ToString();
                        data.OEE = sdr["OEE"].ToString();
                        data.Downtime = sdr["DownTime"].ToString();
                        data.Operator = sdr["LatestOprName"].ToString();
                        data.ProductionTarget = sdr["ProductionTarget"].ToString();
                        data.Actual = sdr["ActualParts"].ToString();
                        data.StatusColor = sdr["StatusColor"].ToString();
                        list.Add(data);
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetANDONDetails: " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        #endregion
        #region-----------------Pooja Andon Master-----------------
        internal static List<GradeMasterEntity> getGradeIDDetail()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<GradeMasterEntity> list = new List<GradeMasterEntity>();
            try
            {
                cmd = new SqlCommand(@"select * from GradeIDMaster_PoojaCasting order by GradeID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    int i = 0;
                    while (sdr.Read())
                    {
                        GradeMasterEntity entity = new GradeMasterEntity();
                        entity.SlNo = (++i).ToString();
                        entity.GradeID = sdr["GradeID"].ToString();

                        list.Add(entity);
                    }
                }
            }
            catch(Exception ex)
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

        internal static List<string> GetParameters()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select ParameterID from ParameterDetails_PoojaCasting", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["ParameterID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }

        internal static string SaveGradeID(string GradeID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string result = string.Empty;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from GradeIDMaster_PoojaCasting where GradeID=@GradeID)
begin
insert into GradeIDMaster_PoojaCasting(GradeID) values(@GradeID)
select 'inserted' as SaveFlag
end
else
begin
select 'exists' as SaveFlag
end", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@GradeID", GradeID);
                result = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }

        internal static string DeleteGradeID(string GradeID)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string result = "";
            try
            {
                cmd = new SqlCommand(@"if exists(select * from MachineParameterDetails_PoojaCasting where GradeID=@GradeID)
begin
select 'InTransaction' as SaveFlag
end
else
begin
delete from GradeIDMaster_PoojaCasting where GradeID=@GradeID
select 'Deleted' as SaveFlag
end", conn);
                cmd.Parameters.AddWithValue("@GradeID", GradeID);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                result = cmd.ExecuteScalar().ToString();
                
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

        internal static List<string> GetGradeIDs()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct GradeID from GradeIDMaster_PoojaCasting order by GradeID", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["GradeID"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return list;
        }
        internal static List<PoojaAndonMasterDetails> GetPoojaAndonMasteretails(string gradeID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PoojaAndonMasterDetails> list = new List<PoojaAndonMasterDetails>();
            PoojaAndonMasterDetails data = null;
            try
            {
                cmd = new SqlCommand(@"select * from ParameterBandwith_PoojaCasting where GradeID=@GradeID order by GradeID ", con);
                cmd.Parameters.AddWithValue("@GradeID", gradeID);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new PoojaAndonMasterDetails();
                        data.Parameter = sdr["ParameterID"].ToString();
                        data.GradeID = sdr["GradeID"].ToString();
                        data.CompareType = sdr["CompareType"].ToString();
                        data.LSL = sdr["LSL"].ToString();
                        data.USL = sdr["USL"].ToString();
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
        internal static bool UpdatePoojaAndonMaster(PoojaAndonMasterDetails data)
        {
            bool IsUpdated = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from ParameterBandwith_PoojaCasting where ParameterID=@ParameterID and GradeID=@GradeID)
begin
	insert into ParameterBandwith_PoojaCasting(ParameterID,GradeID,CompareType,LSL,USL)
	values(@ParameterID,@GradeID,@CompareType,@LSL,@USL)
end
else
begin
	update ParameterBandwith_PoojaCasting set CompareType=@CompareType,LSL=@LSL,USL=@USL where ParameterID=@ParameterID and GradeID=@GradeID
end", con);
                cmd.Parameters.AddWithValue("@ParameterID", data.Parameter);
                cmd.Parameters.AddWithValue("@GradeID", data.GradeID);
                cmd.Parameters.AddWithValue("@LSL", data.LSL);
                if (string.IsNullOrEmpty(data.USL))
                {
                    cmd.Parameters.AddWithValue("@USL", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@USL", data.USL);
                }
                cmd.Parameters.AddWithValue("@CompareType", data.CompareType);
                //cmd.Parameters.AddWithValue("@CompareType",data.CompareType);
                //if (data.USL != null && data.USL != "")
                //{
                //    cmd.Parameters.AddWithValue("@CompareType", "Range");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@CompareType", "Target");
                //}
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                IsUpdated = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return IsUpdated;
        }
        //internal static void deletePoojaAndonTempTable(string tableName)
        //{
        //    SqlConnection con = ConnectionManager.GetConnection();
        //    SqlDataReader sdr = null;
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(@"delete from " + tableName, con);
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandTimeout = 450;
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //    finally
        //    {
        //        if (con != null) con.Close();
        //        if (sdr != null) sdr.Close();
        //    }
        //}
        //        public static string insertPoojaAndonMasterDataToTempTable(List<PoojaAndonMasterDetails> list)
        //        {
        //            SqlConnection con = ConnectionManager.GetConnection();
        //            SqlCommand cmd = null;
        //            SqlDataReader sdr = null;
        //            string result = "";
        //            list = new List<PoojaAndonMasterDetails>();
        //            try
        //            {
        //                cmd = new SqlCommand(@"if not exists(select * from ParameterBandwith_PoojaCasting where ParameterID=@ParameterID and GradeID=@GradeID)
        //begin
        //	insert into ParameterBandwith_PoojaCasting(ParameterID,GradeID,CompareType,LSL,USL)
        //	values(@ParameterID,@GradeID,@CompareType,@LSL,@USL)
        //end
        //else
        //begin
        //	update ParameterBandwith_PoojaCasting set CompareType=@CompareType,LSL=@LSL,USL=@USL where ParameterID=@ParameterID and GradeID=@GradeID
        //end", con);
        //                cmd.Parameters.AddWithValue("@ParameterID", data.Parameter);
        //                cmd.Parameters.AddWithValue("@GradeID", data.GradeID);
        //                cmd.Parameters.AddWithValue("@CompareType", data.CompareType);
        //                cmd.Parameters.AddWithValue("@LSL", data.LSL);
        //                cmd.Parameters.AddWithValue("@USL", data.USL);
        //                cmd.CommandType = CommandType.Text;
        //                sdr = cmd.ExecuteReader();
        //                if (sdr.HasRows)
        //                {
        //                    while (sdr.Read())
        //                    {

        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.WriteErrorLog(ex);
        //            }
        //            return result;
        //        }
        internal static string DeletePoojaMasterData(PoojaAndonMasterDetails data)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int count = 0;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"delete from ParameterBandwith_PoojaCasting where ParameterID=@ParameterID and GradeID=@GradeID
", con);
                cmd.Parameters.AddWithValue("@ParameterID", data.Parameter);
                cmd.Parameters.AddWithValue("@GradeID", data.GradeID);
                cmd.CommandType = CommandType.Text;
                count = cmd.ExecuteNonQuery();
                if (count > 0)
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
        #region-------Pooja Andon Parameter Master--------------------
        internal static List<PoojaParameterMasterDetails> GetPoojaParameterMasterDetails()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<PoojaParameterMasterDetails> list = new List<PoojaParameterMasterDetails>();
            PoojaParameterMasterDetails data = null;
            try
            {
                cmd = new SqlCommand(@"select * from dbo.ParameterDetails_PoojaCasting  order by SortOrder asc", con);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new PoojaParameterMasterDetails();
                        data.ParameterID = sdr["ParameterID"].ToString();
                        data.ParameterDesc = sdr["Description"].ToString();
                        data.SortOrder = Convert.ToInt32(sdr["SortOrder"].ToString());
                        data.IsEnabled = Convert.ToBoolean(sdr["IsEnabled"].ToString());
                        //if (sdr["IsEnabled"].ToString()=="1")
                        //{
                        //    data.IsEnabled = true;
                        //}
                        //else
                        //{
                        //    data.IsEnabled = false;
                        //}
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
        internal static bool UpdateParameterSettings(PoojaParameterMasterDetails data)
        {
            bool IsUpdated = false;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from ParameterDetails_PoojaCasting where ParameterID=@ParameterID)
begin
	insert into ParameterDetails_PoojaCasting(ParameterID,Description,SortOrder,IsEnabled)
	values(@ParameterID,@Description,@SortOrder,@IsEnabled)
end
else
begin
	update ParameterDetails_PoojaCasting set Description=@Description,SortOrder=@SortOrder,IsEnabled=@IsEnabled where ParameterID=@ParameterID 
end", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ParameterID", data.ParameterID);
                cmd.Parameters.AddWithValue("@Description", data.ParameterDesc);
                cmd.Parameters.AddWithValue("@SortOrder", data.SortOrder);
                cmd.Parameters.AddWithValue("@IsEnabled", data.IsEnabled);
                sdr = cmd.ExecuteReader();
                IsUpdated = true;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return IsUpdated;
        }
        internal static string DeleterPoojaAndonParameter(PoojaParameterMasterDetails data)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int count = 0;
            string success = "";
            try
            {
                cmd = new SqlCommand(@"delete from ParameterDetails_PoojaCasting where ParameterID=@ParameterID", con);
                cmd.Parameters.AddWithValue("@ParameterID", data.ParameterID);
                cmd.CommandType = CommandType.Text;
                count = cmd.ExecuteNonQuery();
                if (count > 0)
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
    }

}