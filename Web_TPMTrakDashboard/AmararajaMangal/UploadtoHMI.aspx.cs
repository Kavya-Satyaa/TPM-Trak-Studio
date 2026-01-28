using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.AmararajaMangal.DTO;

namespace Web_TPMTrakDashboard.AmararajaMangal
{
    public partial class UploadtoHMI : System.Web.UI.Page
    {
        //public List<string> selected_machineid = new List<string>();
        // public string machineID = "";
        public string downcat = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rejCatHide.Visible = false;
                rejCatLstHide.Visible = false;
                List<ListItem> listType = new List<ListItem>();
                listType.Add(new ListItem() { Text = "Down Code", Value = "DownCode" });
                if (ConfigurationManager.AppSettings["AceDesignersPage"].ToString() == "1")
                {
                    listType.Add(new ListItem() { Text = "Rejection Code", Value = "RejectionCode" });
                    listType.Add(new ListItem() { Text = "Rejection Category", Value = "RejectionCategory" });
                }
                else if (ConfigurationManager.AppSettings["GEAPages"].ToString() == "1")
                {
                   
                }
                else
                {
                    listType.Add(new ListItem() { Text = "Component", Value = "Component" });
                    listType.Add(new ListItem() { Text = "Down Category", Value = "Category" });
                    listType.Add(new ListItem() { Text = "Customer", Value = "Customer" });
                }
                ddlType.DataSource = listType;
                ddlType.DataTextField = "Text";
                ddlType.DataValueField = "Value";
                ddlType.DataBind();
                Session["UploadHMIRowCount"] = null;
                getMaximumRowCount("CharacterLength");
                BindPlant();
                BindMachineID();
                BindCategory();
                btnView_Click(null, null);
                msgheader.InnerText = "Available  " + ddlType.SelectedValue.ToString();
                msgheader1.InnerText = "Assigned  " + ddlType.SelectedValue.ToString();
            }
        }
        private int getMaximumRowCount(string type)
        {
            int count = 10;
            try
            {
                List<CockpitViewSettingClass> list = new List<CockpitViewSettingClass>();
                if (Session["UploadHMIRowCount"] == null)
                {
                    list = AmararajaMangalDataBaseAccess.getShopDefaultValue("UploadToHMIRowCount");
                    Session["UploadHMIRowCount"] = list;
                }
                list = Session["UploadHMIRowCount"] as List<CockpitViewSettingClass>;
                count = list.Where(k => k.ValueInText == type).Select(k => k.ValueInInt).FirstOrDefault();
                if (type.Equals("CharacterLength", StringComparison.OrdinalIgnoreCase))
                {
                    if (count == 0) count = 20;
                    hdnCharLengthCount.Value = count.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return count;
        }
        private void BindPlant()
        {
            try
            {
                List<string> plantlist = AmararajaMangalDataBaseAccess.GetPlantID();
                ddlPlant.DataSource = plantlist;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineID();
        }

        private void BindMachineID()
        {
            try
            {
                List<string> machlst = AmararajaMangalDataBaseAccess.GetMachineID(ddlPlant.SelectedValue.ToString());
                //lstMachine.DataSource = machlst;
                //lstMachine.DataBind();
                ddlMachine.DataSource = machlst;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindCategory()
        {
            try
            {
                List<string> catlst = AmararajaMangalDataBaseAccess.GetCategory();
                lstcategory.DataSource = catlst;
                lstcategory.DataBind();

                catlst = AmararajaMangalDataBaseAccess.GetRejectionCategory();
                lbRejCategory.DataSource = catlst;
                lbRejCategory.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        //private void processmachineID()
        //{
        //    machineID = string.Empty;
        //    foreach (ListItem item in lstMachine.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            machineID += "'" + item.Value + "',";
        //            selected_machineid.Add(item.Value.ToString());
        //        }
        //    }
        //    if (string.IsNullOrEmpty(machineID))
        //    {
        //        foreach (ListItem item in lstMachine.Items)
        //        {
        //            machineID += "'" + item.Value + "',";
        //        }
        //    }
        //    machineID = machineID.TrimEnd(',');
        //}

        private void hideAvailableGridColumn()
        {
            try
            {
                gridavailableinfo.Columns[2].Visible = false;
                gridavailableinfo.Columns[3].Visible = false;
                gridavailableinfo.Columns[4].Visible = false;
                gridavailableinfo.Columns[5].Visible = false;
                gridavailableinfo.Columns[6].Visible = false;
                gridavailableinfo.Columns[7].Visible = false;
                gridavailableinfo.Columns[8].Visible = false;
                gridavailableinfo.Columns[9].Visible = false;
                gridavailableinfo.Columns[10].Visible = false;
                gridavailableinfo.Columns[11].Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindComponentID()
        {

            try
            {
                hideAvailableGridColumn();
                gridavailableinfo.Columns[2].Visible = true;
                gridavailableinfo.Columns[3].Visible = true;
                gridavailableinfo.Columns[4].Visible = true;
                //processmachineID(); //machineID
                List<HMIEntity> entities = AmararajaMangalDataBaseAccess.GetAvailableCompDetails(ddlMachine.SelectedValue);
                if (entities.Count > 0 && entities != null)
                {
                    gridavailableinfo.DataSource = entities;
                    gridavailableinfo.DataBind();
                }
                else
                {
                    entities.Add(new HMIEntity { MachineID = "", InterfaceID = "" });
                    gridavailableinfo.DataSource = entities;
                    gridavailableinfo.DataBind();
                    gridavailableinfo.Rows[0].Visible = false;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindCustomerID()
        {

            try
            {
                hideAvailableGridColumn();
                gridavailableinfo.Columns[10].Visible = true;
                gridavailableinfo.Columns[11].Visible = true;
                //processmachineID(); //machineID
                List<HMIEntity> entities = AmararajaMangalDataBaseAccess.GetAvailableCustomerDetails(ddlMachine.SelectedValue);
                if (entities.Count > 0 && entities != null)
                {
                    gridavailableinfo.DataSource = entities;
                    gridavailableinfo.DataBind();
                }
                else
                {
                    entities.Add(new HMIEntity());
                    gridavailableinfo.DataSource = entities;
                    gridavailableinfo.DataBind();
                    gridavailableinfo.Rows[0].Visible = false;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void processcatagory()
        {
            downcat = string.Empty;
            foreach (ListItem item in lstcategory.Items)
            {
                if (item.Selected)
                    downcat += "'" + item.Value + "',";
            }
            if (string.IsNullOrEmpty(downcat))
            {
                foreach (ListItem item in lstcategory.Items)
                {
                    downcat += "'" + item.Value + "',";
                }
            }
            downcat = downcat.TrimEnd(',');
        }

        private void BindDownData()
        {

            try
            {
                processcatagory();
                hideAvailableGridColumn();
                gridavailableinfo.Columns[8].HeaderText = "Down Catagory";
                gridavailableinfo.Columns[3].Visible = true;
                gridavailableinfo.Columns[6].Visible = true;
                gridavailableinfo.Columns[8].Visible = true;
                //List<HMIEntity> downentities = AmararajaMangalDataBaseAccess.GetAvailableDownInfo(downcat);
                List<HMIEntity> downentities = AmararajaMangalDataBaseAccess.GetAvailableDownInfo(downcat, ddlMachine.SelectedValue);
                if (downentities.Count > 0 && downentities != null)
                {
                    gridavailableinfo.DataSource = downentities;
                    gridavailableinfo.DataBind();
                }
                else
                {
                    downentities.Add(new HMIEntity { MachineID = "", InterfaceID = "" });
                    gridavailableinfo.DataSource = downentities;
                    gridavailableinfo.DataBind();
                    gridavailableinfo.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private string processRejectionCategory()
        {
            string rejCategory = "";
            foreach (ListItem item in lbRejCategory.Items)
            {
                if (item.Selected)
                    rejCategory += "'" + item.Value + "',";
            }
            if (string.IsNullOrEmpty(rejCategory))
            {
                foreach (ListItem item in lbRejCategory.Items)
                {
                    rejCategory += "'" + item.Value + "',";
                }
            }
            rejCategory = rejCategory.TrimEnd(',');
            return rejCategory;
        }


        private void BindRejectionCodeData()
        {
            try
            {
                string rejCat = processRejectionCategory();
                hideAvailableGridColumn();
                gridavailableinfo.Columns[3].Visible = true;
                gridavailableinfo.Columns[7].Visible = true;
                gridavailableinfo.Columns[9].Visible = true;
                List<HMIEntity> list = AmararajaMangalDataBaseAccess.GetAvailableRejectionCodeInfo(rejCat, ddlMachine.SelectedValue);
                if (list.Count > 0 && list != null)
                {
                    gridavailableinfo.DataSource = list;
                    gridavailableinfo.DataBind();
                }
                else
                {
                    list.Add(new HMIEntity { MachineID = "", InterfaceID = "" });
                    gridavailableinfo.DataSource = list;
                    gridavailableinfo.DataBind();
                    gridavailableinfo.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindCategorydata()
        {
            try
            {
                hideAvailableGridColumn();
                gridavailableinfo.Columns[8].HeaderText = "Down Catagory";
                gridavailableinfo.Columns[8].Visible = true;
                List<HMIEntity> catentities = AmararajaMangalDataBaseAccess.GetCategoryInfo(ddlMachine.SelectedValue);
                if (catentities.Count > 0 && catentities != null)
                {

                    gridavailableinfo.DataSource = catentities;
                    gridavailableinfo.DataBind();
                }
                else
                {
                    catentities.Add(new HMIEntity { MachineID = "", InterfaceID = "" });
                    gridavailableinfo.DataSource = catentities;
                    gridavailableinfo.DataBind();
                    gridavailableinfo.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindRejectionCategorydata()
        {
            try
            {
                hideAvailableGridColumn();
                gridavailableinfo.Columns[9].Visible = true;
                List<HMIEntity> catentities = AmararajaMangalDataBaseAccess.GetRejectionCategoryInfo(ddlMachine.SelectedValue);
                if (catentities.Count > 0 && catentities != null)
                {

                    gridavailableinfo.DataSource = catentities;
                    gridavailableinfo.DataBind();
                }
                else
                {
                    catentities.Add(new HMIEntity { MachineID = "", InterfaceID = "" });
                    gridavailableinfo.DataSource = catentities;
                    gridavailableinfo.DataBind();
                    gridavailableinfo.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue.ToString() == "Component")
            {
                BindComponentID();
            }
            else if (ddlType.SelectedValue.ToString() == "Category")
            {
                BindCategorydata();
            }
            else if (ddlType.SelectedValue.ToString() == "Customer")
            {
                BindCustomerID();
            }
            else if (ddlType.SelectedValue.ToString() == "RejectionCode")
            {
                BindRejectionCodeData();
            }
            else if (ddlType.SelectedValue.ToString() == "RejectionCategory")
            {
                BindRejectionCategorydata();
            }
            else
            {
                BindDownData();

            }
            BindMachineWiseInfo();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                msgheader.InnerText = "Available  " + ddlType.SelectedValue.ToString();
                msgheader1.InnerText = "Assigned  " + ddlType.SelectedValue.ToString();
                cathide.Attributes.Add("style", "display:none");
                catlsthide.Attributes.Add("style", "display:none");
                rejCatHide.Visible = false;
                rejCatLstHide.Visible = false;
                if (ddlType.SelectedValue.ToString() == "DownCode")
                {
                    cathide.Attributes.Add("style", "display:;vertical-align: middle;");
                    catlsthide.Attributes.Add("style", "display:;vertical-align: middle;");
                    BindDownData();

                }
                else if (ddlType.SelectedValue.ToString() == "Component")
                {
                    BindComponentID();
                }
                else if (ddlType.SelectedValue.ToString() == "Category")
                {
                    BindCategorydata();
                }
                else if (ddlType.SelectedValue.ToString() == "Customer")
                {
                    BindCustomerID();
                }
                else if (ddlType.SelectedValue.ToString() == "RejectionCode")
                {
                    rejCatHide.Visible = true;
                    rejCatLstHide.Visible = true;
                    BindRejectionCodeData();
                }
                else if (ddlType.SelectedValue.ToString() == "RejectionCategory")
                {
                    BindRejectionCategorydata();
                }
                BindMachineWiseInfo();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                //processmachineID();
                //if (selected_machineid.Count <= 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Select MachineId')", true);
                //    return;
                //}
                List<HMIEntity> downcodelst = new List<HMIEntity>();
                List<HMIEntity> componentlst = new List<HMIEntity>();
                List<HMIEntity> catagorylst = new List<HMIEntity>();
                string downcat = string.Empty, downID = string.Empty, InterfaceID = string.Empty, desc = string.Empty, componentID = string.Empty, machineID = string.Empty, customerID = string.Empty, customerName = string.Empty, rejectionID = "", rejCategory = "";
                int maxRowCount = getMaximumRowCount(ddlType.SelectedValue.ToString());
                if (ddlType.SelectedValue.ToString().Equals("DownCode", StringComparison.OrdinalIgnoreCase) || ddlType.SelectedValue.ToString().Equals("RejectionCode", StringComparison.OrdinalIgnoreCase))
                {
                    string labelName = "lblDownCategory";
                    if (ddlType.SelectedValue.ToString().Equals("RejectionCode", StringComparison.OrdinalIgnoreCase))
                    {
                        labelName = "lblRejCategory";
                    }
                    var checkedRows = gridavailableinfo.Rows.Cast<GridViewRow>().Where(r => ((CheckBox)r.FindControl("Checkcomp")).Checked).Select(k => (k.FindControl(labelName) as Label).Text).ToList();
                    var checkedRowsDistList = checkedRows.Distinct().ToList();
                    string checkedCatList = "";
                    foreach (var cat in checkedRowsDistList)
                    {
                        if (checkedCatList == "")
                        {
                            checkedCatList += "'" + cat + "'";
                        }
                        else
                        {
                            checkedCatList += ",'" + cat + "'";
                        }
                    }
                    List<ListItem> assignedRowCountList = AmararajaMangalDataBaseAccess.getAssignedDownCodeCount(ddlMachine.SelectedValue, checkedCatList, ddlType.SelectedValue);
                    foreach (var cat in checkedRowsDistList)
                    {
                        int checkedCount = checkedRows.Where(k => k == cat).Count();
                        string c = assignedRowCountList.Where(k => k.Text == cat).Select(k => k.Value).FirstOrDefault();
                        int assignedRowCount = string.IsNullOrEmpty(c) ? 0 : Convert.ToInt32(c);
                        if (checkedCount + assignedRowCount > maxRowCount)
                        {
                            int remainingCount = maxRowCount - assignedRowCount;
                            //if (rem != 0)
                            //{
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Select " + rem + " rows only!!')", true);
                            //    return;
                            //}
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('" + maxRowCount + " rows are allowed. Please select " + remainingCount + " rows for Category " + cat + ".')", true);
                            return;
                        }
                    }
                }
                else
                {
                    int checkedCount = gridavailableinfo.Rows.Cast<GridViewRow>().Count(r => ((CheckBox)r.FindControl("Checkcomp")).Checked);
                    int assignedRowCount = AmararajaMangalDataBaseAccess.getComponentCount(ddlType.SelectedValue.ToString(), ddlMachine.SelectedValue);
                    if (checkedCount + assignedRowCount > maxRowCount)
                    {
                        int remainingCount = maxRowCount - assignedRowCount;
                        //if (rem != 0)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('Select " + rem + " rows only!!')", true);
                        //    return;
                        //}
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "alert('" + maxRowCount + " rows are allowed. Please select " + remainingCount + " rows.')", true);
                        return;
                    }
                }
                for (int i = 0; i < gridavailableinfo.Rows.Count; i++)
                {
                    bool check = (gridavailableinfo.Rows[i].FindControl("Checkcomp") as CheckBox).Checked;
                    machineID = (gridavailableinfo.Rows[i].FindControl("lblmachineID") as Label).Text;
                    downcat = (gridavailableinfo.Rows[i].FindControl("lblDownCategory") as Label).Text;
                    downID = (gridavailableinfo.Rows[i].FindControl("lblDownID") as Label).Text;
                    InterfaceID = (gridavailableinfo.Rows[i].FindControl("lblInterfaceID") as Label).Text;
                    desc = (gridavailableinfo.Rows[i].FindControl("lblDescription") as Label).Text;
                    customerID = (gridavailableinfo.Rows[i].FindControl("lblCustomerID") as Label).Text;
                    customerName = (gridavailableinfo.Rows[i].FindControl("lblCustomerName") as Label).Text;
                    componentID = (gridavailableinfo.Rows[i].FindControl("lblComponentID") as Label).Text;
                    rejectionID = (gridavailableinfo.Rows[i].FindControl("lblRejectionID") as Label).Text;
                    rejCategory = (gridavailableinfo.Rows[i].FindControl("lblRejCategory") as Label).Text;
                    if (check == true)
                    {
                        //for (int j = 0; j < selected_machineid.Count; j++)
                        //{
                        //    if (ddlType.SelectedValue.ToString().Equals("DownCode", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        AmararajaMangalDataBaseAccess.inserttoDownmaster(downcat, downID, desc, InterfaceID, selected_machineid[j].ToString());
                        //    }
                        //    else if (ddlType.SelectedValue.ToString().Equals("Category", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        AmararajaMangalDataBaseAccess.inserttoCategoryMaster(downcat, selected_machineid[j].ToString());
                        //    }
                        //}
                        if (ddlType.SelectedValue.ToString().Equals("DownCode", StringComparison.OrdinalIgnoreCase))
                        {
                            AmararajaMangalDataBaseAccess.inserttoDownmaster(downcat, downID, desc, InterfaceID, ddlMachine.SelectedValue);
                        }
                        else if (ddlType.SelectedValue.ToString().Equals("Category", StringComparison.OrdinalIgnoreCase))
                        {
                            AmararajaMangalDataBaseAccess.inserttoCategoryMaster(downcat, ddlMachine.SelectedValue);
                        }
                        else if (ddlType.SelectedValue.ToString().Equals("Component", StringComparison.OrdinalIgnoreCase))
                        {
                            AmararajaMangalDataBaseAccess.inserttoComponentMaster(machineID, componentID, InterfaceID, desc);

                        }
                        else if (ddlType.SelectedValue.ToString().Equals("Customer", StringComparison.OrdinalIgnoreCase))
                        {
                            AmararajaMangalDataBaseAccess.inserttoCustomerMaster(ddlMachine.SelectedValue, customerID, customerName);

                        }
                        else if (ddlType.SelectedValue.ToString().Equals("RejectionCode", StringComparison.OrdinalIgnoreCase))
                        {
                            AmararajaMangalDataBaseAccess.inserttoRejectionCodeMaster(rejCategory, rejectionID, desc, InterfaceID, ddlMachine.SelectedValue);
                        }
                        else if (ddlType.SelectedValue.ToString().Equals("RejectionCategory", StringComparison.OrdinalIgnoreCase))
                        {
                            AmararajaMangalDataBaseAccess.insertToRejectionCategoryMaster(rejCategory, ddlMachine.SelectedValue);
                        }
                    }
                }
                // BindMachineWiseInfo();
                btnView_Click(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

        }

        private void BindMachineWiseInfo()
        {
            try
            {
                // processmachineID();
                string machineID = ddlMachine.SelectedValue;
                gridassignedinfo.Columns[2].Visible = false;
                gridassignedinfo.Columns[3].Visible = false;
                gridassignedinfo.Columns[4].Visible = false;
                gridassignedinfo.Columns[5].Visible = false;
                gridassignedinfo.Columns[6].Visible = false;
                gridassignedinfo.Columns[7].Visible = false;
                gridassignedinfo.Columns[8].Visible = false;
                gridassignedinfo.Columns[9].Visible = false;
                gridassignedinfo.Columns[10].Visible = false;
                if (ddlType.SelectedValue.ToString() == "DownCode")
                {
                    processcatagory();
                    List<HMIEntity> hMIEntities = AmararajaMangalDataBaseAccess.GetMachineWiseDownMaster(machineID, downcat);
                    gridassignedinfo.Columns[2].Visible = true;
                    gridassignedinfo.Columns[4].Visible = true;
                    gridassignedinfo.Columns[6].Visible = true;
                    gridassignedinfo.Columns[8].Visible = true;
                    gridassignedinfo.DataSource = hMIEntities;
                    gridassignedinfo.DataBind();
                }
                else if (ddlType.SelectedValue.ToString() == "Category")
                {

                    List<HMIEntity> hMIEntities1 = AmararajaMangalDataBaseAccess.GetMachineWiseCategoryMaster(machineID);
                    gridassignedinfo.Columns[2].Visible = true;
                    gridassignedinfo.DataSource = hMIEntities1;
                    gridassignedinfo.DataBind();
                }
                else if (ddlType.SelectedValue.ToString() == "Customer")
                {
                    List<HMIEntity> hMIEntities2 = AmararajaMangalDataBaseAccess.GetMachineWiseCustomerMaster(machineID);
                    gridassignedinfo.Columns[9].Visible = true;
                    gridassignedinfo.Columns[10].Visible = true;
                    gridassignedinfo.DataSource = hMIEntities2;
                    gridassignedinfo.DataBind();
                }
                else if (ddlType.SelectedValue.ToString() == "RejectionCode")
                {
                    List<HMIEntity> hMIEntities = AmararajaMangalDataBaseAccess.GetMachineWiseRejectionCodeMaster(machineID, processRejectionCategory());
                    gridassignedinfo.Columns[3].Visible = true;
                    gridassignedinfo.Columns[5].Visible = true;
                    gridassignedinfo.Columns[6].Visible = true;
                    gridassignedinfo.Columns[8].Visible = true;
                    gridassignedinfo.DataSource = hMIEntities;
                    gridassignedinfo.DataBind();
                }
                else if (ddlType.SelectedValue.ToString() == "RejectionCategory")
                {

                    List<HMIEntity> hMIEntities1 = AmararajaMangalDataBaseAccess.GetMachineWiseRejectionCategoryMaster(machineID);
                    gridassignedinfo.Columns[3].Visible = true;
                    gridassignedinfo.DataSource = hMIEntities1;
                    gridassignedinfo.DataBind();
                }
                else
                {
                    List<HMIEntity> hMIEntities2 = AmararajaMangalDataBaseAccess.GetMachineWiseComponentMaster(machineID);

                    gridassignedinfo.Columns[6].Visible = true;
                    gridassignedinfo.Columns[7].Visible = true;
                    gridassignedinfo.Columns[8].Visible = true;
                    gridassignedinfo.DataSource = hMIEntities2;
                    gridassignedinfo.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

        }

        protected void btndelte_Click(object sender, EventArgs e)
        {
            string sucessfailure = string.Empty;
            int Index = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
            string machineID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssmachineID")).Text;
            string downID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssDownID")).Text;
            string component = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssComponentID")).Text;
            string catagory = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssDownCategory")).Text;
            string InterfaceID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssInterfaceID")).Text;
            string customerID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssCustomerId")).Text;
            string rejectionID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssRejectionID")).Text;
            string rejCategory = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssRejCategory")).Text;
            try
            {
                if (ddlType.SelectedValue.ToString() == "DownCode")
                {
                    AmararajaMangalDataBaseAccess.DeleteDownInformation(machineID, downID, catagory, out sucessfailure);
                }
                else if (ddlType.SelectedValue.ToString() == "Component")
                {
                    AmararajaMangalDataBaseAccess.DeleteComponentInformation(machineID, component, out sucessfailure);
                }
                else if (ddlType.SelectedValue.ToString() == "Customer")
                {
                    AmararajaMangalDataBaseAccess.DeleteCustomerInformation(machineID, customerID, out sucessfailure);
                }
                else if (ddlType.SelectedValue.ToString() == "RejectionCode")
                {
                    AmararajaMangalDataBaseAccess.DeleteRejectionCodeInformation(machineID, rejectionID, rejCategory, out sucessfailure);
                }
                else if (ddlType.SelectedValue.ToString() == "RejectionCategory")
                {
                    AmararajaMangalDataBaseAccess.DeleteRejectionCatagoryInformation(machineID, rejCategory, out sucessfailure);
                }
                else
                {
                    AmararajaMangalDataBaseAccess.DeleteCatagoryInformation(machineID, catagory, out sucessfailure);
                }

                if (sucessfailure.Equals("Successfull"))
                {
                    lblMessages.Text = "Data Successfully Deleted";
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    //BindMachineWiseInfo();
                    btnView_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void chklst_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                if (chk.Checked == true)
                {
                    for (int i = 0; i < gridavailableinfo.Rows.Count; i++)
                    {
                        (gridavailableinfo.Rows[i].FindControl("Checkcomp") as CheckBox).Checked = true;
                    }
                }
                else
                {
                    for (int i = 0; i < gridavailableinfo.Rows.Count; i++)
                    {
                        (gridavailableinfo.Rows[i].FindControl("Checkcomp") as CheckBox).Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string sucessfailure = string.Empty;
                // int Index = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
                for (int Index = 0; Index < gridassignedinfo.Rows.Count; Index++)
                {
                    if ((gridassignedinfo.Rows[Index].FindControl("chkDelete") as CheckBox).Checked)
                    {
                        string machineID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssmachineID")).Text;
                        string downID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssDownID")).Text;
                        string component = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssComponentID")).Text;
                        string catagory = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssDownCategory")).Text;
                        string InterfaceID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssInterfaceID")).Text;
                        string customerID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssCustomerId")).Text;
                        string rejectionID = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssRejectionID")).Text;
                        string rejCategory = ((Label)gridassignedinfo.Rows[Index].FindControl("lblAssRejCategory")).Text;
                        try
                        {
                            if (ddlType.SelectedValue.ToString() == "DownCode")
                            {
                                AmararajaMangalDataBaseAccess.DeleteDownInformation(machineID, downID, catagory, out sucessfailure);
                            }
                            else if (ddlType.SelectedValue.ToString() == "Component")
                            {
                                AmararajaMangalDataBaseAccess.DeleteComponentInformation(machineID, component, out sucessfailure);
                            }
                            else if (ddlType.SelectedValue.ToString() == "Customer")
                            {
                                AmararajaMangalDataBaseAccess.DeleteCustomerInformation(machineID, customerID, out sucessfailure);
                            }
                            else if (ddlType.SelectedValue.ToString() == "RejectionCode")
                            {
                                AmararajaMangalDataBaseAccess.DeleteRejectionCodeInformation(machineID, rejectionID, rejCategory, out sucessfailure);
                            }
                            else if (ddlType.SelectedValue.ToString() == "RejectionCategory")
                            {
                                AmararajaMangalDataBaseAccess.DeleteRejectionCatagoryInformation(machineID, rejCategory, out sucessfailure);
                            }
                            else
                            {
                                AmararajaMangalDataBaseAccess.DeleteCatagoryInformation(machineID, catagory, out sucessfailure);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex.ToString());
                        }
                    }
                }
                if (sucessfailure.Equals("Successfull"))
                {
                    lblMessages.Text = "Data Successfully Deleted";
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                    //BindMachineWiseInfo();
                    btnView_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void chklst1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                if (chk.Checked == true)
                {
                    for (int i = 0; i < gridassignedinfo.Rows.Count; i++)
                    {
                        (gridassignedinfo.Rows[i].FindControl("chkDelete") as CheckBox).Checked = true;
                    }
                }
                else
                {
                    for (int i = 0; i < gridassignedinfo.Rows.Count; i++)
                    {
                        (gridassignedinfo.Rows[i].FindControl("chkDelete") as CheckBox).Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }
}