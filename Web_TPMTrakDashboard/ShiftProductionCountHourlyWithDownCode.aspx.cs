using BusinessClassLibrary;
using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class ShiftProductionCountHourlyWithDownCode : System.Web.UI.Page
    {
        public int Container0 = 0, Container1 = 0, Container2 = 0, Margin0 = 0, Margin1 = 0, Margin2 = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["connectionString"] == null)
                Response.Redirect("~/SignIn.aspx", false);
            if (!IsPostBack)
            {
                timerDataChange.Enabled = false;
                BindPlantId();
                getcontainerandmargin();
                BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                if (ConfigurationManager.AppSettings["sonapages"].Equals("1", StringComparison.OrdinalIgnoreCase))
                    divDowntimeGraph.Visible = false;
                else
                    divDowntimeGraph.Visible = true;
                btnProcess_Click(null, null);
            }
        }

        private void getcontainerandmargin()
        {
            Int32.TryParse(ConfigurationManager.AppSettings["Container0"].ToString(), out Container0);
            Int32.TryParse(ConfigurationManager.AppSettings["Container1"].ToString(), out Container1);
            Int32.TryParse(ConfigurationManager.AppSettings["Container2"].ToString(), out Container2);
            Int32.TryParse(ConfigurationManager.AppSettings["Margin0"].ToString(), out Margin0);
            Int32.TryParse(ConfigurationManager.AppSettings["Margin1"].ToString(), out Margin1);
            Int32.TryParse(ConfigurationManager.AppSettings["Margin2"].ToString(), out Margin2);
        }

        #region "Bind Plant Id"
        private void BindPlantId()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlantId.DataSource = lstPlantData;
                ddlPlantId.DataBind();
                ddlPlantId_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Cell Id"
        private void BindCellId(string plantId)
        {
            try
            {
                List<string> lstCellId = BindCockpitView.ViewCellsToDisplay(plantId == "Plant All" ? "" : plantId);
                ddlCellID.DataSource = lstCellId;
                ddlCellID.DataBind();
                ddlCellID.Items.Insert(0, new ListItem
                {
                    Text = GetGlobalResourceObject("CommanResource", "CellAll").ToString(),
                    Value = "All"
                });
                ddlCellID.SelectedIndex = 0;
                ddlCellID_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Bind Machine Id"
        private void BindMachines(string cellId)
        {
            List<string> allMachineName = new List<string>();
            try
            {
                ddlMachineId.Items.Clear();
                if (string.IsNullOrEmpty(cellId) || cellId == "Cell All" || cellId == "All")
                {
                    cellId = "";
                    allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());
                }
                else
                {
                    allMachineName = CockpitDataBaseAccess.GetMachinesForCell(cellId, ddlPlantId.SelectedValue.ToString());
                }
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }
        #endregion

        #region "Populate data-----"
        private void PopulateData()
        {
            try
            {
                DateTime startDate = DateTime.Now.Date;
                startDate = Util.GetDateTime(txtDate.Text);
                DataTable dt = new DataTable();
                DataTable dtAeeLosses = new DataTable();

                dt = DataBaseAccess.ShiftProductionCountHourlyBNG(startDate.Date, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString());
                dtAeeLosses = DataBaseAccess.ShiftProductionCountHourlyBNGAeeLoss(startDate.Date, ddlPlantId.SelectedValue.ToString(), ddlMachineId.SelectedValue.ToString());

                double[] KWH = new double[dt.Rows.Count];
                List<string> hourlabel = new List<string>();
                List<int> hours = new List<int>();
                List<double> Actual = new List<double>();
                List<double> Target = new List<double>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double value; int valuehr = 0;
                    //if (int.TryParse(dt.Rows[i]["KWH"].ToString(), out value))
                    //{
                    //    KWH[i] = value;
                    //}
                    if (double.TryParse(dt.Rows[i]["Actual"].ToString(), out value))
                    {
                        Actual.Insert(i, value);
                    }
                    if (double.TryParse(dt.Rows[i]["Target"].ToString(), out value))
                    {
                        Target.Insert(i, value);
                    }
                    if (int.TryParse(dt.Rows[i]["Hourid"].ToString(), out valuehr))
                    {
                        hours.Insert(i, valuehr);
                    }
                    hourlabel.Insert(i, dt.Rows[i]["HourLabel"].ToString());
                }
                if (hours.Count != 33)
                {
                    hours.Insert(hours.Count, 0);
                }
                if (Target.Count != 33)
                {
                    Target.Insert(Target.Count, 0);
                }
                if (Actual.Count != 33)
                {
                    Actual.Insert(Actual.Count, 0);
                }
                if (hourlabel.Count != 33)
                {
                    hourlabel.Insert(hourlabel.Count, "");
                }
                #region "First shift"
                if (hours[0] == 1)
                {
                    lblshift1hour1.InnerText = hourlabel[0].ToString();
                    lblshift1hour1TPCS.Text = Target[0].ToString();
                    lblshift1hour1TCumu.Text = Target[0].ToString();
                    lblshift1hour1APCS.Text = Actual[0].ToString();
                    lblshift1hour1ACumu.Text = Actual[0].ToString();
                }

                if (hours[1] == 2)
                {
                    lblshift1hour2.InnerText = hourlabel[1].ToString();
                    lblshift1hour2TPCS.Text = Target[1].ToString();
                    lblshift1hour2TCumu.Text = (double.Parse(lblshift1hour1TCumu.Text) + Target[1]).ToString("00");
                    lblshift1hour2APCS.Text = Actual[1].ToString();
                    lblshift1hour2ACumu.Text = (double.Parse(lblshift1hour1ACumu.Text) + Actual[1]).ToString("00");
                }
                if (hours[2] == 3)
                {
                    lblshift1hour3.InnerText = hourlabel[2].ToString();
                    lblshift1hour3TPCS.Text = Target[2].ToString();
                    lblshift1hour3TCumu.Text = (double.Parse(lblshift1hour2TCumu.Text) + Target[2]).ToString("00");
                    lblshift1hour3APCS.Text = Actual[2].ToString();
                    lblshift1hour3ACumu.Text = (double.Parse(lblshift1hour2ACumu.Text) + Actual[2]).ToString("00");
                }
                if (hours[3] == 4)
                {
                    lblshift1hour4.InnerText = hourlabel[3].ToString();
                    lblshift1hour4TPCS.Text = Target[3].ToString();
                    lblshift1hour4TCumu.Text = (double.Parse(lblshift1hour3TCumu.Text) + Target[3]).ToString("00");
                    lblshift1hour4APCS.Text = Actual[3].ToString();
                    lblshift1hour4ACumu.Text = (double.Parse(lblshift1hour3ACumu.Text) + Actual[3]).ToString("00");
                }
                if (hours[4] == 5)
                {
                    lblshift1hour5.InnerText = hourlabel[4].ToString();
                    lblshift1hour5TPCS.Text = Target[4].ToString();
                    lblshift1hour5TCumu.Text = (double.Parse(lblshift1hour4TCumu.Text) + Target[4]).ToString("00");
                    lblshift1hour5APCS.Text = Actual[4].ToString();
                    lblshift1hour5ACumu.Text = (double.Parse(lblshift1hour4ACumu.Text) + Actual[4]).ToString("00");
                }
                if (hours[5] == 6)
                {
                    lblshift1hour6.InnerText = hourlabel[5].ToString();
                    lblshift1hour6TPCS.Text = Target[5].ToString();
                    lblshift1hour6TCumu.Text = (double.Parse(lblshift1hour5TCumu.Text) + Target[5]).ToString("00");
                    lblshift1hour6APCS.Text = Actual[5].ToString();
                    lblshift1hour6ACumu.Text = (double.Parse(lblshift1hour5ACumu.Text) + Actual[5]).ToString("00");
                }
                if (hours[6] == 7)
                {
                    lblshift1hour7.InnerText = hourlabel[6].ToString();
                    lblshift1hour7TPCS.Text = Target[6].ToString();
                    lblshift1hour7TCumu.Text = (double.Parse(lblshift1hour6TCumu.Text) + Target[6]).ToString("00");
                    lblshift1hour7APCS.Text = Actual[6].ToString();
                    lblshift1hour7ACumu.Text = (double.Parse(lblshift1hour6ACumu.Text) + Actual[6]).ToString("00");
                }
                if (hours[7] == 8)
                {
                    lblshift1hour8.InnerText = hourlabel[7].ToString();
                    lblshift1hour8TPCS.Text = Target[7].ToString();
                    lblshift1hour8TCumu.Text = (double.Parse(lblshift1hour7TCumu.Text) + Target[7]).ToString("00");
                    lblshift1hour8APCS.Text = Actual[7].ToString();
                    lblshift1hour8ACumu.Text = (double.Parse(lblshift1hour7ACumu.Text) + Actual[7]).ToString("00");
                }
                if (hours[8] == 9)
                {

                    lblshift1hour9.InnerText = hourlabel[8].ToString();
                    lblshift1hour9TPCS.Text = Target[8].ToString();
                    lblshift1hour9TCumu.Text = (double.Parse(lblshift1hour8TCumu.Text) + Target[8]).ToString("00");
                    lblshift1hour9APCS.Text = Actual[8].ToString();
                    lblshift1hour9ACumu.Text = (double.Parse(lblshift1hour8ACumu.Text) + Actual[8]).ToString("00");
                }
                else
                {
                    hours.Insert(8, 0);
                    Target.Insert(8, 0);
                    Actual.Insert(8, 0);
                    hourlabel.Insert(8, "");
                    lblshift1hour9.InnerText = "";
                    lblshift1hour9TPCS.Text = "";
                    lblshift1hour9TCumu.Text = (double.Parse(lblshift1hour8TCumu.Text) + 0).ToString("00");
                    lblshift1hour9APCS.Text = "";
                    lblshift1hour9ACumu.Text = (double.Parse(lblshift1hour8ACumu.Text) + 0).ToString("00");
                    trshift1hour9.Visible = false;
                }
                if (hours[9] == 10)
                {

                    lblshift1hour10.InnerText = hourlabel[9].ToString();
                    lblshift1hour10TPCS.Text = Target[9].ToString();
                    lblshift1hour10TCumu.Text = (double.Parse(lblshift1hour9TCumu.Text) + Target[9]).ToString("00");
                    lblshift1hour10APCS.Text = Actual[9].ToString();
                    lblshift1hour10ACumu.Text = (double.Parse(lblshift1hour9ACumu.Text) + Actual[9]).ToString("00");
                }
                else
                {
                    hours.Insert(9, 0);
                    Target.Insert(9, 0);
                    Actual.Insert(8, 0);
                    hourlabel.Insert(9, "");
                    lblshift1hour10.InnerText = "";
                    lblshift1hour10TPCS.Text = "";
                    lblshift1hour10TCumu.Text = (double.Parse(lblshift1hour9TCumu.Text) + 0).ToString("00");
                    lblshift1hour10APCS.Text = "";
                    lblshift1hour10ACumu.Text = (double.Parse(lblshift1hour9ACumu.Text) + 0).ToString("00");
                    trshift1hour10.Visible = false;
                }
                if (hours[10] == 11)
                {

                    lblshift1hour11.InnerText = hourlabel[10].ToString();
                    lblshift1hour11TPCS.Text = Target[10].ToString();
                    lblshift1hour11TCumu.Text = (double.Parse(lblshift1hour10TCumu.Text) + Target[10]).ToString("00");
                    lblshift1hour11APCS.Text = Actual[10].ToString();
                    lblshift1hour11ACumu.Text = (double.Parse(lblshift1hour10ACumu.Text) + Actual[10]).ToString("00");
                }
                else
                {
                    hours.Insert(9, 0);
                    Target.Insert(9, 0);
                    Actual.Insert(9, 0);
                    hourlabel.Insert(9, "");
                    lblshift1hour11.InnerText = "";
                    lblshift1hour11TPCS.Text = "";
                    lblshift1hour11TCumu.Text = (double.Parse(lblshift1hour10TCumu.Text) + 0).ToString("00");
                    lblshift1hour11APCS.Text = "";
                    lblshift1hour11ACumu.Text = (double.Parse(lblshift1hour10ACumu.Text) + 0).ToString("00");
                    trshift1hour11.Visible = false;
                }
                if (hours[11] == 12)
                {

                    lblshift1hour12.InnerText = hourlabel[11].ToString();
                    lblshift1hour12TPCS.Text = Target[11].ToString();
                    lblshift1hour12TCumu.Text = (double.Parse(lblshift1hour11TCumu.Text) + Target[11]).ToString("00");
                    lblshift1hour12APCS.Text = Actual[11].ToString();
                    lblshift1hour12ACumu.Text = (double.Parse(lblshift1hour11ACumu.Text) + Actual[11]).ToString("00");
                }
                else
                {
                    hours.Insert(10, 0);
                    Target.Insert(10, 0);
                    Actual.Insert(10, 0);
                    hourlabel.Insert(10, "");
                    lblshift1hour12.InnerText = "";
                    lblshift1hour12TPCS.Text = "";
                    lblshift1hour12TCumu.Text = (double.Parse(lblshift1hour11TCumu.Text) + 0).ToString("00");
                    lblshift1hour12APCS.Text = "";
                    lblshift1hour12ACumu.Text = (double.Parse(lblshift1hour11ACumu.Text) + 0).ToString("00");
                    trshift1hour12.Visible = false;
                }
                #endregion

                #region "First shift summation"
                lbl1SsumTarget.Text = lblshift1hour12TCumu.Text;
                lbl1SsumActual.Text = lblshift1hour12ACumu.Text;
                #endregion

                #region "Second shift"
                if (hours[12] == 1)
                {
                    lblshift2hour1.InnerText = hourlabel[12].ToString();
                    lblshift2hour1TPCS.Text = Target[12].ToString();
                    lblshift2hour1TCumu.Text = (Target[12]).ToString();
                    lblshift2hour1APCS.Text = Actual[12].ToString();
                    lblshift2hour1ACumu.Text = (Actual[12]).ToString();
                }
                if (hours[13] == 2)
                {
                    lblshift2hour2.InnerText = hourlabel[13].ToString();
                    lblshift2hour2TPCS.Text = Target[13].ToString();
                    lblshift2hour2TCumu.Text = (double.Parse(lblshift2hour1TCumu.Text) + Target[13]).ToString("00");
                    lblshift2hour2APCS.Text = Actual[13].ToString();
                    lblshift2hour2ACumu.Text = (double.Parse(lblshift2hour1ACumu.Text) + Actual[13]).ToString("00");
                }
                if (hours[14] == 3)
                {
                    lblshift2hour3.InnerText = hourlabel[14].ToString();
                    lblshift2hour3TPCS.Text = Target[14].ToString();
                    lblshift2hour3TCumu.Text = (double.Parse(lblshift2hour2TCumu.Text) + Target[14]).ToString("00");
                    lblshift2hour3APCS.Text = Actual[14].ToString();
                    lblshift2hour3ACumu.Text = (double.Parse(lblshift2hour2ACumu.Text) + Actual[14]).ToString("00");
                }
                if (hours[15] == 4)
                {
                    lblshift2hour4.InnerText = hourlabel[15].ToString();
                    lblshift2hour4TPCS.Text = Target[15].ToString();
                    lblshift2hour4TCumu.Text = (double.Parse(lblshift2hour3TCumu.Text) + Target[15]).ToString("00");
                    lblshift2hour4APCS.Text = Actual[15].ToString();
                    lblshift2hour4ACumu.Text = (double.Parse(lblshift2hour3ACumu.Text) + Actual[15]).ToString("00");
                }
                if (hours[16] == 5)
                {
                    lblshift2hour5.InnerText = hourlabel[16].ToString();
                    lblshift2hour5TPCS.Text = Target[16].ToString();
                    lblshift2hour5TCumu.Text = (double.Parse(lblshift2hour4TCumu.Text) + Target[16]).ToString("00");
                    lblshift2hour5APCS.Text = Actual[16].ToString();
                    lblshift2hour5ACumu.Text = (double.Parse(lblshift2hour4ACumu.Text) + Actual[16]).ToString("00");
                }
                if (hours[17] == 6)
                {
                    lblshift2hour6.InnerText = hourlabel[17].ToString();
                    lblshift2hour6TPCS.Text = Target[17].ToString();
                    lblshift2hour6TCumu.Text = (double.Parse(lblshift2hour5TCumu.Text) + Target[17]).ToString("00");
                    lblshift2hour6APCS.Text = Actual[17].ToString();
                    lblshift2hour6ACumu.Text = (double.Parse(lblshift2hour5ACumu.Text) + Actual[17]).ToString("00");
                }
                if (hours[18] == 7)
                {
                    lblshift2hour7.InnerText = hourlabel[18].ToString();
                    lblshift2hour7TPCS.Text = Target[18].ToString();
                    lblshift2hour7TCumu.Text = (double.Parse(lblshift2hour6TCumu.Text) + Target[18]).ToString("00");
                    lblshift2hour7APCS.Text = Actual[18].ToString();
                    lblshift2hour7ACumu.Text = (double.Parse(lblshift2hour6ACumu.Text) + Actual[18]).ToString("00");
                }
                if (hours[19] == 8)
                {
                    lblshift2hour8.InnerText = hourlabel[19].ToString();
                    lblshift2hour8TPCS.Text = Target[19].ToString();
                    lblshift2hour8TCumu.Text = (double.Parse(lblshift2hour7TCumu.Text) + Target[19]).ToString("00");
                    lblshift2hour8APCS.Text = Actual[19].ToString();
                    lblshift2hour8ACumu.Text = (double.Parse(lblshift2hour7ACumu.Text) + Actual[19]).ToString("00");
                }
                if (hours[20] == 9)
                {
                    lblshift2hour9.InnerText = hourlabel[20].ToString();
                    lblshift2hour9TPCS.Text = Target[20].ToString();
                    lblshift2hour9TCumu.Text = (double.Parse(lblshift2hour8TCumu.Text) + Target[20]).ToString("00");
                    lblshift2hour9APCS.Text = Actual[20].ToString();
                    lblshift2hour9ACumu.Text = (double.Parse(lblshift2hour8ACumu.Text) + Actual[20]).ToString("00");
                }
                else
                {
                    hours.Insert(20, 0);
                    hourlabel.Insert(20, "");
                    Target.Insert(20, 0);
                    Actual.Insert(20, 0);
                    lblshift2hour9.InnerText = "";
                    lblshift2hour9TPCS.Text = "";
                    lblshift2hour9TCumu.Text = lblshift2hour8TCumu.Text;
                    lblshift2hour9APCS.Text = "";
                    lblshift2hour9ACumu.Text = lblshift2hour8ACumu.Text;
                    trshift2hour9.Visible = false;
                }
                if (hours[21] == 10)
                {
                    lblshift2hour10.InnerText = hourlabel[21].ToString();
                    lblshift2hour10TPCS.Text = Target[21].ToString();
                    lblshift2hour10TCumu.Text = (double.Parse(lblshift2hour9TCumu.Text) + Target[21]).ToString("00");
                    lblshift2hour10APCS.Text = Actual[21].ToString();
                    lblshift2hour10ACumu.Text = (double.Parse(lblshift2hour9ACumu.Text) + Actual[21]).ToString("00");
                }
                else
                {
                    hours.Insert(21, 0);
                    hourlabel.Insert(21, "");
                    Target.Insert(21, 0);
                    Actual.Insert(21, 0);
                    lblshift2hour10.InnerText = "";
                    lblshift2hour10TPCS.Text = "";
                    lblshift2hour10TCumu.Text = lblshift2hour9TCumu.Text;
                    lblshift2hour10APCS.Text = "";
                    lblshift2hour10ACumu.Text = lblshift2hour9ACumu.Text;
                    trshift2hour10.Visible = false;
                }
                if (hours[22] == 11)
                {
                    lblshift2hour11.InnerText = hourlabel[22].ToString();
                    lblshift2hour11TPCS.Text = Target[22].ToString();
                    lblshift2hour11TCumu.Text = (double.Parse(lblshift2hour10TCumu.Text) + Target[22]).ToString("00");
                    lblshift2hour11APCS.Text = Actual[22].ToString();
                    lblshift2hour11ACumu.Text = (double.Parse(lblshift2hour10ACumu.Text) + Actual[22]).ToString("00");
                }
                else
                {
                    hours.Insert(22, 0);
                    hourlabel.Insert(22, "");
                    Target.Insert(22, 0);
                    Actual.Insert(22, 0);
                    lblshift2hour11.InnerText = "";
                    lblshift2hour11TPCS.Text = "";
                    lblshift2hour11TCumu.Text = lblshift2hour10TCumu.Text;
                    lblshift2hour11APCS.Text = "";
                    lblshift2hour11ACumu.Text = lblshift2hour10ACumu.Text;
                    trshift2hour11.Visible = false;

                }
                if (hours[23] == 12)
                {
                    lblshift2hour12.InnerText = hourlabel[23].ToString();
                    lblshift2hour12TPCS.Text = Target[23].ToString();
                    lblshift2hour12TCumu.Text = (double.Parse(lblshift2hour11TCumu.Text) + Target[23]).ToString("00");
                    lblshift2hour12APCS.Text = Actual[23].ToString();
                    lblshift2hour12ACumu.Text = (double.Parse(lblshift2hour11ACumu.Text) + Actual[23]).ToString("00");
                }
                else
                {
                    hours.Insert(23, 0);
                    hourlabel.Insert(23, "");
                    Target.Insert(23, 0);
                    Actual.Insert(23, 0);
                    lblshift2hour12.InnerText = "";
                    lblshift2hour12TPCS.Text = "";
                    lblshift2hour12TCumu.Text = lblshift2hour11TCumu.Text;
                    lblshift2hour12APCS.Text = "";
                    lblshift2hour12ACumu.Text = lblshift2hour11ACumu.Text;
                    trshift2hour12.Visible = false;
                }


                #endregion

                #region "second shift summation"
                lbl2SsumTarget.Text = lblshift2hour12TCumu.Text;
                lbl2SsumActual.Text = lblshift2hour12ACumu.Text;
                #endregion

                #region "Third shift"
                if (hours[24] == 1)
                {
                    lblshift3hour1.InnerText = hourlabel[24].ToString();
                    lblshift3hour1TPCS.Text = Target[24].ToString();
                    lblshift3hour1TCumu.Text = Target[24].ToString();
                    lblshift3hour1APCS.Text = Actual[24].ToString();
                    lblshift3hour1ACumu.Text = Actual[24].ToString();
                }
                else
                {
                    hours.Insert(24, 0);
                    hourlabel.Insert(24, "");
                    Target.Insert(24, 0);
                    Actual.Insert(24, 0);
                    lblshift3hour1.InnerText = "";
                    lblshift3hour1TPCS.Text = "0";
                    lblshift3hour1TCumu.Text = "0";
                    lblshift3hour1APCS.Text = "0";
                    lblshift3hour1ACumu.Text = "0";
                    trshift3hour1.Visible = false;
                }

                if (hours[25] == 2)
                {
                    lblshift3hour2.InnerText = hourlabel[25].ToString();
                    lblshift3hour2TPCS.Text = Target[25].ToString();
                    lblshift3hour2TCumu.Text = (double.Parse(lblshift3hour1TCumu.Text) + Target[25]).ToString("00");
                    lblshift3hour2APCS.Text = Actual[25].ToString();
                    lblshift3hour2ACumu.Text = (double.Parse(lblshift3hour1ACumu.Text) + Actual[25]).ToString("00");
                }
                else
                {
                    hours.Insert(25, 0);
                    hourlabel.Insert(25, "");
                    Target.Insert(25, 0);
                    Actual.Insert(25, 0);
                    lblshift3hour2.InnerText = "";
                    lblshift3hour2TPCS.Text = lblshift3hour1TPCS.Text;
                    lblshift3hour2TCumu.Text = lblshift3hour1TCumu.Text;
                    lblshift3hour2APCS.Text = lblshift3hour1APCS.Text;
                    lblshift3hour2ACumu.Text = lblshift3hour2ACumu.Text;
                    trshift3hour2.Visible = false;
                }
                if (hours[26] == 3)
                {
                    lblshift3hour3.InnerText = hourlabel[26].ToString();
                    lblshift3hour3TPCS.Text = Target[26].ToString();
                    lblshift3hour3TCumu.Text = (double.Parse(lblshift3hour2TCumu.Text) + Target[26]).ToString("00");
                    lblshift3hour3APCS.Text = Actual[26].ToString();
                    lblshift3hour3ACumu.Text = (double.Parse(lblshift3hour2ACumu.Text) + Actual[26]).ToString("00");
                }
                else
                {
                    hours.Insert(26, 0);
                    hourlabel.Insert(26, "");
                    Target.Insert(26, 0);
                    Actual.Insert(26, 0);
                    lblshift3hour3.InnerText = "";
                    lblshift3hour3TPCS.Text = lblshift3hour2TPCS.Text;
                    lblshift3hour3TCumu.Text = lblshift3hour2TCumu.Text;
                    lblshift3hour3APCS.Text = lblshift3hour2APCS.Text;
                    lblshift3hour3ACumu.Text = lblshift3hour2ACumu.Text;
                    trshift3hour3.Visible = false;
                }
                if (hours[27] == 4)
                {
                    lblshift3hour4.InnerText = hourlabel[27].ToString();
                    lblshift3hour4TPCS.Text = Target[27].ToString();
                    lblshift3hour4TCumu.Text = (double.Parse(lblshift3hour3TCumu.Text) + Target[27]).ToString("00");
                    lblshift3hour4APCS.Text = Actual[27].ToString();
                    lblshift3hour4ACumu.Text = (double.Parse(lblshift3hour3ACumu.Text) + Actual[27]).ToString("00");
                }
                else
                {
                    hours.Insert(27, 0);
                    hourlabel.Insert(27, "");
                    Target.Insert(27, 0);
                    Actual.Insert(27, 0);
                    lblshift3hour4.InnerText = "";
                    lblshift3hour4TPCS.Text = lblshift3hour3TPCS.Text;
                    lblshift3hour4TCumu.Text = lblshift3hour3TCumu.Text;
                    lblshift3hour4APCS.Text = lblshift3hour3APCS.Text;
                    lblshift3hour4ACumu.Text = lblshift3hour3ACumu.Text;
                    trshift3hour4.Visible = false;
                }
                if (hours[28] == 5)
                {
                    lblshift3hour5.InnerText = hourlabel[28].ToString();
                    lblshift3hour5TPCS.Text = Target[28].ToString();
                    lblshift3hour5TCumu.Text = (double.Parse(lblshift3hour4TCumu.Text) + Target[28]).ToString("00");
                    lblshift3hour5APCS.Text = Actual[28].ToString();
                    lblshift3hour5ACumu.Text = (double.Parse(lblshift3hour4ACumu.Text) + Actual[28]).ToString("00");
                }
                else
                {
                    hours.Insert(28, 0);
                    hourlabel.Insert(28, "");
                    Target.Insert(28, 0);
                    Actual.Insert(28, 0);
                    lblshift3hour5.InnerText = "";
                    lblshift3hour5TPCS.Text = lblshift3hour4TPCS.Text;
                    lblshift3hour5TCumu.Text = lblshift3hour4TCumu.Text;
                    lblshift3hour5APCS.Text = lblshift3hour4APCS.Text;
                    lblshift3hour5ACumu.Text = lblshift3hour4ACumu.Text;
                    trshift3hour5.Visible = false;
                }
                if (hours[29] == 6)
                {
                    lblshift3hour6.InnerText = hourlabel[29].ToString();
                    lblshift3hour6TPCS.Text = Target[29].ToString();
                    lblshift3hour6TCumu.Text = (double.Parse(lblshift3hour5TCumu.Text) + Target[29]).ToString("00");
                    lblshift3hour6APCS.Text = Actual[29].ToString();
                    lblshift3hour6ACumu.Text = (double.Parse(lblshift3hour5ACumu.Text) + Actual[29]).ToString("00");
                }
                else
                {
                    hours.Insert(29, 0);
                    hourlabel.Insert(29, "");
                    Target.Insert(29, 0);
                    Actual.Insert(29, 0);
                    lblshift3hour6.InnerText = "";
                    lblshift3hour6TPCS.Text = lblshift3hour5TPCS.Text;
                    lblshift3hour6TCumu.Text = lblshift3hour5TCumu.Text;
                    lblshift3hour6APCS.Text = lblshift3hour5APCS.Text;
                    lblshift3hour6ACumu.Text = lblshift3hour5ACumu.Text;
                    trshift3hour6.Visible = false;
                }
                if (hours[30] == 7)
                {
                    lblshift3hour7.InnerText = hourlabel[30].ToString();
                    lblshift3hour7TPCS.Text = Target[30].ToString();
                    lblshift3hour7TCumu.Text = (double.Parse(lblshift3hour6TCumu.Text) + Target[30]).ToString("00");
                    lblshift3hour7APCS.Text = Actual[30].ToString();
                    lblshift3hour7ACumu.Text = (double.Parse(lblshift3hour6ACumu.Text) + Actual[30]).ToString("00");
                }
                else
                {
                    hours.Insert(30, 0);
                    hourlabel.Insert(30, "");
                    Target.Insert(30, 0);
                    Actual.Insert(30, 0);
                    lblshift3hour7.InnerText = "";
                    lblshift3hour7TPCS.Text = lblshift3hour6TPCS.Text;
                    lblshift3hour7TCumu.Text = lblshift3hour6TCumu.Text;
                    lblshift3hour7APCS.Text = lblshift3hour6APCS.Text;
                    lblshift3hour7ACumu.Text = lblshift3hour6ACumu.Text;
                    trshift3hour7.Visible = false;
                }
                if (hours[31] == 8)
                {
                    lblshift3hour8.InnerText = hourlabel[31].ToString();
                    lblshift3hour8TPCS.Text = Target[31].ToString();
                    lblshift3hour8TCumu.Text = (double.Parse(lblshift3hour7TCumu.Text) + Target[31]).ToString("00");
                    lblshift3hour8APCS.Text = Actual[31].ToString();
                    lblshift3hour8ACumu.Text = (double.Parse(lblshift3hour7ACumu.Text) + Actual[31]).ToString("00");
                }
                else
                {
                    hours.Insert(31, 0);
                    hourlabel.Insert(31, "");
                    Target.Insert(31, 0);
                    Actual.Insert(31, 0);
                    lblshift3hour8.InnerText = "";
                    lblshift3hour8TPCS.Text = lblshift3hour7TPCS.Text;
                    lblshift3hour8TCumu.Text = lblshift3hour7TCumu.Text;
                    lblshift3hour8APCS.Text = lblshift3hour7APCS.Text;
                    lblshift3hour8ACumu.Text = lblshift3hour7ACumu.Text == "" ? "0" : lblshift3hour7ACumu.Text;
                    trshift3hour8.Visible = false;
                }
                if (hours[32] == 9)
                {
                    lblshift3hour9.InnerText = hourlabel[32].ToString();
                    lblshift3hour9TPCS.Text = Target[32].ToString();
                    lblshift3hour9TCumu.Text = (double.Parse(lblshift3hour8TCumu.Text) + Target[32]).ToString("00");
                    lblshift3hour9APCS.Text = Actual[32].ToString();
                    lblshift3hour9ACumu.Text = (double.Parse(lblshift3hour8ACumu.Text) + Actual[32]).ToString("00");
                }
                else
                {
                    hours.Insert(31, 0);
                    hourlabel.Insert(32, "");
                    Target.Insert(32, 0);
                    Actual.Insert(32, 0);
                    lblshift3hour9.InnerText = "";
                    lblshift3hour9TPCS.Text = "";
                    if (lblshift3hour1TCumu.Text != "")
                    {
                        lblshift3hour9TCumu.Text = (double.Parse(lblshift3hour8TCumu.Text) + 0).ToString("##.##");
                        lblshift3hour9APCS.Text = "";
                        lblshift3hour9ACumu.Text = (double.Parse(lblshift3hour8ACumu.Text) + 0).ToString("##.##");
                        trshift3hour9.Visible = false;
                    }
                    else
                    {
                        lblshift3hour9TCumu.Text = "0";
                        lblshift3hour9APCS.Text = "";
                        lblshift3hour9ACumu.Text = "0";
                        trshift3hour9.Visible = false;
                    }
                }
                #endregion

                #region "Third shift summation"
                if (lblshift3hour1TCumu.Text != "" && lblshift3hour1ACumu.Text != "")
                {
                    lbl3SsumTarget.Text = string.IsNullOrEmpty(lblshift3hour9TCumu.Text) ? "00" : lblshift3hour9TCumu.Text;
                    lbl3SsumActual.Text = string.IsNullOrEmpty(lblshift3hour9ACumu.Text) ? "00" : lblshift3hour9ACumu.Text;
                }
                else
                {
                    lbl3SsumTarget.Text = lblshift3hour9TCumu.Text;
                    lbl3SsumActual.Text = lblshift3hour9ACumu.Text;
                    trsummation3shift.Visible = false;
                }
                #endregion

                #region "Total Per Day"

                lblTotalTarget.Text = (double.Parse(lbl1SsumTarget.Text) + double.Parse(lbl2SsumTarget.Text) + double.Parse(lbl3SsumTarget.Text)).ToString("##.##");
                lblTotalActual.Text = (double.Parse(lbl1SsumActual.Text) + double.Parse(lbl2SsumActual.Text) + double.Parse(lbl3SsumActual.Text)).ToString("##.##");
                #endregion

                #region "KWH Values"
                //lblKWH1.Text = KWH[0].ToString();
                //lblKWH2.Text = KWH[1].ToString();
                //lblKWH3.Text = KWH[2].ToString();
                //lblKWH4.Text = KWH[3].ToString();
                //lblKWH5.Text = KWH[4].ToString();
                //lblKWH6.Text = KWH[5].ToString();
                //lblKWH7.Text = KWH[6].ToString();
                //lblKWH8.Text = KWH[7].ToString();
                //lblKWH9.Text = KWH[8].ToString();
                //lblKWH10.Text = KWH[9].ToString();
                //lblKWH11.Text = KWH[10].ToString();
                //lblKWH12.Text = KWH[11].ToString();
                //lblKWH13.Text = KWH[12].ToString();
                //lblKWH14.Text = KWH[13].ToString();
                //lblKWH15.Text = KWH[14].ToString();
                //lblKWH16.Text = KWH[15].ToString();
                //lblKWH17.Text = KWH[16].ToString();
                //lblKWH18.Text = KWH[17].ToString();
                //lblKWH19.Text = KWH[18].ToString();
                //lblKWH20.Text = KWH[10].ToString();
                //lblKWH21.Text = KWH[20].ToString();
                //lblKWH22.Text = KWH[21].ToString();
                //lblKWH23.Text = KWH[22].ToString();
                //lblKWH24.Text = KWH[23].ToString();
                #endregion

                #region "AEE Losses"
                DataView view = new DataView(dtAeeLosses);
                DataTable headerValues = view.ToTable(true, "DownCategory");


                int ShiftHourID = 0;
                DataTable Values = new DataTable();
                //Values.Columns.Add("HourID", typeof(string));
                for (int i = 0; i < headerValues.Rows.Count; i++)
                {
                    string strValue = headerValues.Rows[i]["DownCategory"].ToString();
                    Values.Columns.Add(strValue, typeof(string));
                }

                int newRowId = -1;
                DataRow row = null;
                for (int i = 0; i < dtAeeLosses.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtAeeLosses.Rows[i]["HourID"]) != ShiftHourID)
                    {
                        row = Values.NewRow();
                        Values.Rows.Add(row);
                        // Values.Rows[Values.Rows.Count - 1]["HourID"] = Convert.ToInt32(dtAeeLosses.Rows[i]["HourID"]);
                        newRowId++;
                    }
                    if (Convert.ToInt32(dtAeeLosses.Rows[i]["HourID"]) == 0)
                    {
                        Values.Rows[Values.Rows.Count - 1][dtAeeLosses.Rows[i]["DownCategory"].ToString()] = "";
                    }
                    else
                    {
                        Values.Rows[Values.Rows.Count - 1][dtAeeLosses.Rows[i]["DownCategory"].ToString()] = dtAeeLosses.Rows[i]["DownTime"];
                    }
                    ShiftHourID = Convert.ToInt32(dtAeeLosses.Rows[i]["HourID"]);

                }

                grdValueContain.DataSource = Values;
                grdValueContain.DataBind();
                //decimal d = 0;
                //decimal total2 = Values.AsEnumerable().Where(r => !r.IsNull(0) && decimal.TryParse(r[0].ToString(), out d)).Sum(r => d);
                //decimal? total1 = Values.AsEnumerable().Sum(r => r.Field<decimal?>("TRAINER"));
                //decimal total = Values.AsEnumerable().Sum(r => r.Field<decimal?>("TRAINER") ?? 0);
                //grdValueContain.FooterRow.Cells[1].Text = "Total";
                //grdValueContain.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                //grdValueContain.FooterRow.Cells[2].Text = total2.ToString("N2");

                ///--------------------------------
                ///--------------------------



                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMessages.Text = ex.ToString();
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        #endregion

        protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCellId(ddlPlantId.SelectedItem == null ? "" : ddlPlantId.SelectedItem.Text);
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            PopulateData();
            Label1.Text = GetLocalResourceObject("HourlyTracking").ToString() + " - " + ddlMachineId.SelectedValue;
            hdfValue.Value = "OK";
        }

        protected void timerDataChange_Tick(object sender, EventArgs e)
        {
            try
            {
                PopulateData();
                Label1.Text = GetLocalResourceObject("HourlyTracking").ToString() + " - " + ddlMachineId.SelectedValue;
                hdfValue.Value = "OK";
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(DataBaseAccess.AutoRefreshData);
                if (value > 10000)
                    timerDataChange.Interval = value;
                else
                    timerDataChange.Interval = 10000;

                if (chkAutoBox.Checked)
                    timerDataChange.Enabled = true;
                else
                    timerDataChange.Enabled = false;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        protected void ddlCellID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMachines(ddlCellID.SelectedItem == null ? "" : ddlCellID.SelectedItem.Text);
            hdfValue.Value = "NotOk";
        }
    }
}