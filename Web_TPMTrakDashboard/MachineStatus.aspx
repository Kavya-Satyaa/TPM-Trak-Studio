<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineStatus.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineStatus" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/Ionic.css" rel="stylesheet" />
    <%--    <link href="MyCssAndJS/tosterJs/toastr.min.css" rel="stylesheet" />
    <script src="MyCssAndJS/tosterJs/toastr.min.js"></script>--%>

    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>

    <style>
        .thead-inverse th {
            color: #fff;
            background-color: #2E6886;
        }

        #tblMachineStatus tr:nth-child(odd) {
            background-color: WhiteSmoke;
        }

        #tblMachineStatus tr:nth-child(even) {
            background-color: #DCDCDC;
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
    </style>
    <div class="row">
        <div class="col-lg-1">
        </div>


        <div class="col-lg-10">
            <table class="table table-bordered">
                <tr>
                    <td style="font-weight: bold; vertical-align: middle" class="commontd"><%=GetGlobalResourceObject("CommanResource","PlantID")%></td>
                    <td>
                        <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip">
                            <%--title="<%=GetLocalResourceObject(Plant") %>!"--%>
                        </asp:DropDownList></td>
                    <td style="font-weight: bold; vertical-align: middle" class="commontd"><%=GetGlobalResourceObject("CommanResource","MachineId")%></td>
                    <td>
                        <select id="ddlMachine" class="form-control" data-toggle="tooltip" title="<%=GetGlobalResourceObject("CommanResource","MachineId")%> !">
                        </select></td>
                    <td>
                        <input id="btnSearch" type="button" name="Search" class="btn btn-info" value="<%=GetGlobalResourceObject("CommanResource","Search")%>" /></td>
                </tr>
            </table>

            <table id="tblMachineStatus" class="table table-bordered table-hover">
                <thead class="thead-inverse">
                    <tr>
                        <th style="min-width: 180px;"><%=GetLocalResourceObject("Status") %></th>
                        <th style="text-wrap: normal"><%=GetGlobalResourceObject("CommanResource","MachineId")%></th>
                        <th style="max-width: 220px;"><%=GetLocalResourceObject("In Consistence Components") %></th>
                        <th style="max-width: 250px;"><%=GetLocalResourceObject("Last Data Arrival") %></th>
                        <th style="max-width: 250px;"><%=GetLocalResourceObject("Device Connection Status") %></th>
                        <th style="max-width: 250px;"><%=GetLocalResourceObject("Device Ping Status") %></th>
                    </tr>
                </thead>
            </table>
        </div>

        <div class="col-lg-1">
        </div>
    </div>

    <script>

        $(document).ready(function () {

            var timesRun = 1;
            var timer;

            BindMachineInfo();
            $("[id$=ddlPlantId]").change(function () {
                BindMachineInfo();
            })

            $("#btnSearch").click(function () {
                timesRun = 1;
                clearInterval(timer);
                DashboardDetails();
            });

            function BindMachineInfo() {
                $.ajax({
                    type: "POST",
                    url: "MachineStatus.aspx/GetMachineInfo",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        BindMachine(itmData);
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                    }
                });
            }

            function BindMachine(data) {
                $("#ddlMachine option").remove();
                $("#ddlMachine").append('<option value="All"> <%=GetGlobalResourceObject("CommanResource","MachineAll")%> </option>');
                $(data).each(function (customers, el) {
                    $("#ddlMachine").append('<option value="'
                        + el + '">' + el + '</option>');
                });
            }
            var count = 0;
            DashboardDetails();
            var hideShowTimer;
            var datarefreshInterval = 1000 * 10 * 1;

            function DashboardDetails() {
                if (hideShowTimer != null) clearTimeout(hideShowTimer);
                $.ajax({
                    type: "POST",
                    url: "MachineStatus.aspx/BindMachineStatus",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantId:"' + $("[id$=ddlPlantId]").val() + '", machineId:"' + $("#ddlMachine").val() + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        $("#tblMachineStatus tr:gt(0)").each(function () {
                            $(this).remove();
                        });
                        if (itmData.length > 0) {
                            var tableContain = "";
                            //&nbsp;&nbsp;<img style="float: right;" src=' + md.statusImg + '></img>
                            $(itmData).each(function (index, md) {
                                if (md.MachineLiveStatus == "Running") {
                                    tableContain += ('<tr><td><div class="loaders-container1" style="float: left;" title=' + md.MachineLiveStatus + '><div class="la-cog la-2x" style="float: left;"><div class="Running"></div></div></div><div style="float: right;">' + md.MachineLiveStatus + '</div></td><td style="text-wrap:normal">' + md.MachineID + '&nbsp;&nbsp;<img style="float: right;" src=' + md.statusImg + '></img></td><td>' + md.LastCompOpn + '</td><td> ' + md.LastdataArrivalTime + '&nbsp;&nbsp;<img style="float: right;" src='
                                        + md.LastArrivalStatus + '></img></td><td>' + md.ConnectionTimestamp + '&nbsp;&nbsp;<img style="float: right;" src=' + md.ConnectionStatus + '></img></td><td>'
                                        + md.PingTimestamp + '&nbsp;&nbsp;<img style="float: right;" src=' + md.PingStatus + '></img></td></td></tr>');
                                }
                                else {
                                    tableContain += ('<tr><td><div class="loaders-container1" style="float: left;" title=' + md.MachineLiveStatus + '><div class="la-cog la-2x" style="float: left;"><div style="color:' + md.MachineStatusColor + ';"></div></div></div><div style="float: right;">' + md.MachineLiveStatus + '</div></td><td style="text-wrap:normal">' + md.MachineID + '&nbsp;&nbsp;<img style="float: right;" src=' + md.statusImg + '></img></td><td>' + md.LastCompOpn + '</td><td> ' + md.LastdataArrivalTime + '&nbsp;&nbsp;<img style="float: right;" src='
                                        + md.LastArrivalStatus + '></img></td><td>' + md.ConnectionTimestamp + '&nbsp;&nbsp;<img style="float: right;" src=' + md.ConnectionStatus + '></img></td><td>'
                                        + md.PingTimestamp + '&nbsp;&nbsp;<img style="float: right;" src=' + md.PingStatus + '></img></td></td></tr>');
                                }
                            });
                            $("#tblMachineStatus").append(tableContain);
                        }
                        else {
                            $('#tblMachineStatus').append('<tr><td colspan="9" style="text-align: center;"> <%=GetGlobalResourceObject("CommanResource","Nodataavailable")%> </td></tr>');
                        }
                        if (itmData.length > 0) {
                            if (itmData[0].MachineStatus == "NOT OK")
                                messageNotOk(itmData[0].MachineID, itmData[0].strPingStatus, itmData[0].PingTimestamp, itmData[0].strConStatus, itmData[0].ConnectionTimestamp);
                            //else if (itmData[0].MachineStatus == "OK")
                            //    messageOk(itmData[0].MachineID);
                            //else
                            //    messageOk(itmData[0].MachineID);

                            for (var i = 1; i < itmData.length; i++) {
                                var status = itmData[i].MachineStatus;
                                var doStuff = function () {
                                    if (itmData[timesRun].MachineStatus == "NOT OK")
                                        messageNotOk(itmData[timesRun].MachineID, itmData[timesRun].strPingStatus, itmData[timesRun].PingTimestamp, itmData[timesRun].strConStatus, itmData[timesRun].ConnectionTimestamp);
                                    //else if (itmData[timesRun].MachineStatus == "OK")
                                    //    messageOk(itmData[timesRun].MachineID);
                                    //else
                                    //    messageOk(itmData[timesRun].MachineID);

                                    if (++timesRun == itmData.length) {
                                        timesRun = 1;
                                        clearTimeout(hideShowTimer);
                                        clearInterval(timer);
                                        initTimer();
                                    };
                                };
                            }
                        }
                        timer = setInterval(doStuff, 4000);
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                    }
                });
            };

            function messageNotOk(machineId, pinStatus, pingTime, conStatus, connTime) {
                if (pinStatus == "NOT OK") {
                    Command: toastr["error"]("is not pinging since " + pingTime, machineId)
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
                } else if (conStatus == "NOT OK") {
                    Command: toastr["error"]("is not connect since " + connTime, machineId)
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
            }

            function messageOk(machineId) {
                Command: toastr["success"]("Status OK", machineId)
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

            // initTimer();

            function initTimer() {
                if (hideShowTimer != null) clearTimeout(hideShowTimer);
                hideShowTimer = setTimeout(function () {
                    DashboardDetails();
                    initTimer();
                }, datarefreshInterval);
            }

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
