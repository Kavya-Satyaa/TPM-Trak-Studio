using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public partial class AndonSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null || Session["connectionString"] == null)
                    Response.Redirect("../SignIn.aspx", false);

                //AndonEfficiencyColorEntity colorEntity = DBAccessPradeepMetals.GetEfficiencyColorValues();
                //setColorValues(colorEntity);
                BindGenericSettings();
                BindData();
            }
        }
        
        //internal void setColorValues(AndonEfficiencyColorEntity colorEntity)
        //{
        //    try
        //    {
        //        txtGood.Text = colorEntity.GoodColor.Substring(3);
        //        txtModerate.Text = colorEntity.ModerateColor.Substring(3);
        //        txtBad.Text = colorEntity.BadColor.Substring(3);
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.WriteErrorLog("setColorValues:" + ex.ToString());
        //    }
        //}

        internal void BindData()
        {
            List<AndonSettingsEntity> list = new List<AndonSettingsEntity>();
            try
            {
                list = DBAccessPradeepMetals.GetAndonSettings();

                //txtMachineFontSize.Text = list.Where(x => x.ColumnName.Equals("MachineID", StringComparison.OrdinalIgnoreCase)).Select(x => x.DataFontSize).FirstOrDefault();
                //list = list.Where(x => x.ColumnName != "MachineID").ToList<AndonSettingsEntity>();

                lvParameterDetails.DataSource = list;
                lvParameterDetails.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            bool success = false;
            try
            {
                //string MachineFontSize = txtMachineFontSize.Text.Trim();
                //DBAccessPradeepMetals.UpdateAndonViewSettings("AndonViewColumn", "MachineFontSize", "", 0, 0, "", MachineFontSize,"");
                foreach (ListViewDataItem item in lvParameterDetails.Items)
                {
                    string ColumnName = (item.FindControl("txtColumnName") as Label).Text;
                    string DisplayText = (item.FindControl("txtDisplayText") as TextBox).Text.Trim();
                    int sortOrder = Convert.ToInt32((item.FindControl("txtSortOrder") as TextBox).Text.Trim());
                    int visible = Convert.ToInt32((item.FindControl("chkVisibility") as CheckBox).Checked);
                    string TextAlign = (item.FindControl("ddlTextAlign") as DropDownList).SelectedValue.Trim();
                    string LabelFontSize = (item.FindControl("txtLabelFontSize") as TextBox).Text.Trim();
                    string DataFonSize = (item.FindControl("txtDataFontSize") as TextBox).Text.Trim();

                    int x = DBAccessPradeepMetals.UpdateAndonViewSettings("AndonViewColumn", ColumnName, DisplayText, sortOrder, visible, TextAlign, LabelFontSize, DataFonSize);
                    if (x > 0)
                    {
                        success = true;
                    }
                }

                if (success)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessMsg", "alert('Saved Successfully');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg", "alert('Save Failed');", true);
                }

                BindGenericSettings();
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("AndonPagePradeepMetals.aspx");
        }

        protected void BtnSaveGenric_Click(object sender, EventArgs e)
        {
            try
            {
                int FooterEnabled = 0, MsgEnabled = 0/*, curvedBoxes = 0*/;
                bool res = false;
                if (chkEnableFooter.Checked) FooterEnabled = 1;
                if (chkShowMsg.Checked) MsgEnabled = 1;
                //if (chkCurvedBoxes.Checked) curvedBoxes = 1;

                 res = DBAccessPradeepMetals.UpdateAndonGeneralSettings("update", txtPageTitle.Text.Trim(), ddlFontFamily.SelectedValue, ddlFontStyle.SelectedValue, ddlDataRefreshInterval.SelectedValue, ddlScreenFlipInterval.SelectedValue, FooterEnabled, MsgEnabled, txtScrollingText.Text.Trim(), ddlDateFormat.SelectedValue, ddlTimeFormat.SelectedValue/*, curvedBoxes*/);

                //res = DBAccessPradeepMetals.UpdateColorCodeSettings("update", "GoodColor", "#FF" + txtGood.Text.Trim());
                //res = DBAccessPradeepMetals.UpdateColorCodeSettings("update", "ModerateColor", "#FF" + txtModerate.Text.Trim());
                //res = DBAccessPradeepMetals.UpdateColorCodeSettings("update", "BadColor", "#FF" + txtBad.Text.Trim());

                if (res)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessMsg", "alert('Saved Successfully');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg", "alert('Save Failed');", true);
                }
                BindGenericSettings();
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        internal void BindGenericSettings()
        {
            AndonGeneralSettingsEntity entity = new AndonGeneralSettingsEntity();
            try
            {
                entity = DBAccessPradeepMetals.ViewAndonGeneralSettings("view");
                string computerName = string.Empty;
                if (HttpContext.Current.Request.Cookies["ComputerName"] != null)
                {
                    computerName = HttpContext.Current.Request.Cookies["ComputerName"].Value.ToString();
                }
                txtDeviceName.Text = computerName;
                txtPageTitle.Text = entity.AndonTitle;
                ddlFontFamily.SelectedValue = entity.FontFamily;
                ddlFontStyle.SelectedValue = entity.FontStyle;
                ddlDataRefreshInterval.SelectedValue = entity.DataRefreshInterval;
                ddlScreenFlipInterval.SelectedValue = entity.ScreenFlipInterval;
                chkEnableFooter.Checked = Convert.ToBoolean(Convert.ToInt32(entity.FooterEnabled.Equals("none", StringComparison.OrdinalIgnoreCase) ? "0" : "1"));
                chkShowMsg.Checked = Convert.ToBoolean(Convert.ToInt32(entity.MsgEnabled.Equals("none", StringComparison.OrdinalIgnoreCase) ? "0" : "1"));
                txtScrollingText.Text = entity.ScrollingText;
                ddlDateFormat.SelectedValue = entity.DateFormat;
                ddlTimeFormat.SelectedValue = entity.TimeFormat;
                //chkCurvedBoxes.Checked = Convert.ToBoolean(entity.CurvedBoxes);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}