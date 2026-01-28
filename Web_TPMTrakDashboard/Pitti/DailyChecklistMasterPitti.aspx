<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyChecklistMasterPitti.aspx.cs" Inherits="Web_TPMTrakDashboard.Pitti.DailyChecklistMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .tblfilter {
            width: 95%;
            box-shadow: 0px 0px 2px black;
        }
            /*.tblfilter > tbody > tr > td{
            color: black;
            text-align: center;
            vertical-align: middle;
            font-size: 17px;
            font-weight: bold;
            border: 1px solid #202648 !important;
            background-color: #b3e4eb;
        }*/
            .tblfilter > tbody > tr > td {
                color: white;
                text-align: center;
                vertical-align: middle;
                font-size: 15px;
                border: 1px solid #4e5166 !important;
            }

        .gvChecklist {
            width: 100%;
        }

            .gvChecklist > tbody > tr > th {
                background-color: #2e6886;
                color: white;
                height: 40px;
                text-align: center;
                vertical-align: middle;
                font-size: 15px;
                font-weight: bold;
                position: sticky;
                top: -1px;
                border: 1px solid #ddd;
            }

            .gvChecklist > tbody > tr > td {
                color: black;
                height: 40px;
                border: 1px solid #ddd;
                padding: 6px;
            }

                .gvChecklist > tbody > tr > td:last-child {
                    width: 8%;
                }

            .gvChecklist > tbody > tr:nth-child(even) {
                background-color: white;
            }

            .gvChecklist > tbody > tr:nth-child(odd) {
                background-color: #d7d7d7;
            }

        .show-row {
            display: revert;
        }

        .hide-row {
            display: none;
        }
    </style>
    <table class="table table-bordered tblfilter">
        <tr>
            <td>Machine ID</td>
            <td style="width: 14%">
                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td>Reference No.</td>
            <td style="width: 10%">
                <asp:HiddenField runat="server" ID="hdnHeaderValues" ClientIDMode="Static" />
                <asp:TextBox runat="server" ID="txtRefNo" CssClass="form-control HeaderUpdate"></asp:TextBox>
            </td>
            <td>Revision No.</td>
            <td style="width: 8%">
                <asp:TextBox runat="server" ID="txtRevNo" CssClass="form-control HeaderUpdate"></asp:TextBox>
            </td>
            <td>Supervisor</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSupervisor" CssClass="form-control HeaderUpdate"></asp:DropDownList>
            </td>
            <td>
                <asp:Button runat="server" ID="BtnView" Text="View" CssClass="btn btn-primary" OnClick="BtnView_Click" />
            </td>
            <td style="border: 1px solid #202648 !important;">
                <asp:HiddenField runat="server" ID="hdnValueNewEdit" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-primary" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" ClientIDMode="Static" OnClientClick="return CheckpointIDValidation();" OnClick="btnSave_Click" />
                <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary" ClientIDMode="Static"  OnClientClick="$('#deleteConfirmModal').modal('show');return false;" />
                <asp:Button runat="server" ID="btnCopy" Text="Copy" CssClass="btn btn-primary" ClientIDMode="Static" OnClientClick="$('#CLCopyModal').modal('show');return false;" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="margin-top: 20px; overflow: auto; height: 80vh;">
                <asp:ListView runat="server" ID="lvCheckGrid" ClientIDMode="Static" InsertItemPosition="LastItem">
                    <LayoutTemplate>
                        <table class="gvChecklist headerFixer lvCheckGrid">
                            <tr>
                                <th style="width: 130px;">Checkpoint ID</th>
                                <th>Checkpoint Description</th>
                                <th>Standard</th>
                                <th>Frequency</th>
                                <th>Sort Order</th>
                                <th>Delete?</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <table class="gvChecklist lvCheckGrid" style="margin-top: 20px;">
                            <tr>
                                <th style="width: 130px;">Checkpoint ID</th>
                                <th>Checkpoint Description</th>
                                <th>Standard</th>
                                <th>Frequency</th>
                                <th>Sort Order</th>
                                <th>Delete?</th>
                            </tr>
                    </EmptyDataTemplate>
                    <InsertItemTemplate>
                        <tr runat="server" class="trInsertItemTemplate show-row" id="trInsertItemTemplate" clientidmode="static">
                            <td>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:TextBox runat="server" ID="txtSlNo" Text="" CssClass="textboxcommon txtCheckpointID form-control"></asp:TextBox>
                            </td>
                            <%--  <td>
                                <asp:TextBox runat="server" ID="lblCheckPoint" Text="" CssClass="textboxcommon txtCheckpointID form-control"></asp:TextBox>
                            </td>--%>
                            <td style="width: 40%;">
                                <asp:TextBox runat="server" ID="txtCheckPointDesc" Text="" CssClass="txtUpdate textboxcommon  form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStandard" Text="" CssClass="txtUpdate textboxcommon form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="txtUpdate select textboxcommon form-control">
                                    <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSortOrder" ClientIDMode="Static" Text="" CssClass="txtUpdate textboxcommon form-control"></asp:TextBox>
                            </td>
                            <td style="text-align: center;">
                                <asp:CheckBox runat="server" ID="chkDelete" ClientIDMode="Static" Text="" CssClass="txtUpdate" />
                            </td>
                        </tr>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:Label runat="server" ID="lblSlNo" Text='<%# Eval("CheckPoint") %>' CssClass="textboxcommon txtCheckpointID"></asp:Label>
                            </td>
                            <%-- <td>
                                <asp:Label runat="server" ID="lblCheckPoint" Text='<%# Eval("CheckPoint") %>' CssClass="textboxcommon txtCheckpointID"></asp:Label>
                            </td>--%>
                            <td style="width: 40%;">
                                <asp:TextBox runat="server" ID="txtCheckPointDesc" Text='<%# Eval("CheckPointDesc") %>' CssClass="txtUpdate form-control textboxcommon"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtStandard" Text='<%# Eval("Standard") %>' CssClass="txtUpdate textboxcommon form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnFrequency" ClientIDMode="Static" Value='<%# Eval("Frequency") %>' />
                                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="txtUpdate select textboxcommon form-control">
                                    <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSortOrder" ClientIDMode="Static" Text='<%# Eval("SortOrder") %>' CssClass="txtUpdate textboxcommon form-control"></asp:TextBox>
                            </td>
                            <td style="text-align: center;">
                                <asp:CheckBox runat="server" ID="chkDelete" ClientIDMode="Static" Text="" CssClass="txtUpdate" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                             <div class="modal fade" id="deleteConfirmModal" role="dialog">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content modalContent confirm-modal-content">
                        <div class="modal-header modalHeader confirm-modal-header">
                            <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                            <br />
                            <h4 class="confirm-modal-title">Confirmation!</h4>
                            <br />
                            <span id="DeleteMsg" class="confirm-modal-msg">Are you sure you want to delete Record?</span>
                        </div>
                        <div class="modal-footer modalFooter modal-footer">
                            <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn"  OnClick="btnDelete_Click" ClientIDMode="Static" />
                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    
    <div class="modal infoModal" id="CLCopyModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 650px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="headerCopyModal" runat="server">Checklist Copy</h4>
                </div>
                <div class="modal-body" style="padding: 1px 15px !important">
                            <table class="innertbl" style="display: flex; justify-content: center; margin: 20px 0px;">
                                <tr>
                                    <td>Src MachineID</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSourceMachineID" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <%--<asp:DropDownList runat="server" ID="ddlSourceMachineID" CssClass="form-control"></asp:DropDownList>--%>
                                    </td>
                                    <td>Machines</td>
                                    <td>
                                        <asp:ListBox runat="server" ID="lbMultiMachineID" ClientIDMode="Static" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                    <div class="modal-footer" style="border: 0px !important; border-top: 1px solid black !important;">
                        <asp:Button runat="server" ID="btnCopyModal" OnClick="btnCopyModal_Click" Text="Save" ClientIDMode="Static" CssClass="btn btn-primary" />
                        <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="$('#CLCopyModal').modal('hide');return false;">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $("#btnCancel").hide();
            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            ShowHideNewItem();
        });

        function ShowHideNewItem() {
            var NoofRows = $(".lvCheckGrid tr");
            var firstrow = $(".lvCheckGrid tr")[NoofRows.length - 1];
            if ($(firstrow).hasClass("show-row")) {
                $(firstrow).removeClass("show-row");
                $(firstrow).addClass("hide-row");
            }
            else {
                $(firstrow).removeClass("hide-row");
                $(firstrow).addClass("show-row");
            }
        }

        $("#btnNew").on("click", function () {
            $("#hdnValueNewEdit").val("New");
            ShowHideNewItem();
            $("#btnCancel").show();
            $(".lvCheckGrid tr td").find("#txtSlNo").focus();
            $("#btnNew").hide();
            return false;
        });

        $("#btnCancel").on("click", function () {
            $("#hdnValueNewEdit").val("");
            ShowHideNewItem();
            $("#btnCancel").hide();
            $("#btnNew").show();
            return false;
        });

        $("#btnSave").on("click", function () {
            $("#btnCancel").hide();
            $("#btnNew").show(); 
            return true;
        })

        $(".lvCheckGrid tr td").focusin(function () {
            var hdnUpdate = $(this).closest("tr").find("#hdnUpdate");
            hdnUpdate.val("Updated");
        })

        $(".tblfilter tr td").focusin(function () {
            debugger;
            var hdnHeaderUpdate = $(this).closest("tr").find("#hdnHeaderValues");
            hdnHeaderUpdate.val("Update");
        })

        function CheckpointIDValidation() {
            var rows = $("#gvChecklist tr");
            if (rows.length > 0) {
                var firstRow = rows[0];
                if (firstRow.cells[1].innerHTML == "") {
                    $("#openWarningMessageModal").modal("show");
                    $("#lblWarningMessage").text("Checkpoint ID Cannot be empty.");
                    return false;
                }
            }
            return true;
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            $(document).ready(function () {
                $("#btnCancel").hide();
                ShowHideNewItem();
            });

            $("#btnNew").on("click", function () {
                $("#hdnValueNewEdit").val("New");
                ShowHideNewItem();
                $("#btnCancel").show();
                $(".lvCheckGrid tr td").find("#txtSlNo").focus();
                $("#btnNew").hide();
                return false;
            });

            $("#btnCancel").on("click", function () {
                $("#hdnValueNewEdit").val("");
                ShowHideNewItem();
                $("#btnCancel").hide();
                $("#btnNew").show();
                return false;
            });

            $("#btnSave").on("click", function () {
                $("#btnCancel").hide();
                $("#btnNew").show();
                return true;
            })

            $(".lvCheckGrid tr td").focusin(function () {
                var hdnUpdate = $(this).closest("tr").find("#hdnUpdate");
                hdnUpdate.val("Updated");
            });

            $(".tblfilter tr td").focusin(function () {
                debugger;
                var hdnHeaderUpdate = $(this).closest("tr").find("#hdnHeaderValues");
                hdnHeaderUpdate.val("Update");
            })

        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>




