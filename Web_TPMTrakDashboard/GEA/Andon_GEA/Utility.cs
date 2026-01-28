using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Andon_GEA
{
    public class Utility
    {
        public static int ProductionStatusInterval = 0;
        public static int ShiftOrDaywise = 0;
        public static string PoPlantID = "PLANT-1";
        static Utility()
        {
            int.TryParse(ConfigurationManager.AppSettings["ProductionStatusRefreshInterval"].ToString(), out ProductionStatusInterval);
            int.TryParse(ConfigurationManager.AppSettings["Showshiftwisedatausingshift"].ToString(), out ShiftOrDaywise);
            PoPlantID = ConfigurationManager.AppSettings["POPlantID"].ToString();
        }
    }
}