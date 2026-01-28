<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAccessRights.aspx.cs" Inherits="Web_TPMTrakDashboard.UserAccessRights" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #tblSmartShop tbody > tr > td, #tblCockpit tbody > tr > td, #tblSmartManger tbody > tr > td, #tblEshopX tbody > tr > td, #tblEshopXHelpRequest tbody > tr > td, #tblHistoricalAnalytics tbody > tr > td {
            padding: 0px;
            padding-left: 3px;
            margin-top: 3px;
            margin-bottom: 3px;
        }

            #tblSmartShop tbody > tr > td > label > .checkbox, #tblCockpit tbody > tr > td > label > .checkbox, #tblSmartManger tbody > tr > td > label > .checkbox, #tblEshopX tbody > tr > td > label > .checkbox, #tblEshopXHelpRequest tbody > tr > td > label > .checkbox, tblHistoricalAnalytics tbody > tr > td > label > .checkbox {
                margin-top: 5px;
                margin-bottom: 5px;
            }
    </style>


    <div class="row" style="text-align: center; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
    </div>
    <div class="row">
        <table class="table table-bordered">
            <tr>
                <td class="commontd" style="padding-top: 15px;"><b><%=GetGlobalResourceObject("CommanResource","PlantID") %></b></td>
                <td class="commontd">
                    <asp:DropDownList ID="ddlPlantID" runat="server" CssClass="form-control" Style="display: inline; width: 240px"></asp:DropDownList>
                    <label>
                        <span class="checkbox">
                            <asp:CheckBox ID="chkAllPlant" runat="server" type="checkbox" Style="display: inline;" /><b><%=GetGlobalResourceObject("CommanResource","ALL") %></b></span>
                    </label>
                </td>
                <td class="commontd" style="padding-top: 15px;"><b><%=GetGlobalResourceObject("CommanResource","UserID") %></b></td>
                <td class="commontd">
                    <asp:DropDownList ID="ddlUserId" runat="server" CssClass="form-control" Style="display: inline; width: 240px"></asp:DropDownList>
                    <label>
                        <span class="checkbox">
                            <asp:CheckBox ID="chkAdmin" runat="server" type="checkbox" Style="display: inline;" /><b><%=GetLocalResourceObject("Admin") %></b></span>
                    </label>
                </td>
                <td class="commontd" style="padding-top: 15px;"><b><%=GetGlobalResourceObject("CommanResource","Password") %></b></td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Style="width: 200px"></asp:TextBox></td>
                <td class="commontd">
                    <label>
                        <span class="checkbox">
                            <asp:CheckBox ID="chkSelectAll" runat="server" type="checkbox" Style="display: inline;" /><b><%=GetLocalResourceObject("SelectAll") %></b></span>
                    </label>
                </td>
                <td>
                    <button type="button" class="btn btn-info btn-sm displayCss plantDrop" id="btnAccept"><%=GetGlobalResourceObject("CommanResource","btnAccept") %></button>
                </td>
            </tr>
        </table>
    </div>

    <div class="row">
        <div class="col-lg-2" style="background-color: #0066CC; color: white; border-style: solid; border-width: 3px;">
            <label>
                <span class="checkbox">
                    <asp:CheckBox ID="chkHistoricalAnalytics" runat="server" type="checkbox" Style="display: inline;" /><b>Historical Analytics</b></span>
            </label>
            <table id="tblHistoricalAnalytics" class="table table-bordered" style="background-color: white">
            </table>
        </div>

        <div class="col-lg-2" style="background-color: #0066CC; color: white; border-style: solid; border-width: 3px;">
            <label>
                <span class="checkbox">
                    <asp:CheckBox ID="chkCockpit" runat="server" type="checkbox" Style="display: inline;" /><b><%=GetLocalResourceObject("Cockpit") %></b></span>
            </label>
            <table id="tblCockpit" class="table table-bordered" style="background-color: white">
            </table>
        </div>

        <div class="col-lg-2" style="background-color: #0066CC; color: white; border-style: solid; border-width: 3px;">
            <label>
                <span class="checkbox">
                    <asp:CheckBox ID="chkSmartShop" runat="server" type="checkbox" Style="display: inline;" /><b><%=GetLocalResourceObject("SmartShop") %></b></span>
            </label>
            <table id="tblSmartShop" class="table table-bordered" style="background-color: white">
            </table>
        </div>
        <div class="col-lg-2" style="background-color: #0066CC; color: white; border-style: solid; border-width: 3px;">
            <label>
                <span class="checkbox">
                    <asp:CheckBox ID="chkSmartManger" runat="server" type="checkbox" Style="display: inline;" /><b><%=GetLocalResourceObject("SmartManger") %></b></span>
            </label>
            <table id="tblSmartManger" class="table table-bordered" style="background-color: white">
            </table>
        </div>
        <div class="col-lg-2" style="background-color: #0066CC; color: white; border-style: solid; border-width: 3px;" runat="server" id="divEshopx">
            <label>
                <span class="checkbox">
                    <asp:CheckBox ID="chkEshopX" runat="server" type="checkbox" Style="display: inline;" /><b><%=GetLocalResourceObject("EshopX") %></b></span>
            </label>
            <table id="tblEshopX" class="table table-bordered" style="background-color: white">
            </table>
        </div>
        <div class="col-lg-2" style="background-color: #0066CC; color: white; border-style: solid; border-width: 3px;" runat="server" id="diveshopxHelpreq">
            <label>
                <span class="checkbox">
                    <asp:CheckBox ID="chkEshopXHelpRequest" runat="server" type="checkbox" Style="display: inline;" /><b>Eshopx HelpRequest/b></span>
            </label>
            <table id="tblEshopXHelpRequest" class="table table-bordered" style="background-color: white">
            </table>
        </div>
    </div>

    <script type="text/javascript">
        DashboardDetails();
        function DashboardDetails() {
            $.ajax({
                type: "POST",
                url: "UserAccessRights.aspx/BindUserAccessData",
                contentType: "application/json; charset=utf-8",
                data: '{userID:"' + $("[id$=ddlUserId]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;

                    //var lenth = itmData.keys(testvar).length;
                    $("#tblHistoricalAnalytics tr").each(function () {
                        $(this).remove();
                    });

                    $("#tblSmartShop tr").each(function () {
                        $(this).remove();
                    });
                    $("#tblCockpit tr").each(function () {
                        $(this).remove();
                    });
                    $("#tblEshopX tr").each(function () {
                        $(this).remove();
                    });
                    $("#tblEshopXHelpRequest tr").each(function () {
                        $(this).remove();
                    });
                    $("#tblSmartManger tr").each(function () {
                        $(this).remove();
                    });
                    if (itmData.length > 0) {
                        var tableContain = "";
                        var count = 0, count1 = 0;

                        $(itmData).each(function (index, md) {
                            if (md.Domain == "HA") {
                                tableContain += ('<tr><td><label><span class="checkbox"><input Domain="' + md.Domain + '" class="chkHistoricalAnalytics SelectAll" ' + md.Prochecked + ' type="checkbox" value=' + md.Code + '>' + md.DisplayText + '</span></label></td></tr>');
                                if (md.Prochecked == "checked")
                                    count++;
                                count1++;
                            }
                        });
                        $("#tblHistoricalAnalytics").append(tableContain);
                        if (count == count1)
                            $("[id$=chkHistoricalAnalytics]").prop('checked', true);

                        count = 0, count1 = 0;
                        tableContain = "";
                        $(itmData).each(function (index, md) {
                            if (md.Domain == "CP") {
                                tableContain += ('<tr><td><label><span class="checkbox"><input Domain="' + md.Domain + '" class="Cockpit SelectAll" ' + md.Prochecked + ' type="checkbox" value=' + md.Code + '>' + md.DisplayText + '</span></label></td></tr>');
                                if (md.Prochecked == "checked")
                                    count++;
                                count1++;
                            }
                        });
                        $("#tblCockpit").append(tableContain);
                        if (count == count1)
                            $("[id$=chkCockpit]").prop('checked', true);

                        count = 0, count1 = 0;
                        tableContain = "";
                        $(itmData).each(function (index, md) {
                            if (md.Domain == "SS") {
                                tableContain += ('<tr><td><label><span class="checkbox"><input Domain="' + md.Domain + '" class="SmartShop SelectAll" ' + md.Prochecked + ' type="checkbox" value=' + md.Code + '>' + md.DisplayText + '</span></label></td></tr>');
                                if (md.Prochecked == "checked")
                                    count++;
                                count1++;
                            }
                        });
                        $("#tblSmartShop").append(tableContain);
                        if (count == count1)
                            $("[id$=chkSmartShop]").prop('checked', true);

                        count = 0, count1 = 0;
                        tableContain = "";
                        $(itmData).each(function (index, md) {
                            if (md.Domain == "SM") {
                                tableContain += ('<tr><td><label><span class="checkbox"><input Domain="' + md.Domain + '" class="SmartManger SelectAll" ' + md.Prochecked + ' type="checkbox" value=' + md.Code + '>' + md.DisplayText + '</span></label></td></tr>');
                                if (md.Prochecked == "checked")
                                    count++;
                                count1++;
                            }
                        });
                        $("#tblSmartManger").append(tableContain);
                        //if (itmData.filter(x => x.Prochecked === 'checked' && x.Domain === "SM").length == itmData.filter(x => x.Domain === "SM").length)
                        if (count == count1)
                            $("[id$=chkSmartManger]").prop('checked', true);


                        count = 0, count1 = 0;
                        tableContain = "";
                        $(itmData).each(function (index, md) {
                            if (md.Domain == "eshopx") {
                                tableContain += ('<tr><td><label><span class="checkbox"><input Domain="' + md.Domain + '" class="chkEshopX SelectAll" ' + md.Prochecked + ' type="checkbox" value=' + md.Code + '>' + md.DisplayText + '</span></label></td></tr>');
                                if (md.Prochecked == "checked")
                                    count++;
                                count1++;
                            }
                        });
                        $("#tblEshopX").append(tableContain);
                        //if (itmData.filter(x => x.Prochecked === 'checked' && x.Domain === "SM").length == itmData.filter(x => x.Domain === "SM").length)
                        if (count == count1)
                            $("[id$=chkEshopX]").prop('checked', true);




                        count = 0, count1 = 0;
                        tableContain = "";
                        $(itmData).each(function (index, md) {
                            if (md.Domain == "eshopxHelpRequest") {
                                tableContain += ('<tr><td><label><span class="checkbox"><input Domain="' + md.Domain + '" class="chkEshopXHelpRequest SelectAll" ' + md.Prochecked + ' type="checkbox" value=' + md.Code + '>' + md.DisplayText + '</span></label></td></tr>');
                                if (md.Prochecked == "checked")
                                    count++;
                                count1++;
                            }
                        });
                        $("#tblEshopXHelpRequest").append(tableContain);
                        //if (itmData.filter(x => x.Prochecked === 'checked' && x.Domain === "SM").length == itmData.filter(x => x.Domain === "SM").length)
                        if (count == count1)
                            $("[id$=chkEshopXHelpRequest]").prop('checked', true);


                        if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                            $("[id$=chkSelectAll]").prop('checked', true);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        function GetPasswordDetails() {
            $.ajax({
                type: "POST",
                url: "UserAccessRights.aspx/GetPasswordInfo",
                contentType: "application/json; charset=utf-8",
                data: '{userID:"' + $("[id$=ddlUserId]").val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    if (itmData != null) {
                        $("[id$=txtPassword]").val(itmData.Password);
                        $("[id$=chkAdmin]").prop("checked", itmData.Admin);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        $("[id$=chkSmartShop]").change(function () {
            if ($(this).is(":checked")) {
                $(".SmartShop").prop('checked', true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkCockpit]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $(".SmartShop").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            };
        });



        $(document).on("click", ".SmartShop", function () {
            if ($(".SmartShop").length == $(".SmartShop:checked").length) {
                $("[id$=chkSmartShop]").prop("checked", true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $("[id$=chkSmartShop]").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            }
        });

        $("[id$=chkSelectAll]").change(function () {
            if ($(this).is(":checked")) {
                $(".SelectAll").prop('checked', true);
                $("[id$=chkSmartShop]").prop('checked', true);
                $("[id$=chkCockpit]").prop('checked', true);
                $("[id$=chkSmartManger]").prop('checked', true);
                $("[id$=chkEshopX]").prop('checked', true);
                $("[id$=chkEshopXHelpRequest]").prop('checked', true);
                $("[id$=chkHistoricalAnalytics]").prop('checked', true);
            } else {
                $(".SelectAll").prop('checked', false);
                $("[id$=chkSmartShop]").prop('checked', false);
                $("[id$=chkCockpit]").prop('checked', false);
                $("[id$=chkSmartManger]").prop('checked', false);
                $("[id$=chkEshopX]").prop('checked', false);
                $("[id$=chkEshopXHelpRequest]").prop('checked', false);
                $("[id$=chkHistoricalAnalytics]").prop('checked', true);
            };
        });

        $("[id$=chkCockpit]").change(function () {
            if ($(this).is(":checked")) {
                $(".Cockpit").prop('checked', true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $(".Cockpit").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            };
        });



        $(document).on("click", ".Cockpit", function () {
            if ($(".Cockpit").length == $(".Cockpit:checked").length) {
                $("[id$=chkCockpit]").prop("checked", true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $("[id$=chkCockpit]").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            }
        });

        $("[id$=chkSmartManger]").change(function () {
            if ($(this).is(":checked")) {
                $(".SmartManger").prop('checked', true);
                if ($("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $(".SmartManger").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            };
        });

        $(document).on("click", ".SmartManger", function () {
            if ($(".SmartManger").length == $(".SmartManger:checked").length) {
                $("[id$=chkSmartManger]").prop("checked", true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $("[id$=chkSmartManger]").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            }
        });

        $(document).on("click", ".chkEshopX", function () {
            if ($(".chkEshopX").length == $(".chkEshopX:checked").length) {
                $("[id$=chkEshopX]").prop("checked", true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $("[id$=chkEshopX]").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            }
        });

        $(document).on("click", ".chkEshopXHelpRequest", function () {
            if ($(".chkEshopXHelpRequest").length == $(".chkEshopXHelpRequest:checked").length) {
                $("[id$=chkEshopXHelpRequest]").prop("checked", true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $("[id$=chkEshopXHelpRequest]").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            }
        });

        $(document).on("click", ".chkHistoricalAnalytics", function () {
            if ($(".chkHistoricalAnalytics").length == $(".chkHistoricalAnalytics:checked").length) {
                $("[id$=chkHistoricalAnalytics]").prop("checked", true);
                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $("[id$=chkHistoricalAnalytics]").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            }
        });

        $("[id$=chkEshopX]").change(function () {
            if ($(this).is(":checked")) {
                $(".chkEshopX").prop('checked', true);
                if ($("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $(".chkEshopX").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            };
        });

        $("[id$=chkEshopXHelpRequest]").change(function () {
            if ($(this).is(":checked")) {
                $(".chkEshopXHelpRequest").prop('checked', true);
                if ($("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $(".chkEshopXHelpRequest").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            };
        });

        $("[id$=chkHistoricalAnalytics]").change(function () {
            if ($(this).is(":checked")) {
                $(".chkHistoricalAnalytics").prop('checked', true);
                if ($("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {
                $(".chkHistoricalAnalytics").prop('checked', false);
                $("[id$=chkSelectAll]").prop('checked', false);
            };
        });


        $(document).on("click", ".SmartManger", function () {
            if ($(".SmartManger").length == $(".SmartManger:checked").length) {

                if ($("[id$=chkSmartManger]").is(":checked") && $("[id$=chkSmartShop]").is(":checked") && $("[id$=chkCockpit]").is(":checked") && $("[id$=chkEshopX]").is(":checked") && $("[id$=chkEshopXHelpRequest]").is(":checked") && $("[id$=chkHistoricalAnalytics]").is(":checked"))
                    $("[id$=chkSelectAll]").prop('checked', true);
            } else {

                $("[id$=chkSelectAll]").prop('checked', false);
            }
        });



        $("[id$=ddlUserId]").change(function () {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            DashboardDetails();
            GetPasswordDetails();
            $.unblockUI({});
            return false;
        });

        $("#btnAccept").click(function () {
            if ($("[id$=txtPassword]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","Pleaseenterthepassword") %>");
                $("[id$=txtPassword]").focuse();
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            var values = {
                model:
                {
                    PlantID: $("[id$=ddlPlantID]").val(),
                    PlantAll: $("[id$=chkAllPlant]").is(":checked"),
                    UserID: $("[id$=ddlUserId]").val(),
                    Admin: $("[id$=chkAdmin]").is(":checked"),
                    Password: $("[id$=txtPassword]").val(),
                    SelectAll: $("[id$=chkSelectAll]").is(":checked"),
                    ListUserDataInfo: []
                }
            };
            values;
            $("#tblCockpit tr").each(function () {
                if ($(this).closest("tr").find(".Cockpit").is(":checked")) {
                    var chkModule = $(this).closest("tr").find(".Cockpit").val();
                    var Domain = $(this).closest("tr").find("input").attr('domain');
                    var innerModel =
                    {
                        Code: chkModule,
                        Domain: Domain
                    }
                    values.model.ListUserDataInfo.push(innerModel);
                }
            });
            $("#tblSmartShop tr").each(function () {
                if ($(this).closest("tr").find(".SmartShop").is(":checked")) {
                    var chkModule = $(this).closest("tr").find(".SmartShop").val();
                    var Domain = $(this).closest("tr").find("input").attr('domain');
                    var innerModel =
                    {
                        Code: chkModule,
                        Domain: Domain
                    }
                    values.model.ListUserDataInfo.push(innerModel);
                }
            });
            $("#tblSmartManger tr").each(function () {
                if ($(this).closest("tr").find(".SmartManger").is(":checked")) {
                    var chkModule = $(this).closest("tr").find(".SmartManger").val();
                    var Domain = $(this).closest("tr").find("input").attr('domain');
                    var innerModel =
                    {
                        Code: chkModule,
                        Domain: Domain
                    }
                    values.model.ListUserDataInfo.push(innerModel);
                }
            });
            $("#tblEshopX tr").each(function () {
                if ($(this).closest("tr").find(".chkEshopX").is(":checked")) {
                    var chkModule = $(this).closest("tr").find(".chkEshopX").val();
                    var Domain = $(this).closest("tr").find("input").attr('domain');
                    var innerModel =
                    {
                        Code: chkModule,
                        Domain: Domain
                    }
                    values.model.ListUserDataInfo.push(innerModel);
                }
            });
            $("#tblEshopXHelpRequest tr").each(function () {
                if ($(this).closest("tr").find(".chkEshopXHelpRequest").is(":checked")) {
                    var chkModule = $(this).closest("tr").find(".chkEshopXHelpRequest").val();
                    var Domain = $(this).closest("tr").find("input").attr('domain');
                    var innerModel =
                    {
                        Code: chkModule,
                        Domain: Domain
                    }
                    values.model.ListUserDataInfo.push(innerModel);
                }
            });

            $("#tblHistoricalAnalytics tr").each(function () {
                if ($(this).closest("tr").find(".chkHistoricalAnalytics").is(":checked")) {
                    var chkModule = $(this).closest("tr").find(".chkHistoricalAnalytics").val();
                    var Domain = $(this).closest("tr").find("input").attr('domain');
                    var innerModel =
                    {
                        Code: chkModule,
                        Domain: Domain
                    }
                    values.model.ListUserDataInfo.push(innerModel);
                }
            });
            values;
            $.ajax({
                type: "POST",
                url: "UserAccessRights.aspx/SaveUserAccessData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(values),
                dataType: "json",
                success: function (response) {
                    alert("<%=GetGlobalResourceObject("CommanResource","RecordUpdateSuccessfully") %>");
                    $.unblockUI({});
                    location.reload(true);
                },
                error: function (response) {
                    alert("Error");
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
