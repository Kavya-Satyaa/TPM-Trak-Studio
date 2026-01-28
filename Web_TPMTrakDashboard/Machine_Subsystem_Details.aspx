<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Machine_Subsystem_Details.aspx.cs" Inherits="Web_TPMTrakDashboard.Machine_Subsystem_Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<asp:UpdatePanel runat="server">
		<ContentTemplate>
			<div style="height: 10vh">
				<div style="margin: 10px">
					<table class="table table-bordered">
						<tr>
							<td style="height: 20px; width: 80px">
								<label class="control-label" style="color: white; font-size: larger; width: 45px; height: 20px">Plant</label>
							</td>
							<td style="height: 20px; width: 185px">
								<asp:DropDownList runat="server" ID="ddlplantid" Width="150" AutoPostBack="True" Height="30" OnSelectedIndexChanged="ddlplantid_SelectedIndexChanged" meta:resourcekey="ddlFontStyleResource1" />
							</td>
							<td style="height: 20px; width: 150px">
								<label class="control-label" style="color: white; font-size: larger; height: 20px;">Machine Id</label>
							</td>
							<td style="height: 20px; width: 185px">
								<asp:DropDownList runat="server" ID="ddlmachineid" Width="150" Height="30" meta:resourcekey="ddlFontStyleResource2" />
							</td>
							<td style="height: 20px">
								<asp:Button runat="server" Text="View" ID="btnview" Width="60" OnClick="btnview_Click" Height="30" class="btn btn-info btn-sm" Style="margin-left: 15px;" />
								<asp:Button runat="server" ID="btnsave" OnClick="btnsave_Click" Width="60" Text="Save" Height="30" class="btn btn-info btn-sm" Style="margin-left: 15px;" ClientIDMode="Static" />
								<asp:Button runat="server" ID="btndelete" OnClick="btndelete_Click" Width="60" Height="30" Text="Delete" class="btn btn-info btn-sm" Style="margin-left: 15px;" />
							</td>
						</tr>
					</table>
				</div>
				<div class="row text-center">
					<asp:label runat="server" ID="lblmessage" ForeColor="Green"  EnableViewState="false"  Style="font-weight: bold;font-size:larger font-family: Calibri;"/>
				</div>
				<div id="griddiv" style="margin: 10px; overflow: auto; height: 40" class="headerFixer">
					<asp:GridView runat="server" ID="gridviewmachinesubsystem" AutoGenerateColumns="False" CssClass="table table-bordered " meta:resourcekey="grdViewAndonViewResource1" HeaderStyle-VerticalAlign="Middle" HorizontalAlign="Center" ShowHeaderWhenEmpty="True" HeaderStyle-HorizontalAlign="Center" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" OnDataBound="gridviewmachinesubsystem_DataBound" BorderWidth="1px" CellPadding="3" CellSpacing="2" ShowFooter="true" ClientIDMode="Static">
						<Columns>
							<asp:BoundField HeaderText="SL No." DataField="SLNO" HeaderStyle-ForeColor="White" HeaderStyle-Font-Size="Large" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Wrap="false" meta:resourcekey="TemplateFieldResource1">
								<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" ForeColor="White"></HeaderStyle>
							</asp:BoundField>
							<asp:TemplateField HeaderText="Machine ID" meta:resourcekey="TemplateFieldResource1" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<asp:HiddenField runat="server" ID="hiddenfield" />
									<asp:Label ID="lblmachine" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("MachineID") %>' meta:resourcekey="lblValueInTextResource1"></asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList runat="server" ID="ddlmachineid" Width="180" AutoPostBack="false" meta:resourcekey="ddlFontStyleResource" />
								</FooterTemplate>
								<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" ForeColor="White"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Sub System" meta:resourcekey="TemplateFieldResource2" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<asp:Label ID="lblsubsystem" CssClass="control-label  ValiDate" runat="server" Text='<%# Bind("Subsystem") %>' meta:resourcekey="lblValueInTextResource1"></asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:DropDownList runat="server" ID="ddlsubsystem" AutoPostBack="false" Width="180" meta:resourcekey="ddlFontStyleResource4" />
								</FooterTemplate>
								<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" ForeColor="White"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Equipment ID" meta:resourcekey="TemplateFieldResource3" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<asp:TextBox ID="txtEquipID" Width="100" CssClass="control-label ValiDate" runat="server" Text='<%# Bind("EquipmentID") %>' meta:resourcekey="lblEQUIPIDTextResource1" AutoCompleteType="Disabled"></asp:TextBox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox runat="server" ID="txtfooterequipID" Width="100" AutoCompleteType="Disabled" AutoPostBack="false" ClientIDMode="Static" />
								</FooterTemplate>
								<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" ForeColor="White"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Equipment Details" meta:resourcekey="TemplateFieldResource4" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
								<ItemTemplate>
									<asp:TextBox ID="txtEquipdetails" CssClass="control-label ValiDate" runat="server" Text='<%# Bind("EquipmentDetails") %>' meta:resourcekey="lblEquipdescTextResource1" AutoCompleteType="Disabled" Width="350"></asp:TextBox>
								</ItemTemplate>
								<FooterTemplate>
									<asp:TextBox runat="server" ID="txtfooterequipmentdetails" AutoPostBack="false" Width="300" AutoCompleteType="Disabled" ClientIDMode="Static" />
									<asp:Button runat="server" ID="btnupdate" Text="Insert" OnClick="btnupdate_Click" ClientIDMode="Static" class="btn btn-info btn-sm" />
								</FooterTemplate>
								<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Large" ForeColor="White"></HeaderStyle>
							</asp:TemplateField>
						</Columns>
						<FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />

						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#A55129" Font-Bold="True" ForeColor="White"></HeaderStyle>
						<PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
						<RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
						<SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
						<SortedAscendingCellStyle BackColor="#FFF1D4" />
						<SortedAscendingHeaderStyle BackColor="#B95C30" />
						<SortedDescendingCellStyle BackColor="#F1E5CE" />
						<SortedDescendingHeaderStyle BackColor="#93451F" />
					</asp:GridView>
				</div>
				<div>
					<%--<asp:Button runat="server" ID="btnnew" OnClick="btnnew_Click" Text="New" class="btn btn-info btn-sm" Style="margin-left: 15px;" />--%>
				</div>
			</div>

		</ContentTemplate>
	</asp:UpdatePanel>
	<script type="text/javascript">
		$(document).ready(function () {
			var winHeight = $(window).height();
			if (winHeight < 650) {
				winHeight = (winHeight - 150);
			} else {
				winHeight = (winHeight - 200);

			}
			console.log(winHeight);
			$("#griddiv").height(winHeight);
		});

		$("#btnsave").click(function () {
			var rowscount = $("#<%=gridviewmachinesubsystem.ClientID %> tr").length;
			console.log(rowscount);
			var i = 1;
			for (i = 1 ; i <= rowscount ; i++) {
				if ($("#txtEquipID").val() == "") {
					alert("Enter Equipment ID ar row" + i);
				}
				if ($("#txtEquipdetails").val() == "") {
					alert("Enter Equipment Details ar row" + i);
				}
			}
		})
		
		$("[id$=gridviewmachinesubsystem]").on("click", "td", function () {
			$(this).closest('tr').find('input[type=hidden]').val("true");
		});

		$("#btnupdate").click(function () {
			if ($("#txtfooterequipID").val() == "") {
				alert("Please Enter Equipment ID");
			}
			else if ($("#txtfooterequipmentdetails").val() == "") {
				alert("Please Enter Equipment Description");
			}
		});

		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(function () {
			$(document).ajaxStop($.unblockUI);
			var winHeight = $(window).height();
			if (winHeight < 650) {
				winHeight = (winHeight - 150);
			}
			else {
				winHeight = (winHeight - 200);
			}
			$("#griddiv").height(winHeight);

			$("#btnupdate").click(function () {
				if ($("#txtfooterequipID").val() == "") {
					alert("Please Enter Equipment ID");
				}
				else if ($("#txtfooterequipmentdetails").val() == "") {
					alert("Please Enter Equipment Description");
				}
			})
			$("[id$=gridviewmachinesubsystem]").on("click", "td", function () {
				$(this).closest('tr').find('input[type=hidden]').val("true");
			});


			$("#btnsave").click(function () {
				var rowscount = $("#<%=gridviewmachinesubsystem.ClientID %> tr").length;
				
				var i = 0;
				for (i = 1 ; i <= rowscount ; i++) {
					if ($("#txtEquipID").val() == "") {
						alert("Enter Equipment ID ar row" + i);
					}
					if ($("#txtEquipdetails").val() == "") {
						alert("Enter Equipment Details ar row" + i);
					}
				}
			})
		})
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
