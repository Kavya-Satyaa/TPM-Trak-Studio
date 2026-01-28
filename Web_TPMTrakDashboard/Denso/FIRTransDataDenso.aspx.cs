using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Web_TPMTrakDashboard.Denso.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Denso
{
    public partial class FIRTransDataDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Page.ClientQueryString.Length > 0)
                {
                    if (Request.QueryString["machineID"] != null)
                    {
                        hdnMachineID.Value = Request.QueryString["machineID"];
                        hdnDownCategory.Value = Request.QueryString["downCategory"];
                        hdnDownID.Value = Request.QueryString["downID"];
                        hdnStartTime.Value = Request.QueryString["startTime"].ToString();
                        hdnEndTime.Value = Request.QueryString["endTime"].ToString();
                        lblDownDesc.Text = Request.QueryString["downDesc"].ToString();
                        lblPlant.Text = Request.QueryString["plantID"].ToString();
                        lblMachineID.Text = hdnMachineID.Value;
                        DateTime startDate = Convert.ToDateTime(hdnStartTime.Value);
                        DateTime endDate = Convert.ToDateTime(hdnEndTime.Value);
                        hdnShift.Value = DensoDBAccess.getShiftNameOfDate(startDate);
                        lblStartData.Text = startDate.ToString("hh:mm:ss") + " / " + hdnShift.Value + " / " + startDate.ToString("dd-MM-yyyy");
                        lblEndData.Text = endDate.ToString("hh:mm:ss") + " / " + hdnShift.Value + " / " + endDate.ToString("dd-MM-yyyy");
                        List<ListItem> listHoursColor = new List<ListItem>();
                        double diffInHours = (endDate - startDate).TotalHours;
                        for (int i = 1; i <= 8; i++)
                        {
                            string color = "";
                            if (i <= diffInHours || (i == 8 && diffInHours > i))
                            {
                                color = "red";

                                if (i == 1)
                                {
                                    tdHour1.Attributes.Add("class", "red-back");
                                }
                                else if (i == 2)
                                {
                                    tdHour2.Attributes.Add("class", "red-back");
                                }
                                else if (i == 3)
                                {
                                    tdHour3.Attributes.Add("class", "red-back");
                                }
                                else if (i == 4)
                                {
                                    tdHour4.Attributes.Add("class", "red-back");
                                }
                                else if (i == 5)
                                {
                                    tdHour5.Attributes.Add("class", "red-back");
                                }
                                else if (i == 6)
                                {
                                    tdHour6.Attributes.Add("class", "red-back");
                                }
                                else if (i == 7)
                                {
                                    tdHour7.Attributes.Add("class", "red-back");
                                }
                                else if (i == 8)
                                {
                                    tdHour8.Attributes.Add("class", "red-back");
                                }
                            }
                            listHoursColor.Add(new ListItem() { Value = i.ToString(), Text = color });
                        }
                        Session["FIRHoursList"] = listHoursColor;
                        BindEmployeeID();
                        BindData();
                    }
                }
            }
        }
        private void BindEmployeeID()
        {

            try
            {
                List<string> list = DataBaseAccess.GetAllEmployeesID();
                lbPRDEmpID.DataSource = list;
                lbPRDEmpID.DataBind();
                lbQADEmpID.DataSource = list;
                lbQADEmpID.DataBind();
                lbMTDEmpID.DataSource = list;
                lbMTDEmpID.DataBind();
                lbSQDEmpID.DataSource = list;
                lbSQDEmpID.DataBind();
                lbPEDEmpID.DataSource = list;
                lbPEDEmpID.DataBind();
                lbPCDEmpID.DataSource = list;
                lbPCDEmpID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindData()
        {
            try
            {
                DataTable dtAttendees = new DataTable();
                List<FIRTransDataEntity> listActionTaken = new List<FIRTransDataEntity>();
                DataTable dt = DensoDBAccess.getFIRTransactionDetails(hdnMachineID.Value, hdnDownCategory.Value, hdnDownID.Value, hdnStartTime.Value, hdnEndTime.Value, out listActionTaken, out dtAttendees);
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];

                    txtRouteCause.Text = row["RootCause"].ToString();
                    txtNextActionDecided.Text = row["NextActionDecided"].ToString();
                    HelperClassGeneric.setDropdownValue(ddlNextActionDecidedDept, row["NextActionByWhom"].ToString());
                    txtNextActionDecidedResult.Text = row["NextActionResult"].ToString();
                    txtStockStatus.Text = row["StockStatus"].ToString();
                    txtStockImpact.Text = row["StockImpact"].ToString();
                    txtPresentStatus.Text = row["PresentStatus"].ToString();
                    txtDetails.Text = row["Details"].ToString();
                    txtPartNo.Text = row["PartNo"].ToString();
                    txtPartName.Text = row["PartName"].ToString();
                    txtQtyHold.Text = row["QtyHold"].ToString();
                }

                int flag = 0;
                if (listActionTaken.Count == 0)
                {
                    flag = 1;
                    listActionTaken.Add(new FIRTransDataEntity());
                }
                gvActionTaken.DataSource = listActionTaken;
                gvActionTaken.DataBind();
                if (flag == 1)
                {
                    gvActionTaken.Rows[0].Visible = false;
                }

                if (dtAttendees.Rows.Count > 0)
                {

                    var row = dtAttendees.AsEnumerable().Where(k => k["Department"].ToString() == "PRD").FirstOrDefault();
                    if (row != null)
                    {
                        HelperClassGeneric.clearListBoxValue(lbPRDEmpID);
                        setListBoxValue(lbPRDEmpID, row["EmployeeID"].ToString());
                    }
                    row = dtAttendees.AsEnumerable().Where(k => k["Department"].ToString() == "QAD").FirstOrDefault();
                    if (row != null)
                    {
                        HelperClassGeneric.clearListBoxValue(lbQADEmpID);
                        setListBoxValue(lbQADEmpID, row["EmployeeID"].ToString());
                    }
                    row = dtAttendees.AsEnumerable().Where(k => k["Department"].ToString() == "MTD").FirstOrDefault();
                    if (row != null)
                    {
                        HelperClassGeneric.clearListBoxValue(lbMTDEmpID);
                        setListBoxValue(lbMTDEmpID, row["EmployeeID"].ToString());
                    }
                    row = dtAttendees.AsEnumerable().Where(k => k["Department"].ToString() == "SQD").FirstOrDefault();
                    if (row != null)
                    {
                        HelperClassGeneric.clearListBoxValue(lbSQDEmpID);
                        setListBoxValue(lbSQDEmpID, row["EmployeeID"].ToString());
                    }
                    row = dtAttendees.AsEnumerable().Where(k => k["Department"].ToString() == "PED").FirstOrDefault();
                    if (row != null)
                    {
                        HelperClassGeneric.clearListBoxValue(lbPEDEmpID);
                        setListBoxValue(lbPEDEmpID, row["EmployeeID"].ToString());
                    }
                    row = dtAttendees.AsEnumerable().Where(k => k["Department"].ToString() == "PCD").FirstOrDefault();
                    if (row != null)
                    {
                        HelperClassGeneric.clearListBoxValue(lbPCDEmpID);
                        setListBoxValue(lbPCDEmpID, row["EmployeeID"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        public static void setListBoxValue(ListBox listBox, string value)
        {
            try
            {
                if (listBox != null)
                {
                    if (value != "")
                    {
                        var list = value.Split(',').ToList();
                        foreach (string itemValue in list)
                        {
                            if (listBox.Items.FindByValue(itemValue) != null)
                            {
                                listBox.SelectedValue = itemValue;
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
        private void BindActionTaken()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void gvActionTaken_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string value = (e.Row.FindControl("hdnDepartment") as HiddenField).Value;
                    DropDownList ddl = (e.Row.FindControl("ddlDepartment") as DropDownList);
                    HelperClassGeneric.setDropdownValue(ddl, value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lnkInsertActionTaken_Click(object sender, EventArgs e)
        {
            try
            {
                FIRTransDataEntity data = new FIRTransDataEntity();
                var row = gvActionTaken.FooterRow;
                data.PlantID = lblPlant.Text;
                data.MachineID = hdnMachineID.Value;
                data.DownID = hdnDownID.Value;
                data.DownCategory = hdnDownCategory.Value;
                data.StartTime = hdnStartTime.Value;
                data.EndTime = hdnEndTime.Value;
                data.Shift = "Shift";
                data.ActionTaken = (row.FindControl("txtActionTaken") as TextBox).Text;
                data.ActionTakenByWhom = (row.FindControl("ddlDepartment") as DropDownList).SelectedValue;
                data.ActionTakenResult = (row.FindControl("txtResult") as TextBox).Text;

                string success = DensoDBAccess.insertUpdateFIRActionTakenDetails(data);
                if (success.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
                }
                else if (success.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                }
                else
                {
                    HelperClassGeneric.openInsertErrorModal(this);
                    return;
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSaveActionTaken_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < gvActionTaken.Rows.Count; i++)
                {
                    FIRTransDataEntity data = new FIRTransDataEntity();
                    var row = gvActionTaken.Rows[i];
                    data.PlantID = lblPlant.Text;
                    data.MachineID = hdnMachineID.Value;
                    data.DownID = hdnDownID.Value;
                    data.DownCategory = hdnDownCategory.Value;
                    data.StartTime = hdnStartTime.Value;
                    data.EndTime = hdnEndTime.Value;
                    data.Shift = "Shift";
                    data.ActionTaken = (row.FindControl("txtActionTaken") as TextBox).Text;
                    data.ActionTakenByWhom = (row.FindControl("ddlDepartment") as DropDownList).SelectedValue;
                    data.ActionTakenResult = (row.FindControl("txtResult") as TextBox).Text;

                    string success = DensoDBAccess.insertUpdateFIRActionTakenDetails(data);
                    if (success.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openUpdateSuccessModal(this);
                    }
                    else if (success.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openInsertSuccessModal(this);
                    }
                    else
                    {
                        HelperClassGeneric.openInsertErrorModal(this);
                        return;
                    }
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FIRTransDataEntity data = new FIRTransDataEntity();
                data.PlantID = lblPlant.Text;
                data.MachineID = hdnMachineID.Value;
                data.DownID = hdnDownID.Value;
                data.DownCategory = hdnDownCategory.Value;
                data.StartTime = hdnStartTime.Value;
                data.EndTime = hdnEndTime.Value;
                data.Shift = hdnShift.Value;

                data.RootCause = txtRouteCause.Text;
                data.NextActionDecided = txtNextActionDecided.Text;
                data.NextActionDeciedResult = txtNextActionDecidedResult.Text;
                data.NextActionDeciedByWhom = ddlNextActionDecidedDept.SelectedValue;
                data.StockStatus = txtStockStatus.Text;
                data.Details = txtDetails.Text;
                data.StockImpact = txtStockImpact.Text;
                data.PresentStatus = txtPresentStatus.Text;
                data.PartNo = txtPartNo.Text;
                data.PartName = txtPartName.Text;
                data.QtyHold = txtQtyHold.Text;
                string success = DensoDBAccess.insertUpdateFIRTransDetails(data);
                DensoDBAccess.insertUpdateFIRAttendeesDetails(data, HelperClassGeneric.getListboxSelectedValue(lbPRDEmpID), "PRD");
                DensoDBAccess.insertUpdateFIRAttendeesDetails(data, HelperClassGeneric.getListboxSelectedValue(lbMTDEmpID), "MTD");
                DensoDBAccess.insertUpdateFIRAttendeesDetails(data, HelperClassGeneric.getListboxSelectedValue(lbPEDEmpID), "PED");
                DensoDBAccess.insertUpdateFIRAttendeesDetails(data, HelperClassGeneric.getListboxSelectedValue(lbQADEmpID), "QAD");
                DensoDBAccess.insertUpdateFIRAttendeesDetails(data, HelperClassGeneric.getListboxSelectedValue(lbSQDEmpID), "SQD");
                DensoDBAccess.insertUpdateFIRAttendeesDetails(data, HelperClassGeneric.getListboxSelectedValue(lbPCDEmpID), "PCD");
                if (success.Equals("Updated", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
                }
                else if (success.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                }
                else
                {
                    HelperClassGeneric.openInsertErrorModal(this);
                    return;
                }

                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string ReportStatus = "";
                DataTable dtAttendees = new DataTable();
                List<FIRTransDataEntity> listActionTaken = new List<FIRTransDataEntity>();
                //DataTable dt = DensoDBAccess.getFIRTransactionDetails(hdnMachineID.Value, hdnDownCategory.Value, hdnDownID.Value, hdnStartTime.Value, hdnEndTime.Value, out listActionTaken, out dtAttendees);
                ReportStatus = ReportsDenso.GetFirstInformationReport(hdnMachineID.Value, hdnDownCategory.Value, hdnDownID.Value, hdnStartTime.Value, hdnEndTime.Value, lblDownDesc.Text, lblStartData.Text, lblEndData.Text, Session["FIRHoursList"] as List<ListItem>);

                if (ReportStatus.Equals("Failed", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
                else if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageSuccess", "messageSuccess();", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageFailure", "messageFailure();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}