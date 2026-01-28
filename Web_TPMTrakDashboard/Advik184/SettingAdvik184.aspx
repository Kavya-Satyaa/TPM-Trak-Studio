<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SettingAdvik184.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.SettingAdvik184" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
        <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <link href="../Scripts/ColorPickerJs/css/pick-a-color-1.2.2.min.css" rel="stylesheet" />
    <script src="../Scripts/ColorPickerJs/dependencies/jquery-1.9.1.min.js"></script>
    <script src="../Scripts/ColorPickerJs/dependencies/tinycolor-0.9.15.min.js"></script>
    <script src="../Scripts/ColorPickerJs/js/pick-a-color-1.2.2.min.js"></script>
    <style>
        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 3em 3em 3em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            width:max-content;
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
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <fieldset class="scheduler-border" style="margin:auto">
                <legend class="scheduler-border commontd">Setting</legend>
                <table>
                    <tr>
                        <td class="commanTd" style="vertical-align: middle;width:200px">Threshold Time (Sec)&nbsp;</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtThresholdTime" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="commanTd" style="vertical-align: middle;">Threshold Color&nbsp;</td>
                        <td style="padding:5px">
                            <asp:TextBox ID="txtThresholdColor" data-toggle="tooltip" name="border-color" runat="server" CssClass="pick-a-color form-control" ClientIDMode="Static"  Style="height: 40px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align:center;padding-top:20px">
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" OnClientClick="retun saveValidation();" />
                        </td>
                    </tr>
                </table>
            </fieldset>
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
            $(".pick-a-color").pickAColor();
        });
        function saveValidation() {
            if ($('#txtThresholdTime').val() == "") {
                openWarningModal("Please enter Threshold Time.");
                return false;
            }
            if ($('#txtThresholdColor').val() == "") {
                openWarningModal("Please enter Threshold Color.");
                return false;
            }
            return true;
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
            $(".pick-a-color").pickAColor();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
