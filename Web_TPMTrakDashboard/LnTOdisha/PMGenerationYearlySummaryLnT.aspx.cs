using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.LnTOdisha.Model;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.LnTOdisha.Model.LnTOdishaDTO;

namespace Web_TPMTrakDashboard.LnTOdisha
{
    public partial class PMGenerationYearlySummaryLnT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachine();
                txtYear.Text = DateTime.Now.ToString("yyyy");
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = LnTOdishaDBAccess.GetMachineInfoForPM();
                if (list.Count > 0)
                    list.Insert(0, "All");
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindData()
        {
            DataTable dt = new DataTable();
            DataTable dt_Status = new DataTable();
            int j = 1;
            try
            {
                dt = LnTOdishaDBAccess.getPMGenerationYearlySummaryDetails(txtYear.Text.Trim(), txtMonth.Text.Trim(), ddlMachine.SelectedValue, out dt_Status);

                List<PMGenerationYealyEntity> list = new List<PMGenerationYealyEntity>();
                if (dt.Rows.Count > 0)
                {
                    PMGenerationYealyEntity headerData = new PMGenerationYealyEntity();
                    headerData.HeaderVisible = true;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PMGenerationYealyEntity data = new PMGenerationYealyEntity();
                        data.Machine = dt.Rows[i]["MachineID"].ToString();
                        data.Slno = j++.ToString();
                        data.ContentVisible = true;
                        List<PMGenerationMonthEntity> monthList = new List<PMGenerationMonthEntity>();
                        List<PMGenerationMonthEntity> monthListHeader = new List<PMGenerationMonthEntity>();
                        for (int col = 1; col < dt.Columns.Count; col++)
                        {
                            PMGenerationMonthEntity monthData = new PMGenerationMonthEntity();
                            monthData.ContentVisible = true;
                            monthData.PlanStatus = dt_Status.Rows[i][col].ToString();
                            if (monthData.PlanStatus.Equals("no plan", StringComparison.OrdinalIgnoreCase))
                            {
                                if (string.IsNullOrEmpty(dt.Rows[i][col].ToString()))
                                    monthData.PlanStatus = "X";
                                monthData.PlanDate = "";
                            }
                            else if (string.IsNullOrEmpty(dt_Status.Rows[i][col].ToString()))
                            {
                                monthData.PlanStatus = "";
                                monthData.PlanDate = "";
                            }
                            else
                                monthData.PlanDate = "(" + Util.GetDateTime(dt.Rows[i][col].ToString()).ToString("dd-MM-yyyy") + ")";
                            var arr = dt.Columns[col].ColumnName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                            monthData.MonthNo = arr[0];
                            monthData.MonthName = HelperClassGeneric.getAbbreviatedMonthName(monthData.MonthNo);
                            monthData.Machine = data.Machine;
                            monthData.Year = arr[1];
                            if (Convert.ToUInt16(monthData.Year) > DateTime.Now.Year)
                                monthData.CssClass = "PlanAllowed";
                            else if (Convert.ToInt32(monthData.Year) == DateTime.Now.Year && Convert.ToInt32(monthData.MonthNo) >= DateTime.Now.Month)
                                monthData.CssClass = "PlanAllowed";
                            monthList.Add(monthData);

                            if (i == 0)
                            {
                                PMGenerationMonthEntity monthDataHeader = new PMGenerationMonthEntity();
                                monthDataHeader.HeaderVisible = true;
                                monthDataHeader.MonthName = monthData.MonthName;
                                monthListHeader.Add(monthDataHeader);
                            }
                        }
                        data.MonthList = monthList;
                        if (i == 0)
                        {
                            headerData.MonthList = monthListHeader;
                            list.Add(headerData);
                        }
                        list.Add(data);
                    }
                }
                lvPMData.DataSource = list;
                lvPMData.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string result = "";
            try
            {
                PMGenerationMonthEntity data = new PMGenerationMonthEntity();
                //for (int i = 1; i < lvPMData.Items.Count; i++)
                //{
                //    PMGenerationMonthEntity data = new PMGenerationMonthEntity();
                //    data.Machine = (lvPMData.Items[i].FindControl("lblMachine") as Label).Text;
                //    data.Year = txtYear.Text;
                //    ListView lvMonth = (lvPMData.Items[i].FindControl("lvMonthData") as ListView);
                //    foreach (ListViewItem monthIem in lvMonth.Items)
                //    {
                //        if ((monthIem.FindControl("hdnUpdate") as HiddenField).Value.Trim().Equals("Update", StringComparison.OrdinalIgnoreCase))
                //        {
                //            data.PlanDate = (monthIem.FindControl("hdnPlanDate") as HiddenField).Value;
                //            data.MonthNo = (monthIem.FindControl("hdnMonthNo") as HiddenField).Value;
                //            if (!string.IsNullOrEmpty(data.PlanDate))
                //                result = LnTOdishaDBAccess.insertPMGenerationYearlySummaryDetails(data);
                //            else
                //                result = "Updated";
                //        }
                //    }
                //}
                data.Machine = (lvPMData.Items[Convert.ToInt32(hdnRowIndex.Value)].FindControl("lblMachine") as Label).Text;
                data.PlanStatus = ddlPlanType.SelectedValue;
                var MonthNo = Convert.ToInt32(((lvPMData.Items[Convert.ToInt32(hdnRowIndex.Value)].FindControl("lvMonthData") as ListView).Items[Convert.ToInt32(hdnColIndex.Value)].FindControl("hdnMonthNo") as HiddenField).Value);
                data.MonthNo = string.IsNullOrEmpty(txtMonth.Text) ? (MonthNo < 10 ? $"0{MonthNo}" : MonthNo.ToString()) : HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text);
                data.Year = ((lvPMData.Items[Convert.ToInt32(hdnRowIndex.Value)].FindControl("lvMonthData") as ListView).Items[Convert.ToInt32(hdnColIndex.Value)].FindControl("hdnYear") as HiddenField).Value;

                if (ddlPlanType.SelectedValue.ToLower() == "noplan")
                    data.PlanDate = "";
                else
                    data.PlanDate = string.IsNullOrEmpty(txtPlanDate.Text) ? $"01-{data.MonthNo}-{data.Year}" : txtPlanDate.Text;
                

                result = LnTOdishaDBAccess.insertPMGenerationYearlySummaryDetails(data);

                if (result.Equals("Inserted", StringComparison.OrdinalIgnoreCase) || result.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "PM Activity Generation Succesfully.");
                else if (result.Equals("Transaction Completed", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningModal(this, "Transaction Found Against " + HelperClassGeneric.getAbbreviatedMonthName(MonthNo.ToString()) + ".Could not update.");
                else
                    HelperClassGeneric.openErrorModal(this, "PM Activity Generation Failed. Try Again!");
                BindData();
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

        protected void btnPlanSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Generated = LnTGenerateReport.GeneratePMActivityYearlyGenerationReport(txtYear.Text.Trim(), ddlMachine.SelectedValue.Trim());
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click: " + ex.Message);
            }
        }
    }
}