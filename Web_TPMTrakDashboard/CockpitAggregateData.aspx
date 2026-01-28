<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CockpitAggregateData.aspx.cs" Inherits="Web_TPMTrakDashboard.CockpitAggregateData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<style>
		.label {
			display: inline-block;
			color: white;
			font-weight: bold;
			font-size: large;
		}

		.div {
			border-color: white;
			border-style: solid;
			padding: 5px;
			border-radius: 5px;
		}

		.div {
			border-color: white;
			border-style: solid;
			padding: 5px;
			border-radius: 5px;
		}

		.button {
			display: inline-block;
			background: #4CAF50;
			color: white;
			border: transparent;
			border-style: groove;
			border-radius: 5px;
			height: 30px;
		}

		.div2 {
			padding: 5px;
			border-radius: 5px;
			margin-top: 5px;
		}

		.dropdown {
			display: inline-block;
			border-radius: 5PX;
			height: 25px;
		}

		table {
			width: 100%;
			color: white;
		}

			table tr th {
				color: white;
				padding: 10px;
				background-color: #2E6886 !important;
				text-align: center;
			}

		tr:nth-child(even) {
			background-color: #f2f2f2;
			height: 40px;
			text-align: center;
		}

		tr:nth-child(odd) {
			background-color: white;
			text-align: center;
			height: 40px;
		}
	</style>
	<div>
		<div class="div">
			<asp:UpdatePanel runat="server" ID="updatepnl">
				<ContentTemplate>
					<label class="label">From Date:</label>
					<asp:TextBox Style="display: inline-block" ID="txtfromdate" runat="server" TextMode="Date" />
					<label class="label">To Date</label>
					<asp:TextBox ID="txttodate" runat="server" Style="display: inline-block" TextMode="Date" />
					<label class="label">Plant</label>
					<asp:DropDownList ID="ddlPlantID" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged" AutoPostBack="true" />
					<label class="label">Machine-ID</label>
					<asp:DropDownList ID="ddlMachineID" runat="server" CssClass="dropdown" />
					<label class="label">Shift Name</label>
					<asp:DropDownList ID="ddlshift" runat="server" CssClass="dropdown" />
					<asp:Button CssClass="button" runat="server" ID="btnsearch" Text="Search" OnClick="btnsearch_Click" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div class="div2">
			<%--<asp:GridView runat="server" ID="gridviewaggregation" CssClass="table table-bordered" HeaderStyle-BackColor="#5391CA" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Large" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true">
				<AlternatingRowStyle BackColor="#DCE7F5" />
				<EmptyDataTemplate>
					No data available for selected plant and date time period.
				</EmptyDataTemplate>
				<HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Large" ForeColor="White" HorizontalAlign="Center" />
				<RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
			</asp:GridView>--%>
			<asp:ListView runat="server" ID="lstviewcockpitaggregate">
				<LayoutTemplate>
					<table>
						<tr>
							<th class="th">Machine-Id</th>
							<%--<th>Machine Description</th>--%>
							<th>Component</th>
							<th>AE(%)</th>
							<th>PE(%)</th>
							<th>OEE(%)</th>
							<th>Net Utilised Time</th>
							<th>DownTime</th>
							<%--<th>Max Reason Time</th>--%>
						</tr>
						<tr id="ItemPlaceholder" runat="server">
						</tr>
					</table>
				</LayoutTemplate>
				<ItemTemplate>
					<tr id="value">
						<td>
							<asp:Label runat="server" Text='<%# Eval("MachineID") %>' /></td>
						<%--<td><asp:Label runat="server" Text="<%# Eval("MachineDescription") %>"/></td></td>--%>
						<td>
							<asp:Label runat="server" Text='<%# Eval("ProdCount") %>' /></td>
						<td>
							<asp:Label runat="server" Text='<%# Eval("AEffy") %>' /></td>
						<td>
							<asp:Label runat="server" Text='<%# Eval("PEffy") %>' /></td>
						<td>
							<asp:Label runat="server" Text='<%# Eval("OEffy") %>' /></td>
						<td>
							<asp:Label runat="server" Text='<%# Eval("UtilisedTime") %>' /></td>
						<td>
							<asp:Label runat="server" Text='<%# Eval("DownTime") %>' /></td>
						<%--<td><asp:Label runat="server" Text="<%# Eval("MachineID") %>"/></td>--%>
					</tr>
				</ItemTemplate>
			</asp:ListView>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
