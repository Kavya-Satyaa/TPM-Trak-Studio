using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Denso.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Denso
{
    public partial class FiveSChecksheetReportDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.Year.ToString();
                HelperClassGeneric.setDropdownValue(ddlMonth, DateTime.Now.ToString("MM"));
                BindMachine();
                BindWeekNo();
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachines("");
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindWeekNo()
        {
            try
            {
                List<string> list = DataBaseAccess.getWeekNumbersOfYear(txtYear.Text);
                ddlWeekNo.DataSource = list;
                ddlWeekNo.DataBind();
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
                DataTable dt = DensoDBAccess.getFiveSCheckpointTransactionDetails(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, "Report_CheckSheetDetails");

                List<FiveSChecksheetTransEntity> list = new List<FiveSChecksheetTransEntity>();
                var distDate = dt.AsEnumerable().Select(k => k.Field<DateTime>("Date")).Distinct().ToList();
                var distCheckpoints = dt.AsEnumerable().Select(k => k["CheckItem"].ToString()).Distinct().ToList();
                FiveSChecksheetTransEntity headerData = new FiveSChecksheetTransEntity();
                int i = 0;
                int slNoCount = 1;
                foreach (string checkPoint in distCheckpoints)
                {
                    var distShift = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase)).Select(k => k["ShiftID"].ToString()).Distinct().ToList();
                    int checkpointC = 0;
                    string cycleTimeDisplay = "";
                    int cycleTimeListDataUpdateIndex = 0;
                    foreach (string shift in distShift)
                    {
                        if (cycleTimeDisplay == "")
                        {
                            cycleTimeListDataUpdateIndex = list.Count();
                        }
                        FiveSChecksheetTransEntity mainData = new FiveSChecksheetTransEntity();
                        DataRow firstRow = dt.AsEnumerable().Where(k => k["CheckItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase) && k["ShiftID"].ToString() == shift).FirstOrDefault();
                        mainData.ChecklistID = firstRow["CheckItem"].ToString();
                        cycleTimeDisplay += firstRow["ShiftName"].ToString() + "(By " + Util.GetDateTime(firstRow["ShiftEnd"].ToString()).ToString("HH:mm") + ") <br/>";
                        mainData.Cycle = firstRow["Cycle"].ToString();
                        mainData.SortOrder = firstRow["SortOrder"].ToString();
                        mainData.CheckpointType = firstRow["ChecklistType"].ToString();
                        mainData.ContentVisibility = true;
                        mainData.Shift = shift;
                        mainData.ShiftName = firstRow["ShiftName"].ToString();
                        if (i == 0)
                        {
                            headerData.HeaderVisibility = true;
                        }
                        if (checkpointC == 0)
                        {
                            mainData.RowSpan = distShift.Count.ToString();
                            checkpointC++;

                            mainData.SlNo = slNoCount.ToString();
                            slNoCount++;
                        }
                        else
                        {
                            mainData.RowDisplay = "none";
                        }


                        List<FiveSChecksheetShiftEntity> valueList = new List<FiveSChecksheetShiftEntity>();
                        List<FiveSChecksheetShiftEntity> valueListHeader = new List<FiveSChecksheetShiftEntity>();
                        foreach (DateTime date in distDate)
                        {
                            FiveSChecksheetShiftEntity valueData = new FiveSChecksheetShiftEntity();
                            DataRow valueRow = null;

                            valueRow = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date && k["ShiftID"].ToString() == shift && k["CheckItem"].ToString() == checkPoint).FirstOrDefault();

                            if (date.Date == DateTime.Now.Date)
                            {
                                valueData.ControlEnabled = true;
                            }
                            else
                            {
                                valueData.ControlEnabled = false;
                            }
                            if (valueRow != null)
                            {
                                valueData.ContentVisibility = true;
                                valueData.ActualValue = valueRow["Value"].ToString();
                                if (valueData.ActualValue == "OK" || valueData.ActualValue == "Yes")
                                {
                                    valueData.CheckpointTypeBackColor = "green";
                                    valueData.CheckpointTypeForeColor = "white";
                                }
                                if (valueData.ActualValue == "NG" || valueData.ActualValue == "No")
                                {
                                    valueData.CheckpointTypeBackColor = "red";
                                    valueData.CheckpointTypeForeColor = "white";
                                }
                                valueData.CheckpointType = mainData.CheckpointType;
                                valueData.Date = date.ToString("dd-MM-yyyy");

                                if (valueData.CheckpointType.Equals("Image", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (valueRow["ImageData"].ToString() == "")
                                    {
                                        valueData.FileInBase64 = "";
                                    }
                                    else
                                    {
                                        valueData.FileName = valueRow["ImageName"].ToString();
                                        byte[] bytes = (byte[])valueRow["ImageData"];
                                        valueData.FileInBase64 = Convert.ToBase64String(bytes);
                                    }
                                }
                            }
                            valueList.Add(valueData);

                            if (i == 0)
                            {
                                FiveSChecksheetShiftEntity headerValueData = new FiveSChecksheetShiftEntity();
                                headerValueData.HeaderVisibility = true;
                                headerValueData.Day = date.Day.ToString();
                                valueListHeader.Add(headerValueData);
                            }
                        }

                        mainData.ValueList = valueList;

                        if (i == 0)
                        {
                            headerData.ValueList = valueListHeader;
                            list.Add(headerData);
                        }
                        list.Add(mainData);
                        i++;
                    }
                    list[cycleTimeListDataUpdateIndex == 0 ? 1 : cycleTimeListDataUpdateIndex].CycleTimeDisplay = cycleTimeDisplay;
                }
                lvChecklist.DataSource = list;
                lvChecklist.DataBind();

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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                string ReportStatus = "";
                dt = DensoDBAccess.getFiveSCheckpointTransactionDetails(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, "Report_CheckSheetDetails");
                if (dt.Rows.Count != 0)
                {
                    ReportStatus = ReportsDenso.Get5SChecksheetReport(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue);
                }
                if (ReportStatus.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
                else if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageSuccess", "messageSuccess();", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lvCheckpointValue_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                //if (e.Item.ItemType == ListViewItemType.DataItem)
                //{
                //    string checkpointType = (e.Item.FindControl("hdnCheckpointType") as HiddenField).Value;
                //    string actualValue = (e.Item.FindControl("hdnActualValue") as HiddenField).Value;
                //    if (checkpointType == "Checkbox")
                //    {
                //        if (actualValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                //        {
                //            (e.Item.FindControl("chkValue") as CheckBox).Checked = true;
                //        }
                //    }
                //    else if (checkpointType == "Yes-No")
                //    {
                //        HelperClassGeneric.setDropdownValue((e.Item.FindControl("ddlValue") as DropDownList), actualValue);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}