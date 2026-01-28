<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AggregationPage.aspx.cs" Inherits="Web_TPMTrakDashboard.AggregationPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.js"></script>
    <link href="Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="Scripts/Sematic/semantic.min.js"></script>
    <style type="text/css">
        .headerFixerTable tr th {
            position: sticky;
            top: 0px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
        }

        .table {
            margin-bottom: 0px;
        }

        th {
            cursor: pointer;
            text-align: center;
        }

        .divGrid {
            width: 100%;
            overflow: auto;
            margin-top: 15px;
        }

            .divGrid th {
                background-color: #2e6886;
                color: white;
            }

            .divGrid td {
                color: white;
                height: 20px;
            }

        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        .table tbody > tr > th {
            vertical-align: middle;
            width: auto;
        }

        .table > tr > td {
            vertical-align: middle;
        }
        /* Handle */
        ::-webkit-scrollbar-thumb {
            background-color: blue;
            border-radius: 15px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #000000;
            }

        .table thead > tr > th {
            vertical-align: top;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }

        #tableData {
            width: 100%;
            color: white;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
        }

        table tbody > tr > td {
            text-align: center;
        }

        .table .lbl {
            padding-top: 10px;
        }

        #tblfilter tr td {
            vertical-align: middle;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }

        .headerFixerTable tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        .headerFixerTable tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }
    </style>

    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <div class="ui segment" style="height: 10vh">
                    <table>
                        <tr>
                            <td class="commontd" style="width: 100px; color: black;"><b>From Date:</b></td>
                            <td class="input-group" style="width: 220px; height: 40px">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="190" Style="width: 160px; height: 40px" CssClass="form-control date" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td class="commontd" style="width: 100px; color: black;"><b>To Date:</b></td>
                            <td class="input-group" style="width: 220px; height: 60px; height: 40px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtToDate" Width="160" Style="width: 160px; height: 40px;" runat="server" CssClass="form-control date" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>

                            <td style="width: 80px;"><b>Plant ID:</b>
                            </td>
                            <td style="width: 170px;">
                                <asp:DropDownList Width="150" runat="server" ID="cmbPlantId" CssClass="form-control" OnSelectedIndexChanged="cmbPlantId_SelectedIndexChanged" AutoPostBack="true" />
                            </td>

                            <td style="width: 160px;" colspan="2">
                                <asp:CheckBox runat="server" ID="chkMachineAll" OnCheckedChanged="chkMachineAll_CheckedChanged" AutoPostBack="true" />
                                <span>Select All Machine</span>
                            </td>

                        </tr>
                    </table>
                </div>
                <div style="height: 80vh">
                    <div style="height: 90%; overflow: auto;">
                        <div class="col-lg-6">
                            <asp:GridView runat="server" ID="gridview" CssClass="table table-bordered headerFixerTable" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="MachineID" HeaderStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="MachineID" Text='<%# Bind("MachineID") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:CheckBox Checked='<%# Bind("AggData") %>' runat="server" ID="chkSelect" CssClass="combo"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Aggregated" HeaderStyle-Width="150">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLastAggDate" Text='<%# Bind("LastAggDate") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div style="text-align: right; height: 10%; margin: 10px;">
                        <asp:Button runat="server" ID="btnAgregate" ClientIDMode="Static" OnClick="btnAgregate_Click" CssClass="ui violet button" Text="Aggregate" />
                        <asp:Button runat="server" ID="btnDelete" CssClass="ui violet button" Text="Delete" ClientIDMode="Static" />
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="modal fade" id="divmodal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                <div class="modal-content" style="border: 2px solid #2E6886">
                    <div class="modal-header" style="background-color: #2E6886; padding: 8px">
                        <h4 class="modal-title" style="color: white;">Aggregated Data Password Verification</h4>
                    </div>
                    <div class="modal-body ui segment">
                        <div style="margin: 5px;">
                            <span style="font-size: 17px;">You are Logged in as:</span>
                            <b><span id="lblUserID" style="font-size: 17px; width: 120px"></span></b>

                        </div>
                        <div style="margin: 5px;">
                            <span style="font-size: 17px; height: 40px">Enter Your password:</span>
                            <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" ClientIDMode="Static" />
                        </div>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #2E6886; text-align: right">
                        <input type="button" value="Verify" class="btn btn-info" id="btn2" onserverclick="btnVerify_Click" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                        <input type="button" value="Cancel" class="btn btn-info" id="Button1" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        const _MS_PER_DAY = 1000 * 60 * 60 * 24;
        $(document).ready(function () {
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('#btnDelete').click(function () {

                var val = true;
                document.getElementById("txtPassword").textContent = "";
                $('#txtPassword').val("");
                $("[id$=gridview] tr").each(function () {
                    debugger;
                    $(this).find('.combo').children().is(':checked')
                    if ($(this).find('.combo').children().is(':checked')) {
                        val = false;
                    }
                });
                if (val) {
                    alert('Please Select Any on row to Delete');
                }
                else {
                    $('[id*=divmodal]').modal('show');
                    $("#lblUserID").text('<%= UserName%>');
                }
            })

            $('#btnAgregate').click(function () {
                var val = true;
                $("[id$=gridview] tr").each(function () {
                    $(this).find('.combo').children().is(':checked')
                    if ($(this).find('.combo').children().is(':checked')) {

                        val = false;
                    }
                });

                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();
                if (from != undefined && to != undefined) {
                    var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
                    if (diffe >= 7) {
                        alert("Time interval cannot be more than 7 days.");
                        $("[id$=txtToDate]").focus();
                        return false;
                    }
                }
                if (val) {
                    alert('Please Select Any on row to Aggregate');
                    return false;
                }
                else {
                    if (confirm("Are u Sure To Aggregate?")) {
                        $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            });
        });
        function dateDiffInDays(a, b) {
            // Discard the time and time-zone information.
            const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
            const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
            return Math.floor((utc2 - utc1) / _MS_PER_DAY);
        }
        var text = '<%= UserName%>';
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $('.date').datetimepicker({
                format: 'DD-MM-YYYY',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });

            $('#btnDelete').click(function () {
                var count = 0;
                document.getElementById("txtPassword").textContent = "";
                $('#txtPassword').val("");
                $("[id$=gridview] tr td").each(function () {
                    $(this).find('.combo').children().is(':checked')
                    if ($(this).find('.combo').children().is(':checked')) {
                        count++;
                    }
                });
                if (count == 0) {
                    alert('Please Select Any on row to Delete');
                }
                else {
                    $('[id*=divmodal]').modal('show');
                    $("#lblUserID").text('<%= UserName%>');
                }
            });


            $('#btnAgregate').click(function () {
                var val = true;
                $("[id$=gridview] tr").each(function () {
                    $(this).find('.combo').children().is(':checked')
                    if ($(this).find('.combo').children().is(':checked')) {

                        val = false;
                    }
                });

                var from = $("[id$=txtFromDate]").val();
                var to = $('[id$=txtToDate]').val();
                if (from != undefined && to != undefined) {
                    var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
                    if (diffe >= 7) {
                        alert("Time interval cannot be more than 7 days.");
                        $("[id$=txtToDate]").focus();
                        return false;
                    }
                }
                if (val) {
                    alert('Please Select Any on row to Aggregate');
                    return false;
                }
                else {
                    if (confirm("Are u Sure To Aggregate?")) {
                        $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
