using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Models
{
    public class DTO
    {
    }
    public class CompOpnEntity
    {
        public string Component { get; set; }
        public string Operation { get; set; }
    }
    public class TargetValues
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string TargetValue { get; set; }
        public string PlantID { get; set; }
    }
    public class AutoScheduleMasterEntity
    {
        public string ComponentID { get; set; } = string.Empty;
        public string OperationNo { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public bool ToBeSentToStores { get; set; } = false;
        public string OldMachineID { get; set; } = string.Empty;
    }
    public class StoresDataEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string DateOfSchedule { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string ProductionOrder { get; set; } = string.Empty;
        public string SeriesNo { get; set; } = string.Empty;
        public string TimeWaitingAtStores { get; set; } = string.Empty;
        public string WhoHasReceived { get; set; } = string.Empty;
        public string UpdatedTS { get; set; } = string.Empty;

    }
    public class MaterialTracking_GEA
    {
        public string SNo { get; set; } = string.Empty;
        public string DateOfSchedule { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string ProductionOrderNo { get; set; } = string.Empty;
        public string seriesNo { get; set; } = string.Empty;
        public string TimeWaitingAtStores { get; set; } = string.Empty;
        public string WhoHasReceived { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string CycleTime { get; set; } = string.Empty;
        public bool ReceiptCompletion { get; set; } = false;
        public string DateTimeCompletion { get; set; } = string.Empty;
        public string CompInterfaceID { get; set; } = string.Empty;
        public string CompID { get; set; } = string.Empty;
        public string CompDesc { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    public class TraceabilityDashboardEntity
    {
        public TraceabilityChartEntity WeeklyChartData { get; set; } = new TraceabilityChartEntity();
        public TraceabilityChartEntity MaterialStatusChartData { get; set; } = new TraceabilityChartEntity();
        public double StockAvailability { get; set; } = 0;
        public string TotalDownTime { get; set; } = string.Empty;
        public double ReceiptCompletion { get; set; } = 0;
        public List<MaterialTracking_GEA> StoresList { get; set; } = new List<MaterialTracking_GEA>();
    }
    public class TraceabilityChartEntity
    {
        public List<string> Category { get; set; } = new List<string>();
        public List<double> Data { get; set; } = new List<double>();
        public List<double> Data2 { get; set; } = new List<double>();
    }
    public class ReceiptCompletionTargetEntity
    {
        public string Year { get; set; } = string.Empty;
        public string WeekNo { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
    }
    public class MissingItemEntity
    {
        public string Item { get; set; } = string.Empty;
        public string MaterialID { get; set; } = string.Empty;
        public string MaterialDesc { get; set; } = string.Empty;
        public string Qty { get; set; } = string.Empty;
        public string DateOfMissing { get; set; } = string.Empty;
        public bool IsIssued { get; set; } = false;
        public string Status { get; set; } = string.Empty;
        public string DateOfReIssue { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;
        public string ShortageQty { get; set; } = string.Empty;
    }
}