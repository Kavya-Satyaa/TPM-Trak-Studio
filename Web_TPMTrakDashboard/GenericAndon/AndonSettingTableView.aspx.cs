using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GenericAndon.Model;

namespace Web_TPMTrakDashboard.GenericAndon
{
    public partial class AndonSettingTableView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null || Session["connectionString"] == null)
                    Response.Redirect("../SignIn.aspx", false);

                BindCellId();
                BindFrequency();
                BindGenericSettings();
                BindData();
            }
        }
        internal void BindCellId()
        {
            try
            {
                List<string> list = new List<string>();
                list = AndonDBAccess.getCellID("");
                ddlCellID.DataSource = list;
                ddlCellID.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindCellId= " + ex.Message);
            }
        }

        internal void BindGenericSettings()
        {
            SettingsViewEntity entity = new SettingsViewEntity();
            try
            {
                string computerName = string.Empty;
                if (HttpContext.Current.Request.Cookies["ComputerName_TableView"] != null)
                {
                    computerName = HttpContext.Current.Request.Cookies["ComputerName_TableView"].Value.ToString();
                }

                string RunOption = AndonDBAccess.GetRunOption(computerName, "ComputerRunOption_TableView");
                if (!string.IsNullOrEmpty(RunOption) && ddlRunOption.Items.FindByValue(RunOption) != null)
                    ddlRunOption.SelectedValue = RunOption;
                txtDeviceName.Text = computerName;
                entity = AndonDBAccess.ViewSettings(txtDeviceName.Text, "s_GetAndonUISettings", "CockpitAndonTableViewUISettings"); //.ViewAndonGeneralSettings("view");
                if (entity != null)
                {
                    txtPageTitle.Text = entity.AndonTitle;
                    ddlFontFamily.SelectedValue = entity.FontFamily;
                    ddlFontStyle.SelectedValue = entity.FontStyle;
                    ddlDataRefreshInterval.SelectedValue = entity.DataDisplayInterval.ToString();
                    ddlScreenFlipInterval.SelectedValue = entity.ScreenFlipInterval.ToString();
                    chkEnableFooter.Checked = Convert.ToBoolean(entity.ShowFooterBlock);
                    chkShowMsg.Checked = Convert.ToBoolean(entity.MsgBlockEnabled);
                    txtScrollingText.Text = entity.ScrollingText;
                    ddlDateFormat.SelectedValue = entity.DateFormat;
                    ddlTimeFormat.SelectedValue = entity.TimeFormat;
                    var arr = entity.OrderBy.Split(' ').ToList();
                    if (arr.Count() == 2)
                    {
                        if (ddlSortBy.Items.FindByValue(arr[0]) != null)
                            ddlSortBy.SelectedValue = arr[0];
                        if (ddlSortOrder.Items.FindByValue(arr[1]) != null)
                            ddlSortOrder.SelectedValue = arr[1];
                    }
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                {
                    trAndonDisplayFrequency.Visible = false;
                }
                else
                {
                    trAndonDisplayFrequency.Visible = true; string Freq = AndonDBAccess.GetFrequency(txtDeviceName.Text, "AndonFrequency", "AndonFrequency_TableView");
                    if (!string.IsNullOrEmpty(Freq))
                    {
                        if (ddlFrequency.Items.FindByValue(Freq) != null)
                            ddlFrequency.SelectedValue = Freq;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


        private void BindFrequency()
        {
            try
            {
                List<ListItem> list = new List<ListItem>();
                list.Add(new ListItem() { Value = "Last One", Text = "Latest One Hour" });
                list.Add(new ListItem() { Value = "Two Hours", Text = "Latest Two Hours" });
                list.Add(new ListItem() { Value = "Three Hours", Text = "Latest Three Hours" });
                list.Add(new ListItem() { Value = "Shift", Text = "Shift" });
                list.Add(new ListItem() { Value = "Day", Text = "Day" });
                ddlFrequency.DataSource = list;
                ddlFrequency.DataTextField = "Text";
                ddlFrequency.DataValueField = "Value";
                ddlFrequency.DataBind();
                if (Request.Cookies["AndonFrequency"] == null)
                {
                    Response.Cookies["AndonFrequency"].Value = ddlFrequency.SelectedValue;
                    Response.Cookies["AndonFrequency"].Expires = DateTime.MaxValue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"BindFrequency_CockpitAndon: {ex}");
            }
        }

        internal void BindData()
        {
            List<AndonDefaultsEntity> list = new List<AndonDefaultsEntity>();
            string ComputerName = "", Parameter = "CockpitAndonTableViewParameter";
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    Parameter = "AndonParameter_TableView_KM";
                if (HttpContext.Current.Request.Cookies["ComputerName_TableView"] != null)
                {
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName_TableView"].Value.ToString();
                }
                list = AndonDBAccess.GetAndonViewSettingsData(ddlCellID.SelectedValue, ComputerName, Parameter);

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
            string ComputerName = "", Parameter = "CockpitAndonTableViewParameter";
            string isSuccessfull = "";
            try
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                    Parameter = "AndonParameter_TableView_KM";
                if (HttpContext.Current.Request.Cookies["ComputerName_TableView"] != null)
                {
                    ComputerName = HttpContext.Current.Request.Cookies["ComputerName_TableView"].Value.ToString();
                }
                foreach (ListViewDataItem item in lvParameterDetails.Items)
                {
                    string ColumnName = (item.FindControl("txtColumnName") as Label).Text;
                    string DisplayText = (item.FindControl("txtDisplayText") as TextBox).Text.Trim();
                    string sortOrder = (item.FindControl("txtSortOrder") as TextBox).Text.Trim();
                    string visible = (item.FindControl("chkVisibility") as CheckBox).Checked.ToString();
                    string TextAlign = (item.FindControl("ddlTextAlign") as DropDownList).SelectedValue.Trim();
                    string LabelFontSize = (item.FindControl("txtLabelFontSize") as TextBox).Text.Trim();
                    string DataFonSize = (item.FindControl("txtDataFontSize") as TextBox).Text.Trim();

                    AndonDBAccess.UpdateGridSettingData(Parameter, ColumnName, DisplayText, sortOrder, visible, TextAlign, LabelFontSize, DataFonSize, ddlCellID.SelectedValue, ComputerName, out isSuccessfull);

                }

                if (isSuccessfull.Equals("Successfull"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Successfully Updated')", true);
                    BindData();
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg", "alert('Save Failed');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductionAndonTableView.aspx");
        }

        protected void BtnSaveGenric_Click(object sender, EventArgs e)
        {
            try
            {
                int FooterEnabled = 0, MsgEnabled = 0;
                string res = "";
                if (chkEnableFooter.Checked) FooterEnabled = 1;
                if (chkShowMsg.Checked) MsgEnabled = 1;

                SettingsViewEntity data = new SettingsViewEntity();
                data.AndonTitle = txtPageTitle.Text.Trim();
                data.FontFamily = ddlFontFamily.SelectedValue.Trim();
                data.FontStyle = ddlFontStyle.SelectedValue.Trim();
                data.DataDisplayInterval = ddlDataRefreshInterval.SelectedValue.Trim();
                data.ScreenFlipInterval = ddlScreenFlipInterval.SelectedValue.Trim();
                data.OrderBy = ddlSortBy.SelectedValue.Trim() + " " + ddlSortOrder.SelectedValue.Trim();
                data.ShowFooterBlock = FooterEnabled;
                data.MsgBlockEnabled = MsgEnabled;
                data.DateFormat = ddlDateFormat.SelectedValue;
                data.TimeFormat = ddlTimeFormat.SelectedValue;
                data.ScrollingText = txtScrollingText.Text.Trim();

                AndonDBAccess.UpdateAndonSettings(data, "CockpitAndonTableViewUISettings", txtDeviceName.Text, out string isSuccessfull);

                if (System.Web.Configuration.WebConfigurationManager.AppSettings["KachMotors"].ToString() == "1")
                {
                    trAndonDisplayFrequency.Visible = false;
                }
                else
                {
                    trAndonDisplayFrequency.Visible = true; string Freq = AndonDBAccess.GetFrequency(txtDeviceName.Text, "AndonFrequency", "AndonFrequency_TableView");
                    AndonDBAccess.SaveAndonFrquency(txtDeviceName.Text, ddlFrequency.SelectedValue, "AndonFrequency_TableView");
                }

                AndonDBAccess.SaveRunOption(txtDeviceName.Text, ddlRunOption.SelectedValue, "ComputerRunOption_TableView");

                if (isSuccessfull.Equals("Successfull", StringComparison.OrdinalIgnoreCase))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessMsg", "alert('Saved Successfully');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg", "alert('Save Failed');", true);
                }
                BindGenericSettings();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}