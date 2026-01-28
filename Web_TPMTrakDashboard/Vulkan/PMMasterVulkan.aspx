<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMMasterVulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.Vulkan.PM_Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .tblSettings {
            width: 100%;
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

        .lvDailyCLGrid {
            width: 100%;
        }

            .lvDailyCLGrid > tbody > tr > th {
                text-align: center;
                border: 1px solid white;
                color: white;
                height: 38px;
            }

            .lvDailyCLGrid > tbody > tr > td {
                border: 1px solid #ddd;
                padding: 10px;
                color: black;
            }

            .lvDailyCLGrid > tbody > tr:nth-child(even) > td {
                background-color: white;
            }

            .lvDailyCLGrid > tbody > tr:nth-child(odd) > td {
                background-color: #ddd;
            }


        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 25%;
        }

        .cssclass {
            min-width: 350px;
        }

        .innertbl > tbody > tr > td {
            padding: 5px 10px;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table class="tblSettings">
                <tr>
                    <td>Plant ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlantID" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td>Machine ID</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachineID" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>Frequency</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFrequeny" CssClass="form-control" OnSelectedIndexChanged="ddlFrequeny_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                            <asp:ListItem Text="15 Days" Value="15days"></asp:ListItem>
                            <asp:ListItem Text="Quarterly" Value="Quarterly"></asp:ListItem>
                            <%--<asp:ListItem Text="Half Yearly" Value="HalfYearly"></asp:ListItem>--%>
                            <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Rev. No.</td>
                    <td>
                        <asp:DropDownList runat="server" Visible="false" ID="ddlRevNo" CssClass="form-control"></asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtRevNo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style btnclass" OnClick="btnView_Click" />
                    </td>

                    <td>
                        <asp:Button runat="server" ID="btnCreateRevisionNo" Visible="true" Text="Create Rev. No." CssClass="btn btn-primary btnclass" ClientIDMode="Static" OnClientClick="return openRevNoModal();" />
                    </td>
                </tr>
                <tr>
                    <td>Doc. No.</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDocNo" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>Issue Date</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtIssueDate" CssClass="form-control Date"></asp:TextBox>
                    </td>
                    <td>Rev. Date</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRevisionDate" CssClass="form-control Date"></asp:TextBox>
                    </td>
                    <td colspan="4" style="text-align: left;">
                        <asp:HiddenField runat="server" ID="hdnNew" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="btnNew" Text="New" ClientIDMode="Static" CssClass="bajaj-btn-style btnclass" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" ClientIDMode="Static" CssClass="btn btn-primary" />
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnSave_Click" OnClientClick="return saveValidation();" />
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="bajaj-btn-style btnclass" ClientIDMode="Static" OnClick="btnDelete_Click" OnClientClick="return deleteValidation();" />
                        <asp:Button runat="server" ID="btnCopy" Text="Copy" CssClass="bajaj-btn-style btnclass" OnClientClick="return openCopyModal();" />
                    </td>
                </tr>
            </table>

            <div style="height: 75vh; overflow: auto; margin-top: 10px;" id="gridContainer">
                <asp:ListView runat="server" ID="lvCLGrid" ClientIDMode="Static" InsertItemPosition="FirstItem" OnItemDataBound="lvDailyCLGrid_ItemDataBound">
                    <LayoutTemplate>
                        <table class="lvDailyCLGrid headerFixer" id="lvDailyCLGrid" style="min-width: 100vw;">
                            <tr>
                                <th style="width: 100px;">Sl No.</th>
                                <th>Check Point</th>
                                <th>Requirement</th>
                                <th>Method</th>
                                <th>Instrument</th>
                                <th>Reference Image</th>
                                <th>Observation</th>
                                <th>Image Required?</th>
                                <th>Delete?</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <InsertItemTemplate>
                        <tr runat="server" id="trInsertItemTemplate" class="active" clientidmode="static">
                            <td>
                                <asp:TextBox runat="server" ID="txtSlNo" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCheckPoint" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRequirement" Text="" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ListBox runat="server" ID="lbMethod" SelectionMode="Multiple" CssClass="form-control lbMethod"></asp:ListBox>
                            </td>
                            <td>
                                <asp:ListBox runat="server" ID="lbInstrument" SelectionMode="Multiple" CssClass="form-control lbMethod"></asp:ListBox>
                            </td>
                            <td>
                                <asp:FileUpload runat="server" ID="fuReferenceImage" AllowMultiple="false" CssClass="form-control" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtObservation" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td class="text-center">
                                <asp:CheckBox runat="server" ID="chkImageRequired" />
                            </td>
                            <td></td>
                        </tr>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                                <asp:Label runat="server" ID="lblSlNo" Text='<%# Eval("SlNo") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="lblCheckPoint" Text='<%# Eval("CheckPoint") %>' CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRequirement" Text='<%# Eval("Requirement") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnMethod" Value='<%# Eval("Method") %>' />
                                <asp:ListBox runat="server" ID="lbMethod" SelectionMode="Multiple" CssClass="form-control txtUpdate lbMethod"></asp:ListBox>
                            </td>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnInstrment" Value='<%# Eval("Instrument") %>' />
                                <asp:ListBox runat="server" ID="lbInstrument" ClientIDMode="Static" SelectionMode="Multiple" CssClass="form-control txtUpdate lbMethod"></asp:ListBox>
                            </td>
                            <td>
                                <asp:FileUpload runat="server" ID="fuReferenceImage" />
                                <asp:LinkButton runat="server" ID="lblReferenceImage" Text='<%# Eval("ReferenceImageName") %>' OnClick="lblReferenceImage_Click" Enabled="false" CssClass="txtUpdate"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtObservation" Text='<%# Eval("Observation") %>' CssClass="form-control txtUpdate"></asp:TextBox>
                            </td>
                            <td class="text-center">
                                <asp:CheckBox runat="server" ID="chkImageRequired" Checked='<%# Eval("IsImageRequired") %>' CssClass="txtUpdate" />
                            </td>
                            <td class="text-center">
                                <asp:CheckBox runat="server" ID="chkDelete" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlPlantID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlMachineID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlFrequeny" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
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
                                <asp:TextBox runat="server" ID="txtSrcMachineID" ReadOnly="true" CssClass="form-control"></asp:TextBox>
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

    <div class="modal infoModal" id="RefImageModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 650px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="h1" runat="server">Reference Image</h4>
                </div>
                <div class="modal-body" style="padding: 1px 15px !important">
                    <asp:Image runat="server" ID="imgReference" />
                </div>
                <div class="modal-footer" style="border: 0px !important; border-top: 1px solid black !important;">
                    <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseRefImageModal();">CLOSE</button>
                </div>
            </div>
        </div>
    </div>


    <script>
        $(document).ready(function () {
            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            $(".lbMethod").multiselect({
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
            $('#btnCancel').hide();
            showHideFirstRow();

        });

        $("#btnSaveRevisionNo").click(function () {
            if ($("#txtRevNoNew").val() == "") {
                openWarningModal_1('Revision Number cannot be Empty.');
                return false;
            }
            return true;
        })

        function showHideFirstRow() {
            debugger;
            var rows = $(".lvDailyCLGrid tr");
            var firstRow = $(".lvDailyCLGrid").find("#trInsertItemTemplate");
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
            else {
                $("#hdnNew").val("New");
            }
        }

        $('.lvDailyCLGrid tr td .txtUpdate').focus(function () {
            var row = $(this).closest("tr");
            row.find('#hdnUpdate').val("update");
        });
        $('.lvDailyCLGrid tr td .txtUpdate').change(function () {
            debugger;
            var row = $(this).closest("tr");
            row.find('#hdnUpdate').val("update");
        });

        $("#btnNew").click(function () {
            $("#hdnNew").val("New");
            $('#btnNew').hide();
            $('#btnCancel').show();
            showHideFirstRow();
            return false;
        });
        $("#btnCancel").click(function () {
            $("#hdnNew").val() = "";
            $('#btnCancel').hide();
            $('#btnNew').show();
            showHideFirstRow();
            return false;
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $("#lbMultiMachineID").multiselect({
                includeSelectAllOption: true
            });
            $(".lbMethod").multiselect({
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
            $('#btnCancel').hide();
            showHideFirstRow();


            $('.lvDailyCLGrid tr td .txtUpdate').focus(function () {
                var row = $(this).closest("tr");
                row.find('#hdnUpdate').val("update");
            });
            $('.lvDailyCLGrid tr td .txtUpdate').change(function () {
                debugger;
                var row = $(this).closest("tr");
                row.find('#hdnUpdate').val("update");
            });

            $("#btnNew").click(function () {
                $("#hdnNew").val("New");
                $('#btnNew').hide();
                $('#btnCancel').show();
                showHideFirstRow();
                return false;
            });
            $("#btnCancel").click(function () {
                $("#hdnNew").val() = "";
                $('#btnCancel').hide();
                $('#btnNew').show();
                showHideFirstRow();
                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
