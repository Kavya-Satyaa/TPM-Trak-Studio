<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateSchedule_Old.aspx.cs" Inherits="Web_TPMTrakDashboard.CreateSchedule_Old" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <script src="../Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="../Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <style>
        .header {
            color: white;
        }

        fieldset {
            margin: 8px;
            border: 1px solid silver;
            padding: 8px;
            border-radius: 4px;
        }

        legend {
            text-align: left;
            color: white;
            display: block;
            width: 125px;
            padding-left: 4px;
            padding-top: 0px;
            padding-right: 0px;
            padding-bottom: 0px;
            margin-bottom: 20px;
            font-size: 20px;
            line-height: inherit;
            border-bottom: transparent;
        }

        .tbl1 {
            padding: 0px;
            width: auto;
            margin-left: 10px;
        }

        .emailSetting {
            height: auto;
            margin: 0 auto;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-direction: column;
        }

        .column {
            float: left;
            width: 50%;
            padding: 10px;
        }

        .multiselect-native-select {
            min-width: 550px;
        }

        #MainContent_gridScheduleRpt th {
            color: white;
        }

        #MainContent_gridScheduleRpt td {
            cursor: pointer;
        }

        .WrappedText {
            min-width: 240px;
            word-break: break-all;
            word-wrap: break-word;
        }

        .Hidedisplay {
            display: none;
        }

        .multiselect {
            min-width: 200px;
        }
    </style>
    <%--<div class="row text-center header">
		<h3>Export Schedule Report</h3>
	</div>--%>
    <div class="row text-center">
        <asp:Label ID="lblMessages" runat="server" EnableViewState="False" Style="font-weight: bold; font-family: Calibri; color: green;" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>
    <div style="overflow: auto">
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <div class="row text-center" style="margin-left: -10px;">

                    <ul class="nav nav-pills" style="margin-left: 10px;">
                        <li class="active"><a class="menuData" data-toggle="tab" href="#menu0">Create Schedule</a></li>
                        <li><a class="menuData EmailSettings" data-toggle="tab" href="#menu1">Email Settings</a></li>
                    </ul>

                    <div class="tab-content">
                        <div id="menu0" class="tab-pane fade in active">
                            <fieldset class="container" style="margin-right: 10px; margin-left: 10px; max-width: 95%">
                                <legend style="width: 175px;">Schedule Report</legend>
                                <div class="row">
                                    <div class="column">
                                        <fieldset>
                                            <div class="row">
                                                <div class="col">
                                                    <table class="table table-bordered tbl1">
                                                        <tr>
                                                            <td class="commontd">Report </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlReports" runat="server" AutoPostBack="true" CssClass="select form-control" OnSelectedIndexChanged="ddlReports_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="HidtemplateName" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="commontd">Export Type </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlExportTypes" runat="server" CssClass="select form-control">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="commontd">Export Name </td>
                                                            <td>
                                                                <asp:TextBox ID="txtExportName" runat="server" CssClass="form-control" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="commontd">Export Path </td>
                                                            <td>
                                                                <asp:TextBox ID="txtExportPath" runat="server" CssClass="form-control exportPath" autocomplete="off" data-toggle="tooltip" AutoCompleteType="Disabled"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="col">
                                                    <fieldset>
                                                        <legend>Parameters</legend>
                                                        <div class="col">
                                                            <table class="table table-bordered tbl1">
                                                                <tr>
                                                                    <td class="commontd">Plant ID </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="commontd">Line ID </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlLineID" OnSelectedIndexChanged="ddlLineID_SelectedIndexChanged" runat="server" AutoPostBack="true" CssClass="select form-control">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="commontd">Machine </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="select form-control" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="commontd">Operator </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlOperator" runat="server" CssClass="select form-control">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="commontd">Shift ID </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlShift" runat="server" CssClass="select form-control">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="commontd">Run Report On </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlRptOn" runat="server" CssClass="select form-control"></asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="commontd">Run Report For Every</td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlRptForEvery" runat="server" CssClass="select form-control"></asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>
                                            <div>
                                                <%--<asp:Button runat="server" Text="Save" class="btn btn-info btn-sm" ID="btnSave" OnClick="btnSave_Click"></asp:Button>--%>
                                                <input type="button" id="btnSave" value="Save" class="btn btn-info btn-sm" onclick="javascript: return SaveSchedule();" /><%----%>
                                                <asp:Button ID="btndelete" CssClass="btn btn-info btn-sm" runat="server" OnClick="btndelete_Click" Text="Delete" />
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="column">
                                        <div class="row">
                                            <table style="margin-left: 7px;">
                                                <tr>
                                                    <td class="commontd">Select Email : &nbsp;&nbsp;&nbsp;</td>
                                                    <td class="commontd">
                                                        <asp:CheckBox ID="ckhEnableEmail" runat="server" onclick="EmailEnableState()" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td class="commontd" style="display: none">
                                                        <asp:TextBox ID="txtSlNo" runat="server" CssClass="form-control exportPath" data-toggle="tooltip" AutoCompleteType="Disabled" Visible="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                            <fieldset>
                                                <legend>Parameters</legend>
                                                <div class="row commontd">
                                                    Select Email Id's : &nbsp;
												<asp:ListBox ID="ddlEmailIds" runat="server" SelectionMode="Multiple" CssClass="ddlEmailIdStyle" Style="min-width: 200px; height: 40px; overflow: auto"></asp:ListBox>
                                                </div>
                                                <br />
                                                <div class="row" style="display: inline-block">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="addTo" class="btn btn-info btn-sm" runat="server" value="Add To" onclick="AddToValues()" />
                                                            </td>
                                                            <td>
                                                                <asp:ListBox ID="lbAddTo" runat="server" Width="320" Height="60" SelectionMode="Multiple"></asp:ListBox>
                                                            </td>
                                                            <td>
                                                                <input type="button" id="removeTo" class="btn btn-info btn-sm" runat="server" value="Remove" onclick="RemoveToValues()" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div class="row" style="display: inline-block">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="addCc" class="btn btn-info btn-sm" runat="server" value="Add CC" onclick="AddCCValues()" />
                                                            </td>
                                                            <td>
                                                                <asp:ListBox ID="lbAddCc" runat="server" Width="320" Height="60" SelectionMode="Multiple"></asp:ListBox>
                                                            </td>
                                                            <td>
                                                                <input type="button" id="removeCc" class="btn btn-info btn-sm" runat="server" value="Remove" onclick="RemoveCCValues()" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <%--  <div class="row" style="display: inline-block">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="addBCc" class="btn btn-info btn-sm" value="Add BCC" onclick="AddBCCValues()"></input>
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="lbAddBCc" runat="server" Width="320" Height="60" SelectionMode="Multiple"></asp:ListBox>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="removeBCc" class="btn btn-info btn-sm" value="Remove" onclick="RemoveBCCValues()"></input>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="panel panel-primary" id="divDTDScroll" style="overflow-x: scroll; overflow-y: auto; min-height: 300px; max-height: 500px; margin: 0 10px 0 10px; max-width: 1670px;">
                                        <asp:UpdatePanel ID="updatePanal5" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gridScheduleRpt" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="table table-condensed border" AllowPaging="True" PageSize="1000" AllowSorting="True" Style="overflow: scroll;">
                                                    <AlternatingRowStyle BackColor="#CCFFFF" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Report File Name" DataField="ReportName" />
                                                        <asp:BoundField HeaderText="Export Type" DataField="ExportType" />
                                                        <asp:BoundField HeaderText="Export File Name" DataField="ExportFileName" />
                                                        <asp:BoundField HeaderText="Export Path" DataField="ExportPath" />
                                                        <asp:BoundField HeaderText="Machine" DataField="Machine" />
                                                        <asp:BoundField HeaderText="Shift" DataField="Shift" />
                                                        <asp:BoundField HeaderText="Operator" DataField="Operator" />
                                                        <asp:BoundField HeaderText="Plant ID" DataField="PlantId" />
                                                        <asp:BoundField HeaderText="Line ID" DataField="GroupID" />
                                                        <asp:BoundField HeaderText="Email Flag" DataField="Email_Flag" />
                                                        <asp:BoundField HeaderText="Email TO" DataField="Email_List_To" ItemStyle-CssClass="WrappedText" />
                                                        <asp:BoundField HeaderText="Email CC" DataField="Email_List_CC" ItemStyle-CssClass="WrappedText" />
                                                        <asp:BoundField HeaderText="Email BCC" DataField="Email_List_BCC" ItemStyle-CssClass="WrappedText" Visible="false" />
                                                        <asp:BoundField HeaderText="Run Report For Every" DataField="RunReportForEvery" />
                                                        <%--<asp:BoundField HeaderText="Sl No" DataField="Slno" ControlStyle-Width="0" ControlStyle-CssClass="Hidedisplay" />--%>
                                                        <asp:BoundField HeaderText="Days Before" DataField="DayBefores" ControlStyle-Width="0" ControlStyle-CssClass="Hidedisplay" />
                                                        <asp:BoundField HeaderText="Template Name" DataField="ReportFileName" />

                                                    </Columns>
                                                    <HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </fieldset>
                        </div>

                        <div id="menu1" class="tab-pane fade">
                            <div class="container">
                                <fieldset class="text-center">
                                    <legend style="width: 160px; padding: 0px 4px 0px 4px;">Email Settings</legend>
                                    <div class="emailSetting">
                                        <table class="table table-bordered tbl1 text-center">
                                            <tr>
                                                <td class="commontd">Scheduled Report Last Run </td>
                                                <td class="input-group">
                                                    <div class="input-group-addon">
                                                        <i class="glyphicon glyphicon-calendar"></i>
                                                    </div>
                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                                        title="Date !" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                                                </td>

                                                <td class="commontd">Email Method </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEmailMethod" runat="server" CssClass="select form-control" OnSelectedIndexChanged="ddlEmailMethod_SelectedIndexChanged">
                                                        <asp:ListItem Value="SMTP">SMTP</asp:ListItem>
                                                        <asp:ListItem Value="EWS">EWS</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="commontd"><span id="lblServer">Server</span> </td>
                                                <td>
                                                    <asp:TextBox ID="txtServer" runat="server" CssClass="form-control" data-toggle="tooltip" AutoCompleteType="Disabled"></asp:TextBox>
                                                </td>

                                                <td class="commontd"><span id="lblPort">Port</span> </td>
                                                <td>
                                                    <asp:TextBox ID="txtPort" runat="server" CssClass="form-control" data-toggle="tooltip" AutoCompleteType="Disabled"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="commontd">From Email ID </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromEmailId" runat="server" CssClass="form-control" data-toggle="tooltip" AutoCompleteType="Disabled"></asp:TextBox>
                                                </td>

                                                <td class="commontd">Password </td>
                                                <td>
                                                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-control" data-toggle="tooltip" AutoCompleteType="Disabled"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="4">
                                                    <%--<input type="button" class="btn btn-info btn-sm" ID="btnEnailSetSave" OnClientClick="javascript: return CheckForm();"></input>--%>
                                                    <input type="button" id="btnEnailSetSave" class="btn btn-info btn-sm" value="Save" onclick="javascript: return CheckForm();"></input>
                                                </td>
                                                <%--OnClick="btnEnailSetSave_Click"--%>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        $(document).ready(function () {

            $("a[href$='#menu0']").css("background-color", "#428bca");
            $("a[href$='#menu0']").css("color", "#FFFFFF");
            $(".menuData").click(function () {
                $(".menuData").css("background-color", "");
                $(".menuData").css("color", "");

                $(this).css("background-color", "#428bca");
                $(this).css("color", "#FFFFFF");
            });
            $("[id*=ckhEnableEmail]").prop("checked", false);
            EmailEnableState();
            $(".date").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                //var target = $(e.target).attr("href")
                $("[id$=lblMessages]").text("");
            });

            $("[id$=txtPort]").keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    alert("Only numbers allowed");
                    return false;
                }
            });

            $("[id$=ddlEmailMethod]").on('change', function () {
                if ($("[id$=ddlEmailMethod]").val() == "SMTP") {
                    $("[id$=lblServer]").html("Server");
                    $("[id$=lblPort]").show();
                    $("[id$=txtPort]").show();
                }
                else if ($("[id$=ddlEmailMethod]").val() == "EWS") {
                    $("[id$=lblServer]").html("EWS Url");
                    $("[id$=lblPort]").hide();
                    $("[id$=txtPort]").hide();
                }
            })

            //$('[id$=ddlEmailIds]').multiselect({
            //    includeSelectAllOption: true
            //});

            $("[id$=btnBrowse]").change(function () {
                $("[id$=txtExportPath]").val($(this).val());
            });

            $("#MainContent_gridScheduleRpt td").dblclick(function (event) {
                var row = $(event.target).closest('tr');
                GridDoubleClick(row);
            });
        });


        $(".EmailSettings").on('click', function (event) {

            

        });

        function selectFolder(e) {
            for (var i = 0; i < e.target.files.length; i++) {
                var s = e.target.files[i].name + '\n';
                s += e.target.files[i].size + ' Bytes\n';
                s += e.target.files[i].type;
                console.log(s);
            }
        }
        function DisableAllOptions() {
            $("[id*=ddlPlantId]").prop("disabled", true);
            $("[id*=ddlLineID]").prop("disabled", true);
            $("[id*=ddlMachineId]").prop("disabled", true);
            $("[id*=ddlShift]").prop("disabled", true);
            $("[id*=ddlOperator]").prop("disabled", true);
            $("[id*=ddlRptOn]").prop("disabled", true);
            $("[id*=ddlRptForEvery]").prop("disabled", true);
        }
        function GridDoubleClick(row) {
            DisableAllOptions();
            
            $("[id$=txtSlNo]").val(row.find('td:eq(13)').text());
            //$("[id*=ddlReports]").option.val(row.find('td:eq(0)').text());
            var reptype = row.find('td:eq(0)').text();
            $("[id$=ddlReports]").find('option:selected').val(reptype);
            $("[id$=ddlReports]").find('option:selected').text(reptype);

            if (reptype == "PlanVsActualReport" || reptype == "Line Meter Report" || reptype == "Rejection Report") {
                $("[id*=ddlPlantId]").prop("disabled", false);
                $("[id*=ddlLineID]").prop("disabled", false);
            }
            else if (reptype == "CategoryWise OEE And Loss Time Report" || reptype == "Machine History Report") {
                $("[id*=ddlMachineId]").prop("disabled", false);
            }
            else if (reptype == "Hold Report") {
                $("[id*=ddlLineID]").prop("disabled", false);
                $("[id*=ddlMachineId]").prop("disabled", false);
            }

            $("[id$=HidtemplateName]").val(row.find('td:eq(14)').text())
            if (row.find('td:eq(5)').text() == null || undefined || "") {
                $("[id*=ddlShift]").find('option:selected').val("");
                $("[id*=chkShiftAll]").prop("checked", true);
            }
            else {
                $("[id*=ddlShift]").val(row.find('td:eq(5)').text());
                $("[id*=chkShiftAll]").prop("checked", false);
            }

            $("[id$=txtExportName]").val(row.find('td:eq(2)').text());
            $("[id$=txtExportPath]").val(row.find('td:eq(3)').text());
            var ExportType = row.find('td:eq(1)').text()
            if (ExportType == "0")
                $("[id*=ddlExportTypes]").val("excel");

            if (row.find('td:eq(7)').text() == null || undefined || "") {
                $("[id*=ddlPlantId]").val("ALL");
            }
            else {
                $("[id*=ddlPlantId]").val(row.find('td:eq(7)').text());
            }

            if (row.find('td:eq(8)').text() == null || undefined || "") {
                $("[id$=ddlLineID]").val("ALL");
            }
            else {
                $("[id$=ddlLineID]").val(row.find('td:eq(8)').text());
            }

            if (row.find('td:eq(4)').text() == null || undefined || "") {
                $("[id*=ddlMachineId]").val("ALL");
            }
            else {
                $("[id*=ddlMachineId]").val(row.find('td:eq(4)').text());
            }

            if (row.find('td:eq(6)').text() == null || undefined || "") {
                $("[id*=ddlOperator]").val("All");
            }
            else {
                $("[id*=ddlOperator]").val(row.find('td:eq(6)').text());
            }
            
            //$("[id$=ddlRptForEvery]").find('option:selected').val(row.find('td:eq(11)').text());
            //$("[id*=ddlRptForEvery]").val(row.find('td:eq(11)').text());
            var reportOn = row.find('td:eq(13)').text();
            if (reportOn == 0) {
                $("[id*=ddlRptOn]").val("Today's Data");
            }
            if (reportOn == 1) {
                $("[id*=ddlRptOn]").val("One Day Before Data");
            }
            if (reportOn == 2) {
                $("[id*=ddlRptOn]").val("Two Day Before Data");
            }
            if (reportOn == 3) {
                $("[id*=ddlRptOn]").val("Three Day Before Data");
            }

            $("[id*=ddlRptForEvery]").val(row.find('td:eq(12)').text());

            if (row.find('td:eq(9)').text().toLowerCase() == "true") {
                $("[id*=ckhEnableEmail]").prop("checked", true);
                EmailEnableState();
            }
            else {
                $("[id*=ckhEnableEmail]").prop("checked", false);
                EmailEnableState();
            }

            var emailTo = row.find('td:eq(10)').text().split(",");
            var oprId = row.find('td:eq(6)').text();
            for (var i = 0, len = $("[id*=ddlEmailIds]")[0].options.length; i < len; i++) {
                $("[id*=ddlEmailIds]")[0].options[i].selected = false;
            }

            for (var i in emailTo) {
                var optionVal = emailTo[i];
                for (var i = 0, len = $("[id*=ddlEmailIds]").find('option').length; i < len; i++) {
                    try {
                        var opt = $("[id*=ddlEmailIds]")[0].options[i].value.split("(");
                        //var opt2 = opt[1].split(")")[0];

                        var opt2 = opt.toString();
                        //var opt2 = opt.substring(1, yourString.length - 1);
                        if (opt2.trim() == optionVal) {
                            $("[id*=ddlEmailIds]")[0].options[i].selected = true;
                            $("[id*=lbAddTo]").append(opt2);
                        }
                    }
                    catch (err) {

                    }
                }
            }
            $("[id*=ddlEmailIds]").multiselect('refresh');
            $("[id*=lbAddTo]").empty();
            emailTo = row.find('td:eq(10)').text().split(",");
            emailTo = emailTo.toString().split(';');
            console.log(emailTo);
            var option;
            for (var i in emailTo) {
                if (emailTo[i].trim() != null && emailTo[i] != '') {
                    emailTo[i] = emailTo[i] + ";"
                }

                option = $("<option />").val(emailTo[i]).html(emailTo[i]);
                $("[id*=lbAddTo]").append(option);
            }
            var emailCC = row.find('td:eq(11)').text().split(",");
            emailCC = emailCC.toString().split(';');
            $("[id*=lbAddCc]").empty();
            var option;
            for (var i in emailCC) {
                if (emailCC[i].trim() != null && emailCC[i].trim() != "") {
                    emailCC[i] = emailCC[i] + ";"

                }
                option = $("<option />").val(emailCC[i]).html(emailCC[i]);
                
                if (emailCC[i].trimEnd(' ') != null && emailCC[i].trim(' ') != "")
                    $("[id*=lbAddCc]").append(option);
            }
            //var emailBCC = row.find('td:eq(11)').text().split(",");
            //$("[id*=lbAddBCc]").empty();
            //var option;
            //for (var i in emailBCC) {
            //    option = $("<option />").val(emailBCC[i]).html(emailBCC[i]);
            //    $("[id*=lbAddBCc]").append(option);
            //}
        }

        function BindGrid() {
            $.ajax({
                type: "POST",
                url: "CreateSchedule.aspx/GetScheduleData",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessGridData,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccessGridData(response) {
            var xmlDoc = $.parseXML(response.d);
            console.log(response.d);
            var xml = $(xmlDoc);
            var Schedules = xml.find("Schedules");
            var row = $("[id*=gridScheduleRpt] tr:last-child").clone(true);
            //var totalRows = $("[id*=gridScheduleRpt] tr").length;
            if ($("[id*=gridScheduleRpt] tr").length >= 2)
                $("[id*=gridScheduleRpt] tr").not($("[id*=gridScheduleRpt] tr:first-child")).remove();
            $.each(Schedules, function () {
                
                var Schedule = $(this);
                $("td", row).eq(0).html($(this).find("ReportName").text());
                $("td", row).eq(1).html($(this).find("ExportType").text());
                $("td", row).eq(2).html($(this).find("ExportFileName").text());
                $("td", row).eq(3).html($(this).find("ExportPath").text());
                $("td", row).eq(4).html($(this).find("Machine").text());
                $("td", row).eq(5).html($(this).find("Shift").text());
                $("td", row).eq(6).html($(this).find("Operator").text());
                $("td", row).eq(7).html($(this).find("PlantID").text());
                $("td", row).eq(8).html($(this).find("GroupID").text());
                $("td", row).eq(9).html($(this).find("Email_Flag").text());
                $("td", row).eq(10).html($(this).find("Email_List_TO").text());
                $("td", row).eq(11).html($(this).find("Email_List_CC").text());
                $("td", row).eq(12).html($(this).find("RunReportForEvery").text());
                $("td", row).eq(13).html($(this).find("DayBefores").text());
                $("td", row).eq(14).html($(this).find("ReportFileName").text());
                $("[id*=gridScheduleRpt]").append(row);
                row = $("[id*=gridScheduleRpt] tr:last-child").clone(true);
            });
        }

        function SaveSchedule() {
            
            if ($("[id$=txtExportName]").val() == "") {
                alert("Please add Export File Name");
                return;
            }
            else if ($("[id$=txtExportPath]").val() == "") {
                alert("Please add Export Path");
                return;
            }
                //else if ($('#lbAddTo').children().length == 0 ) {
                //    alert("Please add Email To");
                //    return;
                //}
            else {
                InsertScheduleToDb();
                BindGrid();
            }
        }

        function CheckForm() {
            if ($("[id$=txtDate]").val() == "") {
                alert('Please Enter Date');
                $("[id$=txtDate]").focus();
                return false;
            }
            
            var from = new Date($("[id$=txtDate]").val());
            var today = new Date();
            var Today = (new Date(today.getFullYear(), today.getMonth(), today.getDate() - 3));
            if (from < Today) {
                alert('Cannot be less than 3 days');
                $("[id$=txtDate]").focus();
                return;
            }
            if ($("[id$=ddlEmailMethod]").val() == "") {
                alert('Please Select Email Type');
                $("[id$=ddlEmailMethod]").focus();
                return false;
            }
            if ($("[id$=txtServer]").val() == "") {
                alert('Please Enter Server Name');
                $("[id$=txtServer]").focus();
                return false;
            }
            if ($("[id$=ddlEmailMethod]").val() == "EWS") {
                if ($("[id$=txtPort]").val() == "") {
                    alert('Please Enter Port');
                    $("[id$=txtPort]").focus();
                    return false;
                }
            }
            if ($("[id$=txtFromEmailId]").val() == "") {
                alert('Please Email');
                $("[id$=txtFromEmailId]").focus();
                return false;
            }
            var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test($("[id$=txtFromEmailId]").val())) {
                alert('Please provide a valid email address');
                $("[id$=txtFromEmailId]").focus();
                return false;
            }
            if ($("[id$=txtPassword]").val() == "") {
                alert('Please Enter Password');
                $("[id$=txtPassword]").focus();
                return false;
            }
            InsertDataToDataBase();
            return true;
        }

        function InsertDataToDataBase() {
            $.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
            try {
                $.ajax({
                    type: "POST",
                    url: "CreateSchedule.aspx/InsertDataToDataBase",
                    contentType: "application/json; charset=utf-8",
                    data: '{date:"' + $("[id$=txtDate]").val() + '",emailMethod:"' + $("[id$=ddlEmailMethod]").val() + '",server:"' + $("[id$=txtServer]").val() + '",port:"' + $("[id$=txtPort]").val() + '",fromEmailID:"' + $("[id$=txtFromEmailId]").val() + '",password:"' + $("[id$=txtPassword]").val() + '"}',
                    dataType: "json",
                    success: function (data) {
                        if (data.d) {
                            $("[id$=lblMessages]").text("Successfully Saved !!");
                        }
                        $.unblockUI({});
                    },
                    error: function (data) {
                        alert(data);
                    },
                });
            } catch (e) {
                $.unblockUI({});
            }
            finally {

            }
        }

        function InsertScheduleToDb() {
            
            var email = ($("[id$=ckhEnableEmail]").is(':checked') ? true : false);
            var lbAddToArr = new Array();
            var lbAddCCArr = new Array();
            var lbAddBCCArr = new Array();
            if (email) {
                for (i = 0; i < $("[id*=lbAddTo]")[0].options.length; i++) {
                    var lblto = $("[id*=lbAddTo]")[0].options[i].value;
                    lblto = lblto.replace(';', '');
                    lbAddToArr[i] = lblto;
                }

                for (i = 0; i < $("[id*=lbAddCc]")[0].options.length; i++) {
                    var lblcc = $("[id*=lbAddCc]")[0].options[i].value;
                    lblcc = lblcc.replace(';', '')
                    lbAddCCArr[i] = lblcc;
                }

                //for (i = 0; i < $("[id*=lbAddBCc]")[0].options.length; i++) {
                //    lbAddBCCArr[i] = $("[id*=lbAddBCc]")[0].options[i].value;
                //}

                if (lbAddToArr != undefined) {
                    
                    if (lbAddToArr.length <= 0 || lbAddToArr[0] == undefined || lbAddToArr[0] == "") {
                        alert('Please add email');
                        return;
                    }
                }
            }
            else {
                alert('Please add email');
                return;
            }
            
            //$.blockUI({ message: '<img src="img/loadIcon/ajax-loader.gif" />' });
            try {
                $.ajax({
                    type: "POST",
                    url: "CreateSchedule.aspx/InsertScheduleToDb",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ exportType: $("[id$=ddlExportTypes]").val(), TemplateName: $("[id$=HidtemplateName]").val(), machineId: $("[id$=ddlMachineId]").val(), OperatorId: $("[id$=ddlOperator]").val(), PlantId: $("[id$=ddlPlantId]").val(), LineId: $("[id$=ddlLineID]").val(), ReportForEveryDay: ($("[id$=ddlRptForEvery]").val() == undefined ? "" : $("[id$=ddlRptForEvery]").val()), Reportid: $("[id$=ddlRptOn]").val(), Reports: $("[id$=ddlReports]").val(), ShiftId: $("[id$=ddlShift]").val(), ExportPath: $("[id$=txtExportPath]").val(), ChkEmail: ($("[id$=ckhEnableEmail]").is(':checked') ? true : false), lstAddCC: lbAddCCArr.toString(), lstAddTo: lbAddToArr.toString(), lstAddBCC: lbAddBCCArr.toString(), slno: $("[id$=txtSlNo]").val(), ExportName: $("[id$=txtExportName]").val() }),
                    dataType: "json",
                    success: function (data) {
                        
                        $.unblockUI({});
                        if (data.d) {
                            $("[id$=lblMessages]").text("Successfully Saved !!");
                        }
                        else {
                            alert('Please check path exist or not!!');
                        }
                    },
                    error: function (data) {
                        
                        $.unblockUI({});
                        console.log(data);
                    },
                });
            } catch (e) {
                $.unblockUI({});
            }
            finally {

            }
        }

        function AddToValues() {
            
            //$("[id*=lbAddTo]").empty();
            var option;
            var data = true;
            var options = $("[id*=ddlEmailIds]")[0] && $("[id*=ddlEmailIds]")[0].options;
            var opt;
            for (var i = 0, iLen = options.length; i <= iLen - 1; i++) {
                opt = options[i];
                if (opt.selected) {
                    var arr = opt.value.split('(');
                    var brr = arr[0];
                    option = $("<option />").val(arr).html(arr);
                    if ($("[id*=lbAddTo]")[0].options.length == 0) {
                        $("[id*=lbAddTo]").append('<option value="' + opt.value + ';">' + brr + '</option>');
                    }
                    else {
                        for (var j = 0; j < $("[id*=lbAddTo]")[0].options.length; j++) {
                            var lblto = $("[id*=lbAddTo]")[0].options[j].value;
                            if ((lblto.search(brr) == 0)) {
                                data = false;
                            }
                        }
                        if (data) {
                            
                            data = true;
                            console.log(option + ';')
                            $("[id*=lbAddTo]").append('<option value="' + opt.value + ';">' + brr + '</option>');
                        }
                    }
                }
            }
        }



        function AddCCValues() {
            
            //$("[id*=lbAddCc]").empty();
            var option;
            var data = true;
            var options = $("[id*=ddlEmailIds]")[0] && $("[id*=ddlEmailIds]")[0].options;
            var opt;
            for (var i = 0, iLen = options.length; i < iLen; i++) {
                opt = options[i];
                if (opt.selected) {
                    var arr = opt.value.split('(');
                    var brr = arr[0];
                    option = $("<option />").val(arr).html(arr);
                    if ($("[id*=lbAddCc]")[0].options.length == 0) {
                        $("[id*=lbAddCc]").append('<option value="' + opt.value + ';">' + brr + '</option>');
                    }
                    else {
                        for (var j = 0; j < $("[id*=lbAddCc]")[0].options.length; j++) {
                            var lblto = $("[id*=lbAddCc]")[0].options[j].value;
                            if ((lblto.search(brr) == 0)) {
                                data = false;
                            }

                        }
                        if (data) {
                            
                            data = true;
                            console.log(option + ';')
                            $("[id*=lbAddCc]").append('<option value="' + opt.value + ';">' + brr + '</option>');
                        }
                    }
                }
            }
        }

        function AddBCCValues() {
            $("[id*=lbAddBCc]").empty();
            var option;
            var options = $("[id*=ddlEmailIds]")[0] && $("[id*=ddlEmailIds]")[0].options;
            var opt;
            for (var i = 0, iLen = options.length; i < iLen; i++) {
                opt = options[i];
                if (opt.selected) {
                    var arr = opt.value.split('(');
                    option = $("<option />").val(arr).html(arr);
                    $("[id*=lbAddBCc]").append(option);
                }
            }
        }

        function RemoveToValues() {
            $("[id*=lbAddTo] option:selected").remove();
        }

        function RemoveCCValues() {
            $("[id*=lbAddCc] option:selected").remove();
        }

        function RemoveBCCValues() {
            $("[id*=lbAddBCc] option:selected").remove();
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            
            $.unblockUI({});
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());

            $('[id$=ddlEmailIds]').multiselect({
                includeSelectAllOption: true
            });

            $(".date").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
              });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                //var target = $(e.target).attr("href")
                $("[id$=lblMessages]").text("");
            });
            $("#MainContent_gridScheduleRpt td").dblclick(function (event) {
                var row = $(event.target).closest('tr');
                GridDoubleClick(row);
            });
            EmailEnableState();
        });

        function EmailEnableState() {
            if ($("[id*=ckhEnableEmail]").prop("checked") == true) {
                $("[id*=addTo]").prop("disabled", false);
                $("[id*=removeTo]").prop("disabled", false);
                $("[id*=addCc]").prop("disabled", false);
                $("[id*=removeCc]").prop("disabled", false);
                $("[id*=addBCc]").prop("disabled", false);
                $("[id*=removeBCc]").prop("disabled", false);
                //$("[id*=ddlEmailIds]").prop("disabled", false);
                //$("[id*=ddlEmailIds option]").removeAttr('disabled');
            }
            else {
                $("[id*=addTo]").prop("disabled", true);
                $("[id*=removeTo]").prop("disabled", true);
                $("[id*=addCc]").prop("disabled", true);
                $("[id*=removeCc]").prop("disabled", true);
                $("[id*=addBCc]").prop("disabled", true);
                $("[id*=removeBCc]").prop("disabled", true);
                $("[id*=lbAddTo]").empty();
                $("[id*=lbAddCc]").empty();
                $("[id*=lbAddBCc]").empty();
                //$("[id*=ddlEmailIds]").prop("disabled", true);
            }
            $("[id*=ddlEmailIds]").multiselect('refresh');
        }

    </script>
</asp:Content>
