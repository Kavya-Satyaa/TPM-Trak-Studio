using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class CockpitVDGDTO
    {
    }

    public class ICockpitStyle
    {
        public string GoodRunning { get; set; }
        public string BadlyRunning { get; set; }
        public string ModeratelyRunning { get; set; }
        public string CockpitLabelBackColor { get; set; }
        public string CockpitLabelTextColor { get; set; }
        public string PEGreaterThanHundredBackColor { get; set; }
    }

    public class MachineStatusColorStyle
    {
        public string ColorDown { get; set; }
        public string ColorICD { get; set; }
        public string ColorRunning { get; set; }
        public string ColorAlarm { get; set; }
        public string ColorLoadUnload { get; set; }
        public string ColorDisconnected { get; set; }
        public string ColorPowerOff { get; set; }
        public string ColorOther { get; set; }
        public string NoData { get; set; }
    }

    public class DataCollectionEntity
    {
        public string SerialNo { get; set; }
        public string HeatCode { get; set; }
        public string WorkOrder { get; set; }
    }
    public class ModifiedDataSettings
    {
        public string ChartType { get; set; } = string.Empty;
        public string isModifiedDataRequired { get; set; } = string.Empty;
        public string ModifiedDataBackColor { get; set; } = string.Empty;
        public string EnableProductionLog { get; set; } = string.Empty;
        public string EnableDownLog { get; set; } = string.Empty;
    }
}