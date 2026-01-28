using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ParameterDetailsLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text =Convert.ToDateTime( VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text)).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                BindMachineID();
                BindParameterDetails();
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> list = DataBaseAccess.getmachineidfromplant("");
                list.Remove("All");
                lbMachineID.DataSource = list;
                lbMachineID.DataBind();
                foreach (System.Web.UI.WebControls.ListItem item in lbMachineID.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineID: " + ex.Message);
            }
        }

        private void BindParameterDetails()
        {
            try
            {
                List<DashboardChangeEntity> list = new List<DashboardChangeEntity>();
                DataTable dt = new DataTable();

                string machineID = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                string programNo = DataBaseAccess.getMachineIDWithSeparator(lbMultiProgramNo);

                list = DataBaseAccess.GetIncidentChangeData(machineID, txtFromDate.Text.Trim(), txtToDate.Text.Trim());
                gvParameterChanges.DataSource = list;
                gvParameterChanges.DataBind();

                //string machineID = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                if (!string.IsNullOrEmpty(machineID))
                {
                    lbMultiProgramNo.DataSource = DataBaseAccess.GetProgramNoforMachine(machineID, txtFromDate.Text.Trim(), txtToDate.Text.Trim());
                    lbMultiProgramNo.DataBind();
                    foreach (System.Web.UI.WebControls.ListItem item in lbMultiProgramNo.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    lbMultiProgramNo.DataSource = new List<string>();
                    lbMultiProgramNo.DataBind();
                }

                 


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindProgramChanges: " + ex.ToString());
            }
        }



        private static PdfPCell getPdfCellWithBoldHeader(string value, int size)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = size;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = BaseColor.BLACK;
            return cell;
        }
        private static PdfPCell getPdfCellWithoutBoldText(string value, int size)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 10);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = size;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = BaseColor.BLACK;
            return cell;
        }

        public class HeaderEventHandler : PdfPageEventHelper
        {
            private bool isFirstPage = true;
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                if (!isFirstPage)
                {

                    PdfPTable header = new PdfPTable(5);
                    header.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                    header.DefaultCell.Border = 0;

                    // Add header cells
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Program No. ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Previous Data ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Changed Data ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Changed Time", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    // Set the position of the header
                    header.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + header.TotalHeight, writer.DirectContent);
                }
                isFirstPage = false;
            }
        }

         

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string FromAndToDateValidation(string fromDate, string toDate)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {

                    if (HelperClassGeneric.GetDateTime(fromDate) > HelperClassGeneric.GetDateTime(toDate))
                    {
                        result = "Greater";
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("FromAndToDateValidation: " + ex.Message);
            }
            return result;
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                Session["MachineID"] = lbMachineID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : lbMachineID.SelectedValue;
                Session["FromDate"] = txtFromDate.Text;
                Session["ToDate"] = txtToDate.Text;

                BindParameterDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 20, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfWriter.PageEvent = new HeaderEventHandler();


                pdfDoc.Open();


                string machineID = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                string programNo = DataBaseAccess.getMachineIDWithSeparator(lbMultiProgramNo);
                List<DashboardChangeEntity> list = DataBaseAccess.GetIncidentChangeDataForProgramNos(machineID, programNo, txtFromDate.Text.Trim(), txtToDate.Text.Trim());
                PdfPTable mainDataTbl = new PdfPTable(1);

                var distinctMachines = dt.AsEnumerable().Select(x => x["MachineID"].ToString()).Distinct().ToList();

                if (machineID == "")
                    machineID = "All";

                if (list != null && list.Count > 0)
                {
                    Paragraph p1 = new Paragraph();
                    pdfDoc.Add(p1);
                    pdfDoc.NewPage();
                    mainDataTbl = new PdfPTable(1);
                    mainDataTbl.SplitLate = false;
                    mainDataTbl.WidthPercentage = 100;
                    mainDataTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainDataTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    mainDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Program Changes Report", 13)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    PdfPTable formateheaderTbl = new PdfPTable(3);

                    formateheaderTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine : " + machineID, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    formateheaderTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("From Date : " + txtFromDate.Text, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    formateheaderTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("To Date : " + txtToDate.Text, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    mainDataTbl.AddCell(new PdfPCell(formateheaderTbl) { HorizontalAlignment = Element.ALIGN_CENTER });


                    PdfPTable formateDataTbl = new PdfPTable(5);
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Program No. ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Previous Data ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Changed Data ", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Changed Time", 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });


                    mainDataTbl.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_CENTER });


                    foreach (DashboardChangeEntity data in list)
                    {
                        formateDataTbl = new PdfPTable(5);
                        formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(data.MachineID, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(data.programID, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        //formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(data.PreviousData, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        //formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(data.ChangedData, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        AddStyledCell(formateDataTbl, data.PreviousData.Replace("<br />", "\n"));
                        AddStyledCell(formateDataTbl, data.ChangedData.Replace("<br />", "\n"));

                        //var xx = Convert.ToDateTime(data.changedTime.ToString()).ToString("dd-MM-yyyy hh:mm:ss tt");
                        formateDataTbl.AddCell(new PdfPCell(getPdfCellWithoutBoldText(data.changedTime, 10)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        mainDataTbl.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    }
                    pdfDoc.Add(mainDataTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=ProgramDashboardReport .pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.Flush();
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, "No data found");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click : " + ex.Message);
            }
        }
        private void AddStyledCell(PdfPTable table, string htmlContent)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph paragraph = new Paragraph();

            string pattern = @"<span class=""(\w+)"">(.*?)</span>|([^<]+)";
            MatchCollection matches = Regex.Matches(htmlContent, pattern);

            foreach (Match match in matches)
            {
                if (match.Groups[3].Success) // Plain text
                {
                    paragraph.Add(new Chunk(match.Groups[3].Value, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)));
                }
                else // Styled span
                {
                    string className = match.Groups[1].Value;
                    string content = match.Groups[2].Value;
                    Font font = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);

                    switch (className)
                    {
                        case "classDeletion":
                            font.Color = BaseColor.RED;
                            break;
                        case "classChanged":
                            font.Color = BaseColor.BLUE;
                            break;
                        case "classAddition":
                            font.Color = BaseColor.GREEN;
                            break;
                    }

                    paragraph.Add(new Chunk(content, font));
                }
            }

            cell.AddElement(paragraph);
            table.AddCell(cell);
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string machineID = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
                if (!string.IsNullOrEmpty(machineID))
                {
                    lbMultiProgramNo.DataSource = DataBaseAccess.GetProgramNoforMachine(machineID, txtFromDate.Text.Trim(), txtToDate.Text.Trim());
                    lbMultiProgramNo.DataBind();
                    foreach (System.Web.UI.WebControls.ListItem item in lbMultiProgramNo.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    lbMultiProgramNo.DataSource = new List<string>();
                    lbMultiProgramNo.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnRefresh_Click" + ex.Message);
            }
        }

        protected void btnProgramView_Click(object sender, EventArgs e)
        {
            List<DashboardChangeEntity> list = new List<DashboardChangeEntity>();
            DataTable dt = new DataTable();
             
            string machineID = DataBaseAccess.getMachineIDWithSeparator(lbMachineID);
            string programNo = DataBaseAccess.getMachineIDWithSeparator(lbMultiProgramNo);

            if (!string.IsNullOrEmpty(machineID) && !string.IsNullOrEmpty(programNo))
            {
                list = DataBaseAccess.GetIncidentChangeDataForProgramNos(machineID, programNo, txtFromDate.Text.Trim(), txtToDate.Text.Trim());
            }
            else
            {
                lbMultiProgramNo.DataSource = new List<string>();
                lbMultiProgramNo.DataBind();
            } 
            gvParameterChanges.DataSource = list;
            gvParameterChanges.DataBind();
        }
    }
}