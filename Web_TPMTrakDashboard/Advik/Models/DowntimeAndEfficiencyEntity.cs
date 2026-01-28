using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Advik.Models
{
    public class DowntimeAndEfficiencyEntity
    {
        public List<DownTimeEntity> DownTimeDetails { get; set; }
        public List<EfficiencyEntity> EfficiencyDetails { get; set; }
        public MonthwiseEfficiencyEntity monthwiseAvgEfficiency { get; set; }
    }

    public class DownTimeEntity
    {
        public string DownID { get; set; }
        public double Downtime { get; set; }
    }

    public class EfficiencyEntity
    {
        public string PlantID { get; set; }
        public string EfficiencyID { get; set; }
        public double Efficiency { get; set; }
    }

    public class MonthwiseEfficiencyEntity
    {
        public int Target { get; set; }
        public int Actual { get; set; }
        public int Throughput { get; set; }
    }
}