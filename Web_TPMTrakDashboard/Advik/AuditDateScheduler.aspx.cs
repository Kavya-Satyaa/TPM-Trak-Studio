using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class AuditDateScheduler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                bindAuditDateDetails();
            }
        }
        private void bindAuditDateDetails()
        {
            try
            {
                string year = "";
                if (txtYear.Text != "")
                {
                    if (txtYear.Text.Length != 4)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Year is not in correct format.";
                        return;
                    }
                    year = txtYear.Text + "-01-01 00:00:00";
                }

                List<AudiDateDetails> listAuditDetails = AdvikDatabaseAccess.getAuditDateDetails(year);
                gvAduitDateDetails.DataSource = listAuditDetails;
                gvAduitDateDetails.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        protected void btnGenerateAuditDate_Click(object sender, EventArgs e)
        {
            try
            {
                string year = "";
                if (txtYear.Text != "")
                {
                    if (txtYear.Text.Length != 4)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Year is not in correct format.";
                        return;
                    }
                    year = txtYear.Text + "-01-01 00:00:00";
                }
                string success = AdvikDatabaseAccess.generateAuditDates(year);

                if (success == "Not Inserted")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "confirmmsg", "openConfirmModal()", true);
                    return;

                }
                else
                if (success == "Error")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to generate Audit Date";
                    return;

                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "Audit Date generated successfully.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }

                bindAuditDateDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            bindAuditDateDetails();
        }

        protected void btnRegenerateAuditDate_Click(object sender, EventArgs e)
        {
            try
            {
                string year = "";
                if (txtYear.Text != "")
                {
                    if (txtYear.Text.Length != 4)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Year is not in correct format.";
                        return;
                    }
                    year = txtYear.Text + "-01-01 00:00:00";
                }
                int success = AdvikDatabaseAccess.reGenerateAuditDates(year);
                if (success <=0)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to generate Audit Date";
                    return;

                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "Audit Date generated successfully.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }

                bindAuditDateDetails();

            }
            catch (Exception ex)
            {

            }
        }
    }
  
}