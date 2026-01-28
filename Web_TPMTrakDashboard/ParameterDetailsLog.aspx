<%@ Page Language="C#" Title="" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ParameterDetailsLog.aspx.cs" Inherits="Web_TPMTrakDashboard.ParameterDetailsLog" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>

    <style>
        .outerDivStyle {
            margin: 10px;
            border: 1px solid silver;
            border-radius: 20px;
            box-shadow: 2px 2px 15px #d1d1d1;
            background-color: white;
        }



        .HeaderDivLabel {
            background: radial-gradient(#36ffe3, #199aca); /*radial-gradient(#0c4770, #199aca);*/
            color: black;
            border-top-left-radius: 20px;
            border-top-right-radius: 20px;
            padding: 5px;
            font-weight: bold;
            font-size: 22px;
            height: 40px;
        }

        .lvChangeIncidentsTable {
            margin-left: 10px;
            padding-left: 10px;
            min-height: 80px;
        }

        .IncidentTableDiv {
            overflow: auto;
            height: 87%;
            border-bottom-right-radius: 10px;
        }

        .lvChangeIncidentsTable > tbody > tr > td {
            padding: 10px;
        }

        .innerHeaderDiv {
            height: 100%;
            vertical-align: middle;
            font-size: 20px;
            color: white;
            padding: 5px;
        }

        .wrapper {
            margin-top: 0px;
            padding: 60px 0 0 0;
        }

        .innerHeaderDiv td {
            color: white;
        }

        a:hover {
            color: #1fa190;
        }

        a:visited {
            color: black;
        }

        .filter-table > tbody > tr > td {
            padding: 10px;
        }

        .innerDivStyle {
            padding: 20px 5px 5px 10px;
        }

        .Date {
            position: relative;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            padding: 8px;
            line-height: 1.428571429;
            /* vertical-align: top; */
            border-top: 1px solid #ddd;
        }

        .data-color {
            background-color: aliceblue;
        }

        .headerclass {
            padding: 10px;
            text-align: center;
            width: 300px;
            background-color: cadetblue;
        }

        .data-class {
            padding: 5px;
            text-align: center;
        }

        .gvParameterChanges {
            width: 100%;
            border: 1px solid white;
        }

            .gvParameterChanges > tbody > tr > th {
                text-align: center;
                font-weight: bold;
                border: 1px solid white;
                background-color: #275183de;
                color: white;
                height: 35px;
            }

            .gvParameterChanges > tbody > tr > td {
                padding-left: 20px;
                height: 30px;
            }

            .gvParameterChanges > tbody > tr:nth-child(odd) {
                background-color: #dbd9d9;
            }

            .gvParameterChanges > tbody > tr:nth-child(even) {
                background-color: white;
            }
            .table tbody>tr>td
        {
          border-top:none;
        }
             .classDeletion{
                color: red;
                font-weight:bold;
            }
            .classChanged{
                color: blue;font-weight:bold;
            }
        .classAddition {
            color: green;font-weight:bold;

        }
    </style>

    <div runat="server" style="width: 85vw;">
        <asp:UpdatePanel ID="updatePnal" runat="server">
            <ContentTemplate>
               
                <div style="margin-left: 10px;">
                    


                    <div>
                        <table class="table" style="width: auto">
                            <tr>
                                <td class="commanTd" style="vertical-align: middle;">Machine</td>
                                <td>
                                    <asp:ListBox ID="lbMachineID" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control"></asp:ListBox>
                                </td>
                                <td class="commanTd" style="vertical-align: middle;">From Date</td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date date1 from-date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static" Height="40" Width="200"></asp:TextBox>
                                    </div>
                                </td>
                                <td class="commanTd" style="vertical-align: middle;">To Date</td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date date2 to-date" placeholder="DD-MMM-YYYY" AutoCompleteType="Disabled" ClientIDMode="Static" Height="40" Width="200"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClientClick="return FromAndToDateValidtion();" OnClick="btnView_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" OnClientClick="return FromAndToDateValidtion();" OnClick="btnExport_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="commanTd" style="vertical-align: middle">Program No</td>
                                <td>
                                    <div class="input-group">
                                        <asp:ListBox ID="lbMultiProgramNo" runat="server" SelectionMode="Multiple" ClientIDMode="Static" CssClass="form-control"></asp:ListBox>
                                        <div class="input-group-addon" style="width: 10px; padding: 0px;">
                                            <asp:LinkButton runat="server" ID="lnkRefresh" CssClass="btn btn-success btn-sm glyphicon glyphicon-refresh" Style="font-weight: bold; color: white; top: 0px; font-size: 15px; height: 37px" Text="" OnClick="lnkRefresh_Click" OnClientClick="return FromAndToDateValidtion();"></asp:LinkButton>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="btnProgramView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnProgramView_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>

                  

                    <div style="width: 98.5%; margin-left: 10px; height: 75vh; overflow: auto">
                        <asp:GridView runat="server" ID="gvParameterChanges" CssClass="gvProgramChanges headerFixer data-color" AutoGenerateColumns="false" AllowPaging="true" HeaderStyle-CssClass="text-center gvHeader" ShowHeaderWhenEmpty="true" PagerStyle-BackColor="Transparent" PageSize="100000">
                            <Columns>
                                <asp:TemplateField HeaderText="Machine" HeaderStyle-Width="150px" HeaderStyle-Height="50px" ItemStyle-CssClass="data-class" AccessibleHeaderText="MachineID">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>' CssClass="content" Style="width: 100px;"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program No." HeaderStyle-Width="150px" ItemStyle-CssClass="data-class" AccessibleHeaderText="ProgramID">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblParameterID" Style="width: 100px" Text='<%# Eval("programID") %>' CssClass="content"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Previous Data" HeaderStyle-Width="600px" ItemStyle-CssClass="data-class" AccessibleHeaderText="PreviousData">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPreviousData" Style="width: 500px" Text='<%# Eval("PreviousData") %>' CssClass="content"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Changed Data" HeaderStyle-Width="610px" ItemStyle-CssClass="data-class" AccessibleHeaderText="ChangedData">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblChangedData" Style="width: 500px" Text='<%# Eval("ChangedData") %>' CssClass="content"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Changed Time" HeaderStyle-Width="150px" ItemStyle-CssClass="data-class" AccessibleHeaderText="ChangedAt" ControlStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Style="width: 300px" ID="lblChangedAt" Text='<%# Eval("changedTime") %>' CssClass="content"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Position="Bottom" Mode="Numeric" Visible="true" />
                        </asp:GridView>
                    </div>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="modal fade" id="warningModal" role="dialog" style="z-index: 2000">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content modalContent warning-modal-content">
                    <div class="modal-header modalHeader warning-modal-header">
                        <i class="glyphicon glyphicon-warning-sign modal-icons"></i>
                        <br />
                        <h4 class="warning-modal-title">Warning</h4>
                        <br />
                        <span class="warning-modal-msg" id="lblWarningMsg">...</span>
                    </div>
                    <div class="modal-footer modalFooter modal-footer">
                        <input type="button" value="OK" class="warning-modal-btn" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            var winHeight = $(window).height();
            winHeight = screen.availHeight;
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (640);
                console.log('min');
            } else {
                //winHeight = (840);
                console.log('max');
            }
            //$("#divCustomerList").height(winHeight - 288);

            $.unblockUI({});
            //setControl();

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            $('[id$=lbMachineID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbMultiProgramNo]').multiselect({
                includeSelectAllOption: true
            });
            $("[id$=btnView]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    //$("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }
               <%-- debugger;
                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();

                if (Date.parse(from) > Date.parse(to)) {
                    alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                    $('[id$=txtToDate]').val('');
                    $('[id$=txtToDate]').focus();
                    return false;
                }--%>
                // $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                //RefreshMachineStatus();
            });
            $("[id$=btnRefresh]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    //$("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }

            });
            $("[id$=btnExport]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    //$("[id$=txtFromDate]").focus();
                    return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }

            });
        });

        function openWarningModal(msg) {
            $("#lblWarningMsg").text(msg);
            $("#warningModal").modal('show');
        }
        function stayMultiselectedList(param) {
            debugger;
            if (param == "machine") {
                $("#trMachine .btn-group").addClass('open');
            }
        }

        debugger;
        function FromAndToDateValidtion() {
            debugger;
            let dataitem;
            $.ajax({
                async: false,
                type: "POST",
                url: "ParameterDetailsLog.aspx/FromAndToDateValidation",
                contentType: "application/json; charset=utf-8",
                data: "{fromDate:'" + $(".from-date").val() + "', toDate:'" + $(".to-date").val() + "'}",
                crossDomain: true,
                dataType: "json",
                success: function (response) {
                    dataitem = response.d;
                },
                error: function (Result) {
                    alert("Error" + Result);
                }
            });
            if (dataitem == "Greater") {
                openWarningModal("From Date should be less than To Date.");
                return false;
            }
            else {
                return true;
            }
        }


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});

            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MM-YYYY HH:mm:ss',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });
            $('[id$=lbMachineID]').multiselect({
                includeSelectAllOption: true
            });
            $('[id$=lbMultiProgramNo]').multiselect({
                includeSelectAllOption: true
            });
            $("[id$=btnView]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    //$("[id$=txtFromDate]").focus();
                    //return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }

            });
            $("[id$=btnRefresh]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    //$("[id$=txtFromDate]").focus();
                    //return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }

            });
            $("[id$=btnExport]").click(function () {
                if ($("[id$=txtFromDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                    //$("[id$=txtFromDate]").focus();
                    //return false;
                }
                if ($("[id$=txtToDate]").val() == "") {
                    alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                    $("[id$=txtToDate]").focus();
                    return false;
                }

            });


        });
    </script>
</asp:Content>
