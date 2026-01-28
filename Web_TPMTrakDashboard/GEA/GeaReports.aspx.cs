using BusinessClassLibrary;
using ModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;
using Web_TPMTrakDashboard.Models;
using DataBaseAccess = Web_TPMTrakDashboard.Models.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class GeaReports : System.Web.UI.Page
    {
        public List<UserAccessModel> useAccessData = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                Session["MachineIDProcessList"] = null;
                SessionClear.ClearSession();
                txtYear.Text = DateTime.Now.ToString("yyyy");
                BindPlantID();
                BindCellID();
                //BindMachineID();
                BindAllProdOrders();
                BindComponentIDs();
                BindOperationNos();
                BindInsPlanNumbers();
                BindShiftDetails();
                BindOperatorID();
                BindQualityMachines();
                BindMachineIDForNonMachiningReport();
                ddlReportType_SelectedIndexChanged(null, null);

                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"] != null ? Session["UserName"].ToString() : "PCT");
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;
                BindUserAccessData();
            }
        }

        private void BindUserAccessData()
        {
            try
            {
                if (!useAccessData.Where(ss => ss.Code.Equals("MachineMixReport", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("GEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                    ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("MachineMixReport"));
                if (!useAccessData.Where(ss => ss.Code.Equals("ParkedOrderReasonReport", StringComparison.OrdinalIgnoreCase) && ss.Domain.Equals("GEA", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault())
                    ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("ParkedOrderReasons"));
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindQualityMachines()
        {
            ddlQualityMachines.DataSource = GEADatabaseAccess.GetQualityMachines();
            ddlQualityMachines.DataBind();
        }

        private void BindCellID()
        {

            try
            {
                List<string> CellIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllGroupId(ddlPlantID.SelectedValue != null ? ddlPlantID.SelectedValue.ToString() : "");
                if (CellIds != null && CellIds.Count > 0)
                {
                    CellIds.Insert(0, "All");
                    ddlCellID.DataSource = CellIds;
                    ddlCellID.DataBind();
                    ddlCellID.SelectedIndex = 0;
                }
                BindMachineID();
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindPlantID()
        {

            try
            {
                List<string> PlantIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                if (PlantIds != null && PlantIds.Count > 0)
                {
                    ddlPlantID.DataSource = PlantIds;
                    ddlPlantID.DataBind();
                    ddlPlantID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void BindOperatorID()
        {
            try
            {
                //var allOperation = BindCockpitView.GetOperatorDataForPlant(ddlPlantID.SelectedValue != null ? ddlPlantID.SelectedValue.ToString() : "");
                //var allOperators = GEADatabaseAccess.GetOperatorIDs(ddlPlantID.SelectedValue.ToString());
                List<Operator> allOperatorNames = GEADatabaseAccess.GetOperatorName(ddlPlantID.SelectedValue.ToString());
                //var allOperatorNames = GEADatabaseAccess.GetOperatorName(ddlPlantID.SelectedValue.ToString());
                if (allOperatorNames != null && allOperatorNames.Count > 0)
                {
                    ddlMultiOperatorID.DataSource = allOperatorNames;
                    ddlMultiOperatorID.DataTextField = "OperatorName";
                    ddlMultiOperatorID.DataValueField = "OperatorID";
                    ddlMultiOperatorID.DataBind();

                    ddlOperatorID.DataSource = allOperatorNames;
                    ddlOperatorID.DataTextField = "OperatorName";
                    ddlOperatorID.DataValueField = "OperatorID";
                    ddlOperatorID.DataBind();
                    ddlOperatorID.SelectedIndex = 0;
                    foreach (ListItem item in ddlMultiOperatorID.Items)
                    {
                        item.Selected = true;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void BindMachineID()
        {
            try
            {
                //List<string> machineIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllMacForPlant("");
                List<string> machineIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllMachinedByLineandGroup(ddlPlantID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue, ddlCellID.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCellID.SelectedValue);
                if (machineIds != null && machineIds.Count > 0)
                {
                    ddlMultiMachineId.DataSource = machineIds;
                    ddlMultiMachineId.DataBind();

                    machineIds.Insert(0, "All");
                    if (ddlReportType.SelectedValue.Equals("ProductionScheduleReport", StringComparison.OrdinalIgnoreCase))
                    {
                        machineIds.RemoveAt(0);
                    }
                    ddlMachineId.DataSource = machineIds;
                    ddlMachineId.DataBind();
                    ddlMachineId.SelectedIndex = 0;
                    foreach (ListItem item in ddlMultiMachineId.Items)
                    {
                        item.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindAllProdOrders()
        {
            try
            {
                trfromdate.Visible = false;
                trtodate.Visible = false;
                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    trYear.Visible = false;
                    List<ProdFabricationNumber> ProdorderList = new List<ProdFabricationNumber>();
                    if (ddlType.SelectedValue.Equals("QualityIncoming8DReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("QualityIncomingActionDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "MaterialID", "Quality8DReport", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("HardnessReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("HardnessReportDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "PartNo", "", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("FirstSampleReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("FirstSampleReportDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "PartNo", "", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("DyePenetrationReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("DyePenetrationReportDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "PartNo", "", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("IQRReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("InternalQualityReportDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "PartNo", "", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("NCReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("NonConformanceReportDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "PartNo", "", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("DCReport", StringComparison.OrdinalIgnoreCase))
                    {

                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("DeviationReportDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "PartNo", "", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                    {
                        ProdorderList = GEADatabaseAccess.GetQualityInhouseProductionOrderList("QualityIncomingActionDetails_GEA", "ProductionOrderNo", ddlQualityMachines.SelectedValue.ToString(), "MaterialID", "", "GrnNo");
                    }
                    else if (ddlType.SelectedValue.Equals("DPHUReport", StringComparison.OrdinalIgnoreCase) || (ddlType.SelectedValue.Equals("QualityPCReport", StringComparison.OrdinalIgnoreCase) && ddlFormatType.SelectedValue.ToString().Equals("TimeConsolidated", StringComparison.OrdinalIgnoreCase)))
                    {
                        trfromdate.Visible = true;
                        trtodate.Visible = true;
                    }
                    else if ((ddlType.SelectedValue.Equals("QualityPCReport", StringComparison.OrdinalIgnoreCase) && ddlFormatType.SelectedValue.ToString().Equals("Year", StringComparison.OrdinalIgnoreCase)))
                    {
                        trYear.Visible = true;
                    }
                    Session["Fabricationnumber"] = ProdorderList;
                    ddlProdOrder.DataSource = null;
                    ddlProdOrder.DataBind();
                    ddlComponent.DataSource = null;
                    ddlComponent.DataBind();
                    //if (ProdorderList != null && ProdorderList.Count > 0)
                    //{
                    ddlProdOrder.DataSource = ProdorderList.Select(k => k.ProdOrderNumber).Distinct().ToList();
                    ddlProdOrder.DataBind();
                    if (ProdorderList.Count > 0)
                    {
                        ddlProdOrder.SelectedIndex = 0;
                    }
                    ddlComponent.DataSource = ProdorderList.Select(k => k.MaterialNumber).Distinct().ToList();
                    ddlComponent.DataBind();
                    //}
                    //if (ddlType.SelectedValue.Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                    //{
                    //     ddlProdOrder_SelectedIndexChanged(null, null);
                    //}
                    ddlComponent_SelectedIndexChanged(null, null);
                    BindGRNNumber();
                }
                else if (ddlReportType.SelectedValue.Equals("NonMachiningReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlFormatType.SelectedValue.Equals("MachineDataAssemblyReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetMachineDataAssemblyProductionOrderList(ddlNonMachineMachineID.SelectedValue);
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        //ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();
                        //}
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("ElectroTechEquipmentReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetElectroTechProductionOrderList(ddlNonMachineMachineID.SelectedValue);
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        // ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();

                        //}
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("ProDecanterReportOnly", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.getProDecanterProductionOrderList();
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        // ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();

                        //}
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("TestingReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForTestingPackingReport("DecanterChecklistTransaction_GEA");
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        //ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();

                        //}
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("VibrationTestProtocolReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForTestingPackingReport("VibrationTestProtocolTransaction_GEA");
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        // ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();

                        // }
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("NoiseMeasurementReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForTestingPackingReport("NoiseMeasurementTransaction_GEA");
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        // ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();

                        // }
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("DecanterChecklistPackingReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForTestingPackingReport("DecanterChecklistPackingTransaction_GEA");
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        //  ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();

                        //}
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("DecanterFinalChecklistPackingReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForTestingPackingReport("DecanterFinalTestingPackingTransaction_GEA");
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        //   ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();

                        // }
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                    else if (ddlFormatType.SelectedValue.Equals("PickingListReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetPickingListProductionOrderList(ddlNonMachineMachineID.SelectedValue, ddlSubFormat.SelectedValue);
                        Session["Fabricationnumber"] = ProdorderList;
                        //if (ProdorderList != null && ProdorderList.Count > 0)
                        //{
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        //ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                        ddlFabriation.DataBind();
                        //}
                        ddlProdOrder_SelectedIndexChanged(null, null);
                    }
                }
                else if (ddlReportType.SelectedValue.Equals("DecanterAcceptanceTestCardReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForBlueCard();
                    Session["Fabricationnumber"] = ProdorderList;
                    //if (ProdorderList != null && ProdorderList.Count > 0)
                    //{
                    ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                    ddlProdOrder.DataBind();
                    // ddlProdOrder.SelectedIndex = 0;
                    ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                    ddlFabriation.DataBind();

                    // }
                    ddlProdOrder_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.Equals("AssemblyTestingPackingReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForBlueCard();
                    Session["Fabricationnumber"] = ProdorderList;
                    //if (ProdorderList != null && ProdorderList.Count > 0)
                    //{
                    ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                    ddlProdOrder.DataBind();
                    // ddlProdOrder.SelectedIndex = 0;
                    ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                    ddlFabriation.DataBind();

                    // }
                    ddlProdOrder_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.Equals("BalancingCertificate", StringComparison.OrdinalIgnoreCase))
                {
                    List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderListForTestingPackingReport("BalancingReportTransaction");
                    Session["Fabricationnumber"] = ProdorderList;
                    if (ProdorderList != null && ProdorderList.Count > 0)
                    {
                        ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        ddlProdOrder.DataBind();
                        ddlProdOrder.SelectedIndex = 0;
                        ddlComponent.DataSource = ProdorderList.Select(k => k.MaterialNumber).ToList();
                        ddlComponent.DataBind();
                    }
                    ddlComponent_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProdOrderStatusReport", StringComparison.OrdinalIgnoreCase))
                {
                    trfromdate.Visible = true;
                    trtodate.Visible = true;
                    string proType = ddlProType.SelectedValue.ToString();

                    List<string> ProdorderList = GEADatabaseAccess.GetProductionOrderStatusFabricationNum(proType, txtfromoDate.Text, txtToDate.Text);
                    if (ProdorderList != null && ProdorderList.Count > 0)
                    {
                        //ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                        //ddlProdOrder.DataBind();
                        //ddlProdOrder.SelectedIndex = 0;
                        ddlFabriation.DataSource = ProdorderList;
                        ddlFabriation.DataBind();
                        ddlFabriation.SelectedIndex = 0;
                        Session["Fabricationnumber"] = ProdorderList;
                    }
                }
                else if (ddlReportType.SelectedValue.Equals("CEChecklistReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.getProductionOrderListForCEChecklist();
                    Session["Fabricationnumber"] = ProdorderList;
                    ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                    ddlProdOrder.DataBind();
                    ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                    ddlFabriation.DataBind();
                    ddlProdOrder_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.Equals("ProDecanterReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.getProductionOrderListForProDecanter();
                    Session["Fabricationnumber"] = ProdorderList;
                    ddlProdOrder.DataSource = ProdorderList.AsEnumerable().Select(x => x.ProdOrderNumber).ToList();
                    ddlProdOrder.DataBind();
                    ddlFabriation.DataSource = ProdorderList.AsEnumerable().Select(x => x.FabricationNumber).ToList();
                    ddlFabriation.DataBind();
                    ddlProdOrder_SelectedIndexChanged(null, null);
                }
                else if (ddlReportType.SelectedValue.Equals("ProductionScheduleReport", StringComparison.OrdinalIgnoreCase))
                {
                    trfromdate.Visible = true;
                    trtodate.Visible = true;
                    List<string> ProdorderList = GEADatabaseAccess.GetAllProdOrderbyMachineforProductionSchedule(ddlMachineId.SelectedValue.ToString());
                    //ProdorderList.Insert(0, new ProdFabricationNumber() { ProdOrderNumber = "All" });
                    Session["Fabricationnumber"] = ProdorderList;
                    ddlProdOrder.DataSource = ProdorderList;
                    ddlProdOrder.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindComponentIDs()
        {
            try
            {

                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.Equals("BalancingCertificate", StringComparison.OrdinalIgnoreCase))
                {
                    //List<string> listComponentIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllComponents();
                    //if (listComponentIds != null && listComponentIds.Count > 0)
                    //{
                    //    if (listComponentIds.Contains("All")) listComponentIds.Remove("All");
                    //    ddlComponent.DataSource = listComponentIds;
                    //    ddlComponent.DataBind();
                    //    ddlComponent.SelectedIndex = 0;
                    //}

                    //List<string> listComponentIds = new List<string>();
                    //if (ddlType.SelectedValue.ToString().Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    listComponentIds = GEADatabaseAccess.getComponentsForQuality(ddlQualityMachines.SelectedValue, ddlProdOrder.SelectedValue);
                    //    ddlComponent.DataSource = listComponentIds;
                    //    ddlComponent.DataBind();
                    //    ddlComponent_SelectedIndexChanged(null, null);
                    //}

                    List<ProdFabricationNumber> ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                    var serachedList = ProdorderList.Where(x => x.ProdOrderNumber.Equals(ddlProdOrder.SelectedValue)).ToList();
                    if (serachedList != null)
                    {
                        for (int i = 0; i < serachedList.Count; i++)
                        {
                            if (ddlComponent.Items.FindByValue(serachedList[i].MaterialNumber) != null)
                            {
                                ddlComponent.SelectedValue = serachedList[i].MaterialNumber;
                                break;
                            }
                        }
                    }
                    BindInsPlanNumbers();
                    BindGRNNumber();
                    // ddlComponent.SelectedValue = ProdorderList.Where(x => x.ProdOrderNumber.Equals(ddlProdOrder.SelectedValue)).SingleOrDefault().MaterialNumber;

                }
                else if (ddlReportType.SelectedValue.Equals("ProductionScheduleReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> listComponentIds = GEADatabaseAccess.GetAllComponentbyMachineforProductionSchedule(ddlMachineId.SelectedValue.ToString());
                    //listComponentIds.Insert(0, new ProdFabricationNumber() { MaterialNumber = "All" });
                    ddlComponent.DataSource = listComponentIds;
                    ddlComponent.DataBind();
                    Session["Fabricationnumber"] = listComponentIds;
                }
                else
                if (ddlReportType.SelectedValue.Equals("AssemblyTestingPackingReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> listComponentIds = GEADatabaseAccess.GetAllComponentsByMachine("Testing");
                    ddlComponent.DataSource = listComponentIds;
                    ddlComponent.DataBind();
                }
                else if (ddlReportType.SelectedValue.Equals("NonMachiningReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlFormatType.SelectedValue.Equals("TestingReport", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.Equals("VibrationTestProtocolReport", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.Equals("NoiseMeasurementReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<string> listComponentIds = GEADatabaseAccess.GetAllComponentsByMachine("Testing");
                        ddlComponent.DataSource = listComponentIds;
                        ddlComponent.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindOperationNos()
        {
            try
            {
                List<string> listOpeartions = new List<string>();
                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlType.SelectedValue.ToString().Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                    {
                        listOpeartions = GEADatabaseAccess.getOperationForQuality(ddlQualityMachines.SelectedValue, ddlProdOrder.SelectedValue, ddlComponent.SelectedValue);
                        ddlOperation.DataSource = listOpeartions;
                        ddlOperation.DataBind();
                        ddlOperation_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    string compId = ddlComponent.SelectedItem != null ? ddlComponent.SelectedValue : "";
                    listOpeartions = Web_TPMTrakDashboard.Models.DataBaseAccess.GetOperation("", compId);
                    if (listOpeartions != null && listOpeartions.Count > 0)
                    {
                        ddlOperation.DataSource = listOpeartions;
                        ddlOperation.DataBind();
                        ddlOperation.SelectedIndex = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        private void BindInsPlanNumbers()
        {
            try
            {
                List<string> listInsPlanNumbers = new List<string>();
                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlType.SelectedValue.ToString().Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                    {
                        listInsPlanNumbers = GEADatabaseAccess.getPlanAndRevNoForQuality(ddlQualityMachines.SelectedValue, ddlProdOrder.SelectedValue, ddlComponent.SelectedValue, ddlOperation.SelectedValue);
                        ddlInsPlanNumber.DataSource = listInsPlanNumbers;
                        ddlInsPlanNumber.DataBind();
                    }
                }
                else
                {
                    listInsPlanNumbers = GEADatabaseAccess.GetInsPlanNumbersQuality(ddlComponent.SelectedValue != null ? ddlComponent.SelectedValue.ToString() : "");
                    if (listInsPlanNumbers != null && listInsPlanNumbers.Count > 0)
                    {
                        ddlInsPlanNumber.DataSource = listInsPlanNumbers;
                        ddlInsPlanNumber.DataBind();
                        ddlInsPlanNumber.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string ReportStatus = "Generated";
            try
            {
                string report_type = ddlReportType.SelectedItem != null ? ddlReportType.SelectedValue : "";
                string machine_id = ddlMachineId.SelectedItem != null ? ddlMachineId.SelectedValue : "";
                string prod_order = ddlProdOrder.SelectedItem != null ? ddlProdOrder.SelectedValue : "";
                string comp_id = ddlComponent.SelectedItem != null ? ddlComponent.SelectedValue : "";
                string opn_num = ddlOperation.SelectedItem != null ? ddlOperation.SelectedValue : "";
                string ins_plan = ddlInsPlanNumber.SelectedItem != null ? ddlInsPlanNumber.SelectedValue : "";
                #region Commented
                //if (report_type.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                //{
                //    if (string.IsNullOrEmpty(report_type))
                //    {
                //        lblMessages.ForeColor = Color.Red;
                //        lblMessages.Text = "Please select a report type.";
                //        return;
                //    }
                //    else if (string.IsNullOrEmpty(machine_id))
                //    {
                //        lblMessages.ForeColor = Color.Red;
                //        lblMessages.Text = "Machine id is not available.";
                //        return;
                //    }
                //    else if (string.IsNullOrEmpty(prod_order))
                //    {
                //        lblMessages.ForeColor = Color.Red;
                //        lblMessages.Text = "Production order is not available.";
                //        return;
                //    }
                //    else if (string.IsNullOrEmpty(comp_id))
                //    {
                //        lblMessages.ForeColor = Color.Red;
                //        lblMessages.Text = "Component id is not available.";
                //        return;
                //    }
                //    else if (string.IsNullOrEmpty(opn_num))
                //    {
                //        lblMessages.ForeColor = Color.Red;
                //        lblMessages.Text = "Operation num is not available.";
                //        return;
                //    }
                //    ReportStatus = GEAGenerateReport.GenerateQualityIncomingReport(machine_id, prod_order, comp_id, opn_num, ins_plan);
                //}
                //else 
                #endregion
                if (ddlReportType.SelectedValue.ToString().Equals("GEAProductionReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(txtfromoDate.Text))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "From Date Cannot be Empty.";
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtToDate.Text))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "To Date Cannot be empty.";
                        return;
                    }
                    DateTime fromDate = Util.GetDateTime(txtfromoDate.Text);
                    DateTime toDate = Util.GetDateTime(txtToDate.Text);
                    string machineId = "";
                    foreach (ListItem item in ddlMultiMachineId.Items)
                    {
                        if (item.Selected)
                        {
                            if (machineId == "")
                                machineId += item.Value;
                            else
                                machineId += "," + item.Value;
                        }
                    }
                    ReportStatus = TMPTrakGenerateReport.ProductionReportGEA(ddlPlantID.SelectedValue.ToString(), ddlCellID.SelectedValue.ToString(), machineId, fromDate, toDate);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    bool skipValidation = false;
                    if ((ddlType.SelectedValue.ToString().Equals("DPHUReport", StringComparison.OrdinalIgnoreCase)) || ddlType.SelectedValue.ToString().Equals("QualityPCReport", StringComparison.OrdinalIgnoreCase))
                    {
                        skipValidation = true;
                    }
                    if ((ddlType.SelectedValue.ToString().Equals("DPHUReport", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (string.IsNullOrEmpty(txtfromoDate.Text))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "From Date cannot be Empty.";
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtToDate.Text))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "To Date cannot be empty.";
                            return;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ddlQualityMachines.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Machine ID cannot be Empty.";
                            return;
                        }
                        if (skipValidation == false)
                        {
                            if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                            {
                                lblMessages.ForeColor = Color.Red;
                                lblMessages.Text = "Production Order cannot be empty.";
                                return;
                            }
                        }
                    }
                    if (ddlComponent.Visible)
                    {
                        if (string.IsNullOrEmpty(ddlComponent.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Component ID cannot be empty.";
                            return;
                        }
                    }
                    if (ddlOperation.Visible)
                    {
                        if (string.IsNullOrEmpty(ddlOperation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Operation Number cannot be empty.";
                            return;
                        }
                    }
                    if (ddlInsPlanNumber.Visible)
                    {
                        if (string.IsNullOrEmpty(ddlInsPlanNumber.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Plan Number cannot be empty.";
                            return;
                        }
                    }
                    string grnNumber = "";
                    //if (trGRNNumber.Visible)
                    //{
                    string process = getMachineIdProcess(ddlQualityMachines.SelectedValue);
                    if (string.IsNullOrEmpty(ddlGRNNumber.SelectedValue) && process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase) && skipValidation == false)
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "GRN Number cannot be empty.";
                        return;
                    }
                    grnNumber = ddlGRNNumber.SelectedValue;
                    //}
                    //string partNumber = GEADatabaseAccess.GetPartnumber(ddlProdOrder.SelectedValue.ToString(), ddlQualityMachines.SelectedValue.ToString());
                    string partNumber = ddlComponent.SelectedValue;
                    if (ddlType.SelectedValue.ToString().Equals("HardnessReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.hardnessReport(ddlQualityMachines.SelectedValue.ToString(), ddlProdOrder.SelectedValue.ToString(), partNumber, grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("FirstSampleReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.FirstSampleReport(ddlQualityMachines.SelectedValue.ToString(), ddlProdOrder.SelectedValue.ToString(), partNumber, grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("DyePenetrationReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.DyePenetrationReport(ddlQualityMachines.SelectedValue.ToString(), ddlProdOrder.SelectedValue.ToString(), partNumber, grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("QualityIncoming8DReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.quality8dReport(ddlQualityMachines.SelectedValue.ToString(), ddlProdOrder.SelectedValue.ToString(), grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("IQRReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.IQReport(ddlQualityMachines.SelectedValue.ToString(), ddlProdOrder.SelectedValue.ToString(), partNumber, grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("NCReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.NCReport(ddlQualityMachines.SelectedValue.ToString(), ddlProdOrder.SelectedValue.ToString(), partNumber, grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("DCReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.DCReport(ddlQualityMachines.SelectedValue.ToString(), ddlProdOrder.SelectedValue.ToString(), partNumber, grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("DPHUReport", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.DPHUReport(Util.GetDateTime(txtfromoDate.Text), Util.GetDateTime(txtToDate.Text), ddlQualityMachines.SelectedValue.ToString());
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportStatus = GEAGenerateReport.QualityTestProtocol(ddlQualityMachines.SelectedValue, ddlProdOrder.SelectedValue, ddlComponent.SelectedValue, ddlOperation.SelectedValue, ddlInsPlanNumber.SelectedValue, grnNumber);
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("QualityPCReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ddlFormatType.SelectedValue.ToString().Equals("TimeConsolidated", StringComparison.OrdinalIgnoreCase))
                        {
                            ReportStatus = GEAGenerateReport.QualityPCTimeConsolidatedReport(ddlQualityMachines.SelectedValue, txtfromoDate.Text, txtToDate.Text);
                        }
                        else if (ddlFormatType.SelectedValue.ToString().Equals("Year", StringComparison.OrdinalIgnoreCase))
                        {
                            ReportStatus = GEAGenerateReport.QualityPCYearlyReport(ddlQualityMachines.SelectedValue, txtYear.Text);
                        }
                    }
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("NonMachiningReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddlFormatType.SelectedValue.ToString().Equals("MachineDataAssemblyReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.machineDataAssmeblyReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), ddlNonMachineMachineID.SelectedValue);
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("ElectroTechEquipmentReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.electroTechEquipmentReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), ddlNonMachineMachineID.SelectedValue);
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("ProDecanterReportOnly", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.ProDecanterReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), true);
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("TestingReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlComponent.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Component id cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.testingReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), ddlComponent.SelectedValue);
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("NoiseMeasurementReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlComponent.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Component id cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.NoiseMeasurementReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), ddlComponent.SelectedValue);
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("VibrationTestProtocolReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlComponent.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Component id cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.vibrationTestProtocolReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), ddlComponent.SelectedValue);
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("DecanterChecklistPackingReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.decanterChecklistPackingReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString());
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("DecanterFinalChecklistPackingReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.decanterFinalChecklistPackingReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString());
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("PickingListReport", StringComparison.OrdinalIgnoreCase) && ddlSubFormat.SelectedValue.ToString().Equals("Completed", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.GeneratePickingListCompleteReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString());
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("PickingListReport", StringComparison.OrdinalIgnoreCase) && ddlSubFormat.SelectedValue.ToString().Equals("Missing", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Production Order cannot be empty.";
                            return;
                        }
                        if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                        {
                            lblMessages.ForeColor = Color.Red;
                            lblMessages.Text = "Fabrication Number cannot be empty.";
                            return;
                        }
                        ReportStatus = GEAGenerateReport.GeneratePickingListMissingReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString());
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("", StringComparison.OrdinalIgnoreCase))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Format Type cannot be empty.";
                        return;
                    }
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("DecanterAcceptanceTestCardReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Production Order cannot be empty.";
                        return;
                    }
                    if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Fabrication Number cannot be empty.";
                        return;
                    }
                    ReportStatus = GEAGenerateReport.decanterAcceptanceTestCardReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString());
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("AssemblyTestingPackingReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Production Order cannot be empty.";
                        return;
                    }
                    if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Fabrication Number cannot be empty.";
                        return;
                    }
                    if (string.IsNullOrEmpty(ddlComponent.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Component id cannot be empty.";
                        return;
                    }
                    ReportStatus = GEAGenerateReport.assemblyTestingPackingReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), ddlComponent.SelectedValue);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("BalancingCertificate", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Production Order cannot be empty.";
                        return;
                    }

                    ReportStatus = GEAGenerateReport.balancingCertificateReport(ddlProdOrder.SelectedValue.ToString());
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("CEChecklistReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Production Order cannot be empty.";
                        return;
                    }
                    if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Fabrication Number cannot be empty.";
                        return;
                    }
                    ReportStatus = GEAGenerateReport.CEChecklistReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString());
                    //ReportStatus = GEAGenerateReport.ProDecanterReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString());
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProDecanterReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(ddlProdOrder.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Production Order cannot be empty.";
                        return;
                    }
                    if (string.IsNullOrEmpty(ddlFabriation.SelectedValue))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "Fabrication Number cannot be empty.";
                        return;
                    }
                    ReportStatus = GEAGenerateReport.ProDecanterReport(ddlProdOrder.SelectedValue.ToString(), ddlFabriation.SelectedValue.ToString(), false);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProdOrderStatusReport", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime FromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtfromoDate.Text));
                    DateTime ToDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    string fabNumber = GEADatabaseAccess.getListBoxValueWithSingleQuote(ddlMultiFabNum);
                    string proType = ddlProType.SelectedValue.ToString();
                    ReportStatus = GEAGenerateReport.GenerateProductionStatusReport(FromDate, ToDate, fabNumber, proType);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MachineMixReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(txtfromoDate.Text))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "From Date Cannot be Empty.";
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtToDate.Text))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "To Date Cannot be empty.";
                        return;
                    }
                    DateTime fromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtfromoDate.Text));
                    DateTime toDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    ReportStatus = GEAGenerateReport.GenerateMachineMixReport(fromDate, toDate);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ParkedOrderReasons", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(txtfromoDate.Text))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "From Date Cannot be Empty.";
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtToDate.Text))
                    {
                        lblMessages.ForeColor = Color.Red;
                        lblMessages.Text = "To Date Cannot be empty.";
                        return;
                    }
                    DateTime fromDate = Util.GetDateTime(txtfromoDate.Text);
                    DateTime toDate = Util.GetDateTime(txtToDate.Text);
                    string machineId = "";
                    foreach (ListItem item in ddlMultiMachineId.Items)
                    {
                        machineId += item.Selected ? "'" + item.Value.Trim() + "'," : "";
                    }
                    machineId = machineId.Trim(',');
                    ReportStatus = GEAGenerateReport.ParkedOrderReasonsReport(ddlCellID.SelectedValue.ToString(), machineId, fromDate, toDate);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ProductionScheduleReport", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime FromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtfromoDate.Text));
                    DateTime ToDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    string machineId = ddlMachineId.SelectedValue.ToString();
                    //string status = ddlMultistatus.SelectedValue.ToString();
                    string status = "";
                    foreach (ListItem item in ddlMultistatus.Items)
                    {
                        status += item.Selected ? "'" + item.Value.Trim() + "'," : "";
                    }
                    status = status.Trim(',');
                    string prodOrder = txtProdOrderSearch.Text;
                    string component = txtCompSearch.Text;
                    ReportStatus = GEAGenerateReport.GenerateProductionScheduleReports(txtfromoDate.Text, txtToDate.Text, machineId, status, prodOrder, component);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MonthlyOperatorEfficiencyReport", StringComparison.OrdinalIgnoreCase))
                {
                    string shift = ddlShift.SelectedValue.ToString();
                    string plant = ddlPlantID.SelectedValue.ToString();
                    //string OperatorID= ddlOperatorID.SelectedValue.ToString();
                    string OperatorID = "";
                    foreach (ListItem item in ddlMultiOperatorID.Items)
                    {
                        OperatorID += item.Selected ? "'" + item.Value.Trim() + "'," : "";
                    }
                    OperatorID = OperatorID.Trim(',');
                    ReportStatus = GEAGenerateReport.MonthlyOperatorEfficiencyReport(txtfromoDate.Text, txtToDate.Text, ddlShift.SelectedValue.ToString(), ddlPlantID.SelectedValue.ToString(), OperatorID);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("ModelStdTimevsActual", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime FromDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtfromoDate.Text));
                    DateTime ToDate = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                    string fabNo = txtFabricationSearch.Text;
                    string prodNo = txtProdOrderSearch.Text;
                    string processType = ddlProcessType.SelectedValue.ToString();
                    ReportStatus = GEAGenerateReport.ModelstdTimevsActualReport(txtfromoDate.Text, txtToDate.Text, prodNo, fabNo, processType);
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("MachineWiseAssemblyReport", StringComparison.OrdinalIgnoreCase))
                {
                    string fabNo = ddlFabriation.SelectedValue.ToString();
                    ReportStatus = GEAGenerateReport.MachineWiseAssemblyReport(fabNo);
                }
                if (ReportStatus.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
                else if (ReportStatus.Equals("NoDataFound", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
                else if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageSuccess", "messageSuccess();", true);
                else if (ReportStatus.Equals("TemplateNotFound", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageTemplateNotFound", "messageTemplateNotFound();", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.Equals("BalancingCertificate", StringComparison.OrdinalIgnoreCase))
                {

                    List<ProdFabricationNumber> ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                    var serachedList = ProdorderList.Where(x => x.MaterialNumber.Equals(ddlComponent.SelectedValue)).ToList();
                    if (serachedList != null)
                    {
                        ddlProdOrder.DataSource = serachedList.Select(k => k.ProdOrderNumber).Distinct().ToList();
                        ddlProdOrder.DataBind();
                        //for (int i = 0; i < serachedList.Count; i++)
                        //{
                        //    if (ddlProdOrder.Items.FindByValue(serachedList[i].ProdOrderNumber) != null)
                        //    {
                        //        ddlProdOrder.SelectedValue = serachedList[i].ProdOrderNumber;
                        //        break;
                        //    }
                        //}
                    }
                    // ddlProdOrder.SelectedValue = ProdorderList.Where(x => x.MaterialNumber.Equals(ddlComponent.SelectedValue)).FirstOrDefault().ProdOrderNumber;

                }
                //BindAllProdOrders();
                BindOperationNos();
                BindGRNNumber();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlComponent_SelectedIndexChanged =" + ex.Message);
            }
        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideAlltr();
            ddlType.Items.Clear();
            //if (ddlReportType.SelectedValue.ToString().Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
            //{
            //    trInsPlan.Visible = true;
            //    trMachine.Visible = false;
            //    trMachine.Visible = false;
            //    trGEAQualityMachines.Visible = true;
            //    trOperation.Visible = true;
            //    trPartID.Visible = true;
            //    trProdOrder.Visible = true;
            //}
            //else
            if (ddlReportType.SelectedValue.ToString().Equals("GEAProductionReport", StringComparison.OrdinalIgnoreCase))
            {
                trPlant.Visible = true;
                trCell.Visible = true;
                trfromdate.Visible = true;
                trtodate.Visible = true;
                trMachine.Visible = true;
                ddlMultiMachineId.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
            {
                trProdOrder.Visible = true;
                trMachine.Visible = false;
                trPartID.Visible = true;
                trGEAQualityMachines.Visible = true;
                trType.Visible = true;
                divProdOrderSearch.Visible = true;
                divCompSearch.Visible = true;
                ddlType.Items.Add(new ListItem("Hardness Report", "HardnessReport"));
                ddlType.Items.Add(new ListItem("First Sample Report", "FirstSampleReport"));
                ddlType.Items.Add(new ListItem("Dye Penetration", "DyePenetrationReport"));
                ddlType.Items.Add(new ListItem("8D Report", "QualityIncoming8DReport"));
                ddlType.Items.Add(new ListItem("IQR Report", "IQRReport"));
                ddlType.Items.Add(new ListItem("NC Report", "NCReport"));
                ddlType.Items.Add(new ListItem("Deviation Request Form", "DCReport"));
                ddlType.Items.Add(new ListItem("DPHU Report", "DPHUReport"));
                ddlType.Items.Add(new ListItem("Test Protocol", "QualityTestProtocol"));
                ddlType.Items.Add(new ListItem("Quality PC Report", "QualityPCReport"));
                //BindAllProdOrders();
                ddlQualityMachines_SelectedIndexChanged(null, null);
                List<ListItem> list = new List<ListItem>();
                list.Add(new ListItem("Time-Consolidated", "TimeConsolidated"));
                list.Add(new ListItem("Year", "Year"));
                ddlFormatType.DataSource = list;
                ddlFormatType.DataValueField = "value";
                ddlFormatType.DataTextField = "text";
                ddlFormatType.DataBind();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("AssemblyTestingPackingReport", StringComparison.OrdinalIgnoreCase))
            {
                trProdOrder.Visible = true;
                trFabrication.Visible = true;
                trPartID.Visible = false;
                divProdOrderSearch.Visible = true;
                divFabricationSearch.Visible = true;
                BindAllProdOrders();
                BindComponentIDs();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("NonMachiningReport", StringComparison.OrdinalIgnoreCase))
            {
                trFormatType.Visible = true;
                trNonMachineMachineID.Visible = true;
                divFabricationSearch.Visible = true;
                divProdOrderSearch.Visible = true;
                if (ddlNonMachineMachineID.Items.Count > 0)
                {
                    ddlNonMachineMachineID.SelectedIndex = 0;
                }
                ddlNonMachineMachineID_SelectedIndexChanged(null, null);
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("DecanterAcceptanceTestCardReport", StringComparison.OrdinalIgnoreCase))
            {
                trProdOrder.Visible = true;
                trFabrication.Visible = true;
                divFabricationSearch.Visible = true;
                divProdOrderSearch.Visible = true;
                BindAllProdOrders();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("BalancingCertificate", StringComparison.OrdinalIgnoreCase))
            {
                trProdOrder.Visible = true;
                divProdOrderSearch.Visible = true;
                trPartID.Visible = true;
                divCompSearch.Visible = true;
                BindAllProdOrders();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ProdOrderStatusReport", StringComparison.OrdinalIgnoreCase))
            {
                trProdOrder.Visible = false;
                trFabrication.Visible = true;
                ddlMultiFabNum.Visible = true;
                ddlFabriation.Visible = false;
                divFabricationSearch.Visible = false;
                trfromdate.Visible = true;
                trtodate.Visible = true;
                trProType.Visible = true;
                //BindAllProdOrders();
                BindFabricationNum();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("CEChecklistReport", StringComparison.OrdinalIgnoreCase))
            {
                trProdOrder.Visible = true;
                trFabrication.Visible = true;
                trPartID.Visible = false;
                divProdOrderSearch.Visible = true;
                divFabricationSearch.Visible = true;
                BindAllProdOrders();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ProDecanterReport", StringComparison.OrdinalIgnoreCase))
            {
                trProdOrder.Visible = true;
                trFabrication.Visible = true;
                trPartID.Visible = false;
                divProdOrderSearch.Visible = true;
                divFabricationSearch.Visible = true;
                BindAllProdOrders();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("MachineMixReport", StringComparison.OrdinalIgnoreCase))
            {
                trfromdate.Visible = true;
                trtodate.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ParkedOrderReasons", StringComparison.OrdinalIgnoreCase))
            {
                trfromdate.Visible = true;
                trtodate.Visible = true;
                trMachine.Visible = true;
                trCell.Visible = true;
                ddlMultiMachineId.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ProductionScheduleReport", StringComparison.OrdinalIgnoreCase))
            {
                trPartID.Visible = true;
                divCompSearch.Visible = true;
                trCell.Visible = true;
                trMachine.Visible = true;
                ddlMachineId.Visible = true;
                trfromdate.Visible = true;
                trtodate.Visible = true;
                trStatus.Visible = true;
                trProdOrder.Visible = true;
                divProdOrderSearch.Visible = true;
                trPartID.Visible = true;
                BindMachineID();
                BindAllProdOrders();
                BindComponentIDs();
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("MonthlyOperatorEfficiencyReport", StringComparison.OrdinalIgnoreCase))
            {

                trOperator.Visible = true;
                ddlMultiOperatorID.Visible = true;
                trfromdate.Visible = true;
                trtodate.Visible = true;
                trPlant.Visible = true;
                trShift.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("ModelStdTimevsActual", StringComparison.OrdinalIgnoreCase))
            {
                trfromdate.Visible = true;
                trtodate.Visible = true;
                //trProdOrder.Visible = true;
                //txtFabricationSearch.Visible = true;
                //txtProdOrderSearch.Visible = true;
                //divProdOrderSearch.Visible = true;
                //divFabricationSearch.Visible = true;
                //trFabrication.Visible = true;
                trProcessType.Visible = true;
            }
            else if (ddlReportType.SelectedValue.ToString().Equals("MachineWiseAssemblyReport", StringComparison.OrdinalIgnoreCase))
            {
                trFabrication.Visible = true;
                ddlFabriation.Visible = true;
                divFabricationSearch.Visible = true;
                ddlMultiFabNum.Visible = false;
                txtFabricationSearch.Visible = true;
                BindFabricationNoForMachineWiseAssemblyReport();
            }
        }
        private void BindMachineIDForNonMachiningReport()
        {
            try
            {
                List<string> list = GEADatabaseAccess.getMachineIDForNonMachiningData();
                ddlNonMachineMachineID.DataSource = list;
                ddlNonMachineMachineID.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void HideAlltr()
        {
            try
            {
                trPlant.Visible = false;
                trCell.Visible = false;
                trInsPlan.Visible = false;
                trMachine.Visible = false;
                trOperation.Visible = false;
                trProdOrder.Visible = false;
                trPartID.Visible = false;
                trPlant.Visible = false;
                trfromdate.Visible = false;
                trtodate.Visible = false;
                trGEAQualityMachines.Visible = false;
                trFabrication.Visible = false;
                trNonMachineMachineID.Visible = false;
                trFormatType.Visible = false;
                trType.Visible = false;
                divFabricationSearch.Visible = false;
                divProdOrderSearch.Visible = false;
                divCompSearch.Visible = false;
                txtCompSearch.Text = "";
                txtProdOrderSearch.Text = "";
                txtFabricationSearch.Text = "";
                trGRNNumber.Visible = false;
                ddlMachineId.Visible = false;
                ddlMultiMachineId.Visible = false;
                trYear.Visible = false;
                trProType.Visible = false;
                trOperator.Visible = false;
                trShift.Visible = false;
                ddlMultiFabNum.Visible = false;
                trProcessType.Visible = false;
                trStatus.Visible = false;
                trSubFormat.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void ddlProdOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.Equals("BalancingCertificate", StringComparison.OrdinalIgnoreCase))
                {
                    BindComponentIDs();
                }
                else
                {
                    BindFabrication();
                }
                BindGRNNumber();
                BindInsPlanNumbers();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindGRNNumber()
        {
            try
            {
                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ddlType.SelectedValue.Equals("DPHUReport", StringComparison.OrdinalIgnoreCase))
                    {
                        List<ProdFabricationNumber> ProdorderList = new List<ProdFabricationNumber>();
                        if (Session["Fabricationnumber"] != null)
                        {
                            ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                        }
                        ddlGRNNumber.DataSource = ProdorderList.Where(k => k.MaterialNumber == ddlComponent.SelectedValue && k.ProdOrderNumber == ddlProdOrder.SelectedValue).Select(k => k.GRNNumber).Distinct().ToList();
                        ddlGRNNumber.DataBind();
                        lblGrnMessage.Text = "* GRN not entered.";
                        if (ddlGRNNumber != null)
                        {
                            if (ddlGRNNumber.Items.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ddlGRNNumber.Items[0].Value))
                                {
                                    lblGrnMessage.Text = "";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindGRNNumber =" + ex.Message);
            }
        }

        private void BindFabrication()
        {
            try
            {
                if (ddlProdOrder.SelectedValue != null)
                {
                    List<ProdFabricationNumber> ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                    // ddlFabriation.SelectedValue = ProdorderList.Where(x => x.ProdOrderNumber.Equals(ddlProdOrder.SelectedValue)).SingleOrDefault().FabricationNumber;

                    var serachedList = ProdorderList.Where(x => x.ProdOrderNumber.Equals(ddlProdOrder.SelectedValue)).ToList();
                    if (serachedList != null)
                    {
                        for (int i = 0; i < serachedList.Count; i++)
                        {
                            if (ddlFabriation.Items.FindByValue(serachedList[i].FabricationNumber) != null)
                            {
                                ddlFabriation.SelectedValue = serachedList[i].FabricationNumber;
                                break;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlNonMachineMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<ListItem> list = new List<ListItem>();
                string process = getMachineIdProcess(ddlNonMachineMachineID.SelectedValue);
                if (string.Equals(process, "Assembly", StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(new ListItem("Machine Data Assembly", "MachineDataAssemblyReport"));
                    list.Add(new ListItem("Electro-Tech Equipment Report", "ElectroTechEquipmentReport"));
                    //if (ddlNonMachineMachineID.SelectedValue.Equals("Assembly-2", StringComparison.OrdinalIgnoreCase))
                    //{
                    list.Add(new ListItem("Pro Decanter Report", "ProDecanterReportOnly"));
                    //}
                }
                else if (string.Equals(process, "Packing", StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(new ListItem("Decanter Checklist Packing Report", "DecanterChecklistPackingReport"));
                    list.Add(new ListItem("Decanter Final Checklist Packing Report", "DecanterFinalChecklistPackingReport"));
                }
                else if (string.Equals(process, "Testing", StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(new ListItem("Decanter checklist", "TestingReport"));
                    list.Add(new ListItem("Noise Measurement Report", "NoiseMeasurementReport"));
                    list.Add(new ListItem("Vibration Test Protocol Report", "VibrationTestProtocolReport"));
                }
                else if (string.Equals(process, "Stores", StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(new ListItem("Picking List Report", "PickingListReport"));
                }
                ddlFormatType.DataSource = list;
                ddlFormatType.DataValueField = "value";
                ddlFormatType.DataTextField = "text";
                ddlFormatType.DataBind();
                ddlFormatType_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlFormatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (ddlReportType.SelectedValue.ToString().Equals("NonMachiningReport", StringComparison.OrdinalIgnoreCase))
                {
                    trProdOrder.Visible = false;
                    trFabrication.Visible = false;
                    trPartID.Visible = false;
                    trSubFormat.Visible = false;
                    if (ddlFormatType.SelectedValue.ToString().Equals("MachineDataAssemblyReport", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.ToString().Equals("ElectroTechEquipmentReport", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.ToString().Equals("ProDecanterReportOnly", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.ToString().Equals("PickingListReport", StringComparison.OrdinalIgnoreCase))
                    {
                        trProdOrder.Visible = true;
                        trFabrication.Visible = true;
                        BindAllProdOrders();
                        if (ddlFormatType.SelectedValue.ToString().Equals("PickingListReport", StringComparison.OrdinalIgnoreCase))
                        {
                            trSubFormat.Visible = true;
                        }
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("TestingReport", StringComparison.OrdinalIgnoreCase))
                    {
                        trProdOrder.Visible = true;
                        trFabrication.Visible = true;
                        trPartID.Visible = false;
                        BindAllProdOrders();
                        BindComponentIDs();
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("VibrationTestProtocolReport", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.ToString().Equals("NoiseMeasurementReport", StringComparison.OrdinalIgnoreCase))
                    {
                        trProdOrder.Visible = true;
                        trFabrication.Visible = true;
                        trPartID.Visible = false;
                        BindAllProdOrders();
                        BindComponentIDs();
                    }
                    else if (ddlFormatType.SelectedValue.ToString().Equals("DecanterChecklistPackingReport", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.ToString().Equals("DecanterFinalChecklistPackingReport", StringComparison.OrdinalIgnoreCase) || ddlFormatType.SelectedValue.ToString().Equals("DecanterAcceptanceTestCardReport", StringComparison.OrdinalIgnoreCase))
                    {
                        trProdOrder.Visible = true;
                        trFabrication.Visible = true;
                        BindAllProdOrders();
                    }
                }
                else if (ddlReportType.SelectedValue.ToString().Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    trfromdate.Visible = false;
                    trtodate.Visible = false;
                    trYear.Visible = false;
                    if (ddlType.SelectedValue.ToString().Equals("QualityPCReport", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ddlFormatType.SelectedValue.ToString().Equals("TimeConsolidated", StringComparison.OrdinalIgnoreCase))
                        {
                            trfromdate.Visible = true;
                            trtodate.Visible = true;
                        }
                        else if (ddlFormatType.SelectedValue.ToString().Equals("Year", StringComparison.OrdinalIgnoreCase))
                        {
                            trYear.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlQualityMachines_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                BindAllProdOrders();
                trGRNNumber.Visible = false;
                if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    if (!(ddlType.SelectedValue.Equals("DPHUReport", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.Equals("QualityPCReport", StringComparison.OrdinalIgnoreCase)))
                    {
                        //string process = getMachineIdProcess(ddlQualityMachines.SelectedValue);
                        //if (process.Equals("QualityIncoming", StringComparison.OrdinalIgnoreCase))
                        //{
                        trGRNNumber.Visible = true;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ddlQualityMachines_SelectedIndexChanged = " + ex.Message);
            }
        }

        protected void ddlFabriation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlFabriation.SelectedValue != null)
                {
                    List<ProdFabricationNumber> ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                    ddlProdOrder.SelectedValue = ProdorderList.Where(x => x.FabricationNumber.Equals(ddlFabriation.SelectedValue)).SingleOrDefault().ProdOrderNumber;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCompSearch.Text = "";
                txtProdOrderSearch.Text = "";
                txtFabricationSearch.Text = "";
                if (ddlReportType.SelectedValue.ToString().Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
                {
                    //BindAllProdOrders();
                    ddlQualityMachines_SelectedIndexChanged(null, null);
                    trfromdate.Visible = false;
                    trtodate.Visible = false;
                    trProdOrder.Visible = false;
                    trPartID.Visible = false;
                    trOperation.Visible = false;
                    trInsPlan.Visible = false;
                    divCompSearch.Visible = false;
                    divProdOrderSearch.Visible = false;
                    trFormatType.Visible = false;
                    if (ddlType.SelectedValue.ToString().Equals("DPHUReport", StringComparison.OrdinalIgnoreCase))
                    {
                        trfromdate.Visible = true;
                        trtodate.Visible = true;
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                    {
                        trProdOrder.Visible = true;
                        trPartID.Visible = true;
                        trOperation.Visible = true;
                        trInsPlan.Visible = true;
                        divCompSearch.Visible = true;
                        divProdOrderSearch.Visible = true;
                        BindComponentIDs();
                    }
                    else if (ddlType.SelectedValue.ToString().Equals("QualityPCReport", StringComparison.OrdinalIgnoreCase))
                    {
                        trFormatType.Visible = true;
                        ddlFormatType_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        trPartID.Visible = true;
                        trProdOrder.Visible = true;
                        divCompSearch.Visible = true;
                        divProdOrderSearch.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase))
            {
                if (ddlType.SelectedValue.ToString().Equals("QualityTestProtocol", StringComparison.OrdinalIgnoreCase))
                {
                    BindInsPlanNumbers();
                }
            }
        }

        protected void lnkProdOrderSearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtFabricationSearch.Text = "";
                txtCompSearch.Text = "";
                if (ddlReportType.SelectedValue.Equals("ProductionScheduleReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> prodList = GEADatabaseAccess.GetAllProdOrderbyMachineforProductionSchedule(ddlMachineId.SelectedValue.ToString());
                    List<string> list = prodList.Where(x => x.StartsWith(txtProdOrderSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    ddlProdOrder.DataSource = list.Select(k => k).Distinct().ToList();
                    ddlProdOrder.DataBind();
                }
                else
                {
                    List<ProdFabricationNumber> ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                    List<ProdFabricationNumber> searchedList = ProdorderList.Where(x => x.ProdOrderNumber.StartsWith(txtProdOrderSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    ddlProdOrder.DataSource = searchedList.Select(k => k.ProdOrderNumber).Distinct().ToList();
                    ddlProdOrder.DataBind();

                    if (ddlReportType.SelectedValue.Equals("QualityIncomingReport", StringComparison.OrdinalIgnoreCase) || ddlReportType.SelectedValue.Equals("BalancingCertificate", StringComparison.OrdinalIgnoreCase))
                    {
                        ddlComponent.DataSource = searchedList.Select(k => k.MaterialNumber).Distinct().ToList();
                        ddlComponent.DataBind();
                    }
                    else
                    {
                        ddlFabriation.DataSource = searchedList.Select(k => k.FabricationNumber).ToList();
                        ddlFabriation.DataBind();
                    }
                }


                ddlProdOrder_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindProductionOrderWithSearch()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lnkFabricationSearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtProdOrderSearch.Text = "";
                if (ddlReportType.SelectedValue.Equals("MachineWiseAssemblyReport", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable prodLists = GEADatabaseAccess.GetFabricationNo();
                    List<string> List = prodLists.AsEnumerable().Select(x => x.Field<string>("FabricationNo")).Distinct().ToList();
                    List<string> list1 = List.Where(x => x.StartsWith(txtFabricationSearch.Text, StringComparison.OrdinalIgnoreCase)).Select(x => x).Distinct().ToList();
                    ddlFabriation.DataSource = list1;
                    ddlFabriation.DataBind();
                }
                else
                {
                    List<ProdFabricationNumber> ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                    List<ProdFabricationNumber> searchedList = ProdorderList.Where(x => x.FabricationNumber.StartsWith(txtFabricationSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    ddlFabriation.DataSource = searchedList.Select(k => k.FabricationNumber).ToList();
                    ddlFabriation.DataBind();

                    ddlProdOrder.DataSource = searchedList.Select(k => k.ProdOrderNumber).Distinct().ToList();
                    ddlProdOrder.DataBind();
                }
                ddlFabriation_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lnkCompSearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtProdOrderSearch.Text = "";
                if (ddlReportType.SelectedValue.Equals("ProductionScheduleReport", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> componentlist = GEADatabaseAccess.GetAllComponentbyMachineforProductionSchedule(ddlMachineId.SelectedValue.ToString());
                    List<string> list = componentlist.Where(x => x.StartsWith(txtCompSearch.Text, StringComparison.OrdinalIgnoreCase)).Select(k => k).Distinct().ToList();
                    ddlComponent.DataSource = list;
                    ddlComponent.DataBind();
                }
                else
                {
                    List<ProdFabricationNumber> ProdorderList = Session["Fabricationnumber"] as List<ProdFabricationNumber>;
                    List<ProdFabricationNumber> searchedList = ProdorderList.Where(x => x.MaterialNumber.StartsWith(txtCompSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                    ddlComponent.DataSource = searchedList.Select(k => k.MaterialNumber).Distinct().ToList();
                    ddlComponent.DataBind();
                }
                //ddlProdOrder.DataSource = searchedList.Select(k => k.ProdOrderNumber).ToList();
                //ddlProdOrder.DataBind();

                ddlComponent_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
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

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
            BindOperatorID();
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
            BindOperatorID();
        }
        protected void ddlProType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFabricationNum();
        }

        protected void txtfromoDate_TextChanged(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue.ToString().Equals("ProdOrderStatusReport", StringComparison.OrdinalIgnoreCase))
            {
                BindFabricationNum();
            }
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue.ToString().Equals("ProdOrderStatusReport", StringComparison.OrdinalIgnoreCase))
            {
                BindFabricationNum();
            }
        }
        private void BindFabricationNum()
        {
            try
            {
                string proType = ddlProType.SelectedValue.ToString();
                //List<ProdFabricationNumber> ProdorderList = GEADatabaseAccess.GetProductionOrderStatusFabricationNum(proType,txtfromoDate.Text, txtToDate.Text);
                List<string> ProdorderList = GEADatabaseAccess.GetProductionOrderStatusFabricationNum(proType, txtfromoDate.Text, txtToDate.Text);

                if (ProdorderList != null && ProdorderList.Count > 0)
                {
                    ddlMultiFabNum.DataSource = ProdorderList;
                    ddlMultiFabNum.DataBind();

                    ProdorderList.Insert(0, "All");
                    ddlFabriation.DataSource = ProdorderList;
                    ddlFabriation.DataBind();
                    ddlFabriation.SelectedIndex = 0;
                    foreach (ListItem item in ddlMultiFabNum.Items)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    ddlMultiFabNum.DataSource = new List<string>();
                    ddlMultiFabNum.DataBind();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindFabricationNoForMachineWiseAssemblyReport()
        {
            try
            {
                DataTable dt = GEADatabaseAccess.GetFabricationNo();
                Session["Fabricationnumber"] = dt;
                List<string> List = dt.AsEnumerable().Select(x => x.Field<string>("FabricationNo")).Distinct().ToList();
                if (List != null && List.Count > 0)
                {
                    ddlFabriation.DataSource = List;
                    ddlFabriation.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindShiftDetails()
        {
            try
            {
                var allShift = BindCockpitView.GetAllShift();
                allShift.Insert(0, "All");
                ddlShift.DataSource = allShift;
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlMachineId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComponentIDs();
            BindAllProdOrders();
        }

        protected void ddlSubFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindAllProdOrders();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("ddlSubFormat_SelectedIndexChanged= " + ex.ToString());
            }
        }
    }

    public class ProdFabricationNumber
    {
        public string ProdOrderNumber { get; set; }
        public string FabricationNumber { get; set; }
        public string MaterialNumber { get; set; }
        public string GRNNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ProType { get; set; }
        public string All { get; set; }
        public string machineid { get; set; }
        public string ComponentID { get; set; }
    }
    public class Operator
    {
        public string OperatorID { get; set; }
        public string OperatorName { get; set; }
    }
    public class MachinIdProcessEnity
    {
        public string MachineID { get; set; }
        public string Process { get; set; }
    }
}