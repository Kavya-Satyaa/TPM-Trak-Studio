<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionMasterKTA.aspx.cs" Inherits="Web_TPMTrakDashboard.KTASpindle.InspectionMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <link href="Scripts/SearcableDropDown/select2.min.css" rel="stylesheet" />
    <script src="Scripts/SearcableDropDown/select2.min.js"></script>
    <style>
        .GridView1>thead>tr>th, .GridView1>tbody>tr>th, 
        .GridView1>tfoot>tr>th, .GridView1>thead>tr>td, 
        .GridView1>tbody>tr>td, .GridView1>tfoot>tr>td {
               border: 1px solid #ddd !important;
         }
        .form-control{
            font-weight:bold;
        }
         .select2-container {
            width:195px !important;
            margin-left:10px;
        }
         .select2-container .select2-selection--single {
            box-sizing: border-box;
            cursor: pointer;
            display: block;
            height: 34px;
            user-select: none;
            -webkit-user-select: none;
         }
        .multiselect {
            min-width: 240px;
            max-width:326px;
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
            text-align:center;
            height:35px;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            padding: 8px;
            line-height: 1.428571429;
            vertical-align: top;
            border-top: 1px solid #202648;
            border: 1px solid #202648
        }
        .min-widthCol
        {
            min-width:200px;
        }
        .min-widthSubCol
        {
             min-width:100px;
        }

        .glyphicon-export {
            margin-left: 1px;
            font-size: 20px;
            background: #5bc0de;
            padding: 7px;
            color: white;
            height: 34px;
            border-radius: 3px;
        }
        .glyphicon-import
        {
            margin-left: 1px;
            font-size: 20px;
            background: #5bc0de;
            padding: 7px;
            color: white;
            height: 34px;
            border-radius: 3px;
        }
    </style>

    <div class="container-fluid">
        <div id="main" style="width: auto; padding: 0px; margin: 5px;">
            <div class="bajaj-outer-div-filter-section" style="text-align: left; margin-top: 10px;display:inline-block;">
                <div class="bajaj-inner-div-filter-section left-content-filter-section">
                 
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>Part Family</td>
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlPartFamily" runat="server" class="btn dropdown-toggle form-control PartFamily" Style="width: 150px;font-weight:normal" OnSelectedIndexChanged="ddlPartFamily_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlPartFamily" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>Component</td>
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlComponent" runat="server" class="btn dropdown-toggle form-control ComponentID searchable-dropdown-list" Style="width: 150px; margin-left: 20px" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                          </ContentTemplate>
                                            <Triggers>
                                                  <asp:AsyncPostBackTrigger ControlID="ddlComponent" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>Operation</td>
                                <td>
                                  <asp:UpdatePanel runat="server">
                                     <ContentTemplate>
                                        <asp:DropDownList ID="ddlOperation" runat="server" class="btn dropdown-toggle form-control Operation" >
                                        </asp:DropDownList>
                                      </ContentTemplate>
                                        <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlOperation" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                     <asp:Button runat="server" ID="btnView" Text="View" CssClass="btn btn-info" OnClick="btnView_Click" style="margin-left: 16px;width:100px;" />
                                </td>
                                <td>
                                     <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                             <asp:LinkButton runat="server" ID="lnkExport" ToolTip="Export" OnClick="btnExport_Click" Style="margin-left: 6px;margin-right: 5px;width:100px;">
                                                  <i class="glyphicon glyphicon-export header-top-right-icon hover-change-cursor hover-box-shadow" style="margin-left:5px;"></i>
                                             </asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkExport" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: solid; color: white;">
                                    <asp:FileUpload runat="server" ID="fileupload" style="width:210px;"/>
                                    <asp:LinkButton ID="btnTemplateExport" CssClass="glyphicon glyphicon-download-alt" Text="DownloadTemplate" runat="server" ToolTip="Template" Font-Size="20px" Style="display: inline-block; vertical-align: top;width:200px;" OnClick="btnTemplateExport_Click" />
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID="lnkImport" CssClass="btn btn-info" Text="Import" OnClick="btnImport_Click" Style="margin-left: 12px;width:130px;" Visible="true" ToolTip="Import" />
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:Button runat="server" ID="btnEdit" Text="Edit" CssClass="btn btn-info"  Style="margin-left: 10px;" OnClick="btnEdit_Click" />
                                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" Style="margin-left: 10px;margin-right:10px;" Visible="false" />
                                     </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                     <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                             <asp:Button runat="server" ID="btnCopy" Text="Copy Inspection Data" CssClass="btn btn-info" OnClick="btnCopy_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <div style="margin-top:10px;color:white;font-weight:bold;">
                             <asp:Label ID="lblComp" runat="server"  />
                        </div>
                </div>
            </div>
