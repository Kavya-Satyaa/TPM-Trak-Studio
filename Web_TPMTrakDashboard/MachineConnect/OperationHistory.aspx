<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperationHistory.aspx.cs" Inherits="Web_TPMTrakDashboard.MachineConnect.OperationHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <style>
        .data-conatiners {
            padding: 5px;
        }

        #tvOperationHistoryDetails a {
            color: white;
        }


        .tbl-style {
            width: 100%;
        }

            .tbl-style > tbody > tr > td {
                padding: 5px;
                background-color: #0066CC;
                border: 1px solid white;
            }

        .header-div {
            background-color: #202648 !important;
            color: white !important;
            font-size: 16px;
            height: 34px;
            min-height: 34px;
            text-align: center;
        }

        .content-div {
            height: 76vh;
            vertical-align: top;
            color: white;
            overflow: auto;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table id="tblfilter" class="table table-bordered" style="display: inline-block; margin-bottom: 0px;">
                <tr>
                    <td class="commontd"><b>From Date</b> </td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="form-control date" placeholder="From Date Time" MaxLength="15" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" AutoCompleteType="Disabled"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commontd"><b>To Date</b></td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-calendar"></i>
                            </div>
                            <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control date" placeholder="From Date Time" MaxLength="15" Style="min-width: 130px; max-width: 180px; margin: 0 10px 0 0;" AutoCompleteType="Disabled"></asp:TextBox>
                        </div>
                    </td>
                    <td class="commontd"><b>Plant</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="commontd"><b>Machine</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnView" ClientIDMode="Static" CssClass="btn btn-info btn-sm" Text="View" OnClick="btnView_Click" />

                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnReset" ClientIDMode="Static" CssClass="btn btn-info btn-sm" Text="Reset" OnClick="btnReset_Click" />
                    </td>
                </tr>
            </table>
            <div>
                <div class="col-md-3 col-lg-3 col-sm-3 data-conatiners " id="oprDateDiv">
                    <table class="tbl-style">
                        <tr>
                            <td class="header-div">
                                <span runat="server" id="Span1" style="font-weight: bold;">Operator History File</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="content-div">
                                    <asp:TreeView ID="tvOperationHistoryDetails" runat="server" ImageSet="XPFileExplorer" NodeIndent="15" CssClass="tree-grid" OnSelectedNodeChanged="tvOperationHistoryDetails_SelectedNodeChanged" OnTreeNodeCheckChanged="tvOperationHistoryDetails_TreeNodeCheckChanged" LeafNodeStyle-CssClass="leaf-node" RootNodeStyle-CssClass="root-node" NodeStyle-CssClass="middle-node" ClientIDMode="Static">
                                        <NodeStyle HorizontalPadding="2px"
                                            NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                                        <SelectedNodeStyle CssClass="selected-node" />
                                    </asp:TreeView>
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>
                <div id="filecontainer" class="col-md-9 col-lg-9 col-sm-9  data-conatiners datalist-container">
                    <table class="tbl-style">
                        <tr>
                            <td class="header-div">
                                <span runat="server" id="opnFileHeader" style="font-weight: bold;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="content-div">
                                    <span id="opnFileData" runat="server" clientidmode="static"></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            setControls();
        });
        $("#tvOperationHistoryDetails a").click(function () {
            if ($(this).hasClass("leaf-node")) {
                //callLoader();
            }
        });
        function setControls() {
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setControls();
            });
            $("#tvOperationHistoryDetails a").click(function () {
                if ($(this).hasClass("leaf-node")) {
                    //callLoader();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
