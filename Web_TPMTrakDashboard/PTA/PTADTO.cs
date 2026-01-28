using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.PTA
{
    public class PTADTO
    {
    }
    public class PlantMachineViewEntity
    {
        public string Machine { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string ProductionTime { get; set; } = string.Empty;
        public string LoadUnLoad { get; set; } = string.Empty;
        public string DownTime { get; set; } = string.Empty;
        public string PlannedParts { get; set; } = string.Empty;
        public string ActualParts { get; set; } = string.Empty;
        public string Delivery { get; set; } = string.Empty;
        public string NonProductionTime { get; set; } = string.Empty;
        public string MachineStoppage { get; set; } = string.Empty;
        public string TargetRevenue { get; set; } = string.Empty;
        public double ProductionEffy { get; set; } = 0;
        public double NonProductionEffy { get; set; } = 0;
        public double LoadUnloadEffy { get; set; } = 0;
        public double MachineStoppageEffy { get; set; } = 0;
        public string RowBackColor { get; set; } = string.Empty;
        public string RowForeColor { get; set; } = string.Empty;
    }
    public class MachineStoppageEntity
    {
        public string FromTime { get; set; } = string.Empty;
        public string ToTime { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
    public class OperatorEffyReportEntity
    {
        public DateTime Pdate { get; set; } = new DateTime();
        public string Shift { get; set; } = string.Empty;
        public string Machine { get; set; } = string.Empty;
        public string ProdTime { get; set; } = string.Empty;
        public string DwnTime { get; set; } = string.Empty;
        public string Others { get; set; } = string.Empty;
        public string AE { get; set; } = string.Empty;
        public string PE { get; set; } = string.Empty;
        public string QE { get; set; } = string.Empty;
        public string OE { get; set; } = string.Empty;
        public string NetUseful { get; set; } = string.Empty;
        public string Total { get; set; } = string.Empty;
        public string BlendedOEE { get; set; } = string.Empty;
        public string NetBenefit { get; set; } = string.Empty;
        public string NetLossFromBenchMark { get; set; } = string.Empty;
        public string NetLossOfOperator { get; set; } = string.Empty;
    }
    public class UnmanedReportEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string UtilisedTime { get; set; } = string.Empty;
        public string DownTime { get; set; } = string.Empty;
        public string NoOfComponents { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string BatchStart { get; set; } = string.Empty;
        public string BatchEnd { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
    }
}