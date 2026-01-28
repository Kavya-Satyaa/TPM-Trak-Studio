using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Advik.Andon_Advik.Model
{
    public class DTO
    {
    }
    public class PODetails
    {
        public string Machine { get; set; }
        public string Status { get; set; }
        public string Component { get; set; }
        public string Setting { get; set; }
        public string Alaram { get; set; }
        public string User { get; set; }
        public string AE { get; set; }
        public string AEBackColor { get; set; }
        public string PE { get; set; }
        public string PEBackColor { get; set; }
        public string OEE { get; set; }
        public string OEEBackColor { get; set; }
        public string Plan { get; set; }
        public string Act { get; set; }
        public string Materialtime { get; set; }
        public string Maintainancetime { get; set; }
        public string Inspectiontime { get; set; }
        public string Supervisortime { get; set; }
        public string Emoji { get; set; }
    }
    public class LeadingMachine
    {
        public string Machine1 { get; set; }
        public string Machine2 { get; set; }
        public string Machine3 { get; set; }
        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Header3 { get; set; }
        public string Item { get; set; }
        public string Turning { get; set; }
        public string Turnmill { get; set; }

        public string Machine { get; set; }
        public string OEE { get; set; }
        public string Qty { get; set; }
        public string Operator { get; set; }
        public string MaxDownTime { get; set; }
        public string MaxDown { get; set; }

        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }

    }
    public class LaggingMachine
    {
        public string Item { get; set; }
        public string IG1500 { get; set; }
        public string Turnmill02 { get; set; }
    }
    public class getCurrentShiftTime
    {
        public string Starttime { get; set; }
        public string Endtime { get; set; }
        public string Shiftname { get; set; }
        public string Shiftid { get; set; }
    }
    public class OEEData
    {
        //public string[] Category { get; set; }
        //public double[] Value { get; set; }
        private string category; private double value;

        public OEEData(string category, double value)
        {
            Category = category; Value = value;
        }

        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                if (category != value)
                {
                    category = value;
                }
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                }
            }
        }

    }
    public class ChartSeries
    {
        public string Title { get; set; }
        public string[] Category { get; set; }
        public List<DataSeries> series { get; set; }
    }
    public class DataSeries
    {
        public string type { get; set; }
        public string name { get; set; }
        public double[] data { get; set; }
        public string color { get; set; }
        public Marker marker { get; set; }
        public int yAxis { get; set; }
        public int zIndex { get; set; }
        public int baseSeries { get; set; }
    }
    public class Marker
    {
        public int radius { get; set; }
    }
    public class DownTimeParetoDATAModel
    {
        public string XValue { get; set; }
        public double YValue { get; set; }

    }
    public class Settings
    {
        public List<int> FontSize { get; set; }
        public List<string> POColumns { get; set; }

    }
    public class ImagePath
    {
        public string path { get; set; }
    }
    public class AndonSettingData
    {
        public string Column { get; set; }
        public string CustomColumn { get; set; }
        public bool Visibility { get; set; }
    }
    public class ChartDta
    {
        public ObservableCollection<OEEData> OEEData { get; set; }
        public ChartSeries DownTimeData { get; set; }
    }
}