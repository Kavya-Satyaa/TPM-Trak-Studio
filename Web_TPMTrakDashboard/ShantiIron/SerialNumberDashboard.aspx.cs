using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.ShantiIron.Model;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard.ShantiIron
{
    public partial class SerialNumberDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindAllLineIDs();
                //  bindAllSerialNos();
                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }
        private void bindAllLineIDs()
        {
            try
            {
                List<string> lineIdList = DataBaseAccess.GetAllPlants();
                lineIdList.RemoveAt(0);
                ddlLineId.DataSource = lineIdList;
                ddlLineId.DataBind();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        private void bindAllSerialNos()
        {
            try
            {
                //List<string> lineIdList = DataBaseAccess.GetAllSerialNos();
                //ddlSerialNumber.DataSource = lineIdList;
                //ddlSerialNumber.DataBind();
                List<string> lineIdList = null;
                lineIdList = ShantiDataBaseAccess.GetAllSerialNos();
                lineIdList.Insert(0, "");
                var builder = new System.Text.StringBuilder();
                if (lineIdList.Count > 0)
                {
                    for (int i = 0; i < lineIdList.Count; i++)
                    {
                        if (i == 0)
                        {
                            txtSerialNumber.Text = lineIdList[i].ToString();
                        }
                        builder.Append(String.Format("<option style='font-weight:unset' value='{0}'>", lineIdList[i].ToString()));
                    }
                }
                else
                {
                    txtSerialNumber.Text = "";
                }
                //dlSerialNumbers.InnerHtml = builder.ToString();

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                lblMessages.ForeColor = System.Drawing.Color.Red;
                lblMessages.Text = ex.Message;
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<string> getGroupIDs(string plantId)
        {
            List<string> grpIdList = new List<string>();
            try
            {
                grpIdList = ShantiDataBaseAccess.GetAllGroupIDs(plantId);

            }
            catch (Exception ex)
            {
            }
            return grpIdList;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<SerialNumberDashboardEntity> getSerialNumberDetails(string input, string plantId, string param, string groupId)
        {
            List<SerialNumberDashboardEntity> finalSlnoList = new List<SerialNumberDashboardEntity>();
            SerialNumberDashboardEntity finalslnoDetails = null;
            try
            {
                List<SerialNumberDashboardEntity> listSlNoOperationDetails = new List<SerialNumberDashboardEntity>();
                List<SerialNumberDashboardEntity> listSlNoTimeDetails = new List<SerialNumberDashboardEntity>();
                List<string> operationNameList = new List<string>();
                listSlNoOperationDetails = ShantiDataBaseAccess.getSerialNumberDetialsByDate(input, plantId, param,groupId, out listSlNoTimeDetails, out operationNameList);


                if (listSlNoOperationDetails.Count > 0)
                {
                    var distinctOperationname = operationNameList.Distinct().ToList();
                    finalslnoDetails = new SerialNumberDashboardEntity();
                    finalslnoDetails.SerialNumber = "Serial Number";
                    finalslnoDetails.ComponentID = "Component ID";
                    List<OperationDetails> listOperaionDetails = new List<OperationDetails>();
                    OperationDetails operationDetails = null;
                    for (int i = 0; i < distinctOperationname.Count; i++)
                    {
                        operationDetails = new OperationDetails();
                        operationDetails.OperationName = distinctOperationname[i];
                        listOperaionDetails.Add(operationDetails);
                    }
                    finalslnoDetails.OperatioList = listOperaionDetails;
                    finalslnoDetails.TotalTime = "Total Time (HH:MM:SS)";
                    finalslnoDetails.ElapsedTime = "Elapsed Time (HH:MM:SS)";
                    finalslnoDetails.RunTime = "Run Time (HH:MM:SS)";
                    finalSlnoList.Add(finalslnoDetails);

                    finalslnoDetails = new SerialNumberDashboardEntity();
                    finalslnoDetails.SerialNumber = "";
                    finalslnoDetails.ComponentID = "";
                    listOperaionDetails = new List<OperationDetails>();
                    for (int i = 0; i < distinctOperationname.Count; i++)
                    {
                        operationDetails = new OperationDetails();
                        operationDetails.OperationName = "";
                        operationDetails.Machine = "Machine";
                        operationDetails.Operator = "Operator";
                        operationDetails.StartTime = "Start Time";
                        operationDetails.EndTime = "End Time";
                        listOperaionDetails.Add(operationDetails);
                    }
                    finalslnoDetails.OperatioList = listOperaionDetails;
                    finalslnoDetails.TotalTime = "";
                    finalslnoDetails.ElapsedTime = "";
                    finalslnoDetails.RunTime = "";
                    finalSlnoList.Add(finalslnoDetails);

                    for (int i = 0; i < listSlNoOperationDetails.Count; i++)
                    {
                        finalslnoDetails = new SerialNumberDashboardEntity();
                        finalslnoDetails.SerialNumber = listSlNoOperationDetails[i].SerialNumber;
                        finalslnoDetails.plantId = listSlNoOperationDetails[i].plantId;
                        finalslnoDetails.groupid = listSlNoOperationDetails[i].groupid;
                        finalslnoDetails.OperatioList = listSlNoOperationDetails[i].OperatioList;
                        finalslnoDetails.ComponentID = listSlNoOperationDetails[i].ComponentID;
                        for (int k = 0; k < listSlNoTimeDetails.Count; k++)
                        {
                            if (finalslnoDetails.SerialNumber == listSlNoTimeDetails[k].SerialNumber && finalslnoDetails.plantId == listSlNoTimeDetails[k].plantId && finalslnoDetails.groupid == listSlNoTimeDetails[k].groupid && finalslnoDetails.ComponentID == listSlNoTimeDetails[k].ComponentID)
                            {
                                finalslnoDetails.TotalTime = listSlNoTimeDetails[k].TotalTime;
                                finalslnoDetails.ElapsedTime = listSlNoTimeDetails[k].ElapsedTime;
                                finalslnoDetails.RunTime = listSlNoTimeDetails[k].RunTime;
                            }

                        }
                        finalSlnoList.Add(finalslnoDetails);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            HttpContext.Current.Session["SlnoDashboardData"] = finalSlnoList;
            return finalSlnoList;

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<AlarmDetails> getAlarmDetails(string starttime, string endtime, string machine, string operationname, string slno, string componentId)
        {
            List<AlarmDetails> alarmDetailsList = new List<AlarmDetails>();
            try
            {
                alarmDetailsList = ShantiDataBaseAccess.getAlarmDetails(starttime, endtime, machine, operationname, slno, componentId);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return alarmDetailsList;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<MeasurementDetails> getMeasurementDetails(string starttime, string endtime, string machine, string operationname, string slno, string componentId)
        {
            List<MeasurementDetails> measurementDetailsList = new List<MeasurementDetails>();
            try
            {
                measurementDetailsList = ShantiDataBaseAccess.getMeasurementDetails(starttime, endtime, machine, operationname, slno, componentId);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return measurementDetailsList;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<InspectionDetails> getInspectionDetails(string starttime, string endtime, string machine, string operationname, string slno, string componentId, string plantid, string groupid)
        {
            List<InspectionDetails> inspectionDetailsList = new List<InspectionDetails>();
            InspectionDetails inspectionDetails = null;
            DataTable dtinspectionDetails = new DataTable();
            try
            {
                if (operationname == "90")
                {
                    dtinspectionDetails = ShantiDataBaseAccess.getInspectionDetailsForOpn90(starttime, endtime, machine, operationname, slno, componentId, plantid, groupid);
                    for (int i = 0; i < dtinspectionDetails.Rows.Count; i++)
                    {
                        inspectionDetails = new InspectionDetails();
                        inspectionDetails.SlNo = dtinspectionDetails.Rows[i]["compslno"].ToString();
                        inspectionDetails.ComponentID = dtinspectionDetails.Rows[i]["ComponentID"].ToString();
                        inspectionDetails.Status = dtinspectionDetails.Rows[i]["status"].ToString();
                        inspectionDetailsList.Add(inspectionDetails);
                    }
                }
                else
                {
                    dtinspectionDetails = ShantiDataBaseAccess.getInspectionDetails(starttime, endtime, machine, operationname, slno, componentId, plantid, groupid);
                    for (int i = 0; i < dtinspectionDetails.Rows.Count; i++)
                    {
                        inspectionDetails = new InspectionDetails();
                        inspectionDetails.SlNo = dtinspectionDetails.Rows[i]["compslno"].ToString();
                        inspectionDetails.ComponentID = dtinspectionDetails.Rows[i]["ComponentID"].ToString();
                        inspectionDetails.InspectionDate = dtinspectionDetails.Rows[i]["InspectionDate"].ToString();
                        inspectionDetails.Status = dtinspectionDetails.Rows[i]["status"].ToString();
                        inspectionDetails.Remarks = dtinspectionDetails.Rows[i]["Remarks"].ToString();
                        inspectionDetails.ChekedBy = dtinspectionDetails.Rows[i]["CheckedBy"].ToString();
                        inspectionDetails.DimentionalStatus = dtinspectionDetails.Rows[i]["DimentionalStatus"].ToString();
                        inspectionDetails.Value = dtinspectionDetails.Rows[i][dtinspectionDetails.Columns.Count - 1].ToString();
                        inspectionDetailsList.Add(inspectionDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return inspectionDetailsList;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static List<InspectionDetails> getMPIDetails(string starttime, string endtime, string machine, string operationname, string slno, string componentId)
        {
            List<InspectionDetails> inspectionDetailsList = new List<InspectionDetails>();
            try
            {

                inspectionDetailsList = ShantiDataBaseAccess.getMPIInspectionDetails(starttime, endtime, machine, operationname, slno, componentId);

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return inspectionDetailsList;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ShantiReports.slnoDashboardDetails(hdnSlno.Value, hdnCompID.Value, txtDate.Text, hdnPlant.Value, hdnGroupID.Value);
            }
            catch(Exception ex)
            {

            }
        }
    }
}