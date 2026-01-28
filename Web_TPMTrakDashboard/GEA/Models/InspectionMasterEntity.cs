using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Models
{
    public class InspectionMasterEntity
    {
        public int ID { get; set; }
        public string InsPlanNumber { get; set; }
        public string InsParamID { get; set; }
        public string InsParameter { get; set; }
        public string LSL { get; set; }
        public string USL { get; set; }
        public string UOM { get; set; }
        public string DataTemplate { get; set; }
        public bool AppliesToOperator { get; set; }
        public bool MandatoryForOperator { get; set; }
        public bool AppliesToQuality { get; set; }
        public bool MandatoryForQuality { get; set; }
        public bool IsEnabled { get; set; }
    }
}