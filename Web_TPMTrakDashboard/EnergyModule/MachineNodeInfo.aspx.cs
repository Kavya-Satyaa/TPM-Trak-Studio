using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.EnergyModule
{
    public partial class MachineNodeInfo : System.Web.UI.Page
    {
        List<string> machineinfo = new List<string>();
        List<string> ipaddresses = new List<string>();
        List<string> allCellIDs = new List<string>();
        public List<NodeMachineId> NodeDetails = new List<NodeMachineId>();
        DataTable dt = new DataTable();
        int count;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindPlantIDs();
                BindCellIDs();
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    tdCell.Visible = true;
                    ddlCell.Visible = true;
                    tdCell1.Visible = true;
                }
                else
                {
                    tdCell.Visible = false;
                    ddlCell.Visible = false;
                    tdCell1.Visible = false;
                }
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                //{
                //    tdCell.Visible = true;
                //    ddlCell.Visible = true;
                //    tdCell1.Visible = true;
                //}
                //else
                //{
                //    tdCell.Visible = false;
                //    ddlCell.Visible = false;
                //    tdCell1.Visible = false;
                //}
                BindMachineId();
                BindGridview();
                BindDDLIPAddress();
                changeindex();
            }
        }


        #region Bind Data
        private void BindDDLIPAddress()
        {
            try
            {
                ipaddresses = DataBaseAccess.GetMachineInfo_IPAddress();
                ddlIPAddress.DataSource = ipaddresses;
                ddlIPAddress.DataBind();
                ddlIPAddress.Items.Insert(0, "All");
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void changeindex()
        {
            try
            {
                foreach (GridViewRow gvrow in gridviewmachnode.Rows)
                {
                    string MachineId = ((Label)gvrow.FindControl("labelmachineId")).Text;
                    string EmModelNum = ((Label)gvrow.FindControl("lblEmModelNumber")).Text;
                    DropDownList ddlMachine = ((DropDownList)gvrow.FindControl("ddlMachineID")) as DropDownList;
                    DropDownList ddlEmModelNo = ((DropDownList)gvrow.FindControl("ddlEmModelNumber")) as DropDownList;
                    ddlMachine.Text = string.IsNullOrEmpty(MachineId) ? "None" : MachineId;
                    ddlEmModelNo.Text = EmModelNum;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindMachineId()
        {
            try
            {
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    machineinfo = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), "", "");
                }
                else
                {
                    //machineinfo = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines();
                    machineinfo = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                }
                ddlmachineID.DataSource = machineinfo;
                ddlmachineID.DataBind();
                ddlmachineID.Items.Insert(0, "All");
                count = gridviewmachnode.Rows.Count;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindGridview()
        {
            try
            {
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    dt = DataBaseAccess.MachineNode_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString());
                }
                else
                {
                    //dt = DataBaseAccess.MachineNode();
                    dt = DataBaseAccess.MachineNode_New(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                }
                gridviewmachnode.DataSource = dt;
                gridviewmachnode.DataBind();
                gridviewmachnode.ShowFooter = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var ddl = e.Row.FindControl("ddlMachineID") as DropDownList;
                    if (ddl != null)
                    {
                        //ddl.DataSource = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines();
                        ddl.DataSource = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                        ddl.DataBind();
                        ddl.Items.Insert(0, "None");
                    }

                    // g:
                    //CheckBox checkBox = e.Row.Cells[7].Controls[0] as CheckBox;
                    //checkBox.Enabled = true;
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    var ddl = e.Row.FindControl("ddlMachineID") as DropDownList;
                    if (ddl != null)
                    {
                        ddl.DataSource = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                        ddl.DataBind();
                        ddl.Items.Insert(0, "None");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void OnDataBound(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = gridviewmachnode.FooterRow.FindControl("ddlMachineID") as DropDownList;
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    machineinfo = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines_Kiswok(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), "", "");
                }
                else
                {
                    //machineinfo = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines();
                    machineinfo = EnergyModule.Models.DataBaseAccess.GetAllEM_Machines(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                }
                ddl.DataSource = machineinfo;
                ddl.DataBind();
                ddl.Items.Insert(0, "None");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        #region Add Row

        protected void btnnew_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnnew.Text == "New")
                {
                    btnnew.Text = "Cancel";

                    //gridviewmachnode.FooterRow.Visible = true;
                    AddNewRowToGrid();
                }
                else
                {
                    btnnew.Text = "New";
                    BindGridview();
                    //changeindex();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void AddNewRowToGrid()
        {
            try
            {
                BindGridview();
                changeindex();
                DataTable dt = new DataTable();
                dt.Columns.Add("MachineId");
                dt.Columns.Add("NodeInterface");
                dt.Columns.Add("NodeID");
                dt.Columns.Add("SubSystem");
                dt.Columns.Add("EM_NodeId");
                dt.Columns.Add("EM_ModelNumber");
                dt.Columns.Add("PLC_IP_Address");
                dt.Columns.Add("Enabled");
                dt.Columns.Add("SortOrder");
                foreach (GridViewRow gvRow in gridviewmachnode.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["MachineId"] = ((Label)gvRow.FindControl("labelmachineId")).Text;
                    dr["NodeInterface"] = ((TextBox)gvRow.FindControl("lblNodeInterface")).Text;
                    dr["NodeInterface"] = ((TextBox)gvRow.FindControl("lblEMNodeInterfaceid")).Text;
                    dr["NodeID"] = ((TextBox)gvRow.FindControl("lblnodeOrder")).Text;
                    dr["SubSystem"] = ((TextBox)gvRow.FindControl("lblSubSystem")).Text;
                    dr["EM_ModelNumber"] = ((Label)gvRow.FindControl("lblEmModelNumber")).Text;
                    dr["PLC_IP_Address"] = ((TextBox)gvRow.FindControl("lblPlcIpAddress")).Text;
                    dr["Enabled"] = ((CheckBox)gvRow.FindControl("chkEnabledEM")).Checked.ToString();
                    dr["EM_NodeId"] = ((TextBox)gvRow.FindControl("lblEMNodeInterfaceid")).Text;
                    dr["SortOrder"] = ((TextBox)gvRow.FindControl("lblsortOrder")).Text;
                    dt.Rows.Add(dr);
                }

                DataRow dr1 = dt.NewRow();
                dr1["MachineId"] = "";
                dr1["NodeInterface"] = "";
                dr1["NodeID"] = "";
                dr1["SubSystem"] = "";
                dr1["EM_ModelNumber"] = "";
                dr1["PLC_IP_Address"] = "";
                dr1["Enabled"] = false;
                dr1["SortOrder"] = "";
                dr1["EM_NodeId"] = "";
                dt.Rows.Add(dr1);
                gridviewmachnode.DataSource = dt;
                gridviewmachnode.DataBind();
                ddlmachineID.Text = "All";
                changeindex();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        #endregion

        #region savedata
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool res = false; string success = "";
                int rowcount = 0, i = 1;
                string NodeInterface = string.Empty,
                    NodeId = string.Empty,
                    Sortorder = string.Empty,
                    MachineId = string.Empty,
                    machineId1 = string.Empty,
                    SubSystem = string.Empty,
                    EmModelNum = string.Empty,
                    PlcIpAddress = string.Empty,
                    EMNodeInterfaceId = string.Empty,
                    Enabled = string.Empty,
                    UpdatedTS = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Msg = string.Empty;
                int SortOrder = 0;
                rowcount = gridviewmachnode.Rows.Count;
                if (gridviewmachnode.FooterRow.Visible)
                {
                    var row = gridviewmachnode.FooterRow;
                    MachineId = ((DropDownList)row.FindControl("ddlMachineID")).SelectedValue.ToString().Equals("None", StringComparison.OrdinalIgnoreCase) ? "" : ((DropDownList)row.FindControl("ddlMachineID")).SelectedValue.ToString();
                    //machineId1 = ((Label)row.FindControl("labelmachineId")).Text;
                    NodeInterface = ((TextBox)row.FindControl("lblNodeInterface")).Text;
                    EMNodeInterfaceId = ((TextBox)row.FindControl("lblEMNodeInterfaceid")).Text;
                    NodeId = ((TextBox)row.FindControl("lblnodeOrder")).Text;
                    SubSystem = ((TextBox)row.FindControl("lblSubSystem")).Text;
                    EmModelNum = ((DropDownList)row.FindControl("ddlEmModelNumber")).SelectedValue.ToString();
                    PlcIpAddress = ((TextBox)row.FindControl("lblPlcIpAddress")).Text;
                    Enabled = ((CheckBox)row.FindControl("chkEnabledEM")).Checked.ToString();
                    //Enabled = (row.Cells[7].Controls[0] as CheckBox).Checked.ToString(); // g: to update code
                    Sortorder = ((TextBox)row.FindControl("lblsortOrder")).Text;
                    if (!string.IsNullOrEmpty(Sortorder) && int.TryParse(Sortorder, out SortOrder))
                    {
                        if (SortOrder == 0)
                        {
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            lblMessages.Text = "Please Enter SortOrder";
                        }
                        else
                        {
                            SortOrder = Convert.ToInt32(Sortorder);
                            if (!string.IsNullOrEmpty(NodeId) && !string.IsNullOrEmpty(NodeInterface) && !string.IsNullOrEmpty(EMNodeInterfaceId) && !string.IsNullOrEmpty(EmModelNum) && !string.IsNullOrEmpty(PlcIpAddress) && !string.IsNullOrEmpty(Enabled))
                            {
                                success = DataBaseAccess.NodeSaveUpdate_EM(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                                //res = DataBaseAccess.NodeSaveUpdate(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                                if (success != "")
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Green;
                                    lblMessages.Text = "Records updated successfully";
                                }
                                else
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = string.Format("Records could not be updated after row #{1}: {0}", Msg, i);
                                    //return;
                                }
                            }
                            else
                            {
                                lblMessages.ForeColor = System.Drawing.Color.Red;
                                lblMessages.Text = "Please Enter the Values";
                            }
                        }
                    }
                    else
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Sort Order must be a non zero positive integer.";
                        return;
                    }
                }
                else
                {
                    for (int j = 0; j < gridviewmachnode.Rows.Count; j++)
                    {
                        var row = gridviewmachnode.Rows[j];
                        MachineId = ((DropDownList)row.FindControl("ddlMachineID")).SelectedValue.ToString().Equals("None", StringComparison.OrdinalIgnoreCase) ? "" : ((DropDownList)row.FindControl("ddlMachineID")).SelectedValue.ToString();
                        //machineId1 = ((Label)row.FindControl("labelmachineId")).Text;
                        NodeInterface = ((TextBox)row.FindControl("lblNodeInterface")).Text;
                        EMNodeInterfaceId = ((TextBox)row.FindControl("lblEMNodeInterfaceid")).Text;
                        NodeId = ((TextBox)row.FindControl("lblnodeOrder")).Text;
                        SubSystem = ((TextBox)row.FindControl("lblSubSystem")).Text;
                        EmModelNum = ((DropDownList)row.FindControl("ddlEmModelNumber")).SelectedValue.ToString();
                        PlcIpAddress = ((TextBox)row.FindControl("lblPlcIpAddress")).Text;
                        Enabled = ((CheckBox)row.FindControl("chkEnabledEM")).Checked.ToString();
                        //Enabled = (row.Cells[7].Controls[0] as CheckBox).Checked.ToString(); // g: to update code
                        Sortorder = ((TextBox)row.FindControl("lblsortOrder")).Text;
                        if (!string.IsNullOrEmpty(Sortorder) && int.TryParse(Sortorder, out SortOrder))
                        {
                            if (SortOrder == 0)
                            {
                                lblMessages.ForeColor = System.Drawing.Color.Red;
                                lblMessages.Text = "Please Enter SortOrder";
                            }
                            else
                            {
                                SortOrder = Convert.ToInt32(Sortorder);
                                if (!string.IsNullOrEmpty(NodeId) && !string.IsNullOrEmpty(NodeInterface) && !string.IsNullOrEmpty(EmModelNum) && !string.IsNullOrEmpty(PlcIpAddress) && !string.IsNullOrEmpty(Enabled))
                                {
                                    success = DataBaseAccess.NodeSaveUpdate_EM(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                                    //res = DataBaseAccess.NodeSaveUpdate(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                                    if (success != "")
                                    {
                                        lblMessages.ForeColor = System.Drawing.Color.Green;
                                        lblMessages.Text = "Records updated successfully";
                                    }
                                    else
                                    {
                                        lblMessages.ForeColor = System.Drawing.Color.Red;
                                        lblMessages.Text = string.Format("Records could not be updated after row #{1}: {0}", Msg, i);
                                        //return;
                                    }
                                }
                                else
                                {
                                    lblMessages.ForeColor = System.Drawing.Color.Red;
                                    lblMessages.Text = "Please Enter the Values";
                                }
                            }
                        }
                    }
                }
                //foreach (GridViewRow row in gridviewmachnode.Rows)
                //{
                //    MachineId = ((DropDownList)row.FindControl("ddlMachineID")).Text.Equals("None", StringComparison.OrdinalIgnoreCase) ? "" : ((DropDownList)row.FindControl("ddlMachineID")).Text;
                //    machineId1 = ((Label)row.FindControl("labelmachineId")).Text;
                //    NodeInterface = ((TextBox)row.FindControl("lblNodeInterface")).Text;
                //    EMNodeInterfaceId = ((TextBox)row.FindControl("lblEMNodeInterfaceid")).Text;
                //    NodeId = ((TextBox)row.FindControl("lblnodeOrder")).Text;
                //    SubSystem = ((TextBox)row.FindControl("lblSubSystem")).Text;
                //    EmModelNum = ((DropDownList)row.FindControl("ddlEmModelNumber")).Text;
                //    PlcIpAddress = ((TextBox)row.FindControl("lblPlcIpAddress")).Text;
                //    Enabled = ((CheckBox)row.FindControl("chkEnabledEM")).Checked.ToString();
                //    //Enabled = (row.Cells[7].Controls[0] as CheckBox).Checked.ToString(); // g: to update code
                //    Sortorder = ((TextBox)row.FindControl("lblsortOrder")).Text;

                //    if (!string.IsNullOrEmpty(Sortorder) && int.TryParse(Sortorder, out SortOrder))
                //    {
                //        if (SortOrder == 0)
                //        {
                //            lblMessages.ForeColor = System.Drawing.Color.Red;
                //            lblMessages.Text = "Please Enter SortOrder";
                //        }
                //        else
                //        {
                //            if (btnnew.Text == "Cancel" && i == rowcount)
                //            {
                //                SortOrder = Convert.ToInt32(Sortorder);
                //                if (!string.IsNullOrEmpty(NodeId) && !string.IsNullOrEmpty(NodeInterface) && !string.IsNullOrEmpty(EMNodeInterfaceId) && !string.IsNullOrEmpty(EmModelNum) && !string.IsNullOrEmpty(PlcIpAddress) && !string.IsNullOrEmpty(Enabled))
                //                {
                //                    success = DataBaseAccess.NodeSaveUpdate_EM(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                //                    //res = DataBaseAccess.NodeSaveUpdate(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                //                }
                //                else
                //                {
                //                    lblMessages.ForeColor = System.Drawing.Color.Red;
                //                    lblMessages.Text = "Please Enter the Values";
                //                }
                //            }
                //            else
                //            {
                //                SortOrder = Convert.ToInt32(Sortorder);
                //                if (!string.IsNullOrEmpty(NodeId) && !string.IsNullOrEmpty(NodeInterface) && !string.IsNullOrEmpty(EmModelNum) && !string.IsNullOrEmpty(PlcIpAddress) && !string.IsNullOrEmpty(Enabled))
                //                {
                //                    success = DataBaseAccess.NodeSaveUpdate_EM(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                //                    //res = DataBaseAccess.NodeSaveUpdate(MachineId, NodeInterface, NodeId, SubSystem, EmModelNum, PlcIpAddress, Enabled, SortOrder, EMNodeInterfaceId, UpdatedTS, out Msg);
                //                }
                //                else
                //                {
                //                    lblMessages.ForeColor = System.Drawing.Color.Red;
                //                    lblMessages.Text = "Please Enter the Values";
                //                }
                //            }
                //            if (success!="")
                //            {
                //                lblMessages.ForeColor = System.Drawing.Color.Green;
                //                lblMessages.Text = "Records updated successfully";
                //            }
                //            else
                //            {
                //                lblMessages.ForeColor = System.Drawing.Color.Red;
                //                lblMessages.Text = string.Format("Records could not be updated after row #{1}: {0}", Msg, i);
                //                //return;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        lblMessages.ForeColor = System.Drawing.Color.Red;
                //        lblMessages.Text = "Sort Order must be a non zero positive integer.";
                //        return;
                //    }
                //    i++;
                //}
                ddlmachineID.Text = "All";
                BindGridview();
                BindDDLIPAddress();
                changeindex();
                SetFocus(btnsave);
                btnnew.Text = "New";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        #region Delete Data
        protected void gridviewmachnode_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                bool res = false;
                string NodeId = string.Empty;
                string ipaddr = string.Empty;
                GridViewRow row = (GridViewRow)gridviewmachnode.Rows[e.RowIndex];
                int i = e.RowIndex;
                if (i >= 0)
                {
                    NodeId = ((TextBox)gridviewmachnode.Rows[i].Cells[1].FindControl("lblnodeOrder")).Text;
                    ipaddr = ((TextBox)gridviewmachnode.Rows[i].Cells[1].FindControl("lblPlcIpAddress")).Text;
                    if (!string.IsNullOrEmpty(NodeId))
                    {
                        res = DataBaseAccess.NodeDelete(NodeId, ipaddr);
                    }
                    ddlmachineID.Text = "All";
                    BindGridview();
                    BindDDLIPAddress();
                    changeindex();
                    if (res)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = "Record deleted Successfully.";
                    }
                    else
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Record are not Deleted.";
                    }
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "Please Select data to delete!.";
                }
                SetFocus(btnnew);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        #region View Data
        protected void btnview_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (ddlmachineID.Text == "All")
                {
                    BindGridview();
                    changeindex();
                }
                else
                {
                    dt = DataBaseAccess.getmachinegriddata(ddlmachineID.Text);
                    gridviewmachnode.DataSource = dt;
                    gridviewmachnode.DataBind();
                    changeindex();
                }
                ddlIPAddress.Text = "All";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        #endregion

        protected void btnSendToPlc_Click(object sender, EventArgs e)
        {
            try
            {
                string selected = string.Empty;
                string ipAddr = "";
                string modelNo = "";
                string EM_nodeID = "";
                string enabled = "";
                int index = 0;
                string nodeInterfaceid = "";
                string enableAddr = "";
                string nodeidAddr = "";
                string modelnoAddr = "";
                bool success = true;

                int selectedCount = 0;
                foreach (GridViewRow row in gridviewmachnode.Rows)
                {
                    selected = ((CheckBox)row.FindControl("chkSelect")).Checked.ToString();
                    if (selected.Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedCount += 1;
                        ipAddr = ((TextBox)row.FindControl("lblPlcIpAddress")).Text.ToString();
                        //MachineNodeInterfaceAddr = string.Format("{0}.{1}", index / 8, index % 8);
                        modelNo = ((DropDownList)row.FindControl("ddlEmModelNumber")).Text.ToString();
                        nodeInterfaceid = ((TextBox)row.FindControl("lblNodeInterface")).Text.ToString();
                        EM_nodeID = ((TextBox)row.FindControl("lblEMNodeInterfaceid")).Text;
                        enabled = ((CheckBox)row.FindControl("chkEnabledEM")).Checked.ToString();
                        index = int.Parse(((TextBox)row.FindControl("lblsortOrder")).Text) - 1;
                        enableAddr = string.Format("{0}.{1}", index / 8, index % 8);
                        modelnoAddr = (66 + index * 2).ToString();
                        nodeidAddr = (4 + index * 2).ToString();
                        success &= Profinet.PutWord(ipAddr, "100", nodeidAddr, EM_nodeID);
                        success &= Profinet.PutWord(ipAddr, "100", modelnoAddr, modelNo);
                        success &= Profinet.PutBit(ipAddr, "100", enableAddr, enabled);
                        ////ask
                        success &= Profinet.PutWord(ipAddr, EM_nodeID, "90", nodeInterfaceid);
                        if (!success)
                        {
                            Logger.WriteDebugLog(string.Format("Problem updating to PLC: nodeID {0}: {1}, modelNo {2}: {3}, enabled {4}: {5}",
                                nodeidAddr, EM_nodeID,
                                modelnoAddr, modelNo,
                                enableAddr, enabled));
                            break;

                        }
                    }
                }
                if (selectedCount == 0)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Please select data to send to PLC";
                }
                else
                {
                    if (success)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = string.Format("{0} records sent to PLC successfully", selectedCount);
                    }
                    else
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "PLC could not be updated, please try again...";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnGetPlcConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string selected = string.Empty;
                string ipAddr = "";
                ushort modelNo = 0;
                ushort EM_nodeID = 0;
                ushort nodeInterfaceid = 0;
                bool enabled = false;
                int index = 0;
                string enableAddr = "";
                string nodeidAddr = "";
                string modelnoAddr = "";
                string Em_Node_ID = "";
                bool success = true;

                foreach (GridViewRow row in gridviewmachnode.Rows)
                {
                    ipAddr = ((TextBox)row.FindControl("lblPlcIpAddress")).Text.ToString();
                    Em_Node_ID = ((TextBox)row.FindControl("lblEMNodeInterfaceid")).Text.ToString();
                    index = int.Parse(((TextBox)row.FindControl("lblsortOrder")).Text) - 1;
                    enableAddr = string.Format("{0}.{1}", index / 8, index % 8);
                    modelnoAddr = (66 + index * 2).ToString();
                    nodeidAddr = (4 + index * 2).ToString();
                    success &= Profinet.GetWord(ipAddr, "100", nodeidAddr, out EM_nodeID);
                    success &= Profinet.GetWord(ipAddr, "100", modelnoAddr, out modelNo);
                    success &= Profinet.GetBit(ipAddr, "100", enableAddr, out enabled);
                    //toask
                    success &= Profinet.GetWord(ipAddr, Em_Node_ID, "90", out nodeInterfaceid);


                    if (!success)
                    {
                        Logger.WriteDebugLog(string.Format("Problem reading from PLC: nodeID {0}: {1}, modelNo {2}: {3}, enabled {4}: {5}",
                            nodeidAddr, EM_nodeID,
                            modelnoAddr, modelNo,
                            enableAddr, enabled));
                        break;
                    }
                    ((DropDownList)row.FindControl("ddlEmModelNumber")).Text = modelNo.ToString();
                    ((TextBox)row.FindControl("lblNodeInterface")).Text = nodeInterfaceid.ToString();
                    ((TextBox)row.FindControl("lblEMNodeInterfaceid")).Text = EM_nodeID.ToString();
                    ((CheckBox)row.FindControl("chkEnabledEM")).Checked = enabled;
                }

                if (success)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = string.Format("Showing current PLC configuration");
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "All PLC configuration data could not be retrieved, please try again...";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnIPAddressView_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DataBaseAccess.GetMachineGridDataForIPAddress(ddlIPAddress.Text);
                gridviewmachnode.DataSource = dt;
                gridviewmachnode.DataBind();
                changeindex();
                ddlmachineID.Text = "All";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPlantIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCellIDs()
        {
            try
            {
                List<string> list = DataBaseAccess.GetCellIDs(ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue.ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellIDs();
            BindMachineId();
            BindGridview();
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineId();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox headerCheckBox = (CheckBox)gridviewmachnode.HeaderRow.FindControl("chkSelectAll");

                foreach (GridViewRow row in gridviewmachnode.Rows)
                {
                    CheckBox rowCheckBox = (CheckBox)row.FindControl("chkSelect");
                    rowCheckBox.Checked = headerCheckBox.Checked;
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}