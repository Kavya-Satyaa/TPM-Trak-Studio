using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.AmararajaMangal.DTO;

namespace Web_TPMTrakDashboard.AmararajaMangal
{
    public partial class ScrapEntryScreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    BindShiftData();
                    BindPlantData();
                    ddlPlant_SelectedIndexChanged(null, null);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindShiftData()
        {
            try
            {
                ddlShift.DataSource = DataBaseAccess.shiftdetail();
                ddlShift.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindMachineData()
        {
            try
            {
                lbMachine.DataSource = VDGDataBaseAccess.GetAllMachines(ddlPlant.SelectedValue);
                lbMachine.DataBind();
                int i = 0;
                foreach (ListItem item in lbMachine.Items)
                {
                    item.Selected = true;
                    //if (i == 1)
                    //{
                    //    break;
                    //}

                    i++;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        private void BindPlantData()
        {
            try
            {
                ddlPlant.DataSource = DataBaseAccess.GetAllPlants();
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachineData();
        }
        private void BindData()
        {
            try
            {
                string machine = "";
                foreach (ListItem item in lbMachine.Items)
                {
                    if (item.Selected)
                    {
                        if (machine == "")
                        {
                            machine += "'" + item.Value + "'";
                        }
                        else
                        {
                            machine += ",'" + item.Value + "'";
                        }
                    }
                }
                DataTable dt = AmararajaMangalDataBaseAccess.getScrapEntryDetails(machine, txtDate.Text, ddlShift.SelectedValue);
                List<ScrapEntryData> list = new List<ScrapEntryData>();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var distMachine = dt.AsEnumerable().Select(k => k.Field<string>("Machineid")).Distinct().ToList();
                        for (int machineCount = 0; machineCount < distMachine.Count; machineCount++)
                        {
                            string machineid = distMachine[machineCount];
                            var dataRows = dt.AsEnumerable().Where(k => k.Field<string>("Machineid") == machineid).ToList();
                            int rowSpan = dataRows.Count;
                            foreach (DataRow row in dataRows)
                            {
                                ScrapEntryData data = new ScrapEntryData();
                                data.MachineID = row["Machineid"].ToString();
                                data.PartID = row["ComponentID"].ToString();
                                data.TotalPartProduced = row["TotalParts"].ToString();
                                data.AcceptedPart = row["acceptedparts"].ToString();
                                data.UnitWeight = row["unitweight"].ToString();
                                data.TotalAcceptedPart = row["totalweightaccpeted"].ToString();
                                data.TotalWeight = row["TotalWeightPerMachine"].ToString();
                                data.Rejection = row["RejectedQty"].ToString();
                                data.RejectedParts = row["RejectedParts"].ToString();
                                data.DesignScrap = row["designscrap"].ToString();
                                data.LayoutScrap = row["LayOutScrap"].ToString();
                                data.TotalScrap = row["TotalScrap"].ToString();
                                data.RowSpan = rowSpan.ToString();
                                if (rowSpan > 0)
                                {
                                    data.RowSpanClass = "showRowSpan";
                                }
                                else
                                {
                                    data.RowSpanClass = "hideRowSpan";
                                }
                                rowSpan = 0;
                                list.Add(data);
                            }
                        }
                    }
                }
                lvScrapEntry.DataSource = list;
                lvScrapEntry.DataBind();
                Session["ScrapEntryData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int updatedCount = 0;
                for (int i = 0; i < lvScrapEntry.Items.Count; i++)
                {
                    var item = lvScrapEntry.Items[i];
                    if ((item.FindControl("hdnUpdate") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        ScrapEntryData data = new ScrapEntryData();
                        data.MachineID = (item.FindControl("lblMachine") as Label).Text;
                        data.Date = txtDate.Text;
                        data.Shift = ddlShift.SelectedValue;
                        data.LayoutScrap = (item.FindControl("txtLayoutScrap") as TextBox).Text;
                        data.DesignScrap = (item.FindControl("txtDesignScrap") as TextBox).Text;
                        data.TotalScrap = (item.FindControl("hdnTotalScrap") as HiddenField).Value;
                        updatedCount += AmararajaMangalDataBaseAccess.insertScrapEntryDetails(data);
                    }
                }
                if (updatedCount > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "suucessMsg", "openSuccessModal('Saved Successfully.')", true);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ScrapEntryData"] == null)
                {
                    return;
                }
                string machine = "";
                int i = 0;
                foreach (ListItem item in lbMachine.Items)
                {
                    if (item.Selected)
                    {
                        if (machine == "")
                        {
                            machine += item.Value;
                        }
                        else
                        {
                            machine += "," + item.Value;
                        }
                        i++;
                        if (i == 2)
                        {
                            machine += "...";
                            break;
                        }

                    }
                }
                List<ScrapEntryData> list = Session["ScrapEntryData"] as List<ScrapEntryData>;
                AmarGenerateReport.generateScrapEntryReport(machine, ddlShift.SelectedValue, txtDate.Text, list);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

       
    }
}