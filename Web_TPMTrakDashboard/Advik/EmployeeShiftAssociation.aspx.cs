using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class EmployeeShiftAssociation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindAllPlants();
                bindAllHelpRequest();
                bindEmployeeShiftDetails();
            }
        }
        private void bindAllPlants()
        {
            try
            {
                List<string> plantIds = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                // List<string> plantIds = AdvikDatabaseAccess.GetPlantIds("");
                plantIds.Remove("All");
                ddlPlant.DataSource = plantIds;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }
        private void bindAllHelpRequest()
        {
            try
            {
                List<string> allHealthRequest = new List<string>();
                allHealthRequest = AdvikDatabaseAccess.getAllCallType();
                ddlHelpRequest.DataSource = allHealthRequest;
                ddlHelpRequest.DataBind();
            }
            catch (Exception ex)
            {
                    Logger.WriteErrorLog(ex.Message);
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindEmployeeShiftDetails()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                string helprequest = ddlHelpRequest.SelectedValue == null ? "" : ddlHelpRequest.SelectedItem.ToString();
                //List<string> shiftIDs = AdvikDatabaseAccess.GetShifIDs();
               // SetGridHeader();
                DataTable dtData = AdvikDatabaseAccess.GetHelprequestRuleForEmployeeShift(plant, helprequest);
                // gvEmpShiftDetails.Rows.Clear();
                //gvEmpShiftDetails.DataSource = dt;
                //gvEmpShiftDetails.DataBind();
                ClearDataGridColumns();
                TemplateField tempField = new TemplateField();
                tempField.HeaderText = "";
                tempField.ItemTemplate = new DynamicTemplateField(DataControlRowType.DataRow);
                gvEmpShiftDetails.Columns.Add(tempField);
                if (dtData != null)
                {
                    foreach (DataColumn dataColumn in dtData.Columns)
                    {
                        if (dataColumn.ColumnName.Equals("Plant ID", StringComparison.OrdinalIgnoreCase))
                        {
                            BoundField boundField = new BoundField();
                            boundField.HeaderText = dataColumn.ColumnName;
                            boundField.DataField = dataColumn.ColumnName;
                            boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvEmpShiftDetails.Columns.Add(boundField);
                        }
                        else if (dataColumn.ColumnName.Equals("Employee ID", StringComparison.OrdinalIgnoreCase))
                        {
                            BoundField boundField = new BoundField();
                            boundField.HeaderText = dataColumn.ColumnName;
                            boundField.DataField = dataColumn.ColumnName;
                            boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvEmpShiftDetails.Columns.Add(boundField);
                        }
                        else
                        {
                            CheckBoxField checkBoxField = new CheckBoxField();
                            checkBoxField.HeaderText = dataColumn.ColumnName;
                            checkBoxField.DataField = dataColumn.ColumnName;
                            checkBoxField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvEmpShiftDetails.Columns.Add(checkBoxField);

                        }
                    }

                    gvEmpShiftDetails.DataSource = dtData;
                    gvEmpShiftDetails.DataBind();
                    gvEmpShiftDetails.Columns[0].Visible = false;
                }

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void ClearDataGridColumns()
        {
            if (gvEmpShiftDetails.Columns.Count > 0)
            {
                for (int i = gvEmpShiftDetails.Columns.Count - 1; i >= 0; i--)
                {
                    gvEmpShiftDetails.Columns.RemoveAt(i);
                }
            }
        }
        public class DynamicTemplateField : ITemplate
        {
            DataControlRowType type;
            public DynamicTemplateField(DataControlRowType type)
            {
                this.type = type;
            }
            public void InstantiateIn(Control container)
            {
                switch (type)
                {
                    case DataControlRowType.DataRow:
                        HiddenField hfUpdate = new HiddenField();
                        hfUpdate.ID = "hdfUpdate";
                        hfUpdate.ClientIDMode = ClientIDMode.Static;
                        container.Controls.Add(hfUpdate);
                        break;
                }
                //HiddenField hfUpdate = new HiddenField();
                //hfUpdate.ID = "hdfUpdate";
                //container.Controls.Add(hfUpdate);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ValidateCheckBox())
                //{
                    string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                    foreach (GridViewRow selectedRow in gvEmpShiftDetails.Rows)
                    {
                          string employeename = selectedRow.Cells[2].Text;
                        for (int i = 3; i < selectedRow.Cells.Count; i++)
                        {
                            int shiftid=1;
                            if (i == 3) { shiftid = 1; }
                            if (i == 4) { shiftid = 2; }
                            if (i == 5) { shiftid = 3; }
                            CheckBox checkBox = (CheckBox)selectedRow.Cells[i].Controls[0];
                            if (checkBox.Checked)
                            {
                                int suuces=AdvikDatabaseAccess.InsertEmpShiftAssociation(plant, employeename, shiftid);
                            }
                            else
                            {
                                int suuces = AdvikDatabaseAccess.DeleteEmpShiftAssociation(plant, employeename, shiftid);
                            }
                        }
                    //}
                    bindEmployeeShiftDetails();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
           
        }
        private bool ValidateCheckBox()
        {
            string employees = string.Empty;
            string message = string.Empty;
            foreach (GridViewRow selectedRow in gvEmpShiftDetails.Rows)
            {
                string employeeTemp = string.Empty;
                int countShift = 0;
                if (Convert.ToString(selectedRow.Cells[2].Text) != string.Empty)
                {
                    employeeTemp = Convert.ToString(selectedRow.Cells[2].Text);
                }
                for (int i = 3; i < selectedRow.Cells.Count; i++)
                {
                    CheckBox checkBox = (CheckBox)selectedRow.Cells[i].Controls[0];
                    if (!checkBox.Checked)
                    {
                        countShift++;
                    }
                }
                if (countShift >= 3)
                {
                    if (employees == string.Empty)
                    {
                        employees = employeeTemp;
                    }
                    else
                    {
                        employees = employees + " , " + employeeTemp;
                    }

                }
            }
            if (employees != string.Empty)
            {
                if (employees.Split(',').Length > 1)
                {
                    message = string.Format("Please select atleast one shift for these ({0}) Employees.", employees);
                }
                else
                {
                    message = string.Format("Please select atleast one shift for this ({0}) Employee.", employees);
                }
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = message;
                return false;
            }
            else
            {
                return true;
            }
        }
        protected void gvEmpShiftDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvRow = e.Row;
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < gvRow.Cells.Count; i++)
                {
                    if (gvRow.Cells[i].HasControls())
                    {
                        if (gvRow.Cells[i].Controls[0].GetType().Equals(typeof(CheckBox)))
                        {
                            CheckBox checkBox = (CheckBox)gvRow.Cells[i].Controls[0];
                            checkBox.Enabled = true;
                        }
                    }
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            bindEmployeeShiftDetails();
        }
    }
}