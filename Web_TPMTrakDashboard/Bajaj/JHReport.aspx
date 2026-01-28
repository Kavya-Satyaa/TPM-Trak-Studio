<%@ Page Title="JH Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JHReport.aspx.cs" Inherits="Web_TPMTrakDashboard.Bajaj.JHReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <style>
        .selected-menu-style {
            /*  background-color: unset !important;
            border-bottom: 3px solid #ffa500;
            color: #ffa500 !important;
            font-weight: bold;*/
            background-color: #0e94cb !important;
            border: 2px solid #0e94cb !important;
            color: white !important;
            font-weight: bold;
        }

            .selected-menu-style a {
                color: #555;
                font-weight: bold;
            }

        .submenuData {
            padding: 0px 20px;
            color:  #555;
            background-color: white;
            border: 2px solid white;
        }

        .selected-Submenu {
            /*  border-bottom: 3px solid #ffa500;
            background-color: unset;
            color: #ffa500;
            font-weight: 800;*/
        }

        /*  .other-menu-style {
            border-bottom: unset;
        }*/

        .tbl-report-details .inner-tbl {
            width: 100%;
        }

        .table tbody > tr > th, .table tbody > tr > td {
            border: 1px solid #ddd;
        }

        .tbl-report-details .value-tbl tr td, .tbl-report-details .value-tbl tr th {
            /* min-width: 200px;
            width: 200px;
            max-width: 200px;*/
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .tbl-report-details .checkpoint-tbl .slno {
            min-width: 70px;
            max-width: 70px;
            width: 70px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .tbl-report-details .checkpoint-tbl .group {
            min-width: 200px;
            max-width: 200px;
            width: 200px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .tbl-report-details .checkpoint-tbl .checkpoint {
            min-width: 300px;
            max-width: 300px;
            width: 300px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .tbl-report-details .checkpoint-tbl .items {
            min-width: 150px;
            max-width: 150px;
            width: 150px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }


        /* .tbl-report-details > tbody > tr > td:first-child {
            position: sticky;
            left: 0px;
            background-color: white;
            z-index: 3;
        }*/
        .tbl-report-details tr td {
            background-color: white;
        }

        /*   .value-tbl tr td {
            border: 1px solid red;
        }*/

        .tbl-report-details > tbody > tr:first-child {
            position: sticky;
            top: 0px;
            background-color: white;
            z-index: 4;
        }

        /* .bajaj-flat-tbl tr td {
            padding: 10px;
            color: white;
        }*/
    </style>

    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>



                <div class="bajaj-outer-div-filter-section">
                    <div class="bajaj-inner-div-filter-section left-content-filter-section">
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>Plant</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>Cell</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>Machine</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control"></asp:DropDownList>
                                    <%--AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"--%>
                                </td>
                                <td>Year</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtYear" CssClass="form-control " ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>Month</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMonth" CssClass="form-control " ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" />
                                    <asp:Button runat="server" ID="btnExport" Text="Export" OnClick="btnExport_Click" CssClass="bajaj-btn-style" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>


                <div class="navbar-collapse collapse" style="margin-top: 10px;">
                    <ul id="masterul" class="nav navbar-nav nextPrevious submenus-style " style="margin-right: 20px">
                        <li><a runat="server" class="submenuData" id="A15" clientidmode="static" data-toggle="tab" href="#DnWReportMenu">Daily Weekly Details</a>
                            <i></i>
                        </li>
                        <li><a runat="server" class="submenuData" id="A14" clientidmode="static" data-toggle="tab" href="#QReportMenu">Quaterly Details</a>
                            <i></i>
                        </li>
                    </ul>
                    <div style="margin-top: 6px">
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>Manager</td>
                                <td>
                                    <asp:Label runat="server" ID="lblManager"></asp:Label></td>
                                <td>Group Leader</td>
                                <td>
                                    <asp:Label runat="server" ID="lblGrpLeader"></asp:Label></td>
                                <td>Rev No.</td>
                                <td>
                                    <asp:Label runat="server" ID="lblRevNo"></asp:Label></td>
                                <td>Rev Date</td>
                                <td>
                                    <asp:Label runat="server" ID="lblRevDate"></asp:Label></td>
                            </tr>
                        </table>
                    </div>

                </div>

                <div class="tab-content themetoggle" id="masterContainer">

                    <div id="DnWReportMenu" class="tab-pane fade submenu-tab-content">
                        <div id="scrollMaintainDiv" style="height: 76vh; overflow: auto; margin-top: 12px">
                            <asp:ListView runat="server" ID="lvDandWReportDetails" ClientIDMode="Static">
                                <LayoutTemplate>
                                    <table class="table table-bordered table-hover headerFixer tbl-report-details" id="tblDandWReportDetails">
                                        <tr id="itemplaceholder" runat="server"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="padding: 0; border: unset">
                                            <table style="display: <%# Eval("CheckPointsHeaderVisibility") %>" class="inner-tbl checkpoint-tbl">
                                                <tr>
                                                    <th class="items">Check Point No.</th>
                                                    <th class="items">Route No.</th>
                                                    <%-- <th>Related To</th>--%>
                                                    <th class="items">C, L, I, RT</th>
                                                    <th class="items">Item</th>
                                                    <th class="checkpoint">Check Point</th>
                                                    <th class="checkpoint">Standard</th>
                                                    <th class="checkpoint">If Not Ok</th>
                                                    <th class="items">Method</th>
                                                    <th class="items">Freq.</th>
                                                    <th class="items">Day</th>
                                                </tr>
                                            </table>
                                            <table style="display: <%# Eval("CheckPointsContentVisibility") %>" class=" inner-tbl checkpoint-tbl">
                                                <tr>
                                                    <td class="items"><span title='<%# Eval("CheckPointNo") %>'><%# Eval("CheckPointNo") %></span></td>
                                                    <td class="items"><span title='<%# Eval("RouteNo") %>'><%# Eval("RouteNo") %></span></td>
                                                    <%--<td><span title='<%# Eval("RelatedTo") %>'><%# Eval("RelatedTo") %></td>--%>
                                                    <td class="items"><span title='<%# Eval("C_L_I_RT_Value") %>'><%# Eval("C_L_I_RT_Value") %></span></td>
                                                    <td class="items"><span title='<%# Eval("Item") %>'><%# Eval("Item") %></span></td>
                                                    <td class="checkpoint"><span title='<%# Eval("CheckPoint") %>'><%# Eval("CheckPoint") %></span></td>
                                                    <td class="checkpoint"><span title='<%# Eval("Standard") %>'><%# Eval("Standard") %></span></td>
                                                    <td class="checkpoint"><span title='<%# Eval("IfNotOk") %>'><%# Eval("IfNotOk") %></span></td>
                                                    <td class="items"><span title='<%# Eval("Method") %>'><%# Eval("Method") %></span></td>
                                                    <td class="items"><span title='<%# Eval("Frequency") %>'><%# Eval("Frequency") %></span></td>
                                                    <td class="items"><span title='<%# Eval("DayToDisplay") %>'><%# Eval("DayToDisplay") %></span></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="padding: 0; border-top: unset; border-right: unset; border-left: unset">
                                            <asp:ListView runat="server" ID="lvFrequencyDetails" DataSource='<%# Eval("FrequencyDetails") %>'>
                                                <LayoutTemplate>
                                                    <table class="inner-tbl">
                                                        <tr>
                                                            <td id="itemplaceholder" runat="server"></td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <td style="padding: 0; border: unset">
                                                        <table class="inner-tbl">
                                                            <tr>
                                                                <td style="padding: 0; border: unset">
                                                                    <table class="inner-tbl  value-tbl">
                                                                        <tr style="display: <%# Eval("ValueHeaderVisisbility") %>">
                                                                            <th style="width: <%# Eval("Width")  %>; min-width: <%# Eval("Width")  %>; max-width: <%# Eval("Width")  %>">
                                                                                <%# Eval("Value") %>
                                                                            </th>
                                                                        </tr>
                                                                        <tr style="display: <%# Eval("ValueContentVisisbility") %>">
                                                                            <td style="color: <%# Eval("ValueContentColor")  %>; border-bottom: unset; width: <%# Eval("Width")  %>; min-width: <%# Eval("Width")  %>; max-width: <%# Eval("Width")  %>">
                                                                                <span title='<%# Eval("Value") %>'><%# Eval("Value") %></span>

                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                    </div>

                    <div id="QReportMenu" class="tab-pane fade submenu-tab-content">
                        <div id="scrollMaintainDiv" style="height: 76vh; overflow: auto; margin-top: 12px">
                            <asp:ListView runat="server" ID="lvQReportDetails" ClientIDMode="Static">
                                <LayoutTemplate>
                                    <table class="table table-bordered table-hover headerFixer tbl-report-details" id="tblQReportDetails">
                                        <tr id="itemplaceholder" runat="server"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="padding: 0; border: unset">
                                            <table style="display: <%# Eval("CheckPointsHeaderVisibility") %>" class="inner-tbl checkpoint-tbl">
                                                <tr>
                                                    <th class="items">Check Point No.</th>
                                                    <th class="items">Route No.</th>
                                                    <%-- <th>Related To</th>--%>
                                                    <th class="items">C, L, I, RT</th>
                                                    <th class="items">Item</th>
                                                    <th class="checkpoint">Check Point</th>
                                                    <th class="checkpoint">Standard</th>
                                                    <th class="checkpoint">If Not Ok</th>
                                                    <th class="items">Method</th>
                                                    <th class="items">Freq.</th>
                                                    <th class="items">Day</th>
                                                </tr>
                                            </table>
                                            <table style="display: <%# Eval("CheckPointsContentVisibility") %>" class=" inner-tbl checkpoint-tbl">
                                                <tr>
                                                    <td class="items"><span title='<%# Eval("CheckPointNo") %>'><%# Eval("CheckPointNo") %></span></td>
                                                    <td class="items"><span title='<%# Eval("RouteNo") %>'><%# Eval("RouteNo") %></span></td>
                                                    <%--<td><span title='<%# Eval("RelatedTo") %>'><%# Eval("RelatedTo") %></td>--%>
                                                    <td class="items"><span title='<%# Eval("C_L_I_RT_Value") %>'><%# Eval("C_L_I_RT_Value") %></span></td>
                                                    <td class="items"><span title='<%# Eval("Item") %>'><%# Eval("Item") %></span></td>
                                                    <td class="checkpoint"><span title='<%# Eval("CheckPoint") %>'><%# Eval("CheckPoint") %></span></td>
                                                    <td class="checkpoint"><span title='<%# Eval("Standard") %>'><%# Eval("Standard") %></span></td>
                                                    <td class="checkpoint"><span title='<%# Eval("IfNotOk") %>'><%# Eval("IfNotOk") %></span></td>
                                                    <td class="items"><span title='<%# Eval("Method") %>'><%# Eval("Method") %></span></td>
                                                    <td class="items"><span title='<%# Eval("Frequency") %>'><%# Eval("Frequency") %></span></td>
                                                    <td class="items"><span title='<%# Eval("DayToDisplay") %>'><%# Eval("DayToDisplay") %></span></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="padding: 0; border-top: unset; border-right: unset; border-left: unset">
                                            <asp:ListView runat="server" ID="lvFrequencyDetails" DataSource='<%# Eval("FrequencyDetails") %>'>
                                                <LayoutTemplate>
                                                    <table class="inner-tbl">
                                                        <tr>
                                                            <td id="itemplaceholder" runat="server"></td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <td style="padding: 0; border: unset">
                                                        <table class="inner-tbl">
                                                            <tr>
                                                                <td style="padding: 0; border: unset">
                                                                    <table class="inner-tbl  value-tbl">
                                                                        <tr style="display: <%# Eval("ValueHeaderVisisbility") %>">
                                                                            <th style="width: <%# Eval("Width")  %>; min-width: <%# Eval("Width")  %>; max-width: <%# Eval("Width")  %>">
                                                                                <%# Eval("Value") %>
                                                                            </th>
                                                                        </tr>
                                                                        <tr style="display: <%# Eval("ValueContentVisisbility") %>">
                                                                            <td style="color: <%# Eval("ValueContentColor")  %>; border-bottom: unset; width: <%# Eval("Width")  %>; min-width: <%# Eval("Width")  %>; max-width: <%# Eval("Width")  %>">
                                                                                <span title='<%# Eval("Value") %>'><%# Eval("Value") %></span>

                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            DateTimeSetter();
            activeSubMenu();
        });
        function DateTimeSetter() {
            //$('[id$=txtFromDate]').datepicker({
            //    viewMode: "date",
            //    minViewMode: "date",
            //    format: 'dd-mm-yyyy',
            //    todayHighlight: true,
            //    autoclose: true,
            //    language: 'en-US',
            //});
            $('[id$=txtYear]').datepicker({
                viewMode: "years",
                minViewMode: "years",
                format: 'yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
            });
        }
        function setActiveSubmenuValue() {
            var lilist = $("#masterul li");
            for (let i = 0; i < lilist.length; i++) {
                let li = lilist[i];
                let display = $(li).css('display');
                if (display == "block") {
                    localStorage.setItem("JHReportSelectedMenu", $(li).find('a').attr('href'));
                    activeSubMenu();
                    break;
                }
            }
        }
        function activeSubMenu() {
            debugger;
            if (localStorage.getItem("JHReportSelectedMenu")) {
                if (localStorage.getItem("JHReportSelectedMenu")) {
                    submenu = localStorage.getItem("JHReportSelectedMenu");
                }
                //$(".tab-pane").removeClass("in active");
                $(submenu).addClass("in active");
                $("a[href$='" + submenu + "']").addClass("selected-menu-style");
            }
        }
        $(".submenuData").click(function () {
            $(".submenuData").removeClass("selected-menu-style").addClass("other-menu-style");
            $(".submenuData").closest('li').find('i').removeClass();
            $(this).removeClass("other-menu-style").addClass("selected-menu-style");
            submenu = $(this).attr('href');
            <%--if (submenu == "#StepperMotorLockingMenu") {
                __doPostBack('<%= btnStepperMotorLocking.UniqueID%>', '');
            }
            else if (submenu == "#PointerPressingMenu") {
                __doPostBack('<%= btnPointerPressing.UniqueID%>', '');
            } else if (submenu == "#FitmentGaugeMenu") {
                __doPostBack('<%= btnFitmentGauge.UniqueID%>', '');
             }--%>
            localStorage.setItem("JHReportSelectedMenu", submenu);
            //  activeSubMenu();
            // callUnLoader();
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                DateTimeSetter();
                activeSubMenu();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
