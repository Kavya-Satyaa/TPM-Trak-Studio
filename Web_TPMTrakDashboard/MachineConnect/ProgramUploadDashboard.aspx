<%@ Page Title="Program Upload History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProgramUploadDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineConnect.ProgramUploadDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .table tbody>tr>td
        {
          border-top:none;
        }
        .input-group .multiselect
        {
                border-radius: 4px 0px 0px 4px;
                width:185px;
        }
        .multiselect
        {
               width:287px;
        }
        .multiselect-container
        {
            height:16em;
            overflow:auto;
        }
    </style>
       <asp:UpdatePanel runat="server">
         <ContentTemplate>
            <div>
                <table class="table" style="width:auto">
                    <tr>
                       <td class="commanTd" style="vertical-align: middle;">Machine</td>
                        <td>
                            <asp:ListBox ID="lbMachineID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control" ></asp:ListBox>
                        </td>
                        <td class="commanTd" style="vertical-align:middle;">From Date</td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static" Height="40" Width="200"></asp:TextBox>
                            </div>
                        </td>
                        <td class="commanTd" style="vertical-align:middle;">To Date</td>
                        <td>
                             <div class="input-group">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static" Height="40" Width="200"></asp:TextBox>
                            </div>
                        </td>
                        <td style="vertical-align:middle">
                             <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click"/>
                        </td>
                         <td style="vertical-align:middle">
                             <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="commanTd" style="vertical-align:middle">Program No</td>
                        <td>
                            <div class="input-group">
                                 <asp:ListBox ID="lbProgramNo" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control"></asp:ListBox>
                                 <div class="input-group-addon" style="width:85px;padding:0px;">
                                    <asp:LinkButton runat="server" ID="lnkRefresh" CssClass="btn btn-success btn-sm glyphicon glyphicon-refresh" Style="font-weight: bold; color: white; top:0px;font-size: 15px; height: 37px" Text=" Refresh" OnClick="btnRefresh_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </td>
                         <td style="vertical-align:middle">
                             <asp:Button ID="btnProgramView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnProgramExportView_Click"/>
                        </td>
                       
                    </tr>
                </table>
            </div>
            <div style="height:75vh;overflow:auto;">
                <asp:ListView runat="server" ID="lvProgramUpload" ClientIDMode="Static" >
                    <EmptyDataTemplate>
                         <table class="table table-bordered table-hover headerFixer alternate-table-style">
                            <tr>
                                <th>Machine</th>
                                <th>Uploaded Program No</th>
                                <th>Employee</th>
                                <th>Uploaded TS</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table class="table table-bordered table-hover headerFixer alternate-table-style">
                            <tr>
                                <th>Machine</th>
                                <th>Uploaded Program No</th>
                                <th>Employee</th>
                                <th>Uploaded TS</th>
                            </tr>
                            <tr runat="server" id="itemplaceholder"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("MachineID") %></td>
                            <td><%# Eval("ProgramNo") %></td>
                            <td><%# Eval("Employee") %></td>
                            <td><%# Eval("UpdatedTS") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
      
            </div>
          </ContentTemplate>
           <Triggers>
                 <asp:PostBackTrigger ControlID="btnView" />
                 <asp:PostBackTrigger ControlID="btnExport" />
                 <asp:PostBackTrigger ControlID="lnkRefresh" />
                 <asp:PostBackTrigger ControlID="btnProgramView" />
           </Triggers>
        </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function()
        {
            $('#lbMachineID').multiselect({
                includeSelectAllOption: true
            });
            $('#lbProgramNo').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: 'en-US'
            });
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $(document).ready(function () {
                $('#lbMachineID').multiselect({
                    includeSelectAllOption: true
                });
                $('#lbProgramNo').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=txtFromDate]').datetimepicker({
                    format: 'DD-MM-YYYY HH:mm:ss',
                    locale: 'en-US'
                });
                $('[id$=txtToDate]').datetimepicker({
                    format: 'DD-MM-YYYY HH:mm:ss',
                    locale: 'en-US'
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
