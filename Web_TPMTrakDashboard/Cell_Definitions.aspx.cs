using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using System.Data;
using System.Data.SqlClient;

namespace Web_TPMTrakDashboard
{
    public partial class Cell_Definitions : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"data source=AMIT-DEV01\SQL2017_DEV; database= TPM_Shanti; user id=sa; password=pctadmin$123");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantID();
                ddlLines_SelectedIndexChanged(null, EventArgs.Empty);
                ddlGroupIds_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        private void BindPlantID()
        {
            if (Session["UserName"] != null)
            {
                List<string> lines = DataBaseAccess.GetAllPlantsForPlantInfo(Session["UserName"].ToString());
                if (lines != null && lines.Count > 0)
                {
                    ddlLines.DataSource = lines;
                    ddlLines.DataBind();
                    ddlgrouplines.DataSource = lines;
                    ddlgrouplines.DataBind();
                }
            }
        }

        private void BindGroupId()
        {
            try
            {
                List<string> strlst = null;
                if (ddlLines.SelectedValue != null)
                    strlst = BindCockpitView.ViewCellsToDisplay(ddlLines.SelectedValue.ToString());
                else
                    strlst = BindCockpitView.ViewCellsToDisplay(ddlLines.Text.ToString());
                ddlGroupIds.DataSource = strlst;
                ddlGroupIds.DataBind();
                if (strlst.Count == 0)
                    ddlGroupIds.Text = "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindGroupOrder()
        {
            try
            {
                string str = "";
                if (ddlLines.SelectedValue != null && ddlGroupIds.SelectedValue != null)
                    str = DataBaseAccess.GetGroupOrder(ddlLines.SelectedValue.ToString(), ddlGroupIds.SelectedValue.ToString());
                else
                    str = DataBaseAccess.GetGroupOrder(ddlLines.Text.ToString(), ddlGroupIds.Text.ToString());

                if (str != null)
                    txtGroupOrder.Text = str;
                else
                {
                    if (ddlGroupIds.Text.Equals(""))
                    {
                        txtGroupOrder.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindEOLMachine()
        {
            if (ddlLines.SelectedItem == null) return;
            List<string> assignedMachineIDs = DataBaseAccess.GetAssignedAndUnAssignedMachineIds("select distinct MachineID from PlantMachine where PlantID = @PlantId", ddlLines.SelectedItem.ToString());
            ddlEndOfLine.DataSource = assignedMachineIDs;
            ddlEndOfLine.DataBind();
            string ret = DataBaseAccess.GetEOLMachine(ddlLines.Text);
            if (!ret.Equals(""))
            {
                ddlEndOfLine.SelectedValue = ret;
            }
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGroupId();
            BindEOLMachine();
            ddlGroupIds_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlGroupIds_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGroupOrder();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<LineGroupModel> GetCellDefinations(string Line, string Group)
        {
            return DataBaseAccess.GetLineGroupMachineInfo(Line, Group);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBindSortOrder(string Line, string Group)
        {
            return DataBaseAccess.GetGroupOrder(Line, Group);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetGroupIds(string Line)
        {
            List<string> Cellvalue = new List<string>();
            Cellvalue = BindCockpitView.ViewCellsToDisplay(Line);
            return Cellvalue;
        }

        #region "Save"
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveCellDefinations(LineGroupModel model)
        {
            string msg = string.Empty;
            bool rowAdded = false;
            try
            {
                if (!string.IsNullOrEmpty(model.endOfMachineVal) && !string.IsNullOrEmpty(model.selectedLine))
                {
                    DataBaseAccess.SetEOLMachine(model.selectedLine, model.endOfMachineVal);
                }
                int recordsDeleted = 0;
                foreach (var item in model.ListCellDefination)
                {
                    if (item.chkAssignMachine)
                    {
                        if (recordsDeleted < 1)
                        {
                            DataBaseAccess.DeleteLineGroupAssignInfo(model.selectedLine, model.selectedGroupId);
                        }
                        int delete = DataBaseAccess.AddLineGroupInfo(model.selectedLine, model.selectedGroupId, model.groupOrder, item.MachineId, item.EndOfGroupMachine, item.MachineSequence, model.selectedGroupId, "0");
                        rowAdded = true;
                        recordsDeleted++;
                    }
                }
                if (rowAdded)
                {
                    msg = "Records updated successfully.";
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Logger.WriteErrorLog(ex.ToString());
                msg = ex.Message;
            }
            return msg;
        }
        #endregion
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static void DeleteGroupID1(string Line,string Group)
        //{
        //    DataBaseAccess.DeleteGrpId1(Line, Group);
        //}


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void DeleteCellDefination(string Line, string Group)
        {
            DataBaseAccess.DeleteLineGroupAssignInfo(Line, Group);
        }

        protected void btnSave1_Click(object sender, EventArgs e)
        {
            bool savedata = false;
            if (ddlgrouplines != null && txtgroupid != null)
            {
                if (!string.IsNullOrEmpty(txtgroupid.Text))
                {
                    try
                    {
                        bool check = DataBaseAccess.Checkgrouidpresent(ddlgrouplines.SelectedValue.ToString(),txtgroupid.Text);
                        if (!check)
                        {
                            savedata = DataBaseAccess.Savedata(ddlgrouplines.SelectedValue.ToString(), txtgroupid.Text);
                            if (savedata)
                            {
                                lblMessages.Text = "GroupID saved";
                                BindGroupId();
                                txtgroupid.Text = "";
                            }
                            else
                                lblMessages.Text = "Group ID didnot saved please try again";
                        }
                        else
                            lblMessages.Text = "Group ID name is already assigned for other plant";
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog(ex.Message);
                        throw;
                    }
                }
                else
                    lblMessages.Text = "Please add Group-ID";
            }
        }


        protected void btngrpdelete_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myConfirmationModal", "$('#myConfirmationModal').modal();", true);
            //ddlGroupIds.DataTextField = "GroupID";
            //ddlGroupIds.DataValueField = "GroupID";
            //ddlGroupIds.DataSource = result;
            //ddlGroupIds.DataBind();

        }

        protected void saveConfirmYes_ServerClick(object sender, EventArgs e)
        {

            List<string> result = new List<string>();
            result = DataBaseAccess.DeleteGrpId1(ddlLines.Text, ddlGroupIds.Text);

            BindGroupId();
            txtgroupid.Text = "";


        }



    }
}