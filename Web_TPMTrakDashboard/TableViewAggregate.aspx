<%@ Page Title="Historical Table View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TableViewAggregate.aspx.cs" Inherits="Web_TPMTrakDashboard.TableViewAggregate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <%: Styles.Render("~/bundles/tablecss") %>--%>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>


    <link href="Content/Ionic.css" rel="stylesheet" />
    <%--  <script src="shortingcssjs/js/jquery-1.10.2.min.js"></script>--%>
    <%--<link href="MyCssAndJS/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />--%>
    <%--<script src="MyCssAndJS/DatePicker/jquery-2.1.1.min.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/moment-with-locales.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"></script>--%>


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

        th.headerSortUp {
            background-image: url(Image/asc.gif);
            background-position: right center;
            background-repeat: no-repeat;
        }

        th.headerSortDown {
            background-image: url(Image/desc.gif);
            background-position: right center;
            background-repeat: no-repeat;
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

        /*#MainContent_gridviewTableData tbody td {
            background-color: white;
            color: black;
        }*/
        #MainContent_gridviewTableData tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        #MainContent_gridviewTableData tbody tr:nth-child(even) {
            background-color: white;
        }

        #gvPlantTableData tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        #gvPlantTableData tbody tr:nth-child(even) {
            background-color: white;
        }

        #gvCellTableData tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
        }

        #gvCellTableData tbody tr:nth-child(even) {
            background-color: white;
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

        /*ul .dropdown-menu{
            height: 300px;
            overflow-x: auto;
        }
       ul .multiselect-container{
            height: 300px;
            overflow-x: auto;
       }*/
        #tblfilter tr td {
            vertical-align: middle;
        }
        /*#MainContent_updateTableViewData td:nth-child(3) {
            text-decoration: underline;
            cursor: pointer;
            color: #547CFF;
        }*/
        .auto-style1 {
            font-weight: bold;
            color: white;
            height: 51px;
        }

        .auto-style2 {
            height: 51px;
        }

        #gvPlantTableData tr td:first-child {
            background-color: #0072c6;
        }

        #gvCellTableData tr td:first-child {
            background-color: #0072c6;
        }
    </style>
    <div class="container-fluid">
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:HiddenField ID="hdfMode" runat="server" />
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>
                <div class="row">
                    <table id="tblfilter" class="table table-bordered" style="display: inline-block; width: auto">
                        <tr>
                            <td class="commanTd" style="min-width: 80px; height: 50px"><%=GetGlobalResourceObject("CommanResource","FromDate") %></td>
                            <td class="input-group" style="min-width: 60px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Style="width: 110px; min-height: 40px" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="min-width: 60px; height: 50px"><%=GetGlobalResourceObject("CommanResource","ToDate") %></td>
                            <td class="input-group" style="min-width: 60px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" runat="server" Style="width: 110px; min-height: 40px" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="width: 60px;"><%=GetGlobalResourceObject("CommanResource","Plant") %></td>
                            <td style="min-width: 180px;">
                                <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="commontd" style="width: 54px;"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                            <td style="min-width: 120px;">
                                <%--<asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control" meta:resourcekey="ddlCellIdResource1">
								</asp:DropDownList>--%>
                                <asp:ListBox runat="server" ID="lbCellID" CssClass="form-control" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="commontd" style="width: 54px;"><b>View</b></td>
                            <td style="min-width: 120px;">
                                <asp:DropDownList ID="ddlView" runat="server" CssClass="form-control" Style="background-color: #5bc0de; color: white;" meta:resourcekey="ddlCellIdResource1" OnSelectedIndexChanged="ddlView_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                    <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                    <asp:ListItem Value='Machinewise'>Machine View</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="min-width: 60px; height: 50px">Sort Order</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSortOrder" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                            </td>

                            <td>
                                <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" CssClass="btn btn-info btn-sm displayCss" ID="Button1" OnClick="btnProcess_Click1" OnClientClick="return showLoader();"></asp:Button>
                            </td>
                            <td id="tdBackBtn" runat="server" style="width: 120px; background-color: #e7e7e7">
                                <%-- <asp:Button runat="server" ID="btnBack" Visible="false" CssClass="btn btn-info btn-sm displayCss glyphicon glyphicon-chevron-left" OnClick="lbBackButton_Click" ></asp:Button>--%>
                                <asp:LinkButton runat="server" ID="lbBackButton" CssClass="	glyphicon glyphicon-chevron-left" Style="font-weight: bold; font-size: 16px; color: #115d9f; height: 30px" OnClick="lbBackButton_Click"></asp:LinkButton>
                            </td>
                            <td colspan="3">
                                <asp:LinkButton runat="server" ID="lnkSwitch" CssClass="btn btn-info btn-sm glyphicon glyphicon-random" Style="font-weight: bold; color: white; font-size: 15px; height: 30px" Text=" SwitchToIonic" OnClick="lnkSwitch_Click" OnClientClick="return showLoader();"></asp:LinkButton>
                            </td>
                        </tr>

                    </table>
                </div>
                <%--</ContentTemplate>
		</asp:UpdatePanel>--%>
                <asp:HiddenField runat="server" ID="hfPlantIdForBack" />
                <asp:HiddenField runat="server" ID="hdnCellPlantIDForBack" />
                <asp:HiddenField runat="server" ID="hdnCellIDForBack" />

                <div id="tblGrid" class="row" style="overflow-y: auto; padding-top: 0px;">
                    <%--	<asp:UpdatePanel ID="updateTableViewData" runat="server">
				<ContentTemplate>--%>

                    <asp:GridView ID="gridviewTableData" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered cockpit headerFixerTable" OnRowDataBound="gridviewTableData_RowDataBound" meta:resourcekey="gridviewTableDataResource1">
                        <Columns>
                            <asp:TemplateField HeaderText="Machine Id" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <%--<span style="white-space: nowrap"><%#Eval("MachineId")%></span>--%>
                                    <asp:Button runat="server" ID="machineId" Text='<%#Eval("MachineId")%>' Style="background-color: unset; border: unset" CommandName="Update" />
                                    <asp:HiddenField runat="server" ID="hfPlantId" Value='<%#Eval("Plantid")%>' />
                                    <asp:HiddenField runat="server" ID="hfCellId" Value='<%#Eval("Groupid")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>


                    <asp:GridView ID="gvPlantTableData" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered cockpit headerFixerTable" ClientIDMode="Static" OnRowUpdating="gvPlantTableData_RowUpdating" OnRowDataBound="gvPlantTableData_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Plant Id" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <%--<span style="white-space: nowrap"><%#Eval("Plantid")%></span>--%>
                                    <asp:Button runat="server" ID="plantID" Text='<%#Eval("Plantid")%>' Style="background-color: unset; border: unset; color: white" CommandName="Update" />
                                    <asp:HiddenField runat="server" ID="hfPlantId" Value='<%#Eval("Plantid")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>


                    <asp:GridView ID="gvCellTableData" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-bordered cockpit headerFixerTable" ClientIDMode="Static" OnRowUpdating="gvCellTableData_RowUpdating" OnRowDataBound="gvCellTableData_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Group Id">
                                <ItemTemplate>
                                    <%--<span style="white-space: nowrap"><%#Eval("Groupid")%></span>--%>
                                    <asp:Button runat="server" ID="groupId" Text='<%#Eval("Groupid")%>' Style="background-color: unset; border: unset; color: white" CommandName="Update" />
                                    <asp:HiddenField runat="server" ID="hdnCellPlantID" Value='<%#Eval("Plantid")%>' />
                                    <asp:HiddenField runat="server" ID="hfCellId" Value='<%#Eval("Groupid")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </div>
	<%--<script src="shortingcssjs/js/jquery.tablesorter.min.js"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $.unblockUI({});

            var winHeight = $(window).height();
            winHeight = screen.availHeight;
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (640);
                console.log('min');
            } else {
                //winHeight = (840);
                console.log('max');
            }
            $('[id$=lbCellID]').multiselect({
                includeSelectAllOption: true
            });
            $("#tblGrid").height(winHeight - 261);

            dateTimePicker();
            $("[id$=chkAutoBox]").click(function () {
                $("[id$=btnTrigger]").trigger("click");
                // return false;
            });
            $('[data-toggle="tooltip"]').tooltip();
            $("[id$=btnProcess]").click(function () {
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

                <%--if (Date.parse(from) > Date.parse(to)) {
                    alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                    $('[id$=txtToDate]').val('');
                    $('[id$=txtToDate]').focus();
                    return false;
                }--%>
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            //var newWidth = ($(window).width() - 180);
            //$("#tblGrid").width(newWidth);

            $("[id$=ddlDayShift]").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            $("#toggle").click(function () {
                newWidth = $(window).width();
                $("#tblGrid").width(newWidth);
            });
            //$("#liMenu").click(function () {
            //    newWidth = $(window).width();
            //    var widthMenu = $("#sidebar").width();
            //    if (widthMenu == 180) {
            //        $("#tblGrid").width($(window).width() - 46);
            //    } else {
            //        $("#tblGrid").width($(window).width() - 180);
            //    }
            //});
            // $("[id$=gridviewTableData]").tablesorter();
        });
        $(document).on("click", ".machineClick", function () {
            window.open("VDGScreen.aspx?machineId=" + $(this).html() + "&shiftId=" + $("[id$=ddlShift]").val() + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val() + "&Page=table", "VDGScreen.aspx");
        });
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        $(document).on("click", ".hypercol", function () {
            var machine = $(this).closest('td').prev('td').prev('td').prev('td').text().trim();
            //alert($(this).closest('td').prev('td').text().trim());
            window.open("oeeGraphics.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val());
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

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            var winHeight = $(window).height();
            winHeight = screen.availHeight;
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (640);
                console.log('min');
            } else {
                //winHeight = (840);
                console.log('max');
            }
            $('[id$=lbCellID]').multiselect({
                includeSelectAllOption: true
            });
            $("#tblGrid").height(winHeight - 261);

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
            $("[id$=chkAutoBox]").click(function () {
                $("[id$=btnTrigger]").trigger("click");
                //return false;
            });
            $('[data-toggle="tooltip"]').tooltip();
            $("[id$=ddlDayShift]").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            $("[id$=btnProcess]").click(function () {
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

                <%--if (Date.parse(from) > Date.parse(to)) {
                    alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                    $('[id$=txtToDate]').val('');
                    $('[id$=txtToDate]').focus();
                    return false;
                }--%>
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
            // $("[id$=gridviewTableData]").tablesorter();                
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
