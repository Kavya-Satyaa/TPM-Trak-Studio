<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductionScheduleMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.ProductionScheduleMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="content-header-section">
                <div style="display: inline-block;">
                    <div class="div-selected-menu-name">
                        <span class="selected-menu-name">Cockpit</span>
                    </div>
                    <div class="div-selected-filter">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <span class="filter-selection" runat="server" id="lblFilterSelection" clientidmode="static"></span>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
                <table class="submenu-right-side filter-table">
                    <tr>
                        <td onmouseover="showExportPanels('panelExport');" onmouseout=" hidePanels(this, 'panelExport');">
                            <i class="glyphicon glyphicon-save filter-icon"></i>
                            <div class="panel panel-default panel-subitems export-panel-div" id="panelExport" onmouseout="hidePanels(this,'panelExport')">
                                <div class="panel-body">
                                    <ul class="outer-ul">
                                        <li>
                                            <asp:LinkButton runat="server" ID="btnExport" Text="Export To Excel" OnClick="btnExport_Click"></asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                        <td id="tdPanelFilters" onclick="showPanelFilter(this,event);">
                            <i class="glyphicon glyphicon-filter filter-icon"></i>
                        </td>
                    </tr>
                </table>

                <div class="panel panel-default panel-subitems panel-table-style panel-filter" id="panelFilter" style="width: 680px;">
                    <div class="triangle-right"></div>
                    <div class="panel-heading">
                        <span class="filter-header-name">Filter</span>
                    </div>
                    <div class="panel-body">
                        <div class="panel-menu-conatiner">
                            <div class="navbar-collapse collapse">
                                <ul class="nav navbar-nav submenus-style" id="ulfilter">
                                    <li><a runat="server" class="filter-menus" clientidmode="static" data-toggle="tab" href="#AssetsMenu">Filter By Assets</a>
                                    </li>
                                    <li><a runat="server" class="filter-menus" clientidmode="static" data-toggle="tab" href="#OthersMenu">Filter By Others</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="tab-content themetoggle filter-tab-container">
                            <div id="AssetsMenu" class="tab-pane fade filter-tab-content">
                                <div>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>Company
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlCompany" CssClass="form-control dropdown-list panel-filter-controlstyle" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"></asp:DropDownList>
                                                        <asp:Label runat="server" ID="lblCompany" CssClass="form-control" ClientIDMode="Static"></asp:Label>
                                                    </td>
                                                    <td>Plant
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control dropdown-list panel-filter-controlstyle" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Machine
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control dropdown-list panel-filter-controlstyle" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlPlant" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlMachine" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlPart" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlOperation" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCharacteristic" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectedDate" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div id="OthersMenu" class="tab-pane fade filter-tab-content">
                                <div>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>Select Date
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtfromDate" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control panel-filter-controlstyle" Style="position: relative;" placeholder="DD-MM-YYYY HH:mm:ss"></asp:TextBox>
                                                    </td>

                                                    <td>Component ID
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlPart" CssClass="form-control dropdown-list panel-filter-controlstyle" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPart_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Operation
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlOperation" CssClass="form-control dropdown-list panel-filter-controlstyle" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>

                                                    <td>Characteristic
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlCharacteristic" CssClass="form-control dropdown-list panel-filter-controlstyle" ClientIDMode="Static" OnSelectedIndexChanged="ddlCharacteristic_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Group Size
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control panel-filter-controlstyle" ID="ddlGpSize">
                                                            <asp:ListItem>25</asp:ListItem>
                                                            <asp:ListItem>20</asp:ListItem>
                                                            <asp:ListItem>30</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td>Date Period
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control panel-filter-controlstyle" ID="ddlSelectedDate" OnSelectedIndexChanged="ddlSelectedDate_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Value="SelectedDateTime">Selected Till Now</asp:ListItem>
                                                            <asp:ListItem Value="SelectedDateShift">Selected Date and Shift</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="shiftContainer" runat="server">
                                                    <td>Shift
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control panel-filter-controlstyle" ID="ddlShift">
                                                            <asp:ListItem Value="SelectedDateTime">Selected Till Now</asp:ListItem>
                                                            <asp:ListItem Value="SelectedDateShift">Selected Date and Shift</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="2"></td>
                                                </tr>
                                            </table>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlPlant" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlMachine" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlPart" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlOperation" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCharacteristic" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlSelectedDate" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                                        </Triggers>

                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer" style="text-align: center">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>

                                <asp:Button runat="server" ID="btnOK" CssClass="Btns" OnClick="btnOK_Click" Text="OK" OnClientClick="return showLoader();" />
                                <input type="button" value="Cancel" class="Btns" onclick="hidePanelFilter()" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>


            </div>

            <asp:TextBox runat="server" ID="txtWorkOrderSearch" ClientIDMode="Static"></asp:TextBox>
            <asp:TextBox runat="server" ID="txtComponentSearch" ClientIDMode="Static"></asp:TextBox>
            <asp:DropDownList runat="server" ID="ddlStatus" ClientIDMode="Static">
                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
            </asp:DropDownList>
            <div>
                <asp:Button runat="server" CssClass="btn btn-success" Text="Save" ID="btnWoNew" Style="height: 30px; padding: 3px 15px;" OnClick="btnWoNew_Click" />
            </div>
            <div>
                <asp:GridView ID="gvWorkOrder" CssClass="table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static">
                    <Columns>
                        <asp:TemplateField HeaderText="WorkOrder Number">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkOrderNumber" runat="server" Text='<%# Eval("WorkOrder") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Component">
                            <ItemTemplate>
                                <asp:Label ID="lblComponent" runat="server" Text='<%# Eval("Component") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WorkOrder Date">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkOrderDate" runat="server" Text='<%# Eval("WorkOrderDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblWOEdit" CssClass="glyphicon glyphicon-pencil action-glypicon" OnClick="lblWOEdit_Click" ToolTip="Edit WorkOrder"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lblWODelete" CssClass="glyphicon glyphicon-trash action-glypicon" OnClick="lblWODelete_Click" ToolTip="Delete WorkOrder"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div class="modal infoModal" id="addEditInfo" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered " style="width: 800px">
                    <div class="modal-content modalThemeCss">
                        <div class="modal-header">
                            <h4 class="modal-title" runat="server" id="headerWO">Add WorkOrder</h4>

                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnNEStatusWO" />
                        </div>
                        <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                            <span class="mandatory-message"></span>

                            <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;" class="div-border-style">
                                <table style="width: 100%; margin: auto" class="modal-tbl">
                                    <tr>
                                        <td>Company *</td>
                                        <td>
                                            <asp:Label runat="server" ID="lblCompNECompany" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:Label>
                                            <%--  <asp:DropDownList runat="server" ID="ddlCompNECompany" ClientIDMode="Static" CssClass="form-control dropdown-list txtstyle">
                                                        </asp:DropDownList>--%>
                                        </td>
                                        <td colspan="2" style="width: 108px"></td>

                                    </tr>
                                </table>
                            </div>
                            <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;">
                                <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">

                                    <tr>
                                        <td>Work Order *</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNEWorkOrder" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>Quantity *</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNEWorkOrderQty" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>WorkOrder Date *</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNEWorkOrderDate" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle" Style="position: relative" placeholder="DD-MM-YYYY"></asp:TextBox>
                                        </td>
                                        <td>Component *</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlNEComponent" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <div id="routingContainer" runat="server" clientidmode="static">
                                    <asp:GridView ID="gvWORouting" CssClass="table table-bordered headerFixer" runat="server" AutoGenerateColumns="false" EmptyDataText="No records" ShowHeader="true" ShowHeaderWhenEmpty="true" ClientIDMode="Static">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Machine ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMachine" runat="server" Text='<%# Eval("Machine") %>' ClientIDMode="Static"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Component">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdnCompInterfaceID" ClientIDMode="Static" Value='<%# Eval("CompInterfaceId") %>' />
                                                    <asp:HiddenField runat="server" ID="hdnOpnInterfaceID" ClientIDMode="Static" Value='<%# Eval("OpnInterfaceId") %>' />
                                                    <asp:Label ID="lblComponent" runat="server" Text='<%# Eval("Component") %>' ClientIDMode="Static"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Operation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOperation" runat="server" Text='<%# Eval("Operation") %>' ClientIDMode="Static"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtQuantity" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select Route">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAllRoute" runat="server" Style="float: left; padding-right: 5px" onchange="setAllCheckbox(this);" />
                                                    <span>Assign </span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkSelect" Checked='<%# Eval("IsAssigned") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnWOSave" Text="Save" CssClass="Btns" OnClientClick="return workOrderInsertValidation();" OnClick="btnWOSave_Click" ClientIDMode="Static" />
                            <button type="button" data-dismiss="modal" class="Btns">Close</button>
                            <asp:Button runat="server" ID="btnWONew2" Text="Save" CssClass="Btns" OnClick="btnWoNew_Click" ClientIDMode="Static" />
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
                    <br />
                    <h4 class="warning-modal-title">Warning</h4>
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
                    <br />
                    <h4 class="error-modal-title">Error</h4>
                    <br />
                    <span class="error-modal-msg" id="lblErrorMsg">...</span>
                </div>
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
        function setAllCheckbox(ctrl) {
            debugger;
            if ($(ctrl).find('#chkSelectAllRoute').is(":checked")) {
                $('#gvWORouting #chkSelect').attr("checked", "checked");
            }
            else {
                $('#gvWORouting #chkSelect').removeAttr("checked", "checked");
            }
        }
        function workOrderInsertValidation() {
            if ($('#txtNEWorkOrder').val() == "") {
                $('.mandatory-message').text("WorkOrder is required.");
                return false;
            }
            if ($('#txtNEWorkOrderQty').val() == "") {
                $('.mandatory-message').text("WorkOrder Quantity is required.");
                return false;
            }
            if ($('#txtNEWorkOrderDate').val() == "") {
                $('.mandatory-message').text("WorkOrder Date is required.");
                return false;
            }
            if ($('#ddlNEComponent').val() == "" || $('#ddlNEComponent').val() == null) {
                $('.mandatory-message').text("Component is required.");
                return false;
            }
            if ($('#routingContainer')[0] != undefined) {
                var rows = $('#gvWORouting tr');
                for (var i = 1; i < rows.length; i++) {
                    let row = rows[i];
                    if ($(row).find('#chkSelectAllRoute').is(":checked")) {
                        if ($(row).find('#txtQuantity').val().trim() == "") {
                            let machine = $(row).find('#lblMachine').text();
                            let opn = $(row).find('#lblOperation').text();
                            $('.mandatory-message').text("Quanity is required for Machine '" + machine + "' and Operation '" + opn + "' .");
                            return false;
                        }
                    }
                }
            }
        }
        function openModal(modalid) {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            $("#" + modalid).modal('show');
        }
        function setControls() {
            $("#txtNEWorkOrderDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: 'en-US'
            });
        }
        function openWarningModal(msg) {
            $("#lblWarningMsg").text(msg);
            $("#warningModal").modal('show');
        }
        function openErrorModal(msg) {
            $('#errorModal').modal('show');
            $('#lblErrorMsg').text(msg);
        }
        function showSuccessMsg(msg, title) {
            debugger;
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "4000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "toastClass": "toaster-position"
            }

            toastr['success'](msg, title);
            return false;
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
