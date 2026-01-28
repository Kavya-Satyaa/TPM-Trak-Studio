using OfficeOpenXml2;
using OfficeOpenXml2.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindPlantID();
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> allPlants = DataBaseAccess.GetAllPlants();
                if (allPlants != null && allPlants.Count > 0)
                {
                    ddlPlantId.DataSource = allPlants;
                    ddlPlantId.DataBind();
                    ddlPlantId_SelectedIndexChanged(null, null);
                }
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
                    List<string> allcellID = DataBaseAccess.GetAllGroupId(ddlPlantId.Text);
                    if (allcellID != null && allcellID.Count > 0)
                    {
                        ddlMultiCellID.DataSource = allcellID;
                        ddlMultiCellID.DataBind();
                        foreach (ListItem item in ddlMultiCellID.Items)
                        {
                            item.Selected = true;
                        }
                        ddlMultiCellID_SelectedIndexChanged(null, null);
                    }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindMachinesForPlantCell()
        {
            try
            {
                string cellId = DataBaseAccess.getCellIDWithSeparator(ddlMultiCellID);
                ddlMultiDownID.DataSource = DataBaseAccess.getMachineIDListForScreen(ddlPlantId.SelectedValue.ToString(), cellId, "");
                ddlMultiDownID.DataBind();
                if(!string.IsNullOrEmpty(cellId))
                {
                    foreach (ListItem item in ddlMultiDownID.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        static string appPath = HttpContext.Current.Server.MapPath("~/Reports");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, "TPMTrakReport", reportName);
            return src;
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

        protected void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                string templatefile = string.Empty;
                string Filename = string.Empty;
                Filename = "ComponentMaster_KTA.xlsx";
                DataTable dt = new DataTable();
                string cellId = string.Empty;
                foreach (System.Web.UI.WebControls.ListItem Item in ddlMultiCellID.Items)
                {
                    cellId += Item.Selected ? "'" + Item.Value + "'," : "";
                }
                cellId = cellId.Trim(',');
                string MachineID = string.Empty;
                foreach (System.Web.UI.WebControls.ListItem Item in ddlMultiDownID.Items)
                {
                    MachineID += Item.Selected ? "'" + Item.Value + "'," : "";
                }
                MachineID = MachineID.Trim(',');
                
                DataTable dtCopy = DataBaseAccess.getComponentMasterDetails(ddlPlantId.SelectedValue.ToString(),cellId,MachineID);
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "ComponentMaster_KTA" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("Component Master Report- \n " + Source);
                }
                if (dtCopy.Rows.Count > 0)
                {
                    FileInfo newFile = new FileInfo(Source);
                    ExcelPackage Excel = new ExcelPackage(newFile, true);
                    var exelworksheet = Excel.Workbook.Worksheets["Sheet1"];
                    exelworksheet.Cells["A1"].Value = "Plant Name: " +ddlPlantId.SelectedValue.ToString();

                    exelworksheet.PrinterSettings.PaperSize = ePaperSize.A3;
                    exelworksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    exelworksheet.PrinterSettings.FitToPage = true;
                    exelworksheet.PrinterSettings.FitToWidth = 1;
                    exelworksheet.PrinterSettings.FitToHeight = 0;

                    DataColumn dtColumn;
                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "machineid";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(long);
                    dtColumn.ColumnName = "CompInterfaceID";
                    dt.Columns.Add(dtColumn);


                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "componentid";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "CompDesc";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "operationno";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "OpnDesc";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "cycletime";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "loadunload";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "drawingno";
                    dt.Columns.Add(dtColumn);


                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Double);
                    dtColumn.ColumnName = "StdSetupTime";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(int);
                    dtColumn.ColumnName = "MachiningTimeThreshold";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(int);
                    dtColumn.ColumnName = "loadUnloadTimeThreshold";
                    dt.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(string);
                    dtColumn.ColumnName = "CellID";
                    dt.Columns.Add(dtColumn);

                  
                    for (int i = 0; i < dtCopy.Rows.Count; i++)
                    {
                        DataRow rows = dt.NewRow();
                        rows["machineid"] = dtCopy.Rows[i]["machineid"].ToString();
                        rows["CompInterfaceID"] = dtCopy.Rows[i]["CompInterfaceID"].ToString();
                        rows["componentid"] = dtCopy.Rows[i]["componentid"].ToString();
                        rows["CompDesc"] = dtCopy.Rows[i]["CompDesc"].ToString();
                        rows["operationno"] = dtCopy.Rows[i]["operationno"].ToString();
                        rows["OpnDesc"] = dtCopy.Rows[i]["OpnDesc"].ToString();
                        if (dtCopy.Rows[i]["cycletime"].ToString() != "")
                        {
                            rows["cycletime"] = Convert.ToDouble(dtCopy.Rows[i]["cycletime"].ToString().Trim());
                        }
                        if (dtCopy.Rows[i]["loadunload"].ToString() != "")
                        {
                            rows["loadunload"] = Convert.ToDouble(dtCopy.Rows[i]["loadunload"].ToString().Trim());
                        }
                        rows["drawingno"] = dtCopy.Rows[i]["drawingno"].ToString();
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["StdSetupTime"].ToString()))
                        {
                            rows["StdSetupTime"] = Convert.ToDouble(dtCopy.Rows[i]["StdSetupTime"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["MachiningTimeThreshold"].ToString()))
                        {
                            rows["MachiningTimeThreshold"] = Convert.ToDouble(dtCopy.Rows[i]["MachiningTimeThreshold"].ToString().Trim());
                        }
                        
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["loadUnloadTimeThreshold"].ToString()))
                        {
                            rows["loadUnloadTimeThreshold"] = Convert.ToInt32(dtCopy.Rows[i]["loadUnloadTimeThreshold"].ToString().Trim());
                        }
                        if (!string.IsNullOrEmpty(dtCopy.Rows[i]["GroupID"].ToString()))
                        {
                            rows["CellID"] = dtCopy.Rows[i]["GroupID"].ToString();
                        }
                        dt.Rows.Add(rows);
                    }

                    int rowCount = 4;
                    int cellCount = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cellCount = 1;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["componentid"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CompInterfaceID"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CompDesc"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = Convert.ToInt32(dt.Rows[i]["operationno"]);
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["OpnDesc"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["CellID"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["machineid"].ToString();
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["loadunload"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["cycletime"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["drawingno"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["StdSetupTime"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["MachiningTimeThreshold"];
                        cellCount++;
                        exelworksheet.Cells[rowCount, cellCount].Value = dt.Rows[i]["loadUnloadTimeThreshold"];
                        rowCount++;
                    }
                    exelworksheet.Cells[3, 1, rowCount, cellCount].AutoFitColumns();
                    string[] cellcount = cellId.Split(',');
                    if(cellcount.Length > 1)
                    {
                        exelworksheet.Column(6).Hidden = false;
                        exelworksheet.Cells["L1"].Value = "";
                    }
                    else
                    {
                        exelworksheet.Column(6).Hidden = true;
                        exelworksheet.Cells["L1"].Value = cellcount[0].Trim('\'');
                    }
                 
                    setBorderThin(exelworksheet, 4, 1, rowCount - 1, cellCount);
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

        protected void ddlMultiCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachinesForPlantCell();
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
        }
    }
}