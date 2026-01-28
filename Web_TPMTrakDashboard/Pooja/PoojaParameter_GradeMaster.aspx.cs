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
using Web_TPMTrakDashboard.Pooja.Model;


namespace Web_TPMTrakDashboard.Pooja
{
    public partial class PoojaParameter_GradeMaster : System.Web.UI.Page
    {
        private static int deleteRowIndex = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvPoojaMaster.ShowFooter = false;
                BindGradeIDs();
                BindGradeIDGrid(); 
                BindPoojaAndonMasterData();
                btnCancel.Visible = false;
            }
        }

        private void BindGradeIDGrid()
        {
            List<GradeMasterEntity> list = new List<GradeMasterEntity>();
            try
            {
                list = PoojaDBAccess.getGradeIDDetail();

                gvGradeID.DataSource = list;
                gvGradeID.DataBind();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindParameters()
        {
            try
            {
                List<string> list = PoojaDBAccess.GetParameters();
                DropDownList ddlParam = gvPoojaMaster.FooterRow.FindControl("ddlParameter") as DropDownList;
                ddlParam.DataSource = list;
                ddlParam.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindGradeIDs()
        {
            try
            {
                List<string> list = PoojaDBAccess.GetGradeIDs();
                
                ddlGradeIDFilter.DataSource = list;
                ddlGradeIDFilter.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPoojaAndonMasterData()
        {
            try
            {
                List<PoojaAndonMasterDetails> list = PoojaDBAccess.GetPoojaAndonMasteretails(ddlGradeIDFilter.SelectedValue.Trim());
                if (list.Count > 0)
                {
                    gvPoojaMaster.DataSource = list;
                    gvPoojaMaster.DataBind();
                }
                else
                {
                    list.Add(new PoojaAndonMasterDetails { Parameter = "", GradeID = "" });
                    gvPoojaMaster.DataSource = list;
                    gvPoojaMaster.DataBind();
                    gvPoojaMaster.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            bool IsSaved = false;
            try
            {

                if (gvPoojaMaster.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvPoojaMaster.Rows)
                    {
                        string hdfUpdate = ((HiddenField)row.FindControl("hdfUpdate")).Value;
                        PoojaAndonMasterDetails data = new PoojaAndonMasterDetails();
                        if (hdfUpdate == "Update")
                        {
                            data.Parameter = (row.Cells[1].FindControl("lblParameter") as Label).Text;
                            if (data.Parameter == "")
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('Parameter is required')", true);
                                HelperClassGeneric.openWarningModal(this, "Parameter is required");
                                return;
                            }
                            data.GradeID = (row.Cells[2].FindControl("lblGradeID") as Label).Text;
                            if (data.GradeID == "")
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('GradeID is required')", true);
                                HelperClassGeneric.openWarningModal(this, "GradeID is required");
                                return;
                            }
                            data.CompareType = (row.Cells[3].FindControl("ddlCompareType") as DropDownList).SelectedValue;
                            data.LSL = (row.Cells[4].FindControl("txtLSL") as TextBox).Text;
                            data.USL = (row.Cells[5].FindControl("txtUSL") as TextBox).Text;
                            if (data.CompareType != "Target")
                            {
                                if (data.USL == "")
                                {
                                    HelperClassGeneric.openWarningModal(this, data.Parameter + "- " + "USL can not be empty");
                                    break;
                                }
                                if (data.USL != null && data.USL != "")
                                {
                                    if (Convert.ToDouble(data.LSL) > Convert.ToDouble(data.USL))
                                    {
                                        HelperClassGeneric.openWarningModal(this, data.Parameter + "- " + "LSL can not be greater than USL");
                                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('LSL can not be greater than USL')", true);
                                        BindPoojaAndonMasterData();
                                        return;
                                    }
                                }
                            }

                            IsSaved = PoojaDBAccess.UpdatePoojaAndonMaster(data);
                            if (!IsSaved) break;

                        }
                    }
                    gvPoojaMaster.ShowFooter = false;
                    BindPoojaAndonMasterData();
                    btnCancel.Visible = false;
                    btnNew.Visible = true;
                    if (IsSaved)
                    {
                        HelperClassGeneric.openSuccessModal(this, "Data Saved Successfully.");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No data to save.')", true);
                }
            }
            catch (Exception ex)
            {
                gvPoojaMaster.ShowFooter = true;
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                int success = 0;
                PoojaAndonMasterDetails list = new PoojaAndonMasterDetails();
                if (fileUploader.HasFile)
                {
                    string fileName = fileUploader.FileName;
                    if (!Path.GetExtension(fileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) && !Path.GetExtension(fileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        HelperClassGeneric.openWarningModal(this, "Please choose the valid .xlsx or .xls file");
                        return;
                    }
                    else
                    {
                        if (!Directory.Exists(Server.MapPath("ImportedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                        }
                        string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                        fileUploader.SaveAs(savedFileName);
                        string Errormsg = GetPoojaAndonMasterListFromFile(savedFileName, out list);
                    }
                }
                else
                {
                    HelperClassGeneric.openWarningModal(this, "Please choose a file to import");
                }
                BindPoojaAndonMasterData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private string GetPoojaAndonMasterListFromFile(string fileName, out PoojaAndonMasterDetails list)
        {
            string Errormsg = "";
            bool IsSaved = false;

            list = null;
            try
            {
                using (var pck = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(fileName))
                    {
                        pck.Load(stream);
                    }
                    var workbook = pck.Workbook;
                    if (workbook != null)
                    {
                        try
                        {
                            var workSheet = workbook.Worksheets[1];

                            int rowCount = 2;
                            while (true)
                            {
                                if (workSheet.Cells[rowCount, 1].Value == null)
                                {
                                    break;
                                }
                                list = new PoojaAndonMasterDetails();
                                list.Parameter = workSheet.Cells[rowCount, 1].Value.ToString();
                                list.GradeID = workSheet.Cells[rowCount, 2].Value.ToString();
                                //list.CompareType = workSheet.Cells[rowCount, 3].Value.ToString();
                                list.LSL = workSheet.Cells[rowCount, 3].Value?.ToString() ?? "";
                                var Values = workSheet.Cells[rowCount, 4]?.Value?.ToString() ?? "";
                                if (Values != "")
                                {
                                    list.USL = Values;
                                }

                                if (Values != "")
                                {
                                    list.CompareType = "Range";
                                }
                                else
                                {
                                    list.CompareType = "Target";
                                }
                                if (list.LSL != "")
                                {
                                    IsSaved = PoojaDBAccess.UpdatePoojaAndonMaster(list);
                                    if (!IsSaved) break;
                                }
                                rowCount++;

                            }
                            HelperClassGeneric.openSuccessModal(this, "Data Inserted Successfully.");
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteErrorLog(ex);
                            return "Failed to Import Data.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                return "Failed to Import Data.";
            }
            return Errormsg;
        }

        protected void lbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int index = ((sender as LinkButton).NamingContainer as GridViewRow).DataItemIndex;
                ViewState["DeleteRowIndex"] = index;
                deleteRowIndex = index;
                int DeleteRowIndex = deleteRowIndex;
                PoojaAndonMasterDetails data = new PoojaAndonMasterDetails();
                data.Parameter = (gvPoojaMaster.Rows[DeleteRowIndex].FindControl("lblParameter") as Label).Text;
                data.GradeID = (gvPoojaMaster.Rows[DeleteRowIndex].FindControl("lblGradeID") as Label).Text;
                string result = PoojaDBAccess.DeletePoojaMasterData(data);
                if (result == "Deleted")
                {
                    HelperClassGeneric.openSuccessModal(this, "Data Deleted Successfully.");
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, "Deletion failed");
                    return;
                }
                BindPoojaAndonMasterData();
                ViewState["DeleteRowIndex"] = -1;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void lbAdd_Click(object sender, EventArgs e)
        {
            try
            {
                PoojaAndonMasterDetails data = new PoojaAndonMasterDetails();
                data.Parameter = (gvPoojaMaster.FooterRow.FindControl("ddlParameter") as DropDownList).SelectedValue;
                if (data.Parameter == "")
                {
                    HelperClassGeneric.openWarningModal(this, "Parameter is required");
                    return;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('Parameter is required')", true);
                }
                data.GradeID = (gvPoojaMaster.FooterRow.FindControl("txtGradeID") as TextBox).Text.Trim();
                //if (data.GradeID == "")
                //{
                //    HelperClassGeneric.openWarningModal(this, "GradeID is required");
                //    return;
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('GradeID is required')", true);
                //}
                data.CompareType = (gvPoojaMaster.FooterRow.FindControl("ddlCompareType") as DropDownList).SelectedValue;
                data.LSL = (gvPoojaMaster.FooterRow.FindControl("txtLSL") as TextBox).Text;
                data.USL = (gvPoojaMaster.FooterRow.FindControl("txtUSL") as TextBox).Text;
                if (data.CompareType == "Range")
                {
                    if (data.USL == "" || data.LSL == "")
                    {
                        HelperClassGeneric.openWarningModal(this, data.Parameter + " -" + "LSL/USL can not be empty");
                        return;
                    }
                }
                if (data.USL != null && data.USL != "")
                {
                    if (Convert.ToDouble(data.LSL) > Convert.ToDouble(data.USL))
                    {
                        HelperClassGeneric.openWarningModal(this, data.Parameter + "- " + "LSL can not be greater than USL");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('LSL can not be greater than USL')", true);
                        return;
                    }
                }
                if (data.LSL != "")
                {
                    bool IsSaved = PoojaDBAccess.UpdatePoojaAndonMaster(data);
                    if (!IsSaved)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NoData", "alert('No data is saved.')", true);
                    }
                }
                gvPoojaMaster.ShowFooter = false;
                btnCancel.Visible = false;
                btnNew.Visible = true;
                BindPoojaAndonMasterData();
                HelperClassGeneric.openSuccessModal(this, "Data Added Successfully.");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        static string appPath = HttpContext.Current.Server.MapPath("~/Pooja/Template");
        public static string GetReportPath(string reportName)
        {
            string src;
            src = Path.Combine(appPath, reportName);
            return src;
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
        private static void DownloadFile(string filename, byte[] bytearray)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filename) + "\"");
            HttpContext.Current.Response.OutputStream.Write(bytearray, 0, bytearray.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        protected void btnTemplateExport_Click(object sender, EventArgs e)
        {
            try
            {
                string Filename = string.Empty;
                string templatefile = string.Empty;
                Filename = "PoojaAndonMasterTemplate.xlsx";
                string Source = string.Empty;
                Source = GetReportPath(Filename);
                string Template = string.Empty;
                Template = "PoojaAndonMasterTemplate" + DateTime.Now + ".xlsx";
                string destination = string.Empty;
                destination = Path.Combine(appPath, "Temp", SafeFileName(Template));
                if (!File.Exists(Source))
                {
                    Logger.WriteDebugLog("ImportCOPTemplate- \n " + Source);
                }
                FileInfo newFile = new FileInfo(Source);
                ExcelPackage Excel = new ExcelPackage(newFile, true);
                DownloadFile(destination, Excel.GetAsByteArray());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void gvPoojaMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    List<string> list = PoojaDBAccess.GetParameters();
                    DropDownList ddlParam = e.Row.FindControl("ddlParameter") as DropDownList;
                    ddlParam.DataSource = list;
                    ddlParam.DataBind();

                    //List<string> list1 = PoojaDBAccess.GetGradeIDs();
                    //DropDownList ddlGrade = e.Row.FindControl("ddlGradeID") as DropDownList;
                    //ddlGrade.DataSource = list1;
                    //ddlGrade.DataBind();
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlCompare = e.Row.FindControl("ddlCompareType") as DropDownList;
                    TextBox txtUpperSL = e.Row.FindControl("txtUSL") as TextBox;
                    HiddenField hdnCompareType = e.Row.FindControl("hdnCompareType") as HiddenField;
                    if (ddlCompare != null)
                    {
                        ddlCompare.SelectedValue = hdnCompareType.Value;
                        if (ddlCompare.SelectedValue == "Target")
                        {
                            txtUpperSL.Enabled = false;
                        }
                        else
                        {
                            txtUpperSL.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnCancel_ServerClick(object sender, EventArgs e)
        {
            btnNew.Visible = true;
            btnCancel.Visible = false;
            gvPoojaMaster.ShowFooter = false;
            BindPoojaAndonMasterData();
        }

        protected void btnNew_ServerClick(object sender, EventArgs e)
        {
            btnNew.Visible = false;
            btnCancel.Visible = true;
            gvPoojaMaster.ShowFooter = true;
            BindPoojaAndonMasterData();
            var footerRow = gvGradeID.FooterRow;
            (footerRow.FindControl("txtGradeID") as TextBox).Text = ddlGradeIDFilter.SelectedValue;
            //gvPoojaMaster_RowDataBound(null,null);
            //BindParameters();
            //BindGradeIDs();
        }

        protected void ddlCompareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ((sender as DropDownList).NamingContainer as GridViewRow).DataItemIndex;
            DropDownList ddlcomparision = (gvPoojaMaster.Rows[index].FindControl("ddlCompareType") as DropDownList);
            TextBox txtUpper = (gvPoojaMaster.Rows[index].FindControl("txtUSL") as TextBox);
            HiddenField hdnCompareType = (gvPoojaMaster.Rows[index].FindControl("hdnCompareType") as HiddenField);
            if (ddlcomparision != null)
            {
                //ddlcomparision.SelectedValue = hdnCompareType.Value;
                if (ddlcomparision.SelectedValue == "Target")
                {
                    txtUpper.Enabled = false;
                    txtUpper.Text = "";
                }
                else
                {
                    txtUpper.Enabled = true;
                }
            }
        }

        protected void ddlCompareType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DropDownList ddlcomparision = (gvPoojaMaster.FooterRow.FindControl("ddlCompareType") as DropDownList);
            TextBox txtUpper = (gvPoojaMaster.FooterRow.FindControl("txtUSL") as TextBox);
            if (ddlcomparision != null)
            {
                if (ddlcomparision.SelectedValue == "Target")
                {
                    txtUpper.Enabled = false;
                    txtUpper.Text = "";
                }
                else
                {
                    txtUpper.Enabled = true;
                }
            }
        }
       
        protected void btnSaveGradeID_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGradeID.Text == "")
                {
                    HelperClassGeneric.openWarningModal(this, "Grade ID Can't be Empty");
                    return;
                }
                string res = PoojaDBAccess.SaveGradeID(txtGradeID.Text);

                if (res.Equals("inserted", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openSuccessModal(this, "Inserted");
                    txtGradeID.Text = "";
                }

                else if (res.Equals("exists", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "exists");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Failed saving. Try again");
                BindGradeIDs();
                BindGradeIDGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openGradeIDModal", "OpenGradeIDModal();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var deleteIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
                Session["DeleteIndex"] = deleteIndex;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openGradeIDModal", "OpenGradeIDModal();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "deleteConfirmation", "OpenConfirmationModal();", true);

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnDeleteConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                var deleteIndex = Convert.ToInt32(Session["DeleteIndex"].ToString());
                var GradeID = (gvGradeID.Rows[deleteIndex].FindControl("lblGradeID") as Label).Text.ToString();

                string res = PoojaDBAccess.DeleteGradeID(GradeID.Trim());
                if (res.Equals("InTransaction", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningModal(this, "GradeID Cannot be deleted");
                else
                    HelperClassGeneric.openDeleteSuccessModal(this);

                BindGradeIDGrid();
                BindGradeIDs();
                BindPoojaAndonMasterData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openGradeIDModal", "OpenGradeIDModal();", true);

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindPoojaAndonMasterData();
        }
    }
}