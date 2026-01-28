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
    public partial class DailyCheckPointReportDenso : System.Web.UI.Page
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
                DataTable dt = DensoDBAccess.getDailyCheckpointTransactionDetails(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, ddlWeekNo.SelectedValue, "Report_CheckSheetDetails");
                List<DailyChecklistTransEntity> list = new List<DailyChecklistTransEntity>();
                var distDate = dt.AsEnumerable().Select(k => k.Field<DateTime>("Date")).Distinct().ToList();
                var distCheckpoints = dt.AsEnumerable().Select(k => k["InspectionItem"].ToString()).Distinct().ToList();
                var distShift = dt.AsEnumerable().Where(k => !string.IsNullOrEmpty(k["Shift"].ToString())).Select(k => k["Shift"].ToString()).Distinct().ToList();

                DailyChecklistTransEntity headerData = new DailyChecklistTransEntity();

                int i = 0,j=0;
                int checkPointCount = 1;
                foreach (string checkPoint in distCheckpoints)
                {
                    DailyChecklistTransEntity mainData = new DailyChecklistTransEntity();
                    DataRow firstRow = dt.AsEnumerable().Where(k => k["InspectionItem"].ToString().Equals(checkPoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    mainData.SlNo = checkPointCount.ToString();
                    mainData.Category = firstRow["Category"].ToString();
                    mainData.ChecklistID = firstRow["InspectionItem"].ToString();
                    mainData.JudgementCriteria = firstRow["JudgementCriteria"].ToString();
                    mainData.Method = firstRow["Method"].ToString();
                    mainData.Cycle = firstRow["Cycle"].ToString();
                    mainData.PersonInCharge = firstRow["PersonInCharge"].ToString();
                    mainData.Frequency = firstRow["Frequency"].ToString();
                    mainData.FormatNumber = firstRow["FormatNumber"].ToString();
                    mainData.RevID = firstRow["RevID"].ToString();
                    mainData.RevNo = firstRow["RevNo"].ToString();
                    mainData.RevDate = firstRow["RevDate"].ToString();
                    mainData.RevisedBy = firstRow["RevisedBy"].ToString();
                    mainData.ChecklistType = firstRow["ChecklistType"].ToString();
                    mainData.SortOrder = firstRow["SortOrder"].ToString();
                    mainData.ChecklistDesc = firstRow["InspectionItemDescription"].ToString();
                    mainData.ContentVisibility = true;


                    if (i == 0)
                    {
                        headerData.HeaderVisibility = true;
                    }
                    List<string> distDayShift = new List<string>();
                    if (mainData.Frequency.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                    {
                        distDayShift = distShift;
                    }
                    else if (mainData.Frequency.Equals("Day", StringComparison.OrdinalIgnoreCase))
                    {
                        distDayShift.Add("");
                    }

                    List<DailyChecklistTransShiftDayEntity> shiftDayList = new List<DailyChecklistTransShiftDayEntity>();
                    List<DailyChecklistTransShiftDayEntity> shiftDayListHeader = new List<DailyChecklistTransShiftDayEntity>();
                    foreach (string shift in distDayShift)
                    {
                        DailyChecklistTransShiftDayEntity shiftDayData = new DailyChecklistTransShiftDayEntity();
                        DailyChecklistTransShiftDayEntity shiftDayDataHeader = new DailyChecklistTransShiftDayEntity();

                        List<DailyChecklistTransValueEntity> valueList = new List<DailyChecklistTransValueEntity>();
                        List<DailyChecklistTransValueEntity> valueListHeader = new List<DailyChecklistTransValueEntity>();
                        j = 0;
                        //distDate.RemoveRange(7, (distDate.Count - 7));
                        foreach (DateTime date in distDate)
                        {
                            DailyChecklistTransValueEntity valueData = new DailyChecklistTransValueEntity();
                            DataRow valueRow = null;
                            if (mainData.Frequency.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                            {
                                valueRow = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date && k["Shift"].ToString() == shift && k["InspectionItem"].ToString() == checkPoint).FirstOrDefault();
                            }
                            else if (mainData.Frequency.Equals("Day", StringComparison.OrdinalIgnoreCase))
                            {
                                valueRow = dt.AsEnumerable().Where(k => k.Field<DateTime>("Date") == date && k["InspectionItem"].ToString() == checkPoint).FirstOrDefault();
                            }
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
                                valueData.ShiftName = valueRow["Shift"].ToString();
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
                                valueData.CheckpointType = mainData.ChecklistType;
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
                                DailyChecklistTransValueEntity headerValueData = new DailyChecklistTransValueEntity();
                                headerValueData.HeaderVisibility = true;
                                if (j == 0)
                                {
                                    headerValueData.ShiftColumnVisibility = true;
                                }
                                headerValueData.Day = date.Day.ToString();
                                valueListHeader.Add(headerValueData);
                            }
                            if (j == 0)
                            {
                                valueData.ShiftColumnVisibility = true;
                            }
                            j++;
                        }
                        shiftDayData.Shift = shift;
                        shiftDayData.ValueList = valueList;
                        shiftDayList.Add(shiftDayData);

                        if (i == 0)
                        {
                            shiftDayDataHeader.ValueList = valueListHeader;
                            shiftDayListHeader.Add(shiftDayDataHeader);
                        }
                    }
                    mainData.ShiftDayList = shiftDayList;

                    if (i == 0)
                    {
                        headerData.ShiftDayList = shiftDayListHeader;
                        list.Add(headerData);
                    }
                    list.Add(mainData);
                    i++;
                    checkPointCount++;
                }
                lvChecklist.DataSource = list;
                lvChecklist.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string ReportStatus = "";
            try
            {
                ReportStatus = ReportsDenso.GetDailyCheckPointReport(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, ddlWeekNo.SelectedValue);
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

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void lvChecklist_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {

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