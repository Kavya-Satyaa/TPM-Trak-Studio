using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPM_TRAK_AnalyticsWebReports
{
    public class EfficiencyEntity
    {
        public string MachineID { get; set; }
        public DateTime StartDate { get; set; }
        public double AE { get; set; }
        public double PE { get; set; }
        public double QE { get; set; }
        public double OE { get; set; }
    }
}
