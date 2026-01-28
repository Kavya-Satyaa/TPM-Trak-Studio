<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RejectionCodes.aspx.cs" Inherits="Web_TPMTrakDashboard.RejectionCodes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Scripts/jquery-editable-select.css" rel="stylesheet" />
    <script src="Scripts/jquery-editable-select.js"></script>
        <%: Scripts.Render("~/bundles/toastrJs") %>
    <%: Styles.Render("~/bundles/toastrCss") %>
    <style>
        /*/*#rejectiongrid tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #rejectiongrid tr:nth-child(even) {
            background-color: #DCDCDC;
        }*/

        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
            height: 38px;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
            }

        .textboxcss {
            border: none;
            background-color: transparent;
            font-style: italic;
            color: black;
        }

        .addtextcss {
            border: initial;
            background-color: none;
        }

        .rowheight {
            height: 46px;
        }

        footerheight {
            height: 38px;
        }

        .removefootercss {
            display: none;
        }

        .iconcss {
            color: red;
            text-align: center;
        }

        /*.select {
            border-radius: 5px;
            -webkit-appearance: none;
        }*/

        /*#tblRejection tr:hover {
            background-color: #202648;
        }*/

        #tblRejection .RejDelete, #tblRework .RejDelete {
            color: red;
            cursor: pointer;
        }

        /*#tblRejection .RejDelete:hover {
                color: red;
                cursor: pointer;
            }*/
        #tblRejection tbody tr, #tblRework tbody tr {
            background-color: white;
        }
    </style>

    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <div class="row text-center" style="width: 70%">
                <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; color: white; font-family: Calibri;"></asp:Label>
            </div>
            <div class="row" style="width: 70%; margin-left: 10px;">

                <table class="table table-bordered" id="tblsearch">
                    <tr>
                        <td>
                            <div style="margin-top: 5px;" class="commontd">
                                <b>
                                    <asp:Label ID="lblreason" runat="server" Text="<%$Resources:CommanResource,RejectionReason %>"></asp:Label></b>
                            </div>
                        </td>
                        <td>
                            <div class="col-md-12">
                                <asp:TextBox ID="txtreason" runat="server" CssClass=" form-control"></asp:TextBox>
                            </div>
                        </td>
                        <%--  <td>
                            <div style="margin-top: 5px;" class="commontd">
                                <asp:Label ID="lblinterface" runat="server" Text="Interface ID"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtinterface" runat="server" CssClass=" form-control"></asp:TextBox>
                        </td>--%>
                        <%-- <td>
                            <div>
                                <asp:Button runat="server" Text="Search" class="btn btn-info" ID="btnsearch"></asp:Button>
                            </div>
                        </td>--%>

                        <td>
                            <asp:DropDownList ID="ddloption" runat="server" CssClass="form-control">
                                <asp:ListItem Value="Rejection">Rejection Reason</asp:ListItem>
                                <asp:ListItem Value="Rework">Rework Reason</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="text-center">
                            <input type="button" value="<%=GetGlobalResourceObject("CommanResource","New") %>" class="btn btn-info" id="btnnew" />
                            <input type="button" value="<%=GetGlobalResourceObject("CommanResource","Cancel") %>" class="btn btn-info" id="btncance" style="display: none" />
                            <%--  <asp:Button runat="server" Text="New" class="btn btn-info" ID="btnnew" OnClick="btnnew_Click" OnClick="btnsave_Click"></asp:Button>--%>
                            <asp:Button runat="server" Text="<%$Resources:CommanResource, Save %>" class="btn btn-info" ID="btnsave"></asp:Button>
                            <asp:Button runat="server" Text="<%$Resources:CommanResource,Delete %>	" Visible="false" class="btn btn-info" ID="btndelete"></asp:Button>
                        </td>
                        <td class="text-center">
                            <asp:Button runat="server" Text="Export" class="btn btn-info" Style="background-color: #1c701c;" ID="btnExport" OnClick="btnExport_Click"></asp:Button>
                        </td>
                    </tr>
                </table>


                <div id="grddiv" style="height: 100vh; overflow: auto;">
                    <asp:HiddenField ID="hdfsavecheck" runat="server" />
                    <table id="tblRejection" class="table table-bordered headerFixer">
                        <thead class="blue">
                            <tr>
                                <th><%=GetGlobalResourceObject("CommanResource","RejectionReason") %></th>
                                <th><%=GetGlobalResourceObject("CommanResource","InterfaceID") %></th>
                                <th><%=GetGlobalResourceObject("CommanResource","RejectionCategory") %> </th>
                                <th id="subCatHeader">SubCategory </th>
                                <th><%=GetGlobalResourceObject("CommanResource","RejectionDescription") %></th>
                                <th><%=GetGlobalResourceObject("CommanResource","Action") %></th>
                            </tr>
                        </thead>
                    </table>
                </div>

                <div id="divrework" style="display: none">
                    <asp:HiddenField ID="hdfrework" runat="server" />
                    <table id="tblRework" class="table table-bordered">
                        <thead class="blue">
                            <tr>
                                <th><%=GetGlobalResourceObject("CommanResource","ReworkReason") %></th>
                                <th><%=GetGlobalResourceObject("CommanResource","ReworkInterfaceID") %></th>
                                <th><%=GetGlobalResourceObject("CommanResource","ReworkCategory") %></th>
                                <th><%=GetGlobalResourceObject("CommanResource","ReworkDescription") %></th>
                                <th><%=GetGlobalResourceObject("CommanResource","Action") %></th>
                            </tr>
                        </thead>
                    </table>

                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script>

        //-------Search Start-------------------------

        //---------End Search-----------------------------------

        //..........search rework......

        $(document).ready(function () {
            $('[id$=txtreason]').keyup(function () {
                if ($('[id$=ddloption]').val() == "Rework") {
                    searchTable($(this).val());
                } if ($('[id$=ddloption]').val() == "Rejection") {
                    searchRejectionTable($(this).val());
                }
            });

            function searchTable(inputVal) {
                var table = $('#tblRework');
                table.find('tr').each(function (index, row) {
                    var allCells = $(row).find('td');
                    if (allCells.length > 0) {
                        var found = false;
                        allCells.each(function (index, td) {
                            var regExp = new RegExp(inputVal, 'i');
                            if (regExp.test($(td).html())) {
                                found = true;
                                return false;
                            }
                        });
                        if (found == true) $(row).show(); else $(row).hide();
                    }
                });
            }

            function searchRejectionTable(inputVal) {
                var table = $('#tblRejection');
                table.find('tr').each(function (index, row) {
                    var allCells = $(row).find('td');
                    if (allCells.length > 0) {
                        var found = false;
                        allCells.each(function (index, td) {
                            var regExp = new RegExp(inputVal, 'i');
                            if (regExp.test($(td).html())) {
                                found = true;
                                return false;
                            }
                        });
                        if (found == true) $(row).show(); else $(row).hide();
                    }
                });
            }
        })

        $('th').each(function (col) {            $(this).hover(                function () { $(this).addClass('focus'); },                function () { $(this).removeClass('focus'); }            );            $(this).click(function () {                var headerTxt = $(this).html().trim();                if ($(this).is('.asc')) {                    $(this).removeClass('asc');                    $(this).addClass('desc');                    sortOrder = -1;                }                else {                    $(this).addClass('asc');                    $(this).removeClass('desc');                    sortOrder = 1;                }                $(this).siblings().removeClass('asc');                $(this).siblings().removeClass('desc');                var arrData = $('table').find('tbody >tr:has(td)').get();                arrData.sort(function (a, b) {                    if (col != 1) {                        var val1 = $(a).children('td').eq(col).text().toUpperCase();                        var val2 = $(b).children('td').eq(col).text().toUpperCase();                    }                    else {                        var val1 = $(a).children('td').eq(col).find('input[type=text]').val().toUpperCase();                        var val2 = $(b).children('td').eq(col).find('input[type=text]').val().toUpperCase();                    }                    if ($.isNumeric(val1) && $.isNumeric(val2))                        return sortOrder == 1 ? val1 - val2 : val2 - val1;                    else                        return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;                });                $.each(arrData, function (index, row) {                    $('tbody').append(row);                });            });        });


        //....end search.............

        function hieghtManage() {
            var winHeight = $(window).height();
            console.log(winHeight);
            if (winHeight < 650) {
                winHeight = (630);
                console.log('min');
            } else {
                winHeight = (830);
                console.log('max');
            }
            if ($('[id$=ddloption]').val() == "Rejection Reason") {
                $("#grddiv").height(winHeight);
                $("#grddiv").attr('style', 'overflow-x: hidden; overflow-y: auto; min-height: 300px;max-height: ' + winHeight - 100 + 'px; width: 100%');
            }
            else if ($('[id$=ddloption]').val() == "Rework Reason") {
                $("#divrework").height(winHeight);
                $("#divrework").attr('style', 'overflow:auto; overflow-x: hidden; min-height: 300px;max-height: ' + winHeight - 100 + 'px; width: 100%');
            }
        }

        var lstCategory;
        BindRejectionTable();
        function BindRejectionTable() {
            $.ajax({
                async: false,
                type: "POST",
                url: "RejectionCodes.aspx/RejectionInfo",
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    $("#tblRejection tr:gt(0)").each(function () {
                        $(this).remove();
                    });
                    var str = "";
                    debugger;
                    if (itmData.length > 0) {
                        $(itmData).each(function (index, md) {
                            var string = "";
                            $(md.ListCategory).each(function (index, att) {
                                if (att == md.Catagory) {
                                    string = string + '<option value="' + att + '" selected>' + att + '</option>';
                                }
                                else {
                                    string = string + '<option value="' + att + '">' + att + '</option>';
                                }
                            });
                            str = str + '<tr><td><input class="hdfDelete" value="0" type="hidden"><label class="txtRejection">' + md.Rejectionid + ' </label></td><td><input class="hdfInterface" type="hidden"><input type="text" class="txtInterface textboxcommon form-control txtRejInter" style="text-align:left;" maxlength="7" onkeypress="return event.charCode >= 48 && event.charCode <= 57" value="' + md.Interfaceid + '"</td>';

                            if (isGEAPagesEnabled()) {
                                str += '<td class="categoryTD"><select class="catValue txtupdate textboxcommon form-control editableselect" onblur="bindSubCategory(this)">' + string + '</select></td>';
                                var subCatStr = "";
                                $(md.SubCatgoryList).each(function (index, att) {
                                    if (att == md.SubCatagory) {
                                        subCatStr += '<option value="' + att + '" selected>' + att + '</option>';
                                    }
                                    else {
                                        subCatStr += '<option value="' + att + '">' + att + '</option>';
                                    }
                                });
                                str += '<td class="subCatTD"><select id="ddlSubCategory" class="subCatValue txtupdate textboxcommon form-control editableselect">' + subCatStr + '</select></td >';
                            }
                            else {
                                $('#subCatHeader').css("display", "none");
                                str += '<td><select class="catValue txtupdate textboxcommon form-control editableselect">' + string + '</select></td>';
                            }

                            str += '<td><input type="text" class="txtDesc txtupdate textboxcommon form-control" style="text-align:left;" value="' + md.Description + '"/></td><td style="vertical-align: middle;text-align: center;"><i deleteId="' + md.Rejectionid + '" class="glyphicon glyphicon-trash RejDelete"></i></td></tr>';
                        });
                        $("#tblRejection").append(str);
                        $(".editableselect").editableSelect({ filter: false });
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }
        function bindSubCategory(ddl) {
            debugger;
            var subCatStr = "";
            var itmData = getSubCagoryDetails($(ddl).val());
            $(ddl).closest('tr').find('#ddlSubCategory').val("");
            var subCatDdl = $(ddl).closest('tr').find('.subCatTD ul');
            $(subCatDdl).empty();
            if (itmData != undefined) {
                for (var i = 0; i < itmData.length; i++) {
                    if (i == 0) {
                        subCatStr += '<li value="' + itmData[i] + '" class="es-visible" selected="selected">' + itmData[i] + '</li>';
                        $(ddl).closest('tr').find('#ddlSubCategory').val(itmData[i]);
                    }
                    else {
                        subCatStr += '<li value="' + itmData[i] + '" class="es-visible">' + itmData[i] + '</li>';
                    }
                }
            }
            $(subCatDdl).append(subCatStr);
        }
        function getSubCagoryDetails(category) {
            debugger;
            var itmData;
            $.ajax({
                async: false,
                type: "POST",
                url: "RejectionCodes.aspx/getSubCategoryListForCat",
                contentType: "application/json; charset=utf-8",
                data: '{category:"' + category + '"}',
                dataType: "json",
                success: function (response) {
                    itmData = response.d;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
            return itmData;
        }
        var selectedCategory = "";
        function getCagoryDetails() {
            var catStr = "";
            $.ajax({
                async: false,
                type: "POST",
                url: "RejectionCodes.aspx/getCategoryList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    debugger;
                    if (itmData != undefined) {
                        for (var i = 0; i < itmData.length; i++) {
                            if (i == 0) {
                                catStr += '<option value="' + itmData[i] + '" selected>' + itmData[i] + '</option>';
                                selectedCategory = itmData[i];
                            }
                            else {
                                catStr += '<option value="' + itmData[i] + '">' + itmData[i] + '</option>';
                            }
                        }
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
            return catStr;
        }
        function isGEAPagesEnabled() {
            var isPageEnabled = false;
            debugger;
            $.ajax({
                async: false,
                type: "POST",
                url: "RejectionCodes.aspx/isGEAPageEnabled",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    debugger;
                    isPageEnabled = itmData;
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
            return isPageEnabled;
        }
        function BindReworkTable() {
            $.ajax({
                type: "POST",
                url: "RejectionCodes.aspx/ReworkInformation",
                contentType: "application/json; charset=utf-8",
                data: '{}',
                dataType: "json",
                success: function (response) {
                    var itmData = response.d;
                    $("#tblRework tr:gt(0)").each(function () {
                        $(this).remove();
                    });
                    var strb = "";
                    if (itmData.length > 0) {
                        $(itmData).each(function (index, md) {
                            var stringb = "";
                            $(md.ListCategory).each(function (index, att) {
                                if (att == md.Catagory) {
                                    stringb = stringb + '<option value="' + att + '" selected>' + att + '</option>';
                                }
                                else {
                                    stringb = stringb + '<option value="' + att + '">' + att + '</option>';
                                }
                            });
                            strb = strb + '<tr><td><input class="hdfDelete form-control" value="0" type="hidden"><label class="txtRework">' + md.Reworkid + '</label></td><td><input class="hdfInterface" type="hidden"><input type="text" class="txtInterface textboxcommon form-control" style="text-align:left;" maxlength="7" onkeypress="return event.charCode >= 48 && event.charCode <= 57" value="' + md.Interfaceid + '"</td><td><select class="catValue txtupdate txtRewInter textboxcommon form-control editableselectRework">' + stringb + '</select></td><td><input type="text" class="txtDesc txtupdate textboxcommon form-control" style="text-align:left;" value="' + md.Description + '"/></td><td style="vertical-align: middle;text-align: center;"><i deleteId="' + md.Reworkid + '" class="glyphicon glyphicon-trash RejDelete"></i></td></tr>'
                        });
                        $("#tblRework").append(strb);
                        $(".editableselectRework").editableSelect({ filter: false });
                    }
                },
                error: function (jqXHR, textStatus, err) {
                    alert('Error: ' + err);
                    if (jqXHR.status == 401)
                        window.location.href = "SignIn.aspx";
                }
            });
        }

        $("#btnnew").click(function () {
            if ($("[id$=ddloption]").val() == "Rejection") {
                AddRejection();
                $("#btnnew").css('display', 'none');
                $("#btncance").css('display', "");
                $("[id$=hdfsavecheck]").val("Save")
            } else {
                $("#btnnew").css('display', 'none');
                $("#btncance").css('display', "");
                $("[id$=hdfrework]").val("Save");
                AddRework();
            }
            return false;
        });

        $("#tblRejection").on("click", ".RejDelete", function () {
            if (confirm("Are you sure you want to delete this Rejection reason ?")) {
                var value = $(this).attr('deleteId');
                if (value != undefined) {
                    DeleteRejectionData(value)
                } else {
                    $(this).closest("tr").remove();
                }
                $("#btnnew").css('display', '');
                $("#btncance").css('display', "none");
            } return false;
        });

        $("#tblRework").on("click", ".RejDelete", function () {
            if (confirm("Are you sure you want to delete this Rejection reason ?")) {
                var value = $(this).attr('deleteId');
                if (value != undefined) {
                    DeleteReworkData(value)
                } else {
                    $(this).closest("tr").remove();
                }
                $("#btnnew").css('display', '');
                $("#btncance").css('display', "none");
            } return false;
        });

        function AddRejection() {
            var val = lstCategory;
            var appendStr = '<tr><td><input class="hdfDelete" value="1" type="hidden"><input type="text" class="txtRejection textboxcommon form-control" style="text-align:left;"/></td><td><input class="hdfInterface" type="hidden"><input type="text" class="txtInterface txtRejInter textboxcommon form-control" style="text-align:left;" maxlength="7" onkeypress="return event.charCode >= 48 && event.charCode <= 57" /></td>';
            var categoryList = getCagoryDetails();

            if (isGEAPagesEnabled()) {
                debugger;
                appendStr += '<td class="categoryTD"><select class="catValue txtupdate textboxcommon form-control editableselect" onblur="bindSubCategory(this)">' + categoryList + '</select></td>';
                var subCategoryList = getSubCagoryDetails(selectedCategory);
                selectedCategory = "";
                var subCatStr = "";
                if (subCategoryList != undefined) {
                    for (var i = 0; i < subCategoryList.length; i++) {
                        if (i == 0) {
                            subCatStr += '<option value="' + subCategoryList[i] + '" selected>' + subCategoryList[i] + '</option>';
                        }
                        else {
                            subCatStr += '<option value="' + subCategoryList[i] + '">' + subCategoryList[i] + '</option>';
                        }
                    }
                }
                /* appendStr += '<td><input type="text" class="subCatValue txtSubCat textboxcommon form-control"></input></td>';*/
                appendStr += '<td class="subCatTD"><select id="ddlSubCategory" class="subCatValue txtSubCat textboxcommon form-control editableselect">' + categoryList + '</select></td>';
            }
            else {
                appendStr += '<td class="categoryTD"><select class="catValue  textboxcommon form-control editableselect" >' + categoryList + '</select></td>';
            }
            appendStr += '<td><input type="text" class="txtDesc textboxcommon form-control" style="text-align:left;" /></td></td><td style="vertical-align: middle;text-align: center;"><i OnClientClick="return confirm("Are you sure you want to delete this Rejection reason ?")" class="glyphicon glyphicon-trash RejDelete"></i></td></tr>';
            $("#tblRejection").prepend(appendStr);
            $(".editableselect").editableSelect({ filter: false });
            //$('#tblRejection .catValue option').last().remove();
            //$(lstCategory).each(function (index, att) {
            //    // Loop Adding Data to Dropdowns
            //    $('#tblRejection .catValue').last().append('<option value="'
            //                                 + att + '">' + att + '</option>');

            //});
            //$('table#tblRejection tr:last td:first input').focus();
            //$('table#tblRejection tr:last td:first input').addClass("form-control");
            return false;
        }

        function AddRework() {
            var val = lstCategory;
            $("#tblRework").append('<tr><td><input class="hdfDelete" value="1" type="hidden"><input type="text" class="txtRework textboxcommon form-control" style="text-align:left;"/></td><td><input class="hdfInterface" type="hidden"><input type="text" class="txtInterface txtRewInter textboxcommon form-control" style="text-align:left;" maxlength="7" onkeypress="return event.charCode >= 48 && event.charCode <= 57"/></td><td><input type="text" class="catValue textboxcommon form-control"></input></td><td><input type="text" class="txtDesc textboxcommon form-control" style="text-align:left;" /></td></td><td style="vertical-align: middle;text-align: center;"><i OnClientClick="return confirm("Are you sure you want to delete this Rejection reason ?")" class="glyphicon glyphicon-trash RejDelete"></i></td></tr>');
            return false;
        }

        $("#btncance").click(function () {
            if ($('[id$=ddloption]').val() == "Rejection") {
                $("#btnnew").css('display', "");
                $("#btncance").css('display', 'none');
                $("[id$=hdfsavecheck]").val("");

                $("#tblRejection tr:gt(0)").each(function () {
                    if ($(this).find(".hdfDelete").val() == "1")
                        $(this).remove();
                });
                return false;
            }
            else if ($('[id$=ddloption]').val() == "Rework") {
                $("#btnnew").css('display', "");
                $("#btncance").css('display', 'none');
                $("[id$=hdfrework]").val("");

                $("#tblRework tr:gt(0)").each(function () {
                    if ($(this).find(".hdfDelete").val() == "1")
                        $(this).remove();
                });
                return false;
            }
        });

        $('[id$=ddloption]').change(function () {
            if ($('[id$=ddloption]').val() == "Rejection") {
                $("#grddiv").css('display', "");
                $("#divrework").css('display', 'none');
                $("[id$=hdfsavecheck]").val("");
                $("#MainContent_lblreason").html("<%=GetGlobalResourceObject("CommanResource","RejectionReason")%>");
                BindRejectionTable();
            }
            else if ($('[id$=ddloption]').val() == "Rework") {
                $("#divrework").css('display', "");
                $("#grddiv").css('display', 'none');
                $("[id$=hdfrework]").val("");
                $("#MainContent_lblreason").html("<%=GetGlobalResourceObject("CommanResource","ReworkReason")%>");
                BindReworkTable();
            }
            $("#btnnew").css('display', "");
            $("#btncance").css('display', 'none');
            return false;
        });

        $("#tblRejection").on("change", "td", function () {
            $(this).closest('tr').find('.hdfInterface').val("update");
            //$("#tblcomponentinfo tr td").find('.txtupdate').removeClass("form-control");
            //$("#tblcomponentinfo tr td").find('.txtupdate').addClass("select");
            //$("#tblcomponentinfo tr td").find('.txtupdate').addClass("textboxcommon");
            //$(this).closest('td').find('input').removeClass("textboxcommon");
            //$(this).closest('td').find('input').addClass("form-control");


            //$(this).closest('td').find('select').removeClass("textboxcommon");
            //$(this).closest('td').find('select').removeClass("select");
            //$(this).closest('td').find('select').addClass("form-control");

        });

        $("#tblRejection").on("keyup", ".txtInterface", function () {
            this.value = this.value.replace(/[^0-9\.]/g, '');
        });

        $("#tblRework").on("keyup", ".txtInterface", function () {
            this.value = this.value.replace(/[^0-9\.]/g, '');
        });

        $("#tblRework").on("change", "td", function () {
            $(this).closest('tr').find('.hdfInterface').val("update");
        });

        $("[id$=btnsave]").click(function () {
            if ($('[id$=ddloption]').val() == "Rejection") {
                SaveRejectionData();
                $("#btnnew").css('display', "");
                $("#btncance").css('display', 'none');
                $("[id$=hdfsavecheck]").val("");

                $("#tblRejection tr:gt(0)").each(function () {
                    if ($(this).find(".hdfDelete").val() == "1")
                        $(this).remove();
                });
            }
            else if ($('[id$=ddloption]').val() == "Rework") {
                SaveReworkData();
                $("#btnnew").css('display', "");
                $("#btncance").css('display', 'none');
                $("[id$=hdfrework]").val("");
                $("#tblRework tr:gt(0)").each(function () {
                    if ($(this).find(".hdfDelete").val() == "1")
                        $(this).remove();
                });
            }
            return false;
        });

        function DeleteRejectionData(values) {
            deleteRejectionRework(values, "DeleteRejectionInfo");
        }

        function DeleteReworkData(values) {
            deleteRejectionRework(values, "DeleteReworkInfo");
        }

        function deleteRejectionRework(values, functionName) {
            $.ajax({
                type: "POST",
                url: "RejectionCodes.aspx/" + functionName,
                contentType: "application/json; charset=utf-8",
                data: '{deleteId:"' + values + '"}',
                dataType: "json",
                success: onSave,
                error: function (response) {
                    alert("Error");
                }
            });
        }

        function SaveRejectionData() {
            var count = 0;
            debugger;
            var isGEAEnabled = false;
            if (isGEAPagesEnabled()) {
                isGEAEnabled = true;
            }
            $("#tblRejection tr:gt(0)").each(function (src, i) {
                if ($(this).closest("tr").find(".hdfInterface").val() == "update" || $(this).closest("tr").find(".hdfDelete").val() == "1") {
                    if ($(this).closest("tr").find(".hdfDelete").val() == "1") {
                        if (($(this, i).closest("tr").find('.txtRejection').val() == "")) {
                            alert('Please enter Rejection !!');
                            count++;
                            $(this, i).closest("tr").find('.txtRejection').focus();
                            return false;
                        }
                    }
                    if (($(this, i).closest("tr").find('.txtInterface').val() == "")) {
                        alert('Please enter Interface ID !!');
                        count++;
                        $(this, i).closest("tr").find('.txtInterface').focus();
                        return false;
                    }
                    debugger;
                    var inter = parseInt($(this, i).closest("tr").find('.txtInterface').val());
                    if (inter == 0) {
                        alert('Interface ID cannot be ZERO !!');
                        count++;
                        $(this, i).closest("tr").find('.txtInterface').focus();
                        return false;
                    }
                    if (($(this, i).closest("tr").find('.catValue').val() == "")) {
                        alert('Please enter Category !!');
                        count++;
                        $(this, i).closest("tr").find('.catValue').focus();
                        return false;
                    }
                    if (isGEAEnabled) {
                        if (($(this, i).closest("tr").find('.subCatValue').val() == "")) {
                            alert('Please enter Sub Category !!');
                            count++;
                            $(this, i).closest("tr").find('.subCatValue').focus();
                            return false;
                        }
                    }
                    if (count != 0) {
                        alert('Please enter Interface ID !!');
                        return false;
                    }
                }
            });
            var values = {
                model:
                {
                    ListRejectionRework: []
                }
            };

            $("#tblRejection tr:gt(0)").each(function () {
                if ($(this).closest("tr").find(".hdfInterface").val() == "update") {
                    debugger;
                    let subcat = "";
                    if (isGEAEnabled) {
                        subcat = $(this).closest("tr").find(".subCatValue").val();
                    }
                    var innermodel =
                    {
                        Rejectionid: $(this).closest("tr").find(".txtRejection").text() == "" ? $(this).closest("tr").find(".txtRejection").val() : $(this).closest("tr").find(".txtRejection").text(),
                        Interfaceid: $(this).closest("tr").find(".txtInterface").val(),
                        Catagory: $(this).closest("tr").find(".catValue").val(),
                        Description: $(this).closest("tr").find(".txtDesc").val(),
                        SubCatagory: subcat,
                    };
                    values.model.ListRejectionRework.push(innermodel);
                }
            });
            if (count == 0) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                saveAndUpdateRejection(values, "SaveUpdateRejectionInfo");
            }
        }

        function SaveReworkData() {
            var count = 0;
            $("#tblRework tr:gt(0)").each(function (src, i) {
                if ($(this).closest("tr").find(".hdfInterface").val() == "update" || $(this).closest("tr").find(".hdfDelete").val() == "1") {
                    if ($(this).closest("tr").find(".hdfDelete").val() == "1") {
                        if (($(this, i).closest("tr").find('.txtRework').val() == "")) {
                            alert('Please enter Rework !!');
                            count++;
                            $(this, i).closest("tr").find('.txtRework').focus();
                            return false;
                        }
                    }
                    if (($(this, i).closest("tr").find('.txtInterface').val() == "")) {
                        alert('Please enter Interface ID !!');
                        count++;
                        $(this, i).closest("tr").find('.txtInterface').focus();
                        return false;
                    }
                    var inter = parseInt($(this, i).closest("tr").find('.txtInterface').val());
                    if (inter == 0) {
                        alert('Interface ID cannot be ZERO !!');
                        count++;
                        $(this, i).closest("tr").find('.txtCat').focus();
                        return false;
                    }
                    if (($(this, i).closest("tr").find('.txtCat').val() == "")) {
                        alert('Please enter Category !!');
                        count++;
                        $(this, i).closest("tr").find('.txtCat').focus();
                        return false;
                    }

                    if (count != 0) {
                        alert('Please enter Interface ID !!');
                        return false;
                    }
                }
            });
            var values = {
                model:
                {
                    ListRejectionRework: []
                }
            };
            $("#tblRework tr:gt(0)").each(function () {
                if ($(this).closest("tr").find(".hdfInterface").val() == "update") {
                    var innermodel =
                    {
                        Reworkid: $(this).closest("tr").find(".txtRework").text() == "" ? $(this).closest("tr").find(".txtRework").val() : $(this).closest("tr").find(".txtRework").text(),
                        Interfaceid: $(this).closest("tr").find(".txtInterface").val(),
                        Catagory: $(this).closest("tr").find(".catValue").val(),
                        Description: $(this).closest("tr").find(".txtDesc").val(),
                    };
                    values.model.ListRejectionRework.push(innermodel);
                }
            });
            if (count == 0) {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                saveAndUpdateRejection(values, "SaveUpdateReworkInfo");
            }
        }

        function saveAndUpdateRejection(values, functionName) {
            $.ajax({
                async: false,
                type: "POST",
                url: "RejectionCodes.aspx/" + functionName,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(values),
                dataType: "json",
                success: onSave,
                error: function (response) {
                    alert("Error");
                }
            });
        }

        function onSave(saveResponce) {
            var result = saveResponce.d;
            $("[id$=lblMessages]").text(result);
            $("[id$=lblMessages]").css("color", "white");
            if ($('[id$=ddloption]').val() == "Rejection") {
                BindRejectionTable();
            }
            else if ($('[id$=ddloption]').val() == "Rework") {
                BindReworkTable();
            }
            $.unblockUI({});
        }

        var countValue = 0;
        $("#tblRework").on("change", ".txtRewInter", function () {
            countValue = 0;
            var friValue = $(this).closest('td').find('.txtRewInter').val();
            $("#tblRework .txtRewInter").each(function () {
                var value = $(this).val();
                if (friValue == value) {
                    countValue++;
                }
            });
            if (countValue > 1) {
                alert("Please do not enter dublicate Interface ID");
                $(this).closest('td').find('input').val('');
                countValue = 0;
                return false;
            }
        });

        $("#tblRejection").on("change", "tr .txtRejInter", function () {

            $(this).closest('tr').find('.hdfInterface').val("update");
            countValue = 0;
            var friValue = $(this).closest('td').find('.txtRejInter').val();
            $("#tblRejection .txtRejInter").each(function () {
                var value = $(this).val();
                if (friValue == value) {
                    countValue++;
                }
            });
            if (countValue > 1) {
                alert("Please do not enter dublicate Interface ID");
                $(this).closest('td').find('input').val('');
                countValue = 0;
                return false;
            }
        });

        $("#tblRejection").on("select.editable-select", "tr .editableselect", function () {
            $(this).closest('tr').find('.hdfInterface').val("update");
        });

        $("#tblRework").on("select.editable-select", "tr .editableselectRework", function () {
            $(this).closest('tr').find('.hdfInterface').val("update");
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
