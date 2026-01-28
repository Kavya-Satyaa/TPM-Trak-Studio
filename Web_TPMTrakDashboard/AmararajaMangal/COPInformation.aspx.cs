using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AmararajaMangal
{
    public partial class COPInformation : System.Web.UI.Page
    {
        int RowIndex = 0;
        int FinishOperation = 0;
        string TimeFormatVal = string.Empty;//string.Empty;
        List<string> allMachines = DataBaseAccess.GetAllMacForPlant("");
        string componetid = string.Empty;
        string intefaceID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                componetid = Request.QueryString["name"].ToString();
                Session["intefaceID"] = intefaceID = Request.QueryString["intefaceID"].ToString();
                ComponentInfogrd(componetid);
                if (!DataBaseAccess.GetStatusOfSubOperator())
                {
                    txtsubop.Text = "1";
                    txtsubop.Enabled = false;
                }
                if (WebConfigurationManager.AppSettings["FinishedComponent"].Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    chkFinishOperation.Visible = true;
                    lblFinishOpr.Visible = true;
                    componentgrd.Columns[11].Visible = true;
                }
                else
                {
                    chkFinishOperation.Visible = false;
                    lblFinishOpr.Visible = false;
                    componentgrd.Columns[11].Visible = false;
                }
            }
            // btnUpdateStdTime.Visible = false;
        }
      
        #region -------------Grid Binding---------------
        public void ComponentInfogrd(string componentId)
        {
            txtsearch.Text = componentId.ToString();
            DataTable dt = DataBaseAccess.GetComponentDetails(componentId, "", "ViewCompOpnInfo");
            componentgrd.DataSource = dt;
            componentgrd.DataBind();

            //onsorting purpose
            Session["dirState"] = dt;
            Session["sortdr"] = "Asc";

            //end sorting 
            SetTimeFormat();
            ComponentInformation_Shown();
            DataTable componentDt = DataBaseAccess.GetAllComponentz();
            if (componentDt != null && componentDt.Rows.Count > 0)
            {
                ComponentIdgrd.DataSource = componentDt;
                ComponentIdgrd.DataBind();
                DataTable dt1 = componentgrd.DataSource as DataTable;
                if (dt1 != null)
                {
                    SetValuesToControls(dt1, 0);
                }
                else
                {
                    btnnew_Click(null, EventArgs.Empty);
                }
            }
        }

        private void SetTimeFormat()
        {
            TimeFormatVal = DataBaseAccess.GetTimeFormat();
            if (TimeFormatVal == "ss")
            {
                lgStandartTime.InnerText = GetLocalResourceObject("StdTimeSec").ToString();
                lblstdTime.Text = GetLocalResourceObject("StdSetupSec").ToString();
            }
            else if (TimeFormatVal == "mm")
            {
                lgStandartTime.InnerText = GetLocalResourceObject("StdTimeMin").ToString();
                lblstdTime.Text = GetLocalResourceObject("StdSetupMin").ToString();
            }
            else
            {
                lgStandartTime.InnerText = GetLocalResourceObject("StdTimeSec").ToString();
                lblstdTime.Text = GetLocalResourceObject("StdSetupSec").ToString();
            }
        }
        #endregion

        #region -------------Plant & Machine---------------
        private void ComponentInformation_Shown()
        {
            List<string> allPlants = DataBaseAccess.GetAllPlants();

            if (allPlants != null && allPlants.Count > 0)
            {
                ddlPlantId.DataSource = allPlants;
                ddlPlantId.DataBind();
                allMachines = DataBaseAccess.GetAllMacForPlant(ddlPlantId.Text);
                if (allMachines != null && allMachines.Count > 0)
                {
                    ddlMultiDownID.DataSource = allMachines;
                    ddlMultiDownID.DataBind();
                }
            }
        }
        #endregion

        #region -------------Save and setValue from textbox ---------------
        private void SetValuesToControls(DataTable dt, int index)
        {
            double standardLoad = 0.0;
            double standardCycleTime = 0.0;
            Int64 loadUnloadVal = 0;
            if (dt == null || dt.Rows.Count < 0) return;
            dt.Columns["CycleTime"].ReadOnly = false;
            dt.Columns["LoadUnload"].ReadOnly = false;
            dt.Columns["MachiningTime"].ReadOnly = false;
            allMachines = DataBaseAccess.GetAllMacForPlant("");
            if (allMachines != null && allMachines.Count > 0)
            {
                ddlMultiDownID.DataSource = allMachines;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GetStdCycleAndLoadUnloadTimeForGrid(dt.Rows[i]["Machineid"].ToString(), dt.Rows[i]["Componentid"].ToString(), dt.Rows[i]["OperationNo"].ToString(), out standardLoad, out standardCycleTime, out loadUnloadVal);
                dt.Rows[i]["MachiningTime"] = standardCycleTime.ToString();
                dt.Rows[i]["LoadUnload"] = standardLoad.ToString();

                var val = standardCycleTime + standardLoad;
                dt.Rows[i]["CycleTime"] = val.ToString();
                dt.Rows[i]["LoadUnloadThreshold"] = loadUnloadVal.ToString();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                btnassign.Visible = true;
                btnUpdateStdTime.Visible = true;
                RowIndex = index;
                txtopn_no.Text = dt.Rows[RowIndex]["OperationNo"].ToString();
                txtdrawing.Text = dt.Rows[RowIndex]["DrawingNo"].ToString();
                txtdescription.Text = dt.Rows[RowIndex]["Description"].ToString();
                txtsubop.Text = dt.Rows[RowIndex]["SubOperations"].ToString();
                txtinterfaceid.Text = dt.Rows[RowIndex]["InterfaceID"].ToString();
                txttarget.Text = dt.Rows[RowIndex]["TargetPercent"].ToString();
                txtprice.Text = dt.Rows[RowIndex]["Price"].ToString();
                ddlMultiDownID.SelectedValue = dt.Rows[RowIndex]["machineid"].ToString();
                txtmachinetime.Text = dt.Rows[RowIndex]["MachiningTime"].ToString();
                txtCycleTime.Text = dt.Rows[RowIndex]["CycleTime"].ToString();
                txtloadunload.Text = dt.Rows[RowIndex]["LoadUnload"].ToString();
                txtsetuptime.Text = dt.Rows[RowIndex]["StdSetUpTime"].ToString();
                txtloadunloadthreshold.Text = dt.Rows[RowIndex]["LoadUnloadThreshold"].ToString();
                txtcuttingtime.Text = dt.Rows[RowIndex]["MachiningTimeThreshold"].ToString();
                txtminLoadUnloadThreshold.Text = dt.Rows[RowIndex]["MinLoadUnloadThreshold"].ToString();
                txtPartPerCycle.Text = dt.Rows[RowIndex]["PartsPerCycle"].ToString();
                txtStrokesPerPart.Text = dt.Rows[RowIndex]["StrokesPerPart"].ToString();
                txtWeight.Text = dt.Rows[RowIndex]["Weight"].ToString();
                txtThickness.Text = dt.Rows[RowIndex]["Thickness"].ToString();
                if (ddlGrade.Items.FindByValue(dt.Rows[RowIndex]["Grade"].ToString()) != null)
                {
                    ddlGrade.SelectedValue = dt.Rows[RowIndex]["Grade"].ToString();
                }
                else
                {
                    if(ddlGrade.Items.Count>0)
                    {
                        ddlGrade.SelectedIndex = 0;
                    }
                }
                txtLength.Text = dt.Rows[RowIndex]["Length"].ToString();
                txtWidth.Text = dt.Rows[RowIndex]["Width"].ToString();
                txtopn_no.ReadOnly = true;
                txtinterfaceid.ReadOnly = true;
                txtCycleTime.ReadOnly = true;
                ddlMultiDownID.Attributes.Add("disabled", "true");
                ddlPlantId.Attributes.Add("disabled", "true");
                if (dt.Rows[RowIndex]["FinishedOperation"] != null && dt.Rows[RowIndex]["FinishedOperation"] != System.DBNull.Value)
                {
                    if (Convert.ToInt32(dt.Rows[RowIndex]["FinishedOperation"]).Equals(0))
                        chkFinishOperation.Checked = false;
                    else if (Convert.ToInt32(dt.Rows[RowIndex]["FinishedOperation"]).Equals(1))
                        chkFinishOperation.Checked = true;
                    else
                        chkFinishOperation.Checked = false;
                }
                else
                    chkFinishOperation.Checked = false;

                List<string> machinelist = dt.AsEnumerable().Where(k => k.Field<int>("operationno") == Convert.ToInt32(txtopn_no.Text)).Select(k => k.Field<string>("machineid")).ToList();
                lbUpdateTimeMachine.DataSource = machinelist;
                lbUpdateTimeMachine.DataBind();

                foreach (GridViewRow row in componentgrd.Rows)
                {

                    row.BackColor = Color.Transparent;
                    row.Attributes.Add("rowselected", "false");
                }
                GridViewRow row1 = componentgrd.Rows[RowIndex];
                row1.BackColor = Color.Lime;
                row1.Attributes.Add("rowselected", "true");
            }
            else
            {
                lbUpdateTimeMachine.DataSource = null;
                lbUpdateTimeMachine.DataBind();
                btnnew_Click(null, EventArgs.Empty);
            }
        }
        #endregion

        #region -------------GetStdCycleAndLoadUnloadTimeForGrid---------------
        private void GetStdCycleAndLoadUnloadTimeForGrid(String Machineid, string Componentid, string OperatioNo, out Double standardLoad, out Double standardCycleTime, out Int64 loadUnloadVal)
        {
            standardLoad = 0.0;
            standardCycleTime = 0.0;
            string Machiningtime = string.Empty;
            string CycleTime = string.Empty;
            string loadUnload = string.Empty;
            loadUnloadVal = 0;
            TimeFormatVal = DataBaseAccess.GetTimeFormat();
            if (!string.IsNullOrEmpty(TimeFormatVal))
            {
                if (TimeFormatVal.Equals("ss"))
                {
                    DataBaseAccess.GetAllStdTimes(Machineid, Componentid, OperatioNo, out CycleTime, out Machiningtime, out loadUnload);
                    standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 2);
                    standardCycleTime = Math.Round(Convert.ToDouble(Machiningtime), 2);
                    loadUnloadVal = (Convert.ToInt64(loadUnload));
                    txtloadunload.Text = standardLoad.ToString();
                    txtmachinetime.Text = standardCycleTime.ToString();
                    txtloadunloadthreshold.Text = loadUnload.ToString();
                }
                else
                {
                    DataBaseAccess.GetAllStdTimes(Machineid, Componentid, OperatioNo, out CycleTime, out Machiningtime, out loadUnload);

                    if (!string.IsNullOrEmpty(CycleTime) && !string.IsNullOrEmpty(Machiningtime))
                    {
                        standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 2);
                        standardCycleTime = Math.Round((Convert.ToDouble(Machiningtime)), 2);
                    }
                    if (standardLoad > 0)
                    {
                        standardLoad = Math.Round(standardLoad / 60.0, 2);
                    }
                    if (standardCycleTime > 0)
                    {
                        standardCycleTime = Math.Round(standardCycleTime / 60.0, 2);
                    }

                    if (Convert.ToDouble(loadUnload) > 0)
                    {
                        loadUnloadVal = (Convert.ToInt64(loadUnload)) / 60;

                    }

                    txtloadunload.Text = standardLoad.ToString();
                    txtmachinetime.Text = standardCycleTime.ToString();
                    txtloadunloadthreshold.Text = loadUnloadVal.ToString();
                }
            }
        }
        #endregion

        #region -------------componentgrd_RowEditing---------------
        protected void componentgrd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            // int rowsindex = componentgrd.SelectedRow.RowIndex;
            if (rowIndex > -1)
            {
                //txtopn_no.Enabled = false;
                //txtinterfaceid.Enabled = false;

                //ddlMultiDownID.Enabled = false;
                //ddlMultiDownID.Enabled = false;
                //ddlPlantId.Enabled = false;

                ddlPlantId.SelectedIndex = -1;
                DataTable dt = componentgrd.DataSource as DataTable;
                RowIndex = rowIndex;

                int j = rowIndex;

                if (componentgrd.DataSource != null)
                {
                    string val = componentgrd.Rows[rowIndex].Cells[3].ToString();
                    int i = 0;
                    SetValuesToControls(dt, RowIndex);
                    allMachines = DataBaseAccess.GetAllMacForPlant("");
                    if (allMachines != null && allMachines.Count > 0)
                    {
                        ddlMultiDownID.DataSource = allMachines;
                    }
                    for (int index = 0; index < ddlMultiDownID.Items.Count; ++index)
                    {
                        ddlMultiDownID.SelectedIndex = 0;//SelectedItem.Text = "ALL";
                    }
                    if (i < ddlMultiDownID.Items.Count)
                    {
                        foreach (var item in ddlMultiDownID.Items)
                        {
                            if (item.Equals(val))
                            {
                                ddlMultiDownID.SelectedIndex = i; //(i, true);
                                break;
                            }
                            i++;

                            if (item.ToString() == val)
                            {
                                ddlMultiDownID.SelectedIndex = i; //(i, true);
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(ddlMultiDownID.SelectedItem.ToString()))
                    {
                        string Plantid = DataBaseAccess.GetPlantID(ddlMultiDownID.SelectedItem.ToString());
                        ddlPlantId.SelectedValue = Plantid;
                    }
                    if (chkFinishOperation.Checked)
                        FinishOperation = 1;
                    else
                        FinishOperation = 0;
                }
            }
        }
        #endregion

        #region -------------OnRowDataBound---------------
        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.componentgrd, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }
        #endregion

        #region ------------componentgrd_SelectedIndexChanged---------------
        protected void componentgrd_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // int rowIndex = e.NewEditIndex;
                int rowsindex = componentgrd.SelectedRow.RowIndex;
                string componentid = txtsearch.Text;

                //foreach (GridViewRow row in componentgrd.Rows)
                //{

                //    row.BackColor = Color.Transparent;
                //    row.Attributes.Add("rowselected", "false");
                //}
                //GridViewRow row1 = componentgrd.SelectedRow;
                //row1.BackColor = Color.Lime;
                //row1.Attributes.Add("rowselected","true");

                componentgrd.SelectedRow.Focus();
                if (rowsindex > -1)
                {
                    //txtopn_no.Enabled = false;
                    //txtinterfaceid.Enabled = false;

                    //ddlMultiDownID.Enabled = false;
                    //ddlMultiDownID.Enabled = false;
                    //ddlPlantId.Enabled = false;

                    ddlPlantId.SelectedIndex = -1;
                    //DataTable dt2 = componentgrd.DataSource as DataTable;
                    DataTable dt = DataBaseAccess.GetComponentDetails(componentid, "", "ViewCompOpnInfo");

                    RowIndex = rowsindex;
                    //componentgrd.Rows[componentgrd.SelectedIndex].Attributes["style"] = "focus:Lime;";

                    int j = rowsindex;

                    if (dt != null)
                    {

                        string val = componentgrd.Rows[componentgrd.SelectedIndex].Cells[2].Text;//componentgrd.Rows[rowsindex].Cells[3].Text();
                        int i = 0;
                        SetValuesToControls(dt, RowIndex);
                        //List<string> allMachines = DataBaseAccess.GetAllMacForPlant("");
                        if (allMachines != null && allMachines.Count > 0)
                        {
                            ddlMultiDownID.DataSource = allMachines;
                        }
                        for (int index = 0; index < ddlMultiDownID.Items.Count; ++index)
                        {
                            ddlMultiDownID.SelectedIndex = 0;//SelectedItem.Text = "ALL";
                        }
                        if (i < ddlMultiDownID.Items.Count)
                        {
                            foreach (var item in ddlMultiDownID.Items)
                            {
                                if (item.Equals(val))
                                {
                                    ddlMultiDownID.SelectedIndex = RowIndex; //(i, true);
                                    break;
                                }
                                if (item.ToString() == val)
                                {
                                    ddlMultiDownID.SelectedValue = val;
                                    //ddlMultiDownID.SelectedIndex = RowIndex; //(i, true);
                                    break;
                                }
                                i++;
                            }
                        }
                        if (!string.IsNullOrEmpty(ddlMultiDownID.SelectedItem.ToString()))
                        {
                            string Plantid = DataBaseAccess.GetPlantID(ddlMultiDownID.SelectedItem.ToString());
                            if (Plantid != "")
                            {
                                ddlPlantId.SelectedValue = Plantid;
                            }

                        }
                    }
                }
                if (chkFinishOperation.Checked)
                    FinishOperation = 1;
                else
                    FinishOperation = 0;
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region -------------select ComponentID---------------
        protected void btncomponent_Click(object sender, EventArgs e)
        {
            int Index = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
            LinkButton compid = (LinkButton)ComponentIdgrd.Rows[Index].FindControl("btn_componentid");

            string str = compid.Text;
            ComponentInfogrd(str);
        }
        #endregion

        #region -------------btnnew_Click---------------
        protected void btnnew_Click(object sender, EventArgs e)
        {

            txtopn_no.Text = string.Empty;
            txtdrawing.Text = string.Empty;
            txtdescription.Text = string.Empty;
            txtsubop.Text = "1";
            txtinterfaceid.Text = string.Empty;
            txttarget.Text = "100";
            txtprice.Text = "1";
            txtmachinetime.Text = "10";
            txtsetuptime.Text = string.Empty;
            txtCycleTime.Text = string.Empty;
            txtcuttingtime.Text = string.Empty;
            txtloadunload.Text = string.Empty;
            txtloadunloadthreshold.Text = string.Empty;
            txtopn_no.ReadOnly = false;
            txtinterfaceid.ReadOnly = false;
            txtCycleTime.ReadOnly = true;
            ddlMultiDownID.Attributes.Remove("disabled");
            ddlPlantId.Attributes.Remove("disabled");
            btnassign.Visible = false;
            chkFinishOperation.Checked = false;
            txtminLoadUnloadThreshold.Text = string.Empty;
            txtPartPerCycle.Text = string.Empty;
            txtStrokesPerPart.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtThickness.Text = string.Empty;
            if (ddlGrade.Items.Count > 0)
            {
                ddlGrade.SelectedIndex = 0;
            }
            txtLength.Text = string.Empty;
            txtWidth.Text = string.Empty;
            btnUpdateStdTime.Visible = false;
        }
        #endregion

        #region ------------btnassign_Click---------------
        protected void btnassign_Click(object sender, EventArgs e)
        {
            try
            {
                int c = componentgrd.Rows.Count;
                if (c > 0)
                {

                    txtopn_no.ReadOnly = false;
                    txtinterfaceid.ReadOnly = false;
                    txtCycleTime.ReadOnly = true;
                    ddlMultiDownID.Attributes.Remove("disabled");
                    ddlPlantId.Attributes.Remove("disabled");
                }
                else
                {
                    btnassign.Visible = false;
                    btnUpdateStdTime.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }

        }
        #endregion

        #region ------------btnsave_Click---------------
        protected void btnsave_Click(object sender, EventArgs e)
        {
            bool isSuccessFailure = false;
            //bool IsOperationFinished = false;
            bool rowAdded = false;
            int subop = 0;
            double subop2 = 0;
            string MachineIds = string.Empty;

            foreach (ListItem item in ddlMultiDownID.Items) //(var item in ddlMultiDownID.SelectedValue)
            {
                if (item.Selected)
                {
                    MachineIds += item.Value + ",";
                }
            }

            #region Validations for interface
            if (string.IsNullOrEmpty(txtinterfaceid.Text.ToString()))
            {
                lblMessages.Text = GetLocalResourceObject("InterfaceIDcannotbeempty").ToString();
                return;
            }
            if (Convert.ToUInt32(txtinterfaceid.Text.ToString()) <= 0)
            {
                lblMessages.Text = "Operation Id Cannot be ZERO or Negative";
                return;
            }

            if (string.IsNullOrEmpty(txtopn_no.Text.ToString()))
            {
                lblMessages.Text = GetLocalResourceObject("OperationNocannotbeempty").ToString();
                return;
            }
            if (ddlMultiDownID.SelectedIndex < 1 && ddlMultiDownID.SelectedValue == "")
            {
                lblMessages.Text = GetLocalResourceObject("Pleaseselectamachine").ToString();
                return;
            }

            if (int.TryParse(txtsubop.Text.ToString(), out subop))
            {
                if (subop < 1)
                {
                    lblMessages.Text = GetLocalResourceObject("Suboperationmustbegreaterthan0").ToString();
                    return;
                }
            }
            else
            {
                lblMessages.Text = GetLocalResourceObject("Suboperationmustbeapositiveintegerotherthan0").ToString();
                return;
            }

            if (double.TryParse(txtmachinetime.Text.ToString(), out subop2))
            {
                if (subop == 0)
                {
                    lblMessages.Text = GetLocalResourceObject("Machiningtimemustbegreaterthan0").ToString();
                    return;
                }
            }
            else
            {
                lblMessages.Text = GetLocalResourceObject("Machiningtimemustbeapositiveintegerotherthan0").ToString();
                return;
            }
            #endregion

            try
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (chkFinishOperation.Checked)
                    FinishOperation = 1;
                else
                    FinishOperation = 0;
                if (ConfigurationManager.AppSettings["GEAPages"].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    if (FinishOperation.Equals(1))
                    {
                        DataBaseAccess.RemoveFinishOperation(txtsearch.Text);
                    }
                }
                else
                {
                    if (FinishOperation.Equals(1))
                    {
                        foreach (ListItem item in ddlMultiDownID.Items) //(var item in ddlMultiDownID.SelectedValue)
                        {
                            if (item.Selected)
                                DataBaseAccess.RemoveFinishOperation(txtsearch.Text, item.Value);

                        }
                    }
                }
                DataBaseAccess.InsertOrUpdateComponentIdDetailsForAmarRajMangal(txtsearch.Text, "", "", txtdescription.Text.ToString(), "", "", "", txtopn_no.Text.ToString(), MachineIds,
                txtprice.Text.ToString(), txtCycleTime.Text.ToString(), txtdrawing.Text.ToString(), txtinterfaceid.Text.ToString(), txtloadunloadthreshold.Text.ToString(),
                txtmachinetime.Text.ToString(), txtsubop.Text.ToString(), txtsetuptime.Text.ToString(), txtcuttingtime.Text.ToString(), txttarget.Text.ToString(),
                 Session["UserName"].ToString(), "", "", "", "", "", "", "", "", "", "", "", FinishOperation, "InsertCompOpnInfo", txtminLoadUnloadThreshold.Text, txtPartPerCycle.Text, txtStrokesPerPart.Text, txtWeight.Text, txtThickness.Text, ddlGrade.SelectedValue.Equals("None",StringComparison.OrdinalIgnoreCase)?"": ddlGrade.SelectedValue, txtLength.Text, txtWidth.Text, out isSuccessFailure);
                if (isSuccessFailure)
                {
                    rowAdded = true;
                }
                else
                {
                    lblMessages.Text = GetLocalResourceObject("CouldnotSavetheDetails").ToString();
                }
                if (rowAdded)
                {

                    ComponentInfogrd(txtsearch.Text);
                    lblMessages.Text = GetLocalResourceObject("DetailsaddedUpdatedsuccessfully").ToString();
                    //btnSearchComponent_Click(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error - \n" + ex.ToString());
                throw;
            }

        }
        #endregion

        #region ------------btndelete_Click---------------
        protected void btndelete_Click(object sender, EventArgs e)
        {

            bool isSuccessOrFailure = false;
            string MachineIds = string.Empty;
            string interfaceid = Session["intefaceID"].ToString();
            foreach (ListItem item in ddlMultiDownID.Items) //(var item in ddlMultiDownID.SelectedValue)
            {
                if (item.Selected)
                {
                    MachineIds += item.Value + ",";
                }
            }
            try
            {
                if (componentgrd.Rows.Count <= 0)
                {
                    lblMessages.Text = GetLocalResourceObject("NoDataToDelete").ToString();
                    return;
                }
                DataTable dt = componentgrd.DataSource as DataTable;
                #region
                if (componentgrd.Rows.Count > 0)
                {
                    if (interfaceid != "")
                    {
                        DataBaseAccess.DeleteComponentOperationDetails("DeleteOpnInfo", txtsearch.Text, interfaceid, txtinterfaceid.Text, txtopn_no.Text, ddlMultiDownID.SelectedValue, out isSuccessOrFailure);
                    }
                    else
                    {
                        Response.Redirect("ComponentInformation.aspx", false);
                    }


                }
                if (isSuccessOrFailure == true)
                {
                    ComponentInfogrd(txtsearch.Text);
                    //btnnew_Click(null, EventArgs.Empty);
                }

                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("component operation delete " + ex.Message);
                throw;
            }

        }
        #endregion

        protected void componentgrd_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtrslt = (DataTable)Session["dirState"];
            if (dtrslt.Rows.Count > 0)
            {
                if (Convert.ToString(Session["sortdr"]) == "Asc")
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                    Session["sortdr"] = "Desc";
                }
                else
                {
                    dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                    Session["sortdr"] = "Asc";
                }
                componentgrd.DataSource = dtrslt;
                componentgrd.DataBind();
            }
        }

        protected void txtloadunload_TextChanged(object sender, EventArgs e)
        {
            double val = 0;
            if (double.TryParse(txtloadunload.Text, out val))
            {
                txtCycleTime.Text = (Convert.ToDouble(txtmachinetime.Text) + val).ToString();
            }
        }

        protected void txtmachinetime_TextChanged(object sender, EventArgs e)
        {
            double val = 0;
            if (double.TryParse(txtmachinetime.Text, out val))
            {
                txtCycleTime.Text = (Convert.ToDouble(txtloadunload.Text) + val).ToString();
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static COPSTDTimeData GetUpdateStdTimes(string machineId, string component, string operation)
        {
            COPSTDTimeData data = new COPSTDTimeData();
            try
            {
                string TimeFormatVal = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();
                string Machiningtime = string.Empty;
                string CycleTime = string.Empty;
                Double standardCycleTime = 0;
                Double standardLoad = 0;
                if (!string.IsNullOrEmpty(TimeFormatVal))
                {
                    if (TimeFormatVal == ("ss"))
                    {
                        data.TimeFormat = "Standard Time (in seconds)";
                        VDGDataBaseAccess.GetAllStdTimes(machineId, component, operation, out CycleTime, out Machiningtime);
                        standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 2);
                        standardCycleTime = Math.Round(Convert.ToDouble(Machiningtime), 2);
                        data.LoadUnLoad = standardLoad.ToString();
                        data.CycleTime = standardCycleTime.ToString();
                    }
                    else
                    {
                        data.TimeFormat = "Standard Time (in minutes)";
                        VDGDataBaseAccess.GetAllStdTimes(machineId, component, operation, out CycleTime, out Machiningtime);
                        standardLoad = Math.Round((Convert.ToDouble(CycleTime) - Convert.ToDouble(Machiningtime)), 2);
                        standardCycleTime = Math.Round((Convert.ToDouble(Machiningtime)), 2);
                        if (standardLoad > 0)
                        {
                            standardLoad = Math.Round(standardLoad / 60.0, 2);
                        }
                        if (standardCycleTime > 0)
                        {
                            standardCycleTime = Math.Round(standardCycleTime / 60.0, 2);
                        }
                        data.LoadUnLoad = standardLoad.ToString();
                        data.CycleTime = standardCycleTime.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return data;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string updateStandardTimeData(string machineId, string component, string operation, string stdCycleTime, string stdLoadTime)
        {
            double cycle = 0;
            double loadUnload = 0, stdloadunload = 0;
            string IsSuccessfull = string.Empty;
            string TimeFormatVal = (VDGDataBaseAccess.GetShopdefaultsTimeFormat()).ToLower();
            #region CycleTime
            if (!string.IsNullOrEmpty(TimeFormatVal))
            {
                if (TimeFormatVal == ("ss"))
                    cycle = Convert.ToDouble(stdCycleTime.ToString());
                else
                    cycle = Convert.ToDouble(stdCycleTime.ToString()) * 60;
            }
            #endregion

            #region LoadUnload
            if (!string.IsNullOrEmpty(TimeFormatVal))
            {
                if (TimeFormatVal == ("ss"))
                    loadUnload = Convert.ToDouble(stdLoadTime.ToString());
                else
                    loadUnload = Convert.ToDouble(stdLoadTime.ToString()) * 60;
            }
            #endregion

            stdloadunload = cycle + loadUnload;
            VDGDataBaseAccess.UpdateAllStdTimes(cycle.ToString(), stdloadunload.ToString(), machineId.ToString(), component, operation, false, out IsSuccessfull);
            return IsSuccessfull;
        }

        protected void btnStdTimeUpdateConfirm_Click(object sender, EventArgs e)
        {

            ComponentInfogrd(txtsearch.Text);
        }
    }
}