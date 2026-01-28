<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.ProcessParameterSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .button {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            width: 80px;
            height: 30px;
        }

             .headerFixera tr th {
                position: sticky;
                top: -1px;
                z-index: 1;
                 background: black;
                color: white;
                text-align: center;
                font-size: larger;
                overflow-wrap:break-word;   
            }
             td{
                 height:40px;
                 padding:5px;
             }
    </style>
    <div style="height:90vh" class="headerFixera">
        <div style="height: 90%;overflow:auto;">
            <asp:ListView runat="server" ID="lstviewprocessparameter">
                <LayoutTemplate>
                    <table border="1" style="border-color: white; width: 100%; height: 140px; border-width: 2px; table-layout: fixed">
                        <tr>
                            <th>
                                <span>Machine-ID</span>
                            </th>
                            <th>
                                <span>Parameter-ID</span>
                            </th>
                            <th>
                                <span>Display Header</span>
                            </th>
                            <th>
                                <span>PLC Address</span>
                            </th>
                            <th>
                                <span>DataType</span>
                            </th>
                            <th>
                                <span>Unit</span>
                            </th>
                            <th>
                                <span>Show Unit</span>
                            </th>
                            <th>
                                <span>Show data Date</span>
                            </th>
                            <th>
                                <span>Green Range</span>
                            </th>
                            <th>
                                <table style="width: 100%;overflow-wrap:break-word;">
                                    <tr>
                                        <th colspan="2" style="width: 100%;overflow-wrap:break-word;"><span>Yellow</span>
                                        </th>
                                    </tr>
                                    <tr style="width: 100%">
                                        <th style="width: 50%;font-size:medium;overflow-wrap:break-word;"><span>Lower</span>
                                        </th>
                                        <th style="width: 50%;font-size:medium;overflow-wrap:break-word;"><span>Higher</span>
                                        </th>
                                    </tr>
                                </table>
                            </th>
                            <th>
                                <table style="width: 100%">
                                    <tr>
                                        <th colspan="2" style="width: 100%"><span>RED</span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 50%;font-size:medium;"><span>Lower</span>
                                        </th>
                                        <th style="width: 50%;font-size:medium;"><span>Higher</span>
                                        </th>
                                    </tr>
                                </table>
                            </th>
                            <th>
                                <span>Is Enabled</span>
                            </th>
                            <th>
                                <span>Display Order</span>
                            </th>
                            <th>
                                <span>Display Template</span>
                            </th>
                            <th></th>
                        </tr>
                        <tr id="ItemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="MachineID" Text='<%# Bind("MachineID") %>' Style="width: 100%;Height:70%;" />
                            <asp:HiddenField runat="server" ID="idd" Value='<%# Bind("idd") %>' />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ParameterID" DataSource='<%# Bind("ParameterIDList") %>' SelectedValue='<%# Bind("ParameterID") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="DisplayHeader" Text='<%# Bind("DisplayHeader") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="PlcAddress" Text='<%# Bind("PlcAddress") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="DataType" DataSource='<%# Bind("DatatypeList") %>' SelectedValue='<%# Bind("Datatype") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="Unit" Text='<%# Bind("Unit") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox runat="server" ID="ShowUnit" Checked='<%# Bind("ShowUnit") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox runat="server" ID="ShowDataDate" Checked='<%# Bind("ShowDatadate") %>' Style="width:100%;text-align:center;Height:70%;" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="GreenRange" Text='<%# Bind("GreenRange") %>' Style="width: 100%;Height:70%;" />
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="YellowHigher" Text='<%# Bind("YellowHigher") %>' Style="width:100%;Height:70%;" />

                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="YellowLower" Text='<%# Bind("YellowLower") %>' Style="width:100%;Height:70%;" />

                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="RedHigher" Text='<%# Bind("RedHigher") %>' Style="width:100%;Height:70%;" />

                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="RedLower" Text='<%# Bind("RedLower") %>' Style="width:100%;Height:70%;" />

                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox runat="server" ID="IsEnabled" Checked='<%# Bind("Enable") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="DisplayOrder" Text='<%# Bind("DisplayOrder") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="DisplayTemplate" DataSource='<%# Bind("DisplayTemplateList") %>' SelectedValue='<%# Bind("DisplayTemplate") %>' Style="width:100%;Height:70%;" />
                        </td>
                        <td style="text-align: center;">
                            <asp:CheckBox runat="server" ID="delcheckbox" />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table style="width: 100%">
                        <tr>
                            <th>
                                <span>Machine-ID</span>
                            </th>
                            <th>
                                <span>Parameter-ID</span>
                            </th>
                            <th>
                                <span>Display Header</span>
                            </th>
                            <th>
                                <span>PLC Address</span>
                            </th>
                            <th>
                                <span>DataType</span>
                            </th>
                            <th>
                                <span>Unit</span>
                            </th>
                            <th>
                                <span>Show Unit</span>
                            </th>
                            <th>
                                <span>Show data Date</span>
                            </th>
                            <th>
                                <span>Green Range</span>
                            </th>
                            <th>
                                <table style="width: 100%">
                                    <tr>
                                        <th colspan="2" style="width: 100%"><span>Yellow</span>
                                        </th>
                                    </tr>
                                    <tr style="width: 100%">
                                        <th style="width: 50%"><span>Lower</span>
                                        </th>
                                        <th style="width: 50%"><span>Higher</span>
                                        </th>
                                    </tr>
                                </table>
                            </th>
                            <th>
                                <table style="width: 100%">
                                    <tr>
                                        <th colspan="2" style="width: 100%"><span>RED</span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 50%"><span>Lower</span>
                                        </th>
                                        <th style="width: 50%"><span>Higher</span>
                                        </th>
                                    </tr>
                                </table>
                            </th>
                            <th>
                                <span>Is Enabled</span>
                            </th>
                            <th>
                                <span>Display Order</span>
                            </th>
                            <th>
                                <span>Display Template</span>
                            </th>
                        </tr>
                        <tr id="ItemPlaceholder" runat="server">
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
        <div style="height: 10%; width: 80%; text-align: end;padding:10px;width:100%;">
            <asp:Button Text="Add" runat="server" ID="btnAdd" OnClick="btnAdd_Click" CssClass="button" Style="background-color: steelblue; color: black" />
            <asp:Button Text="Save" runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" CssClass="button" Style="background-color: green; color: black" />
            <asp:Button Text="Delete" runat="server" ID="btnDelete" OnClick="btnDelete_Click" CssClass="button" Style="background-color: indianred; color: black" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
