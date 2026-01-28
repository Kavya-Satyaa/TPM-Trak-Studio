using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Models
{
    public class SelfInspectionDetails
    {
        public string Description { get; set; }
        public string PartNumber { get; set; }
        public string ProductionOrder { get; set; }
        public string OperationNumber { get; set; }
        public string MachineID { get; set; }
        public string PlanNumber { get; set; }
        public string DrawingNumber { get; set; }
        public string SerailNumber { get; set; }
        public string RowNumber { get; set; }
        public string Parameter { get; set; }
        public string OperatorMeasurement { get; set; }
        public string QualityMeasurement { get; set; }
        public string OperatorName { get; set; }
        public string DateorShift { get; set; }
        public string Remarks { get; set; }
    }
}