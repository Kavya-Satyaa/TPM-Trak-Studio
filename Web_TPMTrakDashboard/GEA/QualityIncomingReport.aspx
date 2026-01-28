<%@ Page Language="C#" Title="Quality Incoming Report" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="QualityIncomingReport.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.QualityIncomingReport" EnableEventValidation="false" Async="true" AsyncTimeout="120000" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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

        #tblQltyIncomingDescription {
            width: 80%;
        }

            #tblQltyIncomingDescription tr td:nth-child(2n+1) {
                width: 180px;
                max-width: 200px;
                background-color: #2e6886;
                color: white;
                font-size: 15px;
            }

            #tblQltyIncomingDescription tr td:nth-child(2n) {
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
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row" style="height: 60px;">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine ID</td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="ddlMachineID" runat="server" CssClass="form-control" />
                            </td>

                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Prod Order</td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="ddlPONumber" runat="server" CssClass="form-control" AutoPostBack="true" />
                            </td>

                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Material ID</td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="ddlMaterialID" runat="server" CssClass="form-control" AutoPostBack="true" />
                            </td>

                            <td class="commanTd" style="width: 90px; vertical-align: middle;">Operation</td>
                            <td style="width: 100px;">
                                <asp:DropDownList ID="ddlOperationNumber" runat="server" CssClass="form-control" AutoPostBack="false" />
                            </td>

                            <td class="commanTd" style="width: 80px; vertical-align: middle;">Plan No</td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="ddlInsPlanNumber" runat="server" CssClass="form-control" AutoPostBack="false" />
                            </td>

                            <td style="width: 90px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" Style="min-width: 80px;" />
                            </td>
                            <td style="width: 100px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnExport" Text="Export" Style="min-width: 90px" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>

                <div class="row">
                    <table id="tblQltyIncomingDescription" runat="server" clientidmode="static" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">GEA Company : </td>
                            <td>
                                <asp:Label runat="server" ID="lblCompany" Text="GEA Westfalia Separator India Pvt. Ltd."></asp:Label>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Description : </td>
                            <td>
                                <asp:Label runat="server" ID="lblDescription"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Part Number : </td>
                            <td>
                                <asp:Label runat="server" ID="lblPartNumber"></asp:Label>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Inspection Plan No. : </td>
                            <td>
                                <asp:Label runat="server" ID="lblInsPlanNo"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Prod Order No. : </td>
                            <td>
                                <asp:Label runat="server" ID="lblProdOrderNum"></asp:Label>
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Drawing No. : </td>
                            <td>
                                <asp:Label runat="server" ID="lblDrawingNum"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="commanTd" style="vertical-align: middle;">Inspection Area : </td>
                            <td style="">
                                <asp:CheckBox runat="server" ID="chkIncomingGoods" onclick="return false" Text="Incoming goods" />&nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:CheckBox runat="server" ID="chkProduction" onclick="return false" Checked="true" Text="Production" />
                            </td>
                            <td class="commanTd" style="vertical-align: middle;">Others : </td>
                            <td style="">
                                <asp:TextBox runat="server" ID="txtOthers" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="FooterContent" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
