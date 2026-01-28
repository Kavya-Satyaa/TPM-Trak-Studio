<%@ Page Title="VDG Information" Trace="false" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VDGScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.VDGScreen" meta:resourcekey="PageResource1" AsyncTimeout="6000" %>

<%--AsyncTimeout="60000"--%>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>--%>
    <%--    
    <link href="MyCssAndJS/fontawesome/font-awesome.min.css" rel="stylesheet" />--%>
    <%--  <link href="MyCssAndJS/css/bootstrap.min.css" rel="stylesheet" />--%>
    <%--<link href="MyCssAndJS/css/bootstrap-datepicker.css" rel="stylesheet" />
    <script src="MyCssAndJS/js/jquery.js"></script>
    <script src="MyCssAndJS/js/bootstrap.min.js"></script>
    <script src="MyCssAndJS/js/bootstrap-datepicker.js"></script>--%>

    <%--<link href="MyCssAndJS/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />--%>
    <%--    <script src="MyCssAndJS/DatePicker/jquery-2.1.1.min.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/moment-with-locales.js"></script>--%>
    <%--<script src="MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"></script>--%>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/VDGjs") %>
    <%: Scripts.Render("~/bundles/commanChartjs") %>

    <%-----------Highchrat config-------------%>
    <%--<script src="ChartScript/jquery.min.js"></script>--%>
    <%--  <script src="MyCssAndJS/chartjs/highcharts.js"></script>--%>
    <%--<script src="MyCssAndJS/Charts/highstock.js"></script>--%>
    <%--<script src="MyCssAndJS/Charts/exporting.js"></script>--%>
    <%--<script src="MyCssAndJS/chartjs/data.js"></script>--%>
    <%--<script src="MyCssAndJS/chartjs/drilldown.js"></script>--%>


    <style>
        .hidegraph-eve {
            text-decoration: none;
        }

        .managePanal {
            padding-bottom: 5px;
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

        #DivChart.fullscreen {
            z-index: 9999;
            width: 100vw;
            height: 100vh;
            position: absolute;
            top: 0;
            left: 0;
        }

        .chkUpdateAllMachines label {
            padding-left: 10px;
        }

        .pagination-ys {
            bottom: 0;
            position: sticky;
            background: aliceblue;
        }
    </style>

    <style>
        #testTable {
            width: 300px;
            margin-left: auto;
            margin-right: auto;
        }

        #tablePagination {
            background-color: Transparent;
            font-size: 0.8em;
            padding: 0px 5px;
            height: 20px;
        }

        #tablePagination_paginater {
            margin-left: auto;
            margin-right: auto;
        }

        #tablePagination img {
            padding: 0px 2px;
        }

        #tablePagination_perPage {
            float: left;
        }

        #tablePagination_paginater {
            float: right;
        }



        img#MainContent_PEChart {
            width: 108px;
        }

        #MainContent_gridProductionData {
            text-align: center;
        }

        #MainContent_gridDownTimeData {
            text-align: center;
        }


        /*#gridProductionData tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

         #gridDownTimeData tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }*/

        .custom-menu {
            display: none;
            z-index: 1000;
            position: absolute;
            overflow: hidden;
            border: 1px solid #CCC;
            white-space: nowrap;
            font-family: sans-serif;
            background: #FFF;
            color: #333;
            border-radius: 5px;
            padding-left: 2px;
        }

            .custom-menu li {
                padding: 8px 12px;
                cursor: pointer;
            }

                .custom-menu li:hover {
                    background-color: #DEF;
                }

        #gvModifiedData tr td {
            text-align: center;
            border: 1px solid #c9bebe;
        }
    </style>

    <asp:UpdatePanel runat="server" ID="updatePanelmyDTDG">
        <ContentTemplate>
            <div class="modal fade" id="myDTDG" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title"><%=GetLocalResourceObject("myDTDGTitle")%></h4>
                        </div>
                        <div class="row">
                            <div class="col-lg-5">
                                <div class="panel panel-primary" style="height: 124px;">
                                    <div class="panel-heading"><%=GetLocalResourceObject("myDTDGPanelHeading")%></div>
                                    <div style="padding-left: 4px;">
                                        <input type="radio" name="rdobtnGO" id="chkBarGraph" value="Bar Graph" checked>
                                        <label for="chkBarGraph"><%=GetLocalResourceObject("myDTDGlabelBarGraph")%></label><br>
                                        <input type="radio" name="rdobtnGO" id="chkLineGraph" value="Line Graph">
                                        <label for="chkLineGraph"><%=GetLocalResourceObject("myDTDGlabelLineGraph")%></label>
                                        <br>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-7">
                                <div class="panel panel-primary" style="margin-left: -25px;">
                                    <div class="panel-heading"><%=GetLocalResourceObject("myDTDGProdGraphHeading") %></div>
                                    <div style="padding-left: 4px;">
                                        <input type="radio" id="chkLoadUnLoad" name="rdobtnPG" value="Load UnLoad">
                                        <label for="chkLoadUnLoad"><%=GetLocalResourceObject("myDTDGchkLoadUnLoad") %></label><br>
                                        <input type="radio" id="chkCycleTime" name="rdobtnPG" value="Cycle Time">
                                        <label for="chkCycleTime"><%=GetLocalResourceObject("myDTGchkCycleTime") %></label><br>
                                        <input type="radio" id="chTotalTime" name="rdobtnPG" value="Total Time">
                                        <label for="chkTotalTime">Cycle Time</label><br>
                                        <input type="radio" id="chkAll" name="rdobtnPG" value="All" checked>
                                        <label for="chkAll"><%=GetLocalResourceObject("myDTDGchkAll") %></label><br>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="panel panel-primary">
                                    <div class="panel-heading"><%=GetLocalResourceObject("myDTDGYaxisHeading") %></div>

                                    <table class="table table-bordered">
                                        <tr>
                                            <td><%=GetLocalResourceObject("myDTDGProdGraphHeading") %></td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="text" id="txtProductionGraph" value="0" class="form-control input-sm numbersOnly" style="max-width: 200px;" maxlength="10" /></td>
                                                        <td>
                                                            <button type="button" class="btn btn-info btn-sm" id="btnAplProductionGraph" style="margin-left: 2px;"><%=GetLocalResourceObject("btnAplProductionGraph") %></button></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><%=GetLocalResourceObject("myDTDGDownTimeGraph") %></td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="text" id="txtDownTimeGraph" value="0" class="form-control input-sm numbersOnly" style="max-width: 200px;" maxlength="10" /></td>
                                                        <td>
                                                            <button type="button" class="btn btn-info btn-sm" id="btnApltDownTimeGraph" style="margin-left: 2px;"><%=GetLocalResourceObject("btnAplProductionGraph") %></button></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="updatePanelmyRemarkScreen">
        <ContentTemplate>
            <div class="modal fade" id="myRemarkScreen" role="dialog">
                <div class="modal-dialog modal-sm" style="width: 400px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title"><%=GetLocalResourceObject("updatePanelmyRemarkScreenTitle") %></h4>
                        </div>
                        <div class="modal-body">
                            <p><%=GetLocalResourceObject("pEnterRemarks") %></p>
                            <asp:TextBox ID="txtRemarksArea" TextMode="MultiLine" Columns="50" Rows="5" runat="server" Width="270px" meta:resourcekey="txtRemarksAreaResource1" ClientIDMode="Static" />
                            <br />
                            <p><%=GetLocalResourceObject("pEnterActionTaken") %></p>
                            <br />
                            <asp:TextBox ID="txtActionTaken" TextMode="MultiLine" Columns="50" Rows="5" runat="server" Width="270px" meta:resourcekey="txtRemarksAreaResource1" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnOkRemarks" runat="server" Text="OK" CssClass="btn btn-primary" OnClick="btnOkRemarks_Click" meta:resourcekey="btnOkRemarksResource1" />
                            <button type="button" class="btn btn-primary" id="btnCancel" data-dismiss="modal"><%=GetGlobalResourceObject("CommanResource","btnCancel") %></button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row text-center">
                <asp:Label ID="lblMessages" runat="server" EnableViewState="False" Style="font-weight: bold; color: red; font-family: Calibri;"
                    meta:resourcekey="lblMessagesResource1"></asp:Label>
                <asp:HiddenField runat="server" ID="hdfValue" Value="Production Data" />
                <asp:HiddenField ID="hdfRemarks" runat="server" />
                <asp:HiddenField ID="hdfActionTaken" runat="server" />
                <asp:HiddenField ID="hdnvalueGetN" runat="server" />
                <asp:HiddenField ID="hdfHideValue" runat="server" />
                <asp:HiddenField ID="hdfPaging" runat="server" />
            </div>
            <div class="modal fade" id="myModifiedData" role="dialog">
                <div class="modal-dialog modal-sm" style="width: 60%; margin-top: 6%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 id="modifiedDatamodalTitle" class="modal-title">Modified Data History</h4>
                        </div>
                        <div class="modal-body">
                            <table id="gvModifiedData" class="table table-condensed border headerFixer" style="height: 80%; overflow: auto;">
                                <thead>
                                    <tr>
                                        <th>Record Type</th>
                                        <th>Down CODE</th>
                                        <th>Component ID</th>
                                        <th>Operation No</th>
                                        <th>Operator ID</th>
                                        <th>Parts Count</th>
                                        <th>Updated TS</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                            <%-- <asp:GridView ID="gvModifiedData" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                CssClass="table table-condensed border headerFixer" EmptyDataText="No data available">
                                <AlternatingRowStyle BackColor="#CCFFFF" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Component">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblComp" Text='<%# Eval("Component") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Operation">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOpn" Text='<%# Eval("Operation") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Operator">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOpr" Text='<%# Eval("Operator") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="PartsCount">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPartsCount" Text='<%# Eval("PartsCount") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Down ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDownID" Text='<%# Eval("DownID") %>' ClientIDMode="Static"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>--%>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" id="btnClose" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div class="row" style="margin-top: -15px;" id="formdiv">
        <div class="col-lg-4 managePanal">
            <asp:UpdatePanel ID="updatePanal1" runat="server">
                <ContentTemplate>
                    <div class="panel panel-primary">
                        <div class="panel-heading"><%=GetLocalResourceObject("updatePanal1SearchTitle") %></div>
                        <table class="table table-bordered" style="height: 200px;">
                            <tr>
                                <td>Plant</td>
                                <td>
                                    <asp:DropDownList ID="ddlPlantID" runat="server" CssClass="form-control input-sm" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>Cell</td>
                                <td>
                                    <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control input-sm" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%-- <tr>
                              
                            </tr>--%>
                            <tr>
                                <td>Machine</td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control input-sm" meta:resourcekey="ddlMachineIdResource1">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td><%=GetGlobalResourceObject("CommanResource","FromDate") %>
                                </td>
                                <td class="input-group" style="padding-left: 0px; padding-right: 0px;">
                                    <div class="input-group-addon" style="padding: 5px;">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" placeholder="DD-MMM-YYYY" Style="padding-left: 3px" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <td><%=GetGlobalResourceObject("CommanResource","ToDate") %>
                                </td>
                                <td class="input-group" style="padding-left: 0px; padding-right: 0px;">
                                    <div class="input-group-addon" style="padding: 5px;">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" placeholder="DD-MMM-YYYY" Style="padding-left: 3px" meta:resourcekey="txtToDateResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>

                            <%--   <tr>
                                <td><%=GetGlobalResourceObject("CommanResource","ToDate") %>
                                </td>
                                <td class="input-group" colspan="3">
                                    <div class="input-group-addon">
                                        <i class="glyphicon glyphicon-calendar"></i>
                                    </div>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" placeholder="DD-MMM-YYYY" Style="padding-left: 3px" meta:resourcekey="txtToDateResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <tr>
                                <td colspan="4" style="text-align: right;">
                                    <asp:HiddenField ID="hdfValueMange" runat="server" Value="fristTime" />
                                    <asp:Button ID="btnAnalysis" CssClass="btn btn-info btn-sm" Text="Analysis" runat="server" meta:resourcekey="btnAnalysisResource1" />
                                    <input type="button" class="btn btn-info btn-sm" value='<%=GetLocalResourceObject("btnGetLastN") %>' data-toggle="modal" data-target="#myModal" />
                                    <asp:Button runat="server" ID="btnRefresh" Text="<%$Resources:CommanResource, Refresh %>" OnClick="btnRefresh_Click" CssClass="btn btn-sm btn-primary" /></td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="col-lg-4 managePanal">
            <asp:UpdatePanel ID="updatePanal2" runat="server">
                <ContentTemplate>
                    <div class="panel panel-primary">
                        <div class="panel-heading"><%=GetLocalResourceObject("updatePanal2Heading") %></div>
                        <div style="overflow-x: auto; overflow-y: hidden; min-width: 400px; min-height: 200px; max-height: 213px;">
                            <table class="table table-bordered">
                                <tr>
                                    <td colspan="2" style="text-align: center;">
                                        <chart:WebChartViewer ID="PEChart" runat="server" Height="100px" Width="300px" />
                                    </td>
                                    <td colspan="2" style="text-align: center;">
                                        <chart:WebChartViewer ID="AEChart" runat="server" Height="100px" Width="300px" />
                                    </td>
                                    <td colspan="2" style="text-align: center;">
                                        <chart:WebChartViewer ID="OEChart" runat="server" Height="100px" Width="300px" />
                                    </td>
                                    <td colspan="2" style="text-align: center;">
                                        <chart:WebChartViewer ID="QEChart" runat="server" Height="100px" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><%=GetLocalResourceObject("tdTotal") %></td>
                                    <td>
                                        <asp:Label ID="lblTextBox1" runat="server" Text="---" meta:resourcekey="lblTextBox1Resource1"></asp:Label></td>
                                    <td><%=GetLocalResourceObject("tdDownTime") %>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTextBox2" runat="server" Text="---" meta:resourcekey="lblTextBox2Resource1"></asp:Label>
                                    </td>
                                    <td><%=GetLocalResourceObject("tdCompCount") %> 
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTextBox3" runat="server" Text="---" meta:resourcekey="lblTextBox3Resource1"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><%=GetLocalResourceObject("tdTurnOver") %></td>
                                    <td>
                                        <asp:Label ID="lblTextBox4" runat="server" Text="---" meta:resourcekey="lblTextBox4Resource1"></asp:Label>
                                    </td>
                                    <td><%=GetLocalResourceObject("tdReturnUtil") %></td>
                                    <td>
                                        <asp:Label ID="lblTextBox5" runat="server" Text="---" meta:resourcekey="lblTextBox5Resource1"></asp:Label>
                                    </td>
                                    <td><%=GetLocalResourceObject("tdReturnTot") %></td>
                                    <td>
                                        <asp:Label ID="lblTextBox6" runat="server" Text="---" meta:resourcekey="lblTextBox6Resource1"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="col-lg-4 managePanal">
            <%--<asp:UpdatePanel ID="UpdatePanel14" runat="server">
                <ContentTemplate>--%>
            <div class="panel panel-primary">
                <div class="panel-heading"><%=GetLocalResourceObject("managePanalHeading") %></div>
                <table class="table table-bordered">
                    <tr>
                        <td style="width: 30%"><%=GetLocalResourceObject("tdComp") %></td>
                        <td colspan="3">
                            <select id="ddlComponent" class="form-control input-sm">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%"><%=GetLocalResourceObject("tdStCycleTime") %>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblC1" runat="server" Text="--" meta:resourcekey="lblC1Resource1"></asp:Label>
                        </td>
                        <td style="width: 40%"><%=GetLocalResourceObject("tdAvgCycleTime") %>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblC2" runat="server" Text="--" meta:resourcekey="lblC2Resource1"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><%=GetLocalResourceObject("tdStLoadTime") %>
                        </td>
                        <td>
                            <asp:Label ID="lblC3" runat="server" Text="--" meta:resourcekey="lblC3Resource1"></asp:Label></td>
                        <td><%=GetLocalResourceObject("tdAvgLoadTime") %>
                        </td>
                        <td>
                            <asp:Label ID="lblC4" runat="server" Text="--" meta:resourcekey="lblC4Resource1"></asp:Label></td>
                    </tr>

                    <tr>
                        <td><%=GetLocalResourceObject("tdSpeedRatio") %>
                        </td>
                        <td>
                            <asp:Label ID="lblC5" runat="server" Text="--" meta:resourcekey="lblC5Resource1"></asp:Label></td>
                        <td><%=GetLocalResourceObject("tdLoadRatio") %>
                        </td>
                        <td>
                            <asp:Label ID="lblC6" runat="server" Text="--" meta:resourcekey="lblC6Resource1"></asp:Label></td>
                    </tr>

                    <tr>
                        <td><%=GetLocalResourceObject("tdOpCount") %>
                        </td>
                        <td>
                            <asp:Label ID="lblC7" runat="server" Text="--" meta:resourcekey="lblC7Resource1"></asp:Label></td>
                        <td colspan="2">
                            <input type="button" id="btnStdTime" value='<%=GetLocalResourceObject("btnStdTime") %>' class="btn btn-info btn-sm" style="display: <%: admin %>" />
                            <asp:Button ID="btnStatistics" CssClass="btn btn-info btn-sm" Text="Statistics" runat="server" meta:resourcekey="btnStatisticsResource1" OnClientClick="return goToStatistic();" />
                            <%--  OnClick="linkStatistics_Click"--%>
                        </td>
                    </tr>

                </table>
            </div>

            <%--  </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>

    </div>



    <div class="modal fade" id="myModalGetstdTime" role="dialog">
        <div class="modal-dialog modal-md">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header bg-primary text-center">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"><%=GetLocalResourceObject("myModalGetstdTimeTitle") %></h4>
                </div>

                <table class="table table-bordered">
                    <tr>
                        <td>
                            <label class="control-label"><%=GetGlobalResourceObject("CommanResource","MachineId") %></label></td>
                        <td>
                            <asp:Label ID="lblMachineName" runat="server" Text="-------" meta:resourcekey="lblMachineNameResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="control-label"><%=GetLocalResourceObject("lblCompId") %></label></td>
                        <td>
                            <asp:Label ID="lblComponentID" runat="server" Text="-------" meta:resourcekey="lblComponentIDResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="control-label"><%=GetLocalResourceObject("lblOperationNum") %></label></td>
                        <td>
                            <asp:Label ID="lblOperationNo" runat="server" Text="-------" meta:resourcekey="lblOperationNoResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--  <label class="control-label"><%=GetLocalResourceObject("lblStdCycleTime") %>(<span id="stdCycleTime"></span>)</label></td>--%>
                            <label class="control-label">Std. Machining Time(<span id="stdCycleTime"></span>)</label></td>
                        <td>
                            <asp:TextBox ID="txtStdCycleTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std Cycle Time" MaxLength="7" meta:resourcekey="txtStdCycleTimeResource1"></asp:TextBox>
                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnStdCycleTime" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="control-label"><%=GetLocalResourceObject("lblStdLoadTime") %>(<span id="stdLoadTime"></span>)</label></td>
                        <td>
                            <asp:TextBox ID="txtStdLoadTime" CssClass="form-control numbersOnly" runat="server" placeholder="Std Load Time" MaxLength="7" meta:resourcekey="txtStdLoadTimeResource1"></asp:TextBox>
                            <br />
                            <asp:CheckBox runat="server" ID="chkUpdateAllMachines" Text="Update All Machines" ClientIDMode="Static" />

                        </td>
                    </tr>
                </table>

                <div class="modal-footer text-center">
                    <input type="button" class="btn btn-primary" value="<%=GetGlobalResourceObject("CommanResource","Update") %>" id="btnUpdate" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal"><%=GetGlobalResourceObject("CommanResource","btnCancel") %></button>
                </div>
            </div>

        </div>
    </div>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog modal-sm" style="width: 350px;">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header bg-primary">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title"><%=GetLocalResourceObject("myModalTitle") %></h4>
                        </div>
                        <div class="modal-body">
                            <p><%=GetLocalResourceObject("pNumOfComp") %></p>
                            <asp:TextBox ID="txtNoofComponents" runat="server" CssClass="form-control numbersOnly" ClientIDMode="Static" meta:resourcekey="txtNoofComponentsResource1"></asp:TextBox>
                        </div>
                        <div class="modal-footer text-center">
                            <asp:Button runat="server" ID="btnhide" Text="OK" CssClass="btn btn-sm btn-primary" OnClick="btnGetComponent_Click" meta:resourcekey="btnhideResource1" />
                            <button type="button" class="btn btn-primary" data-dismiss="modal"><%=GetGlobalResourceObject("CommanResource","btnCancel") %></button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="myModalUpdateconfirm" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body text-center ">
                    <h3>
                        <asp:Label runat="server" Text="Data Updated Successively" meta:resourcekey="LabelResource1"></asp:Label>
                    </h3>
                    <br />
                    <asp:Button runat="server" ID="Button2" Text="<%$Resources:CommanResource, OK %>" CssClass="btn btn-sm btn-primary" data-dismiss="modal" />
                </div>

            </div>

        </div>
    </div>

    <div id="containerbottom">
        <div class="row" id="gridHeader">
            <div class="panel panel-primary">
                <div class="panel-heading" style="background-color: black;">
                    <div class="col-lg-6 pull-left" style="padding-bottom: 0; padding-left: 0">
                        <input type="button" class="btn btn-primary btn-sm" value='<%=GetLocalResourceObject("btnProductionData") %>' id="btnProductionData" style="margin-bottom: 4px; margin-left: -13px; height: 30px;" />
                        <input type="button" class="btn btn-default btn-sm" value="<%=GetLocalResourceObject("btnDownData") %>" id="btnDownData" style="margin-bottom: 4px; height: 30px;" />
                        <input type="button" class="btn btn-default btn-sm buttonEvt" value="Events" id="btnEvents" style="margin-bottom: 4px; height: 30px; display: none;" />
                        <input type="button" class="btn btn-default btn-sm buttonProductionAndDownData" value="<%=GetLocalResourceObject("ProductionAndDownData") %>" id="btnProductionAndDownData" style="margin-bottom: 4px; height: 30px; display: none;" />
                        <%--<input type="button" class="btn btn-default btn-sm" value="Full Screen" id="btnFullScreen" onclick="launchfsc()" style="margin-bottom: 5px; height: 30px;"/>--%>
                    </div>

                    <div class="col-lg-6 text-right" style="padding-bottom: 0">
                        <i class="glyphicon glyphicon-fullscreen" aria-hidden="true" style="border: none; font-size: 16px; margin-right: 5px; margin-bottom: 4px; height: 30px; cursor: pointer;"
                            id="btnFullScreen" onclick="launchfsc()"></i>
                        <input type="checkbox" name="hideGraph" id="chkHideGraph" class="hidegraph-eve">
                        <label for="chkHideGraph" class="hidegraph-eve" style="cursor: pointer;"><%=GetLocalResourceObject("chkHideGraph") %></label>&nbsp;
        <asp:ImageButton ID="btnExportData" runat="server" Text="Export Data" Style="margin-bottom: -8px" ImageUrl="~/Images/Excel-icon.png" OnClick="Export_Data_Click" meta:resourcekey="btnExportDataResource1" /><%--OnClick="Export_Data_Click" --%>
                        <label for="btnExportData" id="lblExportData" style="cursor: pointer;"><%=GetLocalResourceObject("btnExportData") %></label>&nbsp;
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="DivChart">
            <div class="col-lg-12" style="padding: 0 1px 0 1px" id="prodGrid">
                <div class="panel panel-primary" id="divPDScroll" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; max-height: 370px; width: 100%; margin-top: -5px;">
                    <asp:UpdatePanel ID="updatePanal4" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridProductionData" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" OnSorting="gridProductionData_Sorting" ShowHeaderWhenEmpty="True"
                                CssClass="table table-condensed border headerFixer" AllowPaging="True" PageSize="1000" OnPageIndexChanging="gridProductionData_PageIndexChanging"
                                EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>"
                                AllowSorting="True" OnRowDataBound="gridProductionData_RowDataBound">
                                <AlternatingRowStyle BackColor="#CCFFFF" />
                                <Columns>
                                    <%--  <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" meta:resourcekey="BoundFieldResource1" />
                                    <asp:BoundField DataField="compslno" SortExpression="SerialNumber" meta:resourcekey="BoundFieldResource18" />
                                    <asp:BoundField DataField="ComponentID" SortExpression="ComponentID" meta:resourcekey="BoundFieldResource2" />
                                    <asp:BoundField DataField="OperationNo" SortExpression="OperationNo" meta:resourcekey="BoundFieldResource3" />
                                    <asp:BoundField DataField="PartSlno" SortExpression="PartSlno" meta:resourcekey="BoundFieldResource17" />
                                    <asp:BoundField DataField="HeatCode" SortExpression="HeatCode" meta:resourcekey="BoundFieldResource18" />
                                    <asp:BoundField DataField="OperatorID" SortExpression="OperatorID" meta:resourcekey="BoundFieldResource4" />
                                    <asp:BoundField DataField="OperatorName" SortExpression="OperatorName" meta:resourcekey="BoundFieldResource5" />
                                    <asp:BoundField DataField="StartTime" SortExpression="StartTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource6" />
                                    <asp:BoundField DataField="EndTime" SortExpression="EndTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource7" />
                                    <asp:BoundField DataField="Partscount" SortExpression="PalletCount" DataFormatString="{0:N0}" meta:resourcekey="BoundFieldResource8" />
                                    <asp:BoundField DataField="CycleTime" SortExpression="CycleTime" meta:resourcekey="BoundFieldResource8" />
                                    <asp:BoundField DataField="LoadUnloadTime" SortExpression="LoadUnloadTime" meta:resourcekey="BoundFieldResource9" />
                                    <asp:BoundField DataField="In_Cycle_DownTime" SortExpression="In_Cycle_DownTime" meta:resourcekey="BoundFieldResource10" />
                                    <asp:BoundField DataField="PDT" SortExpression="PDT" HeaderText="PDT" meta:resourcekey="BoundFieldResource11" />
                                    <asp:BoundField DataField="PVNo" SortExpression="PVNo" meta:resourcekey="BoundFieldResource12" />
                                    <asp:BoundField DataField="FGBatchID" SortExpression="FGBatchID" meta:resourcekey="BoundFieldResource13" />
                                    <asp:BoundField DataField="ChildBatchID" SortExpression="ChildBatchID" meta:resourcekey="BoundFieldResource14" />
                                    <asp:BoundField DataField="ChildPartID" SortExpression="ChildPartID" meta:resourcekey="BoundFieldResource15" />
                                    <asp:BoundField DataField="MachineMode" SortExpression="MachineMode" meta:resourcekey="BoundFieldResource16" />--%>
                                    <%--<asp:TemplateField SortExpression="Remarks" meta:resourcekey="TemplateFieldResource1"> //changed13112021
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" CssClass="control-label" runat="server" Text='<%# Bind("Remarks") %>' meta:resourcekey="lblRemarksResource1"></asp:Label>
                                            <asp:HiddenField ID="hdfRemarks" runat="server" Value='<%# Bind("id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="panel panel-primary" id="divDTDScroll" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; max-height: 370px; width: 100%; margin-top: -5px;">
                    <asp:UpdatePanel ID="updatePanal5" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridDownTimeData" runat="server" ClientIDMode="Static" AutoGenerateColumns="False" OnSorting="gridDownTimeData_Sorting" ShowHeaderWhenEmpty="True"
                                CssClass="table table-condensed border headerFixer" AllowPaging="True" PageSize="1000" OnPageIndexChanging="gridDownTimeData_PageIndexChanging" OnRowDataBound="gridDownTimeData_RowDataBound" EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>"
                                AllowSorting="True">
                                <AlternatingRowStyle BackColor="#CCFFFF" />
                                <Columns>
                                    <%--  <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" meta:resourcekey="BoundFieldResource17" />
                                    <asp:BoundField DataField="compslno" SortExpression="SerialNumber" meta:resourcekey="BoundFieldResource18" />
                                    <asp:BoundField DataField="OperatorID" SortExpression="OperatorID" meta:resourcekey="BoundFieldResource18" />
                                    <asp:BoundField DataField="PartSlno" SortExpression="PartSlno" meta:resourcekey="BoundFieldResource17" />
                                    <asp:BoundField DataField="HeatCode" SortExpression="HeatCode" meta:resourcekey="BoundFieldResource18" />
                                    <asp:BoundField DataField="OperatorName" SortExpression="OperatorName" meta:resourcekey="BoundFieldResource19" />
                                    <asp:BoundField DataField="StartTime" SortExpression="StartTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource20" />
                                    <asp:BoundField DataField="EndTime" SortExpression="EndTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" meta:resourcekey="BoundFieldResource21" />
                                    <asp:BoundField DataField="DownID" SortExpression="DownID" meta:resourcekey="BoundFieldResource22" />
                                    <asp:BoundField DataField="DownDescription" SortExpression="DownDescription" meta:resourcekey="BoundFieldResource23" />
                                    <asp:BoundField DataField="DownTime" SortExpression="DownTime" meta:resourcekey="BoundFieldResource24" />
                                    <asp:BoundField DataField="DownThreshold" SortExpression="DownThreshold" meta:resourcekey="BoundFieldResource25" />
                                    <asp:BoundField DataField="MLE" SortExpression="MLE" meta:resourcekey="BoundFieldResource26" />
                                    <asp:BoundField DataField="PDT" SortExpression="PDT" meta:resourcekey="BoundFieldResource27" />
                                    <asp:BoundField DataField="PVNo" SortExpression="PVNo" meta:resourcekey="BoundFieldResource28" />
                                    <asp:BoundField DataField="FGBatchID" SortExpression="FGBatchID" meta:resourcekey="BoundFieldResource29" />
                                    <asp:BoundField DataField="ChildBatchID" SortExpression="ChildBatchID" meta:resourcekey="BoundFieldResource30" />
                                    <asp:BoundField DataField="ChildPartID" SortExpression="ChildPartID" meta:resourcekey="BoundFieldResource31" />
                                    <asp:BoundField DataField="MachineMode" SortExpression="MachineMode" meta:resourcekey="BoundFieldResource32" />--%>
                                    <%--<asp:TemplateField SortExpression="Remarks" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDownRemarks" runat="server" CssClass="control-label" Text='<%# Bind("Remarks") %>' meta:resourcekey="lblDownRemarksResource1"></asp:Label>
                                            <asp:HiddenField ID="hdfDownRemarks" runat="server" Value='<%# Bind("id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                        <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="gridDownTimeData" EventName="RowCommand" />
            </Triggers>--%>
                    </asp:UpdatePanel>
                </div>

                <div class="panel panel-primary" id="divEventScroll" style="overflow-x: auto; overflow-y: scroll; min-height: 40%; max-height: 40%; width: 100%; margin-top: -5px;">
                    <asp:UpdatePanel ID="updatePanal6" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridEventData" ClientIDMode="Static" runat="server" AutoGenerateColumns="false" OnSorting="gridEventData_Sorting" ShowHeaderWhenEmpty="true" CssClass="table table-condensed border headerFixer" AllowPaging="true" PageSize="1000" OnPageIndexChanging="gridEventData_PageIndexChanging" EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>" AllowSorting="true">
                                <AlternatingRowStyle BackColor="#CCFFFF" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Component ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" CssClass="search-lbl" Text='<%# Eval("componentid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Operation No">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" CssClass="search-lbl" Text='<%# Eval("operationno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Event ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" CssClass="search-lbl" Text='<%# Eval("EventID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Event Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" CssClass="search-lbl" Text='<%# Eval("EventDescription") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Event TS">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" CssClass="search-lbl" Text='<%# Eval("EventTS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Operator ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" CssClass="search-lbl" Text='<%# Eval("Employeeid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Operator Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" CssClass="search-lbl" Text='<%# Eval("OprName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#3176B1" Font-Bold="true" ForeColor="White" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="panel panel-primary" id="divPDDScroll" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; max-height: 370px; width: 100%; margin-top: -5px;">
                    <asp:UpdatePanel ID="updatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvProductionDownData" ClientIDMode="Static" runat="server" AutoGenerateColumns="False" OnSorting="gvProductionDownData_Sorting" ShowHeaderWhenEmpty="True"
                                CssClass="table table-condensed border headerFixer" AllowPaging="True" PageSize="1000" OnPageIndexChanging="gvProductionDownData_PageIndexChanging"
                                EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>"
                                AllowSorting="True" OnRowDataBound="gvProductionDownData_RowDataBound">
                                <Columns>
                                </Columns>
                                <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>

            <div class="col-lg-12" style="padding: 0 1px 0 1px" id="footerchart">
                <div id="productionDataChart" style="display: none; width: 100%; min-height: 250px;">
                </div>
                <div id="downTimeDataChart" style="display: none; width: 100%; min-height: 250px;">
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Warning!</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="lblWarningMsg" style="color: black"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>
    <%--  <div class="row"--%>
    <div class="modal infoModal" id="cycleTimeUpdateLoginModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered " style="width: 22%">
            <div class="modal-content modalThemeCss">
                <div class="modal-header">
                    <h4 class="modal-title">Credentials Required</h4>
                    <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" style="float: right;"></i>
                    <%--position: relative;top: -22px;--%>
                </div>
                <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                    <span style="color: red; margin-left: 20px;" class="mandatory-message"></span>
                    <div style="padding-left: 15px; padding-right: 15px; padding-bottom: 8px;">
                        <table style="width: 100%; margin: auto" id="tblUserLoginInfo">
                            <tr>
                                <td style="width: 160px">User ID *</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtUserid" CssClass="form-control" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 32px; width: 160px">Password *</td>
                                <td style="padding-top: 32px">
                                    <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <label id="lblCycleTimeUpdateLoginMsg" style="color: red; margin-top: 20px;"></label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" onclick="btnCycleTimeUpdateConfirmationClick();">OK</button>
                    <button type="button" data-dismiss="modal" class="btn btn-info">Close</button>
                </div>
            </div>
        </div>
    </div>

    <ul class='custom-menu'>
        <li data-action="ToolWiseCycleTime"><i class="glyphicon glyphicon-sound-dolby"></i>&nbsp;&nbsp;Tool Wise Cycle Time
            <input type="hidden" id="hdnCompIDInContext" />
            <input type="hidden" id="hdnStartTimeInContext" />
            <input type="hidden" id="hdnEndTimeInContext" />
            <input type="hidden" id="hdnOprNoInContext" />
            <input type="hidden" id="hdnSlnoInContext" />
        </li>
    </ul>

    <script type="text/javascript">
        var btnText = null;
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        var ModifiedBackColor = "";
        function setContextMenuData() {
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/isCustomPageEnabled",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var menuPageEnabled = response.d;
                    if (menuPageEnabled) {
                        $('#gridProductionData tr').bind("contextmenu", function (event) {
                            // Avoid the real one
                            event.preventDefault();
                            debugger;
                            // Show contextmenu
                            var message = "";
                            var row = $(this);
                            $.ajax({
                                async: false,
                                type: "POST",
                                url: "VDGScreen.aspx/getRequiredCellNumberForContext",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    var itemdata = response.d;

                                    debugger;
                                    if (itemdata != null && itemdata != "") {
                                        var colnum = getColumnNumber(itemdata.CycleStartTime);
                                        if (colnum == -1) {
                                            $('#hdnStartTimeInContext').val("");
                                        }
                                        else {
                                            $('#hdnStartTimeInContext').val(row.closest('tr').find('td')[colnum].textContent);
                                        }

                                        colnum = getColumnNumber(itemdata.CycleEndTime);
                                        if (colnum == -1) {
                                            $('#hdnEndTimeInContext').val("");
                                        }
                                        else {
                                            $('#hdnEndTimeInContext').val(row.closest('tr').find('td')[colnum].textContent);
                                        }
                                        colnum = getColumnNumber(itemdata.ComponentID);
                                        if (colnum == -1) {
                                            $('#hdnCompIDInContext').val("");
                                        }
                                        else {
                                            $('#hdnCompIDInContext').val(row.closest('tr').find('td')[colnum].textContent);
                                        }
                                        colnum = getColumnNumber(itemdata.SlNo);
                                        if (colnum == -1) {
                                            $('#hdnSlnoInContext').val("");
                                        }
                                        else {
                                            $('#hdnSlnoInContext').val(row.closest('tr').find('td')[colnum].textContent);
                                        }
                                        colnum = getColumnNumber(itemdata.OperationNum);
                                        if (colnum == -1) {
                                            $('#hdnOprNoInContext').val("");
                                        }
                                        else {
                                            $('#hdnOprNoInContext').val(row.closest('tr').find('td')[colnum].textContent);
                                        }
                                    }
                                },
                                error: function (jqXHR, textStatus, err) {
                                    alert('Error: ' + err);
                                    if (jqXHR.status == 401)
                                        window.location.href = "SignIn.aspx";
                                }
                            });
                            $(".custom-menu").stop(true, true).toggle(100).

                                // In the right position (the mouse)
                                css({
                                    top: event.pageY + "px",
                                    left: event.pageX + "px"
                                });
                        });
                        // If  document is clicked somewhere
                        $(document).bind("mousedown", function (e) {
                            // If the clicked element is not the menu
                            if (!$(e.target).parents(".custom-menu").length > 0) {

                                // Hide it
                                $(".custom-menu").hide(100);
                            }
                        });
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        function getColumnNumber(colnum) {
            var colmnnumber = -1;
            if (colnum != "" && colnum != null) {
                colmnnumber = parseInt(colnum);
            }
            return colmnnumber;
        }
        // If the menu element is clicked
        $(".custom-menu li").click(function () {

            // This is the triggered action name
            switch ($(this).attr("data-action")) {

                // A case for each action. Your actions here
                case "ToolWiseCycleTime":
                    debugger;
                    if ($('#hdnStartTimeInContext').val() == "") {
                        openWarningModal("Please select Start Time.");
                        return;
                    }
                    if ($('#hdnEndTimeInContext').val() == "") {
                        openWarningModal("Please select End Time.");
                        return;
                    }
                    if ($("[id$=ddlMachineId]").val() == "" || $("[id$=ddlMachineId]").val() == null) {
                        openWarningModal("Please select Machine.");
                        return;
                    }
                    if (IsPageEnabled("KiswokPage") == true) {
                        PopupCenter("ToolWiseCycleTimes.aspx?MachineID=" + $("[id$=ddlMachineId]").val() + "&CompID=" + $('#hdnCompIDInContext').val() + "&OprNum=" + $('#hdnOprNoInContext').val() + "&StartTime=" + $('#hdnStartTimeInContext').val() + "&EndTime=" + $('#hdnEndTimeInContext').val() + "&Slno=" + $('#hdnSlnoInContext').val(), "Tool Wise Cycle Time", 900, 500);
                        break;
                    }
                    else if (IsPageEnabled("VulkanPages") == true) {
                        PopupCenter("ToolWiseCycleTimes_Vulkan.aspx?MachineID=" + $("[id$=ddlMachineId]").val() + "&CompID=" + $('#hdnCompIDInContext').val() + "&OprNum=" + $('#hdnOprNoInContext').val() + "&StartTime=" + $('#hdnStartTimeInContext').val() + "&EndTime=" + $('#hdnEndTimeInContext').val() + "&Slno=" + $('#hdnSlnoInContext').val(), "Tool Wise Cycle Time Vulkan", 900, 500);
                        break;
                    }
            }

            // Hide it AFTER the action was triggered
            $(".custom-menu").hide(100);
        });
        //which page is enabled for custom menu
        function IsPageEnabled(paggeName) {
            var pageEnabled = false;
            $.ajax({
                async: false,
                type: "POST",
                url: "VDGScreen.aspx/isPageEnabled",
                contentType: "application/json; charset=utf-8",
                data: '{pageName:"' + paggeName + '"}',
                dataType: "json",
                success: function (response) {
                    debugger;
                    pageEnabled = response.d;

                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
            return pageEnabled;
        }
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
        function launchIntoFullscreen(element) {
            if (element.requestFullscreen) {
                element.requestFullscreen();
            } else if (element.mozRequestFullScreen) {
                element.mozRequestFullScreen();
            } else if (element.webkitRequestFullscreen) {
                element.webkitRequestFullscreen();
            } else if (element.msRequestFullscreen) {
                element.msRequestFullscreen();
            }
        }

        function launchfsc() {
            launchIntoFullscreen(document.getElementById("containerbottom"));
            if (btnText == "ProductionData") {
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%");
                //document.getElementById('DivChart').setAttribute("style", "width:100%");
                document.getElementById('DivChart').setAttribute("style", "height:96%;");
                document.getElementById('prodGrid').setAttribute("style", "height:60%");
                $("#divDTDScroll").hide();
                $("#divEventScroll").hide();
                $("#divPDDScroll").hide();
                document.getElementById('divPDScroll').setAttribute("style", "height:96%");
                document.getElementById("divPDScroll").style.overflow = "scroll";
                document.getElementById('footerchart').setAttribute("style", "height:40%");
                $("#downTimeDataChart").hide();
                document.getElementById('productionDataChart').setAttribute("style", "height:95%");
                var chart = $('#productionDataChart').highcharts();
                chart.reflow();

            }
            else if (btnText == "DownTimeData") {
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%");
                document.getElementById('DivChart').setAttribute("style", "height:96%;");
                document.getElementById('prodGrid').setAttribute("style", "height:60%");
                $("#divPDScroll").hide();
                $("#divEventScroll").hide();
                $("#divPDDScroll").hide();
                document.getElementById('divDTDScroll').setAttribute("style", "height:96%");
                document.getElementById("divDTDScroll").style.overflow = "scroll";
                document.getElementById('footerchart').setAttribute("style", "height:40%");
                $("#productionDataChart").hide();
                document.getElementById('downTimeDataChart').setAttribute("style", "height:95%");
                var chart = $('#downTimeDataChart').highcharts();
                chart.reflow();
            }
            else if (btnText == "Events") {
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%");
                document.getElementById('DivChart').setAttribute("style", "height:96%;");
                document.getElementById('prodGrid').setAttribute("style", "height:100%");
                $("#divPDScroll").hide();
                $("#divDTDScroll").hide();
                $("#divPDDScroll").hide();
                document.getElementById('divEventScroll').setAttribute("style", "height:90vh");
                document.getElementById("divEventScroll").style.overflow = "scroll";
                //$("#divEventScroll").css('min-height', $(window).height() - 370);
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
            }
            else if (btnText == "Production&DownData") {
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%");
                document.getElementById('DivChart').setAttribute("style", "height:96%;");
                document.getElementById('prodGrid').setAttribute("style", "height:100%");
                $("#divPDScroll").hide();
                $("#divDTDScroll").hide();
                $("#divEventScroll").hide();
                document.getElementById('divPDDScroll').setAttribute("style", "height:90vh");
                document.getElementById("divPDDScroll").style.overflow = "scroll";
                //$("#divEventScroll").css('min-height', $(window).height() - 370);
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
            }
            else {
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%");
                document.getElementById('DivChart').setAttribute("style", "height:96%");
                document.getElementById('prodGrid').setAttribute("style", "height:60%");
                document.getElementById('divPDScroll').setAttribute("style", "height:96%");
                document.getElementById("divPDScroll").style.overflow = "scroll";
                document.getElementById('footerchart').setAttribute("style", "height:40%");
                document.getElementById('productionDataChart').setAttribute("style", "height:95%");
                document.getElementById('downTimeDataChart').setAttribute("style", "height:95%");
                var chart = $('#productionDataChart').highcharts();
                chart.reflow();
                $("#downTimeDataChart").hide();
            }
        }

        function isFullscreen() {
            var fhd;
            if ((!document.mozFullScreen && !document.webkitIsFullScreen)) {
                fhd = false;
            } else {
                fhd = true;
            }
            return fhd;
        }

        if (document.addEventListener) {
            document.addEventListener('webkitfullscreenchange', exitHandler, false);
            document.addEventListener('mozfullscreenchange', exitHandler, false);
            document.addEventListener('fullscreenchange', exitHandler, false);
            document.addEventListener('MSFullscreenChange', exitHandler, false);
        }

        function exitHandler() {
            if (!document.webkitIsFullScreen && !document.mozFullScreen && !document.msFullscreenElement) {
                if (btnText == "ProductionData") {
                    $("#divPDScroll").css('max-height', '370px');
                    $("#downTimeDataChart").hide();
                    $("#divDTDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#divPDDScroll").hide();
                    $("#divPDScroll").show();
                    $("#productionDataChart").show();
                    //document.getElementById('DivChart').setAttribute("style", "margin:0px;");
                    //document.getElementById('prodGrid').setAttribute("style", "padding:0px;");
                    //document.getElementById('footerchart').setAttribute("style", "padding:0px;");
                }
                else if (btnText == "DownTimeData") {
                    $("#divPDScroll").hide();
                    $("#productionDataChart").hide();
                    $("#divPDDScroll").hide();
                    $("#divDTDScroll").show();
                    $("#divDTDScroll").css('max-height', '370px');
                    $("#downTimeDataChart").show();
                    $("#divEventScroll").hide();

                }
                else if (btnText == "Events") {
                    $("#divPDScroll").hide();
                    $("#productionDataChart").hide();
                    $("#divDTDScroll").hide();
                    $("#divPDDScroll").hide();
                    $("#divEventScroll").show();
                    $("#divEventScroll").css('height', $(window).height() - 370);
                    $("#downTimeDataChart").hide();
                }
                else if (btnText == "Production&DownData") {
                    $("#divPDScroll").hide();
                    $("#divDTDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#productionDataChart").hide();
                    $("#downTimeDataChart").hide();
                    $("#divPDDScroll").show();
                    $("#divPDDScroll").css('height', $(window).height() - 370);
                }
                else {
                    $("#divPDScroll").css('max-height', '370px');
                    $("#downTimeDataChart").hide();
                    $("#divDTDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#divPDScroll").hide();
                }
            }
        }
        function openPopup(slNo, Param) {
            debugger;
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            //let slno = $($(this).closest('tr').children()[0]).text();
            getAutodataID_ModifiedData(slNo, Param);

            /*  $("#myModifiedData").modal('show');*/
        }
        function GetModifiedBackColor() {
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/GetModifiedBackColor",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    ModifiedBackColor = response.d;
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        $(document).ready(function () {
            var winHeight = $(window).height();
            if (winHeight < 650) {
                winHeight = (winHeight - 350);
                console.log('min');
            } else {
                winHeight = (winHeight - 670);
                console.log('max');
            }

            $("#productionDataChart").height(winHeight);
            $("#downTimeDataChart").height(winHeight);
            eventVisible();
            $.unblockUI({});
            dateTimePicker();
            $("[id$=PEChart]").css("width", "auto");
            $("[id$=AEChart]").css("width", "auto");
            $("[id$=OEChart]").css("width", "auto");
            //var winHeight = $(window).height();
            //winHeight = (winHeight - 300);
            //$("#productionDataChart").height(winHeight);
            //$("#downTimeDataChart").height(winHeight);
            $("#myDropdown").click(function () {
                $('li > ul').not($(this).children("ul").toggle()).hide();
                return false;
            });
            jQuery('.numbersOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });

            $("[id$=gridDownTimeData] tr.dtdrow").find("td:nth-last-child(2)").click(function () {
                debugger;
                //var remarkId = $(this).find("input").val();
                //if (remarkId != undefined) {
                //    var remarks = $(this).find("span").html();
                //    $("[id$=txtRemarksArea]").val(remarks);
                //    $("[id$=hdfRemarks]").val(remarkId);
                //    $("#myRemarkScreen").modal('show');
                //}
                /*else*/ {
                    let slno = $($(this).closest('tr').children()[0]).text();
                    getAutodataID(slno, "DownData");
                }

                //$("[id$=txtActionTaken]").hide();
                //$("[id$=txtRemarksArea]").show();
            });

            $("[id$=gridDownTimeData] tr.dtdrow").find("td:last").click(function () {
                //var ActionTakenID = $(this).find("input").val();
                //if (ActionTakenID != undefined) {
                //    var ActionTaken = $(this).find("span").html();
                //    $("[id$=txtActionTaken]").val(ActionTaken);
                //    $("[id$=hdfActionTaken").val(ActionTakenID);
                //    $("#myRemarkScreen").modal('show');
                //}
                /*else*/ {
                    let slno = $($(this).closest('tr').children()[0]).text();
                    getAutodataID(slno, "DownData");
                }
                //$("[id$=txtRemarksArea]").hide();
                //$("[id$=txtActionTaken]").show();
            });

            //$("[id$=gridProductionData] tr.pdrow").find("td:nth-child(" + ($("[id$=gridProductionData] th:contains('IsModified')").index() + 1) + ")").click(function () {
            //    $("#myModifiedData").modal('show');
            //});
            $("[id$=gridProductionData] tr.pdrow").find("td:last").click(function () {
                var remarkId = $(this).find("input").val();
                if (remarkId != undefined) {
                    var remarks = $(this).find("span").html();
                    $("[id$=txtRemarksArea]").val(remarks);
                    $("[id$=hdfRemarks]").val(remarkId);
                    $("[id$=txtActionTaken]").hide();
                    $("#myRemarkScreen").modal('show');
                }
                else {
                    let slno = $($(this).closest('tr').children()[0]).text();
                    getAutodataID(slno, "ProductionData");
                }
            });

            $("input[value='Get Last N']").click(function () {
                $("[id$=txtNoofComponents]").focus();
                $('#myModal').modal('show');
                return false;
            });
            GetModifiedBackColor();
            firstTimeLaod();
            pagingLaodFun();
            //componentPerformance();
            //hideAndShowChart();
            setContextMenuData();
        });

        function firstTimeLaod() {
            if ($("[id$=hdfValueMange]").val() == "fristTime") {
                GetProductionChartData();
                GetDownTimeChartData();
                bindComponent();
            }
        }
        function pagingLaodFun() {
            if ($("[id$=hdfPaging]").val() == "DownTimePaging") {
                GetDownTimeChartData();
            }
            else if ($("[id$=hdfPaging]").val() == "ProductionPaging") {
                GetProductionChartData();
            }

        }
        //function dateTimePicker() {
        //    $('.date1').datepicker({
        //        format: 'dd-MM-yyyy'
        //    });
        //    $('.date2').datepicker({
        //        format: 'dd-MM-yyyy',
        //        useCurrent: false
        //    });
        //    $(".date1").on("dp.change", function (e) {
        //        $('.date2').data("DateTimePicker").minDate(e.date);
        //    });
        //    $(".date2").on("dp.change", function (e) {
        //        $('.date1').data("DateTimePicker").maxDate(e.date);
        //    });
        //}
        function dateTimePicker() {
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale:'<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale:'<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
        }

        $("#chkHideGraph").change(function () {
            var isfscreen = isFullscreen();
            if (isfscreen == false) {
                if ($(this).is(':checked')) {
                    $("#productionDataChart").hide();
                    $("#downTimeDataChart").hide();
                    var hight = $(window).height() - 370;
                    $("#divPDScroll").css('max-height', hight);
                    $("#divDTDScroll").css('max-height', hight);
                    $("#divEventScroll").css('max-height', hight);

                } else {
                    if ($("[id$=hdfValue]").val() == "Production Data") {
                        $("#productionDataChart").show();
                        $("#downTimeDataChart").hide();
                    }
                    if ($("[id$=hdfValue]").val() == "Down Time Data") {
                        $("#productionDataChart").hide();
                        $("#downTimeDataChart").show();
                    }
                    $("#divPDScroll").css('max-height', 370);
                    $("#divDTDScroll").css('max-height', 370);
                    $("#divEventScroll").css('max-height', 770);

                }
            }
            else {
                if ($(this).is(':checked')) {
                    $("#productionDataChart").hide();
                    $("#downTimeDataChart").hide();
                    if ($("[id$=hdfValue]").val() == "Production Data") {
                        document.getElementById('prodGrid').setAttribute("style", "height:100%");
                        document.getElementById('divPDScroll').setAttribute("style", "height:100%");
                        document.getElementById("divPDScroll").style.overflow = "scroll";
                        //document.getElementById('divDTDScroll').setAttribute("style", "height:100%");
                        $("#divDTDScroll").hide();
                        $("#divEventScroll").hide();
                        $("#divPDDScroll").hide();
                    }
                    if ($("[id$=hdfValue]").val() == "Down Time Data") {
                        $("#divPDScroll").hide();
                        $("#divEventScroll").hide();
                        $("#divPDDScroll").hide();
                        document.getElementById('prodGrid').setAttribute("style", "height:100%");
                        document.getElementById('divDTDScroll').setAttribute("style", "height:100%");
                    }
                    if ($("[id$=hdfValue]").val() == "Events") {
                        $("#divPDScroll").hide();
                        $("#divDTDScroll").hide();
                        $("#divPDDScroll").hide();
                        document.getElementById('prodGrid').setAttribute("style", "height:100%");
                        document.getElementById('divEventScroll').setAttribute("style", "height:100%");
                    }
                    if ($("[id$=hdfValue]").val() == "Production&DownData") {
                        $("#divPDScroll").hide();
                        $("#divDTDScroll").hide();
                        $("#divEventScroll").hide();
                        document.getElementById('prodGrid').setAttribute("style", "height:100%");
                        document.getElementById('divPDDScroll').setAttribute("style", "height:100%");
                    }
                }
                else {
                    if ($("[id$=hdfValue]").val() == "Production Data") {
                        $("#productionDataChart").show();
                        $("#downTimeDataChart").hide();
                        $("#divPDScroll").hide();
                        //document.getElementById('containerbottom').setAttribute("style", "width:100%");
                        document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                        //document.getElementById('DivChart').setAttribute("style", "width:100%");
                        document.getElementById('DivChart').setAttribute("style", "height:100%");
                        document.getElementById('prodGrid').setAttribute("style", "height:60%");
                        document.getElementById('divPDScroll').setAttribute("style", "height:100%");
                        document.getElementById("divPDScroll").style.overflow = "scroll";
                        //document.getElementById('divDTDScroll').setAttribute("style", "height:100%");
                        document.getElementById('footerchart').setAttribute("style", "height:40%");
                        document.getElementById('productionDataChart').setAttribute("style", "height:95%");

                    }
                    if ($("[id$=hdfValue]").val() == "Down Time Data") {
                        $("#productionDataChart").hide();
                        $("#downTimeDataChart").show();
                        //document.getElementById('containerbottom').setAttribute("style", "width:100%");
                        document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                        //document.getElementById('DivChart').setAttribute("style", "width:100%");
                        document.getElementById('DivChart').setAttribute("style", "height:100%");
                        document.getElementById('prodGrid').setAttribute("style", "height:60%");
                        document.getElementById('divDTDScroll').setAttribute("style", "height:100%");
                        document.getElementById('footerchart').setAttribute("style", "height:40%");
                        document.getElementById('downTimeDataChart').setAttribute("style", "height:95%");
                    }
                    if ($("[id$=hdfValue]").val() == "Events") {
                        document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                        document.getElementById('DivChart').setAttribute("style", "height:100%");
                        document.getElementById('prodGrid').setAttribute("style", "height:100%");
                        document.getElementById('footerchart').setAttribute("style", "height:5%");
                        document.getElementById('divEventScroll').setAttribute("style", "height:100%");
                    }
                    if ($("[id$=hdfValue]").val() == "Production&DownData") {
                        document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                        document.getElementById('DivChart').setAttribute("style", "height:100%");
                        document.getElementById('prodGrid').setAttribute("style", "height:100%");
                        document.getElementById('footerchart').setAttribute("style", "height:5%");
                        document.getElementById('divPDDScroll').setAttribute("style", "height:100%");
                    }
                }
            }
        });

        $("#downTimeDataChart").hide();
        $("#divDTDScroll").hide();
        $("#divEventScroll").hide();
        $("#divPDDScroll").hide();
        $("#productionDataChart").show();

        $("#btnProductionData").click(function () {
            btnText = "ProductionData";
            var isfscreen = isFullscreen();
            if (isfscreen == false) {
                $("[id$=hdfValue]").val("Production Data");
                hightManage();
                $("#btnProductionData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
                // $("#btnProductionData").css("backgroundColor", "#007AFF", "color", "#DEE9EC");
                $("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
                $("#btnEvents").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
                $("#btnProductionAndDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");

                document.getElementById("chkHideGraph").style.visibility = "visible";
                $(".hidegraph-eve").show();
                var chart = $('#productionDataChart').highcharts();
                chart.reflow();
            }
            else {
                $("[id$=hdfValue]").val("Production Data");
                heightmanageFullScreen();
                $("#btnProductionData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
                // $("#btnProductionData").css("backgroundColor", "#007AFF", "color", "#DEE9EC");
                $("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
                $("#btnEvents").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
                $("#btnProductionAndDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");

                var chart = $('#productionDataChart').highcharts();
                chart.reflow();
            }
        });

        function hightManage() {
            if ($("#chkHideGraph").is(':checked')) {
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
                if ($("[id$=hdfValue]").val() == "Production Data") {
                    $("#divPDScroll").show();
                    $("#divDTDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#divPDDScroll").hide();
                } else if ($("[id$=hdfValue]").val() == "Down Time Data") {
                    $("#divPDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#divDTDScroll").show();
                    $("#divPDDScroll").hide();
                } else if ($("[id$=hdfValue]").val() == "Events") {
                    $("#divPDScroll").hide();
                    $("#divEventScroll").show();
                    $("#divDTDScroll").hide();
                    $("#divPDDScroll").hide();
                } else if ($("[id$=hdfValue").val() == "Production&DownData") {
                    $("#divPDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#divDTDScroll").hide();
                    $("#divPDDScroll").show();
                }

                var hight = $(window).height() - 370;
                $("#divPDScroll").css('max-height', hight);
                $("#divDTDScroll").css('max-height', hight);
                //$("#divEventScroll").css('max-height', hight);
            } else {
                $("#divPDScroll").show();
                $("#productionDataChart").show();
                $("#downTimeDataChart").hide();
                $("#divDTDScroll").hide();
                $("#divEventScroll").hide();
                $("#divPDDScroll").hide();
                $("#divPDScroll").css('max-height', 370);
                $("#divDTDScroll").css('max-height', 370);
                //$("#divEventScroll").css('max-height', 370);
            }
        }

        function heightmanageFullScreen() {
            if ($("#chkHideGraph").is(':checked')) {
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
                if ($("[id$=hdfValue]").val() == "Production Data") {
                    $("#divPDScroll").show();
                    $("#divDTDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#divPDDScroll").hide();
                } else if ($("[id$=hdfValue]").val() == "Down Time Data") {
                    $("#divPDScroll").hide();
                    $("#divDTDScroll").show();
                    $("#divEventScroll").hide();
                    $("#divPDDScroll").hide();
                } else if ($("[id$=hdfValue]").val() == "Events") {
                    $("#divPDScroll").hide();
                    $("#divDTDScroll").hide();
                    $("#divEventScroll").show();
                    $("#divPDDScroll").hide();
                } else if ($("[id$=hdfValue]").val() == "Production&DownData") {
                    $("#divPDScroll").hide();
                    $("#divDTDScroll").hide();
                    $("#divEventScroll").hide();
                    $("#divPDDScroll").show();
                }
            }
            else {

                //document.getElementById('containerbottom').setAttribute("style", "width:100%");
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                document.getElementById('DivChart').setAttribute("style", "height:96%;");
                //document.getElementById('DivChart').setAttribute("style", "height:100%");
                document.getElementById('prodGrid').setAttribute("style", "height:60%");
                document.getElementById('divPDScroll').setAttribute("style", "height:100%");
                document.getElementById("divPDScroll").style.overflow = "scroll";
                document.getElementById('divDTDScroll').setAttribute("style", "height:96%");
                //document.getElementById('divEventScroll').setAttribute("style", "height:96%");
                document.getElementById('footerchart').setAttribute("style", "height:40%");
                document.getElementById('productionDataChart').setAttribute("style", "height:95%");
                document.getElementById('downTimeDataChart').setAttribute("style", "height:95%");
                $("#divPDScroll").show();
                $("#productionDataChart").show();
                $("#downTimeDataChart").hide();
                $("#divDTDScroll").hide();
                $("#divEventScroll").hide();
                $("#divPDDScroll").hide();
            }
        }

        $("#btnDownData").click(function () {
            debugger;
            btnText = "DownTimeData";
            $("#btnDownData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
            $("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("#btnEvents").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("#btnProductionAndDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("[id$=hdfValue]").val("Down Time Data");
            document.getElementById("chkHideGraph").style.visibility = "visible";
            $(".hidegraph-eve").show();
            var isfscreen = isFullscreen();
            if (isfscreen == false) {
                if ($("#chkHideGraph").is(':checked')) {
                    $("#productionDataChart").hide();
                    $("#downTimeDataChart").hide();
                    if ($("[id$=hdfValue]").val() == "Down Time Data") {
                        $("#divPDScroll").hide();
                        $("#divEventScroll").hide();
                        $("#divDTDScroll").show();
                        $("#divPDDScroll").hide();
                    } else {
                        $("#divPDScroll").show();
                        $("#divDTDScroll").hide();
                        $("#divEventScroll").hide();
                        $("#divPDDScroll").hide();
                    }
                    var hight = $(window).height() - 370;
                    $("#divPDScroll").css('max-height', hight);
                    $("#divDTDScroll").css('max-height', hight);
                    $("#divEventScroll").css('max-height', hight);
                    $("#divPDDScroll").css('max-height', hight);
                } else {
                    $("#divPDScroll").hide();
                    $("#productionDataChart").hide();
                    $("#divEventScroll").hide();
                    $("#divPDDScroll").hide();
                    $("#downTimeDataChart").show();
                    $("#divDTDScroll").show();
                    $("#divPDScroll").css('max-height', 370);
                    $("#divDTDScroll").css('max-height', 370);
                    $("#divDTDScroll").css('overflow', 'scroll');
                }
            } else {

                //document.getElementById('containerbottom').setAttribute("style", "width:100%");
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                //document.getElementById('DivChart').setAttribute("style", "width:100%");
                document.getElementById('DivChart').setAttribute("style", "height:100%");
                document.getElementById('prodGrid').setAttribute("style", "height:60%");

                document.getElementById('divDTDScroll').setAttribute("style", "height:100%");
                document.getElementById("divDTDScroll").style.overflow = "scroll";
                document.getElementById('footerchart').setAttribute("style", "height:40%");
                document.getElementById('downTimeDataChart').setAttribute("style", "height:95%");
                $("#divPDScroll").hide();
                $("#productionDataChart").hide();
                $("#divEventScroll").hide();
                $("#divPDDScroll").hide();
                $("#downTimeDataChart").show();
                $("#divDTDScroll").show();
            }
            var chart = $('#downTimeDataChart').highcharts();
            chart.reflow();
        });

        function eventVisible() {
            $(".buttonEvt").css("display", "none");
            $(".buttonProductionAndDownData").css("display", "none");
            if (IsPageEnabled("HawkinsPages") == true) {
                $(".buttonEvt").css("display", "");
                //$('btnEvents').show();
            }
            if (IsPageEnabled("ShowProductionandDownDataInVDG") == true) {
                $(".buttonProductionAndDownData").css("display", "");
            }
        }

        $("#btnEvents").click(function () {
            debugger;
            btnText = "Events";

            $("#btnEvents").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
            $("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("#btnProductionAndDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("[id$=hdfValue]").val("Event Data");
            //document.getElementsByClassName(".hidegraph-eve").style.visibility = "hidden";
            $(".hidegraph-eve").hide();
            var isfscreen = isFullscreen();
            if (isfscreen == false) {
                //if ($("#chkHideGraph").is(':checked')) {
                //    $("#productionDataChart").hide();
                //    $("#downTimeDataChart").hide();
                //    if ($("[id$=hdfValue]").val() == "Event Data") {
                //        $("#divPDScroll").hide();
                //        $("#divDTDScroll").hide();
                //        $("#divEventScroll").show();
                //    } else {
                //        $("#divPDScroll").show();
                //        $("#divDTDScroll").hide();
                //        $("#divEventScroll").hide();
                //    }
                //    var hight = $(window).height() - 370;
                //    $("#divPDScroll").css('max-height', hight);
                //    $("#divDTDScroll").css('max-height', hight);
                //    $("#divEventScroll").css('max-height', hight);
                //} else {
                $("#divPDScroll").hide();
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
                $("#divDTDScroll").hide();
                $("#divPDDScroll").hide();
                $("#divEventScroll").show();
                $("#divPDScroll").css('max-height', 370);
                $("#divDTDScroll").css('max-height', 370);
                $("#divDTDScroll").css('overflow', 'scroll');
                $("#divEventScroll").css('min-height', $(window).height() - 370);
                $("#divEventScroll").css('overflow', 'scroll');
                //}
            } else {

                //document.getElementById('containerbottom').setAttribute("style", "width:100%");
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                //document.getElementById('DivChart').setAttribute("style", "width:100%");
                document.getElementById('DivChart').setAttribute("style", "height:100%");
                document.getElementById('prodGrid').setAttribute("style", "height:100%");

                //document.getElementById('divDTDScroll').setAttribute("style", "height:100%");
                //document.getElementById("divDTDScroll").style.overflow = "scroll";

                document.getElementById('divEventScroll').setAttribute("style", "height:100%");
                document.getElementById("divEventScroll").style.overflow = "scroll";

                /*document.getElementById('footerchart').setAttribute("style", "height:5%");*/
                //document.getElementById('downTimeDataChart').setAttribute("style", "height:95%");
                $("#divPDScroll").hide();
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
                $("#divDTDScroll").hide();
                $("#divEventScroll").show();
                $("#divPDDScroll").hide();
            }
            //var chart = $('#downTimeDataChart').highcharts();
            //chart.reflow();
        });

        $("#btnProductionAndDownData").click(function () {
            btnText = "Production&DownData";

            $("#btnProductionAndDownData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
            $("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("#btnEvents").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
            $("[id$=hdfValue]").val("Production&DownData");
            $(".hidegraph-eve").hide();
            var isfscreen = isFullscreen();
            if (isfscreen == false) {

                $("#divPDScroll").hide();
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
                $("#divDTDScroll").hide();
                $("#divPDDScroll").show();
                $("#divEventScroll").hide();
                $("#divPDScroll").css('max-height', 370);
                $("#divDTDScroll").css('max-height', 370);
                $("#divDTDScroll").css('overflow', 'scroll');
                $("#divEventScroll").css('min-height', $(window).height() - 370);
                $("#divEventScroll").css('overflow', 'scroll');
                $("#divPDDScroll").css('min-height', $(window).height() - 370);
                $("#divPDDScroll").css('overflow', 'scroll');
                //}
            } else {
                document.getElementById('containerbottom').setAttribute("style", "height:96%;width:100%;");
                document.getElementById('DivChart').setAttribute("style", "height:100%");
                document.getElementById('prodGrid').setAttribute("style", "height:100%");
                document.getElementById('divPDDScroll').setAttribute("style", "height:100%");
                document.getElementById("divPDDScroll").style.overflow = "scroll";
                $("#divPDScroll").hide();
                $("#productionDataChart").hide();
                $("#downTimeDataChart").hide();
                $("#divDTDScroll").hide();
                $("#divEventScroll").hide();
                $("#divPDDScroll").show();
            }
        })

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
        });


        $(document).on("keydown", "[id$=txtNoofComponents]", function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });


        $(document).on("click", ".pagination-ys a", function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "th a", function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });
        //var gridElem = document.getElementById("divGrid");
        //var chartElem = document.getElementById("DivChart");
        //$(document).on("click", "#btnFullScreen", function () {

        //    if ((document.fullScreenElement && document.fullScreenElement !== null) ||
        //         (!document.mozFullScreen && !document.webkitIsFullScreen)) {
        //        if (document.documentElement.requestFullScreen) {
        //            document.documentElement.requestFullScreen();
        //        } else if (document.documentElement.msRequestFullscreen) {
        //            document.documentElement.msRequestFullscreen();
        //        } else if (document.documentElement.mozRequestFullScreen) {
        //            document.documentElement.mozRequestFullScreen();
        //        } else if (document.documentElement.webkitRequestFullScreen) {
        //            document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
        //        }
        //    } else {
        //        $("#divPDScroll").css('max-height', '400px');
        //        $("#divDTDScroll").css('max-height', '300px');
        //        $("#downTimeDataChart").hide();
        //        $("#divDTDScroll").hide();
        //        if (document.cancelFullScreen) {
        //            document.cancelFullScreen();
        //        } else if (document.msRequestFullscreen) {
        //            document.msRequestFullscreen();
        //        } else if (document.mozCancelFullScreen) {
        //            document.mozCancelFullScreen();
        //        } else if (document.webkitCancelFullScreen) {
        //            document.webkitCancelFullScreen();
        //        }
        //    }
        //});


        $(document).on("change", "input[name='rdobtnPG']", function () {
            var radioValue = $("input[name='rdobtnPG']:checked").val();
            if (radioValue == "Load UnLoad") {
                var chart = $('#productionDataChart').highcharts();
                //chart.series[0].show();
                //chart.series[2].show();
                //chart.series[1].hide();
                //chart.series[3].hide();
                chart.series[0].show();
                chart.series[3].show();
                chart.series[1].hide();
                chart.series[4].hide();
                chart.series[2].hide();
                chart.series[5].hide();
            }
            else if (radioValue == "Cycle Time") {
                var chart = $('#productionDataChart').highcharts();
                //chart.series[0].hide();
                //chart.series[2].hide();
                //chart.series[1].show();
                //chart.series[3].show();
                chart.series[0].hide();
                chart.series[3].hide();
                chart.series[1].show();
                chart.series[4].show();
                chart.series[2].hide();
                chart.series[5].hide();
            }
            else if (radioValue == "All") {
                var chart = $('#productionDataChart').highcharts();
                chart.series[0].show();
                chart.series[3].show();
                chart.series[1].show();
                chart.series[4].show();
                chart.series[2].show();
                chart.series[5].show();
            } else if (radioValue == "Total Time") {
                var chart = $('#productionDataChart').highcharts();
                chart.series[0].hide();
                chart.series[3].hide();
                chart.series[1].hide();
                chart.series[4].hide();
                chart.series[2].show();
                chart.series[5].show();
            }
        })

        $(document).on("change", "input[name='rdobtnGO']", function () {
            var radioValue = $("input[name='rdobtnGO']:checked").val();
            if (radioValue == "Line Graph") {
                if ($("[id$=hdfValue]").val() == "Down Time Data") {
                    var chart = $('#downTimeDataChart').highcharts();
                    chart.series[0].update({
                        type: 'spline'
                    });
                    chart.series[1].update({
                        type: 'spline'
                    });
                } else if ($("[id$=hdfValue]").val() == "Production Data") {
                    var chart = $('#productionDataChart').highcharts();
                    chart.series[2].update({
                        type: 'spline'
                    });
                    chart.series[3].update({
                        type: 'spline'
                    });
                }

            } else if (radioValue == "Bar Graph") {
                if ($("[id$=hdfValue]").val() == "Down Time Data") {
                    var chart = $('#downTimeDataChart').highcharts();
                    chart.series[0].update({
                        type: 'column'
                    });
                    chart.series[1].update({
                        type: 'column'
                    });
                } else if ($("[id$=hdfValue]").val() == "Production Data") {
                    var chart = $('#productionDataChart').highcharts();
                    chart.series[2].update({
                        type: 'column'
                    });
                    chart.series[3].update({
                        type: 'column'
                    });
                }
            }
        });

        $(document).on("click", "#btnAplProductionGraph", function () {
            var prodiongrapValue = $("#txtProductionGraph").val();
            if (prodiongrapValue != "0") {
                var chart = $('#productionDataChart').highcharts();
                chart.yAxis[0].setExtremes(0, prodiongrapValue);
            }
            else {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnteranyvalue")%>")
            }
        })

        $(document).on("click", "#btnApltDownTimeGraph", function () {
            var downtimegrapValue = $("#txtDownTimeGraph").val();
            if (downtimegrapValue != "0") {
                var chart = $('#downTimeDataChart').highcharts();
                chart.yAxis[0].setExtremes(0, downtimegrapValue);
            }
            else {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnteranyvalue")%>")
            }
        });

        $(document).on("click", "[id$=btnhide]", function () {
            if ($("[id$=txtNoofComponents]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterNoOfComponents")%>");
                $("[id$=txtNoofComponents]").focus();
                return false;
            }
            var value = $("[id$=txtNoofComponents]").val();
            if (parseInt(value) < 0 || parseInt(value) > 100) {
                alert("<%=GetLocalResourceObject("Pleaseenterthenumberbetween1to100")%>");
                $("[id$=txtNoofComponents]").focus();
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            $('#myModal').modal('hide');
        });

        $(document).on("click", "#btnUpdate", function () {
            if ($("#ddlComponent").val() == null) {
                alert("<%=GetLocalResourceObject("PleaseSelectComponent")%>");
                $("#ddlComponent").focus();
                return false;
            }
            if ($("[id$=txtStdCycleTime]").val() == "" || $("[id$=txtStdCycleTime]").val() == "0") {
                alert("<%=GetLocalResourceObject("PleaseEnterStdCycleTimeValuegreaterthanzero")%>");
                $("[id$=txtStdCycleTime]").focus();
                return false;
            }
            if (IsPageEnabled("KTASpindlePages") == true) {
                var oldCycleTime = parseFloat($('#hdnStdCycleTime').val());
                var newCycleTime = parseFloat($("[id$=txtStdCycleTime]").val());
                if (newCycleTime > oldCycleTime) {
                    $('#txtUserid').val("");
                    $('#txtPassword').val("");
                    $('#lblCycleTimeUpdateLoginMsg').text("");
                    $('#cycleTimeUpdateLoginModal').modal('show');
                    return;
                }
            }
            <%--if ($("[id$=txtStdLoadTime]").val() == "" || $("[id$=txtStdLoadTime]").val() == "0") {
                alert("<%=GetLocalResourceObject("PleaseEnterStdLoadTimeValuegreaterthanzero")%>");
                $("[id$=txtStdLoadTime]").focus();
                return false;
            }--%>
            updateStandardTime();
        });

        function btnCycleTimeUpdateConfirmationClick() {
            if ($('#txtUserid').val() == "" || $('#txtPassword').val() == "") {
                $('#lblCycleTimeUpdateLoginMsg').text("Enter Userid and Password.");
                return;
            }
            $.ajax({
                async: false,
                type: "POST",
                url: "VDGScreen.aspx/getCycleTimeUpdateValidationResult",
                contentType: "application/json; charset=utf-8",
                data: '{userName:"' + $('#txtUserid').val() + '", passWord:"' + $('#txtPassword').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var result = response.d;
                    if (result == "Success") {
                        updateStandardTime();
                        $('#cycleTimeUpdateLoginModal').modal('hide');
                    }
                    else {
                        $('#lblCycleTimeUpdateLoginMsg').text(result);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }

        $(document).on("click", "[id$=btnOkRemarks]", function () {
            debugger;
            if ($("[id$=txtRemarksArea]").val() == "" && $("[id$=txtActionTaken]").val() == "") {
                alert("<%=GetLocalResourceObject("PleaseEnterRemark")%>");
                $("[id$=txtRemarksArea]").focus();
                return false;
            }
            $("#myRemarkScreen").modal('hide');
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "[id$=btnExportData]", function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            setTimeout(function () {
                $.unblockUI({});
            }, 10000);
        });

        $(document).on("click", "#lblExportData", function () {
            $("[id$=btnExportData]").trigger('click');
        });

        $(document).on("click", "[id$=btnAnalysis]", function () {
            if ($("[id$=ddlMachineId]").val() == null) {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseSelectMachineId")%>");
                $("[id$=ddlMachineId]").focus();
                return false;
            }
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
            var machineId = $("[id$=ddlMachineId]").val();
            var fromdate = $("[id$=txtFromDate]").val();
            var Todate = $("[id$=txtToDate]").val();
            window.open("DataAnalysis.aspx?machineId=" + machineId + "&fromdate=" + fromdate + "&Todate=" + Todate);
            return false;
        });

        function goToStatistic() {
            debugger;
            if ($("[id$=ddlMachineId]").val() == null) {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseSelectMachineId")%>");
                $("[id$=ddlMachineId]").focus();
                return false;
            }
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
            let fromDatePart = from.split(" ")[0];
            let toDatePart = to.split(" ")[0];
            debugger;
            //  date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
            var diffe = dateDiffInDays(new Date(fromDatePart.split("-").reverse().join("-")), new Date(toDatePart.split("-").reverse().join("-")));
            if (diffe > 15) {
                alert("Difference between to date and from date cannot be more than 15 days.");
                return false;
            }
            var dateCom = compareDates(fromDatePart, toDatePart);
            if (dateCom == 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            debugger;
            if ($("#ddlComponent").val() == null) {
                alert("<%=GetLocalResourceObject("PleaseSelectComponent")%>");
                $("#ddlComponent").focus();
                return false;
            }
            var machineId = $("[id$=ddlMachineId]").val();
            var fromdate = $("[id$=txtFromDate]").val();
            var Todate = $("[id$=txtToDate]").val();
            var component = $("#ddlComponent").val();
            window.open("Statistics.aspx?machineId=" + machineId + "&fromdate=" + fromdate + "&Todate=" + Todate + "&component=" + component);
            return false;
        }
        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            debugger;
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }
        function compareDates(date1, date2) {
            debugger;
            return invert(date1).localeCompare(invert(date2));
        }
        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        <%--$(document).on("click", "[id$=btnStatistics]", function () {
            debugger;
            if ($("[id$=ddlMachineId]").val() == null) {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseSelectMachineId")%>");
                $("[id$=ddlMachineId]").focus();
                return false;
            }
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

            // date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
            var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
            if (diffe > 15) {
                alert("Difference between to date and from date cannot be more than 15 days.");
                return false;
            }
            var dateCom = compareDates(from, to);
            if (dateCom == 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            if ($("#ddlComponent").val() == null) {
                alert("<%=GetLocalResourceObject("PleaseSelectComponent")%>");
                $("#ddlComponent").focus();
                return false;
            }
            var machineId = $("[id$=ddlMachineId]").val();
            var fromdate = $("[id$=txtFromDate]").val();
            var Todate = $("[id$=txtToDate]").val();
            var component = $("#ddlComponent").val();
            window.open("Statistics.aspx?machineId=" + machineId + "&fromdate=" + fromdate + "&Todate=" + Todate + "&component=" + component);
            return false;
        });--%>

        $(document).on("click", "#btnStdTime", function () {
            if ($("#ddlComponent").val() == null) {
                alert("<%=GetLocalResourceObject("PleaseSelectComponent")%>");
                $("#ddlComponent").focus();
                return false;
            }
            getUpdateStdTimes();
        });

        $(document).on("change", "#ddlComponent", function () {
            componentPerformance();
        });

        function getAutodataID(slno, param) {
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/GetRemarksID",
                contentType: "application/json; charset=utf-8",
                data: '{SlNo:"' + slno + '", Param:"' + param + '"}',
                dataType: "json",
                success: function (response) {
                    var itemdata = response.d;
                    remarksid = itemdata;
                    if (itemdata != null) {
                        if (itemdata.length > 0) {
                            if (itemdata[0] != "") {
                                //  var remarks = $(this).find("span").html();
                                $("[id$=txtRemarksArea]").val(itemdata[1]); //remarks
                                $("[id$=hdfRemarks]").val(itemdata[0]); //autoid
                                $("[id$=txtActionTaken]").val(itemdata[2]); //ActionTaken
                                $("[id$=hdfActionTaken]").val(itemdata[0]);
                                if (IsPageEnabled("SKSPages") == true) {
                                    if (param == "DownData") {
                                        $("#myRemarkScreen").modal('show');
                                    }
                                }
                                else {
                                    $("#myRemarkScreen").modal('show');
                                }
                            }
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }
        function getAutodataID_ModifiedData(slno, param) {
            debugger;
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/GetModifiedDataID_VDG",
                contentType: "application/json; charset=utf-8",
                data: '{SlNo:"' + slno + '", Param:"' + param + '"}',
                dataType: "json",
                success: function (response) {
                    var itemdata = response.d;
                    debugger;
                    $('#gvModifiedData tbody').empty();
                    if (itemdata.length > 0) {
                        $.each(itemdata, function (index, item) {
                            var row = '<tr>';
                            row += '<td>' + item.RecordType + '</td>';
                            row += '<td>' + item.DownID + '</td>';
                            row += '<td>' + item.Component + '</td>';
                            row += '<td>' + item.Operation + '</td>';
                            row += '<td>' + item.Operator + '</td>';
                            row += '<td>' + item.PartsCount + '</td>';
                            row += '<td>' + item.UpdatedTS + '</td>';
                            row += '</tr>';
                            $('#gvModifiedData tbody').append(row);
                        });
                    }
                    else {
                        var row = '<tr>';
                        row += '<td colspan="6">No Data Available</td>';
                        row += '</tr>';
                        $('#gvModifiedData tbody').append(row);
                    }
                    if (param == "ProductionData") {
                        $("#gvModifiedData th:nth-child(2), #gvModifiedData td:nth-child(2)").hide();
                        $("#gvModifiedData th:nth-child(6), #gvModifiedData td:nth-child(6)").show();
                    }
                    else if (param == "DownData") {
                        $("#gvModifiedData th:nth-child(2), #gvModifiedData td:nth-child(2)").show();
                        $("#gvModifiedData th:nth-child(6), #gvModifiedData td:nth-child(6)").hide();
                    }
                    else {
                        $("#gvModifiedData th:nth-child(2), #gvModifiedData td:nth-child(2)").show();
                        $("#gvModifiedData th:nth-child(6), #gvModifiedData td:nth-child(6)").show();
                    }
                    if (param == "ProductionData") param = "Production Data";
                    if (param == "DownData") param = "Down Data";
                    if (param == "ProductionDownData") param = "Production Down Data";
                    $("#modifiedDatamodalTitle").text('Modified Data History - ' + param);
                    $.unblockUI({});
                    $("#myModifiedData").modal('show');
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {

            setContextMenuData();

            firstTimeLaod();
            pagingLaodFun();
            var winHeight = $(window).height();
            if (winHeight < 650) {
                winHeight = (winHeight - 350);
                console.log('min');
            } else {
                winHeight = (winHeight - 670);
                console.log('max');
            }
            $("#productionDataChart").height(winHeight);
            $("#downTimeDataChart").height(winHeight);
            // $("[id$=gridProductionData]").tablesorter();
            // $("[id$=gridDownTimeData]").tablesorter();
            $("[id$=PEChart]").css("width", "auto");
            $("[id$=AEChart]").css("width", "auto");
            $("[id$=OEChart]").css("width", "auto");
            //   $('[id$=gridProductionData]').tablePagination({});
            //var winHeight = $(window).height();
            //winHeight = (winHeight - 300);
            //$("#productionDataChart").height(winHeight);
            //$("#downTimeDataChart").height(winHeight);
            dateTimePicker();
            $.unblockUI({});
            //hideAndShowChart();
            //$("[id$=btnRefresh]").click(function () {
            //    if ($("[id$=txtFromDate]").val() == "") {
            //        alert("Please enter the From Date !!");
            //        $("[id$=txtFromDate]").focus();
            //        return false;
            //    }
            //    if ($("[id$=txtToDate]").val() == "") {
            //        alert("Please enter the To Date !!");
            //        $("[id$=txtToDate]").focus();
            //        return false;
            //    }
            //      $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            //    GetProductionChartData();
            //    GetDownTimeChartData();
            //    bindComponent();
            //    //componentPerformance();
            //});

            jQuery('.numbersOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });

            //$("[id*=gridDownTimeData] tr").closest("tr").click(function () {
            //    alert($(this).closest("tr").find("input").val());
            //});

            $("[id*=gridDownTimeData] tr.dtdrow").find("td:nth-last-child(2)").click(function () {
                debugger;
                var remarkId = $(this).find("input").val();
            //    if (remarkId != undefined) {
            //        var remarks = $(this).find("span").html();
            //        $("[id$=txtRemarksArea]").val(remarks);
            //        $("[id$=hdfRemarks]").val(remarkId);
            //        $("#myRemarkScreen").modal('show');
            //    }
                /*else*/ {
                    let slno = $($(this).closest('tr').children()[0]).text();
                    getAutodataID(slno, "DownData");
                }

                //    $("[id$=txtActionTaken]").hide();
                //    $("[id$=txtRemarksArea]").show();
            });

            $("[id$=gridDownTimeData] tr.dtdrow").find("td:last").click(function () {
                //var ActionTakenID = $(this).find("input").val();
                //if (ActionTakenID != undefined) {
                //    var ActionTaken = $(this).find("span").html();
                //    $("[id$=txtActionTaken]").val(ActionTaken);
                //    $("[id$=hdfActionTaken").val(ActionTakenID);
                //    $("#myRemarkScreen").modal('show');
                //}
                /*else*/ {
                    let slno = $($(this).closest('tr').children()[0]).text();
                    getAutodataID(slno, "DownData");
                }
                //$("[id$=txtRemarksArea]").hide();
                //$("[id$=txtActionTaken]").show();
            });

            $("[id*=gridProductionData] tr.pdrow").find("td:last").click(function () {
                var remarkId = $(this).find("input").val();
                if (remarkId != undefined) {
                    var remarks = $(this).find("span").html();
                    $("[id$=txtRemarksArea]").val(remarks);
                    $("[id$=hdfRemarks]").val(remarkId);
                    $("#myRemarkScreen").modal('show');
                }
                else {
                    let slno = $($(this).closest('tr').children()[0]).text();
                    getAutodataID(slno, "ProductionData");
                }
            });

            $("input[value='Get Last N']").click(function () {
                $("[id$=txtNoofComponents]").focus();
            });


            //$("[id$=btnStatistics]").click(function () {
            //    if ($("[id$=ddlMachineId]").val() == null) {
            //        alert("Please select Machine Id !!");
            //        $("[id$=ddlMachineId]").focus();
            //        return false;
            //    }
            //    if ($("[id$=txtFromDate]").val() == "") {
            //        alert("Please enter the From Date !!");
            //        $("[id$=txtFromDate]").focus();
            //        return false;
            //    }
            //    if ($("[id$=txtToDate]").val() == "") {
            //        alert("Please enter the To Date !!");
            //        $("[id$=txtToDate]").focus();
            //        return false;
            //    }
            //    if ($("#ddlComponent").val() == null) {
            //        alert("Please select Component !!");
            //        $("#ddlComponent").focus();
            //        return false;
            //    }
            //    var machineId = $("[id$=ddlMachineId]").val();
            //    var fromdate = $("[id$=txtFromDate]").val();
            //    var Todate = $("[id$=txtToDate]").val();
            //    var component = $("#ddlComponent").val();
            //    window.open("Statistics.aspx?machineId=" + machineId + "&fromdate=" + fromdate + "&Todate=" + Todate + "&component=" + component);
            //    return false;
            //})

            //-----------------------------------------------------------------------

            if ($("[id$=hdfHideValue]").val() == "modalClose") {
                $('#myModalGetstdTime').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                $('#myModalUpdateconfirm').modal('show');
                console.log('modalClose');
            }
            if ($("[id$=hdfHideValue]").val() == "modalOpen") {
                console.log('modalOpen');
                $('#myModalGetstdTime').modal('show');
            }
            if ($("[id$=hdnvalueGetN]").val() == "modalCloseGetN") {
                $('#myModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                //$("[id$=btnRefresh]").trigger('click');
                //alert("ttt");
                //GetProductionChartData();
                //GetDownTimeChartData();
                //bindComponent();
                $("[id$=hdnvalueGetN]").val('');
                console.log('modalCloseGetN');
            }
            if ($("[id$=hdnvalueGetN]").val() == "modalOpenGetN") {
                $('#myModal').modal('show');
                console.log('modalOpenGetN');
            }

        });

        function GetProductionChartData() {
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/GetProductionDataGraph",
                contentType: "application/json; charset=utf-8",
                data: '{machineId:"' + $("[id$=ddlMachineId]").val() + '", strfromDate:"' + $("[id$=txtFromDate]").val() + '", strtoDate:"' + $("[id$=txtToDate]").val() + '",windowSize:"' + $(window).width() + '"}',
                dataType: "json",
                success: OnSuccessGetItems,
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function GetDownTimeChartData() {
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/GetDownTimeDataGraph",
                contentType: "application/json; charset=utf-8",
                data: '{machineId:"' + $("[id$=ddlMachineId]").val() + '", strfromDate:"' + $("[id$=txtFromDate]").val() + '", strtoDate:"' + $("[id$=txtToDate]").val() + '"}',
                dataType: "json",
                success: OnSuccessGetDownTimeData,
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function OnSuccessGetItems(Result) {
            var size =  <%= fontSize %>;
            var sizeLable = (size - 3 + "px");
            var sizeText = (size + "px");
            drawProdctionChart(Result.d, sizeLable);
        }

        function OnSuccessGetDownTimeData(Result) {
            var size =  <%= fontSize %>;
            var sizeLable = (size - 3 + "px");
            var sizeText = (size + "px");
            drawDownTimeChart(Result.d, sizeLable)
        }

        function drawProdctionChart(data, sizeLable) {

            $('#productionDataChart').highcharts({
                colors: ['#FF0000', '#0000EF', '#FF9655', '#00F500', '#225822', '#24CBE5', '#64E572',
                    '#FFF263', '#6AF9C4'],
                credits: {
                    enabled: false
                },
                exporting: { enabled: false },
                title: {
                    useHTML: true,
                    //text: 'Production Data Graph',
                    text: '<img src="Image/ChartImg/increasingbars.png" style="margin-top: -7px;cursor: pointer;color:black" data-toggle="modal" data-target="#myDTDG" /> <%=GetLocalResourceObject("ProductionDataGraph")%>',

                    align: 'left',
                    style: {
                        fontWeight: '600',
                        color: "#0066CC",
                        fontSize: '15px'
                    }
                },
                yAxis: {
                    min: 0,

                    title: {
                        //text: '<%=GetLocalResourceObject("Time(sec)")%>',
                        text: 'Time (hh:mm:ss)',
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
                            var time = this.value;
                            var hours1 = parseInt(time / (60 * 60));
                            var mins1 = parseInt((parseInt(time / 60)) % 60);
                            var secs1 = parseInt(time % (60));
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                        }
                    },
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
                xAxis: {
                    categories: data.categories1,
                    min: 0,
                    max: 20,
                    scrollbar: {
                        enabled: true
                    }, labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'top',
                    floating: true,
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false
                },

                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    var index = this.category;
                                    debugger;
                                    var i = 1;

                                    if ($("[id$=gridProductionData] tbody").find('#pdrow' + index).offset() != undefined) {
                                        var columnIndex;
                                        $("[id$=gridProductionData] th").each(function (index) {
                                            if ($(this).text() == "IsModified") {
                                                columnIndex = index;
                                                return false;
                                            }
                                        });
                                        $("[id$=gridProductionData] tbody .pdrow").each(function () {
                                            debugger;
                                            var cell = $(this).find("td").eq(columnIndex);
                                            var cellValue = "";
                                            var hiddenField = cell.find("input[type='hidden']");
                                            if (hiddenField.length > 0) {
                                                var hiddenFieldValue = hiddenField.val();
                                                if (hiddenFieldValue !== undefined && hiddenFieldValue !== null) {
                                                    cellValue = hiddenFieldValue;
                                                } else {
                                                    cellValue = cell.text().trim();
                                                }
                                            } else {
                                                cellValue = cell.text().trim();
                                            }
                                            if (cellValue === "True") {
                                                debugger;
                                                this.style.backgroundColor = "#" + ModifiedBackColor;
                                            }
                                            else {
                                                debugger;
                                                if (this.style.backgroundColor == "rgb(255, 255, 0)") {
                                                    this.style.backgroundColor = "rgb(255, 255, 255)";
                                                }
                                                if (i % 2 == 0) {
                                                    this.style.backgroundColor = "rgb(204, 255, 255)";
                                                }
                                                else {
                                                    this.style.backgroundColor = "rgb(255, 255, 255)";
                                                }

                                            }
                                            i++;
                                        });
                                        $('#divPDScroll').scrollTop($("[id$=gridProductionData] tbody").find('#pdrow' + index).offset().top - $("#divPDScroll").offset().top + $("#divPDScroll").scrollTop() - $("[id$=gridProductionData] tr th").height() - 6);
                                        $("[id$=gridProductionData] tbody").find('#pdrow' + index).css('background-color', '#FFFF00');

                                        return false;
                                    }

                                }
                            }
                        }
                    }
                },
                series: data.series
            });
            $("[id$=liMenu]").click(function () {
                var chart = $('#productionDataChart').highcharts();
                chart.reflow();
            });
        }
        function drawDownTimeChart(data, sizeLable) {

            $('#downTimeDataChart').highcharts({
                colors: ['#FF0000', '#FF40FF', '#FF40FF', '#225822', '#24CBE5', '#64E572',
                    '#FF9655', '#FFF263', '#6AF9C4'],
                credits: {
                    enabled: false
                },
                exporting: { enabled: false },
                title: {
                    useHTML: true,
                    text: '   <img src="Image/ChartImg/increasingbars.png" style="margin-top: -7px; cursor: pointer;color:black" data-toggle="modal" data-target="#myDTDG" /> <%=GetLocalResourceObject("DownTimeDataGraph")%>',
                    // text: 'Down Time Data Graph',
                    align: 'left',
                    style: {
                        fontWeight: '600',
                        color: "#0066CC",
                        fontSize: '15px'
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        //text: '<%=GetLocalResourceObject("Time(sec)")%>',
                        text: 'Time (hh:mm:ss)',
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
                            var time = this.value;
                            var hours1 = parseInt(time / (60 * 60));
                            var mins1 = parseInt((parseInt(time / 60)) % 60);
                            var secs1 = parseInt(time % (60));
                            return (hours1 < 10 ? '0' + hours1 : hours1) + ':' + (mins1 < 10 ? '0' + mins1 : mins1) + ':' + (secs1 < 10 ? '0' + secs1 : secs1);
                        }
                    },
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
                xAxis: {
                    categories: data.categories1,
                    min: 0,
                    max: 20,
                    scrollbar: {
                        enabled: true
                    },
                    labels: {
                        style: {
                            fontSize: sizeLable
                        }
                    },
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'top',
                    floating: true,
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false,
                    enabled: true
                },
                plotOptions: {
                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function () {
                                    var index = this.category;
                                    var i = 1;
                                    //if ($("[id$=gridDownTimeData] tbody").find('#dtdrow' + index).offset() != undefined) {
                                    //    $("[id$=gridDownTimeData] tbody .dtdrow").each(function () {
                                    //        //if (this.style.backgroundColor == "rgb(255, 255, 0)") {
                                    //        //    this.style.backgroundColor = "";
                                    //        //}
                                    //        //if (i % 2 === 0) { this.style.backgroundColor = "rgb(204, 255, 255)"; }
                                    //        //else { this.style.backgroundColor = ""; }
                                    //        //i++;
                                    //    });
                                    //    // $("[id$=gridDownTimeData] tbody tr").removeAttr('style');
                                    //    $('#divDTDScroll').scrollTop($("[id$=gridDownTimeData] tbody").find('#dtdrow' + index).offset().top - $("#divDTDScroll").offset().top + $("#divDTDScroll").scrollTop() - $("[id$=gridDownTimeData] tr th").height() - 6);
                                    //    $("[id$=gridDownTimeData] tbody").find('#dtdrow' + index).css('background-color', '#FFFF00');
                                    //    return false;
                                    //}
                                    if ($("[id$=gridDownTimeData] tbody").find('#dtrow' + index).offset() != undefined) {
                                        var columnIndex;
                                        $("[id$=gridDownTimeData] th").each(function (index) {
                                            if ($(this).text() == "IsModified") {
                                                columnIndex = index;
                                                return false;
                                            }
                                        });
                                        $("[id$=gridDownTimeData] tbody .dtrow").each(function () {
                                            debugger;
                                            var cell = $(this).find("td").eq(columnIndex);
                                            var cellValue = "";
                                            var hiddenField = cell.find("input[type='hidden']");
                                            if (hiddenField.length > 0) {
                                                var hiddenFieldValue = hiddenField.val();
                                                if (hiddenFieldValue !== undefined && hiddenFieldValue !== null) {
                                                    cellValue = hiddenFieldValue;
                                                } else {
                                                    cellValue = cell.text().trim();
                                                }
                                            } else {
                                                cellValue = cell.text().trim();
                                            }
                                            if (cellValue === "True") {
                                                debugger;
                                                this.style.backgroundColor = "#" + ModifiedBackColor;
                                            }
                                            else {
                                                debugger;
                                                if (this.style.backgroundColor == "rgb(255, 255, 0)") {
                                                    this.style.backgroundColor = "rgb(255, 255, 255)";
                                                }
                                                if (i % 2 == 0) {
                                                    this.style.backgroundColor = "rgb(204, 255, 255)";
                                                }
                                                else {
                                                    this.style.backgroundColor = "rgb(255, 255, 255)";
                                                }
                                            }
                                            i++;
                                        });
                                        $('#divDTDScroll').scrollTop($("[id$=gridDownTimeData] tbody").find('#dtdrow' + index).offset().top - $("#divDTDScroll").offset().top + $("#divDTDScroll").scrollTop() - $("[id$=gridDownTimeData] tr th").height() - 6);
                                        $("[id$=gridDownTimeData] tbody").find('#dtdrow' + index).css('background-color', '#FFFF00');
                                        return false;

                                        return false;
                                    }
                                }
                            }
                        }
                    }
                },
                series: data.series
            });
            $("[id$=liMenu]").click(function () {
                var chart = $('#downTimeDataChart').highcharts();
                chart.reflow();
            });
        }

        function bindComponent() {
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/BindComponentData",
                contentType: "application/json; charset=utf-8",
                data: '{machineId:"' + $("[id$=ddlMachineId]").val() + '", strfromDate:"' + $("[id$=txtFromDate]").val() + '", strtoDate:"' + $("[id$=txtToDate]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var customers = response.d;
                    $("#ddlComponent option").remove();
                    $(customers).each(function (customers, el) {
                        $("#ddlComponent").append('<option value="'
                            + escapeDoubleQuotes(el) + '">' + el + '</option>');
                        //$("#ddlComponent").append('<option>' + el + '</option>');
                    });
                    componentPerformance();
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function escapeDoubleQuotes(el) {
            return el.replace(/"/g, "&#34;");
        }

        function updateStandardTime() {
            if (IsPageEnabled("KTASpindlePages")) {
                var obj = { machineId: $("[id$=ddlMachineId]").val(), componentId: $("#ddlComponent").val(), stdCycleTime: $("[id$=txtStdCycleTime]").val(), stdLoadTime: $("[id$=txtStdLoadTime]").val(), updateAllMachines: $("#chkUpdateAllMachines").is(":checked") };
            }
            else {
                var obj = { machineId: $("[id$=ddlMachineId]").val(), componentId: $("#ddlComponent").val(), stdCycleTime: $("[id$=txtStdCycleTime]").val(), stdLoadTime: $("[id$=txtStdLoadTime]").val(), updateAllMachines: false };
            }
            var ajaxData = JSON.stringify(obj);
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/updateStandardTimeData",
                contentType: "application/json; charset=utf-8",
                //  data: '{machineId:"' + $("[id$=ddlMachineId]").val() + '", componentId:"' + $("#ddlComponent").val() + '", stdCycleTime:"' + $("[id$=txtStdCycleTime]").val() + '", stdLoadTime:"' + $("[id$=txtStdLoadTime]").val() + '"}',
                data: ajaxData,
                dataType: "json",
                success: function (response) {
                    var customers = response.d;
                    alert("Data Updated Successfully");
                    $('#myModalGetstdTime').modal('hide');
                    return false;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function getUpdateStdTimes() {

            var obj = { machineId: $("[id$=ddlMachineId]").val(), componentId: $("#ddlComponent").val() };
            var ajaxData = JSON.stringify(obj);
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/GetUpdateStdTimes",
                contentType: "application/json; charset=utf-8",
                // data: '{machineId:"' + $("[id$=ddlMachineId]").val() + '", componentId:"' + $("#ddlComponent").val() + '"}',
                data: ajaxData,
                dataType: "json",
                success: function (response) {
                    var customers = response.d;
                    var data = $('#ddlComponent').val();
                    var arr = data.split('|');
                    $("[id$=lblComponentID]").html(arr[0]);
                    $("[id$=lblOperationNo]").html(arr[1]);
                    $("[id$=lblMachineName]").html($("[id$=ddlMachineId]").val());
                    if (customers != null) {
                        if (customers[0] == "Standard Time (in seconds)") {
                            $("#stdCycleTime").html('in seconds');
                            $("#stdLoadTime").html('in seconds');
                        } else {
                            $("#stdCycleTime").html('in minutes');
                            $("#stdLoadTime").html('in minutes');
                        }
                        //$("[id$=lblstdHead]").html(customers[0]);
                        $("[id$=txtStdLoadTime]").val(customers[1]);
                        $("[id$=txtStdCycleTime]").val(customers[2]);
                        $('#hdnStdCycleTime').val(customers[2]);
                    }
                    $('#myModalGetstdTime').modal('show');
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function componentPerformance() {
            var obj = { machineId: $("[id$=ddlMachineId]").val(), strfromDate: $("[id$=txtFromDate]").val(), strtoDate: $("[id$=txtToDate]").val(), component: $("#ddlComponent").val() };
            var ajaxData = JSON.stringify(obj);
            $.ajax({
                type: "POST",
                url: "VDGScreen.aspx/ComponentPerformanceData",
                contentType: "application/json; charset=utf-8",
                //data: '{machineId:"' + $("[id$=ddlMachineId]").val() + '", strfromDate:"' + $("[id$=txtFromDate]").val() + '", strtoDate:"' + $("[id$=txtToDate]").val() + '", component:"' + $("#ddlComponent").val() + '"}',
                data: ajaxData,
                dataType: "json",
                success: function (response) {
                    var customers = response.d;
                    if (customers != null) {
                        $("[id$=lblC1]").html(customers.StCycleTime);
                        $("[id$=lblC2]").html(customers.AvgCycleTime);
                        $("[id$=lblC3]").html(customers.StLoadTime);
                        $("[id$=lblC4]").html(customers.AvgLoadTime);
                        $("[id$=lblC7]").html(customers.OperationCount);
                        $("[id$=lblC5]").html(customers.SpeedRatio);
                        $("[id$=lblC6]").html(customers.LoadRatio);
                    } else {
                        $("[id$=lblC1]").html("---");
                        $("[id$=lblC2]").html("---");
                        $("[id$=lblC3]").html("---");
                        $("[id$=lblC4]").html("---");
                        $("[id$=lblC7]").html("---");
                        $("[id$=lblC5]").html("---");
                        $("[id$=lblC6]").html("---");
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }


    </script>
</asp:Content>

