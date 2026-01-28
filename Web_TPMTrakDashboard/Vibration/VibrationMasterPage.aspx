<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VibrationMasterPage.aspx.cs" Inherits="Web_TPMTrakDashboard.VibrationMasterPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        th {
            text-align: center;
            background-color: #2E6886;
            color: white;
            height: 40px;
        }

        table {
            width: 100%;
        }

        td {
            width: 100%;
            height: 30px;
            padding: 5px 10px;
        }

        tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .button {
            background-color: #4CAF50; /* Green */
            border: none;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            width: 80px;
            height: 30px;
        }
    </style>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table class="table table-bordered" id="tblmenu" style="width: 60%">
                    <tr>
                        <td style="vertical-align: middle">Machine
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlMachine" Style="width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged" CssClass="form-control" />
                        </td>
                        <td style="vertical-align: middle">Component
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlComponent" Style="width: 200px;" CssClass="form-control" />
                        </td>
                        <td style="vertical-align: middle" class="paramVisible">Parameter
                        </td>
                        <td class="paramVisible">
                            <asp:DropDownList runat="server" ID="ddlParameterId" Style="width: 200px;" CssClass="form-control">
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                <asp:ListItem Text="Signal Energy" Value="SignalEnergy"></asp:ListItem>
                                <asp:ListItem Text="Temperature" Value="Temperature"></asp:ListItem>
                                <asp:ListItem Text="Noise" Value="Noise"></asp:ListItem>
                                <asp:ListItem Text="Velocity-X" Value="Velocity-X"></asp:ListItem>
                                <asp:ListItem Text="Velocity-Y" Value="Velocity-Y"></asp:ListItem>
                                <asp:ListItem Text="Velocity-Z" Value="Velocity-Z"></asp:ListItem>
                                <asp:ListItem Text="Acceleration-X" Value="Acceleration-X"></asp:ListItem>
                                <asp:ListItem Text="Acceleration-Y" Value="Acceleration-Y"></asp:ListItem>
                                <asp:ListItem Text="Acceleration-Z" Value="Acceleration-Z"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" Text="View" CssClass="button" />
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
                <asp:PostBackTrigger ControlID="btnDelete" />
                <asp:PostBackTrigger ControlID="lstsettings" />
                <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click" />
            </Triggers>
            <ContentTemplate>

                <div style="width: 100%; height: 77vh">

                    <asp:ListView runat="server" ID="lstsettings" ClientIDMode="Static">
                        <LayoutTemplate>
                            <table id="tbllst">
                                <tr>
                                    <th style="width: 500px;">
                                        <span>Machine ID</span>
                                    </th>
                                    <th style="width: 500px;">
                                        <span>Component ID</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Operation</span>
                                    </th>
                                    <th style="width: 500px" class="paramVisible">
                                        <span>Parameter</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Warning USL</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Danger USL</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>M Value </span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>N Value</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Delete Selection</span>
                                    </th>
                                </tr>
                                <tr id="ItemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr style="width: 100px; text-align: center;">
                                <td style="width: 500px;">
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%# Bind("MachineID") %>' Visible='<%# Bind("MachineIDlblVisible") %>' />
                                    <asp:DropDownList runat="server" ID="ddlMachineID" DataSource='<%# Bind("MachineIDList") %>' Style="width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged" Visible='<%# Bind("MachineIDddlVisible") %>' CssClass="form-control" />
                                </td>
                                <td style="width: 500px;">
                                    <asp:Label runat="server" ID="lblcmp" Text='<%# Bind("CompID") %>' Visible='<%# Bind("cmbIDlblVisible") %>' />
                                    <asp:DropDownList runat="server" ID="ddlComponentID" Style="width: 200px;" Visible="false" CssClass="form-control" />
                                </td>
                                <td style="width: 500px;">
                                    <asp:Label runat="server" ID="lblOp" Text='<%# Bind("OperationID") %>' Visible='<%# Bind("lblOpVisible") %>' />
                                    <asp:TextBox runat="server" ID="txtid" Visible="false" CssClass="form-control" />
                                </td>
                                <td style="width: 500px;" class="paramVisible">
                                    <asp:HiddenField runat="server" ID="hdnParameterValue" Value='<%# Bind("ParameterValue") %>' />
                                    <asp:Label runat="server" ID="lblParameter" Text='<%# Bind("Parameter") %>' Visible='<%# Bind("ParameterlblVisible") %>' />
                                    <asp:DropDownList runat="server" ID="ddlParameter" Style="width: 200px;" Visible='<%# Bind("ParameterddlVisible") %>' CssClass="form-control">
                                        <asp:ListItem Text="Signal Energy" Value="SignalEnergy"></asp:ListItem>
                                        <asp:ListItem Text="Temperature" Value="Temperature"></asp:ListItem>
                                        <asp:ListItem Text="Noise" Value="Noise"></asp:ListItem>
                                        <asp:ListItem Text="Velocity-X" Value="Velocity-X"></asp:ListItem>
                                        <asp:ListItem Text="Velocity-Y" Value="Velocity-Y"></asp:ListItem>
                                        <asp:ListItem Text="Velocity-Z" Value="Velocity-Z"></asp:ListItem>
                                        <asp:ListItem Text="Acceleration-X" Value="Acceleration-X"></asp:ListItem>
                                        <asp:ListItem Text="Acceleration-Y" Value="Acceleration-Y"></asp:ListItem>
                                        <asp:ListItem Text="Acceleration-Z" Value="Acceleration-Z"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 500px;">
                                    <asp:TextBox ID="txtxWarningUSL" runat="server" Text='<%# Bind("WarningUSL") %>' onkeypress="return event.charCode >= 48 && event.charCode <= 57 || event.charCode <= 46" CssClass="form-control" />
                                </td>
                                <td style="width: 500px">
                                    <asp:TextBox ID="txtDangerUSL" runat="server" Text='<%# Bind("DangerUSl") %>' onkeypress="return event.charCode >= 48 && event.charCode <= 57 || event.charCode <= 46" CssClass="form-control" />
                                </td>
                                <td style="width: 500px">
                                    <asp:TextBox ID="txtMValue" runat="server" Text='<%# Bind("MValue") %>' onkeypress="return event.charCode >= 48 && event.charCode <= 57" CssClass="form-control" />
                                </td>
                                <td style="width: 500px">
                                    <asp:TextBox ID="txtNValue" runat="server" Text='<%# Bind("NValue") %>' onkeypress="return event.charCode >= 48 && event.charCode <= 57" CssClass="form-control" />
                                </td>
                                <td style="width: 500px">
                                    <asp:CheckBox runat="server" ID="deletecheckbox" />
                                    <asp:HiddenField runat="server" ID="hiddedfield" Value='<%# Bind("hidval") %>' ClientIDMode="Static" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table>
                                <tr>
                                    <th style="width: 500px">
                                        <span>Machine ID</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Warning USL</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Component ID</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Operation</span>
                                    </th>
                                    <th style="width: 500px" class="paramVisible">
                                        <span>Operation</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>Error USL</span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>M Value </span>
                                    </th>
                                    <th style="width: 500px">
                                        <span>N Value</span>
                                    </th>
                                    <th style="width: 500px"></th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
                <div style="text-align: end; height: 10vh">
                    <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Add" CssClass="button" />
                    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" CssClass="button" OnClientClick="if(ValidateData()==false) return (false);" />
                    <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Delete" CssClass="button" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {

            isParameterIdEnabled();
        });
        function isParameterIdEnabled() {
            var isParamEnabled = false;
            $.ajax({
                async: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VibrationDashboard.aspx/isParameterIdRequired",
                dataType: "json",
                success: function (result) {
                    isParamEnabled = result.d;
                },
                error: function (Result) {
                    alert("Error");
                }
            });
            debugger;
            if (isParamEnabled == false) {
                $('.paramVisible').css('display', 'none');
                $('.paramVisible').css('display', 'none');
            }
            return isParamEnabled;
        }
        $("#tbllst tbody tr").click(function () {
            $(this).find('#hiddedfield').val("true");
        });

        function ValidateData() {
            var result = true;
            var machine = "";
            var comp = "";
            var operation = "";
            debugger;
            $("#tbllst tbody tr").each(function () {
                var val = $(this).find('#hiddedfield').val();
                if (val == "true") {
                    var opr = $(this).find("td:eq(2)").text().trim();
                    if (opr == undefined || opr == null || opr == "") {
                        opr = $(this).find("td:eq(2)").find("input[type=text]").val();
                    }
                    debugger;
                    var warningLimit = $(this).find("td:eq(4)").find("input[type=text]").val();
                    var errorLimit = $(this).find("td:eq(5)").find("input[type=text]").val();
                    var Mvalue = $(this).find("td:eq(6)").find("input[type=text]").val();
                    var Nvalue = $(this).find("td:eq(7)").find("input[type=text]").val();
                    if (opr == undefined || opr == null || opr == "") {
                        alert("Operation cannot be empty.");
                        result = false;
                    }
                    else if (warningLimit == undefined || warningLimit == null || warningLimit == "") {
                        alert("Warning limit cannot be empty.");
                        result = false;
                    }
                    else if (errorLimit == undefined || errorLimit == null || errorLimit == "") {
                        alert("Error limit cannot be empty.");
                        result = false;
                    }
                    else if (parseFloat(warningLimit) >= parseFloat(errorLimit)) {
                        alert("Warning limit must be less than Error limit.");
                        result = false;
                    }
                    if (Mvalue == undefined || Mvalue == null || Mvalue == "") {
                        $(this).find("td:eq(6)").find("input[type=text]").val(null);
                    }
                    if (Nvalue == undefined || Nvalue == null || Nvalue == "") {
                        $(this).find("td:eq(7)").find("input[type=text]").val(null);
                    }
                    if (parseFloat(Mvalue) < parseFloat(Nvalue)) {
                        alert("MValue must be greater than or equal to Nvalue.");
                        result = false;
                    }
                    //if ($("[id$=btnAdd").val() == "Cancel") {
                    //    var MCOData = {
                    //        MCOList: { ListMCOInfo: [] }
                    //    };
                    //    machine = $(this).find("td:eq(0)").text().trim();
                    //    comp = $(this).find("td:eq(1)").text().trim();
                    //    operation = $(this).find("td:eq(2)").text().trim();
                    //    MCOData = GetMCOData();
                    //    if (!(MCOData == undefined) && !(MCOData == null)) {
                    //        MCOData.MCOList.ListMCOInfo.forEach(function (value, index) {

                    //        });
                    //    }
                    //}
                }
            });
            return result;
        }

        function GetMCOData() {
            var MCOData = {
                MCOList: { ListMCOInfo: [] }
            };
            $("#tbllst tbody tr:gt(1)").each(function () {

                var Machine = $(this).find("td:eq(0)").text().trim();
                var Component = $(this).find("td:eq(1)").text().trim();
                var Operation = $(this).find("td:eq(2)").text().trim();
                var mcoData = {
                    MachineID: Machine, ComponentID: Component, OperationID: Operation
                };
                MCOData.MCOList.ListMCOInfo.push(mcoData);
            });
            return MCOData;
        }

        function messageDeleted() {
            Command: toastr["success"]("Deleted Successfully")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function messageUNSaved() {
            Command: toastr["success"]("No Change to Save")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function messageUNDelete() {
            Command: toastr["success"]("Please Check to Delete")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }

        function messageSaved() {
            Command: toastr["success"]("Saved Successful")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {

                isParameterIdEnabled();
            });
            $("#tbllst tbody tr").click(function () {
                $(this).find('#hiddedfield').val("true");
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
