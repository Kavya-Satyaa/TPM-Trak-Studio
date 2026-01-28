using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class MachineEfficiencyColorCode
    {
        public string PENotOk { get; set; }
        public string AENotOk { get; set; }
        public string OEENotOk { get; set; }
        public string QENotOk { get; set; }

        public string PEOk { get; set; }
        public string AEOk { get; set; }
        public string OEEOk { get; set; }
        public string QEOk { get; set; }
        public string OperatorPEGreen { get; set; }
        public string OperatorPERed { get; set; }
    }

    public class MachineControlInfo
    {
        public string ControlType { get; set; }
        public string ProgStartId { get; set; }
        public string ProgramEndId { get; set; }
        public string FileNameFrom { get; set; }
        public string RecieveAtMachine { get; set; }
        public string RecieveFromMachine { get; set; }
    }

    public class MachineMakeData
    {
        public string Manufacturer { get; set; }
        public string DateOfManufacturer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
        public string Place { get; set; }
    }

    public class MachineEfficiencyTargetData
    {
        public string EffiTargetOwner { get; set; }
        public bool CriticalMachineEnabled { get; set; }
        public string Period { get; set; }
        public string Year { get; set; }
        public string TargetLevel { get; set; }
        public string PE { get; set; }
        public string AE { get; set; }
        public string OEE { get; set; }
        public string QE { get; set; }
    }
}