<%@ Page Language="C#" Title="Weekly Checklist Master" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="WeeklyChecklistMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.WeeklyChecklistMaster" %>

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

                            <td class="commanTd" style="width: 60px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","Year") %></td>
                            <td style="width: 140px;">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" meta:resourcekey="ddlYearResource1" AutoPostBack="false" />
                            </td>

                            <td class="commanTd" style="width: 80px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","Freq") %></td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="form-control" meta:resourcekey="ddlFreqResource1" AutoPostBack="false" />
                            </td>

                            <td style="width: 130px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" Style="min-width: 120px;" OnClick="btnView_Click" />
                            </td>
                            <td></td>
                             <%--<td style="width:240px">
                                 <asp:DropDownList runat="server" ID="ddlSortOrder" CssClass="form-control" OnSelectedIndexChanged="ddlSortOrder_SelectedIndexChanged" AutoPostBack="true">
                                     <asp:ListItem Text="Sort By Activity ID"  Value="ActivityID"/>
                                     <asp:ListItem Text="Sort By Checklist Points" Value="ChecklistPoints"/>
                                 </asp:DropDownList>
                             </td>--%>
                        </tr>
                    </table>
                </div>

                <div style="overflow-x: auto; overflow-y: auto;height:80vh">
                    <asp:GridView ID="GridWeeklyMaintChklist" runat="server" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="false" OnDataBound="GridWeeklyMaintChklist_DataBound" AutoGenerateColumns="false">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected plant, line and date time period.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>

                <div class="DataOperations">
                    <button type="button" id="btnGenerateActivity" class="btn btn-primary">Generate Activity For Frequency</button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!--Schedule Week Modal Popup -->
    <div id="WeeklyMaintenancePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <h6>Select the week number to change the schedule to another week.</h6>
                    <asp:Label runat="server" ID="lblStatus" Font-Bold="true" ClientIDMode="Static" />
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Week No. : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:Label runat="server" ID="lblWeek" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Check Point : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:Label runat="server" ID="lblActivity" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Machine ID : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:Label runat="server" ID="lblMachine" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Week : " Font-Bold="true" Style="margin-left: 5px;" />
                            <asp:DropDownList runat="server" ID="ddlWeek" CssClass="form-control" ClientIDMode="Static" Style="width: 200px; margin: 5px; display: inline;" />
                        </div>
                        <asp:HiddenField runat="server" ID="hdfMonthName" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdfOldWeekNo" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdfActId" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdfActivity" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdfFreqId" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdfYear" ClientIDMode="Static" />
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Generate Activity Modal Popup -->
    <div id="GenerateActivityPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnOK" CssClass="btn btn-primary" Text="OK" OnClick="btnOK_Click" />
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).on("dblclick", "tr .WeekScheduler", function (e) {
            debugger;
            if ($(this).context.text == "O") {
                var freqId = $(this).attr('FreqID');
                var frequency = $(this).attr('Frequency');
                if (frequency !== null && frequency !== "Weekly") {
                    var macId = $(this).attr('MachineID');
                    var year = $(this).attr('Year');
                    var actId = $(this).attr('ActivityID');
                    var chkPoints = $(this).attr('CheckPoints');
                    var weekNo = $(this).attr('WeekNo');
                    $("#lblActivity").text(chkPoints);
                    $("#lblMachine").text(macId);
                    if (weekNo !== null && weekNo !== undefined && weekNo.indexOf("-") !== -1) {
                        var MonthName = weekNo.split('-')[0];
                        $("#hdfMonthName").val(MonthName);
                        weekNo = weekNo.split('-')[1];
                        $("#hdfOldWeekNo").val(weekNo);
                        $("#ddlWeek").prop('selectedIndex', parseInt(weekNo.slice(4)) - 1);
                    }
                    $("#lblWeek").text(weekNo + " (" + year + ")");
                    $("#hdfActId").val(actId);
                    $("#hdfActivity").val(chkPoints);
                    $("#hdfFreqId").val(freqId);
                    $("#hdfYear").val(year);
                    $("#lblStatus").text("");
                    var title = "Schedule Inspection";
                    $("#WeeklyMaintenancePopup .modal-title").html(title);
                    $("#WeeklyMaintenancePopup").modal("show");
                }
            }
        });

        $("#btnGenerateActivity").click(function () {
            if ($("#MainContent_GridWeeklyMaintChklist tbody tr:first").find("td:first")[0] == undefined) {
                var title = "Generate Activity For Frequency";
                var body = "Are you sure you want to generate activity for frequencies ?";
                $("#GenerateActivityPopup .modal-title").html(title);
                $("#GenerateActivityPopup .modal-body").html(body);
                $("#GenerateActivityPopup").modal("show");
            }
            else {
                alert("Can't generate activity for empty data.");
            }
        });

        function PopupCenter(url, title, w, h) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            if (window.focus) {
                newWindow.focus();
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#btnGenerateActivity").click(function () {
                if ($("#MainContent_GridWeeklyMaintChklist tbody tr:first").find("td:first")[0] == undefined) {
                    var title = "Generate Activity For Frequency";
                    var body = "Are you sure you want to generate activity for frequencies ?";
                    $("#GenerateActivityPopup .modal-title").html(title);
                    $("#GenerateActivityPopup .modal-body").html(body);
                    $("#GenerateActivityPopup").modal("show");
                }
                else {
                    alert("Can't generate activity for empty data.");
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
