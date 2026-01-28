using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class FlowMeterChartData
    {
        public string name { get; set; }
        // public double[] data { get; set; }
        public List<FlowMeterChartSeriesData> data { get; set; }
    }
    public class FlowMeterChartSeriesData
    {
        public double y { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
    }

    public class FlowMeterChartModel
    {
        public double MinValue1 { get; set; }
        public double MaxValue1 { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double MedianValue { get; set; }
        public List<FlowMeterChartData> flowMeterChartDatas { get; set; }
    }
}