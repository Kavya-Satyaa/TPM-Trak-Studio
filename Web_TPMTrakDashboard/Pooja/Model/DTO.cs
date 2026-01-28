using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Pooja.Model
{
    public class DTO
    {
    }
    public class PoojaParameterMasterDetails
    {
        public string ParameterID { get; set; }
        public string ParameterDesc { get; set; }
        public int SortOrder { get; set; }
        public bool IsEnabled { get; set; }
    }
    public class PoojaAndonMasterDetails
    {
        public string Parameter { get; set; }
        public string GradeID { get; set; }
        public string CompareType { get; set; }
        public string LSL { get; set; }
        public string USL { get; set; }
        public string Action { get; set; }

    }
    public class ProductionDetails
    {
        public string Machine { get; set; }
        public string Component { get; set; }
        public string Operation { get; set; }
        public string Downtime { get; set; }
        public string Operator { get; set; }
        public string ProductionTarget { get; set; }
        public string OEE { get; set; }
        public string Actual { get; set; }
        public string StatusColor { get; set; }
        public string HeaderVisibility { get; set; } = "none";
        public string ContentVisibility { get; set; } = "none";
    }
    public class AndonSettingDetails
    {
        public string FontFamily { get; set; } = "Arial";
        public string TableHeaderFontSize { get; set; } = "16";
        public string TableContentFontSize { get; set; } = "14";
        public int ScreenFlipInterval { get; set; } = 10;
        public int NoOfMachinesToDisplay { get; set; } = 0;
        public string FontBold { get; set; } = "unset";
        public string MainHeaderName { get; set; } = "Production Status";
        public string MachineHeaderName { get; set; } = "M/C No.";
        public string ComponentAndOpnHeaderName { get; set; } = "Running Component & Opn";
        public string OEEHeaderName { get; set; } = "OEE (%)";
        public string DownTimeHeaderName { get; set; } = "DownTime (HH:MM:SS)";
        public string OperatorHeaderName { get; set; } = "Operator";
        public string ProductionTargetHeaderName { get; set; } = "Production Target";
        public string ActualHeaderName { get; set; } = "Actual";
        public string StatusHeaderName { get; set; } = "Status";

    }

    public class GradeMasterEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string GradeID { get; set; } = string.Empty;
    }
}