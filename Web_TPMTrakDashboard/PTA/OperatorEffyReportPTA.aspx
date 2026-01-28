<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperatorEffyReportPTA.aspx.cs" Inherits="Web_TPMTrakDashboard.PTA.OperatorEffyReportPTA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table id="tblFilter" class="table table-bordered" style="width: auto;">
                <tr>

                    <td class="commanTd" style="vertical-align: middle;">Plant</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Month-Year</td>
                    <td>

                        <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control date date-control" placeholder="MM" AutoCompleteType="Disabled" ClientIDMode="Static" Style="display: inline-block; width: 100px;"></asp:TextBox>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control date date-control" placeholder="YYYY" AutoCompleteType="Disabled" ClientIDMode="Static" Style="display: inline-block; width: 100px;"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Shift</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Operator</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlOperator" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return callLoader();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto;">
                <asp:ListView runat="server" ID="lvOprData" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="table table-bordered headerFixer alternate-table-style">
                            <tr>
                                <th>Date</th>
                                <th>Shift</th>
                                <th>Machine</th>
                                <th>Production Time (min)</th>
                                <th>Downtime (min)</th>
                                <th>Others (min)</th>
                                <th>AE (%)</th>
                                <th>PE (%)</th>
                                <th>OE (%)</th>
                                <th>Net useful minutes</th>
                                <th>Total minutes</th>
                                <th>Blended OEE</th>
                                <th>Net benefit normalized to better machine</th>
                                <th>Net Loss from 80% benchmark (min)</th>
                                <th>Net Loss (%) because of 1 operator less</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("DateInString") %>
                            </td>
                            <td>
                                <%# Eval("Shift") %>
                            </td>
                            <td>
                                <%# Eval("Machine") %>
                            </td>
                            <td>
                                <%# Eval("ProdTime") %>
                            </td>
                            <td>
                                <%# Eval("DwnTime") %>
                            </td>
                            <td>
                                <%# Eval("Others") %>
                            </td>
                            <td>
                                <%# Eval("AE") %>
                            </td>
                            <td>
                                <%# Eval("PE") %>
                            </td>
                            <td>
                                <%# Eval("OEE") %>
                            </td>
                            <td>
                                <%# Eval("NUseMin") %>
                            </td>
                            <td>
                                <%# Eval("Totmin") %>
                            </td>
                            <td>
                                <%# Eval("BOEE") %>
                            </td>
                            <td>
                                <%# Eval("NbtrMachine") %>
                            </td>
                            <td>
                                <%# Eval("LosBMark") %>
                            </td>
                            <td>
                                <%# Eval("LosOpr") %>
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
            setDateTimePicker();
            callUnLoader();
        });
        function setDateTimePicker() {
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

        }
        function callLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function callUnLoader() {
            $.unblockUI({});
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                setDateTimePicker();
                callUnLoader();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
