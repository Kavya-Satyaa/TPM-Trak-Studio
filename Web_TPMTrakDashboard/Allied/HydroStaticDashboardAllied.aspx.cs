using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard;

namespace Web_TPMTrakDashboard.Allied
{
    public partial class HydroStaticDashboardAllied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["HSData"] = null;
                ddlLineID.DataSource = DataBaseAccess.GetAllPlants();
                ddlLineID.DataBind();

                ddlGroupID.DataSource = DataBaseAccess.GetAllGroupId(ddlLineID.SelectedItem.Value.ToString());
                ddlGroupID.DataBind();

                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindDashboardData();
                //BindDashboardData(ddlLineID.SelectedItem.Value.ToString(), ddlGroupID.SelectedItem.Value.ToString(), "", DateTime.Now.ToString("yyyy-"));
            }
        }

        private void BindDashboardData()
        {
            try
            {

                string LineID = ddlLineID.SelectedItem.Value.ToString();
                string GroupID = ddlGroupID.SelectedItem.Value.ToString();
                if (LineID == "All")
                {
                    LineID = "";
                }
                if (GroupID == "All")
                {
                    GroupID = "";
                }
                string Date = txtDate.Text;
                string UinNO = txtUinNo.Text;

                DataTable dt = new DataTable();
                if (UinNO != "")
                {
                    dt = AlliedDBAccess.GetHydrostaticDashboardData_Allied(LineID, GroupID, UinNO, "");
                }
                else if (Date != "")
                {
                    dt = AlliedDBAccess.GetHydrostaticDashboardData_Allied(LineID, GroupID, "", Date);
                }
                else
                {
                    HelperClassGeneric.openWarningModal(this, "Select UIN Number or Date");
                    return;
                }

                var UINNo_ComP_Dist = dt.AsEnumerable().Select(x => new { UinNo = x["UinNo"].ToString(), ComponentID = x["ComponentID"].ToString() }).Distinct().ToList();

                var OperationNo = dt.AsEnumerable().Select(x => Convert.ToInt32(x["OperationNo"].ToString())).Distinct().ToList();
                OperationNo.Sort();
                List<UINDashboardData> list = new List<UINDashboardData>();
                UINDashboardData HeaderData = new UINDashboardData()
                {
                    HeaderEnable = true,
                };
                int i = 0;
                foreach (var uinNo_ComP in UINNo_ComP_Dist)
                {
                    UINDashboardData data = new UINDashboardData();
                    data.UinNo = uinNo_ComP.UinNo;
                    data.ComponentID = uinNo_ComP.ComponentID;
                    data.ContentEnable = true;

                    List<UINMachineData> machineDatalist = new List<UINMachineData>();
                    List<UINMachineData> machineDatalistHeader = new List<UINMachineData>();

                    foreach (var operation in OperationNo)
                    {
                        UINMachineData machineData = new UINMachineData();
                        UINMachineData machineDataHeader = new UINMachineData()
                        {
                            HeaderEnable = true,
                            OperationNo = "Operation No:" + operation,
                        };
                        machineDatalistHeader.Add(machineDataHeader);

                        var row = dt.AsEnumerable().Where(x => x["UinNo"].ToString() == data.UinNo && x["ComponentID"].ToString() == data.ComponentID && x["OperationNo"].ToString() == operation.ToString()).FirstOrDefault();
                        machineData.ContentEnable = true;
                        if (row != null)
                        {
                            machineData.OperationNo = row["OPerationNo"].ToString();
                            machineData.MachineID = row["MachineID"].ToString();
                            machineData.OperatorName = row["OPeratorName"].ToString();
                            //machineData.StartTime = row["StartTime"].ToString();
                            //machineData.EndTime = row["EndTime"].ToString();
                            if (row["StartTime"].ToString() != "")
                                machineData.StartTime = Convert.ToDateTime(row["StartTime"]).ToString("dd-MM-yyyy HH:mm:ss");
                            else
                                machineData.StartTime = row["StartTime"].ToString();
                            if (row["EndTime"].ToString() != "")
                                machineData.EndTime = Convert.ToDateTime(row["EndTime"]).ToString("dd-MM-yyyy HH:mm:ss");
                            else
                                machineData.EndTime = row["EndTime"].ToString();
                        }
                        machineDatalist.Add(machineData);
                    }
                    HeaderData.machineData = machineDatalistHeader;

                    data.machineData = machineDatalist;
                    if (i == 0)
                    {
                        list.Add(HeaderData);
                    }
                    list.Add(data);
                    i++;
                }


                lvHSDashboard.DataSource = list;
                lvHSDashboard.DataBind();

                Session["HSData"] = list;

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnsearch2_Click(object sender, EventArgs e)
        {
            BindDashboardData();
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            txtUinNo.Text = string.Empty;
            BindDashboardData();
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            string generated = string.Empty;
            if (Session["HSData"] != null)
            {
                List<UINDashboardData> list = (List<UINDashboardData>)Session["HSData"];
                TMPTrakGenerateReport.UINTrackingReport(list, out generated);
                if (generated.Equals("Not Found", StringComparison.OrdinalIgnoreCase) || generated.Equals("Error", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openWarningModal(this, "Error Generating Report");
                }
            }
            else
            {
                HelperClassGeneric.openWarningModal(this, "No Data Found");
            }
        }

        protected void ddlLineID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlGroupID.DataSource = DataBaseAccess.GetAllGroupId(ddlLineID.SelectedItem.Value.ToString());
            ddlGroupID.DataBind();
        }
    }
}