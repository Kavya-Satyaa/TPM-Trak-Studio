<%@ Page Title="Help Request Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HelpRequestReport.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.HelpRequestReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style type="text/css">
        .headerFixerTable tr th {
            position: sticky;
            top: 0px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 100%;
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

        #gvAvgCallTypeDetails td:first-child{
              color: white;
            /*background-color: #2E6886 !important;*/
            font-weight: bold;
        }
          #gvAvgCallTypeDetails tr:first-child th{
              color: white;
              font-weight: bold;
                background-color: #2E6886 !important;
          }
           #gvAvgCallTypeDetails tr:first-child th:first-child{
               color:#202648;
                 background-color: unset !important;
           }
        #gvAvgCallTypeDetails  td {
            color: white;
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

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>


            <table id="tblfilter" class="table table-bordered" style="width: auto;">
                <tr>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Plant</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" Width="240" Style="height: 40px" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                   <%-- <td class="commanTd" style="min-width: 50px; height: 50px">Group</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlGroup" CssClass="form-control" Width="240" Style="height: 40px" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>--%>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Machine</td>
                    <td style="min-width: 160px;">
                        <asp:ListBox ID="ddlMachineId" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource, MachineTooltip %> " Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>

                    <td class="commanTd" style="width: auto;">From Date </td>
                    <td class="input-group" style="min-width: 150px;">

                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>

                    </td>
                    <td class="commanTd" style="width: auto;">To Date </td>
                    <td class="input-group" style="min-width: 150px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Shift</td>
                    <td style="min-width: 160px;">
                        <asp:ListBox ID="ddlShift" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource,ShiftTooltip %>" Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Help Request</td>
                    <td style="min-width: 160px;">
                        <asp:ListBox ID="ddlHelpRequest" runat="server" SelectionMode="Multiple" ToolTip="Help Request" Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td style="text-align: center; width: auto;" colspan="2">
                        <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>
                        <asp:Button runat="server" Text="Export" CssClass="btn btn-info btn-sm displayCss" ID="btnExport" OnClick="btnExport_Click"></asp:Button>
                    </td>

                </tr>
            </table>
            <div style="margin-top: 10px">
                <asp:GridView runat="server" ID="gvAvgCallTypeDetails" ClientIDMode="Static" CssClass="table table-bordered" AutoGenerateColumns="true"></asp:GridView>
            </div>
            <div style="display: flex; justify-content: center; align-content: center;">
                <div id="gridContainer" class="divGrid" style="">
                    <asp:GridView runat="server" ID="gvHelpRequestDetails" AutoGenerateColumns="False" ClientIDMode="Static"
                        CssClass="table table-bordered cockpit headerFixerTable" ShowHeaderWhenEmpty="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDate" Text='<%#Eval("ShiftDate")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Plant ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("PlantID")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("MachineID")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shift">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("ShiftName")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Request Type">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("RequestType")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requested Time">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("RequestedTime")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ack. Time">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("AckTime")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reset Time">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("ResetTime")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ack. by Operator Time">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("AckOperatorTime")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ack. Time from Trigger (min)">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("AckTimeFromTrigger")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reset Time from Trigger (min)">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("ResetTimeFRomTrigger")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ack. by Operator from Trigger (min)">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%#Eval("AckOperatorTimeFromTrigger")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <div style="height: 100%; background-color: white; text-align: center; color: red">No Data Found</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">

        $(document).ready(function () {
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlShift]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlHelpRequest]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: 'en-US'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            var Height = $(window).height() - (220+150);
            $('#gridContainer').css('height', Height);
        });
        $(window).resize(function () {
            var Height = $(window).height() - (220+150);
            $('#gridContainer').css('height', Height);
        });

        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("lblMessages").style.display = "none";
            }, 2000);

        };
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                $('[id$=ddlMachineId]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=ddlShift]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=ddlHelpRequest]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=txtFromDate]').datetimepicker({
                    format: 'DD-MM-YYYY',
                    locale: 'en-US'
                });
                $('[id$=txtToDate]').datetimepicker({
                    format: 'DD-MM-YYYY',
                    useCurrent: false,
                    locale: 'en-US'
                });
                $("[id$=txtFromDate]").on("dp.change", function (e) {
                    $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
                });
                $("[id$=txtToDate]").on("dp.change", function (e) {
                    $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
                });

                var Height = $(window).height() - (220+150);
                $('#gridContainer').css('height', Height);
            });
            $(window).resize(function () {
                var Height = $(window).height() - (220+150);
                $('#gridContainer').css('height', Height);
            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
