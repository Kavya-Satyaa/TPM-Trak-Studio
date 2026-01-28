using OfficeOpenXml2;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PradeepMetals
{
    public partial class SchedulingScreenPradeepMetals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/SignIn.aspx", false);
                }

                else
                {
                    ddlScheduleStatus.Items.Insert(0, "All");
                    BindMachineID();
                    BindPartNumber();
                    BindData("ScheduleStatus");
                }

            }
        }
        internal void BindMachineID()
        {
            List<string> list = new List<string>();
            try
            {
                list = DBAccessPradeepMetals.GetMachineIDForPMScheduleScreen();
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();

                ddlMachineIDNew.DataSource = list;
                ddlMachineIDNew.DataBind();

                ddlMachineID.Items.Insert(0, "All");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        internal void BindPartNumber()
        {
            List<string> list = new List<string>();
            try
            {
                list = DBAccessPradeepMetals.GetPartsForPMScheduleScreen(ddlMachineID.SelectedValue.ToString() == "All" ? "" : ddlMachineID.SelectedValue.ToString());
                ddlPartNumber.DataSource = list;
                ddlPartNumber.DataBind();

                if (list.Count > 0)
                {
                    ddlPartNumber.Items.Insert(0, "All");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        internal void BindData(string Param)
        {
            List<PMSceduleScreenEntity> list = new List<PMSceduleScreenEntity>();
            try
            {
                if (Param.Equals("ScheduleStatus"))
                {
                    list = DBAccessPradeepMetals.GetScheduleScreenData(ddlScheduleStatus.SelectedValue == "All" ? "" : ddlScheduleStatus.SelectedValue, "", "", "", "");
                }
                else if (Param.Equals("SearchByMachine"))
                {
                    list = DBAccessPradeepMetals.GetScheduleScreenData("", ddlMachineID.SelectedValue == "All" ? "" : ddlMachineID.SelectedValue, ddlPartNumber.SelectedValue == "All" ? "" : ddlPartNumber.SelectedValue, "", "");
                }
                else if (Param.Equals("SearchByDate"))
                {
                    list = DBAccessPradeepMetals.GetScheduleScreenData("", "", "", txtFromDate.Text, txtToDate.Text);
                }

                list = list.Where(x => x.SendToHMIStatus != "2").ToList<PMSceduleScreenEntity>();

                lvScheduleDetails.DataSource = list;
                lvScheduleDetails.DataBind();

                if (Param.Equals("ScheduleStatus") || Param.Equals("SearchByMachine"))
                    Session["OriginalData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPartNumber();
        }

        protected void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            int InsertedRows = 0;
            try
            {
                if (FileImport.HasFile)
                {
                    string fileName = FileImport.FileName.ToString();

                    if (!Directory.Exists(Server.MapPath("ScheduleFiles")))
                    {
                        Directory.CreateDirectory(Server.MapPath("ScheduleFiles"));
                    }
                    FileImport.SaveAs(Server.MapPath("ScheduleFiles") + "\\" + fileName);
                    string errorMsg = "";
                    GetExcelDataToList(fileName, out InsertedRows, out dt, out errorMsg);

                    int result = DBAccessPradeepMetals.InsertScheduleData(dt);
                    if (result > 0)
                    {
                        DBAccessPradeepMetals.BulkInsertScheduleData();
                    }
                    if (result == InsertedRows)
                    {
                        if (errorMsg != "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "openWarningModalsss", "openWarningModalMsg('" + errorMsg + "');", true);
                        }
                        HelperClassGeneric.openSuccessModal(this, "Import Successful");
                    }
                    else
                    {
                        HelperClassGeneric.openErrorModal(this, "Import Failed");
                    }

                    BindMachineID();
                    BindPartNumber();
                    BindData("ScheduleStatus");
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Import file is not Selected.");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            BindData("ScheduleStatus");
        }

        protected void BtnNew_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "newEditModal", "OpenNewEditModal();", true);
        }
        protected void BtnView2_Click(object sender, EventArgs e)
        {
            BindData("SearchByMachine");
        }

        protected void BtnView1_Click(object sender, EventArgs e)
        {
            BindData("SearchByDate");
        }

        protected void btnNewSave_Click(object sender, EventArgs e)
        {
            try
            {
                PMSceduleScreenEntity entity = new PMSceduleScreenEntity();
                entity.MachineID = ddlMachineIDNew.SelectedValue.ToString().Trim();
                entity.PartNumber = txtPartNumber.Text.ToString().Trim();
                entity.LotCode = txtLotCode.Text.ToString().Trim();
                entity.JobNumber = txtJobNo.Text.ToString().Trim();
                entity.OperationNo = txtOperationNo.Text.ToString().Trim();
                entity.ScheduleDate = txtScheduleDate.Text.ToString().Trim();
                entity.Priority = txtPriority.Text.ToString().Trim();
                entity.PlannedQuantity = txtPlannedQty.Text.ToString().Trim();
                entity.RevNo = txtRevID.Text.ToString();
                string result = DBAccessPradeepMetals.InsertNewSchdule(entity);
                if (result.Equals("COP Not Available.", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openErrorModal(this, "COP master Not Available.");
                    return;
                }
                else if (result.Equals("Schedule Already Exists.", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openWarningModal(this, result);
                    return;
                }
                else if (result.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Schedule Created Successfully.");
                else
                {
                    HelperClassGeneric.openErrorModal(this, "Error Occured While Creating. Try Again!");
                    return;
                }

                BindData("ScheduleStatus");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private string GetExcelDataToList(string fileName, out int InsertedRows, out DataTable dt, out string errorMsg)
        {
            InsertedRows = 0;
            dt = new DataTable();
            errorMsg = "";
            using (var excel = new ExcelPackage())
            {
                using (var stream = File.OpenRead(Server.MapPath("ScheduleFiles") + "\\" + fileName))
                {
                    excel.Load(stream);
                }
                var wb = excel.Workbook;
                if (wb != null)
                {
                    try
                    {
                        var wsScheduleData = wb.Worksheets[0];
                        dt.Columns.Add("MachineID", typeof(string));
                        dt.Columns.Add("Priority", typeof(int));
                        dt.Columns.Add("PMLPartNo", typeof(string));
                        dt.Columns.Add("JobTravellerNo", typeof(string));
                        dt.Columns.Add("PlannedQty", typeof(float));
                        dt.Columns.Add("LotCode", typeof(string));
                        dt.Columns.Add("OperationNo", typeof(string));
                        dt.Columns.Add("ScheduleDate", typeof(DateTime));
                        dt.Columns.Add("RevID", typeof(string));

                        int rowStart = 3;
                        int LastRow = GetLastUsedRow(wsScheduleData);
                        var temp_machineID = "";
                        for (int i = rowStart; i <= LastRow; i++)
                        {
                            string machineID = "";
                            string PMLPartNumber = "";
                            string JobTravellerNo = "";
                            string LotCode = "";
                            string ScheduleDate = "";
                            int priority = 0;
                            string RevID = "";
                            int cols = 8;
                            if (wsScheduleData.Cells[i, cols].Value != null)
                            {
                                machineID = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                                temp_machineID = machineID;
                            }
                            cols++;
                            if (wsScheduleData.Cells[i, cols].Value == null)
                            {
                                errorMsg += "Priority missing in Row " + i.ToString();
                                return "";
                            }
                            else
                            {
                                if (!int.TryParse(wsScheduleData.Cells[i, cols].Value.ToString().Trim(), out int Priority))
                                {
                                    Priority = 0;
                                    errorMsg += "Priority is not in proper format in row: " + i.ToString() + "@";
                                }
                                priority = Priority;
                            }
                            cols++;

                            if (wsScheduleData.Cells[i, cols].Value == null)
                                errorMsg += "Part Number missing in row: " + i.ToString() + "@";
                            else
                                PMLPartNumber = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                            cols++;

                            if (wsScheduleData.Cells[i, cols].Value == null)
                                errorMsg += "Job Traveller Number missing in row: " + i.ToString() + "@";
                            else
                                JobTravellerNo = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                            cols++;


                            int.TryParse(wsScheduleData.Cells[i, cols].Value.ToString().Trim(), out int plannedQty);

                            if (!int.TryParse(wsScheduleData.Cells[i, cols].Value == null ? "" : wsScheduleData.Cells[i, cols].Value.ToString(), out int PlannedQty))
                            {
                                PlannedQty = 0;
                                errorMsg += "Planned Qunatity is not in proper format in row: " + i.ToString() + "@";
                            }
                            cols++;

                            if (wsScheduleData.Cells[i, cols].Value == null)
                                errorMsg += "LotCode is missing in row:" + i.ToString() + "@";
                            else
                                LotCode = wsScheduleData.Cells[i, cols].Value.ToString().Trim();
                            cols++;

                            if (!int.TryParse(wsScheduleData.Cells[i, cols].Value == null ? "" : wsScheduleData.Cells[i, cols].Value.ToString().Trim(), out int OperationNo))
                            {
                                OperationNo = 0;
                                errorMsg += "Opeartion Number is missing in row: " + i.ToString() + "@";
                            }
                            cols++;

                            if (wsScheduleData.Cells[i, cols].Value == null) errorMsg += "Schedule Date is missing in row: " + i.ToString() + "@";
                            else
                            {
                                try
                                {
                                    ScheduleDate = DateTime.FromOADate(Convert.ToDouble(wsScheduleData.Cells[i, cols].Value.ToString())).ToString("yyyy-MM-dd HH:mm:ss");
                                    ScheduleDate = Util.GetDateTime(ScheduleDate).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch (Exception ex)
                                {
                                    errorMsg += "ScheduleDate not in proper format in row: " + i.ToString() + "@";
                                }
                            }
                            cols++;

                            RevID = string.IsNullOrEmpty(wsScheduleData.Cells[i, cols].Value.ToString()) ? "" : wsScheduleData.Cells[i, cols].Value.ToString();

                            if (DBAccessPradeepMetals.ValidatePartNumberOperationNo(PMLPartNumber, OperationNo.ToString(), temp_machineID))
                            {
                                var tableRow = dt.NewRow();
                                tableRow[0] = temp_machineID;
                                tableRow[1] = priority;
                                tableRow[2] = PMLPartNumber;
                                tableRow[3] = JobTravellerNo;
                                tableRow[4] = PlannedQty;
                                tableRow[5] = LotCode;
                                tableRow[6] = OperationNo;
                                tableRow[7] = ScheduleDate;
                                tableRow[8] = RevID;
                                dt.Rows.Add(tableRow);
                                InsertedRows++;
                            }
                            else
                            {
                                errorMsg += temp_machineID + ", " + PMLPartNumber + ", " + OperationNo + " is not present in Master" + "@";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog("GetExcelDataToList: " + ex.Message);
                    }
                }
            }
            return "Successful";
        }

        private int GetLastUsedRow(ExcelWorksheet worksheet)
        {
            var row = worksheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }

            return row;
        }

        protected void ddlScheduleStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData("ScheduleStatus");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            int deletedRows = 0, RowsToBeDeleted = 0;
            try
            {
                foreach (ListViewDataItem row in lvScheduleDetails.Items)
                {
                    if ((row.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        RowsToBeDeleted++;
                        string machineID = (row.FindControl("lblMachineID") as Label).Text.Trim();
                        string PartNumber = (row.FindControl("lblPartNumber") as Label).Text.Trim();
                        string JobNo = (row.FindControl("lblJobNumber") as Label).Text.Trim();
                        string LotCode = (row.FindControl("lblLotCode") as Label).Text.Trim();
                        string OperationNo = (row.FindControl("lblOperationNo") as Label).Text.Trim();
                        string Status = (row.FindControl("lblScheduleStatus") as Label).Text.Trim();
                        if (Status.Equals("Completed", StringComparison.OrdinalIgnoreCase) || Status.Equals("Hold", StringComparison.OrdinalIgnoreCase))
                            deletedRows += DBAccessPradeepMetals.SoftDeleteSelectedSchedule(machineID, PartNumber, JobNo, LotCode, OperationNo);
                        else
                            deletedRows += DBAccessPradeepMetals.DeleteSelectedSchedule(machineID, PartNumber, JobNo, LotCode, OperationNo);
                    }
                }

                if (RowsToBeDeleted == 0)
                    HelperClassGeneric.openWarningToastrModal(this, "Select atleast one row to Delete.");
                else if (RowsToBeDeleted == deletedRows)
                    HelperClassGeneric.openDeleteSuccessModal(this);
                else
                    HelperClassGeneric.openDeleteErrorModal(this);
                //BindMachineID();
                //BindPartNumber();
                BindData("ScheduleStatus");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            bool updated = false;
            int RowsToBeUpdated = 0, UpdatedRows = 0;
            try
            {
                foreach (ListViewDataItem item in lvScheduleDetails.Items)
                {
                    if ((item.FindControl("hdnUpdate") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        RowsToBeUpdated++;
                        string machineID = (item.FindControl("lblMachineID") as Label).Text;
                        string priority = (item.FindControl("txtPriority") as TextBox).Text;
                        string PartNumber = (item.FindControl("lblPartNumber") as Label).Text.Trim();
                        string JobNo = (item.FindControl("lblJobNumber") as Label).Text.Trim();
                        string PlannedQty = (item.FindControl("lblPlannedQty") as Label).Text.Trim();
                        string LotCode = (item.FindControl("lblLotCode") as Label).Text.Trim();
                        string OperationNo = (item.FindControl("lblOperationNo") as Label).Text.Trim();
                        string ScheduleDate = (item.FindControl("lblScheduleDate") as Label).Text.Trim();
                        string ScheduleStatus = (item.FindControl("lblScheduleStatus") as Label).Text.Trim();
                        string RevID = (item.FindControl("txtRevNo") as TextBox).Text.Trim();

                        UpdatedRows += DBAccessPradeepMetals.UpdateSchdules(machineID, priority, PartNumber, JobNo, PlannedQty, LotCode, OperationNo, ScheduleDate, ScheduleStatus, RevID);

                    }
                }

                if (RowsToBeUpdated == 0)
                    HelperClassGeneric.openWarningToastrModal(this, "No Rows are Updated.");
                else if (RowsToBeUpdated == UpdatedRows)
                    HelperClassGeneric.openUpdateSuccessModal(this);
                else
                    HelperClassGeneric.openWarningToastrModal(this, "TRY AGAIN! Updation Failed. ");

                //BindMachineID();
                //BindPartNumber();
                BindData("ScheduleStatus");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                List<PMSceduleScreenEntity> list = new List<PMSceduleScreenEntity>();
                //List<PMSceduleScreenEntity> list = DBAccessPradeepMetals.GetScheduleReportData("'Completed','Hold'", ddlMachineID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineID.SelectedValue, "", txtFromDate.Text, txtToDate.Text);
                //if (Session["OriginalData"] != null)
                //    list = Session["OriginalData"] as List<PMSceduleScreenEntity>;
                //else
                if (string.IsNullOrEmpty(txtYear.Text.Trim()))
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Kindly Select a Year to Generate Report");
                    return;
                }
                if (string.IsNullOrEmpty(txtMonth.Text.Trim()))
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Kindly, Select a Month to Generate Report");
                    return;
                }
                list = DBAccessPradeepMetals.GetScheduleReportData("Completed,Hold", txtYear.Text.Trim(), txtMonth.Text.Trim());

                //list = list.Where(x => (x.ScheduleStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase) || x.ScheduleStatus.Equals("Hold", StringComparison.OrdinalIgnoreCase))).OrderBy(x => x.MachineID).ToList<PMSceduleScreenEntity>();
                list = list.OrderByDescending(x => Util.GetDateTime(x.ScheduleDate).ToString("yyyy-MM-dd")).ToList<PMSceduleScreenEntity>();
                string res = PradeepMetalsGenerateReport.GetScheduleScreenReport(list);
                if (string.IsNullOrEmpty(res))
                {
                    HelperClassGeneric.openErrorModal(this, "Error generating Report");
                }
                else if (res.Equals("TemplateNotFound", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Template Not Found");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}