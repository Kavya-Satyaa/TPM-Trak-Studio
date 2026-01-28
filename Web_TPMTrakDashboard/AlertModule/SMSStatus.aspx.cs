using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class SMSStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                BindGrid();
            }
        }

        private void BindGrid()
        {
            List<SMSStatusEmtity> List = new List<SMSStatusEmtity>();
            int TotalNoMessages = 0, DistinctPhNo = 0, TotalNoofMessagesSent = 0;
            try
            {
                DateTime FromDate = Util.GetDateTime(txtFromDate.Text);
                DateTime ToDate = Util.GetDateTime(txtFromDate.Text);
                List = DataBaseAccess.GetSMSStatus(FromDate, ToDate, ref TotalNoMessages, ref DistinctPhNo, ref TotalNoofMessagesSent);
                txtDistinctPhNo.Text = DistinctPhNo.ToString();
                txtTotalNoMessages.Text = TotalNoMessages.ToString();
                txtTotalNoofMessagesSent.Text = TotalNoofMessagesSent.ToString();
                Session["SMSStatusList"] = List;
                if (List != null && List.Count > 0)
                {
                    gridmessagedetails.DataSource = List;
                    gridmessagedetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            List<SMSStatusEmtity> List = new List<SMSStatusEmtity>();
            int TotalNoMessages = 0, DistinctPhNo = 0, TotalNoofMessagesSent = 0;
            try
            {
                DateTime FromDate = Util.GetDateTime(txtFromDate.Text);
                DateTime ToDate = Util.GetDateTime(txtToDate.Text);
                List = DataBaseAccess.GetSMSStatus(FromDate, ToDate, ref TotalNoMessages, ref DistinctPhNo, ref TotalNoofMessagesSent);
                if (List != null && List.Count > 0)
                {
                    TMPTrakGenerateReport.GetSMSStatus(FromDate, ToDate, TotalNoMessages, DistinctPhNo, TotalNoofMessagesSent, List);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gridmessagedetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridmessagedetails.PageIndex = e.NewPageIndex;
            gridmessagedetails.DataBind();
        }
    }

    public class SMSStatusEmtity
    {
        public int SLNO { get; set; }
        public DateTime DateTime { get; set; }
        public string MobileNumber { get; set; }
        public int NoOfMessageSent { get; set; }
        public string Message { get; set; }
    }
}