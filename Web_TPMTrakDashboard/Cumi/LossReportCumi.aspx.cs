using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class LossReportCumi : System.Web.UI.Page
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
                    BindDownCategory();
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
        private void BindDownCategory()
        {
            try
            {
                List<string> list = DownCodeInfoDataBase.GetDownCategoryInformation();
                if (list.Count > 0)
                {
                    list.Insert(0, "All");
                }
                ddlCategory.DataSource = list;
                ddlCategory.DataBind();
                BindDownCode();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindPlant: " + ex.Message);
            }
        }
        private void BindDownCode()
        {
            try
            {
                List<string> list = CumiDBAccess.GetDownCodeForCategory(ddlCategory.SelectedValue.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlCategory.SelectedValue);
                lbDownCode.DataSource = list;
                lbDownCode.DataBind();
                if (lbDownCode.Items.Count > 0)
                {
                    foreach (ListItem item in lbDownCode.Items)
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
        private void BindData()
        {
            try
            {
                lblMTTR.Text = "";
                lblMTBF.Text = "";
                string mttr = "", mtbf = "";
                List<LossReportEntity> list = CumiDBAccess.getLossReportDetails(txtFromDate.Text, txtToDate.Text, ddlPlant.SelectedValue, HelperClassGeneric.getListBoxValueWithCommaSeparator(lbMachine), ddlCategory.SelectedValue, HelperClassGeneric.getListBoxValueWithCommaSeparator(lbDownCode), out mttr, out mtbf);
                lblMTTR.Text = mttr;
                lblMTBF.Text = mtbf;
                lvLossReport.DataSource = list;
                lvLossReport.DataBind();
                Session["LossReportData"] = list;
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
            BindDownCode();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["LossReportData"] == null)
                {
                    return;
                }
                List<LossReportEntity> list = Session["LossReportData"] as List<LossReportEntity>;
                CumiGenerateReport.generateLossReport(txtFromDate.Text, txtToDate.Text, ddlPlant.SelectedValue, ddlCategory.SelectedValue, list, lblMTTR.Text, lblMTBF.Text);
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
                for (int i = 0; i < lvLossReport.Items.Count; i++)
                {
                    var row = lvLossReport.Items[i];
                    if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("Update", StringComparison.OrdinalIgnoreCase))
                    {
                        LossReportEntity data = new LossReportEntity();
                        Guid whywhyid = Guid.NewGuid();
                        Guid otherdocid = Guid.NewGuid();
                        string fileValue = (row.FindControl("hdnWhyWhyFileInBase64") as HiddenField).Value;
                        string fileName = (row.FindControl("hdnWhyWhyFileName") as HiddenField).Value;
                        string fileIDExist = (row.FindControl("hdnWhyWhyFileExist") as HiddenField).Value;
                        string fileNameExist = (row.FindControl("hdnWhyWhyFileNameExist") as HiddenField).Value;
                        if (fileValue != "")
                        {
                            fileIDExist = Server.MapPath("~/CumiDocuments/WhyWhyDocuments/" + fileIDExist);
                            if (File.Exists(fileIDExist))
                            {
                                File.Delete(fileIDExist);
                            }
                            var split = fileName.Split('.');
                            string id = split[0] + "_" + whywhyid + "." + split[1];


                            string file = fileValue;
                            byte[] fileinbytes = System.Convert.FromBase64String(file.Substring(file.LastIndexOf(',') + 1));
                            string filePath = Server.MapPath("~/CumiDocuments/WhyWhyDocuments/" + id);
                            File.WriteAllBytes(filePath, fileinbytes);


                            data.WhyWhyDocFileName = fileName;
                            data.WhyWhyDocID = id;
                        }
                        else
                        {
                            data.WhyWhyDocFileName = fileNameExist;
                            data.WhyWhyDocID = fileIDExist;
                        }



                        fileValue = (row.FindControl("hdnOtherDocFileInBase64") as HiddenField).Value;
                        fileName = (row.FindControl("hdnOtherDocFileName") as HiddenField).Value;
                        fileIDExist = (row.FindControl("hdnOtherDocFileExist") as HiddenField).Value;
                        fileNameExist = (row.FindControl("hdnOtherDocFileNameExist") as HiddenField).Value;
                        if (fileValue != "")
                        {
                            fileIDExist = Server.MapPath("~/CumiDocuments/OtherDocuments/" + fileIDExist);
                            if (File.Exists(fileIDExist))
                            {
                                File.Delete(fileIDExist);
                            }
                            var split = fileName.Split('.');
                            string id = split[0] + "_" + otherdocid + "." + split[1];


                            string file = fileValue;
                            byte[] fileinbytes = System.Convert.FromBase64String(file.Substring(file.LastIndexOf(',') + 1));
                            string filePath = Server.MapPath("~/CumiDocuments/OtherDocuments/" + id);
                            File.WriteAllBytes(filePath, fileinbytes);


                            data.OtherDocFileName = fileName;
                            data.OtherDocID = id;
                        }
                        else
                        {
                            data.OtherDocFileName = fileNameExist;
                            data.OtherDocID = fileIDExist;
                        }

                        data.Machine = (row.FindControl("lblMachine") as Label).Text;
                        data.MachineInterfaceID = (row.FindControl("hdnMachineInterfaceID") as HiddenField).Value;
                        data.StartTime = (row.FindControl("lblStartTime") as Label).Text;
                        data.StartDate = (row.FindControl("lblStartDate") as Label).Text;
                        data.LossID = (row.FindControl("lblLossID") as Label).Text;
                        data.MntcEmpID = (row.FindControl("lblMntcEmpIDWithName") as Label).Text;
                        data.Remarks = (row.FindControl("txtRemarks") as TextBox).Text;
                        result += CumiDBAccess.saveLossReportDetails(data);
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