<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModelMasterAdvik.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik184.ModelMasterAdvik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        #gvParameter tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #gvParameter tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }
    </style>
    <div class="container-fluid" style="">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <div style="height: 75vh; overflow: auto;" id="gridContainer">
                    <asp:GridView ID="gvParameter" CssClass="table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Model ID">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnIDD" ClientIDMode="Static" Value='<%# Eval("ID") %>' />
                                    <asp:Label ID="lblModel" runat="server" Text='<%# Eval("ModelID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtModel" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model Name">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtModelName" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("ModelName") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtModelName" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkDelete" ClientIDMode="Static" CssClass="glyphicon glyphicon-trash actionBtn" OnClick="lnkDelete_Click" ToolTip="Delete"></asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" ID="lnkInsert" ClientIDMode="Static" CssClass="glyphicon glyphicon-plus actionBtn" OnClick="lnkInsert_Click" ToolTip="Insert" OnClientClick="return saveValidation(this,'insert');"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="paginationCss" />
                    </asp:GridView>
                </div>
                <div class="modal fade" id="deleteConfirmModal" role="dialog">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content modalContent confirm-modal-content">
                            <div class="modal-header modalHeader confirm-modal-header">
                                <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                            </div>
                            <div>
                                <br />
                                <h4 class="confirm-modal-title">Confirmation!</h4>
                                <br />
                                <span class="confirm-modal-msg">Are you sure you want to delete Records?</span>
                            </div>
                            <div class="modal-footer modalFooter modal-footer">
                                <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" OnClientClick="return clearModalScreen();" />
                                <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                            </div>
                        </div>
                    </div>
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
    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        var bigDiv = document.getElementById('gridContainer');
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        $('#gvParameter tr').click(function () {
            $(this).find("#hdnUpdate").val("update");
        });
        function saveValidation(btn, param) {
            debugger;
            var rows;
            if (param == "insert") {
                rows = $(btn).closest('tr');
            }
            else {
                rows = $("#gvParameter tr:not(:first-child):not(:last-child)");
            }

            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                var paramID = "";
                if (param == "insert") {
                    paramID = $(row).find('#txtModel').val();
                }
                else {
                    if ($(row).find('#hdnUpdate').val() != "update") {
                        continue;
                    }
                    paramID = $(row).find('#lblModel').text();
                }

                var paramName = $(row).find('#txtParameterName').val();

                if (paramID == "") {
                    openWarningModal("Please enter Model ID.");
                    return false;
                }
                if (paramName == "") {
                    openWarningModal("Please enter Model Name.");
                    return false;
                }
            }
            return true;
        }

        function setControls() {
            $('[id$=lbComponent]').multiselect({
                includeSelectAllOption: true
            });
        }
        function clearModalScreen() {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            return true;
        }
        function openConfirmModal(id) {
            $("#" + id).modal('show');
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
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {

                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
            $('#gvParameter tr').click(function () {
                $(this).find("#hdnUpdate").val("update");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
