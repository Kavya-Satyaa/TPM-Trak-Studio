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
    public partial class JobCard : System.Web.UI.Page
    {
        bool rowAdded = false;
        List<string> EmployeeList = new List<string>();
        List<string> downCode = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadData();
                // getGridData();
            }
        }

        public void getGridData()
        {
            try
            {
                EmployeeList = DataBaseAccess.GetAllEmployees();
                EmployeeList.RemoveAt(0);
                downCode = DataBaseAccess.GetAllDownCode();

                DataTable dt = new DataTable();
                dt = DataBaseAccess.GetJobCardDetails(Convert.ToDateTime(txtFromDate.Text.ToString()), ddlshift.SelectedValue.ToString(), ddlPlantId.Text.ToString(), ddlmachineid.SelectedValue.ToString(), "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", 0, "ViewProductionData", "", "", "", "", out rowAdded,0);
                dt.Columns.Add("Rejection");
                foreach (DataColumn col in dt.Columns)
                {
                    col.ReadOnly = false;
                }


                foreach (DataRow dr in dt.Rows)
                {
                    dr["Rejection"] = DataBaseAccess.GetJobCardRejRwkDetails(Convert.ToDateTime(txtFromDate.Text.ToString()), ddlshift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(),
                        ddlmachineid.Text.ToString(), "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", Convert.ToInt32(dr["id"].ToString()), "RejectionQty", out rowAdded);

                    dr["Marked_for_Rework"] = DataBaseAccess.GetJobCardRejRwkDetails(Convert.ToDateTime(txtFromDate.Text.ToString()), ddlshift.Text.ToString(), ddlPlantId.Text.ToString(),
                        ddlmachineid.Text.ToString(), "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", Convert.ToInt32(dr["id"].ToString()), "Rework_Qty", out rowAdded);
                }
                grdproduction.DataSource = dt;
                grdproduction.DataBind();

                //downData

                DataTable dtdown = new DataTable();
                dtdown = DataBaseAccess.GetJobCardDetails(Convert.ToDateTime(txtFromDate.Text.ToString()), ddlshift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), ddlmachineid.SelectedValue.ToString(), "", "", "", 0, 0, 0, 0, 0, 0, "", DateTime.Now, "", 0, "ViewDownData", "", "", "", "", out rowAdded,0);
                Session["DownDataSplitFun"] = dtdown;
                grd_downcode.DataSource = dtdown;
                grd_downcode.DataBind();


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Job Card" + ex.Message);
                throw;

            }


        }


        public void loadData()
        {
            ddlPlantId.DataSource = DataBaseAccess.GetAllPlants();
            ddlPlantId.DataBind();
            if (ddlPlantId.Items.Count > 0)
            {
                ddlPlantId.SelectedIndex = 0;
            }
            ddlshift.DataSource = DataBaseAccess.bindDataForShiftName();
            ddlshift.DataBind();
            if (ddlshift.Items.Count > 0)
            {
                ddlshift.SelectedIndex = 0;
            }
            Bindmachine();

            EmployeeList = DataBaseAccess.GetAllEmployees();
            EmployeeList.RemoveAt(0);
            downCode = DataBaseAccess.GetAllDownCode();
        }

        protected void btnview_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text == "")
            {
                lblMessages.Text = "Please Select Date";
                return;
            }
            EmployeeList = DataBaseAccess.GetAllEmployees();
            EmployeeList.RemoveAt(0);
            downCode = DataBaseAccess.GetAllDownCode();
            getGridData();
        }

        protected void grdproduction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //EmployeeList = DataBaseAccess.GetAllEmployees();
            //EmployeeList.RemoveAt(0);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdfopr = (e.Row.FindControl("hdfoperator") as HiddenField);
                DropDownList ddlopr = (e.Row.FindControl("ddlOperator") as DropDownList);
                ddlopr.DataSource = EmployeeList;
                ddlopr.DataBind();
                ddlopr.SelectedValue = hdfopr.Value;
            }
        }

        protected void grd_downcode_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdfdowncode = (e.Row.FindControl("hdfdowncode") as HiddenField);
                DropDownList ddldown = (e.Row.FindControl("ddldownid") as DropDownList);
                ddldown.DataSource = downCode;
                ddldown.DataBind();
                ddldown.SelectedValue = hdfdowncode.Value;

                HiddenField hdfdownopr = (e.Row.FindControl("hdfdownoperator") as HiddenField);
                DropDownList ddldownopr = (e.Row.FindControl("ddldownOperator") as DropDownList);
                ddldownopr.DataSource = EmployeeList;
                ddldownopr.DataBind();
                ddldownopr.SelectedValue = hdfdownopr.Value;

            }


        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string date = txtFromDate.Text;
                string checksaveprod = hdfprodsave.Value;
                if (checksaveprod.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (GridViewRow row in grdproduction.Rows)
                    {
                        HiddenField hdfvalue = (HiddenField)row.FindControl("hdfprodupdate");
                        if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                        {
                            string grdpartno = ((Label)row.FindControl("PartNo")).Text;
                            string grdoprno = ((Label)row.FindControl("oprno")).Text;
                            string grdoperator = ((DropDownList)row.FindControl("ddlOperator")).SelectedValue;
                            string grdacceptedparts = ((Label)row.FindControl("acceptedparts")).Text;
                            string grdrepeatcycle = ((TextBox)row.FindControl("repeatcycle")).Text;
                            string grddummycycl = ((TextBox)row.FindControl("dummycycle")).Text;
                           // string grdreworkperformed = ((Label)row.FindControl("reworkperformed")).Text;
                            string grdreworkperformed = ((TextBox)row.FindControl("txtreworkperformed")).Text;
                            //string grdmarkreview = ((TextBox)row.FindControl("markrework")).Text;
                            //string grdrejection = ((TextBox)row.FindControl("rejection")).Text;
                            string grdmarkreview = ((Label)row.FindControl("lblmarkrework")).Text;
                            string grdrejection = ((Label)row.FindControl("lblRejection")).Text;
                            string grdworkordno = ((Label)row.FindControl("WorkOrderNumber")).Text;
                            string id = ((Label)row.FindControl("id")).Text;
                            DataBaseAccess.GetJobCardDetails(Convert.ToDateTime(date), ddlshift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), ddlmachineid.SelectedValue.ToString(), grdpartno, grdoprno, grdoperator, Convert.ToInt32(grdacceptedparts), Convert.ToInt32(grdrepeatcycle), Convert.ToInt32(grddummycycl), Convert.ToInt32(grdreworkperformed), Convert.ToInt32(grdmarkreview), 0, "PCT", DateTime.Now, "", Convert.ToInt32(id), "UpdateProductionData", "", "", "", "", out rowAdded,0);
                        }
                    }
                    // hdfprodsave.Value = "";
                }

                string checksavedown = hdfdownsave.Value;
                if (checksavedown.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (GridViewRow row in grd_downcode.Rows)
                    {
                        HiddenField hdfvaluedown = (HiddenField)row.FindControl("hdfdownupdate");
                        if (hdfvaluedown.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                        {
                            string grdpartno = ((Label)row.FindControl("PartNo")).Text;
                            string grdoprno = ((Label)row.FindControl("oprno")).Text;

                            string grdstarttime = ((Label)row.FindControl("starttime")).Text;
                            string grdendtime = ((Label)row.FindControl("endTime")).Text;
                            string grddownid = ((DropDownList)row.FindControl("ddldownid")).SelectedValue;
                            string grdoperator = ((DropDownList)row.FindControl("ddldownOperator")).SelectedValue;
                            string id = ((Label)row.FindControl("id")).Text;
                            string Remarks = (row.FindControl("txtRemarks") as TextBox).Text;
                            string ActionTaken = (row.FindControl("txtActiontaken") as TextBox).Text;
                                DataBaseAccess.GetJobCardDetails(Convert.ToDateTime(date), ddlshift.SelectedValue.ToString(), ddlPlantId.SelectedValue.ToString(), ddlmachineid.SelectedValue.ToString(), grdpartno, grdoprno, grdoperator, 0, 0,
                             0, 0, 0, 0, "PCT", DateTime.Now, grddownid, Convert.ToInt32(id), "UpdateDownData", Remarks, ActionTaken, grdstarttime, grdendtime,  out rowAdded,0); //
                        }
                    }
                    // hdfdownsave.Value = "";
                }
                if (hdfprodsave.Value != "" || hdfdownsave.Value != "")
                {
                    hdfprodsave.Value = "";
                    hdfdownsave.Value = "";
                    getGridData();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw;
            }


        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bindmachine();
        }

        private void Bindmachine()
        {
            if (ddlPlantId.SelectedValue.ToString().Equals("All"))
            {
                ddlmachineid.DataSource = DataBaseAccess.GetAllMacForPlant(ddlPlantId.SelectedValue.ToString());
                ddlmachineid.DataBind();
            }
            else
            {
                ddlmachineid.DataSource = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());
                ddlmachineid.DataBind();
            }
            if (ddlmachineid.Items.Count > 0)
            {
                ddlmachineid.SelectedIndex = 0;
            }
        }
        protected void btnReloadBtn_Click(object sender, EventArgs e)
        {
            getGridData();
        }
    }
}