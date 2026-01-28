<%@ Page Language="C#" Title="Rework Details" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReworkDetails.aspx.cs" Inherits="Web_TPMTrakDashboard.ReworkDetails" meta:resourcekey="PageResource1" Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>

    <style>
        th {
            cursor: pointer;
        }

        .table thead > tr > th {
            vertical-align: top;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 2px solid;
            margin: 0 auto 1em auto; /* centers */
            border-color: darkgray;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        .table tbody > tr > th {
            color: white;
            text-align: center;
        }

        .tbl th {
            width: 10%;
        }

        .tbl td {
            width: 10%;
        }

        .displayCss {
            display: inline;
        }

        .footerCss {
            background-color: #2E6886 !important;
        }

        .ajax-loader {
            display: none;
            background-color: rgba(0, 0, 0, 0.6);
            position: absolute;
            z-index: +100 !important;
            width: 100%;
            height: 100%;
            margin-left: -15px;
            margin-top: -16px;
        }

        #load-div {
            position: fixed;
            padding-right: 100px;
            width: 30%;
            top: 40%;
            left: 35%;
            text-align: center;
            border: 3px solid rgb(170, 170, 170);
            background-color: rgb(255, 255, 255);
            cursor: wait;
        }

        .ajax-loader img {
            position: relative;
            left: 50%;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        text {
            text-decoration: none !important;
        }

        .ui-datepicker .ui-datepicker-prev,
        .ui-datepicker .ui-datepicker-next {
            display: none;
        }

        #tblDashboardInfo tbody tr:nth-child(odd) {
            background-color: white;
        }

        #tblDashboardInfo tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .multiselect-container {
            overflow-y: auto;
        }

        #tblHeader {
            margin-bottom: 0px;
        }

        .dropdowncss {
            font-weight: bold;
            font-size: 1em;
        }

        .HeaderCss {
            color: white;
            background-color: #428bca;
            font-weight: bold;
            min-width: 100px;
        }

        @media screen and (min-width: 1920px) {
            .reworkDetailscsss {
                overflow-x: auto;
                overflow-y: auto;
                border-style: solid;
                border-color: DarkGray;
            }
        }

        @media screen and (min-width: 1200px) {
            .reworkDetailscsss {
                overflow-x: auto;
                overflow-y: auto;
                border-style: solid;
                border-color: DarkGray;
            }
        }

        @media screen and (min-width: 1600px) {
            .reworkDetailscsss {
                overflow-x: auto;
                overflow-y: auto;
                border-style: solid;
                border-color: DarkGray;
            }
        }
    </style>

    <div class="row" style="text-align: center; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
    </div>
    <div class="row" style="margin-bottom: 4px; margin-top: -3px;">
        <table id="tblHeader" class="table table-bordered" style="background-color: #394A59; height: 50px;">
            <tr>
                <td class="commontd" style="width: 110px;padding-top:15px;"><b>Machine ID </b></td>
                <td>
                    <asp:DropDownList ID="ddlMachineID" Style="min-width: 150px; max-width: 250px;" runat="server" CssClass="select form-control dropdowncss" ToolTip="Select machine" />
                </td>

                <td class="commontd" style="width: 110px;padding-top:15px;align-content:center;text-align:center"><b><%=GetGlobalResourceObject("CommanResource","FromDate")%></b></td>
                <td class="input-group" style="margin-bottom: 0px;">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="From DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                </td>

                <td class="commontd" style="width: 110px;padding-top:15px;align-content:center;text-align:center"><b><%=GetGlobalResourceObject("CommanResource","ToDate")%></b></td>
                <td class="input-group" style="margin-bottom: 0px;">
                    <div class="input-group-addon">
                        <i class="glyphicon glyphicon-calendar"></i>
                    </div>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" data-toggle="tooltip" title="To DateTime !" placeholder="DateTime" ToolTip="<%$Resources:CommanResource, Date %>" AutoCompleteType="Disabled"></asp:TextBox>
                </td>

                <td style="padding-left: 2px;">
                    <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="btn btn-primary" OnClick="btnProcess_Click" />
                </td>
				 <td style="padding-left: 2px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
                </td>
            </tr>
        </table>
    </div>

    <div class="row" style="background-color: #DCE7F5">
        <asp:UpdatePanel ID="updateReworkDetails" runat="server">
            <ContentTemplate>
                <asp:GridView ID="ReworkDetailsGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" HeaderStyle-BackColor="#5391CA" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Large" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true">
                    <AlternatingRowStyle BackColor="#DCE7F5" />
                    <Columns>
                        <asp:BoundField HeaderText="Machine ID" DataField="MachineId" meta:resourcekey="BoundFieldResource11" />
						<asp:BoundField HeaderText="Machine Description" DataField="Machinedescription" meta:resourcekey="BoundFieldResource13" />
                        <asp:BoundField HeaderText="Child Batch ID" DataField="ChildBatchID" meta:resourcekey="BoundFieldResource12" />
                        <asp:BoundField HeaderText="Rework Count" DataField="ReworkCount" meta:resourcekey="BoundFieldResource7" />
                        <asp:BoundField HeaderText="Operator ID" DataField="OperatorID" meta:resourcekey="BoundFieldResource8" />
                        <asp:BoundField HeaderText="Plan No." DataField="PlanNo" meta:resourcekey="BoundFieldResource9" />
                        <asp:BoundField HeaderText="FG Material ID" DataField="FGMaterialID" meta:resourcekey="BoundFieldResource10" />
                        <asp:BoundField HeaderText="PV No." DataField="PVNo" meta:resourcekey="BoundFieldResource2" />
                        <asp:BoundField HeaderText="FG Batch ID" DataField="FGBatchID" DataFormatString="{0:N0}" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Child Part ID" DataField="ChildPartID" DataFormatString="{0:N0}" HtmlEncode="false" />
                    </Columns>
                    <EmptyDataTemplate>
                        No data available for selected Machine ID and date time period.
                    </EmptyDataTemplate>
                    <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Large" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
            $('[id$=txtFromDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: true,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('[id$=txtToDate]').datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

            // date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
         
            hideLoader();
        });

        function showLoader() {
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }

        function hideLoader() {
            $.unblockUI({});
            $('.ajax-loader').hide();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
