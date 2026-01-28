<%@ Page Title="Inspection Master" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" Culture="auto" UICulture="auto" CodeBehind="InspectionMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.GEA.InspectionMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
            width: auto;
        }

        .Row {
            display: table;
            border-spacing: 5pt;
            width: 100%;
        }

        .Col {
            display: table-cell;
            height: 50px;
            width: 100%;
            border: 1pt solid black;
            background-color: #DBDBDB;
        }

        .MiddleLeft {
            text-align: left;
            align-items: normal;
            vertical-align: middle;
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
                            <td class="commanTd" style="width: 124px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","ComponentID") %></td>

                            <td style="width: 280px;">
                                <asp:TextBox runat="server" ID="txtComponent" placeholder="Component Search" Height="35" />
                                <asp:LinkButton ID="btnComponent" runat="server" CssClass="btn btn-primary" OnClick="btnComponent_Click">
														<span aria-hidden="true" class="glyphicon glyphicon-refresh"></span>
                                </asp:LinkButton>
                            </td>
                          <%--  <td class="commanTd" style="width: 124px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","ComponentID") %></td>--%>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlCompID" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCompID_SelectedIndexChanged" />
                            </td>

                            <td class="commanTd" style="width: 124px; vertical-align: middle;"><%=GetGlobalResourceObject("CommanResource","OperationNo") %></td>
                            <td style="width: 180px;">
                                <asp:DropDownList runat="server" ID="ddlOperationNum" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" OnSelectedIndexChanged="ddlOperationNum_SelectedIndexChanged" AutoPostBack="true" />
                            </td>

                            <td class="commanTd" style="width: 150px; vertical-align: middle;">Ins. Plan Number : </td>
                            <td style="width: 180px;">
                                <asp:DropDownList runat="server" ID="ddlInsPlanNumber" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" />
                                <%--<ajaxToolkit:ComboBox runat="server" ID="ddlInsPlanNumber" ClientIDMode="Static" CssClass="form-control" Style="margin-left: 5px; margin-bottom: 2px;" DropDownStyle="DropDown" />--%>
                            </td>

                            <td style="width: 120px;">
                                <asp:Button runat="server" CssClass="btn btn-primary btnStyle" ID="btnView" Text="View" Style="min-width: 120px;" OnClick="btnView_Click" />
                            </td>
                            <td>
                                <button type="button" id="btnDownloadTemplate" class="btn btn-success btnStyle" runat="server" clientidmode="static">
                                    <span class="glyphicon glyphicon-download"></span>&nbsp;Import Template
                                </button>
                            </td>
                             <td style="width: 150px;color: white;text-align: center;vertical-align: inherit;">
                                <asp:Label runat="server" Font-Bold="true" Style="display: inline; margin-left: 5px;" ID="lblIQCName"/>
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="overflow-x: auto; overflow-y: auto; height: 70vh">
                    <asp:GridView ID="GridInspectionMaster" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered headerFixerhere" RowStyle-BackColor="#BACADE" AlternatingRowStyle-BackColor="#DCE7F5" RowStyle-Font-Size="Medium" RowStyle-ForeColor="#383838" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true" OnRowDataBound="GridInspectionMaster_RowDataBound">
                        <AlternatingRowStyle BackColor="#DCE7F5" />
                        <EmptyDataTemplate>
                            No data available for selected component, operation and inspection plan number.
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
                                    <asp:Label ID="lblIDD" runat="server" Text='<%#Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ins. Parameter ID" SortExpression="InsParamCode" ControlStyle-Width="180">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtInsParamID" runat="server" CssClass="form-control" Text='<%#Bind("InsParamID") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ins. Parameter" SortExpression="InsParameter" ControlStyle-Width="150">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtInsParameter" runat="server" CssClass="form-control" Text='<%#Bind("InsParameter") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="LSL" SortExpression="LSL" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLSL" runat="server" CssClass="form-control" TextMode="Number" step="any" Text='<%#Bind("LSL") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="USL" SortExpression="USL" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUSL" runat="server" CssClass="form-control" TextMode="Number" step="any" Text='<%#Bind("USL") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="UOM" SortExpression="UOM" ControlStyle-Width="100">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control" Text='<%#Bind("UOM") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Data Template" SortExpression="DataTemplate" ControlStyle-Width="130">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hdnDataTemplate" ClientIDMode="Static" Value='<%#Bind("DataTemplate") %>' />
                                    <asp:DropDownList ID="ddlDataTemplate" runat="server"  CssClass="form-control">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>1 Text Value</asp:ListItem>
                                        <asp:ListItem>2 Text Value</asp:ListItem>
                                        <asp:ListItem>1 Numeric Value</asp:ListItem>
                                        <asp:ListItem>2 Numeric Value</asp:ListItem>
                                        <asp:ListItem>Ok/NotOk</asp:ListItem>
                                        <asp:ListItem>Yes/No</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Applies To Operator">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAppliesToOperator" runat="server" Checked='<%#Bind("AppliesToOperator") %>' CssClass="form-control" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Mandatory For Operator">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryForOperator" runat="server" Checked='<%#Bind("MandatoryForOperator") %>' CssClass="form-control" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Applies To Quality">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAppliesToQuality" runat="server" Checked='<%#Bind("AppliesToQuality") %>' CssClass="form-control" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Mandatory For Quality">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMandatoryForQuality" runat="server" Checked='<%#Bind("MandatoryForQuality") %>' CssClass="form-control" />
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
                    <asp:FileUpload data-toggle="tooltip" ForeColor="Black" CssClass="form-control btnStyle" ID="FileUploadInsMaster" runat="server" ClientIDMode="Static" AllowMultiple="false" Style="display: inline; width: 220px;" />
                    <button type="button" id="btnImport" class="btn btn-primary btnStyle" style="display: inline;">Import</button>
                    <button type="button" id="btnCopy" class="btn btn-primary btnStyle" style="display: inline;">Copy Inspection Plan</button>
                    <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-primary btnStyle" OnClick="btnNew_Click" ClientIDMode="Static" Style="display: inline;" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary btnStyle" OnClick="btnCancel_Click" ClientIDMode="Static" Style="display: inline;" />
                    <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary btnStyle" OnClick="btnDelete_Click" ClientIDMode="Static" Style="display: inline;" />
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary btnStyle" OnClick="btnSave_Click" ClientIDMode="Static" Style="display: inline;" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
             <div id="InspectionPlanmodel" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Inspection Plan Number Entry</h4>
                </div>
                <div class="modal-body">
                    <asp:TextBox runat="server" ID="txtinspectionPlanNumber" CssClass="form-control" />
                </div>
                <div class="modal-footer">
                    <button type="button" runat="server" class="btn btn-success" data-dismiss="modal" onserverclick="btnSave2_Click" id="btnsavedata" value="submit">Save</button>
                    <%--<asp:Button runat="server" ID="btnsave2" OnClick="btnSave_ClickAsync" CssClass="btnStyle btn-success" data-dismiss="modal" Text="Save"/>--%>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
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

    <!--Inspection Master Import Popup -->
    <div id="InsMasterImportPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Selected Filename : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                            <label id="lblFilename" style="display: inline; margin-left: 5px; color: green;" />
                        </div>
                    </div>
                    <h6>Are you sure you want to import the selected data ?</h6>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnImportData" Text="Import" CssClass="btn btn-primary" OnClick="btnImportData_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Copy Inspection Plan Popup -->
    <div id="CopyInspectionPlanPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="Current Inspection Plan : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                            <label id="lblCurrInspectionPlan" style="display: inline; margin-left: 5px; color: green;" />
                        </div>
                    </div>

                    <div class="Row">
                        <div class="Col MiddleLeft">
                            <asp:Label runat="server" Text="New Inspection Plan : " Font-Bold="true" Style="display: inline; margin-left: 5px;" />
                            <asp:TextBox runat="server" ID="txtNewInspectionPlan" Style="display: inline; margin-left: 5px; color: green;" CssClass="form-control" Width="180" ClientIDMode="Static" />
                        </div>
                    </div>
                    <h6>Are you sure you copy all the data with new inspection plan ?</h6>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnCopyInspectionPlan" Text="Yes, Copy" CssClass="btn btn-primary" ClientIDMode="Static" OnClick="btnCopyInspectionPlan_Click"></asp:Button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(".form-control").change(function () {
            $(this).closest('tr').find('input[type=hidden]').val("Update");
        });
        $("#btnDownloadTemplate").click(function () {
            debugger;
            if (document.URL) {
                let Url = document.URL;
                var downloadUrl = false;
                downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/InspectionMasterImportTemplate.xlsx';
                if (downloadUrl) {
                    location.href = downloadUrl;
                }
                else {
                    alert('Import Template not found.');
                }
            }
        });
        $("#btnSave").click(function () {

            if ($("#ddlInsPlanNumber").val() == "" || $("#ddlInsPlanNumber").val() == undefined) {
                $("#InspectionPlanmodel").modal("show");
                return false;
            }
            else
                return true;
        })
        $("#btnsavedata").click(function () {
            debugger;
            $("#InspectionPlanmodel").modal("hide");
        })
        $("#btnImport").click(function () {
            var dd = $("#FileUploadInsMaster")[0].files.length;
            if (dd == 0) {
                var title = "Import Inspection Master";
                var body = "Cannot import as no file is chosen for import.";
                $("#MessagePopup .modal-title").html(title);
                $("#MessagePopup .modal-body").html(body);
                $("#MessagePopup").modal("show");
                return false;
            }
            else {
                var filename = $("#FileUploadInsMaster")[0].files[0].name;
                if (filename !== null && filename !== undefined) {
                    var file_extension = filename.split('.').pop();
                    if (file_extension !== null && file_extension !== undefined) {
                        if (file_extension !== "xlsx") {
                            var title = "Import Inspection Master";
                            var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                            $("#MessagePopup .modal-title").html(title);
                            $("#MessagePopup .modal-body").html(body);
                            $("#MessagePopup").modal("show");
                            return false;
                        }
                        else {
                            var title = "Import Inspection Master";
                            $("#lblFilename").text(filename);
                            $("#InsMasterImportPopup .modal-title").html(title);
                            $("#InsMasterImportPopup").modal("show");
                            return true;
                        }
                    }
                    else {
                        var title = "Import Inspection Master";
                        var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                        $("#MessagePopup .modal-title").html(title);
                        $("#MessagePopup .modal-body").html(body);
                        $("#MessagePopup").modal("show");
                        return false;
                    }
                }
                else {
                    var title = "Import Inspection Master";
                    var body = "Cannot import as no file is chosen for import.";
                    $("#MessagePopup .modal-title").html(title);
                    $("#MessagePopup .modal-body").html(body);
                    $("#MessagePopup").modal("show");
                    return false;
                }
            }
        });

        $("#btnCopy").click(function () {
            var insPlan = $("#ddlInsPlanNumber").val();
            if (insPlan !== null && insPlan !== undefined || insPlan !== "") {
                $("#lblCurrInspectionPlan").text($("#ddlInsPlanNumber").val());
                var title = "Copy Inspection Plan";
                $("#CopyInspectionPlanPopup .modal-title").html(title);
                $("#CopyInspectionPlanPopup").modal("show");
            }
            else {
                var title = "Copy Inspection Plan";
                var body = "Cannot copy inspection plan as no selected inspection plan found.";
                $("#MessagePopup .modal-title").html(title);
                $("#MessagePopup .modal-body").html(body);
                $("#MessagePopup").modal("show");
            }
        });

        $("#btnCopyInspectionPlan").click(function () {
            var newInsPlan = $("#txtNewInspectionPlan").val();
            if (newInsPlan !== null && newInsPlan !== undefined || newInsPlan !== "")
                return true;
            else {
                alert("Please enter a new inspection plan to copy.");
                return false;
            }
        });

        $("#btnDelete").click(function () {
            var chkCount = 0;
            $("#MainContent_GridInspectionMaster tr").not(':first').each(function () {
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
                var title = "Delete Inspection Master";
                var body = "Cannot delete. No record selected for deletion.";
                $("#MessagePopup .modal-title").html(title);
                $("#MessagePopup .modal-body").html(body);
                $("#MessagePopup").modal("show");
                return false;
            }
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#btnSave").click(function () {
                debugger;
                if ($("#ddlInsPlanNumber").val() == "" || $("#ddlInsPlanNumber").val() == undefined) {
                    $("#InspectionPlanmodel").modal("show");
                    return false;
                }
                else
                    return true;
            })
            $("#btnsavedata").click(function () {
                $("#InspectionPlanmodel").modal("hide");
            })
            $("#btnDownloadTemplate").click(function () {
                debugger;
                if (document.URL) {
                    let Url = document.URL;
                    var downloadUrl = false;
                    downloadUrl = Url.slice(0, Url.lastIndexOf('/')) + '/InspectionMasterImportTemplate.xlsx';
                    if (downloadUrl) {
                        location.href = downloadUrl;
                    }
                    else {
                        alert('Import Template not found.');
                    }
                }
            });
            $(".form-control").change(function () {
                $(this).closest('tr').find('input[type=hidden]').val("Update");
            });

            $("#btnImport").click(function () {
                var dd = $("#FileUploadInsMaster")[0].files.length;
                if (dd == 0) {
                    var title = "Import Inspection Master";
                    var body = "Cannot import as no file is chosen for import.";
                    $("#MessagePopup .modal-title").html(title);
                    $("#MessagePopup .modal-body").html(body);
                    $("#MessagePopup").modal("show");
                    return false;
                }
                else {
                    var filename = $("#FileUploadInsMaster")[0].files[0].name;
                    if (filename !== null && filename !== undefined) {
                        var file_extension = filename.split('.').pop();
                        if (file_extension !== null && file_extension !== undefined) {
                            if (file_extension !== "xlsx") {
                                var title = "Import Inspection Master";
                                var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                                $("#MessagePopup .modal-title").html(title);
                                $("#MessagePopup .modal-body").html(body);
                                $("#MessagePopup").modal("show");
                                return false;
                            }
                            else {
                                var title = "Import Inspection Master";
                                $("#lblFilename").text(filename);
                                $("#InsMasterImportPopup .modal-title").html(title);
                                $("#InsMasterImportPopup").modal("show");
                                return true;
                            }
                        }
                        else {
                            var title = "Import Inspection Master";
                            var body = "Wrong file format. File to be imported must be a xlsx excel file.";
                            $("#MessagePopup .modal-title").html(title);
                            $("#MessagePopup .modal-body").html(body);
                            $("#MessagePopup").modal("show");
                            return false;
                        }
                    }
                    else {
                        var title = "Import Inspection Master";
                        var body = "Cannot import as no file is chosen for import.";
                        $("#MessagePopup .modal-title").html(title);
                        $("#MessagePopup .modal-body").html(body);
                        $("#MessagePopup").modal("show");
                        return false;
                    }
                }
            });

            $("#btnCopy").click(function () {
                var insPlan = $("#ddlInsPlanNumber").val();
                if (insPlan !== null && insPlan !== undefined || insPlan !== "") {
                    $("#lblCurrInspectionPlan").text($("#ddlInsPlanNumber").val());
                    var title = "Copy Inspection Plan";
                    $("#CopyInspectionPlanPopup .modal-title").html(title);
                    $("#CopyInspectionPlanPopup").modal("show");
                }
                else {
                    var title = "Copy Inspection Plan";
                    var body = "Cannot copy inspection plan as no selected inspection plan found.";
                    $("#MessagePopup .modal-title").html(title);
                    $("#MessagePopup .modal-body").html(body);
                    $("#MessagePopup").modal("show");
                }
            });

            $("#btnCopyInspectionPlan").click(function () {
                var newInsPlan = $("#txtNewInspectionPlan").val();
                if (newInsPlan !== null && newInsPlan !== undefined || newInsPlan !== "")
                    return true;
                else {
                    alert("Please enter a new inspection plan to copy.");
                    return false;
                }
            });

            $("#btnDelete").click(function () {
                var chkCount = 0;
                $("#MainContent_GridInspectionMaster tr").not(':first').each(function () {
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
                    var title = "Delete Inspection Master";
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
