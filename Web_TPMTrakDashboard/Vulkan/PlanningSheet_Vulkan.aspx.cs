using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Vulkan
{
    public partial class PlanningSheet_Vulkan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MMM");
                txtStartDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                DataTable dt = DataBaseAccessVulkan.GetPlanningSheetData(ddlFrequency.SelectedValue.ToString(), Convert.ToInt32(txtYear.Text), Convert.ToInt32(HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text)));
                //if (ddlFrequency.SelectedValue == "Daily" || ddlFrequency.SelectedValue == "15 Days")
                //{
                //    dt = DataBaseAccessVulkan.GetPlanningSheetData(ddlFrequency.SelectedValue.ToString(), Convert.ToInt32(txtYear.Text),Convert.ToInt32(txtMonth.Text));
                //}
                //else
                //{
                //    dt = DataBaseAccessVulkan.GetPlanningSheetData(ddlFrequency.SelectedValue.ToString(), Convert.ToInt32(txtYear.Text), Convert.ToInt32(txtMonth.Text));
                //}
                List<PlanningSheetData> list = new List<PlanningSheetData>();
                int i = 0;
                if (dt.Rows.Count > 0)
                {
                    PlanningSheetData entity = new PlanningSheetData();
                    List<innerlistview_PlanningSheetData> li_innerData = new List<innerlistview_PlanningSheetData>();
                    innerlistview_PlanningSheetData innerdataValues = new innerlistview_PlanningSheetData();
                    if (i == 0)
                    {
                        for (int j = 1; j < dt.Columns.Count; j++)
                        {
                            innerdataValues = new innerlistview_PlanningSheetData();
                            innerdataValues.DateColumn = dt.Columns[j].ColumnName.ToString();
                            innerdataValues.HeaderVisibility = true;
                            li_innerData.Add(innerdataValues);

                        }
                        entity.MachineID = "Machine ID";
                        entity.HeaderVisibility = true;
                        entity.InnerListViewData = li_innerData;
                        list.Add(entity);
                    }

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        li_innerData = new List<innerlistview_PlanningSheetData>();
                        entity = new PlanningSheetData();
                        entity.MachineID = dtRow["MachineID"].ToString();
                        entity.SlNo = ++i;
                        for (int j = 1; j < dt.Columns.Count; j++)
                        {
                            innerdataValues = new innerlistview_PlanningSheetData();
                            if (dtRow[dt.Columns[j].ColumnName].ToString() != "")
                            {
                                innerdataValues.DateValue = Convert.ToDateTime(dtRow[dt.Columns[j].ColumnName].ToString()).ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                innerdataValues.DateValue = "";
                            }
                            innerdataValues.ContentVisibility = true;
                            li_innerData.Add(innerdataValues);
                        }
                        entity.ContentVisibility = true;
                        entity.InnerListViewData = li_innerData;
                        list.Add(entity);
                    }


                    //var distMachines = dt.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct().ToList();
                    //foreach (var Machine in distMachines)
                    //{
                    //    DataRow firstRow = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    //    entity = new PlanningSheetData();
                    //    entity.MachineID = Machine;
                    //    entity.SlNo = ++i;
                    //    for (int j = 1; j < dt.Columns.Count; j++)
                    //    {
                    //        innerdataValues = new innerlistview_PlanningSheetData();
                    //        innerdataValues.DateValue = (firstRow[dt.Columns[j].ColumnName].ToString());
                    //        innerdataValues.ContentVisibility = true;
                    //        li_innerData.Add(innerdataValues);
                    //    }
                    //    entity.ContentVisibility = true;
                    //    entity.InnerListViewData = li_innerData;
                    //    list.Add(entity);
                    //}

                }
                lvPlanningMaster.DataSource = list;
                lvPlanningMaster.DataBind();
                Session["PlanningSheetdata"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        public static string GetMonthName(int monthNumber)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            return cultureInfo.DateTimeFormat.GetMonthName(monthNumber);
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindMachines()
        {
            try
            {
                List<string> Machines = DataBaseAccessVulkan.GetMachinesForPlanningSheet();
                //lbMachineID.DataSource = Machines;
                //lbMachineID.DataBind();
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
                string format = "dd-MM-yyyy";
                DateTime startDate;
                string startdate = Util.GetDateTime(txtStartDate.Text).ToString("yyyy-MM-dd");
                //if (DateTime.TryParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                //{
                    string success = DataBaseAccessVulkan.SaveStartDate(ddlFrequency.SelectedValue.ToString(), startdate);
                    if (success == "Inserted")
                    {
                        HelperClass.openInsertSuccessModal(this);
                    }
                    else
                    {
                        HelperClass.openErrorModal(this, "Insertion Failed");
                    }
                //}
                //else
                //{
                //    HelperClass.openWarningModal(this, "incorrect Datetime");
                //}
                BindGrid();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = "PlanningSheet_Vulkan.xlsx";
                string Source = GetReportPath(Filename);
                string Template = "PlanningSheet_Vulkan" + DateTime.Now.ToString() + ".xlsx";
                string Destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                List<PlanningSheetData> list = Session["PlanningSheetdata"] as List<PlanningSheetData>;
                if (list.Count > 0)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var worksheet = Excel.Workbook.Worksheets[1];
                    worksheet.Cells["B2"].Value = ddlFrequency.SelectedValue.ToString();
                    worksheet.Cells["E2"].Value = txtYear.Text;
                    if (ddlFrequency.SelectedValue.ToString().Equals("15Days", StringComparison.OrdinalIgnoreCase) || ddlFrequency.SelectedValue.ToString().Equals("Daily", StringComparison.OrdinalIgnoreCase))
                    {
                        worksheet.Cells["G2"].Value = "Month:-";
                        worksheet.Cells["H2"].Value = txtMonth.Text;
                    }
                    int startRow = 4;
                    foreach (var data in list)
                    {
                        int colstart = 1;
                        if (startRow == 4)
                        {
                            worksheet.Cells[startRow, colstart].Value = "SL.NO";
                        }
                        else
                        {
                            worksheet.Cells[startRow, colstart].Value = data.SlNo;
                        }
                        colstart++;
                        if (startRow == 4)
                        {
                            worksheet.Cells[startRow, colstart].Value = "Machine ID";
                        }
                        else
                        {
                            worksheet.Cells[startRow, colstart].Value = data.MachineID;
                        }
                        colstart++;
                        List<innerlistview_PlanningSheetData> innerlist = data.InnerListViewData;
                        foreach (var value in innerlist)
                        {
                            if (startRow == 4)
                            {
                                worksheet.Cells[startRow, colstart].Value = value.DateColumn;
                            }
                            else
                            {
                                worksheet.Cells[startRow, colstart].Value = value.DateValue;
                            }
                            colstart++;
                        }
                        startRow++;

                        setThinBorder(worksheet, 4, 1, startRow - 1, colstart - 1);
                        worksheet.Cells[4, 1, startRow - 1, colstart - 1].AutoFitColumns();
                    }
                    DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        static string appPath = HttpContext.Current.Server.MapPath("~/Vulkan/Template");
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        public static string GetReportPath(string reportName)
        {
            string src;
            if (HttpContext.Current.Session["Language"] == null)
                src = Path.Combine(appPath, reportName);
            else
            {
                if (HttpContext.Current.Session["Language"].ToString() != "en")
                    src = Path.Combine(appPath, reportName);
                else
                    src = Path.Combine(appPath, reportName);
            }
            return src;
        }
        private static void DownloadMultipleFile(string fileName, byte[] byteArray)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(fileName) + "\"");
                HttpContext.Current.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private static void setThinBorder(ExcelWorksheet worksheet, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        }

        protected void btnPostPoneSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Machines = "";
                DateTime CurrentDate = DateTime.Now;
                int checkCount = 0;
                string success = "";
                for (int i = 0; i < lvPostPoneGrid.Items.Count; i++)
                {
                    if ((lvPostPoneGrid.Items[i].FindControl("chkPostPone") as CheckBox).Checked == true)
                    {
                        checkCount++;
                        if (checkCount > 4)
                        {
                            HelperClass.openAddEditModal(this, "postPoneModal");
                            HelperClassGeneric.openWarningModal(this, "Only 4 machines can be postponed at a time");
                            return;
                        }
                        Machines = (lvPostPoneGrid.Items[i].FindControl("lblPostMachineID") as Label).Text;
                        CurrentDate = Convert.ToDateTime((lvPostPoneGrid.Items[i].FindControl("lblActivityDate") as Label).Text);
                        if (CurrentDate.Equals(""))
                        {
                            HelperClass.openAddEditModal(this, "postPoneModal");
                            HelperClass.openWarningModal(this, "Current Date is Required");
                            return;
                        }
                        DateTime PostPoneDate ;
                        if (DateTime.TryParse(Convert.ToDateTime(txtPostPoneDate.Text).ToString("yyyy-MM-dd"), out PostPoneDate))
                        {
                            success = DataBaseAccessVulkan.SavePostPoneDate(Machines, CurrentDate, PostPoneDate, ddlPostPoneFrequency.SelectedValue.ToString());
                            if (success == "PostPoned")
                            {
                                HelperClass.clearModal(this);
                                HelperClass.openSuccessModal(this);
                            }
                            else
                            {
                                HelperClass.openAddEditModal(this, "postPoneModal");
                                HelperClass.openErrorModal(this, "Postpone Failed");
                                //HelperClass.clearModal(this);
                                return;
                            }
                        }
                        else
                        {
                            HelperClass.openAddEditModal(this, "postPoneModal");
                            HelperClass.openWarningModal(this, "wrong time format");
                            //HelperClass.clearModal(this);
                            return;
                        }
                    }
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPostPoneGrid()
        {
            List<PostPoneGridData> list = DataBaseAccessVulkan.GetPostPoneInfo(ddlFrequency.SelectedValue.ToString());
            lvPostPoneGrid.DataSource = list;
            lvPostPoneGrid.DataBind();
        }
        protected void btnPostPone_Click(object sender, EventArgs e)
        {
            try
            {
                txtPostPoneDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPostPoneGrid();
                HelperClass.openAddEditModal(this, "postPoneModal");
                //HelperClass.openModal(this, "postPoneModal", false);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlFrequency.SelectedValue == "Daily" || ddlFrequency.SelectedValue == "15Days")
                {
                    tdMonth.Visible = true;
                    tdMonthValue.Visible = true;
                }
                else
                {
                    tdMonth.Visible = false;
                    tdMonthValue.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}