<%@ Page Title="Process Parameter Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterDetails.aspx.cs" Inherits="Web_TPMTrakDashboard.Cumi.ProcessParameterDetails" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script src="Scripts/jquery-3.3.1.js"></script>
    <%--  <link href="Content/Ionic.css" rel="stylesheet" />--%>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>

    <!-- Font Awesome -->
    <%--<link
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css"
        rel="stylesheet" />--%>
    <!-- Google Fonts -->
    <%--  <link
        href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
        rel="stylesheet" />--%>
    <!-- MDB -->
    <%-- <link
        href="https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/6.0.1/mdb.min.css"
        rel="stylesheet" />

    <!-- MDB -->
    <script
        type="text/javascript"
        src="https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/6.0.1/mdb.min.js"></script>--%>

    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/highcharts-more.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>


    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.1.7/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.1.7/css/dx.light.css" />
    <script src="https://cdn3.devexpress.com/jslib/20.1.7/js/dx.all.js"></script>

    <link href="http://cdn.syncfusion.com/20.3.0.47/js/web/flat-azure/ej.web.all.min.css" rel="stylesheet" />
    <script src="http://cdn.syncfusion.com/20.3.0.47/js/web/ej.web.all.min.js"></script>

    <%-- <link href="../GEA/css/jquery.toast.min.css" rel="stylesheet" />
    <script src="../GEA/css/jquery.toast.min.js"></script>--%>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>


    <%-- <script src="Scripts/Highcharts/highcharts.js"></script>
    <script src="Scripts/Highcharts/exporting.js"></script>
    <script src="Scripts/Highcharts/export-data.js"></script>
    <script src="Scripts/Highcharts/accessibility.js"></script>--%>


    <%-- <link href="Scripts/DevExtreme/dx.common.css" rel="stylesheet" />
    <link href="Scripts/DevExtreme/dx.light.css" rel="stylesheet" />
    <script src="Scripts/DevExtreme/dx.all.js"></script>

    <link href="Scripts/SyncFusion/ej.web.all.min.css" rel="stylesheet" />
    <script src="Scripts/SyncFusion/ej.web.all.min.js"></script>--%>


    <style>
        /*   .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }*/
        .inner-chart-div {
            /*margin-bottom: 10px;*/
            min-width: 380px;
            max-width: 380px;
            /*height: 210px;*/
            display: inline-block;
            /*border-radius: 8px;*/
            background-color: white;
        }

        .inner-parameter-div {
            /*margin: 10px;*/
            min-width: 380px;
            max-width: 380px;
            /*height: 210px;*/
            display: inline-block;
            border-radius: 8px;
            background-color: #29b7a9;
            /*background-color:#2775ea;*/
        }

        .inner-div {
            margin: 10px;
            min-width: 380px;
            max-width: 380px;
            /*height: 210px;*/
            /*height: 323px;*/
            display: inline-block;
            /*border-radius: 8px;*/
            /*background-color: #29b7a9;*/
            /*background-color:#2775ea;*/
        }

        .inner-table {
            width: 100%;
            border: none;
            border-collapse: collapse;
            height: 100%;
        }

            .inner-table tr td {
                border: none;
                /*font-weight: bold;*/
                font-size: 20px;
                text-align: center;
            }

        .td-header {
            vertical-align: middle;
            height: 50px;
            border-radius: 5px 5px 0px 0px;
        }

        .lbl-header {
            font-size: 18px;
            display: inline-block;
            overflow: hidden;
            width: 370px;
            min-width: 370px;
            text-overflow: ellipsis;
            white-space: nowrap;
            /*font-weight: bold;*/
        }


        .lbl-parameter-value {
            background-color: white;
            width: 95px;
            max-width: 95px;
            padding: 5px 2px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .table-min-max {
            width: 100%;
        }

            .table-min-max tr td {
                padding: 5px;
            }

        .lbl-min-max {
            vertical-align: super;
        }

        .lbl-min-max-value {
            background-color: white;
            width: 95px;
            max-width: 95px;
            padding: 5px 2px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            vertical-align: sub;
        }

        .lbl-values {
            min-height: 38px;
        }

        fieldset {
            border: 1px groove #d5d5d5 !important;
            /*     padding: 0.1em 0.5em 1em !important;*/
            margin: 0 0 1.5em 1em !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 2px 2px 6px 3px #060606;
            /*font-weight: bold;*/
            height: 93px;
            padding: 0px 8px;
            width: fit-content;
            background-color: #222e70;
            /*border-radius: 2px;*/
        }

        legend {
            font-size: 1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
            margin-top: -4px;
            color: white;
            margin-bottom: 0px;
        }

        .cumi-tbl-details tr td {
            background-color: white;
            padding: 8px;
            border: 1px solid #dfdede;
        }

        .cumi-filter-tbl tr td {
            color: white;
            border: unset;
            padding: 0px 10px;
        }

        .div-other-details {
            height: 78vh;
            overflow: auto;
            border: 1px solid #192d9b;
            padding: 20px 10px;
            border-radius: 7px;
            /*background-color: #1d2862;*/
            /*background-color:#2775ea;*/
            background-color: #6d9de7;
            box-shadow: 2px 2px 6px 3px #060606;
        }

        .div-parameter-details {
            height: 78vh;
            overflow: auto;
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

        /*td span{
                font-size: x-large;
            }*/

        /*.checkbox {
            font-size: large;
            font-weight: bold;
            color: black;
        }

        .multiselect-selected-text {
            font-size: small;
        }

        .multiselect {
            background-color: white;
        }

        input {
            font-size: initial;
        }

        .datepicker-days {
            display: block;
            font-size: small;
        }
        select{
            font-size: initial;
        }*/
        .card {
            width: fit-content;
            display: inline-block;
            margin-left: 10px;
            margin-bottom: 10px;
        }

        .multiselect-selected-text {
            width: 100%;
            overflow: hidden;
            text-overflow: ellipsis;
        }
    </style>

    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend>Filters</legend>
                    <table class="cumi-filter-tbl">
                        <tr>
                            <td>
                                <label class="filter-lbl-name">Plant</label>
                                <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" onchange="BindMachineID();" Style="font-size: initial;"></asp:DropDownList>
                            </td>
                            <td>
                                <label class="filter-lbl-name">Machine</label><br />
                                <%--OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"    onchange="return BindMachineParameters();"     --%>
                                <asp:DropDownList ClientIDMode="Static" onchange="return RestrictChoices();" multiple="multiple" ID="ddlMachine" runat="server" CssClass="form-control multiselect-ui"></asp:DropDownList>
                            </td>
                            <td>
                                <label class="filter-lbl-name">Date</label><br />
                                <asp:TextBox runat="server" ID="txtDate" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>
                                <label class="filter-lbl-name">Parameter</label><br />
                                <asp:DropDownList multiple="multiple" ID="ddlParameters" runat="server" CssClass="form-control" ClientIDMode="Static">
                                    <%--<asp:ListItem Value="Top Hydraulic Pressure">Top Hydraulic Pressure</asp:ListItem>
                                    <asp:ListItem Value="Bottom Ram Hydraulic Pressure">Bottom Ram Hydraulic Pressure</asp:ListItem>
                                    <asp:ListItem Value="Top Ram Stroke">Top Ram Stroke</asp:ListItem>
                                    <asp:ListItem Value="Bottom Ram Stroke">Bottom Ram Stroke</asp:ListItem>
                                    <asp:ListItem Value="Hydraulic Oil Temperature">Hydraulic Oil Temperature</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <label class="filter-lbl-name">Shift</label><br />
                                <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control" Style="font-size: initial;"></asp:DropDownList>
                            </td>
                            <td>
                                <br />
                                <asp:Button runat="server" ID="btnView" CssClass="bajaj-btn-style" Text="View" OnClientClick="return BindDetails();" />
                            </td>
                            <td style="position: relative; top: 10px;">
                                <label>Auto Refresh</label>
                                <label class="switch">
                                    <asp:CheckBox runat="server" ID="cbAutorefresh" ClientIDMode="Static" />
                                    <span class="slider round"></span>
                                </label>
                            </td>
                            <%--<td>
                                <br />
                                <asp:RadioButtonList runat="server" ID="rblViewType" RepeatDirection="Horizontal" CssClass="radio-btn-list" OnSelectedIndexChanged="rblViewType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Parameter Details" Value="Parameters" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Other Details" Value="Others"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>--%>
                        </tr>
                    </table>
                </fieldset>
                <div id="scrollMaintainDiv" style="height: 80vh; overflow: auto; margin-top: 12px">
                    <div id="allDetails" runat="server">
                        <div class="col-lg-9 div-parameter-details">
                            <div class="card-group div-card-parameter-details">
                            </div>
                            <%--<asp:ListView runat="server" ID="lvParameterDetails">
                                <LayoutTemplate>
                                    <div>
                                        <div runat="server" id="itemplaceholder"></div>
                                    </div>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <div class="inner-div" style="background-color: <%# Eval("BackgroundColor")%>;">
                                        <table class="inner-table">
                                            <tr style="background-color: #e5e1e15e;">
                                                <td class="td-header">
                                                    <asp:Label runat="server" ID="lblParameter" CssClass="lbl-header"> <%# Eval("ParameterName")%></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="border: none; display: <%# Eval("OneValueVisibility")%>">
                                                <td class="td-value">
                                                    <label class="lbl-parameter-value lbl-values" title='<%# Eval("Value") %>'><%# Eval("Value") %></label>
                                                </td>
                                            </tr>
                                            <tr style="border: none; display: <%# Eval("TwoValueVisibility")%>">
                                                <td class="td-min-max">
                                                    <table class="table-min-max">
                                                        <tr>
                                                            <td>
                                                                <span class="lbl-min-max">Min</span>
                                                            </td>
                                                            <td>
                                                                <label class="lbl-min-max-value lbl-values"><%# Eval("MinValue") %></label>
                                                            </td>
                                                            <td>
                                                                <span class="lbl-min-max">Max</span>
                                                            </td>
                                                            <td>
                                                                <label class="lbl-min-max-value lbl-values"><%# Eval("MaxValue") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="border: none; display: <%# Eval("LowHighVisibility")%>">
                                                <td class="td-min-max">
                                                    <table class="table-min-max">
                                                        <tr>
                                                            <td>
                                                                <span class="lbl-min-max">Low</span>
                                                            </td>
                                                            <td>
                                                                <label class="lbl-min-max-value lbl-values"><%# Eval("LowerValue") %></label>
                                                            </td>
                                                            <td>
                                                                <span class="lbl-min-max">High</span>
                                                            </td>
                                                            <td>
                                                                <label class="lbl-min-max-value lbl-values"><%# Eval("HigherValue") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>--%>
                            <%--<div id="parameterDiv">
                            </div>--%>
                        </div>
                        <div class="col-lg-3 div-other-details">
                            <%-- <asp:ListView runat="server" ID="lvOtherDetails">
                                <LayoutTemplate>
                                    <table class="table table-bordered table-hover headerFixer cumi-tbl-details ">
                                        <tr>
                                            <th colspan="2" style="text-align: center">Details</th>
                                        </tr>
                                        <tr runat="server" id="itemplaceholder"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <span style="font-weight: bold"><%# Eval("ParameterName")%></span>
                                        </td>
                                        <td>
                                            <span><%# Eval("Value")%></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>--%>

                            <%--<table id="tblOtherDetails" class="table table-bordered table-hover headerFixer cumi-tbl-details ">
                                <tr><th colspan="2" style="text-align: center">Details</th></tr></table>--%>
                        </div>
                    </div>

                    <%-- <asp:ListView runat="server" ID="lvOtherDetails">
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer cumi-tbl-details " style="width: 50%">
                                <tr>
                                    <th>Parameters</th>
                                    <th>Value</th>
                                </tr>
                                <tr runat="server" id="itemplaceholder"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <span><%# Eval("ParameterName")%></span>
                                </td>
                                <td>
                                    <span><%# Eval("ParameterValue")%></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>--%>
                </div>
                <div class="modal fade" id="loadingModal" role="dialog" style="min-width: 300px; margin-top: 15%" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog  modal-dialog-centered" style="width: 500px">
                        <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                            <%--<div class="modal-header" style="background-color: #0c0922; padding: 8px">

                        <h4 class="modal-title" style="color: white;">Warning!</h4>
                    </div>--%>
                            <div class="modal-body" style="text-align: center; padding: 20px">
                                <span style="color: black; font-size: 25px;">Please wait.....</span>
                            </div>
                            <%--<div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                        <button type="button" data-dismiss="modal" Style="width: 80px;" class="modalBtns">OK</button>
                    </div>--%>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        $(document).ready(function () {
            ControlSetter();
            //BindMachineID();
            //BindMachineParameters();
            BindDetails();

            setControl();

            //$('.multiselect-ui').multiselect({
            //    onChange: function (option, checked) {
            //        // Get selected options.
            //        var selectedOptions = $('.multiselect-ui option:selected');

            //        if (selectedOptions.length >= 3) {
            //            // Disable all other checkboxes.
            //            var nonSelectedOptions = $('.multiselect-ui option').filter(function () {
            //                return !$(this).is(':selected');
            //            });

            //            nonSelectedOptions.each(function () {
            //                var input = $('input[value="' + $(this).val() + '"]');
            //                input.prop('disabled', true);
            //                input.parent('li').addClass('disabled');
            //            });
            //        }
            //        else {
            //            // Enable all checkboxes.
            //            $('.multiselect-ui option').each(function () {
            //                var input = $('input[value="' + $(this).val() + '"]');
            //                input.prop('disabled', false);
            //                input.parent('li').addClass('disabled');
            //            });
            //        }
            //    }
            //});
        });
        function RestrictChoices() {
            debugger;
            //const selectedLi = document.getElementById('ddlMachine').getElementsByClassName('active');
            //var value = $("[id$=ddlMachine]").val();
            //if (value.length > 3) {
            //    alert('max');
            //    return false;
            //}
        }
        function setControl() {
            $('[id$=ddlMachine]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px'
                //rateLimit: 3,
                //includeFilterClearBtn: !0,
                //onChange: function (option, checked) {
                //    var selectedOptions = $('.multiselect-ui option:selected');

                //    if (selectedOptions.length >= 3) {
                //        var nonSelectedOptions = $('.multiselect-ui option').filter(function () {
                //            return !$(this).is(':selected');
                //        });

                //        nonSelectedOptions.each(function () {
                //            var input = $('input[value="' + $(this).val() + '"]');
                //            input.prop('disabled', true);
                //            input.parent('li').addClass('disabled');
                //        });
                //    }
                //    else {
                //        $('.multiselect-ui option').each(function () {
                //            var input = $('input[value="' + $(this).val() + '"]');
                //            input.prop('disabled', false);
                //            input.parent('li').addClass('disabled');
                //        });
                //    }
                //}
            });
            $('[id$=ddlParameters]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px'
            });
            $('#dlparameters').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '250px'
            });
        }
        function ControlSetter() {
            $('[id$=txtDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
            //$('[id$=lbMachine]').multiselect({
            //    includeSelectAllOption: true
            //});
        }

        function BindMachineID() {
            debugger;
            $("[id$=ddlMachine]").empty();
            $("[id$=ddlMachine]").closest('div').find('.dropdown-menu').empty();
            //$("[id$=ddlMachine]").closest('div').find('button').closest('span').find('.multiselect-selected-text').empty();
            $("[id$=ddlMachine]").multiselect("destroy");
            $("[id$=ddlParameters]").multiselect("destroy");

            //$.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });

            //setTimeout(function () {
            $.ajax({
                async: false,
                type: "POST",
                url: "ProcessParameterDetails.aspx/BindMachine",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: "{plant:'" + $("[id$=ddlPlant]").val() + "'}",
                success: function (response) {
                    //$.unblockUI({});
                    let dataitem = response.d;
                    let apprStr = "";

                    //$("[id$=ddlParameters]").empty();
                    //for (let i = 0; i < dataitem.length; i++) {
                    //    apprStr += "<option>" + dataitem[i] + "</option>";
                    //}
                    //$("[id$=ddlParameters]").append(apprStr);

                    $("[id$=ddlMachine]").empty();
                    for (let i = 0; i < dataitem.length; i++) {
                        //if (i == 0) {
                        //    apprStr += "<option selected>" + dataitem[i] + "</option>";
                        //} else {
                        //    apprStr += "<option>" + dataitem[i] + "</option>";
                        //}
                        apprStr += "<option>" + dataitem[i] + "</option>";
                    }
                    $("[id$=ddlMachine]").append(apprStr);

                },
                error: function (Result) {
                    //$.unblockUI({});
                }
            });
            //}, 500);
            if ($("[id$=ddlMachine]").val() == "" || $("[id$=ddlMachine]").val() == null) {
                var selectTags = $("[id$=ddlMachine]");
                //console.log(selectTags);
                selectTags[0].selectedIndex = 0;
            }
            BindMachineParameters();
            $('[id$=ddlMachine]').multiselect({
                includeSelectAllOption: true,
                //rateLimit: 3,
                includeFilterClearBtn: !0,
                //onChange: function (option, checked) {
                //    var selectedOptions = $('.multiselect-ui option:selected');

                //    if (selectedOptions.length >= 3) {
                //        var nonSelectedOptions = $('.multiselect-ui option').filter(function () {
                //            return !$(this).is(':selected');
                //        });

                //        nonSelectedOptions.each(function () {
                //            var input = $('input[value="' + $(this).val() + '"]');
                //            input.prop('disabled', true);
                //            input.parent('li').addClass('disabled');
                //        });
                //    }
                //    else {
                //        $('.multiselect-ui option').each(function () {
                //            var input = $('input[value="' + $(this).val() + '"]');
                //            input.prop('disabled', false);
                //            input.parent('li').addClass('disabled');
                //        });
                //    }
                //}
            });
        }

        function BindMachineParameters() {
            debugger;

            $("[id$=ddlParameters]").empty();
            $('[id$=ddlParameters]').multiselect("destroy");

            //$.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });

            //setTimeout(function () {
            $.ajax({
                async: false,
                type: "POST",
                url: "ProcessParameterDetails.aspx/BindParameters",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: "{machines:'" + $("[id$=ddlMachine]").val() + "'}",
                success: function (response) {
                    //$.unblockUI({});
                    let dataitem = response.d;
                    let apprStr = "";
                    let apprStrLi = "";
                    $("[id$=ddlParameters]").empty();
                    //apprStr += "<option value='Top Ram Hydraulic Pressure'>Top Hydraulic Pressure</option>";
                    //apprStr += "<option value='Bottom Ram Hydraulic Pressure'>Bottom Ram Hydraulic Pressure</option>";
                    //apprStr += "<option value='Top Ram Stroke'>Top Ram Stroke</option>";
                    //apprStr += "<option value='Bottom Ram Stroke'>Bottom Ram Stroke</option>";
                    //apprStr += "<option value='Hydraulic Oil Temperature'>Hydraulic Oil Temperature</option>";

                    for (let i = 0; i < dataitem.length; i++) {
                        apprStr += "<option value='" + dataitem[i].ParameterID + "'>" + dataitem[i].DisplayText + "</option>";
                        //apprStrLi += "<li class='multiselect-item multiselect-all'><a tabindex='0'><label class='checkbox'><input type='checkbox' value='" + dataitem[i].ParameterID + "'> " + dataitem[i].DisplayText + "</label></a></li>"
                    }
                    //if (dataitem.length == 0) {
                    //    apprStr += "<option value=''></option>";
                    //}
                    $("[id$=ddlParameters]").append(apprStr);
                    //$("[id$=ddlParameters]").closest('div').find('ul').append(apprStrLi);
                    //$('[id$=ddlParameters]').multiselect({
                    //    includeSelectAllOption: true
                    //});
                },
                error: function (Result) {
                    //$.unblockUI({});
                }
            });
            //}, 500);
            $('[id$=ddlParameters]').multiselect({
                includeSelectAllOption: true
            });
        }
        $("#cbAutorefresh").change(function () {
            BindDetails();
        });
        let Interval;
        function BindDetails() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            if ($("#cbAutorefresh").prop('checked')) {
                Interval = setTimeout(BindMachineParameterDetails, 1000);
                $("[id$=txtDate]").attr('disabled', 'disabled');
                $("[id$=ddlShift]").attr('disabled', 'disabled');
            } else {
                debugger;

                setTimeout(function () {
                    $("[id$=txtDate]").removeAttr('disabled');
                    $("[id$=ddlShift]").removeAttr('disabled');
                    clearTimeout(Interval);
                    BindMachineParameterDetails();
                }, 500);

            }
            $.unblockUI({});
            return false;
        }

        function BindMachineParameterDetails() {
            if ($("#cbAutorefresh").prop('checked')) {
                // need set current shift

                $.ajax({
                    async: false,
                    type: "POST",
                    url: "ProcessParameterDetails.aspx/GetCurrentShift",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    success: function (response) {
                        let dataitem = response.d;
                        $("[id$=ddlShift]").val(dataitem);
                    },
                    error: function (Result) {
                    }
                });

            }
            var params = $("[id$=ddlParameters]").val();
            if (params == "") {
                var x = document.getElementById("ddlParameters");
                var arr = [];
                var txt = "";
                var i;
                for (i = 0; i < x.options.length; i++) {
                    arr.push(x.options[i].value);
                }
                txt = arr.join(',');
                params = txt;
            }
            debugger;
            //$.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            setTimeout(function () {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "ProcessParameterDetails.aspx/GetParameterDetails",
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: "{shift:'" + $("[id$=ddlShift] :selected").text() + "',shiftdate:'" + $("[id$=ddlShift]").val() + "',plant:'" + $("[id$=ddlPlant]").val() + "',machine:'" + $("[id$=ddlMachine]").val() + "',parameters:'" + params + "', date:'" + $("[id$=txtDate]").val() + "', autoRefresh:'" + $("#cbAutorefresh").prop('checked') + "'}",
                    success: function (response) {
                        //$.unblockUI({});
                        let dataitem = response.d;
                        $(".div-card-parameter-details").empty();
                        $(".div-other-details").empty();
                        for (var i = 0; i < dataitem.length; i++) {
                            debugger;
                            BindParameterDetails(dataitem[i].MachineId, dataitem[i].ParameterDetails);
                            BindOtherDetails(dataitem[i].MachineId, dataitem[i].OtherDetails);
                        }
                    },
                    error: function (Result) {
                        //$.unblockUI({});
                    }
                });
                if ($("#cbAutorefresh").prop('checked')) {
                    let interval = 60000;
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "ProcessParameterDetails.aspx/GetInterval",
                        contentType: "application/json; charset=utf-8",
                        crossDomain: true,
                        dataType: "json",
                        success: function (response) {
                            interval = response.d;
                        },
                        error: function (Result) {

                        }
                    });
                    clearTimeout(Interval);
                    Interval = setTimeout(BindMachineParameterDetails, interval);
                }
            }, 5);

        }


        function BindLinearGuageChart_SF(containerid, actualvalue, actualvalue1, actualvalue2, lowervalue, highervalue, templatetype) {
            let markerPointers = [];
            let details = {};
            let barPointers = [];
            let ranges = [];
            let fillColor = "blue";
            let minValue = 0;
            let maxValue = 100, minorInterval = 2, majorInterval = 10;


            //templatetype = "2 Value With High/Low";
            //actualvalue = 12;
            //actualvalue1 = 10;
            //actualvalue2 = 20;
            //lowervalue = 2;
            //highervalue = 10;

            let array;
            //if (templatetype == "1 Value") {
            //    if (!inRange(0, 100, parseFloat(actualvalue))) {
            //        let val = Math.round(actualvalue);
            //        minValue = val - 50;
            //        maxValue = val + 50;
            //    }

            //} else if (templatetype == "1 Value With High/Low") {
            //    array = [lowervalue, highervalue, actualvalue];
            //    //minValue = Math.round(Math.min.apply(Math, array));
            //    //maxValue = Math.round(Math.max.apply(Math, array));
            //    //if (minValue != 0 && maxValue != 0) {
            //    //    majorInterval = Math.round((maxValue - minValue) / 10);
            //    //    minorInterval = Math.round((minValue + majorInterval) / 5);
            //    //}
            //}
            //else if (templatetype == "2 Value") {
            //    array = [actualvalue1, actualvalue2];
            //    //minValue = Math.round(Math.min.apply(Math, array));
            //    //maxValue = Math.round(Math.max.apply(Math, array));
            //    //if (minValue != 0 && maxValue != 0) {
            //    //    majorInterval = Math.round((maxValue - minValue) / 10);
            //    //    minorInterval = Math.round((minValue + majorInterval) / 5);
            //    //}
            //}
            //else if (templatetype == "2 Value With High/Low") {
            //    array = [lowervalue, highervalue, actualvalue1, actualvalue2];
            //    //minValue = Math.round(Math.min.apply(Math, array));
            //    //maxValue = Math.round(Math.max.apply(Math, array));
            //    //if (minValue != 0 && maxValue != 0) {
            //    //    majorInterval = Math.round((maxValue - minValue) / 10);
            //    //    minorInterval = Math.round((minValue + majorInterval) / 5);
            //    //}
            //}

            //if (templatetype != "1 Value") {
            //    let minval = Math.round(Math.min.apply(Math, array));
            //    let maxval = Math.round(Math.max.apply(Math, array));
            //    if (minval != 0 && maxval != 0) {
            //        minValue = minval;
            //        maxValue = maxval;
            //        majorInterval = Math.round((maxValue - minValue) / 10);
            //        minorInterval = Math.round((minValue + majorInterval) / 5);
            //    }
            //}

            if (templatetype == "1 Value") {
                array = [actualvalue];
            } else if (templatetype == "1 Value With High/Low") {
                array = [lowervalue, highervalue, actualvalue];
            }
            else if (templatetype == "2 Value") {
                array = [actualvalue1, actualvalue2];
            }
            else if (templatetype == "2 Value With High/Low") {
                array = [lowervalue, highervalue, actualvalue1, actualvalue2];
            }

            let maxval = Math.round(Math.max.apply(Math, array));
            if (inRange(0, 100, maxval)) {
                minValue = 0;
                maxValue = 100;
                majorInterval = 10;
                minorInterval = 2;
            } else if (inRange(100, 200, maxval)) {
                minValue = 0;
                maxValue = 200;
                majorInterval = 20;
                minorInterval = 5;
            } else if (inRange(200, 300, maxval)) {
                minValue = 0;
                maxValue = 300;
                majorInterval = 30;
                minorInterval = 10;
            } else if (inRange(300, 400, maxval)) {
                minValue = 0;
                maxValue = 400;
                majorInterval = 40;
                minorInterval = 10;
            } else if (inRange(400, 500, maxval)) {
                minValue = 0;
                maxValue = 500;
                majorInterval = 50;
                minorInterval = 10;
            }

            let width = 20;
            if (templatetype == "1 Value With High/Low" || templatetype == "2 Value With High/Low") {
                fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue);

                details = {
                    endValue: maxValue,  // For setting range end value
                    startValue: highervalue,  //For setting range start value
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    backgroundColor: "#d7565666", //red
                    border: { color: "#d7565666" },
                    distanceFromScale: -5
                }
                ranges.push(details);

                details = {
                    endValue: highervalue,
                    startValue: lowervalue,
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    distanceFromScale: -5,
                    backgroundColor: '#32e60c66', // green
                    border: { color: "#32e60c66" },
                };
                ranges.push(details);

                details = {
                    endValue: lowervalue,  // For setting range end value
                    startValue: minValue,  //For setting range start value
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    backgroundColor: "#d7565666", //red
                    border: { color: "#d7565666" },
                    distanceFromScale: -5
                }
                ranges.push(details);

            } else {
                details = {
                    endValue: maxValue,  // For setting range end value
                    startValue: minValue,  //For setting range start value
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    backgroundColor: "transparent", //gray
                    border: { color: "transparent" },
                    distanceFromScale: -5
                }
                ranges.push(details);
            }


            if (templatetype == "1 Value With High/Low" || templatetype == "1 Value") {

                if (templatetype == "1 Value With High/Low") {
                    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue);
                }

                details = {
                    width: 15,
                    backgroundColor: fillColor,
                    value: parseFloat(actualvalue)
                };
                barPointers.push(details);

                details = {
                    width: 15,
                    length: 15,
                    backgroundColor: fillColor,
                    value: parseFloat(actualvalue),
                };
                markerPointers.push(details);
            }

            if (templatetype == "2 Value With High/Low" || templatetype == "2 Value") {

                let array = [actualvalue1, actualvalue2];
                let maxActualValue = Math.round(Math.max.apply(Math, array));

                if (templatetype == "2 Value With High/Low") {
                    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, maxActualValue);
                }

                details = {
                    width: 15,
                    backgroundColor: fillColor,
                    value: parseFloat(maxActualValue)
                };
                barPointers.push(details);

                //details = {
                //    width: 13,
                //    backgroundColor: "blue",
                //    value: actualvalue2,
                //    distanceFromScale: -23,
                //    opacity: 0.4
                //};
                //barPointers.push(details);

                details = {
                    width: 15,
                    length: 15,
                    backgroundColor: fillColor,
                    value: parseFloat(maxActualValue),
                };
                markerPointers.push(details);

                //if (templatetype == "2 Value With High/Low") {
                //    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue1);
                //}

                //details = {
                //    width: 15,
                //    backgroundColor: fillColor,
                //    value: parseFloat(actualvalue1)
                //};
                //barPointers.push(details);

                ////details = {
                ////    width: 13,
                ////    backgroundColor: "blue",
                ////    value: actualvalue2,
                ////    distanceFromScale: -23,
                ////    opacity: 0.4
                ////};
                ////barPointers.push(details);

                //details = {
                //    width: 15,
                //    length: 15,
                //    backgroundColor: fillColor,
                //    value: parseFloat(actualvalue1),
                //};
                //markerPointers.push(details);


                //if (templatetype == "2 Value With High/Low") {
                //    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue2);
                //}

                //details = {
                //    width: 15,
                //    backgroundColor: fillColor,
                //    value: parseFloat(actualvalue2)
                //};
                //barPointers.push(details);

                //details = {
                //    width: 15,
                //    length: 15,
                //    backgroundColor: fillColor,
                //    value: parseFloat(actualvalue2),
                //};
                //markerPointers.push(details);
            }

            //if (templatetype == "1 Value With High/Low" || templatetype == "2 Value With High/Low") {
            //    details = {
            //        width: 15,
            //        length: 15,
            //        backgroundColor: "#55BF3B",
            //        value: parseFloat(lowervalue),
            //    };
            //    markerPointers.push(details);

            //    details = {
            //        width: 15,
            //        length: 15,
            //        backgroundColor: "#55BF3B",
            //        value: parseFloat(highervalue),
            //    };
            //    markerPointers.push(details);
            //}

            $("#" + containerid).ejLinearGauge({
                enableAnimation: false,
                //Adding frame object
                //height: 210,

                width: 200,
                // For setting linear gauge height
                //height: 250,
                height: 345,
                // For setting linear gauge minimum value
                // minimum: -110,
                // For setting linear gauge maximum value
                // maximum: 200,
                //width: 280,
                frame: {
                    innerWidth: 8,
                    outerWidth: 10,
                    backgroundImageUrl: "Images/labels_img1.png"
                },
                // value: barPointValue,
                //Adding scale collection

                scales: [{
                    //length: 210,
                    width: 15,
                    minimum: minValue,
                    maximum: maxValue,
                    backgroundColor: "transparent", type: "roundedrectangle",
                    border: { color: "gray", width: 1 },
                    showBarPointers: true,
                    //Adding label collection
                    labels: [{
                        distanceFromScale: { x: -30, y: 0 }, font: {
                            size: "15px",
                            fontStyle: 'Normal',
                            textColor: 'black'
                        }
                    }],
                    //Adding marker pointer collection
                    // markerPointers: [{ width: 10, length: 10, value: 60 }, { width: 10, length: 10, value: 80, backgroundColor: "red" }],
                    markerPointers: markerPointers,
                    //Adding bar pointer collection
                    //barPointers: [{ width: 5, backgroundColor: "#95C7E0", value: 50 },
                    //{
                    //    width: 6, backgroundColor: "#EDC1D7",
                    //    distanceFromScale: -15, value: 30, opacity: 0.7
                    //}],
                    barPointers: barPointers,
                    //Adding tick collection

                    minorIntervalValue: minorInterval,
                    majorIntervalValue: majorInterval,
                    ticks: [{
                        type: "majorinterval", width: 2, height: 15,
                        color: "black", distanceFromScale: { x: -5, y: 0 }, position: "far"
                    },
                    {
                        type: "minorinterval", width: 1,
                        color: "gray", distanceFromScale: { x: -5, y: 0 }
                    }],
                    //customLabels: [{
                    //    //position: { x: 55, y: 87 },
                    //    value: "Mathematics Mark",
                    //    color: "Red",
                    //   // textAngle: 30,
                    //   // opacity: 0.5
                    //}],
                    showRanges: true,
                    ranges: ranges
                }]
            });

            // $("#" + containerid).css({ "position": "relative", "top" : "19px"});
        }

        function BindTempCharts(containerid, actualvalue, actualvalue1, actualvalue2, lowervalue, highervalue, templatetype) {
            let series = [];
            let details = {};
            let barPointValue = 0;
            let barPointColor = "blue";
            let ranges = [];
            let fillColor = "blue";
            let minValue = 0;
            let maxValue = 100;

            //templatetype = "1 Value With High/Low";
            //actualvalue = 12;
            //actualvalue1 = 10;
            //actualvalue2 = 20;
            //lowervalue = 2;
            //highervalue = 10;

            let width = 20;
            if (templatetype == "1 Value With High/Low" || templatetype == "2 Value With High/Low") {
                fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue);


                details = {
                    endValue: maxValue,  // For setting range end value
                    startValue: highervalue,  //For setting range start value
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    backgroundColor: "#d7565666", //red
                    border: { color: "#d7565666" },
                    distanceFromScale: 0
                }
                ranges.push(details);

                details = {
                    endValue: highervalue,
                    startValue: lowervalue,
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    distanceFromScale: 0,
                    backgroundColor: '#32e60c66', // green
                    border: { color: "#32e60c66" },
                };
                ranges.push(details);

                details = {
                    endValue: lowervalue,  // For setting range end value
                    startValue: minValue,  //For setting range start value
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    backgroundColor: "#d7565666", //red
                    border: { color: "#d7565666" },
                    distanceFromScale: 0
                }
                ranges.push(details);

            } else {
                details = {
                    endValue: maxValue,  // For setting range end value
                    startValue: minValue,  //For setting range start value
                    startWidth: width,
                    endWidth: width,
                    placement: "near",
                    backgroundColor: "transparent", //gray
                    border: { color: "transparent" },
                    distanceFromScale: 0
                }
                ranges.push(details);
            }

            if (templatetype == "1 Value With High/Low" || templatetype == "1 Value") {

                if (templatetype == "1 Value With High/Low") {
                    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue);
                }

                details = {
                    //width: 10,
                    width: 20,
                    length: 20,
                    //backgroundColor: "Grey",
                    //distanceFromScale: -12,
                    backgroundColor: fillColor,
                    //distanceFromScale: 20,
                    placement: "far",
                    value: parseFloat(actualvalue),
                    dataLabel: {
                        //Set text alignment to data label text	
                        visible: true,
                        // horizontalTextAlignment: "center",
                        //verticalTextAlignment: "far"
                    }
                };
                series.push(details);
                barPointValue = actualvalue;
                barPointColor = fillColor;
            }

            if (templatetype == "2 Value With High/Low" || templatetype == "2 Value") {

                let array = [actualvalue1, actualvalue2];
                let maxActualValue = Math.round(Math.max.apply(Math, array));

                if (templatetype == "2 Value With High/Low") {
                    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, maxActualValue);
                }
                details = {
                    //width: 10,
                    width: 20,
                    length: 20,
                    //backgroundColor: "Grey",
                    //distanceFromScale: -12,
                    backgroundColor: fillColor,
                    //distanceFromScale: 20,
                    placement: "far",
                    value: parseFloat(maxActualValue),
                    dataLabel: {
                        //Set text alignment to data label text	
                        visible: true,
                        // horizontalTextAlignment: "center",
                        //verticalTextAlignment: "far"
                    }
                };
                series.push(details);
                barPointValue = maxActualValue;
                barPointColor = fillColor;

                //if (templatetype == "2 Value With High/Low") {
                //    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue1);
                //}
                //details = {
                //    //width: 10,
                //    width: 20,
                //    length: 20,
                //    //backgroundColor: "Grey",
                //    //distanceFromScale: -12,
                //    backgroundColor: fillColor,
                //    //distanceFromScale: 20,
                //    placement: "far",
                //    value: parseFloat(actualvalue1),
                //    dataLabel: {
                //        //Set text alignment to data label text	
                //        visible: true,
                //        // horizontalTextAlignment: "center",
                //        //verticalTextAlignment: "far"
                //    }
                //};
                //series.push(details);
                //barPointValue = actualvalue1;
                //barPointColor = fillColor;


                //if (templatetype == "2 Value With High/Low") {
                //    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue2);
                //}

                //details = {
                //    //width: 10,
                //    width: 20,
                //    length: 20,
                //    //backgroundColor: "Grey",
                //    //distanceFromScale: -12,
                //    backgroundColor: fillColor,
                //    //distanceFromScale: 20,
                //    placement: "far",
                //    value: parseFloat(actualvalue2),
                //    dataLabel: {
                //        //Set text alignment to data label text	
                //        visible: true,
                //        // horizontalTextAlignment: "center",
                //        //verticalTextAlignment: "far"
                //    }
                //};
                //series.push(details);

            }

            //if (templatetype == "1 Value With High/Low" || templatetype == "2 Value With High/Low") {
            //    details = {
            //        width: 15,
            //        length: 15,
            //        //backgroundColor: "Grey",
            //        //distanceFromScale: -12,
            //        backgroundColor: "#55BF3B",
            //        //distanceFromScale: 20,
            //        placement: "far",
            //        value: parseFloat(lowervalue),
            //    };
            //    series.push(details);

            //    details = {
            //        width: 15,
            //        length: 15,
            //        //backgroundColor: "Grey",
            //        //distanceFromScale: -12,
            //        backgroundColor: "#55BF3B",
            //        //distanceFromScale: 20,
            //        placement: "far",
            //        value: parseFloat(highervalue),
            //    };
            //    series.push(details);
            //}



            //$("#" + containerid).empty();
            //$('#' + containerid).removeAttr("style");
            //$('#' + containerid).removeClass("e-lineargauge e-js e-widget");
            // let endvalue = maxvalue > actualvalue ? (maxvalue + 50) : (actualvalue + 50);
            let minintervalvalue = 10, maxintervalvalue = 20;
            // let val = Math.ceil(endvalue / 100);
            // minintervalvalue = val * 10;
            // maxintervalvalue = minintervalvalue * 2;
            $("#" + containerid).ejLinearGauge({
                //height: 320,
                //width: 320,
                //height: 195,
                height: 340,
                //width: 280,
                labelColor: "black",
                //className: 'highchart-xyaxis-label',
                enableAnimation: false,
                //animationSpeed: 1000,
                //   printMode: true,
                //title: {
                //    text: "",
                //},
                //customLabels: [{
                //    value: displayValue, position: { x: 55, y: 97 }
                //}],
                scales: [{

                    type: "thermometer",
                    backgroundColor: "#e1e2e3",
                    minimum: 0,
                    maximum: 100,
                    labelColor: "red",
                    enableAnimation: false,
                    minorIntervalValue: 2,
                    // intervalValue: 20,
                    // ticksInterval: 20,
                    // tickInterval: 20,
                    majorIntervalValue: 10,
                    width: 23,
                    position: { x: 50, y: 18 },
                    length: 310,
                    border: { width: 2, color: '#206BA4' },
                    //markerPointers: [{ opacity: 0 }],

                    markerPointers: series,
                    //markerPointers: [{
                    //    //width: 10,
                    //    width: 20,
                    //    length: 20,
                    //    //backgroundColor: "Grey",
                    //    //distanceFromScale: -12,
                    //    backgroundColor: "blue",
                    //    //distanceFromScale: 20,
                    //    placement: "far",
                    //    value: 10,
                    //    dataLabel: {
                    //        //Set text alignment to data label text	
                    //        visible: true,
                    //        // horizontalTextAlignment: "center",
                    //        //verticalTextAlignment: "far"
                    //    }
                    //},
                    //{
                    //    width: 15,
                    //    length: 15,
                    //    //backgroundColor: "Grey",
                    //    //distanceFromScale: -12,
                    //    backgroundColor: "red",
                    //    //distanceFromScale: 20,
                    //    placement: "far",
                    //    value: 0,
                    //    labelColor: "Black",
                    //},
                    //{
                    //    width: 15,
                    //    length: 15,
                    //    //backgroundColor: "Grey",
                    //    //distanceFromScale: -12,
                    //    backgroundColor: "red",
                    //    //distanceFromScale: 20,
                    //    placement: "far",
                    //    value: 100,
                    //}
                    //],
                    barPointers: [{
                        width: 11,
                        // distanceFromScale: -0.5,
                        value: barPointValue,
                        backgroundColor: barPointColor,
                    }],
                    labels: [{
                        placement: "near",
                        textColor: 'black',
                        // className: 'highchart-xyaxis-label',
                        font: {
                            size: "15px",
                            fontStyle: 'Normal'
                        },
                        distanceFromScale: { x: -25 },
                        // color: "red"

                    }
                        //,
                        //{
                        // placement: "far",
                        // distanceFromScale: { x: 20 }
                        // }
                    ],
                    ticks: [{
                        type: "majorinterval", width: 2, height: 15,
                        color: "black",
                        //distanceFromScale: { x: -27, y: 0 },
                        placement: "near"
                    },
                    {
                        type: "minorinterval", width: 1,
                        color: "gray",
                        //distanceFromScale: { x: -27, y: 0 },
                        placement: "near"
                    }],
                    //drawLabels: function () {
                    //    var args = $("#"+containerid).data("ejLinearGauge");
                    //    if (args.label.index == 1) {
                    //        args.style.textValue = (args.label.value * (9 / 5)) + 32;
                    //        args.style.font = "Normal 10px Segoe UI";
                    //    }
                    //}
                    // customLabels: [{
                    //    //position: { x: 55, y: 87 },
                    //    value: "Mathematics Mark",
                    //    color: "Red",
                    //    // textAngle: 30,
                    //    // opacity: 0.5
                    //}],
                    showRanges: true,
                    ranges: ranges

                }],
                // drawLabels: "DrawLabel"
                //drawCustomLabel: "DrawCustomLabel",
                //drawLabels: "DrawLabel"

            });
        }

        function BindGuageChart(containerid, actualvalue, actualvalue1, actualvalue2, lowervalue, highervalue, templatetype) {

            let series = [];
            let details = {};
            let fillColor = "blue";
            let minValue = 0;
            let maxValue = 300;
            let meterBackColor = '#ededed';
            let minorInterval = 5, majorInterval = 20;


            //templatetype = "1 Value With High/Low";
            //actualvalue= 10;
            //actualvalue1 = 10;
            //actualvalue2 = 20;
            //lowervalue = 0;
            //highervalue = 20;


            if (templatetype == "1 Value") {
                array = [actualvalue];
            } else if (templatetype == "1 Value With High/Low") {
                array = [lowervalue, highervalue, actualvalue];
            }
            else if (templatetype == "2 Value") {
                array = [actualvalue1, actualvalue2];
            }
            else if (templatetype == "2 Value With High/Low") {
                array = [lowervalue, highervalue, actualvalue1, actualvalue2];
            }

            let maxval = Math.round(Math.max.apply(Math, array));
            if (inRange(0, 100, maxval)) {
                minValue = 0;
                maxValue = 100;
                majorInterval = 10;
                minorInterval = 2;
            } else if (inRange(100, 200, maxval)) {
                minValue = 0;
                maxValue = 200;
                majorInterval = 20;
                minorInterval = 5;
            } else if (inRange(200, 300, maxval)) {
                minValue = 0;
                maxValue = 300;
                majorInterval = 30;
                minorInterval = 10;
            } else if (inRange(300, 400, maxval)) {
                minValue = 0;
                maxValue = 400;
                majorInterval = 40;
                minorInterval = 10;
            } else if (inRange(400, 500, maxval)) {
                minValue = 0;
                maxValue = 500;
                majorInterval = 50;
                minorInterval = 10;
            }


            if (templatetype == "1 Value With High/Low" || templatetype == "1 Value") {
                if (templatetype == "1 Value With High/Low") {
                    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue);
                    meterBackColor = '#fcb8b8';
                }
                details = {
                    data: [parseFloat(actualvalue)],
                    yAxis: 0,
                    dial: {
                        radius: '80%',
                        backgroundColor: fillColor,
                        baseWidth: 12,
                        baseLength: '0%',
                        rearLength: '0%'
                    },
                    pivot: {
                        backgroundColor: fillColor,
                        radius: 6
                    },
                    dataLabels: {
                        enabled: false
                    }
                };
                series.push(details);
            }

            if (templatetype == "2 Value With High/Low" || templatetype == "2 Value") {

                let array = [actualvalue1, actualvalue2];
                let maxActualValue = Math.round(Math.max.apply(Math, array));

                if (templatetype == "2 Value With High/Low") {
                    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, maxActualValue);
                    meterBackColor = '#fcb8b8';
                }

                details = {
                    data: [parseFloat(maxActualValue)],
                    yAxis: 0,
                    dial: {
                        radius: '80%',
                        backgroundColor: fillColor,
                        baseWidth: 12,
                        baseLength: '0%',
                        rearLength: '0%'
                    },
                    pivot: {
                        backgroundColor: fillColor,
                        radius: 6
                    },
                    dataLabels: {
                        enabled: false
                    }
                };
                series.push(details);

                //if (templatetype == "2 Value With High/Low") {
                //    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue1);
                //    meterBackColor = '#fcb8b8';
                //}

                //details = {
                //    data: [parseFloat(actualvalue1)],
                //    yAxis: 0,
                //    dial: {
                //        radius: '80%',
                //        backgroundColor: fillColor,
                //        baseWidth: 12,
                //        baseLength: '0%',
                //        rearLength: '0%'
                //    },
                //    pivot: {
                //        backgroundColor: fillColor,
                //        radius: 6
                //    },
                //    dataLabels: {
                //        enabled: false
                //    }
                //};
                //series.push(details);

                //if (templatetype == "2 Value With High/Low") {
                //    fillColor = GetRedGreenBasedOnRange(lowervalue, highervalue, actualvalue2);
                //}
                //details = {
                //    data: [parseFloat(actualvalue2)],
                //    yAxis: 0,
                //    dial: {
                //        radius: '80%',
                //        backgroundColor: fillColor,
                //        baseWidth: 12,
                //        baseLength: '0%',
                //        rearLength: '0%'
                //    },
                //    pivot: {
                //        backgroundColor: fillColor,
                //        radius: 6
                //    },
                //    dataLabels: {
                //        enabled: false
                //    }
                //};
                //series.push(details);
            }

            //if (templatetype == "1 Value With High/Low" || templatetype == "2 Value With High/Low") {
            //    details = {
            //        data: [parseFloat(lowervalue)],
            //        yAxis: 0,
            //        dial: {
            //            backgroundColor: 'red'
            //        }
            //    };
            //    series.push(details);

            //    details = {
            //        data: [parseFloat(highervalue)],
            //        yAxis: 0,
            //        dial: {
            //            backgroundColor: 'red'
            //        }
            //    };
            //    series.push(details);
            //}


            Highcharts.chart(containerid, {

                chart: {
                    type: 'gauge',
                    plotBackgroundColor: null,
                    plotBackgroundImage: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    height: 300,
                },
                exporting: {
                    enabled: false
                },

                tooltip: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                title: {
                    text: ''
                },

                pane: {
                    startAngle: -90,
                    endAngle: 89.9,
                    background: null,
                    center: ['50%', '75%'],
                    size: '110%'
                },

                // the value axis
                yAxis: {
                    min: minValue,
                    max: maxValue,
                    //tickPixelInterval: 72,
                    //tickPosition: 'inside',
                    //tickColor: Highcharts.defaultOptions.chart.backgroundColor || '#FFFFFF',
                    //tickLength: 20,
                    //tickWidth: 2,
                    //minorTickInterval: null,
                    minorTickInterval: minorInterval,
                    minorTickPosition: 'inside',
                    //tickPixelInterval: 20,
                    tickInterval: majorInterval,
                    tickWidth: 2,
                    tickPosition: 'inside',
                    tickLength: 15,
                    tickColor: 'black',
                    minorTickColor: 'gray',
                    labels: {
                        distance: 20,
                        style: {
                            fontSize: '14px',

                        }
                    },
                    plotBands: [
                        {
                            from: minValue,
                            to: maxValue,
                            color: meterBackColor, // red
                            thickness: 20
                        },
                        {
                            from: parseFloat(lowervalue),
                            to: parseFloat(highervalue),
                            color: '#a3eb91', // green
                            thickness: 20
                        },
                        //{
                        //    from: 160,
                        //    to: 200,
                        //    color: '#DF5353', // red
                        //    thickness: 20
                        //    }
                    ]
                },
                series: series,
                //series: [{
                //    name: 'Speed',
                //    data: [80],
                //    tooltip: {
                //        valueSuffix: ' km/h'
                //    },
                //    dataLabels: {
                //        format: '{y} km/h',
                //        borderWidth: 0,
                //        color: (
                //            Highcharts.defaultOptions.title &&
                //            Highcharts.defaultOptions.title.style &&
                //            Highcharts.defaultOptions.title.style.color
                //        ) || '#333333',
                //        style: {
                //            fontSize: '16px'
                //        }
                //    },
                //    dial: {
                //        radius: '80%',
                //        backgroundColor: 'gray',
                //        baseWidth: 12,
                //        baseLength: '0%',
                //        rearLength: '0%'
                //    },
                //    pivot: {
                //        backgroundColor: 'gray',
                //        radius: 6
                //    }

                //}]

            });

        }

        function GetRedGreenBasedOnRange(low, hight, x) {
            let fillColor = "blue";
            if (inRange(parseFloat(low), parseFloat(hight), parseFloat(x))) {
                fillColor = "green";
            } else {
                fillColor = "red";
            }
            return fillColor;
        }

        function inRange(low, high, x) {
            return ((x - high) * (x - low) <= 0);
        }

        function BindParameterDetails(machine, dataitem) {
            debugger;

            var machineid = 'parameterDiv_' + machine.split(' ').join('_');
            //console.log(machineid);
            $("#label_" + machineid).remove();
            $("#" + machineid).remove();
            $(".div-card-parameter-details").append("<div class='card' id='" + machineid + "'></div>");
            $("#" + machineid).append("<div class='card-header text-center' style='background-color: #00537e;'><label class='text-center' id='label_" + machineid + "' style='color:white;font-size: x-large;' class='text-white'>" + machine + "</label></div>");

            $("#" + machineid).append("<div class='card-body' style='background-color: cornflowerblue;'></div>");
            for (let i = 0; i < dataitem.length; i++) {
                let appendStr = '';
                //let displayValue = "";
                let div2ValueVisibility = "none";
                let div1ValueVisibility = "none";

                if (dataitem[i].TemplateType == "1 Value With High/Low" || dataitem[i].TemplateType == "1 Value") {
                    // displayValue = dataitem[i].Value;
                    div1ValueVisibility = "block";
                }
                if (dataitem[i].TemplateType == "2 Value With High/Low" || dataitem[i].TemplateType == "2 Value") {
                    // displayValue = dataitem[i].MinValue + ", " + dataitem[i].MaxValue;
                    div2ValueVisibility = "block";
                }

                let chartDivId = "chartDiv_" + machine.split(' ').join('_') + i;
                let height = "0";

                //if (dataitem[i].ChartType != "") {
                //    height = "210";
                //}
                height = "350";

                appendStr += '<div class="inner-div" style="verticle-align: top">';

                //appendStr += ' <div  class="inner-chart-div " style="height:' + height + '"px> <div id="' + chartDivId + '"></div> </div>';

                //appendStr += "<br/>";
                appendStr +=
                    '<div class="inner-parameter-div" style="background-color: ' + dataitem[i].BackgroundColor + ';">' +
                    '<table class="inner-table" >' +
                    '<tr style="background-color: #e5e1e15e;">' +
                    '<td class="td-header">' +
                    '<span  class="lbl-header" style="color:' + dataitem[i].ForeColor + '"> ' + dataitem[i].ParameterName + ' (' + dataitem[i].Unit + ')' + '</span>' +
                    '</td>' +
                    '</tr>' +
                    //'<tr><td style="padding: 0px"> <div style="background-color: white">' + displayValue + '</div></td></tr>' +


                    '<tr style="border: none;">' +
                    '<td class="td-min-max">' +
                    '<div style="min-height: 48px; height: 48px"><div  style="display: ' + div2ValueVisibility + '"><table class="table-min-max">' +
                    '<tr>' +
                    '<td>' +
                    '<span class="lbl-min-max" style="color:' + dataitem[i].ForeColor + '; font-size: 16px">Low</span>' +
                    '</td>' +
                    '<td>' +
                    '<label class="lbl-min-max-value lbl-values">' + dataitem[i].MinValue + '</label>' +
                    '</td>' +
                    '<td>' +
                    '<span class="lbl-min-max" style="color:' + dataitem[i].ForeColor + '; font-size: 16px">High</span>' +
                    '</td>' +
                    '<td>' +
                    '<label class="lbl-min-max-value lbl-values">' + dataitem[i].MaxValue + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '</table></div>' +
                    '<div style="display: ' + div1ValueVisibility + '"><table class="table-min-max" style="width: 51%; margin: auto">' +
                    '<tr>' +
                    '<td>' +
                    '<span class="lbl-min-max" style="color:' + dataitem[i].ForeColor + '; font-size: 16px">Value</span>' +
                    '</td>' +
                    '<td>' +
                    '<label class="lbl-min-max-value lbl-values">' + dataitem[i].Value + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '</table></div>' +
                    '</div > ' +
                    '</td>' +
                    '</tr>' +


                    '<tr><td style="padding: 0px"> <div  class="inner-chart-div " style="height:' + height + 'px"> <div id="' + chartDivId + '"></div><div></td></tr>' +

                    //'<tr style="border: none; display:' + dataitem[i].OneValueVisibility + '">' +
                    //'<td class="td-value">' +
                    //'<label class="lbl-parameter-value lbl-values" title="' + dataitem[i].Value + '">' + dataitem[i].Value + '</label>' +
                    //'</td>' +
                    //'</tr>' +

                    //'<tr style="border: none; display: ' + dataitem[i].TwoValueVisibility + '">' +
                    //'<td class="td-min-max">' +
                    //'<table class="table-min-max">' +
                    //'<tr>' +
                    //'<td>' +
                    //'<span class="lbl-min-max" style="color:' + dataitem[i].ForeColor + '">Min</span>' +
                    //'</td>' +
                    //'<td>' +
                    //'<label class="lbl-min-max-value lbl-values">' + dataitem[i].MinValue + '</label>' +
                    //'</td>' +
                    //'<td>' +
                    //'<span class="lbl-min-max" style="color:' + dataitem[i].ForeColor + '">Max</span>' +
                    //'</td>' +
                    //'<td>' +
                    //'<label class="lbl-min-max-value lbl-values">' + dataitem[i].MaxValue + '</label>' +
                    //'</td>' +
                    //'</tr>' +
                    //'</table>' +
                    //'</td>' +
                    //'</tr>' +

                    '<tr style="border: none;">' +
                    '<td class="td-min-max">' +
                    '<div style="min-height: 48px; height: 48px"><div style="display: ' + dataitem[i].LowHighVisibility + '"><table class="table-min-max">' +
                    '<tr>' +
                    '<td>' +
                    '<span class="lbl-min-max" style="color:' + dataitem[i].ForeColor + '; font-size: 16px">Set Low</span>' +
                    '</td>' +
                    '<td>' +
                    '<label class="lbl-min-max-value lbl-values">' + dataitem[i].LowerValue + '</label>' +
                    '</td>' +
                    '<td>' +
                    '<span class="lbl-min-max" style="color:' + dataitem[i].ForeColor + '; font-size: 16px">Set High</span>' +
                    '</td>' +
                    '<td>' +
                    '<label class="lbl-min-max-value lbl-values">' + dataitem[i].HigherValue + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '</table></div></div>' +

                    '</td>' +
                    '</tr>' +
                    '</table>' +
                    '</div>';

                appendStr += "</div>";

                $("#" + machineid).find('.card-body').append(appendStr);

                //if (height != "0") {
                //    if (dataitem[i].ChartType == "Guage") {
                //        BindGuageChart(chartDivId, dataitem[i].Value, dataitem[i].LowerValue, dataitem[i].HigherValue);
                //    } else if (dataitem[i].ChartType == "LinearGuage") {
                //        BindLinearGuageChart(chartDivId, dataitem[i].Value, dataitem[i].LowerValue, dataitem[i].HigherValue);
                //    }
                //}

                //   GetLitreChart(chartDivId);
                // BindLinearGuageChart(chartDivId, dataitem[i].Value, dataitem[i].LowerValue, dataitem[i].HigherValue);
                // 
                //BindGuageChart_JQ(chartDivId);
                if (dataitem[i].ChartType == "LinearGuage") {
                    BindLinearGuageChart_SF(chartDivId, dataitem[i].Value, dataitem[i].MinValue, dataitem[i].MaxValue, dataitem[i].LowerValue, dataitem[i].HigherValue, dataitem[i].TemplateType);
                    // GetLitreChart(chartDivId);
                } else if (dataitem[i].ChartType == "Temperature") {
                    BindTempCharts(chartDivId, dataitem[i].Value, dataitem[i].MinValue, dataitem[i].MaxValue, dataitem[i].LowerValue, dataitem[i].HigherValue, dataitem[i].TemplateType);
                } else {
                    BindGuageChart(chartDivId, dataitem[i].Value, dataitem[i].MinValue, dataitem[i].MaxValue, dataitem[i].LowerValue, dataitem[i].HigherValue, dataitem[i].TemplateType);
                }
            }
            //$.unblockUI({});
        }

        function BindOtherDetails(machine, dataitem) {
            debugger;
            //$.unblockUI({});
            var machineid = 'tblOtherDetails_' + machine.split(' ').join('_');
            $("#" + machineid).remove();
            let appendStr = '';
            $(".div-other-details").append('<table id="' + machineid + '" class="table table-bordered table-hover headerFixer cumi-tbl-details " style="font-size: initial;"></table>');
            //  $("#tblOtherDetails").empty();
            appendStr += '<tr><th style="text-align: center;    padding: 9px;" colspan="2"> ' + machine + ' :- Details</th></tr>';
            for (let i = 0; i < dataitem.length; i++) {
                appendStr += '<tr><td>' + dataitem[i].ParameterName + '</td><td>' + (dataitem[i].Value == null ? "" : dataitem[i].Value) + '</td></tr>';
            }
            $("#" + machineid).append(appendStr);
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                ControlSetter();
                setControl();
                BindMachineParameters();

                $('.multiselect-ui').multiselect({
                    onChange: function (option, checked) {
                        // Get selected options.
                        var selectedOptions = $('.multiselect-ui option:selected');

                        if (selectedOptions.length >= 3) {
                            // Disable all other checkboxes.
                            var nonSelectedOptions = $('.multiselect-ui option').filter(function () {
                                return !$(this).is(':selected');
                            });

                            nonSelectedOptions.each(function () {
                                var input = $('input[value="' + $(this).val() + '"]');
                                input.prop('disabled', true);
                                input.parent('li').addClass('disabled');
                            });
                        }
                        else {
                            // Enable all checkboxes.
                            $('.multiselect-ui option').each(function () {
                                var input = $('input[value="' + $(this).val() + '"]');
                                input.prop('disabled', false);
                                input.parent('li').addClass('disabled');
                            });
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
