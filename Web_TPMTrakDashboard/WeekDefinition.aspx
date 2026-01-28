<%@ Page Title="Week Definition" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WeekDefinition.aspx.cs" Inherits="Web_TPMTrakDashboard.WeekDefinition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="display:flex;justify-content:center;align-content:center;">
                     <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center;width:600px;word-wrap:break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
                </div>
                <div style="display:flex;justify-content:center;align-content:center;">
                    
                        <table id="tblfilter" class="table table-bordered" style="width: auto;">
                            <tr>
                                <td class="commanTd" style="min-width: 50px; height: 50px">Year</td>
                                <td class="input-group" style="min-width: 60px; width: 130px;">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtYear" runat="server" Style="min-width: 100px; min-height: 40px; width: 100px;background-color:white;" CssClass="form-control date1" placeholder="Year Start" AutoCompleteType="Disabled" ></asp:TextBox>
                                </td>

                                <td class="commanTd" style="width: 150px;">Starting Day of Week</td>
                                <td style="min-width: 120px;">
                                    <asp:DropDownList ID="ddlStartingDayOfWeek" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Sunday" Value="0" />
                                        <asp:ListItem Text="Monday" Value="1" />
                                        <asp:ListItem Text="Tuesday" Value="2" />
                                        <asp:ListItem Text="Wednesday" Value="3" />
                                        <asp:ListItem Text="Thursday" Value="4" />
                                        <asp:ListItem Text="Friday" Value="5" />
                                        <asp:listitem text="saturday" value="6" />
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: center; width: auto;">
                                    <asp:Button runat="server" Text="Generate" CssClass="btn btn-info btn-sm displayCss" ID="btnWeekGenerate" OnClick="btnWeekGenerate_Click"></asp:Button>
                                </td>
                                <td style="text-align: center; width: auto;">
                                    <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnWeekView" OnClick="btnWeekView_Click"></asp:Button>
                                </td>
                            </tr>

                        </table>
                </div>
                <div style="display:flex;justify-content:center;align-content:center;">
                    <div id="divWeekDefinition" style="width:50%;overflow:auto;min-height: 750px; border: 1px solid silver;">
                        <asp:GridView runat="server" ID="gvWeekDefinition" AutoGenerateColumns="False"
						CssClass="table table-bordered cockpit headerFixerTable" ShowHeaderWhenEmpty="true" >
                        <Columns>
                            <asp:TemplateField HeaderText="Week Date">
								<ItemTemplate>
									<span style="white-space: nowrap;color:white;"><%#Eval("WeekDate")%></span>
								</ItemTemplate>
							</asp:TemplateField>
                            <asp:TemplateField HeaderText="Week Number">
								<ItemTemplate>
									<span style="white-space: nowrap;color:white;"><%#Eval("WeekNumber")%></span>
								</ItemTemplate>
							</asp:TemplateField>
                            <asp:TemplateField HeaderText="Month Val">
								<ItemTemplate>
									<span style="white-space: nowrap;color:white;"><%#Eval("MonthVal")%></span>
								</ItemTemplate>
							</asp:TemplateField>
                            <asp:TemplateField HeaderText="Year Number">
								<ItemTemplate>
									<span style="white-space: nowrap;color:white;"><%#Eval("YearNo")%></span>
								</ItemTemplate>
							</asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                        <EmptyDataTemplate>
                              <div style="height: 100%; background-color: white;text-align:center;color:red">No Week Information Available</div>
                         </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <style type="text/css">
        .headerFixerTable tr th {
            position: sticky;
            top: -10px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        th {
            cursor: pointer;
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
            height: 60px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        a {
            color: black;
        }

        .table .lbl {
            padding-top: 15px;
        }

        #MainContent_updateTableViewData {
            margin-top: -15px;
        }

        .machineClick {
            text-decoration: underline;
            cursor: pointer;
        }

        th[data-content='OEE'] td {
            text-decoration: underline;
            cursor: pointer;
        }

        .hypercol {
            text-decoration: underline;
            cursor: pointer;
        }

        .GridHeader {
            text-align: center !important;
        }
        #tblfilter tr td {
            vertical-align: middle;
        }        
    </style>
    <script>
        function HideLabel() {
            var seconds = 2;
            setTimeout(function () {
                document.getElementById("<%=lblMessages.ClientID %>").style.display = "none";
                }, seconds * 1000);
        };
        function resize() {
            var heights = window.innerHeight-180;
            document.getElementById("divWeekDefinition").style.height = heights + "px";
        }
        $(document).ready(function () {
            $.unblockUI({});
            resize();
            window.onresize = function () {
                resize();
            };
            //HideLabel();
            //document.getElementById("divWeekDefinition").style.height = screen.availHeight;
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYear]').keypress(function (e) {
                return false;
            });
            $('[id$=txtYear]').keydown(function (e) {
                return false;
            });
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            resize();
            window.onresize = function () {
                resize();
            };
            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=txtYear]').keypress(function (e) {
                return false;
            });
            $('[id$=txtYear]').keydown(function (e) {
                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
