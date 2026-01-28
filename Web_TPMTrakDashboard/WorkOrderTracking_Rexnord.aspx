<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkOrderTracking_Rexnord.aspx.cs" Inherits="Web_TPMTrakDashboard.WorkOrderTracking_Rexnord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
        <tr>
            <td class="commanTd" style="vertical-align: middle;">From Date</td>
            <td>
                <div class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                </div>
            </td>
            <td class="commanTd" style="vertical-align: middle;">To Date</td>
            <td>
                <div class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                </div>
            </td>
            <td>
                <asp:LinkButton ClientIDMode="Static" runat="server" CssClass="glyphicon glyphicon-remove-sign" ToolTip="Clear Date" Font-Size="18" ID="lnkClearDate" OnClick="lnkClearDate_Click" ForeColor="White"></asp:LinkButton>
            </td>
            <td class="commanTd" style="vertical-align: middle;">Work Order</td>
            <td>
                <asp:TextBox ID="txtWorkOrderSearch" runat="server" CssClass="form-control" placeholder="Search.." ClientIDMode="Static"></asp:TextBox>
            </td>
            <td class="commanTd" style="vertical-align: middle;">Serial No.</td>
            <td>
                <asp:TextBox ID="txtSerialNoSearch" runat="server" CssClass="form-control" placeholder="Contain search.." ClientIDMode="Static"></asp:TextBox>
            </td>
            <td>
                <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" OnClientClick="return viewValidation();" />
                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="max-height: 80vh; overflow: auto; margin-top: 20px;" id="gridContainer">
                <asp:ListView runat="server" ClientIDMode="Static" ID="lvGridData" ItemPlaceholderID="itemplaceholder" OnItemDataBound="lvGridData_ItemDataBound">
                    <LayoutTemplate>
                        <table class="gridCs">
                            <tr>
                                <th>Work Order</th>
                                <th>Component ID</th>
                                <th>Serial Number</th>
                                <th>Operation No</th>
                                <th>Machine</th>
                                <th>Operator</th>
                                <th>Start Time</th>
                                <th>End Time</th>
                                <th>Cycle Time</th>
                                <th>Operation Status</th>
                              <%--  <th>Rejection Remarks</th>
                                <th>Rejection By</th>--%>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table class="table table-bordered headerFixer">
                            <tr>
                                <th>Work Order</th>
                                <th>Component ID</th>
                                <th>Serial Number</th>
                                <th>Operation No</th>
                                <th>Machine</th>
                                <th>Operator</th>
                                <th>Start Time</th>
                                <th>End Time</th>
                                <th>Cycle Time</th>
                                <th>Operation Status</th>
                              <%--  <th>Rejection Remarks</th>
                                <th>Rejection By</th>--%>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr runat="server" bgcolor='<%# Eval("trBackColor") %>'>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnOperationType" ClientIDMode="Static" Value='<%# Eval("OperationType") %>' />
                                <asp:Label runat="server" ID="lblWorkOrder" ClientIDMode="Static" Text='<%# Eval("WorkOrder") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblComponentID" ClientIDMode="Static" Text='<%# Eval("ComponentID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSerialNo" ClientIDMode="Static" Text='<%# Eval("SerialNo") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOperationNo" ClientIDMode="Static" Text='<%# Eval("OperationNo") %>'></asp:Label>
                            </td>
                            <td id="tdMachine" runat="server" style="text-align: center" colspan='<%# Eval("colspan") %>'>
                                <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("Machine") %>'></asp:Label>
                                <i style="color: green; font-weight: bold" class='<%# Eval("AdditionalIconClass") %>'></i>
                            </td>
                            <td id="tdOperator" runat="server" visible='<%# Eval("ColumnVisibility") %>' colspan='<%# Eval("colspan") %>'>
                                <asp:Label runat="server" ID="lblOperator" ClientIDMode="Static" Text='<%# Eval("Operator") %>'></asp:Label>
                            </td>
                            <td id="tdStartTime" style="width:155px;" runat="server" visible='<%# Eval("ColumnVisibility") %>' colspan='<%# Eval("colspan") %>'>
                                <asp:Label runat="server" ID="lblStartTime" ClientIDMode="Static" Text='<%# Eval("StartTime") %>'></asp:Label>
                            </td>
                            <td id="tdEndTime" style="width:155px;" runat="server" visible='<%# Eval("ColumnVisibility") %>' colspan='<%# Eval("colspan") %>'>
                                <asp:Label runat="server" ID="lblEndTime" ClientIDMode="Static" Text='<%# Eval("EndTime") %>'></asp:Label>
                            </td>
                            <td id="tdActualTime" runat="server" visible='<%# Eval("ColumnVisibility") %>' colspan='<%# Eval("colspan") %>'>
                                <asp:Label runat="server" ID="lblActualTime" ClientIDMode="Static" Text='<%# Eval("ActualTime") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblRejection" ClientIDMode="Static" Text='<%# Eval("Rejection") %>'></asp:Label>
                            </td>
                           <%-- <td>
                                <asp:Label runat="server" ID="lblRejectionRemarks" ClientIDMode="Static" Text='<%# Eval("RejectionRemarks") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblRejectionBy" ClientIDMode="Static" Text='<%# Eval("RejectionBy") %>'></asp:Label>
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            debugger;
            //$.unblockUI({});
            setControls();
            //$('[id$=btnView]').click(function () {
                
            //});
           
        });
        function viewValidation() {
            if ($('#txtWorkOrderSearch').val() == "" && $('#txtSerialNoSearch').val() == "") {
                /*toasterWarningMsg("Please enter WorkOrder or Serial No.");*/
                openWarningModal_1("Please enter WorkOrder or Serial No.");
                return false;
            }
            //$.unblockUI({});
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function setControls() {
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            bigDiv = document.getElementById('gridContainer');
            //$(document).ready(function () {
            $.unblockUI({});
            setControls();
            //$('[id$=btnView]').click(function () {
            //    $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            //});
            
            //});
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
