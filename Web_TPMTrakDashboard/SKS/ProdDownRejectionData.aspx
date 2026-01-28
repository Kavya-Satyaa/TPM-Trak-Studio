<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProdDownRejectionData.aspx.cs" Inherits="Web_TPMTrakDashboard.SKS.ProdDownRejectionData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>

    <style>
        .commanTd {
            vertical-align: middle !important;
        }

        .submenuData {
            background: white;
            color: #428bca;
            border: 1px solid #428bca;
            font-weight: bold;
            font-size: 15px;
        }

        .selected-Submenu {
            background: #428bca  !important;
            color: white  !important;
        }

        .lvTable tr th {
            vertical-align: middle !important;
            text-align: center;
        }

        .lvTable > tbody > tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
        }

        .lvTable > tbody > tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
        }

        .lvTable tr td {
            white-space: nowrap;
        }

        .headerSecondRow {
            top: 56px !important;
        }

        #paginationTD th, #paginationTD {
            color: white;
            background-color: #2E6886 !important;
        }

        #lvProductionDataPager a, #lvDownDataPager a, #lvRejectionData a {
            padding: 5px 10px;
            background: white;
        }

        #lvProductionDataPager span, #lvDownDataPager span, #lvRejectionData span {
            padding: 5px 10px;
            background: #5bc0de;
            color: white;
        }

        .warning-modal-content {
            border: 2px solid #f7d631;
            background-color: white;
            border-radius: 6px;
            text-align: center;
        }

        .warning-modal-header {
            background-color: #f7d631;
            padding: 15px;
            color: black;
            text-align: center;
        }

        .warning-modal-title {
            color: black;
            font-weight: bold;
            font-size: 28px;
        }

        .warning-modal-msg {
            font-size: 18px;
            color: black;
        }

        .warning-modal-btn {
            background-color: #f7d631;
            border: 1px solid #f7d631;
            color: black;
            font-weight: bold;
            padding: 8px 25px;
            border-radius: 8px;
        }

        .error-modal-content {
            border: 2px solid #f50505;
            background-color: white;
            border-radius: 6px;
            text-align: center;
        }

        .error-modal-header {
            background-color: #f50505;
            padding: 15px;
            color: white;
            text-align: center;
        }

        .error-modal-title {
            color: white;
            font-weight: bold;
            font-size: 28px;
        }

        .error-modal-msg {
            font-size: 18px;
            color: white;
        }

        .error-modal-btn {
            background-color: #f50505;
            border: 1px solid #f50505;
            color: white;
            font-weight: bold;
            padding: 8px 25px;
            border-radius: 8px;
        }

        .modal-footer {
            padding: 18px;
            text-align: center;
        }

        .modal-icons {
            font-size: 4pc;
        }

        .infoModal .modal-content {
            border: 1px solid #6c7884;
            background: #1d1d1d;
        }

        .infoModal .modal-footer {
            padding: 9px;
            border: 1px solid #6c7884;
            background-color: #1d1d1d;
            margin: 0px;
        }

        .infoModal .modal-header {
            border-bottom: 1px solid #6c7884;
            background-color: #6c7884;
        }

            .infoModal .modal-header .modal-title {
                color: white;
            }

            .infoModal .modal-header h4 {
                font-weight: bold;
            }

        .infoModal .modal-body {
            border-left: 1px solid #6c7884;
            border-right: 1px solid #6c7884;
            background-color: white;
        }

        .infoModal .div-border-style {
            border-bottom: 1px solid #575a5d;
        }

        .infoModal .div-border-top-style {
            border-top: 1px solid #575a5d;
        }

        .priorityTypeDiv {
            width: 450px;
            background-color: #f8f8f8;
            border: 1px solid #c2bdbd;
            padding: 5px 10px;
        }
    </style>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnSelectedMenu" ClientIDMode="Static" />
            <table id="tblFilter" class="table table-bordered" style="width: auto">
                <tr>
                    <td class="commanTd">From Date</td>
                    <td class="input-group">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtFromDate" runat="server" Style="width: 120px;" CssClass="form-control date" placeholder="DD-MM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="commanTd">To Date</td>
                    <td class="input-group">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-calendar"></i>
                        </div>
                        <asp:TextBox ID="txtToDate" runat="server" Style="width: 120px;" CssClass="form-control date" placeholder="DD-MM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="commanTd">Machine ID</td>
                    <td>
                        <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" AutoPostBack="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlMachineId_SelectedIndexChanged" />
                    </td>
                    <%--<td class="commanTd">Part ID</td>
                    <td>
                        <asp:DropDownList ID="ddlPartID" runat="server" CssClass="form-control" ClientIDMode="Static" />
                    </td>--%>
                    <td class="commanTd">Part Desc</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPartDescSearch" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td class="commanTd">Shift</td>
                    <td>
                        <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-control" ClientIDMode="Static" />
                    </td>
                    <td>
                        <asp:Button runat="server" Text="View" ID="btnView" CssClass="btn btn-primary btnStyle" OnClick="btnView_Click" OnClientClick="return viewValidation();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="navbar-collapse collapse" style="height: 42px !important; padding-left: 0px">
                <ul id="masterul" class="nav navbar-nav nextPrevious submenus-style ">
                    <li id="ProductionTab" runat="server"><a runat="server" class="submenuData" id="A15" clientidmode="static" data-toggle="tab" href="#productionMenu">Production</a>
                        <i></i>
                    </li>
                    <li id="DownTab" runat="server"><a runat="server" class="submenuData" id="A14" clientidmode="static" data-toggle="tab" href="#downMenu">Down</a>
                        <i></i>
                    </li>

                    <li id="RejectionTab" runat="server"><a runat="server" class="submenuData" id="A16" clientidmode="static" data-toggle="tab" href="#rejectionMenu">Rejection</a>
                        <i></i>
                    </li>
                </ul>
                <div style="display: none">
                    <asp:Button runat="server" ID="btnProduction" OnClick="btnProduction_Click" />
                    <asp:Button runat="server" ID="btnDown" OnClick="btnDown_Click" />
                    <asp:Button runat="server" ID="btnRejection" OnClick="btnRejection_Click" />
                </div>
            </div>
            <div class="tab-content themetoggle" id="processParamContainer" style="margin-top: 22px;">
                <div id="productionMenu" class="tab-pane fade">
                    <div style="height: 75vh; overflow: auto">
                        <asp:ListView runat="server" ID="lvProductionData" ClientIDMode="Static" ItemPlaceholderID="itemPlaceHolder">
                            <LayoutTemplate>
                                <table class="table table-bordered headerFixer lvTable">
                                    <tr>
                                        <th rowspan="2">Date</th>
                                        <th rowspan="2">Shift</th>
                                        <th rowspan="2">Machine</th>
                                        <th rowspan="2">Supervisor name</th>
                                        <th rowspan="2">Setter Name</th>
                                        <th rowspan="2">Operator name</th>
                                        <th rowspan="2">Work Order No.</th>
                                        <th rowspan="2">Catalog Part description</th>
                                        <th rowspan="2">Tool Layout</th>
                                        <th rowspan="2">SKS Drawing No.</th>
                                        <th rowspan="2">RM Grade</th>
                                        <th colspan="2">Planned quantity</th>
                                        <th colspan="2">Actual quantity</th>
                                        <th colspan="3">Rejection</th>
                                        <th rowspan="2">Standard Lot Number </th>
                                        <th rowspan="2">Time period for production</th>
                                        <th rowspan="2">Speed at machine is runing</th>
                                        <th rowspan="2">% AE</th>
                                        <th rowspan="2">% PE</th>
                                        <th rowspan="2">% QE</th>
                                        <th rowspan="2">% OEE</th>
                                        <th rowspan="2">Synced Status</th>
                                        <th rowspan="2">UpdatedTS</th>
                                    </tr>
                                    <tr>
                                        <th class="headerSecondRow">No.</th>
                                        <th class="headerSecondRow">kg</th>
                                        <th class="headerSecondRow">No.</th>
                                        <th class="headerSecondRow">kg</th>
                                        <th class="headerSecondRow">Set-up-Rej </th>
                                        <th class="headerSecondRow">In proces-Rej</th>
                                        <th class="headerSecondRow">Re-setting -Rej</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceHolder"></tr>
                                    <tr id="Tr3" runat="server">
                                        <td id="paginationTD" runat="server" style="text-align: center" colspan="9999">
                                            <asp:DataPager ID="lvProductionDataPager" OnPreRender="lvProductionDataPager_PreRender" PageSize="500" PagedControlID="lvProductionData"
                                                runat="server">
                                                <Fields>
                                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowFirstPageButton="True" ShowNextPageButton="False"
                                                        ShowPreviousPageButton="False" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowLastPageButton="True" ShowNextPageButton="False"
                                                        ShowPreviousPageButton="False" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("FormateDate") %></td>
                                    <td><%# Eval("Shift") %></td>
                                    <td><%# Eval("Machine") %></td>
                                    <td><%# Eval("SupervisorName") %></td>
                                    <td><%# Eval("SetterName") %></td>
                                    <td><%# Eval("OperatorName") %></td>
                                    <td><%# Eval("WorkOrderNo") %></td>
                                    <td><%# Eval("CatalogCode_Description") %></td>
                                    <td><%# Eval("ToolLayout") %></td>
                                    <td><%# Eval("DrawingNumber") %></td>
                                    <td><%# Eval("RMGrade_Size") %></td>
                                    <td><%# Eval("PlannedQty") %></td>
                                    <td><%# Eval("PlnQtyWeightInKg") %></td>
                                    <td><%# Eval("ActualQty") %></td>
                                    <td><%# Eval("ActQtyWeightInKg") %></td>
                                    <td><%# Eval("SetUpRejection") %></td>
                                    <td><%# Eval("InProcessRejection") %></td>
                                    <td><%# Eval("ReSettingRejection") %></td>
                                    <td><%# Eval("LotNumber") %></td>
                                    <td><%# Eval("ProductionTime") %></td>
                                    <td><%# Eval("Speed") %></td>
                                    <td><%# Eval("AE") %></td>
                                    <td><%# Eval("PE") %></td>
                                    <td><%# Eval("QE") %></td>
                                    <td><%# Eval("OEE") %></td>
                                    <td><%# Eval("Status") %></td>
                                    <td><%# Eval("UpdatedTS") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div id="downMenu" class="tab-pane fade">
                    <div style="height: 75vh; overflow: auto">
                        <asp:ListView runat="server" ID="lvDownData" ClientIDMode="Static" ItemPlaceholderID="itemPlaceHolder">
                            <LayoutTemplate>
                                <table class="table table-bordered headerFixer lvTable">
                                    <tr>
                                        <th>Date</th>
                                        <th>Shift</th>
                                        <th>Machine</th>
                                        <th>Supervisor name</th>
                                        <th>Setter Name</th>
                                        <th>Operator name</th>
                                        <th>Catalog Code description</th>
                                        <th>Standard Lot No</th>
                                        <th>Down Start time</th>
                                        <th>Down End time</th>
                                        <th>Total time Lapsed</th>
                                        <th>Delay Code</th>
                                        <th>Delay code description</th>
                                        <th>Synced Status</th>
                                        <th>UpdatedTS</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceHolder"></tr>
                                    <tr id="Tr3" runat="server">
                                        <td id="paginationTD" runat="server" style="text-align: center" colspan="9999">
                                            <asp:DataPager ID="lvDownDataPager" OnPreRender="lvDownDataPager_PreRender" PageSize="500" PagedControlID="lvDownData"
                                                runat="server">
                                                <Fields>
                                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowFirstPageButton="True" ShowNextPageButton="False"
                                                        ShowPreviousPageButton="False" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowLastPageButton="True" ShowNextPageButton="False"
                                                        ShowPreviousPageButton="False" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("FormateDate") %></td>
                                    <td><%# Eval("Shift") %></td>
                                    <td><%# Eval("Machine") %></td>
                                    <td><%# Eval("SupervisorName") %></td>
                                    <td><%# Eval("SetterName") %></td>
                                    <td><%# Eval("OperatorName") %></td>
                                    <td><%# Eval("CatalogCode_Description") %></td>
                                    <td><%# Eval("LotNumber") %></td>
                                    <td><%# Eval("DownstartTime") %></td>
                                    <td><%# Eval("DownEndTime") %></td>
                                    <td><%# Eval("Downtime") %></td>
                                    <td><%# Eval("DelayCode") %></td>
                                    <td><%# Eval("DelayCodeDescription") %></td>
                                    <td><%# Eval("Status") %></td>
                                    <td><%# Eval("UpdatedTS") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div id="rejectionMenu" class="tab-pane fade">
                    <div style="height: 75vh; overflow: auto">
                        <asp:ListView runat="server" ID="lvRejectionData" ClientIDMode="Static" ItemPlaceholderID="itemPlaceHolder">
                            <LayoutTemplate>
                                <table class="table table-bordered headerFixer lvTable">
                                    <tr>
                                        <th>Date</th>
                                        <th>Shift</th>
                                        <th>Machine</th>
                                        <th>Supervisor name</th>
                                        <th>Setter Name</th>
                                        <th>Operator name</th>
                                        <th>Catalog Code description</th>
                                        <th>Standard Lot No</th>
                                        <th>Rejection Category</th>
                                        <th>Rejection code</th>
                                        <th>Quantity</th>
                                        <th>Synced Status</th>
                                        <th>UpdatedTS</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceHolder"></tr>
                                    <tr id="Tr3" runat="server">
                                        <td id="paginationTD" runat="server" style="text-align: center" colspan="9999">
                                            <asp:DataPager ID="lvRejectionDataPager" OnPreRender="lvRejectionDataPager_PreRender" PageSize="500" PagedControlID="lvRejectionData"
                                                runat="server">
                                                <Fields>
                                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowFirstPageButton="True" ShowNextPageButton="False"
                                                        ShowPreviousPageButton="False" />
                                                    <asp:NumericPagerField />
                                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-info btn-sm" ShowLastPageButton="True" ShowNextPageButton="False"
                                                        ShowPreviousPageButton="False" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("FormateDate") %></td>
                                    <td><%# Eval("Shift") %></td>
                                    <td><%# Eval("Machine") %></td>
                                    <td><%# Eval("SupervisorName") %></td>
                                    <td><%# Eval("SetterName") %></td>
                                    <td><%# Eval("OperatorName") %></td>
                                    <td><%# Eval("CatalogCode_Description") %></td>
                                    <td><%# Eval("LotNumber") %></td>
                                    <td><%# Eval("RejectionCategory") %></td>
                                    <td><%# Eval("RejectionID") %></td>
                                    <td><%# Eval("RejectionQty") %></td>
                                    <td><%# Eval("Status") %></td>
                                    <td><%# Eval("UpdatedTS") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="modal fade" id="warningModal" role="dialog" style="z-index: 2000">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content modalContent warning-modal-content">
                <div class="modal-header modalHeader warning-modal-header">
                    <i class="glyphicon glyphicon-warning-sign modal-icons"></i>
                </div>
                <div>
                    <br />
                    <h4 class="warning-modal-title">Warning!</h4>
                    <br />
                    <span class="warning-modal-msg" id="lblWarningMsg">...</span>
                </div>
                <div class="modal-footer modalFooter modal-footer">
                    <input type="button" value="OK" class="warning-modal-btn" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="errorModal" role="dialog" style="z-index: 2000">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content modalContent error-modal-content">
                <div class="modal-header modalHeader error-modal-header">
                    <i class="glyphicon glyphicon-remove-sign modal-icons"></i>
                </div>
                <br />
                <h4 class="error-modal-title">Error</h4>
                <br />
                <span class="error-modal-msg" id="lblErrorMsg">...</span>

                <div class="modal-footer modalFooter modal-footer">
                    <input type="button" value="OK" class="error-modal-btn" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <script>
        var submenu = "";
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        $(document).ready(function () {
            localStorage.setItem("selectedMenu", "#productionMenu");
            if (localStorage.getItem("selectedMenu")) {
                debugger;
                submenu = localStorage.getItem("selectedMenu");
            }
            $(submenu).addClass("in active");
            $("a[href$='" + submenu + "']").addClass("selected-Submenu");
            setControls();
        });
        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }
        function viewValidation() {
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();
            var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
            if (diffe > 30) {
                openWarningModal("Difference between to date and from date cannot be more than 30 days.");
                return false;
            }
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            return true;
        }
        function setControls() {
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: 'en-US'
            });
        }
        $(".submenuData").click(function () {
            debugger;
            //$.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });

            $(".submenuData").removeClass("selected-Submenu");
            $(".submenuData").closest('li').find('i').removeClass();
            $(this).addClass("selected-Submenu");
            submenu = $(this).attr('href');
            $(submenu).addClass("in active");
            if (submenu == "#rejectionMenu") {
                $('#hdnSelectedMenu').val("Rejection");
                //__doPostBack('<%= btnRejection.UniqueID%>', '');
            }
            else if (submenu == "#downMenu") {
                $('#hdnSelectedMenu').val("Down");
               // __doPostBack('<%= btnDown.UniqueID%>', '');
            }
            else {
                $('#hdnSelectedMenu').val("Production");
                //__doPostBack('<%= btnRejection.UniqueID%>', '');
            }
            localStorage.setItem("selectedMenu", submenu);
        });
        function openWarningModal(msg) {
            $("#lblWarningMsg").text(msg);
            $("#warningModal").modal('show');
        }
        function openErrorModal(msg) {
            $('#errorModal').modal('show');
            $('#lblErrorMsg').text(msg);
        }
        function setTabColor() {
            if (localStorage.getItem("selectedMenu")) {
                debugger;
                submenu = localStorage.getItem("selectedMenu");
            }
            $(".tab-pane").removeClass("in active");
            $(submenu).addClass("in active");
            $("a[href$='" + submenu + "']").addClass("selected-Submenu");
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                $.unblockUI({});
                setTabColor();
                setControls();
            });
            $(".submenuData").click(function () {
                debugger;
                //$.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });

                $(".submenuData").removeClass("selected-Submenu");
                $(".submenuData").closest('li').find('i').removeClass();
                $(this).addClass("selected-Submenu");
                submenu = $(this).attr('href');
                $(submenu).addClass("in active");
                if (submenu == "#rejectionMenu") {
                    $('#hdnSelectedMenu').val("Rejection");
                    //__doPostBack('<%= btnRejection.UniqueID%>', '');
                }
                else if (submenu == "#downMenu") {
                    $('#hdnSelectedMenu').val("Down");
                    // __doPostBack('<%= btnDown.UniqueID%>', '');
                }
                else {
                    $('#hdnSelectedMenu').val("Production");
                    //__doPostBack('<%= btnRejection.UniqueID%>', '');
                }
                localStorage.setItem("selectedMenu", submenu);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
