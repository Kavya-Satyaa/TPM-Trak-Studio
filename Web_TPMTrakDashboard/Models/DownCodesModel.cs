using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class DownCodesModel
    {
        public string downid { get; set; }
        public string Catagory { get; set; }
        public string downdescription { get; set; }
        public string interfaceid { get; set; }
        public bool Availeffy { get; set; }
        public string chkAvaileffy { get; set; }
        public string chkOperatorEffy { get; set; }
        public double OperatorThreshold { get; set; }
        public bool OperatorchkThresholdfrmCO { get; set; }
        public bool chkAvaileffyinbool { get; set; } = false;
        public bool retpermchour { get; set; }
        public double Threshold { get; set; }
        public bool prodeffy { get; set; }
        public bool ThresholdfrmCO { get; set; }
        public string chkThresholdfrmCO { get; set; }
        public bool chkThresholdfrmCOinbool { get; set; } = false;
        public string dispalyValue { get; set; } = "none";
        public string OperatordisplayValue { get; set; } = "none";
        List<string> _listCatagory = new List<string>();
        public string Readonly { get; set; }
        public bool readonlybool { get; set; }
        public List<string> ListCatagory
        {
            get { return _listCatagory; }
            set { _listCatagory = value; }
        }
        List<string> _ListEmployeeID = new List<string>();
        public List<string> ListEmployeeID
        {
            get { return _ListEmployeeID; }
            set { _ListEmployeeID = value; }
        }

       
        List<DownCodesModel> _listDownCode = new List<DownCodesModel>();
        public List<DownCodesModel> ListDownCode
        {
            get { return _listDownCode; }
            set { _listDownCode = value; }
        }

        public string Owner { get; set; }
        public string DownDescriptionInHindi { get; set; } = "";
        public string display { get; set; }
        public bool IgnoreForRuntimeTarget { get; set; } = false;
        public string chkIgnoreForRuntimeTarget { get; set; } = string.Empty;
    }

    public class MachineInfoModel_Shanthi
    {
        public string MachineID { get; set; }
        public string Description { get; set; }
        public string InterfaceID { get; set; }
        public string ProtcolInString { get; set; }
        public string OPNNo { get; set; }
        public string Process { get; set; }
        public string IP { get; set; }
        public string PortNo { get; set; }
        public string BulkDataTransferPortNo { get; set; }
        public string mchrrate { get; set; }
        public string DNCEnable { get; set; }
        public string TPMTRAKEnable { get; set; }
        public string SmartTransEnable { get; set; }
        public string SharedDevice { get; set; }
        public string DNSIP { get; set; }
        public string DNSIPPortNo { get; set; }
        public string RequestFolder { get; set; }
        public string Responcefolder { get; set; }
        public string ACKfolder { get; set; }
        public string SPCfolder { get; set; }

        public string ProgramEnabledTextTemplate { get; set; }
        List<string> _lstProgramEn = new List<string>();
        public List<string> TpmFlags
        {
            get { return _lstProgramEn; }
            set { _lstProgramEn = value; }
        }

        List<MachineInfoModel> _listMachineInfo = new List<MachineInfoModel>();
        public List<MachineInfoModel> ListMachineInfo
        {
            get { return _listMachineInfo; }
            set { _listMachineInfo = value; }
        }
    }   

    public class MachineInfoModel
    {
        public string MachineID { get; set; }
        public string Description { get; set; }
        public string InterfaceID { get; set; }
        public string ProtcolInString { get; set; }
        public string IP { get; set; }
        public string PortNo { get; set; }
        public string BulkDataTransferPortNo { get; set; }
        public string mchrrate { get; set; }
        public string DNCEnable { get; set; }
        public string TPMTRAKEnable { get; set; }
        public string SmartTransEnable { get; set; }
        public string SharedDevice { get; set; }
        public string DNSIP { get; set; }
        public string DNSIPPortNo { get; set; }
        public string RequestFolder { get; set; }
        public string Responcefolder { get; set; }
        public string ACKfolder { get; set; }
        public string SPCfolder { get; set; }
        public string OPNNo { get; set; }
        public string Process { get; set; }
        public string ProgramEnabledTextTemplate { get; set; }
        List<string> _lstProgramEn = new List<string>();
        public List<string> TpmFlags
        {
            get { return _lstProgramEn; }
            set { _lstProgramEn = value; }
        }

        List<MachineInfoModel> _listMachineInfo = new List<MachineInfoModel>();
        public List<MachineInfoModel> ListMachineInfo
        {
            get { return _listMachineInfo; }
            set { _listMachineInfo = value; }
        }
        public string CriticalMachineEnable { get; set; }
        public string MobileEnable { get; set; }
        public string OPCUAURL { get; set; }
        public string OPCUAIPAddress { get; set; } = "";
        public string OPCUAPort { get; set; } = "";
        public string NoofFixture { get; set; } = "";
        public string NoofFixtureForPallet2 { get; set; } = "";
        public string PalletEnabled { get; set; } = "";
        public bool PalletEnabledBool { get; set; } = false;
    }

    public class MachineInformationModel
    {
        public string selectedMachine { get; set; }
        public string desc { get; set; }
        public int status { get; set; }
        public int mchrrate { get; set; }
        public int PortNo { get; set; }
        public string settings { get; set; }
        public string InterfaceID { get; set; }
        public string IP { get; set; }
        public string IpPortNo { get; set; }
        public int mode { get; set; }
        public int autoload { get; set; }
        public int tpmTrakEnabled { get; set; }
        public int txtPeOk { get; set; }
        public int txtPeNotOk { get; set; }
        public int txtAeOk { get; set; }
        public int txtAeNotOk { get; set; }
        public int txtOaeOk { get; set; }
        public int txtOaeNotOk { get; set; }
        public int txtQeOk { get; set; }
        public int txtQeNotOk { get; set; }
        public int txtOperatorPEOK { get; set; }
        public int txtOperatorPENotOK { get; set; }
        public string BulkDataTransferPortNo { get; set; }
        public bool multiSpindleFlag { get; set; }
        public int deviceType { get; set; }
        public bool ppTransferEnabled { get; set; }
        public bool smartTransEnabled { get; set; }
        public string ignoreCoFromMach { get; set; }
        public string autoSetupChangeDown { get; set; }
        public string machineWiseOwner { get; set; }
        public bool EffiTargetCriticalMachine { get; set; }
        public int DapEnabled { get; set; }
        public int lowerPowerThreshold { get; set; }
        public int upperPowerThreshold { get; set; }      
        public bool ethernetEnabled { get; set; }
        public bool nTo1Device { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string controlName { get; set; }
        public string txtProgStartId { get; set; }
        public string txtProgEndId { get; set; }
        public string fileNameFrom { get; set; }
        public string txtreceive { get; set; }
        public string txtSend { get; set; }
        public string nodeReference { get; set; }
        public string nodeId { get; set; }
        public int sortOrder { get; set; }
        public string manufacturer { get; set; }
        public string dateOfManufacture { get; set; }
        public string address { get; set; }
        public string place { get; set; }
        public string phone { get; set; }
        public string contactPerson { get; set; }

        List<MachineInformationModel> _listMachineInformation = new List<MachineInformationModel>();
        public List<MachineInformationModel> ListMachineInformation
        {
            get { return _listMachineInformation; }
            set { _listMachineInformation = value; }
        }
        public bool CriticalMachineEnable { get; set; }

        public bool MobileEnable { get; set; }
    }
    public class VisibilityEntity
    {
        public string Text { get; set; }

        public bool Value { get; set; }
    }
}