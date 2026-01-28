<%@ Page Language="C#" Title="Weekly Checklist Transaction" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="WeeklyChecklistTransaction.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.WeeklyChecklistTransaction" %>

<asp:Content ID="MainContentArea" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .header-center {
            text-align: center;
        }

        table tr th {
            text-align: center !important;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }

        .DataOperations {
            bottom: 0;
            right: 0;
            float: right;
            margin-right: 5px;
            min-height: 40px;
            position: fixed;
        }

        .WeekScheduler {
            vertical-align: middle;
        }

        .InspectionStatus {
            vertical-align: middle;
        }

        .Row {
            display: table;
            border-spacing: 5pt;
            width: 100%;
        }

        .Col {
            display: table-cell;
            height: 50px;
            width: 100%;
            border: 1pt solid black;
            background-color: #DBDBDB;
        }

        .MiddleLeft {
            text-align: left;
            align-items: normal;
            vertical-align: middle;
        }

        .form-control {
            width: 98%;
        }
    </style>

    <div class="container-fluid" style="margin-left: 5px;">
        <asp:UpdatePanel ID="UpdatePanelMaintChklist" runat="server">
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row" style="height: 60px;">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="width: 80px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","LineID") %></td>
                            <td style="width: 160px;">
                                <asp:DropDownList ID="ddlLineID" runat="server" CssClass="form-control" meta:resourcekey="ddlLineIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlLineID_SelectedIndexChanged" />
                            </td>

                            <td class="commanTd" style="width: 100px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","MachineId") %></td>
                            <td style="width: 160px;">
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" meta:resourcekey="ddlMachineIdResource1" AutoPostBack="false" />
                            </td>

                            <td style="width: 130px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" Style="min-width: 120px;" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <div style="float:right;margin-top: 5px;">
                                    <img src="NotifyIcons/NotifyIcon_Red.png" alt="Not Done" />
                                    <span style="color: white; margin: 0px 5px;">Not Done</span>
                                    <img src="NotifyIcons/NotifyIcon_Green.gif" alt="Done" />
                                    <span style="color: white; margin: 0px 5px;">Done</span>
                                    <img src="NotifyIcons/NotifyIcon_Blue.png" alt="Check Done & Replaced" />
                                    <span style="color: white; margin: 0px 5px;">Check Done & Replaced</span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="overflow-x: auto; overflow-y: auto;height:70vh">
                    <asp:GridView ID="GridWeeklyMaintTransaction" runat="server" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="false" AutoGenerateColumns="false" OnDataBound="GridWeeklyMaintTransaction_DataBound">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected plant, line and date time period.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
                <div style="height:40px;float:right">
                    <asp:button runat="server" ID="btnMainainanceChecklistSave" CssClass="btn btn-warning" Text="Maintainance Engg Entry" OnClick="btnSaveWeekNumber"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Modal Popup -->
    <div id="InspectionPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Select the option to change inspection status.</h6>
                    <asp:Label runat="server" ID="lblStatus" Font-Bold="true" ClientIDMode="Static" />
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Activity : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:Label runat="server" ID="lblActivity" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Frequency : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:Label runat="server" ID="lblFrequency" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Inspection : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:DropDownList runat="server" ID="ddlInspection" CssClass="form-control" ClientIDMode="Static" Style="margin: 5px;">
                                <asp:ListItem Text="" Value="0" />
                                <asp:ListItem Text="Done" Value="1" />
                                <asp:ListItem Text="Not Done" Value="2" />
                                <asp:ListItem Text="Check Done & Replaced" Value="3" />
                            </asp:DropDownList>
                            <asp:HiddenField runat="server" ID="hdfFreqID" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdfActivityID" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="hdfWeekNo" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtRemarks">Remarks : </label>
                        <asp:TextBox ID="txtRemarks" ClientIDMode="Static" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

   
    <script type="text/javascript">
        Date.prototype.getWeek = function () {
            var date = new Date(this.getTime());
            date.setHours(0, 0, 0, 0);
            date.setDate(date.getDate() + 3 - (date.getDay() + 6) % 7);
            var week1 = new Date(date.getFullYear(), 0, 4);
            return 1 + Math.round(((date.getTime() - week1.getTime()) / 86400000 - 3 + (week1.getDay() + 6) % 7) / 7);
        }

        $(document).on("dblclick", "tr .InspectionStatus", function (e) {
            debugger;
            var d = new Date();
            var weekNo = $(this).attr('WeekNo');
            var curweekno = getweeknumber();
            var currentWeek = "Week" + curweekno;
            if (currentWeek == weekNo) {
                if ($(this).attr("title") == "Not Done" || $(this).attr("title") == "Done" || $(this).attr("title") == "Check Done & Replaced" || $(this).attr("title") == "Not Attempted") {
                    var macId = $(this).attr('MachineID');
                    var freq = $(this).attr('Frequency');
                    var freqId = $(this).attr('FreqID');
                    var actId = $(this).attr('ActivityID');
                    var chkPoints = $(this).attr('CheckPoints');
                    var insId = $(this).attr('insid');
                    $("#lblActivity").text(chkPoints);
                    $("#lblFrequency").text(freq);
                    $("#hdfFreqID").val(freqId);
                    $("#hdfActivityID").val(actId);
                    $("#hdfWeekNo").val(weekNo);
                    if (insId == "1" || insId == "2" || insId == "3") {
                        $("#ddlInspection").prop('selectedIndex', parseInt(insId));
                    }
                    else {
                        $("#ddlInspection").prop('selectedIndex', 0);
                    }
                    $("#lblStatus").text("");
                    $("#txtRemarks").text("");
                    //Getting Remarks - HTTP AJAX Call
                    var httpRequest = new XMLHttpRequest();
                    if (!httpRequest) {
                        alert('Error!! Cannot create an XMLHTTP instance.');
                        return;
                    }
                    httpRequest.onload = function () {
                        if (httpRequest.readyState === XMLHttpRequest.DONE) {
                            if (httpRequest.status === 200) {
                                var remark = JSON.parse(httpRequest.responseText).d;
                                if (remark) {
                                    $("#txtRemarks").text(remark);
                                }
                            }
                        }
                    };
                    httpRequest.onabort = function () {
                        alert('Request Aborted. Please try again.');
                    };
                    httpRequest.ontimeout = function () {
                        alert('Request Timeout. Please try again.');
                    };
                    httpRequest.onerror = function () {
                        alert('Error!!');
                    };
                    httpRequest.open('POST', 'WeeklyChecklistTransaction.aspx/GetChecklistRemarks', true);
                    httpRequest.timeout = 60000;
                    httpRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                    httpRequest.send('{machineID:"' + macId + '", freqId:"' + freqId + '", actId:"' + actId + '", weekNo:"' + currentWeek + '"}');
                    //----------------------------------
                    var title = "Inspection";
                    $("#InspectionPopup .modal-title").html(title);
                    $("#InspectionPopup").modal("show");
                }
            }
        });

        function getRemarks(macid, freqid, actid, weekno) {

        }

        function getweeknumber() {
            var res = "";
            $.ajax({
                async:false,
                type: "POST",
                url: "WeeklyChecklistTransaction.aspx/GetWeekDate",
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (response) {
                    res = response.d;
                },
                error: function (err) {
                    alert('Error : ' + err);
                }
            });
            return res;
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {

        });
    </script>
</asp:Content>

<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
