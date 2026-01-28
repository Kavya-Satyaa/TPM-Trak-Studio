using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ModifiedData : System.Web.UI.Page
    {

        DateTime StartTime = DateTime.Now;
        DateTime EndTime = DateTime.Now;
        string[] MachineID;
        string MachineInterfaceID;
        string MachineName;
        string[] ComponentID;
        string CompInterfaceID;
        string[] OpnID;
        string OpnInterfaceID;
        //string[] Opr;
        //string OprInterfaceID;
        //bool rowAdded = false;
        List<string> downcode = new List<string>();
        List<string> downids = new List<string>();
        List<string> ComponentBusinessID = new List<string>();
        List<string> OperatorBusinessID = new List<string>();

        List<ModifiedDataEntity> downcodeWithInterfaceID = new List<ModifiedDataEntity>();
        List<ModifiedDataEntity> operatorWithInterfaceID = new List<ModifiedDataEntity>();
        List<ModifiedDataEntity> componentWithInterfaceID = new List<ModifiedDataEntity>();
        bool ShowWorkOrder = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadData();
                setlogicaldates();
                ddldatatype_SelectedIndexChanged(null, null);

                hdnShowWorkOrderNo.Value = "Y";
                ShowWorkOrder = DataBaseAccess.WorkOrderVisibility();
                if (!ShowWorkOrder)
                {
                    hdnShowWorkOrderNo.Value = "N";
                    liWorkOrder.Visible = false;
                }
                //txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm"); ;
                //txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd-MMM-yyyy HH:mm");
            }
            setActiveTabs();

        }
        private void setActiveTabs()
        {
            liWorkOrder.Attributes["class"] = "";
            liRejectionCode.Attributes["class"] = "";
            liCompOperation.Attributes["class"] = "";
            liOperator.Attributes["class"] = "";
            liDownID.Attributes["class"] = "";
            liPartCount.Attributes["class"] = "";
            workorder.Attributes["class"] = "";
            rejcode.Attributes["class"] = "";
            componentoperation.Attributes["class"] = "";
            operators.Attributes["class"] = "";
            downid.Attributes["class"] = "";
            partscount.Attributes["class"] = "";

            liWorkOrder.Attributes["class"] = "tabsName";
            liRejectionCode.Attributes["class"] = "tabsName";
            liCompOperation.Attributes["class"] = "tabsName";
            liOperator.Attributes["class"] = "tabsName";
            liDownID.Attributes["class"] = "tabsName";
            liPartCount.Attributes["class"] = "tabsName";
            workorder.Attributes["class"] = "tab-pane fade";
            rejcode.Attributes["class"] = "tab-pane fade";
            componentoperation.Attributes["class"] = "tab-pane fade";
            operators.Attributes["class"] = "tab-pane fade";
            downid.Attributes["class"] = "tab-pane fade";
            partscount.Attributes["class"] = "tab-pane fade";
            if (hdnActiveTabID.Value == "liWorkOrder")
            {
                liWorkOrder.Attributes.Add("class", "tabsName active");
                workorder.Attributes.Add("class", "tab-pane fade in active");
            }
            else if (hdnActiveTabID.Value == "liRejectionCode")
            {
                liRejectionCode.Attributes.Add("class", "tabsName active");
                rejcode.Attributes.Add("class", "tab-pane fade in active");
            }
            else if (hdnActiveTabID.Value == "liCompOperation")
            {
                liCompOperation.Attributes.Add("class", "tabsName active");
                componentoperation.Attributes.Add("class", "tab-pane fade in active");
            }
            else if (hdnActiveTabID.Value == "liOperator")
            {
                liOperator.Attributes.Add("class", "tabsName active");
                operators.Attributes.Add("class", "tab-pane fade in active");
            }
            else if (hdnActiveTabID.Value == "liDownID")
            {
                liDownID.Attributes.Add("class", "tabsName active");
                downid.Attributes.Add("class", "tab-pane fade in active");
            }
            else if (hdnActiveTabID.Value == "liPartCount")
            {
                liPartCount.Attributes.Add("class", "tabsName active");
                partscount.Attributes.Add("class", "tab-pane fade in active");
            }
            else
            {
                liWorkOrder.Attributes.Add("class", "tabsName active");
                workorder.Attributes.Add("class", "tab-pane fade in active");
            }
        }
        private void setlogicaldates()
        {
            string dates = VDGDataBaseAccess.GetLogicalDayStart(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            txtFromDate.Text = Convert.ToDateTime(dates).ToString("dd-MMM-yyyy HH:mm");
            dates = VDGDataBaseAccess.GetLogicalDayEnd(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            txtToDate.Text = Convert.ToDateTime(dates).ToString("dd-MMM-yyyy HH:mm");
        }

        private void BindMachineId()
        {
            try
            {
                List<string> mInfos = new List<string>();
                mInfos = DataBaseAccess.GetAllMachinesForPlant(ddlplant.Text);
                if (ddlmachine.Items.Count > 0)
                {
                    ddlmachine.DataSource = mInfos;
                    ddlmachine.DataBind();
                    ddlmachine.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
        }


        private void loadData()
        {
            ddlplant.DataSource = DataBaseAccess.GetAllPlants();
            ddlplant.DataBind();
            List<string> mInfos = new List<string>();
            mInfos = DataBaseAccess.GetAllMachinesForPlant("All");

            ddlmachine.DataSource = mInfos;
            ddlmachine.DataBind();
            if (ddlmachine.Items.Count > 0)
            {
                ddlmachine.SelectedIndex = 0;
            }
            GetTabInformation();
        }

        private void GetTabInformation()
        {
            try
            {
                StartTime = Util.GetDateTime(txtFromDate.Text); 
                EndTime = Util.GetDateTime(txtToDate.Text); 
                MachineID = ddlmachine.Text.ToString().Split('<');
                MachineName = MachineID[1].ToString().Trim('>');
                ddltocomponent.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Component");
                ddltocomponent.DataBind();
                ddlrejto.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Rejection");
                ddlrejto.DataBind();
                ddlopr_to.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Operator");
                ddlopr_to.DataBind();
                ddldown_to.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Downid");
                ddldown_to.DataBind();

                if (ddltocomponent.Items.Count > 0)
                {
                    ddltocomponent.SelectedIndex = 0;
                    ComponentID = ddltocomponent.Text.ToString().Split('<');
                    CompInterfaceID = ComponentID[0].ToString().Trim();
                    ddlopreration_to.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineName.ToString(), CompInterfaceID.ToString(), "", "", "", "", "Operation");
                    ddlopreration_to.DataBind();
                }
                if (ddlrejto.Items.Count > 0) ddlrejto.SelectedIndex = 0;
                if (ddlopr_to.Items.Count > 0) ddlopr_to.SelectedIndex = 0;
                if (ddldown_to.Items.Count > 0) ddldown_to.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error - \n" + ex.ToString());
            }
        }

        protected void btnview_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loader", "ShowLoader()", true);
            try
            {
                hdnViewValue.Value = "NormalView";
                Session["ComponentBusinessID"] = null;
                Session["downcode"] = null;
                Session["OperatorBusinessID"] = null;
                Session["ProdcutionData"] = null;
                if (string.IsNullOrEmpty(ddlplant.SelectedValue.ToString())) return;
                if (string.IsNullOrEmpty(ddlmachine.SelectedValue.ToString())) return;
                if (string.IsNullOrEmpty(txtFromDate.Text.ToString())) return;
                if (string.IsNullOrEmpty(txtToDate.Text.ToString())) return;
                StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
                EndTime = Util.GetDateTime(txtToDate.Text.ToString());
                if ((EndTime - StartTime).Days > 7)
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showalert", "alert('Time interval cannot be more than 7 days.')", true);
                else
                {
                    GetTabInformation();
                    getGriddata();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
                throw;
                //Logger.WriteErrorLog("" + ex.Message);
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loaderhide", "HideLoader()", true);
        }

        private void BindOperation()
        {
            ComponentID = ddlpartcomp.Text.ToString().Split('<');
            CompInterfaceID = ComponentID[0].ToString().Trim();
            MachineID = ddlmachine.Text.ToString().Split('<');
            MachineInterfaceID = MachineID[0].ToString().Trim();
            StartTime = Util.GetDateTime(txtFromDate.Text);
            EndTime = Util.GetDateTime(txtToDate.Text);
            ddlpatsopr.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, CompInterfaceID, "", "", "", "", "FromOpnForPartsCount");
            ddlpatsopr.DataBind();
        }

        private void BindPartsCount()
        {
            ComponentID = ddlpartcomp.Text.ToString().Split('<');
            CompInterfaceID = ComponentID[0].ToString().Trim();
            MachineID = ddlmachine.Text.ToString().Split('<');
            MachineInterfaceID = MachineID[0].ToString().Trim();
            OpnID = ddlpatsopr.Text.ToString().Split('<');
            StartTime = Util.GetDateTime(txtFromDate.Text);
            EndTime = Util.GetDateTime(txtToDate.Text);
            OpnInterfaceID = OpnID[0].ToString().Trim();
            ddlfrmcount.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, CompInterfaceID, OpnInterfaceID, "", "", "", "PartsCount");
            ddlfrmcount.DataBind();
        }

        private void BindCompOpn()
        {
            ComponentID = ddlfrmcomponent.Text.ToString().Split('<');
            CompInterfaceID = ComponentID[0].ToString().Trim();
            MachineID = ddlmachine.Text.ToString().Split('<');
            MachineInterfaceID = MachineID[0].ToString().Trim();
            StartTime = Util.GetDateTime(txtFromDate.Text);
            EndTime = Util.GetDateTime(txtToDate.Text);
            ddlfrmoperation.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, CompInterfaceID, "", "", "", ddldatatype.Text.ToString(), "Opn");
            ddlfrmoperation.DataBind();
            //if (frmoperation.Items.Count > 0)
            //{
            //    frmoperation.SelectedIndex = 0;
            //}
        }

        //private void getdatatest()
        //{
        //    DataTable dtProductionData = new DataTable();
        //    DataTable dtDownData = new DataTable();
        //    DataTable dtMachineEvents = new DataTable();
        //    DataTable dtRejectionData = new DataTable();
        //    StartTime = Convert.ToDateTime(txtFromDate.Text.ToString());// Convert.ToDateTime("2018-01-01");
        //    EndTime = Convert.ToDateTime(txtToDate.Text.ToString());//Convert.ToDateTime("2018-01-30");*/
        //    dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Production Data");
        //    dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Down Data");
        //}


        #region refresh grid
        public void GridRefresh()
        {
            DataTable updategrd = new DataTable();
            MachineInterfaceID = ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "";
            StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
            EndTime = Util.GetDateTime(txtToDate.Text.ToString());
            if (ddldatatype.SelectedValue.ToString() == "Production Data")
            {
                updategrd = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Production Data");
                if (updategrd.Rows.Count > 0)
                {
                    datagridproductiondata.DataSource = updategrd;
                    datagridproductiondata.DataBind();
                    datagridproductiondata.Columns[1].Visible = false;
                }

            }
            else if (ddldatatype.SelectedValue.ToString() == "Down Data")
            {
                updategrd = null;
                updategrd = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Down Data");
                if (updategrd.Rows.Count > 0)
                {
                    Session["DownDataSplitFun"] = updategrd;
                    downdatagrid.DataSource = updategrd;
                    downdatagrid.DataBind();
                }
            }
            else if (ddldatatype.SelectedValue.ToString() == "Rejection Data")
            {
                updategrd = null;
                updategrd = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Rejection Data");
                if (updategrd.Rows.Count > 0)
                {
                    rejectiongrd.DataSource = updategrd;
                    rejectiongrd.DataBind();
                }

            }
        }
        #endregion

        #region view all gried data depends on the selection
        private void getGriddata()
        {
            try
            {

                DataTable dtProductionData = new DataTable();
                DataTable dtDownData = new DataTable();
                DataTable dtMachineEvents = new DataTable();
                DataTable dtRejectionData = new DataTable();

                //txtFromDate.Text = "2018-01-01" ;

                StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
                EndTime = Util.GetDateTime(txtToDate.Text.ToString());
                MachineID = ddlmachine.Text.ToString().Split('<');
                MachineInterfaceID = MachineID[0].ToString().Trim();
                MachineName = MachineID[1].ToString().Trim('>');

                if (ddldatatype.SelectedValue.ToString() == "Production Data")
                {
                    ddlrejfrom.Items.Clear();
                    ddlfrmdown.Items.Clear();
                    datagridproductiondata.Columns[0].Visible = true;

                    ddlfrm.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "", "ProdWorkOrder");
                    ddlfrm.DataBind();
                    ddlpartcomp.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "", "FromComponentForPartsCount");
                    ddlpartcomp.DataBind();
                    ddlfrmcomponent.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Production Data", "Comp");
                    ddlfrmcomponent.DataBind();
                    if (ddlfrmcomponent.Items.Count > 0)
                    {
                        ddlfrmcomponent.SelectedIndex = 0;
                        BindCompOpn();
                    }
                    if (ddlpartcomp.Items.Count > 0)
                    {
                        ddlpartcomp.SelectedIndex = 0;
                        BindOperation();
                    }
                    if (ddlpatsopr.Items.Count > 0)
                    {
                        ddlpatsopr.SelectedIndex = 0;
                        BindPartsCount();
                    }
                    ddlfrmorp.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Production Data", "Operator");
                    ddlfrmorp.DataBind();
                    if (ddlfrmorp.Items.Count > 0) ddlfrmorp.SelectedIndex = 0;
                    //BindProductionData();
                    Session["ProdcutionData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Production Data");
                    datagridproductiondata.DataSource = dtProductionData;
                    datagridproductiondata.DataBind();
                    datagridproductiondata.Columns[1].Visible = false;
                    datagridproductiondata.Columns[4].Visible = true;
                    datagridproductiondata.Columns[7].Visible = true;
                    downdatagrid.Visible = false;
                    rejectiongrd.Visible = false;
                    datagridproductiondata.Visible = true;
                }
                else if (ddldatatype.SelectedValue.ToString() == "Down Data")
                {
                    ddlrejfrom.Items.Clear();
                    ddlpartcomp.Items.Clear();
                    ddlfrmcount.Items.Clear();
                    ddlpatsopr.Items.Clear();
                    ddlfrmcomponent.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Down Data", "Comp");
                    ddlfrmcomponent.DataBind();
                    //downdatagrid.Columns[0].Visible = true;   
                    ddlfrm.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Down Data", "Comp");
                    if (ddlfrm.Items.Count > 0)
                    {
                        ddlfrm.SelectedIndex = 0;
                        BindCompOpn();
                    }

                    ddlfrmorp.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Down Data", "Operator");
                    ddlfrmorp.DataBind();
                    if (ddlfrmorp.Items.Count > 0) ddlfrmorp.SelectedIndex = 0;
                    ddlfrm.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "", "DownWorkOrder");
                    ddlfrm.DataBind();
                    ddlfrmdown.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Down Data", "DownID");
                    ddlfrmdown.DataBind();
                    if (ddlfrmdown.Items.Count > 0) ddlfrmdown.SelectedIndex = 0;

                    //frmdown
                    Session["DownData"] = dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Down Data");
                    Session["DownDataSplitFun"] = dtDownData;
                    downdatagrid.DataSource = dtDownData;
                    downdatagrid.DataBind();
                    downdatagrid.Columns[6].Visible = true;
                    downdatagrid.Columns[3].Visible = true;
                    datagridproductiondata.Visible = false;
                    rejectiongrd.Visible = false;
                    downdatagrid.Visible = true;
                }
                else if (ddldatatype.SelectedValue.ToString() == "Rejection Data")
                {
                    ddlfrm.Items.Clear();
                    ddlfrmdown.Items.Clear();
                    ddlpartcomp.Items.Clear();
                    ddlfrmcount.Items.Clear();
                    ddlpatsopr.Items.Clear();
                    ddlfrmcomponent.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Rejection Data", "Comp");
                    ddlfrmcomponent.DataBind();
                    if (ddlfrmcomponent.Items.Count > 0)
                    {
                        ddlfrmcomponent.SelectedIndex = 0;
                        BindCompOpn();
                    }
                    ddlrejfrom.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Rejection Data", "Rejection");
                    ddlrejfrom.DataBind();
                    ddlfrmorp.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineInterfaceID, "", "", "", "", "Rejection Data", "Operator");
                    ddlfrmorp.DataBind();
                    if (ddlfrmorp.Items.Count > 0) ddlfrmorp.SelectedIndex = 0;
                    if (ddlrejfrom.Items.Count > 0) ddlrejfrom.SelectedIndex = 0;
                    Session["RejectionData"] = dtRejectionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Rejection Data");
                    rejectiongrd.DataSource = dtRejectionData;
                    rejectiongrd.DataBind();
                    datagridproductiondata.Visible = false;
                    downdatagrid.Visible = false;
                    rejectiongrd.Visible = true;
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loaderhide", "HideLoader()", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("" + ex.Message);
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }

        }
        #endregion

        private void BindProductionData()
        {
            DataTable dtProductionData = new DataTable();
            StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
            EndTime = Util.GetDateTime(txtToDate.Text.ToString());
            if (Session["ProdcutionData"] == null)
                Session["ProdcutionData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Production Data");
            else
                dtProductionData = Session["ProdcutionData"] as DataTable;
            datagridproductiondata.DataSource = dtProductionData;
            datagridproductiondata.DataBind();
            //datagridproductiondata.Columns[1].Visible = false;
            downdatagrid.Visible = false;
            rejectiongrd.Visible = false;
            datagridproductiondata.Visible = true;
        }

        #region store data in session
        private void commanDataBind()
        {

            //if (Session["downcode"] == null)
            //    Session["downcode"] = downcode = DataBaseAccess.GetAllParameterWithInteface(DateTime.Now, DateTime.Now, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Downid");   // Session["downcode"] = downcode = DataBaseAccess.GetAllDownCode();
            //else
            //    downcode = Session["downcode"] as List<string>;
            //if (Session["ComponentBusinessID"] == null)
            //    Session["ComponentBusinessID"] = ComponentBusinessID = DataBaseAccess.GetAllParameterWithInteface(DateTime.Now, DateTime.Now, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Component");  //DataBaseAccess.GetAllComp();
            //else
            //    ComponentBusinessID = Session["ComponentBusinessID"] as List<string>;
            //if (Session["OperatorBusinessID"] == null)
            //    Session["OperatorBusinessID"] = OperatorBusinessID = DataBaseAccess.GetAllParameterWithInteface(DateTime.Now, DateTime.Now, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Operator");// DataBaseAccess.GetAllEmployeesWithIntefaceid();
            //else
            //    OperatorBusinessID = Session["OperatorBusinessID"] as List<string>;


            if (Session["downcode"] == null)
                Session["downcode"] = downcodeWithInterfaceID = DataBaseAccess.GetAllParameterWithIntefaceAndCode(DateTime.Now, DateTime.Now, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Downid"); 
            else
                downcodeWithInterfaceID = Session["downcode"] as List<ModifiedDataEntity>;
            if (Session["ComponentBusinessID"] == null)
                Session["ComponentBusinessID"] = componentWithInterfaceID  = DataBaseAccess.GetAllParameterWithIntefaceAndCode(DateTime.Now, DateTime.Now, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Component"); 
            else
                componentWithInterfaceID = Session["ComponentBusinessID"] as List<ModifiedDataEntity>;
            if (Session["OperatorBusinessID"] == null)
                Session["OperatorBusinessID"] = operatorWithInterfaceID = DataBaseAccess.GetAllParameterWithIntefaceAndCode(DateTime.Now, DateTime.Now, ddlplant.Text.ToString(), MachineName.ToString(), "", "", "", "", "", "Operator");
            else
                operatorWithInterfaceID = Session["OperatorBusinessID"] as List<ModifiedDataEntity>;
        }
        #endregion

        #region Validations
        private bool CompareDates()
        {
            bool isDateGreater = false;
            StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
            EndTime = Util.GetDateTime(txtToDate.Text.ToString());
            //string dateVal1 = txtFromDate.Text.ToString();
            //string dateVal2 = txtToDate.Text.ToString();

            //DateTime dt1 = DateTime.Parse(dateVal1);
            //DateTime dt2 = DateTime.Parse(dateVal2);

            if (StartTime > EndTime)
            {
                isDateGreater = true;
            }
            if (StartTime == EndTime)
            {
                isDateGreater = false;
            }
            return isDateGreater;
        }
        private bool CheckDateValues()
        {
            var val = (EndTime - StartTime).Days;
            if (val > 31)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Inconsistency
        protected void btn_Inconsistency_Click(object sender, EventArgs e)
        {
            try
            {
                hdnViewValue.Value = "InconsistencyView";
                downdatagrid.Visible = false;
                datagridproductiondata.Visible = false;
                rejectiongrd.Visible = false;

                if (string.IsNullOrEmpty(ddlplant.Text.ToString())) return;
                if (string.IsNullOrEmpty(ddlmachine.Text.ToString())) return;

                DataTable dtProductionData = new DataTable();
                DataTable dtDownData = new DataTable();
                DataTable dtMachineEvents = new DataTable();
                DataTable dtRejectionData = new DataTable();

                StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
                EndTime = Util.GetDateTime(txtToDate.Text.ToString());
                if (CompareDates())
                {
                    lblMessages.Text = "To Date-Time should be less than From date.";
                    return;
                }
                if (CheckDateValues())
                {
                    lblMessages.Text = "Date-Time Period Cannot be greater than 31 Days.";
                    return;
                }

                MachineID = ddlmachine.Text.ToString().Split('<');
                MachineInterfaceID = MachineID[0].ToString().Trim();
                if (ddldatatype.SelectedValue.ToString() == "Production Data" && ddlvalidatedata.SelectedValue.ToString() == "Component-Operation")
                {
                    Session["ProdcutionData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Component-OperationInconsistencyInProd");
                    //datagridproductiondata.Columns.Add()
                    datagridproductiondata.Columns[4].Visible = false;
                    datagridproductiondata.Columns[7].Visible = false;
                    datagridproductiondata.Columns[1].Visible = true;

                    //datagridproductiondata.Columns[0].Visible = true;
                    //datagridproductiondata.Columns[1].Visible = true;
                    //datagridproductiondata.Columns[3].Visible = true;
                    //datagridproductiondata.Columns[4].Visible = false;
                    //datagridproductiondata.Columns[7].Visible = false;
                    //datagridproductiondata.Columns["Downid1"].Visible = false;
                    //datagridproductiondata.Columns["OperatorN1"].ReadOnly = true;     
                    datagridproductiondata.DataSource = dtProductionData;
                    datagridproductiondata.DataBind();
                    datagridproductiondata.Visible = true;
                }

                if (ddldatatype.SelectedValue.ToString() == "Down Data" && ddlvalidatedata.SelectedValue.ToString() == "Component-Operation")
                {

                    Session["DownData"] = dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Component-OperationInconsistencyInDown");
                    downdatagrid.Columns[6].Visible = false;
                    downdatagrid.Columns[3].Visible = true;
                    //downdatagrid.Columns[7].Visible = false;
                    //downdatagrid.Columns[3].Visible = true;
                    //downdatagrid.Columns[6].Visible = false;
                    downdatagrid.DataSource = dtDownData;
                    downdatagrid.DataBind();
                    downdatagrid.Visible = true;
                }

                if (ddldatatype.SelectedValue.ToString() == "Rejection Data" && ddlvalidatedata.SelectedValue.ToString() == "Component-Operation")
                {
                    rejectiongrd.Visible = false;
                    Session["RejectionData"] = dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Component-OperationInconsistencyInRejection");
                    //downdatagrid.Columns[7].Visible = false;
                    //downdatagrid.Columns[3].Visible = true;
                    //downdatagrid.Columns[6].Visible = false;
                    downdatagrid.DataSource = dtDownData;
                    downdatagrid.DataBind();
                    downdatagrid.Visible = true;
                }

                if (ddldatatype.SelectedValue.ToString() == "Production Data" && ddlvalidatedata.SelectedValue.ToString() == "Operator")
                {
                    Session["ProdcutionData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "OperatorInconsistencyInProd");
                    if (!(dtProductionData.Columns.Contains("DownId")))
                    {
                        dtProductionData.Columns.Add("DownId");
                    }
                    datagridproductiondata.Columns[4].Visible = false;
                    datagridproductiondata.Columns[7].Visible = false;
                    datagridproductiondata.Columns[1].Visible = false;

                    //datagridproductiondata.Columns[7].Visible = false;
                    //datagridproductiondata.Columns[1].Visible = false;
                    //datagridproductiondata.Columns[5].Visible = true;
                    //datagridproductiondata.Columns[8].Visible = false;
                    //datagridproductiondata.Columns["OperatorN1"].ReadOnly = false;               
                    datagridproductiondata.DataSource = dtProductionData;
                    datagridproductiondata.DataBind();
                    datagridproductiondata.Visible = true;

                }

                if (ddldatatype.SelectedValue.ToString() == "Down Data" && ddlvalidatedata.SelectedValue.ToString() == "Operator")
                {
                    Session["DownData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "OperatorInconsistencyInDown");
                    if (!(dtProductionData.Columns.Contains("downcode")))
                    {
                        dtProductionData.Columns.Add("downcode");
                    }
                    downdatagrid.Columns[3].Visible = false;
                    downdatagrid.Columns[6].Visible = false;
                    //downdatagrid.Columns[3].Visible = false;
                    // downdatagrid.Columns[6].Visible = false;
                    //downdatagrid.Columns[4].Visible = false;
                    //downdatagrid.Columns[7].Visible = false;
                    //downdatagrid.Columns["DownCode1"].Visible = false;
                    downdatagrid.DataSource = dtProductionData;
                    downdatagrid.DataBind();
                    downdatagrid.Visible = true;

                }

                if (ddldatatype.SelectedValue.ToString() == "Rejection Data" && ddlvalidatedata.SelectedValue.ToString() == "Operator")
                {
                    rejectiongrd.Visible = false;
                    Session["RejectionData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "OperatorInconsistencyInRejection");
                    if (!(dtProductionData.Columns.Contains("downcode")))
                    {
                        dtProductionData.Columns.Add("downcode");
                    }
                    //downdatagrid.Columns[3].Visible = false;
                    //downdatagrid.Columns[6].Visible = false;
                    ////downdatagrid.Columns[4].Visible = false;
                    //downdatagrid.Columns[7].Visible = false;
                    ////downdatagrid.Columns["DownCode1"].Visible = false;
                    downdatagrid.DataSource = dtProductionData;
                    downdatagrid.DataBind();
                    downdatagrid.Visible = true;

                }

                if (ddldatatype.SelectedValue.ToString() == "Production Data" && ddlvalidatedata.SelectedValue.ToString() == "DownID")
                {
                    Session["ProdcutionData"] = dtProductionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "downIDInconsistencyInProd");
                    datagridproductiondata.Columns[4].Visible = false;
                    datagridproductiondata.Columns[7].Visible = false;
                    datagridproductiondata.Columns[1].Visible = true;

                    //datagridproductiondata.Columns[7].Visible = false;
                    //datagridproductiondata.Columns[1].Visible = true;
                    //datagridproductiondata.Columns[3].Visible = true;
                    datagridproductiondata.DataSource = dtProductionData;
                    datagridproductiondata.DataBind();
                    datagridproductiondata.Visible = true;

                }

                if (ddldatatype.SelectedValue.ToString() == "Down Data" && ddlvalidatedata.SelectedValue.ToString() == "DownID")
                {

                    Session["DownData"] = dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "downIDInconsistencyInDown");
                    //if (dtDownData.Columns.Contains("DownId"))
                    //{
                    //    dtDownData.Columns["DownId"].ColumnName = "downcode";
                    //}
                    downdatagrid.Columns[6].Visible = false;
                    downdatagrid.Columns[3].Visible = true;

                    //downdatagrid.Columns[0].Visible = false;
                    //downdatagrid.Columns[1].Visible = true;
                    //downdatagrid.Columns[3].Visible = true;
                    downdatagrid.DataSource = dtDownData;
                    downdatagrid.DataBind();
                    downdatagrid.Visible = true;

                }
                if (ddldatatype.SelectedValue.ToString() == "Rejection Data" && ddlvalidatedata.SelectedValue.ToString() == "DownID")
                {
                    rejectiongrd.Visible = false;
                    Session["RejectionData"]=dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "downIDInconsistencyInRejection");
                    if (dtDownData.Columns.Contains("DownId"))
                    {
                        dtDownData.Columns["DownId"].ColumnName = "downcode";
                    }
                    //downdatagrid.Columns[0].Visible = false;
                    //downdatagrid.Columns[1].Visible = true;
                    //downdatagrid.Columns[3].Visible = true;
                    downdatagrid.DataSource = dtDownData;
                    downdatagrid.DataBind();
                    downdatagrid.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error - \n" + ex.ToString());
                lblMessages.Text = ex.Message;
                throw;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "hideloader", "HideLoader()", true);
        }
        #endregion

        protected void downdatagrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                commanDataBind();
                HiddenField hdfcomponent = (e.Row.FindControl("hdfcomponentname") as HiddenField);
                DropDownList componentname = (e.Row.FindControl("ComponentName") as DropDownList);
                //componentname.DataSource = ComponentBusinessID;
                componentname.DataSource = componentWithInterfaceID;
                componentname.DataTextField = "InterfaceIWithID";
                componentname.DataValueField = "InterfaceID";
                componentname.DataBind();
                if (componentname.Items.FindByValue(hdfcomponent.Value) == null)
                {
                    componentname.Items.Add(new ListItem(hdfcomponent.Value, hdfcomponent.Value));
                }
                componentname.SelectedValue = hdfcomponent.Value;


                HiddenField hdfopr = (e.Row.FindControl("hdfoperator") as HiddenField);
                DropDownList operatorname = (e.Row.FindControl("OperatorName") as DropDownList);
                //operatorname.DataSource = OperatorBusinessID;
                operatorname.DataSource = operatorWithInterfaceID;
                operatorname.DataTextField = "InterfaceIWithID";
                operatorname.DataValueField = "InterfaceID";
                operatorname.DataBind();
                if (operatorname.Items.FindByValue(hdfopr.Value) == null)
                {
                    operatorname.Items.Add(new ListItem(hdfopr.Value, hdfopr.Value));
                }
                operatorname.SelectedValue = hdfopr.Value;

                HiddenField hdfdowncode = (e.Row.FindControl("hdfdowncode") as HiddenField);
                DropDownList ddldowncode = (e.Row.FindControl("DownCode") as DropDownList);
               // ddldowncode.DataSource = downcode;
                ddldowncode.DataSource = downcodeWithInterfaceID;
                ddldowncode.DataTextField = "InterfaceIWithID";
                ddldowncode.DataValueField = "InterfaceID";
                ddldowncode.DataBind();
                if (ddldowncode.Items.FindByValue(hdfdowncode.Value) == null)
                {
                    ddldowncode.Items.Add(new ListItem(hdfdowncode.Value, hdfdowncode.Value));
                }
                ddldowncode.SelectedValue = hdfdowncode.Value;
            }
        }

        protected void datagridproductiondata_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                commanDataBind();
                HiddenField hdfcomponent = (e.Row.FindControl("hdfcomponent") as HiddenField);
                DropDownList componentname = (e.Row.FindControl("Component") as DropDownList);
                //componentname.DataSource = ComponentBusinessID;
                componentname.DataSource = componentWithInterfaceID;
                componentname.DataTextField = "InterfaceIWithID";
                componentname.DataValueField = "InterfaceID";
                componentname.DataBind();
                if (componentname.Items.FindByValue(hdfcomponent.Value) == null)
                {
                    componentname.Items.Add(new ListItem(hdfcomponent.Value, hdfcomponent.Value));
                }
                componentname.SelectedValue = hdfcomponent.Value;

                HiddenField hdfopr = (e.Row.FindControl("hdfoperator") as HiddenField);
                DropDownList operatorname = (e.Row.FindControl("Operator") as DropDownList);
                //operatorname.DataSource = OperatorBusinessID;
                operatorname.DataSource = operatorWithInterfaceID;
                operatorname.DataTextField = "InterfaceIWithID";
                operatorname.DataValueField = "InterfaceID";
                operatorname.DataBind();
                if (operatorname.Items.FindByValue(hdfopr.Value) == null)
                {
                    operatorname.Items.Add(new ListItem(hdfopr.Value, hdfopr.Value));
                }
                operatorname.SelectedValue = hdfopr.Value;
            }
        }

        protected void datagridproductiondata_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            datagridproductiondata.PageIndex = e.NewPageIndex;
            BindProductionData();
        }

        protected void rejectiongrd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                commanDataBind();
                HiddenField hdfcomponent = (e.Row.FindControl("hdfcomponentrej") as HiddenField);
                DropDownList componentname = (e.Row.FindControl("comprejection") as DropDownList);
              //  componentname.DataSource = ComponentBusinessID;
                componentname.DataSource = componentWithInterfaceID;
                componentname.DataTextField = "InterfaceIWithID";
                componentname.DataValueField = "InterfaceID";
                componentname.DataBind();
                if (componentname.Items.FindByValue(hdfcomponent.Value) == null)
                {
                    componentname.Items.Add(new ListItem(hdfcomponent.Value, hdfcomponent.Value));
                }
                componentname.SelectedValue = hdfcomponent.Value;

                HiddenField hdfopr = (e.Row.FindControl("hdfoperatorrej") as HiddenField);
                DropDownList operatorname = (e.Row.FindControl("Operator") as DropDownList);
                // operatorname.DataSource = OperatorBusinessID;
                operatorname.DataSource = operatorWithInterfaceID;
                operatorname.DataTextField = "InterfaceIWithID";
                operatorname.DataValueField = "InterfaceID";
                operatorname.DataBind();
                if (operatorname.Items.FindByValue(hdfopr.Value) == null)
                {
                    operatorname.Items.Add(new ListItem(hdfopr.Value, hdfopr.Value));
                }
                operatorname.SelectedValue = hdfopr.Value;
            }
        }

        #region Save All data grid
        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlplant.Text.ToString())) return;
            if (string.IsNullOrEmpty(ddlmachine.Text.ToString())) return;
            try
            {
                bool IsUpdated = false;int DataType = 0;
                //DataTable dt1 = new DataTable();
                if (ddldatatype.Text.ToString().Equals("Production Data"))
                {
                    getdatatype(ddldatatype.SelectedValue.ToString(), out DataType);
                    foreach (GridViewRow row in datagridproductiondata.Rows)
                    {
                        HiddenField hdfvalue = (HiddenField)row.FindControl("hdfprodsavecheck");
                        if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                        {
                            string grdid = ((System.Web.UI.WebControls.Label)row.FindControl("id")).Text;
                            string fromComponent = ((System.Web.UI.WebControls.Label)row.FindControl("lbComponent")).Text;
                            string grdcomponent = ((DropDownList)row.FindControl("Component")).SelectedValue;
                            string grdoperation = ((System.Web.UI.WebControls.TextBox)row.FindControl("Operation")).Text;
                            string fromOperation = ((System.Web.UI.WebControls.Label)row.FindControl("lbOperation")).Text;
                            string grdDownID = ((System.Web.UI.WebControls.TextBox)row.FindControl("Downid")).Text;
                            string fromOperator = ((System.Web.UI.WebControls.Label)row.FindControl("lbOperatorInterfaceid")).Text;
                            string grdoperator = ((DropDownList)row.FindControl("Operator")).SelectedValue;
                            string grdpartscount = ((System.Web.UI.WebControls.Label)row.FindControl("PartsCount")).Text;
                            string grdstartTime = ((System.Web.UI.WebControls.Label)row.FindControl("TimeFrom")).Text;
                            string grdendTime = ((System.Web.UI.WebControls.Label)row.FindControl("TimeTo")).Text;
                            string grdworkorderno = ((System.Web.UI.WebControls.Label)row.FindControl("WorkOrderNo")).Text;
                            getdatatype(ddldatatype.Text.ToString(), out int datatype);
                            bool isEnabled = DataBaseAccess.GetProductionDownLog(datatype);
                            bool isUpdated = false;
                            if (isEnabled)
                            {
                                //TimeSpan difference = Ndtime - Sttime;
                                //if (difference.Days > 1)
                                //{
                                //    lblMessages.Text = "Difference between dates can not be more than 1 day";
                                //    return;
                                //}
                                DataBaseAccess.UpdateProductionDownRejectionData_NotBulk(grdstartTime, Convert.ToInt32(grdid), DateTime.Now, DateTime.Now, ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "", fromComponent, grdcomponent, fromOperation, grdoperation,fromOperator,grdoperator, Convert.ToDecimal(grdpartscount), 0, "", "", DataType, "", "0", out IsUpdated, Session["UserName"].ToString());
                            }
                            DataBaseAccess.UpdateProdDownRejectiondata(Convert.ToInt32(grdid), DateTime.Now, DateTime.Now, "", grdcomponent, "", grdoperation, "", grdoperator, "",
                             grdworkorderno, "", Convert.ToDecimal(grdpartscount), 0, "", "", "", "", "", "", ddldatatype.Text.ToString(), "GridProductionData","0", out IsUpdated);
                        }
                    }
                }
                else if (ddldatatype.Text.ToString().Equals("Down Data"))
                {
                    getdatatype(ddldatatype.SelectedValue.ToString(), out DataType);
                    foreach (GridViewRow row in downdatagrid.Rows)
                    {
                        HiddenField hdfvalue = (HiddenField)row.FindControl("hdfdownsavecheck");
                        if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                        {
                            string grdid = ((System.Web.UI.WebControls.Label)row.FindControl("id")).Text;
                            string grdcomponent = ((DropDownList)row.FindControl("ComponentName")).SelectedValue;
                            string grdoperation = ((System.Web.UI.WebControls.TextBox)row.FindControl("OperationID")).Text;
                            string grdoperator = ((DropDownList)row.FindControl("OperatorName")).SelectedValue;
                            string grdDowncode = ((DropDownList)row.FindControl("DownCode")).SelectedValue;

                            //string grdpartscount = ((DropDownList)row.FindControl("PartsCount")).SelectedValue;
                            string grdownfromtime = ((System.Web.UI.WebControls.Label)row.FindControl("downFromTime")).Text;
                            string grddowntoTime = ((System.Web.UI.WebControls.Label)row.FindControl("downToTime")).Text;
                            string grdworkorderno = ((System.Web.UI.WebControls.Label)row.FindControl("workorderno")).Text;
                            getdatatype(ddldatatype.Text.ToString(), out int datatype);
                            bool isEnabled = DataBaseAccess.GetProductionDownLog(datatype);
                            string fromComponent = ((System.Web.UI.WebControls.Label)row.FindControl("lbComponent")).Text;
                            string fromOperation = ((System.Web.UI.WebControls.Label)row.FindControl("lbOperation")).Text;
                            string fromOperator = ((System.Web.UI.WebControls.Label)row.FindControl("lbOperatorInterfaceid")).Text;
                            string fromDownCode = ((System.Web.UI.WebControls.Label)row.FindControl("lbDownId")).Text;
                            bool isUpdated = false;
                            if (isEnabled)
                            {
                                //TimeSpan difference = Ndtime - Sttime;
                                //if (difference.Days > 1)
                                //{
                                //    lblMessages.Text = "Difference between dates can not be more than 1 day";
                                //    return;
                                //}
                                DataBaseAccess.UpdateProductionDownRejectionData_NotBulk(grdownfromtime,Convert.ToInt32(grdid), DateTime.Now, DateTime.Now, ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "", fromComponent, grdcomponent, fromOperation, grdoperation,fromOperator,grdoperator ,0, 0, fromDownCode, grdDowncode, DataType, "", "0", out IsUpdated, Session["UserName"].ToString());
                            }
                            DataBaseAccess.UpdateProdDownRejectiondata(Convert.ToInt32(grdid), DateTime.Now, DateTime.Now, "", grdcomponent, "", grdoperation, "", grdoperator, "",
                               grdworkorderno, "", 0, 0, "", grdDowncode, "", "", "", "", ddldatatype.Text.ToString(), "GridDownData","0", out IsUpdated);
                        }
                    }
                }
                else if (ddldatatype.Text.ToString().Equals("Rejection Data"))
                {
                    getdatatype(ddldatatype.SelectedValue.ToString(), out DataType);
                    foreach (GridViewRow row in rejectiongrd.Rows)
                    {
                        HiddenField hdfvalue = (HiddenField)row.FindControl("hdfdownsavecheck");
                        if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                        {
                            string grdid = ((System.Web.UI.WebControls.Label)row.FindControl("id")).Text;
                            string grdmc = ((System.Web.UI.WebControls.Label)row.FindControl("mc")).Text;
                            string grdcomponent = ((DropDownList)row.FindControl("comprejection")).SelectedValue;
                            string grdoperation = ((System.Web.UI.WebControls.TextBox)row.FindControl("operation")).Text;
                            string grdoperator = ((DropDownList)row.FindControl("Operator")).SelectedValue;
                            string grdDowncode = ((System.Web.UI.WebControls.TextBox)row.FindControl("RejectionCodeID")).Text;
                            string grdrejqty = ((System.Web.UI.WebControls.TextBox)row.FindControl("RejectionQty")).Text;
                            string grdTimeStamp = ((System.Web.UI.WebControls.TextBox)row.FindControl("CreatedTs")).Text;
                            string grdrejdate = ((System.Web.UI.WebControls.TextBox)row.FindControl("RejectionDate")).Text;
                            string RejectionShift = ((System.Web.UI.WebControls.TextBox)row.FindControl("RejectionShift")).Text;
                            //DataBaseAccess.UpdateProductionDownRejectionData_NotBulk("",Convert.ToInt32(grdid), DateTime.Now, DateTime.Now, ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "", grdcomponent, "", grdoperation, "",grdoperator,"", 0, 0, "", "", DataType, "GridRejectionData", grdrejqty, out IsUpdated, Session["UserName"].ToString());

                            DataBaseAccess.UpdateProdDownRejectiondata(Convert.ToInt32(grdid), DateTime.Now, DateTime.Now, grdmc, grdcomponent, "", grdoperation, "", grdoperator, "",
                               "", "", 0, 0, "", "", "", "", "", "", ddldatatype.Text.ToString(), "GridRejectionData", grdrejqty, out IsUpdated);
                        }
                    }
                }
                if (IsUpdated)
                {
                    // GridRefresh();
                    GetTabInformation();
                    if (isNormalView())
                    {
                        getGriddata();
                    }
                    else
                    {
                        btn_Inconsistency_Click(null, null);
                    }
                    lblMessages.Text = "Successfully Updated Record";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
                throw;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loaderhide", "HideLoader()", true);
        }
        #endregion

        protected void workupdate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Sttime = DateTime.Now; int Datatype = 0;
                DateTime Ndtime = DateTime.Now;
                if (ddlfrm.SelectedValue != null && toWorkOrder != null && !(string.IsNullOrEmpty(toWorkOrder.Text)) && ddlmachine.SelectedValue != null && !string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
                {
                    string OldWorkOrder = ddlfrm.SelectedValue.ToString();
                    string NewWorkOrder = toWorkOrder.Text.ToString();
                    string MachineID = ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "";
                    DateTime.TryParse(txtFromDate.Text.ToString(), out Sttime);
                    DateTime.TryParse(txtToDate.Text.ToString(), out Ndtime);
                    getdatatype(ddldatatype.SelectedValue.ToString(), out Datatype);
                    int rowCount;
                    //getdatatype(ddldatatype.Text.ToString(), out int datatype);
                    bool isEnabled = DataBaseAccess.GetProductionDownLog(Datatype);
                    bool isUpdated = false;
                    if (isEnabled)
                    {
                        TimeSpan difference = Ndtime - Sttime;
                        if (difference.Days > 1)
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Difference between dates can not be more than 1 day");
                            //lblMessages.Text = "Difference between dates can not be more than 1 day";
                            return;
                        }
                        DataBaseAccess.UpdateModifiedData_Bulk(Sttime, Ndtime, MachineID, "", "", "", "", "", "", OldWorkOrder, NewWorkOrder, 0, 0, "", "", "", "", "", "", Datatype, "WorkOrderNumber", "", out isUpdated, Session["UserName"].ToString());
                    }
                    bool Status = DataBaseAccess.UpdateModifiedWorkOrderData(OldWorkOrder, NewWorkOrder, MachineID, Sttime, Ndtime, Datatype, out rowCount);
                    if (Status)
                    {
                        lblMessages.Text = rowCount + " Row Updated Successfully.";
                        GetTabInformation();
                    }
                    //GridRefresh();
                    if (isNormalView())
                    {
                        getGriddata();
                    }
                    else
                    {
                        btn_Inconsistency_Click(null, null);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void rejupdate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Sttime = DateTime.Now; int Datatype = 0;
                DateTime Ndtime = DateTime.Now;
                if (ddlrejfrom.SelectedValue != null && ddlrejto.SelectedValue != null && ddlmachine.SelectedValue != null && !string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text) && ddldatatype.SelectedValue != null && ddldatatype.SelectedValue.ToString().Equals("Rejection Data", StringComparison.OrdinalIgnoreCase))
                {
                    string oldRejectionCode = ddlrejfrom.SelectedValue.ToString();
                    string newRejectionCode = ddlrejto.SelectedValue.ToString().Split('<')[0].Trim(); ;
                    string MachineID = ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "";
                    DateTime.TryParse(txtFromDate.Text.ToString(), out Sttime);
                    DateTime.TryParse(txtToDate.Text.ToString(), out Ndtime);
                    getdatatype(ddldatatype.SelectedValue.ToString(), out Datatype);
                    DateTime.TryParse(txtFromDate.Text.ToString(), out Sttime);
                    DateTime.TryParse(txtToDate.Text.ToString(), out Ndtime);
                    int rowCount;
                    bool isUpdated = false;
                    bool Status = DataBaseAccess.UpdateModifiedRejectionData(oldRejectionCode, newRejectionCode, MachineID, Sttime, Ndtime, Datatype, out rowCount);
                    if (Status)
                    {
                        lblMessages.Text = rowCount + " Row Updated Successfully.";
                        GetTabInformation();
                        //GridRefresh();
                        if (isNormalView())
                        {
                            getGriddata();
                        }
                        else
                        {
                            btn_Inconsistency_Click(null, null);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void compupdate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Sttime = DateTime.Now; int Datatype = 0;
                DateTime Ndtime = DateTime.Now;
                if (ddlfrmcomponent.SelectedValue != null && ddlfrmoperation.SelectedValue != null && ddltocomponent.SelectedValue != null && ddlopreration_to.SelectedValue != null && ddlmachine.SelectedValue != null && !string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text) && ddldatatype.SelectedValue != null)
                {
                    string OldComponentValue = ddlfrmcomponent.SelectedValue.ToString();
                    string NewComponentValue = ddltocomponent.SelectedValue.ToString().Split('<')[0].Trim();
                    string OldOperationValue = ddlfrmoperation.SelectedValue.ToString();
                    string NewOperationValue = ddlopreration_to.SelectedValue.ToString().Split('<')[0].Trim();
                    string MachineID = ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "";
                    getdatatype(ddldatatype.SelectedValue.ToString(), out Datatype);
                    DateTime.TryParse(txtFromDate.Text.ToString(), out Sttime);
                    DateTime.TryParse(txtToDate.Text.ToString(), out Ndtime);
                    bool isUpdated = false;
                    //getdatatype(ddldatatype.Text.ToString(), out int datatype);
                    bool isEnabled = DataBaseAccess.GetProductionDownLog(Datatype);
                    if (isEnabled)
                    {
                        TimeSpan difference = Ndtime - Sttime;
                        if (difference.Days > 1)
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Difference between dates can not be more than 1 day");
                            //lblMessages.Text = "Difference between dates can not be more than 1 day";
                            return;
                        }
                        DataBaseAccess.UpdateModifiedData_Bulk(Sttime, Ndtime, MachineID, OldComponentValue, NewComponentValue, OldOperationValue, NewOperationValue, "", "", "", "", 0, 0, "", "", "", "", "", "", Datatype, "ComponentOperation", "", out isUpdated, Session["UserName"].ToString());
                    }
                    int rowCount;
                    bool Status = DataBaseAccess.UpdateModifiedComponentData(OldComponentValue, OldOperationValue, NewComponentValue, NewOperationValue, MachineID, Sttime, Ndtime, Datatype, out rowCount);
                    if (Status)
                    {
                        lblMessages.Text = rowCount + " Row Updated Successfully.";
                        GetTabInformation();
                        //GridRefresh();
                        if (isNormalView())
                        {
                            getGriddata();
                        }
                        else
                        {
                            btn_Inconsistency_Click(null, null);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void oprupdate_Click(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "loader", "ShowLoader()", true);
                DateTime Sttime = DateTime.Now; int Datatype = 0;
                DateTime Ndtime = DateTime.Now;
                if (ddlfrmorp.SelectedValue != null && ddlopr_to.SelectedValue != null && ddlmachine.SelectedValue != null && !string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
                {

                    string OldOperatorValue = ddlfrmorp.SelectedValue.ToString();
                    string NewOperatorValue = ddlopr_to.SelectedValue.ToString().Split('<')[0].Trim();
                    string MachineID = ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "";
                    DateTime.TryParse(txtFromDate.Text.ToString(), out Sttime);
                    DateTime.TryParse(txtToDate.Text.ToString(), out Ndtime);
                    getdatatype(ddldatatype.SelectedValue.ToString(), out Datatype);
                    bool isUpdated = false;
                    bool isEnabled = DataBaseAccess.GetProductionDownLog(Datatype);
                    if (isEnabled)
                    {
                        TimeSpan difference = Ndtime - Sttime;
                        if (difference.Days > 1)
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Difference between dates can not be more than 1 day");
                            //lblMessages.Text = "Difference between dates can not be more than 1 day";
                            return;
                        }
                        DataBaseAccess.UpdateModifiedData_Bulk(Sttime, Ndtime, MachineID, "", "", "", "", OldOperatorValue, NewOperatorValue, "", "", 0, 0, "", "", "", "", "", "", Datatype, "opr", "", out isUpdated, Session["UserName"].ToString());
                    }
                    int rowCount;
                    bool Status = DataBaseAccess.UpdateModifiedOperatorData(OldOperatorValue, NewOperatorValue, MachineID, Sttime, Ndtime, Datatype, out rowCount);
                    if (Status)
                    {
                        lblMessages.Text = rowCount + " Row Updated Successfully.";
                        GetTabInformation();
                    }
                    // GridRefresh();
                    if (isNormalView())
                    {
                        getGriddata();
                    }
                    else
                    {
                        btn_Inconsistency_Click(null, null);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void downupdate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Sttime = DateTime.Now; int Datatype = 0;
                DateTime Ndtime = DateTime.Now;
                if (ddlfrmdown.SelectedValue != null && ddldown_to.SelectedValue != null && ddlmachine.SelectedValue != null && !string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text) && ddldatatype.SelectedValue.ToString().Equals("Down Data", StringComparison.OrdinalIgnoreCase))
                {
                    string OldDownValue = ddlfrmdown.SelectedValue.ToString();
                    string NewDownValue = ddldown_to.SelectedValue.ToString().Split('<')[0].Trim();
                    string MachineID = ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "";
                    DateTime.TryParse(txtFromDate.Text.ToString(), out Sttime);
                    DateTime.TryParse(txtToDate.Text.ToString(), out Ndtime);
                    getdatatype(ddldatatype.SelectedValue.ToString(), out Datatype);
                    bool isEnabled = DataBaseAccess.GetProductionDownLog(Datatype);
                    bool isUpdated = false;
                    if (isEnabled)
                    {
                        TimeSpan difference = Ndtime - Sttime;
                        if (difference.Days > 1)
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Difference between dates can not be more than 1 day");
                            //lblMessages.Text = "Difference between dates can not be more than 1 day";
                            return;
                        }
                        DataBaseAccess.UpdateModifiedData_Bulk(Sttime, Ndtime, MachineID, "", "", "", "", "", "", "", "", 0, 0, "", OldDownValue, NewDownValue, "", "", "", Datatype, "dcode", "", out isUpdated, Session["UserName"].ToString());
                    }
                    int rowCount;
                    bool Status = DataBaseAccess.UpdateModifiedDownData(OldDownValue, NewDownValue, MachineID, Sttime, Ndtime, Datatype, out rowCount);
                    if (Status)
                    {
                        lblMessages.Text = rowCount + " Row Updated Successfully.";
                        GetTabInformation();
                        // GridRefresh();
                        if (isNormalView())
                        {
                            getGriddata();
                        }
                        else
                        {
                            btn_Inconsistency_Click(null, null);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void Partsupdate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Sttime = DateTime.Now; int Datatype = 0; double OldPartCount = 0; double NewPartCount = 0;
                DateTime Ndtime = DateTime.Now;
                if (ddlpartcomp.SelectedValue != null && ddlfrmcount.SelectedValue != null && ddlpatsopr.SelectedValue != null && ddlpartstocount.Text != null && ddlmachine.SelectedValue != null && !string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text) && ddldatatype.SelectedValue != null && ddldatatype.SelectedValue.ToString().Equals("Production Data", StringComparison.OrdinalIgnoreCase))
                {
                    string Component = ddlpartcomp.SelectedValue.ToString();
                    double.TryParse(ddlfrmcount.SelectedValue.ToString(), out OldPartCount);
                    string Operation = ddlpatsopr.SelectedValue.ToString();
                    double.TryParse(ddlpartstocount.Text.ToString(), out NewPartCount);
                    string MachineID = ddlmachine.SelectedItem != null ? ddlmachine.Text.Split('<')[0].Trim() : "";
                    //string[] mac = MachineID.Split('<');
                    DateTime.TryParse(txtFromDate.Text.ToString(), out Sttime);
                    DateTime.TryParse(txtToDate.Text.ToString(), out Ndtime);
                    getdatatype(ddldatatype.SelectedValue.ToString(), out Datatype);
                    bool isEnabled = DataBaseAccess.GetProductionDownLog(Datatype);
                    bool isUpdated = false;
                    if (isEnabled)
                    {
                        TimeSpan difference = Ndtime - Sttime;
                        if (difference.Days > 1)
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Difference between dates can not be more than 1 day");
                            //lblMessages.Text = "Difference between dates can not be more than 1 day";
                            return;
                        }
                        DataBaseAccess.UpdateModifiedData_Bulk(Sttime, Ndtime, MachineID, "", "", "", "", "", "", "", "", Convert.ToDecimal(OldPartCount), Convert.ToDecimal(NewPartCount), "", "", "", "", "", "", Datatype, "PartsCount", "", out isUpdated, Session["UserName"].ToString());
                    }
                    int rowCount;
                    bool Status = DataBaseAccess.UpdateModifiedPartCountData(Component, Operation, MachineID, Sttime, Ndtime, OldPartCount, NewPartCount, Datatype, out rowCount);
                    if (Status)
                    {
                        lblMessages.Text = rowCount + " Row Updated Successfully.";
                        GetTabInformation();
                        //GridRefresh();
                        if (isNormalView())
                        {
                            getGriddata();
                        }
                        else
                        {
                            btn_Inconsistency_Click(null, null);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void getdatatype(string type, out int datatype)
        {
            datatype = 1;
            switch (type)
            {
                case "Production Data":
                    datatype = 1;
                    break;
                case "Down Data":
                    datatype = 2;
                    break;
                case "Rejection Data":
                    datatype = 3;
                    break;
            }
        }
        private bool isNormalView()
        {
            if (hdnViewValue.Value == "InconsistencyView")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected void downdatagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            downdatagrid.PageIndex = e.NewPageIndex;
            BindDownData();
        }

        private void BindDownData()
        {
            DataTable dtDownData = new DataTable();
            StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
            EndTime = Util.GetDateTime(txtToDate.Text.ToString());
            if (Session["DownData"] == null)
                Session["DownData"] = dtDownData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Down Data");
            else
                dtDownData = Session["DownData"] as DataTable;
            downdatagrid.DataSource = dtDownData;
            downdatagrid.DataBind();
            downdatagrid.Visible = true;
            rejectiongrd.Visible = false;
            datagridproductiondata.Visible = false;
        }

        protected void rejectiongrd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            rejectiongrd.PageIndex = e.NewPageIndex;
            BindRejectionData();
        }

        private void BindRejectionData()
        {
            DataTable dtRejectionData = new DataTable();
            StartTime = Util.GetDateTime(txtFromDate.Text.ToString());
            EndTime = Util.GetDateTime(txtToDate.Text.ToString());
            if (Session["RejectionData"] == null)
                Session["RejectionData"] = dtRejectionData = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Rejection Data");
            else
                dtRejectionData = Session["RejectionData"] as DataTable;
            rejectiongrd.DataSource = dtRejectionData;
            rejectiongrd.DataBind();
            downdatagrid.Visible = false;
            rejectiongrd.Visible = true;
            datagridproductiondata.Visible = false;
        }

        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineId();
        }

        protected void ddlpartcomp_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOperation();
            BindPartsCount();
        }
        protected void ddlfrmcomponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCompOpn();
        }

        protected void ddltocomponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MachineID = ddlmachine.Text.ToString().Split('<');
                MachineName = MachineID[1].ToString().Trim('>');
                StartTime = Util.GetDateTime(txtFromDate.Text);
                EndTime = Util.GetDateTime(txtToDate.Text);
                ComponentID = ddltocomponent.Text.ToString().Split('<');
                CompInterfaceID = ComponentID[0].ToString().Trim();
                ddlopreration_to.DataSource = DataBaseAccess.GetProdDownRejectiondata(StartTime, EndTime, ddlplant.Text.ToString(), MachineName.ToString(), CompInterfaceID.ToString(), "", "", "", "", "Operation");
                ddlopreration_to.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlpatsopr_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPartsCount();
        }

        protected void btnReloadBtn_Click(object sender, EventArgs e)
        {
            if (isNormalView())
            {
                getGriddata();
            }
            else
            {
                btn_Inconsistency_Click(null, null);
            }
        }
        protected void ddldatatype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddldatatype.SelectedValue == "Production Data" || ddldatatype.SelectedValue == "Rejection Data")
            {
                ddlvalidatedata.Items.Clear();
                ddlvalidatedata.Items.Add("Component-Operation");
                ddlvalidatedata.Items.Add("Operator");
            }
            else
            {
                ddlvalidatedata.Items.Clear();
                ddlvalidatedata.Items.Add("Component-Operation");
                ddlvalidatedata.Items.Add("Operator");
                ddlvalidatedata.Items.Add("DownID");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool GetProductionDownLogs()
        {
            bool isProductionDownLogEnabled = false;
            bool isDownLogEnabled = false;
            bool isProductionLogEnabled = false;
            try
            {
                isProductionLogEnabled= DataBaseAccess.GetProductionDownLog(1);
                isDownLogEnabled= DataBaseAccess.GetProductionDownLog(2);
                if (isProductionLogEnabled || isDownLogEnabled)
                {
                    isProductionDownLogEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return isProductionDownLogEnabled;
        }
    }

    public class ModifiedDataEntity
    {
        public string InterfaceID { get; set; }
        public string ID { get; set; }
        public string InterfaceIWithID { get; set; }
    }
    
}




