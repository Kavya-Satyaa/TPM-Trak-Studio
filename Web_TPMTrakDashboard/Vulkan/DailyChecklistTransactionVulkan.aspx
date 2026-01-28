<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyChecklistTransactionVulkan.aspx.cs" Inherits="Web_TPMTrakDashboard.Vulkan.DailyChecklistTransactionVulkan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblgrid {
            width: 100%;
        }

            .tblgrid > tbody > tr > th {
                /*background-color: #0094ff;*/
                color: white;
                border: 1px solid #ddd;
                height: 40px;
                text-align: center;
            }

            .tblgrid > tbody > tr > td {
                border: 1px solid #ddd;
                height: 35px;
                border-collapse: collapse;
                padding: 5px;
            }

            .tblgrid > tbody > tr:nth-child(odd) > td {
                background-color: white;
            }

            .tblgrid > tbody > tr:nth-child(even) > td {
                background-color: #ddd;
            }

        .tblSettings {
            width: 90%;
            box-shadow: 0px 0px 4px #afafaf;
            border-radius: 10px;
        }

            .tblSettings > tbody > tr > td {
                color: white;
                padding: 5px 10px;
                /*border: 1px solid black;*/
                border-collapse: collapse;
                text-align: center;
                font-size: large;
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .Valuetd {
            text-align: center;
        }

        .lvDailyCheckListTransaction {
            height: 1px;
        }

            .lvDailyCheckListTransaction > tbody > tr > td {
                border: 1px solid silver;
                padding: 5px;
            }

            .lvDailyCheckListTransaction > tbody > tr:nth-child(odd) > td {
                background-color: white;
            }

            .lvDailyCheckListTransaction > tbody > tr:nth-child(even) > td {
                background-color: #ddd;
            }

            .lvDailyCheckListTransaction > tbody > tr:first-child td {
                background-color: #2E6886 !important;
                color: white;
                font-weight: bold;
                text-align: center;
                position: sticky;
                top:-1px;
            }


            .lvDailyCheckListTransaction > tbody > tr:last-child > td {
                position: sticky;
                bottom: 0;
                height: 40px;
                background-color: aliceblue;
                font-weight: bold;
            }

            .lvDailyCheckListTransaction > tbody > tr:nth-last-child(2) > td {
                position: sticky;
                bottom: 39px;
                background-color: #8ba3b8;
                font-weight: bold;
                min-height: 45px;
            }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }

            .inner-table > tbody > tr > td {
                border-right: 1px solid #989898;
                border-collapse: collapse;
                min-width: 150px;
                max-width: 150px;
                text-align: center;
            }

        .DayClass {
            background-color: transparent;
        }

        .HolidayClass {
            background-color: red;
            border: 1px solid red;
        }
          .green {
            text-align: center;
            font-size: 25px;
            color: green;
            font-weight: bold;
        }

        .red {
            text-align: center;
            font-size: 25px;
            color: red;
            font-weight: bold;
        }
    </style>
    <table class="tblSettings">
        <tr>
            <td>Plant ID</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPlantID" CssClass="form-control" Style="width: 300px;"></asp:DropDownList>
            </td>
            <td>Machine ID</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass="form-control" Style="width: 300px;"></asp:DropDownList>
            </td>
            <td>Year</td>
            <td>
                <asp:TextBox runat="server" ID="txtYear" ClientIDMode="Static" CssClass="form-control" Style="width: 150px;"></asp:TextBox>
            </td>
            <td>Month</td>
            <td>
                <asp:TextBox runat="server" ID="txtMonth" ClientIDMode="Static" CssClass="form-control" Style="width: 150px;"></asp:TextBox>
            </td>
            <td>
                <asp:Button runat="server" ID="btnView" Text="View" CssClass="bajaj-btn-style" OnClick="btnView_Click" />
            </td>
            <td>
                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="bajaj-btn-style" Visible="false" />
            </td>
        </tr>
    </table>

    <div id="gridContainer" style="height: 80vh; overflow: auto; margin-top: 20px;">
        <asp:ListView runat="server" ID="lvDailyCheckListTransaction">
            <LayoutTemplate>
                <table class="lvDailyCheckListTransaction headerFixer" id="lvDailyCheckListTransaction">
                    <tr runat="server" id="itemplaceholder"></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td style="text-align: center; font-weight: bold;" rowspan="1">
                        <asp:Label runat="server" Text="SR. No" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" ID="lblSlNo" Text='<%# Eval("SlNo") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="min-width: 250px;">
                        <asp:Label runat="server" Text="Checkpoint Desc." Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" ID="lblCheckpointDesc" Text='<%# Eval("CheckpointDesc") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="min-width: 300px;">
                        <asp:Label runat="server" Text="Requirement" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" ID="lblRequirement" Text='<%# Eval("Requirement") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Method" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:ListView runat="server" ID="lvMethodmages" DataSource='<%# Eval("MethodList") %>' Visible='<%# Eval("ContentVisibility") %>'>
                            <LayoutTemplate>
                                <div runat="server" id="itemplaceholder"></div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <asp:Image runat="server" ID="imgMethod" ImageUrl='<%# Eval("imgUrl") %>' />
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Instrument" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:ListView runat="server" ID="lvInstrumentImages" DataSource='<%# Eval("InstrumentList") %>' Visible='<%# Eval("ContentVisibility") %>'>
                            <LayoutTemplate>
                                <div runat="server" id="itemplaceholder"></div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <asp:Image runat="server" ID="imgInstrument" ImageUrl='<%# Eval("imgUrl") %>' />
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Action Plan" Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                        <asp:Label runat="server" ID="lblActionPlan" Text='<%# Eval("ActionPlan") %>' Visible='<%# Eval("ContentVisibility") %>'></asp:Label>
                    </td>
                    <td style="padding: 0px;border: 0px;">
                        <asp:ListView runat="server" ID="lvDailyCLValue" DataSource='<%# Eval("DateColumns") %>'>
                            <LayoutTemplate>
                                <table class="inner-table">
                                    <tr>
                                        <td runat="server" id="itemplaceholder"></td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <td style="text-align: center;" class='<%# string.IsNullOrEmpty(Eval("BackgroundClass").ToString()) ? "DayClass" : "HolidayClass" %>'>
                                    <asp:Label runat="server" Font-Bold="true" Text='<%# Eval("DayValue") %>' Visible='<%# Eval("HeaderVisibility") %>'></asp:Label>
                                    <asp:Label runat="server" ID="lblDailyValue" Font-Bold="true" Text='<%# Eval("DailyCLValue") %>' Visible='<%# Eval("DayValueVisibility") %>' CssClass='<%# Eval("DailyCLValueClass") %>'></asp:Label>
                                    <asp:Button runat="server" ID="btnApprove" Text="Approve" Visible='<%# Eval("ApproveVisibility") %>' CssClass="btn btn-primary" OnClick="btnApprove_Click" />
                                    <asp:HiddenField runat="server" ID="hdnDayValue" Value='<%# Eval("DayValue") %>' />
                                </td>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>

    <script>
        $(document).ready(function () {
            setDateControls();
            freezeColumnFromLeft("lvDailyCheckListTransaction", 3);
        });
        function setDateControls() {
            $("#txtYear").datepicker({
                format: 'yyyy',
                viewMode: "years",
                minViewMode: "years",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("#txtMonth").datepicker({
                format: 'M',
                viewMode: "months",
                minViewMode: "months",
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setDateConrols();
            freezeColumnFromLeft("lvDailyCheckListTransaction", 6);
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
