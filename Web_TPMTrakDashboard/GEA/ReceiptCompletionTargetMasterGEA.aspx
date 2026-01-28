<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiptCompletionTargetMasterGEA.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.ReceiptCompletionTargetMasterGEA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <table class="table table-bordered " id="tblheader" style="margin-top: 0px; color: white; width: 45%; width: auto; margin-left: 15px;">
        <tbody>
            <tr>
                <td class="commontd" style="vertical-align: middle">Year
                </td>
                <td>
                    <div class="input-group">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control date2" data-toggle="tooltip"
                            title="Year here !" placeholder="Year here" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <asp:Button runat="server" Text="View" Style="float: left" class="btn btn-info" ID="btnView" OnClick="btnView_Click"></asp:Button>&nbsp;&nbsp;
                             <asp:Button runat="server" Text="Save" class="btn btn-info" ID="btnSave" OnClick="btnSave_Click"></asp:Button>&nbsp;
                </td>
            </tr>
        </tbody>
    </table>
    <div style="height: 80vh; overflow: auto; min-width: 50%; width: fit-content">
        <asp:GridView ID="gvRCTarget" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true" CssClass="table table-bordered  headerFixer alternate-table-style" ClientIDMode="Static">
            <Columns>
                <asp:TemplateField HeaderText="Week No.">
                    <ItemTemplate>

                        <asp:Label runat="server" ID="lblWeehNo" ClientIDMode="Static" Text='<%# Eval("WeekNo") %>'></asp:Label>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Target">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtTarget" CssClass="textboxcss allowDecimal form-control" Text='<%# Eval("Target") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
        });

        function setControls() {
            $('[id$=txtYear]').datetimepicker({
                format: 'YYYY',
                useCurrent: true

            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
