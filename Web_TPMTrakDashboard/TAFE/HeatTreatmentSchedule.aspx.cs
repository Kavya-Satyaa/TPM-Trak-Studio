using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.AlertModule;

namespace Web_TPMTrakDashboard.TAFE
{
    public partial class HeatTreatmentSchedule : System.Web.UI.Page
    {
        public DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindRatioVersionDetails();
                BindPartNumber();
                BindRatioVersionPartDetails();
            }
        }

        private void BindRatioVersionPartDetails()
        {
            try
            {
                DataTable PartAssociation_dt = DataBaseAccess.GetAllRatioVersion_PartDetails();
                if (PartAssociation_dt.Rows.Count > 0)
                {
                    gvPartAssociation.DataSource = PartAssociation_dt;
                    gvPartAssociation.DataBind();
                }
                else
                {
                    gvPartAssociation.DataSource = new DataTable();
                    gvPartAssociation.DataBind();
                    gvPartAssociation.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindRatioVersionDetails()
        {
            try
            {
                dt = DataBaseAccess.GetRatioVersionInformation();
                if (dt.Rows.Count > 0)
                {
                    grdRatioVersionMaster.DataSource = dt;
                    grdRatioVersionMaster.DataBind();
                }
                else
                {
                    grdRatioVersionMaster.DataSource = new DataTable();
                    grdRatioVersionMaster.DataBind();
                    grdRatioVersionMaster.Rows[0].Visible = false;
                }
                BindRatioInDropdown(dt);
                Session["BindRatioVersionDetails"] = dt;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindRatioInDropdown(DataTable dt)
        {
            try
            {
                List<string> RatioLst = dt.AsEnumerable().Select(x => x.Field<string>("Ratio")).Distinct().ToList();
                ddlRatio.DataSource = RatioLst;
                ddlRatio.DataBind();
                ddlRatio.SelectedIndex = 0;

                BindVersion(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindVersion(DataTable dt)
        {
            try
            {
                
                List<string> VersionLst = dt.AsEnumerable().Where(x => x.Field<string>("Ratio").Equals(ddlRatio.SelectedValue.ToString())).Select(x => x.Field<string>("Version")).Distinct().ToList();
                ddlVersion.DataSource = VersionLst;
                ddlVersion.DataBind();
                ddlVersion.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindPartNumber()
        {
            try
            {
                List<string> Partlst = BindCockpitView.GetAllComponent();
                ddlPartNo.DataSource = Partlst;
                ddlPartNo.DataBind();
                ddlPartNo.SelectedIndex = 0;
                ddlPartNo_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void txtRatio_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            string sucessfailure = string.Empty;
            string checksave = hdfgrdRatioVersionMaster.Value;
            try
            {
                if (checksave.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    string Ratio = (grdRatioVersionMaster.FooterRow.FindControl("hdfRatio") as TextBox).Text;
                    string Version = (grdRatioVersionMaster.FooterRow.FindControl("hdfVersion") as TextBox).Text;
                    hdfgrdRatioVersionMaster.Value = "";
                    DataBaseAccess.insertUpdateRatioVersionInfo(Ratio, Version, out sucessfailure);
                }
                
                if (sucessfailure == "Successfull")
                {
                    BindRatioVersionDetails();
                    lblMessages.Text = "Inserted Successfully";
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("RatioVersionAssociation page" + ex.Message);
            }
        }

        protected void btndelteRatio_Click(object sender, EventArgs e)
        {
            string sucessfailure = string.Empty;
            int Index = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
            String Ratio = ((Label)grdRatioVersionMaster.Rows[Index].FindControl("grdtxtRatio")).Text;
            String Version = ((Label)grdRatioVersionMaster.Rows[Index].FindControl("grdtxtVersion")).Text;
            try
            {
                DataBaseAccess.DeleteRatioVersionAssociation(Ratio,Version, out sucessfailure);
                if (sucessfailure.Equals("Successfull"))
                {
                    lblMessages.Text = "Deleted Succesfully!!";
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    BindRatioVersionDetails();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("delete on Ratio_version" + ex.Message);
            }
        }

        protected void ddlRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Session["BindRatioVersionDetails"] as DataTable;
                BindVersion(dt);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnPartAssociationSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sucessfailure = string.Empty;
                DataBaseAccess.insertRatioVersionPartAssociation(ddlRatio.SelectedValue.ToString(),ddlVersion.SelectedValue.ToString(),ddlPartNo.SelectedValue.ToString(), ddlPartType.SelectedValue.ToString(), out sucessfailure);
                if (sucessfailure.Equals("Successfull"))
                {
                    lblViewMsg.Text = "Inserted Succesfully";
                    lblViewMsg.ForeColor = System.Drawing.Color.Green;
                    BindRatioVersionPartDetails();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btndelteRatioPart_Click(object sender, EventArgs e)
        {
            string sucessfailure = string.Empty;
            int Index = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
            String Ratio = ((Label)gvPartAssociation.Rows[Index].FindControl("grdtxtRatio")).Text;
            String Version = ((Label)gvPartAssociation.Rows[Index].FindControl("grdtxtVersion")).Text;
            String PartNo = ((Label)gvPartAssociation.Rows[Index].FindControl("grdtxtPartNo")).Text;
            String PartType = ((Label)gvPartAssociation.Rows[Index].FindControl("grdtxtPartType")).Text;
            try
            {
                DataBaseAccess.DeleteRatioVersionPartAssociation(Ratio, Version,PartNo, PartType,out sucessfailure);
                if (sucessfailure.Equals("Successfull"))
                {
                    lblViewMsg.Text = "Deleted Succesfully!!";
                    lblViewMsg.ForeColor = System.Drawing.Color.Red;
                    BindRatioVersionPartDetails();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("delete on Ratio_version_Part" + ex.Message);
            }
        }

        protected void ddlPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<string> Partype = DataBaseAccess.GetPartTypeBasedOnPartNo(ddlPartNo.SelectedValue.ToString());
                ddlPartType.DataSource= Partype;
                ddlPartType.DataBind();
                ddlPartType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}