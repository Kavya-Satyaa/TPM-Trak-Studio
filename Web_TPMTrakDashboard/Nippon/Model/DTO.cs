using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Nippon.Model
{
    public class DTO
    {
    }
    public class AndonSettingData
    {
        public string MachineID { get; set; }
        public string SortOrder { get; set; }
        public bool IsEnabled { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string FontColor { get; set; }
        public string ValueInText { get; set; }
        public string Parameter { get; set; }
        public string ProdutionOEE { get; set; }
    }
}