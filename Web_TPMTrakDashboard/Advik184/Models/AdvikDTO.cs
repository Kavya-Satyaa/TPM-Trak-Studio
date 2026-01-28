using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Advik184.Models
{
    public class AdvikDTO
    {
    }
    public class FinalInspectionEnity
    {
        public string PlantID { get; set; }
        public string MachineID { get; set; }
        public string BackColor { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string PartNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Model { get; set; }
        public string ForeColor { get; set; }
        public List<FinalInspectionEnity> StatusList { get; set; }
    }
    public class ParameterMasterEntity
    {
        public string ID { get; set; }
        public string MachineID { get; set; }
        public string ComponentID { get; set; }
        public string OperationNo { get; set; }
        public string ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string LSL { get; set; }
        public string USL { get; set; }
        public string Unit { get; set; }
        public string RegisterAddress { get; set; }
        public bool IsChildPart { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string ControlID { get; set; }
        public string BarCode { get; set; }
        public string Model { get; set; }
        public bool ControlEnabled { get; set; }
        public string MinRegAdd { get; set; }
        public string MaxRegAdd { get; set; }
    }
    public class TraceabilityEntity
    {
        public string Plant { get; set; }
        public string QRCode { get; set; }
        public string Machine { get; set; }
        public string ModelName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ElapsedTime { get; set; }
        public string Result { get; set; }
        public string RowSpan { get; set; }
        public string Value { get; set; }
    }
    public class ModelMasterEntity
    {
        public string ID { get; set; }
        public string ModelID { get; set; }
        public string ModelName { get; set; }
    }
    public class PokayokeMasterEntity
    {
        public string ID { get; set; }
        public string MachineID { get; set; }
        public string ComponentID { get; set; }
        public string OperationNo { get; set; }
        public string PokayokeID { get; set; }
        public string PokayokeName { get; set; }
        public string RegisterID { get; set; }
        public string Result { get; set; }
        public string UpdatedTS { get; set; }
    }
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
}