<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SmartShopDefaults.aspx.cs" Inherits="Web_TPMTrakDashboard.SmartShopDefaults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="Scripts/Toast/jquery.toast.min.css" rel="stylesheet" />
    <script src="Scripts/Toast/jquery.toast.min.js"></script>
    <style>
        .trBottom {
            margin-bottom: 5px;
        }

        td {
            color: white;
        }

        fieldset {
            border: 2px solid white;
            padding-left: 10px;
            padding-bottom: 5px;
            border-radius: 4px;
            width: auto;
        }

        legend {
            color: white;
            width: auto;
            border-bottom: 0px;
            margin: 0px;
        }

        .checkmark {
            position: absolute;
            top: 0;
            left: 0;
            height: 28px;
            width: 28px;
            background-color: #eee;
        }

        .minWidth {
            min-width: 200px;
            min-height: 28px;
        }

        @media screen and (max-width: 600px) {
            .minWidth {
                min-width: 50px;
                min-height: 15px;
            }

            legend {
                font-size: 18px;
            }
        }
    </style>
    <div class="container" style="background-color: #202648;">
        <div>
            <h2 style="font-weight: bolder; color: ghostwhite; text-align: center;">Shop Defaults
            </h2>
        </div>

        <div style="width: auto;">
            <asp:UpdatePanel runat="server" ID="SettingPanel">
                <ContentTemplate>
                    <fieldset>
                        <legend>Settings</legend>
                        <table runat="server" style="color: white; width: 100%;">
                            <%--<tr>
                                <td>
                                    <asp:Label runat="server" Text="Time Format" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="Target From" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="MinLUForLR(Sec)" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="CycleIgnoreThreshold" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="Smart Agent ShutDownTime" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="Show 'Zero' Rows in Report" CssClass="minWidth" /><br />
                                    <br />
                                    <br />

                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeFormat" CssClass="form-control input-sm s" Width="200px" Height="28px">
                                        <asp:ListItem Value="mm" Text="Minute" />
                                        <asp:ListItem Value="ss" Text="Second" />
                                    </asp:DropDownList><br />

                                    <asp:DropDownList runat="server" ID="ddlTargetFrom" CssClass="form-control input-sm" Width="200px" Height="28px">
                                        <asp:ListItem Value="Exact Schedule" Text="Exact Schedule" />
                                        <asp:ListItem Value="DefaultTargetPerCo" Text="DefaultTargetPerCo" />
                                        <asp:ListItem Value="% Ideal" Text="% Ideal" />
                                    </asp:DropDownList><br />

                                    <asp:TextBox runat="server" ID="txtMinLUForLR" TextMode="Number" CssClass="form-control input-sm" Width="200px" Height="28px" Text="5" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtMinLUForLR" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator><br />
                                    <asp:DropDownList runat="server" ID="ddlCycleIgnoreThreshold" CssClass="form-control input-sm" Width="200px" Height="28px">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList><br />

                                    <asp:TextBox runat="server" ID="txtSASTime" CssClass="form-control input-sm" TextMode="Time" Width="90px" Height="28px" /><br />


                                    <asp:CheckBoxList ID="cblShowZRReport" runat="server" AutoPostBack="true" CssClass="form-control input-sm" Width="200px" Height="60px" ForeColor="Black" RepeatLayout="Flow">
                                        <asp:ListItem Text="Plant OEE" Value="Plant OEE"></asp:ListItem>
                                        <asp:ListItem Text="Time Consolidated" Value="Time Consolidated"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="JobCard Update" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="Financial Year From" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="SmartData" CssClass="minWidth" />
                                    <br />
                                    <br />
                                    <asp:Label runat="server" Text="JobCard Setting" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="SmartTrans Auto Close Time" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="InterLockTime" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="EM Weights Display" CssClass="minWidth" />

                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlJCUpdate" CssClass="form-control input-sm" Width="200px" Height="28px">
                                        <asp:ListItem Value="Update Autodata" Text="Update Autodata" />
                                        <asp:ListItem Value="No update" Text="No Update" />
                                    </asp:DropDownList><br />

                                    <asp:DropDownList runat="server" ID="ddlFinancialYearFrom" CssClass="form-control input-sm" Width="200px" Height="26px" /><br />

                                    <asp:DropDownList runat="server" ID="ddlSmartdata" CssClass="form-control input-sm" Width="200px" Height="28px">
                                        <asp:ListItem Value="Service" Text="Service" />
                                        <asp:ListItem Value="Console Application" Text="Console Application" />
                                    </asp:DropDownList><br />

                                    <asp:DropDownList runat="server" ID="ddlJCSetting" CssClass="form-control input-sm" Width="200px" Height="28px">
                                        <asp:ListItem Value="Machine" Text="Machine" />
                                        <asp:ListItem Value="Work Order Number" Text="Work Order Number" />
                                    </asp:DropDownList><br />

                                    <asp:TextBox runat="server" ID="txtSACTime" TextMode="Number" Text="10" CssClass="form-control input-sm" Width="130px" Height="28px" Style="display: inline-block" />

                                    <asp:Label runat="server" Text="(minutes)" CssClass="label label-secondary" ForeColor="White" Style="display: inline-block" />
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtSACTime" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator><br />
                                    <asp:TextBox runat="server" ID="txtInterLockTime" TextMode="Number" Text="60" CssClass="form-control input-sm" Width="130px" Height="28px" Style="display: inline-block" />

                                    <asp:Label runat="server" Text="(seconds)" ForeColor="White" CssClass="label label-secondary" Style="display: inline-block" />
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtInterLockTime" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator><br />
                                    <asp:DropDownList runat="server" ID="ddlEWDisplay" CssClass="form-control input-sm" Width="200px" Height="28px">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Time Format" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTimeFormat" CssClass="form-control input-sm " Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="mm" Text="Minute" />
                                        <asp:ListItem Value="ss" Text="Second" />
                                    </asp:DropDownList>
                                </td>

                                <td>
                                    <asp:Label runat="server" Text="JobCard Update" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlJCUpdate" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Update Autodata" Text="Update Autodata" />
                                        <asp:ListItem Value="No update" Text="No Update" />
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Target From" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTargetFrom" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Exact Schedule" Text="Exact Schedule" />
                                        <asp:ListItem Value="DefaultTargetPerCo" Text="DefaultTargetPerCo" />
                                        <asp:ListItem Value="% Ideal" Text="% Ideal" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="Financial Year From" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlFinancialYearFrom" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Text="January" Value="1" />
                                        <asp:ListItem Text="February" Value="2" />
                                        <asp:ListItem Text="March" Value="3" />
                                        <asp:ListItem Text="April" Value="4" />
                                        <asp:ListItem Text="May" Value="5" />
                                        <asp:ListItem Text="June" Value="6" />
                                        <asp:ListItem Text="July" Value="7" />
                                        <asp:ListItem Text="August" Value="8" />
                                        <asp:ListItem Text="September" Value="9" />
                                        <asp:ListItem Text="October" Value="10" />
                                        <asp:ListItem Text="November" Value="11" />
                                        <asp:ListItem Text="December" Value="12" />

                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="MinLUForLR(Sec)" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMinLUForLR" TextMode="Number" CssClass="form-control input-sm" Width="200px" Height="28px" Text="5" Style="margin-left: 50px; margin-bottom: 5px;" />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="SmartData" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSmartdata" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Service" Text="Service" />
                                        <asp:ListItem Value="Console Application" Text="Console Application" />
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="CycleIgnoreThreshold" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCycleIgnoreThreshold" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="JobCard Setting" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlJCSetting" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Machine" Text="Machine" />
                                        <asp:ListItem Value="Work Order Number" Text="Work Order Number" />
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Smart Agent ShutDownTime" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSASTime" CssClass="form-control input-sm" TextMode="Time" Width="90px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;" />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="SmartTrans Auto Close Time" CssClass="minWidth" />

                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSACTime" TextMode="Number" Text="10" CssClass="form-control input-sm" Width="130px" Height="28px" Style="display: inline-block; margin-left: 50px; margin-bottom: 5px;" />
                                    <asp:Label runat="server" Text="(minutes)" CssClass="label label-secondary" ForeColor="White" Style="display: inline-block" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Show 'Zero' Rows in Report" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cblShowZRReport" runat="server" AutoPostBack="true" CssClass="form-control input-sm" Width="200px" Height="60px" ForeColor="Black" RepeatLayout="Flow" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Text="Plant OEE" Value="Plant OEE"></asp:ListItem>
                                        <asp:ListItem Text="Time Consolidated" Value="Time Consolidated"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="InterLockTime" CssClass="minWidth" /><br />
                                    <br />
                                    <asp:Label runat="server" Text="EM Weights Display" CssClass="minWidth" />

                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtInterLockTime" TextMode="Number" Text="60" CssClass="form-control input-sm" Width="130px" Height="28px" Style="display: inline-block; margin-left: 50px; margin-bottom: 7px;" />
                                    <asp:Label runat="server" Text="(seconds)" ForeColor="White" CssClass="label label-secondary" Style="display: inline-block" />
                                    <asp:DropDownList runat="server" ID="ddlEWDisplay" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Sub Operation " CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlsuboperation" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="QE Enabled?" CssClass="minWidth" />
                                </td>
                                <td style="text-align: center">
                                    <asp:CheckBox runat="server" ID="chkQEEnabled" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Live: Daily and Shift Report - Diff." CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlLiveReportDailyDiff" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="Live: Daily and Shift Report - Target" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlLiveReportDailyTarget" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="DownTime Report Std Setup & Eff. Visisble" CssClass="minWidth" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlStdSetupEffVisible" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;">
                                        <asp:ListItem Value="Y" Text="Y" />
                                        <asp:ListItem Value="N" Text="N" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <label class="control-label">Interface ID Data Type</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCompInfoDataType" runat="server" CssClass="form-control input-md">
                                        <asp:ListItem Value="Numeric">Numeric</asp:ListItem>
                                        <asp:ListItem Value="AlphaNumeric">Alpha Numeric</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Load Unload Threshold" CssClass="minWidth"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" AutoCompleteType="Disabled" TextMode="Number" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;" ID="txtLoadUnloadThreshold"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="Load Unload In Seconds" CssClass="minWidth"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" AutoCompleteType="Disabled"  TextMode="Number" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 50px; margin-bottom: 5px;" ID="txtLoadUnloadInSeconds"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-lg-6 col-sm-12" style="width: auto;">
                <asp:UpdatePanel ID="PDTPanel" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Planned Down Times</legend>
                            <table runat="server" style="width: 100%;">
                                <%--<tr>
                                    <td>
                                        <asp:Label runat="server" Text="Ignore Count" CssClass="minWidth" /><br />
                                        <br />
                                        <asp:Label runat="server" Text="Ignore Production Time" CssClass="minWidth" /><br />
                                        <br />
                                        <asp:Label runat="server" Text="Ignore Downtime" CssClass="minWidth" /><br />
                                        <br />
                                        <asp:Label runat="server" Text="Ignore Avg. Cycletime" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlIgnoreCount" CssClass="form-control input-sm" Width="200px" Height="28px">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="N" Text="N" />
                                        </asp:DropDownList><br />

                                        <asp:DropDownList runat="server" ID="ddlIgnoreProductionTime" CssClass="form-control input-sm" Width="200px" Height="28px">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="Y" Text="N" />
                                        </asp:DropDownList><br />

                                        <asp:DropDownList runat="server" ID="ddlIgnoreDowntime" CssClass="form-control input-sm" Width="200px" Height="28px">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="N" Text="N" />
                                        </asp:DropDownList><br />

                                        <asp:DropDownList runat="server" ID="ddlIgnoreAvgCylcetime" CssClass="form-control input-sm" Width="200px" Height="28px">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="N" Text="N" />
                                        </asp:DropDownList>

                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Ignore Count" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlIgnoreCount" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 110px; margin-bottom: 5px;">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="N" Text="N" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Ignore Production Time" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlIgnoreProductionTime" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 110px; margin-bottom: 5px;">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="N" Text="N" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Ignore Downtime" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlIgnoreDowntime" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 110px; margin-bottom: 5px;">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="N" Text="N" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Ignore Avg. Cycletime" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlIgnoreAvgCylcetime" CssClass="form-control input-sm" Width="200px" Height="28px" Style="margin-left: 110px; margin-bottom: 5px;">
                                            <asp:ListItem Value="Y" Text="Y" />
                                            <asp:ListItem Value="N" Text="N" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-lg-6 col-sm-12">
                <asp:UpdatePanel ID="ANDONPanel" runat="server">
                    <ContentTemplate>
                        <fieldset style="height: 177px;">
                            <legend>ANDON Status Thresholds(in minutes)</legend>
                            <table runat="server" style="width: 100%;">
                                <%--<tr>
                                    <td>
                                        <asp:Label runat="server" Text="Type-11-Threshold" CssClass="minWidth" /><br />
                                        <br />
                                        <asp:Label runat="server" Text="Type-1-Threshold" CssClass="minWidth" /><br />
                                        <br />
                                        <asp:Label runat="server" Text="Type-40-Threshold" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtType11Threshold" runat="server" TextMode="Number" Text="1" CssClass="form-control" Width="200px" Height="28px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtType11Threshold" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator><br />

                                        <asp:TextBox ID="txtType1Threshold" runat="server" TextMode="Number" Text="2" CssClass="form-control" Width="200px" Height="28px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate="txtType1Threshold" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator><br />

                                        <asp:TextBox ID="txtType40Threshold" runat="server" TextMode="Number" Text="3" CssClass="form-control" Width="200px" Height="28px" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate="txtType40Threshold" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Type-11-Threshold" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtType11Threshold" runat="server" TextMode="Number" Text="1" CssClass="form-control" Width="200px" Height="28px" Style="margin-left: 120px; margin-bottom: 5px;" />
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtType11Threshold" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Type-1-Threshold" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtType1Threshold" runat="server" TextMode="Number" Text="2" CssClass="form-control" Width="200px" Height="28px" Style="margin-left: 120px; margin-bottom: 5px;" />
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate="txtType1Threshold" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Type-40-Threshold" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtType40Threshold" runat="server" TextMode="Number" Text="3" CssClass="form-control" Width="200px" Height="28px" Style="margin-left: 120px; margin-bottom: 5px;" />
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate="txtType40Threshold" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Style="text-shadow: 0 0 3px red; font-weight: bold;"></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="col-lg-6 col-sm-12" runat="server" id="operatoeIncentiveSettingDiv">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset style="height: 177px;">
                            <legend>Operator Incentive Report Setting</legend>
                            <table runat="server" style="width: 100%;">

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Operation Rate" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOprIncentiveOpnRate" runat="server" TextMode="Number" CssClass="form-control" Width="200px" Height="28px" Style="margin-left: 120px; margin-bottom: 5px;" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Hourly Incentive" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOprIncentiveHourlyIncentive" runat="server" TextMode="Number" CssClass="form-control" Width="200px" Height="28px" Style="margin-left: 120px; margin-bottom: 5px;" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Shift Incentive In Rs" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOprIncentiveShiftIncentiveInRs" runat="server" TextMode="Number" CssClass="form-control" Width="200px" Height="28px" Style="margin-left: 120px; margin-bottom: 5px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Shift Target" CssClass="minWidth" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOprIncentiveShiftTarget" runat="server" TextMode="Number" CssClass="form-control" Width="200px" Height="28px" Style="margin-left: 120px; margin-bottom: 5px;" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div style="text-align: center; margin-top: 10px;">
            <asp:UpdatePanel ID="pnlButtons" runat="server">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnAccept" Text="Accept" CssClass="btn btn-primary" OnClick="btnAccept_Click" />
                    &nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnCancel" Text="Close" CssClass="btn btn-danger" PostBackUrl="~/Dashboard.aspx" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <style>
            @media screen and (max-width: 600px) {
                table {
                    font-size: 12px;
                    width: auto;
                }
            }
        </style>
        <script type="text/javascript">
            function openSuccessToast() {
                $.toast({
                    text: "Settings Updated Successfully.",
                    heading: 'Success!',
                    icon: 'success',
                    showHideTransition: 'fade',
                    allowToastClose: true,
                    hideAfter: 3000,
                    stack: false,
                    position: 'top-right',
                    bgColor: '#2AB802',

                    textAlign: 'left',
                    loader: true,
                    loaderBg: 'white',

                });
            };
            function openWarningToast(msg) {
                $.toast({
                    text: msg,
                    heading: 'Warning!',
                    icon: 'warning',
                    showHideTransition: 'fade',
                    allowToastClose: true,
                    hideAfter: 5000,
                    stack: false,
                    position: 'top-right',
                    bgColor: '#F87C00',
                    textAlign: 'left',
                    loader: true,
                    loaderBg: 'white',

                });
            };
            function openSuccessToast2() {
                Command: toastr["success"]("Settings Successfully Updated")
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }
            function openWarningToast2(msg) {
                Command: toastr["warning"](msg)
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
