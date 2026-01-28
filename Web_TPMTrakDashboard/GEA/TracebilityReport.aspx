<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TracebilityReport.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.TracebilityReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
         .table-style tr td {
                padding: 12px 10px;
                border: none;
                border-bottom: 0.5px solid #e3e6f0;
                border-right: 0.5px solid #e3e6f0;
                border-left: 0.5px solid #e3e6f0;
                text-align: center;
                color:white;
            }
           .table-style tr td span {
             font-weight:600; 
            }

            .table-style tr:hover td {
                background-color:  #f0f0f042;
            }

            .table-style  {
                border: unset !important;
            }
             .table-style {
                width: 100%;
                box-shadow: 0 .15rem 1.75rem 0 rgba(58,59,69,.15);
            }

            .table-style tr th, .table-header-fixer-for-dynamic-columns > tbody > tr:first-child td {
                padding: 14px 10px;
                border-top: 1px solid #d6d6d7;
                border-bottom: 1px solid #626262;
                background-color: #006099;
                border-right: 0.5px solid #e3e6f0;
                border-left: 0.5px solid #e3e6f0;
                color: white;
                position: sticky;
                top: 0px;
                z-index: 2;
                font-weight: bold;
                text-align: center;
            }
             .btn-style {
                background-color: #00B9FF;
                color: white;
                font-weight: bold;
                border: unset;
                padding: 7px 19px;
                margin-right: 2px;
               /* box-shadow: 2px 2px 5px 1px #cfcfcf;*/
                max-width: unset;
                border-radius: 2px;
            }
    </style>
    <div style="color:white">
         <h3>Tracebility Report</h3>
    </div>
    <div style="margin-bottom:8px;text-align:end">
        <asp:Button runat="server" Text="<%$ Resources:CommanResource,Export %>" class="btn btn-info" ID="btnExport1" OnClick="btnExport_Click"></asp:Button>
    </div>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                 <div id="scrollMaintainDiv" style="overflow: auto; height: 72vh">
                    <asp:ListView runat="server" ID="lvTracebilityGridDetails" ClientIDMode="Static" OnItemDataBound="lvTracebilityGridDetails_ItemDataBound">
                            <EmptyDataTemplate>
                                <table class="table-style" id="tblTracebilityDetails">
                                    <tr>
                                        <th style="width:300px;">Description</th>
                                        <th style="width:300px;">Part Number</th>
                                        <th style="width:300px;">Serial No</th>
                                        <th style="width:300px;">Search</th>
                                        <th>Machine Number</th>
                                    </tr>
                                    <tr>
                                        <td colspan="100" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table class="table-style search-tbl" id="tblTracebilityDetails">
                                    <tr>
                                        <th style="width:300px;">Description</th>
                                        <th style="width:300px;">Part Number</th>
                                        <th style="width:300px;">Serial No</th>
                                        <th style="width:250px;">Search</th>
                                        <th>Machine Number</th>
                                    </tr>
                                    <tr id="itemplaceholder" runat="server"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblParameterDesc" Text='<%# Eval("ParameterDescription") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnParamID" ClientIDMode="Static" Value='<%# Eval("ParameterID") %>' />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPartNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control" placeholder="Part Number" list="dlPartNo" onchange="PartNoChange(this);"></asp:TextBox>
                                        <datalist id="dlPartNo" runat="server" autopostback="true" clientidmode="AutoID"></datalist>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSerialNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control" placeholder="Serial Number" list="dlSerialNo" onchange="SerialNoChange(this);"></asp:TextBox>
                                        <datalist id="dlSerialNo" runat="server" autopostback="true" clientidmode="AutoID"></datalist>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Button runat="server" ID="btnSearch" ClientIDMode="Static" Text="SEARCH" CssClass="btn-style new-edit-btn" OnClick="btnSearch_Click"  />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                       <asp:Label runat="server" ID="lblMachineNo" Text='<%# Eval("FabricationNumber") %>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
            </asp:ListView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
      
    </div>
    <script>
        function PartNoChange(event) {
            if ($(event).val() != "") {
                $(event).closest('tr').find("#txtSerialNo").attr("disabled", "disabled");
                $(event).closest('tr').find("#txtSerialNo").val("");
            }
            else {
                $(event).closest('tr').find("#txtSerialNo").removeAttr('disabled');
            }
        }
        function SerialNoChange(event) {
            if ($(event).val() != "") {
                $(event).closest('tr').find("#txtPartNo").attr("disabled", "disabled");
                $(event).closest('tr').find("#txtPartNo").val("");
            }
            else {
                $(event).closest('tr').find("#txtPartNo").removeAttr('disabled');
            }
        }
    </script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
