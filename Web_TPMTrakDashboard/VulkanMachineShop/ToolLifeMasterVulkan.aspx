<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ToolLifeMasterVulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.VulkanMachineShop.ToolLifeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .tblSettings {
            width: 85%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 5px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: medium;
                /*box-shadow: 2px 2px 2px black;*/
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .commontd {
            width: 120px;
        }

        .lvToolLifeMaster {
            width: 100%;
        }

            .lvToolLifeMaster > tbody > tr > td {
                line-height: 35px;
                border: 1px solid #ddd;
                padding: 5px 5px;
            }

            .lvToolLifeMaster > tbody > tr > th {
                line-height: 40px;
                border: 1px solid #ddd;
                text-align: center;
            }

            .lvToolLifeMaster > tbody > tr:nth-child(odd) > td {
                background-color: white;
            }

            .lvToolLifeMaster > tbody > tr:nth-child(even) > td {
                background-color: #ddd;
            }
    </style>
    <table class="tblSettings">
        <tr>
            <td class="commontd">Machine ID</td>
            <td style="width: 250px;">
                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control"></asp:DropDownList>
            </td>
            <td class="commontd">Tool No.</td>
            <td style="width: 250px;">
                <asp:TextBox runat="server" ID="txtToolNo" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <asp:HiddenField runat="server" ID="hdnNew" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style btnclass" OnClick="btnView_Click" />
                <asp:Button runat="server" ID="btnNew" Text="New" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnEdit" Text="Edit" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnSave_Click" />
                <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnDelete_Click" />
            </td>
        </tr>
    </table>
    <div class="row" style="overflow: auto; max-height: 90vh; margin: 10px 0px 5px 5px;">
        <asp:ListView runat="server" ID="lvToolLifeMaster" InsertItemPosition="FirstItem">
            <LayoutTemplate>
                <table class="lvToolLifeMaster headerFixer">
                    <tr>
                        <th>Machine ID</th>
                        <th>Tool No</th>
                        <th>Interface ID</th>
                        <th>Tool Type</th>
                        <th>Tool Spec.</th>
                        <th>Tool Feed</th>
                        <th>No. Of Edges</th>
                        <th>Tool Life in Meter</th>
                        <th>Delete?</th>
                    </tr>
                    <tr runat="server" id="itemplaceholder"></tr>
                </table>
            </LayoutTemplate>
            <InsertItemTemplate>
                <tr runat="server" class="active" id="trInsertItemTemplate" clientidmode="static">
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolNo" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtInterfaceID" CssClass="form-control allowNumber txtInterNew"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolType" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolSpec" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolFeed" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNoOfEdges" CssClass="form-control allowNumber"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolLifeInMeter" CssClass="form-control allowNumber"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </InsertItemTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnUpdate" />
                        <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblToolNo" Text='<%# Eval("ToolNo") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtInterfaceID" CssClass="form-control txtInter allowNumber txtUpdate" Text='<%# Eval("InterfaceID") %>'></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolType" Text='<%# Eval("ToolType") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolSpec" Text='<%# Eval("ToolSpecification") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolfeed" Text='<%# Eval("ToolFeed") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNoOfEdges" Text='<%# Eval("NoOfEdges") %>' CssClass="form-control txtUpdate allowNumber"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtToolLifeInMeter" Text='<%# Eval("ToolLifeInMeter") %>' CssClass="form-control txtUpdate allowNumber"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkDelete" ClientIDMode="Static" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>

    <script>
        $(document).ready(function () {
            $("#btnCancel").hide();
            $(".txtUpdate").attr("disabled", true);
            showHideFirstRow();
        });

        $("#btnNew").on("click", function () {
            $("#btnCancel").show();
            $(".txtUpdate").attr("disabled", true);
            $("#btnNew").hide();
            $("#btnEdit").hide();
            showHideFirstRow();
            $("#hdnNew").val("New");
            return false;
        })
        $("#btnEdit").on("click", function () {
            $("#btnCancel").show();
            $(".txtUpdate").attr("disabled", false);
            $("#btnNew").hide();
            $("#btnEdit").hide();
            return false;
        })
        $("#btnCancel").on("click", function () {
            if ($("#hdnNew").val() == "New") {
                showHideFirstRow();
            }
            $("#btnNew").show();
            $(".txtUpdate").attr("disabled", true);
            $("#btnEdit").show();
            $("#btnCancel").hide();
            $("#hdnNew").val("");
            return false;
        });

        $(".lvToolLifeMaster tr td .txtUpdate").focus(function () {
            var row = $(this).closest("tr");
            $(row).find("#hdnUpdate").val("Update");
        });

        function showHideFirstRow() {
            debugger;
            var rows = $(".lvToolLifeMaster tr");
            var firstRow = $(".lvToolLifeMaster").find("#trInsertItemTemplate");
            if (rows.length > 2) {
                if ($(firstRow).hasClass("active")) {
                    $(firstRow).removeClass("active");
                    $(firstRow).hide();
                }
                else {
                    $(firstRow).addClass("active");
                    $(firstRow).show();
                }
            }
        }

        $(".lvToolLifeMaster tr td .txtInterNew").focusout(function () {
            debugger;
            var InterfaceIDNew = $(".lvToolLifeMaster tr td .txtInterNew").val();
            var InterfaceIDList = $(".lvToolLifeMaster tr td .txtInter");
            for (var i = 0; i < InterfaceIDList.length; i++) {
                if ($(InterfaceIDList[i]).val() == InterfaceIDNew) {
                    alert('Interface ID already Exists');
                    return false;
                }
            }
            return true;
        });

        $("#btnDelete").click(function () {
            debugger;
            var len = $(".lvToolLifeMaster tr td #chkDelete");
            var RowsToDelete = 0;
            for (var i = 0; i < len.length; i++) {
                if ($(len[i]).is(":checked")) {
                    RowsToDelete++;
                }
            }
            if (RowsToDelete <= 0) {
                toasterWarningMsg('Select atleast one row to delete.');
                return false;
            }
            return true;
        })
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#btnCancel").hide();
            $(".txtUpdate").attr("disabled", true);
            showHideFirstRow();
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
