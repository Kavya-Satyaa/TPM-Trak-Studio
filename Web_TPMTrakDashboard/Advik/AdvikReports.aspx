<%@ Page Title="Advik Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdvikReports.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.AdvikReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="../MyCssAndJS/css/bootstrap-datepicker.css" rel="stylesheet" />
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="../MyCssAndJS/DatePicker/bootstrap-datetimepicker.css" rel="stylesheet" />
    <script src="../MyCssAndJS/DatePicker/bootstrap-datetimepicker.js"></script>
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
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
        <div class="col-md-9">
            <h1 class="text-center login-title commontd">Report</h1>
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
                                            <b>Report Type</b> </td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" >
                                                <asp:ListItem Value="JHChecklistReport" >JH Checklist Report</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                  
                                    <tr id="trFromDate" runat="server">
                                        <td runat="server">
                                            <b>From Date</b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15"  AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trToDate" runat="server">
                                        <td runat="server">
                                            <b>To Date</b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                   <tr id="trmonthlydate" runat="server">
                                        <td><b>From Date</b></td>
                                        <td>
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Year !" placeholder="Year" Style="width: 70px; display: inline;"></asp:TextBox>
                                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control commandateTxt" data-toggle="tooltip" title="Month !" placeholder="Month"  Style="width: 70px; display: inline;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trPlant" runat="server">
                                        <td runat="server">
                                            <b>Plant ID</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trCellId" runat="server">
                                        <td runat="server">
                                            <b>Cell ID</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlCellID" runat="server" CssClass="select form-control loadData cssclass" AutoPostBack="True" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr id="trMachine" runat="server">
                                        <td runat="server">
                                            <b>Machine ID</b> </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="  form-control cssclass" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                    <tr id="trShift" runat="server">
                                        <td runat="server">
                                            <b>Shift</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="select form-control" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                 
                                  
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click"/>
                                            <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:CommanResource, btnCancel %>" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
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
   
    <script type="text/javascript">
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        $(document).ready(function () {
            $.unblockUI({});

            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: 'en-US',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: 'en-US',
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: 'en-US',
             });
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());

            $(".loadData").change(function () {
                $.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif"/>' });
            });
            setInterval(function () {
                Showhide();
            }, 1000);
        });

        function Showhide() {

          <%-- var value = '<%= Session["ReportGenerated"]%>' ;
            if(value =="Ended")
            {
                HideLoader();
            }
            else if(value =="Started")
            {
                ShowLoader();
            }--%>

            //var value ="";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/AggregatedReports.aspx/GetVal",
                data: '{}',
                dataType: "json",
                success: function (result) {

                    OnSuccess(result);
                },
                error: function (Result) {

                }
            });
        }
        function OnSuccess(result) {

            if (result.d == "Ended") {
                HideLoader();
            }
            else if (result.d == "Started") {
                ShowLoader();
            }
        }

        function ShowLoader() {
            $.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif"  />' });
        }

        function HideLoader() {

            $.unblockUI({});
        }

        function invert(date) {
            return date.split(/[/-]/).reverse().join("")
        }

        function compareDates(date1, date2) {
            return invert(date1).localeCompare(invert(date2));
        }

        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }

        function messageNotOk() {
            Command: toastr["error"]("Try Again!")
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
        function messageNodata() {
            Command: toastr["error"]("No Data Found!")
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

            $('[id$=txtYear]').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: 'en-US',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                orientation: "top",
                autoclose: true,
                language: 'en-US',
            });
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: 'en-US',
            });
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());

            $(".loadData").change(function () {
                $.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif"/>' });
            });
            setInterval(function () {
                Showhide();
            }, 1000);


        });

        $(document).on("click", "[id$=btnGenerate]", function () {

            if ($("[id$=ddlReportType]").val() == "JHChecklistReport") {
                if ($("[id$=txtYear]").val() == "") {
                    alert("Please Select Year");
                    $("[id$=txtYear]").focus();
                    return false;

                };
                if ($("[id$=txtMonth]").val() == "") {
                    alert("Please Select Month");
                    $("[id$=txtMonth]").focus();
                    return false;

                };
            }
            });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
