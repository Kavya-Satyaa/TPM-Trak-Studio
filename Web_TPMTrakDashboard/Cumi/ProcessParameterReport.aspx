<%@ Page Title="ProcessParameterReport" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterReport.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.ProcessParameterReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
      .bajaj-table-style tr th
      {
          vertical-align:middle !important;
          text-align:center;
      }
       .headerFixer tr th {
            position: sticky;
            top: -1px;
            z-index: 4;
            background-color: #2e6886;
            color: white;
        }
       .headerFixer tr:nth-child(2) th{
            top:36px;
       }
    </style>
     <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
     <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="bajaj-outer-div-filter-section">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                    <table class="bajaj-filter-tbl" style="box-shadow:0 4px 8px 0 rgb(0 0 0 / 25%), 0 6px 20px 0 rgb(0 0 0 / 19%);">
                        <tr>
                            <td>Date</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDate" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td>Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"  ></asp:DropDownList>
                            </td>
                            <td>Machine</td>
                            <td>
                                 <asp:DropDownList ID="ddlMachineId" runat="server"  CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Shift</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlshift" CssClass="form-control" ></asp:DropDownList>
                            </td>
                            <td colspan="4">
                                <asp:Button runat="server" ID="btnView" Text="View"  CssClass="bajaj-btn-style" OnClick="btnView_Click" OnClientClick="return showLoader();" style="margin-right:10px;"/>
                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="bajaj-btn-style" Style="background-color: #9f0e9f; color: white" OnClick="btnExport_Click" />
                            </td>
                           
                        </tr>
                    </table>
                </div>
            </div>
             <div id="scrollMaintainDiv" style="height: 75vh; overflow: auto;margin-top:5px; width: 100%; display: inline-block">
                <asp:ListView runat="server" ID="lvProcessParameterReport" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                <EmptyDataTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblProcessParameterDetails">
                            <tr>
                                <th rowspan="2">Sl No</th>
                                <th rowspan="2">Item Code</th>
                                <th rowspan="2">PO Number</th>
                                <th rowspan="2">Employee ID</th>
                                <th rowspan="2">Cycle Start</th>
                                <th rowspan="2">Cycle End</th>
                                <th rowspan="2">Machining Time</th>
                                <th rowspan="2">Load Unload Time</th>
                                <th colspan="2">Hydraulic</th>                              
                                <th rowspan="2">Hydraulic Temp (deg C)</th>
                                <th rowspan="2">Top Ram Stroke (mm)</th>
                                <th rowspan="2">Bottom Ram Stroke (mm)</th>
                            </tr>
                            <tr>
                                <th>Top Pressure (Bar)</th>
                                <th>Bottom Pressure (Bar)</th>
                            </tr>
                            <tr>
                                <td colspan="100" class="no-data-found-td"><span class="no-data-found" style="color:black">No Data Found</span></td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblProcessParameterDetails" >
                             <tr>
                               <th rowspan="2">Sl No</th>
                                <th rowspan="2">Item Code</th>
                                <th rowspan="2">PO Number</th>
                                <th rowspan="2">Employee ID</th>
                                <th rowspan="2">Cycle Start</th>
                                <th rowspan="2">Cycle End</th>
                                <th rowspan="2">Machining Time</th>
                                <th rowspan="2">Load Unload Time</th>
                                <th colspan="2">Hydraulic</th>                              
                                <th rowspan="2">Hydraulic Temp (deg C)</th>
                                <th rowspan="2">Top Ram Stroke (mm)</th>
                                <th rowspan="2">Bottom Ram Stroke (mm)</th>
                            </tr>
                            <tr>
                               <th>Top Pressure (Bar)</th>
                               <th>Bottom Pressure (Bar)</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblSlno" ClientIDMode="Static" Text='<%# Eval("Slno") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblProduct" ClientIDMode="Static" Text='<%# Eval("Product") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblPONumber" ClientIDMode="Static" Text='<%# Eval("PONumber") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblEmployeeID" ClientIDMode="Static" Text='<%# Eval("EmployeeID") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCycleStart" ClientIDMode="Static" Text='<%# Eval("StartDate") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCycleEnd" ClientIDMode="Static" Text='<%# Eval("EndDate") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCycleTime" ClientIDMode="Static" Text='<%# Eval("MachiningTime") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblLoadUnload" ClientIDMode="Static" Text='<%# Eval("LoadUnload") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblHydraulicPressureTop" ClientIDMode="Static" Text='<%# Eval("HydraulicPressure_Top") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblHydraulicPressureBottom" ClientIDMode="Static" Text='<%# Eval("HydraulicPressure_Bottom") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblHydraulicTemp" ClientIDMode="Static" Text='<%# Eval("HydraulicTemp") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblTopRamStroke" ClientIDMode="Static" Text='<%# Eval("TopRamStroke") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBottomRamStroke" ClientIDMode="Static" Text='<%# Eval("BottomRamStroke") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            </ContentTemplate>
           <Triggers>
              <asp:PostBackTrigger ControlID="btnExport" />
           </Triggers>
         </asp:UpdatePanel>
     <script type="text/javascript">
         $(document).ready(function () {
             $('[id$=txtDate]').datepicker({
                 viewMode: "date",
                 minViewMode: "date",
                 format: 'dd-mm-yyyy',
                 todayHighlight: true,
                 autoclose: true,
                 language: 'en-US',
             });
             $.unblockUI({});
         });
         
         function showLoader() {
             $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
             return true;
         }
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
             $(document).ready(function () {
                 $('[id$=txtDate]').datepicker({
                     viewMode: "date",
                     minViewMode: "date",
                     format: 'dd-mm-yyyy',
                     todayHighlight: true,
                     autoclose: true,
                     language: 'en-US',
                 });
                 $.unblockUI({});
             });
         });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
