using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Models
{
    public class MaintenanceChecklistEntity
    {
        public int IDD { get; set; }
        public int ActivityID { get; set; }
        public string Activity { get; set; }
        public int FreqID { get; set; }
        public string MachineID { get; set; }
        public bool IsEnabled { get; set; }
        public string Method { get; set; }
        public string Criteria { get; set; }
        public string TemplateType { get; set; }
    }
}