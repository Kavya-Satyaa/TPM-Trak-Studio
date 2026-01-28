<%@ Page Title="Data Analysis" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="DataAnalysis.aspx.cs" Inherits="Web_TPMTrakDashboard.DataAnalysis" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <%--  <link href="MyCssAndJS/css/bootstrap.min.css" rel="stylesheet" />--%>
    <%--<link href="MyCssAndJS/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="MyCssAndJS/js/jquery.js"></script>
    <script src="MyCssAndJS/js/bootstrap.min.js"></script>
    <script src="MyCssAndJS/js/bootstrap-datepicker.js"></script>--%>


    <%--    <script src="MyCssAndJS/DatePicker/jquery-2.1.1.min.js"></script>
    <script src="MyCssAndJS/DatePicker/moment-with-locales.js"></script>
    <script src="MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"></script>
    <link href="MyCssAndJS/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />--%>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>


    <%-----------Highchrat config-------------%>
    <%--<script src="ChartScript/jquery.min.js"></script>--%>


    <style>
        .col-lg-4 {
            padding-left: 1px;
            padding-right: 1px;
        }

        th {
            cursor: pointer;
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

        .panel {
            margin-bottom: 2px;
        }

        .nav > li > a {
            padding: 7px 12px;
        }

        .gridview {
            background-color: #fff;
            padding: 2px;
            margin: 2% auto;
        }

            .gridview a {
                margin: auto 1%;
                border-radius: 50%;
                background-color: #444;
                padding: 5px 10px 5px 10px;
                color: #fff;
                text-decoration: none;
                -o-box-shadow: 1px 1px 1px #111;
                -moz-box-shadow: 1px 1px 1px #111;
                -webkit-box-shadow: 1px 1px 1px #111;
                box-shadow: 1px 1px 1px #111;
                margin-right: 3px;
            }

                .gridview a:hover {
                    background-color: #1e8d12;
                    color: #fff;
                }

            .gridview span {
                background-color: #337AB7;
                color: #fff;
                -o-box-shadow: 1px 1px 1px #111;
                -moz-box-shadow: 1px 1px 1px #111;
                -webkit-box-shadow: 1px 1px 1px #111;
                box-shadow: 1px 1px 1px #111;
                border-radius: 50%;
                padding: 5px 10px 5px 10px;
                margin-right: 3px;
            }
        /*/--------------Boostrap css--------------/*/
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }
    </style>

    <div class="row text-center">
        <asp:Label ID="lblMessages" runat="server" EnableViewState="False" Style="font-weight: bold; font-family: Calibri; color: red;" meta:resourcekey="lblMessagesResource1"></asp:Label>
        <asp:HiddenField runat="server" ID="hdfValue" Value="Production Data" />
    </div>

    <div class="row">
        <asp:UpdatePanel ID="updatePanal1" runat="server">
            <ContentTemplate>
                <div class="col-lg-4">
                    <div class="panel panel-primary" style="height: 192px;">
                        <div class="panel-heading"><b><%=GetLocalResourceObject("updatePanal1Heading") %></b> </div>
                        <table class="table table-bordered">
                            <tr>
                                <td><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b>
                                </td>
                                <td class="input-group">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" placeholder="DD-MMM-YYYY" meta:resourcekey="txtFromDateResource1"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b>
                                </td>
                                <td class="input-group">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" placeholder="DD-MMM-YYYY" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlComponent" runat="server" CssClass="form-control input-sm" meta:resourcekey="ddlComponentResource1">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:Button runat="server" ID="btnRefresh" Text="<%$Resources:CommanResource, Refresh %>" OnClick="btnRefresh_Click" CssClass="btn btn-sm btn-primary" /></td>
                                </td>
                                
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updatePanal2" runat="server">
            <ContentTemplate>
                <div class="col-lg-4">
                    <div class="panel panel-primary" style="height: 192px;">
                        <div class="panel-heading"><b><%=GetLocalResourceObject("updatePanal2Heading") %></b></div>
                        <table class="table table-bordered">
                            <tr>
                                <td><b><%=GetLocalResourceObject("bProgramStart") %></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblProgramStart" runat="server" Text="---" meta:resourcekey="lblProgramStartResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("bDownRecord") %></b>
                                </td>
                                <td>
                                   
                                     <asp:Label ID="lblStoppageRecord" runat="server" Text="---" meta:resourcekey="lblStoppageRecordResource1"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("bProdRecord") %></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblProductionRecord" runat="server" Text="---" meta:resourcekey="lblProductionRecordResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("bInCycleStoppage") %></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblInCycleStoppage" runat="server" Text="---" meta:resourcekey="lblInCycleStoppageResource1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class="col-lg-4">
                    <div class="panel panel-primary" style="height: 192px;">
                        <div class="panel-heading"><b><%=GetLocalResourceObject("UpdatePanel3Heading") %></b></div>
                        <table class="table table-bordered">

                            <tr>
                                <td><b><%=GetLocalResourceObject("bProdRecStart") %></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblProductionRecordAuto" runat="server" Text="---" meta:resourcekey="lblProductionRecordAutoResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("bProdRecEnd") %></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblProductionRecordEnd" runat="server" Text="---" meta:resourcekey="lblProductionRecordEndResource1"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("bDownRecStart") %></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblStoppageRecordStart" runat="server" Text="---" meta:resourcekey="lblStoppageRecordStartResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b><%=GetLocalResourceObject("bDownRecEnd") %></b>
                                </td>
                                <td>
                                    <asp:Label ID="lblStoppageRecordEnd" runat="server" Text="---" meta:resourcekey="lblStoppageRecordEndResource1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row">
        <div class="panel panel-primary">
            <div class="panel-heading" style="background-color: black;">
                <div class="col-lg-4" style="margin-left: -15px;">
                    <input type="button" class="btn btn-sm btn-primary" value="<%=GetLocalResourceObject("btnRawData") %>" id="btnRowData" style="margin-bottom: 5px; height: 30px;" />
                    <input type="button" class="btn btn-sm btn-default" value="<%=GetLocalResourceObject("btnAutoData") %>" id="btnAutoData" style="margin-bottom: 5px; height: 30px;" />
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-lg-12" style="padding: 0 1px 0 1px">
            <div class="panel panel-primary" id="divRDScroll" style="overflow-x: hidden; overflow-y: scroll; min-height: 100px; max-height: 580px; width: 100%;">
                <asp:UpdatePanel ID="updatePanal4" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridRowData" runat="server" AllowSorting="True" AutoGenerateColumns="False" OnSorting="gridrowData_Sorting" AllowPaging="True" OnPageIndexChanging="OnPageIndexChangingRowData" PageSize="1000" CssClass="table table-condensed border" meta:resourcekey="gridRowDataResource1">
                            <PagerStyle CssClass="pagination-ys" />
                            <AlternatingRowStyle BackColor="#CCFFFF" />
                            <Columns>
                                <asp:BoundField DataField="SerialNo" HeaderText="Sl No" SortExpression="SerialNo" meta:resourcekey="BoundFieldResource1" />
                                <asp:BoundField DataField="ComponentID" HeaderText="ComponentID" SortExpression="ComponentID" meta:resourcekey="BoundFieldResource2" />
                                <asp:BoundField DataField="Comp" HeaderText="ComponentNo" SortExpression="Comp" meta:resourcekey="BoundFieldResource3" />
                                <asp:BoundField DataField="OperationNo" HeaderText="Operation No" SortExpression="OperationNo" meta:resourcekey="BoundFieldResource4" />
                                <asp:BoundField DataField="Opr" HeaderText="OperatorID" SortExpression="opn" meta:resourcekey="BoundFieldResource5" />
                                <asp:BoundField DataField="StTime" HeaderText="Start Time" SortExpression="StTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource6" />
                                <asp:BoundField DataField="NdTime" HeaderText="End Time" SortExpression="NdTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource7" />
                                <asp:BoundField DataField="DataType" HeaderText="Data Type" SortExpression="DataType" meta:resourcekey="BoundFieldResource8" />
                            </Columns>
                            <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="panel panel-primary" id="divADScroll" style="overflow-x: hidden; overflow-y: scroll; min-height: 100px; max-height: 580px; width: 100%;">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gridAutoData" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gridAutoData_Sorting"
                            AllowPaging="True" OnPageIndexChanging="OnPageIndexChangingAutoData" PageSize="1000" CssClass="table table-condensed border" meta:resourcekey="gridAutoDataResource1">
                            <PagerStyle CssClass="pagination-ys" />
                            <AlternatingRowStyle BackColor="#CCFFFF" />
                            <Columns>
                                <asp:BoundField DataField="SerialNo" HeaderText="SlNo." SortExpression="SerialNo" meta:resourcekey="BoundFieldResource9" />
                                <asp:BoundField DataField="ComponentID" HeaderText="Component ID" SortExpression="ComponentID" meta:resourcekey="BoundFieldResource10" />
                                <asp:BoundField DataField="Comp" HeaderText="Component No." SortExpression="Comp" meta:resourcekey="BoundFieldResource11" />
                                <asp:BoundField DataField="OperationNo" HeaderText="Operation No." SortExpression="OperationNo" meta:resourcekey="BoundFieldResource12" />
                                <asp:BoundField DataField="Opr" HeaderText="Operator ID" SortExpression="opn" meta:resourcekey="BoundFieldResource13" />
                                <asp:BoundField DataField="StTime" HeaderText="Start Time" SortExpression="StTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource14" />
                                <asp:BoundField DataField="NdTime" HeaderText="End Time" SortExpression="NdTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource15" />
                                <asp:BoundField DataField="DataType" HeaderText="Data Type" SortExpression="DataType" meta:resourcekey="BoundFieldResource16" />
                            </Columns>
                            <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            $.unblockUI({});
            dateTimePicker();
            $("[id$=PEChart]").css("width", "100%");
            $("[id$=AEChart]").css("width", "100%");
            $("[id$=OEChart]").css("width", "100%");

            //hideAndShowChart();
        });

        $(document).on("click", ".pagination-ys a", function () {
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "th a", function () {
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            dateTimePicker();
        });

        $(document).on("click", "[id$=btnRefresh]", function () {
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
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            // GetProductionChartData();
            //  GetDownTimeChartData();
        });

        function dateTimePicker() {
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
        }

        $("#divRDScroll").show();
        $("#divADScroll").hide();

        $("#btnAutoData").click(function () {
            $("#divRDScroll").hide();
            $("#divADScroll").show();
            $("#btnAutoData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
            $("#btnRowData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
        });

        $("#btnRowData").click(function () {
            $("#divRDScroll").show();
            $("#divADScroll").hide();
            $("#btnAutoData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("#btnRowData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
        });




        //$(document).ready(function () {
        //    $("#gridAutoData").tablesorter();
        //});

    </script>
</asp:Content>

