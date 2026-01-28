using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Denso.Model;

namespace Web_TPMTrakDashboard.Denso
{
    public partial class FIROverviewDataDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindMachine();
                BindDownCategory();
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                lbMachineID.DataSource = list;
                lbMachineID.DataBind();
                foreach(ListItem item in lbMachineID.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindDownCategory()
        {
            try
            {
                List<string> list = DensoDBAccess.getDownCategoryData();
                lbDownCategory.DataSource = list;
                lbDownCategory.DataBind();
                foreach(ListItem item in lbDownCategory.Items)
                {
                    item.Selected = true;
                }
                BindDownID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindDownID()
        {
            try
            {
                List<string> list = DensoDBAccess.getDownIDByCategory(HelperClassGeneric.getListBoxValueWithCommaSeparator(lbDownCategory));
                lbDownID.DataSource = list;
                lbDownID.DataBind();
                foreach (ListItem item in lbDownID.Items)
                {
                    item.Selected = true;
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
                List<FIRTransDataEntity> list = DensoDBAccess.getFIRGridDetails(HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID), HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbDownCategory), HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbDownID), txtFromDate.Text, txtToDate.Text);
                gvFIRData.DataSource = list;
                gvFIRData.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void lbDownCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDownID();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "openModal", "openMultiselectPopup();", true);
        }
    }
}