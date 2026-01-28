<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportScheduleErrorData.aspx.cs" Inherits="Web_TPMTrakDashboard.AceDesigners.ImportScheduleErrorData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        fieldset {
            border: 1px solid white !important;
            padding: 0.1em 0.5em 1.1em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            margin: 0px;
            vertical-align: top;
        }

        legend {
            font-size: 1.1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            color: white;
            border-bottom: none;
            margin-top: -4px;
            margin: 0px;
        }

        #gvErrorData tr td {
            background-color: #FFFFFF;
            color: black;
            vertical-align: middle;
        }

        .pagination-style table {
            margin: auto;
        }

        .pagination-style > td {
            background-color: #eae8e8 !important;
        }

        .pagination-style table tr td {
            border: unset !important;
            padding: 0px;
        }

            .pagination-style table tr td span {
                padding: 5px 10px;
                background-color: #428bca;
                color: white;
                border: 1px solid silver;
            }

            .pagination-style table tr td a {
                padding: 5px 10px;
                background-color:white;
                border: 1px solid silver;
            }
    </style>
    <div class="container-fluid" style="">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
                <asp:HiddenField runat="server" ID="hdnViewType" ClientIDMode="Static" />
                <table style="width: auto; margin-left: 10px;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Search by Date</legend>
                                <table>
                                    <tr>
                                        <td class="commanTd" style="vertical-align: middle;">From</td>
                                        <td class="input-group" style="min-width: 130px; border: 0">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                        <td class="commanTd" style="vertical-align: middle;">To</td>
                                        <td class="input-group" style="min-width: 130px; border: 0">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" Style="width: 130px;" CssClass="form-control date1" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnDateView" Text="View" Style="" OnClick="btnDateView_Click" OnClientClick="return showloader();" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <legend>Search by PO</legend>
                                <table>
                                    <tr>
                                        <td class="commanTd" style="vertical-align: middle;">PO&nbsp;</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPOSearch" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnCompPOView" Text="View" OnClick="btnCompPOView_Click" OnClientClick="return showloader();" />
                                        </td>
                                    </tr>
                                </table>
                        </td>
                        <td>&nbsp;
                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnRefresh" Text="Refresh" OnClick="btnRefresh_Click" Visible="false" />
                        </td>
                        <td>&nbsp;
                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnExport" Text="Export" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
                <div id="gridContainer" style="margin-top: 10px; height: 75vh; overflow: auto">
                    <asp:GridView runat="server" ID="gvErrorData" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false" Width="100%" CssClass="table table-bordered  headerFixer" ClientIDMode="Static" BorderStyle="NotSet" GridLines="None" AllowPaging="true" OnPageIndexChanging="gvErrorData_PageIndexChanging" OnPreRender="gvErrorData_PreRender" PageSize="500">
                        <Columns>
                            <asp:TemplateField HeaderText="Machine">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text='<%# Eval("MachineDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Production Order">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPO" ClientIDMode="Static" Text='<%# Eval("ProductionOrder") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Component ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label2" ClientIDMode="Static" Text='<%# Eval("CompID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operation No.">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label3" ClientIDMode="Static" Text='<%# Eval("OpnNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Error Message">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label4" ClientIDMode="Static" Text='<%# Eval("ErroMsg") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Timestamp">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Label5" ClientIDMode="Static" Text='<%# Eval("UpdatedTS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pagination-style" />
                    </asp:GridView>
                    <%--<asp:ListView runat="server" ID="lvErrorData" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table id="tblScheduleData" class="table table-bordered  headerFixer" style="width: 100%; background: white">
                                <tr>
                                    <th>Machine</th>
                                    <th>Production Order</th>
                                    <th>Component ID</th>
                                    <th>Operation No.</th>
                                    <th>Error Message</th>
                                    <th>Timestamp</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label1" ClientIDMode="Static" Text='<%# Eval("MachineDesc") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPO" ClientIDMode="Static" Text='<%# Eval("ProductionOrder") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label2" ClientIDMode="Static" Text='<%# Eval("CompID") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label3" ClientIDMode="Static" Text='<%# Eval("OpnNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label4" ClientIDMode="Static" Text='<%# Eval("ErroMsg") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label5" ClientIDMode="Static" Text='<%# Eval("UpdatedTS") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>--%>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        var bigDiv = document.getElementById('gridContainer');
        $(document).ready(function () {
            setControls();
            $.unblockUI({});
        });
        function setControls() {
            $('.date1').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US',
            });
        }
        function showloader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            var bigDiv = document.getElementById('gridContainer');
            $(document).ready(function () {
                setControls();
                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
                $.unblockUI({});
            });
            bigDiv.onscroll = function () {
                $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            }
            window.onload = function () {

                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
