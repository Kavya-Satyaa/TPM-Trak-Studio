<%@ Page Title="Final Inspection Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FinalInspectionDashboardAdvik.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.FinalInspectionDashboardAdvik" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .dot {
            height: 20px;
            width: 20px;
            background-color: gray;
            border-radius: 50%;
            display: inline-block;
            color: green
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
            margin-bottom: 0px;
        }

        .gridCss tbody tr {
            background-color: #DCDCDC;
            color: black;
        }

        #tblStationStatus tr td {
            text-align: center;
            min-width: 100px;
            background-color: white;
        }

        #tblParameter tr td {
            background-color: white;
            border: 1px solid silver;
        }

        #tblParameter input[type="checkbox"] {
            width: 18px;
            height: 18px;
        }
    </style>
    <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnScrollPos2" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnViewType" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnScannedSlno" ClientIDMode="Static" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table style="margin: auto">
                <tr>
                    <td class="commanTd" style="font-size: 20px">Scan QR Code&nbsp;&nbsp;</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSlNoSearch" CssClass="form-control" Width="200" AutoCompleteType="Disabled" placeholder="Serial Number..." onkeypress="return slnoControlKeyUp(event)" onkeydown="return slnoControlKeyDown(event)" ClientIDMode="Static"></asp:TextBox></td>
                    <td>&nbsp;<asp:Button runat="server" ID="btnViewSlno" Text="View" CssClass="btn btn-info" OnClick="btnViewSlno_Click" Visible="false" />
                    </td>
                    <td>&nbsp;
                        <asp:LinkButton runat="server" ID="lnkRefreshbutton" ClientIDMode="Static" OnClick="lnkRefreshbutton_Click" CssClass="glyphicon glyphicon-refresh" Style="font-size: 22px; font-weight: bold; color: white"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <div id="stationStatusDiv" runat="server">
                <fieldset class="scheduler-border" style="width: max-content; margin: auto">
                    <legend class="scheduler-border commontd">Station Status</legend>
                    <div id="statusContainer" style="text-align: center; margin-top: 20px">
                        <asp:ListView runat="server" ID="lvStatusData" ItemPlaceholderID="placeHolder">
                            <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="placeHolder" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class="myItem" style="margin: 4px; min-width: 100px; display: inline-block; vertical-align: top">
                                    <div>
                                        <div class="" style="padding: 6px; border-radius: 5px; border-top: 5px solid #f39c12; background-color: <%# Eval("BackColor") %>">
                                            <table style='width: 100%;'>
                                                <tr>
                                                    <td style="text-align: center; color: black; font-weight: bold; padding-bottom: 5px;">

                                                        <label style="font-size: 16px; color: <%# Eval("ForeColor") %>;"><%# Eval("MachineID") %></label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered " style='background-color: white; margin-bottom: 4px; text-align: left'>
                                                <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("StatusList") %>' ItemPlaceholderID="addressPlaceHolder">
                                                    <LayoutTemplate>
                                                        <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="min-width: 100px; height: 37px;">
                                                                <asp:Label runat="server" Text='<%# Eval("Label") + " : "%>' Style="font-weight: bold">:</asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </fieldset>
            </div>
            <div style="margin: auto; margin-top: 20px; width: 50%">

                <asp:ListView runat="server" ID="lvFinalData" ItemPlaceholderID="itemplaceholder">
                    <LayoutTemplate>
                        <table id="tblParameter" class="table table-bordered gridCss">
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblLabel" Text='<%# Eval("Label") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnType" ClientIDMode="Static" Value='<%# Eval("Type") %>' />
                                <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("Value") %>' Visible='<%# Eval("Type").ToString()=="PartNo"?true:false %>'></asp:Label>
                                <asp:Label runat="server" ID="lblSlno" Text='<%# Eval("Value") %>' Visible='<%# Eval("Type").ToString()=="Slno"?true:false %>'></asp:Label>

                                <asp:Label runat="server" Text='<%# Eval("Value") %>' Visible='<%# Eval("Type").ToString()=="Label"?true:false %>'></asp:Label>
                                <asp:CheckBox runat="server" ClientIDMode="Static" ID="chkSelect" Checked='<%# Eval("Value").ToString()=="1"?true:false %>' Visible='<%# Eval("Type").ToString()=="Chk"?true:false %>' Enabled='<%# Eval("ControlEnabled") %>' />
                                <asp:TextBox runat="server" ID="txtRemarks" Visible='<%# Eval("Type").ToString()=="Remarks"?true:false %>' Text='<%# Eval("Value") %>' CssClass="form-control" Enabled='<%# Eval("ControlEnabled") %>'></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnParameterName" ClientIDMode="Static" Value='<%# Eval("ParameterName") %>' />
                                <asp:HiddenField runat="server" ID="hdnLSL" ClientIDMode="Static" Value='<%# Eval("LSL") %>' />
                                <asp:HiddenField runat="server" ID="hdnUSL" ClientIDMode="Static" Value='<%# Eval("USL") %>' />
                                <asp:HiddenField runat="server" ID="hdnUnit" ClientIDMode="Static" Value='<%# Eval("Unit") %>' />
                                <asp:HiddenField runat="server" ID="hdnModel" ClientIDMode="Static" Value='<%# Eval("Model") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div id="FTbuttons" style="text-align: center">
                <asp:Button runat="server" ID="btnSave" Text="Save" Style="margin-top: 26px" CssClass="btn btn-info" OnClick="btnSave_Click" />

                <asp:Button runat="server" ID="btnApproval" ClientIDMode="Static" Text="Approve" Style="margin-top: 26px" CssClass="btn btn-info" OnClientClick="return openConfirmationModal('OK');" />
                <asp:Button runat="server" ID="btnRework" ClientIDMode="Static" Text="Rework" Style="margin-top: 26px" CssClass="btn btn-info" OnClientClick="return openConfirmationModal('Rework');" />
                <asp:Button runat="server" ID="btnReject" Text="Reject" Style="margin-top: 26px" CssClass="btn btn-info" OnClientClick="return openConfirmationModal('Reject');" />
            </div>
            <div class="modal fade" id="confirmModal" role="dialog">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content modalContent confirm-modal-content">
                        <div class="modal-header modalHeader confirm-modal-header">
                            <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                        </div>
                        <div>
                            <br />
                            <h4 class="confirm-modal-title">Confirmation!</h4>
                            <br />
                            <span class="confirm-modal-msg" id="confirmMsg">Are you sure you want to delete Records?</span>
                            <asp:HiddenField runat="server" ID="hdnConfirmType" ClientIDMode="Static" />
                        </div>
                        <div class="modal-footer modalFooter modal-footer">
                            <asp:Button runat="server" Text="Yes" ID="btnConfirm" CssClass="confirm-modal-btn" OnClientClick="return clearModalScreen();" OnClick="btnConfirm_Click" />
                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                        </div>
                    </div>
                </div>
            </div>
               <div class="modal fade" id="previousMachineConfirmModal" role="dialog">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content modalContent confirm-modal-content">
                        <div class="modal-header modalHeader confirm-modal-header">
                            <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                        </div>
                        <div>
                            <br />
                            <h4 class="confirm-modal-title">Confirmation!</h4>
                            <br />
                            <span class="confirm-modal-msg" id="confirmMsg2" runat="server">Previous machine operation not completed. Do you want to continue?</span>
                            <asp:HiddenField runat="server" ID="HiddenField1" ClientIDMode="Static" />
                        </div>
                        <div class="modal-footer modalFooter modal-footer">
                            <asp:Button runat="server" Text="Yes" ID="btnFIContinueConfirm" CssClass="confirm-modal-btn" OnClientClick="return clearModalScreen();" OnClick="btnFIContinueConfirm_Click" />
                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnViewSlno" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkRefreshbutton" EventName="Click" />
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
            setControls();
        });
        function openConfirmationModal(param) {
            var msg = "";
            $('#hdnConfirmType').val(param);

            if (param == "OK") {
                msg = "Are you sure you want to approve Records?";
            }
            else if (param == "Reject") {
                msg = "Are you sure you want to reject Records?";
            }
            else if (param == "Rework") {
                msg = "Are you sure you want to rework?";
            }
            $('#confirmMsg').text(msg);
            $('#confirmModal').modal('show');
            return false;
        }
        var firstSNCharCount = 0;
        var firstSNCharPosition = "";
        var completeSNScan = true;
        var inputval;
        var temptime;
        var enterCondition = 0;
        var completePNScan = true;
        var firstPNCharCount = 0;
        var firstPNCharPosition = "";
        var slnoinputvalTimeout;
        function slnoControlKeyUp(e) {
            if (e.keyCode == 13) {
                completeSNScan = true;
                firstSNCharCount = 0;
                firstSNCharPosition = "";
                if (enterCondition == 1) {
                    debugger;

                    enterCondition = 0;
                    temptime = "";
                    clearTimeout(slnoinputvalTimeout);
                    $('#txtSlNoSearch').blur();
                    e.preventDefault();
                    if ($('#txtSlNoSearch').val() == "") {
                        openWarningModal("Please enter QR Code.");
                        return false;
                    }
                    __doPostBack('<%= btnViewSlno.UniqueID%>', '');
                }
                else {
                    e.preventDefault();
                    console.log("Enter 13");
                    return false;
                }
            }
            return true;
        }

        function slnoControlKeyDown(e) {
            var timeout = 3000;
            clearTimeout(slnoinputvalTimeout);
            var timeDiff;
            var today = new Date();
            var time = today.getMilliseconds();
            if (temptime != undefined || temptime != "") {
                timeDiff = parseInt(temptime) - time;
                if (timeDiff < 0) {
                    timeDiff = timeDiff * -1;
                }
                if (timeDiff <= 10) {
                    timeout = 1000;
                }
                else {
                    timeout = 3000;
                    enterCondition = 1;
                }
            }
            if (timeout == 1000 && completeSNScan == true) { //scan slno number again and again. It will remove old data and take new data
                var txtSNValue = $('#txtSlNoSearch').val();
                console.log("TXT =" + txtSNValue);
                var newChar = txtSNValue.substring(txtSNValue.length - 1, txtSNValue.length);
                console.log("New Char" + newChar);
                $('#txtSlNoSearch').val("");
                $('#txtSlNoSearch').val(newChar);
                completeSNScan = false;
            }

            temptime = time;
            slnoinputvalTimeout = setTimeout(function () {
                completeSNScan = true;
                firstSNCharCount = 0;
                firstSNCharPosition = "";

                $('#txtSlNoSearch').blur();
                temptime = "";
                enterCondition = 0;
                if ($('#txtSlNoSearch').val() == "") {
                    openWarningModal("Please enter QR Code.");
                    return false;
                }
                __doPostBack('<%= btnViewSlno.UniqueID%>', '');
            }, timeout);
        }
        function clearModalScreen() {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            return true;
        }
        function setControls() {
            $("#txtFromDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: 'en-US'
            });
            $("#txtToDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: 'en-US'
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        function openModal(modalID) {
            $("#" + modalID).modal('show');
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
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
