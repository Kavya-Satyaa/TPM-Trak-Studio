using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PTA
{
    public partial class OperatorEffyReportPTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                txtMonth.Text = DateTime.Now.ToString("MM");
                BindPlant();
                BindShift();
                BindOperator();
                btnView_Click(null, null);
            }
        }
        private void BindShift()
        {
            try
            {
                List<string> list = DataBaseAccess.GetAllShifts("");
                ddlShift.DataSource = list;
                ddlShift.DataBind();
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
                List<string> list = DataBaseAccess.GetAllPlants();
                list.Remove("All");
                ddlPlant.DataSource = list;
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindOperator()
        {
            try
            {
                List<string> list = DataBaseAccessPTA.getEmployeeForPlant(ddlPlant.SelectedValue);
                ddlOperator.DataSource = list;
                ddlOperator.DataBind();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        private void BindData()
        {
            try
            {
                DateTime fromDate = Util.GetDateTime("01-" + txtMonth.Text + "-" + txtYear.Text);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);

                List<OperatorEffyReportEntity> OprData = DataBaseAccessPTA.getOperatorEffyReport(fromDate.ToString("dd-MM-yyyy"), toDate.ToString("dd-MM-yyyy"), ddlOperator.SelectedValue, ddlShift.SelectedValue);
                Session["OprEffyData"] = OprData;
                List<double> lst_netusemin = new List<double>();
                List<double> lst_totMin = new List<double>();
                List<double> lst_BOEE = new List<double>();
                List<double> lst_NBtrMac = new List<double>();
                List<double> lst_BMark = new List<double>();
                List<double> lst_OprLoss = new List<double>();
                List<int> lst_alternaterow = new List<int>();

                double netloss = 0;
                int count = 0, macCount = 0;
                int rowstart = 0, rowend = 0, rowcount = 0;
                string shift = string.Empty;
                DateTime date = new DateTime(); ;

                System.Data.DataTable Dtab = new System.Data.DataTable();
                DataRow Drow;
                Dtab.Columns.Add("DateInString", typeof(string));
                Dtab.Columns.Add("Date", typeof(DateTime));
                Dtab.Columns.Add("Shift", typeof(string));
                Dtab.Columns.Add("Machine", typeof(string));
                Dtab.Columns.Add("ProdTime", typeof(string));
                Dtab.Columns.Add("DwnTime", typeof(string));
                Dtab.Columns.Add("Others", typeof(string));
                Dtab.Columns.Add("AE", typeof(string));
                Dtab.Columns.Add("PE", typeof(string));
                Dtab.Columns.Add("OEE", typeof(string));
                Dtab.Columns.Add("NUseMin", typeof(double));
                Dtab.Columns.Add("Totmin", typeof(double));
                Dtab.Columns.Add("BOEE", typeof(double));
                Dtab.Columns.Add("NbtrMachine", typeof(double));
                Dtab.Columns.Add("LosBMark", typeof(double));
                Dtab.Columns.Add("LosOpr", typeof(double));

                foreach (OperatorEffyReportEntity OprEffyData in OprData)
                {
                    #region date comparission, if dates are equal

                    if ((OprEffyData.Pdate == date))// && (OprEffyData.Shift == shift))
                    {
                        Drow = Dtab.NewRow();
                        Drow["Date"] = OprEffyData.Pdate.ToString("dd-MMM-yyyy");
                        Drow["DateInString"] = OprEffyData.Pdate.ToString("dd-MMM-yyyy");
                        Drow["Shift"] = OprEffyData.Shift;
                        Drow["Machine"] = OprEffyData.Machine;
                        Drow["ProdTime"] = OprEffyData.ProdTime;
                        Drow["DwnTime"] = OprEffyData.DwnTime;
                        Drow["Others"] = OprEffyData.Others;
                        Drow["AE"] = OprEffyData.AE;
                        Drow["PE"] = OprEffyData.PE;
                        Drow["OEE"] = OprEffyData.OE;

                        /* Net useful min */
                        lst_netusemin.Add((Convert.ToDouble(OprEffyData.ProdTime) * Convert.ToDouble(OprEffyData.PE)) / 100);
                        Drow["NUseMin"] = Math.Round(lst_netusemin[count], 0);

                        /* Total min */
                        lst_totMin.Add(Convert.ToDouble(OprEffyData.ProdTime) + Convert.ToDouble(OprEffyData.DwnTime));
                        Drow["Totmin"] = Math.Round(lst_totMin[count], 0);

                        /* Blended OEE */
                        if (lst_totMin.Sum() > 0)
                            lst_BOEE.Add(Math.Round((lst_netusemin.Sum() / lst_totMin.Sum()) * 100, 0));
                        else
                            lst_BOEE.Add(0.0);

                        /* Net benefit normalized to better machine */
                        /* maxTot is used to get the max value for TotalMinutes and to same to devided by the Useful min. */
                        int maxTot = lst_totMin.IndexOf(lst_totMin.Max());

                        if (lst_totMin.Sum() > 0)
                            // lst_NBtrMac.Add(Math.Round(((lst_netusemin.Sum() - lst_totMin[0]) / lst_totMin[0]) * 100, 0));
                            lst_NBtrMac.Add(Math.Round(((lst_netusemin.Sum() - lst_totMin[maxTot]) / lst_totMin[maxTot]) * 100, 0));
                        else
                            lst_NBtrMac.Add(0.0);

                        /* Net Loss from 80% benchmark (min) */
                        lst_BMark.Add((lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum()));

                        /* Net Loss (%) because of 1 operator less */
                        if (lst_totMin.Sum() > 0)
                            lst_OprLoss.Add(Math.Round((lst_BMark[count] / (lst_totMin.Sum() * 0.8)) * 100, 0));
                        else
                            lst_OprLoss.Add(0.0);

                        macCount++;
                        if (lst_alternaterow[rowcount - 1] == 0) lst_alternaterow.Add(0); else lst_alternaterow.Add(1);
                    }
                    #endregion

                    #region date comparission is not valid
                    else
                    {
                        date = OprEffyData.Pdate;
                        shift = OprEffyData.Shift;

                        if (macCount > 0)
                        {
                            if (count > 0)
                            {
                                Drow = Dtab.Rows[rowstart];
                                Drow["BOEE"] = Math.Round(lst_BOEE[count - 1], 0);
                                Drow["NBtrMachine"] = Math.Round(lst_NBtrMac[count - 1], 0);
                                Drow["LosBMark"] = Math.Round(lst_BMark[count - 1], 0);
                                Drow["LosOpr"] = Math.Round(lst_OprLoss[count - 1], 0);
                            }
                            else
                            {
                                Drow = Dtab.Rows[rowcount - 1];
                                Drow["BOEE"] = 0.0;
                                Drow["NBtrMachine"] = 0.0;
                                Drow["LosBMark"] = 0.0;
                                Drow["LosOpr"] = 0.0;
                            }
                            count = 0;
                            macCount = 0;
                            lst_netusemin.Clear();
                            lst_totMin.Clear();
                            lst_BOEE.Clear();
                            lst_NBtrMac.Clear();
                            lst_BMark.Clear();
                            lst_OprLoss.Clear();
                        }
                        else
                        {
                            count = 0;
                            lst_netusemin.Clear();
                            lst_totMin.Clear();
                            lst_BOEE.Clear();
                            lst_NBtrMac.Clear();
                            lst_BMark.Clear();
                            lst_OprLoss.Clear();
                        }

                        if (rowcount == 0)
                        {
                            Drow = Dtab.NewRow();
                            Drow["Date"] = OprEffyData.Pdate.ToString("dd-MMM-yyyy");
                            Drow["DateInString"] = OprEffyData.Pdate.ToString("dd-MMM-yyyy");
                            Drow["Shift"] = OprEffyData.Shift;
                            Drow["Machine"] = OprEffyData.Machine;
                            Drow["ProdTime"] = OprEffyData.ProdTime;
                            Drow["DwnTime"] = OprEffyData.DwnTime;
                            Drow["Others"] = OprEffyData.Others;
                            Drow["AE"] = OprEffyData.AE;
                            Drow["PE"] = OprEffyData.PE;
                            Drow["OEE"] = OprEffyData.OE;
                            //if (alternaterow == 0) alternaterow = 1; else alternaterow = 0;

                            //Drow = Dtab.NewRow();
                            /* Net useful min */
                            lst_netusemin.Add((Convert.ToDouble(OprEffyData.ProdTime) * Convert.ToDouble(OprEffyData.PE)) / 100);
                            Drow["NUseMin"] = Math.Round(lst_netusemin[count], 0);

                            /* Total min */
                            lst_totMin.Add(Convert.ToDouble(OprEffyData.ProdTime) + Convert.ToDouble(OprEffyData.DwnTime));
                            Drow["Totmin"] = Math.Round(lst_totMin[count], 0);

                            /* Blended OEE */
                            if (lst_totMin.Sum() > 0)
                                //Drow["BOEE"] = Math.Round((lst_netusemin.Sum() / lst_totMin.Sum()) * 100, 0);
                                lst_BOEE.Add(Math.Round((lst_netusemin.Sum() / lst_totMin.Sum()) * 100, 0));
                            else
                                lst_BOEE.Add(0.0);

                            /* Net benefit normalized to better machine */
                            if (lst_totMin.Sum() > 0)
                                // Drow["NBtrMachine"] = Math.Round(((lst_netusemin.Sum() - lst_totMin[count]) / lst_totMin[count]) * 100, 0);
                                lst_NBtrMac.Add(Math.Round(((lst_netusemin.Sum() - lst_totMin[0]) / lst_totMin[0]) * 100, 0));
                            else
                                lst_NBtrMac.Add(0.0);

                            /* Net Loss from 80% benchmark (min) */
                            //netloss = (lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum());
                            //Drow["LosBMark"] = Math.Round(netloss, 0);
                            lst_BMark.Add((lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum()));

                            /* Net Loss (%) because of 1 operator less */
                            if (lst_totMin.Sum() > 0)
                                lst_OprLoss.Add(Math.Round((lst_BMark[0] / (lst_totMin.Sum() * 0.8)) * 100, 0));
                            else
                                lst_OprLoss.Add(0.0);

                            if (lst_BOEE.Count > 0)
                                Drow["BOEE"] = Math.Round(lst_BOEE[count], 0);
                            else
                                Drow["BOEE"] = 0.0;

                            if (lst_NBtrMac.Count > 0)
                                Drow["NBtrMachine"] = Math.Round(lst_NBtrMac[count], 0);
                            else
                                Drow["NBtrMachine"] = 0.0;

                            if (lst_BMark.Count > 0)
                                Drow["LosBMark"] = Math.Round(lst_BMark[count], 0);
                            else
                                Drow["LosBMark"] = 0.0;

                            if (lst_OprLoss.Count > 0)
                                Drow["LosOpr"] = Math.Round(lst_OprLoss[count], 0);
                            else
                                Drow["LosOpr"] = 0.0;

                            //Dtab.Rows.Add(Drow);
                            lst_alternaterow.Add(0);
                        }
                        else
                        {
                            Drow = Dtab.NewRow();
                            //Drow["Date"] = string.Format("{0:dd-MMM-yyyy}", DateTime.Parse(OprEffyData.Pdate));
                            Drow["Date"] = OprEffyData.Pdate.ToString("dd-MMM-yyyy");
                            Drow["DateInString"] = OprEffyData.Pdate.ToString("dd-MMM-yyyy");
                            Drow["Shift"] = OprEffyData.Shift;
                            Drow["Machine"] = OprEffyData.Machine;
                            Drow["ProdTime"] = OprEffyData.ProdTime;
                            Drow["DwnTime"] = OprEffyData.DwnTime;
                            Drow["Others"] = OprEffyData.Others;
                            Drow["AE"] = OprEffyData.AE;
                            Drow["PE"] = OprEffyData.PE;
                            Drow["OEE"] = OprEffyData.OE;

                            //Drow = Dtab.NewRow();
                            /* Net useful min */
                            lst_netusemin.Add((Convert.ToDouble(OprEffyData.ProdTime) * Convert.ToDouble(OprEffyData.PE)) / 100);
                            Drow["NUseMin"] = Math.Round(lst_netusemin[count], 0);

                            /* Total min */
                            lst_totMin.Add(Convert.ToDouble(OprEffyData.ProdTime) + Convert.ToDouble(OprEffyData.DwnTime));
                            Drow["Totmin"] = Math.Round(lst_totMin[count], 0);

                            /* Blended OEE */
                            if (lst_totMin.Sum() > 0)
                                //Drow["BOEE"] = Math.Round((lst_netusemin.Sum() / lst_totMin.Sum()) * 100, 0);
                                lst_BOEE.Add(Math.Round((lst_netusemin.Sum() / lst_totMin.Sum()) * 100, 0));
                            else
                                lst_BOEE.Add(0.0);

                            /* Net benefit normalized to better machine */
                            if (lst_totMin.Sum() > 0)
                                // Drow["NBtrMachine"] = Math.Round(((lst_netusemin.Sum() - lst_totMin[count]) / lst_totMin[count]) * 100, 0);
                                lst_NBtrMac.Add(Math.Round(((lst_netusemin.Sum() - lst_totMin[0]) / lst_totMin[0]) * 100, 0));
                            else
                                lst_NBtrMac.Add(0.0);

                            /* Net Loss from 80% benchmark (min) */
                            //netloss = (lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum());
                            //Drow["LosBMark"] = Math.Round(netloss, 0);
                            lst_BMark.Add((lst_totMin.Sum()) * 0.8 - (lst_netusemin.Sum()));

                            /* Net Loss (%) because of 1 operator less */
                            if (lst_totMin.Sum() > 0)
                                lst_OprLoss.Add(Math.Round((lst_BMark[0] / (lst_totMin.Sum() * 0.8)) * 100, 0));
                            else
                                lst_OprLoss.Add(0.0);

                            //Dtab.Rows.Add(Drow);
                            if (lst_alternaterow[rowcount - 1] == 0) lst_alternaterow.Add(1); else lst_alternaterow.Add(0);

                            if (lst_BOEE.Count > 0)
                                Drow["BOEE"] = Math.Round(lst_BOEE[count], 0);
                            else
                                Drow["BOEE"] = 0.0;

                            if (lst_NBtrMac.Count > 0)
                                Drow["NBtrMachine"] = Math.Round(lst_NBtrMac[count], 0);
                            else
                                Drow["NBtrMachine"] = 0.0;

                            if (lst_BMark.Count > 0)
                                Drow["LosBMark"] = Math.Round(lst_BMark[count], 0);
                            else
                                Drow["LosBMark"] = 0.0;

                            if (lst_OprLoss.Count > 0)
                                Drow["LosOpr"] = Math.Round(lst_OprLoss[count], 0);
                            else
                                Drow["LosOpr"] = 0.0;
                        }
                        rowstart = rowcount;
                    }
                    #endregion

                    Dtab.Rows.Add(Drow);
                    int rc = Dtab.Rows.Count;

                    rowcount++;
                    count++;
                }
                lvOprData.DataSource = Dtab;
                lvOprData.DataBind();
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string ReportStatus = "";
                DateTime fromDate = Util.GetDateTime("01-" + txtMonth.Text + "-" + txtYear.Text);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);

                List<OperatorEffyReportEntity> OprData = Session["OprEffyData"] as List<OperatorEffyReportEntity>;
                PTAGenerateReport.GenerateOperatorEffyReport(OprData, ddlOperator.SelectedValue, fromDate.ToString("dd-MM-yyyy"), toDate.ToString("dd-MM-yyyy"));
                if (ReportStatus.Equals("Generated", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openSuccessModal(this, "Report Generated");
                else if (ReportStatus.Equals("NoData", StringComparison.OrdinalIgnoreCase))
                    HelperClassGeneric.openWarningToastrModal(this, "No data found");
                else
                    HelperClassGeneric.openWarningToastrModal(this, "Try again");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}