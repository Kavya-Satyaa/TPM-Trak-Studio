using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.TAFE;

namespace Web_TPMTrakDashboard
{
    public partial class LineMeterMasterPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                txtMonth.Text = DateTime.Now.Month.ToString("00");
                txtYear.Text = DateTime.Now.Year.ToString("0000");
                BindLineID();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            try
            {
                if (ddlLineID.SelectedValue != null)
                {
                    string date = txtYear.Text + "-" + txtMonth.Text + "-01";
                    DateTime NewDate = Util.GetDateTime(date);
                    date = TafeDataBaseAccess.Gellogicalmonthstart(NewDate);
                    List<LineMeterEntity> dt = TafeDataBaseAccess.BindLineMeterGrid(ddlLineID.SelectedValue.ToString(), Convert.ToDateTime(date));
                    if (dt != null && dt.Count > 0)
                    {
                        
                        LineMeterMaster.DataSource = dt;
                        LineMeterMaster.DataBind();
                    }
                    else
                    {
                        LineMeterMaster.DataSource = new List<LineMeterEntity>();
                        LineMeterMaster.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageNodata", "messageNodata();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void BindLineID()
        {
            try
            {
                List<string> LineID = TafeDataBaseAccess.GetLineIDsForPlant("");
                if (LineID != null && LineID.Count > 0)
                {
                    ddlLineID.DataSource = LineID;
                    ddlLineID.DataBind();
                }
                else
                {
                    ddlLineID.DataSource = new List<string>();
                    ddlLineID.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool saved = false;
                foreach (GridViewRow Row in LineMeterMaster.Rows)
                {
                    string Datetime = (Row.FindControl("lblDate") as Label).Text;
                    string Line = (Row.FindControl("lblLine") as Label).Text;
                    string Hid1 = (Row.FindControl("hiddenfield1") as HiddenField).Value;
                    string txtManPower = (Row.FindControl("txtManPower") as TextBox).Text;
                    string Hid2 = (Row.FindControl("hiddenfield2") as HiddenField).Value;
                    string txtLoadingHours = (Row.FindControl("txtLoadingHours") as TextBox).Text;
                    if(string.IsNullOrEmpty(txtLoadingHours) || string.IsNullOrEmpty(txtManPower))
                    {
                        if(string.IsNullOrEmpty(txtLoadingHours) && !(string.IsNullOrEmpty(txtManPower)))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk('Loading HoursCannot be empty!!!');", true);
                            return;
                        }
                        if (!(string.IsNullOrEmpty(txtLoadingHours)) && (string.IsNullOrEmpty(txtManPower)))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk('Man Power Cannot be empty!!!');", true);
                            return;
                        }

                    }
                    //else if((txtLoadingHours.Equals("0",StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk('Loading Hours Cannot be ZERO!!!');", true);
                    //    return;
                    //}
                    //else if ((txtManPower.Equals("0", StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk('Man Power Cannot be ZERO!!!');", true);
                    //    return;
                    //}
                    else if (!((txtManPower.Equals(Hid1, StringComparison.OrdinalIgnoreCase) && txtLoadingHours.Equals(Hid2, StringComparison.OrdinalIgnoreCase))))
                    {
                        DateTime date = Util.GetDateTime(Datetime);
                        saved = TafeDataBaseAccess.SaveLineMeterValue(Line, txtManPower, txtLoadingHours, date);
                    }
                }
                if (saved)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk('Saved Successfully!!!');", true);
                    BindGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "messageOk", "messageOk('No data changed to SAVE!!!');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
    public class LineMeterEntity
    {
        public DateTime DStart { get; set; }
        public string Line { get; set; }
        public string Loadinghrs { get; set; }
        public string Manpower { get; set; }
    }
}