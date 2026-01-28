<%@ Page Title="Down Time Matrix Info" Trace="false" AsyncTimeout="6000" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineDownReasonTimeMatrix.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineDownReasonTimeMatrix" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>

    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>

    <%: Scripts.Render("~/bundles/paretoandDrillDownChartJs") %>
    <%: Scripts.Render("~/bundles/paretoChartJs") %>
    <%--   <script src="MyCssAndJS/paretoAndDrillDown/highcharts.js"></script>
    <script src="MyCssAndJS/paretoAndDrillDown/data.js"></script>
    <script src="MyCssAndJS/paretoAndDrillDown/drilldown.js"></script>--%>

    <%-- <%: Scripts.Render("~/bundles/VDGjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>--%>
    <%-- <%: Scripts.Render("~/bundles/paretoChartJs") %>--%>
    <%-- <script src="MyCssAndJS/pareto/highcharts.js"></script>
    <script src="MyCssAndJS/pareto/pareto.js"></script>--%>
    <%--    <script src="MyCssAndJS/pareto/pareto.js"></script>--%>
    <%--  <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>--%>
    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }


        #MainContent_gridTimeWiseInfo th {
            color: white !important;
            background-color: #2E6886 !important;
        }

        #MainContent_gridTimeWiseFreq th {
            color: white !important;
            background-color: #2E6886 !important;
        }

        #MainContent_gridFreqWiseInfo td[colspan="3"] {
            text-align: center;
        }

        #MainContent_gridFreqWiseInfo th {
            color: white !important;
            background-color: #2E6886 !important;
        }

        .tbl tr {
            transition: background 0.2s ease-in;
        }

        .tbl tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        .tbl tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .tbl tr:hover {
            background-color: chartreuse;
            cursor: pointer;
        }

        .colorTotal tr td:last-child {
            background-color: #FFFF00;
            font-weight: bold;
        }

        .colorTotal tbody tr:last-child {
            background-color: #FFFF00;
            font-weight: bold;
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

        .td {
            width: auto;
        }

        .chartInfo {
            font-family: segoe ui;
            color: red;
            margin-top: 10px;
        }
        /*.gridHeightSetter{
         height: 500px;
         overflow: auto;
        }

      .headerFixer  {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }*/
    </style>



    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <%--  <div class="row text-center" style="text-align: center; color: red;overflow-y:hidden;overflow-x:auto">--%>
            <asp:HiddenField ID="hdfValue" runat="server" Value="OK" />
            <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
            <table class="table table-bordered" id="tblfilter">
                <tr>
                    <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","PlantID")%></b></td>
                    <td>
                        <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" ToolTip="<%$Resources:CommanResource, PlantTooltip %>" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList></td>
                    <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","CellId")%></b></td>
                    <td>
                        <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control" data-toggle="tooltip" title="Cell ID !" ToolTip="<%$Resources:CommanResource, CellToolTip %>" ClientIDMode="Static" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","MachineId")%></b></td>
                    <td>
                        <asp:ListBox ID="ddlMachineId" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource, MachineTooltip %> " Width="150" ClientIDMode="Static"></asp:ListBox></td>
                    <td class="commontd">
                        <b><%=GetLocalResourceObject("Down ID") %>   </b>
                    </td>
                    <td class="commontd">
                        <asp:ListBox ID="ddlMultiDownID" runat="server" SelectionMode="Multiple" CssClass="form-control" ToolTip="<%$Resources:CommanResource, ALL %>" Style="width: 100px;"></asp:ListBox>
                    </td>
                    <td id="tdPredefinedTimeh" runat="server" class="commontd"><b><%=GetGlobalResourceObject("CommanResource","PredefinedTime") %></b></td>
                    <td id="tdPredefinedTime" runat="server" style="width: 160px;">
                        <asp:DropDownList ID="ddlDayShift" runat="server" Style="width: 200px"
                            CssClass="form-control displayCss" AutoPostBack="True" OnSelectedIndexChanged="ddlDayShift_SelectedIndexChanged" meta:resourcekey="ddlDayShiftResource1">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                    <td class="input-group" style="padding-bottom: 10px; padding-top: 10px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <%--  <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                title="From Date !" placeholder="From Date" ToolTip="<%$Resources:CommanResource, FromDate %>" AutoCompleteType="Disabled" TextMode="DateTimeLocal" Width="220"></asp:TextBox>--%>
                        <asp:TextBox ID="txtFromDate" runat="server" Style="width: 160px; height: 42px" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    <td class="commontd"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                    <td class="input-group" style="padding-bottom: 10px; padding-top: 10px;">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <%--   <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                title="To Date !" placeholder="To Date" ToolTip="<%$Resources:CommanResource, ToDate %>" AutoCompleteType="Disabled" TextMode="DateTimeLocal" Width="220"></asp:TextBox>--%>
                        <asp:TextBox ID="txtToDate" Style="width: 160px; height: 42px;" runat="server" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>
                    
                    <td class="commontd">
                        <b><%=GetLocalResourceObject("Show Top Down Reasons") %>   </b></td>
                    <td>
                        <asp:DropDownList ID="ddlTopDownReasons" runat="server" CssClass="select form-control" meta:resourcekey="ddlTopDownReasonsResource1">
                            <asp:ListItem Value="1" meta:resourcekey="ListItemResource1">Top 1</asp:ListItem>
                            <asp:ListItem Value="2" meta:resourcekey="ListItemResource2">Top 2</asp:ListItem>
                            <asp:ListItem Value="3" meta:resourcekey="ListItemResource3">Top 3</asp:ListItem>
                            <asp:ListItem Value="4" meta:resourcekey="ListItemResource4">Top 4</asp:ListItem>
                            <asp:ListItem Value="5" meta:resourcekey="ListItemResource5">Top 5</asp:ListItem>
                            <asp:ListItem Value="6" meta:resourcekey="ListItemResource6">Top 6</asp:ListItem>
                            <asp:ListItem Value="7" meta:resourcekey="ListItemResource7">Top 7</asp:ListItem>
                            <asp:ListItem Value="8" meta:resourcekey="ListItemResource8">Top 8</asp:ListItem>
                            <asp:ListItem Value="9" meta:resourcekey="ListItemResource9">Top 9</asp:ListItem>
                            <asp:ListItem Value="10" Selected="True" meta:resourcekey="ListItemResource10">Top 10</asp:ListItem>
                            <asp:ListItem Value="11">Top 11</asp:ListItem>
                            <asp:ListItem Value="12">Top 12</asp:ListItem>
                            <asp:ListItem Value="13">Top 13</asp:ListItem>
                            <asp:ListItem Value="14">Top 14</asp:ListItem>
                            <asp:ListItem Value="15">Top 15</asp:ListItem>
                            <asp:ListItem Value="16">Top 16</asp:ListItem>
                            <asp:ListItem Value="17">Top 17</asp:ListItem>
                            <asp:ListItem Value="18">Top 18</asp:ListItem>
                            <asp:ListItem Value="19">Top 19</asp:ListItem>
                            <asp:ListItem Value="20">Top 20</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="2" style="text-align: left">
                        <label class="checkbox" style="display: inline-block; margin-left: 15px; color: white">
                            <asp:CheckBox ID="chkExclude" runat="server" meta:resourcekey="chkExcludeResource1" /><%=GetLocalResourceObject("Exclude")%>
                        </label>
                        <asp:Button runat="server" Text="<%$Resources:CommanResource, Process %>" class="btn btn-info btn-sm" ID="btnProcess" OnClick="btnProcess_Click" Style="margin-left: 15px;" OnClientClick="return processValidation();"></asp:Button>
                        <asp:Button runat="server" Text="Export" class="btn btn-info btn-sm" ID="btnExport" OnClick="btnExport_Click" Style="margin-left: 15px;"></asp:Button>
                    </td>
                </tr>
            </table>
            <%-- </div>--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>



    <ul class="nav nav-pills">
        <li class="active"><a class="menuData" data-toggle="tab" href="#menu7"><%=GetLocalResourceObject("Down Time Pareto") %> </a></li>
        <li><a class="menuData" data-toggle="tab" href="#menu0"><%=GetLocalResourceObject("Time-wise") %>     </a></li>
        <li><a class="menuData" data-toggle="tab" href="#menu1"><%=GetLocalResourceObject("Time-wise Freq") %>     </a></li>
        <li><a class="menuData" data-toggle="tab" href="#menu2"><%=GetLocalResourceObject("MCs by Top - 5 Downs") %>     </a></li>
        <li><a class="menuData" data-toggle="tab" href="#menu3"><%=GetLocalResourceObject("Top - 5 Down by MCs") %>     </a></li>
        <li><a class="menuData" data-toggle="tab" href="#menu4"><%=GetLocalResourceObject("Freq-wise") %>     </a></li>
        <li><a class="menuData" data-toggle="tab" href="#menu5"><%=GetLocalResourceObject("MCs by Top-5 Freq") %>     </a></li>
        <li><a class="menuData" data-toggle="tab" href="#menu6"><%=GetLocalResourceObject("Top 5 Freq by MC") %>     </a></li>
        <li class="chartInfo" style="float: right;">* Top selections not applicable for Time-wise, Freq-wise and Time-wise Freq</li>
    </ul>


    <div class="tab-content">

        <div id="menu7" class="tab-pane fade in active">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white">
                <%=GetLocalResourceObject("Down Time Pareto Graph") %>
            </div>
            <div id="container-pie" style="min-width: 310px; height: 300px; margin: 0 auto"></div>
            <div id="container-col" style="min-width: 310px; height: 350px; margin: 0 auto"></div>
            <div id="container2"></div>
            <br />
            <div id="container5"></div>
        </div>

        <div id="menu0" class="tab-pane fade ">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white">
                <%=GetLocalResourceObject("Machine-DownReason-Time Matrix") %>
            </div>
            <div id="tblGrid" class="row manageData gridHeightSetter" style="min-width: 700px; overflow-x: scroll; padding-top: 0px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridTimeWiseInfo" runat="server" CssClass="table table-bordered tbl colorTotal headerFixer" meta:resourcekey="gridTimeWiseInfoResource1">
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div id="menu1" class="tab-pane fade">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white">
                <%=GetLocalResourceObject("Machine-DownReason-Frequency Matrix") %>
            </div>
            <div id="tblGrid1" class="row manageData gridHeightSetter" style="min-width: 700px; overflow-y: auto; overflow-x: scroll; padding-top: 0px;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridTimeWiseFreq" runat="server" CssClass="table table-bordered tbl colorTotal  headerFixer" meta:resourcekey="gridTimeWiseFreqResource1">
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div id="menu2" class="tab-pane fade">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white">
                <%=GetLocalResourceObject("Down Time Comparison Graph") %>
            </div>
            <div id="container0"></div>
        </div>

        <div id="menu3" class="tab-pane fade">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white">
                <%=GetLocalResourceObject("Down Time Comparison Graph") %>
            </div>
            <div id="container1"></div>
        </div>

        <div id="menu4" class="tab-pane fade">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white">
                <%=GetLocalResourceObject("Machine-DownReason-Frequency Matrix") %>
            </div>
            <div id="tblGrid2" class="row manageData gridHeightSetter" style="min-width: 700px; overflow-x: scroll; padding-top: 0px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridFreqWiseInfo" runat="server" CssClass="table table-bordered tbl colorTotal  headerFixer" meta:resourcekey="gridFreqWiseInfoResource1">
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div id="menu5" class="tab-pane fade">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white;"><%=GetLocalResourceObject("Down Frequency Comparison Graph") %></div>
            <div id="container3"></div>
        </div>

        <div id="menu6" class="tab-pane fade">
            <div class="row text-center" style="font-size: 20px; font-weight: 900; color: white"><%=GetLocalResourceObject("Down Frequency Comparison Graph") %></div>
            <div id="container4"></div>
        </div>
    </div>


    <script>


        //console.log("ParentNode" + t.parentNode.title);

        //   var wHeight1 = $(window).height();
        //var wHeight = $(window).height() - (60 + 155 + 80);
        //console.log(wHeight1 + "" + wHeight);
        //if (wHeight == wHeight1) {
        //    alert("Equal");
        //}



        $(document).ready(function () {
            // $.unblockUI({});
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            $('.gridHeightSetter').css('height', screen.height - (135 + 80) + 28);
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });
            <%--$(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top", 
                language:'<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });--%>

            $("a[href$='#menu7']").css("background-color", "#428bca");
            $("a[href$='#menu7']").css("color", "#FFFFFF");
            $(".menuData").click(function () {
                $(".menuData").css("background-color", "");
                $(".menuData").css("color", "");

                $(this).css("background-color", "#428bca");
                $(this).css("color", "#FFFFFF");
            });

            var tdnum = $('#MainContent_gridTimeWiseFreq tr:last-child').find('td').length - 1;
            $('.colorTotal  tr:last-child').find('td:eq(' + tdnum + ')').css("background-color", "blue");
            $('.colorTotal  tr:last-child').find('td:eq(' + tdnum + ')').css("color", "white");

            BindGraph();

            var newWidth = ($(window).width() - 180);
            $(".manageData").width(newWidth);

            $("#liMenu").click(function () {
                newWidth = $(window).width();
                var widthMenu = $("#sidebar").width();
                if (widthMenu == 180) {
                    $(".manageData").width($(window).width() - 46);
                } else {
                    $(".manageData").width($(window).width() - 180);
                }
                var chart = $('#container0').highcharts();
                chart.reflow();

                var chart1 = $('#container1').highcharts();
                chart1.reflow();

                var chart2 = $('#container-pie').highcharts();
                chart2.reflow();

                var chart6 = $('#container-col').highcharts();
                chart6.reflow();

                var chart3 = $('#container3').highcharts();
                chart3.reflow();

                var chart4 = $('#container4').highcharts();
                chart4.reflow();

                var chart5 = $('#container5').highcharts();
                chart5.reflow();
            });

            //$('[id$=ddlPlantId]').change(function(){
            //    var value = $('[id$=ddlPlantId]').val();
            //    $.ajax({
            //        type: "POST",
            //        contentType: "application/json; charset=utf-8",
            //        url: "MachineDownReasonTimeMatrix.aspx/GetCellIdData",
            //        data: '{PlantId:"' + value + '"}',
            //        dataType: "json",
            //        success: function (result) {
            //            
            //            var cellIdList = result.d;
            //            if (cellIdList.length > 0) {
            //                $("[id$=ddlCellID]").empty();
            //                for (var i = 0; i < cellIdList.length; i++) {
            //                    $("[id$=ddlCellID]").append('<option value="' + cellIdList[i] + '">' + cellIdList[i] + '</option>');
            //                }
            //            }
            //        },
            //        error: function (Result) {
            //            alert("Error");
            //        }
            //    });
            //});

            //$('#ddlCellID').change(function(){
            //    var value = $('[id$=ddlCellID]').val();
            //    $.ajax({
            //        type: "POST",
            //        contentType: "application/json; charset=utf-8",
            //        url: "MachineDownReasonTimeMatrix.aspx/GetMachineIdData",
            //        data: '{CellId:"' + value + '", PlantId:"' + $("[id$=ddlPlantId]").val() + '"}',
            //        dataType: "json",
            //        success: function (result) {
            //            
            //            var machineIdList = result.d;
            //            if (machineIdList.length > 0) {
            //                $('#ddlMachineId').empty();
            //                for (var i = 0; i < machineIdList.length; i++) {
            //                    $("#ddlMachineId").append('<option value="' + machineIdList[i] + '">' + machineIdList[i] + '</option>');
            //                }
            //            }
            //        },
            //        error: function (Result) {
            //            alert("Error");
            //        }
            //    });
            //});
        });

        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        function compareDates(date1, date2) {
            debugger;
            var d1 = Date.parse(date1);
            var d2 = Date.parse(date2);
            return d2 - d1;
        }
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }
        function processValidation() {
            debugger;
            $("[id$=hdfValue]").val("OK");
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
            var from = $("[id$=txtFromDate]").val().split(" ")[0];
            var to = $('[id$=txtToDate]').val().split(" ")[0];


            var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
            var dateInterval =<%= DateTimeIntarvel %>;
            if (diffe > (dateInterval - 1)) {

                alert("Difference between to date and from date cannot be more than " + dateInterval + " days.");
                $("[id$=txtToDate]").focus();
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });

            $("a[href$='#menu2']").text("MCs by Top - " + $("[id$=ddlTopDownReasons]").val() + " Downs");
            $("a[href$='#menu3']").text("Top - " + $("[id$=ddlTopDownReasons]").val() + " Down by MCs");
            $("a[href$='#menu5']").text("MCs by Top - " + $("[id$=ddlTopDownReasons]").val() + " Freq");
            $("a[href$='#menu6']").text("Top - " + $("[id$=ddlTopDownReasons]").val() + " Freq by MCs");
            $("[id$=hdfValue]").val("OK");
            return true;
        }
     <%--   $(document).on("click", "[id$=btnProcess]", function () {
            $("[id$=hdfValue]").val("OK");
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

            var dateCom = compareDates(from, to);
            if (dateCom <= 0) {

                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            else if (dateCom > 30) {

                alert("Difference between to date and from date cannot be more than 31 days.");
                $("[id$=txtToDate]").focus();
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });

            $("a[href$='#menu2']").text("MCs by Top - " + $("[id$=ddlTopDownReasons]").val() + " Downs");
            $("a[href$='#menu3']").text("Top - " + $("[id$=ddlTopDownReasons]").val() + " Down by MCs");
            $("a[href$='#menu5']").text("MCs by Top - " + $("[id$=ddlTopDownReasons]").val() + " Freq");
            $("a[href$='#menu6']").text("Top - " + $("[id$=ddlTopDownReasons]").val() + " Freq by MCs");
            $("[id$=hdfValue]").val("OK");
        });--%>

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // $.unblockUI({});
            $('.gridHeightSetter').css('height', screen.height - (135 + 80) + 28);
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlMultiDownID]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            $('.gridHeightSetter').css('height', screen.height - (135 + 80) + 28);
            <%--$(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language:'<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });--%>

            var tdnum = $('#MainContent_gridTimeWiseFreq tr:last-child').find('td').length - 1;
            $('.colorTotal  tr:last-child').find('td:eq(' + tdnum + ')').css("background-color", "blue");
            $('.colorTotal  tr:last-child').find('td:eq(' + tdnum + ')').css("color", "white");
            BindGraph();

        })

        function BindGraph() {
            if ($("[id$=hdfValue]").val() == "OK") {
                GetFirstDownTimeData();
                GetSecondDownTimeData();
                GetFirstDownFreqData();
                GetSecondDownFreqData();
                GetDownTimeParetoData();
                GetCatagoryDownTimeParetoData();
                $.unblockUI({}); $("[id$=hdfValue]").val("NOTOK");
            }
        }

        $(document).on("click", "#liMenu", function () {
            var chart = $('#container0').highcharts();
            chart.reflow();

            var chart1 = $('#container1').highcharts();
            chart1.reflow();

            var chart2 = $('#container-pie').highcharts();
            chart2.reflow();

            var chart6 = $('#container-col').highcharts();
            chart6.reflow();

            var chart3 = $('#container3').highcharts();
            chart3.reflow();

            var chart4 = $('#container4').highcharts();
            chart4.reflow();

            var chart5 = $('#container5').highcharts();
            chart5.reflow();
        });


        function GetFirstDownTimeData() {
            $.ajax({
                type: "POST",
                url: "MachineDownReasonTimeMatrix.aspx/MCsbyTopDownsChart",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",downID:"' + $("[id$=ddlMultiDownID]").val() + '",exclude:"' + $("[id$=chkExclude]").prop('checked') + '",downReasons:"' + $("[id$=ddlTopDownReasons]").val() + '",CellID:"' + $("[id$=ddlCellID]").val() + '"}',
                dataType: "json",
                success: OnSuccessGetFirst,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function OnSuccessGetFirst(Result) {
            var data = Result.d;
            var size =  <%= fontSize %>;
            var sizeLable = (size - 3 + "px");
            var sizeText = (size + "px");
            bindChartTimeWiseFun(data, sizeLable);
        }


        function GetSecondDownTimeData() {
            $.ajax({
                type: "POST",
                url: "MachineDownReasonTimeMatrix.aspx/TopDownbyMCChart",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",downID:"' + $("[id$=ddlMultiDownID]").val() + '",exclude:"' + $("[id$=chkExclude]").prop('checked') + '",downReasons:"' + $("[id$=ddlTopDownReasons]").val() + '",CellID:"' + $("[id$=ddlCellID]").val() + '"}',
                dataType: "json",
                success: OnSuccessGetSecond,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function OnSuccessGetSecond(Result) {
            var data = Result.d;
            var size =  <%= fontSize %>;
            var sizeLable = (size - 3 + "px");
            var sizeText = (size + "px");
            bindTop5DownbyMCChart(data, sizeLable);
        }


        function bindTop5DownbyMCChart(data, sizeLable) {
            $('#container1').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    height: 550
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                yAxis: {
                    min: 0,
                    title: {
                        //text: '<%=GetLocalResourceObject("DownTime")%>',
                        text: 'DownTime (hh:mm:ss)',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    }, labels: {
                        style: {
                            fontSize: sizeLable
                        },
                        formatter: function () {
                            var time = this.value;
                            var hours1 = parseInt(time / (60 * 60));
                            var mins1 = parseInt((parseInt(time / 60)) % 60);
                            var secs1 = parseInt(time % (60));
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                        }
                    },
                },
                legend: {
                    verticalAlign: 'top',
                    align: 'right',
                    floating: true,
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false
                },
                tooltip: {
                    enabled: true,
                    formatter: function () {
                        var time = this.y;
                        //alert(time);
                        var hours1 = parseInt(time / (3600));
                        var mins1 = parseInt((parseInt(time / 60)) % 60);
                        //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        //alert(hours1 + ':' + mins1);
                        var secs1 = parseInt(time % (60));
                        return '<b>' + this.series.name + '</b><br/>' +
                            this.x + ': ' + (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: true
                    }
                },
                series: data.seriesTimeWise
            });
        }


        function bindChartTimeWiseFun(data, sizeLable) {
            $('#container0').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    height: 550
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '<%=GetLocalResourceObject("Time1")%>',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    }
                    , labels: {
                        style: {
                            fontSize: sizeLable
                        },
                        formatter: function () {
                            var time = this.value;
                            var hours1 = parseInt(time / (60 * 60));
                            var mins1 = parseInt((parseInt(time / 60)) % 60);
                            var secs1 = parseInt(time % (60));
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                        }
                    }
                },
                legend: {
                    verticalAlign: 'top',
                    align: 'right',
                    floating: true,
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false
                },
                tooltip: {
                    enabled: true,
                    formatter: function () {
                        var time = this.y;
                        //alert(time);
                        var hours1 = parseInt(time / (3600));
                        var mins1 = parseInt((parseInt(time / 60)) % 60);
                        //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        //alert(hours1 + ':' + mins1);
                        var secs1 = parseInt(time % (60));
                        return '<b>' + this.series.name + '</b><br/>' +
                            this.x + ': ' + (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: true
                    }
                },
                series: data.seriesTimeWise,
            });
        }

        function GetFirstDownFreqData() {
            $.ajax({
                type: "POST",
                url: "MachineDownReasonTimeMatrix.aspx/MCsbyTopDownFreqChart",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",downID:"' + $("[id$=ddlMultiDownID]").val() + '",exclude:"' + $("[id$=chkExclude]").prop('checked') + '",downReasons:"' + $("[id$=ddlTopDownReasons]").val() + '",CellID:"' + $("[id$=ddlCellID]").val() + '"}',
                dataType: "json",
                success: OnSuccessFirstDownFreq,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function OnSuccessFirstDownFreq(Result) {
            var data = Result.d;
            var size =  <%= fontSize %>;
            var sizeLable = (size - 3 + "px");
            var sizeText = (size + "px");
            bindChartDownFreqFun(data, sizeLable);
        }


        function GetSecondDownFreqData() {
            $.ajax({
                type: "POST",
                url: "MachineDownReasonTimeMatrix.aspx/TopDownFreqbyMCsChart",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",downID:"' + $("[id$=ddlMultiDownID]").val() + '",exclude:"' + $("[id$=chkExclude]").prop('checked') + '",downReasons:"' + $("[id$=ddlTopDownReasons]").val() + '",CellID:"' + $("[id$=ddlCellID]").val() + '"}',
                dataType: "json",
                success: OnSuccessSecondDownFreq,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function OnSuccessSecondDownFreq(Result) {
            var data = Result.d;
            var size =  <%= fontSize %>;
            var sizeLable = (size - 3 + "px");
            var sizeText = (size + "px");
            bindTop5DownFreqbyMCChart(data, sizeLable);
        }

        function bindChartDownFreqFun(data, sizeLable) {
            $('#container3').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    height: 550
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '<%=GetLocalResourceObject("DownFrequency")%>',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    }
                },
                legend: {
                    verticalAlign: 'top',
                    align: 'right',
                    floating: true,
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false
                },
                tooltip: {
                    enabled: true
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: true
                    }
                },
                series: data.seriesTimeWise
            });
        }

        function bindTop5DownFreqbyMCChart(data, sizeLable) {
            $('#container4').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    height: 550
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '<%=GetLocalResourceObject("DownFrequency")%>',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    }
                },
                legend: {
                    verticalAlign: 'top',
                    align: 'right',
                    floating: true,
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false
                },
                tooltip: {
                    enabled: true
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: true
                    }
                },
                series: data.seriesTimeWise
            });
        }

        function GetDownTimeParetoData() {
            $.ajax({
                type: "POST",
                url: "MachineDownReasonTimeMatrix.aspx/TopDownTimeParetoChartColAndPie",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",downID:"' + $("[id$=ddlMultiDownID]").val() + '",exclude:"' + $("[id$=chkExclude]").prop('checked') + '",downReasons:"' + $("[id$=ddlTopDownReasons]").val() + '",CellID:"' + $("[id$=ddlCellID]").val() + '"}',
                dataType: "json",
                success: OnSuccessDownTimePareto,
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function GetCatagoryDownTimeParetoData() {
            $.ajax({
                type: "POST",
                url: "MachineDownReasonTimeMatrix.aspx/CatagoryByDownTimeParetoChart",
                contentType: "application/json; charset=utf-8",
                data: '{plantID:"' + $("[id$=ddlPlantId]").val() + '",machineId:"' + $("[id$=ddlMachineId]").val() + '",startFromDate:"' + $("[id$=txtFromDate]").val() + '",startToDate:"' + $("[id$=txtToDate]").val() + '",downID:"' + $("[id$=ddlMultiDownID]").val() + '",exclude:"' + $("[id$=chkExclude]").prop('checked') + '",downReasons:"' + $("[id$=ddlTopDownReasons]").val() + '",CellID:"' + $("[id$=ddlCellID]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var size =  <%= fontSize %>;
                    var sizeLable = (size - 3 + "px");
                    var sizeText = (size + "px");
                    BindCatagoryDownData(response.d, sizeLable);
                },
                error: function (response) {
                    console.log(response.d);
                }
            });
        }

        function BindCatagoryDownData(data, sizeLable) {
            var catValue = '';
            var cont = 0;
            var chart = Highcharts.chart('container5', {
                //$('#container2').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    //events: {
                    //    drillup: function (e) {
                    //        
                    //        if (e.seriesOptions.id == undefined) {
                    //            chart.xAxis[0].axisTitle.attr({
                    //                text: data.XAxisTitle
                    //            });
                    //        }
                    //    },
                    //    drilldown: function (e) {
                    //        //alert(this.series[0].data[e.point.x].name);    
                    //        
                    //        chart.xAxis[0].axisTitle.attr({
                    //            text: data.XAxisTitle + ' - ' + this.series[0].data[e.point.x].name
                    //        });
                    //    },

                    //}
                    events: {
                        drillup: function (e) {
                            if (e.seriesOptions.id == undefined) {

                                //catValue = '';
                                //contValue = 0;
                                //chart.xAxis[0].setCategories(data.categories);
                                //chart.series[2].setName("Down ID");
                                // chart.xAxis[0].setCategories(data.categories);
                            }
                        },
                        drilldown: function (e) {

                            //var ss = this.series[1].name;

                            //chart.xAxis[0].setCategories(catValue);
                            // chart.series[1].setName("Other");
                        },
                    }
                },
                title: {
                    text: ''
                },
                xAxis: {
                    //categories: data.categories,
                    //categories: ['FF', 'QUALITY', 'TRAINER', 'SUPERVISOR', 'MAINTAINENCE', 'MANAGEMENT'],
                    ///['Overpriced', 'Small portions', 'Wait time', 'Food is tasteless', 'No atmosphere', 'Not clean', 'Too noisy', 'Unfriendly staff']
                    type: 'category',
                    labels: {
                        //rotation: 75,
                        style: {
                            fontSize: sizeLable,
                        }
                    },
                }, tooltip: {
                    enabled: true,
                    formatter: function () {
                        var value = '';
                        if (this.series.name == 'Pareto') {
                            value = this.y;
                        } else {
                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        }
                        return '<b>' + this.series.name + '</b><br/>' + //'<span style="color:{point.color}">{point.name}</span>: <b>' + value + '</b><br/>'
                            this.point.name + ': ' + value;
                    }
                },
                yAxis: [{
                    title: {
                        text: '<%=GetLocalResourceObject("Time")%>',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    }, labels: {
                        style: {
                            fontSize: sizeLable
                        },
                        formatter: function () {
                            var time = this.value;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        }
                    }
                }, {
                        title: {
                            text: ''
                        },
                    }],
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                var value = '';
                                if (this.series.name != 'Pareto') {
                                    //    value = this.y;
                                    //} else {
                                    var time = this.y;
                                    var hours1 = parseInt(time / 60);
                                    var mins1 = parseInt(time % 60);
                                    value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                }
                                return value;
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        },
                    }
                },
                series: data.series,
                drilldown: {
                    series: data.drilldown,
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        }
                    }
                }
                //series: [{
                //    type: 'pareto',
                //    name: 'Pareto',
                //    yAxis: 1,
                //    zIndex: 10,
                //    baseSeries: 1,
                //}, {
                //    name: 'Down ID',
                //    type: 'column',
                //    //colorByPoint: true,
                //    zIndex: 2,
                //    data: data.series,
                //    //dataLabels: {
                //    //    enabled: true,
                //    //    formatter: function () {
                //    //        var time = this.y;
                //    //        var hours1 = parseInt(time / 60);
                //    //        var mins1 = parseInt(time % 60);
                //    //        return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                //    //    }
                //    //}
                //}],
                //drilldown: {
                //    series: data.drilldown,
                //    //dataLabels: {
                //    //    enabled: true,
                //    //    formatter: function () {
                //    //        var time = this.y;
                //    //        var hours1 = parseInt(time / 60);
                //    //        var mins1 = parseInt(time % 60);
                //    //        return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                //    //    }
                //    //}
                //}
            });
        }

        function OnSuccessDownTimePareto(Result) {
            var data = Result.d;
            var size =  <%= fontSize %>;
            var sizeLable = (size - 3 + "px");
            var sizeText = (size + "px");
            BindColumnAndPieChart(data, sizeLable);
            // BindDownTimeParetoData(data,sizeLable);
        }

        function BindDownTimeParetoData(data, sizeLable) {

            console.log(data);
            var catValue = '';
            var cont = 0;
            var chart = Highcharts.chart('container2', {
                //$('#container2').highcharts({
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    events: {
                        drillup: function (e) {
                            if (e.seriesOptions.id == undefined) {

                                catValue = '';
                                contValue = 0;
                                chart.xAxis[0].setCategories(data.categories);
                                chart.series[2].setName("Down ID");
                            }
                        },
                        drilldown: function (e) {

                            //var ss = this.series[1].name;
                            contValue = $("[id$=ddlTopDownReasons]").val();
                            contValue = parseInt(contValue) + 1;
                            var catValue = data.categories;
                            catValue = catValue.slice(contValue);
                            chart.xAxis[0].setCategories(catValue);
                            chart.series[1].setName("Other");
                        },
                    }
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                    ///['Overpriced', 'Small portions', 'Wait time', 'Food is tasteless', 'No atmosphere', 'Not clean', 'Too noisy', 'Unfriendly staff']
                }, tooltip: {
                    enabled: true,
                    formatter: function () {
                        var value = '';
                        if (this.series.name == '<%=GetLocalResourceObject("Pareto")%>') {
                            value = this.y;
                        } else {
                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        }
                        return '<b>' + this.series.name + '</b><br/>' +
                            this.x + ': ' + value;
                    }
                },
                yAxis: [{
                    title: {
                        text: '<%=GetLocalResourceObject("Time")%>',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    }, labels: {
                        style: {
                            fontSize: sizeLable
                        },
                        formatter: function () {
                            var time = this.value;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        }
                    }
                }, {
                        title: {
                            text: ''
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        labels: {
                            format: "{value}%"
                        }
                    }],
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                var value = '';
                                if (this.series.name != '<%=GetLocalResourceObject("Pareto")%>') {
                                    //    value = this.y;
                                    //} else {
                                    var time = this.y;
                                    var hours1 = parseInt(time / 60);
                                    var mins1 = parseInt(time % 60);
                                    value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                }
                                return value;
                            },
                            style: {
                                fontSize: sizeLable
                            }
                        }
                    }
                },
                series: [{
                    type: 'pareto',
                    name: '<%=GetLocalResourceObject("Pareto")%>',
                    yAxis: 1,
                    zIndex: 10,
                    baseSeries: 1,
                }, {
                        name: '<%=GetLocalResourceObject("DownID")%>',
                        type: 'column',
                        //colorByPoint: true,
                        zIndex: 2,
                        data: data.series,
                        //dataLabels: {
                        //    enabled: true,
                        //    formatter: function () {
                        //        var time = this.y;
                        //        var hours1 = parseInt(time / 60);
                        //        var mins1 = parseInt(time % 60);
                        //        return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                        //    }
                        //}
                    }],
                drilldown: {
                    series: data.drilldown,
                    //dataLabels: {
                    //    enabled: true,
                    //    formatter: function () {
                    //        var time = this.y;
                    //        var hours1 = parseInt(time / 60);
                    //        var mins1 = parseInt(time % 60);
                    //        return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                    //    }
                    //}
                }
            });
        }

        function BindColumnAndPieChart(data, sizeLable) {

            console.log(data);
            var catValue = '';
            var cont = 0;
            var increas = 0;
            var charts = '';
            var drilldown = function (e, point, type) {
                if (type == "col") {
                    var charts = [$('#container-col').highcharts(), $('#container-pie').highcharts()];//Highcharts.charts;
                    pointName = e.point.name;
                    point.options.chart.drilled = true;
                    Highcharts.each(charts, function (c, i) {
                        if (!c.options.chart.drilled) {
                            c.options.chart.drilled = true;
                            Highcharts.each(c.series[0].data, function (p, j) {

                                if (p.name === pointName) {
                                    p.doDrilldown();
                                }
                            });
                        }
                    });
                } else {
                    var charts = [$('#container-col').highcharts(), $('#container-pie').highcharts()]; //Highcharts.charts;
                    pointName = e.point.name;
                    point.options.chart.drilled = true;
                    Highcharts.each(charts, function (c, i) {
                        if (!c.options.chart.drilled) {
                            c.options.chart.drilled = true;
                            Highcharts.each(c.series[1].data, function (p, j) {
                                if (p.name === pointName) {
                                    p.doDrilldown();
                                }
                            });
                        }
                    });
                }
            };

            var chartCol = Highcharts.chart('container-col', {
                colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                    '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                    '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'column',
                    drilled: false,
                    events: {
                        drilldown: function (e) {
                            drilldown(e, this, "col");

                            cont = $("[id$=ddlTopDownReasons]").val();
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(cont);
                            chartCol.xAxis[0].setCategories(catValue);
                            //chartCol.series[0].hide(true);
                            //chartCol.yAxis[1].enabled(true);
                        },
                        drillup: function (e) {

                            //chartCol.series[0].show(true);
                            cont = $("[id$=ddlTopDownReasons]").val();
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(0, cont);
                            chartCol.xAxis[0].setCategories(catValue);

                            var chart2 = $('#container-pie').highcharts();//Highcharts.charts[1];
                            if (chart2.options.chart.drilled) {
                                this.options.chart.drilled = false;
                                chart2.options.chart.drilled = false;
                                chart2.drillUp();
                            }
                            chartCol.xAxis[0].setCategories(catValue);
                            chartCol.series[1].setData(data.series);
                            chartCol.series[2].setName("Down ID");
                            //chartCol.series=data.series;
                            //chartCol.redraw();

                        }
                    }
                },
                title: {
                    text: ''
                }, xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                    //type: 'category',
                }, tooltip: {
                    enabled: true,
                    formatter: function () {
                        var value = 0;
                        if (this.series.name == '<%=GetLocalResourceObject("Pareto")%>') {
                            value = (this.y).toFixed(2);
                            return '<b>' + this.series.name + '</b><br/>' +
                                "Cumulative Value (%)" + ': ' + value;
                        } else {

                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            //var hours1 = parseInt(time / (60*60));
                            //var mins1 = parseInt((parseInt(time % 60*60)) / 60);
                            //return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            return '<b>' + this.series.name + '</b><br/>' +
                                this.point.name + ': ' + value;
                        }

                    }
                },
                yAxis: [{
                    title: {
                        text: '<%=GetLocalResourceObject("Time")%>',
                        style: {
                            color: '#525151',
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif',
                            fontWeight: 'bold'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        },
                        formatter: function () {
                            var value = '';
                            //if (this.chart.series[0].name == 'Pareto') {
                            //value = this.y;
                            //} else {
                            var time = this.value;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            // }
                            return value;
                        }
                    }
                }, {
                        title: {
                            text: ''
                        },
                        minPadding: 0,
                        maxPadding: 0,
                        max: 100,
                        min: 0,
                        opposite: true,
                        tickInterval: 25,
                        labels: {
                            format: "{value}%"
                        }
                    }],
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                var value = '';
                                if (this.series.name != '<%=GetLocalResourceObject("Pareto")%>') {
                                    //    value = this.y;
                                    //} else {
                                    var time = this.y;
                                    var hours1 = parseInt(time / 60);
                                    var mins1 = parseInt(time % 60);
                                    value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                }
                                return value;
                            }
                        }, style: {
                            fontSize: sizeLable
                        }
                    }
                },
                series: [{
                    type: 'pareto',//pareto
                    name: '<%=GetLocalResourceObject("Pareto")%>',
                    yAxis: 1,
                    zIndex: 10,
                    baseSeries: 1

                }, {
                        name: '<%=GetLocalResourceObject("DownID")%>',
                        type: 'column',
                        colorByPoint: true,
                        zIndex: 2,
                        data: data.series,
                    }],
                drilldown: {
                    series: data.drilldown,
                }
            });

            var chartPie = Highcharts.chart('container-pie', {
                colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#a6c96a',
                    '#F15C80', '#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4', '#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE',
                    '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'],
                credits: { enabled: false },
                tooltip: { enabled: false },
                exporting: { enabled: false },
                chart: {
                    type: 'pie',
                    drilled: false,
                    events: {
                        drilldown: function (e) {
                            drilldown(e, this, "pie");
                            cont = $("[id$=ddlTopDownReasons]").val();
                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(cont);
                            chartPie.xAxis[0].setCategories(catValue);
                        },
                        drillup: function (e) {
                            cont = $("[id$=ddlTopDownReasons]").val();

                            cont = parseInt(cont) + 1;
                            catValue = data.categories;
                            catValue = catValue.slice(0, cont);
                            chartPie.xAxis[0].setCategories(catValue);

                            var chart = $('#container-col').highcharts();//Highcharts.charts[0];

                            if (chart.options.chart.drilled) {
                                this.options.chart.drilled = false;
                                chart.options.chart.drilled = false;
                                chart.drillUp();
                            }
                        }
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: false,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                var value = '';
                                if (this.series.name != '<%=GetLocalResourceObject("Pareto")%>') {
                                    //    value = this.y;
                                    //} else {
                                    var time = this.y;
                                    var hours1 = parseInt(time / 60);
                                    var mins1 = parseInt(time % 60);
                                    value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                                    value = this.point.name + " : " + value + "  (" + this.percentage.toFixed(1) + "%)";
                                }
                                return value;
                            },
                            //format: '<b>{point.name}</b>: {point.y:.2f} ({point.percentage:.1f}) %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        },
                        showInLegend: true
                    },
                    //series: {
                    //    cursor: 'pointer',
                    //    dataLabels: {
                    //        enabled: true,
                    //        //format: '{point.y:.1f}',
                    //        formatter: function () {
                    //            return Highcharts.numberFormat(this.y, 0);
                    //        },
                    //        showInLegend: false
                    //    },
                    //},
                },
                tooltip: {
                    formatter: function () {
                        var value = '';
                        if (this.series.name != '<%=GetLocalResourceObject("Pareto")%>') {
                            //    value = this.y;
                            //} else {
                            var time = this.y;
                            var hours1 = parseInt(time / 60);
                            var mins1 = parseInt(time % 60);
                            value = (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1);
                            value = this.point.name + " : " + value + "  (" + this.percentage.toFixed(1) + "%)";
                        }
                        return value;
                    }
                    // headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    //pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b> (<b>{point.percentage:.1f}%</b>) <br/>'
                },
                xAxis: {
                    categories: data.categories,
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                    //type: 'category',
                },
                title: {
                    text: ''
                },
                series: [{
                    name: '<%=GetLocalResourceObject("DownID")%>',
                    data: data.series,
                }],
                drilldown: {
                    series: data.drilldown,
                }
            });

        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
