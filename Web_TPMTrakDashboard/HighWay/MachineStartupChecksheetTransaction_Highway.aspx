<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineStartupChecksheetTransaction_Highway.aspx.cs" Inherits="Web_TPMTrakDashboard.HighWay.MachineStartupChecksheetTransaction_Highway" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblSettings {
            width: 97%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 10px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: large;
                /*box-shadow: 2px 2px 2px black;*/
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .lvApprovalGrid {
            width: 100%;
        }

            .lvApprovalGrid > tbody > tr > td {
                height: 30px;
                border-right: 1px solid grey;
                padding: 0px 10px;
            }

            .lvApprovalGrid > tbody > tr:first-child > td {
                height: 40px;
                border: 1px solid #ddd;
                color: white;
                text-align: center;
                padding: 0px !important;
                position: sticky;
                top: -1px;
                z-index: 4;
            }

            .lvApprovalGrid > tbody > tr:nth-child(2) > td {
                height: 40px;
                border: 1px solid #ddd;
                color: white;
                text-align: center;
                padding: 0px !important;
                position: sticky;
                top: 37px;
                z-index: 4;
            }

            .lvApprovalGrid > tbody > tr:nth-child(odd) > td {
                background-color: white;
            }

            .lvApprovalGrid > tbody > tr:nth-child(even) > td {
                background-color: #ddd;
            }

        .txtDate {
            width: 150px;
        }

        .class-red-data {
            background-color: red;
        }

        .class-red {
            background-color: red;
        }

        .class-green {
            background-color: #2e6886 !important;
        }

        .green {
            text-align: center;
            font-size: 25px;
            color: green;
            font-weight: bold;
        }

        .black {
            text-align: center;
            font-size: 17px;
            color: black;
        }

        .red {
            text-align: center;
            font-size: 25px;
            color: red;
            font-weight: bold;
        }

        .approveRowtr td {
            position: sticky;
            bottom: -1px;
        }

        .Operatortr td {
            position: sticky;
            bottom: 27px;
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
                                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>Year</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtYear" ClientIDMode="Static" CssClass="form-control txtDate" Style="width: 120px;"></asp:TextBox>
                            </td>
                            <td>Month</td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnMonthVal" ClientIDMode="Static" />
                                <asp:TextBox runat="server" ID="txtMonth" ClientIDMode="Static" CssClass="form-control txtDate" Style="width: 120px;"></asp:TextBox>
                            </td>
                            <td>Component ID</td>
                            <td>
                                <%--<asp:DropDownList runat="server" ID="ddlComponentID" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="ddlComponentID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                <asp:Label runat="server" ClientIDMode="Static" ID="lblComponent" CssClass="form-control"></asp:Label>
                            </td>
                            <td>Operation No</td>
                            <td>
                                <asp:Label runat="server" ClientIDMode="Static" ID="lblOperation" CssClass="form-control"></asp:Label>
                                <%--<asp:DropDownList runat="server" ID="ddlOperationNo" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>--%>
                            </td>

                            <td>
                                <asp:Button runat="server" ID="btnView" Text="VIEW" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" OnClientClick="return ViewClick();" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnExport" Text="EXPORT" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row" style="max-height: 80vh; overflow: auto; margin: 15px 0px 0px 3px;">
                    <asp:ListView runat="server" ClientIDMode="Static" ID="lvApprovalGrid" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="lvApprovalGrid" id="lvApprovalGrid">
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" visible='<%# Convert.ToBoolean(Eval("ApproveVisibility")) == true ? false : true %>'>
                                <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 120px !important; border-bottom: 1px solid grey; vertical-align: middle; text-align: center" visible='<%# Eval("Charvisibility") %>' rowspan='<%# Eval("Rowspan")%>'>
                                    <asp:Label runat="server" Text="Sl No" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                    <asp:Label runat="server" Text='<%# Eval("SlNo") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                </td>
                                <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 200px !important; border-bottom: 1px solid grey; vertical-align: middle; text-align: center" visible="false" rowspan='<%# Eval("Rowspan")%>'>
                                    <asp:Label runat="server" Text='<%# Eval("CharacteristicID") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                    <asp:Label runat="server" Text='<%# Eval("CharacteristicID") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                </td>
                                <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 150px !important; border-bottom: 1px solid grey; vertical-align: middle; text-align: center" visible='<%# Eval("Charvisibility") %>' rowspan='<%# Eval("Rowspan")%>'>
                                    <asp:Label runat="server" Text='<%# Eval("Description") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                    <asp:Label runat="server" Text='<%# Eval("Description") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                </td>
                                <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 150px !important; border-bottom: 1px solid grey;" rowspan='<%# Eval("Rowspan")%>'>
                                    <asp:Label runat="server" Text='<%# Eval("PointsToBeChecked") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                    <asp:Label runat="server" Text='<%# Eval("PointsToBeChecked") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                </td>
                                <td style="padding: 0px !important;">
                                    <asp:ListView runat="server" DataSource='<%# Eval("TransactionData") %>'>
                                        <LayoutTemplate>
                                            <table style="height: 100%; width: 100%" class="inner-tbl">
                                                <tr>
                                                    <td runat="server" id="itemplaceholder"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td>
                                                <table style="height: 100%; width: 100%" class="inner-tbl">
                                                    <tr>
                                                        <td colspan='<%# Eval("DateColspan") %>' runat="server" visible='<%# Eval("HeaderVisibility") %>' class='<%# Eval("BackgroundClass").ToString() %>' style="min-width: 180px; padding: 0px !important; text-align: center; border-right: 1px solid #ddd;" rowspan='<%# Eval("Rowspan")%>'>
                                                            <asp:Label runat="server" Text='<%# Eval("DayHeader") %>' CssClass='<%# Eval("innerDayValueClass") %>' ForeColor="White"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td runat="server" visible='<%# Eval("Shiftvisibility") %>' class='<%# Eval("BackgroundClass").ToString() %>' style="min-width: 180px; padding: 0px !important; text-align: center; border-right: 1px solid #ddd;" rowspan='<%# Eval("Rowspan")%>'>
                                                            <asp:Label runat="server" Text='<%# Eval("ShiftHeader") %>' CssClass='<%# Eval("innerDayValueClass") %>' ForeColor="White"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td runat="server" visible='<%# Eval("ContentVisibility") %>' class='<%# Eval("BackgroundClass").ToString() %>' style="min-width: 180px; padding: 0px !important; text-align: center; border-right: 1px solid grey; max-width: 180px; overflow-wrap: break-word; border-bottom: 1px solid grey;">
                                                            <asp:Label runat="server" Text='<%# Eval("ShiftValue") %>' CssClass='<%# Eval("innerDayValueClass") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr runat="server" class='<%# Eval("rowclass") %>' visible='<%# Eval("ApproveVisibility") %>'>
                                <td style="border: 0px !important;"></td>
                                <td style="border: 0px !important;"></td>
                                <td style="text-align: right; font-weight: bold; padding: 5px;">
                                    <asp:Label runat="server" Text='<%# Eval("PointsToBeChecked") %>' Font-Bold="true"></asp:Label>
                                </td>
                                <td style="padding: 0px !important;">
                                    <asp:ListView runat="server" ID="lvFootergrid" ClientIDMode="Static" DataSource='<%# Eval("TransactionData") %>'>
                                        <LayoutTemplate>
                                            <table style="height: 100%; width: 100%" class="inner-tbl">
                                                <tr>
                                                    <td runat="server" id="itemplaceholder"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td runat="server" class='<%# Eval("BackgroundClass").ToString() %>' style="min-width: 180px; padding: 5px 0px !important; text-align: center; border-right: 1px solid grey; border-bottom: 1px solid grey;">
                                                <asp:Label runat="server" Text='<%# Eval("ShiftValue") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdnValue" Value='<%# Eval("ShiftHeader") %>' />
                                                <asp:HiddenField runat="server" ID="hdndate" Value='<%# Eval("DayHeader") %>' />
                                                <asp:Button runat="server" ID="btnApprove" Text="APPROVE" ClientIDMode="Static" Visible='<%# Eval("HeaderVisibility") %>' CssClass="bajaj-btn-style" OnClick="btnApprove_Click" />
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
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
            freezeColumnFromLeft("lvApprovalGrid", "3");
            setDateControls();
        });
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function setDateControls() {
            $("#txtYear").datepicker({
                format: 'yyyy',
                viewMode: "years",
                minViewMode: "years",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#txtMonth").datepicker({
                format: 'M',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            freezeColumnFromLeft("lvApprovalGrid", "3");
            setDateControls();
            $.unblockUI({});
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
