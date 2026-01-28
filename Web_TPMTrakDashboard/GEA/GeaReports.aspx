<%@ Page Language="C#" Title="GEA Reports" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="GeaReports.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.GeaReports" EnableEventValidation="false" Async="true" AsyncTimeout="120000" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>

    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }

        #tblfilter tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #tblfilter tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .cssclass {
            min-width: 350px;
        }
    </style>
    <div class="row" style="text-align: center; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
        <asp:HiddenField ID="width" runat="server" />
        <asp:HiddenField ID="height" runat="server" />
    </div>

    <div class="row">
        <div class="col-md-9" style="margin: auto;">
            <h1 class="text-center login-title commontd">Reports</h1>
            <div class="account-wall">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-signin">
                        <asp:UpdatePanel ID="upadetPanalRepor" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnGenerate" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblfilter" class="table table-bordered table-striped" style="width: auto">

                                    <tr>
                                        <td>
                                            <b>Report Type</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                <asp:ListItem Text=" GEA Production Report" Value="GEAProductionReport" />
                                                <%--<asp:ListItem Text="Quality Incoming Report" Value="QualityIncomingReport" />--%>
                                                <asp:ListItem Text="Quality Report" Value="QualityIncomingReport" />
                                                <asp:ListItem Text="Non - Machining Reports" Value="NonMachiningReport" />
                                                <asp:ListItem Text="Decanter Acceptance Test Card Report" Value="DecanterAcceptanceTestCardReport" />
                                                <asp:ListItem Text="Final Report" Value="AssemblyTestingPackingReport" />
                                                <asp:ListItem Text="Balancing Report" Value="BalancingCertificate" />
                                                <asp:ListItem Text="Production Order Status" Value="ProdOrderStatusReport" />
                                                <asp:ListItem Text="CE Checklist Report" Value="CEChecklistReport" />
                                                <asp:ListItem Text="Pro Decanter Final Report" Value="ProDecanterReport" />
                                                <asp:ListItem Text="Machine Mix Report" Value="MachineMixReport" />
                                                <asp:ListItem Text="Parked Order Reasons" Value="ParkedOrderReasons" />
                                                <asp:ListItem Text="Production Schedule Report" Value="ProductionScheduleReport" />
                                                <asp:ListItem Text="Monthly Operator Efficiency Report" Value="MonthlyOperatorEfficiencyReport" />
                                                <asp:ListItem Text="Model std.Time vs Actual" Value="ModelStdTimevsActual" />
                                                <asp:ListItem Text="MachineWise Assembly Report" Value="MachineWiseAssemblyReport"></asp:ListItem>
                                            </asp:DropDownList>

                                        </td>
                                    </tr>
                                    <tr id="trType" runat="server">
                                        <td>
                                            <b>Type</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control cssclass" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>

                                        </td>
                                    </tr>
                                    <tr id="trFormatType" runat="server">
                                        <td runat="server">
                                            <b>Format Type</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlFormatType" runat="server" CssClass="  form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlFormatType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trSubFormat" runat="server">
                                        <td runat="server">
                                            <b>Type</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlSubFormat" runat="server" CssClass="  form-control cssclass" AutoPostBack="true" OnSelectedIndexChanged="ddlSubFormat_SelectedIndexChanged">
                                                <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                                <asp:ListItem Text="Missing" Value="Missing"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trfromdate" runat="server">
                                        <td>
                                            <b>From Date</b>
                                        </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtfromoDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="txtfromoDate_TextChanged" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trtodate" runat="server">
                                        <td>
                                            <b>To Date</b>
                                        </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trYear" runat="server">
                                        <td>
                                            <b>Year</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" placeholder="YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trPlant" runat="server">
                                        <td>
                                            <b>Plant ID</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPlantID" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trCell" runat="server">
                                        <td>
                                            <b>Cell ID</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCellID" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trMachine" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="  form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged">
                                                <asp:ListItem Value="All">All</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ListBox ID="ddlMultiMachineId" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trGEAQualityMachines" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlQualityMachines" runat="server" CssClass="  form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlQualityMachines_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trNonMachineMachineID" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlNonMachineMachineID" runat="server" CssClass="  form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlNonMachineMachineID_SelectedIndexChanged">
                                                <asp:ListItem Text="Assembly" Value="Assembly"></asp:ListItem>
                                                <asp:ListItem Text="Packing" Value="Packing"></asp:ListItem>
                                                <asp:ListItem Text="Testing" Value="Testing"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr id="trPartID" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","ComponentID") %></b> </td>
                                        <td runat="server">
                                            <div runat="server" id="divCompSearch" style="display: inline-block;">
                                                <asp:TextBox runat="server" ID="txtCompSearch" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 150px"></asp:TextBox>
                                                <asp:LinkButton runat="server" ID="lnkCompSearch" ClientIDMode="Static" CssClass="glyphicon glyphicon-search" Style="display: inline-block;" OnClick="lnkCompSearch_Click"></asp:LinkButton>&nbsp;&nbsp;
                                            </div>
                                            <asp:DropDownList ID="ddlComponent" runat="server" CssClass="form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" Style="display: inline-block; width: 150px; min-width: 150px;">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trProdOrder" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","ProdOrder") %></b>
                                        </td>
                                        <td runat="server">
                                            <div runat="server" id="divProdOrderSearch" style="display: inline-block;">
                                                <asp:TextBox runat="server" ID="txtProdOrderSearch" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 150px"></asp:TextBox>
                                                <asp:LinkButton runat="server" ID="lnkProdOrderSearch" ClientIDMode="Static" CssClass="glyphicon glyphicon-search" Style="display: inline-block;" OnClick="lnkProdOrderSearch_Click"></asp:LinkButton>&nbsp;&nbsp;
                                            </div>
                                            <asp:DropDownList ID="ddlProdOrder" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlProdOrder_SelectedIndexChanged" Style="display: inline-block; width: 150px; min-width: 150px;">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trGRNNumber" runat="server">
                                        <td runat="server">
                                            <b>GRN Number</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlGRNNumber" runat="server" CssClass="select form-control loadData cssclass" Style="display: inline-block; width: 150px;">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Label runat="server" ID="lblGrnMessage" ClientIDMode="Static" Style="font-size: 13px; color: red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trOperation" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","OperationNo") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperation" runat="server" CssClass="form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trInsPlan" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","InsPlan") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlInsPlanNumber" runat="server" CssClass="form-control cssclass" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trProType" runat="server">
                                        <td runat="server">
                                            <b>Pro Type</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" CssClass="form-control cssclass" AutoPostBack="true" ID="ddlProType" OnSelectedIndexChanged="ddlProType_SelectedIndexChanged">
                                                <asp:ListItem Text="Pro" Value="Pro"></asp:ListItem>
                                                <asp:ListItem Text="Non-Pro" Value="NonPro"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trFabrication" runat="server">
                                        <td runat="server">
                                            <b>Fabrication Number</b>
                                        </td>
                                        <td runat="server">
                                            <div runat="server" id="divFabricationSearch" style="display: inline-block;">
                                                <asp:TextBox runat="server" ID="txtFabricationSearch" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 150px"></asp:TextBox>
                                                <asp:LinkButton runat="server" ID="lnkFabricationSearch" ClientIDMode="Static" CssClass="glyphicon glyphicon-search" Style="display: inline-block;" OnClick="lnkFabricationSearch_Click"></asp:LinkButton>&nbsp;&nbsp;
                                            </div>
                                            <asp:DropDownList ID="ddlFabriation" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlFabriation_SelectedIndexChanged" Style="display: inline-block; width: 150px; min-width: 150px;">
                                            </asp:DropDownList>
                                            <asp:ListBox ID="ddlMultiFabNum" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trShift" runat="server">
                                        <td runat="server">
                                            <b>Shift</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" CssClass="form-control cssclass" AutoPostBack="true" ID="ddlShift">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trOperator" runat="server">
                                        <td runat="server">
                                            <b>Operator</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlOperatorID" runat="server" CssClass="form-control cssclass" AutoPostBack="true" Visible="false">
                                                <%--<asp:ListItem Value="All">All</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <asp:ListBox ID="ddlMultiOperatorID" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trProcessType" runat="server">
                                        <td runat="server">
                                            <b>Process Type</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlProcessType" runat="server" CssClass="form-control cssclass" AutoPostBack="true">
                                                <asp:ListItem Text="Machining" Value="Machining"></asp:ListItem>
                                                <asp:ListItem Text="Non-Machining" Value="NonMachining"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trStatus" runat="server">
                                        <td runat="server">
                                            <b>Status</b>
                                        </td>
                                        <td runat="server">
                                            <asp:ListBox ID="ddlMultistatus" runat="server" SelectionMode="Multiple" CssClass="form-control cssclass">
                                                <asp:ListItem Text="New" Value="New" Selected="True" />
                                                <asp:ListItem Text="Running" Value="Running" Selected="True" />
                                                <asp:ListItem Text="Parked" Value="Parked" Selected="True" />
                                                <asp:ListItem Text="Completed" Value="Completed" Selected="False" />
                                                <asp:ListItem Text="Pending Inspection Completion" Value="PendingInspectionCompletion" Selected="False" />
                                            </asp:ListBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" meta:resourcekey="btnGenerateResource1" OnClick="btnGenerate_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function messageSuccess() {
            Command: toastr["success"]("Report Generated Succesfully.")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function messageFailure() {
            Command: toastr["error"]("Report generation failed. Please Try Again!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function messageNodata() {
            Command: toastr["error"]("No data found for selected parameters!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function messageTemplateNotFound() {
            Command: toastr["error"]("Report template does not found in source directory!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        $(document).ready(function () {

            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#txtYear").datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiOperatorID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiFabNum]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultistatus]').multiselect({
                includeSelectAllOption: true
            });
            $.unblockUI({});
            //alert(1);
            $("#txtfromoDate").on("change", function () {
                showLoader();
            });
            $("#txtToDate").on("change", function () {
                showLoader();
            });
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                //alert(2);
                setTimeout(function () { $.unblockUI({}) }, 700);
                $("#txtfromoDate").on("change", function () {
                    showLoader();
                });
                $("#txtToDate").on("change", function () {
                    showLoader();
                });
            });

            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#txtYear").datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiOperatorID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiFabNum]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultistatus]').multiselect({
                includeSelectAllOption: true
            });

        });
    </script>
</asp:Content>

<asp:Content ID="FooterContent" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
