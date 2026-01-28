<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Web_TPMTrakDashboard.SignIn" EnableEventValidation="false" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Login</title>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />


    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery-1.8.2.js"></script>


    <%--Browser Display Icon--%>
    <link href="favicon.ico" rel="icon" type="image/x-icon" />
    <!--===============================================================================================-->
    <%--  <link rel="icon" type="image/png" href="LoginDemo/images/icons/favicon.ico" />--%>
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginDemo/vendor/bootstrap/css/bootstrap.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginDemo/fonts/font-awesome-4.7.0/css/font-awesome.min.css" />
    <!--===============================================================================================-->
    <%--    <link rel="stylesheet" type="text/css" href="LoginDemo/vendor/animate/animate.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginDemo/vendor/css-hamburgers/hamburgers.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="LoginDemo/vendor/select2/select2.min.css" />
    <!--===============================================================================================-->--%>
    <%--  <link rel="stylesheet" type="text/css" href="LoginDemo/css/util.css" />--%>
    <link rel="stylesheet" type="text/css" href="LoginDemo/css/main.css" />



    <%--<style>
        .dropdownCss {
            background-color: #FAFFBD;
        }
    </style>--%>
    <style>
        .modalBtns {
            width: 100px;
            font-size: 22px;
            border: 1px solid #0367d8;
            border-radius: 3px;
            padding: 2px;
            background-color: #0367d8;
            color: white
        }

        .versionCs {
            margin-top: 15px;
            width: 100%;
            background-color: #e9f3d4;
            border-radius: 6px;
            padding: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hdnScreenHeight" />
        <asp:HiddenField runat="server" ID="hdnScreenWidth" />
        <asp:HiddenField runat="server" ID="hdnPageName" />
        <div class="limiter">
            <div class="container-login100">
                <div>
                    <div class="wrap-login100">
                        <asp:PlaceHolder runat="server" ID="pnlMessage" Visible="False" ViewStateMode="Disabled">
                            <p class="message-success" style="color: red"><%: SuccessMessage %></p>
                        </asp:PlaceHolder>

                        <form class="login100-form validate-form">
                            <span class="login100-form-title" style="padding-right: 33px;"><%=GetLocalResourceObject("LoginIn")%>
                            </span>
                            <asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessageResource1"></asp:Label>
                            <div class="wrap-input100 validate-input" data-validate="Valid email is required: ex@abc.xyz">
                                <input class="input100" type="text" name="email" placeholder="User ID" id="txtUserName" runat="server" autocomplete="off" />
                                <span class="focus-input100"></span>
                                <span class="symbol-input100">
                                    <i class="fa fa-user fa-lg" aria-hidden="true"></i>
                                </span>
                            </div>

                            <div class="wrap-input100 validate-input" data-validate="Password is required">
                                <input class="input100" type="password" name="pass" placeholder="Password" id="txtPassword" runat="server" />
                                <span class="focus-input100"></span>
                                <span class="symbol-input100">
                                    <i class="fa fa-lock fa-lg" aria-hidden="true"></i>
                                </span>
                            </div>

                            <div class="wrap-input100 validate-input" id="tdDomain" runat="server" visible="false">
                                <input class="input100" type="text" placeholder="Domain" id="txtDomainName" runat="server" disabled>
                                <span class="focus-input100"></span>
                                <span class="symbol-input100">
                                    <i class="fa fa-globe fa-lg" aria-hidden="true"></i>
                                </span>
                            </div>

                            <div class="wrap-input100 validate-input">
                                <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="input100 dropdownCss" meta:resourcekey="ddlLanguageResource1">
                                    <asp:ListItem Value="en" meta:resourcekey="ListItemResource1">English (United Kingdom)</asp:ListItem>
                                    <asp:ListItem Value="zh" meta:resourcekey="ListItemResource2">中文（简体，PRC）</asp:ListItem>
                                </asp:DropDownList>
                                <span class="focus-input100"></span>
                                <span class="symbol-input100">
                                    <i class="fa fa-language fa-lg" aria-hidden="true"></i>
                                </span>
                            </div>

                            <div class="wrap-input100 validate-input">
                                <asp:DropDownList ID="ddlConnectionString" runat="server" CssClass="input100 dropdownCss" meta:resourcekey="ddlConnectionStringResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlConnectionString_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="focus-input100"></span>
                                <span class="symbol-input100">
                                    <i class="fa fa-database fa-lg" aria-hidden="true"></i>
                                </span>
                            </div>

                            <div class="container-login100-form-btn">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" CssClass="login100-form-btn" meta:resourcekey="btnLoginResource1" />

                            </div>

                            <div style="width: 100%; font-size: 12px; margin-top: 20px">
                                <div style="float: left">
                                    Web VER:
                                    <asp:Label runat="server" ID="lblPackageVersion" ClientIDMode="Static"></asp:Label>
                                </div>
                                <div style="float: right">
                                    DB VER:
                                     <asp:Label runat="server" ID="lblDBVersion" ClientIDMode="Static" Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lblScriptName" ClientIDMode="Static"></asp:Label>
                                </div>

                                <div style="margin-top: 30px;">
                                    <asp:Label runat="server" ID="lblVersionMsg" ClientIDMode="Static" Style="font-size: 15px; color: #ff6700"></asp:Label>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>

            </div>
        </div>

        <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 600px">
                <div class="modal-content" style="border: 2px solid #f6d464">
                    <div class="modal-header" style="background-color: #ffcf30; padding: 8px">

                        <h4 class="modal-title" style="color: white; font-size: 25px">Warning!</h4>
                    </div>
                    <div class="modal-body" style="padding: 30px 15px">
                        <%--  <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                        <span id="lblWarningMsg" style="font-size: 23px;"></span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #f6d464; text-align: right">
                        <button type="button" data-dismiss="modal" class="modalBtns">OK</button>
                    </div>
                </div>
            </div>
        </div>

        <!--===============================================================================================-->
        <script src="LoginDemo/vendor/jquery/jquery-3.2.1.min.js"></script>
        <!--===============================================================================================-->
        <%--    <script src="LoginDemo/vendor/bootstrap/js/popper.js"></script>
    <script src="LoginDemo/vendor/bootstrap/js/bootstrap.min.js"></script>
    <!--===============================================================================================-->
    <script src="LoginDemo/vendor/select2/select2.min.js"></script>--%>
        <!--===============================================================================================-->
        <script src="LoginDemo/vendor/tilt/tilt.jquery.min.js"></script>

        <script src="MyCssAndJS/LoadingImgJs/JavaScriptUIBlocker.js"></script>

        <script src="Scripts/Toastr/bootstrap.js"></script>
        <link href="Scripts/Toastr/bootstrap.css" rel="stylesheet" />
        <script>
            function openWarningModal(msg) {
                $('#lblWarningMsg').text(msg);
                $('[id*=warningModal]').modal('show');
                //alert(msg);
            }
            $('.js-tilt').tilt({
                scale: 1.1
            });
            $("[id$=btnLogin]").click(function () {
                //document.body.style.cursor = 'wait';  
                if ($("[id$=txtUserName]").val() == "") {
                    alert('<%=GetLocalResourceObject("PleaseEnterUserId")%>');
                    return false;
                }
                if ($("[id$=txtPassword]").val() == "") {
                    alert('<%=GetLocalResourceObject("PleaseEnterPassword")%>');
                    return false;
                }
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
            $(document).ready(function () {
                maximizeWin();
            })
            function maximizeWin() {

                if (window.screen) {
                    //var aw = screen.availWidth;
                    //var ah = screen.availHeight;
                    $("#hdnScreenHeight").val(screen.availHeight);
                    $("#hdnScreenWidth").val(screen.availWidth);
                    //window.moveTo(0, 0);
                    //window.resizeTo(aw, ah);
                }
            }
        </script>
        <!--===============================================================================================-->
        <script src="LoginDemo/js/main.js"></script>
    </form>
</body>
</html>
