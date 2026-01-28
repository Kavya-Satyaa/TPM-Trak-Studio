using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class AndonSettingsScreenDataBaseAccess
    {
        #region "-------Bind Iconic Cockpit Setting------"
        internal static List<CockpitViewSettingClass> BindIconicCockpitSetting(string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<CockpitViewSettingClass> lstModelData = new List<CockpitViewSettingClass>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetWebAndonColumnSettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "IconicView");
                cmd.Parameters.AddWithValue("@User", user);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CockpitViewSettingClass modelData = new CockpitViewSettingClass();
                    modelData.Parameter = rdr["Parameter"].ToString();
                    modelData.ValueInText = rdr["ValueInText"].ToString();
                    modelData.ValueInText2 = rdr["ValueInText2"].ToString();
                    if (string.IsNullOrEmpty(rdr["ValueInInt"].ToString()))
                        modelData.ValueInInt = 0;
                    else
                        modelData.ValueInInt = (int)rdr["ValueInInt"];
                    if (string.IsNullOrEmpty(rdr["ValueInBool"].ToString()) || rdr["ValueInBool"].ToString() == "1")
                        modelData.ValueInBool = true;
                    else
                        modelData.ValueInBool = false;

                    lstModelData.Add(modelData);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstModelData;
        }
        #endregion

        #region "-------Bind Table Cockpit Setting------"
        internal static List<CockpitViewSettingClass> BindTableCockpitSetting(string user, string param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<CockpitViewSettingClass> lstModelData = new List<CockpitViewSettingClass>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetWebAndonColumnSettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", param);
                cmd.Parameters.AddWithValue("@User", user);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CockpitViewSettingClass modelData = new CockpitViewSettingClass();
                    modelData.Parameter = rdr["Parameter"].ToString();
                    modelData.ValueInText = rdr["ValueInText"].ToString();
                    modelData.ValueInText2 = rdr["ValueInText2"].ToString();
                    if (string.IsNullOrEmpty(rdr["ValueInInt"].ToString()))
                        modelData.ValueInInt = 0;
                    else
                        modelData.ValueInInt = (int)rdr["ValueInInt"];
                    if (string.IsNullOrEmpty(rdr["ValueInBool"].ToString()) || rdr["ValueInBool"].ToString() == "1")
                        modelData.ValueInBool = true;
                    else
                        modelData.ValueInBool = false;

                    lstModelData.Add(modelData);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return lstModelData;
        }
        #endregion

        #region ----------Globel View Setting Part--------------
        internal static AppUISettings ViewApplicationUISettings(string userName)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            AppUISettings settings = new AppUISettings();
            try
            {
                cmd = new SqlCommand(@"[s_GetWebAndonUISettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Parameter", "WebAndonGobalSettings");
                cmd.Parameters.AddWithValue("@param", "View");
                cmd.Parameters.AddWithValue("@user", userName);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("AndonTitle"))
                        {
                            settings.AndonTitle = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("PlantToDisplay"))
                        {
                            settings.PlantToDisplay = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("DataDisplayInterval"))
                        {
                            settings.DataDisplayInterval = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("FontFamily"))
                        {
                            settings.FontFamily = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("FontStyle"))
                        {
                            settings.FontStyle = (sdr["ValueInText2"]).ToString();
                        }
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return settings;
        }
        #endregion

        #region ----------View Iconic Setting Part--------------
        internal static IconicUISetting ViewIconicUISettings(string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            IconicUISetting settings = new IconicUISetting();
            try
            {
                cmd = new SqlCommand(@"[s_GetWebAndonUISettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "View");
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@Parameter", "WebAndonIconicViewSettings");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("ScreenFlipInterval"))
                        {
                            settings.ScreenFlipInterval = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("ShowSmileyBlock"))
                        {
                            settings.ShowSmileyBlock = Convert.ToInt32(sdr["ValueInInt"]);
                            settings.ShowSmileyImage = settings.ShowSmileyBlock == 1 ? "show" : "none";
                        }
                        else if (sdr["ValueInText"].Equals("ShowSmileyBlockSize"))
                        {
                            settings.ShowSmileyBlockSize = (sdr["ValueInText2"]).ToString();
                            settings.SmileyImageSize = settings.ShowSmileyBlockSize + "px";
                        }
                        else if (sdr["ValueInText"].Equals("FormFontSize"))
                        {
                            settings.FormFontSize = (sdr["ValueInText2"]).ToString();
                            int fontSize = 0;
                            int.TryParse(settings.FormFontSize, out fontSize);
                            fontSize = fontSize + 6;
                            settings.FontSizeInerTab = settings.FormFontSize + "px";
                            settings.FontSizeOuterTab = fontSize.ToString() + "px";
                        }
                        else if (sdr["ValueInText"].Equals("EnableImageVideo"))
                        {
                            settings.EnableImageVideo = (sdr["ValueInInt"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("EnableDashBoard"))
                        {
                            settings.EnableDashBoard = (sdr["ValueInInt"]).ToString();
                        }
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return settings;
        }
        #endregion

        #region ----------View Table Setting Part--------------
        internal static TableUISetting ViewTableUISettings(string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            TableUISetting settings = new TableUISetting();
            try
            {
                cmd = new SqlCommand(@"[s_GetWebAndonUISettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "View");
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@Parameter", "WebAndonTableViewSettings");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("BorderColorTableView"))
                        {
                            settings.BorderColorTableView = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("BorderWidthTableView"))
                        {
                            settings.BorderWidthTableView = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("PageSizeTableView"))
                        {
                            settings.PageSizeTableView = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("ScreenFlipInterval"))
                        {
                            settings.ScreenFlipInterval = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("FormFontSize"))
                        {
                            settings.FormFontSize = (sdr["ValueInText2"]).ToString();
                            int fontSize = 0;
                            int.TryParse(settings.FormFontSize, out fontSize);
                            fontSize = fontSize + 4;
                            settings.FontSizeInerTab = settings.FormFontSize + "px";
                            settings.FontSizeOuterTab = fontSize.ToString() + "px";
                        }
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return settings;
        }
        #endregion

        #region "------Populate Setting Color----------"
        internal static ColorUISetting ViewColorSettingData(string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            ColorUISetting settings = null;
            try
            {
                cmd = new SqlCommand(@"[s_GetWebAndonUISettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "Colors");
                cmd.Parameters.AddWithValue("@user", user);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    settings = new ColorUISetting();
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("GoodColor"))
                        {
                            settings.GoodColor = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("ModerateColor"))
                        {
                            settings.ModerateColor = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("BadColor"))
                        {
                            settings.BadColor = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("CockPitLabelBackColor"))
                        {
                            settings.CockPitLabelBackColor = (sdr["ValueInText2"]).ToString();
                        }
                        else if (sdr["ValueInText"].Equals("CockpitLabelTextColor"))
                        {
                            settings.CockpitLabelTextColor = (sdr["ValueInText2"]).ToString();
                        }
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return settings;
        }
        #endregion

        #region ----------Update DataView Setting------------
        internal static void UpdateApplicationUISettings(string FormFontSize, string AndonTitle, string PlantToDisplay,
          string DataDisplayInterval, string ScreenFlipInterval, string ShowSmileyBlock, string FontFamily, string FontStyle,
            string ShowSmileyBlocksize, string PageSizeTableView, string BorderWidthTableView, string BorderColorTableView, string Parameter,
            string EnableImageVideo, out string isSuccessfull, string user, string enableDashBoard)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            isSuccessfull = string.Empty;
            try
            {
                cmd = new SqlCommand(@"[s_GetWebAndonUISettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FormFontSize", FormFontSize);
                cmd.Parameters.AddWithValue("@AndonTitle", AndonTitle);
                cmd.Parameters.AddWithValue("@PlantToDisplay", PlantToDisplay);
                cmd.Parameters.AddWithValue("@DataDisplayInterval", DataDisplayInterval);
                cmd.Parameters.AddWithValue("@ScreenFlipInterval", ScreenFlipInterval);
                cmd.Parameters.AddWithValue("@ShowSmileyBlock", ShowSmileyBlock);
                cmd.Parameters.AddWithValue("@FontFamily", FontFamily);
                cmd.Parameters.AddWithValue("@FontStyle", FontStyle);
                cmd.Parameters.AddWithValue("@ShowSmileyBlockSize", ShowSmileyBlocksize);
                cmd.Parameters.AddWithValue("@PageSizeTableView", PageSizeTableView);
                cmd.Parameters.AddWithValue("@BorderWidthTableView", BorderWidthTableView);
                cmd.Parameters.AddWithValue("@BorderColorTableView", BorderColorTableView);
                cmd.Parameters.AddWithValue("@EnableImageVideo", EnableImageVideo);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@param", "Update");
                cmd.Parameters.AddWithValue("@user", user);
                int a = cmd.ExecuteNonQuery();
                isSuccessfull = "Successfull";
            }
            catch (Exception ex)
            {
                isSuccessfull = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region ----------Update DataView Setting------------
        internal static void UpdateMachineColorSetting(string GoodColor, string ModerateColor, string BadColor,
          string CockPitLabelBackColor, string CockpitLabelTextColor, out string isSuccessfull, string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            isSuccessfull = string.Empty;
            try
            {
                cmd = new SqlCommand(@"[s_GetMachineColorSetting]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GoodColor", "#FF" + GoodColor.ToUpper());
                cmd.Parameters.AddWithValue("@ModerateColor", "#FF" + ModerateColor.ToUpper());
                cmd.Parameters.AddWithValue("@BadColor", "#FF" + BadColor.ToUpper());
                cmd.Parameters.AddWithValue("@User", user);
                //cmd.Parameters.AddWithValue("@CockPitLabelBackColor", "#FF" + CockPitLabelBackColor.ToUpper());
                //cmd.Parameters.AddWithValue("@CockpitLabelTextColor", "#FF" + CockpitLabelTextColor.ToUpper());
                cmd.Parameters.AddWithValue("@param", "Update");
                cmd.ExecuteNonQuery();
                isSuccessfull = "Successfull";
            }
            catch (Exception ex)
            {
                isSuccessfull = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region ------Bind Plant To Display----------
        internal static List<AppUISettings> ViewPlantToDisplay()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            List<AppUISettings> lstPlantData = new List<AppUISettings>();
            try
            {
                cmd = new SqlCommand(@"[s_GetLookups]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", "Plant");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        AppUISettings modelData = new AppUISettings();
                        modelData.PlantToDisplay = sdr["Plantid"].ToString();
                        lstPlantData.Add(modelData);
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (sqlConn != null) sqlConn.Close();
            }
            return lstPlantData;
        }
        #endregion
    }
}