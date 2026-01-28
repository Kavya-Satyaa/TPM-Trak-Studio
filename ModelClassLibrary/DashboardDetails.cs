using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelClassLibrary
{
    public class DashboardDetails
    {
        public string MachineId { get; set; }
        public decimal OEE { get; set; }
        public decimal AE { get; set; }
        public decimal PE { get; set; }
        public decimal QE { get; set; }
        public decimal Accepted { get; set; }
        public decimal Rejected { get; set; }
        public decimal Rework { get; set; }
        public decimal Downtime { get; set; }
		public string MachineDescription { get; set; }
        public string PPM { get; set; }
        public string OEEColor { get; set; }
        public string QEColor { get; set; }
        public string AEColor { get; set; }
        public string PEColor { get; set; }
        public decimal McHrRate { get; set; }
    }

    public class UserAccessModel
    {
        public string PlantID { get; set; }
        public bool PlantAll { get; set; }
        public string UserID { get; set; }
        public bool Admin { get; set; }
        public string Password { get; set; }
        public bool SelectAll { get; set; }
        public string Domain { get; set; }
        public string DisplayText { get; set; }
        public string Code { get; set; }
        public bool Selected { get; set; }
        public string Prochecked { get; set; }
        public string ColumnType { get; set; }
        List<UserAccessModel> _listUserDataInfo = new List<UserAccessModel>();
        public List<UserAccessModel> ListUserDataInfo
        {
            get { return _listUserDataInfo; }
            set { _listUserDataInfo = value; }
        }
        public string UserName { get; set; }
    }

}
