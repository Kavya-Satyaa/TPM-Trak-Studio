using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ToolWiseCycleTimes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["MachineID"] != "")
                {
                    if (Request.QueryString["MachineID"].ToString() == "" || Request.QueryString["StartTime"].ToString() == "" || Request.QueryString["EndTime"].ToString() == "")
                    {
                        Response.Redirect("VDGScreen.aspx", false);
                    }
                    lblMachine.Text = Request.QueryString["MachineID"].ToString();
                    lblCompID.Text = Request.QueryString["CompID"].ToString();
                    lblOpnNum.Text = Request.QueryString["OprNum"].ToString();
                    lblSlno.Text = Request.QueryString["Slno"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["StartTime"].ToString()))
                    {
                        lblCycleStartTime.Text = Util.GetDateTime(Request.QueryString["StartTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["EndTime"].ToString()))
                    {
                        lblCycleEndTime.Text = Util.GetDateTime(Request.QueryString["EndTime"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    }
                    BindData();
                }
                else Response.Redirect("VDGScreen.aspx", false);
            }
        }
        private void BindData()
        {
            try
            {
                List<ToolWiseCycleTimeData> list = VDGDataBaseAccess.getVDGToolWiseCycleTimeDetails(lblMachine.Text, lblCompID.Text, lblOpnNum.Text, lblCycleStartTime.Text, lblCycleEndTime.Text);
                //if (list.Count > 0)
                //{
                //	list[0].Visibility = "table-cell";
                //	list[0].RowSpan = list.Count.ToString();
                //}
                lvToolCycleTime.DataSource = list;
                lvToolCycleTime.DataBind();
                Session["ToolWiseCycleTimeData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        static string appPath = HttpContext.Current.Server.MapPath("");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "Reports/ReportTemplates", reportName);
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ToolWiseCycleTimeData"] == null)
                {
                    return;
                }
                List<ToolWiseCycleTimeData> list = Session["ToolWiseCycleTimeData"] as List<ToolWiseCycleTimeData>;
                string templatefile = string.Empty;
                string Filename = "ToolWiseCycleTime.xlsx";

                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ToolWiseCycleTime_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Tool Wise Cycle Time- \n " + Source);
                }
                if (list.Count > 0)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var exelworksheet = Excel.Workbook.Worksheets[1];
                    exelworksheet.Cells["B3"].Value = lblMachine.Text;
                    exelworksheet.Cells["D3"].Value = lblCompID.Text;
                    exelworksheet.Cells["F3"].Value = lblOpnNum.Text;
                    exelworksheet.Cells["B4"].Value = lblSlno.Text;
                    exelworksheet.Cells["D4"].Value = lblCycleStartTime.Text;
                    exelworksheet.Cells["F4"].Value = lblCycleEndTime.Text;
                    int rowCount = 7;
                    int startRow = rowCount;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == list.Count - 1)
                        {
                            exelworksheet.Row(rowCount).Style.Font.Bold = true;
                            exelworksheet.Cells[rowCount, 1, rowCount, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            exelworksheet.Cells[rowCount, 1, rowCount, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 213, 180));
                            exelworksheet.Cells[rowCount, 1, rowCount, 8].Style.Numberformat.Format = "[h]:mm:ss";
                            exelworksheet.Cells[rowCount, 1].Value = "Total";
                        }
                        else
                        {
                            exelworksheet.Cells[rowCount, 1].Value = list[i].CycleStartTime;
                            exelworksheet.Cells[rowCount, 2].Value = list[i].CycleEndTime;
                            exelworksheet.Cells[rowCount, 3].Value = list[i].ToolNumber;
                        }
                        exelworksheet.Cells[rowCount, 4].Value = list[i].ToolTimeTS;
                        exelworksheet.Cells[rowCount, 5].Value = list[i].OperatingTimeTS;
                        exelworksheet.Cells[rowCount, 6].Value = list[i].CuttingTimeTS;
                        exelworksheet.Cells[rowCount, 7].Value = list[i].NonCuttingTimeTS;
                        exelworksheet.Cells[rowCount, 4, rowCount, 7].Style.Numberformat.Format = "[h]:mm:ss";
                        exelworksheet.Cells[rowCount, 8].Value = list[i].ProgramBlock;
                        rowCount++;
                    }
                    //if (list.Count>0)
                    //{
                    //    exelworksheet.Cells[7, 1, rowCount - 1, 1].Merge = true;
                    //    exelworksheet.Cells[7, 2, rowCount - 1, 2].Merge = true;
                    //}
                    setBorderThin(exelworksheet, 6, 1, rowCount - 1, 8);
                    for (int i = 1; i <= 8; i++)
                    {
                        exelworksheet.Column(i).AutoFit();
                    }
                    DownloadFile(destination, Excel.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void setBorderThin(ExcelWorksheet sheet, int fromRow, int fromcol, int toRow, int toCol)
        {
            var modelTable3 = sheet.Cells[fromRow, fromcol, toRow, toCol];
            modelTable3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

    }
}