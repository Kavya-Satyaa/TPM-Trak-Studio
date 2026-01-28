<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VulkanPMMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.VulkanMachineShop.VulkanPMMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
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

        .lvChecksheetData {
            width: 100%;
        }

            .lvChecksheetData > tbody > tr > td {
                line-height: 35px;
                border: 1px solid #ddd;
                padding: 5px 5px;
            }

            .lvChecksheetData > tbody > tr > th {
                line-height: 40px;
                border: 1px solid #ddd;
                text-align: center;
            }

            .lvChecksheetData > tbody > tr:nth-child(odd) > td {
                background-color: white;
            }

            .lvChecksheetData > tbody > tr:nth-child(even) > td {
                background-color: #ddd;
            }

        #fuImport {
            display: inline-block;
            width: 65% !important;
        }

        .commontd {
            width: 120px;
        }

        #ddlFrequency {
            width: 200px;
        }
    </style>
    <table class="tblSettings">
        <tr>
            <td class="commontd">Machine ID</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMachineID" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
            </td>
            <td class="commontd">Frequency</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" ClientIDMode="Static">
                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="commontd">Rev ID</td>
            <td>
                <asp:DropDownList runat="server" Visible="false" ID="ddlRevNo" CssClass="form-control"></asp:DropDownList>
                <asp:TextBox runat="server" ID="txtRevNo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
            </td>
            <td class="commontd">Doc. No.
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtDocNo" CssClass="form-control" Style="width: 100px;"></asp:TextBox>
            </td>
            <td class="commontd">Rev. Date</td>
            <td>
                <asp:TextBox runat="server" ID="txtRevDate" CssClass="form-control Date" Style="width: 150px;"></asp:TextBox>
            </td>
            <td colspan="2">
                <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style btnclass" OnClick="btnView_Click" />
                <asp:Button runat="server" ID="btnCopy" ClientIDMode="Static" Text="Copy" CssClass="bajaj-btn-style btnclass" OnClientClick="return openCopyModal();" />
                <asp:Button runat="server" ID="btnCreateRevisionNo" Visible="true" Text="New Rev No." CssClass="btn btn-primary btnclass" ClientIDMode="Static" OnClientClick="return openRevNoModal();" />
            </td>
        </tr>
        <tr>
            <td class="commontd">Prepared BY</td>
            <td>
                <asp:HiddenField runat="server" ID="hdnHeaderUpdate" ClientIDMode="Static" />
                <asp:TextBox runat="server" ID="txtPreparedby" CssClass="form-control txtheader"></asp:TextBox>
            </td>
            <td class="commontd">Approved BY</td>
            <td>
                <asp:TextBox runat="server" ID="txtApprovedBy" CssClass="form-control txtheader"></asp:TextBox>
            </td>
            <td class="commontd">Verified BY</td>
            <td>
                <asp:TextBox runat="server" ID="txtVerifiedBy" CssClass="form-control txtheader"></asp:TextBox>
            </td>
            <td colspan="4" style="width: 25%;">
                <asp:HiddenField runat="server" ID="hdnNew" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnNew" Text="New" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" />
                <asp:Button runat="server" ID="btnEdit" Text="Edit" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" />
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" ClientIDMode="Static" CssClass="btn btn-primary" />
                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnSave_Click" OnClientClick="return saveValidation();" />
                <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnDelete_Click" OnClientClick="return deleteValidation();" />
            </td>
            <td colspan="2" style="width: 20%; padding-left: 10px;">
                <asp:FileUpload runat="server" ID="fuImport" ClientIDMode="Static" CssClass="form-control" ToolTip="Select the file to Import" />
                <asp:Button runat="server" ID="btnImport" Text="Import" CssClass="btn btn-primary" OnClick="btnImport_Click" />
                <asp:LinkButton runat="server" ID="lnkDownloadTemplate" ClientIDMode="Static" Text="Download Template" Font-Underline="true" ForeColor="#d3ff78" OnClick="lnkDownloadTemplate_Click"></asp:LinkButton>
            </td>
        </tr>
    </table>

    <div class="row">
        <div style="overflow: auto; height: 80vh; margin: 10px;">
            <asp:HiddenField runat="server" ID="hdnNewItem" ClientIDMode="Static" />
            <%-- <asp:UpdatePanel runat="server">
                <ContentTemplate>--%>
            <asp:ListView runat="server" ID="lvChecksheetData" ClientIDMode="Static" OnItemDataBound="lvChecksheetData_ItemDataBound" InsertItemPosition="FirstItem">
                <LayoutTemplate>
                    <table class="lvChecksheetData headerFixer">
                        <tr>
                            <th style="width: 80px;">Sl. No.</th>
                            <th>Checkpoint Desc.</th>
                            <th>Particular</th>
                            <th style="width: 180px;">Frequency</th>
                            <th>Control Method</th>
                            <th>Responsibility</th>
                            <th>Action</th>
                        </tr>
                        <tr runat="server" id="itemplaceholder"></tr>
                    </table>
                </LayoutTemplate>
                <InsertItemTemplate>
                    <tr id="trInsertItemTemplate" runat="server" clientidmode="static" class="active">
                        <td>
                            <asp:TextBox runat="server" ID="txtCheckpointID" CssClass="form-control"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCheckpointDesc" CssClass="form-control"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtParticular" CssClass="form-control"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Enabled="false">
                                <%--<asp:ListItem Text="All" Value="All"></asp:ListItem>--%>
                                <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtControlMethod" CssClass="form-control"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlResponsibility" CssClass="form-control" Enabled="false">
                                <asp:ListItem Text="Operator" Value="operator"></asp:ListItem>
                                <asp:ListItem Text="Maintenance" Value="maintenance"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                </InsertItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="text-align: center;">
                            <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                            <asp:Label runat="server" ID="lblCheckpointID" Text='<%# Eval("CheckpointID") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCheckpointDesc" CssClass="form-control txtUpdate" Text='<%# Eval("CheckpointDesc") %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtParticular" CssClass="form-control txtUpdate" Text='<%# Eval("Particular") %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:HiddenField runat="server" ID="hdnFrequency" Value='<%# Eval("Frequency") %>'></asp:HiddenField>
                            <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="form-control" Enabled="false">
                                <%--<asp:ListItem Text="All" Value="All"></asp:ListItem>--%>
                                <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtControlMethod" CssClass="form-control txtUpdate" Text='<%# Eval("ControlMethod") %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:HiddenField runat="server" ID="hdnResponsibility" Value='<%# Eval("Responsibility") %>'></asp:HiddenField>
                            <asp:DropDownList runat="server" ID="ddlResponsibility" CssClass="form-control" Enabled="false">
                                <asp:ListItem Text="Operator" Value="operator"></asp:ListItem>
                                <asp:ListItem Text="Maintenance" Value="maintenance"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox runat="server" ID="chkDelete" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <%--    </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlFrequency" EventName="SelectedIndexChanged1" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>


    <div class="modal infoModal" id="NewRevNoModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 650px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="newEditModalTitle" runat="server">Create Revision No.</h4>
                </div>
                <div class="modal-body" style="padding: 1px 15px !important">
                    <table class="innertbl" style="display: flex; justify-content: center; margin: 20px 0px;">
                        <tr>
                            <td>Revision No.</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRevNoNew" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div class="modal-footer" style="border: 0px !important; border-top: 1px solid black !important;">
                        <asp:Button runat="server" ID="btnSaveRevisionNo" OnClick="btnSaveRevisionNo_Click" Text="Save Rev No." ClientIDMode="Static" CssClass="btn btn-primary" />
                        <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseNewEditModal();">CANCEL</button>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="modal infoModal" id="CLCopyModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 650px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="headerCopyModal" runat="server">Checklist Copy</h4>
                </div>
                <div class="modal-body" style="padding: 1px 15px !important">
                    <table class="innertbl" style="display: flex; justify-content: center; margin: 20px 0px;">
                        <tr>
                            <td>Src MachineID</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSourceMachineID" CssClass="form-control"></asp:TextBox>
                                <%--<asp:DropDownList runat="server" ID="ddlSourceMachineID" CssClass="form-control"></asp:DropDownList>--%>
                            </td>
                            <td>Machines</td>
                            <td>
                                <asp:ListBox runat="server" ID="lbMultiMachineID" ClientIDMode="Static" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                    <div class="modal-footer" style="border: 0px !important; border-top: 1px solid black !important;">
                        <asp:Button runat="server" ID="btnCopyModal" OnClick="btnCopyModal_Click" Text="Save" ClientIDMode="Static" CssClass="btn btn-primary" />
                        <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseCopyModal();">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <script>
        $(document).ready(function () {
            $(".txtUpdate").attr("readonly", true);
            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#btnCancel").hide();
            showHideFirstRow();
            var option = $("#ddlFrequency :selected").val();
            if (option == "") {
                $("#btnNew").attr("disabled", true);
                $("#btnEdit").attr("disabled", true);
                $("#btnSave").attr("disabled", true);
                $("#btnDelete").attr("disabled", true);
            }
            else {
                $("#btnNew").attr("disabled", false);
                $("#btnEdit").attr("disabled", false);
                $("#btnSave").attr("disabled", false);
                $("#btnDelete").attr("disabled", false);
            }
        });


        $(".txtheader").focus(function () {
            $("#hdnHeaderUpdate").val("update");
        })

        $("#btnNew").click(function () {
            $("#btnEdit").hide();
            $("#btnCancel").show();
            $("#btnNew").hide();
            $("#hdnNew").val("New");
            $(".txtUpdate").attr("readonly", true);
            showHideFirstRow();
            return false;
        });
        $("#btnEdit").click(function () {
            $("#btnCancel").show();
            $("#btnNew").hide();
            $("#btnEdit").hide();
            $(".txtUpdate").attr("readonly", false);
            return false;
        })
        $("#btnCancel").click(function () {
            debugger;
            if ($("#hdnNew").val() == "New") {
                showHideFirstRow();
            }
            $("#btnCancel").hide();
            $("#btnNew").show();
            $("#btnEdit").show();
            $("#hdnNew").val("");
            $(".txtUpdate").attr("readonly", true);
            return false;
        });

        $("#btnSaveRevisionNo").click(function () {
            if ($("#txtRevNoNew").val() == "") {
                openWarningModal_1("Revision Number cannot be empty");
                return false;
            }
            return true;
        })

        $('#ddlFrequency').change(function () {
            var option = $("#ddlFrequency :selected").val();
            if (option == "") {
                $("#btnNew").attr("disabled", true);
                $("#btnEdit").attr("disabled", true);
                $("#btnSave").attr("disabled", true);
                $("#btnDelete").attr("disabled", true);
            }
            else {
                $("#btnNew").attr("disabled", false);
                $("#btnEdit").attr("disabled", false);
                $("#btnSave").attr("disabled", false);
                $("#btnDelete").attr("disabled", false);
            }
        });

        function showHideFirstRow() {
            debugger;
            var rows = $(".lvChecksheetData tr");
            var firstRow = $(".lvChecksheetData").find("#trInsertItemTemplate");
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

        $('.lvChecksheetData tr td .txtUpdate').focus(function () {
            var row = $('.lvChecksheetData tr td').closest("tr");
            row.find('#hdnUpdate').val("update");
        });


        function openRevNoModal() {
            $("#txtRevNoNew").val("");
            $("#NewRevNoModal").modal('show');
            return false;
        }
        function CloseNewEditModal() {
            $("#NewRevNoModal").modal('hide');
        }
        function openCopyModal() {
            $("#CLCopyModal").modal('show');
            return false;
        }

        function CloseCopyModal() {
            $("#CLCopyModal").modal('hide');
        }

        function OpenRevNoPopUp() {
            $("#NewRevNoModal").modal('show');
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(".txtUpdate").attr("readonly", true);
            $(document).ready(function () {
                $("#btnCancel").hide();
            });
            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            showHideFirstRow();
            var option = $("#ddlFrequency :selected").val();
            if (option == "") {
                $("#btnNew").attr("disabled", true);
                $("#btnEdit").attr("disabled", true);
                $("#btnSave").attr("disabled", true);
                $("#btnDelete").attr("disabled", true);
            }
            else {
                $("#btnNew").attr("disabled", false);
                $("#btnEdit").attr("disabled", false);
                $("#btnSave").attr("disabled", false);
                $("#btnDelete").attr("disabled", false);
            }
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
