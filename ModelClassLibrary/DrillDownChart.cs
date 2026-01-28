using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelClassLibrary
{

    public class ChartSeries<T>
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string YAxisTooltipValueSuffix { get; set; }
        public bool legend { get; set; }
        public List<string> categories { get; set; }
        public List<Series1> series { get; set; }
        public List<DrildownSeries1> drilldown { get; set; }
    }
    public class ChartData
    {
        public string name { get; set; }
        public double y { get; set; }
        public string drilldown { get; set; }
        public string afterTitle { get; set; }
        public string beforeTitle { get; set; }
        public string Title { get; set; }
    }

    public class Series1
    {
        public string type { get; set; }
        public bool colorByPoint { get; set; }
        public string name { get; set; }
        public List<ChartData> data { get; set; }
        public int yAxis { get; set; }
        public int zIndex { get; set; }
        public int baseSeries { get; set; }
    }

    public class DrildownSeries1
    {
        public string type { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public bool colorByPoint { get; set; }
        public List<DrildownData1> data { get; set; }
    }

    public class DrildownData1
    {
        public string name { get; set; }
        public double y { get; set; }
        public string drilldown { get; set; }
        public string afterTitle { get; set; }
        public string beforeTitle { get; set; }
        public string Title { get; set; }
    }
    public class TreeChartEntitySeries
    {
        public string type { get; set; }
        public string Hieght { get; set; }
        public string Width { get; set; }
        public string name { get; set; }
        public string keys { get; set; }
        public string title { get; set; }
        public List<string[]> data { get; set; }
        public List<levels> levels { get; set; }
        public List<node> nodes { get; set; }
     

    }
    public class DataLabels
    {
        public StyleData style { get; set; }
    }
    public class StyleData
    {
        public string textDecoration { get; set; }
    }
    public class node
    {
        public string id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        //public int height { get; set; }
        //public int width { get; set; }
        //public int column { get; set; }
        public string layout { get; set; }
        public string offset { get; set; }
        public string Categoty { get; set; }
        public string Component { get; set; }
        public DataLabels dataLabels { get; set; }
    }

    public class levels
    {
        public int level { get; set; }
        public string color { get; set; }
        // public int height { get; set; }
    }
}
