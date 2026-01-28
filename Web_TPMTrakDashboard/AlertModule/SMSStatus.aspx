<%@ Page Title="SMS Status" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SMSStatus.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.SMSStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>

    <div>
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>
                <div class="ui segment">
                    <table>
                        <tr>
                            <td style="width: 60px;">From :
                            </td>
                            <td class="input-group" style="width: 240px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" Width="180" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                    title="Date !" placeholder="Date"></asp:TextBox>
                            </td>
                            <td style="width: 50px;">To :
                            </td>
                            <td class="input-group" style="width: 240px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" Width="180" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                    title="Date !" placeholder="Date"></asp:TextBox>
                            </td>
                            <td style="width: 100px;">
                                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="ui violet button" />
                            </td>
                            <td style="width: 100px;">
                                <asp:Button runat="server" ID="btnExport" Text="Export" OnClick="btnExport_Click" CssClass="ui violet button" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="ui segment">
                    <h3>Summary
                    </h3>
                    <table>
                        <tr>
                            <td>Total No. of Messages:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTotalNoMessages" Enabled="false" CssClass="form-control" />
                            </td>
                            <td>Distinct Phone No:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDistinctPhNo" Enabled="false" CssClass="form-control" />
                            </td>
                            <td>Total No. of Messages Sent:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTotalNoofMessagesSent" Enabled="false" CssClass="form-control" />
                            </td>
                        </tr>

                    </table>
                </div>
                <div>
                    <asp:GridView runat="server" ID="gridmessagedetails" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CssClass="table table-bordered headerFixerTable">
                        <EmptyDataTemplate >
                            <asp:Label Text="No Data Found" runat="server" />
                        </EmptyDataTemplate>
                        
                        <Columns>
                            <asp:TemplateField HeaderText="SLNO" HeaderStyle-ForeColor="White">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblslno" Text='<%#Bind("SLNO") %>' ForeColor="White" Style="white-space: nowrap" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DateTime" HeaderStyle-ForeColor="White">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDatetime" Text='<%#Bind("DateTime") %>' ForeColor="White" Style="white-space: nowrap" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mobile Number" HeaderStyle-ForeColor="White">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMobileNumber" Text='<%#Bind("MobileNumber") %>' ForeColor="White" Style="white-space: nowrap" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. of Message Sent" HeaderStyle-ForeColor="White">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNoOfMessageSent" Text='<%#Bind("NoOfMessageSent") %>' ForeColor="White" Style="white-space: nowrap" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Message" HeaderStyle-ForeColor="White">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMessage" Text='<%#Bind("Message") %>' ForeColor="White" Style="white-space: nowrap" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
