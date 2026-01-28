<%@ Page Title="Andon Setting" Language="C#" AutoEventWireup="true" CodeBehind="AndonSetting.aspx.cs" Inherits="Web_TPMTrakDashboard.Nippon.AndonSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/UIBlocker/JavaScriptUIBlocker.js"></script>
    <script src="Scripts/toastr.min.js"></script>
    <link href="Scripts/toastr.min.css" rel="stylesheet" />
    <style>
        .gridCss {
            width: 100%;
        }

            .gridCss tr th {
                font-weight: 600;
                font-size: 18px;
                color: #662594;
                color: white;
                background-color: #3777bc;
                padding: 5px;
            }

            .gridCss tr td {
                font-size: 17px;
                padding: 5px;
                border: 1px solid #98b0c9;
            }

            .gridCss td input {
                font-size: 17px;
            }

            .gridCss tr td input[type=checkbox] {
                width: 17px;
                height: 17px;
                outline: none;
            }

        fieldset {
            /*border: 1px solid #2B7B78;*/
            padding: 0px;
            border-radius: 4px;
            border: 1px solid #98b0c9;
            width: auto;
            /*box-shadow: 2px 2px 8px 2px #efe7e7;*/
        }

        .masterFS {
            padding: 0 10px 10px 10px;
        }

        legend {
            text-align: left;
            display: block;
            width: auto;
            padding: 0;
            margin-bottom: 5px;
            font-size: 18px;
            line-height: inherit;
            border-bottom: transparent;
            color: black;
            font-weight: 600;
            color: #662594;
        }

        .settingBtns {
            font-size: 20px;
            background-color: #3777bc;
            color: white;
            border: 1px solid #3777bc;
            padding: 2px 15px;
        }

            .settingBtns:hover {
                font-size: 20px;
                background-color: #3777bc;
                color: white;
                border: 1px solid #3777bc;
                padding: 2px 15px;
            }

        #settingTbl {
            margin: auto;
            width: 100%;
        }

            #settingTbl tr td {
                padding: 7px;
            }

            #settingTbl tr > td {
            }

        .sideHeader {
            font-weight: 600;
            font-size: 18px;
            color: #662594;
        }

        .imgCss {
            width: 50px;
            height: 50x;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="headerDiv" class="row" style="color: black; background: #BDD7EE 1em; margin-top: 5px">
            <div class="col-lg-2">
                <div class="HeaderImage">
                    <%-- <img src="Images/SPFLogo.PNG" height="60" style="padding: 3px;" />--%>
                    <asp:Image ID="Image2" runat="server" class="img-responsive img-rounded" Style="width: 200px; height: 80px; margin-top: 2px" />
                </div>
            </div>
            <div class="col-lg-9" style="font-size: 30px; font-weight: bolder; text-align: center; vertical-align: central">
                <div>
                    Andon Setting
                </div>
            </div>
            <div class="col-lg-1" style="float: right">
                <img runat="server" src="~/Images/logo/AMITLogo.png" id="toggle" class="img-responsive img-rounded" alt="Logo" style="cursor: pointer; padding-right: 1px; margin-top: 4px; float: right; height: 75px" />
            </div>
        </div>
        <div style="margin-top:20px;">
            <div class="" style="height: 75vh; overflow: auto;display: inline-block; width: 40%; margin-left: 9%;vertical-align:top">
                <fieldset class="masterFS">
                    <legend>&nbsp;<b>Machine Setting</b></legend>
                    <asp:ListView runat="server" ID="lvMachineDetails">
                        <LayoutTemplate>
                            <table class="gridCss">
                                <tr>
                                    <th>Machine</th>
                                 <%--   <th>Sort Order</th>--%>
                                    <th>Image</th>
                                </tr>
                                <tr id="itemplaceholder" runat="server"></tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblMachineID" Text='<%# Eval("MachineID") %>'></asp:Label></td>
                             <%--   <td>
                                    <asp:TextBox runat="server" ID="txtSortOrder" Text='<%# Eval("SortOrder") %>' CssClass="form-control" Style="width:100px"></asp:TextBox></td>--%>
                                <td>
                                    <asp:Image runat="server" ID="machineImg" ImageUrl='<%# Eval("ImagePath") %>' CssClass="imgCss" onclick="showImage(this);" />
                                    <asp:FileUpload runat="server" ID="fileUpload" onchange="setClearIcon(this)" CssClass="form-control" Style="display: inline-block; width: 80%"  ClientIDMode="Static"/>
                                    <i id="clearFile" class="glyphicon glyphicon-remove" style="color: red; visibility: hidden; display: inline-block" onclick="clearFileUploadData(this,'fileUpload')"></i>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </fieldset>
            </div>
            <div style="display: inline-block; width: 40%;margin-left:20px;vertical-align:top">
                <fieldset class="masterFS">
                    <legend>&nbsp;<b>Background Setting</b></legend>
                    <table id="settingTbl">
                        <tr>
                            <td class="sideHeader">Background Image</td>
                            <td>
                                <asp:Image runat="server" ID="backGroundImg" CssClass="imgCss" onclick="showImage(this);" />
                                <asp:FileUpload runat="server" ID="bgFileUpload" CssClass="form-control" onchange="setClearIcon(this)" Style="display: inline-block; width: 80%"  ClientIDMode="Static"/>
                                <i id="clearFile" class="glyphicon glyphicon-remove" style="color: red; visibility: hidden; display: inline-block" onclick="clearFileUploadData(this,'bgFileUpload')"></i>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>

        <div style="text-align: center;margin-top:20px">
            <asp:Button runat="server" ID="applyBtn" Text="Apply" CssClass="btn settingBtns" Style="" OnClick="applyBtn_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn settingBtns" Style="" OnClick="btnCancel_Click" />
            <asp:Button runat="server" ID="homeBtn" class="btn settingBtns" OnClick="homeBtn_Click" Text="Andon" />
        </div>
        <div class="modal fade" id="bigImagesModal" role="dialog">
            <div class="modal-dialog modal-dialog-centered" style="width: 70%">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white; text-align: center; font-size: 20px">Images</h4>
                    </div>
                    <div class="modal-body" style="height: 70vh; overflow: auto">
                       <img src="" id="largeImg"  style="width:100%;height:100%"/>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="Close" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
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
                    <div class="modal-body">
                        <span id="lblWarningMsg"></span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #5D7B9D">
                        <button type="button" data-dismiss="modal" style="width: 80px;">OK</button>
                    </div>
                </div>
            </div>
        </div>
        <script>
            function showImage(imgs) {
                var srcimg = $(imgs).attr("src");
                $('#largeImg').attr("src", srcimg);
                $('#bigImagesModal').modal('show');
            }
            function setClearIcon(evt) {
                debugger;
                var row = $(evt).closest('tr');
                var clearIcon = $(row).find("#clearFile");
                $(clearIcon).css("visibility", "hidden");
                if (evt.files[0] != null) {
                    var fileSize = 1048576 * 100;
                    if (evt.files[0].size > fileSize) {
                        openWarningModal("File should be less than 100MB.");
                        $(evt).val("");
                        return;
                    }
                    fileExtension = evt.files[0].name;
                    fileExtension = fileExtension.split('.').pop().toLowerCase();
                    if (!(fileExtension == "png" || fileExtension == "jpeg" || fileExtension == "jpg")) {
                        openWarningModal("File must be Image.");
                        $(evt).val("");
                        return;
                    }
                }
                var selected_file_name = $(evt).val();
                if (selected_file_name.length > 0) {
                    $(clearIcon).css("visibility", "visible");
                }
                else {
                    $(clearIcon).css("visibility", "hidden");
                }
            }
            function clearFileUploadData(evt, fileid) {
                debugger;
                var row = $(evt).closest('tr');
                var fileup = $(row).find("#" + fileid);
                $(fileup).val('');
                $(evt).css("visibility", "hidden");
            }
            function openWarningModal(msg) {
                $('#lblWarningMsg').text(msg);
                $('[id*=warningModal]').modal('show');
            }
            function successMsg(msg, title) {
                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": true,
                    "onclick": null,
                    "showDuration": "1000",
                    "hideDuration": "1000",
                    "timeOut": "500",
                    "extendedTimeOut": "1000",
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
        </script>
    </form>
</body>
</html>
