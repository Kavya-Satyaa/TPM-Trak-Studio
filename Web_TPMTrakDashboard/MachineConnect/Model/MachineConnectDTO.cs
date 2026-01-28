using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.MachineConnect.Model
{
    public class MachineConnectDTO
    {
    }
    public class SpindleChartEntity
    {
        public List<double[]> Temperature { get; set; } = new List<double[]>();
        public List<double[]> SpindleSpeed { get; set; } = new List<double[]>();
        public List<double[]> SpindleLoad { get; set; } = new List<double[]>();
    }
    public class ProductionAnalyticsEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusBackColor { get; set; } = string.Empty;
        public string RunningProgram { get; set; } = string.Empty;
        public string OEE { get; set; } = string.Empty;
        public string TimeHeaderVisibility { get; set; } = string.Empty;
        public string PowerOnTime { get; set; } = string.Empty;
        public string TotalTime { get; set; } = string.Empty;
        public string OperatingTime { get; set; } = string.Empty;
        public string CuttingTime { get; set; } = string.Empty;
        public string DownTime { get; set; } = string.Empty;
        public string PartCountHeaderVisibility { get; set; } = string.Empty;
        public string Program1 { get; set; } = string.Empty;
        public string Program2 { get; set; } = string.Empty;
        public string Program3 { get; set; } = string.Empty;
        public string Program4 { get; set; } = string.Empty;
        public string TotalPartCount { get; set; } = string.Empty;
    }
    public class HourlyRunTimeChartEntity
    {
        public List<string> Date { get; set; } = new List<string>();
        public List<double> PowerOntTime { get; set; } = new List<double>();
        public List<double> CuttingTime { get; set; } = new List<double>();
        public List<double> OperatingTime { get; set; } = new List<double>();
        public double SummaryPowerOntTime { get; set; }
        public double SummaryPowerOffTime { get; set; }
        public double SummaryCuttingTime { get; set; }
        public double SummaryOperatingTime { get; set; }
        public double SummaryTotalTime { get; set; }
        public double SummaryOperatingWithoutCutting { get; set; }
        public double SummaryNonOperatingTime { get; set; }
    }
    public class PARunChartEntity
    {
        public double y { get; set; }
        public double x { get; set; }
        public double x2 { get; set; }
        public string color { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string alarmno { get; set; }
        public bool EnabledTickIneterval { get; set; } = true;
    }
    public class StoppageReasonDataEntity
    {
        public string Value { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AlarmNo { get; set; }
    }
    public class StoppageReasonEntity
    {
        public string Title { get; set; }
        public List<StoppageReasonDataEntity> stoppageReasonDatas { get; set; }
    }

    public class PartCountChartEntity
    {
        public List<string> XAxisData { get; set; } = new List<string>();
        public List<PartCountSeriesDataEntity> partCountSeriesDatas { get; set; } = new List<PartCountSeriesDataEntity>();
    }
    public class PartCountSeriesDataEntity
    {
        public string name { get; set; }
        public List<double> data { get; set; } = new List<double>();
    }
    public class WearOffsetEntity
    {
        public string Timestamp { get; set; } = string.Empty;
        public string OffsetNo { get; set; } = string.Empty;
        public string MachineMode { get; set; } = string.Empty;
        public string ProgramNo { get; set; } = string.Empty;
        public string OffsetX { get; set; } = string.Empty;
        public string OffsetZ { get; set; } = string.Empty;
        public string OffsetR { get; set; } = string.Empty;
        public string OffsetT { get; set; } = string.Empty;
    }
    public class WearOffsetChartEntity
    {
        public List<double[]> LeftChartData { get; set; } = new List<double[]>();
        public List<double[]> RightChartData { get; set; } = new List<double[]>();
        public string LeftChartTitle { get; set; } = "";
        public string RightChartTitle { get; set; } = "";
    }
    public class StoppageDataEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;

        public string Shift { get; set; } = string.Empty;
        public string FromTime { get; set; } = string.Empty;
        public string ToTime { get; set; } = string.Empty;
        public string BatchStart { get; set; } = string.Empty;
        public string BatchEnd { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public double StoppagetimeInInt { get; set; } =0;
        public string StoppageDuration { get; set; } = string.Empty;

    }

    public class ProgramUploadHistoryEntity
    {
        public string MachineId { get; set; } = string.Empty;
        public string ProgramNo { get; set; } = string.Empty;
        public string Employee { get; set; } = string.Empty;
        public string UpdatedTS { get; set; } = string.Empty;
    }
}