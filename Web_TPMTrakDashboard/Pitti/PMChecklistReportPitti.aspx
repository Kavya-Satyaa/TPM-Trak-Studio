<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMChecklistReportPitti.aspx.cs" Inherits="Web_TPMTrakDashboard.Pitti.PMChecklistReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        #gridContainer table tr td {
            background-color: #fcfcfc;
            border: 1px solid silver;
            padding: 2px 5px;
        }

        .td-actual-value {
        }

        .outer-table {
            height: 1px;
        }

            .outer-table > tbody > tr:first-child td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                text-align: center;
                border: 1px solid silver !important;
            }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }

        .txt-control {
            max-width: 150px;
            min-width: unset;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table class="table table-bordered">
                <tr>
                    <td class="commanTd" style="vertical-align: middle;">Machine ID</td>
                    <td>
                        <asp:ListBox runat="server" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control" ID="lbMachineID"></asp:ListBox>
                        <%--<asp:DropDownList runat="server" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control" ID="ddlMachineID"></asp:DropDownList>--%>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Year</td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Frequency</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control">
                            <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                            <asp:ListItem Text="Quarterly" Value="Quaterly"></asp:ListItem>
                            <asp:ListItem Text="Half Yearly" Value="Half Yearly"></asp:ListItem>
                            <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Category</td>
                    <td>
                        <asp:ListBox runat="server" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control" ID="lbCategory"></asp:ListBox>
                        <%--<asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control"></asp:DropDownList>--%>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="VIEW" CssClass="btn btn-info" OnClick="btnView_Click" OnClientClick="return showLoader();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" OnClick="btnExport_Click" Text="Export" CssClass="btn btn-info" />
                    </td>
                </tr>
            </table>
            <div id="gridContainer" style="width: 100%; height: 80vh; overflow: auto;visibility:hidden">
                <asp:ListView runat="server" ID="lvPMReport" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="outer-table" id="tblCheckpoint">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="Machine ID" Visible='<%# Eval("HeaderVisibility") %>' ClientIDMode="Static"></asp:Label>
                                <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>' ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Sl.No" Visible='<%# Eval("HeaderVisibility") %>' ClientIDMode="Static"></asp:Label>
                                <asp:Label runat="server" ID="lblSerialNo" Text='<%# Eval("SerialNo") %>' ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Items" Visible='<%# Eval("HeaderVisibility") %>' ClientIDMode="Static"></asp:Label>
                                <asp:Label runat="server" ID="Label1" Text='<%# Eval("CheckPointDescription") %>' ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Judgement Criteria(Standards)" Visible='<%# Eval("HeaderVisibility") %>' ClientIDMode="Static"></asp:Label>
                                <asp:Label runat="server" ID="Label2" Text='<%# Eval("JudgementalCriteria") %>' ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Resources Needed" Visible='<%# Eval("HeaderVisibility") %>' ClientIDMode="Static"></asp:Label>
                                <asp:Label runat="server" ID="Label3" Text='<%# Eval("ResourcesNeeded") %>' ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" Text="Duration(MIN)" Visible='<%# Eval("HeaderVisibility") %>' ClientIDMode="Static"></asp:Label>
                                <asp:Label runat="server" ID="Label4" Text='<%# Eval("Duration") %>' ClientIDMode="Static" Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                            </td>
                            <td style="padding: 0px; border: 0px;">
                                <asp:ListView runat="server" ID="lvPMChecksheet" ItemPlaceholderID="itemplaceholder2" DataSource='<%# Eval("Monthlist") %>'>
                                    <LayoutTemplate>
                                        <table style="width: 100%;" class="inner-table">
                                            <tr>
                                                <td runat="server" id="itemplaceholder2"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <td class="td-actual-value" style="min-width: 150px;">
                                            <asp:Label runat="server" Visible='<%# Eval("HeaderVisibility") %>' Text='<%# Eval("MonthName") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblMonth" Text='<%# Eval("MonthValue") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                        </td>
                                        <td class="td-actual-value" style="min-width: 70px;"> 
                                            <asp:Label runat="server" Visible='<%# Eval("HeaderVisibility") %>' Text="ATT"></asp:Label>
                                            <asp:Label runat="server" ID="lblATTValue" Text='<%# Eval("ATTValue") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControls();
            $('[id$=lbMachineID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbCategory]').multiselect({
                includeSelectAllOption: true
            });
            $.unblockUI({});
        });
        function setControls() {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControls();
                $('[id$=lbMachineID]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=lbCategory]').multiselect({
                    includeSelectAllOption: true
                });
                $.unblockUI({});
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
