<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WIPDashboard_TAFE.aspx.cs" Inherits="Web_TPMTrakDashboard.WIPDashboard_TAFE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <asp:UpdatePanel runat="server">
 <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
        <ContentTemplate>
            <div>
                <table class="tblSettings">
                    <tr>
                        <td style="color: white;font-weight:bold;">From Date</td>
                        <td style="position: relative;">
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </td>
                        <td style="color: white;font-weight:bold;">To Date</td>
                        <td style="position: relative;">
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </td>
                        <td style="color: white;font-weight:bold;">Type</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlType" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <td style="color: white;font-weight:bold;">Part No</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPartNo" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <td style="color: white;font-weight:bold;">WIP Process</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlWIPProcess" ClientIDMode="Static" CssClass="form-control">
                                <asp:ListItem Text="Waiting for VTL" Value="Waiting for VTL"></asp:ListItem>
                                <asp:ListItem Text="Waiting for MGTL" Value="Waiting for MGTL"></asp:ListItem>
                                <asp:ListItem Text="Waiting for Tester process" Value="Waiting for Tester process"></asp:ListItem>
                                <asp:ListItem Text="Waiting for PDI process" Value="Waiting for PDI process"></asp:ListItem>
                                <asp:ListItem Text="Completed PDI Process" Value="Completed PDI Process"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnView" CssClass="bajaj-btn-style btnclass" Text="VIEW" ClientIDMode="Static" OnClientClick="return ViewClick();" OnClick="btnView_Click" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnExport" CssClass="bajaj-btn-style btnclass" Text="Export" ClientIDMode="Static" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <div class="container-fluid" style="margin-top: 15px;" runat="server" id="divWaiting">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label runat="server" Font-Bold="true" ForeColor="White" Font-Size="Larger" ClientIDMode="Static" Text="COUNT BY MACHINE"></asp:Label>
                            <div style="height: 80vh; overflow: auto">
                                <asp:ListView runat="server" ID="lvCountDetails" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                    <LayoutTemplate>
                                        <table id="tblMachineDetails" class="gridCs alternate-table-style">
                                            <tr>
                                                <th>Machine ID</th>
                                                <th>Part ID</th>
                                                <th>Operation No</th>
                                                <th>Qty</th>
                                            </tr>
                                            <tr id="itemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <table class="gridCs">
                                            <tr>
                                                <th>Machine ID</th>
                                                <th>Part ID</th>
                                                <th>Operation No</th>
                                                <th>Qty</th>
                                            </tr>
                                            <tr>
                                                <td colspan="4"><span>No data Available</span></td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPartNo" ClientIDMode="Static" Text='<%# Eval("PartNo") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblOperationNo" ClientIDMode="Static" Text='<%# Eval("OperationNo") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblQty" ClientIDMode="Static" Text='<%# Eval("Quantity") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-8">
                            <asp:Label runat="server" Font-Bold="true" ForeColor="White" Font-Size="Larger" ClientIDMode="Static" Text="WIP Details"></asp:Label>
                            <div style="height: 80vh; overflow: auto">
                                <asp:ListView runat="server" ID="lvWIPDetails" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                    <LayoutTemplate>
                                        <table id="tblWIP" class="gridCs alternate-table-style">
                                            <tr>
                                                <th>Heat Code Number</th>
                                                <th>Machine ID</th>
                                                <th>Part ID</th>
                                                <th>Opn No</th>
                                                <th>Completed Process</th>
                                                <th>Next Process</th>
                                                <th>Remarks</th>
                                            </tr>
                                            <tr id="itemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <table class="gridCs">
                                            <tr>
                                                <th>Heat Code Number</th>
                                                <th>Machine ID</th>
                                                <th>Part ID</th>
                                                <th>Opn No</th>
                                                <th>Completed Process</th>
                                                <th>Next Process</th>
                                                <th>Remarks</th>
                                            </tr>
                                            <tr>
                                                <td colspan="7"><span>No data Available</span></td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblDerialNumber" ClientIDMode="Static" Text='<%# Eval("HeatCodeNumber") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPartFamily" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPartNo" ClientIDMode="Static" Text='<%# Eval("PartNo") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPartName" ClientIDMode="Static" Text='<%# Eval("OperationNo") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCompletedProcess" ClientIDMode="Static" Text='<%# Eval("CompletedProcess") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblNextProcess" ClientIDMode="Static" Text='<%# Eval("NextProcess") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblRemarks" ClientIDMode="Static" Text='<%# Eval("Remarks") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container-fluid" style="margin-top: 15px;" runat="server" id="divComplete">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label runat="server" Font-Bold="true" ForeColor="White" Font-Size="Larger" ClientIDMode="Static" Text="COMPLETE WIP DETAILS"></asp:Label>
                            <div style="height: 80vh; overflow: auto">
                                <asp:ListView runat="server" ID="lvCompleteDetails" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                    <LayoutTemplate>
                                        <table class="gridCs alternate-table-style">
                                            <tr>
                                                <th>Machine ID</th>
                                                <th>Part Number</th>
                                                <th>Ratio</th>
                                                <th>Version</th>
                                                <th>Completed Qty</th>
                                            </tr>
                                            <tr id="itemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <table class="gridCs">
                                            <tr>
                                                <th>Machine ID</th>
                                                <th>Part Number</th>
                                                <th>Ratio</th>
                                                <th>Version</th>
                                                <th>Completed Qty</th>
                                            </tr>
                                            <tr>
                                                <td colspan="5">No Data Found</td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("PartNumber") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("Ratio") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("version") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("CompletedQty") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>

                                </asp:ListView>
                            </div>
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-8">
                            <asp:Label runat="server" Font-Bold="true" ForeColor="White" Font-Size="Larger" ClientIDMode="Static" Text="MACHINE PROCESS DATA"></asp:Label>
                            <div runat="server" id="divmachine" style="height: 30vh; overflow: auto">
                                <asp:ListView runat="server" ID="lvmachine" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                    <LayoutTemplate>
                                        <table class="gridCs alternate-table-style">
                                            <tr>
                                                <th>Part Number</th>
                                                <th>Completed Quantity</th>
                                            </tr>
                                            <tr id="itemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <table class="gridCs">
                                            <tr>
                                                <th>Part Number</th>
                                                <th>Completed Quantity</th>
                                            </tr>
                                            <tr>
                                                <td colspan="2">No Data Found</td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("PartNumber") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("CompletedQty") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                            <asp:Label runat="server" Font-Bold="true" ForeColor="White" Font-Size="Larger" ClientIDMode="Static" Text="TESTER PROCESS DATA"></asp:Label>
                            <div runat="server" id="divtester" style="height: 25vh; overflow: auto">
                                <asp:ListView runat="server" ID="lvTester" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                    <LayoutTemplate>
                                        <table class="gridCs alternate-table-style">
                                            <tr>
                                                <th>Serial Number</th>
                                                <th>Heat Code</th>
                                                <th>Part Number</th>
                                                <th>Machine Type</th>
                                            </tr>
                                            <tr id="itemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <table class="gridCs">
                                            <tr>
                                                <th>Serial Number</th>
                                                <th>Heat Code</th>
                                                <th>Part Number</th>
                                                <th>Machine Type</th>
                                            </tr>
                                            <tr>
                                                <td colspan="4">No Data Found</td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("HeatCode") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("PartNumber") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("MachineType") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                            <asp:Label runat="server" Font-Bold="true" ForeColor="White" Font-Size="Larger" ClientIDMode="Static" Text="PDI PROCESS DATA"></asp:Label>
                            <div runat="server" id="divPDI" style="height: 25vh; overflow: auto">
                                <asp:ListView runat="server" ID="lvPDI" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                                    <LayoutTemplate>
                                        <table class="gridCs alternate-table-style">
                                            <tr>
                                                <th>Serial Number</th>
                                                <th>Heat Code Number</th>
                                                <th>Heat Code Type</th>
                                                <th>Part Number</th>
                                            </tr>
                                            <tr id="itemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <EmptyDataTemplate>
                                        <table class="gridCs">
                                            <tr>
                                                <th>Serial Number</th>
                                                <th>Heat Code Number</th>
                                                <th>Heat Code Type</th>
                                                <th>Part Number</th>
                                            </tr>
                                            <tr>
                                                <td colspan="4">No Data Found</td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("HeatCodeNumber") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("HeatCodeType") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("PartNumber") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $.unblockUI({});
        });
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                $('.Date').datepicker({
                    format: 'dd-mm-yyyy',
                    todayHighlight: true,
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
