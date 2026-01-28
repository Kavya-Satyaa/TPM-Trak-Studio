using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.LnTOdisha.Model;
using Web_TPMTrakDashboard.Models;
using static Web_TPMTrakDashboard.LnTOdisha.Model.LnTOdishaDTO;

namespace Web_TPMTrakDashboard.LnTOdisha
{
    public partial class PMChecklistPrintOutLnT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMachine();
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MM");
                BindData();
            }
        }
        private void BindMachine()
        {
            try
            {
                List<string> list = LnTOdishaDBAccess.GetMachineInfoForPM();
                ddlMachine.DataSource = list;
                ddlMachine.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindData()
        {   
            DataTable dt = new DataTable();
            DataTable dt_HeaderDetails = new DataTable();
            int j = 0;
            try
            {
                btnPrint.Visible = false;
                 dt = LnTOdishaDBAccess.getChecklistTransacionPrintout(ddlMachine.SelectedValue, txtYear.Text, txtMonth.Text, out dt_HeaderDetails);
               
                List<PMCheckilistPrintOutEntity> list = new List<PMCheckilistPrintOutEntity>();
                string tempCategory = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PMCheckilistPrintOutEntity data = new PMCheckilistPrintOutEntity();
                    var row = dt.Rows[i];
                    if (tempCategory != row["Category"].ToString())
                    {
                        data.CategoryVisible = true;
                        data.Category = row["Category"].ToString();
                        list.Add(data);
                    }
                    tempCategory = row["Category"].ToString();

                    data = new PMCheckilistPrintOutEntity();
                    data.ActivityVisible = true;
                    data.SlNo = (++j).ToString(); //row["SINO"].ToString();
                    data.ActivityID = row["ActivityID"].ToString(); //(++j).ToString();
                    data.Activity = row["ActivityName"].ToString();
                    data.Frequency = row["Frequency"].ToString();
                    data.AllotedTime = row["AllotedTime"].ToString();
                    data.LastCheckedRemark = row["LastChecked"].ToString();
                    data.TodayRemark = "";
                    list.Add(data);
                }
                lvChecklist.DataSource = list;
                lvChecklist.DataBind();
                if (dt_HeaderDetails.Rows.Count > 0 && list.Count>0)
                {
                    var firstRow = dt_HeaderDetails.Rows[dt_HeaderDetails.Rows.Count-1];

                    if (firstRow["TeamLeader"].ToString() != "")
                        btnPrint.Visible = true;
                    (lvChecklist.FindControl("lblMachineID") as Label).Text = firstRow["MachineID"].ToString();
                    (lvChecklist.FindControl("lblMachineNumber") as Label).Text = "M/C No:" + firstRow["MachineDescription"].ToString();
                    (lvChecklist.FindControl("lblLastCheckedDate") as Label).Text = string.IsNullOrEmpty(firstRow["LastCheckedDate"].ToString())  ? "" : Util.GetDateTime(firstRow["LastCheckedDate"].ToString()).ToString("dd-MM-yyyy");
                    (lvChecklist.FindControl("lblLastCheckedTLEntry") as Label).Text = firstRow["LastCheckedTeamLeader"].ToString();
                    (lvChecklist.FindControl("lblLastCheckedCrew") as Label).Text = firstRow["LastCheckedCrewMemberName"].ToString();

                    var xx = firstRow["scheduledDate"].ToString();
                    (lvChecklist.FindControl("lblScheduleDate") as Label).Text = (string.IsNullOrEmpty(firstRow["scheduledDate"].ToString()) || firstRow["status"].ToString().Equals("NoPlan", StringComparison.OrdinalIgnoreCase)) ? "" : Util.GetDateTime(firstRow["scheduledDate"].ToString()).ToString("dd-MM-yyyy");
                    (lvChecklist.FindControl("txtTeamLeaderEntry") as TextBox).Text = firstRow["TeamLeader"].ToString();
                    (lvChecklist.FindControl("txtCrew") as TextBox).Text = firstRow["CrewMemberName"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
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
                string scheduleDate = (lvChecklist.FindControl("lblScheduleDate") as Label).Text;
                string tlEntry = (lvChecklist.FindControl("txtTeamLeaderEntry") as TextBox).Text;
                string crewData = (lvChecklist.FindControl("txtCrew") as TextBox).Text;
                string result = LnTOdishaDBAccess.insertChecklistTransacionPrintout(ddlMachine.SelectedValue, txtYear.Text, txtMonth.Text, scheduleDate, tlEntry, crewData);
                if (result.Equals("Updated", StringComparison.OrdinalIgnoreCase) || result.Equals("Inserted", StringComparison.OrdinalIgnoreCase))
                {
                    HelperClassGeneric.openInsertSuccessModal(this);
                }
                else
                {
                    HelperClassGeneric.openInsertErrorModal(this);
                }
                BindData();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}