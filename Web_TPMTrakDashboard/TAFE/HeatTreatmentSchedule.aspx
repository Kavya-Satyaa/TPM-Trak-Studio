<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HeatTreatmentSchedule.aspx.cs" Inherits="Web_TPMTrakDashboard.TAFE.HeatTreatmentSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <style>
        #grdRatioVersionMaster tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #grdRatioVersionMaster tr:nth-child(even) {
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
            }

        .textboxcss {
            border: none;
            background-color: transparent;
            font-style: italic;
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
    </style>
     <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <div class="row" style="margin:0px;">
               
                <div class="col-lg-12">
                  
                  <div id="divRatioVersionMaster" class="col-lg-6">

                     <div>
                        <h2 style="color:white;margin-top:0px;">
                            <span>Part Master</span>
                        </h2>
                    </div>
                   <table class="table table-bordered" id="tblmenu" style="position: fixed; width: 41%;margin-top: 12px;">
                    <tr>
                        <td style="width: 120px; font-size: 16px; font-family: 'Segoe UI'; font-weight: 600;">
                            <div style="margin-top: 5px;" class="commontd">
                               <span>Ratio</span>&nbsp;</div>
                        </td>
                        <td style="width: 300px;">
                            <asp:TextBox ID="txtRatio" runat="server" Style="float: left" AutoPostBack="True" CssClass=" form-control" OnTextChanged="txtRatio_TextChanged" placeholder="search Ratio information here ......" meta:resourcekey="txtreasonResource1"></asp:TextBox>
                        </td>
                        <td style="width: 240px;">
                            <div style="float: left">
                                <input type="button" value="<%=GetGlobalResourceObject("CommanResource","New") %>" class="btn btn-info" id="btnnew" />
                                <input type="button" value="<%=GetGlobalResourceObject("CommanResource","Cancel") %>" class="btn btn-info" id="btncance" style="display: none" />
                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,Save %>" class="btn btn-info" ID="btnsave" OnClick="btnsave_Click" meta:resourcekey="btnsaveResource1"></asp:Button>
                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,Delete %>" Visible="False" class="btn btn-info" ID="btndelete" meta:resourcekey="btndeleteResource1"></asp:Button>
                            </div>
                        </td>
                    </tr>
                   </table>
                    <div class="row text-center" style="margin-top: 100px;">
                        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                    </div>
                    <asp:HiddenField ID="hdfgrdRatioVersionMaster" runat="server" />
                    <div style="overflow: auto; height: 72vh;" id="displayContainer">
                        <asp:GridView ID="grdRatioVersionMaster" runat="server" AutoGenerateColumns="False" EmptyDataText="No Ratio Version information available." ShowHeaderWhenEmpty="True" Width="100%" ShowFooter="True" BackColor="White" ShowFooterWhenEmpty="true" CssClass="table table-bordered table-hover headerFixer">
                        <AlternatingRowStyle BackColor="#F2F2F2" />
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Ratio %>" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfupdate" runat="server" />
                                    <asp:Label ID="grdtxtRatio" runat="server" Text='<%# Eval("Ratio") %>' ></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="hdfRatio" runat="server" CssClass="form-control footerheight removefootercss"></asp:TextBox>
                                </FooterTemplate>
                                <HeaderStyle Wrap="False" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Version %>" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                     <asp:Label ID="grdtxtVersion" runat="server" Text='<%# Eval("Version") %>' ></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="hdfVersion" runat="server" CssClass="form-control footerheight removefootercss"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField meta:resourcekey="TemplateFieldResource11" HeaderText="Action" HeaderStyle-Width="38">
                                <ItemTemplate>
                                    <div class="iconcss">
                                        <asp:LinkButton ID="btndelteRatio" runat="server" Width="30px" CssClass="iconcss" title="Delete" OnClick="btndelteRatio_Click" OnClientClick="return confirm('Are you sure you want to delete this Ratio ?')" ><i class="glyphicon glyphicon-trash"></i></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="hide" runat="server" ReadOnly="true" CssClass="form-control footerheight removefootercss" Style="margin-left: 5px; visibility: hidden" meta:resourcekey="hideResource1"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="blue" Font-Italic="false" />
                        <RowStyle CssClass="rowheight" />
                    </asp:GridView>
                    </div>
                  </div>
                  <div class="col-lg-6">
                     <div>
                        <h2 style="color:white;margin-top:0px;">
                            <span>Part Association</span>
                        </h2>
                    </div>
                    <div>
                     <table class="table table-bordered" id="tblPartAssociation" style="margin-top:20px;">
                        <tr>
                            <td style="width: 120px; font-size: 16px; font-family: 'Segoe UI'; font-weight: 600;">
                                <div style="margin-top: 5px;" class="commontd">
                                   <span>Ratio</span>&nbsp;
                                </div>
                            </td>
                            <td style="width: 300px;">
                                 <asp:DropDownList ID="ddlRatio" runat="server" CssClass="select form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlRatio_SelectedIndexChanged" >
                                  </asp:DropDownList>
                            </td>
                            <td style="width: 120px; font-size: 16px; font-family: 'Segoe UI'; font-weight: 600;">
                                <div style="margin-top: 5px;" class="commontd">
                                   <span>Version</span>&nbsp;
                                </div>
                            </td>
                            <td style="width: 300px;">
                                 <asp:DropDownList ID="ddlVersion" runat="server" CssClass="select form-control cssclass" >
                                  </asp:DropDownList>
                            </td>
                         </tr>
                         <tr>
                            <td style="width: 120px; font-size: 16px; font-family: 'Segoe UI'; font-weight: 600;">
                                <div style="margin-top: 5px;" class="commontd">
                                   <span>Part No</span>&nbsp;
                                </div>
                            </td>
                            <td style="width: 300px;">
                                 <asp:DropDownList ID="ddlPartNo" runat="server" CssClass="select form-control cssclass" AutoPostBack="true" OnSelectedIndexChanged="ddlPartNo_SelectedIndexChanged" >
                                  </asp:DropDownList>
                            </td>
                            <td style="width: 120px; font-size: 16px; font-family: 'Segoe UI'; font-weight: 600;">
                                <div style="margin-top: 5px;" class="commontd">
                                   <span>Part Type</span>&nbsp;
                                </div>
                            </td>
                            <td style="width: 300px;">
                                <asp:DropDownList ID="ddlPartType" runat="server" CssClass="select form-control cssclass">
                                  </asp:DropDownList>
                            </td>
                            <td>
                                 <asp:Button runat="server" Text="<%$ Resources:CommanResource,Save %>" class="btn btn-info" ID="btnPartAssociationSave" OnClick="btnPartAssociationSave_Click" meta:resourcekey="btnsaveResource1"></asp:Button>
                            </td>
                        </tr>
                     </table>
                     <div class="row text-center">
                        <asp:Label ID="lblViewMsg" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                    </div>
                </div>

                    <div style="overflow: auto; height: 68vh;">
                         <asp:GridView ID="gvPartAssociation" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data available." ShowHeaderWhenEmpty="True" Width="100%" BackColor="White"  CssClass="table table-bordered table-hover headerFixer">
                        <AlternatingRowStyle BackColor="#F2F2F2" />
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Ratio %>" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:Label ID="grdtxtRatio" runat="server" Text='<%# Eval("Ratio") %>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:CommanResource,Version %>" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                     <asp:Label ID="grdtxtVersion" runat="server" Text='<%# Eval("Version") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:CommanResource,PartNumber %>" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                     <asp:Label ID="grdtxtPartNo" runat="server" Text='<%# Eval("PartNumber") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ Resources:CommanResource,PartType %>" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                     <asp:Label ID="grdtxtPartType" runat="server" Text='<%# Eval("PartType") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField meta:resourcekey="TemplateFieldResource11" HeaderText="Action" HeaderStyle-Width="38">
                                <ItemTemplate>
                                    <div class="iconcss">
                                        <asp:LinkButton ID="btndelteRatioPart" runat="server" Width="30px" CssClass="iconcss" title="Delete" OnClick="btndelteRatioPart_Click" OnClientClick="return confirm('Are you sure you want to delete this Ratio ?')" ><i class="glyphicon glyphicon-trash"></i></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="blue" Font-Italic="false" />
                        <RowStyle CssClass="rowheight" />
                    </asp:GridView>
                    </div>
                 </div>
                </div>
              
            </div>
               
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        $(document).ready(function () {
            $('[id$=txtRatio]').keyup(function () {
                searchTable($(this).val());
            });

            $("#btnnew").click(function () {
                newrow();

            });

            $("[id$=grdRatioVersionMaster]").on("click", "td", function () {

                $(this).closest('tr').find('input[type=hidden]').val("updated");
                //$("[id$=grdRatioVersionMaster] tr td").find('input').removeClass("form-control");
                //$("[id$=grdRatioVersionMaster] tr td").find('input').addClass("textboxcss");
                $(this).closest('td').find('input').removeClass("textboxcss");

                if ($("[id$=grdRatioVersionMaster] tr td").find('span').visibility == "visible") {

                    $("#btnsave").css("display", "none");
                }

                if ($("[id$=grdRatioVersionMaster] tr td").find('span').is(":visible")) {

                    $("#btnsave").css("display", "none");
                }
                if ($(this).closest('td').find('input').is(":visible") == true) {
                    $("#btnsave").css("display", "none");
                }
            });

            $("[id$=btnsave]").click(function () {
                if ($("[id$=hdfgrdRatioVersionMaster]").val() == "Save") {

                    var hdfVal = $("[id$=hdfupdate]").val();
                    if (hdfVal == "updated") {
                        if ($("[id$=grdtxtRatio]").val() == "") {
                            alert("Please enter the Ratio");
                            $("[id$=grdtxtRatio]").focus();
                            return false;
                        }
                        if ($("[id$=grdtxtVersion]").val() == "") {
                            alert("Please enter the Version");
                            $("[id$=grdtxtVersion]").focus();
                            return false;
                        }
                    }
                    else {
                        if ($("[id$=hdfRatio]").val() == "") {
                            alert("Please enter the Ratio");
                            $("[id$=hdfRatio]").focus();
                            return false;
                        }
                        if ($("[id$=hdfVersion]").val() == "") {
                            alert("Please enter the Version");
                            $("[id$=hdfVersion]").focus();
                            return false;
                        }
                    }
                    $(".footerheight").addClass("removefootercss");
                }
                $(".footerheight").addClass("removefootercss");
            });

            $("#btncance").click(function () {
                cancelrow();
            });

        });

        function searchTable(inputVal) {
            var table = $('[id$=grdRatioVersionMaster]');
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

      
        function newrow() {       
            $(".footerheight").removeClass("removefootercss");
            $("#btnnew").css('display', 'none');
            $("#btncance").css('display', "");
            $("[id$=hdfgrdRatioVersionMaster]").val("Save")
            setScrollPosition();
            return false;
        };

       
        function cancelrow() {
            $(".footerheight").addClass("removefootercss");
            $("#btnnew").css('display', "");
            $("#btncance").css('display', 'none');
            $("[id$=hdfsavecheck]").val("");
            return false;
        }
        function setScrollPosition() {
            debugger;
          //  window.onload = function () {
                $("#displayContainer").animate({ scrollTop: $("#displayContainer")[0].scrollHeight }, 1000);
          //  }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {

            //...search customerid.............
            $(document).ready(function () {
                $('[id$=txtRatio]').keyup(function () {
                    searchTable($(this).val());
                });

                $("[id$=btnsave]").click(function () {
                    if ($("[id$=hdfgrdRatioVersionMaster]").val() == "Save") {

                        var hdfVal = $("[id$=hdfupdate]").val();
                        if (hdfVal == "updated") {
                            if ($("[id$=grdtxtRatio]").val() == "") {
                                alert("Please enter the Ratio");
                                $("[id$=grdtxtRatio]").focus();
                                return false;
                            }
                            if ($("[id$=grdtxtVersion]").val() == "") {
                                alert("Please enter the Version");
                                $("[id$=grdtxtVersion]").focus();
                                return false;
                            }
                        }
                        else {
                            if ($("[id$=hdfRatio]").val() == "") {
                                alert("Please enter the Ratio");
                                $("[id$=hdfRatio]").focus();
                                return false;
                            }
                            if ($("[id$=hdfVersion]").val() == "") {
                                alert("Please enter the Version");
                                $("[id$=hdfVersion]").focus();
                                return false;
                            }
                        }
                        $(".footerheight").addClass("removefootercss");
                    }
                    $(".footerheight").addClass("removefootercss");
                });

                $("[id$=grdRatioVersionMaster]").on("click", "td", function () {

                    $(this).closest('tr').find('input[type=hidden]').val("updated");
                    //$("[id$=grdRatioVersionMaster] tr td").find('input').removeClass("form-control");
                    //$("[id$=grdRatioVersionMaster] tr td").find('input').addClass("textboxcss");
                    $(this).closest('td').find('input').removeClass("textboxcss");

                    if ($("[id$=grdRatioVersionMaster] tr td").find('span').visibility == "visible") {

                        $("#btnsave").css("display", "none");
                    }

                    if ($("[id$=grdRatioVersionMaster] tr td").find('span').is(":visible")) {

                        $("#btnsave").css("display", "none");
                    }
                    if ($(this).closest('td').find('input').is(":visible") == true) {
                        $("#btnsave").css("display", "none");
                    }
                });


                $("#btnnew").click(function () {
                    newrow();
                });

                $("#btncance").click(function () {
                    cancelrow();
                });
            });
            //...end search.................

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
