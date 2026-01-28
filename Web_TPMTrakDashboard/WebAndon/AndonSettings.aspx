<%@ Page Title="" Language="C#" MasterPageFile="~/WebAndon/AndonMaster.Master" AutoEventWireup="true" CodeBehind="AndonSettings.aspx.cs" Inherits="Web_TPMTrakDashboard.WebAndon.MachineSetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    

    
    <link href="../AndonContent/ColorPickerCss/pick-a-color-1.2.3.min.css" rel="stylesheet" />
    <script src="../AndonScripts/ColorPickerJs/tinycolor-0.9.15.min.js"></script>
    <script src="../AndonScripts/ColorPickerJs/pick-a-color-1.2.3.min.js"></script>


    <script type="text/javascript">


        $(document).ready(function () {
            //$("#imgHide").hide();
            $.unblockUI({});
            $('[data-toggle="tooltip"]').tooltip();
            $(".pick-a-color").pickAColor();
            //------------Check Box Populate Data--------------------
            checkBoxManage();
            function checkBoxManage() {
                if ($("[id$=chkShowSmileyImg]").is(':checked'))
                    $("#imgHide").show();
                else {
                    $("#imgHide").hide();
                    $("[id$=ddlSmileImageSize]").val('0');
                }

                if ($("[id$=hdfNonAdmin]").val() == "NonAdmin") {
                    $("[id$=FileUpload1]").prop('disabled', true);
                    //$("[id$=FileUpload1]").tooltip({ placement: 'bottom', title: "This Text Disable for Admin !!" });
                    $("#btnUpload").prop('disabled', true);
                    //$("#btnUpload").tooltip({ placement: 'bottom', title: "This Text Disable for Admin !!" });
                    $(".toolTip").attr('title', 'This Field Enable for Admin !!');
                    //$(".toolTip").tooltip({ placement: 'top', title: "This Text Disable for Admin !!" });
                    //$("[id$=txtGood]").tooltip({ placement: 'bottom', title: "This Text Disable for Admin !!" });
                    $("[id$=txtGood]").prop('disabled', true);

                    $("[id$=txtBad]").prop('disabled', true);
                    $("[id$=txtModerate]").prop('disabled', true);
                }
            }

            //------------------Check Box Checked Event--------------------
            $("[id$=chkShowSmileyImg]").click(function () {
                if ($("[id$=chkShowSmileyImg]").is(':checked'))
                    $("#imgHide").show();
                else {
                    $("#imgHide").hide();
                    $("[id$=ddlSmileImageSize]").val('0');
                }
            });

            //--------------Save General Setting Validation Data----------------------------
            $("[id$=btnSaveGeneralPageSetting]").click(function () {
                var count = 0;
                if ($("[id$=txtAndonTitle]").val() == "") {
                    alert("Please enter the Andon Title !!");
                    $("[id$=txtAndonTitle]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlFontFamily]").val() == "0") {
                    alert("Please select the Font Family !!");
                    $("[id$=ddlFontFamily]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlFontStyle]").val() == "0") {
                    alert("Please select the Font Style !!");
                    $("[id$=ddlFontStyle]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlRefreshInterval]").val() == "0") {
                    alert("Please select the Data Refresh Interval !!");
                    $("[id$=ddlRefreshInterval]").focus();
                    count++;
                    return false;
                }
                if (count == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("GeneralSetting")
                    $("[id$=btnHideSetting]").trigger("click");
                }
                return false;
            });

            //-----------------Save Iconic Cockpit Setting Validation Data----------------------
            $("[id$=btnSaveIconicCockpitSetting]").click(function () {
                var flag = 0;
                var counter = $("[id$=grdViewIconicView] input[id*='chkSelect']:checked").length;
                if (counter < 3) {
                    alert('Please select at least 3 records to visibility for Iconic Cockpit Setting !!');
                    return false;
                }
                else {
                    $("[id$=grdViewIconicView] input[id*='txtValueInText2']").each(function () {
                        if ($(this).val() == "") {
                            flag++;
                            alert('Please Enter Custom Column Name !!');
                            $(this).focus();
                            return false;
                        }
                    });
                }
                if (flag == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("IconicSetting")
                    $("[id$=btnHideSave]").trigger("click");
                }
                return false;
            });

            //----------------Save Machine Setting Data---------------------------

            $("[id$=btnSave]").click(function () {
                var flag1 = 0;
                var counter = $("[id$=gridviewMachineInfo] input[id*='chkSelect']:checked").length;
                if (counter < 3) {
                    alert('Please select at least 3 records to visibility Machine Setting !!');
                    return false;
                }
                else {
                    $("[id$=gridviewMachineInfo] :checked").closest("tr").find("input[id*='txtSortOrder']").each(function () {
                        if ($(this).val() == "") {
                            flag1++;
                            alert('Please Enter Sort Order !!');
                            $(this).focus();
                            return false;
                        }
                    });
                }
                if (flag1 == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("MachineSetting")
                    $("[id$=btnHideSave]").trigger("click");
                }
                return false;
            })

            //-------------Save Table Cockpit Setting Validation Data------------------------
            $("[id$=btnSaveCockpitSettingTable]").click(function () {
                var flag = 0;
                var counter = $("[id$=grdViewTableView] input[id*='chkSelect']:checked").length;
                if (counter < 3) {
                    alert('Please select at least 3 records to visibility Table Cockpit Setting !!');
                    return false;
                }
                else {
                    $("[id$=grdViewTableView] input[id*='txtValueInText2']").each(function () {
                        if ($(this).val() == "") {
                            flag++;
                            alert('Please Enter Custom Column Name !!');
                            $(this).focus();
                            return false;
                        }
                    });
                }
                if (flag == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("TableSetting")
                    $("[id$=btnHideSave]").trigger("click");
                }
                return false;
            });


            //-------------Save Energy Cockpit Setting Validation Data------------------------
            $("[id$=btnEnergySaveCockpitSetting]").click(function () {
                var flag = 0;
                var counter = $("[id$=gridViewEnergyCollumn] input[id*='chkSelect']:checked").length;
                if (counter < 3) {
                    alert('Please select at least 3 records to visibility Energy Cockpit Setting !!');
                    return false;
                }
                else {
                    $("[id$=gridViewEnergyCollumn] input[id*='txtValueInText2']").each(function () {
                        if ($(this).val() == "") {
                            flag++;
                            alert('Please Enter Custom Column Name !!');
                            $(this).focus();
                            return false;
                        }
                    });
                }
                if (flag == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("EnergySetting")
                    $("[id$=btnHideSave]").trigger("click");
                }
                return false;
            });


            //------------------Save Color Information-----------------------------------
            $("[id$=btnSaveColor]").click(function () {
                $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                $("[id$=hdfParameter]").val("ColorSetting")
                $("[id$=btnHideSetting]").trigger("click");
                return false;
            });

            //--------------Validate Iconic They Are Not Enter Same Values------------------
            $(document).on('change', "input[id*='txtValueInText2']", function () {
                var seen = {};
                $("[id$=grdViewIconicView] input[id*='txtValueInText2']").each(function () {
                    var txt = $(this).val();
                    if (seen[txt]) {
                        if (txt != "") {
                            alert('Please do not enter duplicate Custom Column Name !');
                            $(this).val('');
                            return false;
                        }
                    }
                    else {
                        seen[txt] = true;
                    }
                });
            });

            //--------------Validate Table They Are Not Enter Same Values------------------
            $(document).on('change', "input[id*='txtValueInText2']", function () {
                var seen = {};
                $("[id$=grdViewTableView] input[id*='txtValueInText2']").each(function () {
                    var txt = $(this).val();
                    if (seen[txt]) {
                        if (txt != "") {
                            alert('Please do not enter duplicate Custom Column Name !');
                            $(this).val('');
                            return false;
                        }
                    }
                    else {
                        seen[txt] = true;
                    }
                });
            });

            //----------------Save Iconic Page Setting Data----------------------------
            $("[id$=btnSaveIconicPageSetting]").click(function () {
                var count = 0;
                if ($("[id$=ddlFontSizeMachineBox]").val() == "0") {
                    alert("Please select the Font Size for Iconic View !!");
                    $("[id$=ddlFontSizeMachineBox]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlMachineFlip]").val() == "0") {
                    alert("Please select the Flip Interval for Iconic View !!");
                    $("[id$=ddlMachineFlip]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=chkShowSmileyImg]").is(':checked')) {
                    if ($("[id$=ddlSmileImageSize]").val() == "0") {
                        alert("Please select the Smiley Image Size for Iconic View !!");
                        $("[id$=ddlSmileImageSize]").focus();
                        count++;
                        return false;
                    }
                }
                if (count == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("IconicSetting")
                    $("[id$=btnHideSetting]").trigger("click");
                }
                return false;
            });

            //-----------------Save Table Page Setting Data------------------------------
            $("[id$=btnSavePageSettingSave]").click(function () {
                var count = 0;
                if ($("[id$=ddlFontSizeTable]").val() == "0") {
                    alert("Please select the Font Size for Table View !!");
                    $("[id$=ddlFontSizeTable]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlFlipIntervalTable]").val() == "0") {
                    alert("Please select the Flip Interval for Table View !!");
                    $("[id$=ddlFlipIntervalTable]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlPageSize]").val() == "0") {
                    alert("Please select the Page Size for Table View !!");
                    $("[id$=ddlPageSize]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlBorderWidth]").val() == "0") {
                    alert("Please select the Border Width for Table View !!");
                    $("[id$=ddlBorderWidth]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlBorderColor]").val() == "0") {
                    alert("Please select the Border Color for Table View !!");
                    $("[id$=ddlBorderColor]").focus();
                    count++;
                    return false;
                }
                if (count == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("TableSetting")
                    $("[id$=btnHideSetting]").trigger("click");
                }
                return false;
            });

            //-----------------Save Table Page Setting Data------------------------------
            $("[id$=btnEnergySavePageSetting]").click(function () {
                var count = 0;
                if ($("[id$=ddlEnergyFontSize]").val() == "0") {
                    alert("Please select the Font Size for Energy View !!");
                    $("[id$=ddlEnergyFontSize]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlEnergyFlipInterval]").val() == "0") {
                    alert("Please select the Flip Interval for Energy View !!");
                    $("[id$=ddlEnergyFlipInterval]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlEnergyPageSize]").val() == "0") {
                    alert("Please select the Page Size for Energy View !!");
                    $("[id$=ddlEnergyPageSize]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlEnergyBorderWidth]").val() == "0") {
                    alert("Please select the Border Width for Energy View !!");
                    $("[id$=ddlEnergyBorderWidth]").focus();
                    count++;
                    return false;
                }
                if ($("[id$=ddlEnergyBorderColor]").val() == "0") {
                    alert("Please select the Border Color for Energy View !!");
                    $("[id$=ddlEnergyBorderColor]").focus();
                    count++;
                    return false;
                }
                if (count == 0) {
                    $.blockUI({ message: '<img src="./Image/LoadingImg/logo.jpg" style="height: 160px"  />' });
                    $("[id$=hdfParameter]").val("EnergySetting")
                    $("[id$=btnHideSetting]").trigger("click");
                }
                return false;
            });


            //------------------Upload File------------------------------------------
            $("#btnUpload").click(function () {
                if ($("[id$=FileUpload1]").val() == "") {
                    alert("Please Select File for Uploading !!");
                    $("[id$=FileUpload1]").focus();
                    return false;
                }
                else {
                    var file = $("[id$=FileUpload1]").val();
                    var exts = ['jpg', 'jpeg', 'gif', 'png'];
                    // first check if file field has any value
                    if (file) {
                        // split file name at dot
                        var get_ext = file.split('.');
                        // reverse name to check extension
                        get_ext = get_ext.reverse();
                        // check file type is valid as given in 'exts' array
                        if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
                            $("[id$=btnUploadeHide]").trigger("click");
                        } else {
                            alert('Invalid file Uploading!');
                        }
                    }
                    return false;
                }
            });


            //------------Validation Enter Only Number Values--------------------------
            $(".numberValue").bind("paste", function (e) {
                return false;
            });

            $(".numberValue").bind("drop", function (e) {
                return false;
            });

            $(".numberValue").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) { return false; }
            });
        });

        //--------------Save Records---------------------------------------------------
        function SaveRecordsFun(prm, color) {
            setTimeout(function () {
                $.unblockUI({});
            }, 2000);
            $("[id$=lblMessages]").text(prm);
            $("[id$=lblMessages]").css("color", color);
        }

        //------------Validation Enter Only Number Values--------------------------
        $(".numberValue").bind("paste", function (e) {
            return false;
        });

        $(".numberValue").bind("drop", function (e) {
            return false;
        });

        $(".numberValue").keypress(function (e) {
            //if the letter is not digit then display error and don't type anything
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) { return false; }
        });


    </script>

    <style>
        .btnCss {
            font-family: Calibri;
            height: 25px;
            width: 25px;
            background-color: #428bca;
            color: white;
            font-size: medium;
            border-style: none;
        }

        .row {
            margin-left: 0px;
        }

        .HeaderCss {
            color: white;
            background-color: #428bca;
            font-weight: bold;
            min-width: 100px;
        }

        .tabHeader {
            font-size: 1.2em;
        }
    </style>

    <div class="container-fluid" style='font-family: <%= settings.AppUISettings.FontFamily %>; font-style: <%= settings.AppUISettings.FontStyle %>; font-weight: <%= settings.AppUISettings.FontStyle %>; margin-left: 3px; margin-right: 3px;'>
        <div class="row" style="text-align: center; width: 99%;">
            <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; font-family: Calibri;"></asp:Label>
            <asp:HiddenField ID="hdfParameter" runat="server" />
            <asp:HiddenField ID="hdfNonAdmin" runat="server" />
            <asp:Button ID="btnUploadeHide" runat="server" OnClick="btnUploadeHide_Click" Style="display: none;" />
        </div>
        <div class="panel panel-primary">
            <div id="Tabs" role="tabpanel">
                <!-- Nav tabs -->
                <ul class="nav nav-pills" role="tablist">
                    <li class="active tabHeader"><a href="#GeneralSetting" role="tab" data-toggle="tab">General Setting
                    </a></li>
                    <li><a href="#IconicView" class="tabHeader" role="tab" data-toggle="tab">Iconic View Setting
                    </a></li>                   
                </ul>
                <!-- Tab panes -->
                <div class="tab-content" style="padding-top: 10px">
                    <div role="tabpanel" class="tab-pane active" id="GeneralSetting">
                        <div class="row" style="width: 99%;">
                            <div class="col-lg-6" id="tdGeneralHide" runat="server">
                                <div class="panel panel-primary">
                                    <div class="panel-heading tabHeader">Machine Efficiency Color Setting </div>
                                    <table class="table table-bordered">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtGood" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control toolTip"></asp:TextBox>
                                            </td>
                                            <th>Good</th>
                                            <td>
                                                <asp:TextBox ID="txtBad" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control toolTip"></asp:TextBox>
                                            </td>
                                            <th>Bad</th>
                                            <%--<td>
                                                <asp:TextBox ID="txtCockpit2" name="border-color" runat="server" class="pick-a-color form-control"></asp:TextBox></td>
                                            <th>Cockpit Labels</th>--%>
                                        </tr>
                                        <tr>

                                            <%--<td>
                                                <asp:TextBox ID="txtCockpit1" name="border-color" runat="server" class="pick-a-color form-control"></asp:TextBox>
                                            </td>
                                            <th>Cockpit Labels</th>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtModerate" data-toggle="tooltip" name="border-color" runat="server" class="pick-a-color form-control toolTip"></asp:TextBox>
                                            </td>
                                            <th>Moderate</th>
                                            <td colspan="2">
                                                <asp:Button data-toggle="tooltip" ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-primary toolTip" OnClick="btnReset_Click" />
                                                &nbsp;&nbsp;<asp:Button data-toggle="tooltip" ID="btnSaveColor" runat="server" Text="Save" CssClass="btn btn-primary toolTip" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="panel panel-primary">
                                    <div class="panel-heading tabHeader">Company Logo </div>
                                    <br />
                                    <table class="table table-bordered">
                                        <tr>
                                            <td>
                                                <asp:FileUpload data-toggle="tooltip" CssClass="toolTip" ID="FileUpload1" runat="server" />
                                            </td>
                                            <td>
                                                <button type="button" data-toggle="tooltip" id="btnUpload" class="btn btn-sm btn-primary toolTip">Upload files</button>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <div class="col-lg-6" id="tdApplication" runat="server">
                                <div class="panel panel-primary">
                                    <div class="panel-heading tabHeader">Application Setting </div>
                                    <table class="table table-bordered">
                                        <tr id="tdAndonHide" runat="server">
                                            <td>
                                                <label class="control-label">Andon Title</label></td>
                                            <td>
                                                <asp:TextBox ID="txtAndonTitle" data-toggle="tooltip" runat="server" CssClass="form-control input-sm toolTip"> </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label">Plant To Display</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPlantToDisplay" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label">Font Family</label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlFontFamily" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label">Font Style</label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlFontStyle" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label">Data Refresh Interval (sec.)</label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlRefreshInterval" runat="server" CssClass="form-control input-sm">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label">Default Predefined Time Period</label></td>
                                         
                                            <td>
                                                <asp:DropDownList ID="ddlDefaultPredefinedTimePeriod" runat="server" CssClass="form-control input-sm">
                                                    <asp:ListItem Value="Today">Today</asp:ListItem>
                                                    <asp:ListItem Value="Yesterday">Yesterday</asp:ListItem>
                                                    <asp:ListItem Value="CurrentShift">Current Shift</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnSaveGeneralPageSetting" runat="server" Text="Save" CssClass="btn btn-primary" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="IconicView">
                        <div class="row" style="width: 99%;">
                            <div class="col-lg-6">

                                <div class="panel panel-primary">
                                    <div class="panel-heading tabHeader">Cockpit View Setting </div>
                                    <div style="overflow-x: hidden; overflow-y: scroll; height: 450px;">
                                        <asp:UpdatePanel ID="updatePanal" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdViewIconicView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Field Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValueInText" CssClass="control-label" runat="server" Text='<%#Bind("ValueInText") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfValueInInt" runat="server" Value='<%#Bind("ValueInInt") %>' />
                                                                <asp:HiddenField ID="hdfParameter" runat="server" Value='<%#Bind("Parameter") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display Name">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtValueInText2" CssClass="form-control input-sm" runat="server" Text='<%#Bind("ValueInText2") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SortOrder" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtValueInInt" runat="server" CssClass="form-control input-sm" Text='<%#Bind("ValueInInt") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Visibility">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("ValueInBool") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order by Column">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnUp" CssClass="btnCss"
                                                                    OnClick="MoveIconicGridViewRows" ToolTip="Move Up"
                                                                    CommandName="Up" runat="server" Text="&uArr;" />
                                                                <asp:Button ID="btnDown" CssClass="btnCss"
                                                                    OnClick="MoveIconicGridViewRows" ToolTip="Move Down"
                                                                    CommandName="Down" runat="server" Text="&dArr;" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#004080" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:Button ID="btnHideSave" runat="server" OnClick="btnSave_Click" Style="display: none;" />
                                                <asp:Button ID="btnHideSetting" runat="server" OnClick="btnSaveSetting_Click" Style="display: none;" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div style="margin-bottom: 5px;">
                                    <asp:Button ID="btnSaveIconicCockpitSetting" runat="server" Text="Save Cockpit Setting" CssClass="btn btn-primary" />
                                </div>

                            </div>

                            <div class="col-lg-6">
                                <div class="panel panel-primary">
                                    <div class="panel-heading tabHeader">Page Setting </div>
                                    <table class="table table-bordered">
                                        <tr>
                                            <td>
                                                <label class="control-label">Font Size (px)</label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlFontSizeMachineBox" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label">Flip Interval (sec.)</label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlMachineFlip" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </td>
                                        </tr>
                                     <%--   <tr id="hideTdEnergy" runat="server">
                                            <td>
                                                <label class="control-label">Enable Energy Dashboard Flip</label></td>
                                            <td>
                                                <asp:CheckBox ID="chkEnableDashboard" runat="server" />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                <label class="control-label">Enable Image/Video Page</label></td>
                                            <td>
                                                <asp:CheckBox ID="chkIconicEnableImgVdo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label">Show Smiley Image</label></td>
                                            <td>
                                                <asp:CheckBox ID="chkShowSmileyImg" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="imgHide" style="display: none;">
                                            <td>
                                                <label class="control-label">Smiley Image Size (px)</label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlSmileImageSize" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:Button ID="btnSaveIconicPageSetting" runat="server" Text="Save Page Setting" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
