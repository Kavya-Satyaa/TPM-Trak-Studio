using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
	public partial class CockpitAggregateData : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(!IsPostBack)
			{
				if(Session["FromDate"]!=null && Session["ToDate"] != null)
				{
					txtfromdate.Text = Convert.ToDateTime(Session["FromDate"].ToString()).ToString("yyyy-MM-dd"); ;
					txttodate.Text = Convert.ToDateTime(Session["ToDate"].ToString()).ToString("yyyy-MM-dd"); ;
				}
				else
				{
					txttodate.Text = DateTime.Now.ToString("yyyy-MM-dd"); 
					txtfromdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
				}
				bindshift();bindmachine();bindplant();
				bindgrid(txtfromdate.Text, txttodate.Text, ddlshift.SelectedValue.ToString(), ddlPlantID.SelectedValue.ToString(), ddlMachineID.SelectedValue.ToString());
			}
		}

		private void bindgrid(string fromdate, string todate, string shiftname, string plantid, string machineid)
		{
			DateTime FromDate = Util.GetDateTime(fromdate);
			DateTime ToDate = Util.GetDateTime(todate);
			shiftname = shiftname == "All" ? "" : shiftname;
			DataTable dt = new DataTable();
			plantid = plantid == "All" ? "" : plantid;
			machineid = machineid == "All" ? "" : machineid;
			List<string> ColHeader = new List<string>();
			List<string> ColProperty = new List<string>();
			//ColHeader = DataBaseAccess.GetCockpitdetails(out ColProperty);
			//for(int i=0;i<ColHeader.Count;i++)
			//{
			//	BoundField boundField = new BoundField();
			//	boundField.DataField = ColProperty[i].ToString();
			//	boundField.HeaderText = ColHeader[i].ToString();
			//	gridviewaggregation.Columns.Add(boundField);
			//}
			dt = DataBaseAccess.getcockpitaggregatedata(FromDate, ToDate, plantid, machineid, shiftname);
			//if(dt!=null && dt.Rows.Count>0)
			//{
			//	gridviewaggregation.DataSource = dt;
			//	gridviewaggregation.DataBind();
			//}
			lstviewcockpitaggregate.DataSource = dt;
			lstviewcockpitaggregate.DataBind();
		}

		private void bindplant()
		{
			List<string> plantid = BindCockpitView.ViewPlantToDisplay();
			if(plantid != null && plantid.Count>0)
			{
				plantid.Insert(0, "All");
				ddlPlantID.DataSource = plantid;
				ddlPlantID.DataBind();
			}
		}

		private void bindmachine()
		{
			List<string> machineid = VDGDataBaseAccess.GetAllMachines(ddlPlantID.SelectedValue.ToString());
			if(machineid != null && machineid.Count>0)
			{
				machineid.Insert(0, "All");
				ddlMachineID.DataSource = machineid;
				ddlMachineID.DataBind();
			}
		}

		private void bindshift()
		{
			List<string> shiftname = BindCockpitView.GetAllShift();
			if(shiftname !=null && shiftname.Count>0)
			{
				shiftname.Insert(0, "All");
				ddlshift.DataSource = shiftname;
				ddlshift.DataBind();
			}
		}

		protected void btnsearch_Click(object sender, EventArgs e)
		{
			bindgrid(txtfromdate.Text,txttodate.Text,ddlshift.SelectedValue.ToString(),ddlPlantID.SelectedValue.ToString(),ddlMachineID.SelectedValue.ToString());
		}

		protected void ddlPlantID_SelectedIndexChanged(object sender, EventArgs e)
		{
			bindmachine();
		}
	}
}