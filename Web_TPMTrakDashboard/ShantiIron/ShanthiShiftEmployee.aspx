<%@ Page Title="Opr/Sup Allocation " Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShanthiShiftEmployee.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.ShanthiShiftEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        .header {
            background: #2E6886;
            color: white;
        }

        .table tbody {
            text-align: center;
        }

        .table tfoot > tr > td {
            padding: 8px;
            line-height: 1.428571429;
            vertical-align: top;
            border-top: none;
        }
    </style>
    <div>
        <table class="table table-bordered" style="display: inline-block;">
            <tr>
                <td style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white">Machine ID
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="cmbMachineID" CssClass="form-control" />
                </td>
                <td style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white;">Type
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="cmbtype" CssClass="form-control">
                        <asp:ListItem Text="Operator" Value="Operator" />
                        <asp:ListItem Text="Supervisor" Value="Supervisor" />
                    </asp:DropDownList>
                </td>
                <td style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white">From Date
                </td>
                <td class="input-group">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip"
                        title="Date !" placeholder="Date"></asp:TextBox>
                </td>
                <td>
                    <asp:Button runat="server" ID="View" OnClick="View_Click" CssClass="btn btn-info btn-sm displayCss" Text="View" Width="80" />
                </td>
                <td>
                    <asp:Button runat="server" ID="Save" OnClick="Save_Click" CssClass="btn btn-info btn-sm displayCss" Text="Save" Width="80" />
                </td>
            </tr>
        </table>
    </div>


    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <asp:GridView runat="server" ID="gridOperatorMachine" AutoGenerateColumns="false" CssClass="ui compact celled definition table" OnRowDataBound="gridOperatorMachine_RowDataBound">
                    <EmptyDataTemplate>
                        <asp:Label runat="server" Text="No Data Found" ForeColor="White"/>
                    </EmptyDataTemplate>
                    <Columns>
                        
                        <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White" HeaderText="SL No.">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Bind("SLNO") %>' ID="lblSlno" Style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white" />
                                <asp:HiddenField runat="server" ID="hidMachineid" Value='<%# Bind("MachineID") %>' />
                                <asp:HiddenField runat="server" ID="HidType" Value='<%# Bind("type") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee" HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">

                            <ItemTemplate>
                                <asp:Label runat="server" ID="cmbOperator" Text='<%#Bind("Operator") %>' Style="text-align: center; vertical-align: central; align-content: center; padding-top: 15px; color: white" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BackColor="#2E6886" HeaderStyle-ForeColor="White">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="Header1"  Style="color: white" />
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>

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
                                     <tr style="visibility:collapse;border-style:hidden">
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
                                    <tr>
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
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="Header2" Style="color: white" />
                                        </td>
                                        <td>

                                        </td>
                                        <td>

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
                                     <tr style="text-align: center;visibility:collapse;border-style:hidden">
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
                                    <tr>
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
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="Header3" Style="color: white" />
                                        </td>
                                        <td>

                                        </td>
                                        <td>

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
                                     <tr style="text-align: center;visibility:collapse;border-style:hidden">
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
                                    <tr>
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
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="Header4" Style="color: white" />
                                        </td>
                                        <td>

                                        </td>
                                        <td>

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
                                      <tr style="text-align: center;visibility:collapse;border-style:hidden">
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
                                    <tr>
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
                                    <tr>
                                        <td colspan="3">
                                             <asp:Label runat="server" ID="Header5" Style="color: white" />
                                        </td>
                                        <td>

                                        </td>
                                        <td>

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
                                     <tr style="text-align: center;visibility:collapse;border-style:hidden">
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
                                    <tr>
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
                                    <tr>
                                        <td colspan="3">
                                             <asp:Label runat="server" ID="Header6" Style="color: white" />
                                        </td>
                                        <td>

                                        </td>
                                        <td>

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
                                     <tr style="text-align: center;visibility:collapse;border-style:hidden">
                                        <th style="text-align: center;">
                                            <asp:Label runat="server" ID="lbldate6shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                        </th>
                                        <th style="text-align: center;">
                                            <asp:Label runat="server" ID="lbldate6shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                        </th>
                                        <th style="text-align: center;">
                                            <asp:Label runat="server" ID="lbldate6shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                        </th>
                                    <tr>
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
                                    <tr style="border-style:hidden">
                                        <td colspan="3">
                                             <asp:Label runat="server" ID="Header7" Style="color: white" />
                                        </td>
                                        <td>

                                        </td>
                                        <td>

                                        </td>
                                    </tr>
                                    <tr style="text-align: center;border-style:hidden">
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
                                    <tr style="text-align: center;visibility:collapse;border-style:hidden">
                                        <th style="text-align: center;">
                                            <asp:Label runat="server" ID="lbldate7shift1" Text='<%#Bind("shift1") %>' Style="color: white" />
                                        <th style="text-align: center;">
                                            <asp:Label runat="server" ID="lbldate7shift2" Text='<%#Bind("shift2") %>' Style="color: white" />
                                        </th>
                                        <th style="text-align: center;">
                                            <asp:Label runat="server" ID="lbldate7shift3" Text='<%#Bind("shift3") %>' Style="color: white" />
                                        </th>
                                    </tr>
                                    <tr>
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
