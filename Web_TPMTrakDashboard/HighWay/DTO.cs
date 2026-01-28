using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.HighWay
{
    public class DTO
    {
    }
    public class MachineStartupChecksheetMasterData
    {
        public string CharacteristicID { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PointsToBeChecked { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public int SortOrder { get; set; } = 1;
    }
    public class InspectionReportofShaftmasterData
    {
        public int SlNo { get; set; } = 0;
        public string CharacteristicID { get; set; } = string.Empty;
        public string BalNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Specification { get; set; } = string.Empty;
        public string InspectionMethod { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public float FrequencyQty { get; set; } = 0;
        public int RevID { get; set; } = 0;
        public string DataType { get; set; } = string.Empty;
        public string RevDate { get; set; } = string.Empty;
        public string DocNo { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }
    public class InspectionReportofShaftApprovalData
    {
        public int SlNo { get; set; } = 0;
        public string CharacteristicID { get; set; } = string.Empty;
        public string BalNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Specification { get; set; } = string.Empty;
        public string InspectionMethod { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public float FrequencyQty { get; set; } = 0;
        public string TimeValue { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool FooterVisisbility { get; set; } = false;
        public bool ButtonVisibility { get; set; } = false;
        public bool InspectorButtonVisibility { get; set; } = false;
        public bool ProdButtonVisibility { get; set; } = false;
        public bool QAButtonVisibility { get; set; } = false;
        public string DieHeatValue { get; set; } = string.Empty;
        public int ObservationsColSpan { get; set; } = 1;
        public string ProductionHOD { get; set; } = string.Empty;
        public string QAHOD { get; set; } = string.Empty;
        public string InspectedBy { get; set; } = string.Empty;
        public int InspectionColspan { get; set; } = 1;
        public int RemarksColspan { get; set; } = 1;
        public string Remarks { get; set; } = string.Empty;

        public List<InspectionReportofShaftApprovalData> listofTime { get; set; } = new List<InspectionReportofShaftApprovalData>();
    }
    public class MachineStartupApprovalData
    {
        public int SlNo { get; set; } = 0;
        public string CharacteristicID { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PointsToBeChecked { get; set; } = string.Empty;
        public string DayHeader { get; set; } = string.Empty;
        public string ShiftHeader { get; set; } = string.Empty;
        public string ShiftValue { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool SignVisibility { get; set; } = false;
        public bool ApproveVisibility { get; set; } = false;
        public string BackgroundClass { get; set; } = string.Empty;
        public string innerDayValueClass { get; set; } = string.Empty;
        public int ApprovalcolSpan { get; set; } = 5;
        public bool Charvisibility { get; set; } = true;
        public bool Shiftvisibility { get; set; } = false;
        public int Rowspan { get; set; } = 1;
        public int DateColspan { get; set; } = 1;
        public string rowclass { get; set; } = string.Empty;
        public List<MachineStartupApprovalData> dateHeader { get; set; } = new List<MachineStartupApprovalData>();
        public List<MachineStartupApprovalData> TransactionData { get; set; } = new List<MachineStartupApprovalData>();
    }
    public class AndonParameters_Highway
    {
        public string MainHeaderFontSize { get; set; } = string.Empty;
        public string SubHeaderFontSize { get; set; } = string.Empty;
        public string EfficiencyFontSize { get; set; } = string.Empty;
        public string EfficiencyHeaderFontSize { get; set; } = string.Empty;
        public string SubHeaderText { get; set; } = string.Empty;
        public int DatarefreshInterval { get; set; } = 10;
    }
    public class HeaderData
    {
        public string PlantUtilization { get; set; } = string.Empty;
        public string MachineUtilization { get; set; } = string.Empty;
        public string OperatorEfficiency { get; set; } = string.Empty;
        public string IdleTime { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
    }
    public class AndonData_Highway
    {
        public ChartSeries PlantData { get; set; } = new ChartSeries();
        public ChartSeries MachineData { get; set; } = new ChartSeries();
        public ChartSeries OperatorData { get; set; } = new ChartSeries();
    }
    public class ChartSeries
    {
        public List<string> Time { get; set; } = new List<string>();
        public List<double> Value { get; set; } = new List<double>();
    }
    public class DataForEnergy
    {
        public string TitleEnergy { get; set; }
        public string xAxisTitleEnergy { get; set; }
        public string yAxisTitleEnergy { get; set; }
        public int minValueEnergy { get; set; }
        public int maxValueEnergy { get; set; }

        public List<string> EnergyCategories { get; set; }
        public List<ChartSeries> seriesEnergy { get; set; }
    }
    public class pieData
    {
        public string name { get; set; } = string.Empty;
        public double y { get; set; } = 0;
    }
    public class DataForSummery
    {
        public string name { get; set; }
        public List<pieData> data { get; set; }
        public HeaderData headerdata { get; set; }
    }
    public class TrackingDashboardData
    {
        public string SerialNo { get; set; } = string.Empty;
        public string ComponentID { get; set; } = string.Empty;
        public bool HeaderEnable { get; set; } = false;
        public bool ContentEnable { get; set; } = false;
        public List<ComponentData> componentData { get; set; } = new List<ComponentData>();
    }
    public class ComponentData
    {
        public string OperationNo { get; set; } = string.Empty;
        public string Machine { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string ScannedDateTime { get; set; } = string.Empty;
        public string CycleEndTime { get; set; } = string.Empty;
        public bool HeaderEnable { get; set; } = false;
        public bool ContentEnable { get; set; } = false;
    }
    public class SerialNoTrackingDashboardData
    {
        public string SerialNo { get; set; } = string.Empty;
        public string ComponentID { get; set; } = string.Empty;
        public bool HeaderEnable { get; set; } = false;
        public bool ContentEnable { get; set; } = false;
        public List<SerialNoData> componentData { get; set; } = new List<SerialNoData>();
    }
    public class SerialNoData
    {
        public string OperationNo { get; set; } = string.Empty;
        public string OpnNo { get; set; } = string.Empty;
        public string OperationDesc { get; set; } = string.Empty;
        public string OperationData { get; set; } = string.Empty;
        public string OperationDataBackcolor { get; set; } = string.Empty;
        public string OperationDataBackgroundcolor { get; set; } = string.Empty;
        public bool HeaderEnable { get; set; } = false;
        public bool ContentEnable { get; set; } = false;
    }
}