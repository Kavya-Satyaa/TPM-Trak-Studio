using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class FinalInspectionReportAdvik : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearGridData();
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindLine();
                btnViewDate_Click(null, null);
            }
        }
        public void BindLine()
        {
            try
            {
                List<string> list = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                if (list.Count > 0)
                {
                    list.Remove("All");
                }
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindLine = " + ex.Message);
            }
        }
        protected void btnViewSlno_Click(object sender, EventArgs e)
        {
            try
            {

                hdnViewType.Value = "Slno";
                ClearGridData();
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnViewSlno_Click = " + ex.Message);
            }
        }

        protected void btnViewDate_Click(object sender, EventArgs e)
        {
            try
            {
                hdnViewType.Value = "Date";
                ClearGridData();
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnViewDate_Click = " + ex.Message);
            }
        }
        private void ClearGridData()
        {
            try
            {
                Session["FISlnoList"] = null;
                Session["FIStationStatusData"] = null;
                Session["FIParameterData"] = null;
                List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
                lvFinalData.DataSource = list;
                lvFinalData.DataBind();
                lvStatusData.DataSource = list;
                lvStatusData.DataBind();
                stationStatusDiv.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }
        private void BindData()
        {
            try
            {
                List<FinalInspectionEnity> list = AdvikDatabaseAccess.getFinalInspectionReportSlnoList(ddlPlant.SelectedValue, ddlStatus.SelectedValue, txtSlNoSearch.Text, txtFromDate.Text, txtToDate.Text, hdnViewType.Value);
                Session["FISlnoList"] = list;
                gvSlnoDetails.DataSource = list;
                gvSlnoDetails.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }
        protected void gvSlnoDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvSlnoDetails, "Select$" + e.Row.RowIndex);
                    e.Row.Attributes["style"] = "cursor:pointer";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("gvSlnoDetails_RowDataBound = " + ex.Message);
            }
        }

        protected void gvSlnoDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = gvSlnoDetails.SelectedRow.RowIndex;
                string slno = gvSlnoDetails.SelectedRow.Cells[0].Text;
                string partnumber = gvSlnoDetails.SelectedRow.Cells[2].Text;
                string plant = gvSlnoDetails.SelectedRow.Cells[3].Text;
                foreach (GridViewRow row in gvSlnoDetails.Rows)
                {
                    if (row.RowIndex == gvSlnoDetails.SelectedIndex)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    }
                    else
                    {
                        row.BackColor = Color.White; /*ColorTranslator.FromHtml("#FFFFFF");*/
                    }
                }
                BindStationStatusData(partnumber, slno, plant);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("gvSlnoList_SelectedIndexChanged = " + ex.Message);
            }
        }
        private void BindStationStatusData(string partnumber, string slno, string plant)
        {
            try
            {
                List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
                list = AdvikDatabaseAccess.getFinalInspectionReportMachineStatus(partnumber, slno, plant);

                if (list.Count > 0)
                {
                    stationStatusDiv.Visible = true;
                    lvStatusData.DataSource = list;
                    lvStatusData.DataBind();
                    Session["FIStationStatusData"] = list;
                    BindFinalInspectionParameterData(partnumber, slno, plant);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }
        private void BindFinalInspectionParameterData(string partnumber, string slno, string plant)
        {
            try
            {
                List<ParameterMasterEntity> list = new List<ParameterMasterEntity>();
                DataTable dt2 = new DataTable();
                DataTable dt = AdvikDatabaseAccess.getFinalInspectionReportParameterList(partnumber, slno, plant, AdvikDatabaseAccess.getFinalInspectionMachineID(), out dt2);

                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (i == 0)
                        {
                            list.Add(new ParameterMasterEntity
                            {
                                Type = "PartNo",
                                Label = "Part Number",
                                Value = row["ComponentID"].ToString()
                            });
                            list.Add(new ParameterMasterEntity { Type = "Slno", Label = "Serial Number", Value = slno });
                        }
                        ParameterMasterEntity data = new ParameterMasterEntity();
                        data.Type = "Chk";
                        data.Label = row["ParameterID"].ToString();
                        data.ParameterID = row["ParameterID"].ToString();
                        data.ParameterName = row["ParameterName"].ToString();
                        data.Value = row["Value"].ToString();
                        list.Add(data);
                        i++;
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt2.Rows)
                        {
                            ParameterMasterEntity data = new ParameterMasterEntity();
                            data.Type = "Label";
                            data.Label = "Checked By";
                            data.Value = row["UpdatedBy"].ToString();
                            list.Add(data);

                            data = new ParameterMasterEntity();
                            data.Type = "Label";
                            data.Label = "Date of Inspection";
                            data.Value = row["UpdatedTS"].ToString();
                            list.Add(data);

                            data = new ParameterMasterEntity();
                            data.Type = "Label";
                            data.Label = "Remarks";
                            data.Value = row["Remarks"].ToString();
                            list.Add(data);
                            break;
                        }
                    }
                }
                lvFinalData.DataSource = list;
                lvFinalData.DataBind();
                Session["FIParameterData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }

        protected void lnkExportSlnoList_Click(object sender, EventArgs e)
        {
            try
            {
                List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
                if (Session["FISlnoList"] == null)
                {
                    return;
                }
                list = Session["FISlnoList"] as List<FinalInspectionEnity>;
                if (list.Count > 0)
                {
                    string plant = ddlPlant.SelectedValue;
                    string status = ddlStatus.SelectedValue;
                    bool successfull = AdvikGenerateReports.GenerateFinalInspectionSlnoReport(plant, status, txtFromDate.Text, txtToDate.Text, txtSlNoSearch.Text, hdnViewType.Value, list);
                    if (!successfull)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
        }

        protected void lnkExportStausParam_Click(object sender, EventArgs e)
        {
            try
            {
                List<FinalInspectionEnity> listStatus = new List<FinalInspectionEnity>();
                List<ParameterMasterEntity> listPatameter = new List<ParameterMasterEntity>();
                if (Session["FIStationStatusData"] == null)
                {
                    return;
                }
                if (Session["FIParameterData"] == null)
                {
                    return;
                }
                listStatus = Session["FIStationStatusData"] as List<FinalInspectionEnity>;
                listPatameter = Session["FIParameterData"] as List<ParameterMasterEntity>;
                if (listStatus.Count > 0)
                {
                    string plant = ddlPlant.SelectedValue;
                    string status = ddlStatus.SelectedValue;
                    bool successfull = AdvikGenerateReports.GenerateFinalInspectionStatusAndParamReport(plant, status, txtFromDate.Text, txtToDate.Text, txtSlNoSearch.Text, hdnViewType.Value, listStatus, listPatameter);
                    if (!successfull)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('No data to export.')", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteDebugLog(ex.Message);
            }
        }

        protected void lnkRefreshbutton_Click(object sender, EventArgs e)
        {
            try
            {
                txtSlNoSearch.Text = "";
                txtSlNoSearch.Focus();
                List<FinalInspectionEnity> list = new List<FinalInspectionEnity>();
                gvSlnoDetails.DataSource = list;
                gvSlnoDetails.DataBind();
                ClearGridData();
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}