using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using Web_TPMTrakDashboard.Models;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard
{
	public partial class ProcessParameterConfig : System.Web.UI.Page
	{
		int processid = 0;
		int rowCount = 0;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindComponent();
				BindParameter();
				BindMachineId();
				BindGrid();
			}
		}

		private void BindParameter()
		{
            cmbpara.Items.Add("Temperature");
            cmbpara.Items.Add("Pressure");
		}

		private void BindGrid()
		{
			try
			{
				List<processparaconfig> datapro = new List<processparaconfig>();
				datapro = DataBaseAccess.GetProcessMasterData();
				GridProcessPara.DataSource = datapro;
				GridProcessPara.DataBind();
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
			}
		}

		private void BindMachineId()
		{
			List<string> lstMachine = DataBaseAccess.GetAllMachines("");
			cmbmachine.DataSource = lstMachine;
			cmbmachine.DataBind();
			if (lstMachine.Count > 0) cmbmachine.SelectedIndex = 0;
		}

		private void BindComponent()
		{
			cmbComponent.DataSource = null;
			List<string> compIds = DataBaseAccess.GetComponentId();
			cmbComponent.DataSource = compIds;
			cmbComponent.DataBind();
			//if (compIds.Count == 0) cmbComponent.SelectedIndex = 0;
		}


		protected void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				int i = GridProcessPara.SelectedIndex;
                if(i!=-1)
                {
                    processid = Convert.ToInt32(((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lblSLno")).Text);
                }
                
				
                else
                {
				    i = GridProcessPara.Rows.Count-1;
                    processid = Convert.ToInt32(((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lblSLno")).Text);
                    processid++;
                }
				int rowCount = 0;
				if (string.IsNullOrWhiteSpace(txtlowerr.Text.ToString()))
				{
					lblMessages.Text= "Please enter the LSL !!";
					return;
				}
				if (string.IsNullOrWhiteSpace(txtupperr.Text.ToString()))
				{
					lblMessages.Text = "Please enter the USL !!";
					return;
				}
				if (string.IsNullOrWhiteSpace(txtlowop.Text.ToString()))
				{
					lblMessages.Text ="Please enter the Lower Operating Zone Limit !!";
					return;
				}
				if (string.IsNullOrWhiteSpace(txtupoper.Text.ToString()))
				{
					lblMessages.Text ="Please enter the Please enter the Upper Operating Zone Limit !!";
					return;
				}
				if (string.IsNullOrWhiteSpace(txtlowwar.Text.ToString()))
				{
					lblMessages.Text ="Please enter the Lower Warning Zone Limit !!";
					return;
				}
				if (string.IsNullOrWhiteSpace(txtupwar.Text.ToString()))
				{
					lblMessages.Text ="Please enter the Upper Warning Zone Limit !!";
					return;
				}
				if (Convert.ToDecimal(txtlowerr.Text) > Convert.ToDecimal(txtupperr.Text))
				{
					lblMessages.Text ="LSL Limit must be lesser than USL";
					return;
				}
				if (Convert.ToDecimal(txtlowop.Text) > Convert.ToDecimal(txtupoper.Text))
				{
					lblMessages.Text ="Upper Operating Zone Limit must be lesser than Lower Operating Zone Limit.";
					return;
				}
				if (Convert.ToDecimal(txtlowwar.Text) > Convert.ToDecimal(txtupwar.Text))
				{
					lblMessages.Text ="Upper Warning Zone Limit must be lesser than Lower Warning Zone Limit";
					return;
				}
				if ((Convert.ToDecimal(txtupperr.Text) < Convert.ToDecimal(txtupoper.Text)) || (Convert.ToDecimal(txtupperr.Text) < Convert.ToDecimal(txtupwar.Text)))
				{
					lblMessages.Text ="Upper Warning Zone Limit and Upper Operating Zone Limit must be lesser than USL";
					return;
				}
				if ((Convert.ToDecimal(txtlowop.Text) < Convert.ToDecimal(txtlowerr.Text)) || (Convert.ToDecimal(txtlowop.Text) < Convert.ToDecimal(txtlowwar.Text)))
				{
					lblMessages.Text ="LSL and Lower Warning Zone Limit must be lesser than Lower Operating Zone Limit";
					return;
				}
				if (Convert.ToDecimal(txtlowop.Text) < Convert.ToDecimal(txtlowwar.Text))
				{
					lblMessages.Text ="Lower Operating Zone Limit must be lesser than Lower Warning Zone Limit.";
					return;
				}
				if (Convert.ToDecimal(txtupwar.Text) < Convert.ToDecimal(txtupoper.Text))
				{
					lblMessages.Text ="Upper Operating Zone Limit must be lesser thsan Upper Warning Zone Limit.";
					return;
				}
				if (Convert.ToDecimal(txtlowwar.Text) > Convert.ToDecimal(txtupoper.Text))
				{
					lblMessages.Text = "Lower Warning Zone Limit must be lesser than Upper Operating Zone Limit.";
					return;
				}
				DataBaseAccess.SaveProcessMasterData(processid, cmbmachine.Text, cmbpara.Text, cmbComponent.Text, txtlowerr.Text, txtupperr.Text, txtlowop.Text, txtupoper.Text, txtlowwar.Text, txtupwar.Text, out rowCount);
				if (rowCount > 0)
				{
					lblMessages.Text ="Details added / Updated successfully.";
					BindGrid();
					ResetValue();
				}
				else
				{
					lblMessages.Text ="Records didnot get Updated .";
				}
				GridProcessPara.SelectedIndex = -1;
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.Message);
			}
		}

		private void ResetValue()
		{
			processid = 0;
			cmbComponent.SelectedIndex = 0;
			cmbmachine.SelectedIndex = 0;
			cmbmachine.SelectedIndex = 0;
			txtlowerr.Text = string.Empty;
			txtlowop.Text = string.Empty;
			txtlowwar.Text = string.Empty;
			txtupoper.Text = string.Empty;
			txtupperr.Text = string.Empty;
			txtupwar.Text = string.Empty;
		}

		protected void btndelete_Click(object sender, EventArgs e)
		{
			int i = GridProcessPara.SelectedIndex;
			GridViewRow GWR = GridProcessPara.SelectedRow;
			processid = Convert.ToInt32(((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lblSLno")).Text);
			if(!(string.IsNullOrEmpty(txtlowerr.Text)) && !(string.IsNullOrEmpty(txtlowop.Text)) && !(string.IsNullOrEmpty(txtlowwar.Text)) && !(string.IsNullOrEmpty(txtupoper.Text)) && !(string.IsNullOrEmpty(txtupperr.Text)) && !(string.IsNullOrEmpty(txtupwar.Text)))
			{
				DataBaseAccess.deleteProcessMasterData(processid, out rowCount);
				if (rowCount > 0)
				{
					lblMessages.Text = "Records deleted successfully.";
					BindGrid();
				}
				else
				{
					lblMessages.Text = "Records not deleted.";
				}
			}
			else
			{
				lblMessages.Text = "No data to deleted.";
			}
			txtupwar.Text = string.Empty;
			txtupperr.Text = string.Empty;
			txtupoper.Text = string.Empty;
			txtlowwar.Text = string.Empty;
			txtlowop.Text = string.Empty;
			txtlowerr.Text = string.Empty;
			GridProcessPara.SelectedIndex = -1;
		}

		protected void changedindex(object sender, EventArgs e)
		{			
				try
				{
					int i = GridProcessPara.SelectedIndex;
					processid = Convert.ToInt32(((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lblSLno")).Text);
					cmbmachine.Text = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lblMachineID")).Text;
					cmbpara.Text = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lblParameter")).Text;
					cmbComponent.SelectedValue = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lblComponent")).Text;
					txtlowerr.Text = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lbllezl")).Text;
					txtupperr.Text = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lbluezl")).Text;
					txtlowop.Text = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lbllozl")).Text;
					txtupoper.Text = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lbluozl")).Text;
					txtlowwar.Text =( (Label)GridProcessPara.Rows[i].Cells[1].FindControl("lbllwzl")).Text;
					txtupwar.Text = ((Label)GridProcessPara.Rows[i].Cells[1].FindControl("lbluwzl")).Text;				
				}
				catch (Exception ex)
				{
					Logger.WriteErrorLog(ex.Message);
				}
			

		}
	}
}