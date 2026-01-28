<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Web_TPMTrakDashboard.Contact" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <asp:ListView runat="server" ID="ListView1" ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="ChangePage">
        <LayoutTemplate>
            <table border="1">
                <tr>
                    <th>AutoId</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Age</th>
                    <th>Active</th>
                </tr>
                <asp:PlaceHolder ID="itemPlaceHolder1" runat="server"></asp:PlaceHolder>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("AutoID") %>
                </td>
                <td>
                    <%# Eval("FirstName") %>
                </td>
                <td>
                    <%# Eval("LastName") %>
                </td>
                <td><%#Eval("Age") %></td>
                <td>
                    <%# Eval("Active") %>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>

    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5">
        <Fields>
            <asp:NumericPagerField ButtonCount="5" />
        </Fields>
    </asp:DataPager>


</asp:Content>
