<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AceTPMToSAPProdRejDetails.aspx.cs" Inherits="Web_TPMTrakDashboard.AceDesigners.AceTPMToSAPProdRejDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #tblFilter tr td{
            padding: 5px;
        }
           #gvData tr td {
            background-color: #FFFFFF;
            color: black;
            vertical-align: middle;
        }
    </style>
    <div class="container-fluid" style="">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table id="tblFilter" class="" style="width: auto; margin-left: 10px;">
                    <tr>
                        <td class="commanTd" style="vertical-align: middle;">Machine</td>
                        <td>
                            <asp:DropDownList ID="ddlMachine" runat="server" CssClass="form-control" />
                        </td>
                        <td  class="commanTd" style="vertical-align: middle;">From Date</td>
                        <td style="position: relative">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td  class="commanTd" style="vertical-align: middle;">To Date</td>
                        <td style="position: relative">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="form-control" placeholder="Search Work Order No." AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td class="commanTd" style="vertical-align: middle;">Message Type</td>
                        <td>
                            <asp:DropDownList ID="ddlMessageType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                <asp:ListItem Text="Error" Value="E" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Success" Value="S"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-primary"  OnClientClick="return ViewClick();" OnClick="btnView_Click" />
                        </td>
                        
                    </tr>
                </table>
                <div style="margin-top: 10px; height: 73vh; overflow: auto">
                    <asp:GridView runat="server" ID="gvData" CssClass="table table-bordered  headerFixer" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Data Found"  ClientIDMode="Static">

                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: 'en-US',
            });
        }
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControls();
                $.unblockUI({});
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
