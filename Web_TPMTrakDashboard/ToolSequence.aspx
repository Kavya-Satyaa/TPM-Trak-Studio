<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ToolSequence.aspx.cs" Inherits="Web_TPMTrakDashboard.ToolSequence" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #gvToolContainer {
            overflow: auto;
        }

            #gvToolContainer table tr th {
                position: sticky;
                top: 0px;
            }

        fieldset {
            border: 1px solid #4f4e63;
            padding: 0px;
            border-radius: 4px;
            width: auto;
            /*box-shadow: 2px 2px 8px 2px #efe7e7;*/
        }

        .masterFS {
            margin: 0 8px 0 8px;
            padding: 0 10px 5px 10px;
            width: 70%;
        }

        legend {
            text-align: left;
            color: white;
            display: block;
            width: auto;
            padding: 0;
            margin-bottom: 5px;
            font-size: 15px;
            line-height: inherit;
            border-bottom: transparent;
        }

        .toolLabelCss {
            font-weight: bold;
            color: white;
        }

        #tblToolInfo {
            width: 100%;
        }

            #tblToolInfo tr td {
                padding: 5px;
            }

                #tblToolInfo tr td:nth-child(3), #tblToolInfo tr td:nth-child(5) {
                    padding-left: 20px;
                }

        .HeaderCss th {
            color: white;
            background-color: #2E6886 !important;
            height: 45px;
            vertical-align: inherit;
        }

        .blueLinkBtns, .blueLinkBtns:hover {
            color: deepskyblue;
            font-size: 20px;
        }

        .redLinkBtns, .redLinkBtns:hover {
            color: red;
            font-size: 20px;
        }

        .commanTd {
            vertical-align: middle !important;
        }

        #gvToolContainer {
            min-width:;
            max-height: 80vh;
            overflow: auto;
        }

        .lvToolData > tbody > tr > td, th {
            border: 1px solid silver;
        }

        .lvToolData > tbody > tr > td {
            color: black;
        }

            .lvToolData > tbody > tr > td:last-child {
                min-width: 80px;
                max-width: 80px;
            }

        .lvToolData > tbody > tr:nth-child(odd) > td {
            background-color: white;
        }

        .lvToolData > tbody > tr:nth-child(even) > td {
            background-color: #ddd;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div class="container-fluid">
                <div class="row">
                    <div class="col-lg-12 col-sm-12 col-md-12">
                        <table id="tblfilter" class="table table-bordered" style="width: auto;">
                            <tr>
                                <td class="commanTd" style="min-width: 50px; height: 50px">Machine ID</td>
                                <td style="min-width: 160px;">
                                    <asp:DropDownList runat="server" ID="ddlMachineID" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="commanTd" style="min-width: 50px; height: 50px">Component ID</td>
                                <td style="min-width: 160px;">
                                    <asp:DropDownList runat="server" ID="ddlComponentID" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlComponentID_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="commanTd" style="min-width: 50px; height: 50px">Operation Number</td>
                                <td style="min-width: 160px;">
                                    <asp:DropDownList runat="server" ID="ddlOperationNo" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button runat="server" Text="View" CssClass="btn btn-info btn-sm displayCss" ID="btnView" OnClick="btnView_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>

                        <div>
                            <div runat="server" visible="false">
                                <fieldset class="masterFS" style="margin: 0 4px 0 4px;">
                                    <legend>Tool Information</legend>
                                    <div style="width: 100%">
                                        <table id="tblToolInfo">
                                            <tr>
                                                <td>
                                                    <label class="toolLabelCss">Tool Number</label></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtToolNumber" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label class="toolLabelCss">Length / Offset</label></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtLengthOffset" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label class="toolLabelCss">Tool Holder</label></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtToolHolder" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="toolLabelCss">Ideal Usage</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtIdealUsage" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label class="toolLabelCss">Tool Description</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtToolDescription" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label class="toolLabelCss">Sequence No.</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtSequenceNum" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="toolLabelCss">RPM</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRPM" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label class="toolLabelCss">Insert After</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInsertAfter" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label class="toolLabelCss">Target</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTarget" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="toolLabelCss">DownID</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlDownID" CssClass="form-control"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="toolLabelCss">Notes</label>
                                                </td>
                                                <td colspan="5">
                                                    <asp:TextBox runat="server" ID="txtNotes" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>


                                    </div>
                                </fieldset>
                            </div>
                            <div id="gvToolContainer" style="margin-top: 2%;">
                                <asp:ListView runat="server" ID="lvToolData" OnItemEditing="lvToolData_ItemEditing" OnItemUpdating="lvToolData_ItemUpdating" OnItemCanceling="lvToolData_ItemCanceling" InsertItemPosition="LastItem">
                                    <LayoutTemplate>
                                        <table class="table headerFixer lvToolData">
                                            <tr>
                                                <th>Sequence</th>
                                                <th>Tool Number</th>
                                                <th>Tool Description</th>
                                                <th>Ideal Usage</th>
                                                <th>Offset</th>
                                                <th>Tool Header</th>
                                                <th>RPM</th>
                                                <th>Target</th>
                                                <th>Down Code</th>
                                                <th>Notes</th>
                                                <th runat="server" id="thToolGPL">Tool GPL</th>
                                                <th runat="server" id="thDepthOfCut">Depth Of Cut</th>
                                                <th runat="server" id="thFeedMMMin">Feed MM/Min</th>
                                                <th runat="server" id="thFeedTooth">Feed Tooth</th>
                                                <th runat="server" id="thNoOfCuttingEdges">No Of Cutting Edges</th>
                                                <th runat="server" id="thNoOfCut">No Of Cut</th>
                                                <th></th>
                                            </tr>
                                            <tr runat="server" id="itemplaceholder"></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:HiddenField runat="server" ID="hdnMachineID" Value='<%# Eval("MachineID") %>' />
                                                <asp:HiddenField runat="server" ID="hdnCompID" Value='<%# Eval("ComponentID") %>' />
                                                <asp:HiddenField runat="server" ID="hdnOperationNumber" Value='<%# Eval("OperationNumber") %>' />
                                                <asp:Label runat="server" ID="lblSequence" Text='<%# Eval("Sequence") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblToolNo" Text='<%# Eval("ToolNumber") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblToolDesc" Text='<%# Eval("ToolDescription") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblIdealUsage" Text='<%# Eval("IdealUsage") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblOffset" Text='<%# Eval("Offset") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblToolHolder" Text='<%# Eval("ToolHolder") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblRPM" Text='<%# Eval("RPM") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblTarget" Text='<%# Eval("Target") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblDowncode" Text='<%# Eval("DownCode") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblNotes" Text='<%# Eval("Notes") %>'></asp:Label>
                                            </td>
                                            <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                                <asp:Label runat="server" ID="lblToolGPL" Text='<%# Eval("ToolGPL") %>'></asp:Label>
                                            </td>
                                            <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                                <asp:Label runat="server" ID="lblDepthOfCut" Text='<%# Eval("DepthOfCut") %>'></asp:Label>
                                            </td>
                                            <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                                <asp:Label runat="server" ID="lblFeedMMMin" Text='<%# Eval("FeedMM_Min") %>'></asp:Label>
                                            </td>
                                            <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                                <asp:Label runat="server" ID="lblFeedTooth" Text='<%# Eval("FeedTooth") %>'></asp:Label>
                                            </td>
                                            <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                                <asp:Label runat="server" ID="lblNoOfCuttingEdges" Text='<%# Eval("NoOfCuttingEdges") %>'></asp:Label>
                                            </td>
                                            <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                                <asp:Label runat="server" ID="lblNoOfCut" Text='<%# Eval("NoOfCut") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnEdit" CommandName="Edit" CssClass="glyphicon glyphicon-edit editLinkBtns blueLinkBtns"></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="btnDelete" CssClass="glyphicon glyphicon-trash deleteLinkBtns redLinkBtns" OnClick="btnDelete_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <td>
                                             <asp:HiddenField runat="server" ID="hdnMachineID" Value='<%# Eval("MachineID") %>' />
                                            <asp:HiddenField runat="server" ID="hdnCompID" Value='<%# Eval("ComponentID") %>' />
                                            <asp:HiddenField runat="server" ID="hdnOperationNumber" Value='<%# Eval("OperationNumber") %>' />
                                            <asp:Label runat="server" ID="lblSequence" Text='<%# Eval("Sequence") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblToolNo" Text='<%# Eval("ToolNumber") %>'></asp:Label>
                                        </td>
                                        <td>
                                           
                                            <asp:TextBox runat="server" ID="txtToolDescEdit" CssClass="form-control" Text='<%# Eval("ToolDescription") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtIdealUsageEdit" CssClass="form-control" Text='<%# Eval("IdealUsage") %>' onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOffsetEdit" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;" CssClass="form-control" Text='<%# Eval("Offset") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtToolHolderEdit" CssClass="form-control" Text='<%# Eval("ToolHolder") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtRPMEdit" CssClass="form-control" Text='<%# Eval("RPM") %>' onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTargetEdit" CssClass="form-control" Text='<%# Eval("Target") %>' onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDowncodeEdit" CssClass="form-control" Text='<%# Eval("DownCode") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNotesEdit" CssClass="form-control" Text='<%# Eval("Notes") %>'></asp:TextBox>
                                        </td>
                                        <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                            <asp:TextBox runat="server" ID="txtToolGPLEdit" CssClass="form-control" Text='<%# Eval("ToolGPL") %>'></asp:TextBox>
                                        </td>
                                        <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                            <asp:TextBox runat="server" ID="txtDepthOfCutEdit" CssClass="form-control" Text='<%# Eval("DepthOfCut") %>'></asp:TextBox>
                                        </td>
                                        <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                            <asp:TextBox runat="server" ID="txtFeedMMMinEdit" CssClass="form-control" Text='<%# Eval("FeedMM_Min") %>'></asp:TextBox>
                                        </td>
                                        <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                            <asp:TextBox runat="server" ID="txtFeedToothEdit" CssClass="form-control" Text='<%# Eval("FeedTooth") %>'></asp:TextBox>
                                        </td>
                                        <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                            <asp:TextBox runat="server" ID="txtNoOfCuttingEdgesEdit" CssClass="form-control" Text='<%# Eval("NoOfCuttingEdges") %>'></asp:TextBox>
                                        </td>
                                        <td runat="server" visible='<%# Eval("VulkanMSColumnsEnable") %>'>
                                            <asp:TextBox runat="server" ID="txtNoOfCutEdit" CssClass="form-control" Text='<%# Eval("NoOfCut") %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="btnUpdate" CommandName="Update" CssClass="glyphicon glyphicon-plus-sign updateLinkBtns blueLinkBtns"></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="LinkButton1" CommandName="Cancel" CssClass="glyphicon glyphicon-remove cancelLinkBtns redLinkBtns"></asp:LinkButton>
                                        </td>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <tr runat="server" id="trInsertItemTemplate">
                                            <td></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtToolNoNew" Text="" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtToolDescNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtIdealUsageNew" CssClass="form-control" Text="" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtOffsetNew" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtToolHolderNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRPMNew" CssClass="form-control" Text="" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTargetNew" CssClass="form-control" Text="" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDowncodeNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNotesNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td runat="server" id="tdToolGPLNew">
                                                <asp:TextBox runat="server" ID="txtToolGPLNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td runat="server" id="tdDepthOfCutNew">
                                                <asp:TextBox runat="server" ID="txtDepthOfCutNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td runat="server" id="tdFeedMMMinNew">
                                                <asp:TextBox runat="server" ID="txtFeedMMMinNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td runat="server" id="tdFeedToothNew">
                                                <asp:TextBox runat="server" ID="txtFeedToothNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td runat="server" id="tdNoOfCuttingEdgesNew">
                                                <asp:TextBox runat="server" ID="txtNoOfCuttingEdgesNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td runat="server" id="tdNoOfCutNew">
                                                <asp:TextBox runat="server" ID="txtNoOfCutNew" CssClass="form-control" Text=""></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="btnInsert" CssClass="glyphicon glyphicon-plus-sign blueLinkBtns" Style="font-size: 28px;" OnClick="btnInsert_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </InsertItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="gridConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #5D7B9D">
                <div class="modal-header" style="background-color: #5D7B9D; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body" style="background-color: white; padding: 20px 10px">
                    <%--<img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="messageText2">Do you really want to delete selected Tool Sequence details?</span>
                </div>
                <div class="modal-footer" style="padding: 6px; border-top: 1px solid #5D7B9D; background-color: white; margin-top: 0px">
                    <asp:Button runat="server" Text="Yes" ID="yesGridBtn" OnClick="yesGridBtn_Click" Style="width: 80px; padding: 3px" />
                    <button type="button" style="width: 80px; padding: 3px" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #5D7B9D">
                <div class="modal-header" style="background-color: #5D7B9D; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Warning!</h4>
                </div>
                <div class="modal-body" style="background-color: white; padding: 20px 10px">


                    <span id="lblWarningMsg"></span>
                </div>
                <div class="modal-footer" style="padding: 6px; border-top: 1px solid #5D7B9D; background-color: white; margin-top: 0px">
                    <button type="button" data-dismiss="modal" style="width: 80px; padding: 3px">OK</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="errorModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #5D7B9D">
                <div class="modal-header" style="background-color: #5D7B9D; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Error!</h4>
                </div>
                <div class="modal-body" style="background-color: white; padding: 20px 10px">


                    <span id="lblErrorMsg"></span>
                </div>
                <div class="modal-footer" style="padding: 6px; border-top: 1px solid #5D7B9D; background-color: white; margin-top: 0px">
                    <button type="button" data-dismiss="modal" style="width: 80px; padding: 3px">OK</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            setGridHeight();
        });
        var bigDiv = document.getElementById('gvToolContainer');
        bigDiv.onscroll = function () {
            $('[id*=hdnScrollPos]').val(bigDiv.scrollTop);
            console.log("id scroll =" + $('[id*=hdnScrollPos]').val());
        }
        window.onload = function () {

            bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            console.log("id load =" + bigDiv.scrollTop);
        }
        function openCloseGridConfirmModal(param) {
            $('[id*=gridConfirmModal]').modal(param);
        }
        function setGridHeight() {
            var wHeight = $(window).height() - 180;
            $('#gvToolContainer').css('height', wHeight);
        }
        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        function openErrorModal(msg) {
            $('#lblErrorMsg').text(msg);
            $('[id*=errorModal]').modal('show');
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {

            var bigDiv = document.getElementById('gvToolContainer');
            $(document).ready(function () {
                setGridHeight();
                debugger;

                bigDiv.scrollTop = $('[id*=hdnScrollPos]').val();
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
