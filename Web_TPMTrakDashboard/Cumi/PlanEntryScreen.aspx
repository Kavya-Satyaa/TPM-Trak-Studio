<%@ Page Title="Plan Entry Screen" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlanEntryScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.PlanEntryScreen" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>

    <script src="Scripts/SearchableDropDown/select2.min.js"></script>
    <link href="Scripts/SearchableDropDown/select2.min.css" rel="stylesheet" />

    <style>
        /* #tblPlanDetails tr th:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 5;
            background-color: #2e6886;
            color: white;
        }

        #tblPlanDetails tr:nth-child(odd) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 2;
            background-color: #DCDCDC;
        }

        #tblPlanDetails tr:nth-child(even) td:nth-child(2) {
            position: sticky;
            left: 0px;
            z-index: 2;
            background-color: #FFFFFF;
        }*/
        .cumi-tbl-details tr td {
            background-color: white;
        }

        .add-row-icon {
            color: blue;
            font-size: 17px;
        }

        .remove-row-icon {
            color: red;
            font-size: 17px;
        }

        .bajaj-add-edit-btn-style:hover, .bajaj-add-edit-btn-style:focus {
            color: white;
        }

        .add-plan-desc {
            overflow: hidden;
            min-width: 250px;
            max-width: 250px;
            word-wrap: break-word;
        }
    </style>
    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
                <asp:PostBackTrigger ControlID="btnExportItemStd" />
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnModalValues" ClientIDMode="Static" />
                <div style="width: 100%;">
                    <div style="display: inline-block">
                        <div class="bajaj-outer-div-filter-section">
                            <div class="bajaj-inner-div-filter-section left-content-filter-section">
                                <table class="bajaj-filter-tbl">
                                    <tr>
                                        <td>Machine</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                        <td>From Date</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                        <td>To Date</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" />

                                            <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style" Style="background-color: #9f0e9f; color: white" OnClick="btnExport_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="bajaj-outer-div-filter-section" style="margin-top: 4px">
                            <div class="bajaj-inner-div-filter-section left-content-filter-section">
                                <table class="bajaj-filter-tbl">
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnAddPlanDetails" Text="Add Plan" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" OnClick="btnAddPlanDetails_Click" />

                                        </td>
                                        <td>
                                            <asp:FileUpload runat="server" ID="fuImport" CssClass="form-control" Style="width: 220px" />

                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnImport" Text="Import" OnClick="btnImport_Click" CssClass="bajaj-btn-style" />
                                            <asp:LinkButton runat="server" ID="btnDownloadImportTemplate" OnClick="btnDownloadImportTemplate_Click" ToolTip="Download Format" CssClass="bajaj-btn-style bajaj-add-edit-btn-style">
                                        <span>Download Format</span>
                                        <i class="glyphicon glyphicon-download" style=" font-size: 19px; vertical-align: sub;"></i>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: right; width: 22%">
                        <div>
                             <table class="table table-bordered table-hover headerFixer cumi-tbl-details">
                                <tr>
                                    <th colspan="3">Item Std. Cycle Time</th>
                                </tr>
                                <tr>
                                    <td>New Entries</td>
                                    <td runat="server" id="tdNewEnytriesCount">
                                        <asp:Label runat="server" ID="lblNewEntries"></asp:Label>
                                    </td>
                                    <td rowspan="3" style="vertical-align: middle; padding: 0px; text-align: center;width: 100px;">
                                        <asp:LinkButton runat="server" ID="lblItemStdCycleTimeAction" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" Text="Action" OnClick="lbNewEntries_Click"></asp:LinkButton>
                                        <br />
                                        <asp:Button runat="server" ID="btnExportItemStd" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" Text="Export" OnClick="btnExportItemStd_Click" Style="margin-top: 11px;"></asp:Button>
                                        <br />
                                        <asp:Button runat="server" ID="btnPOScreen" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" Text="Action"  OnClientClick="return goToPOScreen();" Style="margin-top: 6px"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Entries</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblTotalEntries"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>PO Entries</td>
                                    <td>
                                        <asp:Label runat="server" ID="lblPOCount"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="3">Last Sync DateTime</th>
                                </tr>
                                <tr>
                                    <td>Production Order</td>
                                    <td colspan="2">
                                        <asp:Label ID="lblPONumber" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>DateTime</td>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lblDateTime"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="width: 100%">
                    <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px; width: 62%; display: inline-block">
                        <asp:ListView runat="server" ID="lvPlanDetails" ClientIDMode="Static">
                            <EmptyDataTemplate>
                                <table class="table table-bordered table-hover headerFixer cumi-tbl-details" id="tblPlanDetails">
                                    <tr>
                                        <th>Machine ID</th>
                                        <th>Date</th>
                                        <th>Shift</th>
                                        <th>PO No.</th>
                                        <th>Item Code</th>
                                        <th>Plan</th>
                                        <th runat="server" id="thAction">Action</th>
                                    </tr>
                                    <tr>
                                        <td colspan="30" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblPlanDetails">
                                    <tr>
                                        <th>Machine ID</th>
                                        <th>Date</th>
                                        <th>Shift</th>
                                        <th>PO No.</th>
                                        <th>Item Code</th>
                                        <th>Plan</th>
                                        <th runat="server" id="thAction">Action</th>
                                    </tr>
                                    <tr id="itemplaceholder" runat="server"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblDate" Text='<%# Eval("Date") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblShift" Text='<%# Eval("Shift") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hfAutoID" Value='<%# Eval("IDD") %>' />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblPONo" Text='<%# Eval("PONo") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblItemCode" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblPlanValue" Text='<%# Eval("PlanValue") %>'></asp:Label>
                                    </td>
                                    <td style="white-space: nowrap" runat="server">
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


                </div>

                <div class="modal infoModal bajaj-info-modal" id="neweditPlanModal" role="dialog" style="min-width: 500px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 1200px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title" id="modalTitle" runat="server"></h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('neweditPlanModal','compare');"></i>
                                <asp:HiddenField runat="server" ID="hfPlanNewEdit" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px; max-height: 80vh; overflow: auto">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Machine ID *</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlMachineID" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hfAutoID" />
                                            </td>
                                            <td>Date *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDate" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Shift *</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlShift" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                            </td>
                                            <td id="tdPOLabel" runat="server">PO No. *</td>
                                            <td id="tdPOControl" runat="server">
                                                <asp:TextBox runat="server" ID="txtPONo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trItemPlan" runat="server">
                                            <td>Item Code *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItemCode" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Plan *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPlanValue" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowDecimalWithOperator"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div runat="server" id="scanDiv">

                                        <%--   <div>
                                            <asp:RadioButtonList runat="server" ID="rblEntryType" ClientIDMode="Static" RepeatDirection="Horizontal" CssClass="check-box-list" onchange="CheckEntryType();">
                                                <asp:ListItem Text="Scan" Value="Scan"></asp:ListItem>
                                                <asp:ListItem Text="Manual" Value="Manual"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>--%>

                                        <%--   <asp:TextBox runat="server" ID="txtScan" CssClass="form-control" ClientIDMode="Static" onkeypress="ScanKeyPressFunction(event);" onkeydown="ScanKeyDownFunction(event);" placeholder="Scan Here"></asp:TextBox>--%>

                                        <div style="margin-top: 12px;">
                                            <div id="divAddPlanTable" style="display: inline-block; width: 90%;">
                                                <table id="tblAddPlanDetails" class="table table-bordered headerFixer table-hover bajaj-table-style">
                                                    <tr>
                                                        <th style="min-width: 250px; max-width: 250px; width: 250px">PO No.</th>
                                                        <th style="min-width: 200px; max-width: 200px; width: 200px">Item Code</th>
                                                        <th>Description</th>
                                                        <th style="min-width: 150px; max-width: 150px; width: 150px">Plan</th>
                                                        <th>Action</th>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divAddRows" style="display: inline-block; position: relative; bottom: 30px; left: 10px; width: 9%;">
                                                <i class="glyphicon glyphicon-plus-sign add-row-icon" onclick="AddRowToPlanDetails(1);"></i>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('neweditPlanModal','compare');" class="bajaj-btn-style cancel-btn">CANCEL</button>
                                <asp:Button runat="server" ID="btnPlanDetailsSave" ClientIDMode="Static" Text="Save" CssClass="bajaj-btn-style   bajaj-add-edit-btn-style" OnClientClick="return PlanValidation();" OnClick="btnPlanDetailsSave_Click" />
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


                <div class="modal infoModal bajaj-info-modal" id="importModal" role="dialog" style="min-width: 500px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 900px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title">Please correct the below points</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="$('#importModal').modal('hide');"></i>
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px; max-height: 80vh; overflow: auto">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <span runat="server" id="lblInvalidRecords" style="color: black"></span>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="$('#importModal').modal('hide');" class="bajaj-btn-style  bajaj-add-edit-btn-style">OK</button>
                            </div>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnDownloadImportTemplate" />
                <asp:PostBackTrigger ControlID="btnImport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            DateTimeSetter();
        });
        function goToPOScreen() {
            window.open("POScreenCumi.aspx", "PO Screen");
            return false;
        }
        function DateTimeSetter() {

            $('[id$=txtDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
            $('[id$=txtFromDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
            $('[id$=txtToDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
            $(".searchable-dropdown-list").select2({
                placeholder: "Search",
                allowClear: true,
                //dropdownAutoWidth: true,
                width: '100%'
            });
        }
        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('deleteConfirmModal');
        }
        function PlanValidation() {
            if ($('#ddlMachineID').val() == "" || $('#ddlMachineID').val() == null) {
                toasterWarningMsg("Machine ID is required.", "");
                return false;
            }
            if ($('#txtDate').val() == "") {
                toasterWarningMsg("Date is required.", "");
                return false;
            }
            if ($('#ddlShift').val() == "" || $('#ddlShift').val() == null) {
                toasterWarningMsg("Shift is required.", "");
                return false;
            }

            if ($("#hfPlanNewEdit").val() == "New") {

                let isFieldEntered = false;
                let type = "";
                for (let i = 1; i < $("#tblAddPlanDetails tr").length; i++) {
                    let tr = $("#tblAddPlanDetails tr")[i];
                    //let poNo = ($(tr).find("#hfManualOrScannedRow").val() == "1" ? $(tr).find("#lblPONo").val() : $(tr).find("#lblPONo").text());
                    //let itemCode = ($(tr).find("#hfManualOrScannedRow").val() == "1" ? $(tr).find("#lblItemCode").val() : $(tr).find("#lblItemCode").text());
                    //let plan = ($(tr).find("#hfManualOrScannedRow").val() == "1" ? $(tr).find("#lblPlan").val() : $(tr).find("#lblPlan").text());
                    let poNo = $(tr).find("[id$=ddlPO]").val().trim();
                    let itemCode = $(tr).find("#ddlItemCode :selected").text().trim();
                    let plan = $(tr).find("#txtPlan").val().trim();
                    //if (poNo == "" && itemCode == "" && plan == "") {
                    //    continue;
                    //}

                    debugger;
                    if ((poNo == "" && itemCode == "" && plan == "")) {
                        continue;
                    }
                    if ((poNo != "" && (itemCode == "" || plan == "")) || (itemCode != "" && (poNo == "" || plan == "")) || ((poNo == "" || itemCode == "") && plan != "")) {
                        isFieldEntered = true;
                        break;
                    }

                    if ($(tr).find("#ddlItemCode").val().split(';;;')[1] != "1") {
                        isFieldEntered = true;
                        type = itemCode;
                        break;
                    }

                    //if ((poNo == "" && itemCode == "" && plan == "")) {
                    //    isFieldEntered = false;
                    //    break;
                    //}
                }
                if (isFieldEntered) {
                    if (type != "") {
                        toasterWarningMsg("Item Code: " + itemCode + " not in master table.", "");
                    } else {
                        toasterWarningMsg("PO No., Item Code and Plan are required.", "");
                    }

                    return false;
                }

            } else {
                if ($('#txtPONo').val() == "") {
                    toasterWarningMsg("PO No. is required.", "");
                    return false;
                }
                if ($('#txtItemCode').val() == "") {
                    toasterWarningMsg("Item Code is required.", "");
                    return false;
                }
                if ($('#txtPlanValue').val() == "") {
                    toasterWarningMsg("Plan is required.", "");
                    return false;
                }
            }

            var PlanDetails = {};
            let anyError = "";

            if ($("#hfPlanNewEdit").val() == "New") {
                for (let i = 1; i < $("#tblAddPlanDetails tr").length; i++) {
                    let tr = $("#tblAddPlanDetails tr")[i];
                    //let poNo = ($(tr).find("#hfManualOrScannedRow").val() == "1" ? $(tr).find("#lblPONo").val() : $(tr).find("#lblPONo").text());
                    //let itemCode = ($(tr).find("#hfManualOrScannedRow").val() == "1" ? $(tr).find("#lblItemCode").val() : $(tr).find("#lblItemCode").text());
                    //// let desc = ($(tr).find("#hfManualOrScannedRow").val() == "1" ? $(tr).find("#lblDesc").val() : $(tr).find("#lblDesc").text());
                    //let plan = ($(tr).find("#hfManualOrScannedRow").val() == "1" ? $(tr).find("#lblPlan").val() : $(tr).find("#lblPlan").text());
                    let poNo = $(tr).find("[id$=ddlPO]").val();
                    let itemCode = $(tr).find("#ddlItemCode :selected").text();
                    let plan = $(tr).find("#txtPlan").val();

                    if (poNo == "" || (itemCode == "" || itemCode == null) || plan == "") {
                        continue;
                    }

                    PlanDetails = {
                        MachineID: $('#ddlMachineID').val(),
                        Date: $('#txtDate').val(),
                        Shift: $('#ddlShift').val(),
                        PONo: poNo,
                        ItemCode: itemCode,
                        //Description=desc,
                        PlanValue: plan,
                        NewOrEdit: $("#hfPlanNewEdit").val()
                    };

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "PlanEntryScreen.aspx/SavePlanDetails",
                        contentType: "application/json; charset=utf-8",
                        crossDomain: true,
                        dataType: "json",
                        data: JSON.stringify({
                            planDetails: PlanDetails
                        }),
                        success: function (response) {
                            let dataitem = response.d;
                            if (dataitem == "Error") {
                                anyError = i;

                            }
                        },
                        error: function (Result) {
                        }
                    });
                    if (anyError != "") {
                        break;
                    }
                }

            } else {
                PlanDetails = {
                    MachineID: $('#ddlMachineID').val(),
                    Date: $('#txtDate').val(),
                    Shift: $('#ddlShift').val(),
                    PONo: $('#txtPONo').val(),
                    ItemCode: $('#txtItemCode').val(),
                    //Description="",
                    PlanValue: $('#txtPlanValue').val(),
                    NewOrEdit: $("#hfPlanNewEdit").val()
                };

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "PlanEntryScreen.aspx/SavePlanDetails",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: JSON.stringify({
                        planDetails: PlanDetails
                    }),
                    success: function (response) {
                        let dataitem = response.d;
                        if (dataitem == "Error") {
                            anyError = "Error";
                        }
                    },
                    error: function (Result) {
                    }
                });
            }
            if (anyError == "") {
                return true;

            } else {
                toasterWarningMsg("Failed to save record.", "");
                return false;
            }
        }


        //var dataCaptureInterval;
        //var enterCondition = 0;
        //var poScanDisplayOnce = 0;
        //var timeout = 3000;
        //var lastDataCapturedDateTime = new Date();
        //var flag = 0;
        //function ScanKeyPressFunction(e) {
        //    poScanDisplayOnce = 1;
        //    if (e.keyCode == 13) {
        //        if (enterCondition == 1) // if manuallly entry and enterd 'Enter' key
        //        {
        //            clearTimeout(dataCaptureInterval);
        //            enterCondition = 0;
        //            BindPlanDetails();
        //            e.preventDefault();
        //        }
        //        else { // scanning entry uf conatins
        //            e.preventDefault();
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //function ScanKeyDownFunction(e) {
        //    if (flag == 1) // to avoid open new window
        //    {
        //        e.preventDefault();
        //        return false;
        //    }
        //    poScanDisplayOnce = 1;
        //    var timeDiff;
        //    timeout = 3000;
        //    clearTimeout(dataCaptureInterval);
        //    let currentDateTime = new Date();
        //    if (lastDataCapturedDateTime != undefined) {
        //        timeDiff = currentDateTime.getTime() - lastDataCapturedDateTime.getTime();
        //        if (timeDiff <= 100) {
        //            timeout = 1000; // scanning
        //        }
        //        else {
        //            timeout = 3000; // give some time to entry next characters
        //            enterCondition = 1; // entry
        //        }
        //    }
        //    lastDataCapturedDateTime = new Date();
        //    dataCaptureInterval = setTimeout(BindPlanDetails, timeout);
        //    if (e.keyCode == 13) {
        //        flag = 1;
        //        if (enterCondition == 0) {
        //            clearTimeout(dataCaptureInterval);
        //            dataCaptureInterval = setTimeout(BindPlanDetails, timeout);
        //            e.preventDefault();
        //        }

        //    }
        //}

        //var dataCaptureInterval;
        //var enterCondition = 0;
        //var poScanDisplayOnce = 0;
        //var timeout = 3000;
        //var lastDataCapturedDateTime = new Date();
        //var flag = 0;
        //function ScanKeyPressFunction(e) {
        //    poScanDisplayOnce = 1;
        //    if (e.keyCode == 13) {
        //        if (enterCondition == 1) // if manuallly entry and enterd 'Enter' key
        //        {

        //            e.preventDefault();
        //        }
        //        else { // scanning entry uf conatins
        //            e.preventDefault();
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //function ScanKeyDownFunction(e) {

        //    if (e.keyCode == 13) {
        //        e.preventDefault();
        //    }
        //}

        //function Scanning() {
        //    if ($("#neweditPlanModal").hasClass('in')) {
        //        if ($("#hfPlanNewEdit").val() == "New") {
        //            if ($(':focus').length > 0) {
        //                alert("remove");
        //                //document.removeEventListener("keypress");
        //            } else {
        //                //document.addEventListener("keypress", captureMitutoyoData);
        //                alert("add");
        //            }
        //        }
        //    }

        //}
        //Scanning();

        //var ScannerInput = "";
        //var ClearInputData = 0;
        //var LastDataCapturedDateTime = new Date();

        function CheckEntryType() {
            let entryType = $("#rblEntryType input:checked").val();
            $("#divAddPlanTable").css({ "width": "100%", "display": "none" });
            $("#divAddRows").css({ "width": "0%", "display": "none" });
            //if (entryType == "Manual") {
            // document.removeEventListener("keypress", CaptureScannedData);
            $("#divAddPlanTable").css({ "width": "90%", "display": "inline-block" });
            $("#divAddRows").css({ "width": "9%", "display": "inline-block" });
            //} else if (entryType == "Scan") {
            //    $("#divAddPlanTable").css({ "width": "100%", "display": "inline-block" });
            //    $("#divAddRows").css({ "width": "0%", "display": "none" });
            //ScannerInput = "";
            //ClearInputData = 0;
            //LastDataCapturedDateTime = new Date();
            //document.addEventListener("keypress", CaptureScannedData);
            //  }

            AddRowToPlanDetails(5);
        }


        //function CaptureScannedData(e) {
        //    //console.log(e.key);

        //    let currentDateTime = new Date();
        //    if ((currentDateTime.getTime() - LastDataCapturedDateTime.getTime()) > 100) {
        //        console.log((currentDateTime.getTime() - LastDataCapturedDateTime.getTime()));
        //        ScannerInput = "";
        //    }
        //    if (e.keyCode != 13) {
        //        ScannerInput += e.key;
        //       // console.log("Mitutoyo = " + ScannerInput);
        //    }
        //    LastDataCapturedDateTime = new Date();
        //    ClearInputData = window.setTimeout(function () {
        //        if (ScannerInput != "" && ScannerInput.length > 1) {
        //            BindPlanDetails(ScannerInput);
        //            ScannerInput = "";
        //        }
        //    }, 700);

        //    if (e.keyCode == 13) {
        //        e.preventDefault();
        //        return;
        //    }
        //    e.preventDefault();
        //}

        //function BindPlanDetails(scannedInput) {
        //    let scannedValue = scannedInput;
        //    if (scannedValue.split(";;").length < 4) {
        //        toasterWarningMsg("Problen in Scanned Value.", "");
        //        return;
        //    }
        //    let poNo = scannedValue.split(";;")[0];
        //    let itemCode = scannedValue.split(";;")[1];
        //    let desc = scannedValue.split(";;")[2];
        //    let plan = scannedValue.split(";;")[3];
        //    let tr = "<tr><td><span id='lblPONo'>" + poNo + "</span></td><td><span id='lblItemCode'>" + itemCode + "</span></td><td class='add-plan-desc' ><span id='lblDesc'>" + desc + "</span></td><td><span  id='lblPlan'>" + plan + "</span></td><td><i class='glyphicon glyphicon-remove-circle remove-row-icon' onclick='RemoveRowFromPlanDetails(this);'></i></td></tr>";
        //    $("#tblAddPlanDetails").append(tr);
        //}
        function AddRowToPlanDetails(noOfRowsToAdd) {
            /*   let tr = "<tr><td><input type='text' id='lblPONo' class='form-control' /><input type='hidden' id='hfManualOrScannedRow' value='1' /></td><td><input type='text' id='lblItemCode' class='form-control' /></td><td><span  id='lblDesc'></span></td><td><input type='text' id='lblPlan' class='form-control allowDecimalWithOperator' /></td><td>  <i class='glyphicon glyphicon-remove-circle remove-row-icon' onclick='RemoveRowFromPlanDetails(this);'></i></td></tr>";*/


            let erpPODetails = [];
            $.ajax({
                async: false,
                type: "POST",
                url: "PlanEntryScreen.aspx/GetERPPODetails",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: {},
                success: function (response) {
                    erpPODetails = response.d;

                },
                error: function (Result) {
                }
            });
            let POdropDown = '<select name="ddlPO" id="ddlPO" class=" form-control searchable-dropdown-list" onchange="POChange(this);" >';

            for (let i = 0; i < erpPODetails.length; i++) {
                if (i == 0) {
                    POdropDown += '<option value=""> </option>';
                }
                POdropDown += '<option value="' + erpPODetails[i].Value + '">' + erpPODetails[i].Text + '</option>';
            }
            POdropDown += '</select > ';

            //let componentDetails = [];
            //$.ajax({
            //    async: false,
            //    type: "POST",
            //    url: "PlanEntryScreen.aspx/GetComponentDetails",
            //    contentType: "application/json; charset=utf-8",
            //    crossDomain: true,
            //    dataType: "json",
            //    data: {},
            //    success: function (response) {
            //        componentDetails = response.d;

            //    },
            //    error: function (Result) {
            //    }
            //});

            let dropDown = '<select name="ddlItemCode" id="ddlItemCode" class=" form-control ddl-item-code" onchange="ItemCodeChange(this);" >';

            //for (let i = 0; i < componentDetails.length; i++) {
            //    if (i == 0) {
            //        dropDown += '<option value=""> </option>';
            //    }
            //    dropDown += '<option value="' + componentDetails[i].Value + '">' + componentDetails[i].Text + '</option>';
            //}
            dropDown += '</select > ';

            for (let i = 0; i < noOfRowsToAdd; i++) {

                //<input type='text' id='txtPONo' class='form-control' onkeypress='ScanKeyPressFunction(event);' onkeydown='ScanKeyDownFunction(event);' />
                debugger;
                POdropDown = POdropDown.replaceAll("ddlPO", ($("#tblAddPlanDetails tr").length + Math.random() + 1) + "ddlPO");
                let tr = "<tr><td>" + POdropDown + "<input type='hidden' id='hfManualOrScannedRow' value='1' /></td><td>" + dropDown + "</td><td  class='add-plan-desc'><span  id='lblDesc' style='vertical-align: -webkit-baseline-middle;' ></span></td><td><input type='text' id='txtPlan' class='form-control allowDecimalWithOperator' /></td><td>  <i class='glyphicon glyphicon-remove-circle remove-row-icon' onclick='RemoveRowFromPlanDetails(this);'></i></td></tr>";

                $("#tblAddPlanDetails").append(tr);


            }
            DateTimeSetter();
        }
        function RemoveRowFromPlanDetails(element) {
            $(element).closest("tr").remove();
        }
        function ItemCodeChange(element) {
            $(element).closest("tr").find("#lblDesc").text($(element).val().split(';;;')[0]);
        }
        function POChange(element) {
            // $(element).closest("tr").find("#lblDesc").text($(element).val());

            let componentDetails = [];
            $.ajax({
                async: false,
                type: "POST",
                url: "PlanEntryScreen.aspx/GetERPItemDetails",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: '{po:"' + $(element).val() + '"}',
                success: function (response) {
                    componentDetails = response.d;

                },
                error: function (Result) {
                }
            });

            let dropDown = '';

            for (let i = 0; i < componentDetails.length; i++) {
                if (i == 0) {
                    dropDown += '<option value=""> </option>';
                }
                dropDown += '<option value="' + componentDetails[i].Value + '">' + componentDetails[i].Text + '</option>';
            }
            $(element).closest("tr").find("#ddlItemCode").append(dropDown);
        }


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                DateTimeSetter();
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
