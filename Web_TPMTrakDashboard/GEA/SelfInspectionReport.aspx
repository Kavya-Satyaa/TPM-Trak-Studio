<%@ Page Title="Self Inspection Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelfInspectionReport.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.SelfInspectionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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

        .WeekScheduler {
            vertical-align: middle;
        }

        .InspectionStatus {
            vertical-align: middle;
        }

        .Row {
            display: table;
            border-spacing: 5pt;
            width: 100%;
        }

        .Col {
            display: table-cell;
            height: 50px;
            width: 100%;
            border: 1pt solid black;
            background-color: #DBDBDB;
        }

        .MiddleLeft {
            text-align: left;
            align-items: normal;
            vertical-align: middle;
        }

        .form-control {
            width: 98%;
        }

        #tblSelfInspectionDescription {
            width: 70%;
        }

            #tblSelfInspectionDescription tr td:nth-child(2n+1) {
                width: 50px;
                max-width: 50px;
                background-color: #2e6886;
                color: white;
                font-size: 15px;
            }

            #tblSelfInspectionDescription tr td:nth-child(2n) {
                width: 80px;
                max-width: 80px;
                color: white;
            }

        input[type="checkbox"] {
            height: 15px;
            width: 15px;
            vertical-align: text-bottom;
            margin-right: 2px;
        }
    </style>

    <div class="container-fluid" style="margin-left: 5px;">
        <asp:UpdatePanel ID="UpdatePanelMaintChklistReport" runat="server">
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnExport" />--%>
                <asp:PostBackTrigger ControlID="Export" />
            </Triggers>
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row" style="height: 140px;">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="width:400px; vertical-align: middle;">
                                <table>
                                    <tr>
                                        <td class="commanTd" style="width: 170px; vertical-align: middle;border:none;">Production Order No.</td>
                                        <td style="width: 180px;border:none;">
                                            <asp:TextBox ID="txtprodOrder" runat="server" CssClass="form-control" placeholder="ProdOrder Search" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="commanTd" style="width: 170px;border:none; vertical-align: middle;">Component No.</td>
                                        <td style="width: 180px;border:none;">
                                            <asp:TextBox ID="txtCompSearch" runat="server" CssClass="form-control" placeholder="Component Search" />
                                        </td>
                                    </tr>
                                </table>
                            </td>

                            <td style="width: 60px;vertical-align: middle;">
                                <asp:LinkButton ID="btnComponent" runat="server" CssClass="btn btn-primary" OnClick="btnComponent_Click">
								    <span aria-hidden="true" class="glyphicon glyphicon-refresh"></span>
                                </asp:LinkButton>
                            </td>
                            <td style="width: 180px;vertical-align: middle;">
                                <asp:DropDownList ID="ddlPONumber" runat="server" OnSelectedIndexChanged="ddlPONumber_SelectedIndexChanged" CssClass="form-control" AutoPostBack="true" />
                            </td>

                            <td class="commanTd" style="width: 170px; vertical-align: middle;">Operation No.</td>
                            <td style="width: 160px;vertical-align: middle;">
                                <asp:DropDownList ID="ddlOperationNumber" runat="server" CssClass="form-control" AutoPostBack="false" />
                            </td>
                            <td style="width: 130px;vertical-align: middle;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" OnClick="btnView_Click" Style="min-width: 120px;" />
                            </td>
                            <td style="vertical-align: middle;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="Export" Text="Export" OnClick="Export_Click" Style="min-width: 120px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row" style="">
                    <table id="tblSelfInspectionDescription" runat="server" clientidmode="static" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Description</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblDescription"></asp:Label>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Inspection Plan No.</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblPalnNo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Inspection Scope</td>
                            <td style="">
                                <asp:CheckBox runat="server" ID="cbIncominggoods" onclick="return false" Text="Incoming goods" />&nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:CheckBox runat="server" ID="cbProduction" onclick="return false" Checked="true" Text="Production" />
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Others</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblOthers"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Part No.</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblPartNo"></asp:Label>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Drawing No.</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblDrawingNo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Production Order #</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblPONo"></asp:Label>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Series/Serial No.</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblSerialNo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Machine ID</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblMachineId"></asp:Label>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Operation No.</td>
                            <td style="">
                                <asp:Label runat="server" ID="lblOpeartionNo"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="gridHeightset" style="overflow-x: auto; overflow-y: auto; margin-left: -13px">

                    <asp:GridView ID="GridSelfInspectionReport" runat="server" ClientIDMode="Static" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" Height="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="No.">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRowNo" Text='<%# Bind("RowNumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inspection Parameters">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblParameter" Text='<%# Bind("Parameter") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator's Measurement">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperatorMeasurement" Text='<%# Bind("OperatorMeasurement") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quality's Measurement">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblQualityMeasurement" Text='<%# Bind("QualityMeasurement") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operator Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOperatorName" Text='<%# Bind("OperatorName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date / Shift">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDate" Text='<%# Bind("DateorShift") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRemark" Text='<%# Bind("Remarks") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected Production Order and Operation Number.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {

            var wHeight = $(window).height() - ($("#tblSelfInspectionDescription").height() + 170);
            $('.gridHeightset').css('height', wHeight);
        });
        $(window).resize(function () {
            var wHeight = $(window).height() - ($("#tblSelfInspectionDescription").height() + 170);
            $('.gridHeightset').css('height', wHeight);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
