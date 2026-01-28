<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyChecklistMasterDenso.aspx.cs" Inherits="Web_TPMTrakDashboard.Denso.DailyChecklistMasterDenso" %>

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

                    <td class="commanTd" style="vertical-align: middle;">Machine ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Rev. No.</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRevNo" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtRevNo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
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
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-info" OnClick="btnDelete_Click" OnClientClick="return deleteValidation();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnNewRevision" CssClass="btn btn-primary btnStyle" ClientIDMode="Static" Text="New Revision" OnClientClick="return newRevisionClick();" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnCopy" CssClass="btn btn-primary btnStyle" ClientIDMode="Static" Text="Copy" OnClientClick="return openCopyModal();" />
                    </td>

                </tr>
            </table>
            <div style="height: 75vh; overflow: auto;" id="gridContainer">
                <asp:GridView ID="gvChecklist" CssClass="table table-bordered headerFixer alternate-table-style" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static" ShowFooter="true" OnRowDataBound="gvChecklist_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Category">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hdnMachineID" ClientIDMode="Static" Value='<%# Eval("MachineID") %>' />
                                <asp:HiddenField runat="server" ID="hdnRevID" ClientIDMode="Static" Value='<%# Eval("RevID") %>' />
                                <asp:HiddenField runat="server" ID="hdnRevNo" ClientIDMode="Static" Value='<%# Eval("RevNo") %>' />
                                <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtCategory" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Checkpoint">
                            <ItemTemplate>
                                <asp:Label ID="lblChecklistID" runat="server" Text='<%# Eval("ChecklistID") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtChecklistID" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Checkpoint Desc">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtChecklistDesc" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("ChecklistDesc") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtChecklistDesc" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Judgement Criteria">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtJudgementCriteria" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("JudgementCriteria") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtJudgementCriteria" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Method">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtMethod" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("Method") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtMethod" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cycle">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtCycle" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("Cycle") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtCycle" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Person In Charge">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtPersonInCharge" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("PersonInCharge") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtPersonInCharge" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Frequency">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnFrequency" ClientIDMode="Static" Value='<%# Eval("Frequency") %>' />
                                <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                                    <asp:ListItem Text="Shift" Value="Shift"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                                    <asp:ListItem Text="Shift" Value="Shift"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CheckPoint Type">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdnChecklistType" ClientIDMode="Static" Value='<%# Eval("ChecklistType") %>' />
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
                        <asp:TemplateField HeaderText="Format Number">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtFormatNumber" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("FormatNumber") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox runat="server" ID="txtFormatNumber" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                               <asp:CheckBox runat="server" ID="chkSelect" CssClass="chkSelect"  ClientIDMode="Static"/>
                            </ItemTemplate>
                            <FooterTemplate>
                                
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--   <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkDelete" ClientIDMode="Static"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <FooterStyle CssClass="FooterRow" />
                </asp:GridView>
            </div>
            <div class="modal infoModal bajaj-info-modal" id="newRevNumberModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 50%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">New Revision</h4>
                            <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                        </div>
                        <div class="modal-body">
                            <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                    <tr>
                                        <td>Enter New Revision Number *</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNewRevNumber" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnInsertNewRevNumber" Text="Save" CssClass="infoModal-btn" OnClientClick="return revNumberValidation();" OnClick="btnInsertNewRevNumber_Click" />
                            <input type="button" value="Close" class="infoModal-btn" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal infoModal bajaj-info-modal" id="copyModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 600px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Copy</h4>
                            <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                        </div>
                        <div class="modal-body">
                            <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                    <tr>
                                        <td>Source Machine</td>
                                        <td>
                                            <asp:Label runat="server" ID="lblCopySourceMachine" ClientIDMode="Static"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">Destination Machine</td>
                                        <td>
                                            <asp:ListBox ID="lbCopyDestMachine" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnCopyConfirm" Text="Save" CssClass="infoModal-btn" OnClientClick="return copyValidation();" OnClick="btnCopyConfirm_Click" />
                            <input type="button" value="Close" class="infoModal-btn" data-dismiss="modal" />
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
        function deleteValidation() {
            if ($('#gvChecklist .chkSelect input[type="checkbox"]:checked').length == 0) {
                openWarningModal_1("Please select record.");
                return false;
            }
            return true;
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
                if ($(row).find('#txtChecklistID').val() == "") {
                    openWarningModal_1("Please enter Check Point ID.");
                    return false;
                }
            }
            return true;
        }
        function copyValidation() {
            if ($('#lbCopyDestMachine').val() == "" || $('#lbCopyDestMachine').val() == null) {
                openWarningModal_1("Please select destination machine.")
                return false;
            }
            return true;
        }
        function newRevisionClick() {
            $('#txtNewRevNumber').val("");
            $('#newRevNumberModal').modal('show');
            return false;

        }
        function revNumberValidation() {
            debugger;
            if ($('#txtNewRevNumber').val().trim() == "") {
                openWarningModal_1("Please enter Revision Number.");
                return false;
            }
            var newRev = $('#txtNewRevNumber').val().toLowerCase();
            var options = $('#ddlRevNo option');
            for (var i = 0; i < options.length; i++) {
                let valueLowerCase = $(options[i]).text().toLowerCase();
                if (valueLowerCase == newRev) {
                    openWarningModal_1("Revision Number already exists.");
                    return false;
                }
            }
            $(".modal-backdrop").removeClass("modal-backdrop in");
            return true;
        }
        function setControls() {
            $('[id$=lbCopyDestMachine]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px',
            });
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
