using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.AlertModule.Models;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class DefineRule : System.Web.UI.Page
    {
        static string RuleID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ProductionDetailMaster"] = null;
                Session["ProcessParameterMaster"] = null;
                binDefineRuleDetails();
                getProductionDetail();
                getProcessParameterDetails();
            }

        }
        private List<ListItem> getProductionDetail()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                if (Session["ProductionDetailMaster"] == null)
                {
                    list = DataBaseAccess.getProductionMasterData();
                    Session["ProductionDetailMaster"] = list;
                }
                else
                {
                    list = Session["ProductionDetailMaster"] as List<ListItem>;
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        private List<ListItem> getProcessParameterDetails()
        {
            List<ListItem> list = new List<ListItem>();
            try
            {
                if (Session["ProcessParameterMaster"] == null)
                {
                    list = DataBaseAccess.getProcessParameterMasterData();
                    Session["ProcessParameterMaster"] = list;
                }
                else
                {
                    list = Session["ProcessParameterMaster"] as List<ListItem>;
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        private void binDefineRuleDetails()
        {
            try
            {
                List<DefineRuleDetails> defineRuleDetailsList = new List<DefineRuleDetails>();
                defineRuleDetailsList = DataBaseAccess.getDefineRuleDetails();
                if (defineRuleDetailsList != null && defineRuleDetailsList.Count > 0)
                {

                    gvDefineRule.DataSource = defineRuleDetailsList;
                    gvDefineRule.DataBind();
                    for (int i = 0; i < gvDefineRule.Rows.Count; i++)
                    {
                        string evaluateEvry = defineRuleDetailsList[i].EvaluateEveryUnit;
                        if (evaluateEvry == "EveryShift" || evaluateEvry == "EveryDay" || evaluateEvry == "HourlyShift")
                        {
                            (gvDefineRule.Rows[i].FindControl("lblEvaluateEvery") as Label).Text = evaluateEvry.Replace("0", "");
                        }
                    }
                }
                else
                {

                    DefineRuleDetails defineRule = new DefineRuleDetails();
                    defineRuleDetailsList.Add(defineRule);
                    gvDefineRule.DataSource = defineRuleDetailsList;
                    gvDefineRule.DataBind();
                    gvDefineRule.Rows[0].Visible = false;
                }
                ddlnewParameter_SelectedIndexChanged(null, null);
                //ClientScript.RegisterStartupScript(this.GetType(), "paramchange", "ParameterChange();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }

        protected void gvDefineRule_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDefineRule.EditIndex = e.NewEditIndex;
                List<DefineRuleDetails> defineRuleDetailsList = new List<DefineRuleDetails>();
                defineRuleDetailsList = DataBaseAccess.getDefineRuleDetails();
                gvDefineRule.DataSource = defineRuleDetailsList;
                gvDefineRule.DataBind();

                //DropDownList ddl = gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtCompare") as DropDownList;
                ////ddl.DataSource = DBAccess.getDownTimeCategory();
                ////ddl.DataBind();
                //ddl.SelectedValue = defineRuleDetailsList[e.NewEditIndex].Compare;
                //DropDownList ddl1 = gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtThresholdUnit") as DropDownList;
                //ddl1.SelectedValue = defineRuleDetailsList[e.NewEditIndex].ThresholdUnit;
                //DropDownList ddl2 = gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtEvaluateEvery") as DropDownList;
                //ddl2.SelectedValue = defineRuleDetailsList[e.NewEditIndex].EvaluateEveryUnit;
                DropDownList ddl3 = gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtApplyto") as DropDownList;
                ddl3.SelectedValue = defineRuleDetailsList[e.NewEditIndex].AppliesTo;
                // ddlnewParameter_SelectedIndexChanged(null, null);
                //ClientScript.RegisterStartupScript(this.GetType(), "paramchange", "ParameterChange();", true);






                string parametername = (gvDefineRule.Rows[e.NewEditIndex].FindControl("lbledtParameter") as Label).Text;
                if (parametername == "Production Detail" || parametername == "Cycle Start" || parametername == "Cycle Complete" || parametername == "Scheduled Components" || parametername == "Parts Count" || parametername == "Parts Count By CO" || parametername == "Top And Bottom OEE" || parametername == "Top Downtimes" || parametername == "High Performance Machines" || parametername == "Low Performance Machines" || parametername == "Tafe_Quality Status" || parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (AAAPL)")
                {

                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtThresholdUnit") as DropDownList).Visible = false;

                }
                else
                {

                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtThresholdUnit") as DropDownList).Visible = true;
                    DropDownList ddl1 = (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtThresholdUnit") as DropDownList);
                    List<string> ThresoldUnitList = getThresholdUnitList(parametername);
                    ddl1.DataSource = ThresoldUnitList;
                    ddl1.DataBind();
                    ddl1.SelectedValue = defineRuleDetailsList[e.NewEditIndex].ThresholdUnit;
                }


                if (parametername == "Production Detail" || parametername == "Cycle Start" || parametername == "Cycle Complete" || parametername == "Scheduled Components" || parametername == "Top And Bottom OEE" || parametername == "Top Downtimes" || parametername == "High Performance Machines" || parametername == "Low Performance Machines" || parametername == "Tafe_Quality Status" || parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (AAAPL)" || parametername == "Process Parameter Alert")
                {
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtCompare") as DropDownList).Visible = false;
                }
                else
                {
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtCompare") as DropDownList).Visible = true;
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtCompare") as DropDownList).SelectedValue = defineRuleDetailsList[e.NewEditIndex].Compare;
                }

                if (parametername == "Production Detail" || parametername == "Cycle Start" || parametername == "Cycle Complete" || parametername == "Scheduled Components" || parametername == "Tafe_Quality Status" || parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (AAAPL)")
                {
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("txtedtThresold") as TextBox).Visible = false;
                }
                else
                {
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("txtedtThresold") as TextBox).Visible = true;
                }

                if (parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (Shanti)" || parametername == "PM (AAAPL)" || parametername == "Process Parameter Alert")
                {
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtEvaluateEvery") as DropDownList).Visible = false;
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("txtedtEvaluateEvery") as TextBox).Visible = false;
                }
                else
                {
                    List<string> evaluateEveryList = getevaluateEveryList(parametername);
                    DropDownList ddl = (gvDefineRule.Rows[e.NewEditIndex].FindControl("ddledtEvaluateEvery") as DropDownList);
                    ddl.Visible = true;
                    ddl.DataSource = evaluateEveryList;
                    ddl.DataBind();
                    ddl.SelectedValue = defineRuleDetailsList[e.NewEditIndex].EvaluateEveryUnit;
                    ddledtEvaluateEveryChange(e.NewEditIndex);
                }

                if (parametername == "Production Detail" || parametername == "Process Parameter Alert")
                {
                    string ruleid = (gvDefineRule.Rows[e.NewEditIndex].FindControl("lblruleid") as Label).Text;
                    string param = "";
                    List<ListItem> masterlist = new List<ListItem>(); ;
                    if (parametername == "Production Detail")
                    {
                        param = "ProdDetailColTrans";
                        masterlist = getProductionDetail();
                    }
                    else if (parametername == "Process Parameter Alert")
                    {
                        param = "ProcessParameterTrans";
                        masterlist = getProcessParameterDetails();
                    }
                    List<ListItem> list = DataBaseAccess.getProductionAndProcessParameterDetailsForRule(ruleid, param);
                    ListBox listBox = (gvDefineRule.Rows[e.NewEditIndex].FindControl("lbProductionDetails") as ListBox);
                    listBox.Visible = true;
                    listBox.DataSource = masterlist;
                    listBox.DataTextField = "Text";
                    listBox.DataValueField = "Value";
                    listBox.DataBind();
                    foreach (ListItem item in listBox.Items)
                    {
                        if (list.Exists(k => k.Value == item.Value))
                        {
                            item.Selected = true;
                        }
                    }
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("txtedtMsg") as TextBox).Visible = false;
                }
                else
                {
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("lbProductionDetails") as ListBox).Visible = false;
                    (gvDefineRule.Rows[e.NewEditIndex].FindControl("txtedtMsg") as TextBox).Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }

        protected void gvDefineRule_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                RuleID = (gvDefineRule.Rows[e.RowIndex].FindControl("lblruleid") as Label).Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "DelParameters", "openConfirmModal();", true);
                // ClientScript.RegisterStartupScript(this.GetType(), "paramchange", "ParameterChange();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }

        protected void gvDefineRule_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DefineRuleDetails defineRuleDetails = new DefineRuleDetails();
                defineRuleDetails.RuleId = (gvDefineRule.Rows[e.RowIndex].FindControl("lblruleid") as Label).Text;
                defineRuleDetails.Description = (gvDefineRule.Rows[e.RowIndex].FindControl("txtedtDesc") as TextBox).Text;
                defineRuleDetails.Parameter = (gvDefineRule.Rows[e.RowIndex].FindControl("lbledtParameter") as Label).Text;
                DropDownList ddl = (gvDefineRule.Rows[e.RowIndex].FindControl("ddledtCompare") as DropDownList);
                if (ddl.Visible == true)
                {
                    defineRuleDetails.Compare = ddl.SelectedItem == null ? "" : ddl.SelectedItem.ToString();
                }
                else
                {
                    defineRuleDetails.Compare = "";
                }

                if ((gvDefineRule.Rows[e.RowIndex].FindControl("txtedtThresold") as TextBox).Visible == true)
                {
                    defineRuleDetails.Threshold = (gvDefineRule.Rows[e.RowIndex].FindControl("txtedtThresold") as TextBox).Text;
                }
                else
                {
                    defineRuleDetails.Threshold = "";
                }
                DropDownList ddl1 = (gvDefineRule.Rows[e.RowIndex].FindControl("ddledtThresholdUnit") as DropDownList);
                if (ddl1.Visible == true)
                {
                    defineRuleDetails.ThresholdUnit = ddl1.SelectedItem == null ? "" : ddl1.SelectedItem.ToString();
                }
                else
                {
                    defineRuleDetails.ThresholdUnit = "";
                }

                if ((gvDefineRule.Rows[e.RowIndex].FindControl("txtedtEvaluateEvery") as TextBox).Visible == true)
                {
                    defineRuleDetails.EvaluateEvery = (gvDefineRule.Rows[e.RowIndex].FindControl("txtedtEvaluateEvery") as TextBox).Text;
                }
                else
                {
                    defineRuleDetails.EvaluateEvery = "";
                }
                DropDownList ddl2 = (gvDefineRule.Rows[e.RowIndex].FindControl("ddledtEvaluateEvery") as DropDownList);
                if (ddl2.Visible == true)
                {
                    defineRuleDetails.EvaluateEveryUnit = ddl2.SelectedItem == null ? "" : ddl2.SelectedItem.ToString();
                }
                else
                {
                    defineRuleDetails.EvaluateEveryUnit = "";
                }

                defineRuleDetails.Enable = (gvDefineRule.Rows[e.RowIndex].FindControl("cbedtEnable") as CheckBox).Checked;
                DropDownList ddl3 = (gvDefineRule.Rows[e.RowIndex].FindControl("ddledtApplyto") as DropDownList);
                defineRuleDetails.AppliesTo = ddl3.SelectedItem == null ? "" : ddl3.SelectedItem.ToString();
                defineRuleDetails.SMSEnable = (gvDefineRule.Rows[e.RowIndex].FindControl("cbedtSMSEnable") as CheckBox).Checked;
                defineRuleDetails.EmailEnable = (gvDefineRule.Rows[e.RowIndex].FindControl("cbedtEmailEnable") as CheckBox).Checked;
                ListBox listBox = (gvDefineRule.Rows[e.RowIndex].FindControl("lbProductionDetails") as ListBox);
                defineRuleDetails.TelegramEnabled = (gvDefineRule.Rows[e.RowIndex].FindControl("cbedtTelegramEnable") as CheckBox).Checked;
                defineRuleDetails.MobileEnabled = (gvDefineRule.Rows[e.RowIndex].FindControl("cbedtMobileEnable") as CheckBox).Checked;
                List<ListItem> selectedProductionData = new List<ListItem>();
                if (listBox.Visible)
                {
                    string message = "";
                    int msgFlag = 0;
                    foreach (ListItem item in listBox.Items)
                    {
                        if (item.Selected)
                        {
                            if (msgFlag == 0)
                            {
                                if (message == "")
                                {
                                    message += item.Text;
                                }
                                else
                                {
                                    message += "," + item.Text + "...";
                                    msgFlag = 1;
                                }
                            }
                            selectedProductionData.Add(new ListItem() { Text = item.Text, Value = item.Value });
                        }
                    }
                    defineRuleDetails.Message = message;
                }
                else
                {
                    defineRuleDetails.Message = (gvDefineRule.Rows[e.RowIndex].FindControl("txtedtMsg") as TextBox).Text;
                }
                int success = DataBaseAccess.InsertUpdateDefineRuleDetails(defineRuleDetails, "Update");
                if (success > 0)
                {
                    if (listBox.Visible)
                    {
                        string param = "";
                        if (defineRuleDetails.Parameter == "Production Detail")
                        {
                            param = "ProdDetailColTrans";
                        }
                        else if (defineRuleDetails.Parameter == "Process Parameter Alert")
                        {
                            param = "ProcessParameterTrans";
                        }
                        DataBaseAccess.saveProdtionDetailsForRuleID(selectedProductionData, defineRuleDetails.RuleId, param);
                    }
                    gvDefineRule.EditIndex = -1;
                    binDefineRuleDetails();
                }
                else if (success == -1)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Rule ID not exists.";
                    // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to update the record.";
                    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }

        protected void gvDefineRule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDefineRule.EditIndex = -1;
            binDefineRuleDetails();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DefineRuleDetails defineRuleDetails = new DefineRuleDetails();
                defineRuleDetails.RuleId = (gvDefineRule.FooterRow.FindControl("txtnewRuleID") as TextBox).Text;
                defineRuleDetails.Description = (gvDefineRule.FooterRow.FindControl("txtnewDesc") as TextBox).Text;
                DropDownList ddlParam = (gvDefineRule.FooterRow.FindControl("ddlnewParameter") as DropDownList);
                defineRuleDetails.Parameter = ddlParam.SelectedValue == null ? "" : ddlParam.SelectedValue.ToString();
                //if ((gvDefineRule.FooterRow.FindControl("hfcontrolnull") as HiddenField).Value == "0")
                //{
                DropDownList ddl = (gvDefineRule.FooterRow.FindControl("ddlnewCompre") as DropDownList);
                if (ddl.Visible == true)
                {
                    defineRuleDetails.Compare = ddl.SelectedItem == null ? "" : ddl.SelectedItem.ToString();
                }
                else
                {
                    defineRuleDetails.Compare = "";
                }

                DropDownList ddl1 = (gvDefineRule.FooterRow.FindControl("ddlnewThresholdUnit") as DropDownList);
                if (ddl1.Visible == true)
                {
                    defineRuleDetails.ThresholdUnit = ddl1.SelectedItem == null ? "" : ddl1.SelectedItem.ToString();
                }
                else
                {
                    defineRuleDetails.ThresholdUnit = "";
                }

                // }
                //else
                //{
                //    defineRuleDetails.Compare = "";
                //    defineRuleDetails.ThresholdUnit = "";
                //}
                defineRuleDetails.Threshold = (gvDefineRule.FooterRow.FindControl("txtnewThresold") as TextBox).Text;

                defineRuleDetails.EvaluateEvery = (gvDefineRule.FooterRow.FindControl("txtnewEvaluateEvery") as TextBox).Text;
                DropDownList ddl2 = (gvDefineRule.FooterRow.FindControl("ddlnewEvaluateEvery") as DropDownList);
                if (ddl2.Visible == true)
                {
                    defineRuleDetails.EvaluateEveryUnit = ddl2.SelectedItem == null ? "" : ddl2.SelectedItem.ToString();
                }
                else
                {
                    defineRuleDetails.EvaluateEveryUnit = "";
                }

                defineRuleDetails.Enable = (gvDefineRule.FooterRow.FindControl("cbnewEnable") as CheckBox).Checked;
                DropDownList ddl3 = (gvDefineRule.FooterRow.FindControl("ddlnewApplyto") as DropDownList);
                defineRuleDetails.AppliesTo = ddl3.SelectedItem == null ? "" : ddl3.SelectedItem.ToString();
                defineRuleDetails.SMSEnable = (gvDefineRule.FooterRow.FindControl("cbnewSMSEnable") as CheckBox).Checked;
                defineRuleDetails.EmailEnable = (gvDefineRule.FooterRow.FindControl("cbnewEmailEnable") as CheckBox).Checked;
                defineRuleDetails.Message = (gvDefineRule.FooterRow.FindControl("txtnewMSg") as TextBox).Text;
                defineRuleDetails.TelegramEnabled = (gvDefineRule.FooterRow.FindControl("cbnewTelegramEnable") as CheckBox).Checked;
                defineRuleDetails.MobileEnabled = (gvDefineRule.FooterRow.FindControl("cbnewMobileEnable") as CheckBox).Checked;
                //if ((gvDefineRule.FooterRow.FindControl("hfcontrolnull") as HiddenField).Value == "0")
                //{
                //    if (defineRuleDetails.RuleId == "" || defineRuleDetails.Parameter == "" || defineRuleDetails.Compare == "" || defineRuleDetails.Threshold == "" || defineRuleDetails.ThresholdUnit == "" || defineRuleDetails.EvaluateEvery == "" || defineRuleDetails.EvaluateEveryUnit == "")
                //    {
                //        lblMessages.ForeColor = System.Drawing.Color.Red;
                //        lblMessages.Text = "Rule Id, Parameter, Compare, Threshold, Evaluate Every are required.";
                //        return;
                //    }
                //}
                //else
                //{
                //    if (defineRuleDetails.RuleId == "" || defineRuleDetails.Parameter == "" || defineRuleDetails.Threshold == "" || defineRuleDetails.EvaluateEvery == "" || defineRuleDetails.EvaluateEveryUnit == "")
                //    {
                //        lblMessages.ForeColor = System.Drawing.Color.Red;
                //        lblMessages.Text = "Rule Id, Parameter, Threshold, Evaluate Every are required.";
                //        return;
                //    }
                //}


                if (defineRuleDetails.RuleId == "" || defineRuleDetails.Parameter == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Rule Id, Parameter fileds are required.";
                    return;
                }
                ListBox listBox = (gvDefineRule.FooterRow.FindControl("lbProductionDetails") as ListBox);
                List<ListItem> selectedProductionData = new List<ListItem>();
                if (listBox.Visible)
                {
                    string message = "";
                    int msgFlag = 0;
                    foreach (ListItem item in listBox.Items)
                    {
                        if (item.Selected)
                        {
                            if (msgFlag == 0)
                            {
                                if (message == "")
                                {
                                    message += item.Text;
                                }
                                else
                                {
                                    message += "," + item.Text + "...";
                                    msgFlag = 1;
                                }
                            }
                            selectedProductionData.Add(new ListItem() { Text = item.Text, Value = item.Value });
                        }
                    }
                    defineRuleDetails.Message = message;
                }
                else
                {
                    defineRuleDetails.Message = (gvDefineRule.FooterRow.FindControl("txtnewMSg") as TextBox).Text;
                }
                int success = DataBaseAccess.InsertUpdateDefineRuleDetails(defineRuleDetails, "Insert");
                if (success > 0)
                {
                    if (listBox.Visible)
                    {
                        string param = "";
                        if (defineRuleDetails.Parameter == "Production Detail")
                        {
                            param = "ProdDetailColTrans";
                        }
                        else if (defineRuleDetails.Parameter == "Process Parameter Alert")
                        {
                            param = "ProcessParameterTrans";
                        }
                        DataBaseAccess.saveProdtionDetailsForRuleID(selectedProductionData, defineRuleDetails.RuleId, param);
                    }
                    binDefineRuleDetails();
                }
                else if (success == -2)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Rule Id already exists.";
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to insert new record.";
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;

            }
        }

        protected void yesGridDeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int sucess = DataBaseAccess.deleteRuleId(RuleID);
                if (sucess > 0)
                {
                    binDefineRuleDetails();
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to delete the record.";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlnewParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (gvDefineRule.ShowFooter == true)
                {
                    string parametername = (gvDefineRule.FooterRow.FindControl("ddlnewParameter") as DropDownList).SelectedValue == null ? "" : (gvDefineRule.FooterRow.FindControl("ddlnewParameter") as DropDownList).SelectedValue.ToString();
                    if (parametername == "Production Detail" || parametername == "Cycle Start" || parametername == "Cycle Complete" || parametername == "Scheduled Components" || parametername == "Parts Count" || parametername == "Parts Count By CO" || parametername == "Top And Bottom OEE" || parametername == "Top Downtimes" || parametername == "High Performance Machines" || parametername == "Low Performance Machines" || parametername == "Tafe_Quality Status" || parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (AAAPL)")
                    {

                        (gvDefineRule.FooterRow.FindControl("ddlnewThresholdUnit") as DropDownList).Visible = false;

                    }
                    else
                    {

                        (gvDefineRule.FooterRow.FindControl("ddlnewThresholdUnit") as DropDownList).Visible = true;
                        DropDownList ddl1 = (gvDefineRule.FooterRow.FindControl("ddlnewThresholdUnit") as DropDownList);
                        List<string> ThresoldUnitList = getThresholdUnitList(parametername);
                        ddl1.DataSource = ThresoldUnitList;
                        ddl1.DataBind();
                    }


                    if (parametername == "Production Detail" || parametername == "Cycle Start" || parametername == "Cycle Complete" || parametername == "Scheduled Components" || parametername == "Top And Bottom OEE" || parametername == "Top Downtimes" || parametername == "High Performance Machines" || parametername == "Low Performance Machines" || parametername == "Tafe_Quality Status" || parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (AAAPL)" || parametername == "Process Parameter Alert")
                    {
                        //$(val).closest('tr').find("#ddlnewComparediv").css('display', 'none');
                        //$(val).closest('tr').find("#ddlnewThresholdUnitdiv").css('display', 'none');
                        //$(val).closest('tr').find("#hfcontrolnull").val("1");
                        (gvDefineRule.FooterRow.FindControl("ddlnewCompre") as DropDownList).Visible = false;
                        (gvDefineRule.FooterRow.FindControl("hfcontrolnull") as HiddenField).Value = "1";

                    }
                    else
                    {
                        //$(val).closest('tr').find("#ddlnewComparediv").css('display', 'block');
                        //$(val).closest('tr').find("#ddlnewThresholdUnitdiv").css('display', 'block');
                        // $(val).closest('tr').find("#hfcontrolnull").val("0");
                        (gvDefineRule.FooterRow.FindControl("ddlnewCompre") as DropDownList).Visible = true;
                        (gvDefineRule.FooterRow.FindControl("hfcontrolnull") as HiddenField).Value = "0";
                    }

                    if (parametername == "Production Detail" || parametername == "Cycle Start" || parametername == "Cycle Complete" || parametername == "Scheduled Components" || parametername == "Tafe_Quality Status" || parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (AAAPL)")
                    {
                        (gvDefineRule.FooterRow.FindControl("txtnewThresold") as TextBox).Visible = false;
                    }
                    else
                    {
                        (gvDefineRule.FooterRow.FindControl("txtnewThresold") as TextBox).Visible = true;
                    }

                    if (parametername == "Audit Reminder To Supervisor" || parametername == "Audit Reminder To ProdHead" || parametername == "Bypass End Notification" || parametername == "PM (Shanti)" || parametername == "PM (AAAPL)" || parametername == "Process Parameter Alert")
                    {
                        (gvDefineRule.FooterRow.FindControl("txtnewEvaluateEvery") as TextBox).Visible = false;
                        (gvDefineRule.FooterRow.FindControl("ddlnewEvaluateEvery") as DropDownList).Visible = false;
                    }
                    else
                    {
                        List<string> evaluateEveryList = getevaluateEveryList(parametername);
                        DropDownList ddl = (gvDefineRule.FooterRow.FindControl("ddlnewEvaluateEvery") as DropDownList);
                        ddl.Visible = true;
                        ddl.DataSource = evaluateEveryList;
                        ddl.DataBind();
                        ddlnewEvaluateEvery_SelectedIndexChanged(null, null);

                    }
                    if (parametername == "Production Detail" || parametername == "Process Parameter Alert")
                    {
                        (gvDefineRule.FooterRow.FindControl("lbProductionDetails") as ListBox).Visible = true;
                        (gvDefineRule.FooterRow.FindControl("txtnewMSg") as TextBox).Visible = false;
                        List<ListItem> list = new List<ListItem>();
                        if (parametername == "Production Detail")
                        {
                            list = getProductionDetail();
                        }
                        else if (parametername == "Process Parameter Alert")
                        {
                            list = getProcessParameterDetails();
                        }
                        ListBox listBox = gvDefineRule.FooterRow.FindControl("lbProductionDetails") as ListBox;
                        listBox.DataSource = list;
                        listBox.DataTextField = "Text";
                        listBox.DataValueField = "Value";
                        listBox.DataBind();
                    }
                    else
                    {
                        (gvDefineRule.FooterRow.FindControl("lbProductionDetails") as ListBox).Visible = false;
                        (gvDefineRule.FooterRow.FindControl("txtnewMSg") as TextBox).Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }

        }
        private List<string> getThresholdUnitList(string parameterName)
        {
            List<string> list = new List<string>();
            try
            {

                switch (parameterName)
                {
                    case "Ping Status":
                        list.Add("min");
                        list.Add("Hour");
                        break;
                    case "Breakdown Notification":
                        list.Add("min");
                        list.Add("Hour");
                        break;
                    case "Tafe_Quality Status":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Cycle Start After Down":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "JH Checklist Status":
                        list.Add("min");
                        list.Add("Hours");
                        list.Add("Days");
                        list.Add("Shift");
                        break;
                    case "PM Notification":
                        list.Add("%");
                        break;
                    case "PM (Shanti)":
                        list.Add("%");
                        break;
                    case "Downtime Notification":
                        list.Add("min");
                        list.Add("Hour");
                        break;
                    case "Down Start":
                        list.Add("min");
                        list.Add("Hour");
                        break;
                    case "Down End":
                        list.Add("min");
                        list.Add("Hour");
                        break;
                    case "Process Parameter Alert":
                        list.Add("Seconds");
                        list.Add("min");
                        list.Add("Hours");
                        list.Add("Days");
                        list.Add("Shift");
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        private List<string> getevaluateEveryList(string parameterName)
        {
            List<string> list = new List<string>();
            try
            {

                switch (parameterName)
                {
                    case "Parts Count":
                        list.Add("min");
                        list.Add("Hours");
                        list.Add("EveryShift");
                        list.Add("HourlyShift");
                        break;
                    case "Ping Status":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Production Detail":
                        list.Add("min");
                        list.Add("Hours");
                        list.Add("EveryShift");
                        break;
                    case "Parts Count By CO":
                        list.Add("EveryShift");
                        list.Add("HourlyShift");
                        break;
                    case "Cycle Start":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Cycle Complete":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Scheduled Components":
                        list.Add("EveryDay");
                        list.Add("EveryShift");
                        break;
                    case "Top Downtimes":
                        list.Add("EveryDay");
                        list.Add("EveryShift");
                        break;
                    case "High Performance Machines":
                        list.Add("EveryDay");
                        list.Add("EveryShift");
                        break;
                    case "Low Performance Machines":
                        list.Add("EveryDay");
                        list.Add("EveryShift");
                        break;
                    case "Breakdown Notification":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Tafe_Quality Status":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Cycle Start After Down":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "JH Checklist Status":
                        list.Add("min");
                        list.Add("Hours");
                        list.Add("EveryShift");
                        break;
                    case "PM Notification":
                        list.Add("min");
                        list.Add("Hours");
                        list.Add("EveryShift");
                        break;
                    case "Downtime Notification":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Down Start":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                    case "Down End":
                        list.Add("min");
                        list.Add("Hours");
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        protected void ddlnewEvaluateEvery_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string evaluateEvry = (gvDefineRule.FooterRow.FindControl("ddlnewEvaluateEvery") as DropDownList).SelectedItem == null ? "" : (gvDefineRule.FooterRow.FindControl("ddlnewEvaluateEvery") as DropDownList).SelectedItem.ToString();
                if (evaluateEvry == "EveryShift" || evaluateEvry == "EveryDay" || evaluateEvry == "HourlyShift")
                {
                    (gvDefineRule.FooterRow.FindControl("txtnewEvaluateEvery") as TextBox).Visible = false;
                }
                else
                {
                    (gvDefineRule.FooterRow.FindControl("txtnewEvaluateEvery") as TextBox).Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddledtEvaluateEvery_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((sender as DropDownList).NamingContainer as GridViewRow).RowIndex);
                ddledtEvaluateEveryChange(index);
            }
            catch (Exception ex)
            {

            }
        }
        private void ddledtEvaluateEveryChange(int index)
        {
            try
            {
                string evaluateEvry = (gvDefineRule.Rows[index].FindControl("ddledtEvaluateEvery") as DropDownList).SelectedItem == null ? "" : (gvDefineRule.Rows[index].FindControl("ddledtEvaluateEvery") as DropDownList).SelectedItem.ToString();
                if (evaluateEvry == "EveryShift" || evaluateEvry == "EveryDay" || evaluateEvry == "HourlyShift")
                {
                    (gvDefineRule.Rows[index].FindControl("txtedtEvaluateEvery") as TextBox).Visible = false;
                }
                else
                {
                    (gvDefineRule.Rows[index].FindControl("txtedtEvaluateEvery") as TextBox).Visible = true;
                }
            }
            catch (Exception ex)
            {

            }


        }

        protected void gvDefineRule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    var ddlParameter = (e.Row.FindControl("ddlnewParameter") as DropDownList);
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["LnTOdishaPages"].ToString() == "1")
                    {
                        ddlParameter.Items.Add(new ListItem() { Text = "PM Plan (LnT Odisha)", Value = "PM Plan LnT" });
                        ddlParameter.Items.Add(new ListItem() { Text = "PM Pending (LnT Odisha)", Value = "PM Pending LnT" });
                    }
                    string parameter = (e.Row.FindControl("ddlnewParameter") as DropDownList).SelectedValue;
                    if (parameter == "Production Detail" || parameter == "Process Parameter Alert")
                    {
                        (e.Row.FindControl("lbProductionDetails") as ListBox).Visible = true;
                        (e.Row.FindControl("txtnewMSg") as TextBox).Visible = false;
                    }
                    else
                    {
                        (e.Row.FindControl("lbProductionDetails") as ListBox).Visible = false;
                        (e.Row.FindControl("txtnewMSg") as TextBox).Visible = true;
                    }
                    //ListBox listBox = e.Row.FindControl("lbProductionDetails") as ListBox;
                    //listBox.DataSource = getProductionDetail();
                    //listBox.DataTextField = "Text";
                    //listBox.DataValueField = "Value";
                    //listBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}