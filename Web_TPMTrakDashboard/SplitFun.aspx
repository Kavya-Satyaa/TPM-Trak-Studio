<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SplitFun.aspx.cs" Inherits="Web_TPMTrakDashboard.SplitFun" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        .Span {
            color: black;
            font-size: larger;
            font-family: 'Times New Roman', Times, serif;
            /*font-style:oblique;*/
            font-weight: 600;
        }

        .label {
            /*background-color:black;*/
            border-color: white;
            border-width: 3px;
            color: black;
            font-size: larger;
            font-family: 'Microsoft Sans Serif';
            /*font-style:oblique;*/
            font-weight: 600;
        }

        td {
            padding: 10px;
            /*background:black;*/
            white-space: nowrap;
            border-width: 10px;
        }

        table, th, td, tr {
            border: 1px solid black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div style="padding: 10px;background-color:#0A246A">
            <asp:UpdatePanel runat="server" ID="Updatepanel">
                <ContentTemplate>
                    <table style="width: 100%; background-color: white; padding: 10px">
                        <tr>
                            <td>
                                <span class="Span">Machine-ID</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblMachineID" CssClass="label" />
                            </td>
                            <td>
                                <span class="Span">Component-ID</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblComponentID" CssClass="label" />
                            </td>
                            <td>
                                <span class="Span">Operation No.</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOperationNumber" CssClass="label" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="Span">Down Time Start</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblDownTimeStart" CssClass="label" />
                            </td>
                            <td>
                                <span class="Span">Break At</span>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBreakDate" TextMode="Date" AutoPostBack="true" />
                                <asp:TextBox runat="server" ID="txtBreaktime" TextMode="Time" step="1" AutoPostBack="true" />
                            </td>
                            <td>
                                <span class="Span">Down End Time</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblDownEndTime" CssClass="label" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="Span">Down-ID</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblDownID" CssClass="label" />
                            </td>
                            <td>
                                <span class="Span">Operator</span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOperator" CssClass="label" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSplit" Text="Split" OnClick="btnSplit_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
        </div>
    </form>
    <script>
        
    </script>
</body>
</html>
