using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class ShiftDetailsKTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindShiftType();
                BindGridView();
            }
        }

        #region "Bind Gridview Data"
        private void BindGridView()
        {
            try
            {
               List<ShiftDataModel> details= DBAccess.GetAllshiftDetailsKTA(ddlShiftType.SelectedValue.ToString());
                gridShiftDetails.DataSource = details;
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            txtShiftType.Text = String.Empty;
            HelperClass.openAddEditModal(this, "newShiftTypeModal");
        }

        protected void ddlShiftID_SelectedIndexChanged(object sender, EventArgs e)
        {
            getDetailsForShiftId(ddlShiftID.SelectedValue);
        }

        private void getDetailsForShiftId(string shiftId)
        {
            try
            {
                var details = DBAccess.GetShiftDetailsKTA(shiftId,ddlShiftType.SelectedValue.ToString());
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
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void resetAllFeilds()
        {
            txtShiftName.Text = string.Empty;
            ddlFromDay.SelectedIndex = 0;
            ddlToDay.SelectedIndex = 0;
            txtFromTime.Text = string.Empty;
            txtToTime.Text = string.Empty;
        }

        #region "Save Update Records"
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValidEntry = DBAccess.CheckShiftIdKTA(ddlShiftID.SelectedValue,ddlShiftType.SelectedValue);
                if (isValidEntry)
                {
                    DownCodeInfoDataBase.CheckForTheTimeEntry(txtFromTime.Text.ToString(), txtToTime.Text.ToString());
                    DBAccess.UpdateShiftDetailsKTA(ddlShiftType.SelectedValue,ddlShiftID.SelectedValue, txtShiftName.Text, ddlFromDay.SelectedValue, ddlToDay.SelectedValue, Convert.ToDateTime(txtFromTime.Text.ToString()), Convert.ToDateTime(txtToTime.Text.ToString()));
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "Details Added Successfully. !!";
                    BindGridView();
                }
                else
                {
                    if (!DBAccess.CheckForShiftName(txtShiftName.Text, ddlShiftID.SelectedValue,ddlShiftType.SelectedValue))
                    {
                        DBAccess.InsertShiftDetailsKTA(ddlShiftID.SelectedValue, txtShiftName.Text, ddlShiftType.SelectedValue,ddlFromDay.SelectedValue, ddlToDay.SelectedValue, Convert.ToDateTime(txtFromTime.Text), Convert.ToDateTime(txtToTime.Text));
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
                    DBAccess.RemoveAllShiftdataKTA(ddlShiftType.SelectedValue.ToString());
                    resetAllFeilds();
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    lblMessages.Text = "This removes all of your shift details for the specific Shift Type!!";
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


        private void BindShiftType()
        {
            try
            {
                List<ShiftDataModel> list = DBAccess.GetShiftType();
                gvShiftType.DataSource = list;
                gvShiftType.DataBind();

                if(list.Count>0)
                {
                    ddlShiftType.DataSource = list.AsEnumerable().Select(x => x.ShiftType).Distinct().ToList();
                    ddlShiftType.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void btnShiftTypeSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool Updated = false;
                if(!string.IsNullOrEmpty(txtShiftType.Text))
                {
                    Updated = DBAccess.SaveShiftType(txtShiftType.Text);
                    if(Updated)
                    {
                        HelperClass.openInsertSuccessModal(this);
                        HelperClass.openAddEditModal(this, "newShiftTypeModal");
                        txtShiftType.Text = string.Empty;
                        BindShiftType();
                    }
                    else
                    {
                        HelperClass.openWarningModal(this, "Shift Type Already Exsits,.!! \n Please Enter Different Shift Type. !!");
                        HelperClass.openAddEditModal(this, "newShiftTypeModal");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }


        protected void lbShiftTypeDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["DeleteRowIndex"] = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
                string parametr = (gvShiftType.Rows[(int)ViewState["DeleteRowIndex"]].FindControl("lblShiftType") as Label).Text;
                string deletemsg = "Are you sure, you want to delete Shift Type.:" + parametr;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmModal", "openDeleteConfirmModal('" + deletemsg + "')", true);
                HelperClass.openAddEditModal(this, "newShiftTypeModal");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = (int)ViewState["DeleteRowIndex"];
                string ShiftType = (gvShiftType.Rows[(int)ViewState["DeleteRowIndex"]].FindControl("lblShiftType") as Label).Text;
                int success = DBAccess.DeleteShiftType(ShiftType);
                if (success > 0)
                {
                    DBAccess.DeleteShiftDetailsBasedOnShiftType(ShiftType);
                    HelperClass.openDeleteSuccessModal(this);
                    HelperClass.openAddEditModal(this, "newShiftTypeModal");
                    
                }
                else
                {
                    HelperClass.openAddEditModal(this, "newShiftTypeModal");
                    HelperClass.openDeleteErrorModal(this);
                    return;
                }
                BindShiftType();
                ddlShiftType_SelectedIndexChanged(null, null);

                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnDeleteConfirm_Click: " + ex.Message);
            }
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseConfirmModal", "CloseDeleteConfirmModal()", true);
            HelperClass.openAddEditModal(this, "newShiftTypeModal");
        }

        protected void ddlShiftType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView();
        }
    }
}