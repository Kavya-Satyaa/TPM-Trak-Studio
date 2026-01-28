using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Cumi.Model
{
    public class DTO
    {
    }
    public class ProductionRejectionDetails
    {
        public string MachineID { get; set; }
        public string Type { get; set; }
        public string HeaderVisibility { get; set; } = "none";
        public string ContentVisibility { get; set; } = "none";
        public List<DynamicColumnDetails> DynamicColumnDetails { get; set; } = new List<DynamicColumnDetails>();
    }
    public class MachineTypeDetails
    {
        public string MachineID { get; set; }
        public int MachineIDRowSpanForExport { get; set; } = 0;
        public string Date { get; set; }
        public int DateRowSpan { get; set; }
        public int DateRowSpanForExport { get; set; }
        public string DateHeaderVisibility { get; set; } = "none";
        public string DateConentVisibility { get; set; } = "none";
        public string Shift { get; set; } = "";
        public int ShiftRowSpan { get; set; }
        public string ShiftHeaderVisibility { get; set; } = "none";
        public string ShiftConentVisibility { get; set; } = "none";
        public string PlanActualHeaderVisibility { get; set; } = "none";
        public string PlanActualConentVisibility { get; set; } = "none";
        public string LastColumnsHeaderVisibility { get; set; } = "none";
        public string Type { get; set; }
        public string HeaderVisibility { get; set; } = "none";
        public string ContentVisibility { get; set; } = "none";
        public string RowBackColor { get; set; } = "";
        public string PlanName { get; set; }
        public string ActualName { get; set; }
        public string RowCompletionName { get; set; }
        public string RowCompletionNameVisibility { get; set; } = "none";
        public int DynamicColumnCountForExport { get; set; } = 0;
        public List<DynamicColumnDetails> DynamicColumnDetails { get; set; } = new List<DynamicColumnDetails>();
    }
    public class DynamicColumnDetails
    {
        public string DynamicHeaderVisibility { get; set; } = "none";
        public string DynamicTwoRowsVisibility { get; set; } = "none";
        public string DynamicOneRowVisibility { get; set; } = "none";
        public string DynamicMergedRowVisibility { get; set; } = "none";
        //public string PlanVisibility { get; set; } = "none";
        //public string ActualVisibility { get; set; } = "none";
        //public string CompletionVisibility { get; set; } = "none";
        public string DynamicChartVisibility { get; set; } = "none";
        public string DynamicChartIconVisibility { get; set; } = "none";
        public string HeaderChartVisibility { get; set; } = "none";

        public string HeaderID { get; set; }
        public string HeaderName { get; set; }
        public string Plan { get; set; } = ".";
        public string PlanContentColor { get; set; } = "transparent";
        public string Actual { get; set; } = ".";
        public string ActualContentColor { get; set; } = "transparent";
        public string RowCompletionVisibility { get; set; } = "none";
        public string RowCompletion { get; set; } = ".";
        public string RowCompletionContentColor { get; set; } = "transparent";
        public string Completion { get; set; } = ".";
        public string CompletionContentColor { get; set; } = "transparent";
        public string Value { get; set; } = ".";
        public string ValueContentColor { get; set; } = "transparent";
    }
    public class ChartDetails
    {
        public List<PieChartDetails> PieChartDetails { get; set; } = new List<PieChartDetails>();
        public ParetoChartDetails ParetoChartDetails { get; set; } = new ParetoChartDetails();
    }
    public class PieChartDetails
    {
        public string name { get; set; }
        public Double y { get; set; }
        public bool positive { get; set; } = true;
    }
    public class ParetoChartDetails
    {
        public List<string> Categories { get; set; } = new List<string>();
        public List<Double> Values { get; set; } = new List<Double>();
    }
    public class LineChartDetails
    {
        public string title { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
        public List<LineChartSeriesDetails> LineChartSeriesDetails { get; set; } = new List<LineChartSeriesDetails>();
    }
    public class LineChartSeriesDetails
    {
        public string name { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public List<Double> data { get; set; } = new List<Double>();
    }
    public class MachineParameters
    {
        public string ParameterID { get; set; } = string.Empty;
        public string DisplayText { get; set; } = string.Empty;
    }
    public class ProcessParameterSPCDataInfo
    {
        public string MachineId { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string Kwh { get; set; } = string.Empty;
        public string DailtTonnage { get; set; } = string.Empty;
        public string SPC { get; set; } = string.Empty;
        public string AvgKwh { get; set; } = string.Empty;
        public string AvgDailyTonnage { get; set; } = string.Empty;
        public string AvgSPC { get; set; } = string.Empty;
        public List<ProcessParameterSPCDataInfo> ProcessParameterSPCData{ get; set; } = new List<ProcessParameterSPCDataInfo>();
    }
    public class ProcessParametersInfo
    {
        public string MachineId { get; set; } = string.Empty;
        public string ParameterId { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string MinValue { get; set; } = string.Empty;
        public string MaxValue { get; set; } = string.Empty;
        public string LowerValue { get; set; } = string.Empty;
        public string HigherValue { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string TemplateType { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string OneValueVisibility { get; set; } = "none";
        public string TwoValueVisibility { get; set; } = "none";
        public string LowHighVisibility { get; set; } = "none";
        public string ForeColor { get; set; } = string.Empty;
        public DateTime? updatedtimestamp { get; set; } = DateTime.Now;
        public System.Drawing.Color HeaderForeColor { get; set; }
        public List<ProcessParametersInfo> ParameterDetails { get; set; } = new List<ProcessParametersInfo>();
        public List<ProcessParametersInfo> OtherDetails { get; set; } = new List<ProcessParametersInfo>();
        public string ChartType { get; set; } = string.Empty;
    }
    public class ProcessParameterSPCInfo
    {
        public string MachineID { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string Kwh { get; set; } = string.Empty;
        public string DailyTonnage { get; set; } = string.Empty;
        public string SPC { get; set; } = string.Empty;
        public List<ProcessParameterSPCInfo> ParameterSPCInfo { get; set; } = new List<ProcessParameterSPCInfo>();
        public string AvgKwh { get; set; } = string.Empty;
        public string AvgDailyTonnage { get; set; } = string.Empty;
        public string AvgSPC { get; set; } = string.Empty;
    }
    public class MachineProcessParametersInfo
    {
        public string MachineID { get; set; } = string.Empty;
        public List<ProcessParametersInfo> ProcessParameters { get; set; } = new List<ProcessParametersInfo>();
        public List<MachineProcessParametersInfo> MachineProcessParameters { get; set; } = new List<MachineProcessParametersInfo>();
    }

    public class PlanDetails
    {
        public string MachineID { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string PONo { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IDD { get; set; } = string.Empty;
        public string PlanValue { get; set; } = string.Empty;
        public string NewOrEdit { get; set; } = string.Empty;
    }
    public class ProcessParameterMasterDetails
    {
        public string SerialNum { get; set; } = string.Empty;
        public string IDD { get; set; } = string.Empty;
        public string MachineId { get; set; } = string.Empty;
        public string ComponentId { get; set; } = string.Empty;
        public string ParameterId { get; set; } = string.Empty;
        public string ParameterRegister { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
        public string DisplayText { get; set; } = string.Empty;
        public string LowerValue { get; set; } = string.Empty;
        public string HigherValue { get; set; } = string.Empty;
        public string SourceDataType { get; set; } = string.Empty;
        public string TemplateType { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string MaxRegister { get; set; } = string.Empty;
        public string MinRegister { get; set; } = string.Empty;
        public string ChartType { get; set; } = string.Empty;
        public bool IsVisible { get; set; }
        public int SortOrder { get; set; }
        public string NewOrEdit { get; set; } = string.Empty;
    }
    public class LossReportEntity
    {
        public string Plant { get; set; } = string.Empty;
        public string DownCategory { get; set; } = string.Empty;
        public string DownCode { get; set; } = string.Empty;
        public string Machine { get; set; } = string.Empty;
        public string MachineInterfaceID { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string TotalTime { get; set; } = string.Empty;
        public string LossType { get; set; } = string.Empty;
        public string SubLoss { get; set; } = string.Empty;
        public string LossID { get; set; } = string.Empty;
        public string OperatorID { get; set; } = string.Empty;
        public string OperatorIDWithName { get; set; } = string.Empty;
        public string MntcEmpID { get; set; } = string.Empty;
        public string MntcEmpIDWithName { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string MTTR { get; set; } = string.Empty;
        public string MTBF { get; set; } = string.Empty;
        public byte[] WhyWhyDocInByte { get; set; }
        public string WhyWhyDocFileName { get; set; } = string.Empty;
        public string WhyWhyDocID { get; set; } = string.Empty;
        public string OtherDocID { get; set; } = string.Empty;
        public string WhyWhyDocIDExist { get; set; } = string.Empty;
        public string OtherDocIDExist { get; set; } = string.Empty;
        public string WhyWhyDocFileInBase64 { get; set; } = string.Empty;
        public byte[] OtherDocInByte { get; set; }
        public string OtherDocFileName { get; set; } = string.Empty;
        public string OtherDocFileInBase64 { get; set; } = string.Empty;
        public string PDT { get; set; } = string.Empty;

    }
    public class CumiPOScreen
    {
        public string AutoID { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string ProductionOrder { get; set; } = string.Empty;
        public string POQty { get; set; } = string.Empty;
        public string OperationStage { get; set; } = string.Empty;
        public string UpdatedTS { get; set; } = string.Empty;
        public string MouldWeight { get; set; } = string.Empty;
    }
    public class TargetMasterCumi
    {
        public string Month { get; set; } = string.Empty;
        public string MonthInInt { get; set; } = string.Empty;
        public string TargetValue { get; set; } = string.Empty;

    }

    public class ProcessParameterEntity
    {
        public int Slno { get; set; } = 0;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
        public double MachiningTime { get; set; } = 0.0;
        public double HydraulicPressure_Top { get; set; } = 0.0;
        public double HydraulicPressure_Bottom { get; set; } = 0.0;
        public double HydraulicTemp { get; set; } = 0.0;
        public double TopRamStroke { get; set; } = 0.0;
        public double BottomRamStroke { get; set; } = 0.0;
        public double LoadUnload { get; set; } = 0.0;

    }
    public class POStatusEntity
    {
        public string Date { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public int POQty { get; set; } = 0;
        public int ProducedQty { get; set; } = 0;
        public int BalanceQty { get; set; } = 0;
        public string MachineID { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
    }

    public class QualityRejectionEntity
    {
        public string Date { get; set; } = string.Empty;
        public string MachineId { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDesc { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string RejReason { get; set; } = string.Empty;
        public int RejQty { get; set; } = 0;
        public string DocumentID { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public double TotalWeight { get; set; } = 0.0;
    }
}