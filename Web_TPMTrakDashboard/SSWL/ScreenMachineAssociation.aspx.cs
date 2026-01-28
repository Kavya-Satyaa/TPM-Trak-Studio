using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.SSWL.Model;

namespace Web_TPMTrakDashboard.SSWL
{
    public partial class ScreenMachineAssociation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindScreen();
                BindMachineID();
                foreach (ListItem item in lbScreen.Items)
                {
                    item.Selected = true;
                }
                foreach (ListItem item in lbMachine.Items)
                {
                    item.Selected = true;
                }
                BindData();
            }
        }
        private void BindScreen()
        {
            try
            {
                lbScreen.DataSource = SSWLDBAccess.getScreenNames();
                lbScreen.DataBind();
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
                lbMachine.DataSource = VDGDataBaseAccess.GetAllMachines("");
                lbMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindData()
        {
            try
            {
                chkSelectAll.Checked = false;
                string screens = "", machines = "";
                foreach (ListItem item in lbScreen.Items)
                {
                    if (item.Selected)
                    {
                        if (screens == "")
                            screens += "'" + item.Value + "'";
                        else
                            screens += ",'" + item.Value + "'";
                    }
                }
                foreach (ListItem item in lbMachine.Items)
                {
                    if (item.Selected)
                    {
                        if (machines == "")
                            machines += "'" + item.Value + "'";
                        else
                            machines += ",'" + item.Value + "'";
                    }
                }
                List<ScreenMachineEntity> list = new List<ScreenMachineEntity>();
                DataTable dt = SSWLDBAccess.getScreenMachineAssociationData(screens, machines);
                if (dt.Rows.Count > 0)
                {
                    int header = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ScreenMachineEntity data = new ScreenMachineEntity();
                        if (header == 0)
                        {
                            data.ScreenName = "Screen";
                            data.IsHeader = true;
                        }
                        else
                        {
                            data.ScreenName = dt.Rows[i][0].ToString();
                            data.IsHeader = false;
                        }
                        List<ScreenMachineEntity> machieList = new List<ScreenMachineEntity>();
                        for (int j = 1; j < dt.Columns.Count; j++)
                        {
                            ScreenMachineEntity machineData = new ScreenMachineEntity();
                            if (header == 0)
                            {
                                machineData.MachineName = dt.Columns[j].ColumnName.ToString();
                                machineData.IsHeader = true;
                            }
                            else
                            {
                                machineData.MachineSelect = dt.Rows[i][j].ToString().Equals("1") ? true : false;
                                machineData.IsHeader = false;
                            }
                            machieList.Add(machineData);
                        }
                        data.MachineList = machieList;
                        list.Add(data);
                        if (header == 0)
                        {
                            header++;
                            i--;
                        }
                    }
                }
                lvScreenData.DataSource = list;
                lvScreenData.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvScreenData.Items.Count > 0)
                {
                    int updateCount = 0;
                    ListView machineDataHeader = (lvScreenData.Items[0].FindControl("lvMachine") as ListView);
                    for (int i = 1; i < lvScreenData.Items.Count; i++)
                    {
                        var row = lvScreenData.Items[i];
                        string screenName = (row.FindControl("lblScreenName") as Label).Text;
                       
                        ListView machineData = (row.FindControl("lvMachine") as ListView);
                        for (int j = 0; j < machineData.Items.Count; j++)
                        {
                            var machineRow = machineData.Items[j];
                            CheckBox chkSelect = machineRow.FindControl("chkSelect") as CheckBox;
                            string machineID = (machineDataHeader.Items[j].FindControl("lblMachine") as Label).Text;
                            if (chkSelect.Checked)
                            {
                                updateCount += SSWLDBAccess.insertDeleteScreenMachineAssociation(screenName, machineID, "insert");
                               
                            }
                            else
                            {
                                updateCount += SSWLDBAccess.insertDeleteScreenMachineAssociation(screenName, machineID, "delete");
                            }
                            if (updateCount < 0)
                            {
                                updateCount = 0;
                            }
                        }
                    }
                    if (updateCount > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "openSuccessModal('Saved Successfully.')", true);
                    }
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}