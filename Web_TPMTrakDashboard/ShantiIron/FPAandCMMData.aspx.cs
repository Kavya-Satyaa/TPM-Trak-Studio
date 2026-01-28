using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class FPAandCMMData : System.Web.UI.Page
    {
        public string appPath = HttpContext.Current.Server.MapPath("~/ShantiIron/FPA_FIles");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComponentID();
                BindFPAData();
            }
        }

        private void BindComponentID()
        {
            try
            {
                DataTable CmpList = DataBaseAccess.GetAllComponentz();
                if (CmpList != null && CmpList.Rows.Count > 0)
                {
                    drpComponentID.DataSource = CmpList.AsEnumerable().Select(x => x.Field<string>("ComponentId")).ToList();
                    drpComponentID.DataBind();
                }
                else
                {
                    drpComponentID.DataSource = new List<string>();
                    drpComponentID.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindFPAData()
        {
            FPAData list = new FPAData();
            try
            {
                switch(chktype.Checked)
                {
                    case true:
                        drpComponentID.Enabled = false;
                        list = DataBaseAccess.FPAGetSerialNumber();
                        drpComponentID.SelectedValue = list.PartName;
                        break;
                    case false:
                        drpComponentID.Enabled = true;
                        if (drpComponentID.SelectedValue != null)
                        {
                            list = DataBaseAccess.FPAGetSerialNumber(drpComponentID.SelectedValue.ToString(), txtSlnoSearch.Text);
                        }
                        break;
                }
                if (list != null && !string.IsNullOrEmpty(list.SerialNumber))
                {
                    txtSlnoSearch.Text = list.SerialNumber;
                    LinkFPA.Text = list.FPAName;
                    LinkLayout.Text = list.LayoutName;
                    btnCMM.Visible = string.IsNullOrEmpty(list.CMMStatus) ? true : false;
                    btnCMM.Enabled = string.IsNullOrEmpty(list.CMMStatus) ? true : false;
                    lblCMMStatus.Text = "CMM Sent";
                    linkCMM.Text = list.CMMName;
                    switch (list.SerialStatus)
                    {
                        case "New":
                            lblStatus.Text = "New";
                            btnApprove.Enabled = true;
                            btnReject.Enabled = true;
                            break;
                        case "":
                            btnApprove.Enabled = true;
                            btnReject.Enabled = true;
                            break;
                        default:
                            btnApprove.Enabled = false;
                            btnReject.Enabled = false;
                            lblStatus.Text = list.SerialStatus;
                            break;
                    }
                }
                else
                {
                    linkCMM.Text = "";
                    LinkFPA.Text = "";
                    LinkLayout.Text = "";
                    lblStatus.Text = "No Data Found";
                    btnApprove.Enabled = false;
                    btnCMM.Enabled = false;
                    btnReject.Enabled = false;
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void gvFPAData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    (e.Row.FindControl("fuFPAFile") as FileUpload).Attributes["onchange"] = "UploadFPAFile(this)";
                    (e.Row.FindControl("fuLayoutFile") as FileUpload).Attributes["onchange"] = "UploadLayoutFile(this)";
                }
            }
            catch (Exception ex)
            { }
        }

        protected void saveFPAFile_Click(object sender, EventArgs e)
        {
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            FileUpload fileUpload = row.FindControl("fuFPAFile") as FileUpload;
            foreach (HttpPostedFile postedFile in fileUpload.PostedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                //string currentDateTime = System.DateTime.Now.ToString("ddMMyyhhmmssfffff");
                // string file = currentDateTime + "_" + fileName;
                // string filepath = "~/UploadSignalProcess/" + file;
                // postedFile.SaveAs(Server.MapPath("~/UploadSignalProcess/") + file);
            }
        }

        protected void saveLayoutFile_Click(object sender, EventArgs e)
        {
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            FileUpload fileUpload = row.FindControl("fuLayoutFile") as FileUpload;
            foreach (HttpPostedFile postedFile in fileUpload.PostedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                //string currentDateTime = System.DateTime.Now.ToString("ddMMyyhhmmssfffff");
                // string file = currentDateTime + "_" + fileName;
                // string filepath = "~/UploadSignalProcess/" + file;
                // postedFile.SaveAs(Server.MapPath("~/UploadSignalProcess/") + file);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseAccess.SaveFPAData(txtSlnoSearch.Text, drpComponentID.SelectedValue.ToString(), "Rejected");
                BindFPAData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }

        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(LinkFPA.Text.Equals("FPA_Template") && LinkLayout.Text.Equals("Layout_Template")))
                {
                    DataBaseAccess.SaveFPAData(txtSlnoSearch.Text, drpComponentID.SelectedValue.ToString(), "Approved");
                    BindFPAData();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnCMM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!LinkFPA.Text.Equals("FPA_Template") || !LinkLayout.Text.Equals("Layout_Template"))
                {
                    DataBaseAccess.SaveFPAData(txtSlnoSearch.Text, drpComponentID.SelectedValue.ToString(), "CMMSent");
                    BindFPAData();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void linkCMM_Click(object sender, EventArgs e)
        {
            try
            {
                switch ((sender as LinkButton).Text)
                {
                    case "":
                        linkCMM.Text = "";
                        break;
                    default:
                        string Source = Util.GetReportPath(Path.Combine(appPath, "CMM",  (sender as LinkButton).Text));
                        appPath = HttpContext.Current.Server.MapPath("~/Reports");
                        string destination = Path.Combine(appPath, "Temp", SafeFileName((sender as LinkButton).Text));
                        FileInfo newFile = new FileInfo(Source);
                        ExcelPackage Excel = new ExcelPackage(newFile, true);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void LinkLayout_Click(object sender, EventArgs e)
        {

            try
            {
                switch ((sender as LinkButton).Text)
                {
                    case "":
                        break;
                    case "Template.xlsx":
                        string Source = Util.GetReportPath(Path.Combine(appPath, "Layout", "Template", "Template.xlsx"));
                        appPath = HttpContext.Current.Server.MapPath("~/Reports");
                        string destination = Path.Combine(appPath, "Temp", SafeFileName(txtSlnoSearch.Text + "_" + drpComponentID.SelectedValue.ToString() + "_Layout.xlsx"));
                        FileInfo newFile = new FileInfo(Source);
                        ExcelPackage Excel = new ExcelPackage(newFile, true);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        break;
                    default:
                        Source = Util.GetReportPath(Path.Combine(appPath, "Layout", "UploadedTemplate", (sender as LinkButton).Text));
                        appPath = HttpContext.Current.Server.MapPath("~/Reports");
                        destination = Path.Combine(appPath, "Temp", SafeFileName((sender as LinkButton).Text));
                        newFile = new FileInfo(Source);
                        Excel = new ExcelPackage(newFile, true);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                         break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private string SafeFileName(object layoutTemplate)
        {
            throw new NotImplementedException();
        }

        protected void LinkFPA_Click(object sender, EventArgs e)
        {
            try
            {
                switch ((sender as LinkButton).Text)
                {
                    case "":
                        break;
                    case "Template.xlsx":
                        string Source = Util.GetReportPath(Path.Combine(appPath, "FPA", "Template", "Template.xlsx"));
                        appPath = HttpContext.Current.Server.MapPath("~/Reports");
                        string destination = Path.Combine(appPath, "Temp", SafeFileName(txtSlnoSearch.Text + "_" + drpComponentID.SelectedValue.ToString() + "_FPA.xlsx"));
                        FileInfo newFile = new FileInfo(Source);
                        ExcelPackage Excel = new ExcelPackage(newFile, true);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        break;
                    default:
                        Source = Util.GetReportPath(Path.Combine(appPath, "FPA", "UploadedTemplate", (sender as LinkButton).Text) );
                        appPath = HttpContext.Current.Server.MapPath("~/Reports");
                        destination = Path.Combine(appPath, "Temp", SafeFileName((sender as LinkButton).Text));
                        newFile = new FileInfo(Source);
                        Excel = new ExcelPackage(newFile, true);
                        DownloadMultipleFile(destination, Excel.GetAsByteArray());
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        public static string SafeFileName(string name)
        {
            StringBuilder str = new StringBuilder(name);
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                str = str.Replace(c, '_');
            }
            return str.ToString();
        }

        private static void DownloadMultipleFile(string fileName, byte[] byteArray)
        {
            try
            {

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(fileName) + "\"");
                HttpContext.Current.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog("GENERATED ERROR : \n" + "Report generation Failed Error: " + ex.ToString());
            }
        }

        protected void btnFPAImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileuploadFPAfile.HasFiles)
                {
                    if (Path.GetExtension(fileuploadFPAfile.FileName).Equals(".xlsx"))
                    {
                        fileuploadFPAfile.PostedFile.SaveAs(Path.Combine(appPath, "FPA", "UploadedTemplate", fileuploadFPAfile.FileName));
                        DataBaseAccess.SaveFPAChanges(drpComponentID.Text, txtSlnoSearch.Text, fileuploadFPAfile.FileName, (LinkLayout.Text.Equals("Template.xlsx", StringComparison.OrdinalIgnoreCase) ? "" : LinkLayout.Text), string.IsNullOrEmpty(linkCMM.Text) ? "" : linkCMM.Text, "", "");
                        BindFPAData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnImportLayout_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileuploadLayoutfile.HasFiles)
                {
                    if (Path.GetExtension(fileuploadLayoutfile.FileName).Equals(".xlsx"))
                    {
                        fileuploadLayoutfile.PostedFile.SaveAs(Path.Combine(appPath, "Layout", "UploadedTemplate", fileuploadLayoutfile.FileName));
                        DataBaseAccess.SaveFPAChanges(drpComponentID.Text, txtSlnoSearch.Text, (LinkFPA.Text.Equals("Template.xlsx", StringComparison.OrdinalIgnoreCase) ? "" : LinkFPA.Text), fileuploadLayoutfile.FileName, string.IsNullOrEmpty(linkCMM.Text) ? "" : linkCMM.Text, "", "");
                        BindFPAData();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindFPAData();
        }

        protected void chktype_CheckedChanged(object sender, EventArgs e)
        {
            BindFPAData();
        }
    }
}