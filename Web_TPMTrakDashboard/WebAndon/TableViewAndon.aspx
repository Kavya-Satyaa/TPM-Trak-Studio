<%@ Page Title="" Language="C#" MasterPageFile="~/WebAndon/AndonMaster.Master" AutoEventWireup="true" CodeBehind="TableViewAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.WebAndon.TableViewAndon" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        th {
            cursor: pointer;
        }

            th.headerSortUp {
                background-image: url(Image/asc.gif);
                background-position: right center;
                background-repeat: no-repeat;
            }

            th.headerSortDown {
                background-image: url(Image/desc.gif);
                background-position: right center;
                background-repeat: no-repeat;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: <%= settings.TableUISetting.BorderWidthTableView%>px solid;
            margin: 0 auto 1em auto; /* centers */
            border-color: <%= settings.TableUISetting.BorderColorTableView %>;
        }

        .HeaderCss {
            color: white;
            background-color: #428bca;
            font-weight: bold;
            min-width: 100px;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }
    </style>
    <div class="container-fluid" style='font-family: <%= settings.AppUISettings.FontFamily %>; font-style: <%= settings.AppUISettings.FontStyle %>; font-weight: <%= settings.AppUISettings.FontStyle %>;'>
        <div class="row" style="text-align: center; color: red;">
            <asp:HiddenField ID="hdfMode" runat="server" />
            <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
        </div>
        <div class="row">
            <asp:UpdatePanel ID="updateTableViewData" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gridviewTableData" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="gridviewTableData_RowDataBound" CssClass="table table-bordered">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="image" runat="server" ImageUrl='<%# Eval("MachineStatus") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MachineId">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text=<%# Bind("MachineId") %> ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>
                    <asp:Timer ID="timerDataChange" runat="server" OnTick="timerDataChange_Tick" Interval="6000"></asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script src="../Scripts/ShortingJs/jquery.tablesorter.min.js"></script> 
     <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script type="text/javascript">
        dateTimePicker();
        function dateTimePicker() {
            $('[id$=txtDate]').datepicker({
                format: 'dd-M-yyyy',
                //container: container,
                todayHighlight: true,
                autoclose: true,
            });
          
        }
        $(document).ready(function () {
            StartFromBegin();
            StartDesktopMode();
        });
        function StartFromBegin() {
            $.unblockUI({});
            $("table tr td[colspan]").hide();
        };
        function StartDesktopMode() {
            $.unblockUI({});
            $("[id$=gridviewTableData]").tablesorter();
        }
        $("[id$=ddlPlantName]").change(function () {
            if ($("[id$=hdfMode]").val() == "ANDON")
                $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
        });
        $("[id$=btnProcess]").click(function () {
            if ($("[id$=txtDate]").val() == "") {
                alert("Please enter the Date !!");
                return false;
            }
            $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("[id$=ddlPlantName]").change(function () {
                $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
            });
            dateTimePicker();
        });
    </script>
</asp:Content>
