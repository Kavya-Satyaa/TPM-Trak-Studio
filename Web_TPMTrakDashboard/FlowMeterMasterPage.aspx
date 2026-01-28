<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FlowMeterMasterPage.aspx.cs" Inherits="Web_TPMTrakDashboard.FlowMeterMasterPage" %>

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
            /*line-height: 60px;*/
            /*height: 70px;*/
        }

        .button {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            width: 80px;
            height: 30px;
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
            font-size: x-large;
            font-weight: bold;
            text-align: center;
            overflow-wrap: break-word;
        }

        .subHeader {
            color: White;
            font-family: Courier New;
            font-size: large;
            font-weight: bold;
            text-align: center;
            overflow-wrap: break-word;
        }

        .subsubHeader {
            color: White;
            font-family: Courier New;
            font-size: larger;
            font-weight: bold;
            text-align: center;
            overflow-wrap: break-word;
        }

        th {
            border-style: solid;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnupload" />
            </Triggers>
            <ContentTemplate>
        <div>
            <div  style="height: 10%;text-align:left;display:inline-block;padding:10px;">
              
                <span style="font-size:medium;color:white">Component-ID</span>
            <%--<asp:TextBox runat="server" ID="ddldropdow" style="height:30px">
                <
            </asp:TextBox>--%>
                <input runat="server" type="text" id="ddldropdow" list="datalist"/>
                    <datalist id="datalist">

                    </datalist>
              
                            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" CssClass="button" Text="View"/>
                 
           </div>
      <div style="height: 10%;text-align:end;display:inline-block;padding:10px;">
         
          <span style="font-size:medium;color:white">Import</span>
           <asp:FileUpload runat="server" id="fileupload" style="display:inline-block;"/>
            <asp:Button runat="server" id="btnupload" style="display:inline-block;" CssClass="button" OnClick="btnUpload_Click" Text="Import"/>
       
      </div>
        
        </div>

        <div style="height: 70%;overflow:auto;padding:10px;overflow:auto;" id="divFlowMeterMaster">
            <asp:ListView runat="server" ID="lstflowmeterlistview"  class="Header"  OnPagePropertiesChanged="lstflowmeterlistview_PagePropertiesChanged">
                <LayoutTemplate>
                    <table id="Table1" runat="server" border="1" style="border-color: white;width:100%;height:140px; border-width: 2px;table-layout:fixed" >
                        <tr>
                            <th style="width:10%;text-align:center;overflow-wrap: break-word;" class="Header">
                                <span>Number</span>
                            </th>
                            <th style="text-align:center;width:5%;overflow-wrap: break-word;" class="Header">
                                <span>Type Dia</span>
                            </th>
                            <th style="width:47%">
                                <table style="border-color: black;width:100%;table-layout:fixed">
                                    <tr style="width:60%;">
                                        <th colspan='8' style="text-align:center">
                                            <span  class="Header" >HEAD CLEARANCE</span>
                                        </th>
                                    </tr>
                                    <tr style="width:100%;">
                                        <th colspan='4' class="subHeader" style="width:50%;text-align:center">
                                            <span>SETTING</span>
                                        </th>
                                        <th colspan='3' class="subHeader" style="width:37.5%;text-align:center">
                                            <span>ROTA FLOW VALUES</span>
                                        </th>
                                        <th  class="subHeader" rowspan="2" style="width:12.5%;text-align:center">
                                            <span>Remark</span>
                                        </th>
                                    </tr>
                                    <tr style="width:100%;">
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Angle</span> </br> <span style="font-size:13px;"> (Deg.)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Height</span> </br> <span style="font-size:13px"> (mm)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Height.</span> </br> <span >  Gauge</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Pr.</span> </br> <span style="font-size:13px"> (bar)</span>
                                        </th>
                                        <th  style="text-align:center;width:12.5%" class="subHeader">
                                            <span>MIN</span> </br> <span style="font-size:13px">(#cc/min)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>MAX </span> </br> <span style="font-size:13px"> (#cc/min)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>MEDIAN </span> </br> <span style="font-size:13px"> (** ±0.3 μm)</span>
                                        </th>
                                        
                                    </tr>
                                </table>
                            </th>
                            <th style="width:18%">
                                <table style="border-color: black;width:100%;height:100%" >
                                    <tr style="width:100%">
                                        <th style="width:100%;text-align:center" colspan='3'>
                                            <span class="Header">SHAFT CLEARANCE</span>
                                        </th>
                                    </tr>
                                    <tr style="width:100%;height:100%;">
                                        <th style="text-align:center;width:33.33%"; class="subHeader">
                                            <span>TEST Pr.</span> </br> <span style="font-size:13px">  (bar)</span>
                                        </th>
                                        <th  style="text-align:center;width:33.33%" class="subHeader">
                                            <span>ROTA FLOW</span> </br> <span style="font-size:13px"> (Max cc/min)</span>
                                        </th>
                                       <th style="text-align:center;width:33.33%" class="subHeader" rowspan="2">
                                            <span>Remark</span>
                                        </th>
                                    </tr>
                                  
                                </table>
                            </t>
                            <th style="width:10%;text-align:center" class="Header">
                                <span>Barrel Inspection on collar</span>
                            </th>
                            <th style="width:8%;text-align:center" class="Header">
                                <span>TGG Type</span>
                            </th>
                            <th style="width:2%;text-align:center" class="Header">
                                <span>Del</span>
                            </th>
                        </tr>
                        <tr id="ItemPlaceholder" runat="server">
                            </tr>
                        <tr style="text-align:center;">
                            <td colspan="12">
                                <asp:DataPager PageSize="19" ID="DataPager1"  runat="server" PagedControlID="lstflowmeterlistview">
                                   <Fields>
                                       <asp:NextPreviousPagerField ShowLastPageButton="false" ShowNextPageButton="false" ButtonType="Button"  />
                                       <asp:NumericPagerField ButtonType="Button" NumericButtonCssClass="btn" CurrentPageLabelCssClass="btn disabled"  />
                                       <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button"  />
                                   </Fields>
                                </asp:DataPager> 
                           </td>
                        </tr>
                        
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="tritem" style="text-align:center">
                        <td style="width:10%;text-align:center">
                            <asp:Label runat="server" ID="lblComponent" Text='<%# Bind ("Component") %>' Visible='<%# Bind("ComponentlblEnable") %>' style="color:white;font-size:medium;text-align:center" />
                            <asp:DropDownList runat="server" DataSource='<%# Bind("ComponentList") %>' Visible='<%# Bind("ComponentDrpEnable") %>' ID="ddlcomponentdropdown" style="width:100% ;padding:5px" />
                            <asp:HiddenField Value='<%# Bind("idd") %>' ID="idd" runat="server" />
                        </td>
                        <td style="width:2%;">
                            <asp:TextBox runat="server" Text='<%# Bind("TypeDia") %>' ID="txtTypeDia" style="width:100%;padding:2px" />
                            <asp:HiddenField runat="server" Value='<%# Bind("TypeDia") %>' ID="HiddTypeDia" />
                        </td>
                        <td style="width:47%">
                            <table style="width:100%">
                                <tr style="text-align:center">
                                    <td style="text-align:center">
                                        <asp:TextBox runat="server" Text='<%# Bind("SettingsAng") %>' ID="txtAngDeg" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("SettingsAng") %>' ID="HidAngDeg" />
                                    </td>
                                    <td >
                                        <asp:TextBox runat="server" onkeypress="return event.charCode >= 48 && event.charCode <= 57 || event.charCode <= 46" Text='<%# Bind("SettingsHt") %>' ID="txtHTMM" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("SettingsHt") %>' ID="HidHTMM" />
                                    </td>
                                    <td >
                                        <asp:TextBox runat="server" Text='<%# Bind("SettingsHTGuage") %>' ID="txtHTGUAGE"  style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("SettingsHTGuage") %>' ID="HidHTGUAGE" />
                                        </t>
                                    <td >
                                        <asp:TextBox runat="server" TextMode="Number" Text='<%# Bind("SettingsPR") %>' ID="txtPRBAR" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("SettingsPR") %>' ID="HidPRBAR" />
                                    </td>
                                    <td >
                                        <asp:TextBox runat="server" TextMode="Number" Text='<%# Bind("RotaMin") %>' ID="txtROTAMIN" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="width:100%;padding:2px"  />
                                        <asp:HiddenField runat="server" Value='<%# Bind("RotaMin") %>' ID="HidROTAMIN" />
                                    </td>
                                    <td >
                                        <asp:TextBox runat="server" TextMode="Number" Text='<%# Bind("RotaMax") %>' ID="txtRotaMax" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("RotaMax") %>' ID="HidRotaMax" />
                                    </td>
                                    <td >
                                        <asp:TextBox runat="server" TextMode="Number" Text='<%# Bind("RotaMedian") %>' ID="txtRotaMedian" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("RotaMedian") %>' ID="HidRotaMedian" />
                                    </td>
                                    <td >
                                        <asp:TextBox runat="server" Text='<%# Bind("SettingRemark") %>' ID="txtHeadRemark" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("SettingRemark") %>' ID="HidHeadremark" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:18px">
                            <table>
                                <tr >
                                    <td  >
                                        <asp:TextBox runat="server" TextMode="Number" Text='<%# Bind("ShaftTestPr") %>' ID="txtTestPr" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server"  Value='<%# Bind("ShaftTestPr") %>' ID="HidTestPr" />
                                    </td>
                                    <td  >
                                        <asp:TextBox runat="server" TextMode="Number" Text='<%# Bind("RotaFlow") %>' ID="txtRotaFlow" onkeypress="return event.charCode >= 48 && event.charCode <= 57" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("RotaFlow") %>' ID="HidRotaFlow" />
                                    </td>
                                       <td >
                                        <asp:TextBox runat="server" Text='<%# Bind("RotaRemark") %>' ID="txtShaftRemark" style="width:100%;padding:2px" />
                                        <asp:HiddenField runat="server" Value='<%# Bind("RotaRemark") %>' ID="HidShaftRemark" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:10px">
                            <asp:TextBox runat="server" Text='<%# Bind("BaralInspection") %>' ID="txtbarrelInspection" style="width:100%;padding:2px" />
                            <asp:HiddenField runat="server" Value='<%# Bind("BaralInspection") %>' ID="HidbarrelInspection" />
                        </td>
                        <td style="width:8%">
                            <asp:CheckBox runat="server" ID="chkIsTGG" Checked='<%# Bind("IsTGGType") %>' style="width:100%;padding:2px" />
                            <asp:HiddenField runat="server" Value='<%# Bind("IsTGGType") %>' ID="hdfIsTGG" />

                        </td>
                        <td style="width:2%">
                            <asp:CheckBox runat="server" ID="deleteFlowMeter" style="width:100%;padding:2px" />
                        </td>
                    </tr>
                </ItemTemplate>
                 <EmptyDataTemplate>
       <table id="Table1" runat="server" border="1" style="border-color: white;width:100%;height:140px; border-width: 2px;table-layout:fixed" >
                        <tr>
                            <th style="width:10%;text-align:center;overflow-wrap: break-word;" class="Header">
                                <span>Number</span>
                            </th>
                            <th style="text-align:center;width:5%;overflow-wrap: break-word;" class="Header">
                                <span>Type Dia</span>
                            </th>
                            <th style="width:47%">
                                <table style="border-color: black;width:100%;table-layout:fixed">
                                    <tr style="width:60%;">
                                        <th colspan='8' style="text-align:center">
                                            <span  class="Header" >HEAD CLEARANCE</span>
                                        </th>
                                    </tr>
                                    <tr style="width:100%;">
                                        <th colspan='4' class="subHeader" style="width:50%;text-align:center">
                                            <span>SETTING</span>
                                        </th>
                                        <th colspan='3' class="subHeader" style="width:37.5%;text-align:center">
                                            <span>ROTA FLOW VALUES</span>
                                        </th>
                                        <th  class="subHeader" rowspan="2" style="width:12.5%;text-align:center">
                                            <span>Remark</span>
                                        </th>
                                    </tr>
                                    <tr style="width:100%;">
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Angle</span> </br> <span style="font-size:13px;"> (Deg.)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Height</span> </br> <span style="font-size:13px"> (mm)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Height.</span> </br> <span >  Gauge</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>Pr.</span> </br> <span style="font-size:13px"> (bar)</span>
                                        </th>
                                        <th  style="text-align:center;width:12.5%" class="subHeader">
                                            <span>MIN</span> </br> <span style="font-size:13px">(#cc/min)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>MAX </span> </br> <span style="font-size:13px"> (#cc/min)</span>
                                        </th>
                                        <th   style="text-align:center;width:12.5%" class="subHeader">
                                            <span>MEDIAN </span> </br> <span style="font-size:13px"> (** ±0.3 μm)</span>
                                        </th>
                                        
                                    </tr>
                                </table>
                            </th>
                            <th style="width:18%">
                                <table style="border-color: black;width:100%;height:100%" >
                                    <tr style="width:100%">
                                        <th style="width:100%;text-align:center" colspan='3'>
                                            <span class="Header">SHAFT CLEARANCE</span>
                                        </th>
                                    </tr>
                                    <tr style="width:100%;height:100%;">
                                        <th style="text-align:center;width:33.33%"; class="subHeader">
                                            <span>TEST Pr.</span> </br> <span style="font-size:13px">  (bar)</span>
                                        </th>
                                        <th  style="text-align:center;width:33.33%" class="subHeader">
                                            <span>ROTA FLOW</span> </br> <span style="font-size:13px"> (Max cc/min)</span>
                                        </th>
                                       <th style="text-align:center;width:33.33%" class="subHeader" rowspan="2">
                                            <span>Remark</span>
                                        </th>
                                    </tr>
                                  
                                </table>
                            </t>
                            <th style="width:10%;text-align:center" class="Header">
                                <span>Barrel Inspection on collar</span>
                            </th>
                             <th style="width:8%;text-align:center" class="Header">
                                <span>Is TGG</span>
                            </th>
                            <th style="width:2%;text-align:center" class="Header">
                                <span>Del</span>
                            </th>
                        </tr>
                      
                    </table>
         </EmptyDataTemplate>
            </asp:ListView>
        </div>
                <div  style="height: 70%;overflow:auto;padding:10px;text-align:end;">
                      <asp:Button runat="server" Text="Add" ID="btnAdd" OnClick="btnAdd_Click" CssClass="button"/>
                       <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" ClientIDMode="Static" CssClass="button"/>
                         <asp:Button runat="server" Text="Delete" ID="btndelete" OnClick="btndelete_Click" ClientIDMode="Static" CssClass="button"/>
                </div>
                </ContentTemplate>
       </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            $(document).ajaxStop($.unblockUI);
            var winHeight = $(window).height();
            if (winHeight < 650) {
                winHeight = (winHeight - 150);
            }
            else {
                winHeight = (winHeight - 200);
            }
            $("#divFlowMeterMaster").height(winHeight);
            getedidtabledrp();
        });

        function getedidtabledrp()
        {
            
            var param = { blank: '' };
            $.ajax({
                type: "POST",
                url: "FlowMeterMasterPage.aspx/getdata",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(param),
                dataType: "json",
                success: OnSuccessGetFirst,
                error: function (response) {
                    console.log(response.d);
                }
            });

        }

        function OnSuccessGetFirst(Result)
        {
            
            console.log(Result);
            var i=0;
            for(i;i<Result.d.length;i++)
            {
                console.log(Result.d[i]);
                //$('#datalist').append(" <option value="+ Result.d[i]+">");
            }
            
        }


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
            $("#divFlowMeterMaster").height(winHeight);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
