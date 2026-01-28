using SonaAndon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard;

namespace SonaAndon
{
	public partial class SonaAndon : System.Web.UI.Page
	{
		List<SonaAndonEntity> Sonadata = new List<SonaAndonEntity>();
		int rows = 0;
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
				Bindgrid();
				timer.Interval = ConnectionManager.refreshData;
				timer.Enabled = true;
			}
		}

		private void Bindgrid()
		{
			string ShiftStart = "", ShiftEnd = "";
			int remainder = 0;
			List<fontstyling> data = new List<fontstyling>();
			data = DataBase.GetFontstyling();
			fontsize = data[0].fontsize;
			fontfamily = data[0].fontfamily;
			fontstyle = data[0].fontstyle;
			Headerfontsize = data[0].Headerfontsize;
			rows = data[0].Rows;
			Alternativebackground = data[0].AlternativeRowColor;
			Headerbackgroundcolor = data[0].HeaderColor;
			background = data[0].RowColor;
			ShiftStart = DataBase.GetShiftstart(out ShiftEnd);
			ShiftStart = Convert.ToDateTime(ShiftStart).ToString("yyyy-MM-dd HH:mm:ss");
			Sonadata = DataBase.GetSonaAndondata(ShiftStart);
            Label lbldate = (Label)Master.FindControl("lbldatetime");
            lbldate.Text = DateTime.Now.ToString("HH:mm, dd'th' MMMM yyyy");
            if (Sonadata != null && Sonadata.Count > 0)
			{
				count = Sonadata.Count;
                remainder = count % rows;
				flips = remainder != 0 ? ((count / rows)+1) : (count / rows);
				rowstotake = 1;
				IEnumerable<SonaAndonEntity> datas = Sonadata.Take(rows * rowstotake);
				listviewSonaAndon.DataSource = datas;
				listviewSonaAndon.DataBind();
				Session["Flips"] = flips;
				Session["SonaData"] = Sonadata;
				Session["rowstotake"] = rowstotake;
				Session["Rows"] = rows;
				rowstotake++;
			}
		}

		protected void timer_Tick(object sender, EventArgs e)
		{
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
				Sonadata = (List<SonaAndonEntity>)Session["SonaData"];
				int skiprows = rows * rowstotake;
				//Sonadata.Skip(skiprows);
				IEnumerable<SonaAndonEntity> data = Sonadata.Skip(skiprows).Take(rows);
				listviewSonaAndon.DataSource = Sonadata.Skip(skiprows).Take(rows);
				listviewSonaAndon.DataBind();
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
	}

	public class SonaAndonEntity
	{
		public string WorkCenter { get; set; }
		public string PartNumber { get; set; }
		public string Scheduleqty { get; set; }
		public string Target { get; set; }
		public string Completedtime { get; set; }
		public string OEE { get; set; }
		public string Status { get; set; }
		public string OKNOT { get; set; }
		public string Image { get; set; }
		public string Visibility { get; set; }
		public string MachineDescription { get; set; }
		public string DownReason { get; set; }
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