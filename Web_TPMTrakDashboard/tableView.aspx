<%@ Page Title="Table View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tableView.aspx.cs" Inherits="Web_TPMTrakDashboard.tableView" meta:resourcekey="PageResource1" %>

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
        th {
            cursor: pointer;
        }

        .headerFixerTable tr th {
            position: sticky;
            top: -10px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
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


        .table tr th a {
            color: white;
        }

        .table tbody > tr > th {
            vertical-align: middle;
        }

        .table tbody > tr > td {
            vertical-align: middle;
        }

        .table > tr > td {
            vertical-align: middle;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            z-index: 10;
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

        th {
            vertical-align: inherit;
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

        .Down {
            color: <%=machineStatusColors.ColorDown%>;
        }

        .ICD {
            color: <%=machineStatusColors.ColorICD%>;
        }

        .Alarm {
            color: <%=machineStatusColors.ColorAlarm%>;
        }

        .LoadUnload {
            color: <%=machineStatusColors.ColorLoadUnload%>;
        }

        .Disconnected {
            color: <%=machineStatusColors.ColorDisconnected%>;
        }

        .PowerOff {
            color: <%=machineStatusColors.ColorPowerOff%>;
        }

        /*.NoData {
            color: white;
        }*/
        .NoData {
            color: <%=machineStatusColors.NoData%>;
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

        .gridCSS tbody tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        .gridCSS tbody tr:nth-child(even) {
            background-color: white;
            color: black;
        }

        .plantCellGrid tr td:first-child {
            background-color: #39b3d7;
            padding: 0px;
        }

        .switch {
            position: relative;
            display: inline-block;
            vertical-align: middle;
            width: 50px;
            height: 30px;
            /*float: right;*/
            margin: 5px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 22px;
                width: 22px;
                left: 3px;
                bottom: 3px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(23px);
            -ms-transform: translateX(23px);
            transform: translateX(23px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 30px;
        }

            .slider.round:before {
                border-radius: 50%;
            }


            .cockpit{
                max-height: 80vh;
            }
    </style>
    <div class="container-fluid">
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hdnView" ClientIDMode="Static" />
                <div class="row" style="text-align: center; color: red;">
                    <asp:HiddenField ID="hdfMode" runat="server" />
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>
                <div class="row">
                    <table id="tblfilter" class="table table-bordered" > <%--style="width: 62%; margin-left: 20px; margin-bottom: 10px; box-shadow: 1px 1px 5px #5f5f5f;"--%>
                        <tr>
                            <td class="commanTd" style="min-width: 80px;"><%=GetGlobalResourceObject("CommanResource","FromDate") %></td>
                            <td style="min-width: 220px; width: 220px">
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                </div>
                            </td>
                            <td class="commanTd" style="min-width: 60px;"><%=GetGlobalResourceObject("CommanResource","ToDate") %></td>
                            <td style="min-width: 160px; width: 160px">
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtToDate" runat="server" Style="min-width: 160px;" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                                </div>
                            </td>
                            <td class="commanTd" style="width: 60px;"><%=GetGlobalResourceObject("CommanResource","Plant") %></td>
                            <td style="min-width: 180px;">
                                <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span style="color: white;"><b><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></b></span>
                                <label class="switch">
                                    <asp:CheckBox ID="chkAutoBox" runat="server" />
                                    <span class="slider round"></span>
                                </label>
                                <%--  <label>
                                    <span class="checkbox commanTd">
                                        <asp:CheckBox ID="chkAutoBox" runat="server" type="checkbox" /><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></span>
                                </label>--%>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkSwitch" CssClass="btn btn-info btn-sm glyphicon glyphicon-random" Style="font-weight: bold; color: white; font-size: 15px; height: 30px" Text=" SwitchToIonic" OnClick="lnkSwitch_Click" OnClientClick="return showLoader();"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="commontd" style="width: 54px;"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                            <td style="min-width: 160px;">
                                <asp:ListBox runat="server" ID="lbCellID" CssClass="form-control" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                            </td>
                            <td class="commanTd" style="min-width: 80px;"><%=GetGlobalResourceObject("CommanResource","PredefinedTime") %></td>
                            <td style="min-width: 220px;">
                                <asp:DropDownList ID="ddlDayShift" runat="server"
                                    CssClass="form-control displayCss" AutoPostBack="True" OnSelectedIndexChanged="ddlDayShift_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlView" runat="server" CssClass="form-control" ClientIDMode="Static" meta:resourcekey="ddlCellIdResource1" Style="background-color: #39b3d7; color: white; font-weight: bold; width: auto" OnSelectedIndexChanged="ddlView_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                    <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                    <asp:ListItem Value="Machinewise">Machine View</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="min-width: 60px; height: 50px">Sort Order</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSortOrder" ClientIDMode="Static" CssClass="form-control" Style="width: auto"></asp:DropDownList>
                            </td>

                            <td style="min-width: 60px; text-align: center;">
                                <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" CssClass="btn btn-info btn-sm displayCss" ID="btnProcess" OnClick="btnProcess_Click1" OnClientClick="return showLoader();"></asp:Button>
                            </td>
                            <td id="tdBackBtn" runat="server" style="width: 120px; background-color: #e7e7e7">
                                <asp:LinkButton runat="server" ID="lbBackButton" CssClass="	glyphicon glyphicon-chevron-left" Style="font-weight: bold; font-size: 16px; color: #115d9f; height: 30px" OnClick="lbBackButton_Click" OnClientClick="return showLoader();"></asp:LinkButton>
                            </td>

                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="tblGrid" class="row" style="overflow: auto; height: 80vh; padding-top: 0px;">
            <asp:UpdatePanel ID="updateTableViewData" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridviewTableData" runat="server" AutoGenerateColumns="False" AllowSorting="true" OnSorting="gridviewTableData_Sorting"
                        CssClass="table table-bordered cockpit headerFixerTable" OnRowDataBound="gridviewTableData_RowDataBound" meta:resourcekey="gridviewTableDataResource1">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl. No." HeaderStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSlNo" Text='<%# Eval("SlNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="TemplateFieldResource1" HeaderText="Status" HeaderStyle-Width="180">
                                <ItemTemplate>
                                    <div class="loaders-container1" style="float: left;" title='<%# Eval("MachineLiveStatus") %>'>
                                        <div class="la-cog la-2x" style="float: left; vertical-align: middle">
                                            <div class="<%# Eval("MachineLiveStatus") %>"></div>
                                        </div>
                                    </div>
                                    <div style="float: right;"><%# (string)Eval("MachineLiveStatus")=="NoData" ?"NoData" : Eval("MachineLiveStatus") %></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine Id" meta:resourcekey="TemplateFieldResource2" SortExpression="MachineID" HeaderStyle-CssClass="SortClick">
                                <ItemTemplate>
                                    <a class="machineClick"><%#Eval("MachineId")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>
                    <asp:GridView ID="gvPlantTableData" runat="server" AutoGenerateColumns="False" AllowSorting="true" OnSorting="gvPlantTableData_Sorting"
                        CssClass="table table-bordered cockpit headerFixerTable gridCSS plantCellGrid" OnRowDataBound="gvPlantTableData_RowDataBound" meta:resourcekey="gridviewTableDataResource1" OnRowUpdating="gvPlantTableData_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="Plant Id" SortExpression="PlantID">
                                <ItemTemplate>

                                    <asp:Button runat="server" ID="plantID" Text='<%#Eval("Plantid")%>' Style="background-color: unset; border: unset; color: white" CommandName="Update" OnClientClick="return showLoader();" />
                                    <asp:HiddenField runat="server" ID="hfPlantId" Value='<%#Eval("Plantid")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>
                    <asp:GridView ID="gvCellTableData" runat="server" AutoGenerateColumns="False" AllowSorting="true" OnSorting="gvCellTableData_Sorting"
                        CssClass="table table-bordered cockpit headerFixerTable gridCSS plantCellGrid" OnRowDataBound="gvCellTableData_RowDataBound" meta:resourcekey="gridviewTableDataResource1" OnRowUpdating="gvCellTableData_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="Cell Id" SortExpression="GroupID">
                                <ItemTemplate>

                                    <asp:Button runat="server" ID="plantID" Text='<%#Eval("Groupid")%>' Style="background-color: unset; border: unset; color: white" CommandName="Update" OnClientClick="return showLoader();" />
                                    <asp:HiddenField runat="server" ID="hdnCellPlantID" Value='<%#Eval("Plantid")%>' />
                                    <asp:HiddenField runat="server" ID="hfCellId" Value='<%#Eval("Groupid")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>
                    <asp:Timer ID="timerDataChange" runat="server" OnTick="timerDataChange_Tick"></asp:Timer>
                    <asp:Button ID="btnTrigger" runat="server" Style="display: none;" OnClick="btnTrigger_Click" meta:resourcekey="btnTriggerResource1" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--<script src="shortingcssjs/js/jquery.tablesorter.min.js"></script>--%>
    <script type="text/javascript">
        var timer;
        var datarefreshInterval = 1000 * 30;
        $(document).ready(function () {
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
            setColumnNames();
            //$("#tblGrid").height(winHeight - 310);
            setControl();
            $.unblockUI({});
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
                //initTimer();
                RefreshMachineStatus();
            });

            $(".SortClick").on("click", function () {
                showLoader();
                return true;
            })
            //var newWidth = ($(window).width() - 180);
            //$("#tblGrid").width(newWidth);

            //$("[id$=ddlDayShift]").change(function () {
            //      $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            //});
            //$("#toggle").click(function () {
            //    newWidth = $(window).width();
            //    $("#tblGrid").width(newWidth);
            //});
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
            // initTimer();
            RefreshMachineStatus();
        });
        function setControl() {
            $('[id$=lbCellID]').multiselect({
                includeSelectAllOption: true
            });
        }
        function setColumnNames() {
            debugger;
            if ($('#MainContent_gridviewTableData')[0] != undefined) {
                var tbl = $(".cockpit")[0];
                debugger;
                var headers = $(tbl).find('th');
                var flag = 0;
                for (var i = 2; i < headers.length; i++) {
                    if ($(headers[i]).text() == "Target") {
                        debugger;
                        for (var j = 1; j < $(tbl).find('tr').length; j++) {
                            let firstRow = $(tbl).find('tr')[j];
                            if ($($(firstRow).find('td')[i]).text().trim() != "") {
                                flag = 1;
                                break;
                            }
                        }
                        if (flag == 0) {
                            $(tbl).find('td:nth-child(' + (i + 1) + ')').hide();
                            $(tbl).find('th:nth-child(' + (i + 1) + ')').hide();
                            break;
                        }
                    }
                }
            }

        }
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        $(document).on("click", ".machineClick", function () {
            window.open("VDGScreen.aspx?machineId=" + $(this).html() + "&shiftId=" + $("[id$=ddlShift]").val() + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val() + "&Page=table", "VDGScreen.aspx");
        });

        $(document).on("click", ".hypercol", function () {
            debugger;
            var classList = $(this).attr("class");
            var parameterName = "";
            if (classList.includes("HL_")) {
                var splitData = classList.split(" ");
                for (var i = 0; i < splitData.length; i++) {
                    if (splitData[i].includes("HL_")) {
                        let split1 = splitData[i].split("_");
                        parameterName = split1[split1.length - 1];
                        break;
                    }
                }
            }
            var machine = $(this).closest('td').prev('td').prev('td').prev('td').text().trim();
            if (parameterName == "" || parameterName == "OEE") {
                //alert($(this).closest('td').prev('td').text().trim());
                window.open("oeeGraphics.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val());
            }
            else if (parameterName == "AirPressure") {
                machine = $(this).closest('tr').find('.machineClick').text();
                PopupCenter("AirPressureData.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val(), "Air Pressure", 900, 500);
            }
            else if (parameterName == "SpindleRuntime") {
                machine = $(this).closest('tr').find('.machineClick').text();
                PopupCenter("SpindleRunTimeInfoLG.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val(), "Air Pressure", 900, 500);
            }
        });
        function PopupCenter(url, title, w, h) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=yes,toolbar=no,resizable=yes,width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
        function initTimer() {
            console.log("Timer " + timer);
            if ($('#hdnView').val() == "Machinewise") {
                clearTimeout(timer);
                timer = setTimeout(function () {
                    RefreshMachineStatus();
                }, datarefreshInterval);
            }
            else {
                clearTimeout(timer);
            }
        }

        function dateTimePicker() {
            $('.date1').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('.date2').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
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

        function RefreshMachineStatus() {
            clearTimeout(timer);
            if ($('#hdnView').val() == "Machinewise") {
                $.ajax({
                    type: "POST",
                    url: "tableView.aspx/GetMachineStatusData",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '"}',
                    dataType: "json",
                    success: function (response) {
                        var machineStatusData = response.d;
                        if (machineStatusData.length > 0) {
                            $("#MainContent_gridviewTableData").find("tr:gt(0)").each(function (index, tblRow) {
                                var machineId = $(this).find("td:eq(1)").find("a").html();
                                var statdata = machineStatusData.filter(x => x.MachineID == machineId);
                                if (statdata != null && statdata != undefined && statdata.length > 0) {
                                    if (statdata[0].MachineLiveStatus == "Running") {
                                        $(tblRow).find("td:eq(0)").html('<div class="loaders-container1" title="Running" style="float: left;"><div class="la-cog la-2x" style="float: left;"><div class="Running"></div></div></div><div style="float: right;">Running</div>');
                                    }
                                    else {
                                        /*  $(tblRow).find("td:eq(0)").html(('<div class="loaders-container1" style="float: left;" title="' + statdata[0].MachineLiveStatus + '"><div class="la-cog la-2x" style="float: left;"><div style="color:' + statdata[0].MachineStatusColor + ';"></div></div></div><div style="float: right;">' + (statdata[0].MachineLiveStatus == "NoData" ? "" : statdata[0].MachineLiveStatus) + '</div>'));*/
                                        $(tblRow).find("td:eq(0)").html(('<div class="loaders-container1" style="float: left;" title="' + statdata[0].MachineLiveStatus + '"><div class="la-cog la-2x" style="float: left;"><div style="color:' + statdata[0].MachineStatusColor + ';"></div></div></div><div style="float: right;">' + (statdata[0].MachineLiveStatus) + '</div>'));
                                    }
                                }
                            });
                        }
                        initTimer();
                    },
                    error: function (jqXHR, textStatus, err) {
                        //  alert('Error: ' + err);
                        initTimer();
                    }
                });
            }
            else {
                clearTimeout(timer);
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            var timer;
            var datarefreshInterval = 1000 * 30;
            setControl();
            $.unblockUI({});
            var winHeight = $(window).height();
            winHeight = screen.availHeight;
            console.log(winHeight);
            if (winHeight < 650) {
                //winHeight = (640);
                console.log('min');
            } else {
                //winHeight = (840);
                console.log('max');
            }
            setColumnNames();
            //$("#tblGrid").height(winHeight - 310);
            dateTimePicker();
            function dateTimePicker() {
                $('.date1').datetimepicker({
                    format: 'DD-MM-YYYY HH:mm',
                    locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
                });
                $('.date2').datetimepicker({
                    format: 'DD-MM-YYYY HH:mm',
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
            $(".SortClick").on("click", function () {
                showLoader();
                return true;
            })
            //initTimer();
            RefreshMachineStatus();
            function initTimer() {
                clearTimeout(timer);
                timer = null;
                console.log("hdn view " + $('#hdnView').val());
                if ($('#hdnView').val() == "Machinewise") {
                    timer = setTimeout(function () {
                        RefreshMachineStatus();
                    }, datarefreshInterval);
                }
                else {
                    clearTimeout(timer);
                    timer = null;
                }
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
                initTimer();
            });

            //function RefreshMachineStatus() {
            //    var d = new Date();
            //    console.log("Enter " + d);
            //    clearTimeout(timer);
            //    $.ajax({
            //        type: "POST",
            //        url: "tableView.aspx/GetMachineStatusData",
            //        contentType: "application/json; charset=utf-8",
            //        data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '"}',
            //        dataType: "json",
            //        success: function (response) {

            //            var machineStatusData = response.d;
            //            if (machineStatusData.length > 0) {
            //                $("#MainContent_gridviewTableData").find("tr:gt(0)").each(function (index, tblRow) {
            //                    var machineId = $(this).find("td:eq(1)").find("a").html();
            //                    var statdata = machineStatusData.filter(x => x.MachineID == machineId);
            //                    if (statdata != null && statdata != undefined && statdata.length > 0) {
            //                        if (statdata[0].MachineLiveStatus == "Running") {
            //                            $(tblRow).find("td:eq(0)").html('<div class="loaders-container1" style="float: left;" title="Running"><div class="la-cog la-2x" style="float: left;"><div class="Running"></div></div></div><div style="float: right;">Running</div>');
            //                        }
            //                        else {
            //                            $(tblRow).find("td:eq(0)").html(('<div class="loaders-container1" style="float: left;" title="' + statdata[0].MachineLiveStatus + '"><div class="la-cog la-2x" style="float: left;"><div style="color:' + statdata[0].MachineStatusColor + ';"></div></div></div><div style="float: right;">' + (statdata[0].MachineLiveStatus == "NoData" ? "" : statdata[0].MachineLiveStatus) + '</div>'));
            //                        }
            //                    }
            //                });
            //            }
            //            initTimer();
            //        },
            //        error: function (jqXHR, textStatus, err) {
            //            //alert('Error: ' + err);
            //            initTimer();
            //        }
            //    });
            //}               
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
