using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard
{
    public class ShopDefaultsValues
    {
        public string CycleIgnoreThreshold { get; set; }
        public string EMweightsDisplay { get; set; }
        public int FiancialyearFrom { get; set; }
        public string JobCardSetting { get; set; }
        public string Jobcardupdate { get; set; }
        public string SmartData { get; set; }
        public string TargetForm { get; set; }
        public string TimeFormat { get; set; }
        public int MinLuForLr { get; set; }
        public int InterlockTime { get; set; }
        public int SmartTransAutoCloseTime { get; set; }
        public string SmartAgentShutDown { get; set; }
        public string SubOperation { get; set; }
        public bool QEEnabled { get; set; }

        public string CompInterfaceIDDataType { get; set; }
        public int LoadUnloadInSeconds { get; set; }
        public int LoadUnloadThreshold { get; set; }
    }
}