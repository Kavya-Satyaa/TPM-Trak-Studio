using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Models
{
    public class ScheduleDetailsEntity
    {
        public int IDD { get; set; }
        public int Priority { get; set; }
        public string UserPriority { get; set; }
        public string ProductionOrderNumber { get; set; }
        public string MaterialID { get; set; }
        public string Model { get; set; }
        public string ModelDescription { get; set; }
        public string OperationNumber { get; set; }
        public int Quantity { get; set; }
        public string StdCycleTime { get; set; }
        public string StdSetupTime { get; set; }
        public string ScheduledStartTime { get; set; }
        public string ScheduledEndTime { get; set; }
        public string ActualStartTime { get; set; }
        public string PredictedCompletionTime { get; set; }
        public string ActualEndTime { get; set; }
        public string GRNNumber { get; set; }
        public string SupplierName { get; set; }
        public bool NewProdDev { get; set; }
        public string Status { get; set; }
        public string OldScheduledStartTime { get; set; }
        public string OldScheduledEndTime { get; set; }
    }

    public class AssemblyScheduleDetailsEntity
    {
        public int IDD { get; set; }
        public int Priority { get; set; }
        public string UserPriority { get; set; }
        public string ProductionOrderNumber { get; set; }
        public string Priority1 { get; set; }
        public string LocalExport { get; set; }
        public string PONumber { get; set; }
        public string SaleOrder { get; set; }
        public string OperationNumber { get; set; }
        public string Model { get; set; }
        public string ScrollWelded { get; set; }
        public string RDDMachines { get; set; }
        public string FabricationNumber { get; set; }
        public int Quantity { get; set; }
        public string StdCycleTime { get; set; }
        public string StdSetupTime { get; set; }
        public string ScheduledStartTime { get; set; }
        public string ScheduledEndTime { get; set; }
        public string ActualStartTime { get; set; }
        public string PredictedCompletionTime { get; set; }
        public string ActualEndTime { get; set; }
        public string Status { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string Activities { get; set; }
        public string SubOperation { get; set; }
        public bool Enabled { get; set; }
        public string OldScheduledStartTime { get; set; }
        public string OldScheduledEndTime { get; set; }
    }
}