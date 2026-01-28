<%@ Page Title="AuditDate Scheduler" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuditDateScheduler.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.AuditDateScheduler" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

       <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
        <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <style>
        .headerFixerTable tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        /*.headerFixerTable > tbody > tr:last-child > td {
            position: sticky;
            bottom: 0px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }*/

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 27%;
            overflow: auto;
            margin-top: 15px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
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
            padding-top: 15px;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>
            <table id="tblfilter" class="table table-bordered" style="width: auto;">
                <tr>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Year</td>
                    <td>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                    </td>
                    <td class="commanTd" style="width: auto;">
                          <asp:Button runat="server" Text="Populate Audit Date" CssClass="btn btn-info btn-sm displayCss" ID="btnGenerateAuditDate" OnClick="btnGenerateAuditDate_Click" ></asp:Button>
                        </td>
                    <td class="commanTd" style="width: auto;">
                          
                        <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click" ></asp:Button>
                    </td>
                    <tr>
            </table>
            <div id="gridContainer" class="divGrid">
                <asp:GridView runat="server" ID="gvAduitDateDetails" AutoGenerateColumns="False" ClientIDMode="Static"
                    CssClass="table table-bordered cockpit headerFixerTable " ShowHeaderWhenEmpty="true" >
                    <Columns>
                        <asp:TemplateField HeaderText="Audit Date">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPMActivity" Text='<%#Eval("AuditDate") %>' ForeColor="White" />
                            </ItemTemplate>
                                </asp:TemplateField>
                   <%--       <asp:TemplateField HeaderText="Day">
                             <ItemTemplate>
                                <asp:Label runat="server" ID="lblPMActivity" Text='<%#Eval("Day") %>' ForeColor="White" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <HeaderStyle CssClass="HeaderCss" HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                        <div style="height: 100%; background-color: white; text-align: center; color: red">No Data Found</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>

     <div class="modal fade" id="ConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span style="color: black">Audit Date already generated for selected year. Do you want regenerate?</span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <asp:Button runat="server" Text="Yes" ID="btnRegenerateAuditDate" Style="width: 80px;" OnClick="btnRegenerateAuditDate_Click" />
              <input type="button" Style="width: 80px;" value="No" onclick="closeDeletePopup();" />
                </div>
            </div>
        </div>
    </div>
    <script>
        
        $(document).ready(function () {
            var Height = $(window).height() - (190);
            $('#gridContainer').css('height', Height);
        });
         function closeDeletePopup() {
            $('[id*=ConfirmModal]').modal('hide');
        }
         function openConfirmModal() {
            $('[id*=ConfirmModal]').modal('show');
            return false;
        }
        $('[id$=txtYear]').datepicker({
            minViewMode: 2,
            format: 'yyyy',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });

        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("lblMessages").style.display = "none";
            }, 2000);

        };
        $(window).resize(function () {
            var Height = $(window).height() - (190);
            $('#gridContainer').css('height', Height);
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                var Height = $(window).height() - (190);
                $('#gridContainer').css('height', Height);
            });
            $(window).resize(function () {
                var Height = $(window).height() - (190);
                $('#gridContainer').css('height', Height);
            });
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
