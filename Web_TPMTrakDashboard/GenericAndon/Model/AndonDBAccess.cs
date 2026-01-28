using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GenericAndon.Model
{
    public class AndonDBAccess
    {

        #region ----- Andon Setting -----
        internal static EffyColorEntity GetCockpitBackColorValues()
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            EffyColorEntity vals = new EffyColorEntity();
            try
            {
                cmd = new SqlCommand("select * from Cockpitdefaults where parameter = 'CockpitBackColor'", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if ((sdr["ValueInText"].ToString()).Equals("GoodColor"))
                        {
                            vals.GoodColor = sdr["ValueInText2"].ToString();
                        }
                        else if ((sdr["ValueInText"].ToString()).Equals("ModerateColor"))
                        {
                            vals.ModerateColor = sdr["ValueInText2"].ToString();
                        }
                        else if ((sdr["ValueInText"].ToString()).Equals("BadColor"))
                        {
                            vals.BadColor = sdr["ValueInText2"].ToString();
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
            return vals;
        }

        internal static SettingsViewEntity ViewSettings(string computerName, string ProcName, string Parameter)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            SettingsViewEntity settings = null;
            try
            {
                cmd = new SqlCommand(ProcName, sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(sdr);

                    settings = new SettingsViewEntity();

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["ValueInText"].Equals("AndonTitle"))
                        {
                            settings.AndonTitle = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueInText"].Equals("PlantToDisplay"))
                        {
                            settings.PlantToDisplay = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueInText"].Equals("DataDisplayInterval"))
                        {
                            settings.DataDisplayInterval = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueInText"].Equals("ScreenFlipInterval"))
                        {
                            settings.ScreenFlipInterval = (row["ValueInText2"]).ToString();
                        }

                        else if (row["ValueInText"].Equals("ShowSmileyBlock"))
                        {
                            settings.ShowSmileyBlock = Convert.ToInt32(row["ValueInInt"]);
                        }

                        else if (row["ValueInText"].Equals("MsgBlockEnabled"))
                        {
                            settings.MsgBlockEnabled = Convert.ToInt32(row["ValueInInt"]);
                        }

                        else if (row["ValueInText"].Equals("ShowFooterBlock"))
                        {
                            settings.ShowFooterBlock = Convert.ToInt32(row["ValueInInt"]);
                        }
                        else if (row["ValueInText"].Equals("FontFamily"))
                        {
                            settings.FontFamily = (row["ValueInText2"]).ToString();
                        }

                        else if (row["ValueInText"].Equals("FontStyle"))
                        {
                            settings.FontStyle = (row["ValueInText2"]).ToString();
                        }

                        else if (row["ValueInText"].Equals("ShowSmileyBlockSize"))
                        {
                            settings.ShowSmileyBlockSize = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueInText"].Equals("FormFontSize"))
                        {
                            settings.FormFontSize = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueInText"].Equals("ImageFilePath"))
                        {
                            settings.ImageFilePath = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueInText"].Equals("EnableImage"))
                        {
                            settings.EnableImage = Convert.ToInt32(row["ValueInInt"]);
                        }

                        else if (row["ValueInText"].Equals("EnableVideo"))
                        {
                            settings.EnableVideo = Convert.ToInt32(row["ValueInInt"]);
                        }
                        else if (row["ValueInText"].Equals("VideoFilePath"))
                        {
                            settings.VideoFilePath = (row["ValueInText2"]).ToString();
                        }

                        else if (row["ValueIntext"].Equals("ImageFlipInterval"))
                        {
                            settings.ImageFlipInterval = (row["ValueInText2"]).ToString();

                        }

                        else if (row["ValueInText"].Equals("ShowCurvedBox"))
                        {
                            settings.ShowCurvedBoxes = Convert.ToInt32(row["ValueInInt"]);
                        }
                        else if (row["ValueIntext"].Equals("DateFormatForHeader"))
                        {
                            settings.DateFormat = (row["ValueInText2"]).ToString();

                        }
                        else if (row["ValueIntext"].Equals("TimeFormatForHeader"))
                        {
                            settings.TimeFormat = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueIntext"].Equals("PrimaryScreen"))
                        {
                            settings.PrimaryScreen = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueIntext"].Equals("OrderBy"))
                        {
                            settings.OrderBy = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueIntext"].Equals("ScrollingText"))
                        {
                            settings.ScrollingText = (row["ValueInText2"]).ToString();
                        }
                        else if (row["ValueIntext"].Equals("ScheduleInterval"))
                        {
                            settings.ScheduleKTAFlipInterval = (row["ValueInInt"]).ToString();
                        }
                        else if (row["ValueIntext"].Equals("PoojaViewType"))
                        {
                            settings.PoojaViewType = (row["ValueInText2"].ToString());
                        }
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
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return settings;
        }

        internal static List<AndonDefaultsEntity> GetAndonViewSettingsData(string CellID, string computerName, string Parameter)
        {
            List<AndonDefaultsEntity> entities = new List<AndonDefaultsEntity>();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            try
            {

                cmd = new SqlCommand("S_GetAndonParameterSettingsView", conn);
                cmd.Parameters.AddWithValue("@UserID", computerName);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);

                cmd.Parameters.AddWithValue("@CellID", CellID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        AndonDefaultsEntity settingEntity = new AndonDefaultsEntity();
                        //settingEntity.ValueInText = rdr["ValueInText"].ToString()=="OperationID"?"OperationNo" : rdr["ValueInText"].ToString();
                        settingEntity.ValueInText = rdr["ValueInText"].ToString();
                        settingEntity.ValueInText2 = rdr["ValueInText2"].ToString();
                        settingEntity.ValueInInt = string.IsNullOrEmpty(rdr["ValueInInt"].ToString()) ? 0 : Convert.ToInt32(rdr["ValueInInt"].ToString());
                        settingEntity.ValueInBool = Convert.ToBoolean(rdr["ValueInBool"]);
                        settingEntity.LabelFontSize = rdr["LabelFontSize"].ToString();
                        settingEntity.TextAlign = rdr["TextAlign"].ToString();
                        settingEntity.DataFontSize = rdr["DataFontSize"].ToString();
                        settingEntity.CellID = rdr["CellID"].ToString();
                        entities.Add(settingEntity);
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
            return entities;
        }

        internal static void UpdateAndonSettings(SettingsViewEntity settings, string Parameter, string ComputerName, out string isSuccessfull)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            isSuccessfull = string.Empty;
            try
            {
                cmd = new SqlCommand(@"[s_GetAndonUISettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FormFontSize", settings.FormFontSize);
                cmd.Parameters.AddWithValue("@AndonTitle", settings.AndonTitle);
                cmd.Parameters.AddWithValue("@PlantToDisplay", ""); // settings.PlantToDisplay);
                cmd.Parameters.AddWithValue("@DataDisplayInterval", settings.DataDisplayInterval);
                cmd.Parameters.AddWithValue("@ScreenFlipInterval", settings.ScreenFlipInterval);
                cmd.Parameters.AddWithValue("@ShowSmileyBlock", settings.ShowSmileyBlock);
                cmd.Parameters.AddWithValue("@MsgBlockEnabled", settings.MsgBlockEnabled);
                cmd.Parameters.AddWithValue("@ShowFooterBlock", settings.ShowFooterBlock);
                cmd.Parameters.AddWithValue("@FontFamily", settings.FontFamily);
                cmd.Parameters.AddWithValue("@FontStyle", settings.FontStyle);
                cmd.Parameters.AddWithValue("@ShowSmileyBlocksize", settings.ShowSmileyBlockSize);
                cmd.Parameters.AddWithValue("@ImageFilePath", settings.ImageFilePath);
                cmd.Parameters.AddWithValue("@EnableImage", settings.EnableImage);
                cmd.Parameters.AddWithValue("@EnableVideo", settings.EnableVideo);
                cmd.Parameters.AddWithValue("@videoFilePath", settings.VideoFilePath);
                cmd.Parameters.AddWithValue("@ImageFlipInterval", settings.ImageFlipInterval);
                cmd.Parameters.AddWithValue("@ShowCurvedBox", settings.ShowCurvedBoxes);
                cmd.Parameters.AddWithValue("@DateFormatForHeader", settings.DateFormat);
                cmd.Parameters.AddWithValue("@TimeFormatForHeader", settings.TimeFormat);
                //cmd.Parameters.AddWithValue("@primaryScreen", primaryDisplay);
                cmd.Parameters.AddWithValue("@OrderBy", settings.OrderBy);
                cmd.Parameters.AddWithValue("@ScrollingText", settings.ScrollingText);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@Param", "Update");
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.ExecuteNonQuery();

                isSuccessfull = "Successfull";
            }
            catch (Exception ex)
            {
                isSuccessfull = ex.Message;
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }


        internal static void insertUpdateAndonSetting(string Parameter, string ValueInText, string ValueInInt, string computerName)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            SqlCommand cmd;
            try
            {
                query = @"if not exists(select * from Shopdefaults where Parameter = @Parameter and ValueInText=@ValueInText and ISNULL(ValueInText2,'')=ISNULL(@ComputerName, ''))
begin
	  insert into ShopDefaults(Parameter,ValueInText,ValueInText2, ValueInInt,UpdatedTS) values(@Parameter,@ValueInText,@ComputerName,@ValueInInt,@UpdatedTS)
end
else
begin
      update ShopDefaults set ValueInInt = @ValueInInt,UpdatedTS=@UpdatedTS where Parameter = @Parameter and ValueInText=@ValueInText and ValueInText2=@ComputerName
end";
                cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"Parameter", Parameter);
                cmd.Parameters.AddWithValue(@"ValueInText", ValueInText);
                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.Parameters.AddWithValue(@"ValueInInt", Convert.ToInt16(ValueInInt));
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int x = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("insertUpdateAndonSetting : " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static void insertUpdatePoojaAndonSetting(string Parameter, string ValueInText, string ValueInText2)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from Shopdefaults where Parameter = @Parameter and ValueInText=@ValueInText)
begin
	  insert into ShopDefaults(Parameter,ValueInText,ValueInText2,UpdatedTS) values(@Parameter,@ValueInText,@ValueInText2,@UpdatedTS)
end
else
begin
      update ShopDefaults set ValueInText2 = @ValueInText2,UpdatedTS=@UpdatedTS where Parameter = @Parameter and ValueInText=@ValueInText
end", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"Parameter", Parameter);
                cmd.Parameters.AddWithValue(@"ValueInText", ValueInText);
                cmd.Parameters.AddWithValue(@"ValueInText2", ValueInText2);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int x = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }

        internal static void UpdateGridSettingData(string Parameter, string ValueInText, string ValueInText2, string ValueInInt, string ValueInBool, string TextAlign, string LabelFontSize, string DataFontSize, string CellID, string computerName, out string isSuccessfull) //
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            SqlCommand cmd = null;

            try
            {
                if (Parameter.Equals("CockpitBackColor"))
                {
                    //if(System.Web)
                    query = "Update CockpitDefaults set  ValueInText2 = @ValueInText2, UpdatedTS=@UpdatedTS where Parameter = @Parameter and ValueInText = @ValueInText";
                    cmd = new SqlCommand(query, sqlConn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue(@"Parameter", Parameter);
                    cmd.Parameters.AddWithValue(@"ValueInText", ValueInText);
                    cmd.Parameters.AddWithValue(@"ValueInText2", ValueInText2);
                    cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    //if (KTAFlag == 1)
                    {
                        query = @"IF NOT EXISTS(SELECT * FROM AndonDefaults WHERE Parameter = @Parameter AND ValueInText = @ValueInText AND cellId=@cellID AND [User]=@ComputerName)
BEGIN
	INSERT INTO AndonDefaults(Parameter,ValueInText,ValueInText2,ValueInInt,ValueInBool,TextAlign,DataFontSize,LabelFontSize,[User],cellID)
	VALUES (@Parameter,@ValueInText,@ValueInText2,@ValueInInt,@ValueInBool,@TextAlign,@DataFontSize,@LabelFontSize,@ComputerName,@cellID)
END
ELSE
BEGIN
	UPDATE ANDONDefaults SET ValueInText2 = @ValueInText2,ValueInInt = @ValueInInt,ValueInBool = @ValueInBool,TextAlign=@TextAlign,
	DataFontSize=@DataFontSize , LabelFontSize=@LabelFontSize  where Parameter = @Parameter AND ValueInText = @ValueInText 
	AND cellId=@cellID AND [User]=@ComputerName
END";

                        cmd = new SqlCommand(query, sqlConn);
                        cmd.Parameters.AddWithValue("@ComputerName", computerName);
                    }
                    //                    else
                    //                    {
                    //                        query = @"if not exists(select * from AndonDefaults where Parameter = @Parameter and ValueInText = @ValueInText and cellId=@cellID)
                    //begin
                    //Insert into AndonDefaults(Parameter,ValueInText,ValueInText2,ValueInInt,ValueInBool,TextAlign,DataFontSize,LabelFontSize,cellID)values
                    //(@Parameter,@ValueInText,@ValueInText2,@ValueInInt,@ValueInBool,@TextAlign,@DataFontSize,@LabelFontSize,@cellID)
                    //end
                    //else
                    //begin
                    //Update ANDONDefaults set ValueInText2 = @ValueInText2,ValueInInt = @ValueInInt,ValueInBool = @ValueInBool,TextAlign=@TextAlign,
                    //                  DataFontSize=@DataFontSize , LabelFontSize=@LabelFontSize  where Parameter = @Parameter and ValueInText = @ValueInText and cellId=@cellID
                    //end";

                    //                        cmd = new SqlCommand(query, sqlConn);
                    //                    }

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue(@"Parameter", Parameter);
                    cmd.Parameters.AddWithValue(@"ValueInText", ValueInText);
                    cmd.Parameters.AddWithValue(@"ValueInText2", ValueInText2);
                    if (!ValueInInt.Equals(""))
                    {
                        cmd.Parameters.AddWithValue(@"ValueInInt", Convert.ToInt16(ValueInInt));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(@"ValueInInt", "");
                    }
                    cmd.Parameters.AddWithValue(@"TextAlign", TextAlign);
                    cmd.Parameters.AddWithValue(@"DataFontSize", DataFontSize);
                    cmd.Parameters.AddWithValue(@"LabelFontSize", LabelFontSize);
                    cmd.Parameters.AddWithValue(@"cellID", CellID);

                    if (!ValueInBool.Equals(""))
                    {
                        cmd.Parameters.AddWithValue(@"ValueInBool", Convert.ToBoolean(ValueInBool));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(@"ValueInBool", "0");
                    }
                }
                isSuccessfull = "Successfull";
                int x = cmd.ExecuteNonQuery();
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

        public static DataTable GetBoxDimensions(string ComputerName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            string query = @"IF NOT EXISTS(SELECT * FROM AndonDefaults WHERE Parameter='AndonCockpitBoxDimension' AND [User]=@ComputerName)
BEGIN
	SELECT * FROM AndonDefaults WHERE Parameter='AndonCockpitBoxDimension' AND [User]='Master'
END
ELSE 
BEGIN
	SELECT * FROM AndonDefaults WHERE Parameter='AndonCockpitBoxDimension' AND [User]=@ComputerName
END";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"GetBoxDimensions: {ex.Message}");
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        public static bool SaveBoxDimension(string ComputerName, string ValueInText2, string ValueInText, bool ValueInBool = false)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool isSaved = false;
            string query = @"IF NOT EXISTS(SELECT * FROM AndonDefaults WHERE Parameter=@Param AND [User]=@ComputerName AND ValueInText=@ValueInText)
BEGIN
	INSERT INTO AndonDefaults(Parameter, ValueInText, ValueInText2, ValueInBool,[User]) 
	VALUES(@Param, @ValueInText, @ValueInText2, @ValueInBool, @ComputerName)
END
ELSE 
BEGIN
	UPDATE AndonDefaults SET ValueInText2=@ValueInText2, ValueInBool=@ValueInBool 
	WHERE Parameter=@Param AND [User]=@ComputerName AND ValueInText=@ValueInText
END";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue(@"ValueInText2", ValueInText2);
                cmd.Parameters.AddWithValue(@"ValueInText", ValueInText);
                cmd.Parameters.AddWithValue(@"ValueInBool", ValueInBool ? 1 : 0);
                cmd.Parameters.AddWithValue(@"ComputerName", ComputerName);
                cmd.Parameters.AddWithValue(@"Param", "AndonCockpitBoxDimension");
                isSaved = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"SaveBoxDimension: {ex.Message}");
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return isSaved;
        }
        #endregion

        internal static List<string> getCellID(string plantID)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = "";
            if (plantID != "")
            {
                query = "select distinct GroupID from PlantMachineGroups where PlantID=@PlantID";
            }
            else
            {
                query = @"select distinct GroupID from PlantMachineGroups";
            }
            SqlDataReader sdr = null;
            List<string> lineIDList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plantID);
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
                Logger.WriteErrorLog("getCellID - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lineIDList;
        }
        internal static List<string> getPlantID()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = "";
            query = @"select  distinct plantid from PlantInformation order by Plantid";
            SqlDataReader sdr = null;
            List<string> lineIDList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lineIDList.Add(sdr["PlantID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPalntID - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lineIDList;
        }

        internal static List<string> getPlantID_Andon()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = "";
            query = @"select distinct PlantID from PlantInformation where plantid in (select distinct plantid from PlantMachineGroups)";
            SqlDataReader sdr = null;
            List<string> lineIDList = new List<string>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        lineIDList.Add(sdr["PlantID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPlantID_Andon - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lineIDList;
        }

        internal static List<AndonDefaultsEntity> getAndonSettingDetails(string ComputerName, string Parameter)
        {
            List<AndonDefaultsEntity> list = new List<AndonDefaultsEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string query = @"IF NOT EXISTS(select * from ShopDefaults where Parameter = @Parameter and UserID=@ComputerName)
BEGIN
	SELECT * FROM ShopDefaults where Parameter = @Parameter and UserID='Master' 
END
ELSE
BEGIN
	select * from ShopDefaults where Parameter = @Parameter and UserID=@ComputerName
END"; //If TV Level settings are not present, pick Master columns.
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        AndonDefaultsEntity data = new AndonDefaultsEntity();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        data.ValueInText2 = sdr["ValueInText2"].ToString();
                        data.ValueInInt = string.IsNullOrEmpty(sdr["ValueInInt"].ToString()) ? 0 : Convert.ToInt32(sdr["ValueInInt"].ToString());
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getCockpitSettingDetails = " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static List<AndonSettingsEntity> getCockpitSettingDetails(string computerName, string Parameter)
        {
            List<AndonSettingsEntity> list = new List<AndonSettingsEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS(SELECT * FROM AndonDefaults WHERE parameter = @Parameter AND ValueInBool = '1' AND ISNULL([User],'')=ISNULL(@ComputerName,''))
BEGIN
	SELECT ROW_NUMBER() OVER (Partition BY CellID Order By Valueinint) AS SlNO, CellID,[ValueInText],[ValueInText2],[ValueInInt],
	ISNULL(TextAlign,'Left') as TextAlign, ISNULL(DataFontSize,15) as DataFontSize, ISNULL(LabelFontSize,15) as LabelFontSize
	FROM AndonDefaults where parameter = @Parameter AND ValueInBool = '1' AND [User]='Master'
END
ELSE
BEGIN
    SELECT ROW_NUMBER() OVER (Partition BY CellID Order By Valueinint) as SlNO, CellID, [ValueInText], [ValueInText2], [ValueInInt], 
	ISNULL(TextAlign,'Left') as TextAlign, ISNULL(DataFontSize,20) as DataFontSize, ISNULL(LabelFontSize,20) as LabelFontSize
    FROM AndonDefaults where parameter = @Parameter AND ValueInBool = '1' AND ISNULL([User],'')=ISNULL(@ComputerName,'')
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        AndonSettingsEntity data = new AndonSettingsEntity();
                        data.ColumnName = sdr["ValueInText"].ToString();
                        data.DisplayText = sdr["ValueInText2"].ToString();
                        data.TextAlign = sdr["TextAlign"].ToString();
                        data.DataFontSize = sdr["DataFontSize"].ToString();
                        data.LabelFontSize = sdr["LabelFontSize"].ToString();
                        data.CellID = sdr["CellID"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getCockpitSettingDetails = " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static EffyColorEntity getEfficiencyColor()
        {
            EffyColorEntity data = new EffyColorEntity();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"[s_GetAndonUISettings]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FormFontSize", "");
                cmd.Parameters.AddWithValue("@AndonTitle", "");
                cmd.Parameters.AddWithValue("@PlantToDisplay", "");
                cmd.Parameters.AddWithValue("@DataDisplayInterval", "");
                cmd.Parameters.AddWithValue("@ScreenFlipInterval", "");
                cmd.Parameters.AddWithValue("@ShowSmileyBlock", "");
                cmd.Parameters.AddWithValue("@MsgBlockEnabled", "");
                cmd.Parameters.AddWithValue("@ShowFooterBlock", "");
                cmd.Parameters.AddWithValue("@FontFamily", "");
                cmd.Parameters.AddWithValue("@FontStyle", "");
                cmd.Parameters.AddWithValue("@ShowSmileyBlockSize", "");
                cmd.Parameters.AddWithValue("@ImageFilePath", "");
                cmd.Parameters.AddWithValue("@Param", "Colors");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("GoodColor"))
                        {
                            data.GoodColor = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("ModerateColor"))
                        {
                            data.ModerateColor = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("BadColor"))
                        {
                            data.BadColor = (sdr["ValueInText2"]).ToString();
                        }
                    }
                }
                else
                {
                    data.GoodColor = "#FF0000";
                    data.ModerateColor = "#FFFF00";
                    data.BadColor = "#00BF00";
                }
            }
            catch (Exception ex)
            {
                data.GoodColor = "#FF0000";
                data.ModerateColor = "#FFFF00";
                data.BadColor = "#00BF00";
                Logger.WriteErrorLog("getEfficiencyColor = " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return data;
        }
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
        internal static DataTable GetCurrentShiftTimings()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[dbo].[s_GetCurrentShift]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                {
                    dt.Load(rdr);
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
            return dt;
        }
        internal static List<CockpitData> getMachineCockpitData(string machineId, string plantId, string shiftId, string Param, string cellID, string frequency, List<AndonSettingsEntity> MainsettingList, string computerName, string filter, string SortOrder, string Orderby, string ShowSmileyBlock, string SmileySize, bool ShowCurvedBox)
        {
            //List<AndonDefaultsEntity> MainsettingList = getCockpitSettingDetails();
            //settingList = settingList.AsEnumerable().Where(x => x.CellID.Equals(cellID)).ToList();
            EffyColorEntity colorData = getEfficiencyColor();
            DataTable dt = new DataTable();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            string utilizedTime = string.Empty;
            SqlCommand cmd = null;
            CockpitData data = null;
            List<CockpitData> listOfVals = new List<CockpitData>();
            string machines = string.Empty;
            string cells = string.Empty;
            string BackColorParam = "OEE";
            try
            {
                List<string> filtered_list = GetComputerWiseCockpitListKTA(computerName, filter);
                BackColorParam = GetBackColorParam();

                dt = new DataTable();
                cmd = new SqlCommand("s_GetComfitValves_ANDONDetails", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", VDGDataBaseAccess.GetLogicalDayStartForLive(DateTime.Now.ToString()));
                if (filtered_list.Count > 0)
                {
                    if (filter.Equals("RunByMachine", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var li in filtered_list)
                        {
                            if (machines == string.Empty)
                            {
                                machines += "'" + li + "'";
                            }
                            else
                            {
                                machines += ",'" + li + "'";
                            }
                        }
                        cmd.Parameters.AddWithValue("@MachineId", machines);
                        cmd.Parameters.AddWithValue("@GroupID ", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : "'" + cellID + "'");
                    }
                    else if (filter.Equals("RunByCell", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var li in filtered_list)
                        {
                            if (cells == string.Empty)
                            {
                                cells += "'" + li + "'";
                            }
                            else
                            {
                                cells += ",'" + li + "'";
                            }

                        }
                        cmd.Parameters.AddWithValue("@MachineId", machineId);
                        cmd.Parameters.AddWithValue("@GroupID ", cells);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MachineId", machineId);
                    cmd.Parameters.AddWithValue("@GroupID ", (cellID.Equals("All", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(cellID)) ? "" : "'" + cellID + "'");
                }
                //cmd.Parameters.AddWithValue("@StartDate", VDGDataBaseAccess.GetLogicalDayStartForLive("2022-04-20 09:00:00"));
                cmd.Parameters.AddWithValue("@PlantId", plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@Order", Orderby);
                cmd.Parameters.AddWithValue("@AndonType", "CockpitView");
                //cmd.Parameters.AddWithValue("@Frequency ", (frequency.Equals("Shift", StringComparison.OrdinalIgnoreCase) || frequency.Equals("Day", StringComparison.OrdinalIgnoreCase)) ? "" : frequency);
                //cmd.Parameters.AddWithValue("@Shift", shiftId);

                cmd.Parameters.AddWithValue("@ComputerName", computerName);
                cmd.CommandTimeout = 450;
                Logger.WriteDebugLog(cmd.CommandText.ToString());
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> dtColumnList = dt.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
                    foreach (DataRow item in dt.Rows)
                    {
                        data = new CockpitData();
                        CockpitUserControlData listData = null;

                        data.MachineId = Convert.ToString(item["MachineId"]);
                        data.GroupName = Convert.ToString(item["GroupID"]);

                        data.MachineStatus = item["MachineStatus"].ToString();

                        if (BackColorParam.Equals("PE", StringComparison.OrdinalIgnoreCase))
                            data.MachineOEE = Convert.ToString(item["Pecolor"]);
                        else if (BackColorParam.Equals("AE", StringComparison.OrdinalIgnoreCase))
                            data.MachineOEE = Convert.ToString(item["Aecolor"]);
                        else if (BackColorParam.Equals("QE", StringComparison.OrdinalIgnoreCase))
                            data.MachineOEE = Convert.ToString(item["QeColor"]);
                        else if (BackColorParam.Equals("OperatorPE", StringComparison.OrdinalIgnoreCase))
                        {
                            data.MachineOEE = Convert.ToString(item["Oprcolor"]);
                        }
                        else
                            data.MachineOEE = Convert.ToString(item["OeColor"]);


                        //list.MachineOEE = sdr["SmileyColor"].ToString();

                        if (data.MachineStatus.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        {
                            data.StatusImage = "../Image/McStatus/Stopped.gif";
                            // list.MachineOEE = "Red";
                        }
                        else if (data.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            data.StatusImage = "../Image/McStatus/Running.gif";
                            //list.MachineOEE = "Green";
                        }
                        else if (data.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                        {
                            data.StatusImage = "../Image/McStatus/PDT.gif";
                            //list.MachineOEE = "Yellow";
                        }
                        else if (data.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                        {
                            if (data.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || data.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                                data.StatusImage = "../Image/McStatus/ping(4).png";
                            else
                            {
                                data.StatusImage = "../Image/McStatus/ping(2).png";
                                //list.MachineOEE = "white";
                            }
                        }
                        else
                        {
                            //list.MachineOEE = "white";
                        }

                        string backColor = string.Empty, backOEEColor = string.Empty, foreColor = string.Empty, backColorTitle = string.Empty, foreColorTitle = string.Empty;
                        if (data.MachineOEE.Equals("Red", StringComparison.OrdinalIgnoreCase))
                        {
                            backOEEColor = colorData.BadColor == "" ? "#FFFFFF" : colorData.BadColor.Remove(1, 2);
                            data.SmileyImagePath = "../Image/Smileys/Red.png";
                        }
                        else if (data.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        {
                            backOEEColor = colorData.ModerateColor == "" ? "#FFFFFF" : colorData.ModerateColor.Remove(1, 2); ////"Yellow";
                            data.SmileyImagePath = "../Image/Smileys/Yellow.png";
                        }
                        else if (data.MachineOEE.Equals("Green", StringComparison.OrdinalIgnoreCase))
                        {
                            backOEEColor = colorData.GoodColor == "" ? "#FFFFFF" : colorData.GoodColor.Remove(1, 2);////"Green";
                            data.SmileyImagePath = "../Image/Smileys/Green.png";
                        }
                        else
                        {
                            backOEEColor = backColor = "#FFFFFF";
                            data.SmileyImagePath = "../Image/Smileys/Deault.png";
                        }
                        data.BackColor = backOEEColor;
                        data.ShowSmileyBlock = ShowSmileyBlock;
                        data.SmileySize = SmileySize;
                        //data.MachineFontSize = andonSetting.FormFontSize;
                        if (MainsettingList != null)
                            data.MachineFontSize = MainsettingList.AsEnumerable().Where(x => x.ColumnName.Equals("ProductionMachineNameFontSize", StringComparison.OrdinalIgnoreCase)).Where(x => x.CellID.Equals(data.GroupName, StringComparison.OrdinalIgnoreCase)).Select(x => x.LabelFontSize).FirstOrDefault();
                        if (ShowCurvedBox)
                        {
                            data.BorderClass = "addBorder";
                        }
                        else
                        {
                            data.BorderClass = "removeBorder";
                        }
                        if (string.IsNullOrEmpty(machineId))
                        {
                            List<AndonSettingsEntity> settingList = MainsettingList.AsEnumerable().Where(x => x.CellID.Equals(data.GroupName, StringComparison.OrdinalIgnoreCase)).ToList();
                            for (int i = 0; i < settingList.Count; i++)
                            {
                                try
                                {
                                    if (settingList[i].ColumnName.ToString().Equals("OperatorPE", StringComparison.OrdinalIgnoreCase))
                                    {
                                        settingList[i].ColumnName = "OperatorEfficiency";
                                    }
                                    else if (settingList[i].ColumnName.ToString().Equals("OverallOperatorPE", StringComparison.OrdinalIgnoreCase))
                                    {
                                        settingList[i].ColumnName = "OverallOperatorEfficiency";
                                    }
                                    var columnExistence = dtColumnList.Where(k => k.Equals(settingList[i].ColumnName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    if (columnExistence == null)
                                    {
                                        continue;
                                    }

                                    listData = new CockpitUserControlData();
                                    listData.BackColor = string.Empty;
                                    listData.ForeColor = string.Empty;
                                    listData.ForeColorTitle = string.Empty;
                                    listData.BackColorTitle = string.Empty;
                                    backColor = string.Empty;
                                    if (settingList[i].ColumnName.ToString().Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = item["Aecolor"].ToString();
                                        foreColor = "#000000";
                                    }
                                    else if (settingList[i].ColumnName.ToString().Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = item["Pecolor"].ToString();
                                        foreColor = "#000000";
                                    }
                                    else if (settingList[i].ColumnName.ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = item["Oecolor"].ToString();
                                        foreColor = "#000000";
                                    }
                                    else if (settingList[i].ColumnName.ToString().Equals("QualityEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = item["QeColor"].ToString();
                                        foreColor = "#000000";
                                    }
                                    else if (settingList[i].ColumnName.ToString().Equals("OverallOperatorEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = item["Oecolor"].ToString();
                                        foreColor = "#000000";
                                    }
                                    else if (settingList[i].ColumnName.ToString().Equals("OperatorEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = item["Oprcolor"].ToString();
                                        foreColor = "#000000";
                                    }
                                    else
                                    {
                                        backColor = "#FBFBFB";
                                        backColorTitle = "#FBFBFB";
                                        foreColor = "#000000";
                                        foreColorTitle = "#000000";
                                    }

                                    //if (settingList[i].ValueInText.ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                    //    listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(item[settingList[i].ValueInText])) + " " + "%";
                                    //else
                                    if (item[settingList[i].ColumnName].GetType() == typeof(double))
                                    {
                                        if (settingList[i].ColumnName.Equals("components", StringComparison.OrdinalIgnoreCase))
                                        {
                                            listData.LabelValue1 = String.Format("{0:00.##}", Convert.ToDouble(item[settingList[i].ColumnName]));
                                        }
                                        else if (settingList[i].ColumnName.Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                        {
                                            listData.LabelValue1 = Convert.ToInt32(item[settingList[i].ColumnName]).ToString();
                                        }
                                        else
                                        {
                                            listData.LabelValue1 = Math.Round(Convert.ToDouble(item[settingList[i].ColumnName]), 2).ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (settingList[i].ColumnName.ToString().Equals("OperatorName", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string OPName = item[settingList[i].ColumnName].ToString();
                                            if (OPName.Length > 15)
                                            {
                                                listData.LabelValue1 = OPName.Substring(0, 15).Trim();
                                                if (item[settingList[i].ColumnName].ToString().Length > 30)
                                                    listData.LabelValue2 = OPName.Substring(15, 13).Trim() + "..";
                                                else
                                                    listData.LabelValue2 = OPName.Substring(15, OPName.Length - 15).Trim();
                                            }
                                            else
                                                listData.LabelValue1 = item[settingList[i].ColumnName].ToString();
                                        }
                                        else if (settingList[i].ColumnName.ToString().Equals("CompDescription", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string CompDescription = item[settingList[i].ColumnName].ToString();
                                            if (CompDescription.Length > 15)
                                            {
                                                listData.LabelValue1 = CompDescription.Substring(0, 15).Trim();
                                                if (item[settingList[i].ColumnName].ToString().Length > 30)
                                                    listData.LabelValue2 = CompDescription.Substring(15, 13).Trim() + "..";
                                                else
                                                    listData.LabelValue2 = CompDescription.Substring(15, CompDescription.Length - 15).Trim();
                                            }
                                            else
                                                listData.LabelValue1 = item[settingList[i].ColumnName].ToString();
                                        }
                                        else
                                            listData.LabelValue1 = item[settingList[i].ColumnName].ToString();
                                    }

                                    if (backColor.Equals("Red", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = colorData.BadColor.Remove(1, 2); ;
                                    }
                                    else if (backColor.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = colorData.ModerateColor.Remove(1, 2); ;
                                    }
                                    else if (backColor.Equals("Green", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = colorData.GoodColor.Remove(1, 2); ;
                                    }

                                    listData.LabelText = settingList[i].DisplayText;
                                    listData.BackColor = backColor;
                                    listData.BackColorTitle = backColorTitle;
                                    listData.ForeColor = foreColor;
                                    listData.ForeColorTitle = foreColorTitle;
                                    listData.Tag = settingList[i].ColumnName;
                                    listData.FontSizeInnerData = settingList[i].LabelFontSize;
                                    listData.DataFontSize = settingList[i].DataFontSize;
                                    listData.TextAlign = settingList[i].TextAlign;
                                    listData.CellID = settingList[i].CellID;
                                    data.Values.Add(listData);
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteErrorLog("getMachineCockpitData (inner) = " + ex.Message);
                                }
                            }
                            listOfVals.Add(data);
                        }
                    }
                    //listOfVals = GetComputerWiseCockpitListKTA(listOfVals, computerName, filter);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineCockpitData = " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return listOfVals;
        }


        internal static List<CockpitData> getMachineCockpitData_KachMotors(string PlantID, string GroupID, AndonSettingEntity andonSetting, List<AndonSettingsEntity> ParameterSettingsList)
        {
            DataTable dt_Shift = GetCurrentShiftTimings();
            EffyColorEntity colorData = getEfficiencyColor();
            string BackColorParam = "OEE";
            BackColorParam = GetBackColorParam();
            string ShiftStartTime = "", ShiftEndTime = "";

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            List<CockpitData> list = new List<CockpitData>();
            CockpitData data = new CockpitData();

            try
            {
                ShiftStartTime = Convert.ToDateTime(dt_Shift.Rows[0]["StartTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                ShiftEndTime = Convert.ToDateTime(dt_Shift.Rows[0]["EndTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                cmd = new SqlCommand(@"s_GetCockpitData_WithTempTable_eshopx", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PlantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@GroupID", GroupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : GroupID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@param", "");
                cmd.Parameters.AddWithValue("@StartTime", ShiftStartTime);
                cmd.Parameters.AddWithValue("@EndTime", ShiftEndTime);
                cmd.Parameters.AddWithValue("@SortOrder", andonSetting.OrderBy + " " + andonSetting.SortOrder);
                Stopwatch executionTimer = new Stopwatch();
                executionTimer.Start();
                sdr = cmd.ExecuteReader();
                executionTimer.Stop();
                Logger.WriteDebugLog("Andon Proc Execution Time: " + executionTimer.ElapsedMilliseconds.ToString());
                if (sdr.HasRows)
                    dt1.Load(sdr);

                cmd = new SqlCommand(@"s_GetAgg_AndonEffDetails_KachMotors", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PlantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@GroupID", GroupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : GroupID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@Date", (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    dt2.Load(sdr);


                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    List<string> dtColumnList = dt1.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
                    List<string> dtColumnList2 = dt2.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
                    foreach (DataRow item in dt1.Rows)
                    {
                        data = new CockpitData();
                        CockpitUserControlData listData = null;

                        data.MachineId = Convert.ToString(item["MachineId"]);
                        data.GroupName = Convert.ToString(item["GroupID"]);

                        data.MachineStatus = item["Remarks1"].ToString();


                        if (data.MachineStatus.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        {
                            data.StatusImage = "~/Image/McStatus/Stopped.gif";
                            data.MachineOEE = "Red";
                        }
                        else if (data.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            data.StatusImage = "~/Image/McStatus/Running.gif";
                            data.MachineOEE = "Green";
                        }
                        else if (data.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                        {
                            data.StatusImage = "~/Image/McStatus/PDT.gif";
                            data.MachineOEE = "Yellow";
                        }
                        else if (data.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                        {
                            if (data.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || data.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                                data.StatusImage = "~/Image/McStatus/ping(4).png";
                            else
                            {
                                data.StatusImage = "~/Image/McStatus/ping(2).png";
                                data.MachineOEE = "white";
                            }
                        }
                        else
                            data.MachineOEE = "white";


                        string backColor = string.Empty, backOEEColor = string.Empty, foreColor = string.Empty, backColorTitle = string.Empty, foreColorTitle = string.Empty, backPEColor = string.Empty, backOPROEColor = string.Empty;

                        List<AndonSettingsEntity> settingList = ParameterSettingsList.AsEnumerable().Where(x => x.CellID.Equals(data.GroupName, StringComparison.OrdinalIgnoreCase)).ToList();
                        for (int i = 0; i < settingList.Count; i++)
                        {
                            try
                            {
                                var columnExistence = dtColumnList.Where(k => k.Equals(settingList[i].ColumnName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                var columnExistence_2 = dtColumnList2.Where(x => x.Equals(settingList[i].ColumnName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                if (columnExistence == null && columnExistence_2 == null)
                                {
                                    continue;
                                }

                                listData = new CockpitUserControlData();
                                listData.BackColor = string.Empty;
                                listData.ForeColor = string.Empty;
                                listData.ForeColorTitle = string.Empty;
                                listData.BackColorTitle = string.Empty;
                                backColor = string.Empty;
                                //PE Color Settings
                                if (settingList[i].ColumnName.ToString().Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[settingList[i].ColumnName]) > 0)
                                    {
                                        if (Convert.ToDouble(item[settingList[i].ColumnName]) <= Int32.Parse(item["PERed"].ToString()))
                                        {
                                            backPEColor = backColor = colorData.BadColor == "" ? "#FFFFFF" : colorData.BadColor.Remove(1, 2);
                                        }
                                        else if (Convert.ToDouble(item[settingList[i].ColumnName]) >= Int32.Parse(item["PEGreen"].ToString()))
                                        {
                                            backPEColor = backColor = colorData.GoodColor == "" ? "#FFFFFF" : colorData.GoodColor.Remove(1, 2);
                                        }
                                        else
                                        {
                                            backPEColor = backColor = colorData.ModerateColor == "" ? "#FFFFFF" : colorData.ModerateColor.Remove(1, 2);
                                        }
                                    }
                                    else
                                    {
                                        backPEColor = "#FFFFFF";
                                    }
                                    foreColor = "#000000";
                                }

                                //OEE Color Settings
                                else if (settingList[i].ColumnName.ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[settingList[i].ColumnName]) > 0)
                                    {
                                        if (Convert.ToDouble(item[settingList[i].ColumnName]) <= Int32.Parse(item["OERed"].ToString()))
                                            backOEEColor = backColor = colorData.BadColor == "" ? "#FFFFFF" : colorData.BadColor.Remove(1, 2);

                                        else if (Convert.ToDouble(item[settingList[i].ColumnName]) >= Int32.Parse(item["OEGreen"].ToString()))
                                            backOEEColor = backColor = colorData.GoodColor == "" ? "#FFFFFF" : colorData.GoodColor.Remove(1, 2);

                                        else
                                            backOEEColor = backColor = colorData.ModerateColor == "" ? "#FFFFFF" : colorData.ModerateColor.Remove(1, 2);

                                    }
                                    else
                                        backOEEColor = "#FFFFFF";

                                    foreColor = "#000000";
                                }

                                //Operator OEE Color Settings
                                else if (settingList[i].ColumnName.ToString().Equals("OperatorEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[settingList[i].ColumnName]) > 0)
                                    {
                                        if (Convert.ToDouble(item[settingList[i].ColumnName]) <= Int32.Parse(item["OPRRed"].ToString()))
                                            backOPROEColor = backColor = colorData.BadColor == "" ? "#FFFFFF" : colorData.BadColor.Remove(1, 2);

                                        else if (Convert.ToDouble(item[settingList[i].ColumnName]) >= Int32.Parse(item["OPRGreen"].ToString()))
                                            backOPROEColor = backColor = colorData.GoodColor == "" ? "#FFFFFF" : colorData.GoodColor.Remove(1, 2);

                                        else
                                            backOPROEColor = backColor = colorData.ModerateColor == "" ? "#FFFFFF" : colorData.ModerateColor.Remove(1, 2);

                                    }
                                    else
                                        backOPROEColor = "#FFFFFF";

                                    foreColor = "#000000";
                                }

                                else
                                {
                                    backColor = "#FBFBFB";
                                    backColorTitle = "#FBFBFB";
                                    foreColor = "#000000";
                                    foreColorTitle = "#000000";
                                }

                                if (settingList[i].ColumnName.Equals("MTTOEE", StringComparison.OrdinalIgnoreCase) || settingList[i].ColumnName.Equals("LastDayOEE", StringComparison.OrdinalIgnoreCase))
                                {
                                    var row = dt2.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(data.MachineId, StringComparison.OrdinalIgnoreCase)).Select(x => x[settingList[i].ColumnName]).FirstOrDefault();
                                    if (row == null)
                                    {
                                        Logger.WriteDebugLog(data.MachineId + " not found in s_GetAgg_AndonEffDetails_KachMotors proc Execution.");
                                    }

                                    listData.LabelValue1 = Math.Round(Convert.ToDouble(row), 2).ToString();
                                }
                                else
                                {
                                    if (item[settingList[i].ColumnName].GetType() == typeof(double))
                                    {
                                        if (settingList[i].ColumnName.Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                            listData.LabelValue1 = Convert.ToInt32(item[settingList[i].ColumnName]).ToString();
                                        else
                                            listData.LabelValue1 = Math.Round(Convert.ToDouble(item[settingList[i].ColumnName]), 2).ToString();
                                    }
                                    else
                                    {
                                        if (settingList[i].ColumnName.ToString().Equals("OperatorName", StringComparison.OrdinalIgnoreCase) || settingList[i].ColumnName.ToString().Equals("SupervisorName", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string OPName = item[settingList[i].ColumnName].ToString();
                                            if (OPName.Length > 15)
                                            {
                                                listData.LabelValue1 = OPName.Substring(0, 15).Trim();
                                                if (item[settingList[i].ColumnName].ToString().Length > 30)
                                                    listData.LabelValue2 = OPName.Substring(15, 13).Trim() + "..";
                                                else
                                                    listData.LabelValue2 = OPName.Substring(15, OPName.Length - 15).Trim();
                                            }
                                            else
                                                listData.LabelValue1 = item[settingList[i].ColumnName].ToString();
                                        }
                                        else
                                            listData.LabelValue1 = item[settingList[i].ColumnName].ToString();
                                    }
                                }

                                data.BackColor = BackColorParam.Equals("PE", StringComparison.OrdinalIgnoreCase) ? backPEColor : (BackColorParam.Equals("OPRE", StringComparison.OrdinalIgnoreCase) ? backOPROEColor : backOEEColor);

                                listData.LabelText = settingList[i].DisplayText;
                                listData.BackColor = backColor;
                                listData.BackColorTitle = backColorTitle;
                                listData.ForeColor = foreColor;
                                listData.ForeColorTitle = foreColorTitle;
                                listData.Tag = settingList[i].ColumnName;
                                listData.FontSizeInnerData = settingList[i].LabelFontSize;
                                listData.DataFontSize = settingList[i].DataFontSize;
                                listData.TextAlign = settingList[i].TextAlign;
                                listData.CellID = settingList[i].CellID;
                                data.Values.Add(listData);
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteErrorLog("getMachineCockpitData (inner) = " + ex.Message);
                            }
                        }

                        if (BackColorParam.Equals("PE", StringComparison.OrdinalIgnoreCase))
                            data.BackColor = backColor;
                        else if (BackColorParam.Equals("OEE", StringComparison.OrdinalIgnoreCase))
                            data.BackColor = backColor;
                        else
                            data.BackColor = "white";

                        data.BackColor = backOEEColor;
                        data.ShowSmileyBlock = andonSetting.ShowSmileyBlock;
                        data.SmileySize = andonSetting.SmileySize;
                        if (ParameterSettingsList != null)
                            data.MachineFontSize = ParameterSettingsList.AsEnumerable().Where(x => x.ColumnName.Equals("ProductionMachineNameFontSize", StringComparison.OrdinalIgnoreCase)).Where(x => x.CellID.Equals(data.GroupName, StringComparison.OrdinalIgnoreCase)).Select(x => x.LabelFontSize).FirstOrDefault();
                        if (andonSetting.ShowCurvedBox)
                        {
                            data.BorderClass = "addBorder";
                        }
                        else
                        {
                            data.BorderClass = "removeBorder";
                        }

                        list.Add(data);

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getMachineCockpitData_KachMotors= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static DataTable GetProductionAndonData_TableView_KachMotors(string PlantID, string GroupID, string SortOrder, string OrderBy)
        {
            DataTable dt_Shift = GetCurrentShiftTimings();
            List<string> andonColumnsList = GetParametersList_Andon_TableView_KachMotors();
            if (andonColumnsList == null || andonColumnsList.Count == 0)
            {
                Logger.WriteErrorLog("Default Parameters are not available in AndonDefaults");
                return new DataTable();
            }
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable resultDt = new DataTable();
            DataTable finalResult = new DataTable();
            SqlDataReader sdr = null;
            string ShiftStartTime = "", ShiftEndTime = "";
            try
            {
                foreach (var name in andonColumnsList)
                    resultDt.Columns.Add(name);

                ShiftStartTime = Convert.ToDateTime(dt_Shift.Rows[0]["StartTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                ShiftEndTime = Convert.ToDateTime(dt_Shift.Rows[0]["EndTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                cmd = new SqlCommand(@"s_GetCockpitData_WithTempTable_eshopx", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PlantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@GroupID", GroupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : GroupID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@param", "");
                cmd.Parameters.AddWithValue("@StartTime", ShiftStartTime);
                cmd.Parameters.AddWithValue("@EndTime", ShiftEndTime);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder + " " + OrderBy);
                Stopwatch executionTimer = new Stopwatch();
                executionTimer.Start();
                sdr = cmd.ExecuteReader();
                executionTimer.Stop();
                Logger.WriteDebugLog("Andon Proc Execution Time: " + executionTimer.ElapsedMilliseconds.ToString());
                if (sdr.HasRows)
                    dt.Load(sdr);

                cmd = new SqlCommand(@"s_GetAgg_AndonEffDetails_KachMotors", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PlantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@GroupID", GroupID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : GroupID);
                cmd.Parameters.AddWithValue("@MachineID", "");
                cmd.Parameters.AddWithValue("@Date", (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                    dt2.Load(sdr);

                for(int i =1;i<dt2.Columns.Count;i++)
                    dt.Columns.Add(dt2.Columns[i].ColumnName);

                dt.AsEnumerable().ToList<DataRow>().ForEach(row =>
                {
                    row[dt2.Columns[1].ColumnName] = dt2.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(row["MachineID"].ToString())).Select(x => x[dt2.Columns[1].ColumnName].ToString()).FirstOrDefault();
                    row[dt2.Columns[2].ColumnName] = dt2.AsEnumerable().Where(x => x["MachineID"].ToString().Equals(row["MachineID"].ToString())).Select(x => x[dt2.Columns[2].ColumnName].ToString()).FirstOrDefault();
                });
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionAndonData_TableView_KachMotors= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static List<string> GetParametersList_Andon_TableView_KachMotors()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<string> ColumnsLlist = new List<string>();
            try
            {
                cmd = new SqlCommand(@"SELECT * FROM AndonDefaults WHERE Parameter='AndonParameter_TableView_KM' AND CellID='Master' AND [User]='Master'", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read()) ColumnsLlist.Add(sdr["ValueInText"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetParametersList_Andon_TableView_KachMotors= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return ColumnsLlist;
        }

        private static List<string> GetComputerWiseCockpitListKTA(string ComputerName, string filter)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            List<string> list = new List<string>();
            string query = "select ValueInText2 as Value from CockpitDefaults where ValueInText=@ComputerName and Parameter=@Parameter";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                if (filter.Equals("RunByCell", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@Parameter", "ComputerCellAssociation");
                }
                else if (filter.Equals("RunByMachine", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@Parameter", "ComputerMachineAssociation");
                }

                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["value"].ToString());
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

        internal static string GetBackColorParam()
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string BackColorPaam = "OEE";
            try
            {
                cmd = new SqlCommand(@"if exists(select * from CockpitDefaults where Parameter='CockpitBackColorParam')
begin
select ValueInText as BackColorParam from CockpitDefaults where Parameter='CockpitBackColorParam'
end
else
begin
select 'OEE' as BackColorParam
end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                BackColorPaam = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetBackColorParam: " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return BackColorPaam;
        }

        private static void GetHeaderContentFontSize(string computerName, out string HeaderFontSize, out string ContentFontSize, out string WrapContent)
        {
            HeaderFontSize = ContentFontSize = WrapContent = string.Empty;

            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            string query = "if not exists(select * from ShopDefaults where ValueInText in ('HeaderFontSize', 'ContentFontSize', 'WrapContent') and Parameter='AndonCockpitAppSettings' and ValueInText2=@ComputerName) begin select ValueInText, isnull(ValueInInt, 15) as ValueInInt from ShopDefaults where ValueInText = 'HeaderFontSize' and Parameter = 'AndonCockpitAppSettings' and ISNULL(ValueInText2,'')= '' union select ValueInText, isnull(ValueInInt, 14) as ValueInInt from ShopDefaults where ValueInText = 'ContentFontSize' and Parameter = 'AndonCockpitAppSettings' and ISNULL(ValueInText2,'')= '' union select ValueInText, ISNULL(ValueInInt, 0) as ValueInInt from ShopDefaults where ValueInText = 'WrapContent' and Parameter = 'AndonCockpitAppSettings' and ISNULL(ValueInText2,'')= '' end else begin select* from ShopDefaults where ValueInText in ('HeaderFontSize', 'ContentFontSize', 'WrapContent') and Parameter = 'AndonCockpitAppSettings' and ValueInText2 = @ComputerName end";
            try
            {
                SqlCommand cmd = new SqlCommand("S_GetShopDefaultsPcDetails_KTA", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PcName", computerName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);

                    HeaderFontSize = (dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("HeaderFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInInt"].ToString()).FirstOrDefault().ToString());
                    ContentFontSize = (dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("ContentFontSize", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInInt"].ToString()).FirstOrDefault().ToString());
                    WrapContent = (dt.AsEnumerable().Where(x => x["ValueInText"].ToString().Equals("WrapContent", StringComparison.OrdinalIgnoreCase)).Select(x => x["ValueInInt"].ToString()).FirstOrDefault().ToString());
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
        }

        internal static List<string> GetDisplayScreensKTA(string ComputerName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd;
            SqlDataReader sdr = null;
            List<string> screens = new List<string>();
            string query = "select ValueInText2 as ScreenName from CockpitDefaults where Parameter='ComputerScreenAssociationKTA' and ValueInText=@ComputerName";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        screens.Add(sdr["ScreenName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetHeaderContentFontSize : " + ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return screens;
        }
        #region ------------ Schedule Andon KTA --------
        internal static List<ScheduleKTAEntity> getScheduleKTAData(string machineId, string plantId, string cellID, string orderBy, List<AndonDefaultsEntity> MainsettingList, string ComputerName, string filter)
        {
            var maxLength = 40;
            var fixedLabelLength = 15;
            EffyColorEntity colorData = getEfficiencyColor();
            List<ScheduleKTAEntity> cellFontList = AndonDBAccess.GetMaxDataFontSize();
            List<ScheduleKTAEntity> list = new List<ScheduleKTAEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string CellDatafontSize = string.Empty;
            List<string> filtered_list = new List<string>();
            string cells = string.Empty;
            string machines = string.Empty;
            try
            {
                string HeaderFontSize, ContentFontSize = string.Empty, WrapContent = string.Empty;
                GetHeaderContentFontSize(ComputerName, out HeaderFontSize, out ContentFontSize, out WrapContent);


                filtered_list = GetComputerWiseScheduleListKTA(ComputerName, filter);
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[dbo].[SP_ShowScheduleScreen_KTA]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Startdate", VDGDataBaseAccess.GetLogicalDayStartForLive(DateTime.Now.ToString()));
                if (filtered_list.Count > 0)
                {
                    if (filter.Equals("RunByMachine", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var li in filtered_list)
                        {
                            if (machines == string.Empty)
                            {
                                machines = machines + "'" + li + "'";
                            }
                            else
                            {
                                machines += ",'" + li + "'";
                            }
                        }
                        cmd.Parameters.AddWithValue("@MachineId", machines);
                        cmd.Parameters.AddWithValue("@GroupID ", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellID);
                    }
                    else if (filter.Equals("RunByCell", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var li in filtered_list)
                        {
                            if (cells == string.Empty)
                            {
                                cells += "'" + li + "'";
                            }
                            else
                            {
                                cells += ",'" + li + "'";
                            }

                        }
                        cmd.Parameters.AddWithValue("@MachineId", machineId);
                        cmd.Parameters.AddWithValue("@GroupID ", cells);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MachineId", machineId);
                    cmd.Parameters.AddWithValue("@GroupID ", string.IsNullOrEmpty(cellID) ? "" : "'" + cellID + "'");
                }
                cmd.Parameters.AddWithValue("@PlantID", plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                orderBy = orderBy.ToLower();
                cmd.Parameters.AddWithValue("@SortOrder", orderBy.Contains("machineid") ? orderBy : "Machineid asc");
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt != null && dt.Rows.Count > 0)
                {
                    var distMachineId = dt.AsEnumerable().Select(k => k.Field<string>("MachineID").Trim()).Distinct().ToList();
                    int a = 0;
                    foreach (string machineID in distMachineId)
                    {
                        if (machineID == "SHOUN-MC-112")
                        {

                        }
                        ScheduleKTAEntity data = new ScheduleKTAEntity();
                        var compRows = dt.AsEnumerable().Where(k => k.Field<string>("MachineID") == machineID).OrderByDescending(x => string.IsNullOrEmpty(x["Status"].ToString()) ? 0 : x["Status"]).ToList();
                        List<ScheduleKTAEntity> compList = new List<ScheduleKTAEntity>();

                        for (int i = 0; i < compRows.Count; i++)
                        {
                            DataRow row = compRows[i];
                            if (i == 0)
                            {
                                data.PlantID = row["PlantID"].ToString();
                                data.CellID = row["GroupID"].ToString();
                                CellDatafontSize = ContentFontSize;
                                //CellDatafontSize = cellFontList.AsEnumerable().Where(x => x.CellID.Equals(data.CellID, StringComparison.OrdinalIgnoreCase)).Select(x => x.DataFontSize).FirstOrDefault();
                                data.MachineID = row["MachineID"].ToString();
                                data.BackColor = row["MachineStatus"].ToString();
                                if (data.BackColor.Equals("Red", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(data.BackColor))
                                {
                                    data.BackColor = colorData.BadColor.Remove(1, 2);
                                }
                                else if (data.BackColor.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.BackColor = colorData.ModerateColor.Remove(1, 2);
                                }
                                else if (data.BackColor.Equals("Green", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.BackColor = colorData.GoodColor.Remove(1, 2);
                                }
                            }
                            data.MachineNameFontSize = HeaderFontSize;
                            data.KTAWrapContent = WrapContent;
                            //data.MachineNameFontSize = MainsettingList.AsEnumerable().Where(x => x.ValueInText.Equals("ProductionMachineNameFontSize", StringComparison.OrdinalIgnoreCase)).Where(x => x.CellID.Equals(data.CellID, StringComparison.OrdinalIgnoreCase)).Select(x => x.LabelFontSize).FirstOrDefault();

                            ScheduleKTAEntity compData = new ScheduleKTAEntity();
                            compData.ComponentID = row["ComponentID"].ToString();
                            //compData.ComponentID = row["ComponentID"].ToString();
                            compData.ComponentStatus = row["CompStatus"].ToString();
                            compData.ScheduleDate = string.IsNullOrEmpty(row["ScheduleDate"].ToString()) ? "" : Util.GetDateTime(row["ScheduleDate"].ToString()).ToString("dd-MM-yy");

                            if (compData.ComponentStatus.Equals("Close", StringComparison.OrdinalIgnoreCase))
                            {
                                compData.ComponentStatus = "StoppedSchedule";
                                compData.ComponentStatus = "../Image/McStatus/Stopped.gif";
                            }
                            else if (compData.ComponentStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                            {
                                compData.ComponentStatus = "RunningSchedule";
                                compData.ComponentStatus = "../Image/McStatus/Running.gif";
                            }
                            else
                            {
                                compData.ComponentStatus = "";
                            }
                            compData.FontSizeInnerData = CellDatafontSize;
                            compList.Add(compData);
                        }

                        data.ComponentList = compList;
                        list.Add(data);
                    }
                    //list = GetComputerWiseScheduleListKTA(list, ComputerName, filter);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getScheduleKTAData = " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return list;
        }

        private static List<ScheduleKTAEntity> GetMaxDataFontSize()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = "";
            query = @"select MAX(cast(DataFontSize as int)) as DatafontSize,CellID from AndonDefaults where  Parameter='CockpitAndonOrder' group by CellID";
            SqlDataReader sdr = null;
            List<ScheduleKTAEntity> Values = new List<ScheduleKTAEntity>();
            try
            {

                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ScheduleKTAEntity datavalues = new ScheduleKTAEntity();
                        datavalues.DataFontSize = sdr["DatafontSize"].ToString();
                        datavalues.CellID = sdr["CellID"].ToString();
                        Values.Add(datavalues);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getPalntID - \n " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return Values;
        }

        private static List<string> GetComputerWiseScheduleListKTA(string ComputerName, string filter)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            DataTable dt = new DataTable();
            List<string> list = new List<string>();
            string query = "select ValueInText2 as Value from CockpitDefaults where ValueInText=@ComputerName and Parameter=@Parameter";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                if (filter.Equals("RunByCell", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@Parameter", "ComputerCellAssociation");
                }
                else if (filter.Equals("RunByMachine", StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Parameters.AddWithValue("@Parameter", "ComputerMachineAssociation");
                }
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["value"].ToString());
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

        public static bool SaveRunOption(string ComputerName, string RunOption, string parameter)
        {
            bool result = false;
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = "if not exists (select * from CockpitDefaults where Parameter=@Param and ValueInText=@ComputerName) begin insert into CockpitDefaults(Parameter, ValueInText, ValueInText2,UpdatedTS) values(@Param, @ComputerName, @RunOption,@UpdatedTS) end else begin update CockpitDefaults set ValueInText2 = @RunOption,UpdatedTS=@UpdatedTS where Parameter = @Param and ValueInText = @ComputerName end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.Parameters.AddWithValue("@RunOption", RunOption);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Param", parameter);
                result = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {


                if (conn != null) conn.Close();
            }
            return result;
        }

        //RunOption Value = 1 ==> Run By Plant & Cell , Value = 2 ==> Run By Plant & Machine
        public static string GetRunOption(string ComputerName, string Parameter)
        {
            string RunOption = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            string query = @"if exists (select * from CockpitDefaults where ValueInText=@ComputerName and Parameter=@Parameter) 
begin 
	select ValueInText2 as RunOption from CockpitDefaults where ValueInText=@ComputerName and Parameter=@Parameter 
end 
else 
begin 
	select 'not exists' as RunOption
end";
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                RunOption = cmd.ExecuteScalar().ToString();
                if (RunOption.Equals("not exists", StringComparison.OrdinalIgnoreCase))
                {
                    RunOption = "RunByCell";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return RunOption;
        }

        internal static bool SaveAndonFrquency(string ComputerName, string ValueInText2, string Parameter)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            bool success = false;
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM AndonDefaults WHERE [User]=@PCName AND ValueInText='AndonFrequency' AND Parameter=@Parameter)
BEGIN
	INSERT INTO AndonDefaults(Parameter, ValueInText, ValueInText2,[User]) VALUES(@Parameter,'AndonFrequency', @ValueInText2, @PCName)
END
ELSE
BEGIN
	UPDATE AndonDefaults SET ValueInText2=@ValueInText2 WHERE [User]=@PCName AND ValueInText='AndonFrequency' AND Parameter=@Parameter
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ValueInText2", ValueInText2);
                cmd.Parameters.AddWithValue("@PCName", ComputerName);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                success = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SaveAndonFrquency= " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return success;
        }

        internal static string GetFrequency(string ComputerName, string ValueInText, string Parameter)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string Freq = "";
            try
            {
                cmd = new SqlCommand(@"IF EXISTS (SELECT * FROM AndonDefaults WHERE [User]=@PCName AND ValueInText=@ValueInText AND Parameter=@Parameter)
BEGIN
	SELECT * FROM AndonDefaults WHERE [User]=@PCName AND ValueInText=@ValueInText AND Parameter=@Parameter
END
ELSE
BEGIN
	SELECT * FROM AndonDefaults WHERE [User]='Master' AND ValueInText=@ValueInText AND Parameter=@Parameter
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PCName", ComputerName);
                cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                        Freq = sdr["ValueInText2"].ToString();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetFrequency= " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return Freq;
        }
        #endregion

        #region----------------------------Pooja Casting Melting Control-------------------
        internal static DataTable GetPoojaAndonMeltingData(string plantId, out DataTable dt2)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            DataTable dt = new DataTable();
            dt2 = new DataTable();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                //dt.Columns.Add("Melting");
                //dt.Columns.Add("SI");
                //dt.Columns.Add("FE");
                //dt.Columns.Add("CU");
                //dt.Columns.Add("MG");
                //dt.Columns.Add("MN");
                //dt.Columns.Add("Status");

                //DataRow dataRow = dt.NewRow();
                //dataRow["Melting"] = "MF1";
                //dataRow["SI"] = "10%";
                //dataRow["FE"] = "20%";
                //dataRow["CU"] = "30%";
                //dataRow["MG"] = "40%";
                //dataRow["MN"] = "50%";
                //dataRow["Status"] = "Green";
                //dt.Rows.Add(dataRow);

                //dataRow = dt.NewRow();
                //dataRow["Melting"] = "MF2";
                //dataRow["SI"] = "5%";
                //dataRow["FE"] = "10%";
                //dataRow["CU"] = "7%";
                //dataRow["MG"] = "9%";
                //dataRow["MN"] = "11%";
                //dataRow["Status"] = "Red";
                //dt.Rows.Add(dataRow);


                cmd = new SqlCommand(@"S_GetMachineParameter_PoojaCasting", conn);
                cmd.Parameters.AddWithValue("@PlantID", plantId.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantId);
                cmd.Parameters.AddWithValue("@Param", "View");
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                dt.AcceptChanges();
                dt2.Load(sdr);
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
            return dt;
        }
        #endregion

        internal static DataTable GetProductionAndonData_TableView(string ComputerName, string PlantID, string CellID, string SortOrder, string OrderBy)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand("s_GetComfitValves_ANDONDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                cmd.Parameters.AddWithValue("@PlantID", PlantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : PlantID);
                cmd.Parameters.AddWithValue("@GroupID", CellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ("'" + CellID + "'"));
                cmd.Parameters.AddWithValue("@Param", "");
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@Order", OrderBy);
                cmd.Parameters.AddWithValue("@AndonType", "TableView");

                sdr = cmd.ExecuteReader();
                //if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetProductionAndonData_TableView=  " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return dt;
        }

        internal static List<AndonSettingsEntity> GetAndonSettings(string ComputerName, string Parameter)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<AndonSettingsEntity> list = new List<AndonSettingsEntity>();
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM AndonDefaults WHERE Parameter=@Parameter AND [User]=@PCName)
BEGIN

    SELECT * FROM AndonDefaults WHERE Parameter = @Parameter AND[User] = 'Master' AND CellID = 'Master' AND ValueInBool=1 Order by ValueInInt
END
ELSE
BEGIN

    SELECT * FROM AndonDefaults WHERE Parameter = @Parameter AND [User] = @PCName  AND ValueInBool=1 Order by ValueInInt
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@PCName", ComputerName);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
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
                        entity.CellID = sdr["CellID"].ToString();
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


        #region -- table view andon --
        internal static AndonSettingEntity ViewAndonGeneralSettings(string Param, string ComputerName)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            AndonSettingEntity UISettings = new AndonSettingEntity();
            try
            {
                cmd = new SqlCommand("", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@PCName", ComputerName);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("FontFamily", StringComparison.OrdinalIgnoreCase))
                            UISettings.FontFamily = sdr["ValueInText2"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("FontStyle", StringComparison.OrdinalIgnoreCase))
                            UISettings.FontStyle = sdr["ValueInText2"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("DataDisplayInterval", StringComparison.OrdinalIgnoreCase))
                            UISettings.DataRefreshInterval = Convert.ToInt32(sdr["ValueInText2"].ToString());

                        if (sdr["ValueInText"].ToString().Equals("ScreenFlipInterval", StringComparison.OrdinalIgnoreCase))
                            UISettings.ScreenFlipInterval = Convert.ToInt32(sdr["ValueInText2"].ToString());

                        if (sdr["ValueInText"].ToString().Equals("DateFormatForHeader", StringComparison.OrdinalIgnoreCase))
                            UISettings.DateFormatForHeader = sdr["ValueInText2"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("TimeFormatForHeader", StringComparison.OrdinalIgnoreCase))
                            UISettings.TimeFormatForHeader = sdr["ValueInText2"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("ShowSmileyBlock", StringComparison.OrdinalIgnoreCase))
                            UISettings.ShowSmileyBlock = sdr["ValueInInt"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("ShowMsgBlock", StringComparison.OrdinalIgnoreCase))
                            UISettings.ShowMsgBox = sdr["ValueInInt"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("ShowFooterBlock", StringComparison.OrdinalIgnoreCase))
                            UISettings.ShowFooterBlock = sdr["ValueInInt"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("OrderBy", StringComparison.OrdinalIgnoreCase))
                        {
                            UISettings.OrderBy = sdr["ValueInText2"].ToString().Split(' ').ToList()[0];
                            UISettings.SortOrder = sdr["ValueInText2"].ToString().Split(' ').ToList()[1];
                        }

                        if (sdr["ValueInText"].ToString().Equals("ScrollingText", StringComparison.OrdinalIgnoreCase))
                            UISettings.ScrollingText = sdr["ValueInText2"].ToString();

                        if (sdr["ValueInText"].ToString().Equals("SmileyBlockSize", StringComparison.OrdinalIgnoreCase))
                            UISettings.SmileySize = sdr["ValueInText2"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ViewAndonGeneralSettings= " + ex.Message);

            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return UISettings;
        }
        #endregion


        #region --Store Andon--
        internal static List<StoreAndonKTAEntity> getStoreAndonKTAData(string plantID, string cellID, string shiftStart, string shiftEnd, string orderBy, List<AndonDefaultsEntity> MainsettingList, List<AndonDefaultsEntity> AndonDetails, string Type)
        {
            List<StoreAndonKTAEntity> list = new List<StoreAndonKTAEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string CellDatafontSize = string.Empty;
            string HeaderFontSize = string.Empty;
            int maxlength = 40;
            int fixedlength = 15;
            try
            {

                DataTable dt = new DataTable();
                cmd = new SqlCommand("[dbo].[S_GetScheduleDetails_KTA]", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", shiftStart);
                cmd.Parameters.AddWithValue("@EndDate", shiftEnd);
                cmd.Parameters.AddWithValue("@PlantID", plantID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : plantID);
                cmd.Parameters.AddWithValue("@Cell ", cellID.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : cellID);
                cmd.Parameters.AddWithValue("@Param", "StoreAndOn");
                cmd.CommandTimeout = 450;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt != null && dt.Rows.Count > 0)
                {
                    var distMachineId = dt.AsEnumerable().Select(k => k.Field<string>("MachineID")).Distinct().ToList();
                    foreach (string machineID in distMachineId)
                    {
                        StoreAndonKTAEntity datalist = new StoreAndonKTAEntity();
                        var compRows = dt.AsEnumerable().Where(k => k.Field<string>("MachineID") == machineID).ToList();

                        List<StoreAndonKTAEntity> compList = new List<StoreAndonKTAEntity>();
                        for (int i = 0; i < compRows.Count; i++)
                        {
                            DataRow row = compRows[i];
                            if (i == 0)
                            {
                                datalist.CellID = row["GroupID"].ToString();
                                datalist.Machine = row["MachineID"].ToString();
                                CellDatafontSize = MainsettingList.AsEnumerable().Where(x => x.ValueInText.Equals("ContentFontSize", StringComparison.OrdinalIgnoreCase)).Where(x => x.CellID.Equals(datalist.CellID, StringComparison.OrdinalIgnoreCase)).Select(x => x.DataFontSize).FirstOrDefault();
                                HeaderFontSize = MainsettingList.AsEnumerable().Where(x => x.ValueInText.Equals("HeaderFontSize", StringComparison.OrdinalIgnoreCase)).Where(x => x.CellID.Equals(datalist.CellID, StringComparison.OrdinalIgnoreCase)).Select(x => x.LabelFontSize).FirstOrDefault();
                            }
                            datalist.MachineFontSize = MainsettingList.AsEnumerable().Where(x => x.ValueInText.Equals("MachineNameFontSize", StringComparison.OrdinalIgnoreCase)).Where(x => x.CellID.Equals(datalist.CellID, StringComparison.OrdinalIgnoreCase)).Select(x => x.ValueInText2).FirstOrDefault();

                            StoreAndonKTAEntity compData = new StoreAndonKTAEntity();
                            if (i == 0)
                            {
                                compData.Component = "Component";
                                compData.Operation = "Op";
                                compData.OpDesc = "Description";
                                compData.Status = "Status";
                                compData.Priority = "Priority";
                                compData.FontSizeInnerData = HeaderFontSize;
                                compData.LabelVisible = true;
                                compData.LinkVisible = false;
                                compList.Add(compData);
                            }
                            compData = new StoreAndonKTAEntity();

                            compData.Component = row["ComponentID"].ToString();
                            compData.LabelScrolling = string.Empty;
                            compData.LabelVisible = false;
                            compData.LinkVisible = true;
                            //if (Type.Equals("DestopView", StringComparison.OrdinalIgnoreCase))
                            //{
                            //    compData.Component = row["ComponentID"].ToString();
                            //    compData.LabelScrolling = string.Empty;
                            //}
                            //else
                            //{
                            //    if (row["ComponentID"].ToString().Length > maxlength)
                            //    {
                            //        compData.Component = row["ComponentID"].ToString().Substring(0, fixedlength);
                            //        compData.LabelScrolling = row["ComponentID"].ToString().Substring(fixedlength);
                            //    }
                            //    else
                            //    {
                            //        compData.Component = row["ComponentID"].ToString();
                            //        compData.LabelScrolling = string.Empty;
                            //    }
                            //}
                            compData.Operation = row["OperationNo"].ToString();
                            compData.OpDesc = row["OpnDescription"].ToString();
                            compData.Status = row["Status"].ToString();
                            compData.Priority = row["PriorityNo"].ToString();
                            if (compData.Priority.Equals("0"))
                            {
                                compData.Priority = string.Empty;
                            }
                            compData.FontSizeInnerData = CellDatafontSize;
                            compList.Add(compData);
                        }
                        datalist.ComponentList = compList;
                        list.Add(datalist);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getScheduleKTAData = " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return list;
        }
        internal static List<AndonDefaultsEntity> getAndonStoreSettingDetails()
        {
            List<AndonDefaultsEntity> list = new List<AndonDefaultsEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from ShopDefaults where Parameter = 'StoreAndonSetting'", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        AndonDefaultsEntity data = new AndonDefaultsEntity();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        data.ValueInText2 = sdr["ValueInText2"].ToString();
                        data.ValueInInt = string.IsNullOrEmpty(sdr["ValueInInt"].ToString()) ? 0 : Convert.ToInt32(sdr["ValueInInt"].ToString());
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getStoreSettingDetails = " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<AndonDefaultsEntity> getMainStoreSettingDetails()
        {
            List<AndonDefaultsEntity> list = new List<AndonDefaultsEntity>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"SELECT [ValueInText],[ValueInText2],ISNULL(TextAlign,'Left') as TextAlign , isnull(DataFontSize,20) as DataFontSize, ISNULL(LabelFontSize,20) as LabelFontSize,CellID 
FROM AndonDefaults where parameter = 'StoreAndonSetting' and ValueInBool = '1' order by valueinint", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        AndonDefaultsEntity data = new AndonDefaultsEntity();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        data.ValueInText2 = sdr["ValueInText2"].ToString();
                        data.TextAlign = sdr["TextAlign"].ToString();
                        data.DataFontSize = sdr["DataFontSize"].ToString();
                        data.LabelFontSize = sdr["LabelFontSize"].ToString();
                        data.CellID = sdr["CellID"].ToString();
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getStoreSettingDetails = " + ex.Message);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        #endregion
    }
}