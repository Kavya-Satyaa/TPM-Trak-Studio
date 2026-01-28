<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.AndonScreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css" media="screen">

         .HeaderImage {
            flex: 1;
            float: left;
        }

        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }


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

        .part3 {
            display: inline-block;
            vertical-align: bottom;
            height: 85%;
            font-family: Cambria;
            float: right;
            margin-right: 3em;
        }
        .downstyle tr:nth-child(odd) 
        {
            background-color: lightgray; 
        }
        .downstyle tr:nth-child(even)
        { 
            background-color: white;
        }
        .menuicon
        {
            height:20px;
            width:29px;
        }
        .vertview
        {
            width: 50%; 
            display:inline-block;
            float:left;
        }
        /*.horview{ 
            width: 100%; 
        }*/
       .table-bordered {
            border: 1px solid white;
        }

       .tableview>tbody>tr>td
       {
           padding:2px 8px 2px 8px;
       }
        </style>
        <script src="../AndonScripts/jquery-3.1.0.min.js"></script>
        <script src="../AndonScripts/bootstrap.min.js"></script>
        <link href="../AndonContent/bootstrap-glyphicons.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
          <div class="container-fluid" >
            <div class="row" >
                <div class="col-lg-12">
                    <div>
                    <div class="navbar navbar-fixed-top text-center" style="background-color: #1a2732;" id="menu" >
                        <div class="HeaderImage">
                            <asp:Image ID="companyLogo" runat="server" CssClass="companyicon" ImageUrl="/Image/KTAlogo.jpg" />
                        </div>

                        <span id="headerName" style="color: white; font-weight: bold; font-size: 30px; text-align: right; margin: auto; line-height: 60px;margin-left:8em;"> Andon</span>
                         <div class="part3">
                            <table >
                                <tr>
                                  <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                           <asp:Label runat="server" ID="lblRefreshTime" ClientIDMode="Static" ForeColor="White" Text="Last Refresh Time:" style="font-size: 15px"> </asp:Label></td>
                                           <asp:Label runat="server" ID="txtRefreshTime" ClientIDMode="Static" ForeColor="White" Text="12:00:00"> </asp:Label></td>
                                    </ContentTemplate>
                                  </asp:UpdatePanel>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlPlant" Style="width: 150px; max-width: 140px;height: 24px;padding: 2px 12px;" runat="server"
                                            CssClass="select form-control dropdowncss" ToolTip="Select Plant" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="padding:5px;">
                                        <asp:DropDownList ID="ddlCell" Style="width: 150px; max-width: 140px;height: 24px;padding: 2px 12px;" runat="server"
                                            CssClass="select form-control dropdowncss" ToolTip="Select CellID" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged"  AutoPostBack="true" >
                                        </asp:DropDownList>
                                    </td>
                                    <td style=" padding-left: 14px;">
                                          <asp:HyperLink  runat="server" ID="btnSave" NavigateUrl="~/KTASpindle/AndonSettings.aspx" >
                                             <asp:Image ID="settingICon" runat="server" CssClass="menuicon" ImageUrl="/Image/menu.jpg" />
                                          </asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </div>

                       
                    </div>
                    </div>
                </div>

                <div class="col-lg-12" style="margin-top: 70px;">
                 <asp:UpdatePanel runat="server">
		     	  <ContentTemplate>
                    <div id="machineContainer" style="height: 130vh; overflow: hidden">
                                <asp:ListView runat="server" ID="lvMachineData" ClientIDMode="Static">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder runat="server" ID="itemplaceholder" />
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                         <div class="machItem" style="margin: 15px; margin-bottom: 0px;">
                                            <div>
                                                <table class="table table-bordered tblMachineHeader" >
                                                    <tr style="background-color:#0C2D48;color:white;font-weight:bold;">
                                                        <td colspan="3" style="text-align:center;">
                                                             <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("MachineID") %>' CssClass="HeaderFont"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding:0px;">
                                                           <asp:ListView runat="server" ID="lvComponent" ClientIDMode="Static" DataSource='<%#Eval("CompList") %>' ItemPlaceholderID="addressPlaceHolder_2">
                                                           <LayoutTemplate>
                                                                 <table runat="server" class="table table-bordered tableview" style=" margin: 0px; background-color: #EE6C4D">
                                                                    <tr id="addressPlaceHolder_2" runat="server" >
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                             <ItemTemplate>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblAE" Text='AE%' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblAEValue" Text='<%# Eval("AE") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblPE" Text='PE%' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblPEValue" Text='<%# Eval("PE") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblQE" Text='QE%' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblQEValue" Text='<%# Eval("QE") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblOEE" Text='OEE%' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblOEEValue" Text='<%# Eval("OEE") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblComponent" Text='Component' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblComponentValue" Text='<%# Eval("Component") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblOperation" Text='Operation' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblOperationValue" Text='<%# Eval("Operation") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblActualCount" Text='Actual Count' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblActualCountValue" Text='<%# Eval("ActualCount") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblOperator" Text='Operator' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblOperatorValue" Text='<%# Eval("Operator") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                  <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblLastCycleEnd" Text='Last Cycle End' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblLastCycleEndValue" Text='<%# Eval("LastCycleEnd") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                  <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblDownTime" Text='Down Time' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblDownTimeValue" Text='<%# Eval("DownTime") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                                  <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblUtilizedTime" Text='Utilized Time' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblUtilizedTimeValue" Text='<%# Eval("UtilizedTime") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                             </ItemTemplate>
                                                         </asp:ListView>
                                                        </td>
                                                        <td style="padding:0px;background-color: #9ec7e8">
                                                          <asp:ListView runat="server" ID="lvtTargetList" ClientIDMode="Static" DataSource='<%#Eval("TargetList") %>' ItemPlaceholderID="addressPlaceHolder_3">
                                                           <LayoutTemplate>
                                                                 <table runat="server" class="table table-bordered tableview" style="margin: 0px;background-color: #9ec7e8" >
                                                                     <tr style="font-weight:bold;background-color: #0074B7;color:white">                                                                                                                 
                                                                         <td>
                                                                            <asp:Label runat="server" ID="lblTarget" Text="Target/HR" CssClass="HeaderFont" />
                                                                         </td>
                                                                         <td>
                                                                              <asp:Label runat="server" ID="lblVal" Text="20" CssClass="HeaderFont"/>
                                                                         </td>
                                                                     </tr>
                                                                      <tr style="font-weight:bold;background-color: #60A3D9;color:white">                                                                                                               
                                                                         <td>
                                                                          <asp:Label runat="server" ID="lblHr" Text="HR" CssClass="HeaderFont"/>
                                                                         </td>
                                                                         <td>
                                                                              <asp:Label runat="server" ID="lblActualQty" Text="Actual Qty" CssClass="HeaderFont"/>
                                                                         </td>
                                                                     </tr>
                                                                    <tr id="addressPlaceHolder_3" runat="server" >
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                             <ItemTemplate>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblAE" Text='<%# Eval("Hr") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblAEValue" Text='<%# Eval("ActualQty") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                 </tr>
                                                             </ItemTemplate>
                                                         </asp:ListView>
                                                        </td>
                                                        <td style="padding:0px;">
                                                           <asp:ListView runat="server" ID="lstDownCode" ClientIDMode="Static" DataSource='<%#Eval("DownList") %>' ItemPlaceholderID="addressPlaceHolder_4">
                                                           <LayoutTemplate>
                                                                 <table runat="server" class="table table-bordered downstyle tableview" style="margin: 0px; background-color: unset">
                                                                     <tr style="font-weight:bold;background-color:#2e6886;color:white">                                                                                                                
                                                                         <td>
                                                                            <asp:Label runat="server" ID="lblDownCode" Text="Top 5 DownCode" CssClass="HeaderFont"/>
                                                                         </td>
                                                                         <td>
                                                                              <asp:Label runat="server" ID="lblTime" Text="Time" CssClass="HeaderFont"/>
                                                                         </td>
                                                                     </tr>
                                                                     <tr id="addressPlaceHolder_4" runat="server">
                                                                     </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                             <ItemTemplate>
                                                                 <tr>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblTop5Down" Text='<%# Eval("DownCode") %>' CssClass="ContentFont"/>
                                                                     </td>
                                                                     <td>
                                                                         <asp:Label runat="server" ID="lblTime" Text='<%# Eval("Time") %>' CssClass="ContentFont" />
                                                                     </td>
                                                                 </tr>
                                                                 
                                                             </ItemTemplate>
                                                               
                                                              
                                                         </asp:ListView>
                                                        </td>
                                                    </tr>
                                            </div>
                                          </div>
                                   </ItemTemplate>
                                </asp:ListView>
                    </div>
                    <div>
					  <asp:Timer runat="server" ID="timer" Enabled="false" OnTick="timer_Tick" />
				   </div>
                   </ContentTemplate>
                  </asp:UpdatePanel>
                </div>

            </div>
          </div>
        <script type="text/javascript">
            function setsetting() {
                var fontsizeheader = "<%=HeaderFontsize %>";
                var fontsizecontent = "<%=ContentFontsize %>";
                var DisplayType="<%=displaytype %>"
                $('.HeaderFont').css('font-size', fontsizeheader +"px");
                $(".ContentFont").css('font-size', fontsizecontent + "px");

                if (DisplayType == "HorizontalView") {
                    $(".tblMachineHeader").removeClass("vertview");
                }
                else if (DisplayType == "VerticalView") {
                    $(".tblMachineHeader").addClass(" vertview");
                }
                else {
                     $(".tblMachineHeader").removeClass("vertview");
                }
		   }

            $(document).ready(function () {
                setsetting();
            });
		    var prm = Sys.WebForms.PageRequestManager.getInstance();
		    prm.add_endRequest(function () {
			    setsetting();
		    });
        </script>
    </form>
</body>
</html>
