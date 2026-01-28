using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik.DataBaseAccess;
using Web_TPMTrakDashboard.Advik.Models;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Advik
{
    public partial class HelpRequestReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindAllPlants();
                //bindCellID();
                bindAllMachineIDs();
                bindAllShifts();
                bindAllHelpRequest();
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                bindHelpRequestDetails();
            }

        }
        private void bindAllPlants()
        {
            try
            {
                ddlPlant.DataSource = Web_TPMTrakDashboard.Models.DataBaseAccess.GetAllPlants();
                ddlPlant.DataBind();
                bindAllMachineIDs();
                //ddlPlantId.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommanResource", "PlantAll").ToString(), "All"));
                //ddlPlantId_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                //  ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        //private void bindCellID()
        //{
        //    try
        //    {
        //        string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
        //        List<string> GetCell = AdvikDatabaseAccess.GetCell(plant);
        //        ddlGroup.DataSource = GetCell;
        //        ddlGroup.DataBind();
        //        bindAllMachineIDs();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteErrorLog(ex.ToString());
        //    }
        //}

        private void bindAllMachineIDs()
        {
            try
            {
                string plant = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlant.SelectedItem.ToString();
                string Cell = "";
                ddlMachineId.DataSource = AdvikDatabaseAccess.GetMachinesbyPlantCell(plant, Cell);
                ddlMachineId.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }


        }
        private void bindAllShifts()
        {
            try
            {
                var allShift = BindCockpitView.GetAllShift();
                if (allShift != null && allShift.Count > 0)
                {
                    ddlShift.DataSource = allShift;
                    ddlShift.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        private void bindAllHelpRequest()
        {
            try
            {
                List<string> allHealthRequest = new List<string>();
                allHealthRequest = AdvikDatabaseAccess.getAllCallType();
                ddlHelpRequest.DataSource = allHealthRequest;
                ddlHelpRequest.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            bindHelpRequestDetails();
        }

        private void bindHelpRequestDetails()
        {
            try
            {
                string plantID = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                string GroupID = "";
                string machineIDs = "", shifts = "", helpRequest = "";
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        machineIDs += "'" + item.Value + "',";
                    }
                }
                foreach (ListItem item in ddlShift.Items)
                {
                    if (item.Selected)
                    {
                        shifts += "'" + item.Value + "',";
                    }
                }
                foreach (ListItem item in ddlHelpRequest.Items)
                {
                    if (item.Selected)
                    {
                        helpRequest += "'" + item.Value + "',";
                    }
                }
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                List<HelpRequestReportDetails> helpRequestReportDetailsList = new List<HelpRequestReportDetails>();
                List<string> plantIdlist = new List<string>();
                helpRequestReportDetailsList = AdvikDatabaseAccess.getHelpRequestDetails(plantID, machineIDs.TrimEnd(','), shifts.TrimEnd(','), helpRequest.TrimEnd(','), fromDate, toDate, GroupID, out plantIdlist);
                gvHelpRequestDetails.DataSource = helpRequestReportDetailsList;
                gvHelpRequestDetails.DataBind();
                var distinctPlantId = plantIdlist.Distinct().ToList();
                if (distinctPlantId.Count > 1)
                {
                    gvHelpRequestDetails.Columns[1].Visible = true;
                }
                else
                {
                    gvHelpRequestDetails.Columns[1].Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AckByOpreratorTime"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    gvHelpRequestDetails.Columns[8].Visible = true;
                }
                else
                {
                    gvHelpRequestDetails.Columns[8].Visible = false;
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["AckByOpreratorTimeFromTrigger"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    gvHelpRequestDetails.Columns[11].Visible = true;
                }
                else
                {
                    gvHelpRequestDetails.Columns[11].Visible = false;
                }
                Session["HelpRequestReportDetails"] = helpRequestReportDetailsList;
                List<HelpRequestReportDetails> avgCallTypeDetailsList = new List<HelpRequestReportDetails>();
                avgCallTypeDetailsList = AdvikDatabaseAccess.getCallTypeAvgDetails(plantID, machineIDs.TrimEnd(','), shifts.TrimEnd(','), helpRequest.TrimEnd(','), fromDate, toDate);
                Session["AvgCallTypeDetails"] = avgCallTypeDetailsList;
                DataTable avgCallTypedt = new DataTable();
                if (helpRequestReportDetailsList.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i < avgCallTypeDetailsList.Count; i++)
                    {
                        if (i == 0)
                        {
                            avgCallTypedt.Columns.Add("d");
                        }
                        avgCallTypedt.Columns.Add(avgCallTypeDetailsList[i].RequestType);
                    }
                    int k = 0;
                    dr = avgCallTypedt.NewRow();
                    foreach (DataColumn column in avgCallTypedt.Columns)
                    {
                        if (k == 0)
                        {
                            if (ConfigurationManager.AppSettings["GEAPages"].ToString() == "1")
                            {
                                dr["d"] = "Mean Time To Acknowledge ( MTTA in hh:mm:ss )";
                            }
                            else
                            {
                                dr["d"] = "Average Acknowledgement time ( hh:mm:ss )";
                            }
                        }
                        else
                        {
                            var avgacktime = avgCallTypeDetailsList.Where(x => x.RequestType == column.ColumnName).Select(x => x.AvgAckTimeFromTrigger).ToList()[0];
                            dr[column.ColumnName] = avgacktime;
                        }
                        k++;
                    }
                    avgCallTypedt.Rows.Add(dr);
                    k = 0;
                    dr = avgCallTypedt.NewRow();
                    foreach (DataColumn column in avgCallTypedt.Columns)
                    {
                        if (k == 0)
                        {
                            if (ConfigurationManager.AppSettings["GEAPages"].ToString() == "1")
                            {
                                dr["d"] = "Mean Time To Repair ( MTTR in hh:mm:ss )";
                            }
                            else
                            {
                                dr["d"] = "Average Reset time ( hh:mm:ss )";
                            }
                          
                        }
                        else
                        {
                            var avgacktime = avgCallTypeDetailsList.Where(x => x.RequestType == column.ColumnName).Select(x => x.AvgResetTimeFRomTrigger).ToList()[0];
                            dr[column.ColumnName] = avgacktime;
                        }
                        k++;
                    }
                    avgCallTypedt.Rows.Add(dr);

                    if (ConfigurationManager.AppSettings["GEAPages"].ToString() == "1")
                    {
                        k = 0;
                        dr = avgCallTypedt.NewRow();
                        foreach (DataColumn column in avgCallTypedt.Columns)
                        {
                            if (k == 0)
                            {
                                dr["d"] = "Mean Time Between Failures ( MTBF in hh:mm:ss )";
                            }
                            else
                            {
                                var avgacktime = avgCallTypeDetailsList.Where(x => x.RequestType == column.ColumnName).Select(x => x.MTBFValue).ToList()[0];
                                dr[column.ColumnName] = avgacktime;
                            }
                            k++;
                        }
                        avgCallTypedt.Rows.Add(dr);
                    }
                  
                    k = 0;
                    dr = avgCallTypedt.NewRow();
                    foreach (DataColumn column in avgCallTypedt.Columns)
                    {
                        if (k == 0)
                        {
                            dr["d"] = "Average Acknowledge Time By Operator (hh:mm:ss)";
                        }
                        else
                        {
                            var avgacktime = avgCallTypeDetailsList.Where(x => x.RequestType == column.ColumnName).Select(x => x.AvgAckOperatorTimeFromTrigger).ToList()[0];
                            dr[column.ColumnName] = avgacktime;
                        }
                        k++;
                    }
                    avgCallTypedt.Rows.Add(dr);
                    gvAvgCallTypeDetails.DataSource = avgCallTypedt;
                    gvAvgCallTypeDetails.DataBind();
                }
                else
                {
                    gvAvgCallTypeDetails.DataSource = avgCallTypedt;
                    gvAvgCallTypeDetails.DataBind();

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
                // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            bool successfull = false;
            List<HelpRequestReportDetails> listHRDetails = new List<HelpRequestReportDetails>();
            List<HelpRequestReportDetails> listAvgCallTypeDetails = new List<HelpRequestReportDetails>();
            try
            {
                string plantID = ddlPlant.SelectedValue == null ? "" : ddlPlant.SelectedItem.ToString();
                string GroupID = "";
                string plantNames = "";
                if (plantID == "All")
                {
                    for (int i = 1; i < ddlPlant.Items.Count; i++)
                    {
                        plantNames += ddlPlant.Items[i].Text + ",";
                    }
                }
                else
                {
                    plantNames = plantID;
                }
                string machineIDs = "", shifts = "", helpRequest = "";
                foreach (ListItem item in ddlMachineId.Items)
                {
                    if (item.Selected)
                    {
                        machineIDs += "'" + item.Value + "',";
                    }
                }
                foreach (ListItem item in ddlShift.Items)
                {
                    if (item.Selected)
                    {
                        shifts += "'" + item.Value + "',";
                    }
                }
                foreach (ListItem item in ddlHelpRequest.Items)
                {
                    if (item.Selected)
                    {
                        helpRequest += "'" + item.Value + "',";
                    }
                }
                DateTime fromDate = DateTime.Now.Date, toDate = DateTime.Now.Date;
                DateTime.TryParse(txtFromDate.Text, out fromDate);
                fromDate = Util.GetDateTime(txtFromDate.Text + " 11:00:00");
                toDate = Util.GetDateTime(txtToDate.Text + " 11:00:00");
                string Rp = TMPTrakGenerateReport.HelpRequestReport(fromDate, toDate, plantID, machineIDs.TrimEnd(','), shifts.TrimEnd(','));
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('" + Rp + ".')", true);
                //if (Session["HelpRequestReportDetails"] != null && Session["AvgCallTypeDetails"] != null)
                //{
                //    listHRDetails = Session["HelpRequestReportDetails"] as List<HelpRequestReportDetails>;
                //    listAvgCallTypeDetails = Session["AvgCallTypeDetails"] as List<HelpRequestReportDetails>;
                //}
                //else
                //{
                //    List<string> plantIdlist = new List<string>();

                //    listHRDetails = AdvikDatabaseAccess.getHelpRequestDetails(plantID, machineIDs.TrimEnd(','), shifts.TrimEnd(','), helpRequest.TrimEnd(','), fromDate, toDate, GroupID, out plantIdlist);
                //    listAvgCallTypeDetails = AdvikDatabaseAccess.getCallTypeAvgDetails(plantID, machineIDs.TrimEnd(','), shifts.TrimEnd(','), helpRequest.TrimEnd(','), fromDate, toDate);
                //}
                //if (listHRDetails != null && listHRDetails.Count > 0)
                //{
                //    Session["HelpRequestReportDetails"] = listHRDetails;
                //    Session["AvgCallTypeDetails"] = listAvgCallTypeDetails;
                ////    successfull = AdvikGenerateReports.GenerateHelpRequestReport(fromDate, toDate, plantNames, listHRDetails, listAvgCallTypeDetails);
                //    if (!successfull)
                //    {
                //        lblMessages.ForeColor = Color.Red;
                //        lblMessages.Text = "Error. Export Unsuccessful.";
                //        //  ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //    }
                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Export Successful.')", true);
                //  else
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoDataToExport", "alert('Error. Export Unsuccessful.')", true);
                //}
                //else
                //{
                //    lblMessages.ForeColor = Color.Red;
                //    lblMessages.Text = "No data to export";
                //    //  ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //}

            }
            catch (Exception ex)
            {
                lblMessages.ForeColor = Color.Red;
                lblMessages.Text = ex.Message;
                // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindAllMachineIDs();
        }

        //protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    bindAllMachineIDs();
        //}
    }
}