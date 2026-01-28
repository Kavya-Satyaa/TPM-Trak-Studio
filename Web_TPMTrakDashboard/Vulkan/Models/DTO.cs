using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Vulkan.Models
{
    public class DTO
    {
    }
    public class MachineAndonData
    {
        public string Machineid { get; set; }
        public string Part { get; set; }
        public string BatchNo { get; set; }
        public string ExpectedProd { get; set; }
        public string ActualProd { get; set; }
        public string OEE { get; set; }
        public string DownTime { get; set; }
        public string OEEFontColor { get; set; }
        public string OEEBackColor { get; set; }
    }
    public class CLVulkanEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string CheckPoint { get; set; } = string.Empty;
        public string Requirement { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Instrument { get; set; } = string.Empty;
        public string ActionPlan { get; set; } = string.Empty;
        public string Observation { get; set; } = string.Empty;
        public string ReferenceImageName { get; set; } = string.Empty;
        public byte[] ReferenceImageData { get; set; }
        public bool IsImageRequired { get; set; } = false;
        public string ChecklistType { get; set; } = string.Empty;
    }
    public class PMMasterData
    {
        public string SerialNo { get; set; } = string.Empty;
        public string CheckPoint { get; set; } = string.Empty;
        public string Requirement { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Instrument { get; set; } = string.Empty;
        public string Observation { get; set; } = string.Empty;
    }

    public class MetInsEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string imgUrl { get; set; } = string.Empty;
        public string imgName { get; set; } = string.Empty;
        public string RefID { get; set; } = string.Empty;
        public byte[] imgInByte { get; set; }
    }
    public class DailyCLTransactionEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string CheckpointDesc { get; set; } = string.Empty;
        public string Requirement { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Instrument { get; set; } = string.Empty;
        public string ActionPlan { get; set; } = string.Empty;

        public string DateValue { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool SignRowVisibility { get; set; } = false;
        public int DateColSpan { get; set; } = 0;
        public int ColSpanContent { get; set; } = 0;
        public int rowSpanHoliday { get; set; } = 0;
        public List<DateColumnsEntity> DateColumns { get; set; } = new List<DateColumnsEntity>();

        public List<MetInsEntity> MethodList { get; set; } = new List<MetInsEntity>();
        public List<MetInsEntity> InstrumentList { get; set; } = new List<MetInsEntity>();
        public bool HolidayColumn { get; set; } = false;
    }
    public class DateColumnsEntity
    {
        public string BackgroundClass { get; set; } = string.Empty;

        public string DayValue { get; set; } = string.Empty;
        public string DailyCLValue { get; set; } = string.Empty;
        public string DailyCLValueClass { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool DayValueVisibility { get; set; } = false;
        public bool ApproveVisibility { get; set; } = false;
        public bool ApprovedEnabled { get; set; } = false;
    }

    public class DailyCLTransactionEntity_Main
    {
        public List<DailyCLTransactionEntity> list1 { get; set; }
        public List<DailyCLTransactionEntity> list2 { get; set; }
    }
    public class PlanningSheetData
    {
        public int SlNo { get; set; } = 0;
        public string MachineID { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public List<innerlistview_PlanningSheetData> InnerListViewData { get; set; } = new List<innerlistview_PlanningSheetData>();
    }
    public class innerlistview_PlanningSheetData
    {
        public string DateValue { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string DateColumn { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string colspan { get; set; } = string.Empty;
    }
    public class PostPoneGridData
    {
        public string MachineID { get; set; } = string.Empty;
        public string ActivityDate { get; set; } = string.Empty;
        public bool isChecked { get; set; } = false;
    }
    public class PMReportData
    {
        public int SlNo { get; set; } = 0;
        public string MachineID { get; set; } = string.Empty;
        public string CheckPoint { get; set; } = string.Empty;
        public string Requirement { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        //public string MethodimgUrl { get; set; } = string.Empty;
        //public string InstrumentimgUrl { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string DocNo { get; set; } = string.Empty;
        public string IssueDate { get; set; } = string.Empty;
        public string RevNo { get; set; } = string.Empty;
        public string RevDate { get; set; } = string.Empty;
        public List<MetInsEntity> MethodList { get; set; } = new List<MetInsEntity>();
        public List<MetInsEntity> InstrumentList { get; set; } = new List<MetInsEntity>();
        public bool HolidayColumn { get; set; } = false;
        public List<PMReportInnerData> FrequencyWiseValues { get; set; } = new List<PMReportInnerData>();
    }
    public class PMReportInnerData
    {
        public string Day { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string BackgroundClass { get; set; } = string.Empty;
        public string DailyCLValue { get; set; } = string.Empty;
        public string CssClass { get; set; } = string.Empty;
    }
}