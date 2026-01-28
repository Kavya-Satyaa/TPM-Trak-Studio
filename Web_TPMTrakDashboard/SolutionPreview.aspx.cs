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
    public partial class SolutionPreview : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var filters = Session["Preview_Visibility"] as List<SolutionGenerator.FilterConfiguration>;
            if (filters == null) return;

            BuildDynamicFilters(filters);

            if (!IsPostBack)
            {
                // Initial bind for top-level and independent filters
                foreach (var f in filters.Where(f => f.Enabled))
                {
                    var lc = FindControlRecursive(phFilters, (f.Mode == "Single" ? "ddl" : "lb") + f.ID) as ListControl;
                    if (lc == null) continue;

                    if (f.ID == "Ddl1" || f.ID == "Ddl5") BindFilterControl(lc, f.ID, f.Name, f.Mode == "Single");
                    else if (f.ID == "Ddl2") BindFilterControl(lc, f.ID, f.Name, f.Mode == "Single", "");
                    else if (f.ID == "Ddl3") BindFilterControl(lc, f.ID, f.Name, f.Mode == "Single", "");
                    else if (f.ID == "Ddl4") BindFilterControl(lc, f.ID, f.Name, f.Mode == "Single", "", "");
                }
            }
            else
            {
                // On PostBack, restore items from Session immediately in OnInit
                // This is critical for ASP.NET LoadPostData to find the items and map selections.
                foreach (var f in filters.Where(f => f.Enabled))
                {
                    var lc = FindControlRecursive(phFilters, (f.Mode == "Single" ? "ddl" : "lb") + f.ID) as ListControl;
                    if (lc != null)
                    {
                        DataTable dt = Session["Items_" + f.ID] as DataTable;
                        if (dt != null)
                        {
                            lc.DataSource = dt;
                            lc.DataBind();
                            if (f.Mode == "Single") lc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select " + f.Name + " --", ""));
                        }
                        // Manual selection restoration as a failsafe for dynamic controls
                        RestoreSelection(lc, f.ID);
                    }
                }
            }
        }

        private string GetPostBackValue(string id)
        {
            if (Request.Form == null) return "";
            // Exhaustive search for the control's value in the form collection
            var keys = Request.Form.AllKeys.Where(k => k != null && (k.EndsWith("$lb" + id) || k.EndsWith("$ddl" + id) || k.EndsWith("$" + id) || k.EndsWith("_" + id))).ToList();
            
            List<string> vals = new List<string>();
            foreach (var key in keys)
            {
                var values = Request.Form.GetValues(key);
                if (values != null) vals.AddRange(values.Where(v => !string.IsNullOrEmpty(v)));
            }
            return string.Join(",", vals.Distinct());
        }

        private void RestoreSelection(ListControl lc, string id)
        {
            if (lc == null) return;
            string val = GetPostBackValue(id);
            if (string.IsNullOrEmpty(val)) return;

            var items = val.Split(',');
            foreach (var itm in items)
            {
                var li = lc.Items.FindByValue(itm);
                if (li != null) li.Selected = true;
            }
        }

        //private string GetPostBackValue(string id)
        //{
        //    if (Request.Form == null) return "";
        //    // Robust key matching for lbID, ddlID, or pnlID$lbID
        //    string key = Request.Form.AllKeys.FirstOrDefault(k => k != null && 
        //        (k.EndsWith("$lb" + id) || k.EndsWith("$ddl" + id) || k.EndsWith("$" + id) || k == id || k.EndsWith("_lb" + id) || k.EndsWith("_ddl" + id)));
            
        //    if (string.IsNullOrEmpty(key)) return "";
        //    var values = Request.Form.GetValues(key);
        //    if (values == null) return "";
        //    return string.Join(",", values.Where(v => !string.IsNullOrEmpty(v)));
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss");
                BindGrid();
            }

            string title = Session["Preview_Title"] as string;
            if (string.IsNullOrEmpty(title)) title = "Generated Screen Preview";
            litTitle.Text = title;

            var actionSettings = Session["Preview_ActionSettings"] as SolutionGenerator.ActionButtonSettings;
            if (actionSettings != null)
            {
                btnExport.Visible = actionSettings.EnableExport;
                if (actionSettings.EnableExport)
                {
                    btnExport.Text = "Export to " + actionSettings.ExportFormat;
                }

                btnImport.Visible = actionSettings.EnableImport;
                if (actionSettings.EnableImport)
                {
                    btnImport.Text = "Import Excel";
                }
            }
            CreateDynamicColumns();
        }

        private void BuildDynamicFilters(List<SolutionGenerator.FilterConfiguration> filters)
        {
            phFilters.Controls.Clear();
            foreach (var filter in filters)
            {
                if (!filter.Enabled) continue;

                var container = new Panel { ID = "pnl" + filter.ID, CssClass = "dropdown-container" };
                container.Controls.Add(new LiteralControl($"<label>{filter.Name}</label>"));

                if (filter.Mode == "Single" || filter.Mode == "Multi")
                {
                    if (filter.Mode == "Single")
                    {
                        var ddl = new DropDownList { ID = "ddl" + filter.ID, CssClass = "dropdown-control", AutoPostBack = true };
                        ddl.SelectedIndexChanged += Filter_Changed;
                        container.Controls.Add(ddl);
                    }
                    else
                    {
                        var div = new Panel { ID = "div" + filter.ID };
                        var lb = new ListBox 
                        { 
                            ID = "lb" + filter.ID, 
                            SelectionMode = ListSelectionMode.Multiple, 
                            CssClass = "dropdown-control",
                            AutoPostBack = true
                        };
                        lb.SelectedIndexChanged += Filter_Changed;
                        div.Controls.Add(lb);
                        container.Controls.Add(div);
                    }
                }
                else if (filter.ID == "Date")
                {
                    if (filter.Mode == "SingleDate")
                    {
                        container.Controls.Add(new TextBox { ID = "txtDate", TextMode = TextBoxMode.Date, CssClass = "dropdown-control" });
                    }
                    else if (filter.Mode == "DateRange")
                    {
                        container.CssClass += " filter-span-2";
                        var div = new Panel { ID = "divDate", CssClass = "date-range-container" };
                        div.Controls.Add(new TextBox { ID = "txtFromDate", TextMode = TextBoxMode.Date, CssClass = "dropdown-control" });
                        div.Controls.Add(new TextBox { ID = "txtToDate", TextMode = TextBoxMode.Date, CssClass = "dropdown-control" });
                        container.Controls.Add(div);
                    }
                }

                phFilters.Controls.Add(container);
            }
        }



        private Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id) return root;
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
            string senderID = lbSender.ID;

            // Get current selections across all levels
            string plantVal = GetSelectedValues("Ddl1");
            string machineVal = GetSelectedValues("Ddl2");
            string componentVal = GetSelectedValues("Ddl3");

            // Cascade only downstream to preserve parent stability
            if (senderID.EndsWith("Ddl1"))
            {
                UpdateFilter("Ddl2", plantVal);
                UpdateFilter("Ddl3", GetSelectedValues("Ddl2"));
                UpdateFilter("Ddl4", GetSelectedValues("Ddl2"), GetSelectedValues("Ddl3"));
            }
            else if (senderID.EndsWith("Ddl2"))
            {
                UpdateFilter("Ddl3", machineVal);
                UpdateFilter("Ddl4", machineVal, GetSelectedValues("Ddl3"));
            }
            else if (senderID.EndsWith("Ddl3"))
            {
                UpdateFilter("Ddl4", machineVal, componentVal);
            }
        }

        private void UpdateFilter(string id, string p1, string p2 = null)
        {
            var filters = Session["Preview_Visibility"] as List<SolutionGenerator.FilterConfiguration>;
            var config = filters?.FirstOrDefault(f => f.ID == id);
            if (config == null || !config.Enabled) return;

            Control target = FindControlRecursive(phFilters, "ddl" + id) ?? FindControlRecursive(phFilters, "lb" + id);
            if (target is ListControl lc)
            {
                BindFilterControl(lc, id, config.Name, config.Mode == "Single", p1, p2);
            }
        }

        private string GetSelectedValues(string filterID)
        {
            Control ctrl = FindControlRecursive(phFilters, "ddl" + filterID) ?? FindControlRecursive(phFilters, "lb" + filterID);
            if (ctrl is ListControl lc)
            {
                var selected = lc.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(i => i.Selected && !string.IsNullOrEmpty(i.Value)).Select(i => i.Value);
                return string.Join(",", selected);
            }
            return "";
        }

        private void BindFilterControl(ListControl lc, string id, string name, bool addDefault, string parent1 = null, string parent2 = null)
        {
            if (lc == null) return;

            DataTable dt;
            try
            {
                switch (id)
                {
                    case "Ddl1": dt = TPMStudioDBAccess.GetPlants(); break;
                    case "Ddl2": dt = TPMStudioDBAccess.GetMachines(parent1); break;
                    case "Ddl3": dt = TPMStudioDBAccess.GetComponents(parent1); break;
                    case "Ddl4": dt = TPMStudioDBAccess.GetOperations(parent1, parent2); break;
                    case "Ddl5": dt = TPMStudioDBAccess.GetShifts(); break;
                    default: dt = TPMStudioDBAccess.GetDummyData(); break;
                }
            }
            catch { dt = null; }

            // Fallback to dummy data if DB returns nothing or fails
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("Name");
                for (int i = 1; i <= 5; i++) dt.Rows.Add(id + "_" + i, name + " " + i + " (DB Fallback)");
            }

            lc.Items.Clear();
            lc.DataSource = dt;
            if (dt != null && dt.Columns.Count > 0)
            {
                lc.DataValueField = dt.Columns[0].ColumnName;
                lc.DataTextField = dt.Columns.Count > 1 ? dt.Columns[1].ColumnName : dt.Columns[0].ColumnName;
            }
            lc.DataBind();
            if (addDefault) lc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select " + name + " --", ""));

            // Cache items in session for postback recovery
            Session["Items_" + id] = dt;

            // Immediately restore selection after binding
            RestoreSelection(lc, id);
        }

        private void CreateDynamicColumns()
        {
            var columns = Session["Preview_GridColumns"] as List<SolutionGenerator.GridColumnDefinition>;
            if (columns == null) return;

            gvReport.Columns.Clear();

            var keys = columns.Where(c => c.IsKey).Select(c => c.Name).ToArray();
            gvReport.DataKeyNames = keys;

            foreach (var col in columns)
            {
                if (col.ControlType == "Label")
                {
                    var boundCol = new BoundField();
                    boundCol.HeaderText = col.Name;
                    boundCol.DataField = col.Name;
                    gvReport.Columns.Add(boundCol);
                }
                else
                {
                    var templateCol = new TemplateField();
                    templateCol.HeaderText = col.Name;
                    templateCol.ItemTemplate = new DynamicPreviewTemplate(col.Name, col.ControlType);
                    gvReport.Columns.Add(templateCol);
                }
            }

            // Add Action column
            var actionCol = new TemplateField();
            actionCol.HeaderText = "Action";
            actionCol.ItemTemplate = new DynamicPreviewTemplate("Action", "DeleteButton");
            actionCol.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            gvReport.Columns.Add(actionCol);
        }

        protected void gvReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Dummy logic for preview
            string keyInfo = "";
            foreach (string keyName in gvReport.DataKeyNames)
            {
                keyInfo += keyName + ": " + e.Keys[keyName]?.ToString() + " ";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "delPreview", $"alert('Preview Mode: Delete would be called for keys: {keyInfo}');", true);
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            DataTable dt = TPMStudioDBAccess.GetDummyData();
            gvReport.DataSource = dt;
            gvReport.DataBind();
            lblTime.Text = DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var actionSettings = Session["Preview_ActionSettings"] as SolutionGenerator.ActionButtonSettings;
            string format = actionSettings?.ExportFormat ?? "PDF";
            string title = litTitle.Text;

            // Re-bind to ensure data is there for export
            BindGrid();

            if (format == "Excel")
            {
                ExportToExcel(title);
            }
            else
            {
                ExportToPDF(title);
            }
        }

        private void ExportToExcel(string title)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                int colCount = 0;
                List<int> visibleColIndices = new List<int>();
                for (int i = 0; i < gvReport.Columns.Count; i++)
                {
                    if (gvReport.Columns[i].Visible && gvReport.Columns[i].HeaderText != "Action")
                    {
                        colCount++;
                        visibleColIndices.Add(i);
                    }
                }

                // Row 1 & 2: Title and "REPORT"
                if (colCount > 0)
                {
                    ws.Cells[1, 1, 2, colCount].Merge = true;
                    ws.Cells[1, 1].Value = title + " REPORT";
                    ws.Cells[1, 1].Style.Font.Size = 16;
                    ws.Cells[1, 1].Style.Font.Bold = true;
                    ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // Row 4: Headers
                int excelCol = 1;
                foreach (int idx in visibleColIndices)
                {
                    ws.Cells[4, excelCol].Value = gvReport.Columns[idx].HeaderText;
                    ws.Cells[4, excelCol].Style.Font.Bold = true;
                    ws.Cells[4, excelCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[4, excelCol].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    excelCol++;
                }

                // Data Rows
                int excelRow = 5;
                foreach (GridViewRow row in gvReport.Rows)
                {
                    excelCol = 1;
                    foreach (int colIdx in visibleColIndices)
                    {
                        string cellText = "";
                        Control ctrl = row.Cells[colIdx].Controls.Count > 0 ? row.Cells[colIdx].Controls[0] : null;

                        if (ctrl is Label) cellText = ((Label)ctrl).Text;
                        else if (ctrl is TextBox) cellText = ((TextBox)ctrl).Text;
                        else if (ctrl is CheckBox) cellText = ((CheckBox)ctrl).Checked ? "Yes" : "No";
                        else cellText = row.Cells[colIdx].Text.Replace("&nbsp;", "");

                        ws.Cells[excelRow, excelCol].Value = cellText;
                        excelCol++;
                    }
                    excelRow++;
                }

                ws.Cells.AutoFitColumns();
                
                // Final styles for the entire table range (Headers + Data)
                if (excelRow > 4)
                {
                    using (var range = ws.Cells[4, 1, excelRow - 1, colCount])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.WrapText = true;
                    }
                }

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + title.Replace(" ", "_") + "_Report.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();
            }
        }

        private void ExportToPDF(string title)
        {
            int colCount = 0;
            foreach (DataControlField col in gvReport.Columns)
            {
                if (col.Visible && col.HeaderText != "Action") colCount++;
            }

            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                PdfPTable table = new PdfPTable(colCount);
                table.WidthPercentage = 100;

                // Row 1 & 2: Title & Report
                PdfPCell headerCell = new PdfPCell(new Phrase(title + " REPORT", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD)));
                headerCell.Colspan = colCount;
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell.Padding = 10;
                headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(headerCell);

                // Spacing row (equivalent to 2nd/3rd row)
                PdfPCell spacerCell = new PdfPCell(new Phrase(" "));
                spacerCell.Colspan = colCount;
                spacerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                spacerCell.FixedHeight = 20f;
                table.AddCell(spacerCell);

                // Row 4: Headers
                foreach (DataControlField col in gvReport.Columns)
                {
                    if (col.Visible && col.HeaderText != "Action")
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
                        cell.BackgroundColor = new BaseColor(240, 240, 240);
                        table.AddCell(cell);
                    }
                }

                // Data Rows
                foreach (GridViewRow row in gvReport.Rows)
                {
                    for (int i = 0; i < gvReport.Columns.Count; i++)
                    {
                        if (gvReport.Columns[i].Visible && gvReport.Columns[i].HeaderText != "Action")
                        {
                            string cellText = "";
                            Control ctrl = row.Cells[i].Controls.Count > 0 ? row.Cells[i].Controls[0] : null;

                            if (ctrl is Label) cellText = ((Label)ctrl).Text;
                            else if (ctrl is TextBox) cellText = ((TextBox)ctrl).Text;
                            else if (ctrl is CheckBox) cellText = ((CheckBox)ctrl).Checked ? "Yes" : "No";
                            else cellText = row.Cells[i].Text.Replace("&nbsp;", "");

                            table.AddCell(new Phrase(cellText, FontFactory.GetFont("Arial", 9)));
                        }
                    }
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + title.Replace(" ", "_") + "_Report.pdf");
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required for gvReport.RenderControl(hw)
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "previewImp", "alert('Preview: Import would be triggered');", true);
        }

    }

    public class DynamicPreviewTemplate : ITemplate
    {
        private string _colName;
        private string _type;
        public DynamicPreviewTemplate(string colName, string type) { _colName = colName; _type = type; }

        public void InstantiateIn(Control container)
        {
            if (_type == "TextField")
            {
                var txt = new TextBox();
                txt.ID = "txt_" + _colName;
                txt.CssClass = "input-control";
                txt.Style["width"] = "100%";
                txt.Style["background"] = "rgba(255,255,255,0.05)";
                txt.Style["border"] = "1px solid rgba(255,255,255,0.1)";
                txt.Style["color"] = "white";
                txt.Style["padding"] = "4px 8px";
                txt.Style["border-radius"] = "4px";

                txt.DataBinding += (s, e) =>
                {
                    TextBox t = (TextBox)s;
                    GridViewRow row = (GridViewRow)t.NamingContainer;
                    if (row != null && row.DataItem != null)
                    {
                        var val = DataBinder.Eval(row.DataItem, _colName);
                        t.Text = val != null ? val.ToString() : "";
                    }
                };
                container.Controls.Add(txt);
            }
            else if (_type == "Checkbox")
            {
                var chk = new CheckBox();
                chk.ID = "chk_" + _colName;
                chk.DataBinding += (s, e) =>
                {
                    CheckBox c = (CheckBox)s;
                    GridViewRow row = (GridViewRow)c.NamingContainer;
                    if (row != null && row.DataItem != null)
                    {
                        var val = DataBinder.Eval(row.DataItem, _colName);
                        c.Checked = val != null && val != DBNull.Value && Convert.ToBoolean(val);
                    }
                };
                container.Controls.Add(chk);
            }
            else if (_type == "DeleteButton")
            {
                var btn = new LinkButton();
                btn.ID = "btnDelete";
                btn.Text = "&#128465;";
                btn.CommandName = "Delete";
                btn.OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                btn.Style["text-decoration"] = "none";
                btn.Style["color"] = "#f87171";
                btn.Style["font-size"] = "1.2rem";
                container.Controls.Add(btn);
            }
        }
    }
}
