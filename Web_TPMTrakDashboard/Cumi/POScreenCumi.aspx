<%@ Page Title="PO Screen" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POScreenCumi.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.POScreenCumi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td>From Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFromDate" ClientIDMode="Static" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>To Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtToDate" ClientIDMode="Static" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>Component ID</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtComponentID" CssClass="form-control" AutoCompleteType="Disabled" ></asp:TextBox>
                            </td>
                            <td>PO</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPO" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="VIEW" CssClass="bajaj-btn-style" OnClick="btnView_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 100%">
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px; width: 62%; display: inline-block">
                    <asp:ListView runat="server" ID="lvPOScreen" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style">
                                <tr>
                                    <th style="display:none;">Auto ID</th>
                                    <th>Item Code</th>
                                    <th>Production Order</th>
                                    <th>PO Qty</th>
                                    <th>Operation Stage</th>
                                    <th>Updated TS</th>
                                    <th>Mould Weight</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="display:none;"><%# Eval("AutoID") %></td>
                                <td><%# Eval("ItemCode") %></td>
                                <td><%# Eval("ProductionOrder") %></td>
                                <td><%# Eval("POQty") %></td>
                                <td><%# Eval("OperationStage") %></td>
                                <td><%# Eval("UpdatedTS") %></td>
                                <td><%# Eval("MouldWeight") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControl();
        });

        function setControl() {
            $("#txtFromDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                });
                $("#txtToDate").datepicker({
                    format: 'dd-mm-yyyy',
                    todayHighlight: true,
                    autoclose: true,
                    orientation: "top",
                    autocomplete: "off",
                    language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControl();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
