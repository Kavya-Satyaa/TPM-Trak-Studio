<%@ Page Title="Machine History" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TafeMachineHistory.aspx.cs" Culture="auto" UICulture="auto" Inherits="Web_TPMTrakDashboard.TAFE.TafeMachineHistory" %>

<asp:Content ID="MainCotentArea" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        .headerFixerTable tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
            min-width: 150px;
            text-align: center;
        }

        th {
            cursor: pointer;
        }

        ::-webkit-scrollbar {
            width: 7px;
        }

        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 5px;
        }

        .table tbody > tr > th {
            vertical-align: middle;
        }

        .table > tr > td {
            vertical-align: middle;
            height: 35px;
        }
        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 10px;
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
            height: 50px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        #MainContent_gridviewTableData tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        #MainContent_gridviewTableData tbody tr:nth-child(even) {
            background-color: white;
        }

        a {
            color: black;
        }

        table tbody tr td {
            text-align: center;
            vertical-align: middle;
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

        .Running {
            -webkit-animation: cog-rotate 2s linear infinite;
            -moz-animation: cog-rotate 2s linear infinite;
            -o-animation: cog-rotate 2s linear infinite;
            animation: rotate 2s linear infinite;
            color: green;
        }

        .Stopped {
            color: red;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .auto-style1 {
            font-weight: bold;
            color: white;
            height: 51px;
        }

        .auto-style2 {
            height: 51px;
        }
    </style>

    <div class="container-fluid">
        <asp:UpdatePanel ID="update" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>

                <div class="row" style="text-align: center; color: red;">
                    <asp:HiddenField ID="hdfMode" runat="server" />
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>
                <div class="row">
                    <table id="tblfilter" class="table table-bordered" style="display: inline-block">
                        <tr>
                            <td class="commanTd" style="min-width: 80px; height: 50px"><%=GetGlobalResourceObject("CommanResource","FromDate") %></td>
                            <td class="input-group" style="min-width: 60px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Style="min-width: 130px; min-height: 40px" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="min-width: 60px; height: 50px"><%=GetGlobalResourceObject("CommanResource","ToDate") %></td>
                            <td class="input-group" style="min-width: 60px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" Style="min-width: 130px; min-height: 40px" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="width: 60px;"><%=GetGlobalResourceObject("CommanResource","Machine") %></td>
                            <td style="min-width: 180px;">
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" Text="<%$Resources:CommanResource, View %>" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info btn-sm displayCss" OnClick="btnSave_Click" Width="80" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info btn-sm displayCss" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>


                <div id="tblGrid" class="row" style="overflow-y: auto; padding-top: 0px;">
                    <asp:GridView runat="server" ID="gvMacHistory" AutoGenerateColumns="false" CssClass="table table-bordered cockpit headerFixerTable" AllowPaging="false" ShowHeader="true" ShowFooter="false" ShowHeaderWhenEmpty="true" OnRowDataBound="gvMacHistory_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Machine Id">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfUpate" runat="server" />
                                    <asp:Label runat="server" ID="lblMacID" Text='<%# Eval("MachineID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BreakDown Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblDownCode" runat="server" Text='<%# Eval("DownCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Kind Of Problem">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdfKindOfProblem" Value='<%#Eval("KindOfProblem") %>' />
                                    <asp:DropDownList runat="server" ID="ddlKindOfProblem" CssClass="form-control ddnUpdate">
                                        <asp:ListItem Value="" Text="" />
                                        <asp:ListItem Value="Break Down" Text="Break Down" />
                                        <asp:ListItem Value="Accident" Text="Accident" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Breakdown Category">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdfDownCategory" Value='<%#Eval("DownCategory") %>' />
                                    <asp:DropDownList runat="server" ID="ddlBreakdownCategory" CssClass="form-control ddnUpdate">
                                        <asp:ListItem Value="" Text="" />
                                        <asp:ListItem Value="Electrical" Text="Electrical" />
                                        <asp:ListItem Value="Mechanical" Text="Mechanical" />
                                        <asp:ListItem Value="Fixture" Text="Fixture" />
                                        <asp:ListItem Value="Sub-System/Accessories" Text="Sub-System/Accessories" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Problem Reason">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtReason" Width="100%" CssClass="form-control txtupdate" Text='<%# Eval("Reason") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Breakdown Start">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDateTime" runat="server" Text='<%#Eval("BreakDownStart") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Breakdown End">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDateTime" runat="server" Text='<%# Eval("BreakDownEnd") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Resolve Action">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtActionToResolve" Width="100%" CssClass="form-control txtupdate" Text='<%# Eval("ActionToResolve") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Proposed Action">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtActionProposed" Width="100%" CssClass="form-control txtupdate" Text='<%# Eval("ActionProposed") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Time Lost(mins)">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTimeLost" Text='<%# Eval("TimeLost") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Elapsed Time(mins)">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblElapsedTime" Text='<%# Eval("ElapsedTime") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Severity">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdfSeverity" Value='<%#Eval("Severity") %>' />
                                    <asp:DropDownList runat="server" ID="ddlSeverity" CssClass="form-control ddnUpdate">
                                        <asp:ListItem Value="" Text="" />
                                        <asp:ListItem Value="*" Text="*" />
                                        <asp:ListItem Value="**" Text="**" />
                                        <asp:ListItem Value="***" Text="***" />
                                        <asp:ListItem Value="****" Text="****" />
                                        <asp:ListItem Value="*****" Text="*****" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No records found.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="HeaderCss" />
                        <RowStyle BackColor="White" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="#DCDCDC" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $.unblockUI({});

            var winHeight = $(window).height();
            var winWidth = $(window).width();
            winHeight = screen.availHeight;
            winWidth = screen.availWidth;
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (640);
                console.log('min');
            } else {
                console.log('max');
            }

            $("#tblGrid").height(winHeight - 261);
            $('[id$=gvMacHistory]').width(winWidth + 300);
            dateTimePicker();
            $('[data-toggle="tooltip"]').tooltip();
            $("[id$=btnView]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });

            $("#toggle").click(function () {
                
                newWidth = $(window).width();
                $("#tblGrid").width(newWidth);
            });
        });

        function dateTimePicker() {
            $('.date1').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('.date2').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $(".date1").on("dp.change", function (e) {
                $('.date2').data("DateTimePicker").minDate(e.date);
            });
            $(".date2").on("dp.change", function (e) {
                $('.date1').data("DateTimePicker").maxDate(e.date);
            });
        }

        $("#MainContent_gvMacHistory").on("change", ".ddnUpdate", function () {
            $(this).closest('tr').find('input[type=hidden]').first().val("update")
        });

        $("#MainContent_gvMacHistory").on("click", ".txtupdate", function () {
            $(this).closest('tr').find('input[type=hidden]').first().val("update")
        });

        function messageOk() {
            
            Command: toastr["success"]("Report Generated")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function messageNotOk() {
            
            Command: toastr["error"]("Error. Try Again!")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
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

        function messageSaveSuccess() {
            
            Command: toastr["error"]("Data saved successfully.")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
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

        function messageNodata() {
            
            Command: toastr["error"]("Try Again!No Data Found.")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
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
            $.unblockUI({});
            var winHeight = $(window).height();
            var winWidth = $(window).width();
            winHeight = screen.availHeight;
            winWidth = screen.availWidth;
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (640);
                console.log('min');
            } else {
                console.log('max');
            }

            $("#tblGrid").height(winHeight - 261);
            $('[id$=gvMacHistory]').width(winWidth + 300);
            dateTimePicker();
            function dateTimePicker() {
                $('.date1').datetimepicker({
                    format: 'DD-MM-YYYY',
                    locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
                });
                $('.date2').datetimepicker({
                    format: 'DD-MM-YYYY',
                    useCurrent: false,
                    locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
                });
                $(".date1").on("dp.change", function (e) {
                    $('.date2').data("DateTimePicker").minDate(e.date);
                });
                $(".date2").on("dp.change", function (e) {
                    $('.date1').data("DateTimePicker").maxDate(e.date);
                });
            }
            $('[data-toggle="tooltip"]').tooltip();

            $("[id$=btnView]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });

            $("#MainContent_gvMacHistory").on("change", ".ddnUpdate", function () {
                $(this).closest('tr').find('input[type=hidden]').first().val("update")
            });

            $("#MainContent_gvMacHistory").on("click", ".txtupdate", function () {
                $(this).closest('tr').find('input[type=hidden]').first().val("update")
            });

            function messageOk() {
                
                Command: toastr["success"]("Report Generated")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }

            function messageNotOk() {
                
                Command: toastr["error"]("Error. Try Again!")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
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

            function messageSaveSuccess() {
                
                Command: toastr["error"]("Data saved successfully.")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
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

            function messageNodata() {
                
                Command: toastr["error"]("Try Again!No Data Found.")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
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
        });
    </script>
</asp:Content>

<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

