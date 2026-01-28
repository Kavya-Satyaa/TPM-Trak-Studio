<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SerialNoTrackingDetails_Highway.aspx.cs" Inherits="Web_TPMTrakDashboard.HighWay.SerialNoTrackingDetails_Highway" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="../Scripts/DateTimePicker/bootstrap-datepicker3.css" rel="stylesheet" />
    <script src="../Scripts/DateTimePicker/bootstrap-datepicker.js"></script>
    <style>
        .tblSettings {
            width: 100%;
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
                max-width: 150px;
                /*box-shadow: 2px 2px 2px black;*/
            }

        .btnclass {
            box-shadow: 1px 1px 10px #515151;
            font-size: 16px;
        }

        .outer-table {
            height: 1px;
        }

        .inner-table {
            width: 100%;
            height: 100%;
            min-height: 100%;
            max-height: 100%;
        }

            .inner-table tr td {
                width: 150px;
                text-align: center;
            }

        .outer-table > tbody > tr:first-child td {
            background-color: #2E6886 !important;
            color: white;
            font-weight: bold;
        }

        .outer-table tr td {
            text-align: center;
        }

        .table {
            margin: 0px;
        }

        .tabletd {
            margin-bottom: 0px;
        }

            .tabletd > tbody > tr > td {
                border-top: 0px;
                border-collapse: collapse;
                width: 120px;
                min-width: 120px;
                max-width: 120px;
            }

        .tableborder > tbody > tr > td {
            border-right: 1px solid #ddd;
            border-left: 1px solid #ddd;
            overflow-wrap: break-word;
        }

        .class1 {
            background-color: #2e6886;
        }

        .class2 {
            background-color: #c4e8f9;
            border: 1px solid #00000040 !important;
        }

        .Headertbl {
            text-align: center;
        }

        #tblOuter > tbody > tr:first-child {
            position: sticky;
            top: -1px;
            z-index: 1;
            text-align: center;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table class="tblSettings">
                    <tr>
                        <td>Plant</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPlantID" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlPlantID_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td>Cell</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCell" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td>Component</td>
                        <td>
                            <asp:ListBox runat="server" CssClass="form-control" ID="lbComponent" ClientIDMode="Static" SelectionMode="Multiple" OnSelectedIndexChanged="lbComponent_SelectedIndexChanged"></asp:ListBox>
                        </td>
                        <td>Serial No</td>
                        <td>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSerialNo" ClientIDMode="Static" placeholder="Serial Number search.."></asp:TextBox>
                            <%--<asp:ListBox runat="server" CssClass="form-control" ID="lbSerialNo" ClientIDMode="Static" SelectionMode="Multiple"></asp:ListBox>--%>
                        </td>
                        <td>From Date</td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control Date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td>To Date</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control Date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnView" ClientIDMode="Static" Text="VIEW" CssClass="bajaj-btn-style btnclass" OnClick="btnView_Click" />
                        </td>
                    </tr>
                </table>
                <div style="height: 80vh; overflow: auto; width: 100%; margin-top: 1%;">
                    <asp:ListView runat="server" ID="lvGrid" ClientIDMode="Static" ItemPlaceholderID="itemplaceholder">
                        <LayoutTemplate>
                            <table class="table table-bordered outer-table" id="tblOuter" runat="server">
                                <tr runat="server" id="itemplaceholder">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class='<%# Eval("HeaderEnable").ToString() == "true"? "class1" : "class2" %>'>
                                    <asp:Label runat="server" ID="lblHeader1" Text="Serial Number" Visible='<%# Eval("HeaderEnable") %>' ForeColor="White" CssClass="Headertbl"></asp:Label>
                                    <asp:Label runat="server" ID="lblContent" Text='<%# Eval("SerialNo") %>' Visible='<%# Eval("ContentEnable") %>' Font-Bold="true"></asp:Label>
                                </td>
                                <td class='<%# Eval("HeaderEnable").ToString() == "true"? "class1" : "class2" %>'>
                                    <asp:Label runat="server" ID="lblHeader2" Text="Component ID" Visible='<%# Eval("HeaderEnable") %>' ForeColor="White" CssClass="Headertbl"></asp:Label>
                                    <asp:Label runat="server" ID="lblComponentID" Text='<%# Eval("ComponentID") %>' Visible='<%# Eval("ContentEnable") %>' Font-Bold="true"></asp:Label>
                                </td>
                                <td runat="server" style="padding: 0px !important; border-top: 0px;">
                                    <asp:ListView runat="server" ID="lvMachineData" DataSource='<%# Eval("componentData") %>'>
                                        <LayoutTemplate>
                                            <table class="table inner-table" style="width: 100%;">
                                                <tr>
                                                    <td id="itemplaceholder" runat="server"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td style="padding: 0px; margin: 0px; border-top: 0px;">
                                                <table class="table inner-table tableborder tabletd" style="width: 100%;">
                                                    <tr runat="server" visible='<%# Eval("HeaderEnable") %>' style="border-bottom: 1px solid #ddd; border-right: 1px solid #ddd;">
                                                        <th class="text-center class1" style="border-top:0px;">
                                                            <%--<asp:Label runat="server" ID="lblOpeartionHeader" Text='<%# Eval("OperationNo")%>' ForeColor="White"></asp:Label>--%>
                                                            <asp:Label runat="server">
                                                                 <p style="margin: 0;"><%# Eval("OpnNo") %></p>
                                                                 <p style="margin: 0;"><%# Eval("OperationDesc") %></p>
                                                            </asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" visible='<%# Eval("ContentEnable") %>'>
                                                        <td>
                                                            <%--  <span class='<%# Eval("OperationData") %>' style='<%# "color:" + Eval("OperationDataBackcolor") + ";font-size:25px;border-radius:50%;padding:7px;background-color:"+ Eval("OperationDataBackgroundcolor")+";border:1px solid "+Eval("OperationDataBackgroundcolor")+";" %>'></span>--%>
                                                            <span class='<%# Eval("OperationData") %>' style='<%# "color:white;font-size:25px;border-radius:50%;padding:7px;background-color:"+ Eval("OperationDataBackgroundcolor")+";border:1px solid "+Eval("OperationDataBackgroundcolor")+";" %>'></span>
                                                            <%--<asp:Label runat="server" ID="Label1" Text='<%# Eval("OperationData") %>'></asp:Label>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=lbComponent]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbSerialNo]').multiselect({
                includeSelectAllOption: true
            });
        });
        function ViewClick() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $('.Date').datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $('[id$=lbComponent]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbSerialNo]').multiselect({
                includeSelectAllOption: true
            });
            $.unblockUI({});
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
