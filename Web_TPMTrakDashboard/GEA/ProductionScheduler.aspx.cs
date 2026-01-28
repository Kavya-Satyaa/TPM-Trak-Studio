using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.GEA.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class ProductionScheduler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    Session["MaterialIds"] = null;
                    Session["MachineIDProcessList"] = null;
                    SessionClear.ClearSession();
                    BindMachineIDs();
                    LoadMaterialIDs();
                    LoadOperationNos("Save");
                    LooadActivity();
                    LoadPriorities();
                    BindProductionScheduleData("");

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindMachineIDs()
        {
            try
            {
                List<string> lstMachineIDs = GEADatabaseAccess.GetAllMachineByPlantandGroup("", "");
                if (lstMachineIDs != null && lstMachineIDs.Count > 0)
                {
                    ddlMachineId.DataSource = lstMachineIDs;
                    ddlMachineId.DataBind();
                    ddlMachineId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void LoadMaterialIDs()
        {
            try
            {
                List<string> listMaterialIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllComponentbyMachine(ddlMachineId.SelectedValue.ToString());
                if (listMaterialIds != null && listMaterialIds.Count > 0)
                {
                    if (listMaterialIds.Contains("All")) listMaterialIds.Remove("All");
                    Session["MaterialIds"] = listMaterialIds;
                    ddlMaterialID.DataSource = null;
                    ddlMaterialID.DataSource = listMaterialIds;
                    ddlMaterialID.DataBind();
                    ddlMaterialID.SelectedIndex = 0;
                    ddlUpdtMatId.DataSource = listMaterialIds;
                    ddlUpdtMatId.DataBind();
                    ddlUpdtMatId.SelectedIndex = 0;


                    ddlMaterialID_A.DataSource = null;
                    ddlMaterialID_A.DataSource = listMaterialIds;
                    ddlMaterialID_A.DataBind();
                    ddlMaterialID_A.SelectedIndex = 0;
                    ddlUpdtMatId_A.DataSource = listMaterialIds;
                    ddlUpdtMatId_A.DataBind();
                    ddlUpdtMatId_A.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void LoadOperationNos(string action)
        {
            try
            {
                string materialId;
                List<string> listOpearations;
                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                if (action.Equals("Save"))
                {
                    materialId = ddlMaterialID.SelectedItem != null ? ddlMaterialID.SelectedValue : "";
                    listOpearations = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation(machine, materialId);
                    if (listOpearations != null && listOpearations.Count > 0)
                    {
                        ddlOperationNum.DataSource = listOpearations;
                        ddlOperationNum.DataBind();
                        ddlOperationNum.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlOperationNum.DataSource = null;
                        ddlOperationNum.DataBind();
                    }
                }
                else
                {
                    materialId = ddlUpdtMatId.SelectedItem != null ? ddlUpdtMatId.SelectedValue : "";
                    listOpearations = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation(machine, materialId);
                    if (listOpearations != null && listOpearations.Count > 0)
                    {
                        ddlUpdtOpnNum.DataSource = listOpearations;
                        ddlUpdtOpnNum.DataBind();
                        ddlUpdtOpnNum.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlUpdtOpnNum.DataSource = null;
                        ddlUpdtOpnNum.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void LooadActivity()
        {
            try
            {
                string materialId;
                List<string> listOpearations;
                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                listOpearations = Web_TPMTrakDashboard.Models.DataBaseAccess.GetSubOperation(machine, "", "Default");
                cbDefaultActivities_A.DataSource = listOpearations;
                cbDefaultActivities_A.DataBind();
                cbOptionalActivities_A.DataSource = listOpearations;
                cbOptionalActivities_A.DataBind();
                cbUpdateDefaultActivity.DataSource = listOpearations;
                cbUpdateDefaultActivity.DataBind();
                cbUpdateOptionalActivity.DataSource = listOpearations;
                cbUpdateOptionalActivity.DataBind();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void LoadPriorities()
        {
            try
            {

                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                if (!string.IsNullOrEmpty(machine))
                {
                    List<PriorityType> lstPriorities = GEADatabaseAccess.GetAllSchedulePrioritiesKeyValue(machine);
                    if (lstPriorities != null && lstPriorities.Count > 0)
                    {
                        //ddlPriorities.DataSource = null;
                        //ddlAddPriority.DataSource = null;
                        //ddlAddPriority_A.DataSource = null;
                        //foreach (PriorityType priority in lstPriorities)
                        //{
                        //    ddlPriorities.Items.Add(new ListItem { Text = priority.UserPriority, Value = priority.SystemPriority });
                        //    ddlAddPriority.Items.Add(new ListItem { Text = priority.UserPriority, Value = priority.SystemPriority });
                        //    ddlAddPriority_A.Items.Add(new ListItem { Text = priority.UserPriority, Value = priority.SystemPriority });
                        //}
                        ddlPriorities.DataSource = lstPriorities;
                        ddlPriorities.DataTextField = "SystemPriority";
                        ddlPriorities.DataValueField = "SystemPriority";
                        ddlPriorities.DataBind();
                        ddlPriorities.SelectedIndex = 0;

                        ddlAddPriority.DataSource = lstPriorities;
                        ddlAddPriority.DataTextField = "SystemPriority";
                        ddlAddPriority.DataValueField = "SystemPriority";
                        ddlAddPriority.DataBind();
                        ddlAddPriority.SelectedIndex = 0;

                        ddlAddPriority_A.DataSource = lstPriorities;
                        ddlAddPriority_A.DataTextField = "SystemPriority";
                        ddlAddPriority_A.DataValueField = "SystemPriority";
                        ddlAddPriority_A.DataBind();
                        ddlAddPriority_A.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlPriorities.Items.Clear();
                        ddlPriorities.DataBind();
                        ddlPriorities.Items.Clear();
                        ddlAddPriority.DataBind();
                        ddlPriorities.Items.Clear();
                        ddlAddPriority_A.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetOperationsList(string machine, string matId)
        {
            List<string> OpnList = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation(machine, matId);
            return OpnList;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetSubOperationsList(string machine, string matId, string defualtoroptional)
        {
            List<string> OpnList = Web_TPMTrakDashboard.Models.DataBaseAccess.GetSubOperation(machine, matId, defualtoroptional);
            return OpnList;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetComponentList(string machine)
        {
            List<string> listMaterialIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllComponentbyMachine(machine);
            if (listMaterialIds.Contains("All")) listMaterialIds.Remove("All");
            HttpContext.Current.Session["SearchMaterialIds"] = listMaterialIds;
            return listMaterialIds;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetSearchedComponentList(string searchText)
        {
            List<string> searchedlist = new List<string>();
            try
            {
                if (HttpContext.Current.Session["SearchMaterialIds"] != null)
                {
                    if (searchText != "")
                    {
                        List<string> list = HttpContext.Current.Session["SearchMaterialIds"] as List<string>;
                        searchedlist = list.Where(k => k.Contains(searchText)).ToList();
                    }
                    else
                    {
                        searchedlist = HttpContext.Current.Session["SearchMaterialIds"] as List<string>;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return searchedlist;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<PriorityType> BindPriorities(string machine)
        {
            List<PriorityType> Priorities = GEADatabaseAccess.GetAllSchedulePrioritiesKeyValue(machine);
            //if (Priorities.Contains("All")) Priorities.Remove("All");
            return Priorities;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> BindPriorities_A(string machine)
        {
            List<string> Priorities = GEADatabaseAccess.GetAllSchedulePriorities(machine);
            if (Priorities.Contains("All")) Priorities.Remove("All");
            return Priorities;
        }

        private void BindProductionScheduleData(string SortExpression)
        {
            try
            {
                string status = string.Empty;
                string prod_order = string.Empty;
                string material_id = string.Empty;
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string process = getMachineIdProcess(MachineID);
                if (!string.IsNullOrEmpty(MachineID))
                {
                    foreach (ListItem statusItem in lstStatus.Items)
                    {
                        if (statusItem.Selected)
                            status = status + statusItem.Text + ",";
                    }
                    if (radioPoNumber.Checked)
                    {
                        prod_order = !string.IsNullOrEmpty(txtSearchText.Text) ? txtSearchText.Text : "";
                        material_id = "";
                    }
                    if (radioMaterialId.Checked)
                    {
                        prod_order = "";
                        material_id = !string.IsNullOrEmpty(txtSearchText.Text) ? txtSearchText.Text : "";
                    }
                    string from_date = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text : "";
                    string to_date = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text : "";

                    if (process.Equals("Assembly", StringComparison.OrdinalIgnoreCase) || process.Equals("Testing", StringComparison.OrdinalIgnoreCase) || process.Equals("Packing", StringComparison.OrdinalIgnoreCase) || process.Equals("Stores", StringComparison.OrdinalIgnoreCase))
                    {

                        List<AssemblyScheduleDetailsEntity> prodScheduleDetails = GEADatabaseAccess.GetAssemblySchedulesData(MachineID, status, prod_order, material_id, from_date, to_date);
                        if (prodScheduleDetails != null && prodScheduleDetails.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(SortExpression))
                            {
                                Session["SortExpression"] = SortExpression;
                                switch (SortExpression)
                                {
                                    case "Status ASC":
                                        prodScheduleDetails = prodScheduleDetails.OrderBy(x => x.Status).ToList();
                                        break;
                                    case "Status DESC":
                                        prodScheduleDetails = prodScheduleDetails.OrderByDescending(x => x.Status).ToList();
                                        break;
                                    case "Priority ASC":
                                        prodScheduleDetails = prodScheduleDetails.OrderBy(x => x.Priority).ToList();
                                        break;
                                    case "Priority DESC":
                                        prodScheduleDetails = prodScheduleDetails.OrderByDescending(x => x.Priority).ToList();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            Session["AssemblyData"] = prodScheduleDetails;
                            lvAssemblyDetails.DataSource = prodScheduleDetails;
                            lvAssemblyDetails.DataBind();
                        }
                        else
                        {
                            Session["AssemblyData"] = null;
                            lvAssemblyDetails.DataSource = new List<ScheduleDetailsEntity>();
                            lvAssemblyDetails.DataBind();
                        }
                        lvAssemblyDetails.Visible = true;
                        lstProdScheduler.Visible = false;
                    }
                    else
                    {
                        List<ScheduleDetailsEntity> prodScheduleDetails = new List<ScheduleDetailsEntity>();
                        if (MachineID.Equals("Quality In house") || MachineID.Equals("Quality Incoming"))
                        {
                            prodScheduleDetails = GEADatabaseAccess.GetQualityProductionSchedulesData(MachineID, status, prod_order, material_id, from_date, to_date);
                        }
                        else
                        {
                            prodScheduleDetails = GEADatabaseAccess.GetProductionSchedulesData(MachineID, status, prod_order, material_id, from_date, to_date);
                        }
                        if (prodScheduleDetails != null && prodScheduleDetails.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(SortExpression))
                            {
                                Session["SortExpression"] = SortExpression;
                                switch (SortExpression)
                                {
                                    case "Status ASC":
                                        prodScheduleDetails = prodScheduleDetails.OrderBy(x => x.Status).ToList();
                                        break;
                                    case "Status DESC":
                                        prodScheduleDetails = prodScheduleDetails.OrderByDescending(x => x.Status).ToList();
                                        break;
                                    case "Priority ASC":
                                        prodScheduleDetails = prodScheduleDetails.OrderBy(x => x.Priority).ToList();
                                        break;
                                    case "Priority DESC":
                                        prodScheduleDetails = prodScheduleDetails.OrderByDescending(x => x.Priority).ToList();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            Session["ProdScheduleData"] = prodScheduleDetails;
                            lstProdScheduler.DataSource = prodScheduleDetails;
                            lstProdScheduler.DataBind();
                            if (ddlMachineId.SelectedValue.ToString().Equals("Quality Incoming") || ddlMachineId.SelectedValue.ToString().Equals("Quality In house"))
                            {
                                if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = true;
                                if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = true;
                                if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = true;
                                foreach (ListViewItem itm in lstProdScheduler.Items)
                                {
                                    if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = true;
                                    if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = true;
                                    if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = true;
                                }
                            }
                            else
                            {
                                if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = false;
                                if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = false;
                                if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = false;
                                foreach (ListViewItem itm in lstProdScheduler.Items)
                                {
                                    if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = false;
                                    if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = false;
                                    if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = false;
                                }
                            }
                        }
                        else
                        {
                            Session["ProdScheduleData"] = null;
                            lstProdScheduler.DataSource = new List<ScheduleDetailsEntity>();
                            lstProdScheduler.DataBind();
                        }
                        lvAssemblyDetails.Visible = false;
                        lstProdScheduler.Visible = true;
                    }

                    //if (process.Contains("Testing") || process.Contains("Packing") || process.Contains("Assembly") || process.Contains("Balancing"))
                    if (process.Contains("Testing") || process.Equals("Assembly", StringComparison.OrdinalIgnoreCase))
                    {
                        btnCancel.Visible = false;
                        btnDelete.Visible = false;
                        btnAddNewSchedule.Visible = false;
                        btnChangePriority.Visible = true;
                        btnImport.Visible = false;
                        btnDownloadTemplate.Visible = false;

                        List<UserAccessModel> useAccessData = null;
                        if (Session["UserAccessData"] == null)
                            Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                        else
                            useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                        if (useAccessData != null)
                        {
                            if (useAccessData.Where(ss => ss.Code.Equals("ChangeSchedulePriority", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                            {
                                // btnAddNewSchedule.Visible = true;
                                btnChangePriority.Visible = true;
                            }
                        }

                    }
                    else
                    {
                        btnCancel.Visible = true;
                        btnDelete.Visible = true;
                        btnAddNewSchedule.Visible = true;
                        // btnChangePriority.Visible = true;
                        btnImport.Visible = true;
                        btnDownloadTemplate.Visible = true;
                        //if(lvAssemblyDetails.col.Count > 0){
                        //    lvAssemblyDetails.Items[0].cel.Visible = true;
                        //    //lvAssemblyDetails.Items[23].Visible = true;
                        //}
                        //btnAddNewSchedule.Visible = false;
                        btnChangePriority.Visible = false;
                        List<ModelClassLibrary.UserAccessModel> useAccessData = null;
                        if (Session["UserAccessData"] == null)
                            Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                        else
                            useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                        if (useAccessData != null)
                        {
                            if (useAccessData.Where(ss => ss.Code.Equals("ChangeSchedulePriority", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                            {
                                // btnAddNewSchedule.Visible = true;
                                btnChangePriority.Visible = true;
                            }
                        }
                    }

                    if (process.Contains("Balancing") || process.Contains("Assembly"))
                    {
                        List<UserAccessModel> useAccessData = null;
                        if (Session["UserAccessData"] == null)
                            Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                        else
                            useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                        if (useAccessData != null)
                        {
                            if (useAccessData.Where(ss => ss.Code.Equals("SwitchSchedule", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                            {
                                btnSwitchSchedule.Visible = true;

                                if (MachineID.Equals("Assembly-1", StringComparison.OrdinalIgnoreCase))
                                    btnSwitchSchedule.Text = "Switch to Assembly-2";
                                else if (MachineID.Equals("Assembly-2", StringComparison.OrdinalIgnoreCase))
                                    btnSwitchSchedule.Text = "Switch to Assembly-1";

                                if (MachineID.Equals("Balancing", StringComparison.OrdinalIgnoreCase))
                                    btnSwitchSchedule.Text = "Switch to Balancing-2";
                                else if (MachineID.Equals("Balancing-2", StringComparison.OrdinalIgnoreCase))
                                    btnSwitchSchedule.Text = "Switch to Balancing";
                            }
                            else
                            {
                                btnSwitchSchedule.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        btnSwitchSchedule.Visible = false;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No machine id available.')", true);
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void lstProdScheduler_Sorting(object sender, ListViewSortEventArgs e)
        {
            ImageButton imgBtnSortPriority = lstProdScheduler.FindControl("imgBtnSortPriority") as ImageButton;
            ImageButton imgBtnSortStatus = lstProdScheduler.FindControl("imgBtnSortStatus") as ImageButton;
            string DefaultSortIMG = "~/Image/asc.gif";
            string imgUrl = "~/Image/desc.gif";
            if (ViewState["SortExpression"] != null)
            {
                if (ViewState["SortExpression"].ToString() == e.SortExpression)
                {
                    ViewState["SortExpression"] = null;
                    imgUrl = DefaultSortIMG;
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression;
                }
            }
            else
            {
                ViewState["SortExpression"] = e.SortExpression;
            }

            switch (e.SortExpression)
            {
                case "Priority":
                    if (imgBtnSortStatus != null)
                        imgBtnSortStatus.ImageUrl = DefaultSortIMG;
                    if (imgBtnSortPriority != null)
                        imgBtnSortPriority.ImageUrl = imgUrl;
                    break;
                case "Status":
                    if (imgBtnSortPriority != null)
                        imgBtnSortPriority.ImageUrl = DefaultSortIMG;
                    if (imgBtnSortStatus != null)
                        imgBtnSortStatus.ImageUrl = imgUrl;
                    break;
                default:
                    if (imgBtnSortPriority != null)
                        imgBtnSortPriority.ImageUrl = DefaultSortIMG;
                    if (imgBtnSortStatus != null)
                        imgBtnSortStatus.ImageUrl = DefaultSortIMG;
                    break;

            }
            BindProductionScheduleData(e.SortExpression + " " + ((ViewState["SortExpression"] != null) ? "ASC" : "DESC"));
            LoadPriorities();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
            if (ddlMachineId.SelectedValue.ToString().Equals("Quality Incoming") || ddlMachineId.SelectedValue.ToString().Equals("Quality In house"))
            {
                if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = true;
                if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = true;
                if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = true;
                foreach (ListViewItem itm in lstProdScheduler.Items)
                {
                    if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = true;
                    if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = true;
                    if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = true;
                }
            }
            else
            {
                if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = false;
                if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = false;
                if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = false;
                foreach (ListViewItem itm in lstProdScheduler.Items)
                {
                    if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = false;
                    if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = false;
                    if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = false;
                }
            }

            LoadPriorities();
            LoadMaterialIDs();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                string ImportMethod = ddlImportMethod.SelectedItem != null ? ddlImportMethod.SelectedValue : "";
                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";


                if (FileUploadSchedule.HasFile)
                {
                    if (Path.GetExtension(FileUploadSchedule.FileName) == ".xlsx")
                    {
                        lblMessages.Text = string.Empty;
                        ExcelPackage Excel = new ExcelPackage(FileUploadSchedule.PostedFile.InputStream);

                        var worksheetSchedule = Excel.Workbook.Worksheets.First();
                        if (worksheetSchedule.Dimension.End.Column > 9)
                        {
                            //if (FileUploadSchedule.HasFile)
                            //{
                            //    if (Path.GetExtension(FileUploadSchedule.FileName) == ".xlsx")
                            //    {
                            lblMessages.Text = string.Empty;
                            DataTable dtScheduleData = new DataTable();
                            //ExcelPackage Excel = new ExcelPackage(FileUploadSchedule.PostedFile.InputStream);
                            //var worksheetSchedule = Excel.Workbook.Worksheets.First();
                            foreach (var columnHeader in worksheetSchedule.Cells[1, 1, 1, worksheetSchedule.Dimension.End.Column])
                                dtScheduleData.Columns.Add(columnHeader.Text.Trim());
                            // dtScheduleData.Columns[0].DataType = typeof(int);
                            for (int rowNum = 2; rowNum <= worksheetSchedule.Dimension.End.Row; rowNum++)
                            {
                                var worksheetRow = worksheetSchedule.Cells[rowNum, 1, rowNum, worksheetSchedule.Dimension.End.Column];
                                if (worksheetRow != null)
                                {
                                    DataRow dtScheduleDataRow = dtScheduleData.Rows.Add();
                                    foreach (var scheduleData in worksheetRow)
                                    {
                                        //if (scheduleData.Start.Column == 3)
                                        //{
                                        //    try
                                        //    {
                                        //        dtScheduleDataRow[scheduleData.Start.Column - 1] = Convert.ToInt32(scheduleData.Text.Trim());
                                        //    }
                                        //    catch (Exception ex)
                                        //    {
                                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data mismatch at Priority  : " + scheduleData.Text.ToString() + " Is Not an Integer!!')", true);
                                        //        return;
                                        //    }
                                        //}

                                        //else
                                        dtScheduleDataRow[scheduleData.Start.Column - 1] = scheduleData.Text.Trim();
                                    }
                                }
                            }
                            if (dtScheduleData != null && dtScheduleData.Rows.Count > 0)
                            {
                                //dtScheduleData.Columns[0].ColumnName = "SlNo";
                                dtScheduleData.Columns[0].ColumnName = "Machine";
                                //dtScheduleData.Columns[2].ColumnName = "Priority";
                                dtScheduleData.Columns[1].ColumnName = "LocalExport";
                                dtScheduleData.Columns[2].ColumnName = "ProductionOrder";
                                dtScheduleData.Columns[3].ColumnName = "SaleOrder";
                                dtScheduleData.Columns[4].ColumnName = "OperationNumber";
                                dtScheduleData.Columns[5].ColumnName = "Model";
                                dtScheduleData.Columns[6].ColumnName = "ScrollWelded";
                                dtScheduleData.Columns[7].ColumnName = "RDDMachines";
                                dtScheduleData.Columns[8].ColumnName = "FabricationNumber";

                                //if (dtScheduleData.AsEnumerable().Any(x => x.Field<string>("Quantity") != "1"))
                                //{
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Quantity must be 1. Change the quantity to 1 and try again.')", true);
                                //    return;
                                //}
                                foreach (DataRow scheduleDataRow in dtScheduleData.Rows)
                                {
                                    if (!GEADatabaseAccess.IsCompOpnExists(scheduleDataRow["Model"].ToString(), scheduleDataRow["OperationNumber"].ToString()))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data mismatch. Operation ID : " + scheduleDataRow["OperationNumber"].ToString() + " does not exists for selected material id : " + scheduleDataRow["Model"].ToString() + "')", true);
                                        return;
                                    }
                                }
                                foreach (DataRow scheduleDataRow in dtScheduleData.Rows)
                                {
                                    string process = scheduleDataRow["Machine"].ToString();
                                    if (!GEADatabaseAccess.IsProdMatIdUnique(scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["Model"].ToString(), "", process))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data inconsistency. Another material ID exists for the production order : " + scheduleDataRow["ProductionOrder"].ToString() + "')", true);
                                        return;
                                    }
                                    if (GEADatabaseAccess.IsProdMatIdOpnExists(scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["Model"].ToString(), scheduleDataRow["OperationNumber"].ToString(), "", process))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data inconsistency. Material ID : " + scheduleDataRow["Model"].ToString() + " already exists for Production Order : " + scheduleDataRow["ProductionOrder"].ToString() + " and Operation : " + scheduleDataRow["OperationNumber"].ToString() + "')", true);
                                        return;
                                    }
                                    if (process.Equals("QualityInhouse", StringComparison.OrdinalIgnoreCase) || process.Equals("Stores", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (GEADatabaseAccess.IsAutoScheduleMachineAssigned(scheduleDataRow["Model"].ToString(), scheduleDataRow["OperationNumber"].ToString()) == false)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Please assign Auto Schedule Machine for  Material ID : " + scheduleDataRow["Model"].ToString() + " and Material id : " + scheduleDataRow["OperationNumber"].ToString() + "')", true);
                                            return;
                                        }
                                    }
                                }
                                foreach (DataRow scheduleDataRow in dtScheduleData.Rows)
                                {
                                    if (GEADatabaseAccess.IsAssemblyScheduleExists(scheduleDataRow["Machine"].ToString(), scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["Model"].ToString(), scheduleDataRow["OperationNumber"].ToString(), scheduleDataRow["FabricationNumber"].ToString()))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data mismatch. Machine ID: " + scheduleDataRow["Machine"].ToString() + ", Production Order: " + scheduleDataRow["ProductionOrder"].ToString() + ", Material: " + scheduleDataRow["Model"].ToString() + ", Operation ID : " + scheduleDataRow["OperationNumber"].ToString() + "Fabrication No.: " + scheduleDataRow["FabricationNumber"].ToString() + " combination already exists.')", true);
                                        return;
                                    }
                                }
                                //try
                                //{
                                //    dtScheduleData = dtScheduleData.AsEnumerable().OrderBy(x => x.Field<int>("Priority")).CopyToDataTable();
                                //}
                                //catch (Exception ex)
                                //{
                                //    Logger.WriteErrorLog(ex.ToString());
                                //    return;
                                //}

                                var distinctMachineToDelete = dtScheduleData.AsEnumerable().Select(x => x["Machine"]).Distinct().ToList();

                                if (ImportMethod.Equals("DeleteAndImport"))
                                {
                                    for (int m = 0; m < distinctMachineToDelete.Count; m++)
                                    {
                                        GEADatabaseAccess.DeleteNewProductionSchedules(distinctMachineToDelete[m].ToString());
                                    }

                                }
                                string errorMsg = "";
                                foreach (DataRow scheduleDataRow in dtScheduleData.Rows)
                                {
                                    errorMsg = "";
                                    IsSaved = GEADatabaseAccess.ImportSchedueData_Assembly(scheduleDataRow["Machine"].ToString(), "0", scheduleDataRow["LocalExport"].ToString(), scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["SaleOrder"].ToString(), scheduleDataRow["OperationNumber"].ToString(), scheduleDataRow["Model"].ToString(), scheduleDataRow["ScrollWelded"].ToString(), scheduleDataRow["RDDMachines"].ToString(), scheduleDataRow["FabricationNumber"].ToString(), scheduleDataRow["Customer"].ToString(), scheduleDataRow["Location"].ToString(), out errorMsg);
                                    if (!IsSaved) break;
                                }

                                if (IsSaved)
                                {
                                    //var distinctMachine = dtScheduleData.AsEnumerable().Select(x => x["Machine"]).Distinct().ToList();
                                    //foreach (string machineName in distinctMachine)
                                    //{
                                    //    GEADatabaseAccess.InsertDefalultSubActivities_WhileImport(machineName);
                                    //}
                                    var distinctRow = dtScheduleData.AsEnumerable().Select(x => new { machine = x["Machine"].ToString(), prodOrder = x["ProductionOrder"].ToString(), compID = x["Model"].ToString() }).Distinct().ToList();
                                    foreach (var row in distinctRow)
                                    {
                                        GEADatabaseAccess.InsertDefalultSubActivities_WhileImport(row.machine, row.compID, row.prodOrder);
                                    }
                                    lblMessages.ForeColor = Color.Green;
                                    lblMessages.Text = "Data imported successfully";
                                }
                                else
                                {
                                    lblMessages.ForeColor = Color.Red;
                                    lblMessages.Text = "Error. Cannot import proper data." + errorMsg;
                                }
                                BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('No data found in excel file.')", true);
                                return;
                            }
                            //    }
                            //    else
                            //    {
                            //        lblMessages.ForeColor = Color.Red;
                            //        lblMessages.Text = "Wrong file format. File to be imported must be a xlsx excel file.";
                            //    }
                            //}
                            //else
                            //{
                            //    lblMessages.ForeColor = Color.Red;
                            //    lblMessages.Text = "No file chosen for import.";
                            //}
                        }
                        else
                        {
                            //if (FileUploadSchedule.HasFile)
                            //{
                            //    if (Path.GetExtension(FileUploadSchedule.FileName) == ".xlsx")
                            //    {
                            lblMessages.Text = string.Empty;
                            DataTable dtScheduleData = new DataTable();
                            //  ExcelPackage Excel = new ExcelPackage(FileUploadSchedule.PostedFile.InputStream);
                            //var worksheetSchedule = Excel.Workbook.Worksheets.First();
                            foreach (var columnHeader in worksheetSchedule.Cells[1, 1, 1, worksheetSchedule.Dimension.End.Column])
                                dtScheduleData.Columns.Add(columnHeader.Text.Trim());
                            //dtScheduleData.Columns[0].DataType = typeof(int);
                            for (int rowNum = 2; rowNum <= worksheetSchedule.Dimension.End.Row; rowNum++)
                            {
                                var worksheetRow = worksheetSchedule.Cells[rowNum, 1, rowNum, worksheetSchedule.Dimension.End.Column];
                                if (worksheetRow != null)
                                {
                                    DataRow dtScheduleDataRow = dtScheduleData.Rows.Add();
                                    foreach (var scheduleData in worksheetRow)
                                    {
                                        //if (scheduleData.Start.Column == 1)
                                        //{
                                        //    try
                                        //    {
                                        //        if (!string.IsNullOrEmpty(scheduleData.Text.Trim()))
                                        //            dtScheduleDataRow[scheduleData.Start.Column - 1] = Convert.ToInt32(scheduleData.Text.Trim());
                                        //        else
                                        //        {

                                        //        }
                                        //    }
                                        //    catch (Exception ex)
                                        //    {
                                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data mismatch at Priority  : " + scheduleData.Text.ToString() + " Is Not an Integer!!')", true);
                                        //        return;
                                        //    }
                                        //}

                                        //else
                                        dtScheduleDataRow[scheduleData.Start.Column - 1] = scheduleData.Text.Trim();
                                    }
                                }
                            }
                            if (dtScheduleData != null && dtScheduleData.Rows.Count > 0)
                            {
                                // dtScheduleData.Columns[0].ColumnName = "Priority";
                                dtScheduleData.Columns[0].ColumnName = "MachineID";
                                dtScheduleData.Columns[1].ColumnName = "ProductionOrder";
                                dtScheduleData.Columns[2].ColumnName = "MaterialID";
                                dtScheduleData.Columns[3].ColumnName = "OperationNumber";
                                dtScheduleData.Columns[4].ColumnName = "Quantity";
                                if (dtScheduleData.AsEnumerable().Any(x => x.Field<string>("MachineID").Contains("Quality")))
                                {
                                    dtScheduleData.Columns[5].ColumnName = "GRNNumber";
                                    dtScheduleData.Columns[6].ColumnName = "SupplierName";
                                    dtScheduleData.Columns[7].ColumnName = "NewProdDevelopment";
                                }
                                if (!dtScheduleData.AsEnumerable().Any(x => x.Field<string>("MachineID").Equals("Quality Incoming")))
                                {
                                    if (dtScheduleData.AsEnumerable().Any(x => x.Field<string>("Quantity") != "1"))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Quantity must be 1. Change the quantity to 1 and try again.')", true);
                                        return;
                                    }
                                }
                                foreach (DataRow scheduleDataRow in dtScheduleData.Rows)
                                {
                                    string process = getMachineIdProcess(scheduleDataRow["MachineID"].ToString());
                                    if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (string.IsNullOrEmpty(scheduleDataRow["GRNNumber"].ToString()))
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Please enter GRN Number for ProductionOrder " + scheduleDataRow["ProductionOrder"].ToString() + ".')", true);
                                            return;
                                        }
                                    }
                                    if (!GEADatabaseAccess.IsCompOpnExists(scheduleDataRow["MaterialID"].ToString(), scheduleDataRow["OperationNumber"].ToString()))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data mismatch. Operation ID : " + scheduleDataRow["OperationNumber"].ToString() + " does not exists for selected material id : " + scheduleDataRow["MaterialID"].ToString() + "')", true);
                                        return;
                                    }

                                    if (process.Equals("QualityInhouse", StringComparison.OrdinalIgnoreCase) || process.Equals("Stores", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (GEADatabaseAccess.IsAutoScheduleMachineAssigned(scheduleDataRow["MaterialID"].ToString(), scheduleDataRow["OperationNumber"].ToString()) == false)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Please assign Auto Schedule Machine for  Material ID : " + scheduleDataRow["MaterialID"].ToString() + " and Material id : " + scheduleDataRow["OperationNumber"].ToString() + "')", true);
                                            return;
                                        }
                                    }
                                }
                                foreach (DataRow scheduleDataRow in dtScheduleData.Rows)
                                {
                                    string process = getMachineIdProcess(scheduleDataRow["MachineID"].ToString());
                                    string grnNumber = "";
                                    if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
                                    {
                                        grnNumber = scheduleDataRow["GRNNumber"].ToString();
                                    }
                                    if (!GEADatabaseAccess.IsProdMatIdUnique(scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["MaterialID"].ToString(), grnNumber, process))
                                    {
                                        if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data inconsistency. Another material ID exists for the production order : " + scheduleDataRow["ProductionOrder"].ToString() + " and GRN Number : " + scheduleDataRow["GRNNumber"].ToString() + "')", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data inconsistency. Another material ID exists for the production order : " + scheduleDataRow["ProductionOrder"].ToString() + "')", true);
                                        }
                                        return;
                                    }
                                    if (GEADatabaseAccess.IsProdMatIdOpnExists(scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["MaterialID"].ToString(), scheduleDataRow["OperationNumber"].ToString(), grnNumber, process))
                                    {
                                        if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data inconsistency. Material ID : " + scheduleDataRow["MaterialID"].ToString() + " already exists for Production Order : " + scheduleDataRow["ProductionOrder"].ToString() + ", GRN Number " + scheduleDataRow["GRNNumber"].ToString() + " and Operation : " + scheduleDataRow["OperationNumber"].ToString() + "')", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Excel data inconsistency. Material ID : " + scheduleDataRow["MaterialID"].ToString() + " already exists for Production Order : " + scheduleDataRow["ProductionOrder"].ToString() + " and Operation : " + scheduleDataRow["OperationNumber"].ToString() + "')", true);
                                        }
                                        return;
                                    }
                                }
                                //try
                                //{
                                //    dtScheduleData = dtScheduleData.AsEnumerable().OrderBy(x => x.Field<int>("Priority")).CopyToDataTable();
                                //}
                                //catch (Exception ex)
                                //{
                                //    Logger.WriteErrorLog(ex.ToString());
                                //    return;
                                //}

                                //if (ImportMethod.Equals("DeleteAndImport"))
                                //    GEADatabaseAccess.DeleteNewProductionSchedules("");


                                var distinctMachineToDelete = dtScheduleData.AsEnumerable().Select(x => x["MachineID"]).Distinct().ToList();
                                if (ImportMethod.Equals("DeleteAndImport"))
                                {
                                    for (int m = 0; m < distinctMachineToDelete.Count; m++)
                                    {
                                        GEADatabaseAccess.DeleteNewProductionSchedules(distinctMachineToDelete[m].ToString());
                                    }
                                }

                                foreach (DataRow scheduleDataRow in dtScheduleData.Rows)
                                {
                                    string process = getMachineIdProcess(scheduleDataRow["MachineID"].ToString());
                                    if (scheduleDataRow["MachineID"].ToString().Contains("Quality"))
                                    {
                                        IsSaved = GEADatabaseAccess.ImportQualitySchedueData("0", scheduleDataRow["MachineID"].ToString(), scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["MaterialID"].ToString(), scheduleDataRow["OperationNumber"].ToString(), scheduleDataRow["Quantity"].ToString(), scheduleDataRow["GRNNumber"].ToString(), scheduleDataRow["SupplierName"].ToString(), scheduleDataRow["NewProdDevelopment"].ToString(), process);
                                    }
                                    else
                                    {

                                        IsSaved = GEADatabaseAccess.ImportSchedueData("0", scheduleDataRow["MachineID"].ToString(), scheduleDataRow["ProductionOrder"].ToString(), scheduleDataRow["MaterialID"].ToString(), scheduleDataRow["OperationNumber"].ToString(), scheduleDataRow["Quantity"].ToString());
                                    }
                                    if (!IsSaved) break;
                                }
                                if (IsSaved)
                                {
                                    lblMessages.ForeColor = Color.Green;
                                    lblMessages.Text = "Data imported successfully";
                                }
                                else
                                {
                                    lblMessages.ForeColor = Color.Red;
                                    lblMessages.Text = "Error. Cannot import proper data.";
                                }
                                BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('No data found in excel file.')", true);
                                return;
                            }
                            //    }
                            //    else
                            //    {
                            //        lblMessages.ForeColor = Color.Red;
                            //        lblMessages.Text = "Wrong file format. File to be imported must be a xlsx excel file.";
                            //    }
                            //}
                            //else
                            //{
                            //    lblMessages.ForeColor = Color.Red;
                            //    lblMessages.Text = "No file chosen for import.";
                            //}
                        }
                    }
                    else
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Wrong file format. File to be imported must be a xlsx excel file.";
                    }
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "No file chosen for import.";
                }




            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                // string materialId = ddlMaterialID.SelectedItem != null ? ddlMaterialID.SelectedValue : "";
                string materialId = hdnMaterialId.Value;
                //string opnNum = ddlOperationNum.SelectedItem != null ? ddlOperationNum.SelectedValue : "";
                string[] OpnNum = null;
                HidOpn.Value = HidOpn.Value.TrimEnd(',');
                if (!string.IsNullOrEmpty(HidOpn.Value))
                {
                    OpnNum = HidOpn.Value.Split(',');
                }
                if (OpnNum == null) return;
                string prodOrder = txtProdOrder.Text;
                string process = getMachineIdProcess(machine);
                if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(txtAddGRNNumber.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Please enter GRN Number')", true);
                        return;
                    }
                }
                if (!GEADatabaseAccess.IsProdMatIdUnique(prodOrder, materialId, txtAddGRNNumber.Text, process))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Another material ID exists for the production order : " + prodOrder + "')", true);
                    return;
                }
                int opnCount = 0;
                foreach (string opnNum in OpnNum)
                {
                    if (GEADatabaseAccess.IsProdMatIdOpnExists(prodOrder, materialId, opnNum, txtAddGRNNumber.Text, process))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Cannot save as selected material ID and Operation already exists for the production order.')", true);
                        return;
                    }
                    if (!GEADatabaseAccess.IsMachineProdMatIdOpnExists(machine, prodOrder, materialId, opnNum, txtAddGRNNumber.Text, process))
                    {
                        if (radioMoveEnd.Checked)
                        {
                            int UserPriority = 0;
                            if (machine.Equals("Quality Incoming", StringComparison.OrdinalIgnoreCase) || machine.Equals("Quality In house", StringComparison.OrdinalIgnoreCase))
                            {
                                IsSaved = GEADatabaseAccess.UpdateQualityScheduleDetails((GEADatabaseAccess.GetMaxSchedulePriority(out UserPriority) + 1).ToString(), machine, txtProdOrder.Text, materialId, opnNum, txtQuantity.Text, txtAddGRNNumber.Text, txtSupplierName.Text, txtAddNewProdDev.Checked, process);
                                //IsSaved = GEADatabaseAccess.UpdateQualityScheduleDetails((GEADatabaseAccess.GetMaxSchedulePriority(out UserPriority) + 1).ToString(), machine, txtProdOrder.Text, materialId, opnNum, txtQuantity.Text, txtAddGRNNumber.Text, txtSupplierName.Text, txtAddNewProdDev.Checked);
                            }
                            else
                            {
                                IsSaved = GEADatabaseAccess.UpdateScheduleDetails((GEADatabaseAccess.GetMaxSchedulePriority(out UserPriority) + 1).ToString(), machine, txtProdOrder.Text, materialId, opnNum, txtQuantity.Text, "", "", false);
                            }
                        }
                        if (radioMove2Before.Checked)
                        {
                            int priority = Convert.ToInt32(ddlAddPriority.SelectedValue);
                            if (opnCount != 0)
                            {
                                priority += opnCount;
                            }
                            if (ddlAddPriority.SelectedItem != null)
                            {
                                if (machine.Equals("Quality Incoming", StringComparison.OrdinalIgnoreCase) || machine.Equals("Quality In house", StringComparison.OrdinalIgnoreCase))
                                {
                                    //int priority = Convert.ToInt32(ddlAddPriority.SelectedValue);
                                    IsSaved = GEADatabaseAccess.InsertQualityScheduleDetailsBefore(priority, machine, prodOrder, materialId, opnNum, txtQuantity.Text, txtAddGRNNumber.Text, txtSupplierName.Text, txtAddNewProdDev.Checked, process);
                                }
                                else
                                {
                                    //int priority = Convert.ToInt32(ddlAddPriority.SelectedValue);
                                    IsSaved = GEADatabaseAccess.InsertScheduleDetailsBefore(priority, machine, prodOrder, materialId, opnNum, txtQuantity.Text, "", "", false);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Entered prod. order, machine, material id and operation already exists.')", true);
                        return;
                    }
                    opnCount++;
                }
                if (IsSaved)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data updated successfully.')", true);
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                    if (ddlMachineId.SelectedValue.ToString().Equals("Quality Incoming") || ddlMachineId.SelectedValue.ToString().Equals("Quality In house"))
                    {
                        if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = true;
                        if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = true;
                        if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = true;
                        foreach (ListViewItem itm in lstProdScheduler.Items)
                        {
                            if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = true;
                            if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = true;
                            if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = true;
                        }
                    }
                    else
                    {
                        if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = false;
                        if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = false;
                        if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = false;
                        foreach (ListViewItem itm in lstProdScheduler.Items)
                        {
                            if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = false;
                            if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = false;
                            if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = false;
                        }
                    }
                    LoadPriorities();
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void lstProdScheduler_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (lstProdScheduler.EditIndex == (e.Item as ListViewDataItem).DataItemIndex)
            {
                string macId = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                DropDownList ddlMatID = e.Item.FindControl("ddlMatID") as DropDownList;
                List<string> listMaterialIds = new List<string>();
                if (Session["MaterialIds"] == null)
                    listMaterialIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllComponents();
                else
                    listMaterialIds = Session["MaterialIds"] as List<string>;
                if (listMaterialIds != null && listMaterialIds.Count > 0)
                {
                    ddlMatID.DataSource = listMaterialIds;
                    ddlMatID.DataBind();
                    Label lblMatId = (e.Item.FindControl("lblMaterialID") as Label);
                    ddlMatID.Items.FindByValue(lblMatId.Text).Selected = true;
                }

                string matId = ddlMatID.SelectedItem != null ? ddlMatID.SelectedValue : "";
                DropDownList ddlOpnNos = e.Item.FindControl("ddlOpnNum") as DropDownList;
                List<string> listOpeartions = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation(macId, matId);
                if (listOpeartions != null && listOpeartions.Count > 0)
                {
                    ddlOpnNos.DataSource = listOpeartions;
                    ddlOpnNos.DataBind();
                    Label lblOpnNum = (e.Item.FindControl("lblOpnNumber") as Label);
                    ddlOpnNos.Items.FindByValue(lblOpnNum.Text).Selected = true;
                }
            }

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string status = (e.Item.FindControl("lblStatus") as Label).Text;
                CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;
                if (status != null && chkSelect != null)
                {
                    if (status.Equals("Running") || status.Equals("Completed", StringComparison.OrdinalIgnoreCase) || status.Equals("Parked", StringComparison.OrdinalIgnoreCase)) chkSelect.Visible = false;
                    else chkSelect.Visible = true;
                }
            }
        }

        protected void lstProdScheduler_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            var currentItem = lstProdScheduler.Items[e.NewEditIndex];
            string status = (currentItem.FindControl("lblStatus") as Label).Text;
            if (!string.IsNullOrEmpty(status) && !status.Equals("Running"))
            {
                lstProdScheduler.EditIndex = e.NewEditIndex;
                BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
            }
            else
            {
                lstProdScheduler.EditIndex = -1;
                BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
            }
        }

        protected void lstProdScheduler_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lstProdScheduler.EditIndex = -1;
            BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
        }

        protected void lstProdScheduler_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            bool IsSaved = false;
            try
            {
                string macId = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string Idd = (lstProdScheduler.Items[e.ItemIndex].FindControl("hdfUpdate") as HiddenField).Value;
                string priority = (lstProdScheduler.Items[e.ItemIndex].FindControl("lblPriority") as Label).Text;
                string old_prodno = (lstProdScheduler.Items[e.ItemIndex].FindControl("lblProdOrderNumber") as Label).Text;
                string new_prodno = (lstProdScheduler.Items[e.ItemIndex].FindControl("txtPoNum") as TextBox).Text;
                string old_matid = (lstProdScheduler.Items[e.ItemIndex].FindControl("lblMaterialID") as Label).Text;
                string new_matid = (lstProdScheduler.Items[e.ItemIndex].FindControl("ddlMatID") as DropDownList).SelectedValue;
                string old_opnno = (lstProdScheduler.Items[e.ItemIndex].FindControl("lblOpnNumber") as Label).Text;
                string new_opnno = (lstProdScheduler.Items[e.ItemIndex].FindControl("ddlOpnNum") as DropDownList).SelectedValue;
                string qty = (lstProdScheduler.Items[e.ItemIndex].FindControl("txtQty") as TextBox).Text;
                // string grnNumber = (lstProdScheduler.Items[e.ItemIndex].FindControl("lblGRNNumber") as Label).Text;
                if (!string.IsNullOrEmpty(Idd) && !string.IsNullOrEmpty(macId) && !string.IsNullOrEmpty(new_matid) && !string.IsNullOrEmpty(new_opnno))
                {
                    IsSaved = GEADatabaseAccess.UpdateScheduleData(Idd, macId, priority, old_prodno, new_prodno, old_matid, new_matid, old_opnno, new_opnno, qty);
                    if (IsSaved)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data updated successfully.')", true);
                        lstProdScheduler.EditIndex = -1;
                        BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                string macId = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string Idd = hdfIdd.Value;
                string old_prodno = hdfProd.Value;
                string old_matid = hdfMat.Value;
                string new_prodno = txtUpdtProdOrder.Text;
                string new_matid = ddlUpdtMatId.SelectedItem != null ? ddlUpdtMatId.SelectedValue : "";
                string new_opnno = ddlUpdtOpnNum.SelectedItem != null ? ddlUpdtOpnNum.SelectedValue : "";
                string qty = txtUpdtQuantity.Text;
                string process = getMachineIdProcess(macId);
                if (!string.IsNullOrEmpty(Idd) && !string.IsNullOrEmpty(macId) && !string.IsNullOrEmpty(new_matid) && !string.IsNullOrEmpty(new_opnno))
                {
                    if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(txtUpdateGRNNumber.Text))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "GRN Number cannot be empty.";
                            return;
                        }
                        if (!new_prodno.Equals(old_prodno) || !new_matid.Equals(old_matid) || !txtUpdateGRNNumber.Text.Equals(hdnGRNNumber.Value))
                        {
                            if (!GEADatabaseAccess.IsProdMatIdUnique(new_prodno, new_matid, txtUpdateGRNNumber.Text, process))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Another material ID exists for the production order : " + new_prodno + " and GRN Number " + txtUpdateGRNNumber.Text + " ')", true);
                                return;
                            }
                            if (GEADatabaseAccess.IsProdMatIdOpnExists(new_prodno, new_matid, new_opnno, txtUpdateGRNNumber.Text, process))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Cannot save as selected material ID already exists for the production order, GRN Number and Operation.')", true);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!new_prodno.Equals(old_prodno) || !new_matid.Equals(old_matid))
                        {
                            if (!GEADatabaseAccess.IsProdMatIdUnique(new_prodno, old_matid, txtUpdateGRNNumber.Text, process))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Another material ID exists for the production order : " + new_prodno + "')", true);
                                return;
                            }
                            if (GEADatabaseAccess.IsProdMatIdOpnExists(new_prodno, new_matid, new_opnno, txtUpdateGRNNumber.Text, process))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Cannot save as selected material ID already exists for the production order and Operation.')", true);
                                return;
                            }
                        }
                    }
                    if (macId.Equals("Quality Incoming", StringComparison.OrdinalIgnoreCase) || macId.Equals("Quality In house", StringComparison.OrdinalIgnoreCase))
                    {
                        IsSaved = GEADatabaseAccess.UpdateQualityScheduleData(Idd, macId, new_prodno, new_matid, new_opnno, qty, txtUpdateGRNNumber.Text, txtUpdateSupplierName.Text, chkUpdateNewProdDev.Checked);
                    }
                    else
                    {
                        IsSaved = GEADatabaseAccess.UpdateScheduleData(Idd, macId, new_prodno, new_matid, new_opnno, qty, txtUpdateGRNNumber.Text, txtUpdateSupplierName.Text, chkUpdateNewProdDev.Checked);
                    }



                    if (IsSaved)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data updated successfully.')", true);
                        BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                        if (ddlMachineId.SelectedValue.ToString().Equals("Quality Incoming") || ddlMachineId.SelectedValue.ToString().Equals("Quality In house"))
                        {
                            if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = true;
                            if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = true;
                            if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = true;
                            foreach (ListViewItem itm in lstProdScheduler.Items)
                            {
                                if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = true;
                                if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = true;
                                if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = true;
                            }
                        }
                        else
                        {
                            if (lstProdScheduler.FindControl("tdNewProdDev") != null) lstProdScheduler.FindControl("tdNewProdDev").Visible = false;
                            if (lstProdScheduler.FindControl("tdSupplierName") != null) lstProdScheduler.FindControl("tdSupplierName").Visible = false;
                            if (lstProdScheduler.FindControl("tdGRNNumber") != null) lstProdScheduler.FindControl("tdGRNNumber").Visible = false;
                            foreach (ListViewItem itm in lstProdScheduler.Items)
                            {
                                if (itm.FindControl("tdEditGRN") != null) itm.FindControl("tdEditGRN").Visible = false;
                                if (itm.FindControl("tdEditSuppName") != null) itm.FindControl("tdEditSuppName").Visible = false;
                                if (itm.FindControl("tdEditNPD") != null) itm.FindControl("tdEditNPD").Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Material ID and Operation Number cannot be empty.";
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void lstProdScheduler_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Edit"))
            {
                string status = (e.Item.FindControl("lblStatus") as Label).Text;
                if (!string.IsNullOrEmpty(status) && status.Equals("Running"))
                    e.Handled = true;
            }
        }

        protected void btnDeleteSch_Click(object sender, EventArgs e)
        {
            try
            {

                bool IsDeleted = false;
                if (lstProdScheduler.Visible)
                {
                    foreach (ListViewItem listViewItem in lstProdScheduler.Items)
                    {
                        if (listViewItem.FindControl("chkSelect") is CheckBox)
                        {
                            CheckBox chkSelect = listViewItem.FindControl("chkSelect") as CheckBox;
                            if (chkSelect.Checked)
                            {
                                if ((listViewItem.FindControl("lblStatus") as Label).Text.Equals("New"))
                                {
                                    string Idd = (listViewItem.FindControl("hdfUpdate") as HiddenField).Value;
                                    IsDeleted = GEADatabaseAccess.DeleteProdSchedule(Idd);
                                    if (!IsDeleted) break;
                                }
                            }
                        }
                    }
                    if (IsDeleted)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data deleted successfully.')", true);
                    }
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }
                else
                {
                    foreach (ListViewItem listViewItem in lvAssemblyDetails.Items)
                    {
                        if (listViewItem.FindControl("chkSelect") is CheckBox)
                        {
                            CheckBox chkSelect = listViewItem.FindControl("chkSelect") as CheckBox;
                            if (chkSelect.Checked)
                            {
                                if ((listViewItem.FindControl("lblStatus") as Label).Text.Equals("New"))
                                {
                                    string Idd = (listViewItem.FindControl("hdfUpdate") as HiddenField).Value;
                                    IsDeleted = GEADatabaseAccess.DeleteProdSchedule(Idd);
                                    if (!IsDeleted) break;
                                }
                            }
                        }
                    }
                    if (IsDeleted)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data deleted successfully.')", true);
                    }
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }

            }
            catch (Exception ex)
            {

            }

        }

        protected void lstProdScheduler_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            try
            {
                CheckBox headercheck = lstProdScheduler.FindControl("chkHeaderSelect") as CheckBox;
                headercheck.Checked = false;
                DataPager dataPagerSchedule = (lstProdScheduler.FindControl("dataPagerSchedule") as DataPager);
                if (dataPagerSchedule != null)
                {
                    dataPagerSchedule.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            bool IsChanged = false;
            try
            {
                if (lstProdScheduler.Visible)
                {
                    foreach (ListViewItem listViewItem in lstProdScheduler.Items)
                    {
                        if (listViewItem.FindControl("chkSelect") is CheckBox && listViewItem.FindControl("lblPriority") is Label)
                        {
                            CheckBox chkSelect = listViewItem.FindControl("chkSelect") as CheckBox;
                            Label lblPrty = listViewItem.FindControl("lblPriority") as Label;
                            Label UserlblPrty = listViewItem.FindControl("lblUserPriority") as Label;
                            string Idd = (listViewItem.FindControl("hdfUpdate") as HiddenField).Value;
                            if (chkSelect.Checked)
                            {
                                if (radioMoveBefore.Checked)
                                {
                                    if (ddlPriorities.SelectedItem != null)
                                    {
                                        int current_priority = Convert.ToInt32(lblPrty.Text);
                                        int new_priority = Convert.ToInt32(ddlPriorities.SelectedValue);
                                        int current_User_priority = Convert.ToInt32(UserlblPrty.Text);
                                        int new_User_priority = Convert.ToInt32(ddlPriorities.SelectedItem.Text);
                                        IsChanged = GEADatabaseAccess.ChangeSchedulePriority(Idd, current_priority, new_priority, "MoveBefore", current_User_priority, new_User_priority);
                                        GEADatabaseAccess.UpdatePriorityChanged(Idd);
                                    }
                                }
                                if (radioMoveToEnd.Checked)
                                {
                                    int current_max_User_priority = 0, new_User_priority = 0;
                                    int current_priority = Convert.ToInt32(lblPrty.Text);
                                    int current_User_priority = Convert.ToInt32(UserlblPrty.Text);
                                    int current_max_priority = GEADatabaseAccess.GetMaxSchedulePriority(out current_max_User_priority);
                                    int new_priority = current_max_priority + 1;
                                    new_User_priority = current_max_User_priority + 1;
                                    IsChanged = GEADatabaseAccess.ChangeSchedulePriority(Idd, current_priority, new_priority, "MoveToEnd", current_User_priority, new_User_priority);
                                    GEADatabaseAccess.UpdatePriorityChanged(Idd);
                                }
                            }
                        }
                    }
                    if (IsChanged)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Schedule priority changed successfully.')", true);
                    }
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }
                else
                {
                    foreach (ListViewItem listViewItem in lvAssemblyDetails.Items)
                    {
                        if (listViewItem.FindControl("chkSelect") is CheckBox && listViewItem.FindControl("lblPriority") is Label)
                        {
                            CheckBox chkSelect = listViewItem.FindControl("chkSelect") as CheckBox;
                            Label lblPrty = listViewItem.FindControl("lblPriority") as Label;
                            Label UserlblPrty = listViewItem.FindControl("lblUserPriority") as Label;
                            string Idd = (listViewItem.FindControl("hdfUpdate") as HiddenField).Value;
                            if (chkSelect.Checked)
                            {
                                if (radioMoveBefore.Checked)
                                {
                                    if (ddlPriorities.SelectedItem != null)
                                    {
                                        int current_priority = Convert.ToInt32(lblPrty.Text);
                                        int new_priority = Convert.ToInt32(ddlPriorities.SelectedValue);
                                        int current_User_priority = Convert.ToInt32(UserlblPrty.Text);
                                        int new_User_priority = Convert.ToInt32(ddlPriorities.SelectedItem.Text);
                                        IsChanged = GEADatabaseAccess.ChangeSchedulePriority(Idd, current_priority, new_priority, "MoveBefore", current_User_priority, new_User_priority);
                                        GEADatabaseAccess.UpdatePriorityChanged(Idd);
                                    }
                                }
                                if (radioMoveToEnd.Checked)
                                {
                                    int current_max_User_priority = 0, new_User_priority = 0;
                                    int current_priority = Convert.ToInt32(lblPrty.Text);
                                    int current_User_priority = Convert.ToInt32(UserlblPrty.Text);
                                    int current_max_priority = GEADatabaseAccess.GetMaxSchedulePriority(out current_max_User_priority);
                                    int new_priority = current_max_priority + 1;
                                    new_User_priority = current_max_User_priority + 1;
                                    IsChanged = GEADatabaseAccess.ChangeSchedulePriority(Idd, current_priority, new_priority, "MoveToEnd", current_User_priority, new_User_priority);
                                    GEADatabaseAccess.UpdatePriorityChanged(Idd);
                                }
                            }
                        }
                    }
                    if (IsChanged)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Schedule priority changed successfully.')", true);
                    }
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }

            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnCalcPlan_Click(object sender, EventArgs e)
        {
            bool success = false;
            try
            {
                string startDate = string.Empty;
                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                if (!string.IsNullOrEmpty(machine))
                {
                    string CalcMethod = ddlCalculationMethod.SelectedItem != null ? ddlCalculationMethod.SelectedValue : "";
                    if (radioRunningSchedule.Checked)
                        startDate = "";
                    if (radioUserInput.Checked)
                        startDate = txtStartTime.Text;
                    if (!string.IsNullOrEmpty(CalcMethod))
                    {
                        success = GEADatabaseAccess.UpdateScheduleCalcDetails(machine, CalcMethod, startDate);
                    }
                }
                if (success)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Calculation plan has started. It will take sometime. Please come back after sometime.')", true);
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Error calculating plan. Please try after sometime.')", true);
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnSaveDefaultCalcMethod_Click(object sender, EventArgs e)
        {
            bool success = false;
            try
            {
                string defaultCalcMethod = ddlDefaultCalcMethod.SelectedItem != null ? ddlDefaultCalcMethod.SelectedItem.ToString() : "";
                int defaultCalcMethodValue = ddlDefaultCalcMethod.SelectedItem != null ? Convert.ToInt32(ddlDefaultCalcMethod.SelectedValue) : 0;
                if (!string.IsNullOrEmpty(defaultCalcMethod))
                    success = GEADatabaseAccess.UpdateDefaultCalculationMethod(defaultCalcMethod, defaultCalcMethodValue);
                if (success)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Default calculation method has been updated successfully.')", true);
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnCancelSch_Click(object sender, EventArgs e)
        {
            bool success = false;
            try
            {
                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string process = getMachineIdProcess(machine);
                if (lstProdScheduler.Visible)
                {
                    foreach (ListViewItem listViewItem in lstProdScheduler.Items)
                    {
                        if (listViewItem.FindControl("chkSelect") is CheckBox)
                        {
                            CheckBox chkSelect = listViewItem.FindControl("chkSelect") as CheckBox;
                            if (chkSelect.Checked)
                            {
                                string Idd = (listViewItem.FindControl("hdfUpdate") as HiddenField).Value;
                                if (!string.IsNullOrEmpty(Idd) && !string.IsNullOrEmpty(machine))
                                {
                                    string ProdOrder = (listViewItem.FindControl("lblProdOrderNumber") as Label).Text;
                                    string MatId = (listViewItem.FindControl("lblMaterialID") as Label).Text;
                                    string OpnNum = (listViewItem.FindControl("lblOpnNumber") as Label).Text;
                                    string grnNo = (listViewItem.FindControl("lblGRNNumber") as Label).Text;
                                    success = GEADatabaseAccess.CancelProdSchedule(Idd, machine, ProdOrder, MatId, OpnNum, grnNo, process);
                                    if (!success) break;
                                }
                            }
                        }
                    }
                    if (success)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data cancelled successfully.')", true);
                    }
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }
                else
                {
                    foreach (ListViewItem listViewItem in lvAssemblyDetails.Items)
                    {
                        if (listViewItem.FindControl("chkSelect") is CheckBox)
                        {
                            CheckBox chkSelect = listViewItem.FindControl("chkSelect") as CheckBox;
                            if (chkSelect.Checked)
                            {
                                string Idd = (listViewItem.FindControl("hdfUpdate") as HiddenField).Value;
                                if (!string.IsNullOrEmpty(Idd) && !string.IsNullOrEmpty(machine))
                                {
                                    string ProdOrder = (listViewItem.FindControl("lblProdOrderNumber") as Label).Text;
                                    string MatId = (listViewItem.FindControl("lblMaterialID") as Label).Text;
                                    string OpnNum = (listViewItem.FindControl("lblOpnNumber") as Label).Text;
                                    string FabNum = (listViewItem.FindControl("lblFabricationNum") as Label).Text;
                                    success = GEADatabaseAccess.CancelProdSchedule_Assembly(Idd, machine, ProdOrder, MatId, OpnNum, FabNum);
                                    if (!success) break;
                                }
                            }
                        }
                    }
                    if (success)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data cancelled successfully.')", true);
                    }
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }

            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlMaterialID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOperationNos("Save");
        }

        protected void ddlUpdtMatId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOperationNos("Update");
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string IsScheduleExists(string machine, string prodOrder, string matId, string opnNum)
        {
            return GEADatabaseAccess.IsScheduleExists(machine, prodOrder, matId, opnNum).Equals(true) ? "true" : "false";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string IsAssemblyScheduleExists(string machine, string prodOrder, string matId, string opnNum, string fabricationNo)
        {
            return GEADatabaseAccess.IsAssemblyScheduleExists(machine, prodOrder, matId, opnNum, fabricationNo).Equals(true) ? "true" : "false";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool IsAutoScheduleAssigned(string compID, string opnNo)
        {
            bool isAssigned = false;
            try
            {
                isAssigned = GEADatabaseAccess.IsAutoScheduleMachineAssigned(compID, opnNo);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return isAssigned;
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            string status = string.Empty;
            List<ScheduleDetailsEntity> ProdScheduleDetails = new List<ScheduleDetailsEntity>();
            List<AssemblyScheduleDetailsEntity> AssemblyScheduleDetails = new List<AssemblyScheduleDetailsEntity>();
            try
            {
                string MachineID = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";

                if (lstProdScheduler.Visible)
                {
                    if (lstProdScheduler.Items.Count > 0)
                    {
                        string from_date = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text : "";
                        string to_date = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text : "";
                        if (Session["ProdScheduleData"] != null)
                        {
                            ProdScheduleDetails = Session["ProdScheduleData"] as List<ScheduleDetailsEntity>;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(MachineID))
                            {
                                foreach (ListItem statusItem in lstStatus.Items)
                                {
                                    if (statusItem.Selected)
                                        status = status + statusItem.Text + ",";
                                }
                                string prod_order = string.Empty;
                                string material_id = string.Empty;
                                if (radioPoNumber.Checked)
                                {
                                    prod_order = !string.IsNullOrEmpty(txtSearchText.Text) ? txtSearchText.Text : "";
                                    material_id = "";
                                }
                                if (radioMaterialId.Checked)
                                {
                                    prod_order = "";
                                    material_id = !string.IsNullOrEmpty(txtSearchText.Text) ? txtSearchText.Text : "";
                                }
                                ProdScheduleDetails = GEADatabaseAccess.GetProductionSchedulesData(MachineID, status, prod_order, material_id, from_date, to_date);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                            }
                        }
                        if (ProdScheduleDetails != null && ProdScheduleDetails.Count > 0)
                        {
                            Session["ProdScheduleData"] = ProdScheduleDetails;
                            successfull = GEAGenerateReport.GenerateProductionScheduleReport(MachineID, from_date, to_date, ProdScheduleDetails);
                            if (successfull)
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportSuccess", "alert('Export Successful.')", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportError", "alert('Error. Export Unsuccessful.')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                    }
                }
                else
                {

                    if (lvAssemblyDetails.Items.Count > 0)
                    {
                        string from_date = !string.IsNullOrEmpty(txtFromDate.Text) ? txtFromDate.Text : "";
                        string to_date = !string.IsNullOrEmpty(txtToDate.Text) ? txtToDate.Text : "";
                        if (Session["AssemblyData"] != null)
                        {
                            AssemblyScheduleDetails = Session["AssemblyData"] as List<AssemblyScheduleDetailsEntity>;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(MachineID))
                            {
                                foreach (ListItem statusItem in lstStatus.Items)
                                {
                                    if (statusItem.Selected)
                                        status = status + statusItem.Text + ",";
                                }
                                string prod_order = string.Empty;
                                string material_id = string.Empty;
                                if (radioPoNumber.Checked)
                                {
                                    prod_order = !string.IsNullOrEmpty(txtSearchText.Text) ? txtSearchText.Text : "";
                                    material_id = "";
                                }
                                if (radioMaterialId.Checked)
                                {
                                    prod_order = "";
                                    material_id = !string.IsNullOrEmpty(txtSearchText.Text) ? txtSearchText.Text : "";
                                }
                                AssemblyScheduleDetails = GEADatabaseAccess.GetAssemblySchedulesData(MachineID, status, prod_order, material_id, from_date, to_date);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                            }
                        }
                        if (AssemblyScheduleDetails != null && AssemblyScheduleDetails.Count > 0)
                        {
                            Session["AssemblyData"] = AssemblyScheduleDetails;
                            successfull = GEAGenerateReport.GenerateAssemblyScheduleReport(MachineID, from_date, to_date, AssemblyScheduleDetails);
                            if (successfull)
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportSuccess", "alert('Export Successful.')", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ExportError", "alert('Error. Export Unsuccessful.')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.DarkRed;
                lblMessages.Text = ex.Message;
            }
        }

        protected void chkHeaderSelect_CheckedChanged(object sender, EventArgs e)
        {
            bool Headercheck = (sender as CheckBox).Checked;
            if (lstProdScheduler.Visible)
            {
                foreach (ListViewItem item in lstProdScheduler.Items)
                {
                    CheckBox itemchk = item.FindControl("chkSelect") as CheckBox;
                    itemchk.Checked = Headercheck;
                }
            }
            else
            {
                foreach (ListViewItem item in lvAssemblyDetails.Items)
                {
                    CheckBox itemchk = item.FindControl("chkSelect") as CheckBox;
                    itemchk.Checked = Headercheck;
                }
            }

        }

        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMaterialIDs();
            LoadPriorities();
        }

        protected void lvAssemblyDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    string status = (e.Item.FindControl("lblStatus") as Label).Text;
                    CheckBox chkSelect = e.Item.FindControl("chkSelect") as CheckBox;
                    if (status != null && chkSelect != null)
                    {
                        if (status.Equals("Running") || status.Equals("Completed", StringComparison.OrdinalIgnoreCase) || status.Equals("Parked", StringComparison.OrdinalIgnoreCase)) chkSelect.Visible = false;
                        else chkSelect.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lvAssemblyDetails_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            try
            {
                CheckBox headercheck = lvAssemblyDetails.FindControl("chkHeaderSelect") as CheckBox;
                headercheck.Checked = false;
                DataPager dataPagerSchedule = (lvAssemblyDetails.FindControl("dataPagerSchedule") as DataPager);
                if (dataPagerSchedule != null)
                {
                    dataPagerSchedule.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void lvAssemblyDetails_Sorting(object sender, ListViewSortEventArgs e)
        {
            try
            {
                ImageButton imgBtnSortPriority = lvAssemblyDetails.FindControl("imgBtnSortPriority") as ImageButton;
                ImageButton imgBtnSortStatus = lvAssemblyDetails.FindControl("imgBtnSortStatus") as ImageButton;
                string DefaultSortIMG = "~/Image/asc.gif";
                string imgUrl = "~/Image/desc.gif";
                if (ViewState["SortExpression"] != null)
                {
                    if (ViewState["SortExpression"].ToString() == e.SortExpression)
                    {
                        ViewState["SortExpression"] = null;
                        imgUrl = DefaultSortIMG;
                    }
                    else
                    {
                        ViewState["SortExpression"] = e.SortExpression;
                    }
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression;
                }

                switch (e.SortExpression)
                {
                    case "Priority":
                        if (imgBtnSortStatus != null)
                            imgBtnSortStatus.ImageUrl = DefaultSortIMG;
                        if (imgBtnSortPriority != null)
                            imgBtnSortPriority.ImageUrl = imgUrl;
                        break;
                    case "Status":
                        if (imgBtnSortPriority != null)
                            imgBtnSortPriority.ImageUrl = DefaultSortIMG;
                        if (imgBtnSortStatus != null)
                            imgBtnSortStatus.ImageUrl = imgUrl;
                        break;
                    default:
                        if (imgBtnSortPriority != null)
                            imgBtnSortPriority.ImageUrl = DefaultSortIMG;
                        if (imgBtnSortStatus != null)
                            imgBtnSortStatus.ImageUrl = DefaultSortIMG;
                        break;

                }
                BindProductionScheduleData(e.SortExpression + " " + ((ViewState["SortExpression"] != null) ? "ASC" : "DESC"));

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnDeleteAssemblySch_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsDeleted = false;
                foreach (ListViewItem listViewItem in lvAssemblyDetails.Items)
                {
                    if (listViewItem.FindControl("chkSelect") is CheckBox)
                    {
                        CheckBox chkSelect = listViewItem.FindControl("chkSelect") as CheckBox;
                        if (chkSelect.Checked)
                        {
                            string Idd = (listViewItem.FindControl("hdfUpdate") as HiddenField).Value;
                            IsDeleted = GEADatabaseAccess.DeleteProdSchedule(Idd);
                            if (!IsDeleted) break;
                        }
                    }
                }
                if (IsDeleted)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data deleted successfully.')", true);
                }
                BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
            }
            catch (Exception ex)
            {

            }

        }

        protected void btnSaveNewAssembly_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                string machine = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string materialId = ddlMaterialID_A.SelectedItem != null ? ddlMaterialID_A.SelectedValue : "";
                string fabricationNum = txtFabricationNumber_A.Text;
                string opnNum = txtOperationNo_A.Text;
                string prodOrder = txtProdOrder_A.Text;
                string[] DefalutSubActivity = null;
                string[] OptionalSubActivity = null;
                string[] SubActivities = null;
                hfDefaultActivity.Value = hfDefaultActivity.Value.TrimEnd(',');
                if (!string.IsNullOrEmpty(hfDefaultActivity.Value))
                {
                    DefalutSubActivity = hfDefaultActivity.Value.Split(',');
                }
                //if (DefalutSubActivity == null) return;



                hfOptionalActivity.Value = hfOptionalActivity.Value.TrimEnd(',');
                if (!string.IsNullOrEmpty(hfOptionalActivity.Value))
                {
                    OptionalSubActivity = hfOptionalActivity.Value.Split(',');
                }
                //if (DefalutSubActivity == null) return;
                string process = getMachineIdProcess(machine);

                if (!GEADatabaseAccess.IsProdMatIdUnique(prodOrder, materialId, "", process))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Another material ID exists for the production order : " + prodOrder + "')", true);
                    return;
                }
                //foreach (string opnNum in OpnNum)
                //{
                if (GEADatabaseAccess.IsProdMatIdOpnExists(prodOrder, materialId, opnNum, "", process))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Cannot save as selected material ID and Operation already exists for the production order.')", true);
                    return;
                }
                if (!GEADatabaseAccess.IsAssemblyScheduleExists(machine, prodOrder, materialId, opnNum, fabricationNum))
                {
                    if (radioMoveEnd_A.Checked)
                    {
                        IsSaved = GEADatabaseAccess.UpdateScheduleDetails_Assembly((GEADatabaseAccess.GetMaxSchedulePriority(out int Userpriority) + 1).ToString(), machine, prodOrder, materialId, opnNum, txtQuantity.Text, txtLocalExport_A.Text, txtSaleOrder_A.Text, txtScrollWelded_A.Text, txtRDDMachine_A.Text, fabricationNum, txtCustomer_A.Text, txtLocation_A.Text);
                    }
                    if (radioMove2Before_A.Checked)
                    {
                        if (ddlAddPriority_A.SelectedItem != null)
                        {
                            int priority = Convert.ToInt32(ddlAddPriority_A.SelectedValue);
                            IsSaved = GEADatabaseAccess.InsertScheduleDetailsBefore_Assembly(priority, machine, prodOrder, materialId, opnNum, txtQuantity.Text, txtLocalExport_A.Text, txtSaleOrder_A.Text, txtScrollWelded_A.Text, txtRDDMachine_A.Text, fabricationNum, txtCustomer_A.Text, txtLocation_A.Text);
                        }
                    }
                    if (IsSaved)
                    {
                        if (DefalutSubActivity != null)
                        {
                            foreach (string data in DefalutSubActivity)
                            {
                                bool IsSubActSaved = GEADatabaseAccess.InsertSubActivities(machine, prodOrder, materialId, opnNum, fabricationNum, data);
                            }
                        }
                        if (OptionalSubActivity != null)
                        {
                            foreach (string data in OptionalSubActivity)
                            {
                                bool IsSubActSaved = GEADatabaseAccess.InsertSubActivities(machine, prodOrder, materialId, opnNum, fabricationNum, data);
                            }
                        }


                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data updated successfully.')", true);
                        BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Entered prod. order, machine, material id, operation no. and fabrication no. already exists.')", true);
                }
                // }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlMaterialID_A_SelectedIndexChanged(object sender, EventArgs e)
        {
            LooadActivity();
        }

        protected void ddlUpdtMatId_A_SelectedIndexChanged(object sender, EventArgs e)
        {
            LooadActivity();
        }

        protected void btnUpdateAssembly_Click(object sender, EventArgs e)
        {
            bool IsSaved = false;
            try
            {
                string macId = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string Idd = hdfIdd_A.Value;
                string old_prodno = hdfProd_A.Value;
                string old_matid = hdfMat_A.Value;
                string new_prodno = txtUpdtProdOrder_A.Text;
                string new_matid = ddlUpdtMatId_A.SelectedItem != null ? ddlUpdtMatId_A.SelectedValue : "";
                string old_opnno = txtUpdtOperationNum_A.Text;
                // string new_opnno = ddlUpdtSubOperationNum_A.SelectedItem != null ? ddlUpdtSubOperationNum_A.SelectedValue : "";
                string qty = txtUpdtQuantity_A.Text;
                string fabricationno = hfUpdtFabricationNo_A.Value;
                string newfabricationno = txtUpdaterFabricationNum_A.Text;
                string localexport = txtUpdateLocal_A.Text;
                string SaleOrder = txtUpdateSaleOrder_A.Text;
                string ScrollNumber = txtUpdateScrollNum_A.Text;
                string RDDMachines = txtUpdateRDDMachines_A.Text;
                string Customer = txtUpdateCustomer_A.Text;
                string Location = txtUpdateLocation_A.Text;
                string process = getMachineIdProcess(macId);
                if (!string.IsNullOrEmpty(Idd) && !string.IsNullOrEmpty(macId) && !string.IsNullOrEmpty(new_matid))
                {
                    //&& !string.IsNullOrEmpty(new_opnno)
                    if (!new_prodno.Equals(old_prodno) || !new_matid.Equals(old_matid))
                    {
                        if (!GEADatabaseAccess.IsProdMatIdUnique(new_prodno, old_matid, "", process))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Another material ID exists for the production order : " + new_prodno + "')", true);
                            return;
                        }
                        if (GEADatabaseAccess.IsProdMatIdOpnExists(new_prodno, new_matid, old_opnno, "", process))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Cannot save as selected material ID already exists for the production order and Operation.')", true);
                            return;
                        }
                    }
                    IsSaved = GEADatabaseAccess.UpdateScheduleData(Idd, macId, new_prodno, new_matid, old_opnno, qty, newfabricationno, localexport, SaleOrder, ScrollNumber, RDDMachines, Customer, Location);
                    if (IsSaved)
                    {
                        GEADatabaseAccess.UpdateActivityData(macId, old_prodno, new_prodno, old_matid, new_matid, old_opnno, fabricationno);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "alert('Data updated successfully.')", true);
                        BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                    }
                }
                else
                {
                    lblMessages.ForeColor = Color.Red;
                    lblMessages.Text = "Material ID and Operation Number cannot be empty.";
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnUpdateActivity_Click(object sender, EventArgs e)
        {
            try
            {
                string machine = hfUpdateActivityMachineId.Value;
                string prodOrder = hfUpdateActivityPO.Value;
                string materialId = hfUpdateActivityMaterialID.Value;
                string opnNum = hfUpdateActivityOPNum.Value;
                string fabricationNum = hfUpdateActivityFabNo.Value;
                string[] OptionalActivity_Checked = null;
                string[] OptionalActivity_Unchecked = null;
                string[] defaultActivity_Checked = null;
                string[] defaultActivity_Unchecked = null;
                hfUpdateOptionalActivityChecked.Value = hfUpdateOptionalActivityChecked.Value.TrimEnd(',');
                if (!string.IsNullOrEmpty(hfUpdateOptionalActivityChecked.Value))
                {
                    OptionalActivity_Checked = hfUpdateOptionalActivityChecked.Value.Split(',');
                }
                hfUpdateOptionalActivityUnchecked.Value = hfUpdateOptionalActivityUnchecked.Value.TrimEnd(',');
                if (!string.IsNullOrEmpty(hfUpdateOptionalActivityUnchecked.Value))
                {
                    OptionalActivity_Unchecked = hfUpdateOptionalActivityUnchecked.Value.Split(',');
                }
                if (OptionalActivity_Checked != null)
                {
                    foreach (string data in OptionalActivity_Checked)
                    {
                        bool IsSubActSaved = GEADatabaseAccess.InsertRemoveSubActivities(machine, prodOrder, materialId, opnNum, fabricationNum, data, "Checked");
                    }
                }

                if (OptionalActivity_Unchecked != null)
                {
                    foreach (string data in OptionalActivity_Unchecked)
                    {
                        bool IsSubActSaved = GEADatabaseAccess.InsertRemoveSubActivities(machine, prodOrder, materialId, opnNum, fabricationNum, data, "UnChecked");
                    }
                }

                hfUpdateDefaultActivityChekced.Value = hfUpdateDefaultActivityChekced.Value.TrimEnd(',');
                if (!string.IsNullOrEmpty(hfUpdateDefaultActivityChekced.Value))
                {
                    defaultActivity_Checked = hfUpdateDefaultActivityChekced.Value.Split(',');
                }
                hfUpdateDefaultActivityUnChekced.Value = hfUpdateDefaultActivityUnChekced.Value.TrimEnd(',');
                if (!string.IsNullOrEmpty(hfUpdateDefaultActivityUnChekced.Value))
                {
                    defaultActivity_Unchecked = hfUpdateDefaultActivityUnChekced.Value.Split(',');
                }
                if (defaultActivity_Checked != null)
                {
                    foreach (string data in defaultActivity_Checked)
                    {
                        bool IsSubActSaved = GEADatabaseAccess.InsertRemoveSubActivities(machine, prodOrder, materialId, opnNum, fabricationNum, data, "Checked");
                    }
                }

                if (defaultActivity_Unchecked != null)
                {
                    foreach (string data in defaultActivity_Unchecked)
                    {
                        bool IsSubActSaved = GEADatabaseAccess.InsertRemoveSubActivities(machine, prodOrder, materialId, opnNum, fabricationNum, data, "UnChecked");
                    }
                }

                BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
            }
            catch (Exception ex)
            {

            }
        }

        protected void chkHeaderSelect_A_CheckedChanged(object sender, EventArgs e)
        {

        }
        private string getMachineIdProcess(string machineID)
        {
            string process = "";
            try
            {
                List<MachinIdProcessEnity> list = new List<MachinIdProcessEnity>();
                if (Session["MachineIDProcessList"] != null)
                {
                    list = Session["MachineIDProcessList"] as List<MachinIdProcessEnity>;
                }
                else
                {
                    list = GEADatabaseAccess.getMachineIDProcessList();
                }
                if (list.Count > 0)
                {
                    process = list.Where(k => k.MachineID == machineID).Select(k => k.Process).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return process;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getMachineIDProcess(string machineID)
        {
            ProductionScheduler ps = new ProductionScheduler();
            string process = ps.getMachineIdProcess(machineID);
            return process;
        }

        protected void btnSwitchSchedule_Click(object sender, EventArgs e)
        {
            int selectedRowsToMove = 0, UpdatedRows = 0; string result = "";
            try
            {
                string process = getMachineIdProcess(ddlMachineId.SelectedValue);
                string MachineID = btnSwitchSchedule.Text.Replace("Switch to", "").Trim();
                if (process.ToLower().Contains("balancing"))
                {
                    foreach (ListViewDataItem item in lstProdScheduler.Items)
                    {
                        if ((item.FindControl("chkSelect") as CheckBox).Checked)
                        {
                            selectedRowsToMove++;

                            string IDD = (item.FindControl("hdfUpdate") as HiddenField).Value;
                            string ComponentID = (item.FindControl("lblMaterialID") as Label).Text;
                            string OperationNumber = (item.FindControl("lblOpnNumber") as Label).Text;

                            bool IsShufflingAllowed = GEADatabaseAccess.ValidateNewSchedule(MachineID, ComponentID, OperationNumber);
                            if (IsShufflingAllowed)
                                UpdatedRows += GEADatabaseAccess.ShuffleScheduleMachine(IDD, MachineID);
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "NotAllowed", "alert('CO is not defined for " + MachineID + ".');", true);
                                return;
                            }
                        }
                    }
                }
                else if (process.Contains("Assembly"))
                {
                    foreach (ListViewDataItem item in lvAssemblyDetails.Items)
                    {
                        if ((item.FindControl("chkSelect") as CheckBox).Checked)
                        {
                            selectedRowsToMove++;

                            string IDD = (item.FindControl("hdfUpdate") as HiddenField).Value;
                            string ComponentID = (item.FindControl("lblMaterialID") as Label).Text;
                            string OperationNumber = (item.FindControl("lblOpnNumber") as Label).Text;
                            string prodOrder = (item.FindControl("lblProdOrderNumber") as Label).Text;
                            string materialId = (item.FindControl("lblMaterialID") as Label).Text;
                            string opnNum = (item.FindControl("lblOpnNumber") as Label).Text;
                            string fabricationNum = (item.FindControl("lblFabricationNum") as Label).Text;

                            bool IsShufflingAllowed = GEADatabaseAccess.ValidateNewSchedule(MachineID, ComponentID, OperationNumber);

                            if (IsShufflingAllowed)
                            {
                                result = GEADatabaseAccess.InsertDataToAssemblySchedules(MachineID, prodOrder, fabricationNum, ComponentID, OperationNumber);
                                if (result == "Inserted")
                                    UpdatedRows += GEADatabaseAccess.ShuffleScheduleMachine(IDD, MachineID);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "NotAllowed", "alert('CO is not defined for " + MachineID + ".');", true);
                                return;
                            }
                        }
                    }
                }
                if (selectedRowsToMove == 0)
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMsg", "alert('Please select Production Order(New).');", true);

                else if (selectedRowsToMove == UpdatedRows)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMsg", "alert('Schedules Updated Successfully.');", true);
                    BindProductionScheduleData(Session["SortExpression"] != null ? Session["SortExpression"].ToString() : "");
                }

                else
                    HelperClassGeneric.openErrorModal(this, "Failed to Update Schedules. Please Try again.");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnSwitchSchedule_Click= " + ex.ToString());
            }
        }
    }
    public class PriorityType
    {
        public string UserPriority { get; set; }
        public string SystemPriority { get; set; }
    }
    public class SchDetails
    {
        public string Priority { get; set; }
        public string ProdOrder { get; set; }
        public string MatID { get; set; }
        public string OpnID { get; set; }
    }
}