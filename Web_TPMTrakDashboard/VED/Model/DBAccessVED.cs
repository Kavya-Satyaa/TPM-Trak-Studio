using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.GenericAndon.Model;
//using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.VED.Model
{
    public class DBAccessVED
    {
        #region -- Andon Settings -- 
        internal static AndonSettingEntityVED ViewAndonSettings(string DeviceName, string Parameter)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            AndonSettingEntityVED settingList = new AndonSettingEntityVED();
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetAndonUISettings]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", DeviceName);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@Param", "View");
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("FontFamily", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.FontFamily = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("FontStyle", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.FontStyle = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("DataDisplayInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.DataRefreshInterval = string.IsNullOrEmpty(sdr["ValueInText2"].ToString()) ? 60 : Convert.ToInt32(sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ScreenFlipInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.ScreenFlipInterval = string.IsNullOrEmpty(sdr["ValueInText2"].ToString()) ? 10 : Convert.ToInt32(sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("EnableImage", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.ImageEnabled = Convert.ToInt32(sdr["ValueInInt"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("EnableVideo", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.VideoEnabled = Convert.ToInt32(sdr["ValueInInt"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ImageFlipInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.SlideshowInterval = string.IsNullOrEmpty(sdr["ValueInText2"].ToString()) ? 10 : Convert.ToInt32(sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ImageFilePath", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.ImagePath = sdr["ValueInText2"] == DBNull.Value ? "" : sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("VideoFilePath", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.VideoPath = sdr["ValueInText2"] == DBNull.Value ? "" : sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ShowSmileyBlock", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.EmojiEnabled = Convert.ToInt32(sdr["ValueInInt"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ShowSmileyBlockSize", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.EmojiSize = Convert.ToInt32(sdr["ValueInText2"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ShowCurvedBox", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.ShowCurvedBoxes = Convert.ToInt32(sdr["ValueInInt"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ShowFooterBlock", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.FooterBlockEnabled = Convert.ToInt32(sdr["ValueInInt"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("MsgBlockEnabled", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.MsgBlockEnabled = Convert.ToInt32(sdr["ValueInInt"].ToString());
                        }
                        else if (sdr["ValueInText"].ToString().Equals("ScrollingText", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.ScrollingMsg = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("OrderBy", StringComparison.OrdinalIgnoreCase))
                        {
                            if (sdr["ValueInText2"] != DBNull.Value)
                            {
                                var sortArray = sdr["ValueInText2"].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                settingList.Sortorder = sortArray[0];
                                if (sortArray.Count > 1)
                                    settingList.orderby = sortArray[1];
                                else
                                    settingList.orderby = "ASC";
                            }
                        }
                        else if (sdr["ValueInText"].ToString().Equals("DateFormatForHeader", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.DateFormat = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("TimeFormatForHeader", StringComparison.OrdinalIgnoreCase))
                        {
                            settingList.TimeFormat = sdr["ValueInText2"].ToString();
                        }
                        else
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ViewAndonSettings= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return settingList;
        }

        internal static void UpdateAndonSettings(AndonSettingEntityVED data, string Parameter, string DeviceName, out string successFlag)
        {
            successFlag = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"[dbo].[S_GetAndonUISettings]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@FontFamily", data.FontFamily);
                cmd.Parameters.AddWithValue("@FontStyle", data.FontStyle);
                cmd.Parameters.AddWithValue("@DataDisplayInterval", data.DataRefreshInterval);
                cmd.Parameters.AddWithValue("@ScreenFlipInterval", data.ScreenFlipInterval);
                cmd.Parameters.AddWithValue("@EnableImage", data.ImageEnabled);
                cmd.Parameters.AddWithValue("@ImageFilePath", data.ImagePath);
                cmd.Parameters.AddWithValue("@EnableVideo", data.VideoEnabled);
                cmd.Parameters.AddWithValue("@videoFilePath", data.VideoPath);
                cmd.Parameters.AddWithValue("@ImageFlipInterval", data.SlideshowInterval);
                cmd.Parameters.AddWithValue("@ShowSmileyBlock", data.EmojiEnabled);
                cmd.Parameters.AddWithValue("@ShowSmileyBlockSize", data.EmojiSize);
                cmd.Parameters.AddWithValue("@ShowCurvedBox", data.ShowCurvedBoxes);
                cmd.Parameters.AddWithValue("@ShowFooterBlock", data.FooterBlockEnabled);
                cmd.Parameters.AddWithValue("@MsgBlockEnabled", data.MsgBlockEnabled);
                cmd.Parameters.AddWithValue("@ScrollingText", data.ScrollingMsg);
                cmd.Parameters.AddWithValue("@Orderby", data.Sortorder + " " + data.orderby);
                cmd.Parameters.AddWithValue("@DateFormatForHeader", data.DateFormat);
                cmd.Parameters.AddWithValue("@TimeFormatForHeader", data.TimeFormat);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ComputerName", DeviceName);
                cmd.Parameters.AddWithValue("@Param", "Update");

                cmd.ExecuteNonQuery();
                successFlag = "Saved";

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("UpdateAndonSettings= " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static List<ScreenEntityVED> GetAndonSettingsScreensData(string ComputerName, AndonSettingEntityVED setting, bool returnEnabledScreens = false)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<ScreenEntityVED> list = new List<ScreenEntityVED>();
            ScreenEntityVED data = null;
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS(select * from ShopDefaults where Parameter='AndonScreen_VED' AND UserID=@ComputerName)
BEGIN
	select * from ShopDefaults where Parameter='AndonScreen_VED' AND UserID='Master'
END
ELSE
BEGIN
	select * from ShopDefaults where Parameter='AndonScreen_VED' AND UserID=@ComputerName
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new ScreenEntityVED();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        data.ScreenName = sdr["ValueInText2"].ToString();
                        data.IsVisible = sdr["ValueInInt"].ToString() == "1" ? true : false;


                        if (returnEnabledScreens)
                        {
                            if (data.IsVisible)
                                list.Add(data);
                        }
                        else
                            list.Add(data);

                    }

                    if (returnEnabledScreens)
                    {
                        if (setting.ImageEnabled == 1 || setting.VideoEnabled == 1)
                        {
                            list.Add(new ScreenEntityVED() { ValueInText = "ImageVideoSlider", ScreenName = "Image / Video", Parameter = "", IsVisible = true });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ScreenEntityVED= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }
        internal static int SaveAndonSettingsScreensData(ScreenEntityVED data, string ComputerName, string UpdatedTS)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            List<ScreenEntityVED> list = new List<ScreenEntityVED>();
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS(select * from ShopDefaults where Parameter='AndonScreen_VED' AND UserID=@ComputerName AND ValueInText=@ValueInText)
BEGIN
	Insert Into ShopDefaults (Parameter, ValueInText, ValueInText2, ValueInInt, UpdatedTS, Userid)
	VALUES(@Parameter, @ValueInText, @ValueInText2, @ValueInInt, @UpdatedTS,@ComputerName)
END
ELSE
BEGIN
	Update ShopDefaults SET ValueInText2=@ValueInText2, ValueInInt=@ValueInInt, UpdatedTS=@UpdatedTS
	WHERE  Parameter=@Parameter AND UserID=@ComputerName AND ValueInText=@ValueInText
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Parameter", data.Parameter);
                cmd.Parameters.AddWithValue("@ValueInText", data.ValueInText);
                cmd.Parameters.AddWithValue("@ValueInText2", data.ScreenName);
                cmd.Parameters.AddWithValue("@ValueInInt", data.IsVisible ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", UpdatedTS);
                cmd.Parameters.AddWithValue("@ComputerName", ComputerName);

                result += cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ScreenEntityVED= " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return result;
        }

        internal static List<AndonFontSettingEntity> GetFontSettingsAndon(string DeviceName)
        {
            List<AndonFontSettingEntity> fontSettingList = new List<AndonFontSettingEntity>();
            AndonFontSettingEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            try
            {
                cmd = new SqlCommand(@"IF EXISTS(SELECT * FROM ShopDefaults WHERE Parameter='FontSettings_VEDAndon' AND Userid=@ComputerName)
BEGIN
	SELECT * FROM ShopDefaults WHERE Parameter='FontSettings_VEDAndon' AND Userid=@ComputerName
END
ELSE
BEGIN
	SELECT * FROM ShopDefaults WHERE Parameter='FontSettings_VEDAndon' AND Userid='Master'
END", conn);
                cmd.Parameters.AddWithValue("@ComputerName", DeviceName);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;

                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new AndonFontSettingEntity();
                        data.ValueInText = sdr["ValueInText"].ToString();
                        data.ValueInInt = sdr["ValueInInt"].ToString();

                        fontSettingList.Add(data);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("GetFontSettingsAndon= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return fontSettingList;
        }

        internal static void SaveFontSettingData(string Param, string ValueInText, string ValueInInt, string DeviceName, string UpdatedTS)
        {
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"IF NOT EXISTS(SELECT * FROM ShopDefaults where Parameter=@Parameter and ValueInText=@ValueInText and Userid=@UserID)
BEGIN
	INSERT INTO ShopDefaults(Parameter, ValueInText, ValueInInt, UpdatedTS, Userid)
	VALUES(@Parameter,@ValueInText,@ValueInInt,@UpdatedTS,@UserID)
END
ELSE
BEGIN
Update ShopDefaults SET ValueInInt=@ValueInInt, UpdatedTS=@UpdatedTS 
WHERE  Parameter=@Parameter and ValueInText=@ValueInText and Userid=@UserID
END", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@Parameter", Param);
                cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
                cmd.Parameters.AddWithValue("@ValueInInt", ValueInInt);
                cmd.Parameters.AddWithValue("@UserID", DeviceName);
                cmd.Parameters.AddWithValue("@UpdatedTS", UpdatedTS);

                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("SaveFontSettingData = " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }
        #endregion

        #region -- Andon --
        internal static AndonKPIEntity GetPlantEfficiencyData_Andon(List<AndonFontSettingEntity> fontSettings)
        {
            AndonKPIEntity kpiData = new AndonKPIEntity();
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            DataTable dtMachineKPIs = new DataTable();
            DataTable dtCellWiseKPIs = new DataTable();
            DataTable dtPlantKPIs = new DataTable();
            Stopwatch swtimer = new Stopwatch();
            try
            {
                cmd = new SqlCommand(@"S_GetAggAndonDetails_VEDH", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@YearNo", DateTime.Now.Year);
                cmd.Parameters.AddWithValue("@MonthNo", DateTime.Now.Month); //
                swtimer.Start();
                sdr = cmd.ExecuteReader();
                swtimer.Stop();

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AndonLogsEnabled"].ToString() == "1")
                    Logger.WriteDebugLog("Finished Calling proc s_GetSONA_ShiftAgg_DowntimeMatrix in " + swtimer.ElapsedMilliseconds + "msec.");
                if (sdr.HasRows)
                {
                    dtMachineKPIs.Load(sdr);
                    dtMachineKPIs.AcceptChanges();

                    dtCellWiseKPIs.Load(sdr);
                    dtCellWiseKPIs.AcceptChanges();

                    dtPlantKPIs.Load(sdr);
                    dtPlantKPIs.AcceptChanges();



                    if (dtMachineKPIs.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtMachineKPIs.Rows)
                        {
                            kpiData.MachineWiseKPIsMonthly.Add(new AndonKPIEntity()
                            {
                                EntityID = row["MachineID"].ToString(),
                                AE = row["AEffy"] != DBNull.Value ? Convert.ToDouble(row["AEffy"].ToString()) : 0,
                                PE = row["PEffy"] != DBNull.Value ? Convert.ToDouble(row["PEffy"].ToString()) : 0,
                                QE = row["QEffy"] != DBNull.Value ? Convert.ToDouble(row["QEffy"].ToString()) : 0,
                                OEE = row["OEffy"] != DBNull.Value ? Convert.ToDouble(row["OEffy"].ToString()) : 0,
                            });
                        }
                    }

                    if (dtCellWiseKPIs.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtCellWiseKPIs.Rows)
                        {
                            kpiData.CellWiseKPIsMonthly.Add(new AndonKPIEntity()
                            {
                                EntityID = row["GroupID"].ToString(),
                                AE = row["AEffy"] != DBNull.Value ? Convert.ToDouble(row["AEffy"].ToString()) : 0,
                                PE = row["PEffy"] != DBNull.Value ? Convert.ToDouble(row["PEffy"].ToString()) : 0,
                                QE = row["QEffy"] != DBNull.Value ? Convert.ToDouble(row["QEffy"].ToString()) : 0,
                                OEE = row["OEffy"] != DBNull.Value ? Convert.ToDouble(row["OEffy"].ToString()) : 0,

                                OEEColor = row["OEE_ColorCode"].ToString()
                            });
                        }
                    }

                    if (dtPlantKPIs.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtPlantKPIs.Rows)
                        {
                            kpiData.EntityID = row["PlantID"].ToString();
                            kpiData.AE = row["AEffy"] != DBNull.Value ? Convert.ToDouble(row["AEffy"].ToString()) : 0;
                            kpiData.PE = row["PEffy"] != DBNull.Value ? Convert.ToDouble(row["PEffy"].ToString()) : 0;
                            kpiData.QE = row["QEffy"] != DBNull.Value ? Convert.ToDouble(row["QEffy"].ToString()) : 0;
                            kpiData.OEE = row["OEffy"] != DBNull.Value ? Convert.ToDouble(row["OEffy"].ToString()) : 0;

                            kpiData.OEEColor = row["OEE_ColorCode"].ToString();
                            kpiData.AEColor = row["AE_ColorCode"].ToString();
                            kpiData.PEColor = row["PE_ColorCode"].ToString();
                            kpiData.QEColor = row["QE_ColorCode"].ToString();
                        }

                    }

                    try
                    {
                        kpiData.ColumnChartxAxisFontSize = fontSettings.Where(x => x.ValueInText == "ColumnChartxAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        kpiData.ColumnChartyAxisFontSize = fontSettings.Where(x => x.ValueInText == "ColumnChartyAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        kpiData.ColumnChartDataLabelFontSize = fontSettings.Where(x => x.ValueInText == "ColumnChartdataLabelsFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        kpiData.DonutChartFontSize = fontSettings.Where(x => x.ValueInText == "OEEChartFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    }
                    catch(Exception ex)
                    {
                        Logger.WriteErrorLog("Font Settings Error - " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("AndonKPIEntity= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return kpiData;
        }

        internal static DowntimeMainEntity GetDownTimeforCurrentMonth_Andon(List<AndonFontSettingEntity> fontSettings, string PlantID = "", string GroupID = "", string MachineID = "", string DownID = "")
        {
            DowntimeMainEntity list = new DowntimeMainEntity();
            List<DowntimeEntity> downData = new List<DowntimeEntity>();
            DowntimeEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            string StartTime = $"{DateTime.Now.Year}-{DateTime.Now.Month}-01"; //
            string EndTime = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)}";
            Stopwatch swtimer = new Stopwatch();
            try
            {

                cmd = new SqlCommand("s_GetSONA_ShiftAgg_DowntimeMatrix", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartTime", StartTime);
                cmd.Parameters.AddWithValue("@EndTime", EndTime);
                cmd.Parameters.AddWithValue("@Exclude", 0);
                cmd.Parameters.AddWithValue("@MatrixType", "DLoss_By_Catagory");
                cmd.Parameters.AddWithValue("@PlantID", PlantID);
                cmd.Parameters.AddWithValue("@Groupid", GroupID);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@DownID", DownID);
                swtimer.Start();
                sdr = cmd.ExecuteReader();
                swtimer.Stop();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AndonLogsEnabled"].ToString() == "1")
                    Logger.WriteDebugLog("Finished Calling proc s_GetSONA_ShiftAgg_DowntimeMatrix in " + swtimer.ElapsedMilliseconds + "msec.");
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new DowntimeEntity();

                        data.dCategory = sdr["Catagory"].ToString();
                        data.dCode = sdr["DownID"].ToString();
                        data.downTimeinMin = sdr["DownTime"] != DBNull.Value ? Convert.ToDouble(sdr["DownTime"].ToString()) : 0;
                        data.downTimeinSec = sdr["DowntimeInSeconds"] != DBNull.Value ? Convert.ToDouble(sdr["DowntimeInSeconds"].ToString()) : 0;


                        downData.Add(data);
                    }

                    list.downtimeData = downData;
                    list.CategoriesData = downData.GroupBy(x => x.dCategory).Select(x => new DowntimeEntity { dCategory = x.Key, downTimeinMin = x.Sum(y => y.downTimeinMin), dCode = "", downTimeinSec = 0 }).Distinct().ToList();

                    try
                    {
                        list.PieChartFontSize = fontSettings.Where(x => x.ValueInText == "ColumnChartxAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();

                        list.ColumnChartxAxisFontSize = fontSettings.Where(x => x.ValueInText == "ColumnChartxAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        list.ColumnChartyAxisFontSize = fontSettings.Where(x => x.ValueInText == "ColumnChartyAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        list.ColumnChartDataLabelFontSize = fontSettings.Where(x => x.ValueInText == "ColumnChartdataLabelsFontSize").Select(x => x.ValueInInt).FirstOrDefault();

                        list.ParetoChartxAxisFontSize = fontSettings.Where(x => x.ValueInText == "ParetoChartxAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        list.ParetoChartyAxisFontSize = fontSettings.Where(x => x.ValueInText == "ParetoChartyAxisFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        list.ParetoChartColumnDatalabelsFontSize = fontSettings.Where(x => x.ValueInText == "ParetoChartColumnDatalabelsFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                        list.ParetoChartParetoDatalabelsFontSize = fontSettings.Where(x => x.ValueInText == "ParetoChartParetoDatalabelsFontSize").Select(x => x.ValueInInt).FirstOrDefault();
                    }
                    catch(Exception ex)
                    {
                        Logger.WriteErrorLog("Font Settings Error in Downtime details= " + ex.Message);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetDownTimeforCurrentMonth_Andon= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return list;
        }

        internal static List<HourlyTargetAndonEntity> GetHourlyTargetAndon(string MachineID, string Shift, List<AndonFontSettingEntity> fontSettings)
        {
            List<HourlyTargetAndonEntity> hourlyTargetList = new List<HourlyTargetAndonEntity>();
            HourlyTargetAndonEntity data = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            Stopwatch swtimer = new Stopwatch();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("s_GetHourlyTarget_Count_followup", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@StartDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Shift", Shift);
                cmd.Parameters.AddWithValue("@Param", "");
                swtimer.Start();
                 sdr = cmd.ExecuteReader();
                swtimer.Stop();
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AndonLogsEnabled"].ToString() == "1")
                    Logger.WriteDebugLog("Finished Calling proc s_GetSONA_ShiftAgg_DowntimeMatrix in " + swtimer.ElapsedMilliseconds + "msec.");

                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();


                    string HeaderFontSize = fontSettings.Where(x => x.ValueInText == "TabledataHeaderfontsize").Any() ? fontSettings.Where(x => x.ValueInText == "TabledataHeaderfontsize").Select(x => x.ValueInInt).FirstOrDefault() : "";
                    string ContentFontSize = fontSettings.Where(x => x.ValueInText == "TabledataContentfontsize").Any() ? fontSettings.Where(x => x.ValueInText == "TabledataContentfontsize").Select(x => x.ValueInInt).FirstOrDefault() : "";


                    var HourList = dt.AsEnumerable().Select(x => new { HourID = x["HourID"].ToString(), FromTime = Models.Util.GetDateTime(x["FromTime"].ToString()).ToString("HH:mm"), ToTime = Models.Util.GetDateTime(x["ToTime"].ToString()).ToString("HH:mm") }).Distinct().ToList();

                    dt = dt.AsEnumerable().OrderBy(x => Convert.ToInt32(x["HourID"].ToString())).CopyToDataTable();

                    var MachineList = dt.AsEnumerable().Select(x => x["MachineID"].ToString()).Distinct().ToList();

                    data = new HourlyTargetAndonEntity();
                    data.HourTiminigs = "Time";
                    data.fontSize = HeaderFontSize;
                    foreach (var Machine in MachineList)
                    {
                        data.HourDataByMachine.Add(new HourlyTargetAndonEntity()
                        {
                            Target = Machine,
                            Actual = "Actual",
                            cssClass= "colspan-2-class",
                            tdVisibility="none",
                            ActualColorCode = "black",
                            fontSize = HeaderFontSize,
                        });
                    }

                    hourlyTargetList.Add(data);

                    
                    data = new HourlyTargetAndonEntity();
                    data.HourTiminigs = "";
                    data.td1visible = "none";
                    foreach (var Machine in MachineList)
                    {
                        data.HourDataByMachine.Add(new HourlyTargetAndonEntity()
                        {
                            Target = "Target",
                            Actual = "Actual",
                            cssClass= "colspan-1-class",
                            tdVisibility="table-cell",
                            ActualColorCode = "black",
                            fontSize = HeaderFontSize,
                        });
                    }

                    hourlyTargetList.Add(data);



                    if (HourList != null)
                    {
                        foreach (var hour in HourList)
                        {
                            data = new HourlyTargetAndonEntity();

                            data.HourTiminigs = hour.FromTime + " - " + hour.ToTime;
                            data.fontSize = ContentFontSize;

                            var MachineWIseList = dt.AsEnumerable().Where(x => x["HourID"].ToString().Equals(hour.HourID, StringComparison.OrdinalIgnoreCase)).Select(x => new HourlyTargetAndonEntity { MachineID = x["MachineID"].ToString(), Target = x["Target"].ToString(), Actual = x["Actual"].ToString(), ActualColorCode = x["ColourCode"].ToString().Equals("white", StringComparison.OrdinalIgnoreCase) ? "black" : x["ColourCode"].ToString(), IsCurrentHour = x["IsCurrentHour"] != DBNull.Value ? (Convert.ToBoolean(x["IsCurrentHour"]) ? "IsCurrentHour" : "") : "", fontSize = ContentFontSize }).Distinct().ToList();

                            data.IsCurrentHour = MachineWIseList[0].IsCurrentHour;

                            data.HourDataByMachine = MachineWIseList;


                            hourlyTargetList.Add(data);
                        }
                    }

                    data = new HourlyTargetAndonEntity();
                    data.HourTiminigs = "Total PC";
                    data.IsCurrentHour = "TotalRow";
                    var totalList = dt.AsEnumerable().GroupBy(x => x["MachineID"].ToString()).Select(x => new HourlyTargetAndonEntity { Target = x.Sum(y => Convert.ToDouble(y["Target"].ToString())).ToString(), Actual = x.Sum(y => Convert.ToDouble(y["Actual"].ToString())).ToString(),ActualColorCode="black", fontSize = ContentFontSize, IsCurrentHour="TotalRow" }).ToList();

                   

                    data.HourDataByMachine = totalList;

                    hourlyTargetList.Add(data);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetHourlyTargetAndon= " + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return hourlyTargetList;
        }
        #endregion

    }
}