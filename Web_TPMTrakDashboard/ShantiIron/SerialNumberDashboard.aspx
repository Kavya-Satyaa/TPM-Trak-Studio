<%@ Page Title="Serial Number Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SerialNumberDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.SerialNumberDashboard" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #serialNoDetialsContainer > table > tbody > tr:first-child, .popupTable > table > tbody > tr:first-child {
            background-color: #2E6886 !important;
            cursor: pointer;
            height: 38px;
        }

            #serialNoDetialsContainer > table > tbody > tr:first-child td , .popupTable > table > tbody >  tr:first-child th {
                color: white !important;
                cursor: pointer;
                font-weight: bold;
            }
            /*, #serialNoDetialsContainer > table > tbody > tr:nth-child(2) td a*/
        #serialNoDetialsContainer > table > tbody > tr:nth-child(2) td {
            text-align: center;
            font-weight: bold;
            color: black !important;
        }
         .changeCusrsor{
            cursor: pointer;
            display: block;
         }

        #serialNoDetialsContainer tr {
            background-color: white !important;
        }

            #serialNoDetialsContainer tr td {
                color: black !important;
            }

        .menuData {
            border: 1px solid #2E6886;
            border-radius: 5px;
            margin-right: 5px;
            color: #2E6886;
            font-weight: bold;
        }

        #inputContainer {
            box-shadow: 2px 2px 8px 2px #efe7e7;
        }
        input[type="checkbox"] {
            width: 20px;
            height: 20px;
        }
    </style>
    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnSlno" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnCompID" ClientIDMode="Static" />
             <asp:HiddenField runat="server" ID="hdnPlant" ClientIDMode="Static" />
             <asp:HiddenField runat="server" ID="hdnGroupID" ClientIDMode="Static" />
            <div class="row text-center" style="width: 70%">
                <asp:Label ID="lblMessages" EnableViewState="false" runat="server" ClientIDMode="Static" Style="font-weight: bold; color: white; font-family: Calibri;"></asp:Label>
            </div>
            <div class="row" style="width: 85%; margin-left: 10px">

                <table class="table table-bordered" id="tblsearch">
                    <tr>
                        <td>
                            <div style="margin-top: 5px;" class="commontd">
                                <b>
                                    <asp:Label ID="lblreason" runat="server" Text="Plant ID"></asp:Label>

                                </b>
                            </div>
                        </td>
                        <td>
                            <div class="col-md-12">
                                <asp:DropDownList ID="ddlLineId" ClientIDMode="Static" runat="server"  onchange="return BindGroupIDs()" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </td>
                         <td>
                            <div style="margin-top: 5px;" class="commontd">
                                <b>
                                    <asp:Label ID="lblGroupId" runat="server" Text="Cell ID"></asp:Label>

                                </b>
                            </div>
                        </td>
                        <td>
                            <div class="col-md-12">
                                <asp:DropDownList ID="ddlGroupId" ClientIDMode="Static"  runat="server"   CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div style="margin-top: 5px;" class="commontd">
                               <b><asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label></b> 
                            </div>
                        </td>
                        <td class="input-group" style="min-width: 150px; border: 0">

                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>

                        </td>
                        <td>
                            <div>
                                <asp:Button runat="server" Text="Search" class="btn btn-info" OnClientClick="return BindSerialNumberDetails('Date');" ID="btnDatesearch"></asp:Button>
                            </div>
                        </td>

                        <td>
                            <div style="margin-top: 5px;" class="commontd">
                               <b><asp:Label ID="lblSerialNo" runat="server" Text="Serial Number"></asp:Label></b> 
                            </div>
                        </td>
                        <td>
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="txtSerialNumber" ClientIDMode="Static" CssClass="form-control" AutoCompleteType="Disabled" ></asp:TextBox>
                                  <%--<datalist id="dlSerialNumbers" runat="server" clientidmode="static" autopostback="true"></datalist>--%>
                               <%-- <asp:DropDownList ID="ddlSerialNumber" runat="server" CssClass="form-control">
                                </asp:DropDownList>--%>
                            </div>
                        </td>
                        <td>
                            <div>
                                <asp:Button runat="server" Text="Search" class="btn btn-info" ID="btnSlnoSerach" OnClientClick="return BindSerialNumberDetails('SlNo');" ></asp:Button>
                            </div>
                        </td>
                    </tr>
                </table>
