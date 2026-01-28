<%@ Page Title="Process Parameter Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterMasterScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.ProcessParameterMasterScreen" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>

    <style>
        #tblPPMasterDetails tr th:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 5;
            background-color: #2e6886;
            color: white;
        }

        #tblPPMasterDetails tr:nth-child(odd) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 2;
            background-color: #DCDCDC;
        }

        #tblPPMasterDetails tr:nth-child(even) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 2;
            background-color: #FFFFFF;
        }
    </style>
    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnModalValues" ClientIDMode="Static" />
                <div class="bajaj-outer-div-filter-section">
                    <div class="bajaj-inner-div-filter-section left-content-filter-section">
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>Machine</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control"></asp:DropDownList>
                                     <%--AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"--%>
                                </td>
                              <%--  <td>Component</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlComponent" CssClass="form-control"></asp:DropDownList>
                                </td>--%>
                                <td>
                                    <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" />
                                    <asp:Button runat="server" ID="btnAddPPMasterDetails" Text="Add Parameter" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" OnClick="btnAddPPMasterDetails_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px">
                    <asp:ListView runat="server" ID="lvPPMasterDetails" ClientIDMode="Static">
                        <EmptyDataTemplate>
                            <table class="table table-bordered table-hover headerFixer" id="tblPPMasterDetails">
                                <tr>
                                    <th>Sl. No.</th>
                                    <th>Parameter ID</th>
                                    <th>Parameter Register</th>
                                    <th>Display Text</th>
                                    <th>Source Data Type</th>
                                    <th>Template Type</th>
                                    <th>Unit</th>
                                    <th>Low Value</th>
                                    <th>High Value</th>
                                    <th>Min Register</th>
                                    <th>Max Register</th>
                                    <th>Chart Type</th>
                                    <th>Visible</th>
                                    <th>Sort Order</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr>
                                    <td colspan="30" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblPPMasterDetails">
                                <tr>
                                    <th>Sl. No.</th>
                                    <th>Parameter ID</th>
                                    <th>Parameter Register</th>
                                    <th>Display Text</th>
                                    <th>Source Data Type</th>
                                    <th>Template Type</th>
                                    <th>Unit</th>
                                    <th>Low Value</th>
                                    <th>High Value</th>
                                    <th>Min Register</th>
                                    <th>Max Register</th>
                                    <th>Chart Type</th>
                                    <th>Visible</th>
                                    <th>Sort Order</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblSerialNum" Text='<%# Eval("SerialNum") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblParameterId" Text='<%# Eval("ParameterId") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hfAutoID" Value='<%# Eval("IDD") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblParameterRegister" Text='<%# Eval("ParameterRegister") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDisplayText" Text='<%# Eval("DisplayText") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblSourceDataType" Text='<%# Eval("SourceDataType") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblTemplateType" Text='<%# Eval("TemplateType") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblUnit" Text='<%# Eval("Unit") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLowerValue" Text='<%# Eval("LowerValue") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblHigherValue" Text='<%# Eval("HigherValue") %>'></asp:Label>
                                </td>

                                <td>
                                    <asp:Label runat="server" ID="lblMinRegister" Text='<%# Eval("MinRegister") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMaxRegister" Text='<%# Eval("MaxRegister") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblChartType" Text='<%# Eval("ChartType") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbIsVisible" ClientIDMode="Static" Checked='<%# Eval("IsVisible") %>' onclick="return false;" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblSortOrder" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                </td>
                                <td style="white-space: nowrap">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton runat="server" ID="lbEdit" CssClass=" bajaj-action-icons bajaj-add-edit-btn-style" ToolTip="Edit" OnClick="lbEdit_Click">
                                            <i class="glyphicon glyphicon-pencil"></i>
                                            <span>EDIT</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lbDelete" CssClass="bajaj-action-icons bajaj-delete-icons" ToolTip="Delete" OnClick="lbDelete_Click">
                                            <i class="glyphicon glyphicon-trash "></i>
                                           <span>DELETE</span> 
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lbEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lbDelete" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

                <div class="modal infoModal bajaj-info-modal" id="neweditPPModal" role="dialog" style="min-width: 1131px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 1131px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title" id="modalTitle" runat="server"></h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('neweditPPModal','compare');"></i>
                                <asp:HiddenField runat="server" ID="hfPPNewEdit" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Parameter ID *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtParameterId" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowNumber"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hfAutoID" />
                                            </td>
                                            <td>Parameter Register *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtParameterRegister" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Display Text *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDisplayText" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Source Data  Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlSourceDataType" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                            </td>
                                            <td>Template Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlTemplateType" ClientIDMode="Static" CssClass="form-control txtstyle" onchange="TemplateTypeChange();"></asp:DropDownList>
                                            </td>
                                            <td>Unit</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtUnit" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Low Value</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLowValue" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowDecimalWithOperator"></asp:TextBox>
                                            </td>
                                            <td>High Value</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtHighValue" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowDecimalWithOperator"></asp:TextBox>
                                            </td>
                                            <td>Min Register</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMinRegister" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Max Register</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMaxRegister" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Chart Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlChartType" ClientIDMode="Static" CssClass="form-control txtstyle">
                                                   <asp:ListItem Value="Guage" Text="Guage"></asp:ListItem>
                                                    <asp:ListItem Value="Temperature" Text="Temperature"></asp:ListItem>
                                                    <asp:ListItem Value="LinearGuage" Text="LinearGuage"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Visible</td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="cbIsVisible" ClientIDMode="Static" />
                                            </td>
                                            <td>Sort Order</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtSortOrder" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowNumber"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('neweditPPModal','compare');" class="bajaj-btn-style cancel-btn">CANCEL</button>
                                <asp:Button runat="server" ID="btnPPDetailsSave" ClientIDMode="Static" Text="Save" CssClass="bajaj-btn-style   bajaj-add-edit-btn-style" OnClientClick="return PPValidation();" OnClick="btnPPDetailsSave_Click" />
                            </div>
                        </div>
                    </div>
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
    </div>
    <script>
        $(document).ready(function () {

        });
        function PPValidation() {
            if ($('#txtParameterId').val() == "") {
                toasterWarningMsg("Parameter ID is required.", "");
                return false;
            }
            if ($('#txtParameterRegister').val() == "") {
                toasterWarningMsg("Parameter Register is required.", "");
                return false;
            }
            if ($('#txtDisplayText').val() == "") {
                toasterWarningMsg("Display Text is required.", "");
                return false;
            }
            let lv = $('#txtLowValue').val();
            let hv = $('#txtHighValue').val();
            if (lv != "" && hv != "") {
                if (parseFloat(lv) > parseFloat(hv)) {
                    toasterWarningMsg("Low Value should be less than High Value.", "");
                    return false;
                }
            }
            return true;
        }
        function TemplateTypeChange() {
            let value = $("#ddlTemplateType").val();
            if (value == "1 Value" || value == "2 Value") {
                $('#txtLowValue').val("");
                $('#txtHighValue').val("");
                $('#txtLowValue').attr('disabled', 'disabled');
                $('#txtHighValue').attr('disabled', 'disabled');
            } else {
                $('#txtLowValue').removeAttr('disabled');
                $('#txtHighValue').removeAttr('disabled');
            }
        }
        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('deleteConfirmModal');
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {

            });
            function openDeleteConfirmModal(msg) {
                $("#DeleteMsg").text(msg);
                openDeleteModal('deleteConfirmModal');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
