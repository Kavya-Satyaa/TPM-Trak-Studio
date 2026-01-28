<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionReportOfShaftTransaction_Highway.aspx.cs" Inherits="Web_TPMTrakDashboard.HighWay.InspectionReportOfShaftTransaction_Highway" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblSettings {
            width: 100%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 10px;
                border-collapse: collapse;
                text-align: center;
                font-size: large;
                position: sticky;
                top: -1px;
            }

        .lvApproveDetails {
            width: 100%;
            margin: 10px;
        }

            .lvApproveDetails > tbody > tr > td {
                border: 1px solid #b6b6b6;
                padding: 5px;
                text-align:center;
            }

            .lvApproveDetails > tbody > tr:nth-child(even) > td {
                background-color: white;
            }

            .lvApproveDetails > tbody > tr:nth-child(odd) > td {
                background-color: #ddd;
            }

            .lvApproveDetails > tbody > tr > th {
                border: 1px solid #b6b6b6;
                height: 40px;
                text-align: center;
            }

        .inner-tbl {
            width: 100%;
            height: 100%;
        }

            .inner-tbl > tbody > tr > td {
                border-right: 1px solid #b6b6b6;
                height: 40px;
                min-width: 200px;
                max-width: 200px;
                padding: 5px;
                text-align: center;
                overflow-wrap: break-word;
            }

            .inner-tbl > tbody > tr > th {
                border-right: 1px solid #b6b6b6;
                height: 40px;
                min-width: 200px;
                max-width: 200px;
                text-align: center;
                max-height: 100px;
            }

        .approveRowtr td {
            position: sticky;
            bottom: -1px;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>
                    <table class="tblSettings">
                        <tr>
                            <td>Machine ID</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlMachineID" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>Component ID</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlComponentID" AutoPostBack="true" OnSelectedIndexChanged="ddlComponentID_SelectedIndexChanged"></asp:DropDownList>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblComponent" ></asp:Label>--%>
                            </td>
                            <td>Operation No</td>
                            <td>
                                <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblOperation"></asp:Label>--%>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlOperationNo" AutoPostBack="true" OnSelectedIndexChanged="ddlOperationNo_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>Date</td>
                            <%-- <td class="input-group" runat="server">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control Date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>--%>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control Date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" ID="btnExport" Text="Export" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>Shift</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlShift" AutoPostBack="true" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>Die No</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlDieNo"></asp:DropDownList>
                            </td>
                            <td>Heat No</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlHeatNo"></asp:DropDownList>
                            </td>
                            <td>Rev ID</td>
                            <td>
                                <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="form-control" ID="ddlRevID"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" ID="btnView" Text="VIEW" OnClientClick="return ViewClick();" OnClick="btnView_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row" style="height: 80vh; overflow: auto;">
                    <asp:ListView runat="server" ID="lvApproveDetails" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="lvApproveDetails headerFixer" id="lvApproveDetails">
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" visible='<%# Eval("HeaderVisibility") %>'>
                                <th>
                                    <asp:Label runat="server" Text="Sl No"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" Text="Bal No."></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" Text="Characteristic ID"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" Text="Description"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" Text="Specification"></asp:Label>
                                </th>
                                <th>
                                    <asp:Label runat="server" Text="Inspection Method"></asp:Label>
                                </th>
                                <th colspan="2">
                                    <asp:Label runat="server" Text="Frequency"></asp:Label>
                                </th>

                                <th style="padding: 0px !important;" runat="server">
                                    <asp:ListView runat="server" DataSource='<%# Eval("listofTime") %>'>
                                        <LayoutTemplate>
                                            <table class="inner-tbl">
                                                <tr>
                                                    <th runat="server" id="itemplaceholder"></th>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <th>
                                                <%--<asp:Label runat="server" Text='<%# Eval("TimeValue") %>'></asp:Label>--%>
                                                <asp:Label runat="server">
                                                     <p style="margin: 0;"><%# Eval("TimeValue") %></p>
                                                     <p style="margin: 0;"><%# Eval("DieHeatValue") %></p>
                                                </asp:Label>
                                            </th>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </th>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ContentVisibility") %>'>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("SlNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("BalNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("CharacteristicID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("Specification") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("InspectionMethod") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("Frequency") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text='<%# Eval("FrequencyQty") %>'></asp:Label>
                                </td>
                                <td style="padding: 0px !important;">
                                    <asp:ListView runat="server" DataSource='<%# Eval("listofTime") %>'>
                                        <LayoutTemplate>
                                            <table class="inner-tbl">
                                                <tr>
                                                    <td runat="server" id="itemplaceholder"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td>
                                                <asp:Label runat="server" Text='<%# Eval("TimeValue") %>'></asp:Label>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr class="trRemarksRow" runat="server" visible='<%# Eval("FooterVisisbility") %>'>
                                <td colspan='<%# Eval("RemarksColspan") %>'>
                                    <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("Remarks") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr class="approveRowtr" runat="server" visible='<%# Eval("FooterVisisbility") %>'>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" Text="QA Inspector"></asp:Label>
                                </td>
                                <td colspan='<%# Eval("InspectionColspan") %>'>
                                    <%--<asp:Label runat="server" ClientIDMode="Static" Text='<%#Eval("InspectedBy") %>'></asp:Label>--%>
                                    <asp:Button runat="server" ID="btnInspectorApprove" Text="Approve" ClientIDMode="Static" Visible='<%# Eval("InspectorButtonVisibility") %>' CssClass="bajaj-btn-style btnClass" OnClick="btnInspectorApprove_Click" />
                                    <asp:Label runat="server" Text='<%# Eval("InspectedBy") %>' Visible='<%# Convert.ToBoolean(Eval("InspectorButtonVisibility")) == true ? false : true %>' Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" Text="H.O.D (Production)"></asp:Label>
                                </td>
                                <td colspan='<%# Eval("InspectionColspan") %>'>
                                    <asp:Button runat="server" ID="btnProductionApprove" Text="Approve" ClientIDMode="Static" Visible='<%# Eval("ProdButtonVisibility") %>' CssClass="bajaj-btn-style btnClass" OnClick="btnProductionApprove_Click" />
                                    <asp:Label runat="server" Text='<%# Eval("ProductionHOD") %>' Visible='<%# Convert.ToBoolean(Eval("ProdButtonVisibility")) == true ? false : true %>' Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ClientIDMode="Static" Text="H.O.D (QA)"></asp:Label>
                                </td>
                                <td colspan='<%# Eval("InspectionColspan") %>'>
                                    <asp:Button runat="server" ID="btnQAApprove" Text="Approve" ClientIDMode="Static" Visible='<%# Eval("QAButtonVisibility") %>' CssClass="bajaj-btn-style btnClass" OnClick="btnQAApprove_Click" />
                                    <asp:Label runat="server" Text='<%# Eval("QAHOD") %>' Visible='<%# Convert.ToBoolean(Eval("QAButtonVisibility")) == true ? false : true %>' Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

                <div class="modal fade" id="ConfirmModal" role="dialog">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content modalContent confirm-modal-content">
                            <div class="modal-header modalHeader confirm-modal-header">
                                <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                                <br />
                                <h4 class="confirm-modal-title">Confirmation!</h4>
                                <br />
                                <span id="DeleteMsg" class="confirm-modal-msg">Are you sure you want to Approve Records?</span>
                            </div>
                            <div class="modal-footer modalFooter modal-footer">
                                <asp:Button runat="server" Text="Yes" ID="btnApproveConfirm" CssClass="confirm-modal-btn" OnClick="btnApproveConfirm_Click" ClientIDMode="Static" />
                                <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            freezeColumnFromLeft("lvApproveDetails", "7");
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        });
        //$("#btnProductionApprove").on("click", function () {
        //    $("#ConfirmModal").modal("show");
        //    return false;
        //})
        //$("#btnQAApprove").on("click", function () {
        //    $("#ConfirmModal").modal("show");
        //    return false;
        //})
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            freezeColumnFromLeft("lvApproveDetails", "7");
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            }); 
            $.unblockUI({});
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
