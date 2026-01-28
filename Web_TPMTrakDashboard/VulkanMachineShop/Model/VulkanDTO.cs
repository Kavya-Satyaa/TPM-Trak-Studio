using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.VulkanMachineShop.Model
{
    public class VulkanDTO
    {

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
        public string WorkOrderQty { get; set; } = string.Empty;
    }

    public class InspectionTransactionEntity
    {
        public string ComponentID { get; set; } = string.Empty;
        public string OperationNo { get; set; } = string.Empty;
        public string CharacteristicID { get; set; } = string.Empty;
        public string CharacteristicCode { get; set; } = string.Empty;
        public string LSL { get; set; } = string.Empty;
        public string USL { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string HeatCodeValue { get; set; } = string.Empty;
        public string SetValue { get; set; } = string.Empty;
        public int ColSpanApproval { get; set; } = 1;
        public bool HeaderVisibility { get; set; } = false;    
        public bool ContentVisibility { get; set; } = false;
        public bool OperatorRowVisibility { get; set; } = false;
        public bool FooterVisisbility { get; set; } = false;
        public bool ButtonVisibility { get; set; } = false;

        public List<InspectionTransactionEntity> listofHeatCode { get; set; } = new List<InspectionTransactionEntity>();

    }

    public class PMMasterEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string CheckpointID { get; set; } = string.Empty;
        public string CheckpointDesc { get; set; } = string.Empty;
        public string Particular { get; set; } = string.Empty;
        public string ControlMethod { get; set; } = string.Empty;
        public string Responsibility { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;

        public List<string> dropdownvalue_Frequency { get; set; } = new List<string>();
        public List<string> dropdownvalue_Responsibility { get; set; } = new List<string>();
    }

    public class PMTransactionEntity
    {
        public string CheckpointID { get; set; } = string.Empty;
        public string CheckpointItem { get; set; } = string.Empty;
        public string Particular { get; set; } = string.Empty;
        public string ControlMethod { get; set; } = string.Empty;
        public string Responsibility { get; set; } = string.Empty;
        public string DayHeader { get; set; } = string.Empty;
        public string DayValue { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool SignVisibility { get; set; } = false;
        public bool ApproveVisibility { get; set; } = false;
        public string BackgroundClass { get; set; } = string.Empty;
        public string innerDayValueClass { get; set; } = string.Empty;
        public int ApprovalcolSpan { get; set; } = 5;
        public List<PMTransactionEntity> TransactionData { get; set; } = new List<PMTransactionEntity>();
    }

    public class MachineAndonData
    {
        public string Machineid { get; set; }
        public List<ParameterData> ParametersData { get; set; } = new List<ParameterData>();
        public string BackColor { get; set; }
    }
    public class ParameterData
    {
        public string LabelText { get; set; } = string.Empty;
        public string LabelValue { get; set; } = string.Empty;
        public string BackColorTitle { get; set; } = string.Empty;
        public string BackColorValue { get; set; } = string.Empty;
        public string ForeColor { get; set; } = string.Empty;
        public int SortOrder = 1;
    }

    public class ToolLifeEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string ToolNo { get; set; } = string.Empty;
        public string InterfaceID { get; set; } = string.Empty;
        public string ToolType { get; set; } = string.Empty;
        public string ToolSpecification { get; set; } = string.Empty;
        public string ToolFeed { get; set; } = string.Empty;
        public string NoOfEdges { get; set; } = string.Empty;
        public string ToolLifeInMeter { get; set; } = string.Empty;

    }

}