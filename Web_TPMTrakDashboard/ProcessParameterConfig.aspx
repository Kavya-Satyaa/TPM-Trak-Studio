<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessParameterConfig.aspx.cs" Inherits="Web_TPMTrakDashboard.ProcessParameterConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<style>
		.header-center {
			text-align: center;
		}

		.button2 {
			background: #202648;
			border-radius: 60px;
			border-color: white;
			color: #fff;
			height: 30px;
		}
	</style>
	<asp:UpdatePanel runat="server">
		<ContentTemplate>
			<div style="height: auto; width: auto; margin: 15px">
				<table class="table table-bordered" style="height: auto; width: auto">
					<tr>

						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Machine Id" ForeColor="White" />
						</td>
						<td>
							<asp:DropDownList runat="server" ID="cmbmachine" ForeColor="Black" Width="150" AutoCompleteType="Disabled" Height="30" />
						</td>
						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Parameter" ForeColor="White" />
						</td>
						<td>
							<asp:DropDownList runat="server" ID="cmbpara" ForeColor="Black" Width="150" AutoCompleteType="Disabled" Height="30" />
						</td>
					</tr>
					<tr>
						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Component" ForeColor="White" />
						</td>
						<td>
							<asp:DropDownList runat="server" ID="cmbComponent" ForeColor="Black" Width="150" AutoCompleteType="Disabled" Height="30" />
						</td>
					</tr>
					<tr>
						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Lower Error Zero Limit" ForeColor="White" />
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtlowerr" CssClass="form-control date2" AutoCompleteType="Disabled" />
						</td>
						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Upper Error Zero Limit" ForeColor="White" />
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtupperr" CssClass="form-control date2" AutoCompleteType="Disabled" />
						</td>
					</tr>
					<tr>
						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Lower Operating Zero Limit" ForeColor="White" />
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtlowop" CssClass="form-control date2" AutoCompleteType="Disabled" />
						</td>
						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Upper Operating Zero Limit" ForeColor="White" />
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtupoper" CssClass="form-control date2" AutoCompleteType="Disabled" />
						</td>
					</tr>
					<tr>
						<td class="auto-style27" style="vertical-align: middle">
							<asp:Label runat="server" Text="Lower Warning Zero Limit" ForeColor="White" />
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtlowwar" CssClass="form-control date2" AutoCompleteType="Disabled" />
						</td>
						<td style="vertical-align: middle">
							<asp:Label runat="server" Text="Upper Warning Zero Limit" ForeColor="White" />
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtupwar" CssClass="form-control date2" AutoCompleteType="Disabled" />
						</td>
					</tr>

				</table>
				<div class="auto-style21" style="vertical-align: middle; margin: 15px; align-content: center">
					<asp:Button runat="server" ID="btnsave" OnClick="btnSave_Click" Text="<%$Resources:CommanResource, Save %>" CssClass="button2" Width="80" Height="30" />
					<asp:Button runat="server" ID="btndelete" OnClick="btndelete_Click" Text="Delete" CssClass="button2" Width="80px" Height="30px" />
				</div>
			</div>
			<div class="row text-center">
				<asp:Label ID="lblMessages" EnableViewState="false" Font-Size="Large" runat="server" Style="font-weight: bold; visibility: visible; color: yellow; font-family: Calibri;"></asp:Label>
			</div>

			<div style="overflow-x: auto; overflow-y: auto; margin: 15px; min-width: 100px;">
				<asp:GridView runat="server" AutoGenerateColumns="False" CssClass="table table-bordered " meta:resourcekey="grdViewAndonViewResource1" ID="GridProcessPara" OnSelectedIndexChanged="changedindex" CellPadding="4" ForeColor="#333333" GridLines="None">
					<AlternatingRowStyle BackColor="White" />
					<Columns>
						<asp:ButtonField ButtonType="Button"
							CommandName="Select"
							Text="Select"  ControlStyle-CssClass="button2"  />
						<asp:TemplateField HeaderText="Process ID" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource1" Visible="false">
							<ItemTemplate>
								<asp:Label ID="lblSLno" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("SLNO") %>' meta:resourcekey="lblslnoTextResource1" Visible="false"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" Wrap="false" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Machine Id" ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource2">
							<ItemTemplate>
								<asp:Label ID="lblMachineID" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("MachineID") %>' meta:resourcekey="lblMachineIDTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Parameter" meta:resourcekey="TemplateFieldResource3">
							<ItemTemplate>
								<asp:Label ID="lblParameter" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("Parameter") %>' meta:resourcekey="lblParameterTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Component" meta:resourcekey="TemplateFieldResource4" ItemStyle-Wrap="false">
							<ItemTemplate>
								<asp:Label ID="lblComponent" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("Component") %>' meta:resourcekey="lblComponentTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Lower Error Zone Limit" meta:resourcekey="TemplateFieldResource5">
							<ItemTemplate>
								<asp:Label ID="lbllezl" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("LowError") %>' meta:resourcekey="lbllezlTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Upper Error Zone Limit" meta:resourcekey="TemplateFieldResource6">
							<ItemTemplate>
								<asp:Label ID="lbluezl" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("UppError") %>' meta:resourcekey="lbluezlTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" />
						</asp:TemplateField>

						<asp:TemplateField HeaderText="Lower Operating Zone Limit" meta:resourcekey="TemplateFieldResource7">
							<ItemTemplate>
								<asp:Label ID="lbllozl" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("LowOp") %>' meta:resourcekey="lbllozlTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" />
						</asp:TemplateField>

						<asp:TemplateField HeaderText="Upper Operating Zone Limit" meta:resourcekey="TemplateFieldResource8">
							<ItemTemplate>
								<asp:Label ID="lbluozl" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("UppOp") %>' meta:resourcekey="lbluozlTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Lower Warning Zone Limit" meta:resourcekey="TemplateFieldResource9">
							<ItemTemplate>
								<asp:Label ID="lbllwzl" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("LowWar") %>' meta:resourcekey="lbllwzlTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle CssClass="text-center" ForeColor="White" />
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Upper Operating Zone Limit" meta:resourcekey="TemplateFieldResource10">
							<ItemTemplate>
								<asp:Label ID="lbluwzl" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("UppWar") %>' meta:resourcekey="lbluwzlTextResource1"></asp:Label>
							</ItemTemplate>
							<HeaderStyle ForeColor="White" CssClass="HeaderCss" />
						</asp:TemplateField>
					</Columns>
					<EditRowStyle BackColor="#2461BF" />
					<FooterStyle BackColor="#507CD1" Font-Bold="True" />
					<HeaderStyle BackColor="#507CD1" Font-Bold="True" />
					<PagerStyle BackColor="#2461BF" HorizontalAlign="Center" />
					<RowStyle BackColor="#EFF3FB" />
					<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" />
					<SortedAscendingCellStyle BackColor="#F5F7FB" />
					<SortedAscendingHeaderStyle BackColor="#6D95E1" />
					<SortedDescendingCellStyle BackColor="#E9EBEF" />
					<SortedDescendingHeaderStyle BackColor="#4870BE" />

				</asp:GridView>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<script type="text/javascript">
		$(document).ready(function () {
			var winHeight = $(window).height();
			if (winHeight < 650) {
				winHeight = (winHeight - 350);
			} else {
				winHeight = (winHeight - 300);
				$("#GridProcessPara").height(winHeight);
			}
		})
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
