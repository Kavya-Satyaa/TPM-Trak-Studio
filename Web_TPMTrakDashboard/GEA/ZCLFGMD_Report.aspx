<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ZCLFGMD_Report.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.ZCLFGMD_Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
         .table-style tr td {
                padding: 12px 10px;
                border: none;
                border-bottom: 0.5px solid #e3e6f0;
                border-right: 0.5px solid #e3e6f0;
                border-left: 0.5px solid #e3e6f0;
                text-align: left;
                color:white;
            }

            .table-style tr:hover td {
                background-color:  #f0f0f042;
            }

            .table-style  {
                border: unset !important;
            }
             .table-style {
                width: 50%;
                box-shadow: 0 .15rem 1.75rem 0 rgba(58,59,69,.15);
            }

            .table-style tr th, .table-header-fixer-for-dynamic-columns > tbody > tr:first-child td {
                padding: 9px 10px;
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
    <div style="margin-bottom: 9px;color:white">
         <h3>ZCLFGMD Report</h3>
    </div>
     <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
           <%-- <div class="row" style="margin:0px;">--%>
               <div>
                 <table class="table table-bordered" id="tblmenu" style="width: 40%;margin-top: 12px;">
                    <tr>
                        <td style="width: 180px; font-size: 16px; font-family: 'Segoe UI'; font-weight: 600;">
                            <div style="margin-top: 5px;" class="commontd">
                               <span>Fabrication Number</span>&nbsp;</div>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFabNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control" placeholder="Fabrication Number" list="dlFabNo"></asp:TextBox>
                            <datalist id="dlFabNo" runat="server" autopostback="true" clientidmode="Static" class="scrollable"></datalist>
                        </td>
                        <td>
                            <div style="float: left">
                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,View %>" class="btn btn-info" ID="btnView" OnClick="btnView_Click" ></asp:Button>
                                <asp:Button runat="server" Text="<%$ Resources:CommanResource,Export %>" class="btn btn-info" ID="btnExport" OnClick="btnExport_Click"></asp:Button>
                            </div>
                        </td>
                    </tr>
                  </table>
                  <div style="margin-bottom: 10px;">
                     <asp:Label runat="server" ID="lblFabNo" style="color:white;"></asp:Label>
                 </div>
                </div>
           
                 <div id="scrollMaintainDiv" style="overflow: auto; height: 74vh">
                      <asp:ListView runat="server" ID="lvZCLFGMDGridDetails" ClientIDMode="Static">
                            <EmptyDataTemplate>
                                <table class="table-style" id="tblZCLFGMDDetails" >
                                    <tr>
                                        <th>Description</th>
                                        <th>Data</th>
                                    </tr>
                                    <tr>
                                        <td colspan="100" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table class="table-style search-tbl" id="tblZCLFGMDDetails">
                                    <tr>
                                       <th>Description</th>
                                       <th>Data</th>
                                    </tr>
                                    <tr id="itemplaceholder" runat="server"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblDesc" Text='<%# Eval("Parameterdescription") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblData" Text='<%# Eval("ParameterValue") %>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                   </asp:ListView>
                </div>
            <%--</div>--%>
       </ContentTemplate>
         <Triggers>
             <asp:PostBackTrigger ControlID="btnExport" />
         </Triggers>
    </asp:UpdatePanel>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
