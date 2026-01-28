using BusinessClassLibrary;
using Elmah;
using ModelClassLibrary;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Pitti.Model;

namespace Web_TPMTrakDashboard.Pitti
{
    public partial class WorkOrderTrackingPitti : System.Web.UI.Page
    {
        static string appPath = HttpContext.Current.Server.MapPath("~/Reports");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //BindData();
            }
        }
        private void BindData()
        {
            try
            {
                List<UserAccessModel> useAccessData = new List<UserAccessModel>();
                if (Session["UserAccessData"] == null)
                    Session["UserAccessData"] = useAccessData = BindCockpitView.bindListUserAccess(Session["UserName"].ToString());
                else
                    useAccessData = Session["UserAccessData"] as List<UserAccessModel>;

                bool rejectionBtnVisible = useAccessData.Where(ss => ss.Code.Equals("WorkOrderRejectionPermissionPitti", StringComparison.OrdinalIgnoreCase)).Select(ss => ss.Selected).FirstOrDefault();

                DataTable dt = PittiDBAccess.getWorkOrderTrackingDetails(txtFromDate.Text, txtToDate.Text, txtWorkOrderSearch.Text, txtSerialNoSearch.Text);
                List<WorkorderTrackingEntity> list = new List<WorkorderTrackingEntity>();
                if (dt.Rows.Count > 0)
                {
                    WorkorderTrackingEntity headerData = new WorkorderTrackingEntity();
                    headerData.HeaderVisible = true;
                    List<string> distOpnList = dt.AsEnumerable().Select(k => k["OperationNo"].ToString()).Distinct().ToList();
                    List<WorkorderTrackingOperationEntity> headerOperationList = new List<WorkorderTrackingOperationEntity>();
                    foreach (string operationNo in distOpnList)
                    {
                        WorkorderTrackingOperationEntity headerOpnData = new WorkorderTrackingOperationEntity();
                        headerOpnData.HeaderVisible = true;
                        headerOpnData.OperationNo = "Opn " + operationNo;
                        headerOperationList.Add(headerOpnData);
                    }
                    headerData.OperationList = headerOperationList;
                    list.Add(headerData);

                    headerData = new WorkorderTrackingEntity();
                    headerData.HeaderVisible2 = true;
                    List<WorkorderTrackingOperationEntity> headerOperationList2 = new List<WorkorderTrackingOperationEntity>();

                    foreach (string operationNo in distOpnList)
                    {
                        WorkorderTrackingOperationEntity headerOpnData2 = new WorkorderTrackingOperationEntity();
                        headerOpnData2.HeaderVisible2 = true;
                        var operationType = dt.AsEnumerable().Where(k => k["OperationNo"].ToString() == operationNo).Select(x => x["OperationType"].ToString()).FirstOrDefault();
                        if (operationType.Equals("Manual", StringComparison.OrdinalIgnoreCase))
                            headerOpnData2.Template2Visible = true;
                        else
                            headerOpnData2.Template1Visible = true;
                        headerOperationList2.Add(headerOpnData2);
                    }
                    headerData.OperationList = headerOperationList2;
                    list.Add(headerData);

                    var distWorkOrderSlno = dt.AsEnumerable().Select(k => new { WorkOrder = k["WorkorderNumber"].ToString(), SerialNo = k["serialnumber"].ToString() }).Distinct().ToList();
                    foreach (var workSlno in distWorkOrderSlno)
                    {
                        WorkorderTrackingEntity data = new WorkorderTrackingEntity();
                        data.ContentVisible = true;

                        data.WorkOrder = workSlno.WorkOrder;
                        data.SerialNumber = workSlno.SerialNo;

                        var opnDataRows = dt.AsEnumerable().Where(k => k["WorkorderNumber"].ToString() == data.WorkOrder && k["serialnumber"].ToString() == data.SerialNumber).CopyToDataTable();
                        var firstRow = opnDataRows.Rows[0];
                        data.ComponentID = firstRow["Componentid"].ToString();
                        data.RejectionRemarks = firstRow["RejectionRemarks"].ToString();
                        data.RejectionBy = firstRow["RejectionBy"].ToString();
                        data.RejectionVisible = rejectionBtnVisible;
                        if (data.RejectionBy != "")
                            data.RejectionVisible = false;

                        List<WorkorderTrackingOperationEntity> operationList = new List<WorkorderTrackingOperationEntity>();
                        foreach (string operationNo in distOpnList)
                        {
                            var opnRow = opnDataRows.AsEnumerable().Where(k => k["OperationNo"].ToString() == operationNo).FirstOrDefault();
                            WorkorderTrackingOperationEntity opnData = new WorkorderTrackingOperationEntity();
                            opnData.ContentVisible = true;
                            if (opnRow != null)
                            {
                                opnData.MachineID = opnRow["MachineID"].ToString();
                                opnData.OperationNo = opnRow["OperationNo"].ToString();
                                opnData.StartTime = opnRow["StTime"].ToString();
                                opnData.EndTime = opnRow["NdTime"].ToString();
                                opnData.CycleTime = opnRow["CycleTime"].ToString();
                                opnData.UpdatedBy = opnRow["OperatorName"].ToString();
                                opnData.ManualStatus = opnRow["OperationStatus"].ToString();
                                opnData.OperationType = opnRow["OperationType"].ToString().Trim();
                                if (opnRow["OperationType"].ToString().Equals("Auto", StringComparison.OrdinalIgnoreCase))
                                {
                                    opnData.Template1Visible = true;
                                }
                                else if (opnRow["OperationType"].ToString().Equals("Manual", StringComparison.OrdinalIgnoreCase))
                                {
                                    opnData.Template2Visible = true;
                                }
                            }
                            else
                            {
                                opnData.Template1Visible = true;
                            }
                            operationList.Add(opnData);
                        }
                        data.OperationList = operationList;
                        list.Add(data);
                    }
                }
                lvWokrOrder.DataSource = list;
                lvWokrOrder.DataBind();

                Session["WorkOrderData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnRejection_Click(object sender, EventArgs e)
        {
            try
            {
                var item = (sender as Button).NamingContainer as ListViewDataItem;
                string workOrder = (item.FindControl("lblWorkOrder") as Label).Text;
                string slNo = (item.FindControl("lblSerialNumber") as Label).Text;
                string rejectionRemarks = (item.FindControl("txtRejectionRemarks") as TextBox).Text;
                string rejectionBy = Session["UserName"] == null ? "" : Session["UserName"].ToString();
                string compID = (item.FindControl("lblComponentID") as Label).Text;
                int result = PittiDBAccess.insertWorkOrderRejectionDetails(workOrder, slNo, rejectionRemarks, rejectionBy);
                if (result > 0)
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                    BindData();
                }
                else
                {
                    HelperClassGeneric.openInsertErrorModal(this);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void lnkClearDate_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string Generated = "";
            List<WorkorderTrackingEntity> list = new List<WorkorderTrackingEntity>();
            try
            {
                if (Session["WorkOrderData"] != null)
                    list = Session["WorkOrderData"] as List<WorkorderTrackingEntity>;

                Generated = TMPTrakGenerateReport.GenerateWorkOrderReportPitti(list, txtFromDate.Text, txtToDate.Text);

                if (Generated.Equals("TemplateNotFound", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningModal(this, "Template Not Found.");
                else if (Generated.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Download Successful.");
                else if (Generated.Equals("NoDataFound"))
                    HelperClassGeneric.openErrorModal(this, "ERROR! Try Again.");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"btnExport_Click: {ex.Message}");
            }
        }
    }
}