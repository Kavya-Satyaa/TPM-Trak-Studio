using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class PokayOkeDashboardAdvik184 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PokayOkeList"] = null;
                txtFromDate.Text = Convert.ToDateTime(Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDate.Text = Convert.ToDateTime(Web_TPMTrakDashboard.Models.VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("dd-MM-yyyy HH:mm:ss");
                BindMachineID();
                BindData();
            }
        }
        private void BindMachineID()
        {
            try
            {
                lbMachineID.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllMachines("");
                lbMachineID.DataBind();
                foreach(ListItem item in lbMachineID.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindData()
        {
            try
            {
                string machineId = "";
                foreach (ListItem item in lbMachineID.Items)
                {
                    if (item.Selected)
                    {
                        if (machineId == "")
                            machineId += "'" + item.Value + "'";
                        else
                            machineId += ",'" + item.Value + "'";
                    }
                }
                List<PokayokeMasterEntity>  list=AdvikDatabaseAccess.getPoakayOkeDashoboardData(machineId, txtFromDate.Text, txtToDate.Text);
                lvData.DataSource = list;
                lvData.DataBind();
                Session["PokayOkeList"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                List<PokayokeMasterEntity> list = new List<PokayokeMasterEntity>();
                if (Session["PokayOkeList"] == null)
                {
                    return;
                }
                list = Session["PokayOkeList"] as List<PokayokeMasterEntity>;
                if (list.Count > 0)
                {
                    bool successfull = AdvikGenerateReports.GeneratePokayOkeReport(txtFromDate.Text, txtToDate.Text, list);
                    if (!successfull)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
        }
    }
}