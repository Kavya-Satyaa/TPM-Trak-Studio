<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineInformation.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineInformation" Culture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Scripts.Render("~/bundles/editDropDownJs") %>
    <%: Styles.Render("~/bundles/editDropDownCss") %>

    <%--    <script src="MyCssAndJS/EditableDrop/jquery-editable-select.min.js"></script>
    <link href="MyCssAndJS/EditableDrop/jquery-editable-select.min.css" rel="stylesheet" />--%>

    <style>
        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        .dropdown {
            -webkit-appearance: none;
        }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }

        .form-control {
            color: black;
        }

        fieldset.scheduler-border {
            border: 2px groove #ddd !important;
            padding: 0.1em 0.5em 1em !important;
            margin: 0 0 1.5em 1em !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            font-weight: bold;
            height: 100px;
            /*width: 507px;*/
        }



        fieldset.scheduler-border1 {
            border: 2px groove #ddd !important;
            padding: 0.1em 0.5em 1em !important;
            margin: 0 0 1.5em 1em !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            font-weight: bold;
            height: 100px;
            width: 100%;
        }

        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
            color: white;
        }

        .button {
            background-color: #000000;
            font-weight: bold;
            color: white;
            border: 2px solid white;
            width: 40px;
            height: 40px;
            border-radius: 3px 3px;
        }

        .number {
            width: 130px;
        }

        legend {
            margin-bottom: 6px;
        }

        #tblMachineInfo tbody tr:nth-child(odd){
            background-color: #FFFFFF;
        }

        #tblMachineInfo tbody tr:nth-child(even){
            background-color: #DCDCDC;
        }

        #tblMachineInfo tbody tr:nth-child(odd) td{
            background-color: #FFFFFF;
        }

        #tblMachineInfo tbody tr:nth-child(even) td{
            background-color: #DCDCDC;
        }


        #tblMachineInfo tbody tr td:first-child{
            position: sticky;
            left:0px;
            z-index: 799;
        }

        
        #tblMachineInfo thead tr th:first-child{
            position: sticky;
            left:0px;
            z-index: 799;
        }
    </style>
    <div class="row text-center">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>
    <div class="row" style="margin-left: 2px;">
        <table class="table table-bordered" id="tblmenu" style="width: 50%">
            <tr>
                <td>
                    <div class="commontd">
                        <%=GetGlobalResourceObject("CommanResource","Machine") %>
                    </div>
                </td>
                <td>
                    <%-- <div class="col-md-8">
                        <div class="col-md-12">--%>
                    <input type="text" id="txtSearch" class="form-control" data-toggle="tooltip" title="search !" placeholder="search..." style="width: 220px; display: inline" />&nbsp;&nbsp;          
                      <%--  </div>--%>
                    <%-- <div class="col-md-1">
                                  <asp:Button runat="server" Text="Search" Style="float: right" class="btn btn-info" ID="Button1"></asp:Button>
                                </div>--%>
                    <%-- </div>--%>
                </td>

                <%-- <td>
                    <div>
                        <asp:Button runat="server" Text="Search" class="btn btn-info" ID="btnsearch"></asp:Button>
                    </div>
                </td>--%>
                <td>
                    <div style="float: left">
                        <input value="<%=GetGlobalResourceObject("CommanResource","Save") %>" type="button" class="btn btn-info" id="btnUpdate" style="display: inline; float: right; margin-right: 3px;" />&nbsp;&nbsp;
                         <input value="<%=GetGlobalResourceObject("CommanResource","Cancel") %>" type="button" class="btn btn-info" id="btnCancel" style="display: inline; float: right; display: none; margin-right: 3px;" />
                        <input value="<%=GetGlobalResourceObject("CommanResource","New") %>" type="button" class="btn btn-info" id="btnNew" style="display: inline; float: right; margin-right: 3px;" />
                        <input value="<%=GetGlobalResourceObject("CommanResource","Export") %>" type="button" class="btn btn-info" id="btnExport" style="display: none; float: left;" />
                    </div>
                </td>
            </tr>
        </table>
        <div id="griddata" style="min-height: 300px; border-bottom: 1px solid #D2D2D2; overflow: auto">
            <table id="tblMachineInfo" class="table table-bordered table-hover headerFixer">
                <thead class="blue">
                    <tr>
                        <th style="max-width: 150px;"><%=GetGlobalResourceObject("CommanResource","Machine") %>  
                        </th>
                        <th style="max-width: 80px;"><%=GetGlobalResourceObject("CommanResource","Description") %>  
                        </th>
                        <th style="max-width: 80px;"><%=GetGlobalResourceObject("CommanResource","InterfaceID") %>  
                        </th>
                        <th><%=GetGlobalResourceObject("CommanResource","Protocol") %>
                        </th>
                        <th style="max-width: 80px;"><%=GetGlobalResourceObject("CommanResource","IPAddress") %> 
                        </th>
                        <th style="max-width: 80px;"><%=GetGlobalResourceObject("CommanResource","DataPort") %> 
                        </th>
                        <th style="max-width: 100px;"><%=GetGlobalResourceObject("CommanResource","BulkTransferPort") %> 
                        </th>
                        <th>TPM-Trak enabled
                        </th>

                        <th>Smart Trans Enable
                        </th>
                        <th>Shared Device
                        </th>
                        <th>DNC V2 Enabled
                        </th>

                        <th style="max-width: 250px;">DNC IP  - <span style="margin-left: 5px;">DNC Port</span>
                        </th>

                        <th style="max-width: 80px;"><%=GetGlobalResourceObject("CommanResource","McHRRate") %>
                        </th>
                        <th>Critical Machine
                        </th>
                        <th>Enabled for Mobile
                        </th>
                        <th>OPC UA
                        </th>
                        <th runat="server" id="thPalletEnabled">Pallet Enabled?</th>
                        <th id="tdNoOfFixture" runat="server">No.Of Fixtures 1</th>
                        <th id="tdNoOfFixture2" runat="server">No.Of Fixtures 2</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>

    <fieldset class="scheduler-border" style="width: 100%; height: 430px; margin: 0 0 .2em 0em !important">
        <legend class="scheduler-border" id="machinelegend"><%=GetGlobalResourceObject("CommanResource","Machine") %></legend>
        <div class="row" id="tabs">
            <ul class="nav nav-pills">
                <li class="active" style="margin-left: 15px"><a class="menuData" data-toggle="tab" href="#menuColorCode"><%=GetGlobalResourceObject("CommanResource","EfficiencyColorCode") %></a></li>
                <li><a class="menuData" data-toggle="tab" href="#menuControlInfo"><%=GetGlobalResourceObject("CommanResource","MachineMake") %></a></li>
            </ul>
            <div class="tab-content">
                <div id="menuColorCode" class="tab-pane fade in active">
                    <div>
                        <fieldset class="scheduler-border" style="display: inline-block;">
                            <legend class="scheduler-border"><%=GetGlobalResourceObject("CommanResource","AvailabilityEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtAeOk"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtAeNotOk"></td>
                                </tr>
                            </table>
                        </fieldset>

                        <fieldset class="scheduler-border" style="display: inline-block">
                            <legend class="scheduler-border"><%=GetGlobalResourceObject("CommanResource","ProductionEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtPeOk"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtPeNotOk"></td>
                                </tr>
                            </table>
                        </fieldset>


                    </div>
                    <div>
                        <fieldset class="scheduler-border" style="display: inline-block">
                            <legend class="scheduler-border"><%=GetGlobalResourceObject("CommanResource","QualityEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtQeOk"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtQeNotOk"></td>
                                </tr>
                            </table>
                        </fieldset>

                        <fieldset class="scheduler-border" style="display: inline-block">
                            <legend class="scheduler-border"><%=GetGlobalResourceObject("CommanResource","OverAllEfficiency") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOaeOk"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOaeNotOk"></td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div>
                        <fieldset class="scheduler-border" style="display:inline-block;">
                            <legend class="scheduler-border" style="color:white;">%Operator PE</legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Green") %> >=</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOperatorPEOK"></td>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Red") %> <</td>
                                    <td>
                                        <input type="text" class="number" maxlength="3" id="txtOperatorPENotOK">
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>

                <div id="menuControlInfo" class="tab-pane fade in row">
                    <div class="col-lg-5 col-md-5">
                        <fieldset class="scheduler-border" style="height: 300px;">
                            <legend class="scheduler-border"><%=GetGlobalResourceObject("CommanResource","MachineMake") %></legend>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Manufacturer") %></td>
                                    <td>
                                        <asp:DropDownList ID="ddlManufacturer" runat="server" Width="204px" Height="27px" BackColor="White" meta:resourcekey="ddlManufacturerResource1"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","DateOfPurchase") %></td>
                                    <td>
                                        <input type="text" id="txtMfgDate"></td>
                                </tr>
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Address") %></td>
                                    <td>
                                        <input type="text" id="txtAddress">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Place") %></td>
                                    <td>
                                        <input type="text" id="txtPlace">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","Fax") %>/
                                    </td>
                                    <td>
                                        <input type="text" id="txtPhone">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="commontd"><%=GetGlobalResourceObject("CommanResource","ContactPerson") %>
                                    </td>
                                    <td>
                                        <input type="text" id="txtPerson">
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>

                    <div class="col-lg-5 col-md-5" style="display: none">
                        <fieldset class="scheduler-border" style="height: 435px;">
                            <legend class="scheduler-border"><%=GetGlobalResourceObject("CommanResource","McHRRate") %>Control Information</legend>
                            <div style="width: 440px; display: inline-block;">
                                <table class="table table-bordered">
                                    <tr>
                                        <td class="commontd"><%=GetGlobalResourceObject("CommanResource","McHRRate") %>Control Type </td>
                                        <td>
                                            <asp:DropDownList ID="ddlControlType" runat="server" Width="216px" Height="27px" meta:resourcekey="ddlControlTypeResource1"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="commontd"><%=GetGlobalResourceObject("CommanResource","McHRRate") %>Program Start ID</td>
                                        <td>
                                            <input type="text" id="txtProgStartId" maxlength="10" /></td>
                                    </tr>
                                    <tr>
                                        <td class="commontd"><%=GetGlobalResourceObject("CommanResource","McHRRate") %>Program End ID</td>
                                        <td>
                                            <input type="text" id="txtProgEndId" maxlength="10" /></td>
                                    </tr>
                                    <tr>
                                        <td class="commontd"><%=GetGlobalResourceObject("CommanResource","McHRRate") %>Recieve At Machine
                                        </td>
                                        <td>
                                            <input type="file" id="txtReceive" class="commontd" webkitdirectory directory multiple />
                                            <div id="my_file" style="color: white"></div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="commontd"><%=GetGlobalResourceObject("CommanResource","McHRRate") %>Sent At Machine
                                        </td>
                                        <td>
                                            <input type="file" id="txtSent" class="commontd" webkitdirectory directory multiple />
                                            <div id="my_file1" style="color: white"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="display: inline-block; width: 297px; margin-top: -18px">
                                <fieldset class="scheduler-border" style="height: 157px;">
                                    <legend class="scheduler-border">Non Printable Characters</legend>
                                    <input id="btnLf" value="LF" type="button" class="button" />
                                    <input id="btnCR" value="CR" type="button" class="button">
                                    <input id="btnSLE" value="SLE" type="button" class="button" />
                                    <input id="btnCSI" value="CSI" type="button" class="button">
                                    <input id="btnDC2" value="DC2" type="button" class="button">
                                    <input id="btnSYN" value="SYN" type="button" class="button">
                                    <input id="btnCAN" value="CAN" type="button" class="button">
                                    <input id="btnDC3" value="DC3" type="button" class="button">
                                    <input id="btnDC4" value="DC4" type="button" class="button">
                                    <input id="btnEM" value="EM" type="button" class="button">
                                    <input id="btnESC" value="ESC" type="button" class="button">
                                    <input id="btnSIB" value="SIB" type="button" class="button">
                                    <input id="btnFS" style="margin-left: 43px;" value="FS" type="button" class="button">
                                    <input id="btnGS" value="GS" type="button" class="button">
                                    <input id="btnRS" value="RS" type="button" class="button">
                                    <input id="btnUS" value="US" type="button" class="button">
                                </fieldset>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>

    <div class="row">

        <input value="<%=GetGlobalResourceObject("CommanResource","Save") %>" type="button" class="btn btn-info" id="btnSave" style="display: none; float: right; margin-right: 13px; margin-top: 4px;" />
    </div>


    <script type="text/javascript">
        var machineId = null, condition = "fristvalue"
        $(document).ready(function () {

            $('[id$=ddlManufacturer]').editableSelect();

            $('#txtSearch').keyup(function () {
                searchTable($(this).val());
            });

            function searchTable(inputVal) {
                var table = $('#tblMachineInfo');
                table.find('tr').each(function (index, row) {
                    var allCells = $(row).find('td');
                    if (allCells.length > 0) {
                        var found = false;
                        allCells.each(function (index, td) {
                            var regExp = new RegExp(inputVal, 'i');
                            if (regExp.test($(td).html())) {
                                found = true;
                                return false;
                            }
                        });
                        if (found == true) $(row).show(); else $(row).hide();
                    }
                });
            }
            function isAmararajaMangalPageEnabled() {
                var isEnabled = false;
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "MachineInformation.aspx/isAmararajaMangalEnabled",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        isEnabled = response.d;
                    },
                    error: function (response) {
                        alert("Error");
                    }
                });
                return isEnabled;
            }
            function getProtocolOptionsString() {

                var str = '<option selected="selected" value="RAW">RAW</option><option value="DAP">DAP</option><option value="MODBUS">MODBUS</option><option  value="PROFINET">PROFINET</option><option  value="MODFINET">MODFINET</option>';
                if (isAmararajaMangalPageEnabled() == true) {
                    str += '<option  value="Fanuc-MODBUS">Fanuc-MODBUS</option><option  value="Fanuc-Laser-MODBUS">Fanuc-Laser-MODBUS</option>';
                }
                return str;
            }
            var winHeight = $(window).height();
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (350);
            } else {
                winHeight = (450);
            }

            $("#griddata").height(winHeight);

            $("a[href$='#menuColorCode']").css("background-color", "#428bca");
            $("a[href$='#menuColorCode']").css("color", "#FFFFFF");
            $(".menuData").click(function () {
                $(".menuData").css("background-color", "");
                $(".menuData").css("color", "");

                $(this).css("background-color", "#428bca");
                $(this).css("color", "#FFFFFF");
            });

            $("#tblMachineInfo").on("keyup", ".numbersOnly", function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
            var defaultMachine;
            MachineInfoDetails();
            function MachineInfoDetails() {
                $.ajax({
                    type: "POST",
                    url: "MachineInformation.aspx/MachineDetailsInfo",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        $("#tblMachineInfo tr:gt(0)").each(function () {
                            $(this).remove();
                        });
                        if (itmData.length > 0) {

                            var protocolOptionList = getProtocolOptionsString();
                            $(itmData).each(function (index, md) {
                                machineId = itmData[0].MachineID;

                                $("#tblMachineInfo").append('<tr class="macInfoTable"><td style="max-width:150px;"><label class="machineId">' + md.MachineID + '</label></td><td class="update" style="max-width:150px;"><input class="hdfInterface" type="hidden"><input type="text" class="txtDes txtupdate textboxcommon" maxlength="80" style="text-align:left;text-wrap:normal;max-width:130px " value="' + md.Description +
                                    '"/></td><td class="update" style="max-width:110px;t"><input type="text" class="txtInter txtupdate textboxcommon numbersOnly" maxlength="4" style="text-align:left;" value="' + md.InterfaceID +
                                    '" onkeypress="return event.charCode >= 48 && event.charCode <= 57"/></td><td><select class="ddlProtocol dropdown form-control">' + protocolOptionList + '</select></td><td class="update" style="max-width:130px;"><input type="text" class="txtIP txtupdate textboxcommon numbersOnly" maxlength="20" style="text-align:left;" value="'
                                    + md.IP + '"/></td><td class="update" style="max-width:90px;"><input type="text" class="txtPortNo txtupdate textboxcommon numbersOnly" style="text-align:left;" maxlength="4" value="' + md.PortNo
                                    + '"/></td><td class="update" style="max-width:100px;"><input type="text" class="txtBulkNo txtupdate textboxcommon numbersOnly" style="text-align:left;" maxlength="50" value="' + md.BulkDataTransferPortNo
                                    + '"/></td  class="update" ><td class="update text-center" ><input type="checkbox" id="tpmtrak" ' + md.TPMTRAKEnable + ' class="update tpmtrak"/></td><td class="update text-center smarttrans"><input type="checkbox" id="smarttrans" ' + md.SmartTransEnable + ' class="update smarttrans"/></td><td class="update text-center"><input type="checkbox" id="shareddevice" ' + md.SharedDevice + ' class="update shareddevice"/></td><td class="text-center"><input type="checkbox" id="DNCEnable shareddevice"' + md.DNCEnable + ' class="update DNCEnable"/></td> <td class="update" style="display:flex;"><input tag="DNSIP" type="text" class="txtDNSIP txtupdate textboxcommon numbersOnly" maxlength="15" style="text-align:left;width:120px;" value="' + md.DNSIP + '"/><input type="text" class="txtDNSPortNo txtupdate textboxcommon numbersOnly" style="text-align:left;width:80px;" maxlength="4" value="' + md.DNSIPPortNo
                                    + '"/></td><td class="update"><input type="text" class="txtmchrrate txtupdate textboxcommon numbersOnly" maxlength="4"  onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="text-align:left;" value="'
                                    + md.mchrrate + '"/></td><td class="update text-center criticalmachine"><input type="checkbox" id="criticalmachine" ' + md.CriticalMachineEnable + ' class="update criticalmachine"/></td><td class="update text-center"><input type="checkbox" id="mobileEnabled"' + md.MobileEnable + ' class="update mobileEnabled"/></td> <td class="update" style="display:flex;"> <input type="text" class="form-control opcuaipaddress  txtupdate textboxcommon" style="width: 120px" value="' + md.OPCUAIPAddress + '" /> <input type="text" class="form-control opcuaport numbersOnly txtupdate textboxcommon" style="width: 80px" maxlength="4" value="' + md.OPCUAPort + '" /></td><td class="update text-center"><input type="checkbox" id="chkPalletEnabled"' + md.PalletEnabled + ' class="chkPalletEnabled update"  onclick="checkedPalletEnabled();"/></td><td class="update tdNoofFixture"><input type="text" class="form-control NoofFixture numbersOnly txtupdate textboxcommon" style="width: 80px" value="' + md.NoofFixture + '" diabled=' + md.PalletEnabled + ' onkeypress="return kepypressreturn();" /></td><td class="update tdNoofFixture2"><input type="text" class="form-control NoofFixture2 numbersOnly txtupdate textboxcommon" style="width: 80px" value="' + md.NoofFixtureForPallet2 + '" diabled=' + md.PalletEnabled + ' onkeypress="return kepypressreturn();" /> </td></tr> ');

                                $('#tblMachineInfo .ddlProtocol').last().val(md.ProtcolInString);
                                var yourString = md.ProgramEnabledTextTemplate;
                                var result = yourString.substring(1, yourString.length - 1);
                                result = md.ProgramEnabledTextTemplate.split(",");
                                var length = md.TpmFlags.length;
                                var data = '';
                                if (result != undefined) {
                                    $(md.TpmFlags).each(function (index, att) {
                                        if (att != '') {
                                            data += '' + att;
                                            if (index != length - 1) {
                                                data += ',';
                                            }
                                        }
                                    });
                                    // data = data.substr(0, data.length - 1);
                                    $('#tblMachineInfo .ddlProgram').last().val([data]);
                                }
                                $('#tblMachineInfo .ddlProgram').multiselect({
                                    includeSelectAllOption: true
                                });
                            });
                            if ($("#txtSearch").val())
                                searchTable($("#txtSearch").val());
                        }
                        else {
                            $('#tblMachineInfo').append('<tr><td colspan="10" style="text-align: center;">No record found for given search criteria !!</td></tr>');
                        }
                        if (machineId != null) {
                            fristMachineInfo(machineId)
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            window.location.href = "Login.aspx";
                    }
                });
            }

            $("#tblMachineInfo").on("click", ".update", function () {

                $("[id$=lblMessages]").text("");
                $(this).closest('tr').find('input[type=hidden]').val("update");
                $("#tblMachineInfo tr td").find('.txtupdate').removeClass("form-control");
                $("#tblMachineInfo tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input[type=text]').removeClass("textboxcommon");
                $(this).closest('td').find('input[type=text]').addClass("form-control");
            });
            var countValue = 0;
            //$("#tblMachineInfo").on("change", ".txtInter", function () {                
            //    var friValue = $(this).closest('td').find('input').val();
            //    $("#tblMachineInfo .txtInter").each(function () {
            //        var value = $(this).val();
            //        if (friValue == value) {                       
            //            countValue++;
            //        }
            //    });
            //    if (countValue > 1) {
            //        alert("Please do not enter dublicate Interface ID");
            //        $(this).closest('td').find('input').val('');
            //        countValue = 0;
            //        return false;
            //    }
            //    //$("[id$=lblMessages]").text("");
            //    //$(this).closest('tr').find('input[type=hidden]').val("update");
            //    //$("#tblMachineInfo tr td").find('.txtupdate').removeClass("form-control");
            //    //$("#tblMachineInfo tr td").find('.txtupdate').addClass("textboxcommon");
            //    //$(this).closest('td').find('input').removeClass("textboxcommon");
            //    //$(this).closest('td').find('input').addClass("form-control");
            //});

            $("#tblMachineInfo").on("click", ".dropdown", function () {
                $("[id$=lblMessages]").text("");
                $(this).closest('tr').find('input[type=hidden]').val("update");
                $("#tblMachineInfo tr td").find('input').removeClass("form-control");
                $("#tblMachineInfo tr td").find('input').addClass("textboxcommon");
            });

            $("#tblMachineInfo").on("change", ".dropdown", function () {
                $("[id$=lblMessages]").text("");
                $(this).closest('tr').find('input[type=hidden]').val("update");
                $("#tblMachineInfo tr td").find('input').removeClass("form-control");
                $("#tblMachineInfo tr td").find('input').addClass("textboxcommon");
            });

            $("#tblMachineInfo").on("dblclick", "tr", function () {
                $("[id$=lblMessages]").text("");
                if ($("#btnCancel").css("display") == "block") {
                    $("#btnSave").css("display", "none");
                }
                condition = "bothvalue";
                $("#btnSave").css("display", "inline");
                machineId = $(this).closest('tr').find('.machineId').html();
                if (machineId != "") {
                    fristMachineInfo(machineId);
                    //machineEfficiencyTargetData(machineId, "MachineEfficiencyTargetData");
                }
            });

            function fristMachineInfo(machineId) {
                resetValue();
                $("#machinelegend").html("Machine : " + machineId);
                //$("[id$=ddlMachine]").val(machineId);
                machineEfficiencyColorCodeInfo(machineId, "MachineEfficiencyColorCodeInfo");
                //machineControlInfo(machineId, "MachineControlInfo");
                machineMakeData(machineId, "MachineMakeData");
            }

            function resetValue() {
                $("#txtPeOk").val("");
                $("#txtPeNotOk").val("");
                $("#txtAeOk").val("");
                $("#txtAeNotOk").val("");
                $("#txtOaeOk").val("");
                $("#txtOaeNotOk").val("");
                $("#txtQeOk").val("");
                $("#txtQeNotOk").val("");
                $("#txtOperatorPEOK").val("");
                $("#txtOperatorPENotOK").val("");
                $("[id$=ddlManufacturer]").val("");
                $("#txtMfgDate").val("");
                $("#txtAddress").val("");
                $("#txtPlace").val("");
                $("#txtPhone").val("");
                $("#txtPerson").val("");
                $("#txtProgStartId").val("");
                $("#txtProgEndId").val("");
                $("#my_file").html("");
                $("#my_file1").html("");
            }

            function machineEfficiencyColorCodeInfo(machineId, functionName) {
                $.ajax({
                    type: "POST",
                    url: "MachineInformation.aspx/" + functionName,
                    contentType: "application/json; charset=utf-8",
                    data: '{machineId:"' + machineId + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        if (itmData != null) {
                            $("#txtPeOk").val(itmData.PEOk);
                            $("#txtPeNotOk").val(itmData.PENotOk);
                            $("#txtAeOk").val(itmData.AEOk);
                            $("#txtAeNotOk").val(itmData.AENotOk);
                            $("#txtOaeOk").val(itmData.OEEOk);
                            $("#txtOaeNotOk").val(itmData.OEENotOk);
                            $("#txtQeOk").val(itmData.QEOk);
                            $("#txtQeNotOk").val(itmData.QENotOk);
                            $("#txtOperatorPEOK").val(itmData.OperatorPEGreen);
                            $("#txtOperatorPENotOK").val(itmData.OperatorPERed);
                        }
                    },
                    error: function (response) {
                        alert("Error");
                    }
                });
            }

            function machineControlInfo(machineId, functionName) {
                $.ajax({
                    type: "POST",
                    url: "MachineInformation.aspx/" + functionName,
                    contentType: "application/json; charset=utf-8",
                    data: '{machineId:"' + machineId + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        if (itmData != null) {

                            $("[id$=ddlControlType]").val(itmData.ControlType);
                            var pStartId = [];
                            var pEndId = [];
                            if (itmData.ProgStartId != "") {
                                pStartId = itmData.ProgStartId.split(',');
                                for (var i = 0; i < pStartId.length - 1; i++) {
                                    if (Number(pStartId[i]) < 32) {
                                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|" + pStartId[i]);
                                    }
                                    else {
                                        $("#txtProgStartId").val($("#txtProgStartId").val() + pStartId[i]);
                                    }
                                }
                            }
                            else {
                                $("#txtProgStartId").val('');
                            }

                            if (itmData.ProgramEndId != "") {
                                pEndId = itmData.ProgramEndId.split(',');
                                for (var i = 0; i < pEndId.length - 1; i++) {
                                    if (Number(pEndId[i]) < 32) {
                                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|" + pEndId[i]);
                                    }
                                    else {
                                        $("#txtProgEndId").val($("#txtProgEndId").val() + pEndId[i]);
                                    }
                                }
                            }
                            else {
                                $("#txtProgEndId").val('');
                            }
                            //$("#txtProgStartId").val(itmData.ProgStartId);
                            // $("#txtProgEndId").val(itmData.ProgramEndId);
                            var filename;
                            if (itmData.RecieveAtMachine.substring(3, 11) == 'fakepath') {
                                filename = itmData.RecieveAtMachine.substring(12);
                            }
                            $('#my_file').html(filename);
                            $("#txtReceive").html(filename);
                            if (itmData.RecieveFromMachine.substring(3, 11) == 'fakepath') {
                                filename = itmData.RecieveFromMachine.substring(12);
                            }
                            $("#txtSent").html(filename);
                            $('#my_file1').html(filename);
                        }
                    },
                    error: function (response) {
                        alert("Error");
                    }
                });
            }

            function machineMakeData(machineId, functionName) {
                $.ajax({
                    type: "POST",
                    url: "MachineInformation.aspx/" + functionName,
                    contentType: "application/json; charset=utf-8",
                    data: '{machineId:"' + machineId + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        if (itmData != null) {
                            $("[id$=ddlManufacturer]").val(itmData.Manufacturer);
                            $("#txtMfgDate").val(itmData.DateOfManufacturer);
                            $("#txtAddress").val(itmData.Address);
                            $("#txtPlace").val(itmData.Place);
                            $("#txtPhone").val(itmData.Phone);
                            $("#txtPerson").val(itmData.ContactPerson);
                        }
                    },
                    error: function (response) {
                        alert("Error");
                    }
                });
            }

            function machineEfficiencyTargetData(machineId, functionName) {
                $.ajax({
                    type: "POST",
                    url: "MachineInformation.aspx/" + functionName,
                    contentType: "application/json; charset=utf-8",
                    data: '{machineId:"' + machineId + '"}',
                    dataType: "json",
                    success: function (response) {
                        var itmData = response.d;
                        if (itmData != null) {

                        }
                    },
                    error: function (response) {
                        alert("Error");
                    }
                });
            }


            $("#tblMachineInfo").on("click", ".insert", function () {
                $(this).closest('tr').find('input[type=hidden]').val("update");
                $("#tblMachineInfo tr td").find('.txtupdate').addClass("textboxcommon");
            });

            $("#tblMachineInfo").on("keyup", ".txtDelete", function () {
                $(this).closest('tr').find('input[type=hidden]').val("update");
                let value = $(this).val();
                machineId = value;
            });

            $("#btnNew").click(function () {
                $("#tblMachineInfo").append('<tr><td class="insert" style="max-width:120px;"><input type="text" class="machineId txtDelete form-control" style="text-align:left;"/></td><td class="insert" style="max-width:130px;"><input class="hdfInterface" type="hidden"><input type="text" maxlength="150" class="txtDes form-control" style="text-align:left;"/></td><td class="insert" style="max-width:140px;"><input type="text" maxlength="4" class="txtInter form-control numbersOnly" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="text-align:left;"/></td><td class="insert"><select class="ddlProtocol form-control">' + getProtocolOptionsString() + '</select></td><td class="insert" style="max-width:150px;"><input type="text"  maxlength="20" class="txtIP form-control numbersOnly" style="text-align:left;"/></td><td class="insert" style="max-width:110px;"><input type="text"  maxlength="4" class="txtPortNo form-control numbersOnly" style="text-align:left;"/></td><td class="insert" style="max-width:170px;"><input type="text"  maxlength="50" class="txtBulkNo form-control numbersOnly" style="text-align:left;"/></td><td class="insert text-center"><input type="checkbox" class="insert tpmtrak"/></td><td class="insert  text-center"><input type="checkbox" class="insert smarttrans"/></td><td class="insert  text-center"><input type="checkbox" class="insert shareddevice"/></td><td class="insert  text-center"><input type="checkbox" class="insert DNCEnable"/></td> <td class="insert" style="display:flex;"><input type="text" maxlength="15" class="txtDNSIP form-control numbersOnly" style="text-align:left;width:120px;"/><input type="text" maxlength="4" class="txtDNSPortNo form-control numbersOnly" style="text-align:left;width:80px;margin-left:5px;"/></td><td class="insert  text-center" style="max-width:120px;"><input type="text" maxlength="4" class="txtmchrrate form-control numbersOnly" style="text-align:left;"/></td><td class="insert  text-center"><input type="checkbox" class="insert criticalmachine"/></td><td class="insert  text-center"><input type="checkbox" class="insert mobileEnabled"/></td><td class="insert" style="display:flex;"><input type="text"style="width: 120px" class="form-control opcuaipaddress "/> <input type="text" style="width: 80px" maxlength="4"  class="form-control numbersOnly opcuaport"/></td><td class="insert  text-center"><input type="checkbox" id="chkPalletEnabledInsert" onclick="checkedPalletEnabled();" class="chkPalletEnabledInsert chkPalletEnabled"/></td><td class="insert tdNoofFixture"><input type="text" disabled=true class="form-control NoofFixtureInsert NoofFixture numbersOnly" style="width: 80px" onkeypress="return kepypressreturn();" /></td><td class="insert tdNoofFixture2"><input type="text" disabled=true class="form-control NoofFixtureInsert NoofFixture numbersOnly" style="width: 80px" onkeypress="kepypressreturn();" /></td></tr>');
                
                $("#tblMachineInfo .machineId").focus();
                $('#tblMachineInfo .ddlProgram').multiselect({
                    includeSelectAllOption: true
                });
                $("#btnCancel").css("display", "inline");
                $("#btnNew").css("display", "none");
                condition = "bothvalue";
                $("#btnSave").css("display", "none");
                $("[id$=lblMessages]").text("");
                //resetValue();
            });

            $("#btnCancel").click(function () {
                if ($("#tblMachineInfo tr:gt(0)").find(".txtDelete").length > 0) {
                    $(".txtDelete").closest("tr").remove();
                    $("#btnCancel").css("display", "none");
                    $("#btnNew").css("display", "inline");
                    $("#btnSave").css("display", "none");
                    $("[id$=lblMessages]").text("");
                    MachineInfoDetails();
                    return false;
                }
            });


            $("#btnUpdate").click(function () {

                var count = 0; var IP = "", Dataport = "", Machineinterfaceid = ""; var index = 0;
                $("#tblMachineInfo tr:gt(0)").each(function (src, i) {

                    if ($(this, i).closest("tr").find(".hdfInterface").val() == "update") {
                        //
                        index = i;
                        IP = $(this, i).closest("tr").find('.txtIP').val();
                        Dataport = $(this, i).closest("tr").find('.txtPortNo').val();
                        Machineinterfaceid = $(this, i).closest("tr").find('.txtInter').val();

                    }
                    console.log(IP, Dataport);
                    if (($(this, i).closest("tr").find('.machineId').html() == "")) {
                        if (($(this, i).closest("tr").find('.machineId').val() == "")) {
                            count++;
                            alert('MachineID cannot be Empty !!');
                            $(this, i).closest("tr").find('.machineId').focus()
                            return false;
                        }
                    }
                    if (($(this, i).closest("tr").find('.txtDes').val() == "")) {
                        count++;
                        alert('Description cannot be empty !!');
                        $(this, i).closest("tr").find('.txtDes').focus();
                        return false;
                    }
                    if (($(this, i).closest("tr").find('.txtInter').val() == "")) {
                        count++;
                        alert('Interfaceid cannot be empty !!');
                        $(this, i).closest("tr").find('.txtInter').focus();
                        return false;
                    }
                    inter = parseInt($(this, i).closest("tr").find('.txtInter').val());
                    if (inter == 0) {
                        count++;
                        alert('Interfaceid cannot be zero !!');
                        $(this, i).closest("tr").find('.txtInter').focus();
                        return false;
                    }
                    //if (($(this, i).closest("tr").find('.txtPortNo').val() == "")) {
                    //    count++;
                    //    alert('Data Port cannot be empty !!');
                    //    $(this, i).closest("tr").find('.txtPortNo').focus();
                    //    return false;
                    //}
                    //if (($(this, i).closest("tr").find('.txtPortNo').val() == "")) {
                    //    count++;
                    //    alert('Data Port cannot be empty !!');
                    //    $(this, i).closest("tr").find('.txtPortNo').focus();
                    //    return false;
                    //}
                    //if (($(this, i).closest("tr").find('.txtIP').val() == "")) {
                    //    count++;
                    //    alert('Enter valid IP address !!');
                    //    $(this, i).closest("tr").find('.txtIP').focus();
                    //    return false;
                    //}
                    if ($(this).closest("tr").find('.DNCEnable').eq(0).is(':checked')) {
                        if (($(this, i).closest("tr").find('.txtDNSIP').val() == "")) {
                            alert('Enter valid DNS IP address !!');
                            $(this, i).closest("tr").find('.txtDNSIP').focus();
                            return false;

                        }
                        if (($(this, i).closest("tr").find('.txtDNSPortNo').val() == "")) {
                            alert('Enter valid DNS Port Number!!');
                            $(this, i).closest("tr").find('.txtDNSPortNo').focus();
                            return false;
                        }

                    } else {
                        if (($(this, i).closest("tr").find('.txtIP').val() == "")) {
                            alert('Enter valid IP address !!');
                            $(this, i).closest("tr").find('.txtIP').focus();
                            return false;
                        }
                        if (($(this, i).closest("tr").find('.txtPortNo').val() == "")) {
                            alert('Enter valid Port Number !!');
                            $(this, i).closest("tr").find('.txtPortNo').focus();
                            return false;

                        }
                    }

                });
                var tosave = true;
                $("#tblMachineInfo tr:gt(0)").each(function (src, i) {

                    if (i != index) {
                        if ($(this, i).closest("tr").find('.txtIP').val() == IP && !$(this).closest("tr").find('.DNCEnable').eq(0).is(':checked')) {
                            if ($(this, i).closest("tr").find('.txtPortNo').val() == Dataport) {
                                alert('Enter valid IP address !!');
                                $(this, index).closest("tr").find('.txtPortNo').focus();
                                tosave = false;
                                return false;
                            }
                        }
                        if ($(this, i).closest("tr").find('.txtInter').val() == Machineinterfaceid) {
                            tosave = false;
                            alert('Enter Have Duplicate interface ID !!');
                            return false;
                        }
                    }
                });

                if (count != 0) {
                    return false;
                }
                count = 0;

                if (tosave) {
                    var values = {
                        model:
                        {
                            ListMachineInfo: []
                        }
                    };
                    $("#tblMachineInfo tr:gt(0)").each(function () {
                        if ($(this).closest("tr").find(".hdfInterface").val() == "update") {
                            count++;

                            var innermodel =
                            {

                                MachineID: $(this).closest("tr").find(".machineId").html() == "" ? $(this).closest("tr").find(".machineId").val() : $(this).closest("tr").find(".machineId").html(),
                                Description: $(this).closest("tr").find(".txtDes").val(),
                                InterfaceID: $(this).closest("tr").find(".txtInter").val(),
                                ProtcolInString: $(this).closest("tr").find(".ddlProtocol").val(),
                                IP: $(this).closest("tr").find('.txtIP').val(),
                                PortNo: $(this).closest("tr").find('.txtPortNo').val(),
                                BulkDataTransferPortNo: $(this).closest("tr").find('.txtBulkNo').val(),
                                TpmFlags: ($(this).closest("tr").find('.ddlProgram').val()),
                                mchrrate: $(this).closest("tr").find('.txtmchrrate').val(),
                                DNCEnable: $(this).closest("tr").find('.DNCEnable').eq(0).is(':checked'),
                                TPMTRAKEnable: $(this).closest("tr").find('.tpmtrak').eq(0).is(':checked'),
                                SmartTransEnable: $(this).closest("tr").find('#smarttrans').eq(0).is(':checked'),
                                SharedDevice: $(this).closest("tr").find('#shareddevice').eq(0).is(':checked'),
                                DNSIP: ($(this).closest("tr").find('.txtDNSIP').val()),
                                DNSIPPortNo: $(this).closest("tr").find('.txtDNSPortNo').val(),
                                CriticalMachineEnable: $(this).closest("tr").find('#criticalmachine').eq(0).is(':checked'),
                                MobileEnable: $(this).closest("tr").find('.mobileEnabled').eq(0).is(':checked'),
                                OPCUAIPAddress: $(this).closest("tr").find('.opcuaipaddress').val(),
                                OPCUAPort: $(this).closest("tr").find('.opcuaport').val(),
                                NoofFixture: $(this).closest("tr").find('.NoofFixture').val(),
                                NoofFixtureForPallet2: $(this).closest("tr").find('.NoofFixture2').val(),
                                PalletEnabledBool: $(this).closest("tr").find('.chkPalletEnabled').is(":checked"),
                            };
                            values.model.ListMachineInfo.push(innermodel);
                        }
                    });

                    if (count == 0) {

                        if (machineId != null) {
                            $("#btnSave").trigger("click");
                            return false;
                        } else {
                            alert('Please atleast one record update other wise insert !!');
                            return false;
                        }
                    }
                    if ($("#btnCancel").css("display") == "block") {

                        if (ValidateEfficiencyFeilds()) {
                            saveAndUpdate(values, "UpdateMachineInfo");
                            $("#btnSave").trigger("click");
                            return false;
                        }
                    } else {

                        saveAndUpdate(values, "UpdateMachineInfo");
                    }
                }
            });

            function saveAndUpdate(values, functionName) {
                debugger;
                if (functionName == "UpdateMachineInfo") {
                    var DNCEnable = values.model.ListMachineInfo[values.model.ListMachineInfo.length - 1].DNCEnable;
                    var DNSIP = values.model.ListMachineInfo[values.model.ListMachineInfo.length - 1].DNSIP;
                    var DNSIPPortNo = values.model.ListMachineInfo[values.model.ListMachineInfo.length - 1].DNSIPPortNo;
                    var IP = values.model.ListMachineInfo[values.model.ListMachineInfo.length - 1].IP;
                    var PortNo = values.model.ListMachineInfo[values.model.ListMachineInfo.length - 1].PortNo;
                    if (DNCEnable == true && (DNSIP == '' || DNSIPPortNo == '')) {
                        var alertmessage = DNSIP == '' ? "Enter DNS IP Address" : "Enter DNS Port Number";
                        alert(alertmessage);

                        return false;
                    } else if (DNCEnable != true && (IP == '' || PortNo == '')) {
                        var alertmessage = IP == '' ? "Enter IP Address" : "Enter Port Number";
                        alert(alertmessage);

                        return false;
                    }
                }
                $.ajax({
                    type: "POST",
                    url: "MachineInformation.aspx/" + functionName,
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(values),
                    dataType: "json",
                    success: onSave,
                    error: function (response) {
                        alert("Error");
                    }
                });
            }
            var savecolor = false;
            function onSave(saveResponce) {
                debugger;
                var result = saveResponce.d;
                if (result == "Records update successfully") {
                    MachineInfoDetails();
                    resetValue();
                    savecolor = true;
                    $("#btnCancel").css("display", "none");
                    $("#btnNew").css("display", "inline");
                    // $("#btnSave").css("display", "none");
                    $("[id$=lblMessages]").text(result);
                    $("[id$=lblMessages]").css("color", "white");
                    return false;
                }
                else {
                    debugger;
                    savecolor = false;
                    alert(result);
                }
            }

            $(".button").hover(function () {
                $(this).css("background-color", "#2080D0");
            }, function () {
                $(this).css("background-color", "#000000");
            });

            $('.number').keypress(function (event) {
                if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            });

            $("#btnSave").click(function () {

                //if (savecolor) {
                saveColorInfo();
                //}
            });

            function saveColorInfo() {
                var values = {
                    model:
                    {
                        ListMachineInformation: []
                    }
                };
                try {
                    if (ValidateEfficiencyFeilds()) {
                        var EffiTargetCriticalMachine = 0;
                        //alert("Success");
                        var machineInfoData = {
                            selectedMachine: machineId,// $("[id$=ddlMachine]").val(),
                            desc: "",
                            status: 0,
                            mchrrate: 0,
                            PortNo: 0,
                            settings: "",
                            InterfaceID: "",
                            IP: "",
                            IpPortNo: "",
                            mode: 0,
                            autoload: 0,
                            tpmTrakEnabled: 0,
                            txtPeOk: Number($("#txtPeOk").val()),
                            txtPeNotOk: Number($("#txtPeNotOk").val()),
                            txtAeOk: Number($("#txtAeOk").val()),
                            txtAeNotOk: Number($("#txtAeNotOk").val()),
                            txtOaeOk: Number($("#txtOaeOk").val()),
                            txtOaeNotOk: Number($("#txtOaeNotOk").val()),
                            txtQeOk: Number($("#txtQeOk").val()),
                            txtQeNotOk: Number($("#txtQeNotOk").val()),
                            txtOperatorPENotOK: Number($("#txtOperatorPENotOK").val()),
                            txtOperatorPEOK: Number($("#txtOperatorPEOK").val()),
                            BulkDataTransferPortNo: "",
                            multiSpindleFlag: true,
                            deviceType: 0,
                            ppTransferEnabled: true,
                            smartTransEnabled: true,
                            ignoreCoFromMach: "",
                            autoSetupChangeDown: "",
                            machineWiseOwner: "",
                            EffiTargetCriticalMachine: false,
                            DapEnabled: 0,
                            lowerPowerThreshold: 0,
                            upperPowerThreshold: 0,
                            ethernetEnabled: true,
                            nTo1Device: true,
                            startDate: "",
                            endDate: "",
                            txtProgStartId: $("#txtProgStartId").val(), //this part not visible
                            txtProgEndId: $("#txtProgEndId").val(),//this part not visible
                            fileNameFrom: "",
                            txtreceive: $("#txtReceive").val(),//this part not visible
                            txtSend: $("#txtSent").val(),//this part not visible
                            nodeReference: "",
                            nodeId: "",
                            sortOrder: 0,
                            manufacturer: $("[id$=ddlManufacturer]").val(),
                            dateOfManufacture: $("#txtMfgDate").val(),
                            address: $("#txtAddress").val(),
                            place: $("#txtPlace").val(),
                            phone: $("#txtPhone").val(),
                            contactPerson: $("#txtPerson").val(),
                            controlName: $("[id$=ddlControlType]").val(), //this part not visible
                            CriticalMachineEnable: false,
                            MobileEnable: false,
                        };
                        values.model.ListMachineInformation.push(machineInfoData);
                        saveAndUpdate(values, "UpdateMachineInformation");
                        $("#btnSave").css("display", "none");
                    }
                    //$.ajax({
                    //    type: "POST",
                    //    url: "MachineInformation.aspx/UpdateMachineInformation",
                    //    contentType: "application/json; charset=utf-8",
                    //    data: JSON.stringify(machineInfoData),
                    //    dataType: "json",
                    //    success: onSave,
                    //    error: function (response) {
                    //        alert("Error");
                    //    }
                    //});

                } catch (e) {

                }
            }

            function ValidateEfficiencyFeilds() {
                if (machineId == null) {
                    alert("Please select Machine !!");
                    return false;
                }
                if ($("#txtPeOk").val() == "") {
                    alert("Please enter Green PE !!");
                    $("#txtPeOk").focus();
                    return false;
                }
                if ($("#txtPeNotOk").val() == "") {
                    alert("Please enter Red PE !!");
                    $("#txtPeNotOk").focus();
                    return false;
                }
                if ($("#txtAeOk").val() == "") {
                    alert("Please enter Green AE !!");
                    $("#txtAeOk").focus();
                    return false;
                }
                if ($("#txtAeNotOk").val() == "") {
                    alert("Please enter Red AE !!");
                    $("#txtAeNotOk").focus();
                    return false;
                }
                if ($("#txtOaeOk").val() == "") {
                    alert("Please enter Green OAE !!");
                    $("#txtOaeOk").focus();
                    return false;
                }
                if ($("#txtOaeOk").val() == "") {
                    alert("Please enter Green OAE !!");
                    $("#txtOaeOk").focus();
                    return false;
                }
                if ($("#txtOaeNotOk").val() == "") {
                    alert("Please enter Red OAE !!");
                    $("#txtOaeNotOk").focus();
                    return false;
                }
                if ($("#txtQeOk").val() == "") {
                    alert("Please enter Green QE !!");
                    $("#txtQeOk").focus();
                    return false;
                }
                if ($("#txtQeNotOk").val() == "") {
                    alert("Please enter Red QE !!");
                    $("#txtQeNotOk").focus();
                    return false;
                }
                if ($("#txtOperatorPEOK").val() == "") {
                    alert("Please enter Green Operator PE !!");
                    $("#txtOperatorPEOK").focus();
                    return false;
                }
                if ($("#txtOperatorPENotOK").val() == "") {
                    alert("Please enter Red Operator PE !!");
                    $("#txtOperatorPENotOK").focus();
                    return false;
                }
                if (Number($("#txtPeOk").val()) > 100) {
                    alert("Green PE Value cannot be greater than 100..!!");
                    $("#txtPeOk").focus();
                    return;
                }
                else if (Number($("#txtPeNotOk").val()) > 100) {
                    alert("Red PE Value cannot be greater than 100..!!");
                    $("#txtPeNotOk").focus();
                    return;
                }
                else if (Number($("#txtAeOk").val()) > 100) {
                    alert("Green AE Value cannot be greater than 100..!!");
                    $("#txtAeOk").focus();
                    return;
                }
                else if (Number($("#txtAeNotOk").val()) > 100) {
                    alert("Red AE Value cannot be greater than 100..!!");
                    $("#txtAeNotOk").focus();
                    return;
                }
                else if (Number($("#txtOaeOk").val()) > 100) {
                    alert("Green OAE Value cannot be greater than 100..!!");
                    $("#txtOaeOk").focus();
                    return;
                }
                else if (Number($("#txtOaeNotOk").val()) > 100) {
                    alert("Red OAE Value cannot be greater than 100..!!");
                    $("#txtOaeNotOk").focus();
                    return;
                }
                else if (Number($("#txtQeOk").val()) > 100) {
                    alert("Green OE Value cannot be greater than 100..!!");
                    $("#txtQeOk").focus();
                    return;
                }
                else if (Number($("#txtQeNotOk").val()) > 100) {
                    alert("Red OE Value cannot be greater than 100..!!");
                    $("#txtQeNotOk").focus();
                    return;
                }
                else if (Number($("#txtOperatorPEOK").val()) > 100) {
                    alert("Green Operator PE Value cannot be greater than 100..!!");
                    $("#txtQeOk").focus();
                    return;
                }
                else if (Number($("#txtOperatorPENotOK").val()) > 100) {
                    alert("Red Operator PE Value cannot be greater than 100..!!");
                    $("#txtQeNotOk").focus();
                    return;
                }
                else if (Number($("#txtPeOk").val()) <= Number($("#txtPeNotOk").val())) {
                    alert("Green Color Code must be greater then Red Color Code.");
                    $("#txtPeNotOk").focus();
                    return;
                }
                else if (Number($("#txtAeOk").val()) <= Number($("#txtAeNotOk").val())) {
                    alert("Green Color Code must be greater then Red Color Code.");
                    $("#txtAeNotOk").focus();
                    return;
                }
                else if (Number($("#txtOaeOk").val()) <= Number($("#txtOaeNotOk").val())) {
                    alert("Green Color Code must be greater then Red Color Code.");
                    $("#txtOaeNotOk").focus();
                    return;
                }
                else if (Number($("#txtQeOk").val()) <= Number($("#txtQeNotOk").val())) {
                    alert("Green Color Code must be greater then Red Color Code.");
                    $("#txtQeNotOk").focus();
                    return;
                }
                else if (Number($("#txtOperatorPEOK").val()) <= Number($("#txtOperatorPENotOK").val())) {
                    alert("Green Color Code must be greater then Red Color Code.");
                    $("#txtOperatorPENotOK").focus();
                    return;
                }
                else {
                    return true;
                }
            }

            var clear = false;

            $("#txtProgStartId").click(function () {
                clear = true;
            });

            $("#txtProgEndId").click(function () {
                clear = false;
            });

            $("#btnLf").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|10");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|10");
                }
            });
            $("#btnCR").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|13");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|13");
                }
            });
            $("#btnSLE").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|16");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|16");
                }
            });
            $("#btnCSI").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|17");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|17");
                }
            });
            $("#btnDC2").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|18");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|18");
                }
            });
            $("#btnSYN").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|22");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|22");
                }
            });
            $("#btnCAN").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|24");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|24");
                }
            });
            $("#btnDC3").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|19");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|19");
                }
            });
            $("#btnDC4").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|20");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|20");
                }
            });
            $("#btnEM").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|25");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|25");
                }
            });
            $("#btnESC").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|27");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|27");
                }
            });
            $("#btnSIB").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|26");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|26");
                }
            });
            $("#btnFS").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|28");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|28");
                }
            });
            $("#btnGS").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|29");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|29");
                }
            });
            $("#btnRS").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|30");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|30");
                }
            });
            $("#btnUS").click(function () {
                if (clear) {
                    if ($("#txtProgStartId").val().length > 8) {
                        alert("Program Start ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgStartId").val($("#txtProgStartId").val() + "|31");
                } else {
                    if ($("#txtProgEndId").val().length > 8) {
                        alert("Program End ID cannot contain more than 10 characters!")
                        return false
                    } else
                        $("#txtProgEndId").val($("#txtProgEndId").val() + "|31");
                }
            });
        });

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


        function kepypressreturn(evt) {
            if (evt.charCode >= 48 && evt.charCode <= 54) {
                return true;
            }
            return false;
        }

        function checkedPalletEnabled() {
            debugger;
            if ($("#chkPalletEnabledInsert").is(":checked") || $("#chkPalletEnabled").is(":checked")) {
                $(".NoofFixtureInsert").prop("disabled", false);
            }
            else {
                $(".NoofFixtureInsert").prop("disabled", true);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
