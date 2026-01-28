<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyCheckSheetMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.PrecisionEngineering.DailyCheckSheetMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
      <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
            width: 100%;
        }

        .headerFixer > tbody > tr > td {
            background-color: white;
        }
    </style>
    <asp:HiddenField ID="hdfCondition" runat="server" />
    <div class="col-lg-12">
        <div class="col-lg-10" style="margin-bottom: 10px; padding-left: 0px !important;">
            <table>
                <tr>
                    <td>
                        <%--<asp:DropDownList ID="ddlGroupID" ClientIDMode="Static" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupID_SelectedIndexChanged"></asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlMachineID" ClientIDMode="Static" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnNew" runat="server" CssClass="bajaj-btn-style" Text="New" Style="background-color: #77f100; border-radius: 5px; margin-left: 10px;" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="bajaj-btn-style" Text="Cancel" Style="background-color: #FF9191; border-radius: 5px; margin-left: 10px; display: none;" />
                        <asp:Button ID="BtnSave" runat="server" CssClass="bajaj-btn-style" Text="Save" Style="background-color: deepskyblue; border-radius: 5px; margin-right: 10px;" OnClick="BtnSave_Click" />
                    </td>
                    <td style="border: 1px solid; color: white; margin: 5px 10px">
                        <asp:FileUpload runat="server" ID="fileupload" Style="width: 210px;" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkImport" CssClass="bajaj-btn-style" Text="Import" OnClick="lnkImport_Click" Style="background-color: deepskyblue; border-radius: 5px; margin-left: 10px; color: black" Visible="true" ToolTip="Import" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnCopy" CssClass="bajaj-btn-style" Text="Copy" Style="background-color: deepskyblue; border-radius: 5px; margin-left: 10px; color: black" OnClientClick="return openCopyModal();" ClientIDMode="Static" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-lg-2" style="float: right;">
            <asp:LinkButton ID="btnTemplateExport" CssClass="glyphicon glyphicon-download-alt" Text="DownloadTemplate" runat="server" ToolTip="Template" Font-Size="20px" Style="display: inline-block; vertical-align: top; width: 200px;" OnClick="btnTemplateExport_Click" />
        </div>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:ListView runat="server" ID="lvDailyCheckSheetMaster" ClientIDMode="Static" class="table table-bordered table-hover" InsertItemPosition="LastItem" OnItemDataBound="lvDailyCheckSheetMaster_ItemDataBound">
                <EmptyDataTemplate>
                    <table>
                        <tr>
                            <td>No Data Found</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr class="addtablerow" style="display: none">
                        <td>
                            <asp:TextBox ID="txtCheckPointID" runat="server" ClientIDMode="Static" CssClass="form-control allowNumber"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInsertCheckPointDesc" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInsertCheckPointDescInHindi" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control">
                                <asp:ListItem Text="Daily" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Weekly" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Monthly" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                </InsertItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdfUpdate" runat="server" ClientIDMode="AutoID" />
                            <asp:Label ID="lblCheckPointID" Text='<%# Bind("CheckPointID") %>' runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCheckPointDesc" Text='<%# Bind("CheckPointDesc") %>' runat="server" ClientIDMode="Static" CssClass="txtupdate textboxcommon"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCheckPointDescInHindi" Text='<%# Bind("CheckPointDescInHindi") %>' runat="server" ClientIDMode="Static" CssClass="txtupdate textboxcommon"></asp:TextBox>
                        </td>
                        
                        <td>
                            <asp:HiddenField runat="server" Value='<%# Bind("Frequency") %>' ID="hdnFrequency" />
                            <asp:HiddenField runat="server" Value='<%# Bind("FrequencyOrder") %>' ID="hdnFrequencyOrder" />
                            <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="txtupdate textboxcommon">
                                <asp:ListItem Text="Daily" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Weekly" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Monthly" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                            <%--<asp:Label runat="server" ClientIDMode="Static" ID="lblFrequency" Text='<%# Bind("Frequency") %>'></asp:Label>--%>
                        </td>
                        <td style="text-align: center; width: 150px;">
                            <asp:LinkButton ID="lnkCheckPoint" runat="server" CommandArgument='<%# Eval("CheckPointID")%>' Text="Delete" OnClick="lnkCheckPoint_Click" CssClass="bajaj-btn-style" Style="background-color: deepskyblue; border-radius: 5px; color: white"></asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table class="table table-bordered headerFixer dailyChecklsttbl" style="margin: 15px 3px;">
                        <tr>
                            <th>Check Point ID</th>
                            <th>Check Point Desc</th>
                            <th>Check Point Desc In Hindi</th>
                            <th>Frequency</th>
                            <th id="Th1" runat="server" style="text-align: center"><%=GetGlobalResourceObject("CommanResource","Action") %></th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server"></tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            </div>
              <div class="modal infoModal bajaj-info-modal" id="copyModal" role="dialog" style="min-width: 500px;" data-backdrop="static" data-keyboard="false">
                  <div class="modal-dialog" style="width: 600px;">
                      <div class="modal-content">
                          <div class="modal-header" style="background-color: #00b5ff !important;">
                              <h4 class="modal-title" style="color: black !important">Copy</h4>
                              <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal"></i>
                          </div>
                          <div class="modal-body">
                              <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                  <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                      <tr>
                                          <td>Source Machine</td>
                                          <td>
                                              <asp:Label runat="server" ID="lblCopySourceMachine" ClientIDMode="Static"></asp:Label>
                                          </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 180px;">Destination Machine</td>
                                          <td>
                                              <asp:ListBox ID="lbCopyDestMachine" runat="server" SelectionMode="Multiple" CssClass="form-control" ClientIDMode="Static"></asp:ListBox>
                                          </td>
                                      </tr>
                                  </table>
                              </div>
                          </div>
                          <div class="modal-footer" style="text-align: right !important">
                              <asp:Button runat="server" ID="btnCopyConfirm" Text="Save" CssClass="confirm-modal-btn" OnClientClick="return copyValidation();" OnClick="btnCopyConfirm_Click" />
                              <input type="button" value="Close" class="error-modal-btn" data-dismiss="modal" />
                          </div>
                      </div>
                  </div>
              </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        $(document).ready(function () {
            var winHeight = $(window).height();
            $("#lvDailyCheckSheetMaster").height(winHeight - 125);

            $("[id$=btnNew]").click(function () {
                $(".addtablerow").css("display", "");
                $("[id$=btnCancel]").css("display", "");
                $("[id$=hdfCondition]").val("Save");
                $("[id$=btnNew]").css("display", "none");
                $("[id$=txtCheckPointID]").focus();
                return false;
            });

            $("[id$=btnCancel]").click(function () {
                $(".addtablerow").css("display", "none");
                $("[id$=btnCancel]").css("display", "none");
                $("[id$=hdfCondition]").val("Update");
                $("[id$=btnNew]").css("display", "");
                $("#txtCheckPointID").val("");
                $("#txtInsertCheckPointDesc").val("");
                $("#txtInsertCheckPointDescInHindi").val("");
                return false;
            });

            $(".dailyChecklsttbl").on("click", ".txtupdate", function () {
                $("[id$=hdfCondition]").val("Update");
                $(this).closest('tr').find('[id$=hdfUpdate]').val("update");
                $(".dailyChecklsttbl tr td").find('.txtupdate').removeClass("form-control");
                $(".dailyChecklsttbl tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });
            setControls();
        });
        function setControls() {
            $('[id$=lbCopyDestMachine]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px',
            });
        }
        function openCopyModal() {
            $('#copyModal').modal('show');
            return false;
        }
        function copyValidation() {
            if ($('#lbCopyDestMachine').val() == "" || $('#lbCopyDestMachine').val() == null) {
                openWarningModal_1("Please select destination machine.")
                return false;
            }
            return true;
        }
        function openSuccessModal(msg) {
            $('#toast-container').empty();
            Command: toastr["success"](msg)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function showWarningMsg(msg, title) {
            debugger;
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "4000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "toastClass": "toaster-position"
            }
            toastr['warning'](msg, title);
            return false;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var winHeight = $(window).height();
            $("#lvDailyCheckSheetMaster").height(winHeight - 125);

            $("[id$=btnNew]").click(function () {
                $(".addtablerow").css("display", "");
                $("[id$=btnCancel]").css("display", "");
                $("[id$=hdfCondition]").val("Save");
                $("[id$=btnNew]").css("display", "none");
                $("[id$=txtCheckPointID]").focus();
                return false;
            });

            $("[id$=btnCancel]").click(function () {
                $(".addtablerow").css("display", "none");
                $("[id$=btnCancel]").css("display", "none");
                $("[id$=hdfCondition]").val("Update");
                $("[id$=btnNew]").css("display", "");
                $("#txtCheckPointID").val("");
                $("#txtInsertCheckPointDesc").val("");
                $("#txtInsertCheckPointDescInHindi").val("");
                return false;
            });

            $(".dailyChecklsttbl").on("click", ".txtupdate", function () {
                $("[id$=hdfCondition]").val("Update");
                $(this).closest('tr').find('[id$=hdfUpdate]').val("update");
                $(".dailyChecklsttbl tr td").find('.txtupdate').removeClass("form-control");
                $(".dailyChecklsttbl tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });
            setControls();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
