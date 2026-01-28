using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.MachineConnect.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.MachineConnect
{
    public partial class ProgramUploadDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string FromDate = DateTime.Now.AddDays(-6).ToString();
                txtFromDate.Text = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(FromDate)).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                BindMachineID();
                btnView_Click(null, null);
            }
        }

        private void BindProgramNo()
        {
            try
            {
                string MachineId = HelperClassGeneric.getListboxSelectedValue(lbMachineID);
                string FromDate = Util.GetDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                string Todate = Util.GetDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                List<string> ProgramLst = MachineConnectDBAccess.GetProgramForMachinedate(MachineId, FromDate, Todate);
                lbProgramNo.DataSource = ProgramLst;
                lbProgramNo.DataBind();

                foreach (System.Web.UI.WebControls.ListItem item in lbProgramNo.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindMachineID()
        {
            try
            {
                List<string> machineLst= MachineConnectDBAccess.GetAllMachines(string.Empty);
                lbMachineID.DataSource = machineLst;
                lbMachineID.DataBind();

                foreach(System.Web.UI.WebControls.ListItem item in lbMachineID.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindProgramNo();
                BindData(string.Empty);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindData(string ProgramNo)
        {
            try
            {
                string MachineID = HelperClassGeneric.getListboxSelectedValue(lbMachineID);
                string FromDate = Util.GetDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                string Todate = Util.GetDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                List<ProgramUploadHistoryEntity> list = MachineConnectDBAccess.GetProgramUploadHistoryDetails(MachineID, ProgramNo, FromDate, Todate);
                lvProgramUpload.DataSource = list;
                lvProgramUpload.DataBind();

                Session["ProgramUploadHistory"] = list;
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
                List<ProgramUploadHistoryEntity> list = Session["ProgramUploadHistory"] as List<ProgramUploadHistoryEntity>;
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 20, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfWriter.PageEvent = new HeaderEventHandler();
                pdfDoc.Open();

                PdfPTable mainDataTbl = new PdfPTable(1);
                mainDataTbl.SplitLate = false;
                mainDataTbl.WidthPercentage = 100;
                mainDataTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                mainDataTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                mainDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Program Upload History Report", 13,false)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                PdfPTable headerpdfPTable = new PdfPTable(2);
                //headerpdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine: " + HelperClassGeneric.getListboxSelectedValue(lbMachineID), 10)));
                headerpdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("From Date: " + Util.GetDateTime(txtFromDate.Text).ToString("dd-MM-yyyy HH:mm:ss"), 10,false)));
                headerpdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("To Date: " + Util.GetDateTime(txtToDate.Text).ToString("dd-MM-yyyy HH:mm:ss"), 10,false)));
                //headerpdfPTable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Program No: " + HelperClassGeneric.getListboxSelectedValue(lbProgramNo), 10,false)));
                mainDataTbl.AddCell(new PdfPCell(headerpdfPTable) { HorizontalAlignment = Element.ALIGN_LEFT });

                if (list.Count>0)
                {
                    PdfPTable temptable = new PdfPTable(4);
                    int[] gridCellWidth = new int[4];

                    temptable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine", 10,true)));
                    temptable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Uploaded Program No", 10,true)));
                    temptable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Employee", 10,true)));
                    temptable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Uploaded TS", 10,true)));
                    mainDataTbl.AddCell(new PdfPCell(temptable) { HorizontalAlignment = Element.ALIGN_CENTER });

                    foreach (ProgramUploadHistoryEntity item in list)
                    {
                        temptable = new PdfPTable(4);
                        temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(item.MachineId, 10)));
                        temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(item.ProgramNo, 10)));
                        temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(item.Employee, 10)));
                        temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(Util.GetDateTime(item.UpdatedTS).ToString("dd-MM-yyyy HH:mm:ss tt"), 10)));
                        mainDataTbl.AddCell(new PdfPCell(temptable) { HorizontalAlignment = Element.ALIGN_CENTER,Border=0 });
                    }
                }
                pdfDoc.Add(mainDataTbl);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ProgramUploadHistoryReport .pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.Flush();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        public class HeaderEventHandler : PdfPageEventHelper
        {
            private bool isFirstPage = true;
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                if (!isFirstPage)
                {

                    PdfPTable header = new PdfPTable(4);
                    header.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                    header.DefaultCell.Border = 0;

                    // Add header cells
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine", 10, true)));
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Uploaded Program No", 10, true)));
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Employee", 10, true)));
                    header.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Uploaded TS", 10, true)));
                    // Set the position of the header
                    header.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + header.TotalHeight, writer.DirectContent);
                }
                isFirstPage = false;
            }
        }
        private static PdfPCell getPdfCellWithBoldHeader(string value, int size,bool Status)
        {
            iTextSharp.text.Font font = FontFactory.GetFont("Calibri (Body)", 13, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
            Chunk chunk = new Chunk(value, font);
            chunk.Font.Color = Status?BaseColor.WHITE :new iTextSharp.text.BaseColor(10, 10, 10);
            chunk.Font.Size = size;
            Phrase phrase = new Phrase();
            phrase.Add(chunk);
            PdfPCell cell = new PdfPCell(phrase);
            cell.ExtraParagraphSpace = 3;
            cell.BorderColor = BaseColor.BLACK;
            if (Status)
                cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#006987"));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
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

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindProgramNo();
        }

        protected void btnProgramExportView_Click(object sender, EventArgs e)
        {
            string ProgramNo = HelperClassGeneric.getListboxSelectedValue(lbProgramNo);
            BindData(ProgramNo);
        }
    }
}