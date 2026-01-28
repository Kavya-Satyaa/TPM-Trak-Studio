using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.HighWay
{
    public partial class InspectionTracking_Highway : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlant();
                BindCell();
                BindComponent(); BindSerialNo();
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                string Component = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbComponent);
                string serialNo = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbSerialNo);
                DataTable dt = DBAccess.GetTrackingDashboardData(ddlPlantID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), Component, serialNo, txtFromDate.Text, txtToDate.Text);
                //DataTable dt = DBAccess.GetTrackingDashboardData(ddlPlantID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), Component, "", txtFromDate.Text, txtToDate.Text);
                List<TrackingDashboardData> list = new List<TrackingDashboardData>();
                TrackingDashboardData data = null;
                if (dt.Rows.Count > 0)
                {
                    var distSerialNo = dt.AsEnumerable().Select(x => x.Field<string>("SerialNo")).Distinct().ToList();
                    int i = 0;
                    for (int s = 0; s < distSerialNo.Count; s++)
                    {
                        var distComp = dt.AsEnumerable().Where(x => x["SerialNo"].ToString().Equals(distSerialNo[s], StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<string>("ComponentId")).Distinct().ToList();
                        TrackingDashboardData HeaderData = new TrackingDashboardData()
                        {
                            HeaderEnable = true,
                        };
                        for (int c = 0; c < distComp.Count; c++)
                        {
                            data = new TrackingDashboardData();
                            data.SerialNo = distSerialNo[s].ToString() == null ? "" : distSerialNo[s].ToString();
                            data.ComponentID = Convert.IsDBNull(distComp[c].ToString()) ? "" : distComp[c].ToString();
                            data.ContentEnable = true;
                            var distOpn = dt.AsEnumerable().Where(x => x["SerialNo"].ToString().Equals(distSerialNo[s], StringComparison.OrdinalIgnoreCase) && x["ComponentId"].ToString().Equals(distComp[c], StringComparison.OrdinalIgnoreCase)).Select(x => x.Field<dynamic>("OperationNo")).Distinct().ToList();
                            List<ComponentData> componentDatalist = new List<ComponentData>();
                            List<ComponentData> componentDatalistHeader = new List<ComponentData>();
                            for (int o = 0; o < distOpn.Count; o++)
                            {
                                ComponentData componentData = new ComponentData();
                                ComponentData componentDataHeader = new ComponentData()
                                {
                                    HeaderEnable = true,
                                    OperationNo = "Operation No:" + distOpn[o],
                                };
                                componentDatalistHeader.Add(componentDataHeader);

                                //var row = dt.AsEnumerable().Where(x => x["SerialNo"].ToString().Equals(distSerialNo[s].ToString(), StringComparison.OrdinalIgnoreCase) && x["ComponentId"].ToString().Equals(distComp[c].ToString(), StringComparison.OrdinalIgnoreCase) && x["OperationNo"]==(Convert.ToInt32(distOpn[o]))).FirstOrDefault();
                                var row = dt.AsEnumerable().Where(x => (x["SerialNo"].ToString().Equals(distSerialNo[s].ToString(), StringComparison.OrdinalIgnoreCase) || distSerialNo[s] == null) &&
        (x["ComponentId"].ToString().Equals(distComp[c].ToString(), StringComparison.OrdinalIgnoreCase) || distComp[c] == null || distComp[c].ToString() == "") && (x["OperationNo"] == (distOpn[o]) || distOpn[o] == null)).FirstOrDefault();

                                componentData.ContentEnable = true;
                                if (row != null)
                                {
                                    componentData.OperationNo = row["OperationNo"].ToString();
                                    componentData.Machine = row["MachineId"].ToString();
                                    componentData.Operator = row["Operator"].ToString();
                                    if (row["ScannedTS"].ToString() != "")
                                        componentData.ScannedDateTime = Convert.ToDateTime(row["ScannedTS"]).ToString("dd-MM-yyyy HH:mm:ss");
                                    else
                                        componentData.ScannedDateTime = row["ScannedTS"].ToString();
                                    if (row["ndtime"].ToString() != "")
                                        componentData.CycleEndTime = Convert.ToDateTime(row["ndtime"]).ToString("dd-MM-yyyy HH:mm:ss");
                                    else
                                        componentData.CycleEndTime = row["ndtime"].ToString();
                                }
                                else
                                {
                                    componentData.OperationNo = "";
                                    componentData.Machine = "";
                                    componentData.Operator = "";
                                    componentData.ScannedDateTime = "";
                                    componentData.CycleEndTime = "";
                                }
                                componentDatalist.Add(componentData);
                            }
                            HeaderData.componentData = componentDatalistHeader;

                            data.componentData = componentDatalist;
                            if (i == 0)
                            {
                                list.Add(HeaderData);
                            }
                            list.Add(data);
                            i++;
                        }
                    }
                    
                }
                else
                {
                    HelperClassGeneric.openWarningToastrModal(this, "No data found");
                }
                lvGrid.DataSource = list;
                lvGrid.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindPlant()
        {
            try
            {
                List<string> list = DBAccess.GetPlantIDs();
                ddlPlantID.DataSource = list;
                ddlPlantID.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindCell()
        {
            try
            {
                List<string> list = DBAccess.GetCellIds(ddlPlantID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString());
                ddlCell.DataSource = list;
                ddlCell.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindComponent()
        {
            try
            {
                List<string> list = DBAccess.GetComponents();
                lbComponent.DataSource = list;
                lbComponent.DataBind();
                foreach (ListItem item in lbComponent.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindSerialNo()
        {
            try
            {

                string Component = "";
                if (lbComponent.SelectedItem != null)
                {
                    Component = HelperClassGeneric.getListBoxValueWithoutSingleQuotes(lbComponent);
                }
                else
                {
                    Component = "";
                }
                DateTime fromdate = Util.GetDateTime(txtFromDate.Text);
                DateTime toDate = Util.GetDateTime(txtToDate.Text);
                List<string> list = DBAccess.GetSerialNo(Component,fromdate,toDate);
                lbSerialNo.DataSource = list;
                lbSerialNo.DataBind();
                foreach (ListItem item in lbSerialNo.Items)
                {
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var days = Util.GetDateTime(txtToDate.Text.Trim()) - Util.GetDateTime(txtFromDate.Text.Trim());
            if (days.TotalDays > 7)
            {
                HelperClassGeneric.openWarningToastrModal(this, "From and To Date difference cannot be more than 7 days");
                return;
            }
            BindGrid();
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lbComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSerialNo();
        }

        protected void lbSerialNo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCell();
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            BindSerialNo();
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            BindSerialNo();
        }
    }
}