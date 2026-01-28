<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cell_Definitions.aspx.cs" Inherits="Web_TPMTrakDashboard.Cell_Definitions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        fieldset {
            margin: 8px;
            border: 1px solid silver;
            padding: 8px;
            border-radius: 4px;
        }

        legend {
            text-align: left;
            color: white;
            display: block;
            width: auto;
            padding: 0;
            margin-bottom: 20px;
            line-height: inherit;
            border-bottom: transparent;
        }

        .tdDiv {
            width: 110px;
            vertical-align: central;
            font-size: 18px;
        }

        .table > tbody > tr > td {
            vertical-align: middle;
            color: white;
        }


        #tblCellDefination, #tblHdr {
            border-collapse: collapse;
            width: 100%;
        }

            #tblCellDefination td, #tblCellDefination th, #tblHdr td, #tblHdr th {
                border: 0.2px solid #929292;
                padding: 8px;
                color: white;
            }

            #tblCellDefination th, #tblHdr th {
                padding-top: 12px;
                padding-bottom: 12px;
                text-align: left;
                background-color: #3176B1;
                color: white;
            }

			 #tblCellDefination, #tblHdr1 {
            border-collapse: collapse;
            width: 100%;
        }

            #tblCellDefination td, #tblCellDefination th, #tblHdr1 td, #tblHdr1 th {
                border: 0.2px solid #929292;
                padding: 8px;
                color: white;
            }

            #tblCellDefination th, #tblHdr1 th {
                padding-top: 12px;
                padding-bottom: 12px;
                text-align: left;
                background-color: #3176B1;
                color: white;
            }

        .control-wrapper {
            margin: 0 auto;
            width: 250px;
        }

        .property-section .right-side {
            padding-left: 10px;
        }

        .property-section .left-side {
            padding: 5px;
            width: 25%;
        }

        .e-input-group:not(.e-success):not(.e-warning):not(.e-error):not(.e-float-icon-left), .e-input-group.e-float-icon-left:not(.e-success):not(.e-warning):not(.e-error) .e-input-in-wrap, .e-input-group.e-control-wrapper:not(.e-success):not(.e-warning):not(.e-error):not(.e-float-icon-left), .e-input-group.e-control-wrapper.e-float-icon-left:not(.e-success):not(.e-warning):not(.e-error) .e-input-in-wrap, .e-float-input.e-float-icon-left:not(.e-success):not(.e-warning):not(.e-error) .e-input-in-wrap, .e-float-input.e-control-wrapper.e-float-icon-left:not(.e-success):not(.e-warning):not(.e-error) .e-input-in-wrap, .e-input-group .e-input-group-icon:last-child, .e-input-group.e-bigger .e-input-group-icon:last-child, .e-input-group .e-input-group-icon.e-bigger:last-child, .e-bigger .e-input-group .e-input-group-icon:last-child, .e-input-group.e-small .e-input-group-icon:last-child, .e-input-group.e-small.e-bigger .e-input-group-icon:last-child, .e-input-group.e-small .e-input-group-icon.e-bigger:last-child, .e-input-group.e-control-wrapper .e-input-group-icon:last-child, .e-input-group.e-control-wrapper.e-bigger .e-input-group-icon:last-child, .e-input-group.e-control-wrapper .e-input-group-icon.e-bigger:last-child, .e-input-group.e-control-wrapper.e-small .e-input-group-icon:last-child, .e-input-group.e-control-wrapper.e-small.e-bigger .e-input-group-icon:last-child, .e-input-group.e-control-wrapper.e-small .e-input-group-icon.e-bigger:last-child, .e-bigger .e-input-group.e-control-wrapper.e-small .e-input-group-icon:last-child, .e-bigger .e-input-group.e-small .e-input-group-icon:last-child, .e-input-group .e-clear-icon.e-clear-icon-hide, .e-input-group.e-control-wrapper .e-clear-icon.e-clear-icon-hide {
            color: white;
        }

        .e-input-group .e-clear-icon.e-clear-icon-hide, .e-input-group.e-control-wrapper .e-clear-icon.e-clear-icon-hide {
            display: normal;
            color: white;
        }
    </style>

    <div class="container">
        <div class="row text-center">
            <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; color: red; font-family: Calibri; font-size: x-large;"></asp:Label>
        </div>
		   <div class="row">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <fieldset>
                        <legend>&nbsp;Group Creation</legend>
                        <table id="tblHdr1">
                            <tr>
                                <td class="tdDiv">Line : </td>
                                <td>
                                    <asp:DropDownList ID="ddlgrouplines" runat="server" CssClass="select form-control" AutoPostBack="false" OnSelectedIndexChanged="ddlLines_SelectedIndexChanged" />
                                </td>

                                <td class="tdDiv" style="white-space:nowrap">Group ID : </td>
                                <td>
                                    <asp:Textbox ID="txtgroupid" runat="server" CssClass="select form-control" AutoPostBack="false" AutoCompleteType="Disabled"/>
                                 <%--   <ajaxToolkit:ComboBox ID="ddlGroupIds" runat="server" CssClass="select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupIds_SelectedIndexChanged"></ajaxToolkit:ComboBox>--%>
                                </td>
								<td>
									 <asp:Button runat="server" id="btnGroupSave1" class="btn btn-info btn-md" Text="Save" OnClick="btnSave1_Click"/>
								</td>
                            </tr>
                      
                        </table>
						
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <fieldset>
                        <legend>&nbsp;Group Definitions</legend>
                        <table id="tblHdr">
                            <tr>
                                <td class="tdDiv">Line : </td>
                                <td>
                                    <asp:DropDownList ID="ddlLines" runat="server" CssClass="select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLines_SelectedIndexChanged" />
                                </td>

                                <td class="tdDiv">Group ID : </td>
                                <td>
                                    <asp:DropDownList ID="ddlGroupIds" runat="server" CssClass="select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupIds_SelectedIndexChanged"/>
                                 <%--   <ajaxToolkit:ComboBox ID="ddlGroupIds" runat="server" CssClass="select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupIds_SelectedIndexChanged"></ajaxToolkit:ComboBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdDiv">Group Order :</td>
                                <td class="tdDiv">
                                    <asp:TextBox ID="txtGroupOrder" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)" />
                                </td>
                                <td class="tdDiv">End Of Line :</td>
                                <td class="tdDiv">
                                    <asp:DropDownList ID="ddlEndOfLine" runat="server" CssClass="select form-control" />
                                </td>
                            </tr>
                        </table>
                        <div class="row text-center" style="margin-top:5px;">
                           <%--<asp:Button runat="server" ID="btngrpdelete" class="btn btn-info btn-md" Text="Delete"  />--%>

                            <asp:Button runat="server" ID="btngrpdelete" class="btn btn-info btn-md" Text="Delete" OnClick="btngrpdelete_Click" />
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="row">
            <fieldset>
                <legend>&nbsp;Assign / Unassign Groups</legend>
                <div id="TableContainer" style="overflow-x: auto; overflow-y: auto; width: 100%; max-height: 550px;">
                    <table id="tblCellDefination">
                        <thead class="blue">
                            <tr>
                                <th></th>
                                <th>Machine ID </th>
                                <th>End Of Group</th>
                                <th>Sort Order </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <br />
                <div class="row text-center">
                    <%--<asp:Button ID="btnDelete" class="btn btn-info btn-md" Text="Delete" runat="server" />--%>
                    <input type="button" id="btnSave" class="btn btn-info btn-md" value="Save" />
                </div>
            </fieldset>
        </div>

         <div class="modal fade" id="myConfirmationModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog modal-dialog-centered" style="width: 450px">
                <div class="modal-content" style="border: 2px solid #1c1a47">
                    <%--<div class="modal-header" style="background-color: white; padding: 8px">
                        <h4 class="modal-title" style="color:#1c1a47;">Confirmation?</h4>
                    </div>--%>
                    <div class="modal-body">
                        <span id="confirmationmessageText" style="font-size: 17px;">Are you sure, you want to delete this group?</span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #1c1a47">
                        <input type="button" value="Yes" class="btn btn-info" id="saveConfirmYes" onserverclick="saveConfirmYes_ServerClick" runat="server" style="background-color:#1ea5d6; color: white" data-dismiss="modal" />
                        <input type="button" value="No" class="btn btn-info" id="saveConfirmNo" onclick="saveConfirmNo()" style="background-color: #1ea5d6; color: white" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>

    </div>


    <script src="Scripts/Syncfusion/ej2.min.js"></script>
    <link href="Scripts/Syncfusion/material.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            //drp();
            GetCellData();
        });

        //var comboBoxObj = [];
        //function drp() {
        //    
        //    ej.base.enableRipple(true);

        //    comboBoxObj = new ej.dropdowns.ComboBox({

        //        popupHeight: '230px',

        //        change: function () { valueChange(); }
        //    });
        //    comboBoxObj.appendTo('#MainContent_ddlGroupIds');

        //    function valueChange() {
        //        BindGroupSortOrder();
        //        GetCellData();
        //    }
        //}

        function GetCellData() {
            
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            try {
                $.ajax({
                    type: "POST",
                    url: "Cell_Definitions.aspx/GetCellDefinations",
                    contentType: "application/json; charset=utf-8",
                    data: '{Line:"' + $("[id$=ddlLines]").val() + '",Group:"' + $("[id$=ddlGroupIds]").val() + '"}',
                    //data: '{Line:"' + $("[id$=ddlLines]").val() + '",Group:"' + comboBoxObj.value + '"}',
                    dataType: "json",
                    success: function (response) {
                        console.log(response.d);
                        BindTable(response);
                        $.unblockUI({});
                    },
                    error: function (response) {
                        alert(JSON.stringify(response)); $.unblockUI({});
                    },
                })
            } catch (e) {
                $.unblockUI({});
            }
            finally {

            }
        }

        function BindTable(data) {
            $("[id*=tblCellDefination] tr:gt(0)").each(function () {
                $(this).remove();
            });

            if (data.d.length > 0) {
                var tableContain = "";
                $(data.d).each(function (index, md) {
                    
                    tableContain += ('<tr><td><input type="checkbox" class="chkAssignMachined" ' + (md.chkAssignMachine == true ? "checked" : "unchecked") + ' /></td><td class="machineID">' + md.MachineId + '</td><td><input type="radio" class="rdbendOfGroup" name="endOfGroup" ' + (md.EndOfGroupMachine == true ? "checked" : "unchecked") + ' /></td><td><input type="text" class="txtSortOrder" value=' + md.MachineSequence + '  onkeypress="return isNumberKey(event)" ondrop="return false;" onpaste="return false;"/></td></tr>');
                });
                $("[id*=tblCellDefination]").append(tableContain);
            }
            else {
                $("[id*=tblCellDefination]").append('<tr><td colspan="9" style="text-align: center;">No record found for given search criteria !!</td></tr>');
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        $("[id*=btnSave]").click(function () {
              $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            try {
                if ($("[id$=ddlGroupIds]").val() == "") {
                    //$("[id*=ddlGroupIds]").val()

                    $.unblockUI({});
                    alert("Please select Group ID");
                    $("[id$=ddlGroupIds]").focus();
                    return;
                }

                if ($("[id$=txtGroupOrder]").val() == "") {
                    $.unblockUI({});
                    alert("Please insert Group Order");
                    $("[id$=txtGroupOrder]").focus();
                    return;
                }
                Save();
            }
            catch (e) {
                $.unblockUI({});
            }
            finally {

            }
        })
        //btngrpdelete*******************************
        //$("[id*=btngrpdelete]").click(function () {

        //      $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        //    try {
        //        $.ajax({
        //            type: "POST",
        //            url: "Cell_Definitions.aspx/DeleteGroupID1",
        //            contentType: "application/json; charset=utf-8",
        //            data: '{Line:"' + $("[id$=ddlLines]").val() + '",Group:"' + $("[id$=ddlGroupIds]").val() + '"}',
        //            dataType: "json",
                    
        //            success: function () {
        //                // location.reload();
        //                BindGroupSortOrder();
        //                GetCellData();
        //                alert('Record details deleted successfully.');
        //                document.getElementById('txtgroupid').value = "";
        //            },

        //            error: function (response) {
        //                $.unblockUI({});
        //                alert("Error");
        //            }
        //        });
        //    }
        //    catch (e) {
        //        $.unblockUI({});
        //    }
        //    finally {

        //    }
        //})


        //$("[id*=btnDelete]").click(function () {

        //      $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        //    try {
        //        $.ajax({
        //            type: "POST",
        //            url: "Cell_Definitions.aspx/DeleteCellDefination",
        //            contentType: "application/json; charset=utf-8",
        //            data: '{Line:"' + $("[id$=ddlLines]").val() + '",Group:"' + $("[id$=ddlGroupIds]").val() + '"}',
        //            dataType: "json",
        //            success: function () {
        //                // location.reload();
        //                BindGroupSortOrder();
        //                GetCellData();
        //                alert('Plant details deleted successfully.');
        //            },

        //            error: function (response) {
        //                $.unblockUI({});
        //                alert("Error");
        //            }
        //        });
        //    }
        //    catch (e) {
        //        $.unblockUI({});
        //    }
        //    finally {

        //    }
        //})

        function Save() {
            var values = {
                model:
				{
				    endOfMachineVal: "",
				    selectedLine: "",
				    selectedGroupId: "",
				    ListCellDefination: []
				}
            };

            $("#tblCellDefination tr:gt(0)").each(function () {
                {
                    var innermodel =
				   {
				       chkAssignMachine: $(this).closest("tr").find(".chkAssignMachined").prop("checked") == true ? true : false,
				       MachineId: $(this).closest("tr").find(".machineID").html(),
				       EndOfGroupMachine: $(this).closest("tr").find(".rdbendOfGroup").prop("checked") == true ? 1 : 0,
				       MachineSequence: parseInt($(this).closest("tr").find(".txtSortOrder").val()),
				   };
                    values.model.ListCellDefination.push(innermodel);
                }
            });
            values.model.endOfMachineVal = $("[id$=ddlEndOfLine]").val();;
            values.model.selectedLine = $("[id$=ddlLines]").val();
            values.model.selectedGroupId = $("[id$=ddlGroupIds]").val();//;  $("[id*=ddlGroupIds]").val()
            values.model.groupOrder = $("[id$=txtGroupOrder]").val();

            if (!values.model.ListCellDefination.some(e => e.EndOfGroupMachine === 1)) {
                alert("Please select End of Group machine..");
                $.unblockUI({});
                return;
            }

            SaveToDb(values);
        }

        function SaveToDb(values) {
            $.ajax({
                type: "POST",
                url: "Cell_Definitions.aspx/SaveCellDefinations",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(values),
                dataType: "json",
                success: onSave,
                error: function (response) {
                    $.unblockUI({});
                    alert("Error");
                }
            });
        }

        function onSave(saveResponce) {
            // location.reload();
            BindGroupSortOrder();
            GetCellData();
            $("[id$=lblMessages]").text(saveResponce.d);
            $("[id$=lblMessages]").css("color", "white");
        }


        $("[id*=ddlGroupIds]").on('change', function () {
            
            BindGroupSortOrder();
            GetCellData();
        });

        function BindGroupSortOrder() {
            $.ajax({
                type: "POST",
                url: "Cell_Definitions.aspx/GetBindSortOrder",
                contentType: "application/json; charset=utf-8",
                data: '{Line:"' + $("[id$=ddlLines]").val() + '",Group:"' + $("[id$=ddlGroupIds]").val() + '"}',
                dataType: "json",
                success: function (data) {
                    
                    $("[id*=txtGroupOrder]").val(data.d);
                },
                error: function (response) {
                    $.unblockUI({});
                    alert("Error");
                }
            });
        }

        //$("[id*=ddlLines]").on('change', function () {
        //    BindGroupSortOrder();
        //    GetCellData();
        //});

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $(document).ready(function () {
                ///drp();
                GetCellData();
            });

            //var comboBoxObj = [];
            //function drp() {
            //    
            //    ej.base.enableRipple(true);

            //    comboBoxObj = new ej.dropdowns.ComboBox({

            //        popupHeight: '230px',

            //        change: function () { valueChange(); }
            //    });
            //    comboBoxObj.appendTo('#MainContent_ddlGroupIds');

            //    function valueChange() {
            //        BindGroupSortOrder();
            //        GetCellData();
            //    }
            //}

            function GetCellData() {
                
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
                try {
                    $.ajax({
                        type: "POST",
                        url: "Cell_Definitions.aspx/GetCellDefinations",
                        contentType: "application/json; charset=utf-8",
                        data: '{Line:"' + $("[id$=ddlLines]").val() + '",Group:"' + $("[id$=ddlGroupIds]").val() + '"}',
                        //data: '{Line:"' + $("[id$=ddlLines]").val() + '",Group:"' + comboBoxObj.value + '"}',
                        dataType: "json",
                        success: function (response) {
                            console.log(response.d);
                            BindTable(response);
                            $.unblockUI({});
                        },
                        error: function (response) {
                            alert(JSON.stringify(response)); $.unblockUI({});
                        },
                    })
                } catch (e) {
                    $.unblockUI({});
                }
                finally {

                }
            }

            function BindTable(data) {
                $("[id*=tblCellDefination] tr:gt(0)").each(function () {
                    $(this).remove();
                });

                if (data.d.length > 0) {

                    var tableContain = "";
                    $(data.d).each(function (index, md) {
                        
                        tableContain += ('<tr><td><input type="checkbox" class="chkAssignMachined" ' + (md.chkAssignMachine == true ? "checked" : "unchecked") + ' /></td><td class="machineID">' + md.MachineId + '</td><td><input type="radio" class="rdbendOfGroup" name="endOfGroup" ' + (md.EndOfGroupMachine == true ? "checked" : "unchecked") + ' /></td><td><input type="text" class="txtSortOrder" value=' + md.MachineSequence + '  onkeypress="return isNumberKey(event)" ondrop="return false;" onpaste="return false;"/></td></tr>');
                    });
                    $("[id*=tblCellDefination]").append(tableContain);
                }
                else {
                    $("[id*=tblCellDefination]").append('<tr><td colspan="9" style="text-align: center;">No record found for given search criteria !!</td></tr>');
                }
            }

            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            }

            $("[id*=ddlLines]").on('change', function () {
                BindGroupSortOrder();
                GetCellData();
            });
            $("[id*=ddlGroupIds]").on('change', function () {
                BindGroupSortOrder();
                GetCellData();
            });
            //drp();
        });
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
