using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SupplierCodeTafe : System.Web.UI.Page
    {
        List<SupplierCodeEntity> SupCodeData = new List<SupplierCodeEntity>();
        List<SupplierCodeComponentIDEntity> SupCodeComIDData = new List<SupplierCodeComponentIDEntity>();
        DataTable SupCompDetails = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!(IsPostBack))
            {
                //BindSupplierCode();
                //BindComponentID();
                BindSupplierGrid();
                BindSupplierComponentGrid();
            }
        }

        //private void BindSupplierCode()
        //{
        //    try
        //    {
        //        List<string> SupplierCode = DataBaseAccess.GetSupplierCode();
        //        Session["SupplierCode"] = SupplierCode;
        //        SupplierCode.Insert(0, "All");
        //        ddlSupplierCode.DataSource = SupplierCode;
        //        ddlSupplierCode.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());

        //    }

        //}

        //private void BindComponentID()
        //{
        //    try
        //    {
        //        List<string> ComponentID = DataBaseAccess.GetComponentID();
        //        Session["compid"] = ComponentID;
        //        ComponentID.Insert(0, "All");
        //        ddlComponentID.DataSource = ComponentID;
        //        ddlComponentID.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());

        //    }

        //}

        private void BindSupplierGrid()
        {
            try
            {
                Session["SupplierData"] = SupCodeData = DataBaseAccess.GetGridSupCodeDAta();
                if (SupCodeData != null)
                {
                    GridViewSupplierCode.DataSource = SupCodeData;
                    GridViewSupplierCode.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }

        }

        private void BindSupplierComponentGrid()
        {
            try
            {
                //string SupplierCode = "";
                //string ComponentID = "";
                //if (ddlSupplierCode.SelectedValue != null || !string.IsNullOrEmpty(ddlSupplierCode.SelectedValue.ToString()))
                //    SupplierCode = ddlSupplierCode.SelectedValue.ToString();
                //if (ddlComponentID.SelectedValue != null || !string.IsNullOrEmpty(ddlComponentID.SelectedValue.ToString()))
                //    ComponentID = ddlComponentID.SelectedValue.ToString();
                //Session["SupCodeComIDData"] = SupCodeComIDData = DataBaseAccess.GetGridSupComponentID(SupplierCode, ComponentID,Session["compid"] as List<string>,Session["SupplierCode"] as List<string>);
                Session["SupCompDetails"] = SupCompDetails = DataBaseAccess.GetSupComponentDetails();
                ClearDataGridColumns();
                if (SupCompDetails != null)
                {
                    foreach (DataColumn dataColumn in SupCompDetails.Columns)
                    {
                        if (dataColumn.ColumnName.Equals("Part", StringComparison.OrdinalIgnoreCase))
                        {
                            BoundField boundField = new BoundField();
                            boundField.HeaderText = "Component";
                            boundField.DataField = dataColumn.ColumnName;
                            boundField.HeaderStyle.Width = 180;
                            boundField.ItemStyle.Width = 180;
                            GridViewComponentSupplier.Columns.Add(boundField);
                        }
                        else
                        {
                            CheckBoxField checkBoxField = new CheckBoxField();
                            checkBoxField.HeaderText = dataColumn.ColumnName;
                            checkBoxField.DataField = dataColumn.ColumnName;
                            //checkBoxField.HeaderStyle.Width = 170;
                            //checkBoxField.ItemStyle.Width = 170;
                            GridViewComponentSupplier.Columns.Add(checkBoxField);
                        }
                    }
                    GridViewComponentSupplier.DataSource = SupCompDetails;
                    GridViewComponentSupplier.DataBind();
                }
                //if (SupCodeComIDData != null)
                //{
                //    GridViewComponentSupplier.DataSource = SupCodeComIDData;
                //    GridViewComponentSupplier.DataBind();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }

        }

        private void ClearDataGridColumns()
        {
            if (GridViewComponentSupplier.Columns.Count > 0)
            {
                for (int i = GridViewComponentSupplier.Columns.Count - 1; i >= 0; i--)
                {
                    GridViewComponentSupplier.Columns.RemoveAt(i);
                }
            }
        }
        protected void btnSupplierAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSupplierAdd.Text.Equals("Add", StringComparison.OrdinalIgnoreCase))
                {
                    btnSupplierAdd.Text = "Cancel";
                    if (Session["SupplierData"] != null)
                        SupCodeData = Session["SupplierData"] as List<SupplierCodeEntity>;
                    if (SupCodeData != null)
                    {

                        SupplierCodeEntity newSup = new SupplierCodeEntity();
                        newSup.SLNo = SupCodeData.Count + 1;
                        newSup.SupCode = "";
                        SupCodeData.Add(newSup);
                        List<SupplierCodeEntity> dataadd = new List<SupplierCodeEntity>();
                        dataadd = SupCodeData.OrderByDescending(x => x.SLNo).ToList();
                        GridViewSupplierCode.DataSource = dataadd;
                        GridViewSupplierCode.DataBind();
                    }
                    GridViewSupplierCode.Rows[0].BackColor = ColorTranslator.FromHtml("#000000");

                }
                else
                {
                    btnSupplierAdd.Text = "Add";
                    BindSupplierGrid();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }

        }

        protected void btnSupplierUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                
                foreach (GridViewRow rows in GridViewSupplierCode.Rows)
                {
                    string txtsupcode = (rows.FindControl("txtSupCode") as TextBox).Text;
                    string hidsupval = (rows.FindControl("SuppliergridHiddenfield") as Label).Text;
                    string idd = (rows.FindControl("idd") as Label).Text;
                    if ((!(hidsupval.Equals(txtsupcode))))
                    {
                        bool cansave = check(hidsupval);
                        if (!cansave)
                        {
                            if (string.IsNullOrEmpty(txtsupcode))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Supplier Code Cannot be Empty')", true);
                                return;
                            }
                            if(DataBaseAccess.Checkdsupdataata(txtsupcode))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot insert Duplicate Supplier code')", true);
                                return;
                            }
                            cansave = DataBaseAccess.SaveSupplierCode(txtsupcode, idd);
                            if (cansave)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code updated successfully')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code cannot be updated')", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Update Supplier code because it is already used')", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                btnSupplierAdd.Text = "Add";
                //BindSupplierCode();
                BindSupplierGrid();
                BindSupplierComponentGrid();
            }
        }

        private bool check(string hidsupval)
        {
            bool ok = false;
            try
            {
                foreach (GridViewRow rows in GridViewComponentSupplier.Rows)
                {
                    string HidfieldSuppCode = (rows.FindControl("ddlSupplierCode") as DropDownList).SelectedValue;
                    if ((HidfieldSuppCode.Equals(hidsupval, StringComparison.OrdinalIgnoreCase)))
                    {
                        ok = true;
                        return ok;
                    }
                    else
                    {
                        ok = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            return ok;
        }

        protected void btnSupplierDelete_Click(object sender, EventArgs e)
        {
            try
            {
                btnSupplierAdd.Text = "Add";
                foreach (GridViewRow rows in GridViewSupplierCode.Rows)
                {

                    string deletesupcode = (rows.FindControl("deletesupcode") as HiddenField).Value;
                    if ((deletesupcode.Equals("true", StringComparison.OrdinalIgnoreCase)))
                    {
                        string hidsupval = (rows.FindControl("SuppliergridHiddenfield") as Label).Text;
                        bool cansave = check(hidsupval);
                        if (!cansave)
                        {
                            string idd = (rows.FindControl("idd") as Label).Text;
                            bool delsuccessful = DataBaseAccess.DeleteSupCode(hidsupval, idd);
                            if (delsuccessful)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code deleted successfully')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code cannot be deleted')", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Delete Supplier code because it is already used')", true);
                        }
                    }
                  
                }
               
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                //BindSupplierCode();
                BindSupplierGrid();
                BindSupplierComponentGrid();
            }
        }

        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (btnAdd.Text.Equals("Add", StringComparison.OrdinalIgnoreCase))
        //        {
        //            btnAdd.Text = "Cancel";
        //            if (Session["SupCodeComIDData"] != null)
        //                SupCodeComIDData = Session["SupCodeComIDData"] as List<SupplierCodeComponentIDEntity>;
        //            if (SupCodeComIDData != null)
        //            {
        //                SupplierCodeComponentIDEntity newd = new SupplierCodeComponentIDEntity();
        //                newd.SLNO = SupCodeComIDData.Count + 1;
        //                List<string> comp = Session["compid"] as List<string>;
        //                comp.Remove("All");
        //                newd.lstComponentID = comp;
        //                List<string> sup = Session["SupplierCode"] as List<string>;
        //                sup.Remove("All");
        //                newd.lstSupplierCode = sup;
        //                newd.ComponentID = newd.lstComponentID[0];
        //                newd.SupplierCode = newd.lstSupplierCode[0];
        //                newd.lblComponentID = "";
        //                newd.lblSupplierCode = "";
        //                SupCodeComIDData.Add(newd);
        //                List<SupplierCodeComponentIDEntity> datanew = new List<SupplierCodeComponentIDEntity>();
        //                datanew = SupCodeComIDData.OrderByDescending(x => x.SLNO).ToList();
        //                GridViewComponentSupplier.DataSource = datanew;
        //                GridViewComponentSupplier.DataBind();
        //            }
        //            else
        //            {
        //                SupCodeComIDData = new List<SupplierCodeComponentIDEntity>();
        //                SupplierCodeComponentIDEntity newd = new SupplierCodeComponentIDEntity();
        //                newd.SLNO = SupCodeComIDData.Count + 1;
        //                newd.lstComponentID = Session["compid"] as List<string>;
        //                newd.lstSupplierCode = Session["SupplierCode"] as List<string>;
        //                newd.ComponentID = newd.lstComponentID[0];
        //                newd.SupplierCode = newd.lstSupplierCode[0];
        //                newd.lblComponentID = "";
        //                newd.lblSupplierCode = "";
        //                SupCodeComIDData.Add(newd);
        //                List<SupplierCodeComponentIDEntity> datanew = new List<SupplierCodeComponentIDEntity>();
        //                datanew = SupCodeComIDData.OrderByDescending(x => x.SLNO).ToList();
        //                GridViewComponentSupplier.DataSource = datanew;
        //                GridViewComponentSupplier.DataBind();
        //            }
        //            GridViewComponentSupplier.Rows[0].BackColor =  ColorTranslator.FromHtml("#000000");
        //        }
        //        else
        //        {
        //            btnAdd.Text = "Add";
        //            BindSupplierComponentGrid();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());

        //    }


        //}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //int i = 1;
                bool cansave = false;
                foreach (GridViewRow rows in GridViewComponentSupplier.Rows)
                {
                    //string HidfieldSuppCode = (rows.FindControl("SupplierCodeHiddenfield") as Label).Text;
                    //string HidfieldComponentID = (rows.FindControl("ComponentIDHiddenfield") as Label).Text;
                    //string txtSupplierCode = (rows.FindControl("ddlSupplierCode") as DropDownList).SelectedValue.ToString();
                    //string txtComponentID = (rows.FindControl("ddlComponentID") as DropDownList).SelectedValue.ToString();
                    //string idd = (rows.FindControl("idd") as Label).Text;
                    string component = rows.Cells[0].Text;
                    string supplier =string.Empty;
                    
                    for (int i = 1; i < rows.Cells.Count; i++)
                    {
                        supplier = GridViewComponentSupplier.HeaderRow.Cells[i].Text;
                        CheckBox checkBox = (CheckBox)rows.Cells[i].Controls[0];
                        bool supChecked= checkBox.Checked;
                        cansave = DataBaseAccess.SaveSupplierCodeComponent(component, supplier, supChecked);
                    }
                    //if (!(txtComponentID.Equals(HidfieldComponentID) && txtSupplierCode.Equals(HidfieldSuppCode)))
                    //{
                        
                    //    if (string.IsNullOrEmpty(txtSupplierCode))
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Supplier Code Cannot be Empty')", true);
                    //        return;
                    //    }
                    //    if (string.IsNullOrEmpty(txtComponentID))
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component ID Cannot be Empty')", true);
                    //        return;
                    //    }
                    //    if (DataBaseAccess.Checkdataforinsert(txtSupplierCode,txtComponentID))
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Supplier Code with Component ID already present')", true);
                    //        return;
                    //    }
                    //    cansave = DataBaseAccess.SaveSupplierCodeComponentID(txtSupplierCode, txtComponentID, idd);
                    //    if (cansave)
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code updated successfully')", true);
                    //    }
                    //    else
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code cannot be updated')", true);
                    //    }
                    //}
                    //i++;
                }
                if (cansave)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code updated successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code cannot be updated')", true);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {

                BindSupplierComponentGrid();
            }
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        btnAdd.Text = "Add";
        //        foreach (GridViewRow rows in GridViewComponentSupplier.Rows)
        //        {
        //            string HidfieldSuppCode = (rows.FindControl("deletesupcode") as HiddenField).Value;
        //            if (HidfieldSuppCode.Equals("true", StringComparison.OrdinalIgnoreCase))
        //            {
        //                string HidfieldSuppCodeID = (rows.FindControl("SupplierCodeHiddenfield") as Label).Text;
        //                string HidfieldComponentID = (rows.FindControl("ComponentIDHiddenfield") as Label).Text;
        //                string idd = (rows.FindControl("idd") as Label).Text;
        //                bool delsuccessful = DataBaseAccess.DeleteSupCodeComponentID(HidfieldSuppCodeID, HidfieldComponentID, idd);
        //                if (delsuccessful)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code deleted successfully')", true);
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Component-Supplier Code cannot be deleted')", true);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());

        //    }
        //    finally
        //    {
        //        BindSupplierComponentGrid();

        //    }
        //}

        protected void drpComponentID_Load(object sender, EventArgs e)
        {
            DropDownList drp = sender as DropDownList;
            List<string> lstComponentID = DataBaseAccess.GetComponentID();
        }

        protected void txtSupplierCode_Load(object sender, EventArgs e)
        {
            DropDownList drp = sender as DropDownList;
            List<string> lstSupplierCode = DataBaseAccess.GetSupplierCode();
        }

        protected void GridViewComponentSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                //DropDownList DropDownList1 = (e.Row.FindControl("ddlComponentID") as DropDownList);
                //List<string> ComponentID = DataBaseAccess.GetComponentID();
                //DropDownList1.DataSource = ComponentID;
                //DropDownList1.DataBind();
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.Cells[i].HasControls())
                    {
                        if (e.Row.Cells[i].Controls[0].GetType().Equals(typeof(CheckBox)))
                        {
                            CheckBox checkBox = (CheckBox)e.Row.Cells[i].Controls[0];
                            checkBox.Enabled = true;                          
                        }
                    }
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindSupplierComponentGrid();
        }
    }

    public class SupplierCodeEntity
    {
        public int SLNo { get; set; }
        public string SupCode { get; set; }
        public int idd { get; set; }
    }

    public class SupplierCodeComponentIDEntity
    {
        public int SLNO { get; set; }
        public string SupplierCode { get; set; }
        public string lblSupplierCode { get; set; }
        public string ComponentID { get; set; }
        public string lblComponentID { get; set; }
        public int idd { get; set; }
        public List<string> lstSupplierCode { get;set;}
        public List<string> lstComponentID { get; set; }

    }
}