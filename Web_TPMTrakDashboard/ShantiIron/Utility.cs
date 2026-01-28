using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public class Utility
    {
        public static int ShantiHeaderFontSize = 20;
        public static int ShantiContentFontSize = 19;
        public static int NoOfRows = 5;
        public static int ShantiFlipInterval = 30;
        static Utility()
        {
            int.TryParse(ConfigurationManager.AppSettings["ShantiNoOfRows"].ToString(), out NoOfRows);
            int.TryParse(ConfigurationManager.AppSettings["ShantiContentFontSize"].ToString(), out ShantiContentFontSize);
            int.TryParse(ConfigurationManager.AppSettings["ShantiHeaderFontSize"].ToString(), out ShantiHeaderFontSize);
            int.TryParse(ConfigurationManager.AppSettings["ShantiAndonFlipInterval"].ToString(), out ShantiFlipInterval);
        }
    }

    public class SerialNumberDashboardEntity
    {
        public string plantId { get; set; }
        public string groupid { get; set; }
        public string SerialNumber { get; set; }
        public List<OperationDetails> OperatioList { get; set; }
        public string TotalTime { get; set; }
        public string ElapsedTime { get; set; }
        public string RunTime { get; set; }
        public string ComponentID { get; set; }
    }
    public class OperationDetails
    {
        public string OperationName { get; set; }
        public string OperationNameWithDescription { get; set; }
        public string Machine { get; set; }
        public string Operator { get; set; }
        public string ComponentID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class AlarmDetails
    {
        public string MachineID { get; set; }
        public string AlarmNo { get; set; }
        public string Desciption { get; set; }
        public string AlarmStartTime { get; set; }
        public string AlarmEndTime { get; set; }
    }
    public class MeasurementDetails
    {
        public string MachineID{ get; set; }
        public string PlantID { get; set; }
        public string SerialNumber { get; set; }
        public string ComponentID { get; set; }
        public string CharacteristicID { get; set; }
        public string CharecteristicCode { get; set; }
        public string LSL { get; set; }
        public string USL { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
        public string TimeStamp { get; set; }
        public string SpecificationMean { get; set; }
        public string Result { get; set; }
        public string LeakTestRemarks { get; set; }
        public string ScannedData { get; set; }
        public string MarkingData { get; set; }
        public string MarkingStatus { get; set; }
        public string Status { get; set; }
        public string OperationName { get; set; }
        public string BackColor { get; set; }
        public string UpdatedBy { get; set; }
        public string DataType { get; set; }
        public string Remarks { get; set; }
    }
    public class InspectionDetails
    {
        public string ComponentID { get; set; }
        public string SlNo { get; set; }
        public string InspectionDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string ChekedBy { get; set; }
        public string Value { get; set; }
        public string ManualInsResult { get; set; }
        public string CameraInsResult { get; set; }
        public string CameraPicLink { get; set; }
        public string DeMagLevel { get; set; }
        public string MPIRemark { get; set; }
        public string VisualInsResult { get; set; }
        public string DimentionalStatus { get; set; }
        public string OperationName { get; set; }
        public string OperationDesc { get; set; }
    }
}