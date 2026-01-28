<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FiveSChecksheetMasterDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.FiveSChecksheetMasterDenso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>

                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>

                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-info" OnClick="btnNew_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-info" OnClick="btnCancel_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" OnClientClick="return saveValidation();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-info" OnClientClick="$('#deleteConfirmModal').modal('show');return false;" />
                    </td>
                </tr>
            </table>
            <div style="height: 75vh; overflow: auto;" id="gridContainer">
                <asp:GridView ID="gvChecklist" CssClass="table table-bordered headerFixer alternate-table-style" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static" ShowFooter="true" OnRowDataBound="gvChecklist_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Checkpoint">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hdnMachineID" ClientIDMode="Static" Value='<%# Eval("MachineID") %>' />
                                <asp:Label ID="lblCheckpoint" runat="server" Text='<%# Eval("Checkpoint") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtCheckpoint" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cycle">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnShifts" ClientIDMode="Static" Value='<%# Eval("Shifts") %>' />
                                <asp:ListBox ID="lbShift" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ListBox ID="lbShift" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="listBoxControl"></asp:ListBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CheckPoint Type">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnCheckpointType" ClientIDMode="Static" Value='<%# Eval("CheckpointType") %>' />
                                <asp:DropDownList runat="server" ID="ddlChecklistType" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Text="Checkbox" Value="Checkbox"></asp:ListItem>
                                    <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                    <asp:ListItem Text="OK-NG" Value="OK-NG"></asp:ListItem>
                                    <asp:ListItem Text="Image" Value="Image"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ID="ddlChecklistType" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Text="Checkbox" Value="Checkbox"></asp:ListItem>
                                    <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                    <asp:ListItem Text="OK-NG" Value="OK-NG"></asp:ListItem>
                                    <asp:ListItem Text="Image" Value="Image"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sort Order">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtSortOrder" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("SortOrder") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtSortOrder" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSelect" ClientIDMode="Static" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle CssClass="FooterRow" />
                </asp:GridView>
            </div>
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
                            <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                            <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $('.listBoxControl').multiselect({
                includeSelectAllOption: true
            });
        }
        $("[id$=gvChecklist]").on("click", "td", function () {
            $(this).closest('tr').find('#hdnUpdate').val("update");
        });
        function openCopyModal() {
            $('#copyModal').modal('show');
            return false;
        }
        function saveValidation() {
            if ($('#gvChecklist tr.FooterRow').length > 0) {
                if ($('#ddlRevNo')[0] == undefined) {
                    if ($('#txtRevNo').val().trim() == "") {
                        openWarningModal_1("Please enter Revision Number.");
                        return false;
                    }
                }
                var row = $('#gvChecklist tr.FooterRow')[0];
                if ($(row).find('#txtCheckpoint').val() == "") {
                    openWarningModal_1("Please enter Check Point ID.");
                    return false;
                }
            }
            return true;
        }
        function setScrollToBottotm() {
            window.onload = function () {                $("#gridContainer").animate({ scrollTop: $("#gridContainer")[0].scrollHeight }, 1000);            }
        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
            $("[id$=gvChecklist]").on("click", "td", function () {
                $(this).closest('tr').find('#hdnUpdate').val("update");
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
