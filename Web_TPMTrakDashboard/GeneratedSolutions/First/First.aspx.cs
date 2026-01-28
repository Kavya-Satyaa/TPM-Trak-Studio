using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;

namespace Web_TPMTrakDashboard
{
    public partial class First : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss");
                BindGrid();
            }
            CreateDynamicColumns();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack) {
                foreach (string id in new[] { "Ddl1", "Ddl2", "Ddl3", "Ddl4", "Ddl5" }) {
                    Control ctrl = FindControlRecursive(Page, "ddl" + id) ?? FindControlRecursive(Page, "lb" + id);
                    if (ctrl is ListControl lc) {
                        if (id == "Ddl1" || id == "Ddl5") BindFilterControl(lc, id, "Filter", true);
                        else BindFilterControl(lc, id, "Filter", false, "");
                    }
                }
            } else {
                foreach (string id in new[] { "Ddl1", "Ddl2", "Ddl3", "Ddl4", "Ddl5" }) {
                    Control ctrl = FindControlRecursive(Page, "ddl" + id) ?? FindControlRecursive(Page, "lb" + id);
                    if (ctrl is ListControl lc) {
                        DataTable dt = Session["Items_" + id] as DataTable;
                        if (dt != null) {
                            lc.DataSource = dt; lc.DataBind();
                            if (lc is DropDownList) lc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                        }
                        RestoreSelection(lc, id);
                    }
                }
            }
        }

        private Control FindControlRecursive(Control root, string id)
        {
            if (root != null && root.ID == id) return root;
            foreach (Control child in root.Controls)
            {
                Control res = FindControlRecursive(child, id);
                if (res != null) return res;
            }
            return null;
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            var lbSender = sender as ListControl;
            if (lbSender == null) return;
            string id = lbSender.ID;
            string p = GetSelectedValues("Ddl1");
            string m = GetSelectedValues("Ddl2");
            string c = GetSelectedValues("Ddl3");

            if (id.EndsWith("Ddl1")) { 
                UpdateChild("Ddl2", p); 
                UpdateChild("Ddl3", GetSelectedValues("Ddl2"));
                UpdateChild("Ddl4", GetSelectedValues("Ddl2"), GetSelectedValues("Ddl3"));
            }
            else if (id.EndsWith("Ddl2")) { 
                UpdateChild("Ddl3", m);
                UpdateChild("Ddl4", m, GetSelectedValues("Ddl3"));
            }
            else if (id.EndsWith("Ddl3")) { 
                UpdateChild("Ddl4", m, c);
            }
        }

        private void UpdateChild(string id, string p1, string p2 = null)
        {
            Control ctrl = FindControlRecursive(Page, "ddl" + id) ?? FindControlRecursive(Page, "lb" + id);
            if (ctrl is ListControl lc) BindFilterControl(lc, id, "Filter", false, p1, p2);
        }

        private void BindFilterControl(ListControl lc, string id, string name, bool d, string p1 = null, string p2 = null)
        {
            DataTable dt;
            try {
                switch (id) {
                    case "Ddl1": dt = TPMStudioDBAccess.GetPlants(); break;
                    case "Ddl2": dt = TPMStudioDBAccess.GetMachines(p1); break;
                    case "Ddl3": dt = TPMStudioDBAccess.GetComponents(p1); break;
                    case "Ddl4": dt = TPMStudioDBAccess.GetOperations(p1, p2); break;
                    case "Ddl5": dt = TPMStudioDBAccess.GetShifts(); break;
                    default: dt = TPMStudioDBAccess.GetDummyData(); break;
                }
            } catch { dt = null; }
            if (dt == null || dt.Rows.Count == 0) {
                dt = new DataTable(); dt.Columns.Add("ID"); dt.Columns.Add("Name");
                for (int i = 1; i <= 3; i++) dt.Rows.Add(id + "_" + i, id + " Item " + i);
            }
            lc.DataSource = dt;
            if (dt.Columns.Count > 0) {
                lc.DataValueField = dt.Columns[0].ColumnName;
                lc.DataTextField = dt.Columns.Count > 1 ? dt.Columns[1].ColumnName : dt.Columns[0].ColumnName;
            }
            lc.DataBind();
            if (d && lc is DropDownList) lc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            Session["Items_" + id] = dt;
            RestoreSelection(lc, id);
        }

        private void RestoreSelection(ListControl lc, string id)
        {
            string val = GetPostBackValue(id);
            if (string.IsNullOrEmpty(val)) return;
            foreach (string s in val.Split(',')) {
                var itm = lc.Items.FindByValue(s);
                if (itm != null) itm.Selected = true;
            }
        }

        private string GetSelectedValues(string filterID)
        {
            Control ctrl = FindControlRecursive(Page, "ddl" + filterID) ?? FindControlRecursive(Page, "lb" + filterID);
            if (ctrl is ListControl lc)
            {
                var selected = lc.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(i => i.Selected && !string.IsNullOrEmpty(i.Value)).Select(i => i.Value).ToList();
                if (selected.Count > 0) return string.Join(",", selected);
            }
            return GetPostBackValue(filterID);
        }

        private string GetPostBackValue(string id)
        {
            if (Request.Form == null) return "";
            string key = Request.Form.AllKeys.FirstOrDefault(k => k != null && (k.EndsWith("$lb" + id) || k.EndsWith("$ddl" + id) || k.EndsWith("$" + id) || k == id));
            if (string.IsNullOrEmpty(key)) return "";
            var values = Request.Form.GetValues(key);
            if (values == null) return "";
            return string.Join(",", values.Where(v => !string.IsNullOrEmpty(v)));
        }

        private void CreateDynamicColumns()
        {
            if (gvReport.Columns.Count == 0)
            {
                gvReport.DataKeyNames = new string[] { "Module", "Machine" };
                var boundCol1 = new BoundField();
                boundCol1.HeaderText = "Module";
                boundCol1.DataField = "Module";
                gvReport.Columns.Add(boundCol1);
                var boundCol2 = new BoundField();
                boundCol2.HeaderText = "Machine";
                boundCol2.DataField = "Machine";
                gvReport.Columns.Add(boundCol2);
                var templateCol3 = new TemplateField();
                templateCol3.HeaderText = "Status";
                templateCol3.ItemTemplate = new DynamicTemplate_First("Status", "Checkbox");
                gvReport.Columns.Add(templateCol3);
                var templateCol4 = new TemplateField();
                templateCol4.HeaderText = "Value";
                templateCol4.ItemTemplate = new DynamicTemplate_First("Value", "TextField");
                gvReport.Columns.Add(templateCol4);
                var actionCol = new TemplateField();
                actionCol.HeaderText = "Action";
                actionCol.ItemTemplate = new DynamicTemplate_First("Action", "DeleteButton");
                actionCol.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvReport.Columns.Add(actionCol);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string title = "Second Attempt";
            string format = "PDF";
            BindGrid();
            if (format == "Excel") { ExportToExcel(title); } else { ExportToPDF(title); }
        }

        private void ExportToExcel(string title)
        {
            using (OfficeOpenXml.ExcelPackage pck = new OfficeOpenXml.ExcelPackage())
            {
                OfficeOpenXml.ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                int colCountVal = 0; List<int> visibleColIndices = new List<int>();
                for (int i = 0; i < gvReport.Columns.Count; i++)
                {
                    if (gvReport.Columns[i].Visible && gvReport.Columns[i].HeaderText != "Action")
                    { colCountVal++; visibleColIndices.Add(i); }
                }
                if (colCountVal > 0)
                {
                    ws.Cells[1, 1, 2, colCountVal].Merge = true;
                    ws.Cells[1, 1].Value = title + " REPORT";
                    ws.Cells[1, 1].Style.Font.Size = 16; ws.Cells[1, 1].Style.Font.Bold = true;
                    ws.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
                int excelCol = 1;
                foreach (int idx in visibleColIndices)
                {
                    ws.Cells[4, excelCol].Value = gvReport.Columns[idx].HeaderText;
                    ws.Cells[4, excelCol].Style.Font.Bold = true;
                    excelCol++;
                }
                int excelRow = 5;
                foreach (GridViewRow row in gvReport.Rows)
                {
                    excelCol = 1;
                    foreach (int colIdx in visibleColIndices)
                    {
                        string cellText = ""; Control ctrl = row.Cells[colIdx].Controls.Count > 0 ? row.Cells[colIdx].Controls[0] : null;
                        if (ctrl is Label) cellText = ((Label)ctrl).Text;
                        else if (ctrl is TextBox) cellText = ((TextBox)ctrl).Text;
                        else if (ctrl is CheckBox) cellText = ((CheckBox)ctrl).Checked ? "Yes" : "No";
                        else cellText = row.Cells[colIdx].Text.Replace("&nbsp;", "");
                        ws.Cells[excelRow, excelCol].Value = cellText; excelCol++;
                    }
                    excelRow++;
                }
                ws.Cells.AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + title.Replace(" ", "_") + "_Report.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray()); Response.End();
            }
        }

        private void ExportToPDF(string title)
        {
            int colCountVal = 0; foreach(DataControlField col in gvReport.Columns) { if(col.Visible && col.HeaderText != "Action") colCountVal++; }
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
            using (MemoryStream ms = new MemoryStream())
            {
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, ms); pdfDoc.Open();
                iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(colCountVal); table.WidthPercentage = 100;
                iTextSharp.text.pdf.PdfPCell headerCell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(title + " REPORT", iTextSharp.text.FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD)));
                headerCell.Colspan = colCountVal; headerCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER; headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER; table.AddCell(headerCell);
                foreach (DataControlField col in gvReport.Columns) { if (col.Visible && col.HeaderText != "Action") { table.AddCell(new iTextSharp.text.Phrase(col.HeaderText, iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD))); } }
                foreach (GridViewRow row in gvReport.Rows) { for (int i = 0; i < gvReport.Columns.Count; i++) { if (gvReport.Columns[i].Visible && gvReport.Columns[i].HeaderText != "Action") { 
                    string cellText = ""; Control ctrl = row.Cells[i].Controls.Count > 0 ? row.Cells[i].Controls[0] : null;
                    if (ctrl is Label) cellText = ((Label)ctrl).Text; else if (ctrl is TextBox) cellText = ((TextBox)ctrl).Text; else if (ctrl is CheckBox) cellText = ((CheckBox)ctrl).Checked ? "Yes" : "No"; else cellText = row.Cells[i].Text.Replace("&nbsp;", "");
                    table.AddCell(new iTextSharp.text.Phrase(cellText, iTextSharp.text.FontFactory.GetFont("Arial", 9)));
                } } }
                pdfDoc.Add(table); pdfDoc.Close();
                Response.Clear(); Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + title.Replace(" ", "_") + "_Report.pdf");
                Response.BinaryWrite(ms.ToArray()); Response.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "imp", "alert('Importing data...');", true);
        }

        protected void gvReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Bind context again to handle deletion logic or refresh
            BindGrid();
        }

        private void BindGrid()
        {
            DataTable dt = TPMStudioDBAccess.GetDummyData(); gvReport.DataSource = dt; gvReport.DataBind();
        }
    }

    public class DynamicTemplate_First : ITemplate
    {
        private string _colName; private string _type;
        public DynamicTemplate_First(string colName, string type) { _colName = colName; _type = type; }
        public void InstantiateIn(Control container)
        {
            if (_type == "TextField") {
                var txt = new TextBox(); txt.ID = "txt_" + _colName; txt.CssClass = "input-control";
                txt.DataBinding += (s, e) => { var t = (TextBox)s; var row = (GridViewRow)t.NamingContainer; if (row != null && row.DataItem != null) t.Text = DataBinder.Eval(row.DataItem, _colName)?.ToString() ?? ""; };
                container.Controls.Add(txt);
            } else if (_type == "Checkbox") {
                var chk = new CheckBox(); chk.ID = "chk_" + _colName;
                chk.DataBinding += (s, e) => { var c = (CheckBox)s; var row = (GridViewRow)c.NamingContainer; if (row != null && row.DataItem != null) c.Checked = Convert.ToBoolean(DataBinder.Eval(row.DataItem, _colName) ?? false); };
                container.Controls.Add(chk);
            } else if (_type == "DeleteButton") {
                var btn = new LinkButton(); btn.ID = "btnDelete"; btn.Text = "&#128465;"; btn.CommandName = "Delete";
                btn.OnClientClick = "return confirm('Are you sure?');";
                container.Controls.Add(btn);
            }
        }
    }
}
