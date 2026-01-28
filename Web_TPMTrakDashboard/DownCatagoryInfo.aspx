<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="DownCatagoryInfo.aspx.cs" Inherits="Web_TPMTrakDashboard.DownCatagoryInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        #MainContent_gridviewDownCat tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #MainContent_gridviewDownCat tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        .textboxcommon {
            border: none;
            background: transparent;
        }
    </style>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <div class="container">
        <div style="text-align: center">
            <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
        </div>
        <%--     OnRowEditing="EditDownCat" OnRowDataBound="OnRowDataBound" --%>
        <asp:GridView ID="gridviewDownCat" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"
            DataKeyNames="Item1"
            EmptyDataText="No records has been added." AllowPaging="true" ShowFooter="true"
            OnPageIndexChanging="OnPaging"
            PageSize="50">
            <Columns>
                <asp:TemplateField HeaderText="<%$Resources:CommanResource,DownCatagory %>">
                    <ItemTemplate>
                        <asp:Label ID="lblDownCate" runat="server" Text='<%# Eval("Item1") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtAddDownCat"
                            MaxLength="30" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$Resources:CommanResource,DownsDescription %>">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdfDownDesc" runat="server" Value='<%# Eval("Item2")%>' />
                        <asp:TextBox ID="txtEditDownDesc" runat="server" MaxLength="50"
                            Text='<%# Eval("Item2")%>' CssClass="txtupdate textboxcommon"></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtAddDownDesc" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRemove" runat="server"
                            CommandArgument='<%# Eval("Item1")%>' CommandName='<%# Eval("Item2")%>'
                            OnClientClick="return confirm('Do you want to delete?')"
                            Text="<%$Resources:CommanResource,Delete %>" OnClick="DeleteDownCat"></asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnAdd" runat="server" Text="<%$Resources:CommanResource,Add %>" CssClass="btn btn-info"
                            OnClick="AddNewDownCat" />
                        <asp:Button ID="btnUpdate" runat="server" Text="<%$Resources:CommanResource,Update %>" CssClass="btn btn-info"
                            OnClick="UpdateDownCat" />
                    </FooterTemplate>
                </asp:TemplateField>

            </Columns>
            <HeaderStyle CssClass="HeaderCss" />
        </asp:GridView>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {

            //-------Search Start-------------------------
            $('th').each(function (col) {
                
                if ($(this).html().trim() != "&nbsp;") {
                    $(this).hover(
                    function () { $(this).addClass('focus'); },
                    function () { $(this).removeClass('focus'); }

              );
                    $(this).click(function () {
                        var headerTxt = $(this).html().trim();
                        if ($(this).is('.asc')) {
                            $(this).removeClass('asc');
                            $(this).addClass('desc');
                            sortOrder = -1;
                        }
                        else {
                            $(this).addClass('asc');
                            $(this).removeClass('desc');
                            sortOrder = 1;
                        }

                        $(this).siblings().removeClass('asc');
                        $(this).siblings().removeClass('desc');
                        var arrData = $('table').find('tbody >tr:has(td)').get();
                        arrData.sort(function (a, b) {
                            var val1 = $(a).children('td').eq(col).text().toUpperCase();
                            var val2 = $(b).children('td').eq(col).text().toUpperCase();
                            if ($.isNumeric(val1) && $.isNumeric(val2))
                                return sortOrder == 1 ? val1 - val2 : val2 - val1;
                            else
                                return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                        });
                        $.each(arrData, function (index, row) {
                            $('tbody').append(row);
                        });
                    });
                }
            });
            //---------End Search-----------------------------------

            succMesg(<%: SuccMessage %>);
            function succMesg(msg) {
                if (msg == 1) {
                    messageOk("Details added/updated successfully !!");
                } else if (msg == 2) {
                    messageNotOk(" There are some DownIDs belonging to this category. </br>  Please change the DownIDs category before deletion.");
                } else if (msg == 3) {
                    messageOk("Details deleted successfully. !!");
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

            $("[id$=gridviewDownCat]").on("click", ".txtupdate", function () {
                $("[id$=gridviewDownCat] tr td").find('.txtupdate').removeClass("form-control");
                $("[id$=gridviewDownCat] tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });

            $("[id$=btnUpdate]").click(function () {
                var count = 0;
                $("[id$=gridviewDownCat] tr:gt(0)").each(function (src, i) {
                    if (($(this, i).closest("tr").find('.txtupdate').val() == "")) {
                        count++;
                        alert('Please enter Down Description !!');
                        $(this, i).closest("tr").find('.txtupdate').focus();
                        return false;
                    }
                });
                if (count != 0) {
                    return false;
                }
            });

            $("[id$=btnAdd]").click(function () {
                if ($("#MainContent_gridviewDownCat_txtAddDownCat").val() == "") {
                    alert('Please Enter Down Catagory !!');
                    $("#MainContent_gridviewDownCat_txtAddDownCat").focus()
                    return false;
                }
                if ($("#MainContent_gridviewDownCat_txtAddDownDesc").val() == "") {
                    alert('Please Enter Down Description !!');
                    $("#MainContent_gridviewDownCat_txtAddDownDesc").focus()
                    return false;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
