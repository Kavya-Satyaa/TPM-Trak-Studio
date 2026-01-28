<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="ToolWiseCycleTimes.aspx.cs" Inherits="Web_TPMTrakDashboard.ToolWiseCycleTimes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #tblToolCycleTime th, #tblToolCycleTime {
            color: white;
            background-color: #2E6886 !important;
        }

            /*#tblToolCycleTime tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }*/

            /*  #tblToolCycleTime tbody tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }*/
            #tblToolCycleTime tbody tr td {
                background-color: #FFFFFF;
                color: black;
            }

            #tblToolCycleTime tbody tr td {
                vertical-align: middle;
            }

            #tblToolCycleTime tbody tr:last-child td {
                color: white;
                background-color: #2E6886 !important;
            }

        #filtertbl tr td {
            color: white;
        }
    </style>
    <div>
        <div style="margin-bottom: 10px">
            <table class="table table-bordered" id="filtertbl" style="width: auto">
                <tr>
                    <td>Machine: 
                        <asp:Label runat="server" ID="lblMachine"></asp:Label></td>
                    <td>Component ID: 
                        <asp:Label runat="server" ID="lblCompID"></asp:Label></td>
                    <td>Serial Number: 
                        <asp:Label runat="server" ID="lblSlno"></asp:Label></td>
                    <td>Operation Number: 
                        <asp:Label runat="server" ID="lblOpnNum"></asp:Label></td>
                    <td>Cycel Start Time:
                        <asp:Label runat="server" ID="lblCycleStartTime"></asp:Label></td>
                    <td>Cycel End Time:
                        <asp:Label runat="server" ID="lblCycleEndTime"></asp:Label></td>
                    <td>
                        <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" CssClass="btn btn-info btn-sm" Text="Export" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>

        </div>
        <div style="height: 85vh; overflow: scroll">
            <asp:ListView runat="server" ID="lvToolCycleTime" class="table table-bordered table-hover" ClientIDMode="Static">
                <LayoutTemplate>
                    <table class="table table-bordered" id="tblToolCycleTime">
                        <tr>
                            <th>Cycle Start Time</th>
                            <th>Cycle End Time</th>
                            <th>Tool Number</th>
                            <th>Tool Time (hh:mm:ss)</th>
                            <th>Operating Time (hh:mm:ss)</th>
                            <th>Cutting Time (hh:mm:ss)</th>
                            <th>Non Cutting Time (hh:mm:ss)</th>
                            <th>Program Block</th>
                        </tr>
                        <tr id="itemplaceholder" runat="server"></tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <%-- <td style="display: <%# Eval("Visibility") %>" rowspan='<%# Eval("RowSpan") %>'><%# Eval("CycleStartTime") %></td>
                        <td style="display: <%# Eval("Visibility") %>" rowspan='<%# Eval("RowSpan") %>'><%# Eval("CycleEndTime") %></td>--%>
                        <td><%# Eval("CycleStartTime") %></td>
                        <td><%# Eval("CycleEndTime") %></td>
                        <td><%# Eval("ToolNumber") %></td>
                        <td><%# Eval("ToolTimeTS") %></td>
                        <td><%# Eval("OperatingTimeTS") %></td>
                        <td><%# Eval("CuttingTimeTS") %></td>
                        <td><%# Eval("NonCuttingTimeTS") %></td>
                        <td><%# Eval("ProgramBlock") %></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
