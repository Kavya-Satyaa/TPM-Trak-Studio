<%@ Page Title="" Language="C#" MasterPageFile="~/SonaAndon/SonaSite.Master" AutoEventWireup="true" CodeBehind="SonaAndon.aspx.cs" Inherits="SonaAndon.SonaAndon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		.TableHeader {
			background-color: #2665B2;
			color: white;
			font-size: large;
			font-weight: 600;
			font-family: Verdana;
			text-align: Center;
			font-style: normal;
			border-color: white;
			border-width: 2px;
			border: 2px solid orange;
			height: 100%;
			width: 100%;
			text-wrap: unset;
		}

		.TableData {
			background-color: #76CEFF;
			/*#DCE7F5*/
			color: black;
			font-family: Courier New;
			font-size: medium;
			font-weight: bolder;
			text-align: Left;
			width: 100%;
			height: 100%;
		}

		.Header {
			color: White;
			font-family: Courier New;
			font-size: medium;
			font-weight: bold;
			text-align: center;
		}

		tr:nth-child(1) {
			background: #2665B2;
		}

		tr:nth-child(even) {
			background-color: #CBEFFF;
		}

		tr:nth-child(odd) {
			background-color: whitesmoke;
		}

		tr:nth-child(1) {
			background-color: #2665B2;
		}

		.Datatd {
			width: 500px;
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

		.divBody {
			padding-top: 0px;
			padding-bottom: 0px;
		}
	</style>
	<link href="../Content/Ionic.css" rel="stylesheet" />
	<div style="width: 100%; padding: 0px">
		<asp:UpdatePanel runat="server">
			<ContentTemplate>
				<div style="width: 100%; padding: unset">
					<asp:ListView runat="server" ID="listviewSonaAndon">
						<LayoutTemplate>
							<table id="table" class="TableHeader" border="1" style="border-color: #1C4C86; border-width: 5px; padding: 10px">
								<tr>
									<th style="min-width: 15%; white-space: nowrap; padding: 10px; text-align:center;">Work Center</th>
									<th style="min-width: 15%; white-space: nowrap; padding: 10px; text-align:center;">Machine Description</th>
									<th style="white-space: nowrap;text-align:center;">Part Number</th>
									<th style="text-align:center;padding-right:5px;">Schedule <br /> Quantity</th>
									<th style="text-align:center;padding-right:5px;">To be Completed <br /> Till Time </th>
									<th style="text-align:center;padding-right:5px;">Completed <br /> till Time</th>
									<th style="text-align:center;padding-right:5px;">Shift OEE (%)</th>
									<th style="white-space:nowrap;text-align:center;" colspan="2">Machine Status</th>
									<th style="white-space:nowrap;text-align:center;">Last known Process Status</th>

								</tr>
								<tr id="ItemPlaceholder" runat="server">
								</tr>
							</table>
						</LayoutTemplate>
						<ItemTemplate>
							<tr id="tr" class="TableData">
								<td style="min-width: 15%; padding: 10px; white-space: nowrap;">
									<asp:Label runat="server" ID="lblWorkCenter" Text='<%# Bind("WorkCenter") %>' /></td>
								<td style="min-width: 15%; padding: 10px; white-space: nowrap;">
									<asp:Label runat="server" ID="Label1" Text='<%# Bind("MachineDescription") %>' /></td>
								<td style="white-space: nowrap; padding-left: 5px">
									<asp:Label runat="server" ID="lblPartNumber" Text='<%# Bind("PartNumber") %>' /></td>
								<td style="padding-left: 5px;text-align:right;padding-right:5px;">
									<asp:Label runat="server" ID="lblScheduleqty" Text='<%# Bind("Scheduleqty") %>'  /></td>
								<td style="padding-left: 5px;text-align:right;padding-right:5px;">
									<asp:Label runat="server" ID="lbltarget" Text='<%# Bind("Target") %>' /></td>
								<td style="padding-left: 5px;text-align:right;padding-right:5px;">
									<asp:Label runat="server" ID="lblCompletedTime" Text='<%# Bind("Completedtime") %>' /></td>
								<td style="padding-left: 5px;text-align:right;padding-right:5px;">
									<asp:Label runat="server" ID="lblOEE" Text='<%# Bind("OEE") %>' /></td>
                                <td style="padding-left: 5px;text-align:left;padding-right:5px;border-right-style:hidden;width:auto"> 
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Bind("Status") %>' Style="vertical-align: sub" />
                                </td>
								<td style="white-space:nowrap;text-align:center;">
									<%--<asp:Image runat="server" ID="image" ImageUrl='<%# Bind("Image") %>' />--%>
									<div runat="server" class="loaders-container" style="display: inline-block;float:right" visible='<%# Eval("Visibility").ToString() == "false"?false:true %>'>
										<div class="la-cog la-2x" style="float: right;">
											<div class="<%# Eval("OKNOT") %>"></div>
										</div>
									</div>
								</td>
								<td style="padding-left: 5px;white-space:nowrap">
									<asp:Label runat="server" ID="Label2" Text='<%# Bind("DownReason") %>' /></td>
							</tr>
						</ItemTemplate>
					</asp:ListView>
				</div>
				<div>
					<asp:Timer runat="server" ID="timer" Enabled="false" OnTick="timer_Tick" />
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<script type="text/javascript"> 
		function setcolor() {
           
			var fontsize =<%=fontsize %>;
			var fontsizeheader = "<%=Headerfontsize %>"
			var fontfamily="<%=fontfamily %>"
			var fontstyle = "<%=fontstyle %>"
			var backgroundcolor = "<%=background %>"
			var Alternativebackgroundcolor = "<%=Alternativebackground %>"
			var Headerbackgroundcolor = "<%=Headerbackgroundcolor %>"
			$("#table").css('font-size', fontsizeheader);
			$("#table").css('font-family', fontfamily);
			$("#table").css('font-style', fontstyle);
			$("tr").css('font-size', fontsize);
			$("tr").css('font-style', fontstyle);
			$("tr").css('font-family', fontfamily);
			$("tr:even").css("background-color", backgroundcolor);
			$("tr:odd").css("background-color", Alternativebackgroundcolor);
			$("tr:first").css("background-color", Headerbackgroundcolor);
			$("tr:first").css("font-size", fontsizeheader);
		}
		$(document).ready(function () {
			setcolor();
			$("#divbody").css("padding","0px")
		})
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(function () {
			setcolor();
		});
	</script>
</asp:Content>
