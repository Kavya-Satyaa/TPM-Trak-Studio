<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.AndonSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <style>
        #menu {
            position: fixed;
            top: 0;
            background: #1a2732;
            box-shadow: 0 0 10px black;
        }

        .companyicon {
            height: 60px;
            clip: rect(10px,10px,10px,10px);
            min-width: 100px;
        }
          .HeaderImage {
            flex: 1;
            float: left;
            width: 0px;
        }

        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }
   
       .table>tbody>tr>td{
          vertical-align:middle;
       }
       .table{
           	border-color: white;
			border-width: 2px;
			border: 2px solid black;
        }
       .btnview
       {
           width: 60%;
           float: right;
       }
     
    </style>
     <script src="../AndonScripts/jquery-3.1.0.min.js"></script>
     <script src="../AndonScripts/bootstrap.min.js"></script>
     <link href="../AndonContent/bootstrap-glyphicons.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div>
           <div class="container-fluid" >
            <div class="row" >
                <div class="col-lg-12">
                    <div>
                    <div class="navbar navbar-fixed-top text-center" style="background-color: #1a2732;" id="menu" >
                        <div class="HeaderImage">
                            <asp:Image ID="companyLogo" runat="server" CssClass="companyicon" ImageUrl="/Image/KTAlogo.jpg" />
                        </div>
                        <span id="headerName" style="color: white; font-weight: bold; font-size: 30px; margin: auto; line-height: 60px;"> Andon Settings</span>
                        
                    </div>
                    </div>
                </div>
                <div style="margin-left: 400px; margin-top: 90px">
			        <asp:UpdatePanel runat="server">
				        <ContentTemplate>
                            <table class="table table-bordered" style="width:60%">
						        <tr style="height:15px">
							        <td>Header Font Size
							        </td>
                                   	<td>
							        	<asp:DropDownList runat="server" ID="ddlHeaderFontsize" Width="180px" CssClass="form-control">
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
                                 <tr>
                                       <td>
                                           Content Font Size
                                       </td>
                                       <td>
                                           <asp:DropDownList runat="server" ID="ddlContentFontSize" Width="180px" CssClass="form-control">
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
                                  <tr>
                                        <td>Flip time in (Sec)</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="flipTime"  Width="180px" Height="30" CssClass="form-control" />
                                        </td>
                                  </tr>
                                  <tr>
                                        <td>Top Down Code</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="downCode"  Width="180px" Height="30" CssClass="form-control" />
                                        </td>
                                  </tr>
                                  <tr>
                                    <td>Andon Display Type</td>
                                    <td>
                                         <asp:DropDownList runat="server" ID="ddlDisplayType" Width="180px" CssClass="form-control">
                                              <asp:ListItem Value="HorizontalView">Horizontal View</asp:ListItem>
									          <asp:ListItem Value="VerticalView">Vertical View</asp:ListItem>
									          <asp:ListItem Value="OneTypeView">OneType View</asp:ListItem>
                                          </asp:DropDownList>
                                    </td>
                                 </tr>
                                </table>
                            <div>
                                <table class="btnview">
                                    <tr>
                                        <td style="width:0px">
                                           <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="Save" Style="min-width: 100px;" OnClick="btnSave_Click" />
                                        </td>
                                        <td style="padding-left:10px">
                                              <asp:HyperLink  runat="server" Text=" View Andon" CssClass="btn btn-success" ID="ReturnAndon" NavigateUrl="~/KTASpindle/AndonScreen.aspx" >
                                              </asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                     </asp:UpdatePanel>
                </div>
               </div>
        </div>
            </div>
    </form>
</body>
</html>
