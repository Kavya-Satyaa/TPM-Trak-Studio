<%@ Page Title="" Language="C#" MasterPageFile="~/Site_Master_2.Master" AutoEventWireup="true" CodeBehind="HourDefinitionKTA.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.HourDefinitionKTA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <style>
        .form-control[readonly] {
            background-color: #fff;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
        }

        #MainContent_gridviewHourly tbody tr:nth-child(odd) {
            background-color:#DCDCDC;
        }

        #MainContent_gridviewHourly tbody tr:nth-child(even) {
            background-color:  #FFFFFF;
        }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }

        td {
            color: black;
        }

        .commanitemstyle {
            min-width: 200px;
        }
    </style>
     <asp:UpdatePanel ID="updatePanal" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row text-center">
                    <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
                </div>
                <div class="row">
                    <table class="table table-bordered" style="width: 75%">
                        <tr>
                            <td rowspan="2" style="vertical-align: middle; text-align: center">
                                <span style="display: inline" class="commontd">Shift Type&nbsp;&nbsp;&nbsp</span>
                                <asp:DropDownList ID="ddlShiftType" runat="server" CssClass="form-control" Width="140px" AutoPostBack="true" OnSelectedIndexChanged="ddlShiftType_SelectedIndexChanged" Style="display: inline"></asp:DropDownList>
                            </td>
                            </td>
                            <td rowspan="2" style="vertical-align: middle; text-align: center">
                                <span style="display: inline" class="commontd">Shift &nbsp;&nbsp;&nbsp</span>
                                <asp:DropDownList ID="ddlShiftId" runat="server" CssClass="form-control" Width="140px" AutoPostBack="true" OnSelectedIndexChanged="ddlShiftId_SelectedIndexChanged" Style="display: inline" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td style="vertical-align: middle" class="commontd">From</td>
                            <td>
                                <asp:TextBox ID="txtFromDay" runat="server" CssClass="form-control" Style="display: inline; width: 115px;" ReadOnly="true"></asp:TextBox>&nbsp;&nbsp;<asp:TextBox ID="txtFromTime" runat="server" CssClass="form-control" Style="display: inline; width: 115px;" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle;" class="commontd">To</td>
                            <td>
                                <asp:TextBox ID="txtToDay" runat="server" CssClass="form-control" Style="display: inline; width: 115px;" ReadOnly="true"></asp:TextBox>&nbsp;&nbsp;<asp:TextBox ID="txtToTime" runat="server" CssClass="form-control" Style="display: inline; width: 115px;" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>

                            <td colspan="3" style="text-align: center">
                                <asp:Button ID="btnSave" runat="server" Text="<%$Resources:CommanResource,Save %>" CssClass="btn btn-info" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnDelete" runat="server" Text="<%$Resources:CommanResource,Delete %>" CssClass="btn btn-info" OnClick="btnDelete_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnDefault" runat="server" Text="<%$Resources:CommanResource,Default %>" CssClass="btn btn-info" OnClick="btnDefault_Click" /></td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridviewHourly" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false" Width="75%">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:CommanResource,HourDefination %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtHourDef" runat="server" CssClass="txtupdate txtHourdef textboxcommon" Text='<%# Eval("HourName") %>' Style="min-width: 200px" MaxLength="40"></asp:TextBox>
                                    <asp:HiddenField ID="hdfShiftId" runat="server" Value='<%# Eval("ShiftID") %>' />
                                    <asp:HiddenField ID="hdfHourID" runat="server" Value='<%# Eval("HourID") %>' />
                                    <asp:HiddenField ID="hdfFromDay" runat="server" Value='<%# Eval("FromDay") %>' />
                                    <asp:HiddenField ID="hdfToDay" runat="server" Value='<%# Eval("ToDay") %>' />
                                    <asp:HiddenField ID="hdfHourStart" runat="server" Value='<%# Eval("HourStart") %>' />
                                    <asp:HiddenField ID="hdfHourEnd" runat="server" Value='<%# Eval("HourEnd") %>' />
                                    <asp:HiddenField ID="hdfUpdate" runat="server" />
                                </ItemTemplate>
                                <ItemStyle CssClass="commanitemstyle" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="HourStart" HeaderText="<%$Resources:CommanResource,FromTime %>" DataFormatString="{0:hh:mm:ss tt}" />

                            <asp:TemplateField HeaderText="<%$Resources:CommanResource,Minutes %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMinutes" runat="server" CssClass="numberValue txtupdate textboxcommon" Text='<%# Eval("Minutes") %>' Width="80px" MaxLength="2"
                                        AutoPostBack="true" OnTextChanged="txtMinutes_TextChanged"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="HourEnd" HeaderText="<%$Resources:CommanResource,ToTime %>" DataFormatString="{0:hh:mm:ss tt}" />
                        </Columns>
                        <HeaderStyle CssClass="HeaderCss" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        $(document).ready(function () {
            $.unblockUI({});
            $("#MainContent_gridviewHourly .numberValue").keyup(function () {
                
                this.value = this.value.replace(/[^0-9\.]/g, '');
                let minValue = $(this).val();
                if (minValue != "") {
                    if (parseInt(minValue) > 60) alert("Hour minutes cannot exceed from 60 min");
                    else if (parseInt(minValue) == 00) alert("Hour minutes cannot be Zero");
                } else alert("Hour minutes cannot be empty");
                $(this).closest('tr').find('input[name$=hdfUpdate]').val("update");
                return false;
            });

            $("#MainContent_gridviewHourly .txtHourdef").keyup(function () {
                let hourValue = $(this).val();
                if (hourValue == "") alert("Hour defination cannot be empty");
                $(this).closest('tr').find('input[name$=hdfUpdate]').val("update");
            });

            $("[id$=gridviewHourly] .txtupdate").click(function () {
                
                $(this).closest('tr').find('input[name$=hdfUpdate]').val("update");
                $("[id$=gridviewHourly] tr td").find('.txtupdate').removeClass("form-control");
                $("[id$=gridviewHourly] tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });
        });

        $(document).on("click", "[id$=btnSave]", function () {
            var count = 0;
            $("#MainContent_gridviewHourly tr:gt(0)").each(function (src, i) {

                if (($(this, i).closest("tr").find('.txtHourdef').val() == "")) {
                    count++;
                    alert('Please enter the Hour Defination !!');
                    $(this, i).closest("tr").find('.txtHourdef').removeClass("textboxcommon");
                    $(this, i).closest("tr").find('.txtHourdef').addClass("form-control");
                    $(this, i).closest("tr").find('.txtHourdef').focus();
                    return false;
                }
                if (($(this, i).closest("tr").find('.numberValue').val() == "")) {
                    count++;
                    alert('Please enter the Minutes !!');
                    $(this, i).closest("tr").find('.numberValue').removeClass("textboxcommon");
                    $(this, i).closest("tr").find('.numberValue').addClass("form-control");
                    $(this, i).closest("tr").find('.numberValue').focus();
                    return false;
                }
                if (($(this, i).closest("tr").find('.numberValue').val() > 60)) {
                    count++;
                    alert('Hour minutes cannot exceed from 60 min !!');
                    $(this, i).closest("tr").find('.numberValue').removeClass("textboxcommon");
                    $(this, i).closest("tr").find('.numberValue').addClass("form-control");
                    $(this, i).closest("tr").find('.numberValue').focus();
                    return false;
                }
            });
            if (count != 0) {
                return false;
            }
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        });

        $(document).on("click", "[id$=btnDelete]", function () {
            if ($('#ddlShiftId :selected').text() != "") {
                if (confirm("Do you want to delete all data?")) {
                    if ($("#MainContent_gridviewHourly tr").length > 1) $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                }
                else return false;
            }
            else return false;
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $("#MainContent_gridviewHourly .numberValue").keyup(function () {
                
                this.value = this.value.replace(/[^0-9\.]/g, '');
                let minValue = $(this).val();
                if (minValue != "") {
                    if (parseInt(minValue) > 60) alert("Hour minutes cannot exceed from 60 min");
                    else if (parseInt(minValue) == 00) alert("Hour minutes cannot be Zero");
                } else alert("Hour minutes cannot be empty");
                $(this).closest('tr').find('input[name$=hdfUpdate]').val("update");
                return false;
            });

            $("[id$=gridviewHourly] .txtupdate").click(function () {
                
                $(this).closest('tr').find('input[name$=hdfUpdate]').val("update");
                $("[id$=gridviewHourly] tr td").find('.txtupdate').removeClass("form-control");
                $("[id$=gridviewHourly] tr td").find('.txtupdate').addClass("textboxcommon");
                $(this).closest('td').find('input').removeClass("textboxcommon");
                $(this).closest('td').find('input').addClass("form-control");
            });

            $("#MainContent_gridviewHourly .txtHourdef").keyup(function () {
                
                let hourValue = $(this).val();
                if (hourValue == "") alert("Hour defination cannot be empty");
                $(this).closest('tr').find('input[name$=hdfUpdate]').val("update");
            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
