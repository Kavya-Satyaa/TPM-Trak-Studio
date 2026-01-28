using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class SpindleRuntime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string endTime = string.Empty;
            string startTime = DataBaseAccess.GetCurrentShiftStart(out endTime);
            txtFromDate.Text = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss");
            txtToDate.Text = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss");
            BindMachines();
        }

        private void BindMachines()
        {
            try
            {
                var allMachineName = VDGDataBaseAccess.GetAllMachines("All");
                if (allMachineName != null && allMachineName.Count > 0)
                {
                    ddlMachineId.DataSource = allMachineName;
                    ddlMachineId.DataBind();
                    ddlMachineId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> BindCycleStartTime(string startDT, string endDT, string MachineID, string param)
        {
            List<string> cycleStartTime = new List<string>();
            try
            {
                cycleStartTime = DataBaseAccess.GetSpindleCycleStartTimes(startDT, endDT, MachineID, param);
                cycleStartTime.Insert(0, "All");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return cycleStartTime;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static SpindleRuntimeData GetSpindleRuntimeData(string startDT, string endDT, string MachineID, string param)
        {
            SpindleRuntimeData spRuntimeData = new SpindleRuntimeData();
            List<PlotLines> spindlePlotLinesList = new List<PlotLines>();
            PlotLines plotLineData = null;
            List<long[]> MacStatusData = new List<long[]>();
            long[] MacStatus = null;
            SqlConnection sqlConn = ConnectionManager.GetConnection();
            SqlCommand cmd = null;
            DataSet dataSet = new DataSet();
            DataTable dt;
            try
            {
                cmd = new SqlCommand("S_GetSpindleDetailsreport", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartTime", startDT);
                cmd.Parameters.AddWithValue("@EndTime", endDT);
                cmd.Parameters.AddWithValue("@MachineID", MachineID);
                cmd.Parameters.AddWithValue("@Param", param);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);

                dt = dataSet.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["StartDate"].ToString()))
                        {
                            plotLineData = new PlotLines();
                            plotLineLabelParam labelParam = new plotLineLabelParam();
                            labelParam.text = "Start";
                            labelParam.rotation = 90;
                            labelParam.align = "left";
                            labelParam.y = 10;
                            labelParam.x = 10;
                            plotLineData.color = "#FF0000";
                            plotLineData.width = 2;
                            plotLineData.value = (long)(Convert.ToDateTime(dr["StartDate"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            plotLineData.zIndex = 3;
                            plotLineData.label = labelParam;
                            spindlePlotLinesList.Add(plotLineData);
                        }

                        if (!string.IsNullOrEmpty(dr["Enddate"].ToString()))
                        {
                            plotLineData = new PlotLines();
                            plotLineLabelParam labelParam = new plotLineLabelParam();
                            labelParam.text = "End";
                            labelParam.rotation = -90;
                            labelParam.align = "right";
                            labelParam.y = 10;
                            labelParam.x = -10;
                            plotLineData.color = "#FF0000";
                            plotLineData.width = 2;
                            plotLineData.value = (long)(Convert.ToDateTime(dr["Enddate"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            plotLineData.zIndex = 3;
                            plotLineData.label = labelParam;
                            spindlePlotLinesList.Add(plotLineData);
                        }
                    }
                }

                dt = dataSet.Tables[1];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["Starttime"].ToString()))
                        {
                            MacStatus = new long[2];
                            MacStatus[0] = (long)(Convert.ToDateTime(dr["Starttime"]).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            MacStatus[1] = Convert.ToInt32(dr["RecordType"].ToString());
                            MacStatusData.Add(MacStatus);
                        }
                    }
                }

                dt = dataSet.Tables[2];
                if (dt != null && dt.Rows.Count > 0)
                {
                    spRuntimeData.totalSpindleRuntime = "Total Spindle Run Time : " + dt.Rows[0][1].ToString();
                }

                spRuntimeData.spRuntimeDataList = MacStatusData;
                spRuntimeData.plotLinesList = spindlePlotLinesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConn != null) { sqlConn.Close(); }
            }
            return spRuntimeData;
        }
    }
}