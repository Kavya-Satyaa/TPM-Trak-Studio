<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web_TPMTrakDashboard.Login" meta:resourcekey="PageResource1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title><%: Page.Title %> - TPM-Trak Analytics</title>

    <%--  <link href="googleFonts/opensans.css" rel="stylesheet" />--%>
    <link rel="stylesheet" href="css/logincss/style.css" />
    <link href="css/logincss/reset.css" rel="stylesheet" />
    <%--   <link href="css/logincss/weloveiconfonts.css" rel="stylesheet" />--%>
    <style>
        #myDiv {
            text-align: center;
        }

        #btnLogin {
            background-color: #394A59;
            color: white;
            font-size: 21px;
            width: 100px;
            font-family: inherit;
            cursor: pointer;
        }

        #login fieldset input[type="text"], #login fieldset input[type="password"] {
            border-style: ridge;
            border-radius: 9px;
            font-family: inherit;
        }

        .dropdownCss {
            /*background-image:url(../images/arrow.png);*/
            /*background-repeat: no-repeat;
            background-position: 300px;*/
            width: 353px;
            padding: 12px;
            /*margin-top: 8px;*/
            /*font-family: Cursive;*/
            line-height: 1;
            border-radius: 9px;
            background-color: #FAFFBD;
            /*color: #ff0;*/
            font-size: 14px;
            -webkit-appearance: none;
            /*box-shadow: inset 0 0 10px 0 rgba(0,0,0,0.6);*/
            /*outline: none;*/
        }
        /*body {
            font-family: 'Open Sans';
            font-size: 22px;
        }*/
    </style>
    <%--    <%: Styles.Render("~/bundles/logincss") %>
    <%: Scripts.Render("~/bundles/loginjs") %>--%>
</head>
<body>

    <div class="container" id="myDiv">
        <asp:PlaceHolder runat="server" ID="pnlMessage" Visible="False" ViewStateMode="Disabled">
            <p class="message-success" style="color: red"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
        <div id="login" style="border: 1px;">
            <h2 style="text-align: left;">
                <img src="Image/ChartImg/padlock (1).png" />
                <%=GetLocalResourceObject("SignIn") %>      </h2>
            <form id="form1" runat="server">
                <asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessageResource1"></asp:Label>
                <fieldset>

                    <p style="text-align: left;">
                        <img src="Image/ChartImg/users.png" />&nbsp;
                       <%=GetLocalResourceObject("UserID") %>
                    </p>
                    <p>
                        <input type="text" id="txtUserName" runat="server" placeholder="user Id" />
                    </p>

                    <p style="text-align: left;">

                        <img src="Image/ChartImg/padlock (2).png" />&nbsp;<%=GetLocalResourceObject("Password") %>
                    </p>
                    <p>
                        <input type="password" id="txtPassword" runat="server" placeholder="password" />
                    </p>
                    <p id="tdDomain" runat="server" visible="false">
                        <input id="txtDomainName" runat="server" placeholder="Domain" disabled />
                    </p>
                    <p style="text-align: left;">

                        <img src="Image/ChartImg/translate.png" />&nbsp;<%=GetLocalResourceObject("Language") %>
                    </p>
                    <p>
                        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdownCss">
                            <asp:ListItem Value="en">English (United Kingdom)</asp:ListItem>
                            <asp:ListItem Value="zh">中文（简体，PRC）</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                     <p style="text-align: left;">
                        <img src="Image/ChartImg/database (2).png" />&nbsp;<%=GetLocalResourceObject("Server") %>
                    </p>
                    <p>
                        <asp:DropDownList ID="ddlConnectionString" runat="server" CssClass="dropdownCss">
                        </asp:DropDownList>
                    </p>
                    <p>
                        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Log In" meta:resourcekey="btnLoginResource1" />
                    </p>
                </fieldset>
            </form>
        </div>
    </div>
    <script src="MyCssAndJS/toggel/jquery.min.js"></script>
    <script src="MyCssAndJS/LoadingImgJs/JavaScriptUIBlocker.js"></script>

    <script>

        $(document).ready(function () {
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
        });

    </script>
    <!-- end login -->
</body>
</html>
