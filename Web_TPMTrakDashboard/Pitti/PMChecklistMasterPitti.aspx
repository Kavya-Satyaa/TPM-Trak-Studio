<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMChecklistMasterPitti.aspx.cs" Inherits="Web_TPMTrakDashboard.Pitti.PMChecklistMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .tblfilter {
            width: 95%;
            box-shadow: 0px 0px 2px black;
        }

            .tblfilter > tbody > tr > td {
                color: white;
                text-align: center;
                vertical-align: middle;
                font-size: 15px;
                border: 1px solid #4e5166 !important;
                padding: 5px;
            }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <table id="tblFilter" class="tblfilter" style="width: auto; margin-left: 10px;">
                <tr>
                    <td class="commanTd" style="vertical-align: middle;">Machine ID</td>
                    <td style="width: 14%">
                        <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Prepared By</td>
                    <td style="width: 14%">
                        <asp:DropDownList runat="server" ID="ddlPreparedBy" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Checked & Reviewed By</td>
                    <td style="width: 14%">
                        <asp:DropDownList runat="server" ID="ddlReviewedBy" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Approved By</td>
                    <td style="width: 14%">
                        <asp:DropDownList runat="server" ID="ddlApprovedBy" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Reference No.</td>
                    <td style="width: 10%">
                        <asp:TextBox runat="server" ID="txtRefNo" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="commanTd" style="vertical-align: middle;">Revision No.</td>
                    <td style="width: 8%">
                        <asp:TextBox runat="server" ID="txtRevNo" CssClass="form-control non-edit-mode"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Supervisor</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSupervisor" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Maintenance Manager</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMaintManager" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="vertical-align: middle;">Frequency</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control">
                            <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                            <asp:ListItem Text="Quaterly" Value="Quaterly"></asp:ListItem>
                            <asp:ListItem Text="Half-Yearly" Value="Half Yearly"></asp:ListItem>
                            <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <%--<td>
                        <asp:Button runat="server" ID="btnHeaderSave" Text="Save Headers" CssClass="btn btn-primary" OnClick="btnHeaderSave_Click" />
                    </td>--%>
                    <td>
                        <asp:Button runat="server" ID="BtnView" Text="View" CssClass="btn btn-primary" OnClick="BtnView_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-primary" OnClick="btnNew_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" OnClientClick="return saveValidation();" />
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary" OnClientClick="$('#deleteConfirmModal').modal('show');return false;" />
                        <asp:Button runat="server" ID="btnCopy" Text="Copy" CssClass="btn btn-primary" ClientIDMode="Static" OnClientClick="$('#CLCopyModal').modal('show');return false;" />
                    </td>
                </tr>
            </table>
            <div style="height: 80vh; overflow: auto; margin-top: 20px;" id="gridContainer">
                <asp:GridView runat="server" ID="gvPMMaster" ClientIDMode="Static" CssClass="table table-bordered headerFixer alternate-table-style" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ShowFooter="true" OnRowDataBound="gvPMMaster_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Category ID">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:Label runat="server" ClientIDMode="Static" ID="lblCategoryID" Text='<%# Eval("CategoryID")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control" ID="txtCategoryID"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Category">
                            <ItemTemplate>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblCategory" Text='<%# Eval("Category")%>'></asp:Label>--%>
                                <asp:HiddenField runat="server" ID="hdnCategory" ClientIDMode="Static" Value='<%# Eval("Category")%>' />
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddCategory">
                                    <asp:ListItem Value="Standard" Text="Standard"></asp:ListItem>
                                    <asp:ListItem Value="Mechanical" Text="Mechanical"></asp:ListItem>
                                    <asp:ListItem Value="Electrical" Text="Electrical"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlCategory">
                                    <asp:ListItem Value="Standard" Text="Standard"></asp:ListItem>
                                    <asp:ListItem Value="Mechanical" Text="Mechanical"></asp:ListItem>
                                    <asp:ListItem Value="Electrical" Text="Electrical"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Checkpoint ID">
                            <ItemTemplate>
                                <asp:Label runat="server" ClientIDMode="Static" CssClass="form-control" ID="lblCheckpointID" Text='<%# Eval("CheckpointID")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtCheckPointID" CssClass="form-control" ClientIDMode="Static" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblDescription" Text='<%# Eval("Description")%>'></asp:Label>--%>
                                <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txDescription" Text='<%# Eval("Description")%>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtDescription" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Judgement Criteria">
                            <ItemTemplate>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblJudgementCriteria" Text='<%# Eval("JudgementalCriteria")%>'></asp:Label>--%>
                                <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txJudgementCriteria" Text='<%# Eval("JudgementalCriteria")%>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtJudgementCriteria" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Resource Needed">
                            <ItemTemplate>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblResourceNeeded" Text='<%# Eval("ResourcesNeeded")%>'></asp:Label>--%>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txResourcesNeeded" CssClass="form-control" Text='<%# Eval("ResourcesNeeded")%>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtResourceNeeded" ClientIDMode="Static" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Frequency">
                            <ItemTemplate>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblFrequency" Text='<%# Eval("Frequency")%>'></asp:Label>--%>
                                <asp:HiddenField runat="server" ID="hdnFrequency" ClientIDMode="Static" Value='<%# Eval("Frequency")%>' />
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddFrequency">
                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                    <asp:ListItem Text="Quaterly" Value="Quaterly"></asp:ListItem>
                                    <asp:ListItem Text="Half-Yearly" Value="Half Yearly"></asp:ListItem>
                                    <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlFrequency">
                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                    <asp:ListItem Text="Quaterly" Value="Quaterly"></asp:ListItem>
                                    <asp:ListItem Text="Half-Yearly" Value="Half Yearly"></asp:ListItem>
                                    <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sort Order">
                            <ItemTemplate>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblSortOrder" Text='<%# Eval("SortOrder")%>'></asp:Label>--%>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txSortOrder" CssClass="form-control" Text='<%# Eval("SortOrder")%>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtSortOrder" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Duration">
                            <ItemTemplate>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblDuration" Text='<%# Eval("Duration")%>'></asp:Label>--%>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txDuration" CssClass="form-control" Text='<%# Eval("Duration")%>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtDuration" CssClass="form-control"></asp:TextBox>
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
                        <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseCopyModal();">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {

            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
        });
        $("[id$=gvPMMaster]").on("click", "td", function () {
            $(this).closest('tr').find('#hdnUpdate').val("update");
        });
        function setScrollToBottotm() {
            window.onload = function () {                $("#gridContainer").animate({ scrollTop: $("#gridContainer")[0].scrollHeight }, 1000);            }
        }
        function saveValidation() {
            if ($('#gvChecklist tr.FooterRow').length > 0) {
                var row = $('#gvChecklist tr.FooterRow')[0];
                if ($(row).find('#txtCategoryID').val() == "") {
                    openWarningModal_1("Please enter Category ID.");
                    return false;
                }
                else if ($(row).find('#txtCheckPointID').val() == "") {
                    openWarningModal_1("Please enter CheckPoint ID.");
                    return false;
                }
            }
            return true;
        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
            $("[id$=gvPMMaster]").on("click", "td", function () {
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
