using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Pooja.Model;

namespace Web_TPMTrakDashboard.Pooja
{
    public partial class ANDONSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Image1.ImageUrl = Util.getCompanyLogoPath();
                BindSettingDetails();
            }

        }
        private void BindSettingDetails()
        {
            try
            {
                AndonSettingDetails data = PoojaDBAccess.GetANDONSettings();
                if (ddlFontName.Items.FindByValue(data.FontFamily) != null)
                {
                    ddlFontName.SelectedValue = data.FontFamily;
                }
                if (ddlTableHeaderFontSize.Items.FindByValue(data.TableHeaderFontSize) != null)
                {
                    ddlTableHeaderFontSize.SelectedValue = data.TableHeaderFontSize;
                }
                if (ddlTableContentFontSize.Items.FindByValue(data.TableContentFontSize) != null)
                {
                    ddlTableContentFontSize.SelectedValue = data.TableContentFontSize;
                }
                if (ddlScreenFlipInterval.Items.FindByValue(data.ScreenFlipInterval.ToString()) != null)
                {
                    ddlScreenFlipInterval.SelectedValue = data.ScreenFlipInterval.ToString();
                }
                txtNoOfMachineToDiaply.Text = data.NoOfMachinesToDisplay.ToString();
                txtMainHeaderName.Text = data.MainHeaderName;
                txtMachineHeaderName.Text = data.MachineHeaderName;
                txtComponentAndOpnHeaderName.Text = data.ComponentAndOpnHeaderName;
                txtOEEHeaderName.Text = data.OEEHeaderName;
                txtDownTimeHeaderName.Text = data.DownTimeHeaderName;
                txtOperatorHeaderName.Text = data.OperatorHeaderName;
                txtProductionTargetHeaderName.Text = data.ProductionTargetHeaderName;
                txtActualHeaderName.Text = data.ActualHeaderName;
                txtStatusHeaderName.Text = data.StatusHeaderName;
                cbBold.Checked = data.FontBold=="bold" ? true : false;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindSettingDetails: " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AndonSettingDetails data = new AndonSettingDetails();
                data.FontFamily = ddlFontName.SelectedValue;
                data.TableHeaderFontSize = ddlTableHeaderFontSize.SelectedValue;
                data.TableContentFontSize = ddlTableContentFontSize.SelectedValue;
                data.ScreenFlipInterval = Convert.ToInt32(ddlScreenFlipInterval.SelectedValue);
                data.NoOfMachinesToDisplay = (string.IsNullOrEmpty(txtNoOfMachineToDiaply.Text) ? 0 : Convert.ToInt32(txtNoOfMachineToDiaply.Text));
                data.MainHeaderName = txtMainHeaderName.Text;
                data.MachineHeaderName = txtMachineHeaderName.Text;
                data.ComponentAndOpnHeaderName = txtComponentAndOpnHeaderName.Text;
                data.OEEHeaderName = txtOEEHeaderName.Text;
                data.DownTimeHeaderName = txtDownTimeHeaderName.Text;
                data.OperatorHeaderName = txtOperatorHeaderName.Text;
                data.ProductionTargetHeaderName = txtProductionTargetHeaderName.Text;
                data.ActualHeaderName = txtActualHeaderName.Text;
                data.StatusHeaderName = txtStatusHeaderName.Text;
                data.FontBold = cbBold.Checked ? "bold" : "unset";
                PoojaDBAccess.SaveANDONSettings(data);
                BindSettingDetails();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "successMsg", "showSuccessMsg('Record updated Successfully.','');", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSave_Click: " + ex.Message);
            }
        }
    }
}