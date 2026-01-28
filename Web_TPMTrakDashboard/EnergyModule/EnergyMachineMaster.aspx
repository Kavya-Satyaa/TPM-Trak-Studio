<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnergyMachineMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.EnergyModule.EnergyMachineMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .document {
            width: 400px;
            margin-top: 150px;
            background-color: beige;
        }

        .headerstyle {
            font-size: 20px;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bolder;
        }

        .style {
            color: white;
            background-color: #2E6886;
        }

        .Rowstyle {
            color: black;
            background-color: white;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="height:85vh;overflow:auto;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gridMachineInformation" ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" CssClass="table table-bordered headerFixer cockpit headerFixerTable">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="linkEdit" CommandName="Edit" CommandArgument='<%# Bind("MachineID") %>' ClientIDMode="Static" Text="Edit" CssClass="linkEdit" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Machine-ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMachineID" ClientIDMode="Static" Text='<%# Eval("MachineID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Plant-ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPlantID" ClientIDMode="Static" Text='<%# Eval("PlantID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cell ID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCellID" ClientIDMode="Static" Text='<%# Eval("GroupID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Machine-InterfaceID">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMachineInterfaceID" ClientIDMode="Static" Text='<%# Eval("MachineInterfaceID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IP-Address" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblIPAddress" ClientIDMode="Static" Text='<%# Eval("IPAddress")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Port Number" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPortNumber" ClientIDMode="Static" Text='<%# Eval("PortNumber")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Machine Type">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMachineType" ClientIDMode="Static" Text='<%# Eval("MachineType")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Is-Enabled">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsEnabled" ClientIDMode="Static" Checked='<%# Eval("IsEnabled") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SortOrder">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSortOrder" ClientIDMode="Static" Text='<%# Eval("SortOrder")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsDashboardEnabled">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsDashboardEnabled" ClientIDMode="Static" Checked='<%# Eval("IsDashboardEnabled") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DELETE">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDelete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Machine-ID" DataField="MachineID" />
                                    <asp:BoundField HeaderText="Plant-ID" DataField="PlantID" />
                                    <asp:BoundField HeaderText="Machine-InterfaceID" DataField="MachineInterfaceID" />
                                    <asp:BoundField HeaderText="IP-Address" DataField="IPAddress" Visible="false" />
                                    <asp:BoundField HeaderText="Port Number" DataField="PortNumber" Visible="false" />
                                    <asp:BoundField HeaderText="Machine Type" DataField="MachineType" />
                                    <asp:CheckBoxField HeaderText="Is-Enabled" DataField="IsEnabled" />
                                    <asp:BoundField HeaderText="SortOrder" DataField="SortOrder" />
                                    <asp:CheckBoxField HeaderText="IsDashboardEnabled" DataField="IsDashboardEnabled" />
                                    <asp:TemplateField HeaderText="DELETE">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDelete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Cell ID" DataField="GroupID" />--%>
                                </Columns>
                                <HeaderStyle CssClass="style" />
                                <RowStyle CssClass="Rowstyle" />
                            </asp:GridView>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div>
                    <div class="modal" role="dialog" id="addmachineModal">
                        <div class="modal-dialog document" role="document">
                            <div class="modal-content ">
                                <div class="modal-header headerstyle">
                                    <button type="button" class="close closes" data-dismiss="modal">&times;</button>
                                    <h5 class="modal-title"><b>Add Machine</b></h5>
                                </div>
                                <div class="modal-body">
                                    <div>
                                        <asp:TextBox runat="server" ID="txtAddMachineID" placeholder="Machine-ID" CssClass="form-control" ClientIDMode="Static" /><br />
                                        <asp:DropDownList runat="server" ID="ddlPlantID" CssClass="form-control" AutoPostBack="true" />
                                        <br />
                                       <%-- <asp:DropDownList runat="server" ID="ddlCellID" CssClass="form-control" />
                                        <br />--%>
                                        <asp:TextBox runat="server" ID="txtAddmachineInterface" placeholder="Machine-Interface" TextMode="Number" min="1" CssClass="form-control" ClientIDMode="Static" /><br />
                                        <asp:TextBox runat="server" ID="txtAddIPaddress" placeholder="IP-Address" CssClass="form-control" ClientIDMode="Static" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57) ||  event.charCode <= 46)" Visible="false" />
                                        <asp:TextBox runat="server" ID="txtAddPortNumber" placeholder="Port-Number" type="text" MaxLength="4" CssClass="form-control" ClientIDMode="Static" onkeypress="return event.charCode >= 48 && event.charCode <= 57" Visible="false" />
                                        <div style="margin: 5px;">
                                            <asp:CheckBox runat="server" CssClass="form-control" Width="40" ID="chkEnabled" ClientIDMode="Static" />
                                            <span>Enabled</span><br />
                                        </div>
                                        <asp:TextBox runat="server" ID="txtAddSortOrder" placeholder="SortOrder" type="text" MaxLength="4" CssClass="form-control" ClientIDMode="Static" onkeypress="return event.charCode >= 48 && event.charCode <= 57" /><br />
                                        <asp:DropDownList runat="server" ID="ddlAddMachineType" CssClass="form-control" ClientIDMode="Static">
                                            <asp:ListItem Text="" Value="" />
                                            <asp:ListItem Text="Machine EM" Value="Machine EM" />
                                            <asp:ListItem Text="Non-Machine EM" Value="Non-Machine EM" />
                                        </asp:DropDownList><br />
                                        <div style="margin: 5px;">
                                            <asp:CheckBox runat="server" CssClass="form-control" Width="40" ID="chkIsDashboardEnabled" ClientIDMode="Static" />
                                            <span>DashboardEnabled</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" runat="server" class="btn btn-primary btnInsertData" id="btnInsertData" onserverclick="InsertData">Save changes</button>
                                    <asp:Button runat="server" CssClass="btn btn-secondary closes" ID="btnAddmachineClose" Text="Close" OnClick="btnAddmachineClose_Click" />
                                    <%--<button type="button" class="btn btn-secondary closes" id="btnAddmachineClose">Close</button>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div>
                    <div class="modal" role="dialog" id="ErrorModal">
                        <div class="modal-dialog document" role="document" style="background-color: transparent;">
                            <div class="modal-content ">
                                <div class="modal-header headerstyle">
                                    <button type="button" class="close clear" data-dismiss="modal">&times;</button>
                                    <h5 class="modal-title"><b><span id="ErrorHeader"></span></b></h5>
                                </div>
                                <div class="modal-body">
                                    <div>
                                        <span id="ErrorBody"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                    <div class="modal" role="dialog" id="InfoModal">
                        <div class="modal-dialog document" role="document" style="background-color: transparent;">
                            <div class="modal-content ">
                                <div class="modal-header headerstyle">
                                    <button type="button" class="close clear " data-dismiss="modal">&times;</button>
                                    <h5 class="modal-title"><b><span id="InfoHeader"></span></b></h5>
                                </div>
                                <div class="modal-body">
                                    <div>
                                        <span id="InfoBody"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="margin-top:10px;">
                    <button type="button" id="btnAdd" class="btn btn-primary">Add Machine</button>
                    <asp:Button runat="server" ID="btnDelete" Text="Delete" class="btn btn-primary" OnClick="btnDelete_Click"></asp:Button>
                </div>
                <div>
                    <div class="modal" role="dialog" id="UpdatemachineModal">
                        <div class="modal-dialog document" role="document">
                            <div class="modal-content ">
                                <div class="modal-header headerstyle">
                                    <button type="button" class="close updatecloses" data-dismiss="modal">&times;</button>
                                    <h5 class="modal-title"><b>Update Machine</b></h5>
                                </div>
                                <div class="modal-body">
                                    <div>
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="lblUpdateMachine" placeholder="Machine-ID" Enabled="false" CssClass="form-control textboxcommon" /><br />
                                        <asp:TextBox runat="server" ID="lblUpdatePlantID" CssClass="form-control textboxcommon" Enabled="false" ClientIDMode="Static" />
                                        <br />
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="lblUpdateInterfaceID" placeholder="Machine-Interface" Enabled="false" TextMode="Number" min="1" CssClass="form-control textboxcommon" />
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtUpdateIPAddress" Visible="false" placeholder="IP-Address" CssClass="form-control textboxcommon" onkeypress="return ((event.charCode >= 48 && event.charCode <= 57) ||  event.charCode <= 46)" />
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtUpdatePortNumber" Visible="false" placeholder="Port-Number" type="text" MaxLength="4" CssClass="form-control textboxcommon" onkeypress="return event.charCode >= 48 && event.charCode <= 57" /><br />
                                        <div style="margin: 5px;">
                                            <asp:CheckBox runat="server" CssClass="form-control textboxcommon" Width="40" ID="chkUpdateEnabled" />
                                            <span>Enabled</span><br />
                                        </div>
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtUpdateSortorder" placeholder="SortOrder" type="text" MaxLength="4" CssClass="form-control textboxcommon" onkeypress="return event.charCode >= 48 && event.charCode <= 57" /><br />
                                        <asp:DropDownList runat="server" ID="ddlUpdateMachineType" CssClass="form-control" ClientIDMode="Static">
                                            <asp:ListItem Text="" Value="" />
                                            <asp:ListItem Text="Machine EM" Value="Machine EM" />
                                            <asp:ListItem Text="Non-Machine EM" Value="Non-Machine EM" />
                                        </asp:DropDownList>
                                        <div style="margin: 5px;">
                                            <asp:CheckBox runat="server" CssClass="form-control textboxcommon" Width="40" ID="chkUpdateDashboardEnabled" ClientIDMode="Static" />
                                            <span>DashboardEnabled</span><br />
                                        </div>
                                        <br />
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" runat="server" class="btn btn-primary" id="btnUpdateData" onserverclick="UpdateData">Save changes</button>
                                    <button type="button" class="btn btn-secondary updatecloses">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <script>
        $(document).ready(function () {
            $("#btnAdd").click(function () {
                $("#addmachineModal").show();
            });
            $(".btnInsertData").click(function () {
                debugger;
                $("#addmachineModal").hide();
                // var status = ValidateData($("#txtAddMachineID").val(), $("#txtAddmachineInterface").val(), $("#txtAddIPaddress").val(), $("#txtAddPortNumber").val(), $("#txtAddSortOrder").val());
                //if (status == "true") {
                return true;
                //}
            });
            $("#btnUpdateData").click(function () {
                $("#addmachineModal").hide();
                // var status = ValidateData($("#txtUpdateMachineID").val(), $("#txtUpdatemachineInterface").val(), $("#txtUpdateIPaddress").val(), $("#txtUpdatePortNumber").val(), $("#txtUpdateSortorder").val());
                //if (status == "true") {
                return true;
                //}

            });
            $(".linkEdit").click(function () {
                var this_row = $(this).closest("tr");
                if (this_row) {
                    debugger;
                    //var MachineID = this_row.find('td:eq(1)').eq(0).text();
                    //var InterfaceID = this_row.find('td:eq(3)').eq(0).text();
                    //var IPAddress = this_row.find('td:eq(4)').eq(0).text();
                    //var PortNumber = this_row.find('td:eq(5)').eq(0).text();
                    //var SortOrder = this_row.find('td:eq(6)').eq(0).text();
                    //var PlantID = this_row.find('td:eq(2)').eq(0).text();
                    //var MachineType = this_row.find('td:eq(4)').eq(0).text();
                    var MachineID = this_row.find('#lblMachineID').text();
                    var InterfaceID = this_row.find('#lblMachineInterfaceID').text();
                    var IPAddress = this_row.find('#lblIPAddress').text();
                    var PortNumber = this_row.find('#lblPortNumber').text();
                    var SortOrder = this_row.find('#lblSortOrder').text();
                    var PlantID = this_row.find('#lblPlantID').text();
                    var MachineType = this_row.find('#lblMachineType').text(); lblMachineType
                    $("#lblUpdateMachine").val(MachineID);
                    $("#lblUpdateInterfaceID").val(InterfaceID);
                    $("#txtUpdateIPAddress").val(IPAddress);
                    $("#txtUpdatePortNumber").val(PortNumber);
                    $("#txtUpdateSortorder").val(SortOrder);
                    $("#lblUpdatePlantID").val(PlantID);
                    $("#ddlUpdateMachineType").val(MachineType);
                }
                var IsEnabledcheckbox = document.getElementById('MainContent_chkUpdateEnabled');
                IsEnabledcheckbox.checked = $(this).parent().parent().find('#chkIsEnabled').prop('checked');
                var DashboardEnablecheckbox = document.getElementById('chkUpdateDashboardEnabled');
                DashboardEnablecheckbox.checked = $(this).parent().parent().find('#chkIsDashboardEnabled').prop('checked');
                //document.getElementById('MainContent_chkUpdateEnabled').checked = $(this).parent().parent().children().eq(5).children().children()[0].checked;
                //document.getElementById('chkUpdateDashboardEnabled').checked = $(this).parent().parent().children().eq(7).children().children()[0].checked;
                $("#UpdatemachineModal").show();
            });
            $(".closes").click(function () {
                $("#addmachineModal").hide();
            });
            $(".clear").click(function () {
                $("#ErrorModal").hide();
                $("#InfoModal").hide();
                $("#UpdatemachineModal").hide();
                $("#addmachineModal").hide();
            });
            $(".updatecloses").click(function () {
                $("#UpdatemachineModal").hide();
            });
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#btnAdd").click(function () {
                $("#addmachineModal").show();
            });
            $(".linkEdit").click(function () {
                var this_row = $(this).closest("tr");
                if (this_row) {
                    debugger;
                    //var MachineID = this_row.find('td:eq(1)').eq(0).text();
                    //var InterfaceID = this_row.find('td:eq(3)').eq(0).text();
                    //var IPAddress = this_row.find('td:eq(4)').eq(0).text();
                    //var PortNumber = this_row.find('td:eq(5)').eq(0).text();
                    //var SortOrder = this_row.find('td:eq(6)').eq(0).text();
                    //var PlantID = this_row.find('td:eq(2)').eq(0).text();
                    //var MachineType = this_row.find('td:eq(4)').eq(0).text();
                    var MachineID = this_row.find('#lblMachineID').text();
                    var InterfaceID = this_row.find('#lblMachineInterfaceID').text();
                    var IPAddress = this_row.find('#lblIPAddress').text();
                    var PortNumber = this_row.find('#lblPortNumber').text();
                    var SortOrder = this_row.find('#lblSortOrder').text();
                    var PlantID = this_row.find('#lblPlantID').text();
                    var MachineType = this_row.find('#lblMachineType').text(); lblMachineType
                    $("#lblUpdateMachine").val(MachineID);
                    $("#lblUpdateInterfaceID").val(InterfaceID);
                    $("#txtUpdateIPAddress").val(IPAddress);
                    $("#txtUpdatePortNumber").val(PortNumber);
                    $("#txtUpdateSortOrder").val(SortOrder);
                    $("#lblUpdatePlantID").val(PlantID);
                    $("#ddlUpdateMachineType").val(MachineType);
                }
                var IsEnabledcheckbox = document.getElementById('MainContent_chkUpdateEnabled');
                IsEnabledcheckbox.checked = $(this).parent().parent().find('#chkIsEnabled').prop('checked');
                var DashboardEnablecheckbox = document.getElementById('chkUpdateDashboardEnabled');
                DashboardEnablecheckbox.checked = $(this).parent().parent().find('#chkIsDashboardEnabled').prop('checked');
                //document.getElementById('MainContent_chkUpdateEnabled').checked = $(this).parent().parent().children().eq(5).children().children()[0].checked;
                //document.getElementById('chkUpdateDashboardEnabled').checked = $(this).parent().parent().children().eq(7).children().children()[0].checked;
                $("#UpdatemachineModal").show();
            });
            $(".closes").click(function () {
                $("#addmachineModal").hide();
            });
            $(".clear").click(function () {
                $("#ErrorModal").hide();
                $("#InfoModal").hide();
                $("#UpdatemachineModal").hide();
                $("#addmachineModal").hide();
            });
            $(".updatecloses").click(function () {
                $("#UpdatemachineModal").hide();
            });
            $(".btnInsertData").click(function () {
                $("#addmachineModal").hide();
                //var status = ValidateData($("#txtAddMachineID").val(), $("#txtAddmachineInterface").val(), $("#txtAddIPaddress").val(), $("#txtAddPortNumber").val(), $("#txtAddSortOrder").val());
                //if (status == "true") {
                return true;
                //}

            });
            $("#btnUpdateData").click(function () {
                $("#addmachineModal").hide();
                // var status = ValidateData($("#txtUpdateMachineID").val(), $("#txtUpdatemachineInterface").val(), $("#txtUpdateIPaddress").val(), $("#txtUpdatePortNumber").val(), $("#txtUpdateSortorder").val());
                //if (status == "true") {
                return true;
                //}
            });
        });
        function ValidateData(MachineID, MachineInterface, IPAddress, PortNumber, SortOrder) {
            debugger;
            status = true;
            if (MachineID == "" || MachineID == undefined) {

                ErrorMessage('Please Enter Machine-ID,Error');
                status = false;
            }
            else if (MachineInterface == "" || MachineInterface == undefined) {
                ErrorMessage("Please Enter Machine Interface,Error");
                status = false;
            }
            else if (IPAddress == "" || IPAddress == undefined) {
                ErrorMessage("Please Enter IPaddress,Error");
                status = false;
            }
            else if (PortNumber == "" || PortNumber == undefined) {
                ErrorMessage("Please Enter Port Number,Error");
                status = false;
            }
            else if (SortOrder == "" || SortOrder == undefined) {
                ErrorMessage("Please Enter Sort Order,Error");
                status = false;
            }

            return status;
        }
        function ErrorMessage(msg) {
            debugger;
            $('[id*=ErrorModal]').modal('show');
            var arr = msg.split(',');
            $("#ErrorHeader").text(arr[1]);
            $("#ErrorBody").text(arr[0]);
        }
        function InfoMessage(msg) {
            $('[id*=InfoModal]').modal('show');
            var arr = msg.split(',');
            $("#InfoHeader").text(arr[1]);
            $("#InfoBody").text(arr[0]);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
