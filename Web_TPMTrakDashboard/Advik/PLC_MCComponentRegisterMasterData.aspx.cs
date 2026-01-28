using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class PLC_MCComponentRegisterMasterData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindDatagrid();
            }

        }


        private void BindDatagrid()
        {
            try
            {
                ddlMachineID.DataSource = DataBaseAccess.AdvikDatabaseAccess.GetMachines("");
                ddlMachineID.DataBind();
                List<RegisterCompEntity> Entity = DataBaseAccess.AdvikDatabaseAccess.GetRegisterDetails();
                if (Entity != null && Entity.Count > 0)
                {
                    gridcompregidter.DataSource = Entity;
                    gridcompregidter.DataBind();
                }
                else
                {
                    Entity.Add(new RegisterCompEntity { EventID = "", EventName = "", Register = "" });
                    
                    gridcompregidter.DataSource = Entity;
                    gridcompregidter.DataBind();
                    gridcompregidter.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gridcompregidter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                List<string> MachineID = Session["MachineList"] != null ? Session["MachineList"] as List<string> : DataBaseAccess.AdvikDatabaseAccess.GetMachines("");
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList cmb = (e.Row.FindControl("cmbFooterMachineID") as DropDownList);
                    if (cmb != null)
                    {
                        cmb.DataSource = MachineID;
                        cmb.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gridcompregidter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = 0; string MachineID = string.Empty, Operator = string.Empty, EventType = string.Empty, EventID = string.Empty, EventName = string.Empty, Register = string.Empty;
            try
            {
                switch (e.CommandName)
                {
                    case "EditRow":
                        rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        DropDownList cmb = (gridcompregidter.Rows[rowIndex].FindControl("cmbMachineID") as DropDownList);
                        if (cmb != null)
                        {
                            List<string> mac = Session["MachineList"] != null ? Session["MachineList"] as List<string> : DataBaseAccess.AdvikDatabaseAccess.GetMachines("");
                            cmb.DataSource = mac;
                            cmb.DataBind();
                        }
                        gridcompregidter.EditIndex = rowIndex;
                        BindDatagrid();
                        break;
                    case "DeleteRow":
                        rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        MachineID = (gridcompregidter.Rows[rowIndex].FindControl("lblMachineID") as Label).Text;
                        Operator = (gridcompregidter.Rows[rowIndex].FindControl("lblOperation") as Label).Text;
                        EventType = (gridcompregidter.Rows[rowIndex].FindControl("lblEventType") as Label).Text;
                        EventID = (gridcompregidter.Rows[rowIndex].FindControl("lblEventID") as Label).Text;
                        EventName = (gridcompregidter.Rows[rowIndex].FindControl("lblEventName") as Label).Text;
                        Register = (gridcompregidter.Rows[rowIndex].FindControl("lblRegister") as Label).Text;
                        DataBaseAccess.AdvikDatabaseAccess.DeleteComponentRegisterMasterData(MachineID, EventID);
                        BindDatagrid();
                        break;
                    case "UpdateRow":
                        rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        MachineID = (gridcompregidter.Rows[rowIndex].FindControl("lblMachineID") as Label).Text;
                        Operator = (gridcompregidter.Rows[rowIndex].FindControl("txtOperation") as TextBox).Text;
                        EventType = (gridcompregidter.Rows[rowIndex].FindControl("cmbEventType") as DropDownList).SelectedValue.ToString();
                        EventID = (gridcompregidter.Rows[rowIndex].FindControl("lblEventID") as Label).Text;
                        EventName = (gridcompregidter.Rows[rowIndex].FindControl("txtEventName") as TextBox).Text;
                        Register = (gridcompregidter.Rows[rowIndex].FindControl("txtRegister") as TextBox).Text;
                        if (string.IsNullOrEmpty(MachineID) || string.IsNullOrEmpty(Operator) || string.IsNullOrEmpty(EventType) || string.IsNullOrEmpty(EventID) || string.IsNullOrEmpty(EventName) || string.IsNullOrEmpty(Register))
                        {

                        }
                        else
                        {
                            DataBaseAccess.AdvikDatabaseAccess.UpdateComponentRegisterMasterData(MachineID, Operator, EventType, EventID, EventName, Register);
                        }
                        gridcompregidter.EditIndex = -1;
                        BindDatagrid();
                        break;
                    case "CancelRow":
                        gridcompregidter.EditIndex = -1;
                        BindDatagrid();
                        break;
                    case "NewRow":
                        MachineID = (gridcompregidter.FooterRow.FindControl("cmbFooterMachineID") as DropDownList).SelectedValue.ToString();
                        Operator = (gridcompregidter.FooterRow.FindControl("txtFooterOperation") as TextBox).Text;
                        EventType = (gridcompregidter.FooterRow.FindControl("cmbFooterEventType") as DropDownList).SelectedValue.ToString();
                        EventID = (gridcompregidter.FooterRow.FindControl("cmbFooterEventID") as DropDownList).SelectedValue.ToString();
                        EventName = (gridcompregidter.FooterRow.FindControl("txtFooterEventName") as TextBox).Text;
                        Register = (gridcompregidter.FooterRow.FindControl("txtFooterRegester") as TextBox).Text;
                        if (string.IsNullOrEmpty(MachineID) || string.IsNullOrEmpty(Operator) || string.IsNullOrEmpty(EventType) || string.IsNullOrEmpty(EventID) || string.IsNullOrEmpty(EventName) || string.IsNullOrEmpty(Register))
                        {

                        }
                        else
                        {
                            DataBaseAccess.AdvikDatabaseAccess.UpdateComponentRegisterMasterData(MachineID, Operator, EventType, EventID, EventName, Register);
                        }
                        BindDatagrid();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

        }

        protected void btnView_Click(object sender, EventArgs e)
        {

        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            int i = 0;
            bool dataSaved = false;
            try
            {
                if (fileupload.HasFile)
                {
                    if (Path.GetExtension(fileupload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        ExcelPackage Excel = new ExcelPackage(fileupload.PostedFile.InputStream);
                        var worksheetSchedule = Excel.Workbook.Worksheets.First();
                        var dt = new DataTable();
                        dt.Columns.Add("SLNO");
                        dt.Columns.Add("MachineID");
                        dt.Columns.Add("Operation");
                        dt.Columns.Add("EventType");
                        dt.Columns.Add("EventID");
                        dt.Columns.Add("EventName");
                        dt.Columns.Add("Register");
                        for (int rowNum = 2; rowNum <= worksheetSchedule.Dimension.End.Row; rowNum++)
                        {
                            var worksheetRow = worksheetSchedule.Cells[rowNum, 1, rowNum, worksheetSchedule.Dimension.End.Column];
                            if (worksheetRow != null)
                            {
                                DataRow dtRow = dt.Rows.Add();
                                foreach (var Data in worksheetRow)
                                    dtRow[Data.Start.Column - 1] = Data.Text.Trim();
                            }
                        }
                       foreach(DataRow dtRow in dt.Rows)
                        {
                            DataBaseAccess.AdvikDatabaseAccess.UpdateComponentRegisterMasterData(dtRow["MachineID"].ToString(), dtRow["Operation"].ToString(), dtRow["EventType"].ToString(), dtRow["EventID"].ToString(), dtRow["EventName"].ToString(), dtRow["Register"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }

    public class RegisterCompEntity
    {
        public int SLNO { get; set; }
        public string MachineID { get; set; }
        public string Operation { get; set; }
        public string EventType { get; set; }
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string Register { get; set; }

    }
}