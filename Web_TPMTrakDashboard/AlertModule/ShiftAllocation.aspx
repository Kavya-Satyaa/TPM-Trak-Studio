<%@ Page Title="Shift Allocation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShiftAllocation.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.ShiftAllocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
      <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>
    
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="ui segment">
                    <table style="display: inline-block;">
                        <tr>
                            <td style="text-align: center; vertical-align: central; align-content: center;width:80px; color: black">Plant-ID :
                            </td>
                            <td style="width:150px">
                                <asp:DropDownList runat="server" ID="cmbPlantID" CssClass="form-control" Width="140px" />
                            </td>
                            <td style="text-align: center; vertical-align: central; align-content: center;width:60px;  color: black">Date :
                            </td>
                            <td class="input-group" style="width:180px">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="150" CssClass="form-control date" data-toggle="tooltip"
                                    title="Date !" placeholder="Date"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="View" OnClick="View_Click" CssClass="ui violet button" Text="View" Width="80" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="Save" OnClick="Save_Click" CssClass="ui violet button" Text="Save" Width="80" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="cmbOperator" CssClass="form-control" ToolTip="Operator" Width="100"/>

                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnPrev" OnClick="btnPrev_Click" CssClass="ui violet button" Text="Prev" Width="80" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnNext" OnClick="btnNext_Click" CssClass="ui violet button" Text="Next" Width="80" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="overflow: auto">
                    <asp:GridView runat="server" ID="gridOperatorMachine" AutoGenerateColumns="false" CssClass=" compact celled definition table" OnRowDataBound="gridOperatorMachine_RowDataBound">
                        <EmptyDataTemplate>
                            <div>
                                <asp:Label runat="server" ForeColor="White" Text="No Data Found Please select Different Plant or Date" />
                            </div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White" HeaderText="SL No.">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Bind("SLNO") %>' ID="lblSlno" Style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white" />
                                    <asp:HiddenField runat="server" ID="hidPlantid" Value='<%# Bind("PlantID") %>' />
                                    <%--<asp:HiddenField runat="server" ID="HidType" Value='<%# Bind("type") %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Consumer" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="cmbOperator" Text='<%#Bind("Consumer") %>' Style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email-Id" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="cmbEmailID" Text='<%#Bind("EmailId") %>' Style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white" />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone No." HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="cmbPhone" Text='<%#Bind("PhoneNo") %>' Style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white" />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table>
                                        <tr style="border-style: hidden; white-space: nowrap;">
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="Header1" Style="color: white" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lbldate1shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lbldate1shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lbldate1shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <table>
                                        <tr style="text-align: center; border-style: hidden">
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate1shift1" Checked='<%#Bind("chkdate1shift1") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate1shift2" Checked='<%#Bind("chkdate1shift2") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate1shift3" Checked='<%#Bind("chkdate1shift3") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table>
                                        <tr style="border-style: hidden; white-space: nowrap;">
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="Header2" Style="color: white" />
                                            </td>
                                        </tr>
                                        <tr style="text-align: center;">
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate2shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate2shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate2shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                            </th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr style="text-align: center; border-style: hidden">
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate2shift1" Checked='<%#Bind("chkdate2shift1") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate2shift2" Checked='<%#Bind("chkdate2shift2") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate2shift3" Checked='<%#Bind("chkdate2shift3") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table>
                                        <tr style="border-style: hidden; white-space: nowrap;">
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="Header3" Style="color: white" />
                                            </td>
                                        </tr>
                                        <tr style="text-align: center;">
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate3shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate3shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate3shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                            </th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr style="text-align: center; border-style: hidden">
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate3shift1" Checked='<%#Bind("chkdate3shift1") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate3shift2" Checked='<%#Bind("chkdate3shift2") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate3shift3" Checked='<%#Bind("chkdate3shift3") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table>
                                        <tr style="border-style: hidden; white-space: nowrap;">
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="Header4" Style="color: white" />
                                            </td>
                                        </tr>
                                        <tr style="text-align: center;">
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate4shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate4shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate4shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                            </th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr style="text-align: center; border-style: hidden">
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate4shift1" Checked='<%#Bind("chkdate4shift1") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate4shift2" Checked='<%#Bind("chkdate4shift2") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate4shift3" Checked='<%#Bind("chkdate4shift3") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table>
                                        <tr style="border-style: hidden; white-space: nowrap;">
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="Header5" Style="color: white" />
                                            </td>
                                        </tr>
                                        <tr style="text-align: center;">
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate5shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate5shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate5shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                            </th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr style="text-align: center; border-style: hidden">
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate5shift1" Checked='<%#Bind("chkdate5shift1") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate5shift2" Checked='<%#Bind("chkdate5shift2") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate5shift3" Checked='<%#Bind("chkdate5shift3") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table>
                                        <tr style="border-style: hidden; white-space: nowrap;">
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="Header6" Style="color: white" />
                                            </td>
                                        </tr>
                                        <tr style="text-align: center;">
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate6shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate6shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate6shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                            </th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr style="text-align: center; border-style: hidden">
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate6shift1" Checked='<%#Bind("chkdate6shift1") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate6shift2" Checked='<%#Bind("chkdate6shift2") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate6shift3" Checked='<%#Bind("chkdate6shift3") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                                <HeaderTemplate>
                                    <table>
                                        <tr style="border-style: hidden; white-space: nowrap;">
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="Header7" Style="color: white" />
                                            </td>
                                        </tr>
                                        <tr style="text-align: center; border-style: hidden">
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate7shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate7shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                            </th>
                                            <th style="text-align: center;">
                                                <asp:Label runat="server" ID="lbldate7shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                            </th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table>
                                        <tr style="text-align: center; border-style: hidden">
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate7shift1" Checked='<%#Bind("chkdate7shift1") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate7shift2" Checked='<%#Bind("chkdate7shift2") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkdate7shift3" Checked='<%#Bind("chkdate7shift3") %>' />
                                            </td>
                                        </tr>
                                    </table>
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
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
