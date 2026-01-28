using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class AirPressureData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["AirPressureData"] = null;
                    if (Request.QueryString["machineId"] != "")
                    {
                        var machine = Request.QueryString["machineId"].ToString();
                        lblMachine.Text = machine;
                        lblStartTime.Text = Request.QueryString["fromDate"].ToString();
                        lblEndTime.Text = Request.QueryString["toDate"].ToString();
                        BindData(machine, lblStartTime.Text, lblEndTime.Text);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindData(string machine, string startTime, string endTime)
        {
            try
            {
                List<AirPressureEntity> list = VDGDataBaseAccess.getCockpitAirPressureDetails(machine, startTime, endTime);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new AirPressureEntity());
                }
                gvAirPressureData.DataSource = list;
                gvAirPressureData.DataBind();
                if (flag == 1)
                {
                    gvAirPressureData.Rows[0].Visible = false;
                }
                Session["AirPressureData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AirPressureData"]==null)
                {
                    return;
                }

                TMPTrakGenerateReport.cockpitAirPressureReport(Session["AirPressureData"] as List<AirPressureEntity>, lblMachine.Text, lblStartTime.Text, lblEndTime.Text);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}