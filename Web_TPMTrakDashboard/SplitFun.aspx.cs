using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SplitFun : System.Web.UI.Page
    {
        string MachineID = "", Component = "", Operation = "", Operator = "", DownID = "", Type = "";
        DateTime Sttime = DateTime.Now, ndtime = DateTime.Now;
        new int ID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["MachineID"]) && !string.IsNullOrEmpty(Request.QueryString["ComponentID"]) && !string.IsNullOrEmpty(Request.QueryString["Operation"]) && !string.IsNullOrEmpty(Request.QueryString["Operator"]) && !string.IsNullOrEmpty(Request.QueryString["DownID"])
                    && !string.IsNullOrEmpty(Request.QueryString["sttime"]) && !string.IsNullOrEmpty(Request.QueryString["ndtime"]) && !string.IsNullOrEmpty(Request.QueryString["Type"]) && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    lblMachineID.Text = MachineID = Request.QueryString["MachineID"].ToString();
                    lblComponentID.Text = Component = Request.QueryString["ComponentID"].ToString();
                    lblOperationNumber.Text = Operation = Request.QueryString["Operation"].ToString();
                    lblOperator.Text = Operator = Request.QueryString["Operator"].ToString();
                    lblDownID.Text = DownID = Request.QueryString["DownID"].ToString();
                    DateTime.TryParse(Request.QueryString["sttime"].ToString(), out Sttime);
                    lblDownTimeStart.Text = Request.QueryString["sttime"].ToString();
                    DateTime.TryParse(Request.QueryString["ndtime"].ToString(), out ndtime);
                    Type = Request.QueryString["Type"].ToString();
                    lblDownEndTime.Text = Request.QueryString["ndtime"].ToString();
                    txtBreakDate.Text = Sttime.ToString("yyyy-MM-dd");
                    txtBreaktime.Text = Sttime.ToString("HH:mm:ss");
                    Session["Type"] = Type;
                    int.TryParse(Request.QueryString["ID"].ToString(), out ID);
                    Session["ID"] = ID;
                }
            }
        }

        protected void btnSplit_Click(object sender, EventArgs e)
        {
            DateTime SplitDate = DateTime.Now;
            DateTime ddate = DateTime.Now;
            try
            {
                DataTable dt = Session["DownDataSplitFun"] as DataTable;
                int.TryParse(Session["ID"].ToString(), out ID);
                if (Session["Type"].ToString().Equals("ModifiedData", StringComparison.OrdinalIgnoreCase))
                {
                    dt = DataBaseAccess.GetModifiedandJobDate(ID, "ModifiedData");
                }
                else if (Session["Type"].ToString().Equals("JobCard", StringComparison.OrdinalIgnoreCase))
                {
                    dt = DataBaseAccess.GetModifiedandJobDate(ID, "JobCard");
                }
                if (ID != 0 && dt != null && dt.Rows.Count > 0)
                {
                    bool success = false;
                    string splitdate = txtBreakDate.Text + " " + txtBreaktime.Text;
                    DateTime.TryParse(splitdate, out SplitDate);
                    DateTime.TryParse(lblDownTimeStart.Text, out Sttime);

                    DateTime.TryParse(lblDownEndTime.Text, out ndtime);
                    if (SplitDate <= Sttime)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Split Date Cannot Be Less Than Down Time Start')", true);
                        //Response.Write("<script>alert('Split Date Cannot Be Less Than Down Time Start');</script>");
                    }
                    else if (SplitDate >= ndtime)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Split Date Cannot Be Greater Than Down End Start')", true);
                        //Response.Write("<script>alert('Split Date Cannot Be Greater Than Down End Start');</script>");
                    }
                    else
                    {
                        if (Session["Type"] != null)
                        {
                            Type = Session["Type"].ToString();
                            if (Type.Equals("ModifiedData", StringComparison.OrdinalIgnoreCase))
                            {
                                success = DataBaseAccess.UpdateModifiedSplitData(ID, SplitDate);
                                if (success)
                                {
                                    success = DataBaseAccess.InsertModifiedSplitDate(SplitDate, ndtime, dt.Rows[0]);
                                    if (success)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Split and Inserted Successfully')", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Try Again !!')", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Try Again!!')", true);
                                }
                            }
                            else if (Type.Equals("JobCard", StringComparison.OrdinalIgnoreCase))
                            {

                                string DownTime = (SplitDate - Sttime).TotalSeconds.ToString();
                                success = DataBaseAccess.UpdateJobCard(ID, DownTime, Sttime, SplitDate, Session["UserName"].ToString());
                                if (success)
                                {
                                    DownTime = (ndtime - SplitDate).TotalSeconds.ToString();
                                    success = DataBaseAccess.InsertJobCard(SplitDate, ndtime, DownTime, Session["UserName"].ToString(), DateTime.Now, dt.Rows[0]);
                                    if (success)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Split and Inserted Successfully')", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Try Again !!')", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Try Again!!')", true);
                                }
                            }
                        }
                    }

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