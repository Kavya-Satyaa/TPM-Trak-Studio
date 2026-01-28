using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Cumi.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.Cumi
{
    public partial class SPCTargetMasterCumi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtYear.Text = DateTime.Now.ToString("yyyy");
                BindData();
            }
        }
        private void BindData()
        {
            try
            {
                List<TargetMasterCumi> list = new List<TargetMasterCumi>();
                DataTable dt = CumiDBAccess.GetTargetDetails_Cumi(txtYear.Text);
                for (int i = 1; i <= 12; i++)
                {
                    TargetMasterCumi data = new TargetMasterCumi();
                    DateTime date = new DateTime(Convert.ToInt32(txtYear.Text), i, 1);
                    data.Month = date.ToString("MMMM");
                    data.MonthInInt = i.ToString();
                    if (dt.Rows.Count > 0)
                    {
                        data.TargetValue = dt.AsEnumerable().Where(k => k.Field<dynamic>("TargetDate") == date).Select(k => k.Field<double>("SPCTargetValue")).FirstOrDefault().ToString();
                    }
                    list.Add(data);
                }
                lvData.DataSource = list;
                lvData.DataBind();
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
                TargetMasterCumi data = new TargetMasterCumi();
                int result = 0;
                for (int i = 0; i < lvData.Items.Count; i++)
                {
                    var row = lvData.Items[i];
                    string month = (row.FindControl("hdnMonthInInt") as HiddenField).Value;
                    DateTime targetDate = new DateTime(HelperClassGeneric.getIntValueFromString(txtYear.Text), HelperClassGeneric.getIntValueFromString(month), 1);
                    string targetValue = (row.FindControl("txtTargetValue") as TextBox).Text;
                    result += CumiDBAccess.SaveSPCTargetMasterDetails(targetDate, targetValue);
                }
                if (result > 0)
                {
                    Bajaj.Model.HelperClass.openInsertSuccessModal(this);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
    }
}