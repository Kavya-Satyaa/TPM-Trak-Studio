<%@ Page Title="Inspection Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionMasterShanti.aspx.cs" Inherits="Web_TPMTrakDashboard.InspectionMasterShanti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <style>
        .multiselect {
            min-width: 240px;
        }

        .multiselect-container {
            height: 50vh;
            overflow: auto;
        }

        .multiselect-native-select {
            margin-right: 10px;
        }

        .lbl1 {
            color: white;
            margin-left: 10px;
            padding: 0px;
        }

        .tbl1 tr td {
            padding: 0px;
            margin: 0px;
            width: auto;
        }

            .tbl1 tr td select {
                width: 10px;
            }

            .tbl1 tr td input {
                width: auto;
                margin-left: 10px;
            }

        .gridBtn {
            margin-left: 20px;
            margin-right: 20px;
        }

        .GridView1 tr th {
            padding-left: 5px;
            padding-right: 5px;
            border: none;
            color: white;
            background-color: #2E6886 !important;
        }

        .GridView1 tr:nth-child(odd) {
            background-color: #DCDCDC;
            color: black;
            padding-left: 5px;
            padding-right: 5px;
            border: none;
            /*background-color: #d0e0f5;*/
        }

        .GridView1 tr:nth-child(even) {
            background-color: #FFFFFF;
            color: black;
            padding-left: 5px;
            padding-right: 5px;
            border: none;
        }

        /*fixed Grid header*/
        .GridView1 tr th {
            position: sticky;
            top: -1px;
            left: -1px;
            z-index: 1;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            padding: 8px;
            line-height: 1.428571429;
            vertical-align: top;
            border-top: 1px solid #202648;
            border: 1px solid #202648
        }
    </style>

    <div class="container-fluid">
        <div id="main" style="width: auto; padding: 0px; margin: 5px;">

            <div class="col-lg-4" style="text-align: left; margin-top: 10px">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlMachine" runat="server" class="btn dropdown-toggle form-control MachID" Style="width: 150px" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlComponent" runat="server" class="btn dropdown-toggle form-control ComponentID" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" AutoPostBack="true" Style="width: 150px; margin-left: 20px">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlOperation" runat="server" class="btn dropdown-toggle form-control Operation" Style="width: 150px; margin-left: 20px">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-lg-4" style="text-align: right; margin-top: 3px">
                <table>
                    <tr>
                        <td style="padding: 5px">
                            <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" />
                        </td>
                        <td style="border: solid; color: white;">
                            <asp:FileUpload runat="server" ID="fileupload" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnImport" Text="Import" CssClass="btn btn-info" OnClick="btnImport_Click" Style="margin-left: 20px" Visible="true" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" OnClientClick="if(!checkAllowSave()){return false};" Style="margin-left: 20px" />
                        </td>
                        <td></td>
                    </tr>
                </table>

                <%--  <asp:Button runat="server" ID="btnNew" Text="New" CssClass="btn btn-info" OnClick="btnNew_Click" Style="margin-left: 20px" />
                <asp:Button runat="server" ID="btnCancel" Visible="false" Text="Cancel" CssClass="btn btn-info" OnClick="btnCancel_Click" Style="margin-left: 20px" />--%>
            </div>
            <div class="col-lg-4" style="margin-top: 3px;">
                <table style="float: right; margin-right: 50px;">
                    <tr>
                        <td>
                             <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" Style="margin-right: 10px"/>
                        </td>
                        <td style="margin-left: 10px;">
                            <asp:Button runat="server" ID="btnCopy" Text="Copy Inspection Data" CssClass="btn btn-info" OnClick="btnCopy_Click1" />
                        </td>
                    </tr>
                </table>
            </div>

            <asp:UpdatePanel ID="updatepnl" runat="server">
                <ContentTemplate>
                    <div id="gridContainer" class="gridContainer" style="overflow: auto; padding: 0px; width: 100%; margin-top: 50px;" runat="server">
                        <asp:GridView ID="GridView1" CssClass="GridView1" ClientIDMode="Static" ShowFooter="true" runat="server" AutoGenerateColumns="false" Style="margin-top: 30px;" OnRowDeleting="GridView1_RowDeleting" OnRowCommand="GridView1_RowCommand">
                            <Columns>

                                <asp:BoundField DataField="MachineID" HeaderText="Machine ID" />
                                <asp:BoundField DataField="ComponentID" HeaderText="Component ID" />
                                <asp:BoundField DataField="OperationID" HeaderText="Operation" />

                                <asp:TemplateField HeaderText="Characteristic ID">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnMachineID" Value='<%# Eval("MachineID") %>' runat="server" />
                                        <asp:HiddenField ID="hdnCompID" Value='<%# Eval("ComponentID") %>' runat="server" />
                                        <asp:HiddenField ID="hdnOperationID" Value='<%# Eval("OperationID") %>' runat="server" />
                                        <asp:Label ID="txtCharID" Text='<%# Eval("CharID") %>' runat="server" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCharIDfooter" runat="server" CssClass="form-control" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Characterstic Code">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCharactersticCode" Text='<%# Eval("CharCode") %>' runat="server" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCharCodefooter" runat="server" CssClass="form-control" Enabled="true" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Characteristic Code">
                                    <ItemTemplate>
                                        <asp:Label ID="txtCharCode" Text='<%# Eval("CharCode") %>' runat="server" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCharCodefooter" runat="server" CssClass="form-control" Enabled="true"/>
                                    </FooterTemplate>
                                </asp:TemplateField>--%>



                                <asp:TemplateField HeaderText="LSL">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtLSL" Text='<%# Eval("LSL") %>' onkeypress="return allowNumeric(event);" Width="80" onblur="return checkLSLUSLValue(this,'LSL','Edit')" ClientIDMode="Static" CssClass="form-control" />
                                        <asp:HiddenField runat="server" ID="hfLSLLimitCross" ClientIDMode="Static" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtLSLfooter" runat="server" onkeypress="return allowNumeric(event);" Width="80" onblur="return checkLSLUSLValue(this,'LSL','New')" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hfFooterLSLLimitCross" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lower Warning Zone">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtLowerWarningZone" Text='<%# Eval("LowerWarningZone") %>' onkeypress="return allowNumeric(event);" ClientIDMode="Static" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtLowerWarningZonefooter" runat="server" onkeypress="return allowNumeric(event);" CssClass="form-control" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lower Operating Zone">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtLowerOperatingZone" Text='<%# Eval("LowerOperatingZone") %>' onkeypress="return allowNumeric(event);" ClientIDMode="Static" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtLowerOperatingZonefooter" runat="server" onkeypress="return allowNumeric(event);" CssClass="form-control" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Specific Mean">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSpecification" Text='<%# Eval("specification") %>' runat="server" CssClass="form-control" onkeypress="return allowNumeric(event);" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtSpecificationfooter" runat="server" CssClass="form-control" Enabled="true" onkeypress="return allowNumeric(event);" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upper Operating Zone">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtUpperOperatingZone" Text='<%# Eval("UpperOperatingZone") %>' onkeypress="return allowNumeric(event);" ClientIDMode="Static" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUpperOperatingZonefooter" runat="server" onkeypress="return allowNumeric(event);" CssClass="form-control" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upper Warning Zone">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtUpperWarningZone" Text='<%# Eval("UpperWarningZone") %>' onkeypress="return allowNumeric(event);" ClientIDMode="Static" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUpperWarningZonefooter" runat="server" onkeypress="return allowNumeric(event);" CssClass="form-control" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="USL">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUSL" Text='<%# Eval("USL") %>' runat="server" onkeypress="return allowNumeric(event);" Width="80" onblur="return checkLSLUSLValue(this,'USL','Edit')" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hfUSLLimitCross" ClientIDMode="Static" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUSLfooter" runat="server" onkeypress="return allowNumeric(event);" Width="80" onblur="return checkLSLUSLValue(this,'USL','New')" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hfFooterUSLLimitCross" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SampleSize">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSampleSize" Text='<%# Eval("SampleSize") %>' runat="server" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtSampleSizefooter" runat="server" CssClass="form-control" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="UOM">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUOM" Text='<%# Eval("UOM") %>' runat="server" CssClass="form-control" Width="80" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUOMfooter" runat="server" CssClass="form-control" Width="80" />
                                    </FooterTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Channel">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtChannel" Text='<%# Eval("Channel") %>' runat="server" CssClass="form-control" Width="80" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFooterChannel" runat="server" CssClass="form-control" Width="80" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Input Method">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlInputMethod" Text='<%# Eval("InputMethod") %>' runat="server" CssClass="form-control" Width="120">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Auto" Value="Auto"></asp:ListItem>
                                            <asp:ListItem Text="Manual" Value="Manual"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFooterInputMethod" runat="server" CssClass="form-control" Width="120">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Auto" Value="Auto"></asp:ListItem>
                                            <asp:ListItem Text="Manual" Value="Manual"></asp:ListItem>
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Data Template" ControlStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlDataTemp" runat="server" Text='<%# Eval("DataTemplate") %>' CssClass="form-control">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Ok" Value="Ok"></asp:ListItem>
                                            <asp:ListItem Text="NotOk" Value="NotOk"></asp:ListItem>
                                            <asp:ListItem Text="Numeric" Value="Numeric"></asp:ListItem>
                                            <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                            <asp:ListItem Text="VisualInspection" Value="VisualInspection"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlDataTempfooter" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="" Value="Text"></asp:ListItem>
                                            <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                            <asp:ListItem Text="Ok" Value="Ok"></asp:ListItem>
                                            <asp:ListItem Text="NotOk" Value="NotOk"></asp:ListItem>
                                            <asp:ListItem Text="Numeric" Value="Numeric"></asp:ListItem>
                                            <asp:ListItem Text="VisualInspection" Value="VisualInspection"></asp:ListItem>
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="InspectedBy" ControlStyle-Width="130px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlInspectedBy" runat="server" Text='<%# Eval("InspectedBy") %>' CssClass="form-control">
                                            <asp:ListItem Text="Operator" Value="Operator"></asp:ListItem>
                                            <asp:ListItem Text="Inspector" Value="Inspector"></asp:ListItem>
                                            <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlInspectedByfooter" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Operator" Value="Operator"></asp:ListItem>
                                            <asp:ListItem Text="Inspector" Value="Inspector"></asp:ListItem>
                                            <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="IsEnabled?" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Eval("IsEnabled") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkEnabledfooter" Checked="true" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton runat="server" CssClass="glyphicon glyphicon-save" CommandName="Delete" ToolTip="Delete" />--%>
                                        <asp:LinkButton runat="server" CssClass="glyphicon glyphicon-trash gridBtn" CommandName="Delete" ToolTip="Delete" Style="margin-left: 40px" ForeColor="Red"></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" ID="LinkButton1" CssClass="glyphicon glyphicon-plus-sign gridBtn" CommandName="AddNew" ToolTip="AddNew"></asp:LinkButton>
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="modal" role="dialog" id="viewmode modal">
                        <div class="modal-dialog modal-dialog-centered" role="document" style="width: 30%">
                            <div class="modal-content" style="border: 2px solid #5D7B9D; background-color: #202648">
                                <div class="modal-header" style="background-color: #2E6886; color: white;">
                                    <button type="button" class="close updatecloses" data-dismiss="modal">&times;</button>
                                    <h4 class="modal-title"><span style="font-size: 18px; font-weight: 600;">Copy Information</span></h4>
                                </div>
                                <div class="modal-body" style="min-height: 115px;">
                                    <div class="divframe">
                                        <table id="ulmenu" class="table">
                                            <tr>
                                                <td>
                                                    <span style="color:white">Source Machine:</span>
                                                </td>
                                                <td>
                                                   <asp:Label ID="lblsourceMachine" runat="server" ClientIDMode="Static"  style="color:white" />
                                                </td>
                                             </tr>
                                            <tr>
                                                <td>
                                                    <span style="color:white">Component ID:</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCompID" runat="server"  ClientIDMode="Static" style="color:white"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color:white">Operation No:</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblOperationNo" runat="server"  ClientIDMode="Static" style="color:white"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="vertical-align: -webkit-baseline-middle; color: white;">Target Machines:</span>
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="ddlMultiMachineId" ClientIDMode="Static" CssClass="dropdown multiDropdown" runat="server" SelectionMode="Multiple" Style="min-width: 180px; margin-right: 5px;"></asp:ListBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align:center">
                                                    <asp:Button runat="server" ID="ButtonSave" Text="Save" CssClass="btn btn-info" OnClick="btnCopy_Click" style="margin-right:10px"/>
                                                    <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CssClass="btn btn-info" OnClientClick="return HideCopyModel();"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
                <%--<Triggers>
                    <asp:PostBackTrigger ControlID="GridView1" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>--%>
            </asp:UpdatePanel>



            <div class="modal fade" id="myConfirmationModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                    <div class="modal-content" style="border: 2px solid #2E6886">
                        <div class="modal-header" style="background-color: #2E6886; padding: 8px">
                            <h4 class="modal-title" style="color: white;">Confirmation?</h4>
                        </div>
                        <div class="modal-body">
                            <span id="confirmationmessageText" style="font-size: 17px;">Confirmation</span>
                        </div>
                        <div class="modal-footer" style="padding: 5px; border-top: 1px solid #2E6886">
                            <input type="button" value="Yes" class="btn btn-info" id="saveConfirmYes" onserverclick="saveConfirmYes_ServerClick" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                            <input type="button" value="No" class="btn btn-info" id="saveConfirmNo" onclick="saveConfirmNo()" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="WarningModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                    <div class="modal-content" style="border: 2px solid #2E6886">
                        <div class="modal-header" style="background-color: #2E6886; padding: 8px">
                            <h4 class="modal-title" style="color: white;">Warning</h4>
                        </div>
                        <div class="modal-body">
                            <span id="warningmessageText" style="font-size: 17px;"></span>
                        </div>
                        <div class="modal-footer" style="padding: 5px; border-top: 1px solid #2E6886">
                            <input type="button" value="OK" class="btn btn-info" id="Button1" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="myFileUploadModal" role="dialog" style="min-width: 300px;">
                <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                    <div class="modal-content" style="border: 2px solid #2E6886">
                        <div class="modal-header" style="background-color: #2E6886; padding: 8px">

                            <h4 class="modal-title" style="color: white;">Choose File</h4>
                        </div>
                        <div class="modal-body">
                            <asp:FileUpload ID="globe_Check" CssClass="form-control" runat="server" BackColor="LightGray" />
                        </div>
                        <div class="modal-footer" style="padding: 5px; border-top: 1px solid #2E6886">
                            <input type="button" value="Import" class="btn btn-info" id="saveConfirmYes1" onserverclick="importClick" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                            <input type="button" value="Cancel" class="btn btn-info" id="saveConfirmNo1" onclick="cancelImport()" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>


    <script type="text/javascript">  
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $('.multiDropdown').multiselect({
                includeSelectAllOption: true
            });

            var wHeight = $(window).height() - 150;
            $('.gridContainer').css('height', wHeight);

            $(".updatecloses").click(function () {
                $('.modal').modal('hide');
                $('.modal-backdrop').remove();
            });

            function allowNumeric(evt) {
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                var pos = evt.target.selectionStart;
                if (charCode == 45 && pos != 0) {
                    return false;
                } else if (charCode == 43 && pos != 0) {
                    return false;
                } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                    return false;
                } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                    return false;
                }
                return true;
            }
            function checkLSLUSLValue(val, param, mode) {
                if (mode == "Edit") {
                    if (param == "LSL") {
                        if (parseFloat($(val).val()) > parseFloat($(val).closest('tr').find('#txtUSL').val())) {
                            $(val).val("");
                            $('[id*=WarningModal]').modal('show');
                            $("#warningmessageText").text('LSL value is greater than USL value');
                        }
                    }
                    if (param == "USL") {
                        if (parseFloat($(val).val()) < parseFloat($(val).closest('tr').find('#txtLSL').val())) {
                            $(val).val("");
                            $('[id*=WarningModal]').modal('show');
                            $("#warningmessageText").text('USL value is smaller than LSL value');
                        }
                    }
                }
                if (mode == "New") {
                    if (param == "LSL") {
                        if (parseFloat($(val).val()) > parseFloat($(val).closest('tr').find('#txtUSLfooter').val())) {
                            $(val).val("");
                            $('[id*=WarningModal]').modal('show');
                            $("#warningmessageText").text('LSL value is greater than USL value');
                        }
                    }
                    if (param == "USL") {
                        if (parseFloat($(val).val()) < parseFloat($(val).closest('tr').find('#txtLSLfooter').val())) {
                            $(val).val("");
                            $('[id*=WarningModal]').modal('show');
                            $("#warningmessageText").text('USL value is smaller than LSL value');
                        }
                    }
                }
            }
        });

        $(document).ready(function () {
            var wHeight = $(window).height() - 150;
            $('.gridContainer').css('height', wHeight);

            $('.multiDropdown').multiselect({
                includeSelectAllOption: true
            });

            $(".updatecloses").click(function () {
                $('.modal').modal('hide');
                $('.modal-backdrop').remove();
            });
        });
        function allowNumeric(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            var pos = evt.target.selectionStart;
            if (charCode == 45 && pos != 0) {
                return false;
            } else if (charCode == 43 && pos != 0) {
                return false;
            } else if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                return false;
            } else if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57) && charCode != 45 && charCode != 43) {
                return false;
            }
            return true;
        }


        function CopyView() {
            $('[id*=viewmode]').modal('show');
            var SouceMach = $('.MachID :selected').text();
            var CompId = $('.ComponentID :selected').text();
            var Operation = $('.Operation :selected').text();
            $("#lblsourceMachine").text(SouceMach);
            $("#lblCompID").text(CompId);
            $("#lblOperationNo").text(Operation);

        }

        function HideCopyModel()
        {
            $('.modal').modal('hide');
            $('.modal-backdrop').remove();
        }

        function checkLSLUSLValue(val, param, mode) {

            if (mode == "Edit") {
                if (param == "LSL") {
                    if (parseFloat($(val).val()) > parseFloat($(val).closest('tr').find('#txtUSL').val())) {
                        //$(val).val("");
                        $(val).closest('tr').find('#hfLSLLimitCross').val(1);
                        $(val).closest('tr').css('background-color', '#ff6969');
                        $('[id*=WarningModal]').modal('show');
                        $("#warningmessageText").text('LSL value is greater than USL value');
                    } else {
                        $(val).closest('tr').find('#hfLSLLimitCross').val(0);
                        $(val).closest('tr').css('background-color', 'white');
                    }
                }
                if (param == "USL") {
                    if (parseFloat($(val).val()) < parseFloat($(val).closest('tr').find('#txtLSL').val())) {
                        //$(val).val("");
                        $(val).closest('tr').find('#hfUSLLimitCross').val(1);
                        $(val).closest('tr').css('background-color', '#ff6969');
                        $('[id*=WarningModal]').modal('show');
                        $("#warningmessageText").text('USL value is smaller than LSL value');
                    } else {
                        $(val).closest('tr').find('#hfUSLLimitCross').val(0);
                        $(val).closest('tr').css('background-color', 'white');
                    }
                }
            }
            if (mode == "New") {

                if (param == "LSL") {
                    if (parseFloat($(val).val()) > parseFloat($(val).closest('tr').find('#txtUSLfooter').val())) {
                        $(val).closest('tr').find('#hfFooterLSLLimitCross').val(1);
                        $(val).closest('tr').css('background-color', '#ff6969');
                        $('[id*=WarningModal]').modal('show');
                        $("#warningmessageText").text('LSL value is greater than USL value');
                    }
                } else {
                    $(val).closest('tr').find('#hfFooterLSLLimitCross').val(0);
                    $(val).closest('tr').css('background-color', 'white');
                }
                if (param == "USL") {
                    if (parseFloat($(val).val()) < parseFloat($(val).closest('tr').find('#txtLSLfooter').val())) {
                        $(val).closest('tr').find('#hfFooterUSLLimitCross').val(1);
                        $(val).closest('tr').css('background-color', '#ff6969');
                        $('[id*=WarningModal]').modal('show');
                        $("#warningmessageText").text('USL value is smaller than LSL value');
                    } else {
                        $(val).closest('tr').find('#hfFooterUSLLimitCross').val(0);
                        $(val).closest('tr').css('background-color', 'white');
                    }
                }
            }
        }
        function checkAllowSaveForNew() {
            let returnvalue = true;
            let row = $('#GridView1 tr')[$('#GridView1 tr').length - 1];
            if ($(row).find("#hfFooterLSLLimitCross").val() == "1" || $(row).find("#hfFooterUSLLimitCross").val() == "1") {
                returnvalue = false;
                $('[id*=WarningModal]').modal('show');
                $("#warningmessageText").text('Please reset the LSL and USL value.');
            }
            return returnvalue;
        }
        function checkAllowSave() {

            //let returnvalue = true;
            //for (let i = 1; i < $('#GridView1 tr').length; i++) {
            //    let row = $('#GridView1 tr')[i];
            //    if ($(row).find("#hfLSLLimitCross").val() == "1" || $(row).find("#hfUSLLimitCross").val() == "1") {
            //        returnvalue = false;
            //        $('[id*=WarningModal]').modal('show');
            //        $("#warningmessageText").text('Please reset the LSL and USL value.');
            //        break;
            //    }
            //if ($(row).find("#hfLSLLimitCross").val() == "1") {
            //    returnvalue = false;
            //    $('[id*=WarningModal]').modal('show');
            //    $("#warningmessageText").text('LSL cannot be greater than USL');
            //    break;
            //} else if ($(row).find("#hfUSLLimitCross").val() == "1") {
            //     returnvalue = false;
            //    $('[id*=WarningModal]').modal('show');
            //    $("#warningmessageText").text('USL cannot be smaller than LSL');
            //    break;
            //}

            return returnvalue;
        }
        function openConfirmModal(msg) {
            $('[id*=myConfirmationModal]').modal('show');
            $("#confirmationmessageText").text(msg);
        }

        function saveConfirmNo() {
            $('[id*=myConfirmationModal]').modal('hide');
        }

        function openFileUploadModal(msg) {
            debugger;
            $('[id*=myFileUploadModal]').modal('show');
            $("#confirmationmessageText1").text(msg);
        }

        function saveConfirmNo1() {
            $('[id*=myFileUploadModal]').modal('hide');
        }

        function showpop(msg, title) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "3000",
                "hideDuration": "1000",
                "timeOut": "1000",
                "extendedTimeOut": "100",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            // toastr['success'](msg, title);
            var d = Date();
            toastr.success(msg, title);
            return false;
        }

        $(window).resize(function () {
            var Height = $(window).height() - 150;
            $('.gridContainer').css('height', Height);
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
