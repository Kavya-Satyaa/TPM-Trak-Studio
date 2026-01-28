<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionTransactionVulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.VulkanMachineShop.InspectionTransactionVulkan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblSettings {
            width: 70%;
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
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .lvApproveDetails {
            width: 95%;
        }

            .lvApproveDetails > tbody > tr > td {
                border: 1px solid #b6b6b6;
                padding: 5px;
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
                max-width: 100px;
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
        .trOperatorRow td{
            position: sticky;
            bottom:50px;
            background-color: grey;
            padding: 0px !important;
        }
    </style>

    <table class="tblSettings">
        <tr>
            <td>Machine ID</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control" Style="width: 300px;"></asp:DropDownList>
            </td>
            <td>Date</td>
            <td>
                <asp:TextBox runat="server" ID="txtDate" ClientIDMode="Static" CssClass="form-control" Style="width: 180px;"></asp:TextBox>
            </td>
            <td>Shift</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>
                <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style btnclass" OnClick="btnView_Click" />
            </td>
        </tr>
    </table>

    <div class="row">
        <div style="height: 100vh; overflow: auto; margin: 10px 0px 0px 10px;">
            <asp:ListView runat="server" ID="lvInspectionApproval" ClientIDMode="Static">
                <LayoutTemplate>
                    <table class="lvApproveDetails headerFixer" id="lvApproveDetails">
                        <tr runat="server" id="itemplaceholder"></tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr runat="server" visible='<%# Eval("HeaderVisibility") %>'>
                        <th>
                            <asp:Label runat="server" Text="Component ID"></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" Text="Operation No."></asp:Label>
                        </th>
                        <th>
                            <asp:Label runat="server" Text="Characteristic ID"></asp:Label>
                        </th>
                        <th style="padding: 0px !important;">
                            <asp:ListView runat="server" DataSource='<%# Eval("listofHeatCode") %>'>
                                <LayoutTemplate>
                                    <table class="inner-tbl">
                                        <tr>
                                            <th runat="server" id="itemplaceholder"></th>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <th>
                                        <asp:Label runat="server" Text='<%# Eval("HeatCodeValue") %>'></asp:Label>
                                    </th>
                                </ItemTemplate>
                            </asp:ListView>
                        </th>
                    </tr>
                    <tr runat="server" visible='<%# Eval("ContentVisibility") %>'>
                        <td>
                            <asp:Label runat="server" Text='<%# Eval("ComponentID") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%# Eval("OperationNo") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%# Eval("CharacteristicID") %>'></asp:Label>
                        </td>
                        <td style="padding: 0px !important;">
                            <asp:ListView runat="server" DataSource='<%# Eval("listofHeatCode") %>'>
                                <LayoutTemplate>
                                    <table class="inner-tbl">
                                        <tr>
                                            <td runat="server" id="itemplaceholder"></td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <td>
                                        <asp:Label runat="server" Text='<%# Eval("HeatCodeValue") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr class="trOperatorRow" runat="server" visible='<%# Eval("OperatorRowVisibility") %>'>
                        <td style="text-align: center; font-weight: bold; background-color: #ababab !important; color: black;
            z-index: 899 !important; padding: 5px 10px;">
                            <asp:Label runat="server" Text='<%# Eval("ComponentID") %>'></asp:Label>
                        </td>
                        <td style="text-align: center; font-weight: bold; background-color: #ababab !important; color: black;
            z-index: 899 !important; padding: 5px 10px;">
                            <asp:Label runat="server" Text='<%# Eval("OperationNo") %>'></asp:Label>
                        </td>
                        <td style="text-align: center; font-weight: bold; background-color: #ababab !important; color: black;
            z-index: 899 !important; padding: 5px 10px;">
                            <asp:Label runat="server" Text='<%# Eval("CharacteristicID") %>'></asp:Label>
                        </td>
                        <td style="padding: 0px !important;">
                            <asp:ListView runat="server" DataSource='<%# Eval("listofHeatCode") %>'>
                                <LayoutTemplate>
                                    <table class="inner-tbl">
                                        <tr>
                                            <td runat="server" id="itemplaceholder"></td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <td  style="text-align: center; font-weight: bold; background-color: #ababab !important; color: black">
                                        <asp:Label runat="server" Text='<%# Eval("HeatCodeValue") %>'></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr runat="server" visible='<%# Eval("FooterVisisbility") %>' class="approveRowtr" style="padding: 5px; height: 50px;">
                        <td style=" z-index: 799 !important;"></td>
                        <td style=" z-index: 799 !important;"></td>
                        <td style=" z-index: 799 !important;"></td>
                        <td runat="server" colspan='<%# Eval("ColSpanApproval") %>' style="text-align: center; padding: 10px;">
                            <asp:Button runat="server" ID="btnApprove" Text="Approve" ClientIDMode="Static" Visible='<%# Eval("ButtonVisibility") %>' CssClass="bajaj-btn-style btnClass" />
                            <asp:Label runat="server" Text='<%# Eval("ComponentID") %>' Visible='<%# Convert.ToBoolean(Eval("ButtonVisibility")) == true ? false : true %>' Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
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
                    <asp:Button runat="server" Text="Yes" ID="btnApproveConfirm" CssClass="confirm-modal-btn" OnClick="btnApprove_Click" ClientIDMode="Static" />
                    <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            setDateTimePicker();
            freezeColumnFromLeft("lvApproveDetails", "3");
        });
        function setDateTimePicker() {
            $("#txtDate").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        $("#btnApprove").on("click", function () {
            $("#ConfirmModal").modal("show");
            return false;
        })

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setDateTimePicker();
            freezeColumnFromLeft("lvApproveDetails", "3");
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
