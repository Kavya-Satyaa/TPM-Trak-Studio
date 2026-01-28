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

namespace Web_TPMTrakDashboard.TAFE
{
    public partial class TafePlanVsActual : System.Web.UI.Page
    {
        DataTable dtPlanVsActualDataDaywise = null;
        DataTable dtPlanVsActualDataCumulative = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                if (!IsPostBack)
                {
                    txtYear.Text = DateTime.Now.Year.ToString();
                    txtMonth.Text = DateTime.Now.Month.ToString("00");
                    SessionClear.ClearSession();
                    BindPlantIDs();
                    BindLineIDs();
                    BindPlanVsActualGrids();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        #region "Bind Plant Id"
        private void BindPlantIDs()
        {
            try
            {
                List<string> lstPlantData = TafeDataBaseAccess.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Plant Id"
        private void BindLineIDs()
        {
            try
            {
                string PlantId = ddlPlantId.SelectedItem != null ? ddlPlantId.SelectedItem.ToString() : "";
                if (!string.IsNullOrEmpty(PlantId))
                {
                    List<string> lstLineData = TafeDataBaseAccess.GetLineIDsForPlant(PlantId);
                    ddlLineId.DataSource = lstLineData;
                    ddlLineId.DataBind();
                    ddlLineId.Items.Insert(0, new ListItem
                    {
                        Text = "Line All",
                        Value = "All"
                    });
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Plan Vs Actual Grid"
        private void BindPlanVsActualGrids()
        {
            try
            {
                string PlantId = ddlPlantId.SelectedItem != null ? ddlPlantId.SelectedItem.ToString() : "";
                string LineId = ddlLineId.SelectedItem != null ? ddlLineId.SelectedItem.ToString() : "";
                string Year = txtYear.Text;
                string Month = txtMonth.Text;
                if (!string.IsNullOrEmpty(PlantId) && !string.IsNullOrEmpty(LineId) && !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Month))
                {
                    dtPlanVsActualDataDaywise = new DataTable();
                    dtPlanVsActualDataCumulative = new DataTable();
                    string Date = Year + "-" + Month + "-01";
                    dtPlanVsActualDataDaywise = TafeDataBaseAccess.GetPlanVsActualData(PlantId, LineId, Date, out dtPlanVsActualDataCumulative);
                    Session["PlanVsActualDataDaywise"] = dtPlanVsActualDataDaywise;
                    Session["PlanVsActualDataCumulative"] = dtPlanVsActualDataCumulative;
                    if (dtPlanVsActualDataDaywise != null && dtPlanVsActualDataDaywise.Rows.Count > 0)
                    {
                        GridPlanVsActualDaywise.DataSource = dtPlanVsActualDataDaywise;
                        GridPlanVsActualDaywise.DataBind();
                    }
                    else
                    {
                        GridPlanVsActualDaywise.DataSource = new DataTable();
                        GridPlanVsActualDaywise.DataBind();
                    }
                    if (dtPlanVsActualDataCumulative != null && dtPlanVsActualDataCumulative.Rows.Count > 0)
                    {
                        GridPlanVsActualCumulative.DataSource = dtPlanVsActualDataCumulative;
                        GridPlanVsActualCumulative.DataBind();
                    }
                    else
                    {
                        GridPlanVsActualCumulative.DataSource = new DataTable();
                        GridPlanVsActualCumulative.DataBind();
                    }
                }
                else
                {
                    GridPlanVsActualDaywise.DataSource = new DataTable();
                    GridPlanVsActualDaywise.DataBind();
                    GridPlanVsActualCumulative.DataSource = new DataTable();
                    GridPlanVsActualCumulative.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        protected void GridPlanVsActualCumulative_DataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            string Month = "", Year = "";
            if (dtPlanVsActualDataCumulative != null && dtPlanVsActualDataCumulative.Rows.Count > 0)
            {
                Month = Convert.ToDateTime(dtPlanVsActualDataCumulative.Rows[0]["LastAggDateForMonth"].ToString()).ToString("dd-MMM-yyyy") + ")";
                Year = Convert.ToDateTime(dtPlanVsActualDataCumulative.Rows[0]["LastAggDateForYear"].ToString()).ToString("dd-MMM-yyyy") + ")";
            }
            else
            {
                Month = Util.GetDateTime(txtYear.Text + "-" + txtMonth.Text + "-01").ToString("MMMM") + ")";
                Year = Util.GetDateTime(txtYear.Text + "-" + txtMonth.Text + "-01").ToString("yyyy") + ")";
            }
            TableHeaderCell cellHeaderGroup = new TableHeaderCell
            {
                Text = "Cumulative By Part",
                ColumnSpan = 2
            };
            row.Controls.Add(cellHeaderGroup);

            cellHeaderGroup = new TableHeaderCell
            {
                Text = "MTD(" + Month,
                ColumnSpan = 6,
                HorizontalAlign = HorizontalAlign.Center
            };
            row.Controls.Add(cellHeaderGroup);

            cellHeaderGroup = new TableHeaderCell
            {
                Text = "YTD(" + Year,
                ColumnSpan = 6,
                HorizontalAlign = HorizontalAlign.Center
            };
            row.Controls.Add(cellHeaderGroup);

            row.BackColor = ColorTranslator.FromHtml("#5391CA");
            GridPlanVsActualCumulative.HeaderRow.Parent.Controls.AddAt(0, row);
        }

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLineIDs();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindPlanVsActualGrids();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successful = false;
            try
            {
                string Year = txtYear.Text;
                string Month = txtMonth.Text;
                string Date = Year + "-" + Month + "-01";
                string PlantId = ddlPlantId.SelectedItem != null ? ddlPlantId.SelectedItem.ToString() : "";
                string LineId = ddlLineId.SelectedItem != null ? ddlLineId.SelectedItem.ToString() : "";
                if (Session["PlanVsActualDataDaywise"] != null && Session["PlanVsActualDataCumulative"] != null)
                {
                    dtPlanVsActualDataDaywise = Session["PlanVsActualDataDaywise"] as DataTable;
                    dtPlanVsActualDataCumulative = Session["PlanVsActualDataCumulative"] as DataTable;
                }
                else
                {
                    dtPlanVsActualDataDaywise = TafeDataBaseAccess.GetPlanVsActualData(PlantId, LineId, Date, out dtPlanVsActualDataCumulative);
                }
                if (dtPlanVsActualDataDaywise != null && dtPlanVsActualDataDaywise.Rows.Count > 0)
                {
                    successful = TAFEGenerateReports.GeneratePlanVsActualReport(PlantId, LineId, Date, dtPlanVsActualDataDaywise, dtPlanVsActualDataCumulative);
                    if (successful)
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Export Successful.')", true);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                }
            }
            catch (Exception ex)
            {
                lblMessages.Text = ex.Message;
            }
        }
    }
}