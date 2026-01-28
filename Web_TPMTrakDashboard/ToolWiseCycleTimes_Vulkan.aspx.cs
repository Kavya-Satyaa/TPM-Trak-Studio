using ChartDirector;
using Elmah;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using ModelClassLibrary;
using BusinessClassLibrary;
using System.Drawing;
using System.Configuration;
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
using OfficeOpenXml2;
using OfficeOpenXml2.Drawing.Chart;
using OfficeOpenXml2.Style;

namespace Web_TPMTrakDashboard
{
    public partial class ToolWiseCycleTimes_Vulkan : System.Web.UI.Page
    {
        public int fontSize = 20;
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
                ToolWiseChart toolWiseChart = new ToolWiseChart();
                List<ToolWiseVulkan> list = VDGDataBaseAccess.GetToolWises_Vulkan(lblMachine.Text, lblCycleStartTime.Text, lblCycleEndTime.Text, out toolWiseChart);
                lvToolWise.DataSource = list;
                lvToolWise.DataBind();
                Session["ToolWiseVulkan"] = list;
                Session["ToolWiseChart"] = toolWiseChart;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ToolWiseChart GetToolData()
        {
            ToolWiseChart list = new ToolWiseChart();
            try
            {
                list = HttpContext.Current.Session["ToolWiseChart"] as ToolWiseChart;
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return list;
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
                if (Session["ToolWiseVulkan"] == null)
                {
                    return;
                }
                List<ToolWiseVulkan> list = Session["ToolWiseVulkan"] as List<ToolWiseVulkan>;
                string templatefile = string.Empty;
                string Filename = "ToolWiseCycleTime_Vulkan.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ToolWiseCycleTime_Vulkan_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                string timeformat = DataBaseAccess.getTimeFormatFromCockpit();
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Tool Wise Cycle Time- \n " + Source);
                }
                if (list.Count > 0)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var excelworksheet = Excel.Workbook.Worksheets[0];
                    excelworksheet.Cells["B3"].Value = lblMachine.Text;
                    excelworksheet.Cells["D3"].Value = lblCycleStartTime.Text;
                    excelworksheet.Cells["F3"].Value = lblCycleEndTime.Text;
                    int rowCount = 6;
                    int startRow = 25;
                    for (int i = 0; i < list.Count; i++)
                    {
                        excelworksheet.Cells[startRow, 1].Value = list[i].slNo;
                        excelworksheet.Cells[startRow, 2].Value = list[i].Tool;
                        excelworksheet.Cells[startRow, 3].Value = list[i].StartTime;
                        excelworksheet.Cells[startRow, 4].Value = list[i].EndTime;
                        //excelworksheet.Cells[startRow, 4].Value = list[i].Ideal;
                        TPMTrakGenerateReportNewDll.setTimeSpanFormat(timeformat, excelworksheet,startRow, 5,list[i].Ideal);
                        //excelworksheet.Cells[startRow, 5].Value = list[i].Actual;
                        TPMTrakGenerateReportNewDll.setTimeSpanFormat(timeformat, excelworksheet, startRow, 6, list[i].Actual);
                        startRow++;
                    }
                    int chartstartRow = 6;
                    for (int i = 0; i < list.Count; i++)
                    {
                        excelworksheet.Cells[5, 7, chartstartRow, 9].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                        excelworksheet.Cells[chartstartRow, 8].Value = Convert.ToDouble(list[i].ActualToolUsage)/86400;
                        excelworksheet.Cells[chartstartRow, 8].Style.Numberformat.Format = "[h]:mm:ss";
                        excelworksheet.Cells[chartstartRow, 9].Value = list[i].Tool;
                        
                        excelworksheet.Cells[chartstartRow, 7].Value = i + 1+"-"+ list[i].Tool;
                        chartstartRow++;
                    }

                    ExcelBarChart barChart = excelworksheet.Drawings.AddChart("barChart", eChartType.ColumnClustered) as ExcelBarChart;
                    barChart.Title.Text = "";
                    barChart.YAxis.Format = "[h]:mm:ss;@";
                    barChart.DataLabel.ShowValue = true;
                    
                    barChart.Legend.Remove();
                    barChart.XAxis.Title.Text = "Tools";
                    barChart.YAxis.Title.Text = "Time(hh:mm:ss)";
                    barChart.YAxis.Title.Rotation = 270;
                    barChart.SetPosition(4, 0, 0, 0);
                    barChart.SetSize(1600, 350);
                    
                    var chartSeries =(ExcelBarChartSerie) barChart.Series.Add(ExcelRange.GetAddress(6, 8, chartstartRow - 1, 8), ExcelRange.GetAddress(6, 7, chartstartRow - 1, 7));
                    
                    TPMTrakGenerateReportNewDll.chartDataLabelVerticle270(chartSeries, 0);
                    chartSeries.Header = "Tool";
                    setBorderThin(excelworksheet, 25, 1, startRow - 1, 6);
                    for (int i = 1; i <= 8; i++)
                    {
                        excelworksheet.Column(i).AutoFit();
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