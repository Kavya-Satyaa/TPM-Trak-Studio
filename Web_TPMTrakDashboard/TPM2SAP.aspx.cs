using Elmah;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class TPM2SAP : System.Web.UI.Page
    {
		public static DataTable dtReworkDetails = new DataTable();
		DataTable dtSummary = new DataTable();
        DateTime FromDate = DateTime.Now.Date;
        DateTime ToDate = DateTime.Now.Date;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				txtFromDate.Text = DateTime.Now.AddDays(-29).ToString("yyyy-MM-dd hh:mm:ss");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                LoadMachineID();
                LoadPlanNumbers();
                LoadTPM2SAPGrids(txtFromDate.Text, txtToDate.Text, ddlMachineId.SelectedItem == null || ddlMachineId.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineId.Text, ddlPlanNumber.SelectedItem == null || ddlPlanNumber.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlanNumber.Text, ddlMode.SelectedItem == null || ddlMode.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMode.Text);
            }
        }

        private void LoadMachineID()
        {
            try
            {
                var allMachineName = VDGDataBaseAccess.GetAllMachines("All");
                allMachineName.Insert(0, "All");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                    ddlMachineId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void LoadPlanNumbers()
        {
            List<string> planno = new List<string>();
            string fromdate = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
            string todate = Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd HH:mm:ss");
            string Machine = ddlMachineId.SelectedItem == null || ddlMachineId.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineId.Text;
            if (Machine != null)
                planno = VDGDataBaseAccess.getpalnno(fromdate, todate, Machine);
            if (planno != null && planno.Count > 0)
            {
                ddlPlanNumber.DataSource = planno;
                ddlPlanNumber.DataBind();
            }
        }

        private void LoadTPM2SAPGrids(string fromDate, string toDate, string machineId, string planNumber, string mode)
        {
            FromDate = Util.GetDateTime(fromDate);
            ToDate = Util.GetDateTime(toDate);
            if ((ToDate - FromDate).TotalDays > 30)
            {
                lblMessages.Text = "Difference between to date and from date cannot be more than 31 days.";
                return;
            }
            else
            {
                BindScheduleDataGrid(fromDate, toDate, machineId, planNumber, mode, "Schedule");
                BindProductionDataGrid(fromDate, toDate, machineId, planNumber, mode, "Production");
                BindDownDataGrid(fromDate, toDate, machineId, planNumber, mode, "Down");
                BindSummaryDataGrid(fromDate, toDate, machineId, planNumber, mode, "Summary");
				BindReworkDetails(fromDate, toDate, machineId);
			}
        }

		private void BindReworkDetails(string fromDate, string toDate, string machineID)
		{
			try
			{
				dtReworkDetails = DataBaseAccess.GetAllReworkDetails(fromDate, toDate, machineID);
				grdRework.DataSource = dtReworkDetails;
				grdRework.DataBind();
				ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "hideLoader();", true);
			}
			catch (Exception ex)
			{
				lblMessages.Text = ex.Message;
			}
		}

		private void BindScheduleDataGrid(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            DataTable dtSchedule = null;
            try
            {
                if (Session["ScheduleDataSource"] == null)
                {
                    dtSchedule = DataBaseAccess.GetScheduleDataForMachine(fromDate, toDate, machineId, planNumber, mode, param);
                    Session["ScheduleDataSource"] = dtSchedule;
                }
                else
                {
                    dtSchedule = Session["ScheduleDataSource"] as DataTable;
                }

                if (dtSchedule.Rows.Count > 0)
                {
                    gridScheduleData.AllowPaging = true;
                    gridScheduleData.PageSize = VDGDataBaseAccess.PageSize;
                    gridScheduleData.DataSource = dtSchedule;
                    gridScheduleData.DataBind();
                    foreach (GridViewRow oItem in gridScheduleData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "pdrow");
                        oItem.Attributes.Add("id", "pdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridScheduleData.DataSource = dtSchedule;
                    gridScheduleData.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindProductionDataGrid(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            DataTable dtProduction = null;
            try
            {
                if (Session["ProductionDataSource"] == null)
                {
                    dtProduction = DataBaseAccess.GetProductionDataForMachine(fromDate, toDate, machineId, planNumber, mode, param);
                    Session["ProductionDataSource"] = dtProduction;
                }
                else
                {
                    dtProduction = Session["ProductionDataSource"] as DataTable;
                }

                if (dtProduction.Rows.Count > 0)
                {
                    gridProductionData.AllowPaging = true;
                    gridProductionData.PageSize = VDGDataBaseAccess.PageSize;
                    gridProductionData.DataSource = dtProduction;
                    gridProductionData.DataBind();
                    foreach (GridViewRow oItem in gridProductionData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "pdrow");
                        oItem.Attributes.Add("id", "pdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridProductionData.DataSource = dtProduction;
                    gridProductionData.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindDownDataGrid(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            DataTable dtDowntime = null;
            try
            {
                if (Session["DowntimeDataSource"] == null)
                {
                    dtDowntime = DataBaseAccess.GetDowntimeDataForMachine(fromDate, toDate, machineId, planNumber, mode, param);
                    Session["DowntimeDataSource"] = dtDowntime;
                }
                else
                {
                    dtDowntime = Session["DowntimeDataSource"] as DataTable;
                }

                if (dtDowntime.Rows.Count > 0)
                {
                    gridDownData.AllowPaging = true;
                    gridDownData.PageSize = VDGDataBaseAccess.PageSize;
                    gridDownData.DataSource = dtDowntime;
                    gridDownData.DataBind();
                    foreach (GridViewRow oItem in gridDownData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "pdrow");
                        oItem.Attributes.Add("id", "pdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridDownData.DataSource = dtDowntime;
                    gridDownData.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        private void BindSummaryDataGrid(string fromDate, string toDate, string machineId, string planNumber, string mode, string param)
        {
            try
            {
                if (Session["SummaryDataSource"] == null)
                {
                    dtSummary = DataBaseAccess.GetSummaryDataForMachine(fromDate, toDate, machineId, planNumber, mode, param);
                    Session["SummaryDataSource"] = dtSummary;
                }
                else
                {
                    dtSummary = Session["SummaryDataSource"] as DataTable;
                }

                if (dtSummary.Rows.Count > 0)
                {
                    gridSummaryData.AllowPaging = true;
                    gridSummaryData.PageSize = VDGDataBaseAccess.PageSize;
                    gridSummaryData.DataSource = dtSummary;
                    gridSummaryData.DataBind();
                    foreach (GridViewRow oItem in gridSummaryData.Rows)
                    {
                        string getFirstColValue = oItem.Cells[0].Text;
                        oItem.Attributes.Add("class", "pdrow");
                        oItem.Attributes.Add("id", "pdrow" + getFirstColValue);
                    }
                }
                else
                {
                    gridSummaryData.DataSource = dtSummary;
                    gridSummaryData.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Session["ScheduleDataSource"] = null;
            Session["ProductionDataSource"] = null;
            Session["DowntimeDataSource"] = null;
            Session["SummaryDataSource"] = null;
            LoadTPM2SAPGrids(txtFromDate.Text, txtToDate.Text, ddlMachineId.SelectedItem == null || ddlMachineId.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMachineId.Text, ddlPlanNumber.SelectedItem == null || ddlPlanNumber.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlanNumber.Text, ddlMode.SelectedItem == null || ddlMode.Text.Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlMode.Text);
        }

        protected void btnRefreshPlanNo_ServerClick(object sender, EventArgs e)
        {
            LoadPlanNumbers();
        }

        protected void btnExportSummary_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int val = 0;
                string appPath = HttpContext.Current.Server.MapPath("~/Reports");
                string src, dst = string.Empty;
                string ReportName = "TPM_TO_SAP_Summary.xlsx";
                if (HttpContext.Current.Session["Language"] == null)
                    src = Path.Combine(appPath, "TPMTOSAP", ReportName);
                else
                {
                    if (HttpContext.Current.Session["Language"].ToString() != "en")
                        src = Path.Combine(appPath, "TPMTOSAP-" + HttpContext.Current.Session["Language"].ToString() + "", ReportName);
                    else
                        src = Path.Combine(appPath, "TPMTOSAP", ReportName);
                }
                string tempfileName = "ProductionSummaryData" + "_" + Guid.NewGuid() + ".xlsx";
                dst = Path.Combine(appPath, "Temp", tempfileName);
                FileInfo newFile = new FileInfo(src);
                ExcelPackage excelPackage = new ExcelPackage(newFile, true);
                ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets[1];
                worksheet1.Cells["B4"].Value = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MM-yyyy HH:mm:ss");
                worksheet1.Cells["D4"].Value = Convert.ToDateTime(txtToDate.Text).ToString("dd-MM-yyyy HH:mm:ss");
                worksheet1.Cells["F4"].Value = ddlMachineId.SelectedValue.ToString();
                worksheet1.Cells["H4"].Value = ddlPlanNumber.SelectedValue.ToString();
                int y = 8, z = 1, k = 8, l = 1;
                foreach (GridViewRow row in gridSummaryData.Rows)
                {
                    z = 1;
                    for (int j = 0; j < gridSummaryData.Columns.Count; j++)
                    {
                        if (z > 5)
                        {
                            if (int.TryParse(row.Cells[j].Text, out val) && !string.IsNullOrEmpty(row.Cells[j].Text))
                            {
                                worksheet1.Cells[y, z].Value = val;
                            }
                            else
                            {
                                worksheet1.Cells[y, z].Value = row.Cells[j].Text;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(row.Cells[j].Text) && !row.Cells[j].Text.Equals("&nbsp;"))
                            {
                                worksheet1.Cells[y, z].Value = row.Cells[j].Text;
                            }
                            else
                            {
                                worksheet1.Cells[y, z].Value = "";
                            }
                        }
                        z++;
                    }
                    y++;
                }
                z--;
                y--;
                worksheet1.Cells[k, l, y, z].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet1.Cells[k, l, y, z].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet1.Cells[k, l, y, z].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet1.Cells[k, l, y, z].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet1.Cells[k, l, y, z].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                worksheet1.Cells[k, l, y, z].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                worksheet1.Cells[k, l, y, z].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                worksheet1.Cells[k, l, y, z].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                DownloadMultipleFile(dst, excelPackage.GetAsByteArray());
                Logger.WriteDebugLog("TPM to SAP report saved.");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);

            }
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
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //Logger.WriteErrorLog("GENERATED ERROR : \n" + "Report generation Failed Error: " + ex.ToString());
            }
        }

		protected void btnReworkExport_ServerClick(object sender, EventArgs e)
		{
			try
			{
				bool successful = false;
				DateTime Fromdate = DateTime.Now;
				DateTime.TryParse(txtFromDate.Text, out Fromdate);
				DateTime Todate = DateTime.Now;
				DateTime.TryParse(txtToDate.Text, out Todate);
				if (dtReworkDetails != null && dtReworkDetails.Rows.Count > 0)
				{
					successful = TMPTrakGenerateReport.ReworkDetailReport(Fromdate, Todate, ddlMachineId.SelectedValue.ToString(), dtReworkDetails);
					if (successful)
						lblMessages.Text = "Export Successful";
					else
						lblMessages.Text = "Export Unsuccessful";
				}
				else
				{
					lblMessages.Text = "No data to export";
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
				throw;
			}
		}
	}
}