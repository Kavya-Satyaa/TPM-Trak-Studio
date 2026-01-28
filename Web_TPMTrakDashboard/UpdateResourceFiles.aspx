<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateResourceFiles.aspx.cs" Inherits="Web_TPMTrakDashboard.UpdateResourceFiles" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #MainContent_gridViewResource tr th {
            color: white;
            background-color: #2E6886 !important;
        }

        .comanStyle {
            color: white;
        }
    </style>
    <div class="row" style="text-align: center; color: white;">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>
    <div class="row">
        <table class="table table-bordered" style="width: 70%">
            <tr>
                <td style="vertical-align: middle; color: white"><b><%=GetLocalResourceObject("Language") %></b></td>
                <td>
                    <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged" meta:resourcekey="ddlLanguageResource1">
                        <asp:ListItem Value="en" meta:resourcekey="ListItemResource1">English (United Kingdom)</asp:ListItem>
                        <asp:ListItem Value="zh" meta:resourcekey="ListItemResource2">中文（简体，PRC）</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="vertical-align: middle; color: white"><b><%=GetLocalResourceObject("PageName") %></b></td>
                <td>
                    <asp:DropDownList ID="ddlPageName" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageName_SelectedIndexChanged" meta:resourcekey="ddlPageNameResource1"></asp:DropDownList></td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="<%$Resources:CommanResource, Save %>" CssClass="btn btn-info btn-sm" OnClick="btnSave_Click"  /></td>
            </tr>
        </table>
    </div>
    <div class="row">
        <div style="overflow-x: hidden; overflow-y: auto; height: 700px; width: 70%;">
            <asp:GridView ID="gridViewResource" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%" meta:resourcekey="gridViewResourceResource1">
                <Columns>
                    <asp:TemplateField HeaderText="Key" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:Label ID="lblKey" runat="server" CssClass="comanStyle" Text='<%# Bind("Key") %>' meta:resourcekey="lblKeyResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Value" meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValue" runat="server" Text='<%# Eval("Value") %>' Width="100%" CssClass="form-control txtupdate" meta:resourcekey="txtValueResource1"></asp:TextBox>
                            <asp:HiddenField ID="hdfCondition" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <script type="text/javascript">
        $("#MainContent_gridViewResource").on("change", ".txtupdate", function () {
            $("[id$=hdfCondition]").val("update");
            $(this).closest('tr').find('input[type=hidden]').val("update");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
