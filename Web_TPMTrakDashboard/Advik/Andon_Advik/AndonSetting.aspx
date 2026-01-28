<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AndonSetting.aspx.cs" Inherits="Web_TPMTrakDashboard.Advik.Andon_Advik.AndonSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Andon Settings</title>
    <%--    <script src="/AndonScripts/jquery-3.1.0.min.js"></script>
    <script src="/AndonScripts/bootstrap.min.js"></script>
    <link href="/AndonContent/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/Site.css" rel="stylesheet" />--%>
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <script src="Scripts/toastr.min.js"></script>
    <link href="Scripts/toastr.min.css" rel="stylesheet" />

    <style>
        p {
            display: inline-block;
        }

        .HeaderImage {
            flex: 1;
            float: left;
        }

        .headerRight {
            color: white;
            font-weight: 600;
            font-size: 16px;
            margin: 3px;
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

        #fontTbl tr td {
            font-weight: 500;
            font-size: 17px;
            color: #662594;
            padding: 10px 10px 10px 0px;
        }

        .chkTbl {
            display: inline-block;
        }

            .chkTbl tr td {
                color: black;
                padding: 5px;
                font-size: 17px;
                font-weight: 500;
            }

                .chkTbl tr td label {
                    font-weight: unset;
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

        .imagelist {
            width: 200px;
            height: 200px;
        }

        .makeStyle {
            width: 200px;
            height: 200px;
            display: inline-block;
        }

        #showAllImages table, #showAllVideos table {
            width: 100%;
            text-align: center;
        }

            #showAllImages table tr td, #showAllVideos table tr td {
                padding: 5px;
            }

        .chkImageDelete {
            width: 22px;
            height: 22px;
            margin-left: 2% !important;
            vertical-align: top;
        }

        #lvPOColummsDataTbl {
            width: 100%;
        }

            #lvPOColummsDataTbl tr th {
                font-weight: 600;
                font-size: 18px;
                color: #662594;
                color: white;
                background-color: #3777bc;
                padding: 5px;
            }

            #lvPOColummsDataTbl tr {
            }

                #lvPOColummsDataTbl tr td {
                    font-size: 17px;
                    padding: 5px;
                    border: 1px solid #98b0c9;
                }

                    #lvPOColummsDataTbl tr td input {
                        font-size: 17px;
                    }

                        #lvPOColummsDataTbl tr td input[type=checkbox] {
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

        input[type="checkbox"] {
            width: 17px;
            height: 17px;
            -webkit-appearance: none;
            background-color: #fafafa;
            border: 1px solid #cacece;
            box-shadow: 0 1px 2px rgba(0,0,0,0.05), inset 0px -15px 10px -12px rgba(0,0,0,0.05);
            padding: 9px;
            border-radius: 3px;
            display: inline-block;
            position: relative;
            border-radius: 50%;
            border: 2px solid #65737c;
            outline: none;
        }

            input[type="checkbox"]:active, input[type="checkbox"]:checked:active {
                box-shadow: 0 1px 2px rgba(0,0,0,0.05), inset 0px 1px 3px rgba(0,0,0,0.1);
            }

            input[type="checkbox"]:checked {
                background-color: #e9ecee;
                border: 2px solid #3777bc;
                box-shadow: 0 1px 2px rgba(0,0,0,0.05), inset 0px -15px 10px -12px rgba(0,0,0,0.05), inset 15px 10px -12px rgba(255,255,255,0.1);
                color: #3777bc;
            }

                input[type="checkbox"]:checked:after {
                    content: '\2714';
                    font-size: 14px;
                    position: absolute;
                    top: 0px;
                    left: 3px;
                    color: #3777bc;
                }

        .imagesPreview {
            width: 95%;
            height: 150px;
            border: 1px solid silver;
        }

        .imageVideoDiv {
            width: 20%;
            display: inline-block;
            text-align: center;
            margin-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class=" container-fluid">
            <asp:HiddenField runat="server" ID="hdnshowToastr" ClientIDMode="Static" />
            <div class="row text-center">
                <div class="navbar navbar-default navbar-fixed-top text-center" style="padding: 0px 5px; background-color: #3777bc">
                    <div class="HeaderImage">
                        <%--<img src="Images/SPFLogo.PNG" height="60" style="padding: 3px;" />--%>
                        <asp:Image ID="Image2" runat="server" class="img-responsive img-rounded" Style="width: 165px; height: 60px;" />
                    </div>
                    <label id="headerName" style="color: white; font-weight: bold; font-size: 33px; text-align: right; margin-top: 5px">Application UI Setting</label>&nbsp;&nbsp;
                     
                    <img src="Images/UI_Settings.png" height="50" style="margin-top: -10px" />


                    <div style="float: right; position: relative; display: inline-flex">

                        <div style="text-align: left">
                            <p class="headerRight"><%: DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")%>&nbsp;&nbsp;</p>
                            <p class="headerRight"><span style="font-size: 20px; margin-top: 25px; cursor: pointer; color: white;" id="btnFullScreen"><i class="glyphicon glyphicon-fullscreen"></i></span>&nbsp;&nbsp;&nbsp;</p>
                        </div>
                        <%-- <div>
                            <p style="margin: 0px" onclick="settingClick()">
                                <img src="Images/List-Icon.jpg" height="29" />
                            </p>
                            <p style="margin: 0px">
                                <img src="Images/Power1.jpg" height="29" />
                            </p>
                        </div>--%>
                    </div>
                    &nbsp;&nbsp;
               
                </div>
            </div>
            <div>
                <div class="" style="width: 70%; margin: auto">
                    <div style="border: 2px solid  #3777bc; margin-top: 8%; box-shadow: 2px 2px 8px 2px #efe7e7; padding: 10px;">
                        <div class="row">
                            <div class="col-lg-6">
                                <fieldset class="masterFS">
                                    <legend>&nbsp;<b>APP Setting</b></legend>
                                    <table id="settingTbl">
                                        <tr>
                                            <td class="sideHeader">Font Size</td>
                                            <td>
                                                <table id="fontTbl">
                                                    <tr>
                                                        <td>Header
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="headerFontSz" AutoCompleteType="Disabled" onkeypress="return FontSizeRestriction(event,this, 'Header');" TextMode="Number" CssClass="form-control showToastr" ClientIDMode="Static"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Content</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="contentFontSz" AutoCompleteType="Disabled" onkeypress="return FontSizeRestriction(event,this, 'Content');" TextMode="Number" CssClass="form-control showToastr" ClientIDMode="Static"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </td>


                                        </tr>
                                        <tr>
                                            <td class="sideHeader">Screen Flip Interval (ss)</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtFlipInterval" AutoCompleteType="Disabled" CssClass="form-control showToastr" ClientIDMode="Static" onkeypress="return allowNumbericWithoutzero(event,this);"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="sideHeader">Refresh Interval (ss)</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDataRefreshInterval" AutoCompleteType="Disabled" CssClass="form-control showToastr" ClientIDMode="Static" onkeypress="return allowNumberic(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--  <tr>
                                <td class="sideHeader" style="vertical-align: top">Column Visibility</td>
                                <td>
                            <asp:CheckBoxList runat="server" ID="chkListviewColumns" ClientIDMode="Static" CssClass="chkTbl">
                                    </asp:CheckBoxList>
                                    <asp:CheckBoxList runat="server" ID="chkListviewColumns2" ClientIDMode="Static" CssClass="chkTbl">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>--%>
                                        <tr>
                                            <td class="sideHeader">Enable Slide Show for</td>
                                            <td>
                                                <asp:CheckBox runat="server" ID="chkShowImage" Text="&nbsp;Image" ClientIDMode="Static" Style="font-size: 17px;" CssClass="showToastr" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                               
                                                <asp:CheckBox runat="server" ID="chkShowVideo" Text="&nbsp;Video" ClientIDMode="Static" Style="font-size: 17px;" CssClass="showToastr" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="sideHeader">Enable Slide Show Interval (ss)</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRefreshInterval" AutoCompleteType="Disabled" CssClass="form-control showToastr" ClientIDMode="Static" onkeypress="return allowNumberic(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td class="sideHeader">No. of Rows</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNoOfRows" TextMode="Number" AutoCompleteType="Disabled" CssClass="form-control showToastr" ClientIDMode="Static"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="sideHeader">Image Path</td>
                                            <td>
                                                <asp:FileUpload runat="server" ID="imageFileUpload" AllowMultiple="true" Style="display: inline-block" ClientIDMode="Static" />
                                                <%-- onchange="imagePreview();"--%>
                                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnImage" />
                                                <%-- <asp:TextBox runat="server" ID="txtImagePath" ClientIDMode="Static"></asp:TextBox>--%>
                                                <i class="glyphicon glyphicon-eye-open" title="Preview" onclick="showImageListPreview();" id="imagePreviewIcon" style="visibility: hidden; color: #024302; font-size: 20px;"></i>
                                                <i class="glyphicon glyphicon-trash" title="Delete" style="color: #a20909; font-size: 20px;" onclick="showImageList();"></i>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="sideHeader">Video Path</td>
                                            <td>
                                                <asp:FileUpload runat="server" ID="videoFileUpload" AllowMultiple="true" Style="display: inline-block" ClientIDMode="Static" />
                                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnVideo" />
                                                <%--  <asp:TextBox runat="server" ID="txtVideoPath" ClientIDMode="Static"></asp:TextBox>--%>
                                                <i class="glyphicon glyphicon-eye-open" title="Preview" onclick="showVideoListPreview();" id="videoPreviewIcon" style="visibility: hidden; color: #024302; font-size: 20px;"></i>
                                                <i class="glyphicon glyphicon-trash" title="Delete" style="color: #a20909; font-size: 20px;" onclick="showVideoList();"></i>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="sideHeader">Scrolling Text</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtScrolling" ClientIDMode="Static" CssClass="form-control showToastr" AutoCompleteType="Disabled"></asp:TextBox></td>
                                        </tr>
                                      
                                    </table>
                                </fieldset>
                            </div>
                            <div class="col-lg-6">
                                <fieldset class="masterFS">
                                    <legend>&nbsp;<b>ANDON View Setting</b></legend>
                                    <asp:ListView runat="server" ID="lvPOColummsData">
                                        <LayoutTemplate>
                                            <table id="lvPOColummsDataTbl">
                                                <tr>
                                                    <th>Column</th>
                                                    <th>Custom Column Name</th>
                                                    <th>Visibility</th>
                                                </tr>
                                                <tr runat="server" id="itemplaceholder"></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblColumn" Text='<%# Eval("Column") %>'></asp:Label></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtCustomName" Text='<%# Eval("CustomColumn") %>' CssClass="form-control showToastr" AutoCompleteType="Disabled"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkColumnVisibility" CssClass="chkBoxCss showToastr" Checked='<%# Eval("Visibility") %>' ClientIDMode="Static" /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </fieldset>

                            </div>
                        </div>

                        <div style="text-align: center; margin: 30px auto 10px auto">
                            <asp:Button runat="server" ID="applyBtn" Text="Apply" CssClass="btn settingBtns" Style="" OnClick="applyBtn_Click" OnClientClick="return dataValidation();" />
                            <input type="button" value="Cancel" class="btn settingBtns" runat="server" id="cancelBtn" onserverclick="cancelBtn_ServerClick" />
                            <asp:Button runat="server" ID="homeBtn" class="btn settingBtns" OnClick="homeBtn_Click" Text="Home" />
                        </div>
                        <%--   <div class="panel panel-default" id="settingPanel" style="width: 50%; margin: auto; z-index: 30; box-shadow: 2px 2px 8px 2px #efe7e7; border: 1px solid #3777bc">
                       <div class="panel-heading" style="padding: 10px; text-align: center; background-color: #3777bc; font-size: 17px; color: white">Application UI Setting</div>
                        <div class="panel-body" style="height: 32%;">
                            <div>
                              
                            </div>
                        </div>
                        <div class="panel-footer" style="padding: 10px; text-align: center; background-color: #3777bc;">
                         
                          
                        </div>
                    </div>--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="showAllImages" role="dialog">
            <div class="modal-dialog modal-dialog-centered" style="width: 70%">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white; text-align: center; font-size: 20px">Images</h4>
                    </div>
                    <div class="modal-body" style="height: 70vh; overflow: auto">
                        <div id="ImageList"></div>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="Delete" class="btn btn-info" style="background-color: #3777bc; color: white" onclick="imageDeleteBtnClick();" />
                        <input type="button" value="Close" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="showAllVideos" role="dialog">
            <div class="modal-dialog modal-dialog-centered" style="width: 70%">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white; text-align: center; font-size: 20px">Video</h4>
                    </div>
                    <div class="modal-body" style="height: 70vh; overflow: auto">
                        <div id="VideoList"></div>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="Delete" class="btn btn-info" style="background-color: #3777bc; color: white" onclick="videoDeleteBtnClick();" />
                        <input type="button" value="Close" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myErrorModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white;">Error!</h4>
                    </div>
                    <div class="modal-body">
                        <img src="Images/error.png" width="40" />&nbsp;&nbsp;&nbsp;
                   
						

                        <span id="errormessageText" style="font-size: 17px;">Error</span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="OK" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="deleteConfirmationModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white;">Confirmation?</h4>
                    </div>
                    <div class="modal-body">
                        <img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;
                
                        <span id="confirmationText" style="font-size: 17px;">Error</span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="Yes" class="btn btn-info" onclick="deleteImagesClick();" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                        <input type="button" value="No" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="deletevideoConfirmationModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white;">Confirmation?</h4>
                    </div>
                    <div class="modal-body">
                        <img src="Images/confirm.png" width="40" />&nbsp;&nbsp;&nbsp;
                
                        <span id="confirmationVideoText" style="font-size: 17px;">Error</span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="Yes" class="btn btn-info" onclick="deleteVideoClick();" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                        <input type="button" value="No" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="showAllImagesPreviewModal" role="dialog">
            <div class="modal-dialog modal-dialog-centered" style="width: 70%">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white; text-align: center; font-size: 20px">Images</h4>
                    </div>
                    <div class="modal-body" style="height: 70vh; overflow: auto">
                        <div id='previewImg'></div>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="OK" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="showAllVideoPreview" role="dialog">
            <div class="modal-dialog modal-dialog-centered" style="width: 70%">
                <div class="modal-content" style="border: 2px solid #3777bc">
                    <div class="modal-header" style="background-color: #3777bc; padding: 8px">

                        <h4 class="modal-title" style="color: white; text-align: center; font-size: 20px">Videos</h4>
                    </div>
                    <div class="modal-body" style="height: 70vh; overflow: auto">
                        <div id='previewVideo'></div>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #3777bc; text-align: center">
                        <input type="button" value="OK" class="btn btn-info" style="background-color: #3777bc; color: white" data-dismiss="modal" />
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
                        <img src="Images/warnig.png" width="40" />&nbsp;&nbsp;&nbsp;
				
					<span id="lblWarningMsg"></span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #5D7B9D">
                        <button type="button" data-dismiss="modal" style="width: 80px;">OK</button>
                    </div>
                </div>
            </div>
        </div>
        <script>

            var imageFlag = 0, videoFlag = 0;
            $('.showToastr').change(function () {
                $('#hdnshowToastr').val("update");
            });
            function allowNumbericWithoutzero(evt, val) {
                debugger;
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                let value = $(val).val();
                let firstNumber = $(val).val().charAt(0);
                if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                else {

                    if (value.length == 0) {
                        if (charCode == 48) {
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        return true;
                    }
                }
            }
            $("[id$=btnFullScreen]").click(function () {
                if ((document.fullScreenElement && document.fullScreenElement !== null) ||
                    (!document.mozFullScreen && !document.webkitIsFullScreen)) {
                    if (document.documentElement.requestFullScreen) {
                        document.documentElement.requestFullScreen();
                    } else if (document.documentElement.msRequestFullscreen) {
                        document.documentElement.msRequestFullscreen();
                    } else if (document.documentElement.mozRequestFullScreen) {
                        document.documentElement.mozRequestFullScreen();
                    } else if (document.documentElement.webkitRequestFullScreen) {
                        document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
                    }
                } else {
                    if (document.cancelFullScreen) {
                        document.cancelFullScreen();
                    } else if (document.msRequestFullscreen) {
                        document.msRequestFullscreen();
                    } else if (document.mozCancelFullScreen) {
                        document.mozCancelFullScreen();
                    } else if (document.webkitCancelFullScreen) {
                        document.webkitCancelFullScreen();
                    }
                }
            });

            function dataValidation() {
                if ($('#headerFontSz').val().trim() == "") {
                    openWarningModal('Please enter Header Font Size.');
                    return false;
                }
                if ($('#contentFontSz').val().trim() == "") {
                    openWarningModal('Please enter Content Font Size.');
                    return false;
                }
                if ($('#txtFlipInterval').val().trim() == "") {
                    openWarningModal('Please enter Screen Flip Interval.');
                    return false;
                }
                if ($('#txtFlipInterval').val().trim() == "") {
                    openWarningModal('Please enter Screen Flip Interval.');
                    return false;
                }
                if ($('#txtDataRefreshInterval').val().trim() == "") {
                    openWarningModal('Please enter Refresh Interval.');
                    return false;
                }
                if (imageFlag == 1) {
                    debugger;
                    openErrorModal('Invalid image file.');
                    return false;
                }
                if (videoFlag == 1) {
                    openErrorModal('Invalid video file.');
                    return false;
                }
                if ($('#chkShowImage').prop('checked') == true || $('#chkShowVideo').prop('checked') == true) {
                    if ($('#txtRefreshInterval').val().trim() == "") {
                        openWarningModal('Please enter Enable Slide Show Interval.');
                        return false;
                    }
                }
                return true;
            }
            function readURL(input) {
                if (input.files && input.files[0]) {
                    $(input.files).each(function () {
                        var reader = new FileReader();
                        reader.readAsDataURL(this);
                        reader.onload = function (e) {
                            debugger;
                            if ((e.target.result).includes("image")) {
                                $("#previewImg").append("<div class='imageVideoDiv'><img class='thumb imagesPreview' src='" + e.target.result + "'></div>");
                            }
                            else if ((e.target.result).includes("video")) {
                                imageFlag = 1;
                                $("#previewImg").append("<div class='imageVideoDiv'><video  class='imagesPreview' style='vertical-align: middle;' muted='muted' autoplay='autoplay'><source src='" + e.target.result + "' /></video></div>");
                            }
                        }
                    });
                }
            }
            function readVideoURL(input) {
                if (input.files && input.files[0]) {
                    $(input.files).each(function () {
                        var reader = new FileReader();
                        reader.readAsDataURL(this);
                        reader.onload = function (e) {
                            debugger;
                            if ((e.target.result).includes("image")) {
                                videoFlag = 1;
                                $("#previewVideo").append("<div class='imageVideoDiv'><img class='thumb imagesPreview' src='" + e.target.result + "'></div>");
                            }
                            else if ((e.target.result).includes("video")) {
                                $("#previewVideo").append("<div class='imageVideoDiv'><video  class='imagesPreview' style='vertical-align: middle;' muted='muted' autoplay='autoplay'><source src='" + e.target.result + "' /></video></div>");
                            }
                        }
                    });
                }
            }
            $("#imageFileUpload").bind("change", function () {
                $('#hdnshowToastr').val("update");
                var selected_file_name = $(this).val();
                if (selected_file_name.length > 0) {
                    $("#previewImg").empty();
                    readURL(this);
                    document.getElementById('imagePreviewIcon').style.visibility = 'visible';
                }
                else {
                    $("#previewImg").empty();
                    document.getElementById('imagePreviewIcon').style.visibility = 'hidden';
                }

            });
            //$("#imageFileUpload").change(function () {
            //    readURL(this);
            //    document.getElementById('imagePreviewIcon').style.visibility = 'visible';
            //});
            //$("#imageFileUpload").blur(function () {
            //    debugger;
            //    var target = event.target;
            //});

            $("#videoFileUpload").bind("change", function () {
                $('#hdnshowToastr').val("update");
                var selected_file_name = $(this).val();
                if (selected_file_name.length > 0) {
                    $("#previewVideo").empty();
                    readVideoURL(this);
                    document.getElementById('videoPreviewIcon').style.visibility = 'visible';
                }
                else {
                    $("#previewVideo").empty();
                    document.getElementById('videoPreviewIcon').style.visibility = 'hidden';
                }

            });
            function showImageListPreview() {
                $('[id*=showAllImagesPreviewModal]').modal('show');
            }
            function showVideoListPreview() {
                $('[id*=showAllVideoPreview]').modal('show');
            }
            function imagePreview() {
                debugger;
                var preview = document.getElementById('imageP');
                var file = document.getElementById('imageFileUpload').files;
                var reader = new FileReader();

                reader.onloadend = function () {
                    preview.src = reader.result;
                }
                if (file.length > 0) {
                    reader.readAsDataURL(file);
                } else {
                    preview.src = "";
                }
            }
            function showpop5(msg, title) {
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
            function imageDeleteBtnClick() {
                var chkBox = $('#ImageList input');
                var flag = 0;
                for (var i = 0; i < chkBox.length; i++) {
                    if (chkBox[i].checked) {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1) {
                    openConfimationModal("Are you sure, you want to delete selected Images?");
                }
                else {
                    openErrorModal("Select Image");
                }
            }
            function videoDeleteBtnClick() {
                var chkBox = $('#VideoList input');
                var flag = 0;
                for (var i = 0; i < chkBox.length; i++) {
                    if (chkBox[i].checked) {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1) {
                    openVideoConfimationModal("Are you sure, you want to delete selected Videos?");
                }
                else {
                    openErrorModal("Select Video");
                }
            }
            function deleteImagesClick() {
                debugger;
                var chkBox = $('#ImageList input');
                var images = $('#ImageList img');
                if (images.length > 0) {
                    var deletedImages = [];
                    for (var i = 0; i < chkBox.length; i++) {
                        if (chkBox[i].checked) {
                            deletedImages.push(images[i].alt.split('/').pop());
                        }
                    }
                    if (deletedImages.length > 0) {
                        var imageNames = JSON.stringify(deletedImages);
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "AndonSetting.aspx/deleteImages",
                            contentType: "application/json; charset=utf-8",
                            data: '{names:' + imageNames + '}',
                            dataType: "json",
                            success: function (response) {

                                var dataitem = response.d;
                                bindImageFromFolder();
                            },
                            error: function (Result) {

                            }
                        });
                    }
                }
            }
            function deleteVideoClick() {
                debugger;
                var chkBox = $('#VideoList input');
                var video = $('#VideoList video');
                if (video.length > 0) {
                    var deletedVideo = [];
                    for (var i = 0; i < chkBox.length; i++) {
                        if (chkBox[i].checked) {
                            deletedVideo.push(video[0].getAttribute('alt').split('/').pop());
                        }
                    }
                    if (deletedVideo.length > 0) {
                        var videoNames = JSON.stringify(deletedVideo);
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "AndonSetting.aspx/deleteVideos",
                            contentType: "application/json; charset=utf-8",
                            data: '{names:' + videoNames + '}',
                            dataType: "json",
                            success: function (response) {

                                var dataitem = response.d;
                                bindVideoFromFolder();
                            },
                            error: function (Result) {

                            }
                        });
                    }
                }
            }
            function bindImageFromFolder() {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "AndonSetting.aspx/getImageDetails",
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (response) {
                        var itmdata = response.d;
                        $('#ImageList').empty();
                        if (itmdata.length > 0) {
                            //var appendString = "<table><tr>";
                            //for (var i = 0; i < itmdata.length; i++) {
                            //    if ((i % 3) == 0) {
                            //        appendString += '</tr>';
                            //        appendString += '<tr><td><img src="' + itmdata[i] + '" alt="' + itmdata[i] + '" class="imagelist" /><input type="checkbox" class="chkImageDelete" /></td>';
                            //    }
                            //    else {
                            //        appendString += '<td><img src="' + itmdata[i] + '" alt="' + itmdata[i] + '" class="imagelist" /><input type="checkbox" class="chkImageDelete" /></td>';
                            //    }
                            //}
                            //appendString += '</tr></table>';
                            //$('#ImageList').append(appendString);
                            var appendString = "";
                            for (var i = 0; i < itmdata.length; i++) {
                                appendString += '<div class="imageVideoDiv"><img src="' + itmdata[i] + '" alt="' + itmdata[i] + '" class="imagelist" /><input type="checkbox" class="chkImageDelete" /></div>';
                            }
                            $('#ImageList').append(appendString);
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                    }
                });
            }
            function bindVideoFromFolder() {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "AndonSetting.aspx/getVideoDetails",
                    contentType: "application/json; charset=utf-8",
                    datatype: "json",
                    success: function (response) {
                        var itmdata = response.d;
                        $('#VideoList').empty();
                        if (itmdata.length > 0) {
                            //var appendString = "<table><tr>";
                            //for (var i = 0; i < itmdata.length; i++) {
                            //    if ((i % 3) == 0) {
                            //        appendString += '</tr>';
                            //        appendString += '<tr><td> <video class="slide-image embed-responsive-item center-block makeStyle" alt="' + itmdata[i] + '" playsinline="playsinline" muted="muted" autoplay="autoplay" controls><source src="' + itmdata[i] + '" /></video><input type="checkbox" class="chkImageDelete" /></td>';
                            //    }
                            //    else {
                            //        appendString += '<td> <video class="slide-image embed-responsive-item center-block makeStyle" alt="' + itmdata[i] + '" playsinline="playsinline" muted="muted" autoplay="autoplay" controls><source src="' + itmdata[i] + '" /></video><input type="checkbox" class="chkImageDelete" /></td>';
                            //    }
                            //}
                            //appendString += '</tr></table>';
                            var appendString = "";
                            for (var i = 0; i < itmdata.length; i++) {

                                appendString += '<div class="imageVideoDiv"><video class="slide-image embed-responsive-item center-block makeStyle" alt="' + itmdata[i] + '" playsinline="playsinline" muted="muted" autoplay="autoplay" controls><source src="' + itmdata[i] + '" /></video><input type="checkbox" class="chkImageDelete" /></div>';

                            }
                            $('#VideoList').append(appendString);
                        }
                    },
                    error: function (jqXHR, textStatus, err) {
                    }
                });
            }
            function showImageList() {
                debugger;
                bindImageFromFolder();
                $('[id*=showAllImages]').modal('show');
                $('[id*=showAllImagesPreviewModal]').modal('hide');

            }
            function showVideoList() {
                bindVideoFromFolder();
                $('[id*=showAllVideos]').modal('show');
            }
            function FontSizeRestriction(evt, val, param) {
                let charCode = (evt.which) ? evt.which : evt.keyCode;
                let fontSize = $(val).val();
                let firstNumber = $(val).val().charAt(0);
                if (param == "Header") {
                    if (charCode < 48 || charCode > 57) {
                        return false;
                    } else {
                        if (fontSize.length > 1) {
                            return false;
                        } else if (fontSize.length == 1) {
                            if (firstNumber == 4) {
                                if (charCode >= 49 && charCode <= 57) {
                                    return false;
                                } else {
                                    return true;
                                }
                            } else if (firstNumber > 4) {
                                return false;
                            } else {
                                return true;
                            }
                        } else {
                            return true;
                        }
                    }
                }

                if (param == "Content") {
                    if (charCode < 48 || charCode > 57) {
                        return false;
                    } else {
                        if (fontSize.length > 1) {
                            return false;
                        } else if (fontSize.length == 1) {
                            if (firstNumber == 3) {
                                if (charCode == 57) {
                                    return false;
                                } else {
                                    return true;
                                }
                            } else if (firstNumber > 3) {
                                return false;
                            } else {
                                return true;
                            }
                        } else {
                            return true;
                        }
                    }
                }
            }
            function SlideShowIntervalRestriction(evt, val) {
                //let charCode = (evt.which) ? evt.which : evt.keyCode;
                //let interval = $(val).val();
                //let firstNumber = $(val).val().charAt(0);
                //if (charCode < 48 || charCode > 57) {
                //    return false;
                //} else {
                //    if (interval.length > 1) {
                //        return false;
                //    } else if (interval.length == 1) {
                //        if (firstNumber == 6) {
                //            if (charCode >= 49 && charCode <= 57) {
                //                return false;
                //            } else {
                //                return true;
                //            }
                //        } else if (firstNumber > 6) {
                //            return false;
                //        } else {
                //            return true;
                //        }
                //    } else {
                //        return true;
                //    }
                //}
            }
            function DataRefreshIntervalRestriction(evt, val) {
                let charCode = (evt.which) ? evt.which : evt.keyCode;
                let interval = $(val).val();
                let firstNumber = $(val).val().charAt(0);
                if (charCode < 48 || charCode > 57) {
                    return false;
                } else {
                    if (interval.length > 1) {
                        return false;
                    } else if (interval.length == 1) {
                        if (firstNumber == 3) {
                            if (charCode >= 49 && charCode <= 57) {
                                return false;
                            } else {
                                return true;
                            }
                        } else if (firstNumber > 3) {
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        return true;
                    }
                }
            }
            function allowNumberic(evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if ((charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            $('#txtRefreshInterval').blur(function () {
                var flipInterval;
                var refreshInterval;
                if ($('#txtRefreshInterval').val() != "") {
                    refreshInterval = parseInt($('#txtRefreshInterval').val());
                    if (refreshInterval < 60) {
                        $('#txtRefreshInterval').val("");
                        openWarningModal("Minimum value of Enable Slide Show Interval should be 60.");
                        return;
                    }
                }
                if ($('#txtFlipInterval').val() != "") {
                    flipInterval = parseInt($('#txtFlipInterval').val().trim());
                    if ($('#txtRefreshInterval').val() != "") {
                        refreshInterval = parseInt($('#txtRefreshInterval').val().trim());
                        if (refreshInterval <= flipInterval) {
                            $('#txtRefreshInterval').val("");
                            openWarningModal("Enable Slide Show Interval should be more than Screen Flip Interval.");
                            return;
                        }
                    }
                }
            });
            $('#txtFlipInterval').blur(function () {
                var flipInterval;
                var refreshInterval;
                debugger;
                if ($('#txtRefreshInterval').val() != "") {
                    refreshInterval = parseInt($('#txtRefreshInterval').val().trim());
                    if ($('#txtFlipInterval').val() != "") {
                        flipInterval = parseInt($('#txtFlipInterval').val().trim());
                        if (flipInterval >= refreshInterval) {
                            $('#txtFlipInterval').val("");
                            openWarningModal("Screen Flip Interval should be less than Enable Slide Show Interval.");
                            return;
                        }
                    }
                }
                if ($('#txtFlipInterval').val() != "") {
                    flipInterval = parseInt($('#txtFlipInterval').val().trim());
                    if (flipInterval == 0) {
                        $('#txtFlipInterval').val("");
                        openWarningModal("Screen Flip Interval should be greater than Zero.");
                        return;
                    }
                }
            });
            $('#txtDataRefreshInterval').blur(function () {
                var intrval;
                if ($('#txtDataRefreshInterval').val() != "") {
                    intrval = parseInt($('#txtDataRefreshInterval').val().trim());
                    if (intrval < 30 || intrval > 120) {
                        $('#txtDataRefreshInterval').val("");
                        openWarningModal("Refresh Interval should be between 30 and 120 seconds.");
                        return;
                    }
                }
            });
            function openErrorModal(msg) {
                $('[id*=myErrorModal]').modal('show');
                $("#errormessageText").text(msg);
            };
            function openConfimationModal(msg) {
                $('[id*=deleteConfirmationModal]').modal('show');
                $("#confirmationText").text(msg);
            };
            function openWarningModal(msg) {
                $('#lblWarningMsg').text(msg);
                $('[id*=warningModal]').modal('show');
            }
            function openVideoConfimationModal(msg) {
                $('[id*=deletevideoConfirmationModal]').modal('show');
                $("#confirmationVideoText").text(msg);
            };
            $('#imageFileUpload').change(function () {
                $('#hdnImage').val("addimage");
            });
            $('#videoFileUpload').change(function () {
                $('#hdnVideo').val("addvideo");
            });
        </script>
    </form>

</body>
</html>
