<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShiftAllocationDetails.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.ShiftAllocationDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
      <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>

    <div>
         <asp:UpdatePanel runat="server">
            <ContentTemplate>
                 <div class="ui segment">
                    <table style="display: inline-block;">
                        <tr>
                            <td style="text-align: center; vertical-align: central; align-content: center;width:80px; color: black">Plant-ID :
                            </td>
                            <td style="width:150px">
                                <asp:DropDownList runat="server" ID="cmbPlantID" ClientIDMode="Static" CssClass="form-control" Width="140px" />
                            </td>
                            <td style="text-align: center; vertical-align: central; align-content: center;width:60px;  color: black">Date :
                            </td>
                            <td class="input-group" style="width:180px">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="150" CssClass="form-control date" data-toggle="tooltip" ClientIDMode="Static"
                                    title="Date !" placeholder="Date"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="View"  CssClass="ui violet button" Text="View" Width="80" OnClientClick="return GetShiftAllocationData();" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="Save"  CssClass="ui violet button" Text="Save" Width="80" OnClientClick="return saveDetails();" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="cmbOperator" CssClass="form-control ddlop" ToolTip="Operator" Width="100" ClientIDMode="Static"/>
                              
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnPrev"  CssClass="ui violet button" Text="Prev" Width="80" OnClientClick="return btnPrev_Click();" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnNext"  CssClass="ui violet button" Text="Next" Width="80" OnClientClick="return btnNext_Click();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="overflow: auto">
                    <asp:HiddenField runat="server" ID="hidPlantID" ClientIDMode="Static" />
                       <table id="ShiftAllocationTable" class="table table-bordered  tbl tblSetting headerdiv"></table>
                </div>
            </ContentTemplate>
         </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
           
            GetShiftAllocationData();
        });

        function btnPrev_Click() {

            var FromDate = $("#txtFromDate").val();
            var Operator = $('#cmbOperator option:selected').val();
            
            $.ajax({
                async: true,
                type: "POST",
                url: "ShiftAllocationDetails.aspx/PreviousClick",
                contentType: "application/json; charset=utf-8",
                data: '{FromDate:"' + FromDate + '",Operator:"' + Operator + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    debugger;
                    $('#txtFromDate').val(itmData);
                    GetShiftAllocationData();
                },
                error: function (itmData) {
                    alert("Error" + Result.responseText);
                }
            });

            return false;
        }

        function btnNext_Click() {
            var FromDate = $("#txtFromDate").val();
            var Operator = $('#cmbOperator option:selected').val();
            $.ajax({
                async: true,
                type: "POST",
                url: "ShiftAllocationDetails.aspx/NextClick",
                contentType: "application/json; charset=utf-8",
                data: '{FromDate:"' + FromDate + '",Operator:"' + Operator + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    $("#txtFromDate").val(itmData);
                    GetShiftAllocationData();
                },
                error: function (itmData) {
                    alert("Error" + Result.responseText);
                }
            });

            return false;
        }

        function DeleteAlertShiftAllocation(fromdate, todate, consumer) {
            $.ajax({
                async: false,
                type: "POST",
                url: "ShiftAllocationDetails.aspx/DeleteShiftAllocation",
                contentType: "application/json; charset=utf-8",
                data: '{FromDate:"' + fromdate + '",ToDate:"' + todate + '",Consumer:"' + consumer + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                },
                error: function (itmData) {
                    alert("Error" + Result.responseText);
                }
            });
        }

        function saveDetails() {
            debugger;
            var firstrowlen = "";
            var FromDate = "";
            var ToDate = "";
            var rows = $("#ShiftAllocationTable tr");
            for (var i = 0; i < rows.length; i++) {
                var row = $(rows)[i];
                if (i == 0) {
                    firstrowlen = $(row).find(".mainClass").length;
                    FromDate = $(row).find(".mainClass").eq(0).closest('th').find(".hdnDate").val();
                    ToDate = $(row).find(".mainClass").eq(firstrowlen - 1).closest('th').find(".hdnDate").val();
                }
                else {
               
                var Consumer = $(row).find("#lblConsumer").text();
                if (Consumer != "") {
                    DeleteAlertShiftAllocation(FromDate, ToDate, Consumer);
                }   
                for (let j = 0; j < $(row).find(".mainClass").length; j++) {
                    var dateCol = $(row).find(".mainClass").eq(j).find('td').length;
                    for (let k = 0; k < dateCol; k++) {
                        var tdcol = $(row).find(".mainClass").eq(j).find('td').eq(k);
                        if ($(tdcol).find('#ChkVal').prop("checked")) {
                            var ShiftId = $(tdcol).find('.hdnShift1').val();
                            var ShiftDate =$(tdcol).find('.hdnShift2').val();
                            var ShiftData = {};
                            ShiftData.Consumer = Consumer;
                            ShiftData.ShiftId = ShiftId;
                            ShiftData.ShiftDate = ShiftDate;
                            $.ajax({
                                async: false,
                                type: "POST",
                                url: "ShiftAllocationDetails.aspx/saveDetails",
                                contentType: "application/json; charset=utf-8",
                                //data: '{FromDate:"' + FromDate + '",ToDate:"' + ToDate + '",Consumer:"' + Consumer + '",ShiftID"' + ShiftID + '"}',
                                data: JSON.stringify({
                                    Data: ShiftData
                                }),
                                dataType: "json",
                                success: function (response) {
                                    var itmData = response.d;
                                     
                                },
                                error: function (Result) {
                                    alert("Error" + Result.responseText);
                                }
                            });
                        }
                    }
                }
                }
            }
            alert("Data saved");
            GetShiftAllocationData();
           
            return false;
        }


        function GetShiftAllocationData()
        {
            debugger;
            var FromDate = $("#txtFromDate").val();
            var PlantID = $('#cmbPlantID').val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ShiftAllocationDetails.aspx/GetData",
                data: '{PlantID:"' + PlantID + '",fromDate:"' +FromDate+'"}',
                dataType: "json",
                success: function (result) {
                    data = result.d;
                    $("#ShiftAllocationTable").empty();
                    var body = '<thead><tr class="shiftAllocationrow" style="text-align:center;background-color:#2E6886;color:white">';
                    for (let i = 0; i < data.length; i++) {
                        if (i == 0) {
                            body += '<th style="text-align:center;width:50px;padding-bottom: 23px;">SLNO</th><th style="text-align: center;padding-bottom: 23px;width:100px;">Consumer</th><th style="text-align: center;padding-bottom: 23px;width:250px;">EmailId</th><th style="text-align: center;width:100px;    padding-bottom: 23px;">PhoneNo</th>';
                            for (let j = 0; j < data[i].DateShift.length; j++) {
                                body += '<th style="text-align:center;width:50px;">' + data[i].DateShift[j].Date + '<input type="hidden" class="hdnDate" value="' + data[i].DateShift[j].Date + '" /></br>';
                                body += '<table style="width:100%" class="mainClass"><tr>';
                                for (let k = 0; k < data[i].DateShift[j].Shift.length; k++) {
                                    body += '<td style="color:white;">' + data[i].DateShift[j].Shift[k] + '</td><input type="hidden" class="hdnShift" value="' + data[i].DateShift[j].Shift[k] + '" />';
                                }
                                body += '</tr></table></th>';
                            }

                        }

                        body += "</tr></thead><tbody><tr>";
                        if (i > 0) {
                            body += '<td><label id="lblSlno" style="color:white">' + data[i].SLNO + '</td><td><label id="lblConsumer" style="color:white">' + data[i].Consumer + '</td><td><label id="lblEmailID" style="color:white">' + data[i].EmailId + '</td><td><label id="lblPhoneNo" style="color:white">' + data[i].PhoneNo + '</td>';
                            for (let k = 0; k < data[i].DateShift.length; k++) {
                                body += '<td style="padding:0px;"><table style="width:100%;" class="mainClass"><tr>';
                                for (let l = 0; l < data[i].DateShift[k].Checked.length; l++) {
                                    if (data[i].DateShift[k].Checked[l] == 1) {
                                        body += '<td style="color:white;"><input style="margin:10px;" type="checkbox" id="ChkVal" checked="checked" /><input type="hidden" class="hdnShift1" value="' + data[i].DateShift[k].Shift[l] + '" /><input type="hidden" class="hdnShift2" value="' + data[i].DateShift[k].Date + '" /></td>';
                                    }
                                    else {
                                        body += '<td style="color:white;"><input  style="margin:10px;" id="ChkVal" type="checkbox"  /><input type="hidden" class="hdnShift1" value="' + data[i].DateShift[k].Shift[l] + '" /><input type="hidden" class="hdnShift2" value="' + data[i].DateShift[k].Date + '" /></td>';
                                    }

                                }
                                body += '</tr></table></td>';
                            }
                        }
                        body += "</tr>"
                    }

                    body += "</tbody>";
                    $("#ShiftAllocationTable").append(body);
                },
                error: function (Result) {
                    alert("Error" + Result.responseText);
                }
            });
            return false;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            GetShiftAllocationData();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
