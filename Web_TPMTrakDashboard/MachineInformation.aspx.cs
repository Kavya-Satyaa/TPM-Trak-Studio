using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class MachineInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlType();
                BindManufacturer();
            }
            //if(System.Web.Configuration.WebConfigurationManager.AppSettings["KTASpindlePages"].ToString() != "1")
           
        }

        #region "Bind Control Type"
        private void BindControlType()
        {
            try
            {
                ddlControlType.DataSource = DownCodeInfoDataBase.GetControlNames();
                ddlControlType.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        #region "Machine Manufacturer info"
        private void BindManufacturer()
        {
            try
            {
                ddlManufacturer.DataSource = DownCodeInfoDataBase.BindManufacturerInfo();
                ddlManufacturer.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        #region "Get Top Down Time Info"
        [WebMethod]
        public static List<MachineInfoModel> MachineDetailsInfo()
        {
            List<MachineInfoModel> lstMachineInfo = null;
            try
            {
                lstMachineInfo = DownCodeInfoDataBase.GetAllMachineDetails("");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return lstMachineInfo;
        }
        #endregion

        #region "Update Machine Info"
        [WebMethod]
        public static string UpdateMachineInfo(MachineInfoModel model)
        {
            string msg = null;
            int TPMTrakEnabled = 0;
            int SmartTransEnabled = 0;
            int EthernetEnabled = 0;
            int Nto1Device = 0;
            int DNC = 0;
            int CriticalMachineEnabled = 0;
            int MobileEnabled = 0;
            try
            {
                bool param = false;
                foreach (var item in model.ListMachineInfo)
                {
                    msg = "";
                    //if (item.TpmFlags != null)
                    //{
                    //    foreach (var item1 in item.TpmFlags)
                    //    {
                    //        if (item1.Equals("TPMTrakEnabled", StringComparison.OrdinalIgnoreCase))
                    //            TPMTrakEnabled = 1;
                    //        if (item1.Equals("SmartTransEnabled", StringComparison.OrdinalIgnoreCase))
                    //            SmartTransEnabled = 1;
                    //        if (item1.Equals("EthernetEnabled", StringComparison.OrdinalIgnoreCase))
                    //            EthernetEnabled = 1;
                    //        if (item1.Equals("Nto1Device", StringComparison.OrdinalIgnoreCase))
                    //            Nto1Device = 1;
                    //    }
                    //}
                    if (item.TPMTRAKEnable.Equals("true", StringComparison.OrdinalIgnoreCase))
                        TPMTrakEnabled = 1;
                    if (item.SmartTransEnable.Equals("true", StringComparison.OrdinalIgnoreCase))
                        SmartTransEnabled = 1;
                    if (item.DNCEnable.Equals("true", StringComparison.OrdinalIgnoreCase))
                        DNC = 1;
                    if (item.SharedDevice.Equals("true", StringComparison.OrdinalIgnoreCase))
                        Nto1Device = 1;
                    if (item.CriticalMachineEnable.Equals("true", StringComparison.OrdinalIgnoreCase))
                        CriticalMachineEnabled = 1;
                    if (item.MobileEnable.Equals("true", StringComparison.OrdinalIgnoreCase))
                        MobileEnabled = 1;
                    char chrFullStop = '.';
                    string[] arrOctets = item.IP.Split(chrFullStop);
                    if(!item.DNCEnable.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (arrOctets.Length != 4)
                        {
                            msg = "Invalid IP Address";
                        }
                        //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
                        Int16 MAXVALUE = 255;
                        Int32 temp; // Parse returns Int32
                        foreach (String strOctet in arrOctets)
                        {
                            if (strOctet.Length > 3)
                            {
                                msg = "Invalid IP Address";
                                break;
                            }
                            try
                            {
                                temp = int.Parse(strOctet);
                            }
                            catch (Exception)
                            {
                                msg = "Invalid IP Address";
                                break;
                            }

                            if (temp > MAXVALUE)
                            {
                                msg = "Invalid IP Address";
                                break;
                            }
                        }
                    }
                    if (item.DNCEnable.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        chrFullStop = '.';
                        arrOctets = item.DNSIP.Split(chrFullStop);
                        //if (arrOctets.Length != 4)
                        //{
                        //    msg = "Invalid IP Address";
                        //}
                        //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
                        Int16 MAXVALUE = 255;
                        Int32 temp;
                        foreach (String strOctet in arrOctets)
                        {
                            if (strOctet.Length > 3)
                            {
                                msg = "Invalid DNS IP Address";
                                break;
                            }
                            try
                            {
                                temp = int.Parse(strOctet);
                            }
                            catch (Exception)
                            {
                                msg = "Invalid DNS IP Address";
                                break;
                            }

                            if (temp > MAXVALUE)
                            {
                                msg = "Invalid DNS IP Address";
                                break;
                            }
                        }
                    }
                    string opcuaUrl = "";
                    if(!string.IsNullOrEmpty(item.OPCUAIPAddress) && !string.IsNullOrEmpty(item.OPCUAPort))
                    {
                        opcuaUrl = (item.OPCUAIPAddress + ":" + item.OPCUAPort);
                    }
                    else if (!string.IsNullOrEmpty(item.OPCUAIPAddress) && string.IsNullOrEmpty(item.OPCUAPort))
                    {
                        opcuaUrl = (item.OPCUAIPAddress);
                    }
                    else if (string.IsNullOrEmpty(item.OPCUAIPAddress) && !string.IsNullOrEmpty(item.OPCUAPort))
                    {
                        opcuaUrl = ":" + (item.OPCUAPort);
                    }

                    if (string.IsNullOrEmpty(msg))
                    {
                        DownCodeInfoDataBase.InsertUpdateMachineInformation(item.MachineID.Trim(), item.Description, item.InterfaceID, item.ProtcolInString, item.IP, item.PortNo, item.BulkDataTransferPortNo, item.mchrrate, TPMTrakEnabled, SmartTransEnabled, EthernetEnabled, Nto1Device, DNC, item.DNSIP, item.DNSIPPortNo, CriticalMachineEnabled, MobileEnabled, opcuaUrl, item.NoofFixture, item.NoofFixtureForPallet2, item.PalletEnabledBool, out param);

                        if (param)
                        {
                            msg = "Records update successfully";
                        }
                        else
                        {
                            msg = "Error. Could not save the details";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return msg;
        }
        #endregion

        #region "Update Machine Information"
        [WebMethod]
        public static string UpdateMachineInformation(MachineInformationModel model)
        {
            string msg = null;
            bool isSuccessFailure = false;
            try
            {
                foreach (var item in model.ListMachineInformation)
                {
                    DownCodeInfoDataBase.InsertUpdateMachineInfo("EffColorCode", item.selectedMachine, "", 0,
                        0, 0, "", "", "", "", 0, 0, 0, item.txtPeOk, item.txtPeNotOk, item.txtAeOk, item.txtAeNotOk,
                        item.txtOaeOk, item.txtOaeNotOk, "", true, 0, true, true, "", "", "",
                        item.EffiTargetCriticalMachine, 0, 0, 0, item.txtQeNotOk, item.txtQeOk,
                        true, true, "", "", "",
                        item.txtProgStartId, item.txtProgEndId, "", item.txtreceive, item.txtSend, "", "", 0,
                        "", "", "", "", "", "",true, out isSuccessFailure,item.txtOperatorPEOK,item.txtOperatorPENotOK);

                    DownCodeInfoDataBase.InsertUpdateMachineInfo("InsertEffTarget", item.selectedMachine, "", 0,
                        0, 0, "", "", "", "", 0, 0, 0, item.txtPeOk, item.txtPeNotOk, item.txtAeOk, item.txtAeNotOk,
                        item.txtOaeOk, item.txtOaeNotOk, "", true, 0, true, true, "", "", "",
                        item.EffiTargetCriticalMachine, 0, 0, 0, item.txtQeNotOk, item.txtQeOk,
                        true, true, "", "", "",
                        item.txtProgStartId, item.txtProgEndId, "", item.txtreceive, item.txtSend, "", "", 0,
                        "", "", "", "", "", "", true, out isSuccessFailure, item.txtOperatorPEOK, item.txtOperatorPENotOK);

                    int i = 1;
                    string pStartID = string.Empty;
                    string pEndID = string.Empty;
                    if (item.controlName.Trim().ToString() != "PLC" && item.controlName.Trim().ToString() != "NON CNC")
                    {
                        while (i <= item.txtProgStartId.Trim().Length)
                        {
                            if (Mid(item.txtProgStartId, i, 1) == "|")
                            {
                                pStartID = pStartID + Mid(item.txtProgStartId, i + 1, 2) + ",";
                                i = i + 2;
                            }
                            else
                            {
                                string val = Mid(item.txtProgStartId, i, 1);
                                Char b = Char.Parse(val);
                                int n = b;
                                pStartID = pStartID + n + ",";

                            }
                            i = i + 1;
                        }


                        i = 1;
                        while (i <= item.txtProgEndId.Trim().Length)
                        {
                            if (Mid(item.txtProgEndId, i, 1) == "|")
                            {
                                pEndID = pEndID + Mid(item.txtProgEndId.ToString(), i + 1, 2) + ",";
                                i = i + 2;
                            }
                            else
                            {
                                string val = Mid(item.txtProgEndId, i, 1);
                                Char b = Char.Parse(val);
                                int n = b;
                                pEndID = pEndID + n + ",";

                            }
                            i = i + 1;
                        }

                        DownCodeInfoDataBase.InsertUpdateMachineInfo("InsertMachineControlInfo", item.selectedMachine, "", 0,
                            0, 0, "", "", "", "", 0, 0, 0, item.txtPeOk, item.txtPeNotOk, item.txtAeOk, item.txtAeNotOk,
                            item.txtOaeOk, item.txtOaeNotOk, "", true, 0, true, true, "", "", "", true, 0, 0, 0, item.txtQeNotOk,
                            item.txtQeOk, true, true, "", "", item.controlName,
                            pStartID, pEndID, "", item.txtreceive, item.txtSend, "", "", 0,
                           "", "", "", "", "", "",true, out isSuccessFailure, item.txtOperatorPEOK, item.txtOperatorPENotOK);
                    }

                    DateTime dateTime;
                    if (!DateTime.TryParse(item.dateOfManufacture, out dateTime))
                    {
                        item.dateOfManufacture = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    DownCodeInfoDataBase.InsertUpdateMachineInfo("InsertMachineMakeInfo", item.selectedMachine, "", 0,
                       0, 0, "", "", "", "", 0, 0, 0, item.txtPeOk, item.txtPeNotOk, item.txtAeOk, item.txtAeNotOk,
                       item.txtOaeOk, item.txtOaeNotOk, "", true, 0, true, true, "", "", "", true, 0, 0, 0, item.txtQeNotOk,
                       item.txtQeOk, true, true, "", "", "",
                       item.txtProgStartId, item.txtProgEndId, "", item.txtreceive, item.txtSend, "", "", 0,
                       item.manufacturer, item.dateOfManufacture, item.address, item.place, item.phone, item.contactPerson, true,out isSuccessFailure, item.txtOperatorPEOK, item.txtOperatorPENotOK);

                    if (isSuccessFailure)
                    {
                        msg = "Records update successfully";
                    }
                    else
                    {
                        msg = "Error. Could not save the details";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                throw;
            }
            return msg;
        }

        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        #endregion

        #region "Machine Efficiency Color Code Info"
        [WebMethod]
        public static MachineEfficiencyColorCode MachineEfficiencyColorCodeInfo(string machineId)
        {
            MachineEfficiencyColorCode value = new MachineEfficiencyColorCode();
            try
            {
                value = DownCodeInfoDataBase.GetMachineColorCodes(machineId, "MachineColorCode");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return value;
        }
        #endregion

        #region "Machine Control Info"
        [WebMethod]
        public static MachineControlInfo MachineControlInfo(string machineId)
        {
            MachineControlInfo value = new MachineControlInfo();
            try
            {
                value = DownCodeInfoDataBase.GetMachineControlInfo(machineId, "MachineControlinfo");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return value;
        }
        #endregion

        #region "Machine Make Data"
        [WebMethod]
        public static MachineMakeData MachineMakeData(string machineId)
        {
            MachineMakeData value = new MachineMakeData();
            try
            {
                value = DownCodeInfoDataBase.GetMachineMakeInfo(machineId, "machineMakeinfo");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return value;
        }
        #endregion

        #region "Machine Efficiency Target Data"
        [WebMethod]
        public static MachineEfficiencyTargetData MachineEfficiencyTargetData(string machineId)
        {
            MachineEfficiencyTargetData value = new MachineEfficiencyTargetData();
            try
            {
                value = DownCodeInfoDataBase.GetMachineEfficiencyTarget(machineId, "machineEfficiencyTarget");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
            }
            return value;
        }
        #endregion
        [WebMethod]
        public static bool isAmararajaMangalEnabled()
        {
            bool isEnabled = false;
            try
            {
                if (ConfigurationManager.AppSettings["AmararagaMangalPages"].ToString() == "1")
                {
                    isEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return isEnabled;
        }

    }
}