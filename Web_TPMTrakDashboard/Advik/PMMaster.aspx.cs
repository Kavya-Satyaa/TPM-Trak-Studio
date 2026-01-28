using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class PMMaster : System.Web.UI.Page
    {
        static string Machineid = "", PMID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindCell();
                bindMachineID();
                bindPMMasterDetails();
            }
        }

        private void bindCell()
        {
            try
            {
                List<string> GetCell = AdvikDatabaseAccess.GetCell("");
                ddlGroup.DataSource = GetCell;
                ddlGroup.DataBind();
                bindMachineID();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void bindMachineID()
        {
            try
            {
                string Cell = ddlGroup.SelectedValue == null ? "" : ddlGroup.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlGroup.SelectedItem.ToString();
                ddlMachineid.DataSource = AdvikDatabaseAccess.GetMachinesbyPlantCell("", Cell);
                ddlMachineid.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        private void bindPMMasterDetails()
        {
            try
            {
                string machineid = ddlMachineid.SelectedValue == null ? "" : ddlMachineid.SelectedItem.ToString();
                List<PMMasterDetails> listPMMasetDetails = AdvikDatabaseAccess.getPMMasterDetails(machineid);
                gvPMMaster.DataSource = listPMMasetDetails;
                gvPMMaster.DataBind();
            }catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            bindPMMasterDetails();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            int success = 0;
            string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DataTable dtPMDetails = new DataTable();
            if (filePMDetails.HasFile)
            {
                string fileName = filePMDetails.FileName;
                if (Path.GetExtension(fileName) != ".xlsx")
                {

                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Please choose the valid xlsx file";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                else
                {
                    string machineID = ddlMachineid.SelectedValue == null ? "" : ddlMachineid.SelectedItem.ToString();
                   
                    if (machineID == "")
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Machine ID required.";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        return;
                    }
                    if (!Directory.Exists(Server.MapPath("ImportedFiles")))
                    {
                        Directory.CreateDirectory(Server.MapPath("ImportedFiles"));
                        string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                        filePMDetails.SaveAs(savedFileName);
                        dtPMDetails = GetDataTableFromFile(savedFileName);
                        if (dtPMDetails != null && dtPMDetails.Rows.Count > 0)
                        {
                            success = AdvikDatabaseAccess.storeImportPMDetailsInfoExcelToDB(dtPMDetails, machineID);
                        }
                        else
                        {
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            lblMessages.Text = "Import failed. Empty excel file.";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            return;
                        }
                    }
                    else
                    {
                        string savedFileName = Server.MapPath("ImportedFiles//" + fileName);
                        filePMDetails.SaveAs(savedFileName);
                        dtPMDetails = GetDataTableFromFile(savedFileName);
                        if (dtPMDetails != null && dtPMDetails.Rows.Count > 0)
                        {
                            success = AdvikDatabaseAccess.storeImportPMDetailsInfoExcelToDB(dtPMDetails, machineID);
                        }
                        else
                        {
                            lblMessages.ForeColor = System.Drawing.Color.Red;
                            lblMessages.Text = "Import failed. Empty excel file.";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            return;
                        }
                    }
                }
             
                if (success <= 0)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Excel file import failed.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                if (success > 0)
                {
                    gvPMMaster.EditIndex = -1;
                    bindPMMasterDetails();
                }
            }
            else
            {
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = "Please choose a file to import.";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }

        private DataTable GetDataTableFromFile(string fileName)
        {
            DataTable dtPMMAster = new DataTable();
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    pck.Load(stream);
                }
                var workBook = pck.Workbook;
                if (workBook != null)
                {
                    try
                    {
                        var worksheet = workBook.Worksheets[1];
                        dtPMMAster.Columns.Add("PMActivity");
                        dtPMMAster.Columns.Add("NoOfCyle");
                        int startRow = 7, tblColStart = 0;
                        int lastRow = GetLastUsedRow(worksheet);
                        for (var rowNum = startRow; rowNum <= lastRow; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, 2, rowNum, 3];
                            var tblRow = dtPMMAster.NewRow();
                            tblColStart = 0;
                            foreach (var cell in wsRow)
                            {
                                
                                tblRow[tblColStart] = cell.Text.Trim();
                                tblColStart++;
                            }
                            dtPMMAster.Rows.Add(tblRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteDebugLog("Error while importing Part Station Information excel file : " + ex.Message);
                    }
                }
            }
            return dtPMMAster;
        }
        private static int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        protected void gvPMMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPMMaster.EditIndex = e.NewEditIndex;
            bindPMMasterDetails();
        }

        protected void gvPMMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Machineid = ddlMachineid.SelectedValue == null ? "" : ddlMachineid.SelectedItem.ToString(); 
                PMID = (gvPMMaster.Rows[e.RowIndex].FindControl("hfPMId") as HiddenField).Value;
                ScriptManager.RegisterStartupScript(this, GetType(), "DelParameters", "openConfirmModal();", true);
               // ClientScript.RegisterStartupScript(this.GetType(), "alert", "openConfirmModal();", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }

        }

        protected void gvPMMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                PMMasterDetails pmDetails = new PMMasterDetails();
                pmDetails.MachineID = ddlMachineid.SelectedValue == null ? "" : ddlMachineid.SelectedItem.ToString();
                pmDetails.PMActivity = (gvPMMaster.Rows[e.RowIndex].FindControl("edttxtPMActivity") as TextBox).Text;
                pmDetails.NoOfCycle = (gvPMMaster.Rows[e.RowIndex].FindControl("txtNoofCycle") as TextBox).Text;
                pmDetails.PMID = (gvPMMaster.Rows[e.RowIndex].FindControl("edthfPMId") as HiddenField).Value;
                if (pmDetails.MachineID == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Machine ID required.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                if (pmDetails.PMActivity == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "PM Activity field required.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                if (pmDetails.NoOfCycle == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Number of Cycle field required.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    return;
                }
                else
                {
                    if (Convert.ToInt32(pmDetails.NoOfCycle) <= 0)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Number of Cycle should be greater than 0";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        return;
                    }
                }
                int success = AdvikDatabaseAccess.InsertUpdatePMMasterDetails(pmDetails, "Update");
                if (success > 0)
                {
                    gvPMMaster.EditIndex = -1;
                    bindPMMasterDetails();
                }
                else if (success == -1)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "PM Activity not exists.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
                else if (success == -2)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "PM Activity already exists.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to update the record.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void gvPMMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPMMaster.EditIndex = -1;
            bindPMMasterDetails();
        }

        protected void yesGridDeleteBtn_Click(object sender, EventArgs e)
        {
            try
            {


                int sucess = AdvikDatabaseAccess.deletePMActivityDetails(Machineid, PMID);
                if (sucess > 0)
                {
                    bindPMMasterDetails();
                }
                else if (sucess == -1)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Cannot delete the Parameter details exists in transaction table.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to delete the record.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                PMMasterDetails pmDetails = new PMMasterDetails();
                pmDetails.MachineID = ddlMachineid.SelectedValue == null ? "" : ddlMachineid.SelectedItem.ToString();
                pmDetails.PMActivity = (gvPMMaster.FooterRow.FindControl("newtxtPMActivity") as TextBox).Text;
                pmDetails.NoOfCycle = (gvPMMaster.FooterRow.FindControl("newtxtNoofcycle") as TextBox).Text;
                pmDetails.PMID = "";
                if (pmDetails.MachineID == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text ="Machine ID required.";

                    ClientScript.RegisterStartupScript(this.GetType(), "alert1", "HideLabel();", true);
                    return;
                }
                if (pmDetails.PMActivity == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "PM Activity field required.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert2", "HideLabel();", true);
                    return;
                }
                if (pmDetails.NoOfCycle == "")
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Number of Cycle field required.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert3", "HideLabel();", true);
                    return;
                }
                else
                {
                    if (Convert.ToInt32(pmDetails.NoOfCycle) <= 0)
                    {
                        lblMessages.ForeColor = System.Drawing.Color.Red;
                        lblMessages.Text = "Number of Cycle should be greater than 0";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert4", "HideLabel();", true);
                        return;
                    }
                }
                int success = AdvikDatabaseAccess.InsertUpdatePMMasterDetails(pmDetails, "Insert");
                if (success > 0)
                {
                    bindPMMasterDetails();
                }
                else if (success == -1)
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text =" PM Activity already exists.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
                else
                {
                    lblMessages.ForeColor = System.Drawing.Color.Red;
                    lblMessages.Text = "Failed to insert new record.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }           
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindMachineID();
        }
    }
}