<%@ Page Title="LiveData" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LiveData.aspx.cs" Inherits="Web_TPMTrakDashboard.EnergyModule.LiveData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="">
            <div class="" style="height: auto; border: 2px solid #ece3e3; margin: auto; width: auto; display: flex; justify-content: flex-end; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); border-radius: 7px">
                <asp:UpdatePanel runat="server" ID="updFilter">
                    <ContentTemplate>
                        <table style="width: auto; vertical-align: middle; margin: 5px;">
                            <tr>
                                <td style="padding: 10px;">
                                    <asp:DropDownList runat="server" ID="ddlMachineType" CssClass="form-control specInputField" Style="margin: 0 10px 0 0;" OnSelectedIndexChanged="ddlMachineType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="Machine EM" Text="Machining" />
                                        <asp:ListItem Value="Non-Machine EM" Text="Non-Machining" />
                                    </asp:DropDownList>
                                </td>
                                <td id="tdPlant" runat="server" style="padding: 10px; font-size: 16px; line-height: 35px;" class="selectlbl">Plant</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td id="tdCell" runat="server" style="padding: 10px; font-size: 16px; line-height: 35px;" class="selectlbl">Cell</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                                </td>
                                <td style="padding: 10px;">
                                    <asp:Button runat="server" ID="btnView" Text="VIEW" CssClass="btn btn-info btn-sm" ClientIDMode="Static" OnClick="btnView_Click" />
                                </td>
                                <td style="width: auto;">
                                    <asp:Label runat="server" Text="Auto Refresh" CssClass=" selectlbl" Style="display: inline-block; vertical-align: middle; font-size: 18px; margin-right: 10px; line-height: 35px;" />
                                    <label class="switch">
                                        <asp:CheckBox runat="server" ID="cbAutoRefresh" AutoPostBack="True" OnCheckedChanged="cbAutoRefresh_CheckedChanged" meta:resourcekey="cbAutoRefreshResource1" />
                                        <span class="slider round"></span>
                                    </label>
                                </td>
                            </tr>
                        </table>
                        <asp:Timer ID="timerToAutoRefresh" runat="server" Enabled="False" OnTick="timerToAutoRefresh_Tick"></asp:Timer>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnView" />
                        <asp:PostBackTrigger ControlID="ddlPlant" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
            <div class="" style="border: 2px solid #ece3e3; margin: 10px auto 0 auto;">
                <div id="tblGrid" class="" style="overflow: auto; background-color: transparent; height: 100%; width: 100%;">
                    <asp:GridView runat="server" ID="gvLiveData" AutoGenerateColumns="false" CssClass="table table-bordered cockpit headerFixerTable" AllowPaging="false" ShowHeader="true" ShowFooter="false" ShowHeaderWhenEmpty="true" ClientIDMode="Static">
                        <Columns>
                            <asp:TemplateField HeaderText="Machine ID" AccessibleHeaderText="MachineID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMacID" Text='<%# Eval("Machineid") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="kW" AccessibleHeaderText="Kw">
                                <ItemTemplate>
                                    <asp:Label ID="lblKw" runat="server" Text='<%# Eval("Kw") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="kWh" AccessibleHeaderText="KwH">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblKwH" Text='<%# Eval("Kwh") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Power Factor" AccessibleHeaderText="PF">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPowerFactor" Text='<%# Eval("PowerFactor") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DateTime" AccessibleHeaderText="DateTime" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDateTime" Text='<%# Eval("LastArrival_TS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="V LN (R)" AccessibleHeaderText="VLN-R">
                                <ItemTemplate>
                                    <asp:Label ID="lblVLN_R" runat="server" Text='<%# Eval("VLN_R") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="V LN (Y)" AccessibleHeaderText="VLN-Y">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVLN_Y" Text='<%# Eval("VLN_Y") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VLN-B" AccessibleHeaderText="VLN-B">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVLN_B" Text='<%# Eval("VLN_B") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VLN-R-Y" AccessibleHeaderText="VLN-R-Y">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVLN_R_Y" Text='<%# Eval("VLN_R_Y") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VLN-Y-B" AccessibleHeaderText="VLN-Y-B">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVLN_Y_B" Text='<%# Eval("VLN_Y_B") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VLN-B-R" AccessibleHeaderText="VLN-B-R">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVLN_B_R" Text='<%# Eval("VLN_B_R") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="R-amp." AccessibleHeaderText="R-amp">
                                <ItemTemplate>
                                    <asp:Label ID="lblR_amp" runat="server" Text='<%#Eval("R_AMP") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Y-amp." AccessibleHeaderText="Y-amp">
                                <ItemTemplate>
                                    <asp:Label ID="lblY_amp" runat="server" Text='<%#Eval("Y_AMP") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="B-amp." AccessibleHeaderText="B-amp">
                                <ItemTemplate>
                                    <asp:Label ID="lblB_amp" runat="server" Text='<%#Eval("B_AMP") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                          
                            <asp:TemplateField HeaderText="Last Arrival Time" AccessibleHeaderText="LastArrivalTime">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblLAT" Text='<%# Eval("LastArrival_TS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            No records found.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="HeaderCss" />
                        <RowStyle BackColor="White" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="#DCDCDC" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <style>
        .selectlbl {
            color: white;
        }

        .headerFixerTable tr th {
            position: sticky;
            top: -1px;
            z-index: 1;
            background-color: #2e6886;
            color: white;
            min-width: 150px;
            text-align: center;
        }

        .headerFixerTable tbody tr:nth-child(odd) td {
            background-color: #DCDCDC;
        }

        .headerFixerTable tbody tr:nth-child(even) td {
            background-color: #FFFFFF;
        }

        .table tbody > tr > th {
            vertical-align: middle;
        }

        .table tbody > tr > td {
            text-align: center;
            vertical-align: middle;
            height: 30px;
        }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 50px;
            vertical-align: inherit;
        }

        .switch {
            position: relative;
            display: inline-block;
            vertical-align: middle;
            width: 50px;
            height: 30px;
            float: right;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 22px;
                width: 22px;
                left: 3px;
                bottom: 3px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(23px);
            -ms-transform: translateX(23px);
            transform: translateX(23px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 30px;
        }

            .slider.round:before {
                border-radius: 50%;
            }
    </style>
    <script>
        function resize() {
            var heights = window.innerHeight;
            document.getElementById("tblGrid").style.height = heights - 170 + "px";
        }
        $(document).ready(function () {
            resize();
            window.onresize = function () {
                this.resize();
            };
            freezeColumnFromLeft('gvLiveData', 1);
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            resize();
            window.onresize = function () {
                this.resize();
            };
            freezeColumnFromLeft('gvLiveData', 1);
        });
    </script>
</asp:Content>
