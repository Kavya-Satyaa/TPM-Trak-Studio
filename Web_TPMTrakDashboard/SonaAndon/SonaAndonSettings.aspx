<%@ Page Title="" Language="C#" MasterPageFile="~/SonaAndon/SonaSite.Master" AutoEventWireup="true" CodeBehind="SonaAndonSettings.aspx.cs" Inherits="SonaAndon.SonaAndonSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		.TableHeader {
			background-color: white;
			color: black;
			font-size: large;
			font-weight: 600;
			font-family: Verdana;
			text-align: Center;
			font-style: normal;
			border-color: white;
			border-width: 2px;
			border: 2px solid black;
			/*line-height: 60px;*/
			height: 100px;
		}

		.TableData {
			background-color: #DCE7F5;
			color: black;
			font-family: Courier New;
			font-size: medium;
			font-weight: bolder;
			text-align: Left;
			width: 100%;
		}

		.Header {
			color: White;
			font-family: Courier New;
			font-size: medium;
			font-weight: bold;
			text-align: center;
		}

		.Datatd {
			width: 500px;
		}
		.button {
  display: inline-block;
  font-size: 24px;
    text-align: center;
  text-decoration: none;
  outline: none;
  color: #fff;
  background-color: #4CAF50;
  border: none;
}
		.auto-style1 {
			margin-left: 40px;
		}
		.panel-heading {
    color: #ffffff;
    background-color: #428bca;
    border-color: #428bca;
}
	</style>
	<div>

		<div style="margin-left: 400px; margin-top: 50px">
			<asp:UpdatePanel runat="server">
				<ContentTemplate>
				
					<table class="TableHeader">
						<tr>
							<td colspan="2" style="height:20px">
								<div class="panel-heading tabHeader">Andon Settings</div>
							</td>
						</tr>
						<tr style="height:20px">
							<td>Font Size
							</td>
							<td>
								<asp:DropDownList runat="server" ID="ddlfontsize" Width="180px">
									<asp:ListItem Value="05">05</asp:ListItem>
									<asp:ListItem Value="06">06</asp:ListItem>
									<asp:ListItem Value="07">07</asp:ListItem>
									<asp:ListItem Value="08">08</asp:ListItem>
									<asp:ListItem Value="09">09</asp:ListItem>
									<asp:ListItem Value="10">10</asp:ListItem>
									<asp:ListItem Value="11">11</asp:ListItem>
									<asp:ListItem Value="12">12</asp:ListItem>
									<asp:ListItem Value="13">13</asp:ListItem>
									<asp:ListItem Value="14">14</asp:ListItem>
									<asp:ListItem Value="15">15</asp:ListItem>
									<asp:ListItem Value="16">16</asp:ListItem>
									<asp:ListItem Value="17">17</asp:ListItem>
									<asp:ListItem Value="18">18</asp:ListItem>
									<asp:ListItem Value="19">19</asp:ListItem>
									<asp:ListItem Value="20">20</asp:ListItem>
									<asp:ListItem Value="21">21</asp:ListItem>
									<asp:ListItem Value="22">22</asp:ListItem>
									<asp:ListItem Value="23">23</asp:ListItem>
									<asp:ListItem Value="24">24</asp:ListItem>
									<asp:ListItem Value="25">25</asp:ListItem>
									<asp:ListItem Value="26">26</asp:ListItem>
									<asp:ListItem Value="27">27</asp:ListItem>
									<asp:ListItem Value="28">28</asp:ListItem>
									<asp:ListItem Value="29">29</asp:ListItem>
									<asp:ListItem Value="30">30</asp:ListItem>
									<asp:ListItem Value="31">31</asp:ListItem>
									<asp:ListItem Value="32">32</asp:ListItem>
									<asp:ListItem Value="33">33</asp:ListItem>
									<asp:ListItem Value="34">34</asp:ListItem>
									<asp:ListItem Value="35">35</asp:ListItem>
									<asp:ListItem Value="36">36</asp:ListItem>
									<asp:ListItem Value="37">37</asp:ListItem>
									<asp:ListItem Value="38">38</asp:ListItem>
									<asp:ListItem Value="39">39</asp:ListItem>
									<asp:ListItem Value="40">40</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr style="height:20px">
							<td>Header Font Size
							</td>
							<td>
								<asp:DropDownList runat="server" ID="ddlHeaderFontSize" Width="180px">
									<asp:ListItem Value="05">05</asp:ListItem>
									<asp:ListItem Value="06">06</asp:ListItem>
									<asp:ListItem Value="07">07</asp:ListItem>
									<asp:ListItem Value="08">08</asp:ListItem>
									<asp:ListItem Value="09">09</asp:ListItem>
									<asp:ListItem Value="10">10</asp:ListItem>
									<asp:ListItem Value="11">11</asp:ListItem>
									<asp:ListItem Value="12">12</asp:ListItem>
									<asp:ListItem Value="13">13</asp:ListItem>
									<asp:ListItem Value="14">14</asp:ListItem>
									<asp:ListItem Value="15">15</asp:ListItem>
									<asp:ListItem Value="16">16</asp:ListItem>
									<asp:ListItem Value="17">17</asp:ListItem>
									<asp:ListItem Value="18">18</asp:ListItem>
									<asp:ListItem Value="19">19</asp:ListItem>
									<asp:ListItem Value="20">20</asp:ListItem>
									<asp:ListItem Value="21">21</asp:ListItem>
									<asp:ListItem Value="22">22</asp:ListItem>
									<asp:ListItem Value="23">23</asp:ListItem>
									<asp:ListItem Value="24">24</asp:ListItem>
									<asp:ListItem Value="25">25</asp:ListItem>
									<asp:ListItem Value="26">26</asp:ListItem>
									<asp:ListItem Value="27">27</asp:ListItem>
									<asp:ListItem Value="28">28</asp:ListItem>
									<asp:ListItem Value="29">29</asp:ListItem>
									<asp:ListItem Value="30">30</asp:ListItem>
									<asp:ListItem Value="31">31</asp:ListItem>
									<asp:ListItem Value="32">32</asp:ListItem>
									<asp:ListItem Value="33">33</asp:ListItem>
									<asp:ListItem Value="34">34</asp:ListItem>
									<asp:ListItem Value="35">35</asp:ListItem>
									<asp:ListItem Value="36">36</asp:ListItem>
									<asp:ListItem Value="37">37</asp:ListItem>
									<asp:ListItem Value="38">38</asp:ListItem>
									<asp:ListItem Value="39">39</asp:ListItem>
									<asp:ListItem Value="40">40</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr style="height:20px">
							<td>Font Family
							</td>
							<td>
								<asp:DropDownList runat="server" ID="ddlfontfamily" Width="180px">
									<asp:ListItem Value="Aharoni" >Aharoni</asp:ListItem>                                       
									<asp:ListItem Value="Arial" >Arial</asp:ListItem>                                      
									<asp:ListItem Value="Arial Black" >Arial Black</asp:ListItem>                                      
									<asp:ListItem Value="Baskerville Old Face" >Baskerville Old Face</asp:ListItem>                                      
									<asp:ListItem Value="Bodoni MT Black" >Bodoni MT Black</asp:ListItem>                                      
									<asp:ListItem Value="Bookman Old Style" >Bookman Old Style</asp:ListItem>                                      
									<asp:ListItem Value="Calibri" >Calibri</asp:ListItem>                                      
									<asp:ListItem Value="Cooper Black" >Cooper Black</asp:ListItem>                                      
									<asp:ListItem Value="Californian FB" >Californian FB</asp:ListItem>                                      
									<asp:ListItem Value="Constantia" >Constantia</asp:ListItem>                                      
									<asp:ListItem Value="Elephant" >Elephant</asp:ListItem>                                      
									<asp:ListItem Value="Georgia" >Georgia</asp:ListItem>                                      
									<asp:ListItem Value="Goudy Old Style" >Goudy Old Style</asp:ListItem>                                      
									<asp:ListItem Value="High Tower Text" >High Tower Text</asp:ListItem>                                      
									<asp:ListItem Value="Segoe UI" >Segoe UI</asp:ListItem>                                      
									<asp:ListItem Value="Segoe UI Semibold" >Segoe UI Semibold</asp:ListItem>                                      
									<asp:ListItem Value="Segoe WP Black" >Segoe WP Black</asp:ListItem>                                      
									<asp:ListItem Value="Times New Roman" >Times New Roman</asp:ListItem>                                      
									<asp:ListItem Value="Tw Cen MT" >Tw Cen MT</asp:ListItem>
									<asp:ListItem Value="Verdana" >Verdana</asp:ListItem>
									<asp:ListItem Value="Vrinda" >Vrinda</asp:ListItem>
								</asp:DropDownList>

							</td>
						</tr>
						<tr style="height:20px">
							<td>Font Style
							</td>
							<td>
								<asp:DropDownList runat="server" ID="ddlfontstyle" Width="180px" >
										<asp:ListItem Value="Regular" >Regular</asp:ListItem>
										<asp:ListItem Value="Bold" >Bold</asp:ListItem>
										<asp:ListItem Value="Italic" >Italic</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr style="height:20px">
							<td>Number of Rows to display
							</td>
							<td>
								<asp:DropDownList runat="server" ID="ddlrows" Width="180px" >
									<asp:ListItem Value="5">5</asp:ListItem>
									<asp:ListItem Value="6">6</asp:ListItem>
									<asp:ListItem Value="7">7</asp:ListItem>
									<asp:ListItem Value="8">8</asp:ListItem>
									<asp:ListItem Value="9">9</asp:ListItem>
									<asp:ListItem Value="10">10</asp:ListItem>
									<asp:ListItem Value="11">11</asp:ListItem>
									<asp:ListItem Value="12">12</asp:ListItem>
									<asp:ListItem Value="13">13</asp:ListItem>
									<asp:ListItem Value="14">14</asp:ListItem>
									<asp:ListItem Value="15">15</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<%--<tr style="height:20px">
							<td>Flip time in (Sec)
							</td>
							<td>
								<asp:DropDownList runat="server" ID="DropDownList1" Width="180px" >
									<asp:ListItem Value="50">50</asp:ListItem>
									<asp:ListItem Value="6"></asp:ListItem>
									<asp:ListItem Value="7">7</asp:ListItem>
									<asp:ListItem Value="8">8</asp:ListItem>
									<asp:ListItem Value="9">9</asp:ListItem>
									<asp:ListItem Value="10">10</asp:ListItem>
									<asp:ListItem Value="11">11</asp:ListItem>
									<asp:ListItem Value="12">12</asp:ListItem>
									<asp:ListItem Value="13">13</asp:ListItem>
									<asp:ListItem Value="14">14</asp:ListItem>
									<asp:ListItem Value="15">15</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>--%>
						<tr>
							<td>
								Header Background Color
							</td>
							<td>
								<asp:TextBox runat="server" ID="headercolor" TextMode="Color" Width="180px" Height="30" />
							</td>
						</tr>
							<tr>
							<td>
								Row Background Color
							</td>
							<td>
								<asp:TextBox runat="server" ID="RowBackgroundColor" TextMode="Color" Width="180px" Height="30"/>
							</td>
						</tr>
							<tr>
							<td>
								Alternative Row Background Color
							</td>
							<td>
								<asp:TextBox runat="server" ID="AlternativeRowBackgroundColor" TextMode="Color" Width="180px"  Height="30"/>
							</td>
						</tr>
					</table>
					<div style="height:50px;margin-left:200px;margin-top:10px">
						<asp:Button runat="server" Text="Save" ID="btnsave" CssClass="btn btn-primary" OnClick="btnsave_Click" Height="40"/>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div>
		</div>
	</div>
</asp:Content>
