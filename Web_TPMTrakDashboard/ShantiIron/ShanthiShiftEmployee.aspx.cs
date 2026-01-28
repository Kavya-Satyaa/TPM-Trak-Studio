using AjaxControlToolkit;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class ShanthiShiftEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindDetails();


                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                BindGrid();
            }
        }

        private void BindDetails()
        {
            cmbMachineID.DataSource = DataBaseAccess.GetAllMachines("");
            cmbMachineID.DataBind();
        }

        private void BindGrid()
        {
            try
            {
                DateTime Fromdate = Util.GetDateTime(txtFromDate.Text);
                DateTime Todate = Fromdate.AddDays(7);
                if ((Todate - Fromdate).Days > 7)
                {

                }
                else
                {
                    List<DateTime> dates = new List<DateTime>();
                    List<string> Shift = DataBaseAccess.shiftdetail();
                    if (cmbtype.SelectedValue == null) return;

                    DataTable Data = DataBaseAccess.GetMachineOperatordetails(Fromdate, Todate, cmbMachineID.SelectedValue.ToString(), cmbtype.SelectedValue.ToString());
                    if (Data != null && Data.Rows.Count > 0)
                    {
                        List<ShanthiMachineOperator> entitydata = GetList(Data, Shift, Fromdate, Todate);
                        gridOperatorMachine.DataSource = entitydata;
                        gridOperatorMachine.DataBind();
                    }
                    else
                    {
                        gridOperatorMachine.DataSource = null;
                        gridOperatorMachine.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private List<ShanthiMachineOperator> GetList(DataTable data, List<string> shift, DateTime Fromdate, DateTime Todate)
        {
            List<ShanthiMachineOperator> entitydata = new List<ShanthiMachineOperator>();
            DateTime fromdate = Fromdate;
            List<DateTime> dates = new List<DateTime>();
            while (!(fromdate == Todate))
            {
                dates.Add(fromdate); fromdate = fromdate.AddDays(1);
            }

            for (int i = 0; i < data.Rows.Count; i++)
            {
                ShanthiMachineOperator entity = new ShanthiMachineOperator();
                entity.SLNO = i + 1;
                entity.MachineID = data.Rows[i][0].ToString();
                entity.Operator = data.Rows[i][1].ToString();
                entity.type = data.Rows[i][2].ToString();
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
                    ShanthiMachineOperator entity = new ShanthiMachineOperator();
                    entity.MachineID = (Row.FindControl("hidMachineid") as HiddenField).Value;
                    entity.type = (Row.FindControl("HidType") as HiddenField).Value;
                    entity.Operator = (Row.FindControl("cmbOperator") as Label).Text;
                    entity.date1 = (gridOperatorMachine.HeaderRow.Cells[2].FindControl("Header1") as Label).Text;
                    entity.date2 = (gridOperatorMachine.HeaderRow.Cells[3].FindControl("Header2") as Label).Text;
                    entity.date3 = (gridOperatorMachine.HeaderRow.Cells[4].FindControl("Header3") as Label).Text;
                    entity.date4 = (gridOperatorMachine.HeaderRow.Cells[5].FindControl("Header4") as Label).Text;
                    entity.date5 = (gridOperatorMachine.HeaderRow.Cells[6].FindControl("Header5") as Label).Text;
                    entity.date6 = (gridOperatorMachine.HeaderRow.Cells[7].FindControl("Header6") as Label).Text;
                    entity.date7 = (gridOperatorMachine.HeaderRow.Cells[8].FindControl("Header7") as Label).Text;
                    entity.shift1 = (Row.FindControl("lbldate1shift1") as Label).Text;
                    entity.shift2 = (Row.FindControl("lbldate1shift2") as Label).Text;
                    entity.shift3 = (Row.FindControl("lbldate1shift3") as Label).Text;
                    DataBaseAccess.DeleteEmployeeShift(entity.date1, entity.MachineID, entity.type, entity.Operator);
                    DataBaseAccess.DeleteEmployeeShift(entity.date2, entity.MachineID, entity.type, entity.Operator);
                    DataBaseAccess.DeleteEmployeeShift(entity.date3, entity.MachineID, entity.type, entity.Operator);
                    DataBaseAccess.DeleteEmployeeShift(entity.date4, entity.MachineID, entity.type, entity.Operator);
                    DataBaseAccess.DeleteEmployeeShift(entity.date5, entity.MachineID, entity.type, entity.Operator);
                    DataBaseAccess.DeleteEmployeeShift(entity.date6, entity.MachineID, entity.type, entity.Operator);
                    DataBaseAccess.DeleteEmployeeShift(entity.date7, entity.MachineID, entity.type, entity.Operator);
                    #region date1
                    if ((Row.FindControl("chkdate1shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date1, entity.MachineID, entity.type, entity.shift1, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate1shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date1, entity.MachineID, entity.type, entity.shift2, entity.Operator);
                    }
                    if ((Row.FindControl("chkdate1shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date1, entity.MachineID, entity.type, entity.shift3, entity.Operator);
                    }
                    #endregion

                    #region date2
                    if ((Row.FindControl("chkdate2shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date2, entity.MachineID, entity.type, entity.shift1, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate2shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date2, entity.MachineID, entity.type, entity.shift2, entity.Operator);
                    }
                    if ((Row.FindControl("chkdate2shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date2, entity.MachineID, entity.type, entity.shift3, entity.Operator);
                    }
                    #endregion

                    if ((Row.FindControl("chkdate3shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date3, entity.MachineID, entity.type, entity.shift1, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate3shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date3, entity.MachineID, entity.type, entity.shift2, entity.Operator);
                    }
                    if ((Row.FindControl("chkdate3shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date3, entity.MachineID, entity.type, entity.shift3, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate4shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date4, entity.MachineID, entity.type, entity.shift1, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate4shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date4, entity.MachineID, entity.type, entity.shift2, entity.Operator);
                    }
                    if ((Row.FindControl("chkdate4shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date4, entity.MachineID, entity.type, entity.shift3, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate5shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date5, entity.MachineID, entity.type, entity.shift1, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate5shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date5, entity.MachineID, entity.type, entity.shift2, entity.Operator);
                    }
                    if ((Row.FindControl("chkdate5shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date5, entity.MachineID, entity.type, entity.shift3, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate6shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date6, entity.MachineID, entity.type, entity.shift1, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate6shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date6, entity.MachineID, entity.type, entity.shift2, entity.Operator);
                    }
                    if ((Row.FindControl("chkdate6shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date6, entity.MachineID, entity.type, entity.shift3, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate7shift1") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date7, entity.MachineID, entity.type, entity.shift1, entity.Operator);
                    }

                    if ((Row.FindControl("chkdate7shift2") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date7, entity.MachineID, entity.type, entity.shift2, entity.Operator);
                    }
                    if ((Row.FindControl("chkdate7shift3") as CheckBox).Checked)
                    {
                        DataBaseAccess.SaveEmployeeShift(entity.date7, entity.MachineID, entity.type, entity.shift3, entity.Operator);
                    }



                }
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
    }
    public class ShanthiMachineOperator
    {
        public int SLNO { get; set; }
        public string MachineID { get; set; }
        public string Shift { get; set; }
        public string Operator { get; set; }
        public string type { get; set; }
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