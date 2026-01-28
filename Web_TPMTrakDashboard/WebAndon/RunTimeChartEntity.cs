using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.WebAndon
{
    public class RunTimeChartEntity
    {
        public Series series { get; set; }
        public List<string> category { get; set; }
        public int MaxValue { get; set; }
    }

    public class Series
    {
        public string name { get; set; }
        public List<data> data { get; set; }
        public string borderColor { get; set; }
        public double pointWidth { get; set; }
        public double turboThreshold { get; set; }
    }

    public class data
    {
        public double y { get; set; }
        public double x { get; set; }
        public double x2 { get; set; }
        public string color { get; set; }
        public string status { get; set; }
    }
}