using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.ShantiIron.Model
{
    public class DTO
    {
    }
    public class FIData
    {
        public string Parameter { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string ControlIDOrClass { get; set; }
        public string ComponentID { get; set; }
        public string Approval { get; set; }
    }
    public class FIDataDetails
    {
        public string SerialNumber { get; set; }
        public string PlantID { get; set; }
        public string MachineID { get; set; }
        public string ComponentID { get; set; }
        public string OperationNumber { get; set; }
        public string IssueFound { get; set; }
        public string Refer { get; set; }
    }
    public class FIDataList
    {
        public List<FIData> FIData { get; set; }
        public string SaveVisibility { get; set; }
        public string RejectVisibility { get; set; }
        public string ApproveVisibility { get; set; }
        public string ReworkVisibility { get; set; }
        public string Status { get; set; }
    }
    public class FIDatList
    {
        public List<FIDataList> FICheckList { get; set; }
        public List<FIDataDetails> FISlnoDetails { get; set; }
    }
    public class DownTimeData
    {
        public string AutodataID { get; set; }
        public string MachineID { get; set; }
        public string MachineInterfaceID { get; set; }
        public string DownCode { get; set; }
        public string DownInterfaceID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class CoolantTopUpData
    {
        public string PlantID { get; set; }
        public string CellID { get; set; }
        public string MachineID { get; set; }
        public string Operator { get; set; }
        public string TopUpValue { get; set; }
        public string TopUpDatetime { get; set; }
        public string Remarks { get; set; }
        public string TotalPlant { get; set; }
        public string TotalCell { get; set; }
    }
    public class SPCDetails
    {
        public string PlantID { get; set; }
        public string CellID { get; set; }
        public string ComponentID { get; set; }
        public string SerialNumber { get; set; }
    }
    public class QualityGateSlnoStatusData
    {
        public string ProcessStatus { get; set; }
        public int ProcessCount { get; set; }
        public string ProcessStartTime { get; set; }
        public string SendToCMMStatus { get; set; }
        public string QualityGateStatus { get; set; }
        public string CMMFileName { get; set; }
    }
    public class FPAData
    {
        public string MachineID { get; set; }
        public string OperationID { get; set; }
        public string PartName { get; set; }
        public string RevNo { get; set; }
        public string SerialNumber { get; set; }
        public string FPAFile { get; set; }
        public string FPAName { get; set; }
        public string LayoutFile { get; set; }
        public string LayoutName { get; set; }
        public string CMMFile { get; set; }
        public string CMMName { get; set; }
        public string CMMStatus { get; set; }
        public string SendToCMMStatus { get; set; }
        public string SerialStatus { get; set; }

    }
    public class MPIGateData
    {
        public string Component { get; set; }
        public string CompDesc { get; set; }
        public string SupplierCode { get; set; }
        public string SerialNumber { get; set; }
        public string HeatCode { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedTS { get; set; }
        public string RevisionNumber { get; set; }
        public string CompValidation { get; set; }
        public string ACKValidation { get; set; }
    }
}