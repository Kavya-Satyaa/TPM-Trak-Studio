using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Advik.Models
{
    public class AssemblyLineDataEntity
    {
        public string MachineID { get; set; }
        public List<MachineParameterData> ParameterDataList { get; set; }
        public AssemblyLineDataEntity()
        {
            ParameterDataList = new List<MachineParameterData>();
        }
    }

    public class MachineParameterData
    {
        public string Parameter { get; set; }
        public int SuccessValue { get; set; }
        public int FailValue { get; set; }
    }

    public class AndonPartsCountDataEntity
    {
        public string DateShift { get; set; }
        public List<AndonPartsCountData> AndonPartsCountDataList;
        public int TGT { get; set; }
        public int ACT { get; set; }
        public string Throughput { get; set; }

        public AndonPartsCountDataEntity()
        {
            AndonPartsCountDataList = new List<AndonPartsCountData>();
        }
    }

    public class AndonPartsCountData
    {
        public string PlantID { get; set; }
        public string MachineID { get; set; }
        public int TotalPartsCount { get; set; }
        public int OkPartsCount { get; set; }
        public int NotOkPartsCount { get; set; }
    }
}