<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VulkanPMApprovalDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.VulkanMachineShop.VulkanPMApprovalDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblSettings {
            width: 90%;
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
            }

            .lvApprovalGrid > tbody > tr:nth-child(odd) > td {
                background-color: white;
            }

            .lvApprovalGrid > tbody > tr:nth-child(even) > td {
                background-color: #ddd;
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

        .red {
            text-align: center;
            font-size: 25px;
            color: red;
            font-weight: bold;
        }

        .txtDate {
            width: 150px;
        }

        .ddlMachine {
            width: 300px;
        }
    </style>

    <table class="tblSettings">
        <tr>
            <td>Machine ID</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control ddlMachine" Style="width: 300px;"></asp:DropDownList>
            </td>
            <td>Frequency</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control" Style="width: 200px;">
                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                </asp:DropDownList>
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
            <td>
                <asp:Button runat="server" ID="btnView" Text="View" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" OnClick="btnView_Click" />
            </td>
        </tr>
    </table>

    <div class="row" style="max-height: 80vh; overflow: auto; margin: 15px 0px 0px 3px;">
        <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
        <asp:ListView runat="server" ID="lvApprovalGrid">
            <LayoutTemplate>
                <table class="lvApprovalGrid" id="lvApprovalGrid">
                    <tr runat="server" id="itemplaceholder"></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr runat="server" visible='<%# Convert.ToBoolean(Eval("ApproveVisibility")) == true ? false : true %>'>
                    <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 120px !important; border-bottom: 1px solid grey;">
                        <asp:Label runat="server" Text='<%# Eval("CheckpointID") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("CheckpointID") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 200px !important; border-bottom: 1px solid grey;">
                        <asp:Label runat="server" Text='<%# Eval("CheckpointItem") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("CheckpointItem") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 150px !important; border-bottom: 1px solid grey;">
                        <asp:Label runat="server" Text='<%# Eval("Particular") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("Particular") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 150px !important; border-bottom: 1px solid grey;">
                        <asp:Label runat="server" Text='<%# Eval("ControlMethod") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("ControlMethod") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td runat="server" class='<%# Eval("BackgroundClass") %>' style="min-width: 150px !important; border-bottom: 1px solid grey;">
                        <asp:Label runat="server" Text='<%# Eval("Responsibility") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" Text='<%# Eval("Responsibility") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
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
                                <td runat="server" visible='<%# Eval("HeaderVisibility") %>' class='<%# Eval("BackgroundClass").ToString() %>' style="min-width: 180px; padding: 0px !important; text-align: center; border-right: 1px solid #ddd;">
                                    <asp:Label runat="server" Text='<%# Eval("DayHeader") %>' CssClass='<%# Eval("innerDayValueClass") %>' ForeColor="White"></asp:Label>
                                </td>
                                <td runat="server" visible='<%# Eval("ContentVisibility") %>' class='<%# Eval("BackgroundClass").ToString() %>' style="min-width: 180px; padding: 0px !important; text-align: center; border-right: 1px solid grey; border-bottom: 1px solid grey;">
                                    <asp:Label runat="server" Text='<%# Eval("DayValue") %>' CssClass='<%# Eval("innerDayValueClass") %>'></asp:Label>
                                </td>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr runat="server" visible='<%# Eval("ApproveVisibility") %>'>
                    <td style="border: 0px !important;"></td>
                    <td style="border: 0px !important;"></td>
                    <td style="border: 0px !important;"></td>
                    <td style="border: 0px !important;"></td>
                    <td style="text-align: right; font-weight: bold; padding: 5px;">
                        <asp:Label runat="server" Text='<%# Eval("CheckpointID") %>' Font-Bold="true"></asp:Label>
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
                                <td runat="server" class='<%# Eval("BackgroundClass").ToString() %>' style="min-width: 180px; padding: 5px 0px !important; text-align: center; border-right: 1px solid grey; border-bottom: 1px solid grey;">
                                    <asp:Label runat="server" Text='<%# Eval("DayValue") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnValue" Value='<%# Eval("DayHeader") %>' />
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
                    <asp:Button runat="server" Text="Yes" ID="btnApproveConfirm" CssClass="confirm-modal-btn" OnClick="btnApprove_Click1" ClientIDMode="Static" />
                    <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            freezeColumnFromLeft("lvApprovalGrid", "5");
            setDateControls();
            var option = $("#ddlFrequency :selected").val();
            if (option == "Monthly") {
                $("#txtMonth").attr("disabled", true);
            }
            else {
                $("#txtMonth").attr("disabled", false);
            }
        });
        $("#btnView").on("click", function () {
            debugger;
            if ($("#ddlFrequency :selected").val() == "Weekly" || $("#ddlFrequency :selected").val() == "Daily") {
                if ($("#txtMonth").val() == "") {
                    openWarningModal_1("Please select a Month!");
                    return false;
                }
            }
            return true;
        })
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


        function OpenConfirmModal() {
            $("#ConfirmModal").modal("show");
        }

        $('#ddlFrequency').change(function () {
            debugger;
            var option = $("#ddlFrequency :selected").val();
            if (option == "Monthly") {
                $("#txtMonth").attr("disabled", true);
            }
            else {
                $("#txtMonth").attr("disabled", false);
            }
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            freezeColumnFromLeft("lvApprovalGrid", "5");
            setDateControls();
            var option = $("#ddlFrequency :selected").val();
            if (option == "Monthly") {
                $("#txtMonth").attr("disabled", true);
            }
            else {
                $("#txtMonth").attr("disabled", false);
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
