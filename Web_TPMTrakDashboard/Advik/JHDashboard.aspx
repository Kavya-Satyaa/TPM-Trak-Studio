<%@ Page Title="JH Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JHDashboard.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.JHDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>


    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style type="text/css">
        .headerFixerTable tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 100%;
            overflow: auto;
            margin-top: 15px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
            }

        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        .table tbody > tr > th {
            vertical-align: middle;
        }

        .table > tr > td {
            vertical-align: middle;
        }
        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 15px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }


        .table .lbl {
            padding-top: 15px;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }
     
    </style>


    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnEmpRole" ClientIDMode="Static" />
            <div style="display: flex; justify-content: center; align-content: center;">
                <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; font-style: italic; vertical-align: central; text-align: center; width: 600px; word-wrap: break-word;" Font-Size="Larger" ClientIDMode="Static"></asp:Label>
            </div>


            <table id="tblfilter" class="table table-bordered" style="width: auto;">
                <tr>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Plant</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" Width="160" Style="height: 40px" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Cell</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" Width="160" Style="height: 40px" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="commanTd" style="min-width: 50px; height: 50px">Machine</td>
                    <td style="min-width: 160px;">
                        <asp:ListBox ID="ddlMachineId" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource, MachineTooltip %> " Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="width: 70px;">Shift</td>
                    <td style="min-width: 160px;">
                        <asp:ListBox ID="ddlShift" runat="server" SelectionMode="Multiple" ToolTip="<%$Resources:CommanResource,ShiftTooltip %>" Width="150" ClientIDMode="Static"></asp:ListBox>
                    </td>
                    <td class="commanTd" style="width: auto;">JH Type</td>
                    <td style="min-width: 160px;">
                        <asp:DropDownList ID="ddlJHType" runat="server" CssClass="form-control" Style="height: 40px">
                            <asp:ListItem Value="" Text="All" />
                            <asp:ListItem Value="Auto" Text="JH Auto" />
                            <asp:ListItem Value="Manual" Text="JH Manual" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="commanTd" style="width: auto;">From Date </td>
                    <td class="input-group" style="min-width: 150px; border: 0">

                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date1" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>

                    </td>
                    <td class="commanTd" style="width: auto;">To Date </td>
                    <td class="input-group" style="min-width: 150px; border: 0">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" Style="min-width: 130px; min-height: 40px;" CssClass="form-control date2" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                    </td>

                    <td style="text-align: center; width: auto; border-right: none"></td>
                    <td style="text-align: center; width: auto; border-left: none; border-right: none">
                        <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>
                        <asp:Button runat="server" Visible="false" Text="Save" CssClass="btn btn-info btn-sm displayCss" ID="btnSave" OnClick="btnSave_Click"></asp:Button>
                        <asp:Button runat="server" Text="Export" CssClass="btn btn-info btn-sm displayCss" ID="btnExport" OnClick="btnExport_Click"></asp:Button>
                    </td>
                    <td style="border-left: none"></td>
                </tr>
            </table>

            <div style="display: flex; justify-content: center; align-content: center;">
                <div id="gridContainer" class="divGrid" style="">
                    <asp:GridView runat="server" ID="gvJHDashboard" AutoGenerateColumns="False" ClientIDMode="Static"
                        CssClass="table table-bordered cockpit headerFixerTable" ShowHeaderWhenEmpty="true">
                        <Columns>

                            <%-- <asp:TemplateField HeaderText="Sl. No.">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSlno" Text='<%#Eval("SlNo")%>' ForeColor="White" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbldate" Text='<%#Eval("Date")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shift">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblShift" Text='<%#Eval("Shift")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Machine ID">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnChecklistID" Value='<%#Eval("JHActivityID")%>'/>
                                    <asp:Label runat="server" ID="lblMachine" Text='<%#Eval("Machine")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                           <%-- <asp:TemplateField HeaderText="JH Activity">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbkJHActivity" Text='<%#Eval("JHActivity")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                             <asp:TemplateField HeaderText="Mc Area">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbkMcArea" Text='<%#Eval("McArea")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbkLocation" Text='<%#Eval("Location")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Std Condition">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbkStdCondition" Text='<%#Eval("StdCondition")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Checking Method">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbkCheckingMethod" Text='<%#Eval("CheckingMethod")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="JH Type">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblJHType" Text='<%#Eval("JHType")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("Status")%>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                          <%--  <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                     <asp:HiddenField runat="server" ID="hdnRemarksUpdate" ClientIDMode="Static" />
                                    <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" Text='<%#Eval("Remarks")%>' CssClass="form-control" AutoCompleteType="Disabled" onkeyup="remarksChange(this);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <div style="height: 100%; background-color: white; text-align: center; color: red">No Data Found</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <%--  <div style="display: flex; justify-content: center; width: 75%; margin: auto; margin-top: 10px;">
                    <asp:Button runat="server" Text="New" CssClass="btn btn-info btn-sm displayCss" ID="btnAdd" OnClick="btnAdd_Click" ClientIDMode="Static" Style="margin-right: 20px;"></asp:Button>
                    <asp:Button runat="server" Text="Edit" CssClass="btn btn-info btn-sm displayCss" ID="btnSave" OnClick="btnSave_Click" Style="margin-right: 20px;"></asp:Button>
                    <asp:Button runat="server" Text="Cancel" CssClass="btn btn-info btn-sm displayCss" ID="btnCancel" OnClick="btnCancel_Click"></asp:Button>
                </div>--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">

        $(document).ready(function () {
            $('[id$=ddlMachineId]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=ddlShift]').multiselect({
                includeSelectAllOption: true
            });

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                useCurrent: false,
                locale: 'en-US'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            var Height = $(window).height() - (220);
            $('#gridContainer').css('height', Height);
        });
        function remarksChange(txt) {
            $(txt).closest('td').find('#hdnRemarksUpdate').val("update");
        }
        function supervisorChkChange(chk) {
            debugger;
            $(chk).closest('td').find('#hdnSupUpdate').val("update");
        }
        function productionHeadChkChange(chk) {
            debugger;
            $(chk).closest('td').find('#hdnProdHeadUpdate').val("update");
        }
        $(window).resize(function () {
            var Height = $(window).height() - (220);
            $('#gridContainer').css('height', Height);
        });

        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("lblMessages").style.display = "none";
            }, 2000);

        };
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                $('[id$=ddlMachineId]').multiselect({
                    includeSelectAllOption: true
                });
                $('[id$=ddlShift]').multiselect({
                    includeSelectAllOption: true
                });

                $('[id$=txtFromDate]').datetimepicker({
                    format: 'DD-MM-YYYY',
                    locale: 'en-US'
                });
                $('[id$=txtToDate]').datetimepicker({
                    format: 'DD-MM-YYYY',
                    useCurrent: false,
                    locale: 'en-US'
                });
                $("[id$=txtFromDate]").on("dp.change", function (e) {
                    $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
                });
                $("[id$=txtToDate]").on("dp.change", function (e) {
                    $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
                });

                var Height = $(window).height() - (220);
                $('#gridContainer').css('height', Height);
            });
            $(window).resize(function () {
                var Height = $(window).height() - (220);
                $('#gridContainer').css('height', Height);
            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
