using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.util;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.EnergyModule.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class InspectionTransaction : System.Web.UI.Page
    {
        static NetworkConnection nc = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPlant();
                BindShift();
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                footermainDiv.Visible = true;
                footerdiv.Visible = false;
                linetd.Visible = false;
                drawingtd.Visible = false;
                shifttd.Visible = false;
                maindiv.Visible = true;
                
                if(!string.IsNullOrEmpty(ddlMachine.SelectedValue) && !string.IsNullOrEmpty(ddlComponent.SelectedValue))
                {
                    DataTable dt = DBAccess.GetInspectionTransactionDetails(ddlMachine.SelectedValue.ToString(), ddlComponent.SelectedItem.Text, ddlOperation.SelectedValue.ToString(), Web_TPMTrakDashboard.Models.Util.GetDateTime(txtDate.Text), ddlShift.SelectedValue, out DataTable ApprovedDt, out DataTable QtyDt);
                    Session["ApprovedDetails"] = ApprovedDt;
                    Session["QtyDetails"] = QtyDt;
                    List<string> columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                    if (columnNames[0].Equals("SaveFlag"))
                    {
                        string message = dt.AsEnumerable().Select(x => x.Field<string>(columnNames[0])).FirstOrDefault();
                        if (message.Equals("No jobs", StringComparison.OrdinalIgnoreCase))
                        {
                            footermainDiv.Visible = false;
                            maindiv.Visible = false;
                            HelperClass.openWarningModal(this, "No Jobs Found!!");
                            lvInspectiondetails.DataSource = new List<InspectionEntity>();
                            lvInspectiondetails.DataBind();
                            return;
                        }
                    }
                    List<InspectionEntity> list = new List<InspectionEntity>();
                    InspectionEntity entContent = null;
                    InspectionEntity entHeader = new InspectionEntity();
                    int i = 0;
                    foreach (DataRow item in dt.Rows)
                    {
                        if (i == 0)
                        {
                            entHeader.HeaderVisibility = "table-cell";

                        }
                        entContent = new InspectionEntity();
                        entContent.ContentVisibility = "table-cell";
                        entContent.InspectionChar = item["CharacteristicID"].ToString();
                        entContent.SetValue = item["SetValue"].ToString();
                        entContent.LSL = item["LSL"].ToString();
                        entContent.USL = item["USL"].ToString();
                        entContent.Shift = item["ShiftName"].ToString();


                        List<string> cols = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                        InspectionEntity inspectionHeader = null;
                        InspectionEntity inspectionContent = null;
                        List<InspectionEntity> insHeaderlst = new List<InspectionEntity>();
                        List<InspectionEntity> insContentlst = new List<InspectionEntity>();
                        for (int k = 8; k < cols.Count; k++)
                        {
                            if (i == 0)
                            {
                                inspectionHeader = new InspectionEntity();
                                inspectionHeader.HeaderName = "Job " + cols[k].ToString();
                                inspectionHeader.JobHeaderVisibility = "table-cell";
                                inspectionHeader.HeaderColor = "#2e6886";
                                insHeaderlst.Add(inspectionHeader);
                            }
                            inspectionContent = new InspectionEntity();
                            string val = cols[k].ToString();
                            inspectionContent.HeaderName = "Job " + cols[k].ToString();
                            inspectionContent.JobValue = item[val].ToString();
                            inspectionContent.JobContentVisibility = "table-cell";
                            inspectionContent.HeaderColor = "transparent";
                            inspectionContent.DropdownVisibility = false;
                            inspectionContent.TextVisibility = false;
                            inspectionContent.NumericVisibility = false;
                            inspectionContent.Datatype = item["DataType"].ToString();
                            if (item["DataType"].ToString().Equals("Text"))
                            {
                                inspectionContent.TextVisibility = true;
                            }
                            else if (item["DataType"].ToString().Equals("Numeric"))
                            {
                                inspectionContent.NumericVisibility = true;
                            }
                            else
                            {
                                inspectionContent.DropdownVisibility = true;
                            }
                            insContentlst.Add(inspectionContent);
                        }
                        if (i == 0)
                        {
                            entHeader.HeaderList = insHeaderlst;
                            list.Add(entHeader);
                        }

                        entContent.HeaderList = insContentlst;
                        list.Add(entContent);
                        i++;
                    }
                    lvInspectiondetails.DataSource = list;
                    lvInspectiondetails.DataBind();
                    Session["InspectionDetails"] = dt;

                    if (ApprovedDt.Rows.Count > 0)
                    {
                        btnLineInspection.Enabled = true;
                        btnDrawing.Enabled = true;
                        btnSupervisor.Enabled = true;
                        footerdiv.Visible = true;
                        if (ApprovedDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>("LineInspectorID"))).Count() > 0)
                        {
                            linetd.Visible = true;
                            btnLineInspection.Enabled = false;
                            DateTime Date = ApprovedDt.AsEnumerable().Select(x => x.Field<dynamic>("LineInspectionTS")).FirstOrDefault();
                            string msg = Web_TPMTrakDashboard.Models.Util.GetDateTime(Date.ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            lblLineInspectorID.Text = ApprovedDt.AsEnumerable().Select(x => x.Field<string>("LineInspectorID")).FirstOrDefault() + " , " + msg;
                        }
                        if (ApprovedDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>("DrawingCheckedBy"))).Count() > 0)
                        {
                            btnDrawing.Enabled = false;
                            drawingtd.Visible = true;
                            DateTime Date = ApprovedDt.AsEnumerable().Select(x => x.Field<dynamic>("DrawingCheckedTS")).FirstOrDefault();
                            string msg = Web_TPMTrakDashboard.Models.Util.GetDateTime(Date.ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            lblDrawingCheckedBy.Text = ApprovedDt.AsEnumerable().Select(x => x.Field<string>("DrawingCheckedBy")).FirstOrDefault() + " , " + msg;
                        }
                        if (ApprovedDt.AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>("SupervisorID"))).Count() > 0)
                        {
                            btnSupervisor.Enabled = false;
                            shifttd.Visible = true;
                            DateTime Date = ApprovedDt.AsEnumerable().Select(x => x.Field<dynamic>("SupervisorTS")).FirstOrDefault();
                            string msg = Web_TPMTrakDashboard.Models.Util.GetDateTime(Date.ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            lblShiftSupervisor.Text = ApprovedDt.AsEnumerable().Select(x => x.Field<string>("SupervisorID")).FirstOrDefault() + " , " + msg;
                        }
                    }
                    prodQty.Text = QtyDt.AsEnumerable().Select(x => x.Field<string>("ProductedQty")).Distinct().FirstOrDefault();
                    rejQty.Text = QtyDt.AsEnumerable().Select(x => x.Field<string>("RejectionQty")).Distinct().FirstOrDefault();
                }
                else
                {
                    List<InspectionEntity> list = new List<InspectionEntity>();
                    lvInspectiondetails.DataSource = list;
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                list.RemoveAt(0);
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
                BindCellID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindCellID()
        {
            try
            {
                List<string> list = DBAccess.getCellAssignedForEmployee(ddlPlant.SelectedValue, Session["UserName"].ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();
                BindMachineID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindMachineID()
        {
            try
            {
                string cell = "";
                if (ddlCell.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (System.Web.UI.WebControls.ListItem item in ddlCell.Items)
                    {
                        if (cell == "")
                        {
                            cell += "'" + item.Value + "'";
                        }
                        else
                        {
                            cell += ",'" + item.Value + "'";
                        }
                    }
                }
                else
                {
                    cell = "'" + ddlCell.SelectedValue + "'";
                }
                List<System.Web.UI.WebControls.ListItem> list = DBAccess.getMachineIdInterfaceIDByPlantCell(ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue, cell, false);
                ddlMachine.DataSource = list;
                ddlMachine.DataTextField = "Text";
                ddlMachine.DataValueField = "Value";
                ddlMachine.DataBind();
                BindComponent();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindComponent()
        {
            try
            {
                List<System.Web.UI.WebControls.ListItem> Complst = DBAccess.getComponentIDWithInterface(ddlMachine.SelectedValue);
                ddlComponent.DataSource = Complst;
                ddlComponent.DataTextField = "Text";
                ddlComponent.DataValueField = "Value";
                ddlComponent.DataBind();
                Bindoperation(ddlComponent.SelectedItem.Text.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void Bindoperation(string Comp)
        {
            try
            {
                List<System.Web.UI.WebControls.ListItem> OpnLst = DBAccess.getOperationWithInterface(ddlMachine.SelectedValue, Comp);
                ddlOperation.DataSource = OpnLst;
                ddlOperation.DataTextField = "Text";
                ddlOperation.DataValueField = "Value";
                ddlOperation.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindShift()
        {
            try
            {
                List<string> allShifts = DataBaseAccess.GetAllShift();
                ddlShift.DataSource = allShifts;
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Session["InspectionDetails"] as DataTable;
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 20, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                PdfPTable mainDataTbl = new PdfPTable(1);
                mainDataTbl.SplitLate = false;
                mainDataTbl.WidthPercentage = 100;
                mainDataTbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                mainDataTbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                mainDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Inspection Report", 13)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                PdfPTable formateDataTbl = new PdfPTable(3);
                formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Machine Name: " + ddlMachine.SelectedValue, 10)) );
                formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Component ID: " + ddlComponent.SelectedItem.Text, 10)) { Colspan = 2 });
                formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Operation: " + ddlOperation.SelectedItem.Text,10)));
                formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Date: "+ Web_TPMTrakDashboard.Models.Util.GetDateTime(txtDate.Text).ToString("dd-MM-yyyy"),10)));
                formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Shift: " + ddlShift.SelectedValue,10)));
                mainDataTbl.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_LEFT });

                if (dt.Rows.Count>0)
                {
                    if(dt.Columns.Contains("DataType"))
                        dt.Columns.Remove("DataType");
                    if (dt.Columns.Contains("CharacteristicCode"))
                        dt.Columns.Remove("CharacteristicCode");
                    if (dt.Columns.Contains("SetValue"))
                        dt.Columns.Remove("SetValue");
                    if (dt.Columns.Contains("LSL"))
                        dt.Columns.Remove("LSL");
                    if (dt.Columns.Contains("USL"))
                        dt.Columns.Remove("USL");
                    if (dt.Columns.Contains("ShiftID"))
                        dt.Columns.Remove("ShiftID");
                    if (dt.Columns.Contains("ShiftName"))
                        dt.Columns.Remove("ShiftName");
                    int totalClumn = dt.Columns.Count;

                    formateDataTbl = new PdfPTable(totalClumn);
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Specified Reading", 10)) { BackgroundColor= BaseColor.LIGHT_GRAY});
                    formateDataTbl.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Actual Reading",10)) { Colspan = totalClumn-1, HorizontalAlignment=Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                    mainDataTbl.AddCell(new PdfPCell(formateDataTbl) { HorizontalAlignment = Element.ALIGN_MIDDLE, VerticalAlignment= Element.ALIGN_CENTER });

                    PdfPTable temptable = new PdfPTable(totalClumn);
                    int[] gridCellWidth = new int[totalClumn];

                    for (int i = 0; i < totalClumn; i++)
                    {
                        if (i == 0)
                        {
                            gridCellWidth[i] = 30;
                            continue;
                        }
                        if (i == 1 || i == 2 || i == 3 || i == 4 || i == 5)
                        {
                            gridCellWidth[i] = 30;
                            continue;
                        }
                    }
                    temptable.SetWidths(gridCellWidth);
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName == "CharacteristicID")
                        {
                            temptable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("",10)));
                        }
                        else
                        {
                            temptable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Job " +column.ColumnName,10)));
                        }
                    }

                    
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName == "CharacteristicID")
                        {
                            temptable.AddCell(new PdfPCell(getPdfCellWithBoldHeader("Job Checking Time:",10)));
                        }
                        else
                        {
                            temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("",10)));
                        }
                    }

                    PdfPTable Celltemptable = new PdfPTable(totalClumn);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            Celltemptable = new PdfPTable(1);
                            Celltemptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(dt.Rows[i][j].ToString(),10)) { Border = 0, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 14 });

                            temptable.AddCell(new PdfPCell(Celltemptable) { Padding = 4 });
                        }
                    }
                    mainDataTbl.AddCell(new PdfPCell(temptable) { Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    DataTable ApDt = Session["ApprovedDetails"] as DataTable;
                    DataTable QtyDt = Session["QtyDetails"] as DataTable;

                    temptable = new PdfPTable(totalClumn);
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("LINE INSPECTOR SIGN:", 10)));
                    string LineInspector = ApDt.AsEnumerable().Select(x => x.Field<string>("LineInspectorID")).Distinct().FirstOrDefault();
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(LineInspector, 10)) { Colspan = totalClumn - 1 });
                    mainDataTbl.AddCell(new PdfPCell(temptable) { Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    temptable = new PdfPTable(totalClumn);
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("TIME:", 10)));
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("", 10)) { Colspan = totalClumn - 1 });
                    mainDataTbl.AddCell(new PdfPCell(temptable) { Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    temptable = new PdfPTable(totalClumn);
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("DRAWING CHECKED BY:", 10)));
                    string drawing = ApDt.AsEnumerable().Select(x => x.Field<string>("DrawingCheckedBy")).Distinct().FirstOrDefault();
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(drawing, 10)) { Colspan = totalClumn - 1 });
                    mainDataTbl.AddCell(new PdfPCell(temptable) { Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    temptable = new PdfPTable(6);
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("OPERATOR NAME:", 10)));
                    string Operator = ApDt.AsEnumerable().Select(x => x.Field<string>("Operator")).Distinct().FirstOrDefault();
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(Operator, 10)) );
                    string Qty = QtyDt.AsEnumerable().Select(x => x.Field<string>("ProductedQty")).Distinct().FirstOrDefault();
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("QTY:", 10)));
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(Qty, 10)) );
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("REJECTION:", 10)));
                    string Rej = QtyDt.AsEnumerable().Select(x => x.Field<string>("RejectionQty")).Distinct().FirstOrDefault();
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(Rej, 10)));
                    mainDataTbl.AddCell(new PdfPCell(temptable) { Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    
                    temptable = new PdfPTable(4);
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("SHIFT SUPERVISOR SIGN:", 10)));
                    string Supervisor = ApDt.AsEnumerable().Select(x => x.Field<string>("SupervisorID")).Distinct().FirstOrDefault();
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(Supervisor, 10)));
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText("CYCLE TIME:", 10)));
                    string CycleTime = QtyDt.AsEnumerable().Select(x => x.Field<string>("CycleTime")).Distinct().FirstOrDefault();
                    temptable.AddCell(new PdfPCell(getPdfCellWithoutBoldText(CycleTime, 10)));
                    mainDataTbl.AddCell(new PdfPCell(temptable) { Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    pdfDoc.Add(mainDataTbl);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=InspectionReport .pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.Flush();
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
        }

        protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bindoperation(ddlComponent.SelectedItem.Text);
        }

        public class InspectionEntity
        {
            public int ID;
            public string Shift { get; set; }
            public string InspectionChar { get; set; }
            public string SetValue { get; set; }
            public string LSL { get; set; }
            public string USL { get; set; }
            public string Datatype { get; set; }
            public string Job1 { get; set; }
            public string Job2 { get; set; }
            public string Job3 { get; set; }
            public string Job4 { get; set; }
            public string Job5 { get; set; }
            public string HeaderVisibility { get; set; }
            public string ContentVisibility { get; set; }

            public List<InspectionEntity> HeaderList { get; set; }
            public string JobHeaderVisibility { get; set; }
            public string JobContentVisibility { get; set; }
            public string JobValue { get; set; }
            public string HeaderName { get; set; }
            public bool TextVisibility { get; set; }
            public bool DropdownVisibility { get; set; }
            public string HeaderColor { get; set; }
            public bool NumericVisibility { get; set; }
        }


        protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponent();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string success = string.Empty;
                bool isError = false;
                InspectionEntity data = null;
                for (int i = 1; i < lvInspectiondetails.Items.Count; i++)
                {
                    bool isSave = false;
                    string InspectionChar = (lvInspectiondetails.Items[i].FindControl("hfCharCode") as HiddenField).Value;
                    string Shift = (lvInspectiondetails.Items[i].FindControl("hfShift") as HiddenField).Value;
                    ListView listView = (lvInspectiondetails.Items[i].FindControl("lvJobDetails") as ListView);
                    foreach (ListViewItem listViewItem in listView.Items)
                    {
                        data = new InspectionEntity();
                        data.InspectionChar = InspectionChar;
                        data.Datatype = (listViewItem.FindControl("hfDataType") as HiddenField).Value;
                        data.HeaderName = (listViewItem.FindControl("hfJobHeader") as HiddenField).Value;
                        data.HeaderName = data.HeaderName.Replace("Job", "").Trim();
                        
                        if (data.Datatype.Equals("Text",StringComparison.OrdinalIgnoreCase))
                        {
                            data.JobValue= (listViewItem.FindControl("txtJobVal") as TextBox).Text;
                        }
                        else if(data.Datatype.Equals("Numeric", StringComparison.OrdinalIgnoreCase))
                        {
                            data.JobValue = (listViewItem.FindControl("txtnumJobVal") as TextBox).Text;
                        }
                        else
                        {
                            data.JobValue= (listViewItem.FindControl("ddlJob") as DropDownList).SelectedValue;
                        }
                        if ((listViewItem.FindControl("hfJobVal") as HiddenField).Value == data.JobValue)
                        {
                            continue;
                        }
                        success = DBAccess.SaveInspectionTransactionDetails(ddlMachine.SelectedValue.ToString(),ddlComponent.SelectedItem.Text,ddlOperation.SelectedValue.ToString(), Web_TPMTrakDashboard.Models.Util.GetDateTime(txtDate.Text), Shift, data);
                    }
                }
                if (success.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClass.openUpdateSuccessModal(this);
                }
                if (success == "")
                {
                    isError = true;
                    HelperClass.openWarningModal(this, "Data Not Saved!!");
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void lvInspectiondetails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if(e.Item.ItemType== ListViewItemType.DataItem)
                {
                    ListView listView = (e.Item.FindControl("lvJobDetails") as ListView);
                    foreach(ListViewItem item in listView.Items)
                    {
                        string value = (item.FindControl("hfJobVal") as HiddenField).Value;
                        (item.FindControl("ddlJob") as DropDownList).SelectedValue = value;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            BindShift();
            string btnName = (sender as Button).Text;
            string Shift = string.Empty;
            if(ddlShift.SelectedValue.ToString().Equals("All",StringComparison.OrdinalIgnoreCase))
            {
                List<string> ShiftIDs= ddlShift.DataSource as List<string>;
                ShiftIDs.RemoveAt(0);
                foreach(var item in ShiftIDs)
                {
                    Shift += item +",";
                }
                Shift = Shift.TrimEnd(',');
            }

            string msg = DBAccess.UpdateApproverDetails(ddlMachine.SelectedValue.ToString(), ddlComponent.SelectedItem.Text, ddlOperation.SelectedValue.ToString(), Web_TPMTrakDashboard.Models.Util.GetDateTime(txtDate.Text), Shift, ddlPlant.SelectedValue,ddlCell.SelectedValue,btnName);
            if(msg.Equals("Inserted",StringComparison.OrdinalIgnoreCase) || msg.Equals("Updated", StringComparison.OrdinalIgnoreCase))
            {
                HelperClass.openSuccessModal(this);
            }
            else
            {
                HelperClass.openErrorModal(this, "Data Not Inserted!!");
            }
            BindData();
        }

        protected void btnPDf_Click(object sender, EventArgs e)
        {
            try
            {
                string userame = System.Web.Configuration.WebConfigurationManager.AppSettings["KTAUserName"].ToString();
                string password = System.Web.Configuration.WebConfigurationManager.AppSettings["KTAPassword"].ToString();

                string RootPath = DBAccess.GetRootPath("RootPath");

                if (!string.IsNullOrEmpty(userame) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        nc = new NetworkConnection(RootPath, new NetworkCredential(userame, password));
                    }
                    catch (Exception ex)
                    {
                        if (nc != null)
                            nc.Dispose();
                        Logger.WriteErrorLog(ex.ToString());
                    }
                    finally
                    {
                        if (nc != null)
                            nc.Dispose();
                    }
                }

                string regSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex rg = new Regex(string.Format("[{0}]", Regex.Escape(regSearch)));

                string Comp = ddlComponent.SelectedItem.Text;
                Comp = rg.Replace(Comp, "");

                string OpDesc = DBAccess.GetOperationDesc(ddlComponent.SelectedItem.Text, ddlOperation.SelectedValue);
                OpDesc = rg.Replace(OpDesc, "");

                string Op = ddlOperation.SelectedValue + "_" + OpDesc;
                // Comp = Comp.Replace("/", "~");
                string path = Path.Combine(RootPath, Comp);
                if (path != null)
                {
                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(path, "Inspection", Op));
                    if (!dir.Exists)
                    {
                        HelperClass.openWarningModal(this, "Directory Not found");
                        return;
                    }
                    FileInfo[] files = dir.GetFiles("*.pdf").OrderBy(p => p.CreationTime).ToArray();
                    if (files.Length <= 0)
                    {
                        HelperClass.openWarningModal(this, "No PDF found");
                        return;
                    }
                    FileInfo LastUpdatedfile = dir.GetFiles("*.pdf").OrderByDescending(p => p.CreationTime).FirstOrDefault();
                    path = Path.Combine(path, "Inspection", Op, LastUpdatedfile.FullName);
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[(int)fs.Length];
                        fs.Read(bytes, 0, (int)fs.Length);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment; filename=" + LastUpdatedfile.Name);
                        Response.BinaryWrite(bytes);
                        Response.Flush();
                    }
                }

            }
            catch (Exception ex)
            {
                if (nc != null)
                    nc.Dispose();
                Logger.WriteErrorLog(ex.ToString());
                Web_TPMTrakDashboard.Models.HelperClassGeneric.openWarningToastrModal(this, "Network Connection Failed to Open");
            }
            finally
            {
                if (nc != null)
                    nc.Dispose();
            }
        }
    }
}