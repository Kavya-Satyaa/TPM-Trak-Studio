using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class FinalInspectionDashboardAdvik : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSlNoSearch.Focus();
                ClearGridData();
            }
        }
        protected void btnViewSlno_Click(object sender, EventArgs e)
        {
            try
            {
                hdnViewType.Value = "Slno";
                ClearGridData();
                hdnScannedSlno.Value = txtSlNoSearch.Text;
                BindStationStatusData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnViewSlno_Click = " + ex.Message);
            }
        }

        protected void btnViewDate_Click(object sender, EventArgs e)
        {
            try
            {
                hdnViewType.Value = "Date";
                ClearGridData();
                BindStationStatusData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnViewDate_Click = " + ex.Message);
            }
        }
        private void ClearGridData()
        {
            try
            {
               
                hdnScannedSlno.Value = "";
                stationStatusDiv.Visible = false;
                btnSave.Visible = false;
                btnApproval.Visible = false;
                btnReject.Visible = false;
                btnRework.Visible = false;
                List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
                lvFinalData.DataSource = list;
                lvFinalData.DataBind();
                lvStatusData.DataSource = list;
                lvStatusData.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }
        private void BindStationStatusData()
        {
            try
            {
                List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
                list = AdvikDatabaseAccess.getFinalInspectionList(hdnScannedSlno.Value);
                if (list.Count > 0)
                {
                    stationStatusDiv.Visible = true;
                    lvStatusData.DataSource = list;
                    lvStatusData.DataBind();
                    //if (!previsousStatinStatus.Equals("OK", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('QR Code Status: " + previsousStatinStatus + ".')", true);
                    //    return;
                    //}

                    string previsousStatinStatus = AdvikDatabaseAccess.isFinalInspectionLastStationCompleted(hdnScannedSlno.Value);
                    string fILatestStaus = AdvikDatabaseAccess.finalInspectionLatestStatus(hdnScannedSlno.Value, AdvikDatabaseAccess.getFinalInspectionMachineID());
                    if (fILatestStaus.Equals("OK", StringComparison.OrdinalIgnoreCase) || fILatestStaus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase) || fILatestStaus.Equals("Reject", StringComparison.OrdinalIgnoreCase) || previsousStatinStatus.Equals("OK", StringComparison.OrdinalIgnoreCase) || fILatestStaus.Equals("null", StringComparison.OrdinalIgnoreCase))
                    {
                        BindFinalInspectionParameterData();
                    }
                    else
                    {
                        confirmMsg2.InnerText = "QR Code Validation Status: " + previsousStatinStatus + ". Do you want to continue?";
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openModal('previousMachineConfirmModal');", true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }
        protected void btnFIContinueConfirm_Click(object sender, EventArgs e)
        {
            BindFinalInspectionParameterData();
        }
        private void BindFinalInspectionParameterData()
        {
            try
            {
                //bool previsousStatusCompleted = false;
                //string previsousStatinStatus = AdvikDatabaseAccess.isFinalInspectionLastStationCompleted(hdnScannedSlno.Value);
                //if (previsousStatinStatus.Equals("OK", StringComparison.OrdinalIgnoreCase))
                //{
                //    previsousStatusCompleted = true;
                //}

                List<ParameterMasterEntity> list = new List<ParameterMasterEntity>();
                DataTable dt2 = new DataTable();
                DataTable dt = AdvikDatabaseAccess.getFinalInspectionParameterList(hdnScannedSlno.Value, AdvikDatabaseAccess.getFinalInspectionMachineID(), out dt2);
                ParameterMasterEntity data = null;
                if (dt.Rows.Count > 0)
                {
                    string updatedBy = "", updatedTS = "", remarks = "", status = "";
                    if (dt2.Rows.Count > 0)
                    {
                        DataRow firstRow = dt2.Rows[0];
                        updatedBy = firstRow["UpdatedBy"].ToString();
                        updatedTS = firstRow["UpdatedTS"].ToString();
                        remarks = firstRow["Remarks"].ToString();
                        status = firstRow["Status"].ToString();
                    }

                    bool btnControlEnabled = true;
                    if (status.Equals("OK", StringComparison.OrdinalIgnoreCase) || status.Equals("REJECTED", StringComparison.OrdinalIgnoreCase) || status.Equals("REJECT", StringComparison.OrdinalIgnoreCase))
                    {
                        btnControlEnabled = false;
                    }
                    //bool btnControlEnabled = false;
                    //if (string.IsNullOrEmpty(status) && previsousStatusCompleted)
                    //{
                    //    btnControlEnabled = true;
                    //}

                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (i == 0)
                        {
                            list.Add(new ParameterMasterEntity
                            {
                                Type = "PartNo",
                                Label = "Part Number",
                                Value = row["ComponentID"].ToString()
                            });
                            list.Add(new ParameterMasterEntity { Type = "Slno", Label = "Serial Number", Value = hdnScannedSlno.Value, Model = row["Model"].ToString() });
                        }
                        data = new ParameterMasterEntity();
                        data.Type = "Chk";
                        data.Label = row["ParameterID"].ToString();
                        data.ParameterID = row["ParameterID"].ToString();
                        data.ParameterName = row["ParameterName"].ToString();
                        data.LSL = row["LSL"].ToString();
                        data.USL = row["USL"].ToString();
                        data.Unit = row["Unit"].ToString();
                        data.Value = row["Value"].ToString();
                        data.ControlEnabled = btnControlEnabled;
                        list.Add(data);
                        i++;
                    }

                    data = new ParameterMasterEntity();
                    data.Type = "Label";
                    data.Label = "Checked By";
                    data.Value = updatedBy;
                    list.Add(data);

                    data = new ParameterMasterEntity();
                    data.Type = "Label";
                    data.Label = "Date of Inspection";
                    data.Value = updatedTS;
                    list.Add(data);

                    data = new ParameterMasterEntity();
                    data.Type = "Remarks";
                    data.Label = "Remarks";
                    data.Value = remarks;
                    data.ControlEnabled = btnControlEnabled;
                    list.Add(data);

                    btnSave.Visible = btnControlEnabled;
                    btnApproval.Visible = btnControlEnabled;
                    btnReject.Visible = btnControlEnabled;
                    btnRework.Visible = btnControlEnabled;

                    //if (string.IsNullOrEmpty(status) && !previsousStatinStatus.Equals("OK", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('QR Code Validation Status: " + previsousStatinStatus + ".')", true);
                    //}
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openWarningModal('QR Code Validation Status: " + previsousStatinStatus + ".')", true);
                }
                lvFinalData.DataSource = list;
                lvFinalData.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ParameterMasterEntity data = new ParameterMasterEntity();
                data.ComponentID = (lvFinalData.Items[0].FindControl("lblPartNumber") as Label).Text;
                data.BarCode = (lvFinalData.Items[1].FindControl("lblSlno") as Label).Text;
                data.MachineID = AdvikDatabaseAccess.getFinalInspectionMachineID();
                data.Model = (lvFinalData.Items[1].FindControl("hdnModel") as HiddenField).Value;
                int insertCount = 0;
                Session["CycleDateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 0; i < lvFinalData.Items.Count; i++)
                {
                    string type = (lvFinalData.Items[i].FindControl("hdnType") as HiddenField).Value;
                    if (type.Equals("chk", StringComparison.OrdinalIgnoreCase))
                    {
                        data.Value = (lvFinalData.Items[i].FindControl("chkSelect") as CheckBox).Checked ? "1" : "0";
                        data.ParameterID = (lvFinalData.Items[i].FindControl("lblLabel") as Label).Text;
                        data.ParameterName = (lvFinalData.Items[i].FindControl("hdnParameterName") as HiddenField).Value;
                        data.LSL = (lvFinalData.Items[i].FindControl("hdnLSL") as HiddenField).Value;
                        data.USL = (lvFinalData.Items[i].FindControl("hdnUSL") as HiddenField).Value;
                        data.Unit = (lvFinalData.Items[i].FindControl("hdnUnit") as HiddenField).Value;
                        insertCount += AdvikDatabaseAccess.saveFinalInspectionParameterDetails(data, Session["CycleDateTime"].ToString());
                    }
                    else if (type.Equals("Remarks", StringComparison.OrdinalIgnoreCase))
                    {
                        string remarks = (lvFinalData.Items[i].FindControl("txtRemarks") as TextBox).Text;
                        insertCount += AdvikDatabaseAccess.insertFinalInspectionStatusRemarksDetails(data, remarks, Session["UserName"].ToString());
                    }
                }
                if (sender != null)
                {
                    if (insertCount > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openErrorModal('Insertion failed.')", true);
                    }
                    BindStationStatusData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave_Click(null, null);
                SaveFIData();
                //if (hdnConfirmType.Value.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                //{
                //    BindFinalInspectionParameterData();
                //}
                //else
                //{
                BindStationStatusData();
                // }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void SaveFIData()
        {
            try
            {
                ParameterMasterEntity data = new ParameterMasterEntity();
                data.ComponentID = (lvFinalData.Items[0].FindControl("lblPartNumber") as Label).Text;
                data.BarCode = (lvFinalData.Items[1].FindControl("lblSlno") as Label).Text;
                data.Model = (lvFinalData.Items[1].FindControl("hdnModel") as HiddenField).Value;
                string reworkMachine = "", reworkOpn = "90";
                if (hdnConfirmType.Value.Equals("Rework", StringComparison.OrdinalIgnoreCase))
                {
                    reworkMachine = AdvikDatabaseAccess.getMachineByOperation("90");
                }
                data.OperationNo = "100";
                data.MachineID = AdvikDatabaseAccess.getFinalInspectionMachineID();

                int insertCount = AdvikDatabaseAccess.saveFinalInspectionStatusDetails(data, hdnConfirmType.Value, Session["UserName"].ToString(), reworkMachine, reworkOpn, Session["CycleDateTime"].ToString());
                if (insertCount > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openErrorModal('Insertion failed.')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void btnReworkMachine_Click(object sender, EventArgs e)
        {
            try
            {
                btnConfirm_Click(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lnkRefreshbutton_Click(object sender, EventArgs e)
        {
            txtSlNoSearch.Text = "";
            txtSlNoSearch.Focus();
            ClearGridData();
        }
    }
}