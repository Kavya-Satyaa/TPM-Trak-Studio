using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class EmployeeInfoModel
    {
		public string employeeid { get; set; }
		public int employeeno { get; set; }
		public string Name { get; set; }
		public string designation { get; set; }
		public string qualification { get; set; }
		public string address1 { get; set; }
		public string phone { get; set; }
		public string interfaceid { get; set; }
		public bool company_default { get; set; }
		public string Email { get; set; }
		public string EmployeeRole { get; set; }
		public List<string> PlantID { get; set; }
		public string SMSTextTemplate { get; set; }
		public List<string> CellID { get; set; }
		public string CellIDTextTemplate { get; set; }
		public bool IsActiveStatus { get; set; }
		public string Password { get; set; }
	}

    public class ShiftDataModel
    {
		public int Slno { get; set; }
        public string shiftId { get; set; }
        public string ShiftName { get; set; }
        public string FromDay { get; set; }
        public string FromTime { get; set; }
        public string ToDay { get; set; }
        public string ToTime { get; set; }
		public string ShiftType { get; set; }
    }
	public class MachineShiftTypeModel
    {
		public string Machine { get; set; }
		public string EffDate { get; set; }
		public string ShiftType { get; set; }
		public bool ISShiftTypeChecked { get; set; }
		public string HeaderVisible { get; set; }
		public string ContentVisible { get; set; }
		public string HeaderColor { get; set; }
		public string ForeHeaderColor { get; set; }
		public bool HeaderChecked { get; set; }
		public List<MachineShiftTypeModel> machineShiftslst { get; set; }

	}
}