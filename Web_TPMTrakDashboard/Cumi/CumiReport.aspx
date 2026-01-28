<%@ Page Title="Cumi Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CumiReport.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.CumiReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
  
    <style>
        .multiselect-container{
            height: 35vh;
            overflow: auto;
         }
        .report-container {
            width: 30%;
            margin: 10px auto 0px auto;
            border: 1px solid gray;
            padding: 10px;
            border-radius: 10px;
            background-color: #11879d;
            box-shadow: 2px 2px 9px 1px #605d5d;
        }

        .report-table {
            width: 100%;
        }

            .report-table tr td {
                padding: 10px;
                color: white;
            }

        .btn-group, .multiselect-native-select .btn-group button {
            width: 100% !important;
        }

        .multiselect-native-select .btn-group button {
            text-align: left;
        }
    </style>
    <div class="content-div">

        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGenerate" />

            </Triggers>
            <ContentTemplate>
                <div class="report-container">
                    <table class="report-table">
                        <tr>
                            <td>Report Type
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlReportType" CssClass="form-control " AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server" id="trPlant">
                            <td>Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>

                            </td>
                        </tr>
                        <tr runat="server" id="trFromDate">
                            <td>From Date
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control" ClientIDMode="Static" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true"></asp:TextBox>

                            </td>
                        </tr>
                        <tr runat="server" id="trToDate">
                            <td>To Date
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" ClientIDMode="Static" OnTextChanged="txtToDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trDate">
                            <td>Date
                            </td>
                            <td style="position: relative">
                                <asp:TextBox runat="server" ID="txtDate" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trShift">
                            <td>Shift
                            </td>
                            <td>
                                <asp:ListBox ID="lbShift" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trMachine">
                            <td>Machine
                            </td>
                            <td>
                                <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trddMachine">
                            <td>Machine
                            </td>
                            <td>
                                 <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server" id="trEmployee">
                            <td>Employee
                            </td>
                            <td>
                                <asp:ListBox ID="lbEmployee" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trItemCode">
                            <td>Item Code
                            </td>
                            <td>
                                 <asp:ListBox ID="lbItemCode" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                            </td>
                        </tr>
                        <tr id="trYear" runat="server">
                            <td>Year</td>
                            <td>
                                <asp:TextBox ID="txtYearOnly" runat="server" CssClass="form-control" data-toggle="tooltip" title="Year !" placeholder="Year"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trFrequency" runat="server">
                            <td>Frequency</td>
                            <td>
                                <asp:ListBox runat="server" SelectionMode="Multiple" ID="lstFrequency" ClientIDMode="Static" CssClass="form-control">
                                </asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Button runat="server" ID="btnGenerate" Text="Generate" OnClick="btnGenerate_Click" CssClass="bajaj-btn-style" />
                            </td>
                        </tr>
                    </table>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        $(document).ready(function () {
            ControlSetter();

            $(document).on("click", "[id$=btnGenerate]", function () {
                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();
                var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
                if ($("[id$=ddlReportType]").val() == "CoolingTower" || $("[id$=ddlReportType]").val() == "EnergyReport") {
                    if (diffe > 30) {
                        alert("Difference between to date and from date cannot be more than " + 30 + " days.");
                        return false;
                    }
                }

            });

        });
        function ControlSetter() {
            $('[id$=lbMachine]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbShift]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbEmployee]').multiselect({
                includeSelectAllOption: true,
                //enableFiltering: true,
                maxHeight: 300,
                //buttonWidth: '250px',
                //enableCaseInsensitiveFiltering: true
            });
            $('[id$=lbItemCode').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lstFrequency]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=txtFromDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
                endDate: '1d'
            });
            $('[id$=txtToDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                endDate: '1d'
            });
            $('[id$=txtDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                endDate: '1d'
            });
            $('[id$=txtYearOnly]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
            });
        }
        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                ControlSetter();

            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
