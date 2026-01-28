<%@ Page Title="FPA and CMM Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FPAandCMMData.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.FPAandCMMData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .divSegment {
            position: relative;
            background: white;
            -webkit-box-shadow: 0px 1px 2px 0 rgba(34, 36, 38, 0.15);
            box-shadow: 0px 1px 2px 0 rgba(34, 36, 38, 0.15);
            margin: auto 1rem 0em;
            color: black;
            padding: 1em 1em;
            margin: auto;
            width: 90%;
            border-radius: 0.28571429rem;
            border: 1px solid rgba(34, 36, 38, 0.15);
        }

        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
        }

            /* Hide default HTML checkbox */
            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        /* The slider */
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
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
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
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
            background-color: darkblue;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        #gridContainer table tr:first-child th {
            background-color: #2e6886 !important;
        }

        #gridContainer table tr:nth-child(odd) {
            background-color: green;
            color: black;
        }

        #gridContainer table tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        .btnDisabled input[type="submit"] {
            background-color: #85d5ec !important;
        }

        .btnHide input[type="submit"] {
            display: none;
        }

        .btn-info {
            background-color: #1d98bd;
        }

        #gridContainer a {
            color: #086cc1;
        }
    </style>
    <div class="">
        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="LinkFPA" />
                <asp:PostBackTrigger ControlID="btnImportLayout" />
                <asp:PostBackTrigger ControlID="LinkLayout" />
                <asp:PostBackTrigger ControlID="btnFPAImport" />
                <asp:PostBackTrigger ControlID="linkCMM" />
                <%--    <asp:PostBackTrigger ControlID="fileuploadFPAfile" />
                <asp:PostBackTrigger ControlID="fileuploadLayoutfile" />--%>
            </Triggers>
            <ContentTemplate>

                <div style="text-align: center; width: 100%" class="divSegment">
                    <table>
                        <tr>
                            <td style="color: black; font: 30px">
                                <span style="font-size: 30px;"><b>Part Name :</b></span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="drpComponentID" Style="max-width: 300px; min-width: 150px; display: inline; font-size: 20px; height: 45px; padding-bottom: 9px" placeholder="Part Number..." CssClass="form-control" />
                            </td>
                            <td style="color: black; font: 30px">
                                <span style="font-size: 30px;"><b>Serial Number :</b></span>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSlnoSearch" ClientIDMode="Static" Style="max-width: 300px; min-width: 150px; display: inline; font-size: 20px; height: 45px; padding-bottom: 9px" placeholder="Serial Number..." CssClass="form-control" onkeypress="scanSlNo(event)" onkeydown="manuallyEnterSlNo()"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" CssClass="btn btn-primary" Text="View" BackColor="mediumvioletred" Width="130" OnClick="btnView_Click" />
                            </td>

                            <td style="float: right; text-align: right; margin-left: 20px;">
                                <table>
                                    <tr>
                                        <td>
                                            <span><b>Manual</b></span>
                                        </td>
                                        <td>

                                            <label class="switch">
                                                <asp:CheckBox runat="server" ID="chktype" OnCheckedChanged="chktype_CheckedChanged" AutoPostBack="true"/>
                                                <span class="slider round"></span>
                                            </label>
                                        </td>
                                        <td>
                                            <span><b>Automatic </b></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </div>
                <br />
                <div class="divSegment">
                    <table>
                        <%--  <tr style="height: 60px;">
                            <td style="width: 120px">
                                <span><b>Part Name :</b></span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblPartName" CssClass="form-control" />
                            </td>
                            <td style="width: 130px">
                                <span><b>Serial Number :</b></span>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSerialNumber" CssClass="form-control" />
                            </td>
                        </tr>--%>
                        <tr style="height: 60px;">
                            <td style="width: 120px">
                                <span><b>FPA File :</b></span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="min-width: 500px; max-width: 800px">
                                            <asp:LinkButton runat="server" ID="LinkFPA" OnClick="LinkFPA_Click" />
                                        </td>
                                        <td style="min-width: 150px;">
                                            <asp:Button runat="server" ID="btnFPAImport" Text="Upload FPA" CssClass="btn btn-outline-primary" OnClick="btnFPAImport_Click" Width="130" />
                                        </td>
                                        <td>
                                            <asp:FileUpload runat="server" ID="fileuploadFPAfile" Width="210" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px">
                                <span><b>Layout File :</b></span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="min-width: 500px; max-width: 800px">
                                            <asp:LinkButton runat="server" ID="LinkLayout" OnClick="LinkLayout_Click" Style="min-width: 300px; max-width: 500px" />
                                        </td>
                                        <td style="min-width: 150px;">
                                            <asp:Button runat="server" ID="btnImportLayout" Text="Upload Layout" CssClass="btn btn-outline-primary" OnClick="btnImportLayout_Click" Width="130" />
                                        </td>
                                        <td>
                                            <asp:FileUpload runat="server" ID="fileuploadLayoutfile" Width="210" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 60px">
                            <td>
                                <span><b>CMM File :</b></span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="min-width: 500px; max-width: 800px">
                                            <asp:LinkButton runat="server" ID="linkCMM" OnClick="linkCMM_Click" />
                                        </td>
                                        <td style="min-width: 150px;">
                                            <asp:Button runat="server" ID="btnCMM" OnClick="btnCMM_Click" Text="Send To CMM" CssClass="btn btn-info" Style="float: right" Width="130" />
                                            <b><asp:Label runat="server" ID="lblCMMStatus" CssClass="form-control" Style="max-width: 150px; min-width: 100px; display: inline; font-size: 18px; height: 35px; padding-bottom: 9px" Text="New" /></b></b>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 60px">
                            <td>
                                <span><b>Status :</b></span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="min-width: 500px; max-width: 800px">
                                            <b>
                                                <asp:Label runat="server" ID="lblStatus" CssClass="form-control" Style="max-width: 150px; min-width: 100px; display: inline; font-size: 18px; height: 35px; padding-bottom: 9px" Text="New" /></b>
                                        </td>
                                        <td style="min-width: 150px;">
                                            <asp:Button runat="server" ID="btnApprove" OnClick="btnApprove_Click" Text="Approve" CssClass="btn btn-success" Width="130" /></td>
                                        <td style="min-width: 150px;">
                                            <asp:Button runat="server" ID="btnReject" OnClick="btnReject_Click" Text="Reject" CssClass="btn btn-danger" Width="130" />
                                        </td>
                            </td>

                        </tr>
                    </table>
                    </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <div id="infomodal" class="modal" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Information</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <p><span id="body"></span></p>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--   <div id="gridContainer" style="height:80vh;">
             <asp:GridView runat="server" ID="gvFPAData" AutoGenerateColumns="false" CssClass="table table-bordered table-hover headerFixer" ClientIDMode="Static" OnRowDataBound="gvFPAData_RowDataBound">
                 <Columns>
                     <asp:TemplateField  HeaderText="Machine ID">
                         <ItemTemplate>
                             <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>'></asp:Label>
                         </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField  HeaderText="Operation">
                         <ItemTemplate>
                             <asp:Label runat="server" ID="lblOperation" Text='<%# Eval("OperationID") %>'></asp:Label>
                         </ItemTemplate>
                     </asp:TemplateField>
                       <asp:TemplateField  HeaderText="Part Name">
                         <ItemTemplate>
                             <asp:Label runat="server" ID="lblPartName" Text='<%# Eval("PartName") %>'></asp:Label>
                         </ItemTemplate>
                     </asp:TemplateField>
                      <asp:TemplateField  HeaderText="Serial Number">
                         <ItemTemplate>
                             <asp:Label runat="server" ID="lblSlno" Text='<%# Eval("SerialNumber") %>'></asp:Label>
                         </ItemTemplate>
                     </asp:TemplateField>
                       <asp:TemplateField  HeaderText="FPA File">
                         <ItemTemplate>
                             <asp:LinkButton runat="server" ID="lnkFPAFile" Text='<%# Eval("FPAFile") %>'></asp:LinkButton>
                             &nbsp;&nbsp;&nbsp;
                                <a runat="server" id="Button1" onclick="return FPAFileUploadClick(this);" style="height: 35px; display: inline-block; text-decoration: none" class="Btns">
                            <i class="glyphicon glyphicon-floppy-open"></i>&nbsp;Upload FPA File </a>
                        <asp:FileUpload runat="server" ID="fuFPAFile" ClientIDMode="Static" AllowMultiple="false" CssClass="form-control Btns" Style="color: #454444; display: none" />
                               <asp:Button runat="server" ID="saveFPAFile" UseSubmitBehavior="false" OnClick="saveFPAFile_Click" CssClass="Btns" Style="margin-left: 4px; display: none" Text="Save File" />
                         </ItemTemplate>
                     </asp:TemplateField>
                       <asp:TemplateField  HeaderText="Layout File">
                         <ItemTemplate>
                             <asp:LinkButton runat="server" ID="lnkLayoutFile" Text='<%# Eval("LayoutFile") %>'></asp:LinkButton>
                               &nbsp;&nbsp;&nbsp;
                                <a runat="server"  onclick="return LayoutFileUploadClick(this);" style="height: 35px; display: inline-block; text-decoration: none" class="Btns">
                            <i class="glyphicon glyphicon-floppy-open"></i>&nbsp;Upload Layout File </a>
                        <asp:FileUpload runat="server" ID="fuLayoutFile" ClientIDMode="Static" AllowMultiple="false" CssClass="form-control Btns" Style="color: #454444; display: none" />
                               <asp:Button runat="server" ID="saveLayoutFile" UseSubmitBehavior="false" OnClick="saveLayoutFile_Click" CssClass="Btns" Style="margin-left: 4px; display: none" Text="Save File" />
                         </ItemTemplate>
                     </asp:TemplateField>
                       <asp:TemplateField  HeaderText="Send to CMM">
                         <ItemTemplate>
                              <asp:LinkButton runat="server" ID="lnkCMMFile" Text='<%# Eval("CMMFile") %>'></asp:LinkButton>
                             <div class='<%# Eval("SendToCMMStatus") %>'>
                             <asp:Button runat="server" ID="btnSendToCMM" ClientIDMode="Static" Text="Send To CMM"  CssClass="btn btn-info"/>
                                 </div>
                         </ItemTemplate>
                     </asp:TemplateField>
                      <asp:TemplateField  HeaderText="Approved">
                         <ItemTemplate>
                             <div class='<%# Eval("ApprovedStatus") %>'>
                             <asp:Button runat="server" ID="btnApproved" ClientIDMode="Static" Text="Approved" CssClass="btn btn-info"/>
                                   </div>
                         </ItemTemplate>
                     </asp:TemplateField>
                      <asp:TemplateField  HeaderText="Reject">
                         <ItemTemplate>
                             <div class='<%# Eval("RejectStation") %>'>
                             <asp:Button runat="server" ID="btnRejection" ClientIDMode="Static" Text="Reject"  CssClass="btn btn-info"/>
                                   </div>
                         </ItemTemplate>
                     </asp:TemplateField>
                 </Columns>
             </asp:GridView>
         </div>--%>
    </div>
    <script>
        $(document).ready(function () {
            //ShowPopup(Hello);
        });
        //function FPAFileUploadClick(lnk) {
        //    debugger;
        //    $(lnk).closest('td').find('#fuFPAFile').click();
        //    //   document.getElementById("fuFPAFile").click();
        //    return false;
        //}
        //function LayoutFileUploadClick(lnk) {
        //    debugger;
        //    $(lnk).closest('td').find('#fuLayoutFile').click();
        //    return false;
        //}
        //function UploadFPAFile(fileUpload) {
        //    debugger;
        //    if (fileUpload.value != '') {
        //        $(fileUpload).closest("tr").find("#saveFPAFile").click();

        //    }
        //}
        //function UploadLayoutFile(fileUpload) {
        //    debugger;
        //    if (fileUpload.value != '') {
        //        $(fileUpload).closest("tr").find("#saveLayoutFile").click();

        //    }
        //}
        function ShowPopup(msg) {
            $("#infomodal").show();
            $("#body").val(msg);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
