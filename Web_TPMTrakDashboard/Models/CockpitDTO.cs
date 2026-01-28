using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class CockpitData
    {
        public string MachineId { get; set; }
        public string MachineStatus { get; set; }
        public string MachineRemarks { get; set; }
        public string MachineOEE { get; set; }
        public string SmileyImagePath { get; set; }
        public string StatusImage { get; set; }
        public string GroupName { get; set; }
        public string BackColor { get; set; }
        public string PlantID { get; set; }
        public string AERed { get; set; } = string.Empty;
        public string AEGreen { get; set; } = string.Empty;
        public string PERed { get; set; } = string.Empty;
        public string PEGreen { get; set; } = string.Empty;
        public string QERed { get; set; } = string.Empty;
        public string QEGreen { get; set; } = string.Empty;
        public string OEERed { get; set; } = string.Empty;
        public string OEEGreen { get; set; } = string.Empty;
        public string BadColor { get; set; } = string.Empty;
        public string ModerateColor { get; set; } = string.Empty; 
        public string GoodColor { get; set; } = string.Empty;
        public string OperatorPEGreen { get; set; } = string.Empty;
        public string OperatorPERed { get; set; } = string.Empty;
        public List<CockpitUserControlData> Values { get; set; }

        public CockpitData()
        {
            Values = new List<CockpitUserControlData>();
        }
    }

   

    public class CockpitUserControlData
    {
        public string Tag { get; set; }
        public string LabelText { get; set; }
        public string LabelValue { get; set; }
        public string ColorProperties { get; set; }
        public string ForeColor { get; set; }
        public string ForeColorTitle { get; set; }
        public string BackColor { get; set; }
        public string BackColorTitle { get; set; }
        public string FontSizeInnerData { get; set; }
        public string HyperLink { get; set; }
        public string MachineName { get; set; }

        public bool RequireTooltip { get; set; }
        public string LabelValueToolTip { get; set; }
    }


    public class HourlyTrackingGraphData
    {
        public List<int> TargetFirstShift = new List<int>();
        public List<int> ActualFirstShift = new List<int>();
        public List<int> TargetSecondShift = new List<int>();
        public List<int> ActualSecondShift = new List<int>();
        public List<int> TargetThirdShift = new List<int>();
        public List<int> ActualThirdShift = new List<int>();

        //------------KWH ----------------------------//

        public List<int> KWHFirstShift = new List<int>();
        public List<int> KWHSecondShift = new List<int>();
        public List<int> KWHThirdShift = new List<int>();
    }
    public class AirPressureEntity
    {
        public string  SlNo { get; set; }
        public string MachineID { get; set; }
        public string AirPressureLow { get; set; }
        public string AirPressureRetained { get; set; }
        public string LapsedTime { get; set; }
    }
    public class SpindleRuntimeEntity
    {
        public string Date { get; set; }
        public string Machine { get; set; }
        public string RunTime { get; set; }
        public string Shift { get; set; }
    }
}