<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerInformation.aspx.cs" Inherits="Web_TPMTrakDashboard.CustomerInformation" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        #grdcustomer > tbody > tr:nth-child(odd) > td {
            background-color: #FFFFFF;
        }

        #grdcustomer > tbody > tr:nth-child(even) > td {
            background-color: #F2F2F2;
        }

        .blue {
            background-color: #2E6886 !important;
            /*cursor: pointer;*/
            height: 38px;
        }

            .blue th {
                color: white !important;
                /*cursor: pointer;*/
                text-align: center;
            }

        .textboxcss {
            border: none;
            background-color: transparent;
            /*font-style: italic;*/
            color: black;
            min-width: 40px;
        }

        .addtextcss {
            border: initial;
            background-color: none;
        }

        .rowheight {
            height: 46px;
        }

        footerheight {
            height: 38px;
            min-width: 40px;
        }

        .removefootercss {
            display: none;
            min-width: 40px;
        }

        .iconcss {
            color: red;
            text-align: center;
        }

        #grdcustomer tr td:first-child{
            min-width: 200px;
        }
    </style>
    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <div class="row" style="margin-left: 2px; margin-right: 5px;">
                <table class="table table-bordered" id="tblmenu" style="position: fixed; width: 800px;">
                    <tr>
                        <td style="width: 120px; font-size: 16px; font-family: 'Segoe UI'; font-weight: 600;">
                            <div style="margin-top: 5px;" class="commontd">
                                <%=GetGlobalResourceObject("CommanResource","CustomerID") %>&nbsp;
                            </div>
                        </td>
                        <td style="width: 300px;">
                            <asp:TextBox ID="txtreason" runat="server" Style="float: left" AutoPostBack="True" CssClass=" form-control" OnTextChanged="txtreason_TextChanged" placeholder="search customer information here ......" meta:resourcekey="txtreasonResource1"></asp:TextBox>
                        </td>
                        <td style="width: 240px;">
                            <div style="float: left">
                                <input type="button" value="<%=GetGlobalResourceObject("CommanResource","New") %>" class="btn btn-info" id="btnnew" />
                                <input type="button" value="<%=GetGlobalResourceObject("CommanResource","Cancel") %>" class="btn btn-info" id="btncance" style="display: none" />
                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,Save %>" class="btn btn-info" ID="btnsave" OnClick="btnsave_Click" meta:resourcekey="btnsaveResource1"></asp:Button>
                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,Delete %>" Visible="False" class="btn btn-info" ID="btndelete" meta:resourcekey="btndeleteResource1"></asp:Button>
                            </div>
                        </td>
                        <td>
                            <asp:Button runat="server" Text="Export" Style="background-color: #1c701c;" class="btn btn-info" ID="btnExport" OnClick="btnExport_Click"></asp:Button>
                        </td>
                    </tr>
                </table>

                <div id="divcustomer" style="overflow-x: auto;">
                    <div class="row text-center" style="margin-top: 60px;">
                        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                    </div>
                    <asp:HiddenField ID="hdfgrdcustomer" runat="server" />
                    <div style="border: 1px solid silver;">
                        <asp:GridView ID="grdcustomer" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" EmptyDataText="No customer information available." ShowHeaderWhenEmpty="True" Width="100%" ShowFooter="True" BackColor="White" ShowFooterWhenEmpty="true" meta:resourcekey="grdcustomerResource1" CssClass="headerFixer">
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,CustomerID %>" meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfupdate" runat="server" />
                                        <asp:Label ID="grdtxtcusid" runat="server" ReadOnly="true" Text='<%# Eval("customerid") %>' meta:resourcekey="grdtxtcusidResource1"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfcusid" runat="server" CssClass="form-control footerheight removefootercss" meta:resourcekey="hdfcusidResource1"></asp:TextBox>
                                    </FooterTemplate>
                                    <HeaderStyle Wrap="False" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,CustomerName %>" meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <i>
                                            <asp:TextBox ID="grdtxtcustomername" runat="server" CssClass="textboxcss" Text='<%# Eval("customername") %>' meta:resourcekey="grdtxtcustomernameResource1"></asp:TextBox></i>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfcustomername" runat="server" CssClass="form-control footerheight removefootercss " meta:resourcekey="hdfcustomernameResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Address %>" meta:resourcekey="TemplateFieldResource3">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtaddress" runat="server" CssClass="textboxcss" Text='<%# Eval("address1") %>' meta:resourcekey="grdtxtaddressResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfaddress" runat="server" CssClass="form-control footerheight removefootercss" meta:resourcekey="hdfaddressResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Place %>" meta:resourcekey="TemplateFieldResource4">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtplace" runat="server" CssClass="textboxcss" Text='<%# Eval("place") %>' meta:resourcekey="grdtxtplaceResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfplace" CssClass="form-control footerheight removefootercss" runat="server" meta:resourcekey="hdfplaceResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,State %>" meta:resourcekey="TemplateFieldResource5">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtstate" runat="server" CssClass="textboxcss" Text='<%# Eval("state") %>' meta:resourcekey="grdtxtstateResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfstate" CssClass="form-control footerheight removefootercss" runat="server" meta:resourcekey="hdfstateResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Country %>" meta:resourcekey="TemplateFieldResource6">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtcountry" runat="server" CssClass="textboxcss" Text='<%# Eval("country") %>' meta:resourcekey="grdtxtcountryResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfcountry" CssClass="form-control footerheight removefootercss" runat="server" meta:resourcekey="hdfcountryResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Pin %>" meta:resourcekey="TemplateFieldResource7">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtpin" runat="server" CssClass="textboxcss" Text='<%# Eval("pin") %>' Width="100px" TextMode="Number" meta:resourcekey="grdtxtpinResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfpin" CssClass="form-control footerheight removefootercss" runat="server" TextMode="Number" meta:resourcekey="hdfpinResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Phone %>" meta:resourcekey="TemplateFieldResource8">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtphone" runat="server" CssClass="textboxcss" Text='<%# Eval("phone") %>' Width="125px" TextMode="Phone" meta:resourcekey="grdtxtphoneResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfphone" CssClass="form-control footerheight removefootercss" runat="server" TextMode="Phone" meta:resourcekey="hdfphoneResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Email %>" meta:resourcekey="TemplateFieldResource9">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtemail" runat="server" CssClass="textboxcss" Text='<%# Eval("email") %>' TextMode="Email" meta:resourcekey="grdtxtemailResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfemail" CssClass="form-control footerheight removefootercss" runat="server" TextMode="Email" meta:resourcekey="hdfemailResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ Resources:CommanResource,ContactPerson %>" meta:resourcekey="TemplateFieldResource10">
                                    <ItemTemplate>
                                        <asp:TextBox ID="grdtxtcontact" runat="server" CssClass="textboxcss" Text='<%# Eval("contactperson") %>' meta:resourcekey="grdtxtcontactResource1"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="hdfcontact" CssClass="form-control footerheight removefootercss" runat="server" meta:resourcekey="hdfcontactResource1"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delete?" meta:resourcekey="TemplateFieldResource11">
                                    <ItemTemplate>
                                        <div class="iconcss">
                                            <asp:LinkButton ID="btndeltecust" runat="server" Width="30px" CssClass="iconcss" title="Delete" OnClick="btndeltecust_Click" OnClientClick="return confirm('Are you sure you want to delete this Customer ?')" meta:resourcekey="btndeltecustResource1"><i class="glyphicon glyphicon-trash"></i></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="hide" runat="server" ReadOnly="true" CssClass="form-control footerheight removefootercss" Style="margin-left: 5px; visibility: hidden" meta:resourcekey="hideResource1"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="blue" />
                            <RowStyle CssClass="rowheight" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            $('[id$=txtreason]').keyup(function () {
                searchTable($(this).val());
            });
            freezeColumnFromLeft('grdcustomer', 2);
            function searchTable(inputVal) {
                var table = $('[id$=grdcustomer]');
                table.find('tr').each(function (index, row) {
                    var allCells = $(row).find('td');
                    if (allCells.length > 0) {
                        var found = false;
                        allCells.each(function (index, td) {
                            var regExp = new RegExp(inputVal, 'i');
                            if (regExp.test($(td).html())) {
                                found = true;
                                return false;
                            }
                        });
                        if (found == true) $(row).show(); else $(row).hide();
                    }
                });
            }
        });

        $("[id$=btnsave]").click(function () {
            if ($("[id$=hdfgrdcustomer]").val() == "Save") {

                var hdfVal = $("[id$=hdfupdate]").val();
                if (hdfVal == "updated") {
                    if ($("[id$=grdtxtcusid]").val() == "") {
                        alert("<%=GetLocalResourceObject("PleaseentertheCustomerID")%>");
                        $("[id$=grdtxtcusid]").focus();
                        return false;
                    }
                    var inter = parseInt($("[id$=hdfcusid]").val());
                    if (inter == 0) {
                        alert("Customer ID cannot be ZERO");
                        $("[id$=hdfcusid]").focus();
                        return false;
                    }
                    if ($("[id$=grdtxtcustomername]").val() == "") {
                        alert("<%=GetLocalResourceObject("PleaseentertheCustomerName")%>");
                        $("[id$=grdtxtcustomername]").focus();
                        return false;
                    }
                }
                else {
                    if ($("[id$=hdfcusid]").val() == "") {
                        alert("<%=GetLocalResourceObject("PleaseentertheCustomerID")%>");
                        $("[id$=hdfcusid]").focus();
                        return false;
                    }
                    var inter = parseInt($("[id$=hdfcusid]").val());
                    if (inter == 0) {
                        alert("Customer ID cannot be ZERO");
                        $("[id$=hdfcusid]").focus();
                        return false;
                    }
                    if ($("[id$=hdfcustomername]").val() == "") {
                        alert("<%=GetLocalResourceObject("PleaseentertheCustomerName")%>");
                        $("[id$=hdfcustomername]").focus();
                        return false;
                    }
                }
                $(".footerheight").addClass("removefootercss");
            }
            $(".footerheight").addClass("removefootercss");
        });

        $("[id$=grdcustomer]").on("click", "td", function () {

            $(this).closest('tr').find('input[type=hidden]').val("updated");
            $("[id$=grdcustomer] tr td").find('input').removeClass("form-control");
            $("[id$=grdcustomer] tr td").find('input').addClass("textboxcss");
            $(this).closest('td').find('input').removeClass("textboxcss");
            //$(this).closest('td').find('input').addClass("form-control");


            if ($("[id$=grdcustomer] tr td").find('span').visibility == "visible") {

                $("#btnsave").css("display", "none");
            }

            if ($("[id$=grdcustomer] tr td").find('span').is(":visible")) {

                $("#btnsave").css("display", "none");
            }
            if ($(this).closest('td').find('input').is(":visible") == true) {
                $("#btnsave").css("display", "none");
            }
        });

        $("#btnnew").click(function () {

            newrow();
        });

        function newrow() {
            $(".footerheight").removeClass("removefootercss");
            $("#MainContent_grdcustomer_hdfcusid").focus();
            $("#btnnew").css('display', 'none');
            $("#btncance").css('display', "");
            $("[id$=hdfgrdcustomer]").val("Save")
            return false;
        };

        $("#btncance").click(function () {
            cancelrow();
        });

        function cancelrow() {
            $(".footerheight").addClass("removefootercss");
            //$(".footerheight").focus = true;
            $("#btnnew").css('display', "");
            $("#btncance").css('display', 'none');
            $("[id$=hdfsavecheck]").val("");
            return false;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {

            freezeColumnFromLeft('grdcustomer', 2);
            //...search customerid.............
            $(document).ready(function () {
                $('[id$=txtreason]').keyup(function () {
                    searchTable($(this).val());
                });

                function searchTable(inputVal) {
                    var table = $('[id$=grdcustomer]');
                    table.find('tr').each(function (index, row) {
                        var allCells = $(row).find('td');
                        if (allCells.length > 0) {
                            var found = false;
                            allCells.each(function (index, td) {
                                var regExp = new RegExp(inputVal, 'i');
                                if (regExp.test($(td).html())) {
                                    found = true;
                                    return false;
                                }
                            });
                            if (found == true) $(row).show(); else $(row).hide();
                        }
                    });
                }
            });
            //...end search.................
            $("[id$=btnsave]").click(function () {
                if ($("[id$=hdfgrdcustomer]").val() == "Save") {
                    debugger;
                    var hdfVal = $("[id$=hdfupdate]").val();
                    if (hdfVal == "updated") {
                        debugger;
                        if ($("[id$=grdtxtcusid]").val() == "") {
                            alert("<%=GetLocalResourceObject("PleaseentertheCustomerID")%>");
                            $("[id$=grdtxtcusid]").focus();
                            return false;
                        }
                        var inter = parseInt($("[id$=hdfcusid]").val());
                        if (inter == 0) {
                            alert("Customer ID cannot be ZERO");
                            $("[id$=hdfcusid]").focus();
                            return false;
                        }
                        if ($("[id$=grdtxtcustomername]").val() == "") {
                            alert("<%=GetLocalResourceObject("PleaseentertheCustomerName")%>");
                            $("[id$=grdtxtcustomername]").focus();
                            return false;
                        }
                    }
                    else {
                        debugger;
                        if ($("[id$=hdfcusid]").val() == "") {
                            alert("<%=GetLocalResourceObject("PleaseentertheCustomerID")%>");
                            $("[id$=hdfcusid]").focus();
                            return false;
                        }
                        var inter = parseInt($("[id$=hdfcusid]").val());
                        if (inter == 0) {
                            alert("Customer ID cannot be ZERO");
                            $("[id$=hdfcusid]").focus();
                            return false;
                        }
                        if ($("[id$=hdfcustomername]").val() == "") {
                            alert("<%=GetLocalResourceObject("PleaseentertheCustomerName")%>");
                            $("[id$=hdfcustomername]").focus();
                            return false;
                        }
                    }
                    $(".footerheight").addClass("removefootercss");
                }
                $(".footerheight").addClass("removefootercss");
            });

            $("[id$=grdcustomer]").on("click", "td", function () {

                $(this).closest('tr').find('input[type=hidden]').val("updated");
                $("[id$=grdcustomer] tr td").find('input').removeClass("form-control");
                $("[id$=grdcustomer] tr td").find('input').addClass("textboxcss");
                $(this).closest('td').find('input').removeClass("textboxcss");
                //$(this).closest('td').find('input').addClass("form-control");

                if ($("[id$=grdcustomer] tr td").find('span').visibility == "visible") {
                    $("#btnsave").css("display", "none");
                }

                if ($("[id$=grdcustomer] tr td").find('span').is(":visible")) {
                    $("#btnsave").css("display", "none");
                }
                if ($(this).closest('td').find('input').is(":visible") == true) {
                    $("#btnsave").css("display", "none");
                }
            });

            $("#btnnew").click(function () {

                newrow();
            });

            function newrow() {
                $(".footerheight").removeClass("removefootercss");
                $("#MainContent_grdcustomer_hdfcusid").focus();
                $("#btnnew").css('display', 'none');
                $("#btncance").css('display', "");
                $("[id$=hdfgrdcustomer]").val("Save")
                return false;
            };

            $("#btncance").click(function () {
                cancelrow();
            });

            function cancelrow() {
                $(".footerheight").addClass("removefootercss");
                //$(".footerheight").focus = true;
                $("#btnnew").css('display', "");
                $("#btncance").css('display', 'none');
                $("[id$=hdfsavecheck]").val("");
                return false;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
