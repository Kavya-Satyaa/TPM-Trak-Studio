using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class MapRulesToConsumers : System.Web.UI.Page
    {
        bool isSuccessful = false;
        List<Tuple<string, string>> consumerMachruleIDs = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnFirstTimeMachineView.Value = "true";
                hdnFirstTimeRuleView.Value = "true";
                hdnFirstTimeConsumerView.Value = "true";
                BindPlant();
                chkMachine.Checked = true;
                chkMachine_CheckedChanged(null, null);
            }
        }
        public void BindPlant()
        {
            try
            {
                List<string> plants = DataBaseAccess.GetPlantIDs();
                ddlPlant.DataSource = plants;
                ddlPlant.DataBind();
                ddlPlant_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //List<string> Machines = AlertDataBaseAccess.GetMachineid(ddlPlant.SelectedValue.ToString());
                //ddlMRC.DataSource = Machines;
                //ddlMRC.DataBind();

                if (chkMachine.Checked == true)
                {
                    List<string> Machines = DataBaseAccess.GetMachineid(ddlPlant.SelectedValue.ToString());
                    ddlMRC.DataSource = Machines;
                    ddlMRC.DataBind();
                }
                if (chkConsumer.Checked == true)
                {
                    string plantid = ddlPlant.SelectedValue.ToString();
                    List<string> Consumer = DataBaseAccess.GetUserid(plantid);
                    ddlMRC.DataSource = Consumer;
                    ddlMRC.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkRule.Checked == true)
                {
                    hdnRecentDdlRule.Value = ddlMRC.SelectedIndex.ToString();
                    hdnRecentDdlPlant_Rule.Value = ddlPlant.SelectedIndex.ToString();
                }
                if (chkMachine.Checked == true)
                {
                    if (hdnRecentPlantName.Value != ddlPlant.SelectedValue.ToString())
                    {
                        List<string> Machine = DataBaseAccess.GetMachineid(ddlPlant.SelectedValue.ToString());
                        ddlMRC.DataSource = Machine;
                        ddlMRC.DataBind();
                        if (ddlMRC.SelectedIndex == -1)
                        {
                            ddlMRC.SelectedIndex = 0;
                            hdnRecentDdlMachine.Value = "0";
                        }
                    }
                    hdnRecentDdlMachine.Value = ddlMRC.SelectedIndex.ToString();
                    hdnRecentDdlPlant_Machine.Value = ddlPlant.SelectedIndex.ToString();
                    hdnRecentPlantName.Value = ddlPlant.SelectedValue.ToString();
                }
                if (chkConsumer.Checked == true)
                {
                    hdnRecentDdlConsumer.Value = ddlMRC.SelectedIndex.ToString();
                    hdnRecentDdlPlant_Consumer.Value = ddlPlant.SelectedIndex.ToString();
                }

                if (ddlPlant.Items.Count < 0) return;
                BindDatagrid();
            }
            catch (Exception ex)
            {
            }
        }
        private void BindDatagrid()
        {
            gvRule.Visible = false;
            gvMachine.Visible = false;
            gvConsumer.Visible = false;
            if (chkRule.Checked == true)
            {
                DataTable dtRule = DataBaseAccess.getDataForRuleDataTable(ddlPlant.SelectedValue.ToString(), ddlMRC.SelectedValue.ToString(), "View");

                if (dtRule.Rows.Count > 0)
                {
                    gvRule.Visible = true;
                    DataTable newDataTable = dtRule.Clone();
                    foreach (DataColumn dc in newDataTable.Columns)
                    {
                        if (dc.DataType == typeof(int))
                        {
                            dc.DataType = typeof(bool);
                        }
                    }
                    foreach (DataRow dr in dtRule.Rows)
                    {
                        newDataTable.ImportRow(dr);
                    }
                    dtRule = newDataTable;
                    ClearAndBindDataGridColumns(gvRule, newDataTable);

                }
            }
            if (chkMachine.Checked == true)
            {

                DataTable dtMachine = DataBaseAccess.getDataForMachineDataTable(ddlPlant.SelectedValue.ToString(), ddlMRC.SelectedValue.ToString(), "View");
                if (dtMachine.Rows.Count > 0)
                {
                    gvMachine.Visible = true;
                    DataTable newDataTable = dtMachine.Clone();
                    foreach (DataColumn dc in newDataTable.Columns)
                    {
                        if (dc.DataType == typeof(int))
                        {
                            dc.DataType = typeof(bool);
                        }
                    }
                    foreach (DataRow dr in dtMachine.Rows)
                    {
                        newDataTable.ImportRow(dr);
                    }
                    dtMachine = null;
                    dtMachine = newDataTable;

                    ClearAndBindDataGridColumns(gvMachine, newDataTable);
                }
            }
            if (chkConsumer.Checked == true)
            {
                DataTable dtConsumer = DataBaseAccess.getDataForConsumerDataTable(ddlPlant.SelectedValue.ToString(), ddlMRC.SelectedValue.ToString(), "View");
                if (dtConsumer.Rows.Count > 0 && !ddlMRC.SelectedValue.ToString().Equals(String.Empty))
                {
                    gvConsumer.Visible = true;
                    DataTable newDataTable = dtConsumer.Clone();

                    foreach (DataColumn dc in newDataTable.Columns)
                    {
                        if (dc.DataType == typeof(int))
                        {
                            dc.DataType = typeof(bool);
                        }
                    }
                    foreach (DataRow dr in dtConsumer.Rows)
                    {
                        newDataTable.ImportRow(dr);
                    }
                    dtConsumer = null;
                    dtConsumer = newDataTable;

                    consumerMachruleIDs = DataBaseAccess.GetMachRuleID(ddlPlant.SelectedValue.ToString());
                    ClearAndBindDataGridColumns(gvConsumer, newDataTable);

                }
            }
        }
        private void ClearAndBindDataGridColumns(GridView gv, DataTable dtData)
        {
            if (gv.Columns.Count > 0)
            {
                for (int i = gv.Columns.Count - 1; i >= 0; i--)
                {
                    gv.Columns.RemoveAt(i);
                }
            }
            if (dtData != null)
            {
                int i = 0;
                foreach (DataColumn dataColumn in dtData.Columns)
                {
                    if (i == 0)
                    {
                        i++;
                        BoundField boundField = new BoundField();
                        boundField.HeaderText = dataColumn.ColumnName;
                        boundField.DataField = dataColumn.ColumnName;
                        boundField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        gv.Columns.Add(boundField);
                    }
                    else
                    {
                        CheckBoxField checkBoxField = new CheckBoxField();
                        checkBoxField.HeaderText = dataColumn.ColumnName;
                        checkBoxField.DataField = dataColumn.ColumnName;
                        checkBoxField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        gv.Columns.Add(checkBoxField);

                    }
                }

                gv.DataSource = dtData;
                gv.DataBind();
            }
        }

        protected void chkMachine_CheckedChanged(object sender, EventArgs e)
            {
            chkRule.Checked = false;
            chkConsumer.Checked = false;
         //   chkMachine.Checked = true;
            BindRule();
            //if (hdnFirstTimeMachineView.Value == "true")
            //{
                BindDatagrid();
            //    hdnFirstTimeMachineView.Value = "false";
            //}
            //else
            //{
            //    ddlPlant.SelectedIndex = hdnRecentDdlPlant_Machine.Value == "" ? 0 : Convert.ToInt32(hdnRecentDdlPlant_Machine.Value); ;
            //    //Display(Constants.MACHINEID);
            //    BindDatagrid();
            //}
        }

        protected void chkRule_CheckedChanged(object sender, EventArgs e)
        {
            chkMachine.Checked = false;
            chkConsumer.Checked = false;
           // chkRule.Checked = true;
            BindRule();
            //if (hdnFirstTimeRuleView.Value == "true")
            //{
                BindDatagrid();
               // hdnFirstTimeRuleView.Value = "false";
            //}
            //else
            //{
            //    ddlPlant.SelectedIndex = hdnRecentDdlPlant_Rule.Value == "" ? 0 : Convert.ToInt32(hdnRecentDdlPlant_Rule.Value);
            //    //Display(Constants.RULE);
            //    BindDatagrid();
            //}

        }

        protected void chkConsumer_CheckedChanged(object sender, EventArgs e)
        {
            chkRule.Checked = false;
            chkMachine.Checked = false;
          //  chkConsumer.Checked = true;
            BindRule();
            //if (hdnFirstTimeConsumerView.Value == "true")
            //{
                BindDatagrid();
            //    hdnFirstTimeConsumerView.Value = "false";
            //}
            //else
            //{
            //    ddlPlant.SelectedIndex = hdnRecentDdlPlant_Consumer.Value == "" ? 0 : Convert.ToInt32(hdnRecentDdlPlant_Consumer.Value);
            //    //Display(Constants.CONSUMER);
            //    BindDatagrid();
            //}
        }
        private void BindRule()
        {
            if (chkRule.Checked == true)
            {
                lblMRC.InnerText = "Rule";
                List<string> Rules = DataBaseAccess.GetRule();
                
                    ddlMRC.DataSource = Rules;
                    ddlMRC.DataBind();
                if (Rules != null && Rules.Count > 0)
                {
                    ddlMRC.SelectedIndex = hdnRecentDdlRule.Value == "" ? 0 : Convert.ToInt32(hdnRecentDdlRule.Value);
                }
            }
            else if (chkMachine.Checked == true)
            {
                lblMRC.InnerText = "Machine";
                List<string> Machine = DataBaseAccess.GetMachineid(ddlPlant.SelectedValue.ToString());
                // string plantid = ddlPlant.SelectedValue.ToString();
                // List<string> Machine = AlertDataBaseAccess.GetUserid(plantid);
               
                    ddlMRC.DataSource = Machine;
                    ddlMRC.DataBind();
                if (Machine != null && Machine.Count > 0)
                {
                    ddlMRC.SelectedIndex = hdnRecentDdlMachine.Value == "" ? 0 : Convert.ToInt32(hdnRecentDdlMachine.Value);
                }
            }
            else if (chkConsumer.Checked == true)
            {
                lblMRC.InnerText = "Consumer";
                string plantid = ddlPlant.SelectedValue.ToString();
                List<string> Consumer = DataBaseAccess.GetUserid(plantid);
                // List<string> Consumer = AlertDataBaseAccess.GetMachineid(ddlPlant.SelectedValue.ToString());

                ddlMRC.DataSource = Consumer;
                ddlMRC.DataBind();
                if (Consumer != null && Consumer.Count > 0)
                {
                    ddlMRC.SelectedIndex = hdnRecentDdlConsumer.Value == "" ? 0 : Convert.ToInt32(hdnRecentDdlConsumer.Value);
                }
            }
        }

        protected void gvMachine_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvRule_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvConsumer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvRow = e.Row;
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                string tb = gvRow.Cells[0].Text;
                for (int i = 0; i < gvRow.Cells.Count; i++)
                {
                    string col = gvConsumer.HeaderRow.Cells[i].Text;
                    if (gvRow.Cells[i].HasControls())
                    {
                        if (gvRow.Cells[i].Controls[0].GetType().Equals(typeof(CheckBox)))
                        {
                            CheckBox checkBox = (CheckBox)gvRow.Cells[i].Controls[0];
                            if (consumerMachruleIDs.Any(m => m.Item1.Equals(tb) && m.Item2.Equals(col)))
                                checkBox.Enabled = true;
                            else
                                checkBox.Enabled = false;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            try
            {
                DataTable result = null;

                if (chkConsumer.Checked == true && ddlMRC.SelectedIndex != -1 && gvConsumer.Visible == true)
                {
                    hdnRecentDdlConsumer.Value = ddlMRC.SelectedIndex.ToString();

                    DataBaseAccess.deletedataRuleMachine(ddlMRC.SelectedValue.ToString(), "", "");
                    foreach (GridViewRow selectedRow in gvConsumer.Rows)
                    {
                        string Machineid = selectedRow.Cells[0].Text;
                        for (int i = 1; i < selectedRow.Cells.Count; i++)
                        {
                            string RuleID = gvConsumer.HeaderRow.Cells[i].Text;
                            CheckBox checkBox = (CheckBox)selectedRow.Cells[i].Controls[0];
                            if (checkBox.Checked)
                            {
                                DataBaseAccess.insertintoAlert_AssignRulesToUser(Machineid, RuleID, ddlMRC.SelectedValue.ToString(), out isSuccessful);
                            }
                        }
                    }
                }
                else if (chkMachine.Checked == true && ddlMRC.SelectedIndex != -1 && gvMachine.Visible == true)
                {
                    hdnRecentDdlMachine.Value = ddlMRC.SelectedIndex.ToString();
                    DataBaseAccess.deletedataRuleMachine(string.Empty, ddlMRC.SelectedValue.ToString(), string.Empty);
                    foreach (GridViewRow selectedRow in gvMachine.Rows)
                    {
                        string Machineid = selectedRow.Cells[0].Text;
                        for (int i = 1; i < selectedRow.Cells.Count; i++)
                        {
                            string RuleID = gvMachine.HeaderRow.Cells[i].Text;
                            string consumer = selectedRow.Cells[0].Text;
                            CheckBox checkBox = (CheckBox)selectedRow.Cells[i].Controls[0];
                            if (checkBox.Checked)
                            {
                                DataBaseAccess.insertintoAlert_AssignRulesToUser(ddlMRC.SelectedValue.ToString(), RuleID, consumer, out isSuccessful);
                            }
                        }
                    }
                }
                else if (chkRule.Checked == true && ddlMRC.SelectedIndex != -1 && gvRule.Visible == true)
                {
                    hdnRecentDdlRule.Value = ddlMRC.SelectedIndex.ToString();
                    DataBaseAccess.deleteAssignRulesToUser(ddlPlant.SelectedValue.ToString(), ddlMRC.SelectedValue.ToString());
                    foreach (GridViewRow selectedRow in gvRule.Rows)
                    {
                        string Machineid = selectedRow.Cells[0].Text;
                        for (int i = 1; i < selectedRow.Cells.Count; i++)
                        {
                            string RuleID = gvRule.HeaderRow.Cells[i].Text;
                            string consumer = selectedRow.Cells[0].Text;
                            CheckBox checkBox = (CheckBox)selectedRow.Cells[i].Controls[0];
                            if (checkBox.Checked)
                            {
                                DataBaseAccess.insertintoAlert_AssignRulesToUser(RuleID, ddlMRC.SelectedValue.ToString(), consumer, out isSuccessful);
                            }
                        }
                    }

                }

                if (isSuccessful)
                {
                    //MessageBox.Show("Data Saved Successfully", "Information Message", MessageBoxButton.OK);
                }
                BindDatagrid();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
        }

        protected void rdoMachine_CheckedChanged(object sender, EventArgs e)
        {

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