using BusinessClassLibrary;
using ChartDirector;
using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.PTA
{
	public partial class MachineUpDownTime : System.Web.UI.Page
	{
		XYChart c = null;
		string[] arMac = new string[12];
		string frmDate, todate;
		string shiiftName;

		DateTime[] LogicalStartEnd = new DateTime[2];
		string machineIDs = string.Empty;
		List<string> machines = new List<string>();

		int runStoredProcFirstTime = 0;
		DataTable dtPDT = null;
		DataView dvPDT = null;
		DataView dvICD = null;
		DataTable dtICD = null;
		DataView dvML = null;
		DataTable dtML = null;
		int machineIndex = 0, iniMachineIndex = 6;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
				BindPlantId();
				BindShiftData();
				SelectAllMAchines();

				machineIndex = 0;
				if (Session["ScreenHeight"] != null)
					iniMachineIndex = (Convert.ToInt32(Session["ScreenHeight"])) / 100;

				iniMachineIndex = ddlMachineId.Items.Count > iniMachineIndex ? iniMachineIndex : ddlMachineId.Items.Count;
				ViewState["machineIndex"] = machineIndex;
				ViewState["iniMachineIndex"] = iniMachineIndex;
				createPTAchart();
			}
		}

		private void SelectAllMAchines()
		{
			try
			{
				foreach (ListItem item in ddlMachineId.Items)
				{
					item.Selected = true;
				}
			}
			catch (Exception ex)
			{
				Logger.WriteDebugLog(ex.ToString());
			}
		}

		public void getshiftTime()
		{
			string shiftdetailsStartTime, ShiftStartDateTime;
			try
			{
				shiftdetailsStartTime = string.Empty;
				ShiftStartDateTime = string.Empty;

				if (ddlShift.SelectedIndex == 0)
				{
					frmDate = DataBaseAccessPTA.CurrentStartEndTime(DateTime.ParseExact(string.Format("{0} {1}", txtFromDate.Text, DateTime.Now.ToString("HH:mm:ss")), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss"), out todate, out shiiftName);
				}
				else
				{
					shiftdetailsStartTime = DataBaseAccessPTA.GetshiftTime(ddlShift.Text);

					if (ddlShift.SelectedIndex == 3)
					{
						ShiftStartDateTime = DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddDays(1).ToString("yyyy-MM-dd");
					}
					else
					{
						ShiftStartDateTime = DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
					}
					ShiftStartDateTime = ShiftStartDateTime + " " + shiftdetailsStartTime;
					frmDate = DataBaseAccessPTA.CurrentStartEndTime(ShiftStartDateTime, out todate, out shiiftName);
				}
			}
			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.ToString());
			}
			finally
			{

			}
		}

		private void createPTAchart()
		{

			machineIndex = (int)ViewState["machineIndex"];
			iniMachineIndex = (int)ViewState["iniMachineIndex"];

			DataTable dt = null;
			double d = 1.0;
			runStoredProcFirstTime = 0;
			getshiftTime();


			c = new XYChart(Convert.ToInt32(Session["ScreenWidth"]) - 200, Convert.ToInt32(Session["ScreenHeight"]) - 280, 0xe0e0ff, 0x000000, 1);

			//c.setRoundedFrame();

			c.setPlotArea(80, 10, Convert.ToInt32(Session["ScreenWidth"]) - 300, Convert.ToInt32(Session["ScreenHeight"]) - 320, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);


			//LogicalStartEnd = DataBaseAccessPTA.GetLogicalStartEnd(txtFromDate.Text);
			//c.xAxis().setDateScale(LogicalStartEnd[0], LogicalStartEnd[1], 30 * 60);
			//c.xAxis().setLabelFormat("{value|hh:nn:ss}");
			c.xAxis().setLabelFormat("{value|hh:nn}");

			//c.xAxis().setLabelFormat("{value|dd-MMM-yy hh:nn:ss}");

			//hiding the Y Label Format
			c.yAxis().setLabelFormat("");

			#region multipleMachine		

			int rec = 0;
			int Drec = 0;

			machineIDs = string.Empty;

			foreach (ListItem item in ddlMachineId.Items)
			{
				if (item.Selected)
				{
					machines.Add(item.Value);
				}
			}

			machineIDs = string.Empty;
			List<string> labels = null;


			if (ViewState["IsPrev"] != null && (bool)ViewState["IsPrev"] == true)
			{
				if (ViewState["PrevIndex"] != null)
					machineIndex = (int)ViewState["PrevIndex"] - iniMachineIndex;
			}
			if (machineIndex == 0)
				btnPrevious.Enabled = false;
			else
				btnPrevious.Enabled = true;

			var tempList = machines.Skip(machineIndex).Take(iniMachineIndex).ToList();
			for (int j = 0; j < tempList.Count; j++)
			{
				if (tempList[j] == null || tempList[j] == string.Empty)
				{
					machineIDs = machineIDs;
				}
				else
				{
					machineIDs = machineIDs + "'" + tempList[j].ToString() + "',";
				}
			}

			labels = new List<string>(tempList);
			labels.Insert(0, string.Empty);
			labels.Insert(labels.Count, string.Empty);


			ViewState["PrevIndex"] = machineIndex;
			btnNext.Enabled = true;
			if ((machines.Count - machineIndex) > iniMachineIndex)
			{
				machineIndex += machines.Skip(machineIndex).Take(iniMachineIndex).ToArray().Length;
			}
			else
			{
				machineIndex += machines.Skip(machineIndex).ToArray().Length;
			}
			if (machineIndex == machines.Count)
			{
				machineIndex = 0;
				btnNext.Enabled = false;
			}
			ViewState["machineIndex"] = machineIndex;
			ViewState["iniMachineIndex"] = iniMachineIndex;
			if (machines.Count <= iniMachineIndex)
			{
				btnPrevious.Enabled = false;
				btnNext.Enabled = false;
			}

			dt = new DataTable();
			DataView dv = null;
			DateTime DtS = new DateTime();
			DateTime DtE = new DateTime();
			DateTime[] StartTime = new DateTime[0];
			DateTime[] EndTime = new DateTime[0];
			DateTime[] DX3 = new DateTime[0];
			double[] Arr3 = new double[0];
			double[] Arrval = new double[0];
			DateTime[] DNSStr = new DateTime[0];
			DateTime[] DNEStr = new DateTime[0];

			if (machineIDs.Length != 0)
			{
				machineIDs = machineIDs.Remove(machineIDs.Length - 1);
			}

			try
			{
				dt = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, ddlShift.Text, ddlPlantId.Text, machineIDs, "");
				foreach (string machine in tempList)
				{
					dv = new DataView(dt);
					dv.RowFilter = "MachineID='" + machine + "'";
					rec = 0;

					Array.Clear(StartTime, 0, StartTime.Length);
					Array.Clear(EndTime, 0, EndTime.Length);
					Array.Clear(Arrval, 0, Arrval.Length);

					for (int k = 0; k < dv.Count; k++)
					{
						ReDim(ref StartTime, rec + 1);
						ReDim(ref EndTime, rec + 1);

						ReDim(ref Arrval, rec + 1);

						StartTime[rec] = DateTime.Parse(dv[k]["Starttime"].ToString());
						EndTime[rec] = DateTime.Parse(dv[k]["Endtime"].ToString());
						Arrval[rec++] = double.Parse(dv[k]["Value"].ToString());
					}

					Array.Clear(DNSStr, 0, DNSStr.Length);
					Array.Clear(DNEStr, 0, DNEStr.Length);

					for (int KJ = 0; KJ < Arrval.Length; KJ++)
					{
						try
						{
							if (Arrval[KJ] == 0 && Arrval[KJ] == 0)
							{
								ReDim(ref DNSStr, Drec + 1);
								ReDim(ref DNEStr, Drec + 1);
								DNSStr[Drec] = StartTime[KJ];
								DNEStr[Drec++] = EndTime[KJ];
							}
						}
						catch
						{
						}
						Arrval[KJ] = 1;
					}

					if (EndTime.Length > 0)
					{
						DtS = StartTime[0];
						DtE = EndTime[EndTime.Length - 1];
					}

					Array.Clear(Arr3, 0, Arr3.Length);
					Array.Clear(DX3, 0, DX3.Length);

					rec = 0;
					while (DtE > DtS)
					{
						ReDim(ref DX3, rec + 1);
						ReDim(ref Arr3, rec + 1);
						Arr3[rec] = d;
						DX3[rec++] = DtS;
						DtS = DtS.AddMinutes(1);
					}

					SquareWaveChartWithLinePDTDownTime(ref StartTime, ref EndTime, ref Arrval, ref DNSStr, ref DNEStr, ref DX3, ref Arr3, d, machine);
					d = d + 1.0;
				}

			}

			catch (Exception ex)
			{
				Logger.WriteErrorLog(ex.ToString());
				lblMessage.Text = ex.Message.ToString();
			}
			finally
			{

			}

			#endregion


			//c.yAxis().setLinearScale(0.0, 5.0);
			c.yAxis().setLabels(labels.ToArray());
			c.yAxis().setLabelStyle("Verdana", 8.0, 0x000000, 60);

			WebChartViewer1.Image = c.makeWebImage(ChartDirector.Chart.PNG);

			WebChartViewer1.ImageMap = c.getHTMLImageMap("", "", "title='{x|hh:nn}'");
		}

		private void SquareWaveChartWithLinePDTDownTime(ref DateTime[] dataX0, ref DateTime[] dataXEndtime, ref Double[] dataY0, ref DateTime[] DNStart, ref DateTime[] DNEnd, ref DateTime[] dataX3, ref Double[] dataY3, double YPositionMachine, string machine)
		{
			try
			{
				double d1 = YPositionMachine;

				// The icons for the sectors
				int icons = c.patternColor2("..\\Resources\\HeaderBlue.jpg");

				//setting the Reference Layer to Show the Time for every 1 min. in IMap of chart            
				StepLineLayer layer3 = c.addStepLineLayer(dataY3, 0x000000, machine);

				DateTime[] datax10 = new DateTime[dataX0.Length + 1];
				dataX0.CopyTo(datax10, 0);
				// datax10[datax10.Length-1] = dataXEndtime[dataXEndtime.Length - 1];
				//	datax10[datax10.Length - 1] = LogicalStartEnd[1];				
				layer3.setXData(dataX3);
				layer3.setLineWidth(1);

				int rec = 0;
				StepLineLayer[] layer1 = new StepLineLayer[0];

				object OPDT = null;
				try
				{
					OPDT = DataBaseAccessPTA.IgnorePDT();
				}
				catch (Exception ex)
				{
					ErrorSignal.FromCurrentContext().Raise(ex);
				}

				StepLineLayer[] layer0 = new StepLineLayer[0];

				//else if (flag_PDT_ML.ToString().ToUpper() == "PDT")
				//{
				#region PDT
				if (OPDT != null)
				{
					if (OPDT.ToString().ToUpper() == "Y")
					{
						if (runStoredProcFirstTime == 0)
						{
							dtPDT = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, ddlShift.SelectedValue, ddlPlantId.Text, machineIDs, "PlannedDT");
						}

						DateTime[] DTStr = new DateTime[0];
						DateTime[] DTEtr = new DateTime[0];
						//double[] Arrval = new double[0];
						StepLineLayer layer2 = new StepLineLayer();
						DateTime[] dataX2 = new DateTime[0];

						rec = 0;
						dvPDT = new DataView(dtPDT);
						dvPDT.RowFilter = "MachineID=" + "'" + machine + "'";

						for (int k3 = 0; k3 < dvPDT.Count; k3++)
						{
							ReDim(ref DTStr, rec + 1);
							ReDim(ref DTEtr, rec + 1);
							//ReDim(ref Arrval, rec + 1);
							//ReDim(ref layer2, rec + 1);
							ReDim(ref dataX2, 2);
							DTStr[rec] = DateTime.Parse(dvPDT[k3]["Starttime"].ToString());
							DTEtr[rec] = DateTime.Parse(dvPDT[k3]["Endtime"].ToString());

							double[] dataY1 = { d1, d1 };
							dataX2[0] = DTStr[rec];
							dataX2[1] = DTEtr[rec];
							layer2 = c.addStepLineLayer(dataY1, 0x0000ff, machine); //Blue Color 0x0000ff
							layer2.setXData(dataX2);
							layer2.setLineWidth(40);
							//layer2.setAlignment(1);                                 

						}
					}

				}
				#endregion

				#region ManagementLoss
				if (runStoredProcFirstTime == 0)
				{
					dtML = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, ddlShift.SelectedValue, ddlPlantId.Text, machineIDs, "ML");
				}

				DateTime[] IDTStrml = new DateTime[0];
				DateTime[] IDTEtrml = new DateTime[0];
				StepLineLayer[] Ilayer3 = new StepLineLayer[0];
				DateTime[] IdataX3 = new DateTime[0];


				rec = 0;
				dvML = new DataView(dtML);
				dvML.RowFilter = "MachineID=" + "'" + machine + "'";
				for (int k2 = 0; k2 < dvML.Count; k2++)
				{

					ReDim(ref IDTStrml, rec + 1);
					ReDim(ref IDTEtrml, rec + 1);
					//ReDim(ref IArrval, rec + 1);
					ReDim(ref Ilayer3, rec + 1);
					ReDim(ref IdataX3, 2);
					IDTStrml[rec] = DateTime.Parse(dvML[k2]["Starttime"].ToString());
					IDTEtrml[rec] = DateTime.Parse(dvML[k2]["Endtime"].ToString());

					double[] IdataY1 = { d1, d1 };

					IdataX3[0] = IDTStrml[rec];
					IdataX3[1] = IDTEtrml[rec];

					Ilayer3[rec] = c.addStepLineLayer(IdataY1, 0xFFFF00, machine); //Yellow color
					Ilayer3[rec].setXData(IdataX3);
					//IArrval[rec] = Convert.ToDouble(IdataX2[rec]);
					Ilayer3[rec++].setLineWidth(40);

				}
				#endregion

				#region ICD
				//Getting data for ICD
				if (runStoredProcFirstTime == 0)
				{
					dtICD = DataBaseAccessPTA.GetMachineUpDownTimes(frmDate, todate, ddlShift.SelectedValue, ddlPlantId.Text, machineIDs, "ICD");
				}


				DateTime[] IDTStr = new DateTime[0];
				DateTime[] IDTEtr = new DateTime[0];
				double[] IArrval = new double[0];
				StepLineLayer[] Ilayer2 = new StepLineLayer[0];
				DateTime[] IdataX2 = new DateTime[0];

				dvICD = new DataView(dtICD);
				dvICD.RowFilter = "MachineID=" + "'" + machine + "'";
				rec = 0;
				for (int k1 = 0; k1 < dvICD.Count; k1++)
				{

					ReDim(ref IDTStr, rec + 1);
					ReDim(ref IDTEtr, rec + 1);
					//ReDim(ref IArrval, rec + 1);
					ReDim(ref Ilayer2, rec + 1);
					ReDim(ref IdataX2, 2);
					IDTStr[rec] = DateTime.Parse(dvICD[k1]["Starttime"].ToString());
					IDTEtr[rec] = DateTime.Parse(dvICD[k1]["Endtime"].ToString());
					double[] IdataY1 = { d1, d1 };
					IdataX2[0] = IDTStr[rec];
					IdataX2[1] = IDTEtr[rec];
					Ilayer2[rec] = c.addStepLineLayer(IdataY1, 0x990000, machine); //Maroon Color
					Ilayer2[rec].setXData(IdataX2);
					Ilayer2[rec++].setLineWidth(40);
				}

				#endregion

				//plotting the DownTime Values
				for (int KJ = 0; KJ < DNStart.Length; KJ++)
				{
					ReDim(ref layer1, rec + 1);
					DateTime[] dataX1 = { DNStart[KJ], DNEnd[KJ] };
					double[] dataY1 = { d1, d1 };
					layer1[rec] = c.addStepLineLayer(dataY1, 0xff0000, machine); //Red Color
					layer1[rec].setXData(dataX1);
					layer1[rec++].setLineWidth(40);
				}

				rec = 0;
				for (int KJ = 0; KJ < dataXEndtime.Length; KJ++)
				{
					ReDim(ref layer0, rec + 1);
					DateTime[] dataXtemp = { dataX0[KJ], dataXEndtime[KJ] };
					double[] dataY1 = { d1, d1 };
					layer0[rec] = c.addStepLineLayer(dataY1, 0x00ff00, machine); //Green color
					layer0[rec].setXData(dataXtemp);
					layer0[rec++].setLineWidth(40);
				}
				runStoredProcFirstTime = 1;
			}
			catch (Exception ex)
			{
				lblMessage.Text = ex.Message.ToString();
				Logger.WriteErrorLog(ex.ToString());
			}
		}

		protected void btnProcess_Click(object sender, EventArgs e)
		{
			try
			{
				machineIndex = 0;
				if (Session["ScreenHeight"] != null)
					iniMachineIndex = (Convert.ToInt32(Session["ScreenHeight"])) / 100;

				iniMachineIndex = ddlMachineId.Items.Count > iniMachineIndex ? iniMachineIndex : ddlMachineId.Items.Count;
				ViewState["machineIndex"] = machineIndex;
				ViewState["iniMachineIndex"] = iniMachineIndex;
				ViewState["IsPrev"] = false;
				createPTAchart();
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void ReDim<T>(ref T[] arr, int length)
		{
			T[] arrTemp = new T[length];
			if (length > arr.Length)
			{
				Array.Copy(arr, 0, arrTemp, 0, arr.Length);
				arr = arrTemp;
			}
			else
			{
				Array.Copy(arr, 0, arrTemp, 0, length);
				arr = arrTemp;
			}
		}

		#region "Bind Shift Names"
		private void BindShiftData()
		{
			try
			{
				var allShift = BindCockpitView.GetAllShift();
				if (allShift != null && allShift.Count > 0)
				{
					ddlShift.DataSource = allShift;
					ddlShift.DataBind();

					ddlShift.Items.Insert(0, new ListItem
					{
						Text = GetGlobalResourceObject("CommanResource", "ShiftAll").ToString(),
						Value = "All"
					});

				}
			}
			catch (Exception ex)
			{
				lblMessages.Text = ex.Message;
			}
		}
		#endregion

		#region "Bind Plant Id"
		private void BindPlantId()
		{
			try
			{
				List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
				ddlPlantId.DataSource = lstPlantData;
				ddlPlantId.DataBind();
				//ddlPlantId.Items.Insert(0, new ListItem
				//{
				//	Text = GetGlobalResourceObject("CommanResource", "PlantAll").ToString(),
				//	Value = "All"
				//});
				ddlPlantId_SelectedIndexChanged(null, null);
			}
			catch (Exception ex)
			{
				// ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}
		#endregion

		#region "Bind Machine Id"
		private void BindMachines()
		{
			try
			{
				ddlMachineId.Items.Clear();// = null;
				var allMachineName = VDGDataBaseAccess.GetAllMachines(ddlPlantId.SelectedValue.ToString());// AccessReportData.GetAllMachines(ddlPlantId.SelectedItem.ToString());
				if (allMachineName != null && allMachineName.Count > 0)
				{
					ddlMachineId.DataSource = allMachineName;
					ddlMachineId.DataBind();
				}
			}
			catch (Exception ex)
			{
				ErrorSignal.FromCurrentContext().Raise(ex);
				lblMessages.Text = ex.Message;
			}
		}
		#endregion

		protected void btnPrevious_Click(object sender, EventArgs e)
		{
			ViewState["IsPrev"] = true;
			createPTAchart();
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{
			ViewState["IsPrev"] = false;
			createPTAchart();
		}

		protected void ddlPlantId_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindMachines();
		}

	}
}