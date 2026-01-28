<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SpecialReports.aspx.cs" Inherits="Web_TPMTrakDashboard.SpecialReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	   <%--<link href="Scripts/MultiCheckBox/bootstrap-multiselect.css" rel="stylesheet" />--%>
    <%--<script src="Scripts/MultiCheckBox/bootstrap-multiselect.js"></script>--%>
    <!--Font Awesome (added because you use icons in your prepend/append)-->
    <%: Scripts.Render("~/bundles/multiselectjs") %>
    <%: Styles.Render("~/bundles/multiselectcss") %>
    <%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
        #tblfilter tr td {
            vertical-align: middle;
        }

        #tblfilter tbody tr:nth-child(odd) {
            background-color: #FFFFFF;
        }

        #tblfilter tbody tr:nth-child(even) {
            background-color: #DCDCDC;
        }

        .multiselect-container {
            height: 300px;
            overflow-x: auto;
        }

        .multiselect-selected-text {
            padding-right: 181px;
        }

        .multiselect .dropdown-toggle {
            width: 50%;
        }
    </style>
    <div class="row" style="text-align: center; color: red;">
        <asp:Label ID="lblMessages" EnableViewState="False" runat="server" Style="font-weight: bold; font-family: Calibri;" meta:resourcekey="lblMessagesResource1"></asp:Label>
        <asp:HiddenField ID="width" runat="server" />
        <asp:HiddenField ID="height" runat="server" />
    </div>
    <div class="row">
        <div class="col-md-9">
            <h1 class="text-center login-title commontd">Reports</h1>
            <div class="account-wall">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="form-signin">
                        <asp:UpdatePanel ID="upadetPanalRepor" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnGenerate" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblfilter" class="table table-bordered table-striped">
                                    <tr>
                                        <td>
                                            <b>Report Type</b> </td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="select form-control loadData" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" meta:resourcekey="ddlReportTypeResource1"/></td>
                                    </tr>
									<tr id="trShift" runat="server">
                                        <td runat="server">
                                            <b>Shift</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="select form-control">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trFromDate" runat="server">
                                        <td>
                                            <b>From Date</b> </td>
                                        <td class="input-group">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" meta:resourcekey="txtFromDateResource1" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trToDate" runat="server">
                                        <td runat="server">
                                            <b>To Date</b> </td>
                                        <td class="input-group" runat="server">
                                            <div class="input-group-addon">
                                                <i class="glyphicon glyphicon-calendar"></i>
                                            </div>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control date" placeholder="DD-MMM-YYYY" MaxLength="15" AutoCompleteType="Disabled" AutoPostBack="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trPlant" runat="server">
                                        <td runat="server">
                                            <b>Plant ID</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlPlantId" runat="server" CssClass="select form-control loadData" AutoPostBack="True" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
									 <tr id="trMachine" runat="server">
                                        <td runat="server">
                                            <b>Machine ID</b>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddlMachineID" runat="server" CssClass="select form-control loadData" AutoPostBack="True" OnSelectedIndexChanged="ddlMachineID_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" meta:resourcekey="btnGenerateResource1" />
                                        </td>
										
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Extra JavaScript/CSS added manually in "Settings" tab -->

    <%: Scripts.Render("~/bundles/datejs") %>
    <%: Styles.Render("~/bundles/datecss") %>
    <!-- Include Date Range Picker -->
    <script src="Content/Multi/locales/bootstrap-datepicker.zh-CN.min.js"></script>
    <script src="Content/Multi/locales/bootstrap-datepicker.en-IE.min.js"></script>
	 <script type="text/javascript">
	 	const _MS_PER_DAY = 1000 * 60 * 60 * 24;
		  $(document).ready(function () {
		  	$.unblockUI({});
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());

            $(".loadData").change(function () {
                $.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif"/>' });
            });
		  });

	 	function invert(date) {
	 		return date.split(/[/-]/).reverse().join("")
	 	}

	 	function compareDates(date1, date2) {
	 		return invert(date1).localeCompare(invert(date2));
	 	}

	 	function dateDiffInDays(a, b) {
	 		// Discard the time and time-zone information.
	 		const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
	 		const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
	 		return Math.floor((utc2 - utc1) / _MS_PER_DAY);
	 	}

	 	$(document).on("click", "[id$=btnGenerate]", function () {

            if ($("[id$=ddlReportType]").val() == null) {
                alert("PleaseSelectReportType");
                $("[id$=ddlReportType]").focus();
                return false;
            }
            if ($("[id$=txtFromDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromDate")%>");
                $("[id$=txtFromDate]").focus();
                return false;
            }
            if ($("[id$=txtToDate]").val() == "") {
                alert("<%=GetGlobalResourceObject("CommanResource","PleaseEnterFromTodate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            var from = $("[id$=txtFromDate]").val();
            var to = $('[id$=txtToDate]').val();

	 		// date.split("-").reverse().join("-") is used to convert date format from dd-MM-yyyy to yyyy-MM-ddd in javascript.
            if ($("[id$=ddlReportType]").val() != "HourlyReport")
            {
            	
			  var diffe = dateDiffInDays(new Date(from.split("-").reverse().join("-")), new Date(to.split("-").reverse().join("-")));
            if (diffe > 31) {
                alert("Difference between to date and from date cannot be more than 31 days.");
                return false;
            }
            var dateCom = compareDates(from, to);
            if (dateCom == 1) {
                alert("<%=GetGlobalResourceObject("CommanResource","EndDateGreaterThenStartDate")%>");
                $("[id$=txtToDate]").focus();
                return false;
            }
            }
	 		$.blockUI({ message: '<img src="/img/loadIcon/ajax-loader.gif"  />' });
	 		if ($("[id$=ddlReportType]").val() == "HourlyReport")
	 		{
	 			setTimeout(function () {
	 				$.unblockUI({});
	 			}, 10000);
	 		}
	 		else {
	 			setTimeout(function () {
	 				$.unblockUI({});
	 			}, 7000);
	 		}
            
		 });
		 var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $.unblockUI({});
            $("[id$=width]").val($(window).width());
            $("[id$=height]").val($(window).height());
            $(".date").datepicker({
                format: 'dd-mm-yyyy',
                todayHighlight: true,
                autoclose: true,
                orientation: "top",
                autocomplete: "off",
                language: '<%=GetGlobalResourceObject("CommanResource","dateLanguage")%>',
            });

        	$(".loadData").change(function () {
        		
                  $.blockUI({ message: '<img runat="server" src="~/img/loadIcon/ajax-loader.gif" />' });
            });

        });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
