<%@ Page Title="Andon" Language="C#" AutoEventWireup="true" CodeBehind="MachineAndon.aspx.cs" Inherits="Web_TPMTrakDashboard.Vulkan.MachineAndon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>
    <script src="../GEA/Andon_GEA/Scripts/bootstrap.js"></script>
    <link href="../GEA/Andon_GEA/Content/bootstrap.css" rel="stylesheet" />
    <style>
        .HeaderImage {
            flex: 1;
            float: left;
        }

        .border {
            border-radius: 25px;
            border: 0.5px solid #cccccc;
            background-color: white;
        }

        .myItem {
            border-radius: 15px;
        }

        .tblMachineData {
            margin-top: 10px;
            margin-bottom: 10px;
        }

            .tblMachineData tr td {
                border: 1px solid #cebdbd;
                background-color: #fbfbfb;
                font-size: 28px;
                padding: 5px;
                font-weight: 500;
            }

        .tblMachineHeader tr td {
            font-size: 30px;
        }

            .tblMachineHeader tr td span {
                border: unset;
                background-color: #0072c6;
                color: white;
                font-weight: bold;
                padding: 7px 15px;
                border-radius: 8px;
            }

        #machineContainer .Green {
            color: green;
        }

        #machineContainer .Red {
            color: Red;
        }

        #machineContainer .Yellow {
            color: yellow;
        }

        #machineContainer .Black {
            color: black;
        }

        #machineContainer .backWhite {
            background: #fbfbfb;
        }

        #machineContainer .backYellow {
            background: yellow;
        }
    </style>
</head>
<body style="background-color: #202648">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div>
                        <div class="navbar navbar-default navbar-fixed-top text-center" style="background-color: #1a2732; border-color: white; border-style: double; border-width: thick">
                            <div class="HeaderImage">
                                <img src="../Images/logo/AMITLogo.png" height="60" style="padding: 3px" class="img-responsive img-rounded" />
                            </div>

                            <span id="headerName" style="color: white; font-weight: bold; font-size: 30px; text-align: right; margin: auto; line-height: 60px;">Andon</span>

                            <div class="top-nav" style="width: auto; margin-top: 13px; margin-right: 3px; float: right">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <table style="font-size: 24px">
                                            <tr>
                                                <td style="color: white">Last Refresh Time:&nbsp;</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblRefreshTime" ClientIDMode="Static" ForeColor="White"> </asp:Label>

                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="machineInteral" EventName="Tick" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12" style="margin-top: 70px;">
                    <asp:HiddenField runat="server" ID="hdnBoxWidth" ClientIDMode="Static" />
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Timer runat="server" ID="machineInteral" ClientIDMode="Static" OnTick="machineInteral_Tick"></asp:Timer>
                            <div id="machineContainer" style="height: 88vh; overflow: hidden">
                                <asp:ListView runat="server" ID="lvMachineData" ClientIDMode="Static">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder runat="server" ID="itemplaceholder" />
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <div class="myItem" style="margin: 15px; margin-bottom: 0px; min-width: 100px; display: inline-block; background-color: white">
                                            <div class="" style="padding: 10px;">
                                                <table style='width: 100%;' class="tblMachineHeader">
                                                    <tr>
                                                        <td style="text-align: center; color: black; font-weight: bold;">
                                                            <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("Machineid") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style='width: 100%;' class="tblMachineData">
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ClientIDMode="Static">Part</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("Part") %>' Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ClientIDMode="Static">Batch No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("BatchNo") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ClientIDMode="Static">Expected Prod. Qty.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("ExpectedProd") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background: #098e09; color: white">
                                                            <asp:Label runat="server" ClientIDMode="Static">Actual Prod. Qty.</asp:Label>
                                                        </td>
                                                        <td style="background: #098e09; color: white">
                                                            <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("ActualProd") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ClientIDMode="Static">OEE %</asp:Label>
                                                        </td>
                                                        <td class='<%# Eval("OEEBackColor") %>'>
                                                            <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("OEE") %>' CssClass='<%# Eval("OEEFontColor") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background: red; color: white">
                                                            <asp:Label runat="server" ClientIDMode="Static">Down Time (min.)</asp:Label>
                                                        </td>
                                                        <td style="background: red; color: white">
                                                            <asp:Label runat="server" ClientIDMode="Static" Text='<%# Eval("DownTime") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="machineInteral" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <script type="text/javascript"> 
            $(document).ready(function () {

            });
            function setSameWidthToAllBox() {
                var divWidth;
                if ($('#hdnBoxWidth').val() == "" || $('#hdnBoxWidth').val() == undefined) {
                    var wid = [];
                    var divs = $('#machineContainer .myItem');
                    for (var i = 0; i < divs.length; i++) {
                        wid.push(divs[i].offsetWidth);
                    }
                    divWidth = Math.max(...wid);
                    // divWidth = Math.max.apply(null, $('.myItem').map(function () {
                    //    return $(this).outerHeight(true);
                    //}).get());
                }
                else {
                    divWidth = $('#hdnBoxWidth').val();
                }
                $('.myItem').css("width", divWidth);
                console.log(".myItems width from setSameWidthToAllBox=" + divWidth);

            }
            function setBoxBasedOnHeight() {
                debugger;
                $('#hdnBoxWidth').val("");
                setSameWidthToAllBox();
                var machineContainerHeight = $('#machineContainer').height();
                var machineContainerWidth = $('#machineContainer').width();
                var divHeight = $('.myItem').outerHeight(true);
                var divWidth = $('.myItem').width() + 40; //margin=15 (15+15)
                var divs = $('#machineContainer .myItem');
                let divsCount = divs.length;
                var fittedDivsRows = Math.floor(machineContainerHeight / divHeight);
                var fittedDivsCol = Math.floor(machineContainerWidth / divWidth);
                let fittedDiv = fittedDivsRows * fittedDivsCol;
                for (var i = divsCount - 1; i >= fittedDiv; i--) {
                    divs[i].remove();
                }
                console.log(".myItems width from setBoxBasedOnHeight=" + divWidth);
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '<%=ResolveUrl("MachineAndon.aspx/setNoOfDivsToSession")%>',
                    contentType: "application/json; charset=utf-8",
                    crossDomain: true,
                    dataType: "json",
                    data: '{divs:' + fittedDiv + ',divsLength:' + divsCount + '}',
                    success: function (response) {
                    },
                    error: function (Result) {
                        // alert("Error 2");
                    }
                });
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                $(document).ready(function () {
                    setSameWidthToAllBox();
                });

            });
        </script>
    </form>
</body>
</html>
