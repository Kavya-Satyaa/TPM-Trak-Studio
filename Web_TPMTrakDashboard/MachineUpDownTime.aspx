<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MachineUpDownTime.aspx.cs" Inherits="Web_TPMTrakDashboard.PTA.MachineUpDownTime" %>

<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <script src="../Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="../Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>

    <style>
        .lblDiv {
            width: 130px;
            color: white;
            padding-top: 15px;
            text-align: center;
        }

        .ValsDiv {
            width: 180px;
            text-align: center;
        }

        .DownReason {
            height: 20px;
            width: 20px;
            background-color: red;
            display: inline-block;
        }

        .Running {
            height: 20px;
            width: 20px;
            background-color: lime;
            display: inline-block;
        }

        .ICD {
            height: 20px;
            width: 20px;
            background-color: maroon;
            display: inline-block;
        }

        .ManagementLoss {
            height: 20px;
            width: 20px;
            background-color: yellow;
            display: inline-block;
        }

        .PDT {
            height: 20px;
            width: 20px;
            background-color: blue;
            display: inline-block;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            vertical-align: inherit;
        }
    </style>

    <div class="row text-center">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Style="font-weight: bold; font-family: Calibri; color: red;" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>
    <br />
    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <div class="row">
                <asp:Label ID="lblMessages" runat="server" EnableViewState="False" meta:resourcekey="lblMessagesResource1"></asp:Label>
            </div>
            <div class="row" style="margin-bottom: 4px; margin-top: -11px;">
                <table id="tblHeader" class="table table-bordered" style="background-color: #394A59; width: auto;">
                    <tr>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Plant") %></b>
                        </td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <asp:DropDownList ID="ddlPlantId" runat="server" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" ToolTip="<%$Resources:CommanResource, PlantTooltip %>" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Cell") %></b>
                        </td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <asp:DropDownList ID="ddlCellID" runat="server" CssClass="form-control" data-toggle="tooltip" title="Cell ID !" ToolTip="<%$Resources:CommanResource, CellId %>" AutoPostBack="True" OnSelectedIndexChanged="ddlCellID_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Machine") %></b>
                        </td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <asp:ListBox ID="ddlMachineId" runat="server" SelectionMode="Multiple" CssClass="ddlMachineIdStyle"></asp:ListBox>
                        </td>
                        <td class="lblDiv" style="width: 60px;">
                            <b><%=GetGlobalResourceObject("CommanResource","Shift") %></b>
                        </td>
                        <td class="ValsDiv" style="min-width: 120px;">
                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" ToolTip="<%$Resources:CommanResource,ShiftTooltip %>">
                            </asp:DropDownList>
                        </td>

                        <td class="lblDiv" style="width: 60px;"><b>Date</b></td>
                        <td class="input-group ValsDiv" style="min-width: 120px;">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip"
                                title="From Date !" placeholder="From Date" ToolTip="<%$Resources:CommanResource, FromDate %>" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td style="text-align: left" class="lblDiv">
                            <asp:Button runat="server" Text="<%$ Resources: CommanResource, View %>" class="btn btn-info btn-sm" ID="btnProcess" OnClick="btnProcess_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="container row">
                <chart:WebChartViewer ID="WebChartViewer1" CssClass="myItem" runat="server" Height="100%" Width="100%" />
                <div class="row" style="text-align: center">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <span class="commontd" style="display: inline-block;">
                                <span style="border-style:double;padding:5px;margin-right:10px;border-color:orchid">
                                    <asp:CheckBox runat="server" ID="AndonMode" Checked="false" OnCheckedChanged="AndonMode_CheckedChanged" AutoPostBack="true" />
                                    <span style="display: inline-block; color: lime; font-weight: bolder; padding: 10px">Andon Mode</span>
                                </span>
                                <asp:Button ID="btnPrevious" runat="server" CssClass="btn btn-primary btn-round-sm btn-sm" OnClick="btnPrevious_Click" Text="<%$ Resources: CommanResource, Previous %>"></asp:Button>
                                &nbsp;&nbsp;
                            <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary btn-round-sm btn-sm" OnClick="btnNext_Click" Text="<%$ Resources: CommanResource, Next %>"></asp:Button>
                            </span>


                            <span class="commontd" style="float: right">
                                <span class="DownReason"></span>
                                <label><%=GetGlobalResourceObject("CommanResource","DownReason") %></label>&nbsp;&nbsp;
						<span class="Running"></span>
                                <label><%=GetGlobalResourceObject("CommanResource","Running") %></label>&nbsp;&nbsp;
						<span class="ICD"></span>
                                <label><%=GetGlobalResourceObject("CommanResource","ICD") %></label>&nbsp;&nbsp;
						<span class="ManagementLoss"></span>
                                <label><%=GetGlobalResourceObject("CommanResource","ManagementLoss") %></label>&nbsp;&nbsp;
						<span class="PDT"></span>
                                <label><%=GetGlobalResourceObject("CommanResource","PDT") %></label>&nbsp;&nbsp;
                            </span>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div>
                <asp:Timer runat="server" ID="timer" OnTick="timer_Tick" Enabled="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">

        $(document).ready(function () {

            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
            //CountNumberOfBox();

        })
        $(document).on("click", "[id$=btnProcess]", function () {
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "[id$=btnPrevious]", function () {
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "[id$=btnNext]", function () {
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
        })

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
