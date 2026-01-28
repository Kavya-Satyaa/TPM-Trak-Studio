<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnergyLivescreen.aspx.cs" Inherits="Web_TPMTrakDashboard.EnergyLivescreen" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		.TableHeader {
			background-color: #5391CA;
			color: #0066FF;
			font-size: large;
			font-weight: 600;
			font-family: Verdana;
			text-align: center;
			border-color: black;
			border-width: 2px;
			border: 2px solid black;
			line-height: 60px;
			height: 70px;
		}

		.TableData {
			background-color: #DCE7F5;
			color: white;
			font-family: Courier New;
			font-size: medium;
			font-weight: bold;
			text-align: center;
		}

		.Header {
			color: White;
			font-family: Courier New;
			font-size: medium;
			font-weight: bold;
			text-align: center;
		}

		.Datatd {
			width: 200px;
		}
	</style>

	<asp:UpdatePanel runat="server">
	<%--	<Triggers>
			<asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick"></asp:AsyncPostBackTrigger>
		</Triggers>--%>
		<ContentTemplate>
			<div style="margin: 5px">
				<table border="1" style="border-color: white; border-width: 1px; height: 40px">
					<tr>
						<td style="width: 300px;">
							<font color="white"><b>Machine- ID</b></font>
							<asp:DropDownList runat="server" ID="ddlMachineID" Width="160px" meta:resourcekey="ddlMachineIDResource1" />
						</td>
						<td style="width: 300px;">
							<font color="white"><b>Node- ID</b></font>
							<asp:DropDownList runat="server" ID="ddlNodeID" Width="160px" meta:resourcekey="ddlNodeIDResource1" />
						</td>
						<td class="commontd" style="width:180px;">
                            <label>
                                <span class="checkbox">
                                    <asp:CheckBox ID="chkAutoBox" runat="server" type="checkbox" OnCheckedChanged="chkAutoBox_CheckedChanged" meta:resourcekey="chkAutoBoxResource1" /><b><%=GetGlobalResourceObject("CommanResource","AutoRefresh") %></b></span>
                            </label>
							</td>
						<td style="width: 95px; text-align: center">
							<asp:Button runat="server" ID="btnview" Text="View" OnClick="btnview_Click" CssClass="btn btn-primary" meta:resourcekey="btnviewResource1" />
						</td>
					</tr>
				</table>
			</div>
			<div>
				<label class="Header"><b>Live Energy Data</b></label>
			</div>
			<div style="overflow: auto">
				<asp:ListView runat="server" ID="listviewlivescreen">
					<LayoutTemplate>
						<table id="Table1" runat="server" border="1" style="border-color: black; border-width: 5px">
							<tr id="Tr1" runat="server" class="TableHeader">
								<td id="tdMachine" runat="server"><font color="white"><%=GetLocalResourceObject("Machine") %></font></td>
								<td id="tdNode" runat="server"><font color="white"><%=GetLocalResourceObject("Node") %></font></td>
								<td id="tdV1" runat="server"><font color="white"><%=GetLocalResourceObject("R2N") %></font></td>
								<td id="tdV2" runat="server"><font color="white"><%=GetLocalResourceObject("Y2N") %></font></td>
								<td id="tdV3" runat="server"><font color="white"><%=GetLocalResourceObject("G2N") %></font></td>
								<td id="tdV12" runat="server"><font color="white"><%=GetLocalResourceObject("R2Y") %></font></td>
								<td id="tdV23" runat="server"><font color="white"><%=GetLocalResourceObject("Y2G") %></font></td>
								<td id="tdV31" runat="server"><font color="white"><%=GetLocalResourceObject("G2R") %></font></td>
								<td id="tdA1" runat="server"><font color="white"><%=GetLocalResourceObject("A1") %></font></td>
								<td id="tdA2" runat="server"><font color="white"><%=GetLocalResourceObject("A2") %></font></td>
								<td id="tdA3" runat="server"><font color="white"><%=GetLocalResourceObject("A3") %></font></td>
								<td id="tdPF1" runat="server"><font color="white"><%=GetLocalResourceObject("PF1") %></font></td>
								<td id="tdPF2" runat="server"><font color="white"><%=GetLocalResourceObject("PF2") %></font></td>
								<td id="tdPF3" runat="server"><font color="white"><%=GetLocalResourceObject("PF3") %></font></td>
								<td id="tdAverage" runat="server"><font color="white"><%=GetLocalResourceObject("Average") %></font></td>
								<td id="tdWatts" runat="server"><font color="white"><%=GetLocalResourceObject("Watt") %></font></td>
								<td id="tdLasttime" runat="server"><font color="white"><%=GetLocalResourceObject("LastArrivalTime") %></font></td>
							</tr>
							<tr id="ItemPlaceholder" runat="server">
							</tr>
						</table>
					</LayoutTemplate>
					<ItemTemplate>
						<tr class="TableData">
							<td>
								<asp:Label runat="server" ID="lblMachine" Text='<%# Eval("MachineID") %>' Width="150" meta:resourcekey="lblMachineResource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblNode" Text='<%# Eval("NodeID") %>' Width="100" meta:resourcekey="lblNodeResource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblV1" Text='<%# Eval("V1") %>' Width="100" meta:resourcekey="lblV1Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblV2" Text='<%# Eval("V2") %>' Width="100" meta:resourcekey="lblV2Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblV3" Text='<%# Eval("V3") %>' Width="100" meta:resourcekey="lblV3Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblV12" Text='<%# Eval("V12") %>' Width="100" meta:resourcekey="lblV12Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblV23" Text='<%# Eval("V23") %>' Width="100" meta:resourcekey="lblV23Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblV31" Text='<%# Eval("V31") %>' Width="100" meta:resourcekey="lblV31Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblA1" Text='<%# Eval("A1") %>' Width="100" meta:resourcekey="lblA1Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblA2" Text='<%# Eval("A2") %>' Width="100" meta:resourcekey="lblA2Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblA3" Text='<%# Eval("A3") %>' Width="100" meta:resourcekey="lblA3Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblPF1" Text='<%# Eval("PF1") %>' Width="100" meta:resourcekey="lblPF1Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblPF2" Text='<%# Eval("PF2") %>' Width="100" meta:resourcekey="lblPF2Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblPF3" Text='<%# Eval("PF3") %>' Width="100" meta:resourcekey="lblPF3Resource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblavergae" Text='<%# Eval("LivePF") %>' Width="100" meta:resourcekey="lblavergaeResource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lblwatts" Text='<%# Eval("Watt") %>' Width="100" meta:resourcekey="lblwattsResource1" />
							</td>
							<td>
								<asp:Label runat="server" ID="lbllasttime" Text='<%# Eval("LastArrivalTime") %>' Width="250px" Height="40px" meta:resourcekey="lbllasttimeResource1" />
							</td>
						</tr>
					</ItemTemplate>
				</asp:ListView>
			</div>
            <asp:Timer ID="timerDataChange" runat="server" OnTick="timerDataChange_Tick"></asp:Timer>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
