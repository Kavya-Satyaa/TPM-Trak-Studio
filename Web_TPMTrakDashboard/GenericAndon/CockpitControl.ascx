<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CockpitControl.ascx.cs" Inherits="Web_TPMTrakDashboard.GenericAndon.CockpitControl" %>

<%--<script src="../GEA/Andon_GEA/Scripts/jquery-3.3.1.js"></script>--%>
<style>
    .Green {
        /*background-color: green;*/
        border-radius: 15px;
        border: 0.1px solid #cccccc;
    }

    .Red {
        /*background-color: red;*/
        border-radius: 15px;
        border: 0.1px solid #cccccc;
    }

    .Yellow {
        /*background-color: yellow;*/
        border-radius: 15px;
        border: 0.1px solid #cccccc;
    }

    .white {
        border-radius: 15px;
        border: 1px solid #cccccc;
    }

    .Running {
        -webkit-animation: cog-rotate 2s linear infinite;
        -moz-animation: cog-rotate 2s linear infinite;
        -o-animation: cog-rotate 2s linear infinite;
        animation: rotate 2s linear infinite;
        color: green;
    }

    .Stopped {
        color: red;
    }

    .PDT {
        color: blue;
    }

    /* .border {
        border-radius: 25px;
        border: 0.5px solid #cccccc;
        background-color: white;
    }*/
    .addBorder {
        border-radius: 15px;
        border: 0.5px solid #504e4e;
        background-color: white;
        box-shadow: 0px 0px 2px grey;
    }

    .removeBorder {
        border-radius: 5px;
        border: 0.5px solid #504e4e;
        background-color: white;
    }

    .cellLabelDiv {
        background-color: #0f4987;
        padding: 5px 2px 0px 2px;
    }

        .cellLabelDiv span {
            color: white;
            font-weight: bold;
            font-size: 25px;
            font-family: sans-serif;
        }

    .cockpit > tbody > tr > td {
        padding: 1px;
    }
