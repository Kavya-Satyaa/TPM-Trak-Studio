using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class InspectionMasterData
    {
        public string MachineID { get; set; }
        public string ComponentID { get; set; }
        public string OperationID { get; set; }
        public string CharID { get; set; }
        public string CharCode { get; set; }
        public string specification { get; set; }
        public string LSL { get; set; }
        public string USL { get; set; }
        public string LowerWarningZone { get; set; }
        public string UpperWarningZone { get; set; }
        public string LowerOperatingZone { get; set; }
        public string UpperOperatingZone { get; set; }
        public string UOM { get; set; }
        public string DataTemplate { get; set; }
        public string SampleSize { get; set; }
        public string InspectedBy { get; set; }
        public bool IsEnabled { get; set; }
        public string InputMethod { get; set; }
        public string Channel { get; set; }
        public string SelectedMachineID { get; set; }
        public string SelectedComponentID { get; set; }
        public string SortOrder { get; set; }

    }
}