<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionTransaction.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.InspectionTransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../MyCssAndJS/DatePicker2/moment.js"></script>
    <script src="../MyCssAndJS/DatePicker2/bootstrap-datetimepicker.min.js"></script>
    <link href="../MyCssAndJS/DatePicker2/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="Scripts/SearcableDropDown/select2.min.css" rel="stylesheet" />
    <script src="Scripts/SearcableDropDown/select2.min.js"></script>
    <style>
        .select2-container {
            width: 360px !important;
            margin-left: 10px;
        }

        .select2-container--open .select2-dropdown {
            left: -9px;
        }

        .select2-container .select2-selection--single {
            box-sizing: border-box;
            cursor: pointer;
            display: block;
            height: 34px;
            user-select: none;
            -webkit-user-select: none;
        }

        .table-style td {
            border-top: unset !important;
        }

        .footertable {
            background-color: #2e6886;
            margin-bottom: unset !important;
        }

            .footertable tr td {
                font-weight: bold;
                color: white;
                padding: 4px !important;
                margin-bottom: unset !important;
                margin-top: 5px;
            }

        .headerFixer tbody tr {
            background-color: #FFFFFF;
            color: black;
        }

            .headerFixer tbody tr:nth-last-child(2) {
                background-color: #DCDCDC;
                color: black;
            }

        .part {
            display: none;
            color: black;
        }

        .inner-tbl {
            background-color: transparent !important;
            color: black !important;
            margin: 0;
        }

            .inner-tbl tr th {
                border-top: unset !important;
            }

            .inner-tbl tr td {
                border-top: unset !important;
            }

        .btn-style {
            color: #fff !important;
            background-color: #4bd7ff4f;
            border-color: #31a7c9;
            font-weight: bold;
            height: 37px;
        }

        .lblStyle {
            color: white;
            font-weight: bold;
            font-size: 16px;
        }

        .lblStyle1 {
            color: white;
            font-weight: bold;
            font-size: 14px;
        }

        #tblFilter tr td {
            padding: 5px;
        }

        #tblInspection tr td {
            background-color: white;
        }

        .headertable tr th, .headertable > tbody > tr:first-child td {
                position: sticky;
                top: -1px;
                z-index: 25;
                font-weight: bold;
                text-align: center;
            }
    </style>
    <div class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="col-lg-11">
                    <table id="tblFilter" class="table table-bordered" style="width: auto; margin-left: 10px;">
                        <tr>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Plant</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlant" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Cell</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCell" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Machine</td>
                            <td style="width: 200px;">
                                <asp:DropDownList runat="server" ID="ddlMachine" ClientIDMode="Static" CssClass="form-control" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged" AutoPostBack="true" Width="255px">
                                </asp:DropDownList>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Component</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlComponent" CssClass="form-control searchable-dropdown-list" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>

                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Operation</td>
                            <td style="width: 200px;">
                                <asp:DropDownList runat="server" ID="ddlOperation" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Day</td>
                            <td class="input-group" style="min-width: 150px; border: 0">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Style="width: 150px;" CssClass="form-control date" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;">Shift</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" />
                                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlPlant" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlCell" EventName="SelectedIndexChanged" />
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="col-lg-1" style="text-align: end; margin-top: 35px;">
            <asp:Label Text="Drawing" ID="lblDrawing" runat="server" class="lblStyle"></asp:Label>
            <asp:LinkButton runat="server" ID="btnPDf" ToolTip="Drawing" OnClick="btnPDf_Click">
                <asp:Image runat="server" ImageUrl='~/Images/PDF_Icon.png' Height="40" Width="45"/>
            </asp:LinkButton>
        </div>
        <div style="height: 69vh; overflow: auto;" id="gridContainer" class="col-lg-12">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:ListView runat="server" ID="lvInspectiondetails" ClientIDMode="Static" OnItemDataBound="lvInspectiondetails_ItemDataBound">
                        <LayoutTemplate>
                            <table class="table table-bordered inner-tbl headertable" id="tblInspection">
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <%-- <th class="part" style="display: <%# Eval("HeaderVisibility") %>">Sl No.
                                </th>--%>

                                <th class="part insp-th" style="min-width: 226px; background-color: #2e6886;color:white; display: <%# Eval("HeaderVisibility") %>">Inspection Characteristics
                                </th>
                                <th class="part setvalue-th" style="min-width: 150px; background-color: #2e6886;color:white; display: <%# Eval("HeaderVisibility") %>">Set Value
                                </th>
                                <th class="part lsl-th" style="width: 100px; background-color: #2e6886;color:white; display: <%# Eval("HeaderVisibility") %>">LSL
                                </th>
                                <th class="part usl-th" style="width: 100px; background-color: #2e6886;color:white; display: <%# Eval("HeaderVisibility") %>">USL
                                </th>
                                <%--<td class="" style="display: <%# Eval("ContentVisibility") %>"><%# Eval("SlNo") %></td>--%>
                                <td class="part insp-td" style="display: <%# Eval("ContentVisibility") %>"><%# Eval("InspectionChar") %></td>
                                <td class="part setvalue-td" style="display: <%# Eval("ContentVisibility") %>"><%# Eval("SetValue") %></td>
                                <td class="part lsl-td" style="display: <%# Eval("ContentVisibility") %>"><%# Eval("LSL") %></td>
                                <td class="part usl-td" style="display: <%# Eval("ContentVisibility") %>"><%# Eval("USL") %></td>
                                <asp:HiddenField runat="server" ID="hfCharCode" Value=' <%# Eval("InspectionChar") %>' />
                                <asp:HiddenField runat="server" ID="hfShift" Value=' <%# Eval("Shift") %>' />

                                <td style="padding: 0; border-top: unset; border-right: unset; border-left: unset">
                                    <asp:ListView runat="server" ID="lvJobDetails" DataSource='<%# Eval("HeaderList") %>'>
                                        <LayoutTemplate>
                                            <table class="table inner-tbl">
                                                <tr>
                                                    <td id="itemplaceholder" runat="server"></td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <td style="padding: 0; border: unset; vertical-align: inherit; border-right: 1px solid white; width: 200px; background-color: <%# Eval("HeaderColor") %>">
                                                <table class="inner-tbl  value-header-tbl part" style="display: <%# Eval("JobHeaderVisibility") %>">
                                                    <tr>
                                                        <th style="background-color: #2e6886;color:white;">
                                                            <%# Eval("HeaderName") %>
                                                        </th>
                                                    </tr>
                                                </table>
                                                <table class="inner-tbl value-content-tbl part" style="display: <%# Eval("JobContentVisibility") %>">
                                                    <tr>
                                                        <td>

                                                            <asp:HiddenField runat="server" ID="hfJobHeader" Value='<%# Eval("HeaderName") %>' />
                                                            <asp:HiddenField runat="server" ID="hfDataType" Value='<%# Eval("Datatype") %>' />
                                                            <asp:TextBox runat="server" ID="txtJobVal" CssClass="form-control entryVal" AutoCompleteType="Disabled" Text='<%# Eval("JobValue") %>' Visible='<%# Eval("TextVisibility") %>' Style="width: 181px;"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtnumJobVal" CssClass="form-control allowDecimal entryVal" AutoCompleteType="Disabled" Text='<%# Eval("JobValue") %>' Visible='<%# Eval("NumericVisibility") %>' Style="width: 181px;"></asp:TextBox>
                                                            <asp:DropDownList runat="server" ID="ddlJob" ClientIDMode="Static" CssClass="form-control entryVal" Style="width: 181px;" Visible='<%# Eval("DropdownVisibility") %>'>
                                                                <asp:ListItem Text="Ok" Value="Ok"></asp:ListItem>
                                                                <asp:ListItem Text="Not Ok" Value="Not Ok"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:HiddenField runat="server" ID="hfJobVal" ClientIDMode="Static" Value='<%# Eval("JobValue") %>' />
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="border: 1px solid white; margin-top: 10px;" id="maindiv" runat="server">
                    <table class="table table-bordered footertable" id="footerdiv" runat="server">
                        <tr>
                            <td id="linetd" runat="server">Line Inspector :
                                <asp:Label ID="lblLineInspectorID" runat="server" class="lblStyle1" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td id="drawingtd" runat="server">Drawing Checked By :
                                 <asp:Label ID="lblDrawingCheckedBy" runat="server" class="lblStyle1" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td id="shifttd" runat="server">Shift Supervisor :
                                <asp:Label ID="lblShiftSupervisor" runat="server" class="lblStyle1" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table class="table" style="width: 70%; margin-left: 10%; text-align: center; margin-bottom: unset !important" id="footermainDiv" runat="server">
                        <tr class="table-style">
                            <td>
                                <asp:Button runat="server" ID="btnLineInspection" Text="Line Inspector Approval" CssClass="btn btn-style" OnClick="btnApproval_Click" Style="margin-right: 20px" />
                                <asp:Button runat="server" ID="btnDrawing" Text="Drawing Checked By" CssClass="btn btn-style" OnClick="btnApproval_Click" Style="margin-right: 20px" />
                                <%--                                <asp:Button runat="server" ID="BtnQuality" Text="Quality" CssClass="btn btn-style" OnClick="btnView_Click" style="margin-right:20px"/>--%>
                                <asp:Button runat="server" ID="btnSupervisor" Text="Shift Supervisor Approval" CssClass="btn btn-style" OnClick="btnApproval_Click" Style="margin-right: 20px" />
                            </td>
                            <td>
                                <asp:Label Text="Produced Qty :" runat="server" class="lblStyle"></asp:Label>
                                <asp:Label ID="prodQty" runat="server" class="lblStyle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label Text="Rejection : " runat="server" class="lblStyle"></asp:Label>
                                <asp:Label ID="rejQty" runat="server" class="lblStyle"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            setControls();
            setLeftFreeze();
        });
        function setControls() {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US',
            });

            $('.inner-tbl tr td .entryVal').blur(function () {
                $(this).closest("tr").find("#hdnUpdate").val("update");
            });

            $(".searchable-dropdown-list").select2({
                allowClear: false,
                placeholder: "Component ID"
            });
        }

        function setLeftFreeze() {
            let tblRow = $('[id$=tblInspection] tr');
            if (tblRow.length > 0) {
                let rowth = tblRow[0].children;
                let width1 = parseInt($(rowth[0]).css('width').replace('px', ''));
                let width2 = parseInt($(rowth[1]).css('width').replace('px', ''));
                let width3 = parseInt($(rowth[2]).css('width').replace('px', ''));
                $(".headertable tr .insp-td").css({ "position": "sticky", "left": "0px", "z-index": "20", "background- color": "white" });
                $(".headertable tr:first-child .insp-th").css({ "position": "sticky", "left": "0px", "top": "0px", "z-index": "40" });

                $(".headertable tr .setvalue-td").css({ "position": "sticky", "left": width1 + "px", "z-index": "20", "background- color": "white" });
                $(".headertable tr:first-child .setvalue-th").css({ "position": "sticky", "left": width1 + "px", "z-index": "40",  });

                $(".headertable tr .lsl-td").css({ "position": "sticky", "left": width1 + width2 + "px", "z-index": "20", "background- color": "white" });
                $(".headertable tr:first-child .lsl-th").css({ "position": "sticky", "left": width1 + width2 + "px", "z-index": "40", });

                $(".headertable tr .usl-td").css({ "position": "sticky", "left": width1 + width2 + width3 + "px", "z-index": "20", "background- color": "white" });
                $(".headertable tr:first-child .usl-th").css({ "position": "sticky", "left": width1 + width2 + width3 + "px", "z-index": "40", });
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                setControls();
                setLeftFreeze();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
