<%@ Page Title="PokayOke Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PokayOkeDashboardAdvik184.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.PokayOkeDashboardAdvik184" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .gridCss tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        .gridCss tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <table>
                <tr>
                    <td class="commanTd" style="vertical-align: middle;">Machine</td>
                    <td>
                        <asp:ListBox runat="server" ID="lbMachineID" ClientIDMode="Static" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">&nbsp;From DateTime</td>
                    <td class="input-group" style="min-width: 220px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">&nbsp;To DateTime</td>
                    <td class="input-group" style="min-width: 220px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control date1" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td>&nbsp;  &nbsp;
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" OnClientClick="return viewValidation();" />
                        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto;margin-top:10px">
                <asp:ListView runat="server" ID="lvData" ClientIDMode="Static" ItemPlaceholderID="itemPlaceholder">
                    <LayoutTemplate>
                        <table class="table table-bordered headerFixer gridCss">
                            <tr>
                                <th>Machine ID</th>
                                <th>Pokayoke ID</th>
                                <th>Pokayoke Name</th>
                               <%-- <th>Register ID</th>--%>
                                <th>Result</th>
                                <th>UpdatedTS</th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("MachineID") %></td>
                            <td><%# Eval("PokayokeID") %></td>
                            <td><%# Eval("PokayokeName") %></td>
                          <%--  <td><%# Eval("RegisterID") %></td>--%>
                            <td><%# Eval("Result") %></td>
                            <td><%# Eval("UpdatedTS") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="modal fade" id="warningModal" role="dialog" style="z-index: 2000">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content modalContent warning-modal-content">
                <div class="modal-header modalHeader warning-modal-header">
                    <i class="glyphicon glyphicon-warning-sign modal-icons"></i>
                </div>
                <div>
                    <br />
                    <h4 class="warning-modal-title">Warning!</h4>
                    <br />
                    <span class="warning-modal-msg" id="lblWarningMsg">...</span>
                </div>
                <div class="modal-footer modalFooter modal-footer">
                    <input type="button" value="OK" class="warning-modal-btn" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="errorModal" role="dialog" style="z-index: 2000">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content modalContent error-modal-content">
                <div class="modal-header modalHeader error-modal-header">
                    <i class="glyphicon glyphicon-remove-sign modal-icons"></i>
                </div>
                <br />
                <h4 class="error-modal-title">Error</h4>
                <br />
                <span class="error-modal-msg" id="lblErrorMsg">...</span>

                <div class="modal-footer modalFooter modal-footer">
                    <input type="button" value="OK" class="error-modal-btn" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $.unblockUI({});
            setControls();
        });
        function viewValidation() {
            if ($('#lbMachineID').val() == "" || $('#lbMachineID').val() == null) {
                openWarningModal("Please select Machine ID.");
                return false;
            }
            if ($('#txtFromDate').val() == "" || $('#txtFromDate').val() == null) {
                openWarningModal("Please select From Date.");
                return false;
            }
            if ($('#txtToDate').val() == "" || $('#txtToDate').val() == null) {
                openWarningModal("Please select To Date.");
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function setControls() {
            $('[id$=lbMachineID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        }
        function openWarningModal(msg) {
            $("#lblWarningMsg").text(msg);
            $("#warningModal").modal('show');
        }
        function openErrorModal(msg) {
            $('#errorModal').modal('show');
            $('#lblErrorMsg').text(msg);
        }
        function openSuccessModal(msg) {
            $('#toast-container').empty();
            Command: toastr["success"](msg)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                $.unblockUI({});
                setControls();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
