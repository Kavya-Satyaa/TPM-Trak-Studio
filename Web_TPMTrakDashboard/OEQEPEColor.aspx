<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OEQEPEColor.aspx.cs" Inherits="Web_TPMTrakDashboard.OEQEPEColor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .UL {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            background-color: #333333;
        }

            .UL li {
                float: left;
            }

                .UL li a {
                    display: block;
                    color: white;
                    text-align: center;
                    padding: 14px;
                    text-decoration: none;
                    font-size: 16px;
                }

            .UL.active {
                background-color: #428bca;
            }
        /*.UL li a:hover {
                    background-color: #111111;
                }*/
        .subHeader {
            text-align: center;
            width: 100%;
            font-family: Verdana;
            font-size: 23px;
            font-weight: 600;
            color: white;
            background-color: #1A2732;
            height: 50px;
            line-height: 50px;
        }
    </style>
    <div>

        <div>
            <div>
                 <a class="glyphicon glyphicon-arrow-left" style="font-size: 25px; font-weight: bold" onclick="goToHomePage();"></a>&nbsp;
            </div>
            <label class="subHeader">Efficiency Color Code</label>
            <%--  <div style="text-align: center; width: 100%; font-family: Verdana; margin-top: 7px; float: left; font-size: 23px; font-weight: 600;color:white;background-color:#1A2732;height:50px;">
                Efficiency Color Code
            </div>--%>
            <ul class="UL">
                <li class="menuPlantData active"><a class="" data-toggle="tab" href="#menuplantColorCode">Plant-ID</a></li>
                <li class="menuCellData"><a data-toggle="tab" href="#menuCellColorCode">Cell-ID</a></li>
              <%--  <li class="menuComponentData"><a data-toggle="tab" href="#menuComponentColorCode">ComponentID</a></li>
                <li class="menuOperatorData"><a data-toggle="tab" href="#menuOperatorColorCode">Operator</a></li>--%>
            </ul>
        </div>
        <div>
            <%--<input type="text" value="PlantID" id="lblHeading" />--%>
            <label id="lblHeading" style="color: white; font-size: 16px; font-weight: bold; visibility: collapse;">PlantID</label>
        </div>
        <div>
            <div class="tab-content">
                <div id="menuColorCode" class="tab-pane fade in active">
                    <div>
                        <fieldset class="scheduler-border" style="display: inline-block; width: 49%;">
                            <legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource","AvailabilityEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtAeOk" style="width: 100%;"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtAeNotOk" style="width: 100%;"></td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="scheduler-border" style="display: inline-block; width: 49%;">
                            <legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource","ProductionEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtPeOk" style="width: 100%;"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtPeNotOk" style="width: 100%;"></td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div>
                        <fieldset class="scheduler-border" style="display: inline-block; width: 49%;">
                            <legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource","QualityEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtQeOk" style="width: 100%;"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtQeNotOk" style="width: 100%;"></td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="scheduler-border" style="display: inline-block; width: 49%;">
                            <legend class="scheduler-border commontd"><%=GetGlobalResourceObject("CommanResource","OverAllEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOaeOk" style="width: 100%;"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOaeNotOk" style="width: 100%;"></td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div>
                        <fieldset class="scheduler-border" style="display:inline-block;width:49%">
                            <legend class="scheduler-border commantd" style="color:white;">%Operator PE</legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOperatorPEOK" style="width: 100%;"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOperatorPENotOK" style="width: 100%;">
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div style="text-align: right;">
                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" Style="width: auto; min-width: 80px; margin-right: 15px;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            //$("a[href$='#menuplantColorCode']").css("background-color", "#428bca");
            //$("a[href$='#menuplantColorCode']").css("color", "#FFFFFF");
            $(".menuPlantData").css("background-color", "#428bca");
            $(".menuPlantData").click(function () {


                //$(".menuCellID").css("background-color", "#333333");
                //$(".menuCellID").css("color", "");
                //$(".menuComponentID").css("background-color", "#333333");
                //$(".menuComponentID").css("color", "");
                $(this).css("background-color", "#428bca");
                $(this).css("color", "#ffffff");
                $(this).siblings().css("background-color", "#333333");
                $('#lblHeading').text('PlantID');
                getdata();
            });

            $(".menuCellData").click(function () {

                //$(".menuPlantData").css("background-color", "#333333");
                //$(".menuPlantData").css("color", "");
                //$(".menuComponentID").css("background-color", "#333333");
                //$(".menuComponentID").css("color", "");
                $(this).css("background-color", "#428bca");
                $(this).css("color", "#FFFFFF");
                $(this).siblings().css("background-color", "#333333");
                $('#lblHeading').text('CellID');
                getdata();
            });

            $(".menuComponentData").click(function () {

                //$(".menuPlantData").css("background-color", "#333333");
                //$(".menuPlantData").css("color", "");
                //$(".menuCellID").css("background-color", "#333333");
                //$(".menuCellID").css("color", "");
                $(this).css("background-color", "#428bca");
                $(this).css("color", "#FFFFFF");
                $(this).siblings().css("background-color", "#333333");
                $('#lblHeading').text('ComponentID');
                getdata();
            });
            $(".menuOperatorData").click(function () {
                $(this).css("background-color", "#428bca");
                $(this).css("color", "#FFFFFF");
                $(this).siblings().css("background-color", "#333333");
                $('#lblHeading').text('Operator');
                getdata();
            })
            $('[id$=btnSave]').click(function () {
                savedata();
            });
            getdata();
        })
        function getdata() {
            $.ajax({
                type: "POST",
                url: "OEQEPEColor.aspx/GetData",
                contentType: "application/json; charset=utf-8",
                data: '{ViewType:"' + $('#lblHeading').text() + '"}',
                dataType: "json",
                success: loaddata,
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            })
        }

        function savedata() {
            $.ajax({
                type: "POST",
                url: "OEQEPEColor.aspx/SaveData",
                contentType: "application/json; charset=utf-8",
                data: '{PEGreen:"' + $('#txtPeOk').val() + '", PERed:"' + $('#txtPeNotOk').val() + '", AEGreen:"' + $('#txtAeOk').val() + '", AERed:"' + $('#txtAeNotOk').val() + '", OEGreen:"' + $('#txtOaeOk').val() + '", OERed:"' + $('#txtOaeNotOk').val() + '", QEGreen:"' + $('#txtQeOk').val() + '", QERed:"' + $('#txtQeNotOk').val() + '",ViewType:"' + $('#lblHeading').text() + '",OperatorPEGreen:"' + $('#txtOperatorPEOK').val() + '", OperatorPERed:"' + $('#txtOperatorPENotOK').val() + '"}',
                dataType: "json",
                success: datasaved,
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            })
        }
        function loaddata(result) {

            console.log(result);
            $('#txtPeOk').val(result.d.PEGreen);
            $('#txtPeNotOk').val(result.d.PERed);
            $('#txtAeOk').val(result.d.AEGreen);
            $('#txtAeNotOk').val(result.d.AERed);
            $('#txtOaeOk').val(result.d.OEGreen);
            $('#txtOaeNotOk').val(result.d.OERed);
            $('#txtQeOk').val(result.d.QEGreen);
            $('#txtQeNotOk').val(result.d.QERed);
            $('#txtOperatorPEOK').val(result.d.OperatorPEGreen);
            $('#txtOperatorPENotOK').val(result.d.OperatorPERed);
        }

        function datasaved() {

            Command: toastr["success"]("Saved Successfully")
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
            getdata();
        }

        function checkdetails() {
            Command: toastr["info"]("Please give all the values")
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
        function goToHomePage() {
            window.location.href = "ApplicationSettingPage.aspx";
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
