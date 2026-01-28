<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JobCard.aspx.cs" Inherits="Web_TPMTrakDashboard.JobCard" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<%: Styles.Render("~/bundles/dateTimecss") %>
    <%: Scripts.Render("~/bundles/dateTimejs") %>
    <style>
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
            height: 65px;
        }


        fieldset.scheduler-border1 {
            border: 1px groove #ddd !important;
            padding: 0.1em 0.5em 1.1em !important;
            /*0 1.4em 0.5em 1.4em !important;*/
            margin: 0px 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
            /*font-size: 13px;*/
            /*color: #277AB7;*/
            /*margin-top: -17px;*/
            font-weight: bold;
            height: 330px;
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
        }

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
            text-align: left;
            /*width: 65px;*/
            height: 10px;
            /*padding: 2px;*/
        }

        .txtcolor {
            color: gray;
        }

        .headeerwidth {
            min-width: 120px;
        }
        .splitfun{

        }
        #MainContent_grdproduction td[colspan="11"], #MainContent_grd_downcode td[colspan="7"] {
            text-align: center;
        }
    </style>
    <asp:UpdatePanel ID="updatePanal1" runat="server">
        <ContentTemplate>

       <div class="row" style="margin-left: 0.5px;">
				<asp:Label ID="lblMessages" EnableViewState="false" runat="server" Style="font-weight: bold; color: white; text-align: center; text-align-last: center; font-family: Calibri;"></asp:Label>
				<div id="headeroption">
					<fieldset class="scheduler-border" id="serch" style="height: 100px; margin-bottom: 0em;">
						<legend class="scheduler-border commontd" style="font-weight: bold">Search </legend>
						<table class="table table-bordered" id="operationinfo" style="margin-top:-23px;width:auto;column-fill:auto">
							<%--  class="table table-bordered"--%>
							<thead>
							</thead>
							<tbody style="width:auto">
								<tr style="width:auto">
									<td style="width: auto">
										<asp:Label runat="server" Text="Date" ForeColor="White" Font-Bold="true" />
									</td>
									<td class="input-group" style="border: none; width: 197px;">
										<div class="input-group-addon">
											<i class="glyphicon glyphicon-calendar"></i>
										</div>
										<asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control date1" data-toggle="tooltip"
											placeholder=" Date" AutoCompleteType="Disabled"></asp:TextBox>
									</td>
									<td>
										<asp:Label runat="server" Text="Plant ID" ForeColor="White" Font-Bold="true" />
									</td>
									<td>
										<asp:DropDownList ID="ddlPlantId" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" AutoPostBack="true" OnSelectedIndexChanged="ddlPlantId_SelectedIndexChanged">
										</asp:DropDownList>
									</td>
									<td>
										<asp:Button runat="server" Text="View Details" Style="display: inline-block" class="btn btn-info" ID="btnview" OnClick="btnview_Click" Width="127"></asp:Button>
									</td>
								</tr>
								<tr>
									<td style="text-wrap: normal">
										<asp:Label runat="server" Text="Shift ID" ForeColor="White" Font-Bold="true" />
									</td>
									<td>
										<asp:DropDownList ID="ddlshift" runat="server" CssClass="form-control" data-toggle="tooltip" title="Plant ID !" AutoPostBack="true" >
										</asp:DropDownList>
									</td>
									<td>
										<asp:Label runat="server" Text="Machine ID" ForeColor="White" Font-Bold="true" />
									</td>
									<td>
										<asp:DropDownList ID="ddlmachineid" runat="server" CssClass="form-control" data-toggle="tooltip" AutoPostBack="true">
										</asp:DropDownList>
									</td>
									<td>

										<asp:Button runat="server" Text="Save" Style="display: inline-block" class="btn btn-info" ID="btnsave" OnClick="btnsave_Click"></asp:Button>
										<asp:Button runat="server" Text="Clear"  Visible="false" Style="display: inline-block" class="btn btn-info" ID="btnclear"></asp:Button>
									</td>
								</tr>
							</tbody>
						</table>
					</fieldset>
				</div>
				<div id="productiongrd" style="overflow: auto">
					<fieldset class="scheduler-border1" id="prdgrp">
						<legend class="scheduler-border commontd">Production Details </legend>
						<div style="overflow-x: auto; overflow-y: auto; height:270px" >
							<asp:HiddenField ID="hdfprodsave" runat="server" />
							<asp:GridView ID="grdproduction" runat="server" AutoGenerateColumns="false"  CssClass="headerFixer" 
								HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true" Width="100%"
								HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnRowDataBound="grdproduction_RowDataBound" >
								<Columns>
									
									<asp:TemplateField HeaderText="Part No" HeaderStyle-Width="80px" ItemStyle-Width="80px">
										<ItemTemplate>
											<asp:HiddenField ID="hdfprodupdate" runat="server" />
											<%-- <asp:HiddenField ID="hdfcomponent" runat="server" Value='<%# Eval("Component") %>' />--%>
											<asp:Label runat="server" CssClass="txtcolor" ID="PartNo"  Text='<%# Eval("ComponentID") %>'></asp:Label>
										</ItemTemplate>

									</asp:TemplateField>

									<asp:TemplateField HeaderText="Operation NO" HeaderStyle-Wrap="false">
										<ItemTemplate>
											<asp:Label runat="server" CssClass="txtcolor" ID="oprno"  Text='<%# Eval("OperationNo") %>'></asp:Label>
										</ItemTemplate>

									</asp:TemplateField>

									<asp:TemplateField HeaderText="Operator">
										<ItemTemplate>
											<asp:HiddenField ID="hdfoperator" runat="server" Value='<%# Eval("OperatorID") %>' />
											<asp:DropDownList ID="ddlOperator" CssClass="textboxcss select" runat="server"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="AcceptedParts">
										<ItemTemplate>
											<asp:Label runat="server" ID="acceptedparts" CssClass="txtcolor" Text='<%# Eval("AcceptedParts") %>'></asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Repeat Cycles"  HeaderStyle-Wrap="false">
										<ItemTemplate>
											<asp:TextBox ID="repeatcycle" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;" CssClass="textboxcss" runat="server" Text='<%# Eval("Repeat_Cycles") %>'></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Dummy Cycle" HeaderStyle-Wrap="false">
										
										<ItemTemplate>
											<asp:TextBox ID="dummycycle" CssClass="textboxcss" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;" runat="server" Text='<%# Eval("Dummy_Cycles") %>' ></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Rework Performed" HeaderStyle-Wrap="false">
										<ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtreworkperformed"  CssClass="textboxcss" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"  Text='<%# Eval("Rework_Performed") %>'></asp:TextBox>
											<%--<asp:Label ID="reworkperformed" CssClass="txtcolor" runat="server" Text='<%# Eval("Rework_Performed") %>'></asp:Label>--%>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Marked For Rework" >
										<ItemTemplate>
											<%--<asp:TextBox ID="markrework" reverkValue='<%# Eval("ID") %>' Qty='<%# Eval("AcceptedParts") %>'  CssClass="textboxcss markForReverk" runat="server" Text='<%# Eval("Marked_for_Rework") %>'></asp:TextBox>--%>
                                             <asp:Label  ID="lblmarkrework" date='<%# Eval("pDate") %>' shift='<%# Eval("Shift") %>' plant='<%# Eval("PlantID") %>' machine='<%# Eval("MachineID") %>' partNo='<%# Eval("ComponentID") %>' opnNo='<%# Eval("OperationNo") %>' operator='<%# Eval("OperatorID") %>' repeatCycle='<%# Eval("Repeat_Cycles") %>' dummyCycle='<%# Eval("Dummy_Cycles") %>' reworkperformed='<%# Eval("Rework_Performed") %>'  reverkValue='<%# Eval("ID") %>' Qty='<%# Eval("AcceptedParts") %>' CssClass="markForReverk" runat="server" Text='<%# Eval("Marked_for_Rework") %>'  MarkedForRework='<%# Eval("Marked_for_Rework") %>' RejectionValue='<%# Eval("Rejection") %>' Style="width:100%" ></asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Rejection" HeaderStyle-Width="80px">
										<ItemTemplate>
											<%--<asp:TextBox ID="rejection" rejection='<%# Eval("ID") %>' Qty='<%# Eval("AcceptedParts") %>' CssClass="textboxcss rejectionValue" runat="server" Text='<%# Eval("Rejection") %>'></asp:TextBox>--%>
                                             <asp:Label ID="lblRejection" date='<%# Eval("pDate") %>' shift='<%# Eval("Shift") %>' plant='<%# Eval("PlantID") %>' machine='<%# Eval("MachineID") %>' partNo='<%# Eval("ComponentID") %>' opnNo='<%# Eval("OperationNo") %>' operator='<%# Eval("OperatorID") %>' repeatCycle='<%# Eval("Repeat_Cycles") %>' dummyCycle='<%# Eval("Dummy_Cycles") %>' reworkperformed='<%# Eval("Rework_Performed") %>'   rejection='<%# Eval("ID") %>' Qty='<%# Eval("AcceptedParts") %>'  MarkedForRework='<%# Eval("Marked_for_Rework") %>' RejectionValue='<%# Eval("Rejection") %>' CssClass="rejectionValue" runat="server" Text='<%# Eval("Rejection") %>' Style="width:100%"></asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="WorkOrderNo">
										<ItemTemplate>
											<asp:Label runat="server"  CssClass="textboxcss" Style="float: left" ID="WorkOrderNumber" Text='<%# Eval("WorkOrderNumber") %>'></asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="ID" Visible="false">
										<ItemTemplate>
											<asp:Label ID="id" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									
								</Columns>
								
							</asp:GridView>
						</div>
						</fieldset>
				</div>

				<div id="downgrd">
					<fieldset class="scheduler-border1" id="downgrp">
						<legend class="scheduler-border commontd">DownTimes Details </legend>
						<asp:HiddenField ID="hdfdownsave" runat="server" />
						<div style="overflow-x: auto; overflow-y: auto; height:270px">
							<asp:GridView ID="grd_downcode" runat="server" AutoGenerateColumns="false" Width="100%" Style="overflow: auto"
							HeaderStyle-Font-Italic="true" EmptyDataText="No Data Available" ShowHeaderWhenEmpty="true" ShowHeader="true" CssClass="headerFixer"
							HeaderStyle-CssClass="blue" BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#F2F2F2" OnRowDataBound="grd_downcode_RowDataBound">
							<Columns>

								<asp:TemplateField HeaderText="Part No">
									<ItemTemplate>
										<asp:HiddenField ID="hdfdownupdate" runat="server" />
										<%-- <asp:HiddenField ID="hdfcomponent" runat="server" Value='<%# Eval("Component") %>' />--%>
										<asp:Label runat="server" CssClass="txtcolor" ID="PartNo" Style="min-width: 120px;" Text='<%# Eval("ComponentID") %>'></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Operation NO" HeaderStyle-Width="50">
									<ItemTemplate>
										<asp:Label runat="server" Width="50" ID="oprno" CssClass="txtcolor" Style="width: 140px;" Text='<%# Eval("OperationNo") %>'></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>

								<asp:TemplateField HeaderText="Start Time">
									<ItemTemplate>
										<asp:Label ID="starttime" Width="180" runat="server"
                                            Ids='<%# Eval("ID") %>' PlantId='<%# Eval("plantid") %>' Shift ='<%# Eval("shift") %>' Downcategory = '<%# Eval("Downcategory") %>' ML_flag='<%# Eval("ML_flag") %>'  Turnover='<%# Eval("Turnover") %>' Threshold='<%# Eval("Threshold") %>' RetPerMcHour_Flag='<%# Eval("RetPerMcHour_Flag") %>' StdSetupTime ='<%# Eval("StdSetupTime") %>' PE_Flag='<%# Eval("PE_Flag") %>' Component='<%# Eval("ComponentID") %>' Operation='<%# Eval("OperationNo") %>' DownID='<%# Eval("DownID") %>' Opeartor='<%# Eval("OperatorID") %>' ndtime='<%# Eval("EndTime") %>'  sttime='<%# Eval("StartTime") %>' CssClass="txtcolor splitfun" Text='<%# Eval("StartTime", "{0:dd/MM/yyyy HH:mm:ss}") %>'></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="End TIme">
									<ItemTemplate>
										<asp:Label ID="endTime" Width="180" runat="server" ddate='<%# Eval("ddate") %>' Ids='<%# Eval("ID") %>' PlantId='<%# Eval("plantid") %>' Shift ='<%# Eval("shift") %>' Downcategory = '<%# Eval("Downcategory") %>' ML_flag='<%# Eval("ML_flag") %>' Turnover='<%# Eval("Turnover") %>' Threshold='<%# Eval("Threshold") %>' RetPerMcHour_Flag='<%# Eval("RetPerMcHour_Flag") %>' StdSetupTime ='<%# Eval("StdSetupTime") %>' PE_Flag='<%# Eval("PE_Flag") %>' Component='<%# Eval("ComponentID") %>' Operation='<%# Eval("OperationNo") %>' DownID='<%# Eval("DownID") %>' Opeartor='<%# Eval("OperatorID") %>' sttime='<%# Eval("StartTime") %>' ndtime='<%# Eval("EndTime") %>' CssClass="txtcolor splitfun" Text='<%# Eval("EndTime", "{0:dd/MM/yyyy HH:mm:ss}") %>'></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="DownCode" HeaderStyle-Width="20">
									<ItemTemplate>
										<asp:HiddenField ID="hdfdowncode" runat="server" Value='<%# Eval("DownID") %>' />
										<%--<asp:TextBox ID="downcode" runat="server" Text='<%# Eval("DownID") %>'></asp:TextBox>--%>
										<asp:DropDownList ID="ddldownid" CssClass="textboxcss select" runat="server" Width="180"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Operator">
									<ItemTemplate>
										<asp:HiddenField ID="hdfdownoperator" runat="server" Value='<%# Eval("OperatorID") %>' />
										<asp:DropDownList ID="ddldownOperator" CssClass="textboxcss select" runat="server"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="ID" Visible="false">
									<ItemTemplate>
										<asp:Label ID="id" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdnRemarks" ClientIDMode="Static" Value='<%# Eval("Remarks") %>' />
                                        <asp:TextBox runat="server" ID="txtRemarks" CssClass="textboxcss select" Text='<%# Eval("Remarks") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action Taken">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdnActionTaken" ClientIDMode="Static" Value='<%# Eval("ActionTaken") %>' />
                                        <asp:TextBox runat="server" ID="txtActiontaken" CssClass="txtboxcss select" Text='<%# Eval("ActionTaken") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
							</Columns>
							<HeaderStyle Font-Bold="True" ForeColor="White" />
						</asp:GridView>
						</div>
					</fieldset>
				</div>

				<%--  </div>--%>
			</div>
             <asp:Button runat="server" ID="btnReloadBtn" Visible="false" OnClick="btnReloadBtn_Click"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
         var reworkChild,rejectionChild,splitChild ;
        var reworkTimer, RejectionTimer, splitTimer;
         function checkReworkChild() {
            if (reworkChild.closed) {
                clearInterval(reworkTimer);
                __doPostBack('<%= btnReloadBtn.UniqueID%>', '');
            }
        }
        function checkRejectionChild() {
            if (rejectionChild.closed) {
                clearInterval(RejectionTimer);
                __doPostBack('<%= btnReloadBtn.UniqueID%>', '');
            }
        }
        function checkSplitChild() {
            if (splitChild.closed) {
                clearInterval(splitTimer);
                __doPostBack('<%= btnReloadBtn.UniqueID%>', '');
            }
        }
        $('[id$=txtFromDate]').datetimepicker({
            format: 'DD-MMM-YYYY'
        });

        //$("[id$=btnsave]").click(function () {
        //    
        //    if ($("[id$=hdfprodsave]").val() == "Save") {
        //        $("[id$=productiongrd] tr").each(function (src, i) {
        //            
        //            alert("[id$ = MainContent_grdproduction_dummycycle_i]").val();
        //            if ($("[id$=MainContent_grdproduction_dummycycle_i]").val() == null) {
        //                alert("Please enter the AcceptedParts  Value !!");
        //                $("[id$=acceptedparts]").focus();
        //                return false;
        //            } 
        //            if ($("[id$=repeatcycle]").val() == null) {
        //                alert("Please enter the Repeat Cycle value !!");
        //                $("[id$=repeatcycle]").focus();
        //                return false;
        //            }
        //            if ($("[id$=dummycycle]").val() == null) {
        //                alert("Please enter the Dummy Cycle value !!");
        //                $("[id$=dummycycle]").focus();
        //                return false;
        //            }
        //            if ($("[id$=reworkperformed]").val() == null) {
        //                alert("Please enter the Rework Performed !!");
        //                $("[id$=reworkperformed]").focus();
        //                return false;
        //            }
        //        });
        //    }
        //});



        $("[id$=grdproduction]").on("click", "td", function () {
            debugger;
            $("[id$=hdfprodsave]").val("Save");
            $(this).closest('tr').find('input[type=hidden]').val("updated");
            $("[id$=grdproduction] tr td").find('input').removeClass("form-control");
            $("[id$=grdproduction] tr td").find('input').addClass("textboxcss");
            $("[id$=grdproduction] tr td").find('select').addClass("select");
            $("[id$=grdproduction] tr td").find('select').addClass("textboxcss");
            $("[id$=grdproduction] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");
        });

        $("[id$=grd_downcode]").on("click", "td", function () {
            debugger;
            $("[id$=hdfdownsave]").val("Save");
            $(this).closest('tr').find('input[type=hidden]').val("updated");
            $("[id$=grd_downcode] tr td").find('input').removeClass("form-control");
            $("[id$=grd_downcode] tr td").find('input').addClass("textboxcss");
            $("[id$=grd_downcode] tr td").find('select').addClass("select");
            $("[id$=grd_downcode] tr td").find('select').addClass("textboxcss");
            $("[id$=grd_downcode] tr td").find('select').removeClass("form-control");

            $(this).closest('td').find('input').removeClass("textboxcss");
            $(this).closest('td').find('input').addClass("form-control");
            $(this).closest('td').find('select').addClass("form-control");
            $(this).closest('td').find('select').removeClass("textboxcss");
            $(this).closest('td').find('select').removeClass("select");
        });

        //$("[id$=txtFromDate]").on("dp.change", function (e) {
        //    $('[id$=txtToDate]').data("DateTimePicker").minDate(e.date);
        //});

        $(document).on("click", "tr .rejectionValue", function (e) {
            debugger;
            var rectionId = $(this).attr('rejection');
            var rejqty = $(this).attr('Qty');
            var markedForReworkValue = $(this).attr('markedforrework');
            var Rejectionvalue = $(this).attr('rejectionvalue');
            var Date = $(this).attr('date');
            var shift = $(this).attr('shift');
            var plant = $(this).attr('plant');
            var machine = $(this).attr('machine');
            var partNo = $(this).attr('partNo');
            var Opn = $(this).attr('opnNo');
            var Opr = $(this).attr('operator');
            var repeatCycles = $(this).attr('repeatCycle');
            var dummycycles = $(this).attr('dummyCycle');
            var reworkperformed = $(this).attr('reworkperformed');
            PopupCenter("MarkedReowk.aspx?rectionId=" + rectionId + "&rejectionValue=" + "rejection" + "&rejectionQty=" + rejqty + "&markedForRework=" + markedForReworkValue + "&rejectionTotalValue=" + Rejectionvalue + "&Date=" + Date + "&shift=" + shift + "&PlantID=" + plant + "&machine=" + machine + "&component=" + partNo + "&opn=" + Opn + "&Opr=" + Opr + "&RepeatCycles=" + repeatCycles + "&dummyCycles=" + dummycycles + "&reworkperformed=" + reworkperformed, "", 1000, 400, "reject");
            RejectionTimer = setInterval(checkRejectionChild, 1000);
        });
        
        $(document).on("click", "tr .markForReverk", function (e) {
            debugger;
            var rectionId = $(this).attr('reverkValue');
            var rewqty = $(this).attr('qty');
            var markedForReworkValue = $(this).attr('markedforrework');
            var Rejectionvalue = $(this).attr('rejectionvalue');
            var Date = $(this).attr('date');
            var shift = $(this).attr('shift');
            var plant = $(this).attr('plant');
            var machine = $(this).attr('machine');
            var partNo = $(this).attr('partNo');
            var Opn = $(this).attr('opnNo');
            var Opr = $(this).attr('operator');
            var repeatCycles = $(this).attr('repeatCycle');
            var dummycycles = $(this).attr('dummyCycle');
            var reworkperformed = $(this).attr('reworkperformed');
            PopupCenter("MarkedReowk.aspx?rectionId=" + rectionId + "&rejectionValue=" + "rework" + "&rejectionQty=" + rewqty + "&markedForRework=" + markedForReworkValue + "&rejectionTotalValue=" + Rejectionvalue + "&Date=" + Date + "&shift=" + shift + "&PlantID=" + plant + "&machine=" + machine + "&component=" + partNo + "&opn=" + Opn + "&Opr=" + Opr + "&RepeatCycles=" + repeatCycles + "&dummyCycles=" + dummycycles + "&reworkperformed=" + reworkperformed, "", 1000, 400, "rework");
            reworkTimer = setInterval(checkReworkChild, 1000);
        });

        function toValidDate(datestring) {
            return datestring.replace(/(\d{2})(\/)(\d{2})/, "$3$2$1");
        }

        $(document).on("click", "tr .splitfun", function (e) {
            debugger
            var MachineID = $("[id$=ddlmachineid]").val();
            var Component = $(this).attr('Component');
            var Operation = $(this).attr('Operation');
            var OperatorID = $(this).attr('Opeartor');
            var DownID = $(this).attr('DownID');
            var sttime = $(this).attr('sttime'); 
            var Type = "JobCard"
            var ndtime = $(this).attr('ndtime');
            var ID = $(this).attr('Ids');
            //var ddate = $(this).attr('ddate');
            //var Downcategory = $(this).attr('Downcategory');
            //var ML_flag = $(this).attr('ML_flag');
            //var Turnover = $(this).attr('Turnover');
            //var Threshold = $(this).attr('Threshold');
            //var RetPerMcHour_Flag = $(this).attr('RetPerMcHour_Flag');
            //var StdSetupTime = $(this).attr('StdSetupTime');
            //var PE_Flag = $(this).attr('PE_Flag');
            //var PlantId = $(this).attr('PlantId');
            //var Shift = $(this).attr('shift'); 
            
            //PopupCenter("SplitFun.aspx?MachineID=" + MachineID + "&ID=" + ID + "&ComponentID=" + Component + "&Operation=" + Operation + "&Operator=" + OperatorID + "&DownID=" + DownID + "&sttime=" + sttime + "&ndtime=" + ndtime + "&Type=" + Type + "&DownCategory=" + Downcategory + "&ML_flag=" + ML_flag + "&TurnOver=" + Turnover + "&Threshold=" + Threshold + "&RetPerMcHour_Flag=" + RetPerMcHour_Flag + "&StdSetupTime=" + StdSetupTime + "&PE_Flag=" + PE_Flag + "&PlantId=" + PlantId + "&Shift=" + Shift + "&ddate=" + ddate, 1000, 400);

            PopupCenter("SplitFun.aspx?MachineID=" + MachineID + "&ID=" + ID + "&ComponentID=" + Component + "&Operation=" + Operation + "&Operator=" + OperatorID + "&DownID=" + DownID + "&sttime=" + sttime + "&ndtime=" + ndtime + "&Type=" + Type, "Split Logic", 2000, 600,"split");
                  splitTimer = setInterval(checkSplitChild, 1000); 
        });

        function PopupCenter(url, title, w, h,param) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=yes,toolbar=no,resizable=yes,width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            //location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=no
             if (param == "rework") {
                reworkChild = newWindow;
            }
            else if (param == "reject") {
                rejectionChild = newWindow;
            }
            else if (param == "split") {
                splitChild = newWindow;
            }
            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $('[id$=txtFromDate]').datetimepicker({
                format: 'DD-MMM-YYYY',
                useCurrent: true
            });


            $("[id$=grdproduction]").on("click", "td", function () {
                debugger;
                $("[id$=hdfprodsave]").val("Save");
                $(this).closest('tr').find('input[type=hidden]').val("updated");
                $("[id$=grdproduction] tr td").find('input').removeClass("form-control");
                $("[id$=grdproduction] tr td").find('input').addClass("textboxcss");
                $("[id$=grdproduction] tr td").find('select').addClass("select");
                $("[id$=grdproduction] tr td").find('select').addClass("textboxcss");
                $("[id$=grdproduction] tr td").find('select').removeClass("form-control");

                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
                $(this).closest('td').find('select').addClass("form-control");
                $(this).closest('td').find('select').removeClass("textboxcss");
                $(this).closest('td').find('select').removeClass("select");
            });

            $("[id$=grd_downcode]").on("click", "td", function () {
                debugger;
                $("[id$=hdfdownsave]").val("Save");
                $(this).closest('tr').find('input[type=hidden]').val("updated");
                $("[id$=grd_downcode] tr td").find('input').removeClass("form-control");
                $("[id$=grd_downcode] tr td").find('input').addClass("textboxcss");
                $("[id$=grd_downcode] tr td").find('select').addClass("select");
                $("[id$=grd_downcode] tr td").find('select').addClass("textboxcss");
                $("[id$=grd_downcode] tr td").find('select').removeClass("form-control");

                $(this).closest('td').find('input').removeClass("textboxcss");
                $(this).closest('td').find('input').addClass("form-control");
                $(this).closest('td').find('select').addClass("form-control");
                $(this).closest('td').find('select').removeClass("textboxcss");
                $(this).closest('td').find('select').removeClass("select");
            });


            //    $("[id$=btnsave]").click(function () {
            //        
            //        if ($("[id$=hdfprodsave]").val() == "Save") {
            //            $("[id$=productiongrd] tr:gt(0)").each(function (src, i) {

            //                if ($("[id$=MainContent_grdproduction_dummycycle_i]").val() == null) {
            //                    alert("Please enter the AcceptedParts  Value !!");
            //                    $("[id$=acceptedparts]").focus();
            //                    return false;
            //                }
            //                if ($("[id$=repeatcycle]").val() == "") {
            //                    alert("Please enter the Repeat Cycle value !!");
            //                    $("[id$=repeatcycle]").focus();
            //                    return false;
            //                }
            //                if ($("[id$=dummycycle]").val() == "") {
            //                    alert("Please enter the Dummy Cycle value !!");
            //                    $("[id$=dummycycle]").focus();
            //                    return false;
            //                }
            //                if ($("[id$=reworkperformed]").val() == null) {
            //                    alert("Please enter the Rework Performed !!");
            //                    $("[id$=reworkperformed]").focus();
            //                    return false;
            //                }
            //            });
            //        }
            //    });

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
