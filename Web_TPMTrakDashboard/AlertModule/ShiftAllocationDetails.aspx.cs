using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.EnergyModule;

namespace Web_TPMTrakDashboard.AlertModule
{
    public partial class ShiftAllocationDetails : System.Web.UI.Page
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
                    hidPlantID.Value = cmbPlantID.Text.ToString();
                    BindOperator();
                


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
                Logger.WriteErrorLog(ex.Message);
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
                Logger.WriteErrorLog(ex.Message);
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<ShiftAllocationEntityData> GetData(string PlantID,string fromDate)
        {
            List<ShiftAllocationEntityData> Entities = new List<ShiftAllocationEntityData>();
            try
            {
                DateTime FromDate = Util.GetDateTime(fromDate);
                DateTime Todate = FromDate.AddDays(7);
                List<string> Shift = DataBaseAccess.shiftdetail();
                DataTable dt = DataBaseAccess.getAlertShiftAllocation(PlantID, FromDate, "View");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DateTime fromdate = FromDate;
                    List<DateTime> dates = new List<DateTime>();
                    while (!(fromdate == Todate))
                    {
                        dates.Add(fromdate); fromdate = fromdate.AddDays(1);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            ShiftAllocationEntityData entity = new ShiftAllocationEntityData();
                            entity.SLNO = "SLNO";
                            entity.Consumer = "Consumer";
                            entity.EmailId = "EmailId";
                            entity.PhoneNo = "PhoneNo";
                            List<DateShift> Dateshiftlist = new List<DateShift>();
                            for (int j = 0; j < dates.Count; j++)
                            {
                                Dateshiftlist.Add(new DateShift { Date = dates[j].ToString("yyyy-MM-dd"), Shift = Shift });
                            }
                            entity.DateShift = Dateshiftlist;
                            Entities.Add(entity);
                        }
                        ShiftAllocationEntityData Ent = new ShiftAllocationEntityData();
                        Ent.SLNO = Convert.ToString(i + 1);
                        Ent.Consumer = dt.Rows[i]["Consumer"].ToString();
                        Ent.EmailId = dt.Rows[i]["Email"].ToString();
                        Ent.PhoneNo = dt.Rows[i]["Phone"].ToString();
                        
                        int k = 3;
                        List<DateShift> Dateshiftlist1 = new List<DateShift>();
                        for (int j = 0; j < dates.Count; j++)
                        {
                            List<string> checkList = new List<string>();
                            int count = 0;
                            while (count<Shift.Count)
                            {
                                checkList.Add(dt.Rows[i][k].ToString());
                                k++;count++;
                            }
                            Dateshiftlist1.Add(new DateShift { Date = dates[j].ToString("yyyy-MM-dd"), Shift=Shift ,Checked=checkList });
                        }
                        Ent.DateShift = Dateshiftlist1;
                        Entities.Add(Ent);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return Entities;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void DeleteShiftAllocation(string FromDate, string ToDate, string Consumer)
        {
            try
            {
                DataBaseAccess.DeleteAlertShiftAllocation(FromDate, ToDate, Consumer);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }



        [System.Web.Services.WebMethod(EnableSession = true)]
        public static int saveDetails(ShiftAllocationEntityData Data)
        {
            int result = 0;
            try
            {
                string shiftID = DataBaseAccess.GetShiftID(Data.ShiftID);
                DataBaseAccess.SaveAlertUserShiftAllocation(Data.ShiftDate,shiftID, Data.Consumer);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            return result;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string PreviousClick(string FromDate, string Operator)
        {
            DateTime fromdate = Util.GetDateTime(FromDate);
            fromdate = DataBaseAccess.GetUserShiftAllocationDate(fromdate, "Prev", Operator);
            return fromdate.ToString("dd-MM-yyyy");
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string NextClick(string FromDate, string Operator)
        {
            DateTime fromdate = Util.GetDateTime(FromDate);
            fromdate = DataBaseAccess.GetUserShiftAllocationDate(fromdate, "Next", Operator);
            return fromdate.ToString("dd-MM-yyyy");
        }
    }

    public class ShiftAllocationEntityData
    {
        public string SLNO { get; set; }
        public string PlantID { get; set; }
        public string Consumer { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public List<DateShift> DateShift { get; set; }
        public string ShiftID { get; set; }
        public string ShiftDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        
    }

    public class DateShift
    {
        public string Date { get; set; }
        public List<string> Shift { get; set; }
        public List<string> Checked { get; set; }
    }
}