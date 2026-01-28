using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class EnergyDataEntity
    {
        public string MachineID { get; set; }
        public string NodeID { get; set; }
        public double Energy { get; set; }
    }

    public class EnergyChartData
    {
        public string name { get; set; }
        public double y { get; set; }
    }
}