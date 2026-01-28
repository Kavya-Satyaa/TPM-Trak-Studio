<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShantiAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.ShantiAndon" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <style type="text/css">
        .dark-bg {
            background: #1a2732;
            border-bottom: 1px solid #f1f2f7;
        }

        .dark-bg {
            background: #1a2732;
            border-bottom: 1px solid #f1f2f7;
        }

        .header {
            /*position: fixed;*/
            left: 0;
            right: 0;
            /*z-index: 1002;*/
            padding: 0px 6px;
        }

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
</head>
<body style="padding: 0px; margin: 0px; background-color: #202648">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
       <div >
            <div class="row" style=" height: 80px;border-style:double;border-color:white; margin:5px;">
                <div class="col-lg-12">
                    <header class="header dark-bg" >
                        <div class="col-lg-2" style="width: 20%; float: left; margin-left: 0px;">

                            <img runat="server" src="~/Images/logo/AMITLogo.png" id="toggle" class="img-responsive img-rounded" alt="Logo" style="width: 104px; cursor: pointer; padding-right: 1px; margin-top: 7px; margin-bottom: 3px;float:left">
                        </div>
                        <div class="col-lg-6" style="text-align: center; width: 50%;margin-top: 10px; float: left; font-size: 30px; font-weight: 900; color: white">
                            Production Andon
                        </div>
                        <div style="width: 400px; margin-top: 5px; margin-right: 0px; float: right" class   ="col-lg-4">

                            <div style="float: right;">
                                 <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <p runat="server" class="headerRight" id="lblDatetime" style="color: white; font-size: 20px;margin-bottom:0px">&nbsp;&nbsp;</p>
                                <asp:DropDownList runat="server" ID="ddlPlantID" CssClass="form-control" style="width:auto;margin-bottom:4px;font-size:18px" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged"></asp:DropDownList>
                                 </ContentTemplate>
                                     <Triggers>
                                         <asp:AsyncPostBackTrigger ControlID="timer"  EventName="Tick"/>
                                         <asp:AsyncPostBackTrigger ControlID="ddlPlantID"  EventName="SelectedIndexChanged"/>
                                     </Triggers>
                        </asp:UpdatePanel>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
           
            <div class="row" style="margin-top:20px">
                <div class="col-lg-12">
                    <link href="../Content/Ionic.css" rel="stylesheet" />
                    <div style="width: 100%; padding: 0px">
                       
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div style="width: 100%; padding: unset">
                                    <asp:ListView runat="server" ID="listviewShantiAndon">
                                        <LayoutTemplate>
                                            <table id="table" class="TableHeader" border="1" style="border-color: #1C4C86; border-width: 5px; padding: 10px">
                                                <tr>
                                                    <th style="min-width: 15%; white-space: nowrap; padding: 10px; text-align: center;">Machine ID</th>
                                                   <%-- <th style="min-width: 15%; white-space: nowrap; padding: 10px; text-align: center;">Machine Description</th>--%>
                                                     <th style="min-width: 15%; white-space: nowrap; padding: 10px; text-align: center;">Utilised Time</th>
                                                    <th style=" text-align: center;">Running<br /> Component</th>
                                                    <th style="text-align: center; padding-right: 5px;">Running Opn</th>
                                                    <th style="text-align: center; padding-right: 5px;">Running SI. No.</th>
                                                    <th style="text-align: center; padding-right: 5px;">Heat Code</th>
                                                    <th style="text-align: center; padding-right: 5px;">Current Operator</th>
                                                    <th style="white-space: nowrap; text-align: center;" >Avg. Cycle<br /> Time</th>
                                                    <th style="white-space: nowrap; text-align: center;">&nbsp;AE %&nbsp;</th>
                                                    <th style="white-space: nowrap; text-align: center;">&nbsp;PE %&nbsp;</th>
                                                    <th style="white-space: nowrap; text-align: center;">&nbsp;QE %&nbsp;</th>
                                                      <th style="white-space: nowrap; text-align: center;">&nbsp;OEE %&nbsp;</th>
                                                    <th style="white-space: nowrap; text-align: center;">Rejected<br />Qty</th>
                                                    <th style="white-space: nowrap; text-align: center;">Part <br />Count</th>
                                                </tr>
                                                <tr id="ItemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="tr" class="TableData">
                                                <td style="min-width: 15%; padding: 10px; white-space: nowrap;">
                                                    <asp:Label runat="server" ID="lblWorkCenter" Text='<%# Bind("MachineID") %>' /></td>
                                                <td style="min-width: 15%; padding: 10px; white-space: nowrap;">
                                                 <%--   <asp:Label runat="server" ID="Label1" Text='<%# Bind("MachineDescription") %>' /></td>--%>
                                                       <asp:Label runat="server" ID="Label1" Text='<%# Bind("UtilisedTime") %>' /></td>
                                                <td style="white-space: nowrap; padding-left: 5px">
                                                    <asp:Label runat="server" ID="lblPartNumber" Text='<%# Bind("RunningComponent") %>' /></td>
                                                <td style="padding-left: 5px; text-align: right; padding-right: 5px;">
                                                    <asp:Label runat="server" ID="lblScheduleqty" Text='<%# Bind("RunningOpn") %>' /></td>
                                                <td style="padding-left: 5px; text-align: right; padding-right: 5px;">
                                                    <asp:Label runat="server" ID="lbltarget" Text='<%# Bind("RunningSlNo") %>' /></td>
                                                <td style="padding-left: 5px; text-align: right; padding-right: 5px;">
                                                    <asp:Label runat="server" ID="lblCompletedTime" Text='<%# Bind("HeatCode") %>' /></td>
                                                <td style="padding-left: 5px; text-align: right; padding-right: 5px;">
                                                    <asp:Label runat="server" ID="lblOEE" Text='<%# Bind("CurrentOperator") %>' /></td>
                                                <td style="padding-left: 5px; text-align: left; padding-right: 5px; width: auto">
                                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Bind("AvgCycletime") %>' />
                                                </td>
                                            
                                                <td style="padding-left: 5px; white-space: nowrap">
                                                    <asp:Label runat="server" ID="Label3" Text='<%# Bind("AE") %>' /></td>
                                                <td style="padding-left: 5px; white-space: nowrap">
                                                    <asp:Label runat="server" ID="Label4" Text='<%# Bind("PE") %>' /></td>
                                                <td style="padding-left: 5px; white-space: nowrap">
                                                    <asp:Label runat="server" ID="Label5" Text='<%# Bind("QE") %>' /></td>
                                                <td style="padding-left: 5px; white-space: nowrap">
                                                    <asp:Label runat="server" ID="Label6" Text='<%# Bind("OEE") %>' /></td>
                                                <td style="padding-left: 5px; white-space: nowrap">
                                                    <asp:Label runat="server" ID="Label7" Text='<%# Bind("RejectCount") %>' /></td>
                                                <td style="padding-left: 5px; white-space: nowrap">
                                                    <asp:Label runat="server" ID="Label8" Text='<%# Bind("Components") %>' /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                                <div>
                                    <asp:Timer runat="server" ID="timer" Enabled="false" OnTick="timer_Tick" />
                                   
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="timer"  EventName="Tick"/>
                                  <asp:AsyncPostBackTrigger ControlID="ddlPlantID"  EventName="SelectedIndexChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript"> 
        function setcolor() {
            var fontsize =<%=fontsize %>;
            var fontsizeheader = "<%=Headerfontsize %>"
            <%--  var fontfamily ="<%=fontfamily %>"
            var fontstyle = "<%=fontstyle %>"
            var backgroundcolor = "<%=background %>"
            var Alternativebackgroundcolor = "<%=Alternativebackground %>"
            var Headerbackgroundcolor = "<%=Headerbackgroundcolor %>"--%>
            $("#table").css('font-size', fontsizeheader);
            $("tr").css('font-size', fontsize);
            // $("#table").css('font-family', fontfamily);
            // $("#table").css('font-style', fontstyle);
            //$("tr").css('font-style', fontstyle);
            // $("tr").css('font-family', fontfamily);
            // $("tr:even").css("background-color", backgroundcolor);
            // $("tr:odd").css("background-color", Alternativebackgroundcolor);
            // $("tr:first").css("background-color", Headerbackgroundcolor);
            $("tr:first").css("font-size", fontsizeheader);
        }
        $(document).ready(function () {
            setcolor();
            $("#divbody").css("padding", "0px")
        })
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            setcolor();
        });
    </script>
</body>
</html>
