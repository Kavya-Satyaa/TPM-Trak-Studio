using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class POStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                    txtToDate.Text= DateTime.Now.ToString("dd-MM-yyyy");
                    //BindPlant();
                    //BindShift();
                    BindPONumber();
                    BindData(txtFromDate.Text, txtToDate.Text,ddlPONumber.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        //private void BindPlant()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
        //        ddlPlant.DataSource = list;
        //        ddlPlant.DataBind();

        //        BindMachine();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindPlant: " + ex.Message);
        //    }
        //}
        //private void BindMachine()
        //{
        //    try
        //    {
        //        List<string> list = CumiDBAccess.GetAllMachinedByPlant(ddlPlant.SelectedValue);
        //        lbMachine.DataSource = list;
        //        lbMachine.DataBind();
        //        for (int i = 0; i < lbMachine.Items.Count; i++)
        //        {
        //            lbMachine.Items[i].Selected = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindMachine: " + ex.Message);
        //    }
        //}
        //private void BindShift()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.GetAllShifts("");
        //        if (list.Contains("All")) list.Remove("All");
        //        lbShift.DataSource = list;
        //        lbShift.DataBind();

        //        for (int i = 0; i < lbShift.Items.Count; i++)
        //        {
        //            lbShift.Items[i].Selected = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog("BindShift: " + ex.Message);
        //    }
        //}
        private void BindPONumber()
        {
            try
            {
                List<string> POnumberLst = DataBaseAccess.GetAllPONumber(Util.GetDateTime(txtFromDate.Text), Util.GetDateTime(txtToDate.Text),"");
                if (POnumberLst.Count > 0)
                {
                    POnumberLst.Insert(0, "All");
                }
                ddlPONumber.DataSource = POnumberLst;
                ddlPONumber.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPONumber: " + ex.Message);
            }
        }
        //protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindMachine();
        //}
       
        //protected void txtFromDate_TextChanged(object sender, EventArgs e)
        //{
        //    BindPONumber();
        //}

        //protected void txtToDate_TextChanged(object sender, EventArgs e)
        //{
        //     BindPONumber();
        //}

        private void BindData(string fromDate, string ToDate, string PONumber)
        {
            try
            {
                //if(ddlPONumber.Items.Count.Equals(0))
                //{
                //    lvPOStatus.DataSource = new List<ProcessParameterEntity>();
                //    lvPOStatus.DataBind();
                //    return;
                //}
                //string machine = String.Join(",", (lbMachine.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray()));
                //string shift = String.Join(",", (lbShift.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray()));
                
                List<POStatusEntity> list = CumiDBAccess.GetPOStatusReport(PONumber, fromDate, ToDate);
                if (list.Count > 0)
                {
                    lvPOStatus.DataSource = list;
                    lvPOStatus.DataBind();
                }
                else
                {
                    lvPOStatus.DataSource = new List<ProcessParameterEntity>();
                    lvPOStatus.DataBind();
                }
                Session["POStatus"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData("", "", ddlPONumber.SelectedValue);
            hdnViewType.Value = "PONumberView";
            txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        protected void btnDateView_Click(object sender, EventArgs e)
        {
            BindData(txtFromDate.Text, txtToDate.Text, "");
            hdnViewType.Value = "DateView";
            txtPONumberSearch.Text = "";
            btnPOSearch_Click(null, null);
        }
        protected void btnPOSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> PONumber = new List<string>();
                if (txtPONumberSearch != null)
                {
                    if (!(string.IsNullOrEmpty(txtPONumberSearch.Text)) || !(string.IsNullOrEmpty(txtFromDate.Text)))
                    {
                        DateTime fromdate = Util.GetDateTime(txtFromDate.Text);
                        DateTime todate = Util.GetDateTime(txtToDate.Text);
                        PONumber = DataBaseAccess.GetAllPONumber(fromdate, todate, txtPONumberSearch.Text.ToString());
                        if (PONumber != null && PONumber.Count > 0)
                        {
                            ddlPONumber.DataSource = PONumber;
                            ddlPONumber.DataBind();
                            if (string.IsNullOrEmpty(txtPONumberSearch.Text))
                                ddlPONumber.Items.Insert(0, new ListItem
                                {
                                    Text = "All",
                                    Value = "All"
                                });
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No PO Number Found or No Production planned for the Given Search')", true);
                            txtPONumberSearch.Text = "";
                            BindPONumber();
                        }
                    }
                    else
                    {
                        BindPONumber();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void btnPoClear_Click(object sender, EventArgs e)
        {
            txtPONumberSearch.Text = "";
            btnPOSearch_Click(null, null);
        }
        protected void txtFromDate_TextChanged1(object sender, EventArgs e)
        {
            //BindPONumber();
        }

        protected void txtToDate_TextChanged1(object sender, EventArgs e)
        {
            //BindPONumber();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["POStatus"] == null)
                {
                    return;
                }
                
                //string machine = String.Join(",", (lbMachine.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray()));
                //string shift = String.Join(",", (lbShift.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Value).ToArray()));
                //if (machine.Split(',').Count().Equals(lbMachine.Items.Count))
                //    machine = "All";
                List<POStatusEntity> list = Session["POStatus"] as List<POStatusEntity>;
                string FromDate = string.Empty;
                string ToDate = string.Empty;
                string PONumber = string.Empty;
                if (hdnViewType.Value.Equals("DateView", StringComparison.OrdinalIgnoreCase))
                {
                    FromDate = txtFromDate.Text;
                    ToDate = txtToDate.Text;
                }
                else
                    PONumber = ddlPONumber.SelectedValue;
                CumiGenerateReport.GenerateCumiPOStatusReport(list, FromDate, ToDate, PONumber, hdnViewType.Value);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("ExportProcessParameter: " + ex.Message);
            }
        }

      
    }
}