<%@ Page Title="Reports" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="TafeReports.aspx.cs" Inherits="Web_TPMTrakDashboard.TAFE.TafeReports" %>

<asp:Content ID="MainContentArea" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <script src="/Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="/Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }

        #tblfilter tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #tblfilter tbody tr:nth-child(even) {
            background-color: #DCDCDC;
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

        .cssclass {
            min-width: 350px;
        }
    </style>
    <div class="row" style="text-align: center; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
        <asp:HiddenField ID="width" runat="server" />
        <asp:HiddenField ID="height" runat="server" />
    </div>
    <div class="row">
        <%--   <div class="col-md-3"></div>--%>
        <div class="col-md-9" style="margin: auto;">
            <h1 class="text-center login-title commontd"><%=GetLocalResourceObject("Report") %></h1>
            <div class="account-wall">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-signin">
                        <asp:UpdatePanel ID="upadetPanalRepor" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnGenerate" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblfilter" class="table table-bordered table-striped" style="width: auto">
                                    <tr>
                                        <td>
                                            <b>Report Type</b></td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" meta:resourcekey="ddlReportTypeResource1" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                <asp:ListItem Value="CategoryWiseOEEAndLossTimeReport">OEE And Loss Time Report</asp:ListItem>
                                                <asp:ListItem Value="HoldReport" meta:resourcekey="ListItemResource3">Hold Report</asp:ListItem>
                                                <asp:ListItem Value="RejectionReport" meta:resourcekey="ListItemResource4">Rejection Report</asp:ListItem>
                                                <asp:ListItem Value="BatchWiseReport" meta:resourcekey="ListItemResource6">Batch Wise Report</asp:ListItem>
                                                <asp:ListItem Value="LineMeterReport" meta:resourcekey="ListItemResource7">Line Meter Report</asp:ListItem>
                                            <%--    <asp:ListItem Value="PDIReport" meta:resourcekey="ListItemResource8">PDI Report</asp:ListItem>--%>
                                            </asp:DropDownList></td>
                                    </tr>

                                    <tr id="trFromDate" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="YYYY-MMM-DD" MaxLength="15" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trToDate" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="YYYY-MMM-DD" MaxLength="15" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trfromdatetimeconsolidate" runat="server">
                                        <td style="min-width: 80px;" runat="server"><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>
                                        <td class="input-group" style="min-width: 220px;" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txttimeconsolidate_fromdate" runat="server" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trtodatetimeconsolidate" runat="server">
                                        <td runat="server"><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b></td>
                                        <td class="input-group" style="min-width: 220px;" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txttimeconsolidate_todate" runat="server" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trPlant" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","PlantID") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlPlantId" runat="server" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" CssClass="select form-control loadData cssclass" AutoPostBack="True">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trLine" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","LineID") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlLineId" runat="server" OnSelectedIndexChanged="ddlLineId_SelectedIndexChanged" CssClass="  form-control cssclass" AutoPostBack="True">
                                                <asp:ListItem Value="All">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trMachine" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","MachineId") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="  form-control cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged">
                                                <asp:ListItem Value="All">All</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trPartID" runat="server">
                                        <td runat="server">
                                            <b><%=GetGlobalResourceObject("CommanResource","ComponentID") %></b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlComponent" runat="server" CssClass="  form-control cssclass" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trCategory" runat="server">
                                        <td runat="server">
                                            <b><%=GetLocalResourceObject("Category") %></b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlcategory" runat="server" CssClass="select form-control cssclass" AutoPostBack="True">
                                                <asp:ListItem Value="Material">Material Rejection</asp:ListItem>
                                                <asp:ListItem Value="Process">Process Rejection</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr id="trmonthlydate" runat="server">
                                        <td runat="server"><b><%=GetGlobalResourceObject("CommanResource","FromDate") %></b></td>
                                        <td runat="server">
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource1" Style="width: 70px; display: inline;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trmonthlytodate" runat="server">
                                        <td runat="server"><b><%=GetGlobalResourceObject("CommanResource","ToDate") %></b></td>
                                        <td runat="server">
                                            <asp:TextBox ID="txttoyear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" meta:resourcekey="txtYearResource2" Style="width: 70px; display: inline;"></asp:TextBox>
                                            <asp:TextBox ID="txttomonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month" meta:resourcekey="txtMonthResource2" Style="width: 70px; display: inline;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trSerialNumber" runat="server">
                                        <td runat="server">
                                            <b>Serial No.</b> </td>
                                        <td runat="server">
                                            <asp:TextBox runat="server" ID="txtSlnoSearch" ClientIDMode="Static" CssClass="form-control" Style="display: inline-block; width: 200px" placeholder="Contains search.."></asp:TextBox>&nbsp;&nbsp;
                                            <asp:LinkButton runat="server" ClientIDMode="Static" CssClass="glyphicon glyphicon-search" ID="lnkSlnoSearch" OnClick="lnkSlnoSearch_Click" Style="font-size: 18px; vertical-align: middle;"></asp:LinkButton>
                                            <asp:DropDownList ID="ddlSlno" runat="server" CssClass="  form-control cssclass">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" meta:resourcekey="btnGenerateResource1" OnClick="btnGenerate_Click" />
                                            <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:CommanResource, btnCancel %>" CssClass="btn btn-primary" meta:resourcekey="btnCancelResource1" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Include Date Range Picker -->
    <script type="text/javascript">
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        $(document).ready(function () {
            $.unblockUI({});
            $('[id$=txttoyear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txttomonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txttimeconsolidate_fromdate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txttimeconsolidate_todate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $("[id$=txttimeconsolidate_fromdate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_todate]').data("DateTimePicker").minDate(e.date);
            });

            $("[id$=txttimeconsolidate_todate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_fromdate]').data("DateTimePicker").maxDate(e.date);
            });

            $(".date").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());

            $(".loadData").change(function () {
                $.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif"/>' });
            });
        });

        function invert(date) {

            date = date.split(/[/-]/).reverse().join("")
            return date;
        }

        function compareDates(date1, date2) {

            var Fromdate = new Date(date1);
            var Todate = new Date(date2);
            if ((Todate - Fromdate) >= 0)
                return 0;
            else
                return 1;
        }

        $(document).on("click", "[id$=btnGenerate]", function () {
            if (($("[id$=ddlReportType] option:selected").val() == "HoldReport") || ($("[id$=ddlReportType] option:selected").val() == "RejectionReport")) {

                if ($("[id$=txtFromDate]").val() == "") {
                    alert("Please select Start Date");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
                else if ($("[id$=txtToDate]").val() == "") {
                    alert("Please select To End Date");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
                else {
                    var from = $("[id$=txtFromDate]").val();
                    var to = $('[id$=txtToDate]').val();

                    var dateCom = compareDates(from, to);
                    if (dateCom == 1) {
                        alert("End Date Cannot be Greater Than Start Date");
                        $("[id$=txtToDate]").focus();
                        return false;
                    }

                }
            }
            if (($("[id$=ddlReportType] option:selected").val() == "CategoryWiseOEEAndLossTimeReport")) {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("Please select fromdate");
                    $("[id$=txtFromDate]").focus();
                    return false;
                }
            }
            if (($("[id$=ddlReportType] option:selected").val() == "BatchWiseReport") || ($("[id$=ddlReportType] option:selected").val() == "LineMeterReport")) {
                if ($("[id$=txtYear]").val() == "") {
                    alert("Please select Year");
                    $("[id$=txtYear]").focus();
                    return false;
                }
                if ($("[id$=txtMonth]").val() == "") {
                    alert("Please select Month");
                    $("[id$=txtMonth]").focus();
                    return false;
                }
            }
        });

        function messageOk() {
            Command: toastr["success"]("Report Generated Succesfully.")
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
            Command: toastr["error"]("Unknown Error. Please Try Again!")
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
            Command: toastr["error"]("No Data to export. Try Again!")
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
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());

            $('[id$=txttoyear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                orientation: "top",
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                orientation: "top",
                todayHighlight: true,
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $(".date").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txttomonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $(".date").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('[id$=txttimeconsolidate_fromdate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txttimeconsolidate_todate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $("[id$=txttimeconsolidate_fromdate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_todate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txttimeconsolidate_todate]").on("dp.change", function (e) {
                $('[id$=txttimeconsolidate_fromdate]').data("DateTimePicker").maxDate(e.date);
            });

            $(".loadData").change(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });


        });
    </script>
</asp:Content>
<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
