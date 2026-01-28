<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GEA_MaterialTracking.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.GEA_MaterialTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <style>
        .dateControl {
            width: 120px;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl">
                        <tr>
                            <td>
                                <span>From Date</span>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFromDate" ClientIDMode="Static" CssClass="form-control dateControl" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <span>To Date</span>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtToDate" ClientIDMode="Static" CssClass="form-control dateControl" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <%-- <td>
                                <span>MachineID</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlMachineID" CssClass="form-control dateControl" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <span>ProductionOrder</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductionOrder" runat="server" AutoCompleteType="Disabled" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            --%>
                            <td>
                                <span>Status</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true">
                                    <%-- <asp:ListItem Text="All" Value=""></asp:ListItem>--%>
                                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                    <asp:ListItem Text="Not Completed" Value="Not Completed" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" CssClass="btn btn-info" Text="VIEW" />
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-info" Text="SAVE" />
                            </td>
                            <td>
                                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" CssClass="btn btn-info" Text="EXPORT" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 100%">
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px; min-width: 40%; display: inline-block">
                    <asp:ListView runat="server" ID="lvMaterial" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder" OnItemDataBound="lvMaterial_ItemDataBound">
                        <EmptyItemTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style">
                                <tr>
                                    <th>SNo</th>
                                    <th>Machine ID</th>
                                    <th>Date Of Schedule</th>
                                    <th>Production Order No</th>
                                    <th>Component ID</th>
                                    <th>Component Desc</th>
                                    <th>SeriesNo</th>
                                    <th>Cycle End Time</th>
                                    <th>TimeWaiting At Stores</th>
                                    <th>Receiver Name</th>
                                    <th>IsReceiptCompleted</th>
                                    <th>Date&Time Completion</th>
                                </tr>
                            </table>
                        </EmptyItemTemplate>
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style">
                                <tr>
                                    <th>SNo</th>
                                    <th>Machine ID</th>
                                    <th>Date Of Schedule</th>
                                    <th>Production Order No</th>
                                    <th>Component ID</th>
                                    <th>Component Desc</th>
                                    <th>SeriesNo</th>
                                    <th>Cycle End Time</th>
                                    <th>TimeWaiting At Stores</th>
                                    <th>Receiver Name</th>
                                    <th>IsReceiptCompleted</th>
                                    <th>Date&Time Completion</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblSNo" ClientIDMode="Static" Text='<%# Eval("SNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnCycleTime" ClientIDMode="Static" Value='<%# Eval("CycleTime") %>' />
                                    <asp:HiddenField runat="server" ID="hdnCompInterfaceID" ClientIDMode="Static" Value='<%# Eval("CompInterfaceID") %>' />
                                    <asp:Label runat="server" ID="lblDateOfSchedule" ClientIDMode="Static" Text='<%#Eval("DateOfSchedule") %>'></asp:Label>
                                </td>

                                <td>
                                    <asp:Label runat="server" ID="lblProductionOrderNo" ClientIDMode="Static" Text='<%# Eval("ProductionOrderNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblComponentID" ClientIDMode="Static" Text='<%# Eval("CompID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblCompDesc" ClientIDMode="Static" Text='<%# Eval("CompDesc") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblSeriesNo" Text='<%# Eval("seriesNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblCycleTime" Text='<%# Eval("CycleTime") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblTimeWaiting" Text='<%# Eval("TimeWaitingAtStores") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnWhoReceived" ClientIDMode="Static" Value='<%# Eval("WhoHasReceived") %>' />
                                    <asp:DropDownList runat="server" ID="ddlWhoReceived" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblWhoReceived" Text='<%# Eval("ReceiverName") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblReceiptCompletion" Text="Yes"></asp:Label>
                                    <asp:CheckBox runat="server" ClientIDMode="Static" ID="chkReceiptCompletion" AutoPostBack="true" OnCheckedChanged="chkReceiptCompletion_CheckedChanged" />

                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblDateTimeCompletion" Text='<%# Eval("DateTimeCompletion") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
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
