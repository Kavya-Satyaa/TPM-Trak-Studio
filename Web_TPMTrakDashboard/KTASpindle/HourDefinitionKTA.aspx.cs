using Elmah;
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
    public partial class HourDefinitionKTA : System.Web.UI.Page
    {
        private DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ShiftID"] != "")
                {
                    var ShiftType = Request.QueryString["ShiftType"].ToString();
                    var shiftId = Request.QueryString["ShiftID"].ToString();
                    ViewState["ShiftIdVale"] = shiftId;
                    var shiftName = Request.QueryString["ShiftName"].ToString();
                    BindShiftType(ShiftType);
                    BindShift(ShiftType);
                    ddlShiftId.SelectedValue = shiftName;
                    BindShiftName();
                }
                else Response.Redirect("ShiftDetailsKTA.aspx", false);
            }
        }
        private void BindShiftType(string ShiftType)
        {
            try
            {
                List<ShiftDataModel> list = DBAccess.GetShiftType();
                if(list!=null)
                {
                    ddlShiftType.DataSource = list.AsEnumerable().Select(x => x.ShiftType).Distinct().ToList();
                    ddlShiftType.DataBind();
                    ddlShiftType.SelectedValue = ShiftType;
                }
                
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        private void BindShiftName()
        {
            try
            {
                ddlShiftId_SelectedIndexChanged(null, null);
                NewShiftHourFormShown();
                if (dt == null)
                {
                    btnDefault_Click(null, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        private void BindShift(string ShiftType)
        {
            try
            {
                List<ShiftDataModel> list = DBAccess.GetAllshiftDetailsKTA(ShiftType);
                if(list!=null)
                {
                    ddlShiftId.DataSource = list.AsEnumerable().Select(x => x.ShiftName).Distinct().ToList();
                    ddlShiftId.DataBind();
                }
                else
                {
                    ddlShiftId.DataSource = new List<string>();
                    ddlShiftId.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void ddlShiftType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShift(ddlShiftType.SelectedValue.ToString());
            BindShiftName();
        }
        #region "selected index change shift Name"
        protected void ddlShiftId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DBAccess.bindtextShiftFromname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue) == "0") txtFromDay.Text = "Today";
                else if (DBAccess.bindtextShiftFromname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue) == "-1") txtFromDay.Text = "Yesterday";
                else if (DBAccess.bindtextShiftFromname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue) == "1") txtFromDay.Text = "Tommorow";
                else
                    txtFromDay.Text = string.Empty;

                string fromtime = DBAccess.binddtpfromShiftname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue);
                if (!string.IsNullOrEmpty(fromtime))
                    txtFromTime.Text = Convert.ToDateTime(fromtime).ToString("HH:mm:ss");
                else
                    txtFromTime.Text = string.Empty;
                string shiftId = DBAccess.FindShiftId(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue);
                ViewState["ShiftIdVale"] = shiftId;

                if (DBAccess.bindtextShiftToname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue) == "0") txtToDay.Text = "Today";
                else if (DBAccess.bindtextShiftToname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue) == "-1") txtToDay.Text = "Yesterday";
                else if (DBAccess.bindtextShiftToname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue) == "1") txtToDay.Text = "Tommorow";
                else
                    txtToDay.Text = string.Empty;

                string Totime = DBAccess.bindtextdtptoShiftname(ddlShiftId.SelectedValue, ddlShiftType.SelectedValue);
                if (!string.IsNullOrEmpty(Totime))
                    txtToTime.Text = Convert.ToDateTime(Totime).ToString("HH:mm:ss");
                else
                    txtToTime.Text = string.Empty;
                NewShiftHourFormShown();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        private void NewShiftHourFormShown()
        {
            dt = new DataTable();
            string shiftId = ViewState["ShiftIdVale"] as string;
            dt = DBAccess.bindhourlytimeDataKTA(shiftId,ddlShiftType.SelectedValue);
            gridviewHourly.DataSource = null;
            gridviewHourly.DataSource = dt;
            gridviewHourly.DataBind();
            Session["HourlyDatatable"] = dt;

        }
        protected void btnDefault_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(ddlShiftId.SelectedValue))
                {
                    DateTime fromDate = Convert.ToDateTime(txtFromTime.Text);
                    if (txtFromDay.Text == "Yesterday") { fromDate = fromDate.AddDays(-1); }
                    DateTime toDate = Convert.ToDateTime(txtToTime.Text);
                    if (txtFromDay.Text == "Tommorow")
                    {
                        fromDate = fromDate.AddDays(1);
                    }

                    if (txtToDay.Text == "Tommorow") { toDate = toDate.AddDays(1); }
                    DateTime newdate = fromDate;
                    int newdates = Convert.ToInt32(newdate.Day);
                    DataTable dtNew = new DataTable();
                    dtNew.Columns.Add("ShiftID", typeof(int));
                    dtNew.Columns.Add("HourName", typeof(string));
                    dtNew.Columns.Add("HourID", typeof(int));
                    dtNew.Columns.Add("FromDay", typeof(int));
                    dtNew.Columns.Add("ToDay", typeof(int));
                    dtNew.Columns.Add("HourStart", typeof(DateTime));
                    dtNew.Columns.Add("HourEnd", typeof(DateTime));
                    dtNew.Columns.Add("Minutes", typeof(int));
                    int i = 1, j = 0, k = 0;
                    DateTime d = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, fromDate.Hour, fromDate.Minute, fromDate.Second, fromDate.Kind);
                    if (d < toDate)
                    {
                        while (d < toDate)
                        {
                            if (txtFromDay.Text == "Today" && (Convert.ToInt32(d.Day) - newdates) == 0) j = 0;
                            else j = 1;
                            if (txtFromDay.Text == "Yesterday") j = -1;
                            if (txtFromDay.Text == "Tommorow") j = 1;
                            d = d.AddMinutes(60);
                            if (txtToDay.Text == "Today") k = 0;
                            if (txtToDay.Text == "Yesterday") k = -1;
                            if (txtToDay.Text == "Tommorow" && (Convert.ToInt32(d.Day) - newdates) == 1) k = 1;
                            else k = 0;
                            d = d.AddMinutes(-60);
                            DateTime fromdate = d.AddMinutes(60);
                            if (fromdate > toDate)
                            {
                                fromdate = toDate;
                            }
                            dtNew.Rows.Add(1, "Hour" + i, i, j, k, d, fromdate, 60);
                            d = fromdate;
                            i++;
                        }
                    }
                    else
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = "To Date Cannot Be Greater Than From Date !!";
                    }
                    if (dtNew != null && dtNew.Columns.Count > 0)
                    {
                        gridviewHourly.DataSource = dtNew;
                        gridviewHourly.DataBind();
                        Session["HourlyDatatable"] = dtNew;

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string sucessFailure = string.Empty;
                if (gridviewHourly.Rows != null && gridviewHourly.Rows.Count > 0)
                {
                    string shiftId = ViewState["ShiftIdVale"] as string;
                    DBAccess.DeleteDataFromHourlyDefinationKTA(ddlShiftType.SelectedValue, shiftId, out sucessFailure);
                }
                if (sucessFailure.Equals("Successfull"))
                {
                    DataTable dt1 = DBAccess.bindhourlytimeDataKTA(ddlShiftId.SelectedValue,ddlShiftType.SelectedValue);
                    gridviewHourly.DataSource = dt1;
                    gridviewHourly.DataBind();
                    Session["HourlyDatatable"] = dt1;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        #region "Save Button Click"
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sucessFailure = string.Empty;
                string shiftId = ViewState["ShiftIdVale"] as string;
                DBAccess.DeleteDataFromHourlyDefinationKTA(ddlShiftType.SelectedValue, shiftId, out sucessFailure);
                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;
                foreach (GridViewRow item in gridviewHourly.Rows)
                {
                    HiddenField hdfShiftId = ((HiddenField)item.FindControl("hdfShiftId"));
                    HiddenField hdfHourID = ((HiddenField)item.FindControl("hdfHourID"));
                    HiddenField hdfHourStart = ((HiddenField)item.FindControl("hdfHourStart"));
                    HiddenField hdfHourEnd = ((HiddenField)item.FindControl("hdfHourEnd"));

                    fromDate = Convert.ToDateTime(hdfHourStart.Value);
                    toDate = Convert.ToDateTime(hdfHourEnd.Value);


                    HiddenField hdfFromDay = ((HiddenField)item.FindControl("hdfFromDay"));

                    if (txtFromDay.Text == "Today" && (Convert.ToInt32(fromDate.Day) - DateTime.Now.Day) == 0) hdfFromDay.Value = "0";
                    else hdfFromDay.Value = "1";
                    if (txtFromDay.Text == "Yesterday") hdfFromDay.Value = "-1";
                    if (txtFromDay.Text == "Tommorow") hdfFromDay.Value = "1";

                    HiddenField hdfToDay = ((HiddenField)item.FindControl("hdfToDay"));
                    if (txtToDay.Text == "Today") hdfToDay.Value = "0";
                    if (txtToDay.Text == "Yesterday") hdfToDay.Value = "-1";
                    if (txtToDay.Text == "Tommorow" && (Convert.ToInt32(toDate.Day) - DateTime.Now.Day) == 1) hdfToDay.Value = "1";
                    else hdfToDay.Value = "0";

                    TextBox txtHourDef = ((TextBox)item.FindControl("txtHourDef"));
                    TextBox txtMinutes = ((TextBox)item.FindControl("txtMinutes"));
                    DBAccess.InsertHourDefination(ddlShiftType.SelectedValue, shiftId, txtHourDef.Text, hdfHourID.Value, hdfFromDay.Value, hdfToDay.Value, hdfHourStart.Value, hdfHourEnd.Value, txtMinutes.Text.Trim(), out sucessFailure);
                    if (sucessFailure == "Successfull")
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = "Data Inserted Sucessfully. !!";
                    }

                }
                NewShiftHourFormShown();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        #region "Minutes Text Change Event"
        protected void txtMinutes_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridviewHourly.Rows.Count; i++)
            {
                TextBox txtMinutes = (TextBox)gridviewHourly.Rows[i].FindControl("txtMinutes");//((TextBox)item.FindControl("txtMinutes"));
                if (((Convert.ToInt32(txtMinutes.Text)) > 0) && ((Convert.ToInt32(txtMinutes.Text) <= 60)) && (txtMinutes.Text != string.Empty))
                {
                    string hourStart = gridviewHourly.Rows[i].Cells[1].Text.ToString();
                    var val = hourStart;
                    DateTime StartTime = Convert.ToDateTime(val);
                    gridviewHourly.Rows[i].Cells[3].Text = StartTime.AddMinutes(Convert.ToInt32(txtMinutes.Text)).ToString("hh:mm:ss tt");

                    HiddenField hdfHourStart = ((HiddenField)gridviewHourly.Rows[i].FindControl("hdfHourStart"));
                    HiddenField hdfHourEnd = ((HiddenField)gridviewHourly.Rows[i].FindControl("hdfHourEnd"));
                    val = hdfHourStart.Value;
                    StartTime = Convert.ToDateTime(val);
                    hdfHourEnd.Value = StartTime.AddMinutes(Convert.ToInt32(txtMinutes.Text)).ToString();
                    if (i < gridviewHourly.Rows.Count - 1)
                    {
                        HiddenField hdfHourStart1 = (HiddenField)gridviewHourly.Rows[i + 1].FindControl("hdfHourStart");
                        HiddenField hdfHourEnd1 = (HiddenField)gridviewHourly.Rows[i].FindControl("hdfHourEnd");
                        hdfHourStart1.Value = hdfHourEnd1.Value;

                        //gridviewHourly.Rows[i + 1].Cells[1].Text = gridviewHourly.Rows[i].Cells[3].Text.ToString();
                        gridviewHourly.Rows[i + 1].Cells[1].Text = gridviewHourly.Rows[i].Cells[3].Text;

                    }
                }
                else
                {
                    if (((Convert.ToInt32(txtMinutes.Text)) <= 0))
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = "Hour minutes cannot be Zero !!";
                    }
                    if (((Convert.ToInt32(txtMinutes.Text)) > 60))
                    {
                        lblMessages.Text = "Hour Defination Cannot exceed from 60 min";
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                    }
                    if ((txtMinutes.Text == string.Empty))
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = "Hour minutes cannot be empty. !!";
                    }
                }
            }



        }
        #endregion

    }
}