using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.MahindraAndMahindra
{
    public class DashBoardChartSeries
    {
    }
    public class ChartSeries
    {
        public string Title { get; set; }
        public List<double> Category { get; set; }
        public List<DataSeries> series { get; set; }
        public List<PlotLines> PlotLines { get; set; }
        public List<CycleTimes> CycleStartEnd { get; set; }
    }
    public class DataSeries
    {
        public string name { get; set; }
        public List<double[]> data { get; set; }
        public bool step { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public Tooltipdata tooltip { get; set; }
        public Marker marker { get; set; }
    }

    public class PlotLines
    {
        public string color { get; set; }
        public int width { get; set; }
        public double value { get; set; }
        public int zIndex { get; set; }
        public plotLineLabelParam label { get; set; }
    }

    public class CycleTimes
    {
        public string Start { get; set; }
        public string End { get; set; }
    }
    public class Tooltipdata
    {
        public List<double> ApplyRuleFor_N_Observation { get; set; }
        public List<double> Total_M_Observation { get; set; }
        public List<string> Operationno { get; set; }
        public List<string> Machineid { get; set; }
        public List<string> ComponentID { get; set; }
    }
    public class Marker
    {
        public bool enabled { get; set; }
        public int radius { get; set; }
        public string symbol { get; set; }
    }
    public class plotLineLabelParam
    {
        public string text { get; set; }
        public int rotation { get; set; }
        public string align { get; set; }
        public int y { get; set; }
        public int x { get; set; }
    }

}