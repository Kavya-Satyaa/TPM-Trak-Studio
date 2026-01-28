using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.SKS.Model
{
    public class SKSDTO
    {
    }
    public class ScheduleEntity
    {
        public string UserPriority { get; set; }
        public string SchedulePriority { get; set; }
        public string MachineID { get; set; }
        public string WorkOrder { get; set; }
        public string PartID { get; set; }
        public string PartDesc { get; set; }
        public string ToolLayout { get; set; }
        public string DrawingNumber { get; set; }
        public string RMGradeSize { get; set; }
        public string PlannedQtyNo { get; set; }
        public string PlannedQtyWt { get; set; }
        public string OperationNo { get; set; }
        public string Speed { get; set; }
        public string Status { get; set; }
        public string UpdatedTS { get; set; }
    }
    public class PriorityType
    {
        public string UserPriority { get; set; }
        public string SystemPriority { get; set; }
    }
}