<%@ Page Title="PM Transaction" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PMCheckListTransaction.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.PMCheckListTransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <link href="../Scripts/Sematic/semantic.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.js"></script>
    <link href="../Scripts/Sematic/semantic.min.css" rel="stylesheet" />
    <script src="../Scripts/Sematic/semantic.min.js"></script>
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
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            border-top: 0px none;
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
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <div class="ui segment">
                    <h3>Search
                    </h3>
                    <table>
                        <tr>
                            <%--<td>
                                <span>Plant ID:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlplantID" OnSelectedIndexChanged="ddlplantID_SelectedIndexChanged" CssClass="form-control" />
                            </td>
                            <td>
                                <span>Cell ID:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlcellID" CssClass="form-control" OnSelectedIndexChanged="ddlcellID_SelectedIndexChanged" />
                            </td>--%>
                            <td>
                                <span>Machine ID:</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlMachineID" CssClass=" form-control" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnLoad" CssClass="ui violet button" OnClick="btnLoad_Click" Text="View" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="ui segment">
                    <table>
                        <tr>
                            <td><span>PM Start Date :</span></td>
                            <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td><span>PM Next Date :</span></td>
                            <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtPMENdDate" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                            <td >
                                <asp:Button runat="server" ID="btnUpdatePMDate" CssClass="ui violet button" OnClick="btnUpdatePMDate_Click" Text="Update PM Date" Visible="true"/>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnPostponePMDate" CssClass="ui violet button" OnClick="btnPostponePMDate_Click" Text="Postpone PM Date" ClientIDMode="Static"/>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnSaveTransaction" CssClass="ui violet button" OnClick="btnSaveTransaction_Click" Text="Save Transaction" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView runat="server" ID="gridViewTransaction" AutoGenerateColumns="false" ShowFooter="false" ShowHeaderWhenEmpty="true" CssClass="table table-bordered headerFixerTable">
                        <Columns>
                            <asp:TemplateField HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCategory" Text='<%#Bind("Category") %>' ForeColor="White" />
                                    <asp:HiddenField runat="server" ID="lblMachine" Value='<%# Bind("MachineID") %>' />
                                    <asp:HiddenField runat="server" ID="hidIDD" Value='<%# Bind("IDD") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="txtItem" Text='<%#Bind("Items") %>' ForeColor="White" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OK">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkOK" Checked='<%# Bind("OkData")%>' OnCheckedChanged="chkOK_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Not OK">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkNotOK" Checked='<%# Bind("NotOkData") %>' OnCheckedChanged="chkNotOK_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtRemarks" Text='<%# Bind("Remarks") %>' CssClass="form-control" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="modal fade" id="divmodal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #2E6886">
                <div class="modal-header" style="background-color: #2E6886; padding: 8px">
                    <h4 class="modal-title" style="color: white;">Next PM Date</h4>
                </div>
                <div class="modal-body ui segment">
                    <table>
                        <tr>

                            <td class="input-group" style="width: 160px; border-color: transparent; height: 40px;">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-calendar"></i>
                                </div>
                                <asp:TextBox ID="txtNextPmCheck" Style="width: 180px; height: 42px;" runat="server" CssClass="form-control date" placeholder="Date" AutoCompleteType="Disabled"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #2E6886; text-align: right">
                    <input type="button" value="Verify" class="btn btn-info" id="btn2" onserverclick="btnVerify_Click" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                    <input type="button" value="Cancel" class="btn btn-info" id="Button1" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $.unblockUI({});
            $(".date").datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtNextPmCheck]").on("dp.change", function () {
                $('[id$=txtNextPmCheck]').data("DateTimePicker").minDate(new Date());
            });
            $("#btnPostponePMDate").click(function () {
                if (confirm("Are you sure to Postpone the Date")) {
                    return true;
                }
                else
                    return false;
            });
        });

        function ModelNextDateShow() {
            $('[id*=divmodal]').modal('show');
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(".date").datetimepicker({
                format: 'DD-MM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("#btnPostponePMDate").click(function () {
                if (confirm("Are you sure to Postpone the Date")) {
                    return true;
                }
                else
                    return false;
            });
            $("[id$=txtNextPmCheck]").on("dp.change", function () {
                $('[id$=txtNextPmCheck]').data("DateTimePicker").minDate(new Date());
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
