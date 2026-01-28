using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.TAFE
{
    public class DTO
    {
    }
    public class PDIDetails
    {
        public string MachineID { get; set; } = "";
        public string PartNo { get; set; } = "";
        public string PartDesc { get; set; } = "";
        public string PartName { get; set; } = "";
        public string PartType { get; set; } = "";
        public string OperationNo { get; set; } = "";
        public string Material { get; set; } = "";
        public string Version { get; set; } = "";
        public string IssuedNo { get; set; } = "";
        public string DocNo { get; set; } = "";
        public string Type { get; set; } = "";
        public string Param { get; set; } = "";
        public string NewOrEdit { get; set; } = "";
        public byte[] Image { get; set; } = new byte[] { };
        public string ImageName { get; set; } = "";
        public string ImageInBase64 { get; set; } = "";
    }
}