</div>
             <div class="row" style="width: 90%;  margin-left: 10px">
                <div id="serialNoDetialsContainer" style="overflow: auto">
                  
                </div>
            <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" Visible="false"  OnClick="btnExport_Click"/>
            </div>


            <div class="modal fade" id="modalpopup" role="dialog" >
        <div class="modal-dialog modal-dialog-centered" style="width: 70vw; height: 90vh">
            <div class="modal-content" style="border: 2px solid #2E6886; border-radius: 10px; height: 100%">
                <%--<div class="modal-header" style="background-color: #5D7B9D; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation?</h4>
                </div>--%>
                <div class="modal-body" style="height: 92%"> 
                     <div class="navbar-collapse collapse">
                        <ul class="nav navbar-nav nextPrevious">
                            <li class="active"><a runat="server" id="alarm" class="menuData" data-toggle="tab" href="#menu0">Alarm</a> </li>
                            <li><a runat="server" class="menuData" id="measurement" data-toggle="tab" href="#menu1">Measurement</a></li>
                            <li><a runat="server" class="menuData" id="inspection" data-toggle="tab" href="#menu2">Inspection</a></li>
                        </ul>
                    </div>
                    <div class="tab-content themetoggle" id="inputContainer" style="overflow: auto; height: 93%; margin-top: 20px; background-color: #f5f5f5">
                        <div id="menu0" class="tab-pane fade">
                           <div id="alramContainer" class="popupTable">

                           </div>
                        </div>
                        <div id="menu1" class="tab-pane fade" >
                           <div id="measurementContainer" class="popupTable">

                           </div>
                        </div>
                        <div id="menu2" class="tab-pane fade">
                           <div id="inspectionContainer" class="popupTable"  style="overflow:auto;">

                           </div>
                             <div id="MPIContainer" class="popupTable" style="overflow:auto;margin-top:20px;"> 

                           </div>
                        </div>

                    </div>
                </div>
                 <div class="modal-footer" style="padding: 5px; border-top: 1px solid #2E6886;">
                    <input type="button" value="Close" class="btn btn-info" id="removePopup"  style="background-color: #2E6886; color: white" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
   
   
    <script type="text/javascript">
        $(document).ready(function () {
            $('[id$=txtDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            var Height = $(window).height() - (180);
            $('#serialNoDetialsContainer').css('height', Height);
              BindGroupIDs();
            BindSerialNumberDetails('Date');
            $.unblockUI({});
        });
        var menu = "#menu0";
        $(".menuData").click(function () {
            $(".menuData").css("background-color", "");
            $(".menuData").css("color", "#2E6886");
            $(this).css("background-color", "#2E6886");
            $(this).css("color", "#FFFFFF");
            menu = $(this).attr('href');
             $(menu).addClass("in active");
             $("#menu0").removeClass("in active");
           
           
        });

         $(window).resize(function () {
             var Height = $(window).height() - (180);
            $('#serialNoDetialsContainer').css('height', Height);
        });
        function BindGroupIDs() {
            let plantId = $("#ddlLineId").val();
            $.ajax({
                async: false,
                type: "POST",
                url: "SerialNumberDashboard.aspx/getGroupIDs",
                contentType: "application/json; charset=utf-8",
                data: '{plantId:"' + plantId + '"}',
                datatype: "json",
                // data: "date:'" + date + "'",
                success: function (response) {
                    var itemdata = response.d;
                    $("#ddlGroupId").empty();
                    for (let i = 0; i < itemdata.length; i++) {
                        $("#ddlGroupId").append($("<option></option>").val(itemdata[i]).html(itemdata[i]));
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        alert('Error: ' + err);
                }
            });


        }
        function BindSerialNumberDetails(param) {
            debugger;
            let input = "";
            if (param == 'Date') {
                input = $("#txtDate").val();
                $("#txtSerialNumber").val("");
            } else {
                input = $("#txtSerialNumber").val();
                let currentDate = new Date();
                $("#txtDate").val(currentDate.format("dd-MM-yyyy"));
            }
           
            let plantId = $("#ddlLineId").val();
            let groupId = $("#ddlGroupId").val();
            if (groupId == "" || groupId == null) {
                alert("Group Id required");
                return;
            } 
            $.ajax({
                async: false,
                type: "POST",
                url: "SerialNumberDashboard.aspx/getSerialNumberDetails",
                contentType: "application/json; charset=utf-8",
                data: '{input:"' + input + '", plantId:"' + plantId + '", param:"' + param + '", groupId:"'+ groupId +'"}',
                datatype: "json",
                // data: "date:'" + date + "'",
                success: function (response) {
                    var itemdata = response.d;
                    debugger;
                    $('#serialNoDetialsContainer').empty();
                    console.log(itemdata);
                    if (itemdata.length > 0) {
                        var appendStr = '<table id="tblSerialNoDetails" class="table table-bordered">';
                        for (var i = 0; i < itemdata.length; i++) {
                            if (i > 1) {
                                appendStr += '<tr><td>' + itemdata[i].SerialNumber + '<br/>  <a onclick="exportTheSlnoData(this)" slno="' + itemdata[i].SerialNumber + '" compid="' + itemdata[i].ComponentID + '">Export</a></td><td>' + itemdata[i].ComponentID + '</td>';
                            }
                            else {
                                appendStr += '<tr><td>' + itemdata[i].SerialNumber + '</td><td>' + itemdata[i].ComponentID + '</td>';
                            }
                            console.log(itemdata[i].OperatioList);
                            for (let j = 0; j < itemdata[i].OperatioList.length; j++) {
                                if (i == 0) {
                                    appendStr += '<td colspan="3" style="text-align: center">' + itemdata[i].OperatioList[j].OperationName + '</td>';
                                }
                                //else if (i == 1) {
                                //    appendStr += '<td style="width: 100px">' + itemdata[i].OperatioList[j].Machine + '</td><td style="width: 100px">' + itemdata[i].OperatioList[j].Operator + '</td><td style="width: 100px">' + itemdata[i].OperatioList[j].StartTime + '</td>';
                                //}

                                else if (i == 1) {
                                    appendStr += '<td>' + itemdata[i].OperatioList[j].Machine + '</td><td style="width: 100px">' + itemdata[i].OperatioList[j].Operator + '</td><td style="width: 100px">' + itemdata[i].OperatioList[j].StartTime + '</td>';
                                }
                                else {
                                    // alert(j + ";" + itemdata[i].OperatioList.length+";"+ itemdata[i].OperatioList[j].OperationName);
                                    if (itemdata[i].OperatioList[j].Machine == "Bypassed") {
                                        appendStr += '<td colspan="3">' + itemdata[i].OperatioList[j].Machine + '</td>';
                                    }
                                    else {
                                        appendStr += '<td>' + itemdata[i].OperatioList[j].Machine + '</td><td>' + itemdata[i].OperatioList[j].Operator + '</td><td class="hdnValuesField"><a onclick="return showPopup(this)" class="changeCusrsor">' + itemdata[i].OperatioList[j].StartTime + '</a><input type="hidden" id="hdendtime" value="' + itemdata[i].OperatioList[j].EndTime + '" /><input type="hidden" id="hdSlNo" value="' + itemdata[i].SerialNumber + '" /><input type="hidden" id="hdOperationName" value="' + itemdata[i].OperatioList[j].OperationName + '" /><input type="hidden" id="hdComponentId" value="' + itemdata[i].OperatioList[j].ComponentID + '" /><input type="hidden" id="hdmachineName" value="' + itemdata[i].OperatioList[j].Machine + '" /><input type="hidden" id="hdPlantId" value="' + itemdata[i].plantId + '" /><input type="hidden" id="hdGroupId" value="' + itemdata[i].groupid + '" /><input type="hidden" id="hdnOpWithDesc" value="'+itemdata[0].OperatioList[j].OperationName+'" /></td>';
                                    }
                                    
                                }
                            }
                            let totaltime = "";
                            if (itemdata[i].TotalTime != null) {
                                totaltime= itemdata[i].TotalTime;
                            }
                            let elapsedtime = "";
                            if (itemdata[i].ElapsedTime != null) {
                                elapsedtime = itemdata[i].ElapsedTime;
                            }
                            let runtime = "";
                            if (itemdata[i].RunTime != null) {
                                runtime = itemdata[i].RunTime;
                            }
                            appendStr += '<td>' +totaltime + '</td>';
                            appendStr += '<td>' + elapsedtime+ '</td>';
                            appendStr += '<td>' +  runtime+ '</td></tr>';
                        }
                        appendStr += "</table>";
                        $('#serialNoDetialsContainer').append(appendStr);
                    }
                    else {
                        let appendStr = '<div style="text-align:center;"><span style="font-size: 25px; font-weight: bold; color: white">No Data Found</span></div>';
                        $('#serialNoDetialsContainer').append(appendStr);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        alert('Error: ' + err);
                }
            });
            return false;
        }
        function exportTheSlnoData(evt) {
            debugger;
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            let slno = $(evt).attr("slno");
            let compID = $(evt).attr("compid");
            $('#hdnSlno').val(slno);
            $('#hdnCompID').val(compID);
            $('#hdnPlant').val($("#ddlLineId").val());
            $('#hdnGroupID').val($("#ddlGroupId").val());
            __doPostBack('<%= btnExport.UniqueID%>', '');
            $.unblockUI({});
           <%-- var tdList = $(evt).closest('tr').find('.hdnValuesField');
            if (tdList.length > 0) {
                for (var i = 0; i < tdList.length; i++) {
                    var td = $(evt).closest('tr').find('.hdnValuesField')[i];
                    var endTime = $(td).find('#hdendtime').val();
                    if (endTime.trim() != "") {
                        var startTime = $(td).find('a').text();
                        var endTime = $(td).find('#hdendtime').val();
                    }
                }
                __doPostBack('<%= btnExport.UniqueID%>', '');
            }--%>

          
        }
        function showPopup(val) {
            debugger;
             $(menu).removeClass("in active");
            $("#menu0").addClass("in active");
            $(".menuData").css("background-color", "");
            $(".menuData").css("color", "#2E6886");
             $("a[href$='#menu0']").css("background-color", "#2E6886");
            $("a[href$='#menu0']").css("color", "#FFFFFF");
            let starttime = val.innerText;
            let endtime = $(val).closest('td').find("#hdendtime").val();
            let machine = $(val).closest('td').find("#hdmachineName").val();
            let operationname =  $(val).closest('td').find("#hdOperationName").val();
            let slno =  $(val).closest('td').find("#hdSlNo").val();
            let componentId = $(val).closest('td').find("#hdComponentId").val();
            let groupid = $(val).closest('td').find("#hdGroupId").val();
            let plantid = $(val).closest('td').find("#hdPlantId").val();
            let opWithDesc = $(val).closest('td').find("#hdnOpWithDesc").val().trim().toLowerCase();
            //If change here, change in export also
            $.ajax({
                async: false,
                type: "POST",
                url: "SerialNumberDashboard.aspx/getAlarmDetails",
                contentType: "application/json; charset=utf-8",
                data: '{starttime:"' + starttime + '", endtime:"' + endtime + '", machine:"' + machine+'", operationname:"'+operationname +'", slno:"'+slno +'", componentId:"'+componentId+'"}',
                datatype: "json",
                // data: "date:'" + date + "'",
                success: function (response) {
                    var itemdata = response.d;
                    debugger;
                    $('#alramContainer').empty();
                    console.log(itemdata);
                    if (itemdata.length > 0) {
                        var appendStr = '<table id="tblAlarmDetails" class="table table-bordered">';
                        for (var i = 0; i < itemdata.length; i++) {
                            if (i == 0) {
                                appendStr += '<tr><th>Machine ID</th><th>Alarm No</th><th>Description</th><th>Alarm Start Time</th></tr>';
                            }
                            appendStr += '<tr><td>' + itemdata[i].MachineID + '</td><td>' + itemdata[i].AlarmNo + '</td><td>' + itemdata[i].Desciption + '</td><td>' + itemdata[i].AlarmStartTime + '</td></tr>';

                        }
                        appendStr += "</table>";
                        $('#alramContainer').append(appendStr);
                    } else {
                        let appendStr = ' <span style="font-size: 20px; font-weight: bold; padding: 10px;">No Data Found</span>';
                         $('#alramContainer').append(appendStr);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        alert('Error: ' + err);
                }
            });

             $.ajax({
                async: false,
                type: "POST",
                url: "SerialNumberDashboard.aspx/getMeasurementDetails",
                contentType: "application/json; charset=utf-8",
                data: '{starttime:"' + starttime + '", endtime:"' + endtime + '", machine:"' + machine+'", operationname:"'+operationname +'", slno:"'+slno +'", componentId:"'+componentId+'"}',
                datatype: "json",
                // data: "date:'" + date + "'",
                success: function (response) {
                    var itemdata = response.d;
                    debugger;
                    $('#measurementContainer').empty();
                    console.log(itemdata);
                    if (itemdata.length > 0) {
                        var appendStr = '<table id="tblMeasurementDetails" class="table table-bordered">';
                        if (machine == "Leak Test Machine") {
                            for (var i = 0; i < itemdata.length; i++) {
                                if (i == 0) {
                                    appendStr += '<tr><th>Result</th><th>Remark</th></tr>';
                                }
                                appendStr += '<tr><td>' + itemdata[i].Result + '</td><td>' + itemdata[i].LeakTestRemarks + '</td></tr>';

                            }
                        }
                        else if (machine == "Laser Marking Machine Phantom" || machine == "Laser Marking Machine Compact x")
                        {
                            for (var i = 0; i < itemdata.length; i++) {
                                if (i == 0) {
                                    appendStr += '<tr><th>Scanned Data</th><th>Marking Data</th><th>Marking Status</th><th>Status</th></tr>';
                                }
                                appendStr += '<tr><td>' + itemdata[i].ScannedData + '</td><td>' + itemdata[i].MarkingData + '</td><td>' + itemdata[i].MarkingStatus + '</td><td>' + itemdata[i].Status + '</td></tr>';

                            }
                        }
                        else {
                            for (var i = 0; i < itemdata.length; i++) {
                                if (i == 0) {
                                    appendStr += '<tr><th>Component ID</th><th>Characteristic ID</th><th>Characteristic Code</th><th>LSL</th><th>USL</th><th>Unit</th><th>Value</th><th>Time Stamp</th><th>Specification Mean</th>';
                                    if (machine == "QA Gate") {
                                        appendStr += '<th>Remarks</th></tr>';
                                    }
                                    else {
                                        appendStr += '</tr>';
                                    }
                                }
                                appendStr += '<tr><td>' + itemdata[i].ComponentID + '</td><td>' + itemdata[i].CharacteristicID + '</td><td>' + itemdata[i].CharecteristicCode + '</td><td>' + itemdata[i].LSL + '</td><td>' + itemdata[i].USL + '</td><td>' + itemdata[i].Unit + '</td><td>' + itemdata[i].Value + '</td><td>' + itemdata[i].TimeStamp + '</td><td>' + itemdata[i].SpecificationMean + '</td>';
                                if (machine == "QA Gate") {
                                    appendStr += '<td>' + itemdata[i].Remarks + '</td></tr>';
                                }
                                else {
                                    appendStr += '</tr>';
                                }
                            }
                        }
                        appendStr += "</table>";
                        $('#measurementContainer').append(appendStr);
                    }  else {
                        let appendStr = ' <span style="font-size: 20px;font-weight: bold; padding: 10px;">No Data Found</span>';
                         $('#measurementContainer').append(appendStr);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        alert('Error: ' + err);
                }
            });

            $.ajax({
                async: false,
                type: "POST",
                url: "SerialNumberDashboard.aspx/getInspectionDetails",
                contentType: "application/json; charset=utf-8",
                data: '{starttime:"' + starttime + '", endtime:"' + endtime + '", machine:"' + machine + '", operationname:"' + operationname + '", slno:"' + slno + '", componentId:"' + componentId + '",plantid:"'+plantid+'",groupid:"'+groupid+'"}',
                datatype: "json",
                // data: "date:'" + date + "'",
                success: function (response) {
                    var itemdata = response.d;
                    debugger;
                    $('#inspectionContainer').empty();
                    console.log(itemdata);
                    if (itemdata.length > 0) {
                        if (operationname == "90") {
                            var appendStr = '<table id="tblInspectionDetails" class="table table-bordered">';
                            //for (var i = 0; i < itemdata.length; i++) {
                            //    if (i == 0) {
                            //        appendStr += '<tr><th>Serial Number</th><th>Component ID</th><th>Dimensional OK OPN ' + operationname + '</th><th>Visual OK OPN ' + operationname + '</th><th>Inspection Date</th><th>Status</th><th>Remarks</th><th>Checked By</th></tr>';
                            //    }
                            //    appendStr += '<tr><td>' + itemdata[i].SlNo + '</td><td>' + itemdata[i].ComponentID + '</td>';
                            //    if (itemdata[i].DimentionalStatus == "Green") {
                            //        appendStr += '<td><i class="glyphicon glyphicon-ok-circle" style="color:green;font-size:25px"></i></td>';
                            //    } else {
                            //        appendStr += '<td> <i class="glyphicon glyphicon-remove-circle" style="color:red;font-size:25px"></i></td>';
                            //    }
                            //    if (itemdata[i].Value == "Green") {
                            //        appendStr += '<td><i class="glyphicon glyphicon-ok-circle" style="color:green;font-size:25px"></i></td>';
                            //    } else {
                            //        appendStr += '<td> <i class="glyphicon glyphicon-remove-circle" style="color:red;font-size:25px"></i></td>';
                            //    }
                            //    appendStr += '<td> ' + itemdata[i].InspectionDate + '</td > <td>' + itemdata[i].Status + '</td> <td>' + itemdata[i].Status + '</td> <td>' + itemdata[i].ChekedBy + '</td></tr > ';
                            //}
                            for (var i = 0; i < itemdata.length; i++) {
                                debugger;
                                if (i == 0) {
                                    appendStr += '<tr><th>Serial Number</th><th>Component ID</th><th>Status</th></tr>';
                                }
                                appendStr += '<tr><td>' + itemdata[i].SlNo + '</td><td>' + itemdata[i].ComponentID + '</td>';
                                if (itemdata[i].Status == "Rejected") {
                                    appendStr += '<td> <i class="glyphicon glyphicon-remove-circle" style="color:red;font-size:25px"></i></td></tr >';
                                } else  {
                                    appendStr += '<td><i class="glyphicon glyphicon-ok-circle" style="color:green;font-size:25px"></i></td></tr >';
                                }
                            }
                            appendStr += "</table>";
                            $('#inspectionContainer').append(appendStr);

                        } else {
                            var appendStr = '<table id="tblInspectionDetails" class="table table-bordered">';
                            for (var i = 0; i < itemdata.length; i++) {
                                if (i == 0) {
                                    appendStr += '<tr><th>Serial Number</th><th>Component ID</th><th>Dimensional OK OPN ' + operationname + '</th><th>Visual OK OPN ' + operationname + '</th></tr>';
                                }
                                appendStr += '<tr><td>' + itemdata[i].SlNo + '</td><td>' + itemdata[i].ComponentID + '</td>';
                                 if (itemdata[i].DimentionalStatus == "Green") {
                                    appendStr += '<td><i class="glyphicon glyphicon-ok-circle" style="color:green;font-size:25px"></i></td>';
                                } else {
                                    appendStr += '<td> <i class="glyphicon glyphicon-remove-circle" style="color:red;font-size:25px"></i></td>';
                                }
                                if (itemdata[i].Value == "Green") {
                                    appendStr += '<td><i class="glyphicon glyphicon-ok-circle" style="color:green;font-size:25px"></i></td>';
                                } else {
                                    appendStr += '<td> <i class="glyphicon glyphicon-remove-circle" style="color:red;font-size:25px"></i></td>';
                                }
                                //appendStr += '<td> ' + itemdata[i].InspectionDate + '</td > <td>' + itemdata[i].Status + '</td> <td>' + itemdata[i].Remarks + '</td> <td>' + itemdata[i].ChekedBy + '</td></tr > ';
                            }
                            appendStr += "</table>";
                            $('#inspectionContainer').append(appendStr);
                        }
                    } else {
                        let appendStr = ' <span style="font-size: 20px;font-weight: bold; padding: 10px;">No Data Found</span>';
                        $('#inspectionContainer').append(appendStr);
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    console.log(err);
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        alert('Error: ' + err);
                }
            });
            if (opWithDesc.includes("crack detection")) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "SerialNumberDashboard.aspx/getMPIDetails",
                    contentType: "application/json; charset=utf-8",
                    data: '{starttime:"' + starttime + '", endtime:"' + endtime + '", machine:"' + machine + '", operationname:"' + operationname + '", slno:"' + slno + '", componentId:"' + componentId + '"}',
                    datatype: "json",
                    // data: "date:'" + date + "'",
                    success: function (response) {
                        var itemdata = response.d;
                        debugger;
                        $('#MPIContainer').empty();
                        console.log(itemdata);
                        if (itemdata.length > 0) {
                            var appendStr = '<table id="tblMPIDetails" class="table table-bordered">';
                            for (var i = 0; i < itemdata.length; i++) {
                                if (i == 0) {
                                    appendStr += "<tr><th>Manual Insp/Result</th><th>Camera Insp/Result</th><th>Camera Pic's Link</th><th>De-Mag Level</th><th>Remarks</th><th>Visual Insp/Result</th></tr>";
                                }
                                appendStr += '<tr><td>' + itemdata[i].ManualInsResult + '</td><td>' + itemdata[i].CameraInsResult + '</td><td>' + itemdata[i].CameraPicLink + '</td><td>' + itemdata[i].DeMagLevel + '</td><td>' + itemdata[i].MPIRemark + '</td><td>' + itemdata[i].VisualInsResult + '</td></tr>';

                            }
                            appendStr += "</table>";
                            $('#MPIContainer').append(appendStr);



                        } else {
                            let appendStr = ' <span style="font-size: 20px;font-weight: bold; padding: 10px;">No Data Found</span>'; //kkkk
                            $('#MPIContainer').append(appendStr);
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                        console.log(err);
                        alert('Error: ' + err);
                        if (jqXHR.status == 401)
                            alert('Error: ' + err);
                    }
                });
                $('#MPIContainer').css("height", "32vh");
                $('#inspectionContainer').css("height", "32vh");
                $('#MPIContainer').css("display", "block");
            }
            else {
                $('#inspectionContainer').css("height", "66vh");
                $('#MPIContainer').css("display", "none");
            }
            $('[id*=modalpopup]').modal('show');
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
