using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class MachineShiftTypeAssociation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantID();
                BindShiftType();
                BindMachineShiftType();
            }
        }
        private void BindShiftType()
        {
            try
            {
                List<ShiftDataModel> list = DBAccess.GetShiftType();
                if(list.Count>0)
                {
                    list.Insert(0,new ShiftDataModel { ShiftType = "All" });
                }
                ddlShiftType.DataSource = list.AsEnumerable().Select(x => x.ShiftType).Distinct().ToList();
                ddlShiftType.DataBind();
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
                lstMachine.DataSource = list;
                lstMachine.DataTextField = "Text";
                lstMachine.DataValueField = "Value";
                lstMachine.DataBind();

                foreach (ListItem Item in lstMachine.Items)
                {
                    Item.Selected = true;
                }
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
        private void BindMachineShiftType()
        {
            try
            {
                DataTable dt = new DataTable();
                string machine = string.Empty;
                foreach(ListItem Item in lstMachine.Items)
                {
                    if(Item.Selected)
                    {
                        machine += "'" + Item.Value + "',";
                    }
                    else
                    {
                        machine += "";
                    }
                }
                machine = machine.Trim(',');
                dt = DBAccess.GetMachineShiftTypeAssocDetails(ddlCell.SelectedValue.ToString(),machine, ddlShiftType.SelectedValue);
                List<MachineShiftTypeModel> list = new List<MachineShiftTypeModel>();
                MachineShiftTypeModel data = new MachineShiftTypeModel();

                if (dt.Columns.Count > 2)
                {
                    data = new MachineShiftTypeModel();
                    data.Machine = "Machine";
                    data.HeaderColor = "#2e6886";
                    data.ForeHeaderColor = "white";

                    List<MachineShiftTypeModel> listShiftType = new List<MachineShiftTypeModel>();
                    MachineShiftTypeModel ent = null;
                    for (int col = 2; col < dt.Columns.Count; col++)
                    {
                        ent = new MachineShiftTypeModel();
                        ent.ShiftType = dt.Columns[col].ColumnName;
                        ent.HeaderVisible = "visible";
                        ent.ContentVisible = "collapse";
                        ent.HeaderColor = "#2e6886";
                        ent.ForeHeaderColor = "white";
                        ent.HeaderChecked = dt.AsEnumerable().All(x => x.Field<dynamic>(dt.Columns[col].ColumnName).Equals(1));
                        listShiftType.Add(ent);
                    }
                    data.machineShiftslst = listShiftType;
                    list.Add(data);
                }
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    data = new MachineShiftTypeModel();
                    data.Machine = dt.Rows[row]["MachineID"].ToString();
                    data.EffDate = Util.GetDateTime(dt.Rows[row]["EffectiveFromDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    data .HeaderColor = "white";

                    List<MachineShiftTypeModel> listContent = new List<MachineShiftTypeModel>();
                    MachineShiftTypeModel entContent = null;
                    for (int col = 2; col < dt.Columns.Count; col++)
                    {
                        entContent = new MachineShiftTypeModel();
                        entContent.ShiftType = dt.Columns[col].ColumnName;
                        entContent.ISShiftTypeChecked = Convert.ToBoolean(dt.Rows[row][col]);
                        entContent.HeaderVisible = "collapse";
                        entContent.ContentVisible = "visible";
                        entContent.HeaderColor = "white";
                        listContent.Add(entContent);
                    }
                    data.machineShiftslst = listContent;
                    list.Add(data);
                }
                lvMachineShiftTypeAssoc.DataSource = list;
                lvMachineShiftTypeAssoc.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindMachineShiftType();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int isupdated = 0;
                
                for(int i=1;i<lvMachineShiftTypeAssoc.Items.Count;i++)
                {
                    string MachineID = (lvMachineShiftTypeAssoc.Items[i].FindControl("lblMachine") as Label).Text;
                    string EffectiveDate = (lvMachineShiftTypeAssoc.Items[i].FindControl("hfDate") as HiddenField).Value;
                    string EffectiveToDate = string.Empty;
                    string ExistShiftType = string.Empty;
                    bool ExistShiftTypeCurrentTime = false;
                    ListView innerLisview = lvMachineShiftTypeAssoc.Items[i].FindControl("lvShiftTypeDetails") as ListView;
                    for(int j=0;j<innerLisview.Items.Count;j++)
                    {
                        MachineShiftTypeModel data = new MachineShiftTypeModel();
                        if((innerLisview.Items[j].FindControl("rbShiftType") as RadioButton).Checked)
                        {
                            data.ShiftType = (innerLisview.Items[j].FindControl("hfShiftType") as HiddenField).Value;
                            ExistShiftType = DBAccess.CheckShiftTypeForMachine(MachineID, EffectiveDate);
                            ExistShiftTypeCurrentTime = DBAccess.CheckShiftTypeForCurrentDate(MachineID,data.ShiftType);
                            if(ExistShiftTypeCurrentTime)
                            {
                                continue;
                            }
                            if (!String.IsNullOrEmpty(ExistShiftType) && ExistShiftType!=data.ShiftType)
                            {
                                EffectiveToDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            isupdated += DBAccess.SaveMachineShiftTypeDetails(MachineID, EffectiveDate, EffectiveToDate, data.ShiftType);
                        }
                    }
                }
                if(isupdated!=0)
                {
                    HelperClass.openInsertSuccessModal(this);
                    BindMachineShiftType();
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}