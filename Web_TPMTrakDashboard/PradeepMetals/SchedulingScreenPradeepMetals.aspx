<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SchedulingScreenPradeepMetals.aspx.cs" Inherits="Web_TPMTrakDashboard.PradeepMetals.SchedulingScreenPradeepMetals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>

    <style>
        span {
            color: white;
        }

        .legendStyleSettings {
            color: white;
        }

        .searchCategory {
            border: 1px solid white;
        }

        .legendStyleSettings {
            display: block;
            padding: 4px;
            width: 40%;
            margin-bottom: 0px;
            font-size: 15px;
            color: white;
        }

        .searchTbl > tbody > tr > td:nth-child(even) {
            width: 30%;
            text-align: center;
        }

        .searchTbl {
            margin: 8px;
            width: 95%;
        }

        legend {
            border: 0px;
        }

        .Btn-style {
            border-radius: 2px;
            padding: 5px;
            box-shadow: 2px 2px 5px black;
            width: 70px;
        }

        .Btn-Import {
            width: 20px;
            height: 10px;
            color: blue;
        }

        .tblScheduleStatus > tbody > tr > th {
            background-color: #2d7093;
            color: white;
        }

        .tblScheduleStatus > tbody > tr:nth-child(odd) {
            background-color: white;
        }

        .tblScheduleStatus > tbody > tr:nth-child(even) {
            background-color: #dddcdc;
        }

        .Contents {
            color: black;
        }

        .date-control {
            position: relative;
        }

        .addScheduleTable span {
            color: black;
        }

        .allowNumber {
            box-shadow: 2px 2px 5px #e3e1e1;
        }

        .tblFilter > tbody > tr > td {
            color: white;
            padding: 5px;
        }

        .Year {
            color: black;
        }

        .datepicker span {
            color: black;
        }
    </style>
    <div style="display: inline-block; width: 98%">
        <div class="col-lg-4">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td><span>Schedule Status</span></td>
                            <td style="padding-left: 5px;">
                                <asp:DropDownList runat="server" ID="ddlScheduleStatus" OnSelectedIndexChanged="ddlScheduleStatus_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" ClientIDMode="Static">
                                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                    <asp:ListItem Text="Running" Value="Running"></asp:ListItem>
                                    <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlScheduleStatus" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div style="float: right; display: inline-flex" class="col-lg-7">
            <table>
                <tr>
                    <td style="width: 250px;">
                        <asp:FileUpload runat="server" ID="FileImport" CssClass="form-control" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="BtnImport" CssClass="bajaj-btn-style" Text="Import" OnClick="BtnImport_Click" />
                    </td>
                </tr>
            </table>
            <fieldset class="searchCategory" style="border-color: #373c52; box-shadow: 2px 2px black; margin-left: 9px;">
                <table class="tblFilter">
                    <tr>
                        <td>Year</td>
                        <td>
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtYear" CssClass="form-control Year"></asp:TextBox>
                        </td>
                        <td>Month</td>
                        <td>
                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMonth" CssClass="form-control Month"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="BtnExport" CssClass="bajaj-btn-style" Text="Export" OnClick="BtnExport_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>

    <table style="width: 95%;">
        <tr>
            <td style="width: 35%;">
                <fieldset class="searchCategory" runat="server" id="SearchbyDate" style="border-color: #373c52; box-shadow: 2px 2px">
                    <legend class="legendStyleSettings">Search By Scheduled Date</legend>
                    <table class="searchTbl">
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">From</td>
                            <td class="input-group" style="min-width: 130px; border: 0">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date-control" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">To</td>
                            <td class="input-group" style="min-width: 130px; border: 0">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date-control" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>&nbsp;
                                            <asp:Button runat="server" CssClass="btn btn-primary" ID="BtnView1" Text="View" Style="" OnClick="BtnView1_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td style="width: 45%">
                <fieldset class="searchCategory" id="searchby" style="border-color: #373c52; box-shadow: 2px 2px">
                    <legend class="legendStyleSettings">Search By Machine and PartNumber</legend>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <table class="searchTbl">
                                <tr>
                                    <td class="commanTd" style="vertical-align: middle;"><span style="font-size: 14px;">MachineID</span></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMachineID" ClientIDMode="Static" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                    </td>
                                    <td class="commanTd" style="vertical-align: middle;"><span style="font-size: 14px;">Part Number</span></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPartNumber" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:Button runat="server" ID="BtnView2" CssClass="btn btn-primary" Text="View" OnClick="BtnView2_Click" />
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlMachineID" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </fieldset>
            </td>
            <td style="padding-left: 20px;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="BtnRefresh" Text="Refresh" ClientIDMode="Static" CssClass="btn btn-primary Btn-style" OnClick="BtnRefresh_Click" />
                        <asp:Button runat="server" ID="BtnSave" Text="Save" ClientIDMode="Static" CssClass="btn btn-primary Btn-style" OnClick="BtnSave_Click" />
                        <asp:Button runat="server" ID="BtnNew" Text="New" ClientIDMode="Static" CssClass="btn btn-primary Btn-style" OnClientClick="return OpenNewEditModal();" />
                        <asp:Button runat="server" ID="BtnDelete" Text="Delete" ClientIDMode="Static" CssClass="btn btn-primary Btn-style" OnClick="BtnDelete_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnRefresh" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnDelete" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="margin-top: 10px;">
                <div style="height: 75vh; overflow: auto;">
                    <asp:ListView runat="server" ID="lvScheduleDetails" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered HeaderFixer tblScheduleStatus" id="tblScheduleStatus" style="width: 100%;">
                                <tr style="position: sticky; top: -1px; z-index: 2;">
                                    <th>Machine Name</th>
                                    <th>Priority</th>
                                    <th>PML Part Number</th>
                                    <th>Job Traveller Number</th>
                                    <th>Planned Qty</th>
                                    <th>Actual Qty</th>
                                    <th>Lot Code</th>
                                    <th>Operation Number</th>
                                    <th>Schedule Date</th>
                                    <th>Schedule Status</th>
                                    <th>HMI Updated TS</th>
                                    <th>Rev. No.</th>
                                    <th>Delete</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                    <asp:TextBox runat="server" ID="txtPriority" Text='<%# Eval("Priority") %>' ReadOnly='<%# Eval("IsReadOnly") %>' CssClass="Contents form-control allowNumber" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("PartNumber") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblJobNumber" Text='<%# Eval("JobNumber") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPlannedQty" Text='<%# Eval("PlannedQuantity") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblActualQty" Text='<%# Eval("ActualQty") %>' CssClass="Contents"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLotCode" Text='<%# Eval("LotCode") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOperationNo" Text='<%# Eval("OperationNo") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblScheduleDate" Text='<%# Eval("ScheduleDate") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblScheduleStatus" Text='<%# Eval("ScheduleStatus") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblHmiUpdatedTs" Text='<%# Eval("HmiUpdatedTs") %>' CssClass="Contents" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRevNo" Text='<%# Eval("RevNo") %>' CssClass="Contents form-control RevNo RevNoCharRest" ReadOnly='<%# Eval("IsReadOnly") %>' ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:CheckBox runat="server" ID="chkDelete" Enabled='<%# Eval("IsDelete") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table class="table table-bordered HeaderFixer tblScheduleStatus" id="tblScheduleStatus" style="width: 100%;">
                                <tr style="position: sticky; top: -1px; z-index: 2;">
                                    <th>Machine Name</th>
                                    <th>Priority</th>
                                    <th>PML Part Number</th>
                                    <th>Job Traveller Number</th>
                                    <th>Planned Qty</th>
                                    <th>Lot Code</th>
                                    <th>Opeartion Number</th>
                                    <th>Schedule Date</th>
                                    <th>Schedule Status</th>
                                    <th>HMI Updated TS</th>
                                    <th>Rev. No</th>
                                </tr>
                                <tr>
                                    <td colspan="11" style="text-align: center;">
                                        <asp:Label runat="server" ID="lblNoData" Text="No Data Found" ForeColor="Black" Font-Size="Medium" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal infoModal" id="newEditModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 950px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="newEditModalTitle" runat="server">Add Schedule</h4>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                        <table style="width: 100%; margin: auto" class="modal-tbl addScheduleTable">
                            <tr>
                                <td style="padding-left: 20px">Machine ID</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMachineIDNew" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td style="padding-left: 20px">Part Number</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPartNumber" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 20px">Lot Code</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtLotCode" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td style="padding-left: 20px">Job Traveller Number</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtJobNo" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 20px">Operation Number</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOperationNo" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                    <%--<asp:DropDownList runat="server" ID="ddlOperationNo" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>--%>
                                </td>
                                <td style="padding-left: 20px">Schedule Date</td>
                                <td class="input-group" style="min-width: 130px; border: 0">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtScheduleDate" runat="server" ClientIDMode="Static" CssClass="form-control date-control" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 20px">Priority</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPriority" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td style="padding-left: 20px">Planned Quantity</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPlannedQty" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 20px">Revision No.</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRevID" CssClass="form-control RevNoCharRest" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseNewEditModal();">CANCEL</button>
                    <asp:Button runat="server" ID="btnNewSave" ClientIDMode="Static" Text="SAVE" CssClass="btn btn-primary btn-style" OnClientClick="return SaveValidation();" OnClick="btnNewSave_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="openWarningModal1" role="dialog" style="z-index: 2000">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content modalContent warning-modal-content">
                <div class="modal-header modalHeader warning-modal-header">
                    <i class="glyphicon glyphicon-warning-sign modal-icons"></i>
                    <br />
                    <h4 class="warning-modal-title">Warning</h4>
                    <br />
                    <div class="warning-modal-msg">
                        <label id="lblWarningMessage">..</label>
                    </div>
                </div>
                <div class="modal-footer modalFooter modal-footer" style="margin-top: 0px;">
                    <input type="button" value="OK" class="warning-modal-btn" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            setDateControl();
        });
        $('#tblScheduleStatus tr td .allowNumber').focus(function () {
            $(this).closest("tr").find("#hdnUpdate").val("update");
        });
        $('#tblScheduleStatus tr td .RevNo').focus(function () {
            $(this).closest("tr").find("#hdnUpdate").val("update");
        });

        $('.RevNoCharRest').keypress(function (evt) {
            if ($(this).val() == undefined) {
                $(this).val() = "0";
            }
            if ($(this).val().length >= 4) {
                return false;
            }
            else {
                return true;
            }
        });

        function SaveValidation() {
            if ($("#txtPartNumber").val() == "") {
                $("#lblWarningMessage").text("Component Id Cannot be Empty.");
                $("#openWarningModal1").modal('show');
                return false;
            }
            if ($("#txtLotCode").val() == "") {
                $("#lblWarningMessage").text("Lot Code Cannot be Empty.");
                $("#openWarningModal1").modal('show');
                return false;
            }
            if ($("#txtJobNo").val() == "") {
                $("#lblWarningMessage").text("Job No. Cannot be Empty.");
                $("#openWarningModal1").modal('show');
                return false;
            }
            if ($("#txtOperationNo").val() == "") {
                $("#lblWarningMessage").text("Operation No. Cannot be Empty.");
                $("#openWarningModal1").modal('show');
                return false;
            }
            if ($("#txtScheduleDate").val() == "") {
                $("#lblWarningMessage").text("Schedule Date Cannot be Empty.");
                $("#openWarningModal1").modal('show');
                return false;
            }
            return true;
        }

        $('.allowNumber').keypress(function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        });

        function setDateControl() {
            $('.date-control').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US',
            });
            $(".Year").datepicker({
                format: 'yyyy',
                viewMode: "years",
                minViewMode: "years",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $(".Month").datepicker({
                format: 'mm',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            })
        }
        function OpenNewEditModal() {
            debugger;
            if ($("#ddlMachineID").val() != "All" && $("#ddlMachineID").val() != "")
                $("#ddlMachineIDNew").val($("#ddlMachineID").val());
            if ($("#ddlPartNumber").val() != "All" && $("#ddlPartNumber").val() != "")
                $("#txtPartNumber").val($("#ddlPartNumber").val());

            $("#txtPartNumber").val("");
            $("#txtLotCode").val("");
            $("#txtJobNo").val("");
            $("#txtOperationNo").val("");
            $("#txtScheduleDate").val("");
            $("#txtPriority").val("");
            $("#txtPlannedQty").val("");
            $("#txtRevID").val("");
           
            $("#newEditModal").show();
            return false;
        }

        function CloseNewEditModal() {
            $("#newEditModal").hide();
            return false;
        }

        function openWarningModalMsg(errorMsg) {
            $("#lblWarningMessage").text(errorMsg.replaceAll("@", "\n"));
            $("#openWarningModal1").modal('show');
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setDateControl();
            $('#tblScheduleStatus tr td .allowNumber').focus(function () {
                $(this).closest("tr").find("#hdnUpdate").val("update");
            });

            $('#tblScheduleStatus tr td .RevNo').focus(function () {
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


            $('.RevNoCharRest').keypress(function (evt) {
                if ($(this).val() == undefined) {
                    $(this).val() = "0";
                }
                $(this).val((Number($(this).val()) + 1).toString());
                if (Number($(this).val()) > 4) {
                    return false;
                }
                else {
                    return true;
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
