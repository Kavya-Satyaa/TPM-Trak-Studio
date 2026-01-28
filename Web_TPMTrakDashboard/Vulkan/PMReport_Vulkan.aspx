<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMReport_Vulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.Vulkan.PMReport_Vulkan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .outer-table {
            height: 1px;
        }

            .outer-table > tbody > tr:first-child td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                text-align: center;
                height: 5vh;
            }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }

        #gridContainer table tr td {
            background-color: #fcfcfc;
            border: 1px solid silver;
            padding: 2px 5px;
        }

        .td-actual-value {
        }

        .DayClass {
            background-color: transparent;
        }

        .HolidayClass {
            background-color: white;
        }

        .tblSettings {
            width: 95%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 10px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: large;
            }

        .green {
            text-align: center;
            font-size: 25px;
            color: green;
            font-weight: bold;
        }

        .red {
            text-align: center;
            font-size: 25px;
            color: red;
            font-weight: bold;
        }
        .inner-tbl tbody tr td{
            color: white;
            padding: 5px;
        }
    </style>
    <div>
        <table class="tblSettings">
            <tr>
                <td style="width: 200px;">Machine ID</td>
                <td>
                    <%-- <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlMachineID" CssClass="form-control" Width="100px"></asp:DropDownList>--%>
                    <asp:ListBox runat="server" SelectionMode="Multiple" ClientIDMode="Static" ID="lbMachineID" CssClass="form-control" Width="250px"></asp:ListBox>
                </td>
                <td>Frequency</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Width="100px">
                        <asp:ListItem Value="15Days" Text="15 Days"></asp:ListItem>
                        <asp:ListItem Value="Daily" Text="30 days"></asp:ListItem>
                        <asp:ListItem Value="Quarterly" Text="Quaterly"></asp:ListItem>
                        <asp:ListItem Value="Yearly" Text="Yearly"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Year</td>
                <td>
                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtYear" CssClass="form-control year"></asp:TextBox>
                </td>
                <td>Month</td>
                <td>
                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMonth" CssClass="form-control month"></asp:TextBox>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnView_Click" />
                </td>

                <td>
                    <table class="inner-tbl">
                        <tr>
                            <td>Report Type</td>
                            <td>
                                <asp:RadioButtonList ID="rdnreportType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="PDF" Value="PDF"></asp:ListItem>
                                    <asp:ListItem Text="Excel" Value="Excel" Selected="true"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style btnclass btn-success" ClientIDMode="Static" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="gridContainer" style="width: 100%; height: 80vh; overflow: auto; margin-top: 10px;">
        <asp:ListView runat="server" ID="lvPMReport" ClientIDMode="Static">
            <LayoutTemplate>
                <table class="outer-table headerFixer">
                    <tr runat="server" id="itemplaceholder"></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Sl. No." ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("SlNo") %>' ID="lblSlNo" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Machine ID" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("MachineID") %>' ID="lblMachine" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Checkpoint" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("CheckPoint") %>' ID="lblCheckPoint" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Requirement" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("Requirement") %>' ID="lblRequirement" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Method" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:ListView runat="server" ID="lvMethodmages" Visible='<%# Eval("ContentVisibility") %>' DataSource='<%# Eval("MethodList") %>'>
                            <LayoutTemplate>
                                <div runat="server" id="itemplaceholder"></div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <asp:Image runat="server" ID="imgMethod" ImageUrl='<%# Eval("imgUrl") %>' />
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Instrument" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:ListView runat="server" ID="lvInstrumentImages" Visible='<%# Eval("ContentVisibility") %>' DataSource='<%# Eval("InstrumentList") %>'>
                            <LayoutTemplate>
                                <div runat="server" id="itemplaceholder"></div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <asp:Image runat="server" ID="imgInstrument" ImageUrl='<%# Eval("imgUrl") %>' />
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Action" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("Action") %>' ID="lblAction" ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="padding: 0px; border: 0px;">
                        <asp:ListView runat="server" ClientIDMode="Static" ID="lvFrequency" ItemPlaceholderID="itemplaceholder2" DataSource='<%# Eval("FrequencyWiseValues") %>'>
                            <LayoutTemplate>
                                <table class="inner-table">
                                    <tr>
                                        <td runat="server" id="itemplaceholder2"></td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <td style="min-width: 150px; text-align: center;" class='<%# string.IsNullOrEmpty(Eval("BackgroundClass").ToString()) ? "DayClass" : "HolidayClass" %>' runat="server">
                                    <asp:Label runat="server" ID="lblDay" ClientIDMode="Static" Text='<%# Eval("Day") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblValue" ClientIDMode="Static" Text='<%# Eval("Value") %>' Visible='<%# Eval("ContentVisibility") %>' CssClass='<%# Eval("CssClass") %>'></asp:Label>
                                </td>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
    <script>
        $(document).ready(function () {
            $('[id$=lbMachineID]').multiselect({
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
            $('.month').datepicker({
                format: 'M',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $('[id$=lbMachineID]').multiselect({
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
            $('.month').datepicker({
                format: 'M',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
