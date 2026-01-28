using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineStatusPage.Models
{
    public class MachineData
    {
        public string MachineID { get; set; }
        public string LastCompOpn { get; set; }
        public string LastdataArrivalTime { get; set; }
        public string LastDataArrivalTimeinMin { get; set; }
        public string LastArrivalStatus { get; set; }
        public string ConnectionTimestamp { get; set; }
        public string ConnectionStatus { get; set; }
        public string PingTimestamp { get; set; }
        public string PingStatus { get; set; }
        public string MachineStatus { get; set; }
        public string MachineLiveStatus { get; set; }
        public string MachineStatusColor { get; set; }
        public string statusImg { get; set; }
        public string strPingStatus { get; set; }
        public string strConStatus { get; set; }
    }

    public class MachineStatusData
    {
        public string MachineID { get; set; }
        public string MachineLiveStatus { get; set; }
        public string MachineStatusColor { get; set; }
    }
}