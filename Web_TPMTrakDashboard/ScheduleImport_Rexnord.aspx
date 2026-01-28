<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleImport_Rexnord.aspx.cs" Inherits="Web_TPMTrakDashboard.ScheduleImport_Rexnord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Scripts/Toastr/toastr.min.css" rel="stylesheet" />
    <script src="Scripts/Toastr/toastr.min.js"></script>
    <style>
        .legendstylesettings {
            border: 0px;
            font-size: 16px;
            margin-left: 25px;
            padding-left: 5px;
            margin-bottom: 0px !important;
            color: white;
        }

        #gvData tr th {
            text-align: center;
        }

        #gvData tr td {
            text-align: center;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <table class="filter-table">
                            <tr>
                                <td>
                                    <asp:FileUpload runat="server" ID="fileUploader" CssClass="form-control" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ClientIDMode="Static" ID="btnImport" Text="IMPORT" OnClick="btnImport_Click" CssClass="btn btn-info" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-4" style="float: right; text-align: right;">
                        <asp:LinkButton runat="server" ID="btnTemplate" Text="Download Sample Template" CssClass="glyphicon glyphicon-download-alt btn btn-info" ClientIDMode="Static" OnClick="btnTemplate_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
            </div>
            <div style="height: 80vh; overflow: auto;">
                <asp:GridView runat="server" ID="gvData" CssClass="table table-bordered alternate-table-style headerFixer" ClientIDMode="Static" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="IDD" HeaderText="Sl No" ReadOnly="true" />
                        <asp:BoundField DataField="OperationDesc" HeaderText="Operation short text" ReadOnly="true" />
                        <asp:BoundField DataField="WorkCenter" HeaderText="Work center" ReadOnly="true" />
                        <asp:BoundField DataField="SerialNo" HeaderText="Order/Serial number" ReadOnly="true" />
                        <asp:BoundField DataField="WorkOrderNo" HeaderText="SO Line /Work order no" ReadOnly="true" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" ReadOnly="true" />
                        <asp:BoundField DataField="ComponentID" HeaderText="Material/Com id" ReadOnly="true" />
                        <asp:BoundField DataField="ComponentDesc" HeaderText="Material Description" ReadOnly="true" />
                        <asp:BoundField DataField="Operation" HeaderText="Operation/Activity" ReadOnly="true" />
                        <%--<asp:BoundField DataField="ProcessingTime" HeaderText="Processing Time" ReadOnly="true" />--%>
                        <asp:BoundField DataField="UpdatedTS" HeaderText="Updated TS" ReadOnly="true" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnTemplate" />
            <asp:PostBackTrigger ControlID="btnImport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
