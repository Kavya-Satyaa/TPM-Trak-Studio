using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.LnTOdisha.Model
{
    public class LnTOdishaDTO
    {
        public class ActivityInfoLnTEntity
        {
            public string MachineID { get; set; }
            public string SerialNumber { get; set; }
            public string ActivityID { get; set; }
            public string Activity { get; set; }
            public string Frequency { get; set; }
            public string FrequencyID { get; set; }
            public bool ActivityHasFile { get; set; }
            public string FileName { get; set; }
            public string Shifts { get; set; } = string.Empty;
            public string Criteria { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string AllotedTime { get; set; } = string.Empty;
        }
        public class PMGenerationYealyEntity
        {
            public string Slno { get; set; } = string.Empty;
            public string Machine { get; set; } = string.Empty;
            public List<PMGenerationMonthEntity> MonthList { get; set; } = new List<PMGenerationMonthEntity>();
            public bool HeaderVisible { get; set; } = false;
            public bool ContentVisible { get; set; } = false;
        }
        public class PMGenerationMonthEntity
        {
            public string Year { get; set; } = string.Empty;
            public string MonthName { get; set; } = string.Empty;
            public string MonthNo { get; set; } = string.Empty;
            public string Machine { get; set; } = string.Empty;
            public string PlanStatus { get; set; } = string.Empty;
            public string PlanDate { get; set; } = string.Empty;
            public bool HeaderVisible { get; set; } = false;
            public bool ContentVisible { get; set; } = false;
            public string CssClass { get; set; } = "";
        }
        public class PMCheckilistPrintOutEntity
        {
            public string SlNo { get; set; } = string.Empty;
            public string ActivityID { get; set; } = string.Empty;

            public string Activity { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string Frequency { get; set; } = string.Empty;
            public string AllotedTime { get; set; } = string.Empty;
            public string LastCheckedRemark { get; set; } = string.Empty;
            public string TodayRemark { get; set; } = string.Empty;
            public bool CategoryVisible { get; set; } = false;
            public bool ActivityVisible { get; set; } = false;
        }
        public class PMReportLnTEntity
        {
            public string Activity { get; set; } = string.Empty;
            public string AllotedTime { get; set; } = string.Empty;
            public string Frequency { get; set; } = string.Empty;
            public string LastChecked { get; set; } = string.Empty;
            public string TodayPlan { get; set; } = string.Empty;
            public List<PMReportMonthLnTEntity> MonthData { get; set; } = new List<PMReportMonthLnTEntity>();
            public string RowSpan { get; set; } = string.Empty;
            public bool CategoryEnabled { get; set; } = false;
        }
        public class PMReportMonthLnTEntity
        {
            public string MonthValue { get; set; } = string.Empty;
        }
    }
}