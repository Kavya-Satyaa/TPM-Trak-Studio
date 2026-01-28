using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Denso.Model;
using System.Text.RegularExpressions;

namespace Web_TPMTrakDashboard.Denso
{
    public partial class DailyCheckpointTransactionDenso : System.Web.UI.Page
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
                DataTable dt = DensoDBAccess.getDailyCheckpointTransactionDetails(ddlMachine.SelectedValue, txtYear.Text, ddlMonth.SelectedValue, ddlWeekNo.SelectedValue, "TransactionView");

                List<DailyChecklistTransEntity> list = new List<DailyChecklistTransEntity>();
                var distDate = dt.AsEnumerable().Select(k => k.Field<DateTime>("Date")).Distinct().ToList();
                var distCheckpoints = dt.AsEnumerable().Select(k => k["InspectionItem"].ToString()).Distinct().ToList();
                var distShift = dt.AsEnumerable().Where(k => !string.IsNullOrEmpty(k["Shift"].ToString())).Select(k => k["Shift"].ToString()).Distinct().ToList();

                DailyChecklistTransEntity headerData = new DailyChecklistTransEntity();

                int i = 0, j = 0;
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
                    //mainData.ShiftName = firstRow["Shift"].ToString();
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
                            //DateTime now = DateTime.Now;
                            //DateTime sixAMToday = DateTime.Today.AddHours(6);
                            //DateTime sixAMTomorrow = DateTime.Today.AddHours(30); // 30 hours = 6 am tomorrow

                            //if (date.Date >= sixAMToday && date.Date < sixAMTomorrow)
                            //{
                            //    valueData.ControlEnabled = true;
                            //}
                            //else
                            //{
                            //    valueData.ControlEnabled = false;
                            //}

