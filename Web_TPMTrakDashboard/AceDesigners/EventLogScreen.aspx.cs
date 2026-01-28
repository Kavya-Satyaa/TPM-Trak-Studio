using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.AceDesigners.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AceDesigners
{
    public partial class EventLogScreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ShiftEnd = "";
                string ShiftStart = "";

                ShiftStart = DataBaseAccess.GetCurrentShiftStart(out ShiftEnd);
                txtFromDate.Text = Util.GetDateTime(ShiftStart).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDate.Text = Util.GetDateTime(ShiftEnd).ToString("dd-MM-yyyy HH:mm:ss");
                BindMachines();
                BindEventID();
                BindEventGrid();
            }
        }

        private void BindMachines()
        {
            try
            {
                List<string> list = new List<string>();
                list = AceDatabaseAccess.GetMachineID();

                lbMachineID.DataSource = list;
                lbMachineID.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindEventID()
        {
            try
            {
                List<ListItem> list = new List<ListItem>();
                //list.Add(new ListItem() { Text = "All", Value = "" });
                list.Add(new ListItem() { Text = "IN CYCLE DOWN", Value = "IN CYCLE DOWN" });
                list.Add(new ListItem() { Text = "BETWEEN CYCLE DOWN", Value = "BETWEEN CYCLE DOWN" });
                list.Add(new ListItem() { Text = "SCHEDULE BYPASS", Value = "SCHEDULE BYPASS" });

                lbEventID.DataSource = list;
                lbEventID.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindEventGrid()
        {
            List<EventLogEntity> list = new List<EventLogEntity>();
            try
            {
                string MachineID = lbMachineID.SelectedValue == "" ? "" : HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                list = AceDatabaseAccess.GetEventLogsData(txtFromDate.Text, txtToDate.Text, MachineID, lbEventID.SelectedValue.Equals("", StringComparison.OrdinalIgnoreCase) ? "" : HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbEventID));

                lvEventGrid.DataSource = list;
                lvEventGrid.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindEventGrid();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
           
            try
            {
                string MachineID = lbMachineID.SelectedValue == "" ? "" : HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                string Generated = AceGenerateReport.GenerateEventLogReport(txtFromDate.Text.Trim(), txtToDate.Text.Trim(), MachineID, lbEventID.SelectedValue.Equals("", StringComparison.OrdinalIgnoreCase) ? "" : HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbEventID));

                if(Generated.Equals("Template does not exist", StringComparison.OrdinalIgnoreCase) || (Generated.Equals("Data not Found", StringComparison.OrdinalIgnoreCase)))
                {
                    HelperClassGeneric.openWarningToastrModal(this, Generated);
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Successfully Generated.");
                }
            }
            catch (Exception ex){
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}