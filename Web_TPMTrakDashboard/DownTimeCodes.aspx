<%@ Page Title="Down Code Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DownTimeCodes.aspx.cs" Inherits="Web_TPMTrakDashboard.DownTimeCodes" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        .textboxcommon {
            border: none;
            background: transparent;
            color: black;
        }

        .dropdown {
            -webkit-appearance: none;
        }


        #tblDownCodesInfo tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
            color: black;
        }

        #tblDownCodesInfo tbody tr:nth-child(even) {
            background-color: #DCDCDC;
            color: black;
        }

        .form-control {
            color: black;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 100px;
        }

        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            margin-bottom: 10px;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
        }

        #btnExport {
            background-color: #1c701c;
        }
    </style>

    <%--  <link href="MyCssAndJS/tosterJs/toastr.min.css" rel="stylesheet" />
    <script src="MyCssAndJS/tosterJs/toastr.min.js"></script>--%>
    <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>

    <div class="row text-center">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri; color: white" meta:resourcekey="lblMessagesResource1"></asp:Label>
    </div>
    <div class="row">
        <%--<table class="table table-bordered">
            <tr>
                <td style="vertical-align: middle">Down ID</td>
                <td>
                    <input id="txtDownID" type="text" class="form-control" /></td>
                <td style="vertical-align: middle">Interface ID</td>
                <td>
                    <input id="txtInterfaceID" type="text" class="form-control" />
                </td>
                <td style="vertical-align: middle">Category</td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList></td>
            </tr>
            <tr>
                <td style="vertical-align: middle">Description</td>
                <td>
                    <textarea id="txtDescription" class="form-control"> </textarea>
                </td>

                <td style="vertical-align: middle">
                    <label>
                        <span class="checkbox">
                            <input id="chkAvailBox" type="checkbox" />Availability Efficiency</span>
                    </label>
                </td>
                <td style="vertical-align: middle">Threshold (in seconds) &nbsp
                    <input id="txtThreshold" type="text" class="numbersOnly form-control" style="width: 42px; display: inline" value="0" />

                </td>
                <td style="vertical-align: middle">
                    <label>
                        <span class="checkbox">
                            <input id="chkThresholdBox" type="checkbox" />Threshold from CO</span>
                    </label>
                </td>
                <td style="vertical-align: middle">
                    <input value="Insert" type="button" class="btn btn-info btn-sm displayCss" id="btnProcess" />
                    <input value="Update" type="button" class="btn btn-info btn-sm displayCss" id="btnUpdate" />
                </td>
            </tr>
        </table>--%>
        <%--        <div class="col-lg-6">
            <fieldset class="scheduler-border">
                <legend class="scheduler-border">Down Catagory Information</legend>
                <table class="table table-bordered">
                    <tr>
                        <td style="vertical-align: middle">Down Catagory</td>
                        <td>
                            <input id="txtDownCatagory" type="text" class="form-control" /></td>
                        <td style="vertical-align: middle">Down Description</td>
                        <td>
                            <input id="txtDownDesc" type="text" class="form-control" /></td>
                        <td>
                            <input value="Accept" type="button" class="btn btn-info displayCss" id="btnAccept" />
                            <input value="Delete" type="button" class="btn btn-info displayCss" id="btnDelete" /></td>
                    </tr>
                </table>
            </fieldset>
        </div>--%>

        <div class="col-lg-8" style="text-align: left; display: block; margin-bottom: 10px;">
            <table>
                <tr>
                    <td>
                        <input type="text" autocomplete="off" id="search" data-toggle="tooltip" title="search !" placeholder="search..." class="form-control" style="width: 250px; display: inline;">
                    </td>
                    <td style="padding-left: 30px;">
                        <input value="<%=GetGlobalResourceObject("CommanResource","New") %>" type="button" class="btn btn-info btnNew" id="btnProcess" style="display: inline; margin-left: 6px" />
                        <input value="<%=GetGlobalResourceObject("CommanResource","Cancel") %>" type="button" class="btn btn-info btnRemove" id="btnCancel" style="display: none; margin-left: 6px" />
                        <input value="<%=GetGlobalResourceObject("CommanResource","Save") %>" type="button" class="btn btn-info btnSave" id="btnUpdate" style="display: inline;" />
                    </td>
                    <td style="padding-left: 30px;">
                        <asp:Button runat="server" ID="btnExport" ClientIDMode="Static" CssClass="btn btn-info export-btn" Text="Export" OnClick="btnExport_Click" />
                    </td>
                    <td style="padding-left: 30px;">
                        <input value="<%=GetGlobalResourceObject("CommanResource","DownCatagory") %>" type="button" class="btn btn-info btnDown" id="btnDownCat" style="display: inline;" />
                    </td>
                    <td style="padding-left: 30px;">
                        <input value="DownCodeLoss" runat="server" type="button" class="btn btn-info btnLoss" id="btnDownLoss" style="display: inline;" />
                    </td>
                </tr>
            </table>
        </div>

    </div>
    <div id="griddata" style="overflow-x: hidden; overflow-y: auto; min-height: 550px; width: 100%">
        <table id="tblDownCodesInfo" class="table table-bordered table-hover headerFixer" style="width: 100%">
            <thead class="blue">
                <tr>
                    <th><%=GetGlobalResourceObject("CommanResource","DownTime") %> 
                    </th>
                    <th><%=GetGlobalResourceObject("CommanResource","InterfaceID") %> 
                    </th>
                    <th><%=GetGlobalResourceObject("CommanResource","Catagory") %>
                    </th>
                    <th><%=GetGlobalResourceObject("CommanResource","Description") %>
                    </th>
                    <th><%=GetGlobalResourceObject("CommanResource","ManagementLoss") %>
                    </th>
                    <th>Operator Loss</th>
                    <th><%=GetGlobalResourceObject("CommanResource","Owner") %>
                    </th>
                    <th>Ignore For Runtime Target</th>
                    <th>Delete</th>
                </tr>
            </thead>
        </table>
    </div>

    <%--  <div class="row">
        <div class="col-lg-8" style="text-align: right; margin-bottom: 6px; margin-top: -10px; display: block;">

            <input value="New" type="button" class="btn btn-info btnNew" id="btnNew" style="display: inline; margin-left: 6px" />
            <input value="Cancel" type="button" class="btn btn-info btnRemove" id="btnRemove" style="display: none; margin-left: 6px" />
            <input value="Save" type="button" class="btn btn-info btnSave" id="btnSave" style="display: inline;" />
            <input value="Down Catagory" type="button" class="btn btn-info btnDown" id="btnDownCatInfo" style="display: inline;" />
        </div>
    </div>--%>
    <div class="modal fade" id="deleteConfirmModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Confirmation!</h4>
                </div>
                <div class="modal-body">
                    <span style="color: black">Are you sure you want to Delete this Record?</span>
                    <input type="hidden" id="hdnDeleteDownID" />
                    <input type="hidden" id="hdnDeleteInterfaceID" />
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922">
                    <button type="button" style="width: 80px;" onclick="deleteDownIDConfirm();">Yes</button>
                    <button type="button" style="width: 80px;" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
        <div class="modal-dialog  modal-dialog-centered" style="width: 450px">
            <div class="modal-content" style="border: 2px solid #0c0922; background-color: white">
                <div class="modal-header" style="background-color: #0c0922; padding: 8px">

                    <h4 class="modal-title" style="color: white;">Warning!</h4>
                </div>
                <div class="modal-body">
                    <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>

                    <span id="lblWarningMsg" style="color: black"></span>
                </div>
                <div class="modal-footer" style="padding: 5px; border-top: 1px solid #0c0922; text-align: right">
                    <button type="button" data-dismiss="modal" style="width: 80px;" class="modalBtns">OK</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            var winHeight = $(window).height();
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (640);
                console.log('min');
            } else {
                winHeight = (840);
                console.log('max');
            }

            $("#griddata").height(winHeight - 100);
            DownCodeDetails();

        });
        //-------Search Start-------------------------
        $('th').each(function (col) {
            $(this).hover(
                function () { $(this).addClass('focus'); },
                function () { $(this).removeClass('focus'); }

            );
            $(this).click(function () {
                debugger;
                var headerTxt = $(this).html().trim();
                if ($(this).is('.asc')) {
                    $(this).removeClass('asc');
                    $(this).addClass('desc');
                    sortOrder = -1;
                }
                else {
                    $(this).addClass('asc');
                    $(this).removeClass('desc');
                    sortOrder = 1;
                }

                $(this).siblings().removeClass('asc');
                $(this).siblings().removeClass('desc');
                var arrData = $('#tblDownCodesInfo').find('tbody >tr:has(td)').get();
                arrData.sort(function (a, b) {
                    debugger;
                    if (col != 1) {
                        var val1 = $(a).children('td').eq(col).text().toUpperCase();
                        var val2 = $(b).children('td').eq(col).text().toUpperCase();

                    }
                    else {

                        var val1 = $(a).children('td').eq(col).find('input[type=text]').val().toUpperCase();
                        var val2 = $(b).children('td').eq(col).find('input[type=text]').val().toUpperCase();
                    }
                    if ($.isNumeric(val1) && $.isNumeric(val2))
                        return sortOrder == 1 ? val1 - val2 : val2 - val1;
                    else
                        return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                });
                $.each(arrData, function (index, row) {
                    $('#tblDownCodesInfo tbody').append(row);
                });
            });
        });
        //---------End Search-----------------------------------


        $(document).on("click", ".btnDown", function () {
            //window.open("AcceptedPage.aspx?machineId=" + machineId + "&shiftId=" + $("[id$=ddlShift]").val() + "&date=" + $("[id$=txtYear]").val(), "MyTargetWindowName");
            PopupCenter("DownCatagoryInfo.aspx", "<%=GetLocalResourceObject("DownCategoryInformation")%>", 900, 500);
        });
        $(document).on("click", ".btnLoss", function () {
            PopupCenter("DownCodeLossInfoo.aspx", "SubLoss Master", 1400, 700);
        });

        function PopupCenter(url, title, w, h) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=yes,toolbar=no,resizable=yes,width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            //location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=no

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        $('#search').keyup(function () {
            searchTable($(this).val());
        });

        function searchTable(inputVal) {
            var table = $('#tblDownCodesInfo');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td input');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).val())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) $(row).show(); else $(row).hide();
                }
            });
        }

        jQuery('.numbersOnly').keyup(function () {
            this.value = this.value.replace(/[^0-9\.]/g, '');
        });

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }


        var strappend = true;
        //DownCodeDetails();
        var AllCategory = [];
        var AllEmployee = [];
        function DownCodeDetails() {
            var lstCatagory = [];
            var lstOwner = [];
            var ExcludeFromTartget = [];
            strappend = true;
            strappendemp = true;
            strIgnoreforRuntimeTarget = true;

            $.ajax({
                type: "POST",
                url: "DownTimeCodes.aspx/DownCodesInfo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    $("#tblDownCodesInfo tr:gt(0)").each(function () {
                        $(this).remove();
                    });
                    if (itmData.length > 0) {
                        var str = "";
                        var strEmp = "";
                        var strIgnoreforRuntime = "";
                        debugger;
                        $(itmData).each(function (index, md) {
                            $("#tblDownCodesInfo").append('<tr><td><label class="txtDownId">' + md.downid + '</label></td><td class="update"><input class="hdfInterface" type="hidden"><input type="text" class="txtinter txtupdate textboxcommon" maxlength="25" style="text-align:left;" value="' + md.interfaceid + '" onkeypress="return event.charCode >= 48 && event.charCode <= 57" ' + md.Readonly + '/></td><td class="update"><select class="catValue dropdown textboxcommon" style="width:189px"></select></td><td class="update"><input type="text" class="txtdes txtupdate textboxcommon"  style="text-align:left;" value="' + md.downdescription + '"/></td><td><label class="checkboxvale" style="margin-bottom: 0px;"><span class="checkbox" style="display:block;margin-top: 0px">Availability Efficiency&nbsp;<input id="chkBox" class="chkAvail" type="checkbox" ' + md.chkAvaileffy + '></span></label><span class="hidespan" style="display: ' + md.dispalyValue + ';"> Threshold &nbsp;<input type="text" class="txtThre form-control" maxlength="4" style="width: 80px; display: inline" value="' + md.Threshold + '">&nbsp;<label style="margin-bottom: 0px;"><span class="checkbox" style="margin-top: 0px"><input class="chkThreshold" type="checkbox" ' + md.chkThresholdfrmCO + '>&nbsp; Threshold from CO</span></label> </span></td><td><label class="checkboxvale" style="margin-bottom:0px;"><span class="checkbox" style="display:block;margin-top: 0px">Operator Efficiency&nbsp;<input id="OperatorchkBox" class="chkOperator" type="checkbox" ' + md.chkOperatorEffy + '><br /></span></label></td></td><td class="update"><input list="OwnerLst" type="text" id="Owner" name="Owner" class="txtowner txtupdate textboxcommon"></input><datalist id ="OwnerLst" class="OwnerVal"></td><td><input class="ignoreForTarget" type="checkbox" ' + md.chkIgnoreForRuntimeTarget + '></td><td><input value="Delete" type="button" class="btn btn-info" onclick="deleteDownCode(this);" style="display:' + md.display + '" /></td></tr>'); /**/
                            $('#tblDownCodesInfo .catValue option').last().remove();
                            $('#tblDownCodesInfo .ignoreForTarget option').last().remove();
                            lstCatagory.push(md.Catagory);
                            lstOwner.push(md.Owner);
                            //ExcludeFromTartget.push(md.IgnoreForRuntimeTarget)
                            AllCategory = md.ListCatagory
                            if (strappend) {
                                $(md.ListCatagory).each(function (index, att) {
                                    str += '<option value="' + att + '">' + att + '</option>'
                                    strappend = false;
                                });
                            }
                            AllEmployee = md.ListEmployeeID
                            if (strappendemp) {
                                $(md.ListEmployeeID).each(function (index, att) {
                                    strEmp += '<option value="' + att + '">' + att + '</option>'
                                    //strEmp += '<option value="' + att.EmployeeID + '">' + att.EmployeeName + '</option>'
                                    strappendemp = false;
                                });
                            }
                            //if (strIgnoreforRuntimeTarget) {
                            //    strIgnoreforRuntime += '<option value="true">True</option>'
                            //    strIgnoreforRuntime += '<option value="false">False</option>'
                            //    strIgnoreforRuntimeTarget = false;
                            //}
                            // $(this).find(".ignoreForTarget").val(md.IgnoreForRuntimeTarget);

                        });
                        var i = 0;
                        debugger;
                        $("#tblDownCodesInfo tbody tr").each(function () {
                            debugger;
                            $(this).find(".catValue").append(str);
                            $(this).find(".catValue").val(lstCatagory[i]);

                            $(this).find(".OwnerVal").append(strEmp);
                            $(this).find(".txtowner").val(lstOwner[i]);

                            //if (ExcludeFromTartget[i] == "true")
                            //    $(this).find(".ignoreForTarget").find("option")[0].prop("selected", "selected");
                            //else
                            //    $(this).find(".ignoreForTarget").find("option")[1].prop("selected", "selected");

                            //$(this).find(".ignoreForTarget").append(strIgnoreforRuntime);
                            //alert(ExcludeFromTartget[i]);
                           /* $(this).find(".ignoreForTarget").val(ExcludeFromTartget[i]);*/
                            i++;
                        });
                        if (itmData.length == 1) {
                            if (itmData[0].downid == null) {
                                $('#tblDownCodesInfo tbody tr:nth-child(1)').hide()
                            }
                        }
                    }
                    else {
                        $('#tblDownCodesInfo').append('<tr><td colspan="8" style="text-align: center;"><%=GetGlobalResourceObject("CommanResource","NoRecordFound")%></td></tr>');
                    }
                    // $('.ajax-loader').hide();
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }

        function DownCodeDetails_old() {
            var lstCatagory = [];
            var lstOwner = [];
            var ExcludeFromTartget = [];
            var strappend = true;
            var strappendemp = true;
            var strIgnoreforRuntimeTarget = true;

            $.ajax({
                type: "POST",
                url: "DownTimeCodes.aspx/DownCodesInfo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    $("#tblDownCodesInfo tr:gt(0)").remove();
                    if (itmData.length > 0) {
                        var str = "";
                        var strEmp = "";
                        var strIgnoreforRuntime = "";

                        $(itmData).each(function (index, md) {
                            $("#tblDownCodesInfo").append('<tr><td><label class="txtDownId">' + md.downid + '</label></td><td class="update"><input class="hdfInterface" type="hidden"><input type="text" class="txtinter txtupdate textboxcommon" maxlength="25" style="text-align:left;" value="' + md.interfaceid + '" onkeypress="return event.charCode >= 48 && event.charCode <= 57" ' + md.Readonly + '/></td><td class="update"><select class="catValue dropdown textboxcommon" style="width:189px"></select></td><td class="update"><input type="text" class="txtdes txtupdate textboxcommon"  style="text-align:left;" value="' + md.downdescription + '"/></td><td><label class="checkboxvale" style="margin-bottom: 0px;"><span class="checkbox" style="display:block;margin-top: 0px">Availability Efficiency&nbsp;<input id="chkBox" class="chkAvail" type="checkbox" ' + md.chkAvaileffy + '></span></label><span class="hidespan" style="display: ' + md.dispalyValue + ';"> Threshold &nbsp;<input type="text" class="txtThre form-control" maxlength="4" style="width: 80px; display: inline" value="' + md.Threshold + '">&nbsp;<label style="margin-bottom: 0px;"><span class="checkbox" style="margin-top: 0px"><input class="chkThreshold" type="checkbox" ' + md.chkThresholdfrmCO + '>&nbsp; Threshold from CO</span></label> </span></td><td><label class="checkboxvale" style="margin-bottom:0px;"><span class="checkbox" style="display:block;margin-top: 0px">Operator Efficiency&nbsp;<input id="OperatorchkBox" class="chkOperator" type="checkbox" ' + md.chkOperatorEffy + '><br /></span></label></td></td><td class="update"><input list="OwnerLst" type="text" id="Owner" name="Owner" class="txtowner txtupdate textboxcommon"></input><datalist id ="OwnerLst" class="OwnerVal"></td><td class="update"><select class="ignoreForTarget dropdown textboxcommon" style="width:189px"><option value="true">true</option><option value="false">false</option></select></td><td><input value="Delete" type="button" class="btn btn-info" onclick="deleteDownCode(this);" style="display:' + md.display + '" /></td></tr>');

                            lstCatagory.push(md.Catagory);
                            lstOwner.push(md.Owner);
                            ExcludeFromTartget.push(md.IgnoreForRuntimeTarget);

                            if (strappend) {
                                $(md.ListCatagory).each(function (index, att) {
                                    str += '<option value="' + att + '">' + att + '</option>';
                                    strappend = false;
                                });
                            }

                            if (strappendemp) {
                                $(md.ListEmployeeID).each(function (index, att) {
                                    strEmp += '<option value="' + att + '">' + att + '</option>';
                                    strappendemp = false;
                                });
                            }

                        });

                        var i = 0;
                        $("#tblDownCodesInfo tbody tr").each(function () {
                            debugger;
                            $(this).find(".catValue").append(str);
                            $(this).find(".catValue").val(lstCatagory[i]);

                            $(this).find(".OwnerVal").append(strEmp);
                            $(this).find(".txtowner").val(lstOwner[i]);

                            if (ExcludeFromTartget[i] == true)
                                $($(this).find(".ignoreForTarget").find("option")[0]).prop("selected", "selected");
                            else
                                $($(this).find(".ignoreForTarget").find("option")[1]).prop("selected", "selected");
                            i++;
                        });

                        if (itmData.length == 1 && itmData[0].downid == null) {
                            $('#tblDownCodesInfo tbody tr:nth-child(1)').hide();
                        }
                    } else {
                        $('#tblDownCodesInfo').append('<tr><td colspan="8" style="text-align: center;"><%=GetGlobalResourceObject("CommanResource","NoRecordFound")%></td></tr>');
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                }
            });
        }

        function openWarningModal(msg) {
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }

        function deleteDownCode(btn) {
            debugger;
            var downid = $(btn).closest('tr').find('.txtDownId').text();
            var interfaceid = $(btn).closest('tr').find('.txtinter').val();
            $('#hdnDeleteDownID').val(downid);
            $('#hdnDeleteInterfaceID').val(interfaceid);
            $('#deleteConfirmModal').modal('show');

        }
        function deleteDownIDConfirm() {
            $.ajax({
                async: false,
                type: "POST",
                url: "DownTimeCodes.aspx/getDownIDDeleteStatus",
                contentType: "application/json; charset=utf-8",
                data: '{downcode:"' + $('#hdnDeleteDownID').val() + '",interfaceid:"' + $('#hdnDeleteInterfaceID').val() + '"}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    $('#deleteConfirmModal').modal('hide');
                    if (itmData == "success") {
                        DownCodeDetails();
                    }
                    else {
                        openWarningModal("Down Code is in use.You are not allowed to delete this Down Code");
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "Login.aspx";
                }
            });
        }

        $(".btnRemove").click(function () {
            if ($("#tblDownCodesInfo tr:gt(0)").find(".txtDelete").length > 0) {
                if (confirm("Are you sure?")) {
                    $(".txtDelete").closest("tr").remove();
                    $(".btnRemove").css("display", "none");
                    $(".btnNew").css("display", "inline");
                }
                return false;
            }
        });
        $(".btnNew").click(function () {

            $("#tblDownCodesInfo").append('<tr><td class="update"><input type="text" class="txtDownId txtupdate txtDelete form-control" maxlength="25" style="text-align:left;"/></td><td class="update"><input class="hdfInterface" type="hidden"  /><input type="text" onkeypress="return event.charCode >= 48 && event.charCode <= 57" maxlength="25" class="txtinter txtupdate form-control" style="text-align:left;"/></td><td class="update"><select class="catValue form-control"></select></td><td class="update"><input type="text" class="txtdes txtupdate form-control" style="text-align:left;" /></td><td><label class="checkboxvale" style="margin-bottom: 0px;"><span class="checkbox" style="display:block;margin-top: 0px">Availability Efficiency&nbsp;<input id="chkBox" class="chkAvail" type="checkbox"><br /></span></label><span class="hidespan" style="display:none;"> Threshold &nbsp;<input type="text" maxlength="4" class="txtThre form-control" style="width: 80px; display: inline" value="0">&nbsp;<label style="margin-bottom: 0px;"><span class="checkbox" style="margin-top: 0px"><input class="chkThreshold" type="checkbox" >&nbsp; Threshold from CO</span></label></span></td><td><label class="checkboxvale" style="margin-bottom: 0px;"><span class="checkbox" style="display:block;margin-top: 0px">Operator Efficiency&nbsp;<input id="OperatorchkBox" class="chkOperator" type="checkbox"><br /></span></label></td><td class="update"><input list="OwnerLst" type="text" id="Owner" name="Owner" class="txtowner txtupdate textboxcommon"></input><datalist id ="OwnerLst" class="OwnerVal"></td><td class="checkbox"><label class="checkboxvale" style="margin-bottom: 0px;"><span class="checkbox" style="display:block;margin-top: 0px"><input class="ignoreForTarget" type="checkbox" ><br /></span></label></td><td></td></tr>');
            $(AllCategory).each(function (index, att) {
                // Loop Adding Data to Dropdowns
                $('#tblDownCodesInfo .catValue').last().append('<option value="'
                    + att + '">' + att + '</option>');
            });
            $(AllEmployee).each(function (index, att) {
                $('#tblDownCodesInfo .OwnerVal').last().append('<option value="'
                    + att + '">' + att + '</option>');
            })
            $('#tblDownCodesInfo .txtDownId').focus();
            $(".btnRemove").css("display", "inline");
            $(".btnNew").css("display", "none");
        });
        //$(".btnNew").click(function () {

        //    $("#tblDownCodesInfo").append('<tr><td class="update"><input type="text" class="txtDownId txtupdate txtDelete form-control" maxlength="25" style="text-align:left;"/></td><td class="update"><input class="hdfInterface" type="hidden"  /><input type="text" onkeypress="return event.charCode >= 48 && event.charCode <= 57" maxlength="25" class="txtinter txtupdate form-control" style="text-align:left;"/></td><td class="update"><select class="catValue form-control"></select></td><td class="update"><input type="text" class="txtdes txtupdate form-control" style="text-align:left;" /></td><td><label class="checkboxvale" style="margin-bottom: 0px;"><span class="checkbox" style="display:block;margin-top: 0px">Availability Efficiency&nbsp;<input id="chkBox" class="chkAvail" type="checkbox"><br /></span></label><span class="hidespan" style="display:none;"> Threshold &nbsp;<input type="text" maxlength="4" class="txtThre form-control" style="width: 80px; display: inline" value="0">&nbsp;<label style="margin-bottom: 0px;"><span class="checkbox" style="margin-top: 0px"><input class="chkThreshold" type="checkbox" >&nbsp; Threshold from CO</span></label></span></td><td><label class="checkboxvale" style="margin-bottom: 0px;"><span class="checkbox" style="display:block;margin-top: 0px">Operator Efficiency&nbsp;<input id="OperatorchkBox" class="chkOperator" type="checkbox"><br /></span></label></td></td><td class="update"><input list="OwnerLst" type="text" id="Owner" name="Owner" class="txtowner txtupdate textboxcommon"></input><datalist id ="OwnerLst" class="OwnerVal"></td><td class="update"><select class="ignoreForTarget form-control"><option value="true">true</option><option value="false">false</option></select></td><td></td></tr>');
        //    $(AllCategory).each(function (index, att) {
        //        // Loop Adding Data to Dropdowns
        //        $('#tblDownCodesInfo .catValue').last().append('<option value="'
        //            + att + '">' + att + '</option>');
        //    });
        //    $(AllEmployee).each(function (index, att) {
        //        $('#tblDownCodesInfo .OwnerVal').last().append('<option value="'
        //            + att + '">' + att + '</option>');
        //    })
        //    $('#tblDownCodesInfo .txtDownId').focus();
        //    $(".btnRemove").css("display", "inline");
        //    $(".btnNew").css("display", "none");
        //});

        $("#tblDownCodesInfo").on("keyup", ".txtThre", function () {
            debugger;
            $(this).closest('tr').find('input[type=hidden]').val("update");
            this.value = this.value.replace(/[^0-9\.]/g, '');
        });
        //$("#tblDownCodesInfo").on("keyup", ".txtProdThre", function () {
        //    debugger;
        //    $(this).closest('tr').find('input[type=hidden]').val("update");
        //    this.value = this.value.replace(/[^0-9\.]/g, '');
        //});
        $("#tblDownCodesInfo").on("click", ".ignoreForTarget", function () {
            debugger;
            $(this).closest('tr').find('input[type=hidden]').val("update");
            if ($(this).prop("checked") == true) {
                //$(this).closest('td').find('.ignoreForTarget').prop("checked", false);
            } else if ($(this).prop("checked") == false) {
                //$(this).closest('td').find('.hidespan').css("display", "none");
            }
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $("#tblDownCodesInfo tr td").find('.catValue').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.catValue').addClass("dropdown textboxcommon");
            $(this).closest('td').find('select').addClass("form-control");
        });
        $("#tblDownCodesInfo").on("click", ".chkAvail", function () {
            debugger;
            $(this).closest('tr').find('input[type=hidden]').val("update");
            if ($(this).prop("checked") == true) {
                //$(this).closest('td').find('#OperatorchkBox').prop("checked", false);
                $(this).closest('td').find('.hidespan').css("display", "block");
                //$(this).closest('td').find('.prodhidespan').css("display", "none");
                $(this).closest('td').find('.chkThreshold').prop("checked", false);
                $(this).closest('td').find('.txtThre').val(0);
                //$(this).closest('td').find('.chkThreshold').attr("disabled", false);
            } else if ($(this).prop("checked") == false) {
                $(this).closest('td').find('.hidespan').css("display", "none");
            }
            $(this).closest('td').find('.txtThre').attr("disabled", false);
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $("#tblDownCodesInfo tr td").find('.catValue').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.catValue').addClass("dropdown textboxcommon");
            $(this).closest('td').find('select').removeClass("dropdown textboxcommon");
            $(this).closest('td').find('select').addClass("form-control");
        });
        $("#tblDownCodesInfo").on("click", ".chkOperator", function () {
            debugger;
            $(this).closest('tr').find('input[type=hidden]').val("update");
            if ($(this).prop("checked") == true) {
                //$(this).closest('td').find('.prodhidespan').css("display", "block");
            } else if ($(this).prop("checked") == false) {
                //$(this).closest('td').find('.prodhidespan').css("display", "none");
            }
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $("#tblDownCodesInfo tr td").find('.catValue').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.catValue').addClass("dropdown textboxcommon");
            $(this).closest('td').find('select').removeClass("dropdown textboxcommon");
            $(this).closest('td').find('select').addClass("form-control");
        });

        $("#tblDownCodesInfo").on("click", ".chkThreshold", function () {
            debugger;
            $(this).closest('tr').find('input[type=hidden]').val("update");
            if ($(this).prop("checked") == true) {
                $(this).closest('td').find('.txtThre').attr("disabled", true);
            } else if ($(this).prop("checked") == false) {
                $(this).closest('td').find('.txtThre').attr("disabled", false);
            }
            $(this).closest('td').find('.txtThre').val(0);
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $("#tblDownCodesInfo tr td").find('.catValue').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.catValue').addClass("dropdown textboxcommon");
            $(this).closest('td').find('select').removeClass("dropdown textboxcommon");
            $(this).closest('td').find('select').addClass("form-control");
        });

        $("#tblDownCodesInfo").on("click", ".update", function () {
            debugger;
            $(this).closest('tr').find('input[type=hidden]').val("update");
            $("#tblDownCodesInfo tr td").find('.txtupdate').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.txtupdate').addClass("textboxcommon");
            $("#tblDownCodesInfo tr td").find('.catValue').removeClass("form-control");
            $("#tblDownCodesInfo tr td").find('.catValue').addClass("dropdown textboxcommon");
            //$("#tblDownCodesInfo tr td").find('.ignoreForTarget').removeClass("form-control");
            //$("#tblDownCodesInfo tr td").find('.ignoreForTarget').addClass("dropdown textboxcommon");
            $(this).closest('td').find('input').removeClass("textboxcommon");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').removeClass("dropdown textboxcommon");
            $(this).closest('td').find('select').addClass("form-control");
        });

        var countValue = 0; var i = 0;
        $("#tblDownCodesInfo").on("change", ".txtinter", function () {
            var friValue = $(this).closest('td').find('.txtinter').val();
            var index = $(this).parent().parent().index();
            var i = 0;
            $("#tblDownCodesInfo .txtinter").each(function () {
                var value = $(this).val();
                if (index != i) {
                    if (friValue == value) {
                        countValue++;
                    }
                }
                i++;
            });
            if (countValue >= 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","DuplicateInfIdChk")%>");
                $(this).closest('td').find('input').val('');
                countValue = 0;
                return false;
            }
        });
        //$("#tblDownCodesInfo").on("click", ".dropdown", function () {
        //    $(this).closest('tr').find('input[type=hidden]').val("update");
        //    $("#tblDownCodesInfo tr td").find('.catValue').removeClass("form-control");
        //    $("#tblDownCodesInfo tr td").find('.catValue').addClass("textboxcommon");
        //    $(this).closest('td').find('select').removeClass("textboxcommon");
        //    $(this).closest('td').find('select').addClass("form-control");
        //});

        //$("#tblDownCodesInfo").on("keyup", ".insert", function () {
        //    $(this).closest('tr').find('input[type=hidden]').val("update");
        //});


        //$("#chkAvailBox").change(function () {
        //    if ($("#chkAvailBox").prop("checked") == true) {
        //        $("#chkThresholdBox").prop("checked", false);
        //        $("#chkThresholdBox").attr('disabled', true);
        //        $("#txtThreshold").attr('disabled', true);
        //    } else {
        //        $("#chkThresholdBox").attr('disabled', false);
        //        $("#txtThreshold").attr('disabled', false);
        //    }
        //});

        //$("#chkThresholdBox").change(function () {
        //    if ($("#chkThresholdBox").prop("checked") == true) {
        //        $("#txtThreshold").attr('disabled', true);
        //    } else {
        //        $("#txtThreshold").attr('disabled', false);
        //    }
        //});

        //$("#btnProcess").click(function () {
        //    if ($("#txtDownID").val() == "") {
        //        alert('Please enter Down ID !');
        //        $("#txtDownID").focus();
        //        return false;
        //    }
        //    if ($("#txtInterfaceID").val() == "") {
        //        alert('Please enter Interface ID !');
        //        $("#txtInterfaceID").focus();
        //        return false;
        //    }
        //    var values = {
        //        model:
        //        {
        //            downid: $("#txtDownID").val(),
        //            interfaceid: $("#txtInterfaceID").val(),
        //            Catagory: $("[id$=ddlCategory]").val(),
        //            downdescription: $("#txtDescription").val(),
        //            Availeffy: $("#chkAvailBox").prop("checked"),
        //            Threshold: $("#txtThreshold").val(),
        //            ThresholdfrmCO: $("#chkThresholdBox").prop("checked"),
        //        }
        //    };
        //    saveAndUpdate(values, "SaveDownTimeInfo");
        //});

        $("#btnUpdate").click(function () {
            var count = 0;
            $("#tblDownCodesInfo tr:gt(0)").each(function (src, i) {
                if (($(this, i).closest("tr").find('.txtDownId').html() == "")) {
                    if (($(this, i).closest("tr").find('.txtDownId').val() == "")) {
                        count++;
                        alert('<%=GetGlobalResourceObject("CommanResource","PleaseenterDownID")%>');
                        $(this, i).closest("tr").find('.txtDownId').focus()
                        return false;
                    }
                }
                if (($(this, i).closest("tr").find('.txtinter').val() == "")) {
                    count++;
                    alert('<%=GetGlobalResourceObject("CommanResource","PlzEnterInterface")%>');
                    $(this, i).closest("tr").find('.txtinter').focus();
                    return false;
                }
                var inter = parseInt($(this, i).closest("tr").find('.txtinter').val());
                if (inter == 0) {
                    count++;
                    alert('Interface ID cannot be ZERO');
                    $(this, i).closest("tr").find('.txtinter').focus();
                    return false;
                }

            });
            if (count != 0) {
                return false;
            }
            count = 0;
            var values = {
                model:
                {
                    ListDownCode: []
                }
            };

            $("#tblDownCodesInfo tr:gt(0)").each(function () {
                if ($(this).closest("tr").find(".hdfInterface").val() == "update") {
                    count++;
                    var innermodel =
                    {
                        downid: $(this).closest("tr").find(".txtDownId").html() == "" ? $(this).closest("tr").find(".txtDownId").val() : $(this).closest("tr").find(".txtDownId").html(),
                        interfaceid: $(this).closest("tr").find(".txtinter").val(),
                        Catagory: $(this).closest("tr").find(".catValue").val(),
                        downdescription: $(this).closest("tr").find(".txtdes").val(),
                        Availeffy: $(this).closest("tr").find('.chkAvail').prop("checked"),
                        Threshold: $(this).closest("tr").find('.txtThre').val() == "" ? 0 : $(this).closest("tr").find('.txtThre').val(),
                        ThresholdfrmCO: $(this).closest("tr").find('.chkThreshold').prop("checked"),
                        prodeffy: $(this).closest("tr").find('#OperatorchkBox').prop("checked"),
                        //OperatorThreshold: $(this).closest("tr").find('.txtProdThre').val() == "" ? 0 : $(this).closest("tr").find('.txtProdThre').val(),
                        Owner: $(this).closest("tr").find(".txtowner").val(),
                        IgnoreForRuntimeTarget: $(this).closest("tr").find(".ignoreForTarget").prop("checked"),
                        //IgnoreForRuntimeTarget: $(this).closest("tr").find(".ignoreForTarget").val(),
                    };
                    values.model.ListDownCode.push(innermodel);
                }
            });
            if (count == 0) {
                alert('<%=GetLocalResourceObject("InsertAtLeastOne")%>');
                return false;
            }
            saveAndUpdate(values, "UpdateDownTimeInfo");
            // DownCodeDetails();
        });

        function saveAndUpdate(values, functionName) {
            $.ajax({
                type: "POST",
                url: "DownTimeCodes.aspx/" + functionName,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(values),
                dataType: "json",
                success: onSave,
                error: function (response) {
                    alert("Error");
                }
            });
        }

        function messageNotOk(msg) {
            Command: toastr["success"](msg, "<%=GetGlobalResourceObject("CommanResource","Save")%>")
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-center",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
        function onSave(saveResponce) {
            var result = saveResponce.d;
            DownCodeDetails();
            messageNotOk(result);
            $(".btnRemove").css('display', 'none');
            $(".btnNew").css('display', 'inline');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
