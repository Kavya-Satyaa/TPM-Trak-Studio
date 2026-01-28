using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Configuration;
//using System.Windows.Forms;
using Web_TPMTrakDashboard.Models;


namespace Web_TPMTrakDashboard
{

    public partial class Cleanup : System.Web.UI.Page
    {
        string conString = WebConfigurationManager.ConnectionStrings[0].ToString();

        public string nameAtIndex = "";
        public string folderPath = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Session["connectionString"]==null)
                {
                    Response.Redirect("SignIn.aspx");
                }
                Session["DeletedTable"] = null;
                rbnDBBackup.Checked = true;
                Legend1.InnerText = rbnDBBackup.Text;
                tblRecordsUpdate.Visible = false;
                BindDB();
            }
        }

        private void BindDB()
        {
            try
            {
                string ConxnName = HttpContext.Current.Session["connectionString"].ToString();
                ConnectionStringSettings ConnectionStringSetting = ConfigurationManager.ConnectionStrings[ConxnName];
                Dictionary<string, string> result = new Dictionary<string, string>();
                string[] pairs = ConnectionStringSetting.ConnectionString.Split(';');

                foreach (string pair in pairs)
                {
                    string[] keyValue = pair.Trim().Split('=');
                    if (keyValue.Length == 2)
                    {
                        result.Add(keyValue[0].Trim(), keyValue[1].Trim());
                    }
                }
                nameAtIndex = result["Initial Catalog"];
                txtDBName.Text = nameAtIndex;

                DataTable dt = DataBaseAccess.BindDB();

                gvDBBackup.DataSource = dt;
                gvDBBackup.DataBind();

            }
            catch(Exception ex) 
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void rbnDBBackup_CheckedChanged(object sender, EventArgs e)
        {
            Legend1.InnerText = rbnDBBackup.Text;
            DBBackup.Visible = true;
            tblRecordsUpdate.Visible = false;
        }

        protected void rbnTblRecordsCleanup_CheckedChanged(object sender, EventArgs e)
        {
            Legend1.InnerText = rbnTblRecordsCleanup.Text;
            tblRecordsUpdate.Visible = true;
            DBBackup.Visible = false;
            PopulateDdl();
            BindRecords();
        }

        protected void rbnFilesCleanup_CheckedChanged(object sender, EventArgs e)
        {
            Legend1.InnerText = "";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bool isUpdated = false;
                isUpdated = DataBaseAccess.AddUpdate("0", txtDBName.Text.Trim(), txtFolderPath.Text.Trim(), "DBBackUpName");
                isUpdated = DataBaseAccess.AddUpdate(txtFreq.Text, txtDBName.Text.Trim(), "", "DBMaintenance");
                if (isUpdated)
                    HelperClassGeneric.openSuccessModal(this, "Saved Successfully.");
                else
                    HelperClassGeneric.openInsertErrorModal(this);
                BindDB();
            }
            catch(Exception ex) { }
        }



        protected void gvDBBackup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int SlNo = e.Row.RowIndex + 1;
                string OpName = DataBinder.Eval(e.Row.DataItem, "Parameter").ToString();
                string DbName = DataBinder.Eval(e.Row.DataItem, "ValueInText").ToString();
                string FilePath = DataBinder.Eval(e.Row.DataItem, "ValueInText2").ToString();
                string NoOfDays = DataBinder.Eval(e.Row.DataItem, "ValueInInt").ToString();

                System.Web.UI.WebControls.Label lblSlNo = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblSlNo");
                lblSlNo.Text = SlNo.ToString();


                e.Row.Cells[1].Text = OpName;
                e.Row.Cells[2].Text = DbName;
                e.Row.Cells[3].Text = FilePath;
                e.Row.Cells[4].Text = NoOfDays;

            }
        }

        protected void btnFolderPicker_Click(object sender, EventArgs e)
        {
            //var dialog = new FolderBrowserDialog();
        //    dialog.ShowDialog();
        //    txtFolderPath.Text = dialog.SelectedPath;
        }

        protected void PopulateDdl()
        {
            try
            {
                DataTable dt = DataBaseAccess.BindRecords();
                ddlRecords.DataSource = dt;
                ddlRecords.DataTextField = "TableName";
                ddlRecords.DataValueField = "TableName";
                ddlRecords.DataBind();


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDays.Text = DataBaseAccess.GetNoOfDays(ddlRecords.SelectedValue);
            BindRecords();
        }

        private void BindRecords()
        {
            DataTable dt = DataBaseAccess.BindRecords();
            
            gvRecords.DataSource = dt;
            gvRecords.DataBind();
        }


       

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string days = txtDays.Text;

            if (DataBaseAccess.UpdateRecords(ddlRecords.SelectedValue, days))
                HelperClassGeneric.openSuccessModal(this, "Successfully Inserted");
            else
                HelperClassGeneric.openInsertErrorModal(this);
            BindRecords();
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int DeleteRowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
                //string delete = (gvRecords.Rows[DeleteRowIndex].FindControl("lblTableName") as Label).Text; 
                Session["DeletedTable"] = (gvRecords.Rows[DeleteRowIndex].FindControl("lblTableName") as Label).Text;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myConfirmationModal", "$('#myConfirmationModal').modal();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void saveConfirmYes_ServerClick(object sender, EventArgs e)
        {

            string tblName = Session["DeletedTable"].ToString();

            string success = DataBaseAccess.DeleteRecords(tblName);
            if(success== "Deleted")
            {
                Session["DeletedTable"] = null;
            }        
            PopulateDdl();
            BindRecords();
        }
    }
}