using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Pitti
{
    public partial class DailyChecklistTRansaction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataGrid();
            }
        }

        private void BindDataGrid()
        {
            try
            {
                string MachineID = ddlMachineID.SelectedValue;
                DataTable dt1 = new DataTable();
                List<DailyChecklistReportEntity_Pitti> list = DataBaseAccess.GetCheckPointTransactionData_Pitti(MachineID, Convert.ToInt32(txtYear.Text.Trim()), Convert.ToInt32(txtMonth.Text.Trim()), out dt1);

                lvCheckpointReportGrid.DataSource = list;
                lvCheckpointReportGrid.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetAllMachinesFromMaster: " + ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }
    }
}