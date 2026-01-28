using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ShiftDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        #region "Bind Gridview Data"
        private void BindGridView()
        {
            try
            {
                gridShiftDetails.DataSource = DownCodeInfoDataBase.GetAllshiftDetails();
                gridShiftDetails.DataBind();
                ddlShiftID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        #region "Save Update Records"
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValidEntry = DownCodeInfoDataBase.CheckShiftId(ddlShiftID.SelectedValue);
                if (isValidEntry)
                {
                    DownCodeInfoDataBase.CheckForTheTimeEntry(txtFromTime.Text.ToString(), txtToTime.Text.ToString());
                    DownCodeInfoDataBase.UpdateShiftDetails(ddlShiftID.SelectedValue, txtShiftName.Text, ddlFromDay.SelectedValue, ddlToDay.SelectedValue, Convert.ToDateTime(txtFromTime.Text.ToString()), Convert.ToDateTime(txtToTime.Text.ToString()));
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "Details Added Successfully. !!";
                    BindGridView();
                }
                else
                {
                    if (!DownCodeInfoDataBase.CheckForShiftName(txtShiftName.Text, ddlShiftID.SelectedValue))
                    {
                        DownCodeInfoDataBase.InsertShiftDetails(ddlShiftID.SelectedValue, txtShiftName.Text, ddlFromDay.SelectedValue, ddlToDay.SelectedValue, Convert.ToDateTime(txtFromTime.Text), Convert.ToDateTime(txtToTime.Text));
                        lblMessages.ForeColor = System.Drawing.Color.Green;
                        lblMessages.Text = "Details Added Successfully. !!";
                        BindGridView();
                    }
                    else
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Shift Name Already Exsits,.!! \n Please Enter Different Shift Name. !!";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        #region "Clear Data"
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridShiftDetails.Rows.Count > 0)
                {
                    DownCodeInfoDataBase.RemoveAllShiftdata();
                    resetAllFeilds();
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "This removes all of your shift details !!";
                    BindGridView();
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "No Records To Delete. !!";
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        private void resetAllFeilds()
        {
            txtShiftName.Text = string.Empty;
            ddlFromDay.SelectedIndex = 0;
            ddlToDay.SelectedIndex = 0;
            txtFromTime.Text = string.Empty;
            txtToTime.Text = string.Empty;
            //dTPFromTime.Value =
        }

        protected void ddlShiftID_SelectedIndexChanged(object sender, EventArgs e)
        {
            getDetailsForShiftId(ddlShiftID.SelectedValue);
        }

        private void getDetailsForShiftId(string shiftId)
        {
            var details = DownCodeInfoDataBase.GetShiftDetails(shiftId);
            if (details != null)
            {
                if (details.FromDay.ToString().Equals("0")) ddlFromDay.SelectedIndex = 0;
                else if (details.FromDay.ToString().Equals("1")) ddlFromDay.SelectedIndex = 1;
                else if (details.FromDay.ToString().Equals("2")) ddlFromDay.SelectedIndex = 2;

                if (details.ToDay.ToString().Equals("0")) ddlToDay.SelectedIndex = 0;
                else if (details.ToDay.ToString().Equals("1")) ddlToDay.SelectedIndex = 1;
                else if (details.ToDay.ToString().Equals("2")) ddlToDay.SelectedIndex = 2;

                DateTime dt = DateTime.Parse(details.FromTime.ToString());
                txtFromTime.Text = dt.ToString("HH:mm:ss");
                DateTime dt2 = DateTime.Parse(details.ToTime.ToString());
                txtToTime.Text = dt2.ToString("HH:mm:ss");
                txtShiftName.Text = details.ShiftName.ToString();
            }
            else
            {
                resetAllFeilds();
            }
        }
    }
}