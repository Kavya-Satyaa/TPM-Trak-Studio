using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class MapRulesToMachines : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindRules();
                bindMapRulesToMachinesDetails();
            }

        }
        private void bindRules()
        {
            try
            {
                ddlRules.DataSource = DataBaseAccess.GetAllRules();
                ddlRules.DataBind();
                foreach (ListItem item in ddlRules.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        
        private void bindMapRulesToMachinesDetails()
            {
            try
            {
                if (gvRuleMachine.Columns.Count > 0)
                {
                    for (int ij = gvRuleMachine.Columns.Count - 1; ij >= 0; ij--)
                    {
                        gvRuleMachine.Columns.RemoveAt(ij);
                    }
                }
                List<string> rulesList = new List<string>();
                List<string> removeRule = new List<string>();
                List<string> headerrule = new List<string>();
                foreach (ListItem item in ddlRules.Items)
                {
                    if (item.Selected)
                    {
                        rulesList.Add(item.Text);
                    }
                }
                if (rulesList.Count <= 0)
                {
                    if (ddlRules.Items.Count > 0)
                    {
                        rulesList.Add(ddlRules.Items[0].Text);
                        ddlRules.Items[0].Selected = true;
                    }
                }
                DataTable dtData = DataBaseAccess.GetMapRulesToMachineDetails();
                foreach (DataColumn dc in dtData.Columns)
                {
                     headerrule.Add(dc.ColumnName);
                    //if (dc.ColumnName == "Machineid") continue;
                    //if (rulesList.Contains(dc.ColumnName))
                    //{

                    //}
                    //else
                    //{
                    //    dtData.Columns.Remove(dc.ColumnName);
                    //}
                }
                for (int i = 0; i < headerrule.Count; i++)
                {
                    if (headerrule[i] == "Machineid") continue;
                    if (rulesList.Contains(headerrule[i]))
                    {

                    }
                    else
                    {
                        dtData.Columns.Remove(headerrule[i]);
                    }
                }

                DataTable newDataTable = dtData.Clone();
                foreach (DataColumn dc in newDataTable.Columns)
                {
                    if (dc.DataType == typeof(int))
                    {
                        dc.DataType = typeof(bool);
                    }
                }
                foreach (DataRow dr in dtData.Rows)
                {
                    newDataTable.ImportRow(dr);
                }

                if (newDataTable != null)
                {

                    int i = 0;
                    foreach (DataColumn dataColumn in newDataTable.Columns)
                    {
                        if (i == 0)
                        {
                            i++;
                            BoundField boundField = new BoundField();
                            boundField.HeaderText = dataColumn.ColumnName;
                            boundField.DataField = dataColumn.ColumnName;
                            boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvRuleMachine.Columns.Add(boundField);
                        }
                        else
                        {
                            CheckBoxField checkBoxField = new CheckBoxField();
                            checkBoxField.HeaderText = dataColumn.ColumnName;
                            checkBoxField.DataField = dataColumn.ColumnName;
                            checkBoxField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvRuleMachine.Columns.Add(checkBoxField);

                        }
                    }
                  
                    gvRuleMachine.DataSource = newDataTable;
                    gvRuleMachine.DataBind();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
               // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void gvRuleMachine_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvRow = e.Row;
            if (e.Row.RowType == DataControlRowType.Header)            {                for (int i = 0; i < e.Row.Cells.Count; i++)                {                    if (i == 0)
                    {
                        continue;
                    }                    CheckBox checkBox = new CheckBox();                    checkBox.Text = e.Row.Cells[i].Text;                    e.Row.Cells[i].Controls.Add(checkBox);                }            }            if (gvRow.RowType == DataControlRowType.DataRow)            {                for (int i = 0; i < gvRow.Cells.Count; i++)                {                    if (gvRow.Cells[i].HasControls())                    {                        if (gvRow.Cells[i].Controls[0].GetType().Equals(typeof(CheckBox)))                        {                            CheckBox checkBox = (CheckBox)gvRow.Cells[i].Controls[0];                            checkBox.Enabled = true;                        }                    }                }            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, List<string>> dct = new Dictionary<string, List<string>>();
                for (int i = 0; i < gvRuleMachine.Rows.Count; i++)
                {
                    dct.Add(gvRuleMachine.Rows[i].Cells[0].Text, new List<string>());
                }

                for (int i = 0; i < gvRuleMachine.Rows.Count; i++)
                {
                    for (int j = 1; j < gvRuleMachine.Columns.Count; j++)
                    {
                        CheckBox checkBox = (CheckBox)gvRuleMachine.Rows[i].Cells[j].Controls[0];
                        if (checkBox.Checked)
                        {
                            dct[gvRuleMachine.Rows[i].Cells[0].Text].Add(gvRuleMachine.HeaderRow.Cells[j].Text);
                        }
                        //if (gvRuleMachine.Rows[i].Cells[j].ToString() == "1")
                        //{
                        //    dct[dt.Rows[i]["Machineid"].ToString()].Add(dt.Columns[j].Caption);
                        //}
                    }
                }

                DataBaseAccess.ClearRuleMap();
                DataBaseAccess.InsertRuleMap(dct);
                DataBaseAccess.UpdateAlertMachineMap();
                bindMapRulesToMachinesDetails();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
               // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
         
        }

        protected void ddlRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bindMapRulesToMachinesDetails();
            }catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
               // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
    }
}