                            if (valueRow != null)
                            {
                                valueData.ContentVisibility = true;
                                valueData.ShiftName = valueRow["Shift"].ToString();
                                valueData.ActualValue = valueRow["Value"].ToString();
                                if (mainData.JudgementCriteria.Contains("-"))
                                {
                                    //string[] values = mainData.JudgementCriteria.Split('-');
                                    //bool containsNumber = values[0].Any(char.IsDigit);
                                    //double value1 = 0.0;
                                    //if (containsNumber)
                                    //{
                                    //    value1 = Convert.ToDouble(values[0]);
                                    //}
                                    //string pattern = @"([\d.]+)\s*([A-Za-z/²]+)";

                                    //Match match = Regex.Match(values[1], pattern);
                                    //double value2 = 0.0;
                                    //if (match.Success)
                                    //{
                                    //    value2 = Convert.ToDouble(match.Groups[1].Value);
                                    //}
                                    string[] values = mainData.JudgementCriteria.Split('-');
                                    double value1 = 0.0;
                                    double value2 = 0.0;
                                    if (values.Length > 0)
                                    {
                                        bool containsNumber = values[0].Any(char.IsDigit);
                                        if (containsNumber)
                                        {
                                            if (double.TryParse(values[0], out double parsedValue))
                                            {
                                                value1 = parsedValue;
                                            }
                                            else
                                            {
                                                Logger.WriteDebugLog("Failed to convert value1.");
                                            }
                                        }
                                    }
                                    if (values.Length > 1)
                                    {
                                        string pattern = @"([\d.]+)\s*([A-Za-z/²]+)";
                                        Match match = Regex.Match(values[1], pattern);
                                        if (match.Success)
                                        {
                                            if (double.TryParse(match.Groups[1].Value, out double parsedValue))
                                            {
                                                value2 = parsedValue;
                                            }
                                            else
                                            {
                                                Logger.WriteDebugLog("Failed to convert value2 from regex match.");
                                            }
                                        }
                                    }

                                    if (valueData.ActualValue != "")
                                    {
                                        if(double.TryParse(valueData.ActualValue,out double parsedvalue1))
                                        {
                                            if (Convert.ToDouble(valueData.ActualValue) > value1 && Convert.ToDouble(valueData.ActualValue) < value2)
                                            {
                                                valueData.CheckpointTypeBackColor = "green";
                                                valueData.CheckpointTypeForeColor = "white";
                                            }
                                            else
                                            {
                                                valueData.CheckpointTypeBackColor = "red";
                                                valueData.CheckpointTypeForeColor = "white";
                                            }
                                        }
                                        else
                                        {
                                            Logger.WriteDebugLog("Failed to convert value2 from regex match.");
                                        }
                                        
                                    }
                                }
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
                        shiftDayData.ShiftName = shift;
                        shiftDayData.Shift = shift;
                        shiftDayData.ValueList = valueList;
                        shiftDayList.Add(shiftDayData);
                        if (shift == distDayShift[0])
                        {
                            if (i == 0)
                            {
                                shiftDayDataHeader.ValueList = valueListHeader;
                                shiftDayListHeader.Add(shiftDayDataHeader);
                            }
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
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    string checkpointType = (e.Item.FindControl("hdnCheckpointType") as HiddenField).Value;
                    string actualValue = (e.Item.FindControl("hdnActualValue") as HiddenField).Value;
                    if (checkpointType == "Checkbox")
                    {
                        if (actualValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            (e.Item.FindControl("chkValue") as CheckBox).Checked = true;
                        }
                    }
                    else if (checkpointType == "OK-NG" || checkpointType == "Yes-No")
                    {
                        if (actualValue == "Yes" || actualValue == "OK")
                        {
                            actualValue = "OK";
                        }
                        else if (actualValue == "NG" || actualValue == "No")
                        {
                            actualValue = "NG";
                        }
                        HelperClassGeneric.setDropdownValue((e.Item.FindControl("ddlValue") as DropDownList), actualValue);
                    }
                }
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
                int updateCount = 0;
                int i = 0;
                foreach (ListViewItem item in lvChecklist.Items)
                {
                    if (i == 0)
                    {
                        i++;
                        continue; //skip 
                    }

                    DailyChecklistTransEntity data = new DailyChecklistTransEntity();
                    data.MachineID = ddlMachine.SelectedValue;
                    data.Category = (item.FindControl("lblCategory") as Label).Text;
                    data.ChecklistID = (item.FindControl("lblChecklistID") as Label).Text;
                    data.JudgementCriteria = (item.FindControl("lblJudgementCriteria") as Label).Text;
                    data.Method = (item.FindControl("lblMethod") as Label).Text;
                    data.Cycle = (item.FindControl("lblCycle") as Label).Text;
                    data.PersonInCharge = (item.FindControl("lblPersonInCharge") as Label).Text;
                    data.Frequency = (item.FindControl("hdnFrequency") as HiddenField).Value;
                    data.FormatNumber = (item.FindControl("hdnFormatNumber") as HiddenField).Value;
                    data.RevID = (item.FindControl("hdnRevID") as HiddenField).Value;
                    data.RevNo = (item.FindControl("hdnRevNo") as HiddenField).Value;
                    data.RevDate = (item.FindControl("hdnRevDate") as HiddenField).Value;
                    data.RevisedBy = (item.FindControl("hdnRevisedBy") as HiddenField).Value;
                    data.SortOrder = (item.FindControl("hdnSortOrder") as HiddenField).Value;
                    data.ChecklistDesc = (item.FindControl("hdnChecklistDesc") as HiddenField).Value;
                    data.ChecklistType = (item.FindControl("hdnChecklistType") as HiddenField).Value;



                    ListView lvDayShift = item.FindControl("lvChecklistDayShift") as ListView;
                    foreach (ListViewItem dayShiftItem in lvDayShift.Items)
                    {
                        data.Shift = (dayShiftItem.FindControl("hdnShift") as HiddenField).Value;

                        ListView lvActualValue = dayShiftItem.FindControl("lvCheckpointValue") as ListView;
                        foreach (ListViewItem valueItem in lvActualValue.Items)
                        {
                            //string day = (valueItem.FindControl("hdnDay") as HiddenField).Value;

                            if (HelperClassGeneric.getCheckboxValue((valueItem.FindControl("hdnControlEnabled") as HiddenField).Value))
                            {
                                DateTime date = Util.GetDateTime((valueItem.FindControl("hdnDate") as HiddenField).Value);
                                //string date = (valueItem.FindControl("hdnDate") as HiddenField).Value;
                                //DateTime dateTime = Util.GetDateTime(date);
                                data.Year = date.Year.ToString();
                                data.Month = date.Month.ToString();

                                data.Date = date.ToString("yyyy-MM-dd");
                                data.InspectedBy = Session["UserName"].ToString();
                                if (data.ChecklistType.Equals("Text", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.ActualValue = (valueItem.FindControl("txtValue") as TextBox).Text;
                                }
                                else if (data.ChecklistType.Equals("OK-NG", StringComparison.OrdinalIgnoreCase) || data.ChecklistType.Equals("Yes-No", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.ActualValue = (valueItem.FindControl("ddlValue") as DropDownList).SelectedValue;
                                }
                                else if (data.ChecklistType.Equals("Checkbox", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.ActualValue = (valueItem.FindControl("chkValue") as CheckBox).Checked.ToString();
                                }
                                else if (data.ChecklistType.Equals("Image", StringComparison.OrdinalIgnoreCase))
                                {
                                    data.FileInBase64 = (valueItem.FindControl("hdnFileInBase64") as HiddenField).Value;
                                    if (data.FileInBase64 != "")
                                    {
                                        byte[] drawinginbytes = System.Convert.FromBase64String(data.FileInBase64.Substring(data.FileInBase64.LastIndexOf(',') + 1));
                                        data.FileInByte = drawinginbytes;
                                        data.FileName = (valueItem.FindControl("hdnFileName") as HiddenField).Value;
                                    }
                                }
                                string result = "";string prevValue = (valueItem.FindControl("hdnActualValue") as HiddenField).Value;
                                if (data.ChecklistType.Equals("OK-NG", StringComparison.OrdinalIgnoreCase) || data.ChecklistType.Equals("Yes-No", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (prevValue == "" && data.ActualValue != "None")
                                    {
                                        result = DensoDBAccess.insertUpdateDailyChecklistTransactionEntity(data);
                                    }
                                    else if(prevValue != "" && ((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                    {
                                        result = DensoDBAccess.insertUpdateDailyChecklistTransactionEntity(data);
                                    }
                                }
                                else if(data.ChecklistType == "Checkbox")
                                {
                                    if (prevValue == "" && data.ActualValue != "False")
                                    {
                                        result = DensoDBAccess.insertUpdateDailyChecklistTransactionEntity(data);
                                    }
                                    else if(prevValue!="" && ((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                    {
                                        result = DensoDBAccess.insertUpdateDailyChecklistTransactionEntity(data);
                                    }
                                }
                                else
                                {
                                    if (((valueItem.FindControl("hdnActualValue") as HiddenField).Value) != data.ActualValue)
                                    {
                                        result = DensoDBAccess.insertUpdateDailyChecklistTransactionEntity(data);
                                    }
                                }
                                if (result == "Updated" || result == "Inserted")
                                {
                                    updateCount++;
                                }
                            }
                            else
                            {
                                i++;
                                continue;
                            }
                        }
                    }
                    i++;
                }
                if (updateCount > 0)
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
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