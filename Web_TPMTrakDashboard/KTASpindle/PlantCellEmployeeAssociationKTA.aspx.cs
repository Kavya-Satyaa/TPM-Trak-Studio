using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class PlantCellEmployeeAssociationKTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantID();
                BindData();
            }
        }
        private void BindPlantID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                if (list.Count > 0)
                {
                    list.Remove("All");
                }
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindData()
        {
            try
            {
                DataTable dtDataTemp = DBAccess.getCellEmployeeAssociationDetails(ddlPlant.SelectedValue);
                ClearDataGridColumns();
                //TemplateField tempField = new TemplateField();
                //tempField.HeaderText = "";
                //tempField.ItemTemplate = new DynamicTemplateField(DataControlRowType.DataRow);
                //gvEmployeeData.Columns.Add(tempField);
                DataTable dtData = dtDataTemp.Clone();
                for (int i = 3; i < dtData.Columns.Count; i++)
                {
                    dtData.Columns[i].DataType = typeof(bool);
                }
                foreach (DataRow row in dtDataTemp.Rows)
                {
                    dtData.ImportRow(row);
                }
                if (dtData != null)
                {
                    foreach (DataColumn dataColumn in dtData.Columns)
                    {
                        if (dataColumn.ColumnName.Equals("PlantID", StringComparison.OrdinalIgnoreCase))
                        {
                            //BoundField boundField = new BoundField();
                            //boundField.HeaderText = dataColumn.ColumnName;
                            //boundField.DataField = dataColumn.ColumnName;
                            //boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            //gvEmployeeData.Columns.Add(boundField);
                            continue;
                        }
                        else if (dataColumn.ColumnName.Equals("EmployeeID", StringComparison.OrdinalIgnoreCase))
                        {
                            BoundField boundField = new BoundField();
                            boundField.HeaderText = "Employee ID";
                            boundField.DataField = dataColumn.ColumnName;
                            boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvEmployeeData.Columns.Add(boundField);
                        }
                        else if (dataColumn.ColumnName.Equals("EmployeeName", StringComparison.OrdinalIgnoreCase))
                        {
                            BoundField boundField = new BoundField();
                            boundField.HeaderText = "EmployeeName";
                            boundField.DataField = dataColumn.ColumnName;
                            boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvEmployeeData.Columns.Add(boundField);
                        }
                        else
                        {
                            CheckBoxField checkBoxField = new CheckBoxField();
                            checkBoxField.HeaderText = dataColumn.ColumnName;
                            checkBoxField.DataField = dataColumn.ColumnName;
                            checkBoxField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvEmployeeData.Columns.Add(checkBoxField);

                        }
                    }
                    gvEmployeeData.DataSource = dtData;
                    gvEmployeeData.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void ClearDataGridColumns()
        {
            if (gvEmployeeData.Columns.Count > 0)
            {
                for (int i = gvEmployeeData.Columns.Count - 1; i >= 0; i--)
                {
                    gvEmployeeData.Columns.RemoveAt(i);
                }
            }
        }

        protected void gvEmployeeData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string value = hdnCheckboxClick.Value;
                if (value.Equals("update", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (GridViewRow selectedRow in gvEmployeeData.Rows)
                    {

                        string IsUpdate = selectedRow.Cells[0].Text;
                        for (int i = 2; i < selectedRow.Cells.Count; i++)
                        {
                            string cellID = gvEmployeeData.HeaderRow.Cells[i].Text;
                            string plantID = ddlPlant.SelectedValue;
                            string empid = selectedRow.Cells[0].Text;
                            CheckBox checkBox = (CheckBox)selectedRow.Cells[i].Controls[0];
                            if (checkBox.Checked)
                            {
                                DBAccess.insertDeleteEmployeeCellAssociation(plantID, cellID, empid, "insert");
                            }
                            else
                            {
                                DBAccess.insertDeleteEmployeeCellAssociation(plantID, cellID, empid, "delete");
                            }

                        }
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "openSuccessModal('Saved Successfully.')", true);
                    hdnCheckboxClick.Value = "";
                }
                BindData();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
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
}