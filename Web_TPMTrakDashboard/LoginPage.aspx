w<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="Web_TPMTrakDashboard.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%: Page.Title %> - TPM-Trak Analytics</title>

    <link href="css/bootstrap-theme.css" rel="stylesheet" />

    <style type="text/css" media="screen">
        body, html {
            padding: 0;
            margin: 0;
            height: 100%;
            width: 100%;
        }

        #menu {
            position: fixed;
            top: 0;
            height: 50px;
            width: 100%;
            background: white;
            box-shadow: 0 0 10px black;
        }

        .part2 {
            background: #266590;
            color: white;
            display: inline-block;
            vertical-align: bottom;
            /*border-radius: 5px 0 0 0;*/
            font-family: Cambria;
            /*margin-top: 2px;*/
        }

        .part4 {
            float: right;
            height: 100%;
            background: white;
        }

        h1.myheader {
            height: 30px;
            margin: 10px;
            min-width: 100px;
            font-weight: bold;
            font-size: 1.5em;
        }

        .companyicon {
            height: 50px;
            clip: rect(10px,10px,10px,10px);
            min-width: 100px;
        }

        #divBody {
            padding: 55px 0;
            width: 100%;
        }

        .footerDiv {
            position: fixed;
            bottom: 0;
            height: 23px;
            width: 100%;
            background: white;
            font-family: Cambria;
            box-shadow: 0px 0px 10px black;
        }

            .footerDiv footer {
                height: 100%;
                font-size: 14px;
                font-weight: 700;
                color: #266590;
                font-family: Cambria;
            }

        .footerLeft {
            float: left;
            margin-left: 2px;
        }

        .footerRight {
            float: right;
            margin-right: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
        </asp:ScriptManager>

        <div id="menu">
            <div class="part2">
                <h1 class="myheader">BanP MFE Elements</h1>
            </div>

            <div class="part4">
                <asp:Image ID="Image1" runat="server" CssClass="companyicon" ImageUrl="CompanyLogo/bosch_logo_english3.png" />
            </div>
        </div>

        <div id="divBody">
            <div class="container">
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessage" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
                </div>
                <div class="row" style="font-family: Calibri;">
                    <div class="col-sm-6 col-md-4 col-md-offset-4">
                        <h1 class="text-center login-title">Sign in to continue</h1>
                        <div class="account-wall">
                            <div class="form-signin">
                                <table class="table table-bordered table-striped">
                                    <tr>
                                        <td style="text-align: left;">
                                            <label class="control-label">User Name</label></td>
                                        <td>
                                            <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="User Name" required autofocus></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <label class="control-label">Password</label></td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password" placeholder="Password" required></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="tdDomain" runat="server" visible="false">
                                        <td style="text-align: left;">
                                            <label class="control-label">Domain Name</label></td>
                                        <td>
                                            <asp:TextBox ID="txtDomainName" runat="server" class="form-control" placeholder="Domain" disabled></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" colspan="2">
                                            <asp:Button runat="server" ID="btnSignIn" Text="Sign in" CssClass="btn btn-primary btn-block" Style="background-color: #337AB7; color: #f9f9f9" OnClick="btnSignIn_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="footerDiv">
            <footer>
                <div class="footerLeft">Powered by TPM-Trak</div>

                <div class="footerRight">
                    <asp:UpdatePanel ID="update" runat="server">
                        <ContentTemplate>
                            Data updated at  <span id="spandatetime"><%=DateTime.Now.ToString("HH:mm, dd'th' MMMM yyyy") %></span>.
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </footer>
        </div>
    </form>
</body>
</html>
