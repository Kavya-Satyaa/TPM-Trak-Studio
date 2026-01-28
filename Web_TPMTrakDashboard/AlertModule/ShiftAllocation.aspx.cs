using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class ShiftAllocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(IsPostBack))
            {
                if (Session["Language"] == null || Session["connectionString"] == null)
                    Response.Redirect("~/SignIn.aspx", false);
                else
                {
                    SessionClear.ClearSession();
                    txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    BindPlantID();
                    BindOperator();
                    BindGrid();
                }
            }
        }

        private void BindOperator()
        {
            try
            {
                List<string> List = DataBaseAccess.GetEmployeeDetails("");
                if (cmbPlantID != null)
                {
                    cmbOperator.DataSource = List;
                    cmbOperator.DataBind();
                    cmbOperator.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> List = DataBaseAccess.GetPlantDetails();
                if (cmbPlantID != null)
                {
                    cmbPlantID.DataSource = List;
                    cmbPlantID.DataBind();
                    cmbPlantID.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void BindGrid()
        {
            DateTime FromDate = Util.GetDateTime(txtFromDate.Text);
            DateTime Todate = FromDate.AddDays(7);
            List<string> Shift = DataBaseAccess.shiftdetail();
            DataTable dt = DataBaseAccess.getAlertShiftAllocation(cmbPlantID.Text.ToString(), FromDate, "View");
            if (dt != null && dt.Rows.Count > 0)
            {
                List<ShiftAllocationEntity> list = GetList(dt, Shift, FromDate, Todate);
                gridOperatorMachine.DataSource = list;
                gridOperatorMachine.DataBind();
            }
            else
            {
                List<ShiftAllocationEntity> list = new List<ShiftAllocationEntity>();
                gridOperatorMachine.DataSource = list;
                gridOperatorMachine.DataBind();
            }
        }

        private List<ShiftAllocationEntity> GetList(DataTable data, List<string> shift, DateTime Fromdate, DateTime Todate)
        {
            List<ShiftAllocationEntity> entitydata = new List<ShiftAllocationEntity>();
            DateTime fromdate = Fromdate;
            List<DateTime> dates = new List<DateTime>();
            while (!(fromdate == Todate))
            {
                dates.Add(fromdate); fromdate = fromdate.AddDays(1);
            }

            for (int i = 0; i < data.Rows.Count; i++)
            {
                ShiftAllocationEntity entity = new ShiftAllocationEntity();
                entity.SLNO = i + 1;
                entity.Consumer = data.Rows[i][0].ToString();
                entity.EmailId = data.Rows[i][1].ToString();
                entity.PlantID = cmbPlantID.SelectedValue.ToString(); 
                entity.PhoneNo = data.Rows[i][2].ToString();
                entity.date1 = dates[0].ToString("yyyy-MM-dd");
                entity.shift1 = shift[0];
                entity.shift2 = shift[1];
                entity.shift3 = shift[2];
                entity.chkdate1shift1 = data.Rows[i][3].ToString() == "0" ? false : true;
                entity.chkdate1shift2 = data.Rows[i][4].ToString() == "0" ? false : true;
                entity.chkdate1shift3 = data.Rows[i][5].ToString() == "0" ? false : true;
                entity.date2 = dates[1].ToString("yyyy-MM-dd");
                entity.chkdate2shift1 = data.Rows[i][6].ToString() == "0" ? false : true;
                entity.chkdate2shift2 = data.Rows[i][7].ToString() == "0" ? false : true;
                entity.chkdate2shift3 = data.Rows[i][8].ToString() == "0" ? false : true;
                entity.date3 = dates[2].ToString("yyyy-MM-dd");
                entity.chkdate3shift1 = data.Rows[i][9].ToString() == "0" ? false : true;
                entity.chkdate3shift2 = data.Rows[i][10].ToString() == "0" ? false : true;
                entity.chkdate3shift3 = data.Rows[i][11].ToString() == "0" ? false : true;
                entity.date4 = dates[3].ToString("yyyy-MM-dd");
                entity.chkdate4shift1 = data.Rows[i][12].ToString() == "0" ? false : true;
                entity.chkdate4shift2 = data.Rows[i][13].ToString() == "0" ? false : true;
                entity.chkdate4shift3 = data.Rows[i][14].ToString() == "0" ? false : true;
                entity.date5 = dates[4].ToString("yyyy-MM-dd");
                entity.chkdate5shift1 = data.Rows[i][15].ToString() == "0" ? false : true;
                entity.chkdate5shift2 = data.Rows[i][16].ToString() == "0" ? false : true;
                entity.chkdate5shift3 = data.Rows[i][17].ToString() == "0" ? false : true;
                entity.date6 = dates[5].ToString("yyyy-MM-dd");
                entity.chkdate6shift1 = data.Rows[i][18].ToString() == "0" ? false : true;
                entity.chkdate6shift2 = data.Rows[i][19].ToString() == "0" ? false : true;
                entity.chkdate6shift3 = data.Rows[i][20].ToString() == "0" ? false : true;
                entity.date7 = dates[6].ToString("yyyy-MM-dd");
                entity.chkdate7shift1 = data.Rows[i][21].ToString() == "0" ? false : true;
                entity.chkdate7shift2 = data.Rows[i][22].ToString() == "0" ? false : true;
                entity.chkdate7shift3 = data.Rows[i][23].ToString() == "0" ? false : true;
                entitydata.Add(entity);
            }
            return entitydata;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in gridOperatorMachine.Rows)
                {
                    ShiftAllocationEntity entity = new ShiftAllocationEntity();
                    entity.PlantID = (Row.FindControl("hidPlantid") as HiddenField).Value;
                    entity.PhoneNo = (Row.FindControl("cmbPhone") as Label).Text;
                    entity.EmailId = (Row.FindControl("cmbEmailID") as Label).Text;
                    entity.Consumer = (Row.FindControl("cmbOperator") as Label).Text;
                    entity.date1 = (gridOperatorMachine.HeaderRow.Cells[2].FindControl("Header1") as Label).Text;
                    entity.date2 = (gridOperatorMachine.HeaderRow.Cells[3].FindControl("Header2") as Label).Text;
                    entity.date3 = (gridOperatorMachine.HeaderRow.Cells[4].FindControl("Header3") as Label).Text;
                    entity.date4 = (gridOperatorMachine.HeaderRow.Cells[5].FindControl("Header4") as Label).Text;
                    entity.date5 = (gridOperatorMachine.HeaderRow.Cells[6].FindControl("Header5") as Label).Text;
                    entity.date6 = (gridOperatorMachine.HeaderRow.Cells[7].FindControl("Header6") as Label).Text;
                    entity.date7 = (gridOperatorMachine.HeaderRow.Cells[8].FindControl("Header7") as Label).Text;
                    List<string> shiftID = DataBaseAccess.GetShiftID();
                    DataBaseAccess.DeleteAlertShiftAllocation(entity.date1, entity.date7, entity.Consumer);
                    #region date1
                    if ((Row.FindControl("chkdate1shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date1, shiftID[0], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate1shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date1, shiftID[1], entity.Consumer);
                    }
                    if ((Row.FindControl("chkdate1shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date1, shiftID[2], entity.Consumer);
                    }
                    #endregion

                    #region date2
                    if ((Row.FindControl("chkdate2shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date2, shiftID[0], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate2shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date2, shiftID[1], entity.Consumer);
                    }
                    if ((Row.FindControl("chkdate2shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date2, shiftID[2], entity.Consumer);
                    }
                    #endregion

                    if ((Row.FindControl("chkdate3shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date3, shiftID[0], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate3shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date3, shiftID[1], entity.Consumer);
                    }
                    if ((Row.FindControl("chkdate3shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date3, shiftID[2], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate4shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date4, shiftID[0], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate4shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date4, shiftID[1], entity.Consumer);
                    }
                    if ((Row.FindControl("chkdate4shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date4, shiftID[2], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate5shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date5, shiftID[0], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate5shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date5, shiftID[1], entity.Consumer);
                    }
                    if ((Row.FindControl("chkdate5shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date5, shiftID[2], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate6shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date6, shiftID[0], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate6shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date6, shiftID[1], entity.Consumer);
                    }
                    if ((Row.FindControl("chkdate6shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date6, shiftID[2], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate7shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date7, shiftID[0], entity.Consumer);
                    }

                    if ((Row.FindControl("chkdate7shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date7, shiftID[1], entity.Consumer);
                    }
                    if ((Row.FindControl("chkdate7shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveAlertUserShiftAllocation(entity.date7, shiftID[2], entity.Consumer);
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Dats Saved')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void View_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void gridOperatorMachine_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                List<string> Shift = DataBaseAccess.shiftdetail();
                DateTime Fromdate = Util.GetDateTime(txtFromDate.Text);
                DateTime Todate = Fromdate.AddDays(7);
                e.Row.Cells[0].Text = "SLNO";
                int i = 2;
                Label abc = e.Row.FindControl("Header1") as Label;
                abc.Text = Fromdate.ToString("yyyy-MM-dd");
                abc = e.Row.Cells[i].FindControl("lbldate1shift1") as Label;
                abc.Text = Shift[0];
                abc = e.Row.Cells[i].FindControl("lbldate1shift2") as Label;
                abc.Text = Shift[1];
                abc = e.Row.Cells[i].FindControl("lbldate1shift3") as Label;
                abc.Text = Shift[2];
                Fromdate = Fromdate.AddDays(1);

                abc = e.Row.FindControl("Header2") as Label;
                abc.Text = Fromdate.ToString("yyyy-MM-dd");
                abc = e.Row.Cells[i].FindControl("lbldate2shift1") as Label;
                abc.Text = Shift[0];
                abc = e.Row.Cells[i].FindControl("lbldate2shift2") as Label;
                abc.Text = Shift[1];
                abc = e.Row.Cells[i].FindControl("lbldate2shift3") as Label;
                abc.Text = Shift[2];
                Fromdate = Fromdate.AddDays(1);

                abc = e.Row.FindControl("Header3") as Label;
                abc.Text = Fromdate.ToString("yyyy-MM-dd");
                abc = e.Row.Cells[i].FindControl("lbldate3shift1") as Label;
                abc.Text = Shift[0];
                abc = e.Row.Cells[i].FindControl("lbldate3shift2") as Label;
                abc.Text = Shift[1];
                abc = e.Row.Cells[i].FindControl("lbldate3shift3") as Label;
                abc.Text = Shift[2];
                Fromdate = Fromdate.AddDays(1);
                abc = e.Row.FindControl("Header4") as Label;
                abc.Text = Fromdate.ToString("yyyy-MM-dd");
                abc = e.Row.Cells[i].FindControl("lbldate4shift1") as Label;
                abc.Text = Shift[0];
                abc = e.Row.Cells[i].FindControl("lbldate4shift2") as Label;
                abc.Text = Shift[1];
                abc = e.Row.Cells[i].FindControl("lbldate4shift3") as Label;
                abc.Text = Shift[2];
                Fromdate = Fromdate.AddDays(1);
                abc = e.Row.FindControl("Header5") as Label;
                abc.Text = Fromdate.ToString("yyyy-MM-dd");
                abc = e.Row.Cells[i].FindControl("lbldate5shift1") as Label;
                abc.Text = Shift[0];
                abc = e.Row.Cells[i].FindControl("lbldate5shift2") as Label;
                abc.Text = Shift[1];
                abc = e.Row.Cells[i].FindControl("lbldate5shift3") as Label;
                abc.Text = Shift[2];
                Fromdate = Fromdate.AddDays(1);
                abc = e.Row.FindControl("Header6") as Label;
                abc.Text = Fromdate.ToString("yyyy-MM-dd");
                abc = e.Row.Cells[i].FindControl("lbldate6shift1") as Label;
                abc.Text = Shift[0];
                abc = e.Row.Cells[i].FindControl("lbldate6shift2") as Label;
                abc.Text = Shift[1];
                abc = e.Row.Cells[i].FindControl("lbldate6shift3") as Label;
                abc.Text = Shift[2];
                Fromdate = Fromdate.AddDays(1);
                abc = e.Row.FindControl("Header7") as Label;
                abc.Text = Fromdate.ToString("yyyy-MM-dd");
                abc = e.Row.Cells[i].FindControl("lbldate7shift1") as Label;
                abc.Text = Shift[0];
                abc = e.Row.Cells[i].FindControl("lbldate7shift2") as Label;
                abc.Text = Shift[1];
                abc = e.Row.Cells[i].FindControl("lbldate7shift3") as Label;
                abc.Text = Shift[2];
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            DateTime fromdate = Util.GetDateTime(txtFromDate.Text);
            fromdate = DataBaseAccess.GetUserShiftAllocationDate(fromdate, "Prev", cmbOperator.SelectedValue.ToString());
            try
            {
                txtFromDate.Text = fromdate.ToString("dd-MM-yyyy");
                BindGrid(); ;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

            DateTime fromdate = Util.GetDateTime(txtFromDate.Text);
            fromdate = DataBaseAccess.GetUserShiftAllocationDate(fromdate, "Next", cmbOperator.SelectedValue.ToString());
            try
            {
                txtFromDate.Text = fromdate.ToString("dd-MM-yyyy");
                BindGrid(); ;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
    }

    public class ShiftAllocationEntity
    {
        public int SLNO { get; set; }
        public string PlantID { get; set; }
        public string Consumer { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string shift1 { get; set; }
        public string shift2 { get; set; }
        public string shift3 { get; set; }
        public string date1 { get; set; }
        public bool chkdate1shift1 { get; set; }
        public bool chkdate1shift2 { get; set; }
        public bool chkdate1shift3 { get; set; }

        public string date2 { get; set; }
        public bool chkdate2shift1 { get; set; }
        public bool chkdate2shift2 { get; set; }
        public bool chkdate2shift3 { get; set; }

        public string date3 { get; set; }
        public bool chkdate3shift1 { get; set; }
        public bool chkdate3shift2 { get; set; }
        public bool chkdate3shift3 { get; set; }
        public string date4 { get; set; }
        public bool chkdate4shift1 { get; set; }
        public bool chkdate4shift2 { get; set; }
        public bool chkdate4shift3 { get; set; }
        public string date5 { get; set; }
        public bool chkdate5shift1 { get; set; }
        public bool chkdate5shift2 { get; set; }
        public bool chkdate5shift3 { get; set; }
        public string date6 { get; set; }
        public bool chkdate6shift1 { get; set; }
        public bool chkdate6shift2 { get; set; }
        public bool chkdate6shift3 { get; set; }
        public string date7 { get; set; }
        public bool chkdate7shift1 { get; set; }
        public bool chkdate7shift2 { get; set; }
        public bool chkdate7shift3 { get; set; }
    }
}
