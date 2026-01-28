using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class UpdateComponentStore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("~/SignIn.aspx", false);
                }
                if (!IsPostBack)
                {
                    BindPlantID();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindData()
        {
            try
            {
                List<NextComponentEntity> list = DBAccess.GetNextComponentOperationData(ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString(), ddlMachine.SelectedValue.ToString());
                gvNextCompOpnSelection.DataSource = list;
                gvNextCompOpnSelection.DataBind();
                Session["UpdateStoreDetails"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlants();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
                BindCellID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindCellID()
        {
            try
            {
                List<string> list = DBAccess.getCellAssignedForEmployee(ddlPlant.SelectedValue, Session["UserName"].ToString());
                if (list.Count > 1)
                {
                    list.Insert(0, "All");
                }
                ddlCell.DataSource = list;
                ddlCell.DataBind();
                BindMachineID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindMachineID()
        {
            try
            {
                string cell = "";
                if (ddlCell.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ListItem item in ddlCell.Items)
                    {
                        if (cell == "")
                        {
                            cell += "'" + item.Value + "'";
                        }
                        else
                        {
                            cell += ",'" + item.Value + "'";
                        }
                    }
                }
                else
                {
                    cell = "'" + ddlCell.SelectedValue + "'";
                }
                List<ListItem> list = DBAccess.getMachineIdInterfaceIDByPlantCell(ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue, cell, false);
                if (list.Count > 0)
                {
                    list.Insert(0, new ListItem() { Text = "All", Value = "All" });
                }
                ddlMachine.DataSource = list;
                ddlMachine.DataTextField = "Text";
                ddlMachine.DataValueField = "Value";
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellID();
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                List<string> prioritylst = new List<string>();
                for (int i = 0; i < gvNextCompOpnSelection.Rows.Count; i++)
                {
                    var row = gvNextCompOpnSelection.Rows[i];
                    if ((row.FindControl("hdnUpdate") as HiddenField).Value == "update")
                    {
                        NextComponentEntity data = new NextComponentEntity();
                        data.PlantID = (row.FindControl("lblPlant") as Label).Text;
                        data.GroupID = (row.FindControl("lblCell") as Label).Text;
                        data.Machineid = (row.FindControl("lblMachine") as Label).Text;
                        data.ComponentID = (row.FindControl("lblComponent") as Label).Text;
                        data.OperationNo = (row.FindControl("lblOperation") as Label).Text;
                        data.Status= (row.FindControl("lblStatus") as Label).Text;
                        data.Priority = (row.FindControl("txtPriority") as TextBox).Text;
                        data.NextComponent = (row.FindControl("chkNxtPart") as CheckBox).Checked;

                        List<NextComponentEntity> list = Session["UpdateStoreDetails"] as List<NextComponentEntity>;
                        if (data.NextComponent==true && string.IsNullOrEmpty(data.Priority))
                        {
                            
                            HelperClassGeneric.openWarningToastrModal(this, "Priority cannot be Empty!!");
                            (row.FindControl("chkNxtPart") as CheckBox).Checked = false;
                            (row.FindControl("hdnUpdate") as HiddenField).Value = "";
                            return;
                        }

                        
                        if(list.AsEnumerable().Where(x => x.Machineid == data.Machineid && x.ComponentID!=data.ComponentID).Where(x=>x.Priority!=null && x.Priority.Equals(data.Priority)).Count()>0)
                        {
                            HelperClassGeneric.openWarningToastrModal(this, "Priority No Already Exists!!");
                            (row.FindControl("chkNxtPart") as CheckBox).Checked = false;
                            (row.FindControl("txtPriority") as TextBox).Text = string.Empty;
                            return;
                        }
                        result += DBAccess.SaveNextPartSelectionDetails(data);
                    }
                }
                if (result > 0)
                {
                    HelperClassGeneric.openSuccessModal(this, "Saved Successfully.");
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}