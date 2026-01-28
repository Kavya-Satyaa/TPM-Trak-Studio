using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Pitti.Model
{
    public class PittiDTO
    {
    }
    public class WorkorderTrackingEntity
    {
        public string WorkOrder { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string ComponentID { get; set; } = string.Empty;
        public bool HeaderVisible { get; set; } = false;
        public bool HeaderVisible2 { get; set; } = false;
        public bool ContentVisible { get; set; } = false;
        public bool RejectionVisible { get; set; } = false;
        public string RejectionRemarks { get; set; } = string.Empty;
        public string RejectionBy { get; set; } = string.Empty;
        public List<WorkorderTrackingOperationEntity> OperationList { get; set; } = new List<WorkorderTrackingOperationEntity>();
    }
    public class WorkorderTrackingOperationEntity
    {
        public string OperationNo { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string ManualStatus { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string CycleTime { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public bool HeaderVisible { get; set; } = false;
        public bool HeaderVisible2 { get; set; } = false;
        public bool ContentVisible { get; set; } = false;
        public bool Template1Visible { get; set; } = false;
        public bool Template2Visible { get; set; } = false;
    }
}