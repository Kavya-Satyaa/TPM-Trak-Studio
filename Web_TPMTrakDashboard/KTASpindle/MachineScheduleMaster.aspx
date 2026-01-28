<%@ Page Title="Schedule Comp-Opn" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineScheduleMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.MachineScheduleMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        /* .headerFixer tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        .headerFixer tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }*/
        .headerFixer tbody tr {
            background-color: #FFFFFF;
            color: black;
        }

            .headerFixer tbody tr:nth-last-child(2) {
                background-color: #DCDCDC;
                color: black;
            }

        .searchField {
            width: 100px;
            display: inline-block;
        }

        .actionBtn {
            font-size: 17px;
        }

        .paginationCss table {
            margin: auto;
        }

            .paginationCss table tr td {
                border: 0;
                padding: 0px;
            }

                .paginationCss table tr td a {
                    padding: 5px 10px;
                    border: 1px solid silver;
                }

                .paginationCss table tr td span {
                    padding: 5px 10px;
                    background: #ddd;
                    border: 1px solid silver;
                }
    </style>
    <div class="container-fluid" style="">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <asp:HiddenField runat="server" ID="hdnSelectedDate" ClientIDMode="Static" />
                <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                    <tr>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Plant</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Cell</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCell" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Status</td>
                        <td>
                            <asp:ListBox ID="lbStatus" runat="server" SelectionMode="Multiple" Width="150" ClientIDMode="Static">
                                <asp:ListItem Text="New" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Running" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Closed" Value="1"></asp:ListItem>
                            </asp:ListBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnUpdateStore" CssClass="btn btn-info" Text="Update Store" OnClick="btnUpdateStore_Click" />

                        </td>
                    </tr>
                    <tr>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">From Date</td>
                        <td class="input-group" style="min-width: 150px; border: 0">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1 dateField" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">To Date</td>
                        <td class="input-group" style="min-width: 150px; border: 0">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1 dateField" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Comp ID</td>
                        <td class="commanTd">
                            <asp:TextBox runat="server" ID="txtCompIDSearch1" CssClass="form-control searchField" placeholder="Start with.."></asp:TextBox>*
                            <asp:TextBox runat="server" ID="txtCompIDSearch2" CssClass="form-control searchField" placeholder="Contain.."></asp:TextBox>*
                        </td>
                        <td class="commanTd" style="width: 100px; vertical-align: middle;">Comp Desc</td>
                        <td class="commanTd">
                            <asp:TextBox runat="server" ID="txtCompDescSearch1" CssClass="form-control searchField" placeholder="Start with.."></asp:TextBox>*
                            <asp:TextBox runat="server" ID="txtCompDescSearch2" CssClass="form-control searchField" placeholder="Contain.."></asp:TextBox>*
                        </td>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                            <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
                <div style="height: 75vh; overflow: auto;" id="gridContainer">
                    <asp:GridView ID="gvScheduleCreate" CssClass="gvScheduleCreate table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static" OnRowDataBound="gvScheduleCreate_RowDataBound" ShowFooter="true" AllowPaging="true" OnPageIndexChanging="gvScheduleCreate_PageIndexChanging" OnPreRender="gvScheduleCreate_PreRender" PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnIDD" ClientIDMode="Static" Value='<%# Eval("IDD") %>' />
                                    <asp:HiddenField runat="server" ID="hdnScheduleDateTime" ClientIDMode="Static" Value='<%# Eval("ScheduleDateTime") %>' />
                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ScheduleDate") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="input-group" style="min-width: 150px; border: 0">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtScheduleDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Plant">
                                <ItemTemplate>
                                    <asp:Label ID="lblPlant" runat="server" Text='<%# Eval("Plant") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlPlantNew" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantNew_SelectedIndexChanged" onchange="storeDateValue();">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cell">
                                <ItemTemplate>
                                    <asp:Label ID="lblCell" runat="server" Text='<%# Eval("Cell") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlCellNew" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCellNew_SelectedIndexChanged" onchange="storeDateValue();">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine">
                                <ItemTemplate>
                                    <asp:Label ID="lblMachine" runat="server" Text='<%# Eval("Machine") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlMachineNew" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineNew_SelectedIndexChanged" onchange="storeDateValue();">
                                    </asp:DropDownList>
                                    <%--  <asp:TextBox runat="server" ID="txtMachineNew" ClientIDMode="Static" placeholder="Search Machine" AutoCompleteType="Disabled" list="dlMachineNew" CssClass="form-control panel-filter-controlstyle" Style="display: inline-block; width: 150px" AutoPostBack="true" OnTextChanged="txtMachineNew_TextChanged"></asp:TextBox>
                                    <datalist id="dlMachineNew" runat="server" clientidmode="static" autopostback="true"></datalist>--%>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Component">
                                <ItemTemplate>
                                    <asp:Label ID="lblComponent" runat="server" Text='<%# Eval("Component") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%--<asp:DropDownList runat="server" ID="ddlComponentNew" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlComponentNew_SelectedIndexChanged" onchange="storeDateValue();">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox runat="server" ID="txtComponentNew" ClientIDMode="Static" placeholder="Search Component" AutoCompleteType="Disabled" list="dlComponentNew" CssClass="form-control" Style="display: inline-block;" AutoPostBack="true" OnTextChanged="txtComponentNew_TextChanged"></asp:TextBox>
                                    <datalist id="dlComponentNew" runat="server" clientidmode="static" autopostback="true"></datalist>
                                    <asp:HiddenField runat="server" ID="hdnCompNew" ClientIDMode="Static" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                    <asp:Label ID="lblOperation" runat="server" Text='<%# Eval("Operation") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlOperationNew" ClientIDMode="Static" CssClass="form-control">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Component Desc">
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentDesc" runat="server" Text='<%# Eval("CompDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Priority No">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPriority" runat="server" Text='<%# Eval("PriorityNo") %>' ReadOnly='<%# Eval("PriorityReadOnly") %>' CssClass="form-control txtUpdate allowNumber"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="txtPriority" ClientIDMode="Static" CssClass="form-control txtPriority allowNumber"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkDelete" ClientIDMode="Static" CssClass="glyphicon glyphicon-trash actionBtn" OnClick="lnkDelete_Click" ToolTip="Delete"></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkClose" ClientIDMode="Static" CssClass="glyphicon glyphicon-remove actionBtn" OnClick="lnkClose_Click" ToolTip="Close"></asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" ID="lnkInsert" ClientIDMode="Static" CssClass="glyphicon glyphicon-plus actionBtn" OnClick="lnkInsert_Click" ToolTip="Insert" OnClientClick="return insertValidation(this);"></asp:LinkButton>
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
                <div class="modal fade" id="closeConfirmModal" role="dialog">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content modalContent confirm-modal-content">
                            <div class="modal-header modalHeader confirm-modal-header">
                                <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                            </div>
                            <div>
                                <br />
                                <h4 class="confirm-modal-title">Confirmation!</h4>
                                <br />
                                <span class="confirm-modal-msg">Are you sure you want to Close Records?</span>
                            </div>
                            <div class="modal-footer modalFooter modal-footer">
                                <asp:Button runat="server" Text="Yes" ID="btnCloseConfirm" CssClass="confirm-modal-btn" OnClick="btnCloseConfirm_Click" OnClientClick="return clearModalScreen();" />
                                <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
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
                <span class="error-modal-msg" style="color: black !important;" id="lblErrorMsg">...</span>

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
        var bigDiv = document.getElementById('gridContainer');
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        function insertValidation(ctrl) {
            var txtComp = $(ctrl).closest('tr').find('#txtComponentNew').val();
            if (txtComp == "") {
                openWarningModal("Please select Component.");
                return false;
            }
            var datalistOption = $(ctrl).closest('tr').find('#dlComponentNew option');
            var datalistHidden = $(ctrl).closest('tr').find('#dlComponentNew .compHdn');
            var flag = 0;
            debugger;
            for (var i = 0; i < datalistOption.length; i++) {
                if (txtComp == $(datalistOption[i]).val()) {
                    flag = 1;
                    $(ctrl).closest('tr').find('#hdnCompNew').val($(datalistHidden[i]).val());
                    break;
                }
            }
            if (flag == 1) {
                return true;
            }
            else {
                openWarningModal("Please select correct Component.");
                return false;
            }
        }

        $("#lnkInsert").on("click", function () {
            if ($(".txtPriority").val() == "") {
                openErrorModal("Priority should not be empty.");
                return false;
            }
            return true;
        })

        function storeDateValue() {
            $('#hdnSelectedDate').val($('#txtScheduleDate').val());
        }
        function setControls() {
            $('.dateField').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US',
            });
            $('#txtScheduleDate').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US',
                minDate: new Date(),
            });
            $('[id$=lbStatus]').multiselect({
                includeSelectAllOption: true
            });
        }
        function setAllCheckbox(ctrl) {
            debugger;
            if ($(ctrl).find('#chkSelectAll').is(":checked")) {
                $('#gvScheduleEnable #chkSelect').attr("checked", "checked");
            }
            else {
                $('#gvScheduleEnable #chkSelect').removeAttr("checked", "checked");
            }
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

        $(".gvScheduleCreate tr td .txtUpdate").focusin(function () {
            debugger;
            $(this).closest("tr").find("#hdnUpdate").val("update");
        });

        $('.allowNumber').keypress(function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                $('#txtScheduleDate').val($('#hdnSelectedDate').val());
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {

                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
            $(".gvScheduleCreate tr td .txtUpdate").focusin(function () {
                debugger;
                $(this).closest("tr").find("#hdnUpdate").val("update");
            });
            $("#lnkInsert").on("click", function () {
                if ($(".txtPriority").val() == "") {
                    openErrorModal("Priority should not be empty.");
                    return false;
                }
                return true;
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
