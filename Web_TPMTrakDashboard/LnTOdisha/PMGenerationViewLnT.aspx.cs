using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.LnTOdisha.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.LnTOdisha
{
    public partial class PMGenerationViewLnT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                DateTime now = DateTime.Now;
                now = new DateTime(now.Year, now.Month, 1);
                txtFromDate.Text = now.ToString("dd-MM-yyyy");
                txtToDate.Text = now.AddMonths(1).AddSeconds(-1).ToString("dd-MM-yyyy");
                BindMachineIds();
                BindActivityTimingData();
            }
        }
        private void BindMachineIds()
        {
            try
            {
                List<string> machines = DataBaseAccess.GetMachineInfoForPM();
                ddlMachineID.DataSource = machines;
                ddlMachineID.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void BindActivityTimingData()
        {
            try
            {
                DataTable dt = LnTOdishaDBAccess.getPMGenerationActivityDetails(txtYear.Text, txtFromDate.Text, txtToDate.Text, ddlMachineID.SelectedValue);
                DataTable newDt = new DataTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow newRow = newDt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string columnName = dt.Columns[j].ColumnName;
                        string rowValue = "";
                        if (i == 0)
                        {
                            newDt.Columns.Add(columnName, typeof(string));
                        }

                        if (j >= 9)
                        {
                            string date = dt.Rows[i][j].ToString();
                            if (date != "" && date != null)
                            {
                                rowValue = Util.GetDateTime(date).ToString("dd-MM-yyyy");
                            }
                        }
                        else
                        {
                            rowValue = dt.Rows[i][j].ToString();
                        }
                        newRow[columnName] = rowValue;
                    }
                    newDt.Rows.Add(newRow);
                }
                gvActvityTiming.DataSource = newDt;
                gvActvityTiming.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            BindActivityTimingData();
        }

    }
}