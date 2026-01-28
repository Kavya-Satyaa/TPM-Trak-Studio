using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class CustomerInformation : System.Web.UI.Page
    {
        public DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustomerGrdData();
            }
        }

        private void CustomerGrdData()
        {
            try
            {
                dt = DataBaseAccess.AddDataForCustomerInformstion();
                if(dt.Rows.Count>0)
                {
                    grdcustomer.DataSource = dt;
                    grdcustomer.DataBind();
                }
                else
                {
                    grdcustomer.DataSource = this.GetEmptyDataTable();
                    grdcustomer.DataBind();
                    grdcustomer.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("" + ex.Message);
                throw;
            }
        }

        public DataTable GetEmptyDataTable()
        {
            DataTable dtEmpty = new DataTable();
            dtEmpty.Columns.Add("customerid", typeof(string));
            dtEmpty.Columns.Add("customername", typeof(string));
            dtEmpty.Columns.Add("address1", typeof(string));
            dtEmpty.Columns.Add("place", typeof(string));
            dtEmpty.Columns.Add("state", typeof(string));
            dtEmpty.Columns.Add("country", typeof(string));
            dtEmpty.Columns.Add("pin", typeof(string));
            dtEmpty.Columns.Add("phone", typeof(string));
            dtEmpty.Columns.Add("email", typeof(string));
            dtEmpty.Columns.Add("contactperson", typeof(string));
            dtEmpty.Columns.Add("address2", typeof(string));
            DataRow datatRow = dtEmpty.NewRow();
            dtEmpty.Rows.Add(datatRow);
            return dtEmpty;
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            string sucessfailure = string.Empty;
            string checksave = hdfgrdcustomer.Value;
            try
            {
                if (checksave.Equals("Save", StringComparison.OrdinalIgnoreCase))
                {
                    string cusid = (grdcustomer.FooterRow.FindControl("hdfcusid") as TextBox).Text;
                    string cusname = (grdcustomer.FooterRow.FindControl("hdfcustomername") as TextBox).Text;
                    string cusaddress = (grdcustomer.FooterRow.FindControl("hdfaddress") as TextBox).Text;
                    string cusplace = (grdcustomer.FooterRow.FindControl("hdfplace") as TextBox).Text;
                    string state = (grdcustomer.FooterRow.FindControl("hdfstate") as TextBox).Text;
                    string country = (grdcustomer.FooterRow.FindControl("hdfcountry") as TextBox).Text;
                    string pin = (grdcustomer.FooterRow.FindControl("hdfpin") as TextBox).Text;
                    string phone = (grdcustomer.FooterRow.FindControl("hdfphone") as TextBox).Text;
                    string email = (grdcustomer.FooterRow.FindControl("hdfemail") as TextBox).Text;
                    string contact = (grdcustomer.FooterRow.FindControl("hdfcontact") as TextBox).Text;
                    hdfgrdcustomer.Value = "";
                    if (!DataBaseAccess.CheckCustomerId(cusid))
                    {
                        DataBaseAccess.insertUpdateCustomerInfo(cusid, cusname, cusaddress, cusplace, state, country, pin, phone, email, contact, out sucessfailure);
                    }
                }

                foreach (GridViewRow row in grdcustomer.Rows)
                {
                    HiddenField hdfvalue = (HiddenField)row.FindControl("hdfupdate");
                    if (hdfvalue.Value.Equals("updated", StringComparison.OrdinalIgnoreCase))
                    {
                        string cusid = ((Label)row.FindControl("grdtxtcusid")).Text;
                        string cusname = ((TextBox)row.FindControl("grdtxtcustomername")).Text;
                        string cusaddress = ((TextBox)row.FindControl("grdtxtaddress")).Text;
                        string cusplace = ((TextBox)row.FindControl("grdtxtplace")).Text;
                        string cusstate = ((TextBox)row.FindControl("grdtxtstate")).Text;
                        string cuscountry = ((TextBox)row.FindControl("grdtxtcountry")).Text;
                        string cuspin = ((TextBox)row.FindControl("grdtxtpin")).Text;
                        string cusphone = ((TextBox)row.FindControl("grdtxtphone")).Text;
                        string cusemail = ((TextBox)row.FindControl("grdtxtemail")).Text;
                        string cuscontact = ((TextBox)row.FindControl("grdtxtcontact")).Text;
                        DataBaseAccess.UpdateCustomerInfo(cusid, cusname, cusaddress, cusplace, cusstate, cuscountry, cuspin, cusphone, cusemail, cuscontact, out sucessfailure);
                    }
                }
                if (sucessfailure == "Successfull")
                {
                    CustomerGrdData();
                    lblMessages.Text = GetLocalResourceObject("CustInfUpdt").ToString();
                    lblMessages.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Customer page" + ex.Message);
            }
        }

        protected void btndeltecust_Click(object sender, EventArgs e)
        {
            string sucessfailure = string.Empty;
            int Index = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
            String customerid = ((Label)grdcustomer.Rows[Index].FindControl("grdtxtcusid")).Text;
            try
            {
                DataBaseAccess.DeleteCustomerInformation(customerid, out sucessfailure);
                if (sucessfailure.Equals("Successfull"))
                {
                    lblMessages.Text = GetLocalResourceObject("DelData").ToString();
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    CustomerGrdData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("delete on Customer" + ex.Message);
            }
        }

        protected void txtreason_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = ConnectionManager.GetConnection();
            SqlDataAdapter adapt;
            adapt = new SqlDataAdapter("select * from customerinformation where customerid like '" + txtreason.Text + "%'", con);
            dt = new DataTable();
            adapt.Fill(dt);
            grdcustomer.DataSource = dt;
            grdcustomer.DataBind();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string success = TMPTrakGenerateReport.GenerateCustomerInfoReport();
                try
                {
                    if (success.Equals("Template Not Found", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningModal(this, "Template not found");
                    }
                    else if (success.Equals("Data Not Found", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningToastrModal(this, "Data not found");
                    }
                    else
                    {
                        HelperClassGeneric.openErrorModal(this, "Error generating report. Try Again.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}