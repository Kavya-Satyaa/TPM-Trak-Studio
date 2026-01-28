<%@ Page Title="JH Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JHMaster.aspx.cs" Inherits="Web_TPMTrakDashboard.Bajaj.JHMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/datecss") %>
    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <%: Scripts.Render("~/bundles/toastrJs") %>

    <div class="content-div">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <div class="bajaj-outer-div-filter-section">
                    <div class="bajaj-inner-div-filter-section left-content-filter-section">
                        <table class="bajaj-filter-tbl">
                            <tr>
                                <td>Plant</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlPlant" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>Cell</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCell" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>Machine</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMachine" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_SelectedIndexChanged"></asp:DropDownList>
                                </td>

                                <td>Rev No.</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlRevNo" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" CssClass="bajaj-btn-style" />
                                    <asp:Button runat="server" ID="btnAddJHMasterDetails" Text="Add Details" CssClass="bajaj-btn-style bajaj-add-edit-btn-style" OnClick="btnAddJHMasterDetails_Click" />

                                </td>
                            </tr>
                            <tr>
                                <td>Interface ID</td>
                                <td>
                                    <asp:Label runat="server" ID="lblMachineInterfaceID" CssClass="form-control"></asp:Label>
                                </td>
                                <td>Manager</td>
                                <td>
                                    <asp:Label runat="server" ID="lblManager" CssClass="form-control"></asp:Label>
                                </td>
                                <td>Group Leader</td>
                                <td>
                                    <asp:Label runat="server" ID="lblGroupLeader" CssClass="form-control"></asp:Label>
                                </td>
                                <td colspan="7">
                                    <asp:Button runat="server" ID="btnNewRevisin" OnClick="btnNewRevisin_Click" CssClass="bajaj-btn-style" Text="Create New Rev  No." />
                                    <asp:Button runat="server" ID="btnCopyToMachine" OnClick="btnCopyToMachine_Click" CssClass="bajaj-btn-style" Text="Copy To Machine" />
                                    <asp:Button runat="server" ID="btnAddEditRevLevelData" OnClick="btnAddEditRevLevelData_Click" CssClass="bajaj-btn-style" Text="Add/Edit Rev. Level Details" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="scrollMaintainDiv" style="height: 76vh; overflow: auto; margin-top: 12px">
                    <asp:ListView runat="server" ID="lvJHMasterDetails" ClientIDMode="Static" OnDataBound="lvJHMasterDetails_DataBound">
                        <EmptyDataTemplate>
                            <table class="table table-bordered  headerFixer" id="tblJHMasterDetails">
                                <tr>
                                    <th>Check Point No.</th>
                                    <th>Route No.</th>
                                    <th>Related To</th>
                                    <th>Freq.</th>
                                    <th>C, L, I, RT</th>
                                    <th>Item</th>
                                    <th>Check Point</th>
                                    <th>Standard</th>
                                    <th>If Not Ok</th>
                                    <th>Method</th>
                                    <th>Day</th>
                                    <th>Machine Condition</th>
                                    <th>Time (Sec)</th>
                                    <th>Remark</th>
                                    <th>Drawing</th>
                                    <th>Min.</th>
                                    <th>Max.</th>
                                    <th>Unit</th>
                                    <th>Method Type</th>
                                    <th>Parameter</th>
                                    <th>Data Type</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr>
                                    <td colspan="30" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table class="table table-bordered table-hover headerFixer bajaj-table-style" id="tblJHMasterDetails">
                                <tr>
                                    <th>Check Point No.</th>
                                    <th>Route No.</th>
                                    <th>Related To</th>
                                    <th>Freq.</th>
                                    <th>C, L, I, RT</th>
                                    <th>Item</th>
                                    <th>Check Point</th>
                                    <th>Standard</th>
                                    <th>If Not Ok</th>
                                    <th>Method</th>
                                    <th>Day</th>
                                    <th>Machine Condition</th>
                                    <th>Time (Sec)</th>
                                    <th>Remark</th>
                                    <th>Drawing</th>
                                    <th>Min.</th>
                                    <th>Max.</th>
                                    <th>Unit</th>
                                    <th>Method Type</th>
                                    <th>Parameter</th>
                                    <th>Data Type</th>
                                    <th runat="server" id="thAction">Action</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>

                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblCheckPointNo" Text='<%# Eval("CheckPointNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblRouteNo" Text='<%# Eval("RouteNo") %>'></asp:Label>
                                </td>
                                <td>
                                    <img src='<%# Eval("RelatedToImagePath") %>' style="height: 25px" title='<%# Eval("RelatedTo") %>' />
                                    <%--                  <br />
                                    <asp:Label runat="server" ID="lblRelatedTo" Text='<%# Eval("RelatedTo") %>'></asp:Label>--%>
                                    <asp:HiddenField runat="server" ID="hfRelatedTo" Value='<%# Eval("RelatedTo") %>' />

                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblFrequency" Text='<%# Eval("Frequency") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblC_L_I_RT_Value" Text='<%# Eval("C_L_I_RT_Value") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblItem" Text='<%# Eval("Item") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblCheckPoint" Text='<%# Eval("CheckPoint") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblStandard" Text='<%# Eval("Standard") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblIfNotOk" Text='<%# Eval("IfNotOk") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMethod" Text='<%# Eval("Method") %>'></asp:Label>
                                </td>
                                <td runat="server" bgcolor='<%# Eval("DayColor") %>'>
                                    <asp:Label runat="server" ID="lblDay" Text='<%# Eval("DayToDisplay") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hfDay" Value='<%# Eval("Day") %>' />
                                    <asp:HiddenField runat="server" ID="hfMonth" Value='<%# Eval("Month") %>' />
                                    <asp:HiddenField runat="server" ID="hfDayNo" Value='<%# Eval("DayNo") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMachineCondition" Text='<%# Eval("MachineCondition") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblTime" Text='<%# Eval("Time") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblRemarks" Text='<%# Eval("Remarks") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton runat="server" ID="lbDrawing" Text='<%# Eval("DrawingName") %>' CssClass="anchor-highlight-color" ClientIDMode="Static" OnClientClick="return showDrawing(this);"></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hfDrawingInBase64" ClientIDMode="Static" Value='<%# Eval("DrawingInBase64") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMin" Text='<%# Eval("Min") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMax" Text='<%# Eval("Max") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblUnit" Text='<%# Eval("Unit") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblMethodType" Text='<%# Eval("MethodType") %>'></asp:Label>
                                </td>

                                <td>
                                    <asp:Label runat="server" ID="lblParameter" Text='<%# Eval("ParameterName") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hfParameterID" Value='<%# Eval("ParameterID") %>' />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDataType" Text='<%# Eval("DataType") %>'></asp:Label>
                                </td>
                                <td style="white-space: nowrap" runat="server" visible='<%# Eval("IsActionRequired") %>'>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton runat="server" ID="lbEdit" CssClass=" bajaj-action-icons bajaj-add-edit-btn-style" ToolTip="Edit" OnClick="lbEdit_Click">
                                            <i class="glyphicon glyphicon-pencil"></i>
                                            <span>EDIT</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lbDelete" CssClass="bajaj-action-icons bajaj-delete-icons" ToolTip="Delete" OnClick="lbDelete_Click">
                                            <i class="glyphicon glyphicon-trash "></i>
                                           <span>DELETE</span> 
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lbEdit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lbDelete" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

                <div class="modal infoModal bajaj-info-modal" id="neweditJHModal" role="dialog" style="min-width: 1131px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 1131px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title" id="modalTitle" runat="server"></h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('neweditJHModal','compare'); SetControlValueNull('hfFromCreateRevision');"></i>
                                <asp:HiddenField runat="server" ID="hfJHNewEdit" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hfFromCreateRevision" ClientIDMode="Static" />
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Rev No. *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevNo" CssClass="form-control txtstyle" ClientIDMode="Static"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hfRevID" />
                                            </td>
                                            <td>Rev Date *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevDate" CssClass="form-control txtstyle" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Check Point No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCheckPointNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowNumber"></asp:TextBox>
                                            </td>
                                            <td>Route No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRouteNo" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowNumber"></asp:TextBox>
                                            </td>
                                            <td>Related To</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlRelatedTo" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Frequency *</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlFrequency" ClientIDMode="Static" CssClass="form-control txtstyle" AutoPostBack="true" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                            <td>C, L, I, RT</td>
                                            <td>
                                                <asp:CheckBoxList runat="server" ID="cblC_L_I_RT_Value" ClientIDMode="Static" CssClass="check-box-list" RepeatDirection="Horizontal">
                                                </asp:CheckBoxList>
                                            </td>
                                            <td>Item</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtItem" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Check Point *</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCheckPoint" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Standard</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtStandard" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>If Not Ok</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtIfNotOk" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Method</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMethod" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                            <td>Day</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlDayNo" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                                <asp:DropDownList runat="server" ID="ddlDay" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                                <asp:TextBox runat="server" ID="txtMonth" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblDay" ClientIDMode="Static" CssClass="form-control txtstyle" Text="Daily"></asp:Label>
                                            </td>
                                            <td>Machine Condition</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMachineCondition" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Time (Sec)</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTime" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowNumber"></asp:TextBox>
                                            </td>
                                            <td>Remarks</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRemarks" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                             <td>Drawing</td>
                                            <td>
                                                <asp:FileUpload runat="server" ID="fuDrawing" ClientIDMode="Static" CssClass="form-control txtstyle" />
                                                <asp:Label runat="server" ID="lblDrawingName" ClientIDMode="Static"></asp:Label>
                                                <i id="clearDrawing" runat="server" title="Remove Drawing" clientidmode="static" class="glyphicon glyphicon-remove" style="color: red; display: inline-block; margin-left: 10px; vertical-align: middle;" onclick="clearUploadedDrawing()"></i>

                                                <asp:HiddenField runat="server" ID="hfDrawing" ClientIDMode="Static" />
                                                <asp:HiddenField runat="server" ID="hfDrawingName" ClientIDMode="Static" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Min</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMin" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowDecimalWithOperator"></asp:TextBox>
                                            </td>
                                            <td>Max</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMax" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle allowDecimalWithOperator"></asp:TextBox>
                                            </td>
                                            <td>Unit</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtUnit" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control txtstyle"></asp:TextBox>
                                            </td>
                                           

                                        </tr>
                                        <tr>
                                            <td>Method Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlMethodType" ClientIDMode="Static" CssClass="form-control txtstyle" onchange="MethodTypeChange();"></asp:DropDownList>
                                            </td>
                                            <td>Parameter</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlParameter" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                            </td>
                                            <td>Data Type</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlDataType" ClientIDMode="Static" CssClass="form-control txtstyle"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('neweditJHModal','compare');  SetControlValueNull('hfFromCreateRevision');" class="bajaj-btn-style cancel-btn">Cancel</button>
                                <asp:Button runat="server" ID="btnJHDetailsSave" ClientIDMode="Static" Text="Save" CssClass="bajaj-btn-style   bajaj-add-edit-btn-style" OnClientClick="return JHValidation();" OnClick="btnJHDetailsSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="modal infoModal bajaj-info-modal" id="revOrCopyToMcModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 90%">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title" runat="server" id="newRevTemplateModalHeader">Machine Details</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick="storeModalDataBeforeChange('revOrCopyToMcModal','compare');"></i>
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <div>
                                        <table class="modal-tbl addUpdateTbl">
                                            <tr>
                                                <td id="tdMachineLbl" runat="server">Machine</td>
                                                <td id="tdMachineValue" runat="server">
                                                    <asp:DropDownList runat="server" ID="ddlMachine_RevOrCopyToMc" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachine_RevOrCopyToMc_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                                <td id="tdExistingRevOrNewRev" runat="server">
                                                    <asp:CheckBox runat="server" ID="cbNewRevNo" ClientIDMode="Static" Text="New Rev No." AutoPostBack="true" OnCheckedChanged="cbNewRevNo_CheckedChanged" CssClass="check-box-list" />
                                                </td>
                                                <td>Rev No.</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRevNo_RevOrCopyToMc" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                                    <asp:DropDownList runat="server" ID="ddlRevNo_RevOrCopyToMc" CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
                                                </td>
                                                <td id="tdRevDateLbl" runat="server">Rev Date</td>
                                                <td id="tdRevDateValue" runat="server">
                                                    <asp:TextBox runat="server" ID="txtRevDate_RevOrCopyToMc" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField runat="server" ID="hfNewRevOrCopyToMc" ClientIDMode="Static" />
                                    </div>

                                    <div style="width: 100%; max-height: 77vh; overflow: auto">
                                        <asp:ListView runat="server" ID="lvRevOrCopyToMcDetails" ClientIDMode="Static">
                                            <EmptyDataTemplate>
                                                <table class="table table-bordered table-hover headerFixer">
                                                    <tr>
                                                        <th>Check Point No.</th>
                                                        <th>Route No.</th>
                                                        <th>Related To</th>
                                                        <th>Freq.</th>
                                                        <th>C, L, I, RT</th>
                                                        <th>Item</th>
                                                        <th>Check Point</th>
                                                        <th>Standard</th>
                                                        <th>If Not Ok</th>
                                                        <th>Method</th>
                                                        <th>Day</th>
                                                        <th>Machine Condition</th>
                                                        <th>Time (Sec)</th>
                                                        <th>Remark</th>
                                                        <th>Method Type</th>
                                                        <th>Parameter</th>
                                                        <th>Data Type</th>
                                                        <th>Select</th>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" class="no-data-found-td"><span class="no-data-found">No Data Found</span></td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table class="table table-bordered table-hover headerFixer " id="tblRevOrCopyToMcDetails">
                                                    <tr>
                                                        <th>Check Point No.</th>
                                                        <th>Route No.</th>
                                                        <th>Related To</th>
                                                        <th>Freq.</th>
                                                        <th>C, L, I, RT</th>
                                                        <th>Item</th>
                                                        <th>Check Point</th>
                                                        <th>Standard</th>
                                                        <th>If Not Ok</th>
                                                        <th>Method</th>
                                                        <th>Day</th>
                                                        <th>Machine Condition</th>
                                                        <th>Time (Sec)</th>
                                                        <th>Remark</th>
                                                        <th>Method Type</th>
                                                        <th>Parameter</th>
                                                        <th>Data Type</th>
                                                        <th style="white-space: nowrap">
                                                            <asp:CheckBox runat="server" ID="cbSelectAll" ClientIDMode="Static" CssClass="check-box-list" Text="Select" onchange="SelectUnSelectAllCheckBox(this,'tblRevOrCopyToMcDetails' )" />
                                                        </th>
                                                    </tr>
                                                    <tr id="itemplaceholder" runat="server"></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCheckPointNo" Text='<%# Eval("CheckPointNo") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblRouteNo" Text='<%# Eval("RouteNo") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <img src='<%# Eval("RelatedToImagePath") %>' style="height: 25px" title='<%# Eval("RelatedTo") %>' />
                                                        <%-- <br />
                                                        <asp:Label runat="server" ID="lblRelatedTo" Text='<%# Eval("RelatedTo") %>'></asp:Label>--%>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblFrequency" Text='<%# Eval("Frequency") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblC_L_I_RT_Value" Text='<%# Eval("C_L_I_RT_Value") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblItem" Text='<%# Eval("Item") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCheckPoint" Text='<%# Eval("CheckPoint") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblStandard" Text='<%# Eval("Standard") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblIfNotOk" Text='<%# Eval("IfNotOk") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblMethod" Text='<%# Eval("Method") %>'></asp:Label>
                                                    </td>
                                                    <td runat="server" bgcolor='<%# Eval("DayColor") %>'>
                                                        <asp:Label runat="server" ID="lblDay" Text='<%# Eval("DayToDisplay") %>'></asp:Label>

                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblMachineCondition" Text='<%# Eval("MachineCondition") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTime" Text='<%# Eval("Time") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblRemarks" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblMethodType" Text='<%# Eval("MethodType") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblParameter" Text='<%# Eval("ParameterName") %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hfParameterID" Value='<%# Eval("ParameterID") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDataType" Text='<%# Eval("DataType") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="cbSelect" CssClass="" onchange="SetUnsetSelectAll(this,'tblRevOrCopyToMcDetails')" />
                                                    </td>

                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('revOrCopyToMcModal','compare');" class="bajaj-btn-style cancel-btn">Cancel</button>
                                <asp:Button runat="server" ID="btnSaveNewRevOrCopyToMc" ClientIDMode="Static" Text="SAVE" CssClass=" bajaj-btn-style  bajaj-add-edit-btn-style" OnClientClick="return CreateRevNoValidation('tblRevOrCopyToMcDetails');" OnClick="btnSaveNewRevOrCopyToMc_Click" />
                                <asp:Button runat="server" ID="btnCreateNewRev" ClientIDMode="Static" Text="Create New Rev" CssClass=" bajaj-btn-style  bajaj-add-edit-btn-style" OnClick="btnCreateNewRev_Click" />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="modal fade" id="deleteConfirmModal" role="dialog">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content modalContent confirm-modal-content">
                            <div class="modal-header modalHeader confirm-modal-header">
                                <i class="glyphicon glyphicon glyphicon glyphicon-question-sign modal-icons"></i>
                                <br />
                                <h4 class="confirm-modal-title">Confirmation!</h4>
                                <br />
                                <span id="DeleteMsg" class="confirm-modal-msg">Are you sure you want to delete Record?</span>
                            </div>
                            <div class="modal-footer modalFooter modal-footer" style="margin-top: 0px">
                                <asp:Button runat="server" Text="Yes" ID="btnDeleteConfirm" CssClass="confirm-modal-btn" OnClick="btnDeleteConfirm_Click" ClientIDMode="Static" />
                                <input type="button" value="No" data-dismiss="modal" class="confirm-modal-btn" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal infoModal  bajaj-info-modal" id="neweditRevLevelModal" role="dialog" style="min-width: 300px;" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered " style="width: 900px">
                        <div class="modal-content modalThemeCss">
                            <div class="modal-header">
                                <h4 class="modal-title">Enter Details</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" onclick=" storeModalDataBeforeChange('neweditRevLevelModal','compare');"></i>
                            </div>
                            <div class="modal-body" style="padding-left: 0px; padding-right: 0px">
                                <div style="padding-left: 15px; padding-right: 15px; padding-top: 8px;">
                                    <table style="width: 100%; margin: auto" class="modal-tbl addUpdateTbl">
                                        <tr>
                                            <td>Manager</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlManager" CssClass="form-control txtstyle" ClientIDMode="Static"></asp:DropDownList>
                                            </td>
                                            <td>Group Leader</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlGroupLeader" CssClass="form-control txtstyle" ClientIDMode="Static"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" onclick="storeModalDataBeforeChange('neweditRevLevelModal','compare');" class="bajaj-btn-style  cancel-btn">Cancel</button>
                                <asp:Button runat="server" ID="btnRevLevelDataSave" ClientIDMode="Static" Text="Save" CssClass=" bajaj-btn-style bajaj-add-edit-btn-style" OnClick="btnRevLevelDataSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="modal infoModal bajaj-info-modal" id="showDrawingModal" role="dialog" style="min-width: 300px;">
                    <div class="modal-dialog modal-dialog-centered" style="width: 50%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Drawing</h4>
                                <i class="glyphicon glyphicon-remove modal-close-icon" data-dismiss="modal" onclick="closeLargeDocumentModal()"></i>
                            </div>
                            <div class="modal-body">
                                <div style="height: 70vh">
                                    <iframe id="iframeDocument" style="width: 100%; height: 100%"></iframe>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <input type="button" value="Close" class="bajaj-btn-style" data-dismiss="modal" onclick="closeLargeDocumentModal()" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            DateTimeSetter();
        });
        function DateTimeSetter() {
            //$('[id$=txtDate]').datepicker({
            //    viewMode: "date",
            //    minViewMode: "date",
            //    format: 'dd-mm-yyyy',
            //    todayHighlight: true,
            //    autoclose: true,
            //    language: 'en-US',
            //    //startDate: '-1d'
            //});
            $('[id$=txtRevDate]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
            $('[id$=txtRevDate_RevOrCopyToMc]').datepicker({
                viewMode: "date",
                minViewMode: "date",
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
            $('[id$=txtMonth]').datepicker({
                viewMode: "months",
                minViewMode: "months",
                format: 'mm',
                todayHighlight: true,
                autoclose: true,
                language: 'en-US',
                //startDate: '-1d'
            });
        }
        function JHValidation() {
            if ($("#txtRevNo").attr('disabled') != "disabled") {
                if ($("#txtRevNo").val() == "") {
                    toasterWarningMsg("Rev No. is required.", "");
                    return false;
                }
            }
            if ($("#txtRevDate").attr('disabled') != "disabled") {
                if ($("#txtRevDate").val() == "") {
                    toasterWarningMsg("Rev Date is required.", "");
                    return false;
                }
            }
            if ($('#txtCheckPoint').val() == "") {
                toasterWarningMsg("Check Point is required.", "");
                return false;
            }
            if ($('#ddlFrequency').val() == "" || $('#ddlFrequency').val() == null) {
                toasterWarningMsg("Frequency is required.", "");
                return false;
            }
            if ($('#ddlFrequency').val() == "Q") {
                if ($('#txtMonth').val() == "") {
                    toasterWarningMsg("Month is required.", "");
                    return false;
                }
            }
            if ($("#ddlMethodType").val() == "Auto") {
                if ($('#ddlParameter').val() == "" || $('#ddlParameter').val() == null) {
                    toasterWarningMsg("Parameter is required.", "");
                    return false;
                }
            }
            var fileInput = document.getElementById('fuDrawing');
            if (fileInput.files[0] != undefined) {
                var reader = new FileReader();
                reader.readAsDataURL(fileInput.files[0]);
                reader.onload = function () {
                    $("#hfDrawing").val(reader.result);
                    $("#hfDrawingName").val($("#fuDrawing").val().split('\\').pop());
                    __doPostBack('<%= btnJHDetailsSave.UniqueID%>', '');
                }
            } else {
                __doPostBack('<%= btnJHDetailsSave.UniqueID%>', '');
            }
            return false;
        }
        $("#fuDrawing").change(function () {
            var fileExtension = ['pdf', 'png', 'jpeg', 'img'];

            var fileSize = 1048576 * 3;
            var fileInput = document.getElementById('fuDrawing');
            if (fileInput.files[0].size > fileSize) {
                toasterWarningMsg("File should be less than 3MB.", "");
                $("#fuDrawing").val("");
                return;
            }
            if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                toasterWarningMsg("Only pdf file can upload", "");
                $("#fuDrawing").val("");
            }
        });
        function closeLargeDocumentModal() {
            $('#iframeDocument').attr("src", "");
        }
        function showDrawing(element) {
            let name = $(element).closest('tr').find("#lbDrawing").text();
            let extension = name.substring(name.lastIndexOf(".") + 1);
            var arrrayBuffer = base64ToArrayBuffer($(element).closest('tr').find("#hfDrawingInBase64").val()); //data is the base64 encoded 
            if (extension == "pdf") {
                var blob = new Blob([arrrayBuffer], { type: "application/pdf" });
                var link = window.URL.createObjectURL(blob);
                $('#iframeDocument').attr("src", link + "#toolbar=0");
            } else {
                var blob = new Blob([arrrayBuffer]);
                $('#iframeDocument').attr("src", "data:image/png;base64," + $(element).closest('tr').find("#hfDrawingInBase64").val());
            }

            $("#showDrawingModal").modal('show');
            return false;
        }
        function base64ToArrayBuffer(base64) {
            var binaryString = window.atob(base64);
            var binaryLen = binaryString.length;
            var bytes = new Uint8Array(binaryLen);
            for (var i = 0; i < binaryLen; i++) {
                var ascii = binaryString.charCodeAt(i);
                bytes[i] = ascii;
            }
            return bytes;
        }
        function clearUploadedDrawing() {
            $("#hfDrawing").val("");
            $("#hfDrawingName").val("");
            //$('#fuDrawing').val("");
            $("#lblDrawingName").text("");
            $("#clearDrawing").css('display', 'none');
        }
        function CreateRevNoValidation(gridId) {
            if ($("#hfNewRevOrCopyToMc").val() == "CopyToMachine") {
                if ($('#ddlMachine_RevOrCopyToMc').val() == "" || $('#ddlMachine_RevOrCopyToMc').val() == null) {
                    toasterWarningMsg("Machine is required.", "");
                    return false;
                }

                if ($("#cbNewRevNo").length > 0) {
                    if ($("#cbNewRevNo").prop('checked')) {
                        if ($('#txtRevNo_RevOrCopyToMc').val() == "") {
                            toasterWarningMsg("Rev No. is required.", "");
                            return false;
                        }
                        if ($('#txtRevDate_RevOrCopyToMc').val() == "") {
                            toasterWarningMsg("Rev Date is required.", "");
                            return false;
                        }
                    } else {
                        if ($('#ddlRevNo_RevOrCopyToMc').val() == "" || $('#ddlRevNo_RevOrCopyToMc').val() == null) {
                            toasterWarningMsg("Rev No. is required.", "");
                            return false;
                        }
                    }


                } else {
                    if ($('#txtRevNo_RevOrCopyToMc').val() == "") {
                        toasterWarningMsg("Rev No. is required.", "");
                        return false;
                    }
                    if ($('#txtRevDate_RevOrCopyToMc').val() == "") {
                        toasterWarningMsg("Rev Date is required.", "");
                        return false;
                    }
                }

            } else {
                if ($('#txtRevNo_RevOrCopyToMc').val() == "") {
                    toasterWarningMsg("Rev No. is required.", "");
                    return false;
                }
                if ($('#txtRevDate_RevOrCopyToMc').val() == "") {
                    toasterWarningMsg("Rev Date is required.", "");
                    return false;
                }
            }


            if ($("#" + gridId + " tr td:last-child input[type='checkbox']:checked").length > 0) {
                return true;
            }
            toasterWarningMsg("Please select any one row.", "");
            return false;
        }
        function MethodTypeChange() {
            if ($('#ddlMethodType').val() == "Auto") {
                $("#ddlParameter").removeAttr('disabled');
            } else {
                $("#ddlParameter").attr('disabled', 'disabled');
            }
        }
        function openDeleteConfirmModal(msg) {
            $("#DeleteMsg").text(msg);
            openDeleteModal('deleteConfirmModal');
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            $(document).ready(function () {
                DateTimeSetter();
            });
            function openDeleteConfirmModal(msg) {
                $("#DeleteMsg").text(msg);
                openDeleteModal('deleteConfirmModal');
            }
            $("#fuDrawing").change(function () {
                var fileExtension = ['pdf','png','jpeg','img'];

                var fileSize = 1048576 * 3;
                var fileInput = document.getElementById('fuDrawing');
                if (fileInput.files[0].size > fileSize) {
                    toasterWarningMsg("File should be less than 3MB.", "");
                    $("#fuDrawing").val("");
                    return;
                }
                if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                    toasterWarningMsg("Only pdf file can upload", "");
                    $("#fuDrawing").val("");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
