<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Web_TPMTrakDashboard.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #MainContent_gridView1 td, table td {
            color: white;
        }
    </style>
    <asp:Label ID="Label2" runat="server" Style="color: white" EnableViewState="false"></asp:Label>
    <br />
    <table>
        <tr>
            <td>Resource Name</td>
            <td>
                <asp:DropDownList ID="LanguageDropdownlist" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="LanguageDropdownlist_SelectedIndexChanged">
                </asp:DropDownList>
            </td>   
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>        
        </tr>
       
    </table>

    <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="False" CellPadding="6"
        Width="100%">
        <Columns>                        
            <asp:TemplateField HeaderText="Key">
                <ItemTemplate>
                <asp:Label ID="lblKey" runat="server" Text='<%# Bind("Key") %>'></asp:Label>
                </ItemTemplate>              
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Value">
                <ItemTemplate>
                  <asp:TextBox ID="txtValue" runat="server" Text='<%#Eval("Value") %>' Width="100%"></asp:TextBox>
                </ItemTemplate>              
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
