using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class QualityRejectionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    BindPlant();
                    BindEmploee();
                    BindRejectionCategory();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        private void BindPlant()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllPlantsForPlantInfo();
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();

                BindMachine();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = CumiDBAccess.GetAllMachinedByPlant(ddlPlant.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedValue);
                lbMachine.DataSource = list;
                lbMachine.DataBind();
                if (lbMachine.Items.Count > 0)
                {
                    foreach (ListItem item in lbMachine.Items)
                    {
                        item.Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindMachine: " + ex.Message);
            }
        }
        private void BindEmploee()
        {
            try
            {
                List<ListItem> list = DataBaseAccess.GetAllEmployeesWithName();
                list.RemoveAt(0);
                lbEmployee.DataSource = list;
                lbEmployee.DataTextField = "Text";
                lbEmployee.DataValueField = "Value";
                lbEmployee.DataBind();
                for (int i = 0; i < lbEmployee.Items.Count; i++)
                {
                    lbEmployee.Items[i].Selected = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindEmploee: " + ex.Message);
            }
        }
       
        private void BindRejectionCategory()
        {
            try
            {
                List<string> list = DataBaseAccess.GetRejCatagory("catagory");
                list.RemoveAt(0);
                ddlCategory.DataSource = list;
                ddlCategory.DataBind();
                BindRejectionCode();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindRejectionCategory: " + ex.Message);
            }
        }
        private void BindRejectionCode()
        {
            try
            {
                List<string> list = DataBaseAccess.GetSelectedRejCatagory(ddlCategory.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCategory.SelectedValue);
                list.RemoveAt(0);
                lbRejectionCode.DataSource = list;
                lbRejectionCode.DataBind();
                if (lbRejectionCode.Items.Count > 0)
                {
                    foreach (ListItem item in lbRejectionCode.Items)
                    {
                        item.Selected = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindRejectionCode: " + ex.Message);
            }
        }
        public static string getListBoxValueWithoutSingleQuotes(ListBox listBox)
        {
            string value = "";
            try
            {
                if (listBox != null)
                {
                    foreach (ListItem item in listBox.Items)
                    {
                        if (item.Selected)
                        {
                            if (value == "")
                                value += item.Value;
                            else
                                value += "," + item.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return value;
        }

        private void BindData()
        {
            try
            {
                List<QualityRejectionEntity> list = CumiDBAccess.getQualityReportDetails(Util.GetDateTime(txtFromDate.Text),Util.GetDateTime(txtToDate.Text), ddlPlant.SelectedValue, HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachine), HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbEmployee), ddlCategory.SelectedValue, HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbRejectionCode));
                lvQualityReport.DataSource = list;
                lvQualityReport.DataBind();
                Session["QualityReport"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachine();
        }
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRejectionCode();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["QualityReport"] == null)
                {
                    return;
                }
                string employee = string.Empty;
                foreach (ListItem item in lbEmployee.Items)
                {
                    if (item.Selected)
                    {
                        if (employee == "")
                            employee += item.Text;
                        else
                            employee += "," + item.Text;
                    }
                }
                List<QualityRejectionEntity> list = Session["QualityReport"] as List<QualityRejectionEntity>;
                string machine = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbMachine);
                string rejection = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbRejectionCode);
                if (machine.Split(',').Count().Equals(lbMachine.Items.Count))
                    machine = "All";
                if (employee.Split(',').Count().Equals(lbEmployee.Items.Count))
                    employee = "All";
                if (rejection.Split(',').Count().Equals(lbRejectionCode.Items.Count))
                    rejection = "All";
                CumiGenerateReport.GenerateCumiQualityReport(list, Util.GetDateTime(txtFromDate.Text), Util.GetDateTime(txtToDate.Text), ddlPlant.SelectedValue,machine, employee, ddlCategory.SelectedValue, rejection);
                
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
                int result = 0;
                for (int i = 0; i < lvQualityReport.Items.Count; i++)
                {
                    var row = lvQualityReport.Items[i];
                    if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                    {
                        QualityRejectionEntity data = new QualityRejectionEntity();
                        Guid Docid = Guid.NewGuid();
                        string fileValue = (row.FindControl("hdnDocFileInBase64") as HiddenField).Value;
                        string fileName = (row.FindControl("hdnDocFileName") as HiddenField).Value;
                        string fileIDExist = (row.FindControl("hdnDocFileExist") as HiddenField).Value;
                        string fileNameExist = (row.FindControl("hdnDocFileNameExist") as HiddenField).Value;
                        if (fileValue != "")
                        {
                            fileIDExist = Server.MapPath("~/CumiDocuments/QualityRejectionDocument/" + fileIDExist);
                            if (File.Exists(fileIDExist))
                            {
                                File.Delete(fileIDExist);
                            }
                            var split = fileName.Split('.');
                            string id = split[0] + "_" + Docid + "." + split[1];

                            string file = fileValue;
                            byte[] fileinbytes = System.Convert.FromBase64String(file.Substring(file.LastIndexOf(',') + 1));
                            string filePath = Server.MapPath("~/CumiDocuments/QualityRejectionDocument/" + id);
                            File.WriteAllBytes(filePath, fileinbytes);

                            data.Document = fileName;
                            data.DocumentID = id;
                        }
                        else
                        {
                            data.Document = fileNameExist;
                            data.DocumentID = fileIDExist;
                        }

                        data.MachineId = (row.FindControl("lblMachine") as Label).Text;
                        data.Date = (row.FindControl("lblDate") as Label).Text;
                        data.PONumber = (row.FindControl("lblPONumber") as Label).Text;
                        data.ItemCode = (row.FindControl("lblItemCode") as Label).Text;
                        data.ItemDesc = (row.FindControl("lblItemDesc") as Label).Text;
                        data.Operator = (row.FindControl("lblOperator") as Label).Text;
                        data.RejReason = (row.FindControl("lblRejReason") as Label).Text;
                        data.RejQty = Convert.ToInt32((row.FindControl("lblRejQty") as Label).Text);
                        result += CumiDBAccess.saveQualityReportDetails(data);
                    }
                }
                if (result > 0)
                {
                    Bajaj.Model.HelperClass.openInsertSuccessModal(this);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}