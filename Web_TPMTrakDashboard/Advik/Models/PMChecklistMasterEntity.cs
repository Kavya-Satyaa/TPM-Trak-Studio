using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Advik.Models
{
    public class PMChecklistMasterEntity
    {
        public string MachineID { get; set; }
        public string ChecklistID { get; set; }
        public string ChecklistItem { get; set; }
        public int NoOfCycles { get; set; }
        public bool IsEnabled { get; set; }
        public string JHType { get; set; }
        public string SortOrder { get; set; }
        public string McArea { get; set; }
        public string Location { get; set; }
        public string StdCondition { get; set; }
        public string CheckingMethod { get; set; }
    }
    public class JHDashboardDetails
    {
        public string SlNo { get; set; }
        public string Date { get; set; }
        public string AuditDate { get; set; }
        public string Shift { get; set; }
        public string Machine { get; set; }
        public string JHActivity { get; set; }
        public string JHType { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string McArea { get; set; }
        public string Location { get; set; }
        public string StdCondition { get; set; }
        public string CheckingMethod { get; set; }
        public bool SupervisorObservation { get; set; }
        public bool ProdHeadObservation { get; set; }
        public string JHActivityID { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorTS { get; set; }
        public string ProdHeadName { get; set; }
        public string ProdHeadTS { get; set; }
        public string RowSpan { get; set; }
        public string ChkBoxVisibility { get; set; }
        public string CellVisibility1 { get; set; }
        public string ChkRowSpan { get; set; }
        public string OperatorStatus { get; set; }
        public string SupervisorStatus { get; set; }
        public string BackColor { get; set; }
    }

    public class PMMasterDetails
    {
        public string PMID { get; set; }
        public string MachineID { get; set; }
        public string PMActivity { get; set; }
        public string NoOfCycle { get; set; }
        public string Status { get; set; }
        public string Updatedby { get; set; }
        public string Updatedts { get; set; }
    }

    public class HelpRequestReportDetails
    {
        public string PlantID { get; set; }
        public string MachineID { get; set; }
        public string RequestType { get; set; }
        public string ShiftDate { get; set; }
        public string ShiftName { get; set; }
        public string RequestedTime { get; set; }
        public string AckTime { get; set; }
        public string ResetTime { get; set; }
        public string AckOperatorTime { get; set; }
        public string AckTimeFromTrigger { get; set; }
        public string ResetTimeFRomTrigger { get; set; }
        public string AckOperatorTimeFromTrigger { get; set; }
        public string AvgAckTimeFromTrigger { get; set; }
        public string AvgResetTimeFRomTrigger { get; set; }
        public string AvgAckOperatorTimeFromTrigger { get; set; }
        public string MTBFValue { get; set; }
    }
    public class EmployeeDetail
    {
        public string EmployeeId { get; set; }
        public string MobNo { get; set; }
    }
    public class HelpRequestSettingDetails
    {
        public string PlantID { get; set; }
        public string SlNo { get; set; }
        public string MachineID { get; set; }
        public string RequestType { get; set; }
        public string Action { get; set; }
        public string Level1Empid { get; set; }
        public string Level2Empid { get; set; }
        public string Level2Threshold { get; set; }
        public string Level3Empid { get; set; }
        public string Level3Threshold { get; set; }
        public string Message { get; set; }
    }
    public class AudiDateDetails
    {
        public string AuditDate { get; set; }
        public string Day { get; set; }
    }
}