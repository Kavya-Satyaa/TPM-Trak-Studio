<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HMI_Setting.aspx.cs" Inherits="Web_TPMTrakDashboard.HMI_Setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="Scripts/Toast/jquery.toast.min.css" rel="stylesheet" />
    <script src="Scripts/Toast/jquery.toast.min.js"></script>
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery-1.8.2.js"></script>

    <script>
        $("[id$=btnView]").click(function () {
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .6,
                    color: '#fff'
                }
            });
            //$("#container").css("cursor", "wait");
            //   $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            //myFunction();
        });
        function closeLoading() {
            $.unblockUI();
        }
    </script>
    <style>
        .trBottom {
            margin-bottom: 10px;
        }

        td {
            color: white;
            text-align: left;
        }

        .flex-container {
            display: flex;
            flex-direction: row;
            align-content: center;
            align-items: center;
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
            min-height: 35px;
            font-size: 16px;
        }

        .settingBtn {
            background-color: red;
            color: white;
            margin: 5px;
            min-width: 160px;
            min-height: 35px;
            width: 95%;
        }

        .searchDiv {
            box-shadow: 3px;
            border-radius: 6px;
        }

        .updateBtn {
            margin: 10px;
            float: right;
        }

        @media screen and (max-width: 600px) {
            .minWidth {
                min-width: 50px;
                min-height: 15px;
                font-size: 12px;
            }

            legend {
                font-size: 18px;
            }
        }
    </style>
    <div class="container" style="background-color: #202648;">
        <div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                     <h2 id="HMIheading" runat="server" style="font-weight: bolder; color: ghostwhite; text-align: center;">HMI Setting
            </h2>
            <div class="flex-container" style="width: 100%; align-content: center; align-items: center;">
                <asp:Label ID="lblError" ClientIDMode="Static" Text="" runat="server" Style="color: red; visibility: visible; font-size: 16px; width: 100%; text-align: center; height:auto;min-height:25px;" />

            </div>
                </ContentTemplate>
                <Triggers >
                    <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
           
        </div>
        <div class="row" style="width: auto; flex-flow: column; color: white">
            <div class="col-md-1">
            </div>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="col-md-10 col-sm-12">
                        <div style="display: inline-block; padding-left: 15px; padding-right: 0px; width: 100%;">
                            <asp:Label runat="server" Text="Machine" CssClass="minWidth" Style="margin: 3px; align-content: center; font-size: 18px; vertical-align: middle;" />
                            <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control input-sm" Style="display: inline; margin: 3px; width: 65%; font-size: 16px;"></asp:DropDownList>
                            <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" Text="View" CssClass="btn btn-primary minWidth" Width="50px" Style="display: inline; margin: 3px;" />

                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-1">
        </div>


    </div>
    <div class="row" style="margin-top: 30px;">
        <div class="col-md-1 col-sm-0">
        </div>
        <div class="col-md-5 col-sm-12">
            <asp:UpdatePanel ID="SettingPanel" runat="server">
                <ContentTemplate>
                    <fieldset>
                        <legend>Admin Setting</legend>
                        <table runat="server" style="color: white; width: 100%; font-size: 16px;">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="BCD" CssClass="minWidth" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnBCD" OnClick="btnBCD_Click" Text="DISABLE" CssClass="settingBtn" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="BCD Threshold" CssClass="minWidth" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBCDThreshold" MaxLength="4" TextMode="Number" Text="0" CssClass="form-control-md " OnTextChanged="txtThreshold_TextChanged" Style="margin: 5px; width: 95%; height: 35px; text-align: center" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="ICD" CssClass="minWidth" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnICD" OnClick="btnBCD_Click" Text="DISABLE" CssClass="settingBtn" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="ICD Threshold" CssClass="minWidth" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtICDThreshold" MaxLength="4" TextMode="Number" Text="0" CssClass="form-control-md" OnTextChanged="txtThreshold_TextChanged" Style="margin: 5px; width: 95%; height: 35px; text-align: center" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Shift Alarm" CssClass="minWidth" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnShiftAlarm" OnClick="btnBCD_Click" Text="DISABLE" CssClass="settingBtn" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Schedule Bypass" CssClass="minWidth" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnScheduleBypass" OnClick="btnBCD_Click" Text="DISABLE" CssClass="settingBtn" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="EmpID Valid Bypass" CssClass="minWidth" Style="width: auto;" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnEmpIDValidBypass" OnClick="btnBCD_Click" Text="DISABLE" CssClass="settingBtn" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Schedule Mode" CssClass="minWidth" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnScheduleMode" OnClick="btnBCD_Click" Text="MANUAL" CssClass="settingBtn" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Rework" CssClass="minWidth" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnRework" OnClick="btnBCD_Click" Text="DISABLE" CssClass="settingBtn" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Rework %" CssClass="minWidth" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtReworkPercent" OnTextChanged="txtThreshold_TextChanged" Text="" CssClass="form-control-md" Style="margin: 5px; width: 95%; height: 35px; text-align: center" /></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" Text="Send To PLC" CssClass="btn btn-primary updateBtn" /></td>
                            </tr>

                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div class="col-md-5 col-sm-12">
            <asp:UpdatePanel ID="PasswordPanel" runat="server">
                <ContentTemplate>
                    <fieldset>
                        <legend>Password Setting</legend>
                        <table runat="server" style="color: white; width: 100%; font-size: 16px;">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Old Password" CssClass="minWidth" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOldPassword" MaxLength="4" CssClass="form-control-md" Style="font-size: 16px; height: 35px; width: 95%;" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="New Password" CssClass="minWidth" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNewPassword" MaxLength="4" TextMode="Password" CssClass="form-control-md" Style="font-size: 16px; height: 35px; width: 95%;" OnTextChanged="txtNewPassword_TextChanged" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Confirm Password" CssClass="minWidth" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtConfirmPassword" MaxLength="4" TextMode="Password" CssClass="form-control-md" Style="font-size: 16px; height: 35px; width: 95%;" /></td>
                            </tr>

                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button runat="server" ID="btnUpdatePassword" OnClick="btnUpdatePassword_Click" Text="Reset" CssClass="btn btn-primary updateBtn" /></td>
                            </tr>

                        </table>
                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
        <div class="col-md-1 col-sm-0">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
