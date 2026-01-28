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
	public partial class Machine_Subsystem_Details : System.Web.UI.Page
	{
		public List<MachineSubsystemDetails> Machinesubsystemdata = new List<MachineSubsystemDetails>();
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["connectionString"] == null)
				Response.Redirect("~/SignIn.aspx", false);
			if (!IsPostBack)
			{
				if (Session["Language"] == null || Session["connectionString"] == null)
					Response.Redirect("~/SignIn.aspx", false);
				else
					SessionClear.ClearSession();
				Bindplantmachineid();
				BindGrid();
			}
		}

		private void Bindplantmachineid()
		{
			ddlplantid.DataSource = DataBaseAccess.GetAllPlants();
			ddlplantid.DataBind();
			ddlmachineid.DataSource = DataBaseAccess.getmachineidfromplant("");
			ddlmachineid.DataBind();


		}

		protected void ddlplantid_SelectedIndexChanged(object sender, EventArgs e)
		{
			string plantid = ddlplantid.SelectedValue;
			ddlmachineid.DataSource = DataBaseAccess.getmachineidfromplant(plantid);
			ddlmachineid.DataBind();
		}

		private void BindGrid()
		{
			Machinesubsystemdata = DataBaseAccess.Getmachinesubsystemdetails(ddlmachineid.SelectedValue);
			gridviewmachinesubsystem.DataSource = Machinesubsystemdata;
			gridviewmachinesubsystem.DataBind();
		}

		protected void btnview_Click(object sender, EventArgs e)
		{
			string plantid = ddlplantid.SelectedValue;
			string MachineID = ddlmachineid.SelectedValue;
			if (!string.IsNullOrEmpty(plantid) && !string.IsNullOrEmpty(MachineID))
			{
				Machinesubsystemdata = DataBaseAccess.Getmachinesubsystemdetails( MachineID);
				gridviewmachinesubsystem.DataSource = Machinesubsystemdata;
				gridviewmachinesubsystem.DataBind();
			}
		}

		protected void btnsave_Click(object sender, EventArgs e)
		{
			foreach (GridViewRow row in gridviewmachinesubsystem.Rows)
			{
				HiddenField hdfvalue = (HiddenField)row.FindControl("hiddenfield");
				if(hdfvalue.Value == "true")
				{
					string MachineID = ((Label)row.FindControl("lblmachine")).Text;
					string subsystems = ((Label)row.FindControl("lblsubsystem")).Text;
					string subsystem = DataBaseAccess.Getsubsystemid(subsystems);
					string Equip_ID = ((TextBox)row.FindControl("txtEquipID")).Text;
					string Equip_description = ((TextBox)row.FindControl("txtEquipdetails")).Text;
					if (!string.IsNullOrEmpty(MachineID) && !string.IsNullOrEmpty(subsystem) && !string.IsNullOrEmpty(Equip_ID) && !string.IsNullOrEmpty(Equip_description))
					{
						DataBaseAccess.savesubsystemdata(MachineID, subsystem, Equip_ID, Equip_description);
						lblmessage.Text = "Hurray!! Data Updated Successfully!!";
					}
					else
					{
						lblmessage.Text = ("Please Enter Equipment ID or Equipment Details!!");
					}
				}
				
			}
			BindGrid();
		}

		protected void btnupdate_Click(object sender, EventArgs e)
		{
			string MachineID = ((DropDownList)gridviewmachinesubsystem.FooterRow.FindControl("ddlmachineid")).Text;
			string subsystem = ((DropDownList)gridviewmachinesubsystem.FooterRow.FindControl("ddlsubsystem")).SelectedItem.Value;
			string Equip_ID = ((TextBox)gridviewmachinesubsystem.FooterRow.FindControl("txtfooterequipID")).Text;
			string Equip_description = ((TextBox)gridviewmachinesubsystem.FooterRow.FindControl("txtfooterequipmentdetails")).Text;
			if (!string.IsNullOrEmpty(MachineID) && !string.IsNullOrEmpty(subsystem) && !string.IsNullOrEmpty(Equip_ID) && !string.IsNullOrEmpty(Equip_description))
			{
				DataBaseAccess.savesubsystemdata(MachineID, subsystem, Equip_ID, Equip_description);
				lblmessage.Text=("Data Inserted");
			}
			
			BindGrid();
		}

		protected void btndelete_Click(object sender, EventArgs e)
		{

		}

		protected void gridviewmachinesubsystem_DataBound(object sender, EventArgs e)
		{
			DropDownList ddlmachine = gridviewmachinesubsystem.FooterRow.FindControl("ddlmachineid") as DropDownList;
			List<string> machineid = DataBaseAccess.getmachineidfromplant("");
			machineid.Remove("All");
			if (machineid != null && machineid.Count > 0)
			{
				ddlmachine.DataSource = machineid;
				ddlmachine.DataBind();
			}

			DropDownList ddlsubsystem = gridviewmachinesubsystem.FooterRow.FindControl("ddlsubsystem") as DropDownList;
			ddlsubsystem.DataTextField = "Categorydescription";
			ddlsubsystem.DataValueField = "categoryID";
			List<subsystem> subsystem = DataBaseAccess.Getsubsysteminfo();
			if (subsystem != null && subsystem.Count > 0)
			{
				ddlsubsystem.DataSource = subsystem;
				ddlsubsystem.DataBind();
			}
		}		
	}
}