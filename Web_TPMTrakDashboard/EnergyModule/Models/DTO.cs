using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.EnergyModule.Models
{
    public class DTO
    {
    }
    public class GridSettings
    {
        public string ColumnName { get; set; }
        public string ColumnText { get; set; }
        public string SortOrder { get; set; }
        public bool Visibility { get; set; }
    }
    public class LiveDataCs
    {
        public string Machineid { get; set; }
        public string DateTime { get; set; }
        public string VLN_R { get; set; }
        public string VLN_Y { get; set; }
        public string VLN_B { get; set; }
        public string VLN_R_Y { get; set; }
        public string VLN_Y_B { get; set; }
        public string VLN_B_R { get; set; }
        public string R_AMP { get; set; }
        public string Y_AMP { get; set; }
        public string B_AMP { get; set; }
        public string PowerFactor { get; set; }
        public string Kw { get; set; }
        public string Kwh { get; set; }
        public string LastArrival_TS { get; set; }
    }
    public class ShopDefaultEntity
    {
        public string Parameter { get; set; }
        public string ValueInText { get; set; }
        public string ValueInText2 { get; set; }
        public int ValueInInt { get; set; }
    }
}