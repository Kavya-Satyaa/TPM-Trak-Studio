using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.SSWL.Model
{
    public class SSWLDTO
    {
    }
    public class ScreenMachineEntity
    {
        public string ScreenName { get; set; }
        public bool IsHeader { get; set; }
        public bool MachineSelect { get; set; }
        public string MachineName { get; set; }
        public List<ScreenMachineEntity> MachineList { get; set; }
    }
}