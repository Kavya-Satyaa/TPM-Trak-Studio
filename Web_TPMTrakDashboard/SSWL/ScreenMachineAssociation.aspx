<%@ Page Title="Screen-Machine Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScreenMachineAssociation.aspx.cs" Inherits="Web_TPMTrakDashboard.SSWL.ScreenMachineAssociation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .headerFixer tbody tr {
            background-color: #FFFFFF;
            color: black;
        }

        .tblMachine tr td {
            text-align: center;
            border:1px solid #ddd;
        }

        #tblScreenAss > tbody > tr:first-child td {
            position: sticky;
            top: 0px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
            vertical-align:middle;
        }

    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <asp:HiddenField runat="server" ID="hdnCheckboxClick" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Screen</td>
                    <td>
                        <asp:ListBox ID="lbScreen" runat="server" SelectionMode="Multiple" Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine</td>
                    <td>
                        <asp:ListBox ID="lbMachine" runat="server" SelectionMode="Multiple" Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" OnClientClick="return viewValidation();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" Text="Select All" onclick="selectAllCheckBox(this);"  ForeColor="White" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto;">
                <asp:ListView runat="server" ID="lvScreenData" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table class="table table-bordered  headerFixer" id="tblScreenAss">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td >
                                <asp:Label runat="server" ID="lblScreenName" ClientIDMode="Static" Text='<%# Eval("ScreenName") %>'></asp:Label>
                            </td>
                            <td style="text-align:center">
                                 <asp:Label runat="server"  ClientIDMode="Static" Text="Select Row" visible='<%# Eval("IsHeader") %>'></asp:Label>
                                <asp:CheckBox runat="server" ID="chkRowSelect" onclick="selectAllRowCheckBox(this);" visible='<%# Eval("IsHeader").ToString()=="True"?false:true %>' />
                            </td>
                            <td style="padding: 0px">
                                <asp:ListView runat="server" ID="lvMachine" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder2" DataSource='<%# Eval("MachineList") %>'>
                                    <LayoutTemplate>
                                        <table style="width: 100%" class="tblMachine">
                                            <tr>
                                                <td runat="server" id="itemplaceholder2"></td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>

                                        <td runat="server" visible='<%# Eval("IsHeader") %>'>
                                            <asp:CheckBox runat="server" ID="chkColumnSelect" onclick="selectAllColumnCheckBox(this);" />
                                            <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" Text='<%# Eval("MachineName") %>'></asp:Label>
                                        </td>
                                        <td runat="server" visible='<%# Eval("IsHeader").ToString()=="True"?false:true %>' class="machineSelectTD">
                                            <asp:CheckBox runat="server" ID="chkSelect" Checked='<%# Eval("MachineSelect") %>' />
                                        </td>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
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
            setControls();
            setMachineTDWidth();
        });
        function viewValidation() {
            if ($('#lbScreen').val() == "" || $('#lbScreen').val() == null) {
                openWarningModal("Please select Screen.");
                return false;
            }
            if ($('#lbMachine').val() == "" || $('#lbMachine').val() == null) {
                openWarningModal("Please select Machine.");
                return false;
            }
            return true;
        }
        function selectAllCheckBox(chk) {
            $('.tblMachine #chkSelect').prop("checked", chk.checked);
        }
        function selectAllColumnCheckBox(chk) {
            var tdIndex = $(chk.closest('td')).index() + 1;
            $('.tblMachine  td:nth-child(' + tdIndex + ') #chkSelect').prop("checked", chk.checked);

        }
        function selectAllRowCheckBox(chk) {
            var tdIndex = $(chk.closest('tr')).index() + 1;
            $('#tblScreenAss > tbody > tr:nth-child(' + tdIndex + ') .tblMachine  #chkSelect').prop("checked", chk.checked);
        }
        function setMachineTDWidth() {
            var firstRow = $('#tblScreenAss tr')[0];
            var machineHeaderTD = $(firstRow).find('table.tblMachine td');
            for (var i = 0; i < machineHeaderTD.length; i++) {
                var headerTDWidth = $(machineHeaderTD[i]).width();
                $('.tblMachine  td:nth-child(' + (i + 1) + ')').css("width", headerTDWidth);
            }
        }
        function setControls() {
            $('[id$=lbScreen]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbMachine]').multiselect({
                includeSelectAllOption: true
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
                setControls();
                setMachineTDWidth();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
