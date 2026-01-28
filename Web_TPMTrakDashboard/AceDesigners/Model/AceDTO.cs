using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.AceDesigners.Model
{
    public class AceDTO
    {
    }
    public class ScheduleEntity
    {
        public string ID { get; set; }
        public string MachineID { get; set; }
        public string ProductionOrder { get; set; }
        public string CompID { get; set; }
        public string OpnNo { get; set; }
        public string StdCycleTime { get; set; }
        public string PlannedQty { get; set; }
        public string CompletedQty { get; set; }
        public string ScheduleDate { get; set; }
        public string SchedulePriority { get; set; }
        public string PalletNo { get; set; }
        public string Sequence { get; set; }
        public string UpdatedTsHMI { get; set; }
        public string UpdatedBy { get; set; }
        public bool chkEnabled { get; set; }
        public string Status { get; set; }
        public bool ControlEnabled { get; set; }
        public string SendToHMI { get; set; }
        public string SAPStatus { get; set; }
        public string JobType { get; set; }
        public bool ChkJobType { get; set; }
    }
    public class PrioritySequenceExistenceEntity
    {
        public bool PriorityExists { get; set; } = false;
        public bool SequenceExists { get; set; } = false;
    }
    public class ScheduleImportErrorEntity
    {
        public string MachineDesc { get; set; }
        public string ProductionOrder { get; set; }
        public string CompID { get; set; }
        public string OpnNo { get; set; }
        public string ErroMsg { get; set; }
        public string UpdatedTS { get; set; }
    }

    public class EventLogEntity
    {
        public string SlNo { get; set; } = string.Empty;
        public string MachineID { get; set; } = string.Empty;
        public string AlarmTime { get; set; } = string.Empty;
        public string EnableDisable { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public bool MachineColumnVisibility { get; set; } = false;

        public string EventID { get; set; } = string.Empty;
        public string RowSpan { get; set; } = string.Empty;
        public List<EventLogEntity> EventDetails { get; set; } = new List<EventLogEntity>();
    }
}