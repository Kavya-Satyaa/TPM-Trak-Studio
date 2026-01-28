using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class LineGroupModel
    {
        public string MachineId { get; set; }
        public int EndOfGroupMachine { get; set; }
        public int EndOfLineMachine { get; set; }
        public int MachineSequence { get; set; }
        public bool chkAssignMachine { get; set; }

        public string endOfMachineVal { get; set; }
        public string selectedLine { get; set; }
        public string selectedGroupId { get; set; }
        public string groupOrder { get; set; }

        List<LineGroupModel> _listCellDefination = new List<LineGroupModel>();
        public List<LineGroupModel> ListCellDefination
        {
            get { return _listCellDefination; }
            set { _listCellDefination = value; }
        }
    }
}