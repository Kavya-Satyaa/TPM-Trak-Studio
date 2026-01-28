<%@ Page Title="Historical Iconic View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IonicViewAggregated.aspx.cs" Inherits="Web_TPMTrakDashboard.IonicViewAggregated" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/Ionic.css" rel="stylesheet" />
    <%--<link href="MyCssAndJS/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />--%>

    <%--    <script src="MyCssAndJS/DatePicker/jquery-2.1.1.min.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/moment-with-locales.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"></script>--%>

    <%-- <%: Styles.Render("~/bundles/tablecss") %>--%>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        .table .lbl {
            padding-top: 15px;
        }

        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
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

        .table {
            margin-bottom: 0px;
        }

        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }

        .Green {
            /*background-color: green;*/
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .Red {
            /*background-color: red;*/
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .Yellow {
            /*background-color: yellow;*/
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .white {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
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

        .PDT {
            color: blue;
        }

        a {
            color: black;
        }

        .HyperLink {
            text-decoration: underline;
            cursor: pointer;
            color: #547CFF;
        }

        .pager li > a:hover, .pager li > a:focus {
            background-color: #428bca;
            color: #ffffff;
            margin: 0px 0;
        }

        .pager li > a, .pager li > span {
            background-color: #428bca;
            color: #ffffff;
            margin: 0px 0;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .ellipsistooltip {
            text-overflow: ellipsis;
            overflow: hidden;
            width: 160px;
            white-space: nowrap;
        }

        .unselectable {
            -moz-user-select: none;
            -webkit-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        .all {
            -moz-user-select: all;
            -webkit-user-select: all;
            -ms-user-select: all;
            user-select: all;
        }

        #lvPlantDetails myItem:nth-child(3), #lvPlantDetails div:nth-child(3), #lvPlantDetails div:nth-child(4), #lvPlantDetails div:nth-child(5), #lvPlantDetails div:nth-child(6) {
            display: none
        }

        .fixHeader {
            position: sticky;
            top: 70px;
            background-color: #202648;
            z-index: 999;
        }
    </style>
    <asp:UpdatePanel ID="updatePanal" runat="server">
        <ContentTemplate>
            <div class="row" style="text-align: center; color: red;">
                <asp:HiddenField ID="hdfMode" runat="server" />
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
            </div>

            <div class="row fixHeader" style="width: 100%">
                <table id="tblfilter" class="table table-bordered" style="width: auto">
                    <tr>
                        <td class="commontd" style="min-width: 80px; height: 50px"><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>
                        <td class="input-group" style="min-width: 110px;">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" Style="width: 110px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td class="commontd" style="height: 50px"><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b></td>
                        <td class="input-group" style="min-width: 110px;">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" Style="width: 110px; min-height: 40px;" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td class="commontd" style="width: 60px;"><b><%=GetGlobalResourceObject("CommanResource","Plant") %></b></td>
                        <td style="min-width: 180px;">
                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" meta:resourcekey="ddlPlantIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="commontd" style="width: 54px;"><b><%=GetGlobalResourceObject("CommanResource","CellId") %></b></td>
                        <td style="min-width: 120px;">
                            <%-- <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control" meta:resourcekey="ddlCellIdResource1">
                            </asp:DropDownList>--%>
                            <asp:ListBox runat="server" ID="lbCellID" CssClass="form-control" SelectionMode="Multiple" ClientIDMode="Static"></asp:ListBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="commontd" style="width: 54px;"><b>View</b></td>
                        <td style="min-width: 120px;">
                            <asp:DropDownList ID="ddlView" runat="server" CssClass="form-control" meta:resourcekey="ddlCellIdResource1" Style="background-color: #39b3d7; color: white; font-weight: bold" OnSelectedIndexChanged="ddlView_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="Plantwise">Plant View</asp:ListItem>
                                <asp:ListItem Value="cellwise">Cell View</asp:ListItem>
                                <asp:ListItem Value="Machinewise">Machine View</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="commontd" style="width: 95px;">Sort Order</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSortOrder" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        </td>
                        <td style="width: 60px;">
                            <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" class="btn btn-info btn-sm displayCss" ID="Button1" OnClick="btnProcess_Click" OnClientClick="return showLoader();"></asp:Button>
                        </td>
                        <td runat="server" id="tdlbBackButton" style="width: 120px; background-color: #e7e7e7">
                            <%-- <asp:Button runat="server" ID="btnBack" Visible="false" CssClass="btn btn-info btn-sm displayCss glyphicon glyphicon-chevron-left" OnClick="lbBackButton_Click" ></asp:Button>--%>

                            <asp:LinkButton runat="server" ID="lbBackButton" CssClass="	glyphicon glyphicon-chevron-left" Style="font-weight: bold; color: #115d9f; font-size: 15px; height: 30px" OnClick="lbBackButton_Click"></asp:LinkButton>

                        </td>
                        <td colspan="3">
                            <asp:LinkButton runat="server" ID="lnkSwitch" CssClass="btn btn-info btn-sm glyphicon glyphicon-random" Style="font-weight: bold; color: white; font-size: 15px; height: 30px" Text=" SwitchToTable" OnClick="lnkSwitch_Click" OnClientClick="return showLoader();"></asp:LinkButton>
                        </td>
                        <%--<td colspan="2" class="commontd" style="min-width: 80px;"></td>--%>
                    </tr>

                </table>

            </div>

            <div class="row">
                <%-- <asp:LinkButton runat="server" ID="lbBackButton" CssClass="glyphicon glyphicon-chevron-left btn btn-info btn-sm displayCss" OnClick="lbBackButton_Click" style=" margin: 10px 0px 0px 30px"></asp:LinkButton>--%>

                <asp:HiddenField runat="server" ID="hfPlantIdForBack" />
                <asp:HiddenField runat="server" ID="hdnCellPlantIDForBack" />
                <asp:HiddenField runat="server" ID="hdnCellIDForBack" />
                <div id="divCustomerList" style="overflow-x: auto;">
                    <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="LstCustomers">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="myItem" style="margin: 15px; min-width: 100px; display: inline-block;">
                                <div class="border">
                                    <div class="<%# Eval("MachineOEE") %>" style="padding: 10px; background-color: <%# Eval("BackColor") %>; border-radius: 25px">
                                        <table style='width: 100%;' class="outercockpit">
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                                    <asp:Label runat="server" ID="lblplant">Machine : </asp:Label>
                                                    <%# Eval("MachineId") %>
                                                </td>

                                            </tr>
                                        </table>
                                        <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white;'>
                                            <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>;">
                                                            <%# Eval("LabelText")%>                                           
                                                        </td>
                                                        <td style='text-align: left; width: 170px; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                            <div class="ellipsistooltip" title='<%# Eval("LabelValueToolTip") %>'><%# Eval("LabelValue")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </div>
                                    <div style='text-align: center; padding: 1px; display: <%= settings.ShowSmileyBlock %>;'>
                                        <img src="<%# Eval("SmileyImagePath") %>" style='height: <%= settings.SmileyBlockSize %>; width: auto;' />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>

                    <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="lvPlantDetails" ClientIDMode="Static" OnItemUpdating="lvPlantDetails_ItemUpdating">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="myItem" style="margin: 15px; min-width: 100px; display: inline-block;">
                                <div class="border">
                                    <div class="<%# Eval("MachineOEE") %>" style="padding: 10px; background-color: <%# Eval("BackColor") %>; border-radius: 25px">
                                        <table style='width: 100%;' class="outercockpit">
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                                    <%--<%# Eval("MachineId") %>--%>
                                                    <asp:HiddenField runat="server" ID="hfPlantId" Value=' <%# Eval("PlantID") %>' />
                                                    <asp:HiddenField runat="server" ID="hfViewType" Value="Plant" />
                                                    <%-- <asp:Label runat="server" ID="lblplant" style="background-color: #0072c6; color: white;padding: 3px">Plant: </asp:Label>--%>
                                                    <asp:Button runat="server" ID="btnPlant" Text='<%# "Plant : "+ Eval("PlantID") %>' Style="border: unset; background-color: unset; background-color: #0072c6; color: white; font-weight: bold; padding: 5px 15px; border-radius: 8px; cursor: pointer" CommandName="Update" />
                                                </td>

                                            </tr>
                                        </table>
                                        <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white;'>
                                            <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>;">
                                                            <%# Eval("LabelText")%>                                           
                                                        </td>
                                                        <td style='text-align: left; width: 170px; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                            <div class="ellipsistooltip" title='<%# Eval("LabelValueToolTip") %>'><%# Eval("LabelValue")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </div>
                                    <div style='text-align: center; padding: 1px; display: <%= settings.ShowSmileyBlock %>;'>
                                        <img src="<%# Eval("SmileyImagePath") %>" style='height: <%= settings.SmileyBlockSize %>; width: auto;' />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>

                    <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="lvCellDetails" OnItemUpdating="lvCellDetails_ItemUpdating">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="myItem" style="margin: 15px; min-width: 100px; display: inline-block;">
                                <div class="border">
                                    <div class="<%# Eval("MachineOEE") %>" style="padding: 10px; background-color: <%# Eval("BackColor") %>; border-radius: 25px">
                                        <table style='width: 100%;' class="outercockpit">
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                                    <asp:HiddenField runat="server" ID="hdnCellPlantID" Value=' <%# Eval("PlantID") %>' />
                                                    <asp:HiddenField runat="server" ID="hfCellId" Value=' <%# Eval("GroupName") %>' />
                                                    <asp:HiddenField runat="server" ID="hfViewType" Value="Cell" />
                                                    <%-- <asp:Label runat="server" ID="lblplant" style="background-color: #0072c6; color: white;padding: 3px">Cell: </asp:Label>--%>
                                                    <asp:Button runat="server" ID="btnPlant" Text='<%# "Cell : "+Eval("GroupName") %>' Style="border: unset; background-color: unset; background-color: #0072c6; color: white; font-weight: bold; padding: 5px 15px; border-radius: 8px; cursor: pointer" CommandName="Update" />
                                                </td>

                                            </tr>
                                        </table>
                                        <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white;'>
                                            <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: left; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>;">
                                                            <%# Eval("LabelText")%>                                           
                                                        </td>
                                                        <td style='text-align: left; width: 170px; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                            <div class="ellipsistooltip" title='<%# Eval("LabelValueToolTip") %>'><%# Eval("LabelValue")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </div>
                                    <div style='text-align: center; padding: 1px; display: <%= settings.ShowSmileyBlock %>;'>
                                        <img src="<%# Eval("SmileyImagePath") %>" style='height: <%= settings.SmileyBlockSize %>; width: auto;' />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>

                </div>
            </div>
            <%--  <asp:Button ID="btnTrigger" runat="server" Style="display: none;" OnClick="btnTrigger_Click" />
            <asp:Timer ID="timerDataChange" runat="server" OnTick="timerDataChange_Tick"></asp:Timer>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var showItem = NaN, divH, divW, screenH, screenW, totalH, totalW, totalBox;
        var currentTab = 0;
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

            //$("#divCustomerList").height(winHeight - 223);

            $.unblockUI({});
            $('.ellipsistooltip').tooltipOnOverflow();
            $('[data-toggle="tooltip"]').tooltip();
            // showFirstNItems();

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            $('[id$=lbCellID]').multiselect({
                includeSelectAllOption: true
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

            $("[id$=ddlDayShift]").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });
        });
        $.fn.tooltipOnOverflow = function (options) {
            $(this).on("mouseenter", function () {
                if (this.offsetWidth < this.scrollWidth) {
                    options = options || { placement: "auto" }
                    options.title = $(this).text();
                    $(this).tooltip(options);
                    $(this).tooltip("show");
                } else {
                    if ($(this).data("bs.tooltip")) {
                        $tooltip.tooltip("hide");
                        $tooltip.removeData("bs.tooltip");
                    }
                }
            });
        };
        function showLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function CountNumberOfBox() {
            divH = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());

            divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            screenH = $(window).height() - 170;//screen.availHeight - 100;
            screenW = $(window).width() - 180;//screen.availWidth - 5;
            totalH = Math.floor(screenH / (divH));
            totalW = Math.floor(screenW / (divW));
            totalBox = Math.floor(totalH * totalW);
            showItem = totalBox;
            console.log(showItem);
        }

        function SetIconicBoxWidth() {
            divH = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());

            divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            $("#divCustomerList .myItem").width(divW);
        }

        function showFirstNItems() {
            $("#divCustomerList .myItem").hide();
            $("#divCustomerList .myItem").slice(0, showItem).show();
        }

        function NextAndPreviusFun() {
            $("#btnNext").show();
            showFirstNItems();
        }

        $("#btnPrevious").click(function () {
            var c = $("#divCustomerList .myItem").length;
            if (currentTab == c - 1 || currentTab != 0) {
                currentTab = currentTab - showItem;
                showNextNItems();
            }
            if (currentTab <= 0) {
                $("#btnPrevious").hide();
                $("#btnNext").show();
            }
            else {
                $("#btnNext").show();
            }
        });

        $("#btnNext").click(function () {
            var c = $("#divCustomerList .myItem").length;
            if (currentTab < c - 1) {
                currentTab = currentTab + showItem;
                showNextNItems();
            }
            if (currentTab >= c - showItem) {
                $("#btnNext").hide();
                $("#btnPrevious").show();
            }
            else {
                $("#btnPrevious").show();
                $("#btnNext").show();
            }
        });

        function showNextNItems() {
            var nextItems = $("#divCustomerList .myItem:visible ~ .myItem:hidden").slice(0, showItem);
            if (nextItems.length == 0) {
                // if Next items ended
                showFirstNItems();
                //nextItems = $("#divCustomerList .myItem").slice(0, showItem); // then start from first again
            }
            if (nextItems.length > 0) {
                $("#divCustomerList .myItem:visible").hide();
                nextItems.show();
            }
        }

        $("#chkBox").click(function () {
            if (this.checked) {
                currentTab = 0;
                $("#divCustomerList .myItem").show();
                $("#divCustomerList .myItem").css('display', 'inline-block');
                CountNumberOfBox();
                NextAndPreviusFun();
            }
            else {
                $("#divCustomerList .myItem").show();
                $("#btnPrevious").hide();
                $("#btnNext").hide();
            }
        });

        $("[id$=chkAutoBox]").click(function () {
            $("[id$=btnTrigger]").trigger("click");
            // return false;
        });

        $(document).on("click", "[id^='MainContent_LstCustomers_lnkMachine_']", function () {
            var machineId = $(this).text();
            window.open("VDGScreen.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val() + "&Page=table", "VDGScreen.aspx");
            return false;
        });

        $(document).on("click", ".HyperLink", function (e) {
            var machine = $(this).attr('machineName');
            window.open("oeeGraphics.aspx?machineId=" + machine + "&fromDate=" + $("[id$=txtFromDate]").val() + "&toDate=" + $("[id$=txtToDate]").val());
        })
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

            //$("#divCustomerList").height(winHeight - 223);


            $('.ellipsistooltip').tooltipOnOverflow();
            $('[data-toggle="tooltip"]').tooltip();
            function CountNumberOfBox() {
                divH = Math.max.apply(null, $('.myItem').map(function () {
                    return $(this).outerHeight(true);
                }).get());

                divW = Math.max.apply(null, $('.myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                screenH = $(window).height() - 170;//screen.availHeight - 100;
                screenW = $(window).width() - 180;//screen.availWidth - 5;
                totalH = Math.floor(screenH / (divH));
                totalW = Math.floor(screenW / (divW));
                totalBox = Math.floor(totalH * totalW);
                showItem = totalBox;
                console.log(showItem);
            }

            function showFirstNItems() {
                $("#divCustomerList .myItem").hide();
                $("#divCustomerList .myItem").slice(0, showItem).show();
            }

            function NextAndPreviusFun() {
                $("#btnNext").show();
                showFirstNItems();
            }

            $("#btnPrevious").click(function () {
                var c = $("#divCustomerList .myItem").length;
                if (currentTab == c - 1 || currentTab != 0) {
                    currentTab = currentTab - showItem;
                    showNextNItems();
                }
                if (currentTab <= 0) {
                    $("#btnPrevious").hide();
                    $("#btnNext").show();
                }
                else {
                    $("#btnNext").show();
                }
            });

            $("#btnNext").click(function () {
                var c = $("#divCustomerList .myItem").length;
                if (currentTab < c - 1) {
                    currentTab = currentTab + showItem;
                    showNextNItems();
                }
                if (currentTab >= c - showItem) {
                    $("#btnNext").hide();
                    $("#btnPrevious").show();
                }
                else {
                    $("#btnPrevious").show();
                    $("#btnNext").show();
                }
            });

            function showNextNItems() {
                var nextItems = $("#divCustomerList .myItem:visible ~ .myItem:hidden").slice(0, showItem);
                if (nextItems.length == 0) {
                    // if Next items ended
                    showFirstNItems();
                    //nextItems = $("#divCustomerList .myItem").slice(0, showItem); // then start from first again
                }
                if (nextItems.length > 0) {
                    $("#divCustomerList .myItem:visible").hide();
                    nextItems.show();
                }
            }

            $("#chkBox").click(function () {
                if (this.checked) {
                    currentTab = 0;
                    $("#divCustomerList .myItem").show();
                    $("#divCustomerList .myItem").css('display', 'inline-block');
                    CountNumberOfBox();
                    NextAndPreviusFun();
                }
                else {
                    $("#divCustomerList .myItem").show();
                    $("#btnPrevious").hide();
                    $("#btnNext").hide();
                }
            });

            $("#chkBox").click(function () {
                if (this.checked) {
                    currentTab = 0;
                    $("#divCustomerList .myItem").show();
                    $("#divCustomerList .myItem").css('display', 'inline-block');
                    CountNumberOfBox();
                    NextAndPreviusFun();
                }
                else {
                    $("#divCustomerList .myItem").show();
                    $("#btnPrevious").hide();
                    $("#btnNext").hide();
                }
            });
            $("[id$=chkAutoBox]").click(function () {
                $("[id$=btnTrigger]").trigger("click");
                // return false;
            });
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            $('[id$=lbCellID]').multiselect({
                includeSelectAllOption: true
            });
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
