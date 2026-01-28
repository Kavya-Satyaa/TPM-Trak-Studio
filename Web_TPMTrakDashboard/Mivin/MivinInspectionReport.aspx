<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MivinInspectionReport.aspx.cs" Inherits="Web_TPMTrakDashboard.Mivin.MivinInspectionReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div>

        </div>
        <div>
            <asp:ListView runat="server" ID="lstViewMainWindow">
                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="MainWindowPlaceHolder" />
                </LayoutTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <span>Machine ID: </span><asp:Label runat="server" ID="lblMachineID" />
                            </td>
                            <td>
                                <span>Component ID: </span><asp:Label runat="server" ID="Label1" />
                            </td>
                            <td>
                                <span>Operation: </span><asp:Label runat="server" ID="Label2" />
                            </td>
                        </tr>
                    </table>
                    <asp:ListView runat="server" ID="lstViewGrid">
                        <LayoutTemplate>
                            <table>
                                <tr>
                                    <th>
                                        Date / Shift
                                    </th>
                                    <th>
                                        Sl.No
                                    </th>
                                    <th>

                                    </th>
                                </tr>
                            </table>
                            <asp:PlaceHolder runat="server" ID="GridPlaceHolder">
                            </asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            
                        </ItemTemplate>
                    </asp:ListView>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
