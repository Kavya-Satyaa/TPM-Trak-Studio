using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class AndonCockpitView
    {
        #region ----------Globel View Setting Part--------------
        internal static AppUISettings ViewApplicationUISettings(string userName)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            AppUISettings settings = new AppUISettings();
            string query = @"select * from CockpitDefaults where Parameter='Andon'  and ValueInText = 'AndonTitle'
                                select * from CockpitDefaults where Parameter='Andon' and ValueInText = 'AndonRefreshInterval'
                                select * from CockpitDefaults where Parameter='Andon' and ValueInText = 'AndonType'
                                select * from CockpitDefaults where Parameter='TPMTrakAppSettings' and ValueInText = 'FontFamily'
                                select * from CockpitDefaults where Parameter='TPMTrakAppSettings' and ValueInText = 'FontStyle'";

            try
            {
                cmd = new SqlCommand(query, sqlConn);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("AndonTitle"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.AndonTitle = (sdr["ValueInText2"]).ToString();
                            }
                            else
                            {
                                settings.AndonTitle = "Production Andon Data";
                            }
                        }
                    }
                }
                else
                    settings.AndonTitle = "Production Andon Data";
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("AndonRefreshInterval"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.DataDisplayInterval = (sdr["ValueInText2"]).ToString();
                            }
                            else
                            {
                                settings.DataDisplayInterval = "10";
                            }
                        }
                    }
                }
                else
                {
                    settings.DataDisplayInterval = "10";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("AndonType"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.DefaultPredefinedTimePeriod = (sdr["ValueInText2"]).ToString();
                            }
                            else
                            {
                                settings.DefaultPredefinedTimePeriod = "ShiftWise";
                            }
                        }
                    }
                }
                else
                {
                    settings.DefaultPredefinedTimePeriod = "ShiftWise";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                   
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("FontFamily"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.FontFamily = (sdr["ValueInText2"]).ToString();
                            }
                            else
                            {
                                settings.FontFamily = "Verdana";
                            }
                        }
                    }
                }
                else
                {
                    settings.FontFamily = "Verdana";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("FontStyle"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.FontStyle = (sdr["ValueInText2"]).ToString();
                            }
                            else
                            {
                                settings.FontStyle = "Regular";
                            }
                        }
                    }
                }
                else
                {
                    settings.FontStyle = "Regular";
                }
              
                sdr.Close();
                settings.PlantToDisplay = "All";
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
            string Query = @"select * from CockpitDefaults where Parameter='Andon'  and ValueInText = 'AndonFlipInterval'
                            select * from CockpitDefaults where Parameter='TPMTrakAppSettings' and ValueInText = 'ShowSmileyImage'
                            select * from CockpitDefaults where Parameter='TPMTrakAppSettings' and ValueInText = 'SmileyImageSize'
                            select * from CockpitDefaults where Parameter='TPMTrakAppSettings' and ValueInText = 'FormFontSize'";
            try
            {
                cmd = new SqlCommand(Query, sqlConn);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@user", user);
                //cmd.Parameters.AddWithValue("@param", "View");
                ////if (AndonCockpitView.GetCompany == "1") WebAndonIconicViewSettings
                ////    cmd.Parameters.AddWithValue("@Parameter", "TPMAnalyticsBWebAndonIconicViewSettings");
                ////else
                ////    cmd.Parameters.AddWithValue("@Parameter", "TPMAnalyticsWebAndonIconicViewSettings");
                //cmd.Parameters.AddWithValue("@Parameter", "WebAndonIconicViewSettings");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("AndonFlipInterval"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                                settings.ScreenFlipInterval = (sdr["ValueInText2"]).ToString();
                            else
                            {
                                settings.ScreenFlipInterval = "10";
                            }
                        }
                    }
                }
                else
                {
                    settings.ScreenFlipInterval = "10";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("ShowSmileyImage"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.ShowSmileyBlock = Convert.ToInt32(sdr["ValueInText2"]);
                                settings.ShowSmileyImage = settings.ShowSmileyBlock == 1 ? "show" : "none";
                            }
                            else
                            {
                                settings.ShowSmileyBlock = 0;
                                settings.ShowSmileyImage = settings.ShowSmileyBlock == 1 ? "show" : "none";
                            }
                        }
                    }
                }
                else
                {
                    settings.ShowSmileyBlock = 0;
                    settings.ShowSmileyImage = settings.ShowSmileyBlock == 1 ? "show" : "none";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("SmileyImageSize"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.ShowSmileyBlockSize = (sdr["ValueInText2"]).ToString();
                                settings.SmileyImageSize = settings.ShowSmileyBlockSize + "px";
                            }
                            else
                            {
                                settings.ShowSmileyBlockSize = "0";
                                settings.SmileyImageSize = settings.ShowSmileyBlockSize + "px";
                            }
                        }
                    }
                }
                else
                {
                    settings.ShowSmileyBlockSize = "0";
                    settings.SmileyImageSize = settings.ShowSmileyBlockSize + "px";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {

                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("FormFontSize"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.FormFontSize = (sdr["ValueInText2"]).ToString();
                                int fontSize = 0;
                                int.TryParse(settings.FormFontSize, out fontSize);
                                fontSize = fontSize + 6;
                                settings.FontSizeInerTab = settings.FormFontSize + "px";
                                settings.FontSizeOuterTab = fontSize.ToString() + "px";
                            }
                            else
                            {
                                settings.FormFontSize = "10";
                                int fontSize = 0;
                                int.TryParse(settings.FormFontSize, out fontSize);
                                fontSize = fontSize + 6;
                                settings.FontSizeInerTab = settings.FormFontSize + "px";
                                settings.FontSizeOuterTab = fontSize.ToString() + "px";
                            }
                        }
                    }
                }
                else
                {
                    settings.FormFontSize = "10";
                    int fontSize = 0;
                    int.TryParse(settings.FormFontSize, out fontSize);
                    fontSize = fontSize + 6;
                    settings.FontSizeInerTab = settings.FormFontSize + "px";
                    settings.FontSizeOuterTab = fontSize.ToString() + "px";
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
            string Query = @"select * from CockpitDefaults where Parameter='CockpitBackColor' and ValueInText = 'GoodColor'
                                select * from CockpitDefaults where Parameter='CockpitBackColor' and ValueInText = 'ModerateColor'
                                select * from CockpitDefaults where Parameter='CockpitBackColor' and ValueInText = 'BadColor'";
            try
            {
                cmd = new SqlCommand(Query, sqlConn);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                ////if (AndonCockpitView.GetRotate == "1")
                ////    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsBColors");
                ////else
                ////    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsColors");
                //cmd.Parameters.AddWithValue("@param", "Colors");
                //cmd.Parameters.AddWithValue("@user", user);
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                settings = new ColorUISetting();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("GoodColor"))
                        {
                            settings.GoodColor = (sdr["ValueInText2"]).ToString();
                        }
                    }
                }
                else
                {
                    settings.GoodColor = "green";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("ModerateColor"))
                        {
                            settings.ModerateColor = (sdr["ValueInText2"]).ToString();
                        }
                    }
                }
                else
                {
                    settings.GoodColor = "yellow";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("BadColor"))
                        {
                            settings.BadColor = (sdr["ValueInText2"]).ToString();
                        }
                    }
                }
                else
                {
                    settings.GoodColor = "red";
                }
                settings.CockPitLabelBackColor = "white";
                settings.CockpitLabelTextColor = "black";
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

        #region "Get All Predefined Shifts"
        public static List<string> GetAllPredefinedShifts()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> shifts = new List<string>();
            try
            {

                SqlCommand cmd = new SqlCommand(@"s_GetLookups", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"name", "PredefinedTimePeriod");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    shifts.Add(rdr["ShiftName"].ToString());
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
            return shifts;
        }
        #endregion

        #region "Get Current Shift"
        public static string CurrentShiftTime()
        {
            string shiftName = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[s_GetCurrentShift]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    sdr.Read();

                    if (!Convert.IsDBNull(sdr["ShiftName"]))
                    {
                        shiftName = sdr["ShiftName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Retriving ShiftName from s_GetCurrentShift - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return shiftName;
        }
        #endregion

        public static string GetDefaultANDONUser()
        {

            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string deafultUser = "none";
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ValueInText from ShopDefaults where Parameter = 'ANDONDefaultUser'", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    deafultUser = rdr["ValueInText"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return deafultUser;

        }

        internal static List<string> GetOrderedLabels(out List<string> listOfColNames, string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            listOfColNames = new List<string>();

            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetWebAndonColumnSettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (AndonCockpitView.GetCompany == "1")
                    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsBIconicViewVisibility");
                else
                    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsIconicViewVisibility");
                cmd.Parameters.AddWithValue("@User", user);
                cmd.CommandTimeout = 120;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["ValueInText2"].ToString());
                    listOfColNames.Add(rdr["ValueInText"].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        internal static List<CockpitData> GetMachineCockpitDetails(string procName, string fromDateTime, string shiftId, string machineId, string plantId, string Param, List<string> Orderlist,
          List<string> sortOrder, AppUISettings settings, string groupName, string user, string mode, string endDate)
        {
            Orderlist.RemoveAt(5); Orderlist.RemoveAt(5); Orderlist.RemoveAt(5); Orderlist.RemoveAt(5); Orderlist.RemoveAt(5);
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            string utilizedTime = string.Empty;
            SqlCommand cmd = null;
            CockpitData list = null;
            List<CockpitData> listOfVals = new List<CockpitData>();
            try
            {
                cmd = new SqlCommand(procName, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDateTime);//fromDateTime
                                                                        //cmd.Parameters.AddWithValue("@Enddate", endDate= endDate =="" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : endDate);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@PlantId", plantId);
                cmd.Parameters.AddWithValue("@Shift", shiftId);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@GroupID", groupName);
                cmd.Parameters.AddWithValue("@UserID", user);
                cmd.Parameters.AddWithValue("@Type", mode);
                cmd.CommandTimeout = 300;
                sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    list = new CockpitData();
                    CockpitUserControlData listData = null;
                    list.MachineId = Convert.ToString(sdr["MachineId"]);
                    //TODO Ping,connection, machineStatus
                    if (Param.Equals("MachinewiseDetails", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            if (sdr["PingStatus"].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                                list.MachineStatus = sdr["PingStatus"].ToString();
                            else if (sdr["ConnectionStatus"].ToString().Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                                list.MachineStatus = sdr["ConnectionStatus"].ToString();
                            else
                                list.MachineStatus = sdr["MachineStatus"].ToString();// Convert.ToString(sdr["MachineStatus"] == null ? "" : sdr["MachineStatus"]);
                        }
                        catch
                        {
                            list.MachineStatus = sdr["MachineStatus"].ToString();
                        }
                    }
                    else
                    {
                        list.MachineStatus = sdr["MachineStatus"].ToString();
                    }
                    //if (BindCockpitView.GetCompany == "1")
                    //    list.MachineOEE = sdr["OeColor"].ToString();
                    //else
                    list.MachineOEE = sdr["SmileyColor"].ToString();

                    if (list.MachineOEE.Equals("white", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "../Image/Smileys/Deault.png";
                    else if (list.MachineOEE.Equals("Green", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "../Image/Smileys/Green.png";
                    else if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "../Image/Smileys/Yellow.png";
                    else if (list.MachineOEE.Equals("Red", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "../Image/Smileys/Red.png";

                    if (list.MachineStatus.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        list.StatusImage = "../Image/McStatus/Stopped.gif";//list.StatusImage = "Image/McStatus/Stopped.gif";
                    else if (list.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        list.StatusImage = "../Image/McStatus/Running.gif";//list.StatusImage = "Image/McStatus/Running.gif";
                    else if (list.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                        list.StatusImage = "../Image/McStatus/PDT.gif";
                    else if (list.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                    {
                        if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || list.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                            list.StatusImage = "../Image/McStatus/ping(4).png";
                        else
                            list.StatusImage = "../Image/McStatus/ping(2).png";
                    }

                    if (Param.Equals("GroupwiseDeatails", StringComparison.OrdinalIgnoreCase))
                    {
                        list.GroupName = sdr["Machineid"].ToString();
                    }
                    // list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
                    if (string.IsNullOrEmpty(machineId))
                    {
                        for (int i = 0; i < Orderlist.Count; i++)
                        {
                            listData = new CockpitUserControlData();
                            listData.BackColor = string.Empty;
                            if (sdr[Orderlist[i]].GetType() == typeof(double))
                            {
                                listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(sdr[Orderlist[i]]));
                            }
                            else
                            {
                                listData.LabelValue = sdr[Orderlist[i]].ToString();
                            }
                            listData.ColorProperties = "#0033cc";
                            if (Orderlist[i].Equals("RunningComponent", StringComparison.OrdinalIgnoreCase))
                            {
                                if (listData.LabelValue.Equals("Part not defined for MC", StringComparison.OrdinalIgnoreCase)
                                    || listData.LabelValue.Equals("Part not defined", StringComparison.OrdinalIgnoreCase))
                                    listData.ColorProperties = "#FF0000";
                            }

                            if ((Orderlist[i].Equals("Components", StringComparison.OrdinalIgnoreCase) ||
                                //Orderlist[i].Equals("QtyGap", StringComparison.OrdinalIgnoreCase) ||
                                Orderlist[i].Equals("CurrentTimeShiftTarget", StringComparison.OrdinalIgnoreCase) ||
                                Orderlist[i].Equals("CurrentTimeQtyGap", StringComparison.OrdinalIgnoreCase)) && mode == "AndonMode")
                            {
                                listData.LabelText = sortOrder[i] + " @ " + DateTime.Now.ToString("hh:mm tt");
                            }
                            else
                            {
                                listData.LabelText = sortOrder[i];
                            }
                            listData.Tag = Orderlist[i];

                            if (Orderlist[i].Equals("AvailabilityEfficiency"))
                            {
                                listData.BackColor = Convert.ToString(sdr["AeColor"]);
                            }
                            else if (Orderlist[i].Equals("ProductionEfficiency"))
                            {
                                listData.BackColor = Convert.ToString(sdr["PeColor"]);
                            }
                            else if (Orderlist[i].Equals("OverAllEfficiency"))
                            {
                                listData.BackColor = Convert.ToString(sdr["OeColor"]);
                            }
                            list.Values.Add(listData);
                        }
                        listOfVals.Add(list);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return listOfVals;
        }

        internal static List<CockpitData> GetMachineCockpitData(string procName, string fromDateTime, string toDateTime, string plantId, string Line, string machineId, List<string> Orderlist, List<string> sortOrder, string Order, ColorUISetting settings)
        {
            DataTable dt = new DataTable();

            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            string utilizedTime = string.Empty;
            SqlCommand cmd = null;
            CockpitData list = null;
            List<CockpitData> listOfVals = new List<CockpitData>();
            try
            {
                cmd = new SqlCommand(procName, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                if ((plantId).Equals("All")) plantId = string.Empty;
                cmd.Parameters.AddWithValue("@PlantId", plantId);
                cmd.Parameters.AddWithValue("@GroupId", Line);
                cmd.Parameters.AddWithValue("@SortOrder", Order);
                cmd.CommandTimeout = 300;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        list = new CockpitData();
                        CockpitUserControlData listData = null;
                        list.MachineId = Convert.ToString(item["MachineId"]);
                        //TODO Ping,connection, machineStatus

                        list.MachineStatus = item["Remarks1"].ToString();

                        //list.MachineOEE = sdr["SmileyColor"].ToString();

                        if (list.MachineStatus.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        {
                            list.StatusImage = "../Image/McStatus/Stopped.gif";//list.StatusImage = "Image/McStatus/Stopped.gif";
                            list.MachineOEE = "Red";
                        }
                        else if (list.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            list.StatusImage = "../Image/McStatus/Running.gif";//list.StatusImage = "Image/McStatus/Running.gif";
                            list.MachineOEE = "Green";
                        }
                        else if (list.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                        {
                            list.StatusImage = "../Image/McStatus/PDT.gif";
                            list.MachineOEE = "Yellow";
                        }
                        else if (list.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                        {
                            if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || list.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                                list.StatusImage = "../Image/McStatus/ping(4).png";
                            else
                                list.StatusImage = "../Image/McStatus/ping(2).png";
                        }

                        //if (list.MachineOEE.Equals("white", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Deault.png";
                        //else if (list.MachineOEE.Equals("Green", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Green.png";
                        //else if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Yellow.png";
                        //else if (list.MachineOEE.Equals("Red", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Red.png";

                        string backColor = string.Empty, backOEEColor = string.Empty;
                        // list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
                        if (string.IsNullOrEmpty(machineId))
                        {
                            for (int i = 0; i < Orderlist.Count; i++)
                            {
                                listData = new CockpitUserControlData();
                                listData.BackColor = string.Empty;
                                backColor = string.Empty;
                                if (item[Orderlist[i]].GetType() == typeof(double))
                                {

                                    if (Orderlist[i].ToString().Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["AERed"].ToString()))
                                            {
                                                backColor = settings.BadColor == "" ? "#FFFFFF" : settings.BadColor.Remove(1, 2);
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["AEGreen"].ToString()))
                                            {
                                                backColor = settings.GoodColor == "" ? "#FFFFFF" : settings.GoodColor.Remove(1, 2);
                                            }
                                            else
                                            {
                                                backColor = settings.ModerateColor == "" ? "#FFFFFF" : settings.ModerateColor.Remove(1, 2);
                                            }
                                        }
                                        else
                                        {
                                            backColor = "#FFFFFF";
                                        }
                                    }
                                    else if (Orderlist[i].ToString().Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {

                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["PERed"].ToString()))
                                            {
                                                backColor = settings.BadColor == "" ? "#FFFFFF" : settings.BadColor.Remove(1, 2);////"Red";                                               
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["PEGreen"].ToString()))
                                            {
                                                backColor = settings.GoodColor == "" ? "#FFFFFF" : settings.GoodColor.Remove(1, 2);////"Green";                                              
                                            }
                                            else
                                            {
                                                backColor = settings.ModerateColor == "" ? "#FFFFFF" : settings.ModerateColor.Remove(1, 2);////"Yellow";                                               
                                            }
                                        }
                                        else
                                        {
                                            backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadColor;//
                                        }
                                    }
                                    else if (Orderlist[i].ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {

                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["OERed"].ToString()))
                                            {
                                                backOEEColor = backColor = settings.BadColor == "" ? "#FFFFFF" : settings.BadColor.Remove(1, 2);////"Red";
                                                list.SmileyImagePath = "../Image/Smileys/Red.png";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["OEGreen"].ToString()))
                                            {
                                                backOEEColor = backColor = settings.GoodColor == "" ? "#FFFFFF" : settings.GoodColor.Remove(1, 2);////"Green";
                                                list.SmileyImagePath = "../Image/Smileys/Green.png";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                            {
                                                backOEEColor = backColor = settings.ModerateColor == "" ? "#FFFFFF" : settings.ModerateColor.Remove(1, 2);////"Yellow";
                                                list.SmileyImagePath = "../Image/Smileys/Yellow.png";
                                            }
                                            else
                                            {
                                                backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadColor;//
                                                list.SmileyImagePath = "../Image/Smileys/Deault.png";
                                            }
                                        }
                                        else
                                        {
                                            list.SmileyImagePath = "../Image/Smileys/Deault.png";
                                            backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadColor;//
                                        }

                                    }
                                    else if (Orderlist[i].ToString().Equals("QualityEfficiency", StringComparison.OrdinalIgnoreCase)) //vas 
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["QERed"].ToString()))
                                            {
                                                backColor = settings.BadColor == "" ? "#FFFFFF" : settings.BadColor.Remove(1, 2);
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["QEGreen"].ToString()))
                                            {
                                                backColor = settings.GoodColor == "" ? "#FFFFFF" : settings.GoodColor.Remove(1, 2);
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                            {
                                                backColor = settings.ModerateColor == "" ? "#FFFFFF" : settings.ModerateColor.Remove(1, 2);
                                            }
                                            else
                                            {
                                                backColor = "#FFFFFF";
                                            }
                                        }
                                        else
                                        {
                                            backColor = "#FFFFFF";
                                        }
                                    }
                                    if (Orderlist[i].ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                        listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(item[Orderlist[i]])) + " " + "%";
                                    else
                                        listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(item[Orderlist[i]]));
                                }
                                else
                                {
                                    if (Orderlist[i].ToString().Equals("LastCycleTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (item[Orderlist[i]].ToString().Length > 7)
                                            listData.LabelValue = item[Orderlist[i]].ToString().Substring(0, 6);
                                        else
                                            listData.LabelValue = item[Orderlist[i]].ToString();
                                    }
                                    else
                                        listData.LabelValue = item[Orderlist[i]].ToString();
                                }
                                if (Orderlist[i].Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    listData.LabelText = sortOrder[i];
                                    listData.HyperLink = "HyperLink";
                                    listData.MachineName = list.MachineId;
                                }
                                else
                                {
                                    listData.LabelText = sortOrder[i];
                                    listData.HyperLink = "";
                                    listData.MachineName = list.MachineId;
                                }
                                list.BackColor = backOEEColor;
                                listData.BackColor = backColor;
                                listData.Tag = Orderlist[i];
                                list.Values.Add(listData);
                            }
                            listOfVals.Add(list);
                        }
                    }
                }
                //--------------------------
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }

            return listOfVals;
        }

        #region -------------Plant Rotating---------------
        static public string GetRotate
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["RotatingPlant"].ToString();
            }
        }
        #endregion

        #region -------------Plant Rotating---------------
        static public string GetCompany
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["Company"].ToString();
            }
        }
        #endregion

        #region "-------Bind Iconic Cockpit Setting------"
        internal static List<CockpitViewSettingClass> BindIconicCockpitSetting(string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<CockpitViewSettingClass> lstModelData = new List<CockpitViewSettingClass>();
            try
            {
                SqlCommand cmd = new SqlCommand(@"[s_GetWebAndonColumnSettings]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (AndonCockpitView.GetCompany == "1")
                    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsBIconicView");
                else
                    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsIconicView");
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

        #region ----------Update DataView Setting------------
        internal static void UpdateApplicationUISettings(string FormFontSize, string AndonTitle, string PlantToDisplay,
          string DataDisplayInterval, string ScreenFlipInterval, string ShowSmileyBlock, string FontFamily, string FontStyle,
            string ShowSmileyBlocksize, string PageSizeTableView, string BorderWidthTableView, string BorderColorTableView, string Parameter,
            string EnableImageVideo, out string isSuccessfull, string user, string enableDashBoard, string DefaultPredefinedTimePeriod)
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
                //if (BindCockpitView.GetEnergyDasboard == "1")
                //    cmd.Parameters.AddWithValue("@EnableDashBoard", enableDashBoard);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@param", "Update");
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@DefaultPredefinedTimePeriod", DefaultPredefinedTimePeriod);
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
                if (AndonCockpitView.GetCompany == "1")
                    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsBUpdate");
                else
                    cmd.Parameters.AddWithValue("@param", "TPMAnalyticsUpdate");
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

        #region ----------View Table Setting Part--------------
        internal static TableUISetting ViewTableUISettings(string user)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            if (sqlConn.State == ConnectionState.Closed) return null;
            SqlCommand cmd = null;
            SqlDataReader sdr = null;
            TableUISetting settings = new TableUISetting();
            string Query = @"select * from CockpitDefaults where Parameter='Andon'  and ValueInText = 'AndonPageSizeTableView'
                                select * from CockpitDefaults where Parameter='Andon'  and ValueInText = 'AndonFlipInterval'
                                select * from CockpitDefaults where Parameter='TPMTrakAppSettings'  and ValueInText = 'FormFontSize'";
            try
            {
                cmd = new SqlCommand(Query, sqlConn);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@param", "View");
                //cmd.Parameters.AddWithValue("@user", user);
                //cmd.Parameters.AddWithValue("@Parameter", "WebAndonTableViewSettings");
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("AndonPageSizeTableView"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.PageSizeTableView = (sdr["ValueInText2"]).ToString();
                            }
                            else
                            {
                                settings.PageSizeTableView = "10";
                            }
                        }
                    }
                }
                else
                {
                    settings.PageSizeTableView = "10";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("AndonFlipInterval"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.ScreenFlipInterval = (sdr["ValueInText2"]).ToString();
                            }
                            else
                            {
                                settings.ScreenFlipInterval = "10";
                            }
                        }
                    }
                }
                else
                {
                    settings.ScreenFlipInterval = "10";
                }
                sdr.NextResult();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].Equals("FormFontSize"))
                        {
                            if (!(sdr["ValueInText2"].ToString().Equals(DBNull.Value)))
                            {
                                settings.FormFontSize = (sdr["ValueInText2"]).ToString();
                                int fontSize = 0;
                                int.TryParse(settings.FormFontSize, out fontSize);
                                fontSize = fontSize + 4;
                                settings.FontSizeInerTab = settings.FormFontSize + "px";
                                settings.FontSizeOuterTab = fontSize.ToString() + "px";
                            }
                            else
                            {
                                settings.FormFontSize = "10";
                                int fontSize = 0;
                                int.TryParse(settings.FormFontSize, out fontSize);
                                fontSize = fontSize + 4;
                                settings.FontSizeInerTab = settings.FormFontSize + "px";
                                settings.FontSizeOuterTab = fontSize.ToString() + "px";
                            }
                        }
                    }
                }
                else
                {
                    settings.FormFontSize = "10";
                    int fontSize = 0;
                    int.TryParse(settings.FormFontSize, out fontSize);
                    fontSize = fontSize + 4;
                    settings.FontSizeInerTab = settings.FormFontSize + "px";
                    settings.FontSizeOuterTab = fontSize.ToString() + "px";
                }
                settings.BorderColorTableView = "black";
                settings.BorderWidthTableView = "2px";
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

    }
}