</style>
<div>
    <asp:HiddenField runat="server" ID="hdnWidth" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnHeight" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdntotalWidth" ClientIDMode="Static" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Timer runat="server" ID="flipInterval" ClientIDMode="Static" OnTick="flipInterval_Tick"></asp:Timer>
            <div style="text-align: center" class="cellLabelDiv" runat="server" id="cellLabelDiv">
                <asp:Label runat="server" ID="lblCellName" CssClass="cellLabel"></asp:Label>
            </div>

            <div id="OuterDivContainer">
                <div id="cockpitContainer" class="OuterDivContainerStyle">
                    <asp:ListView runat="server" ItemPlaceholderID="placeHolderCustomer" ID="lvCockpit">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="placeHolderCustomer" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="myItem" style="margin: 2px 2px 7px 2px; min-width: 100px; display: inline-block; vertical-align: top">
                                <div class="<%# Eval("BorderClass") %>">
                                    <div class="<%# Eval("MachineOEE") %> <%# Eval("BorderClass") %>" style="padding: 5px; background-color: <%# Eval("BackColor") %>;">
                                        <table style='width: 100%;' class="outercockpit">
                                            <tr>
                                                <td style="text-align: center; color: black; font-weight: bold; padding-left: 20px; padding-bottom: 0px;" id="tdMachineID">
                                                    <%-- <asp:LinkButton ID="lnkMachine" runat="server" CommandArgument='<%# Eval("MachineId") %>'><%# Eval("MachineId")%></asp:LinkButton>--%>
                                                    <label id="lblMachineID" style="font-size: <%# Eval("MachineFontSize")%>px;" clientidmode="static"><%# Eval("MachineId") %></label>
                                                </td>
                                                <td style="background-color: transparent; width: 35px;">
                                                    <%-- <asp:Image ImageUrl='<%# Eval("StatusImage") %>' runat="server"
                                                    Visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?true:false %>' meta:resourcekey="ImageResource1" />--%>
                                                    <asp:Image ImageUrl='<%# Eval("StatusImage") %>' runat="server" meta:resourcekey="ImageResource1" />
                                                    <div class="loaders-container" runat="server" visible='<%# Eval("MachineStatus").ToString() == "NOT OK"?false:true %>'>
                                                        <div class="la-cog la-2x" style="float: right;">
                                                            <div class="<%# Eval("MachineStatus") %>"></div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <table class="table table-bordered cssNonAdmin cockpit" style='background-color: white; margin-bottom: 0px'>
                                            <asp:ListView ID="ListView1" runat="server" DataSource='<%# Eval("Values") %>' ItemPlaceholderID="addressPlaceHolder">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder runat="server" ID="addressPlaceHolder" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: <%# Eval("TextAlign") %>; min-width: 100px; color: <%# Eval("ForeColorTitle") %>; background-color: <%# Eval("BackColorTitle") %>; font-size: <%# Eval("FontSizeInnerData") %>px; width: 45%;">
                                                            <%# Eval("LabelText")%>                                           
                                                        </td>
                                                        <td style='text-align: <%# Eval("TextAlign") %>; white-space: nowrap; color: <%# Eval("ForeColor") %>; background-color: <%# Eval("BackColor") %>; font-size: <%# Eval("DataFontSize") %>px; width: 45%;' machinename='<%# Eval("MachineName") %>' class='<%# Eval("HyperLink") %>'>
                                                            <div class="ellipsistooltip">
                                                                <asp:Label runat="server" Text='<%# Eval("LabelValue1")%>'></asp:Label>
                                                                <br />
                                                                <asp:Label runat="server" Text='<%# Eval("LabelValue2")%>'></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </div>
                                    <div style='text-align: center; padding: 1px; display: <%# Eval("ShowSmileyBlock") %>;'>
                                        <img src="<%# Eval("SmileyImagePath") %>" style='height: <%# Eval("SmileySize") %>px; width: auto;' />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="flipInterval" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        var scalefactor = 1.0;
        //debugger;
        if (window.devicePixelRatio >= 1.25) {
            scalefactor = 1.0 / window.devicePixelRatio;
        }

        function setFlipInterval() {
            debugger;
            $("#hdnWidth").val("");
            $("#hdntotalWidth").val("");

            let footerHeight = $('#footerDiv').height();
            if (footerHeight == undefined) {
                footerHeight = 0;
            }
            let cellLabelDivHeight = $(".cellLabelDiv").height();
            if (cellLabelDivHeight == undefined) {
                cellLabelDivHeight = 0;
            }
            $('#cockpitContainer').css("height", $(window).height() - $('#headerDiv').height() - cellLabelDivHeight - footerHeight - 15);
            $("#OuterDivContainer").css("height", $("#cockpitContainer").height());

            setMachineFontSize();
            SetIconicBoxWidth();

            var divH = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerHeight(true);
            }).get());

            var divW = Math.max.apply(null, $('.myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            let screenH = $('#cockpitContainer').height();
            let screenW = $('#cockpitContainer').width() - 15;
            let totalH = Math.floor(screenH / (divH));
            let totalW = Math.floor(screenW / (divW));
            let totalBox = Math.floor(totalH * totalW);
            $("#cockpitContainer .myItem").hide();
            $("#cockpitContainer .myItem").slice(0, totalBox).show();
            $("#hdnWidth").val(divW);
            $("#hdntotalWidth").val(totalW);
            setCenterDiv();

            $.ajax({
                async: false,
                type: "POST",
                url: "AndonPage.aspx/setFlipIntervalToSession",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                dataType: "json",
                data: '{displayedItemCount:' + totalBox + '}',
                success: function (response) {
                },
                error: function (Result) {
                }
            });
        }

        function SetIconicBoxWidth() {
            debugger;
            setDivWidthHeight();
            if ($("#hdnWidth").val() == "") {
                var divw = Math.max.apply(null, $('.myItem').map(function () {
                    return $(this).outerWidth(true);
                }).get());
                $("#cockpitContainer .myItem").width(divw);
            }
            else {
                $("#cockpitContainer .myItem").width($("#hdnWidth").val());
            }
            if ($("#hdnHeight").val() == "" || $("#hdnHeight").val() == undefined) {
                var divH = Math.max.apply(null, $('.myItem .cockpit').map(function () {
                    return $(this).outerHeight(true);
                }).get());
                $("#hdnHeight").val(divH);
            }
            else {
                $("#cockpitContainer .myItem .cockpit").height($("#hdnHeight").val());
            }
            if ($("#hdntotalWidth").val() != "") {
                setCenterDiv();
            }
        }

        function setCenterDiv() {
            var divW = Math.max.apply(null, $('#cockpitContainer .myItem').map(function () {
                return $(this).outerWidth(true);
            }).get());
            var totalW = $("#hdntotalWidth").val();
            $("#cockpitContainer").width((Math.ceil(divW) + 5) * totalW);
            $("#OuterDivContainer").addClass("OuterDivContainerStyle");
        }

        function setMachineFontSize() {
            debugger
            var list = $('#cockpitContainer .myItem');
            var maxLength = 14;
            for (var i = 0; i < list.length; i++) {
                if (list[i].querySelector('#lblMachineID').innerHTML.length >= maxLength) {
                    var machineID = list[i].querySelector('#lblMachineID').innerHTML;
                    list[i].querySelector('#lblMachineID').innerHTML = machineID.slice(0, maxLength) + '..';
                }
            }
        }

    </script>
</div>
