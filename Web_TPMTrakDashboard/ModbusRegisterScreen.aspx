<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModbusRegisterScreen.aspx.cs" Inherits="Web_TPMTrakDashboard.ModbusRegisterScreen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>

    <style>
        .Btn {
            margin-bottom: 10px;
            border-radius: 2px;
            min-width: 100px;
            max-width: 100px;
            min-height: 40px;
            max-height: 40px;
        }

        .Header-GridView {
            background-color: #2e6886;
            color: white;
        }

        #gvModbus {
            border-color: white;
        }

            #gvModbus > tbody > tr > th {
                border-right: 1px solid white;
                z-index: 900;
                text-align: center;
            }

                #gvModbus > tbody > tr > th:nth-child(1) {
                    position: sticky;
                    left: -1px;
                    z-index: 1000;
                }

            #gvModbus > tbody > tr:nth-child(odd) > td {
                background-color: #DCDCDC;
            }

            #gvModbus > tbody > tr:nth-child(even) > td {
                background-color: white;
            }

        .txtboxSetting {
            border: 1px solid;
            padding-left: 5px;
            border-color: #ccc;
        }

        .itemclass {
            padding: 5px;
        }

        #gvModbus > tbody > tr > td {
            z-index: 800;
            border-right: 1px solid white;
            min-width: 120px;
        }

            #gvModbus > tbody > tr > td:nth-child(1) {
                position: sticky;
                left: -1px;
                z-index: 900;
                min-width: 120px;
            }
    </style>
    <div>

        <asp:Button CssClass="bajaj-btn-style Btn" Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" />
    </div>
    <div style="overflow: auto; height: 85vh;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:GridView runat="server" ID="gvModbus" AutoGenerateColumns="false" CssClass="headerFixer" ClientIDMode="Static">
                    <Columns>
                        <asp:TemplateField HeaderText="Machine ID" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnUpdate" />
                                <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("Machine_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Holding Register For Communication" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHoldingRegisterForCommunication" Text='<%# Eval("HoldingRegisterForCommunication") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Holding Register Date And Status" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHoldingRegisterDateAndStatus" Text='<%# Eval("HoldingRegisterDateAndStatus") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Holding Register Date And Status Ack Address" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHoldingRegisterDateAndStatusAckAddress" Text='<%# Eval("HoldingRegisterDateAndStatusAckAddress") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Holding Register Start Address_M1" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHoldingRegisterStartAddress_M1" Text='<%# Eval("HoldingRegisterStartAddress_M1") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bytes To Read_M1" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtBytesToRead_M1" Text='<%# Eval("BytesToRead_M1") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AckAddress_M1" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtAckAddress_M1" Text='<%# Eval("AckAddress_M1") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Holding Register Start Address_M2" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHoldingRegisterStartAddress_M2" Text='<%# Eval("HoldingRegisterStartAddress_M2") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bytes To Read_M2" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtBytesToRead_M2" Text='<%# Eval("BytesToRead_M2") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ack Address_M2" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtAckAddress_M2" Text='<%# Eval("AckAddress_M2") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Holding Register Start Address_M3" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHoldingRegisterStartAddress_M3" Text='<%# Eval("HoldingRegisterStartAddress_M3") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bytes To Read_M3" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtBytesToRead_M3" Text='<%# Eval("BytesToRead_M3") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ack Address_M3" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtAckAddress_M3" Text='<%# Eval("AckAddress_M3") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DownCode Request Addess" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtDownCodeRequestAddess" Text='<%# Eval("DownCodeRequestAddess") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DownCode Starting Address" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtDownCodeStartingAddress" Text='<%# Eval("DownCodeStartingAddress") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DownInterface ID Starting Address" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtDownInterfaceIDStartingAddress" Text='<%# Eval("DownInterfaceIDStartingAddress") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Number Of DownCode Address" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtTotalNumberOfDownCodeAddress" Text='<%# Eval("TotalNumberOfDownCodeAddress") %>' CssClass="txtboxSetting form-control allowNumber"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Inserted Date/time" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass" Visible="false">
                            <ItemTemplate>
                                    <asp:Label runat="server" ID="txtInsertedDatetime" ClientIDMode="Static" AutoCompleteType="Disabled" Text='<%# Eval("Inserted Date/time") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Updated Date/Time" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass" Visible="false">
                            <ItemTemplate>
                                    <asp:Label runat="server" ID="txtUpdatedDateTime" Text='<%# Eval("Updated Date/Time") %>' ClientIDMode="Static" AutoCompleteType="Disabled" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Updated By" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass" Visible="false">
                            <ItemTemplate>
                                <%--<asp:TextBox runat="server" ID="txtLastUpdateBy" Text='<%# Eval("LastUpdated By") %>' CssClass="form-control textboxSetting"></asp:TextBox>--%>
                                <asp:Label runat="server" ID="txtLastUpdatedBy" Text='<%# Eval("LastUpdated By") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HMI Register_M1" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHMIRegister_M1" Text='<%# Eval("HMIRegister_M1") %>' CssClass="txtboxSetting form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HMI Register_M2" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtHMIRegister_M2" Text='<%# Eval("HMIRegister_M2") %>' CssClass="txtboxSetting form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SharedMachineID" HeaderStyle-CssClass="Header-GridView" ItemStyle-CssClass="itemclass">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtSharedMachineID" Text='<%# Eval("SharedMachineID") %>' CssClass="txtboxSetting form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            setDateTimePicker();
        });
        $('#gvModbus tr td .form-control').focus(function () {
            $(this).closest("tr").find("#hdnUpdate").val("update");
        });
        function setDateTimePicker() {
            $('[id$=txtInsertedDatetime]').datetimepicker({
                format: 'MM-DD-YYYY HH:mm:SS',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtUpdatedDateTime]').datetimepicker({
                format: 'MM-DD-YYYY HH:mm:SS',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
        }

        $('.allowNumber').keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode == 48 && pos == 0) {
                return false
            } else if ((charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                setDateTimePicker();
            });
            $('#gvModbus tr td .form-control').focus(function () {
                $(this).closest("tr").find("#hdnUpdate").val("update");
            });
            $('.allowNumber').keypress(function (evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 48 && pos == 0) {
                    return false
                } else if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
