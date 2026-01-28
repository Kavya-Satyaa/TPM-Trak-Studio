<%@ Page Title="" Language="C#" MasterPageFile="~/WebAndon/AndonMaster.Master" AutoEventWireup="true" CodeBehind="Andon.aspx.cs" Inherits="Web_TPMTrakDashboard.WebAndon.AndonViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
        }

        .Green {
            background-color: #<%= settings.ColorUISetting.GoodColor.Substring(3) %>;
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .Red {
            background-color: #<%= settings.ColorUISetting.BadColor.Substring(3) %>;
            border-radius: 25px;
            border: 0.1px solid #cccccc;
        }

        .Yellow {
            background-color: #<%= settings.ColorUISetting.ModerateColor.Substring(3) %>;
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
            color: white;
        }

        .Stopped {
            color: red;
        }

        .PDT {
            color: blue;
        }

        a {
            color: #0033cc;
        }
    </style>
    <div class="row" style="text-align: center; width: 99%; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
    </div>
    <asp:UpdatePanel ID="updatePabal" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfMode" runat="server" />
            <asp:HiddenField ID="hdfGroupId" runat="server" />
            <%--          <asp:Button ID="btnProcessHide" runat="server" OnClick="btnProcess_Click" Style="display: none;" />--%>
            <asp:Button ID="btnPlantChange" runat="server" OnClick="btnPlantChange_Click" Style="display: none;" />
            <asp:Button ID="btnLoadPageEvent" runat="server" OnClick="btnLoadPageEvent_Click" Style="display: none;" />
            <div id="divCustomerList" style='font-family: <%= settings.AppUISettings.FontFamily %>; font-style: <%= settings.AppUISettings.FontStyle %>; font-weight: <%= settings.AppUISettings.FontStyle %>;'>
                <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="LstCustomers">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="myItem" style="margin: 15px; min-width: 100px; display: inline-block;">
                            <div class=' <%= settings.IconicUISetting.ShowSmileyImage == "hide" ? "" :"border" %>'>
                                <div class="<%# Eval("MachineOEE") %>" style="padding: 10px;">
                                    <table style='font-size: <%= settings.IconicUISetting.FontSizeOuterTab %>; width: 100%'>
                                        <tr>
                                            <td style="text-align: center; color: #0033cc; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                                <%# Eval("MachineId")%>
                                              <%--  <asp:LinkButton ID="lnkMachine" CssClass="cssNonAdmin" runat="server" CommandArgument='<%# Eval("MachineId")%>' CommandName='<%# Eval("GroupName")%>'
                                                    OnClick="lnkMachine_Click"> <%# Eval("MachineId")%>
                                                </asp:LinkButton>--%>
                                            </td>
                                            <td style="padding-bottom: 8px; background-color: transparent; width: 35px;">
                                                <asp:Image ImageUrl='<%# Eval("StatusImage") %>' title='<%# Eval("MachineStatus") %>' runat="server"
                                                    Visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?true:false %>' />
                                                <div class="loaders-container" title='<%# Eval("MachineStatus") %>' runat="server" visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?false:true %>'>
                                                    <div class="la-cog la-2x" style="float: right;">
                                                        <div class="<%# Eval("MachineStatus") %>"></div>
                                                    </div>
                                                </div>

                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table table-bordered" style='font-size: <%= settings.IconicUISetting.FontSizeInerTab %>; background-color: white;'>
                                        <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                            <LayoutTemplate>
                                                <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: left; min-width: 100px; color: #0033cc;">
                                                        <%# Eval("LabelText")%>                                           
                                                    </td>
                                                    <td style='text-align: left; min-width: 100px; color: <%# Eval("ColorProperties") %>'>
                                                        <%# Eval("LabelValue")%>                                           
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </table>
                                </div>
                                <div style='text-align: center; padding: 1px; display: <%= settings.IconicUISetting.ShowSmileyImage %>;'>
                                    <img src="<%# Eval("SmileyImagePath") %>" style='height: <%= settings.IconicUISetting.SmileyImageSize %>; width: auto;' />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
   <%--   <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>--%>
    <link href="../AndonScripts/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />
    <script src="../AndonScripts/DatePicker/jquery-2.1.1.min.js"></script>
    <script src="../AndonScripts/DatePicker/moment-with-locales.js"></script>
    <script src="../AndonScripts/DatePicker/bootstrap-datetimepicker.js"></script>
    <script>
        //dateTimePicker();
        function dateTimePicker() {
          
        }


        //var idleTime = 0;
        var hideShowTimer, delayTimePage, stopPageTimer;
        var _refreshDataRequired = 0;
        var modeData = $("[id$=hdfMode]").val();
        var boxMovingTimer = parseInt(<%= settings.IconicUISetting.ScreenFlipInterval %>);
        var pageLoadTimer = parseInt(<%= settings.AppUISettings.DataDisplayInterval %>);
        delayTimePage = pageLoadTimer * 1000;
        var showItem = NaN, delay = boxMovingTimer * 1000, divH, divW, screenH, screenW, totalH, totalW, totalBox;
        //var screenH = $(window).height() - 73;//screen.availHeight - 100;
        //var screenW = $(window).width();//screen.availWidth - 5;
        //var totalH = Math.floor(screenH / (divH));
        //var totalW = Math.floor(screenW / (divW));
        //var totalBox = Math.floor(totalH * totalW);
        CountNumberOfBox();
        //showItem = totalBox;
        $(".cssNonAdmin").removeAttr('href').css({ 'cursor': 'pointer', 'pointer-events': 'none' });
        $(".cssNonAdmin").css("text-decoration", "none");
        if (modeData == "ANDON") {
            //Zero the idle timer on mouse movement. 
            //$(document).on("mouseover", "#divCustomerList", function (e) {
            //    StopAllTimer();
            //});

            //$(document).on("mouseenter", "#divCustomerList", function () {               
            //    StopAllTimer();
            //});

            //$(document).on("mouseout", "#divCustomerList", function () {
            //    StopAllTimer();               
            //    initTimer(showItem, delay);
            //    if (stopPageTimer != null) clearTimeout(stopPageTimer);
            //    stopPageTimer = setTimeout(timerIncrement, delayTimePage);
            //});

            //$(this).keypress(function (e) {
            //    StopAllTimer();
            //});
            document.body.style.overflow = 'hidden';
            StartFromBegin();

        }
        else {

            //$(document).on("mouseover", "#divCustomerList", function () {
            //    if (stopPageTimer != null) clearTimeout(stopPageTimer);
            //});

            //$(document).on("mouseout", "#divCustomerList", function () {
            //    if (stopPageTimer != null) clearTimeout(stopPageTimer);
            //    stopPageTimer = setTimeout(timerIncrement, delayTimePage);
            //});

            //$(this).keypress(function (e) {
            //    if (stopPageTimer != null) clearTimeout(stopPageTimer);
            //});

            stopPageTimer = setTimeout(timerIncrement, delayTimePage);
            var divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(false);
            }).get());
            $(".myItem").width(divW);
        }
        //});

        $(document).on("change", "[id*=ddlPlantName]", function () {
            if (modeData == "ANDON") {
                StopAllTimer();
                $.blockUI({ message: '<img src="../Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
            }
        });

        $(document).on("change", "[id$=ddlShift]", function () {
            if (modeData == "DESKTOP") {
                StopAllTimer();
                $.blockUI({ message: '<img src="../Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
            }
        });

        $(document).on("click", "[id$=btnProcess]", function () {
            if ($("[id$=txtDate]").val() == "") {
                alert("Please enter from Date !!");
                $("[id$=txtDate]").focus();
                return false;
            }
            //--------------Date Diff.-------------------------------------
            //var firstDate = new Date($("[id$=txtDate]").val());
            //var secondDate = new Date($("[id$=txtToDate]").val());
            //var oneDay = 24 * 60 * 60 * 1000;
            //var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));

            StopAllTimer();
            //clearInterval(stopPageTimer);
            //clearInterval(hideShowTimer);
            $.blockUI({ message: '<img src="../Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
            //$("[id$=btnProcessHide]").trigger("click");
        });

        $(document).on("click", "[id^='MainContent_LstCustomers_lnkMachine_']", function () {
            var machineId = $(this).text();
            var passdate = $("[id$=txtDate]").val();
            var passTodate = $("[id$=txtToDate]").val();
            window.open("VDGScreen.aspx?machineId=" + machineId + "&passdate=" + passdate + "&passTodate=" + passTodate, "MyTargetWindowName");
            return false;
            //if (enavleMachineGroup == 0 && appCompabyId != 1) {
            //    $(".cssNonAdmin").removeAttr('href').css({ 'cursor': 'pointer', 'pointer-events': 'none' });
            //    $(".cssNonAdmin").css("text-decoration", "none");
            //}
            //else {
            StopAllTimer();
            $("#divCustomerList").css({ 'cursor': 'wait' });
            $("a").css({ 'cursor': 'wait' });
            //}
        });

        function StartFromBegin() {
            $('#divCustomerList').css({ 'cursor': 'auto' });
            $("a").css({ 'cursor': 'auto' });
            if (isNaN(showItem)) {
                CountNumberOfBox();
            }
            //if (enavleMachineGroup == 0) {
            //    $(".cssNonAdmin").removeAttr('href').css({ 'cursor': 'pointer', 'pointer-events': 'none' });
            //    $(".cssNonAdmin").css("text-decoration", "none");
            //}
            //divW = Math.max.apply(null, $('.myItem').map(function () {
            //    return $(this).outerWidth(false);
            //}).get());
            //$(".myItem").width(divW);
            StopAllTimer();
            showFirstNItems();
            //showNextNItems();
            initTimer();
            stopPageTimer = setTimeout(timerIncrement, delayTimePage);
            return false;
        }

        function CountNumberOfBox() {
            divH = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());

            divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            screenH = $(window).height() - 73;//screen.availHeight - 100;
            screenW = $(window).width();//screen.availWidth - 5;
            totalH = Math.floor(screenH / (divH));
            totalW = Math.floor(screenW / (divW));
            totalBox = Math.floor(totalH * totalW);
            showItem = totalBox;
            //if (enavleMachineGroup == 0) {
            //    $(".cssNonAdmin").removeAttr('href').css({ 'cursor': 'pointer', 'pointer-events': 'none' });
            //    $(".cssNonAdmin").css("text-decoration", "none");
            //}
            //divW = Math.max.apply(null, $('.myItem').map(function () {
            //    return $(this).outerWidth(false);
            //}).get());
            //$(".myItem").width(divW);

        }

        function StartFromBeginDesktopMode() {
            //$('#divCustomerList').css({ 'cursor': 'auto' });
            //$("a").css({ 'cursor': 'auto' });
            if (isNaN(showItem)) {
                CountNumberOfBox();
            }
            //if (enavleMachineGroup == 0 && appCompabyId != 1) {
            //    $(".cssNonAdmin").removeAttr('href').css({ 'cursor': 'pointer', 'pointer-events': 'none' });
            //    $(".cssNonAdmin").css("text-decoration", "none");
            //}
            divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(false);
            }).get());
            
            
            $(".myItem").width(divW);
            StopAllTimer();
            stopPageTimer = setTimeout(timerIncrement, delayTimePage);
            return false;
        }

        function StopAllTimer() {
            if (hideShowTimer != null) clearTimeout(hideShowTimer);
            if (stopPageTimer != null) clearTimeout(stopPageTimer);
            hideShowTimer = null; stopPageTimer = null;
        }

        //data refesh timer
        function timerIncrement() {
            _refreshDataRequired = 1;
            if (modeData == "DESKTOP") {
                $("[id$=btnLoadPageEvent]").trigger("click");
            }
            else if ($("[id$=hdfGroupId]").val() != "") {
                $("[id$=btnLoadPageEvent]").trigger("click");
            }
            //stopPageTimer = setTimeout(timerIncrement, delayTimePage);
            return false;
        }

        function initTimer() {
            // alert("init timer called");
            if (hideShowTimer != null) clearTimeout(hideShowTimer);
            hideShowTimer = setTimeout(function () {
                showNextNItems(showItem);
                initTimer(showItem, delay);
            }, delay);
        }

        function showFirstNItems() {
            $("#divCustomerList .myItem").hide();
            $("#divCustomerList .myItem").slice(0, showItem).show();
        }

        function showNextNItems() {
            var nextItems = $("#divCustomerList .myItem:visible ~ .myItem:hidden").slice(0, showItem);

            if (nextItems.length == 0) {
                // if Next items ended
                if (_refreshDataRequired == 1) {
                    StopAllTimer();
                    _refreshDataRequired = 0;
                    $("[id$=btnLoadPageEvent]").trigger("click");
                    _refreshDataRequired = 0;
                }
                else {
                    showFirstNItems();
                }
                //nextItems = $("#divCustomerList .myItem").slice(0, showItem); // then start from first again
            }
            if (nextItems.length > 0) {
                $("#divCustomerList .myItem:visible").hide();
                nextItems.show();
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            dateTimePicker();
        });
    </script>
</asp:Content>
