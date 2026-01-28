<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PoojaParameter_GradeMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.Pooja.PoojaParameter_GradeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <style>
        .legendstyle {
            color: Black;
            width: 35%;
            border: 0px;
            margin: 0px 0px 0px 20px;
            padding: 0px 0px 0px 10px;
        }

        .innertbl {
            padding: 0px 10px;
        }

            .innertbl > tbody > tr > td {
                color: black;
                padding: 5px 10px;
            }

        #gvPoojaMaster > tbody > tr > th {
            text-align: center;
        }
        .gvGradeID {
            width: 100%;
        }
        .gvGradeID > tbody > tr > td{
            padding: 10px;
        }
         .gvGradeID > tbody > tr > th{
             text-align: center;
             height: 30px;
         }

    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <table class="table table-bordered" id="tblfilter" style="width: fit-content;">
                    <tr>
                        <td>
                            <input value="New" type="button" class="btn btn-primary" id="btnNew" style="display: inline; margin-right: 3px;" runat="server" onserverclick="btnNew_ServerClick" />
                            <input value="Cancel" type="button" class="btn btn-primary" id="btnCancel" style="display: inline; margin-right: 3px;" runat="server" onserverclick="btnCancel_ServerClick" />
                            <asp:Button runat="server" ID="btnEdit" Text="Edit" Visible="false" ClientIDMode="Static" />
                            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnCreateGradeID" Text="Create GradeID" ClientIDMode="Static" CssClass="btn btn-primary" />
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="btnTemplateExport" CssClass="glyphicon glyphicon-download-alt" Text="DownloadTemplate" runat="server" ToolTip="Template" Font-Size="20px" Style="display: inline-block; vertical-align: top;" OnClick="btnTemplateExport_Click" />
                        </td>
                        <td>
                            <asp:FileUpload ID="fileUploader" runat="server" Style="width: 400px; height: 40px; display: inline-block; max-width: 200px;" CssClass="form-control input-sm" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnImport" CssClass="btn btn-info" OnClick="btnImport_Click" Text="Import" />
                        </td>
                        <%--<td>Grade ID</td>--%>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlGradeIDFilter" CssClass="form-control"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" />
                        </td>
                    </tr>
                </table>
                </td>
                    </tr>
                </table>
                <div id="gridContainer" style="height: 80vh; overflow: auto; margin-top: 10px;">
                    <asp:GridView runat="server" AutoGenerateColumns="false" ID="gvPoojaMaster" OnRowDataBound="gvPoojaMaster_RowDataBound" CssClass="table table-bordered headerFixer  alternate-table-style" ShowHeaderWhenEmpty="true" ShowFooter="true" ClientIDMode="Static">
                        <Columns>
                            <asp:TemplateField HeaderText="Parameter">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfUpdate" runat="server" ClientIDMode="Static" />
                                    <asp:Label runat="server" ID="lblParameter" Text='<%# Eval("Parameter") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlParameter" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Grade ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblGradeID" Text='<%# Eval("GradeID") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%--<asp:DropDownList ID="ddlGradeID" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>--%>
                                    <asp:TextBox runat="server" ID="txtGradeID" CssClass="form-control"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Compare Type">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnCompareType" Value='<%# Eval("CompareType") %>' />
                                    <asp:DropDownList ID="ddlCompareType" runat="server" CssClass="form-control textupdate entryVal" OnSelectedIndexChanged="ddlCompareType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="Range" Text="Range"></asp:ListItem>
                                        <asp:ListItem Value="Target" Text="Target"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlCompareType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCompareType_SelectedIndexChanged1">
                                        <asp:ListItem Value="Range" Text="Range"></asp:ListItem>
                                        <asp:ListItem Value="Target" Text="Target"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LSL">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLSL" runat="server" CssClass="form-control textupdate allowDecimal entryVal" Text='<%# Eval("LSL") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtLSL" runat="server" CssClass="form-control allowDecimal"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="USL">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUSL" runat="server" CssClass="form-control textupdate allowDecimal entryVal" Text='<%# Eval("USL") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtUSL" runat="server" CssClass="form-control allowDecimal"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lbDelete" CssClass="action-icons delete-icons" ToolTip="Delete" OnClick="lbDelete_Click">
                                        <i class="glyphicon glyphicon-trash "></i>
                                        <span>DELETE</span> 
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" ID="lbAdd" CssClass="action-icons delete-icons" ToolTip="Add" OnClick="lbAdd_Click">
                                        <i class="glyphicon glyphicon-plus "></i>
                                        <span>ADD</span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnImport" />
                <asp:PostBackTrigger ControlID="btnTemplateExport" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <!--Message Popup -->
    <div id="MessagePopup" class="modal fade" role="dialog">
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

    <div class="modal infoModal" id="NewGradeID" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 650px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="newEditModalTitle" runat="server">Add New GradeID</h4>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <div>
                        <fieldset style="border: 1px solid black; margin: 0px 10px;">
                            <legend class="legendstyle">Add Grade ID</legend>
                            <table class="innertbl" style="margin-left: 50px;">
                                <tr>
                                    <td>Grade ID</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGradeID" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnSaveGradeID" Text="Save" OnClick="btnSaveGradeID_Click" ClientIDMode="Static" CssClass="btn btn-primary" />
                                    </td>
                                </tr>
                            </table> 
                        </fieldset>
                        <div style="height: 300px; overflow: auto; width: 80%; text-align: center;margin: 5px 0px 5px 30px;">
                            <asp:GridView runat="server" ID="gvGradeID" CssClass="headerFixer gvGradeID" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl No.">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSlNo" Text='<%# Eval("SlNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grade ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblGradeID" Text='<%# Eval("GradeID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete?">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary" OnClick="btnDelete_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary btn-style cancel-btn" onclick="CloseNewEditModal();">CANCEL</button>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="DeleteConfirmatioModal" role="dialog" style="z-index: 2000">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content modalContent confirm-modal-content">
                <div class="modal-header modalHeader confirm-modal-header">
                    <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                    <br />
                    <h4 class="confirm-modal-title">Confirmation!</h4>
                    <br />
                    <span class="confirm-modal-msg">Are you sure you want to Delete the Record?</span>
                </div>
                <div class="modal-footer modalFooter modal-footer">
                    <asp:Button runat="server" ID="btnDeleteConfirmation" CssClass="btn btn-primary" Text="Yes" OnClick="btnDeleteConfirmation_Click" />
                    <input type="button" value="No" data-dismiss="modal" class="btn-style confirm-modal-btn" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        debugger;
        var bigDiv = document.getElementById('gridContainer');

        $("#gvPoojaMaster .form-control").change(function () {
            $(this).closest('tr').find('input[type=hidden]').val("Update");
        });
        $(document).ready(function () {
            $(".entryVal").blur(function () {
                $(this).closest('tr').find("#hdfUpdate").val("Update");
            });
        });

        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        bigDiv.onscroll = function () {

            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }

        $("#btnSaveGradeID").on("click", function () {
            if($("#txtGradeID").val() == "") {
                openWarningModal_1("Grade ID can't be empty");
                return false;
            }
            return true;
        });

        function OpenConfirmationModal() {
            $("#DeleteConfirmatioModal").modal("show");
        }

        function CloseNewEditModal() {
            $('#NewGradeID').modal("hide");
        }

        $('#btnCreateGradeID').on("click", function () {
            $('#NewGradeID').modal("show");
            return false;
        })

        function OpenGradeIDModal() {
            $('#NewGradeID').modal("show");
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $("#gvPoojaMaster .form-control").change(function () {
                $(this).closest('tr').find('input[type=hidden]').val("Update");
            });
            $(document).ready(function () {
                $(".entryVal").blur(function () {
                    $(this).closest('tr').find("#hdfUpdate").val("Update");
                });
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();

            });

            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {

                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
