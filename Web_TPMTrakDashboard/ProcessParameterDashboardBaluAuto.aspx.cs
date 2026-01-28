using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.ProcessParameter;

namespace Web_TPMTrakDashboard
{
    public partial class ProcessParameterDashboardBaluAuto : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindListview();
            }
        }

        private void BindListview()
        {
            try
            {
                DataTable Header = new DataTable();
                DataTable ParameterData = ProcessParameterDataBaseAccess.GetProcessParameterData(out Header);
                Session["ProcessParameterHeader"] = Header;
                if (ParameterData != null)
                {
                    gridProcessparamwter.DataSource = ParameterData;
                    gridProcessparamwter.DataBind();
                    //timer1.Enabled = true;
                    //timer1.Interval = 10000;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }

        }

        public void BindGrid()
        {
            DataTable Header = new DataTable();
            DataTable ParameterData = ProcessParameterDataBaseAccess.GetProcessParameterData(out Header);
            Session["ProcessParameterHeader"] = Header;
            if (ParameterData != null)
            {
                gridProcessparamwter.DataSource = ParameterData;
                gridProcessparamwter.DataBind();
            }
        }

        protected void gridProcessparamwter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Session["ProcessParameterHeader"] != null)
                {
                    DataTable Header = Session["ProcessParameterHeader"] as DataTable;
                    for (int i = 0; i < Header.Rows.Count; i++)
                    {
                        string headername = "txtsp" + (i + 1);
                        ((Label)e.Row.FindControl(headername) as Label).Text = Header.Rows[0]["ParameterName"].ToString();
                    }
                    for(int i=(5+Header.Rows.Count);i<gridProcessparamwter.Columns.Count;i++)
                    {
                        gridProcessparamwter.Columns[i].Visible = false;
                        gridProcessparamwter.Columns[i].ItemStyle.Width = 0;
                    }
                }
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            //BindGrid();
        }
    }
}