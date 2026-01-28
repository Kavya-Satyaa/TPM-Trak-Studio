<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="AirPressureData.aspx.cs" Inherits="Web_TPMTrakDashboard.AirPressureData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #gvAirPressureData {
            width: 100%
        }

            #gvAirPressureData tbody tr:nth-child(odd) {
                background-color: #DCDCDC;
            }

            #gvAirPressureData tbody tr:nth-child(even) {
                background-color: #FFFFFF;
            }
             #gvAirPressureData tr td span{
                 color:black;
             }
        #filtertbl tr td {
            color: white;
            vertical-align:middle;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
        }
    </style>
    <div>
        <div style="margin-bottom: 10px">
            <table class="table table-bordered" id="filtertbl" style="width: auto">
                <tr>
                    <td>Machine: 
                        <asp:Label runat="server" ID="lblMachine"></asp:Label></td>
                    <td>Start Datetime:
                        <asp:Label runat="server" ID="lblStartTime"></asp:Label></td>
                    <td>End Datetime:
                        <asp:Label runat="server" ID="lblEndTime"></asp:Label></td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" CssClass="btn btn-info btn-sm" Text="Export" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>

        </div>
        <div style="margin-top: 20px; height: 80vh; overflow: auto; width: 75%">
            <asp:GridView ID="gvAirPressureData" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" ClientIDMode="Static">
                <Columns>
                    <asp:TemplateField HeaderText="SI.No">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("SlNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Machine ID">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("MachineID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Air Pressure Low">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("AirPressureLow") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Air Pressure Retained">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("AirPressureRetained") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lapsed Time">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("LapsedTime") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="commanitemstyle" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="HeaderCss" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
