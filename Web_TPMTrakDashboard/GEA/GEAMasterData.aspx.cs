using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.GEA.DataBaseAccess;

namespace Web_TPMTrakDashboard.GEA
{
    public partial class GEAMasterData : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSaveDeleteModel("View");
                BindSaveDeleteAssemblyData("GetType");
                BindSaveDeleteAssemblyData("GetSecondaryType");
                BindSaveDeleteAssemblyData("View");
                BindSaveDeleteAssemblyData("ViewSecondary");
                BindSaveDeleteBalancingData("GetType");
                BindSaveDeleteBalancingData("View");
                BindNoiseDimensionData("");
            }
        }

        private void BindSaveDeleteModel(string Param)
        {
            try
            {
                switch (Param)
                {
                    case "View":
                        {
                            List<Model> Model = DataBaseAccess.GEADatabaseAccess.GetSaveDeleteModelData("View", "");
                            ddlModelData.DataSource = Model;
                            ddlModelData.DataBind();
                            break;
                        }
                    case "Save":
                        {
                            if (string.IsNullOrEmpty(txtModel.Text))
                            {
                                return;
                            }
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteModelData("Save", txtModel.Text);
                            break;
                        }
                    case "Delete":
                        {
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteModelData("Delete", "");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "id1", "TypeChange('ModelData');", true);
        }

        protected void btnModelSave_Click(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteModel("Save");
                BindSaveDeleteModel("View");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Saveid0", "alert('Saved Successfully');", true);
                BindNoiseDimensionData("noAjax");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnModelDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridItem row in ddlModelData.Items)
                {
                    if ((row.FindControl("chkDelete") as CheckBox).Checked)
                    {
                        DataBaseAccess.GEADatabaseAccess.GetSaveDeleteModelData("Delete", (row.FindControl("lblModel") as Label).Text);
                    }
                }
                BindSaveDeleteModel("View");
                BindNoiseDimensionData("noAjax");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindSaveDeleteAssemblyData(string Param)
        {
            AssemblyModel assembly = new AssemblyModel();
            try
            {
                switch (Param)
                {
                    case "GetType":
                        {
                            ddlAssemblyType.DataSource = DataBaseAccess.GEADatabaseAccess.GetAssemblyModelList("Main");
                            ddlAssemblyType.DataBind();
                            break;
                        }
                    case "GetSecondaryType":
                        {
                            ddlsecondaryMotorType.DataSource = DataBaseAccess.GEADatabaseAccess.GetAssemblyModelList("Secondary");
                            ddlsecondaryMotorType.DataBind();
                            break;
                        }
                    case "View":
                        {
                            if (string.IsNullOrEmpty(ddlAssemblyType.SelectedValue.ToString()))
                            {
                                return;
                            }
                            assembly._AssemblyType = ddlAssemblyType.SelectedValue.ToString();
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("view", ref assembly);
                            AssemblyModel ModelData = assembly;
                            ddlAssemblyType.SelectedValue = ModelData._AssemblyType;
                            txtAssemblyMainVoltage.Text = ModelData.MainVoltage;
                            txtAssemblyMainCurrent.Text = ModelData.MainCurrent;
                            txtAssemblyMainFrequency.Text = ModelData.MainFrequency;
                            txtAssemblyMainPower.Text = ModelData.MainPower;
                            txtAssemblyMainSpeed.Text = ModelData.MainSpeed;
                            txtAssemblyMainCosValue.Text = ModelData.MainCosValue;
                            txtAssemblyMainEfficiency.Text = ModelData.MainEfficiency;
                            txtAssemblyMainBearingDrive.Text = ModelData.MainBearing;
                            txtAssemblyMainBearingNonDrive.Text = ModelData.MainNonBearing;
                            break;
                        }
                    case "ViewSecondary":
                        {
                            if (string.IsNullOrEmpty(ddlsecondaryMotorType.SelectedValue.ToString()))
                            {
                                return;
                            }
                            assembly._AssemblySecondaryType = ddlsecondaryMotorType.SelectedValue.ToString();
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("viewsecondary", ref assembly);
                            AssemblyModel ModelData = assembly;
                            txtAssemblySecondaryVoltage.Text = ModelData.SecondaryVoltage;
                            txtAssemblySecondaryCurrent.Text = ModelData.SecondaryCurrent;
                            txtAssemblySecondaryFrequency.Text = ModelData.SecondaryFrequency;
                            txtAssemblySecondaryPower.Text = ModelData.SecondaryPower;
                            txtAssemblySecondarySpeed.Text = ModelData.SecondarySpeed;
                            txtAssemblySecondaryCosValue.Text = ModelData.SecondaryCosValue;
                            txtAssemblySecondaryEfficiency.Text = ModelData.SecondaryEfficiency;
                            txtAssemblySecondaryBearingDrive.Text = ModelData.SecondaryBearing;
                            txtAssemblySecondaryBearingNonDrive.Text = ModelData.SecondaryNonBearing;
                            break;
                        }
                    case "SaveType":
                        {
                            if (string.IsNullOrEmpty(txtAssemblyType.Text))
                            {
                                return;
                            }
                            assembly._AssemblyType = txtAssemblyType.Text;
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("Save", ref assembly);

                            break;
                        }
                    case "SaveSecondaryType":
                        {
                            if (string.IsNullOrEmpty(txtSecondaryMotorType.Text))
                            {
                                return;
                            }
                            assembly._AssemblySecondaryType = txtSecondaryMotorType.Text;
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("SaveSecondary", ref assembly);

                            break;
                        }
                    case "SaveSpecification":
                        {
                            if (string.IsNullOrEmpty(ddlAssemblyType.SelectedValue.ToString()))
                            {
                                return;
                            }
                            assembly = new AssemblyModel
                            {
                                _AssemblyType = ddlAssemblyType.SelectedValue.ToString(),
                                MainVoltage = txtAssemblyMainVoltage.Text,
                                MainCurrent = txtAssemblyMainCurrent.Text,
                                MainFrequency = txtAssemblyMainFrequency.Text,
                                MainPower = txtAssemblyMainPower.Text,
                                MainSpeed = txtAssemblyMainSpeed.Text,
                                MainCosValue = txtAssemblyMainCosValue.Text,
                                MainEfficiency = txtAssemblyMainEfficiency.Text,
                                MainBearing = txtAssemblyMainBearingDrive.Text,
                                MainNonBearing = txtAssemblyMainBearingNonDrive.Text,

                            };
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("save", ref assembly);
                            break;
                        }
                    case "SaveSecondarySpecification":
                        {
                            if (string.IsNullOrEmpty(ddlsecondaryMotorType.SelectedValue.ToString()))
                            {
                                return;
                            }
                            assembly = new AssemblyModel
                            {
                                _AssemblySecondaryType = ddlsecondaryMotorType.SelectedValue.ToString(),
                                SecondaryVoltage = txtAssemblySecondaryVoltage.Text,
                                SecondaryCurrent = txtAssemblySecondaryCurrent.Text,
                                SecondaryFrequency = txtAssemblySecondaryFrequency.Text,
                                SecondaryPower = txtAssemblySecondaryPower.Text,
                                SecondarySpeed = txtAssemblySecondarySpeed.Text,
                                SecondaryCosValue = txtAssemblySecondaryCosValue.Text,
                                SecondaryEfficiency = txtAssemblySecondaryEfficiency.Text,
                                SecondaryBearing = txtAssemblySecondaryBearingDrive.Text,
                                SecondaryNonBearing = txtAssemblySecondaryBearingNonDrive.Text,
                            };
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("savesecondary", ref assembly);
                            break;
                        }
                    case "Delete":
                        {
                            if (string.IsNullOrEmpty(ddlAssemblyType.SelectedValue.ToString()))
                            {
                                return;
                            }
                            assembly._AssemblyType = ddlAssemblyType.SelectedValue.ToString();
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("delete", ref assembly);
                            break;
                        }
                    case "DeleteSecondary":
                        {
                            if (string.IsNullOrEmpty(ddlsecondaryMotorType.SelectedValue.ToString()))
                            {
                                return;
                            }
                            assembly._AssemblyType = ddlsecondaryMotorType.SelectedValue.ToString();
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteAssemblyModel("deletesecondary", ref assembly);
                            break;
                        }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "id2", "TypeChange('AssemblyData')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnAssemplyTypeSave_Click(object sender, EventArgs e)
        {
            try
            {

                BindSaveDeleteAssemblyData("SaveType");
                BindSaveDeleteAssemblyData("GetType");
                BindSaveDeleteAssemblyData("View");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Saveid10", "alert('Saved Successfully')", true);
                txtAssemblyType.Text = "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnSecondarymotor_Click(object sender, EventArgs e)
        {
            try
            {

                BindSaveDeleteAssemblyData("SaveSecondaryType");
                txtSecondaryMotorType.Text = "";
                BindSaveDeleteAssemblyData("GetSecondaryType");
                BindSaveDeleteAssemblyData("ViewSecondary");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Saveid11", "alert('Saved Successfully')", true);
                txtAssemblyType.Text = "";
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void ddlAssemblyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteAssemblyData("View");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void ddlsecondaryMotorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteAssemblyData("ViewSecondary");

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnSaveSecondary_Click(object sender, EventArgs e)
        {
            BindSaveDeleteAssemblyData("SaveSecondarySpecification");
            BindSaveDeleteAssemblyData("GetSecondaryType");
            BindSaveDeleteAssemblyData("ViewSecondary");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Saveid12", "alert('Saved Successfully')", true);
            txtAssemblyType.Text = "";
        }


        protected void btnSaveSpecification_Click(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteAssemblyData("SaveSpecification");
                BindSaveDeleteAssemblyData("View");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Saveid1", "alert('Saved Successfully')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        protected void btnDeleteSecondary_Click(object sender, EventArgs e)
        {
            BindSaveDeleteAssemblyData("DeleteSecondary");
            BindSaveDeleteAssemblyData("GetSecondaryType");
            BindSaveDeleteAssemblyData("ViewSecondary");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Deleteid2", "alert('Deleted Successfully')", true);
        }
        protected void btnDeleteSpectification_Click(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteAssemblyData("Delete");
                BindSaveDeleteAssemblyData("GetType");
                BindSaveDeleteAssemblyData("View");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Deleteid2", "alert('Deleted Successfully')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        private void BindSaveDeleteBalancingData(string Param)
        {
            BalancingModel balancing = new BalancingModel();
            try
            {
                switch (Param)
                {
                    case "GetType":
                        {
                            ddlBalancingType.DataSource = DataBaseAccess.GEADatabaseAccess.GetBalancingModelList();
                            ddlBalancingType.DataBind();
                            break;
                        }
                    case "View":
                        {
                            if ((string.IsNullOrEmpty(ddlBalancingType.SelectedValue.ToString())))
                            {
                                return;
                            }
                            balancing._BalancingType = ddlBalancingType.SelectedValue.ToString();
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteBalancingModel("view", ref balancing);
                            BalancingModel Model = balancing;

                            txtP1Tol.Text = Model.P1Tol;
                            txtP1r.Text = Model.P1R;
                            txtP1Dim.Text = Model.P1DimA;
                            txtP1DimA.Text = Model.P1DimB;
                            txtP1ISO.Text = Model.P1ISO;
                            txtP1Unit.Text = Model.P1Unt;

                            txtP2Tol.Text = Model.P2Tol;
                            txtP2r.Text = Model.P2R;
                            txtP2Dim.Text = Model.P2DimA;
                            txtP2DimA.Text = Model.P2DimB;
                            txtP2ISO.Text = Model.P2ISO;
                            txtP2Unit.Text = Model.P2Unt;

                            txtSTTOl.Text = Model.STTol;
                            txtSTr.Text = Model.STR;
                            txtSTDim.Text = Model.STDimA;
                            txtSTDimA.Text = Model.STDimB;
                            txtSTISO.Text = Model.STISO;
                            txtSTUnit.Text = Model.STUnt;
                            break;
                        }
                    case "SaveType":
                        {
                            if ((string.IsNullOrEmpty(txtBalancingType.Text)))
                            {
                                return;
                            }
                            balancing._BalancingType = txtBalancingType.Text.ToString();

                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteBalancingModel("save", ref balancing);
                            break;
                        }
                    case "SaveSpecification":
                        {
                            if ((string.IsNullOrEmpty(ddlBalancingType.SelectedValue.ToString())))
                            {
                                return;
                            }
                            balancing._BalancingType = ddlBalancingType.SelectedValue.ToString();
                            balancing.P1Tol = txtP1Tol.Text;
                            balancing.P1R = txtP1r.Text;
                            balancing.P1DimA = txtP1Dim.Text;
                            balancing.P1DimB = txtP1DimA.Text;
                            balancing.P1ISO = txtP1ISO.Text;
                            balancing.P1Unt = txtP1Unit.Text;
                            balancing.P2Tol = txtP2Tol.Text;
                            balancing.P2R = txtP2r.Text;
                            balancing.P2DimA = txtP2Dim.Text;
                            balancing.P2DimB = txtP2DimA.Text;
                            balancing.P2ISO = txtP2ISO.Text;
                            balancing.P2Unt = txtP2Unit.Text;
                            balancing.STTol = txtSTTOl.Text;
                            balancing.STR = txtSTr.Text;
                            balancing.STDimA = txtSTDim.Text;
                            balancing.STDimB = txtSTDimA.Text;
                            balancing.STISO = txtSTISO.Text;
                            balancing.STUnt = txtSTUnit.Text;
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteBalancingModel("save", ref balancing);
                            break;
                        }
                    case "Delete":
                        {
                            balancing._BalancingType = ddlBalancingType.SelectedValue.ToString();
                            DataBaseAccess.GEADatabaseAccess.GetSaveDeleteBalancingModel("delete", ref balancing);
                            break;
                        }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "id3", "TypeChange('BalancingData')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnBalancingType_Click(object sender, EventArgs e)
        {
            BindSaveDeleteBalancingData("SaveType");
            BindSaveDeleteBalancingData("GetType");
            BindSaveDeleteBalancingData("View");
            txtBalancingType.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "saveid3", "alert('Saved Successfully')", true);
        }

        protected void ddlBalancingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteBalancingData("View");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnSaveBalancingSpecfication_Click(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteBalancingData("SaveSpecification");
                BindSaveDeleteBalancingData("View");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "saveid4", "alert('Saved Successfully')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }

        protected void btnDeleteBalancingSpecfication_Click(object sender, EventArgs e)
        {
            try
            {
                BindSaveDeleteBalancingData("Delete");
                BindSaveDeleteBalancingData("GetType");
                BindSaveDeleteBalancingData("View");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Deleteid3", "alert('Deleted Successfully')", true);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.ToString());
            }
        }
        private void BindNoiseDimensionData(string param)
        {
            try
            {
                List<NoiseDimensionModel> list = GEADatabaseAccess.getNoiseMeasurementDimensionMasterDara();
                gvNoiseDimension.DataSource = list;
                gvNoiseDimension.DataBind();
                if (string.IsNullOrEmpty(param))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "id3", "TypeChange('NoiseDimensionData');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("BindNoiseDimensionData = " + ex.Message);
            }
        }
        protected void btnSaveNoiseDimension_Click(object sender, EventArgs e)
        {
            try
            {
                int flag = 0;
                for (int i = 0; i < gvNoiseDimension.Rows.Count; i++)
                {
                    var row = gvNoiseDimension.Rows[i];
                    if ((row.FindControl("hdnUpdate") as HiddenField).Value.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        NoiseDimensionModel data = new NoiseDimensionModel();
                        data.Model = (row.FindControl("lblModel") as Label).Text;
                        data.Dimension = (row.FindControl("txtDimension") as TextBox).Text;
                        data.Value = (row.FindControl("txtValue") as TextBox).Text;
                        flag += GEADatabaseAccess.saveNoiseMeasurementDimensionMasterDara(data);
                    }
                }
                if (flag > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "saveNoise", "alert('Saved Successfully');", true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("btnNoiseDimension_Click = " + ex.Message);
            }
            BindNoiseDimensionData("");
        }


    }

    public class Model
    {
        public int Slno { get; set; }
        public string _Model { get; set; }
    }

    public class AssemblyModel
    {
        public int Slno { get; set; }
        public string _AssemblyType { get; set; }
        public string _AssemblySecondaryType { get; set; }
        public string MainVoltage { get; set; }
        public string MainCurrent { get; set; }
        public string MainSpeed { get; set; }
        public string MainFrequency { get; set; }
        public string MainPower { get; set; }
        public string MainEfficiency { get; set; }
        public string MainCosValue { get; set; }
        public string MainBearing { get; set; }
        public string MainNonBearing { get; set; }
        public string SecondaryVoltage { get; set; }
        public string SecondaryCurrent { get; set; }
        public string SecondarySpeed { get; set; }
        public string SecondaryFrequency { get; set; }
        public string SecondaryPower { get; set; }
        public string SecondaryEfficiency { get; set; }
        public string SecondaryCosValue { get; set; }
        public string SecondaryBearing { get; set; }
        public string SecondaryNonBearing { get; set; }
    }
    public class BalancingModel
    {
        public int Slno { get; set; }
        public string _BalancingType { get; set; }
        public string P1Tol { get; set; }
        public string P1R { get; set; }
        public string P1DimA { get; set; }
        public string P1DimB { get; set; }
        public string P1ISO { get; set; }
        public string P1Unt { get; set; }
        public string STTol { get; set; }
        public string STR { get; set; }
        public string STDimA { get; set; }
        public string STDimB { get; set; }
        public string STISO { get; set; }
        public string STUnt { get; set; }
        public string P2Tol { get; set; }
        public string P2R { get; set; }
        public string P2DimA { get; set; }
        public string P2DimB { get; set; }
        public string P2ISO { get; set; }
        public string P2Unt { get; set; }


    }
    public class NoiseDimensionModel
    {
        public string Model { get; set; }
        public string Dimension { get; set; }
        public string Value { get; set; }
        public string Formulaset { get; set; }
    }
}