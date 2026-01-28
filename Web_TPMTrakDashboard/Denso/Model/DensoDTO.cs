using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Denso.Model
{
    public class DensoDTO
    {
    }
    public class DailyChecklistMasterEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ChecklistID { get; set; } = string.Empty;
        public string ChecklistDesc { get; set; } = string.Empty;
        public string JudgementCriteria { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Cycle { get; set; } = string.Empty;
        public string PersonInCharge { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string FormatNumber { get; set; } = string.Empty;
        public string RevID { get; set; } = string.Empty;
        public string RevNo { get; set; } = string.Empty;
        public string RevDate { get; set; } = string.Empty;
        public string RevisedBy { get; set; } = string.Empty;
        public string ChecklistType { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }
    public class DailyChecklistTransEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ChecklistID { get; set; } = string.Empty;
        public string ChecklistDesc { get; set; } = string.Empty;
        public string JudgementCriteria { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Cycle { get; set; } = string.Empty;
        
        public string PersonInCharge { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string FormatNumber { get; set; } = string.Empty;
        public string RevID { get; set; } = string.Empty;
        public string RevNo { get; set; } = string.Empty;
        public string ChecklistType { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public string RevDate { get; set; } = string.Empty;
        public string RevisedBy { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string Year { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string InspectedBy { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string ActualValue { get; set; } = string.Empty;
        public List<DailyChecklistTransShiftDayEntity> ShiftDayList { get; set; } = new List<DailyChecklistTransShiftDayEntity>();
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
       

    }
    public class DailyChecklistTransShiftDayEntity
    {
        public string Shift { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public List<DailyChecklistTransValueEntity> ValueList { get; set; } = new List<DailyChecklistTransValueEntity>();
    }
    public class DailyChecklistTransValueEntity
    {
        public string CheckpointTypeBackColor { get; set; } = string.Empty;
        public string CheckpointTypeForeColor { get; set; } = string.Empty;
        public bool ShiftColumnVisibility { get; set; } = false;
        public string ShiftName { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool ControlEnabled { get; set; } = true;
       
        public string Day { get; set; } = string.Empty;
        public string ActualValue { get; set; } = string.Empty;
        public string CheckpointType { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
    }
    public class StaticAccuracyMasterEntity
    {
        public string SortOrder { get; set; } = string.Empty;
        public string Checkpoint { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string CheckpointType { get; set; } = string.Empty;
    }
    public class StaticAccuracyTransEntity
    {
        public string SortOrder { get; set; } = string.Empty;
        public string Checkpoint { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string CheckpointType { get; set; } = string.Empty;
        public string RowSpan { get; set; } = "1";
        public string MachineDisplay { get; set; } = string.Empty;
        public string ActualValue { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string WeekNo { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string TMSign { get; set; } = string.Empty;
        public string TLSign { get; set; } = string.Empty;
        public string HossSign { get; set; } = string.Empty;
        public List<StaticAccuracyMonthEntity> MonthList { get; set; } = new List<StaticAccuracyMonthEntity>();
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
    }
    public class StaticAccuracyMonthEntity
    {
        public string Month { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string DisplayMonth { get; set; } = string.Empty;
        public List<StaticAccuracyTransValueEntity> ValueList { get; set; } = new List<StaticAccuracyTransValueEntity>();
        
    }
    public class StaticAccuracyTransValueEntity
    {
        public string CheckpointTypeBackColor { get; set; } = string.Empty;
        public string CheckpointTypeForeColor { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string ActualValue { get; set; } = string.Empty;
        public string CheckpointType { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string WeekNumber { get; set; } = string.Empty;
        public bool ControlEnabled { get; set; } = true;
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
    }
    public class PokayOkeMasterEntity
    {
        public string MachineID { get; set; } = string.Empty;
        public string PokayOkeItem { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        public string CheckMethod { get; set; } = string.Empty;
        public string CheckInterval { get; set; } = string.Empty;
        public string RevID { get; set; } = string.Empty;
        public string RevNo { get; set; } = string.Empty;
        public string RevisedBy { get; set; } = string.Empty;
        public string CheckPointType { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }
    public class PokeyOkeTransEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string PokayOkeItem { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        public string CheckMethod { get; set; } = string.Empty;
        public string CheckInterval { get; set; } = string.Empty;
        public string RevID { get; set; } = string.Empty;
        public string RevNo { get; set; } = string.Empty;
        public string RevDate { get; set; }
        public string RevisedBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string CheckpointType { get; set; } = string.Empty;
        public string RowSpan { get; set; } = "1";
        public string RowSpanDisplay { get; set; } = "1";
        public string ActualValue { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string WeekNo { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public List<PokeyOkeWeekEntity> WeekList { get; set; } = new List<PokeyOkeWeekEntity>();
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
    }
    public class PokeyOkeWeekEntity
    {
        public string CheckpointTypeBackColor { get; set; } = string.Empty;
        public string CheckpointTypeForeColor { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string WeekNumber { get; set; } = string.Empty;
        public string ActualValue { get; set; } = string.Empty;
        public string CheckpointType { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool ControlEnabled { get; set; } = true;
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
    }

    public class FIRTransDataEntity
    {
        public string PlantID { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string DownCategory { get; set; } = string.Empty;
        public string DownID { get; set; } = string.Empty;
        public string DownDesc { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string ActionTaken { get; set; } = string.Empty;
        public string ActionTakenByWhom { get; set; } = string.Empty;
        public string ActionTakenResult { get; set; } = string.Empty;
        public string RootCause { get; set; } = string.Empty;
        public string NextActionDecided { get; set; } = string.Empty;
        public string NextActionDeciedByWhom { get; set; } = string.Empty;
        public string NextActionDeciedResult { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string AttendeesPrdEmp { get; set; } = string.Empty;
        public string AttendeesMtdEmp { get; set; } = string.Empty;
        public string AttendeesPedEmp { get; set; } = string.Empty;
        public string AttendeesQadEmp { get; set; } = string.Empty;
        public string AttendeesSqdEmp { get; set; } = string.Empty;
        public string AttendeesPcdEmp { get; set; } = string.Empty;
        public string StockStatus { get; set; } = string.Empty;
        public string StockImpact { get; set; } = string.Empty;
        public string PresentStatus { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string QtyHold { get; set; } = string.Empty;
    }
    public class FiveSChecksheetMasterEntity
    {
        public string SortOrder { get; set; } = string.Empty;
        public string Checkpoint { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string Shifts { get; set; } = string.Empty;
        public string CheckpointType { get; set; } = string.Empty;
    }
    public class FiveSChecksheetTransEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;

        public string ChecklistID { get; set; } = string.Empty;
        public string Cycle { get; set; } = string.Empty;
        //public string CycleTime { get; set; } = string.Empty;
        public string CycleTimeDisplay { get; set; } = string.Empty;
        public string CheckpointType { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string Year { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string InspectedBy { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public string ActualValue { get; set; } = string.Empty;
        public string RowSpan { get; set; } = "1";
        public string RowDisplay { get; set; } = string.Empty;
        public List<FiveSChecksheetShiftEntity> ValueList { get; set; } = new List<FiveSChecksheetShiftEntity>();
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
    }
    public class FiveSChecksheetShiftEntity
    {
        public string CheckpointTypeBackColor { get; set; } = string.Empty;
        public string CheckpointTypeForeColor { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool ControlEnabled { get; set; } = true;
        public string ActualValue { get; set; } = string.Empty;
        public string CheckpointType { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public byte[] FileInByte { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileInBase64 { get; set; } = string.Empty;
    }
}