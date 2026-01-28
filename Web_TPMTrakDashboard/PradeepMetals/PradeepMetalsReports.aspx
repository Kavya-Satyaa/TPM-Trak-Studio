<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PradeepMetalsReports.aspx.cs" Inherits="Web_TPMTrakDashboard.PradeepMetals.PradeepMetalsReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
    <div class="row">
        <div class="col-md-9" style="margin: auto;">
            <h1 class="text-center login-title commontd">Reports</h1>
            <div class="account-wall">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-signin">
                        <asp:UpdatePanel ID="upadetPanalReport" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnGenerate" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblfilter" class="table table-bordered table-striped" style="width: 100%">
                                    <tr>
                                        <td>
                                            <b>Report Type</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                <asp:ListItem Text="Maintenance Downtime Pareto" Value="MaintenanceDowntimePareto"></asp:ListItem>
                                                <asp:ListItem Text="Pareto Overall Downtime Reasons Report" Value="ParetoOverallDowntimeReasonsReport" Enabled="false"></asp:ListItem>
                                                <asp:ListItem Text="OVERALL PML OEE TREND" Value="OVERALLPMLOEETREND"></asp:ListItem>
                                                <asp:ListItem Text="CNC Production Report" Value="CNCProductionReport"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trMaintenanceReporttype">
                                        <td>
                                            <b>Report Level</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlMaintenaceReport" CssClass="form-control cssclass" >
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="Maintenance Machine Level Downtime Pareto" Value="MaintenanceMachineLevelDowntimePareto"></asp:ListItem>
                                                <asp:ListItem Text="Maintenance-SubSystem Level Downtime Pareto" Value="MaintenanceSubSystemLevelDowntimePareto"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trDownCategory">
                                        <td>
                                            <b>Down Category</b>
                                        </td>
                                        <td>
                                           <%-- <asp:DropDownList runat="server" ID="ddlDownCategory" CssClass="form-control"></asp:DropDownList>--%>
                                            <asp:ListBox runat="server" SelectionMode="Multiple" CssClass="form-control cssclass" ID="ddlMultiDownCategory"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trFormat" runat="server" visible="false">
                                        <td>
                                            <b>Format</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlFormat" CssClass="form-control cssclass" AutoPostBack="true" OnSelectedIndexChanged="ddlFormat_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>From Date</b>
                                        </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>To Date</b>
                                        </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trShift" runat="server">
                                        <td>
                                            <b>Shift</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlShift">
                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="FIRST" Value="FIRST"></asp:ListItem>
                                                <asp:ListItem Text="SECOND" Value="SECOND"></asp:ListItem>
                                                <asp:ListItem Text="THIRD" Value="THIRD"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Plant ID</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlPlantID" CssClass="form-control cssclass" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trCellId">
                                        <td>
                                            <b>Cell ID</b>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList runat="server" ID="ddlCellID" AutoPostBack="true" CssClass="form-control cssclass" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged"></asp:DropDownList>--%>
                                            <asp:ListBox runat="server" SelectionMode="Multiple" CssClass="form-control cssclass" ID="ddlMultiCellID" OnSelectedIndexChanged="ddlMultiCellID_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static"  ></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Machine ID</b>
                                        </td>
                                        <td>
                                            <asp:ListBox runat="server" SelectionMode="Multiple" CssClass="form-control cssclass" ID="ddlMultiMachineId" ></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trDownID" runat="server" visible="false">
                                        <td>
                                            <b>Down ID</b>
                                        </td>
                                        <td>
                                            <asp:ListBox runat="server" SelectionMode="Multiple" CssClass="form-control cssclass" ID="ddlMultiDownID"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" OnClientClick="return ReportGenerateValidation();" />
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
    <script>
        $(document).ready(function () {
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiCellID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiDownCategory]').multiselect({
                includeSelectAllOption: true
            });

        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=ddlMultiMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiCellID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiDownCategory]').multiselect({
                includeSelectAllOption: true
            });
        });

        function ReportGenerateValidation() {
            if ($("[id$=txtFromDate]").val() == "") {
                toasterWarningMsg("Please enter fromDate.", "");
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                toasterWarningMsg("Please Enter toDate", "");
                return false;
            }
            return true;
        }
        function stayMultiselectedList(param) {
            debugger;
            //setControls();
            $('[id$=ddlMultiCellID]').multiselect({
                includeSelectAllOption: true
            });
            if (param == "cell") {
                $("#trCellId .btn-group").addClass('open');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
