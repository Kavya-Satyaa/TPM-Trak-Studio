using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using S7.Net;
using SchedulerNetAsp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class LoadDefinition : System.Web.UI.Page
    {
        List<string> Shift = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Language"] == null || Session["connectionString"] == null || Session["UserName"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            else if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPlant();
                btnView_Click(null, null);
                Shift = DataBaseAccess.shiftdetail();
            }
        }

        private void BindPlant()
        {
            List<string> PlantID = DataBaseAccess.GetAllPlants();
            cmbPlant.DataSource = PlantID;
            cmbPlant.DataBind();
            cmbPlant_SelectedIndexChanged(null, null);
        }

        #region SelectionChange
        protected void cmbPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> Machineid = new List<string>();
            if (cmbPlant.SelectedValue == null)
            {
                Machineid = DataBaseAccess.GetAllMachines("");
            }
            else
            {
                Machineid = DataBaseAccess.GetAllMachines(cmbPlant.SelectedValue.ToString());
            }
            cmbMachineID.DataSource = Machineid;
            cmbMachineID.DataBind();
            cmbMachineID_SelectedIndexChanged(null, null);
        }

        protected void cmbMachineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> ComponentID = DataBaseAccess.GetAllComp(cmbMachineID.SelectedValue.ToString());
            cmbComponentID.DataSource = ComponentID;
            cmbComponentID.DataBind();
            ddlComponentID_SelectedIndexChanged(null, null);
        }

        protected void ddlComponentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> Operation = new List<string>();
            string Machineid = string.Empty;
            string Component = string.Empty;
            foreach (ListItem item in cmbMachineID.Items)
            {
                if (item.Selected)
                    Machineid = "'" + item + "',";
            }
            Machineid = Machineid.TrimEnd(',');
            foreach (ListItem item in cmbComponentID.Items)
            {
                if (item.Selected)
                    Component = "'" + item + "',";
            }
            Component = Component.TrimEnd(',');
            if (cmbMachineID.SelectedValue == null)
            {
                Operation = DataBaseAccess.GetOperation("", "");
            }
            else
            {
                Operation = DataBaseAccess.GetOperationByQuotes(Machineid, Component);
            }
            cmbOperation.DataSource = Operation;
            cmbOperation.DataBind();
        }

        #endregion

        protected void gridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    switch (Shift.Count)
                    {
                        case 1:
                            (e.Row.FindControl("lblhdrShift1") as Label).Text = Shift[0];
                            (e.Row.FindControl("lblShift1PDT") as Label).Text = Shift[0];
                            (e.Row.FindControl("lblhdrShift2") as Label).Visible = false;
                            (e.Row.FindControl("lblhdrShift3") as Label).Visible = false;
                            (e.Row.FindControl("lblShift2PDT") as Label).Visible = false;
                            (e.Row.FindControl("lblShift3PDT") as Label).Visible = false;
                            break;
                        case 2:
                            (e.Row.FindControl("lblhdrShift1") as Label).Text = Shift[0];
                            (e.Row.FindControl("lblhdrShift2") as Label).Text = Shift[1];
                            (e.Row.FindControl("lblShift1PDT") as Label).Text = Shift[0];
                            (e.Row.FindControl("lblShift2PDT") as Label).Text = Shift[1];
                            (e.Row.FindControl("lblhdrShift3") as Label).Visible = false;
                            (e.Row.FindControl("lblShift3PDT") as Label).Visible = false;
                            break;
                        case 3:
                            (e.Row.FindControl("lblhdrShift1") as Label).Text = Shift[0];
                            (e.Row.FindControl("lblhdrShift2") as Label).Text = Shift[1];
                            (e.Row.FindControl("lblhdrShift3") as Label).Text = Shift[2];
                            (e.Row.FindControl("lblShift1PDT") as Label).Text = Shift[0];
                            (e.Row.FindControl("lblShift2PDT") as Label).Text = Shift[1];
                            (e.Row.FindControl("lblShift3PDT") as Label).Text = Shift[2];
                            break;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    List<string> Shift = DataBaseAccess.shiftdetail();
                    switch (Shift.Count)
                    {
                        case 1:
                            (e.Row.FindControl("txtShift2") as TextBox).Visible = false;
                            (e.Row.FindControl("txtShift3") as TextBox).Visible = false;
                            (e.Row.FindControl("lblShift2PDT") as Label).Visible = false;
                            (e.Row.FindControl("lblShift3PDT") as Label).Visible = false;
                            break;
                        case 2:
                            (e.Row.FindControl("txtShift3") as TextBox).Visible = false;
                            (e.Row.FindControl("lblShift3PDT") as Label).Visible = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            List<LoadDefinitionEntity> EntityList = new List<LoadDefinitionEntity>();
            string Operation = string.Empty;
            foreach (ListItem item in cmbOperation.Items)
            {
                if (item.Selected)
                {
                    if(Operation==string.Empty)
                    {
                        Operation += "'" + item.Value.ToString() + "'";
                    }
                    else
                    {
                        Operation += ",'" + item.Value.ToString() + "'";
                    }
                   
                }
            }
            Operation = Operation.TrimEnd(',');
            string ComponentID = string.Empty;
            foreach (ListItem item in cmbComponentID.Items)
            {
                if (item.Selected)
                {
                    if (ComponentID == string.Empty)
                    {
                        ComponentID += "'" + item.Value.ToString() + "'";
                    }
                    else
                    {
                        ComponentID += ",'" + item.Value.ToString() + "'";
                    }
                }
            }
            ComponentID = ComponentID.TrimEnd(',');
            Shift = DataBaseAccess.shiftdetail();
            EntityList = DataBaseAccess.GetLoadDefinitiondata(Util.GetDateTime(txtFromDate.Text), Util.GetDateTime(txtToDate.Text), cmbPlant.SelectedValue.ToString(), cmbMachineID.SelectedValue.ToString(), ComponentID, Operation,Shift.Count);
            if (EntityList != null && EntityList.Count > 0)
            {
                gridview.DataSource = EntityList;
                gridview.DataBind();
            }
            else
            {

                gridview.DataSource = EntityList;
                gridview.DataBind();
            }
        }

        protected void btnGenerateShiftTarget_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridview.Rows)
            {
                if ((row.FindControl("chkselect") as CheckBox).Checked)
                {
                    (row.FindControl("txtShift1") as TextBox).Text = (row.FindControl("Calculatetgt1") as HiddenField).Value;
                    (row.FindControl("txtShift2") as TextBox).Text = (row.FindControl("Calculatetgt2") as HiddenField).Value;
                    (row.FindControl("txtShift3") as TextBox).Text = (row.FindControl("Calculatetgt3") as HiddenField).Value;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string PlantID = string.Empty, MachineId = string.Empty, Component = string.Empty, Operation = string.Empty, Shift1Target = string.Empty, Shift2Target = string.Empty, Shift3Target = string.Empty, TargetTotal = string.Empty;
            int idd = 0;
            bool saved = false;
            DateTime Date = DateTime.Now;
            try
            {
                foreach (GridViewRow Row in gridview.Rows)
                {
                    idd = Convert.ToInt32((Row.FindControl("idd") as HiddenField).Value);
                    MachineId = (Row.FindControl("lblMachineID") as Label).Text;
                    Component = (Row.FindControl("lblComponentID") as Label).Text;
                    Operation = (Row.FindControl("lblOperation") as Label).Text;
                    Shift1Target = (Row.FindControl("txtShift1") as TextBox).Text;
                    Shift2Target = (Row.FindControl("txtShift2") as TextBox).Text;
                    Shift3Target = (Row.FindControl("txtShift3") as TextBox).Text;
                    TargetTotal = (Row.FindControl("lblTotal") as Label).Text;
                    Date = Util.GetDateTime((Row.FindControl("lblDate") as Label).Text);
                    Shift = DataBaseAccess.shiftdetail();
                    if (string.IsNullOrEmpty(Shift1Target) || string.IsNullOrEmpty(Shift1Target) || string.IsNullOrEmpty(Shift1Target))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide Some Shift Target Value')", true);
                    }
                    switch (Shift.Count)
                    {
                        case 1:
                            saved = DataBaseAccess.SaveLoadDefinition(idd, MachineId, Component, Operation, Date, TargetTotal, Shift1Target, Shift[0]);
                            break;
                        case 2:
                            saved = DataBaseAccess.SaveLoadDefinition(idd, MachineId, Component, Operation, Date, TargetTotal, Shift1Target, Shift[0]);
                            saved = DataBaseAccess.SaveLoadDefinition(idd, MachineId, Component, Operation, Date, TargetTotal, Shift2Target, Shift[1]);
                            break;
                        case 3:
                            saved = DataBaseAccess.SaveLoadDefinition(idd, MachineId, Component, Operation, Date, TargetTotal, Shift1Target, Shift[0]);
                            saved = DataBaseAccess.SaveLoadDefinition(idd, MachineId, Component, Operation, Date, TargetTotal, Shift2Target, Shift[1]);
                            saved = DataBaseAccess.SaveLoadDefinition(idd, MachineId, Component, Operation, Date, TargetTotal, Shift3Target, Shift[2]);
                            break;
                    }

                }
                if (saved)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Updated Successfully')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string PlantID = string.Empty, MachineId = string.Empty, Component = string.Empty, Operation = string.Empty, Shift1Target = string.Empty, Shift2Target = string.Empty, Shift3Target = string.Empty, TargetTotal = string.Empty;
            int idd = 0;
            bool delete = false;
            DateTime Date = DateTime.Now;
            try
            {
                foreach (GridViewRow Row in gridview.Rows)
                {
                    if ((Row.FindControl("chkselect") as CheckBox).Checked)
                    {
                        idd = Convert.ToInt32((Row.FindControl("idd") as HiddenField).Value);
                        MachineId = (Row.FindControl("lblMachineID") as Label).Text;
                        Component = (Row.FindControl("lblComponentID") as Label).Text;
                        Operation = (Row.FindControl("lblOperation") as Label).Text;
                        Shift1Target = (Row.FindControl("txtShift1") as TextBox).Text;
                        Shift2Target = (Row.FindControl("txtShift2") as TextBox).Text;
                        Shift3Target = (Row.FindControl("txtShift3") as TextBox).Text;
                        TargetTotal = (Row.FindControl("lblTotal") as Label).Text;
                        Date = Util.GetDateTime((Row.FindControl("lblDate") as Label).Text);
                        if (idd != 0)
                        {
                            delete = DataBaseAccess.DeleteLoadDefinition(idd, Date, MachineId);
                        }
                    }
                }
                if (delete)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Deleted Successfully')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void txtShift1_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            var cell = txt.Parent;
            var row = cell.Parent;

            string value1 = ((row.FindControl("txtShift1") as TextBox).Text);
            value1 = string.IsNullOrEmpty(value1) ? "0" : value1;
            string value2 = ((row.FindControl("txtShift2") as TextBox).Text);
            value2 = string.IsNullOrEmpty(value2) ? "0" : value2;
            string value3 = ((row.FindControl("txtShift3") as TextBox).Text);
            value3 = string.IsNullOrEmpty(value3) ? "0" : value3;
            int sum = Convert.ToInt32(value1) + Convert.ToInt32(value2) + Convert.ToInt32(value3);
            (row.FindControl("lblTotal") as Label).Text = sum.ToString();
        }
    }

    public class LoadDefinitionEntity
    {
        public int SLNO { get; set; }
        public int idd { get; set; }
        public string MachineID { get; set; }
        public string Operation { get; set; }
        public string ComponentID { get; set; }
        public string StdCycleTime { get; set; }
        public string PDTShift1 { get; set; }
        public string PDTShift2 { get; set; }
        public string PDTShift3 { get; set; }
        public string Shift1Target { get; set; }
        public string Shift2Target { get; set; }
        public string Shift3Target { get; set; }
        public string Calculatetgt1 { get; set; }
        public string Calculatetgt2 { get; set; }
        public string Calculatetgt3 { get; set; }
        public string TotalTarget { get; set; }
        public string Date { get; set; }
    }
}