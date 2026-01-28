<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleMasterSKS.aspx.cs" Inherits="Web_TPMTrakDashboard.SKS.ScheduleMasterSKS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .commanTd {
            vertical-align: middle !important;
        }

        #gvScheduleData > tbody > tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #gvScheduleData > tbody > tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }
         #gvScheduleData > tbody > tr th{
             text-align:center;
         }

        .gridTbl {
            width: 100%;
        }

            .gridTbl tr td, .gridTbl tr th {
                width: 100px;
                border: unset !important;
                white-space: nowrap;
            }
             .gridTbl tr th{
                 border:1px solid #ddd !important;
             }

        .gridMergeTD {
            padding: 0px !important;
        }
        .warning-modal-content {
            border: 2px solid #f7d631;
            background-color: white;
            border-radius: 6px;
            text-align: center;
        }

        .warning-modal-header {
            background-color: #f7d631;
            padding: 15px;
            color: black;
            text-align: center;
        }

        .warning-modal-title {
            color: black;
            font-weight: bold;
            font-size: 28px;
        }

        .warning-modal-msg {
            font-size: 18px;
            color: black;
        }

        .warning-modal-btn {
            background-color: #f7d631;
            border: 1px solid #f7d631;
            color: black;
            font-weight: bold;
            padding: 8px 25px;
            border-radius: 8px;
        }

        .error-modal-content {
            border: 2px solid #f50505;
            background-color: white;
            border-radius: 6px;
            text-align: center;
        }

        .error-modal-header {
            background-color: #f50505;
            padding: 15px;
            color: white;
            text-align: center;
        }

        .error-modal-title {
            color: white;
            font-weight: bold;
            font-size: 28px;
        }

        .error-modal-msg {
            font-size: 18px;
            color: white;
        }

        .error-modal-btn {
            background-color: #f50505;
            border: 1px solid #f50505;
            color: white;
            font-weight: bold;
            padding: 8px 25px;
            border-radius: 8px;
        }

        .modal-footer {
            padding: 18px;
            text-align: center;
        }

        .modal-icons {
            font-size: 4pc;
        }

        .infoModal .modal-content {
            border: 1px solid #6c7884;
            background: #1d1d1d;
        }

        .infoModal .modal-footer {
            padding: 9px;
            border: 1px solid #6c7884;
            background-color: #1d1d1d;
            margin: 0px;
        }

        .infoModal .modal-header {
            border-bottom: 1px solid #6c7884;
            background-color: #6c7884;
        }

            .infoModal .modal-header .modal-title {
                color: white;
            }

            .infoModal .modal-header h4 {
                font-weight: bold;
            }

        .infoModal .modal-body {
            border-left: 1px solid #6c7884;
            border-right: 1px solid #6c7884;
            background-color: white;
        }

        .infoModal .div-border-style {
            border-bottom: 1px solid #575a5d;
        }

        .infoModal .div-border-top-style {
            border-top: 1px solid #575a5d;
        }

        .priorityTypeDiv {
            width: 450px;
            background-color: #f8f8f8;
            border: 1px solid #c2bdbd;
            padding: 5px 10px;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table id="tblFilter" class="table table-bordered" style="width: auto">
                <tr>
                    <td class="commanTd">Machine ID</td>
                    <td>
                        <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" AutoPostBack="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged" />
                    </td>
                    <td class="commanTd">Part ID</td>
                    <td>
                        <asp:DropDownList ID="ddlPartID" runat="server" CssClass="form-control" ClientIDMode="Static" />
                    </td>
                    <td class="commanTd">Status</td>
                    <td>
                        <asp:ListBox ID="lbStatus" runat="server" SelectionMode="Multiple" Style="width: 220px;" ClientIDMode="Static">
                            <asp:ListItem Text="New" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Running" Value="2" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Hold" Value="3" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtWorkOrder" ClientIDMode="Static" placeholder="WorkOrder Search" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button runat="server" Text="View" ID="btnView" CssClass="btn btn-primary btnStyle" OnClick="btnView_Click" OnClientClick="return viewValidation();" />
                    </td>
                    <td style="display:none">
                        <asp:Button runat="server" Text="Change Priority" ID="btnChangePriority" CssClass="btn btn-primary btnStyle" OnClick="btnChangePriority_Click" OnClientClick="return prioritySelectValidation();"  Visible="false"/>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="height: 80vh; overflow: auto">
                <asp:GridView runat="server" ID="gvScheduleData" ClientIDMode="Static" CssClass="table table-bordered headerFixer " AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="User Priority">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblUserPriority" ClientIDMode="Static" Text='<%# Eval("UserPriority") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Schedule Priority">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSchedulePriority" ClientIDMode="Static" Text='<%# Eval("SchedulePriority") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Machine Name">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WO Number">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWorkOrder" ClientIDMode="Static" Text='<%# Eval("WorkOrder") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Catalog code">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPartID" ClientIDMode="Static" Text='<%# Eval("PartID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Catalog Code Description">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPartDesc" ClientIDMode="Static" Text='<%# Eval("PartDesc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tool Layout">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblToolLayout" ClientIDMode="Static" Text='<%# Eval("ToolLayout") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SKS Drawing number">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDrawingNumber" ClientIDMode="Static" Text='<%# Eval("DrawingNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RM Grade &Size ">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblRMGradeSize" ClientIDMode="Static" Text='<%# Eval("RMGradeSize") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Planned Quantity" ItemStyle-CssClass="gridMergeTD" HeaderStyle-CssClass="gridMergeTD">
                            <HeaderTemplate>
                                <table class="gridTbl">
                                    <tr class="gridTblHeader">
                                        <th colspan="2" style="text-align: center">Planned Quantity</th>
                                    </tr>
                                    <tr>
                                        <th class="gridTblSubHeader CycleFrequency">Nos.</th>
                                        <th class="CycleNumber">Wt in KGs</th>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table class="gridTbl" border="0">
                                    <tr>
                                        <td class="gridTblFirstTd innerTblTd CycleFrequency">
                                            <asp:Label runat="server" ID="lblPlannedQtyNo" ClientIDMode="Static" Text='<%# Eval("PlannedQtyNo") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblPlannedQtyWt" ClientIDMode="Static" Text='<%# Eval("PlannedQtyWt") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operation no.">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblOperationNo" ClientIDMode="Static" Text='<%# Eval("OperationNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Speed">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSpeed" ClientIDMode="Static" Text='<%# Eval("Speed") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStatus" ClientIDMode="Static" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UpdatedTS">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblUpdatedTS" ClientIDMode="Static" Text='<%# Eval("UpdatedTS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSelect" ClientIDMode="Static" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="modal infoModal" id="changePriorityModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog  modal-dialog-centered " style="width: 60%">
                    <div class="modal-content">
                        <div class="modal-header">

                            <h4 class="modal-title" style="color: white;">Change Signature Details</h4>
                            <%-- <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>--%>
                        </div>
                        <div class="modal-body">
                            Select options to change the priorities of following items.
                            <div>
                                <asp:GridView runat="server" ID="gvChangePriority" ClientIDMode="Static" CssClass="table table-bordered headerFixer " AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="User Priority">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblUserPriority" ClientIDMode="Static" Text='<%# Eval("UserPriority") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Schedule Priority">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSchedulePriority" ClientIDMode="Static" Text='<%# Eval("SchedulePriority") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Machine Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("MachineID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WO Number">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblWorkOrder" ClientIDMode="Static" Text='<%# Eval("WorkOrder") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Catalog code">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPartID" ClientIDMode="Static" Text='<%# Eval("PartID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Catalog Code Description">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPartDesc" ClientIDMode="Static" Text='<%# Eval("PartDesc") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Operation no.">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblOperationNo" ClientIDMode="Static" Text='<%# Eval("OperationNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            Select whether you want to move thsee records before a specific priority or to end.
                            <div class="priorityTypeDiv">
                                <asp:RadioButton ID="radioMoveToEnd" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" Checked="true" GroupName="MoveOptions" ClientIDMode="Static" />
                                <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move to end</label>
                                <asp:RadioButton ID="radioMoveBefore" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" GroupName="MoveOptions" ClientIDMode="Static" />
                                <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move Before</label>
                                <asp:DropDownList runat="server" ID="ddlPriorities" CssClass="form-control" Style="display: inline; width: 150px; margin-left: 5px;" Enabled="false" ClientIDMode="Static" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" Text="Save" ID="btnSavePriority" OnClick="btnSavePriority_Click" CssClass="btn btn-primary" />
                            <button type="button" data-dismiss="modal" class="btn btn-primary">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
        </Triggers>
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
            $.unblockUI({});
            setControls();
        });
        //function priorityDDLDisplay(ctrl, param) {
        //    debugger;
        //    if (param == "movebefore") {
        //        if ($(ctrl)[0].checked == true) {
        //            $("#ddlPriorities").prop("disabled", false);
        //        }
        //        else {
        //            $("#ddlPriorities").prop("disabled", true);
        //        }
        //    }
        //    else {
        //        if ($(ctrl)[0].checked == true) {
        //            $("#ddlPriorities").prop("disabled", true);
        //        }
        //        else {
        //            $("#ddlPriorities").prop("disabled", false);
        //        }
        //    }
        //}
        $("#radioMoveBefore").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlPriorities").prop("disabled", false);
            }
            else {
                $("#ddlPriorities").prop("disabled", true);
            }
        });
        $("#radioMoveToEnd").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlPriorities").prop("disabled", true);
            }
            else {
                $("#ddlPriorities").prop("disabled", false);
            }
        });
        function prioritySelectValidation() {
            if ($('#gvScheduleData #chkSelect:checked').length == 0) {
                openWarningModal("Please select row.");
                return false;
            }
            return true;
        }
        function viewValidation() {
            if ($('#ddlMachineId').val() == "" || $('#ddlMachineId').val() == null) {
                openWarningModal("Please select Machine.");
                return false;
            }
            if ($('#ddlPartID').val() == "" || $('#ddlPartID').val() == null) {
                openWarningModal("Please select Part.");
                return false;
            }
            if ($('#lbStatus').val() == "" || $('#lbStatus').val() == null) {
                openWarningModal("Please select Status.");
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function setControls() {
            $('[id*=lbStatus]').multiselect({
                includeSelectAllOption: true
            });
        }
        function openChangePriorityModal() {
            $("#changePriorityModal").modal('show');
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
                $.unblockUI({});
            });
            $("#radioMoveBefore").change(function () {
                if ($(this)[0].checked == true) {
                    $("#ddlPriorities").prop("disabled", false);
                }
                else {
                    $("#ddlPriorities").prop("disabled", true);
                }
            });
            $("#radioMoveToEnd").change(function () {
                if ($(this)[0].checked == true) {
                    $("#ddlPriorities").prop("disabled", true);
                }
                else {
                    $("#ddlPriorities").prop("disabled", false);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
