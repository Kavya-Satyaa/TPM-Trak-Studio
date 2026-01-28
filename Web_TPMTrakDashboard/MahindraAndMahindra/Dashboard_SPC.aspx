<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard_SPC.aspx.cs" Inherits="Web_TPMTrakDashboard.MahindraAndMahindra.Dashboard_SPC" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id="formProcessParam" runat="server">
        <asp:ScriptManager runat="server" />
        <asp:Timer runat="server" ID="UpdateTimer" Interval="30000" OnTick="UpdateTimer_Tick" />
        <div class="row3" id="titleContainer">
            <img src="Images/Logo/amit.png" class="img-responsive imgsrc " />
            <%--            <div id="titleHeading" class="dd">Process Parameter Dashboard</div>--%>
            <asp:Label runat="server" ID="titleHeading" ForeColor="White" Font-Names="Segoe UI" Text="Process Parameter Dashboard"/>
        </div>
        <div class="container-fluid" id="mainContainer">
            <asp:UpdatePanel ID="updatePanelProcessParam" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div id="divProcessParams">
                            <asp:ListView runat="server" ItemPlaceholderID="placeHolderProcessParams" ID="listViewProcessParams">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="placeHolderProcessParams" />
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <div class="myItem" style="margin: 15px; min-width: 380px; max-width: 500px; display: inline-block;">
                                        <table style='width: 100%; border: none; border-collapse: collapse;' class="outerbox">
                                            <tr style="background-color: #304F7C;">
                                                <td style="text-align: center; font-weight: bold; padding-bottom: 5px; border: none; height: 26px;">
                                                    <asp:Label runat="server" ID="lblParameter" ForeColor="White" Font-Size="Medium" Font-Names="Segoe UI"><%# Eval("ParameterName")%></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="background-color: <%# Eval("BackgroundColor")%>; border: none;">
                                                <td style="text-align: center; font-weight: bold; padding-bottom: 5px; border: none;">
                                                    <asp:TextBox ID="txtParamval" runat="server" Enabled="false" Style="text-align: center; margin-top: 6px; width: 130px; height: 22px;" Text='<%# Bind("ParameterValue") %>' ForeColor="#2C639B" Font-Size="18px" Font-Names="Segoe UI" Font-Bold="true" />
                                                </td>
                                            </tr>
                                            <tr style="background-color: <%# Eval("BackgroundColor")%>; border: none;">
                                                <td style="text-align: center; font-weight: bold; padding-bottom: 5px; border: none;">
                                                    <asp:Label runat="server" ID="lblUom" ForeColor="Black" Font-Names="Segoe UI" Font-Bold="true"><%# Eval("Unit")%></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="background-color: <%# Eval("BackgroundColor")%>; border: none;">
                                                <td style="text-align: center; font-weight: bold; padding-bottom: 5px; border: none;">
                                                    <div class="row">
                                                        <asp:Label runat="server" ForeColor="Black" Font-Names="Segoe UI" Font-Bold="true">Low</asp:Label>
                                                        <asp:TextBox ID="txtLow" runat="server" Enabled="false" Style="text-align: center; width: 100px; height: 22px; margin-left: 10px;" Text='<%# Bind("MinValue") %>' ForeColor="#2C639B" Font-Size="18px" Font-Names="Segoe UI" Font-Bold="true" />
                                                        <asp:Label runat="server" ForeColor="Black" Font-Names="Segoe UI" Font-Bold="true" Style="margin-left: 10px;">Hi</asp:Label>
                                                        <asp:TextBox ID="txtHi" runat="server" Enabled="false" Style="text-align: center; width: 100px; height: 22px; margin-left: 10px;" Text='<%# Bind("MaxValue") %>' ForeColor="#2C639B" Font-Size="18px" Font-Names="Segoe UI" Font-Bold="true" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
