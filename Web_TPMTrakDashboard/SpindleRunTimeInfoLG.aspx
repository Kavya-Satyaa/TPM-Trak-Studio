<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="SpindleRunTimeInfoLG.aspx.cs" Inherits="Web_TPMTrakDashboard.SpindleRunTimeInfoLG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        .commanTd {
            color:white;
        }
         #gvRuntimeData tbody tr:nth-child(odd) {
                background-color: #DCDCDC;
            }

            #gvRuntimeData tbody tr:nth-child(even) {
                background-color: #FFFFFF;
            }
             #gvRuntimeData tr td span{
                 color:black;
             }
        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
        }
    </style>
    <div>
        <div style="margin-bottom: 10px">
            <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                <tr>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">From Date</td>
                    <td class="input-group" style="width: 160px; height: 60px">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" Style="width: 160px; height: 42px" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">To Date</td>
                    <td class="input-group" style="width: 160px; height: 60px">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" runat="server" Style="width: 160px; height: 42px" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" CssClass="btn btn-info btn-sm" Text="View" OnClick="btnView_Click" />
                    </td>
                    <td>
                         <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" CssClass="btn btn-info btn-sm" Text="Export" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>

        </div>
          <div style="margin-top: 20px; height: 80vh; overflow: auto; width: 75%">
            <asp:GridView ID="gvRuntimeData" runat="server" CssClass="table table-bordered " AutoGenerateColumns="false" ClientIDMode="Static">
                <Columns>
                     <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Shift">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Shift") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Machine ID">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Machine") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RunTime">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("RunTime") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="HeaderCss" />
            </asp:GridView>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });
        function setControls() {
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en'
            });
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                setControls();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
