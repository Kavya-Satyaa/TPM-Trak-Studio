<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports_Pitti.aspx.cs" Inherits="Web_TPMTrakDashboard.Pitti.Reports_Pitti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
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
            min-width: 400px;
        }

        .cssclass2 {
            min-width: 200px;
        }
    </style>
    <div class="row">
        <div class="col-md-9" style="margin-left: 10%; margin-top: 5%;">
            <h1 class="text-center login-title commontd">Pitti Reports</h1>
            <div class="account-wall">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-signin">
                        <asp:UpdatePanel ID="upadetPanalRepor" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnGenerate" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblfilter" class="table table-bordered table-striped">
                                    <tr>
                                        <td>Report Type</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlReportType" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="PMChecklistReport" Text="PM Checklist Report - Pitti"></asp:ListItem>
                                                <asp:ListItem Value="DailyChecklistReport" Text="Daily Checklist Report - Pitti"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Machine ID</td>
                                        <td>
                                            <asp:ListBox runat="server" SelectionMode="Multiple" ClientIDMode="Static" ID="lbMachineID" CssClass="form-control" Width="100px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Year</td>
                                        <td>
                                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtYear" CssClass="form-control year"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trMonth" runat="server">
                                        <td>Month</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtMonth" CssClass="form-control Month"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Frequency</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control">
                                                <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                                <asp:ListItem Text="Quarterly" Value="Quaterly"></asp:ListItem>
                                                <asp:ListItem Text="Half Yearly" Value="Half Yearly"></asp:ListItem>
                                                <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trCategory" runat="server">
                                        <td>Category</td>
                                        <td>
                                            <asp:ListBox runat="server" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control" ID="lbCategory"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
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
            /*setControls();*/
            $('[id$=lbMachineID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbCategory]').multiselect({
                includeSelectAllOption: true
            });
            $('.year').datepicker({
                format: 'yyyy',
                viewMode: "years",
                minViewMode: "years",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('.Month').datepicker({
                format: 'mm',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $.unblockUI({});
        });
        function setControls() {
            $('.year').datepicker({
                format: 'yyyy',
                viewMode: "years",
                minViewMode: "years",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $(".Month").datepicker({
                format: 'mm',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            })
        }
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                /*setControls();*/
                $('[id$=lbMachineID]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=lbCategory]').multiselect({
                    includeSelectAllOption: true
                });
                $('.year').datepicker({
                    format: 'yyyy',
                    viewMode: "years",
                    minViewMode: "years",
                    autoclose: true,
                    orientation: "top",
                    autocomplete: "off",
                    language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                });
                $('.Month').datepicker({
                    format: 'mm',
                    viewMode: "months",
                    minViewMode: "months",
                    autoclose: true,
                    orientation: "top",
                    autocomplete: "off",
                    language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                });
                $.unblockUI({});
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
