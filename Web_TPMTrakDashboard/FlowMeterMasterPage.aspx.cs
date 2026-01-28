using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class FlowMeterMasterPage : System.Web.UI.Page
    {
        List<string> Component = new List<string>();
        List<FlowMeterBoschEntity> FlowMeterData = new List<FlowMeterBoschEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindComponent();
                    //BindGrid("");
                    ddldropdow.Value = "";
                    BindGrid("");

                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);

            }
        }

        [WebMethod]
        public static List<string> getdata(string blank)
        {
            List<string> Componentid = new List<string>();
            try
            {
                Componentid = DataBaseAccess.GetAllComponents();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error {0}", ex.Message);
            }
            return Componentid;
        }

        private void BindComponent()
        {
            try
            {
                Component = DataBaseAccess.GetAllComponents();
                if (Component != null && Component.Count > 0)
                {
                    // ddldropdow.DataSource = Component;
                    //ddldropdow.DataBind();
                    //ddldropdow.SelectedIndex = 0;
                }
                else
                {
                    // ddldropdow.DataSource = new List<string>();
                    ddldropdow.DataBind();
                }
                Component.Remove("All");
                Session["ComponentList"] = Component;
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);

            }

        }

        private void BindGrid(string dropdownvalue)
        {
            Session["FlowMeterData"] = FlowMeterData = DataBaseAccess.GetFlowMeterData(dropdownvalue);
            if (FlowMeterData != null && FlowMeterData.Count > 0)
            {
                lstflowmeterlistview.DataSource = FlowMeterData;
                lstflowmeterlistview.DataBind();
            }
            else
            {
                lstflowmeterlistview.DataSource = FlowMeterData;
                lstflowmeterlistview.DataBind();
            }
            btnSearch.Attributes.Add("onclick", "javascript: hourglass();");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnAdd.Text.Equals("Add", StringComparison.OrdinalIgnoreCase))
                {
                    btnAdd.Text = "Cancel";
                    if (Session["FlowMeterData"] != null)
                        FlowMeterData = Session["FlowMeterData"] as List<FlowMeterBoschEntity>;
                    if (FlowMeterData != null)
                    {

                        FlowMeterBoschEntity data = new FlowMeterBoschEntity();
                        data.slno = FlowMeterData.Count + 1;
                        if (Session["ComponentList"] != null)
                        {
                            data.ComponentList = Session["ComponentList"] as List<string>;
                            data.Component = data.ComponentList[0];
                        }
                        data.ComponentdrpEnable = true;
                        data.ComponentlblEnable = false;
                        data.TypeDia = "";
                        data.SettingsAng = "";
                        data.SettingsHt = "";
                        data.SettingsHTGuage = "";
                        data.RotaMin = "";
                        data.RotaMax = "";
                        data.RotaMedian = "";
                        data.ShaftTestPr = "";
                        data.RotaFlow = "";
                        data.BaralInspection = "";
                        data.IsTGGType = false;
                        FlowMeterData.Insert(0, data);
                        lstflowmeterlistview.DataSource = null;
                        lstflowmeterlistview.DataSource = FlowMeterData;

                        lstflowmeterlistview.DataBind();
                    }
                    else
                    {
                        FlowMeterBoschEntity data = new FlowMeterBoschEntity();
                        DataPager DP = (lstflowmeterlistview.FindControl("DataPager1") as DataPager);
                        DP.SetPageProperties(1, 19, true);
                        data.slno = FlowMeterData.Count + 1;
                        if (Session["ComponentList"] != null)
                        {
                            data.ComponentList = Session["ComponentList"] as List<string>;
                            data.Component = data.ComponentList[0];
                        }
                        data.ComponentdrpEnable = true;
                        data.ComponentlblEnable = false;
                        data.TypeDia = "";
                        data.SettingsAng = "";
                        data.SettingsHt = "";
                        data.SettingsHTGuage = "";
                        data.RotaMin = "";
                        data.RotaMax = "";
                        data.RotaMedian = "";
                        data.ShaftTestPr = "";
                        data.RotaFlow = "";
                        data.BaralInspection = "";
                        data.IsTGGType = false;
                        FlowMeterData.Insert(0, data);
                        lstflowmeterlistview.DataSource = null;
                        lstflowmeterlistview.DataSource = FlowMeterData;
                        lstflowmeterlistview.DataBind();
                    }
                    //lstflowmeterlistview.Items[0].BackColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    btnAdd.Text = "Add";
                    ddldropdow.Value = "";
                    BindGrid("");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool save = false;
            try
            {

                foreach (ListViewItem Items in lstflowmeterlistview.Items)
                {
                    string ComponentID = "";
                    if (btnAdd.Text.Equals("Cancel"))
                    {
                        ComponentID = (Items.FindControl("ddlcomponentdropdown") as DropDownList).SelectedValue.ToString();
                    }
                    else
                    {
                        ComponentID = (Items.FindControl("lblComponent") as Label).Text;
                    }

                    string txtAngDeg = (Items.FindControl("txtAngDeg") as TextBox).Text;
                    string HidAngDeg = (Items.FindControl("HidAngDeg") as HiddenField).Value;
                    string txtTypeDia = (Items.FindControl("txtTypeDia") as TextBox).Text;
                    string HiddTypeDia = (Items.FindControl("HiddTypeDia") as HiddenField).Value;
                    string txtHTMM = (Items.FindControl("txtHTMM") as TextBox).Text;
                    string HidHTMM = (Items.FindControl("HidHTMM") as HiddenField).Value;
                    string txtHTGUAGE = (Items.FindControl("txtHTGUAGE") as TextBox).Text;
                    string HidHTGUAGE = (Items.FindControl("HidHTGUAGE") as HiddenField).Value;
                    string txtPRBAR = (Items.FindControl("txtPRBAR") as TextBox).Text;
                    string HidPRBAR = (Items.FindControl("HidPRBAR") as HiddenField).Value;
                    string txtROTAMIN = (Items.FindControl("txtROTAMIN") as TextBox).Text;
                    string HidROTAMIN = (Items.FindControl("HidROTAMIN") as HiddenField).Value;
                    string txtRotaMax = (Items.FindControl("txtRotaMax") as TextBox).Text;
                    string HidRotaMax = (Items.FindControl("HidRotaMax") as HiddenField).Value;
                    string txtRotaMedian = (Items.FindControl("txtRotaMedian") as TextBox).Text;
                    string HidRotaMedian = (Items.FindControl("HidRotaMedian") as HiddenField).Value;
                    string txtTestPr = (Items.FindControl("txtTestPr") as TextBox).Text;
                    string HidTestPr = (Items.FindControl("HidTestPr") as HiddenField).Value;
                    string txtRotaFlow = (Items.FindControl("txtRotaFlow") as TextBox).Text;
                    string HidRotaFlow = (Items.FindControl("HidRotaFlow") as HiddenField).Value;
                    string txtbarrelInspection = (Items.FindControl("txtbarrelInspection") as TextBox).Text;
                    string HidbarrelInspection = (Items.FindControl("HidbarrelInspection") as HiddenField).Value;
                    string txtHeadRemark = (Items.FindControl("txtHeadRemark") as TextBox).Text;
                    string HidHeadremark = (Items.FindControl("HidHeadremark") as HiddenField).Value;
                    string txtShaftRemark = (Items.FindControl("txtShaftRemark") as TextBox).Text;
                    string HidShaftRemark = (Items.FindControl("HidShaftRemark") as HiddenField).Value;
                    bool isTGGType = (Items.FindControl("chkIsTGG") as CheckBox).Checked;
                    bool HidTGGType = Convert.ToBoolean((Items.FindControl("hdfIsTGG") as HiddenField).Value);
                    string idd = (Items.FindControl("idd") as HiddenField).Value;
                    if (!(txtShaftRemark.Equals(HidShaftRemark) && txtHeadRemark.Equals(HidHeadremark)))
                    {
                        save = DataBaseAccess.UpdateFlowMeterData(ComponentID, txtTypeDia, txtAngDeg, txtHTMM, txtHTGUAGE, txtPRBAR, txtROTAMIN, txtRotaMax, txtRotaMedian, txtTestPr, txtRotaFlow, txtbarrelInspection, idd, txtShaftRemark, txtHeadRemark, isTGGType);
                    }
                    else if ((!(txtShaftRemark.Equals(HidShaftRemark, StringComparison.OrdinalIgnoreCase)) && (txtShaftRemark.Equals(HidShaftRemark, StringComparison.OrdinalIgnoreCase))) || (!(txtAngDeg.Equals(HidAngDeg, StringComparison.OrdinalIgnoreCase) && txtTypeDia.Equals(HiddTypeDia, StringComparison.OrdinalIgnoreCase) && txtHTMM.Equals(HidHTMM, StringComparison.OrdinalIgnoreCase) && txtHTGUAGE.Equals(HidHTGUAGE, StringComparison.OrdinalIgnoreCase) && txtPRBAR.Equals(HidPRBAR, StringComparison.OrdinalIgnoreCase) && txtROTAMIN.Equals(HidROTAMIN, StringComparison.OrdinalIgnoreCase) && txtRotaMax.Equals(HidRotaMax, StringComparison.OrdinalIgnoreCase) && txtRotaMedian.Equals(HidRotaMedian, StringComparison.OrdinalIgnoreCase) && txtTestPr.Equals(HidTestPr, StringComparison.OrdinalIgnoreCase) && txtRotaFlow.Equals(HidRotaFlow, StringComparison.OrdinalIgnoreCase) && txtbarrelInspection.Equals(HidbarrelInspection, StringComparison.OrdinalIgnoreCase) && isTGGType.Equals(HidTGGType))))
                    {

                        if (validatedata(ComponentID, txtTypeDia, txtAngDeg, HidAngDeg, txtHTMM, HidHTMM, txtHTGUAGE, HidHTGUAGE, txtPRBAR, HidPRBAR, txtROTAMIN, HidROTAMIN, txtRotaMax, HidRotaMax, txtRotaMedian, HidRotaMedian, txtTestPr, HidTestPr, txtRotaFlow, HidRotaFlow, txtbarrelInspection, HidbarrelInspection))
                        {
                            save = DataBaseAccess.UpdateFlowMeterData(ComponentID, txtTypeDia, txtAngDeg, txtHTMM, txtHTGUAGE, txtPRBAR, txtROTAMIN, txtRotaMax, txtRotaMedian, txtTestPr, txtRotaFlow, txtbarrelInspection, idd, txtShaftRemark, txtHeadRemark, isTGGType);
                        }
                    }

                }
                if (save)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data Updated successfully!!!')", true);
                    ddldropdow.Value = "";
                    BindGrid("");
                    btnAdd.Text = "Add";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No change in data to update')", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {

            }
        }

        private bool validateremarks(string txtshaftrem, string txtheadrem)
        {
            bool remakspresent = false;
            if (!string.IsNullOrEmpty(txtshaftrem) || !string.IsNullOrEmpty(txtheadrem))
            {
                if (string.IsNullOrEmpty(txtshaftrem))
                {

                }
                else if (string.IsNullOrEmpty(txtheadrem))
                {

                }
                else
                {
                    remakspresent = true;
                }
            }
            return remakspresent;
        }

        private bool validatedata(string componentID, string txtTypeDia, string txtAngDeg, string hidAngDeg, string txtHTMM, string hidHTMM, string txtHTGUAGE, string hidHTGUAGE, string txtPRBAR, string hidPRBAR, string txtROTAMIN, string hidROTAMIN, string txtRotaMax, string hidRotaMax, string txtRotaMedian, string hidRotaMedian, string txtTestPr, string hidTestPr, string txtRotaFlow, string hidRotaFlow, string txtbarrelInspection, string hidbarrelInspection)
        {
            bool status = false;
            try
            {
                if (string.IsNullOrEmpty(componentID))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Supplier Code Cannot be Empty')", true);

                }
                else if (string.IsNullOrEmpty(txtTypeDia))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('TypeDia Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtAngDeg))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('[Angle] Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtbarrelInspection))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Barrel Inspection Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtHTGUAGE))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Height Guage Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtHTMM))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(Height Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtPRBAR))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PR Bar Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtRotaFlow))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ROTA Flow Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtRotaMax))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ROTA max Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtRotaMedian))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ROTA Median Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtROTAMIN))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ROTA Min Cannot be Empty')", true);
                }
                else if (string.IsNullOrEmpty(txtTestPr))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Test Pr. Cannot be Empty')", true);
                }
                else
                {
                    status = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return status;
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                btnAdd.Text = "Add";
                foreach (ListViewItem Item in lstflowmeterlistview.Items)
                {
                    if ((Item.FindControl("deleteFlowMeter") as CheckBox).Checked)
                    {
                        string idd = (Item.FindControl("idd") as HiddenField).Value;
                        bool delsuccessful = DataBaseAccess.DeleteFlowMeter(idd);
                        if (delsuccessful)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Flow Meter Master data deleted successfully')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Flow Meter Master data cannot be deleted')", true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());

            }
            finally
            {
                ddldropdow.Value = "";
                BindGrid("");
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int i = 0;
            bool dataSaved = false;
            try
            {
                if (fileupload.HasFile)
                {

                    if (Path.GetExtension(fileupload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(fileupload.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        ExcelPackage Excel = new ExcelPackage(fileupload.PostedFile.InputStream);
                        var dtFlowMeter = new DataTable();
                        dtFlowMeter.Columns.Add("Slno");
                        dtFlowMeter.Columns.Add("PartName");
                        dtFlowMeter.Columns.Add("TypeDia");
                        dtFlowMeter.Columns.Add("Angle");
                        dtFlowMeter.Columns.Add("Height");
                        dtFlowMeter.Columns.Add("HeightGuage");
                        dtFlowMeter.Columns.Add("PrBar");
                        dtFlowMeter.Columns.Add("RotaMin");
                        dtFlowMeter.Columns.Add("RotaMax");
                        dtFlowMeter.Columns.Add("RotaMedian");
                        dtFlowMeter.Columns.Add("TestPr");
                        dtFlowMeter.Columns.Add("RotaFlow");
                        dtFlowMeter.Columns.Add("BarrelInscription");
                        dtFlowMeter.Columns.Add("HeadRemarks");
                        dtFlowMeter.Columns.Add("ShaftRemarks");
                        //dtFlowMeter.Columns.Add("IsTGG");
                        var ws = Excel.Workbook.Worksheets.First();
                        int startrow = 7; i = 1;
                        string heightvalue = ws.Cells["E6"].Value.ToString();
                        Regex regex = new Regex("[.. ]");
                        heightvalue = regex.Replace(heightvalue, "");
                        for (int row = startrow; row <= ws.Dimension.End.Row; row++)
                        {

                            var Rows = ws.Cells[row, 1, row, 12];
                            string s = "A" + row;
                            if (ws.Cells[row, 1].Value != null && !(ws.Cells[row, 1].Value.ToString().Contains("DATE") || ws.Cells[row, 1].Value.ToString().Contains("ORIGINAL")))
                            {
                                DataRow singlerow = dtFlowMeter.Rows.Add();
                                singlerow[0] = i++;
                                if (ws.Cells[row, 4].Value != null)
                                {
                                    foreach (var cell in Rows)
                                    {
                                        if (i == 528)
                                        {

                                        }
                                        if (!(ws.Cells[row, 1].Value.ToString().Contains("DATE") || ws.Cells[row, 1].Value.ToString().Equals("ORIGINAL", StringComparison.OrdinalIgnoreCase)))
                                        {
                                            if (cell.Start.Column == 1)
                                            {
                                                string partnumber = cell.Text.Replace(" ", "");
                                                singlerow[cell.Start.Column] = partnumber;
                                            }
                                            else if (cell.Start.Column == 5)
                                            {
                                                if (cell.Text.Contains(".") || cell.Text.Contains("…"))
                                                {
                                                    regex = new Regex("[.. …]");
                                                    string heightno = regex.Replace(cell.Text, "");
                                                    singlerow[cell.Start.Column] = (heightvalue + heightno);
                                                }
                                                else
                                                {
                                                    string heightno = cell.Text.Replace(" ", "");
                                                    singlerow[cell.Start.Column] = (heightno);
                                                }
                                            }
                                            else
                                                singlerow[cell.Start.Column] = cell.Text;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ws.Cells[row, 1].Value != null)
                                    {
                                        string partnumber = ws.Cells[row, 1].Value.ToString();
                                        partnumber = partnumber.Replace(" ", "");
                                        singlerow[1] = partnumber;
                                    }
                                    if (ws.Cells[row, 2].Value != null)
                                        singlerow[2] = ws.Cells[row, 2].Value.ToString();
                                    if (ws.Cells[row, 12].Value != null)
                                        singlerow[12] = ws.Cells[row, 12].Value.ToString();
                                    if (ws.Cells[row, 3].Value != null)
                                    {
                                        string[] remarks = ws.Cells[row, 3].Value.ToString().Split(',');
                                        int j = 13;
                                        foreach (string remark in remarks)
                                        {
                                            if (remark.Contains("Head") || remark.Contains("head") || remark.Contains("HEAD"))
                                            {
                                                string abc = remark;
                                                regex = new Regex("[a-zA-Z()\n]");
                                                abc = regex.Replace(abc, "");
                                                abc = abc.Replace(" ", "");
                                                if (abc.Substring(0, 1).Equals("."))
                                                    abc = abc.Remove(0, 1);
                                                singlerow[j] = abc + "m";
                                            }
                                            else if (remark.Contains("shaft") || remark.Contains("Shaft") || remark.Contains("SHAFT"))
                                            {
                                                string abc = remark;
                                                regex = new Regex("[a-zA-Z()\n]");
                                                abc = regex.Replace(abc, "");
                                                abc = abc.Replace(" ", "");
                                                if (abc.Substring(0, 1).Equals("."))
                                                    abc = abc.Remove(0, 1);
                                                singlerow[j + 1] = abc + "m";
                                            }
                                            else
                                            {
                                                string abc = remark;
                                                regex = new Regex("[a-zA-Z()\n]");
                                                abc = regex.Replace(abc, "");
                                                abc = abc.Replace(" ", "");
                                                if (abc.Substring(0, 1).Equals("."))
                                                    abc = abc.Remove(0, 1);
                                                singlerow[j] = abc + "m";
                                                singlerow[j + 1] = abc + "m";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (dtFlowMeter != null && dtFlowMeter.Rows.Count > 0)
                        {
                            foreach (DataRow item in dtFlowMeter.Rows)
                            {
                                dataSaved = DataBaseAccess.BoschFlowMeterImportedData(item);
                            }
                            if (dataSaved)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Excel file Imported')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Excel File not Imported')", true);
                                return;
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Excel file')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select a Excile file')", true);
                    return;
                }
            }
            catch (Exception ex)
            {

                Logger.WriteErrorLog(ex.Message);
            }
            finally
            {
                BindGrid("".ToString());
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid(ddldropdow.Value);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void lstflowmeterlistview_PagePropertiesChanged(object sender, EventArgs e)
        {
            try
            {
                DataPager DP = (lstflowmeterlistview.FindControl("DataPager1") as DataPager);
                //DP.SetPageProperties(DP.PageSize)

                BindGrid("".ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }

    public class FlowMeterBoschEntity
    {
        public int slno { get; set; }
        public int idd { get; set; }
        public string Component { get; set; }
        public List<string> ComponentList { get; set; }
        public bool ComponentlblEnable { get; set; }
        public bool ComponentdrpEnable { get; set; }
        public string TypeDia { get; set; }
        public string SettingsAng { get; set; }
        public string SettingsHt { get; set; }
        public string SettingsHTGuage { get; set; }
        public string SettingsPR { get; set; }
        public string RotaMin { get; set; }
        public string RotaMax { get; set; }
        public string RotaMedian { get; set; }
        public string ShaftTestPr { get; set; }
        public string RotaFlow { get; set; }
        public string BaralInspection { get; set; }
        public string SettingRemark { get; set; }
        public string RotaRemark { get; set; }
        public bool IsTGGType { get; set; }
    }
}