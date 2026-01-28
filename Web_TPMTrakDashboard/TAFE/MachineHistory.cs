using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.TAFE
{
    public class MachineHistory
    {
        public string MachineID { get; set; }
        public string DownCode { get; set; }
        public string DownDescription { get; set; }
        public string KindOfProblem { get; set; }
        public string Reason { get; set; }
        public string DownCategory { get; set; }
        public string BreakDownStart { get; set; }
        public string BreakDownEnd { get; set; }
        public string ActionToResolve { get; set; }
        public string ActionProposed { get; set; }
        public string TimeLost { get; set; }
        public string ElapsedTime { get; set; }
        public string Severity { get; set; }
    }
}