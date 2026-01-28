<%@ Page Language="C#" Title="TPM to SAP" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TPM2SAP.aspx.cs" Inherits="Web_TPMTrakDashboard.TPM2SAP" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<%: Styles.Render("~/bundles/dateTimecss") %>
	<%: Scripts.Render("~/bundles/dateTimejs") %>

	<style>
		.blue {
			background-color: #2E6886 !important;
			cursor: pointer;
		}

			.blue th {
				color: white !important;
				cursor: pointer;
			}

		.chartHeader {
			background-color: #2E6886 !important;
		}

		.tbl th {
			width: 10%;
		}

		.tbl td {
			width: 10%;
		}

		.displayCss {
			display: inline;
		}

		.footerCss {
			background-color: #2E6886 !important;
		}

		.ajax-loader {
			display: none;
			background-color: rgba(0, 0, 0, 0.6);
			position: absolute;
			z-index: +100 !important;
			width: 100%;
			height: 100%;
			margin-left: -15px;
			margin-top: -16px;
		}

		#load-div {
			position: fixed;
			padding-right: 100px;
			width: 30%;
			top: 40%;
			left: 35%;
			text-align: center;
			border: 3px solid rgb(170, 170, 170);
			background-color: rgb(255, 255, 255);
			cursor: wait;
		}

		.ajax-loader img {
			position: relative;
			left: 50%;
		}

		.asc:after {
			content: "\25B2";
		}

		.desc:after {
			content: "\25BC";
		}

		text {
			text-decoration: none !important;
		}

		.ui-datepicker .ui-datepicker-prev,
		.ui-datepicker .ui-datepicker-next {
			display: none;
		}

		#tblDashboardInfo tbody tr:nth-child(odd) {
			background-color: white;
		}

		#tblDashboardInfo tbody tr:nth-child(even) {
			background-color: #DCDCDC;
		}

		.chartColor {
			margin-top: 3px;
			font-size: 22px;
			cursor: pointer;
			color: white;
		}

		.highcharts-title {
			width: 600px;
			text-align: center;
			top: 1px;
		}

		.multiselect-container {
			overflow-y: auto;
		}

		
		#tblHeader {
			margin-bottom: 0px;
		}
	</style>

	<div class="row" style="text-align: center; color: red;">
		<asp:Label ID="lblMessages" runat="server" EnableViewState="False" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
	</div>

	<div class="row" style="margin-bottom: 4px; margin-top: -3px;">
		<table id="tblHeader" class="table table-bordered" style="background-color: #394A59">
			<tr>
				<td class="commontd" style="width: 82px;"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
				<td class="input-group" style="min-width: 220px;">
					<div class="input-group-addon">
						<i class="glyphicon glyphicon-calendar"></i>
					</div>
					<asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="From DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
				</td>

				<td class="commontd" style="width: 68px;"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
				<td class="input-group" style="min-width: 220px;">
					<div class="input-group-addon">
						<i class="glyphicon glyphicon-calendar"></i>
					</div>
					<asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="To DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
				</td>

				<td class="commontd" style="width: 85px;"><b><%=GetGlobalResourceObject("CommanResource","MachineId")%></b></td>
				<td>
					<asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" data-toggle="tooltip" ToolTip="Select Machine ID" Style="min-width: 100px;"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td class="commontd" style="width: 72px;"><b><%=GetGlobalResourceObject("CommanResource","PlanNo")%></b></td>
				<td>
					<div class="row">
						<div class="col-md-10" style="padding-right: 5px;">
							<asp:DropDownList ID="ddlPlanNumber" runat="server" CssClass="form-control" data-toggle="tooltip" ToolTip="Select Plan Number" Style="min-width: 100px;"></asp:DropDownList>
						</div>
						<div class="col-md-2" style="padding-left: 0px;">
							<button runat="server" id="btnRefreshPlanNo" type="button" class="btn btn-default" style="width: 45px; height: 34px;" onserverclick="btnRefreshPlanNo_ServerClick">
								<span class="glyphicon glyphicon-refresh"></span>
							</button>
						</div>
					</div>
				</td>

				<td class="commontd" style="width: 50px;"><b><%=GetGlobalResourceObject("CommanResource","Mode")%></b></td>
				<td>
					<asp:DropDownList ID="ddlMode" runat="server" CssClass="form-control" data-toggle="tooltip" ToolTip="Select Mode" Style="min-width: 80px;">
						<asp:ListItem>All</asp:ListItem>
						<asp:ListItem>N</asp:ListItem>
						<asp:ListItem>B</asp:ListItem>
					</asp:DropDownList>
				</td>

				<td style="width: 80px;">
					<asp:Button ID="btnRefresh" runat="server" Text="Refresh" class="btn btn-info btn-sm displayCss" Enabled="true" OnClick="btnRefresh_Click" ClientIDMode="Static" />
				</td>
			</tr>
		</table>
	</div>

	<div id="containerbottom">
		<div class="row" id="gridHeader" style="height: 34px;">
			<div class="panel panel-primary">
				<div class="panel-heading" style="background-color: black;">
					<div class="col-lg-12 pull-left" style="padding-bottom: 0; padding-left: 0">
						<input type="button" class="btn btn-primary btn-sm" value="Schedule Data" id="btnScheduleData" style="margin-bottom: 4px; margin-left: -13px; height: 30px;" />
						<input type="button" class="btn btn-default btn-sm" value="Production Data" id="btnProductionData" style="margin-bottom: 4px; height: 30px;" />
						<input type="button" class="btn btn-default btn-sm" value="Down Data" id="btnDownData" style="margin-bottom: 4px; height: 30px;" />
						<input type="button" class="btn btn-default btn-sm" value="Summary Data" id="btnSummaryData" style="margin-bottom: 4px; height: 30px;" />
						<input type="button" class="btn btn-default btn-sm" value="Rework Data" id="btnRework" style="margin-bottom: 4px; height: 30px;" />
						<input type="button" class="btn btn-default btn-sm" value="Export Rework" id="btnReworkExport" runat="server" onserverclick="btnReworkExport_ServerClick" style="margin-bottom: 4px; height: 30px; float: right;" clientidmode="Static" />
						<input type="button" class="btn btn-default btn-sm" value="Export Summary" id="btnExportSummary" runat="server" onserverclick="btnExportSummary_ServerClick" style="margin-bottom: 4px; height: 30px; float: right;" clientidmode="Static" />
						<asp:HiddenField runat="server" Value="1" ID="hiddenforshow" ClientIDMode="Static" />
					</div>
				</div>
			</div>
		</div>

		<div class="row" id="divDataGrids">
			<div class="col-lg-12" style="padding: 0 1px 0 1px" id="gridsContainer">
				<div class="panel panel-primary" id="divScheduleData" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; height: 100%; width: 100%; margin-top: -5px">
					<asp:UpdatePanel ID="updatePanelScheduleData" runat="server">
						<ContentTemplate>
							<asp:GridView ID="gridScheduleData" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="table table-condensed border" AllowPaging="True" PageSize="1000" EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>" AllowSorting="true">
								<AlternatingRowStyle BackColor="#CCFFFF" />
								<Columns>
									<asp:BoundField DataField="PlanNo" SortExpression="PlanNo" meta:resourcekey="BoundFieldResource1" HeaderText="Plan No." />
									<asp:BoundField DataField="Date" SortExpression="Date" meta:resourcekey="BoundFieldResource2" HeaderText="Date" />
									<asp:BoundField DataField="PlantID" SortExpression="PlantID" meta:resourcekey="BoundFieldResource3" HeaderText="Plant ID" />
									<asp:BoundField DataField="EquipmentID" SortExpression="EquipmentID" meta:resourcekey="BoundFieldResource4" HeaderText="Equipment ID" />
									<asp:BoundField DataField="FGMaterialID" SortExpression="FGMaterialID" meta:resourcekey="BoundFieldResource5" HeaderText="FG Material ID" />
									<asp:BoundField DataField="Qty" SortExpression="Qty" meta:resourcekey="BoundFieldResource6" HeaderText="Quantity" />
									<asp:BoundField DataField="PV" SortExpression="PV" meta:resourcekey="BoundFieldResource7" HeaderText="PV_Num" />
									<asp:BoundField DataField="Status" SortExpression="Status" meta:resourcekey="BoundFieldResource8" HeaderText="Status" />
									<asp:BoundField DataField="CycleTime" SortExpression="CycleTime" meta:resourcekey="BoundFieldResource9" HeaderText="Cycle Time" DataFormatString="{0:N2}" />
									<asp:BoundField DataField="LoadUnloadTime" SortExpression="LoadUnloadTime" meta:resourcekey="BoundFieldResource10" HeaderText="Load Unload Time" />
									<asp:BoundField DataField="UpdatedTS" SortExpression="UpdatedTS" meta:resourcekey="BoundFieldResource11" HeaderText="Updated TS" />
								</Columns>
								<HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="#F2F2F2" Height="40" VerticalAlign="Middle" />
								<PagerStyle CssClass="pagination-ys" />
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>

				<div class="panel panel-primary" id="divProductionData" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; height: 100%; width: 100%; margin-top: -5px">
					<asp:UpdatePanel ID="updatePanelProductionData" runat="server">
						<ContentTemplate>
							<asp:GridView ID="gridProductionData" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="table table-condensed border" AllowPaging="True" PageSize="1000" EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>" AllowSorting="true">
								<AlternatingRowStyle BackColor="#CCFFFF" />
								<Columns>
									<asp:BoundField DataField="PlanNo" SortExpression="PlanNo" meta:resourcekey="BoundFieldResource12" HeaderText="Plan No." />
									<asp:BoundField DataField="PlantID" SortExpression="PlantID" meta:resourcekey="BoundFieldResource13" HeaderText="Plant ID" />
									<asp:BoundField DataField="EquipmentID" SortExpression="EquipmentID" meta:resourcekey="BoundFieldResource14" HeaderText="Equipment ID" />
									<asp:BoundField DataField="FGMaterialID" SortExpression="FGMaterialID" meta:resourcekey="BoundFieldResource15" HeaderText="FG MaterialID" />
									<asp:BoundField DataField="PVNo" SortExpression="PVNo" meta:resourcekey="BoundFieldResource16" HeaderText="PV_Num" />
									<asp:BoundField DataField="FGBatchID" SortExpression="FGBatchID" meta:resourcekey="BoundFieldResource17" HeaderText="FG Batch ID" />
									<asp:BoundField DataField="ChildBatchID" SortExpression="ChildBatchID" meta:resourcekey="BoundFieldResource18" HeaderText="Child Batch ID" />
									<asp:BoundField DataField="ProducedQty" SortExpression="ProducedQty" meta:resourcekey="BoundFieldResource19" HeaderText="Produced Qty" />
									<asp:BoundField DataField="ProductionTime" SortExpression="ProductionTime" meta:resourcekey="BoundFieldResource20" HeaderText="Production Time" />
									<asp:BoundField DataField="Status" SortExpression="Status" meta:resourcekey="BoundFieldResource21" HeaderText="Status" />
									<asp:BoundField DataField="BatchStart" SortExpression="BatchStart" meta:resourcekey="BoundFieldResource22" HeaderText="Batch Start" />
									<asp:BoundField DataField="BatchEnd" SortExpression="BatchEnd" meta:resourcekey="BoundFieldResource23" HeaderText="Batch End" />
									<asp:BoundField DataField="RecordedTS" SortExpression="RecordedTS" meta:resourcekey="BoundFieldResource24" HeaderText="Recorded TS" />
									<asp:BoundField DataField="Mode" SortExpression="Mode" meta:resourcekey="BoundFieldResource25" HeaderText="Mode" />
								</Columns>
								<HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="#F2F2F2" Height="40" VerticalAlign="Middle" />
								<PagerStyle CssClass="pagination-ys" />
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>

				<div class="panel panel-primary" id="divDownData" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; height: auto; width: 100%; margin-top: -5px">
					<asp:UpdatePanel ID="updatePanelDownData" runat="server">
						<ContentTemplate>
							<asp:GridView ID="gridDownData" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="table table-condensed border" AllowPaging="True" PageSize="1000" EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>" AllowSorting="True">
								<AlternatingRowStyle BackColor="#CCFFFF" />
								<Columns>
									<asp:BoundField DataField="PlanNo" SortExpression="PlanNo" meta:resourcekey="BoundFieldResource12" HeaderText="Plan No." />
									<asp:BoundField DataField="PlantID" SortExpression="PlantID" meta:resourcekey="BoundFieldResource13" HeaderText="Plant ID" />
									<asp:BoundField DataField="AssetID" SortExpression="AssetID" meta:resourcekey="BoundFieldResource15" HeaderText="Asset ID" />
									<asp:BoundField DataField="EquipmentID" SortExpression="EquipmentID" meta:resourcekey="BoundFieldResource14" HeaderText="Equipment ID" />
									<asp:BoundField DataField="Operator" SortExpression="Operator" meta:resourcekey="BoundFieldResource16" HeaderText="Operator" />
									<asp:BoundField DataField="DownCode" SortExpression="DownCode" meta:resourcekey="BoundFieldResource17" HeaderText="Down Code" />
									<asp:BoundField DataField="DownStartTime" SortExpression="DownStartTime" meta:resourcekey="BoundFieldResource18" HeaderText="Down Start Time" />
									<asp:BoundField DataField="DownEndTime" SortExpression="DownEndTime" meta:resourcekey="BoundFieldResource19" HeaderText="Down End Time" />
									<asp:BoundField DataField="Status" SortExpression="Status" meta:resourcekey="BoundFieldResource20" HeaderText="Status" />
									<asp:BoundField DataField="Mode" SortExpression="Mode" meta:resourcekey="BoundFieldResource21" HeaderText="Mode" />
								</Columns>
								<HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="#F2F2F2" Height="40" VerticalAlign="Middle" />
								<PagerStyle CssClass="pagination-ys" />
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>

				<div class="panel panel-primary" id="divSummaryData" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; height: auto; width: 100%; margin-top: -5px">
					<asp:UpdatePanel ID="updatePanelSummaryData" runat="server">
						<ContentTemplate>
							<asp:GridView ID="gridSummaryData" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="table table-condensed border" AllowPaging="True" PageSize="1000" EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>" AllowSorting="True">
								<AlternatingRowStyle BackColor="#CCFFFF" />
								<Columns>
									<asp:BoundField DataField="SlNo" SortExpression="SlNo" meta:resourcekey="BoundFieldResource21" HeaderText="Serial No." />
									<asp:BoundField DataField="PlanNo" SortExpression="PlanNo" meta:resourcekey="BoundFieldResource22" HeaderText="Plan No." />
									<asp:BoundField DataField="PlantID" SortExpression="PlantID" meta:resourcekey="BoundFieldResource23" HeaderText="Plant ID" />
									<asp:BoundField DataField="EquipmentID" SortExpression="EquipmentID" meta:resourcekey="BoundFieldResource24" HeaderText="Equipment ID" />
									<asp:BoundField DataField="FGMaterialID" SortExpression="FGMaterialID" meta:resourcekey="BoundFieldResource25" HeaderText="FG Material ID" />
									<asp:BoundField DataField="PVNo" SortExpression="PVNo" meta:resourcekey="BoundFieldResource26" HeaderText="PV Number" />
									<asp:BoundField DataField="RegularProductionCount" SortExpression="RegularProductionCount" meta:resourcekey="BoundFieldResource27" HeaderText="Production Count (Regular)" />
									<asp:BoundField DataField="BypassProductionCount" SortExpression="BypassProductionCount" meta:resourcekey="BoundFieldResource28" HeaderText="Production Count (Bypass)" />
									<%--<asp:BoundField DataField="ReworkCount" SortExpression="ReworkCount" meta:resourcekey="BoundFieldResource29" HeaderText="Rework Count" Visible="false" />--%>
								</Columns>
								<HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="#F2F2F2" Height="40" VerticalAlign="Middle" />
								<PagerStyle CssClass="pagination-ys" />
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
				<div class="panel panel-primary" id="divReworkData" style="overflow-x: auto; overflow-y: scroll; min-height: 300px; height: auto; width: 100%; margin-top: -5px">
					<asp:UpdatePanel runat="server" ID="updaterework">
						<ContentTemplate>
							<asp:GridView ID="grdRework" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" CssClass="table table-condensed border" AllowPaging="True" PageSize="1000" EmptyDataText="<%$Resources:CommanResource, Nodataavailable %>" AllowSorting="True">
								<AlternatingRowStyle BackColor="#CCFFFF" />
								<Columns>
									<asp:BoundField HeaderText="Machine ID" DataField="MachineId" meta:resourcekey="BoundFieldResource11" />
									<asp:BoundField HeaderText="Machine Description" DataField="Machinedescription" meta:resourcekey="BoundFieldResource13" />
									<asp:BoundField HeaderText="Child Batch ID" DataField="ChildBatchID" meta:resourcekey="BoundFieldResource12" />
									<asp:BoundField HeaderText="Rework Count" DataField="ReworkCount" meta:resourcekey="BoundFieldResource7" />
									<asp:BoundField HeaderText="Operator ID" DataField="OperatorID" meta:resourcekey="BoundFieldResource8" />
									<asp:BoundField HeaderText="Plan No." DataField="PlanNo" meta:resourcekey="BoundFieldResource9" />
									<asp:BoundField HeaderText="FG Material ID" DataField="FGMaterialID" meta:resourcekey="BoundFieldResource10" />
									<asp:BoundField HeaderText="PV No." DataField="PVNo" meta:resourcekey="BoundFieldResource2" />
									<asp:BoundField HeaderText="FG Batch ID" DataField="FGBatchID" DataFormatString="{0:N0}" HtmlEncode="false" />
									<asp:BoundField HeaderText="Child Part ID" DataField="ChildPartID" DataFormatString="{0:N0}" HtmlEncode="false" />
								</Columns>
								<HeaderStyle BackColor="#3176B1" Font-Bold="True" ForeColor="#F2F2F2" Height="40" VerticalAlign="Middle" />
								<PagerStyle CssClass="pagination-ys" />
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</div>
			<asp:HiddenField runat="server" ID="hdfValue" Value="Production Data" />
		</div>
	</div>

	<script type="text/javascript">
		$(document).ready(function () {
			$.unblockUI({});
			$('[id$=txtFromDate]').datetimepicker({
				format: 'YYYY-MM-DD HH:mm:ss',
				useCurrent: true,
				locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
			});
		    $('[id$=txtFromDate]').removeClass("table tr th ");
			$('[id$=txtToDate]').datetimepicker({
				format: 'YYYY-MM-DD HH:mm:ss',
				useCurrent: false,
				locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
			});
		    $('[id$=txtToDate]').removeClass("table tr th ");
			$("[id$=txtFromDate]").on("dp.change", function (e) {
				$('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
			});

			$("[id$=txtToDate]").on("dp.change", function (e) {
				$('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
			});
			showHideDivs();
		});

		$(document).on("click", "[id$=btnRefresh]", function () {
			if ($("[id$=txtFromDate]").val() == "") {
				alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
				$("[id$=txtFromDate]").focus();
				return false;
			}
			if ($("[id$=txtToDate]").val() == "") {
				alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
				$("[id$=txtToDate]").focus();
				return false;
			}
			var from = $("[id$=txtFromDate]").val();
			var to = $('[id$=txtToDate]').val();

			// date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
			var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
			if (diffe > 15) {
				alert("Difference between to date and from date cannot be more than 15 days.");
				return false;
			}
			var dateCom = compareDates(from, to);
			if (dateCom == 1) {
				alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
				$("[id$=txtToDate]").focus();
				return false;
			}
			showHideDivs();
			  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
		});

		$("#btnScheduleData").click(function () {
			$("#btnScheduleData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
			$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnSummaryData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#divScheduleData").show();
			$("#divProductionData").hide();
			$("#btnReworkExport").hide();
			$("#divDownData").hide();
			$("#divSummaryData").hide();
			$("#btnExportSummary").hide();
			$("#divReworkData").hide();
			$('[Id$=ddlPlanNumber]').attr('disabled', false);
			$('[Id$=ddlMode]').attr('disabled', false);
			$("#hiddenforshow").val(1); btnReworkExport
		});

		$("#btnProductionData").click(function () {
			$("#btnProductionData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
			$("#btnScheduleData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnSummaryData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#divScheduleData").hide();
			$("#divProductionData").show();
			$("#divDownData").hide();
			$("#btnReworkExport").hide();
			$("#divSummaryData").hide();
			$("#divReworkData").hide();
			$("#btnExportSummary").hide();
			$('[Id$=ddlPlanNumber]').attr('disabled', false);
			$('[Id$=ddlMode]').attr('disabled', false);
			$("#hiddenforshow").val(2);
		});

		$("#btnDownData").click(function () {
			$("#btnDownData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
			$("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnScheduleData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnSummaryData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#divScheduleData").hide();
			$("#divProductionData").hide();
			$("#divReworkData").hide();
			$("#btnReworkExport").hide();
			$("#divDownData").show();
			$("#divSummaryData").hide();
			$("#btnExportSummary").hide();
			$('[Id$=ddlPlanNumber]').attr('disabled', false);
			$('[Id$=ddlMode]').attr('disabled', false);
			$("#hiddenforshow").val(3);
		});

		$("#btnSummaryData").click(function () {
			$("#btnSummaryData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
			$("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnScheduleData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#divScheduleData").hide();
			$("#divProductionData").hide();
			$("#divDownData").hide();
			$("#divReworkData").hide();
			$("#btnReworkExport").hide();
			$("#divSummaryData").show();
			$("#btnExportSummary").show();
			$('[Id$=ddlPlanNumber]').attr('disabled', false);
			$('[Id$=ddlMode]').attr('disabled', false);
			$("#hiddenforshow").val(4);
		});
		$("#btnRework").click(function () {
			$("#btnRework").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
			$("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnScheduleData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#btnSummaryData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
			$("#divReworkData").show();
			$("#btnReworkExport").show();
			$("#divScheduleData").hide();
			$("#divProductionData").hide();
			$("#divDownData").hide();
			$("#divSummaryData").hide();
			$("#btnExportSummary").hide();
			$('[Id$=ddlPlanNumber]').attr('disabled', true);
			$('[Id$=ddlMode]').attr('disabled', true);
			$("#hiddenforshow").val(2);
		});

		function showHideDivs() {
			if ($("#hiddenforshow").val() == 1) {
				$("#btnScheduleData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
				$("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnSummaryData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#divScheduleData").show();
				$("#divProductionData").hide();
				$("#divDownData").hide();
				$("#divSummaryData").hide();
				$("#btnExportSummary").hide();
				$("#divReworkData").hide();
				$("#btnReworkExport").hide();
			}
			else if ($("#hiddenforshow").val() == 2) {
				$("#btnProductionData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
				$("#btnScheduleData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnSummaryData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#divScheduleData").hide();
				$("#divProductionData").show();
				$("#divReworkData").hide();
				$("#btnReworkExport").hide();
				$("#divDownData").hide();
				$("#divSummaryData").hide();
				$("#btnExportSummary").hide();
			}
			else if ($("#hiddenforshow").val() == 3) {
				$("#btnDownData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
				$("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnScheduleData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnSummaryData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#divScheduleData").hide();
				$("#divProductionData").hide();
				$("#divDownData").show();
				$("#divSummaryData").hide();
				$("#divReworkData").hide();
				$("#btnReworkExport").hide();
				$("#btnExportSummary").hide();
			}
			else if ($("#hiddenforshow").val() == 4) {
				$("#btnSummaryData").removeClass("btn btn-sm btn-default").addClass("btn btn-sm btn-primary");
				$("#btnProductionData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnScheduleData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnRework").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#btnDownData").removeClass("btn btn-sm btn-primary").addClass("btn btn-sm btn-default");
				$("#divScheduleData").hide();
				$("#divProductionData").hide();
				$("#divDownData").hide();
				$("#divReworkData").hide();
				$("#btnReworkExport").hide();
				$("#divSummaryData").show();
				$("#btnExportSummary").show();
			}
		}
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
