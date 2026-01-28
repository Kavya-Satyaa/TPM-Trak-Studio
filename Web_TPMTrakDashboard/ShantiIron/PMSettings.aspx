<%@ Page Title="PM Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.PMSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>
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
    
    .headerFixerTable tr th {
            position: sticky;
            top: 0px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 100%;
            overflow: auto;
            margin-top: 15px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
            }

               .divGrid td {
               
                color: white;
                height:20px;
            }
        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        .table tbody > tr > th {
            vertical-align: middle;
            width:auto;
        }

        .table > tr > td {
            vertical-align: middle;
        }
        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 15px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }


        .table .lbl {
            padding-top: 10px;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }
        </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <div class="ui segment">
                    <h3>Search</h3>
                    <table>
                        <tr>
                           <%-- <td>
                                <span>Plant ID:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlplantID" OnSelectedIndexChanged="ddlplantID_SelectedIndexChanged" CssClass="form-control" />
                            </td>
                            <td>
                                <span>Cell ID:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlcellID" CssClass="form-control" OnSelectedIndexChanged="ddlcellID_SelectedIndexChanged" />
                            </td>--%>
                            <td>
                                <span>Machine ID:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass=" form-control" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnLoad" CssClass="ui violet button" OnClick="btnLoad_Click" Text="View" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView runat="server" ID="grdPMChecklist" OnRowCommand="grdPMChecklist_RowCommand" ClientIDMode="Static" CssClass="table table-bordered headerFixerTablee" AutoGenerateColumns="false" ShowFooter="true" ShowHeaderWhenEmpty="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Category" HeaderStyle-BackColor="#2E6886">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRule" Text='<%# Bind("Rule") %>' CssClass="form-control" />
                                    <asp:HiddenField runat="server" ID="hidRule" Value='<%# Bind("Rule") %>' />
                                    <asp:HiddenField runat="server" ID="IDD" Value='<%# Bind("ID") %>' />
                                </ItemTemplate>
                                <FooterTemplate >
                                    <asp:TextBox runat="server" ID="txtFooterRule" CssClass="form-control" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#2E6886">
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkUpdateRow" runat="server" CommandName="UpdateRow" CommandArgument='<%# Bind("Rule") %>' CssClass="glyphicon glyphicon-save" ToolTip="Update" Style="font-size: 20px;" />
                                    <asp:LinkButton ID="linkCancelRow" ClientIDMode="Static" runat="server" CommandName="DeleteRow" CommandArgument='<%#Bind("Rule") %>' CssClass="glyphicon glyphicon-remove delete" ToolTip="Cancel" Style="font-size: 20px;" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="linkAddRow" runat="server" CommandName="AddRow" CommandArgument='<%# Bind("Rule") %>' CssClass="glyphicon glyphicon-plus-sign" ToolTip="New" Style="font-size: 20px;" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="ui segment">
                    <h3>Search</h3>
                    <table>
                        <tr>
                            <td>
                                <span>Category</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCategory" CssClass="form-control" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="ui violet button" Text="View" />

                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView runat="server" ID="gridViewItems" ClientIDMode="Static" OnRowCommand="gridViewItems_RowCommand" CssClass="table table-bordered headerFixerTable" AutoGenerateColumns="false" ShowFooter="true" ShowHeaderWhenEmpty="true" OnRowDataBound="gridViewItems_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCategory" Text='<%#Bind("Rule") %>' CssClass="form-control" />
                                    <asp:HiddenField runat="server" ID="IDD" Value='<%# Bind("ID") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlFooterCategory" CssClass="form-control" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidItems" Value='<%# Bind("Items") %>' />
                                    <asp:TextBox runat="server" ID="txtItem" Text='<%#Bind("Items") %>' CssClass="form-control" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox runat="server" ID="footerItem" CssClass="form-control" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkUpdateRow" runat="server" CommandName="UpdateRow" CommandArgument='<%# Bind("Items") %>' CssClass="glyphicon glyphicon-save" ToolTip="Update" Style="font-size: 20px;" />
                                    <asp:LinkButton ID="linkCancelItemsRow" ClientIDMode="Static" runat="server" CommandName="DeleteRow" CommandArgument='<%#Bind("Items") %>' CssClass="glyphicon glyphicon-remove delete" ToolTip="Cancel" Style="font-size: 20px;" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="linkAddRow" runat="server" CommandName="AddRow" CommandArgument='<%# Bind("Items") %>' CssClass="glyphicon glyphicon-plus-sign" ToolTip="New" Style="font-size: 20px;" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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

            $(".delete").click(function () {
                debugger;
                if (confirm("Are you sure to delete the row"))
                    return true;
                else
                    return false;
            });
           
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(".date").datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $(".delete").click(function () {
                debugger;
                if (confirm("Are you sure to delete the row"))
                    return true;
                else
                    return false;
            });
            
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
