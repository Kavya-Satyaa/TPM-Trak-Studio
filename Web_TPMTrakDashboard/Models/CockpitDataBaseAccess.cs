using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using Elmah;

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


        internal static List<CockpitData> GetMachineCockpitData(string procName, string fromDateTime, string toDateTime, string plantId, string machineId, List<string> Orderlist, List<string> sortOrder, string Order, ICockpitStyle settings)
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
                    cmd.Parameters.AddWithValue("@SortOrder", Order);
                    cmd.CommandTimeout = 300;
                    sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                }
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
                                                backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["AEGreen"].ToString()))
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
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {

                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["PERed"].ToString()))
                                            {
                                                backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red";                                               
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["PEGreen"].ToString()))
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
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {

                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["OERed"].ToString()))
                                            {
                                                backOEEColor = backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);////"Red";
                                                list.SmileyImagePath = "Image/Smileys/Red.png";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["OEGreen"].ToString()))
                                            {
                                                backOEEColor = backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);////"Green";
                                                list.SmileyImagePath = "Image/Smileys/Green.png";
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                            {
                                                backOEEColor = backColor = settings.ModeratelyRunning == "" ? "#FFFFFF" : settings.ModeratelyRunning.Remove(1, 2);////"Yellow";
                                                list.SmileyImagePath = "Image/Smileys/Yellow.png";
                                            }
                                            else
                                            {
                                                backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                                list.SmileyImagePath = "Image/Smileys/Deault.png";
                                            }
                                        }
                                        else
                                        {
                                            list.SmileyImagePath = "Image/Smileys/Deault.png";
                                            backOEEColor = backColor = "#FFFFFF";// backColor =  Cockpit.CockpitStyle.BadlyRunning;//
                                        }

                                    }
                                    else if (Orderlist[i].ToString().Equals("QualityEfficiency", StringComparison.OrdinalIgnoreCase)) //vas 
                                    {
                                        if (Convert.ToDouble(item[Orderlist[i]]) > 0)
                                        {
                                            if (Convert.ToDouble(item[Orderlist[i]]) <= Int32.Parse(item["QERed"].ToString()))
                                            {
                                                backColor = settings.BadlyRunning == "" ? "#FFFFFF" : settings.BadlyRunning.Remove(1, 2);
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) >= Int32.Parse(item["QEGreen"].ToString()))
                                            {
                                                backColor = settings.GoodRunning == "" ? "#FFFFFF" : settings.GoodRunning.Remove(1, 2);
                                            }
                                            else if (Convert.ToDouble(item[Orderlist[i]]) > 0)
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

        internal static DataTable GetTabelCockpitDetails(string procName, string fromDateTime, string toDateTime, string plantId)
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
                cmd.Parameters.AddWithValue("@MachineId", "");
                if ((plantId).Equals("All")) plantId = string.Empty;
                cmd.Parameters.AddWithValue("@PlantId", plantId);
                cmd.Parameters.AddWithValue("@SortOrder", "");
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


    }
}