using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SpindleRunTimeInfoLG : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["SpindleRunTimeData"] = null;
                    if (Request.QueryString["machineId"] != "")
                    {

                        var machine = Request.QueryString["machineId"].ToString();
                        BindMachine(machine);
                        txtFromDate.Text = Util.GetDateTime(Request.QueryString["fromDate"].ToString()).ToString("dd-MM-yyyy");
                        txtToDate.Text = Util.GetDateTime(Request.QueryString["toDate"].ToString()).ToString("dd-MM-yyyy");
                        BindData(Util.GetDateTime(Request.QueryString["fromDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"), Util.GetDateTime(Request.QueryString["toDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindMachine(string machine)
        {
            try
            {
                ddlMachine.DataSource = DataBaseAccess.GetAllMachines("");
                ddlMachine.DataBind();
                if (ddlMachine.Items.Count > 0)
                {
                    ddlMachine.Items.Insert(0, "All");
                    if (ddlMachine.Items.FindByValue(machine) != null)
                    {
                        ddlMachine.SelectedValue = machine;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindData(string fromDate, string toDate)
        {
            try
            {
                if (fromDate == "" || toDate == "")
                {
                    fromDate = VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text);
                    toDate = VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text);
                }
                List<SpindleRuntimeEntity> list = VDGDataBaseAccess.getCockpitSpindleRuntimeDetails(ddlMachine.SelectedValue == "All" ? "" : ddlMachine.SelectedValue, fromDate, toDate);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new SpindleRuntimeEntity());
                }
                gvRuntimeData.DataSource = list;
                gvRuntimeData.DataBind();
                if (flag == 1)
                {
                    gvRuntimeData.Rows[0].Visible = false;
                }
                Session["SpindleRunTimeData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData("", "");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["SpindleRunTimeData"] == null)
                {
                    return;
                }

                TMPTrakGenerateReport.cockpitSpindleRuntimeReport(Session["SpindleRunTimeData"] as List<SpindleRuntimeEntity>, ddlMachine.SelectedValue, txtFromDate.Text, txtToDate.Text);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}