using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class DownCodeInfoDataBase
    {
        internal static List<string> GetDownCategoryInformation()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select downCategory,[description] from [dbo].[DownCategoryInformation]";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["downCategory"].ToString());
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

        internal static string GetShopdefaultsTimeFormat()
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            string timeFormat = "ss";
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("select valueinText from Shopdefaults where Parameter='TimeInFormat'", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        timeFormat = sdr["valueinText"].ToString();
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

            return timeFormat;
        }

        internal static void InsertUpdateMachineInformation(string macId, string desc, string interfaceID, string protcolInString, string iP, string portNo, string bulkDataTransferPortNo, string mchrrate, int TPMTrakEnabled, int SmartTransEnabled, int EthernetEnabled, int Nto1Device, int DNS, string DNSIP, string DNSPortNo, int CriticalMachineEnabled, int MobileEnabled, string opcuaUrl, string NoofFixture, string NoofFxtureForPallet2, bool PalletEnabled, out bool param)
        {
            param = false;
            int protocolDAP = 0;
            if (protcolInString == "RAW")
                protocolDAP = 0;
            else if (protcolInString == "DAP")
                protocolDAP = 1;
            else if (protcolInString == "MODBUS")
                protocolDAP = 2;
            else if (protcolInString == "MODFINET")
                protocolDAP = 4;
            else if (protcolInString.Equals("Fanuc-MODBUS", StringComparison.OrdinalIgnoreCase))
                protocolDAP = 4;
            else if (protcolInString.Equals("Fanuc-Laser-MODBUS", StringComparison.OrdinalIgnoreCase))
                protocolDAP = 5;
            else
                protocolDAP = 3;
            DNSIP = DNS == 0 ? "" : DNSIP;
            DNSPortNo = DNS == 0 ? "" : DNSPortNo;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string query = string.Empty;
            try
            {
                {
                    query = @"if exists(select MachineId from machineinformation where MachineId = @MachineId) 
                            BEGIN 
                                update machineinformation set InterfaceID=@InterfaceID, description=@description, BulkDataTransferPortNo=@BulkDataTransferPortNo, IP=@IP, mchrrate=@mchrrate, IPPortNO=@PortNo, DAPEnabled=@DAPEnabled, TPMTrakEnabled=@TPMTrakEnabled, SmartTransEnabled=@SmartTransEnabled, EthernetEnabled=@EthernetEnabled, Nto1Device=@Nto1Device, DNCIP=@DNCIP, DNCIPPortNo=@DNCIPPortNo, DNCTransferEnabled=@DNCTransferEnabled, CriticalMachineEnabled=@CriticalMachineEnabled, MobileEnabled=@MobileEnabled, OPCUAURL=@OPCUAURL,PalletEnabled=@PalletEnabled, NoOfFixtures=@NoOfFixtures, NoOfFixturesForPallet2=@NoofFixturesForPallet2 , UpdatedTS=@UpdatedTS
                                    where machineid=@MachineId 
                            END 
                            ELSE 
                            BEGIN
                                insert into machineinformation(machineid, InterfaceID, description, BulkDataTransferPortNo, IP, mchrrate, IPPortNO, DAPEnabled, TPMTrakEnabled, SmartTransEnabled, EthernetEnabled, Nto1Device,DNCTransferEnabled,DNCIPPortNo,DNCIP,CriticalMachineEnabled,MobileEnabled,OPCUAURL, PalletEnabled, NoOfFixtures,NoOfFixturesForPallet2,UpdatedTS) values(@MachineId, @InterfaceID, @description, @BulkDataTransferPortNo, @IP, @mchrrate, @PortNo, @DAPEnabled, @TPMTrakEnabled, @SmartTransEnabled, @EthernetEnabled, @Nto1Device,@DNCTransferEnabled,@DNCIPPortNo,@DNCIP,@CriticalMachineEnabled,@MobileEnabled,@OPCUAURL, @PalletEnabled,@NoOfFixtures, @NoofFixturesForPallet2,@UpdatedTS) 
                            END";
                }

                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineId", macId);
                cmd.Parameters.AddWithValue("@description", desc);
                cmd.Parameters.AddWithValue("@InterfaceID", interfaceID);
                cmd.Parameters.AddWithValue("@DAPEnabled", protocolDAP);
                cmd.Parameters.AddWithValue("@IP", iP);
                cmd.Parameters.AddWithValue("@PortNo", portNo);
                cmd.Parameters.AddWithValue("@BulkDataTransferPortNo", bulkDataTransferPortNo);
                cmd.Parameters.AddWithValue("@mchrrate", string.IsNullOrEmpty(mchrrate) ? "" : mchrrate);
                cmd.Parameters.AddWithValue("@TPMTrakEnabled", TPMTrakEnabled);
                cmd.Parameters.AddWithValue("@SmartTransEnabled", SmartTransEnabled);
                cmd.Parameters.AddWithValue("@EthernetEnabled", EthernetEnabled);
                cmd.Parameters.AddWithValue("@Nto1Device", Nto1Device);
                cmd.Parameters.AddWithValue("@DNCTransferEnabled", DNS);
                cmd.Parameters.AddWithValue("@DNCIP", DNSIP);
                cmd.Parameters.AddWithValue("@DNCIPPortNo", DNSPortNo);
                cmd.Parameters.AddWithValue("@CriticalMachineEnabled", CriticalMachineEnabled);
                cmd.Parameters.AddWithValue("@MobileEnabled", MobileEnabled);
                cmd.Parameters.AddWithValue("@OPCUAURL", opcuaUrl);
                cmd.Parameters.AddWithValue("@NoOfFixtures", NoofFixture);
                cmd.Parameters.AddWithValue("@NoofFixturesForPallet2", NoofFxtureForPallet2);
                cmd.Parameters.AddWithValue("@PalletEnabled", PalletEnabled == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    param = true;
                }
            }
            catch (Exception ex)
            {
                param = false;
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }


        internal static void InsertUpdateMachineInformation_Shanthi(string macId, string desc, string interfaceID, string protcolInString, string iP, string portNo, string bulkDataTransferPortNo, string mchrrate, int TPMTrakEnabled, int SmartTransEnabled, int EthernetEnabled, int Nto1Device, int DNS, string DNSIP, string DNSPortNo, string OPNNo, string Process, string ReqFolder, string RespFolder, string ackfolder, string SPCfolder, out bool param)
        {
            param = false;
            int protocolDAP = 0;
            if (protcolInString == "RAW")
                protocolDAP = 0;
            else if (protcolInString == "DAP")
                protocolDAP = 1;
            else if (protcolInString == "MODBUS")
                protocolDAP = 2;
            else if (protcolInString == "CSV")
                protocolDAP = 4;
            else
                protocolDAP = 3;
            DNSIP = DNS == 0 ? "" : DNSIP;
            DNSPortNo = DNS == 0 ? "" : DNSPortNo;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                string query = @"if exists(select MachineId from machineinformation where MachineId = @MachineId) BEGIN 
                        update machineinformation set InterfaceID=@InterfaceID, description=@description, BulkDataTransferPortNo=@BulkDataTransferPortNo, IP=@IP, mchrrate=@mchrrate, IPPortNO=@PortNo, DAPEnabled=@DAPEnabled, TPMTrakEnabled=@TPMTrakEnabled, SmartTransEnabled=@SmartTransEnabled, EthernetEnabled=@EthernetEnabled, Nto1Device=@Nto1Device,DNCIP=@DNCIP,DNCIPPortNo=@DNCIPPortNo,DNCTransferEnabled=@DNCTransferEnabled,OpnID=@OPN,Process=@Process,RESFolderPath=@ResFolder,REQFolderPath=@ReqFolder,ACKFolderPath=@AckFolder,SPCFolderPath=@SpcFolder where machineid=@MachineId END ELSE BEGIN
                         insert into machineinformation(machineid, InterfaceID, description, BulkDataTransferPortNo, IP, mchrrate, IPPortNO, DAPEnabled, TPMTrakEnabled, SmartTransEnabled, EthernetEnabled, Nto1Device,DNCTransferEnabled,DNCIPPortNo,DNCIP,OpnID,Process,RESFolderPath,REQFolderPath,ACKFolderPath,SPCFolderPath) values(@MachineId, @InterfaceID, @description, @BulkDataTransferPortNo, @IP, @mchrrate, @PortNo, @DAPEnabled, @TPMTrakEnabled, @SmartTransEnabled, @EthernetEnabled, @Nto1Device,@DNCTransferEnabled,@DNCIPPortNo,@DNCIP,@OPN,@Process,@ResFolder,@ReqFolder,@AckFolder,@SpcFolder) END";

                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@MachineId", macId);
                cmd.Parameters.AddWithValue("@description", desc);
                cmd.Parameters.AddWithValue("@InterfaceID", interfaceID);
                cmd.Parameters.AddWithValue("@DAPEnabled", protocolDAP);
                cmd.Parameters.AddWithValue("@IP", iP);
                cmd.Parameters.AddWithValue("@PortNo", portNo);
                cmd.Parameters.AddWithValue("@BulkDataTransferPortNo", bulkDataTransferPortNo);
                cmd.Parameters.AddWithValue("@mchrrate", string.IsNullOrEmpty(mchrrate) ? "" : mchrrate);
                cmd.Parameters.AddWithValue("@TPMTrakEnabled", TPMTrakEnabled);
                cmd.Parameters.AddWithValue("@SmartTransEnabled", SmartTransEnabled);
                cmd.Parameters.AddWithValue("@EthernetEnabled", EthernetEnabled);
                cmd.Parameters.AddWithValue("@Nto1Device", Nto1Device);
                cmd.Parameters.AddWithValue("@DNCTransferEnabled", DNS);
                cmd.Parameters.AddWithValue("@DNCIP", DNSIP);
                cmd.Parameters.AddWithValue("@DNCIPPortNo", DNSPortNo);


                cmd.Parameters.AddWithValue("@OPN", OPNNo);
                cmd.Parameters.AddWithValue("@Process", Process);
                cmd.Parameters.AddWithValue("@ResFolder", RespFolder);
                cmd.Parameters.AddWithValue("@ReqFolder", ReqFolder);
                cmd.Parameters.AddWithValue("@AckFolder", ackfolder);
                cmd.Parameters.AddWithValue("@SpcFolder", SPCfolder);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    param = true;
                }
            }
            catch (Exception ex)
            {
                param = false;
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }



        internal static void InsertUpdateMachineInfo(string param, string MachineID, string description, int status, float mchrrate,
            int portno, string settings, string interfaceid, string ip, string ipportno, int mode, int autoload, int tpmtrakEnabled,
            int PEGreen, int PERed, int aegreen, int aered, int oegreen, int oered, string bulkdatatransferportno, bool multispindleflag,
            int DeviceType, bool PPtransferEnabled, bool SmartTransEnabled, string IgnoreCoFromMach, string AutoSetupchangeDown, string Machinewiseowner,
            bool criticalmachineenabled, int DAPenabled,
            decimal lowerpowerthreshold, decimal upperpowerthreshold, int qered, int qegreen, bool ethernetenabled, bool Nto1Device, string StartDate, string Enddate,
            string ControlName, string pStartID, string pEndID, string FileNameFrom, string receiveAtMachineFilePath, string SentFromMachineFilePath, string nodeInterface,
            string nodeid, int SortOrder, string manufacturer, string dateofmanufacture, string address, string Place, string Phone,
            string contactperson, bool MobileEnabled, out bool isSuccessOrFailure, int operatorPEgreen, int OperatorPEred)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            isSuccessOrFailure = false;
            try
            {
                SqlCommand cmd = new SqlCommand("s_InsertMachineMaster", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Param", param);
                cmd.Parameters.AddWithValue("@machineID", MachineID);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@mchrrate", mchrrate);
                cmd.Parameters.AddWithValue("@portno", portno);
                cmd.Parameters.AddWithValue("@settings", settings);
                cmd.Parameters.AddWithValue("@interfaceid", interfaceid);
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@ipportno", ipportno);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@autoload", autoload);
                cmd.Parameters.AddWithValue("@tpmtrakEnabled", tpmtrakEnabled);
                cmd.Parameters.AddWithValue("@PEGreen", PEGreen);
                cmd.Parameters.AddWithValue("@PERed", PERed);
                cmd.Parameters.AddWithValue("@aegreen", aegreen);
                cmd.Parameters.AddWithValue("@aered", aered);
                cmd.Parameters.AddWithValue("@oegreen", oegreen);
                cmd.Parameters.AddWithValue("@oered", oered);
                cmd.Parameters.AddWithValue("@OPRGreen", operatorPEgreen);
                cmd.Parameters.AddWithValue("@OPRRed", OperatorPEred);
                cmd.Parameters.AddWithValue("@bulkdatatransferportno", bulkdatatransferportno);
                cmd.Parameters.AddWithValue("@multispindleflag", multispindleflag);
                cmd.Parameters.AddWithValue("@DeviceType", DeviceType);
                cmd.Parameters.AddWithValue("@PPtransferEnabled", PPtransferEnabled);
                cmd.Parameters.AddWithValue("@SmartTransEnabled", SmartTransEnabled);
                cmd.Parameters.AddWithValue("@IgnoreCoFromMach", IgnoreCoFromMach);
                cmd.Parameters.AddWithValue("@AutoSetupchangeDown", AutoSetupchangeDown);
                cmd.Parameters.AddWithValue("@Machinewiseowner", Machinewiseowner);
                cmd.Parameters.AddWithValue("@criticalmachineenabled", criticalmachineenabled);
                cmd.Parameters.AddWithValue("@DAPenabled", DAPenabled);
                cmd.Parameters.AddWithValue("@lowerpowerthreshold", lowerpowerthreshold);
                cmd.Parameters.AddWithValue("@upperpowerthreshold", upperpowerthreshold);
                cmd.Parameters.AddWithValue("@qered", qered);
                cmd.Parameters.AddWithValue("@qegreen", qegreen);
                cmd.Parameters.AddWithValue("@ethernetenabled", ethernetenabled);
                cmd.Parameters.AddWithValue("@Nto1Device", Nto1Device);
                cmd.Parameters.AddWithValue("@ControlName", ControlName);

                if (!string.IsNullOrEmpty(StartDate.ToString())) cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(StartDate));
                if (!string.IsNullOrEmpty(Enddate.ToString())) cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(Enddate));
                cmd.Parameters.AddWithValue("@pStartID", pStartID);
                cmd.Parameters.AddWithValue("@pEndId", pEndID);
                cmd.Parameters.AddWithValue("@FileNameFrom", FileNameFrom);
                cmd.Parameters.AddWithValue("@receiveAtMachineFilePath", receiveAtMachineFilePath);
                cmd.Parameters.AddWithValue("@SentFromMachineFilePath", SentFromMachineFilePath);
                cmd.Parameters.AddWithValue("@nodeInterface", nodeInterface);
                cmd.Parameters.AddWithValue("@nodeid", nodeid);
                cmd.Parameters.AddWithValue("@SortOrder", SortOrder);
                cmd.Parameters.AddWithValue("@manufacturer", manufacturer);
                if (!string.IsNullOrEmpty(dateofmanufacture.ToString()))
                {
                    cmd.Parameters.AddWithValue("@dateofmanufacture", Convert.ToDateTime(dateofmanufacture.ToString()));
                }
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@contactperson", contactperson);
                cmd.Parameters.AddWithValue("@MobileEnabled", MobileEnabled);
                int x = cmd.ExecuteNonQuery();
                isSuccessOrFailure = true;
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

        internal static List<DownCodesModel> GetAllDownCodeInfo(string downid, string interfaceid, string catagory, string param, string timeFormat)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();

            List<DownCodesModel> list = new List<DownCodesModel>();
            try
            {
                SqlDataReader sdr = null;
                SqlCommand cmd = new SqlCommand("ss_GetDownCodeInformation", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@downid", downid);
                cmd.Parameters.AddWithValue("@interfaceid", interfaceid);
                cmd.Parameters.AddWithValue("@category", catagory);
                cmd.Parameters.AddWithValue("@Param", param);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        //   downid,interfaceid,downdescription,Catagory,availeffy,retpermchour,threshold,prodeffy,thresholdfromCO,
                        DownCodesModel DownVals = new DownCodesModel();
                        if (!Convert.IsDBNull(sdr["downid"]))
                        {
                            DownVals.downid = Convert.ToString(sdr["downid"]);
                        }

                        if (!Convert.IsDBNull(sdr["Catagory"]))
                        {
                            DownVals.Catagory = Convert.ToString(sdr["Catagory"]);
                        }
                        if (!Convert.IsDBNull(sdr["downdescription"]))
                        {
                            DownVals.downdescription = Convert.ToString(sdr["downdescription"]);
                        }
                        if (!Convert.IsDBNull(sdr["interfaceid"]))
                        {
                            DownVals.interfaceid = Convert.ToString(sdr["interfaceid"]);

                            // To show Planned Downtime McTI In Web --Inlcuded by Bindu 20-11-2023
                            if (sdr["interfaceid"].ToString().Equals("McTI", StringComparison.OrdinalIgnoreCase))
                            {
                                DownVals.Readonly = "";
                                DownVals.display = "none";
                            }
                            else
                            {
                                DownVals.display = "block";
                                if (Regex.IsMatch(DownVals.interfaceid, @"^[0-9]+$"))
                                {
                                    DownVals.Readonly = "";
                                }
                                else
                                {
                                    DownVals.Readonly = "readonly";
                                }
                            }
                        }
                        if (!Convert.IsDBNull(sdr["availeffy"]))
                        {
                            DownVals.Availeffy = Convert.ToBoolean(sdr["availeffy"]);
                            if (DownVals.Availeffy)
                            {
                                DownVals.chkAvaileffy = "checked";
                                DownVals.dispalyValue = "block";
                            }
                            else
                                DownVals.dispalyValue = "none";
                        }
                        else
                        {
                            DownVals.Availeffy = false;
                        }
                        if (!Convert.IsDBNull(sdr["retpermchour"]))
                        {
                            DownVals.retpermchour = Convert.ToBoolean(sdr["retpermchour"]);
                        }

                        if (!Convert.IsDBNull(sdr["Threshold"]))
                        {
                            if (timeFormat.Equals("ss", StringComparison.OrdinalIgnoreCase))
                            {
                                DownVals.Threshold = Convert.ToDouble(sdr["Threshold"]);
                            }
                            else
                            {
                                DownVals.Threshold = Convert.ToDouble(sdr["Threshold"]) / 60.00;
                            }
                            // DownVals.Threshold = Convert.ToDouble(sdr["Threshold"]) / 60.00;
                        }
                        if (!Convert.IsDBNull(sdr["prodeffy"]))
                        {
                            DownVals.prodeffy = Convert.ToBoolean(sdr["prodeffy"]);
                            if (DownVals.prodeffy)
                            {
                                DownVals.chkOperatorEffy = "checked";
                                DownVals.OperatordisplayValue = "block";
                            }
                            else
                                DownVals.OperatordisplayValue = "none";
                        }
                        if (!Convert.IsDBNull(sdr["thresholdfromCO"]))
                        {
                            DownVals.ThresholdfrmCO = Convert.ToBoolean(sdr["thresholdfromCO"]);
                            if (DownVals.ThresholdfrmCO)
                                DownVals.chkThresholdfrmCO = "checked";
                        }
                        //if (!Convert.IsDBNull(sdr["interfaceid"]))
                        //{
                        //    EmpVals.interfaceid = Convert.ToInt32(sdr["interfaceid"]);
                        //}
                        //if (!Convert.IsDBNull(sdr["company_default"]))
                        //{
                        //    EmpVals.company_default = Convert.ToBoolean(sdr["company_default"]);
                        //}
                        //if (!Convert.IsDBNull(sdr["Email"]))
                        //{
                        //    EmpVals.Email = Convert.ToString(sdr["Email"]);
                        //}
                        if (!Convert.IsDBNull(sdr["Owner"]))
                        {
                            DownVals.Owner = Convert.ToString(sdr["Owner"]);
                        }
                        //if (!Convert.IsDBNull(sdr["prodeffyThreshold"]))
                        //{
                        //    if (timeFormat.Equals("ss", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        DownVals.OperatorThreshold = Convert.ToDouble(sdr["prodeffyThreshold"]);
                        //    }
                        //    else
                        //    {
                        //        DownVals.OperatorThreshold = Convert.ToDouble(sdr["prodeffyThreshold"]) / 60.00;
                        //    }
                        //}
                        if (!Convert.IsDBNull(sdr["ExcludeFromTarget"]))
                        {
                            DownVals.IgnoreForRuntimeTarget =Convert.ToBoolean(sdr["ExcludeFromTarget"].ToString());
                            if (DownVals.IgnoreForRuntimeTarget)
                            {
                                DownVals.chkIgnoreForRuntimeTarget = "checked";
                            }
                        }
                        if (DownVals.Readonly != "readonly")
                            list.Add(DownVals);
                    }
                }
                else
                {
                    list = null;
                }
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

        internal static string GetAllDownid(string downcode)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string downCode = string.Empty;
            try
            {
                string sqlQuery = @"select * from downcodeinformation where downid=@downcode";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@downcode", downcode);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    downCode = (Convert.ToString(rdr["interfaceid"]));
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
            return downCode;
        }

        #region "Machine Efficiency Color Code Info"
        internal static MachineEfficiencyColorCode GetMachineColorCodes(string machineid, string param)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            MachineEfficiencyColorCode machineColorCodes = null;
            try
            {
                if (machineid.Equals("All"))
                {
                    machineid = string.Empty;
                }
                cmd = new SqlCommand("[s_ViewMachineMaster]", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();

                if (machineid != "")
                {
                    if (sdr.HasRows)
                    {
                        machineColorCodes = new MachineEfficiencyColorCode();
                        while (sdr.Read())
                        {
                            machineColorCodes.AENotOk = (sdr["AERed"]).ToString();
                            machineColorCodes.AEOk = (sdr["AEGreen"]).ToString();
                            machineColorCodes.PENotOk = (sdr["PERed"]).ToString();
                            machineColorCodes.PEOk = (sdr["PEGreen"]).ToString();
                            machineColorCodes.QENotOk = (sdr["QERed"]).ToString();
                            machineColorCodes.QEOk = (sdr["QEGreen"]).ToString();
                            machineColorCodes.OEENotOk = (sdr["OERed"]).ToString();
                            machineColorCodes.OEEOk = (sdr["OEGreen"]).ToString();
                            machineColorCodes.OperatorPEGreen = (sdr["OPRGreen"]).ToString();
                            machineColorCodes.OperatorPERed = (sdr["OPRRed"]).ToString();
                        }
                    }
                    sdr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return machineColorCodes;
        }
        #endregion

        #region "Machine Control Info"
        internal static MachineControlInfo GetMachineControlInfo(string machineid, string param)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            MachineControlInfo machineControldetails = null;
            try
            {
                if (machineid.Equals("All"))
                {
                    machineid = string.Empty;
                }
                cmd = new SqlCommand("[s_ViewMachineMaster]", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();

                if (machineid != "")
                {
                    if (sdr.HasRows)
                    {
                        machineControldetails = new MachineControlInfo();
                        while (sdr.Read())
                        {
                            machineControldetails.ControlType = (sdr["ControlName"]).ToString();
                            machineControldetails.ProgStartId = sdr["pStartId"].ToString();
                            machineControldetails.ProgramEndId = sdr["pEndId"].ToString();
                            machineControldetails.RecieveAtMachine = sdr["ReceiveAtMachineFilePath"].ToString();
                            machineControldetails.RecieveFromMachine = sdr["SentFromMachineFilePath"].ToString();
                        }
                    }
                    sdr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return machineControldetails;
        }
        #endregion

        #region "Machine Make Data"
        internal static MachineMakeData GetMachineMakeInfo(string machineid, string param)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            MachineMakeData machinemakedetails = null;
            try
            {
                if (machineid.Equals("All"))
                {
                    machineid = string.Empty;
                }
                cmd = new SqlCommand("[s_ViewMachineMaster]", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();

                if (machineid != "")
                {
                    if (sdr.HasRows)
                    {
                        machinemakedetails = new MachineMakeData();
                        while (sdr.Read())
                        {
                            machinemakedetails.Manufacturer = (sdr["manufacturer"]).ToString();
                            machinemakedetails.Phone = sdr["phone"].ToString();
                            machinemakedetails.Address = sdr["address"].ToString();
                            machinemakedetails.DateOfManufacturer = sdr["dateofmanufacture"].ToString();
                            machinemakedetails.Place = sdr["place"].ToString();
                            machinemakedetails.ContactPerson = sdr["ContactPerson"].ToString();
                        }
                    }
                    sdr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return machinemakedetails;
        }

        #endregion

        #region "Machine Efficiency Target Data"
        internal static MachineEfficiencyTargetData GetMachineEfficiencyTarget(string machineid, string param)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            MachineEfficiencyTargetData machineefficiencyTarget = null;
            try
            {
                if (machineid.Equals("All"))
                {
                    machineid = string.Empty;
                }
                cmd = new SqlCommand("[s_ViewMachineMaster]", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", machineid);
                cmd.Parameters.AddWithValue("@param", param);
                sdr = cmd.ExecuteReader();

                if (machineid != "")
                {
                    if (sdr.HasRows)
                    {
                        machineefficiencyTarget = new MachineEfficiencyTargetData();
                        while (sdr.Read())
                        {

                            machineefficiencyTarget.AE = (sdr["AE"]).ToString();
                            machineefficiencyTarget.PE = (sdr["PE"]).ToString();
                            machineefficiencyTarget.OEE = (sdr["OEE"]).ToString();
                            machineefficiencyTarget.QE = (sdr["QE"]).ToString();
                            machineefficiencyTarget.TargetLevel = (sdr["TargetLevel"]).ToString();
                            machineefficiencyTarget.EffiTargetOwner = (sdr["EffiTargetOwner"]).ToString();
                            machineefficiencyTarget.CriticalMachineEnabled = Convert.ToBoolean(sdr["CriticalMachineEnabled"]);
                        }
                    }
                    sdr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n" + ex.ToString());
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
            return machineefficiencyTarget;
        }

        #endregion

        #region "Bind Down Catagory Information"
        internal static List<Tuple<string, string>> GetDownCategoryInformation(string downcategory, out string description)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            string sqlQuery = string.Empty;
            description = string.Empty;
            try
            {
                sqlQuery = "select downCategory,[description] from [dbo].[DownCategoryInformation] where downcategory= @downCategory or @downCategory =''";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.Parameters.AddWithValue(@"downCategory", downcategory);
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new Tuple<string, string>(rdr["downCategory"].ToString(), rdr["description"].ToString()));
                    description = rdr["description"].ToString();
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
        #endregion

        #region "Add Down Catagory Info"
        internal static int InsertDeleteDownCategoryInformation(string DownCategory, string Description, string action)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int val = 0;
            try
            {
                string sqlQuery = string.Empty;
                if (action == "save")
                {
                    sqlQuery = @"if not exists(select * from downcategoryInformation where DownCategory = @DownCategory)
                BEGIN
                insert into downcategoryInformation(DownCategory,[Description],UpdatedTS) values (@DownCategory , @Description,@UpdatedTS)
                END
                else
                BEGIN
                update downcategoryInformation set   Description= @Description,UpdatedTS=@UpdatedTS  where DownCategory = @DownCategory 
                END ";
                }
                else if (action == "delete")
                {
                    sqlQuery = @"delete from downCategoryInformation where DownCategory=@DownCategory";
                }


                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@DownCategory", DownCategory);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                val = cmd.ExecuteNonQuery();
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
            return val;
        }
        #endregion

        internal static bool CheckDownCategoryExist(string DownCategory)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool isSuccessful = false;
            try
            {
                string sqlQuery = "select * from downCodeInformation where catagory= @downcategory";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@DownCategory", DownCategory);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    isSuccessful = true;
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
            return isSuccessful;
        }

        #region "Save Down Code Information"
        internal static void InsertUpdateDownCodeInformation(string downid, string interfaceid, string downDescription, string catagory, bool availeffy,
            bool retpermchour, double Threshold, bool Prodeffy, bool ThresholdFromCO, string Owner, string param, out string msg,bool ExcludeFromTarget)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                if (!string.IsNullOrEmpty(GetAllDownid(downid)))
                {
                    param = "Update";
                }

                SqlCommand cmd = new SqlCommand(@"ss_InsertUpdateDownCodeInformation", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"downid", downid);
                cmd.Parameters.AddWithValue(@"interfaceid", interfaceid);
                cmd.Parameters.AddWithValue(@"downDescription", downDescription);
                cmd.Parameters.AddWithValue(@"catagory", catagory);
                cmd.Parameters.AddWithValue(@"availeffy", availeffy);
                cmd.Parameters.AddWithValue(@"retpermchour", retpermchour);
                cmd.Parameters.AddWithValue(@"Threshold", Threshold);
                cmd.Parameters.AddWithValue(@"Prodeffy", Prodeffy);
                cmd.Parameters.AddWithValue(@"ThresholdFromCO", ThresholdFromCO);
                cmd.Parameters.AddWithValue("@Owner", Owner);
                cmd.Parameters.AddWithValue("@ExcludeFromTarget", ExcludeFromTarget);
                cmd.Parameters.AddWithValue(@"param", param);
                //cmd.Parameters.AddWithValue("@prodeffyThreshold", ProdThreshold);
                int x = cmd.ExecuteNonQuery();
                msg = "Records save successfully";

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        #region Delete Down Code Information
        internal static string deleteDownCodeDetails(string downcode, string interfaceID)
        {
            string result = "";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("ss_InsertUpdateDownCodeInformation", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(@"Param", "Delete");
                cmd.Parameters.AddWithValue(@"interfaceid", interfaceID);
                cmd.Parameters.AddWithValue(@"downid", downcode);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        if (string.Equals(sdr["result"].ToString(), "Deleted", StringComparison.OrdinalIgnoreCase))
                        {
                            result = "success";
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
                if (sqlConn != null) sqlConn.Close();
                if (sdr != null) sdr.Close();
            }
            return result;
        }
        #endregion
        #region--------------------------------------------------------------------Down ID SubLoss Master-----------------------------------------
        internal static List<DownIDSubLossInfoData> GetSubLossMasterData(string DownCategory, string DownID)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<DownIDSubLossInfoData> list = new List<DownIDSubLossInfoData>();
            DownIDSubLossInfoData data = null;
            try
            {
                cmd = new SqlCommand(@"select * from SubLossMasterDetails where DownCategory=@DownCategory and (DownID=@DownID or ISNULL( @DownID,'')='')order by SubLossInterfaceID asc", con);
                cmd.Parameters.AddWithValue("@DownCategory", DownCategory);
                cmd.Parameters.AddWithValue("@DownID", DownID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        data = new DownIDSubLossInfoData();
                        data.DownID = sdr["DownID"].ToString();
                        data.SubLossID = sdr["SubLossID"].ToString();
                        data.SubLossDescription = sdr["SubLossDescription"].ToString();
                        data.SubLossInterfaceID = sdr["SubLossInterfaceID"].ToString();
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
                if (sdr != null) sdr.Close();
                if (con != null) con.Close();
            }
            return list;
        }
        internal static string SaveSubLossMasterData(string DownCategory, string SubLossID, string DownID, string SubLossDescription, string SublossInterfaceID,string Param)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                //                cmd = new SqlCommand(@"if not exists(select * from SubLossMasterDetails where DownCategory=@DownCategory and DownID=@DownID and SubLossID=@SubLossID and SubLossInterfaceID=@SubLossInterfaceID)
                //begin
                //insert into SubLossMasterDetails (DownCategory,DownID,SubLossID,SubLossDescription ,UpdatedTS,SubLossInterfaceID)
                //values(@DownCategory,@DownID,@SubLossID,@SubLossDescription ,@UpdatedTS,@SubLossInterfaceID)
                //select 'INSERTED' as SAVEFLAG
                //end
                //else
                //begin
                //Update SubLossMasterDetails set SubLossDescription=@SubLossDescription,UpdatedTS=@UpdatedTS where DownCategory=@DownCategory and DownID=@DownID and SubLossID=@SubLossID and SubLossInterfaceID=@SubLossInterfaceID
                //select 'UPDATED' as SAVEFLAG
                //end", con);
                if (Param == "Update")
                {
                    cmd = new SqlCommand(@"if not exists(select * from SubLossMasterDetails where DownCategory=@DownCategory and DownID=@DownID and SubLossInterfaceID=@SubLossInterfaceID)
begin
insert into SubLossMasterDetails (DownCategory,DownID,SubLossDescription ,UpdatedTS,SubLossInterfaceID)
values(@DownCategory,@DownID,@SubLossDescription ,@UpdatedTS,@SubLossInterfaceID)
select 'INSERTED' as SAVEFLAG
end
else
begin
Update SubLossMasterDetails set SubLossDescription=@SubLossDescription,UpdatedTS=@UpdatedTS where DownCategory=@DownCategory and DownID=@DownID and SubLossInterfaceID=@SubLossInterfaceID
select 'UPDATED' as SAVEFLAG
end", con);
                }
                else if (Param == "Insert")
                {
                    cmd = new SqlCommand(@"if not exists(select * from SubLossMasterDetails where DownCategory=@DownCategory and DownID=@DownID and SubLossInterfaceID=@SubLossInterfaceID)
begin
insert into SubLossMasterDetails (DownCategory,DownID,SubLossDescription ,UpdatedTS,SubLossInterfaceID)
values(@DownCategory,@DownID,@SubLossDescription ,@UpdatedTS,@SubLossInterfaceID)
select 'INSERTED' as SAVEFLAG
end
else
begin
select 'Interface ID already exist' as SAVEFLAG
end", con);
                }
                cmd.Parameters.AddWithValue("@DownCategory", DownCategory);
                //cmd.Parameters.AddWithValue("@SubLossID", SubLossID);
                cmd.Parameters.AddWithValue("@DownID", DownID);
                cmd.Parameters.AddWithValue("@SubLossInterfaceID", SublossInterfaceID);
                cmd.Parameters.AddWithValue("@SubLossDescription", SubLossDescription);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SAVEFLAG"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return success;
        }
        internal static string DeleteSubLossMasterData(string DownCategory, string DownID, string SublossInterfaceID)
        {
            string success = "";
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            try
            {
                //                cmd = new SqlCommand(@"delete from SubLossMasterDetails where DownCategory=@DownCategory and DownID=@DownID and SubLossID=@SubLossID and SubLossInterfaceID=@SubLossInterfaceID
                //select 'Deleted' as SAVEFLAG", con);
                cmd = new SqlCommand(@"delete from SubLossMasterDetails where DownCategory=@DownCategory and DownID=@DownID and SubLossInterfaceID=@SubLossInterfaceID
select 'Deleted' as SAVEFLAG", con);
                cmd.Parameters.AddWithValue("@DownCategory", DownCategory);
                //cmd.Parameters.AddWithValue("@SubLossID", SubLossID);
                cmd.Parameters.AddWithValue("@DownID", DownID);
                cmd.Parameters.AddWithValue("@SubLossInterfaceID", SublossInterfaceID);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        success = sdr["SAVEFLAG"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return success;
        }
        internal static List<string> GetDownCategories()
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct DownCategory from DownCategoryInformation", con);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["DownCategory"].ToString());
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
                if (con != null) con.Close();
            }
            return list;
        }
        internal static List<string> GetDownIDs(string Category)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataReader sdr = null;
            SqlCommand cmd = null;
            List<string> list = new List<string>();
            try
            {
                cmd = new SqlCommand(@"select distinct downid from downcodeinformation where Catagory=@Catagory", con);
                cmd.Parameters.AddWithValue("@Catagory", Category);
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        list.Add(sdr["downid"].ToString());
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
                if (con != null) con.Close();
            }
            return list;
        }
        #endregion

        #region "Bind Machine Information"
        internal static List<MachineInfoModel> GetAllMachineDetails(string MachineName)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            DataTable dt = new DataTable();
            List<MachineInfoModel> machineList = new List<MachineInfoModel>();

            try
            {
                if (MachineName.Equals("All"))
                {
                    MachineName = string.Empty;
                }
                cmd = new SqlCommand("[s_ViewMachineMaster]", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", MachineName);
                sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    MachineInfoModel machine = new MachineInfoModel();
                    machine.MachineID = sdr["machineid"].ToString();
                    machine.InterfaceID = sdr["InterfaceID"].ToString();
                    machine.IP = sdr["IP"].ToString();
                    machine.BulkDataTransferPortNo = sdr["BulkDataTransferPortNo"].ToString();
                    machine.Description = sdr["description"].ToString();
                    machine.mchrrate = sdr["mchrrate"].ToString();
                    machine.PortNo = sdr["IPPortNO"].ToString();

                    if (sdr["DAPEnabled"].ToString().Equals("0"))
                    {
                        machine.ProtcolInString = "RAW";
                    }
                    else if (sdr["DAPEnabled"].ToString().Equals("1"))
                    {
                        machine.ProtcolInString = "DAP";
                    }
                    else if (sdr["DAPEnabled"].ToString().Equals("2"))
                    {
                        machine.ProtcolInString = "MODBUS";
                    }
                    else if (sdr["DAPEnabled"].ToString().Equals("4"))
                    {
                        machine.ProtcolInString = "MODFINET";
                    }
                    else
                    {
                        machine.ProtcolInString = "PROFINET";
                    }
                    if (ConfigurationManager.AppSettings["AmararagaMangalPages"].ToString() == "1")
                    {
                        if (sdr["DAPEnabled"].ToString().Equals("4"))
                        {
                            machine.ProtcolInString = "Fanuc-MODBUS";
                        }
                        else if (sdr["DAPEnabled"].ToString().Equals("5"))
                        {
                            machine.ProtcolInString = "Fanuc-Laser-MODBUS";
                        }
                    }

                    machine.TpmFlags = new List<string>();
                    if (!Convert.IsDBNull(sdr["TPMTrakEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["TPMTrakEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("TPMTrakEnabled");
                            machine.TPMTRAKEnable = "checked";
                        }
                        else
                        {
                            machine.TPMTRAKEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["SmartTransEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["SmartTransEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("SmartTransEnabled");
                            machine.SmartTransEnable = "checked";
                        }
                        else
                        {
                            machine.SmartTransEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["EthernetEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["EthernetEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("EthernetEnabled");
                        }
                    }
                    if (!Convert.IsDBNull(sdr["Nto1Device"]))
                    {
                        if (Convert.ToBoolean(sdr["Nto1Device"]) == true)
                        {
                            machine.TpmFlags.Add("Nto1Device");
                            machine.SharedDevice = "checked";
                        }
                        else
                        {
                            machine.SharedDevice = "unchecked";
                        }

                    }
                    if (!Convert.IsDBNull(sdr["DNCTransferEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["DNCTransferEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("DNCTransferEnabled");
                            machine.DNCEnable = "checked";
                        }
                        else
                        {
                            machine.DNCEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["DNCIP"]))
                    {
                        machine.DNSIP = sdr["DNCIP"].ToString();
                    }
                    if (!Convert.IsDBNull(sdr["DNCIPPortNo"]))
                    {
                        machine.DNSIPPortNo = sdr["DNCIPPortNo"].ToString();
                    }

                    // machine.ProgramEnabledTextTemplate = string.Join(",", machine.TpmFlags.Select(item => "\"" + item + "\""));
                    //}
                    machine.ProgramEnabledTextTemplate = String.Join(",", machine.TpmFlags.ToList());
                    if (!Convert.IsDBNull(sdr["CriticalMachineEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["CriticalMachineEnabled"]) == true)
                        {
                            //machine.TpmFlags.Add("SmartTransEnabled");
                            machine.CriticalMachineEnable = "checked";
                        }
                        else
                        {
                            machine.CriticalMachineEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["MobileEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["MobileEnabled"]) == true)
                        {
                            machine.MobileEnable = "checked";
                        }
                        else
                        {
                            machine.MobileEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["OPCUAURL"]))
                    {
                        string opcuaUrl = sdr["OPCUAURL"].ToString();
                        if (!string.IsNullOrEmpty(opcuaUrl))
                        {
                            string[] opcuaUrlSplit = opcuaUrl.Split(':');
                            if (opcuaUrlSplit.Length == 1)
                            {
                                machine.OPCUAIPAddress = opcuaUrlSplit[0];
                            }
                            else
                            {
                                machine.OPCUAIPAddress = opcuaUrlSplit[0];
                                machine.OPCUAPort = opcuaUrlSplit[1];
                            }
                        }
                    }
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString() == "1")
                    {
                        if (!Convert.IsDBNull(sdr["PalletEnabled"]))
                        {
                            machine.PalletEnabled = Convert.ToBoolean(sdr["PalletEnabled"].ToString().ToLower()) == true ? "checked" : "unchecked";
                        }
                        if (!Convert.IsDBNull(sdr["NoOfFixtures"]))
                        {
                            machine.NoofFixture = sdr["NoOfFixtures"].ToString();
                        }
                        if (!Convert.IsDBNull(sdr["NoOfFixturesForPallet2"]))
                        {
                            machine.NoofFixtureForPallet2 = sdr["NoOfFixturesForPallet2"].ToString();
                        }
                    }
                    machineList.Add(machine);
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

            return machineList;
        }

        internal static List<MachineInfoModel_Shanthi> GetAllMachineDetails_Shanthi(string MachineName)
        {
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            DataTable dt = new DataTable();
            List<MachineInfoModel_Shanthi> machineList = new List<MachineInfoModel_Shanthi>();

            try
            {
                if (MachineName.Equals("All"))
                {
                    MachineName = string.Empty;
                }
                cmd = new SqlCommand("[s_ViewMachineMaster]", conn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@machineid", MachineName);
                sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    MachineInfoModel_Shanthi machine = new MachineInfoModel_Shanthi();
                    machine.MachineID = sdr["machineid"].ToString().Equals("null") ? "" : sdr["machineid"].ToString();
                    machine.InterfaceID = sdr["InterfaceID"].ToString().Equals("null") ? "" : sdr["InterfaceID"].ToString();
                    machine.IP = sdr["IP"].ToString().Equals("null") ? "" : sdr["IP"].ToString();
                    machine.OPNNo = sdr["OpnID"].ToString().Equals("null") ? "" : sdr["OpnID"].ToString();
                    machine.Process = sdr["Process"].ToString().Equals("null") ? "" : sdr["Process"].ToString();
                    machine.RequestFolder = sdr["REQFolderPath"].ToString().Equals("null") ? "" : sdr["REQFolderPath"].ToString();
                    machine.Responcefolder = sdr["RESFolderPath"].ToString().Equals("null") ? "" : sdr["RESFolderPath"].ToString();
                    machine.ACKfolder = sdr["ACKFolderPath"].ToString().Equals("null") ? "" : sdr["ACKFolderPath"].ToString();
                    machine.SPCfolder = sdr["SPCFolderPath"].ToString().Equals("null") ? "" : sdr["SPCFolderPath"].ToString();
                    machine.BulkDataTransferPortNo = sdr["BulkDataTransferPortNo"].ToString().Equals("null") ? "" : sdr["BulkDataTransferPortNo"].ToString();
                    machine.Description = sdr["description"].ToString().Equals("null") ? "" : sdr["description"].ToString();
                    machine.mchrrate = sdr["mchrrate"].ToString().Equals("null") ? "" : sdr["mchrrate"].ToString();
                    machine.PortNo = sdr["IPPortNO"].ToString().Equals("null") ? "" : sdr["IPPortNO"].ToString();

                    if (sdr["DAPEnabled"].ToString().Equals("0"))
                    {
                        machine.ProtcolInString = "RAW";
                    }
                    else if (sdr["DAPEnabled"].ToString().Equals("1"))
                    {
                        machine.ProtcolInString = "DAP";
                    }
                    else if (sdr["DAPEnabled"].ToString().Equals("2"))
                    {
                        machine.ProtcolInString = "MODBUS";
                    }
                    else if (sdr["DAPEnabled"].ToString().Equals("4"))
                    {
                        machine.ProtcolInString = "CSV";
                    }
                    else
                    {
                        machine.ProtcolInString = "PROFINET";
                    }

                    machine.TpmFlags = new List<string>();
                    if (!Convert.IsDBNull(sdr["TPMTrakEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["TPMTrakEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("TPMTrakEnabled");
                            machine.TPMTRAKEnable = "checked";
                        }
                        else
                        {
                            machine.TPMTRAKEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["SmartTransEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["SmartTransEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("SmartTransEnabled");
                            machine.SmartTransEnable = "checked";
                        }
                        else
                        {
                            machine.SmartTransEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["EthernetEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["EthernetEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("EthernetEnabled");
                        }
                    }
                    if (!Convert.IsDBNull(sdr["Nto1Device"]))
                    {
                        if (Convert.ToBoolean(sdr["Nto1Device"]) == true)
                        {
                            machine.TpmFlags.Add("Nto1Device");
                            machine.SharedDevice = "checked";
                        }
                        else
                        {
                            machine.SharedDevice = "unchecked";
                        }

                    }
                    if (!Convert.IsDBNull(sdr["DNCTransferEnabled"]))
                    {
                        if (Convert.ToBoolean(sdr["DNCTransferEnabled"]) == true)
                        {
                            machine.TpmFlags.Add("DNCTransferEnabled");
                            machine.DNCEnable = "checked";
                        }
                        else
                        {
                            machine.DNCEnable = "unchecked";
                        }
                    }
                    if (!Convert.IsDBNull(sdr["DNCIP"]))
                    {
                        machine.DNSIP = sdr["DNCIP"].ToString().Equals("null") ? "" : sdr["DNCIP"].ToString();
                    }
                    else
                    {
                        machine.DNSIP = "";
                    }
                    if (!Convert.IsDBNull(sdr["DNCIPPortNo"]))
                    {
                        machine.DNSIPPortNo = sdr["DNCIPPortNo"].ToString().Equals("null") ? "" : sdr["DNCIPPortNo"].ToString();
                    }
                    else
                    {
                        machine.DNSIPPortNo = "";
                    }
                    // machine.ProgramEnabledTextTemplate = string.Join(",", machine.TpmFlags.Select(item => "\"" + item + "\""));
                    //}
                    machine.ProgramEnabledTextTemplate = String.Join(",", machine.TpmFlags.ToList());
                    machineList.Add(machine);
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

            return machineList;
        }
        #endregion

        #region "Bind Control Type"
        internal static List<string> GetControlNames()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select controlName from ControlInformation";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["controlName"].ToString());
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
        #endregion

        #region "Machine Manufacturer Info"
        internal static List<string> BindManufacturerInfo()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<string> list = new List<string>();
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select distinct manufacturer from machinemakeinformation where manufacturer!=''";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr["manufacturer"].ToString());
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
        #endregion

        internal static List<ShiftDataModel> GetAllshiftDetails()
        {
            List<ShiftDataModel> list = new List<ShiftDataModel>();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand(@"select * from shiftdetails where running=1 order by shiftid", sqlConn);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ShiftDataModel shftVals = new ShiftDataModel();
                        if (!Convert.IsDBNull(rdr["ShiftId"]))
                        {
                            shftVals.shiftId = Convert.ToString(rdr["ShiftId"]);
                        }

                        if (!Convert.IsDBNull(rdr["ShiftName"]))
                        {
                            shftVals.ShiftName = Convert.ToString(rdr["ShiftName"]);
                        }

                        if (!Convert.IsDBNull(rdr["FromDay"]))
                        {
                            if (rdr["FromDay"].ToString() == "0")
                                shftVals.FromDay = "Today";
                            else if (rdr["FromDay"].ToString() == "1")
                                shftVals.FromDay = "Tomorrow";
                            else
                                shftVals.FromDay = "Yesterday";
                        }
                        if (!Convert.IsDBNull(rdr["FromTime"]))
                        {
                            DateTime dt = Convert.ToDateTime(rdr["FromTime"]);
                            shftVals.FromTime = dt.ToString("hh:mm:ss tt");
                        }

                        if (!Convert.IsDBNull(rdr["ToDay"]))
                        {
                            if (rdr["ToDay"].ToString() == "0")
                                shftVals.ToDay = "Today";
                            else if (rdr["ToDay"].ToString() == "1")
                                shftVals.ToDay = "Tomorrow";
                            else
                                shftVals.ToDay = "Yesterday";
                        }

                        if (!Convert.IsDBNull(rdr["ToTime"]))
                        {
                            DateTime dt = Convert.ToDateTime(rdr["ToTime"]);
                            shftVals.ToTime = dt.ToString("hh:mm:ss tt");
                        }

                        list.Add(shftVals);
                    }

                }
                else
                {
                    list = null;
                }
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

        internal static bool CheckShiftId(string shiftId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            bool allreadyPresent = false;
            object obj = null;
            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select shiftId from shiftDetails where shiftId = @shiftId and Running = 1";

                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@shiftId", shiftId);
                obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    allreadyPresent = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return allreadyPresent;
        }

        internal static bool CheckForTheTimeEntry(string fromTime, string toTime)
        {
            bool isPresent = false;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(@"s_GetCurrentShiftTime", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", fromTime);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        isPresent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return isPresent;
        }

        internal static void UpdateShiftDetails(string shiftId, string shiftName, string fromDay, string toDay, DateTime fromTime, DateTime toTime)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int rowsAffected = 0, fDay = 0, tDay = 0;

            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            sqlQuery = "update ShiftDetails set shiftName = @shiftName ,fromDay =@fromDay, toDay= @toDay, fromTime= @fromTime,Totime= @Totime,UpdatedTS=@UpdatedTS where shiftId= @shiftId ";

            try
            {

                cmd = new SqlCommand(sqlQuery, conn);

                cmd.Parameters.AddWithValue("@shiftId", shiftId);
                cmd.Parameters.AddWithValue("@shiftName", shiftName);

                if (fromDay.Equals("Tomorrow")) fDay = 1;
                else if (fromDay.Equals("Yesterday")) fDay = 2;

                cmd.Parameters.AddWithValue("@fromDay", fDay);

                if (toDay.Equals("Tomorrow")) tDay = 1;
                else if (toDay.Equals("Yesterday")) tDay = 2;

                cmd.Parameters.AddWithValue("@toDay", tDay);

                cmd.Parameters.AddWithValue("@fromTime", fromTime);
                cmd.Parameters.AddWithValue("@toTime", toTime);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static bool CheckForShiftName(string shiftName, string shiftId)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlConnection DBcon = sqlConn;
            SqlCommand cmd = new SqlCommand();
            object obj = null;// reader;
            string valz = string.Empty;
            bool isPresent = false;
            try
            {

                cmd = new SqlCommand("Select shiftId from ShiftDetails where  shiftName = @shiftName and shiftId != @shiftId ", DBcon);
                cmd.Parameters.AddWithValue("@shiftId", shiftId);
                cmd.Parameters.AddWithValue("@shiftName", shiftName);

                obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    isPresent = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                DBcon.Close();
            }
            return isPresent;
        }

        internal static void InsertShiftDetails(string shiftId, string shiftName, string fromDay, string toDay, DateTime fromTime, DateTime toTime)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int rowsAffected = 0, fDay = 0, tDay = 0;

            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = string.Empty;
            sqlQuery = "Insert into ShiftDetails ([shiftName],[fromDay] ,[toDay],[fromTime] ,[toTime], [shiftId],Running,UpdatedTS) values  (@shiftName , @fromDay, @toDay,  @fromTime, @Totime,@shiftId,1,@UpdatedTS ) ";

            try
            {

                cmd = new SqlCommand(sqlQuery, conn);

                cmd.Parameters.AddWithValue("@shiftId", shiftId);
                cmd.Parameters.AddWithValue("@shiftName", shiftName);

                if (fromDay.Equals("Tomorrow")) fDay = 1;
                else if (fromDay.Equals("Yesterday")) fDay = 2;

                cmd.Parameters.AddWithValue("@fromDay", fDay);

                if (toDay.Equals("Tomorrow")) tDay = 1;
                else if (toDay.Equals("Yesterday")) tDay = 2;

                cmd.Parameters.AddWithValue("@toDay", tDay);

                cmd.Parameters.AddWithValue("@fromTime", fromTime);
                cmd.Parameters.AddWithValue("@toTime", toTime);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static void RemoveAllShiftdata()
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            int rowsAffected = 0;
            SqlConnection conn = sqlConn;
            SqlCommand cmd = null;
            string sqlQuery = "Update shiftDetails SET Running = 0,UpdatedTS=@UpdatedTS where Running = 1";
            try
            {

                cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        internal static ShiftDataModel GetShiftDetails(string shiftId)
        {

            ShiftDataModel shftVals = new ShiftDataModel();
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            DataTable table = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand(@"select * from shiftdetails where running=1 and ShiftId = @ShiftId ", sqlConn);
                cmd.Parameters.AddWithValue("@shiftId", shiftId);

                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        //
                        if (!Convert.IsDBNull(rdr["ShiftId"]))
                        {
                            shftVals.shiftId = Convert.ToString(rdr["ShiftId"]);
                        }

                        if (!Convert.IsDBNull(rdr["ShiftName"]))
                        {
                            shftVals.ShiftName = Convert.ToString(rdr["ShiftName"]);
                        }

                        if (!Convert.IsDBNull(rdr["FromDay"]))
                        {
                            shftVals.FromDay = Convert.ToString(rdr["FromDay"]);
                        }
                        if (!Convert.IsDBNull(rdr["FromTime"]))
                        {
                            shftVals.FromTime = Convert.ToString(rdr["FromTime"]);
                        }

                        if (!Convert.IsDBNull(rdr["ToDay"]))
                        {
                            shftVals.ToDay = Convert.ToString(rdr["ToDay"]);
                        }

                        if (!Convert.IsDBNull(rdr["ToTime"]))
                        {
                            shftVals.ToTime = Convert.ToString(rdr["ToTime"]);
                        }
                    }
                }
                else
                {
                    shftVals = null;
                }
            }

            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return shftVals;
        }

        internal static List<string> bindDataForShiftName()
        {

            List<string> list = new List<string>();
            string sqlquery = "Select * from shiftdetails where running =1";
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlquery, sqlConn);
                cmd.CommandType = CommandType.Text;
                //  cmd.Parameters.AddWithValue("@shiftid", shiftid);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    list.Add(sdr["ShiftName"].ToString());
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;

            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
            return list;
        }

        internal static string bindtextShiftFromname(string shiftname)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                string sqlQuery = "select FromDay from shiftdetails where shiftname=@ShiftName and running=1";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ShiftName", shiftname);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    ShiftName = rdr["FromDay"].ToString();
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
            return ShiftName;
        }

        internal static string binddtpfromShiftname(string Shift)
        {

            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                // string sqlQuery = "select FromTime from shiftdetails where shiftid='" + p + "' and running=1";
                SqlCommand cmd = new SqlCommand("[s_GetShiftTime]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@startdatetime", "01-jan-2000");
                cmd.Parameters.AddWithValue("@Shift", Shift);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftName = rdr["StartTime"].ToString();
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
            return ShiftName;
        }

        internal static string bindtextShiftToname(string shiftname)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                string sqlQuery = "select ToDay from shiftdetails where shiftname=@ShiftName and running=1";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ShiftName", shiftname);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftName = rdr["ToDay"].ToString();
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
            return ShiftName;
        }

        internal static string FindShiftId(string shiftname)
        {

            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftId = string.Empty;
            try
            {
                string sqlQuery = "select shiftid from shiftdetails where ShiftName=@ShiftName and running=1";
                SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@ShiftName", shiftname);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftId = rdr["shiftid"].ToString();
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
            return ShiftId;
        }

        internal static string bindtextdtptoShiftname(string Shift)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            string ShiftName = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("[s_GetShiftTime]", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@startdatetime", "01-jan-2000");
                cmd.Parameters.AddWithValue("@Shift", Shift);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    ShiftName = rdr["EndTime"].ToString();
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
            return ShiftName;
        }

        internal static DataTable bindhourlytimeData(string shiftid)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("select * from ShiftHourDefinition where shiftid=@shiftid order by HourID asc", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@shiftid", shiftid);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return dt;
        }

        internal static void DeleteDataFromHourlyDefination(string shiftId, out string sucessFailure)
        {
            sucessFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("delete from ShiftHourDefinition where ShiftID= @shiftname", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@shiftname", shiftId);
                cmd.ExecuteNonQuery();
                sucessFailure = "Successfull";
            }
            catch (Exception ex)
            {
                sucessFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving data - \n" + ex.ToString());
                throw;
            }
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        internal static void InsertHourDefination(string shiftid, string hrname, string HourId, string FromDay, string ToDay, string HourStart, string HourEnd, string min, out string successFailure)
        {
            successFailure = string.Empty;
            SqlDataReader sdr = null;
            SqlConnection conn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataTable dt = new DataTable();

            string query = @"if not  exists(select * from [ShiftHourDefinition] where  [ShiftID] = @shiftid and [HourName] =@hrname  and [HourID]=@HourId)
                BEGIN
                 insert into ShiftHourDefinition([ShiftID],[HourName],[HourID],[FromDay],[ToDay],[HourStart],[HourEnd],[Minutes],UpdatedTS) values(@shiftid,@hrname,@HourId,@FromDay,@ToDay,@HourStart,@HourEnd,@min,@UpdatedTS)
                END
                else
                BEGIN
                update [ShiftHourDefinition] set [ShiftID]=@shiftid,[HourName]=@hrname ,[FromDay]=@FromDay,[ToDay]=@ToDay,[HourStart]=@HourStart,[HourEnd]=@HourEnd,[Minutes]=@min,UpdatedTS=@UpdatedTS
                where [ShiftID] = @shiftid and [HourName] =@hrname  and [HourID]=@HourId
                END";
            try
            {
                cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@shiftid", shiftid);
                cmd.Parameters.AddWithValue("@hrname", hrname);
                cmd.Parameters.AddWithValue("@HourId", HourId);
                cmd.Parameters.AddWithValue("@FromDay", FromDay);
                cmd.Parameters.AddWithValue("@ToDay", ToDay);
                cmd.Parameters.AddWithValue("@HourStart", DateTime.Parse(HourStart).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@HourEnd", DateTime.Parse(HourEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@min", min);
                cmd.Parameters.AddWithValue("@UpdatedTS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                successFailure = "Successfull";
            }
            catch (Exception ex)
            {
                successFailure = ex.Message;
                Logger.WriteErrorLog("Error in retriving Data - \n" + ex.ToString());
                throw;
            } 
            finally
            {
                if (sdr != null) sdr.Close();
                if (conn != null) conn.Close();
            }
        }

        #region --- Precision Engg ----
        #region "Save Down Code Information"
        internal static void InsertUpdateDownCodeInformationPE(string downid, string interfaceid, string downDescription, string downdescriptionInHindi, string catagory, out string msg)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            try
            {

                SqlCommand cmd = new SqlCommand(@"IF Exists(select * from downcodeinformation where downid=@downid and interfaceid=@interfaceid)
BEGIN
UPDATE downcodeinformation set downdescription=@downDescription, DownDescriptionHindi=@downDescriptionInHindi where downid=@downid and interfaceid=@interfaceid and Catagory=@catagory
END", sqlConn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue(@"downid", downid);
                cmd.Parameters.AddWithValue(@"interfaceid", interfaceid);
                cmd.Parameters.AddWithValue(@"downDescription", downDescription);
                cmd.Parameters.AddWithValue(@"downDescriptionInHindi", downdescriptionInHindi);
                cmd.Parameters.AddWithValue(@"catagory", catagory);
                int x = cmd.ExecuteNonQuery();
                msg = "Records save successfully";

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                Logger.WriteErrorLog("Error Log - \n " + ex.ToString());
            }
            finally
            {
                if (sqlConn != null) sqlConn.Close();
            }
        }
        #endregion

        internal static List<DownCodesModel> GetAllDownCodeInfoPE(string downid, string interfaceid, string catagory, string param, string timeFormat)
        {
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            List<DownCodesModel> list = new List<DownCodesModel>();
            try
            {
                SqlDataReader sdr = null;
                SqlCommand cmd = new SqlCommand("select * from downcodeinformation", sqlConn);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.Text;
                
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        DownCodesModel DownVals = new DownCodesModel();
                        if (!Convert.IsDBNull(sdr["downid"]))
                        {
                            DownVals.downid = Convert.ToString(sdr["downid"]);
                        }

                        if (!Convert.IsDBNull(sdr["Catagory"]))
                        {
                            DownVals.Catagory = Convert.ToString(sdr["Catagory"]);
                        }
                        if (!Convert.IsDBNull(sdr["downdescription"]))
                        {
                            DownVals.downdescription = (Convert.ToString(sdr["downdescription"]));
                        }
                        if (!Convert.IsDBNull(sdr["DownDescriptionHindi"]))
                        {
                            DownVals.DownDescriptionInHindi = Convert.ToString(sdr["DownDescriptionHindi"]).Equals("null", StringComparison.OrdinalIgnoreCase) ? "" : Convert.ToString(sdr["DownDescriptionHindi"]);
                        }
                        if (!Convert.IsDBNull(sdr["interfaceid"]))
                        {
                            DownVals.interfaceid = Convert.ToString(sdr["interfaceid"]);                          
                        }

                        list.Add(DownVals);
                    }
                }
                else
                {
                    list = null;
                }
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


        #endregion
    }
}