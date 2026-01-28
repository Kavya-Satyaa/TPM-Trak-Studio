using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Bajaj.Model;
using Web_TPMTrakDashboard.EnergyModule.Models;

namespace Web_TPMTrakDashboard.EnergyModule
{
    public partial class EnergyMachineMaster : System.Web.UI.Page
    {
        List<string> allCellIDs = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();
                //BindCellIDs();
                BindGrid();
            }
        }

        private void BindDropDown()
        {
            try
            {
                List<string> PlantID = DataBaseAccess.GetAllPlants();
                PlantID.Remove("All");
                ddlPlantID.DataSource = PlantID;
                ddlPlantID.DataBind();

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
                List<EnergyMachinemasterEntity> entities = DataBaseAccess.GetMachineData();

                foreach (GridViewRow row in gridMachineInformation.Rows)
                {
                    Control targetControl = row.FindControl("lblCellID");
                    targetControl.Visible = true;
                    break;
                }
                allCellIDs = DataBaseAccess.GetCellIDs("");
                if (allCellIDs.Count > 0)
                {
                    gridMachineInformation.Columns[3].Visible = true;
                }
                else
                {
                    gridMachineInformation.Columns[3].Visible = false;
                }
                //if (System.Web.Configuration.WebConfigurationManager.AppSettings["KiswokPage"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                //{
                //    gridMachineInformation.Columns[3].Visible = true;
                //}
                //else
                //{
                //    gridMachineInformation.Columns[3].Visible = false;
                //}
                if (entities.Count > 0)
                {
                    gridMachineInformation.DataSource = entities;
                }
                else
                    gridMachineInformation.DataSource = new List<EnergyMachinemasterEntity>();
                gridMachineInformation.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in gridMachineInformation.Rows)
                {
                    if ((Row.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        string Machine = (Row.FindControl("lblMachineID") as Label).Text;
                        string Plant = (Row.FindControl("lblPlantID") as Label).Text;
                        string Interfaceid = (Row.FindControl("lblMachineInterfaceID") as Label).Text;
                        int Result=DataBaseAccess.DeleteEnergyMachindData(Machine,Interfaceid,Plant);
                        if (Result > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openErrorModalMessage", "ErrorMessage('Data Deleted Successfully,Information')", true);
                            BindGrid();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openErrorModalMessage", "ErrorMessage('Data Deletion Failed')", true);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void InsertData(object sender, EventArgs e)
        {
            if (ValidateData(txtAddMachineID.Text, ddlPlantID.SelectedValue, txtAddmachineInterface.Text, txtAddIPaddress.Text, txtAddPortNumber.Text, txtAddSortOrder.Text, "Insert", out string Message))
            {
                DataBaseAccess.SaveEnergyMachindData(txtAddMachineID.Text.Trim(), ddlPlantID.SelectedValue.Trim(), txtAddmachineInterface.Text.Trim(), txtAddIPaddress.Text.Trim(), txtAddPortNumber.Text.Trim(), chkEnabled.Checked, txtAddSortOrder.Text.Trim(), ddlAddMachineType.SelectedValue.ToString(), chkIsDashboardEnabled.Checked);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openErrorModalMessage", "ErrorMessage('Data Saved Successfully,Information')", true);
                BindGrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openErrorModalMessage", "ErrorMessage('" + Message + ",Error')", true);
            }
        }

        private bool ValidateData(string MachineID, string PlantID, string MachineDesc, string IPAddress, string PortNumber, string SortOrderNo, string Type, out string message)
        {
            message = string.Empty;
            try
            {
                message = string.IsNullOrEmpty(MachineID) ? "Machine-ID cannot be Empty" : string.IsNullOrEmpty(PlantID) ? "Plant-ID cannot be Empty" : string.IsNullOrEmpty(MachineDesc) ? "Interface-ID cannot be Empty" : "";
                if (message != null)
                {
                    //List<string> MachineInterfaceID = DataBaseAccess.GetAllMachineInterfaceID();
                    //if (MachineInterfaceID.AsEnumerable().Contains(MachineDesc))
                    //{
                    //    message = @"Please Check Interface ID. \n It is already used in Machine Information";
                    //    return false;
                    //}
                    switch (Type)
                    {
                        case "Insert":
                            foreach (GridViewRow Row in gridMachineInformation.Rows)
                            {
                                if (Row.Cells[2].ToString().Equals(MachineDesc))
                                {
                                    message = "Interdface ID is Duplicate!!";
                                    break;
                                }
                            }
                            break;
                    }
                    if (!string.IsNullOrEmpty(IPAddress))
                    {
                        char chrFullStop = '.';
                        string[] arrOctets = IPAddress.Split(chrFullStop);
                        if (arrOctets.Length != 4)
                        {
                            message = "Invalid IP Address";
                        }
                        //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
                        Int16 MAXVALUE = 255;
                        Int32 temp; // Parse returns Int32
                        foreach (String strOctet in arrOctets)
                        {
                            if (strOctet.Length > 3)
                            {
                                message = "Invalid IP Address";
                                break;
                            }
                            try
                            {
                                temp = int.Parse(strOctet);
                            }
                            catch (Exception)
                            {
                                message = "Invalid IP Address";
                                break;
                            }

                            if (temp > MAXVALUE)
                            {
                                message = "Invalid IP Address";
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return (string.IsNullOrEmpty(message) ? true : false);
        }

        protected void UpdateData(object sender, EventArgs e)
        {
            if (ValidateData(lblUpdateMachine.Text.Trim(), lblUpdatePlantID.Text.Trim(), lblUpdateInterfaceID.Text.Trim(), txtUpdateIPAddress.Text.Trim(), txtUpdatePortNumber.Text.Trim(), txtUpdateSortorder.Text.Trim(), "Update", out string Message))
            {
                DataBaseAccess.SaveEnergyMachindData(lblUpdateMachine.Text, lblUpdatePlantID.Text, lblUpdateInterfaceID.Text, txtUpdateIPAddress.Text, txtUpdatePortNumber.Text, chkUpdateEnabled.Checked, txtUpdateSortorder.Text, ddlUpdateMachineType.SelectedValue.ToString(), chkUpdateDashboardEnabled.Checked);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openErrorModalMessage", "ErrorMessage('Data Saved Successfully,Information')", true);
                BindGrid();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openErrorModalMessage", "ErrorMessage('" + Message + ",Error')", true);
            }
        }

        protected void gridMachineInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //switch (e.CommandName)
                //{
                //    case "Edit":
                //        int RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                //        lblUpdateMachine.Text = gridMachineInformation.Rows[RowIndex].Cells[1].Text;
                //        lblUpdateInterfaceID.Text = gridMachineInformation.Rows[RowIndex].Cells[3].Text;
                //        lblUpdatePlantID.Text = gridMachineInformation.Rows[RowIndex].Cells[2].Text;
                //        txtUpdateIPAddress.Text = gridMachineInformation.Rows[RowIndex].Cells[4].Text;
                //        txtUpdatePortNumber.Text = gridMachineInformation.Rows[RowIndex].Cells[5].Text;
                //        txtUpdateSortorder.Text = gridMachineInformation.Rows[RowIndex].Cells[7].Text;
                //        chkUpdateEnabled.Checked = gridMachineInformation.Rows[RowIndex].Cells[6].Text.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openErrorModalMessage", "ErrorMessage('Error')", true);
                //        break;
                //}
            }
            catch (Exception ex)
            {

            }
        }
        //private void BindCellIDs()
        //{
        //    try
        //    {
        //        List<string> list = DataBaseAccess.GetCellIDs(ddlPlantID.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString());
        //        list.Remove("All");
        //        ddlCellID.DataSource = list;
        //        ddlCellID.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex);
        //    }
        //}
        //protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //BindCellIDs();
        //    HelperClass.openAddEditModal(this, "addmachineModal");
        //}

        protected void btnAddmachineClose_Click(object sender, EventArgs e)
        {
            HelperClass.clearModal(this);
        }
    }

    public class EnergyMachinemasterEntity
    {
        public int Slno { get; set; }
        public string MachineID { get; set; }
        public string MachineType { get; set; }
        public string PlantID { get; set; }
        public string GroupID { get; set; }
        public string MachineInterfaceID { get; set; }
        public string IPAddress { get; set; }
        public string PortNumber { get; set; }
        public bool IsEnabled { get; set; }
        public string SortOrder { get; set; }
        public bool IsDashboardEnabled { get; set; }
    }
}