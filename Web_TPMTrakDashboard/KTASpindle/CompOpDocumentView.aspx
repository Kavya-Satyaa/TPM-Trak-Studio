<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompOpDocumentView.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.CompOpDocumentView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Document View</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="../AndonContent/bootstrap.min.css" rel="stylesheet" />
    <style>
        .innergrid tbody tr:nth-child(odd) {
            background-color: #cddadf;
        }

        a {
            color: #2e2eff !important;
            text-decoration: none;
        }

        .headerFixer {
            box-shadow: 4px 6px 5px 0px #88888840;
            font-weight: 600;
        }

        .table > tbody > tr > td {
            padding: 4px;
            vertical-align: top;
            /*  border-top: 1px solid white;*/
        }

        .headerFixer > tbody > tr > td {
            padding: 4px;
            vertical-align: top;
            /* border-top: 1px solid black;*/
        }

        .headerFixer tr th {
            padding: 4px 6px;
            font-size: 14px;
            background-color: #2a5483;
            border: none;
            color: white;
            position: sticky;
            top: 0px;
            z-index: 20;
        }

        .headerFixer tr td {
            padding: 4px 6px;
            font-size: 14px;
            border: none;
            border-bottom: 1px solid #d6d7df;
        }

        #menu {
            position: fixed;
            top: 0;
            background: #1a2732;
            box-shadow: 0 0 10px black;
        }

        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }

        .HeaderImage {
            float: right;
            flex: 1;
            margin-top: 7px;
        }
        /*   .headerstyle>tbody>tr>td
        {
           border: 1px solid #ddd0;
        }*/
        .headerstyle {
            width: 100%;
            font-size: 16px;
            font-weight: bold;
            background-color: #cddadf;
        }

        .td-style {
            background-color: #00b8ff2b;
            vertical-align: middle !important;
            text-align: center !important;
        }

        .table {
            width: 100%;
            max-width: 100%;
            margin-bottom: 1px !important;
        }

        .maintable {
            border: 2px solid black;
        }

        .main_tr {
            border: 2px solid black;
        }
    </style>
    <script src="../AndonScripts/jquery-3.1.0.min.js"></script>
    <script src="../AndonScripts/bootstrap.min.js"></script>
    <link href="../AndonContent/bootstrap-glyphicons.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $(".innergrid").on("click", "td .DrawingControl", function () {
                $("#hdnUpdate").val("drawing");
            });
            $(".innergrid").on("click", "td .ProgramControl", function () {
                $("#hdnUpdate").val("program");
            });
            $(".innergrid").on("click", "td .ToolsControl", function () {
                $("#hdnUpdate").val("tools");
            });
            $(".innergrid").on("click", "td .FixtureControl", function () {
                $("#hdnUpdate").val("fixture");
            });
            $(".innergrid").on("click", "td .GaugeControl", function () {
                $("#hdnUpdate").val("gauge");
            });
            $(".innergrid").on("click", "td .InspectionControl", function () {
                $("#hdnUpdate").val("inspection");
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <%--   <asp:UpdatePanel runat="server">
            <ContentTemplate>--%>
        <div>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-12">
                        <div>
                            <div class="navbar navbar-fixed-top text-center" style="background-color: #3777bc;" id="menu">
                                <span id="headerName" style="color: white; font-weight: bold; font-size: 30px; margin: auto; line-height: 60px;">Document View</span>
                                <div class="HeaderImage">
                                    <asp:LinkButton ClientIDMode="Static" ID="lnkBackBtn" Visible="false" runat="server" CssClass="btn-style1" OnClick="lnkBackBtn_Click">
                                        <asp:Image ID="BackImg" runat="server" CssClass="Btnicon" ImageUrl="~/Image/Backbutton_img.png" Style="width: 54px; height: 43px" ToolTip="Back" />
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-12" style="margin-top: 9vh;">
                        <table class="table table-bordered headerstyle">
                            <tr>
                                <td style="width: 113px;">Component : </td>
                                <td style="width: 540px;">
                                    <asp:Label runat="server" ID="lblComp" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td style="width: 97px;">Operation : </td>
                                <td style="min-width: 67px;">
                                    <asp:Label runat="server" ID="lblOp" ClientIDMode="Static"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="documentName" ClientIDMode="Static" Style="color: #2e2eff;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-lg-4" style="overflow: auto; height: 84vh; margin-top: 15px;">
                        <asp:HiddenField runat="server" ID="hdnUpdate" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnComp" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnOpDesc" ClientIDMode="Static" />
                        <asp:ListView ItemPlaceholderID="itemplaceholder" ID="lvDocumentUpload" runat="server" ClientIDMode="Static">
                            <LayoutTemplate>
                                <table class="table table-bordered table-hover headerFixer bajaj-table-style maintable" id="tblDocumentUpload">
                                    <%-- <tr>
                                                <th>Component</th>
                                                <th>Operation</th>
                                                <th>Drawing</th>
                                                <th style="display:none;">Program</th>
                                                <th>Tools</th>
                                                <th>Fixture</th>
                                                <th>Gauge</th>
                                                <th>Inspection</th>
                                            </tr>--%>
                                    <tr runat="server" id="itemplaceholder"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <%--  <tr>
                                            <td>Component</td>
                                            <td>
                                                <asp:Label runat="server" ID="lblComponent" ClientIDMode="Static" Text='<%# Eval("ComponentID") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Operation</td>
                                            <td>
                                               <asp:Label runat="server" ID="lblOpnID" ClientIDMode="Static" Text='<%# Eval("operationno") %>'></asp:Label>
                                            </td>
                                         </tr>--%>
                                <tr class="main_tr">
                                    <td class="td-style">Drawing</td>
                                    <td style="min-width: 250px;">
                                        <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvDrawingDocumentDisplay" runat="server" DataSource='<%#Eval("DrawingData")%>'>
                                            <LayoutTemplate>
                                                <table runat="server" class="table table-bordered innergrid" id="tblDrawinglst">
                                                    <tr id="inneritemplaceholder" runat="server"></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton CssClass="DrawingControl" runat="server" ID="lnDrawingFileName" ClientIDMode="Static" Text='<%# Eval("DrawingDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr style="visibility: collapse" class="main_tr">
                                    <td class="td-style">Program</td>
                                    <td style="display: none; min-width: 250px">
                                        <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvProgramDocumentDisplay" runat="server" DataSource='<%#Eval("ProgramData")%>'>
                                            <LayoutTemplate>
                                                <table runat="server" class="table table-bordered innergrid">
                                                    <tr id="inneritemplaceholder" runat="server"></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnkProgram" ClientIDMode="Static" CssClass="ProgramControl" Text='<%# Eval("ProgramDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr class="main_tr">
                                    <td class="td-style">Tools</td>
                                    <td style="min-width: 250px;">
                                        <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvTools" runat="server" DataSource='<%#Eval("ToolsData")%>'>
                                            <LayoutTemplate>
                                                <table runat="server" class="table table-bordered innergrid">
                                                    <tr id="inneritemplaceholder" runat="server"></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnkTools" ClientIDMode="Static" CssClass="ToolsControl" Text='<%# Eval("ToolsDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr class="main_tr">
                                    <td class="td-style">Fixture</td>
                                    <td style="min-width: 250px;">
                                        <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvFixture" runat="server" DataSource='<%#Eval("FixtureData")%>'>
                                            <LayoutTemplate>
                                                <table runat="server" class="table table-bordered innergrid">
                                                    <tr id="inneritemplaceholder" runat="server"></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnkFixture" ClientIDMode="Static" CssClass="FixtureControl" Text='<%# Eval("FixtureDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr class="main_tr">
                                    <td class="td-style">Gauge</td>
                                    <td style="min-width: 250px;">
                                        <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvGauge" runat="server" DataSource='<%#Eval("GaugeData")%>'>
                                            <LayoutTemplate>
                                                <table runat="server" class="table table-bordered innergrid">
                                                    <tr id="inneritemplaceholder" runat="server"></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnkGauge" ClientIDMode="Static" CssClass="GaugeControl" Text='<%# Eval("GaugeDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr class="main_tr">
                                    <td class="td-style">Inspection</td>
                                    <td style="min-width: 250px;">
                                        <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvInspection" runat="server" DataSource='<%#Eval("InspectionData")%>'>
                                            <LayoutTemplate>
                                                <table runat="server" class="table table-bordered innergrid">
                                                    <tr id="inneritemplaceholder" runat="server"></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnkInspection" CssClass="InspectionControl" ClientIDMode="Static" Text='<%# Eval("InspectionDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr class="main_tr">
                                    <td class="td-style">Program</td>
                                    <td style="min-width: 250px; padding: 0px !important;">
                                        <table>
                                            <tr>
                                                <td class="td-style">Proven Machine Program</td>
                                                <td style="min-width: 250px;">
                                                    <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvProvenProgram" runat="server" DataSource='<%#Eval("ProvenProgramData")%>'>
                                                        <LayoutTemplate>
                                                            <table runat="server" class="table table-bordered innergrid">
                                                                <tr id="inneritemplaceholder" runat="server"></tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton runat="server" ID="lnkInspection" CssClass="ProvenProgramControl" ClientIDMode="Static" Text='<%# Eval("ProvenProgramDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td-style">Standard Software Program</td>
                                                <td>
                                                    <asp:ListView ItemPlaceholderID="inneritemplaceholder" ID="lvStandardProgram" runat="server" DataSource='<%#Eval("StandardProgramData")%>'>
                                                        <LayoutTemplate>
                                                            <table runat="server" class="table table-bordered innergrid">
                                                                <tr id="inneritemplaceholder" runat="server"></tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton runat="server" ID="lnkStandardProgram" CssClass="StandardProgramControl" ClientIDMode="Static" Text='<%# Eval("StandardProgramDocFileName") %>' OnClick="lnFileName_Click"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>

                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>

                    <div class="col-lg-8" style="margin-top: 15px;">
                        <div style="height: 84vh">
                            <iframe id="iframeDocument" style="width: 100%; height: 100%" runat="server"></iframe>
                            <iframe id="iframeText" style="width: 100%; height: 100%" runat="server"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>
    <script>

        $(".innergrid").on("click", "td .DrawingControl", function () {
            $("#hdnUpdate").val("drawing");
        });
        $(".innergrid").on("click", "td .ProgramControl", function () {
            $("#hdnUpdate").val("program");
        });
        $(".innergrid").on("click", "td .ToolsControl", function () {
            $("#hdnUpdate").val("tools");
        });
        $(".innergrid").on("click", "td .FixtureControl", function () {
            $("#hdnUpdate").val("fixture");
        });
        $(".innergrid").on("click", "td .GaugeControl", function () {
            $("#hdnUpdate").val("gauge");
        });
        $(".innergrid").on("click", "td .InspectionControl", function () {
            $("#hdnUpdate").val("inspection");
        });
        $(".innergrid").on("click", "td .ProvenProgramControl", function () {
            $("#hdnUpdate").val("ProvenProgram");
        });
        $(".innergrid").on("click", "td .StandardProgramControl", function () {
            $("#hdnUpdate").val("StandardProgram");
        });


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(".innergrid").on("click", "td .DrawingControl", function () {
                $("#hdnUpdate").val("drawing");
            });
            $(".innergrid").on("click", "td .ProgramControl", function () {
                $("#hdnUpdate").val("program");
            });
            $(".innergrid").on("click", "td .ToolsControl", function () {
                $("#hdnUpdate").val("tools");
            });
            $(".innergrid").on("click", "td .FixtureControl", function () {
                $("#hdnUpdate").val("fixture");
            });
            $(".innergrid").on("click", "td .GaugeControl", function () {
                $("#hdnUpdate").val("gauge");
            });
            $(".innergrid").on("click", "td .InspectionControl", function () {
                $("#hdnUpdate").val("inspection");
            });
            $(".innergrid").on("click", "td .ProvenProgramControl", function () {
                $("#hdnUpdate").val("ProvenProgram");
            });
            $(".innergrid").on("click", "td .StandardProgramControl", function () {
                $("#hdnUpdate").val("StandardProgram");
            });

        });

    </script>
</body>
</html>
