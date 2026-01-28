using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Bajaj.Model
{
    public class DTO
    {
    }

    public class JHMasterDetails
    {
        public string Plant { get; set; }
        public string Cell { get; set; }
        public string Machine { get; set; }
        public string OldMachine { get; set; }
        public string MachineInterfaceID { get; set; }
        public string Manager { get; set; }
        public string GroupLeader { get; set; }
        public string RevID { get; set; }
        public string OldRevNo { get; set; }
        public string RevNo { get; set; }
        public string RevDate { get; set; }
        public string CheckPointNo { get; set; }
        public string RouteNo { get; set; }
        public string RelatedTo { get; set; }
        public string RelatedToImagePath { get; set; }
        public string Frequency { get; set; }
        public string C_L_I_RT_Value { get; set; }
        public string Item { get; set; }
        public string CheckPoint { get; set; }
        public string Standard { get; set; }
        public string IfNotOk { get; set; }
        public string Method { get; set; }
        public string Day { get; set; }
        public string DayNo { get; set; }
        public string DayToDisplay { get; set; }
        public string Month { get; set; }
        public string DayColor { get; set; }
        public string MachineCondition { get; set; }
        public string Time { get; set; }
        public string Remarks { get; set; }
        public string MethodType { get; set; }
        public string ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string DataType { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Unit { get; set; }
        public string QuaterlyDate { get; set; }
        public string Param { get; set; }
        public string NewOrEdit { get; set; }
        public string CopyType { get; set; }
        public bool IsActionRequired { get; set; }
        public byte[] Drawing { get; set; }
        public string DrawingName { get; set; }
        public string DrawingInBase64 { get; set; }
    }

    public class JHReportDetails
    {
        public string CheckPointNo { get; set; }
        public string RouteNo { get; set; }
        public string RelatedTo { get; set; }
        public string RelatedToImagePath { get; set; }
        public string Frequency { get; set; }
        public string C_L_I_RT_Value { get; set; }
        public string Item { get; set; }
        public string CheckPoint { get; set; }
        public string Standard { get; set; }
        public string IfNotOk { get; set; }
        public string Method { get; set; }
        public string DayToDisplay { get; set; }
        public string HeaderBackGroundClass { get; set; }
        public string CheckPointsHeaderVisibility { get; set; } = "none";
        public string CheckPointsContentVisibility { get; set; } = "none";
        //public string CheckPointsVisibility { get; set; } = "none";
        public List<JHFrequencyDetails> FrequencyDetails { get; set; } = new List<JHFrequencyDetails>();
    }
    public class JHFrequencyDetails
    {
        //public string FrequencyHeaderVisisbility { get; set; } = "none";
        public string Month { get; set; }
        public string Width { get; set; }
        public int MergeColumnNo { get; set; }
        public string ValueHeaderVisisbility { get; set; } = "none";
        public string ValueContentVisisbility { get; set; } = "none";
        public string Value { get; set; }
        public string ValueContentColor { get; set; }
    }

    class ProcessParameterMasterDetails
    {
        public string SerialNum { get; set; }
        public string IDD { get; set; }
        public string MachineId { get; set; }
        public string ParameterId { get; set; }
        public string ParameterName { get; set; }
        public string DisplayText { get; set; }
        public string LowerValue { get; set; }
        public string HigherValue { get; set; }
        public string SourceType { get; set; }
        public string PollingType{ get; set; }
        public string DataReadAddress { get; set; }
        public string SourceDataType { get; set; }
        public string DBDataType { get; set; }
        public string Frequency { get; set; }
        public string TemplateType { get; set; }
        //public string Register { get; set; }
        public string Unit { get; set; }
        public string HighRedLimit { get; set; }
        public string LowRedLimit { get; set; }
        public string HighGreenLimit { get; set; }
        public string LowGreenLimit { get; set; }
        //public string HighYellowLimit { get; set; }
        //public string LowYellowLimit { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDashboardVisible { get; set; }
        public bool IsGraphVisible { get; set; }
        public bool IsMobileVisible { get; set; }
        public bool IsAlertVisible { get; set; }
        
        public int SortOrder { get; set; }
        public bool IsActionRequired { get; set; }
        public string NewOrEdit { get; set; }
    }

    public class OperatorMessageDetails
    {
        public string AlarmNo { get; set; }
        public string AlarmDate { get; set; }
        public string AlarmMessage { get; set; }
        public string GroupNo { get; set; }
    }
    public class FocasToolLifeDetails
    {
        public string MachineID { get; set; }
        public string ProgramNo { get; set; }
        public string ToolNo { get; set; }
        public string ToolDesc { get; set; }
        public string NoOfTimeChanged { get; set; }
        public string ChangeTime { get; set; }
        public string Type { get; set; }
        public string ToolTarget { get; set; }
        public string ToolActual { get; set; }
        public string RemainingToolLife { get; set; }
        public string PartsCount { get; set; }
    }


}