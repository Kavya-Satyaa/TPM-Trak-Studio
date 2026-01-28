using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class ReceiptCompletionTargetMasterGEA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtYear.Text = DateTime.Now.Year.ToString();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindData()
        {
            try
            {
                List<ReceiptCompletionTargetEntity> list = GEADatabaseAccess.getReceiptCompletionTargetMasterDeatils(txtYear.Text);
                gvRCTarget.DataSource = list;
                gvRCTarget.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                foreach (GridViewRow row in gvRCTarget.Rows)
                {
                    ReceiptCompletionTargetEntity data = new ReceiptCompletionTargetEntity();
                    data.Year = txtYear.Text;
                    data.WeekNo = (row.FindControl("lblWeehNo") as Label).Text;
                    data.Target = (row.FindControl("txtTarget") as TextBox).Text;
                    result += GEADatabaseAccess.saveReceiptCompletionTargetMasterDeatils(data);
                }
                if (result > 0)
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}