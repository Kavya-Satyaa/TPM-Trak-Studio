<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierCodeTafe.aspx.cs" Inherits="Web_TPMTrakDashboard.SupplierCodeTafe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        td {
            color: white;
            text-align: center;
            font-size: medium;
        }

        th {
            text-align: center;
        }

        .button {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            width: 80px;
            height: 30px;
        }

        .drp {
            text-align: center;
            display: inline-block;
            background: white;
            width: 120px;
            height: 30px;
            font-size: large;
        }
       
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>


                <div style="" id="SupplierCode">
                    <div style="height: 90%; width: 70%; align-items: center; overflow: auto;">
                        <asp:GridView runat="server" ID="GridViewSupplierCode" ClientIDMode="Static" ShowHeaderWhenEmpty="true" ShowHeader="true" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer" >
                            <Columns>
                                <asp:BoundField HeaderText="SL No." DataField="SLNo" />
                                <asp:TemplateField HeaderText="Supplier Code">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSupCode" runat="server" Text='<%# Bind("SupCode") %>' Style="text-align: center" AutoCompleteType="Disabled" ViewStateMode="Disabled" autocomplete="new-password"/>
                                        <asp:Label Visible="false" runat="server" Text='<%# Bind("SupCode") %>' ID="SuppliergridHiddenfield" />
                                        <asp:Label Visible="false" runat="server" Text='<%# Bind("idd") %>' ID="idd" />
                                        <asp:HiddenField runat="server" Value="false" ID="deletesupcode" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="text-center" ForeColor="White" />
                                </asp:TemplateField>


                            </Columns>
                            <HeaderStyle BackColor="#004080" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        </asp:GridView>
                    </div>
                    <div style="width: 70%; text-align: end;">
                        <asp:Button Text="Add" runat="server" ID="btnSupplierAdd" OnClick="btnSupplierAdd_Click" CssClass="button"/>
                        <asp:Button Text="Save" runat="server" ID="btnSupplierUpdate" OnClick="btnSupplierUpdate_Click" CssClass="button" />
                        <asp:Button Text="Delete" runat="server" ID="btnSupplierDelete" OnClick="btnSupplierDelete_Click" CssClass="button" />
                    </div>
                </div>
                <div style="height: 50%;padding-top:20px;">
                    <%--<div style="height: 10%; margin: 10px; align-items: center; ">
                        <span style="display: inline-block; color: white; height: 30px; font-size: large;">Supplier Code</span>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlSupplierCode" Style="text-align: center; display: inline-block" CssClass="drp" />
                        <span style="display: inline-block; color: white; height: 30px; font-size: large;">Component-ID</span>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlComponentID" Style="text-align: center; display: inline-block" CssClass="drp" />
                        <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="button" />
                    </div>--%>
                    <div id="ComponentIDSupplierCodea">
                        <div style="overflow:auto;height:90%;" id="ComponentIDSupplierCode">
                            <asp:GridView runat="server" ID="GridViewComponentSupplier" ClientIDMode="Static" ShowHeaderWhenEmpty="true" ShowHeader="true" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer" OnRowDataBound="GridViewComponentSupplier_RowDataBound">
                                <%--<Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdfUpdated" Value="Not Updated" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="SL No." DataField="SLNO" />
                                    <asp:TemplateField HeaderText="Supplier Code">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlSupplierCode" runat="server" DataSource='<%# Bind("lstSupplierCode") %>' SelectedValue='<%# Bind ("SupplierCode") %>' OnLoad="txtSupplierCode_Load" Style="text-align: center" CssClass="drp" />
                                            <asp:Label runat="server" Visible="false" Text='<%# Bind("lblSupplierCode") %>' ID="SupplierCodeHiddenfield" />
                                            <asp:Label Visible="false" runat="server" Text='<%# Bind("idd") %>' ID="idd" />
                                            <asp:HiddenField runat="server" Value="false" ID="deletesupcode" />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Component ID">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="drp" runat="server" ID="ddlComponentID" DataSource='<%# Bind("lstComponentID") %>' SelectedValue='<%# Bind ("ComponentID") %>' OnLoad="drpComponentID_Load" Style="text-align: center" />
                                            <asp:Label runat="server" Visible="false" Text='<%# Bind("ComponentID") %>' ID="ComponentIDHiddenfield" />
                                            <asp:HiddenField runat="server" Value="false" ID="deletesupcodecom" />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                </Columns>--%>
                                <HeaderStyle BackColor="#004080" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                
                            </asp:GridView>
                        </div>
                        <div style="height: 10%; width: 100%; text-align: end">
                            <%--<asp:Button Text="Add" runat="server" ID="btnAdd" OnClick="btnAdd_Click" CssClass="button"/>--%>
                            <asp:Button Text="Save" runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" CssClass="button" />
                            <%--<asp:Button Text="Delete" runat="server" ID="btnDelete" OnClick="btnDelete_Click" CssClass="button" />--%>
                        </div>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        function resize() {

            var gridWidth = screen.availWidth-150;
            document.getElementById("GridViewComponentSupplier").style.width = gridWidth + "px";
        }
        $(document).ready(function () {
            resize();
            window.onresize = function () {
                resize();
            };
            var winHeight = $(window).height();
            if (winHeight < 650) {
                winHeight = (winHeight - 150);
            } else {
                winHeight = (winHeight - 200);

            }
            console.log(winHeight);
            
            $("#SupplierCode").height(winHeight / 2);
            $("#ComponentIDSupplierCode").height((winHeight / 2));
        });

        $("#GridViewSupplierCode").on("click", "td", function () {
            
            $("#GridViewSupplierCode tr td").each(function () {
                
                $("tr").css("background-color", "#202648");
                $("td").find('input[type=hidden]').val("false");
            });
            $(this).closest('tr').find('input[type=hidden]').val("true");
            $(this).closest('tr').css("background-color", "black");
        });
        

        //$("#GridViewComponentSupplier").on("click", "td", function () {
        //    
        //    $("#GridViewComponentSupplier tr td").each(function () {
        //        $("td").find('input[type=hidden]').val("false");
        //        $("tr").css("background-color", "#202648");
        //    });
        //    $(this).closest('tr').find('input[type=hidden]').val("true");
        //    $(this).closest('tr').css("background-color", "black");
        //});
       
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ajaxStop($.unblockUI);
            var winHeight = $(window).height();
            if (winHeight < 650) {
                winHeight = (winHeight - 150);
            }
            else {
                winHeight = (winHeight - 200);
            }
            function resize() {

                var gridWidth = screen.availWidth-150;
                document.getElementById("GridViewComponentSupplier").style.width = gridWidth + "px";
            }
            resize();
            window.onresize = function () {
                resize();
            };
            $("#SupplierCode").height(winHeight / 2);
            $("#ComponentIDSupplierCode").height(winHeight / 2);

            $("#GridViewSupplierCode").on("click", "td", function () {
                
                $("#GridViewSupplierCode tr td").each(function () {
                    
                    $("tr").css("background-color", "#202648");
                    $("td").find('input[type=hidden]').val("false");
                });
                $(this).closest('tr').find('input[type=hidden]').val("true");
                $(this).closest('tr').css("background-color","black");
            });
            //$("#GridViewComponentSupplier").on("click", "td", function () {
            //    
            //    $("#GridViewComponentSupplier tr td").each(function () {
            //        $("tr").css("background-color", "#202648");
            //        $("td").find('input[type=hidden]').val("false");
            //    });
            //    $(this).closest('tr').find('input[type=hidden]').val("true");
            //    $(this).closest('tr').css("background-color", "black");
            //});
        })

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
