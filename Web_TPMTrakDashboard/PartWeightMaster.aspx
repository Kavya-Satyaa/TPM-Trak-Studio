<%@ Page Title="Part Weight Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PartWeightMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.PartWeightMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<%: Styles.Render("~/bundles/dateTimecss") %>
	<%: Scripts.Render("~/bundles/dateTimejs") %>
	<style>
		.GVFixedFooter {
			font-weight: bold;
			text-align: center;
			height: 50px;
			vertical-align: middle;
		}

		.table tbody > tr > td {
			vertical-align: middle;
		}
		.table tbody >tr>td> table{
			border-style:double;
		}
		.centerHeaderText th {
			text-align: center;
		}
		.Pager{
			border-style:none;
			text-align: -webkit-center;
			
		}
	</style>
	
			<div>
				<asp:Label runat="server" ID="lblMessages" />
			</div>
			<div style="margin-left: -10px; height: 70px; background-color: transparent; color: white" class="panel-heading tabHeader">
				<table border="1" style="border-color: white; border-width: 1px; height: 60px">
					<tr>
						<td style="width: 380px">
							<font color="white"><b>Component ID</b></font>
							<asp:TextBox runat="server" Height="30px" ID="txtComponentsearch" />
							<asp:Button runat="server" ID="btnview" Text="View" OnClick="btnview_Click" CssClass="btn btn-primary" />
						</td>
						<td style="width: 220px">
							<asp:FileUpload data-toggle="tooltip" ForeColor="White" CssClass="toolTip" ID="FileUpload1" runat="server" meta:resourcekey="FileUpload1Resource1" Width="220" />
						</td>
						<td style="width: 100px; text-align: center">
							<asp:Button runat="server" ID="btnimport" Text="Import" OnClick="btnimport_Click" CssClass="btn btn-primary" />
						</td>
						<td style="width: 95px; text-align: center">
							<asp:Button runat="server" ID="btnsave" Text="Save" OnClick="btnsave_Click" CssClass="btn btn-primary" />
						</td>
					</tr>
				</table>
			</div>
	<asp:UpdatePanel runat="server">
		<ContentTemplate>
			<div style="margin: 5px;id="divforgrid">
				<asp:GridView runat="server" ID="partweightgridview" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="#5391CA" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Large" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" ShowFooter="true" RowStyle-Wrap="false" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" ShowHeaderWhenEmpty="true" AllowPaging="true" OnPageIndexChanging="partweightgridview_PageIndexChanging" pagesize="12" >
					<HeaderStyle CssClass="centerHeaderText" />
					<FooterStyle CssClass="GVFixedFooter" />
					<PagerStyle CssClass="Pager" />
					<Columns>
						<asp:TemplateField HeaderText="Component ID" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Label runat="server" ID="lblComponent" Text='<%# Eval("ComponentID") %>' CssClass="control-label  ValiDate" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox runat="server" ID="lblfootercomponent" Style="vertical-align: middle" Height="34" />
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Unit Weight" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:HiddenField ID="hiddenfieltosave" runat="server" Value='<%# Eval("UnitWeight") %>' />
								<asp:TextBox runat="server" ID="lblunitweight" Text='<%# Eval("UnitWeight") %>' CssClass="control-label  ValiDate" />
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox runat="server" ID="footerUnitWeight" Style="vertical-align: middle" Height="34" />
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Effective Date" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Label runat="server" ID="lbleffectivedate" Text='<%# Eval("EffectiveDateTime") %>' CssClass="control-label  ValiDate" />
							</ItemTemplate>
							<FooterTemplate>
								<table style="border-style: hidden;">
									<tr>
										<td>
											<div class="col-md-12">
												<asp:TextBox ID="txtEffectivedate" runat="server" CssClass="form-control date1" placeholder="Effective Date" AutoCompleteType="Disabled"></asp:TextBox>
											</div>
										</td>
										<td>
											<asp:Button runat="server" ID="btnfootersave" Text="Save" OnClick="btnfootersave_Click" CssClass="btn btn-primary" />
										</td>
									</tr>
								</table>
							</FooterTemplate>
						</asp:TemplateField>
					</Columns>
					<EmptyDataTemplate>
						No data available.
					</EmptyDataTemplate>
					<HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Large" ForeColor="White" />
					<RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
				</asp:GridView>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

	<%-- <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>--%>
	<%: Styles.Render("~/bundles/dateTimecss") %>
	<%: Scripts.Render("~/bundles/dateTimejs") %>

	<%-- <!-- Include Date Range Picker -->
    <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>--%>
	<script type="text/javascript">
		$(document).ready(function () {
			$.unblockUI({});
			$(".date1").datetimepicker({
				format: 'DD-MM-YYYY HH:mm',
				locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
			});
		});
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(function () {
			$(".date1").datetimepicker({
				format: 'DD-MM-YYYY HH:mm',
				locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
