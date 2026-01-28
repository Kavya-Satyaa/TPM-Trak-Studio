<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyCheckListMasterVulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.Vulkan.DailyCheckListMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblSettings {
            width: 100%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
            /*background-color: #676767;*/
            /*width: 80%;
    box-shadow: 0px 0px 6px transparent;
    border-radius: 10px;
    background-color: #463c4e;
    border: 1px solid #00b5ff;*/
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 10px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: large;
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .innertbl > tbody > tr > td {
            padding: 0px 10px;
        }

        .lvDailyCLGrid {
            width: 100%;
        }

            .lvDailyCLGrid > tbody > tr > th {
                text-align: center;
                border: 1px solid white;
                color: white;
                height: 38px;
            }

            .lvDailyCLGrid > tbody > tr > td {
                border: 1px solid #ddd;
                padding: 10px;
                color: black;
            }

            .lvDailyCLGrid > tbody > tr:nth-child(even) > td {
                background-color: white;
            }

            .lvDailyCLGrid > tbody > tr:nth-child(odd) > td {
                background-color: #ddd;
            }


        .multiselect-container {
            max-height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .cssclass {
            min-width: 350px;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table class="tblSettings">
                <tr>
                    <td>Plant ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlantID" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>Machine ID</td>
                    <td>
                        <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlMachineID" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>Rev. No.</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRevNo" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtRevNo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td style="text-align: right;">
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style btnclass" OnClick="btnView_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnCopy" Text="Copy" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" OnClientClick="return openCopyModal();" />
                        <asp:Button runat="server" ID="btnCreateRevisionNo" Visible="true" Text="New Rev. No." CssClass="btn btn-primary btnclass" ClientIDMode="Static" OnClientClick="return openRevNoModal();" />
                    </td>
                </tr>
                <tr>
                    <td>Doc No.</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDocNo" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>Issue Date</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtIssueDate" CssClass="form-control Date"></asp:TextBox>
                    </td>
                    <td>Rev Date</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRevDate" CssClass="form-control Date"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:HiddenField runat="server" ID="hdnNew" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="btnNew" ClientIDMode="Static" Text="New" OnClientClick="return onNewClick();" CssClass="bajaj-btn-style btnclass" />
                        <%--<asp:Button runat="server" ID="btnEdit" ClientIDMode="Static" Text="Edit" OnClientClick="return onEditClick();" CssClass="bajaj-btn-style btnclass" />--%>
                        <asp:Button runat="server" ID="btnCancel" ClientIDMode="Static" Text="Cancel" OnClientClick="return onCancelClick();" CssClass="btn btn-primary btnclass" />
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="bajaj-btn-style btnclass" OnClick="btnSave_Click" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="bajaj-btn-style btnclass" OnClick="btnDelete_Click" ClientIDMode="Static" />
                    </td>
                </tr>
            </table>
            <div style="height: 75vh; overflow: auto; margin-top: 15px;" id="gridContainer">
                <asp:ListView runat="server" ID="lvDailyCLGrid" ClientIDMode="Static" InsertItemPosition="FirstItem" OnItemDataBound="lvDailyCLGrid_ItemDataBound">
                    <LayoutTemplate>
                        <table class="lvDailyCLGrid headerFixer" id="lvDailyCLGrid">
                            <tr>
                                <th style="width: 5%">SR. No.</th>
                                <th>Check Point</th>
                                <th>Requirement</th>
                                <th>Method</th>
                                <th>Instrument</th>
                                <th>Action Plan</th>
                                <th style="width: 8%;">AM Type</th>
                                <th>Delete?</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <InsertItemTemplate>
                        <tr runat="server" id="trInsertItemTemplate" class="active" clientidmode="static">
                            <td>
                                <asp:TextBox runat="server" ID="txtSlNo" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCheckPoint" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRequirement" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ListBox runat="server" ID="lbMethod" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control lbMethod"></asp:ListBox>
                            </td>
                            <td>
                                <asp:ListBox runat="server" ID="lbInstrument" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control lbMethod"></asp:ListBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActionPlan" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlChecklistType" CssClass="form-control">
                                    <asp:ListItem Text="Morning" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Shift" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:Label runat="server" ID="lblSlNo" Text='<%# Eval("SlNo") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="lblCheckPoint" Text='<%# Eval("CheckPoint") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRequirement" Text='<%# Eval("Requirement") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnMethod" Value='<%# Eval("Method") %>' />
                                <asp:ListBox runat="server" ID="lbMethod" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control lbMethod txtUpdate"></asp:ListBox>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnInstrment" Value='<%# Eval("Instrument") %>' />
                                <asp:ListBox runat="server" ID="lbInstrument" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control lbMethod txtUpdate"></asp:ListBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActionPlan" Text='<%# Eval("ActionPlan") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnChecklistType" Value='<%# Eval("ChecklistType") %>' />
                                <asp:DropDownList runat="server" ID="ddlChecklistType" CssClass="form-control txtUpdate">
                                    <asp:ListItem Text="Morning" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Shift" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="text-center">
                                <asp:CheckBox runat="server" ID="chkDelete" ClientIDMode="Static" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div class="modal infoModal" id="NewRevNoModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered " style="width: 650px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="newEditModalTitle" runat="server">Create Revision No.</h4>
                        </div>
                        <div class="modal-body" style="padding: 1px 15px !important">
                            <table class="innertbl" style="display: flex; justify-content: center; margin: 20px 0px;">
                                <tr>
                                    <td>Revision No.</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtRevNoNew" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="modal-footer" style="border: 0px !important; border-top: 1px solid black !important;">
                                <asp:Button runat="server" ID="btnSaveRevisionNo" OnClick="btnSaveRevisionNo_Click" Text="ADD" ClientIDMode="Static" CssClass="btn btn-primary" />
                                <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseNewEditModal();">CANCEL</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal infoModal" id="CLCopyModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered " style="width: 750px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="headerCopyModal" runat="server">Checklist Copy</h4>
                        </div>
                        <div class="modal-body" style="padding: 1px 15px !important">
                            <table class="innertbl" style="display: flex; justify-content: center; margin: 20px 0px;">
                                <tr>
                                    <td>Source MachineID</td>
                                    <td>
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtSourceMachineID" Enabled="false" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>Dest. Machines</td>
                                    <td>
                                        <asp:ListBox runat="server" ID="lbMultiMachineID" ClientIDMode="Static" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="modal-footer" style="border: 0px !important; border-top: 1px solid black !important;">
                                <asp:Button runat="server" ID="btnCopyModal" OnClick="btnCopyModal_Click" Text="COPY" ClientIDMode="Static" CssClass="btn btn-primary" />
                                <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseCopyModal();">CANCEL</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlPlantID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlMachineID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>




    <script>
        $(document).ready(function () {
            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            $(".lbMethod").multiselect({
                includeSelectAllOption: true
            });
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('#btnCancel').hide();
            //$('.lvDailyCLGrid tr td .txtUpdate').attr("disabled", true);
            //$('.lvDailyCLGrid tr td .multiselect').attr("disabled", true);
            showHideFirstRow();
        });

        function showHideFirstRow() {
            var rows = $(".lvDailyCLGrid tr");
            var firstRow = $(".lvDailyCLGrid").find("#trInsertItemTemplate");
            if (rows.length > 2) {
                if ($(firstRow).hasClass("active")) {
                    $(firstRow).removeClass("active");
                    $(firstRow).hide();
                }
                else {
                    $(firstRow).addClass("active");
                    $(firstRow).show();
                }
            }
        }

        $('.lvDailyCLGrid tr td .txtUpdate').focus(function () {
            var row = $(this).closest("tr");
            row.find('#hdnUpdate').val("update");
        });
        $('.lvDailyCLGrid tr td .txtUpdate').change(function () {
            debugger;
            var row = $(this).closest("tr");
            row.find('#hdnUpdate').val("update");
        });

        function onNewClick() {
            $("#hdnNew").val("New");
            $('#btnNew').hide();
            $('#btnEdit').hide();
            $('#btnCancel').show();
            showHideFirstRow();
            return false;
        }
        function onEditClick() {
            $('#btnEdit').hide();
            $('#btnNew').hide();
            $('#btnCancel').show();
            $('.lvDailyCLGrid tr td .txtUpdate').attr("disabled", false);
            $('.lvDailyCLGrid tr td .multiselect').attr("disabled", false);
            return false;
        }
        function onCancelClick() {
            if ($("#hdnNew").val() == "New")
                showHideFirstRow();
            $("#hdnNew").val("");
            $('#btnCancel').hide();
            $('#btnNew').show();
            $('#btnEdit').show();
            //$('.lvDailyCLGrid tr td .txtUpdate').attr("disabled", true);
            //$('.lvDailyCLGrid tr td .multiselect').attr("disabled", true);
            return false;
        }
        $("#btnSave").click(function () {
            if ($("#hdnNew").val() == "New") {
                if ($(".lvDailyCLGrid tr #txtSlNo").val() == "") {
                    toasterWarningMsg('Checkpoint ID Can not be Empty');
                    return false;
                }
            }
            return true;
        });
        $("#btnDelete").click(function () {
            var len = $('.lvDailyCLGrid tr td #chkDelete');
            var rowstodelete = 0;
            for (var i = 0; i < len.length; i++) {
                if ($(len[i]).is(":checked"))
                    rowstodelete += 1;
            }
            if (rowstodelete <= 0) {
                toasterWarningMsg('Select Rows To Delete');
                return false;
            }
            return true;
        })

        function openRevNoModal() {
            $("#txtRevNoNew").val("");
            $("#NewRevNoModal").modal('show');
            return false;
        }
        function CloseNewEditModal() {
            $("#NewRevNoModal").modal('hide');
        }
        function openCopyModal() {
            $("#CLCopyModal").modal('show');
            return false;
        }

        function CloseCopyModal() {
            $("#CLCopyModal").modal('hide');
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            $(".lbMethod").multiselect({
                includeSelectAllOption: true
            });

            $("#btnSave").click(function () {
                if ($("#hdnNew").val() == "New") {
                    if ($(".lvDailyCLGrid tr #txtSlNo").val() == "") {
                        toasterWarningMsg('Checkpoint ID Can not be Empty');
                        return false;
                    }
                }
                return true;
            });

            $("#btnDelete").click(function () {
                var len = $('.lvDailyCLGrid tr td #chkDelete');
                var rowstodelete = 0;
                for (var i = 0; i < len.length; i++) {
                    if ($(len[i]).is(":checked"))
                        rowstodelete += 1;
                }
                if (rowstodelete <= 0) {
                    toasterWarningMsg('Select Rows To Delete');
                    return false;
                }
                return true;
            })

            $('.lvDailyCLGrid tr td .txtUpdate').focus(function () {
                var row = $(this).closest("tr");
                row.find('#hdnUpdate').val("update");
            });

            $('.lvDailyCLGrid tr td .txtUpdate').change(function () {
                debugger;
                var row = $(this).closest("tr");
                row.find('#hdnUpdate').val("update");
            });

            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('#btnCancel').hide();
            //$('.lvDailyCLGrid tr td .txtUpdate').attr("disabled", true);
            //$('.lvDailyCLGrid tr td .multiselect').attr("disabled", true);
            showHideFirstRow();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
