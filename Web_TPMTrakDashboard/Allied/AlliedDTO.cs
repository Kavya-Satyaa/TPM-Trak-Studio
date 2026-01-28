using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Allied
{
    public class AlliedDTO
    {
    }
    public class DashBoardData
    {
        public string MachineID { get; set; }
        public string EventStartTime { get; set; }
        public string EventEndTime { get; set; }
    }
    public class DashboardChartData
    {
        public List<string> Category { get; set; } = new List<string>();
        public List<DashboardChart> data { get; set; } = new List<DashboardChart>();
    }
    public class DashboardChart
    {
        public double y { get; set; }
        public double? x { get; set; } = null;
        public double? x2 { get; set; } = null;
        public string color { get; set; }
        public string startTime { get; set; }
        public string EndTime { get; set; }
        public string borderColor { get; set; }
    }


    public class UINDashboardData
    {
        public string UinNo { get; set; } = string.Empty;
        public string ComponentID { get; set; } = string.Empty;
        public bool HeaderEnable { get; set; } = false;
        public bool ContentEnable { get; set; } = false;

        public List<UINMachineData> machineData { get; set; } = new List<UINMachineData>();
    }
    public class UINMachineData
    {
        public string OperationNo { get; set; } = string.Empty;

        public string MachineID { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public bool HeaderEnable { get; set; } = false;
        public bool ContentEnable { get; set; } = false;
    }
    public class AMMAsterData
    {
        public string MachineID { get; set; } = string.Empty;
        public string CheckpointID { get; set; } = string.Empty;
        public string CheckpointDesc { get; set; } = string.Empty;
        public string RefNo { get; set; } = string.Empty;
        public int RevID { get; set; } = 1;
        public string RevNo { get; set; } =string.Empty;
        public string RevDate { get; set; } = string.Empty;
        public string CategoryID { get; set; } = string.Empty;
        public string CategoryDesc { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public bool DuedateVisibility { get; set; } = false;
    }
    public class AMTransactionData
    {
        public string CheckpointID { get; set; } = string.Empty;
        public string CheckpointDesc { get; set; } = string.Empty;
        public string RefNo { get; set; } = string.Empty;
        public string RevID { get; set; } = string.Empty;
        public int RevNo { get; set; } = 1;
        public string RevDate { get; set; } = string.Empty;
        public string CategoryID { get; set; } = string.Empty;
        public string CategoryDesc { get; set; } = string.Empty;
        public bool HeaderVisibility { get; set; } = false;
        public bool ContentVisibility { get; set; } = false;
        public string HearderValue { get; set; } = string.Empty;
        public string ContentValue { get; set; } = string.Empty;
        public bool ApproveVisibility { get; set; } = false;
        public List<AMTransactionData> transactionData { get; set; } = new List<AMTransactionData>();
        public List<AMTransactionData> transactionHeader { get; set; } = new List<AMTransactionData>();
    }
}