<%--            <div style="text-align: right; margin-top: 3px;vertical-align:top;display:inline-block;">
                <table>
                    <tr>
                        <td style="border: solid; color: white;">
                            <asp:FileUpload runat="server" ID="fileupload" />                      
                        </td>
                        <td>
                            <asp:LinkButton runat="server" ID="lnkImport" Text="" CssClass="glyphicon glyphicon-import" OnClick="btnImport_Click" Style="margin-left: 20px" Visible="true" ToolTip="Import" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnEdit" Text="Edit" CssClass="btn btn-info"  Style="margin-left: 10px;" OnClick="btnEdit_Click" />
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click" Style="margin-left: 10px;margin-right:10px;" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align:left;">
                              <asp:LinkButton ID="btnTemplateExport" CssClass="glyphicon glyphicon-download-alt" Text="DownloadTemplate" runat="server" ToolTip="Template" Font-Size="20px" Style="display: inline-block; vertical-align: top;" OnClick="btnTemplateExport_Click" />
                        </td>
                    </tr>
                </table>
            </div>--%>
          <%--  <div style="margin-top: 3px;display:inline-block;vertical-align:top">
                 <div style="margin-top: 3px;">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                              <asp:Button runat="server" ID="btnCopy" Text="Copy Inspection Data" CssClass="btn btn-info" OnClick="btnCopy_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>--%>
           

            <asp:UpdatePanel ID="updatepnl" runat="server">
                <ContentTemplate>
                    <div id="gridContainer" class="gridContainer" style="padding: 0px; width: 100%; margin-top: 27px;" runat="server">
                        <div style="overflow:auto;height:70vh;">
                            <asp:GridView ID="GridView1" CssClass="GridView1" ClientIDMode="Static" ShowFooter="true" runat="server" AutoGenerateColumns="false" OnRowDeleting="GridView1_RowDeleting" OnRowCommand="GridView1_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ComponentID" HeaderText="Component ID" Visible="false" />
                                <asp:BoundField DataField="OperationID" HeaderText="Operation" Visible="false" />
                                <asp:TemplateField HeaderText="Characteristic ID">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnCompID" Value='<%# Eval("ComponentID") %>' runat="server" />
                                        <asp:HiddenField ID="hdnOperationID" Value='<%# Eval("OperationID") %>' runat="server" />
                                        <asp:TextBox ID="txtCharID" Text='<%# Eval("CharID") %>' runat="server" CssClass="form-control" Enabled="false" ToolTip='<%# Eval("CharID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="min-widthCol" />
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCharIDfooter" runat="server" CssClass="form-control" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Characterstic Code">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCharactersticCode" Text='<%# Eval("CharCode") %>' runat="server" CssClass="form-control" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="min-widthCol" />
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCharCodefooter" runat="server" CssClass="form-control" Enabled="true" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LSL">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtLSL" Text='<%# Eval("LSL") %>' onkeypress="return allowNumeric(event);"  onblur="return checkLSLUSLValue(this,'LSL','Edit')" ClientIDMode="Static" CssClass="form-control" />
                                        <asp:HiddenField runat="server" ID="hfLSLLimitCross" ClientIDMode="Static" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtLSLfooter" runat="server" onkeypress="return allowNumeric(event);" onblur="return checkLSLUSLValue(this,'LSL','New')" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hfFooterLSLLimitCross" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lower Warning Zone" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtLowerWarningZone" Text='<%# Eval("LowerWarningZone") %>' onkeypress="return allowNumeric(event);" ClientIDMode="Static" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtLowerWarningZonefooter" runat="server" onkeypress="return allowNumeric(event);" CssClass="form-control" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lower Operating Zone"  Visible="false">
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
                                     <ItemStyle CssClass="min-widthSubCol" />
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtSpecificationfooter" runat="server" CssClass="form-control" Enabled="true" onkeypress="return allowNumeric(event);" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upper Operating Zone"  Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtUpperOperatingZone" Text='<%# Eval("UpperOperatingZone") %>' onkeypress="return allowNumeric(event);" ClientIDMode="Static" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUpperOperatingZonefooter" runat="server" onkeypress="return allowNumeric(event);" CssClass="form-control" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upper Warning Zone"  Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtUpperWarningZone" Text='<%# Eval("UpperWarningZone") %>' onkeypress="return allowNumeric(event);" ClientIDMode="Static" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUpperWarningZonefooter" runat="server" onkeypress="return allowNumeric(event);" CssClass="form-control" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="USL">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUSL" Text='<%# Eval("USL") %>' runat="server" onkeypress="return allowNumeric(event);"  onblur="return checkLSLUSLValue(this,'USL','Edit')" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hfUSLLimitCross" ClientIDMode="Static" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUSLfooter" runat="server" onkeypress="return allowNumeric(event);" onblur="return checkLSLUSLValue(this,'USL','New')" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:HiddenField runat="server" ID="hfFooterUSLLimitCross" ClientIDMode="Static" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="SampleSize" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSampleSize" Text='<%# Eval("SampleSize") %>' runat="server" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtSampleSizefooter" runat="server" CssClass="form-control" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Data Template"  ControlStyle-Width="140px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlDataTemp" runat="server" Text='<%# Eval("DataTemplate") %>' CssClass="form-control">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Ok/NotOk" Value="Ok/NotOk"></asp:ListItem>
                                           <%-- <asp:ListItem Text="NotOk" Value="NotOk"></asp:ListItem>--%>
                                            <asp:ListItem Text="Numeric" Value="Numeric"></asp:ListItem>
                                            <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlDataTempfooter" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                            <asp:ListItem Text="Ok/NotOk" Value="Ok/NotOk"></asp:ListItem>
                                            <%--<asp:ListItem Text="NotOk" Value="NotOk"></asp:ListItem>--%>
                                            <asp:ListItem Text="Numeric" Value="Numeric"></asp:ListItem>
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
                                   <asp:TemplateField HeaderText="SortOrder" Visible="true" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtSortOrder" Text='<%# Eval("SortOrder") %>' runat="server" CssClass="form-control" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtSortOrderfooter" runat="server" CssClass="form-control" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CssClass="glyphicon glyphicon-trash gridBtn" CommandName="Delete" ToolTip="Delete" Style="font-size:16px;" ForeColor="Red"></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" ID="lnkbtnAdd" CssClass="glyphicon glyphicon-plus-sign gridBtn" CommandName="AddNew" ToolTip="AddNew"></asp:LinkButton>
                                    </FooterTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                        </div>
                    </div>
                    <div class="modal" role="dialog" id="viewmode modal"  data-backdrop="static" data-keyboard="false">
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
                                                    <span style="color:white">Source Component:</span>
                                                </td>
                                                <td>
                                                   <asp:Label ID="lblsourceComponent" runat="server" ClientIDMode="Static"  style="color:white" />
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
                                                    <span style="color:white">Part Family:</span>
                                                </td>
                                                <td>
                                                     <asp:DropDownList ID="ddlCopyPartFamily" runat="server" class="btn dropdown-toggle form-control" Style="width: 240px;" OnSelectedIndexChanged="ddlCopyPartFamily_SelectedIndexChanged" AutoPostBack="true">
                                                     </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color:white">Target Components:</span>
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="ddlMultiComponent" ClientIDMode="Static" CssClass="dropdown multiDropdown" runat="server" SelectionMode="Multiple" Style="min-width: 180px; margin-right: 5px;"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align:center">
                                                    <asp:Button runat="server" ID="btnSaveCopy" Text="Save" CssClass="btn btn-info" OnClick="btnSaveCopy_Click" style="margin-right:10px"/>
                                                    <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CssClass="btn btn-info" OnClientClick="return HideCopyModel();"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

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
                                    <input type="button" value="Import" class="btn btn-info" id="saveConfirmYes1" runat="server" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                                    <input type="button" value="Cancel" class="btn btn-info" id="saveConfirmNo1" onclick="cancelImport()" style="background-color: #2E6886; color: white" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
               </ContentTemplate>
           </asp:UpdatePanel>
        </div>
    </div>
   

    <script type="text/javascript">  
        $(document).ready(function () {
            var wHeight = $(window).height() - 300;
            $('.gridContainer').css('height', wHeight);

            $('.multiDropdown').multiselect({
                includeSelectAllOption: true
            });

            $(".updatecloses").click(function () {
                $('.modal').modal('hide');
                $('.modal-backdrop').remove();
            });
            $(".searchable-dropdown-list").select2({
                allowClear: false,
                placeholder: "Component ID"
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

        function CopyView() {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            $('[id*=viewmode]').modal('show');
            var CompId = $('.ComponentID :selected').text();
            var Operation = $('.Operation :selected').text();
            $("#lblsourceComponent").text(CompId);
            $("#lblOperationNo").text(Operation);
            return true;
        }

        function HideCopyModel()
        {
            $(".modal-backdrop").removeClass("modal-backdrop in");
            $(".modal").modal("hide");
            $('body').removeClass('modal-open'); 
            return true;
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

        function openSuccessModal(msg) {
            $('#toast-container').empty();
            Command: toastr["success"](msg)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
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
        function showWarningMsg(msg, title) {
            debugger;
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "4000",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "toastClass": "toaster-position"
            }

            toastr['warning'](msg, title);
            return false;
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            $('.multiDropdown').multiselect({
                includeSelectAllOption: true
            });

            var wHeight = $(window).height() - 300;
            $('.gridContainer').css('height', wHeight);

            $(".updatecloses").click(function () {
                $('.modal').modal('hide');
                $('.modal-backdrop').remove();
            });

            $(".searchable-dropdown-list").select2({
                allowClear: false,
                placeholder: "Component ID"
            });
        });

        $(window).resize(function () {
            var Height = $(window).height() - 150;
            $('.gridContainer').css('height', Height);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
