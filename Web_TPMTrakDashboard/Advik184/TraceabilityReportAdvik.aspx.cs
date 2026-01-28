using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Advik184.Models;

namespace Web_TPMTrakDashboard.Advik184
{
    public partial class TraceabilityReportAdvik : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["TraceabilityData"] = null;
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindPlant();
                btnViewDate_Click(null, null);
            }
        }
        public void BindPlant()
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
                BindStation();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnViewSlno_Click = " + ex.Message);
            }
        }
        public void BindStation()
        {
            try
            {
                List<string> list = Web_TPMTrakDashboard.Models.DataBaseAccess.getmachineidfromplant(ddlPlant.SelectedValue);
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnViewSlno_Click = " + ex.Message);
            }
        }
        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStation();
        }

        protected void btnViewSlno_Click(object sender, EventArgs e)
        {
            try
            {
                hdnViewType.Value = "Slno";
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
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnViewDate_Click = " + ex.Message);
            }
        }
        private void BindData()
        {
            try
            {
                List<TraceabilityEntity> list = new List<TraceabilityEntity>();
                DataTable dt = AdvikDatabaseAccess.getTraceabilityReportDetails(ddlPlant.SelectedValue, ddlMachine.SelectedValue, txtSlNoSearch.Text, txtFromDate.Text, txtToDate.Text, hdnViewType.Value);

                var distSlno = dt.AsEnumerable().Select(k => k.Field<string>("Barcode")).Distinct().ToList();
                foreach (string slno in distSlno)
                {
                    var dataRows = dt.AsEnumerable().Where(k => k.Field<string>("Barcode") == slno).ToList();
                    int firstRow = 0;
                    foreach(DataRow row in dataRows)
                    {
                        TraceabilityEntity data = new TraceabilityEntity();
                        if (firstRow == 0)
                        {
                            data.RowSpan = dataRows.Count.ToString();
                        }
                        else
                        {
                            data.RowSpan = "0";
                        }
                        data.Plant = row["PlantID"].ToString();
                        data.QRCode = row["Barcode"].ToString();
                        data.Machine = row["MachineID"].ToString();
                        data.ModelName = row["Model"].ToString();
                        data.StartTime = row["CycleStart"].ToString();
                        data.EndTime = row["CycleEnd"].ToString();
                        data.ElapsedTime = row["ElapsedTime"].ToString();
                        data.Value = row["RPMValue"].ToString();
                        data.Result = row["Result"].ToString();
                        firstRow++;
                        list.Add(data);
                    }
                }
                lvSlnoData.DataSource = list;
                lvSlnoData.DataBind();
                Session["TraceabilityData"] = list;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindData = " + ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                List<TraceabilityEntity> list = new List<TraceabilityEntity>();
                if (Session["TraceabilityData"] == null)
                {
                    return;
                }
                list = Session["TraceabilityData"] as List<TraceabilityEntity>;
                if (list.Count > 0)
                {
                    bool successfull = AdvikGenerateReports.GenerateTraceabilityReport(ddlPlant.SelectedValue, ddlMachine.SelectedValue, txtFromDate.Text, txtToDate.Text, txtSlNoSearch.Text, hdnViewType.Value, list);
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
                List<TraceabilityEntity> list = new List<TraceabilityEntity>();
                lvSlnoData.DataSource = list;
                lvSlnoData.DataBind();
                Session["TraceabilityData"] = null;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}