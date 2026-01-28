using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class AddConsumer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!(IsPostBack))
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                    SessionClear.ClearSession();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            List<AddConsumerEntity> List = new List<AddConsumerEntity>();
            try
            {
                List = DataBaseAccess.GetConsumerInfo();
                if (List != null && List.Count > 0)
                {
                    gridAddCustomer.DataSource = List;
                    gridAddCustomer.DataBind();
                    bindEmailPhone((gridAddCustomer.FooterRow.FindControl("cmbFooterUserID") as DropDownList).SelectedValue==null?"": (gridAddCustomer.FooterRow.FindControl("cmbFooterUserID") as DropDownList).SelectedValue.ToString());
                }
                else
                {
                    List.Add(new AddConsumerEntity { });
                    gridAddCustomer.DataSource = List;
                    gridAddCustomer.DataBind();
                    gridAddCustomer.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gridAddCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType == DataControlRowType.Footer)
                {
                    List<string> List = DataBaseAccess.GetPlantDetails();
                    DropDownList cmb = e.Row.FindControl("cmbFooterPlantID") as DropDownList;
                    if(cmb!=null)
                    {
                        cmb.DataSource = List;
                        cmb.DataBind();
                        cmb.SelectedIndex = 0;
                    }
                    List = DataBaseAccess.GetEmployeeDetails(cmb.SelectedValue.ToString());
                    cmb = e.Row.FindControl("cmbFooterUserID") as DropDownList;
                    if (cmb != null)
                    {
                        cmb.DataSource = List;
                        cmb.DataBind();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void gridAddCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string PlantID = string.Empty, UserID = string.Empty, PhNO = string.Empty, Email = string.Empty, EmailValidation=string.Empty, chatID=string.Empty;
            int RowIndex = 0;bool IsUpdated = false;
            try
            {
                switch(e.CommandName)
                {
                    case ("AddRow"):
                        PlantID = (gridAddCustomer.FooterRow.FindControl("cmbFooterPlantID") as DropDownList).SelectedValue.ToString();
                        UserID = (gridAddCustomer.FooterRow.FindControl("cmbFooterUserID") as DropDownList).SelectedValue.ToString();
                        PhNO = (gridAddCustomer.FooterRow.FindControl("txtFooterPhoneNo") as TextBox).Text.ToString();
                        Email = (gridAddCustomer.FooterRow.FindControl("txtFooterEmail") as TextBox).Text.ToString();
                        EmailValidation = (gridAddCustomer.FooterRow.FindControl("hfFooterEmailValidation") as HiddenField).Value.ToString();
                        chatID = (gridAddCustomer.FooterRow.FindControl("txtFooterChatID") as TextBox).Text.ToString();
                        if (string.IsNullOrEmpty(PhNO) || string.IsNullOrEmpty(Email))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide Phone number and Email ID')", true);
                            return;
                        }
                        if (EmailValidation=="1")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please eneter valid Email ID')", true);
                            return;
                        }
                        DataBaseAccess.UpdateConsumerInfo(PlantID, UserID, Email, PhNO, chatID, out IsUpdated);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Added Row Successfully')", true);
                        BindGrid();
                        break;
                    case "CancelRow":
                        gridAddCustomer.EditIndex = -1;
                        
                        BindGrid();
                        break;
                    case ("EditRow"):
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        gridAddCustomer.EditIndex = RowIndex;
                        BindGrid();
                        break;
                    case ("UpdateRow"):
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        PlantID = (gridAddCustomer.Rows[RowIndex].FindControl("lblPlantID") as Label).Text;
                        UserID = (gridAddCustomer.Rows[RowIndex].FindControl("lblUserID") as Label).Text;
                        PhNO = (gridAddCustomer.Rows[RowIndex].FindControl("txtPhoneNo") as TextBox).Text;
                        Email = (gridAddCustomer.Rows[RowIndex].FindControl("txtEmail") as TextBox).Text;
                        EmailValidation = (gridAddCustomer.Rows[RowIndex].FindControl("hfEmailValidation") as HiddenField).Value.ToString();
                        chatID = (gridAddCustomer.Rows[RowIndex].FindControl("txtChatID") as TextBox).Text;
                        if (string.IsNullOrEmpty(PhNO)|| string.IsNullOrEmpty(Email))
                        if(string.IsNullOrEmpty(PhNO)|| string.IsNullOrEmpty(Email))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide Phone number and Email ID')", true);
                            return;
                        }
                        if (EmailValidation == "1")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please eneter valid Email ID')", true);
                            return;
                        }
                        DataBaseAccess.UpdateConsumerInfo(PlantID,  UserID,  Email,  PhNO,chatID, out IsUpdated);
                        gridAddCustomer.EditIndex = -1;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Updated Row Successfully')", true);
                        BindGrid();
                        break;
                    case ("DeleteRow"):
                        RowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                        PlantID = (gridAddCustomer.Rows[RowIndex].FindControl("lblPlantID") as Label).Text;
                        UserID = (gridAddCustomer.Rows[RowIndex].FindControl("lblUserID") as Label).Text;
                        PhNO = (gridAddCustomer.Rows[RowIndex].FindControl("lblPhoneNo") as Label).Text;
                        Email = (gridAddCustomer.Rows[RowIndex].FindControl("lblEmail") as Label).Text;
                        DataBaseAccess.deleteConsumerInfo(UserID, out IsUpdated);
                        gridAddCustomer.EditIndex = -1;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Deleted Successfully')", true);
                        BindGrid();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void cmbFooterUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bindEmailPhone((gridAddCustomer.FooterRow.FindControl("cmbFooterUserID") as DropDownList).SelectedValue==null?"" : (gridAddCustomer.FooterRow.FindControl("cmbFooterUserID") as DropDownList).SelectedValue.ToString());

                //int index = Convert.ToInt32(((sender as DropDownList).NamingContainer as GridViewRow).RowIndex);
                //DropDownList cmb = sender as DropDownList;


            }
            catch(Exception ex)
            {

            }
        }

        private void bindEmailPhone(string userID)
        {
            try
            {
                string phone;
                string email = DataBaseAccess.GetEmployeeDetailsForEmpID(userID, out phone);
                (gridAddCustomer.FooterRow.FindControl("txtFooterPhoneNo") as TextBox).Text = phone;
                (gridAddCustomer.FooterRow.FindControl("txtFooterEmail") as TextBox).Text = email;
            }
            catch(Exception ex)
            {

            }
        }
        
        protected void cmbFooterPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList cmb = sender as DropDownList;
            if (cmb.SelectedValue != null)
            {
                List<string> List = DataBaseAccess.GetEmployeeDetails(cmb.SelectedValue);
                cmb = gridAddCustomer.FooterRow.FindControl("cmbFooterUserID") as DropDownList;
                if (cmb != null)
                {
                    cmb.DataSource = List;
                    cmb.DataBind();
                }
            }
            cmbFooterUserID_SelectedIndexChanged(null, null);
        }
    }

    public class AddConsumerEntity
    {
        public int SLNO { get; set; }
        public string PlantID { get; set; }
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ChatID { get; set; }
    }
}