using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class PartWeightMaster : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] == null || !Request.IsAuthenticated)
                {
                    Response.Redirect("~/SignIn");
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showLoader();", true);
                if (!IsPostBack)
                {
                    bindgrid("");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }

        }

        private void bindgrid(string component)
        {
            try
            {
                dt = DataBaseAccess.GetPartWeightData(component);
                Session["PartWeightMaster"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    partweightgridview.DataSource = dt;
                    partweightgridview.DataBind();
                }
                else
                {

                    dt.Columns.Add("ComponentID");
                    dt.Columns.Add("UnitWeight");
                    dt.Columns.Add("EffectiveDateTime");
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    partweightgridview.DataSource = dt;
                    partweightgridview.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        protected void btnimport_Click(object sender, EventArgs e)
        {
            try
            {
                bool saved = false;
                if (FileUpload1.HasFile && Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                {
                    lblMessages.Text = "";
                    DateTime effectivedate = DateTime.Now;
                    ExcelPackage Excel = new ExcelPackage(FileUpload1.PostedFile.InputStream);
                    var tbl = new DataTable();
                    bool hasHeader = true;
                    var ws = Excel.Workbook.Worksheets.First();
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text
                            : String.Format("Column {0}", firstRowCell.Start.Column));
                    int startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                            row[cell.Start.Column - 1] = cell.Text;
                    }
                    tbl.Columns[0].ColumnName = "ComponentID";
                    tbl.Columns[2].ColumnName = "UnitWeight";
                    tbl.Columns[3].ColumnName = "EffectiveDateTime";
                    if (tbl != null && tbl.Rows.Count > 0)
                    {
                        foreach (DataRow item in tbl.Rows)
                        {
                            DateTime effdate = DateTime.Now;
                            DateTime.TryParse(item["EffectiveDateTime"].ToString(), out effdate);
                            string effectivedates = effdate.ToString("yyyy-MM-dd HH:mm:ss");
                            effectivedates = VDGDataBaseAccess.GetLogicalDayStart(effectivedates);
                            saved = DataBaseAccess.Savepartweight(item["ComponentID"].ToString(), item["UnitWeight"].ToString(), effectivedates);
                        }
                    }
                }
                if (saved)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Imported SUCCESSFULLY')", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unit Weight Mis-Match \n Import Failed')", true);
                }

                bindgrid("");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessages.Text = "";
                bool saved = false;
                string component = "", Unitweight = "", Effectivedate = "";
                DateTime effectivedate = DateTime.Now;
                string hiddentxt = "";
                foreach (GridViewRow row in partweightgridview.Rows)
                {
                    hiddentxt = ((HiddenField)row.FindControl("hiddenfieltosave")).Value;
                    Double UnitWeight = 0.0;
                    Unitweight = ((TextBox)row.FindControl("lblunitweight")).Text;
                    if (Double.TryParse(Unitweight, out UnitWeight))
                    {
                        if (hiddentxt != (Unitweight) && !(string.IsNullOrEmpty(Unitweight)) && !(string.IsNullOrEmpty(hiddentxt)))
                        {
                            component = ((Label)row.FindControl("lblComponent")).Text;
                            Effectivedate = ((Label)row.FindControl("lbleffectivedate")).Text;
                            DateTime EffectivedateDate = DateTime.Now;
                            DateTime.TryParse(Effectivedate, out EffectivedateDate);
                            Effectivedate = EffectivedateDate.ToString("yyyy-MM-dd HH:mm:ss");
                            Effectivedate = VDGDataBaseAccess.GetLogicalDayStart(Effectivedate);
                            saved = DataBaseAccess.Savepartweight(component, Unitweight, Effectivedate);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unit Weight Mis-Match')", true);
                        return;
                    }
                }
                if (saved)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Saved SUCCESSFULLY')", true);
                bindgrid("");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                throw;
            }
        }

        protected void btnview_Click(object sender, EventArgs e)
        {
            string component = "";
            if (string.IsNullOrEmpty(txtComponentsearch.Text))
                component = "";
            else
                component = txtComponentsearch.Text;
            bindgrid(component);
        }

        protected void btnfootersave_Click(object sender, EventArgs e)
        {
            lblMessages.Text = "";
            bool saved = false;
            string component = "", Unitweight = "", Effectivedate = "";
            DateTime effectivedate = DateTime.Now;
            component = (partweightgridview.FooterRow.FindControl("lblfootercomponent") as TextBox).Text;
            Unitweight = (partweightgridview.FooterRow.FindControl("footerUnitWeight") as TextBox).Text;
            Double UnitWeight = 0.0;
            if (Double.TryParse(Unitweight, out UnitWeight))
            {
                Effectivedate = (partweightgridview.FooterRow.FindControl("txtEffectivedate") as TextBox).Text;
                DateTime.TryParse(Effectivedate, out effectivedate);
                Effectivedate = effectivedate.ToString("yyyy-MM-dd HH:mm:ss");
                Effectivedate = VDGDataBaseAccess.GetLogicalDayStart(Effectivedate);
                saved = DataBaseAccess.Savepartweight(component, Unitweight, Effectivedate);
                if (saved)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Inserted SUCCESSFULLY')", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Insert Unsuccessful')", true);
                    return;
                }

                bindgrid("");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unit Weight Mis-Match')", true);
                return;
            }
        }

        protected void partweightgridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dt = Session["PartWeightMaster"] as DataTable;
            partweightgridview.DataSource = dt;
            partweightgridview.PageIndex = e.NewPageIndex;
            partweightgridview.DataBind();
        }
    }
}