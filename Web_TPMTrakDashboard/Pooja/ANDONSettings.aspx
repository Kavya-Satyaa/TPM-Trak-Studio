<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ANDONSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.Pooja.ANDONSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ANDON Settings</title>
    <%--  <%: Styles.Render("~/bundles/toastrJs") %>      
      <%: Styles.Render("~/bundles/toastrCss") %>   --%>
    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <link href="../Scripts/ColorPickerJs/css/pick-a-color-1.2.2.min.css" rel="stylesheet" />
    <script src="../Scripts/ColorPickerJs/dependencies/jquery-1.9.1.min.js"></script>
    <script src="../Scripts/ColorPickerJs/dependencies/tinycolor-0.9.15.min.js"></script>
    <script src="../Scripts/ColorPickerJs/js/pick-a-color-1.2.2.min.js"></script>
    <script src="../MyCssAndJS/tosterJs/toastr.min.js"></script>
    <link href="../MyCssAndJS/tosterJs/toastr.css" rel="stylesheet" />

    <style>
        * {
            font-family: Arial;
            font-size: 15px;
        }

        body {
            margin: 0px;
        }

        .header {
            display: inline-block;
            width: 100%;
            background-color: #1a2732;
            color: white;
            font-weight: bold;
            height: 74px;
            padding: 2px;
            position: sticky;
            top: 0px;
            text-align: center;
        }

        .div-logo {
            float: left;
        }

            .div-logo img {
                border-radius: 5px;
            }

        .div-right-header-content {
            float: right;
            min-width: 100px;
            position: relative;
            top: 19px;
        }

        .div-headername {
            font-size: 30px;
            margin: auto;
        }

        fieldset {
        }

        legend {
            display: block;
            width: 13%;
            padding: 0;
            margin-bottom: 0px;
            font-size: 18px;
            color: #333;
        }

        .table-style {
            width: 100%;
            font-size: 14px;
        }

            .table-style tr td {
                padding: 7px;
            }
    </style>

    <link href="../AndonContent/bootstrap-glyphicons.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <div id="header" class="header">
            <div class="div-logo">
                <%--width: 200px; height: 48px;--%>
                <asp:Image ID="Image1" runat="server" class="img-responsive img-rounded" Style=" height: 70px; width: 130px;" AlternateText="Company Logo" />
            </div>
            <div class="div-right-header-content">
                <i class="glyphicon glyphicon-share-alt" title="Go To ANDON Screen" style="font-size: 25px; color: #49b8d3" onclick="location.href='PoojaANDON.aspx';"></i>
            </div>
            <div class="div-headername"><span style="font-size: 30px; vertical-align: -webkit-baseline-middle">ANDON Settings</span></div>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container-fluid" style="margin: auto; width: fit-content; padding-top: 43px;">
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <fieldset>
                                <legend>Settings</legend>
                                <table class="table-style">
                                    <tr>
                                        <td><span>Font Name</span></td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlFontName" CssClass="form-control">
                                                <asp:ListItem Value="Aharoni">Aharoni</asp:ListItem>
                                                <asp:ListItem Value="Arial">Arial</asp:ListItem>
                                                <asp:ListItem Value="Arial Black">Arial Black</asp:ListItem>
                                                <asp:ListItem Value="Baskerville Old Face">Baskerville Old Face</asp:ListItem>
                                                <asp:ListItem Value="Bodoni MT Black">Bodoni MT Black</asp:ListItem>
                                                <asp:ListItem Value="Bookman Old Style">Bookman Old Style</asp:ListItem>
                                                <asp:ListItem Value="Calibri">Calibri</asp:ListItem>
                                                <asp:ListItem Value="Cooper Black">Cooper Black</asp:ListItem>
                                                <asp:ListItem Value="Californian FB">Californian FB</asp:ListItem>
                                                <asp:ListItem Value="Constantia">Constantia</asp:ListItem>
                                                <asp:ListItem Value="Elephant">Elephant</asp:ListItem>
                                                <asp:ListItem Value="Georgia">Georgia</asp:ListItem>
                                                <asp:ListItem Value="Goudy Old Style">Goudy Old Style</asp:ListItem>
                                                <asp:ListItem Value="High Tower Text">High Tower Text</asp:ListItem>
                                                <asp:ListItem Value="Segoe UI">Segoe UI</asp:ListItem>
                                                <asp:ListItem Value="Segoe UI Semibold">Segoe UI Semibold</asp:ListItem>
                                                <asp:ListItem Value="Segoe WP Black">Segoe WP Black</asp:ListItem>
                                                <asp:ListItem Value="Times New Roman">Times New Roman</asp:ListItem>
                                                <asp:ListItem Value="Tw Cen MT">Tw Cen MT</asp:ListItem>
                                                <asp:ListItem Value="Verdana">Verdana</asp:ListItem>
                                                <asp:ListItem Value="Vijaya">Vijaya</asp:ListItem>
                                                <asp:ListItem Value="Vrinda">Vrinda</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>Table Header FontSize</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlTableHeaderFontSize" CssClass="form-control">
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                <asp:ListItem Value="13">13</asp:ListItem>
                                                <asp:ListItem Value="14">14</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="16">16</asp:ListItem>
                                                <asp:ListItem Value="17">17</asp:ListItem>
                                                <asp:ListItem Value="18">18</asp:ListItem>
                                                <asp:ListItem Value="19">19</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="21">21</asp:ListItem>
                                                <asp:ListItem Value="22">22</asp:ListItem>
                                                <asp:ListItem Value="23">23</asp:ListItem>
                                                <asp:ListItem Value="24">24</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem Value="26">26</asp:ListItem>
                                                <asp:ListItem Value="27">27</asp:ListItem>
                                                <asp:ListItem Value="28">28</asp:ListItem>
                                                <asp:ListItem Value="29">29</asp:ListItem>
                                                <asp:ListItem Value="30">30</asp:ListItem>
                                                <asp:ListItem Value="31">31</asp:ListItem>
                                                <asp:ListItem Value="32">32</asp:ListItem>
                                                <asp:ListItem Value="33">33</asp:ListItem>
                                                <asp:ListItem Value="34">34</asp:ListItem>
                                                <asp:ListItem Value="35">35</asp:ListItem>
                                                <asp:ListItem Value="36">36</asp:ListItem>
                                                <asp:ListItem Value="37">37</asp:ListItem>
                                                <asp:ListItem Value="38">38</asp:ListItem>
                                                <asp:ListItem Value="39">39</asp:ListItem>
                                                <asp:ListItem Value="40">40</asp:ListItem>
                                                <asp:ListItem Value="41">41</asp:ListItem>
                                                <asp:ListItem Value="42">42</asp:ListItem>
                                                <asp:ListItem Value="43">43</asp:ListItem>
                                                <asp:ListItem Value="44">44</asp:ListItem>
                                                <asp:ListItem Value="45">45</asp:ListItem>
                                                <asp:ListItem Value="46">46</asp:ListItem>
                                                <asp:ListItem Value="47">47</asp:ListItem>
                                                <asp:ListItem Value="48">48</asp:ListItem>
                                                <asp:ListItem Value="49">49</asp:ListItem>
                                                <asp:ListItem Value="50">50</asp:ListItem>
                                                <asp:ListItem Value="51">51</asp:ListItem>
                                                <asp:ListItem Value="52">52</asp:ListItem>
                                                <asp:ListItem Value="53">53</asp:ListItem>
                                                <asp:ListItem Value="54">54</asp:ListItem>
                                                <asp:ListItem Value="55">55</asp:ListItem>
                                                <asp:ListItem Value="56">56</asp:ListItem>
                                                <asp:ListItem Value="57">57</asp:ListItem>
                                                <asp:ListItem Value="58">58</asp:ListItem>
                                                <asp:ListItem Value="59">59</asp:ListItem>
                                                <asp:ListItem Value="60">60</asp:ListItem>
                                                <asp:ListItem Value="61">61</asp:ListItem>
                                                <asp:ListItem Value="62">62</asp:ListItem>
                                                <asp:ListItem Value="63">63</asp:ListItem>
                                                <asp:ListItem Value="64">64</asp:ListItem>
                                                <asp:ListItem Value="65">65</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>Table Content FontSize</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlTableContentFontSize" CssClass="form-control">
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                <asp:ListItem Value="13">13</asp:ListItem>
                                                <asp:ListItem Value="14">14</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="16">16</asp:ListItem>
                                                <asp:ListItem Value="17">17</asp:ListItem>
                                                <asp:ListItem Value="18">18</asp:ListItem>
                                                <asp:ListItem Value="19">19</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="21">21</asp:ListItem>
                                                <asp:ListItem Value="22">22</asp:ListItem>
                                                <asp:ListItem Value="23">23</asp:ListItem>
                                                <asp:ListItem Value="24">24</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem Value="26">26</asp:ListItem>
                                                <asp:ListItem Value="27">27</asp:ListItem>
                                                <asp:ListItem Value="28">28</asp:ListItem>
                                                <asp:ListItem Value="29">29</asp:ListItem>
                                                <asp:ListItem Value="30">30</asp:ListItem>
                                                <asp:ListItem Value="31">31</asp:ListItem>
                                                <asp:ListItem Value="32">32</asp:ListItem>
                                                <asp:ListItem Value="33">33</asp:ListItem>
                                                <asp:ListItem Value="34">34</asp:ListItem>
                                                <asp:ListItem Value="35">35</asp:ListItem>
                                                <asp:ListItem Value="36">36</asp:ListItem>
                                                <asp:ListItem Value="37">37</asp:ListItem>
                                                <asp:ListItem Value="38">38</asp:ListItem>
                                                <asp:ListItem Value="39">39</asp:ListItem>
                                                <asp:ListItem Value="40">40</asp:ListItem>
                                                <asp:ListItem Value="41">41</asp:ListItem>
                                                <asp:ListItem Value="42">42</asp:ListItem>
                                                <asp:ListItem Value="43">43</asp:ListItem>
                                                <asp:ListItem Value="44">44</asp:ListItem>
                                                <asp:ListItem Value="45">45</asp:ListItem>
                                                <asp:ListItem Value="46">46</asp:ListItem>
                                                <asp:ListItem Value="47">47</asp:ListItem>
                                                <asp:ListItem Value="48">48</asp:ListItem>
                                                <asp:ListItem Value="49">49</asp:ListItem>
                                                <asp:ListItem Value="50">50</asp:ListItem>
                                                <asp:ListItem Value="51">51</asp:ListItem>
                                                <asp:ListItem Value="52">52</asp:ListItem>
                                                <asp:ListItem Value="53">53</asp:ListItem>
                                                <asp:ListItem Value="54">54</asp:ListItem>
                                                <asp:ListItem Value="55">55</asp:ListItem>
                                                <asp:ListItem Value="56">56</asp:ListItem>
                                                <asp:ListItem Value="57">57</asp:ListItem>
                                                <asp:ListItem Value="58">58</asp:ListItem>
                                                <asp:ListItem Value="59">59</asp:ListItem>
                                                <asp:ListItem Value="60">60</asp:ListItem>
                                                <asp:ListItem Value="61">61</asp:ListItem>
                                                <asp:ListItem Value="62">62</asp:ListItem>
                                                <asp:ListItem Value="63">63</asp:ListItem>
                                                <asp:ListItem Value="64">64</asp:ListItem>
                                                <asp:ListItem Value="65">65</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Content Bold</td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="cbBold" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>Screen Flip Interval (ss)</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlScreenFlipInterval" CssClass="form-control">
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="30">30</asp:ListItem>
                                                <asp:ListItem Value="40">40</asp:ListItem>
                                                <asp:ListItem Value="50">50</asp:ListItem>
                                                <asp:ListItem Value="60">60</asp:ListItem>
                                                <asp:ListItem Value="70">70</asp:ListItem>
                                                <asp:ListItem Value="80">80</asp:ListItem>
                                                <asp:ListItem Value="90">90</asp:ListItem>
                                                <asp:ListItem Value="100">100</asp:ListItem>
                                                <asp:ListItem Value="110">110</asp:ListItem>
                                                <asp:ListItem Value="120">120</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>No. Of Machines to Display</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNoOfMachineToDiaply" CssClass="form-control allowNumber" AutoCompleteType="Disabled"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding: 0px">
                                            <span style="color: #094adb">* If suppose values is blank or 0 then record will be displayed based on screen height</span>
                                        </td>
                                    </tr>
                                  <%--  <tr>
                                        <td colspan="2" style="text-align: center">
                                         
                                        </td>
                                    </tr>--%>
                                </table>
                            </fieldset>

                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <fieldset>
                                <legend>Naming</legend>
                                <table class="table-style">
                                    <tr>
                                        <td>Main Header Name</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtMainHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Machine</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtMachineHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Running Component & Opn</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtComponentAndOpnHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>OEE</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>DownTime</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDownTimeHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Operator</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOperatorHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Production Target</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtProductionTargetHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Actual</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtActualHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Status</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtStatusHeaderName" CssClass="form-control " AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center; margin-top: 10px">
                               <asp:Button runat="server" CssClass="btn btn-success" Text="Save" ID="btnSave" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <%--<%: Styles.Render("~/bundles/masterjs") %>--%>
        <script>
            $('.allowNumber').keypress(function (evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                //if (charCode == 48 && pos == 0) {
                //    return false
                //} else
                if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            });
            function showSuccessMsg(msg, title) {
                debugger;
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": true,
                    "onclick": null,
                    "showDuration": "4000",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut",
                    "toastClass": "toaster-position"
                }

                toastr['success'](msg, title);
                return false;
            }
        </script>
    </form>
</body>
</html>
