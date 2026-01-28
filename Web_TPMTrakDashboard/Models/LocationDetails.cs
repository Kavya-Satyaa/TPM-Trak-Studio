using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class LocationDetails
    {
        public string Name { get; set; }
        public string IconImage { get; set; }
        public string Address { get; set; }
        public string Content { get; set; }
        public string Details { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OEEString { get; set; }
        public string dbName { get; set; }
    }
}