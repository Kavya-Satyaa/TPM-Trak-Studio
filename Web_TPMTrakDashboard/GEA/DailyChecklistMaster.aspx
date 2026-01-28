<%@ Page Language="C#" Title="Daily Checklist Master" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="DailyChecklistMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.DailyChecklistMaster" %>

<asp:Content ID="MainContentArea" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .header-center {
            text-align: center;
        }

        table tr th {
            text-align: center !important;
        }

        .headerFixerhere tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #5391CA;
            color: white;
        }

        .DataOperations {
            bottom: 0;
            right: 0;
            float: right;
            margin-right: 5px;
            min-height: 40px;
            position: fixed;
        }

        .btnStyle {
            margin-left: 2px;
            margin-right: 2px;
        }
    </style>

    <div class="container-fluid" style="margin-left: -5px;">
        <asp:UpdatePanel ID="UpdatePanelMaintChklist" runat="server">
            <ContentTemplate>
                <div class="row" style="text-align: center; color: red;">
                    <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
                </div>

                <div class="row" style="height: 60px;">
                    <table id="tblFilter" class="table table-bordered">
                        <tr>
                            <td class="commanTd" style="width: 100px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","LineID") %></td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlLineID" runat="server" CssClass="form-control" meta:resourcekey="ddlLineIdResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlLineID_SelectedIndexChanged" />
                            </td>

                            <td class="commanTd" style="width: 100px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","MachineId") %></td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlMachineId" runat="server" CssClass="form-control" meta:resourcekey="ddlMachineIdResource1" AutoPostBack="true" />
                            </td>

                            <td class="commanTd" style="width: 80px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","Freq") %></td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="form-control" meta:resourcekey="ddlFreqResource1" AutoPostBack="true" />
                            </td>

                            <td style="width: 130px;">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnView" Text="View" Style="min-width: 120px;" OnClick="btnView_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>

                <div id="gridviewdiv" style="overflow-x: auto; overflow-y: auto;height:70vh">
                    <asp:GridView ID="GridChecklistMaster" ClientIDMode="Static" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true" OnRowEditing="GridChecklistMaster_RowEditing" OnRowDataBound="GridChecklistMaster_RowDataBound">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected plant, line and date time period.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfUpdate" runat="server" />
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="IDD" SortExpression="IDD" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDD" runat="server" Text='<%#Bind("IDD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Check Point ID" SortExpression="ActivityID" ControlStyle-Width="150">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCheckPointID" runat="server" CssClass="form-control " TextMode="Number" Text='<%#Bind("ActivityID") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Check Points" SortExpression="Activity">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCheckPoints" runat="server" CssClass="form-control" Text='<%#Bind("Activity") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Method" SortExpression="Method" ControlStyle-Width="130">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMethod" runat="server" SelectedValue='<%#Bind("Method") %>' CssClass="form-control">
                                        <asp:ListItem>Manual</asp:ListItem>
                                        <asp:ListItem>Visual</asp:ListItem>
                                        <asp:ListItem>Hear</asp:ListItem>
                                        <asp:ListItem>Touch</asp:ListItem>
                                        <asp:ListItem>Refracto-Meter</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Criteria" SortExpression="Criteria" ControlStyle-Width="150">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCriteria" runat="server" CssClass="form-control" Text='<%#Bind("Criteria") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Template Type" ControlStyle-Width="160">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTemplateType" runat="server" SelectedValue='<%#Bind("TemplateType") %>' CssClass="form-control">
                                        <asp:ListItem>Text</asp:ListItem>
                                        <asp:ListItem>Numeric</asp:ListItem>
                                        <asp:ListItem>Ok/NotOk</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Is Enabled" ControlStyle-Width="130">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsEnabled" runat="server" Checked='<%#Bind("IsEnabled") %>' CssClass="form-control" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle BackColor="#6699ff" />
                        <HeaderStyle BackColor="#5391CA" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                        <RowStyle BackColor="#BACADE" Font-Size="Medium" ForeColor="#383838" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
                <div class="DataOperations">
                    <asp:Button runat="server" ID="btnApplyRules" Text="Apply Rules For All Frequency" CssClass="btn btn-primary btnStyle" OnClick="btnApplyRules_Click" />
                    <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-primary btnStyle" OnClick="btnNew_Click" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary btnStyle" OnClick="btnCancel_Click" />
                    <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary btnStyle" OnClick="btnDelete_Click" />
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary btnStyle" OnClick="btnSave_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!--Message Popup -->
    <div id="MessagePopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //$(document).ready(function () {
        //    debugger;
        //    var winHeight = $(window).height();
        //    //winHeight = screen.availHeight;
        //    console.log(winHeight);
        //    $("#gridviewdiv").css('height', winHeight-140);
        //});
        $(".form-control").change(function () {
            $(this).closest('tr').find('input[type=hidden]').val("Update");
        });
        
           
           
      
        $("[id$=btnDelete]").click(function () {
            var chkCount = 0;
            $("#GridChecklistMaster tr").not(':first').each(function () {
                var this_row = $(this);
                var chkSelect = this_row.find('input[type=checkbox]')[0];
                if (chkSelect !== null & chkSelect !== undefined) {
                    if (chkSelect.checked == true) {
                        chkCount += 1;
                    }
                }
            });
            if (chkCount > 0) {
                return true;
            }
            else {
                var title = "Delete Daily Checklist Master";
                var body = "Cannot delete. No record selected for deletion.";
                $("#MessagePopup .modal-title").html(title);
                $("#MessagePopup .modal-body").html(body);
                $("#MessagePopup").modal("show");
                return false;
            }
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            //debugger;
            //var winHeight = $(window).height();
            //console.log(winHeight);
            ////winHeight = screen.availHeight;
            //$("#gridviewdiv").css('height', winHeight-140);
            $(".form-control").change(function () {
                $(this).closest('tr').find('input[type=hidden]').val("Update");
            });

            $("[id$=btnDelete]").click(function () {
                var chkCount = 0;
                $("#GridChecklistMaster tr").not(':first').each(function () {
                    var this_row = $(this);
                    var chkSelect = this_row.find('input[type=checkbox]')[0];
                    if (chkSelect !== null & chkSelect !== undefined) {
                        if (chkSelect.checked == true) {
                            chkCount += 1;
                        }
                    }
                });
                if (chkCount > 0) {
                    return true;
                }
                else {
                    var title = "Delete Daily Checklist Master";
                    var body = "Cannot delete. No record selected for deletion.";
                    $("#MessagePopup .modal-title").html(title);
                    $("#MessagePopup .modal-body").html(body);
                    $("#MessagePopup").modal("show");
                    return false;
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="FooterContentArea" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
