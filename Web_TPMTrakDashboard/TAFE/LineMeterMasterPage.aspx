<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LineMeterMasterPage.aspx.cs" Inherits="Web_TPMTrakDashboard.LineMeterMasterPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <script src="/Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="/Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <style>
        .label {
            color: white;
            text-align: center;
            font-size: medium;
            font-weight: bold;
        }

        .button {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            width: 80px;
            height: 30px;
            margin-left: 30px;
        }

        .grid {
            margin: 15px;
            display: grid;
            text-align: center;
        }

        .headergrid {
            background: #2E6886;
            color: white;
            text-align: center;
            height: 40px;
            font-weight: bold;
            font-size: 16px;
        }
        .Alternativecss{
            background: #DCDCDC;
            color: black;
            text-align: center;
            height: 30px;
            font-weight: bold;
            font-size: 12px;
        }
         .Rowcss{
            background: white;
            color: black;
            text-align: center;
            height: 30px;
            font-weight: bold;
            font-size: 12px;
        }
         th{
             text-align:center;
         }
         /*td{
             width:150px;
         }*/
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="width: 100%; height: 8vh; text-align: center;">
                    <table style="border-style: double;width:750px;text-align:left; height: 80%; padding: 5px;padding-right:10px">
                        <tr>
                            <td style="width:60px;"><span class="label">Line  </span></td>
                            <td style="width:210px;">
                                <asp:DropDownList runat="server" ID="ddlLineID" Style="width: 200px; height: 30px;" /></td>
                            <td style="width:65px;" runat="server"><b><span class="label">Date</span></b></td>
                            <td style="width:150px;" runat="server">
                                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                            </td>
                            <td style="width:90px;">
                                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="button" />
                            </td>
                            <td style="width:9  0px;">
                                <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grid headerFixer" style="height:75vh;overflow:auto;" >
                    <asp:GridView runat="server" ID="LineMeterMaster" AutoGenerateColumns="false" EmptyDataText="No Data Found For the Line and the Month"  ShowHeader="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDate" Text='<%# Bind("DStart", "{0:dd-MMM-yyyy }")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Line" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLine" Text='<%# Bind("Line")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Man Power"  ItemStyle-Width="250">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" CssClass="txtManpower" ID="txtManPower" Text='<%# Bind("Manpower") %>'  onkeypress="return event.charCode >= 48 && event.charCode <= 57 || event.charCode <= 46" />
                                    <asp:HiddenField runat="server" Value='<%# Bind("Manpower") %>' ID="hiddenfield1" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Loading Hours"  ItemStyle-Width="250">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" CssClass="txtLoadingHour" ID="txtLoadingHours" Text='<%# Bind("Loadinghrs") %>' onkeypress="return event.charCode >= 48 && event.charCode <= 57 || event.charCode <= 46" />
                                    <asp:HiddenField runat="server" Value='<%# Bind("Loadinghrs") %>' ID="hiddenfield2" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="headergrid" />
                        <AlternatingRowStyle CssClass="Alternativecss" />
                        <RowStyle CssClass="Rowcss" />
                        <EmptyDataTemplate>
                            <span>No Data Found</span>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        });

        function messageOk(msg) {
            
            Command: toastr["success"](msg)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-center",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

        });

      


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
