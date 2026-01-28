<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMGenerationViewLnT.aspx.cs" Inherits="Web_TPMTrakDashboard.LnTOdisha.PMGenerationViewLnT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <style>
        .table-style tr td {
            color: white;
            padding: 4px;
            white-space: nowrap;
        }

        .table-style tr th {
            background-color: #5391CA;
            color: white;
            font-weight: bold;
            padding: 5px;
            white-space: nowrap;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }

        .headerFixerhere tr:nth-child(2n+1) td:nth-child(3), .headerFixerhere tr:nth-child(2n) td:nth-child(3) {
            position: sticky;
            left: 0px;
            z-index: 1;
            background-color: #202648;
        }

        .headerFixerhere tr:first-child th:nth-child(3) {
            position: sticky;
            left: 0px;
            z-index: 6;
            background-color: #5391CA;
        }

        .bootstrap-datetimepicker-widget table td {
            /*background-color: #5391CA !important;*/
            color: black !important;
        }

        .table-condensed > thead > tr > th:hover, .table-condensed > tbody > tr > th:hover, .table-condensed > tfoot > tr > th:hover, .table-condensed > thead > tr > td:hover, .table-condensed > tbody > tr > td:hover, .table-condensed > tfoot > tr > td:hover {
            background-color: #5391CA !important;
        }

        fieldset {
            border: 1px solid white !important;
            padding: 0px 5px 5px 5px !important;
            margin: 0 0 8px 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            margin: 0px;
            vertical-align: top;
        }

        legend {
            font-size: 1.1em !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            color: white;
            border-bottom: none;
            margin-top: -4px;
            margin: 0px;
        }
    </style>
    <div>
        <div>
            <table style="display: inline-block;" class="table-style">
                <tr>


                    <td style="text-align: center; vertical-align: central; align-content: center;">Machine ID:
                    </td>
                    <td style="width: 150px">
                        <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" Width="140px" ClientIDMode="Static" />
                    </td>
                    <td>
                        <fieldset class="masterFS" style="margin-left: 5px !important; margin-right: 4px !important; display: inline-block">
                            <legend>View by Date</legend>
                            <table class="table-style">
                                <tr>
                                    <td style="text-align: center; vertical-align: central; align-content: center;">From Date:
                                    </td>

                                    <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar" style="color: black"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date restrictDateControl" placeholder="Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td style="text-align: center; vertical-align: central; align-content: center;">ToDate:
                                    </td>

                                    <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar" style="color: black;"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date restrictDateControl" placeholder="Date" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" CssClass="btn btn-info" Text="View" Width="80" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="masterFS" style="margin-left: 5px !important; margin-right: 4px !important; display: inline-block">
                            <legend>View by Year</legend>
                            <table class="table-style">
                                <tr>
                                    <td style="vertical-align: central; align-content: center;">Year:
                                    </td>
                                    <td style="width: 120px">
                                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control commandateTxt" Style="width: 100%; display: inline;" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="Button1" OnClick="btnView_Click" CssClass="btn btn-info" Text="View" Width="80" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; height: 75vh; overflow: auto">
            <asp:GridView ID="gvActvityTiming" runat="server" Width="100%" EmptyDataText="No Data Found." ShowHeaderWhenEmpty="true" ShowHeader="true" ClientIDMode="Static" CssClass="table table-bordered table-hover headerFixer bajaj-table-style">
            </asp:GridView>
        </div>

    </div>

    <script>
        $('[id$=txtYear]').datepicker({
            minViewMode: 2,
            format: 'yyyy',
            todayHighlight: true,
            autoclose: true,
            language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
        });
        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MM-YYYY',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtToDate]').datetimepicker({
            format: 'DD-MM-YYYY',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtNewDate]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtNewHeaderDate]').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });

        showAndHideColumns();

        function showAndHideColumns() {
            $('#gvActvityTiming tr th:nth-child(5)').hide();
            $('#gvActvityTiming tr td:nth-child(5)').hide();
            if ($('#ddlFrequency option:selected').text() == "Shift") {
                $('#gvActvityTiming tr td:nth-child(8)').show();
                $('#gvActvityTiming tr th:nth-child(8)').show();
                $('#gvActvityTiming tr td:nth-child(9)').show();
                $('#gvActvityTiming tr th:nth-child(9)').show();
            }
            else {
                $('#gvActvityTiming tr td:nth-child(8)').hide();
                $('#gvActvityTiming tr th:nth-child(8)').hide();
                $('#gvActvityTiming tr td:nth-child(9)').hide();
                $('#gvActvityTiming tr th:nth-child(9)').hide();
            }
        }

        $('#gvActvityTiming tr th .update').click(function () {
            $("#activityHeaderTimingUpdateModal").modal('show');
            $("#hdnDateVal").val($(this).parent().index());
        });

        $(document).keypress(function (e) {
            if ($("#activityTimingUpdateModal").hasClass('in') && (e.keycode == 13 || e.which == 13)) {
                $("#btnActivityTimingDateUpdate").trigger("click");
                return false;
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
