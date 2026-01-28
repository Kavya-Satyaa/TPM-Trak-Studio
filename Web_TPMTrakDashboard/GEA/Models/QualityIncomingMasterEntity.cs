using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Models
{
    public class QualityIncomingMasterEntity
    {
        public int ID { get; set; }
        public string CharacteristicID { get; set; }
        public string CharacteristicCode { get; set; }
        public string InsPlanNumber { get; set; }
        public string LSL { get; set; }
        public string USL { get; set; }
        public string SetValue { get; set; }
        public string UOM { get; set; }
        public string DataType { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsEnabled { get; set; }
        public string Sequence { get; set; }
    }
}