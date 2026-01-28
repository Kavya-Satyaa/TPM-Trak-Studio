<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="OEERejectionEntry.aspx.cs" Inherits="Web_TPMTrakDashboard.OEERejectionEntry" EnableEventValidation="false" AsyncTimeout="6000" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        ::-webkit-scrollbar {
            width: 7px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 7px;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 10px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }


        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }

        .ellipsistooltip {
            text-overflow: ellipsis;
            overflow: hidden;
            width: 170px;
            white-space: nowrap;
        }

        legend {
            background-color: midnightblue;
            color: white;
            width: auto;
            margin: 0;
            border: 0;
        }

        th {
            text-align: center;
            height: 45px;
        }

        tr {
            cursor: pointer;
        }

        .HeaderStyleAlt {
            text-align: center;
            background-color: midnightblue;
            color: white;
            height: 30px;
        }

        .HeaderStyle {
            text-align: center;
            width: 33%;
            background-color: midnightblue;
            color: white;
            height: 30px;
        }

        .ddlStyle {
            min-width: 170px;
            margin: auto;
            min-height: 40px;
            width: 170px;
        }

        #tblfilters tr td {
            vertical-align: middle;
        }

        .switch {
            position: relative;
            display: inline-block;
            vertical-align: middle;
            width: 50px;
            height: 30px;
            float: right;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 22px;
                width: 22px;
                left: 3px;
                bottom: 3px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(23px);
            -ms-transform: translateX(23px);
            transform: translateX(23px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 30px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .divFilter {
            position: -webkit-sticky; /* Safari */
            position: sticky;
            top: 60px;
            z-index: 3;
            background-color: #202648;
        }

        .footerFixed {
            font-weight: bold;
            position: -webkit-sticky;
            position: sticky;
            bottom: 0;
            z-index: 2;
        }

        #overlay {
            position: fixed;
            display: none;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            right: -1px;
            bottom: 0;
            background-color: rgba(0,0,0,0.5);
            z-index: 999;
            cursor: pointer;
        }

        #process {
            position: absolute;
            padding: 0;
            margin: 0;
            width: 30%;
            top: 50%;
            left: 50%;
            text-align: center;
            color: #000;
            border: 3px solid #aaa;
            background-color: #fff;
            transform: translate(-50%,-50%);
            -ms-transform: translate(-50%,-50%);
        }
        .headerFixer tr th{
            z-index:2;
        }
    </style>
    <div style="">
        <div id="overlay">
            <div id="process"><asp:Image runat="server" ImageUrl="~//img/loadIcon/ajax-loader.gif" /></div>
            
        </div>
        <div class="divFilter">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <table id="tblfilters" class="table table-bordered" style="width: 100%;">
                        <tr>
                            <td style="">
                                <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control ddlStyle" meta:resourcekey="ddlMachineResource1" />
                            </td>
                            <td class="input-group" style="border: 0; width: 300px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox runat="server" ID="txtTimePeriod" CssClass="form-control date2" Style="min-height: 40px; min-width: 130px; margin-right: 10px;" AutoCompleteType="Disabled" meta:resourcekey="txtTimePeriodResource1" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control ddlStyle" meta:resourcekey="ddlShiftResource1" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Button runat="server" ID="btnView" Text="<%$ Resources:CommanResource, View %>" OnClick="btnView_Click" CssClass="btn btn-primary" Style="width: 170px; margin-left: auto; margin-right: auto; min-height: 40px; font-size: large;" ToolTip="<%$ Resources:CommanResource, View %>" />
                            </td>
                            <td style="width: 250px;">
                                <asp:Label runat="server" Text="<%$ Resources:CommanResource, AutoRefresh %>" Style="color: white; display: inline-block; vertical-align: middle; font-size: 24px;" />
                                <label class="switch">
                                    <asp:CheckBox runat="server" ID="cbAutoRefresh" OnCheckedChanged="cbAutoRefresh_CheckedChanged" AutoPostBack="True" meta:resourcekey="cbAutoRefreshResource1" />
                                    <span class="slider round"></span>
                                </label>
                            </td>
                        </tr>
                    </table>
                    <asp:Timer ID="timerToAutoRefresh" runat="server" Enabled="False" OnTick="timerToAutoRefresh_Tick"></asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row" style="margin-top: 20px; height: 100%;">
            <%--<div class="col-lg-5" style="">
                <div id="divMachineId" style="overflow-x: auto; width: 370px; height: 500px; margin: auto;">
                    <div class="border">
                        <div id="divCockpit" style="padding: 10px; background-color: yellow; border-radius: 25px;">
                            <table class="outercockpit" style="width: 100%">
                                <tr>
                                    <td style="text-align: center; color: black; font-weight: bold; padding-left: 30px; padding-bottom: 5px;">
                                        <asp:Label ID="lblMachineID" runat="server" />
                                        <asp:Image runat="server" Style="width: 30px; height: 30px; float: right;" ImageUrl="~/Images/star.png" />
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered cssNonAdmin cockpit" style='background-color: midnightblue; color: white;'>
                                <asp:ListView ID="lvMachineValues" runat="server" ItemPlaceholderID="addressPlaceHolder">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: left; min-width: 100px; color: white">
                                                <%# Eval("LabelText")%>                                           
                                            </td>
                                            <td style='text-align: left; width: 100px; color: <%# Eval("ForeColor") %>; background-color: <%# Eval              ("BackColor") %>;'>
                                                <div class="ellipsistooltip"><%# Eval("LabelValue")%></div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </table>
                        </div>
                    </div>
                </div>
            </div>--%>
            <div id="divRejectionInformation" class="col-lg-12" style="bottom: 0;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblDownTime" Text="<%$ Resources:CommanResource, DownTime %> (0.00)" Style="color: white; font-size: 21px; background-color: midnightblue; margin-top: 10px; margin-bottom: 10px;" />
                        <div style="height: 250px; background-color: #DCE7F5; overflow: auto; width: 50%;">
                            <asp:GridView runat="server" ID="gvDownTime" BackColor="#DCE7F5" CssClass="headerFixer"
                                Style="width: 100%" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" meta:resourcekey="gvDownTimeResource1">
                                <RowStyle Height="35px" BackColor="White" />
                                <AlternatingRowStyle Height="35px" BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Reason %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Downid") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Size="14pt" Height="45px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Time %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("downtime") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Size="14pt" Height="45px"/>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="height: 100%; background-color: #DCE7F5"></div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <asp:Label runat="server" ID="lblProdCount" Text="<%$ Resources:ProductionCount %>" Style="color: white; font-size: 21px; background-color: midnightblue; margin-top: 10px; margin-bottom: 10px;" />
                        <div style="overflow: auto; height: 250px; width: 100%; background-color: #DCE7F5;">
                            <asp:GridView ID="gvProductionCount" runat="server" CssClass="headerFixer" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" BackColor="#DCE7F5" OnSelectedIndexChanged="gvProductionCount_SelectedIndexChanged" OnRowDataBound="gvProductionCount_RowDataBound" Style="min-width: 1200px; overflow: auto; max-width: 2000px; width: 100%;" meta:resourcekey="gvProductionCountResource1">
                                <RowStyle Height="35px" BackColor="White" />
                                <AlternatingRowStyle Height="35px" BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource, Shift %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblShift" Text='<%# Eval("shift") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource, ComponentID %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblComponent" Text='<%# Eval("Component") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource, OprNo %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOperation" Text='<%# Eval("Operation") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Oprt %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblOperator" Text='<%# Eval("Operator") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:PartsCount %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPartsCount" Text='<%# Eval("OperationCount") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource, RejCount %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRejCount" Text='<%# Eval("RejCount") %>' />
                                        </ItemTemplate>

                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:StdCycleTime %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStdCycleTime" Text='<%# Eval("StdCycletime") %>' />
                                        </ItemTemplate>

                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:ActCycleTime %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblActCycleTime" Text='<%# Eval("ActCycletime") %>' />
                                        </ItemTemplate>

                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:WorkOrderNumber %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblWorkOrderNumber" Text='<%# Eval("WorkOrderNumber") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Lot Number">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLotNumber" Text='<%# Eval("LotNumber") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="170px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="height: 100%; background-color: #DCE7F5"></div>
                                </EmptyDataTemplate>

                                <HeaderStyle Width="170px" Font-Size="14pt" Height="45px" />

                            </asp:GridView>
                        </div>

                        <asp:Label runat="server" ID="lblRejInform" Text="<%$ Resources:CommanResource, RejectionDescription %>" Style="color: white; font-size: 21px; background-color: midnightblue; margin-top: 10px; margin-bottom: 10px;" />
                        <div style="overflow: auto; min-height: 140px; height: 250px; background-color: #DCE7F5; width: 100%; position: relative;">
                            <asp:GridView ID="gvRejectionCount" runat="server" CssClass="headerFixer" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" BackColor="#DCE7F5" Style="height: auto; width: 100%;" OnDataBound="gvRejectionCount_DataBound" OnRowDataBound="gvRejectionCount_RowDataBound" ShowFooter="True" OnSelectedIndexChanged="gvRejectionCount_SelectedIndexChanged" meta:resourcekey="gvRejectionCountResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource, RejectionCategory %>">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="lblID" Value='<%# Eval("id") %>' />
                                            <asp:Label runat="server" ID="lblRejCat" Text='<%# Eval("Catagory") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlRejCat" runat="server" CssClass="form-control input-sm" Style="min-width: 130px; border: 0; font-weight: bold;" OnSelectedIndexChanged="ddlRejCat_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:RejCode %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRejCode" Text='<%# Eval("rejectionid") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlRejCode" runat="server" CssClass="form-control input-sm" Style="min-width: 130px; border: 0; font-weight: bold;">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource, RejCount %>">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCount" Text='<%# Eval("Rejection_Qty") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="txtCount" CssClass="form-control input-sm" onkeypress="return event.charCode >= 48 && event.charCode <= 57" onpast="return false" TextMode="Number" min="0" step="1" Style="width: 100%;" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:CommanResource, Action %>">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lbtnDelete" OnClick="lbtnDelete_Click" OnClientClick="return confirm('Are you sure, you want to delete?');" CssClass="glyphicon glyphicon-trash" ToolTip="<%$ Resources:CommanResource, Delete %>" Font-Size="18pt" ForeColor="Red" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" ID="lbtnAdd" CommandName="New" OnClick="lbtnAdd_Click" ToolTip="<%$ Resources:CommanResource, Add %>" CssClass="glyphicon glyphicon-plus-sign" Font-Size="18pt" ForeColor="White" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <FooterStyle HorizontalAlign="Center" BackColor="#2E6886" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="height: 100%; background-color: #DCE7F5"></div>
                                </EmptyDataTemplate>

                                <HeaderStyle Font-Size="14pt" Height="45px" />

                                <RowStyle Height="35px" BackColor="White" />
                                <AlternatingRowStyle Height="35px" BackColor="White" />
                                <FooterStyle Height="35px" BackColor="#2E6886" CssClass="footerFixed" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script type="text/javascript">        
        $(document).ready(function () {
            $('[id$=txtTimePeriod]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=btnView]').click(function () {
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //document.getElementById('overlay').style.display = "block";
            });
            $('[id$=lbtnAdd]').click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //document.getElementById('overlay').style.display = "block";
            });
            $('[id$=lbtnDelete]').click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //document.getElementById('overlay').style.display = "block";
            });
        });
        
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('[id$=txtTimePeriod]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: true,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            
            $('[id$=btnView]').click(function () {
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //document.getElementById('overlay').style.display = "block";
            });
            $('[id$=lbtnAdd]').click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //document.getElementById('overlay').style.display = "block";
            });
            $('[id$=lbtnDelete]').click(function () {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //document.getElementById('overlay').style.display = "block";
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
