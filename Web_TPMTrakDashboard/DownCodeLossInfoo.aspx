<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="DownCodeLossInfoo.aspx.cs" Inherits="Web_TPMTrakDashboard.DownCodeLossInfoo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <style>
        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            text-align: center;
            position: sticky;
            top: -1px;
            z-index: 4;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        #gvGrid tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #gvGrid tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        .textboxcommon {
            border: none;
            background: transparent;
        }

        #tblfilter {
            box-shadow: 1px 1px 10px #515151;
        }

            #tblfilter tr td {
                padding: 10px;
            }

        .footercss {
            position: sticky;
            bottom: 0px;
            z-index: 4;
        }
    </style>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <div class="row">
        <div class="col-lg-10" style="text-align: left; display: block; margin-bottom: 10px;">
            <table id="tblfilter">
                <tr>
                    <td style="color: white;">Down Category</td>
                    <td>
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDownCategory" ClientIDMode="Static" OnSelectedIndexChanged="ddlDownCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="color: white;">Down ID</td>
                    <td>
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDownID" ClientIDMode="Static" OnSelectedIndexChanged="ddlDownID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="container" style="height: 75vh; overflow: auto;">
        <asp:GridView ID="gvGrid" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" ClientIDMode="Static" ShowHeaderWhenEmpty="true" EmptyDataText="No records has been added." ShowFooter="true" OnRowDataBound="gvGrid_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Down ID">
                    <ItemTemplate>
                        <asp:Label ID="lblDownCate" runat="server" Text='<%# Eval("DownID") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList runat="server" ID="ddlid" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        <%--<asp:TextBox ID="txtDownID" MaxLength="30" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>--%>
                    </FooterTemplate>
                </asp:TemplateField>
               <%-- <asp:TemplateField HeaderText="Subloss ID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSublossid" Text='<%# Eval("SubLossID")%>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox runat="server" CssClass="form-control" ClientIDMode="Static" ID="txtSublossID"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>--%>
                  <asp:TemplateField HeaderText="SubLoss">
                    <ItemTemplate>
                          <asp:HiddenField ID="hdfSublossDesc" runat="server" Value='<%# Eval("SubLossDescription")%>' />
                        <asp:TextBox ID="txtEditSublossDesc" runat="server" MaxLength="50"
                            Text='<%# Eval("SubLossDescription")%>' CssClass="txtupdate textboxcommon"></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtSublossDesc" MaxLength="50" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Subloss Interface ID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSublossinterfaceid" Text='<%# Eval("SubLossInterfaceID")%>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox runat="server" CssClass="form-control allowNumber" ClientIDMode="Static" ID="txtSublossinterfaceID"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRemove" runat="server" ClientIDMode="Static" CssClass="btn btn-info" Text="Delete" OnClick="lnkRemove_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-info"
                            OnClick="btnAdd_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-info"
                            OnClick="btnUpdate_Click" />
                    </FooterTemplate>
                    <FooterStyle Width="160px" />
                </asp:TemplateField>

            </Columns>
            <FooterStyle CssClass="footercss" />
            <HeaderStyle CssClass="HeaderCss" />
        </asp:GridView>
    </div>
     <div class="modal fade" id="deleteConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <span style="color: black">Are you sure you want to Delete this Record?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" style="width: 80px;" onclick="lnkRemove_Click">Yes</button>
                    <button type="button" style="width: 80px;" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            succMesg(<%: SuccMessage %>);
            function succMesg(msg) {
                if (msg == 1) {
                    messageOk("Details added/updated successfully !!");
                } else if (msg == 2) {
                    messageOk("Details deleted successfully. !!");
                } else if (msg == 3) {
                    messageNotOk("Interface ID already exists!!");
                }
            }
            function messageOk(msg) {
                Command: toastr["success"](msg, "Information Message")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-center",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "500",
                    "timeOut": "500",
                    "extendedTimeOut": "500",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }

            function messageNotOk(msg) {
                Command: toastr["error"](msg, "Error Message")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "500",
                    "timeOut": "500",
                    "extendedTimeOut": "500",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }

            $("[id$=gvGrid]").on("click", ".txtupdate", function () {
                $("[id$=gvGrid] tr td").find('.txtupdate').removeClass("form-control");
                $("[id$=gvGrid] tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });

            $("[id$=btnUpdate]").click(function () {
                var count = 0;
                $("[id$=gvGrid] tr:gt(0)").each(function (src, i) {
                    if (($(this, i).closest("tr").find('.txtupdate').val() == "")) {
                        count++;
                        alert('Please enter Sub Loss Description !!');
                        $(this, i).closest("tr").find('.txtupdate').focus();
                        return false;
                    }
                });
                if (count != 0) {
                    return false;
                }
            });

            $("[id$=btnAdd]").click(function () {
                //if ($("#txtSublossID").val() == "") {
                //    alert('Please Enter Sub Loss ID!!');
                //    $("#txtSublossID").focus()
                //    return false;
                //}
                if ($("#txtSublossinterfaceID").val() == "") {
                    alert('Please Enter Sub Loss Interface ID!!');
                    $("#txtSublossinterfaceID").focus()
                    return false;
                }
                if ($("#txtSublossDesc").val() == "") {
                    alert('Please Enter Sub Loss Description !!');
                    $("#txtSublossDesc").focus()
                    return false;
                }
            });
            $('.allowNumber').keypress(function (evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 48 && pos == 0) {
                    return false
                } else if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            });
        });
        function deleteDownCode(btn) {
            debugger;
            $('#deleteConfirmModal').modal('show');
            $('.allowNumber').keypress(function (evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 48 && pos == 0) {
                    return false
                } else if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
