using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;
using Web_TPMTrakDashboard.ShantiIron.Model;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class ShantiAndon : System.Web.UI.Page
    {
        List<ShantiAndonEntity> Shantidata = new List<ShantiAndonEntity>();
        int rows = 6;
        int count = 0;
        int flips = 0;
        int rowstotake = 0;
        public int fontsize = 19;
        public int Headerfontsize = 20;
        public string fontfamily = "";
        public string fontstyle = "";
        public string Headerbackgroundcolor = "";
        public string Alternativebackground = "";
        public string background = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlantID();
                Bindgrid();
                timer.Interval = Utility.ShantiFlipInterval;
                timer.Enabled = true;
            }
        }
        private void BindPlantID()
        {
            List<string> lineIdList = DataBaseAccess.GetAllPlants();
            lineIdList.RemoveAt(0);
            ddlPlantID.DataSource = lineIdList;
            ddlPlantID.DataBind();
        }
        private void Bindgrid()
        {
            string ShiftStart = "", ShiftEnd = "";
            int remainder = 0;
            List<fontstyling> data = new List<fontstyling>();
            // data = DataBaseAccess.GetFontstyling();
            // fontsize = data[0].fontsize;
            // fontfamily = data[0].fontfamily;
            //fontstyle = data[0].fontstyle;
            // Headerfontsize = data[0].Headerfontsize;
            // rows = data[0].Rows;
            // Alternativebackground = data[0].AlternativeRowColor;
            //Headerbackgroundcolor = data[0].HeaderColor;
            // background = data[0].RowColor;
            rows = Utility.NoOfRows;
            Headerfontsize = Utility.ShantiHeaderFontSize;
            fontsize = Utility.ShantiContentFontSize;
            ShiftStart = ShantiDataBaseAccess.GetShiftstart(out ShiftEnd);
            ShiftStart = Convert.ToDateTime(ShiftStart).ToString("yyyy-MM-dd HH:mm:ss");
            ShiftEnd = Convert.ToDateTime(ShiftEnd).ToString("yyyy-MM-dd HH:mm:ss");
            string plant = ddlPlantID.SelectedValue == null ? "" : ddlPlantID.SelectedValue;
            Shantidata = ShantiDataBaseAccess.GetShantiAndondata(ShiftStart, ShiftEnd, plant);

            lblDatetime.InnerText = "Refresh Time: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
            if (Shantidata != null && Shantidata.Count > 0)
            {
                count = Shantidata.Count;
                remainder = count % rows;
                flips = remainder != 0 ? ((count / rows) + 1) : (count / rows);
                rowstotake = 1;
                IEnumerable<ShantiAndonEntity> datas = Shantidata.Take(rows * rowstotake);
                listviewShantiAndon.DataSource = datas;
                listviewShantiAndon.DataBind();
                Session["Flips"] = flips;
                Session["ShantiData"] = Shantidata;
                Session["rowstotake"] = rowstotake;
                Session["Rows"] = rows;
                rowstotake++;
            }
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            lblDatetime.InnerText = "Refresh Time: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
            if (Session["Flips"] == null)
            {
                timer.Enabled = false;
                return;
            }
            int.TryParse(Session["Flips"].ToString(), out flips);
            if (flips > 1)
            {
                int.TryParse(Session["rowstotake"].ToString(), out rowstotake);
                int.TryParse(Session["Rows"].ToString(), out rows);
                Shantidata = (List<ShantiAndonEntity>)Session["ShantiData"];
                int skiprows = rows * rowstotake;
                //Sonadata.Skip(skiprows);
                IEnumerable<ShantiAndonEntity> data = Shantidata.Skip(skiprows).Take(rows);
                listviewShantiAndon.DataSource = Shantidata.Skip(skiprows).Take(rows);
                listviewShantiAndon.DataBind();
                flips--;
                rowstotake++;
                Session["Flips"] = flips;
                Session["rowstotake"] = rowstotake;
            }
            else
            {
                Bindgrid();
            }
        }

        protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer.Enabled = false;
            Bindgrid();
            timer.Interval = Utility.ShantiFlipInterval;
            timer.Enabled = true;
        }
    }

    public class ShantiAndonEntity
    {
        public string MachineID { get; set; }
        public string MachineDescription { get; set; }
        public string RunningComponent { get; set; }
        public string RunningOpn { get; set; }
        public string RunningSlNo { get; set; }
        public string HeatCode { get; set; }
        public string CurrentOperator { get; set; }
        public string AvgCycletime { get; set; }
        public string AvgLoadunload { get; set; }
        public string AE { get; set; }
        public string PE { get; set; }
        public string QE { get; set; }
        public string OEE { get; set; }
        public string RejectCount { get; set; }
        public string Components { get; set; }
        public string UtilisedTime { get; set; }
    }
    public class fontstyling
    {
        public int fontsize { get; set; }
        public string fontstyle { get; set; }
        public string fontfamily { get; set; }
        public int Rows { get; set; }
        public int Headerfontsize { get; set; }
        public string HeaderColor { get; set; }
        public string RowColor { get; set; }
        public string AlternativeRowColor { get; set; }
    }
}