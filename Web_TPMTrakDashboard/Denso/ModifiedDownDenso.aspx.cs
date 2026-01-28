using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.Denso.Model;
using System.Data;

namespace Web_TPMTrakDashboard.Denso
{
    public partial class ModifiedDownDenso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["DownCategory"] = null;
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindMachineId();
                BindGrid();
            }

        }
        private void BindMachineId()
        {
            try
            {
                List<string> mInfos = new List<string>();
                mInfos = DataBaseAccess.GetAllMachinesForPlant("");
                ddlMachine.DataSource = mInfos;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindDownCategory(DropDownList ddl)
        {
            try
            {
                List<string> list = new List<string>();
                if (Session["DownCategory"] == null)
                {
                    Session["DownCategory"] = list = DensoDBAccess.getDownCategoryData();
                }
                list = Session["DownCategory"] as List<string>;
                ddl.DataSource = list;
                ddl.DataBind();
                ddl.Items.Insert(0, "All");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }
        private void BindDownCode(DropDownList ddl, string downCategory, string value, string source)
        {
            try
            {
                string[] MachineID = ddlMachine.Text.ToString().Split('<');
                string MachineInterfaceID = MachineID[0].ToString().Trim();
                List<ModifiedDataEntity> list = new List<ModifiedDataEntity>();
                if (Session["downcode"] == null)
                    Session["downcode"] = list = DensoDBAccess.GetAllParameterWithIntefaceAndCode("", "", "", MachineInterfaceID, "", "", "", "", "", "Downid", downCategory);
                else
                    list = Session["downcode"] as List<ModifiedDataEntity>;

                list = Session["downcode"] as List<ModifiedDataEntity>;
                ddl.DataSource = list;
                ddl.DataTextField = "InterfaceIWithID";
                ddl.DataValueField = "InterfaceID";
                ddl.DataBind();
                if (source.Equals("databound", StringComparison.OrdinalIgnoreCase))
                {
                    if (ddl.Items.FindByValue(value) == null)
                    {
                        ddl.Items.Add(new ListItem(value, value));
                    }
                }
                HelperClassGeneric.setDropdownValue(ddl, value);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }
        private void BindGrid()
        {
            try
            {
                DateTime StartTime = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayStart(txtFromDate.Text));
                DateTime EndTime = Util.GetDateTime(VDGDataBaseAccess.GetLogicalDayEnd(txtToDate.Text));
                string[] MachineID = ddlMachine.Text.ToString().Split('<');
                string MachineInterfaceID = MachineID[0].ToString().Trim();
                Session["downcode"] = null;
                DataTable updategrd = new DataTable();
                updategrd = DataBaseAccess.GetProdDownEventRejectionGridDetails(MachineInterfaceID, StartTime, EndTime, "Down Data");
                Session["DownDataSplitFun"] = updategrd;
                Session["DownData"] = updategrd;
                downdatagrid.DataSource = updategrd;
                downdatagrid.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }



        protected void downdatagrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //commanDataBind();

                    HiddenField hdfdowncode = (e.Row.FindControl("hdfdowncode") as HiddenField);
                    DropDownList ddldowncode = (e.Row.FindControl("DownCode") as DropDownList);
                    //ddldowncode.DataSource = downcodeWithInterfaceID;
                    //ddldowncode.DataTextField = "InterfaceIWithID";
                    //ddldowncode.DataValueField = "InterfaceID";
                    //ddldowncode.DataBind();
                    //if (ddldowncode.Items.FindByValue(hdfdowncode.Value) == null)
                    //{
                    //    ddldowncode.Items.Add(new ListItem(hdfdowncode.Value, hdfdowncode.Value));
                    //}
                    //ddldowncode.SelectedValue = hdfdowncode.Value;
                    DropDownList ddlCategory = e.Row.FindControl("ddlDownCategory") as DropDownList;
                    BindDownCategory(ddlCategory);
                    BindDownCode(ddldowncode, "", hdfdowncode.Value, "databound");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void downdatagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                downdatagrid.PageIndex = e.NewPageIndex;
                DataTable dtDownData = Session["DownData"] as DataTable;
                downdatagrid.DataSource = dtDownData;
                downdatagrid.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsUpdated = false;
                foreach (GridViewRow row in downdatagrid.Rows)
                {
                    HiddenField hdfvalue = (HiddenField)row.FindControl("hdfdownsavecheck");
                    if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                    {
                        string grdid = ((System.Web.UI.WebControls.Label)row.FindControl("lblID")).Text;
                        string grdcomponent = ((System.Web.UI.WebControls.Label)row.FindControl("lblComponentName")).Text;
                        string grdoperation = ((System.Web.UI.WebControls.TextBox)row.FindControl("OperationID")).Text;
                        string grdoperator = ((System.Web.UI.WebControls.Label)row.FindControl("lblOperator")).Text;
                        string grdDowncode = ((DropDownList)row.FindControl("DownCode")).SelectedValue;

                        //string grdpartscount = ((DropDownList)row.FindControl("PartsCount")).SelectedValue;
                        string grdownfromtime = ((System.Web.UI.WebControls.Label)row.FindControl("downFromTime")).Text;
                        string grddowntoTime = ((System.Web.UI.WebControls.Label)row.FindControl("downToTime")).Text;
                        string grdworkorderno = ((System.Web.UI.WebControls.Label)row.FindControl("workorderno")).Text;
                        DataBaseAccess.UpdateProdDownRejectiondata(Convert.ToInt32(grdid), DateTime.Now, DateTime.Now, "", grdcomponent, "", grdoperation, "", grdoperator, "",
                           grdworkorderno, "", 0, 0, "", grdDowncode, "", "", "", "", "Down Data", "GridDownData", "0", out IsUpdated);
                    }
                }
                if (IsUpdated)
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSplit_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime SplitDate = DateTime.Now;
                DateTime ddate = DateTime.Now;
                DateTime Sttime = DateTime.Now;
                DateTime ndtime = DateTime.Now;
                int ID = 0;
                string Type = "ModifiedData";
                Session["Type"] = Type;
                bool success = false;
                string splitdate = txtBreakDate.Text + " " + txtBreaktime.Text;
                int rowIndex = Convert.ToInt32(hdnindex.Value);
                lblDownTimeStart.Text = (downdatagrid.Rows[rowIndex].FindControl("downFromTime") as Label).Text;
                lblDownEndTime.Text = (downdatagrid.Rows[rowIndex].FindControl("downToTime") as Label).Text;
                lblIDs.Text = (downdatagrid.Rows[rowIndex].FindControl("lblID") as Label).Text;
                string NDTime = lblDownEndTime.Text;
                string STTime = lblDownTimeStart.Text;
                string IDD = lblIDs.Text;
                DateTime.TryParse(splitdate, out SplitDate);
                DateTime.TryParse(STTime, out Sttime);
                DateTime.TryParse(NDTime, out ndtime);
                int.TryParse(IDD, out ID);

                Session["ID"] = ID;


                DataTable dt = Session["DownDataSplitFun"] as DataTable;
                if (Session["Type"].ToString().Equals("ModifiedData", StringComparison.OrdinalIgnoreCase))
                {
                    dt = DataBaseAccess.GetModifiedandJobDate(ID, "ModifiedData");
                }

                if (SplitDate <= Sttime)
                {
                    HelperClassGeneric.openModal(this, "splitModel", false);
                    HelperClassGeneric.openWarningModal(this, "Split Date Cannot Be Less Than Down Time Start");
                    return;
                }
                else if (SplitDate >= ndtime)
                {
                    HelperClassGeneric.openModal(this, "splitModel", false);
                    HelperClassGeneric.openWarningModal(this, "Split Date Cannot Be Greater Than Down End Start");
                    return;
                }
                else
                {
                    success = DataBaseAccess.UpdateModifiedSplitData(ID, SplitDate);
                    if (success)
                    {
                        success = DataBaseAccess.InsertModifiedSplitDate(SplitDate, ndtime, dt.Rows[0]);
                        if (success)
                        {
                            HelperClassGeneric.openSuccessModal(this, "Data Split and Inserted Successfully");
                            HelperClassGeneric.clearModal(this);
                            HelperClassGeneric.clearModal(this);
                            BindGrid();
                        }
                        else
                        {
                            HelperClassGeneric.openModal(this, "splitModel", false);
                            HelperClassGeneric.openErrorModal(this, "Try Again");
                            return;
                        }
                    }
                    else
                    {
                        HelperClassGeneric.openModal(this, "splitModel", false);
                        HelperClassGeneric.openErrorModal(this, "Try Again");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


        protected void ddlDownCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["downcode"] = null;
                var row = (sender as DropDownList).NamingContainer as GridViewRow;
                BindDownCode(row.FindControl("DownCode") as DropDownList, (row.FindControl("ddlDownCategory") as DropDownList).SelectedValue, (row.FindControl("hdfdowncode") as HiddenField).Value, "ddl");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnview_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}