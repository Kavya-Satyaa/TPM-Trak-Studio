using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class DTO
    {
    }

    public class CockpitViewSettingClass
    {
        public string Parameter { get; set; }
        public string ValueInText { get; set; }
        public string ValueInText2 { get; set; }
        public int ValueInInt { get; set; }
        public bool ValueInBool { get; set; }

    }

    public class AlarmHistory
    {
        public int SLNO { get; set; }
        public string MachineID { get; set; }
        public string AlarmNo { get; set; }
        public string Message { get; set; }
        public string FirstOccurence { get; set; }
        public string LastOccurence { get; set; }
        public string NoOfOccur { get; set; }
        public string StartTime { get; set; }
        public string Endtime { get; set; }
        public string duration { get; set; }
        public string PDFPath { get; set; }
        public string PageStartNo { get; set; }
        public string PageEndNo { get; set; }
    }

    public class PPMGraphEntity
    {
        public string ComponentID { get; set; }
        public string RejectionID { get; set; }
        public string PPM { get; set; }
        public string MachineID { get; set; }
        public string OperatorID { get; set; }
        public string Operationno { get; set; }
    }

    public class AppUISettings
    {
        public string FontSize { get; set; }
        public string FontFamily { get; set; }
        public string FontStyle { get; set; }
        public string DownTime { get; set; }
        public string ShowSmileyImage { get; set; }
        public string SmileyImageSize { get; set; }
        public string ShowSmileyBlock { get; set; }
        public string SmileyBlockSize { get; set; }
        public string CockpitFontSize { get; set; }
        public string OuterCockpitFontSize { get; set; }
        public string PlantToDisplay { get; set; }
        public string AndonTitle { get; set; }
        public string DataDisplayInterval { get; set; }
        public string DefaultPredefinedTimePeriod { get; set; }
    }

    public class ColorUISetting
    {
        public string GoodColor { get; set; }
        public string ModerateColor { get; set; }
        public string BadColor { get; set; }
        public string CockPitLabelBackColor { get; set; }
        public string CockpitLabelTextColor { get; set; }
    }

    public class TableUISetting
    {
        public string BorderColorTableView { get; set; }
        public string BorderWidthTableView { get; set; }
        public string PageSizeTableView { get; set; }
        public string ScreenFlipInterval { get; set; }
        public string FormFontSize { get; set; }
        public string FontSizeInerTab { get; set; }
        public string FontSizeOuterTab { get; set; }
    }

    public class IconicUISetting
    {
        public string ScreenFlipInterval { get; set; }
        public int ShowSmileyBlock { get; set; }
        public string ShowSmileyImage { get; set; }
        public string ShowSmileyBlockSize { get; set; }
        public string SmileyImageSize { get; set; }
        public string FormFontSize { get; set; }
        public string FontSizeInerTab { get; set; }
        public string FontSizeOuterTab { get; set; }
        public string EnableImageVideo { get; set; }
        public string EnableDashBoard { get; set; }
    }

    public class ColumnViewSetting
    {
        public string Parameter { get; set; }
        public string ValueInText { get; set; }
        public string ValueInText2 { get; set; }
        public int ValueInInt { get; set; }
        public bool ValueInBool { get; set; }
        public string Display { get; set; }
        public int Setvalue { get; set; }
        public int SortOrder { get; set; }

    }
    public class ModifiedData_VDG
    {
        public string RecordType { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public decimal PartsCount { get; set; } = 0;
        public string DownID { get; set; } = string.Empty;
        public string UpdatedTS { get; set; } = string.Empty;
    }
    public class ICockpitData
    {
        public string MachineId { get; set; }
        public string MachineStatus { get; set; }
        public string MachineRemarks { get; set; }
        public List<ICockpitUserControlData> Values { get; set; }

        public ICockpitData()
        {
            Values = new List<ICockpitUserControlData>();
        }
    }
    public class ICockpitUserControlData
    {
        public string DBkey { get; set; }
        public string Value { get; set; }
        public string BackColor { get; set; }
    }

    public class SeriesStack
    {
        public int yAxis { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public List<decimal> data { get; set; }
    }

    public class SeriesStackVDG
    {
        public int yAxis { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public List<decimal?> data { get; set; }
    }
    public class SeriesStackVDGDown
    {
        public int yAxis { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public List<SeriesStackVDGDownData> data { get; set; }
    }
    //public class SeriesStackVDGDownData
    //{
    //    public decimal? y { get; set; }
    //    public string color { get; set; }
    //}
    public class SeriesStackVDGDownData
    {
        public decimal? y { get; set; }
        public decimal? x { get; set; }

    }
    public class ChartData<T>
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string YAxisTooltipValueSuffix { get; set; }
        public List<string> categories { get; set; }
        public List<T> series { get; set; }
        public List<T> seriesTimeWise { get; set; }
        public List<int> categories1 { get; set; }
    }

    public class SeriesTimeWise
    {
        public string name { get; set; }
        public List<int> data { get; set; }
    }

    public class VDGComponentValues
    {
        public string StCycleTime { get; set; }
        public string StLoadTime { get; set; }
        public string AvgCycleTime { get; set; }
        public string AvgLoadTime { get; set; }
        public string SpeedRatio { get; set; }
        public string LoadRatio { get; set; }
        public string OperationCount { get; set; }
    }
    public class VDGDataAnalysis
    {
        //Raw Data
        public string ProgramStart { get; set; }
        public string ProductionRecord { get; set; }
        public string DownRecord { get; set; }
        public string InCycleDownRecord { get; set; }

        // Auto Data
        public string ProductionRecordStart { get; set; }
        public string ProductionRecordEnded { get; set; }
        public string DownRecordStarted { get; set; }
        public string DownRecordEnded { get; set; }
    }
    public class VDGComponentStatisticValues
    {
        public string CuttingStTime { get; set; }
        public string CuttingAvgTime { get; set; }
        public string CuttingMax { get; set; }
        public string CuttingMin { get; set; }
        public string CuttingRange { get; set; }

        public string LoadUnLoadStTime { get; set; }
        public string LoadUnLoadAvgTime { get; set; }
        public string LoadUnLoadMax { get; set; }
        public string LoadUnLoadMin { get; set; }
        public string LoadUnLoadRange { get; set; }
    }

    public class SeriesBar
    {
        public int yAxis { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public List<double> data { get; set; }
        public string color { get; set; }
    }

    public class NodeMachineId
    {
        public int SortOrder { get; set; }
        public string NodeID { get; set; }
        public string NodeInterface { get; set; }
        public string MachineID { get; set; }
    }


    public class UserAccessDTO
    {
        public string Domain { get; set; }
        public string DisplayText { get; set; }
        public string Code { get; set; }
        public bool Selected { get; set; }


        public UserAccessDTO()
        {
            Selected = true;
            Domain = string.Empty;
            DisplayText = string.Empty;
            Code = string.Empty;

        }
    }
    public class componentInformation
    {
        public string Componentid { get; set; }
        public string Interfaceid { get; set; }
        public Int64 InterfaceidInInt { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        List<string> _listCustomer = new List<string>();
        public List<string> ListCustomer
        {
            get { return _listCustomer; }
            set { _listCustomer = value; }
        }

        List<componentInformation> _listComponet = new List<componentInformation>();
        public List<componentInformation> ListComponet
        {
            get { return _listComponet; }
            set { _listComponet = value; }
        }
        public string Weight { get; set; } = string.Empty;
        List<string> _listPart = new List<string>();
        public List<string> listPart
        {
            get { return _listPart; }
            set { _listPart = value; }
        }
        public string PartFamily { get; set; } = string.Empty;
    }

    public class PlantData
    {
        public string PlantID { get; set; }
        public string PlantDescription { get; set; }
        public string PlantCode { get; set; }
        public string PlantSLno { get; set; }
    }
    public class processparaconfig
    {
        public string SLNO { get; set; }
        public string MachineID { get; set; }
        public string Parameter { get; set; }
        public string Component { get; set; }
        public string LowError { get; set; }
        public string UppError { get; set; }
        public string LowOp { get; set; }
        public string UppOp { get; set; }
        public string LowWar { get; set; }
        public string UppWar { get; set; }

    }
    public class RejectionAndReworkinModel
    {
        public string Reworkid { get; set; }
        public string Rejectionid { get; set; }
        public string Description { get; set; }
        public string DescriptionInHindi { get; set; }
        public string Catagory { get; set; }
        public string SubCatagory { get; set; }
        public string Interfaceid { get; set; }

        List<string> _listCategory = new List<string>();
        public List<string> ListCategory
        {
            get { return _listCategory; }
            set { _listCategory = value; }
        }

        List<RejectionAndReworkinModel> _listRejectionRework = new List<RejectionAndReworkinModel>();
        public List<RejectionAndReworkinModel> ListRejectionRework
        {
            get { return _listRejectionRework; }
            set { _listRejectionRework = value; }
        }
        public List<string> SubCatgoryList { get; set; }
    }

    public class DTOShcheduleReport
    {
        public string Reports { get; set; }
        public string ExportType { get; set; }
        public string ExportPath { get; set; }
        public string PlantID { get; set; }
        public string GroupID { get; set; }
        public string Machine { get; set; }
        public string Operator { get; set; }
        public string ShiftId { get; set; }
        public string RunreportOn { get; set; }
        public string RunReportOnEvery { get; set; }
        public bool PlantIDAll { get; set; }
        public bool GroupIDAll { get; set; }
        public bool MachineAll { get; set; }
        public bool OperatorAll { get; set; }
        public bool ShiftIdAll { get; set; }
        public bool RunReportforEveryVisibility { get; set; }
        public bool RunReportOnVisibity { get; set; }
        public string ReportTemplate { get; set; }
        public string Subject { get; set; }

    }

    public class ToolChangefrequency
    {
        public string Tool { get; set; }
        public int NoOfChanges { get; set; }
        public string Starttime { get; set; }
        public string Endtime { get; set; }
        public string Operation { get; set; }
        public int cyclecount { get; set; }
    }

    public class MachineSubsystemDetails
    {
        public string SLNO { get; set; }
        public string MachineID { get; set; }
        public string Subsystem { get; set; }
        public string EquipmentID { get; set; }
        public string EquipmentDetails { get; set; }
        public bool rowedit { get; set; }
    }

    public class subsystem
    {
        public int categoryID { get; set; }
        public string Categorydescription { get; set; }
    }

    public class MOWISEREPORTENTITY
    {
        public string MOStartTime { get; set; }
        public string MOEndTime { get; set; }
        public string CellNo { get; set; }
        public string MachineNO { get; set; }
        public string MONumber { get; set; }
        public string ItemCode { get; set; }
        public string EmployeeName { get; set; }
        public double ActualCount { get; set; }
        public int MOQty { get; set; }
        public string MOSettingtime { get; set; }
        public string MORunningtime { get; set; }
        public string TotalCycletime { get; set; }
        public string AllowanceTime { get; set; }
        public string Actualtimeconsumedforproduction { get; set; }
        public string Difference { get; set; }
        public string Percentage { get; set; }
        public string Remarks { get; set; }
        public string Remarks1 { get; set; }
        public string Category { get; set; }
    }

    public class RejectAnalysisEntity
    {
        public string MachineID { get; set; }
        public string RejectionDesc { get; set; }
        public string ComponentID { get; set; }
        public string OperationNo { get; set; }
        public int RejQty { get; set; }
        public int RRejQty { get; set; }
        public int RMRejQty { get; set; }
        public int McRejQty { get; set; }
        public int SCoRejQty { get; set; }
        public int CoRejQty { get; set; }
        public int TotalRej { get; set; }
    }
    public class ToolSequanceData
    {
        public string MachineID { get; set; }
        public string OperationNumber { get; set; }
        public string ComponentID { get; set; }
        public string Sequence { get; set; }
        public string ToolNumber { get; set; }
        public string IdealUsage { get; set; }
        public string Offset { get; set; }
        public string ToolHolder { get; set; }
        public string RPM { get; set; }
        public string Target { get; set; }
        public string DownCode { get; set; }
        public string Notes { get; set; }
        public string ToolDescription { get; set; }

        public string ToolGPL { get; set; }
        public string DepthOfCut { get; set; }
        public string FeedMM_Min { get; set; }
        public string FeedTooth { get; set; }
        public string NoOfCuttingEdges { get; set; }
        public string NoOfCut { get; set; }

        public bool VulkanMSColumnsEnable { get; set; } = false;

    }
    public class JagdevRejectionData
    {
        public string Shift { get; set; }
        public string MachineID { get; set; }
        public string RejectionDesc { get; set; }
        public string ComponentID { get; set; }
        public string OperationNo { get; set; }
        public string Date { get; set; }
        public string Operator { get; set; }
        public decimal Accepted { get; set; }
        public decimal RejQty { get; set; }
        public decimal RejPercentage { get; set; }
    }
    public class JagdevChartData
    {
        public string Date { get; set; }
        public int Red { get; set; }
        public int Yellow { get; set; }
        public int Green { get; set; }
    }
    public class JParameters
    {
        public string ParameterId { get; set; }
        public string Date { get; set; }
        public string DivWidth { get; set; }
        public string EffLegend { get; set; }
        public string OEELegend { get; set; }
        public string DowntimeLegend { get; set; }
        public string RejLegend { get; set; }
        public string DesktopDowntimeLegend { get; set; }
        public List<JShiftDetails> JShiftDetails { get; set; } = new List<JShiftDetails>();
    }
    public class JShiftDetails
    {
        public string ShiftId { get; set; }
        public List<JCellDetails> JCellDetails { get; set; } = new List<JCellDetails>();
    }
    public class JCellDetails
    {
        public string CellId { get; set; }
        public string JMachineDetailsHeight { get; set; }
        public List<JMachineDetails> JMachineDetails { get; set; } = new List<JMachineDetails>();

    }
    public class JMachineDetails
    {
        public string MachineID { get; set; }

        public string BackColor { get; set; }
        public string MachineID1 { get; set; }
        public string BackColor1 { get; set; }
        public string ForeColor1 { get; set; }
        public string MachineID2 { get; set; }
        public string BackColor2 { get; set; }
        public string ForeColor2 { get; set; }
        public string MachineID3 { get; set; }
        public string BackColor3 { get; set; }
        public string ForeColor3 { get; set; }
        public string MachineID4 { get; set; }
        public string BackColor4 { get; set; }
        public string ForeColor4 { get; set; }
        public string MachineID5 { get; set; }
        public string BackColor5 { get; set; }
        public string ForeColor5 { get; set; }
        public string MachineID6 { get; set; }
        public string BackColor6 { get; set; }
        public string ForeColor6 { get; set; }
    }
    public class JDataset
    {
        public string ShiftId { get; set; }
        public string CellId { get; set; }
        public string MachineID { get; set; }
        public string EffBackColor { get; set; }
        public string OEEBackColor { get; set; }
        public string DowntimeBackColor { get; set; }
        public string RejectionBackColor { get; set; }
    }
    public class HolidayListDetails
    {
        public string Holiday { get; set; }
        public string Date { get; set; }
        public string Reason { get; set; }
        public string MachineID { get; set; }
        public bool CkdValue { get; set; }
    }
    public class PDTData
    {
        public string ID { get; set; }
        public string MachineID { get; set; }
        public string Reason { get; set; }
        public string FromDateTime { get; set; }
        public string ToDateTime { get; set; }
        public string FromDateTime1 { get; set; }
        public string ToDateTime1 { get; set; }
        public string ShiftName { get; set; }
        public string DownType { get; set; }
        public string Day { get; set; }

        public bool CkdValue { get; set; }
    }
    public class ToolWiseCycleTimeData
    {
        public string CycleStartTime { get; set; }
        public string CycleEndTime { get; set; }
        public string ToolNumber { get; set; }
        public string ToolTime { get; set; }
        public string OperatingTime { get; set; }
        public string CuttingTime { get; set; }
        public string RowSpan { get; set; }
        public string Visibility { get; set; }
        public string ComponentID { get; set; }
        public string SlNo { get; set; }
        public string OperationNum { get; set; }
        public TimeSpan OperatingTimeTS { get; set; }
        public TimeSpan CuttingTimeTS { get; set; }
        public TimeSpan ToolTimeTS { get; set; }
        public TimeSpan NonCuttingTimeTS { get; set; }
        public string NonCuttingTime { get; set; }
        public string ProgramBlock { get; set; }
    }
    public class TreeDataEntity
    {
        public string Catogory { get; set; }
        public string Component { get; set; }
        public string SubCatogory { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string CatogoryQty { get; set; }
        public string ComponentQty { get; set; }
        public string SubCatogoryQty { get; set; }
        public string DescriptionQty { get; set; }
        public string CodeQty { get; set; }
    }
    public class ActivityInfoEntity
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
    }
    public class DBVersionEntity
    {
        public string ScriptName { get; set; }
        public string ScriptDate { get; set; }
        public string DbVersionNumber { get; set; }
        public string SoftwareVersionNumber { get; set; }
    }
    public class UtilisedDowntimeReportEntity
    {
        public string Machine { get; set; }
        public string UtilisedTime { get; set; }
        public string DownTime { get; set; }
        public string ManagementTime { get; set; }
    }
    public class UtilisedDowntimeReportChartEntity
    {
        public List<string> Category { get; set; }
        public List<UtilisedDowntimeReportChartSeriesEntity> series { get; set; }
        public string DownTime { get; set; }
        public string ManagementTime { get; set; }
    }
    public class UtilisedDowntimeReportChartSeriesEntity
    {
        public string name { get; set; }
        public List<double> data { get; set; }
        public string color { get; set; }
    }
    public class ProductionData
    {


        public string MachineID { get; set; }
        public string Date { get; set; }
        public string Shift { get; set; }
        public double PowerOnTime { get; set; }
        public double CuttingTime { get; set; }

        public double OperationTime { get; set; }
        public string FromTime { get; set; }

        public string ToTime { get; set; }
        public string ProgramNoVal { get; set; }
        public double PartsCount { get; set; }
        public string ProgramComment { get; set; }
        public double OEE { get; set; }
        public double StdShiftTime { get; set; }

    }
    public class ProductionAnalysisForExcelSummary
    {
        public double PowerOnTime { get; set; }
        public double CuttingTime { get; set; }
        public double OperatingTime { get; set; }

        public double TotalTime { get; set; }
        public double OperatingWithoutCutting { get; set; }
        public double NonOperatingTime { get; set; }
        public double PowerOffTime { get; set; }

        public double PartsCount { get; set; }
        public double StdShiftTime { get; set; }
        // public double PowerOffTime { get; set; }  
    }
    public class ItemStdCycleTimeDetails
    {
        public string OperationNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string MachineId { get; set; }
        public string StdCycleTime { get; set; }
        public string StdMachiningTime { get; set; }
        public string StdLoadUnloadTime { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedTS { get; set; }
        public string Param { get; set; }
    }
    public class IconicTableFilterEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PlantId { get; set; }
        public string CellId { get; set; }
        public string View { get; set; }
        public string PredefinedTime { get; set; }
        public string SortOrder { get; set; }
    }
    public class RunTimeChartMachineTSData
    {
        public string MachineID { get; set; }
        public string Reason { get; set; }
        public DateTime? LastTimeStamp { get; set; } = null;
    }
    public class RunTimeChartData
    {
        public List<string> Category { get; set; } = new List<string>();
        public List<RunChartData> data { get; set; } = new List<RunChartData>();
        public List<RunTimeChartPlantLine> plotLines { get; set; }
    }
    public class RunChartMachineIndexData
    {
        public int yValue { get; set; }
        public string MachineID { get; set; }
        public string CompanyID { get; set; }
        public DateTime? LastProdStartTime { get; set; }
        public DateTime? LastDownStartTime { get; set; }
        public DateTime? LastNoDataStartTime { get; set; }
    }
    public class RunChartData
    {
        public double y { get; set; }
        public double? x { get; set; } = null;
        public double? x2 { get; set; } = null;
        public string color { get; set; }
        public string status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string borderColor { get; set; }
        public string DownReason { get; set; }
    }
    public class RunTimeChartPlantLine
    {
        public int width { get; set; }
        public string color { get; set; }
        public int value { get; set; }
        public RunTimeChartPlantLineLabel label { get; set; }
    }
    public class RunTimeChartPlantLineLabel
    {
        public bool useHTML { get; set; }
        public string text { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public RunTimeChartPlantLineLabelStyle style { get; set; }
    }
    public class RunTimeChartPlantLineLabelStyle
    {
        public int fontSize { get; set; }
        public string color { get; set; }
        public int zIndex { get; set; }
    }
    public class ToolWiseVulkan
    {
        public string slNo { get; set; }
        public string Tool { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Ideal { get; set; }
        public string Actual { get; set; }
        public string ActualToolUsage { get; set; }

    }
    public class ToolWiseChartData
    {
        public double y { get; set; }
        public string ToolName { get; set; }
    }
    public class ToolWiseChart
    {
        public List<string> Category { get; set; }
        public List<ToolWiseChartData> Data { get; set; } = new List<ToolWiseChartData>();
    }

    public class DocumentEntity
    {
        public string CompInterface { get; set; } = string.Empty;
        public string ComponentID { get; set; } = string.Empty;
        public string OpnInterface { get; set; } = string.Empty;
        public string operationno { get; set; } = string.Empty;
        public List<DocumentEntity> DrawingData { get; set; }
        public string DrawingDocFileName { get; set; } = string.Empty;
        public string DrawingFilePath { get; set; } = string.Empty;
        public List<DocumentEntity> ProgramData { get; set; }
        public string ProgramDocFileName { get; set; } = string.Empty;
        public string ProgramFilePath { get; set; } = string.Empty;
        public List<DocumentEntity> ToolsData { get; set; }
        public string ToolsDocFileName { get; set; } = string.Empty;
        public string ToolsFilePath { get; set; } = string.Empty;
        public List<DocumentEntity> FixtureData { get; set; }
        public string FixtureDocFileName { get; set; } = string.Empty;
        public string FixtureFilePath { get; set; } = string.Empty;
        public List<DocumentEntity> GaugeData { get; set; }
        public string GaugeDocFileName { get; set; } = string.Empty;
        public string GaugeFilePath { get; set; } = string.Empty;
        public List<DocumentEntity> InspectionData { get; set; }
        public string InspectionDocFileName { get; set; } = string.Empty;
        public string InspectionFilePath { get; set; } = string.Empty;

        public List<DocumentEntity> ProvenProgramData { get; set; }
        public string ProvenProgramDocFileName { get; set; } = string.Empty;
        public string ProvenProgramFilePath { get; set; } = string.Empty;
        public List<DocumentEntity> StandardProgramData { get; set; }
        public string StandardProgramDocFileName { get; set; } = string.Empty;
        public string StandardProgramFilePath { get; set; } = string.Empty;

    }

    public class NextComponentEntity
    {
        public string PlantID { get; set; }
        public string GroupID { get; set; }
        public string Machineid { get; set; }
        public string ComponentID { get; set; }
        public string ComponentDesc { get; set; }
        public string Status { get; set; }
        public string OperationNo { get; set; }
        public string Priority { get; set; }
        public bool NextComponent { get; set; }
    }

    public class DownTimeActionTakenEntity
    {
        public string DownStartTime { get; set; } = string.Empty;
        public string DownEndTime { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string ActionTaken { get; set; } = string.Empty;

    }

    public class DailyChecklistEntity_Pitti
    {
        public string SlNo { get; set; } = string.Empty;
        public string CheckPoint { get; set; } = string.Empty;
        public string Standard { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string CheckPointDesc { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }
    public class PreventiveMaintenanceChecksheet_Pitti
    {
        public string MachineID { get; set; } = string.Empty;
        public string CategoryID { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CheckpointID { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string JudgementalCriteria { get; set; } = string.Empty;
        public string ResourcesNeeded { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
    }
    public class PMReportEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CheckPointDescription { get; set; } = string.Empty;
        public string JudgementalCriteria { get; set; } = string.Empty;
        public string ResourcesNeeded { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public int ColSpan { get; set; }
        public List<PMReportMonthValues> Monthlist { get; set; } = new List<PMReportMonthValues>();
    }
    public class PMReportMonthValues
    {
        public string MonthName { get; set; } = string.Empty;
        public string ATTValue { get; set; } = string.Empty;
        public string MonthValue { get; set; } = string.Empty;
        public string ResourcesNeeded { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
    }

    public class DailyChecklistReportEntity_Pitti
    {
        public string SlNo { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string CheckPointID { get; set; } = string.Empty;
        public string CheckPointDesc { get; set; } = string.Empty;
        public string Standard { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public List<InnerLViewEntity_Pitti> InnerListViewData { get; set; } = new List<InnerLViewEntity_Pitti>();
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
    }

    public class InnerLViewEntity_Pitti
    {
        public string DateValue { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
    }

    public class EmployeeInformationModel
    {
        public string employeeid { get; set; }
        public string name { get; set; }
        public string interfaceid { get; set; }
        public string designation { get; set; }
        public string qualification { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string plantid { get; set; }
        public string role { get; set; }

    }
    public class RejectionCodesModel
    {
        public string Category { get; set; }
        public string RejectionNo { get; set; }
        public string InterfaceID { get; set; }
        public string RejectionID { get; set; }
        public string RejectionDesc { get; set; }
    }
    public class CustomerInformationModel
    {
        public string customerid { get; set; }
        public string customername { get; set; }
        public string address { get; set; }
        public string place { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pin { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string contactperson { get; set; }

    }
    public class ReworkCodesModel
    {
        public string reworkid { get; set; }
        public string reworkno { get; set; }
        public string reworkdesc { get; set; }
        public string reworkcategory { get; set; }
        public string interfaceid { get; set; }
    }
    public class WIPDashboardMachineDetails
    {
        public string MachineID { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public string OperationNo { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
    }
    public class WIPDashboardWIPDetails
    {
        public string HeatCodeNumber { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public string OperationNo { get; set; } = string.Empty;
        public string CompletedProcess { get; set; } = string.Empty;
        public string NextProcess { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }


    public class ProgramTransferModel
    {
        public string ProgNo { get; set; }
        public string MachineSideFolders { get; set; }
        public string ProgComments { get; set; }
        public string ProgModifiedTs { get; set; }
        public string FileNames { get; set; }
        public string FilePath { get; set; }
        public int ProgLength { get; set; }
        public bool IsSupportFolder { get; set; }
        public string RowColor { get; set; } = "#f5f5f5";
        public bool RowEnabled { get; set; }
    }
    public class WorkOrderTrackingData_Rexnord
    {
        public string WorkOrder { get; set; } = string.Empty;
        public string ComponentID { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
        public string OperationNo { get; set; } = string.Empty;
        public string Machine { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string ActualTime { get; set; } = string.Empty;
        public string Rejection { get; set; } = string.Empty;
        public string RejectionRemarks { get; set; } = string.Empty;
        public string RejectionBy { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public int colspan { get; set; } = 0;
        public bool ColumnVisibility { get; set; } = true;
        public bool ClassVisibility { get; set; } = false;
        public string AdditionalIconClass { get; set; } = string.Empty;
        public string trBackColor { get; set; } = string.Empty;
    }
    public class WorkOrderData_KunAero
    {
        public int SlNo { get; set; } = 1;
        public string WorkOrderID { get; set; } = string.Empty;
        public string WorkOrderFY { get; set; } = string.Empty;
        public string WorkOrderNumber { get; set; } = string.Empty;
        public string WorkOrderDate { get; set; } = string.Empty;
        public string WorkOrderQty { get; set; } = string.Empty;
    }

    public class GroupDefintion
    {
        public string GroupID { get; set; } = string.Empty;
        public string GroupDesc { get; set; } = string.Empty;
    }
    public class DailyCheckSheet_Precision
    {
        public string GroupID { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string FrequencyOrder { get; set; } = string.Empty;
        public string CheckPointID { get; set; } = string.Empty;
        public string CheckPointDesc { get; set; } = string.Empty;
        public string CheckPointDescInHindi { get; set; } = string.Empty;
    }
    public class DownIDSubLossInfoData
    {
        public string DownID { get; set; } = string.Empty;
        public string SubLossID { get; set; } = string.Empty;
        public string SubLossDescription { get; set; } = string.Empty;
        public string SubLossInterfaceID { get; set; } = string.Empty;
    }
    public class IncidentChangeEntity
    {
        public string Incident { get; set; } = string.Empty;
    }
    public class DashboardChangeEntity
    {
        public string LastDataRefershTime { get; set; }
        public List<IncidentChangeEntity> list_Program { get; set; }
        public string[] IncidentCount { get; set; } = new string[5];

        public string MachineID { get; set; }
        public string PreviousData { get; set; }
        public string ChangedData { get; set; }
        public string programID { get; set; }
        public string changedTime { get; set; }
    }

    public class VEDSchedule
    {
        public string StartDate { get; set; } 
        public string PlantID { get; set; }
        public string CellID { get; set; }
        public string ComponentID { get; set; }
        public string ComponentDesc { get; set; }
        public string OperationNumber { get; set; }
        public string PriorityNumber { get; set; }
        public string Quantity { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string Schedule { get; set; }
        public string Param { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedTS { get; set; }
        public bool PriorityEnabled { get; set; }
        public bool QtyEnabled { get; set; }

    }



}

