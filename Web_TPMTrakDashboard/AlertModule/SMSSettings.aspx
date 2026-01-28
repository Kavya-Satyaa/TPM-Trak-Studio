<%@ Page Title="SMS Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SMSSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.SMSSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>
    <div>
        <asp:UpdatePanel runat="server">
            <%-- <Triggers>
                <asp:PostBackTrigger ControlID="cmbsmsMethod" />
                <asp:PostBackTrigger ControlID="btnSaveAPIAddress" />
                <asp:PostBackTrigger ControlID="btnPortSettings" />
                <asp:PostBackTrigger ControlID="cmbEmailMethod" />
                <asp:PostBackTrigger ControlID="btnSaveEmailSettings" />
            </Triggers>--%>
            <ContentTemplate>
                <div>
                    <div class="col-lg-6" style="height: 150px">
                        <div class="column ui segment" style="height: 100px" id="divStyle">
                            <h3>SMS API Settings</h3>
                            <table>
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="apiName">Api Address</span>
                                    </td>
                                    <td class="two wide column">
                                        <asp:TextBox runat="server" ID="txtApiLink" CssClass="form-control" Width="350" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <div class="col-lg-6" style="height: 150px">
                        <div class="column ui segment" style="height: 100px" id="divStyle1">
                            <h3>Telegram  Settings</h3>
                            <table>
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span1">Api Address</span>
                                    </td>
                                    <td class="two wide column">
                                        <asp:TextBox runat="server" ID="txtTelegramApiLink" CssClass="form-control" Width="350" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div>
                    <div class="col-lg-6" style="height: 250px">
                        <div class="column ui segment" style="height: 200px" id="divStyle2">
                            <h3>Email Settings
                            </h3>
                            <table>
                                <tr>
                                    <td style="width: 150px">
                                        <span>
                                            <asp:Label runat="server" ID="lblServer" Text="SMTP Server" />
                                        </span>
                                    </td>
                                    <td style="width: 220px">
                                        <asp:TextBox runat="server" ID="txtSMTPSrrver" CssClass="form-control" Width="200" />
                                    </td>
                                    <td style="width: 150px">
                                        <span>Email-Method</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="cmbEmailMethod" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="cmbEmailMethod_SelectedIndexChanged">
                                            <asp:ListItem Text="SMTP" Value="SMTP" />
                                            <asp:ListItem Text="EWS" Value="EWS" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px">
                                        <span>User-ID</span>
                                    </td>
                                    <td style="width: 220px">
                                        <asp:TextBox runat="server" ID="txtUserID" CssClass="form-control" Width="200" />
                                    </td>
                                    <td style="width: 150px">
                                        <span runat="server" id="lblPorts">SMTP Port No.</span>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPort" CssClass="form-control" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px">
                                        <span>Password</span>
                                    </td>
                                    <td style="width: 220px">
                                        <asp:TextBox TextMode="Password" runat="server" ID="txtPassword" Text="asdadsasd" CssClass="form-control" Width="200" />
                                    </td>
                                    <td style="width: 150px">
                                        <span runat="server" id="lblEnableSSL">Enable SSl</span>
                                    </td>
                                    <td style="vertical-align: middle">
                                        <asp:CheckBox runat="server" ID="chkSSlEnable" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <div class="col-lg-6" style="height: 250px">
                        <div class="column ui segment" style="height: 200px" id="divStyle3">
                            <h3>COM Port Settings</h3>
                            <table>
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span10">COM Port Number</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCOMPortNo" CssClass="form-control" Style="width: 160px;padding: 3px 12px;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span11">Flow Control</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFlowControl" CssClass="form-control" Style="width: 160px;padding: 3px 12px;">
                                             <asp:ListItem Value="Xon/Xoff">Xon/Xoff</asp:ListItem>
                                             <asp:ListItem Value="Hardware">Hardware</asp:ListItem>
                                             <asp:ListItem Value="None">None</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                     <td style="width: 180px">
                                        <span runat="server" id="Span12">Settings</span>
                                    </td>
                                     <td>
                                        <asp:DropDownList runat="server" ID="ddlSettingsVal1" CssClass="form-control" Style="width: 160px;padding: 3px 12px;">
                                            <asp:ListItem Value="110">110</asp:ListItem>
                                            <asp:ListItem Value="300">300</asp:ListItem>
                                            <asp:ListItem Value="600">600</asp:ListItem>
                                            <asp:ListItem Value="1200">1200</asp:ListItem>
                                            <asp:ListItem Value="2400">2400</asp:ListItem>
                                            <asp:ListItem Value="4800">4800</asp:ListItem>
                                            <asp:ListItem Value="9600">9600</asp:ListItem>
                                            <asp:ListItem Value="14400">14400</asp:ListItem>
                                            <asp:ListItem Value="19200">19200</asp:ListItem>
                                            <asp:ListItem Value="28800">28800</asp:ListItem>
                                            <asp:ListItem Value="38400">38400</asp:ListItem>
                                            <asp:ListItem Value="56000">56000</asp:ListItem>
                                            <asp:ListItem Value="57600">57600</asp:ListItem>
                                            <asp:ListItem Value="115200">115200</asp:ListItem>
                                            <asp:ListItem Value="128000">128000</asp:ListItem>
                                            <asp:ListItem Value="256000">256000</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                         <asp:DropDownList runat="server" ID="ddlSettingsVal2" CssClass="form-control" Style="width: 160px;padding: 3px 12px;">
                                             <asp:ListItem Value="Even">Even</asp:ListItem>
                                             <asp:ListItem Value="Odd">Odd</asp:ListItem>
                                             <asp:ListItem Value="None">None</asp:ListItem>
                                             <asp:ListItem Value="Mark">Mark</asp:ListItem>
                                             <asp:ListItem Value="Space">Space</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                         <asp:DropDownList runat="server" ID="ddlSettingsVal3" CssClass="form-control" Style="width: 160px; padding: 3px 12px;">
                                             <asp:ListItem Value="4">4</asp:ListItem>
                                             <asp:ListItem Value="5">5</asp:ListItem>
                                             <asp:ListItem Value="6">6</asp:ListItem>
                                             <asp:ListItem Value="7">7</asp:ListItem>
                                             <asp:ListItem Value="8">8</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                     <td>
                                         <asp:DropDownList runat="server" ID="ddlSettingsVal4" CssClass="form-control" Style="width: 160px; padding: 3px 12px;">
                                             <asp:ListItem Value="1">1</asp:ListItem>
                                             <asp:ListItem Value="1.5">1.5</asp:ListItem>
                                             <asp:ListItem Value="2">2</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </div>
                <div style="display:inline-block;width:100%">
                     <div class="col-lg-6" style="height: 250px;" >
                        <div class="column ui segment" style="height: 200px" id="">
                            <h3>Enable Settings</h3>
                            <table>
                                
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span3">Internet Gateway</span>
                                    </td>
                                    <td style="width: 400px">
                                        <asp:CheckBox runat="server" ID="chkApiEnabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span4">Telegram Enabled</span>
                                    </td>
                                    <td style="width: 400px">
                                        <asp:CheckBox runat="server" ID="chkTelegramEnabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span5">Email Enabled</span>
                                    </td>
                                    <td style="width: 400px">
                                        <asp:CheckBox runat="server" ID="chkEnabledEmailSettings"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span6">Mobile Enabled</span>
                                    </td>
                                    <td style="width: 400px">
                                        <asp:CheckBox runat="server" ID="chkMobileEnabled" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width: 180px">
                                        <span runat="server" id="Span7">GSM Modem</span>
                                    </td>
                                    <td style="width: 400px">
                                        <asp:CheckBox runat="server" ID="chkGSMModem" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>

                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div style="float:right;">
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="ui violet button" OnClick="btnSave_Click" style="width:122px;height:46px"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        //$(document).ready(function () {
        //    HideShow();
        //});

        //function HideShow() {
        //    $('#divStyle3 *').children().prop('disabled', true);
        //    $('#divStyle *').children().prop('disabled', true);
        //    $('#divStyle1 *').children().prop('disabled', true);
        //    $('#divStyle2 *').children().prop('disabled', true);
        //    $.ajax({
        //        type: "POST",
        //        url: "SMSSettings.aspx/GetDetails",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            var itmData = response.d;
        //            if (itmData != null) {
        //                for (let i = 0; i < itmData.length; i++) {
        //                    if (itmData[i].Value) {
        //                        if (itmData[i].Text == "GSMMODEM") {
        //                            $('#divStyle3 *').children().prop('disabled', false);
        //                        }
        //                        if (itmData[i].Text == "INTERNETGATEWAY") {
        //                            $('#divStyle *').children().prop('disabled', false);
        //                        }
        //                        if (itmData[i].Text == "TELEGRAM") {
        //                            $('#divStyle1 *').children().prop('disabled', false);
        //                        }
        //                        if (itmData[i].Text == "EMAIL") {
        //                            $('#divStyle2 *').children().prop('disabled', false);
        //                        }
        //                    }

        //                }
        //            }
        //        },
        //        error: function (response) {
        //            alert("Error");
        //        }
        //    });
        //}

        //var prm = Sys.WebForms.PageRequestManager.getInstance();
        //prm.add_endRequest(function () {
        //    $(document).ready(function () {
        //        HideShow();
        //    });

        //});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
