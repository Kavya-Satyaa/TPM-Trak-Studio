<%@ Page Language="C#" Title="Production Scheduler" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="ProductionScheduler.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.ProductionScheduler" EnableEventValidation="false" %>

<asp:Content ID="MainContentArea" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style type="text/css">
        .header-center {
            text-align: center;
        }

        .form-control {
            height: 35px;
            height: 35px;
        }

        .btn-default {
            height: 35px;
        }

        .Header {
            color: White;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 13px;
            font-weight: bold;
            text-align: center;
            overflow-wrap: break-word;
        }

        .Pos {
            margin-right: -5px;
            margin-left: -5px;
        }

        .Row {
            display: table;
            border-spacing: 5pt;
            width: 100%;
        }

        .Col {
            display: table-cell;
            height: 50px;
            width: 100%;
            border: 1pt solid black;
            background-color: #DBDBDB;
        }

        .MiddleLeft {
            text-align: left;
            align-items: normal;
            vertical-align: middle;
        }

        .textCenter {
            vertical-align: middle;
            text-align: center;
        }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }

        .textboxedit {
            border: 1px solid #B2B2B2;
            background: white;
            color: black;
        }

        .form-control {
            width: 98%;
        }

        #MainContent_lstProdScheduler_tblProdScheduler tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_lstProdScheduler_tblProdScheduler tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }

        #MainContent_lstProdScheduler_trProdScheduleHeader th, #MainContent_lstProdScheduler_tdSchedulePager {
            color: white;
            background-color: #2E6886 !important;
        }


        #MainContent_lvAssemblyDetails_tblAssemblyDetails tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        #MainContent_lvAssemblyDetails_tblAssemblyDetails tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }

        #MainContent_lvAssemblyDetails_trAssemblyDetailsHeader th, #MainContent_lvAssemblyDetails_tdSchedulePager {
            color: white;
            background-color: #2E6886 !important;
        }

        .grid-position-fixed tr th:last-child {
            position: sticky;
            right: 0px;
        }

        .grid-position-fixed tr:nth-child(odd) td:last-child {
            position: sticky;
            right: 0px;
            background-color: #DCDCDC;
            color: black;
        }

        .grid-position-fixed tr:nth-child(even) td:last-child {
            position: sticky;
            right: 0px;
            background-color: #FFFFFF;
            color: black;
        }

        .grid-position-fixed tr th:first-child {
            position: sticky;
            left: -1px;
            z-index: 2;
        }

        .grid-position-fixed tr:nth-child(odd) td:first-child {
            position: sticky;
            left: 0px;
            background-color: #DCDCDC;
            color: black;
        }

        .grid-position-fixed tr:nth-child(even) td:first-child {
            position: sticky;
            left: 0px;
            background-color: #FFFFFF;
            color: black;
        }


        .popupTableStyles tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
            vertical-align: middle;
            min-width: 80px;
            text-align: center;
        }

        .popupTableStyles tr td {
            color: black;
            vertical-align: middle;
            text-align: center;
            border: 1px solid #9A9A9A;
        }

        .modal-body {
            color: black;
        }

        .DataOperations {
            bottom: 0;
            right: 0;
            float: right;
            margin-right: 5px;
            min-height: 40px;
            position: fixed;
            width: auto;
        }

        #btnCalculatePlan, #btnExport {
            bottom: 0;
            position: fixed;
        }

        #btnExport {
            margin-left: 120px;
            float: left;
            display: inline;
            margin-bottom: 3px;
        }

        .btnStyle {
            margin-left: 2px;
            margin-right: 2px;
        }

        fieldset {
            border: 1px solid black;
            margin: 2px;
        }

        legend {
            font-size: 14px;
            margin-left: 10px;
            margin-bottom: 0px;
            width: 160px;
            font-weight: 900;
            padding-left: 4px;
        }

        #divStartDate {
            min-width: 200px;
            border-spacing: 0px;
            margin: 5px;
        }
    </style>

    <div class="container-fluid" style="margin-left: 5px;">
        <asp:UpdatePanel ID="UpdatePanelProdScheduler" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="Pos" style="height: 110px;">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","MachineId") %></td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" meta:resourcekey="ddlMachineIdResource1" AutoPostBack="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged" />
                            </td>

                            <td class="commanTd" style="width: 70px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","Status") %></td>
                            <td style="width: 230px;">
                                <asp:ListBox ID="lstStatus" runat="server" SelectionMode="Multiple" Height="30px" Style="width: 220px;">
                                    <asp:ListItem Text="New" Value="1" Selected="True" />
                                    <asp:ListItem Text="Running" Value="2" Selected="True" />
                                    <asp:ListItem Text="Parked" Value="3" Selected="True" />
                                    <asp:ListItem Text="Completed" Value="4" Selected="False" />
                                    <asp:ListItem Text="Pending Inspection Completion" Value="5" Selected="False" />
                                </asp:ListBox>
                            </td>
                            <td class="commanTd" style="width: 100px; height: 50px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","FromDate") %></td>
                            <td class="input-group" style="min-width: 200px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Style="min-width: 130px;" CssClass="form-control date" placeholder="YYYY-MM-DD HH:mm:ss" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="width: 90px; height: 50px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","ToDate") %></td>
                            <td class="input-group" style="min-width: 200px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" Style="min-width: 130px;" CssClass="form-control date" placeholder="YYYY-MM-DD HH:mm:ss" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","Search") %></td>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control" placeholder="search here..." />
                            </td>
                            <td style="vertical-align: middle; min-width: 300px;" colspan="2">
                                <asp:RadioButton runat="server" ID="radioPoNumber" GroupName="SearchScheduler" Text="&nbsp;Prod Order Num" TextAlign="Right" CssClass="form-control" Style="display: inline; vertical-align: middle; padding-left: 2px;" ClientIDMode="Static" Checked="true"></asp:RadioButton>
                                <asp:RadioButton runat="server" ID="radioMaterialId" GroupName="SearchScheduler" Text="&nbsp;&nbsp;Material ID" CssClass="form-control" Style="display: inline; margin-left: 5px; padding-left: 2px;" ClientIDMode="Static" TextAlign="Right" />
                            </td>
                            <td style="width: 100px;">
                                <asp:Button runat="server" Text="View" ID="btnView" CssClass="btn btn-primary btnStyle" Style="width: 80px;" OnClick="btnView_Click" OnClientClick="return showLoader();" />
                            </td>
                            <td>
                                <asp:FileUpload data-toggle="tooltip" ForeColor="Black" CssClass="form-control toolTip" ID="FileUploadSchedule" runat="server" ClientIDMode="Static" AllowMultiple="false" Style="min-width: 200px;" />
                            </td>
                            <td style="width: 90px;">
                                <button type="button" id="btnImport" value="Import" class="btn btn-primary btnStyle" runat="server" clientidmode="static">Import</button>
                            </td>
                            <td>
                                <button type="button" id="btnDownloadTemplate" class="btn btn-primary btnStyle" runat="server" clientidmode="static">
                                    <span class="glyphicon glyphicon-download"></span>&nbsp;Import Template
                                </button>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>

                <div style="overflow-x: auto; overflow-y: auto; height: 70vh" class="Pos">
                    <asp:ListView ID="lstProdScheduler" runat="server" DataKeyNames="IDD" class="table table-bordered table-hover" Style="height: 500px;" OnItemDataBound="lstProdScheduler_ItemDataBound" OnItemEditing="lstProdScheduler_ItemEditing" OnItemCommand="lstProdScheduler_ItemCommand" OnItemCanceling="lstProdScheduler_ItemCanceling" OnItemUpdating="lstProdScheduler_ItemUpdating" OnPagePropertiesChanging="lstProdScheduler_PagePropertiesChanging" OnSorting="lstProdScheduler_Sorting">
                        <LayoutTemplate>
                            <table id="tblProdScheduler" runat="server" class="table table-bordered headerFixer" style="margin-bottom: 0px;">
                                <thead class="blue">
                                    <tr id="trProdScheduleHeader" runat="server">
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 60px;" class="Header">
                                            <span>Action</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 0px; width: 0px;" class="Header">
                                            <span>Priority</span>
                                            <%-- <asp:ImageButton ID="imgBtnSortPriority" CommandArgument="Priority" CommandName="Sort" ImageUrl="~/Image/asc.gif" runat="server" Style="display: inline;" />--%>
                                        </th>

                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 120px; display: none;" class="Header">
                                            <span>User Priority</span>
                                        </th>

                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 120px;" class="Header">
                                            <span>Production Order Number</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 200px;" class="Header">
                                            <span>Material ID</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Model</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Description</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Operation Number</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Quantity</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Standard Cycle Time(min)</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Standard Setup Time(min)</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Scheduled Start Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 125px;" class="Header">
                                            <span>Scheduled Completed Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Revised Scheduled Start Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 125px;" class="Header">
                                            <span>Revised Scheduled Completed Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Actual Start Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Predicted Completion</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Actual End Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header" id="tdGRNNumber" runat="server">
                                            <span>GRN Number</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header" id="tdSupplierName" runat="server">
                                            <span>Supplier Name</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header" id="tdNewProdDev" runat="server">
                                            <span>New Product Development</span>
                                        </th>

                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Status</span>
                                            <asp:ImageButton ID="imgBtnSortStatus" CommandArgument="Status" CommandName="Sort" ImageUrl="~/Image/asc.gif" runat="server" Style="display: inline;" />
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Select All</span>
                                            <asp:CheckBox runat="server" ID="chkHeaderSelect" OnCheckedChanged="chkHeaderSelect_CheckedChanged" AutoPostBack="true" />

                                        </th>
                                    </tr>
                                </thead>
                                <tr id="ItemPlaceholder" runat="server"></tr>
                                <tr id="trSchedulePager" runat="server" style="text-align: center;">
                                    <td colspan="25" id="tdSchedulePager" runat="server">
                                        <asp:DataPager ID="dataPagerSchedule" PageSize="8" runat="server" PagedControlID="lstProdScheduler">
                                            <Fields>
                                                <asp:NextPreviousPagerField ShowLastPageButton="false" ShowNextPageButton="false" ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" />
                                                <asp:NumericPagerField ButtonType="Button" NumericButtonCssClass="btn" CurrentPageLabelCssClass="btn disabled" />
                                                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" />
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:ImageButton runat="server" ID="btnEdit" ToolTip="Edit row" ImageUrl="~/GEA/Icons/edit_icon.png" ImageAlign="AbsMiddle" />
                                </td>
                                <td class="textCenter" style="width: 0px;">
                                    <asp:HiddenField ID="hdfUpdate" runat="server" Value='<%# Bind("IDD") %>' ClientIDMode="Static" />
                                    <asp:Label ID="lblPriority" CssClass="textboxcommon" runat="server" Text='<%# Bind("Priority") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter" style="display: none;">
                                    <asp:Label ID="lblUserPriority" CssClass="textboxcommon" runat="server" Text='<%# Bind("UserPriority") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblProdOrderNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("ProductionOrderNumber") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblMaterialID" CssClass="textboxcommon" runat="server" Text='<%# Bind("MaterialID") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblModel" CssClass="textboxcommon" runat="server" Text='<%# Bind("Model") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblDesc" CssClass="textboxcommon" runat="server" Text='<%# Bind("ModelDescription") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOpnNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("OperationNumber") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblQuantity" CssClass="textboxcommon" runat="server" Text='<%# Bind("Quantity") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStdCycleTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("StdCycleTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStdSetupTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("StdSetupTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOldSchStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("OldScheduledStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOldSchEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("OldScheduledEndTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblSchStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ScheduledStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblSchEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ScheduledEndTime") %>' ClientIDMode="Static" />
                                </td>

                                <td class="textCenter">
                                    <asp:Label ID="lblActualStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ActualStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblPredCompTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("PredictedCompletionTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblActualEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ActualEndTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter" runat="server" id="tdEditGRN">
                                    <asp:Label ID="lblGRNNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("GRNNumber") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter" runat="server" id="tdEditSuppName">
                                    <asp:Label ID="lblSupplierName" CssClass="textboxcommon" runat="server" Text='<%# Bind("SupplierName") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter" runat="server" id="tdEditNPD">
                                    <%--<asp:Label ID="lblNewProdDev" CssClass="textboxcommon" runat="server" Text='<%# Bind("NewProdDev") %>' />--%>
                                    <asp:CheckBox ID="chkCheckBox" runat="server" Checked='<%# Bind("NewProdDev") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStatus" CssClass="textboxcommon" runat="server" Text='<%# Bind("Status") %>' ClientIDMode="Static" />

                                </td>
                                <td class="textCenter">
                                    <asp:CheckBox runat="server" ID="chkSelect" Style="padding: 2px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <tr>
                                <td>
                                    <asp:ImageButton runat="server" ID="btnUpdate" CommandName="Update" ToolTip="Update row" ImageUrl="~/GEA/Icons/update_icon.png" Style="display: inline;" ImageAlign="Middle" />
                                    <asp:ImageButton runat="server" ID="btnCancelEdit" CommandName="Cancel" ToolTip="Cancel edit" ImageUrl="~/GEA/Icons/cancel_icon.png" Style="display: inline;" ImageAlign="Middle" />
                                </td>
                                <td class="textCenter">
                                    <asp:HiddenField ID="hdfUpdate" runat="server" Value='<%# Bind("IDD") %>' ClientIDMode="Static" />
                                    <asp:Label ID="lblPriority" CssClass="textboxcommon" runat="server" Text='<%# Bind("Priority") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:TextBox runat="server" ID="txtPoNum" CssClass="textboxedit textCenter form-control" Text='<%# Bind("ProductionOrderNumber") %>' ClientIDMode="Static" />
                                    <asp:Label ID="lblProdOrderNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("ProductionOrderNumber") %>' Visible="false" ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:DropDownList runat="server" ID="ddlMatID" CssClass="form-control" />
                                    <asp:Label ID="lblMaterialID" CssClass="textboxcommon" runat="server" Text='<%# Bind("MaterialID") %>' Visible="false" ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblModel" CssClass="textboxcommon" runat="server" Text='<%# Bind("Model") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblDesc" CssClass="textboxcommon" runat="server" Text='<%# Bind("ModelDescription") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:DropDownList runat="server" ID="ddlOpnNum" CssClass="form-control" />
                                    <asp:Label ID="lblOpnNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("OperationNumber") %>' Visible="false" ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:TextBox runat="server" ID="txtQty" CssClass="textboxedit textCenter form-control" Text='<%# Bind("Quantity") %>' TextMode="Number" MaxLength="4" ClientIDMode="Static" />
                                    <asp:Label ID="lblQuantity" CssClass="textboxcommon" runat="server" Text='<%# Bind("Quantity") %>' Visible="false" ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStdCycleTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("StdCycleTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStdSetupTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("StdSetupTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOldSchStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("OldScheduledStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOldSchEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("OldScheduledEndTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblSchStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ScheduledStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblSchEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ScheduledEndTime") %>' ClientIDMode="Static" />
                                </td>

                                <td class="textCenter">
                                    <asp:Label ID="lblActualStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ActualStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblPredCompTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("PredictedCompletionTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblActualEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ActualEndTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStatus" CssClass="textboxcommon" runat="server" Text='<%# Bind("Status") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblExtra" CssClass="textboxcommon" runat="server" Text="" />
                                    <asp:CheckBox runat="server" ID="chkSelect" Style="padding: 2px;" Visible="false" />
                                </td>
                            </tr>
                        </EditItemTemplate>

                        <EmptyDataTemplate>
                            <table id="tblEmptyScheduleView" runat="server" class="table table-bordered">
                                <tr style="background-color: #004848;">
                                    <td style="color: white;">No production schedule details found.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <asp:ListView ID="lvAssemblyDetails" runat="server" DataKeyNames="IDD" class="table table-bordered table-hover" OnItemDataBound="lvAssemblyDetails_ItemDataBound" OnPagePropertiesChanging="lvAssemblyDetails_PagePropertiesChanging" OnSorting="lvAssemblyDetails_Sorting">
                        <LayoutTemplate>
                            <table id="tblAssemblyDetails" runat="server" class="table table-bordered headerFixer grid-position-fixed" style="margin-bottom: 0px;">
                                <thead class="blue">
                                    <tr id="trAssemblyDetailsHeader" runat="server">
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 60px;" class="Header">
                                            <span>Action</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 0px; width: 0px;" class="Header">
                                            <span>Priority</span>
                                            <%--  <asp:ImageButton ID="imgBtnSortPriority" CommandArgument="Priority" CommandName="Sort" ImageUrl="~/Image/asc.gif" runat="server" Style="display: inline;" />--%>
                                        </th>

                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 120px; display: none;" class="Header">
                                            <span>User Priority</span>
                                        </th>

                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 120px;" class="Header">
                                            <span>Production Order Number</span>
                                        </th>
                                        <%--  <th style="text-align: center; overflow-wrap: break-word; min-width: 200px;" class="Header">
                                            <span>Priority </span>
                                        </th>--%>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Local/Export</span>
                                        </th>
                                        <%--     <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>PO Number</span>
                                        </th>--%>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Sale Order</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Operation No.</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Material ID/Model</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Machine/Scroll/Bowl Number</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>RDD Machines </span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Fabrication Number </span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Quantity</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Standard Cycle Time(min)</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Standard Setup Time(min)</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Scheduled Start Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 125px;" class="Header">
                                            <span>Scheduled Completed Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Revised Scheduled Start Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 125px;" class="Header">
                                            <span>Revised Scheduled Completed Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Actual Start Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Predicted Completion</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Actual End Time</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Customer</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Location</span>
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Activities</span>
                                        </th>
                                        <%--   <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Sub Operation</span>
                                        </th>--%>
                                        <th style="text-align: center; overflow-wrap: break-word; min-width: 100px;" class="Header">
                                            <span>Status</span>
                                            <asp:ImageButton ID="imgBtnSortStatus" CommandArgument="Status" CommandName="Sort" ImageUrl="~/Image/asc.gif" runat="server" Style="display: inline;" />
                                        </th>
                                        <th style="text-align: center; overflow-wrap: break-word;" class="Header">
                                            <span>Select All</span>
                                            <asp:CheckBox runat="server" ID="chkHeaderSelect" OnCheckedChanged="chkHeaderSelect_CheckedChanged" AutoPostBack="true" />

                                        </th>
                                    </tr>
                                </thead>
                                <tr id="ItemPlaceholder" runat="server"></tr>
                                <tr id="trSchedulePager" runat="server" style="text-align: center;">
                                    <td colspan="30" id="tdSchedulePager" runat="server">
                                        <asp:DataPager ID="dataPagerSchedule" PageSize="8" runat="server" PagedControlID="lvAssemblyDetails">
                                            <Fields>
                                                <asp:NextPreviousPagerField ShowLastPageButton="false" ShowNextPageButton="false" ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" />
                                                <asp:NumericPagerField ButtonType="Button" NumericButtonCssClass="btn" CurrentPageLabelCssClass="btn disabled" />
                                                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" />
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>

                        <ItemTemplate>
                            <tr id="rowId" runat="server">
                                <td>
                                    <asp:ImageButton runat="server" ID="btnEdit" ToolTip="Edit row" ImageUrl="~/GEA/Icons/edit_icon.png" ImageAlign="AbsMiddle" />
                                </td>
                                <td class="textCenter" style="width: 0px;">
                                    <asp:HiddenField ID="hdfUpdate" runat="server" Value='<%# Bind("IDD") %>' ClientIDMode="Static" />
                                    <asp:Label ID="lblPriority" CssClass="textboxcommon" runat="server" Text='<%# Bind("Priority") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter" style="display: none;">
                                    <asp:Label ID="lblUserPriority" CssClass="textboxcommon" runat="server" Text='<%# Bind("UserPriority") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblProdOrderNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("ProductionOrderNumber") %>' ClientIDMode="Static" />
                                </td>
                                <%--  <td class="textCenter">
                                    <asp:Label ID="lblMaterialID" CssClass="textboxcommon" runat="server" Text='<%# Bind("Priority1") %>' />
                                </td>--%>
                                <td class="textCenter">
                                    <asp:Label ID="lblModel" CssClass="textboxcommon" runat="server" Text='<%# Bind("LocalExport") %>' ClientIDMode="Static" />
                                </td>
                                <%--  <td class="textCenter">
                                    <asp:Label ID="lblDesc" CssClass="textboxcommon" runat="server" Text='<%# Bind("PONumber") %>' />
                                </td>--%>
                                <td class="textCenter">
                                    <asp:Label ID="lblSO" CssClass="textboxcommon" runat="server" Text='<%# Bind("SaleOrder") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOpnNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("OperationNumber") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblMaterialID" CssClass="textboxcommon" runat="server" Text='<%# Bind("Model") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblScrollNumber" CssClass="textboxcommon" runat="server" Text='<%# Bind("ScrollWelded") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblRDDMachine" CssClass="textboxcommon" runat="server" Text='<%# Bind("RDDMachines") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblFabricationNum" CssClass="textboxcommon" runat="server" Text='<%# Bind("FabricationNumber") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblQuantity" CssClass="textboxcommon" runat="server" Text='<%# Bind("Quantity") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStdCycleTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("StdCycleTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblStdSetupTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("StdSetupTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOldSchStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("OldScheduledStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblOldSchEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("OldScheduledEndTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblSchStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ScheduledStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblSchEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ScheduledEndTime") %>' ClientIDMode="Static" />
                                </td>

                                <td class="textCenter">
                                    <asp:Label ID="lblActualStartTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ActualStartTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblPredCompTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("PredictedCompletionTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblActualEndTime" CssClass="textboxcommon" runat="server" Text='<%# Bind("ActualEndTime") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblCustomer" CssClass="textboxcommon" runat="server" Text='<%# Bind("Customer") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:Label ID="lblLocation" CssClass="textboxcommon" runat="server" Text='<%# Bind("Location") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:LinkButton runat="server" ID="lbActivities" ClientIDMode="Static" CssClass="textboxcommon" Text='<%# Bind("Activities") %>' Width="140px" Style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis; color: #4a4aed; text-decoration: underline;"
                                        ToolTip='<%# Bind("Activities") %>'></asp:LinkButton>
                                    <%--<asp:Label ID="Label12" CssClass="textboxcommon" runat="server" Text='<%# Bind("Activities") %>' />--%>
                                </td>
                                <%-- <td class="textCenter">
                                    <asp:Label ID="Label13" CssClass="textboxcommon" runat="server" Text='<%# Bind("SubOperation") %>' />
                                </td>--%>
                                <td class="textCenter">
                                    <asp:Label ID="lblStatus" CssClass="textboxcommon" runat="server" Text='<%# Bind("Status") %>' ClientIDMode="Static" />
                                </td>
                                <td class="textCenter">
                                    <asp:CheckBox runat="server" ID="chkSelect" Style="padding: 2px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </ItemTemplate>

                        <EmptyDataTemplate>
                            <table id="tblEmptyScheduleView" runat="server" class="table table-bordered">
                                <tr style="background-color: #004848;">
                                    <td style="color: white;">No production schedule details found.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
                <asp:Button runat="server" ID="btnCalculatePlan" Text="Calculate Plan" CssClass="btn btn-primary btnStyle" Style="float: left; margin: 3px; display: inline;" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" Text="Export" CssClass="btn btn-primary btnStyle" OnClick="btnExport_Click" />
                <div class="DataOperations">
                    <asp:Button runat="server" ID="btnAddNewSchedule" Text="Add New Schedule" CssClass="btn btn-primary btnStyle" Style="float: right; margin: 3px; display: inline;" ClientIDMode="Static" />
                    <asp:Button runat="server" ID="btnChangePriority" Text="Change Priority" CssClass="btn btn-primary btnStyle" Style="float: right; display: inline; margin: 3px;" ClientIDMode="Static" />
                    <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary btnStyle" Style="float: right; display: inline; margin: 3px;" ClientIDMode="Static" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary btnStyle" Style="float: right; display: inline; margin: 3px;" ClientIDMode="Static" />
                    <asp:Button runat="server" ID="btnSwitchSchedule" CssClass="btn btn-success btnStyle" Style="float: right; margin: 3px; display: inline;" ClientIDMode="Static" OnClick="btnSwitchSchedule_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!--Schedule Error Popup -->
    <div id="ScheduleErrorPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Schedule Import Popup -->
    <div id="ScheduleImportPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Select the desired import method to import the schedule.</h6>
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Selected Filename : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                            <label id="lblFilename" style="display: inline; margin-left: 5px; color: green;" />
                        </div>
                    </div>
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Import Method : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                            <asp:DropDownList runat="server" ID="ddlImportMethod" CssClass="form-control" Style="display: inline; min-width: 150px; margin-left: 5px; margin-bottom: 5px;" ClientIDMode="Static">
                                <asp:ListItem Text="Append Mode" Value="Append" Selected="True" />
                                <asp:ListItem Text="Delete and Import Mode" Value="DeleteAndImport" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnImportData" Text="Import" CssClass="btn btn-primary" OnClick="btnImport_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Change Priority Popup -->
    <div id="ChangePriorityPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Select options to change the priorities of following items.</h6>
                    <table id="tblChangePriority" class="table table-bordered table popupTableStyles">
                        <tbody>
                            <tr>
                                <th>System Priority</th>
                                <th style="width: 0px; display: none;">User Priority</th>
                                <th>Prod. Order</th>
                                <th>Material ID</th>
                                <th>Operation Num</th>
                            </tr>
                        </tbody>
                    </table>
                    <h6>Select whether you want to move thsee records before a specific priority or to end.</h6>
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:RadioButton ID="radioMoveToEnd" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" Checked="true" GroupName="MoveOptions" ClientIDMode="Static" />
                            <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move to end</label>
                            <asp:RadioButton ID="radioMoveBefore" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" GroupName="MoveOptions" ClientIDMode="Static" />
                            <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move Before</label>
                            <asp:DropDownList runat="server" ID="ddlPriorities" CssClass="form-control" Style="display: inline; width: 150px; margin-left: 5px;" Enabled="false" ClientIDMode="Static" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnChange" Text="Change" CssClass="btn btn-primary" OnClick="btnChange_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Calculate Plan Popup -->
    <div id="CalculatePlanPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body" style="margin: 5px;">
                    <h5 style="margin-top: 0px;">Calculate the plan using following calculation methods : </h5>
                    <fieldset>
                        <legend>User Defined Method</legend>
                        <div class="Row">
                            <div class="Col MiddleLeft">
                                <asp:Label runat="server" Text="Calculation Method : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                                <asp:DropDownList runat="server" ID="ddlCalculationMethod" CssClass="form-control" Style="display: inline; min-width: 150px; margin-left: 5px; margin-bottom: 5px;" ClientIDMode="Static">
                                    <asp:ListItem Text="Use Running + New + Parked in Priority order" Value="1" Selected="True" />
                                    <asp:ListItem Text="Use Running + Priority order, pick only New and Parked" Value="2" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="Row">
                            <div class="Col MiddleLeft">
                                <asp:Label runat="server" Text="Start Time : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                                <asp:RadioButton ID="radioRunningSchedule" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" Checked="true" GroupName="StartTime" ClientIDMode="Static" />
                                <label style="margin-left: 5px; margin-top: 4px; display: inline;">Running Schedule</label>
                                <asp:RadioButton ID="radioUserInput" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" GroupName="StartTime" ClientIDMode="Static" />
                                <label style="margin-left: 5px; margin-top: 4px; display: inline;">User Input</label>
                                <div class="input-group" id="divStartDate" style="min-width: 200px;">
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                    <asp:TextBox ID="txtStartTime" runat="server" Style="min-width: 130px; display: inline;" CssClass="form-control date" placeholder="YYYY-MM-DD HH:mm:ss" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="Row">
                           <asp:Button runat="server" ID="btnCalcPlan" Text="Calculate" CssClass="btn btn-primary" OnClick="btnCalcPlan_Click" Style="float: right;" OnClientClick="return calculatePlanValidation();"></asp:Button>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Default Calc. Method</legend>
                        <div class="Row">
                            <div class="Col MiddleLeft">
                                <asp:Label runat="server" Text=" Default Calculation Method : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                                <asp:DropDownList runat="server" ID="ddlDefaultCalcMethod" CssClass="form-control" Style="display: inline; min-width: 150px; margin-left: 5px; margin-bottom: 5px;" ClientIDMode="Static">
                                    <asp:ListItem Text="Use Running + New + Parked in Priority order" Value="1" Selected="True" />
                                    <asp:ListItem Text="Use Running + Priority order, pick only New and Parked" Value="2" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="Row">
                            <asp:Button runat="server" ID="btnSaveDefaultCalcMethod" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveDefaultCalcMethod_Click" Style="float: right; width: 90px;"></asp:Button>
                        </div>
                    </fieldset>
                </div>
                <div class="modal-footer" style="margin-top: 0px;">
                    <button type="button" class="btn btn-danger" data-dismiss="modal" style="width: 90px; margin-right: 12px;">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Delete Schedule Popup -->
    <div id="DeleteSchedulePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Following are the schedules selected for deletion.</h6>
                    <table id="tblDeleteSchedule" class="table table-bordered table popupTableStyles">
                        <%--   <tbody>
                            <tr>
                                <th>Priority</th>
                                <th>Prod. Order</th>
                                <th>Material ID</th>
                                <th>Operation Num</th>
                            </tr>
                        </tbody>--%>
                    </table>
                    <h6>Are you sure you want to delete selected schedules ?</h6>
                </div>
                <div class="modal-footer" style="margin-top: 0px;">
                    <asp:Button runat="server" ID="btnDeleteSch" Text="Delete" CssClass="btn btn-primary" OnClick="btnDeleteSch_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Cancel Schedule Popup -->
    <div id="CancelSchedulePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Following are the schedules selected to be cancelled.</h6>
                    <table id="tblCancelSchedule" class="table table-bordered table popupTableStyles">
                        <%-- <tbody>
                            <tr>
                                <th>Priority</th>
                                <th>Prod. Order</th>
                                <th>Material ID</th>
                                <th>Operation Num</th>
                            </tr>
                        </tbody>--%>
                    </table>
                    <h6>Are you sure you want to cancel all these schedules ?</h6>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnCancelSch" Text="Yes" CssClass="btn btn-primary" OnClick="btnCancelSch_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Add new schedule popup -->
    <div id="AddNewSchedulePopup" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Enter the details to add a new production schedule.</h6>
                    <h6 id="statusmessage" style="color: red;"></h6>
                    <asp:Label runat="server" ID="lblStatus" Font-Bold="true" ClientIDMode="Static" />
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <label style="margin-left: 5px; display: inline; font-weight: 700">Insert Position : </label>
                            <asp:RadioButton ID="radioMoveEnd" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" Checked="true" GroupName="MoveOption" ClientIDMode="Static" />
                            <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move to end</label>
                            <asp:RadioButton ID="radioMove2Before" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" GroupName="MoveOption" ClientIDMode="Static" />
                            <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move before</label>
                            <asp:DropDownList runat="server" ID="ddlAddPriority" CssClass="form-control" Style="display: inline; width: 150px; margin-left: 5px;" Enabled="false" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Prod. Order : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtProdOrder" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Material ID : " Font-Bold="true" Style="margin-left: 5px;" />
                            <br />
                            <asp:TextBox runat="server" ID="txtMaterialSearch" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 200px"></asp:TextBox>
                            <a class="glyphicon glyphicon-refresh" style="display: inline-block; font-size: 18px;" onclick="BindSearchCompnentID();"></a>
                            <%--  <asp:LinkButton runat="server" ID="lnkMaterialSearch" ClientIDMode="Static" CssClass="glyphicon glyphicon-search" Style="display: inline-block;" OnClientClick="return BindSearchCompnentID();"></asp:LinkButton>--%>
                            <asp:DropDownList runat="server" ID="ddlMaterialID" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; display: inline-block; width: 280px" OnSelectedIndexChanged="ddlMaterialID_SelectedIndexChanged" />
                        </div>
                    </div>


                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Operation Num : " Font-Bold="true" Style="margin-left: 5px;" />
                            <%--<asp:DropDownList runat="server" ID="ddlOperationNum" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />--%>
                            <asp:CheckBoxList runat="server" ID="ddlOperationNum" ClientIDMode="Static" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Quantity : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtQuantity" ClientIDMode="Static" TextMode="Number" Text="1" Enabled="false" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                    <div class="Row" id="divAddGRNNumber">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="GRN Number : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtAddGRNNumber" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                    <div class="Row" id="divAddSupplierCode">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Supplier Name : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtSupplierName" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                    <div class="Row" id="divAddNewProdDev">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="New Prod Dev : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:CheckBox runat="server" ID="txtAddNewProdDev" ClientIDMode="Static" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ClientIDMode="Static"></asp:Button>
                    <asp:HiddenField runat="server" ID="hdnMaterialId" Value="" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="HidOpn" Value="" ClientIDMode="Static" />
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Update schedule popup -->
    <div id="UpdateSchedulePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Enter the details to update the production schedule.</h6>
                    <h6 id="statusmsg" style="color: red;"></h6>
                    <asp:Label runat="server" ID="Label1" Font-Bold="true" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hdfIdd" ClientIDMode="Static" Value="1" />
                    <asp:HiddenField runat="server" ID="hdfProd" ClientIDMode="Static" Value="" />
                    <asp:HiddenField runat="server" ID="hdfMat" ClientIDMode="Static" Value="" />
                    <asp:HiddenField runat="server" ID="hdnGRNNumber" ClientIDMode="Static" Value="" />
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Prod. Order : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtUpdtProdOrder" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Material ID : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:DropDownList runat="server" ID="ddlUpdtMatId" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" OnSelectedIndexChanged="ddlUpdtMatId_SelectedIndexChanged" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Operation Num : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:DropDownList runat="server" ID="ddlUpdtOpnNum" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Quantity : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtUpdtQuantity" ClientIDMode="Static" TextMode="Number" Text="1" Enabled="false" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                    <div class="Row" id="divUpdateGRNNumber">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="GRN Number : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtUpdateGRNNumber" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                    <div class="Row" id="divUpdateSupplierName">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Supplier Name : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtUpdateSupplierName" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                    <div class="Row" id="divUpdateNewProdDev">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="New Prod Dev : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:CheckBox runat="server" ID="chkUpdateNewProdDev" ClientIDMode="Static" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnUpdate" Text="Update" CssClass="btn btn-primary" ClientIDMode="Static" OnClick="btnUpdate_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Add new Assembly schedule popup -->
    <div id="AddNewAssemblySchedulePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content" style="width: max-content; margin-left: -175px">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Enter the details to add a new production schedule.</h6>
                    <h6 id="assemblystatusmessage" style="color: red;"></h6>
                    <asp:Label runat="server" ID="Label8" Font-Bold="true" ClientIDMode="Static" />
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <label style="margin-left: 5px; display: inline; font-weight: 700">Insert Position : </label>
                            <asp:RadioButton ID="radioMoveEnd_A" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" Checked="true" GroupName="MoveOption" ClientIDMode="Static" />
                            <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move to end</label>
                            <asp:RadioButton ID="radioMove2Before_A" runat="server" Font-Bold="true" Style="margin-left: 5px; width: 150px; display: inline; text-align: center; vertical-align: middle;" GroupName="MoveOption" ClientIDMode="Static" />
                            <label style="margin-left: 5px; margin-top: 4px; display: inline;">Move before</label>
                            <asp:DropDownList runat="server" ID="ddlAddPriority_A" CssClass="form-control" Style="display: inline; width: 150px; margin-left: 5px;" Enabled="false" ClientIDMode="Static" />
                        </div>
                    </div>


                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <div style="display: inline-block">


                                <div class="Row">
                                    <div class="Col MiddleLeft" style="padding: 5px;">
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Prod. Order : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtProdOrder_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere" />
                                        </div>
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Material ID : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:DropDownList runat="server" ID="ddlMaterialID_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                            <%--OnSelectedIndexChanged="ddlMaterialID_A_SelectedIndexChanged"--%>
                                        </div>

                                    </div>
                                </div>

                                <div class="Row">
                                    <div class="Col MiddleLeft" style="padding: 5px;">
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Operation Nummer : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtOperationNo_A" ClientIDMode="Static" TextMode="Number" Text="9000" Enabled="false" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                        </div>
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Fabrication Nummer : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtFabricationNumber_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere;" />
                                        </div>

                                    </div>
                                </div>


                                <div class="Row">
                                    <div class="Col MiddleLeft" style="padding: 5px;">
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Local / Export : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtLocalExport_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                        </div>
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Sale Order : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtSaleOrder_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                        </div>

                                    </div>
                                </div>


                                <div class="Row">
                                    <div class="Col MiddleLeft" style="padding: 5px;">
                                        <div style="display: inline-block; vertical-align: text-top; width: 48%">
                                            <asp:Label runat="server" Text="Machine/Scroll/Bowl Number : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtScrollWelded_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                        </div>
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="RDD Machines  : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <div class="input-group" style="min-width: 200px; margin-top: -6px">
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                                <asp:TextBox runat="server" ID="txtRDDMachine_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: -7px; margin-bottom: 2px; min-width: 179px; display: inline;" placeholder="DD-MM-YYYY" AutoCompleteType="Disabled" />
                                            </div>
                                        </div>

                                    </div>
                                </div>


                                <div class="Row">
                                    <div class="Col MiddleLeft" style="padding: 5px;">
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Customer : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtCustomer_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                        </div>
                                        <div style="display: inline-block; width: 48%">
                                            <asp:Label runat="server" Text="Location : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:TextBox runat="server" ID="txtLocation_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                        </div>

                                    </div>
                                </div>

                                <div class="Row">
                                    <div class="Col MiddleLeft" style="padding: 5px;">
                                        <asp:Label runat="server" Text="Quantity : " Font-Bold="true" Style="margin-left: 5px;" />
                                        <asp:TextBox runat="server" ID="txtQuantity_A" ClientIDMode="Static" TextMode="Number" Text="1" Enabled="false" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                    </div>
                                </div>

                            </div>
                            <div style="display: inline-block; vertical-align: top; margin-left: 25px;">
                                <div class="Row">
                                    <div class="Col MiddleLeft">
                                        <div style="display: inline-block">
                                            <asp:Label runat="server" Text="Default Sub Operation : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:CheckBoxList runat="server" ID="cbDefaultActivities_A" ClientIDMode="Static" Style="margin-left: 5px; margin-bottom: 2px;" />
                                            <asp:HiddenField runat="server" ID="hfDefaultActivity" Value="" ClientIDMode="Static" />
                                        </div>
                                        <div style="display: inline-block; margin-left: 40px">
                                            <asp:Label runat="server" Text="Optional Sub Operation : " Font-Bold="true" Style="margin-left: 5px;" />
                                            <asp:CheckBoxList runat="server" ID="cbOptionalActivities_A" ClientIDMode="Static" Style="margin-left: 5px; margin-bottom: 2px;" />
                                            <asp:HiddenField runat="server" ID="hfOptionalActivity" Value="" ClientIDMode="Static" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnSaveNewAssembly" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveNewAssembly_Click" ClientIDMode="Static"></asp:Button>

                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Update Assembly schedule popup -->
    <div id="UpdateAssemblySchedulePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Enter the details to update the production schedule.</h6>
                    <h6 id="assemblystatusmsg" style="color: red;"></h6>
                    <asp:Label runat="server" ID="Label9" Font-Bold="true" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hdfIdd_A" ClientIDMode="Static" Value="1" />
                    <asp:HiddenField runat="server" ID="hdfProd_A" ClientIDMode="Static" Value="" />
                    <asp:HiddenField runat="server" ID="hdfMat_A" ClientIDMode="Static" Value="" />
                    <div class="Row">
                        <div class="Col MiddleLeft" style="padding: 5px;">
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Prod. Order : " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdtProdOrder_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere" />
                            </div>
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Material ID : " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:DropDownList runat="server" ID="ddlUpdtMatId_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />

                            </div>
                        </div>
                    </div>
                    <div class="Row">
                        <div class="Col MiddleLeft" style="padding: 5px;">
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Prod. Order : " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdtOperationNum_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere" Enabled="false" />
                            </div>
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Fabrication ID : " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdaterFabricationNum_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />

                            </div>
                        </div>
                    </div>
                    <div class="Row">
                        <div class="Col MiddleLeft" style="padding: 5px;">
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Local/Export: " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdateLocal_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere" />
                            </div>
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Sale Order: " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdateSaleOrder_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />

                            </div>
                        </div>
                    </div>
                    <div class="Row">
                        <div class="Col MiddleLeft" style="padding: 5px;">
                            <div style="display: inline-block; width: 47%">
                                <asp:Label runat="server" Text="Machine/Scroll/BowlNumber: " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdateScrollNum_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere" />
                            </div>
                            <div style="display: inline-block; width: 47%">
                                <asp:Label runat="server" Text="RDD Machines: " Font-Bold="true" Style="margin-left: 5px;" />
                                <div class="input-group" style="min-width: 200px; margin-top: -6px">
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                    <asp:TextBox runat="server" ID="txtUpdateRDDMachines_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: -7px; margin-bottom: 2px; min-width: 179px; display: inline;" placeholder="DD-MM-YYYY" AutoCompleteType="Disabled" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="Row">
                        <div class="Col MiddleLeft" style="padding: 5px;">
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Customer: " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdateCustomer_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px; line-break: anywhere" />
                            </div>
                            <div style="display: inline-block; width: 48%">
                                <asp:Label runat="server" Text="Location: " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:TextBox runat="server" ID="txtUpdateLocation_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />

                            </div>
                        </div>
                    </div>
                    <%-- <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Prod. Order : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtUpdtProdOrder_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Material ID : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:DropDownList runat="server" ID="ddlUpdtMatId_A" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                            
                        </div>
                    </div>--%>

                    <%--<div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Operation Num: " Font-Bold="true" Style="margin-left: 5px;" />
                            
                            <asp:TextBox runat="server" ID="txtUpdtOperationNum_A" ClientIDMode="Static" TextMode="Number" Text="1" Enabled="false" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>--%>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Quantity : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtUpdtQuantity_A" ClientIDMode="Static" TextMode="Number" Text="1" Enabled="false" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:HiddenField runat="server" ID="hfUpdtFabricationNo_A" ClientIDMode="Static" />
                    <asp:Button runat="server" ID="btnUpdateAssembly" Text="Update" CssClass="btn btn-primary" ClientIDMode="Static" OnClick="btnUpdateAssembly_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Update Assembly Activity popup -->
    <div id="UpdateAssemblyActivityPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Select options to change the activity of following items.</h6>
                    <table id="tblUpdateActivity" class="table table-bordered table popupTableStyles">
                    </table>
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <div style="display: inline-block">
                                <asp:Label runat="server" Text="Default Sub Operation : " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:CheckBoxList runat="server" ID="cbUpdateDefaultActivity" ClientIDMode="Static" Style="margin-left: 5px; margin-bottom: 2px;" />
                                <asp:HiddenField runat="server" ID="hfUpdateDefaultActivityChekced" Value="" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hfUpdateDefaultActivityUnChekced" Value="" ClientIDMode="Static" />
                            </div>
                            <div style="display: inline-block; margin-left: 11%">
                                <asp:Label runat="server" Text="Optional Sub Operation : " Font-Bold="true" Style="margin-left: 5px;" />
                                <asp:CheckBoxList runat="server" ID="cbUpdateOptionalActivity" ClientIDMode="Static" Style="margin-left: 5px; margin-bottom: 2px;" />
                                <asp:HiddenField runat="server" ID="hfUpdateOptionalActivityChecked" Value="" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hfUpdateOptionalActivityUnchecked" Value="" ClientIDMode="Static" />
                            </div>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:HiddenField runat="server" ID="hfUpdateActivityMachineId" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfUpdateActivityOPNum" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfUpdateActivityPO" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfUpdateActivityFabNo" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hfUpdateActivityMaterialID" ClientIDMode="Static" />
                    <asp:Button runat="server" ID="btnUpdateActivity" Text="Save" CssClass="btn btn-primary" OnClick="btnUpdateActivity_Click" ClientIDMode="Static"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstStatus]').multiselect({
                includeSelectAllOption: true
            });
            showHideColumn();
        });
        $(document).ready(function () {
            $.unblockUI({});
        });
        function calculatePlanValidation() {
            if ($('#ddlMachineId').val() == "" || $('#ddlMachineId').val() == null) {
                alert("Select Machine ID.");
                return false;
            }
            return true;
        }

        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        $('[id$=txtFromDate]').datetimepicker({
            format: 'YYYY-MM-DD HH:mm:ss',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });

        $('[id$=txtToDate]').datetimepicker({
            format: 'YYYY-MM-DD HH:mm:ss',
            useCurrent: false,
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });

        $('[id$=txtUpdateRDDMachines_A]').datetimepicker({
            format: 'DD-MM-YYYY',
            useCurrent: false,
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtRDDMachine_A]').datetimepicker({
            format: 'DD-MM-YYYY',
            useCurrent: false,
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtStartTime]').datetimepicker({
            format: 'YYYY-MM-DD HH:mm:ss',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            minDate: new Date()
        });

        $("[id$=txtFromDate]").on("dp.change", function (e) {
            $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
        });

        $("[id$=txtToDate]").on("dp.change", function (e) {
            $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
        });

        $("#btnDownloadTemplate").click(function () {
            if (document.URL) {
                let Url = document.URL;
                var downloadUrl = false;
                var process = getMachineIDProcess($("#ddlMachineId").val());
                if (process.includes("Assembly") || process.includes("Testing") || process.includes("Packing") || process.includes("Stores")) {
                    downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/AssemblyImportPlanTemplate.xlsx';
                }
                else if ($("#ddlMachineId").val().includes("Quality In house") || $("#ddlMachineId").val().includes("Quality Incoming")) {
                    downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/QualityImportPlanTemplate.xlsx';
                }
                else {
                    downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/ImportPlanTemplate.xlsx';
                }

                if (downloadUrl) {
                    location.href = downloadUrl;
                }
                else {
                    alert('Import Template not found.');
                }
            }
        });
        $("#btnUpdateActivity").click(function () {
            var default_sub_activity_checked = "", default_sub_activity_unchecked = "", optional_sub_activity_Checked = "", optional_sub_activity_Unchecked = "";
            $("#cbUpdateDefaultActivity tr td").each(function () {
                debugger;
                if ($(this).find('input[type=checkbox]')[0].checked) {
                    default_sub_activity_checked += $(this).find('input[type=checkbox]').val() + ",";
                } else {
                    default_sub_activity_unchecked += $(this).find('input[type=checkbox]').val() + ",";
                }
            });
            default_sub_activity_checked = default_sub_activity_checked.trimEnd(',');
            $('#hfUpdateDefaultActivityChekced').val(default_sub_activity_checked);

            default_sub_activity_unchecked = default_sub_activity_unchecked.trimEnd(',');
            $('#hfUpdateDefaultActivityUnChekced').val(default_sub_activity_unchecked);

            debugger;

            $("#cbUpdateOptionalActivity tr td").each(function () {
                debugger;
                if ($(this).find('input[type=checkbox]')[0].checked) {
                    optional_sub_activity_Checked += $(this).find('input[type=checkbox]').val() + ",";
                } else {
                    optional_sub_activity_Unchecked += $(this).find('input[type=checkbox]').val() + ",";
                }
            });
            optional_sub_activity_Checked = optional_sub_activity_Checked.trimEnd(',');
            $('#hfUpdateOptionalActivityChecked').val(optional_sub_activity_Checked);

            optional_sub_activity_Unchecked = optional_sub_activity_Unchecked.trimEnd(',');
            $('#hfUpdateOptionalActivityUnchecked').val(optional_sub_activity_Unchecked);
        });
        function showHideColumn() {
            var macId = $("#ddlMachineId").val();
            var process = getMachineIDProcess(macId);
            if (process == "Testing" || process == "Packing" || process == "Assembly") {
                let th1 = $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr th")[0];
                // let th2 = $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr th")[23];
                $(th1).css('display', 'none');
                //$(th2).css('display', 'none');
                for (let i = 1; i < $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").length - 1; i++) {
                    let tr = $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr")[i];
                    let td1 = $(tr).children()[0];
                    //let td2 = $(tr).children()[23];
                    $(td1).css('display', 'none');
                    //$(td2).css('display', 'none');
                }
            } else if (process == "Stores" || process == "Assembly") {
                let th1 = $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr th")[0];
                let th2 = $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr th")[25];
                $(th1).css('display', 'table-cell');
                $(th2).css('display', 'table-cell');
                for (let i = 1; i < $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").length - 1; i++) {
                    let tr = $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr")[i];
                    let td1 = $(tr).children()[0];
                    let td2 = $(tr).children()[25];
                    $(td1).css('display', 'table-cell');
                    $(td2).css('display', 'table-cell');
                }
            }
        }
        $("[id*=lbActivities]").click(function () {
            var macId = $("#ddlMachineId").val();
            var this_row = $(this).closest("tr");
            if (this_row) {
                debugger;
                var status = this_row.find('#lblStatus').text().trim();
                if (status !== "" && status !== "Running" && status !== "Completed") {
                    var idd = this_row.find('#hdfUpdate').val();
                    var priority = this_row.find('#lblPriority').text().trim();
                    var prodOrder = this_row.find('#lblProdOrderNumber').text().trim();
                    var matId = this_row.find('#lblMaterialID').text().trim();
                    var opnNumber = this_row.find('#lblOpnNumber').text().trim();
                    var fabNumber = this_row.find('#lblFabricationNum').text().trim();
                    var activityList = this_row.find('#lbActivities').text().trim();
                    $("#hfUpdateActivityMachineId").val(macId);
                    $("#hfUpdateActivityOPNum").val(opnNumber);
                    $("#hfUpdateActivityPO").val(prodOrder);
                    $("#hfUpdateActivityFabNo").val(fabNumber);
                    $("#hfUpdateActivityMaterialID").val(matId);
                    $("#tblUpdateActivity").empty();
                    var tableContain = "<tbody><tr><th>Priority</th> <th>Prod. Order</th> <th>Material ID</th> <th>Operation Num</th><th>Fabrication Num</th></tr></tbody><tbody>";
                    tableContain += ('<tr><td>' + priority + '</td><td style="line-break: anywhere;">' + prodOrder + '</td><td>' + matId + '</td><td>' + opnNumber + '</td><td style="line-break: anywhere;">' + fabNumber + '</td></tr>');
                    tableContain += '</tbody>';
                    $("#tblUpdateActivity").append(tableContain);
                    BindActivityForUpdate(macId, matId, activityList);
                    var title = "Update Activity";
                    $("#UpdateAssemblyActivityPopup .modal-title").html(title);
                    $("#UpdateAssemblyActivityPopup").modal("show");
                } else {
                    var title = "Update Activity";
                    var body = "Cannot update running schedules.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                }
            }
            else {
                var title = "Update Activity";
                var body = "Cannot update data due to some unknown error. Please try after sometime.";
                $("#ScheduleErrorPopup .modal-title").html(title);
                $("#ScheduleErrorPopup .modal-body").html(body);
                $("#ScheduleErrorPopup").modal("show");
            }

        });
        $("#btnAddNewSchedule").click(function () {
            debugger;
            var process = getMachineIDProcess($("#ddlMachineId").val());
            if (process.includes("Stores") || process.includes("Assembly")) {
                $("#txtProdOrder_A").val("");
                $("#txtFabricationNumber_A").val("");
                $("#txtLocalExport_A").val("");
                $("#txtSaleOrder_A").val("");
                $("#txtScrollWelded_A").val("");
                $("#txtRDDMachine_A").val("");
                $("#txtCustomer_A").val("");
                $("#txtLocation_A").val("");
                $("#txtQuantity_A").val(1);
                $("#assemblystatusmessage").text("");
                $("#radioMoveEnd_A").prop('checked', true);
                $("#ddlAddPriority_A").prop("disabled", true);
                var title = "Add New Schedule";
                if (process.includes("Stores")) {
                    $('#txtOperationNo_A').val("9000");
                }
                else if (process.includes("Assembly")) {
                    $('#txtOperationNo_A').val("9001");
                }
                BindComponent();
                //BindAssemblySchedulePriorities();
                BindSchedulePriorities();
                BindAssemblySubOperation();
                $("#AddNewAssemblySchedulePopup .modal-title").html(title);
                $("#AddNewAssemblySchedulePopup").modal("show");
            } else {
                debugger;
                $("#txtProdOrder").val("");
                $("#txtQuantity").val(1);
                $("#statusmessage").text("");
                $("#radioMoveEnd").prop('checked', true);
                $("#ddlAddPriority").prop("disabled", true);
                var title = "Add New Schedule";
                if ($("#ddlMachineId").val() == "Quality Incoming" || $("#ddlMachineId").val() == "Quality In house") {
                    $("#divAddNewProdDev").css('display', '');
                    $("#divAddSupplierCode").css('display', '');
                    $("#divAddGRNNumber").css('display', '');
                    /*$("#txtAddQuantity").css('disabled')*/
                    if ($("#ddlMachineId").val() == "Quality Incoming")
                        $("#txtQuantity").prop('disabled', false);
                    else
                        $("#txtQuantity").prop('disabled', true);
                }
                else {
                    $("#divAddNewProdDev").css('display', 'none');
                    $("#divAddSupplierCode").css('display', 'none');
                    $("#divAddGRNNumber").css('display', 'none');
                    $("#txtQuantity").prop('disabled', true);
                }
                $('#txtMaterialSearch').val("");
                BindComponent();
                BindSchedulePriorities();
                BindOperation();
                $("#AddNewSchedulePopup .modal-title").html(title);
                $("#AddNewSchedulePopup").modal("show");
            }
        });

        $("#btnCalculatePlan").click(function () {
            var rowCount = 0;
            if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {
                    var this_row = $(this);
                    if (this_row !== null & this_row !== undefined) {
                        rowCount += 1;
                    }
                });
            } else {
                $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                    var this_row = $(this);
                    if (this_row !== null & this_row !== undefined) {
                        rowCount += 1;
                    }
                });
            }


            if (rowCount > 0) {
                $("#radioRunningSchedule").prop('checked', true);
                $("#txtStartTime").val("");
                $("#txtStartTime").prop("disabled", true);
                var title = "Calculate Schedule Plan";
                $("#CalculatePlanPopup .modal-title").html(title);
                $("#CalculatePlanPopup").modal("show");
                return true;
            }
            else {
                var title = "Calculate Schedule Plan";
                var body = "Cannot calculate plan as no data is available.";
                $("#ScheduleErrorPopup .modal-title").html(title);
                $("#ScheduleErrorPopup .modal-body").html(body);
                $("#ScheduleErrorPopup").modal("show");
                return false;
            }
        });

        $("#btnCancel").click(function () {
            var selectedSchedules = [];
            var chkCount = 0;
            var notParkedCount = 0;
            if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {
                    var this_row = $(this);
                    var status = "";
                    if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                        status = this_row.find('#lblStatus').text().trim();
                    }
                    else {
                        status = this_row.find('#lblStatus').text().trim();
                    }
                    if (status !== "" && status !== "Running") {
                        var chkSelect = this_row.find('#chkSelect');
                        if (chkSelect !== null & chkSelect !== undefined) {
                            if ($(chkSelect).is(':checked') == true) {
                                chkCount += 1;
                                if (status !== "Parked") notParkedCount += 1;
                                selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblUserPriority').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                            }
                        }
                    }
                });
                if (chkCount > 0) {
                    if (notParkedCount == 0) {
                        $("#tblCancelSchedule tbody tr").not(':first').each(function () {
                            $(this).remove();
                        });
                        if (selectedSchedules.length > 0) {
                            $("#tblCancelSchedule").empty();
                            var tableContain = " <tbody> <tr><th>Priority</th>  <th>Prod. Order</th> <th>Material ID</th>  <th>Operation Num</th> </tr> </tbody>";
                            tableContain += "<tbody>";
                            //var tableContain = "<tbody>";
                            selectedSchedules.forEach(function (currentValue, index) {
                                tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                            });
                            tableContain += '</tbody>';
                            $("#tblCancelSchedule").append(tableContain);
                            var title = "Cancel Parked Schedules";
                            $("#CancelSchedulePopup .modal-title").html(title);
                            $("#CancelSchedulePopup").modal("show");
                            return true;
                        }
                        else {
                            $('#tblCancelSchedule').append('<tr><td colspan="4" style="text-align: center;">No parked record selected for cancellation !!</td></tr>');
                            return false;
                        }
                    }
                    else {
                        var title = "Cancel Parked Schedules";
                        var body = "Only parked schedules can be cancelled. Please uncheck all the new or running schedules.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
                else {
                    var title = "Cancel Parked Schedules";
                    var body = "Cannot cancel schedules as as no data is selected.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            } else {
                $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                    var this_row = $(this);
                    var status = this_row.find('#lblStatus').text().trim();
                    if (status !== "" && status !== "Running") {
                        var chkSelect = this_row.find('#chkSelect');
                        if (chkSelect !== null & chkSelect !== undefined) {
                            if ($(chkSelect).is(':checked') == true) {
                                chkCount += 1;
                                if (status !== "Parked") notParkedCount += 1;
                                selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim(), feb_num: this_row.find('#lblFabricationNum').text().trim() });
                            }
                        }
                    }
                });
                if (chkCount > 0) {
                    if (notParkedCount == 0) {
                        $("#tblCancelSchedule tbody tr").not(':first').each(function () {
                            $(this).remove();
                        });
                        if (selectedSchedules.length > 0) {
                            $("#tblCancelSchedule").empty();
                            var tableContain = " <tbody> <tr><th>Priority</th>  <th>Prod. Order</th> <th>Material ID</th>  <th>Operation Num</th> <th>Febrication Num</th> </tr> </tbody>";
                            tableContain += "<tbody>";
                            //var tableContain = "<tbody>";
                            selectedSchedules.forEach(function (currentValue, index) {
                                tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td><td>' + currentValue['feb_num'] + '</td></tr>');
                            });
                            tableContain += '</tbody>';
                            $("#tblCancelSchedule").append(tableContain);
                            var title = "Cancel Parked Schedules";
                            $("#CancelSchedulePopup .modal-title").html(title);
                            $("#CancelSchedulePopup").modal("show");
                            return true;
                        }
                        else {
                            $('#tblCancelSchedule').append('<tr><td colspan="4" style="text-align: center;">No parked record selected for cancellation !!</td></tr>');
                            return false;
                        }
                    }
                    else {
                        var title = "Cancel Parked Schedules";
                        var body = "Only parked schedules can be cancelled. Please uncheck all the new or running schedules.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
                else {
                    var title = "Cancel Parked Schedules";
                    var body = "Cannot cancel schedules as as no data is selected.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            }


        });

        $("#btnDelete").click(function () {
            var selectedSchedules = [];
            var chkCount = 0;
            var parkedOrRunningCount = 0; debugger;
            if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {

                    var this_row = $(this);
                    debugger;
                    var status = "";
                    if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                        status = this_row.find('#lblStatus').text().trim();
                    }
                    else {
                        status = this_row.find('#lblStatus').text().trim();
                    }
                    if (status !== "" && status !== "Running" && status !== "Parked") {
                        if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    if (status == "Running" || status == "Parked") parkedOrRunningCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                }
                            }
                        }
                        else {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    if (status == "Running" || status == "Parked") parkedOrRunningCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                }
                            }
                        }
                    }
                });
                if (chkCount > 0) {
                    if (parkedOrRunningCount == 0) {
                        $("#tblDeleteSchedule tbody tr").not(':first').each(function () {
                            $(this).remove();
                        });
                        if (selectedSchedules.length > 0) {

                            $("#tblDeleteSchedule").empty();
                            var tableContain = "  <tbody> <tr> <th>Priority</th> <th>Prod. Order</th><th>Material ID</th> <th>Operation Num</th></tr></tbody>";
                            tableContain += "<tbody>";
                            // var tableContain = "<tbody>";
                            selectedSchedules.forEach(function (currentValue, index) {
                                tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                            });
                            tableContain += '</tbody>';
                            $("#tblDeleteSchedule").append(tableContain);
                            var title = "Delete Schedule";
                            $("#DeleteSchedulePopup .modal-title").html(title);
                            $("#DeleteSchedulePopup").modal("show");
                            return true;
                        }
                        else {
                            $('#tblDeleteSchedule').append('<tr><td colspan="4" style="text-align: center;">No record selected for deletion !!</td></tr>');
                            return false;
                        }
                    }
                    else {
                        var title = "Delete Schedule";
                        var body = "Cannot delete running or parked records. Please uncheck running or parked records.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
                else {
                    var title = "Delete Schedule";
                    var body = "No schedule is selected for deletion.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            } else {

                $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                    var this_row = $(this);
                    var status = this_row.find('#lblStatus').text().trim();
                    if (status !== "" && status !== "Running") {
                        var chkSelect = this_row.find('#chkSelect');
                        if (chkSelect !== null & chkSelect !== undefined) {
                            if ($(chkSelect).is(':checked') == true) {
                                chkCount += 1;
                                if (status == "Running" || status == "Parked") parkedOrRunningCount += 1;
                                selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim(), feb_num: this_row.find('#lblFabricationNum').text().trim() });
                            }
                        }
                    }
                });
                if (chkCount > 0) {
                    if (parkedOrRunningCount == 0) {
                        $("#tblDeleteSchedule tbody tr").not(':first').each(function () {
                            $(this).remove();
                        });
                        if (selectedSchedules.length > 0) {
                            $("#tblDeleteSchedule").empty();
                            var tableContain = "  <tbody> <tr> <th>Priority</th> <th>Prod. Order</th><th>Material ID</th> <th>Operation Num</th><th>Febrication Num</th></tr></tbody>";
                            tableContain += "<tbody>";
                            //var tableContain = "<tbody>";
                            selectedSchedules.forEach(function (currentValue, index) {
                                tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td><td>' + currentValue['feb_num'] + '</td></tr>');
                            });
                            tableContain += '</tbody>';
                            $("#tblDeleteSchedule").append(tableContain);
                            var title = "Delete Schedule";
                            $("#DeleteSchedulePopup .modal-title").html(title);
                            $("#DeleteSchedulePopup").modal("show");
                            return true;
                        }
                        else {
                            $('#tblDeleteSchedule').append('<tr><td colspan="4" style="text-align: center;">No record selected for deletion !!</td></tr>');
                            return false;
                        }
                    }
                    else {
                        var title = "Delete Schedule";
                        var body = "Cannot delete running or parked records. Please uncheck running or parked records.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
                else {
                    var title = "Delete Schedule";
                    var body = "No schedule is selected for deletion.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            }
        });

        $("#btnChangePriority").click(function () {
            debugger;
            var selectedSchedules = [];
            var chkCount = 0;
            if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {
                    var this_row = $(this);
                    var status = "";
                    if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                        status = this_row.find('#lblStatus').text().trim();
                        if (status !== "" && status !== "Running") {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), UserPriority: this_row.find('#lblUserPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                }
                            }
                        }
                    }
                    else {
                        status = this_row.find('#lblStatus').text().trim();
                        if (status !== "" && status !== "Running") {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), UserPriority: this_row.find('#lblUserPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                }
                            }
                        }
                    }

                });
                if (chkCount > 0) {
                    $("#tblChangePriority tbody tr").not(':first').each(function () {
                        $(this).remove();
                    });
                    BindPriorities()
                    if (selectedSchedules.length > 0) {
                        var tableContain = "<tbody>";
                        selectedSchedules.forEach(function (currentValue, index) {
                            tableContain += ('<tr><td >' + currentValue['priority'] + '</td><td style="width:0px;display:none;">' + currentValue['UserPriority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                        });
                        tableContain += '</tbody>';
                        $("#tblChangePriority").append(tableContain);
                        $("#radioMoveToEnd").prop('checked', true);
                        $("#ddlPriorities").prop("disabled", true);
                        var title = "Change Schedule Priority";
                        $("#ChangePriorityPopup .modal-title").html(title);
                        $("#ChangePriorityPopup").modal("show");
                        return true;
                    }
                    else {
                        $('#tblChangePriority').append('<tr><td colspan="4" style="text-align: center;">No record selected for priority change !!</td></tr>');
                        return false;
                    }
                }
                else {
                    var title = "Change Schedule Priority";
                    var body = "No schedule is selected for priority change.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            }
            else {
                $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                    var this_row = $(this);
                    var status = this_row.find('#lblStatus').text().trim();
                    if (status !== "" && status !== "Running") {
                        var chkSelect = this_row.find('#chkSelect');
                        if (chkSelect !== null & chkSelect !== undefined) {
                            if ($(chkSelect).is(':checked') == true) {
                                chkCount += 1;
                                selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), Userpriority: this_row.find('#lblUserPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                            }
                        }
                    }
                });
                if (chkCount > 0) {
                    $("#tblChangePriority tbody tr").not(':first').each(function () {
                        $(this).remove();
                    });
                    BindPriorities()
                    if (selectedSchedules.length > 0) {
                        var tableContain = "<tbody>";
                        selectedSchedules.forEach(function (currentValue, index) {
                            tableContain += ('<tr><td >' + currentValue['priority'] + '</td><td style="width:0px;display:none;">' + currentValue['Userpriority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                        });
                        tableContain += '</tbody>';
                        $("#tblChangePriority").append(tableContain);
                        $("#radioMoveToEnd").prop('checked', true);
                        $("#ddlPriorities").prop("disabled", true);
                        var title = "Change Schedule Priority";
                        $("#ChangePriorityPopup .modal-title").html(title);
                        $("#ChangePriorityPopup").modal("show");
                        return true;
                    }
                    else {
                        $('#tblChangePriority').append('<tr><td colspan="4" style="text-align: center;">No record selected for priority change !!</td></tr>');
                        return false;
                    }
                }
                else {
                    var title = "Change Schedule Priority";
                    var body = "No schedule is selected for priority change.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            }

        });

        $("#ddlMaterialID").change(function () {
            $("#ddlOperationNum tr").each(function () {
                $(this).remove();
            });
            BindOperation();
        });
        $("#ddlMaterialID_A").change(function () {
            //$("#cbDefaultActivities_A tr").each(function () {
            //    $(this).remove();
            //});
            BindAssemblySubOperation();
        });
        $("#btnImport").click(function () {
            var dd = $("#FileUploadSchedule")[0].files.length;
            if (dd == 0) {
                var title = "Import Schedule";
                var body = "Cannot import as no file is chosen for import.";
                $("#ScheduleErrorPopup .modal-title").html(title);
                $("#ScheduleErrorPopup .modal-body").html(body);
                $("#ScheduleErrorPopup").modal("show");
                return false;
            }
            else {
                var filename = $("#FileUploadSchedule")[0].files[0].name;
                if (filename !== null && filename !== undefined) {
                    var file_extension = filename.split('.').pop();
                    if (file_extension !== null && file_extension !== undefined) {
                        if (file_extension !== "xlsx") {
                            var title = "Import Schedule";
                            var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                            return false;
                        }
                        else {
                            var title = "Import Schedule";
                            $("#lblFilename").text(filename);
                            $("#ScheduleImportPopup .modal-title").html(title);
                            $("#ScheduleImportPopup").modal("show");
                            return true;
                        }
                    }
                    else {
                        var title = "Import Schedule";
                        var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
                else {
                    var title = "Import Schedule";
                    var body = "Cannot import as no file is chosen for import.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            }
        });
        function getMachineIDProcess() {
            var process = "";
            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/getMachineIDProcess",
                contentType: "application/json; charset=utf-8",
                data: '{machineID:"' + $('#ddlMachineId').val() + '"}',
                dataType: "json",
                success: function (response) {
                    process = response.d;
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
            return process;
        }
        $("#btnSave").click(function () {
            debugger;
            var process = getMachineIDProcess($("#ddlMachineId").val());
            var prod_order = $("#txtProdOrder").val();
            var material_id = $("#ddlMaterialID").val();
            var opn_num = "";
            var autoOpnNo = "";
            $("#ddlOperationNum tr td").each(function () {
                debugger;
                if ($(this).find('input[type=checkbox]')[0].checked) {
                    opn_num += $(this).find('input[type=checkbox]').val() + ",";
                    if (process.toLowerCase() == "qualityinhouse" && isAutoScheduleMachineAssigned(material_id, $(this).find('input[type=checkbox]').val()) == false) {
                        autoOpnNo = $(this).find('input[type=checkbox]').val();
                    }
                }
            });
            if (autoOpnNo != "") {
                $("#statusmessage").text("Please assign Auto Schedule Machine.");
                return false;
            }
            opn_num = opn_num.trimEnd(',');
            $('#HidOpn').val(opn_num);
            var qty = $("#txtQuantity").val();
            if (prod_order.trim().length == 0) {
                $("#statusmessage").text("* Production Order is mandatory.");
                return false;
            }
            else if (material_id == null || material_id.trim().length == 0) {
                $("#statusmessage").text("* Material ID is mandatory.");
                return false;
            }
            else if (opn_num == null || opn_num.trim().length == 0) {
                $("#statusmessage").text("* Operation number is mandatory.");
                return false;
            }
            else if (qty.trim().length == 0) {
                $("#statusmessage").text("* Quantity is mandatory.");
                return false;
            }
            else if (qty.trim() < 1) {
                $("#statusmessage").text("* Quantity must be greater than 0.");
                return false;
            }
            else if (getMachineIDProcess() == "QualityIncoming") {
                var grnNumber = $("#txtAddGRNNumber").val();
                if (grnNumber.trim() == "") {
                    $("#statusmessage").text("* GRN Number is mandatory.");
                    return false;
                }
            }
            else {
                var macId = $("#ddlMachineId").val().trim();
                $('#hdnMaterialId').val(material_id);
                $.ajax({
                    type: "POST",
                    url: "ProductionScheduler.aspx/IsScheduleExists",
                    contentType: "application/json; charset=utf-8",
                    data: '{machine:"' + macId + '",prodOrder:"' + prod_order + '",matId:"' + material_id + '",opnNum:"' + opn_num + '"}',
                    dataType: "json",
                    success: function (response) {
                        var res = response.d;
                        if (res == "true") {
                            $("#statusmessage").text("* This machine, prod order, material id and operation combination already exists.");
                            return false;
                        }
                    },
                    error: function (err) {
                        alert('Error : ' + err);
                    }
                });
                return res !== "true";
            }
        });

        $("#btnSaveNewAssembly").click(function () {
            debugger;
            var opn_num = $("#txtOperationNo_A").val();
            var rddMachine = $("#txtRDDMachine_A").val();
            var prod_order = $("#txtProdOrder_A").val();
            var material_id = $("#ddlMaterialID_A").val();
            var fabricationNo = $("#txtFabricationNumber_A").val();
            var process = getMachineIDProcess($("#ddlMachineId").val());
            if (process.includes("Stores") && isAutoScheduleMachineAssigned(material_id, opn_num) == false) {
                $("#assemblystatusmessage").text("Please assign Auto Schedule Machine.");
                return false;
            }

            var SaleOrder = $("#txtSaleOrder_A").val();
            var Location = $("#txtLocation_A").val();
            var LocalExport = $("#txtLocalExport_A").val();
            var ScrollNumber = $("#txtScrollWelded_A").val();
            var Customer = $("#txtCustomer_A").val();


            var default_sub_activity = "", optional_sub_activity = "";
            var defaultActFlag = false;
            $("#cbDefaultActivities_A tr td").each(function () {
                if ($(this).find('input[type=checkbox]')[0].checked) {
                    default_sub_activity += $(this).find('input[type=checkbox]').val() + ",";
                    defaultActFlag = true;
                }
            });
            default_sub_activity = default_sub_activity.trimEnd(',');
            $('#hfDefaultActivity').val(default_sub_activity);

            var optionalActFlag = false;
            $("#cbOptionalActivities_A tr td").each(function () {
                if ($(this).find('input[type=checkbox]')[0].checked) {
                    optional_sub_activity += $(this).find('input[type=checkbox]').val() + ",";
                    optionalActFlag = true;
                }
            });
            optional_sub_activity = optional_sub_activity.trimEnd(',');
            $('#hfOptionalActivity').val(optional_sub_activity);
            if (defaultActFlag == false && optionalActFlag == false) {
                $("#assemblystatusmessage").text("Please select Sub Operation.");
                return false;
            }

            var qty = $("#txtQuantity_A").val();
            if (prod_order.trim().length == 0) {
                $("#assemblystatusmessage").text("* Production Order is mandatory.");
                return false;
            }
            else if (material_id == null || material_id.trim().length == 0) {
                $("#assemblystatusmessage").text("* Material ID is mandatory.");
                return false;
            }
            else if (fabricationNo.trim().length == 0) {
                $("#assemblystatusmessage").text("* Fabrication Number is mandatory.");
                return false;
            }
            else if (SaleOrder.trim().length == 0) {
                $("#assemblystatusmessage").text("* SaleOrder is mandatory.");
                return false;
            }
            else if (Location.trim().length == 0) {
                $("#assemblystatusmessage").text("* Location is mandatory.");
                return false;
            }
            else if (LocalExport.trim().length == 0) {
                $("#assemblystatusmessage").text("* LocalExport is mandatory.");
                return false;
            }
            else if (ScrollNumber.trim().length == 0) {
                $("#assemblystatusmessage").text("* Machine/Scroll/Bowl Number is mandatory.");
                return false;
            }
            else if (Customer.trim().length == 0) {
                $("#assemblystatusmessage").text("* Customer is mandatory.");
                return false;
            }
            //if (opn_num == null || opn_num.trim().length == 0) {
            //    $("#assemblystatusmessage").text("* Sub Operation is mandatory.");
            //    return false;
            //}
            else if (rddMachine != "") {
                if (!moment(rddMachine, 'DD-MM-YYYY', true).isValid()) {
                    $("#assemblystatusmessage").text("* RDD Machine is not in correct format.");
                    return false;
                }
            }
            else
                if (qty.trim().length == 0) {
                    $("#assemblystatusmessage").text("* Quantity is mandatory.");
                    return false;
                }
                else if (qty.trim() < 1) {
                    $("#assemblystatusmessage").text("* Quantity must be greater than 0.");
                    return false;
                }
                else {
                    var macId = $("#ddlMachineId").val().trim();
                    $.ajax({
                        type: "POST",
                        url: "ProductionScheduler.aspx/IsAssemblyScheduleExists",
                        contentType: "application/json; charset=utf-8",
                        data: '{machine:"' + macId + '",prodOrder:"' + prod_order + '",matId:"' + material_id + '",opnNum:"' + opn_num + '",fabricationNo:"' + fabricationNo + '"}',
                        dataType: "json",
                        success: function (response) {
                            var res = response.d;
                            if (res == "true") {
                                $("#statusmessage").text("* This machine, prod order, material id, operation and fabrication number combination already exists.");
                                return false;
                            }
                        },
                        error: function (err) {
                            alert('Error : ' + err);
                        }
                    });
                    return res !== "true";
                }
        });

        function isAutoScheduleMachineAssigned(compID, opnNo) {
            var isAssigned = false;
            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/IsAutoScheduleAssigned",
                contentType: "application/json; charset=utf-8",
                data: '{compID:"' + compID + '",opnNo:"' + opnNo + '"}',
                dataType: "json",
                success: function (response) {
                    isAssigned = response.d;
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
            return isAssigned;
        }

        $("*[id*=btnEdit]").click(function () {
            debugger;
            if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                var this_row = $(this).closest("tr");
                if (this_row) {
                    var status = "";
                    var MachineID = $("#ddlMachineId").val();
                    if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming")
                        status = this_row.find('#lblStatus').text().trim();
                    else
                        status = this_row.find('#lblStatus').text().trim();
                    if (status !== "" && status !== "Running" && status !== "Completed") {
                        var idd = this_row.find('#hdfUpdate').val();
                        var prodOrder = this_row.find('#lblProdOrderNumber').text().trim();
                        var matId = this_row.find('#lblMaterialID').text().trim();
                        var opnNumber = this_row.find('#lblOpnNumber').text().trim();

                        BindComponent();
                        var schQty = this_row.find('#lblQuantity').text().trim();
                        $("#ddlUpdtMatId").val(matId);
                        getOperationForMatIdAndMachine(opnNumber);
                        $("#statusmsg").text("");
                        $("#hdfIdd").val(idd);
                        $("#txtUpdtProdOrder").val(prodOrder);
                        $("#hdfProd").val(prodOrder);
                        $("#hdfMat").val(matId);
                        $("#txtUpdtQuantity").val(1);
                        $("#ddlUpdtOpnNum").val(opnNumber);
                        if (MachineID == "Quality In house" || MachineID == "Quality Incoming") {
                            $("#divUpdateGRNNumber").css('display', '');
                            $("#divUpdateSupplierName").css('display', '');
                            $("#divUpdateNewProdDev").css('display', '');
                            $("#txtUpdateGRNNumber").val(this_row.find('#lblGRNNumber').text().trim());
                            $('#hdnGRNNumber').val($("#txtUpdateGRNNumber").val());
                            $("#txtUpdateSupplierName").val(this_row.find('#lblSupplierName').text().trim());
                            $("#chkUpdateNewProdDev").prop('checked', (this_row.find('#chkCheckBox').is(':checked')));
                            if (MachineID == "Quality Incoming") {
                                $("#txtUpdtQuantity").val(this_row.find('#lblQuantity').text().trim());
                                $("#txtUpdtQuantity").prop('disabled', false);
                            }
                            else
                                $("#txtUpdtQuantity").prop('disabled', true);
                        }
                        else {
                            $("#divUpdateGRNNumber").css('display', 'none');
                            $("#divUpdateSupplierName").css('display', 'none');
                            $("#divUpdateNewProdDev").css('display', 'none');
                        }
                        var title = "Update Schedule";
                        $("#UpdateSchedulePopup .modal-title").html(title);
                        $("#UpdateSchedulePopup").modal("show");
                    }
                    else {
                        var title = "Update Schedule";
                        var body = "Cannot update running schedules.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                    }
                }
                else {
                    var title = "Update Schedule";
                    var body = "Cannot update data due to some unknown error. Please try after sometime.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                }
            } else {
                var this_row = $(this).closest("tr");
                if (this_row) {
                    var status = this_row.find('#lblStatus').text().trim();
                    if (status !== "" && status !== "Running" && status !== "Completed") {
                        debugger;
                        var idd = this_row.find('#hdfUpdate').val();
                        var prodOrder = this_row.find('#lblProdOrderNumber').text().trim();
                        var matId = this_row.find('#lblMaterialID').text().trim();
                        var opnNumber = this_row.find('#lblOpnNumber').text().trim();
                        var fabNumber = this_row.find('#lblFabricationNum').text().trim();

                        var LocalExport = this_row.find('#lblModel').text().trim();
                        var SaleOrder = this_row.find('#lblSO').text().trim();
                        var ScrollNumber = this_row.find('#lblScrollNumber').text().trim();
                        var RDDMachine = this_row.find('#lblRDDMachine').text().trim();
                        var Customer = this_row.find('#lblCustomer').text().trim();
                        var Location = this_row.find('#lblLocation').text().trim();
                        // var schQty = this_row.find('td:eq(7)').find('.textboxcommon').text().trim();
                        BindComponent();
                        $("#ddlUpdtMatId_A").val(matId);
                        // getSubOperationForMatIdAndMachine(opnNumber);
                        $("#assemblystatusmsg").text("");
                        $("#hdfIdd_A").val(idd);
                        $("#txtUpdtProdOrder_A").val(prodOrder);
                        $("#hdfProd_A").val(prodOrder);
                        $("#hdfMat_A").val(matId);
                        $("#txtUpdtQuantity_A").val(1);
                        $("#txtUpdtOperationNum_A").val(opnNumber);
                        $("#hfUpdtFabricationNo_A").val(fabNumber);


                        $("#txtUpdaterFabricationNum_A").val(fabNumber);
                        $("#txtUpdateLocal_A").val(LocalExport);
                        $("#txtUpdateSaleOrder_A").val(SaleOrder);
                        $("#txtUpdateScrollNum_A").val(ScrollNumber);
                        $("#txtUpdateRDDMachines_A").val(RDDMachine);
                        $("#txtUpdateCustomer_A").val(Customer);
                        $("#txtUpdateLocation_A").val(Location);

                        // $("#ddlUpdtSubOperationNum_A").val(opnNumber);
                        var title = "Update Schedule";
                        $("#UpdateAssemblySchedulePopup .modal-title").html(title);
                        $("#UpdateAssemblySchedulePopup").modal("show");
                    }
                    else {
                        var title = "Update Schedule";
                        var body = "Cannot update running schedules.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                    }
                }
                else {
                    var title = "Update Schedule";
                    var body = "Cannot update data due to some unknown error. Please try after sometime.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                }
            }

        });

        $("#btnUpdate").click(function () {
            if ($("#ddlUpdtOpnNum").val())
                return true;
            else {
                $("#statusmsg").css('color', 'red');
                $("#statusmsg").text("* Operation number cannot be empty.");
                return false;
            }
        });

        $("#btnUpdateAssembly").click(function () {
            //if ($("#ddlUpdtSubOperationNum_A").val())
            //    return true;
            //else {
            //    $("#assemblystatusmsg").css('color', 'red');
            //    $("#assemblystatusmsg").text("* Operation number cannot be empty.");
            //    return false;
            //}

            if ($("#txtUpdtProdOrder_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* Production order cannot be empty.");
                return false;
            }
            if ($("#txtUpdaterFabricationNum_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* Fabrication Number cannot be empty.");
                return false;
            }
            if ($("#txtUpdateLocal_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* Local Export cannot be empty.");
                return false;
            }
            if ($("#txtUpdateSaleOrder_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* Sale order cannot be empty.");
                return false;
            }
            if ($("#txtUpdateScrollNum_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* Machine/Scroll/Bowl Number cannot be empty.");
                return false;
            }
            if ($("#txtUpdateRDDMachines_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* RDD Machines cannot be empty.");
                return false;
            }
            else if ($("#txtUpdateRDDMachines_A").val() != "") {
                if (!moment($("#txtUpdateRDDMachines_A").val(), 'DD-MM-YYYY', true).isValid()) {
                    $("#assemblystatusmessage").text("* RDD Machine is not in correct format.");
                    return false;
                }
            }
            if ($("#txtUpdateCustomer_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* Customer cannot be empty.");
                return false;
            }
            if ($("#txtUpdateLocation_A").val() == "") {
                $("#assemblystatusmsg").css('color', 'red');
                $("#assemblystatusmsg").text("* Location cannot be empty.");
                return false;
            } else {
                return true;
            }
        });



        $("#ddlUpdtMatId").change(getOperationForMatIdAndMachine);

        function getOperationForMatIdAndMachine(opn = 'OpnNotPassedInJs') {
            debugger;
            var macId = $("#ddlMachineId").val();
            var matId = $("#ddlUpdtMatId").val();
            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/GetOperationsList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '",matId:"' + matId + '"}',
                dataType: "json",
                success: function (response) {
                    var opnList = response.d;
                    $("#ddlUpdtOpnNum").empty();
                    if (opnList !== null && opnList !== undefined) {
                        opnList.forEach(function (value, index) {
                            $("#ddlUpdtOpnNum").append($("<option></option>").val(value).html(value));
                        });
                        if (opn !== 'OpnNotPassedInJs') {
                            $("#ddlUpdtOpnNum").val(opn);
                        }
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
        }


        //$("#ddlUpdtMatId_A").change(getSubOperationForMatIdAndMachine);
        function BindMaterialId() {
            var macId = $("#ddlMachineId").val();
        }

        function getSubOperationForMatIdAndMachine(opn = 'OpnNotPassedInJs') {
            var macId = $("#ddlMachineId").val();
            var matId = $("#ddlUpdtMatId_A").val();
            var counter = 0;
            $.ajax({
                type: "POST",
                url: "ProductionScheduler.aspx/GetSubOperationsList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '",matId:"' + matId + '"}',
                dataType: "json",
                success: function (response) {
                    //var opnList = response.d;
                    //$("#ddlUpdtSubOperationNum_A").empty();
                    //if (opnList !== null && opnList !== undefined) {
                    //    opnList.forEach(function (value, index) {
                    //        $("#ddlUpdtSubOperationNum_A").append($("<option></option>").val(value).html(value));
                    //    });
                    //    if (opn !== 'OpnNotPassedInJs') {
                    //        $("#ddlUpdtSubOperationNum_A").val(opn);
                    //    }
                    //}
                    var opnList = response.d;
                    $("#ddlUpdtSubOperationNum_A").empty();
                    if (opnList !== null && opnList !== undefined) {
                        debugger;
                        $("#ddlUpdtSubOperationNum_A tr").each(function () {
                            $(this).remove();
                        });
                        opnList.forEach(function (value, index) {

                            //$("#ddlOperationNum").append($("<option></option>").val(value).html(value));
                            $("#ddlUpdtSubOperationNum_A").append($('<tr></tr>').append($('<td></td>').append($('<input>').attr({
                                type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter
                            })).append(
                                $('<label>').attr({
                                    for: 'chklistitem' + counter++
                                }).text(value))));
                        });
                    }

                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
        }

        $("#txtProdOrder").focusout(function () {
            if ($("#txtProdOrder").val().length == 0) {
                $("#statusmessage").text("* Production Order is mandatory.");
                return false;
            }
            else {
                $("#statusmessage").text("");
                return true;
            }
        });

        $("#txtProdOrder_A").focusout(function () {
            if ($("#txtProdOrder_A").val().length == 0) {
                $("#assemblystatusmessage").text("* Production Order is mandatory.");
                return false;
            }
            else {
                $("#assemblystatusmessage").text("");
                return true;
            }
        });

        $("#txtFabricationNumber_A").focusout(function () {
            if ($("#txtFabricationNumber_A").val().length == 0) {
                $("#assemblystatusmessage").text("* Fabrication Number is mandatory.");
                return false;
            }
            else {
                $("#assemblystatusmessage").text("");
                return true;
            }
        });

        $("#txtQuantity").focusout(function () {
            if ($("#txtQuantity").val().length == 0) {
                $("#statusmessage").text("* Quantity is mandatory.");
                return false;
            }
            else if ($("#txtQuantity").val() < 1) {
                $("#statusmessage").text("* Quantity must be greater than 0.");
                return false;
            }
            else {
                $("#statusmessage").text("");
                return true;
            }
        });

        const formattedTimestamp = () => {
            const d = new Date()
            const date = d.toISOString().split('T')[0];
            const time = d.toTimeString().split(' ')[0];
            return `${date} ${time}`
        }

        $("#radioMoveToEnd").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlPriorities").prop("disabled", true);
            }
            else {
                $("#ddlPriorities").prop("disabled", false);
            }
        });

        $("#radioMoveEnd").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlAddPriority").prop("disabled", true);
            }
            else {
                $("#ddlAddPriority").prop("disabled", false);
            }
        });

        $("#radioMoveBefore").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlPriorities").prop("disabled", false);
            }
            else {
                $("#ddlPriorities").prop("disabled", true);
            }
        });

        $("#radioMove2Before").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlAddPriority").prop("disabled", false);
            }
            else {
                $("#ddlAddPriority").prop("disabled", true);
            }
        });

        $("#radioMove2Before_A").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlAddPriority_A").prop("disabled", false);
            }
            else {
                $("#ddlAddPriority_A").prop("disabled", true);
            }
        });
        $("#radioMoveEnd_A").change(function () {
            if ($(this)[0].checked == true) {
                $("#ddlAddPriority_A").prop("disabled", true);
            }
            else {
                $("#ddlAddPriority_A").prop("disabled", false);
            }
        });

        $("#radioRunningSchedule").change(function () {
            if ($(this)[0].checked == true) {
                $("#txtStartTime").prop("disabled", true);
                $("#txtStartTime").val("");
            }
            else {
                $("#txtStartTime").prop("disabled", false);
            }
        });

        $("#radioUserInput").change(function () {
            if ($(this)[0].checked == true) {
                $("#txtStartTime").prop("disabled", false);
                $("#txtStartTime").val(formattedTimestamp());
            }
            else {
                $("#txtStartTime").prop("disabled", true);
            }
        });

        function BindComponent() {
            var macId = $("#ddlMachineId").val();
            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/GetComponentList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '"}',
                dataType: "json",
                success: function (response) {
                    var CompList = response.d;
                    $("#ddlMaterialID").empty();
                    $("#ddlMaterialID_A").empty();
                    $("#ddlUpdtMatId").empty();
                    $("#ddlUpdtMatId_A").empty();
                    if (CompList !== null && CompList !== undefined) {
                        CompList.forEach(function (value, index) {
                            $("#ddlMaterialID").append($("<option></option>").val(value).html(value));
                            $("#ddlUpdtMatId").append($("<option></option>").val(value).html(value));
                            $("#ddlUpdtMatId_A").append($("<option></option>").val(value).html(value));
                            $("#ddlMaterialID_A").append($("<option></option>").val(value).html(value));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });

        }
        function BindSearchCompnentID() {
            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/GetSearchedComponentList",
                contentType: "application/json; charset=utf-8",
                data: '{searchText:"' + $('#txtMaterialSearch').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var CompList = response.d;
                    $("#ddlMaterialID").empty();
                    if (CompList !== null && CompList !== undefined) {
                        CompList.forEach(function (value, index) {
                            $("#ddlMaterialID").append($("<option></option>").val(value).html(value));
                        });
                    }
                    $("#ddlMaterialID").change();
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
        }

        function BindPriorities() {
            debugger;
            var macId = $("#ddlMachineId").val();
            //if (macId.includes("Assembly")) {
            //    $.ajax({
            //        type: "POST",
            //        url: "ProductionScheduler.aspx/BindPriorities_A",
            //        contentType: "application/json; charset=utf-8",
            //        data: '{machine:"' + macId + '"}',
            //        dataType: "json",
            //        success: function (response) {
            //            var CompList = response.d;
            //            $("#ddlPriorities").empty();
            //            if (CompList !== null && CompList !== undefined) {
            //                CompList.forEach(function (value, index) {
            //                    $("#ddlPriorities").append($("<option></option>").val(value).html(value));
            //                });
            //            }
            //        },
            //        error: function (err) {
            //            alert('Error : ' + err);
            //        }
            //    });
            //} else {
            $.ajax({
                type: "POST",
                url: "ProductionScheduler.aspx/BindPriorities",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '"}',
                dataType: "json",
                success: function (response) {
                    var CompList = response.d;
                    $("#ddlPriorities").empty();
                    if (CompList !== null && CompList !== undefined) {
                        CompList.forEach(function (value, index) {
                            //$("#ddlPriorities").append($("<option></option>").val(value.SystemPriority).html(value.UserPriority));
                            $("#ddlPriorities").append($("<option></option>").val(value.SystemPriority).html(value.SystemPriority));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
            //  }


        }

        function BindSchedulePriorities() {
            debugger;
            var macId = $("#ddlMachineId").val();
            $.ajax({
                type: "POST",
                url: "ProductionScheduler.aspx/BindPriorities",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '"}',
                dataType: "json",
                success: function (response) {
                    var CompList = response.d;
                    $("#ddlAddPriority").empty();
                    $("#ddlAddPriority_A").empty();
                    if (CompList !== null && CompList !== undefined) {
                        CompList.forEach(function (value, index) {
                            debugger;
                            $("#ddlAddPriority").append($("<option></option>").val(value.SystemPriority).html(value.SystemPriority));
                            $("#ddlAddPriority_A").append($("<option></option>").val(value.SystemPriority).html(value.SystemPriority));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });

        }

        function BindAssemblySchedulePriorities() {
            debugger;
            var macId = $("#ddlMachineId").val();
            $.ajax({
                type: "POST",
                url: "ProductionScheduler.aspx/BindPriorities_A",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '"}',
                dataType: "json",
                success: function (response) {
                    var CompList = response.d;
                    $("#ddlAddPriority_A").empty();
                    if (CompList !== null && CompList !== undefined) {
                        CompList.forEach(function (value, index) {
                            debugger;
                            $("#ddlAddPriority_A").append($("<option></option>").val(value.SystemPriority).html(value.SystemPriority));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });

        }
        function BindActivityForUpdate(macId, materialID, activityList) {
            var counter = 0;
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/GetSubOperationsList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '",matId:"' + materialID + '", defualtoroptional: "Default"}',
                dataType: "json",
                success: function (response) {
                    var opnList = response.d;
                    $("#cbUpdateDefaultActivity").empty();
                    if (opnList !== null && opnList !== undefined) {
                        debugger;
                        $("#cbUpdateDefaultActivity tr").each(function () {
                            $(this).remove();
                        });
                        opnList.forEach(function (value, index) {
                            //$("#ddlOperationNum").append($("<option></option>").val(value).html(value));
                            let cbchecked = false;
                            value = value.trim();
                            if (activityList.includes(value)) {

                                cbchecked = true;
                            }
                            $("#cbUpdateDefaultActivity").append($('<tr></tr>').append($('<td></td>').append($('<input>').attr({
                                //type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter, checked: cbchecked, onclick: 'return false'
                                type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter, checked: cbchecked
                            })).append(
                                $('<label>').attr({
                                    for: 'chklistitem' + counter++
                                }).text(value))));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });

            $.ajax({
                type: "POST",
                url: "ProductionScheduler.aspx/GetSubOperationsList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '",matId:"' + materialID + '", defualtoroptional: "Optional"}',
                dataType: "json",
                success: function (response) {
                    var opnList = response.d;
                    $("#cbUpdateOptionalActivity").empty();
                    if (opnList !== null && opnList !== undefined) {
                        debugger;
                        $("#cbUpdateOptionalActivity tr").each(function () {
                            $(this).remove();
                        });
                        opnList.forEach(function (value, index) {
                            //$("#ddlOperationNum").append($("<option></option>").val(value).html(value));
                            let cbchecked = false;
                            //alert(activityList + "    "+ value);
                            debugger;
                            value = value.trim();
                            if (activityList.includes(value)) {
                                cbchecked = true;
                            }
                            $("#cbUpdateOptionalActivity").append($('<tr></tr>').append($('<td></td>').append($('<input>').attr({
                                type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter, checked: cbchecked
                            })).append(
                                $('<label>').attr({
                                    for: 'chklistitem' + counter++
                                }).text(value))));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
        }
        function BindAssemblySubOperation() {
            var macId = $("#ddlMachineId").val();
            var counter = 0;
            var matId = $("#ddlMaterialID_A").val();
            $("#cbDefaultActivities_A").each(function () {
                debugger;
            });
            $.ajax({
                type: "POST",
                url: "ProductionScheduler.aspx/GetSubOperationsList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '",matId:"' + matId + '", defualtoroptional: "Default"}',
                dataType: "json",
                success: function (response) {
                    var opnList = response.d;
                    $("#cbDefaultActivities_A").empty();
                    if (opnList !== null && opnList !== undefined) {
                        debugger;
                        $("#cbDefaultActivities_A tr").each(function () {
                            $(this).remove();
                        });
                        opnList.forEach(function (value, index) {
                            //$("#ddlOperationNum").append($("<option></option>").val(value).html(value));
                            $("#cbDefaultActivities_A").append($('<tr></tr>').append($('<td></td>').append($('<input>').attr({
                                //type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter, checked: 'true', onclick: 'return false'
                                type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter, checked: 'true'
                            })).append(
                                $('<label>').attr({
                                    for: 'chklistitem' + counter++
                                }).text(value))));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });

            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/GetSubOperationsList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '",matId:"' + matId + '", defualtoroptional: "Optional"}',
                dataType: "json",
                success: function (response) {
                    var opnList = response.d;
                    $("#cbOptionalActivities_A").empty();
                    if (opnList !== null && opnList !== undefined) {
                        debugger;
                        $("#cbOptionalActivities_A tr").each(function () {
                            $(this).remove();
                        });
                        opnList.forEach(function (value, index) {
                            //$("#ddlOperationNum").append($("<option></option>").val(value).html(value));
                            $("#cbOptionalActivities_A").append($('<tr></tr>').append($('<td></td>').append($('<input>').attr({
                                type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter
                            })).append(
                                $('<label>').attr({
                                    for: 'chklistitem' + counter++
                                }).text(value))));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
        }

        function BindOperation() {
            var macId = $("#ddlMachineId").val();
            var counter = 0;
            var matId = $("#ddlMaterialID").val();
            $("#MainContent_ddlOperationNum").each(function () {
                debugger;
            });
            $.ajax({
                async: false,
                type: "POST",
                url: "ProductionScheduler.aspx/GetOperationsList",
                contentType: "application/json; charset=utf-8",
                data: '{machine:"' + macId + '",matId:"' + matId + '"}',
                dataType: "json",
                success: function (response) {
                    var opnList = response.d;
                    $("#MainContent_ddlOperationNum").empty();
                    if (opnList !== null && opnList !== undefined) {
                        debugger;
                        $("#ddlOperationNum tr").each(function () {
                            $(this).remove();
                        });
                        opnList.forEach(function (value, index) {
                            //$("#ddlOperationNum").append($("<option></option>").val(value).html(value));
                            $("#ddlOperationNum").append($('<tr></tr>').append($('<td></td>').append($('<input>').attr({
                                type: 'checkbox', name: 'chklistitem', value: value, id: 'chklistitem' + counter
                            })).append(
                                $('<label>').attr({
                                    for: 'chklistitem' + counter++
                                }).text(value))));
                        });
                    }
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
        }




        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $(function () {
                $('[id*=lstStatus]').multiselect({
                    includeSelectAllOption: true
                });
                showHideColumn();
            });

            $('[id$=txtFromDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txtToDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txtRDDMachine_A]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txtUpdateRDDMachines_A]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtStartTime]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
                minDate: new Date()
            });

            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });

            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            $("#btnDownloadTemplate").click(function () {
                if (document.URL) {
                    let Url = document.URL;
                    var downloadUrl = false;
                    var process = getMachineIDProcess($("#ddlMachineId").val());
                    if (process.includes("Assembly") || process.includes("Testing") || process.includes("Packing") || process.includes("Stores")) {
                        downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/AssemblyImportPlanTemplate.xlsx';
                    }
                    else if ($("#ddlMachineId").val().includes("Quality In house") || $("#ddlMachineId").val().includes("Quality Incoming")) {
                        downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/QualityImportPlanTemplate.xlsx';
                    }
                    else {
                        downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/ImportPlanTemplate.xlsx';
                    }


                    if (downloadUrl) {
                        location.href = downloadUrl;
                    }
                    else {
                        alert('Import Template not found.');
                    }
                }
            });
            $("#btnUpdateActivity").click(function () {
                var default_sub_activity_checked = "", default_sub_activity_unchecked = "", optional_sub_activity_Checked = "", optional_sub_activity_Unchecked = "";
                $("#cbUpdateDefaultActivity tr td").each(function () {
                    debugger;
                    if ($(this).find('input[type=checkbox]')[0].checked) {
                        default_sub_activity_checked += $(this).find('input[type=checkbox]').val() + ",";
                    } else {
                        default_sub_activity_unchecked += $(this).find('input[type=checkbox]').val() + ",";
                    }
                });
                default_sub_activity_checked = default_sub_activity_checked.trimEnd(',');
                $('#hfUpdateDefaultActivityChekced').val(default_sub_activity_checked);

                default_sub_activity_unchecked = default_sub_activity_unchecked.trimEnd(',');
                $('#hfUpdateDefaultActivityUnChekced').val(default_sub_activity_unchecked);

                debugger;

                $("#cbUpdateOptionalActivity tr td").each(function () {
                    debugger;
                    if ($(this).find('input[type=checkbox]')[0].checked) {
                        optional_sub_activity_Checked += $(this).find('input[type=checkbox]').val() + ",";
                    } else {
                        optional_sub_activity_Unchecked += $(this).find('input[type=checkbox]').val() + ",";
                    }
                });
                optional_sub_activity_Checked = optional_sub_activity_Checked.trimEnd(',');
                $('#hfUpdateOptionalActivityChecked').val(optional_sub_activity_Checked);

                optional_sub_activity_Unchecked = optional_sub_activity_Unchecked.trimEnd(',');
                $('#hfUpdateOptionalActivityUnchecked').val(optional_sub_activity_Unchecked);
            });
            $("[id*=lbActivities]").click(function () {
                var macId = $("#ddlMachineId").val();
                var this_row = $(this).closest("tr");
                if (this_row) {
                    debugger;
                    var status = this_row.find('#lblStatus').text().trim();
                    if (status !== "" && status !== "Running" && status !== "Completed") {
                        var idd = this_row.find('#hdfUpdate').val();
                        var priority = this_row.find('#lblPriority').text().trim();
                        var prodOrder = this_row.find('#lblProdOrderNumber').text().trim();
                        var matId = this_row.find('#lblMaterialID').text().trim();
                        var opnNumber = this_row.find('#lblOpnNumber').text().trim();
                        var fabNumber = this_row.find('#lblFabricationNum').text().trim();
                        var activityList = this_row.find('#lbActivities').text().trim();
                        $("#hfUpdateActivityMachineId").val(macId);
                        $("#hfUpdateActivityOPNum").val(opnNumber);
                        $("#hfUpdateActivityPO").val(prodOrder);
                        $("#hfUpdateActivityFabNo").val(fabNumber);
                        $("#hfUpdateActivityMaterialID").val(matId);
                        $("#tblUpdateActivity").empty();
                        var tableContain = "<tbody><tr><th>Priority</th> <th>Prod. Order</th> <th>Material ID</th> <th>Operation Num</th><th>Fabrication Num</th></tr></tbody><tbody>";
                        tableContain += ('<tr><td>' + priority + '</td><td>' + prodOrder + '</td><td>' + matId + '</td><td>' + opnNumber + '</td><td>' + fabNumber + '</td></tr>');
                        tableContain += '</tbody>';
                        $("#tblUpdateActivity").append(tableContain);
                        debugger;
                        BindActivityForUpdate(macId, matId, activityList);
                        var title = "Update Activity";
                        $("#UpdateAssemblyActivityPopup .modal-title").html(title);
                        $("#UpdateAssemblyActivityPopup").modal("show");
                    } else {
                        var title = "Update Activity";
                        var body = "Cannot update running schedules.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                    }
                }
                else {
                    var title = "Update Activity";
                    var body = "Cannot update data due to some unknown error. Please try after sometime.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                }

            });
            $("#btnAddNewSchedule").click(function () {
                var process = getMachineIDProcess($("#ddlMachineId").val());
                if (process.includes("Stores") || process.includes("Assembly")) {
                    $("#txtProdOrder_A").val("");
                    $("#txtFabricationNumber_A").val("");
                    $("#txtLocalExport_A").val("");
                    $("#txtSaleOrder_A").val("");
                    $("#txtScrollWelded_A").val("");
                    $("#txtRDDMachine_A").val("");
                    $("#txtCustomer_A").val("");
                    $("#txtLocation_A").val("");
                    $("#txtQuantity_A").val(1);
                    $("#assemblystatusmessage").text("");
                    $("#radioMoveEnd_A").prop('checked', true);
                    $("#ddlAddPriority_A").prop("disabled", true);
                    var title = "Add New Schedule";
                    if (process.includes("Stores")) {
                        $('#txtOperationNo_A').val("9000");
                    }
                    else if (process.includes("Assembly")) {
                        $('#txtOperationNo_A').val("9001");
                    }
                    BindComponent();
                    //BindAssemblySchedulePriorities();
                    BindSchedulePriorities();
                    BindAssemblySubOperation();
                    $("#AddNewAssemblySchedulePopup .modal-title").html(title);
                    $("#AddNewAssemblySchedulePopup").modal("show");
                } else {
                    debugger;
                    $("#txtProdOrder").val("");
                    $("#txtQuantity").val(1);
                    $("#statusmessage").text("");
                    $("#radioMoveEnd").prop('checked', true);
                    $("#ddlAddPriority").prop("disabled", true);

                    var title = "Add New Schedule";
                    BindComponent();
                    BindSchedulePriorities();
                    BindOperation();
                    if ($("#ddlMachineId").val() == "Quality Incoming" || $("#ddlMachineId").val() == "Quality In house") {
                        $("#divAddNewProdDev").css('display', '');
                        $("#divAddSupplierCode").css('display', '');
                        $("#divAddGRNNumber").css('display', '');
                        /*$("#txtAddQuantity").css('disabled')*/
                        if ($("#ddlMachineId").val() == "Quality Incoming") {
                            $("#txtQuantity").val()
                            $("#txtQuantity").prop('disabled', false);
                        }
                        else
                            $("#txtQuantity").prop('disabled', true);
                    }
                    else {
                        $("#divAddNewProdDev").css('display', 'none');
                        $("#divAddSupplierCode").css('display', 'none');
                        $("#divAddGRNNumber").css('display', 'none');
                        $("#txtQuantity").prop('disabled', true);
                    }
                    $("#AddNewSchedulePopup .modal-title").html(title);
                    $("#AddNewSchedulePopup").modal("show");
                }
            });

            $("#btnCalculatePlan").click(function () {
                var rowCount = 0;
                if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                    $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {
                        var this_row = $(this);
                        if (this_row !== null & this_row !== undefined) {
                            rowCount += 1;
                        }
                    });
                } else {
                    $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                        var this_row = $(this);
                        if (this_row !== null & this_row !== undefined) {
                            rowCount += 1;
                        }
                    });
                }
                if (rowCount > 0) {
                    $("#radioRunningSchedule").prop('checked', true);
                    $("#txtStartTime").val("");
                    $("#txtStartTime").prop("disabled", true);
                    var title = "Calculate Schedule Plan";
                    $("#CalculatePlanPopup .modal-title").html(title);
                    $("#CalculatePlanPopup").modal("show");
                    return true;
                }
                else {
                    var title = "Calculate Schedule Plan";
                    var body = "Cannot calculate plan as no data is available.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
            });

            $("#btnCancel").click(function () {
                var selectedSchedules = [];
                var chkCount = 0;
                var notParkedCount = 0;
                if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                    $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {
                        var this_row = $(this);
                        var status = "";
                        if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                            status = this_row.find('#lblStatus').text().trim();
                        }
                        else {
                            status = this_row.find('#lblStatus').text().trim();
                        }
                        if (status !== "" && status !== "Running") {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    if (status !== "Parked") notParkedCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                }
                            }
                        }
                    });
                    if (chkCount > 0) {
                        if (notParkedCount == 0) {
                            $("#tblCancelSchedule tbody tr").not(':first').each(function () {
                                $(this).remove();
                            });
                            if (selectedSchedules.length > 0) {
                                $("#tblCancelSchedule").empty();
                                var tableContain = " <tbody> <tr><th>Priority</th>  <th>Prod. Order</th> <th>Material ID</th>  <th>Operation Num</th> </tr> </tbody>";
                                tableContain += "<tbody>";
                                //var tableContain = "<tbody>";
                                selectedSchedules.forEach(function (currentValue, index) {
                                    tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                                });
                                tableContain += '</tbody>';
                                $("#tblCancelSchedule").append(tableContain);
                                var title = "Cancel Parked Schedules";
                                $("#CancelSchedulePopup .modal-title").html(title);
                                $("#CancelSchedulePopup").modal("show");
                                return true;
                            }
                            else {
                                $('#tblCancelSchedule').append('<tr><td colspan="4" style="text-align: center;">No parked record selected for cancellation !!</td></tr>');
                                return false;
                            }
                        }
                        else {
                            var title = "Cancel Parked Schedules";
                            var body = "Only parked schedules can be cancelled. Please uncheck all the new or running schedules.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                            return false;
                        }
                    }
                    else {
                        var title = "Cancel Parked Schedules";
                        var body = "Cannot cancel schedules as as no data is selected.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                } else {
                    $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                        var this_row = $(this);
                        var status = this_row.find('#lblStatus').text().trim();
                        if (status !== "" && status !== "Running") {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    if (status !== "Parked") notParkedCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim(), feb_num: this_row.find('#lblFabricationNum').text().trim() });
                                }
                            }
                        }
                    });
                    if (chkCount > 0) {
                        if (notParkedCount == 0) {
                            $("#tblCancelSchedule tbody tr").not(':first').each(function () {
                                $(this).remove();
                            });
                            if (selectedSchedules.length > 0) {
                                $("#tblCancelSchedule").empty();
                                var tableContain = " <tbody> <tr><th>Priority</th>  <th>Prod. Order</th> <th>Material ID</th>  <th>Operation Num</th> <th>Febrication Num</th> </tr> </tbody>";
                                tableContain += "<tbody>";
                                //var tableContain = "<tbody>";
                                selectedSchedules.forEach(function (currentValue, index) {
                                    tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td><td>' + currentValue['feb_num'] + '</td></tr>');
                                });
                                tableContain += '</tbody>';
                                $("#tblCancelSchedule").append(tableContain);
                                var title = "Cancel Parked Schedules";
                                $("#CancelSchedulePopup .modal-title").html(title);
                                $("#CancelSchedulePopup").modal("show");
                                return true;
                            }
                            else {
                                $('#tblCancelSchedule').append('<tr><td colspan="4" style="text-align: center;">No parked record selected for cancellation !!</td></tr>');
                                return false;
                            }
                        }
                        else {
                            var title = "Cancel Parked Schedules";
                            var body = "Only parked schedules can be cancelled. Please uncheck all the new or running schedules.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                            return false;
                        }
                    }
                    else {
                        var title = "Cancel Parked Schedules";
                        var body = "Cannot cancel schedules as as no data is selected.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }


            });

            $("#btnDelete").click(function () {
                var selectedSchedules = [];
                var chkCount = 0;
                var parkedOrRunningCount = 0;

                if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                    $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {
                        debugger;
                        var this_row = $(this);
                        var status = "";
                        if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                            status = this_row.find('#lblStatus').text().trim();
                        }
                        else {
                            status = this_row.find('#lblStatus').text().trim();
                        }
                        if (status !== "" && status !== "Running" && status !== "Parked") {
                            if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                                var chkSelect = this_row.find('#chkSelect');
                                if (chkSelect !== null & chkSelect !== undefined) {
                                    if ($(chkSelect).is(':checked') == true) {
                                        chkCount += 1;
                                        if (status == "Running" || status == "Parked") parkedOrRunningCount += 1;
                                        selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                    }
                                }
                            }
                            else {
                                var chkSelect = this_row.find('#chkSelect');
                                if (chkSelect !== null & chkSelect !== undefined) {
                                    if ($(chkSelect).is(':checked') == true) {
                                        chkCount += 1;
                                        if (status == "Running" || status == "Parked") parkedOrRunningCount += 1;
                                        selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                    }
                                }
                            }
                        }
                    });
                    if (chkCount > 0) {
                        if (parkedOrRunningCount == 0) {
                            $("#tblDeleteSchedule tbody tr").not(':first').each(function () {
                                $(this).remove();
                            });
                            if (selectedSchedules.length > 0) {

                                $("#tblDeleteSchedule").empty();
                                var tableContain = "  <tbody> <tr> <th>Priority</th> <th>Prod. Order</th><th>Material ID</th> <th>Operation Num</th></tr></tbody>";
                                tableContain += "<tbody>";
                                // var tableContain = "<tbody>";
                                selectedSchedules.forEach(function (currentValue, index) {
                                    tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                                });
                                tableContain += '</tbody>';
                                $("#tblDeleteSchedule").append(tableContain);
                                var title = "Delete Schedule";
                                $("#DeleteSchedulePopup .modal-title").html(title);
                                $("#DeleteSchedulePopup").modal("show");
                                return true;
                            }
                            else {
                                $('#tblDeleteSchedule').append('<tr><td colspan="4" style="text-align: center;">No record selected for deletion !!</td></tr>');
                                return false;
                            }
                        }
                        else {
                            var title = "Delete Schedule";
                            var body = "Cannot delete running or parked records. Please uncheck running or parked records.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                            return false;
                        }
                    }
                    else {
                        var title = "Delete Schedule";
                        var body = "No schedule is selected for deletion.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                } else {

                    $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                        var this_row = $(this);
                        var status = this_row.find('#lblStatus').text().trim();
                        if (status !== "" && status !== "Running") {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    if (status == "Running" || status == "Parked") parkedOrRunningCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim(), feb_num: this_row.find('#lblFabricationNum').text().trim() });
                                }
                            }
                        }
                    });
                    if (chkCount > 0) {
                        if (parkedOrRunningCount == 0) {
                            $("#tblDeleteSchedule tbody tr").not(':first').each(function () {
                                $(this).remove();
                            });
                            if (selectedSchedules.length > 0) {
                                $("#tblDeleteSchedule").empty();
                                var tableContain = "  <tbody> <tr> <th>Priority</th> <th>Prod. Order</th><th>Material ID</th> <th>Operation Num</th><th>Febrication Num</th></tr></tbody>";
                                tableContain += "<tbody>";
                                //var tableContain = "<tbody>";
                                selectedSchedules.forEach(function (currentValue, index) {
                                    tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td><td>' + currentValue['feb_num'] + '</td></tr>');
                                });
                                tableContain += '</tbody>';
                                $("#tblDeleteSchedule").append(tableContain);
                                var title = "Delete Schedule";
                                $("#DeleteSchedulePopup .modal-title").html(title);
                                $("#DeleteSchedulePopup").modal("show");
                                return true;
                            }
                            else {
                                $('#tblDeleteSchedule').append('<tr><td colspan="4" style="text-align: center;">No record selected for deletion !!</td></tr>');
                                return false;
                            }
                        }
                        else {
                            var title = "Delete Schedule";
                            var body = "Cannot delete running or parked records. Please uncheck running or parked records.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                            return false;
                        }
                    }
                    else {
                        var title = "Delete Schedule";
                        var body = "No schedule is selected for deletion.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
            });

            $("#btnChangePriority").click(function () {
                debugger;
                var selectedSchedules = [];
                var chkCount = 0;
                if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                    $("#MainContent_lstProdScheduler_tblProdScheduler tr").not(':first').not(':last').each(function () {
                        var this_row = $(this);
                        var status = "";
                        if ($("#ddlMachineId").val() == "Quality In house" || $("#ddlMachineId").val() == "Quality Incoming") {
                            status = this_row.find('#lblStatus').text().trim();
                            if (status !== "" && status !== "Running") {
                                var chkSelect = this_row.find('#chkSelect');
                                if (chkSelect !== null & chkSelect !== undefined) {
                                    if ($(chkSelect).is(':checked') == true) {
                                        chkCount += 1;
                                        selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), UserPriority: this_row.find('#lblUserPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                    }
                                }
                            }
                        }
                        else {
                            status = this_row.find('#lblStatus').text().trim();
                            if (status !== "" && status !== "Running") {
                                var chkSelect = this_row.find('#chkSelect');
                                if (chkSelect !== null & chkSelect !== undefined) {
                                    if ($(chkSelect).is(':checked') == true) {
                                        chkCount += 1;
                                        selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), UserPriority: this_row.find('#lblUserPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                    }
                                }
                            }
                        }
                    });
                    if (chkCount > 0) {
                        $("#tblChangePriority tbody tr").not(':first').each(function () {
                            $(this).remove();
                        });
                        BindPriorities()
                        if (selectedSchedules.length > 0) {
                            var tableContain = "<tbody>";
                            selectedSchedules.forEach(function (currentValue, index) {
                                tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td style="display:none">' + currentValue['UserPriority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                            });
                            tableContain += '</tbody>';
                            $("#tblChangePriority").append(tableContain);
                            $("#radioMoveToEnd").prop('checked', true);
                            $("#ddlPriorities").prop("disabled", true);
                            var title = "Change Schedule Priority";
                            $("#ChangePriorityPopup .modal-title").html(title);
                            $("#ChangePriorityPopup").modal("show");
                            return true;
                        }
                        else {
                            $('#tblChangePriority').append('<tr><td colspan="4" style="text-align: center;">No record selected for priority change !!</td></tr>');
                            return false;
                        }
                    }
                    else {
                        var title = "Change Schedule Priority";
                        var body = "No schedule is selected for priority change.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
                else {
                    $("#MainContent_lvAssemblyDetails_tblAssemblyDetails tr").not(':first').not(':last').each(function () {
                        var this_row = $(this);
                        var status = this_row.find('#lblStatus').text().trim();
                        if (status !== "" && status !== "Running") {
                            var chkSelect = this_row.find('#chkSelect');
                            if (chkSelect !== null & chkSelect !== undefined) {
                                if ($(chkSelect).is(':checked') == true) {
                                    chkCount += 1;
                                    selectedSchedules.push({ priority: this_row.find('#lblPriority').text().trim(), UserPriority: this_row.find('#lblUserPriority').text().trim(), prod_order: this_row.find('#lblProdOrderNumber').text().trim(), material_id: this_row.find('#lblMaterialID').text().trim(), opn_number: this_row.find('#lblOpnNumber').text().trim() });
                                }
                            }
                        }
                    });
                    if (chkCount > 0) {
                        $("#tblChangePriority tbody tr").not(':first').each(function () {
                            $(this).remove();
                        });
                        BindPriorities()
                        if (selectedSchedules.length > 0) {
                            var tableContain = "<tbody>";
                            selectedSchedules.forEach(function (currentValue, index) {
                                tableContain += ('<tr><td>' + currentValue['priority'] + '</td><td style="display:none">' + currentValue['UserPriority'] + '</td><td>' + currentValue['prod_order'] + '</td><td>' + currentValue['material_id'] + '</td><td>' + currentValue['opn_number'] + '</td></tr>');
                            });
                            tableContain += '</tbody>';
                            $("#tblChangePriority").append(tableContain);
                            $("#radioMoveToEnd").prop('checked', true);
                            $("#ddlPriorities").prop("disabled", true);
                            var title = "Change Schedule Priority";
                            $("#ChangePriorityPopup .modal-title").html(title);
                            $("#ChangePriorityPopup").modal("show");
                            return true;
                        }
                        else {
                            $('#tblChangePriority').append('<tr><td colspan="4" style="text-align: center;">No record selected for priority change !!</td></tr>');
                            return false;
                        }
                    }
                    else {
                        var title = "Change Schedule Priority";
                        var body = "No schedule is selected for priority change.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }

            });

            $("#ddlMaterialID").change(function () {
                BindOperation();
            });
            $("#ddlMaterialID_A").change(function () {

                BindAssemblySubOperation();
            });
            $("#btnImport").click(function () {
                var dd = $("#FileUploadSchedule")[0].files.length;
                if (dd == 0) {
                    var title = "Import Schedule";
                    var body = "Cannot import as no file is chosen for import.";
                    $("#ScheduleErrorPopup .modal-title").html(title);
                    $("#ScheduleErrorPopup .modal-body").html(body);
                    $("#ScheduleErrorPopup").modal("show");
                    return false;
                }
                else {
                    var filename = $("#FileUploadSchedule")[0].files[0].name;
                    if (filename !== null && filename !== undefined) {
                        var file_extension = filename.split('.').pop();
                        if (file_extension !== null && file_extension !== undefined) {
                            if (file_extension !== "xlsx") {
                                var title = "Import Schedule";
                                var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                                $("#ScheduleErrorPopup .modal-title").html(title);
                                $("#ScheduleErrorPopup .modal-body").html(body);
                                $("#ScheduleErrorPopup").modal("show");
                                return false;
                            }
                            else {
                                var title = "Import Schedule";
                                $("#lblFilename").text(filename);
                                $("#ScheduleImportPopup .modal-title").html(title);
                                $("#ScheduleImportPopup").modal("show");
                                return true;
                            }
                        }
                        else {
                            var title = "Import Schedule";
                            var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                            return false;
                        }
                    }
                    else {
                        var title = "Import Schedule";
                        var body = "Cannot import as no file is chosen for import.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                        return false;
                    }
                }
            });

            $("#btnSave").click(function () {
                debugger;
                var process = getMachineIDProcess($("#ddlMachineId").val());
                var prod_order = $("#txtProdOrder").val();
                var material_id = $("#ddlMaterialID").val();
                var opn_num = $("#ddlOperationNum").val();
                var qty = $("#txtQuantity").val();
                var autoOpnNo = "";
                $("#ddlOperationNum tr td").each(function () {
                    if ($(this).find('input[type=checkbox]')[0].checked) {
                        opn_num += $(this).find('input[type=checkbox]').val() + ",";
                        if (process.toLowerCase() == "qualityinhouse" && isAutoScheduleMachineAssigned(material_id, $(this).find('input[type=checkbox]').val()) == false) {
                            autoOpnNo = $(this).find('input[type=checkbox]').val();
                        }
                    }
                });
                if (autoOpnNo != "") {
                    $("#statusmessage").text("* Please assign Auto Schedule Machine.");
                    return false;
                }
                opn_num = opn_num.trimEnd(',');
                $('#HidOpn').val(opn_num);

                if (prod_order.trim().length == 0) {
                    $("#statusmessage").text("* Production Order is mandatory.");
                    return false;
                }
                else if (material_id == null || material_id.trim().length == 0) {
                    $("#statusmessage").text("* Material ID is mandatory.");
                    return false;
                }
                else if (opn_num == null || opn_num.trim().length == 0) {
                    $("#statusmessage").text("* Operation number is mandatory.");
                    return false;
                }
                else if (qty.trim().length == 0) {
                    $("#statusmessage").text("* Quantity is mandatory.");
                    return false;
                }
                else if (qty.trim() < 1) {
                    $("#statusmessage").text("* Quantity must be greater than 0.");
                    return false;
                }
                else {
                    var macId = $("#ddlMachineId").val().trim();
                    $('#hdnMaterialId').val(material_id);
                    $.ajax({
                        type: "POST",
                        url: "ProductionScheduler.aspx/IsScheduleExists",
                        contentType: "application/json; charset=utf-8",
                        data: '{machine:"' + macId + '",prodOrder:"' + prod_order + '",matId:"' + material_id + '",opnNum:"' + opn_num + '"}',
                        dataType: "json",
                        success: function (response) {
                            var res = response.d;
                            if (res == "true") {
                                $("#statusmessage").text("* This machine, prod order, material id and operation combination already exists.");
                                return false;
                            }
                        },
                        error: function (err) {
                            alert('Error : ' + err);
                        }
                    });
                    return res !== "true";
                }
            });
            $("#btnSaveNewAssembly").click(function () {
                debugger;
                var opn_num = $("#txtOperationNo_A").val();
                var rddMachine = $("#txtRDDMachine_A").val();
                var prod_order = $("#txtProdOrder_A").val();
                var material_id = $("#ddlMaterialID_A").val();
                var fabricationNo = $("#txtFabricationNumber_A").val();
                var process = getMachineIDProcess($("#ddlMachineId").val());
                if (process.includes("Stores") && isAutoScheduleMachineAssigned(material_id, opn_num) == false) {
                    $("#assemblystatusmessage").text("Please assign Auto Schedule Machine.");
                    return false;
                }
                var SaleOrder = $("#txtSaleOrder_A").val();
                var Location = $("#txtLocation_A").val();
                var LocalExport = $("#txtLocalExport_A").val();
                var ScrollNumber = $("#txtScrollWelded_A").val();
                var Customer = $("#txtCustomer_A").val();

                var default_sub_activity = "", optional_sub_activity = "";
                $("#cbDefaultActivities_A tr td").each(function () {
                    debugger;
                    if ($(this).find('input[type=checkbox]')[0].checked) {
                        default_sub_activity += $(this).find('input[type=checkbox]').val() + ",";
                    }
                });
                default_sub_activity = default_sub_activity.trimEnd(',');
                $('#hfDefaultActivity').val(default_sub_activity);

                $("#cbOptionalActivities_A tr td").each(function () {
                    debugger;
                    if ($(this).find('input[type=checkbox]')[0].checked) {
                        optional_sub_activity += $(this).find('input[type=checkbox]').val() + ",";
                    }
                });
                optional_sub_activity = optional_sub_activity.trimEnd(',');
                $('#hfOptionalActivity').val(optional_sub_activity);

                var qty = $("#txtQuantity_A").val();
                if (prod_order.trim().length == 0) {
                    $("#assemblystatusmessage").text("* Production Order is mandatory.");
                    return false;
                }
                else if (material_id == null || material_id.trim().length == 0) {
                    $("#assemblystatusmessage").text("* Material ID is mandatory.");
                    return false;
                }
                else if (fabricationNo.trim().length == 0) {
                    $("#assemblystatusmessage").text("* Fabrication Number is mandatory.");
                    return false;
                }

                else if (SaleOrder.trim().length == 0) {
                    $("#assemblystatusmessage").text("* SaleOrder is mandatory.");
                    return false;
                }
                else if (Location.trim().length == 0) {
                    $("#assemblystatusmessage").text("* Location is mandatory.");
                    return false;
                }
                else if (LocalExport.trim().length == 0) {
                    $("#assemblystatusmessage").text("* LocalExport is mandatory.");
                    return false;
                }
                else if (ScrollNumber.trim().length == 0) {
                    $("#assemblystatusmessage").text("* Machine/Scroll/Bowl Number is mandatory.");
                    return false;
                }
                else if (Customer.trim().length == 0) {
                    $("#assemblystatusmessage").text("* Customer is mandatory.");
                    return false;
                }


                //if (opn_num == null || opn_num.trim().length == 0) {
                //    $("#assemblystatusmessage").text("* Sub Operation is mandatory.");
                //    return false;
                //}
                else if (rddMachine != "") {
                    if (!moment(rddMachine, 'DD-MM-YYYY', true).isValid()) {
                        $("#assemblystatusmessage").text("* RDD Machine is not in correct format.");
                        return false;
                    }
                }
                else
                    if (qty.trim().length == 0) {
                        $("#assemblystatusmessage").text("* Quantity is mandatory.");
                        return false;
                    }
                    else if (qty.trim() < 1) {
                        $("#assemblystatusmessage").text("* Quantity must be greater than 0.");
                        return false;
                    }
                    else {
                        var macId = $("#ddlMachineId").val().trim();
                        $.ajax({
                            type: "POST",
                            url: "ProductionScheduler.aspx/IsAssemblyScheduleExists",
                            contentType: "application/json; charset=utf-8",
                            data: '{machine:"' + macId + '",prodOrder:"' + prod_order + '",matId:"' + material_id + '",opnNum:"' + opn_num + '", fabricationNo:"' + fabricationNo + '"}',
                            dataType: "json",
                            success: function (response) {
                                var res = response.d;
                                if (res == "true") {
                                    $("#statusmessage").text("* This machine, prod order, material id, operation and fabrication number combination already exists.");
                                    return false;
                                }
                            },
                            error: function (err) {
                                alert('Error : ' + err);
                            }
                        });
                        return res !== "true";
                    }
            });
            $("*[id*=btnEdit]").click(function () {
                debugger;
                if ($("#MainContent_lstProdScheduler_tblProdScheduler").length > 0) {
                    var this_row = $(this).closest("tr");
                    if (this_row) {
                        var status = "";
                        var MachineID = $("#ddlMachineId").val();
                        if (MachineID == "Quality In house" || MachineID == "Quality Incoming")
                            status = this_row.find('#lblStatus').text().trim();
                        else
                            status = this_row.find('#lblStatus').text().trim();
                        if (status !== "" && status !== "Running" && status !== "Completed") {
                            var idd = this_row.find('#hdfUpdate').val();
                            var prodOrder = this_row.find('#lblProdOrderNumber').text().trim();
                            var matId = this_row.find('#lblMaterialID').text().trim();
                            var opnNumber = this_row.find('#lblOpnNumber').text().trim();
                            var schQty = this_row.find('#lblQuantity').text().trim();
                            BindComponent();

                            $("#ddlUpdtMatId").val(matId);
                            getOperationForMatIdAndMachine(opnNumber);
                            $("#statusmsg").text("");
                            $("#hdfIdd").val(idd);
                            $("#txtUpdtProdOrder").val(prodOrder);
                            $("#hdfProd").val(prodOrder);
                            $("#hdfMat").val(matId);
                            $("#txtUpdtQuantity").val(1);
                            $("#ddlUpdtOpnNum").val(opnNumber);
                            if (MachineID == "Quality In house" || MachineID == "Quality Incoming") {
                                $("#divUpdateGRNNumber").css('display', '');
                                $("#divUpdateSupplierName").css('display', '');
                                $("#divUpdateNewProdDev").css('display', '');
                                $("#txtUpdateGRNNumber").val(this_row.find('#lblGRNNumber').text().trim());
                                $('#hdnGRNNumber').val($("#txtUpdateGRNNumber").val());
                                $("#txtUpdateSupplierName").val(this_row.find('#lblSupplierName').text().trim());
                                $("#chkUpdateNewProdDev").prop('checked', (this_row.find('#chkCheckBox').is(':checked')));
                                if (MachineID == "Quality Incoming") {
                                    $("#txtUpdtQuantity").val(this_row.find('#lblQuantity').text().trim());
                                    $("#txtUpdtQuantity").prop('disabled', false);
                                }
                                else
                                    $("#txtUpdtQuantity").prop('disabled', true);
                            }
                            else {
                                $("#divUpdateGRNNumber").css('display', 'none');
                                $("#divUpdateSupplierName").css('display', 'none');
                                $("#divUpdateNewProdDev").css('display', 'none');
                            }
                            var title = "Update Schedule";
                            $("#UpdateSchedulePopup .modal-title").html(title);
                            $("#UpdateSchedulePopup").modal("show");
                        }
                        else {
                            var title = "Update Schedule";
                            var body = "Cannot update running schedules.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                        }
                    }
                    else {
                        var title = "Update Schedule";
                        var body = "Cannot update data due to some unknown error. Please try after sometime.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                    }
                } else {
                    var this_row = $(this).closest("tr");
                    if (this_row) {
                        var status = this_row.find('#lblStatus').text().trim();
                        if (status !== "" && status !== "Running" && status !== "Completed") {
                            var idd = this_row.find('#hdfUpdate').val();
                            var prodOrder = this_row.find('#lblProdOrderNumber').text().trim();
                            var matId = this_row.find('#lblMaterialID').text().trim();
                            var opnNumber = this_row.find('#lblOpnNumber').text().trim();
                            var fabNumber = this_row.find('#lblFabricationNum').text().trim();

                            var LocalExport = this_row.find('#lblModel').text().trim();
                            var SaleOrder = this_row.find('#lblSO').text().trim();
                            var ScrollNumber = this_row.find('#lblScrollNumber').text().trim();
                            var RDDMachine = this_row.find('#lblRDDMachine').text().trim();
                            var Customer = this_row.find('#lblCustomer').text().trim();
                            var Location = this_row.find('#lblLocation').text().trim();
                            // var schQty = this_row.find('td:eq(7)').find('.textboxcommon').text().trim();
                            BindComponent();
                            $("#ddlUpdtMatId_A").val(matId);
                            // getSubOperationForMatIdAndMachine(opnNumber);
                            $("#assemblystatusmsg").text("");
                            $("#hdfIdd_A").val(idd);
                            $("#txtUpdtProdOrder_A").val(prodOrder);
                            $("#hdfProd_A").val(prodOrder);
                            $("#hdfMat_A").val(matId);
                            $("#txtUpdtQuantity_A").val(1);
                            $("#txtUpdtOperationNum_A").val(opnNumber);
                            $("#hfUpdtFabricationNo_A").val(fabNumber);

                            $("#txtUpdaterFabricationNum_A").val(fabNumber);
                            $("#txtUpdateLocal_A").val(LocalExport);
                            $("#txtUpdateSaleOrder_A").val(SaleOrder);
                            $("#txtUpdateScrollNum_A").val(ScrollNumber);
                            $("#txtUpdateRDDMachines_A").val(RDDMachine);
                            $("#txtUpdateCustomer_A").val(Customer);
                            $("#txtUpdateLocation_A").val(Location);
                            // $("#ddlUpdtSubOperationNum_A").val(opnNumber);
                            var title = "Update Schedule";
                            $("#UpdateAssemblySchedulePopup .modal-title").html(title);
                            $("#UpdateAssemblySchedulePopup").modal("show");
                        }
                        else {
                            var title = "Update Schedule";
                            var body = "Cannot update running schedules.";
                            $("#ScheduleErrorPopup .modal-title").html(title);
                            $("#ScheduleErrorPopup .modal-body").html(body);
                            $("#ScheduleErrorPopup").modal("show");
                        }
                    }
                    else {
                        var title = "Update Schedule";
                        var body = "Cannot update data due to some unknown error. Please try after sometime.";
                        $("#ScheduleErrorPopup .modal-title").html(title);
                        $("#ScheduleErrorPopup .modal-body").html(body);
                        $("#ScheduleErrorPopup").modal("show");
                    }
                }

            });

            $("#ddlUpdtMatId").change(getOperationForMatIdAndMachine);
            //$("#ddlUpdtMatId_A").change(getSubOperationForMatIdAndMachine);
            $("#btnUpdate").click(function () {
                if ($("#ddlUpdtOpnNum").val())
                    return true;
                else {
                    $("#statusmsg").css('color', 'red');
                    $("#statusmsg").text("* Operation number cannot be empty.");
                    return false;
                }
            });
            $("#btnUpdateAssembly").click(function () {
                if ($("#txtUpdtProdOrder_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* Production order cannot be empty.");
                    return false;
                }
                if ($("#txtUpdaterFabricationNum_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* Fabrication Number cannot be empty.");
                    return false;
                }
                if ($("#txtUpdateLocal_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* Local Export cannot be empty.");
                    return false;
                }
                if ($("#txtUpdateSaleOrder_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* Sale order cannot be empty.");
                    return false;
                }
                if ($("#txtUpdateScrollNum_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* Machine/Scroll/Bowl Number cannot be empty.");
                    return false;
                }
                if ($("#txtUpdateRDDMachines_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* RDD Machines cannot be empty.");
                    return false;
                }
                else if ($("#txtUpdateRDDMachines_A").val() != "") {
                    if (!moment($("#txtUpdateRDDMachines_A").val(), 'DD-MM-YYYY', true).isValid()) {
                        $("#assemblystatusmessage").text("* RDD Machine is not in correct format.");
                        return false;
                    }
                }
                if ($("#txtUpdateCustomer_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* Customer cannot be empty.");
                    return false;
                }
                if ($("#txtUpdateLocation_A").val() == "") {
                    $("#assemblystatusmsg").css('color', 'red');
                    $("#assemblystatusmsg").text("* Location cannot be empty.");
                    return false;
                } else {
                    return true;
                }
            });
            $("#txtProdOrder").focusout(function () {
                if ($("#txtProdOrder").val().length == 0) {
                    $("#statusmessage").text("* Production Order is mandatory.");
                    return false;
                }
                else {
                    $("#statusmessage").text("");
                    return true;
                }
            });
            $("#txtProdOrder_A").focusout(function () {
                if ($("#txtProdOrder_A").val().length == 0) {
                    $("#assemblystatusmessage").text("* Production Order is mandatory.");
                    return false;
                }
                else {
                    $("#assemblystatusmessage").text("");
                    return true;
                }
            });

            $("#txtFabricationNumber_A").focusout(function () {
                if ($("#txtFabricationNumber_A").val().length == 0) {
                    $("#assemblystatusmessage").text("* Fabrication Number is mandatory.");
                    return false;
                }
                else {
                    $("#assemblystatusmessage").text("");
                    return true;
                }
            });
            $("#txtQuantity").focusout(function () {
                if ($("#txtQuantity").val().length == 0) {
                    $("#statusmessage").text("* Quantity is mandatory.");
                    return false;
                }
                else if ($("#txtQuantity").val().trim() < 1) {
                    $("#statusmessage").text("* Quantity must be greater than 0.");
                    return false;
                }
                else {
                    $("#statusmessage").text("");
                    return true;
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

            $("#radioMoveEnd").change(function () {
                if ($(this)[0].checked == true) {
                    $("#ddlAddPriority").prop("disabled", true);
                }
                else {
                    $("#ddlAddPriority").prop("disabled", false);
                }
            });

            $("#radioMoveBefore").change(function () {
                if ($(this)[0].checked == true) {
                    $("#ddlPriorities").prop("disabled", false);
                }
                else {
                    $("#ddlPriorities").prop("disabled", true);
                }
            });

            $("#radioMove2Before").change(function () {
                if ($(this)[0].checked == true) {
                    $("#ddlAddPriority").prop("disabled", false);
                }
                else {
                    $("#ddlAddPriority").prop("disabled", true);
                }
            });

            $("#radioMove2Before_A").change(function () {
                if ($(this)[0].checked == true) {
                    $("#ddlAddPriority_A").prop("disabled", false);
                }
                else {
                    $("#ddlAddPriority_A").prop("disabled", true);
                }
            });
            $("#radioMoveEnd_A").change(function () {
                if ($(this)[0].checked == true) {
                    $("#ddlAddPriority_A").prop("disabled", true);
                }
                else {
                    $("#ddlAddPriority_A").prop("disabled", false);
                }
            });

            $("#radioRunningSchedule").change(function () {
                if ($(this)[0].checked == true) {
                    $("#txtStartTime").prop("disabled", true);
                    $("#txtStartTime").val("");
                }
                else {
                    $("#txtStartTime").prop("disabled", false);
                }
            });

            $("#radioUserInput").change(function () {
                if ($(this)[0].checked == true) {
                    $("#txtStartTime").prop("disabled", false);
                    $("#txtStartTime").val(formattedTimestamp());
                }
                else {
                    $("#txtStartTime").prop("disabled", true);
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
