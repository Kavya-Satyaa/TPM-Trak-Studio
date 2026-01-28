using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public class DTOPradeepMetals
    {
    }
    public class PMSceduleScreenEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string JobNumber { get; set; } = string.Empty;
        public string PlannedQuantity { get; set; } = string.Empty;
        public string LotCode { get; set; } = string.Empty;
        public string OperationNo { get; set; } = string.Empty;
        public string ScheduleDate { get; set; } = string.Empty;
        public string ScheduleStatus { get; set; } = string.Empty;
        public string ActualQty { get; set; } = string.Empty;
        public string HmiUpdatedTs { get; set; } = string.Empty;
        public bool IsDelete { get; set; } = true;
        public bool IsReadOnly { get; set; } = true;
        public string SendToHMIStatus { get; set; } = "2";
        public string RevNo { get; set; } = string.Empty;
    }
    public class CellSupervisorScreen
    {
        public string CellID { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string Supervisor { get; set; } = string.Empty;
    }
    public class AndonEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string GroupID { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public string AE { get; set; } = string.Empty;
        public string PE { get; set; } = string.Empty;
        public string QE { get; set; } = string.Empty;
        public string OEE { get; set; } = string.Empty;
        public string AEColor { get; set; } = string.Empty;
        public string PEColor { get; set; } = string.Empty;
        public string QEColor { get; set; } = string.Empty;
        public string OEEColor { get; set; } = string.Empty;
        public string DownTimeInMin { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string ProductionTarget { get; set; } = string.Empty;
        public string ActualProductionCount { get; set; } = string.Empty;
        public string CumulativeProductionTarget { get; set; } = string.Empty;
        public string CumulativeActualProdCount { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    public class AndonGeneralSettingsEntity
    {
        public string AndonTitle { get; set; } = string.Empty;
        public string FontFamily { get; set; } = string.Empty;
        public string FontStyle { get; set; } = string.Empty;
        public string DataRefreshInterval { get; set; } = string.Empty;
        public string ScreenFlipInterval { get; set; } = string.Empty;
        public string FooterEnabled { get; set; } = string.Empty;
        public string MsgEnabled { get; set; } = string.Empty;
        public string ScrollingText { get; set; } = string.Empty;
        public string DateFormat { get; set; } = string.Empty;
        public string TimeFormat { get; set; } = string.Empty;
        public int CurvedBoxes { get; set; } = 0;
    }
    public class AndonSettingsEntity
    {
        public string ColumnName { get; set; } = string.Empty;
        public string DisplayText { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public int Visibility { get; set; } = 0;
        public string TextAlign { get; set; } = string.Empty;
        public string LabelFontSize { get; set; } = string.Empty;
        public string DataFontSize { get; set; } = string.Empty;
    }
    public class AllAndonProductionEntity
    {
        public string AutoGenerateID { get; set; } = string.Empty;
        //public List<AndonEntity> ProductionData { get; set; }
        public DataTable ProductionData { get; set; }
        public List<AndonSettingsEntity> HeaderData { get; set; } = new List<AndonSettingsEntity>();
        public AndonGeneralSettingsEntity AndonSettings { get; set; }
    }

}