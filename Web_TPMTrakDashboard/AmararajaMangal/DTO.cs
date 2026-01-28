using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.AmararajaMangal
{
    public class DTO
    {
        public class HMIEntity
        {
            public int Slno { get; set; }

            public string MachineID { get; set; }

            public string InterfaceID { get; set; }

            public string ComponentID { get; set; }

            public string Description { get; set; }

            public string DownID { get; set; }
            public string RejectionID { get; set; }

            public string DownCategory { get; set; }
            public string RejectionCategory { get; set; }
            public string CustomerId { get; set; }
            public string CustomerName { get; set; }

        }
        public class ScrapEntryData
        {
            public string MachineID { get; set; }
            public string PartID { get; set; }
            public string TotalPartProduced { get; set; }
            public string AcceptedPart { get; set; }
            public string UnitWeight { get; set; }
            public string TotalAcceptedPart { get; set; }
            public string TotalWeight { get; set; }
            public string Rejection { get; set; }
            public string RejectedParts { get; set; }
            public string DesignScrap { get; set; }
            public string LayoutScrap { get; set; }
            public string TotalScrap { get; set; }
            public string RowSpan { get; set; }
            public string RowSpanClass { get; set; }
            public string Date { get; set; }
            public string Shift { get; set; }
        }

    }
}