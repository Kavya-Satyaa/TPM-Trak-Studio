using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Vulkan.Models;

namespace Web_TPMTrakDashboard.Vulkan
{
    public partial class PMReport_Vulkan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachines();
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MMM");
                BindGrid();
            }
        }
        private void BindMachines()
        {
            try
            {
                List<string> list = DataBaseAccessVulkan.GetMachinesForPlanningSheet();
                //list.Insert(0, "All");
                lbMachineID.DataSource = list;
                lbMachineID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindGrid()
        {
            try
            {
                DataTable dt2 = new DataTable();
                string Machines = "";
                //if (lbMachineID.SelectedValue.ToString().Equals("Select All", StringComparison.OrdinalIgnoreCase))
                //{
                //    Machines = "";
                //}
                //else
                //{
                Machines = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                //}
                DataTable dt = DataBaseAccessVulkan.GetPMReportData(Machines, ddlFrequency.SelectedValue.ToString(), Convert.ToInt32(txtYear.Text), Convert.ToInt32(HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text)), out dt2);
                int i = 0;
                List<PMReportData> MainList = new List<PMReportData>();
                if (dt.Rows.Count > 0)
                {
                    List<string> list_Holiday = new List<string>();
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt2.Rows)
                            list_Holiday.Add(Convert.ToDateTime(row["Holiday"].ToString()).ToString("dd"));
                    }
                    PMReportData Data = new PMReportData();
                    List<PMReportInnerData> FrequencyList = new List<PMReportInnerData>();
                    PMReportInnerData FrequencyData = new PMReportInnerData();
                    if (i == 0)
                    {
                        for (int j = 17; j < dt.Columns.Count; j++)
                        {
                            FrequencyData = new PMReportInnerData();
                            FrequencyData.Day = dt.Columns[j].ColumnName.ToString();
                            FrequencyData.HeaderVisibility = true;
                            FrequencyList.Add(FrequencyData);
                        }
                        //Data.SlNo = "Sl.No";
                        Data.MachineID = "Machine ID";
                        Data.CheckPoint = "CHECK POINT";
                        Data.Requirement = "REQUIREMENT";
                        Data.Action = "OBSERVATION";
                        Data.FrequencyWiseValues = FrequencyList;
                        Data.HeaderVisibility = true;
                        MainList.Add(Data);
                    }
                    var distMachines = dt.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct().ToList();
                    foreach (string Machine in distMachines)
                    {
                        var distCheckpoints = dt.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine, StringComparison.OrdinalIgnoreCase)).Select(k => k["CheckpointID"].ToString()).Distinct().ToList();
                        foreach (string Checkpoint in distCheckpoints)
                        {
                            DataRow firstRow = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase) && k["CheckpointID"].ToString().Equals(Checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            Data = new PMReportData();
                            Data.MachineID = Machine;
                            Data.SlNo = ++i;
                            Data.CheckPoint = firstRow["CheckpointItem"].ToString();
                            Data.Requirement = firstRow["Requirement"].ToString();
                            Data.Action = firstRow["Observation"].ToString();
                            Data.ContentVisibility = true;
                            Data.Department = firstRow["Department"].ToString();
                            Data.DocNo = firstRow["DocNo"].ToString();
                            Data.IssueDate = firstRow["IssueDate"].ToString();
                            Data.RevDate = firstRow["RevDate"].ToString();
                            Data.RevNo = firstRow["RevNo"].ToString();
                            string method = firstRow["Method"].ToString();
                            if (!string.IsNullOrEmpty(method))
                            {
                                var arr = method.Split(',').ToList();

                                if (arr.Count > 0)
                                {
                                    foreach (var a in arr)
                                    {
                                        Data.MethodList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                    }
                                }
                            }
                            string Instrument = firstRow["Instrument"].ToString().Trim();
                            if (!string.IsNullOrEmpty(Instrument))
                            {
                                var arr = Instrument.Split(',').ToList();

                                if (arr.Count > 0)
                                {
                                    foreach (var a in arr)
                                    {
                                        Data.InstrumentList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                    }
                                }
                            }
                            FrequencyList = new List<PMReportInnerData>();
                            for (int j = 17; j < dt.Columns.Count; j++)
                            {
                                FrequencyData = new PMReportInnerData();

                                FrequencyData.Value = string.IsNullOrEmpty(firstRow[dt.Columns[j].ColumnName].ToString()) ? "" : (firstRow[dt.Columns[j].ColumnName].ToString().Equals("OK", StringComparison.OrdinalIgnoreCase) ? "&#10004;" : "&#10008;"); // row[column].ToString();
                                FrequencyData.CssClass = string.IsNullOrEmpty(firstRow[dt.Columns[j].ColumnName].ToString()) ? "" : (firstRow[dt.Columns[j].ColumnName].ToString().Equals("Ok", StringComparison.OrdinalIgnoreCase) ? "green" : "red");


                                //FrequencyData.Value = (firstRow[dt.Columns[j].ColumnName].ToString());
                                FrequencyData.ContentVisibility = true;
                                FrequencyList.Add(FrequencyData);
                            }
                            Data.FrequencyWiseValues = FrequencyList;
                            MainList.Add(Data);
                        }
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "No data Found");
                }
                lvPMReport.DataSource = MainList;
                lvPMReport.DataBind();
                Session["Data"] = MainList;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt2 = new DataTable();
                string Machines = "";
                Machines = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachineID);
                DataTable dt = DataBaseAccessVulkan.GetPMReportData(Machines, ddlFrequency.SelectedValue.ToString(), Convert.ToInt32(txtYear.Text), Convert.ToInt32(HelperClassGeneric.getMonthNumberFromAbbMonthName(txtMonth.Text)), out dt2);
                List<PMReportData> MainList = new List<PMReportData>();

                if (dt.Rows.Count > 0 && dt.Columns.IndexOf("SaveFlag") == -1)
                {
                    PMReportData Data = new PMReportData();
                    List<PMReportInnerData> FrequencyList = new List<PMReportInnerData>();
                    PMReportInnerData FrequencyData = new PMReportInnerData();
                    var distMachines = dt.AsEnumerable().Select(x => x.Field<string>("MachineID")).Distinct().ToList();
                    int i = 0;

                    if (rdnreportType.SelectedValue.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                    {
                        string Template = "PreventiveMaintenanceReport_Vulkan" + DateTime.Now;
                        Template = SafeFileName(Template);

                        if (appPath == "")
                        {
                            appPath = HttpContext.Current.Server.MapPath("~/Docs");
                        }

                        string path = appPath;
                        // Must have write permissions to the path folder
                        if (!System.IO.Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        path = Path.Combine(path, Template + ".pdf");

                        Document doc = new Document(PageSize.A3.Rotate());
                        doc.SetMargins(2, 2, 10, 10);

                        FileStream fileStream = new FileStream(path, FileMode.CreateNew);
                        PdfWriter writer = PdfWriter.GetInstance(doc, fileStream);
                        doc.Open();
                        int ValueCol = dt.Columns.Count - 17;

                        PdfPTable pTable = new PdfPTable(6 + ValueCol);

                        PdfPCell headerCell = new PdfPCell();
                        try
                        {

                            foreach (string Machine in distMachines)
                            {
                                DataRow Row = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                                headerCell = new PdfPCell();
                                headerCell.Phrase = new Phrase(" ");
                                headerCell.Image = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(@"~/Vulkan/Logo/Vulkan_MS_Logo.png")), null, false);
                                headerCell.Padding = 10;
                                headerCell.Rowspan = 3;
                                pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase("PREVENTIVE MAINTENANCE REPORT", FontFactory.GetFont("Calibri", 16, iTextSharp.text.Font.BOLD)));
                                headerCell.Colspan = (3 + ValueCol);
                                headerCell.Rowspan = 3;
                                headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                headerCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                                pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase("Dept.:", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Row["Department"].ToString(), FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("DOC No.:", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Row["DocNo"].ToString(), FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Machine Desc.:", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Machine, FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("VULKAN TECHNOLOGIES PVT. LTD.\nPIRANGUT, PUNE.", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD)));
                                headerCell.Colspan = 2;
                                headerCell.Rowspan = 3;
                                pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase(" "));
                                headerCell.Colspan = (2 + ValueCol);
                                pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase("Issue Date:", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Convert.ToDateTime(Row["IssueDate"].ToString()).ToString("yyyy-MM-dd"), FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase(" "));
                                headerCell.Colspan = (2 + ValueCol);
                                pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase("Revision No.:", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Row["RevNo"].ToString(), FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase(" "));
                                headerCell.Colspan = (2 + ValueCol);
                                pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase("Revision Date:", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Convert.ToDateTime(Row["RevDate"].ToString()).ToString("yyyy-MM-dd"), FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);

                                headerCell = new PdfPCell(new Phrase("Machine No.:-", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Machine)); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Frequency:-", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(ddlFrequency.SelectedValue.ToString())); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Responsibility:-", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase(Row["Responsibilty"].ToString())); pTable.AddCell(headerCell);
                                for (int j = 0; j < ValueCol; j++)
                                {
                                    headerCell = new PdfPCell(); pTable.AddCell(headerCell);
                                }

                                headerCell = new PdfPCell(new Phrase("Sl. No.", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Checkpoint", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Requirement", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Method", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Instrument", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                headerCell = new PdfPCell(new Phrase("Observation", FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);


                                for (int j = 17; j < dt.Columns.Count; j++)
                                {
                                    headerCell = new PdfPCell(new Phrase(dt.Columns[j].ColumnName, FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD))); pTable.AddCell(headerCell);
                                }

                                var distCheckpoints = dt.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine, StringComparison.OrdinalIgnoreCase)).Select(k => k["CheckpointID"].ToString()).Distinct().ToList();

                                foreach (string Checkpoint in distCheckpoints)
                                {
                                    DataRow firstRow = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase) && k["CheckpointID"].ToString().Equals(Checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                                    headerCell = new PdfPCell(new Phrase((++i).ToString(), FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.NORMAL))); pTable.AddCell(headerCell);
                                    headerCell = new PdfPCell(new Phrase(firstRow["CheckpointItem"].ToString(), FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.NORMAL))); pTable.AddCell(headerCell);
                                    headerCell = new PdfPCell(new Phrase(firstRow["Requirement"].ToString(), FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.NORMAL))); pTable.AddCell(headerCell);

                                    headerCell = new PdfPCell();
                                    string PharseMethod = "";
                                    string method = firstRow["Method"].ToString();
                                    if (!string.IsNullOrEmpty(method))
                                    {
                                        var arr = method.Split(',').ToList();
                                        ViewState["MethodImageCount"] = arr.Count;
                                        if (arr.Count > 0)
                                        {
                                            foreach (var a in arr)
                                            {
                                                Data = new PMReportData();
                                                Data.MethodList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                                List<MetInsEntity> methodimglist = Data.MethodList;
                                                int imgNo = 1;
                                                foreach (var img in methodimglist)
                                                {
                                                    if (img.imgInByte != null)
                                                    {

                                                        using (MemoryStream imageStream = new MemoryStream(img.imgInByte))
                                                        {
                                                            System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream);
                                                            int cellWidth = 150;
                                                            int cellHeight = 70;
                                                            int imageWidth = image.Width;
                                                            int imageHeight = image.Height;

                                                            double widthRatio = (double)cellWidth / imageWidth;
                                                            double heightRatio = (double)cellHeight / imageHeight;

                                                            double scaleFactor = Math.Min(widthRatio, heightRatio);

                                                            int newImageWidth = (int)(imageWidth * scaleFactor);
                                                            int newImageHeight = (int)(imageHeight * scaleFactor);
                                                            using (System.Drawing.Image resizedImage = new Bitmap(image, newImageWidth, newImageHeight))
                                                            {
                                                                headerCell.Image = iTextSharp.text.Image.GetInstance(resizedImage, null, false);
                                                                headerCell.Padding = 10;
                                                            }
                                                        }
                                                        PharseMethod = "ImageFound";
                                                    }
                                                    else
                                                    {
                                                        if (imgNo == 1)
                                                            PharseMethod = " ";
                                                    }
                                                    imgNo++;
                                                }
                                                if (!string.IsNullOrEmpty(PharseMethod.Trim()))
                                                    headerCell.CalculatedHeight = (80 * imgNo);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PharseMethod = " ";
                                    }

                                    if (string.IsNullOrEmpty(PharseMethod.Trim()))
                                        headerCell.Phrase = new Phrase(PharseMethod);

                                    pTable.AddCell(headerCell);

                                    headerCell = new PdfPCell();
                                    string phraseInstrument = "";
                                    string Instrument = firstRow["Instrument"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(Instrument))
                                    {
                                        var arr = Instrument.Split(',').ToList();
                                        ViewState["InstrumentImageCount"] = arr.Count;
                                        if (arr.Count > 0)
                                        {
                                            foreach (var a in arr)
                                            {
                                                Data.InstrumentList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                                List<MetInsEntity> instrumentimglist = Data.InstrumentList;
                                                int imageNo = 1;
                                                foreach (var img in instrumentimglist)
                                                {
                                                    if (img.imgInByte != null)
                                                    {
                                                        using (System.IO.MemoryStream imageStream = new System.IO.MemoryStream(img.imgInByte))
                                                        {
                                                            System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream);

                                                            int cellWidth = 150;
                                                            int cellHeight = 70;
                                                            int imageWidth = image.Width;
                                                            int imageHeight = image.Height;

                                                            double widthRatio = (double)cellWidth / imageWidth;
                                                            double heightRatio = (double)cellHeight / imageHeight;

                                                            double scaleFactor = Math.Min(widthRatio, heightRatio);

                                                            int newImageWidth = (int)(imageWidth * scaleFactor);
                                                            int newImageHeight = (int)(imageHeight * scaleFactor);
                                                            using (System.Drawing.Image resizedImage = new Bitmap(image, newImageWidth, newImageHeight))
                                                            {
                                                                headerCell.Image = iTextSharp.text.Image.GetInstance(resizedImage, null, false);
                                                                headerCell.Padding = 10;
                                                            }
                                                        }
                                                        phraseInstrument = "ImageFound";
                                                    }
                                                    else
                                                    {
                                                        if (imageNo == 1)
                                                            phraseInstrument = " ";
                                                    }
                                                    imageNo++;
                                                }
                                                if (!string.IsNullOrEmpty(phraseInstrument.Trim()))
                                                    headerCell.CalculatedHeight = (80 * imageNo);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        phraseInstrument = " ";
                                    }

                                    if (string.IsNullOrEmpty(phraseInstrument.Trim()))
                                        headerCell.Phrase = new Phrase(phraseInstrument);

                                    pTable.AddCell(headerCell);

                                    headerCell = new PdfPCell(new Phrase(firstRow["Observation"].ToString(), FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.NORMAL)));
                                    pTable.AddCell(headerCell);

                                    for (int j = 17; j < dt.Columns.Count; j++)
                                    {
                                        headerCell = new PdfPCell();
                                        //headerCell.Phrase = new Phrase((string.IsNullOrEmpty(firstRow[dt.Columns[j].ColumnName].ToString()) ? " " : firstRow[dt.Columns[j].ColumnName].ToString()), FontFactory.GetFont("Calibri", 12, iTextSharp.text.Font.ITALIC));
                                        if (firstRow[dt.Columns[j].ColumnName].ToString().Equals("OK", StringComparison.OrdinalIgnoreCase) || firstRow[dt.Columns[j].ColumnName].ToString().Equals("Done", StringComparison.OrdinalIgnoreCase))
                                        {
                                            headerCell.Image = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(@"~/Vulkan/Logo/tick_1.png")), null, false);
                                        }
                                        else if (firstRow[dt.Columns[j].ColumnName].ToString().Equals("Not OK", StringComparison.OrdinalIgnoreCase) || firstRow[dt.Columns[j].ColumnName].ToString().Equals("Not Done", StringComparison.OrdinalIgnoreCase))
                                        {
                                            headerCell.Image = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(@"~/Vulkan/Logo/cross_1.png")), null, false);
                                        }

                                        pTable.AddCell(headerCell);
                                    }

                                }

                                doc.Add(pTable);
                                doc.NewPage();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog("Error generating PDF: " + ex.Message);
                        }
                        finally
                        {
                            doc.Close();
                        }

                        string isDownloaded = DownloadPDF(path, Template);
                        if (isDownloaded.Equals("Downloaded", StringComparison.OrdinalIgnoreCase))
                            HelperClassGeneric.openSuccessModal(this, "Downloaded Successfully");
                    }
                    else if (rdnreportType.SelectedValue.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                    {
                        string Filename = "PreventiveMaintenanceReport_Vulkan.xlsx";
                        string Source = GetReportPath(Filename);
                        string Template = "PreventiveMaintenanceReport_Vulkan" + DateTime.Now + ".xlsx";
                        string Destination = Path.Combine(appPath, "Temp", SafeFileName(Template));

                        if (!File.Exists(Source))
                        {
                            Logger.WriteDebugLog("Template does not found at - \n " + Source);
                        }
                        else
                        {
                            FileInfo newFile = new FileInfo(Source);
                            ExcelPackage Excel = new ExcelPackage(newFile, true);
                            var copyWorksheet = Excel.Workbook.Worksheets["sheet1"];
                            foreach (string Machine in distMachines)
                            {
                                var distCheckpoints = dt.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(Machine, StringComparison.OrdinalIgnoreCase)).Select(k => k["CheckpointID"].ToString()).Distinct().ToList();
                                var worksheet = Excel.Workbook.Worksheets.Add(Machine, copyWorksheet);
                                int startrow = 10;
                                worksheet.Cells["B8"].Value = Machine;
                                worksheet.Cells["D8"].Value = ddlFrequency.SelectedValue.ToString();
                                worksheet.Cells["J3"].Value = Machine;
                                foreach (string Checkpoint in distCheckpoints)
                                {
                                    DataRow firstRow = dt.AsEnumerable().Where(k => k["MachineID"].ToString().Equals(Machine, StringComparison.OrdinalIgnoreCase) && k["CheckpointID"].ToString().Equals(Checkpoint, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    worksheet.Cells["J1"].Value = firstRow["Department"].ToString();
                                    worksheet.Cells["J2"].Value = firstRow["DocNo"].ToString();
                                    worksheet.Cells["J4"].Value = Convert.ToDateTime(firstRow["IssueDate"].ToString()).ToString("yyyy-MM-dd");
                                    worksheet.Cells["J5"].Value = firstRow["RevNo"].ToString();
                                    worksheet.Cells["J6"].Value = Convert.ToDateTime(firstRow["RevDate"].ToString()).ToString("yyyy-MM-dd");
                                    worksheet.Cells["G8"].Value = firstRow["Responsibilty"].ToString();
                                    int colstart = 1;
                                    worksheet.Cells[startrow, colstart].Value = ++i;
                                    colstart++;
                                    worksheet.Cells[startrow, colstart].Value = firstRow["CheckpointItem"].ToString();
                                    colstart++;
                                    worksheet.Cells[startrow, colstart].Value = firstRow["Requirement"].ToString();
                                    colstart++;
                                    string method = firstRow["Method"].ToString();
                                    if (!string.IsNullOrEmpty(method))
                                    {
                                        var arr = method.Split(',').ToList();
                                        ViewState["MethodImageCount"] = arr.Count;
                                        if (arr.Count > 0)
                                        {
                                            foreach (var a in arr)
                                            {
                                                Data = new PMReportData();
                                                Data.MethodList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                                List<MetInsEntity> methodimglist = Data.MethodList;
                                                int imgNo = 1;
                                                foreach (var img in methodimglist)
                                                {
                                                    if (img.imgInByte != null)
                                                    {
                                                        using (System.IO.MemoryStream imageStream = new System.IO.MemoryStream(img.imgInByte))
                                                        {
                                                            System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream);
                                                            var drawing = worksheet.Drawings.AddPicture(imgNo + i + "_" + startrow, image);
                                                            //drawing.SetSize(8);
                                                            drawing.SetPosition(startrow - 1, 10, colstart - 1, 10);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[startrow, colstart].Value = "";
                                                    }
                                                    imgNo++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        worksheet.Cells[startrow, colstart].Value = "";
                                    }
                                    colstart++;
                                    string Instrument = firstRow["Instrument"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(Instrument))
                                    {
                                        var arr = Instrument.Split(',').ToList();
                                        ViewState["InstrumentImageCount"] = arr.Count;
                                        if (arr.Count > 0)
                                        {
                                            foreach (var a in arr)
                                            {
                                                Data.InstrumentList.Add(DataBaseAccessVulkan.GetMethodImageData(a.Trim()));
                                                List<MetInsEntity> instrumentimglist = Data.InstrumentList;
                                                int imageNo = 1;
                                                foreach (var img in instrumentimglist)
                                                {
                                                    if (img.imgInByte != null)
                                                    {
                                                        using (System.IO.MemoryStream imageStream = new System.IO.MemoryStream(img.imgInByte))
                                                        {
                                                            System.Drawing.Image image = System.Drawing.Image.FromStream(imageStream);
                                                            int cellWidth = 150;
                                                            int cellHeight = 70;
                                                            int imageWidth = image.Width;
                                                            int imageHeight = image.Height;

                                                            double widthRatio = (double)cellWidth / imageWidth;
                                                            double heightRatio = (double)cellHeight / imageHeight;

                                                            double scaleFactor = Math.Min(widthRatio, heightRatio);

                                                            int newImageWidth = (int)(imageWidth * scaleFactor);
                                                            int newImageHeight = (int)(imageHeight * scaleFactor);
                                                            using (System.Drawing.Image resizedImage = new Bitmap(image, newImageWidth, newImageHeight))
                                                            {
                                                                var drawing = worksheet.Drawings.AddPicture(imageNo + "_" + startrow, resizedImage);
                                                                drawing.SetPosition(startrow - 1, 10, colstart - 1, 10);
                                                            }
                                                            //var drawing = worksheet.Drawings.AddPicture(imageNo + "_" + startrow, image);
                                                            //drawing.SetPosition(startrow - 1, 10, colstart - 1, 10);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[startrow, colstart].Value = "";
                                                    }
                                                    imageNo++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        worksheet.Cells[startrow, colstart].Value = "";
                                    }
                                    colstart++;
                                    worksheet.Cells[startrow, colstart].Value = firstRow["Observation"].ToString();
                                    colstart++;
                                    for (int j = 17; j < dt.Columns.Count; j++)
                                    {
                                        if (startrow == 10)
                                        {
                                            worksheet.Cells[9, colstart].Value = dt.Columns[j].ColumnName.ToString();
                                        }
                                        if (firstRow[dt.Columns[j].ColumnName].ToString().Equals("OK", StringComparison.OrdinalIgnoreCase) || firstRow[dt.Columns[j].ColumnName].ToString().Equals("Done", StringComparison.OrdinalIgnoreCase))
                                        {
                                            worksheet.Cells[startrow, colstart].Style.Font.Name = "Wingdings";
                                            worksheet.Cells[startrow, colstart].Style.Font.Size = 28;
                                            worksheet.Cells[startrow, colstart].Style.Font.Bold = true;
                                            worksheet.Cells[startrow, colstart].Style.Font.Color.SetColor(Color.FromArgb(44, 140, 41));
                                            worksheet.Cells[startrow, colstart].Value = "ü";
                                        }
                                        else if (firstRow[dt.Columns[j].ColumnName].ToString().Equals("Not OK", StringComparison.OrdinalIgnoreCase) || firstRow[dt.Columns[j].ColumnName].ToString().Equals("Not Done", StringComparison.OrdinalIgnoreCase))
                                        {
                                            worksheet.Cells[startrow, colstart].Style.Font.Name = "Wingdings";
                                            worksheet.Cells[startrow, colstart].Style.Font.Size = 28;
                                            worksheet.Cells[startrow, colstart].Style.Font.Bold = true;
                                            worksheet.Cells[startrow, colstart].Style.Font.Color.SetColor(Color.FromArgb(255, 0, 0));
                                            worksheet.Cells[startrow, colstart].Value = "û";
                                        }
                                        else
                                        {
                                            worksheet.Cells[startrow, colstart].Value = (firstRow[dt.Columns[j].ColumnName].ToString());
                                        }
                                        colstart++;
                                    }
                                    startrow++;
                                    for (int rowIndex = 10; rowIndex <= startrow - 1; rowIndex++)
                                    {
                                        worksheet.Row(rowIndex).Height = 75;
                                    }
                                    //if (Convert.ToInt32(ViewState["InstrumentImageCount"])> Convert.ToInt32(ViewState["MethodImageCount"]))
                                    //{
                                    //    worksheet.Cells[startrow, colstart, startrow + Convert.ToInt32(ViewState["InstrumentImageCount"]), colstart].Merge = true;
                                    //}
                                    //else
                                    //{
                                    //    worksheet.Cells[startrow, colstart, startrow + Convert.ToInt32(ViewState["MethodImageCount"]), colstart].Merge = true;
                                    //}
                                    setThinBorder(worksheet, 9, 1, startrow - 1, colstart - 1);
                                }
                            }
                            Excel.Workbook.Worksheets.Delete(1);
                            DownloadMultipleFile(Destination, Excel.GetAsByteArray());
                        }
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "No Data Found");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        public static string DownloadPDF(string Path, string Name)
        {
            string success = "";
            try
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", $"attachment; filename={Name}.pdf");
                    HttpContext.Current.Response.BinaryWrite(bytes);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.Headers.Clear();
                    success = "Downloaded";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return success;
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
    }
}