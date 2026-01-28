<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProgramTransferPage.aspx.cs" EnableEventValidation="false" Inherits="Web_TPMTrakDashboard.MachineConnect.ProgramTransferPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" />
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .align-righttd {
            text-align: right;
            padding-right: 5px;
        }
        .progTransTh{
            background-color:#2E6886;
            position:sticky;
            top:0;
            z-index:100;
        }

        .table-header-labels {
            text-align: center;
            font-weight: bolder;
            color: black;
            width: 20%;
            height: 3%;
        }

        .table-data {
            text-align: center;
            width: 20%;
            vertical-align: middle !important;
        }

        .table-data-labels {
            color: black;
            font-weight: 400;
        }

        .treeView img {
            width: 20px;
            height: 20px;
        }

        .scrollable-label {
            overflow: auto;
            font-size: 20px;
            font-weight: 400;
            resize: none;
            height: 400px;
            width: 100%;
        }

        .treeView table tr td {
            border: none !important;
            padding-bottom: 0px !important;
            padding-right: 0px !important;
        }

        .local-table-tree{
            vertical-align:middle !important;
        }
        .buttons-container {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .button {
            background-color: darkblue;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 19px;
            border: 1px solid #ccc;
            cursor: pointer;
            width:130px;
            height:75px;
            white-space:normal;
        }
        .down-arrow {
            width: 5px;
            height: 5px;
            border-left: 10px solid transparent;
            border-right: 10px solid transparent;
            border-top: 10px solid white;
            display: inline-block;
            margin-left: 5px;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td class="commontd">
                            <asp:Label runat="server" Text="PLant ID:" CssClass="align-righttd" />
                        </td>
                        <td class="commontd">
                            <asp:DropDownList runat="server" ID="ddlPlantID" CssClass="form-control txtstyle" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged"/>
                        </td>
                        <td class="commontd">
                            <asp:Label runat="server" Text="Machine ID:" Width="120px" CssClass="align-righttd" />
                        </td>
                        <td class="commontd">
                            <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control txtstyle" Width="200px" />
                        </td>
                        <td style="text-align: center; width: 200px;">
                            <asp:Button runat="server" ID="btnCNCDir" Text="Show CNC dir." ClientIDMode="Static" OnClientClick="ShowLoader()" CssClass="btn btn-primary" Width="140px" OnClick="btnCNCDir_Click" />
                        </td>
                        <td style="text-align: center; width: 200px;">
                            <asp:Button runat="server" ID="btnSettings" Text="Settings" CssClass="btn btn-primary" Width="140px" OnClick="btnSettings_Click" />
                        </td>
         <%--               <td style="text-align: center; width: 200px;">
                            <asp:Button runat="server" ID="btnCompareProg" Text="Compare Programs" CssClass="btn btn-primary" Width="180px" OnClick="btnCompareProg_Click" />
                        </td>--%>
                    </tr>
                </table>
            </div>
            <div style="display: flex; margin-top: 10px;">
                <div style="margin-top: 10px; height: 85vh; width: 20%; flex: 2.9; margin: 5px;">
                    <table class="table table-bordered" style="height: 100%; background-color: transparent; vertical-align: top;">
                        <tr>
                            <th id="thLocal1" colspan="2" runat="server" class="table-header-labels" style=";color:white;background-color:#2E6886;">Computer</th>
                            <%--<th class="table-header-labels" style="width: 60%;color:white;background-color:#2E6886;">Computer Programs</th>--%>
                        </tr>
                        <tr  style="background-color:whitesmoke;">
                            <td style="padding: 0px !important;width:40%;">
                                <asp:TreeView runat="server" CssClass="treeView" ClientIDMode="Static" SelectedNodeStyle-ForeColor="OrangeRed" ID="treeComputerDir" ForeColor="DarkBlue" Font-Bold="true" NodeIndent="4" OnSelectedNodeChanged="treeView1_SelectedNodeChanged" onchange="Hello();" />
                            </td>
                            <td style="padding: 0px">
                                <div style="height:80vh;overflow: auto;">
                                    <asp:ListView runat="server" ID="lvLocalFiles" ClientIDMode="Static">
                                        <LayoutTemplate>
                                            <table id="tblLocalFiles" class="table table-bordered" style="background-color: transparent; border: none !important; width: 100%">
                                                <tr>
                                                    <th class="table-header-labels progTransTh" style="width: 15%;">
                                                        <asp:CheckBox runat="server" ClientIDMode="Static" ID="cbxLocalFilesAll" onchange="toggleCheckboxesLocal(this)" />
                                                    </th>
                                                    <th class="table-header-labels progTransTh" style="width: 85%; color: white; background-color: #2E6886;">File Names</th>
                                                </tr>
                                                <tr id="itemplaceholder" runat="server"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr style="padding: 0px !important;">
                                                <td style="width: 15%;" class="table-data">
                                                    <asp:CheckBox runat="server" ClientIDMode="Static" ID="cbxProgsLocal" />
                                                </td>
                                                <td style="width: 85%;" class="table-data">
                                                    <asp:LinkButton runat="server" ID="lbFiles" ForeColor="DarkBlue" Font-Bold="true" Text='<%# Eval("FileNames") %>' OnClick="lbFiles_Click" />
                                                    <asp:HiddenField runat="server" ID="hfFilePath" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-top: 10px; height: 85vh; width: 20%; flex: 0.9; margin:0px; background-color: transparent;display: flex;justify-content: center;align-items: center;">
                        <div class="buttons-container">
                            <asp:Button ID="btnUploadProgs" runat="server" CssClass="button" OnClientClick="ShowLoader();" OnClick="btnUploadProgs_Click"/>
                            <asp:Button ID="btnDownloadProgs" runat="server" CssClass="button" OnClientClick="ShowLoader();" OnClick="btnDownloadProgs_Click"/>
                            <asp:Button ID="btnDeleteProg" runat="server" CssClass="button" OnClick="btnDeleteProg_Click"/>
                            <%--<asp:Button ID="Button5" runat="server" CssClass="button" Text="Compare Programs" />--%>
                        </div>
                </div>
                <div style="margin-top: 10px; height: 85vh; width: 20%; flex: 1.3; margin: 5px 0px 5px 10px; background-color: transparent;">
                    <%--<div class="buttons-container" style="gap:0px;">--%>
                    <table class="table table-bordered" style="height: 85vh; background-color: transparent; vertical-align: top;">
                        <tr>
                            <th class="table-header-labels" style="color:white;background-color:#2E6886;">CNC Folders</th>
                        </tr>
                        <tr style="background-color:whitesmoke;">
                            <td>
                                <asp:TreeView runat="server" CssClass="treeView" SelectedNodeStyle-ForeColor="OrangeRed" ClientIDMode="Static" ID="treeViewMachineFolders" ForeColor="DarkBlue" Font-Bold="true" NodeIndent="4" OnSelectedNodeChanged="treeViewMachineFolders_SelectedNodeChanged"  HoverNodeStyle-ForeColor="OrangeRed"/>
                            </td>
                        </tr>
                    </table>
    <%--                    <asp:Button ID="btnDeleteProgs" runat="server" CssClass="button" Text="Delete Programs"  />
                        </div>--%>
                </div>
                <div style="margin-top: 10px; height: 85vh; width: 60%; flex: 4.8; margin: 5px; overflow: auto; background-color: transparent;">
                    <div class="buttons-container">

                        <asp:ListView runat="server" ID="lvMachinePrograms" ClientIDMode="Static">
                            <LayoutTemplate>
                                <table id="tblMachProgs" class="table table-bordered">
                                    <tr>
                                        <th class="table-header-labels progTransTh" colspan="4">
                                            <asp:Label ForeColor="White" runat="server" ID="lblMachinePath" />
                                        </th>
                                    </tr>
                                    <tr>
                                        <th class="table-header-labels progTransTh" style="width: 5%;">
                                            <asp:CheckBox runat="server" ClientIDMode="Static" ID="cbxMachineProgsAll" onchange="toggleCheckboxes(this)" />
                                        </th>
                                        <th class="table-header-labels progTransTh" style="width: 15%; color: white; background-color: #2E6886;">
                                            <asp:Label runat="server" Text="Prog No."></asp:Label>
                                            <asp:LinkButton ID="btnSortPrg" runat="server" OnClick="btnSortPrg_Click" class="down-arrow" />
                                        </th>
                                        <th class="table-header-labels progTransTh" style="width: 50%; color: white; background-color: #2E6886;">Comments</th>
                                        <th class="table-header-labels progTransTh" style="width: 35%; color: white; background-color: #2E6886;">
                                            <asp:Label runat="server" Text="Modified On."></asp:Label>
                                            <asp:LinkButton ID="btnSortMod" runat="server" OnClick="btnSortMod_Click" class="down-arrow" />
                                        </th>
                                        <%--<th class="table-header-labels progTransTh" style="width: 20%;color:white;background-color:#2E6886;">Actions</th>--%>
                                    </tr>
                                    <tr id="itemplaceholder" runat="server"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr style="background-color: <%# Eval("RowColor") %>">
                                    <td class="table-data" style="width: auto;">
                                        <asp:CheckBox runat="server" ID="cbxProgs" ClientIDMode="Static" />
                                    </td>
                                    <td class="table-data">
                                        <asp:LinkButton runat="server" ID="lbProgNo" ForeColor="DarkBlue" Font-Bold="true" Font-Underline="true" CssClass="table-data-labels" Text='<%# Eval("ProgNo") %>' OnClick="lbProgNo_Click" />
                                    </td>
                                    <td class="table-data">
                                        <asp:Label runat="server" ID="lblComments" CssClass="table-data-labels" Text='<%# Eval("ProgComments") %>' />
                                        <asp:HiddenField runat="server" ID="hfProgLength" Value='<%# Eval("ProgLength") %>' />
                                    </td>
                                    <td class="table-data">
                                        <asp:Label runat="server" ID="lblModified" CssClass="table-data-labels" Text='<%# Eval("ProgModifiedTs") %>' />
                                    </td>
                                    <%--                                <td style="text-align: center">
                                    <asp:LinkButton runat="server" ID="lnkDownload" ClientIDMode="Static" CssClass="btn btn-info" BackColor="DarkBlue" OnClick="lnkDownload_Click" ToolTip="Download">
                                        <i class="fa fa-file-download"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkdltList" CssClass="btn btn-info" BackColor="Red" ToolTip="Delete" OnClick="lnkdltList_Click">
                                        <i class="fa fa-trash"></i>
                                    </asp:LinkButton>
                                </td>--%>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
            <div class="modal" id="progViewModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered " style="width: 1000px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="modalTitle" runat="server" style="color: black; font-weight: bold; font-size: large;">Program Content</h4>
                        </div>
                        <div class="modal-body" style="padding-left: 0px; padding-right: 0px; background-color: white">
                            <asp:TextBox ID="txtProgData" runat="server" Rows="10" TextMode="MultiLine" CssClass="scrollable-label" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="SAVE" Width="100px" CssClass="btn btn-info" OnClick="btnSave_Click" />
                            <asp:Button runat="server" ID="btnProgViewmodalCancel" Text="CANCEL" class="btn btn-info" Style="width: 100px;" OnClick="btnProgViewmodalCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal" id="progCompareModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered " style="width: 1500px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="H1" runat="server" style="color: black; font-weight: bold; font-size: large;">Compare Programs</h4>
                        </div>
                        <div class="modal-body" style="padding-left: 0px; padding-right: 0px; background-color: white">
                            <table style="width: 100%;">
                                <tr>
                                    <th style="width: 48%; text-align: center;">Program 1</th>
                                    <th style="width: 2%;" />
                                    <th style="width: 48%; text-align: center;">Program 2</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server" Rows="10" TextMode="MultiLine" CssClass="scrollable-label" />
                                    </td>
                                    <td />
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" Rows="10" TextMode="MultiLine" CssClass="scrollable-label" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="Button2" Text="CLOSE" class="btn btn-info" Style="width: 100px;" OnClick="btnProgViewmodalCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal" id="progTransSettings" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-dialog-centered " style="width: 950px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="H2" runat="server" style="color: black; font-weight: bold; font-size: large;">Program Transfer Settings</h4>
                        </div>
                        <div class="modal-body" style="padding-left: 0px; padding-right: 0px; background-color: white">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align:right;">
                                        <asp:Label runat="server" Text="Program Folder Path:" />
                                    </td>
                                    <td class="commontd">
                                        <asp:TextBox runat="server" ID="txtPrgFolderPath" CssClass="form-control txtstyle" Width="250px" />
                                    </td>
                                    <td  style="text-align:right;">
                                        <asp:Label runat="server" ForeColor="Black" Text="Program File Extension:" CssClass="align-righttd" />
                                    </td>
                                    <td class="commontd">
                                        <asp:DropDownList runat="server" ID="ddlExtension" CssClass="form-control txtstyle" Width="200px"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSaveSettings" ClientIDMode="Static" Text="SAVE" Width="100px" CssClass="btn btn-info" OnClick="btnSaveSettings_Click" />
                            <asp:Button runat="server" ID="btnCancelSettings" Text="CANCEL" class="btn btn-info" Style="width: 100px;" OnClick="btnCancelSettings_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal" id="ReplaceConfirmationModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered " style="width: 500px;">
                     <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" style="color: black; font-weight: bold; font-size: large;">Programs below already exists in CNC Folder. Select the programs to replace.</h4>
                        </div>
                        <div class="modal-body" style="padding-left: 0px; padding-right: 0px; background-color: white;overflow:auto;height:400px;">
                            <asp:ListView runat="server" ID="lvLocalFilesModal" ClientIDMode="Static">
                                    <LayoutTemplate>
                                        <table id="tblLocalModal" class="table table-bordered" style="background-color:transparent;border:none !important;width:100%;">
                                            <tr style="height:50px;">
                                                <th class="table-header-labels progTransTh" id="tblLocalFilesModal" style="width: 30%;">
                                                    <asp:CheckBox runat="server" ClientIDMode="Static" ID="cbxLocalFilesAllModal" onchange="toggleCheckboxesModal(this);" />
                                                </th>
                                                <th class="table-header-labels progTransTh" style="width:70%;color:white;">Program Number</th>
                                            </tr>
                                            <tr id="itemplaceholder" runat="server"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr style="background-color: <%# Eval("RowColor") %>">
                                            <td style="width: 30%;" class="table-data">
                                                <asp:CheckBox runat="server" ID="cbxProgsLocalModal" Enabled='<%# Eval("RowEnabled") %>' />
                                            </td>
                                            <td style="width: 70%;" class="table-data">
                                                <asp:Label runat="server" ID="lbFilesModal" Font-Bold="true" Text='<%# Eval("ProgNo") %>' />
                                                <asp:HiddenField runat="server" ID="hfFilePathModal" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                        </div>
                        <div class="modal-footer" style="border-top: 1px solid #3777bc; text-align: center">
                            <asp:Button  ID="btnYes" runat="server" Text="Confirm" class="btn btn-info" style="background-color: #3777bc; color: white" OnClick="btnYes_Click" OnClientClick="ShowLoader();"/>
                            <asp:Button ID="btnNo" runat="server" Text="Cancel" class="btn btn-info" style="background-color: #3777bc; color: white" OnClick="btnNo_Click" OnClientClick="ShowLoader();"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal" id="DeleteConfirmationModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                    <div class="modal-content" style="border: 2px solid #3777bc">
                        <div class="modal-header" style="background-color: #3777bc; padding: 8px">
                            <h4 class="modal-title" style="color: white;">Delete Confirmation</h4>
                        </div>
                        <div class="modal-body" style="background-color: white;height:100%">
                            <span id="DeleteconfirmationText" style="font-size: 17px;">Are you sure You want to delete the selected program(s)?</span>
                        </div>
                        <div class="modal-footer" style="border-top: 1px solid #3777bc; text-align: center">
                            <asp:Button ID="btnYesDelete" runat="server" Text="Yes" class="btn btn-info" Style="background-color: #3777bc; color: white" OnClick="btnYesDelete_Click" OnClientClick="ShowLoader();"/>
                            <asp:Button ID="btnNoDelete" runat="server" Text="No" class="btn btn-info" Style="background-color: #3777bc; color: white" OnClick="btnNoDelete_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function ShowLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }
        function HideLoader() {
            $.unblockUI({});
        }
        function toggleCheckboxes() {
            var mainChk = $('[id$=cbxMachineProgsAll]').prop("checked");
            for (let i = 1; i < $("#tblMachProgs tr").length; i++) {
                var tr = $("#tblMachProgs tr")[i];
                var checkbox = $(tr).find("[id$=cbxProgs]");
                checkbox.prop("checked", mainChk);
            }
        }
        function toggleCheckboxesModal() {
            var mainChk = $('[id$=cbxLocalFilesAllModal]').prop("checked");
            for (let i = 1; i < $("#tblLocalModal tr").length; i++) {
                var tr = $("#tblLocalModal tr")[i];
                var checkbox = $(tr).find("[id$=cbxProgsLocalModal]");
                var chkken = !checkbox.prop("disabled");
                if (chkken) {
                    checkbox.prop("checked", mainChk);
                }
            }
        }
        function toggleCheckboxesLocal() {
            debugger;
            var mainChk = $('[id$=cbxLocalFilesAll]').prop("checked");
            for (let i = 1; i < $("#tblLocalFiles tr").length; i++) {
                var tr = $("#tblLocalFiles tr")[i];
                var checkbox = $(tr).find("[id$=cbxProgsLocal]");
                checkbox.prop("checked", mainChk);
            }
        }
    </script>
</asp:Content>