<%@ Page Language="C#" Title="Operation Bypass" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="OperationBypass.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.OperationBypass" %>

<asp:Content ID="MainContentArea" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.js"></script>
    <link href="Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.min.js"></script>
    <style type="text/css">
        .header-center {
            text-align: center;
        }

        table tr th {
            text-align: center !important;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }

        .DataOperations {
            bottom: 0;
            right: 0;
            float: right;
            margin-right: 5px;
            min-height: 40px;
            position: fixed;
        }

        .btnStyle {
            margin-left: 2px;
            margin-right: 2px;
        }
    </style>

    <div class="container-fluid" style="margin-left: -5px;">
        <asp:UpdatePanel ID="UpdatePanelOpnBypass" runat="server">
            <ContentTemplate>
                <div>
                    <table style="display: inline-block; margin: 5px;">
                        <tr>
                            <td style="color: white">Component ID:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtComp" Width="180px" CssClass="form-control" />
                            </td>
                            <td style="color: white; margin-left: 5px;">Date:
                            </td>
                            <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary btnStyle" />

                            </td>
                        </tr>
                    </table>
                </div>

                <div style="overflow-x: auto; overflow-y: auto; height: 75vh;">
                    <asp:GridView ID="GridOperationBypass" runat="server" OnRowDataBound="GridOperationBypass_RowDataBound" AutoGenerateColumns="false" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" HeaderStyle-Font-Size="Large" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true" HeaderStyle-Height="40" RowStyle-Height="30" ClientIDMode="Static" OnRowCommand="GridOperationBypass_RowCommand" ShowFooter="false">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for operation bypass.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="IDD" SortExpression="IDD" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDD" runat="server" Text='<%#Bind("SLNO") %>'></asp:Label>
                                    <asp:HiddenField ID="hid" runat="server" Value=""></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Component ID" ControlStyle-Width="160">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblComponentID" Text='<%# Bind("ComponentID")  %>' />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation No">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperationNo" Text='<%# Bind("OperationNo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective From Date">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <%--<td class="commontd" style="width: 60px;"><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b></td>--%>
                                            <td class="input-group" style="width: 160px; border-color: transparent; height: 60px;">
                                                <div class="input-group-addon">
                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                </div>
                                                <asp:Label ID="lbleffectivefromdate" runat="server" Text='<%# Bind("EffectiveFromDate") %>' Visible="false" />
                                                <asp:TextBox ID="txtEffectivefrom" Text='<%# Bind("EffectiveFromDate") %>' Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Effective Form Date" AutoCompleteType="Disabled"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective To Date">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td class="input-group" style="width: 160px; border-color: transparent; height: 60px;">
                                                <div class="input-group-addon">
                                                    <i class="glyphicon glyphicon-calendar"></i>
                                                </div>
                                                <asp:Label ID="lbleffectivetodate" runat="server" Text='<%# Bind("EffectiveToDate") %>' Visible="false" />
                                                <asp:TextBox ID="txtEffectiveto" runat="server" Text='<%# Bind("EffectiveToDate") %>' Style="width: 160px; height: 42px" CssClass="form-control date" placeholder="Effective To Date" AutoCompleteType="Disabled"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkDeleteRow" runat="server" CommandName="DeleteRow" CommandArgument='<%#Bind("SLNO") %>' CssClass="glyphicon glyphicon-trash "
                                        ToolTip="Delete" Style="font-size: 20px;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="ui segment" style="margin: 5px;">
                    <table style="width: 100%; height: 70px;" class="table table-bordered">
                        <tr>
                            <td style="width: 170px; vertical-align: middle;">
                                <asp:DropDownList runat="server" ID="ddlComponent" Width="160" CssClass="form-control" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                            <td style="width: 100px; vertical-align: middle;">
                                <asp:DropDownList Width="90" runat="server" ID="ddlOperation" CssClass="form-control" />

                            </td>
                            <td class="commanTd" style="text-align: center; width: 140px; vertical-align: central; font-size: medium">Effective From date:
                            </td>
                            <td class="input-group" style="width: 180px; border-color: transparent; height: 40px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFooterEffectivefrom" Text='<%# Bind("EffectiveToDate") %>' Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Effective Form Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="text-align: center; width: 140px; vertical-align: central; font-size: medium">Effective To Date:
                            </td>
                            <td class="input-group" style="width: 180px; border-color: transparent; height: 40px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFooterEffectiveto" runat="server" Style="width: 180px; height: 42px" CssClass="form-control date" placeholder="Effective To Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnAdd" Text="Add" CssClass="btn btn-primary btnStyle" OnClick="btnAdd_Click" />
                                <asp:Button runat="server" ID="btnsave" Text="Save" CssClass="btn btn-primary btnStyle" OnClick="btnsave_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $.unblockUI({});
            $(".date").datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('#GridOperationBypass tr').click(function () {
                debugger;

                $(this).find('td').find('input[type="hidden"]').val("Update");
            });
            $("[id$=linkDeleteRow]").click(function () {
                debugger;
                if (confirm("Are u Sure To Aggregate?")) {
                    return true;
                }
                else {
                    return false;
                }
            });
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(".date").datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('#GridOperationBypass tr').click(function () {
                debugger;
                $(this).find('td').find('input[type="hidden"]').val("Update");
            });
            $("[id$=linkDeleteRow]").click(function () {
                debugger;
                if (confirm("Are u Sure To Aggregate?")) {
                    return true;
                }
                else {
                    return false;
                }
            });
        });
    </script>

</asp:Content>

<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
