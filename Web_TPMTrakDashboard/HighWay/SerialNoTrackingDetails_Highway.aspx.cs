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
    public partial class SerialNoTrackingDetails_Highway : System.Web.UI.Page
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
                DataTable dt = DBAccess.GetSerialNoTrackingDashboardData(ddlPlantID.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlPlantID.SelectedValue.ToString(), ddlCell.SelectedValue.ToString().Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "" : ddlCell.SelectedValue.ToString(), Component, txtSerialNo.Text, txtFromDate.Text, txtFromDate.Text);
                List<SerialNoTrackingDashboardData> list = new List<SerialNoTrackingDashboardData>();
                SerialNoTrackingDashboardData data = null;
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    SerialNoTrackingDashboardData HeaderData = new SerialNoTrackingDashboardData()
                    {
                        HeaderEnable = true,
                    };
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        data = new SerialNoTrackingDashboardData();
                        data.SerialNo = dtRow["SerialNumber"].ToString();
                        data.ComponentID = dtRow["ComponentId"].ToString();
                        data.ContentEnable = true;
                        List<SerialNoData> componentDatalist = new List<SerialNoData>();
                        List<SerialNoData> componentDatalistHeader = new List<SerialNoData>();
                        for (int k = 2; k < dt.Columns.Count; k++)
                        {
                            SerialNoData componentData = new SerialNoData();
                            var opndata = dt.Columns[k].ColumnName.Split('-');
                            SerialNoData componentDataHeader = new SerialNoData()
                            {
                                HeaderEnable = true,
                                OpnNo= "Operation No:" + opndata[0],
                                OperationDesc= opndata[1],
                                //OperationNo = "Operation No:" + dt.Columns[k].ColumnName,
                            };
                            componentDatalistHeader.Add(componentDataHeader);
                            componentData.ContentEnable = true;
                            if (dtRow[dt.Columns[k].ColumnName].ToString() != "")
                            {
                                componentData.OperationData = "glyphicon glyphicon-ok";
                                componentData.OperationDataBackcolor = "green";
                                componentData.OperationDataBackgroundcolor = "#0cab0c";
                            }
                            else
                            {
                                componentData.OperationData = "glyphicon glyphicon-remove";
                                componentData.OperationDataBackcolor = "red";
                                componentData.OperationDataBackgroundcolor = "red";
                            }
                            //componentData.OperationData =dtRow[dt.Columns[k].ColumnName].ToString();
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
                foreach(ListItem item in lbComponent.Items)
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
                //List<string> list = DBAccess.GetSerialNo(lbComponent.SelectedValue.ToString());
                //lbSerialNo.DataSource = list;
                //lbSerialNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCell();
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lbComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSerialNo();
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
    }
}