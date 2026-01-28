using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelClassLibrary
{
    public class Chart<T>
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string YAxisTooltipValueSuffix { get; set; }
        public bool legend { get; set; }
        public List<string> categories { get; set; }
        public List<T> series { get; set; }
        //public List<T> series1 { get; set; }
        public List<DrildownSeries> drilldown { get; set; }
    }
    public class Data
    {
        public string name { get; set; }
        public decimal y { get; set; }
        //public int y1 { get; set; } = 0;
        public string drilldown { get; set; }
        //public int turboThreshold { get; set; }
        public string afterTitel { get; set; }
        public string beforeTitle { get; set; }
    }

    public class Series
    {
        public string type { get; set; }
        public bool colorByPoint { get; set; }
        public string name { get; set; }
        public List<Data> data { get; set; }
        public int yAxis { get; set; }
        public int zIndex { get; set; }
        public int baseSeries { get; set; }
        public string previousParam { get; set; }//kkk
        public string currentParam { get; set; }//kkk
        public string nextParam { get; set; }//kkk
        public string machine { get; set; }//kkk
        public string month { get; set; }//kkk
        public string day { get; set; }//kkk
        public string btnVisible { get; set; }//kkk
        public string btnText { get; set; }//kkk
        public string color { get; set; } 
    }
    public class Data_VDG
    {
        //public string name { get; set; }
        public decimal y { get; set; }
    }

    public class Series_VDG
    {
        public string type { get; set; }
        public bool colorByPoint { get; set; }
        public string name { get; set; }
        //public List<Data_VDG> data { get; set; }
        public List<double> data { get; set; }
        public List<string> Category { get; set; }
        public List<string> drilldown { get; set; }
        public int yAxis { get; set; }
        public int zIndex { get; set; }
        public int baseSeries { get; set; }
        public string previousParam { get; set; }//kkk
        public string currentParam { get; set; }//kkk
        public string nextParam { get; set; }//kkk
        public string machine { get; set; }//kkk
        public string month { get; set; }//kkk
        public string day { get; set; }//kkk
        public string btnVisible { get; set; }//kkk
        public string btnText { get; set; }//kkk
        public string color { get; set; }
    }

    public class DrildownSeries
    {
        public string type { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public bool colorByPoint { get; set; }
        public List<DrildownData> data { get; set; }
    }

    public class DrildownData
    {
        public string name { get; set; }
        public decimal y { get; set; }
        public string drilldown { get; set; }
        public string afterTitel { get; set; }
        public string beforeTitle { get; set; }
    }

    //========================================Down Catagory==========================
    

}
