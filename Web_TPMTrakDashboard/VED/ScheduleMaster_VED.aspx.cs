using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;


namespace Web_TPMTrakDashboard.VED
{
    public partial class ScheduleMaster_VED : System.Web.UI.Page
    {
        private static int selectedIDD = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = "";
                lbStatus.Items[0].Selected = true;
                lbStatus.Items[1].Selected = true;
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

                BindPlantID();
                BindVEDScheduleData();
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlantId.DataSource = list;
                ddlPlantId.DataBind();
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
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue);
                ddlCellID.DataSource = list;
                ddlCellID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindPlantIDFooter()
        {
            try
            {
                DropDownList ddl = gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList;
                List<string> list = DataBaseAccess.GetAllPlants();
                if (list.Count > 0)
                {
                    list.Remove("All");
                }
                ddl.DataSource = list;
                ddl.DataBind();
                ddl.SelectedValue = string.IsNullOrEmpty(ddlPlantId.SelectedValue) ? list[0] : ddlPlantId.SelectedValue;
                BindCellIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindCellIDFooter()
        {
            try
            {
                string plant = (gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList).SelectedValue;
                DropDownList ddl = gvScheduleCreate.FooterRow.FindControl("ddlCellNew") as DropDownList;
                List<string> list = DataBaseAccess.GetCellIDs(plant);
                if (list.Count > 0)
                {
                    list.Remove("All");
                }
                ddl.DataSource = list;
                ddl.DataBind();
                if (ddl.Items.FindByText(ddlCellID.SelectedValue) != null)
                    ddl.SelectedValue = string.IsNullOrEmpty(ddlCellID.SelectedValue) ? list[0] : ddlCellID.SelectedValue;
                BindComponentIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindComponentIDFooter()
        {
            try
            {
                string plant = (gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList).SelectedItem.Text;
                string cell = (gvScheduleCreate.FooterRow.FindControl("ddlCellNew") as DropDownList).SelectedItem.Text;
                //DropDownList ddl = (gvScheduleCreate.FooterRow.FindControl("ddlComponentNew") as DropDownList);
                HtmlGenericControl ddl = (gvScheduleCreate.FooterRow.FindControl("ddlComponentNew") as HtmlGenericControl);
                List<VEDSchedule> list = DataBaseAccess.getComponentIDwithplantcell(plant, cell);
                Session["compopnlist"] = list;
                var builder = new System.Text.StringBuilder();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {

                        string str = "<option style='font-weight:unset'>" + list[i].ComponentID.ToString();
                        builder.Append(str);

                    }
                }
                ddl.InnerHtml = builder.ToString();
                if (Session["NewComponentID"] != null)
                    (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text = Session["NewComponentID"] as string;

                TextBox txtcomponent = (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox);
                BindComponentDescFooter();
                BindOperationFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


        private void BindComponentDescFooter()
        {
            try
            {
                string compID = (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text;
                List<VEDSchedule> listitem = new List<VEDSchedule>();
                listitem = Session["compopnlist"] as List<VEDSchedule>;
                if (!string.IsNullOrEmpty(compID))
                {
                    var compDesc = listitem.Where(x => x.ComponentID == compID).Select(x => x.ComponentDesc.ToString()).FirstOrDefault();


                    int k = 1;
                    Label lblcompdesc = (gvScheduleCreate.FooterRow.FindControl("lblComponentDescNew") as Label);
                    lblcompdesc.Text = compDesc.ToString();
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindOperationFooter()
        {
            try
            {

                string plant = (gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList).SelectedItem.Text;
                string cell = (gvScheduleCreate.FooterRow.FindControl("ddlCellNew") as DropDownList).SelectedItem.Text;
                string compID = (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text;
                List<VEDSchedule> listitem = new List<VEDSchedule>();
                listitem = DataBaseAccess.getOperatiowithplantcell(plant, cell, compID);

                DropDownList ddl = (gvScheduleCreate.FooterRow.FindControl("ddlOperationNew") as DropDownList);
                var list = listitem.Select(x => x.OperationNumber).ToList();
                ddl.DataSource = list;
                ddl.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlPlantNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text = "";
                (gvScheduleCreate.FooterRow.FindControl("lblComponentDescNew") as Label).Text = "";
                BindCellIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlCellNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text = "";
            (gvScheduleCreate.FooterRow.FindControl("lblComponentDescNew") as Label).Text = "";
            BindComponentIDFooter();
        }
        protected void ddlComponentNew_SelectedIndexChanged(object sender, EventArgs e)
        {

            BindOperationFooter();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                selectedIDD = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "confirmModal", "openConfirmModal('deleteConfirmModal')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkDelete_Click = " + ex.Message);
            }
        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            try
            {
                selectedIDD = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "confirmModal", "openConfirmModal('closeConfirmModal')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkClose_Click = " + ex.Message);
            }
        }

        protected void lnkInsert_Click(object sender, EventArgs e)
        {
            try
            {
                VEDSchedule data = new VEDSchedule();
                data.StartDate = (gvScheduleCreate.FooterRow.FindControl("txtStartDateNew") as TextBox).Text;
                data.PlantID = (gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList).SelectedValue;
                data.CellID = (gvScheduleCreate.FooterRow.FindControl("ddlCellNew") as DropDownList).SelectedValue;
                data.ComponentID = (gvScheduleCreate.FooterRow.FindControl("txtComponentNew") as TextBox).Text;
                data.ComponentDesc = (gvScheduleCreate.FooterRow.FindControl("lblComponentDescNew") as Label).Text;
                data.PriorityNumber = (gvScheduleCreate.FooterRow.FindControl("txtPriorityNew") as TextBox).Text;
                data.OperationNumber = (gvScheduleCreate.FooterRow.FindControl("ddlOperationNew") as DropDownList).SelectedValue;
                data.Quantity = (gvScheduleCreate.FooterRow.FindControl("txtQtyNew") as TextBox).Text;
                data.Status = (gvScheduleCreate.FooterRow.FindControl("lblStatusNew") as Label).Text;
                data.EndDate = "";
                data.UpdatedBy = "";
                data.Status = "1";

                string result = DataBaseAccess.InsertVEDScheduleDetails(data);
                if (result == "Priority Number already Exists")
                {
                    HelperClassGeneric.openWarningModal(this, "Priority No. Already Exists!! For Plant : " + data.PlantID + ", Cell : " + data.CellID + ", Component : " + data.ComponentID + ", Operation No : " + data.OperationNumber);
                    return;
                }
                else if (result == "Inserted")
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                    BindVEDScheduleData();
                }
                else if (result == "Schedule Already Exists")
                {
                    HelperClassGeneric.openWarningModal(this, " Schedule Already Exists!! For Plant : " + data.PlantID + ", Cell : " + data.CellID + ", Component : " + data.ComponentID + ",Operation No : " + data.OperationNumber + " " + "");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkInsert_Click = " + ex.Message);
            }
        }


        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BindVEDScheduleData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnView_Click : " + ex.Message);
            }
        }


        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = gvScheduleCreate.FooterRow.FindControl("ddlPlantNew") as DropDownList;
                if (ddl.Items.FindByText(ddlPlantId.SelectedValue) != null)
                    ddl.SelectedValue = ddlPlantId.SelectedValue;
                BindCellID();
                BindCellIDFooter();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlPlantId_SelectedIndexChanged : " + ex.Message);
            }
        }

        protected void gvScheduleCreate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lnkDelete = e.Row.FindControl("lnkDelete") as LinkButton;
                    LinkButton lnkClose = e.Row.FindControl("lnkClose") as LinkButton;
                    string status = (e.Row.FindControl("lblStatus") as Label).Text;
                    bool deleteVisible = false, closeVisible = false;

                    if (!status.Equals("Running", StringComparison.OrdinalIgnoreCase))
                    {
                        deleteVisible = true;
                    }
                    if (status.Equals("Closed", StringComparison.OrdinalIgnoreCase))
                    {
                        deleteVisible = false;
                        closeVisible = false;
                    }
                    if (status.Equals("Running", StringComparison.OrdinalIgnoreCase))
                    {
                        closeVisible = true;
                    }
                    if (lnkDelete != null)
                    {
                        lnkDelete.Visible = deleteVisible;
                    }
                    if (lnkClose != null)
                    {
                        lnkClose.Visible = closeVisible;
                    }

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    TextBox txtstartDate = (TextBox)e.Row.FindControl("txtStartDateNew");
                    txtstartDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

                    HtmlGenericControl divContainer = (HtmlGenericControl)e.Row.FindControl("gvScheduleCreate");

                    if (divContainer != null)
                    {
                        DataList ddlComponentNew = (DataList)divContainer.FindControl("ddlComponentNew");

                        if (ddlComponentNew != null)
                        {
                            ddlComponentNew.Style["height"] = "20px";
                            ddlComponentNew.Style["min-height"] = "10px";
                        }
                    }

                    //DataList ddlComponentNewheight = (DataList)e.Row.FindControl("ddlComponentNew") as DataList;
                    //ddlComponentNewheight.Style["height"] = "20px";
                    //// Set the height of the DataList control
                    //if (ddlComponentNewheight != null)
                    //{
                    //    ddlComponentNewheight.Style["min-height"] = "10px"; // Adjust the height as needed
                    //}
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("gvScheduleCreate_RowDataBound = " + ex.Message);
            }
        }

        protected void gvScheduleCreate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvScheduleCreate.PageIndex = e.NewPageIndex;
                if (Session["ScheduleDataList"] != null)
                {
                    gvScheduleCreate.DataSource = Session["ScheduleDataList"] as List<VEDSchedule>;
                    gvScheduleCreate.DataBind();
                    (gvScheduleCreate.FooterRow.FindControl("txtStartDateNew") as TextBox).Text = DateTime.Now.ToString("dd-MM-yyyy");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void gvScheduleCreate_PreRender(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                if (grid != null)
                {
                    GridViewRow pagerRow = (GridViewRow)grid.BottomPagerRow;
                    if (pagerRow != null)
                    {
                        pagerRow.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void txtComponentNew_TextChanged(object sender, EventArgs e)
        {
            BindComponentDescFooter();
            BindOperationFooter();
        }

        private void BindVEDScheduleData()
        {
            try
            {
                string plantID = ddlPlantId.SelectedValue == "All" ? "" : ddlPlantId.SelectedValue;
                string cellID = ddlCellID.SelectedValue == "All" ? "" : ddlCellID.SelectedValue;
                string status = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbStatus);
                var startDate = string.IsNullOrEmpty(txtFromDate.Text) ? "" : Util.GetDateTime(txtFromDate.Text).ToString("yyyy-MM-dd");
                var endDate = Util.GetDateTime(txtToDate.Text).ToString("yyyy-MM-dd");
                List<VEDSchedule> list = DataBaseAccess.GetVEDScheduleDetails(plantID, cellID, startDate, endDate, status);
                if (list.Count > 0)
                {
                    gvScheduleCreate.DataSource = list;
                    gvScheduleCreate.DataBind();
                }
                else
                {
                    list.Add(new VEDSchedule { CellID = "" });
                    gvScheduleCreate.DataSource = list;
                    gvScheduleCreate.DataBind();
                    gvScheduleCreate.Rows[0].Visible = false;
                }
                Session["ScheduleDataList"] = list;
                BindPlantIDFooter();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindVEDScheduleData : " + ex.Message);
            }
        }

        protected void btnCloseConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                VEDSchedule data = new VEDSchedule();
                data.StartDate = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblStartDate") as Label).Text;
                data.PlantID = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblPlantID") as Label).Text;
                data.CellID = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblCell") as Label).Text;
                data.ComponentID = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblComponent") as Label).Text;
                data.OperationNumber = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblOperation") as Label).Text;
                data.EndDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                data.Status = "3";
                int result = DataBaseAccess.CloseVEDScheduleDetails(data);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Closed Successfully.')", true);
                }
                HelperClassGeneric.clearModal(this);
                selectedIDD = -1;
                BindVEDScheduleData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnCloseConfirm_Click = " + ex.Message);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                VEDSchedule data = new VEDSchedule();
                data.StartDate = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblStartDate") as Label).Text;
                data.PlantID = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblPlantID") as Label).Text;
                data.CellID = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblCell") as Label).Text;
                data.ComponentID = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblComponent") as Label).Text;
                data.OperationNumber = (gvScheduleCreate.Rows[selectedIDD].FindControl("lblOperation") as Label).Text;
                int result = DataBaseAccess.DeleteVEDScheduleDetails(data);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Deleted Successfully.')", true);
                }
                HelperClassGeneric.clearModal(this);
                selectedIDD = -1;
                BindVEDScheduleData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click = " + ex.Message);
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                List<VEDSchedule> list = new List<VEDSchedule>();
                VEDSchedule data = new VEDSchedule();
                foreach (GridViewRow row in gvScheduleCreate.Rows)
                {
                    data = new VEDSchedule();
                    data.StartDate = (row.FindControl("lblStartDate") as Label).Text;
                    data.PlantID = (row.FindControl("lblPlantID") as Label).Text;
                    data.CellID = (row.FindControl("lblCell") as Label).Text;
                    data.ComponentID = (row.FindControl("lblComponent") as Label).Text;
                    data.ComponentDesc = (row.FindControl("lblComponentDesc") as Label).Text;
                    data.OperationNumber = (row.FindControl("lblOperation") as Label).Text;
                    data.PriorityNumber = (row.FindControl("txtPriority") as TextBox).Text;
                    data.Quantity = (row.FindControl("txtQty") as TextBox).Text;
                    data.Status = (row.FindControl("lblStatus") as Label).Text;
                    list.Add(data);
                }

                var newlist = list.Where(x => x.Status.Equals("New", StringComparison.OrdinalIgnoreCase)).Distinct().ToList();


                var prioritynum = newlist.Select(x => new { plantID = x.PlantID, cellID = x.CellID, componentID = x.ComponentID, priorityno = x.PriorityNumber, operationNo = x.OperationNumber }).Distinct().ToList();

                var duplicatePriorities = newlist.GroupBy(x => new { x.PlantID, x.CellID, x.PriorityNumber }).Where(g => g.Count() > 1).SelectMany(g => g).ToList();
                if (duplicatePriorities.Any())
                {
                    foreach (var item in duplicatePriorities)
                    {

                        HelperClassGeneric.openWarningModal(this, "For Plant : " + item.PlantID + ", Cell : " + item.CellID + ", Component : " + item.ComponentID + ",Operation No : " + item.OperationNumber + " " + "Priority No. Already Exists!!");
                        return;
                    }
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        VEDSchedule data1 = new VEDSchedule();
                        data1.StartDate = list[i].StartDate.ToString();
                        data1.PlantID = list[i].PlantID.ToString();
                        data1.CellID = list[i].CellID.ToString();
                        data1.ComponentID = list[i].ComponentID.ToString();
                        data.ComponentDesc = list[i].ComponentDesc.ToString();
                        data1.OperationNumber = list[i].OperationNumber.ToString();
                        data1.PriorityNumber = list[i].PriorityNumber.ToString();
                        data1.Quantity = list[i].Quantity.ToString();
                        result = DataBaseAccess.UpdateVEDScheduleDetails(data1);
                    }
                }

                if (result > 0)
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
                    BindVEDScheduleData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSave_Click = " + ex.Message);
            }
        }

        protected void btnTemplateExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "VEDScheduleMasterTemplate.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "VEDScheduleMasterTemplate_" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("VEDScheduleMasterTemplate- \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                DownloadFile(destination, Excel.GetAsByteArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnTemplateExport_Click = " + ex.Message);
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

        protected void lnkImportPumpFile_Click(object sender, EventArgs e)
        {
            try
            {
                int success = 0;
                DataTable dtCOP = new DataTable();
                if (fuPumpImport.HasFile)
                {
                    string fileName = fuPumpImport.FileName;
                    if (Path.GetExtension(fileName) != ".xlsx" && Path.GetExtension(fileName) != ".xls")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "openWarningModal('Please choose the valid .xlsx or .xls file');", true);
                        return;
                    }
                    else
                    {
                        if (!Directory.Exists(Server.MapPath("ImportedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                        }
                        string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                        fuPumpImport.SaveAs(savedFileName);
                        string Errormsg = GetDataTableFromFile(savedFileName, out dtCOP);
                        if (Errormsg != "")
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "warning", "alert('" + Errormsg + "')", true);
                            HelperClassGeneric.openWarningModal(this, Errormsg);
                            return;
                        }
                        if (dtCOP.Rows.Count > 0)
                        {
                            string errorResult = DataBaseAccess.saveImportedDataToTempTable(dtCOP);
                            //string errorResult = VDGDataBaseAccess.saveImportedCOPDataToTempTable(dtCOP, true);
                            if (errorResult != "")
                            {
                                //ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "alert('Import failed.');", true);
                                HelperClassGeneric.openWarningModal(this, "" + errorResult);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "alert('Data Imported Successfully.');", true);
                                BindVEDScheduleData();
                            }
                        }
                        else
                        {
                            HelperClassGeneric.openWarningModal(this, "Empty Excel File");
                            //ScriptManager.RegisterStartupScript(this, GetType(), "RecordsTextopenModaladded", "alert('Import failed. Empty excel file.');", true);
                            return;
                        }
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "Please select file ");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lnkImportPumpFile_Click = " + ex.Message);
            }
        }

        private string GetDataTableFromFile(string fileName, out DataTable dt)
        {
            string Errormsg = "";
            dt = new DataTable();
            try
            {
                string pono = "";
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
                            int totalCols = worksheet.Dimension.End.Column;
                            int totalRows = GetLastUsedRow(worksheet);
                            int startRow = 4;
                            ExcelRange wsRow;
                            DataRow dr;

                            foreach (var firstRowCell in worksheet.Cells[3, 1, 3, totalCols])
                            {
                                dt.Columns.Add(firstRowCell.Text.Trim());
                            }
                            dt.Columns.Add("Status");
                            int i = 0;
                            for (int rowNum = startRow; rowNum <= totalRows; rowNum++)
                            {

                                wsRow = worksheet.Cells[rowNum, 1, rowNum, totalCols];
                                dr = dt.NewRow();
                                int datecol = 0;
                                int k = 2;
                                foreach (var cell in wsRow)
                                {
                                    //var date1 = Convert.ToDateTime(cell.Text).ToString("yyyy-MM-dd");
                                    if (datecol == 0)
                                    {
                                        k = 3;
                                        if (string.IsNullOrEmpty(cell.Text.Trim()))
                                        {
                                            dr[cell.Start.Column - 1] = DBNull.Value;
                                        }
                                        else
                                        {
                                            dr[cell.Start.Column - 1] = Convert.ToDateTime(cell.Text).ToString("yyyy-MM-dd");
                                        }
                                        datecol++;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(cell.Text.Trim()))
                                        {
                                            dr[cell.Start.Column - 1] = DBNull.Value;
                                        }
                                        else
                                        {
                                            dr[cell.Start.Column - 1] = cell.Text;
                                        }
                                        k++;
                                    }
                                    if (k == 10)
                                    {
                                        dr[cell.Start.Column] = 1;
                                    }

                                }

                                dt.Rows.Add(dr);
                                // dt.Rows[i]["Id"] = i + 1;
                                i++;
                            }
                            Errormsg = validate(dt);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteDebugLog("Error while importing Pump Master Details : " + ex.Message);
                            return "Failed to Import Data";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog("Error while importing Pump Master Details= " + ex.Message);
                return "Failed to Import Data.";
            }
            return Errormsg;
        }

