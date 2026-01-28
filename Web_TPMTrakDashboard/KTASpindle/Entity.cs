using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public class Entity
    {

    }

    public class MachineEntity
    {
        public string MachineID { get; set; }
        public List<ComponentData> CompList { get; set; }
        public List<TargetData> TargetList { get; set; }
        public List<DownCodeData> DownList { get; set; }
    }

    public class DownCodeData
    {
        public string MachineID { get; set; }
        public string DownCode { get; set; }
        public string Time { get; set; }
        //public List<string> TotalTime { get; set; }
    }

    public class TargetData
    {
        public string MachineID { get; set; }
        public int Hr { get; set; }
        public double ActualQty { get; set; }
    }

    public class ComponentData
    {
        public string MachineID { get; set; }
        public string AE { get; set; }
        public string PE { get; set; }
        public string QE { get; set; }
        public string OEE { get; set; }
        public string Component { get; set; }
        public string Operation { get; set; }
        public string ActualCount { get; set; }
        public string Operator { get; set; }
        public string LastCycleEnd { get; set; }
        public string DownTime { get; set; }
        public string UtilizedTime { get; set; }
    }

    public class SettingsEntity
    {
        public int HeaderFontSize { get; set; }
        public int ContentFontSize { get; set; }
        public int FlipInterval { get; set; }
        public int TopDownCode { get; set; }
        public string DisplayType { get; set; }
    }

    public class ScheduleMasterEntity
    {
        public string IDD { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleDateTime { get; set; }
        public string Plant { get; set; }
        public string Cell { get; set; }
        public string Machine { get; set; }
        public string MachineInterfaceID { get; set; }
        public string Component { get; set; }
        public string Operation { get; set; }
        public bool IsAssigned { get; set; }
        public string CompInterfaceId { get; set; }
        public string OpnInterfaceId { get; set; }
        public string WorkOrder { get; set; }
        public string Quantity { get; set; }
        public string WorkOrderDate { get; set; }
        public string Status { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedTS { get; set; }
        public string CompDesc { get; set; }
        public string Priorityno { get; set; }
        public bool PriorityReadOnly { get; set; }
        public bool chkUpdateStore { get; set; }
    }

}