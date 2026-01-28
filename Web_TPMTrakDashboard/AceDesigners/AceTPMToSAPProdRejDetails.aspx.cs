using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.AceDesigners.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AceDesigners
{
    public partial class AceTPMToSAPProdRejDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                BindMachineId();
                BindData();
            }
        }
        private void BindMachineId()
        {
            try
            {
                List<string> list = DataBaseAccess.GetMachineIDForPlant("");
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachineId: " + ex.Message);
            }
        }

        private void BindData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = AceDatabaseAccess.GetAceTPMToSAPProdRejDetails(ddlMachine.SelectedValue, txtFromDate.Text, txtToDate.Text, txtWorkOrderNo.Text,ddlMessageType.SelectedValue.ToString());
                if (dt!=null)
                {
                    dt.Columns.Remove("IDD");
                    dt.Columns.Remove("RejectedQty");
                    dt.Columns.Remove("RejReason");
                    dt.Columns["MachineID"].ColumnName = "Machine ID";
                    dt.Columns["MachineDescription"].ColumnName = "Machine Description";
                    dt.Columns["MachineInterface"].ColumnName = "Machine Interface";
                    dt.Columns["OperationNo"].ColumnName = "Operation No.";
                    dt.Columns["OpnInterface"].ColumnName = "Opn Interface";
                    dt.Columns["Operator"].ColumnName = "Operator";
                    dt.Columns["OprInterface"].ColumnName = "Opr Interface";
                    dt.Columns["BatchStart"].ColumnName = "Batch Start";
                    dt.Columns["BatchEnd"].ColumnName = "Batch End";
                    dt.Columns["AcceptedQty"].ColumnName = "Accepted Qty.";
                    dt.Columns["MachiningTime"].ColumnName = "Machining Time";
                    dt.Columns["Status"].ColumnName = "SAP Status";
                    dt.Columns["Remarks"].ColumnName = "SAP Remarks";
                    dt.Columns["Timestamp"].ColumnName = "SAP Timestamp";
                }
               
                gvData.DataSource = dt;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData:  " + ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}