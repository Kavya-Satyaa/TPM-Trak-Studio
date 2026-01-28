using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Pitti
{
    public partial class Reports_Pitti : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MM");
                BindMachines();
                BindCategory();
                trMonth.Visible = false;
                //trCategory.Visible = false;
            }
        }
        private void BindMachines()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllMachinesFromMaster();
                lbMachineID.DataSource = list;
                lbMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCategory()
        {
            try
            {
                List<string> list = DataBaseAccess.GetCategories();
                lbCategory.DataSource = list;
                lbCategory.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string MachineID = "";string Category = "";
                if (ddlReportType.SelectedValue.ToString() == "PMChecklistReport")
                {
                    string templatefile = string.Empty;
                    string FileName = "PMChecklistReport_Pitti.xlsx";
                    string Source = string.Empty;
                    Source = Util.GetReportPath(FileName);
                    string Template = "PMChecklistReport_Pitti" + DateTime.Now + ".xlsx";
                    string destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                    if (!File.Exists(Source))
                    {
                        HelperClassGeneric.openErrorModal(this, "Template does not found");
                        return;
                    }
                    if (lbMachineID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                        MachineID = "";
                    else
                        MachineID = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachineID);
                    if (lbCategory.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                        Category = "";
                    else
                        Category = HelperClassGeneric.getListBoxValueWithCommaSeparator(lbCategory);
                    DataTable dt2 = null;
                    DataTable dt3 = null;
                    DataTable dt4 = null;
                    DataTable dt1 = DataBaseAccess.GetPMChecksheetReportDetails(txtYear.Text, ddlFrequency.SelectedValue, MachineID, Category, out dt2, out dt3, out dt4);
                    List<PMReportEntity> list = new List<PMReportEntity>();
                    var distMachines = dt1.AsEnumerable().Select(k => k["MachineID"].ToString()).Distinct().ToList();

                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var Copyworksheet = Excel.Workbook.Worksheets[1];
                    int a = 0;
                    if (dt1.Rows.Count > 0)
                    {
                        for (int machinec = 0; machinec < distMachines.Count; machinec++)
                        {
                            int startRow = 9, startCol = 1;
                            var worksheet = Excel.Workbook.Worksheets.Add(distMachines[machinec], Copyworksheet);
                            var distCategory = dt1.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(distMachines[machinec], StringComparison.OrdinalIgnoreCase)).Select(k => k["Category"].ToString()).Distinct().ToList();
                            PMReportEntity headerdata = new PMReportEntity();
                            bool hasValueInRange = false;
                            for (int categoryC = 0; categoryC < distCategory.Count; categoryC++)
                            {
                                DataTable dtCategory = dt1.AsEnumerable().Where(k => k["Category"].ToString().Equals(distCategory[categoryC], StringComparison.OrdinalIgnoreCase) && k["MachineID"].ToString().Equals(distMachines[machinec], StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                                DataTable dtCategory1 = dt2.AsEnumerable().Where(k => k["Category"].ToString().Equals(distCategory[categoryC], StringComparison.OrdinalIgnoreCase) && k["MachineID"].ToString().Equals(distMachines[machinec], StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                                var distCheckpoint = dtCategory.AsEnumerable().Select(k => k["SerialNo"].ToString()).Distinct().ToList();
                                PMReportEntity mainData = new PMReportEntity();

                                startCol = 1;
                                worksheet.Cells[startRow - 1, 1, startRow - 1, ((dt1.Columns.Count - 10) * 2) + 5].Merge = true;
                                worksheet.Cells[startRow - 1, startCol].Value = distCategory[categoryC];
                                worksheet.Cells[startRow - 1, 1, startRow - 1, ((dt1.Columns.Count - 10) * 2) + 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[startRow - 1, 1, startRow - 1, ((dt1.Columns.Count - 10) * 2) + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                                worksheet.Cells[startRow - 1, 1, startRow - 1, ((dt1.Columns.Count - 10) * 2) + 5].Style.Font.Bold.ToString();
                                worksheet.Cells[startRow - 1, 1, startRow - 1, ((dt1.Columns.Count - 10) * 2) + 5].Style.Font.Bold = true;
                                worksheet.Cells[startRow - 1, 1, startRow - 1, ((dt1.Columns.Count - 10) * 2) + 5].Style.Font.Size = 15;
                                foreach (string checkpoint in distCheckpoint)
                                {
                                    var firstRow = dtCategory.AsEnumerable().Where(k => k["SerialNo"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    var firstRow1 = dtCategory1.AsEnumerable().Where(k => k["SerialNo"].ToString().Equals(checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    startCol = 1;
                                    worksheet.Cells[startRow, startCol].Value = firstRow["SerialNo"].ToString();
                                    startCol++;
                                    worksheet.Cells[startRow, startCol].Value = firstRow["CheckpointDescription"].ToString();
                                    startCol++;
                                    worksheet.Cells[startRow, startCol].Value = firstRow["JudgementCriteria"].ToString();
                                    startCol++;
                                    worksheet.Cells[startRow, startCol].Value = firstRow["ResourcesNeeded"].ToString();
                                    startCol++;
                                    worksheet.Cells[startRow, startCol].Value = firstRow["Duration"].ToString();
                                    startCol++;
                                    //worksheet.Cells[startRow, startCol].Value = firstRow["Category"].ToString();
                                    //startCol++;

                                    for (int i = 10; i < dtCategory.Columns.Count; i++)
                                    {
                                        if (startRow == 9)
                                        {
                                            string[] splitValues = dtCategory.Columns[i].ColumnName.ToString().Split('[');
                                            worksheet.Cells[startRow - 2, startCol, startRow - 2, startCol + 1].Merge = true;
                                            worksheet.Cells[startRow - 2, startCol].Value = splitValues[0];
                                            //worksheet.Cells[startRow - 2, startCol + 1].Value = "ATT";
                                            //startCol++;
                                        }
                                        worksheet.Cells[startRow, startCol, startRow, startCol + 1].Merge = true;
                                        worksheet.Cells[startRow, startCol].Value = firstRow[i].ToString();
                                        startCol = startCol + 2;

                                        //worksheet.Cells[startRow, startCol].Value = "";
                                        //startCol++;
                                    }
                                    startRow++;
                                }
                                startRow++;
                            }
                            worksheet.Cells[startRow - 1, 1, startRow + 5, ((dt1.Columns.Count - 10) * 2) + 5].Style.Font.Bold.ToString();
                            worksheet.Cells[startRow - 1, 1, startRow + 5, ((dt1.Columns.Count - 10) * 2) + 5].Style.Font.Bold = true;
                            worksheet.Cells[startRow, 3, startRow, 4].Merge = true;
                            worksheet.Cells[startRow, 3].Value = "Total time required:";
                            double sum = 0;
                            for (int row = 9; row <= startRow - 2; row++)
                            {
                                double value;
                                if (double.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out value))
                                {
                                    sum += value;
                                }
                            }
                            worksheet.Cells[startRow, 5].Value = sum;

                            startRow++;
                            if (dt3.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    if (dt3.Rows.Count > 0 && dt3.Rows[i]["MachineID"].ToString() == distMachines[machinec])
                                    {
                                        worksheet.Cells["B4"].Value = dt3.Rows[i]["MachineID"].ToString();
                                        worksheet.Cells["F3"].Value = dt3.Rows[i]["PreparedByName"].ToString();
                                        worksheet.Cells["G3"].Value = dt3.Rows[i]["CheckedByName"].ToString();
                                        worksheet.Cells["H3"].Value = dt3.Rows[i]["ApprovedByName"].ToString();
                                        worksheet.Cells["J2"].Value = dt3.Rows[i]["RefNo"].ToString();
                                        worksheet.Cells["J3"].Value = dt3.Rows[i]["PageNo"].ToString();
                                        worksheet.Cells["J4"].Value = dt3.Rows[i]["RevNo"].ToString();
                                        startCol = 6;
                                        for (int j = 10; j < dt1.Columns.Count; j++)
                                        {
                                            for (int row = 8; row < startRow; row++)
                                            {
                                                if (worksheet.Cells[row, startCol].Value == "" || worksheet.Cells[row, startCol].Value == null)
                                                {
                                                    hasValueInRange = false;
                                                }
                                                else
                                                {
                                                    hasValueInRange = true;
                                                    break;
                                                }
                                            }
                                            if (hasValueInRange)
                                            {
                                                worksheet.Cells[startRow, startCol, startRow, startCol + 1].Merge = true;
                                                //worksheet.Cells[startRow, startCol].Value = Session["UserName"].ToString();
                                                worksheet.Cells[startRow, startCol].Value = dt3.Rows[i]["OperatorName"].ToString();
                                                worksheet.Cells[startRow + 1, startCol, startRow + 1, startCol + 1].Merge = true;
                                                worksheet.Cells[startRow + 1, startCol].Value = dt3.Rows[i]["SupervisorName"].ToString();
                                                worksheet.Cells[startRow + 2, startCol, startRow + 2, startCol + 1].Merge = true;
                                                worksheet.Cells[startRow + 2, startCol].Value = dt3.Rows[i]["MaintenanceManagerName"].ToString();
                                                if (dt4.Rows.Count > i && dt4.Rows[i]["MachineID"].ToString() == distMachines[machinec] && a < dt4.Rows.Count)
                                                {
                                                    worksheet.Cells[startRow - 2, startCol].Style.WrapText = true;
                                                    worksheet.Cells[startRow - 2, startCol].Value = "PM starts at:" + dt4.Rows[a]["ActivityStartTS"].ToString();
                                                    worksheet.Cells[startRow - 1, startCol].Style.WrapText = true;
                                                    worksheet.Cells[startRow - 1, startCol].Value = "PM ends at:" + dt4.Rows[a]["ActivityEndTS"].ToString();
                                                    worksheet.Cells[startRow - 2, startCol + 1, startRow - 1, startCol + 1].Merge = true;
                                                    worksheet.Cells[startRow - 2, startCol + 1].Style.WrapText = true;
                                                    worksheet.Cells[startRow - 2, startCol + 1].Value = "Total Time:-" + dt4.Rows[a]["TimeTaken"].ToString();
                                                    a++;
                                                }
                                                else
                                                {
                                                    worksheet.Cells[startRow - 2, startCol].Value = "PM starts at:";
                                                    worksheet.Cells[startRow - 1, startCol].Value = "PM ends at:";
                                                    worksheet.Cells[startRow - 2, startCol + 1, startRow - 1, startCol + 1].Merge = true;
                                                    worksheet.Cells[startRow - 2, startCol + 1].Value = "Total Time:-";
                                                }
                                            }
                                            startCol = startCol + 2;

                                            //for (int rowIndex = 8; rowIndex <= startRow; rowIndex++)
                                            //{
                                            //    for(int k = 4; k < dt2.Columns.Count; k++)
                                            //    {
                                            //        if (worksheet.Cells[rowIndex, startCol].Value==""|| worksheet.Cells[rowIndex, startCol].Value==null)
                                            //        {

                                            //        }
                                            //        else
                                            //        {
                                            //            hasValueInRange = true;
                                            //            break;
                                            //        }
                                            //        if (hasValueInRange)
                                            //        {
                                            //            worksheet.Cells[startRow, startCol].Value = Session["UserName"].ToString();
                                            //            worksheet.Cells[startRow + 1, startCol].Value = dt3.Rows[i]["SupervisorName"].ToString();
                                            //            worksheet.Cells[startRow + 2, startCol].Value = dt3.Rows[i]["MaintenanceManagerName"].ToString();
                                            //            for(int a = 0; a < dt4.Rows.Count; a++)
                                            //            {
                                            //                worksheet.Cells[startRow - 2, startCol].Value = "PM starts at:" + dt4.Rows[a]["ActivityStartTS"].ToString();
                                            //                worksheet.Cells[startRow - 1, startCol].Value = "PM ends at:"+dt4.Rows[a]["ActivityEndTS"].ToString();
                                            //            }
                                            //            //startCol++;
                                            //        }
                                            //        startCol++;
                                            //    }

                                            //}
                                            //if (hasValueInRange)
                                            //{
                                            //    worksheet.Cells[startRow, startCol].Value = Session["UserName"].ToString();
                                            //    worksheet.Cells[startRow + 1, startCol].Value = dt3.Rows[i]["SupervisorName"].ToString();
                                            //    worksheet.Cells[startRow + 2, startCol].Value = dt3.Rows[i]["MaintenanceManagerName"].ToString();
                                            //    worksheet.Cells[startRow-2, startCol].Value = "PM starts at:";
                                            //    worksheet.Cells[startRow-1, startCol].Value = "PM ends at:";
                                            //    startCol++;
                                            //}
                                        }
                                    }
                                }
                            }
                            worksheet.Cells[startRow, 3, startRow, 4].Merge = true;
                            worksheet.Cells[startRow, 3].Value = "Engineer: ";
                            startRow++;
                            worksheet.Cells[startRow, 3, startRow, 4].Merge = true;
                            worksheet.Cells[startRow, 3].Value = "Supervisor:";
                            startRow++;
                            worksheet.Cells[startRow, 3, startRow, 4].Merge = true;
                            worksheet.Cells[startRow, 3].Value = "Maint Manager:";
                            startRow++;
                            setBorderThin(worksheet, 7, 1, startRow - 1, startCol - 1);
                        }
                        Excel.Workbook.Worksheets.Delete(1);
                        DownloadFile(destination, Excel.GetAsByteArray());
                    }
                    else
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "No Data Found");
                    }
                }
                else if (ddlReportType.SelectedValue.ToString() == "DailyChecklistReport")
                {
                   
                    if (lbMachineID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                        MachineID = "";
                    else
                        MachineID = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);

                    string res = GenerateReport_Pitti.GenerateDailyChecksheetReport_Pitti(MachineID, txtYear.Text.Trim(), txtMonth.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        static string appPath = HttpContext.Current.Server.MapPath("");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "Reports/TPMTrakReport", reportName);
            return src;
        }
        private static void DownloadFile(string filename, byte[] bytearray)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filename) + "\"");
            HttpContext.Current.Response.OutputStream.Write(bytearray, 0, bytearray.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }
        private void setBorderThin(ExcelWorksheet sheet, int fromRow, int fromcol, int toRow, int toCol)
        {
            var modelTable3 = sheet.Cells[fromRow, fromcol, toRow, toCol];
            modelTable3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlReportType.SelectedValue.ToString() == "PMChecklistReport")
                {
                    trCategory.Visible = true;
                    trMonth.Visible = false;
                }
                else if (ddlReportType.SelectedValue.ToString() == "DailyChecklistReport")
                {
                    trCategory.Visible = false;
                    trMonth.Visible = true;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}