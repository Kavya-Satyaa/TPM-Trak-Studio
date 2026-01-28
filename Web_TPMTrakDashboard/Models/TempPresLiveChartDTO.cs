using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class TempPresLiveChartDTO
    {
        public string name { get; set; }
        public List<long[]> data { get; set; }
    }

    public class TempPresLiveChartDTORefresh
    {
        public string name { get; set; }
        public List<long[]> data { get; set; }
    }

    public class OperatingLimits
    {
        public double FromTime { get; set; }
        public double ToTime { get; set; }
        public string Colour { get; set; }
    }

    public class Header
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string MachineId { get; set; }
        public string CycleStartTime { get; set; }
        public string Param { get; set; }
        public string Parameter { get; set; }
    }

    public class PlotLines
    {
        public string color { get; set; }
        public int width { get; set; }
        public long value { get; set; }
        public int zIndex { get; set; }
        public plotLineLabelParam label { get; set; }
    }

    public class plotLineLabelParam
    {
        public string text { get; set; }
        public int rotation { get; set; }
        public string align { get; set; }
        public int y { get; set; }
        public int x { get; set; }
    }

    public class PlotBands
    {
        public double from { get; set; }
        public double to { get; set; }
        public string color { get; set; }
    }

    public class StartEndCycleTime
    {
        public string MachineId { get; set; }
        public long CycleStart { get; set; }
        public long CycleEnd { get; set; }
    }

    public class TempPresChartData
    {
        public List<long> updatedTimeList { get; set; }
        public List<StartEndCycleTime> CycleTimeList { get; set; }
        public List<TempPresLiveChartDTO> ChartDataList { get; set; }
        public List<PlotLines> plotLinesList { get; set; }
    }

    public class TempPresChartDataRefresh
    {
        public List<PlotLines> plotLinesList { get; set; }
        public List<TempPresLiveChartDTORefresh> ChartDataList { get; set; }
    }

    public class SpindleRuntimeData
    {
        public List<long[]> spRuntimeDataList { get; set; }
        public List<PlotLines> plotLinesList { get; set; }
        public string totalSpindleRuntime { get; set; }
    }
}