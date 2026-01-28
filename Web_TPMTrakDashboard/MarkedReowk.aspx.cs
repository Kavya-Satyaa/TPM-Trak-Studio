using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class MarkedReowk : System.Web.UI.Page
    {
        bool rowadded = false;
        int sl_no;
        string rejrew; bool rowAdded = false;
        string rejqty, date, shift, plant, machine, component, opn, opr, repeatCycle, dummycycle, reworkperformed;
        string rectionId;

        protected void Page_Load(object sender, EventArgs e)
        {
            //rework.Visible = false;
            if (!IsPostBack)
            {
                if (Request.QueryString["rectionId"] != "")
                {
                    rectionId = Request.QueryString["rectionId"].ToString();
                    rejrew = Request.QueryString["rejectionValue"].ToString();
                    Session["date"] = date = Request.QueryString["Date"].ToString();
                    Session["shift"] = shift = Request.QueryString["shift"].ToString();
                    Session["plant"] = plant = Request.QueryString["PlantID"].ToString();
                    Session["machine"] = machine = Request.QueryString["machine"].ToString();
                    Session["component"] = component = Request.QueryString["component"].ToString();
                    Session["opn"] = opn = Request.QueryString["opn"].ToString();
                    Session["opr"] = opr = Request.QueryString["Opr"].ToString();
                    Session["repeatCycle"] = repeatCycle = Request.QueryString["RepeatCycles"].ToString();
                    Session["dummycycle"] = dummycycle = Request.QueryString["dummyCycles"].ToString();
                    Session["reworkperformed"] = reworkperformed = Request.QueryString["reworkperformed"].ToString();
                    Session["rejqty"] = rejqty = Request.QueryString["rejectionQty"].ToString();
                    hdnMarkedForRework.Value = Request.QueryString["markedForRework"].ToString();
                    hdnAcceptedParts.Value = Request.QueryString["rejectionQty"].ToString();
                    hdnrejectionTotalValue.Value = Request.QueryString["rejectionTotalValue"].ToString();
                }
                getdata();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            //JobCard.getGridData();
        }

        private void getdata()
        {
            try
            {
                DataTable dtRejection = new DataTable();
                DataTable dtRework = new DataTable();
                ddlreworkcat.DataSource = DataBaseAccess.GetAllRejectionRework("Rework", string.Empty);
                ddlreasoncat.DataSource = DataBaseAccess.GetAllRejectionRework("Rejection", string.Empty);
                ddlreworkcat.DataBind();
                ddlreasoncat.DataBind();
                if (ddlreworkcat.Items.Count > 0)
                {
                    ddlreworkcat.SelectedIndex = 0;
                    ddlreworkreason.DataSource = DataBaseAccess.GetAllRejectionRework("Rework", ddlreworkcat.Text.ToString());
                    ddlreworkreason.DataBind();
                }
                if (ddlreasoncat.Items.Count > 0)
                {
                    ddlreasoncat.SelectedIndex = 0;
                    ddlreason.DataSource = DataBaseAccess.GetAllRejectionRework("Rejection", ddlreasoncat.Text.ToString());
                    ddlreason.DataBind();
                }
                dtRejection = DataBaseAccess.GetJobCardDetails(DateTime.Now, "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", Convert.ToInt32(rectionId), "ShiftRejectionDetails", "", "", "", "", out rowadded,0);
                if (dtRejection != null)
                {
                    grdreason.DataSource = dtRejection;
                    grdreason.DataBind();
                }

                dtRework = DataBaseAccess.GetJobCardDetails(DateTime.Now, "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", Convert.ToInt32(rectionId), "ShiftreworkDetails", "", "", "", "", out rowadded,0);
                if (dtRework != null)
                {
                    grdmarkrework.DataSource = dtRework;
                    grdmarkrework.DataBind();
                }
                if (rejrew == "rework")
                {
                    reasondiv.Visible = false;
                    rework.Visible = true;
                    rejrew = string.Empty;


                }
                else if (rejrew == "rejection")
                {
                    rework.Visible = false;
                    reasondiv.Visible = true;
                    rejrew = string.Empty;

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }
        }

        protected void btnaddrework_Click(object sender, EventArgs e)
        {
            try
            {
                rectionId = Request.QueryString["rectionId"].ToString();
                rejrew = Request.QueryString["rejectionValue"].ToString();
                int qty = 0;
                if (Session["qty"] != null)
                {
                    int.TryParse(Session["qty"].ToString(), out qty);
                }
                if (ddlreworkcat.Items.Count < 0) return;

                if (string.IsNullOrEmpty(txtqty.Text.ToString()))
                {
                    lblMessages.Text = "Rework Qty cannot be empty!!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtqty.Text.Trim('0')))
                {
                    lblMessages.Text = "Rework Qty should be greater than zero!!";
                    return;
                }
                int updatedtext = 0; int total = 0;
                int.TryParse(txtqty.Text.ToString(), out updatedtext);
                for (int k = 0; k < grdmarkrework.Rows.Count; k++)
                {
                    int val = 0;
                    int.TryParse((grdmarkrework.Rows[k].FindControl("lblReworkQty") as Label).Text, out val);
                    total += val;
                }
                total += updatedtext;
                //if (total > qty)
                //{
                //    lblMessages.Text = "Rejection Qty cannot be greater than accepted parts!!";
                //    return;
                //}
                int acceptedParts = hdnAcceptedParts.Value != "" ? Convert.ToInt32(hdnAcceptedParts.Value) : 0;
                int rejectionValue = hdnrejectionTotalValue.Value != "" ? Convert.ToInt32(hdnrejectionTotalValue.Value) : 0;
                int calCount = acceptedParts - rejectionValue;
                if (total > calCount)
                {
                    lblMessages.Text = "Rework Qty cannot be greater than accepted parts!!";
                    return;
                }
                DataBaseAccess.insertRejRwkDetails(Convert.ToInt32(txtqty.Text.ToString()), ddlreworkcat.SelectedValue, "PCT", DateTime.Now, Convert.ToInt32(rectionId), 0, "SaveRework", out rowadded);
                if (rowadded)
                {
                    updateProductionData("Rework","");
                    getdata();
                    lblMessages.Text = "Details added Successfully";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnupdaterework_Click(object sender, EventArgs e)
        {
            try
            {
                string cell_1_Value = string.Empty;
                int i = grdmarkrework.SelectedIndex;
                if (i >= 0)
                    cell_1_Value = (grdmarkrework.Rows[i].FindControl("lblSlno") as Label).Text; // grdmarkrework.Rows[i].Cells[0].Text;
                else
                    cell_1_Value = "0";
                rectionId = Request.QueryString["rectionId"].ToString();
                rejrew = Request.QueryString["rejectionValue"].ToString();
                int total = 0, updatedtext = 0;
                int qty = 0;
                if (Session["qty"] != null)
                {
                    int.TryParse(Session["qty"].ToString(), out qty);
                }
                for (int k = 0; k < grdmarkrework.Rows.Count; k++)
                {
                    int val = 0;
                    int.TryParse((grdmarkrework.Rows[i].FindControl("lblReworkQty") as Label).Text, out val);  //int.TryParse(grdmarkrework.Rows[k].Cells[1].Text, out val);
                    if (k != i)
                    {
                        total += val;
                    }
                    else
                    {
                        int.TryParse(txtqty.Text.ToString(), out updatedtext);
                    }
                }
                if (i == -1)
                {
                    int.TryParse(txtqty.Text.ToString(), out updatedtext);
                }
                total += updatedtext;
                if (ddlreworkcat.Items.Count < 0) return;

                if (string.IsNullOrEmpty(txtqty.Text.ToString()))
                {
                    lblMessages.Text = "Rework Qty cannot be empty!!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtqty.Text.Trim('0')))
                {
                    lblMessages.Text = "Rework Qty should be greater than zero!!";
                    return;
                }
                int acceptedParts = hdnAcceptedParts.Value != "" ? Convert.ToInt32(hdnAcceptedParts.Value) : 0;
                int rejectionValue = hdnrejectionTotalValue.Value != "" ? Convert.ToInt32(hdnrejectionTotalValue.Value) : 0;
                int calCount = acceptedParts - rejectionValue;
                if (total > calCount)
                {
                    lblMessages.Text = "Rework Qty cannot be greater than accepted parts!!";
                    return;
                }
                Int32.TryParse(cell_1_Value, out sl_no);
                DataBaseAccess.insertRejRwkDetails(Convert.ToInt32(txtqty.Text.ToString()), Convert.ToString(ddlreworkcat.Text), "PCT", DateTime.Now, Convert.ToInt32(rectionId), sl_no, "SaveRework", out rowadded);
                if (rowadded)
                {
                    updateProductionData("Rework","");
                    getdata();
                    lblMessages.Text = "Details added Successfully";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void reasonRejAdd_Click(object sender, EventArgs e)
        {
            try
            {
                rectionId = Request.QueryString["rectionId"].ToString(); int qty = 0;
                rejrew = Request.QueryString["rejectionValue"].ToString();
                if (ddlreasoncat.Items.Count < 0) return;
                if (Session["qty"] != null)
                {
                    int.TryParse(Session["qty"].ToString(), out qty);
                }
                if (string.IsNullOrEmpty(txtreasonqty.Text.ToString()))
                {
                    lblMessages.Text = "Rejection Qty cannot be empty!!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtreasonqty.Text.Trim('0')))
                {
                    lblMessages.Text = "Rejection Qty should be greater than zero!!";
                    return;
                }
                int updatedtext = 0; int total = 0;
                int.TryParse(txtreasonqty.Text.ToString(), out updatedtext);
                for (int k = 0; k < grdreason.Rows.Count; k++)
                {
                    int val = 0;
                    int.TryParse((grdreason.Rows[k].FindControl("lblRejectionQty") as Label).Text, out val);  //int.TryParse(grdreason.Rows[k].Cells[1].Text, out val);
                    total += val;
                }
                total += updatedtext;
                //if (total > qty)
                //{
                //    lblMessages.Text = "Rejection Qty cannot be greater than accepted parts!!";
                //    return;
                //}
                int acceptedParts = hdnAcceptedParts.Value != "" ? Convert.ToInt32(Convert.ToDouble(hdnAcceptedParts.Value)) : 0;
                int markedForRework = hdnMarkedForRework.Value != "" ? Convert.ToInt32(Convert.ToDouble(hdnMarkedForRework.Value)) : 0;
                int calCount = acceptedParts - markedForRework;
                if (total > calCount)
                {
                    lblMessages.Text = "Rejection Qty cannot be greater than accepted parts!!";
                    return;
                }
                DataBaseAccess.insertRejRwkDetails(Convert.ToInt32(txtreasonqty.Text.ToString()), Convert.ToString(ddlreason.Text), "PCT", DateTime.Now, Convert.ToInt32(rectionId), 0, "SaveRejections", out rowadded);
                if (rowadded)
                {
                    updateProductionData("Rejection","");
                    getdata();
                    lblMessages.Text = "Details added Successfully !!";

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void reasonRejUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string cell_1_Value = string.Empty;
                int i = grdreason.SelectedIndex; int updatedtext = 0;
                if (i >= 0)
                    cell_1_Value = (grdreason.Rows[i].FindControl("lblSlno") as Label).Text; // grdreason.Rows[i].Cells[0].Text;
                else
                    cell_1_Value = "0";
                int total = 0;
                int qty = 0;
                if (Session["qty"] != null)
                {
                    int.TryParse(Session["qty"].ToString(), out qty);
                }
                for (int k = 0; k < grdreason.Rows.Count; k++)
                {
                    int val = 0;
                    int.TryParse((grdreason.Rows[i].FindControl("lblRejectionQty") as Label).Text, out val);  //int.TryParse(grdreason.Rows[k].Cells[1].Text, out val);
                    if (k != i)
                    {
                        total += val;
                    }
                    else
                    {
                        int.TryParse(txtreasonqty.Text.ToString(), out updatedtext);
                    }
                }
                if (i == -1)
                {
                    int.TryParse(txtreasonqty.Text.ToString(), out updatedtext);
                }
                total += updatedtext;

                Int32.TryParse(cell_1_Value, out sl_no);
                rectionId = Request.QueryString["rectionId"].ToString();
                rejrew = Request.QueryString["rejectionValue"].ToString();

                if (ddlreasoncat.Items.Count < 0) return;
                if (string.IsNullOrEmpty(txtreasonqty.Text.ToString()))
                {
                    lblMessages.Text = "Rejection Qty cannot be empty!!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtreasonqty.Text.Trim('0')))
                {
                    lblMessages.Text = "Rejection Qty should be greater than zero!!";
                    return;
                }
                //if (total > qty)
                //{
                //    lblMessages.Text = "Rejection Qty cannot be greater than accepted parts!!";
                //    return;
                //}
                int acceptedParts = hdnAcceptedParts.Value != "" ? Convert.ToInt32(Convert.ToDouble(hdnAcceptedParts.Value)) : 0;
                int markedForRework = hdnMarkedForRework.Value != "" ? Convert.ToInt32(Convert.ToDouble(hdnMarkedForRework.Value)) : 0;
                int calCount = acceptedParts - markedForRework;
                if (total > calCount)
                {
                    lblMessages.Text = "Rejection Qty cannot be greater than accepted parts!!";
                    return;
                }
                DataBaseAccess.insertRejRwkDetails(Convert.ToInt32(txtreasonqty.Text.ToString()), Convert.ToString(ddlreason.Text), "PCT", DateTime.Now, Convert.ToInt32(rectionId), sl_no, "SaveRejections", out rowadded);
                if (rowadded)
                {
                    updateProductionData("Rejection","");
                    getdata();
                    lblMessages.Text = "Details added Successfully !!";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void reasonRejDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int i = grdreason.SelectedIndex;
                string cell_1_Value = (grdreason.Rows[i].FindControl("lblSlno") as Label).Text;  //grdreason.Rows[i].Cells[0].Text;
                Int32.TryParse(cell_1_Value, out sl_no);
                rectionId = Request.QueryString["rectionId"].ToString();
                rejrew = Request.QueryString["rejectionValue"].ToString();
                if (ddlreasoncat.Items.Count < 0) return;
                if (string.IsNullOrEmpty(txtreasonqty.Text.ToString()))
                {

                    lblMessages.Text = "Rejection Qty cannot be empty!!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtreasonqty.Text.Trim('0')))
                {
                    lblMessages.Text = "Rejection Qty should be greater than zero!!";
                    return;
                }
                DataBaseAccess.insertRejRwkDetails(Convert.ToInt32(txtreasonqty.Text.ToString()), Convert.ToString(ddlreason.Text), "PCT", DateTime.Now, Convert.ToInt32(rectionId), sl_no, "deleteRejection", out rowadded);
                if (rowadded)
                {
                    updateProductionData("Rejection", "delete");
                    getdata();
                    txtreasonqty.Text = "";
                    lblMessages.Text = "Details Deleted Successfully !!";

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btndeleterework_Click(object sender, EventArgs e)
        {
            try
            {
                int i = grdmarkrework.SelectedIndex;
                string cell_1_Value = (grdmarkrework.Rows[i].FindControl("lblSlno") as Label).Text; // grdmarkrework.Rows[i].Cells[0].Text;
                Int32.TryParse(cell_1_Value, out sl_no);
                rectionId = Request.QueryString["rectionId"].ToString();
                rejrew = Request.QueryString["rejectionValue"].ToString();
                if (ddlreworkcat.Items.Count < 0) return;
                if (string.IsNullOrEmpty(txtqty.Text.ToString()))
                {
                    lblMessages.Text = "Rejection Qty cannot be empty!!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtqty.Text.Trim('0')))
                {
                    lblMessages.Text = "Rejection Qty should be greater than zero!!";
                    return;
                }
                DataBaseAccess.insertRejRwkDetails(Convert.ToInt32(txtqty.Text.ToString()), Convert.ToString(ddlreworkcat.Text), "PCT", DateTime.Now, Convert.ToInt32(rectionId), sl_no, "deleteRwk", out rowadded);
                if (rowadded)
                {
                    updateProductionData("Rework","delete");
                    getdata();
                    lblMessages.Text = "Details Deleted Successfully !!";

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void updateProductionData(string RejectionRework,string delete)
        {
            try
            {
                //int rejQty = DataBaseAccess.GetJobCardRejRwkDetails(Convert.ToDateTime(Session["date"].ToString()), Session["shift"].ToString(), Session["plant"].ToString(),
                //        Session["machine"].ToString(), "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", Convert.ToInt32(rectionId), "RejectionQty", out rowAdded);
                //int reworkQty = DataBaseAccess.GetJobCardRejRwkDetails(Convert.ToDateTime(Session["date"].ToString()), Session["shift"].ToString(), Session["plant"].ToString(),
                //        Session["machine"].ToString(), "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", Convert.ToInt32(rectionId), "Rework_Qty", out rowAdded);
                int rejQty = RejectionRework == "Rejection" ? Convert.ToInt32(txtreasonqty.Text) : 0; 
                int reworkQty = RejectionRework == "Rework" ? Convert.ToInt32(txtreasonqty.Text) : 0;
                int deleteQty = delete == "" ? 0 : Convert.ToInt32(txtreasonqty.Text);
                //DataBaseAccess.GetJobCardDetails(Convert.ToDateTime(Session["date"].ToString()), Session["shift"].ToString(), Session["plant"].ToString(), Session["machine"].ToString(), Session["component"].ToString(), Session["opn"].ToString(), Session["opr"].ToString(), hdnAcceptedParts.Value != "" ? Convert.ToInt32(hdnAcceptedParts.Value) : 0, Convert.ToInt32(Session["repeatCycle"].ToString()), Convert.ToInt32(Session["dummycycle"].ToString()), Convert.ToInt32(Session["reworkperformed"].ToString()), hdnMarkedForRework.Value != "" ? Convert.ToInt32(hdnMarkedForRework.Value) : 0, 0, "PCT", DateTime.Now, "", Convert.ToInt32(rectionId), "UpdateProductionData", "", "", "", "", out rowAdded);
                DataBaseAccess.GetJobCardDetails(Convert.ToDateTime(Session["date"].ToString()), Session["shift"].ToString(), Session["plant"].ToString(), Session["machine"].ToString(), Session["component"].ToString(), Session["opn"].ToString(), Session["opr"].ToString(), hdnAcceptedParts.Value != "" ? Convert.ToInt32(Convert.ToDouble(hdnAcceptedParts.Value)) : 0, Convert.ToInt32(Session["repeatCycle"].ToString()), Convert.ToInt32(Session["dummycycle"].ToString()), reworkQty, hdnMarkedForRework.Value != "" ? Convert.ToInt32(Convert.ToDouble(hdnMarkedForRework.Value)) : 0, rejQty, "PCT", DateTime.Now, "", Convert.ToInt32(rectionId), "UpdateProductionData", "", "", "", "", out rowAdded,deleteQty);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void changedindex(object sender, EventArgs e)
        {
            try
            {
                GridView dg = sender as GridView;
                int i = dg.SelectedIndex;
                if (dg.ID.Equals("grdreason", StringComparison.OrdinalIgnoreCase))
                {
                    string cell_2_Value = (dg.Rows[i].FindControl("lblRejectionQty") as Label).Text; // dg.Rows[i].Cells[1].Text;
                    string cell_3_Value = (dg.Rows[i].FindControl("lblRejectionReason") as Label).Text; // dg.Rows[i].Cells[2].Text;
                    Session["RejSec"] = (dg.Rows[i].FindControl("lblSlno") as Label).Text;  //dg.Rows[i].Cells[0].Text;
                    ddlreasoncat.SelectedValue = DataBaseAccess.getCategoryForSelectedReason(cell_3_Value, "Rejection");
                    ddlreasoncat_SelectedIndexChanged(null, null);
                    ddlreason.SelectedValue = cell_3_Value;
                    //ddlreason.Text = cell_3_Value;
                    txtreasonqty.Text = cell_2_Value;
                }
                else if (dg.ID.Equals("grdmarkrework", StringComparison.OrdinalIgnoreCase))
                {
                    string cell_2_Value = (dg.Rows[i].FindControl("lblReworkQty") as Label).Text; // dg.Rows[i].Cells[1].Text;
                    string cell_3_Value = (dg.Rows[i].FindControl("lblReworkReason") as Label).Text; // dg.Rows[i].Cells[2].Text;
                                                                                                     // ddlreworkreason.Text = cell_3_Value;
                    ddlreworkcat.SelectedValue = DataBaseAccess.getCategoryForSelectedReason(cell_3_Value, "Rework");
                    ddlreworkcat_SelectedIndexChanged(null, null);
                    ddlreworkreason.SelectedValue = cell_3_Value;
                    txtqty.Text = cell_2_Value;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }


        }
        protected void ddlreasoncat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlreason.DataSource = DataBaseAccess.GetAllRejectionRework("Rejection", ddlreasoncat.Text.ToString());
            ddlreason.DataBind();
        }

        protected void ddlreworkcat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlreworkreason.DataSource = DataBaseAccess.GetAllRejectionRework("Rework", ddlreworkcat.Text.ToString());
            ddlreworkreason.DataBind();
        }
    }
}