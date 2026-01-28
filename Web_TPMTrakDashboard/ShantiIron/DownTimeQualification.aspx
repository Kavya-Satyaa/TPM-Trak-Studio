<%@ Page Title="Down Modification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DownTimeQualification.aspx.cs" Inherits="Web_TPMTrakDashboard.ShantiIron.DownTimeQualification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #downdatagrid tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #downdatagrid tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .blue {
            background-color: #2E6886 !important;
            cursor: pointer;
            height: 38px;
        }

            .blue th {
                color: white !important;
                cursor: pointer;
                position: sticky;
                top: -1px;
                background-color: #2E6886 !important;
                z-index: 20;
                text-align:center;
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

        .select {
            /*border-radius: 5px;*/
            -webkit-appearance: none;
        }

        #tblheader tbody > tr > td {
            padding-top: 3px;
            height: 10px;
            padding-bottom: 0px;
        }

        .table thead > tr > th, .table tbody > tr > th, .table tfoot > tr > th, .table thead > tr > td, .table tbody > tr > td, .table tfoot > tr > td {
            padding: 1px;
        }

        td {
            text-align: center;
            /*width: 65px;*/
            height: 10px;
            /*padding: 2px;*/
        }

        .txtcolor {
            color: white;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            /*margin: -16px 0 1.5em 0 !important;*/
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            height: 136px;
        }



        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: -14px 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            /*font-weight: bold;*/
            /*height: 80px;*/
        }

        legend.scheduler-border {
            font-size: 1.1em !important;
            /*font-weight: bold !important;*/
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            /*color: #277AB7;*/
            border-bottom: none;
            margin-top: -4px;
            color: white;
            margin-bottom:5px;
        }
        
        /*/--------------Boostrap css--------------/*/
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }

        /*td{
            color:white;
        }*/
        .nav-pills > li {
            display: inline-block;
            float: none;
        }
         fieldset.scheduler-border1 table tr td{
             padding:3px;
         }
    </style>
    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdnScrollPos" ClientIDMode="Static" />
            <div class="row">
                <div style="width: auto; column-fill: auto" id="filterDiv">
                    <fieldset class="scheduler-border1" id="viewbox" style="padding-bottom: 6px !important">
                        <legend class="scheduler-border">View Data</legend>
                        <table class="table table-bordered" id="tblheader" style="margin-top: -24px; column-width: auto; width: auto; color: white; margin-bottom: 0px;margin:3px">
                            <tbody>
                                <tr>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">Plant ID</div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlplant" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">Cell ID</div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlCell" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCell_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">Machine </div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlmachine" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 70px;" class="txtcolor">From DateTime</div>
                                    </td>
                                    <td class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" data-toggle="tooltip"
                                            title="From Date !" placeholder="From Date" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">To DateTime</div>
                                    </td>
                                    <td class="input-group">
                                        <div class="input-group-addon">
                                            <i class="glyphicon glyphicon-calendar"></i>
                                        </div>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date2" data-toggle="tooltip"
                                            title="To Date !" placeholder="To Date" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                    
                                   
                                </tr>
                                <tr>
                                     <td>
                                        <div style="margin-top: 5px; min-width: 60px;" class="txtcolor">Prefined Day </div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlDayShift" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDayShift_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </td>
                                     <td colspan="2">
                                        <div style="min-width: 115px;margin:3px">
                                            <asp:Button runat="server" Text="View" Style="float: none" class="btn btn-info" ID="btnview" OnClick="btnview_Click" OnClientClick="ShowLoader()"></asp:Button>&nbsp;
									
                                            <asp:Button runat="server" Text="Save" Style="float: none" class="btn btn-info" ID="btnSave" OnClick="btnSave_Click" OnClientClick="ShowLoader()"></asp:Button>&nbsp;
                                       
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </fieldset>
                </div>
                <div id="updateDiv">
                     <fieldset class="scheduler-border1" id="updatebox" style="padding-bottom: 6px !important;width:60%">
                        <legend class="scheduler-border">Update Down Data</legend>
                    <table class="table table-bordered" style="width: 90%;margin:3px">
                        <tr>
                            <td>
                                <div style="margin-top: 5px;" class="txtcolor">From</div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlfrmdown" Style="min-width: 100px;" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <div style="margin-top: 5px;" class="txtcolor">To</div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldown_to" CssClass="form-control" runat="server" style="width:auto" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" Text="Update" class="btn btn-info" ID="downupdate" OnClick="downupdate_Click" OnClientClick="return getDownUpdateConfirmation()"></asp:Button>&nbsp;
                            </td>
                        </tr>
                    </table>
                         </fieldset>
                </div>
                <div id="divgrd" style="height: 65vh; overflow: auto; margin-top: 5px;">
                    <asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; color: white; text-align: center; font-family: Calibri;"></asp:Label>
                    <asp:GridView ID="downdatagrid" runat="server" AutoGenerateColumns="false" Width="100%" HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available." ShowHeaderWhenEmpty="true" ShowHeader="true" HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnRowDataBound="downdatagrid_RowDataBound">
                        <Columns>
                             <asp:TemplateField HeaderText="Machine ID" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMachineID" CssClass="splitfun" runat="server" Text='<%# Eval("MachineID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Down Code" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfdownsavecheck" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnAutoDataID" runat="server" Value='<%# Eval("AutodataID") %>' />
                                    <asp:HiddenField runat="server" ID="hdnMachine" Value='<%# Eval("MachineID") %>' />
                                    <%-- <asp:HiddenField runat="server" ID="hdnDownCode"   Value='<%# Eval("DownCode") %>' />--%>
                                    <asp:HiddenField runat="server" ID="hdnDownCode" Value='<%# Eval("DownInterfaceID") %>' />
                                    <asp:DropDownList ID="ddlDownCode" runat="server" CssClass="textboxcss select"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sart Time" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblDownFromTime" CssClass="splitfun" runat="server" Text='<%# Eval("StartTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Time" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblDownToTime" CssClass="splitfun" runat="server" Text='<%# Eval("EndTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Bold="True" ForeColor="White" />
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="warningModal" role="dialog" style="min-width: 300px;">
            <div class="modal-dialog  modal-dialog-centered" style="width: 600px">
                <div class="modal-content" style="border: 2px solid #5D7B9D;background-color:white">
                    <div class="modal-header" style="background-color: #5D7B9D; padding: 8px">

                        <h4 class="modal-title" style="color: white; font-size: 25px">Warning!</h4>
                    </div>
                    <div class="modal-body" style="padding: 20px 15px">
                     <%--   <img src="Images/warnig.png" width="60" />&nbsp;&nbsp;&nbsp;--%>
				
					<span id="lblWarningMsg" style="font-size: 23px;"></span>
                    </div>
                    <div class="modal-footer" style="padding: 5px; border-top: 1px solid #5D7B9D; text-align: right">
                        <button type="button" data-dismiss="modal" Style="width: 80px;font-size:18px" class="modalBtns">OK</button>
                    </div>
                </div>
            </div>
        </div>
    <script>
        $(document).ready(function () {
            setGridHeight();
        });
        function getDownUpdateConfirmation() {
            return window.confirm("Do you really want to update Down Code from " + $("[id$=ddlfrmdown]").val() + " to " + $("[id$=ddldown_to]").val() + " ?");
        }
        function setGridHeight() {
            var wHeight = $(window).height() - $('#updateDiv').height() - $('#filterDiv').height()- 120;
            $('#divgrd').css('height', wHeight);
        }
        function openWarningModal(msg) {
            debugger;
            $('#lblWarningMsg').text(msg);
            $('[id*=warningModal]').modal('show');
        }
        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MMM-YYYY HH:mm',
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $('[id$=txtToDate]').datetimepicker({
            format: 'DD-MMM-YYYY HH:mm',
            useCurrent: false,
            locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
        });
        $("[id$=txtFromDate]").on("dp.change", function (e) {
            $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
        });
        $("[id$=txtToDate]").on("dp.change", function (e) {
            $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
        });
       
        //............add remove csss..............
        $("[id$=downdatagrid]").on("click", "td", function () {
            debugger;
            //$(this).closest('tr').find('input[type=hidden]').val("updated");
            $(this).closest('tr').find('#hdfdownsavecheck').val("updated");
            $("[id$=downdatagrid] tr td").find('input').removeClass("form-control");
            $("[id$=downdatagrid] tr td").find('input').addClass("textboxcss");
            $("[id$=downdatagrid] tr td").find('select').addClass("select");
            $("[id$=downdatagrid] tr td").find('select').addClass("textboxcss");
            $("[id$=downdatagrid] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");
        });

      
        function ShowLoader() {
            $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
        }

        function HideLoader() {
            $.unblockUI({});
        }

        //.......end add remove css.................


        //because of updatepannel
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
          
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MMM-YYYY HH:mm',
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $('[id$=txtToDate]').datetimepicker({
                format: 'DD-MMM-YYYY HH:mm',
                useCurrent: false,
                locale: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>'
            });
            $("[id$=txtFromDate]").on("dp.change", function (e) {
                $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
            });
            $("[id$=txtToDate]").on("dp.change", function (e) {
                $('[id$=txtFromDate]').data("DateTimePicker").maxDate(e.date);
            });

            $(document).ready(function () {
                setGridHeight();
            });
            //add remove css
            $("[id$=downdatagrid]").on("click", "td", function () {
                debugger;
                $(this).closest('tr').find('#hdfdownsavecheck').val("updated");
                $("[id$=downdatagrid] tr td").find('input').removeClass("form-control");
                $("[id$=downdatagrid] tr td").find('input').addClass("textboxcss");
                $("[id$=downdatagrid] tr td").find('select').addClass("select");
                $("[id$=downdatagrid] tr td").find('select').addClass("textboxcss");
                $("[id$=downdatagrid] tr td").find('select').removeClass("form-control");

                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
                $(this).closest('td').find('select').addClass("form-control");
                $(this).closest('td').find('select').removeClass("textboxcss");
                $(this).closest('td').find('select').removeClass("select");
            });

            //end add remve css

            function ShowLoader() {
                $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            }

            function HideLoader() {
                $.unblockUI({});
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
