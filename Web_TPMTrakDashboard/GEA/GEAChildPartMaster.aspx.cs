using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using System.IO;
using Web_TPMTrakDashboard.Models;
using OfficeOpenXml;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class GEAChildPartMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductionOrder();
                BindData();
            }
        }
        private void BindProductionOrder()
        {
            try
            {
                List<string> list = GEADatabaseAccess.getTopProductionOrderData();
                ddlProductioOrder.DataSource = list;
                ddlProductioOrder.DataBind();
                BindFabricationNumber();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindFabricationNumber()
        {
            try
            {
                List<string> list = GEADatabaseAccess.getFabricationNumberByPO(ddlProductioOrder.SelectedValue);
                ddlFabricationNo.DataSource = list;
                ddlFabricationNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlProductioOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFabricationNumber();
        }
        private void BindData()
        {
            try
            {
                DataTable dt = GEADatabaseAccess.getChildPartMasterDetails(ddlProductioOrder.SelectedValue, ddlFabricationNo.SelectedValue);
                gvChildPart.DataSource = dt;
                gvChildPart.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbImportType.SelectedValue == "" || rbImportType.SelectedValue == null)
                {
                    HelperClassGeneric.openWarningModal(this, "Please select import type");
                    return;
                }

                int success = 0;
                DataTable dtData = new DataTable();
                if (fileUploader.HasFile)
                {
                    string fileName = fileUploader.FileName;
                    if (!Path.GetExtension(fileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) && !Path.GetExtension(fileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningModal(this, "Please choose the valid .xlsx or .xls file");
                        return;
                    }
                    else
                    {
                        if (!Directory.Exists(Server.MapPath("ImportedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                        }
                        string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                        fileUploader.SaveAs(savedFileName);
                        string Errormsg = GetDataTableFromFile(savedFileName, out dtData);
                        if (Errormsg != "")
                        {
                            HelperClassGeneric.openWarningModal(this, Errormsg);
                            return;
                        }
                        GEADatabaseAccess.deleteChildPartMasterTempTable("StoresItemListTemp_GEA");
                        if (dtData.Rows.Count > 0)
                        {

                            string errorResult = GEADatabaseAccess.insertImportedChildPartMasterDataToTempTable(dtData, rbImportType.SelectedValue);
                            if (errorResult != "")
                            {
                                HelperClassGeneric.openErrorModal(this, "Import failed");
                            }
                            else
                            {
                                HelperClassGeneric.openSuccessModal(this, "Data Imported Successfully.");
                                BindData();
                            }
                        }
                        else
                        {
                            HelperClassGeneric.openErrorModal(this, "Import failed.Empty excel file.");
                            return;
                        }
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningModal(this, "Please choose a file to import");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private string GetDataTableFromFile(string fileName, out DataTable dt)
        {
            string Errormsg = "";
            dt = new DataTable();
            try
            {
                using (var pck = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(fileName))
                    {
                        pck.Load(stream);
                    }
                    var workBook = pck.Workbook;
                    if (workBook != null)
                    {
                        try
                        {
                            var worksheet = workBook.Worksheets[1];
                            dt = new DataTable(worksheet.Name);
                            DataRow dr;
                            //dt.Columns.Add("ItemSortorder", typeof(int));
                            dt.Columns.Add("ProductionOrderNo", typeof(string));
                            dt.Columns.Add("PagedRequirement", typeof(string));
                            dt.Columns.Add("MaterialID", typeof(string));
                            dt.Columns.Add("MaterialDescription", typeof(string));
                            dt.Columns.Add("ReservationItem", typeof(string));
                            dt.Columns.Add("RequirementQuantity", typeof(float));
                            dt.Columns.Add("BaseUnitOfMeasure", typeof(string));
                            dt.Columns.Add("QuantityWithDrawn", typeof(float));
                            dt.Columns.Add("Shortage", typeof(float));
                            dt.Columns.Add("ProdStorageBin", typeof(string));
                            dt.Columns.Add("StorageLocation", typeof(string));
                            dt.Columns.Add("ItemCategory", typeof(string));
                            dt.Columns.Add("ProcurementType", typeof(string));
                            dt.Columns.Add("SparePartIndicator", typeof(string));
                            dt.Columns.Add("OpenQuantity", typeof(float));
                            dt.Columns.Add("FabricationNo", typeof(string));
                            int rowCount = 2;
                            while (true)
                            {
                                if (worksheet.Cells[rowCount, 1].Value == null)
                                {
                                    break;
                                }

                                dr = dt.NewRow();
                                //dr["ItemSortorder"] = worksheet.Cells[rowCount, 1].Value.ToString();
                                string poNo = worksheet.Cells[rowCount, 1].Value.ToString();
                                dr["ProductionOrderNo"] = worksheet.Cells[rowCount, 1].Value.ToString();
                                dr["PagedRequirement"] = getCellValue(worksheet, rowCount, 2);
                                dr["MaterialID"] = getCellValue(worksheet, rowCount, 3);
                                dr["MaterialDescription"] = getCellValue(worksheet, rowCount, 4);
                                dr["ReservationItem"] = getCellValue(worksheet, rowCount, 5);
                                dr["RequirementQuantity"] = getDoubleValue(worksheet, rowCount, 6);
                                dr["BaseUnitOfMeasure"] = getCellValue(worksheet, rowCount, 7);
                                dr["QuantityWithDrawn"] = getDoubleValue(worksheet, rowCount, 8);
                                dr["Shortage"] = getDoubleValue(worksheet, rowCount, 9);
                                dr["ProdStorageBin"] = getCellValue(worksheet, rowCount, 10);
                                dr["StorageLocation"] = getCellValue(worksheet, rowCount, 11);
                                dr["ItemCategory"] = getCellValue(worksheet, rowCount, 12);
                                dr["ProcurementType"] = getCellValue(worksheet, rowCount, 13);
                                dr["SparePartIndicator"] = getCellValue(worksheet, rowCount, 14);
                                dr["OpenQuantity"] = getDoubleValue(worksheet, rowCount, 15);
                                dr["FabricationNo"] = ddlFabricationNo.SelectedValue;
                                rowCount++;
                                if (!poNo.Equals(ddlProductioOrder.SelectedValue, StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex);
                            return "Failed to Import Data.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                return "Failed to Import Data.";
            }
            return Errormsg;
        }
        private static string getCellValue(ExcelWorksheet excelWorksheet, int row, int col)
        {
            string value = "";
            try
            {
                if (excelWorksheet.Cells[row, col].Value != null)
                {
                    value = excelWorksheet.Cells[row, col].Value.ToString();
                }
            }
            catch (Exception ex) { }
            return value;
        }
        private static Double getDoubleValue(ExcelWorksheet excelWorksheet, int row, int col)
        {
            Double value = 0;
            try
            {
                if (excelWorksheet.Cells[row, col].Value != null)
                {
                    value = Convert.ToDouble(excelWorksheet.Cells[row, col].Value.ToString());
                }
            }
            catch (Exception ex) { }
            return value;
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExportTemplate_Click(object sender, EventArgs e)
        {
            GEAGenerateReport.GeneratePickingListMasterImportReport();
        }
    }
}