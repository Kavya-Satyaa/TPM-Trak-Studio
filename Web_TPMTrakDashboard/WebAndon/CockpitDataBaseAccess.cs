using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using Elmah;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.WebAndon;

namespace Web_TPMTrakDashboard.Models
{
    public class CockpitDataBaseAccess
    {
        internal static ICockpitStyle GetCockpitBackColorValues()
        {
            SqlDataReader sdr = null;
            string currentShift = string.Empty;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            ICockpitStyle vals = new ICockpitStyle();
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
                            vals.GoodRunning = sdr["ValueInText2"].ToString();
                        }
                        else if ((sdr["ValueInText"].ToString()).Equals("ModerateColor"))
                        {
                            vals.ModeratelyRunning = sdr["ValueInText2"].ToString();
                        }
                        else if ((sdr["ValueInText"].ToString()).Equals("BadColor"))
                        {
                            vals.BadlyRunning = sdr["ValueInText2"].ToString();
                        }
                        else if ((sdr["ValueInText"].ToString()).Equals("CockPitLabelBackColor"))
                        {
                            vals.CockpitLabelBackColor = sdr["ValueInText2"].ToString();
                        }
                        else if ((sdr["ValueInText"].ToString()).Equals("CockpitLabelTextColor"))
                        {
                            vals.CockpitLabelTextColor = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("PEGreaterThanHundredBackColor"))
                        {
                            vals.PEGreaterThanHundredBackColor = sdr["ValueInText2"].ToString();
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

        internal static int SaveMachineConnectDashboardSetting(string ProgramPath, string ProgramExt, string StoppageThresold, string OperationHistoryPath)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                cmd = new SqlCommand(@"[s_GetFocasAppSettings]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@param", "Update");
                cmd.Parameters.AddWithValue("@StoppagesThreshold", StoppageThresold);
                cmd.Parameters.AddWithValue("@OperationHistoryPath", OperationHistoryPath);
                cmd.Parameters.AddWithValue("@ProgramsPath", ProgramPath);
                cmd.Parameters.AddWithValue("@ProgramFileExtention", ProgramExt);
                result = cmd.ExecuteNonQuery();
                if (result != 0)
                {
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return result;
        }

        internal static MachineConnectEntity GetServiceDetails(string param)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            MachineConnectEntity MCEntity = new MachineConnectEntity();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from Focas_Defaults where Parameter='" + param + "'", conn);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["ValueInText"].ToString().Equals("SpindleDataInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            MCEntity.SpindleDataInterval = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("LiveDataInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            MCEntity.LiveDataInterval = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("AlarmDataInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            MCEntity.AlarmDataInterval = sdr["ValueInText2"].ToString();
                        }
                        else if (sdr["ValueInText"].ToString().Equals("OperationHistoryDataInterval", StringComparison.OrdinalIgnoreCase))
                        {
                            MCEntity.OperationHistoryDataInterval = sdr["ValueInText2"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return MCEntity;
        }

        internal static MachineStatusColorStyle GetMachineStatusColorValues()
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            MachineStatusColorStyle colorStyle = new MachineStatusColorStyle();
            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from Focas_MachineColorcode", conn);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (sdr["Status"].ToString().Equals("Down", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.ColorDown = sdr["Colorcode"].ToString();
                        }
                        else if (sdr["Status"].ToString().Equals("ICD", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.ColorICD = sdr["Colorcode"].ToString();
                        }
                        else if (sdr["Status"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.ColorRunning = sdr["Colorcode"].ToString();
                        }
                        else if (sdr["Status"].ToString().Equals("Alarm", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.ColorAlarm = sdr["Colorcode"].ToString();
                        }
                        else if (sdr["Status"].ToString().Equals("Load Unload", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.ColorLoadUnload = sdr["Colorcode"].ToString();
                        }
                        else if (sdr["Status"].ToString().Equals("Disconnected", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.ColorDisconnected = sdr["Colorcode"].ToString();
                        }
                        else if (sdr["Status"].ToString().Equals("Power Off", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.ColorPowerOff = sdr["Colorcode"].ToString();
                        }
                        else if (sdr["Status"].ToString().Equals("NoData", StringComparison.OrdinalIgnoreCase))
                        {
                            colorStyle.NoData = sdr["Colorcode"].ToString();
                        }
                        else
                        {
                            colorStyle.ColorOther = "white";
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
            return colorStyle;
        }

        #region "Update Color Setting Records"
        internal static int UpdateColorSettings(string Param, string valueIntext, string valueInText2)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                cmd = new SqlCommand(@"update Cockpitdefaults set [ValueInText2] = @ValueInText2 where [Parameter] = @Param and [ValueInText] =@ValueInText", sqlConn);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ValueInText2", valueInText2);
                cmd.Parameters.AddWithValue("@ValueInText", valueIntext);
                result = cmd.ExecuteNonQuery();

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
            return result;
        }
        #endregion

        internal static List<string> GetOrderedLabels(out List<string> listOfColNames, string param, string LanguageSpecified)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            listOfColNames = new List<string>();

            try
            {
                SqlCommand cmd = new SqlCommand(@"select * from CockpitDefaults where parameter = @Param and ValueInBool = '1' and LanguageSpecified=@LanguageSpecified order by valueinint  ", sqlConn);
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@LanguageSpecified", LanguageSpecified);
                cmd.CommandType = System.Data.CommandType.Text;
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
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }
        internal static int getLiveCockpitDateInterval()
        {
            int interval = 7;
            try
            {
                interval = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["LiveCockpitDateInterval"].ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {

            }
            return interval;
        }

        internal static int SaveCockpitBackColor(string Val, string Param)
        {
            int res = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"if not exists(select * from CockpitDefaults where Parameter=@Param)
                begin
                insert into CockpitDefaults(Parameter,ValueInText)values(@Param,@ValueInText)
                end
                else
                begin
                update CockpitDefaults set ValueInText=@ValueInText where Parameter=@Param
                end", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ValueInText", Val);
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        internal static int SaveDataCollectionValue(string SerialNo, string WorkOrder, string HeatCode)
        {
            int res = 0;
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(@"update SmartdataPortRefreshDefaults set SerialNumber='" + SerialNo + "'UPDATE SmartdataPortRefreshDefaults SET HeatCode='" + HeatCode + "'UPDATE smartdataportrefreshdefaults SET WorkOrder= '" + WorkOrder + "'", conn);
                cmd.CommandType = CommandType.Text;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return res;
        }

        #region "Bind Ionic View Data"
        //internal static List<ICockpitData> GetMachineCockpitDetails(string procName, string fromDateTime, string toDateTime, string plantId, string machineId, List<string> Orderlist, string sortOrder)
        //{
        //    SqlDataReader sdr = null;
        //    SqlConnection conn = ConnectionManager.GetConnection();
        //    string utilizedTime = string.Empty;
        //    SqlCommand cmd = null;
        //    ICockpitData list = null;
        //    List<ICockpitData> listOfVals = new List<ICockpitData>();
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        cmd = new SqlCommand(procName, conn);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
        //        cmd.Parameters.AddWithValue("@EndTime", toDateTime);
        //        cmd.Parameters.AddWithValue("@MachineId", machineId);
        //        if ((plantId).Equals("All")) plantId = string.Empty;
        //        cmd.Parameters.AddWithValue("@PlantId", plantId);
        //        cmd.Parameters.AddWithValue("@SortOrder", sortOrder);
        //        cmd.CommandTimeout = 120;
        //        sdr = cmd.ExecuteReader();

        //        while (sdr.Read())
        //        {
        //            list = new ICockpitData();
        //            list.MachineId = Convert.ToString(sdr["MachineId"]);
        //            list.MachineStatus = Convert.ToString(sdr["Remarks1"]);
        //            //list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
        //            utilizedTime = Convert.ToString(sdr["utilisedTime"]);
        //            if (string.IsNullOrEmpty(machineId))
        //            {
        //                for (int i = 0; i < Orderlist.Count; i++)
        //                {
        //                    if (utilizedTime.Equals("0"))
        //                    {
        //                        list.MachineRemarks = "Machine Not In Production";
        //                    }
        //                    else
        //                    {
        //                        list.MachineRemarks = string.Empty;
        //                    }
        //                    //Use the visibility set to true                          
        //                    if (sdr[Orderlist[i]].GetType() == typeof(double))
        //                    {
        //                        string backColor = string.Empty;
        //                        if (Orderlist[i] == "AvailabilityEfficiency")
        //                        {
        //                            if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
        //                            {
        //                                if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["AERed"].ToString()))
        //                                {
        //                                    backColor = "Red";//Cockpit.CockpitStyle.BadlyRunning;
        //                                }
        //                                else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["AEGreen"].ToString()))
        //                                {
        //                                    backColor = "Green";//Cockpit.CockpitStyle.GoodRunning;
        //                                }
        //                                else
        //                                {
        //                                    backColor = "Blue";//Cockpit.CockpitStyle.ModeratelyRunning;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                backColor = "0xFFFFFF";
        //                            }
        //                        }
        //                        else if (Orderlist[i] == "ProductionEfficiency")
        //                        {
        //                            if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
        //                            {

        //                                if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["PERed"].ToString()))
        //                                {
        //                                    backColor = "Red";//Cockpit.CockpitStyle.BadlyRunning;////
        //                                }
        //                                else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["PEGreen"].ToString()))
        //                                {
        //                                    backColor = "Green";//Cockpit.CockpitStyle.GoodRunning;////
        //                                }
        //                                else
        //                                {
        //                                    backColor = "Yellow";//Cockpit.CockpitStyle.ModeratelyRunning;////
        //                                }
        //                            }
        //                            else
        //                            {
        //                                backColor = "0xFFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
        //                            }
        //                        }
        //                        else if (Orderlist[i] == "OverAllEfficiency")
        //                        {
        //                            if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
        //                            {

        //                                if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["OERed"].ToString()))
        //                                {
        //                                    backColor = "Red"; //Cockpit.CockpitStyle.BadlyRunning;
        //                                }
        //                                else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["OEGreen"].ToString()))
        //                                {
        //                                    backColor = "Green";//Cockpit.CockpitStyle.GoodRunning;////
        //                                }
        //                                else if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
        //                                {
        //                                    backColor = "Yellow";//Cockpit.CockpitStyle.ModeratelyRunning;////
        //                                }
        //                                else
        //                                {
        //                                    backColor = ColorTranslator.FromHtml("0xFFFFFF").ToString();// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
        //                                }
        //                            }
        //                            else
        //                            {
        //                                backColor = "0xFFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
        //                            }

        //                        }
        //                        else if (Orderlist[i] == "QualityEfficiency") //vas 
        //                        {
        //                            if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
        //                            {
        //                                if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["QERed"].ToString()))
        //                                {
        //                                    backColor = "Red";//Cockpit.CockpitStyle.BadlyRunning;
        //                                }
        //                                else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["QEGreen"].ToString()))
        //                                {
        //                                    backColor = "Green";// Cockpit.CockpitStyle.GoodRunning;
        //                                }
        //                                else if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
        //                                {
        //                                    backColor = "Blue";//Cockpit.CockpitStyle.ModeratelyRunning;
        //                                }
        //                                else
        //                                {
        //                                    backColor = ColorTranslator.FromHtml("0xFFFFFF").ToString();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                backColor = "0xFFFFFF";
        //                            }
        //                        }

        //                        else if (Orderlist[i] == "RevenueLoss")
        //                        {
        //                            list.Values.Add(new ICockpitUserControlData
        //                            {
        //                                Value = "Rs. " + sdr[Orderlist[i]].ToString(),
        //                                DBkey = Orderlist[i],
        //                                BackColor = string.Empty
        //                            });
        //                        }

        //                        list.Values.Add(new ICockpitUserControlData
        //                        {
        //                            Value = String.Format("{0:0.##}", Convert.ToDouble(sdr[Orderlist[i]])),
        //                            DBkey = Orderlist[i],
        //                            BackColor = backColor,
        //                        });
        //                    }


        //                    else
        //                    {
        //                        list.Values.Add(new ICockpitUserControlData
        //                        {
        //                            Value = sdr[Orderlist[i]].ToString(),
        //                            DBkey = Orderlist[i],
        //                            BackColor = string.Empty
        //                        });
        //                    }
        //                    //list.Values.Add(String.Format("{0:0.##}", Convert.ToDouble(sdr[Orderlist[i]]))); 
        //                }
        //                listOfVals.Add(list);
        //            }
        //            else
        //            {
        //                dt.Load(sdr);
        //                dt.AcceptChanges();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
        //        throw;
        //    }
        //    finally
        //    {
        //        if (sdr != null) sdr.Close();
        //        if (conn != null) conn.Close();
        //    }

        //    return listOfVals;
        //}
        #endregion

        internal static List<CockpitData> GetMachineCockpitDetails(string procName, string fromDateTime, string toDateTime, string plantId, string machineId, List<string> Orderlist, List<string> sortOrder, string Order, ICockpitStyle settings)
        {
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
                cmd.Parameters.AddWithValue("@SortOrder", Order);
                cmd.CommandTimeout = 300;
                sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    list = new CockpitData();
                    CockpitUserControlData listData = null;
                    list.MachineId = Convert.ToString(sdr["MachineId"]);
                    //TODO Ping,connection, machineStatus

                    list.MachineStatus = sdr["Remarks1"].ToString();

                    //list.MachineOEE = sdr["SmileyColor"].ToString();

                    if (list.MachineStatus.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                    {
                        list.StatusImage = "~/Image/McStatus/Stopped.gif";//list.StatusImage = "Image/McStatus/Stopped.gif";
                        list.MachineOEE = "Red";
                    }
                    else if (list.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                    {
                        list.StatusImage = "~/Image/McStatus/Running.gif";//list.StatusImage = "Image/McStatus/Running.gif";
                        list.MachineOEE = "Green";
                    }
                    else if (list.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                    {
                        list.StatusImage = "~/Image/McStatus/PDT.gif";
                        list.MachineOEE = "Yellow";
                    }
                    else if (list.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                    {
                        if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || list.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                            list.StatusImage = "~/Image/McStatus/ping(4).png";
                        else
                            list.StatusImage = "~/Image/McStatus/ping(2).png";
                    }

                    if (list.MachineOEE.Equals("white", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Deault.png";
                    else if (list.MachineOEE.Equals("Green", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Green.png";
                    else if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Yellow.png";
                    else if (list.MachineOEE.Equals("Red", StringComparison.OrdinalIgnoreCase))
                        list.SmileyImagePath = "Image/Smileys/Red.png";

                    string backColor = string.Empty, backOEEColor = string.Empty;
                    // list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
                    if (string.IsNullOrEmpty(machineId))
                    {
                        for (int i = 0; i < Orderlist.Count; i++)
                        {
                            listData = new CockpitUserControlData();
                            listData.BackColor = string.Empty;
                            backColor = string.Empty;
                            if (sdr[Orderlist[i]].GetType() == typeof(double))
                            {

                                if (Orderlist[i].ToString().Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
                                    {
                                        if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["AERed"].ToString()))
                                        {
                                            backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                        }
                                        else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["AEGreen"].ToString()))
                                        {
                                            backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                        }
                                        else
                                        {
                                            backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                        }
                                    }
                                    else
                                    {
                                        backColor = "#FFFFFF";
                                    }
                                }
                                else if (Orderlist[i].ToString().Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
                                    {

                                        if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["PERed"].ToString()))
                                        {
                                            backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red";
                                        }
                                        else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["PEGreen"].ToString()))
                                        {
                                            backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green";
                                        }
                                        else
                                        {
                                            backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow";
                                        }
                                    }
                                    else
                                    {
                                        backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                    }
                                }
                                else if (Orderlist[i].ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
                                    {

                                        if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["OERed"].ToString()))
                                        {
                                            backOEEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red";
                                        }
                                        else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["OEGreen"].ToString()))
                                        {
                                            backOEEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green";
                                        }
                                        else if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
                                        {
                                            backOEEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow";
                                        }
                                        else
                                        {
                                            backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                        }
                                    }
                                    else
                                    {
                                        backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                    }

                                }
                                else if (Orderlist[i].ToString().Equals("QualityEfficiency", StringComparison.OrdinalIgnoreCase)) //vas 
                                {
                                    if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
                                    {
                                        if (Convert.ToDouble(sdr[Orderlist[i]]) <= Int32.Parse(sdr["QERed"].ToString()))
                                        {
                                            backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                        }
                                        else if (Convert.ToDouble(sdr[Orderlist[i]]) >= Int32.Parse(sdr["QEGreen"].ToString()))
                                        {
                                            backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                        }
                                        else if (Convert.ToDouble(sdr[Orderlist[i]]) > 0)
                                        {
                                            backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
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
                                    listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(sdr[Orderlist[i]])) + " " + "%";
                                else
                                    listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(sdr[Orderlist[i]]));
                            }
                            else
                            {
                                if (Orderlist[i].ToString().Equals("LastCycleTime", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sdr[Orderlist[i]].ToString().Length > 7)
                                        listData.LabelValue = sdr[Orderlist[i]].ToString().Substring(0, 6);
                                    else
                                        listData.LabelValue = sdr[Orderlist[i]].ToString();
                                }
                                else
                                    listData.LabelValue = sdr[Orderlist[i]].ToString();
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


        internal static List<CockpitData> GetMachineCockpitData(string procName, string fromDateTime, string toDateTime, string plantId, string machineId, List<string> Orderlist, List<string> sortOrder, string Order, ICockpitStyle settings, string cellId, string reportType, string sortordercolumn)
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
                dt = CachedData.getChachedData(fromDateTime, toDateTime);
                string BackColorParam = GetParamForCockpitBackColor();
                if (dt == null)
                {
                    dt = new DataTable();
                    cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                    cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                    cmd.Parameters.AddWithValue("@MachineId", machineId);
                    if ((plantId).Equals("All")) plantId = string.Empty;
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    //cmd.Parameters.AddWithValue("@SortOrder", Order);
                    cmd.Parameters.AddWithValue("@GroupID", cellId);
                    cmd.Parameters.AddWithValue("@param", reportType);
                    if (sortordercolumn == "CustomSortorder")
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", "");
                        cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", sortordercolumn);
                        cmd.Parameters.AddWithValue("@SortType", "");
                    }
                    cmd.CommandTimeout = 300;
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> dtColumnList = dt.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
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
                            list.StatusImage = "~/Image/McStatus/Stopped.gif";//list.StatusImage = "Image/McStatus/Stopped.gif";
                            list.MachineOEE = "Red";
                        }
                        else if (list.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        {
                            list.StatusImage = "~/Image/McStatus/Running.gif";//list.StatusImage = "Image/McStatus/Running.gif";
                            list.MachineOEE = "Green";
                        }
                        else if (list.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                        {
                            list.StatusImage = "~/Image/McStatus/PDT.gif";
                            list.MachineOEE = "Yellow";
                        }
                        else if (list.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                        {
                            if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || list.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                                list.StatusImage = "~/Image/McStatus/ping(4).png";
                            else
                            {
                                list.StatusImage = "~/Image/McStatus/ping(2).png";
                                list.MachineOEE = "white";
                            }
                        }
                        else
                        {
                            list.MachineOEE = "white";
                        }
                        list.AERed = item["AERed"].ToString();
                        list.AEGreen = item["AEGreen"].ToString();
                        list.PERed = item["PERed"].ToString();
                        list.PEGreen = item["PEGreen"].ToString();
                        list.QERed = item["QERed"].ToString();
                        list.QEGreen = item["QEGreen"].ToString();
                        list.OEERed = item["OERed"].ToString();
                        list.OEEGreen = item["OEGreen"].ToString();
                        list.OperatorPEGreen = item["OPRGreen"].ToString();
                        list.OperatorPERed = item["OPRRed"].ToString();
                        list.GoodColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                        list.BadColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                        list.ModerateColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                        //if (list.MachineOEE.Equals("white", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Deault.png";
                        //else if (list.MachineOEE.Equals("Green", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Green.png";
                        //else if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Yellow.png";
                        //else if (list.MachineOEE.Equals("Red", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Red.png";

                        string backColor = string.Empty, backOEEColor = string.Empty, foreColor = string.Empty, backColorTitle = string.Empty, foreColorTitle = string.Empty, backAEColor = string.Empty, backPEColor = string.Empty, backOperatorPEColor = string.Empty;
                        // list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
                        //if (string.IsNullOrEmpty(machineId))
                        //{
                        for (int i = 0; i < Orderlist.Count; i++)
                        {
                            try
                            {
                                if (Orderlist[i].ToString().Equals("OperatorPEEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    Orderlist[i] = "OperatorEfficiency";
                                }
                                else if (Orderlist[i].ToString().Equals("OverallOperatorPEEfficiency", StringComparison.OrdinalIgnoreCase))
                                {
                                    Orderlist[i] = "OverallOperatorEfficiency";
                                }
                                var columnExistence = dtColumnList.Where(k => k.Equals(Orderlist[i], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
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
                                if (item[Orderlist[i]].GetType() == typeof(double))
                                {

                                    if (Orderlist[i].ToString().Equals("AvailabilityEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["AERed"].ToString()))
                                            {
                                                backAEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["AEGreen"].ToString()))
                                            {
                                                backAEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else
                                            {
                                                backAEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                        }
                                        else
                                        {
                                            backAEColor = backColor = "#FFFFFF";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else if (Orderlist[i].ToString().Equals("ProductionEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {

                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["PERed"].ToString()))
                                            {
                                                backPEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red"; 
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["PEGreen"].ToString()))
                                            {
                                                if (ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    if (Convert.ToDouble(item[Orderlist[i]]) > 100 && !string.IsNullOrEmpty(settings.PEGreaterThanHundredBackColor))
                                                    {
                                                        backPEColor = backColor = settings.PEGreaterThanHundredBackColor == "" ? "#FFFFFF" : settings.PEGreaterThanHundredBackColor;////"Purple"; 
                                                        listData.LabelValueToolTip = "Check Cycle Time";
                                                    }
                                                    else
                                                        backPEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green"; 
                                                }
                                                else
                                                    backPEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green"; 
                                                foreColor = "#000000";
                                            }
                                            else
                                            {
                                                backPEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow"; 
                                                foreColor = "#000000";
                                            }
                                        }
                                        else
                                        {
                                            backPEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                            foreColor = "#000000";
                                        }
                                    }
                                    else if (Orderlist[i].ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase) || Orderlist[i].ToString().Equals("OverallOperatorEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {

                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["OERed"].ToString()))
                                            {
                                                backOEEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red";
                                                list.SmileyImagePath = "Image/Smileys/Red.png";
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["OEGreen"].ToString()))
                                            {
                                                backOEEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green";
                                                list.SmileyImagePath = "Image/Smileys/Green.png";
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                            {
                                                backOEEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow";
                                                list.SmileyImagePath = "Image/Smileys/Yellow.png";
                                                foreColor = "#000000";
                                            }
                                            else
                                            {
                                                backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                                list.SmileyImagePath = "Image/Smileys/Deault.png";
                                                foreColor = "#000000";
                                            }
                                        }
                                        else
                                        {
                                            list.SmileyImagePath = "Image/Smileys/Deault.png";
                                            backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                            foreColor = "#000000";
                                        }

                                    }
                                    else if (Orderlist[i].ToString().Equals("QualityEfficiency", StringComparison.OrdinalIgnoreCase)) //vas 
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["QERed"].ToString()))
                                            {
                                                backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["QEGreen"].ToString()))
                                            {
                                                backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                            {
                                                backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else
                                            {
                                                backColor = "#FFFFFF";
                                                foreColor = "#000000";
                                            }
                                        }
                                        else
                                        {
                                            backColor = "#FFFFFF";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else if (Orderlist[i].ToString().Equals("OperatorEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["OPRRed"].ToString()))
                                            {
                                                backOperatorPEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["OPRGreen"].ToString()))
                                            {
                                                backOperatorPEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                            {
                                                backOperatorPEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                                foreColor = "#000000";
                                            }
                                            else
                                            {
                                                backOperatorPEColor = backColor = "#FFFFFF";
                                                foreColor = "#000000";
                                            }
                                        }
                                        else
                                        {
                                            backOperatorPEColor = backColor = "#FFFFFF";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else if (Orderlist[i].ToString().Contains("Running") || Orderlist[i].ToString().Contains("Spindle"))
                                    {
                                        backColor = "#4A87C9";
                                        backColorTitle = "#4A87C9";
                                        foreColor = "#FBFBFB";
                                        foreColorTitle = "#FBFBFB";
                                    }
                                    else if (Orderlist[i].ToString().StartsWith("Last", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = "#4A87C9";
                                        backColorTitle = "#4A87C9";
                                        foreColor = "#FBFBFB";
                                        foreColorTitle = "#FBFBFB";
                                    }
                                    //else if (Orderlist[i].ToString().Equals("LastCycleCO", StringComparison.OrdinalIgnoreCase))
                                    //{
                                    //    backColor = "#4A87C9";
                                    //    backColorTitle = "#4A87C9";
                                    //    foreColor = "#FBFBFB";
                                    //    foreColorTitle = "#FBFBFB";
                                    //}
                                    else
                                    {
                                        backColor = "#FBFBFB";
                                        backColorTitle = "#FBFBFB";
                                        foreColor = "#000000";
                                        foreColorTitle = "#000000";
                                    }
                                    if (Orderlist[i].ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                        listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(item[Orderlist[i]])) + " " + "%";
                                    else
                                        listData.LabelValue = Convert.ToDouble(item[Orderlist[i]]).ToString();
                                }
                                else
                                {
                                    if (Orderlist[i].ToString().Equals("LastCycleTime", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = "#FBFBFB";
                                        backColorTitle = "#FBFBFB";
                                        foreColor = "#000000";
                                        foreColorTitle = "#000000";
                                        //if (item[Orderlist[i]].ToString().Length > 7)
                                        //    listData.LabelValue = item[Orderlist[i]].ToString().Substring(0, 6);
                                        //else
                                        listData.LabelValue = item[Orderlist[i]].ToString();
                                    }
                                    else if (Orderlist[i].ToString().Contains("Running") || Orderlist[i].ToString().Contains("Spindle"))
                                    {
                                        backColor = "#4A87C9";
                                        backColorTitle = "#4A87C9";
                                        foreColor = "#FBFBFB";
                                        foreColorTitle = "#FBFBFB";
                                        listData.LabelValue = item[Orderlist[i]].ToString();
                                    }
                                    else if (Orderlist[i].ToString().Equals("LastCycleStart", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = "#4A87C9";
                                        backColorTitle = "#4A87C9";
                                        foreColor = "#FBFBFB";
                                        foreColorTitle = "#FBFBFB";
                                        listData.LabelValue = item[Orderlist[i]].ToString();
                                    }
                                    else if (Orderlist[i].ToString().Equals("LastCycleCO", StringComparison.OrdinalIgnoreCase))
                                    {
                                        backColor = "#4A87C9";
                                        backColorTitle = "#4A87C9";
                                        foreColor = "#FBFBFB";
                                        foreColorTitle = "#FBFBFB";
                                        listData.LabelValue = item[Orderlist[i]].ToString();
                                    }
                                    else
                                    {
                                        backColor = "#FBFBFB";
                                        backColorTitle = "#FBFBFB";
                                        foreColor = "#000000";
                                        foreColorTitle = "#000000";
                                        listData.LabelValue = item[Orderlist[i]].ToString();

                                    }
                                }
                                if (Orderlist[i].Equals("Target", StringComparison.OrdinalIgnoreCase))
                                {
                                    var targetRes = dt.AsEnumerable().Select(k => k.Field<double?>("Target")).Distinct().ToList();
                                    if (targetRes.Count == 1)
                                    {
                                        if (targetRes[0] == null)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (Orderlist[i].Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase) || Orderlist[i].Equals("AirPressureCnt", StringComparison.OrdinalIgnoreCase) || Orderlist[i].Equals("SpindleRunTime", StringComparison.OrdinalIgnoreCase))
                                {
                                    listData.LabelText = sortOrder[i];
                                    listData.MachineName = list.MachineId;
                                    if (Orderlist[i].Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                    {
                                        listData.HyperLink = "HyperLink HL_OEE";
                                    }
                                    if (reportType.Equals("Machinewise", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (Orderlist[i].Equals("AirPressureCnt", StringComparison.OrdinalIgnoreCase))
                                        {
                                            listData.HyperLink = "HyperLink HL_AirPressure";
                                        }
                                        else if (Orderlist[i].Equals("SpindleRunTime", StringComparison.OrdinalIgnoreCase))
                                        {
                                            listData.HyperLink = "HyperLink HL_SpindleRuntime";
                                        }
                                    }
                                }
                                else
                                {
                                    listData.LabelText = sortOrder[i];
                                    listData.HyperLink = "";
                                    listData.MachineName = list.MachineId;
                                }
                                list.BackColor = BackColorParam.Equals("AE", StringComparison.OrdinalIgnoreCase) ? backAEColor : BackColorParam.Equals("PE", StringComparison.OrdinalIgnoreCase) ? backPEColor : BackColorParam.Equals("OEE", StringComparison.OrdinalIgnoreCase) ? backOEEColor : BackColorParam.Equals("OperatorPE", StringComparison.OrdinalIgnoreCase) ? backOperatorPEColor : backOEEColor;
                                listData.BackColor = backColor;
                                listData.BackColorTitle = backColorTitle;
                                listData.ForeColor = foreColor;
                                listData.ForeColorTitle = foreColorTitle;
                                listData.Tag = Orderlist[i];
                                list.Values.Add(listData);
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteErrorLog(ex.Message);
                            }
                        }
                        listOfVals.Add(list);
                        //}
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

        internal static bool GetExcludeTPMTrakDown()
        {
            bool Val = false;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select  [ValueInInt] from [CockpitDefaults] where [Parameter] = 'ExcludeTPMTrakDown'", conn);
                cmd.CommandType = CommandType.Text;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Val = sdr["ValueInInt"].ToString() == "1" ? true : false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return Val;
        }

        internal static int SaveMachineConnectServiceSetting(string ValueinIext2, string ValueInText, string Param)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            int result = 0;
            try
            {
                cmd = new SqlCommand(@"if not exists(select * from Focas_Defaults where Parameter=@Param and ValueInText=@ValueInText)
                begin
                insert into Focas_Defaults(Parameter, ValueInText, ValueInText2) values(@Param, @ValueInText, @ValueInText2)
                end
                else
                begin
                update Focas_Defaults set ValueInText2= @ValueInText2 where Parameter = @Param and ValueInText = @ValueInText
                end", sqlConn);
                cmd.Parameters.AddWithValue("@Param", Param);
                cmd.Parameters.AddWithValue("@ValueInText2", ValueinIext2);
                cmd.Parameters.AddWithValue("@ValueInText", ValueInText);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);//("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return result;
        }

        internal static MachineConnectEntity GetFocusSettingDetails()
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            MachineConnectEntity mc = new MachineConnectEntity();
            try
            {
                cmd = new SqlCommand(@"[s_GetFocasAppSettings]", conn);
                cmd.Parameters.AddWithValue("@param", "View");
                cmd.CommandType = CommandType.StoredProcedure;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    if (sdr["ValueInText"].ToString().Equals("ProgramsPath", StringComparison.OrdinalIgnoreCase))
                    {
                        mc.ProgramPath = sdr["ValueInText2"].ToString();
                    }
                    else if (sdr["ValueInText"].ToString().Equals("ProgramFileExtention", StringComparison.OrdinalIgnoreCase))
                    {
                        mc.ProgramFileExtension = sdr["ValueInText2"].ToString();
                    }
                    else if (sdr["ValueInText"].ToString().Equals("OperationHistoryPath", StringComparison.OrdinalIgnoreCase))
                    {
                        mc.OperationHistoryFolderPath = sdr["ValueInText2"].ToString();
                    }
                    else if (sdr["Parameter"].ToString().Equals("DowntimeThreshold", StringComparison.OrdinalIgnoreCase))
                    {
                        mc.StoppageThreshold = sdr["ValueInText"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return mc;
        }

        internal static string bindExtensionCombobox()
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string lst = string.Empty;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select valueInText2 from  Focas_Defaults  where valueInText = @valueInText and parameter = @parameter";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@valueInText", "ValidProgramFileExtentions");
                cmd.Parameters.AddWithValue("@parameter", "FocasAppSettings");
                sdr = cmd.ExecuteReader();
                {
                    while (sdr.Read())
                    {

                        lst = sdr["ValueInText2"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return lst;
        }
        internal static DataCollectionEntity GetDataCollectionDetails()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            DataCollectionEntity val = new DataCollectionEntity();
            try
            {
                cmd = new SqlCommand(@"select WorkOrder,SerialNumber,HeatCode from SmartdataPortRefreshDefaults", sqlConn);
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    val.WorkOrder = rdr.GetString(0);
                    val.SerialNo = rdr.GetString(1);
                    val.HeatCode = rdr.GetString(2);
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
            return val;
        }
        internal static string GetParamForCockpitBackColor()
        {
            string Param = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select ValueInText from Cockpitdefaults where parameter = 'CockpitBackColorParam'", conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Param = sdr["ValueInText"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return Param;
        }
        internal static DataTable GetMachineCockpitTotalData(string procName, string fromDateTime, string toDateTime, string plantId, string machineId, List<string> Orderlist, List<string> sortOrder, string Order, ICockpitStyle settings, string cellId, string reportType, string sortordercolumn)
        {
            DataTable dt = new DataTable();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            string utilizedTime = string.Empty;
            SqlCommand cmd = null;
            try
            {
                dt = CachedData.getChachedData(fromDateTime, toDateTime);
                if (dt == null)
                {
                    dt = new DataTable();
                    cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                    cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                    cmd.Parameters.AddWithValue("@MachineId", machineId);
                    if ((plantId).Equals("All")) plantId = string.Empty;
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    //cmd.Parameters.AddWithValue("@SortOrder", Order);
                    cmd.Parameters.AddWithValue("@GroupID", cellId);
                    cmd.Parameters.AddWithValue("@param", reportType);
                    if (sortordercolumn == "CustomSortorder")
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", "");
                        cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", sortordercolumn);
                        cmd.Parameters.AddWithValue("@SortType", "");
                    }
                    cmd.CommandTimeout = 300;
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
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

            return dt;
        }
        internal static List<CockpitData> GetMachineCockpitDataForPlantCell(string procName, string fromDateTime, string toDateTime, string plantId, string machineId, List<string> Orderlist, List<string> sortOrder, string Order, ICockpitStyle settings, string cellId, string reportType, string sortordercolumn)
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
                dt = CachedData.getChachedData(fromDateTime, toDateTime);
                string BackColorParam = GetParamForCockpitBackColor();
                if (dt == null)
                {
                    dt = new DataTable();
                    cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                    cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                    cmd.Parameters.AddWithValue("@MachineId", machineId);
                    if ((plantId).Equals("All")) plantId = string.Empty;
                    cmd.Parameters.AddWithValue("@PlantId", plantId);
                    //cmd.Parameters.AddWithValue("@SortOrder", Order);
                    cmd.Parameters.AddWithValue("@GroupID", cellId);
                    cmd.Parameters.AddWithValue("@param", reportType);
                    if (sortordercolumn == "CustomSortorder")
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", "");
                        cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", sortordercolumn);
                        cmd.Parameters.AddWithValue("@SortType", "");
                    }
                    cmd.CommandTimeout = 300;
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> dtColumnList = dt.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
                    foreach (DataRow item in dt.Rows)
                    {
                        list = new CockpitData();
                        CockpitUserControlData listData = null;
                        if (reportType == "Plantwise")
                        {
                            list.PlantID = Convert.ToString(item["Plantid"]);
                        }
                        else if (reportType == "cellwise")
                        {
                            list.PlantID = Convert.ToString(item["Plantid"]);
                            list.GroupName = Convert.ToString(item["Groupid"]);
                        }

                        //list.MachineId = Convert.ToString(item["MachineId"]);
                        //TODO Ping,connection, machineStatus

                        //list.MachineStatus = item["Remarks1"].ToString();

                        //list.MachineOEE = sdr["SmileyColor"].ToString();

                        //if (list.MachineStatus.Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    list.StatusImage = "~/Image/McStatus/Stopped.gif";//list.StatusImage = "Image/McStatus/Stopped.gif";
                        //    list.MachineOEE = "Red";
                        //}
                        //else if (list.MachineStatus.Equals("Running", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    list.StatusImage = "~/Image/McStatus/Running.gif";//list.StatusImage = "Image/McStatus/Running.gif";
                        //    list.MachineOEE = "Green";
                        //}
                        //else if (list.MachineStatus.Equals("PDT", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    list.StatusImage = "~/Image/McStatus/PDT.gif";
                        //    list.MachineOEE = "Yellow";
                        //}
                        //else if (list.MachineStatus.Equals("NOT OK", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase) || list.MachineOEE.Equals("White", StringComparison.OrdinalIgnoreCase))
                        //        list.StatusImage = "~/Image/McStatus/ping(4).png";
                        //    else
                        //    {
                        //        list.StatusImage = "~/Image/McStatus/ping(2).png";
                        //        list.MachineOEE = "white";
                        //    }
                        //}
                        //else
                        //{
                        //    list.MachineOEE = "white";
                        //}
                        //if (list.MachineOEE.Equals("white", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Deault.png";
                        //else if (list.MachineOEE.Equals("Green", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Green.png";
                        //else if (list.MachineOEE.Equals("Yellow", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Yellow.png";
                        //else if (list.MachineOEE.Equals("Red", StringComparison.OrdinalIgnoreCase))
                        //    list.SmileyImagePath = "Image/Smileys/Red.png";

                        string backColor = string.Empty, backOEEColor = string.Empty, foreColor = string.Empty, backColorTitle = string.Empty, foreColorTitle = string.Empty, backAEColor = string.Empty, backPEColor = string.Empty, backOperatorOEColor = string.Empty;
                        // list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
                        //if (string.IsNullOrEmpty(machineId))
                        //{
                        for (int i = 0; i < Orderlist.Count; i++)
                        {
                            if (Orderlist[i].Equals("OPEffy", StringComparison.OrdinalIgnoreCase))
                            {
                                if (reportType == "cellwise")
                                {
                                    Orderlist[i] = "Offy";
                                }
                                else
                                {
                                    Orderlist[i] = "OPREffy";
                                }
                            }
                            else if (Orderlist[i].Equals("OverallOPEffy", StringComparison.OrdinalIgnoreCase))
                            {
                                if (reportType == "cellwise")
                                {
                                    Orderlist[i] = "OOffy";
                                }
                                else
                                {
                                    Orderlist[i] = "OverallOPREffy";
                                }
                            }
                            var columnExistence = dtColumnList.Where(k => k.Equals(Orderlist[i], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
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
                            if (item[Orderlist[i]].GetType() == typeof(double))
                            {

                                if (Orderlist[i].ToString().Equals("AEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["AERed"].ToString()))
                                        {
                                            backAEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["AEGreen"].ToString()))
                                        {
                                            backAEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backAEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        backAEColor = backColor = "#FFFFFF";
                                        foreColor = "#000000";
                                    }
                                }
                                else if (Orderlist[i].ToString().Equals("PEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {

                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["PERed"].ToString()))
                                        {
                                            backPEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red"; 
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["PEGreen"].ToString()))
                                        {
                                            if (ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (Convert.ToDouble(item[Orderlist[i]]) > 100 && !string.IsNullOrEmpty(settings.PEGreaterThanHundredBackColor))
                                                {
                                                    backPEColor = backColor = settings.PEGreaterThanHundredBackColor == "" ? "#FFFFFF" : settings.PEGreaterThanHundredBackColor;////"Purple"; 
                                                    listData.LabelValueToolTip = "Check Cycle Time";
                                                }
                                                else
                                                    backPEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green"; 
                                            }
                                            else
                                                backPEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green"; 
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backPEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow"; 
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        backPEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                        foreColor = "#000000";
                                    }
                                }
                                else if (Orderlist[i].ToString().Equals("OEffy", StringComparison.OrdinalIgnoreCase) || Orderlist[i].ToString().Equals("OOffy", StringComparison.OrdinalIgnoreCase) || Orderlist[i].ToString().Equals("OverallOPREffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {

                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["OERed"].ToString()))
                                        {
                                            backOEEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red";
                                            list.SmileyImagePath = "Image/Smileys/Red.png";
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["OEGreen"].ToString()))
                                        {
                                            backOEEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green";
                                            list.SmileyImagePath = "Image/Smileys/Green.png";
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            backOEEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow";
                                            list.SmileyImagePath = "Image/Smileys/Yellow.png";
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                            list.SmileyImagePath = "Image/Smileys/Deault.png";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        list.SmileyImagePath = "Image/Smileys/Deault.png";
                                        backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                        foreColor = "#000000";
                                    }

                                }
                                else if (Orderlist[i].ToString().Equals("QEffy", StringComparison.OrdinalIgnoreCase)) //vas 
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["QERed"].ToString()))
                                        {
                                            backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["QEGreen"].ToString()))
                                        {
                                            backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backColor = "#FFFFFF";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        backColor = "#FFFFFF";
                                        foreColor = "#000000";
                                    }
                                }
                                else if (Orderlist[i].ToString().Equals("OPREffy", StringComparison.OrdinalIgnoreCase) || Orderlist[i].ToString().Equals("Offy", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["OPRRed"].ToString()))
                                        {
                                            backOperatorOEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["OPRGreen"].ToString()))
                                        {
                                            backOperatorOEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            backOperatorOEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backOperatorOEColor = backColor = "#FFFFFF";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        backOperatorOEColor = backColor = "#FFFFFF";
                                        foreColor = "#000000";
                                    }
                                }
                                else if (Orderlist[i].ToString().Contains("Running") || Orderlist[i].ToString().Contains("Spindle"))
                                {
                                    backColor = "#4A87C9";
                                    backColorTitle = "#4A87C9";
                                    foreColor = "#FBFBFB";
                                    foreColorTitle = "#FBFBFB";
                                }
                                else if (Orderlist[i].ToString().StartsWith("Last", StringComparison.OrdinalIgnoreCase))
                                {
                                    backColor = "#4A87C9";
                                    backColorTitle = "#4A87C9";
                                    foreColor = "#FBFBFB";
                                    foreColorTitle = "#FBFBFB";
                                }
                                //else if (Orderlist[i].ToString().Equals("LastCycleCO", StringComparison.OrdinalIgnoreCase))
                                //{
                                //    backColor = "#4A87C9";
                                //    backColorTitle = "#4A87C9";
                                //    foreColor = "#FBFBFB";
                                //    foreColorTitle = "#FBFBFB";
                                //}
                                else
                                {
                                    backColor = "#FBFBFB";
                                    backColorTitle = "#FBFBFB";
                                    foreColor = "#000000";
                                    foreColorTitle = "#000000";
                                }
                                if (Orderlist[i].ToString().Equals("OEffy", StringComparison.OrdinalIgnoreCase))
                                    listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(item[Orderlist[i]])) + " " + "%";
                                else
                                    listData.LabelValue = Convert.ToDouble(item[Orderlist[i]]).ToString();
                            }
                            else
                            {
                                if (Orderlist[i].ToString().Equals("LastCycleTime", StringComparison.OrdinalIgnoreCase))
                                {
                                    backColor = "#FBFBFB";
                                    backColorTitle = "#FBFBFB";
                                    foreColor = "#000000";
                                    foreColorTitle = "#000000";
                                    //if (item[Orderlist[i]].ToString().Length > 7)
                                    //    listData.LabelValue = item[Orderlist[i]].ToString().Substring(0, 6);
                                    //else
                                    listData.LabelValue = item[Orderlist[i]].ToString();
                                }
                                else if (Orderlist[i].ToString().Contains("Running") || Orderlist[i].ToString().Contains("Spindle"))
                                {
                                    backColor = "#4A87C9";
                                    backColorTitle = "#4A87C9";
                                    foreColor = "#FBFBFB";
                                    foreColorTitle = "#FBFBFB";
                                    listData.LabelValue = item[Orderlist[i]].ToString();
                                }
                                else if (Orderlist[i].ToString().Equals("LastCycleStart", StringComparison.OrdinalIgnoreCase))
                                {
                                    backColor = "#4A87C9";
                                    backColorTitle = "#4A87C9";
                                    foreColor = "#FBFBFB";
                                    foreColorTitle = "#FBFBFB";
                                    listData.LabelValue = item[Orderlist[i]].ToString();
                                }
                                else if (Orderlist[i].ToString().Equals("LastCycleCO", StringComparison.OrdinalIgnoreCase))
                                {
                                    backColor = "#4A87C9";
                                    backColorTitle = "#4A87C9";
                                    foreColor = "#FBFBFB";
                                    foreColorTitle = "#FBFBFB";
                                    listData.LabelValue = item[Orderlist[i]].ToString();
                                }
                                else
                                {
                                    backColor = "#FBFBFB";
                                    backColorTitle = "#FBFBFB";
                                    foreColor = "#000000";
                                    foreColorTitle = "#000000";
                                    listData.LabelValue = item[Orderlist[i]].ToString();
                                }
                            }
                            if (Orderlist[i].Equals("OEffy", StringComparison.OrdinalIgnoreCase))
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
                            list.BackColor = BackColorParam.Equals("AE", StringComparison.OrdinalIgnoreCase) ? backAEColor : BackColorParam.Equals("PE", StringComparison.OrdinalIgnoreCase) ? backPEColor : BackColorParam.Equals("OEE", StringComparison.OrdinalIgnoreCase) ? backOEEColor : BackColorParam.Equals("OperatorPE", StringComparison.OrdinalIgnoreCase) ? backOperatorOEColor : backOEEColor;
                            listData.BackColor = backColor;
                            listData.BackColorTitle = backColorTitle;
                            listData.ForeColor = foreColor;
                            listData.ForeColorTitle = foreColorTitle;
                            listData.Tag = Orderlist[i];
                            list.Values.Add(listData);
                        }
                        listOfVals.Add(list);
                        //}
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
        internal static DataTable GetTabelCockpitDetails(string procName, string fromDateTime, string toDateTime, string plantId, string cellId, string viewType, string sortOrderColumn, string machineId)
        {
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                cmd = new SqlCommand(procName, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", fromDateTime);
                cmd.Parameters.AddWithValue("@EndTime", toDateTime);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                if ((plantId).Equals("All")) plantId = string.Empty;
                cmd.Parameters.AddWithValue("@PlantId", plantId);
                //cmd.Parameters.AddWithValue("@SortOrder", "");
                cmd.Parameters.AddWithValue("@GroupID", cellId);
                cmd.Parameters.AddWithValue("@param", viewType);
                if (sortOrderColumn == "CustomSortorder")
                {
                    cmd.Parameters.AddWithValue("@SortOrder", "");
                    cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SortOrder", sortOrderColumn);
                    cmd.Parameters.AddWithValue("@SortType", "");
                }
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    if (row["Remarks1"].ToString().Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                    //        row["Remarks1"] = "Image/McStatus/Stopped.gif";
                    //    else if (row["Remarks1"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase))
                    //        row["Remarks1"] = "Image/McStatus/Running.gif";
                    //}
                    dt.AcceptChanges();
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

            return dt;
        }

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

        #region "Get Logic Start Day"
        public static string GetLogicalDay(string dateVal)
        {
            string list = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select [dbo].[f_GetLogicalDayStart](' " + dateVal + "') as logicalDay";
                cmd = new SqlCommand(sqlQuery, conn);
                //cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["logicalDay"]))
                        {
                            list = DateTime.Parse((sdr["logicalDay"]).ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                else
                {
                    list = null;
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
            return list;
        }
        #endregion

        #region "Get Logic End Day"
        public static string GetLogicalDayEnd(string dateVal)
        {
            string list = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select [dbo].[f_GetLogicalDayEnd](' " + dateVal + "') as logicalDay";
                cmd = new SqlCommand(sqlQuery, conn);
                //cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["logicalDay"]))
                        {
                            list = DateTime.Parse((sdr["logicalDay"]).ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                else
                {
                    list = null;
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
            return list;
        }
        #endregion

        #region "Current Shift "
        public static List<string> GetCurrentShiftTime(string shiftVal)
        {
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "[s_GetShiftTime]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shiftVal);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["StartTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (!Convert.IsDBNull(sdr["EndTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
                else
                {
                    list = null;
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
            return list;
        }
        #endregion

        internal static List<string> GetMachinesForCell(string cellId, string plant)
        {
            string sqlQuery = string.Empty;
            plant = plant == "All" ? "" : plant;
            List<string> listMachineId = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            if (cellId == "Cell All" || cellId == "All")
            {
                sqlQuery = @"select MachineID from PlantMachineGroups where PlantID = @PlantID OR  @PlantID = ''";
            }
            else
            {
                sqlQuery = @"select MachineID from PlantMachineGroups where (PlantID = @PlantID OR @PlantID ='') and GroupID = @GroupID";
            }
            try
            {
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@PlantID", plant);
                cmd.Parameters.AddWithValue("@GroupID", cellId);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        listMachineId.Add(sdr["MachineID"].ToString());
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
            return listMachineId;
        }
        internal static string getHistoricalAnalyticsDefaultView(string parameter, string text1)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            string view = "";
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ValueInText2 from CockpitDefaults where Parameter=@parameter and ValueInText=@text1", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@parameter", parameter);
                cmd.Parameters.AddWithValue("@text1", text1);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        view = sdr["ValueInText2"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                view = "";
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());

            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return view;
        }
        internal static List<CockpitData> GetMachineCockpitDataaggregate(string procName, string fromDateTime, string toDateTime, string plantId, string machineId, List<string> Orderlist, List<string> sortOrder, string Order, ICockpitStyle settings, string cellId, string reportType, string sortOrderColumn)
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

                dt = CachedData.getChachedData(fromDateTime, toDateTime);
                string BackColorParam = GetParamForCockpitBackColor();
                if (dt == null)
                {
                    dt = new DataTable();
                    cmd = new SqlCommand(procName, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", fromDateTime);
                    cmd.Parameters.AddWithValue("@EndDate", toDateTime);
                    cmd.Parameters.AddWithValue("@ShiftName", "");
                    cmd.Parameters.AddWithValue("@PlantID", plantId == "All" ? "" : plantId);
                    cmd.Parameters.AddWithValue("@MachineId", machineId);

                    //cmd.Parameters.AddWithValue("@SortOrder", Order);
                    cmd.Parameters.AddWithValue("@GroupID", cellId);
                    cmd.Parameters.AddWithValue("@ReportType", reportType);
                    cmd.Parameters.AddWithValue("@Parameter", "SONA_AggCockpit");
                    if (sortOrderColumn == "CustomSortorder")
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", "");
                        cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SortOrder", sortOrderColumn);
                        cmd.Parameters.AddWithValue("@SortType", "");
                    }
                    cmd.CommandTimeout = 300;
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> dtColumnList = dt.Columns.Cast<DataColumn>().Select(k => k.ColumnName).ToList();
                    foreach (DataRow item in dt.Rows)
                    {
                        list = new CockpitData();
                        CockpitUserControlData listData = null;
                        if (reportType == "Plantwise")
                        {
                            list.PlantID = Convert.ToString(item["Plantid"]);
                        }
                        else if (reportType == "cellwise")
                        {
                            list.PlantID = Convert.ToString(item["Plantid"]);
                            list.GroupName = Convert.ToString(item["Groupid"]);
                        }
                        else if (reportType == "Machinewise")
                        {
                            list.PlantID = Convert.ToString(item["Plantid"]);
                            list.GroupName = Convert.ToString(item["Groupid"]);
                            list.MachineId = Convert.ToString(item["MachineId"]);
                        }


                        string backColor = string.Empty, backOEEColor = string.Empty, foreColor = string.Empty, backColorTitle = string.Empty, foreColorTitle = string.Empty, backAEColor = string.Empty, backPEColor = string.Empty, backOperatorPEColor = string.Empty;
                        // list.MachineRemarks = Convert.ToString(sdr["Remarks"]);
                        //if (string.IsNullOrEmpty(machineId))
                        //{
                        for (int i = 0; i < Orderlist.Count; i++)
                        {
                            var columnExistence = dtColumnList.Where(k => k.Equals(Orderlist[i], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
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
                            //string columnname="";
                            //if (Orderlist[i].ToString() == "MachineDescription")
                            //{
                            //    if (reportType == "Plantwise")
                            //    {
                            //        columnname = "PlantDescription";
                            //    }
                            //    else if (reportType == "cellwise")
                            //    {
                            //        columnname = "GroupDescription";
                            //    }
                            //    else if (reportType == "Machinewise")
                            //    {
                            //        columnname = Orderlist[i];
                            //    }
                            //}
                            //else
                            //{
                            //    columnname = Orderlist[i];
                            //}
                            if (item[Orderlist[i]].GetType() == typeof(double))
                            {

                                if (Orderlist[i].ToString().Equals("AEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["AERed"].ToString()))
                                        {
                                            backAEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["AEGreen"].ToString()))
                                        {
                                            backAEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backAEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        backAEColor = backColor = "#FFFFFF";
                                        foreColor = "#000000";
                                    }
                                }
                                else if (Orderlist[i].ToString().Equals("PEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {

                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["PERed"].ToString()))
                                        {
                                            backPEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red"; 
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["PEGreen"].ToString()))
                                        {
                                            if (ConfigurationManager.AppSettings["DantalHydraulicsPages"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (Convert.ToDouble(item[Orderlist[i]]) > 100 && !string.IsNullOrEmpty(settings.PEGreaterThanHundredBackColor))
                                                {
                                                    backPEColor = backColor = settings.PEGreaterThanHundredBackColor == "" ? "#FFFFFF" : settings.PEGreaterThanHundredBackColor;////"Purple"; 
                                                    listData.LabelValueToolTip = "Check Cycle Time";
                                                }
                                                else
                                                    backPEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green"; 
                                            }
                                            else
                                                backPEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green"; 
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backPEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow"; 
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        backPEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                        foreColor = "#000000";
                                    }
                                }
                                else if (Orderlist[i].ToString().Equals("OEffy", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {

                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["OERed"].ToString()))
                                        {
                                            backOEEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red";
                                            list.SmileyImagePath = "Image/Smileys/Red.png";
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["OEGreen"].ToString()))
                                        {
                                            backOEEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green";
                                            list.SmileyImagePath = "Image/Smileys/Green.png";
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            backOEEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow";
                                            list.SmileyImagePath = "Image/Smileys/Yellow.png";
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                            list.SmileyImagePath = "Image/Smileys/Deault.png";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        list.SmileyImagePath = "Image/Smileys/Deault.png";
                                        backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                        foreColor = "#000000";
                                    }

                                }
                                else if (Orderlist[i].ToString().Equals("QEffy", StringComparison.OrdinalIgnoreCase)) //vas 
                                {
                                    if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["QERed"].ToString()))
                                        {
                                            backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["QEGreen"].ToString()))
                                        {
                                            backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);
                                            foreColor = "#000000";
                                        }
                                        else
                                        {
                                            backColor = "#FFFFFF";
                                            foreColor = "#000000";
                                        }
                                    }
                                    else
                                    {
                                        backColor = "#FFFFFF";
                                        foreColor = "#000000";
                                    }
                                }
                                else
                                {
                                    backColor = "#FBFBFB";
                                    backColorTitle = "#FBFBFB";
                                    foreColor = "#000000";
                                    foreColorTitle = "#000000";
                                }
                                if (Orderlist[i].ToString().Equals("OverAllEfficiency", StringComparison.OrdinalIgnoreCase))
                                    listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(item[Orderlist[i]])) + " " + "%";
                                else
                                    listData.LabelValue = String.Format("{0:00}", Convert.ToDouble(item[Orderlist[i]]));
                                if (Orderlist[i].ToString().Equals("AcceptedParts", StringComparison.OrdinalIgnoreCase))
                                {
                                    listData.LabelValue = Convert.ToDouble(item[Orderlist[i]]).ToString("00.00");
                                }
                                if (Orderlist[i].ToString().Equals("ProdCount", StringComparison.OrdinalIgnoreCase))
                                {
                                    listData.LabelValue = Convert.ToDouble(item[Orderlist[i]]).ToString("00.00");
                                }

                            }
                            else
                            {
                                backColor = "#FBFBFB";
                                backColorTitle = "#FBFBFB";
                                foreColor = "#000000";
                                foreColorTitle = "#000000";
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

                            list.BackColor = BackColorParam.Equals("AE", StringComparison.OrdinalIgnoreCase) ? backAEColor : BackColorParam.Equals("PE", StringComparison.OrdinalIgnoreCase) ? backPEColor : BackColorParam.Equals("OEE", StringComparison.OrdinalIgnoreCase) ? backOEEColor : backOEEColor;
                            listData.BackColor = backColor;
                            listData.BackColorTitle = backColorTitle;
                            listData.ForeColor = foreColor;
                            listData.ForeColorTitle = foreColorTitle;
                            listData.Tag = Orderlist[i];
                            list.Values.Add(listData);
                        }
                        listOfVals.Add(list);
                        // }
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
        internal static DataTable GetTabelCockpitDetailsAggregated(string procName, string fromDateTime, string toDateTime, string plantId, string cellId, string viewtype, string sortOrderColumn, string machineid)
        {
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            SqlConnection conn = ConnectionManager.GetConnection();
            try
            {
                cmd = new SqlCommand(procName, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromDateTime);
                cmd.Parameters.AddWithValue("@EndDate", toDateTime);
                cmd.Parameters.AddWithValue("@ShiftName", "");
                cmd.Parameters.AddWithValue("@MachineID", machineid);
                if ((plantId).Equals("All")) plantId = string.Empty;
                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.Parameters.AddWithValue("@ReportType", viewtype);
                cmd.Parameters.AddWithValue("@Parameter", "SONA_AggCockpit");
                if ((cellId).Equals("All")) cellId = string.Empty;
                cmd.Parameters.AddWithValue("@GroupID", cellId);
                if (sortOrderColumn == "CustomSortorder")
                {
                    cmd.Parameters.AddWithValue("@SortOrder", "");
                    cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SortOrder", sortOrderColumn);
                    cmd.Parameters.AddWithValue("@SortType", "");
                }
                cmd.CommandTimeout = 120;
                sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    if (row["Remarks1"].ToString().Equals("Stopped", StringComparison.OrdinalIgnoreCase))
                    //        row["Remarks1"] = "Image/McStatus/Stopped.gif";
                    //    else if (row["Remarks1"].ToString().Equals("Running", StringComparison.OrdinalIgnoreCase))
                    //        row["Remarks1"] = "Image/McStatus/Running.gif";
                    //}
                    dt.AcceptChanges();
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

            return dt;
        }

        public static List<RuntimeDataEntity> GetRuntimechartData(DateTime startDate, DateTime toDate, string shift, string plantId, string machineId, string param, out int MaxVal)
        {
            SqlConnection Con = ConnectionManager.GetConnection();// new SqlConnection(@"Data Source=AMIT-DEV7\SQL2017STD; Initial Catalog=Peekay_29-10-2018;User ID=sa; password=pctadmin$123");// 
            SqlCommand Cmd = null;
            SqlDataReader sdr = null; MaxVal = 0;
            DataTable RuntimeData = new DataTable();
            List<RuntimeDataEntity> DataList = new List<RuntimeDataEntity>();
            machineId = machineId == "All" ? "" : machineId; int i = -1; string Machineid = "";
            plantId = plantId == "All" ? "" : plantId;
            try
            {
                Cmd = new SqlCommand("s_GetMachineUptimeDowntimeDetails", Con);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@StartTime", startDate.ToString("yyyy-MM-dd HH:mm:ss"));//"2018-10-24 06:00:00"
                Cmd.Parameters.AddWithValue("@EndTime", toDate.ToString("yyyy-MM-dd HH:mm:ss"));//"2018-10-24 14:00:00"																
                shift = shift.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : shift;
                Cmd.Parameters.AddWithValue("@shift", shift);
                //Cmd.Parameters.AddWithValue("@Machineid", "'" + drpMachineId.Text + "'");
                Cmd.Parameters.AddWithValue("@Machineid", machineId);
                Cmd.Parameters.AddWithValue("@plantid", plantId);
                Cmd.Parameters.AddWithValue("@param", param);
                sdr = Cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        RuntimeDataEntity data = new RuntimeDataEntity();
                        if (Machineid != sdr["MachineID"].ToString())
                        {
                            i++;
                            Machineid = sdr["MachineID"].ToString();
                        }
                        data.MachineName = sdr["MachineID"].ToString();
                        data.Color = sdr["Color"].ToString();
                        //data.Status = sdr["Status"].ToString();
                        data.StartDate = Convert.ToDateTime(sdr["Starttime"].ToString());
                        data.EndDate = Convert.ToDateTime(sdr["Endtime"].ToString());
                        data.MachineID = i;
                        DataList.Add(data);
                    }
                }
                MaxVal = i + 1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (Con != null) Con.Close();
            }
            return DataList;
        }

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

        internal static List<string> GetShiftTime(string shiftVal, string selectedDate)
        {
            List<string> list = new List<string>();
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            DateTime dateValue = Util.GetDateTime(selectedDate);
            try
            {
                sqlQuery = "[s_GetShiftTime]";
                cmd = new SqlCommand(sqlQuery, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDateTime", dateValue.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Shift", shiftVal);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (!Convert.IsDBNull(sdr["StartTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        if (!Convert.IsDBNull(sdr["EndTime"]))
                        {
                            list.Add(DateTime.Parse(sdr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
                else
                {
                    list = null;
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
            return list;
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

        internal static DataTable GetMachineCockpitDataForPlantCell(string procName, string fromDateTime, string toDateTime, string cellId)
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
                cmd.Parameters.AddWithValue("@MachineId", "");
                cmd.Parameters.AddWithValue("@PlantId", "");
                cmd.Parameters.AddWithValue("@GroupID", cellId);
                cmd.Parameters.AddWithValue("@param", "Machinewise");
                cmd.Parameters.AddWithValue("@SortOrder", "");
                cmd.Parameters.AddWithValue("@SortType", "CustomSortorder");
                cmd.CommandTimeout = 300;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);
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

            return dt;
        }
        #region------Modified Data Settings- Application seting
        internal static DataTable GetModifiedSettingsData()
        {
            DataTable dt = new DataTable();
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"select * from CockpitDefaults where Parameter in ('EnableProductionLog','EnableDownLog','HistoricalDashboardChartType','VDGModifiedDataBackColor','HistoricalDashboardCombinedCharts')", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return dt;
        }
        internal static int SavemodifiedDataSettings(string Parameter,string Value)
        {
            int count = 0;
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(@"update CockpitDefaults set ValueInText=@ValueInText where Parameter=@Parameter", con);
                cmd.Parameters.AddWithValue("@Parameter", Parameter);
                cmd.Parameters.AddWithValue("@ValueInText", Value);
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return count;
        }
        #endregion
    }
}