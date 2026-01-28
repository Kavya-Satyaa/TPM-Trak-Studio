<%@ Page Title="Map Rule to Consumers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MapRulesToConsumers.aspx.cs" Inherits="Web_TPMTrakDashboard.AlertModule.MapRulesToConsumers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>
    <style>
        #gvContainer {
            overflow: auto;
            margin-top: 10px;
        }

            #gvContainer table {
                width: 100%;
            }

                #gvContainer table tr td {
                    text-align: left;
                    padding: 8px;
                    border: 1px solid #ddd;
                    color: white;
                    font-size: 15px;
                }

                #gvContainer table tr th {
                    font-weight: bold;
                    font-size: 15px;
                    text-align: center;
                    position: sticky;
                    top: -1px;
                    z-index: 1;
                    background-color: #2e6886;
                    color: white;
                    padding: 8px;
                    border: 1px solid #ddd;
                }

        /*#gvContainer table tr:last-child {
                    display: none;
                }*/
        /*#gvContainer table tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }*/

        input[type="checkbox"] {
            width: 20px;
            height: 20px;
        }

        input[type="radio"] {
            width: 25px;
            height: 25px;
        }

        .lblcss {
            color: white;
            font-size: 16px;
        }

        .emptydatacss {
            color: white;
        }

        .btn {
            font-size: 16px;
        }

        fieldset {
            border: 1px solid #4f4e63;
            padding: 0px;
            border-radius: 4px;
            width: 400px;
            /*box-shadow: 2px 2px 8px 2px #efe7e7;*/
        }

        .masterFS {
            margin: 0 8px 0 8px;
            padding: 0 10px 5px 10px;
        }

        legend {
            text-align: left;
            color: white;
            display: block;
            width: auto;
            padding: 0;
            margin-bottom: 0px;
            font-size: 21px;
            line-height: inherit;
            border-bottom: transparent;
            color: white;
        }
    </style>
    <div class="row">
        <div class="col-lg-12" style="text-align: left; display: block; margin-bottom: 10px;">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hdnRecentDdlRule" />
                    <asp:HiddenField runat="server" ID="hdnFirstTimeMachineView" />
                    <asp:HiddenField runat="server" ID="hdnFirstTimeRuleView" />
                    <asp:HiddenField runat="server" ID="hdnFirstTimeConsumerView" />
                    <asp:HiddenField runat="server" ID="hdnRecentDdlPlant_Rule" />
                    <asp:HiddenField runat="server" ID="hdnRecentDdlPlant_Machine" />
                    <asp:HiddenField runat="server" ID="hdnRecentPlantName" />
                    <asp:HiddenField runat="server" ID="hdnRecentDdlMachine" />
                    <asp:HiddenField runat="server" ID="hdnRecentDdlConsumer" />
                    <asp:HiddenField runat="server" ID="hdnRecentDdlPlant_Consumer" />
                    <div class="ui segment">
                        <table>
                            <tr>
                                <td style="width:60px">
                                    <label class="lblcss" style="color:black">Plant :</label>
                                </td>
                                <td  style="width:220px">
                                    <asp:DropDownList Width="200" ID="ddlPlant" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td style="width:80px">
                                <label runat="server" id="lblMRC" class="lblcss" style="color:black">Machine ID</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMRC" runat="server" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td>&nbsp;&nbsp;   
                                <asp:Button runat="server" ID="btnView" ClientIDMode="Static" CssClass="ui violet button" Text="View" OnClick="btnView_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="ui segment">
                    <fieldset class="masterFS" style="color:black">
                        <legend style="color:black">View</legend>
                        <table>
                            <tr>

                                <td>
                                    <label class="lblcss" style="vertical-align: bottom;color:black;">Machine</label>
                                    <%-- <asp:CheckBox runat="server" ID="chkMachine" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chkMachine_CheckedChanged" />--%>
                                    <asp:RadioButton runat="server" ID="chkMachine" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chkMachine_CheckedChanged" />

                                </td>
                                <td>&nbsp;&nbsp; &nbsp; &nbsp; 
                                <label class="lblcss" style="vertical-align: bottom;color:black;">Rule</label>
                                    <%-- <asp:CheckBox runat="server" ID="chkRule" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chkRule_CheckedChanged" />--%>
                                    <asp:RadioButton runat="server" ID="chkRule" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chkRule_CheckedChanged" />

                                </td>
                                <td>&nbsp;&nbsp; &nbsp; &nbsp; 
                                <label class="lblcss" style="vertical-align: bottom;color:black;">Consumer</label>
                                    <%-- <asp:CheckBox runat="server" ID="chkConsumer" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chkConsumer_CheckedChanged" />--%>
                                    <asp:RadioButton runat="server" ID="chkConsumer" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chkConsumer_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div id="gvContainer">
                        <asp:GridView runat="server" ID="gvMachine" ClientIDMode="Static" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnRowDataBound="gvMachine_RowDataBound" EmptyDataText="No Data Found" EmptyDataRowStyle-CssClass="emptydatacss" ShowFooter="false" CssClass="CentreHeader ">
                        </asp:GridView>
                        <asp:GridView runat="server" ID="gvRule" ClientIDMode="Static" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnRowDataBound="gvRule_RowDataBound" EmptyDataText="No Data Found" ShowFooter="false" CssClass="CentreHeader ">
                        </asp:GridView>
                        <asp:GridView runat="server" ID="gvConsumer" ClientIDMode="Static" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnRowDataBound="gvConsumer_RowDataBound" EmptyDataText="No Data Found" ShowFooter="false" CssClass="CentreHeader ">
                        </asp:GridView>
                    </div>
                    <div style="margin-top: 10px">
                        <asp:Button runat="server" ID="btnSave" ClientIDMode="Static" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-info" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="chkMachine" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkRule" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkConsumer" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            setGridHeight();
        });
        function setGridHeight() {
            var wHeight = $(window).height() - 340;
            $('#gvContainer').css('height', wHeight);
        }
        $(window).resize(function () {
          setGridHeight();
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setGridHeight();
            });
            $(window).resize(function () {
              setGridHeight();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