        private static int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        private string validate(DataTable dt)
        {
            string result = "";
            try
            {
                DataTable dtCOP = new DataTable();
                DataTable dtexist = DataBaseAccess.GetVEDScheduleDetailsfortable();
                List<string> DuplicatesRows = new List<string>();
                List<string> BlankItemNo = new List<string>();
                List<string> OperationNo = new List<string>();
                string RowIndex = string.Empty;
                string dupValue = string.Empty;
                foreach (DataRow row in dt.Rows)
                {
                    var startDate = Util.GetDateTime(row["StartDate"].ToString()).ToString("yyyy-MM-dd");
                    if (startDate != null)
                    {
                        var plantID = row["PlantID"].ToString().Trim();
                        if (!string.IsNullOrEmpty(plantID))
                        {
                            var cellID = row["CellID"].ToString().Trim();
                            if (!string.IsNullOrEmpty(cellID))
                            {
                                var componentID = row["ComponentID"].ToString().Trim();
                                if (!string.IsNullOrEmpty(componentID))
                                {
                                    var operationNo = row["Operation"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(operationNo))
                                    {
                                        var priority = row["Priority"].ToString().Trim();
                                        if (!string.IsNullOrEmpty(priority))
                                        {
                                            if (HelperClassGeneric.IsValueNumeric(priority))
                                            {
                                                var qty = row["Quantity"].ToString().Trim();
                                                if (!string.IsNullOrEmpty(qty))
                                                {
                                                    if (HelperClassGeneric.IsValueDecimal(qty))
                                                    {
                                                        //var date1 = dtexist.AsEnumerable().Where(x => x["priority"].ToString() == "78").Select(x=>x["startDate"].ToString()).FirstOrDefault();
                                                        //var date2 = Util.GetDateTime(date1.ToString()).ToString("yyyy-MM-dd");
                                                        //var x1 = dtexist.AsEnumerable().Where(x => x["priority"].ToString() == "78").Select(x => x["PlantID"].ToString()).FirstOrDefault();
                                                        //var x2 = dtexist.AsEnumerable().Where(x => x["priority"].ToString() == "78").Select(x => x["GroupID"].ToString()).FirstOrDefault();
                                                        //var x3 = dtexist.AsEnumerable().Where(x => x["priority"].ToString() == "78").Select(x => x["PartID"].ToString()).FirstOrDefault();
                                                        //var x4 = dtexist.AsEnumerable().Where(x => x["priority"].ToString() == "78").Select(x => x["OperationNo"].ToString()).FirstOrDefault();

                                                        //var plant = dtexist.AsEnumerable().Where(x => x["PlantID"].ToString() == plantID).Count();
                                                        //var cell = dtexist.AsEnumerable().Where(x => x["PlantID"].ToString() == plantID && x["GroupID"].ToString() == cellID).Count() ;
                                                        //var component = dtexist.AsEnumerable().Where(x => x["PartID"].ToString() == componentID);
                                                        //var operation = dtexist.AsEnumerable().Where(x => x["operationNo"].ToString() == operationNo);

                                                        var operation1 = dtexist.AsEnumerable().Where(x => x["PlantID"].ToString().Trim() == plantID && x["GroupID"].ToString().Trim() == cellID && x["PartID"].ToString().Trim() == componentID && x["OperationNo"].ToString().Trim() == operationNo && Util.GetDateTime(x["StartDate"].ToString()).ToString("dd-MM-yyyy").Trim() == startDate).Count();

                                                        //var start= dtexist.AsEnumerable().Where(x => x["StartDate"].ToString() == startDate);
                                                        //Select(x => x["PlantID"].ToString()).Distinct();

                                                        var existingRow = dtexist.AsEnumerable().FirstOrDefault(x => x["PlantID"].ToString() == plantID && x["GroupID"].ToString() == cellID && x["PartID"].ToString() == componentID && x["OperationNo"].ToString() == operationNo && Util.GetDateTime(x["StartDate"].ToString()).ToString("yyyy-MM-dd") == startDate);

                                                        if (existingRow != null)
                                                        {
                                                            result = string.Format("Schedule Already exists  for  PlantID: {0}, CellID: {1}, ComponentID: {2}, Operation: {3}, StartDate: {4} ", plantID, cellID, componentID, operationNo, Util.GetDateTime(startDate).ToString("dd-MM-yyyy"));
                                                            return result;
                                                        }


                                                        var existingpriority = dtexist.AsEnumerable().FirstOrDefault(x => x["PlantID"].ToString() == plantID && x["GroupiD"].ToString() == cellID && x["Priority"].ToString() == priority && x["Status"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase));
                                                        if (existingpriority != null)
                                                        {
                                                            result = string.Format("Priority No.  Already exists  for  PlantID: {0}, CellID: {1}, Component : {2}, Operation : {3}", plantID, cellID, componentID, operationNo);
                                                            return result;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result = string.Format("Quantity should be a decimal value");
                                                        return result;
                                                    }
                                                }
                                                else
                                                {
                                                    result = string.Format("Quantity " + qty + "  cannot be empty");
                                                    return result;
                                                }
                                            }
                                            else
                                            {
                                                result = string.Format("Priority should be a numeric value");
                                                return result;
                                            }
                                        }
                                        else
                                        {
                                            result = string.Format("Priority " + priority + "  cannot be empty");
                                            return result;
                                        }
                                    }
                                    else
                                    {
                                        result = string.Format("OperationNo " + operationNo + "  cannot be empty");
                                        return result;
                                    }
                                }
                                else
                                {
                                    result = string.Format("ComponentID " + componentID + "  cannot be empty");
                                    return result;
                                }
                            }
                            else
                            {
                                result = string.Format("CellID " + cellID + "  cannot be empty");
                                return result;
                            }
                        }
                        else
                        {
                            result = string.Format("plantID " + plantID + "  cannot be empty");
                            return result;
                        }
                    }
                    else
                    {
                        result = string.Format("StartDate " + startDate + "  cannot be empty");
                        return result;
                    }

                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
                Logger.WriteErrorLog(ex.Message);
            }
            return result;
        }
    }
}