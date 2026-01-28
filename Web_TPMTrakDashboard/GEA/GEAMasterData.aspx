<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GEAMasterData.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.GEAMasterData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="ui segment">
                    <select name="type" id="type" onchange="TypeChanges();" class="ui dropdown" style="width: 300px">
                        <option value="ModelData">Model Data</option>
                        <option value="AssemblyData">Assembly Data</option>
                        <option value="BalancingData">Balancing Data</option>
                        <option value="NoiseDimensionData">Noise Dimension Data</option>
                    </select>
                </div>
                <div id="divModel">
                    <div class="ui segment">
                        <h3>Model Data
                        </h3>
                        <div style="height: 80px">
                            <div class="ui grid">
                                <div class="ui input focus four wide column">
                                    <asp:TextBox runat="server" ID="txtModel" placeholder="Enter New Model Type" Width="400" />
                                </div>
                                <div class="four wide column">
                                    <asp:Button runat="server" ID="btnModelSave" CssClass="positive ui button" Text="Save New Model Type" OnClick="btnModelSave_Click"></asp:Button>
                                     <asp:Button runat="server" ID="btnModelDelete" CssClass="positive ui button" Text="Delete" OnClientClick="return deleteValidation();" OnClick="btnModelDelete_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div margin="5px">
                            <asp:DataGrid runat="server" ID="ddlModelData" AutoGenerateColumns="false" class="ui blue table" ClientIDMode="Static">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Model">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblModel" Text='<%# Eval("_Model") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                     <asp:TemplateColumn HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDelete" ClientIDMode="Static" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
                <div id="divAssemblyData">
                    <div class="ui segment">
                        <h3>Assembly Data
                        </h3>
                        <div style="margin: 5px; height: 60px;">
                            <div class="ui grid">
                                <div class="ui input focus four wide column">
                                    <asp:TextBox runat="server" ID="txtAssemblyType" placeholder="Enter New Main Motor Type" Width="400" />
                                </div>
                                <div class="four wide column">
                                    <asp:Button runat="server" ID="btnAssemplyTypeSave" CssClass="positive ui button" Text="Save New Main Motor Type" Width="240" OnClick="btnAssemplyTypeSave_Click"></asp:Button>
                                </div>
                                <div class="ui input focus four wide column">
                                    <asp:TextBox runat="server" ID="txtSecondaryMotorType" placeholder="Enter New Secondary Motor Type" Width="400" />
                                </div>
                                <div class="four wide column">
                                    <asp:Button runat="server" ID="btnSecondarymotor" CssClass="positive ui button" Text="Save New Secondary Motor Type" Width="240" OnClick="btnSecondarymotor_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div style="height: 80px;" class=" ui grid">
                                <div class="two wide column" style="text-align: center; top: 10px; width: 150px!important">
                                    <span>Main Motor type</span>
                                </div>
                                <div class="two wide column">
                                    <asp:DropDownList runat="server" ID="ddlAssemblyType" CssClass="ui dropdown" Width="200" OnSelectedIndexChanged="ddlAssemblyType_SelectedIndexChanged" AutoPostBack="true" />
                                </div>
                                <div class="two wide column">
                                    <asp:Button runat="server" ID="btnSaveSpecification" CssClass="positive ui button" Text="Save Main Data" Width="150" OnClick="btnSaveSpecification_Click"></asp:Button>
                                </div>
                                <div class="two wide column">
                                    <asp:Button runat="server" ID="btnDeleteSpectification" CssClass="negative ui button" Text="Delete Main Data" Width="150" OnClick="btnDeleteSpectification_Click"></asp:Button>
                                </div>
                                <div class="two wide column" style="text-align: center; top: 10px; width: 150px!important">
                                    <span>Secondary Motor type</span>
                                </div>
                                <div class="two wide column">
                                    <asp:DropDownList runat="server" ID="ddlsecondaryMotorType" CssClass="ui dropdown" Width="200" OnSelectedIndexChanged="ddlsecondaryMotorType_SelectedIndexChanged" AutoPostBack="true" />
                                </div>
                                <div class="two wide column">
                                    <asp:Button runat="server" ID="btnSaveSecondary" CssClass="positive ui button" Text="Save Secondary Data" Width="170" OnClick="btnSaveSecondary_Click"></asp:Button>
                                </div>
                                <div class="two wide column">
                                    <asp:Button runat="server" ID="btnDeleteSecondary" CssClass="negative ui button" Text="Delete Secondary Data" Width="170" OnClick="btnDeleteSecondary_Click"></asp:Button>
                                </div>
                            </div>
                            <div style="margin: 5px; height: 600px;">
                                <div class="ui grid">
                                    <table class="ui blue table" style="text-align: center">
                                        <thead>
                                            <tr>
                                                <th style="text-align: left; width: 200px;">Specification</th>
                                                <th>Main Motor</th>
                                                <th>Secondary Motor</th>
                                                <th style="width: 200px"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Voltage (V)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainVoltage" placeholder="Voltage" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryVoltage" placeholder="Voltage" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Current (A)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainCurrent" placeholder="Current" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryCurrent" placeholder="Current" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Frequency (Hz)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainFrequency" placeholder="Frequency" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryFrequency" placeholder="Frequency" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Power (KW)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainPower" placeholder="Power" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryPower" placeholder="Power" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Speed (RPM)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainSpeed" placeholder="Speed" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondarySpeed" placeholder="Speed" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Cos Value</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainCosValue" placeholder="Cos Value" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryCosValue" placeholder="Cos Value" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Efficiency (%)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainEfficiency" placeholder="Efficiency" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryEfficiency" placeholder="Efficiency" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Bearing (Drive)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainBearingDrive" placeholder="Bearing Drive" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryBearingDrive" placeholder="Bearing Drive" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: auto;">Bearing (Non-Drive)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblyMainBearingNonDrive" placeholder="Bearing Non-Drive" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtAssemblySecondaryBearingNonDrive" placeholder="Bearing Non-Drive" />
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <div id="divBalancingData">
                    <div class="ui segment">
                        <h3>Balancing Data
                        </h3>
                        <div style="margin: 5px; height: 60px;">
                            <div class="ui grid">
                                <div class="ui input focus four wide column">
                                    <asp:TextBox runat="server" ID="txtBalancingType" placeholder="Enter New Balancing Model" Width="400" />
                                </div>
                                <div class="four wide column">
                                    <asp:Button runat="server" ID="btnBalancingType" CssClass="positive ui button" Text="Save New Balancing Type" Width="187" OnClick="btnBalancingType_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div style="height: 80px;" class=" ui grid">
                                <div class="four wide column">
                                    <asp:DropDownList runat="server" ID="ddlBalancingType" CssClass="ui dropdown" Width="390" OnSelectedIndexChanged="ddlBalancingType_SelectedIndexChanged" AutoPostBack="true" />
                                </div>
                                <div class="two wide column">
                                    <asp:Button runat="server" ID="btnSaveBalancingSpecfication" CssClass="positive ui button" Text="Save Specification Data" Width="190" OnClick="btnSaveBalancingSpecfication_Click"></asp:Button>
                                </div>
                                <div class="four wide column">
                                    <asp:Button runat="server" ID="btnDeleteBalancingSpecfication" CssClass="negative ui button" Text="Delete Specification Data" Width="190" OnClick="btnDeleteBalancingSpecfication_Click"></asp:Button>
                                </div>
                            </div>
                            <div style="margin: 5px; height: 600px;">
                                <div class="ui grid">
                                    <table class="ui blue table" style="text-align: center">
                                        <thead>
                                            <tr>
                                                <th style="text-align: left;">Specification</th>
                                                <th>P1</th>
                                                <th>ST</th>
                                                <th>P2</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="text-align: left;">Tol(g)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP1Tol" placeholder="P1 Tol" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtSTTOl" placeholder="ST Tol" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP2Tol" placeholder="P2 Tol" />
                                                    </div>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="text-align: left;">r (mm)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP1r" placeholder="P1 R" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtSTr" placeholder="St R" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP2r" placeholder="P2 R" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="2" style="text-align: left; vertical-align: middle">Dim(mm)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP1Dim" placeholder="P1 Dim" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtSTDim" placeholder="ST Dim" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP2Dim" placeholder="P2 Dim" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP1DimA" Text="a" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtSTDimA" Text="b" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP2DimA" Text="c" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;">ISO</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP1ISO" placeholder="P1 ISO" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtSTISO" placeholder="ST ISO" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP2ISO" placeholder="P2 ISO" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;">Unit (KG)</td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP1Unit" placeholder="P1 Unit" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtSTUnit" placeholder="ST Unit" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="ui input focus four wide column">
                                                        <asp:TextBox runat="server" ID="txtP2Unit" placeholder="P2 Unit" />
                                                    </div>
                                                </td>
                                            </tr>

                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div id="divNoiseMeasurementDimensionData">
                    <div class="ui segment">
                        <h3>Noise Dimension Data
                        </h3>
                        <div style="height: 80px">
                            <div class="ui grid">
                                <div class="four wide column">
                                    <asp:Button runat="server" ID="btnSaveNoiseDimension" CssClass="positive ui button" Text="Save" OnClick="btnSaveNoiseDimension_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div margin="5px">
                            <asp:GridView ID="gvNoiseDimension" CssClass="table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static">
                                <Columns>
                                    <asp:TemplateField HeaderText="Model">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                            <asp:Label ID="lblModel" runat="server" Text='<%# Eval("Model") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dimension">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtDimension" ClientIDMode="Static" Text='<%# Eval("Dimension") %>' CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtValue" ClientIDMode="Static" Text='<%# Eval("Value") %>' CssClass="form-control allowDecimal"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            $("#divAssemblyData").css('display', 'none');
            $("#divBalancingData").css('display', 'none');
            $("#divNoiseMeasurementDimensionData").css('display', 'none');
        });
        $("#gvNoiseDimension").on("blur", "td", function () {
            $(this).closest('tr').find("#hdnUpdate").val("update");
        });
        function deleteValidation() {
            debugger;
            var flag = 0;
            var rows = $("#ddlModelData tr:gt(0)");
            for (var i = 0; i < rows.length; i++) {
                if ($(rows[i]).find("#chkDelete").prop("checked")) {
                    flag++;
                    break;
                }
            }
            if (flag == 0) {
                alert("Please select record.");
                return false;
            }
            return true;
        }
        $('.allowDecimal').keypress(function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            debugger;
            if (charCode == 45 && pos != 0) {
                return false;
            } else if (charCode == 43 && pos != 0) {
                return false;
            } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                return false;
            } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                return false;
            }
            return true;
        });
        function TypeChanges() {
            debugger;
            switch ($("#type").val()) {
                case "ModelData":
                    $("#divModel").css('display', '');
                    $("#divAssemblyData").css('display', 'none');
                    $("#divBalancingData").css('display', 'none');
                    $("#divNoiseMeasurementDimensionData").css('display', 'none');
                    break;
                case "AssemblyData":
                    $("#divModel").css('display', 'none');
                    $("#divAssemblyData").css('display', '');
                    $("#divBalancingData").css('display', 'none');
                    $("#divNoiseMeasurementDimensionData").css('display', 'none');
                    break;
                case "BalancingData":
                    $("#divModel").css('display', 'none');
                    $("#divAssemblyData").css('display', 'none');
                    $("#divBalancingData").css('display', '');
                    $("#divNoiseMeasurementDimensionData").css('display', 'none');
                    break;
                case "NoiseDimensionData":
                    $("#divModel").css('display', 'none');
                    $("#divAssemblyData").css('display', 'none');
                    $("#divBalancingData").css('display', 'none');
                    $("#divNoiseMeasurementDimensionData").css('display', '');
                    break;
            }
        }
        function TypeChange(Param) {
            debugger;
            switch (Param) {
                case "ModelData":
                    $("#type").val('ModelData');
                    $("#divModel").css('display', '');
                    $("#divAssemblyData").css('display', 'none');
                    $("#divBalancingData").css('display', 'none');
                    $("#divNoiseMeasurementDimensionData").css('display', 'none');
                    break;
                case "AssemblyData":
                    $("#type").val('AssemblyData');
                    $("#divModel").css('display', 'none');
                    $("#divAssemblyData").css('display', '');
                    $("#divBalancingData").css('display', 'none');
                    $("#divNoiseMeasurementDimensionData").css('display', 'none');
                    break;
                case "BalancingData":
                    $("#type").val('BalancingData');
                    $("#divModel").css('display', 'none');
                    $("#divAssemblyData").css('display', 'none');
                    $("#divBalancingData").css('display', '');
                    $("#divNoiseMeasurementDimensionData").css('display', 'none');
                    break;
                case "NoiseDimensionData":
                    $("#type").val('NoiseDimensionData');
                    $("#divModel").css('display', 'none');
                    $("#divAssemblyData").css('display', 'none');
                    $("#divBalancingData").css('display', 'none');
                    $("#divNoiseMeasurementDimensionData").css('display', '');
                    break;
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#gvNoiseDimension").on("blur", "td", function () {
                $(this).closest('tr').find("#hdnUpdate").val("update");
            });
            $('.allowDecimal').keypress(function (evt) {
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                debugger;
                if (charCode == 45 && pos != 0) {
                    return false;
                } else if (charCode == 43 && pos != 0) {
                    return false;
                } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                    return false;
                } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                    return false;
                }
                return true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
