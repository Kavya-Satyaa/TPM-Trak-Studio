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
    public partial class ModbusRegisterScreen : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindScreenData();
            }
        }

        public void BindScreenData()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DataBaseAccess.GetModbusData();
                
                gvModbus.DataSource = dt;
                gvModbus.DataBind();
                Session["ScreenData"] = dt;

            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog("BindScreenData: "+ ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["ScreenData"];
            bool updated = false;
            string res = "";
            try
            {
                foreach (GridViewRow row in gvModbus.Rows)
                {
                    if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        List<string> ModbusUpdatedValues = new List<string>();
                        try
                        {
                            string MachineID = ((Label)row.FindControl("lblMachineID")).Text;
                            string f1 = (row.FindControl("txtHoldingRegisterForCommunication") as TextBox).Text;
                            string f2 = (row.FindControl("txtHoldingRegisterDateAndStatus") as TextBox).Text;
                            string f3 = (row.FindControl("txtHoldingRegisterDateAndStatusAckAddress") as TextBox).Text;
                            string f4 = (row.FindControl("txtHoldingRegisterStartAddress_M1") as TextBox).Text;
                            string f5 = (row.FindControl("txtBytesToRead_M1") as TextBox).Text;
                            string f6 = (row.FindControl("txtAckAddress_M1") as TextBox).Text;
                            string f7 = (row.FindControl("txtHoldingRegisterStartAddress_M2") as TextBox).Text;
                            string f8 = (row.FindControl("txtBytesToRead_M2") as TextBox).Text;
                            string f9 = (row.FindControl("txtAckAddress_M2") as TextBox).Text;
                            string f10 = (row.FindControl("txtHoldingRegisterStartAddress_M3") as TextBox).Text;
                            string f11 = (row.FindControl("txtBytesToRead_M3") as TextBox).Text;
                            string f12 = (row.FindControl("txtAckAddress_M3") as TextBox).Text;
                            string f13 = (row.FindControl("txtDownCodeRequestAddess") as TextBox).Text;
                            string f14 = (row.FindControl("txtDownCodeStartingAddress") as TextBox).Text;
                            string f15 = (row.FindControl("txtDownInterfaceIDStartingAddress") as TextBox).Text;
                            string f16 = (row.FindControl("txtTotalNumberOfDownCodeAddress") as TextBox).Text;
                            string f17 = (row.FindControl("txtInsertedDatetime") as Label).Text;
                            if (f17 == "")
                            {
                                f17 = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            string f18 = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                            string f19 = Session["UserName"].ToString();
                            string f20 = (row.FindControl("txtHMIRegister_M1") as TextBox).Text;
                            string f21 = (row.FindControl("txtHMIRegister_M2") as TextBox).Text;
                            string f22 = (row.FindControl("txtSharedMachineID") as TextBox).Text;

                            var row_original = dt.AsEnumerable().Where(x => x["Machine_ID"].ToString() == MachineID).FirstOrDefault();

                            ModbusUpdatedValues.Add(MachineID);
                            ModbusUpdatedValues.Add(f1);
                            ModbusUpdatedValues.Add(f2);
                            ModbusUpdatedValues.Add(f3);
                            ModbusUpdatedValues.Add(f4);
                            ModbusUpdatedValues.Add(f5);
                            ModbusUpdatedValues.Add(f6);
                            ModbusUpdatedValues.Add(f7);
                            ModbusUpdatedValues.Add(f8);
                            ModbusUpdatedValues.Add(f9);
                            ModbusUpdatedValues.Add(f10);
                            ModbusUpdatedValues.Add(f11);
                            ModbusUpdatedValues.Add(f12);
                            ModbusUpdatedValues.Add(f13);
                            ModbusUpdatedValues.Add(f14);
                            ModbusUpdatedValues.Add(f15);
                            ModbusUpdatedValues.Add(f16);
                            ModbusUpdatedValues.Add(f17);
                            ModbusUpdatedValues.Add(f18);
                            ModbusUpdatedValues.Add(f19);
                            ModbusUpdatedValues.Add(f20);
                            ModbusUpdatedValues.Add(f21);
                            ModbusUpdatedValues.Add(f22);

                            updated = DataBaseAccess.UpdateModbusScreenData(ModbusUpdatedValues);

                            if(updated == false)
                            {
                                res += MachineID;
                            }
                        }
                        catch(Exception ex)
                        {
                            Logger.WriteErrorLog(ex.Message);
                        }
                    }
                }
                if(res == "")
                {
                    HelperClassGeneric.openUpdateSuccessModal(this);
                }
                else
                {
                    HelperClassGeneric.openErrorModal(this, "Error Updating following Machine ID:" + res);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
                HelperClassGeneric.openErrorModal(this, "Error Updating records");
            }
            BindScreenData();
        }

    }
}