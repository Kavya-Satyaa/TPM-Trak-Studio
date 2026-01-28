using BusinessClassLibrary;
using ModelClassLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class PlanEntryScreen : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<UserAccessModel> useAccessData = new List<UserAccessModel>();
                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;

                if (useAccessData.Where(ss => ss.Code.Equals("ItemStdCycleTimeMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                {
                    lblItemStdCycleTimeAction.Visible = true;
                    btnExportItemStd.Visible = true;
                }
                else
                {
                    lblItemStdCycleTimeAction.Visible = false;
                    btnExportItemStd.Visible = false;
                }
                Session["PESComponentDetails"] = CumiDBAccess.GetComponentInformation();
                Session["PESERPPODetails"] = CumiDBAccess.GetERPPODetails();
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindMachine();
                BindShift();
                BindPlanDetails();
            }
        }

        private void BindMachine()
        {
            try
            {
                List<string> list = CumiDBAccess.GetAllMachinedByPlant("");
                list.Insert(0, "All");

                ddlMachine.DataSource = list;
                ddlMachine.DataBind();

                list.Remove("All");
                ddlMachineID.DataSource = list;
                ddlMachineID.DataBind();
                Session["PESMachines"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindShift()
        {
            try
            {
                List<PDTData> shiftDetails = DataBaseAccess.getShiftTimeDetails(DateTime.Now);
                List<ListItem> list = new List<ListItem>();
                foreach (PDTData data in shiftDetails)
                {
                    list.Add(new ListItem() { Text = data.ShiftName, Value = data.ShiftName });
                }
                ddlShift.DataSource = list;
                ddlShift.DataTextField = "Text";
                ddlShift.DataValueField = "Value";
                ddlShift.DataBind();

                Session["PESShifts"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindShift: " + ex.Message);
            }
        }



        protected void btnView_Click(object sender, EventArgs e)
        {
            BindPlanDetails();
        }
        private void BindPlanDetails()
        {
            try
            {
                ViewState["Machine"] = ddlMachine.SelectedValue;
                List<PlanDetails> list = CumiDBAccess.GetPlanDetails(ViewState["Machine"].ToString(), txtFromDate.Text, txtToDate.Text);

                lvPlanDetails.DataSource = list;
                lvPlanDetails.DataBind();

                DataTable dtTotalEntry = new DataTable();
                DataTable dtNewEntry = DataBaseAccess.GetNewAndTotalEntryDetails(out dtTotalEntry);
                lblNewEntries.Text = dtNewEntry.AsEnumerable().Select(x => x["NewEntries"].ToString()).FirstOrDefault();
                lblTotalEntries.Text = dtTotalEntry.AsEnumerable().Select(x => x["TotalEntries"].ToString()).FirstOrDefault();


              
                string POCount = CumiDBAccess.GetPOCount();
                lblPOCount.Text = POCount;
              


                DataTable dtPODetails = CumiDBAccess.GetPODetails();
                if (dtPODetails.Rows.Count > 0)
                {
                    lblPONumber.Text = dtPODetails.AsEnumerable().Select(x => x["ProductionOrder"].ToString()).FirstOrDefault();
                    lblDateTime.Text = dtPODetails.AsEnumerable().Select(x => x["UpdatedTS"].ToString()).FirstOrDefault();
                }


                if (!String.IsNullOrEmpty(lblNewEntries.Text))
                {
                    int count = Convert.ToInt32(lblNewEntries.Text);
                    if (count > 0)
                    {
                        tdNewEnytriesCount.Attributes.Add("style", "background-color: red; color: white");
                    }
                    else
                    {
                        tdNewEnytriesCount.Attributes.Add("style", "background-color: white; color: black");
                    }
                }
                else
                {
                    tdNewEnytriesCount.Attributes.Add("style", "background-color: white; color: black");
                }

                Session["CumiPlanEntryDetails"] = list;
                ViewState["FromDate"] = txtFromDate.Text;
                ViewState["ToDate"] = txtToDate.Text;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPPDetails: " + ex.Message);
            }
        }

        protected void btnAddPlanDetails_Click(object sender, EventArgs e)
        {
            try
            {

                txtDate.Text = "";
                hfAutoID.Value = "";

                tdPOLabel.Visible = false;
                tdPOControl.Visible = false;
                trItemPlan.Visible = false;
                scanDiv.Visible = true;

                hfPlanNewEdit.Value = "New";
                btnPlanDetailsSave.Text = "Save";
                modalTitle.InnerText = "Add Plan Details";


                string machine = ddlMachine.SelectedValue;
                if (ddlMachineID.Items.FindByValue(machine) != null)
                {
                    ddlMachineID.SelectedValue = machine;
                }

                HelperClass.openAddEditModal(this, "neweditPlanModal");

                //if (rblEntryType.Items.FindByValue("Scan") != null)
                //{
                //    rblEntryType.Items.FindByValue("Scan").Selected = true;
                //}


                HelperClass.openFunction(this, "CheckEntryType");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnAddPPMasterDetails_Click: " + ex.Message);
            }
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                string value = (lvPlanDetails.Items[rowIndex].FindControl("lblMachineID") as Label).Text;
                if (ddlMachineID.Items.FindByValue(value) != null)
                {
                    ddlMachineID.SelectedValue = value;
                }
                txtDate.Text = (lvPlanDetails.Items[rowIndex].FindControl("lblDate") as Label).Text;
                value = (lvPlanDetails.Items[rowIndex].FindControl("lblShift") as Label).Text;
                if (ddlShift.Items.FindByValue(value) != null)
                {
                    ddlShift.SelectedValue = value;
                }
                txtItemCode.Text = (lvPlanDetails.Items[rowIndex].FindControl("lblItemCode") as Label).Text;
                txtPONo.Text = (lvPlanDetails.Items[rowIndex].FindControl("lblPONo") as Label).Text;
                txtPlanValue.Text = (lvPlanDetails.Items[rowIndex].FindControl("lblPlanValue") as Label).Text;
                hfAutoID.Value = (lvPlanDetails.Items[rowIndex].FindControl("hfAutoID") as HiddenField).Value;

                tdPOLabel.Visible = true;
                tdPOControl.Visible = true;
                trItemPlan.Visible = true;
                scanDiv.Visible = false;

                hfPlanNewEdit.Value = "Edit";
                btnPlanDetailsSave.Text = "UPDATE";
                modalTitle.InnerText = "Edit Plan Details";

                HelperClass.openAddEditModal(this, "neweditPlanModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbEdit_Click: " + ex.Message);
            }
        }

        protected void btnPlanDetailsSave_Click(object sender, EventArgs e)
        {
            try
            {
                // PlanDetails data = new PlanDetails();
                // data.IDD = hfAutoID.Value;
                // data.MachineID = ddlMachineID.SelectedValue;
                // data.Date = txtDate.Text;
                // data.Shift = ddlShift.SelectedValue;
                //// data.PlanValue = txtPlanValue.Text;
                // data.NewOrEdit = hfPlanNewEdit.Value;
                // string success = CumiDBAccess.SavePlanDetails(data);
                // if (success == "Exist")
                // {
                //     HelperClass.openModal(this, "neweditPlanModal", false);
                //     HelperClass.openWarningModal(this, "Record already exists.");
                //     return;
                // }
                // else if (success == "Inserted")
                // {
                //     HelperClass.openInsertSuccessModal(this);
                //     HelperClass.clearModal(this);
                // }
                // else if (success == "Updated")
                // {

                //     HelperClass.openUpdateSuccessModal(this);
                //     HelperClass.clearModal(this);
                // }
                // else
                // {
                //     HelperClass.openModal(this, "neweditPlanModal", false);
                //     HelperClass.openInsertErrorModal(this);
                //     return;
                // }
                HelperClass.openInsertSuccessModal(this);
                HelperClass.clearModal(this);
                BindPlanDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnPlanDetailsSave_Click: " + ex.Message);
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SavePlanDetails(PlanDetails planDetails)
        {
            string success = "";
            try
            {
                PlanDetails data = new PlanDetails();
                data.IDD = "";
                data.MachineID = planDetails.MachineID;
                data.Date = planDetails.Date;
                data.Shift = planDetails.Shift;
                data.PONo = planDetails.PONo;
                data.ItemCode = planDetails.ItemCode;
                data.Description = planDetails.Description;
                data.PlanValue = planDetails.PlanValue;
                data.NewOrEdit = planDetails.NewOrEdit;
                success = CumiDBAccess.SavePlanDetails(data);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("SavePlanDetails: " + ex.Message);
            }
            return success;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<ListItem> GetComponentDetails()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                if (HttpContext.Current.Session["PESComponentDetails"] == null)
                {
                    HttpContext.Current.Session["PESComponentDetails"] = CumiDBAccess.GetComponentInformation();
                }
                list = (List<ListItem>)HttpContext.Current.Session["PESComponentDetails"];
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetComponentDetails: " + ex.Message);
            }
            return list;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<ListItem> GetERPPODetails()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                if (HttpContext.Current.Session["PESERPPODetails"] == null)
                {
                    HttpContext.Current.Session["PESERPPODetails"] = CumiDBAccess.GetERPPODetails();
                }
                list = (List<ListItem>)HttpContext.Current.Session["PESERPPODetails"];
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetERPPODetails: " + ex.Message);
            }
            return list;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<ListItem> GetERPItemDetails(string po)
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                //if (HttpContext.Current.Session["PESERPPODetails"] == null)
                //{
                //    HttpContext.Current.Session["PESERPPODetails"] = CumiDBAccess.GetERPPODetails();
                //}
                list = CumiDBAccess.GetERPItemDetails(po);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("GetERPItemDetails: " + ex.Message);
            }
            return list;
        }
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as ListViewDataItem).DataItemIndex);
                //string parametr = (lvPlanDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("lbl") as Label).Text;
                string deletemsg = "Are you sure, you want to delete this record?";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "openDeleteConfirmModal('" + deletemsg + "')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("lbDelete_Click: " + ex.Message);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string idd = (lvPlanDetails.Items[(int)ViewState["DeleteRowIndex"]].FindControl("hfAutoID") as HiddenField).Value;
                bool result;
                CumiDBAccess.DeletePlanData(idd, out result);
                if (result)
                {

                    HelperClass.openDeleteSuccessModal(this);
                    HelperClass.clearModal(this);
                }
                else
                {
                    HelperClass.clearModal(this);
                    HelperClass.openDeleteErrorModal(this);
                    return;
                }
                BindPlanDetails();
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click: " + ex.Message);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuImport.HasFile)
                {
                    int success = 0;
                    string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    DataTable dtPlanDetails = new DataTable();

                    string fileName = fuImport.FileName;
                    if (Path.GetExtension(fileName) != ".xlsx" && Path.GetExtension(fileName) != ".xls")
                    {

                        HelperClass.openWarningModal(this, "Please choose the valid .xlsx or .xls file");
                        return;
                    }
                    else
                    {
                        string savedFileName = "";
                        if (!Directory.Exists(Server.MapPath("Cumi//ImportedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("Cumi//ImportedFiles"));
                            savedFileName = Server.MapPath("Cumi//ImportedFiles//" + fileName);
                            fuImport.SaveAs(savedFileName);
                        }
                        else
                        {
                            savedFileName = Server.MapPath("Cumi//ImportedFiles//" + fileName);
                            fuImport.SaveAs(savedFileName);
                        }
                        string errorMsg = "";
                        dtPlanDetails = GetDataTableFromFile(savedFileName, out errorMsg);
                        if (!String.IsNullOrEmpty(errorMsg))
                        {
                            if (errorMsg == "EmptyFile")
                            {
                                HelperClass.openWarningModal(this, "Empty excel file");
                                return;
                            }
                            else
                            {
                                lblInvalidRecords.InnerHtml = errorMsg;
                                HelperClass.openModal(this, "importModal", false);
                                return;
                            }
                        }

                        string rowNoToDisplay = string.Empty, sucess = string.Empty;
                        int rowNo = 1;
                        foreach (DataRow row in dtPlanDetails.Rows)
                        {
                            PlanDetails data = new PlanDetails();
                            data.MachineID = row["MachineId"].ToString();
                            data.Date = row["Date"].ToString();
                            data.Shift = row["Shift"].ToString();
                            data.PONo = row["PO"].ToString();
                            data.ItemCode = row["ItemCode"].ToString();
                            data.PlanValue = row["Plan"].ToString();
                            sucess = SavePlanDetails(data);
                            if (sucess == "Error")
                            {
                                if (String.IsNullOrEmpty(rowNoToDisplay))
                                {
                                    rowNoToDisplay += rowNo;
                                }
                                else
                                {
                                    rowNoToDisplay += ", " + rowNo;
                                }
                            }
                            rowNo++;
                        }
                        if (!String.IsNullOrEmpty(rowNoToDisplay))
                        {
                            HelperClass.openErrorModal(this, "Failed to insert records of row no.: " + rowNoToDisplay);
                            return;
                        }
                        BindPlanDetails();
                        HelperClass.openInsertSuccessModal(this);
                        HelperClass.clearModal(this);
                    }
                }
                else
                {
                    HelperClass.openWarningModal(this, "Please choose the file to import");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnImport_Click: " + ex.Message);
            }
        }
        private DataTable GetDataTableFromFile(string fileName, out string errorMsg)
        {

            List<ListItem> itemList = (List<ListItem>)Session["PESComponentDetails"];
            List<string> machineList = (List<string>)Session["PESMachines"];
            List<ListItem> shiftList = (List<ListItem>)Session["PESShifts"];

            DataTable dtPlanMaster = new DataTable();
            errorMsg = "";
            int rowNum = 0;
            string value = "";
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
                        dtPlanMaster.Columns.Add("MachineId");
                        dtPlanMaster.Columns.Add("Date");
                        dtPlanMaster.Columns.Add("Shift");
                        dtPlanMaster.Columns.Add("PO");
                        dtPlanMaster.Columns.Add("ItemCode");
                        dtPlanMaster.Columns.Add("Plan");
                        int startRow = 2, tblColStart = 0;
                        int lastRow = GetLastUsedRow(worksheet);
                        rowNum = startRow;
                        for (rowNum = startRow; rowNum <= lastRow; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, 1, rowNum, 6];
                            var tblRow = dtPlanMaster.NewRow();
                            tblColStart = 0;
                            foreach (var cell in wsRow)
                            {
                                if (String.IsNullOrEmpty(cell.Value.ToString()))
                                {
                                    errorMsg += "Value is empty in Row No.: " + rowNum + " and Cell No.: " + (tblColStart + 1) + "<br />";
                                    tblColStart++;
                                    continue;
                                }

                                if (tblColStart == 0)
                                {
                                    value = cell.Value.ToString();
                                    if (machineList.Contains(value))
                                    {
                                        tblRow[tblColStart] = value;
                                    }
                                    else
                                    {
                                        errorMsg += "Invalid Machine in Row No.: " + rowNum + " and Cell No.: " + (tblColStart + 1) + "<br />";
                                    }
                                }
                                else if (tblColStart == 1)
                                {
                                    value = cell.Value.ToString().Split(' ')[0];
                                    //string[] formats = new string[] { "dd-MM-yyyy", "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd-MMM-yyyy", "yyyy-MM-dd", "MM/dd/yyyy", "MM/d/yyyy", "M/d/yyyy", "M/dd/yyyy", };
                                    //DateTime datetime = DateTime.Now;
                                    //if (!DateTime.TryParseExact(value, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datetime))
                                    //{
                                    //    errorMsg += "Invalid Date in Row No.: " + rowNum + " and Cell No.: " + (tblColStart + 1) + "<br />";
                                    //}
                                    //else
                                    //{
                                    //    tblRow[tblColStart] = Util.GetDateTime(value).ToString("yyyy-MM-dd");
                                    //}
                                    try
                                    {
                                        tblRow[tblColStart] = Util.GetDateTime(cell.Value.ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    catch (Exception ex)
                                    {
                                        errorMsg += "Invalid Date in Row No.: " + rowNum + " and Cell No.: " + (tblColStart + 1) + "<br />";
                                    }
                                    //DateTime temp;
                                    //if (DateTime.TryParse(value, out temp))
                                    //{
                                    //    tblRow[tblColStart] = Util.GetDateTime(value).ToString("yyyy-MM-dd");
                                    //}
                                    //else
                                    //{
                                    //    errorMsg += "Invalid Date in Row No.: " + rowNum + " and Cell No.: " + (tblColStart + 1) + "<br />";
                                    //}
                                }
                                else if (tblColStart == 2)
                                {
                                    value = cell.Value.ToString();
                                    if (shiftList.Where(x => x.Value.Contains(value)).Select(x => x.Value).FirstOrDefault() != null)
                                    {
                                        tblRow[tblColStart] = value;
                                    }
                                    else
                                    {
                                        errorMsg += "Invalid Shift in Row No.: " + rowNum + " and Cell No.: " + (tblColStart + 1) + "<br />";
                                    }
                                }
                                else if (tblColStart == 4)
                                {
                                    value = cell.Value.ToString();
                                    if (itemList.Where(x => x.Text.Contains(value)).Select(x => x.Text).FirstOrDefault() != null)
                                    {
                                        tblRow[tblColStart] = value;
                                    }
                                    else
                                    {
                                        errorMsg += "Invalid Item Code in Row No.: " + rowNum + " and Cell No.: " + (tblColStart + 1) + "<br />";
                                    }
                                }
                                else
                                {
                                    tblRow[tblColStart] = cell.Value.ToString();

                                }
                                tblColStart++;
                            }
                            dtPlanMaster.Rows.Add(tblRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Object reference"))
                        {
                            errorMsg += "EmptyFile";
                        }
                        Logger.WriteDebugLog("Error while importing PO Details : " + ex.Message);
                    }
                }
            }
            return dtPlanMaster;
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


        protected void btnDownloadImportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string src, dst = string.Empty;
                string reportName = "PlanEntryImportTemplate.xlsx";

                src = Path.Combine(HttpContext.Current.Server.MapPath("~/Cumi"), "ImportTemplate", reportName);
                dst = Path.Combine(HttpContext.Current.Server.MapPath("~/Cumi"), "ImportTemplate", reportName);

                if (!File.Exists(src))
                {
                    HelperClass.openWarningModal(this, "Template Not found");
                    return;
                }
                FileInfo newFile = new FileInfo(src);
                ExcelPackage pck = new ExcelPackage(newFile, true);

                DownloadMultipleFile(dst, pck.GetAsByteArray());

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDownloadImportTemplate_Click: " + ex.Message);
            }
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
                //HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //  ErrorSignal.FromCurrentContext().Raise(ex);
                //Logger.WriteErrorLog("GENERATED ERROR : \n" + "Report generation Failed Error: " + ex.ToString());
            }
        }

        protected void lbNewEntries_Click(object sender, EventArgs e)
        {
            try
            {
                List<UserAccessModel> useAccessData = new List<UserAccessModel>();
                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;

                if (useAccessData.Where(ss => ss.Code.Equals("ItemStdCycleTimeMaster", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("SS", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                {
                    Response.Redirect("~/ItemStdCycleTimeMasterScreen.aspx?NavigateFrom=PlanEntry", false);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                List<PlanDetails> list = new List<PlanDetails>();
                string fromDate = ViewState["FromDate"].ToString();
                string toDate = ViewState["ToDate"].ToString();
                if (Session["CumiPlanEntryDetails"] == null)
                {
                    list = CumiDBAccess.GetPlanDetails(ddlMachine.SelectedValue, txtFromDate.Text, txtToDate.Text);
                    fromDate = txtFromDate.Text;
                    toDate = txtToDate.Text;
                }
                else
                {
                    list = (List<PlanDetails>)Session["CumiPlanEntryDetails"];
                }
                string Generated = TMPTrakGenerateReport.GenerateCumiPlanEntryDetails(fromDate, toDate, list);
                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    // HelperClass.opene(this, "No Data Found");
                }
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    //
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    //HelperClass.ope(this, "No Data Found");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExport_Click: " + ex.Message);
            }
        }

        protected void btnExportItemStd_Click(object sender, EventArgs e)
        {
            try
            {
                List<ItemStdCycleTimeDetails> list =  DataBaseAccess.GetItemStdCycleTimeDetails("", "", "", "", "", "ExportView"); 
                string Generated = TMPTrakGenerateReport.GenerateItemStdCycleTimeDetails(ViewState["FromDate"].ToString(), ViewState["ToDate"].ToString(), list, false);
                if (Generated.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    // HelperClass.opene(this, "No Data Found");
                }
                else if (Generated.Equals("NodataFound", StringComparison.OrdinalIgnoreCase))
                {
                    //
                }
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                {
                    //HelperClass.ope(this, "No Data Found");
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnExportItemStd_Click: " + ex.Message);
            }
        }
        protected void btnPOScreen_Click(object sender, EventArgs e)
        {
            Response.Redirect("POScreenCumi.aspx");

        }
    }
}